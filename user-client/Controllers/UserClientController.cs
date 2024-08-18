using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using user_client.Dto;
using user_client.Utils;
using user_client.ViewModel;

namespace user_client.Controllers
{
    public class UserClientController(IHttpClientFactory clientFactory) : Controller
    {
        private readonly string baseUrl = "https://localhost:7168/api/User";
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
            var result = await httpClient.PostAsync($"{baseUrl}/create", httpContent);
            if (result.IsSuccessStatusCode)
            {
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
        public IActionResult Login(string Email, string Password)
        {
            bool isAuth = false;

            var httpContent = new StringContent(
               JsonSerializer.Serialize(new { Email, Password }),
               Encoding.UTF8,
               "application/json"
           );
            if (!isAuth)
            {
                return View("AuthError");
            }
            return RedirectToAction("Index");
        }
    }
}
