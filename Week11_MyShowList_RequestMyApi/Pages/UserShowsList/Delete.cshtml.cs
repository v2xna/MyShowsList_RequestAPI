using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Week11_MyShowList_RequestMyApi.Models;

namespace Week11_MyShowList_RequestMyApi.Pages.UserShowsList
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public MyShow MyShow { get; set; }
        private readonly HttpClient _httpClient;

        public DeleteModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGet(int id)
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
			// DeleteAsync                                                     All the information are inside the "Show" from OnGet()
			var response = await _httpClient.DeleteAsync($"https://localhost:7060/api/MyShows/Delete/{MyShow.Id}");
			var values = await response.Content.ReadAsStringAsync();

			return RedirectToPage("/UserShowsList/Index");
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
