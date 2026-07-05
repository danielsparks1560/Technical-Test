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
        public ActionResult<PropertyViewing> BookViewing([FromBody] BookViewingRequest request)
        {
            var booking = _service.BookViewing(request);
            return CreatedAtAction(nameof(BookViewing), new { id = booking.Id }, booking);
        }

        [HttpGet("search", Name = "SearchViewings")]
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
