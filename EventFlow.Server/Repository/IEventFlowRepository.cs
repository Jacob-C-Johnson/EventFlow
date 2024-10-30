using EventFlow.Server.Models;

namespace EventFlow.Repository {
    public interface IEventFlowRepository {
        public Task<int> GetUserId(string userEmail);
        public Task<DateTime> GetEventReservation(int userId);
    }
}
