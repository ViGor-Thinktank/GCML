using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace NuclearWinter.GameFlow
{
    //--------------------------------------------------------------------------
    /// Each GameState handles part of the game : menu, in-game, settings, etc.
    public abstract class GameState<T>: IGameState<T> where T:NuclearGame
    {
        //----------------------------------------------------------------------
        public GameState( T _game )
        {
            Game = _game;
            Content = new ContentManager( _game.Services );
            Content.RootDirectory = "Content";
        }

        //----------------------------------------------------------------------
        // Start the GameState, called when it becomes the current one
        public virtual void Start()
        {
            Game.ResetElapsedTime();
        }

        //----------------------------------------------------------------------
        // Stop the GameState, called when switching to another one
        public virtual void Stop()
        {
            Content.Unload();
        }

        //----------------------------------------------------------------------
        public virtual void OnActivated() {}
        public virtual void OnExiting() {}

        //----------------------------------------------------------------------
        // Called repeatedly when starting the GameState, until it returns true
        public virtual bool UpdateFadeIn( float _fElapsedTime )
        {
            // NOTE: Don't call Update() here, it'll be done automatically since we return true
            return true;
        }

        //----------------------------------------------------------------------
        // Draw while fading in
        public virtual void DrawFadeIn()
        {
            Draw();
        }

        //----------------------------------------------------------------------
        // Called repeatedly when stopping the GameState, until it returns true
        public virtual bool UpdateFadeOut( float _fElapsedTime )
        {
            // NOTE: Don't call Update() here, it'll be done automatically since we return true
            return true;
        }

        //----------------------------------------------------------------------
        // Draw while fading out
        public virtual void DrawFadeOut()
        {
            Draw();
        }

        //----------------------------------------------------------------------
        // Updates the GameState
        public abstract void Update( float _fElapsedTime );
        
        //----------------------------------------------------------------------
        // Draw the GameState
        public abstract void Draw();

        //----------------------------------------------------------------------
        public readonly T           Game;
        public ContentManager       Content;
    }
}
