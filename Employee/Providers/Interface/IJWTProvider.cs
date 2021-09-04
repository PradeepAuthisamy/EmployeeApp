using System.Threading.Tasks;

namespace Employee.Providers.Interface
{
    public interface IJWTProvider
    {
        Task<string> GetToken(string userName);
    }
}