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
    

    public partial class frmStatus : Form
    {
        public static frmStatus frmStat;
        public static void Status(string strStatus)
        {
            if (frmStat == null)
            {
                frmStat = new frmStatus();
                frmStat.Show();
            }
            //else
              //  frmStat.BringToFront();

            frmStat.textBox1.Text = strStatus + Environment.NewLine + frmStat.textBox1.Text;
        }

        public frmStatus()
        {
            if (frmStat != null)
                this.Close();
            else
                InitializeComponent();
        }

        private void frmStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmStat = null;
        }

    }
}
