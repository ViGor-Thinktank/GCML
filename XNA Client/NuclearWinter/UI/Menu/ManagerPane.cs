using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuclearWinter.UI
{
    public abstract class ManagerPane<T>: Group where T:IMenuManager
    {
        public T                    Manager         { get; private set; }

        //----------------------------------------------------------------------
        public ManagerPane( T _manager )
        : base( _manager.MenuScreen )
        {
            Manager     = _manager;
        }
    }
}
