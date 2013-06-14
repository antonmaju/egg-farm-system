using EggFarmSystem.Models;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Services
{
    public interface IEmployeeService
    {
        IList<Employee> GetAll();

        Employee Get(Guid id);

        bool Save(Employee model);

        bool Delete(Guid id);
    }

    public class EmployeeService : DataService<Employee>, IEmployeeService
    {
        private IDbConnectionFactory factory;

        public EmployeeService(IDbConnectionFactory factory) : base(factory)
        {
            this.factory = factory;
        }

    }
}
