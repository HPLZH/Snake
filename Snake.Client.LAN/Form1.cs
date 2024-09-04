using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace Snake.Client.LAN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            IPAddress host = IPAddress.Parse(hostIP.Text);
            int sPort = int.Parse(serverPort.Text);
            int cPort = int.Parse(clientPort.Text);
        }
    }
}
