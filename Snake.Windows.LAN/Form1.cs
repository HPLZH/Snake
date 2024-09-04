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

namespace Snake.Windows.LAN
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			string host = hostIP.Text;
			int sPort = int.Parse(serverPort.Text);
			int cPort = int.Parse(clientPort.Text);
			new Form2(host, sPort, cPort, userName.Text).Show();
			okButton.Enabled = false;
		}

		private void label3_Click(object sender, EventArgs e)
		{

		}
	}
}
