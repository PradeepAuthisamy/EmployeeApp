using Employee.Providers.Interface;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Providers
{
    public class JWTProvider : IJWTProvider
    {
        public async Task<string> GetToken(string userName)
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

                var tokenResponse = await authClient.PostAsync("http://jwt-authenticator:20/api/authenticator/authenticate", data);
                if (tokenResponse.IsSuccessStatusCode)
                {
                    token = tokenResponse.Content.ReadAsStringAsync().Result;
                }
                return token;
            }
        }
    }
}