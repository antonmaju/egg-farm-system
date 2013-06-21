using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using EggFarmSystem.Models.Data;
using EggFarmSystem.Services;
using ServiceStack.OrmLite;
using Xunit;

namespace EggFarmSystem.Core.Tests.Services
{
    public class ConsumableUsageServiceTests : IDisposable
    {
        private IConsumableUsageService service;
        private IDbConnectionFactory factory;

        public ConsumableUsageServiceTests()
        {
            factory = DatabaseTestInitializer.GetConnectionFactory();
            service = new ConsumableUsageService(factory);
        }

        [Fact]
        public void Can_SearchByDate()
        {
            Models.Data.ConsumableUsage usage1, usage2;

            using (var conn = factory.OpenDbConnection())
            {
                var id1 = Guid.NewGuid();
                var id2 = Guid.NewGuid();

                usage1 = new Models.Data.ConsumableUsage
                {
                    Id = id1,
                    Date = DateTime.Today.AddDays(-1),
                    Total = 10000
                };
                usage2 = new Models.Data.ConsumableUsage()
                {
                    Id = id2,
                    Date = DateTime.Today,
                    Total = 5000
                };

                using (var tx = conn.OpenTransaction())
                {
                    conn.InsertParam(usage1);
                    conn.InsertParam(usage2);
                    tx.Commit();
                }
            }

            var searchInfo = new ConsumableUsageSearchInfo
                {
                    Start = DateTime.Today.AddDays(-2),
                    End = DateTime.Today.AddDays(1),
                    PageIndex = 1,
                    PageSize = 1
                };

            var result = service.Search(searchInfo);

            Assert.Equal(2, result.Total);
            Assert.Equal(usage2.Id, result.Items[0].Id);


        }

        public void Dispose()
        {
            using (var conn = factory.OpenDbConnection())
            {
                conn.DeleteAll<Models.Data.ConsumableUsageDetail>();
                conn.DeleteAll<Models.Data.ConsumableUsage>();
            }
        }
    }
}
