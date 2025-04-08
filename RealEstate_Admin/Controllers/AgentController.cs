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
    public class AgentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AgentController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configuration["WebApiBaseUrl"])
            };
        }

        // 1. List All Agents
        public async Task<IActionResult> AgentList()
        {
            var response = await _httpClient.GetAsync("api/Agent");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var agents = JsonConvert.DeserializeObject<List<AgentModel>>(data);
                return View(agents);
            }
            return View(new List<RealEstate_Admin.Models.AgentModel>());
        }

        public async Task<IActionResult> AddAgent(int? AgentId)
        {
            if (AgentId.HasValue)
            {
                var response = await _httpClient.GetAsync($"api/Agent/{AgentId}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var agent = JsonConvert.DeserializeObject<AgentModel>(data);
                    return View(agent);
                }
            }

            return View(new AgentModel()); // If no AgentId, return an empty model for adding
        }

        // 3. Save or Update Agent
        [HttpPost]
        public async Task<IActionResult> SaveAgent(AgentModel agent, IFormFile ProfilePicture)
        {
            if (ProfilePicture != null)
            {
                try
                {
                    var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                    if (!Directory.Exists(imageFolder))
                    {
                        Directory.CreateDirectory(imageFolder);
                    }

                    // Use the original file name as provided by the user
                    var fileName = ProfilePicture.FileName;
                    var fullPath = Path.Combine(imageFolder, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await ProfilePicture.CopyToAsync(stream);
                    }

                    // Update the property model with image details
                    agent.ProfilePicture = fileName;
                    agent.ProfilePicturePath = $"/img/{fileName}";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Image Upload Error: {ex.Message}");
                    ModelState.AddModelError("ProfilePicture", "Failed to upload profile picture. Please try again.");
                    return View("AddAgent", agent);
                }
            }

            var json = JsonConvert.SerializeObject(agent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            if (agent.AgentId > 0)
            {
                response = await _httpClient.PutAsync($"api/Agent/{agent.AgentId}", content);
            }
            else
            {
                response = await _httpClient.PostAsync("api/Agent", content);
            }

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AgentList");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
            ModelState.AddModelError("", "Failed to save agent. Please try again.");
            return View("AddAgent", agent);
        }

        public IActionResult DeleteAgent(int AgentId)
        {
            var response = _httpClient.DeleteAsync($"api/Agent/{AgentId}").Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AgentList");
            }

            var errorContent = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
            return RedirectToAction("AgentList");
        }
    }
}
