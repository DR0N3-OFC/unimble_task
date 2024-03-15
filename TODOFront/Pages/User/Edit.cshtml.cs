using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TODOFront.Models;
using TODOFront.Models.APIConnection;
using System.Net.Http;
using System;
using System.Reflection.Metadata;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TODOFront.Pages.User
{
    [BindProperties]
    public class Edit : PageModel
    {
        public UserModel UserToEdit { get; set; } = new();

        [Required(ErrorMessage = "Informe uma senha.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirme sua senha.")]
        [Compare("Password", ErrorMessage = "As senhas não são idênticas.")]
        public string? ConfirmPassword { get; set; }

        readonly HttpContext httpContext;
        public Edit (IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Users/{httpContext.Session!.GetInt32("UserID")}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var existingUser = JsonConvert.DeserializeObject<UserModel>(content);

                UserToEdit = existingUser!;

                return Page();
            }

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            UserToEdit.Password = Password;

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/EditUser/{httpContext.Session!.GetInt32("UserID")}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, url);
            var jsonUser = JsonConvert.SerializeObject(UserToEdit);
            requestMessage.Content = new StringContent(jsonUser, Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(requestMessage);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/User/Profile");
            }
            else
            {
                ModelState.AddModelError(string.Empty, content);

                return Page();
            }
        }
    }
}
