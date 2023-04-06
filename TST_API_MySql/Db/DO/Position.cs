using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TST_API_MySql.Db.DO
{
    /// <summary>
    /// Defines the database entity of Employee Position
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        [Range(1, 15, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Grade { get; set; }

        /// <summary>
        /// Gets or sets the employees.
        /// </summary>
        /// <value>
        /// The employees.
        /// </value>
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}