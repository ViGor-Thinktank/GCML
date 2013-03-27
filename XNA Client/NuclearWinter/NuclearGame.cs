using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

#if WINDOWS
using System.Windows.Forms;
using System.Runtime.InteropServices;
#endif

#if MONOMAC
using MonoMac.Foundation;
#endif

#if WINDOWS_PHONE
using Microsoft.Phone.Shell;
#endif

namespace NuclearWinter
{
    public class NuclearGame: Game
    {
        //----------------------------------------------------------------------
        public Texture2D                            WhitePixelTex           { get; protected set; }
        public Matrix                               SpriteMatrix            { get; protected set; }

        //----------------------------------------------------------------------
        public GraphicsDeviceManager                Graphics                { get; private set; }
        public SpriteBatch                          SpriteBatch             { get; private set; }
        public RasterizerState                      ScissorRasterizerState  { get; private set; }

        
#if WINDOWS || LINUX || MACOSX || XBOX
        // Index of the player responsible for menu navigation (or null if none yet)
        public PlayerIndex?                         PlayerInCharge;
#endif

        public static readonly string               ApplicationDataFolderPath;

        public Storage.SaveHandler                  NuclearSaveHandler;
#if WINDOWS || LINUX || MACOSX || XBOX
        public StorageDevice                        SaveGameStorageDevice;
        bool                                        mbShouldDisplayStorageSelector;
        Action                                      DeviceSelectedCallback;
#endif

        //----------------------------------------------------------------------
        // Sound & Music
        public Song                                 Song;

        //----------------------------------------------------------------------
        // Game States
        protected bool                              UseGameStateManager;
        public GameFlow.GameStateMgr<NuclearGame>   GameStateMgr            { get; private set; }

        //----------------------------------------------------------------------
        // Input
#if WINDOWS || LINUX || MACOSX || XBOX
        public Input.InputManager                   InputMgr                { get; private set; }
#endif

#if WINDOWS_PHONE
        public Input.TouchManager                   TouchMgr                { get; private set; }
#endif

        public const float                          LerpMultiplier = 15f;

        //----------------------------------------------------------------------
#if WINDOWS

#if !MONOGAME
        public Form                                 Form                    { get; private set; }
#endif

        [DllImport("user32.dll")]
        static extern bool IsWindowUnicode(IntPtr hWnd);

        /// <summary>
        /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs..</param>
        /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer. To set any other value, specify one of the following values: GWL_EXSTYLE, GWL_HINSTANCE, GWL_ID, GWL_STYLE, GWL_USERDATA, GWL_WNDPROC </param>
        /// <param name="dwNewLong">The replacement value.</param>
        /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer. 
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError. </returns>
        [DllImport("user32.dll")]
        static extern int SetWindowLongW(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError=true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowTextW( IntPtr hwnd, string strText );

        [DllImport("user32.dll", SetLastError=true, CharSet=CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public enum GWL
        {
             GWL_WNDPROC =      (-4),
             GWL_HINSTANCE =    (-6),
             GWL_HWNDPARENT =   (-8),
             GWL_STYLE =        (-16),
             GWL_EXSTYLE =      (-20),
             GWL_USERDATA =     (-21),
             GWL_ID =           (-12)
        }
#endif

        static NuclearGame()
        {
#if !MONOMAC
            ApplicationDataFolderPath = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData );
#else
            NSError error;
            ApplicationDataFolderPath = NSFileManager.DefaultManager.GetUrl( NSSearchPathDirectory.LibraryDirectory, NSSearchPathDomain.User, new NSUrl(), true, out error ).Path;
#endif
        }

        //----------------------------------------------------------------------
        public NuclearGame( bool _bUseGameStateManager=true )
        {
#if WINDOWS_PHONE
            PhoneApplicationService.Current.Deactivated += new EventHandler<DeactivatedEventArgs>( OnDeactivated );
            PhoneApplicationService.Current.Closing     += new EventHandler<ClosingEventArgs>( OnClosing );
#endif

#if WINDOWS || LINUX || MACOSX || XBOX
            StorageDevice.DeviceChanged                 += new EventHandler<EventArgs>( OnStorageDeviceChange );
#endif

#if WINDOWS

#if !MONOGAME
            Form = (Form)Form.FromHandle( Window.Handle );
#endif

            // Is the Game window unicode-aware?
            if( ! IsWindowUnicode( Window.Handle ) )
            {
                // No? Well, no problem, we'll just make it aware!

                int iTitleLength = GetWindowTextLength( Window.Handle );
                StringBuilder sbTitle = new StringBuilder(iTitleLength + 1);
                GetWindowText( Window.Handle, sbTitle, sbTitle.Capacity);

                SetWindowLongW( Window.Handle, (int)GWL.GWL_WNDPROC, GetWindowLong( Window.Handle, (int)GWL.GWL_WNDPROC ) );
                SetWindowTextW( Window.Handle, sbTitle.ToString() ); 
            }
#endif

            Graphics = new GraphicsDeviceManager(this);
            SpriteMatrix = Matrix.Identity;

            UseGameStateManager = _bUseGameStateManager;
        }

        //----------------------------------------------------------------------
        /// <summary>
        /// Sets the window title (Unicode-aware)
        /// </summary>
        public void SetWindowTitle( string _strTitle )
        {
#if !MONOGAME
            SetWindowTextW( Window.Handle, _strTitle );
#else
            Window.Title = _strTitle;
#endif
        }

        //----------------------------------------------------------------------
        protected override void Initialize()
        {
            SpriteBatch = new SpriteBatch( GraphicsDevice );

            ScissorRasterizerState = new RasterizerState();
            ScissorRasterizerState.ScissorTestEnable = true;

            if( UseGameStateManager )
            {
                GameStateMgr = new GameFlow.GameStateMgr<NuclearGame>( this );
                Components.Add( GameStateMgr );
            }

#if WINDOWS || LINUX || MACOSX || XBOX
            InputMgr                = new Input.InputManager( this );
            Components.Add( InputMgr );
#endif

#if WINDOWS_PHONE
            TouchMgr                = new Input.TouchManager( this );
            Components.Add( TouchMgr );
#endif

            base.Initialize();
        }



#if WINDOWS || LINUX || MACOSX || XBOX
        protected override void Update( GameTime _time )
        {
#if GAMERSERVICES || XBOX
            if( ! Guide.IsVisible )
            {
#endif
                if( mbShouldDisplayStorageSelector && PlayerInCharge.HasValue )
                {
                    StorageDevice.BeginShowSelector( PlayerInCharge.Value, new AsyncCallback( StorageDeviceCallback ), null );
                    mbShouldDisplayStorageSelector= false;
                }
#if GAMERSERVICES || XBOX
            }
#endif

            base.Update( _time );
        }
#endif

#if WINDOWS_PHONE
        //----------------------------------------------------------------------
        protected virtual void OnDeactivated( object _sender, DeactivatedEventArgs _args )
        {
            if( NuclearSaveHandler != null )
            {
                NuclearSaveHandler.Save();
            }

            if( MediaPlayer.GameHasControl )
            {
                MediaPlayer.Stop();
            }

            // FIXME: We might want to handle tombstoning through PhoneApplicationService.Current.State
        }

        //----------------------------------------------------------------------
        void OnClosing( object _sender, ClosingEventArgs _args )
        {
            if( NuclearSaveHandler != null )
            {
                NuclearSaveHandler.Save();
            }

            if( MediaPlayer.GameHasControl )
            {
                MediaPlayer.Stop();
            }
        }
#endif

        //----------------------------------------------------------------------
        protected override void LoadContent()
        {
            WhitePixelTex = new Texture2D( GraphicsDevice, 1, 1, false, SurfaceFormat.Color );
            WhitePixelTex.SetData( new[] { Color.White } );
        }

#if WINDOWS || LINUX || MACOSX || XBOX
        //----------------------------------------------------------------------
        public void ShowDeviceSelector( Action _deviceSelectedCallback )
        {
            SaveGameStorageDevice = null;
            mbShouldDisplayStorageSelector = true;

            DeviceSelectedCallback = _deviceSelectedCallback;
        }

        //----------------------------------------------------------------------
        void StorageDeviceCallback( IAsyncResult _result )
        {
            SaveGameStorageDevice = StorageDevice.EndShowSelector( _result );
            
            if( SaveGameStorageDevice == null )
            {
                // The dialog was cancelled, we're not taking no as an answer!
                // Let's ask again
                mbShouldDisplayStorageSelector = true;
            }
            else
            {
                DeviceSelectedCallback();
            }
        }
        
        //----------------------------------------------------------------------
        protected void OnStorageDeviceChange( object _sender, EventArgs _args )
        {
            if( SaveGameStorageDevice != null )
            {
                mbShouldDisplayStorageSelector = ! SaveGameStorageDevice.IsConnected;
            }
        }
#endif

        //----------------------------------------------------------------------
#if WINDOWS_PHONE
        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);

            if( GameStateMgr != null && GameStateMgr.CurrentState != null )
            {
                GameStateMgr.CurrentState.OnActivated();
            }
        }
#endif


#if MACOSX
        //----------------------------------------------------------------------
        public void DoExiting()
        {
            OnExiting(this, EventArgs.Empty);
            UnloadContent();
        }
#endif

        //----------------------------------------------------------------------
        protected override void OnExiting( object _sender, EventArgs _args )
        {
#if WINDOWS || LINUX || MACOSX || XBOX
            if( NuclearSaveHandler != null )
            {
                NuclearSaveHandler.SaveGameSettings();
                NuclearSaveHandler.SaveGameData();
            }
#endif

            if( GameStateMgr != null && GameStateMgr.CurrentState != null )
            {
                GameStateMgr.CurrentState.OnExiting();
            }

            base.OnExiting( _sender, _args );
        }

        //----------------------------------------------------------------------
        public void DrawLine( Vector2 _vFrom, Vector2 _vTo, Color _color, float _fWidth = 1f )
        {
            float fAngle     = (float)Math.Atan2( _vTo.Y - _vFrom.Y, _vTo.X - _vFrom.X );
            float fLength    = Vector2.Distance( _vFrom, _vTo );
 
            Vector2 vOrigin = new Vector2( fLength / 2f, _fWidth / 2f );

            SpriteBatch.Draw( WhitePixelTex, ( _vFrom + _vTo ) / 2f, null, _color, fAngle, new Vector2( 0.5f ), new Vector2( fLength + _fWidth, _fWidth ), SpriteEffects.None, 0 );
        }

        public void DrawRect( Vector2 _vFrom, Vector2 _vTo, Color _color, float _fWidth = 1 )
        {
            if( _vFrom.X > _vTo.X )
            {
                float x = _vFrom.X;
                _vFrom.X = _vTo.X;
                _vTo.X = x;
            }

            if( _vFrom.Y > _vTo.Y )
            {
                float y = _vFrom.Y;
                _vFrom.Y = _vTo.Y;
                _vTo.Y = y;
            }

            DrawLine( _vFrom + new Vector2( _fWidth / 2f, 0f ), new Vector2( _vTo.X - _fWidth, _vFrom.Y ), _color, _fWidth );
            DrawLine( new Vector2( _vFrom.X + _fWidth / 2f, _vTo.Y ), _vTo - new Vector2( _fWidth, 0f ), _color, _fWidth );

            DrawLine( _vFrom, new Vector2( _vFrom.X, _vTo.Y ), _color, _fWidth );
            DrawLine( new Vector2( _vTo.X, _vFrom.Y ), _vTo, _color, _fWidth );
        }

        //----------------------------------------------------------------------
        public virtual string GetUIString( string _strId ) { return _strId; }

        //----------------------------------------------------------------------
        public List<Tuple<string,bool>> WrapText( SpriteFont _font, string _strText, float _fLineWidth )
        {
            List<Tuple<string,bool>> lText = new List<Tuple<string,bool>>();

            foreach( string strChunk in _strText.Split( '\n' ) )
            {
                string strLine = string.Empty;

                if( strChunk != "" )
                {
                    string[] aWords = strChunk.Split( ' ' );

                    bool bFirst = true;
                    foreach( string strWord in aWords )
                    {
                        if( bFirst ) bFirst = false;
                        else strLine += " ";

                        if( strLine != "" && _font.MeasureString(strLine + strWord).Length() > _fLineWidth )
                        {
                            lText.Add( new Tuple<string,bool>( strLine, false ) );
                            strLine = string.Empty;
                        }

                        strLine += strWord;
                    }
                }

                lText.Add( new Tuple<string,bool>( strLine + " ", true ) );
            }

            return lText;
        }

        //----------------------------------------------------------------------
        public void DrawBlurredText( float _fBlurRadius, SpriteFont _font, string _strLabel, Vector2 _vPosition )
        {
            DrawBlurredText( _fBlurRadius, _font, _strLabel, _vPosition, Color.White, Color.Black, Vector2.Zero, 1f  );
        }

        //----------------------------------------------------------------------
        public void DrawBlurredText( float _fBlurRadius, SpriteFont _font, string _strLabel, Vector2 _vPosition, Color _color )
        {
            DrawBlurredText( _fBlurRadius, _font, _strLabel, _vPosition, _color, Color.Black, Vector2.Zero, 1f );
        }

        //----------------------------------------------------------------------
        public void DrawBlurredText( float _fBlurRadius, SpriteFont _font, string _strLabel, Vector2 _vPosition, Color _color, Vector2 _vOrigin, float _fScale )
        {
            DrawBlurredText( _fBlurRadius, _font, _strLabel, _vPosition, _color, Color.Black, _vOrigin, _fScale );
        }

        //----------------------------------------------------------------------
        public void DrawBlurredText( float _fBlurRadius, SpriteFont _font, string _strLabel, Vector2 _vPosition, Color _color, Color _blurColor )
        {
            DrawBlurredText( _fBlurRadius, _font, _strLabel, _vPosition, _color, _blurColor, Vector2.Zero, 1f );
        }

        //----------------------------------------------------------------------
        public void DrawBlurredText( float _fBlurRadius, SpriteFont _font, string _strLabel, Vector2 _vPosition, Color _color, Color _blurColor, Vector2 _vOrigin, float _fScale )
        {
            if( _fBlurRadius > 0f )
            {
                //Color blurColor = _blurColor * 0.1f * (_color.A / 255f);

                SpriteBatch.DrawString( _font, _strLabel, new Vector2( _vPosition.X - _fBlurRadius, _vPosition.Y - _fBlurRadius ), _blurColor, 0f, _vOrigin, _fScale, SpriteEffects.None, 0f );
                SpriteBatch.DrawString( _font, _strLabel, new Vector2( _vPosition.X + _fBlurRadius, _vPosition.Y - _fBlurRadius ), _blurColor, 0f, _vOrigin, _fScale, SpriteEffects.None, 0f );
                SpriteBatch.DrawString( _font, _strLabel, new Vector2( _vPosition.X + _fBlurRadius, _vPosition.Y + _fBlurRadius ), _blurColor, 0f, _vOrigin, _fScale, SpriteEffects.None, 0f );
                SpriteBatch.DrawString( _font, _strLabel, new Vector2( _vPosition.X - _fBlurRadius, _vPosition.Y + _fBlurRadius ), _blurColor, 0f, _vOrigin, _fScale, SpriteEffects.None, 0f );
            }

            SpriteBatch.DrawString( _font, _strLabel, new Vector2( _vPosition.X, _vPosition.Y ), _color, 0f, _vOrigin, _fScale, SpriteEffects.None, 0f );
        }

        //----------------------------------------------------------------------
        public void DrawBlurredText( float _fBlurRadius, SpriteFont _font, StringBuilder _strbLabel, Vector2 _vPosition )
        {
            DrawBlurredText( _fBlurRadius, _font, _strbLabel, _vPosition, Color.White, Color.Black, Vector2.Zero, 1f  );
        }

        //----------------------------------------------------------------------
        public void DrawBlurredText( float _fBlurRadius, SpriteFont _font, StringBuilder _strbLabel, Vector2 _vPosition, Color _color )
        {
            DrawBlurredText( _fBlurRadius, _font, _strbLabel, _vPosition, _color, Color.Black, Vector2.Zero, 1f );
        }

        //----------------------------------------------------------------------
        public void DrawBlurredText( float _fBlurRadius, SpriteFont _font, StringBuilder _strbLabel, Vector2 _vPosition, Color _color, Vector2 _vOrigin, float _fScale )
        {
            DrawBlurredText( _fBlurRadius, _font, _strbLabel, _vPosition, _color, Color.Black, _vOrigin, _fScale );
        }

        //----------------------------------------------------------------------
        public void DrawBlurredText( float _fBlurRadius, SpriteFont _font, StringBuilder _strbLabel, Vector2 _vPosition, Color _color, Color _blurColor )
        {
            DrawBlurredText( _fBlurRadius, _font, _strbLabel, _vPosition, _color, _blurColor, Vector2.Zero, 1f );
        }


        //----------------------------------------------------------------------
        public void DrawBlurredText( float _fBlurRadius, SpriteFont _font, StringBuilder _strbLabel, Vector2 _vPosition, Color _color, Color _blurColor, Vector2 _vOrigin, float _fScale )
        {
            if( _fBlurRadius > 0f )
            {
                Color blurColor = _blurColor * 0.1f * (_color.A / 255f);

                SpriteBatch.DrawString( _font, _strbLabel, new Vector2( _vPosition.X - _fBlurRadius, _vPosition.Y - _fBlurRadius ), blurColor, 0f, _vOrigin, _fScale, SpriteEffects.None, 0f );
                SpriteBatch.DrawString( _font, _strbLabel, new Vector2( _vPosition.X + _fBlurRadius, _vPosition.Y - _fBlurRadius ), blurColor, 0f, _vOrigin, _fScale, SpriteEffects.None, 0f );
                SpriteBatch.DrawString( _font, _strbLabel, new Vector2( _vPosition.X + _fBlurRadius, _vPosition.Y + _fBlurRadius ), blurColor, 0f, _vOrigin, _fScale, SpriteEffects.None, 0f );
                SpriteBatch.DrawString( _font, _strbLabel, new Vector2( _vPosition.X - _fBlurRadius, _vPosition.Y + _fBlurRadius ), blurColor, 0f, _vOrigin, _fScale, SpriteEffects.None, 0f );
            }

            SpriteBatch.DrawString( _font, _strbLabel, new Vector2( _vPosition.X, _vPosition.Y ), _color, 0f, _vOrigin, _fScale, SpriteEffects.None, 0f );
        }

        //----------------------------------------------------------------------
        public void PlayMusic( Song _song )
        {
            if( Song != _song && MediaPlayer.GameHasControl )
            {
                MediaPlayer.IsRepeating = true;

                if( _song != null )
                {
                    MediaPlayer.Play( _song );
                }
                else
                {
                    MediaPlayer.Stop();
                }

                Song = _song;
            }
        }
    }
}
