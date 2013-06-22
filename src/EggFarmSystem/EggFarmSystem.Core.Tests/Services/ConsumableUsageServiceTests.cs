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
using ConsumableUsageDetail = EggFarmSystem.Models.ConsumableUsageDetail;

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

        [Fact]
        public void Can_GetById()
        {
            Models.Data.ConsumableUsage usage1;
            var id1 = Guid.NewGuid();
            using (var conn = factory.OpenDbConnection())
            {
                usage1 = new Models.Data.ConsumableUsage
                {
                    Id = id1,
                    Date = DateTime.Today.AddDays(-1),
                    Total = 10000
                };

                using (var tx = conn.OpenTransaction())
                {
                    conn.InsertParam(usage1);
                    tx.Commit();
                }
            }

            var usage = service.Get(id1);
            Compare(usage1, null, usage);
        }

        [Fact]
        public void Can_GetByDate()
        {
            Models.Data.ConsumableUsage usage1 = null;
            using (var conn = factory.OpenDbConnection())
            {
                usage1 = new Models.Data.ConsumableUsage
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Today.AddDays(-1),
                    Total = 10000
                };

                using (var tx = conn.OpenTransaction())
                {
                    conn.InsertParam(usage1);
                    tx.Commit();
                }
            }

            var usage = service.GetByDate(usage1.Date);
            Compare(usage1, null, usage);
        }

        void Compare(Models.Data.ConsumableUsage data, IList<Models.Data.ConsumableUsageDetail> details, Models.ConsumableUsage model)
        {
            Assert.Equal(data.Id, model.Id);
            Assert.Equal(data.Total, model.Total);
            Assert.Equal(data.Date, model.Date);

            if(details != null)
            {
                foreach (var detail in details)
                {
                    var detailModel =
                        model.Details.FirstOrDefault(
                            d => d.ConsumableId == detail.ConsumableId && d.HouseId == detail.HouseId);
                    Assert.Equal(detail.ConsumableId, detailModel.ConsumableId);
                    Assert.Equal(detail.Count, detailModel.Count);
                    Assert.Equal(detail.HouseId, detailModel.HouseId);
                    Assert.Equal(detail.SubTotal, detailModel.SubTotal);
                    Assert.Equal(detail.UnitPrice, detailModel.UnitPrice);
                }
            }
        }

        [Fact]
        public void Can_SaveNewConsumable()
        {
            var consumable = new Consumable
                {
                    Id = Guid.NewGuid(),
                    Name = "Zat 1",
                    Type = (byte) ConsumableType.Feed,
                    Unit = "liter",
                    UnitPrice = 10000
                };

            var house = new HenHouse
                {
                    Id = Guid.NewGuid(),
                    Name = "House 1",
                    Depreciation = 100,
                    Active = true,
                    PurchaseCost = 100000
                };

            using (var conn = factory.OpenDbConnection())
            {
                conn.InsertParam(house);
                conn.InsertParam(consumable);
            }

            var usage = new Models.ConsumableUsage
                {
                    Total = 10000,
                    Date = DateTime.Today,
                    Details = new List<ConsumableUsageDetail>
                        {
                            new ConsumableUsageDetail
                                {
                                    ConsumableId = consumable.Id,
                                    Count = 10,
                                    HouseId = house.Id,
                                    SubTotal = 10000,
                                    UnitPrice = 100000
                                }
                        }
                };

            service.Save(usage);

            using (var conn = factory.OpenDbConnection())
            {
                var usageData = conn.FirstOrDefault<Models.Data.ConsumableUsage>(c => c.Date == usage.Date);
                var details = conn.Where<Models.Data.ConsumableUsageDetail>(new {UsageId = usageData.Id});
                Compare(usageData, details, usage);
            }
        }

        [Fact]
        public void Can_SaveExistingConsumable()
        {
            var consumable = new Consumable
            {
                Id = Guid.NewGuid(),
                Name = "Zat 1",
                Type = (byte)ConsumableType.Feed,
                Unit = "liter",
                UnitPrice = 10000
            };

            var house = new HenHouse
            {
                Id = Guid.NewGuid(),
                Name = "House 1",
                Depreciation = 100,
                Active = true,
                PurchaseCost = 100000
            };

            var usage = new Models.Data.ConsumableUsage
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Today.AddDays(-1),
                Total = 10000
            };

            using (var conn = factory.OpenDbConnection())
            {
                conn.InsertParam(house);
                conn.InsertParam(consumable);
                conn.InsertParam(usage);
            }

            var model = new Models.ConsumableUsage
            {
                Id = usage.Id,
                Total = 12000,
                Date = DateTime.Today,
                Details = new List<ConsumableUsageDetail>
                        {
                            new ConsumableUsageDetail
                                {
                                    ConsumableId = consumable.Id,
                                    Count = 10,
                                    HouseId = house.Id,
                                    SubTotal = 10000,
                                    UnitPrice = 100000
                                }
                        }
            };

            service.Save(model);

            using (var conn = factory.OpenDbConnection())
            {
                var usageData = conn.GetById<Models.Data.ConsumableUsage>(usage.Id.ToString());
                var details = conn.Where<Models.Data.ConsumableUsageDetail>(new { UsageId = usageData.Id });
                Compare(usageData, details, model);
            }
        }

        [Fact]
        public void Can_DeleteById()
        {
           var usage = new Models.Data.ConsumableUsage
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Today.AddDays(-1),
                Total = 10000
            };

            using (var conn = factory.OpenDbConnection())
            {
                conn.InsertParam(usage);
            }

            service.Delete(usage.Id);

            using (var conn = factory.OpenDbConnection())
            {
                var usageData = conn.GetByIdOrDefault<Models.Data.ConsumableUsage>(usage.Id);
                Assert.Null(usageData);
            }
        }

        public void Dispose()
        {
            using (var conn = factory.OpenDbConnection())
            {
                conn.DeleteAll<Models.Consumable>();
                conn.DeleteAll<Models.HenHouse>();
                conn.DeleteAll<Models.Data.ConsumableUsageDetail>();
                conn.DeleteAll<Models.Data.ConsumableUsage>();
            }
        }
    }
}
