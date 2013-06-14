using EggFarmSystem.Models;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Services
{
    public interface IConsumableService
    {
        IList<Consumable> GetAll();

        Consumable Get(Guid id);

        bool Save(Consumable model);

        bool Delete(Guid id);
    }

    public class ConsumableService : DataService<Consumable>, IConsumableService
    {
        private readonly IDbConnectionFactory factory;

        public ConsumableService(IDbConnectionFactory factory) : base(factory)
        {
            this.factory = factory;
        }
    }
}
