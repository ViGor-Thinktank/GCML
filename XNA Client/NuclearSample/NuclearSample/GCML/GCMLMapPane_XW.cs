using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using GenericCampaignMasterLib;
using GenericCampaignMasterModel; 

using NuclearUI = NuclearWinter.UI;

namespace NuclearSample.GCML
{
    class GCMLMapPane_XW : NuclearUI.ManagerPane<MainMenuManager>
    {
        public GCMLMapPane_XW(MainMenuManager _manager) : base(_manager)
        {
            this.initTextures();
            this.initMap(true);

            Program.m_objCampaign.onTick += new CampaignController.delTick(m_objCampaign_onTick);
        }

        void m_objCampaign_onTick()
        {
            this.initMap(false);
        }

        private void imgBtn_ClickHandler(NuclearUI.Image sender)
        {
            string strID = m_dicImgCmds[sender];
            ICommand aktCommand = Program.m_objCampaign.getCommand(strID);
            //Manager.MessagePopup.Setup("Move Info", aktCommand.strInfo, NuclearWinter.i18n.Common.Confirm, false);
            //Manager.MessagePopup.Open(250, 250);
            aktCommand.Register();
            initMap(false);
        }

        private void imgUnit_ClickHandler(NuclearUI.Image sender)
        {
            clsUnit aktUnit = m_dicUnits[sender];
    
            clsGCML_Unit Unit = new clsGCML_Unit(aktUnit);

            NuclearUI.GridGroup gridAction = new NuclearUI.GridGroup(Manager.MenuScreen, 3, 3, false, 0);
            m_gridMap.AddChildAt(gridAction, Unit.aktSektor.objSektorKoord.X, Unit.aktSektor.objSektorKoord.Y);
            gridAction.AnchoredRect = NuclearUI.AnchoredRect.CreateCentered(sender.ContentWidth, sender.ContentHeight);

            m_dicImgCmds = new Dictionary<NuclearUI.Image, string>();

            IList<ICommand> tm = Program.m_objCampaign.getCommandsForUnit(aktUnit);
            foreach (ICommand aktCommand in tm)
            {
                NuclearUI.Image imgBtn = new NuclearUI.Image(Manager.MenuScreen, this.m_dicTextures["move"], false);
                m_dicImgCmds.Add(imgBtn, aktCommand.CommandId);
                imgBtn.ClickHandler = new Action<NuclearUI.Image>(imgBtn_ClickHandler);

                string[] str = ((Move)aktCommand).TargetSektor.strUniqueID.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                int x_offset = Convert.ToInt32(str[0]);
                int y_offset = Convert.ToInt32(str[1]);
                m_gridMap.AddChildAt(imgBtn, x_offset, y_offset);
            }
        }

        private void initXW()
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
            
        }

        private NuclearUI.GridGroup m_gridMap;
        protected void initMap(bool GCML_init)
        {
            if (GCML_init)
            {
                this.initXW();                
            }

            this.Clear();
            
            //Background Image
            NuclearUI.Image imgMap = new NuclearUI.Image(Manager.MenuScreen, m_dicTextures["Stars"], false);
            
            AddChild(imgMap);

            int intGridWidth = Program.m_objCampaign.campaignEngine.FieldField.FieldDimension.X+1;
            int intGridHeight = Program.m_objCampaign.campaignEngine.FieldField.FieldDimension.Y+1;

            //Grid erzeugen
            m_gridMap = new NuclearUI.GridGroup(Manager.MenuScreen, intGridHeight, intGridWidth, false, 0);
            AddChild(m_gridMap);
            
            //Grid auf das Background Image werfen
            m_gridMap.AnchoredRect = NuclearUI.AnchoredRect.CreateCentered(imgMap.ContentWidth, imgMap.ContentHeight);
   
            foreach (GenericCampaignMasterModel.Player aktPly in Program.m_objCampaign.getPlayerList())
            { 
                foreach (clsUnit aktUnit in aktPly.ListUnits)
                {
                    clsGCML_Unit objUnit = new clsGCML_Unit(aktUnit);

                    if (objUnit.objUnit.aktCommand != null && !objUnit.objUnit.aktCommand.blnExecuted)
                    {
                        NuclearUI.Image imgBtn = new NuclearUI.Image(Manager.MenuScreen, this.m_dicTextures["pfeil"], false);

                        imgBtn.fltRotation = 0;

                        string[] str = ((Move)objUnit.objUnit.aktCommand).TargetSektor.strUniqueID.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        int x_offset = Convert.ToInt32(str[0]);
                        int y_offset = Convert.ToInt32(str[1]);
                        m_gridMap.AddChildAt(imgBtn, x_offset, y_offset);
                        //Token einblenden das den Command angezeigt

                    }

                    NuclearUI.Image imgUnit = new NuclearUI.Image(Manager.MenuScreen, this.m_dicTextures[objUnit.strTexName], false);
                    m_dicUnits.Add(imgUnit, aktUnit);
                    imgUnit.ClickHandler = new Action<NuclearUI.Image>(imgUnit_ClickHandler);

                    m_gridMap.AddChildAt(imgUnit, objUnit.aktSektor.objSektorKoord.X, objUnit.aktSektor.objSektorKoord.Y);
                    
                }
            }
        }

        private void addButton(ICommand aktCommand, ref int offset, string p)
        {
            throw new NotImplementedException();
        }
        
        private Dictionary<string, Texture2D> m_dicTextures;
        private Dictionary<NuclearUI.Image, string> m_dicImgCmds;
        private Dictionary<NuclearUI.Image, clsUnit> m_dicUnits = new Dictionary<NuclearUI.Image,clsUnit>();

        private void initTextures()
        {
            m_dicTextures = new Dictionary<string, Texture2D>();
            this.loadTexture("Stars");
            
            this.loadTexture("TieF");
            this.loadTexture("XW");

            this.loadTexture("eins");
            this.loadTexture("zwei");

            this.loadTexture("move");
            this.loadTexture("pfeil");
        }

        private void loadTexture(string p)
        {
            if (!m_dicTextures.ContainsKey(p))
                m_dicTextures.Add(p, base.Manager.Content.Load<Texture2D>("Sprites/" + p));
            else
                m_dicTextures[p] = base.Manager.Content.Load<Texture2D>("Sprites/" + p);
            
        }
        
    }
}
