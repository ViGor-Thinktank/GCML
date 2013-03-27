using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NuclearWinter.Collections
{
    //--------------------------------------------------------------------------
    // ObservableList<T> is a decorator for List<T> that allows subscribing to list changes
    // (from http://damieng.com/blog/2006/06/14/Observing_change_events_on_a_ListT)
    public class ObservableList<T> : IList<T>
    {
        //----------------------------------------------------------------------
        private IList<T>    internalList;

        //----------------------------------------------------------------------
        public class ListChangedEventArgs: EventArgs
        {
            public int      Index;
            public T        Item;
            public bool     Added;

            public ListChangedEventArgs( int _index, T _item, bool _bAdded )
            {
                Index   = _index;
                Item    = _item;
                Added   = _bAdded;
            }
        }

        //----------------------------------------------------------------------
        public delegate void ListChangedEventHandler( object source, ListChangedEventArgs e );
        public delegate void ListClearedEventHandler( object source, EventArgs e );

        public event ListChangedEventHandler        ListChanged;
        public event ListClearedEventHandler        ListCleared;

        //----------------------------------------------------------------------
        public ObservableList()
        {
            internalList = new List<T>();
        }

        public ObservableList( IList<T> list )
        {
            internalList = list;
        }

        public ObservableList( IEnumerable<T> collection )
        {
            internalList = new List<T>( collection );
        }

        //----------------------------------------------------------------------
        protected virtual void OnListChanged( ListChangedEventArgs e )
        {
            if( ListChanged != null )
            {
                ListChanged( this, e );
            }
        }

        protected virtual void OnListCleared( EventArgs e )
        {
            if( ListCleared != null )
            {
                ListCleared( this, e );
            }
        }

        //----------------------------------------------------------------------
        public int IndexOf( T item )
        {
            return internalList.IndexOf( item );
        }

        public void Insert( int index, T item )
        {
            internalList.Insert( index, item );
            OnListChanged( new ListChangedEventArgs( index, item, true ) );
        }

        public void RemoveAt( int index )
        {
            T item = internalList[index];
            internalList.Remove( item );
            OnListChanged( new ListChangedEventArgs( index, item, false ) );
        }

        public T this[int index]
        {
            get { return internalList[index]; }
            set {
                internalList[index] = value;
                OnListChanged( new ListChangedEventArgs( index, value, true ) );
            }
        }

        public void Add( T item )
        {
            internalList.Add(item);
            OnListChanged( new ListChangedEventArgs( internalList.IndexOf( item ), item, true ) );
        }

        public void Clear() {
            internalList.Clear();
            OnListCleared( new EventArgs() );
        }

        public bool Contains( T item )
        {
            return internalList.Contains( item );
        }

        public void CopyTo( T[] array, int arrayIndex )
        {
            internalList.CopyTo( array, arrayIndex );
        }

        //----------------------------------------------------------------------
        public int      Count           { get { return internalList.Count; } }
        public bool     IsReadOnly      { get { return IsReadOnly; } }

        //----------------------------------------------------------------------
        public bool Remove(T item) {
            lock( this )
            {
                int index = internalList.IndexOf(item);
                if( internalList.Remove(item) )
                {
                    OnListChanged( new ListChangedEventArgs( index, item, false ) );
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //----------------------------------------------------------------------
        public IEnumerator<T> GetEnumerator()
        {
            return internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ( (IEnumerable) internalList ).GetEnumerator();
        }
    }
}
