using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;

#if !MONOGAME
using OSKey = System.Windows.Forms.Keys;
#elif !MONOMAC
using OSKey = OpenTK.Input.Key;
#else
using OSKey = MonoMac.AppKit.NSKey;
#endif

namespace NuclearWinter.UI
{
    public struct Box
    {
        //----------------------------------------------------------------------
        public int Top;
        public int Right;
        public int Bottom;
        public int Left;

        public int Horizontal   { get { return Left + Right; } }
        public int Vertical     { get { return Top + Bottom; } }

        public Box( int _iTop, int _iRight, int _iBottom, int _iLeft )
        {
            Top     = _iTop;
            Right   = _iRight;
            Bottom  = _iBottom;
            Left    = _iLeft;
        }

        //----------------------------------------------------------------------
        public Box( int _iValue )
        {
            Left = Right = Top = Bottom = _iValue;
        }

        //----------------------------------------------------------------------
        public Box( int _iVertical, int _iHorizontal )
        {
            Top = Bottom = _iVertical;
            Left = Right = _iHorizontal;
        }
    }

    public abstract class Widget
    {
        //----------------------------------------------------------------------
        public Screen           Screen              { get; private set; }
        public Widget           Parent;

        /// <summary>
        /// Returns whether the widget is connected to its Screen's root
        /// </summary>
        public bool IsOrphan
        {
            get {
                Widget widget = this;
                while( widget.Parent != null && widget.Parent != Screen.Root ) widget = widget.Parent;
                return widget.Parent == null;
            }
        }

        public int              ContentWidth        { get; protected set; }
        public int              ContentHeight       { get; protected set; }

        public AnchoredRect     AnchoredRect;
        public Rectangle        LayoutRect          { get; protected set; }

        protected Box           mPadding;
        public Box              Padding
        {
            get { return mPadding; }
            set {
                mPadding = value;
                UpdateContentSize();
            }
        }

        public virtual Widget GetSibling( UI.Direction _direction, Widget _child )
        {
            if( Parent != null )
            {
                return Parent.GetSibling( _direction, this );
            }

            return null;
        }

        public virtual Widget GetFirstFocusableDescendant( UI.Direction _direction )
        {
            return this;
        }

        public bool HasFocus
        {
            get
            {
                return Screen.FocusedWidget == this;
            }
        }

        public bool IsHovered
        {
            get
            {
                return Screen.HoveredWidget == this;
            }
        }

        public Rectangle        HitBox              { get; protected set; }

        //----------------------------------------------------------------------
        public Widget( Screen _screen, AnchoredRect _anchoredRect )
        {
            Screen = _screen;
            AnchoredRect = _anchoredRect;
        }

        public Widget( Screen _screen )
        : this( _screen, AnchoredRect.Full )
        {
        }

        //----------------------------------------------------------------------
        public virtual void DoLayout( Rectangle _rect )
        {
            Rectangle childRectangle;

            // Horizontal
            if( AnchoredRect.Left.HasValue )
            {
                childRectangle.X = _rect.Left + AnchoredRect.Left.Value;
                if( AnchoredRect.Right.HasValue )
                {
                    // Horizontally anchored
                    childRectangle.Width = ( _rect.Right - AnchoredRect.Right.Value ) - childRectangle.X;
                }
                else
                {
                    // Left-anchored
                    childRectangle.Width = AnchoredRect.Width;
                }
            }
            else
            {
                childRectangle.Width = AnchoredRect.Width;

                if( AnchoredRect.Right.HasValue )
                {
                    // Right-anchored
                    childRectangle.X = ( _rect.Right - AnchoredRect.Right.Value ) - childRectangle.Width;
                }
                else
                {
                    // Centered
                    childRectangle.X = _rect.Center.X - childRectangle.Width / 2;
                }
            }

            // Vertical
            if( AnchoredRect.Top.HasValue )
            {
                childRectangle.Y = _rect.Top + AnchoredRect.Top.Value;
                if( AnchoredRect.Bottom.HasValue )
                {
                    // Horizontally anchored
                    childRectangle.Height = ( _rect.Bottom - AnchoredRect.Bottom.Value ) - childRectangle.Y;
                }
                else
                {
                    // Top-anchored
                    childRectangle.Height = AnchoredRect.Height;
                }
            }
            else
            {
                childRectangle.Height = AnchoredRect.Height;

                if( AnchoredRect.Bottom.HasValue )
                {
                    // Bottom-anchored
                    childRectangle.Y = ( _rect.Bottom - AnchoredRect.Bottom.Value ) - childRectangle.Height;
                }
                else
                {
                    // Centered
                    childRectangle.Y = _rect.Center.Y - childRectangle.Height / 2;
                }
            }

            LayoutRect = childRectangle;
        }

        //----------------------------------------------------------------------
        public virtual Widget HitTest( Point _point )
        {
            return HitBox.Contains( _point ) ? this : null;
        }

        //----------------------------------------------------------------------
        protected Point TransformPointScreenToWidget( Point _point )
        {
            return new Point( _point.X - LayoutRect.X, _point.Y - LayoutRect.Y );
        }

        //----------------------------------------------------------------------
        public virtual void Update( float _fElapsedTime ) {}

        protected internal virtual void UpdateContentSize()
        {
            // Compute content size then call this!

            if( Parent != null )
            {
                Parent.UpdateContentSize();
            }
        }

        //----------------------------------------------------------------------
        // Events

        // Called when a mouse button was pressed. Widget should return true if event was consumed
        // Otherwise it will bubble up to its parent
        protected internal virtual bool       OnMouseDown ( Point _hitPoint, int _iButton ) { return true; }
        protected internal virtual void       OnMouseUp   ( Point _hitPoint, int _iButton ) {}


        protected internal virtual bool       OnMouseDoubleClick( Point _hitPoint ) { return false; }

        public virtual void                 OnMouseEnter( Point _hitPoint ) {}
        public virtual void                 OnMouseOut  ( Point _hitPoint ) {}
        public virtual void                 OnMouseMove ( Point _hitPoint ) {}
        
        protected internal virtual void     OnMouseWheel( Point _hitPoint, int _iDelta ) { if( Parent != null ) Parent.OnMouseWheel( _hitPoint, _iDelta ); }

        public const int                    MouseDragTriggerDistance   = 10;

        protected internal virtual void OnOSKeyPress( OSKey _key )
        {
            if( _key == OSKey.Tab )
            {
                List<Direction> directions = new List<Direction>();

                if( Screen.Game.InputMgr.KeyboardState.Native.IsKeyDown( Keys.LeftShift ) || Screen.Game.InputMgr.KeyboardState.Native.IsKeyDown( Keys.RightShift ) )
                {
                    directions.Add( Direction.Left );
                    directions.Add( Direction.Up );
                }
                else
                {
                    directions.Add( Direction.Right );
                    directions.Add( Direction.Down );
                }

                foreach( Direction direction in directions )
                {
                    Widget widget = GetSibling( direction, this );

                    if( widget != null )
                    {
                        Widget focusableWidget = widget.GetFirstFocusableDescendant( direction );

                        if( focusableWidget != null )
                        {
                            Screen.Focus( focusableWidget );
                            break;
                        }
                    }
                }
            }
            else
            {
                if( Parent != null ) Parent.OnOSKeyPress( _key );
            }
        }

        protected internal virtual void       OnKeyPress  ( Keys _key ) { if( Parent != null ) Parent.OnKeyPress( _key ); }

        protected internal virtual void       OnTextEntered( char _char ) {}

        protected internal virtual void       OnActivateDown() {}
        protected internal virtual void       OnActivateUp() {}
        protected internal virtual bool       OnCancel( bool _bPressed ) { return false; } // return true to consume the event

        protected internal virtual void       OnFocus() {}
        protected internal virtual void       OnBlur() {}

        protected internal virtual bool       OnPadButton ( Buttons _button, bool _bIsDown ) { return false; }

        protected internal virtual void       OnPadMove( Direction _direction ) {
            Widget widget = GetSibling( _direction, this );

            if( widget != null )
            {
                Widget focusableWidget = widget.GetFirstFocusableDescendant( _direction );

                if( focusableWidget != null )
                {
                    Screen.Focus( focusableWidget );
                }
            }
        }

        //----------------------------------------------------------------------
        public abstract void                Draw();
        protected internal virtual void     DrawFocused() {}
        protected internal virtual void     DrawHovered() {}
    }
}
