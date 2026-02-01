using MathTestSystem.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;

    public AccountController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7254"); // API base URL
    }

    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var content = new StringContent(
            JsonSerializer.Serialize(new { username = model.Username, password = model.Password }),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync("api/auth/login", content);

        if (!response.IsSuccessStatusCode)
        {
            model.ErrorMessage = "Invalid username or password";
            return View(model);
        }

        var token = await response.Content.ReadAsStringAsync();

        // Store token in cookie
        Response.Cookies.Append("AuthToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        // Optionally decode JWT to get role or call API to get user info
        var role = await GetUserRole(token);

        return role == "Teacher"
            ? RedirectToAction("TeacherDashboard", "Teacher")
            : RedirectToAction("StudentDashboard", "Student");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        return RedirectToAction("Login");
    }
    private async Task<string> GetUserRole(string tokenJson)
    {
        // Parse JSON and extract token
        using var doc = JsonDocument.Parse(tokenJson);
        var token = doc.RootElement.GetProperty("token").GetString();

        if (string.IsNullOrWhiteSpace(token))
            throw new Exception("JWT token is missing");

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync("https://localhost:7254/api/auth/me");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var responseDoc = JsonDocument.Parse(json);

        return responseDoc.RootElement.GetProperty("role").GetString()!;
    }
}
