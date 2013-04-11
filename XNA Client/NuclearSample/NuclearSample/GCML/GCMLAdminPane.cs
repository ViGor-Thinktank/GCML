using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NuclearUI = NuclearWinter.UI;

using GenericCampaignMasterLib;
using GenericCampaignMasterModel; 


namespace NuclearSample.GCML
{
    class GCMLAdminPane: NuclearUI.ManagerPane<MainMenuManager>
    {
        //----------------------------------------------------------------------
        public GCMLAdminPane(MainMenuManager _manager)
        : base( _manager )
        {
            int iRows = 4;

            NuclearUI.GridGroup gridGroup = new NuclearUI.GridGroup( Manager.MenuScreen, 4, iRows, false, 0 );
            gridGroup.AnchoredRect = NuclearUI.AnchoredRect.CreateTopLeftAnchored( 0, 0, 400, iRows * 50 );
            
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
                    int XW_ID = Program.m_objCampaign.getCampaignInfo_UnitTypeByName("X-Wing").ID;
                    Player ply = Program.m_objCampaign.getPlayerByName("Rebel");
                    //erzeuge Spieler1 
                    Program.m_objCampaign.createNewUnit(ply.Id, XW_ID);

                };
                gridGroup.AddChildAt(initButton, 1, 0);

                NuclearUI.Button tieButton = new NuclearUI.Button(Manager.MenuScreen, "add Tie");
                tieButton.ClickHandler = delegate
                {
                    int XW_ID = Program.m_objCampaign.getCampaignInfo_UnitTypeByName("Tie Fighter").ID;
                    Player ply = Program.m_objCampaign.getPlayerByName("Empire");
                    //erzeuge Spieler1 
                    Program.m_objCampaign.createNewUnit(ply.Id, XW_ID);

                };

                gridGroup.AddChildAt(tieButton, 2, 0);


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
                    
            clsUnitType newUnit = new clsUnitType();
            newUnit.strBez = "X-Wing";
            newUnit.intSichtweite = 2;
            newUnit.intMovement = 1;
            newUnit.strClientData = "Texture:=XW";
            int XW_ID = Program.m_objCampaign.addCampaignInfo_UnitTypes(newUnit);

            newUnit = new clsUnitType();
            newUnit.strBez = "Tie Fighter";
            newUnit.intSichtweite = 1;
            newUnit.intMovement = 2;
            newUnit.strClientData = "Texture:=TieF";
            int TF_ID = Program.m_objCampaign.addCampaignInfo_UnitTypes(newUnit);

            //erzeuge Spieler1
            Player ply = Program.m_objCampaign.addPlayer("Rebel");
            ply.unitspawnSektor = Program.m_objCampaign.getSektor("|0|0|0|");
            
            //erzeuge Spieler2
            ply = Program.m_objCampaign.addPlayer("Empire");
            ply.unitspawnSektor = Program.m_objCampaign.getSektor("|5|5|0|");
            
                };
                gridGroup.AddChildAt(button, 1, 2);
            }

            
            
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
                foreach (Player newP in listPlayers)
                {
                    /*frmPlayerMainForm frmP = new frmPlayerMainForm();
                    frmP.strMyPlayerID = newP.Id;
                    frmP.button1.Visible = false;
                    frmP.Text = newP.Playername;
                    frmP.Show();
                    lisForms.Add(frmP);*/
                }

            }
            else
            {
                Program.m_objCampaign = new CampaignBuilderSchach().buildNew();
            }

            //Program.m_objCampaign.onStatus += new Field.delStatus(Global_onStatus);
        
        }
    }
}
