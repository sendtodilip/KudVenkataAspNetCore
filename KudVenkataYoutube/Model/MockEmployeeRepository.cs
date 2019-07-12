using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KudVenkataYoutube.Model
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _emplyeeList;

        public MockEmployeeRepository()
        {
            _emplyeeList = new List<Employee>()
            {
                new Employee(){ Id=1, Name="Mary", Department= Dept.HR, Email="marry@gmail.com"},
                new Employee(){ Id=2, Name="John", Department=Dept.IT, Email="john@gmail.com"},
                new Employee(){ Id=3, Name="sam", Department= Dept.IT, Email="sam@gmail.com"}
                };
        }

        public Employee CreateEmployee(Employee employee)
        {
            employee.Id = _emplyeeList.Select(x => x.Id).Max() + 1;
            _emplyeeList.Add(employee);
            return employee;
        }

        public Employee DeleteEmployee(int id)
        {
            Employee employee = _emplyeeList.Where(x => x.Id == id).FirstOrDefault();
            if (employee != null)
            {
                _emplyeeList.Remove(employee);

            }
            return employee;
        }

        public Employee GetEmployee(int Id)
        {
            return this._emplyeeList.FirstOrDefault(e => e.Id == Id);
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _emplyeeList;
        }

        public Employee UpdateEmployee(Employee newEmployee)
        {
            Employee employee = _emplyeeList.Where(x => x.Id == newEmployee.Id).FirstOrDefault();
            if (employee != null)
            {
                employee.Name = newEmployee.Name;
                employee.Department = newEmployee.Department;
                employee.Email = newEmployee.Email;
            }
            return employee;
        }
    }
}
