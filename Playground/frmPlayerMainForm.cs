using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GenericCampaignMasterLib;
using GenericCampaignMasterLib.Code.Unit;


namespace Playground
{
    public partial class frmPlayerMainForm : Form
    {
        Player myPlayer; 

        public frmPlayerMainForm()
        {
            InitializeComponent();            
        }        

        private void button1_Click(object sender, EventArgs e)
        {
            
            myPlayer = Program.m_objCampaign.addPlayer(txtPlayerName.Text);
            this.Text = myPlayer.Playername;
            myPlayer.createNewUnit(typeof(DummyUnit));

            myPlayer.getGameState();

            
            raiseTick();
           
        }

        void Global_onStatus(string strText)
        {
            frmStatus.Status(strText);
        }

        private int hoffset = 0;

        private void addButton(ICommand aktCommand, ref int offset)
        {
            System.Windows.Forms.Button btnNew = new System.Windows.Forms.Button();
            
            btnNew.Size = new System.Drawing.Size(65, 25);
            if (offset > 5)
            {
                offset = 1;
                hoffset += 1;
            }
            btnNew.Location = new System.Drawing.Point((12 + offset * btnNew.Size.Width), 50 + hoffset * btnNew.Size.Height);
            
            btnNew.Name = "movecmd" + offset.ToString();
            btnNew.Tag = aktCommand;
            btnNew.TabIndex = 0;
            btnNew.Text = aktCommand.strInfo.Replace("Move", "").Replace(":", "").Trim();
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += new System.EventHandler(button_Click);

            this.Controls.Add(btnNew);
            
        }

        private void button_Click(object sender, EventArgs e)
        {
            ((ICommand)((Button)sender).Tag).Execute();
            
        }

        private void raiseTick()
        {
            this.Controls.Clear();

            myPlayer.getGameState();

            hoffset = 1;

            int offset = 1;
            
            foreach (ICommand aktCommand in this.myPlayer.lisCommands)
            {
                this.addButton(aktCommand, ref offset);
                offset += 1;
            }

            hoffset += 1;
        }

    }
}
