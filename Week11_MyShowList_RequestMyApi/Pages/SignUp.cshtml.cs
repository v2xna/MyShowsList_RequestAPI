using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;
using Week11_MyShowList_RequestMyApi.Models;

namespace Week11_MyShowList_RequestMyApi.Pages
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public User User { get; set; }
        private readonly HttpClient _httpClient;
        public string Error { get; set; }

        public SignUpModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult OnGet()
        {
            // No Access to this page if user is logged
			if (Request.Cookies["LoggedUserId"] != null)
			{
				return RedirectToPage("/Index");
			}

			return Page();
		}

        public async Task<IActionResult> OnPost()
        {
            // Call API when form is valid
            if (ModelState.IsValid)
            {
				// User has all the properties binded

				// Content
				StringContent jsonContent = new StringContent(
					JsonSerializer.Serialize(User),
					Encoding.UTF8,
					"application/json"
					);

				// PostAsync --> needs a second param for Content
				var response = await _httpClient.PostAsync("https://localhost:7060/api/Users/Register", jsonContent);
				var values = await response.Content.ReadAsStringAsync();
				var obj = JObject.Parse(values);

                // if Everything is good
                if (response.IsSuccessStatusCode)
                {
					return RedirectToPage("/Login");
				}
                else
                {
                    // error_Message --> Key from API
                    Error = obj["error_Message"].ToString();
                }
			}

            return Page(); 
        }

    }
}
