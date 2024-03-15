using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TODOBack.Data;
using TODOBack.Models;

namespace PlanejaiBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("/Users/")]
        public IActionResult Get([FromServices] AppDbContext context)
        {
            return Ok(context.Users!.OrderBy(u => u.UserName).ToList());
        }

        [HttpGet("/Users/{id:int}")]
        public IActionResult GetByID([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var user = context.Users!.FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("/Users/{email}/{password}")]
        public IActionResult GetByEmailAndPassword([FromRoute] string email, string password, [FromServices] AppDbContext context)
        {
            var user = context.Users!.FirstOrDefault(u => (u.Email == email) && (u.Password == password));

            if (user == null)
            {
                return NotFound("Dados inválidos. Confira os dados inseridos e tente novamente.");
            }

            return Ok(user);
        }

        [HttpPost("/Users/")]
        public IActionResult Post([FromBody] UserModel user, [FromServices] AppDbContext context)
        {
            var existingUser = context.Users!.SingleOrDefault(u => u.Email == user.Email);

            if (existingUser == null)
            {
                context.Users!.Add(user);
                context.SaveChanges();

                return Created($"/{user.UserId}", user);
            }

            return BadRequest("O e-mail informado já está em uso.");
        }

        [HttpPut("/EditUser/{userId:int}")]
        public IActionResult Put([FromRoute] int userId, [FromBody] UserModel user, [FromServices] AppDbContext context)
        {
            if (userId == user.UserId)
            {
                context.Users!.Update(user);
                context.SaveChanges();

                return Ok(user);
            }

            return NotFound();
        }
    }
}
