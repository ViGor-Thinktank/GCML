using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GenericCampaignMasterLib;


namespace Playground
{
    public partial class frmPlayerMainForm : Form
    {
        public string strMyPlayerID; 

        public frmPlayerMainForm()
        {
            InitializeComponent();            
        }

        
        public void Tick()
        {
            try
            {
                this.clearCommandButtons();


                Player myPlayer = Program.m_objCampaign.getCampaignStateForPlayer(this.strMyPlayerID);

                
                comboBox1.DisplayMember = "Bezeichnung";
                comboBox1.ValueMember = "Id";
                comboBox1.DataSource = myPlayer.ListUnits;
                
                this.txtUnitInfo.Text = "";
                foreach (GenericCampaignMasterLib.clsUnit objUnit in myPlayer.ListUnits)
                {
                    this.txtUnitInfo.Text += objUnit.Bezeichnung + " " + Program.m_objCampaign.getSektorForUnit(objUnit).strUniqueID + Environment.NewLine;
                }

                this.textBox1.Text = "";
                foreach (Sektor x in myPlayer.dicVisibleSectors.Values)
                {
                    this.textBox1.Text += x.strUniqueID + Environment.NewLine;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.strMyPlayerID  = Program.m_objCampaign.addPlayer(txtPlayerName.Text).Id;
            this.Text = this.strMyPlayerID;

            button1.Visible = false;
        }

        void Global_onStatus(string strText)
        {
            frmStatus.Status(strText);
        }

        private int hoffset = 0;

        private void addButton(ICommand aktCommand, ref int offset, string strCommandInfo)
        {
            System.Windows.Forms.Button btnNew = new System.Windows.Forms.Button();
            
            btnNew.Size = new System.Drawing.Size(100, 25);
            
            if (offset > 4)
            {
                offset = 1;
                hoffset += 1;
            }

            btnNew.Location = new System.Drawing.Point((5 + offset * btnNew.Size.Width), 5 + hoffset * btnNew.Size.Height);
            
            btnNew.Name = "movecmd" + offset.ToString();
            btnNew.Tag = aktCommand;
            btnNew.TabIndex = 0;
            btnNew.Text = aktCommand.strInfo.Replace("Move", "").Replace(" ", "").Replace(":", "").Trim();
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += new System.EventHandler(CommandButton_Click);

            if (aktCommand.strInfo == strCommandInfo)
                btnNew.BackColor = Color.LightGreen; 

            this.panel1.Controls.Add(btnNew);
            
        }

        private void CommandButton_Click(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.LightGreen;
            ((ICommand)((Button)sender).Tag).Register();         
        }

        private void erzeugeCommandButtonsForUnit(GenericCampaignMasterLib.clsUnit unit)
        {
            try
            {
                this.clearCommandButtons();

                hoffset = 1;
                int offset = 1;
                foreach (ICommand aktCommand in Program.m_objCampaign.getCommandsForUnit(unit))
                {
                    this.addButton(aktCommand, ref offset, unit.strAktCommandInfo);
                    offset += 1;
                }

                hoffset += 1;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void clearCommandButtons()
        {
            panel1.Controls.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            if (comboBox1.SelectedValue.ToString() != "-1")
            {
                string strUnitID = comboBox1.SelectedValue.ToString();

                GenericCampaignMasterLib.clsUnit Unit = Program.m_objCampaign.getUnit(strUnitID);

                this.erzeugeCommandButtonsForUnit(Unit);

            }
        }

        private void btnAddUNit_Click(object sender, EventArgs e)
        {
            Program.m_objCampaign.createNewUnit(strMyPlayerID, 1);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            try
            {
                Program.m_objCampaign.getPlayer(this.strMyPlayerID).Done();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
