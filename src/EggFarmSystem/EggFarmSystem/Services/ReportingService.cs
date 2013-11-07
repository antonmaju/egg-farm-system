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
        IList<UsageSummary> GetUsageSummary(DateTime start, DateTime end);
        IList<EggProductionReport> GetEggProductionReport(DateTime start, DateTime end);

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

        public IList<UsageSummary> GetUsageSummary(DateTime start, DateTime end)
        {
            string sql = @"SELECT ConsumableUsageDetail.*, HenHouse.Name AS 'HouseName',Consumable.Name AS 'ConsumableName' FROM ConsumableUsageDetail JOIN HenHouse ON ConsumableUsageDetail.HouseId = HenHouse.Id Join Consumable ON ConsumableUsageDetail.ConsumableId = Consumable.Id where ConsumableUsageDetail.UsageId=@usageId order by HenHouse.Name";

            using (var conn = factory.OpenDbConnection())
            {

                var ev = OrmLiteConfig.DialectProvider.ExpressionVisitor<Models.Data.ConsumableUsage>()
                                      .Where(e => e.Date >= start.Date && e.Date <= end.Date)
                                      .OrderBy(e => e.Date);

                var usageList = conn.Select(ev).Select(p => new UsageSummary
                {
                    Date = p.Date,
                    Id = p.Id,
                    Total = p.Total
                }).ToList();

                var command = conn.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                foreach (var usageData in usageList)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new MySqlParameter("@usageId", MySqlDbType.Guid)
                    {
                        Value = usageData.Id
                    });

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var detail = new UsageSummaryDetail();
                            detail.Count = DataConverter.ToInt32(reader["Count"]);
                            detail.SubTotal = DataConverter.ToInt32(reader["SubTotal"]);
                            detail.UnitPrice = DataConverter.ToInt32(reader["UnitPrice"]);
                            detail.House = new HouseInfo
                                {
                                    Id = DataConverter.ToGuid(reader["HouseId"]),
                                    Name = DataConverter.ToString(reader["HouseName"])
                                };
                            detail.Consumable = new ConsumableUsageInfo
                                {

                                    Id = DataConverter.ToGuid(reader["HouseId"]),
                                    Name = DataConverter.ToString(reader["ConsumableName"])
                                };
                            usageData.Details.Add(detail);
                        }
                    }
                }

                return usageList;
            }
        }

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
        }
    }
}