using EggFarmSystem.Models;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Services
{
    public interface IHenHouseService
    {
        SearchResult<HenHouse> Search();

        IList<HenHouse> GetAll();

        IList<HenHouse> GetAllActive();
            
        HenHouse Get(Guid id);

        bool Save(HenHouse model);

        bool Delete(Guid id);
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
