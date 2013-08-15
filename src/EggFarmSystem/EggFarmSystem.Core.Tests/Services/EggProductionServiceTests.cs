using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using EggFarmSystem.Services;
using ServiceStack.OrmLite;
using Xunit;

namespace EggFarmSystem.Core.Tests.Services
{
    public class EggProductionServiceTests : IDisposable
    {
        private IDbConnectionFactory factory;
        private IEggProductionService service;

        public EggProductionServiceTests()
        {
            factory = DatabaseTestInitializer.GetConnectionFactory();
            service = new EggProductionService(factory);
        }

        [Fact]
        public void Can_SearchByDate()
        {
            var production1 = new Models.Data.EggProduction
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Today,
                };

            var production2 = new Models.Data.EggProduction
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Today.AddDays(-3)
                };

            using (var conn = factory.OpenDbConnection())
            {
                conn.InsertParam(production1);
                conn.InsertParam(production2);
            }

            var result = service.Search(new DateRangeSearchInfo
            {
                Start = DateTime.Today.AddDays(-1),
                End = DateTime.Today.AddDays(10),
                PageIndex = 1,
                PageSize = 10
            });

            Assert.Equal(1, result.Total);
            Compare(production1, null, result.Items[0]);
        }

        [Fact]
        public void Can_GetById()
        {
            var id = Guid.NewGuid();

            var house = new Models.HenHouse
                {
                    Id = Guid.NewGuid(),
                    Name = "House 1",
                    PurchaseCost = 1,
                    Depreciation = 1,
                    YearUsage = 1,
                    Active = true
                };

            
            var data = new Models.Data.EggProduction
                {
                    Id=id,
                    Date=DateTime.Today
                };
            var detail = new Models.Data.EggProductionDetail
                {
                    CrackedEggCount = 1,
                    Fcr = 1,
                    GoodEggCount = 1,
                    HouseId = house.Id,
                    ProductionId = id,
                    RetailQuantity = 1.2m
                };

            using (var conn = factory.OpenDbConnection())
            {
                conn.InsertParam(house);
                conn.InsertParam(data);
                conn.InsertParam(detail);
            }

            var production = service.Get(id);
            Compare(data, null, production);
        }

        void Compare(Models.Data.EggProduction productionData, List<Models.Data.EggProductionDetail> details, Models.EggProduction production)
        {
            Assert.Equal(productionData.Id, production.Id);
            Assert.Equal(productionData.Date, production.Date);

            if (details != null)
            {
                Assert.Equal(details.Count, production.Details.Count);

                foreach (var detailData in details)
                {
                    var detail = production.Details.FirstOrDefault(d => d.HouseId == detailData.HouseId);

                    Assert.Equal(detailData.CrackedEggCount, detail.CrackedEggCount);
                    Assert.Equal(detailData.Fcr, detail.Fcr);
                    Assert.Equal(detailData.GoodEggCount, detail.CrackedEggCount);
                    Assert.Equal(detailData.HouseId, detail.HouseId);
                    Assert.Equal(detailData.RetailQuantity, detail.RetailQuantity);
                }
            }
        }

        public void Dispose()
        {
            using (var conn = factory.OpenDbConnection())
            {
                conn.DeleteAll<Models.Data.EggProductionDetail>();
                conn.DeleteAll<Models.Data.EggProduction>();
                conn.DeleteAll<Models.HenHouse>();
            }
        }
    }
}
