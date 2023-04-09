using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;
using Week11_MyShowList_RequestMyApi.Models;

namespace Week11_MyShowList_RequestMyApi.Pages.UserShowsList
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public MyShow MyShow { get; set; }
        private readonly HttpClient _httpClient;
        public string Error { get; set; }

        public AddModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void OnGet(int userId, string showId)
        {
            MyShow = new MyShow();
            MyShow.UserId = userId;
            MyShow.ShowId = showId;
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                // Everyting Binded to MyShow


                StringContent jsonContent = new StringContent(JsonSerializer.Serialize(MyShow), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://localhost:7060/api/MyShows/Post", jsonContent);
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
