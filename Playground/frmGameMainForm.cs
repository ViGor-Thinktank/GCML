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

        private List<frmPlayerMainForm> lisForms = new List<frmPlayerMainForm>();

        private void button1_Click(object sender, EventArgs e)
        {
            frmPlayerMainForm frm = new frmPlayerMainForm();
            lisForms.Add(frm);
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.m_objCampaign.Plenk();

            return;
            foreach (frmPlayerMainForm aktForm in lisForms)
            {
                aktForm.Tick();
            }
        }
    }
}
