using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TST_API_MySql.Db;

namespace TST_API_MySql.Controllers
{
    /// <summary>
    /// Defines the base class of Api Controllers
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    public class ApiControllerBase : ControllerBase
    {
        #region Protected Properties

        /// <summary>
        /// The logger
        /// </summary>
        protected ILogger<ApiControllerBase> Logger { get;  }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        protected EmployeeDbContext Db { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiControllerBase"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="db">The database.</param>
        public ApiControllerBase(ILogger<ApiControllerBase> logger, EmployeeDbContext db)
        {
            Logger = logger;
            Db = db;
        }

        #endregion
    }
}