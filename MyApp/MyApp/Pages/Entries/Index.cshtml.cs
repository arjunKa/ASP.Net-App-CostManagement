using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics.Contracts;

namespace MyApp.Pages.Entries
{
    public class IndexModel : PageModel
    {
        public List<BillingInfo> listClients = new List<BillingInfo>();
        public string? username;
        public string? role;
        public void OnGet()
        {
            username = HttpContext.Session.GetString("username");
			role = HttpContext.Session.GetString("role");
			try
            {
                String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myAppDBCS"].ConnectionString;
                Console.WriteLine(connectionString);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM billings WHERE username = @username";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
						command.Parameters.AddWithValue("@username", username);
						
						using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BillingInfo clientInfo = new BillingInfo();
                                clientInfo.Id = "" + reader.GetInt32(0);
                                clientInfo.Name = reader.GetString(2);
                                clientInfo.service = reader.GetString(3);
                                clientInfo.cost = reader.GetDecimal(4);
                                clientInfo.created_at = reader.GetDateTime(5).ToLocalTime().ToString();
								
                                listClients.Add(clientInfo);

                            }
                        }
                    }
                }
            
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Exception: " + ex.ToString());

            }
        }
    }

    public class BillingInfo
    {
        public String Id;
        public String Name;
        public decimal cost;
        public String service;
        public String created_at;
	}
}
