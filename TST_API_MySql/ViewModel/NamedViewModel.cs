using System;

namespace TST_API_MySql.ViewModel
{
    /// <summary>
    /// Defines the class of Named View Model
    /// </summary>
    public class NamedViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}