using Common.CinemaMS.DTO;
using CinemaMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMS.Controllers
{
    /// <summary>
    /// Genre Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _service;

        /// <summary>
        /// Genre Controller
        /// </summary>
        /// <param name="service"></param>
        public GenreController(IGenreService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get Most Successful Genres 
        /// </summary>
        /// <returns>Returns a list of most successful genres.</returns>
        /// <response code="200">Returns the list of most successful genres.</response>
        [HttpGet("mostsuccessful")]
        [ProducesResponseType(typeof(IEnumerable<GenreDTO>), 200)]
        public async Task<IActionResult> GetMostSuccessfulGenres()
        {
            return Ok(await _service.GetMostSuccessfulGenres());
        }
    }
}