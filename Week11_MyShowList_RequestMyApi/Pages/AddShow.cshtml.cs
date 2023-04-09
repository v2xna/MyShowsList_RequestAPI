using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;
using Week11_MyShowList_RequestMyApi.Models;

namespace Week11_MyShowList_RequestMyApi.Pages
{
    public class AddShowModel : PageModel
    {
        [BindProperty]
        public Show Show { get; set; }

        private readonly HttpClient _httpClient;
        public string Error { get; set; }

        public AddShowModel (HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult OnGet()
        {
            // if they try accessing via URL
            if (Request.Cookies["LoggedUserId"] == null)
            {
                return RedirectToPage("/Login", new { Error = "Please login!" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                StringContent jsonContent = new StringContent(JsonSerializer.Serialize(Show), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://localhost:7060/api/Shows/Post", jsonContent);
                var values = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(values);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    Error = obj["error_Message"].ToString();
                }
            }

            return Page();
        }
    }
}
