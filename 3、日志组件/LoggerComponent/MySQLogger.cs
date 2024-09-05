using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace LoggerComponent
{
    internal class MySQLogger : ILogger
    {
        public string _connectionString { get; set; }

        public MySQLogger(string connectionString) 
        {
            this._connectionString = connectionString;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);

            using (var connection = new MySqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();

                command.CommandText = "INSERT INTO Logs (Message, LogLevel, CreatedTime) VALUES (@message, @logLevel, @createdTime)";
                command.Parameters.AddWithValue("@message", message);
                command.Parameters.AddWithValue("@logLevel", logLevel.ToString());
                command.Parameters.AddWithValue("@createdTime", DateTime.UtcNow);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
