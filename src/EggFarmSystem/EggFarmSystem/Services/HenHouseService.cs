using System.Data;
using EggFarmSystem.Models;
using MySql.Data.MySqlClient;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Services
{
    public interface IHenHouseService : IDataService<HenHouse>
    {
        int GetPopulation(Guid houseId);
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

        public override IList<HenHouse> GetAll()
        {
            return base.GetAll().OrderBy(h => h.Name).ToList();
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

        public int GetPopulation(Guid houseId)
        {
            int population = 0;

            using (var conn = factory.OpenDbConnection())
            {
                var command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select count(*) from Hen where HouseId=@houseId";
                command.Parameters.Add(new MySqlParameter("@houseId", MySqlDbType.Guid) {Value = houseId});
                
                var result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    population = Convert.ToInt32(result);                
            }

            return population;
        }
    }
}
