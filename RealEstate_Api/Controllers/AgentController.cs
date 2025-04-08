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
    //[ServiceFilter(typeof(CheckAccess))] // Apply access check filter

    public class AgentController : ControllerBase
    {
        private readonly AgentRepository _agentRepository;

        public AgentController(AgentRepository agentRepository)
        {
            _agentRepository = agentRepository;
        }

        [HttpGet]
        public IActionResult GetAllAgents()
        {
            var agents = _agentRepository.SelectAllAgents();
            return Ok(agents);
        }

        [HttpGet("{id}")]
        public IActionResult GetAgentById(int id)
        {
            var agent = _agentRepository.GetAgentById(id);
            if (agent == null)
            {
                return NotFound("Agent not found.");
            }
            return Ok(agent);
        }

        [HttpPost]
        public IActionResult InsertAgent([FromBody] AgentModel agent)
        {
            if (agent == null)
            {
                return BadRequest("Invalid agent data.");
            }
            var isInserted = _agentRepository.InsertAgent(agent);
            if (isInserted)
            {
                return Ok(new { Message = "Agent inserted successfully!" });
            }
            return StatusCode(500, "An error occurred while inserting the agent.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAgent(int id, [FromBody] AgentModel agent)
        {
            if (agent == null || agent.AgentId != id)
            {
                return BadRequest("Invalid agent data or mismatched ID.");
            }
            var isUpdated = _agentRepository.UpdateAgent(agent);
            if (!isUpdated)
            {
                return NotFound("Agent not found.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAgent(int id)
        {
            var isDeleted = _agentRepository.DeleteAgent(id);
            if (!isDeleted)
            {
                return NotFound("Agent not found.");
            }
            return NoContent();
        }
    }
}