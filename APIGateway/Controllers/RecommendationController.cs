using APIGateway.DTO.AVProduct;
using APIGateway.Services.Interfaces;
using Common.AVProductMS.DTO;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers
{
    /// <summary>
    /// Recommendation Controller
    /// </summary>
    [ApiController]
    [Route("apigateway/[controller]")]
    [Produces("application/json")]
    public class RecommendationController : ControllerBase
    {

        private readonly IRecommendationService _recommendationService;

        /// <summary>
        /// Recommendation Controller
        /// </summary>
        /// <param name="service"></param>
        public RecommendationController(IRecommendationService service)
        {
            _recommendationService = service;
        }

        /// <summary>
        /// Get all-time movies based on search criteria.
        /// </summary>
        /// <param name="keywords">Keywords separated by commas.</param>
        /// <param name="genres">Genres ids separated by commas.</param>
        /// <returns>Returns a list of all-time movies.</returns>
        /// <response code="200">Returns the list of all-time movies.</response>
        [HttpGet("/movie/alltime")]
        [ProducesResponseType(typeof(IEnumerable<MovieRecommendation>), 200)]
        public async Task<IActionResult> GetAllTimeMoviesRecommendation([FromQuery] string? keywords, [FromQuery] string? genres)
        {
            if (String.IsNullOrEmpty(keywords) && String.IsNullOrEmpty(genres))
            {
                return BadRequest("At least one search criteria must be specified");
            }

            return Ok(await _recommendationService.GetAllTimeMoviesRecommendation(keywords, genres));
        }

        /// <summary>
        /// Get upcoming movies based on search criteria.
        /// </summary>
        /// <param name="keywords">Keywords separated by commas.</param>
        /// <param name="genres">Genres ids separated by commas.</param>
        /// <param name="daysFromNow">Number of days from now.</param>
        /// <returns>Returns a list of upcoming movies.</returns>
        /// <response code="200">Returns the list of upcoming movies.</response>
        [HttpGet("/movie/upcoming")]
        [ProducesResponseType(typeof(IEnumerable<MovieRecommendation>), 200)]
        public async Task<IActionResult> GetUpcomingMoviesRecommendation([FromQuery] string? keywords, [FromQuery] string? genres, [FromQuery] int daysFromNow)
        {
            if (String.IsNullOrEmpty(keywords) && String.IsNullOrEmpty(genres))
            {
                return BadRequest("At least one search criteria must be specified");
            }

            return Ok(await _recommendationService.GetUpcomingMoviesRecommendation(keywords, genres, daysFromNow));
        }

        /// <summary>
        /// Get upcoming movies based on search criteria and agerate.
        /// </summary>
        /// <param name="genres">Genres ids separated by commas.</param>
        /// <param name="daysFromNow">Number of days from now.</param>
        /// <param name="ageRate">Age rate.</param>
        /// <returns>Returns a list of upcoming movies.</returns>
        /// <response code="200">Returns the list of upcoming movies.</response>
        [HttpGet("/movie/upcomingbyagerate")]
        [ProducesResponseType(typeof(IEnumerable<MovieRecommendation>), 200)]
        public async Task<IActionResult> GetUpcomingMoviesRecommendationByAgeRate([FromQuery] string genres, [FromQuery] int daysFromNow, [FromQuery] string ageRate)
        {
            return Ok(await _recommendationService.GetUpcomingMoviesRecommendationByAgeRate(genres, daysFromNow, ageRate));
        }


        /// <summary>
        /// Get all-time TV shows based on search criteria.
        /// </summary>
        /// <param name="keywords">Keywords separated by commas.</param>
        /// <param name="genres">Genres ids separated by commas.</param>
        /// <returns>Returns a list of all-time TV Shows.</returns>
        /// <response code="200">Returns the list of all-time TV Shows.</response>
        [HttpGet("/tvshow/alltime")]
        [ProducesResponseType(typeof(IEnumerable<TVShowRecommendation>), 200)]
        public async Task<IActionResult> GetAllTimeTVShowsRecommendation([FromQuery] string? keywords, [FromQuery] string? genres)
        {
            return Ok(await _recommendationService.GetAllTimeTVShowsRecommendation(keywords, genres));
        }


        /// <summary>
        /// Get all-time documentaries based on search criteria.
        /// </summary>
        /// <param name="topics">Topics separated by commas.</param>
        /// <returns>Returns a list of all-time documentaries.</returns>
        /// <response code="200">Returns the list of all-time documentaries.</response>
        [HttpGet("/documentary/alltime")]
        [ProducesResponseType(typeof(IEnumerable<DocumentaryRecommendation>), 200)]
        public async Task<IActionResult> GetAllTimeDocumentariesRecommendation([FromQuery] string topics)
        {
            return Ok(await _recommendationService.GetAllTimeDocumentariesRecommendation(topics));
        }

        /// <summary>
        /// Get suggested billboard.
        /// </summary>
        /// <param name="numberScreens">Number of screens.</param>
        /// <param name="numberWeeks">Number of weeks.</param>
        /// <returns>Returns a suggested billboard.</returns>
        /// <response code="200">Returns a suggested billboard.</response>
        [HttpGet("billboard")]
        [ProducesResponseType(typeof(Billboard), 200)]
        public async Task<IActionResult> GetSuggestedBillboard([FromQuery] int numberWeeks, [FromQuery] int numberScreens)
        {
            if (numberWeeks <= 0)
            {
                return BadRequest("The 'numberOfWeeks' parameter must be a positive number.");
            }
            if (numberScreens <= 0)
            {
                return BadRequest("The 'numberScreens' parameter must be a positive number.");
            }

            return Ok(await _recommendationService.GetSuggestedBillboard(numberWeeks, numberScreens));
        }

        /// <summary>
        /// Get intelligent billboard.
        /// </summary>
        /// <param name="numberWeeks">Number of weeks.</param>
        /// <param name="bigScreens">Number of big screens.</param>
        /// <param name="smallScreens">Number of small screens.</param>
        /// <param name="useSuccessfulGenres">Use or not most successful movie genres to create billboard.</param>
        /// <returns>Returns a intelligent billboard.</returns>
        /// <response code="200">Returns a intelligent billboard.</response>
        [HttpGet("billboard/intelligent")]
        [ProducesResponseType(typeof(IntelligentBillboard), 200)]
        public async Task<IActionResult> GetIntelligentBillboard([FromQuery] int numberWeeks, [FromQuery] int bigScreens, [FromQuery] int smallScreens, [FromQuery] bool? useSuccessfulGenres)
        {
            if (numberWeeks <= 0)
            {
                return BadRequest("The 'numberOfWeeks' parameter must be a positive number.");
            }

            if (bigScreens <= 0 && smallScreens <= 0)
            {
                return BadRequest("Either 'bigScreens' or 'smallScreens' must be a positive number.");
            }

            return Ok(await _recommendationService.GetIntelligentBillboard(numberWeeks, bigScreens, smallScreens, useSuccessfulGenres ?? false));
        }
    }
}