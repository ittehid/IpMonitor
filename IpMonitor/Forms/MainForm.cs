using IpMonitor.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace IpMonitor
{
    public partial class MainForm : Form
    {
        private List<DataItem> dataItems = new List<DataItem>();
        private bool isPinging = false;
        private Logger logger;
        public MainForm()
        {
            InitializeComponent();
            //Окно всегда поверх всех окон
            TopMost = true;

            dataGridView.Columns.Add("HostName", "Название");
            dataGridView.Columns.Add("IpName", "IP адрес");

            // Инициализация логгера с указанием имени файла
            logger = new Logger();

            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(Width, Height);
            // Максимальная высота - может быть любой
            MaximumSize = new Size(Width, int.MaxValue);

            pingTimer = new Timer
            {
                Interval = 5000 // Интервал в миллисекундах
            };
            pingTimer.Tick += async (s, eventArgs) => await PingHostsAsync();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AddForm add = new AddForm();
            if (add.ShowDialog() == DialogResult.OK)
            {
                // Получение данных из формы AddForm
                string hostName = add.hostName;
                string ipName = add.ipAdress;

                dataItems.Add(new DataItem { HostName = hostName, IpAddress = ipName });
                dataGridView.Rows.Add(hostName, ipName);
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Проверяем, есть ли выбранная ячейка в DataGridView
            if (dataGridView.SelectedCells.Count > 0)
            {
                // Получаем индекс выбранной строки и выбранной ячейки
                int selectedRowIndex = dataGridView.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView.Rows[selectedRowIndex];

                // Получаем старые значения хоста и IP-адреса из выбранной строки
                string oldHostName = selectedRow.Cells["HostName"].Value.ToString();
                string oldIpAddress = selectedRow.Cells["IpName"].Value.ToString();

                // Создаем экземпляр формы редактирования и передаем старые значения
                EditForm editForm = new EditForm(oldHostName, oldIpAddress);

                // Открываем форму редактирования
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // Получаем новые значения хоста и IP-адреса после редактирования
                    string newHostName = editForm.HostName;
                    string newIpAddress = editForm.IpAddress;

                    // Обновляем данные в DataGridView
                    selectedRow.Cells["HostName"].Value = newHostName;
                    selectedRow.Cells["IpName"].Value = newIpAddress;

                    // Обновляем данные в объекте DataItem
                    DataItem selectedItem = dataItems[selectedRowIndex];
                    selectedItem.HostName = newHostName;
                    selectedItem.IpAddress = newIpAddress;

                    // Сохраняем обновленные данные в XML
                    SaveToXml("data.xml");
                }
            }
        }

        private void SaveToXml(string filePath)
        {
            // Путь к папке "IpMonitor"
            string folderPath = Path.Combine(Path.GetDirectoryName(filePath), "IPMonitor");

            // Проверяем, существует ли папка "IpMonitor", и создаем ее, если нет
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Полный путь к файлу "data.xml"
            string fullPath = Path.Combine(folderPath, "data.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(List<DataItem>));
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                serializer.Serialize(stream, dataItems);
            }
        }

        private void LoadFromXml(string filePath)
        {
            // Путь к папке "IpMonitor"
            string folderPath = Path.Combine(Path.GetDirectoryName(filePath), "IPMonitor");

            // Полный путь к файлу "data.xml"
            string fullPath = Path.Combine(folderPath, "data.xml");

            if (File.Exists(fullPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<DataItem>));
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    dataItems = (List<DataItem>)serializer.Deserialize(stream);
                }

                foreach (var item in dataItems)
                {
                    dataGridView.Rows.Add(item.HostName, item.IpAddress);
                }
            }
        }

        private async void DataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //вызываем меню и подключаемся по RDP
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dataGridView.ClearSelection();
                dataGridView.Rows[e.RowIndex].Selected = true;

                ContextMenuStrip menu = new ContextMenuStrip();
                ToolStripMenuItem item = new ToolStripMenuItem("Подключиться по RDP")
                {
                    //добавляем иконку к пункту меню Подключиться по RDP
                    Image = Properties.Resources.RDPlogo.ToBitmap()
                };
                item.Click += (s, ev) => OpenRdp(dataGridView.Rows[e.RowIndex].Cells["IpName"].Value.ToString());
                
                menu.Items.Add(item);
                menu.Show(Cursor.Position);
                //записываем в лог, что подключались по RDP
                string ip = dataGridView.Rows[e.RowIndex].Cells["IpName"].Value.ToString();
                string logMessage = $"{DateTime.Now}: подключение по RDP к {ip}";
                await logger.WriteToLogAsync(logMessage);
            }
        }

        private void OpenRdp(string ipAddress)
        {
            Process.Start("mstsc", $"/v:{ipAddress}");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string filePath = "data.xml";
            LoadFromXml(filePath);
            // Очистить выделение в DataGridView при запуске приложения
            dataGridView.ClearSelection();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string filePath = "data.xml";
            SaveToXml(filePath);
        }

        private void DelButton_Click(object sender, EventArgs e)
        {
            // Проверяем, есть ли выбранная ячейка в DataGridView
            if (dataGridView.SelectedCells.Count > 0)
            {
                // Получаем индекс выбранной строки и выбранной ячейки
                int selectedRowIndex = dataGridView.SelectedCells[0].RowIndex;

                // Получаем значение из первого столбца выбранной строки
                string columnName = dataGridView.Rows[selectedRowIndex].Cells[0].Value.ToString();

                // Выводим диалоговое окно для подтверждения удаления
                DialogResult result = MessageBox.Show($"Вы уверены, что хотите удалить \"{columnName}\"?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Удаляем выбранную строку из DataGridView
                    dataGridView.Rows.RemoveAt(selectedRowIndex);

                    // Удаляем соответствующий объект из коллекции (dataItems)
                    dataItems.RemoveAt(selectedRowIndex);

                    // Сохраняем обновленные данные в XML
                    SaveToXml("data.xml");
                }
            }
        }

        private async Task PingHostsAsync()
        {
            List<Task> pingTasks = new List<Task>();

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                string ipAddress = row.Cells["IpName"].Value.ToString();
                pingTasks.Add(PingAndColorAsync(row, ipAddress));
            }

            //ждем выполнение всех задач, и только потом записываем в лог
            await Task.WhenAll(pingTasks);
        }

        private async Task PingAndColorAsync(DataGridViewRow row, string ipAddress)
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = await ping.SendPingAsync(ipAddress);

                bool isHostReachable = reply.Status == IPStatus.Success;

                

                if (isHostReachable)
                {
                    row.Cells["IpName"].Style.BackColor = Color.Green;
                }
                else
                {
                    row.Cells["IpName"].Style.BackColor = Color.Red;
                    string logText = "недоступен";
                    await logger.LogPingEventAsync(ipAddress, logText);
                }                
            }
            catch (Exception ex)
            {
                // Обработайте ошибку, если что-то пошло не так
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RunButton_Click(object sender, EventArgs e)
        {
            if (!isPinging)
            {
                isPinging = true;
                runButton.Text = "Остановить";

                addButton.Enabled = false;
                editButton.Enabled = false;
                delButton.Enabled = false;

                // Запустите таймера
                pingTimer.Start();

                // Запустите асинхронный метод для пинга в фоновом потоке
                await PingHostsAsync();
                await logger.DeleteOldLogFiles();
            }
            else
            {
                isPinging = false;
                runButton.Text = "Запустить";

                addButton.Enabled = true;
                editButton.Enabled = true;
                delButton.Enabled = true;

                // Остановите таймер
                pingTimer.Stop();

            }
        }
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            dataGridView.ClearSelection();
        }        
    }
}