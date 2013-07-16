using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;

namespace EggFarmSystem.Services
{
    public interface IEmployeeCostService
    {
        SearchResult<EmployeeCost> Search(DateRangeSearchInfo searchInfo);

        EmployeeCost Get(Guid id);

        EmployeeCost GetByDate(DateTime date);

        void Save(EmployeeCost cost);

        void Delete(Guid id);
    }

    public class EmployeeCostService : IEmployeeCostService
    {
        public SearchResult<EmployeeCost> Search(DateRangeSearchInfo searchInfo)
        {
            throw new NotImplementedException();
        }

        public EmployeeCost Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public EmployeeCost GetByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public void Save(EmployeeCost cost)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
