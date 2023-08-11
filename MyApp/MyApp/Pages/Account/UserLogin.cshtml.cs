using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

using Microsoft.AspNetCore.Http;


namespace MyApp.Pages.Account
{
    public class UserLoginModel : PageModel
    {
		public UserInfo userInfo = new UserInfo();
		public String errorMessage = "";
		public String successMessage = "";

		public String username { get; set; }
		public String password { get; set; }


		public void OnGet()
		{
		}

		public void OnPost()
		{
			username = Request.Form["username"];
			password = Request.Form["password"];

			if (username.Length == 0 || password.Length == 0)
			{
				errorMessage = "All fields are required.";
				return;
			}

			try
			{
				String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myAppDBCS"].ConnectionString;
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM users WHERE " +
						"username = @username " + "AND password = @password";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{

						command.Parameters.AddWithValue("@username", username);
						command.Parameters.AddWithValue("@password", password);
						SqlDataReader dr = command.ExecuteReader();

						if(dr.HasRows)
						{
							if (dr.Read())
							{
								
								HttpContext.Session.SetString("username", username);
								HttpContext.Session.SetString("role", "user");

							}
						} else
						{
							errorMessage = "No such login.";
							return;
						}

					}
				}

			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}

			username = ""; password = "";
			successMessage = "User logged in successfully.";

			Response.Redirect("/Account/Index");
		}
	}
}
