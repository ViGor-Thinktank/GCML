using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NuclearWinter.UI
{
    public class Scrollbar: Widget
    {
        //----------------------------------------------------------------------

        public int              Offset;
        int                     mMouseOffset;

        public int              Max
        {
            get { return miMax; }

            set
            {
                miMax = value;
                Offset = Math.Min( Offset, miMax );
            }
        }

        public float            LerpOffset;

        public bool             ScrollToBottom;

        //----------------------------------------------------------------------
        int                     miMax;

        int                     miScrollbarHeight;
        int                     miScrollbarOffset;

        Point                   mMouseDragPoint;
        Point                   mLastMouseDragPoint;

        bool                    mbIsMouseDown;

        Color                   mScrollbarColor;

        public Rectangle        ScrollRect;

        static Color            sDefaultColor = Color.White * 0.5f;
        static Color            sHoveredColor = Color.White;

        //----------------------------------------------------------------------
        public Scrollbar( Screen _screen )
        : base( _screen )
        {
            mScrollbarColor = sDefaultColor;
        }

        //----------------------------------------------------------------------
        public override void Update( float _fElapsedTime )
        {
            float fLerpAmount = Math.Min( 1f, _fElapsedTime * NuclearGame.LerpMultiplier );
            LerpOffset = MathHelper.Lerp( LerpOffset, Offset, fLerpAmount );
            LerpOffset = Math.Min( LerpOffset, Max );
        }

        //----------------------------------------------------------------------
        public void DoLayout( Rectangle _rect, int _iContentHeight )
        {
            ScrollRect = _rect;

            HitBox = new Rectangle( ScrollRect.Right - 10 - Screen.Style.VerticalScrollbar.Width / 2, ScrollRect.Y, Screen.Style.VerticalScrollbar.Width, ScrollRect.Height );

            bool bScrolledToBottom = ScrollToBottom && Offset >= Max;

            Max = Math.Max( 0, _iContentHeight - ScrollRect.Height );
            Offset = (int)MathHelper.Clamp( Offset, 0, Max );

            if( bScrolledToBottom )
            {
                Offset = Max;
            }

            miScrollbarHeight = Math.Max( 10,  (int)( ( ScrollRect.Height - 20 ) / ( (float)_iContentHeight / ( ScrollRect.Height - 20 ) ) ) );
            miScrollbarOffset = (int)( (float)LerpOffset / Max * (float)( ScrollRect.Height - 20 - miScrollbarHeight ) );
        }

        //----------------------------------------------------------------------
        public override void OnMouseEnter( Point _hitPoint )
        {
            mScrollbarColor = sHoveredColor;
        }

        public override void OnMouseOut( Point _hitPoint )
        {
            mScrollbarColor = sDefaultColor;
        }

        protected internal override bool OnMouseDown( Point _hitPoint, int _iButton )
        {
            if( _iButton != Screen.Game.InputMgr.PrimaryMouseButton ) return false;

            mbIsMouseDown = true;
            mLastMouseDragPoint = _hitPoint;

            int iScrollBarBottom = ScrollRect.Top + miScrollbarOffset + Screen.Style.VerticalScrollbarCornerSize;
            int iScrollBarTop    = ScrollRect.Top + miScrollbarOffset + miScrollbarHeight + Screen.Style.VerticalScrollbarCornerSize;
            
            if( ! ( mLastMouseDragPoint.Y > iScrollBarBottom && mLastMouseDragPoint.Y < iScrollBarTop ) )
                Offset += ( mLastMouseDragPoint.Y - ( ScrollRect.Top + miScrollbarOffset + ( miScrollbarHeight / 2 )  ) ) * ScrollRect.Height / miScrollbarHeight;

            mMouseOffset = Offset;

            return true;
        }

        protected internal override void OnMouseUp( Point _hitPoint, int _iButton )
        {
            if( _iButton != Screen.Game.InputMgr.PrimaryMouseButton ) return;

            mbIsMouseDown = false;
        }

        public override void OnMouseMove( Point _hitPoint )
        {
            if( mbIsMouseDown )
            {
                mMouseDragPoint = _hitPoint;

                mMouseOffset += (mMouseDragPoint.Y - mLastMouseDragPoint.Y) * ScrollRect.Height / miScrollbarHeight;
                Offset = mMouseOffset;


                mLastMouseDragPoint = mMouseDragPoint;
            }

        }

        //----------------------------------------------------------------------
        public override void Draw()
        {
            if( miMax > 0 )
            {
                Screen.DrawBox(
                    Screen.Style.VerticalScrollbar,
                    new Rectangle(
                        ScrollRect.Right - 5 - Screen.Style.VerticalScrollbar.Width / 2,
                        ScrollRect.Y + 10 + miScrollbarOffset,
                        Screen.Style.VerticalScrollbar.Width,
                        miScrollbarHeight ),
                    Screen.Style.VerticalScrollbarCornerSize,
                    mScrollbarColor );
            }
        }
    }
}
