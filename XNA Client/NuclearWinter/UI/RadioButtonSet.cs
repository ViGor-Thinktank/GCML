using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NuclearWinter.Animation;
using Microsoft.Xna.Framework.Graphics;

namespace NuclearWinter.UI
{
    public class RadioButtonSet: Widget
    {
        //----------------------------------------------------------------------
        public struct RadioButtonSetStyle
        {
            //------------------------------------------------------------------
            public int                      CornerSize;
            public int                      FrameOffset;

            public Color                    TextColor;
            public Color                    TextDownColor;

            public Texture2D                ButtonFrameLeft;
            public Texture2D                ButtonFrameMiddle;
            public Texture2D                ButtonFrameRight;

            public Texture2D                ButtonFrameLeftDown;
            public Texture2D                ButtonFrameMiddleDown;
            public Texture2D                ButtonFrameRightDown;

            //------------------------------------------------------------------
            public RadioButtonSetStyle(
                int         _iCornerSize,
                int         _iFrameOffset,

                Color       _textColor,
                Color       _textDownColor,

                Texture2D   _buttonFrameLeft,
                Texture2D   _buttonFrameMiddle,
                Texture2D   _buttonFrameRight,

                Texture2D   _buttonFrameLeftDown,
                Texture2D   _buttonFrameMiddleDown,
                Texture2D   _buttonFrameRightDown
            )
            {
                CornerSize              = _iCornerSize;
                FrameOffset             = _iFrameOffset;

                TextColor               = _textColor;
                TextDownColor           = _textDownColor;

                ButtonFrameLeft         = _buttonFrameLeft;
                ButtonFrameMiddle       = _buttonFrameMiddle;
                ButtonFrameRight        = _buttonFrameRight;

                ButtonFrameLeftDown     = _buttonFrameLeftDown;
                ButtonFrameMiddleDown   = _buttonFrameMiddleDown;
                ButtonFrameRightDown    = _buttonFrameRightDown;
           }
        }

        //----------------------------------------------------------------------
        List<Button>                    mlButtons;
        public IList<Button>            Buttons { get { return mlButtons.AsReadOnly(); } }

        int                             miHoveredButton;
        bool                            mbIsPressed;

        RadioButtonSetStyle             mStyle;
        public RadioButtonSetStyle Style
        {
            get { return mStyle; }
            set {
                mStyle = value;

                int i = 0;
                foreach( Button button in mlButtons )
                {
                    button.Parent = this;
                    button.TextColor = ( SelectedButtonIndex == i ) ? mStyle.TextDownColor : mStyle.TextColor;
                    button.Padding = new Box( 0, mStyle.FrameOffset );
                    button.Margin = new Box( 0, -mStyle.FrameOffset );

                    button.Style.FrameDown  = Style.ButtonFrameMiddleDown;
                    button.ClickHandler     = ButtonClicked;

                    i++;
                }

                Button firstButton = mlButtons.First();
                firstButton.Style.FrameDown = Style.ButtonFrameLeftDown;
                firstButton.Margin = new Box( 0, -mStyle.FrameOffset, 0, 0 );

                Button lastButton = mlButtons.Last();
                lastButton.Style.FrameDown = Style.ButtonFrameRightDown;
                lastButton.Margin = new Box( 0, 0, 0, -mStyle.FrameOffset );
            }
        }

        public bool Expand;

        public Action<RadioButtonSet,int>   ClickHandler;
        int                                 miSelectedButtonIndex = 0;
        public int                          SelectedButtonIndex
        {
            get {
                return miSelectedButtonIndex;
            }

            set {
                miSelectedButtonIndex = value;

                for( int iButton = 0; iButton < mlButtons.Count; iButton++ )
                {
                    Button button = mlButtons[iButton];

                    button.Style.CornerSize     = Style.CornerSize;

                    if( iButton == miSelectedButtonIndex )
                    {
                        button.TextColor            = mStyle.TextDownColor;
                        if( iButton == 0 )
                        {
                            button.Style.Frame          = Style.ButtonFrameLeftDown;
                        }
                        else
                        if( iButton == mlButtons.Count - 1 )
                        {
                            button.Style.Frame          = Style.ButtonFrameRightDown;
                        }
                        else
                        {
                            button.Style.Frame          = Style.ButtonFrameMiddleDown;
                        }
                    }
                    else
                    {
                        button.TextColor            = mStyle.TextColor;

                        if( iButton == 0 )
                        {
                            button.Style.Frame          = Style.ButtonFrameLeft;
                        }
                        else
                        if( iButton == mlButtons.Count - 1 )
                        {
                            button.Style.Frame          = Style.ButtonFrameRight;
                        }
                        else
                        {
                            button.Style.Frame          = Style.ButtonFrameMiddle;
                        }
                    }
                }
            }
        }

        public Button SelectedButton
        {
            get { return mlButtons[ miSelectedButtonIndex ]; }
        }

        //----------------------------------------------------------------------
        public override Widget GetFirstFocusableDescendant( Direction _direction )
        {
            switch( _direction )
            {
                case Direction.Left:
                    return mlButtons[ mlButtons.Count - 1 ];
                default:
                    return mlButtons[ 0 ];
            }
        }

        //----------------------------------------------------------------------
        public override Widget GetSibling( Direction _direction, Widget _child )
        {
            int iIndex = mlButtons.IndexOf( (Button)_child );

            switch( _direction )
            {
                case Direction.Left:
                    if( iIndex > 0 )
                    {
                        return mlButtons[iIndex - 1];
                    }
                    break;
                case Direction.Right:
                    if( iIndex < mlButtons.Count - 1 )
                    {
                        return mlButtons[iIndex + 1];
                    }
                    break;
            }

            return base.GetSibling( _direction, this );
        }

        //----------------------------------------------------------------------
        public RadioButtonSet( Screen _screen, List<Button> _lButtons, int _iInitialButtonIndex, bool _bExpand = false )
        : base( _screen )
        {
            mlButtons = _lButtons;

            Style = new RadioButtonSetStyle(
                Screen.Style.RadioButtonCornerSize,
                Screen.Style.RadioButtonFrameOffset,
                
                Color.White * 0.6f,
                Color.White,
                Screen.Style.ButtonFrameLeft,
                Screen.Style.ButtonFrameMiddle,
                Screen.Style.ButtonFrameRight,

                Screen.Style.ButtonFrameLeftDown,
                Screen.Style.ButtonFrameMiddleDown,
                Screen.Style.ButtonFrameRightDown
            );

            SelectedButtonIndex = _iInitialButtonIndex;
            Expand = _bExpand;

            UpdateContentSize();
        }

        public RadioButtonSet( Screen _screen, List<Button> _lButtons, bool _bExpand = false )
        : this( _screen, _lButtons, 0, _bExpand )
        {
        }

        //----------------------------------------------------------------------
        public RadioButtonSet( Screen _screen, RadioButtonSetStyle _style, List<Button> _lButtons, int _iInitialButtonIndex )
        : base( _screen )
        {
            mlButtons = _lButtons;

            Style = _style;

            SelectedButtonIndex = _iInitialButtonIndex;

            UpdateContentSize();
        }

        public RadioButtonSet( Screen _screen, RadioButtonSetStyle _style, List<Button> _lButtons )
        : this( _screen, _style, _lButtons, 0 )
        {
        }

        //----------------------------------------------------------------------
        protected internal override void UpdateContentSize()
        {
            ContentWidth    = Padding.Horizontal;
            ContentHeight   = 0;
            foreach( Button button in mlButtons )
            {
                ContentWidth += button.ContentWidth;
                ContentHeight = Math.Max( ContentHeight, button.ContentHeight );
            }

            ContentHeight += Padding.Vertical;

            base.UpdateContentSize();
        }

        //----------------------------------------------------------------------
        public override void DoLayout( Rectangle _rect )
        {
            base.DoLayout( _rect );

            Point pCenter = LayoutRect.Center;

            int iHeight = LayoutRect.Height;

            HitBox = new Rectangle(
                pCenter.X - ( Expand ? LayoutRect.Width : ContentWidth ) / 2,
                pCenter.Y - iHeight / 2,
                Expand ? LayoutRect.Width : ContentWidth,
                iHeight
            );

            float fExpandedButtonWidth = (float)LayoutRect.Width / mlButtons.Count;

            int iButton = 0;
            int iButtonX = 0;

            float fOffset = 0f;

            foreach( Button button in mlButtons )
            {
                int iWidth = button.ContentWidth;

                if( Expand )
                {
                    if( iButton < mlButtons.Count - 1 )
                    {
                        iWidth = (int)Math.Floor( fExpandedButtonWidth + fOffset - Math.Floor( fOffset ) );
                    }
                    else
                    {
                        iWidth = (int)( LayoutRect.Width - Math.Floor( fOffset ) );
                    }
                    fOffset += fExpandedButtonWidth;
                }
                else
                {
                    fOffset += iWidth;
                }

                button.DoLayout( new Rectangle(
                    HitBox.X + iButtonX, pCenter.Y - iHeight / 2,
                    iWidth, iHeight
                ) );

                iButtonX += iWidth;
                iButton++;
            }
        }

        public override void Update( float _fElapsedTime )
        {
            foreach( Button button in mlButtons )
            {
                button.Update( _fElapsedTime );
            }
        }

        //----------------------------------------------------------------------
        public override void OnMouseEnter( Point _hitPoint )
        {
            base.OnMouseEnter( _hitPoint );
            UpdateHoveredButton( _hitPoint );

            mlButtons[miHoveredButton].OnMouseEnter( _hitPoint );
        }

        public override void OnMouseOut( Point _hitPoint )
        {
            base.OnMouseOut( _hitPoint );

            mlButtons[miHoveredButton].OnMouseOut( _hitPoint );
        }

        public override void OnMouseMove(Point _hitPoint)
        {
            base.OnMouseMove(_hitPoint);

            if( ! mbIsPressed )
            {
                int iPreviousHoveredButton = miHoveredButton;
                UpdateHoveredButton( _hitPoint );

                if( iPreviousHoveredButton != miHoveredButton )
                {
                    mlButtons[iPreviousHoveredButton].OnMouseOut( _hitPoint );
                }
                mlButtons[miHoveredButton].OnMouseEnter( _hitPoint );
            }
            else
            {
                mlButtons[miHoveredButton].OnMouseMove( _hitPoint );
            }
        }

        void UpdateHoveredButton( Point _hitPoint )
        {
            int iButton = 0;
            foreach( Button button in mlButtons )
            {
                if( button.HitTest( _hitPoint ) != null )
                {
                    miHoveredButton = iButton;
                    break;
                }

                iButton++;
            }
        }

        //----------------------------------------------------------------------
        protected internal override bool OnMouseDown( Point _hitPoint, int _iButton )
        {
            if( _iButton != Screen.Game.InputMgr.PrimaryMouseButton ) return false;

            mbIsPressed = true;
            mlButtons[miHoveredButton].OnMouseDown( _hitPoint, _iButton );

            return true;
        }

        protected internal override void OnMouseUp( Point _hitPoint, int _iButton )
        {
            if( _iButton != Screen.Game.InputMgr.PrimaryMouseButton ) return;

            mbIsPressed = false;
            mlButtons[miHoveredButton].OnMouseUp( _hitPoint, _iButton );
        }

        internal void ButtonClicked( Button _button )
        {
            int iSelectedButtonIndex = mlButtons.IndexOf( _button );
            if( ClickHandler != null )
            {
                ClickHandler( this, iSelectedButtonIndex );
            }
            SelectedButtonIndex = iSelectedButtonIndex;
        }

        //----------------------------------------------------------------------
        public override void Draw()
        {
            foreach( Button button in mlButtons )
            {
                button.Draw();
            }
        }

        protected internal override void DrawHovered()
        {
            mlButtons[ miHoveredButton ].DrawHovered();
        }
    }
}
