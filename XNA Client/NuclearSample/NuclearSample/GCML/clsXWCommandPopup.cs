using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuclearWinter;
using Microsoft.Xna.Framework.Input;
using System;
using GenericCampaignMasterModel;
using GenericCampaignMasterModel.Commands;
using System.Collections.Generic;
using NuclearWinter.UI;

namespace GCML_XNA_Client.GCML
{
    public class clsXWCommandPopup : NuclearWinter.UI.GCML.clsXWCommandPopupBase 
    {
        private clsUnit m_aktUnit;

        public Label TitleLabel { get; private set; }
        public Label MessageLabel { get; private set; }

        public Group ContentGroup { get; private set; }

        BoxGroup mActionsGroup;

        Button mCloseButton;

        Action mCloseCallback;
        Action<ICommand, clsCommandCollection> mCommandCallback;

        //----------------------------------------------------------------------
        public clsXWCommandPopup(IMenuManager _manager)
            : base(_manager)
        {
            TitleLabel = new Label(Screen, "", Anchor.Start);
            TitleLabel.Font = Screen.Style.MediumFont;
            TitleLabel.AnchoredRect = AnchoredRect.CreateTopAnchored(0, 0, 0, Screen.Style.DefaultButtonHeight);

            AddChild(TitleLabel);

            {
                ContentGroup = new Group(Screen);
                ContentGroup.AnchoredRect = AnchoredRect.CreateFull(0, 60, 0, 80);
                AddChild(ContentGroup);

                MessageLabel = new Label(Screen, "MessageLabel", Anchor.Start);
                MessageLabel.WrapText = true;
                MessageLabel.Font = Screen.Style.SmallFont;

                // Actions
                mActionsGroup = new BoxGroup(Screen, Orientation.Horizontal, 0, Anchor.End);
                mActionsGroup.AnchoredRect = AnchoredRect.CreateBottomAnchored(0, 0, 0, Screen.Style.DefaultButtonHeight);

                AddChild(mActionsGroup);

                // Close / Cancel
                mCloseButton = new Button(Screen, NuclearWinter.i18n.Common.Close);
                mCloseButton.ClickHandler = delegate { Dismiss(); };
                mCloseButton.BindPadButton(Buttons.A); //*/
            }
                    
        }

        
        private void imgCommandIcon_ClickHandler(Button sender)
        {
            this.Confirm((ICommand)sender.Tag);
        }

        private clsCommandCollection m_objCommandCollection;
        private GridGroup m_gridSubUnitRooster;

        private void entfernen_ClickHandler(Button sender)
        {
            int id = ((clsSubUnit)sender.Tag).ID;

            Program.m_objCampaign.Unit_RemoveSubunit(this.m_objCommandCollection.aktUnit.strOwnerID, this.m_objCommandCollection.aktUnit.Id, id);
            
            if (m_objCommandCollection.aktUnit.lisSubUnits.Count == 0)
            {
                Program.m_objCampaign.Unit_Remove(this.m_objCommandCollection.aktUnit.strOwnerID, this.m_objCommandCollection.aktUnit.Id);
                this.Close();
                base.raiseUnitWasDestroyd(this.m_objCommandCollection.aktUnit);
                
            }
            Refresh();
        }

        public override void Setup(clsUnit aktUnit, Action<ICommand, clsCommandCollection> _commandCallback)
        {
            m_aktUnit = aktUnit;
            m_objCommandCollection = Program.m_objCampaign.Unit_getCommandsForUnit(aktUnit);
            mCommandCallback = _commandCallback;

            Refresh();
        }
        
        public void Refresh()
        {
            mActionsGroup.Clear();

            TitleLabel.Text = m_aktUnit.strBez + " ID " + m_aktUnit.Id;

            ContentGroup.Clear();

            List<clsSubUnit> lisSubUnits = m_aktUnit.lisSubUnits;
            m_gridSubUnitRooster = new GridGroup(Manager.MenuScreen, 5, lisSubUnits.Count, false, 0);
            ContentGroup.AddChild(m_gridSubUnitRooster);
            
            for (int i = 0; i < lisSubUnits.Count; i++)
            {
                Image imgBtn = new Image(Manager.MenuScreen, base.Manager.Content.Load<Texture2D>("Sprites/" + lisSubUnits[i].objUnitType.strIconName));
                m_gridSubUnitRooster.AddChildAt(imgBtn, 0, i);

                Label subLabel = new Label(Manager.MenuScreen);
                subLabel.Font = Screen.Style.SmallFont;
                subLabel.Text = lisSubUnits[i].objUnitType.strBez;
                m_gridSubUnitRooster.AddChildAt(subLabel, 1, i);

                subLabel = new Label(Manager.MenuScreen);
                subLabel.Font = Screen.Style.SmallFont;
                subLabel.Text = "Move: " + lisSubUnits[i].objUnitType.intMovement.ToString();
                m_gridSubUnitRooster.AddChildAt(subLabel, 2, i);

                subLabel = new Label(Manager.MenuScreen);
                subLabel.Font = Screen.Style.SmallFont;
                subLabel.Text = "See: " + lisSubUnits[i].objUnitType.intSichtweite.ToString();
                m_gridSubUnitRooster.AddChildAt(subLabel, 3, i);

                BoxGroup testGroup = new BoxGroup(Screen, Orientation.Vertical, 0, Anchor.Center);
                m_gridSubUnitRooster.AddChildAt(testGroup, 4, i);

                //add Roosterrow
                Button mCommandButton = new Button(Screen);
                mCommandButton.Text = "Destroy " + lisSubUnits[i].ID.ToString();
                mCommandButton.Tag = lisSubUnits[i];
                mCommandButton.ClickHandler = new Action<Button>(entfernen_ClickHandler);
                testGroup.AddChild(mCommandButton, false);

            }


            mCloseButton.Text = "Schließen";


            foreach (ICommand aktCommandType in m_objCommandCollection.listRawCommands)
            {
                Button mCommandButton = new Button(Screen);
                mCommandButton.Text = aktCommandType.strTypeName;
                mCommandButton.Tag = aktCommandType;
                mCommandButton.ClickHandler = new Action<Button>(imgCommandIcon_ClickHandler);
                mActionsGroup.AddChild(mCommandButton);
            }


            mActionsGroup.AddChild(mCloseButton);

            
        }

        //----------------------------------------------------------------------
        public override void Close()
        {
            TitleLabel.Text = "";
            MessageLabel.Text = "";

            mCloseButton.Text = NuclearWinter.i18n.Common.Close;

            mCloseCallback = null;
            mCommandCallback = null;

            ContentGroup.Clear();

            base.Close();
        }

        //----------------------------------------------------------------------
        protected override void Dismiss()
        {
            var closeCallback = mCloseCallback;
            var confirmCallback = mCommandCallback;
            base.Dismiss();
            if (closeCallback != null) closeCallback();

        }

        //----------------------------------------------------------------------
        protected void Confirm(ICommand chosenCommand)
        {
            var confirmCallback = mCommandCallback;
            Close();

            confirmCallback(chosenCommand, m_objCommandCollection);
        }
    }
}
