using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NuclearWinter.UI
{
    /*
     * Holds styles for widgets
     */
    public class Style
    {
        public int              BlurRadius = 4;

        public UIFont           SmallFont;
        public UIFont           MediumFont;
        public UIFont           LargeFont;
        public UIFont           ExtraLargeFont;

        public Texture2D        SpinningWheel;

        public int              ButtonCornerSize = 30;
        public int              DefaultButtonHeight = 80;
        public Color            DefaultTextColor = Color.White;
        public Texture2D        ButtonFrame;

        public int              TooltipCornerSize   = 10;
        public Texture2D        TooltipFrame;
        public Color            TooltipTextColor = Color.White;

        public Texture2D        ButtonHover;

        public Texture2D        ButtonFrameDown;
        public Texture2D        ButtonPress;

        public Texture2D        ButtonFocus;
        public Texture2D        ButtonDownFocus;

        public int              ButtonVerticalPadding = 10;
        public int              ButtonHorizontalPadding = 20;

        public int              RadioButtonCornerSize = 30;
        public int              RadioButtonFrameOffset = 10;
        public Texture2D        ButtonFrameLeft;
        public Texture2D        ButtonFrameLeftDown;
        public Texture2D        ButtonFrameMiddle;
        public Texture2D        ButtonFrameMiddleDown;
        public Texture2D        ButtonFrameRight;
        public Texture2D        ButtonFrameRightDown;

        public Texture2D        EditBoxFrame;
        public int              EditBoxCornerSize = 30;

        public Texture2D        DropDownArrow;

        public Texture2D        ListFrame;

        public Texture2D        ListRowInsertMarker;
        public int              ListRowInsertMarkerCornerSize = 10;

        public Texture2D        GridHeaderFrame;

        public int              GridBoxFrameCornerSize = 30;

        public Texture2D        GridBoxFrame;
        public Texture2D        GridBoxFrameHover;
        public Texture2D        GridBoxFrameFocus;

        public Texture2D        GridBoxFrameSelected;
        public Texture2D        GridBoxFrameSelectedFocus;
        public Texture2D        GridBoxFrameSelectedHover;

        public Texture2D        Panel;
        public int              PanelCornerSize = 10;

        public int              NotebookTabCornerSize = 10;
        public Texture2D        NotebookTab;
        public Texture2D        NotebookActiveTab;
        public Texture2D        NotebookTabFocus;
        public Texture2D        NotebookActiveTabFocus;

        public Texture2D        NotebookTabClose;
        public Texture2D        NotebookTabCloseHover;
        public Texture2D        NotebookTabCloseDown;
        public Texture2D        NotebookUnreadTabMarker;
        
        public int              PopupFrameCornerSize;
        public Texture2D        PopupFrame;
        public Color            PopupBackgroundFadeColor = Color.Black * 0.2f;

        public Texture2D        TreeViewContainerFrame;
        public Texture2D        TreeViewContainerFrameSelected;

        public Texture2D        TreeViewBranchClosed;
        public Texture2D        TreeViewBranchOpen;
        public Texture2D        TreeViewBranchOpenEmpty;
        public Texture2D        TreeViewBranch;
        public Texture2D        TreeViewBranchLast;
        public Texture2D        TreeViewLine;

        public Texture2D        TreeViewCheckBoxFrame;
        public Texture2D        CheckBoxChecked;
        public Texture2D        CheckBoxUnchecked;
        public Texture2D        CheckBoxInconsistent;

        public int              CheckBoxSize = 40;
        public Texture2D        CheckBoxFrame;
        public Texture2D        CheckBoxFrameHover;
        public int              CheckBoxFrameCornerSize = 15;

        public Texture2D        VerticalScrollbar;
        public int              VerticalScrollbarCornerSize = 10;

        public int              SplitterSize                = 10;
        public int              SplitterFrameCornerSize     = 5;
        public Texture2D        SplitterFrame;
        public Texture2D        SplitterDragHandle;
        public Texture2D        SplitterCollapseArrow;

        public Texture2D        ProgressBarFrame;
        public int              ProgressBarFrameCornerSize;

        public Texture2D        ProgressBar;
        public int              ProgressBarCornerSize;

        public int              SliderHandleSize = 30;

        public Texture2D        TextAreaGutterFrame;
        public int              TextAreaGutterCornerSize = 15;
    }
}
