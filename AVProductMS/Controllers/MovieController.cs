using Common.AVProductMS.DTO;
using Microsoft.AspNetCore.Mvc;
using AVProduct.Services.Interfaces;

namespace AVProduct.Controllers
{
    /// <summary>
    /// Movie Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _service;

        /// <summary>
        /// Movie Controller
        /// </summary>
        /// <param name="service"></param>
        public MovieController(IMovieService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all-time movies based on search criteria.
        /// </summary>
        /// <param name="keywords">Keywords separated by commas.</param>
        /// <param name="genres">Genres ids separated by commas.</param>
        /// <returns>Returns a list of all-time movies.</returns>
        /// <response code="200">Returns the list of all-time movies.</response>
        [HttpGet("alltime")]
        [ProducesResponseType(typeof(IEnumerable<MovieDTO>), 200)]
        public async Task<IActionResult> GetAllTimeMovies([FromQuery] string? keywords, [FromQuery] string? genres)
        {
            if (String.IsNullOrEmpty(keywords) && String.IsNullOrEmpty(genres))
            {
                return BadRequest("At least one search criteria must be specified");
            }

            return Ok(await _service.GetAllTimeMovies(keywords, genres));
        }

        /// <summary>
        /// Get upcoming movies based on search criteria.
        /// </summary>
        /// <param name="keywords">Keywords separated by commas.</param>
        /// <param name="genres">Genres ids separated by commas.</param>
        /// <param name="daysFromNow">Number of days from now.</param>
        /// <param name="ageRate">Movie Age Rate</param>
        /// <returns>Returns a list of upcoming movies.</returns>
        /// <response code="200">Returns the list of upcoming movies.</response>
        [HttpGet("upcoming")]
        [ProducesResponseType(typeof(IEnumerable<MovieDTO>), 200)]
        public async Task<IActionResult> GetUpcomingMovies([FromQuery] string? keywords, [FromQuery] string? genres, [FromQuery] int daysFromNow)
        {
            if (daysFromNow <= 0)
            {
                return BadRequest("The 'daysFromNow' parameter must be a positive number.");
            }

            return Ok(await _service.GetUpcomingMovies(keywords, genres, daysFromNow, null));
        }

        /// <summary>
        /// Get upcoming movies based on search criteria.
        /// </summary>
        /// <param name="keywords">Keywords separated by commas.</param>
        /// <param name="genres">Genres ids separated by commas.</param>
        /// <param name="daysFromNow">Number of days from now.</param>
        /// <param name="ageRate">Movie Age Rate</param>
        /// <returns>Returns a list of upcoming movies.</returns>
        /// <response code="200">Returns the list of upcoming movies.</response>
        [HttpGet("upcomingbyagerate")]
        [ProducesResponseType(typeof(IEnumerable<MovieDTO>), 200)]
        public async Task<IActionResult> GetUpcomingMovies([FromQuery] string? genres, [FromQuery] int daysFromNow, string? ageRate)
        {
            if (daysFromNow <= 0)
            {
                return BadRequest("The 'daysFromNow' parameter must be a positive number.");
            }

            return Ok(await _service.GetUpcomingMovies(null, genres, daysFromNow, ageRate));
        }

        /// <summary>
        /// Get blockbusters movies.
        /// </summary>
        /// <param name="genres">Keywords separated by commas.</param>
        /// <param name="numberOfWeeks">Number of weeks.</param>
        /// <param name="numberOfBigScreens">Number of big screens.</param>
        /// <returns>Returns a list of blockbuster movies.</returns>
        /// <response code="200">Returns the list of blockbuster movies.</response>
        [HttpGet("blockbusters")]
        [ProducesResponseType(typeof(IEnumerable<MovieDTO>), 200)]
        public async Task<IActionResult> GetBlockbusterMovies([FromQuery] string? genres, [FromQuery] int numberOfWeeks, [FromQuery] int numberOfBigScreens)
        {
            if (numberOfWeeks <= 0)
            {
                return BadRequest("The 'numberOfWeeks' parameter must be a positive number.");
            }

            if (numberOfBigScreens <= 0)
            {
                return BadRequest("The 'numberOfBigScreens' parameter must be a positive number.");
            }

            return Ok(await _service.GetBlockbusterMovies(genres, numberOfWeeks, numberOfBigScreens));
        }


        /// <summary>
        /// Get minority genres movies.
        /// </summary>
        /// <param name="withoutgenres">Genres ids separated by commas to ignore.</param>
        /// <param name="numberOfWeeks">Number of weeks.</param>
        /// <param name="numberOfSmallScreens">Number of small screens.</param>
        /// <returns>Returns a list of minority genres movies.</returns>
        /// <response code="200">Returns the list of minority movies.</response>
        [HttpGet("minority_genres")]
        [ProducesResponseType(typeof(IEnumerable<MovieDTO>), 200)]
        public async Task<IActionResult> GetMinorityGenresMovies([FromQuery] string? withoutgenres, [FromQuery] int numberOfWeeks, [FromQuery] int numberOfSmallScreens)
        {
            if (numberOfWeeks <= 0)
            {
                return BadRequest("The 'numberOfWeeks' parameter must be a positive number.");
            }

            if (numberOfSmallScreens <= 0)
            {
                return BadRequest("The 'numberOfSmallScreens' parameter must be a positive number.");
            }

            return Ok(await _service.GetMinorityGenresMovies(withoutgenres, numberOfWeeks, numberOfSmallScreens));
        }
    }
}