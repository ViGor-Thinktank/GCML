using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NuclearUI = NuclearWinter.UI;
using Microsoft.Xna.Framework.Graphics;

namespace NuclearSample.Demos
{
    class TextAreaPane: NuclearUI.ManagerPane<MainMenuManager>
    {
        //----------------------------------------------------------------------
        public TextAreaPane( MainMenuManager _manager )
        : base( _manager )
        {
            NuclearUI.TextArea textArea = new NuclearUI.TextArea( Manager.MenuScreen );
            textArea.Font = new NuclearUI.UIFont( Manager.Content.Load<SpriteFont>( "Fonts/MediumMonoFont" ) );
            textArea.DisplayLineNumbers = true;

            AddChild( textArea );
        }

        //----------------------------------------------------------------------
    }
}
