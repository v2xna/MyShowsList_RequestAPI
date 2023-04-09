using System.ComponentModel.DataAnnotations;

namespace Week11_MyShowList_RequestMyApi.Models
{
	public class LoginRequest
	{
		[Required]
		public string username { get; set; }
		[Required]
		public string password { get; set; }

		// Needed for BindProperty
		public LoginRequest()
		{

		}

		public LoginRequest(string username, string password)
		{
			this.username = username;
			this.password = password;
		}
	}
}
