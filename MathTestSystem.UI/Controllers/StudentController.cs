using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MathTestSystem.Shared.DTOs;
using System.Text.Json;

[Authorize(Roles = "Student")]
public class StudentController : Controller
{
    private readonly HttpClient _httpClient;

    public StudentController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:5001");
    }

    public async Task<IActionResult> Index()
    {
        var studentId = User.Claims.First(c => c.Type == "UserId").Value;

        var response = await _httpClient.GetAsync($"/api/exams/bystudent/{studentId}");
        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Failed to load exams";
            return View(new List<ExamDto>());
        }

        var json = await response.Content.ReadAsStringAsync();
        var exams = JsonSerializer.Deserialize<List<ExamDto>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return View(exams);
    }
}
