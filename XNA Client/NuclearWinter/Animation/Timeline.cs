using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuclearWinter.Animation
{
    public class TimelineEvent
    {
        public TimelineEvent( float _fTime, Action _action )
        {
            Time        = _fTime;
            Action      = _action;
        }

        public float        Time;
        public Action       Action;
    }

    public class Timeline
    {
        public Timeline()
        {
            mlEvents = new List<TimelineEvent>();
        }

        public void AddEvent( TimelineEvent _event )
        {
            mlEvents.Add( _event );

            mlEvents.Sort( delegate( TimelineEvent _a, TimelineEvent _b ) { return _a.Time.CompareTo( _b.Time ); } );
        }

        public void Update( float _fElapsedTime )
        {
            Time += _fElapsedTime;

            while( miEventOffset < mlEvents.Count && mlEvents[ miEventOffset ].Time <= Time )
            {
                // NOTE: We must increment miEventOffset before calling the
                // Action, in case the Action calls Reset() or does any other
                // change to the timeline
                miEventOffset++;

                mlEvents[ miEventOffset - 1 ].Action();
            }
        }

        public void Reset()
        {
            Time = 0f;
            miEventOffset = 0;
        }

        public float            Time;

        /// Time-sorted list of events
        List<TimelineEvent>     mlEvents;
        int                     miEventOffset;
    }
}
