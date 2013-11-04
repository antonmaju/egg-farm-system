using System.Data;
using EggFarmSystem.Models.Reporting;
using EggFarmSystem.Utilities;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace EggFarmSystem.Services
{
    public interface IReportingService
    {
        IList<EmployeeCostSummary> GetEmployeeCostSummary(DateTime start, DateTime end);
<<<<<<< HEAD
        IList<UsageSummary> GetUsageSummary(DateTime start, DateTime end);

=======

        IList<EggProductionReport> GetEggProductionReport(DateTime start, DateTime end);
>>>>>>> remotes/upstream/master
    }

    public class ReportingService : IReportingService
    {
        private IDbConnectionFactory factory;

        public ReportingService(IDbConnectionFactory factory) 
        {
            this.factory = factory;
        }

        public IList<EmployeeCostSummary> GetEmployeeCostSummary(DateTime start, DateTime end)
        {
            string sql =
                @"SELECT Employee.Id, Employee.Name, COUNT(EmployeeCostDetail.EmployeeId) AS 'Days', SUM(EmployeeCostDetail.Salary) AS 'TotalSalary'
FROM EmployeeCostDetail JOIN EmployeeCost ON EmployeeCostDetail.CostId = EmployeeCost.Id
JOIN Employee ON EmployeeCostDetail.EmployeeId = Employee.Id
WHERE EmployeeCost.Date BETWEEN @start AND @end
GROUP BY Employee.Id
ORDER BY Employee.Name";

            var result = new List<EmployeeCostSummary>();

            using (var conn = factory.OpenDbConnection())
            {
                var command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                command.Parameters.Add(new MySqlParameter("@start", MySqlDbType.Date) {Value = start});
                command.Parameters.Add(new MySqlParameter("@end", MySqlDbType.Date) {Value = end});
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new EmployeeCostSummary
                        {
                            Id = new Guid(reader["Id"].ToString()),
                            Name = reader["Name"].ToString(),
                            Days = Convert.ToInt32(reader["Days"]),
                            TotalSalary = Convert.ToInt64(reader["TotalSalary"])
                        };
                        result.Add(item);
                    }
                }               
            }

            return result;
        }

<<<<<<< HEAD
        public IList<UsageSummary> GetUsageSummary(DateTime start, DateTime end)
        {
            string sql =
                @"SELECT Consumable.Id, Consumable.Name,ConsumableUsage.Date as UsageDate,ConsumableUsageDetail.Count AS 'Count', ConsumableUsageDetail.Subtotal AS 'SubTotal',ConsumableUsageDetail.UnitPrice,HenHouse.Name as 'HenHouseName'
FROM ConsumableUsageDetail JOIN ConsumableUsage ON ConsumableUsageDetail.UsageId = ConsumableUsage.Id
JOIN Consumable ON ConsumableUsageDetail.ConsumableId = Consumable.Id JOIN HenHouse on ConsumableUsageDetail.HouseId = HenHouse.Id
WHERE ConsumableUsage.Date BETWEEN @start AND @end
ORDER BY ConsumableUsage.Date,ConsumableUsage.Date";

            var result = new List<UsageSummary>();

            using (var conn = factory.OpenDbConnection())
            {
                var command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                command.Parameters.Add(new MySqlParameter("@start", MySqlDbType.Date) { Value = start });
                command.Parameters.Add(new MySqlParameter("@end", MySqlDbType.Date) { Value = end });
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new UsageSummary
                        {
                            Id = new Guid(reader["Id"].ToString()),
                            Name = reader["Name"].ToString(),
                            Count = Convert.ToInt32(reader["Count"]),
                            SubTotal = Convert.ToInt64(reader["SubTotal"]),
                            UnitPrice = Convert.ToInt64(reader["UnitPrice"]),
                            UsageDate = Convert.ToDateTime(reader["UsageDate"]),
                            HenHouseName = reader["HenHouseName"].ToString()
                            
                        };
                        result.Add(item);
                    }
                }
            }

            return result;
=======

        public IList<EggProductionReport> GetEggProductionReport(DateTime start, DateTime end)
        {
            string sql =
                @"SELECT EggProductionDetail.*, HenHouse.Name AS 'HouseName' FROM EggProductionDetail JOIN HenHouse ON 
EggProductionDetail.HouseId = HenHouse.Id where EggProductionDetail.ProductionId=@productionId order by HenHouse.Name";

            using (var conn = factory.OpenDbConnection())
            {

                var ev = OrmLiteConfig.DialectProvider.ExpressionVisitor<Models.Data.EggProduction>()
                                      .Where(e => e.Date >= start.Date && e.Date <= end.Date)
                                      .OrderBy(e => e.Date);

                var productionList = conn.Select(ev).Select(p => new EggProductionReport
                    {
                        Date = p.Date,
                        Id = p.Id
                    }).ToList();

                var command = conn.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                foreach (var productionData in productionList)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new MySqlParameter("@productionId", MySqlDbType.Guid)
                        {
                            Value = productionData.Id
                        });

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var detail = new EggProductionReportDetail();
                            detail.GoodEggCount = DataConverter.ToInt32(reader["GoodEggCount"]);
                            detail.RetailQuantity = DataConverter.ToDecimal(reader["RetailQuantity"]);
                            detail.CrackedEggCount = DataConverter.ToInt32(reader["CrackedEggCount"]);
                            detail.Fcr = DataConverter.ToDecimal(reader["Fcr"]);
                            detail.House = new HouseInfo
                                {
                                    Id = DataConverter.ToGuid(reader["HouseId"]),
                                    Name = DataConverter.ToString(reader["HouseName"])
                                };

                            productionData.Details.Add(detail);
                        }
                    }
                }

                return productionList;
            }
>>>>>>> remotes/upstream/master
        }
    }
}