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
        public NuclearUI.Panel         mCenterPanel;

        public NuclearUI.Label         mMessageLabel;


        //----------------------------------------------------------------------
        public MainMenuManager( GCML_XNA_Client _game, ContentManager _content )
        : base( _game, _game.UIStyle, _content )
        {
            // Splitter
            mSplitterLinks = new NuclearUI.Splitter( MenuScreen, NuclearUI.Direction.Left );
            mSplitterLinks.AnchoredRect = NuclearUI.AnchoredRect.CreateFull( 10 );
            mSplitterLinks.Collapsable = true;
            mSplitterLinks.FirstPaneMinSize = 300;
            
            MenuScreen.Root.AddChild(mSplitterLinks);

            NuclearUI.BoxGroup objBoxGroup = new NuclearUI.BoxGroup( MenuScreen, NuclearUI.Orientation.Vertical, 0, NuclearUI.Anchor.Start );
            mSplitterLinks.FirstPane = objBoxGroup;

            mCenterPanel = new NuclearUI.Panel(MenuScreen, Content.Load<Texture2D>("Sprites/UI/Panel04"), MenuScreen.Style.PanelCornerSize);
            mSplitterLinks.SecondPane = mCenterPanel;


            mMessageLabel = new NuclearUI.Label(MenuScreen, "", NuclearUI.Anchor.Start);
            mMessageLabel.WrapText = true;

            mMessageLabel.Text = "In a Galaxy far far away";

            objBoxGroup.AddChild(mMessageLabel, true);

            NuclearUI.Button button = new NuclearUI.Button(MenuScreen, "Tick");
            button.ClickHandler = delegate
            {
                Program.m_objCampaign.Tick();
            };
            objBoxGroup.AddChild(button, false);

            objBoxGroup.AddChild(CreateMapPageButton("Rebel Player Map", new GCML.GCMLMapPane_XW(this, 0)), false);
            objBoxGroup.AddChild(CreateMapPageButton("Empire Player Map", new GCML.GCMLMapPane_XW(this, 1)), false);

            objBoxGroup.AddChild(CreateMapPageButton("GCML Admin", new GCML.GCMLAdminPane(this)), false);

            //objBoxGroup.AddChild(CreateMapPageButton("Note Pain", new Demos.NotebookPane(this)), false);
            
        }

        //----------------------------------------------------------------------
        NuclearUI.Button CreateMapPageButton( string _strName, NuclearUI.ManagerPane<MainMenuManager> _Pane )
        {
            NuclearUI.Button PaneButton = new NuclearUI.Button( MenuScreen, _strName );
            PaneButton.ClickHandler = delegate {
                /**/
                mCenterPanel.Clear();
                mCenterPanel.AddChild( _Pane );
                
            };

            return PaneButton;
        }
    }
}
