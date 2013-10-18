using System.Data;
using EggFarmSystem.Models.Reporting;
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
    }
}