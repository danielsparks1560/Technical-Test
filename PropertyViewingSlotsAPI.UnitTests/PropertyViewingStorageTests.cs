using Microsoft.Extensions.Configuration;
using Property_Viewing_Slots_API.Models;
using Property_Viewing_Slots_API.Storage;

namespace PropertyViewingSlotsAPI.UnitTests
{
    [TestFixture]
    public class PropertyViewingStorageTests
    {
        private string _tempFilePath = string.Empty;
        private PropertyViewingStorage _storage = null!;

        [SetUp]
        public void SetUp()
        {
            _tempFilePath = Path.GetTempFileName();
            _storage = CreateStorage(_tempFilePath);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_tempFilePath))
                File.Delete(_tempFilePath);
        }

        [Test]
        public void Add_AssignsNewId_AndReturnsViewing()
        {
            var viewing = new PropertyViewing { PropertyId = Guid.NewGuid(), UserId = Guid.NewGuid(), StartTime = DateTime.UtcNow };

            var result = _storage.Add(viewing);

            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void Add_PersistsViewingToFile()
        {
            var propertyId = Guid.NewGuid();
            var startTime = new DateTime(2026, 7, 10, 10, 0, 0, DateTimeKind.Utc);
            _storage.Add(new PropertyViewing { PropertyId = propertyId, UserId = Guid.NewGuid(), StartTime = startTime });

            var reloadedStorage = CreateStorage(_tempFilePath);
            var results = reloadedStorage.GetByPropertyAndDateRange(propertyId, startTime.AddHours(-1), startTime.AddHours(1));

            Assert.That(results.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetByPropertyAndDateRange_ReturnsViewingsWithinRange()
        {
            var propertyId = Guid.NewGuid();
            var startTime = new DateTime(2026, 7, 10, 10, 0, 0, DateTimeKind.Utc);
            _storage.Add(new PropertyViewing { PropertyId = propertyId, UserId = Guid.NewGuid(), StartTime = startTime });

            var results = _storage.GetByPropertyAndDateRange(propertyId, startTime.AddHours(-1), startTime.AddHours(1));

            Assert.That(results.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetByPropertyAndDateRange_ExcludesViewingsOutsideRange()
        {
            var propertyId = Guid.NewGuid();
            var startTime = new DateTime(2026, 7, 10, 10, 0, 0, DateTimeKind.Utc);
            _storage.Add(new PropertyViewing { PropertyId = propertyId, UserId = Guid.NewGuid(), StartTime = startTime });

            var results = _storage.GetByPropertyAndDateRange(propertyId, startTime.AddHours(2), startTime.AddHours(4));

            Assert.That(results, Is.Empty);
        }

        [Test]
        public void GetByPropertyAndDateRange_ExcludesViewingsForDifferentProperty()
        {
            var startTime = new DateTime(2026, 7, 10, 10, 0, 0, DateTimeKind.Utc);
            _storage.Add(new PropertyViewing { PropertyId = Guid.NewGuid(), UserId = Guid.NewGuid(), StartTime = startTime });

            var results = _storage.GetByPropertyAndDateRange(Guid.NewGuid(), startTime.AddHours(-1), startTime.AddHours(1));

            Assert.That(results, Is.Empty);
        }

        private static PropertyViewingStorage CreateStorage(string filePath)
        {
            var config = Substitute.For<IConfiguration>();
            config["PropertyViewingsFilePath"].Returns(filePath);
            return new PropertyViewingStorage(config);
        }
    }
}
