using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace NuclearWinter
{
    /// <summary>
    /// Describes a screen mode
    /// </summary>
    public struct ScreenMode: IComparable<ScreenMode>
    {
        //----------------------------------------------------------------------
        public ScreenMode( int _iWidth, int _iHeight )
        {
            Width           = _iWidth;
            Height          = _iHeight;
        }

        //----------------------------------------------------------------------
        public override string ToString()
        {
            return Width.ToString() + "x" + Height.ToString();
        }

        //----------------------------------------------------------------------
        public int CompareTo( ScreenMode _other )
        {
            int iOrder = Width.CompareTo( _other.Width );

            if( iOrder == 0 )
            {
                return Height.CompareTo( _other.Height );
            }
            else
            {
                return iOrder;
            }
        }

        //----------------------------------------------------------------------
        public int      Width;
        public int      Height;

        public Vector2  Size
        {
            get {
                return new Vector2( Width, Height );
            }
        }

        public Rectangle Rectangle
        {
            get {
                return new Rectangle( 0, 0, Width, Height );
            }
        }
    }

    //--------------------------------------------------------------------------
    /// <summary>
    /// Static class to manage game resolution and scaling
    /// </summary>
    public sealed class Resolution
    {
        static Resolution()
        {
            ScaleFactor = 1f;
            InternalMode = new ScreenMode( 1920, 1080 );

            SortedScreenModes = new List<ScreenMode>();

            foreach( DisplayMode displayMode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes )
            {
                if( displayMode.AspectRatio > 1f && displayMode.Width >= 800f && displayMode.Width <= InternalMode.Width && displayMode.Height <= InternalMode.Height )
                {
                    ScreenMode screenMode = new ScreenMode( displayMode.Width, displayMode.Height );
                    if( ! SortedScreenModes.Contains( screenMode ) )
                    {
                        SortedScreenModes.Add( screenMode );
                    }
                }
            }

            SortedScreenModes.Sort();
        }

        //----------------------------------------------------------------------
        // Initialize the best Resolution available
        public static ScreenMode Initialize( GraphicsDeviceManager _graphics )
        {
            List<ScreenMode> lReversedScreenModes = new List<ScreenMode>( SortedScreenModes );
            lReversedScreenModes.Reverse();

            int i = 0;
            foreach( ScreenMode mode in lReversedScreenModes )
            {
                if( SetScreenMode( _graphics, mode, true ) )
                {
                    return mode;
                }
                i++;
            }

            // Cannot happen
            Debug.Assert( false );

            return new ScreenMode( 0, 0 );
        }

        //----------------------------------------------------------------------
        // Initialize the Resolution using the specified ScreenMode
        public static bool Initialize( GraphicsDeviceManager _graphics, ScreenMode _mode, bool _bFullscreen )
        {
            InternalMode = new ScreenMode( 1920, 1080 );
            return SetScreenMode( _graphics, _mode, _bFullscreen );
        }

        //----------------------------------------------------------------------
        // Set the specified screen mode
        public static bool SetScreenMode( GraphicsDeviceManager _graphics, ScreenMode _mode, bool _bFullscreen )
        {
            if( _bFullscreen )
            {
                // Fullscreen
                foreach( DisplayMode displayMode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes )
                {
                    if( (_mode.Width == displayMode.Width)
                    &&  (_mode.Height == displayMode.Height) )
                    {
                        try
                        {
                            ApplyScreenMode( _graphics, _mode, true );
                        }
                        catch
                        {
                            return false;
                        }
                        return true;
                    }
                }
            }
            else
            {
                if( (_mode.Width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                &&  (_mode.Height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) )
                {
                    ApplyScreenMode( _graphics, _mode, false );
                    return true;
                }
            }

            return false;
        }

        //----------------------------------------------------------------------
        // Apply the specified ScreenMode
        private static void ApplyScreenMode( GraphicsDeviceManager _graphics, ScreenMode _mode, bool _bFullscreen )
        {
            Mode = _mode;
            _graphics.PreferredBackBufferWidth  = Mode.Width;
            _graphics.PreferredBackBufferHeight = Mode.Height;
            _graphics.PreferMultiSampling       = true;
            _graphics.IsFullScreen              = _bFullscreen;
            _graphics.ApplyChanges();

            ScaleFactor = (float)( Mode.Width * 9 / 16 ) / (float)InternalMode.Height;
            Scale = Matrix.CreateScale( ScaleFactor );

            DefaultViewport = _graphics.GraphicsDevice.Viewport;

            mViewport        = new Viewport();
            mViewport.X      = 0;
            mViewport.Y      = (Mode.Height - (Mode.Width * 9 / 16 )) / 2;
            mViewport.Width  = Mode.Width;
            mViewport.Height = Mode.Width * 9 / 16;
            _graphics.GraphicsDevice.Viewport = mViewport;
        }
        
        //----------------------------------------------------------------------
        public static List<ScreenMode> SortedScreenModes;

        public static ScreenMode    Mode                { get; private set; }                   // Actual mode
        public static ScreenMode    InternalMode        { get; private set; }                   // Internal mode

        public static Viewport      DefaultViewport     { get; private set; }                   // The full viewport

        private static Viewport     mViewport;                                                  // The 16/9 viewport (with black borders)
        public static Viewport      Viewport            { get { return mViewport; } }

        public static Matrix        Scale               { get; private set; }
        public static float         ScaleFactor         { get; private set; }
    }
}
