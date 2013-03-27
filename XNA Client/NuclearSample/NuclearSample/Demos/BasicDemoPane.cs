using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NuclearUI = NuclearWinter.UI;

namespace NuclearSample.Demos
{
    class BasicDemoPane: NuclearUI.ManagerPane<MainMenuManager>
    {
        enum Flavor
        {
            Chocolate,
            Vanilla,
            Cheese
        }

        //----------------------------------------------------------------------
        public BasicDemoPane( MainMenuManager _manager )
        : base( _manager )
        {
            int iRows = 3;

            NuclearUI.GridGroup gridGroup = new NuclearUI.GridGroup( Manager.MenuScreen, 2, iRows, false, 0 );
            gridGroup.AnchoredRect = NuclearUI.AnchoredRect.CreateTopLeftAnchored( 0, 0, 400, iRows * 50 );
            AddChild( gridGroup );

            int iRowIndex = 0;

            //------------------------------------------------------------------
            gridGroup.AddChildAt( new NuclearUI.Label( Manager.MenuScreen, "Select Flavor", NuclearUI.Anchor.Start ), 0, iRowIndex );

            {

                List<NuclearUI.DropDownItem> lItems = new List<NuclearUI.DropDownItem>();
                lItems.Add( new NuclearUI.DropDownItem( Manager.MenuScreen, "Chocolate", Flavor.Chocolate ) );
                lItems.Add( new NuclearUI.DropDownItem( Manager.MenuScreen, "Vanilla", Flavor.Vanilla ) );
                lItems.Add( new NuclearUI.DropDownItem( Manager.MenuScreen, "Cheese", Flavor.Cheese ) );

                NuclearUI.DropDownBox dropDownBox = new NuclearUI.DropDownBox( Manager.MenuScreen, lItems, 0 );
                gridGroup.AddChildAt( dropDownBox, 1, iRowIndex );
            }

            iRowIndex++;

            //------------------------------------------------------------------
            gridGroup.AddChildAt( new NuclearUI.Label( Manager.MenuScreen, "Choose Cone Size", NuclearUI.Anchor.Start ), 0, iRowIndex );

            {
                NuclearUI.Slider sizeSlider = new NuclearUI.Slider( Manager.MenuScreen, 1, 5, 1, 1 );
                gridGroup.AddChildAt( sizeSlider, 1, iRowIndex );
            }

            iRowIndex++;

            //------------------------------------------------------------------
            gridGroup.AddChildAt( new NuclearUI.Label( Manager.MenuScreen, "Clicky clicky", NuclearUI.Anchor.Start ), 0, iRowIndex );

            {
                NuclearUI.Button button = new NuclearUI.Button( Manager.MenuScreen, "Get Ice Cream!" );
                button.ClickHandler = delegate {
                    Manager.MessagePopup.Setup( "Oh noes!", "It melted already. Sorry.", NuclearWinter.i18n.Common.Close, false );
                    Manager.MessagePopup.Open( 600, 250 );
                };
                gridGroup.AddChildAt( button, 1, iRowIndex );
            }

            iRowIndex++;

            //------------------------------------------------------------------
            
        }

        //----------------------------------------------------------------------
    }
}
