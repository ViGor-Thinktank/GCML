using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace NuclearWinter.UI
{
    //--------------------------------------------------------------------------
    public interface IMenuManager
    {
        //----------------------------------------------------------------------
        ContentManager      Content         { get; }

        //----------------------------------------------------------------------
        Screen              MenuScreen      { get; }
        Screen              PopupScreen     { get; }

        void PushPopup( Panel _popup );
        void PopPopup( Panel _popup );
    }
}
