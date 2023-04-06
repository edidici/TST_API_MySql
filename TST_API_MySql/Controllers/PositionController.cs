using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TST_API_MySql.Db;
using TST_API_MySql.Db.DO;
using TST_API_MySql.ViewModel;

namespace TST_API_MySql.Controllers
{
    /// <summary>
    ///  Defines the class of Position Api Controller
    /// </summary>
    /// <seealso cref="TST_API_MySql.Controllers.ApiControllerBase" />
    public class PositionController : ApiControllerBase
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="db">The database.</param>
        public PositionController(ILogger<PositionController> logger, EmployeeDbContext db) : base(logger, db)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the positions.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/[controller]/[action]")]
        [ProducesResponseType(typeof(IEnumerable<Position>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPositions()
        {
            return Ok(await Db.Positions.Select(p => new NamedViewModel { Id = p.Id.Value, Name = p.Name }).ToListAsync());
        }

        /// <summary>
        /// Gets the position by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/[controller]/[action]")]
        [ProducesResponseType(typeof(Position), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPositionById([FromQuery] Guid id)
        {
            var position = await Db.Positions.FirstOrDefaultAsync(p => p.Id == id);
            if (position == null)
            {
                return NotFound($"Position with id = {id} is not found");
            }

            return Ok(position);
        }

        /// <summary>
        /// Saves the position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/[controller]/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SavePosition([FromQuery] Position position)
        {
            if (position.Id == null)
            {
                position.Id = Guid.NewGuid();
                await Db.Positions.AddAsync(position);
                return Ok();
            }

            Db.Positions.Update(position);
            await Db.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Deletes the position by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/[controller]/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeletePositionById([FromQuery] Guid id)
        {
            var position = await Db.Positions.FirstOrDefaultAsync(p => p.Id.Value == id);
            if (position == null)
            {
                return NotFound($"Position with id = {id} is not found");
            }

            Db.Positions.Remove(position);

            await Db.SaveChangesAsync();

            return Ok();
        }

        #endregion
    }
}