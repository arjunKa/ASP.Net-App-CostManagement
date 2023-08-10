using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace MyApp.Pages.Clients
{
    public class EditModel : PageModel
    {
		public ClientInfo clientInfo = new ClientInfo();
		public String errorMessage = "";
		public String successMessage = "";
		public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myAppDBCS"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM clients WHERE id=@id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if(reader.Read())
							{
								clientInfo.Id = "" + reader.GetInt32(0);
								clientInfo.Name = reader.GetString(1);
								clientInfo.email = reader.GetString(2);
								clientInfo.phone = reader.GetString(3);
								clientInfo.address = reader.GetString(4);

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
			clientInfo.Id = Request.Form["id"];
			clientInfo.Name = Request.Form["name"];
			clientInfo.email = Request.Form["email"];
			clientInfo.phone = Request.Form["phone"];
			clientInfo.address = Request.Form["address"];

			if (clientInfo.Id.Length == 0 || clientInfo.Name.Length == 0 || clientInfo.email.Length == 0
				|| clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
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
					String sql = "UPDATE clients " +
						"SET name=@name, email=@email, address=@address, phone=@phone " +
						"WHERE id=@id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@name", clientInfo.Name);
						command.Parameters.AddWithValue("@email", clientInfo.email);
						command.Parameters.AddWithValue("@phone", clientInfo.phone);
						command.Parameters.AddWithValue("@address", clientInfo.address);
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
