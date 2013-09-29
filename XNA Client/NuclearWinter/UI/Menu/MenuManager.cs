using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using NuclearWinter;
using System.Diagnostics;
using System.Collections.Generic;
using System;

namespace NuclearWinter.UI
{
    public abstract class MenuManager<T>: IMenuManager where T:NuclearGame
    {
        public T                        Game                    { get; private set; }
        public ContentManager           Content                 { get; private set; }

        //----------------------------------------------------------------------
        // Menu
        public Screen                   MenuScreen              { get; private set; }

        //----------------------------------------------------------------------
        // Popup
        public Screen                       PopupScreen             { get; private set; }
        public MessagePopup                 MessagePopup            { get; private set; }
        public GCML.clsXWCommandPopupBase   XWCommandPopup { get; set; }

        public IEnumerable<Panel>       PopupStack              { get { return (IEnumerable<Panel>)mPopupStack; } }
        public Panel                    TopMostPopup            { get { return mPopupStack.Count > 0 ? mPopupStack.Peek() : null; } }

        Stack<Panel>                    mPopupStack;
        NuclearWinter.UI.Image          mPopupFade;

        //----------------------------------------------------------------------
        public MenuManager( T _game, Style _style, ContentManager _content )
        {
            Game        = _game;
            Content     = _content;

            //------------------------------------------------------------------
            // Menu
            MenuScreen  = new NuclearWinter.UI.Screen( _game, _style, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height );

            //------------------------------------------------------------------
            // Popup
            PopupScreen         = new NuclearWinter.UI.Screen( _game, _style, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height );
            mPopupStack         = new Stack<Panel>();
            MessagePopup        = new MessagePopup( this );
            XWCommandPopup      = null;
            
            mPopupFade          = new NuclearWinter.UI.Image( PopupScreen, Game.WhitePixelTex, true );
            mPopupFade.Color    = PopupScreen.Style.PopupBackgroundFadeColor;
        }

        //----------------------------------------------------------------------
        public virtual void Update( float _fElapsedTime )
        {
            MenuScreen.IsActive     = Game.IsActive && ( Game.GameStateMgr == null || ! Game.GameStateMgr.IsSwitching ) && mPopupStack.Count == 0;
            PopupScreen.IsActive    = Game.IsActive && ( Game.GameStateMgr == null || ! Game.GameStateMgr.IsSwitching ) && mPopupStack.Count > 0;

            MenuScreen.HandleInput();
            if( mPopupStack.Count > 0 )
            {
                PopupScreen.HandleInput();
            }

            MenuScreen.Update( _fElapsedTime );
            if( mPopupStack.Count > 0 )
            {
                PopupScreen.Update( _fElapsedTime );
            }
        }

        //----------------------------------------------------------------------
        public void Draw()
        {
            MenuScreen.Draw();

            if( mPopupStack.Count > 0 )
            {
                PopupScreen.Draw();
            }
        }

        //----------------------------------------------------------------------
        public void PushPopup( Panel _popup )
        {
            if( mPopupStack.Contains( _popup ) ) throw new InvalidOperationException( "Cannot push same popup twice" );

            mPopupStack.Push( _popup );

            PopupScreen.Root.Clear();
            PopupScreen.Root.AddChild( mPopupFade );
            PopupScreen.Root.AddChild( (Panel)_popup );
        }

        //----------------------------------------------------------------------
        // NOTE: This method takes the removed popup as an argument to help ensure consistency
        public void PopPopup( Panel _popup )
        {
            if( mPopupStack.Count == 0 || mPopupStack.Peek() != _popup ) throw new InvalidOperationException( "Cannot pop a popup if it isn't at the top of the stack" );

            mPopupStack.Pop();

            PopupScreen.Root.Clear();

            if( mPopupStack.Count > 0 )
            {
                PopupScreen.Root.AddChild( mPopupFade );

                var panel = (Panel)mPopupStack.Peek();
                PopupScreen.Root.AddChild( panel );
                PopupScreen.Focus( panel );
            }
        }

       
        
    }
}
