using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using GenericCampaignMasterLib; 


using NuclearWinter;
using NuclearUI = NuclearWinter.UI;

namespace GCML_XNA_Client
{
    //--------------------------------------------------------------------------
    public class GCML_XNA_Client : NuclearGame
    {
        //----------------------------------------------------------------------
        internal GameStates.GameStateIntro      gmsIntro       { get; private set; }
        internal GameStates.GameStateMainMenu   gmsMainMenu    { get; private set; }

        //----------------------------------------------------------------------
        internal NuclearUI.Style                UIStyle     { get; private set; }

        //----------------------------------------------------------------------
        public GCML_XNA_Client()
        {
            Content.RootDirectory = "Content";

            Graphics.PreferredBackBufferWidth = 800;
            Graphics.PreferredBackBufferHeight = 600;

        }

        //----------------------------------------------------------------------
        protected override void Initialize()
        {
            base.Initialize();

            Program.objCampaignState = new GCML.clsCampaignInfo();
            Program.m_objCampaign = new CampaignBuilderSchach().buildNew(7);
           
            SetWindowTitle("GCML Demo");

            //------------------------------------------------------------------
            Window.AllowUserResizing = true;
            
            Form.MinimumSize = new System.Drawing.Size(800,600);

            IsMouseVisible = true;

            //------------------------------------------------------------------
            // Game states
            gmsIntro       = new GameStates.GameStateIntro( this );
            gmsMainMenu    = new GameStates.GameStateMainMenu( this );
            GameStateMgr.SwitchState(gmsMainMenu);

            //------------------------------------------------------------------
            Form.Resize += delegate { EnsureProperPresentationParams(); };
        }

        //----------------------------------------------------------------------
        void EnsureProperPresentationParams()
        {
            if( Form.ClientSize.IsEmpty ) return;

            if( Form.ClientSize.Width != GraphicsDevice.Viewport.Width
            ||  Form.ClientSize.Height != GraphicsDevice.Viewport.Height )
            {
                PresentationParameters updatedPresentationParams = GraphicsDevice.PresentationParameters.Clone();
                updatedPresentationParams.BackBufferWidth   = Form.ClientSize.Width;
                updatedPresentationParams.BackBufferHeight  = Form.ClientSize.Height;
                GraphicsDevice.Reset( updatedPresentationParams );
            }
        }

        //----------------------------------------------------------------------
        protected override void LoadContent()
        {
            base.LoadContent();

            //------------------------------------------------------------------
            // UI Style
            UIStyle = new NuclearUI.Style();

            UIStyle.SmallFont   = new NuclearUI.UIFont( Content.Load<SpriteFont>( "Fonts/SmallFont" ), 14, 0 );
            UIStyle.MediumFont  = new NuclearUI.UIFont( Content.Load<SpriteFont>( "Fonts/MediumFont" ), 18, -2 );
            UIStyle.LargeFont   = new NuclearUI.UIFont( Content.Load<SpriteFont>( "Fonts/LargeFont" ), 24, 0 );

            UIStyle.BlurRadius = 0;
            UIStyle.SpinningWheel               = Content.Load<Texture2D>( "Sprites/UI/SpinningWheel" );

            UIStyle.DefaultTextColor            = new Color( 224, 224, 224 );
            UIStyle.DefaultButtonHeight         = 60;

            UIStyle.ButtonFrame                 = Content.Load<Texture2D>( "Sprites/UI/ButtonFrame" );
            UIStyle.ButtonHover                 = Content.Load<Texture2D>( "Sprites/UI/ButtonHover" );
            UIStyle.ButtonFocus                 = Content.Load<Texture2D>( "Sprites/UI/ButtonFocus" );

            UIStyle.ButtonFrameDown             = Content.Load<Texture2D>( "Sprites/UI/ButtonFrameDown" );
            UIStyle.ButtonPress                 = Content.Load<Texture2D>( "Sprites/UI/ButtonPress" );
            UIStyle.ButtonDownFocus             = Content.Load<Texture2D>( "Sprites/UI/ButtonFocus" );

            UIStyle.TooltipFrame                = Content.Load<Texture2D>( "Sprites/UI/TooltipFrame" );

            UIStyle.ButtonCornerSize            = 20;
            UIStyle.ButtonVerticalPadding       = 10;
            UIStyle.ButtonHorizontalPadding     = 15;

            UIStyle.RadioButtonCornerSize       = UIStyle.ButtonCornerSize;
            UIStyle.RadioButtonFrameOffset      = 7;
            UIStyle.ButtonFrameLeft             = Content.Load<Texture2D>( "Sprites/UI/ButtonFrameLeft" );
            UIStyle.ButtonFrameLeftDown         = Content.Load<Texture2D>( "Sprites/UI/ButtonFrameLeftDown" );

            UIStyle.ButtonFrameMiddle           = Content.Load<Texture2D>( "Sprites/UI/ButtonFrameMiddle" );
            UIStyle.ButtonFrameMiddleDown       = Content.Load<Texture2D>( "Sprites/UI/ButtonFrameMiddleDown" );

            UIStyle.ButtonFrameRight            = Content.Load<Texture2D>( "Sprites/UI/ButtonFrameRight" );
            UIStyle.ButtonFrameRightDown        = Content.Load<Texture2D>( "Sprites/UI/ButtonFrameRightDown" );

            UIStyle.EditBoxFrame                = Content.Load<Texture2D>( "Sprites/UI/EditBoxFrame" );
            UIStyle.EditBoxCornerSize           = 20;

            UIStyle.Panel                       = Content.Load<Texture2D>( "Sprites/UI/Panel01" );
            UIStyle.PanelCornerSize             = 15;

            UIStyle.NotebookTabCornerSize       = 15;
            UIStyle.NotebookTab                 = Content.Load<Texture2D>( "Sprites/UI/Tab" );
            UIStyle.NotebookTabFocus            = Content.Load<Texture2D>( "Sprites/UI/ButtonFocus" );
            UIStyle.NotebookActiveTab           = Content.Load<Texture2D>( "Sprites/UI/ActiveTab" );
            UIStyle.NotebookActiveTabFocus      = Content.Load<Texture2D>( "Sprites/UI/ActiveTabFocused" );
            UIStyle.NotebookTabClose            = Content.Load<Texture2D>( "Sprites/UI/TabClose" );
            UIStyle.NotebookTabCloseHover       = Content.Load<Texture2D>( "Sprites/UI/TabCloseHover" );
            UIStyle.NotebookTabCloseDown        = Content.Load<Texture2D>( "Sprites/UI/TabCloseDown" );
            UIStyle.NotebookUnreadTabMarker     = Content.Load<Texture2D>( "Sprites/UI/UnreadTabMarker" );

            UIStyle.ListFrame                   = Content.Load<Texture2D>( "Sprites/UI/ListFrame" );
            UIStyle.ListRowInsertMarker         = Content.Load<Texture2D>( "Sprites/UI/ListRowInsertMarker" );

            UIStyle.GridBoxFrameCornerSize      = 10;
            UIStyle.GridBoxFrame                = Content.Load<Texture2D>( "Sprites/UI/ListRowFrame" );
            UIStyle.GridBoxFrameSelected        = Content.Load<Texture2D>( "Sprites/UI/ListRowFrameSelected" );
            UIStyle.GridBoxFrameFocus           = Content.Load<Texture2D>( "Sprites/UI/ListRowFrameFocused" );
            UIStyle.GridBoxFrameHover           = Content.Load<Texture2D>( "Sprites/UI/ListRowFrameHover" );
            UIStyle.GridHeaderFrame             = Content.Load<Texture2D>( "Sprites/UI/ButtonFrame" ); // FIXME

            UIStyle.PopupFrame                  = Content.Load<Texture2D>( "Sprites/UI/PopupFrame" );
            UIStyle.PopupFrameCornerSize        = 30;

            UIStyle.TreeViewContainerFrameSelected = Content.Load<Texture2D>( "Sprites/UI/TreeViewContainerFrameSelected" );

            UIStyle.TreeViewBranchOpen          = Content.Load<Texture2D>( "Sprites/UI/TreeViewBranchOpen" );
            UIStyle.TreeViewBranchOpenEmpty     = Content.Load<Texture2D>( "Sprites/UI/TreeViewBranchOpenEmpty" );
            UIStyle.TreeViewBranchClosed        = Content.Load<Texture2D>( "Sprites/UI/TreeViewBranchClosed" );
            UIStyle.TreeViewBranch              = Content.Load<Texture2D>( "Sprites/UI/TreeViewBranch" );
            UIStyle.TreeViewBranchLast          = Content.Load<Texture2D>( "Sprites/UI/TreeViewBranchLast" );
            UIStyle.TreeViewLine                = Content.Load<Texture2D>( "Sprites/UI/TreeViewLine" );
            UIStyle.TreeViewCheckBoxFrame       = Content.Load<Texture2D>( "Sprites/UI/TreeViewCheckBoxFrame" );

            UIStyle.CheckBoxFrameHover          = Content.Load<Texture2D>( "Sprites/UI/CheckBoxFrameHover" );
            UIStyle.CheckBoxChecked             = Content.Load<Texture2D>( "Sprites/UI/Checked" );
            UIStyle.CheckBoxUnchecked           = Content.Load<Texture2D>( "Sprites/UI/Unchecked" );

            UIStyle.VerticalScrollbar           = Content.Load<Texture2D>( "Sprites/UI/VerticalScrollbar" );
            UIStyle.VerticalScrollbarCornerSize = 5;

            UIStyle.DropDownArrow               = Content.Load<Texture2D>( "Sprites/UI/DropDownArrow" );
            UIStyle.SplitterFrame               = Content.Load<Texture2D>( "Sprites/UI/SplitterFrame" );
            UIStyle.SplitterDragHandle          = Content.Load<Texture2D>( "Sprites/UI/SplitterDragHandle" );
            UIStyle.SplitterCollapseArrow       = Content.Load<Texture2D>( "Sprites/UI/SplitterCollapseArrow" );

            UIStyle.ProgressBarFrame            = Content.Load<Texture2D>( "Sprites/UI/EditBoxFrame" );
            UIStyle.ProgressBarFrameCornerSize  = 15;
            UIStyle.ProgressBar                 = Content.Load<Texture2D>( "Sprites/UI/ProgressBar" );
            UIStyle.ProgressBarCornerSize       = 15;

            UIStyle.TextAreaGutterFrame         = Content.Load<Texture2D>( "Sprites/UI/TextAreaGutterFrame" );
            UIStyle.TextAreaGutterCornerSize    = 15;

            EnsureProperPresentationParams();
        }

        //----------------------------------------------------------------------
        protected override void Update( GameTime _time )
        {
            base.Update( _time );
        }

        //----------------------------------------------------------------------
        protected override void Draw( GameTime _time )
        {
            base.Draw( _time );
        }
    }
}
