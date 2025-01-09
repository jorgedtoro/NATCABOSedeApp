namespace NATCABOSede.Utilities
{
    using System;
    using System.IO;

    public static class Logger
    {
        private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        static Logger()
        {
            // Ensure the Logs directory exists
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }
        }

        public static void LogError(string message, Exception ex)
        {
            string logFilePath = Path.Combine(LogDirectory, $"{DateTime.Now:yyyy-MM-dd}.log");

            // Format the log message
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}\n{ex}\n";

            // Append the log message to the file
            File.AppendAllText(logFilePath, logMessage);
        }

        public static void LogInfo(string message)
        {
            string logFilePath = Path.Combine(LogDirectory, $"{DateTime.Now:yyyy-MM-dd}.log");

            // Format the log message
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] INFO: {message}\n";

            // Append the log message to the file
            File.AppendAllText(logFilePath, logMessage);
        }
    }

}
