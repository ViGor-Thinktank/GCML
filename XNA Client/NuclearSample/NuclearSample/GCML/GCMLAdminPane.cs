using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NuclearUI = NuclearWinter.UI;

using GenericCampaignMasterLib;
using GenericCampaignMasterModel; 


namespace GCML_XNA_Client.GCML
{
    class GCMLAdminPane: NuclearUI.ManagerPane<MainMenuManager>
    {
        //----------------------------------------------------------------------
        public GCMLAdminPane(MainMenuManager _manager)
        : base( _manager )
        {
            int iRows = 5;

            NuclearUI.GridGroup gridGroup = new NuclearUI.GridGroup( Manager.MenuScreen, 5, iRows, false, 0 );
            gridGroup.AnchoredRect = NuclearUI.AnchoredRect.CreateTopLeftAnchored(0, 0, 700, iRows * 50);
            
            AddChild( gridGroup );

            
            {
                NuclearUI.Button button = new NuclearUI.Button(Manager.MenuScreen, "Tick");
                button.ClickHandler = delegate
                {
                    Program.m_objCampaign.Tick();
                };
                gridGroup.AddChildAt(button, 0, 0);
                
                NuclearUI.Button initButton = new NuclearUI.Button(Manager.MenuScreen, "add X-Wing");
                initButton.ClickHandler = delegate
                {
                    this.addUnit("XWing", "Admiral Ackbar");
                };
                gridGroup.AddChildAt(initButton, 0, 3);

                NuclearUI.Button tieButton = new NuclearUI.Button(Manager.MenuScreen, "add Tie");
                tieButton.ClickHandler = delegate
                {
                    this.addUnit("Tie", "Grand Moff Tarkin");
                };

                gridGroup.AddChildAt(tieButton, 1, 3);

                button = new NuclearUI.Button(Manager.MenuScreen, "add Station");
                button.ClickHandler = delegate
                {
                    this.addUnit("Raumstation", "Admiral Ackbar");
                };
                gridGroup.AddChildAt(button, 2, 3);

                button = new NuclearUI.Button(Manager.MenuScreen, "add Kreuzer");
                button.ClickHandler = delegate
                {
                    this.addUnit("Raumkreuzer", "Grand Moff Tarkin");
                };
                gridGroup.AddChildAt(button, 3, 3);

                button = new NuclearUI.Button(Manager.MenuScreen, "add Transporter");
                button.ClickHandler = delegate
                {
                    this.addUnit("Raumtransporter", "Admiral Ackbar");
                };
                gridGroup.AddChildAt(button, 4, 3);


                button = new NuclearUI.Button(Manager.MenuScreen, "safe State");
                button.ClickHandler = delegate
                {
                    string strState = Program.m_objCampaign.GameState_saveCurrent();

                    Program.objCampaignState.strCCKey = strState;
                    Program.objCampaignState.strSaveKey = strState;
                    Program.objCampaignState.save();
                };
                gridGroup.AddChildAt(button, 0, 1);



                button = new NuclearUI.Button(Manager.MenuScreen, "load State");
                button.ClickHandler = delegate
                {
                    Program.m_objCampaign.GameState_restoreByKey(Program.objCampaignState.strCCKey);
                };
                gridGroup.AddChildAt(button, 1, 1);

                button = new NuclearUI.Button(Manager.MenuScreen, "Init");
                button.ClickHandler = delegate
                {
                    this.manualDataInit();
                };
                gridGroup.AddChildAt(button, 1, 2);

                button = new NuclearUI.Button(Manager.MenuScreen, "Init Build Demo");
                button.ClickHandler = delegate
                {
                    this.InitBuildDemo();
                   
                };
                gridGroup.AddChildAt(button, 0, 4);

            }

            
            
        }

        private void InitBuildDemo()
        {
            this.manualDataInit();
            this.addUnit("Raumtransporter", "Admiral Ackbar");
            this.addUnit("Raumstation", "Admiral Ackbar", "2|2|0");
            Program.m_objCampaign.Tick();
        }

        private void addUnit(string strUnit, string strSpieler, string strSektor = "")
        {
            int XW_ID = Program.m_objCampaign.UnitType_getTypeByName(strUnit).ID;
            Player ply = Program.m_objCampaign.Player_getByName(strSpieler);
            Program.m_objCampaign.Unit_createNew(ply.Id, XW_ID, strSektor);
        }

        private void manualDataInit()
        {
            clsUnitType objXWing = new clsUnitType("XWing", 2, 1, "XW", "Standart Rebellen Kampfgeschwader");
            clsUnitType objTie = new clsUnitType("Tie", 1, 2, "TieF");
            Program.m_objCampaign.UnitType_addNew(objXWing);
            Program.m_objCampaign.UnitType_addNew(objTie);

            Program.m_objCampaign.UnitType_addNew(new clsUnitType("Raumstation", 2, 0, "Station", "mit [%intResourceValue%] Punkten beladen", new List<clsUnitType> { objXWing }, 0));
            Program.m_objCampaign.UnitType_addNew(new clsUnitType("Raumtransporter", 0, 1, "Transport", "Blind, langsam und mit [%intResourceValue%] Punkten beladen", null, 100));
            Program.m_objCampaign.UnitType_addNew(new clsUnitType("Raumkreuzer", 1, 1, "Cruiser", "kann Tie Schwadronen spawnen", new List<clsUnitType> { objTie }, 1));

            //erzeuge Spieler1
            Player ply = Program.m_objCampaign.Player_add("Admiral Ackbar", "Rebellen Allianz");
            ply.unitspawnSektor = Program.m_objCampaign.Sektor_getByID("|0|0|0|");

            //erzeuge Spieler2
            ply = Program.m_objCampaign.Player_add("Grand Moff Tarkin", "Imperium");
            ply.unitspawnSektor = Program.m_objCampaign.Sektor_getByID("|5|5|0|");
           
        }

        private void initGCML()
        {
            Program.objCampaignState = new clsCampaignInfo();

            if (System.IO.File.Exists(".\\CCDate.dat")) //&& MessageBox.Show("laden?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Program.objCampaignState.load();
                Program.m_objCampaign = new CampaignBuilderSchach().restoreFromDb(Program.objCampaignState.strCCKey, Program.objCampaignState.strSaveKey);
                List<Player> listPlayers = Program.m_objCampaign.Player_getPlayerList();
           
            }
            else
            {
                Program.m_objCampaign = new CampaignBuilderSchach().buildNew();
            }

            //Program.m_objCampaign.onStatus += new Field.delStatus(Global_onStatus);
        
        }
    }
}
