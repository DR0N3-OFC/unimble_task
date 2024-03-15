using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TODOFront.Models;
using TODOFront.Models.APIConnection;

namespace TODOFront.Pages.Billing
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        public BillingModel? Billing { get; set; } = null;

        public string? QRCode { get; set; }


        readonly HttpContext httpContext;
        public IndexModel(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!ModelState.IsValid || !httpContext.Session!.GetInt32("UserID").HasValue)
            {
                return RedirectToPage("/User/Login");
            }

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/BillingByUser/{httpContext.Session!.GetInt32("UserID")}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var existingBilling = JsonConvert.DeserializeObject<BillingModel>(content);
                
                Billing = existingBilling!;

                url = $"{APIConnection.URL}/BillingQrCode/{Billing!.TxID}";
                requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

                response = await httpClient.SendAsync(requestMessage);
                
                content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var qrCode = content;

                    QRCode = qrCode!;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Billing!.UserID = (int)httpContext.Session.GetInt32("UserID")!;
            Billing.CPF = Billing.CPF!.Replace(".", "").Replace("-", "");

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Billings/";

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            var jsonBilling = JsonConvert.SerializeObject(Billing);
            requestMessage.Content = new StringContent(jsonBilling, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Billing/Index");
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError("Billing.CPF", errorResponse);

                return Page();
            }
        }
    }
}
