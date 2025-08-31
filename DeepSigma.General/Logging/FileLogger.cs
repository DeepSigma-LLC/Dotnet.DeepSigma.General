using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.Logging
{
    /// <summary>
    /// A logger that writes log messages to a file.
    /// </summary>
    public class FileLogger : ILogger
    {
        protected readonly FileLoggerProvider _provider;

        public FileLogger(FileLoggerProvider provider)
        {
            _provider = provider;

        }

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if the given log level is enabled.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if(!IsEnabled(logLevel))
            {
                return;
            }

            string full_file_path = Path.Combine(_provider.Options.FolderPath, _provider.Options.FilePath);

            string log_record = string.Format("{0} - [{1}] EventId: {2} State: {3} Exception: {4}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                logLevel.ToString().ToUpper(),
                eventId.Id,
                formatter(state, exception),
                exception != null ? Environment.NewLine + exception.ToString() + Environment.NewLine + exception.StackTrace : string.Empty);


            using(var stream = new StreamWriter(full_file_path, true))
            {
                stream.WriteLine(log_record);
            }
        }
    }
}
