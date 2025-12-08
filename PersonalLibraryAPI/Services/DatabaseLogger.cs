using Microsoft.Extensions.Logging;
using System;
using Microsoft.Data.SqlClient;

namespace PersonalLibraryAPI.Services
{
    public class DatabaseLogger : ILogger
    {
        private readonly string _connectionString;
        private readonly string _categoryName;

        public DatabaseLogger(string categoryName, string connectionString)
        {
            _categoryName = categoryName;
            _connectionString = connectionString;
        }

        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(
                "INSERT INTO LogEntries (LogLevel, Message, Exception) VALUES (@level, @msg, @ex)", conn);

            cmd.Parameters.AddWithValue("@level", logLevel.ToString());
            cmd.Parameters.AddWithValue("@msg", message);
            cmd.Parameters.AddWithValue("@ex", exception?.ToString() ?? "");

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
