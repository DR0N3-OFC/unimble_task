using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using TODOFront.Models;
using TODOFront.Models.APIConnection;

namespace TODOFront.Pages.Tasks
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        public List<TaskModel> TasksList { get; set; } = new();
        public TaskModel NewTask { get; set; } = new();

        private bool _userIsPremium;

        readonly HttpContext httpContext;
        public IndexModel(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task LoadPropertiesListAsync()
        {
            if (httpContext.Session.GetInt32("UserID") == null)
            {
                return;
            }

            _userIsPremium = httpContext.Session!.GetString("AccountType") == "Premium" ? true : false;
            
            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/TasksByUser/{httpContext.Session.GetInt32("UserID")}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var tasksList = JsonConvert.DeserializeObject<List<TaskModel>>(content);

            TasksList = tasksList!;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (httpContext.Session.GetInt32("UserID") == null)
            {
                return RedirectToPage("/User/Login");
            }

            await LoadPropertiesListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadPropertiesListAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            if (TasksList.Count >= 5 && !_userIsPremium)
            {
                return RedirectToPage("/Billing/Index");
            }

            NewTask.OrganizerId = (int)httpContext.Session.GetInt32("UserID")!;

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Tasks/";

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            var jsonTask = JsonConvert.SerializeObject(NewTask);
            requestMessage.Content = new StringContent(jsonTask, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Tasks/Index");
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError(string.Empty, errorResponse);

                return Page();
            }
        }

        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            await LoadPropertiesListAsync();

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Tasks/{id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var taskModel = JsonConvert.DeserializeObject<TaskModel>(content)!;

            if (taskModel.Status == 0)
            {
                taskModel.Status = 1;
            }
            else
            {
                taskModel.Status = 0;
            }

            url = $"{APIConnection.URL}/EditTask/{id}";

            requestMessage = new HttpRequestMessage(HttpMethod.Put, url);
            var jsonTask = JsonConvert.SerializeObject(taskModel);
            requestMessage.Content = new StringContent(jsonTask, Encoding.UTF8, "application/json");

            response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Tasks/Index");
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError(string.Empty, errorResponse);

                return Page();
            }
        }
    }
}
