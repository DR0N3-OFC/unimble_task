using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TODOFront.Models;
using TODOFront.Models.APIConnection;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TODOFront.Pages.User
{
    [BindProperties]
    public class Register : PageModel
    {
        public UserModel NewUser { get; set; } = new();

        [Required(ErrorMessage = "Informe uma senha.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirme sua senha.")]
        [Compare("Password", ErrorMessage = "As senhas não são idênticas.")]
        public string? ConfirmPassword { get; set; }

        readonly HttpContext httpContext;
        public Register (IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            NewUser.Password = Password;

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Users/";

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            var jsonUser = JsonConvert.SerializeObject(NewUser);
            requestMessage.Content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var createdUserUrl = response.Headers.Location?.ToString();
                var userId = Convert.ToInt32(createdUserUrl?.Split('/').LastOrDefault());

                httpContext.Session.SetInt32("UserID", userId);
                httpContext.Session.SetString("UserName", NewUser.UserName!);

                return RedirectToPage("/Tasks/Index");
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError("NewUser.Email", errorResponse);

                return Page();
            }
        }
    }
}
