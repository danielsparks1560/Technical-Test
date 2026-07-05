using Microsoft.AspNetCore.Mvc;
using Property_Viewing_Slots_API.Models;
using Property_Viewing_Slots_API.Services;

namespace Property_Viewing_Slots_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PropertyViewingController : ControllerBase
    {
        private readonly IPropertyViewingService _service;

        public PropertyViewingController(IPropertyViewingService service)
        {
            _service = service;
        }

        [HttpPost("book", Name = "BookViewing")]
        [ProducesResponseType(typeof(PropertyViewing), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public ActionResult<PropertyViewing> BookViewing([FromBody] BookViewingRequest request)
        {
            try
            {
                var booking = _service.BookViewing(request);
                return CreatedAtAction(nameof(BookViewing), new { id = booking.Id }, booking);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("search", Name = "SearchViewings")]
        [ProducesResponseType(typeof(IEnumerable<PropertyViewing>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<PropertyViewing>> SearchViewings(
            [FromQuery] Guid propertyId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var results = _service.SearchViewings(propertyId, startDate, endDate);
            return Ok(results);
        }
    }
}
