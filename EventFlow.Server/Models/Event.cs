namespace EventFlow.Server.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string EventDescription { get; set; }
        public string EventLocation { get; set; }
        public string Title { get; set; }
        public int TotalAttendees { get; set; }
    }
}