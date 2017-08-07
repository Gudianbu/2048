using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2048
{
    public partial class messageBox : Form
    {
        public messageBox()
        {
            InitializeComponent();
        }

        public string a { get; set; }
        public string b { get; set; }

        private void messageBox_Load(object sender, EventArgs e)
        {
            label1.Text = a;
            textBox1.Text = b;
            this.TopLevel = true;
        }
        public void b_add(string x)
        {
            textBox1.AppendText(x);
        }

        private void _messageBox_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
        private void _messageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.Close();
        }
    }
}
