using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace TextToAscii
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            LoadSettings();
        }

        void LoadSettings()
        {
            if (File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}settings.xml"))
            {
                var settings = XmlFunction.DeSerializeObject<List<SettingsModel>>($"{AppDomain.CurrentDomain.BaseDirectory}settings.xml");

                loadSettingsToolStripMenuItem.DropDownItems.Clear();

                foreach (var setting in settings)
                {
                    ToolStripItem subItem = new ToolStripMenuItem(setting.Name);
                    subItem.Click += menuClick;
                    loadSettingsToolStripMenuItem.DropDownItems.Add(subItem);
                }
            }
        }

        private void menuClick(object sender, EventArgs e)
        {
            var settings = XmlFunction.DeSerializeObject<List<SettingsModel>>($"{AppDomain.CurrentDomain.BaseDirectory}settings.xml");
            var setting = settings.Where(x => x.Name == ((ToolStripMenuItem)sender).Text).First();

            txtPortName.Text = setting.ComPortName;
            txtBaudRate.Text = setting.BuadRate;
            cbDatabits.SelectedItem = setting.Databits;
            cbStopBits.SelectedItem = setting.Stopbit;
            cbParity.SelectedItem = setting.Parity;
            cbHandshake.SelectedItem = (Handshake)Enum.Parse(typeof(Handshake), setting.Handshake);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            int baudrate = Convert.ToInt32(txtBaudRate.Text);
            int databits = Convert.ToInt32(cbDatabits.SelectedItem);
            Parity parity = (Parity)cbParity.SelectedItem;
            StopBits stopBits = (StopBits)cbStopBits.SelectedItem;
            Handshake handshake = (Handshake)cbHandshake.SelectedItem;

            try
            {
                using (SerialPort _serialPort
                    = new SerialPort(txtPortName.Text, baudrate, parity, databits, stopBits))
                {
                    // Set the read/write timeouts
                    _serialPort.ReadTimeout = 2000;
                    _serialPort.WriteTimeout = 2000;

                    _serialPort.Open();

                    _serialPort.Handshake = handshake;

                    foreach (char @char in txtText.Text.ToCharArray())
                    {
                        _serialPort.Write(@char.ToString());

                        Thread.Sleep(1000);
                    }

                    _serialPort.Close();

                    lbMessage.Text = $"Data sent to Port : {_serialPort.PortName}";
                }
            }
            catch (Exception ex)
            {
                lbMessage.Text = ex.Message;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbParity.DataSource = Enum.GetValues(typeof(Parity));
            cbStopBits.DataSource = Enum.GetValues(typeof(StopBits));
            cbHandshake.DataSource = Enum.GetValues(typeof(Handshake));
        }

        private void txtBaudRate_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(txtBaudRate.Text, out int result))
            {
                txtBaudRate.Text = string.Empty;
            }
        }

        private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSave frm = new FrmSave();
            frm.ShowDialog();

            var model = new SettingsModel()
            {
                Name = frm.Name.ToString(),
                ComPortName = txtPortName.Text,
                BuadRate = txtBaudRate.Text,
                Databits = cbDatabits.SelectedItem.ToString(),
                Stopbit = cbStopBits.SelectedItem.ToString(),
                Parity = cbParity.SelectedItem.ToString(),
                Handshake = cbHandshake.SelectedItem.ToString()
            };

            if (!File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}settings.xml"))
            {
                List<SettingsModel> settings = new List<SettingsModel>();
                settings.Add(model);

                using (File.Create($"{AppDomain.CurrentDomain.BaseDirectory}settings.xml")) { }

                XmlFunction.SerializeObject(settings, $"{AppDomain.CurrentDomain.BaseDirectory}settings.xml");
            }
            else
            {
                var settings = XmlFunction.DeSerializeObject<List<SettingsModel>>($"{AppDomain.CurrentDomain.BaseDirectory}settings.xml");

                settings.Add(model);

                XmlFunction.SerializeObject(settings, $"{AppDomain.CurrentDomain.BaseDirectory}settings.xml");
            }

            LoadSettings();
        }
    }
}
