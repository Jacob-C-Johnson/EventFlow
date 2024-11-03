using MySql.Data.MySqlClient;

namespace EventFlow.Repository {
    public class EventFlowRepository : IEventFlowRepository {
        private readonly string _connectionString = "server=localhost;userid=eventFlowAdmins;password=Ev3ntf10wg0@ts;database=event_flow";

        public async Task<int> GetUserId (string userEmail) {
            int userId;
            using (MySqlConnection _connection = new MySqlConnection(_connectionString)) {
                await _connection.OpenAsync();
                var statement = "SELECT user_id FROM user WHERE user_email = @userEmail";
                using (MySqlCommand command = new MySqlCommand(statement, _connection)) {
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.String) {Value = userEmail});
                    var result = await command.ExecuteScalarAsync();

                    if (result != null) {
                        userId = (int) result;
                    } else {
                        userId = -1;
                    }
                }
            } return userId;
        }

        public async Task<List<string>> GetEventReservation(int userId) {
            using (MySqlConnection _connection = new MySqlConnection(_connectionString)) {
                List<string> reservationDetails = new List<string>();
                await _connection.OpenAsync();
                var statement1 = "SELECT reservation_time, reservation_date FROM reservation WHERE User_user_id = @userEmail";
                using (MySqlCommand command = new MySqlCommand(statement1, _connection)) {
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar, 100) {Value = userId});
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync()) {
                        if (await reader.ReadAsync()) {
                            var reservationTime = reader.GetString(reader.GetOrdinal("reservation_time"));
                            var reservationDate = reader.GetString(reader.GetOrdinal("reservation_date"));
                            reservationDetails.Add(reservationTime);
                            reservationDetails.Add(reservationDate);

                            return reservationDetails;
                        } else {
                            throw new Exception("No reservation found for this user.");
                        }
                    }
                }
            }
        }

        public async Task<List<(int EventId, string EventLocation, string EventDescription, int TotalAttendees, string Title)>> GetAllEvents()
        {
            var events = new List<(int, string, string, int, string)>();
            using (MySqlConnection _connection = new MySqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                var statement = "SELECT event_id, event_location, event_description, total_attendees, title FROM event";
                using (MySqlCommand command = new MySqlCommand(statement, _connection))
                {
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var eventId = reader.GetInt32(reader.GetOrdinal("event_id"));
                            var eventLocation = reader.GetString(reader.GetOrdinal("event_location"));
                            var eventDescription = reader.GetString(reader.GetOrdinal("event_description"));
                            var totalAttendees = reader.GetInt32(reader.GetOrdinal("total_attendees"));
                            var title = reader.GetString(reader.GetOrdinal("title"));
        
                            events.Add((eventId, eventLocation, eventDescription, totalAttendees, title));
                        }
                    }
                }
            }
            return events;
        }
        
        public async Task<int> AddUser(string userName, string userEmail)
        {
            using (MySqlConnection _connection = new MySqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                var statement = "INSERT INTO user (user_name, user_email) VALUES (@userName, @userEmail)";
                using (MySqlCommand command = new MySqlCommand(statement, _connection))
                {
                    command.Parameters.Add(new MySqlParameter("@userName", MySqlDbType.VarChar, 45) { Value = userName });
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar, 45) { Value = userEmail });
                    await command.ExecuteNonQueryAsync();
                    return (int)command.LastInsertedId;
                }
            }
        }
        
        public async Task<int> AddReservation(string reservationTime, string reservationDate, string status, int userId, int eventId)
        {
            using (MySqlConnection _connection = new MySqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                var statement = "INSERT INTO reservation (reservation_time, reservation_date, status, User_user_id, Event_event_id) VALUES (@reservationTime, @reservationDate, @status, @userId, @eventId)";
                using (MySqlCommand command = new MySqlCommand(statement, _connection))
                {
                    command.Parameters.Add(new MySqlParameter("@reservationTime", MySqlDbType.VarChar, 45) { Value = reservationTime });
                    command.Parameters.Add(new MySqlParameter("@reservationDate", MySqlDbType.VarChar, 45) { Value = reservationDate });
                    command.Parameters.Add(new MySqlParameter("@status", MySqlDbType.VarChar, 45) { Value = status });
                    command.Parameters.Add(new MySqlParameter("@userId", MySqlDbType.Int32) { Value = userId });
                    command.Parameters.Add(new MySqlParameter("@eventId", MySqlDbType.Int32) { Value = eventId });
                    await command.ExecuteNonQueryAsync();
                    return (int)command.LastInsertedId;
                }
            }
        }
        
        public async Task DeleteReservation(int reservationId)
        {
            using (MySqlConnection _connection = new MySqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                var statement = "DELETE FROM reservation WHERE reservation_id = @reservationId";
                using (MySqlCommand command = new MySqlCommand(statement, _connection))
                {
                    command.Parameters.Add(new MySqlParameter("@reservationId", MySqlDbType.Int32) { Value = reservationId });
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        throw new Exception("No reservation found with the provided ID.");
                    }
                }
            }
        }
        
        public async Task<List<(int ReservationId, string ReservationTime, string ReservationDate, string Status, int UserId, int EventId)>> GetReservation(int reservationId)
        {
            using (MySqlConnection _connection = new MySqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                var statement = "SELECT reservation_id, reservation_time, reservation_date, status, User_user_id, Event_event_id FROM reservation WHERE reservation_id = @reservationId";
                using (MySqlCommand command = new MySqlCommand(statement, _connection))
                {
                    command.Parameters.Add(new MySqlParameter("@reservationId", MySqlDbType.Int32) { Value = reservationId });
        
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var reservationTime = reader.GetString(reader.GetOrdinal("reservation_time"));
                            var reservationDate = reader.GetString(reader.GetOrdinal("reservation_date"));
                            var status = reader.GetString(reader.GetOrdinal("status"));
                            var userId = reader.GetInt32(reader.GetOrdinal("User_user_id"));
                            var eventId = reader.GetInt32(reader.GetOrdinal("Event_event_id"));
        
                            return (reservationId, reservationTime, reservationDate, status, userId, eventId);
                        }
                        else
                        {
                            throw new Exception("No reservation found with the provided ID.");
                        }
                    }
                }
            }
        }
    }
}
