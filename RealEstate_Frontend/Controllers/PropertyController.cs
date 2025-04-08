using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace RealEstate.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public PropertyController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        public async Task<IActionResult> PropertyList()
        {
            var response = await _httpClient.GetAsync("api/property");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                // Deserialize into your custom PropertyModel
                var properties = JsonConvert.DeserializeObject<List<RealEstate.Models.PropertyModel>>(data);
                return View(properties);
            }
            return View(new List<RealEstate.Models.PropertyModel>());
        }

        public async Task<IActionResult> AllProperty()
        {
            var response = await _httpClient.GetAsync("api/property");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                // Deserialize into your custom PropertyModel
                var properties = JsonConvert.DeserializeObject<List<RealEstate.Models.PropertyModel>>(data);
                return View(properties);
            }
            return View(new List<RealEstate.Models.PropertyModel>());
        }

        public async Task<IActionResult> PropertyDetails(int id)
        {
            var response = await _httpClient.GetAsync($"api/property/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                // Deserialize into your custom PropertyModel
                var property = JsonConvert.DeserializeObject<RealEstate.Models.PropertyModel>(data);
                return View(property);
            }
            return NotFound("Property not found.");
        }

        public async Task<IActionResult> PropertyCategory(string category, string viewName = "PropertyList")
        {
            var response = await _httpClient.GetAsync($"api/Property/category/{category}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var propertyCategory = JsonConvert.DeserializeObject<List<RealEstate.Models.PropertyModel>>(data);

                return View(viewName, propertyCategory); // Dynamically return the correct view
            }

            return View(viewName, new List<RealEstate.Models.PropertyModel>()); // Empty list if no properties are found
        }



        public async Task<IActionResult> PropertyType(string propertyType)
        {
            //ViewBag.SelectedPropertyType = propertyType;

            var response = await _httpClient.GetAsync($"api/property/propertytype/{propertyType}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var propertyTypeList = JsonConvert.DeserializeObject<List<RealEstate.Models.PropertyModel>>(data); // Note the List here
                return View("PropertyList", propertyTypeList); // Reuse the PropertyList view
            }
            return View("PropertyList", new List<RealEstate.Models.PropertyModel>()); // Empty list if no properties are found
        }


        public IActionResult PropertyAgent()
        {
            return View();
        }
    }
}
