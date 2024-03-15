using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TODOFront.Models;
using TODOFront.Models.APIConnection;

namespace TODOFront.Pages.User
{
    public class Profile : PageModel
    {
        [BindProperty]
        public UserModel? ExistingUser { get; set; }

        readonly HttpContext httpContext;
        public Profile (IHttpContextAccessor httpContextAccessor)
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
            var url = $"{APIConnection.URL}/Users/{httpContext.Session!.GetInt32("UserID")}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var existingUser = JsonConvert.DeserializeObject<UserModel>(content);

                ExistingUser = existingUser;

                return Page();
            }

            return RedirectToPage("/Index");
        }

        public IActionResult OnPost()
        {
            httpContext.Session.Clear();

            return RedirectToPage("/Index");
        }
    }
}
