using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuclearWinter;
using Microsoft.Xna.Framework.Input;
using System;
using GenericCampaignMasterModel;
using GenericCampaignMasterModel.Commands;

namespace NuclearWinter.UI
{
    public class clsXWCommandPopup : Popup<IMenuManager>
    {
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
            TitleLabel.Font = Screen.Style.LargeFont;
            TitleLabel.AnchoredRect = AnchoredRect.CreateTopAnchored(0, 0, 0, Screen.Style.DefaultButtonHeight);
            AddChild(TitleLabel);

            {
                // Message label
                ContentGroup = new Group(Screen);
                ContentGroup.AnchoredRect = AnchoredRect.CreateFull(0, 60, 0, 80);
                AddChild(ContentGroup);

                MessageLabel = new Label(Screen, "", Anchor.Start);
                MessageLabel.WrapText = true;

                // Actions
                mActionsGroup = new BoxGroup(Screen, Orientation.Horizontal, 0, Anchor.End);
                mActionsGroup.AnchoredRect = AnchoredRect.CreateBottomAnchored(0, 0, 0, Screen.Style.DefaultButtonHeight);

                AddChild(mActionsGroup);

                // Close / Cancel
                mCloseButton = new Button(Screen, i18n.Common.Close);
                mCloseButton.ClickHandler = delegate { Dismiss(); };
                mCloseButton.BindPadButton(Buttons.A);
            }
        }

        //----------------------------------------------------------------------
        public void Open(int _iWidth, int _iHeight)
        {
            AnchoredRect.Width = _iWidth;
            AnchoredRect.Height = _iHeight;

            Manager.PushPopup(this);
            Screen.Focus(GetFirstFocusableDescendant(Direction.Down));


        }
        private void imgCommandIcon_ClickHandler(Button sender)
        {
            this.Confirm((ICommand)sender.Tag);
        }

        private clsCommandCollection m_objCommandCollection;

        //----------------------------------------------------------------------
        public void Setup(clsCommandCollection objCommandCollection, Action<ICommand, clsCommandCollection> _commandCallback)
        {
            m_objCommandCollection = objCommandCollection;

            TitleLabel.Text = objCommandCollection.aktUnit.strBez + " ID: " + objCommandCollection.aktUnit.Id;
            MessageLabel.Text = objCommandCollection.aktUnit.strDescription;

            ContentGroup.Clear();
            ContentGroup.AddChild(MessageLabel);

            mCloseButton.Text = "Schließen";

            mActionsGroup.Clear();

            foreach (ICommand aktCommandType in objCommandCollection.listRawCommands)
            {
                Button mCommandButton = new Button(Screen);
                mCommandButton.Text = aktCommandType.strTypeName;
                mCommandButton.Tag = aktCommandType;
                mCommandButton.ClickHandler = new Action<Button>(imgCommandIcon_ClickHandler);
                mActionsGroup.AddChild(mCommandButton);
            }

            //immer
            mActionsGroup.AddChild(mCloseButton);

            mCommandCallback = _commandCallback;
        }

        //----------------------------------------------------------------------
        public override void Close()
        {
            TitleLabel.Text = "";
            MessageLabel.Text = "";

            mCloseButton.Text = i18n.Common.Close;

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
