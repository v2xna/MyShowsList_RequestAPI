using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Week11_MyShowList_RequestMyApi.Models;

namespace Week11_MyShowList_RequestMyApi.Pages
{
    public class MoreInfoModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public Show Show { get; set; }
        public string dateOnlyString { get; set; }

        public MoreInfoModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
        }

        public async Task<IActionResult> OnGet(string id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7060/api/Shows/Get/{id}");
            var values = await response.Content.ReadAsStringAsync();
            var obj = JObject.Parse(values);


            Show = new Show(obj["id"].ToString(), obj["picture"].ToString(), obj["title"].ToString(), obj["synopsis"].ToString(), obj["type"].ToString(), obj["genres"].ToString(), Convert.ToInt32(obj["episodes"]), obj["studio"].ToString(), DateTime.Parse(obj["aired"].ToString()), obj["language"].ToString()); ;
            DateTime dateTime = Show.Aired;
            dateOnlyString = dateTime.ToString("yyyy-MM-dd");

            //Show.Id = obj["id"].ToString();
            //Show.Title = obj["title"].ToString();

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
