using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NuclearWinter
{
    public struct AABB
    {
        //----------------------------------------------------------------------
        public Vector2      Min;
        public Vector2      Max;

        //----------------------------------------------------------------------
        public AABB( Vector2 _min, Vector2 _max )
        {
            Min = _min;
            Max = _max;
        }

        //----------------------------------------------------------------------
        public void Extend( Vector2 _vPoint )
        {
            Min.X = Math.Min( _vPoint.X, Min.X );
            Min.Y = Math.Min( _vPoint.Y, Min.Y );

            Max.X = Math.Max( _vPoint.X, Max.X );
            Max.Y = Math.Max( _vPoint.Y, Max.Y );
        }

        //----------------------------------------------------------------------
        public bool Intersects( AABB _aabb )
        {
            return
                ( ( Min.X <= _aabb.Max.X ) || ( Max.X >= _aabb.Min.X ) )
                &&
                ( ( Min.Y <= _aabb.Max.Y ) || ( Max.Y >= _aabb.Min.Y ) );
        }

        //----------------------------------------------------------------------
        public bool Contains( AABB _aabb )
        {
            return
                ( Min.X <= _aabb.Min.X && Max.X >= _aabb.Max.X )
                &&
                ( Min.Y <= _aabb.Min.Y && Max.Y >= _aabb.Max.Y );
        }

        //----------------------------------------------------------------------
        public bool Contains( Vector2 _vPoint )
        {
            return 
                ( Min.X <= _vPoint.X && Max.X >= _vPoint.X )
                && 
                ( Min.Y <= _vPoint.Y && Max.Y >= _vPoint.Y );
        }

        //----------------------------------------------------------------------
        public bool Contains( Vector2 _vPoint, float _fAxisDistance )
        {
            return
                ( _vPoint.X >= Min.X    - _fAxisDistance )
            &&  ( _vPoint.X <= Max.X    + _fAxisDistance )
            &&  ( _vPoint.Y >= Min.Y    - _fAxisDistance )
            &&  ( _vPoint.Y <= Max.Y    + _fAxisDistance );
        }

        //----------------------------------------------------------------------
        public float Width
        {
            get
            {
                return Math.Abs( Max.X - Min.X );
            }
        }

        //----------------------------------------------------------------------
        public float Height 
        {
            get
            {
                return Math.Abs( Max.Y - Min.Y );
            }
        }

        public Vector2 Center
        {
            get
            {
                return ( Min + Max ) / 2f;
            }
        }
    }
}
