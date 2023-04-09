using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;
using Week11_MyShowList_RequestMyApi.Models;

namespace Week11_MyShowList_RequestMyApi.Pages.UserShowsList
{
    public class EditModel : PageModel
    {
		[BindProperty]
		public MyShow MyShow { get; set; }
		private readonly HttpClient _httpClient;
		public string Error { get; set; }

		public EditModel (HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

        public async Task<IActionResult> OnGet(string id)
        {
			var response = await _httpClient.GetAsync($"https://localhost:7060/api/MyShows/GetOneMyShow/{id}");
			var values = await response.Content.ReadAsStringAsync();
			var obj = JObject.Parse(values);


			// Only need 1 show
			MyShow = new MyShow(Convert.ToInt32(obj["id"]), Convert.ToInt32(obj["userId"]), obj["showId"].ToString(), Convert.ToInt32(obj["rating"]), obj["progress"].ToString(), obj["comment"].ToString());

			return Page(); // Task<IActionResult>
		}

		public async Task<IActionResult> OnPost()
		{
			if (ModelState.IsValid)
			{
				// Everyting Binded to MyShow


				StringContent jsonContent = new StringContent(JsonSerializer.Serialize(MyShow), Encoding.UTF8, "application/json");

				// PutAsync
				var response = await _httpClient.PutAsync($"https://localhost:7060/api/MyShows/Put/{MyShow.Id}", jsonContent);
				var values = await response.Content.ReadAsStringAsync();
				var obj = JObject.Parse(values);

				if (response.IsSuccessStatusCode)
				{
					return RedirectToPage("/UserShowsList/Index");
				}
				else
				{
					Error = obj["error_Message"].ToString();
				}
			}

			return Page();
		}

		public IActionResult OnPostLogout()
		{
			CookieOptions options = new CookieOptions();
			options.Expires = DateTime.Now.AddDays(-1);

			Response.Cookies.Append("LoggedUserId", "", options);
			Response.Cookies.Append("LoggedUserPicture", "", options);

			return RedirectToPage("/Login");
		}
	}
}
