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

        //public async Task<IActionResult> PropertyList()
        //{
        //    var response = await _httpClient.GetAsync("api/Property");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var data = await response.Content.ReadAsStringAsync();
        //        // Deserialize into your custom PropertyModel
        //        var properties = JsonConvert.DeserializeObject<List<RealEstate_Admin.Models.PropertyModel>>(data);
        //        return View(properties);
        //    }
        //    return View(new List<RealEstate_Admin.Models.PropertyModel>());
        //}


        public async Task<IActionResult> PropertyList(int? minPrice, int? maxPrice, int? bedrooms, int? bathrooms)
        {
            List<PropertyModel> properties = new List<PropertyModel>();

            try
            {
                // Always fetch from API, pass filters as query params
                var queryParams = $"?minPrice={minPrice}&maxPrice={maxPrice}&bedrooms={bedrooms}&bathrooms={bathrooms}";
                var response = await _httpClient.GetAsync($"api/Property{queryParams}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    properties = JsonConvert.DeserializeObject<List<PropertyModel>>(data);
                }
                else
                {
                    TempData["errorMessage"] = "Failed to load properties.";
                }
            }
            catch
            {
                TempData["errorMessage"] = "An error occurred while fetching properties.";
            }

            return View(properties);
        }


        public async Task<IActionResult> AddProperty(int? PropertyID)
        {
            if (PropertyID.HasValue)
            {
                // Fetch the property details for editing
                var response = await _httpClient.GetAsync($"api/Property/{PropertyID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var property = JsonConvert.DeserializeObject<PropertyModel>(data);
                    return View(property); // Return the populated property model to the view for editing
                }
            }

            // If no PropertyID is provided, return a new instance for adding a property
            return View(new PropertyModel());
        }

        [HttpPost]
        public async Task<IActionResult> SaveProperty(PropertyModel property, IFormFile ImagePath)
        {
            if (ImagePath != null)
            {
                try
                {
                    // Define the upload folder path
                    var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                    if (!Directory.Exists(imageFolder))
                    {
                        Directory.CreateDirectory(imageFolder);
                    }

                    // Use the original file name as provided by the user
                    var fileName = ImagePath.FileName;
                    var fullPath = Path.Combine(imageFolder, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await ImagePath.CopyToAsync(stream);
                    }

                    // Update the property model with image details
                    property.ImageName = fileName;
                    property.ImagePath = $"/img/{fileName}";
                }
                catch (Exception ex)
                {
                    // Log the error
                    Console.WriteLine($"Image Upload Error: {ex.Message}");
                    ModelState.AddModelError("ImagePath", "Failed to upload image. Please try again.");
                    return View("AddProperty", property);
                }
            }


            // Serialize the property data
            var json = JsonConvert.SerializeObject(property);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            if (property.PropertyId > 0)
            {
                // Update existing property
                response = await _httpClient.PutAsync($"api/Property/{property.PropertyId}", content);
            }
            else
            {
                // Add new property
                response = await _httpClient.PostAsync("api/Property", content);
            }

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("PropertyList"); // Redirect to PropertyList
            }

            // Handle API failure
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
            ModelState.AddModelError("", "Failed to save property. Please try again.");
            return View("AddProperty", property); // Reload the form with current data
        }

        public IActionResult DeleteProperty(int PropertyID)
        {
            var response = _httpClient.DeleteAsync($"api/Property/{PropertyID}").Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("PropertyList");
            }
            // Handle API failure
            var errorContent = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
            return RedirectToAction("PropertyList");
        }

    }
}
