using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuclearWinter;
using Microsoft.Xna.Framework.Input;
using System;
using GenericCampaignMasterModel;
using GenericCampaignMasterModel.Commands;

namespace NuclearWinter.UI
{
    public class clsXWUnitManagerPopup : Popup<IMenuManager>
    {
        public Label TitleLabel { get; private set; }
        public Label MessageLabel { get; private set; }

        public Group ContentGroup { get; private set; }

        BoxGroup mActionsGroup;

        Button mCloseButton;

        Action mCloseCallback;
        Action<object> mCommandCallback;


        //----------------------------------------------------------------------
        public clsXWUnitManagerPopup(IMenuManager _manager)
            : base(_manager)
        {
          
            TitleLabel = new Label(Screen, "", Anchor.Start);
            TitleLabel.Font = Screen.Style.MediumFont;
            TitleLabel.AnchoredRect = AnchoredRect.CreateTopAnchored(0, 0, 0, Screen.Style.DefaultButtonHeight);
            AddChild(TitleLabel);

            {
                // Message label
                ContentGroup = new Group(Screen);
                ContentGroup.AnchoredRect = AnchoredRect.CreateFull(0, 60, 0, 80);
                AddChild(ContentGroup);

                


                MessageLabel = new Label(Screen, "", Anchor.Start);
                MessageLabel.WrapText = true;
                MessageLabel.Font = Screen.Style.SmallFont;
                // Actions
                mActionsGroup = new BoxGroup(Screen, Orientation.Horizontal, 0, Anchor.End);
                mActionsGroup.AnchoredRect = AnchoredRect.CreateBottomAnchored(0, 0, 0, Screen.Style.DefaultButtonHeight);

                AddChild(mActionsGroup);

                // Close / Cancel
                mCloseButton = new Button(Screen, i18n.Common.Close);
                mCloseButton.ClickHandler = delegate { Dismiss(); };
                mCloseButton.BindPadButton(Buttons.A); //*/
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
        
        public void Setup(clsUnit aktUnit, Action<object> _commandCallback)
        {
            TitleLabel.Text = aktUnit.strBez + " ID: " + aktUnit.Id;
            MessageLabel.Text = "aktUnit.lisSubUnits";

            ContentGroup.Clear();
            ContentGroup.AddChild(MessageLabel);
            
            mCloseButton.Text = "Schließen";

            mActionsGroup.Clear();
            
            mActionsGroup.AddChild(mCloseButton);
            
            mCommandCallback = _commandCallback;
        }

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

        protected override void Dismiss()
        {
            var closeCallback = mCloseCallback;
            var confirmCallback = mCommandCallback;
            base.Dismiss();
            if (closeCallback != null) closeCallback();

        }

        protected void Confirm()
        {
            var confirmCallback = mCommandCallback;
            Close();

            object something = new object();
            confirmCallback(something);
        }
    }
}
