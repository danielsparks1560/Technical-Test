using Property_Viewing_Slots_API.Models;
using Property_Viewing_Slots_API.Storage;

namespace Property_Viewing_Slots_API.Services
{
    public class PropertyViewingService : IPropertyViewingService
    {
        private readonly IPropertyViewingStorage _storage;

        public PropertyViewingService(IPropertyViewingStorage storage)
        {
            _storage = storage;
        }

        public PropertyViewing BookViewing(BookViewingRequest request)
        {
            return _storage.Add(new PropertyViewing
            {
                PropertyId = request.PropertyId,
                UserId = request.UserId,
                StartTime = request.StartTime
            });
        }

        public IEnumerable<PropertyViewing> SearchViewings(Guid propertyId, DateTime startDate, DateTime endDate) 
        {
            return _storage.GetByPropertyAndDateRange(propertyId, startDate, endDate);
        }
    }
}
