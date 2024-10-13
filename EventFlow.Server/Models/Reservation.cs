namespace EventFlow.Server.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public string ReservationTime { get; set; }
        public string ReservationDate { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}