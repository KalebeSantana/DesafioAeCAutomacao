using System;
using System.IO;

namespace AluraRpa.Shared
{
    public class Logger
    {
        private readonly string logFilePath;

        public Logger(string logFilePath)
        {
            this.logFilePath = logFilePath;

            if (!Directory.Exists(Path.GetDirectoryName(logFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
            }

            // Verifica se o arquivo de log existe, se não, cria-o.
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Close();
            }
        }

        public void LogInfo(string message)
        {
            LogToFile($"[{DateTime.Now}][INFO] - {message}");
        }

        public void LogWarning(string message)
        {
            LogToFile($"[{DateTime.Now}][WARNING] - {message}");
        }

        public void LogError(string message)
        {
            LogToFile($"[{DateTime.Now}][ERROR] - {message}");
        }

        private void LogToFile(string logMessage)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    sw.WriteLine(logMessage);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao escrever no arquivo de log: {ex.Message}");
            }
        }
    }
}
