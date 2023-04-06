using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TST_API_MySql.Db.DO
{
    /// <summary>
    /// Defines the database entity of Employee
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the fio.
        /// </summary>
        /// <value>
        /// The fio.
        /// </value>
        [Required]
        public string Fio { get; set; }

        /// <summary>
        /// Gets or sets the birthday.
        /// </summary>
        /// <value>
        /// The birthday.
        /// </value>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// Gets or sets the positions.
        /// </summary>
        /// <value>
        /// The positions.
        /// </value>
        public List<Position> Positions { get; set; } = new List<Position>();
    }
}