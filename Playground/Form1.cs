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
    public partial class Form1 : Form
    {
        private CampaignEngine m_objEngine;
        public Form1()
        {
            InitializeComponent();
            
            
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            m_objEngine = new CampaignEngine();

            if (MessageBox.Show("Schachbrett?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                m_objEngine.FieldField = new Field_Schachbrett(5, 5);
            }
            else
            {
                m_objEngine.FieldField = new Field_Schlauch(3);
            }

            m_objEngine.FieldField.onFieldStatus += new Field.delStatus(Global_onStatus);
            m_objEngine.onEngineStatus += new Field.delStatus(Global_onStatus);

            Player p1 = m_objEngine.addPlayer(new Player(1));

            m_objEngine.addUnit(p1.Id, new DummyUnit(0));
            List<IUnit> lisEinheiten = m_objEngine.getActiveUnitsForPlayer(m_objEngine.dicPlayers[1]);
            m_objEngine.FieldField.get(m_objEngine.FieldField.nullSektor).ListUnits.Add(lisEinheiten[0]);

            raiseTick();
           
        }

        void Global_onStatus(string strText)
        {
            frmStatus.Status(strText);
        }

        private int hoffset = 0;

        private void addButton(ICommand aktCommand, int offset)
        {
            System.Windows.Forms.Button btnNew = new System.Windows.Forms.Button();
            
            btnNew.Size = new System.Drawing.Size(125, 25);
            btnNew.Location = new System.Drawing.Point((12 + offset * btnNew.Size.Width), 50 + hoffset * btnNew.Size.Height);
            btnNew.Name = "movecmd" + offset.ToString();
            btnNew.Tag = aktCommand;
            btnNew.TabIndex = 0;
            btnNew.Text = aktCommand.strInfo;
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += new System.EventHandler(button_Click);

            this.Controls.Add(btnNew);
            
        }

        private void button_Click(object sender, EventArgs e)
        {
            ((ICommand)((Button)sender).Tag).Execute();
            raiseTick();
        }

        private void raiseTick()
        {
            List<IUnit> lisEinheiten = m_objEngine.getActiveUnitsForPlayer(m_objEngine.dicPlayers[1]);

            List<ICommand> lisCommands = m_objEngine.getCommandsForUnit(lisEinheiten[0]);

            hoffset += 1;

            foreach (ICommand aktCommand in lisCommands)
            {
                this.addButton(aktCommand, lisCommands.IndexOf(aktCommand));
            }
        }

    }
}
