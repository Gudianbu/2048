using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _2048
{
    public partial class Game_OverFrom : Form
    {
        public Game_OverFrom()
        {
            InitializeComponent();
        }

        public int gv { get; set; }
        public int bg { get; set; }

        private void Game_OverFrom_Load(object sender, EventArgs e)
        {
            label2.Text += gv;
            label3.Text += bg;
            this.TopLevel = true;
        }

    }
}
