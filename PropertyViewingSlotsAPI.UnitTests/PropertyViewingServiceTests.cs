using Property_Viewing_Slots_API.Models;
using Property_Viewing_Slots_API.Services;
using Property_Viewing_Slots_API.Storage;

namespace PropertyViewingSlotsAPI.UnitTests
{
    [TestFixture]
    public class PropertyViewingServiceTests
    {
        private IPropertyViewingStorage _storage = null!;
        private PropertyViewingService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _storage = Substitute.For<IPropertyViewingStorage>();
            _service = new PropertyViewingService(_storage);
        }

        [Test]
        public void BookViewing_ValidSlot_CallsStorageAndReturnsBooking()
        {
            var request = new BookViewingRequest
            {
                PropertyId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                StartTime = new DateTime(2026, 7, 10, 10, 0, 0, DateTimeKind.Utc) // 11:00 BST — within hours
            };
            var expected = new PropertyViewing { Id = Guid.NewGuid(), PropertyId = request.PropertyId, UserId = request.UserId, StartTime = request.StartTime };
            _storage.GetByPropertyAndDateRange(Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns([]);
            _storage.Add(Arg.Any<PropertyViewing>()).Returns(expected);

            var result = _service.BookViewing(request);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void BookViewing_BeforeAllowedHours_ThrowsArgumentException()
        {
            var request = new BookViewingRequest
            {
                PropertyId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                StartTime = new DateTime(2026, 7, 10, 7, 0, 0, DateTimeKind.Utc) // 08:00 BST — before 09:00
            };

            Assert.Throws<ArgumentException>(() => _service.BookViewing(request));
        }

        [Test]
        public void BookViewing_AfterAllowedHours_ThrowsArgumentException()
        {
            var request = new BookViewingRequest
            {
                PropertyId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                StartTime = new DateTime(2026, 7, 10, 19, 30, 0, DateTimeKind.Utc) // 20:30 BST — after 20:00
            };

            Assert.Throws<ArgumentException>(() => _service.BookViewing(request));
        }

        [Test]
        public void BookViewing_OverlappingSlotExists_ThrowsArgumentException()
        {
            var propertyId = Guid.NewGuid();
            var startTime = new DateTime(2026, 7, 10, 10, 0, 0, DateTimeKind.Utc); // 11:00 BST
            var request = new BookViewingRequest { PropertyId = propertyId, UserId = Guid.NewGuid(), StartTime = startTime };

            // Existing viewing starts 15 mins before new one — overlaps
            _storage.GetByPropertyAndDateRange(Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>())
                .Returns([new PropertyViewing { PropertyId = propertyId, StartTime = startTime.AddMinutes(-15) }]);

            Assert.Throws<ArgumentException>(() => _service.BookViewing(request));
        }

        [Test]
        public void BookViewing_AdjacentSlot_DoesNotThrow()
        {
            var propertyId = Guid.NewGuid();
            var startTime = new DateTime(2026, 7, 10, 10, 30, 0, DateTimeKind.Utc); // 11:30 BST
            var request = new BookViewingRequest { PropertyId = propertyId, UserId = Guid.NewGuid(), StartTime = startTime };

            // Existing viewing ends exactly when new one starts — no overlap
            _storage.GetByPropertyAndDateRange(Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>())
                .Returns([new PropertyViewing { PropertyId = propertyId, StartTime = startTime.AddMinutes(-30) }]);
            _storage.Add(Arg.Any<PropertyViewing>()).Returns(new PropertyViewing());

            Assert.DoesNotThrow(() => _service.BookViewing(request));
        }

        [Test]
        public void SearchViewings_DelegatesToStorage()
        {
            var propertyId = Guid.NewGuid();
            var start = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(2026, 7, 31, 0, 0, 0, DateTimeKind.Utc);
            var expected = new List<PropertyViewing> { new() { PropertyId = propertyId } };
            _storage.GetByPropertyAndDateRange(propertyId, start, end).Returns(expected);

            var result = _service.SearchViewings(propertyId, start, end);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
