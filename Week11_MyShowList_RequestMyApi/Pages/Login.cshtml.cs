using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;
using Week11_MyShowList_RequestMyApi.Models;

namespace Week11_MyShowList_RequestMyApi.Pages
{
    public class LoginModel : PageModel
    {
		[BindProperty]
        public LoginRequest Login { get; set; }
        private readonly HttpClient _httpClient;
		public string Error { get; set; }
		public int UserId { get; set; }
		public string Picture { get; set; }

		public LoginModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult OnGet(string error)
        {
			Error = error; // Error validations from other pages

			if (Request.Cookies["LoggedUserId"] != null)
			{
				return RedirectToPage("/Index");
			}

			return Page();
		}

		public async Task<IActionResult> OnPost()
		{
			if (ModelState.IsValid)
			{
				// body
				// Binds it automatically [BindProperty]

				// content
				StringContent jsonContent = new StringContent(
					JsonSerializer.Serialize(Login),
					Encoding.UTF8,
					"application/json"
					);

				// PostAsync --> needs content
				var response = await _httpClient.PostAsync("https://localhost:7060/api/Users/Login", jsonContent);
				var values = await response.Content.ReadAsStringAsync();
				var obj = JObject.Parse(values);

				// if Everything is good
				if (response.IsSuccessStatusCode)
				{
					UserId = Convert.ToInt32(obj["userId"]);
					Picture = obj["picture"].ToString();

					Response.Cookies.Append("LoggedUserId", UserId.ToString());
					Response.Cookies.Append("LoggedUserPicture", Picture);

					return RedirectToPage("/Index");
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
