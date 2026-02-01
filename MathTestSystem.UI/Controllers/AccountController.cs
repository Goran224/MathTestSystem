using MathTestSystem.Shared.DTOs;
using MathTestSystem.UI.Models;
using MathTestSystem.Domain.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IHttpClientFactory httpClientFactory, ILogger<AccountController> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
        _logger = logger;
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

        var responseContent = await response.Content.ReadAsStringAsync();

        string? rawToken = null;
        try
        {
            using var doc = JsonDocument.Parse(responseContent);
            if (doc.RootElement.TryGetProperty("Token", out var tok)) rawToken = tok.GetString();
            else if (doc.RootElement.TryGetProperty("token", out tok)) rawToken = tok.GetString();
        }
        catch (JsonException)
        {
            model.ErrorMessage = "Invalid response from auth server";
            return View(model);
        }

        if (string.IsNullOrWhiteSpace(rawToken))
        {
            model.ErrorMessage = "Authentication token missing";
            return View(model);
        }

        Response.Cookies.Append("AuthToken", rawToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", rawToken);

        var meResp = await _httpClient.GetAsync("api/auth/me");
        if (!meResp.IsSuccessStatusCode)
        {
            model.ErrorMessage = "Failed to retrieve user info";
            return View(model);
        }

        var meJson = await meResp.Content.ReadAsStringAsync();

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        jsonOptions.Converters.Add(new JsonStringEnumConverter());

        var user = JsonSerializer.Deserialize<UserDto>(meJson, jsonOptions);

        if (user == null)
        {
            model.ErrorMessage = "Failed to get user info";
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("UserId", user.Id.ToString())
        };

        if (!string.IsNullOrWhiteSpace(user.ExternalId))
        {
            claims.Add(new Claim("ExternalId", user.ExternalId));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = true }
        );

        return user.Role == UserRole.Teacher
            ? RedirectToAction("Index", "Teacher")
            : RedirectToAction("Index", "Student");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("AuthToken");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}