using System.Threading.Tasks;

namespace Employee.Services.Interface
{
    public interface IEmployeeService
    {
        Task<string> GetEmployee();
    }
}