using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using NuclearWinter.Collections;

namespace NuclearWinter.UI
{
    //--------------------------------------------------------------------------
    public class NotebookTab: Widget
    {
        //----------------------------------------------------------------------
        public Group            PageGroup       { get; private set; }
        public object           Tag;

        public bool             IsActive        { get { return mNotebook.Tabs[mNotebook.ActiveTabIndex] == this; } }
        public bool             IsUnread;
        public Action           OnReadHandler;

        public bool             IsPinned
        {
            get { return mbIsPinned; }
            set { mbIsPinned = value; UpdateContentSize(); }
        }

        public bool             IsClosable
        {
            get { return mNotebook.HasClosableTabs && ! IsPinned; }
        }

        //----------------------------------------------------------------------
        public string Text
        {
            get { return mLabel.Text; }
            
            set
            {
                mLabel.Text = value;
                mTooltip.Text = value;

                UpdatePaddings();
                UpdateContentSize();
            }
        }

        //----------------------------------------------------------------------
        public Color TextColor
        {
            get { return mLabel.Color; }
            set { mLabel.Color = value; }
        }

        //----------------------------------------------------------------------
        public Texture2D Icon
        {
            get { return mIcon.Texture; }

            set
            {
                mIcon.Texture = value;
                UpdatePaddings();
                UpdateContentSize();
            }
        }

        //----------------------------------------------------------------------
        Notebook                mNotebook;
        Tooltip                 mTooltip;

        //----------------------------------------------------------------------
        Label                   mLabel;
        Image                   mIcon;

        Button                  mCloseButton;
        bool                    mbIsPinned;

        internal int            DragOffset;

        //----------------------------------------------------------------------
        void UpdatePaddings()
        {
            if( mIcon.Texture != null )
            {
                mIcon.Padding = mLabel.Text != "" ? new Box( 10, 0, 10, 10 ) : new Box( 10, 0, 10, 20 );
                mLabel.Padding = mLabel.Text != "" ? new Box( 10, 10, 10, 10 ) : new Box( 10, 20, 10, 0 );
            }
            else
            {
                mLabel.Padding = new Box( 10, 20 );
            }
        }

        //----------------------------------------------------------------------
        public NotebookTab( Notebook _notebook, string _strText, Texture2D _iconTex )
        : base( _notebook.Screen )
        {
            mNotebook       = _notebook;
            Parent          = _notebook;

            mLabel          = new Label( Screen, "", Anchor.Start, Screen.Style.DefaultTextColor );
            mIcon           = new Image( Screen, _iconTex );

            mTooltip        = new Tooltip( Screen, "" );

            mCloseButton    = new Button( Screen, new Button.ButtonStyle( 5, null, null, Screen.Style.NotebookTabCloseHover, Screen.Style.NotebookTabCloseDown, null, 0, 0 ), "", Screen.Style.NotebookTabClose, Anchor.Center );
            mCloseButton.Parent = this;
            mCloseButton.Padding = new Box(0);
            mCloseButton.ClickHandler = delegate {
                mNotebook.Tabs.Remove( this );
                
                Screen.Focus( mNotebook );

                if( mNotebook.TabClosedHandler != null )
                {
                    mNotebook.TabClosedHandler( this );
                }
            };

            Text            = _strText;

            PageGroup       = new Group( Screen );
            PageGroup.Parent = this;
        }

        //----------------------------------------------------------------------
        protected internal override void UpdateContentSize()
        {
            if( mIcon.Texture != null )
            {
                ContentWidth    = mIcon.ContentWidth + mLabel.ContentWidth + Padding.Horizontal;
            }
            else
            {
                ContentWidth    = mLabel.ContentWidth + Padding.Horizontal;
            }

            if( IsClosable )
            {
                ContentWidth += mCloseButton.ContentWidth;
            }

            ContentHeight   = Math.Max( mIcon.ContentHeight, mLabel.ContentHeight ) + Padding.Top + Padding.Bottom;

            base.UpdateContentSize();
        }

        //----------------------------------------------------------------------
        public override void Update( float _fElapsedTime )
        {
            mCloseButton.Update( _fElapsedTime );
            PageGroup.Update( _fElapsedTime );

            mTooltip.EnableDisplayTimer = mNotebook.HoveredTab == this && mNotebook.DraggedTab != this;
            mTooltip.Update( _fElapsedTime );
        }

        //----------------------------------------------------------------------
        public override void DoLayout( Rectangle _rect )
        {
            base.DoLayout( _rect );

            HitBox = _rect;

            Point pCenter = LayoutRect.Center;

            if( mIcon.Texture != null )
            {
                mIcon.DoLayout ( new Rectangle( LayoutRect.X + Padding.Left, pCenter.Y - mIcon.ContentHeight / 2, mIcon.ContentWidth, mIcon.ContentHeight ) );
            }

            int iLabelWidth = LayoutRect.Width - Padding.Horizontal - ( mIcon.Texture != null ? mIcon.ContentWidth : 0 ) - ( IsClosable ? mCloseButton.ContentWidth : 0 );

            mLabel.DoLayout(
                new Rectangle(
                    LayoutRect.X + Padding.Left + ( mIcon.Texture != null ? mIcon.ContentWidth : 0 ), pCenter.Y - mLabel.ContentHeight / 2,
                    iLabelWidth, mLabel.ContentHeight
                )
            );

            if( IsClosable )
            {
                mCloseButton.DoLayout( new Rectangle(
                    LayoutRect.Right - 10 - mCloseButton.ContentWidth,
                    pCenter.Y - Screen.Style.NotebookTabClose.Height / 2,
                    mCloseButton.ContentWidth, mCloseButton.ContentHeight )
                );
            }
        }

        //----------------------------------------------------------------------
        public override Widget HitTest( Point _point )
        {
            return mCloseButton.HitTest( _point ) ?? base.HitTest( _point );
        }

        protected internal override bool OnMouseDown( Point _hitPoint, int _iButton )
        {
            if( _iButton == Screen.Game.InputMgr.PrimaryMouseButton )
            {
                Screen.Focus( this );

                if( IsClosable )
                {
                    mNotebook.DraggedTab = this;
                    DragOffset = _hitPoint.X - LayoutRect.X;
                }
            }
            else
            if( _iButton == 1 )
            {
                Screen.Focus( this );
            }

            return true;
        }

        protected internal override void OnMouseUp( Point _hitPoint, int _iButton )
        {
            if( _iButton == Screen.Game.InputMgr.PrimaryMouseButton ) 
            {
                if( mNotebook.DraggedTab == this )
                {
                    mNotebook.DropTab();
                    DragOffset = 0;
                }

                if( _hitPoint.Y < mNotebook.LayoutRect.Y + mNotebook.TabHeight /* && IsInTab */ )
                {
                    if( _hitPoint.X > LayoutRect.X && _hitPoint.X < LayoutRect.Right )
                    {
                        OnActivateUp();
                    }
                }
            }
            else
            if( _iButton == 1 )
            {
                if( IsClosable && HitBox.Contains( _hitPoint ) )
                {
                    Close();
                    Screen.Focus( mNotebook );

                    if( mNotebook.TabClosedHandler != null )
                    {
                        mNotebook.TabClosedHandler( this );
                    }
                }
            }
        }

        public override void OnMouseEnter( Point _hitPoint )
        {
            mNotebook.HoveredTab = this;
        }

        public override void OnMouseOut( Point _hitPoint )
        {
            if( mNotebook.HoveredTab == this )
            {
                mNotebook.HoveredTab = null;
            }

            mTooltip.EnableDisplayTimer = false;
        }

        public override void OnMouseMove( Point _hitPoint )
        {
            if( mNotebook.DraggedTab == this )
            {
                if( mNotebook.Tabs[ mNotebook.ActiveTabIndex ] != this )
                {
                    mNotebook.SetActiveTab( this );
                }
            }
        }

        protected internal override void OnPadMove( Direction _direction )
        {
            int iTabIndex = mNotebook.Tabs.IndexOf( this );

            if( _direction == Direction.Left && iTabIndex > 0 )
            {
                NotebookTab tab = mNotebook.Tabs[iTabIndex - 1];
                Screen.Focus( tab );
            }
            else
            if( _direction == Direction.Right && iTabIndex < mNotebook.Tabs.Count - 1 )
            {
                NotebookTab tab = mNotebook.Tabs[iTabIndex  + 1];
                Screen.Focus( tab );
            }
            else
            {
                base.OnPadMove( _direction );
            }
        }

        protected internal override void OnActivateUp()
        {
            mNotebook.SetActiveTab( this );
        }

        //----------------------------------------------------------------------
        public override void Draw()
        {
            if( mNotebook.DraggedTab != this )
            {
                DrawTab();
            }
        }

        void DrawTab()
        {
            bool bIsActive = IsActive;

            Screen.DrawBox( bIsActive ? mNotebook.Style.ActiveTab : mNotebook.Style.Tab, LayoutRect, mNotebook.Style.TabCornerSize, Color.White );

            if( mNotebook.HoveredTab == this && ! bIsActive )
            {
                if( Screen.IsActive )
                {
                    Screen.DrawBox( Screen.Style.ButtonHover, LayoutRect, mNotebook.Style.TabCornerSize, Color.White );
                }
            }

            if( IsUnread )
            {
                    Screen.DrawBox( mNotebook.Style.UnreadTabMarker, LayoutRect, mNotebook.Style.TabCornerSize, Color.White );
            }

            if( Screen.IsActive && HasFocus )
            {
                Screen.DrawBox( bIsActive ? mNotebook.Style.ActiveTabFocus : mNotebook.Style.TabFocus, LayoutRect, mNotebook.Style.TabCornerSize, Color.White );
            }

            mLabel.Draw();
            mIcon.Draw();

            if( IsClosable )
            {
                mCloseButton.Draw();
            }
        }

        //----------------------------------------------------------------------
        protected internal override void DrawHovered()
        {
            if( ! mLabel.HasEllipsis ) return;
            
            mTooltip.Draw();
        }

        //----------------------------------------------------------------------
        protected internal override void DrawFocused()
        {
            if( mNotebook.DraggedTab == this )
            {
                DrawTab();
            }
        }

        //----------------------------------------------------------------------
        public void Close( bool _bTriggerCloseHandler=false )
        {
            mNotebook.Tabs.Remove( this );

            if( HasFocus )
            {
                Screen.Focus( mNotebook );
            }

            if( _bTriggerCloseHandler && mNotebook.TabClosedHandler != null )
            {
                mNotebook.TabClosedHandler( this );
            }

        }
    }

    //--------------------------------------------------------------------------
    public class Notebook: Widget
    {
        //----------------------------------------------------------------------
        public struct NotebookStyle
        {
            public NotebookStyle( int _iTabCornerSize, Texture2D _tab, Texture2D _tabFocus, Texture2D _activeTab, Texture2D _activeTabFocus, Texture2D _unreadTabMarker )
            {
                TabCornerSize   = _iTabCornerSize;
                Tab             = _tab;
                TabFocus        = _tabFocus;
                ActiveTab       = _activeTab;
                ActiveTabFocus  = _activeTabFocus;
                UnreadTabMarker = _unreadTabMarker;
            }

            public int              TabCornerSize;

            public Texture2D        Tab;
            public Texture2D        TabFocus;
            public Texture2D        ActiveTab;
            public Texture2D        ActiveTabFocus;
            public Texture2D        UnreadTabMarker;
        }

        //----------------------------------------------------------------------
        public NotebookStyle                        Style;
        public int                                  TabHeight           = 50;
        public int                                  MaxUnpinnedTabWidth = 250;
        public int                                  TabBarLeftOffset    = 0;
        public int                                  TabBarRightOffset   = 0;

        public ObservableList<NotebookTab>          Tabs                { get; private set; }

        public bool                                 HasClosableTabs
        {
            get { return mbHasClosableTabs; }
            set
            {
                mbHasClosableTabs = value;

                foreach( NotebookTab tab in Tabs )
                {
                    tab.UpdateContentSize();
                }
            }
        }

        public Action<NotebookTab>                  TabClosedHandler;

        public int                                  ActiveTabIndex      { get; private set; }

        //----------------------------------------------------------------------
        int                                         miUnpinnedTabWidth;

        internal NotebookTab                        HoveredTab;
        internal NotebookTab                        DraggedTab;
        int                                         miDraggedTabTargetIndex = -1;

        //----------------------------------------------------------------------
        Panel                                       mPanel;
        bool                                        mbHasClosableTabs;

        //----------------------------------------------------------------------
        public Notebook( Screen _screen )
        : base( _screen )
        {
            Style = new NotebookStyle(
                Screen.Style.NotebookTabCornerSize,
                Screen.Style.NotebookTab,
                Screen.Style.NotebookTabFocus,
                Screen.Style.NotebookActiveTab,
                Screen.Style.NotebookActiveTabFocus,
                Screen.Style.NotebookUnreadTabMarker
            );

            mPanel = new Panel( Screen, Screen.Style.Panel, Screen.Style.PanelCornerSize );

            Tabs = new ObservableList<NotebookTab>();

            Tabs.ListChanged += delegate( object _source, ObservableList<NotebookTab>.ListChangedEventArgs _args ) {
                if( ! _args.Added )
                {
                    if( DraggedTab == _args.Item )
                    {
                        DraggedTab = null;
                    }

                    if( HoveredTab == _args.Item )
                    {
                        HoveredTab = null;
                    }
                }

                if( Tabs.Count > 0 )
                {
                    ActiveTabIndex = Math.Min( Tabs.Count - 1, ActiveTabIndex );
                    Tabs[ActiveTabIndex].IsUnread = false;
                }
            };
        }

        //----------------------------------------------------------------------
        protected internal override void UpdateContentSize()
        {
            // No content size
        }

        //----------------------------------------------------------------------
        public override void DoLayout( Rectangle _rect )
        {
            base.DoLayout( _rect );
            HitBox = LayoutRect;

            Rectangle contentRect = new Rectangle( LayoutRect.X, LayoutRect.Y + ( TabHeight - 10 ), LayoutRect.Width, LayoutRect.Height - ( TabHeight - 10 ) );

            mPanel.DoLayout( contentRect );

            int iTabBarStartX = LayoutRect.Left + 20 + TabBarLeftOffset;
            int iTabBarEndX = LayoutRect.Right - 20 - TabBarRightOffset;

            int iTabBarWidth = iTabBarEndX - iTabBarStartX;

            int iUnpinnedTabsCount = Tabs.Count( tab => ! tab.IsPinned );

            int iPinnedTabsWidth = Tabs.Sum( tab => tab.IsPinned ? tab.ContentWidth : 0 );
            int iUnpinnedTabsWidth = MaxUnpinnedTabWidth * iUnpinnedTabsCount;

            miUnpinnedTabWidth = MaxUnpinnedTabWidth;

            if( iPinnedTabsWidth + iUnpinnedTabsWidth > iTabBarWidth && iUnpinnedTabsCount > 0 )
            {
                miUnpinnedTabWidth = ( iTabBarWidth - iPinnedTabsWidth ) / iUnpinnedTabsCount;
            }

            int iDraggedTabX = 0;
            if( DraggedTab != null )
            {
                iDraggedTabX = Math.Min( iTabBarEndX - miUnpinnedTabWidth, Math.Max( iTabBarStartX, Screen.Game.InputMgr.MouseState.X - DraggedTab.DragOffset ) );
            }

            int iTabX = 0;
            int iTabIndex = 0;
            bool bDraggedTabInserted = DraggedTab == null;
            miDraggedTabTargetIndex = Tabs.Count - 1;
            foreach( NotebookTab tab in Tabs )
            {
                if( tab == DraggedTab ) continue;

                int iTabWidth = tab.IsPinned ? tab.ContentWidth : miUnpinnedTabWidth;

                if( ! tab.IsPinned && ! bDraggedTabInserted && iDraggedTabX - iTabBarStartX < iTabX + iTabWidth / 2 )
                {
                    miDraggedTabTargetIndex = iTabIndex;
                    iTabX += miUnpinnedTabWidth;
                    bDraggedTabInserted = true;
                }

                Rectangle tabRect = new Rectangle(
                    iTabBarStartX + iTabX,
                    LayoutRect.Y,
                    iTabWidth,
                    TabHeight
                    );

                tab.DoLayout( tabRect );

                iTabX += iTabWidth;
                iTabIndex++;
            }

            if( DraggedTab != null )
            {
                Rectangle tabRect = new Rectangle(
                    iDraggedTabX,
                    LayoutRect.Y,
                    miUnpinnedTabWidth,
                    TabHeight
                    );

                DraggedTab.DoLayout( tabRect );
            }

            Tabs[ActiveTabIndex].PageGroup.DoLayout( contentRect );
        }

        //----------------------------------------------------------------------
        public void SetActiveTab( NotebookTab _tab )
        {
            Debug.Assert( Tabs.Contains( _tab ) );

            ActiveTabIndex = Tabs.IndexOf( _tab );
            _tab.IsUnread = false;
            if( _tab.OnReadHandler != null ) _tab.OnReadHandler();
        }

        //----------------------------------------------------------------------
        internal void DropTab()
        {
            NotebookTab droppedTab = DraggedTab;
            int iOldIndex = Tabs.IndexOf( droppedTab );

            if( miDraggedTabTargetIndex != iOldIndex )
            {
                Tabs.RemoveAt( iOldIndex );
                Tabs.Insert( miDraggedTabTargetIndex, droppedTab );
                ActiveTabIndex = miDraggedTabTargetIndex;
            }

            DraggedTab = null;
            miDraggedTabTargetIndex = -1;
        }

        //----------------------------------------------------------------------
        public override Widget HitTest( Point _point )
        {
            if( _point.Y < LayoutRect.Y + TabHeight )
            {
                int iTabBarStartX = LayoutRect.Left + 20 + TabBarLeftOffset;

                if( _point.X < iTabBarStartX ) return null;

                int iTabX = 0;
                int iTab = 0;

                foreach( NotebookTab tab in Tabs )
                {
                    int iTabWidth = tab.IsPinned ? tab.ContentWidth : miUnpinnedTabWidth;

                    if( _point.X - iTabBarStartX < iTabX + iTabWidth )
                    {
                        return Tabs[ iTab ].HitTest( _point );
                    }

                    iTabX += iTabWidth;
                    iTab++;
                }

                return null;
            }
            else
            {
                return Tabs[ActiveTabIndex].PageGroup.HitTest( _point );
            }
        }

        protected internal override bool OnPadButton( Buttons _button, bool _bIsDown )
        {
            return Tabs[ActiveTabIndex].OnPadButton( _button, _bIsDown );
        }

        //----------------------------------------------------------------------
        protected internal override void OnKeyPress( Keys _key )
        {
            bool bShortcutKey = Screen.Game.InputMgr.IsShortcutKeyDown();

            NotebookTab activeTab = Tabs[ ActiveTabIndex ];
            if( bShortcutKey && _key == Keys.W && activeTab.IsClosable )
            {
                activeTab.Close();
                Screen.Focus( this );

                if( TabClosedHandler != null )
                {
                    TabClosedHandler( activeTab );
                }
            }
            else
            {
                base.OnKeyPress( _key );
            }
        }

        //----------------------------------------------------------------------
        public override void Update( float _fElapsedTime )
        {
            foreach( NotebookTab tab in Tabs )
            {
                tab.Update( _fElapsedTime );
            }
        }

        //----------------------------------------------------------------------
        public override void Draw()
        {
            mPanel.Draw();

            foreach( NotebookTab tab in Tabs )
            {
                tab.Draw();
            }

            Tabs[ActiveTabIndex].PageGroup.Draw();
        }
    }
}
