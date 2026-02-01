using MathTestSystem.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

[Authorize(Roles = "Student")]
public class StudentController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StudentController> _logger;
    private readonly IConfiguration _configuration;

    public StudentController(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<StudentController> logger)
    {
        _logger = logger;
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            // Prefer ExternalId claim (mapped to Student.ExternalStudentId), fallback to UserId
            var externalStudentId = User.FindFirstValue("ExternalId") ?? User.FindFirstValue("UserId");
            if (string.IsNullOrWhiteSpace(externalStudentId))
            {
                _logger.LogWarning("Student user has no ExternalId or UserId claim");
                return Challenge();
            }

            var token = Request.Cookies["AuthToken"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync($"/api/exam/student/{externalStudentId}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to load exams for student {StudentId}. Status: {Status}",
                    externalStudentId, response.StatusCode);
                ViewBag.Error = response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                    ? "Not authorized to load exams."
                    : "Failed to load exams";
                return View(new List<ExamDto>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            // Ensure enum values serialized as strings ("Correct","Incorrect",...) deserialize correctly
            jsonOptions.Converters.Add(new JsonStringEnumConverter());

            var exams = JsonSerializer.Deserialize<List<ExamDto>>(json, jsonOptions)
                ?? new List<ExamDto>();

            return View(exams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error loading student dashboard");
            ViewBag.Error = "Something went wrong. Please try again.";
            return View(new List<ExamDto>());
        }
    }
}