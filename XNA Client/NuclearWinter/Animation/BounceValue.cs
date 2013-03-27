using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace NuclearWinter.Animation
{
    public class BounceValue: AnimatedValue
    {
        //----------------------------------------------------------------------
        public BounceValue( float _fStart, float _fEnd, float _fDuration, int _iBounceCount, float _fDelay, AnimationLoop _loop )
        {
            Start       = _fStart;
            End         = _fEnd;
            Duration    = _fDuration;
            BounceCount = _iBounceCount;
            Delay       = _fDelay;
            Time        = 0f;
            Loop        = _loop;
        }

        public BounceValue( float _fStart, float _fEnd, float _fDuration, int _iBounceCount, AnimationLoop _loop )
        :   this ( _fStart, _fEnd, _fDuration, _iBounceCount, 0f, _loop )
        {
        }

        public BounceValue( float _fStart, float _fEnd, float _fDuration, int _iBounceCount, float _fDelay )
        :   this ( _fStart, _fEnd, _fDuration, _iBounceCount, _fDelay, AnimationLoop.NoLoop )
        {
        }

        public BounceValue( float _fStart, float _fEnd, float _fDuration, int _iBounceCount )
        :   this ( _fStart, _fEnd, _fDuration, _iBounceCount, 0f, AnimationLoop.NoLoop )
        {
        }

        //----------------------------------------------------------------------
        public override float CurrentValue
        {
            get {
                // This might be overly complicated.
                // But it (kind of) works and I don't care enough to fix it.
                if( Time <= Delay )
                {
                    return Start;
                }

                float fProgress = (Time - Delay) / Duration;

                float fBounceInterval = 1f / BounceCount;
                fProgress += ( fBounceInterval / 2f );

                float fBounceNumber = (int)( fProgress * BounceCount );
                float fCurrentBounceProgress = (fProgress - fBounceNumber * fBounceInterval) / fBounceInterval;
                
                if( fCurrentBounceProgress > 0.5f )
                {
                    fCurrentBounceProgress = 1f - fCurrentBounceProgress;
                }
                fCurrentBounceProgress = 1f - ( 1f - fCurrentBounceProgress ) * ( 1f - fCurrentBounceProgress );
                
                float fBounceAmplitude = (float)Math.Pow( (float)fBounceNumber / BounceCount, BounceRestitution );
                float fValue = 1f - ( 1f - fBounceAmplitude ) * fCurrentBounceProgress;

                return Start + fValue * (End - Start);
            }
        }

        //----------------------------------------------------------------------
        public float    Start;
        public float    End;
        public int      BounceCount;
        public float    BounceRestitution = 0.5f;
    }
}
