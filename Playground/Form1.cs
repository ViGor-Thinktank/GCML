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

            m_objEngine.FieldField = new Field_Schlauch(3);

            m_objEngine.AddPlayer(new Player(1));
            Player p1 = m_objEngine.ListPlayers[0];
            p1.ListUnits.Add(new DummyUnit(1));

            List<IUnit> lisEinheiten = m_objEngine.getActiveUnitsForPlayer(p1);

            m_objEngine.FieldField.get("1").ListUnits.Add(lisEinheiten[0]);
            m_objEngine.getCommandsForUnit(lisEinheiten[0])[0].Execute();
        }
    }
}
