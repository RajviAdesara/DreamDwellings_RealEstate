using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RealEstate_Admin.Models;
using RealEstate_Admin.Services;
using System.Text;

namespace RealEstate_Admin.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly EmailSenderService _emailSender;


        public AppointmentController(IConfiguration configuration, EmailSenderService emailSender)
        {
            _configuration = configuration;
            _emailSender = emailSender;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configuration["WebApiBaseUrl"])
            };
        }

        public async Task<IActionResult> AppointmentList()
        {
            var response = await _httpClient.GetAsync("api/Appointment");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var appointments = JsonConvert.DeserializeObject<List<AppointmentModel>>(data);
                return View(appointments);
            }
            return View(new List<RealEstate_Admin.Models.AppointmentModel>());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] AppointmentStatusUpdateModel request)
        {
            if (request == null || request.Id == 0 || string.IsNullOrEmpty(request.Status))
            {
                return BadRequest("Invalid request data.");
            }

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Appointment/{request.Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to update appointment status.";
                return RedirectToAction("AppointmentList");
            }

            // Now fetch updated appointment details
            var updatedResponse = await _httpClient.GetAsync($"api/Appointment/{request.Id}");
            if (!updatedResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error fetching updated appointment.";
                return RedirectToAction("AppointmentList");
            }

            var data = await updatedResponse.Content.ReadAsStringAsync();
            var appointment = JsonConvert.DeserializeObject<AppointmentModel>(data);

            // Prepare email
            string formattedDate = appointment.AppointmentDate.ToString("MMMM dd, yyyy");
            string subject = $"Appointment {request.Status}";
            string body = request.Status == "Confirmed"
                ? $"Dear {appointment.UserEmail},\n\nYour appointment for {formattedDate} has been confirmed.\n\nThank you."
                : $"Dear {appointment.UserEmail},\n\nYour appointment for {formattedDate} has been canceled.\n\nIf this was a mistake, please reschedule.\n\nThank you.";
            try
            {
                await _emailSender.SendEmailAsync(appointment.UserEmail, subject, body, false);
                TempData["ConfirmationMessage"] = $"Appointment {request.Status.ToLower()} successfully and email sent!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Status updated, but error sending email: {ex.Message}";
            }

            return RedirectToAction("AppointmentList");
        }

    }
}
