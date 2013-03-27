using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NuclearWinter.Input
{
    //-------------------------------------------------------------------------
    // Inspired by Nuclex.Input and
    // http://stackoverflow.com/questions/375316/xna-keyboard-text-input
    internal class WindowMessageFilter: IMessageFilter, IDisposable
    {
        //---------------------------------------------------------------------
        public Action<Keys>     KeyUpHandler;
        public Action<Keys>     KeyDownHandler;
        public Action<char>     CharacterHandler;
        public Action           DoubleClickHandler;

        //---------------------------------------------------------------------
        bool                    mbIsDisposed;

        const int               WM_CHAR             = 0x0102;
        const int               WM_KEYDOWN          = 0x0100;
        const int               WM_KEYUP            = 0x0101;
        const int               WM_LBUTTONDBLCLK    = 0x0203;

        //---------------------------------------------------------------------
        public WindowMessageFilter( IntPtr _hWnd )
        {
            Application.AddMessageFilter( this );
        }

        //---------------------------------------------------------------------
        ~WindowMessageFilter()
        {
            Dispose();
        }

        //---------------------------------------------------------------------
        public void Dispose()
        {
            if( ! mbIsDisposed )
            {
                Application.RemoveMessageFilter( this );
                mbIsDisposed = true;
            }
            }

        //---------------------------------------------------------------------
        bool IMessageFilter.PreFilterMessage( ref Message _message )
        {
            switch( _message.Msg )
            {
                case WM_KEYDOWN: {
                    int virtualKeyCode = _message.WParam.ToInt32();
                    if( KeyDownHandler != null )
                    {
                        KeyDownHandler( (Keys)virtualKeyCode );
                    }

                    TranslateMessage( ref _message );

                    return true;
                }
                case WM_KEYUP: {
                    int virtualKeyCode = _message.WParam.ToInt32();
                    if( KeyUpHandler != null )
                    {
                        KeyUpHandler( (Keys)virtualKeyCode );
                    }

                    return true;
                }
                case WM_CHAR: {
                    char character = (char)_message.WParam.ToInt32();
                    if( CharacterHandler != null )
                    {
                        CharacterHandler( character );
                    }

                    return true;
                }
                case WM_LBUTTONDBLCLK: {
                    if( DoubleClickHandler != null )
                    {
                        DoubleClickHandler();
                    }
                    return true;
                }
            }

            return false;
        }

        //---------------------------------------------------------------------
        [DllImport("user32", SetLastError = true)]
        public extern static bool TranslateMessage(ref Message message);
    }
}
