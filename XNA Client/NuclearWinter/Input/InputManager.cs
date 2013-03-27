using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System.Linq;
using System.Collections;
using System.Diagnostics;

#if MONOMAC
using MonoMac.AppKit;
using MonoMac.OpenGL;
#endif

namespace NuclearWinter.Input
{
    [Flags]
    public enum ShortcutKey {
        LeftCtrl = 1 << 0,
        RightCtrl = 1 << 1,
  
        LeftAlt = 1 << 2,
        RightAlt = 1 << 3,
  
        LeftWindows = 1 << 4,
        RightWindows = 1 << 5,
    }

#if MONOMAC
    // KeyboardResponder has to be an NSView rather than a bare NSResponder to be able to track
    // mouse events and pass them along to MonoGame's game view
    public class KeyboardResponder : NSView {
        public delegate void EnterTextDelegate(char c);
        public delegate void JustPressedKeyDelegate(NSKey key);

        public KeyboardResponder(
            EnterTextDelegate enterText,
            JustPressedKeyDelegate justPressedKey,
            MonoMacGameView gameView)
                // Our rectangle has to match that of the containing view, if we want to be able
                // to properly get all the mouse events.
                : base (gameView.Window.ContentView.Frame)
        {
            EnterText = enterText;
            JustPressedKey = justPressedKey;
            GameView = gameView;

            // We authorize the parent's view to automatically change our width and height in case
            // of a window resize.
            AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable;
            // We need to ˝display˝ ourselves if we want to get any mouse events.
            GameView.Window.ContentView.AddSubview(this);
            // Just in case...
            GameView.Window.ContentView.AutoresizesSubviews = true;
            GameView.AutoresizesSubviews = true;

            // For all the events we don't care about, the GameView will get them. There might not
            // be that many though, as we should cover all the bases there, but one never knows.
            NextResponder = GameView;
            // We force ourselves to be the first responder to be called, and we will never resign
            // that duty. That will enable us to get all of the key events, but will also prevent
            // the GameView to get the mouse events. We'll have to forward them.
            GameView.Window.MakeFirstResponder(this);
        }

        public override void ResizeWithOldSuperviewSize(System.Drawing.SizeF oldSize)
        {
            base.ResizeWithOldSuperviewSize(oldSize);
            Frame = Superview.Bounds;
        }

        // We won't resign our duty as first responder.
        public override bool ResignFirstResponder() { return false; }
        // And we accept the duty to be first responder.
        public override bool BecomeFirstResponder() { return true; }
        // Also, we accept any mouse event.
        public override bool AcceptsFirstMouse(NSEvent theEvent) { return true; }

        // We're not going to let the NSTextView have any of these events. It can heavily mess
        // with the state of the mouse and keyboard. Moreover, as the NSTextView is on the front
        // it hides the mouse hits. So we have to forward all the events to the GameView.
        public override void MouseUp(NSEvent theEvent) { GameView.MouseUp(theEvent); }
        public override void MouseDown(NSEvent theEvent) { GameView.MouseDown(theEvent); }
        public override void MouseDragged(NSEvent theEvent) { GameView.MouseDragged(theEvent); }

        public override void RightMouseUp(NSEvent theEvent) { GameView.RightMouseUp(theEvent); }
        public override void RightMouseDown(NSEvent theEvent) { GameView.RightMouseDown(theEvent); }
        public override void RightMouseDragged(NSEvent theEvent) { GameView.RightMouseDragged(theEvent); }
        
        public override void OtherMouseUp(NSEvent theEvent) { GameView.OtherMouseUp(theEvent); }
        public override void OtherMouseDown(NSEvent theEvent) { GameView.OtherMouseDown(theEvent); }
        public override void OtherMouseDragged(NSEvent theEvent) { GameView.OtherMouseDragged(theEvent); }
        
        public override void KeyUp(NSEvent theEvent) { GameView.KeyUp(theEvent); }

        // This method override is the crux of that whole hack. We snoop on the KeyDown events,
        // in order to fill the JustPressedOSKeys array, and to feed the events to the NSTextView's
        // ˝InterpretKeyEvents˝, which will call our InsertText as a result. We still want the
        // GameView to get these events though, so to properly process the state of the keyboard
        // inside MonoGame.
        public override void KeyDown(NSEvent theEvent)
        {
            NSKey theKey = (NSKey)Enum.ToObject(typeof(NSKey), theEvent.KeyCode);
            // Under MacOS, the backspace key is called ˝Delete˝, and the delete key is called ForwardDelete.
            // Well, this unfortunately collides with the XNA's ˝Delete˝ definition of the normal delete key.
            // So, we swap the notion of ˝Delete˝ and ˝ForwardDelete˝ there. This is kind of hackish, but
            // this will prevent any potential bug where a new programmer may implement a widget that do
            // actions based on the ˝Delete˝ key and forgets the MacOS case where it's called ForwardDelete.
            switch (theKey) {
                case NSKey.Delete: theKey = NSKey.ForwardDelete; break;
                case NSKey.ForwardDelete: theKey = NSKey.Delete; break;
            }
            // Seems some keys are weirdly mapped. Let's re-assign them properly.
            switch (theEvent.KeyCode) {
                case 115: theKey = NSKey.Home; break;
                case 116: theKey = NSKey.PageUp; break;
                case 119: theKey = NSKey.End; break;
                case 121: theKey = NSKey.PageDown; break;
                case 123: theKey = NSKey.LeftArrow; break;
                case 124: theKey = NSKey.RightArrow; break;
                case 125: theKey = NSKey.DownArrow; break;
                case 126: theKey = NSKey.UpArrow; break;
            }
            // Signalling NuclearWinter of the ˝OS˝ key that has been pushed.
            JustPressedKey(theKey);
            // Passing down the event to the OS, to get a nice interpretation.
            InterpretKeyEvents(new NSEvent[] { theEvent} );
            GameView.KeyDown(theEvent);
        }

        // This may get called as a result of InterpretKeyEvents, by giving us a string the
        // OS interpreted from the events. For example, Shift+o will get the string ˝O˝.
        [MonoMac.Foundation.Export("insertText:")]
        public void InsertText(MonoMac.Foundation.NSObject insertString)
        {
            string str = insertString.ToString();
            foreach (char c in str)
                EnterText(c);
        }

        // This may get called as a result of InterpretKeyEvents. Exporting this method
        // is required, otherwise we'd get a system beep instead
        [MonoMac.Foundation.Export("doCommandBySelector:")]
        public void DoCommandBySelector( MonoMac.ObjCRuntime.Selector aSelector)
        {
            // Ignore command
        }

        // We need to de-reference the GameView so that the pool can collect it properly.
        protected override void Dispose(bool dispose)
        {
            if (dispose)
                GameView = null;
        }

        private EnterTextDelegate EnterText;
        private JustPressedKeyDelegate JustPressedKey;
        private MonoMacGameView GameView;
    }
#endif

    public class InputManager: GameComponent
    {
        public const int                            siMaxInput              = 4;
        public const float                          sfStickThreshold        = 0.4f;

        public readonly GamePadState[]              GamePadStates;
        public readonly GamePadState[]              PreviousGamePadStates;

#if WINDOWS || LINUX || MACOSX
        public MouseState                           MouseState              { get; private set; }
        public MouseState                           PreviousMouseState      { get; private set; }

        public int PrimaryMouseButton
        {
#if ! MONOMAC
            get { return System.Windows.Forms.SystemInformation.MouseButtonsSwapped ? 2 : 0; }
#else
            get { return 0; }
#endif
        }

        public int SecondaryMouseButton
        {
#if ! MONOMAC
            get { return System.Windows.Forms.SystemInformation.MouseButtonsSwapped ? 0 : 2; }
#else
            get { return 2; }
#endif
        }

        public LocalizedKeyboardState               KeyboardState           { get; private set; }
        public LocalizedKeyboardState               PreviousKeyboardState   { get; private set; }
        public PlayerIndex?                         KeyboardPlayerIndex     { get; private set; }
        /*Keys                                        mLastKeyPressed;
        bool                                        mbRepeatKey;
        float                                       mfRepeatKeyTimer;*/
        public ShortcutKey                          ActiveShortcutKey;

        public List<char>                           EnteredText             { get; private set; }
        public List<Keys>                           JustPressedKeys         { get; private set; }

#if MONOMAC
        public KeyboardResponder                    OurResponder;
#endif

#if !MONOGAME
        public List<System.Windows.Forms.Keys>      JustPressedOSKeys       { get; private set; }
        WindowMessageFilter                         mMessageFilter;
#else
#if !MONOMAC
        public List<OpenTK.Input.Key>               JustPressedOSKeys       { get; private set; }
#else
        public List<NSKey>                          JustPressedOSKeys       { get; private set; }
#endif
        float                                       mfTimeSinceLastClick;
        Point                                       mLastPrimaryClickPosition;
#endif
        bool                                        mbDoubleClicked;
#endif

        Buttons[]                                   maLastPressedButtons;
        float[]                                     mafRepeatTimers;
        bool[]                                      mabRepeatButtons;
        const float                                 sfButtonRepeatDelay     = 0.3f;
        const float                                 sfButtonRepeatInterval  = 0.1f;

        List<Buttons>                               lButtons;

        //---------------------------------------------------------------------
        public InputManager( Game _game )
        : base ( _game )
        {
            GamePadStates           = new GamePadState[ siMaxInput ];
            PreviousGamePadStates   = new GamePadState[ siMaxInput ];

            maLastPressedButtons    = new Buttons[ siMaxInput ];
            mafRepeatTimers         = new float[ siMaxInput ];
            mabRepeatButtons        = new bool[ siMaxInput ];

            lButtons                = Utils.GetValues<Buttons>();
            
#if !MACOSX
            ActiveShortcutKey       = ShortcutKey.LeftCtrl | ShortcutKey.RightCtrl;
#else
            ActiveShortcutKey       = ShortcutKey.LeftWindows | ShortcutKey.RightWindows;
#endif

#if WINDOWS || LINUX || MACOSX
            EnteredText             = new List<char>();
            JustPressedKeys         = new List<Keys>();

#if !MONOGAME
            JustPressedOSKeys  = new List<System.Windows.Forms.Keys>();

            mMessageFilter              = new WindowMessageFilter( Game.Window.Handle );
            mMessageFilter.CharacterHandler = delegate( char _char ) { EnteredText.Add( _char ); };
            mMessageFilter.KeyDownHandler   = delegate( System.Windows.Forms.Keys _key ) { JustPressedOSKeys.Add( _key ); };
            mMessageFilter.DoubleClickHandler   = delegate { mbDoubleClicked = true; };
#else

#if !MONOMAC
            JustPressedOSKeys  = new List<OpenTK.Input.Key>();
            System.Reflection.FieldInfo info = typeof(OpenTKGameWindow).GetField( "window", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField );
            OpenTK.GameWindow window = (OpenTK.GameWindow)info.GetValue( Game.Window );
            window.Keyboard.KeyRepeat = true;
            window.KeyPress += delegate( object _sender, OpenTK.KeyPressEventArgs _e ) { EnteredText.Add( _e.KeyChar ); };
            window.Keyboard.KeyDown  += delegate( object _sender, OpenTK.Input.KeyboardKeyEventArgs  _e ) { JustPressedOSKeys.Add( _e.Key ); };
#else
            JustPressedOSKeys  = new List<NSKey>();
            OurResponder = new KeyboardResponder(
                delegate(char c) { EnteredText.Add(c); },
                delegate(NSKey k) { JustPressedOSKeys.Add(k); },
                Game.Window);
#endif

            MouseState = Mouse.GetState();
            PreviousMouseState = MouseState;
#endif
#endif
        }

        //---------------------------------------------------------------------
        public override void Update( GameTime _time )
        {
            float fElapsedTime = (float)_time.ElapsedGameTime.TotalSeconds;

            for( int i = 0; i < siMaxInput; i++ )
            {
                PreviousGamePadStates[ i ] = GamePadStates[ i ];
                GamePadStates[ i ] = GamePad.GetState( (PlayerIndex)i );
            }

#if WINDOWS || LINUX || MACOSX
            if( ! IsMouseCaptured )
            {
                PreviousMouseState = MouseState;
            }
            else
            {
                PreviousMouseState = new MouseState(
                    PreviousMouseState.X, PreviousMouseState.Y, // Keep old mouse position
                    MouseState.ScrollWheelValue,
                    MouseState.LeftButton,
                    MouseState.MiddleButton,
                    MouseState.RightButton,
                    MouseState.XButton1,
                    MouseState.XButton2 );
            }

            MouseState = Mouse.GetState();

            if( IsMouseCaptured )
            {
                Mouse.SetPosition( PreviousMouseState.X, PreviousMouseState.Y );
            }


            PreviousKeyboardState = KeyboardState;
            KeyboardState = new LocalizedKeyboardState( Keyboard.GetState() );

            KeyboardPlayerIndex = null;
            for( int i = 0; i < siMaxInput; i++ )
            {
                if( ! GamePadStates[i].IsConnected )
                {
                    KeyboardPlayerIndex = (PlayerIndex)i;
                    break;
                }
            }

            EnteredText.Clear();
            JustPressedOSKeys.Clear();

            Keys[] currentPressedKeys = KeyboardState.Native.GetPressedKeys();
            Keys[] previousPressedKeys = PreviousKeyboardState.Native.GetPressedKeys();
            
            JustPressedKeys = currentPressedKeys.Except( previousPressedKeys ).ToList();

            mbDoubleClicked = false;

#if MONOGAME
            if( WasMouseButtonJustPressed( PrimaryMouseButton ) )
            {
#if !MONOMAC
                float fDoubleClickTime = System.Windows.Forms.SystemInformation.DoubleClickTime / 1000f;
                int iDoubleClickWidth = System.Windows.Forms.SystemInformation.DoubleClickSize.Width;
                int iDoubleClickHeight = System.Windows.Forms.SystemInformation.DoubleClickSize.Height;
#else
                float fDoubleClickTime = 0.5f;
                int iDoubleClickWidth = 4;
                int iDoubleClickHeight = 4;
#endif

                if( mfTimeSinceLastClick <= fDoubleClickTime )
                {
                    if( Math.Abs( MouseState.X - mLastPrimaryClickPosition.X ) <= iDoubleClickWidth
                       && Math.Abs( MouseState.Y - mLastPrimaryClickPosition.Y ) <= iDoubleClickHeight )
                    {
                        mbDoubleClicked = true;
                    }
                }

                mLastPrimaryClickPosition = new Point( MouseState.X, MouseState.Y );
                mfTimeSinceLastClick = 0f;
            }
            else
            {
                mfTimeSinceLastClick += fElapsedTime;
            }
#endif
#endif

            for( int iGamePad = 0; iGamePad < siMaxInput; iGamePad++ )
            {
                mabRepeatButtons[ iGamePad ] = false;

                foreach( Buttons button in lButtons )
                {
                    bool bButtonPressed;

                    switch( button )
                    {
                        // Override for left stick
                        case Buttons.LeftThumbstickLeft:
                            bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Left.X < -sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Left.X > -sfStickThreshold;
                            break;
                        case Buttons.LeftThumbstickRight:
                            bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Left.X > sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Left.X < sfStickThreshold;
                            break;
                        case Buttons.LeftThumbstickDown:
                            bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Left.Y < -sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Left.Y > -sfStickThreshold;
                            break;
                        case Buttons.LeftThumbstickUp:
                            bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Left.Y > sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Left.Y < sfStickThreshold;
                            break;

                        // Override for right stick
                        case Buttons.RightThumbstickLeft:
                            bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Right.X < -sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Right.X > -sfStickThreshold;
                            break;
                        case Buttons.RightThumbstickRight:
                            bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Right.X > sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Right.X < sfStickThreshold;
                            break;
                        case Buttons.RightThumbstickDown:
                            bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Right.Y < -sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Right.Y > -sfStickThreshold;
                            break;
                        case Buttons.RightThumbstickUp:
                            bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Right.Y > sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Right.Y < sfStickThreshold;
                            break;
                        
                        // Default button behavior for the rest
                        default:
                            bButtonPressed = GamePadStates[ iGamePad ].IsButtonDown( button ) && PreviousGamePadStates[ iGamePad ].IsButtonUp( button );
                            break;
                    }

#if WINDOWS || LINUX || MACOSX
                    if( ! bButtonPressed )
                    {
                        Keys key = GetKeyboardMapping( button );
                        bButtonPressed = key != Keys.None && KeyboardState.IsKeyDown( key ) && ! PreviousKeyboardState.IsKeyDown( key );
                    }
#endif

                    if( bButtonPressed )
                    {
                        mafRepeatTimers[ iGamePad ] = 0f;
                        maLastPressedButtons[ iGamePad ] = button;
                        break;
                    }
                }

                if( maLastPressedButtons[iGamePad] != 0 )
                {
                    bool bIsButtonStillDown = GamePadStates[ iGamePad ].IsButtonDown( maLastPressedButtons[ iGamePad ] );

#if WINDOWS || LINUX || MACOSX
                    if( ! bIsButtonStillDown )
                    {
                        Keys key = GetKeyboardMapping( maLastPressedButtons[iGamePad] );
                        bIsButtonStillDown = key != Keys.None && KeyboardState.IsKeyDown( key );
                    }
#endif

                    if( bIsButtonStillDown )
                    {
                        float fRepeatValue      = ( mafRepeatTimers[ iGamePad ] - sfButtonRepeatDelay ) % ( sfButtonRepeatInterval );
                        float fNewRepeatValue   = ( mafRepeatTimers[ iGamePad ] + fElapsedTime - sfButtonRepeatDelay ) % ( sfButtonRepeatInterval );

                        if( mafRepeatTimers[ iGamePad ] < sfButtonRepeatDelay && mafRepeatTimers[ iGamePad ] + fElapsedTime >= sfButtonRepeatDelay )
                        {
                            mabRepeatButtons[ iGamePad ] = true;
                        }
                        else
                        if( mafRepeatTimers[ iGamePad ] > sfButtonRepeatDelay && fRepeatValue > fNewRepeatValue )
                        {
                            mabRepeatButtons[ iGamePad ] = true;
                        }

                        mafRepeatTimers[ iGamePad ] += fElapsedTime;
                    }
                    else
                    {
                        mafRepeatTimers[ iGamePad ] = 0f;
                        maLastPressedButtons[ iGamePad ] = 0;
                    }
                }
            }


        }
        
#if WINDOWS || LINUX || MACOSX
        //----------------------------------------------------------------------
        public bool IsMouseButtonDown( int _iButton )
        {
            switch( _iButton )
            {
                case 0:
                    return MouseState.LeftButton    == ButtonState.Pressed;
                case 1:
                    return MouseState.MiddleButton  == ButtonState.Pressed;
                case 2:
                    return MouseState.RightButton   == ButtonState.Pressed;
                case 3:
                    return MouseState.XButton1      == ButtonState.Pressed;
                case 4:
                    return MouseState.XButton2      == ButtonState.Pressed;
                case 5:
                    return ( MouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue ) > 0;
                case 6:
                    return ( MouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue ) < 0;
                default:
                    return false;
            }
        }

        //----------------------------------------------------------------------
        public bool WasMouseButtonJustPressed( int _iButton )
        {
            switch( _iButton )
            {
                case 0:
                    return MouseState.LeftButton    == ButtonState.Pressed && PreviousMouseState.LeftButton     == ButtonState.Released;
                case 1:
                    return MouseState.MiddleButton  == ButtonState.Pressed && PreviousMouseState.MiddleButton   == ButtonState.Released;
                case 2:
                    return MouseState.RightButton   == ButtonState.Pressed && PreviousMouseState.RightButton    == ButtonState.Released;
                case 3:
                    return MouseState.XButton1      == ButtonState.Pressed && PreviousMouseState.XButton1       == ButtonState.Released;
                case 4:
                    return MouseState.XButton2      == ButtonState.Pressed && PreviousMouseState.XButton2       == ButtonState.Released;
                case 5:
                    return ( MouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue ) > 0;
                case 6:
                    return ( MouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue ) < 0;
                default:
                    return false;
            }
        }

        //----------------------------------------------------------------------
        public bool WasMouseButtonJustReleased( int _iButton )
        {
            switch( _iButton )
            {
                case 0:
                    return MouseState.LeftButton    == ButtonState.Released && PreviousMouseState.LeftButton     == ButtonState.Pressed;
                case 1:
                    return MouseState.MiddleButton  == ButtonState.Released && PreviousMouseState.MiddleButton   == ButtonState.Pressed;
                case 2:
                    return MouseState.RightButton   == ButtonState.Released && PreviousMouseState.RightButton    == ButtonState.Pressed;
                case 3:
                    return MouseState.XButton1      == ButtonState.Released && PreviousMouseState.XButton1       == ButtonState.Pressed;
                case 4:
                    return MouseState.XButton2      == ButtonState.Released && PreviousMouseState.XButton2       == ButtonState.Pressed;
                default:
                    return false;
            }
        }

        //----------------------------------------------------------------------
        public bool WasMouseJustDoubleClicked()
        {
            return mbDoubleClicked;
        }

        //----------------------------------------------------------------------
        public bool IsShortcutKeyDown()
        {
            return ( ( ActiveShortcutKey & ShortcutKey.LeftCtrl ) == ShortcutKey.LeftCtrl && KeyboardState.IsKeyDown( Keys.LeftControl ) )
                || ( ( ActiveShortcutKey & ShortcutKey.RightCtrl ) == ShortcutKey.RightCtrl && KeyboardState.IsKeyDown( Keys.RightControl ) )
                || ( ( ActiveShortcutKey & ShortcutKey.LeftAlt ) == ShortcutKey.LeftAlt && KeyboardState.IsKeyDown( Keys.LeftAlt ) )
                || ( ( ActiveShortcutKey & ShortcutKey.RightAlt ) == ShortcutKey.RightAlt && KeyboardState.IsKeyDown( Keys.RightAlt ) )
                || ( ( ActiveShortcutKey & ShortcutKey.LeftWindows ) == ShortcutKey.LeftWindows && KeyboardState.IsKeyDown( Keys.LeftWindows ) )
                || ( ( ActiveShortcutKey & ShortcutKey.RightWindows ) == ShortcutKey.RightWindows && KeyboardState.IsKeyDown( Keys.RightWindows ) );
        }

        //----------------------------------------------------------------------
        public bool WasKeyJustPressed( Keys _key, bool _bNative=false )
        {
            if( ! _bNative )
            {
                return KeyboardState.IsKeyDown(_key) && ! PreviousKeyboardState.IsKeyDown(_key);
            }
            else
            {
                return KeyboardState.Native.IsKeyDown(_key) && ! PreviousKeyboardState.Native.IsKeyDown(_key);
            }
        }

        //----------------------------------------------------------------------
        public bool WasKeyJustReleased( Keys _key )
        {
            return KeyboardState.IsKeyUp(_key) && ! PreviousKeyboardState.IsKeyUp(_key);
        }


        //----------------------------------------------------------------------
        public int GetMouseWheelDelta()
        {
            return MouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue;
        }
#endif

        //----------------------------------------------------------------------
        public bool WasButtonJustPressed( Buttons _button, PlayerIndex _controllingPlayer )
        {
            return WasButtonJustPressed( _button, _controllingPlayer, false );
        }

        //----------------------------------------------------------------------
        public bool WasButtonJustPressed( Buttons _button, PlayerIndex _controllingPlayer, bool _bRepeat )
        {
            PlayerIndex discardedPlayerIndex;
            return WasButtonJustPressed( _button, _controllingPlayer, out discardedPlayerIndex, _bRepeat );
        }

        //----------------------------------------------------------------------
        public bool WasButtonJustPressed( Buttons _button, PlayerIndex? _controllingPlayer, out PlayerIndex _playerIndex )
        {
            return WasButtonJustPressed( _button, _controllingPlayer, out _playerIndex, false );
        }

        //----------------------------------------------------------------------
        public bool WasButtonJustPressed( Buttons _button, PlayerIndex? _controllingPlayer, out PlayerIndex _playerIndex, bool _bRepeat )
        {
            if( _controllingPlayer.HasValue )
            {
                _playerIndex = _controllingPlayer.Value;
                int iGamePad = (int)_playerIndex;

                //--------------------------------------------------------------
                bool bButtonPressed;

                switch( _button )
                {
                    // Override for left stick
                    case Buttons.LeftThumbstickLeft:
                        bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Left.X < -sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Left.X > -sfStickThreshold;
                        break;
                    case Buttons.LeftThumbstickRight:
                        bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Left.X > sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Left.X < sfStickThreshold;
                        break;
                    case Buttons.LeftThumbstickDown:
                        bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Left.Y < -sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Left.Y > -sfStickThreshold;
                        break;
                    case Buttons.LeftThumbstickUp:
                        bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Left.Y > sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Left.Y < sfStickThreshold;
                        break;

                    // Override for right stick
                    case Buttons.RightThumbstickLeft:
                        bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Right.X < -sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Right.X > -sfStickThreshold;
                        break;
                    case Buttons.RightThumbstickRight:
                        bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Right.X > sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Right.X < sfStickThreshold;
                        break;
                    case Buttons.RightThumbstickDown:
                        bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Right.Y < -sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Right.Y > -sfStickThreshold;
                        break;
                    case Buttons.RightThumbstickUp:
                        bButtonPressed = GamePadStates[ iGamePad ].ThumbSticks.Right.Y > sfStickThreshold && PreviousGamePadStates[ iGamePad ].ThumbSticks.Right.Y < sfStickThreshold;
                        break;
                    
                    // Default button behavior for the rest
                    default:
                        bButtonPressed = GamePadStates[ iGamePad ].IsButtonDown( _button ) && PreviousGamePadStates[ iGamePad ].IsButtonUp( _button );
                        break;
                }

                if( ! bButtonPressed && _bRepeat )
                {
                    bButtonPressed = ( mabRepeatButtons[ (int)_playerIndex ] && _button == maLastPressedButtons[ (int)_playerIndex ] );
                }
                
#if WINDOWS || LINUX || MACOSX
                //--------------------------------------------------------------
                // Keyboard controls
                Keys key = GetKeyboardMapping( _button );

                if( _playerIndex == KeyboardPlayerIndex && key != Keys.None && KeyboardState.IsKeyDown( key ) && ! PreviousKeyboardState.IsKeyDown( key ) )
                {
                    bButtonPressed = true;
                }
#endif

                return bButtonPressed;
            }
            else
            {
                return  WasButtonJustPressed( _button, PlayerIndex.One, out _playerIndex, _bRepeat )
                    ||  WasButtonJustPressed( _button, PlayerIndex.Two, out _playerIndex, _bRepeat )
                    ||  WasButtonJustPressed( _button, PlayerIndex.Three, out _playerIndex, _bRepeat )
                    ||  WasButtonJustPressed( _button, PlayerIndex.Four, out _playerIndex, _bRepeat );
            }
        }

        //----------------------------------------------------------------------
        public bool WasButtonJustReleased( Buttons _button, PlayerIndex _controllingPlayer )
        {
            PlayerIndex discardedPlayerIndex;
            return WasButtonJustReleased( _button, _controllingPlayer, out discardedPlayerIndex );
        }

        //----------------------------------------------------------------------
        public bool WasButtonJustReleased( Buttons _button, PlayerIndex? _controllingPlayer, out PlayerIndex _playerIndex )
        {
            if( _controllingPlayer.HasValue )
            {
                _playerIndex = _controllingPlayer.Value;
                int iGamePad = (int)_playerIndex;

                //--------------------------------------------------------------
                bool bButtonPressed;

                switch( _button )
                {
                    // Override for left stick
                    case Buttons.LeftThumbstickLeft:
                        bButtonPressed = PreviousGamePadStates[ iGamePad ].ThumbSticks.Left.X < -sfStickThreshold && GamePadStates[ iGamePad ].ThumbSticks.Left.X > -sfStickThreshold;
                        break;
                    case Buttons.LeftThumbstickRight:
                        bButtonPressed = PreviousGamePadStates[ iGamePad ].ThumbSticks.Left.X > sfStickThreshold && GamePadStates[ iGamePad ].ThumbSticks.Left.X < sfStickThreshold;
                        break;
                    case Buttons.LeftThumbstickDown:
                        bButtonPressed = PreviousGamePadStates[ iGamePad ].ThumbSticks.Left.Y < -sfStickThreshold && GamePadStates[ iGamePad ].ThumbSticks.Left.Y > -sfStickThreshold;
                        break;
                    case Buttons.LeftThumbstickUp:
                        bButtonPressed = PreviousGamePadStates[ iGamePad ].ThumbSticks.Left.Y > sfStickThreshold && GamePadStates[ iGamePad ].ThumbSticks.Left.Y < sfStickThreshold;
                        break;

                    // Override for right stick
                    case Buttons.RightThumbstickLeft:
                        bButtonPressed = PreviousGamePadStates[ iGamePad ].ThumbSticks.Right.X < -sfStickThreshold && GamePadStates[ iGamePad ].ThumbSticks.Right.X > -sfStickThreshold;
                        break;
                    case Buttons.RightThumbstickRight:
                        bButtonPressed = PreviousGamePadStates[ iGamePad ].ThumbSticks.Right.X > sfStickThreshold && GamePadStates[ iGamePad ].ThumbSticks.Right.X < sfStickThreshold;
                        break;
                    case Buttons.RightThumbstickDown:
                        bButtonPressed = PreviousGamePadStates[ iGamePad ].ThumbSticks.Right.Y < -sfStickThreshold && GamePadStates[ iGamePad ].ThumbSticks.Right.Y > -sfStickThreshold;
                        break;
                    case Buttons.RightThumbstickUp:
                        bButtonPressed = PreviousGamePadStates[ iGamePad ].ThumbSticks.Right.Y > sfStickThreshold && GamePadStates[ iGamePad ].ThumbSticks.Right.Y < sfStickThreshold;
                        break;
                    
                    // Default button behavior for the rest
                    default:
                        bButtonPressed = PreviousGamePadStates[ iGamePad ].IsButtonDown( _button ) && GamePadStates[ iGamePad ].IsButtonUp( _button );
                        break;
                }
                
#if WINDOWS || LINUX || MACOSX
                //--------------------------------------------------------------
                // Keyboard controls
                Keys key = GetKeyboardMapping( _button );

                if( _playerIndex == KeyboardPlayerIndex && key != Keys.None && PreviousKeyboardState.IsKeyDown( key ) && ! KeyboardState.IsKeyDown( key ) )
                {
                    bButtonPressed = true;
                }
#endif

                return bButtonPressed;
            }
            else
            {
                return  WasButtonJustReleased( _button, PlayerIndex.One, out _playerIndex )
                    ||  WasButtonJustReleased( _button, PlayerIndex.Two, out _playerIndex )
                    ||  WasButtonJustReleased( _button, PlayerIndex.Three, out _playerIndex )
                    ||  WasButtonJustReleased( _button, PlayerIndex.Four, out _playerIndex );
            }
        }

        //----------------------------------------------------------------------
        public Keys GetKeyboardMapping( Buttons button )
        {
            switch( button )
            {
                case Buttons.A:
                    return Keys.Space;
                // Keys.Enter is not a good match for the Start button
                /*case Buttons.Start:
                    return Keys.Enter;*/
                case Buttons.Back:
                    return Keys.Escape;
                case Buttons.LeftThumbstickLeft:
                    return Keys.Left;
                case Buttons.LeftThumbstickRight:
                    return Keys.Right;
                case Buttons.LeftThumbstickUp:
                    return Keys.Up;
                case Buttons.LeftThumbstickDown:
                    return Keys.Down;
            }

            return Keys.None;
        }

        //----------------------------------------------------------------------
        Point                   mSavedMousePosition;
        public bool             IsMouseCaptured     { get; private set; }

        //----------------------------------------------------------------------
        public void CaptureMouse()
        {
            mSavedMousePosition = new Point( Mouse.GetState().X, Mouse.GetState().Y );
            Game.IsMouseVisible = false;
            Point mouseCenter = new Point( Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            Mouse.SetPosition( mouseCenter.X, mouseCenter.Y );

            MouseState = Mouse.GetState();
            PreviousMouseState = MouseState;

            IsMouseCaptured = true;
        }

        //----------------------------------------------------------------------
        public void ReleaseMouse()
        {
            Debug.Assert( IsMouseCaptured );

            IsMouseCaptured = false;

            Game.IsMouseVisible = true;
            Mouse.SetPosition( mSavedMousePosition.X, mSavedMousePosition.Y );

            MouseState = Mouse.GetState();
            PreviousMouseState = MouseState;
        }
    }
}
