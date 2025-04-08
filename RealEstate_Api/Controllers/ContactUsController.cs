using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate_Api.Data;
using RealEstate_Api.Models;

namespace RealEstate_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly ContactUsRepository _contactusRepository;

        public ContactUsController(ContactUsRepository contactusRepository)
        {
            _contactusRepository = contactusRepository;
        }

        // GET: api/Feedback
        [HttpGet]
        public IActionResult GetAllFeedback()
        {
            var feedbacks = _contactusRepository.SelectAllFeedback();
            return Ok(feedbacks);
        }

        // POST: api/Feedback
        [HttpPost]
        public IActionResult InsertFeedback([FromBody] ContactUsModel feedback)
        {
            if (feedback == null)
            {
                return BadRequest("Invalid feedback data.");
            }
            var isInserted = _contactusRepository.InsertFeedback(feedback);
            if (isInserted)
            {
                return Ok(new { Message = "Feedback inserted successfully!" });
            }
            return StatusCode(500, "An error occurred while inserting the feedback.");
        }

    }
}
