using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using RealEstate.Models;

public class ContactUsController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public ContactUsController(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient
        {
            BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
        };
    }

    public async Task<IActionResult> ContactUs(int? FeedbackID)
    {
        return await AddFeedback(FeedbackID);
    }

    public async Task<IActionResult> AddFeedback(int? FeedbackID)
    {
        ContactUsModel feedback = new ContactUsModel();

        if (FeedbackID.HasValue)
        {
            var response = await _httpClient.GetAsync($"api/ContactUs/{FeedbackID}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                feedback = JsonConvert.DeserializeObject<ContactUsModel>(data);
            }
        }
        return View("ContactUs", feedback);  // Ensure the correct view name
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> SaveFeedback(ContactUsModel feedback)
    {
        if (ModelState.IsValid)
        {
            var json = JsonConvert.SerializeObject(feedback);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("api/ContactUs", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Your feedback has been submitted successfully!";
                return RedirectToAction("ContactUs");
            }
        }
        return View("ContactUs", feedback);
    }

}
