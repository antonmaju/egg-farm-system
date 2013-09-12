using EggFarmSystem.Services;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace EggFarmSystem.Core.Tests.Services
{
    public class HenDepreciationServiceTests : IDisposable
    {
        private IHenDepreciationService service;
        private IDbConnectionFactory factory;

        public HenDepreciationServiceTests()
        {
            factory = DatabaseTestInitializer.GetConnectionFactory();
            service = new HenDepreciationService(factory);
        }

        public void Dispose()
        {

        }
    }
}
