﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using GenericCampaignMasterLib;
using GenericCampaignMasterModel; 

using NuclearUI = NuclearWinter.UI;

namespace GCML_XNA_Client.GCML
{
    class GCMLMapPane_XW : NuclearUI.ManagerPane<MainMenuManager>
    {
        private int m_intPlayerIndex;

        public GCMLMapPane_XW(MainMenuManager _manager, int intPlayerIndex) : base(_manager)
        {
            this.initTextures();
            
            this.initMap();

            this.m_intPlayerIndex = intPlayerIndex;

            Program.m_objCampaign.onHasTicked += new CampaignController.delTick(m_objCampaign_onTick);
        }

        void m_objCampaign_onTick()
        {
            this.initMap();
        }

        private void imgBtn_ClickHandler(NuclearUI.Image sender)
        {
            ICommand aktCommand = Program.m_objCampaign.getCommand(m_dicImgCmds[sender]);
            //Manager.MessagePopup.Setup("Move Info", aktCommand.strInfo, NuclearWinter.i18n.Common.Confirm, false);
            //Manager.MessagePopup.Open(250, 250);
            aktCommand.Register();
            initMap();
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
           

        }

        private NuclearUI.Image newNumberCounter(int Index)
        {
            switch (Index)
            {
                case 0:
                    return new NuclearUI.Image(Manager.MenuScreen, this.m_dicTextures["eins"], false);

                case 1:
                    return new NuclearUI.Image(Manager.MenuScreen, this.m_dicTextures["zwei"], false);

                default:
                    return null;

            }
        }

        private NuclearUI.GridGroup m_gridMap;
        protected void initMap()
        {
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

            GenericCampaignMasterModel.Player aktPly = Program.m_objCampaign.getCampaignStateForPlayer(m_intPlayerIndex.ToString());
            if (aktPly != null && aktPly.ListUnits != null)
            {
                foreach (Sektor aktSek in aktPly.dicVisibleSectors.Values)
                {
                    foreach (clsUnit aktUnit in aktSek.ListUnits)
                    {
                        drawUnit(aktUnit, aktPly);
                    }                    
                }
                

/*                foreach (clsUnit aktUnit in aktPly.ListUnits)
                {
                    drawUnit(aktUnit);
                }*/
            }
        }

        private void drawUnit(clsUnit aktUnit, GenericCampaignMasterModel.Player aktPly)
        {
            NuclearUI.Image imgNumber = newNumberCounter(aktPly.ListUnits.IndexOf(aktUnit));

            clsGCML_Unit objUnit = new clsGCML_Unit(aktUnit);

            if (objUnit.objUnit.aktCommand != null && !objUnit.objUnit.aktCommand.blnExecuted)
            {
                NuclearUI.Image imgBtn = new NuclearUI.Image(Manager.MenuScreen, this.m_dicTextures["pfeil"], false);

                imgBtn.fltRotation = 0;

                string[] str = ((Move)objUnit.objUnit.aktCommand).TargetSektor.strUniqueID.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                int x_offset = Convert.ToInt32(str[0]);
                int y_offset = Convert.ToInt32(str[1]);
                m_gridMap.AddChildAt(imgBtn, x_offset, y_offset);
                if (imgNumber != null)
                    m_gridMap.AddChildAt(imgNumber, x_offset, y_offset);

                //Token einblenden das den Command angezeigt

            }

            NuclearUI.Image imgUnit = new NuclearUI.Image(Manager.MenuScreen, this.m_dicTextures[objUnit.strTexName], false);
            m_dicUnits.Add(imgUnit, aktUnit);
            imgUnit.ClickHandler = new Action<NuclearUI.Image>(imgUnit_ClickHandler);

            m_gridMap.AddChildAt(imgUnit, objUnit.aktSektor.objSektorKoord.X, objUnit.aktSektor.objSektorKoord.Y);
            if (imgNumber != null)
                m_gridMap.AddChildAt(newNumberCounter(aktPly.ListUnits.IndexOf(aktUnit)), objUnit.aktSektor.objSektorKoord.X, objUnit.aktSektor.objSektorKoord.Y);

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