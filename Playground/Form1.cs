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
        private CampaignEngine  m_objEngine;
        public Form1()
        {
            InitializeComponent();
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_objEngine = new CampaignEngine();

            m_objEngine.FieldField = new Field_Schachbrett(3, 3);

            m_objEngine.addPlayer(new Player(1));
            
            Player p1 = m_objEngine.addPlayer(new Player(1));

            m_objEngine.addUnit(p1.Id, new DummyUnit(0));

            List<IUnit> lisEinheiten = m_objEngine.getActiveUnitsForPlayer(p1);
            IUnit unitArsch = lisEinheiten[0];
            m_objEngine.FieldField.get("0|0").ListUnits.Add(unitArsch );

            List<ICommand> lisCommands = m_objEngine.getCommandsForUnit(unitArsch);
            lisCommands[2].Execute();
        }
    }
}
