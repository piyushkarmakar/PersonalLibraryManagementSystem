using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace PersonalLibraryAPI.Logging
{
    // Extension method visible to ILoggingBuilder
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath, bool append = true)
        {
            builder.AddProvider(new FileLoggerProvider(filePath, append));
            return builder;
        }
    }

    // Provider that creates FileLogger instances
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _filePath;
        private readonly bool _append;

        public FileLoggerProvider(string filePath, bool append)
        {
            _filePath = filePath;
            _append = append;

            var folder = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        public ILogger CreateLogger(string categoryName) => new FileLogger(_filePath, _append);

        public void Dispose() { }
    }

    // Simple rolling file logger (daily file)
    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        private readonly bool _append;

        public FileLogger(string filePath, bool append)
        {
            _filePath = filePath;
            _append = append;
        }

        public IDisposable BeginScope<TState>(TState state) => null!;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId,
                                TState state, Exception exception,
                                Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var datedFile = _filePath.Replace(".txt", $"{DateTime.Now:yyyy-MM-dd}.txt");
            var message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {formatter(state, exception)}";
            if (exception != null) message += Environment.NewLine + exception;

            // Atomic append
            File.AppendAllText(datedFile, message + Environment.NewLine);
        }
    }
}
