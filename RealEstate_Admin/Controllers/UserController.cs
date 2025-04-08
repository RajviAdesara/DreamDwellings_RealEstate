using RealEstate_Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace RealEstate_Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly HttpClient _httpClient;

        public UserController(IConfiguration _configuration)
        {
            configuration = _configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        #region UserLogin

        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                        {
                            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                            sqlCommand.CommandText = "PR_User_Login";
                            sqlCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = userLoginModel.Email;
                            sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;
                            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                            DataTable dataTable = new DataTable();
                            dataTable.Load(sqlDataReader);

                            if (dataTable.Rows.Count == 0)
                            {
                                TempData["ErrorMessage"] = "Invalid username or password. Please try again.";
                                return RedirectToAction("UserLogin");
                            }

                            // Retrieve user data
                            DataRow dr = dataTable.Rows[0];
                            string userRole = dr["Role"].ToString(); // Assuming "Role" column exists in your DB

                            // Check if the role is "User"
                            if (userRole.Equals("User", StringComparison.OrdinalIgnoreCase))
                            {
                                TempData["ErrorMessage"] = "You are not authorized to log in.";
                                return RedirectToAction("UserLogin");
                            }

                            // Store session values
                            HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                            HttpContext.Session.SetString("Email", dr["Email"].ToString());

                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("UserLogin");
        }
        #endregion

        #region RegisterUser

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserModel userRegisterModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "PR_User_Insert";

                            // Add parameters
                            command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userRegisterModel.UserName;
                            command.Parameters.Add("@Password", SqlDbType.VarChar).Value = userRegisterModel.Password;
                            command.Parameters.Add("@Email", SqlDbType.VarChar).Value = userRegisterModel.Email;
                            command.Parameters.Add("@Role", SqlDbType.VarChar).Value = userRegisterModel.Role;

                            // Execute the query
                            command.ExecuteNonQuery();
                        }
                    }

                    // Confirmation message
                    TempData["ConfirmationMessage"] = "User registered successfully!";

                    // Redirect to login page
                    return RedirectToAction("UserLogin", "User");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            // If something fails, return to the register page
            return View("Register", userRegisterModel);
        }
        #endregion

        #region UserList
        public async Task<IActionResult> UserList()
        {
            var response = await _httpClient.GetAsync("api/User");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var properties = JsonConvert.DeserializeObject<List<RealEstate_Admin.Models.UserModel>>(data);
                return View(properties);
            }
            return View(new List<RealEstate_Admin.Models.UserModel>());
        }
        #endregion


        public async Task<IActionResult> AddUser(int? UserID)
           {
            if (UserID.HasValue)
            {
                var response = await _httpClient.GetAsync($"api/user/{UserID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserModel>(data);
                    return View(user);
                }
            }
            return View(new UserModel());
        }

        [HttpPost]
        public async Task<IActionResult> SaveUser(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (user.UserID == 0) // Assuming 0 means new user
                    response = await _httpClient.PostAsync("api/user", content);
                else
                    response = await _httpClient.PutAsync($"api/user/{user.UserID}", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("UserList");
            }

            return View("AddUser", user);
        }



        #region UserDelete
        public IActionResult DeleteUser(int UserID)
        {
            var response = _httpClient.DeleteAsync($"api/User/{UserID}").Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("UserList");
            }
            // Handle API failure
            var errorContent = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
            return RedirectToAction("UserList");
        }
        #endregion

        #region UserLogout

        public IActionResult Logout()
        {
            // Clear Session
            HttpContext.Session.Clear();

            // Remove Cookies
            Response.Cookies.Delete("UserID");
            Response.Cookies.Delete("Email");

            return RedirectToAction("UserLogin", "User");
        }

        #endregion
    }
}
