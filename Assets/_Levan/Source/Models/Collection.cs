using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LevanPangInterview.Models
{
    [Serializable]
    public class Collection<T> : Model, IList<T>
    {
        //// Members

        public List<T> Items = new List<T>();

        //// Operatoes

        public T this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }

        //// IList

        #region IList Implementation
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        public int              Count                               => this.Items.Count;
        public bool             IsReadOnly                          => false;
        public void             Add(T item)                         => this.Items.Add(item);
        public void             Clear()                             => this.Items.Clear();
        public bool             Contains(T item)                    => Items.Contains(item);
        public void             CopyTo(T[] array, int arrayIndex)   => Items.CopyTo(array, arrayIndex);
        public IEnumerator<T>   GetEnumerator()                     => Items.GetEnumerator();
        public int              IndexOf(T item)                     => this.Items.IndexOf(item);
        public void             Insert(int index, T item)           => this.Items.Insert(index, item);
        public bool             Remove(T item)                      => this.Items.Remove(item);
        public void             RemoveAt(int index)                 => this.Items.RemoveAt(index);
        IEnumerator             IEnumerable.GetEnumerator()         => this.Items?.GetEnumerator();
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion // IList Implementation
    }
}
