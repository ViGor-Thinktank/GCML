using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NuclearSample.GameStates
{
    //--------------------------------------------------------------------------
    internal class GameStateMainMenu: NuclearWinter.GameFlow.GameStateFadeTransition<NuclearSampleGame>
    {
        public Texture2D texMapTex;
        public Texture2D texIconGI;
        public Texture2D texIconLandser;

        //----------------------------------------------------------------------
        public MainMenuManager      MainMenuManager { get; private set; }

        //----------------------------------------------------------------------
        public GameStateMainMenu( NuclearSampleGame _game )
        : base( _game )
        {

        }

        //----------------------------------------------------------------------
        public override void Start()
        {
            this.texMapTex = Content.Load<Texture2D>("Sprites/Map_klein");
            this.texIconLandser = Content.Load<Texture2D>("Sprites/IconLandser");
            this.texIconGI = Content.Load<Texture2D>("Sprites/IconGI");

            MainMenuManager = new MainMenuManager( Game, Content );
            Game.IsMouseVisible = true;

            Game.GraphicsDevice.DeviceReset += GraphicsDevice_DeviceReset;


            base.Start();
        }

        //----------------------------------------------------------------------
        public override void Stop()
        {
            Game.GraphicsDevice.DeviceReset -= GraphicsDevice_DeviceReset;

            MainMenuManager = null;

            base.Stop();
        }
        
        void GraphicsDevice_DeviceReset( object sender, EventArgs e )
        {
            MainMenuManager.MenuScreen.Resize( Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height );
            MainMenuManager.PopupScreen.Resize( Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height );
        }

        //----------------------------------------------------------------------
        public override void Update( float _fElapsedTime )
        {
            MainMenuManager.Update( _fElapsedTime );
        }

        //----------------------------------------------------------------------
        public override void Draw()
        {
            Game.GraphicsDevice.Clear( new Color( 111, 125, 120 ) );
            
            /*Game.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Game.SpriteMatrix);
            Game.SpriteBatch.Draw(this.texMapTex, new Vector2(20, 20), Color.White);
            Game.SpriteBatch.End();
        */
            MainMenuManager.Draw();
        }
    }
}
