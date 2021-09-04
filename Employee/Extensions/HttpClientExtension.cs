using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Extensions
{
    public static class HttpClientExtension
    {
        public static async Task AddBearerToken(this HttpClient client, UserManager<IdentityUser> userManager,
                                                string userName)
        {
            var token = string.Empty;
            using (var authClient = new HttpClient())
            {
                var cred = new
                {
                    username = userName
                };

                string json = JsonConvert.SerializeObject(cred);

                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var tokenResponse = await authClient.PostAsync("https://jwt-authenticator/api/authenticator/authenticate", data);
                if (tokenResponse.IsSuccessStatusCode)
                {
                    token = tokenResponse.Content.ReadAsStringAsync().Result;
                }
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}