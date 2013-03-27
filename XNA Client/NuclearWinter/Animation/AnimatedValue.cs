using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace NuclearWinter.Animation
{
    public enum AnimationDirection
    {
        Forward,
        Backward,
        Stopped
    }

    public enum AnimationLoop
    {
        NoLoop,
        Loop,
        LoopBackAndForth
    }

    public abstract class AnimatedValue
    {
        //----------------------------------------------------------------------
        public void SetTime( float _fTotalTime )
        {
            Time = _fTotalTime;
            UpdateDirection();
        }

        //----------------------------------------------------------------------
        public void Update( float _fElapsedTime )
        {
            if( Direction == AnimationDirection.Stopped )
            {
                return;
            }

            Time += ( (Direction == AnimationDirection.Forward) ? _fElapsedTime : -_fElapsedTime );

            UpdateDirection();
        }

        public bool IsOver {
            get {
                return ( Loop == AnimationLoop.NoLoop && Time >= Delay + Duration );
            }
        }

        //----------------------------------------------------------------------
        void UpdateDirection()
        {
            if( Time >= Delay + Duration )
            {
                switch( Loop )
                {
                    case AnimationLoop.NoLoop:
                        Time = Delay + Duration;
                        break;
                    case AnimationLoop.Loop:
                        Time -= Delay + Duration;
                        break;
                    case AnimationLoop.LoopBackAndForth:
                        Time = (Delay + Duration) - ( Time - (Delay + Duration) );

                        Direction = ( Direction == AnimationDirection.Forward ) ? AnimationDirection.Backward : AnimationDirection.Forward;
                        break;
                }
            }
            else
            if( Time < 0 )
            {
                switch( Loop )
                {
                    case AnimationLoop.NoLoop:
                        Time = 0;
                        break;
                    case AnimationLoop.Loop:
                        Time += Delay + Duration;
                        break;
                    case AnimationLoop.LoopBackAndForth:
                        Time = -Time;
                        Direction = ( Direction == AnimationDirection.Forward ) ? AnimationDirection.Backward : AnimationDirection.Forward;
                        break;
                }
            }
        }
        
        //----------------------------------------------------------------------
        public abstract float CurrentValue { get; }

        //----------------------------------------------------------------------
        public AnimationDirection   Direction;
        public float                Duration;
        public float                Delay;
        public float                Time;
        public AnimationLoop        Loop;
    }
}
