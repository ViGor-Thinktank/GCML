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

            m_objEngine.FieldField = new Field_Schachbrett(3, 3);

            Player p1 = m_objEngine.addPlayer(new Player(1));

            m_objEngine.addUnit(p1.Id, new DummyUnit(0));

            List<IUnit> lisEinheiten = m_objEngine.getActiveUnitsForPlayer(p1);
            
            
            m_objEngine.FieldField.get("0|0").ListUnits.Add(lisEinheiten[0]);

            textBox1.Text+= m_objEngine.FieldField.getSektorForUnit(lisEinheiten[0]).objSektorKoord.uniqueIDstr();
            textBox1.Text += " -> ";

            List<ICommand> lisCommands = m_objEngine.getCommandsForUnit(lisEinheiten[0]);
            
            lisCommands[3].Execute();

            textBox1.Text += m_objEngine.FieldField.getSektorForUnit(lisEinheiten[0]).objSektorKoord.uniqueIDstr();

        }
    }
}
