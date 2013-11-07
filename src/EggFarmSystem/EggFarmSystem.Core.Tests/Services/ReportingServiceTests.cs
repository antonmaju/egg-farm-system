using EggFarmSystem.Models;
using EggFarmSystem.Models.Data;
using EggFarmSystem.Services;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ConsumableUsage = EggFarmSystem.Models.ConsumableUsage;
using ConsumableUsageDetail = EggFarmSystem.Models.Data.ConsumableUsageDetail;

namespace EggFarmSystem.Core.Tests.Services
{
    public class ReportingServiceTests : IDisposable
    {
        private IReportingService service;
        private IDbConnectionFactory factory;

        public ReportingServiceTests()
        {
            factory = DatabaseTestInitializer.GetConnectionFactory();
            service = new ReportingService(factory);
        }

        [Fact]
        public void Can_GetEmployeeCostSummary()
        {
            var employees = new List<Employee>
                {
                    new Employee
                        {
                            Active = true,
                            Id = Guid.NewGuid(),
                            Name = "Employee #1",
                            Salary = 30000
                        },
                    new Employee
                        {
                            Active = true,
                            Id = Guid.NewGuid(),
                            Name = "Employee #2",
                            Salary = 25000
                        }
                };

            var costList = new List<Models.Data.EmployeeCost>
                {
                    new Models.Data.EmployeeCost
                        {
                            Id = Guid.NewGuid(),
                            Date = DateTime.Today.AddDays(-1),
                            Total = 55000
                        },
                    new Models.Data.EmployeeCost
                        {
                              Id = Guid.NewGuid(),
                            Date = DateTime.Today,
                            Total = 27000
                        }
                };

            var detailList = new List<Models.Data.EmployeeCostDetail>
                {
                    new Models.Data.EmployeeCostDetail
                        {
                            CostId = costList[0].Id,
                            EmployeeId = employees[0].Id,
                            Present = true,
                            Salary = employees[0].Salary
                        },
                    new Models.Data.EmployeeCostDetail
                        {
                            CostId = costList[0].Id,
                            EmployeeId = employees[1].Id,
                            Present = true,
                            Salary = employees[1].Salary
                        },
                    new Models.Data.EmployeeCostDetail
                        {
                            CostId = costList[1].Id,
                            EmployeeId = employees[1].Id,
                            Present = true,
                            Salary = 27000
                        }
                };
            using (var conn = factory.OpenDbConnection())
            {
                foreach (var employee in employees)
                {
                    conn.InsertParam(employee);
                }

                foreach (var employeeCost in costList)
                {
                    conn.InsertParam(employeeCost);
                }

                foreach (var employeeCostDetail in detailList)
                {
                    conn.InsertParam(employeeCostDetail);
                }
            }

            var list = service.GetEmployeeCostSummary(DateTime.Today.AddDays(-1), DateTime.Today);

            foreach (var item in list)
            {
                if (item.Id == employees[0].Id)
                {
                    Assert.Equal(1, item.Days);
                    Assert.Equal(30000, item.TotalSalary);
                }
                else
                {
                    Assert.Equal(2, item.Days);
                    Assert.Equal(52000, item.TotalSalary);
                }
            }
        }

        [Fact]
        public void Can_GetEggProductionReport()
        {
            var houses  = new List<HenHouse>
                {
                    new HenHouse
                        {
                           Id = Guid.NewGuid(),
                           Active = true,
                           Depreciation = 120,
                           Name = "House 1",
                           ProductiveAge = 90,
                           PurchaseCost = 120,
                           Weight = 120,
                           YearUsage = 5
                        },
                    new HenHouse
                        {
                           Id = Guid.NewGuid(),
                           Active = true,
                           Depreciation = 100,
                           Name = "House 2",
                           ProductiveAge = 80,
                           PurchaseCost = 100,
                           Weight = 190,
                           YearUsage = 3
                        }
                };

            var productionList = new List<Models.Data.EggProduction>
                {
                    new Models.Data.EggProduction
                        {
                            Id = Guid.NewGuid(),
                            Date = DateTime.Today.AddDays(-1)
                        },
                    new Models.Data.EggProduction
                        {
                            Id = Guid.NewGuid(),
                            Date = DateTime.Today
                        }
                };

            var detailList = new List<Models.Data.EggProductionDetail>
                {
                    new Models.Data.EggProductionDetail
                        {
                            CrackedEggCount = 1,
                            Fcr = 1,
                            GoodEggCount = 12,
                            HouseId = houses[0].Id,
                            ProductionId = productionList[0].Id,
                            RetailQuantity = 100
                        },
                    new Models.Data.EggProductionDetail
                        {
                            CrackedEggCount = 2,
                            Fcr = 1,
                            GoodEggCount = 12,
                            HouseId = houses[1].Id,
                            ProductionId = productionList[0].Id,
                            RetailQuantity = 200
                        },
                    new Models.Data.EggProductionDetail
                        {
                            CrackedEggCount = 1,
                            Fcr = 1,
                            GoodEggCount = 12,
                            HouseId = houses[0].Id,
                            ProductionId = productionList[1].Id,
                            RetailQuantity = 100
                        },
                    new Models.Data.EggProductionDetail
                        {
                            CrackedEggCount = 2,
                            Fcr = 1,
                            GoodEggCount = 12,
                            HouseId = houses[1].Id,
                            ProductionId = productionList[1].Id,
                            RetailQuantity = 200
                        },
                };



            using (var conn = factory.OpenDbConnection())
            {
                foreach (var house in houses)
                {
                    conn.InsertParam(house);
                }

                foreach (var production in productionList)
                {
                    conn.InsertParam(production);
                }

                foreach (var detail in detailList)
                {
                    conn.InsertParam(detail);
                }
            }

            var list = service.GetEggProductionReport(DateTime.Today.AddDays(-1), DateTime.Today);

            foreach (var item in list)
            {
                var production = productionList.First(p => p.Id == item.Id);

                Assert.Equal(production.Date, item.Date);

                foreach (var detail in item.Details)
                {
                    var detailData =detailList.First(d => d.ProductionId == item.Id && d.HouseId == detail.House.Id);

                    Assert.Equal(detailData.RetailQuantity, detail.RetailQuantity);
                    Assert.Equal(detailData.GoodEggCount, detail.GoodEggCount);
                    Assert.Equal(detailData.CrackedEggCount, detail.CrackedEggCount);
                    Assert.Equal(detailData.Fcr, detail.Fcr);
                }
            }
        }

        [Fact]
        public void Can_GetUsageSummaryReport()
        {
            var houses = new List<HenHouse>
                {
                    new HenHouse
                        {
                           Id = Guid.NewGuid(),
                           Active = true,
                           Depreciation = 120,
                           Name = "House 1",
                           ProductiveAge = 90,
                           PurchaseCost = 120,
                           Weight = 120,
                           YearUsage = 5
                        },
                    new HenHouse
                        {
                           Id = Guid.NewGuid(),
                           Active = true,
                           Depreciation = 100,
                           Name = "House 2",
                           ProductiveAge = 80,
                           PurchaseCost = 100,
                           Weight = 190,
                           YearUsage = 3
                        }
                };

            var consumables = new List<Consumable>
                {
                    new Consumable
                        {
                            Id = Guid.NewGuid(),
                            Active = true,
                            Name = "consumable #1",
                            Type = 0,
                            Unit = "kg",
                            UnitPrice = 30000
                        },
                    new Consumable
                        {
                            Id = Guid.NewGuid(),
                            Active = true,
                            Name = "consumable #2",
                            Type = 0,
                            Unit = "kg",
                            UnitPrice = 40000
                        },
                    new Consumable
                        {
                            Id = Guid.NewGuid(),
                            Active = true,
                            Name = "consumable #3",
                            Type = 1,
                            Unit = "kg",
                            UnitPrice = 50000
                        },
                    new Consumable
                        {
                            Id = Guid.NewGuid(),
                            Active = true,
                            Name = "consumable #4",
                            Type = 1,
                            Unit = "kg",
                            UnitPrice = 60000
                        }

                };
            var consumableUsageList = new List<Models.Data.ConsumableUsage>
                {
                    new Models.Data.ConsumableUsage
                        {
                            Id = Guid.NewGuid(),
                            Date = DateTime.Today.AddDays(-1),
                            Total = 70000
                        },
                    new Models.Data.ConsumableUsage
                        {
                            Id = Guid.NewGuid(),
                            Date = DateTime.Today,
                            Total = 110000
                        }
                };

            var detailList = new List<Models.Data.ConsumableUsageDetail>
                {
                    new Models.Data.ConsumableUsageDetail
                        {
                            ConsumableId = consumables[0].Id,
                            Count = 1,
                            UnitPrice = consumables[0].UnitPrice,
                            HouseId = houses[0].Id,
                            SubTotal = 30000,
                            UsageId = consumableUsageList[0].Id
                            
                        },
                    new Models.Data.ConsumableUsageDetail
                        {
                            ConsumableId = consumables[1].Id,
                            Count = 1,
                            UnitPrice = consumables[1].UnitPrice,
                            HouseId = houses[1].Id,
                            SubTotal = 40000,
                            UsageId = consumableUsageList[0].Id
                            
                        },
                    new Models.Data.ConsumableUsageDetail
                        {
                            ConsumableId = consumables[2].Id,
                            Count = 1,
                            UnitPrice = consumables[2].UnitPrice,
                            HouseId = houses[0].Id,
                            SubTotal = 50000,
                            UsageId = consumableUsageList[1].Id
                            
                        },
                    new Models.Data.ConsumableUsageDetail
                        {
                            ConsumableId = consumables[3].Id,
                            Count = 1,
                            UnitPrice = consumables[3].UnitPrice,
                            HouseId = houses[0].Id,
                            SubTotal = 60000,
                            UsageId = consumableUsageList[1].Id
                            
                        }
                };



            using (var conn = factory.OpenDbConnection())
            {
                foreach (var house in houses)
                {
                    conn.InsertParam(house);
                }
                foreach (var consumable in consumables)
                {
                    conn.InsertParam(consumable);
                }

                foreach (var usage in consumableUsageList)
                {
                    conn.InsertParam(usage);
                }

                foreach (var detail in detailList)
                {
                    conn.InsertParam(detail);
                }
            }

            var list = service.GetUsageSummary(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(3));

            foreach (var item in list)
            {
                var usage = consumableUsageList.FirstOrDefault(p => p.Id == item.Id );
                Assert.Equal(usage.Date, item.Date);
                Assert.Equal(usage.Total,item.Total);

                foreach (var detail in item.Details)
                {
                    

                    foreach (var detailData in detailList)
                    {
                        //Console.WriteLine(detail.Consumable.Id);
                        if(detailData.UsageId == item.Id && detailData.ConsumableId == detail.Consumable.Id && detailData.HouseId == detail.House.Id)
                        {
                            //Console.WriteLine(detailData.ConsumableId);
                            //Console.WriteLine("----------");
                            Assert.Equal(detailData.Count, detail.Count);
                            Assert.Equal(detailData.SubTotal, detail.SubTotal);
                            Assert.Equal(detailData.UnitPrice, detail.UnitPrice);
                        }
                        
                    }
                
                }
               

            }
        }

        public void Dispose()
        {
            using (var conn = factory.OpenDbConnection())
            {
                conn.DeleteAll<Models.Data.EmployeeCostDetail>();
                conn.DeleteAll<Models.Data.EmployeeCost>();
                conn.DeleteAll<Models.Data.EggProductionDetail>();
                conn.DeleteAll<Models.Data.EggProduction>();
                conn.DeleteAll<Models.Data.ConsumableUsageDetail>();
                conn.DeleteAll<Models.Data.ConsumableUsage>();
                conn.DeleteAll<Models.Consumable>();
                conn.DeleteAll<Models.Employee>();
                conn.DeleteAll<Models.HenHouse>();
            }
        }
    }
}
