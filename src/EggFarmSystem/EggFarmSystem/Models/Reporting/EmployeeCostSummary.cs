using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models.Reporting
{
    /// <summary>
    /// DTO for employee cost summary report
    /// </summary>
    public class EmployeeCostSummary
    {
        /// <summary>
        /// Gets or sets the employee id.
        /// </summary>
        /// <value>The employee id.</value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the employee name.
        /// </summary>
        /// <value>The employee name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets total working days.
        /// </summary>
        /// <value>The working days.</value>
        public int Days { get; set; }

        /// <summary>
        /// Gets or sets the total salary.
        /// </summary>
        /// <value>The total salary.</value>
        public long TotalSalary { get; set; } 
    }
}
