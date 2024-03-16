using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public async Task WriteToLogAsync(string logMessage)
        {
            //отдельный метод для записи в лог
            try
            {
                string logFilePath = GetLogFilePath();

                using (StreamWriter logStreamWriter = File.AppendText(logFilePath))
                {
                    await logStreamWriter.WriteLineAsync(logMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при записи в лог: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task LogPingEventAsync(string ipAddress, string logText)
        {
            string logMessage = $"{DateTime.Now}: {ipAddress} {logText}";
            await WriteToLogAsync(logMessage);
        }

        public async Task DeleteOldLogFilesAsync()
        {
            // Проверяем существование папки
            if (Directory.Exists(logFolderPath))
            {
                // Получаем все файлы в папке
                string[] logFiles = Directory.GetFiles(logFolderPath);

                for (int i = 0; i < logFiles.Length; i++)
                {
                    try
                    {
                        // Получаем название лог файлов
                        string fileName = Path.GetFileNameWithoutExtension(logFiles[i]);
                        // Извлекаем дату из имени файла
                        DateTime dt = DateTime.ParseExact(fileName, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        // Если дата лог файла меньше текущей даты на 5 дней, удаляем файл лога
                        if (dt < DateTime.Now.AddDays(-5))
                        {
                            File.Delete(logFiles[i]);
                            //записываем в лог, что удалили
                            string logMessage = $"{DateTime.Now}: удален файл лога {Path.GetFileName(logFiles[i])}";
                            await WriteToLogAsync(logMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении файла лога {Path.GetFileName(logFiles[i])}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}