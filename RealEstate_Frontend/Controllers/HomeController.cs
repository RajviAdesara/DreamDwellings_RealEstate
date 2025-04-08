using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Models;
using RealEstate;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net;
using RealEstate.Services;
using System.Text;
using Hangfire;

namespace RealEstate.Controllers
{
    [CheckAccess]
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly HttpClient _httpClient;
        private readonly EmailSenderService _emailSender;

        public HomeController(IConfiguration _configuration, EmailSenderService emailSender)
        {
            configuration = _configuration;
            _emailSender = emailSender;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        //public async Task<IActionResult> Index()
        //{
        //    string connectionString = this.configuration.GetConnectionString("ConnectionString");

        //    List<HomeModel> combinedData = new List<HomeModel>();

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        SqlCommand command = connection.CreateCommand();
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "PR_GetTotalPropertiesByType";

        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read()) // Read all rows
        //            {
        //                combinedData.Add(new HomeModel
        //                {
        //                    PropertyType = reader["PropertyType"].ToString(),
        //                    TotalCount = Convert.ToInt32(reader["TotalCount"])
        //                });
        //            }
        //        }
        //    }
        //    // Get agent data from API
        //    var response = await _httpClient.GetAsync("api/Agent");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var data = await response.Content.ReadAsStringAsync();
        //        var agents = JsonConvert.DeserializeObject<List<HomeModel>>(data);
        //        if (agents != null)
        //        {
        //            // Add agents to the combined data
        //            combinedData.AddRange(agents);
        //        }
        //    }

        //    return View(combinedData);
        //}

        public async Task<IActionResult> Index()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            List<HomeModel> propertyTypes = new List<HomeModel>();
            List<PropertyModel> recentProperties = new List<PropertyModel>();

            // Fetch Property Types from the Database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_GetTotalPropertiesByType";

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        propertyTypes.Add(new HomeModel
                        {
                            PropertyType = reader["PropertyType"].ToString(),
                            TotalCount = Convert.ToInt32(reader["TotalCount"])
                        });
                    }
                }
            }

            // Fetch Recent Properties from the API
            var response = await _httpClient.GetAsync("api/property");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                recentProperties = JsonConvert.DeserializeObject<List<PropertyModel>>(data);
            }

            // Create a ViewModel to pass both PropertyTypes and RecentProperties
            var viewModel = new HomeModel
            {
                PropertyTypes = propertyTypes,
                RecentProperties = recentProperties
            };

            return View(viewModel);
        }


        public IActionResult PropertiesByType(string propertyType, string category = null)
        {
            if (string.IsNullOrEmpty(propertyType))
            {
                return RedirectToAction("Index");
            }

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            List<PropertyModel> properties = new List<PropertyModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PR_GetPropertiesByTypeAndCategory", conn)) 
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PropertyType", propertyType);
                    cmd.Parameters.AddWithValue("@Category", (object)category ?? DBNull.Value); 

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            properties.Add(new PropertyModel
                            {
                                PropertyId = Convert.ToInt32(reader["PropertyId"]),
                                Title = reader["Title"]?.ToString(),
                                Description = reader["Description"]?.ToString(),
                                PropertyType = reader["PropertyType"]?.ToString(),
                                Category = reader["Category"]?.ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Location = reader["Location"]?.ToString(),
                                Area = Convert.ToSingle(reader["Area"]),
                                Bedrooms = reader["Bedrooms"] != DBNull.Value ? Convert.ToInt32(reader["Bedrooms"]) : (int?)null,
                                Bathrooms = reader["Bathrooms"] != DBNull.Value ? Convert.ToInt32(reader["Bathrooms"]) : (int?)null,
                                Amenities = reader["Amenities"]?.ToString(),
                                Status = reader["Status"]?.ToString(),
                                ImageName = reader["ImageName"]?.ToString(),
                                ImagePath = reader["ImagePath"]?.ToString()
                            });
                        }
                    }
                }
            }

            return View(properties);
        }



        [HttpPost]
        public async Task<IActionResult> SendAppointmentEmail(string userEmail, DateTime appointmentDate)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                return Json(new { success = false, message = "Invalid request data." });
            }

            string formattedDate = appointmentDate.ToString("MMMM dd, yyyy"); // Format without time

            string adminEmail = "rajzad912@gmail.com";
            string adminSubject = "New Appointment Request";
            string adminBody = $"User {userEmail} has booked an appointment for {formattedDate}."; // Only date

            string userSubject = "Appointment Request";
            string userBody = $"Dear User,\n\nYour appointment request for {formattedDate} has been received. We will confirm it soon.\n\nThank you.";

            try
            {
                if (_emailSender == null)
                {
                    return Json(new { success = false, message = "Email service not initialized." });
                }

                // Send email to admin
                await _emailSender.SendEmailAsync(adminEmail, adminSubject, adminBody, false);

                // Send confirmation email to the user
                await _emailSender.SendEmailAsync(userEmail, userSubject, userBody, false);

                return Json(new { success = true, message = "Emails sent successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending error: {ex.Message}");
                return Json(new { success = false, message = "Error sending email." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddAppointment([FromBody] AppointmentModel appointment)
        {
            if (appointment == null || string.IsNullOrEmpty(appointment.UserEmail) || appointment.AppointmentDate == default)
            {
                return BadRequest(new { success = false, message = "Invalid appointment data." });
            }

            Console.WriteLine($"Received Appointment: {JsonConvert.SerializeObject(appointment)}"); // Debugging

            var json = JsonConvert.SerializeObject(appointment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("api/Appointment", content);

            if (response.IsSuccessStatusCode)
            {
                // Call SendAppointmentEmail and pass DateTime directly
                var emailResponse = await SendAppointmentEmail(appointment.UserEmail, appointment.AppointmentDate);

                return Ok(new { success = true, message = "Appointment added successfully!" });
            }

            return StatusCode(500, new { success = false, message = "An error occurred while adding the appointment." });
        }


        public IActionResult Help()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
