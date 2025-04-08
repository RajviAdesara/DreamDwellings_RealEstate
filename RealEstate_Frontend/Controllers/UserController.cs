using RealEstate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using Hangfire;
using RealEstate.Services;

namespace RealEstate.Controllers
{
    public class UserController : Controller
    {
        private IConfiguration configuration;
        private readonly EmailSenderService _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IConfiguration _configuration, EmailSenderService emailSender, IHttpContextAccessor httpContextAccessor)
        {
            configuration = _configuration;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
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

                            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(sqlDataReader);

                                if (dataTable.Rows.Count == 0)
                                {
                                    TempData["ErrorMessage"] = "Invalid username or password. Please try again.";
                                    return RedirectToAction("UserLogin");
                                }

                                foreach (DataRow dr in dataTable.Rows)
                                {
                                    string userId = dr["UserID"].ToString();
                                    string email = dr["Email"].ToString();

                                    // ✅ Store user details in Session
                                    HttpContext.Session.SetString("UserID", userId);
                                    HttpContext.Session.SetString("Email", email);

                                    // ✅ Store user details in Cookies
                                    CookieOptions cookieOptions = new CookieOptions
                                    {
                                        Expires = DateTime.UtcNow.AddDays(7), // Cookie expires in 7 days
                                        HttpOnly = false,
                                        Secure = true, // Ensures the cookie is sent over HTTPS
                                        IsEssential = true
                                    };

                                    Response.Cookies.Append("UserID", userId, cookieOptions);
                                    Response.Cookies.Append("Email", email, cookieOptions);

                                    // ✅ Schedule an email using Hangfire (runs after login)
                                    BackgroundJob.Enqueue(() => _emailSender.SendPromotionalEmailWithEmail(email));
                                }
                            }
                        }
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("UserLogin");
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

    }
}

