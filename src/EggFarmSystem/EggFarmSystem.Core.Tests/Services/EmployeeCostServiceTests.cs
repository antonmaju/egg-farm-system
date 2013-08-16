using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using EggFarmSystem.Models.Data;
using EggFarmSystem.Services;
using ServiceStack.OrmLite;
using Xunit;
using EmployeeCostDetail = EggFarmSystem.Models.EmployeeCostDetail;

namespace EggFarmSystem.Core.Tests.Services
{
    public class EmployeeCostServiceTests : IDisposable
    {
        private IEmployeeCostService service;
        private IDbConnectionFactory factory;

        public EmployeeCostServiceTests()
        {
            factory = DatabaseTestInitializer.GetConnectionFactory();
            service = new EmployeeCostService(factory);
        }

        [Fact]
        public void Can_SearchByDate()
        {
            var employee = new Employee
            {
                Active = true,
                Id = Guid.NewGuid(),
                Name = "Employee #1",
                Salary = 50000
            };

            var costData = new Models.Data.EmployeeCost
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Today,
                Total = 1000
            };

            using (var conn = factory.OpenDbConnection())
            {
                conn.InsertParam(employee);
                conn.InsertParam(costData);
            }

            var result = service.Search(new DateRangeSearchInfo
                {
                    Start = DateTime.Today.AddDays(-1),
                    End = DateTime.Today.AddDays(10),
                    PageIndex = 1,
                    PageSize = 10
                });

            Assert.Equal(1, result.Total);
            Compare(costData, null, result.Items[0]);
        }

        [Fact]
        public void Can_GetById()
        {
            Models.Data.EmployeeCost costData;
            var id = Guid.NewGuid();
            var details = new List<Models.Data.EmployeeCostDetail>();
            using (var conn = factory.OpenDbConnection())
            {
                costData = new Models.Data.EmployeeCost
                    {
                        Id = id,
                        Date = DateTime.Today,
                        Total = 100000
                    };

                conn.InsertParam(costData);

                
                for (int i = 0; i < 2; i++)
                {
                    var employee = new Employee
                        {
                            Active = true,
                            Id = Guid.NewGuid(),
                            Name = "Employee-" + i,
                            Salary = 50000
                        };

                    conn.InsertParam(employee);

                    var detail = new Models.Data.EmployeeCostDetail();
                    detail.CostId = id;
                    detail.Description = "Desc" + i;
                    detail.Present = true;
                    detail.Salary = 50000;
                    detail.EmployeeId = employee.Id;

                    conn.InsertParam(detail);
                    details.Add(detail);
                }
            }

            var cost = service.Get(id);
            Compare(costData, details, cost);
        }

        [Fact]
        public void Can_GetByDate()
        {
            Models.Data.EmployeeCost costData = null;
            var details = new List<Models.Data.EmployeeCostDetail>();
            DateTime today = DateTime.Today;

            using (var conn = factory.OpenDbConnection())
            {
                Guid dataId = Guid.NewGuid();

                costData = new Models.Data.EmployeeCost
                {
                    Id = dataId,
                    Date = today,
                    Total = 100000
                };

                conn.InsertParam(costData);

                for (int i = 0; i < 2; i++)
                {
                    var employee = new Employee
                    {
                        Active = true,
                        Id = Guid.NewGuid(),
                        Name = "Employee-" + i,
                        Salary = 50000
                    };

                    conn.InsertParam(employee);

                    var detail = new Models.Data.EmployeeCostDetail();
                    detail.CostId = dataId;
                    detail.Description = "Desc" + i;
                    detail.Present = true;
                    detail.Salary = 50000;
                    detail.EmployeeId = employee.Id;

                    conn.InsertParam(detail);
                    details.Add(detail);
                }

                var cost = service.GetByDate(today);
                Compare(costData, details, cost);
            }
        }

        [Fact]
        public void Can_SaveNewCost()
        {
            var employee = new Employee
            {
                Active = true,
                Id = Guid.NewGuid(),
                Name = "Employee #1",
                Salary = 50000
            };

            using (var conn = factory.OpenDbConnection())
            {
                conn.InsertParam(employee);
            }

            var cost = new Models.EmployeeCost
                {
                    Date = DateTime.Today,
                    Total = 100000,
                    Details = new List<EmployeeCostDetail>()
                        {
                            new EmployeeCostDetail
                                {
                                    EmployeeId = employee.Id,
                                    Description = "Desc",
                                    Present = true,
                                    Salary = 100000
                                }
                        }
                };

            service.Save(cost);

            using (var conn = factory.OpenDbConnection())
            {
                var costData = conn.FirstOrDefault<Models.Data.EmployeeCost>(c => c.Date == cost.Date);
                var details = conn.Where<Models.Data.EmployeeCostDetail>(new {CostId = costData.Id});
                Compare(costData, details, cost);
            }
        }

        [Fact]
        public void Can_SaveExistingCost()
        {
            var employee = new Employee
            {
                Active = true,
                Id = Guid.NewGuid(),
                Name = "Employee #1",
                Salary = 50000
            };

            Guid id = Guid.NewGuid();

            Models.Data.EmployeeCost costData = null;
            Models.Data.EmployeeCostDetail costDetail = null;

            using (var conn = factory.OpenDbConnection())
            {
                conn.InsertParam(employee);

                costData = new Models.Data.EmployeeCost
                {
                    Id =id,
                    Date = DateTime.Today,
                    Total = 1000
                };

                conn.InsertParam(costData);

                costDetail = new Models.Data.EmployeeCostDetail
                    {
                        CostId = id,
                        EmployeeId = employee.Id,
                        Description = "desc",
                        Present = true,
                        Salary = 1000
                    };

                conn.InsertParam(costDetail);
            }

            var model = new Models.EmployeeCost
                {
                    Id = id,
                    Date = DateTime.Today.AddDays(1),
                    Total = 10000,
                    Details = new List<EmployeeCostDetail>
                        {
                            new EmployeeCostDetail
                                {
                                    Description = "des",
                                    EmployeeId = employee.Id,
                                    Present = false,
                                    Salary = 10000
                                }
                        }
                };

            service.Save(model);

            using (var conn = factory.OpenDbConnection())
            {
                costData = conn.FirstOrDefault<Models.Data.EmployeeCost>(c => c.Date == model.Date);
                var details = conn.Where<Models.Data.EmployeeCostDetail>(new { CostId = costData.Id });
                Compare(costData, details, model);
            }
        }

        [Fact]
        public void Can_DeleteById()
        {
            Guid id = Guid.NewGuid();

            var costData = new Models.Data.EmployeeCost
            {
                Id = id,
                Date = DateTime.Today,
                Total = 1000
            };

            using (var conn = factory.OpenDbConnection())
            {
                conn.InsertParam(costData);
            }

            service.Delete(id);

            using (var conn = factory.OpenDbConnection())
            {
                var usageData = conn.GetByIdOrDefault<Models.Data.EmployeeCost>(id);
                Assert.Null(usageData);
            }
        }

        void Compare(Models.Data.EmployeeCost costData, List<Models.Data.EmployeeCostDetail> details, Models.EmployeeCost cost)
        {
            Assert.Equal(costData.Id, cost.Id);
            Assert.Equal(costData.Date, cost.Date);
            Assert.Equal(costData.Total, cost.Total);

            if (details != null)
            {
                Assert.Equal(details.Count, cost.Details.Count);

                foreach (var detailData in details)
                {
                    var detail = cost.Details.FirstOrDefault(d => d.EmployeeId == detailData.EmployeeId);

                    Assert.Equal(detailData.Description, detail.Description);
                    Assert.Equal(detailData.Present, detail.Present);
                    Assert.Equal(detailData.Salary, detail.Salary);
                }
            }
        }

        public void Dispose()
        {
            using (var conn = factory.OpenDbConnection())
            {
                conn.DeleteAll<Models.Data.EmployeeCostDetail>();
                conn.DeleteAll<Models.Data.EmployeeCost>();
                conn.DeleteAll<Models.Employee>();
            }
        }
    }
}
