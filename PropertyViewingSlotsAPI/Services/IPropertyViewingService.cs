using Property_Viewing_Slots_API.Models;

namespace Property_Viewing_Slots_API.Services
{
    public interface IPropertyViewingService
    {
        PropertyViewing BookViewing(BookViewingRequest request);
        IEnumerable<PropertyViewing> SearchViewings(Guid propertyId, DateTime startDate, DateTime endDate);
    }
}
