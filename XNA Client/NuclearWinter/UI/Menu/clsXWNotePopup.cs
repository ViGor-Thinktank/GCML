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
        public Label TitleLabel { get; private set; }
        
        public Group ContentGroup { get; private set; }

        Notebook notebook;
            

        Button mCloseButton;


        //----------------------------------------------------------------------
        public clsXWNotePopup(IMenuManager _manager)
            : base(_manager)
        {
          
            TitleLabel = new Label(Screen, "", Anchor.Start);
            TitleLabel.Font = Screen.Style.LargeFont;
            TitleLabel.AnchoredRect = AnchoredRect.CreateTopAnchored(0, 0, 0, Screen.Style.DefaultButtonHeight);

            TitleLabel.Text = "Home 1234";
            
            AddChild(TitleLabel);
            
            {
                // Message label
                ContentGroup = new Group(Screen);
                ContentGroup.AnchoredRect = AnchoredRect.CreateFull(0, 60, 0, 80);
                AddChild(ContentGroup);
                        
                // Close / Cancel
                mCloseButton = new Button(Screen, i18n.Common.Close);
                mCloseButton.ClickHandler = delegate { Dismiss(); };
                mCloseButton.BindPadButton(Buttons.A);
                //AddChild(mCloseButton); //*/

                notebook = new Notebook(Manager.MenuScreen);
                ContentGroup.AddChild(notebook);
                notebook.HasClosableTabs = true;

                NotebookTab homeTab = new NotebookTab(notebook, "Home", null);
                                notebook.Tabs.Add(homeTab);

                homeTab.PageGroup.AddChild(mCloseButton);

                homeTab = new NotebookTab(notebook, "Home2", null);
                homeTab.IsPinned = true;
                notebook.Tabs.Add(homeTab);
            }
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
            
            
            
            //mCloseButton.Text = "Schließen";*/

            }

        //----------------------------------------------------------------------
        public override void Close()
        {
            TitleLabel.Text = "";
            
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
