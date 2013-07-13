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
            int iRows = 4;

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
                    this.addUnit("XWing", "Rebel");
                };
                gridGroup.AddChildAt(initButton, 0, 3);

                NuclearUI.Button tieButton = new NuclearUI.Button(Manager.MenuScreen, "add Tie");
                tieButton.ClickHandler = delegate
                {
                    this.addUnit("Tie", "Empire");
                };

                gridGroup.AddChildAt(tieButton, 1, 3);

                button = new NuclearUI.Button(Manager.MenuScreen, "add Station");
                button.ClickHandler = delegate
                {
                    this.addUnit("Raumstation", "Rebel");
                };
                gridGroup.AddChildAt(button, 2, 3);

                button = new NuclearUI.Button(Manager.MenuScreen, "add Kreuzer");
                button.ClickHandler = delegate
                {
                    this.addUnit("Raumkreuzer", "Empire");
                };
                gridGroup.AddChildAt(button, 3, 3);

                button = new NuclearUI.Button(Manager.MenuScreen, "add Transporter");
                button.ClickHandler = delegate
                {
                    this.addUnit("Raumtransporter", "Rebel");
                };
                gridGroup.AddChildAt(button, 4, 3);


                button = new NuclearUI.Button(Manager.MenuScreen, "safe State");
                button.ClickHandler = delegate
                {
                    string strState = Program.m_objCampaign.saveCurrentGameState();

                    Program.objCampaignState.strCCKey = strState;
                    Program.objCampaignState.strSaveKey = strState;
                    Program.objCampaignState.save();
                };
                gridGroup.AddChildAt(button, 0, 1);



                button = new NuclearUI.Button(Manager.MenuScreen, "load State");
                button.ClickHandler = delegate
                {
                    Program.m_objCampaign.restoreGameState(Program.objCampaignState.strCCKey);
                };
                gridGroup.AddChildAt(button, 1, 1);

                button = new NuclearUI.Button(Manager.MenuScreen, "Init");
                button.ClickHandler = delegate
                {
                    this.manualDataInit();
                    
            
                };
                gridGroup.AddChildAt(button, 1, 2);
            }

            
            
        }

        private void addUnit(string strUnit, string strSpieler)
        {
            int XW_ID = Program.m_objCampaign.getCampaignInfo_UnitTypeByName(strUnit).ID;
            Player ply = Program.m_objCampaign.getPlayerByName(strSpieler);
            //erzeuge Spieler1 
            Program.m_objCampaign.createNewUnit(ply.Id, XW_ID);
        }

        private void manualDataInit()
        {
            Program.m_objCampaign.addCampaignInfo_UnitTypes(new clsUnitType("XWing", 2, 1, "XW", "Standart Rebellen Kampfgeschwader"));
            Program.m_objCampaign.addCampaignInfo_UnitTypes(new clsUnitType("Tie", 1, 2, "TieF"));

            Program.m_objCampaign.addCampaignInfo_UnitTypes(new clsUnitType("Raumstation", 2, 0, "Station", "mit [%intResourceValue%] Punkten beladen", 3, 0));
            Program.m_objCampaign.addCampaignInfo_UnitTypes(new clsUnitType("Raumtransporter", 0, 1, "Transport", "Blind, langsam und mit [%intResourceValue%] Punkten beladen", 0, 100));
            Program.m_objCampaign.addCampaignInfo_UnitTypes(new clsUnitType("Raumkreuzer", 1, 1, "Cruiser","",1));

            //erzeuge Spieler1
            Player ply = Program.m_objCampaign.addPlayer("Rebel");
            ply.unitspawnSektor = Program.m_objCampaign.getSektor("|0|0|0|");

            //erzeuge Spieler2
            ply = Program.m_objCampaign.addPlayer("Empire");
            ply.unitspawnSektor = Program.m_objCampaign.getSektor("|5|5|0|");
           
        }

        //----------------------------------------------------------------------



        private void initGCML()
        {
            Program.objCampaignState = new clsCampaignInfo();

            if (System.IO.File.Exists(".\\CCDate.dat")) //&& MessageBox.Show("laden?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Program.objCampaignState.load();
                Program.m_objCampaign = new CampaignBuilderSchach().restoreFromDb(Program.objCampaignState.strCCKey, Program.objCampaignState.strSaveKey);
                List<Player> listPlayers = Program.m_objCampaign.getPlayerList();
           
            }
            else
            {
                Program.m_objCampaign = new CampaignBuilderSchach().buildNew();
            }

            //Program.m_objCampaign.onStatus += new Field.delStatus(Global_onStatus);
        
        }
    }
}
