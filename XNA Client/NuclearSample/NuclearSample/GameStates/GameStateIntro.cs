using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using NuclearAnim = NuclearWinter.Animation;

namespace GCML_XNA_Client.GameStates
{
    //--------------------------------------------------------------------------
    internal class GameStateIntro: NuclearWinter.GameFlow.GameStateFadeTransition<GCML_XNA_Client>
    {
        //----------------------------------------------------------------------
        NuclearAnim.LerpValue               mSwitchTimer;
        NuclearAnim.AnimatedValue           mLogoAnim;
        
        Texture2D                           mLogoTex;

        Random                              mRandom;

        //----------------------------------------------------------------------
        public GameStateIntro( GCML_XNA_Client _game )
        : base( _game )
        {

        }

        //----------------------------------------------------------------------
        public override void Start()
        {
            mRandom = new Random();

            mLogoAnim = new NuclearAnim.LerpValue( 0f, 1f, 0.3f, 0.3f );
        
            mLogoTex = Content.Load<Texture2D>("Sprites/gcml_logo_big");

            mSwitchTimer = new NuclearAnim.LerpValue( 0f, 1f, 1f, 1.5f );

            base.Start();
        }

        //----------------------------------------------------------------------
        public override void Update( float _fElapsedTime )
        {
            mLogoAnim.Update( _fElapsedTime );
            mSwitchTimer.Update( _fElapsedTime );

            if( mSwitchTimer.IsOver && ! Game.GameStateMgr.IsSwitching )
            {
                Game.GameStateMgr.SwitchState( Game.gmsMainMenu );
            }
        }

        //----------------------------------------------------------------------
        public override void Draw()
        {
            Game.GraphicsDevice.Clear( new Color( 45, 51, 49 ) );

            Game.SpriteBatch.Begin( SpriteSortMode.Deferred, null, null, null, null, null, Game.SpriteMatrix );

            Vector2 vScreenCenter = new Vector2( Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height ) / 2f;

            Vector2 vLogoOrigin     = new Vector2( mLogoTex.Width, mLogoTex.Height ) / 2f;

            float fTitleOffsetAngle = (float)mRandom.NextDouble() * MathHelper.TwoPi;
            
            Game.SpriteBatch.Draw(mLogoTex, vScreenCenter + new Vector2(-mLogoTex.Width / 2, -mLogoTex.Height / 2) + vLogoOrigin, null, Color.White * mLogoAnim.CurrentValue, 0f, vLogoOrigin, Vector2.One * (2f - (float)Math.Pow(mLogoAnim.CurrentValue, 3)), SpriteEffects.None, 0f);

            Game.SpriteBatch.End();
        }
    }
}
