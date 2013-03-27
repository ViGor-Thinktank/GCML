using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuclearWinter;
using Microsoft.Xna.Framework.Input;
using System;

namespace NuclearWinter.UI
{
    public class MessagePopup: Popup<IMenuManager>
    {
        public Label                TitleLabel      { get; private set; }
        public Label                MessageLabel    { get; private set; }

        public Group                ContentGroup    { get; private set; }

        BoxGroup                    mActionsGroup;

        Button                      mCloseButton;
        Button                      mConfirmButton;

        Action                      mCloseCallback;
        Action<bool>                mConfirmCallback;

        SpinningWheel               mSpinningWheel;

        public bool ShowSpinningWheel {
            set
            {
                if( value )
                {
                    if( mSpinningWheel.Parent == null )
                    {
                        AddChild( mSpinningWheel );
                    }
                }
                else
                {
                    if( mSpinningWheel.Parent != null )
                    {
                        RemoveChild( mSpinningWheel );
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        public MessagePopup( IMenuManager _manager )
        : base( _manager )
        {
            TitleLabel = new Label( Screen, "", Anchor.Start );
            TitleLabel.Font = Screen.Style.LargeFont;
            TitleLabel.AnchoredRect = AnchoredRect.CreateTopAnchored( 0, 0, 0, Screen.Style.DefaultButtonHeight );
            AddChild( TitleLabel );

            {
                mSpinningWheel = new SpinningWheel( Screen, Screen.Style.SpinningWheel );
                mSpinningWheel.AnchoredRect = AnchoredRect.CreateCentered( mSpinningWheel.ContentWidth, mSpinningWheel.ContentHeight );

                // Message label
                ContentGroup = new Group( Screen );
                ContentGroup.AnchoredRect = AnchoredRect.CreateFull( 0, 60, 0, 80 );
                AddChild( ContentGroup );

                MessageLabel = new Label( Screen, "", Anchor.Start );
                MessageLabel.WrapText = true;

                // Actions
                mActionsGroup = new BoxGroup( Screen, Orientation.Horizontal, 0, Anchor.End );
                mActionsGroup.AnchoredRect = AnchoredRect.CreateBottomAnchored( 0, 0, 0, Screen.Style.DefaultButtonHeight );

                AddChild( mActionsGroup );

                // Close / Cancel
                mCloseButton = new Button( Screen, i18n.Common.Close );
                mCloseButton.ClickHandler = delegate { Dismiss(); };
                mCloseButton.BindPadButton( Buttons.A );

                // Confirm
                mConfirmButton = new Button( Screen, i18n.Common.Confirm );
                mConfirmButton.ClickHandler = delegate { Confirm(); };
                mActionsGroup.AddChild( mConfirmButton );
            }
        }

        //----------------------------------------------------------------------
        public void Open( int _iWidth, int _iHeight )
        {
            AnchoredRect.Width = _iWidth;
            AnchoredRect.Height = _iHeight;

            Manager.PushPopup( this );
            Screen.Focus( GetFirstFocusableDescendant( Direction.Down ) );

            mSpinningWheel.Reset();
        }

        //----------------------------------------------------------------------
        public void Setup( string _strTitleText, string _strMessageText, string _strCloseButtonCaption, bool _bShowSpinningWheel=false, Action _closeCallback=null )
        {
            TitleLabel.Text     = _strTitleText;

            if( _strMessageText != null )
            {
                MessageLabel.Text   = _strMessageText;
                ContentGroup.Clear();
                ContentGroup.AddChild( MessageLabel );
            }

            mCloseButton.Text   = _strCloseButtonCaption;
            ShowSpinningWheel   = _bShowSpinningWheel;

            mActionsGroup.Clear();
            mActionsGroup.AddChild( mCloseButton );
            mCloseCallback = _closeCallback;
        }

        //----------------------------------------------------------------------
        public void Setup( string _strTitleText, string _strMessageText, string _strConfirmButtonCaption, string _strCloseButtonCaption, Action<bool> _confirmCallback=null )
        {
            TitleLabel.Text     = _strTitleText;

            if( _strMessageText != null )
            {
                MessageLabel.Text   = _strMessageText;
                ContentGroup.Clear();
                ContentGroup.AddChild( MessageLabel );
            }

            mConfirmButton.Text = _strConfirmButtonCaption;
            mCloseButton.Text   = _strCloseButtonCaption;

            mActionsGroup.Clear();
            mActionsGroup.AddChild( mConfirmButton );
            mActionsGroup.AddChild( mCloseButton );

            mConfirmCallback = _confirmCallback;
        }

        //----------------------------------------------------------------------
        public override void Close()
        {
            TitleLabel.Text = "";
            MessageLabel.Text = "";
            mConfirmButton.Text = i18n.Common.Confirm;
            mCloseButton.Text = i18n.Common.Close;

            ShowSpinningWheel = false;
            mCloseCallback = null;
            mConfirmCallback = null;

            ContentGroup.Clear();

            base.Close();
        }

        //----------------------------------------------------------------------
        protected override void Dismiss()
        {
            var closeCallback = mCloseCallback;
            var confirmCallback = mConfirmCallback;
            base.Dismiss();
            if( closeCallback != null ) closeCallback();
            if( confirmCallback != null ) confirmCallback( false );
        }

        //----------------------------------------------------------------------
        protected void Confirm()
        {
            var confirmCallback = mConfirmCallback;
            Close();
            confirmCallback( true );
        }
    }
}
