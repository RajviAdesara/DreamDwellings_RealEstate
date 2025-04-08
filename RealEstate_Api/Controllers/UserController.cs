using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using RealEstate_Api.Data;
using RealEstate_Api.Models;

namespace RealEstate_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.SelectAllUsers();
            return Ok(users);
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
        public IActionResult InsertUser([FromBody] UserModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            var isInserted = _userRepository.InsertUser(user);

            if (isInserted)
            {
                return Ok(new { Message = "User inserted successfully!" });
            }

            return StatusCode(500, "An error occurred while inserting the user.");
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserModel user)
        {
            if (user == null || user.UserId != id)
            {
                return BadRequest("Invalid user data or mismatched ID.");
            }

            var isUpdated = _userRepository.UpdateUser(user);

            if (!isUpdated)
            {
                return NotFound("User not found.");
            }

            return NoContent();
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var isDeleted = _userRepository.DeleteUser(id);
            if (!isDeleted)
            {
                return NotFound("User not found.");
            }

            return NoContent();
        }

        // POST: api/User/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var user = _userRepository.Login(loginRequest.Email, loginRequest.Password);

            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            // Store UserID in session to allow access to protected routes
            HttpContext.Session.SetString("UserID", user.UserId.ToString());

            return Ok(new
            {
                Message = "Login successful.",
                User = user,
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clears all session data
            return Ok(new { Message = "Logout successful." });
        }

    }
}

