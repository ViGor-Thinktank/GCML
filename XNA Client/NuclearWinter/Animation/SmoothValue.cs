using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace NuclearWinter.Animation
{
    public class SmoothValue: AnimatedValue
    {
        //----------------------------------------------------------------------
        public SmoothValue( float _fStart, float _fEnd, float _fDuration, float _fDelay, AnimationLoop _loop )
        {
            Start       = _fStart;
            End         = _fEnd;
            Duration    = _fDuration;
            Delay       = _fDelay;
            Time        = 0f;
            Loop        = _loop;
        }

        public SmoothValue( float _fStart, float _fEnd, float _fDuration, AnimationLoop _loop )
        :   this ( _fStart, _fEnd, _fDuration, 0f, _loop )
        {
        }

        public SmoothValue( float _fStart, float _fEnd, float _fDuration, float _fDelay )
        :   this ( _fStart, _fEnd, _fDuration, _fDelay, AnimationLoop.NoLoop )
        {
        }

        public SmoothValue( float _fStart, float _fEnd, float _fDuration )
        :   this ( _fStart, _fEnd, _fDuration, 0f, AnimationLoop.NoLoop )
        {
        }

        //----------------------------------------------------------------------
        public override float CurrentValue
        {
            get { return MathHelper.SmoothStep( Start, End, ( Time - Delay ) / Duration ); }
        }

        //----------------------------------------------------------------------
        public float    Start;
        public float    End;
    }
}
