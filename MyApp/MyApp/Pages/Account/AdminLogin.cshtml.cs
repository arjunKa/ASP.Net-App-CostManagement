using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

using Microsoft.AspNetCore.Http;


namespace MyApp.Pages.Account
{
    public class AdminLoginModel : PageModel
    {
		public UserInfo userInfo = new UserInfo();
		public String errorMessage = "";
		public String successMessage = "";

		public void OnGet()
		{
		}

		public void OnPost()
		{
			userInfo.username = Request.Form["username"];
			userInfo.password = Request.Form["password"];

			if (userInfo.username.Length == 0 || userInfo.password.Length == 0)
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
					String sql = "SELECT * FROM admin_users WHERE " +
						"username = @username " + "AND password = @password";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{

						command.Parameters.AddWithValue("@username", userInfo.username);
						command.Parameters.AddWithValue("@password", userInfo.password);
						SqlDataReader dr = command.ExecuteReader();

						if(dr.HasRows)
						{
							if (dr.Read())
							{
								
								HttpContext.Session.SetString("username", userInfo.username);
								HttpContext.Session.SetString("role", "admin");

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

			userInfo.username = ""; userInfo.email = ""; userInfo.password = "";
			successMessage = "User logged in successfully.";

			Response.Redirect("/Account/Index");
		}
	}
}
