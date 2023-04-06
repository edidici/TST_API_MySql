using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TST_API_MySql.Db;
using TST_API_MySql.Db.DO;
using TST_API_MySql.ViewModel;

namespace TST_API_MySql.Controllers
{
    /// <summary>
    /// Defines the class of Employee Api Controller
    /// </summary>
    /// <seealso cref="TST_API_MySql.Controllers.ApiControllerBase" />
    public class EmployeeController : ApiControllerBase
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="db">The database.</param>
        public EmployeeController(ILogger<EmployeeController> logger, EmployeeDbContext db) : base(logger, db)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the employees.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/[controller]/[action]")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployees()
        {
            return Ok(await Db.Employees.Select(p => new NamedViewModel { Id = p.Id.Value, Name = p.Fio }).ToListAsync());
        }

        /// <summary>
        /// Gets the employee by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/[controller]/[action]")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeById([FromQuery] Guid id)
        {
            var employee = await Db.Employees.Include(p => p.Positions).FirstOrDefaultAsync(p => p.Id == id);
            if (employee == null)
            {
                return NotFound($"Employee with id = {id} is not found");
            }

            return Ok(employee);
        }

        /// <summary>
        /// Saves the employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/[controller]/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SaveEmployee([FromQuery] Employee employee)
        {
            if (employee.Id == null)
            {
                employee.Id = Guid.NewGuid();
                await Db.Employees.AddAsync(employee);
                return Ok();
            }

            Db.Employees.Update(employee);
            await Db.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Deletes the employee by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/[controller]/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteEmployeeById([FromQuery] Guid id)
        {
            var employee = await Db.Employees.FirstOrDefaultAsync(p => p.Id.Value == id);
            if (employee == null)
            {
                return NotFound($"Employee with id = {id} is not found");
            }

            Db.Employees.Remove(employee);
            await Db.SaveChangesAsync();

            return Ok();
        }

        #endregion
    }
}