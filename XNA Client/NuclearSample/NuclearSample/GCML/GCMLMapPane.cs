using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using NuclearUI = NuclearWinter.UI;

namespace NuclearSample.GCML
{
    class GCMLMapPane: NuclearUI.ManagerPane<MainMenuManager>
    {
        public GCMLMapPane(MainMenuManager _manager) : base(_manager)
        {
            this.initTextures();
            this.initMap();

        }
            /*
            NuclearUI.Image imgLandser = new NuclearUI.Image(Manager.MenuScreen, texLandser, false);
            imgLandser.ClickHandler = delegate
            {
                Manager.MessagePopup.Setup("Oh Nein", "Verdammt!", NuclearWinter.i18n.Common.Close, false);
                Manager.MessagePopup.Open(600, 250);
            };
            gridGroup.AddChildAt(imgLandser, 0, 2);

            NuclearUI.Image imgGI = new NuclearUI.Image(Manager.MenuScreen, texGi, false);
            imgGI.ClickHandler = delegate
            {
                Manager.MessagePopup.Setup("Oh noes2", "Sorry.", NuclearWinter.i18n.Common.Close, false);
                Manager.MessagePopup.Open(600, 250);
            };
            gridGroup.AddChildAt(imgGI, 1, 0);    
            
            */
           
        
        
        protected void initMap()
        {
            //Background Image
            NuclearUI.Image imgMap = new NuclearUI.Image(Manager.MenuScreen, m_dicTextures["Map_klein"], false);
            AddChild(imgMap);

            //Grid erzeugen
            NuclearUI.GridGroup gridGroup = new NuclearUI.GridGroup(Manager.MenuScreen, 3, 3, false, 0);
            AddChild(gridGroup);

            //Grid auf das Background Image werfen
            gridGroup.AnchoredRect = NuclearUI.AnchoredRect.CreateCentered(imgMap.ContentWidth, imgMap.ContentHeight);
           
        }
        
        private Dictionary<string, Texture2D> m_dicTextures;
         
        private void initTextures()
        {
            m_dicTextures = new Dictionary<string, Texture2D>();
            this.loadTexture("Map_klein");
            this.loadTexture("IconGI");
            this.loadTexture("IconLandser");
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
