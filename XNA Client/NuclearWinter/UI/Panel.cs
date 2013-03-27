using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NuclearWinter.UI
{
    /*
     * A widget that draws a box with the specified texture & corner size
     * Can also contain stuff
     */
    public class Panel: Group
    {
        public Texture2D        Texture;
        public int              CornerSize;

        public bool             DoClipping;

        bool mbEnableScrolling;
        public bool             EnableScrolling {
            get { return mbEnableScrolling; }
            set {
                mbEnableScrolling = value;
                HasDynamicHeight = mbEnableScrolling;
            }
        }

        public Scrollbar        Scrollbar { get; private set; }

        protected Box           mMargin;
        public Box              Margin
        {
            get { return mMargin; }

            set {
                mMargin = value;
                UpdateContentSize();
            }
        }

        //----------------------------------------------------------------------
        public Panel( Screen _screen, Texture2D _texture, int _iCornerSize )
        : base( _screen )
        {
            Texture     = _texture;
            CornerSize  = _iCornerSize;
            Padding     = new Box( CornerSize );

            Scrollbar   = new Scrollbar( _screen );
            Scrollbar.Parent = this;
        }

        //----------------------------------------------------------------------
        public override void Update( float _fElapsedTime )
        {
            if( EnableScrolling )
            {
                Scrollbar.Update( _fElapsedTime );
            }

            base.Update( _fElapsedTime );
        }

        //----------------------------------------------------------------------
        public override void DoLayout( Rectangle _rect )
        {
            base.DoLayout( _rect );

            if( EnableScrolling )
            {
                Scrollbar.DoLayout( LayoutRect, ContentHeight );
            }

            HitBox = LayoutRect;
        }

        protected override void LayoutChildren()
        {
            Rectangle childRectangle;
            
            if( EnableScrolling )
            {
                childRectangle = new Rectangle( LayoutRect.X + Padding.Left + Margin.Left, LayoutRect.Y + Padding.Top + Margin.Top - (int)Scrollbar.LerpOffset, LayoutRect.Width - Padding.Horizontal - Margin.Horizontal, LayoutRect.Height - Padding.Vertical - Margin.Vertical + Scrollbar.Max );
            }
            else
            {
                childRectangle = new Rectangle( LayoutRect.X + Padding.Left + Margin.Left, LayoutRect.Y + Padding.Top + Margin.Top, LayoutRect.Width - Padding.Horizontal - Margin.Horizontal, LayoutRect.Height - Padding.Vertical - Margin.Vertical );
            }
            
            foreach( Widget widget in mlChildren )
            {
                widget.DoLayout( childRectangle );
            }
        }

        public override Widget HitTest( Point _point )
        {
            return Scrollbar.HitTest( _point ) ?? base.HitTest( _point ) ?? ( HitBox.Contains( _point ) ? this : null );
        }

        //----------------------------------------------------------------------
        protected internal override void OnMouseWheel( Point _hitPoint, int _iDelta )
        {
            if( ( _iDelta < 0 && Scrollbar.Offset >= Scrollbar.Max )
             || ( _iDelta > 0 && Scrollbar.Offset <= 0 ) )
            {
                base.OnMouseWheel( _hitPoint, _iDelta );
                return;
            }

            DoScroll( -_iDelta / 120 * 50 );
        }

        void DoScroll( int _iDelta )
        {
            int iScrollChange = (int)MathHelper.Clamp( _iDelta, -Scrollbar.Offset, Math.Max( 0, Scrollbar.Max - Scrollbar.Offset ) );
            Scrollbar.Offset += iScrollChange;
        }

        //----------------------------------------------------------------------
        public override void Draw()
        {
            Screen.DrawBox( Texture, new Rectangle( LayoutRect.X + Margin.Left, LayoutRect.Y + Margin.Top, LayoutRect.Width - Margin.Horizontal, LayoutRect.Height - Margin.Vertical ), CornerSize, Color.White );

            if( DoClipping )
            {
                Screen.PushScissorRectangle( new Rectangle( LayoutRect.X + Padding.Left + Margin.Left, LayoutRect.Y + Padding.Top + Margin.Top, LayoutRect.Width - Padding.Horizontal - Margin.Horizontal, LayoutRect.Height - Padding.Vertical - Margin.Vertical ) );
            }

            base.Draw();

            if( DoClipping )
            {
                Screen.PopScissorRectangle();
            }

            if( EnableScrolling )
            {
                Scrollbar.Draw();
            }
        }
    }
}
