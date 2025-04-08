using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RealEstate_Admin.Models;

namespace RealEstate_Admin.Controllers
{
    public class PropertyTypeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public PropertyTypeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        public async Task<IActionResult> PropertyTypeList()
        {
            var response = await _httpClient.GetAsync("api/PropertyType");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var propertyTypes = JsonConvert.DeserializeObject<List<PropertyTypeModel>>(data);
                return View(propertyTypes);
            }
            return View(new List<PropertyTypeModel>());
        }

        public async Task<IActionResult> AddPropertyType(int? PropertyTypeID)
        {
            if (PropertyTypeID.HasValue)
            {
                var response = await _httpClient.GetAsync($"api/PropertyType/{PropertyTypeID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var propertyType = JsonConvert.DeserializeObject<PropertyTypeModel>(data);
                    return View(propertyType);
                }
            }
            return View(new PropertyTypeModel());
        }

        [HttpPost]
        public async Task<IActionResult> SavePropertyType(PropertyTypeModel ptype)
        {
            if (!ModelState.IsValid)
            {
                return View("AddPropertyType", ptype);
            }
            var json = JsonConvert.SerializeObject(ptype);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            if (ptype.PropertyTypeId > 0)
            {
                response = await _httpClient.PutAsync($"api/PropertyType/{ptype.PropertyTypeId}", content);
            }
            else
            {
                response = await _httpClient.PostAsync("api/PropertyType", content);
            }

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("PropertyTypeList");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
            ModelState.AddModelError("", "Failed to save property type. Please try again.");
            return View("AddPropertyType", ptype);
        }

        public IActionResult DeletePropertyType(int PropertyTypeID)
        {
            var response = _httpClient.DeleteAsync($"api/PropertyType/{PropertyTypeID}").Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("PropertyTypeList");
            }

            var errorContent = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
            return RedirectToAction("PropertyTypeList");
        }
    }
}
