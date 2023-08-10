using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Pages.Account
{
    public class IndexModel : PageModel
    {
        public String username = "";

		[BindProperty]
		public string Action { get; set; }
		public void OnGet()
        {

            if(HttpContext.Session.GetString("role")!= null)
            {
				username = HttpContext.Session.GetString("username");
			}
            

		}


		
		public void OnPost()
		{
			//String answer = "Logout";
			switch (Action) 
			{
				case "Logout":
					username = "";
					HttpContext.Session.Clear();
					Response.Redirect("/Index");
					
			
					break;

			}


		}



	}


}
