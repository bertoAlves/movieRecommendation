using Common.AVProductMS.DTO;
using Microsoft.AspNetCore.Mvc;
using AVProduct.Services.Interfaces;

namespace AVProduct.Controllers
{
    /// <summary>
    /// Documentary Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DocumentaryController : ControllerBase
    {
        private readonly IDocumentaryService _service;

        /// <summary>
        /// Documentary Controller
        /// </summary>
        /// <param name="service"></param>
        public DocumentaryController(IDocumentaryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all-time documentaries based on search criteria.
        /// </summary>
        /// <param name="topics">Topics separated by commas.</param>
        /// <returns>Returns a list of all-time documentaries.</returns>
        /// <response code="200">Returns the list of all-time documentaries.</response>
        [HttpGet("alltime")]
        [ProducesResponseType(typeof(IEnumerable<DocumentaryDTO>), 200)]
        public async Task<IActionResult> GetAllTimeDocumentaries([FromQuery] string topics)
        {
            return Ok(await _service.GetAllTimeDocumentaries(topics));
        }
    }
}