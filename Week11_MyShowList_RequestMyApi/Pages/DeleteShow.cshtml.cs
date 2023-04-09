using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Week11_MyShowList_RequestMyApi.Models;

namespace Week11_MyShowList_RequestMyApi.Pages
{
    public class DeleteShowModel : PageModel
    {
        [BindProperty]
        public Show Show { get; set; }
        private readonly HttpClient _httpClient;

        public DeleteShowModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            // Retrieve data for html
			var response = await _httpClient.GetAsync($"https://localhost:7060/api/Shows/Get/{id}");
			var values = await response.Content.ReadAsStringAsync(); // Read the response as a string
			var obj = JObject.Parse(values); // The key (shows) with all the shows inside an array

			
		    // Only need 1 show
			Show = new Show(obj["id"].ToString(), obj["picture"].ToString(), obj["title"].ToString(), obj["synopsis"].ToString(), obj["type"].ToString(), obj["genres"].ToString(), Convert.ToInt32(obj["episodes"]), obj["studio"].ToString(), DateTime.Parse(obj["aired"].ToString()), obj["language"].ToString());
			
			return Page(); // Task<IActionResult>
		}

        public async Task<IActionResult> OnPost()
        {
			// DeleteAsync                                                     All the information are inside the "Show" from OnGet()
			var response = await _httpClient.DeleteAsync($"https://localhost:7060/api/Shows/Delete/{Show.Id}");
			var values = await response.Content.ReadAsStringAsync();

            return RedirectToPage("/Index");
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
