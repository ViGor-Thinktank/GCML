using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using GenericCampaignMasterLib;
using GenericCampaignMasterModel;
using GenericCampaignMasterModel.Commands;

using NuclearUI = NuclearWinter.UI;

namespace GCML_XNA_Client.GCML
{
    class GCMLMapPane_XW : NuclearUI.ManagerPane<MainMenuManager>
    {
        private int m_intPlayerIndex;

        private Dictionary<NuclearUI.Image, string> m_dicCommandIcons;
        private Dictionary<NuclearUI.Image, clsUnit> m_dicUnits = new Dictionary<NuclearUI.Image, clsUnit>();

        private NuclearUI.GridGroup m_gridMap;

#region Texturehandling

        private Dictionary<string, Texture2D> m_dicTextures;
        
        private void initTextures()
        {
            m_dicTextures = new Dictionary<string, Texture2D>();
            
            this.loadTexture("Stars");
            this.loadTexture("Grid7");
            this.loadTexture("Grid8");
            this.loadTexture("Grid10");
            this.loadTexture("Planet");

            this.loadTexture("Tie");
            this.loadTexture("TieA");
            this.loadTexture("XWing");
            this.loadTexture("YWing");

            this.loadTexture("Korvette");
            this.loadTexture("Cruiser");

            this.loadTexture("Transport");
            this.loadTexture("Station");

            //Icons
            this.loadTexture("Move");
            this.loadTexture("Move_Done");
            
            this.loadTexture("DropResource");
            this.loadTexture("DropResource_Done");
            
            this.loadTexture("CreateResource");
            this.loadTexture("CreateResource_Done");

            this.loadTexture("PlaceUnit");
            this.loadTexture("PlaceUnit_Done");

            this.loadTexture("DestroyUnit");
            this.loadTexture("DestroyUnit_Done");

        }

        private void loadTexture(string p)
        {
            if (!m_dicTextures.ContainsKey(p))
                m_dicTextures.Add(p, base.Manager.Content.Load<Texture2D>("Sprites/" + p));
            else
                m_dicTextures[p] = base.Manager.Content.Load<Texture2D>("Sprites/" + p);

        }

        

#endregion 

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

        private void imgCommandIcon_ClickHandler(NuclearUI.Image sender)
        {
            Program.m_objCampaign.Command_getByID(m_dicCommandIcons[sender]).Register();
            initMap();
        }

        private void XWCommandPopup_Confirm(ICommand aktCommandType, clsCommandCollection objCommandCollection)
        {
            
            foreach (ICommand aktCommand in objCommandCollection.listReadyCommandsWithTypeFilter(aktCommandType))
                {
                    int x_offset = 0;
                    int y_offset = 0;

                    NuclearUI.Image imgBtn = new NuclearUI.Image(Manager.MenuScreen, this.m_dicTextures[aktCommand.strTypeName], false);
                    m_dicCommandIcons.Add(imgBtn, aktCommand.CommandId);
                    imgBtn.ClickHandler = new Action<NuclearUI.Image>(this.imgCommandIcon_ClickHandler);

                    if (aktCommand.GetType() == typeof(comMove) || aktCommand.GetType() == typeof(comDropResource) || aktCommand.GetType() == typeof(comPlaceUnit))
                    {
                        string[] str = ((ICommandWithSektor)aktCommand).TargetSektor.strUniqueID.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        x_offset = Convert.ToInt32(str[0]);
                        y_offset = Convert.ToInt32(str[1]);
                    }
                    else
                    {
                        clsGCML_Unit objGCML_Unit = new clsGCML_Unit(objCommandCollection.aktUnit);
                        x_offset = objGCML_Unit.aktSektor.objSektorKoord.X;
                        y_offset = objGCML_Unit.aktSektor.objSektorKoord.Y;
                    }

                    m_gridMap.AddChildAt(imgBtn, x_offset, y_offset);
                }
            }
        
        private void imgUnit_ClickHandler(NuclearUI.Image sender)
        {
            clsUnit aktUnit = m_dicUnits[sender];
    
            clsGCML_Unit Unit = new clsGCML_Unit(aktUnit);

            NuclearUI.GridGroup gridAction = new NuclearUI.GridGroup(Manager.MenuScreen, 3, 3, false, 0);
            m_gridMap.AddChildAt(gridAction, Unit.aktSektor.objSektorKoord.X, Unit.aktSektor.objSektorKoord.Y);
            gridAction.AnchoredRect = NuclearUI.AnchoredRect.CreateCentered(sender.ContentWidth, sender.ContentHeight);

            m_dicCommandIcons = new Dictionary<NuclearUI.Image, string>();

            Manager.XWCommandPopup.Setup(aktUnit, new Action<ICommand, clsCommandCollection>(this.XWCommandPopup_Confirm));
            Manager.XWCommandPopup.Open(500, 500);

        }

        protected void initMap()
        {
            
            this.Clear();

            int intGridWidth = Program.m_objCampaign.FieldField.FieldDimension.X + 1;
            int intGridHeight = Program.m_objCampaign.FieldField.FieldDimension.Y + 1;

            //Background Image
            NuclearUI.Image imgMap = new NuclearUI.Image(Manager.MenuScreen, m_dicTextures["Stars"], false);
            AddChild(imgMap);

            /*
            string strGrid = "Grid" + intGridWidth.ToString();
            if (this.m_dicTextures.ContainsKey(strGrid))
            {
                imgMap = new NuclearUI.Image(Manager.MenuScreen, m_dicTextures[strGrid], false);
                AddChild(imgMap);
            }
             * */

            //Grid erzeugen
            m_gridMap = new NuclearUI.GridGroup(Manager.MenuScreen, intGridHeight, intGridWidth, false, 0);
            AddChild(m_gridMap);
            
            //Grid auf das Background Image werfen
            m_gridMap.AnchoredRect = NuclearUI.AnchoredRect.CreateCentered(imgMap.ContentWidth, imgMap.ContentHeight);

            GenericCampaignMasterModel.Player aktPly = Program.m_objCampaign.Campaign_getStateForPlayer(m_intPlayerIndex.ToString());
            if (aktPly != null && aktPly.ListUnits != null)
            {
                
                foreach (Sektor aktSek in aktPly.dicVisibleSectors.Values)
                {
                    foreach (clsUnit aktUnit in aktSek.ListUnits)
                    {
                        drawUnit(aktUnit, aktPly);
                    }                    
                }
                
            }
        }
        
        private void drawUnit(clsUnit aktUnit, GenericCampaignMasterModel.Player aktPly)
        {
            clsGCML_Unit objUnit = new clsGCML_Unit(aktUnit);
            NuclearUI.Image imgUnit = null;

            foreach (string strTex in objUnit.strTexNames)
            {
                imgUnit = new NuclearUI.Image(Manager.MenuScreen, this.m_dicTextures[strTex], false);
                m_gridMap.AddChildAt(imgUnit, objUnit.aktSektor.objSektorKoord.X, objUnit.aktSektor.objSektorKoord.Y);
            }

            if (aktUnit.strOwnerID == aktPly.Id)
            {
                imgUnit.ClickHandler = new Action<NuclearUI.Image>(imgUnit_ClickHandler);
                m_dicUnits.Add(imgUnit, aktUnit);
            
                NuclearUI.Label lblZahl = new NuclearUI.Label(Manager.MenuScreen, aktUnit.cnt.ToString());
                m_gridMap.AddChildAt(lblZahl, objUnit.aktSektor.objSektorKoord.X, objUnit.aktSektor.objSektorKoord.Y);
            }

            if (objUnit.objUnit.aktCommand != null && !objUnit.objUnit.aktCommand.blnExecuted && objUnit.objUnit.strOwnerID == aktPly.Id)
            {
                NuclearUI.Image imgDoneIcon = new NuclearUI.Image(Manager.MenuScreen, this.m_dicTextures[objUnit.objUnit.aktCommand.strTypeName + "_Done"], false);

                imgDoneIcon.fltRotation = 0;

                int x_offset = 0;
                int y_offset = 0;

                if (objUnit.objUnit.aktCommand.GetType() == typeof(comMove) || objUnit.objUnit.aktCommand.GetType() == typeof(comDropResource) || objUnit.objUnit.aktCommand.GetType() == typeof(comPlaceUnit))
                {
                    //Token einblenden das den Command angezeigt
                    string[] str = ((ICommandWithSektor)objUnit.objUnit.aktCommand).TargetSektor.strUniqueID.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    x_offset = Convert.ToInt32(str[0]);
                    y_offset = Convert.ToInt32(str[1]);
                }
                else
                {
                    x_offset = objUnit.aktSektor.objSektorKoord.X;
                    y_offset = objUnit.aktSektor.objSektorKoord.Y;
                }

                if (imgDoneIcon != null)
                    m_gridMap.AddChildAt(imgDoneIcon, x_offset, y_offset);

            }
        }
        
    
    }
}
