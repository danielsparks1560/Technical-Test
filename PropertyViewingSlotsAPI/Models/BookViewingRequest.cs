namespace Property_Viewing_Slots_API.Models
{
    public class BookViewingRequest
    {
        public Guid PropertyId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
    }
}
