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
                m_objEngine.FieldField = new Field_Schachbrett(3, 3);
            }
            else
            {
                m_objEngine.FieldField = new Field_Schlauch(3);
            }

            m_objEngine.FieldField.onFieldStatus += new Field.delFieldStatus(FieldField_onFieldStatus);


            Player p1 = m_objEngine.addPlayer(new Player(1));

            m_objEngine.addUnit(p1.Id, new DummyUnit(0));

            List<IUnit> lisEinheiten = m_objEngine.getActiveUnitsForPlayer(p1);


            m_objEngine.FieldField.get(m_objEngine.FieldField.nullSektor).ListUnits.Add(lisEinheiten[0]);

        
            List<ICommand> lisCommands = m_objEngine.getCommandsForUnit(lisEinheiten[0]);
            
        
            foreach (ICommand aktCommand in lisCommands)
            {
                this.addButton(aktCommand, lisCommands.IndexOf(aktCommand));
            }
        }

        void FieldField_onFieldStatus(string strText)
        {
            frmStatus.Status(strText);
        }

        private void addButton(ICommand aktCommand, int offset)
        {
            System.Windows.Forms.Button btnNew = new System.Windows.Forms.Button();
            
            btnNew.Size = new System.Drawing.Size(125, 25);
            btnNew.Location = new System.Drawing.Point((12 + offset * btnNew.Size.Width), 50);
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
        }

    }
}
