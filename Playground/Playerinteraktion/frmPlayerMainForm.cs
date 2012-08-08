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
        public Player myPlayer; 

        public frmPlayerMainForm()
        {
            InitializeComponent();            
        }

        
        public void Tick()
        {
            try
            {
                this.clearCommandButtons();

                string strPlayerData = Program.m_objCampaign.getCampaignStateForPlayer(myPlayer.Id);

                myPlayer = Player.FromString(strPlayerData);

                
                    comboBox1.DisplayMember = "Bezeichnung";
                    comboBox1.ValueMember = "Id";

                    comboBox1.DataSource = myPlayer.ListUnits;
                
                this.txtUnitInfo.Text = "";
                foreach (BaseUnit objUnit in myPlayer.ListUnits)
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
            
            myPlayer = Program.m_objCampaign.addPlayer(txtPlayerName.Text);
            this.Text = myPlayer.Playername;

            
            button1.Visible = false;
        }

        void Global_onStatus(string strText)
        {
            frmStatus.Status(strText);
        }

        private int hoffset = 0;

        private void addButton(ICommand aktCommand, ref int offset)
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

            this.panel1.Controls.Add(btnNew);
            
        }

        private void CommandButton_Click(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.LightGreen;
            ((ICommand)((Button)sender).Tag).Execute();         
        }

        private void erzeugeCommandButtonsForUnit(BaseUnit unit)
        {
            try
            {
                this.clearCommandButtons();

                hoffset = 1;
                int offset = 1;
                foreach (ICommand aktCommand in Program.m_objCampaign.getCommandsForUnit(unit))
                {
                    this.addButton(aktCommand, ref offset);
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

                foreach (BaseUnit unit in myPlayer.ListUnits)
                {
                    if (unit.Id == strUnitID)
                    {
                        this.erzeugeCommandButtonsForUnit(unit);
                        break;
                    }
                }
            }
        }

        private void btnAddUNit_Click(object sender, EventArgs e)
        {
            Program.m_objCampaign.createNewUnit(myPlayer.Id, typeof(DummyUnit));
        }

    }
}
