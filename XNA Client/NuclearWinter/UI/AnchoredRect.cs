using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace NuclearWinter.UI
{
    public struct AnchoredRect
    {
        public int?     Left;
        public int?     Top;

        public int?     Right;
        public int?     Bottom;

        public int      Width;
        public int      Height;

        public static AnchoredRect Full
        {
            get {
                return new AnchoredRect( 0, 0, 0, 0, 0, 0 );
            }
        }

        //----------------------------------------------------------------------
        public AnchoredRect( int? _iLeft, int? _iTop, int? _iRight, int? _iBottom, int _iWidth, int _iHeight )
        {
            Left    = _iLeft;
            Top     = _iTop;

            Right   = _iRight;
            Bottom  = _iBottom;

            Width   = _iWidth;
            Height  = _iHeight;
        }

        //----------------------------------------------------------------------
        public static AnchoredRect CreateFixed( int _iLeft, int _iTop, int _iWidth, int _iHeight )
        {
            return new AnchoredRect( _iLeft, _iTop, null, null, _iWidth, _iHeight );
        }

        public static AnchoredRect CreateFixed( Rectangle _rect )
        {
            return new AnchoredRect( _rect.Left, _rect.Top, null, null, _rect.Width, _rect.Height );
        }

        public static AnchoredRect CreateFull( int _iValue )
        {
            return new AnchoredRect( _iValue, _iValue, _iValue, _iValue, 0, 0 );
        }

        public static AnchoredRect CreateFull( int _iLeft, int _iTop, int _iRight, int _iBottom )
        {
            return new AnchoredRect( _iLeft, _iTop, _iRight, _iBottom, 0, 0 );
        }

        public static AnchoredRect CreateLeftAnchored( int _iLeft, int _iTop, int _iBottom, int _iWidth )
        {
            return new AnchoredRect( _iLeft, _iTop, null, _iBottom, _iWidth, 0 );
        }

        public static AnchoredRect CreateRightAnchored( int _iRight, int _iTop, int _iBottom, int _iWidth )
        {
            return new AnchoredRect( null, _iTop, _iRight, _iBottom, _iWidth, 0 );
        }

        public static AnchoredRect CreateTopAnchored( int _iLeft, int _iTop, int _iRight, int _iHeight )
        {
            return new AnchoredRect( _iLeft, _iTop, _iRight, null, 0, _iHeight );
        }

        public static AnchoredRect CreateBottomAnchored( int _iLeft, int _iBottom, int _iRight, int _iHeight )
        {
            return new AnchoredRect( _iLeft, null, _iRight, _iBottom, 0, _iHeight );
        }

        public static AnchoredRect CreateBottomLeftAnchored( int _iLeft, int _iBottom, int _iWidth, int _iHeight )
        {
            return new AnchoredRect( _iLeft, null, null, _iBottom, _iWidth, _iHeight );
        }

        public static AnchoredRect CreateBottomRightAnchored( int _iRight, int _iBottom, int _iWidth, int _iHeight )
        {
            return new AnchoredRect( null, null, _iRight, _iBottom, _iWidth, _iHeight );
        }

        public static AnchoredRect CreateTopRightAnchored( int _iRight, int _iTop, int _iWidth, int _iHeight )
        {
            return new AnchoredRect( null, _iTop, _iRight, null, _iWidth, _iHeight );
        }

        public static AnchoredRect CreateTopLeftAnchored( int _iLeft, int _iTop, int _iWidth, int _iHeight )
        {
            return new AnchoredRect( _iLeft, _iTop, null, null, _iWidth, _iHeight );
        }

        public static AnchoredRect CreateCentered( int _iWidth, int _iHeight )
        {
            return new AnchoredRect( null, null, null, null, _iWidth, _iHeight );
        }
    }
}
