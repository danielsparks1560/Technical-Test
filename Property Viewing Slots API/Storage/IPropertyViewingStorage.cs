using Property_Viewing_Slots_API.Models;

namespace Property_Viewing_Slots_API.Storage
{
    public interface IPropertyViewingStorage
    {
        PropertyViewing Add(PropertyViewing viewing);
        IEnumerable<PropertyViewing> GetByPropertyAndDateRange(Guid propertyId, DateTime start, DateTime end);
    }
}
