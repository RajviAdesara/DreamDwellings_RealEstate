using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate_Api.Data;
using RealEstate_Api.Models;

namespace RealEstate_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyTypeController : ControllerBase
    {
        private readonly PropertyTypeRepository _propertyTypeRepository;

        public PropertyTypeController(PropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        // GET: api/PropertyType
        [HttpGet]
        public IActionResult GetAllPropertyTypes()
        {
            var propertyTypes = _propertyTypeRepository.GetAllPropertyTypes();
            return Ok(propertyTypes);
        }

        // GET: api/PropertyType/{id}
        [HttpGet("{id}")]
        public IActionResult GetPropertyTypeById(int id)
        {
            var propertyType = _propertyTypeRepository.GetPropertyTypeById(id);
            if (propertyType == null)
            {
                return NotFound("Property type not found.");
            }
            return Ok(propertyType);
        }

        // POST: api/PropertyType
        [HttpPost]
        public IActionResult InsertPropertyType([FromBody] PropertyTypeModel propertyType)
        {
            if (propertyType == null)
            {
                return BadRequest("Invalid property type data.");
            }

            var isInserted = _propertyTypeRepository.InsertPropertyType(propertyType);

            if (isInserted)
            {
                return Ok(new { Message = "Property type inserted successfully!" });
            }

            return StatusCode(500, "An error occurred while inserting the property type.");
        }

        // PUT: api/PropertyType/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePropertyType(int id, [FromBody] PropertyTypeModel propertyType)
        {
            if (propertyType == null || propertyType.PropertyTypeId != id)
            {
                return BadRequest("Invalid property type data or mismatched ID.");
            }

            var isUpdated = _propertyTypeRepository.UpdatePropertyType(propertyType);

            if (!isUpdated)
            {
                return NotFound("Property type not found.");
            }

            return NoContent();
        }

        // DELETE: api/PropertyType/{id}
        [HttpDelete("{id}")]
        public IActionResult DeletePropertyType(int id)
        {
            var isDeleted = _propertyTypeRepository.DeletePropertyType(id);
            if (!isDeleted)
            {
                return NotFound("Property type not found.");
            }

            return NoContent();
        }
    }
}
