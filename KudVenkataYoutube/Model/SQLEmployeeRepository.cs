using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KudVenkataYoutube.Model
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;

        public SQLEmployeeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Employee CreateEmployee(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee DeleteEmployee(int id)
        {
            Employee employee = context.Employees.Find(id);
            if (employee!=null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            return employee;
        }

        public Employee GetEmployee(int Id)
        {
            return context.Employees.Find(Id);
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return context.Employees; 
        }

        public Employee UpdateEmployee(Employee newEmployee)
        {
            var employee = context.Employees.Attach(newEmployee);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return newEmployee;
        }
    }
}
