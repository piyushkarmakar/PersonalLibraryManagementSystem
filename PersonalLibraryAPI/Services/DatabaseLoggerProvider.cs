using Microsoft.Extensions.Logging;

namespace PersonalLibraryAPI.Services
{
    public class DatabaseLoggerProvider : ILoggerProvider
    {
        private readonly string _connectionString;

        public DatabaseLoggerProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger(categoryName, _connectionString);
        }

        public void Dispose() { }
    }
}
