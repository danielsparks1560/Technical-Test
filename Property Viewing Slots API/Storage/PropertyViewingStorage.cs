using Property_Viewing_Slots_API.Models;

namespace Property_Viewing_Slots_API.Storage
{
    public class PropertyViewingStorage : IPropertyViewingStorage
    {
        private readonly List<PropertyViewing> _viewings = [];

        public PropertyViewing Add(PropertyViewing viewing)
        {
            viewing.Id = Guid.NewGuid();
            _viewings.Add(viewing);
            return viewing;
        }

        public IEnumerable<PropertyViewing> GetByPropertyAndDateRange(Guid propertyId, DateTime start, DateTime end)
        {
            return _viewings.Where(v => v.PropertyId == propertyId && v.StartTime >= start && v.StartTime <= end);
        }
    }
}
