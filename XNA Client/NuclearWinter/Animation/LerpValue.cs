using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace NuclearWinter.Animation
{
    public class LerpValue: AnimatedValue
    {
        //----------------------------------------------------------------------
        public LerpValue( float _fStart, float _fEnd, float _fDuration, float _fDelay, AnimationLoop _loop )
        {
            Start       = _fStart;
            End         = _fEnd;
            Duration    = _fDuration;
            Delay       = _fDelay;
            Time        = 0f;
            Loop        = _loop;
            Direction   = AnimationDirection.Forward;
        }

        public LerpValue( float _fStart, float _fEnd, float _fDuration, AnimationLoop _loop )
        :   this ( _fStart, _fEnd, _fDuration, 0f, _loop )
        {
        }

        public LerpValue( float _fStart, float _fEnd, float _fDuration, float _fDelay )
        :   this ( _fStart, _fEnd, _fDuration, _fDelay, AnimationLoop.NoLoop )
        {
        }

        public LerpValue( float _fStart, float _fEnd, float _fDuration )
        :   this ( _fStart, _fEnd, _fDuration, 0f, AnimationLoop.NoLoop )
        {
        }

        //----------------------------------------------------------------------
        public override float CurrentValue
        {
            get { return MathHelper.Lerp( Start, End, ( Time - Delay ) / Duration ); }
        }

        //----------------------------------------------------------------------
        public float    Start;
        public float    End;
    }
}
