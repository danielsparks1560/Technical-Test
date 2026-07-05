namespace Property_Viewing_Slots_API.Models
{
    public class PropertyViewing
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
    }
}
