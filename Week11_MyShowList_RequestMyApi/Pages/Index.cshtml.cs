using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Week11_MyShowList_RequestMyApi.Models;

namespace Week11_MyShowList_RequestMyApi.Pages
{
	public class IndexModel : PageModel
	{
		public List<Show> Shows = new List<Show>();
		private readonly HttpClient _httpClient;
		public IndexModel(HttpClient httpClient)
		{
			_httpClient= httpClient;
		}

		public async Task<IActionResult> OnGet()
		{
			//														API URL
			var response = await _httpClient.GetAsync("https://localhost:7060/api/Shows/Get");
			var values = await response.Content.ReadAsStringAsync(); // Read the response as a string
			var obj = JObject.Parse(values); // The key (shows) with all the shows inside an array

			foreach(var item in obj["shows"])
			{
				// Populating my list
				Shows.Add(new Show(item["id"].ToString(), item["picture"].ToString(), item["title"].ToString(), item["synopsis"].ToString(), item["type"].ToString(), item["genres"].ToString(), Convert.ToInt32(item["episodes"]), item["studio"].ToString(), DateTime.Parse(item["aired"].ToString()), item["language"].ToString()));
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