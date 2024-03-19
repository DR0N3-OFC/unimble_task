using Microsoft.AspNetCore.Mvc;
using TODOBack.Data;
using TODOBack.Models;
using Gerencianet.NETCore.SDK;
using Newtonsoft.Json.Linq;
using Microsoft.CSharp.RuntimeBinder;

namespace TODOBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : Controller
    {
        [HttpGet]
        [Route("/Billings/")]
        public IActionResult Get([FromServices] AppDbContext context)
        {
            return Ok(context.Billings!.ToList());
        }

        [HttpGet("/Billings/{txid:guid}")]
        public IActionResult GetByID([FromRoute] string txid, [FromServices] AppDbContext context)
        {
            var billingModel = context.Billings!.SingleOrDefault(e => e.TxID == txid);

            if (billingModel == null)
            {
                return NotFound();
            }

            return Ok(billingModel);
        }

        [HttpGet("/BillingPixCode/{txid:guid}")]
        public IActionResult GetBillingPixCode([FromRoute] string txid, [FromServices] AppDbContext context)
        {
            try
            {
                var charge = new PixCharge();

                var chargeDetails = charge.Execute(txid);

                var result = (string)JObject.Parse(chargeDetails)["pixCopiaECola"];

                if (result != null)
                {
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return BadRequest();
        }

        [HttpGet("/BillingQrCode/{txid:guid}")]
        public IActionResult GetBillingQRCode([FromRoute] string txid, [FromServices] AppDbContext context)
        {
            try 
            {
                var charge = new PixCharge();

                var chargeDetails = charge.Execute(txid);
                
                var locationId = (int)JObject.Parse(chargeDetails)["loc"]["id"];
                
                var result = charge.GetQRCode(locationId);
                
                if (result != null)
                {
                    var qrCode = (string)JObject.Parse(result)["imagemQrcode"];
                    
                    return Ok(qrCode);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return BadRequest();
        }

        [HttpGet("/BillingByUser/{id:int}")]
        public IActionResult GetByUser([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var existingBilling = context.Billings!.FirstOrDefault(e => e.UserID == id);

            if (existingBilling != null)
            {
                var charge = new PixCharge();
                var result = charge.Execute(existingBilling.TxID!);

                if (existingBilling.Deadline < DateTime.Now && (string)JObject.Parse(result)["status"] == "ATIVA")
                {
                    context.Billings!.Remove(existingBilling);

                    context.SaveChanges();

                    return BadRequest("O pedido expirou");
                }
                else if ((string)JObject.Parse(result)["status"] == "CONCLUIDA")
                {
                    var user = context.Users!.FirstOrDefault(e => e.UserId == existingBilling.UserID);
                    user!.IsPremium = true;

                    context.Users!.Update(user);

                    context.SaveChanges();
                    
                    return Ok((string)JObject.Parse(result)["status"]);
                }
                
                return Ok(existingBilling);
            }

            return NotFound();
        }

        [HttpPost("/Billings/")]
        public IActionResult Post([FromBody] BillingModel billingModel, [FromServices] AppDbContext context)
        {
            var existingBilling = context.Billings!.FirstOrDefault(e => e.UserID == billingModel.UserID || e.CPF == billingModel.CPF);

            var charge = new PixCharge();

            if (existingBilling == null)
            {
                try
                {
                    var result = charge.Execute(billingModel.CPF!, billingModel.FullName!);

                    DateTime date; 
                    DateTime.TryParseExact((string)JObject.Parse(result)["calendario"]["criacao"], "MM/dd/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out date);
                    DateTime localDate = date.ToLocalTime();

                    billingModel.TxID = (string)JObject.Parse(result)["txid"];
                    billingModel.Deadline = localDate.AddSeconds((int)JObject.Parse(result)["calendario"]["expiracao"]);
                    billingModel.Value = (float)JObject.Parse(result)["valor"]["original"];

                    context.Billings!.Add(billingModel);

                    context.SaveChanges();

                    return Ok(result);
                }
                catch (RuntimeBinderException)
                {
                    return BadRequest("Informe um CPF real.");
                }
            }
            else
            {
                if (existingBilling.UserID == billingModel.UserID && existingBilling.CPF == billingModel.CPF)
                {
                    var result = charge.Execute(existingBilling.TxID!);

                    return Ok(result);
                }
                else
                {
                    return BadRequest("O CPF informado já está sendo utilizado.");
                }
            }
        }

        [HttpPut("/EditBilling/{id:alpha}")]
        public IActionResult Put([FromRoute] string id, [FromBody] BillingModel billingModel, [FromServices] AppDbContext context)
        {
            if (id == billingModel.TxID)
            {
                context.Billings!.Update(billingModel);
                context.SaveChanges();

                return Ok(billingModel);
            }

            return NotFound();
        }

        [HttpDelete("/Billings/{billingId:alpha}")]
        public IActionResult Delete([FromRoute] string billingId, [FromServices] AppDbContext context)
        {
            var billingModel = context.Billings!.Find(billingId);

            if (billingModel != null)
            {
                context.Billings!.Remove(billingModel);
                context.SaveChanges();

                return Ok(billingModel);
            }

            return NotFound("A cobrança já foi removida.");
        }
    }
}
