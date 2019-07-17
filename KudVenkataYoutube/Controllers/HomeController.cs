using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KudVenkataYoutube.Model;
using KudVenkataYoutube.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KudVenkataYoutube.Controllers
{
    //[Route("[Controller]")]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeRepository, IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeRepository;
            this.hostingEnvironment = hostingEnvironment;
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
            throw new Exception("Error in Details view");
            Employee employee = _employeeRepository.GetEmployee(id.Value);
            if (employee==null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }
           
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,    
                PageTitle = "Employee Details"
            };
            return View(homeDetailsViewModel);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)  
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadFile(model);
                }
                _employeeRepository.UpdateEmployee(employee);
                return RedirectToAction("index");
            }
            return View();
        }

        private string ProcessUploadFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = "";
            if (model.Photo != null && model.Photo.Count > 0)
            {
                foreach (IFormFile photo in model.Photo)
                {
                    string folderPath = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(folderPath, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                }
            }

            return uniqueFileName;
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadFile(model);

                Employee newEmployee = new Employee
                {
                    Department = model.Department,
                    Email = model.Email,
                    Name = model.Name,
                    PhotoPath = uniqueFileName
                };
                _employeeRepository.CreateEmployee(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }
    }
}
