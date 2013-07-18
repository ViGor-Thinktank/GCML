using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuclearWinter;
using Microsoft.Xna.Framework.Input;
using System;
using GenericCampaignMasterModel;
using GenericCampaignMasterModel.Commands;

namespace NuclearWinter.UI
{
    public class clsXWNotePopup : Popup<IMenuManager>
    {
        
        Button mCloseButton;


        //----------------------------------------------------------------------
        public clsXWNotePopup(IMenuManager _manager)
            : base(_manager)
        {
                        
            // Close / Cancel
            mCloseButton = new Button(Screen, "alpha2");
            mCloseButton.ClickHandler = delegate { Dismiss(); };
               
         

           
        }

        //----------------------------------------------------------------------
        public void Open(int _iWidth, int _iHeight)
        {
            AnchoredRect.Width = _iWidth;
            AnchoredRect.Height = _iHeight;

            Manager.PushPopup(this);
          

        }
        private void imgCommandIcon_ClickHandler(Button sender)
        {
            this.Confirm((ICommand)sender.Tag);
        }

        private clsCommandCollection m_objCommandCollection;

        //----------------------------------------------------------------------
        public void Setup(clsCommandCollection objCommandCollection)
        {
            m_objCommandCollection = objCommandCollection;

           // TitleLabel.Text = objCommandCollection.aktUnit.strBez + " ID: " + objCommandCollection.aktUnit.Id;

            Notebook notebook = new Notebook(Manager.MenuScreen);
            AddChild(notebook);

            notebook.HasClosableTabs = true;

            NotebookTab homeTab = new NotebookTab(notebook, "Home", null);
            homeTab.IsPinned = true;
            notebook.Tabs.Add(homeTab);

            Button createTab = new Button(Manager.MenuScreen, "Create tab");
            createTab.AnchoredRect = AnchoredRect.CreateFull(10);

            int iTabCounter = 0;
            createTab.ClickHandler = delegate
            {
                NotebookTab tab = new NotebookTab(notebook, string.Format("Tab {0}", ++iTabCounter), null);

                notebook.Tabs.Add(tab);
            };

            homeTab.PageGroup.AddChild(createTab);
            
            //mCloseButton.Text = "Schließen";*/

            }

        //----------------------------------------------------------------------
        public override void Close()
        {
            
            mCloseButton.Text = i18n.Common.Close;

            //ContentGroup.Clear();

            base.Close();
        }

        //----------------------------------------------------------------------
        protected override void Dismiss()
        {
            base.Dismiss();
  

        }

        //----------------------------------------------------------------------
        protected void Confirm(ICommand chosenCommand)
        {
            Close();

           
        }
    }
}
