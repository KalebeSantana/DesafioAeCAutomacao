using System;
using System.IO;

namespace AluraRpa.Shared
{
    /// <summary>
    /// Classe para registrar logs em um arquivo de log.
    /// </summary>
    public class Logger
    {
        private readonly string logFilePath;

        /// <summary>
        /// Construtor da classe Logger.
        /// </summary>
        /// <param name="logFilePath">Caminho do arquivo de log.</param>
        public Logger(string logFilePath)
        {
            this.logFilePath = logFilePath;

            // Cria o diretório do arquivo de log, se não existir
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

        /// <summary>
        /// Registra uma mensagem de informação no arquivo de log.
        /// </summary>
        /// <param name="message">Mensagem a ser registrada.</param>
        public void LogInfo(string message)
        {
            LogToFile($"[{DateTime.Now}][INFO] - {message}");
        }

        /// <summary>
        /// Registra uma mensagem de aviso no arquivo de log.
        /// </summary>
        /// <param name="message">Mensagem a ser registrada.</param>
        public void LogWarning(string message)
        {
            LogToFile($"[{DateTime.Now}][WARNING] - {message}");
        }

        /// <summary>
        /// Registra uma mensagem de erro no arquivo de log.
        /// </summary>
        /// <param name="message">Mensagem a ser registrada.</param>
        public void LogError(string message)
        {
            LogToFile($"[{DateTime.Now}][ERROR] - {message}");
        }

        /// <summary>
        /// Registra uma mensagem no arquivo de log.
        /// </summary>
        /// <param name="logMessage">Mensagem a ser registrada.</param>
        private void LogToFile(string logMessage)
        {
            try
            {
                // Adiciona a mensagem ao final do arquivo de log
                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    sw.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro, exibe uma mensagem na console
                Console.WriteLine($"Erro ao escrever no arquivo de log: {ex.Message}");
            }
        }
    }
}
