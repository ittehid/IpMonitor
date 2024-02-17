using System;
using System.Windows.Forms;

namespace IpMonitor
{
    public partial class AddForm : Form
    {
        public string hostName { get; set; }
        public string ipAdress { get; set; }
        public AddForm()
        {
            InitializeComponent();
            //Окно всегда поверх всех окон
            TopMost = true;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            hostName = nameTextBox.Text;
            ipAdress = ipTextBox.Text;
            DialogResult = DialogResult.OK;
        }

        private void CanсelButton_Click(object sender, EventArgs e)
        {
            // Закрываем форму с результатом DialogResult.Cancel
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}