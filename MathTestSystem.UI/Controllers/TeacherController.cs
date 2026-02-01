using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MathTestSystem.Shared.DTOs;
using System.Text.Json;

[Authorize(Roles = "Teacher")]
public class TeacherController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TeacherController> _logger;

    public TeacherController(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<TeacherController> logger)
    {
        _logger = logger;

        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(
            configuration["ApiSettings:BaseUrl"]!
        );
    }

    // GET: /Teacher
    public async Task<IActionResult> Index()
    {
        try
        {
            var teacherId = User.Claims.First(c => c.Type == "UserId").Value;

            var response = await _httpClient.GetAsync($"/api/exam/teacher/{teacherId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to load exams for teacher {TeacherId}", teacherId);
                ViewBag.Error = "Failed to load exams.";
                return View(new List<ExamDto>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var exams = JsonSerializer.Deserialize<List<ExamDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(exams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error loading teacher dashboard");
            ViewBag.Error = "Something went wrong. Please try again.";
            return View(new List<ExamDto>());
        }
    }

    // GET: /Teacher/UploadXml
    public IActionResult UploadXml()
    {
        return View();
    }

    // POST: /Teacher/UploadXml
    [HttpPost]
    public async Task<IActionResult> UploadXml(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select an XML file.");
                return View();
            }

            using var content = new MultipartFormDataContent();
            using var stream = file.OpenReadStream();

            content.Add(new StreamContent(stream), "file", file.FileName);

            var response = await _httpClient.PostAsync("/api/exam/upload", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("XML upload failed with status {StatusCode}", response.StatusCode);
                ModelState.AddModelError("", "Failed to upload XML.");
                return View();
            }

            TempData["Message"] = "XML uploaded successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while uploading XML");
            ModelState.AddModelError("", "Unexpected error while uploading file.");
            return View();
        }
    }
}
