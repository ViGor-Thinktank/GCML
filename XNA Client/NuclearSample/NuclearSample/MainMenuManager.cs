using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NuclearUI = NuclearWinter.UI;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NuclearSample
{
    //--------------------------------------------------------------------------
    internal class MainMenuManager: NuclearUI.MenuManager<NuclearSampleGame>
    {
        public NuclearUI.Splitter      mSplitter;
        public NuclearUI.Panel         mRightPanel;

        //----------------------------------------------------------------------
        public MainMenuManager( NuclearSampleGame _game, ContentManager _content )
        : base( _game, _game.UIStyle, _content )
        {
            // Splitter
            mSplitter = new NuclearUI.Splitter( MenuScreen, NuclearUI.Direction.Left );
            mSplitter.AnchoredRect = NuclearUI.AnchoredRect.CreateFull( 10 );
            MenuScreen.Root.AddChild( mSplitter );
            mSplitter.Collapsable = true;

            mSplitter.FirstPaneMinSize = 200;

            // Demo list
            NuclearUI.BoxGroup objBoxGroup = new NuclearUI.BoxGroup( MenuScreen, NuclearUI.Orientation.Vertical, 0, NuclearUI.Anchor.Start );
            mSplitter.FirstPane = objBoxGroup;

            mRightPanel = new NuclearUI.Panel( MenuScreen, Content.Load<Texture2D>( "Sprites/UI/Panel04" ), MenuScreen.Style.PanelCornerSize );

            Demos.BasicDemoPane basicDemoPane = new Demos.BasicDemoPane( this );
            mSplitter.SecondPane = mRightPanel;

            //mDemoPanel.AddChild( basicDemoPane );

            //demosBoxGroup.AddChild(CreateDemoButton("Basic", basicDemoPane), true);

            objBoxGroup.AddChild(CreateDemoButton("GCML Admin", new GCML.GCMLAdminPane(this)), true);
            objBoxGroup.AddChild(CreateDemoButton("Map", new GCML.GCMLMapPane_XW(this)), true);
            //objBoxGroup.AddChild(CreateDemoButton("bsps Stuff", new NuclearSample.Demos.NotebookPane(this)), true);

            NuclearUI.Button button = new NuclearUI.Button(MenuScreen, "Tick");
            button.ClickHandler = delegate
            {
                Program.m_objCampaign.Tick();
            };
            objBoxGroup.AddChild(button);
            

            /*demosBoxGroup.AddChild( CreateDemoButton( "Basic", basicDemoPane ), true );
            demosBoxGroup.AddChild( CreateDemoButton( "Notebook", new Demos.NotebookPane( this ) ), true );
            demosBoxGroup.AddChild( CreateDemoButton( "Text Area", new Demos.TextAreaPane( this ) ), true );
            demosBoxGroup.AddChild(CreateDemoButton("Text Area 2", new Demos.TextAreaPane(this)), true);
            demosBoxGroup.AddChild( CreateDemoButton( "Leer", new Demos.CustomViewportPane( this ) ), true );
             * */
        }

        //----------------------------------------------------------------------
        NuclearUI.Button CreateDemoButton( string _strDemoName, NuclearUI.ManagerPane<MainMenuManager> _demoPane )
        {
            NuclearUI.Button demoPaneButton = new NuclearUI.Button( MenuScreen, _strDemoName );
            demoPaneButton.ClickHandler = delegate {
                mRightPanel.Clear();
                mRightPanel.AddChild( _demoPane );
            };

            return demoPaneButton;
        }
    }
}
