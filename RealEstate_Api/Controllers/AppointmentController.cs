using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate_Api.Data;
using RealEstate_Api.Models;

namespace RealEstate_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentRepository _appointmentRepository;

        public AppointmentController(AppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        [HttpGet]
        public IActionResult GetAllAppointments()
        {
            var appointments = _appointmentRepository.GetAllAppointments();
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public IActionResult GetAppointmentById(int id)
        {
            var appointment = _appointmentRepository.GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }
            return Ok(appointment);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateStatus(int id, [FromBody] AppointmentStatusUpdateModel appointment)
        {
            if (appointment == null || appointment.Id != id)
            {
                return BadRequest("Invalid appointment data or mismatched ID.");
            }

            var isUpdated = _appointmentRepository.UpdateStatus(id, appointment.Status);

            if (!isUpdated)
            {
                return NotFound("Appointment not found.");
            }

            return NoContent();
        }

        [HttpPost]
        public IActionResult InsertAppointment([FromBody] AppointmentModel appointment)
        {
            if (appointment == null)
            {
                return BadRequest("Invalid appointment data.");
            }

            var isInserted = _appointmentRepository.InsertAppointment(appointment);
            if (isInserted)
            {
                return Ok(new { Message = "Appointment inserted successfully!" });
            }

            return StatusCode(500, "An error occurred while inserting the appointment.");
        }

    }
}
