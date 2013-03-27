using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace NuclearWinter.UI
{
    /*
     * GridGroup allows packing widgets in a grid
     * It takes care of positioning each child widget properly
     */
    public class GridGroup: Group
    {
        //----------------------------------------------------------------------
        bool                        mbExpand;
        int                         miSpacing; // FIXME: Not taken into account
        Widget[,]                   maTiles;
        Dictionary<Widget,Point>    maWidgetLocations;

        //----------------------------------------------------------------------
        public GridGroup( Screen _screen, int _iCols, int _iRows, bool _bExpand, int _iSpacing )
        : base( _screen )
        {
            mbExpand    = _bExpand;
            miSpacing   = _iSpacing;

            maTiles = new Widget[ _iCols, _iRows ];
            maWidgetLocations = new Dictionary<Widget,Point>();
        }

        public override void AddChild( Widget _widget, int _iIndex )
        {
            throw new NotSupportedException();
        }

        public void AddChildAt( Widget _child, int _iColumn, int _iRow )
        {
            Debug.Assert( ! maWidgetLocations.ContainsKey( _child ) );
            Debug.Assert( _child.Parent == null );
            Debug.Assert( _child.Screen == Screen );

            _child.Parent = this;

            maTiles[ _iColumn, _iRow ] = _child;
            maWidgetLocations[ _child ] = new Point( _iColumn, _iRow );
            mlChildren.Add( _child );
        }

        public override void RemoveChild( Widget _widget )
        {
            Debug.Assert( _widget.Parent == this );

            _widget.Parent = null;
            
            Point widgetLocation = maWidgetLocations[ _widget ];
            maWidgetLocations.Remove( _widget );

            maTiles[ widgetLocation.X, widgetLocation.Y ] = null;
            mlChildren.Remove( _widget );
        }

        public override Widget GetFirstFocusableDescendant( Direction _direction )
        {
            switch( _direction )
            {
                case Direction.Left:
                    for( int i = maTiles.GetLength(0) - 1; i >= 0; i-- )
                    {
                        for( int j = 0; j < maTiles.GetLength(1); j++ )
                        {
                            Widget widget = maTiles[ i, j ];
                            if( widget != null )
                            {
                                Widget focusableWidget = widget.GetFirstFocusableDescendant( _direction );
                                if( focusableWidget != null )
                                {
                                    return focusableWidget;
                                }
                            }
                        }
                    }
                    break;
                case Direction.Right:
                    for( int i = 0; i < maTiles.GetLength(0); i++ )
                    {
                        for( int j = 0; j < maTiles.GetLength(1); j++ )
                        {
                            Widget widget = maTiles[ i, j ];
                            if( widget != null )
                            {
                                Widget focusableWidget = widget.GetFirstFocusableDescendant( _direction );
                                if( focusableWidget != null )
                                {
                                    return focusableWidget;
                                }
                            }
                        }
                    }
                    break;
                case Direction.Up:
                    for( int j = maTiles.GetLength(1) - 1; j >= 0; j-- )
                    {
                        for( int i = 0; i < maTiles.GetLength(0); i++ )
                        {
                            Widget widget = maTiles[ i, j ];
                            if( widget != null )
                            {
                                Widget focusableWidget = widget.GetFirstFocusableDescendant( _direction );
                                if( focusableWidget != null )
                                {
                                    return focusableWidget;
                                }
                            }
                        }
                    }
                    break;
                case Direction.Down:
                    for( int j = 0; j < maTiles.GetLength(1); j++ )
                    {
                        for( int i = 0; i < maTiles.GetLength(0); i++ )
                        {
                            Widget widget = maTiles[ i, j ];
                            if( widget != null )
                            {
                                Widget focusableWidget = widget.GetFirstFocusableDescendant( _direction );
                                if( focusableWidget != null )
                                {
                                    return focusableWidget;
                                }
                            }
                        }
                    }
                    break;
            }

            return null;
        }

        public override Widget GetSibling( Direction _direction, Widget _child )
        {
            Widget tileChild = null;
            Point childLocation = maWidgetLocations[ _child ];
            int iOffset = 0;

            do
            {
                iOffset++;

                switch( _direction )
                {
                    case Direction.Left:
                        if( childLocation.X - iOffset < 0 ) return base.GetSibling( _direction, _child );
                        tileChild = maTiles[ childLocation.X - iOffset, childLocation.Y ];
                        break;
                    case Direction.Right:
                        if( childLocation.X + iOffset >= maTiles.GetLength(0) ) return base.GetSibling( _direction, _child );
                        tileChild = maTiles[ childLocation.X + iOffset, childLocation.Y ];
                        break;
                    case Direction.Up:
                        if( childLocation.Y - iOffset < 0 ) return base.GetSibling( _direction, _child );
                        tileChild = maTiles[ childLocation.X, childLocation.Y - iOffset ];
                        break;
                    case Direction.Down:
                        if( childLocation.Y + iOffset >= maTiles.GetLength(1) ) return base.GetSibling( _direction, _child );
                        tileChild = maTiles[ childLocation.X, childLocation.Y + iOffset ];
                        break;
                }
            }
            while( tileChild == null );

            if( tileChild != null )
            {
                return tileChild;
            }
            else
            {
                return base.GetSibling( _direction, this );
            }
        }

        //----------------------------------------------------------------------
        public override void DoLayout( Rectangle _rect )
        {
            base.DoLayout( _rect );
        }

        protected override void LayoutChildren()
        {
            if( ! mbExpand )
            {
                int iColumnCount    = maTiles.GetLength(0);
                int iRowCount       = maTiles.GetLength(1);

                Point widgetSize = new Point(
                    LayoutRect.Width / iColumnCount,
                    LayoutRect.Height / iRowCount );

                foreach( KeyValuePair<Widget,Point> kvpChild in maWidgetLocations  )
                {
                    Point widgetPosition = new Point(
                        LayoutRect.X + widgetSize.X * kvpChild.Value.X,
                        LayoutRect.Y + widgetSize.Y * kvpChild.Value.Y );

                    kvpChild.Key.DoLayout( new Rectangle( widgetPosition.X, widgetPosition.Y, widgetSize.X, widgetSize.Y ) );
                }
            }
        }
    }
}
