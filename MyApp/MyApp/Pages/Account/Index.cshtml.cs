using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Pages.Account
{
    public class IndexModel : PageModel
    {
        public String username = "";
        public void OnGet()
        {

            if(HttpContext.Session.GetString("role")!= null && HttpContext.Session.GetString("role").Equals("user"))
            {
				username = HttpContext.Session.GetString("username");
			}
            

		}

		public void loadGuestPage()
		{



		}

	}


}
