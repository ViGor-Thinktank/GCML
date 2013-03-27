using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NuclearWinter.UI
{
    //--------------------------------------------------------------------------
    public enum CheckBoxState
    {
        Unchecked,
        Checked,
        Inconsistent
    }

    //--------------------------------------------------------------------------
    public class CheckBox: Widget
    {
        //----------------------------------------------------------------------
        public string Text
        {
            get { return mLabel.Text; }
            set { mLabel.Text = value; }
        }

        public CheckBoxState    CheckState;
        public Action<CheckBox,CheckBoxState> ChangeHandler;

        public Texture2D        Frame;
        public int              FrameCornerSize;
        
        //----------------------------------------------------------------------
        Label                   mLabel;
        bool                    mbIsHovered;
        Rectangle               mCheckBoxRect; 

        //----------------------------------------------------------------------
        public CheckBox( Screen _screen, string _strText )
        : base( _screen )
        {
            Frame = Screen.Style.CheckBoxFrame;
            FrameCornerSize = Screen.Style.CheckBoxFrameCornerSize;

            mLabel = new Label( Screen, _strText, Anchor.Start );
            mLabel.Padding = new Box( 10, 10, 10, 0 );

            UpdateContentSize();
        }

        //----------------------------------------------------------------------
        protected internal override void UpdateContentSize()
        {
            ContentWidth = Screen.Style.CheckBoxSize + mLabel.ContentWidth;

            base.UpdateContentSize();
        }

        //----------------------------------------------------------------------
        public override void DoLayout( Rectangle _rect )
        {
            base.DoLayout( _rect );

            mCheckBoxRect = new Rectangle( LayoutRect.X, LayoutRect.Center.Y - Screen.Style.CheckBoxSize / 2, Screen.Style.CheckBoxSize, Screen.Style.CheckBoxSize );
            mLabel.DoLayout( new Rectangle( LayoutRect.X + Screen.Style.CheckBoxSize, LayoutRect.Y, LayoutRect.Width - Screen.Style.CheckBoxSize, LayoutRect.Height ) );
            
            UpdateContentSize();
        }

        //----------------------------------------------------------------------
        public override Widget HitTest( Point _point )
        {
            return ( mCheckBoxRect.Contains( _point ) || mLabel.HitBox.Contains( _point ) ) ? this : null;
        }

        //----------------------------------------------------------------------
        public override void OnMouseMove( Point _hitPoint )
        {
            mbIsHovered = mCheckBoxRect.Contains( _hitPoint ) || mLabel.LayoutRect.Contains( _hitPoint );
        }

        //----------------------------------------------------------------------
        public override void OnMouseOut( Point _hitPoint )
        {
            mbIsHovered = false;
        }

        protected internal override void OnMouseUp( Point _hitPoint, int _iButton )
        {
            if( mbIsHovered )
            {
                CheckBoxState newState = ( CheckState == CheckBoxState.Checked ) ? CheckBoxState.Unchecked : CheckBoxState.Checked;
                if( ChangeHandler != null ) ChangeHandler( this, newState );
                CheckState = newState;
            }
        }

        //----------------------------------------------------------------------
        public override void Draw()
        {
            DrawWithOffset( Point.Zero );
        }

        //----------------------------------------------------------------------
        public void DrawWithOffset( Point _pOffset )
        {
            var rect = mCheckBoxRect;
            rect.Offset( _pOffset );
            Screen.DrawBox( Frame, rect, FrameCornerSize, Color.White );

            if( mbIsHovered )
            {
                Screen.DrawBox( Screen.Style.CheckBoxFrameHover, rect, FrameCornerSize, Color.White );
            }

            Texture2D tex;
                
            switch( CheckState )
            {
                case UI.CheckBoxState.Checked:
                    tex = Screen.Style.CheckBoxChecked;
                    break;
                case UI.CheckBoxState.Unchecked:
                    tex = Screen.Style.CheckBoxUnchecked;
                    break;
                case UI.CheckBoxState.Inconsistent:
                    tex = Screen.Style.CheckBoxInconsistent;
                    break;
                default:
                    throw new NotSupportedException();
            }

            Screen.Game.SpriteBatch.Draw( tex, new Vector2( rect.Center.X, rect.Center.Y ), null, Color.White, 0f, new Vector2( tex.Width, tex.Height ) / 2f, 1f, SpriteEffects.None, 1f );

            mLabel.DrawWithOffset( _pOffset );
        }
    }
}
