using EggFarmSystem.Models;
using EggFarmSystem.Models.Data;
using EggFarmSystem.Services;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

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
