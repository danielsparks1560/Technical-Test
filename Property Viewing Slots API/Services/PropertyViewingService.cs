using Property_Viewing_Slots_API.Models;
using Property_Viewing_Slots_API.Storage;

namespace Property_Viewing_Slots_API.Services
{
    public class PropertyViewingService : IPropertyViewingService
    {
        private readonly IPropertyViewingStorage _storage;

        private static readonly TimeZoneInfo BritishZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");
        private static readonly TimeSpan SlotDuration = TimeSpan.FromMinutes(30);

        public PropertyViewingService(IPropertyViewingStorage storage)
        {
            _storage = storage;
        }

        public PropertyViewing BookViewing(BookViewingRequest request)
        {
            var utcTime = DateTime.SpecifyKind(request.StartTime, DateTimeKind.Utc);
            var britishTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, BritishZone);

            if (britishTime.Hour < 9 || britishTime.Hour >= 20)
                throw new ArgumentException("Bookings are only available between 09:00 and 20:00 British time.");

            var newSlotEnd = request.StartTime.Add(SlotDuration);
            var candidates = _storage.GetByPropertyAndDateRange(
                request.PropertyId,
                request.StartTime.Subtract(SlotDuration),
                newSlotEnd);

            if (candidates.Any(v => request.StartTime < v.StartTime.Add(SlotDuration) && newSlotEnd > v.StartTime))
                throw new ArgumentException("This time slot overlaps with an existing viewing for this property.");

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
