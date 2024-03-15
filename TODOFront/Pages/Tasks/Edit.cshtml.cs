using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using TODOFront.Models;
using TODOFront.Models.APIConnection;

namespace TODOFront.Pages.Tasks
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public TaskModel TaskToEdit { get; set; } = new();

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

                TaskToEdit = existingTask!;

                return Page();
            }

            return RedirectToPage("/Tasks/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/EditTask/{TaskToEdit.TaskId}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, url);
            var jsonEvent = JsonConvert.SerializeObject(TaskToEdit);
            requestMessage.Content = new StringContent(jsonEvent, Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(requestMessage);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Tasks/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, content);

                return Page();
            }
        }
    }
}
