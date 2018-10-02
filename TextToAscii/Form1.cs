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
            int databits = Convert.ToInt32(cbDatabits.SelectedItem);
            Parity parity = (Parity)cbParity.SelectedItem;
            StopBits stopBits = (StopBits)cbStopBits.SelectedItem;
            Handshake handshake = (Handshake)cbHandshake.SelectedItem;

            try
            {
                using (SerialPort _serialPort
                    = new SerialPort(tbPortName.Text, baudrate, parity, databits, stopBits))
                {
                    // Set the read/write timeouts
                    _serialPort.ReadTimeout = 500;
                    _serialPort.WriteTimeout = 500;

                    _serialPort.Open();

                    _serialPort.Handshake = handshake;

                    _serialPort.Write(txtText.Text.ConvertToASCII());

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
    }
}
