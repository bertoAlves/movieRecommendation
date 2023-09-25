using Common.AVProductMS.DTO;
using Microsoft.AspNetCore.Mvc;
using AVProduct.Services.Interfaces;

namespace AVProduct.Controllers
{
    /// <summary>
    /// TVShow Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TVShowController : ControllerBase
    {
        private readonly ITVShowService _service;

        /// <summary>
        /// TVShow Controller
        /// </summary>
        /// <param name="service"></param>
        public TVShowController(ITVShowService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all-time TV shows based on search criteria.
        /// </summary>
        /// <param name="keywords">Keywords separated by commas.</param>
        /// <param name="genres">Genres ids separated by commas.</param>
        /// <returns>Returns a list of all-time TV Shows.</returns>
        /// <response code="200">Returns the list of all-time TV Shows.</response>
        [HttpGet("alltime")]
        [ProducesResponseType(typeof(IEnumerable<TVShowDTO>), 200)]
        public async Task<IActionResult> GetAllTimeTVShows([FromQuery] string? keywords, [FromQuery] string? genres)
        {
            if (String.IsNullOrEmpty(keywords) && String.IsNullOrEmpty(genres))
            {
                return BadRequest("At least one search criteria must be specified");
            }

            return Ok(await _service.GetAllTimeTVShows(keywords, genres));
        }
    }
}