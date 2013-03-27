using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NuclearWinter.UI
{
    //---------------------------------------------------------------------------
    public class Splitter: Widget
    {
        //-----------------------------------------------------------------------
        Widget mFirstPane;
        public Widget           FirstPane
        {
            get { return mFirstPane; }
            set {
                if( mFirstPane != null )
                {
                    Debug.Assert( mFirstPane.Parent == this );

                    mFirstPane.Parent = null;
                }

                mFirstPane = value;

                if( mFirstPane != null )
                {
                    Debug.Assert( mFirstPane.Parent == null );

                    mFirstPane.Parent = this;
                }
            }
        }

        Widget mSecondPane;
        public Widget           SecondPane
        {
            get { return mSecondPane; }
            set {
                if( mSecondPane != null )
                {
                    Debug.Assert( mSecondPane.Parent == this );

                    mSecondPane.Parent = null;
                }

                mSecondPane = value;

                if( mSecondPane != null )
                {
                    Debug.Assert( mSecondPane.Parent == null );

                    mSecondPane.Parent = this;
                }
            }
        }

        public int      FirstPaneMinSize = 100;
        public int      SecondPaneMinSize = 100;

        public bool     InvertDrawOrder;

        public bool     Collapsable;

        public void ToggleCollapse()
        {
            if( Collapsable ) mbCollapsed = ! mbCollapsed;
        }

        bool            mbCollapsed;
        Animation.AnimatedValue mCollapseAnim;
        bool            mbDisplayFirstPane;
        bool            mbDisplaySecondPane;

        Direction       mDirection;
        public int      SplitterOffset;
        int             SplitterSize { get { return Screen.Style.SplitterSize; } }

        bool            mbIsDragging;
        int             miDragOffset;
        bool            mbIsHovered;

        //-----------------------------------------------------------------------
        // NOTE: Splitter is using a Direction instead of an Orientation so
        // it know from which side the offset is computed
        public Splitter( Screen _screen, Direction _direction )
        : base( _screen )
        {
            mDirection = _direction;

            mCollapseAnim = new Animation.SmoothValue( 0f, 1f, 0.2f );
        }

        public override void Update( float _fElapsedTime )
        {
            if( mbCollapsed )
            {
                mCollapseAnim.Direction = Animation.AnimationDirection.Forward;
                mCollapseAnim.Update( _fElapsedTime );
            }
            else
            {
                mCollapseAnim.Direction = Animation.AnimationDirection.Backward;
                mCollapseAnim.Update( _fElapsedTime );
            }

            if( mFirstPane != null )
            {
                mFirstPane.Update( _fElapsedTime );
            }

            if( mSecondPane != null )
            {
                mSecondPane.Update( _fElapsedTime );
            }
        }

        //-----------------------------------------------------------------------
        public override void DoLayout( Rectangle _rect )
        {
            base.DoLayout( _rect );

            bool bHidePane = mbCollapsed || mCollapseAnim.CurrentValue != 0f;

            mbDisplayFirstPane = ( ! bHidePane || ( mDirection != Direction.Left && mDirection != Direction.Up ) );
            mbDisplaySecondPane = ( ! bHidePane || ( mDirection != Direction.Right && mDirection != Direction.Down ) );

            if( bHidePane )
            {
                int iCollapseOffset = (int)( ( 1f - mCollapseAnim.CurrentValue ) * SplitterOffset );

                switch( mDirection )
                {
                    case Direction.Left: {
                        mSecondPane.DoLayout( new Rectangle( LayoutRect.Left + iCollapseOffset, LayoutRect.Top, LayoutRect.Width - iCollapseOffset, LayoutRect.Height ) );

                        HitBox = new Rectangle(
                            LayoutRect.Left + iCollapseOffset - SplitterSize / 2,
                            LayoutRect.Top,
                            SplitterSize,
                            LayoutRect.Height );
                        break;
                    }
                    case Direction.Up: {
                        mSecondPane.DoLayout( LayoutRect );

                        HitBox = new Rectangle(
                            LayoutRect.Top,
                            LayoutRect.Top + iCollapseOffset - SplitterSize / 2,
                            LayoutRect.Width,
                            SplitterSize
                            );
                        break;
                    }
                    case Direction.Right: {
                        mFirstPane.DoLayout( LayoutRect );

                        HitBox = new Rectangle(
                            LayoutRect.Right + iCollapseOffset - SplitterSize / 2,
                            LayoutRect.Top,
                            SplitterSize,
                            LayoutRect.Height );
                        break;
                    }
                    case Direction.Down: {
                        mFirstPane.DoLayout( LayoutRect );

                        HitBox = new Rectangle(
                            LayoutRect.Left,
                            LayoutRect.Bottom + iCollapseOffset - SplitterSize / 2,
                            SplitterSize,
                            LayoutRect.Height );
                        break;
                    }
                }
            }
            else
            {
                switch( mDirection )
                {
                    case Direction.Left: {
                        if( LayoutRect.Width >= FirstPaneMinSize + SecondPaneMinSize )
                        {
                            SplitterOffset = (int)MathHelper.Clamp( SplitterOffset, FirstPaneMinSize, LayoutRect.Width - SecondPaneMinSize );
                        }

                        HitBox = new Rectangle(
                            LayoutRect.Left + SplitterOffset - SplitterSize / 2,
                            LayoutRect.Top,
                            SplitterSize,
                            LayoutRect.Height );

                        if( mFirstPane != null )
                        {
                            mFirstPane.DoLayout( new Rectangle( LayoutRect.Left, LayoutRect.Top, SplitterOffset, LayoutRect.Height ) );
                        }

                        if( mSecondPane != null )
                        {
                            mSecondPane.DoLayout( new Rectangle( LayoutRect.Left + SplitterOffset, LayoutRect.Top, LayoutRect.Width - SplitterOffset, LayoutRect.Height ) );
                        }
                        break;
                    }
                    case Direction.Right: {
                        if( LayoutRect.Width >= FirstPaneMinSize + SecondPaneMinSize )
                        {
                            SplitterOffset = (int)MathHelper.Clamp( SplitterOffset, SecondPaneMinSize, LayoutRect.Width - FirstPaneMinSize );
                        }

                        HitBox = new Rectangle(
                            LayoutRect.Right - SplitterOffset - SplitterSize / 2,
                            LayoutRect.Top,
                            SplitterSize,
                            LayoutRect.Height );

                        if( mFirstPane != null )
                        {
                            mFirstPane.DoLayout( new Rectangle( LayoutRect.Left, LayoutRect.Top, LayoutRect.Width - SplitterOffset, LayoutRect.Height ) );
                        }

                        if( mSecondPane != null )
                        {
                            mSecondPane.DoLayout( new Rectangle( LayoutRect.Right - SplitterOffset, LayoutRect.Top, SplitterOffset, LayoutRect.Height ) );
                        }
                        break;
                    }
                    case Direction.Up: {
                        if( LayoutRect.Height >= FirstPaneMinSize + SecondPaneMinSize )
                        {
                            SplitterOffset = (int)MathHelper.Clamp( SplitterOffset, FirstPaneMinSize, LayoutRect.Height - SecondPaneMinSize );
                        }

                        HitBox = new Rectangle(
                            LayoutRect.Left,
                            LayoutRect.Top + SplitterOffset - SplitterSize / 2,
                            LayoutRect.Width,
                            SplitterSize );

                        if( mFirstPane != null )
                        {
                            mFirstPane.DoLayout( new Rectangle( LayoutRect.Left, LayoutRect.Top, LayoutRect.Width, SplitterOffset ) );
                        }

                        if( mSecondPane != null )
                        {
                            mSecondPane.DoLayout( new Rectangle( LayoutRect.Left, LayoutRect.Top + SplitterOffset, LayoutRect.Width, LayoutRect.Height - SplitterOffset ) );
                        }
                        break;
                    }
                    case Direction.Down: {
                        if( LayoutRect.Height >= FirstPaneMinSize + SecondPaneMinSize )
                        {
                            SplitterOffset = (int)MathHelper.Clamp( SplitterOffset, SecondPaneMinSize, LayoutRect.Height - FirstPaneMinSize );
                        }

                        HitBox = new Rectangle(
                            LayoutRect.Left,
                            LayoutRect.Bottom - SplitterOffset - SplitterSize / 2,
                            LayoutRect.Width,
                            SplitterSize );

                        if( mFirstPane != null )
                        {
                            mFirstPane.DoLayout( new Rectangle( LayoutRect.Left, LayoutRect.Top, LayoutRect.Width, LayoutRect.Height - SplitterOffset ) );
                        }

                        if( mSecondPane != null )
                        {
                            mSecondPane.DoLayout( new Rectangle( LayoutRect.Left, LayoutRect.Bottom - SplitterOffset, LayoutRect.Width, SplitterOffset ) );
                        }
                        break;
                    }
                }
            }
        }

        public override Widget HitTest( Point _point )
        {
            // The splitter itself
            if( HitBox.Contains( _point ) )
            {
                return this;
            }

            // The panes
            if( mFirstPane != null && mbDisplayFirstPane )
            {
                Widget widget = mFirstPane.HitTest( _point );
                if( widget != null )
                {
                    return widget;
                }
            }

            if( mSecondPane != null && mbDisplaySecondPane )
            {
                Widget widget = mSecondPane.HitTest( _point );
                if( widget != null )
                {
                    return widget;
                }
            }

            return null;
        }

        public override void OnMouseEnter( Point _hitPoint )
        {
            mbIsHovered = true;

            switch( mDirection )
            {
                case Direction.Left:
                case Direction.Right:
#if !MONOGAME
                    Screen.Game.Form.Cursor = Collapsable ? System.Windows.Forms.Cursors.Hand : System.Windows.Forms.Cursors.SizeWE;
#endif
                    break;
                case Direction.Up:
                case Direction.Down:
#if !MONOGAME
                    Screen.Game.Form.Cursor = Collapsable ? System.Windows.Forms.Cursors.Hand : System.Windows.Forms.Cursors.SizeNS;
#endif
                    break;
            }
        }

        public override void OnMouseOut( Point _hitPoint )
        {
            mbIsHovered = false;
#if !MONOGAME
            Screen.Game.Form.Cursor = System.Windows.Forms.Cursors.Default;
#endif
        }

        public override void OnMouseMove( Point _hitPoint )
        {
            if( ! Collapsable && mbIsDragging )
            {
                switch( mDirection )
                {
                    case Direction.Left:
                        SplitterOffset = miDragOffset + _hitPoint.X;
                        break;
                    case Direction.Right:
                        SplitterOffset = miDragOffset - _hitPoint.X;
                        break;
                    case Direction.Up:
                        SplitterOffset = miDragOffset + _hitPoint.Y;
                        break;
                    case Direction.Down:
                        SplitterOffset = miDragOffset - _hitPoint.Y;
                        break;
                }
            }
        }

        protected internal override bool OnMouseDown( Point _hitPoint, int _iButton )
        {
            if( _iButton != Screen.Game.InputMgr.PrimaryMouseButton ) return false;

            if( Collapsable )
            {
                mbIsDragging = true;
            }
            else
            {
                mbIsDragging = true;

                switch( mDirection )
                {
                    case Direction.Left:
                        miDragOffset = SplitterOffset - _hitPoint.X;
                        break;
                    case Direction.Right:
                        miDragOffset = SplitterOffset + _hitPoint.X;
                        break;
                    case Direction.Up:
                        miDragOffset = SplitterOffset - _hitPoint.Y;
                        break;
                    case Direction.Down:
                        miDragOffset = SplitterOffset + _hitPoint.Y;
                        break;
                }
            }

            Screen.Focus( this );

            return true;
        }

        protected internal override void OnMouseUp( Point _hitPoint, int _iButton )
        {
            if( Collapsable )
            {
                mbCollapsed = ! mbCollapsed;
            }

            mbIsDragging = false;
        }

        //-----------------------------------------------------------------------
        public override void Draw()
        {
            if( mbIsHovered )
            {
                Color handleColor = Color.White * ( mbIsDragging ? 1f : 0.8f );

                Screen.DrawBox( Screen.Style.SplitterFrame, HitBox, Screen.Style.SplitterFrameCornerSize, handleColor );
                Texture2D handleTex = Collapsable ? Screen.Style.SplitterCollapseArrow : Screen.Style.SplitterDragHandle;
                float fHandleAngle;
                switch( mDirection )
                {
                    case Direction.Left:
                        fHandleAngle = mbCollapsed ? 0f : MathHelper.Pi;
                        break;
                    case Direction.Right:
                        fHandleAngle = mbCollapsed ? MathHelper.Pi : 0f;
                        break;
                    case Direction.Up:
                        fHandleAngle = mbCollapsed ? MathHelper.PiOver2 : ( 3f * MathHelper.PiOver2 );
                        break;
                    case Direction.Down:
                        fHandleAngle = mbCollapsed ? ( 3f * MathHelper.PiOver2 ) : MathHelper.PiOver2;
                        break;
                    default:
                        throw new NotSupportedException();
                }

                Screen.Game.SpriteBatch.Draw( handleTex, new Vector2( HitBox.Center.X, HitBox.Center.Y ), null, handleColor, fHandleAngle, new Vector2( handleTex.Width / 2f, handleTex.Height / 2f ), 1f, SpriteEffects.None, 0f );
            }


            if( ! InvertDrawOrder )
            {
                if( mFirstPane != null && mbDisplayFirstPane )
                {
                    mFirstPane.Draw();
                }

                if( mSecondPane != null && mbDisplaySecondPane )
                {
                    mSecondPane.Draw();
                }
            }
            else
            {
                if( mSecondPane != null && mbDisplaySecondPane )
                {
                    mSecondPane.Draw();
                }

                if( mFirstPane != null && mbDisplayFirstPane )
                {
                    mFirstPane.Draw();
                }
            }
        }
    }
}
