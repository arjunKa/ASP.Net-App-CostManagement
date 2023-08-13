using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyApp.Pages.Entries
{
    public class CreateModel : PageModel
    {
        public BillingInfo clientInfo = new BillingInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public String username;
        public int last_id=0;

        public void OnGet()
        {
			username = HttpContext.Session.GetString("username");
		}

        public void OnPost()
        {
			username = HttpContext.Session.GetString("username");
			clientInfo.Name = Request.Form["name"];
            clientInfo.cost = decimal.Parse(Request.Form["cost"]);
            clientInfo.service = Request.Form["service"];


            if (clientInfo.Name.Length == 0)
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
					String sql = "SELECT TOP 1 id FROM billings WHERE username = @username  " +
	                " ORDER BY created_at DESC;";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@username", username);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								last_id = reader.GetInt32(0) + 1;
								
							}

						}
					}
				}

				using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO billings " +
                        "(username, id, bill_name, service, cost) VALUES " +
                        "(@username, @id, @name, @service, @cost);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.Name);
						command.Parameters.AddWithValue("@username", username);
						command.Parameters.AddWithValue("@service", clientInfo.service);
						command.Parameters.AddWithValue("@cost", clientInfo.cost);
						command.Parameters.AddWithValue("@id", last_id);

						command.ExecuteNonQuery();
					}
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            clientInfo.Name = ""; clientInfo.service = ""; clientInfo.cost = 0.0M;
            successMessage = "New Client added successfully.";

            Response.Redirect("/Entries/Index");
        }
    }
}
