using Employee.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Employee.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            string token = GetToken();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                                new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = client.GetAsync("https://localhost:44303/api/Employee/GetEmployeeInfo").Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var model = JsonConvert.DeserializeObject<List<EmployeeInfoModel>>(result);
                    var viewModel = new Employees
                    {
                        EmployeesInfo = model
                    };
                    return View(model);
                }
            }
            return View();
        }

        private string GetToken()
        {
            var token = string.Empty;
            using (var authClient = new HttpClient())
            {
                var credential = new
                {
                    username = _userManager.GetUserId(User)
                };

                string json = JsonConvert.SerializeObject(credential);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                var tokenResponse = authClient.PostAsync("http://jwt-authenticator/api/authenticator/authenticate", data).Result;
                if (tokenResponse.IsSuccessStatusCode)
                {
                    token = tokenResponse.Content.ReadAsStringAsync().Result;
                }
            }
            return token;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}