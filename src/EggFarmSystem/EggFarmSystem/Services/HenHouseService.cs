using EggFarmSystem.Models;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Services
{
    public interface IHenHouseService : IDataService<HenHouse>
    {
    }

    public class HenHouseService : DataService<HenHouse>, IHenHouseService
    {
        IDbConnectionFactory factory;

        public HenHouseService(IDbConnectionFactory factory) : base(factory)
        {
            this.factory = factory;
        }

        public SearchResult<HenHouse> Search()
        {
            throw new NotImplementedException();
        }

        public IList<HenHouse> GetAllActive()
        {
            using (var db = factory.CreateDbConnection())
            {
                db.Open();
                return db.Where<HenHouse>(new {Active = true})
                  .OrderBy(h => h.Name)
                  .ToList();

            }
        }
    }
}
