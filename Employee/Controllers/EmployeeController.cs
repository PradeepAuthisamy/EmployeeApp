using Employee.Models;
using Employee.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<HomeController> logger,
                                  IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetEmployee();
            var employeesInfoModel = JsonConvert.DeserializeObject<List<EmployeeInfoModel>>(employees);
            var viewModel = new Employees
            {
                EmployeesInfo = employeesInfoModel
            };
            viewModel.UserName = User.Identity.Name;
            return View(viewModel);
        }
    }
}