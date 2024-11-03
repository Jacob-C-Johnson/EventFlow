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
                            reservationDetails.append(reservationTime);
                            reservationDetails.append(reservationDate);

                            return reservationDetails;
                        } else {
                            throw new Exception("No reservation found for this user.");
                        }
                    }
                }
            }
        }
    }
}
