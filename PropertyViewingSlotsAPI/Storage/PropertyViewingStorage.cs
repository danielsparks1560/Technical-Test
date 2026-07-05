using System.Text.Json;
using Property_Viewing_Slots_API.Models;

namespace Property_Viewing_Slots_API.Storage
{
    public class PropertyViewingStorage : IPropertyViewingStorage
    {
        private readonly string _filePath;
        private readonly List<PropertyViewing> _viewings;
        private readonly object _lock = new();

        public PropertyViewingStorage(IConfiguration configuration)
        {
            var fileName = configuration["PropertyViewingsFilePath"] ?? "viewings.json";
            _filePath = Path.IsPathRooted(fileName)
                ? fileName
                : Path.Combine(AppContext.BaseDirectory, fileName);
            _viewings = LoadFromFile();
        }

        public PropertyViewing Add(PropertyViewing viewing)
        {
            lock (_lock)
            {
                viewing.Id = Guid.NewGuid();
                _viewings.Add(viewing);
                SaveToFile();
                return viewing;
            }
        }

        public IEnumerable<PropertyViewing> GetByPropertyAndDateRange(Guid propertyId, DateTime start, DateTime end)
        {
            lock (_lock)
            {
                return _viewings
                    .Where(v => v.PropertyId == propertyId && v.StartTime >= start && v.StartTime <= end)
                    .ToList();
            }
        }

        private List<PropertyViewing> LoadFromFile()
        {
            if (!File.Exists(_filePath))
                return [];

            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                return [];

            return JsonSerializer.Deserialize<List<PropertyViewing>>(json) ?? [];
        }

        private void SaveToFile()
        {
            var json = JsonSerializer.Serialize(_viewings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
