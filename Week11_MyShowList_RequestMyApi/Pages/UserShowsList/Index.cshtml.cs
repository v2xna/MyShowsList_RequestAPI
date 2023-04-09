using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Week11_MyShowList_RequestMyApi.Models;

namespace Week11_MyShowList_RequestMyApi.Pages.UserShowsList
{
    public class IndexModel : PageModel
    {
        public List<MyShow> MyShows = new List<MyShow>();
        public List<Show> Shows = new List<Show>();
        private readonly HttpClient _httpClient;
        public int UserId { get; set; }

        public IndexModel(HttpClient httpclient)
        {
            _httpClient = httpclient;
        }
        public async Task<IActionResult> OnGet()
        {
            UserId = Convert.ToInt32(Request.Cookies["LoggedUserId"]);

            if (Request.Cookies["LoggedUserId"] == null)
            {
                return RedirectToPage("/Login", new {Error = "Please login!"});
            }


            var response = await _httpClient.GetAsync($"https://localhost:7060/api/MyShows/GetShowsByUser/{UserId}");
            var values = await response.Content.ReadAsStringAsync();
            var obj = JObject.Parse(values);

            foreach(var item in obj["myFavorites"])
            {
                MyShows.Add(new MyShow(Convert.ToInt32(item["id"]), Convert.ToInt32(item["userId"]), item["showId"].ToString(), Convert.ToInt32(item["rating"]), item["progress"].ToString(), item["comment"].ToString()));
            }

			foreach (var item in obj["shows"])
			{
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
