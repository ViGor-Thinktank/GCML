using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NuclearUI = NuclearWinter.UI;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GCML_XNA_Client
{
    //--------------------------------------------------------------------------
    internal class MainMenuManager: NuclearUI.MenuManager<GCML_XNA_Client>
    {
        public NuclearUI.Splitter      mSplitterLinks;
        //public NuclearUI.Splitter      mSplitterRechts;
        public NuclearUI.Panel         mCenterPanel;
        
      
        //----------------------------------------------------------------------
        public MainMenuManager( GCML_XNA_Client _game, ContentManager _content )
        : base( _game, _game.UIStyle, _content )
        {
            // Splitter
            mSplitterLinks = new NuclearUI.Splitter( MenuScreen, NuclearUI.Direction.Left );
            mSplitterLinks.AnchoredRect = NuclearUI.AnchoredRect.CreateFull( 10 );
            mSplitterLinks.Collapsable = true;
            mSplitterLinks.FirstPaneMinSize = 200;
            
            MenuScreen.Root.AddChild(mSplitterLinks);

            // Linkes Menü
            NuclearUI.BoxGroup objBoxGroup = new NuclearUI.BoxGroup( MenuScreen, NuclearUI.Orientation.Vertical, 0, NuclearUI.Anchor.Start );
            mSplitterLinks.FirstPane = objBoxGroup;

            /*mSplitterRechts = new NuclearUI.Splitter(MenuScreen, NuclearUI.Direction.Right);
            mSplitterRechts.AnchoredRect = NuclearUI.AnchoredRect.CreateFull(10);
            mSplitterRechts.Collapsable = false;
            mSplitterRechts.FirstPaneMinSize = 200;

            mSplitterLinks.SecondPane = mSplitterRechts;

            NuclearUI.BoxGroup objBoxGroup_rechts = new NuclearUI.BoxGroup(MenuScreen, NuclearUI.Orientation.Vertical, 0, NuclearUI.Anchor.Start);
            mSplitterRechts.FirstPane = objBoxGroup_rechts;
            */

            mCenterPanel = new NuclearUI.Panel(MenuScreen, Content.Load<Texture2D>("Sprites/UI/Panel04"), MenuScreen.Style.PanelCornerSize);
            //mSplitterRechts.SecondPane = mCenterPanel;
            mSplitterLinks.SecondPane = mCenterPanel;

            objBoxGroup.AddChild(CreateMapPageButton("GCML Admin", new GCML.GCMLAdminPane(this)), true);
            objBoxGroup.AddChild(CreateMapPageButton("Rebel Player Map", new GCML.GCMLMapPane_XW(this, 0)), true);
            objBoxGroup.AddChild(CreateMapPageButton("Empire Player Map", new GCML.GCMLMapPane_XW(this, 1)), true);
            
            NuclearUI.Button msgButton = new NuclearUI.Button( MenuScreen, "Msgbox" );
            msgButton.ClickHandler = delegate {
                
            };
            objBoxGroup.AddChild(msgButton);


            NuclearUI.Button button = new NuclearUI.Button(MenuScreen, "Tick");
            button.ClickHandler = delegate
            {
                Program.m_objCampaign.Tick();
            };
            objBoxGroup.AddChild(button);
        }

        //----------------------------------------------------------------------
        NuclearUI.Button CreateMapPageButton( string _strDemoName, NuclearUI.ManagerPane<MainMenuManager> _demoPane )
        {
            NuclearUI.Button demoPaneButton = new NuclearUI.Button( MenuScreen, _strDemoName );
            demoPaneButton.ClickHandler = delegate {
                /**/
                mCenterPanel.Clear();
                mCenterPanel.AddChild( _demoPane );
                
            };

            return demoPaneButton;
        }
    }
}
