using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake.Client.LAN
{
    public partial class Form2 : Form
    {
        


        public Form2(IPAddress host, int serverPort, int clientPort, string name)
        {
            InitializeComponent();

        }
    }
}
