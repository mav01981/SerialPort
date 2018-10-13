using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextToAscii
{
    public partial class FrmSave : Form
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                value = name;
            }
        }

        public FrmSave()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = txtbxName.Text;

            this.Close();
        }
    }
}
