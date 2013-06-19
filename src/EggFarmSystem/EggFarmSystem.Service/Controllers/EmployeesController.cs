using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using EggFarmSystem.Models;
using EggFarmSystem.Services;

namespace EggFarmSystem.Service.Controllers
{
    public class EmployeesController : ApiControllerBase<Employee>
    {
        private readonly IEmployeeService service;

        public EmployeesController(IEmployeeService employeeService) :base(employeeService)
        {
            this.service = employeeService;
        }   
    }
}
