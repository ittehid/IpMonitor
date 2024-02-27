using System;
using System.IO;
using System.Threading.Tasks;

namespace IpMonitor
{
    public class Logger
    {
        private string logFolderPath;

        public Logger()
        {
            // Определите путь к папке для хранения логов в папке "IpMonitor/log"
            logFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IPMonitor", "logs");

            // Убедитесь, что папка существует, иначе создайте ее
            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }
        }

        private string GetLogFilePath()
        {
            // Формируйте имя файла на основе текущей даты
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            return Path.Combine(logFolderPath, $"{currentDate}.log");
        }

        public async Task LogPingEventAsync(string ipAddress, string logText)
        {
            try
            {
                string logFilePath = GetLogFilePath();

                using (StreamWriter logStreamWriter = File.AppendText(logFilePath))
                {
                    string logMessage = $"{DateTime.Now}: {ipAddress} {logText}";
                    await logStreamWriter.WriteLineAsync(logMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи в лог: {ex.Message}");
            }
        }

        public void DeleteOldLogFiles()
        {            
            // Проверяем существование папки
            if (Directory.Exists(logFolderPath))
            {
                // Получаем все файлы в папке
                string[] logFiles = Directory.GetFiles(logFolderPath);

                // Текущее время
                DateTime now = DateTime.Now;

                foreach (string logFile in logFiles)
                {
                    try
                    {
                        // Время создания лог файла
                        DateTime creationTime = File.GetCreationTime(logFile);

                        // Проверяем, прошло ли более 5 дней с момента создания файла
                        if ((now - creationTime).TotalDays > 5)
                        {
                            // Удаляем файл
                            File.Delete(logFile);
                        }
                    }
                    catch (Exception ex)
                    {                        
                        Console.WriteLine($"Ошибка при удалении файла лога {logFile}: {ex.Message}");
                    }
                }
            }
        }
    }
}