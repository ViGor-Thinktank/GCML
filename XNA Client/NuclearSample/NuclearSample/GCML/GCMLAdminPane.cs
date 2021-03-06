﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NuclearUI = NuclearWinter.UI;

using GenericCampaignMasterLib;
using GenericCampaignMasterModel;
using GcmlDataAccess;


namespace GCML_XNA_Client.GCML
{
    class GCMLAdminPane: NuclearUI.ManagerPane<MainMenuManager>
    {
        private bool m_blnInit = false;

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
                
                NuclearUI.Button initButton = new NuclearUI.Button(Manager.MenuScreen, "X-Wing");
                initButton.ClickHandler = delegate
                {
                    this.addUnit("XWing", "Admiral Ackbar");
                };
                gridGroup.AddChildAt(initButton, 0, 3);

                NuclearUI.Button tieButton = new NuclearUI.Button(Manager.MenuScreen, "Tie");
                tieButton.ClickHandler = delegate
                {
                    this.addUnit("Tie", "Grand Moff Tarkin");
                };

                gridGroup.AddChildAt(tieButton, 1, 3);

                button = new NuclearUI.Button(Manager.MenuScreen, "Station");
                button.ClickHandler = delegate
                {
                    this.addUnit("Raumstation", "Admiral Ackbar");
                };
                gridGroup.AddChildAt(button, 2, 3);

                button = new NuclearUI.Button(Manager.MenuScreen, "Kreuzer");
                button.ClickHandler = delegate
                {
                    this.addUnit("Raumkreuzer", "Grand Moff Tarkin");
                };
                gridGroup.AddChildAt(button, 3, 3);

                button = new NuclearUI.Button(Manager.MenuScreen, "Transporter");
                button.ClickHandler = delegate
                {
                    this.addUnit("Raumtransporter", "Admiral Ackbar");
                };
                gridGroup.AddChildAt(button, 4, 3);

                button = new NuclearUI.Button(Manager.MenuScreen, "TieA");
                button.ClickHandler = delegate
                {
                    this.addUnit("TieA", "Grand Moff Tarkin");
                };
                gridGroup.AddChildAt(button, 4, 4);

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

                button = new NuclearUI.Button(Manager.MenuScreen, "Init HQ Demo");
                button.ClickHandler = delegate
                {
                    this.InitHQDemo();

                };
                gridGroup.AddChildAt(button, 1, 4);

                button = new NuclearUI.Button(Manager.MenuScreen, "Planet Randomizer");
                button.ClickHandler = delegate
                {
                    this.setRandomPlanets();

                };
                gridGroup.AddChildAt(button, 2, 4);

                button = new NuclearUI.Button(Manager.MenuScreen, "UnitPopup Test");
                button.ClickHandler = delegate
                {
                    this.setRandomPlanets();

                };
                gridGroup.AddChildAt(button, 3, 4);

            }

            
            
        }
        
        private void setRandomPlanets()
        {
            this.manualDataInit();

            this.addUnit("Planet", "Grand Moff Tarkin", "1|3|0");
            this.addUnit("Planet", "Grand Moff Tarkin", "4|2|0");
               
           /* Random objRand = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 2; i++)
            {
                this.addUnit("Planet", "Grand Moff Tarkin", objRand.Next(0, 6).ToString() + "|" + objRand.Next(0, 6).ToString() + "|0");
                
            }*/

            Program.m_objCampaign.Tick();
        }

        private void InitHQDemo()
        {
            this.manualDataInit();
            
            this.addUnit("Korvette", "Admiral Ackbar");

            this.addUnit("XWing", "Admiral Ackbar", "1|0|0");
            this.addUnit("XWing", "Admiral Ackbar", "1|0|0");
            this.addUnit("YWing", "Admiral Ackbar", "1|0|0");

            this.addUnit("XWing", "Admiral Ackbar", "1|1|0");
            this.addUnit("YWing", "Admiral Ackbar", "1|2|0");
            
            this.addUnit("Cruiser", "Grand Moff Tarkin");
            
            this.addUnit("Planet", "Grand Moff Tarkin", "4|3|0");
            this.addUnit("Planet", "Grand Moff Tarkin", "0|3|0");
            this.addUnit("Planet", "Admiral Ackbar", "2|5|0");

            Program.m_objCampaign.Tick();
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
            int Unit_ID = Program.m_objCampaign.UnitType_getTypeByName(strUnit).ID;
            Player ply = Program.m_objCampaign.Player_getByName(strSpieler);
            Program.m_objCampaign.Unit_createNew(ply.Id, Unit_ID, strSektor);
        }

        private void manualDataInit()
        {
            if (m_blnInit)
                return;

            m_blnInit = true;
            clsUnitType objXWing = new clsUnitType("XWing", 2, 1, "XWing", "X-Wing");
            Program.m_objCampaign.UnitType_addNew(objXWing);

            clsUnitType objYWing = new clsUnitType("YWing", 1, 1, "YWing", "Y-Wing");
            Program.m_objCampaign.UnitType_addNew(objYWing);

            clsUnitType objTie = new clsUnitType("Tie", 1, 2, "Tie");
            Program.m_objCampaign.UnitType_addNew(objTie);

            objTie = new clsUnitType("TieA", 2, 2, "TieA");
            Program.m_objCampaign.UnitType_addNew(objTie);

            clsUnitType objStation = new clsUnitType("Raumstation", 2, 0, "Station", "mit [%intResourceValue%] Punkten beladen", new List<clsUnitType>(), 250);
            Program.m_objCampaign.UnitType_addNew(objStation);

            clsUnitType objTrans = new clsUnitType("Raumtransporter", 0, 1, "Transport", "Blind, langsam und mit [%intResourceValue%] Punkten beladen", new List<clsUnitType> { objStation }, 100);
            Program.m_objCampaign.UnitType_addNew(objTrans);
            
            clsUnitType objPlanet = new clsUnitType("Planet", 0, 0, "Planet", "Produziert Resourcen, mit [%intResourceValue%] Punkten beladen. Kann Transporter spawnwn", new List<clsUnitType> { objTrans }, 1000, true, true);
            Program.m_objCampaign.UnitType_addNew(objPlanet);
            
            Program.m_objCampaign.UnitType_addNew(new clsUnitType("Cruiser", 1, 1, "Cruiser", "Empire HQ", new List<clsUnitType> { objTie }, 250));
            Program.m_objCampaign.UnitType_addNew(new clsUnitType("Korvette", 1, 1, "Korvette", "Rebel HQ", new List<clsUnitType> { objXWing }, 250));

            Faction facRebel = Program.m_objCampaign.Faction_add("Rebellen Allianz", new List<clsUnitType> { objXWing });
            Player ply = Program.m_objCampaign.Player_add("Admiral Ackbar", facRebel);
            ply.unitspawnSektor = Program.m_objCampaign.Sektor_getByID("|0|0|0|");

            Faction facEmpire = Program.m_objCampaign.Faction_add("Imperium", new List<clsUnitType> { objTie });
            ply = Program.m_objCampaign.Player_add("Grand Moff Tarkin", facEmpire);
            ply.unitspawnSektor = Program.m_objCampaign.Sektor_getByID("|6|6|0|");
           
        }

        private void initGCML()
        {
            Program.objCampaignState = new clsCampaignInfo();

            if (false && System.IO.File.Exists(".\\CCDate.dat"))// && MessageBox.Show("laden?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Program.objCampaignState.load();
                Program.m_objCampaign = Program.gcmlData.getCampaignController(Program.objCampaignState.strSaveKey);
                //Program.m_objCampaign = CampaignBuilder.Instance.restoreFromDb(Program.objCampaignState.strCCKey, Program.objCampaignState.strSaveKey);
                
                List<Player> listPlayers = Program.m_objCampaign.Player_getPlayerList();
           
            }
            else
            {
                Program.m_objCampaign = Program.gcmlData.createNewCampaign(new CampaignInfo() { campaignId = "", campaignName = "test", FieldDimension = new clsSektorKoordinaten(6, 6, 0) });
            }

            //Program.m_objCampaign.onStatus += new Field.delStatus(Global_onStatus);
        
        }
    }
}
