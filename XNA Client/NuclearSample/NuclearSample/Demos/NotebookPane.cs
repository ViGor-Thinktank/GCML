using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NuclearUI = NuclearWinter.UI;

namespace GCML_XNA_Client.Demos
{
    class NotebookPane: NuclearUI.ManagerPane<MainMenuManager>
    {
        //----------------------------------------------------------------------
        public NotebookPane( MainMenuManager _manager )
        : base( _manager )
        {
            //------------------------------------------------------------------
            NuclearUI.Notebook notebook = new NuclearUI.Notebook( Manager.MenuScreen );
            AddChild( notebook );

            notebook.HasClosableTabs  = true;

            NuclearUI.NotebookTab homeTab = new NuclearUI.NotebookTab( notebook, "Home", null );
            homeTab.IsPinned = true;
            notebook.Tabs.Add( homeTab );

            NuclearUI.Button createTab = new NuclearUI.Button( Manager.MenuScreen, "Create tab" );
            createTab.AnchoredRect = NuclearUI.AnchoredRect.CreateFull( 10 );

            int iTabCounter = 0;
            createTab.ClickHandler = delegate {
                NuclearUI.NotebookTab tab = new NuclearUI.NotebookTab( notebook, string.Format( "Tab {0}", ++iTabCounter ), null );

                notebook.Tabs.Add( tab );
            };

            homeTab.PageGroup.AddChild( createTab );
        }

        //----------------------------------------------------------------------
    }
}
