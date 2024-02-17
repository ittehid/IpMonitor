using System;
using System.Windows.Forms;

namespace IpMonitor.Forms
{
    public partial class EditForm : Form
    {
        public string HostName { get; private set; }
        public string IpAddress { get; private set; }
        public EditForm(string oldHostName, string oldIpAddress)
        {
            InitializeComponent();
            //Окно всегда поверх всех окон
            TopMost = true;

            // Установка текущих значений в элементы управления
            nameTextBox.Text = oldHostName;
            ipTextBox.Text = oldIpAddress;
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Получаем новые значения из элементов управления
            HostName = nameTextBox.Text;
            IpAddress = ipTextBox.Text;

            // Закрываем форму с результатом DialogResult.OK
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CanсelButton_Click(object sender, EventArgs e)
        {
            // Закрываем форму с результатом DialogResult.Cancel
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
