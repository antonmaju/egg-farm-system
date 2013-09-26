using EggFarmSystem.Models;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Services
{
    public interface IEmployeeService : IDataService<Employee>
    {
    }

    public class EmployeeService : DataService<Employee>, IEmployeeService
    {
        private IDbConnectionFactory factory;

        public EmployeeService(IDbConnectionFactory factory) : base(factory)
        {
            this.factory = factory;
        }

        public override IList<Employee> GetAll()
        {
            return base.GetAll().OrderBy(e => e.Name).ToList();
        }

    }
}
