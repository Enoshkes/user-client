using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using user_client.Dto;
using user_client.Models;
using user_client.Utils;
using user_client.ViewModel;

namespace user_client.Controllers
{
    public class UserClientController(IHttpClientFactory clientFactory, Authentication auth) : Controller
    {
        private readonly string baseUrl = "https://localhost:7168/api/User";

        public async Task<IActionResult> Enosh()
        {
            var client = clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/enosh");

            request.Headers.Authorization = 
                new AuthenticationHeaderValue("Bearer", auth.Token);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return View();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            var result = await httpClient.GetAsync(baseUrl);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<UserDto>? users = JsonSerializer.Deserialize<List<UserDto>>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );
                return View(users);

            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Create()
        {
            return View(new UserVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserVM user)
        {
            if (user.Image == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var httpClient = clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/create");

            var httpContent = new StringContent(
                JsonSerializer.Serialize(new {
                    user.Email,
                    user.Password,
                    user.Name,
                    Image = ImageUtils.ConvertFromIFormFile(user.Image)
                }),
                Encoding.UTF8,
                "application/json"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", auth.Token);

            request.Content = httpContent;

            var result = await httpClient.SendAsync(request);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var httpClient = clientFactory.CreateClient();

            var httpContent = new StringContent(
               JsonSerializer.Serialize(new { Email, Password }),
               Encoding.UTF8,
               "application/json"
           );

            var result = await httpClient.PostAsync($"{baseUrl}/auth", httpContent);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                auth.Token = content;
                return RedirectToAction("Index");
            }
            return View("AuthError");
        }
    }
}
