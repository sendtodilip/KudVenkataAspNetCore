﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KudVenkataYoutube.Model
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int Id);
        IEnumerable<Employee> GetEmployees();
        Employee CreateEmployee(Employee employee);
        Employee UpdateEmployee(Employee newEmployee);
        Employee DeleteEmployee(int id);
    }
}
