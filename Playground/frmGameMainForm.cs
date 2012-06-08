using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Playground
{
    public partial class frmGameMainForm : Form
    {
        public frmGameMainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new frmPlayerMainForm().Show();
        }
    }
}
