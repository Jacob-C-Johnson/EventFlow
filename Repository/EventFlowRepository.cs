using EventFlow.Models;
using MySql.Data.MySqlClient;

namespace EventFlow.Repository {
    public class EventFlowRepository : IEventFlowRepository {
        private readonly string _connectionString = "server=sql5.freesqldatabase.com;database=sql5749035;user=sql5749035;password=JzRRxAi2Xk";

        public async Task<int> GetUserId (string userEmail) {
            int userId;
            using (MySqlConnection _connection = new MySqlConnection(_connectionString)) {
                await _connection.OpenAsync();
                var statement = "SELECT user_id FROM User WHERE user_email = @userEmail";
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

        public async Task<List<Reservation>> GetEventReservation(int userId) {
            using (MySqlConnection _connection = new MySqlConnection(_connectionString)) {
                var reservations = new List<Reservation>();
                await _connection.OpenAsync();
                var statement = "SELECT reservation_id, reservation_time, reservation_date, status, Event_event_id FROM Reservation WHERE User_user_id = @userId";
                using (MySqlCommand command = new MySqlCommand(statement, _connection)) {
                    command.Parameters.Add(new MySqlParameter("@userId", MySqlDbType.Int32) { Value = userId });
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync()) {
                        while (await reader.ReadAsync()) {
                            reservations.Add(new Reservation {
                                ReservationTime = reader.GetString("reservation_time"),
                                ReservationDate = reader.GetString("reservation_date"),
                                Status = reader.GetString("status"),
                                UserId = userId,
                                EventId = reader.GetInt32("Event_event_id")
                            });
                        }
                    }
                }
                return reservations;
            }
        }

        public async Task<List<Event>> GetAllEvents()
        {
            var events = new List<Event>();
            using (MySqlConnection _connection = new MySqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                var statement = "SELECT event_id, event_location, event_description, total_attendees, title FROM Event";
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

                            Event tempEvent = new Event
                            {
                                EventId = eventId,
                                EventLocation = eventLocation,
                                EventDescription = eventDescription,
                                TotalAttendees = totalAttendees,
                                Title = title
                            };
                            events.Add(tempEvent);
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
                var statement = "INSERT INTO User (user_name, user_email) VALUES (@userName, @userEmail)";
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
                var statement = "INSERT INTO Reservation (reservation_time, reservation_date, status, User_user_id, Event_event_id) VALUES (@reservationTime, @reservationDate, @status, @userId, @eventId)";
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
                var statement = "DELETE FROM Reservation WHERE reservation_id = @reservationId";
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
        
        public async Task<Reservation> GetReservation(int reservationId)
        {
            using (MySqlConnection _connection = new MySqlConnection(_connectionString))
            {
        
        
                await _connection.OpenAsync();
                var statement = "SELECT reservation_id, reservation_time, reservation_date, status, User_user_id, Event_event_id FROM Reservation WHERE reservation_id = @reservationId";
                using (MySqlCommand command = new MySqlCommand(statement, _connection))
                {
                    command.Parameters.Add(new MySqlParameter("@reservationId", MySqlDbType.Int32) { Value = reservationId });
        
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var reservationDetails = new Reservation
                            {
                                ReservationTime = reader.GetString(reader.GetOrdinal("reservation_time")),
                                ReservationDate = reader.GetString(reader.GetOrdinal("reservation_date")),
                                Status = reader.GetString(reader.GetOrdinal("status")),
                                UserId = reader.GetInt32(reader.GetOrdinal("User_user_id")),
                                EventId = reader.GetInt32(reader.GetOrdinal("Event_event_id"))
                            };
                            return reservationDetails;
                        }
                        else
                        {
                            throw new Exception("No reservation found with the provided ID.");
                        }
                    }
                }
            }
        }
        public async Task UpdateReservation(int reservationId, string reservationTime, string reservationDate, string status)
        {
            using (MySqlConnection _connection = new MySqlConnection(_connectionString))
            {
                await _connection.OpenAsync();

                const string statement = @"
                    UPDATE Reservation 
                    SET 
                        reservation_time = @reservationTime, 
                        reservation_date = @reservationDate, 
                        status = @status 
                    WHERE reservation_id = @reservationId";

                using (MySqlCommand command = new MySqlCommand(statement, _connection))
                {
                    command.Parameters.Add(new MySqlParameter("@reservationId", MySqlDbType.Int32) { Value = reservationId });
                    command.Parameters.Add(new MySqlParameter("@reservationTime", MySqlDbType.VarChar, 45) { Value = reservationTime });
                    command.Parameters.Add(new MySqlParameter("@reservationDate", MySqlDbType.VarChar, 45) { Value = reservationDate });
                    command.Parameters.Add(new MySqlParameter("@status", MySqlDbType.VarChar, 45) { Value = status });

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"No reservation found with ID {reservationId}.");
                    }
                }
            }
        }

    }
}
