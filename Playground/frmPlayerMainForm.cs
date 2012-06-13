﻿using System;
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
        Player myPlayer; 

        public frmPlayerMainForm()
        {
            InitializeComponent();            
        }

        private CampaignState m_aktState = null;

        public void Tick()
        {
            this.m_aktState = Program.m_objCampaign.getCampaignStateForPlayer(myPlayer.Id);

            //this.erzeugeCommandButtonsForUnit(myPlayer.ListUnits[0]);
            }

        private void button1_Click(object sender, EventArgs e)
        {
            
            myPlayer = Program.m_objCampaign.addPlayer(txtPlayerName.Text);
            this.Text = myPlayer.Playername;

            Program.m_objCampaign.createNewUnit(myPlayer.Id, typeof(DummyUnit));
            
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
            btnNew.Click += new System.EventHandler(CommandButton_Click);

            this.Controls.Add(btnNew);
            
        }

        private void CommandButton_Click(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.LightGreen;
            ((ICommand)((Button)sender).Tag).Execute();         
        }

        private void erzeugeCommandButtonsForUnit(IUnit unit)
        {
            this.Controls.Clear();

            hoffset = 1;

            int offset = 1;
            
            foreach (ICommand aktCommand in Program.m_objCampaign.getCommandsForUnit(unit))
            {
                this.addButton(aktCommand, ref offset);
                offset += 1;
            }

            hoffset += 1;
        }

    }
}