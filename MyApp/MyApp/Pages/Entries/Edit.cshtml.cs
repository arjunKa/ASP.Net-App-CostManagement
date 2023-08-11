using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace MyApp.Pages.Entries
{
    public class EditModel : PageModel
    {
		public BillingInfo clientInfo = new BillingInfo();
		public String errorMessage = "";
		public String successMessage = "";
		public String username;
		public void OnGet()
        {
            String id = Request.Query["id"];
			username = HttpContext.Session.GetString("username");

			try
            {
                String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myAppDBCS"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM billings WHERE username=@username AND id=@id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", id);
						command.Parameters.AddWithValue("@username", username);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if(reader.Read())
							{
								clientInfo.Id = "" + reader.GetInt32(0);
								clientInfo.Name = reader.GetString(2);
								clientInfo.service = reader.GetString(3);
								clientInfo.cost = reader.GetDecimal(4);
								

							}



						}

					}
				}
			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
		public void OnPost()
		{
			username = HttpContext.Session.GetString("username");
			clientInfo.Id = Request.Form["id"];
			clientInfo.Name = Request.Form["name"];
			clientInfo.service = Request.Form["service"];
			clientInfo.cost = decimal.Parse(Request.Form["cost"]);
			

			if (clientInfo.Id.Length == 0 || clientInfo.Name.Length == 0
				|| clientInfo.service.Length == 0)
			{
				errorMessage = "All fields are required";
				return;

			}

			try
			{

				String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myAppDBCS"].ConnectionString;
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE billings " +
						"SET bill_name=@name, service=@service, cost=@cost " +
						"WHERE username=@username AND id=@id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@username", username);
						command.Parameters.AddWithValue("@name", clientInfo.Name);
						command.Parameters.AddWithValue("@service", clientInfo.service);
						command.Parameters.AddWithValue("@cost", clientInfo.cost);
						command.Parameters.AddWithValue("@id", clientInfo.Id);

						command.ExecuteNonQuery();
					}
				}

			}
			catch (Exception ex)
			{ 
				errorMessage=ex.Message;
				return;
			}

			Response.Redirect("/Clients/Index");

		}

    }
}
