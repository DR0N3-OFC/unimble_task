using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Diagnostics;
using TODOFront.Models;
using TODOFront.Models.APIConnection;

namespace TODOFront.Pages.Tasks
{
    public class RemoveModel : PageModel
    {
        [BindProperty]
        public TaskModel Task { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Tasks/{id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var existingTask = JsonConvert.DeserializeObject<TaskModel>(content);

                Task = existingTask!;

                return Page();
            }

            return RedirectToPage("/Tasks/Index");
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Tasks/{id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);

            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Tasks/Index");
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, content);

                return Page();
            }
        }
    }
}
