using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NuclearWinter.Animation;
using Microsoft.Xna.Framework.Input;

namespace NuclearWinter.UI
{
    /*
     * A clickable Button containing a Label and an optional Image icon
     */
    public class Button: Widget
    {
        //----------------------------------------------------------------------
        public struct ButtonStyle
        {
            //------------------------------------------------------------------
            public int              CornerSize;
            public Texture2D        Frame;
            public Texture2D        FrameDown;
            public Texture2D        FrameHover;
            public Texture2D        FramePressed;
            public Texture2D        FrameFocus;
            public int              VerticalPadding;
            public int              HorizontalPadding;

            //------------------------------------------------------------------
            public ButtonStyle(
                int         _iCornerSize,
                Texture2D   _buttonFrame,
                Texture2D   _buttonFrameDown,
                Texture2D   _buttonFrameHover,
                Texture2D   _buttonFramePressed,
                Texture2D   _buttonFrameFocused,
                int         _iVerticalPadding,
                int         _iHorizontalPadding
            )
            {
                CornerSize      = _iCornerSize;
                Frame           = _buttonFrame;
                FrameDown       = _buttonFrameDown;
                FrameHover      = _buttonFrameHover;
                FramePressed    = _buttonFramePressed;
                FrameFocus      = _buttonFrameFocused;

                VerticalPadding     = _iVerticalPadding;
                HorizontalPadding   = _iHorizontalPadding;
            }
        }

        public UIFont Font {
            get { return mLabel.Font; }
            set { mLabel.Font = value; UpdateContentSize(); }
        }

        //----------------------------------------------------------------------
        Label                   mLabel;
        Image                   mIcon;
        Buttons                 mBoundPadButton;

        bool                    mbIsHovered;

        AnimatedValue           mPressedAnim;
        bool                    mbIsPressed;

        protected Box           mMargin;
        public Box              Margin
        {
            get { return mMargin; }

            set {
                mMargin = value;
                UpdateContentSize();
            }
        }

        public string           Text
        {
            get
            {
                return mLabel.Text;
            }
            
            set
            {
                mLabel.Text = value;
                mLabel.Padding = mLabel.Text != "" ? new Box( Style.VerticalPadding, Style.HorizontalPadding ) : new Box( Style.VerticalPadding, Style.HorizontalPadding, Style.VerticalPadding, 0 );
                UpdateContentSize();
            }
        }

        public Texture2D        Icon
        {
            get {
                return mIcon.Texture;
            }

            set
            {
                mIcon.Texture = value;
                UpdateContentSize();
            }
        }

        public Color            IconColor
        {
            get {
                return mIcon.Color;
            }

            set
            {
                mIcon.Color = value;
            }
        }


        Anchor mAnchor;
        public Anchor Anchor
        {
            get {
                return mAnchor;
            }

            set
            {
                mAnchor = value;
            }
        }

        public Color TextColor
        {
            get { return mLabel.Color; }
            set { mLabel.Color = value; }
        }

        public ButtonStyle Style;

        public Action<Button>   ClickHandler;
        public object           Tag;

        public string       TooltipText
        {
            get { return mTooltip.Text; }
            set { mTooltip.Text = value; }
        }

        //----------------------------------------------------------------------
        Tooltip             mTooltip;

        //----------------------------------------------------------------------
        public Button( Screen _screen, ButtonStyle _style, string _strText = "", Texture2D _iconTex = null, Anchor _anchor = Anchor.Center, string _strTooltipText="", object _tag=null )
        : base( _screen )
        {
            Style = _style;

            mPadding    = new Box(5, 0);
            mMargin     = new Box(0);

            mLabel          = new Label( _screen );

            mIcon           = new Image( _screen );
            mIcon.Texture   = _iconTex;
            mIcon.Padding   = new Box( Style.VerticalPadding, 0, Style.VerticalPadding, Style.HorizontalPadding );

            Text            = _strText;
            TextColor       = Screen.Style.DefaultTextColor;

            Anchor          = _anchor;

            mPressedAnim    = new SmoothValue( 1f, 0f, 0.2f );
            mPressedAnim.SetTime( mPressedAnim.Duration );

            mTooltip        = new Tooltip( Screen, "" );

            TooltipText     = _strTooltipText;
            Tag             = _tag;

            UpdateContentSize();
        }

        //----------------------------------------------------------------------
        public Button( Screen _screen, string _strText = "", Texture2D _iconTex = null, Anchor _anchor = Anchor.Center, string _strTooltipText="", object _tag=null )
        : this( _screen, new ButtonStyle(
                _screen.Style.ButtonCornerSize,
                _screen.Style.ButtonFrame,
                _screen.Style.ButtonFrameDown,
                _screen.Style.ButtonHover,
                _screen.Style.ButtonPress,
                _screen.Style.ButtonFocus,
                _screen.Style.ButtonVerticalPadding,
                _screen.Style.ButtonHorizontalPadding
            ), _strText, _iconTex, _anchor, _strTooltipText, _tag )
        {
        }

        //----------------------------------------------------------------------
        protected internal override void UpdateContentSize()
        {
            ContentWidth    = ( (mIcon.Texture != null ) ? mIcon.ContentWidth : 0 ) + mLabel.ContentWidth + Padding.Horizontal + mMargin.Horizontal;
            ContentHeight   = Math.Max( mIcon.ContentHeight, mLabel.ContentHeight ) + Padding.Vertical + mMargin.Vertical;

            base.UpdateContentSize();
        }

        //----------------------------------------------------------------------
        public override void DoLayout( Rectangle _rect )
        {
            base.DoLayout( _rect );
            HitBox = LayoutRect;
            Point pCenter = LayoutRect.Center;

            switch( mAnchor )
            {
                case UI.Anchor.Start:
                    if( mIcon.Texture != null )
                    {
                        mIcon.DoLayout( new Rectangle( LayoutRect.X + Padding.Left + Margin.Left, pCenter.Y - mIcon.ContentHeight / 2, mIcon.ContentWidth, mIcon.ContentHeight ) );
                    }

                    mLabel.DoLayout(
                        new Rectangle(
                            LayoutRect.X + Padding.Left + Margin.Left + ( mIcon.Texture != null ? mIcon.ContentWidth : 0 ), pCenter.Y - mLabel.ContentHeight / 2,
                            mLabel.ContentWidth, mLabel.ContentHeight
                        )
                    );
                    break;
                case UI.Anchor.Center:
                    if( mIcon.Texture != null )
                    {
                        mIcon.DoLayout( new Rectangle( pCenter.X - ContentWidth / 2 + Padding.Left + Margin.Left, pCenter.Y - mIcon.ContentHeight / 2, mIcon.ContentWidth, mIcon.ContentHeight ) );
                    }

                    mLabel.DoLayout(
                        new Rectangle(
                            pCenter.X - ContentWidth / 2 + Padding.Left + Margin.Left + ( mIcon.Texture != null ? mIcon.ContentWidth : 0 ), pCenter.Y - mLabel.ContentHeight / 2,
                            mLabel.ContentWidth, mLabel.ContentHeight
                        )
                    );
                    break;
            }
        }

        //----------------------------------------------------------------------
        public override void Update( float _fElapsedTime )
        {
            if( ! mPressedAnim.IsOver )
            {
                mPressedAnim.Update( _fElapsedTime );
            }

            mTooltip.EnableDisplayTimer = mbIsHovered;
            mTooltip.Update( _fElapsedTime );
        }

        //----------------------------------------------------------------------
        public override void OnMouseEnter( Point _hitPoint )
        {
            base.OnMouseEnter( _hitPoint );
            mbIsHovered = true;
        }

        public override void OnMouseOut( Point _hitPoint )
        {
            base.OnMouseOut( _hitPoint );
            mbIsHovered = false;
            mTooltip.EnableDisplayTimer = false;
        }

        //----------------------------------------------------------------------
        protected internal override bool OnMouseDown( Point _hitPoint, int _iButton )
        {
            if( _iButton != Screen.Game.InputMgr.PrimaryMouseButton ) return false;

            Screen.Focus( this );
            OnActivateDown();

            return true;
        }

        protected internal override void OnMouseUp( Point _hitPoint, int _iButton )
        {
            if( _iButton != Screen.Game.InputMgr.PrimaryMouseButton ) return;

            if( HitTest( _hitPoint ) == this )
            {
                OnActivateUp();
            }
            else
            {
                ResetPressState();
            }
        }

        //----------------------------------------------------------------------
        public void BindPadButton( Buttons _button )
        {
            mBoundPadButton = _button;
        }

        //----------------------------------------------------------------------
        protected internal override bool OnPadButton( Buttons _button, bool _bIsDown )
        {
            if( _button == mBoundPadButton )
            {
                if( _bIsDown )
                {
                    Screen.Focus( this );
                    OnActivateDown();
                }
                else
                {
                    OnActivateUp();
                }

                return true;
            }

            return false;
        }

        //----------------------------------------------------------------------
        protected internal override void OnActivateDown()
        {
            mbIsPressed = true;
            mPressedAnim.SetTime( 0f );
        }

        protected internal override void OnActivateUp()
        {
            mPressedAnim.SetTime( 0f );
            mbIsPressed = false;
            if( ClickHandler != null ) ClickHandler( this );
        }

        protected internal override void OnBlur()
        {
            ResetPressState();
        }

        //----------------------------------------------------------------------
        internal void ResetPressState()
        {
            mPressedAnim.SetTime( 1f );
            mbIsPressed = false;
        }

        //----------------------------------------------------------------------
        public override void Draw()
        {
            Texture2D frame = (!mbIsPressed) ? Style.Frame : Style.FrameDown;

            if( frame != null )
            {
                Screen.DrawBox( frame, LayoutRect, Style.CornerSize, Color.White );
            }

            Rectangle marginRect = new Rectangle( LayoutRect.X + Margin.Left, LayoutRect.Y + Margin.Top, LayoutRect.Width - Margin.Left - Margin.Right, LayoutRect.Height - Margin.Top - Margin.Bottom );

            if( mbIsHovered && ! mbIsPressed && mPressedAnim.IsOver )
            {
                if( Screen.IsActive && Style.FrameHover != null )
                {
                    Screen.DrawBox( Style.FrameHover, marginRect, Style.CornerSize, Color.White );
                }
            }
            else
            if( mPressedAnim.CurrentValue > 0f )
            {
                if( Style.FramePressed != null )
                {
                    Screen.DrawBox( Style.FramePressed, marginRect, Style.CornerSize, Color.White * mPressedAnim.CurrentValue );
                }
            }

            if( Screen.IsActive && HasFocus && ! mbIsPressed )
            {
                if( Style.FrameFocus != null )
                {
                    Screen.DrawBox( Style.FrameFocus, marginRect, Style.CornerSize, Color.White );
                }
            }

            mLabel.Draw();

            mIcon.Draw();
        }

        //----------------------------------------------------------------------
        protected internal override void DrawHovered()
        {
            mTooltip.Draw();
        }
    }
}
