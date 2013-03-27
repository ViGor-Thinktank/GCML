using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NuclearWinter.UI
{
    /*
     * A Image to be displayed
     */
    public class Image: Widget
    {
        //GMCL specific
        public float fltRotation = 0;

        protected bool mbStretch;
        public bool Stretch {
            get { return mbStretch; }
            set { mbStretch = value; }
        }

        public Color Color = Color.White;

        public Action<Image>    ClickHandler;
        public Action<Image>    MouseEnterHandler;
        public Action<Image>    MouseOutHandler;
        public Action<Image>    MouseDownHandler;
        public Action<Image,float>  UpdateHandler;

        //----------------------------------------------------------------------
        protected Texture2D mTexture;
        public Texture2D    Texture {
            get { return mTexture; }
            set {
                mTexture = value;
                UpdateContentSize();
            }
        }

        //----------------------------------------------------------------------
        public override Widget GetFirstFocusableDescendant( Direction _direction )
        {
            return null;
        }

        //----------------------------------------------------------------------
        protected internal override void UpdateContentSize()
        {
            int iWidth = mTexture != null ? mTexture.Width : 0;
            int iHeight = mTexture != null ? mTexture.Height : 0;
            
            ContentWidth    = iWidth + Padding.Horizontal;
            ContentHeight   = iHeight + Padding.Vertical;

            base.UpdateContentSize();
        }

        //----------------------------------------------------------------------
        public Image( Screen _screen, Texture2D _texture = null, bool _bStretch = false )
        : base( _screen )
        {
            mTexture = _texture;
            mbStretch = _bStretch;
            UpdateContentSize();
        }

        //----------------------------------------------------------------------
        public override Widget HitTest( Point _point )
        {
            return ClickHandler != null ? base.HitTest( _point ) : null;
        }

        public override void OnMouseEnter( Point _hitPoint )
        {
            if( ClickHandler != null )
            {
#if !MONOGAME
                Screen.Game.Form.Cursor = System.Windows.Forms.Cursors.Hand;
#endif
            }

            if( MouseEnterHandler != null ) MouseEnterHandler( this );
        }

        public override void OnMouseOut( Point _hitPoint )
        {
            if( ClickHandler != null )
            {
#if !MONOGAME
                Screen.Game.Form.Cursor = System.Windows.Forms.Cursors.Default;
#endif
            }

            if( MouseOutHandler != null ) MouseOutHandler( this );
        }

        protected internal override bool OnMouseDown( Point _hitPoint, int _iButton )
        {
            if( MouseDownHandler != null )
            {
                MouseDownHandler( this );
                return true;
            }

            return ClickHandler != null;
        }

        protected internal override void OnMouseUp(Point _hitPoint, int _iButton)
        {
            if( _iButton != Screen.Game.InputMgr.PrimaryMouseButton ) return;

            if( ClickHandler != null )
            {
                ClickHandler( this );
            }
        }

        public override void Update( float _fElapsedTime )
        {
            if( UpdateHandler != null )
            {
                UpdateHandler( this, _fElapsedTime );
            }

            base.Update( _fElapsedTime );
        }

        //----------------------------------------------------------------------
        public override void DoLayout( Rectangle _rect )
        {
            base.DoLayout( _rect );

            Point pCenter = LayoutRect.Center;

            HitBox = new Rectangle(
                pCenter.X - ContentWidth / 2,
                pCenter.Y - ContentHeight / 2,
                ContentWidth,
                ContentHeight
            );
        }

        //----------------------------------------------------------------------
        public override void Draw()
        {
            if( mTexture == null ) return;

            if (false && fltRotation != 0)
            {
                Rectangle rec = new Rectangle(LayoutRect.X + Padding.Left, LayoutRect.Y + Padding.Top, LayoutRect.Width - Padding.Horizontal, LayoutRect.Height - Padding.Vertical);
                Vector2 vec = new Vector2(LayoutRect.Center.X - ContentWidth / 2 + Padding.Left, LayoutRect.Center.Y - ContentHeight / 2 + Padding.Top);
                //Screen.Game.SpriteBatch.Draw(mTexture, rec,null, Color, this.fltRotation, vec, SpriteEffects.None, 0);
                vec = new Vector2(340, 150);
                rec = new Rectangle(150, 150, 50, 50);
                fltRotation = 45;
                //Screen.Game.SpriteBatch.Draw(mTexture, rec, null, Color, this.fltRotation, new Vector2(LayoutRect.Center.X - ContentWidth / 2 + Padding.Left, LayoutRect.Center.Y - ContentHeight / 2 + Padding.Top), SpriteEffects.None, 1);
                
                Screen.Game.SpriteBatch.Draw(mTexture, rec, null, Color, this.fltRotation, vec, SpriteEffects.None, 1);
            
            }
            else
            {
                if (!mbStretch)
                {
                    Screen.Game.SpriteBatch.Draw(mTexture, new Vector2(LayoutRect.Center.X - ContentWidth / 2 + Padding.Left, LayoutRect.Center.Y - ContentHeight / 2 + Padding.Top), Color);
                }
                else
                {
                    Screen.Game.SpriteBatch.Draw(mTexture, new Rectangle(LayoutRect.X + Padding.Left, LayoutRect.Y + Padding.Top, LayoutRect.Width - Padding.Horizontal, LayoutRect.Height - Padding.Vertical), Color);
                }
            }
        }
    }
}
