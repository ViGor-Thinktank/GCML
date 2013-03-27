using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NuclearWinter.Xna
{
    public static class XnaExtensions
    {
        public static Rectangle Clip( this Rectangle _rect1, Rectangle _rect2 )
        {
            Rectangle rect = new Rectangle(
                Math.Max( _rect1.X, _rect2.X ),
                Math.Max( _rect1.Y, _rect2.Y ),
                0, 0 );

            rect.Width = Math.Min( _rect1.Right, _rect2.Right ) - rect.X;
            rect.Height = Math.Min( _rect1.Bottom, _rect2.Bottom ) - rect.Y;

            if( rect.Width < 0 )
            {
                rect.X += rect.Width;
                rect.Width = 0;
            }

            if( rect.Height < 0 )
            {
                rect.Y += rect.Height;
                rect.Height = 0;
            }

            return rect;
        }

        public static Vector2 ToVector2( this Point _point )
        {
            return new Vector2( _point.X, _point.Y );
        }
    }
}
