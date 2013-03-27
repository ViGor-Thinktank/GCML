#if WINDOWS_PHONE
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace NuclearWinter.Input
{
    public class TouchInputManager: GameComponent
    {
        //----------------------------------------------------------------------
        public TouchInputManager( Game _game )
        : base ( _game )
        {

        }

        //----------------------------------------------------------------------
        public override void Update( GameTime _time )
        {
            Touches = TouchPanel.GetState();
        }

        //----------------------------------------------------------------------
        public TouchCollection     Touches { get; private set; }
    }
}
#endif