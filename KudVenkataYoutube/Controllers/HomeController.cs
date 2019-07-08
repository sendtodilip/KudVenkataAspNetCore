using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KudVenkataYoutube.Model;
using KudVenkataYoutube.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KudVenkataYoutube.Controllers
{
    //[Route("[Controller]")]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeRepository)
        {
            _employeeRepository = employeRepository;
        }
        //[Route("")]
        //[Route("[action]")]
        //[Route("~/")]
        public ViewResult Index()
        {
            var model = _employeeRepository.GetEmployees();
            return View(model);
        }
        //[Route("[action]/{id?}")]
        public ObjectResult Detail(int id = 1)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            return new ObjectResult(employee);
        }
        //[Route("[action]/{id?}")]
        public ViewResult Details(int? id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id ?? 1),
                PageTitle = "Employee Details"
            };
            return View(homeDetailsViewModel);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                Employee newEmployee = _employeeRepository.CreateEmployee(employee);
                //return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }
    }
}
