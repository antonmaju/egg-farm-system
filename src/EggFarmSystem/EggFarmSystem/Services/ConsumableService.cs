using EggFarmSystem.Models;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Services
{
    public interface IConsumableService :  IDataService<Consumable>
    {
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
