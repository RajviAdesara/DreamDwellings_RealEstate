using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace RealEstate_Admin.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly HttpClient _httpClient;

        public FeedbackController(IConfiguration _configuration)
        {
            configuration = _configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }
        #region FeedbackList
        public async Task<IActionResult> FeedbackList()
        {
            var response = await _httpClient.GetAsync("api/ContactUs");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var feedbacks = JsonConvert.DeserializeObject<List<RealEstate_Admin.Models.FeedbackModel>>(data);
                return View(feedbacks);
            }
            return View(new List<RealEstate_Admin.Models.FeedbackModel>());
        }
        #endregion

    }
}
