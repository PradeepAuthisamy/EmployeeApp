using Employee.Providers.Interface;
using Employee.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Employee.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJWTProvider _jwtProvider;
        private readonly HttpContext _httpContext;
        private readonly HttpClient _httpClient;

        public EmployeeService(IHttpContextAccessor httpContextAccess,
                               UserManager<IdentityUser> userManager,
                               IJWTProvider jwtProvider)
        {
            _httpClient = new HttpClient();
            _httpContext = httpContextAccess.HttpContext;
            //var user = _httpContext.User;
            _userManager = userManager;
            _jwtProvider = jwtProvider;
            //var userName = user.Identity.Name;// _userManager.GetUserId(user.Identity.Name);
            //_httpClient.AddBearerToken(_userManager, userName);
        }

        public async Task<string> GetEmployee()
        {
            var user = _httpContext.User;
            var token = await _jwtProvider.GetToken(user.Identity.Name);
            if (string.IsNullOrEmpty(token)) return default;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("http://employeeinfo-api:30/api/Employee/GetEmployeeInfo");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return default;
        }
    }
}