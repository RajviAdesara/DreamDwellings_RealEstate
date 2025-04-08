using RealEstate_Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.Eventing.Reader;

namespace RealEstate_Admin.Controllers
{
    //[CheckAccess]
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        #region DynamicDashboardTabs

        public IActionResult Index()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            HomeModel dashboardViewModel = new HomeModel();

            #region TotalUsers
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_GetTotalUsers"; 
                dashboardViewModel.TotalUsers = Convert.ToInt32(command.ExecuteScalar());
            }
            #endregion

            #region TotalProperties
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_GetTotalProperties"; 
                dashboardViewModel.TotalProperties = Convert.ToInt32(command.ExecuteScalar());
            }
            #endregion

            #region TotalAgents
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_GetTotalAgents";
                dashboardViewModel.TotalAgents = Convert.ToInt32(command.ExecuteScalar());
            }
            #endregion

            #region TotalBuy
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_GetTotalBuyProperties";
                dashboardViewModel.TotalBuyProperties = Convert.ToInt32(command.ExecuteScalar());
            }
            #endregion

            #region TotalSell
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_GetTotalSellProperties";
                dashboardViewModel.TotalSellProperties = Convert.ToInt32(command.ExecuteScalar());
            }
            #endregion

            #region TotalRent
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_GetTotalRentProperties";
                dashboardViewModel.TotalRentProperties = Convert.ToInt32(command.ExecuteScalar());
            }
            #endregion

            #region TotalActiveAgents
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_GetTotalActiveAgents";
                dashboardViewModel.TotalActiveAgents = Convert.ToInt32(command.ExecuteScalar());
            }
            #endregion

            #region TotalInactiveAgents
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_GetTotalInactiveAgents";
                dashboardViewModel.TotalInactiveAgents = Convert.ToInt32(command.ExecuteScalar());
            }
            #endregion

            #region TotalSuspendedAgents
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_GetTotalSuspendedAgents";
                dashboardViewModel.TotalSuspendedAgents = Convert.ToInt32(command.ExecuteScalar());
            }
            #endregion


            #region RecentPropertiesForsell
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PR_Property_Dashboard", conn)) // Calling the stored procedure for sold properties
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dashboardViewModel.RecentPropertySold.Add(new PropertiesModel
                            {
                                PropertyId = Convert.ToInt32(reader["PropertyId"]),
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"].ToString(),
                                PropertyType = reader["PropertyType"].ToString(),
                                Category = reader["Category"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Location = reader["Location"].ToString(),
                                Area = Convert.ToSingle(reader["Area"]),
                                Bedrooms = Convert.ToInt32(reader["Bedrooms"]),
                                Bathrooms = Convert.ToInt32(reader["Bathrooms"]),
                                Amenities = reader["Amenities"].ToString(),
                                ListedDate = Convert.ToDateTime(reader["ListedDate"]),
                                Status = reader["Status"].ToString(),
                                ImageName = reader["ImageName"].ToString(),
                                ImagePath = reader["ImagePath"].ToString()
                            });
                        }
                    }
                }
            }

            #endregion

            return View(dashboardViewModel);

        }

        #endregion


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
