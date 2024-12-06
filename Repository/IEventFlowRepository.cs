using EventFlow.Models;

namespace EventFlow.Repository {
    public interface IEventFlowRepository {
        public Task<int> GetUserId(string userEmail);
        public Task<List<Reservation>> GetEventReservation(int userId);
        public Task<List<Event>> GetAllEvents();
        public Task<int> AddUser(string userName, string userEmail);
        public Task<int> AddReservation(string reservationTime, string reservationDate, string status, int userId, int eventId);
        public Task DeleteReservation(int reservationId);
        public Task<Reservation> GetReservation(int reservationId);
        public Task UpdateReservation(int reservationId, string reservationTime, string reservationDate, string status);
    }
}
