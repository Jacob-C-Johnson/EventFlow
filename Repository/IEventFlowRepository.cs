using EventFlow.Models;

namespace EventFlow.Repository {
    public interface IEventFlowRepository {
        public Task<int> GetUserId(string userEmail);
        public Task<List<string>> GetEventReservation(int userId);
        public Task<List<(int EventId, string EventLocation, string EventDescription, int TotalAttendees, string Title)>> GetAllEvents();
        public Task<int> AddUser(string userName, string userEmail);
        public Task<int> AddReservation(string reservationTime, string reservationDate, string status, int userId, int eventId);
        public Task DeleteReservation(int reservationId);
        public Task<Reservation> GetReservation(int reservationId);
            }
}
