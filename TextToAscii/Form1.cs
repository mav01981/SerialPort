using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace TextToAscii
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            int baudrate = Convert.ToInt32(txtBaudRate.Text);

            try
            {
                using (SerialPort _serialPort
                    = new SerialPort(cbxPorts.SelectedItem.ToString(), baudrate, Parity.None, 8, StopBits.One))
                {
                    // Set the read/write timeouts
                    _serialPort.ReadTimeout = 500;
                    _serialPort.WriteTimeout = 500;

                    _serialPort.Open();

                    _serialPort.WriteLine(txtText.Text.ConvertToASCII());

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
            foreach (string s in SerialPort.GetPortNames())
            {
                cbxPorts.Items.Add(s);
            }
        }

        private void txtBaudRate_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(txtBaudRate.Text, out int result))
            {
                txtBaudRate.Text = string.Empty;
            }
        }
    }
}
