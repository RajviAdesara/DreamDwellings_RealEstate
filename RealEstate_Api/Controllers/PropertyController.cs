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
    public class PropertyController : ControllerBase
    {
        private readonly PropertyRepository _propertyRepository;

        public PropertyController(PropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        [HttpGet]
        public IActionResult GetAllProperties()
        {
            var properties = _propertyRepository.SelectAllProperties();
            return Ok(properties);
        }

        [HttpGet("category/{category}")]
        public IActionResult PropertyList(string category)
        {
                var properties = _propertyRepository.GetPropertiesByCategory(category);
                if (properties == null || !properties.Any())
                {
                    return NotFound("No properties found for the specified category.");
                }
                return Ok(properties);
        }

        [HttpGet("propertytype/{propertyType}")]
        public IActionResult PropertyListByType(string propertyType)
        {
            var properties = _propertyRepository.GetPropertiesByPropertyType(propertyType);
            if (properties == null || !properties.Any())
            {
                return NotFound("No properties found for the specified property type.");
            }
            return Ok(properties);
        }

        [HttpGet("{id}")]
        public IActionResult GetPropertyById(int id)
        {
            var property = _propertyRepository.GetPropertyById(id);
            if (property == null)
            {
                return NotFound("Property not found.");
            }
            return Ok(property);
        }
        
        [HttpPost]
        public IActionResult InsertProperty([FromBody] PropertyModel property)
        {
            if (property == null)
            {
                return BadRequest("Invalid property data.");
            }
            var isInserted = _propertyRepository.InsertProperty(property);
            if (isInserted)
            {
                return Ok(new { Message = "Property inserted successfully!" });
            }
            return StatusCode(500, "An error occurred while inserting the property.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProperty(int id, [FromBody] PropertyModel property)
        {
            if (property == null || property.PropertyId != id)
            {
                return BadRequest("Invalid property data or mismatched ID.");
            }
            var isUpdated = _propertyRepository.UpdateProperty(property);
            if (!isUpdated)
            {
                return NotFound("Property not found.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProperty(int id)
        {
            var isDeleted = _propertyRepository.DeleteProperty(id);
            if (!isDeleted)
            {
                return NotFound("Property not found.");
            }
            return NoContent();
        }
    }
}
