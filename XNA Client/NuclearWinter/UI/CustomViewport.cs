using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NuclearWinter.UI
{
    public abstract class CustomViewport: Widget
    {
        //----------------------------------------------------------------------
        public CustomViewport( Screen _screen )
        : base( _screen )
        {
        }

        //----------------------------------------------------------------------
        public override void DoLayout( Rectangle _rect )
        {
            base.DoLayout( _rect );
            HitBox = LayoutRect;
        }

        //----------------------------------------------------------------------
        Viewport mPreviousViewport;

        protected internal virtual void BeginDraw()
        {
            Screen.SuspendBatch();
            mPreviousViewport = Screen.Game.GraphicsDevice.Viewport;

            Viewport viewport = new Viewport( LayoutRect );
            Screen.Game.GraphicsDevice.Viewport = viewport;
        }

        //----------------------------------------------------------------------
        protected internal virtual void EndDraw()
        {
            Screen.Game.GraphicsDevice.Viewport = mPreviousViewport;
            Screen.ResumeBatch();
        }
    }
}
