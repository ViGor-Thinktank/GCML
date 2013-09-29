using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuclearWinter;
using Microsoft.Xna.Framework.Input;
using System;
using GenericCampaignMasterModel;
using GenericCampaignMasterModel.Commands;
using System.Collections.Generic;
using NuclearWinter.UI;

namespace NuclearWinter.UI.GCML
{
    public abstract class clsXWCommandPopupBase : NuclearWinter.UI.Popup<IMenuManager>
    {
        public event delUnitWasDestroyd onUnitWasDestroyd;
        public delegate void delUnitWasDestroyd(clsUnit objUnit);

        //----------------------------------------------------------------------
        public clsXWCommandPopupBase(IMenuManager _manager)
            : base(_manager)
        {
       
                    
        }

        //----------------------------------------------------------------------
        public void Open(int _iWidth, int _iHeight)
        {
            AnchoredRect.Width = _iWidth;
            AnchoredRect.Height = _iHeight;

            Manager.PushPopup(this);
            Screen.Focus(GetFirstFocusableDescendant(Direction.Down));


        }

        public abstract void Setup(clsUnit aktUnit, Action<ICommand, clsCommandCollection> _commandCallback);



        public void raiseUnitWasDestroyd(clsUnit aktUnit)
        {
            onUnitWasDestroyd(aktUnit);
        }
    }
}
