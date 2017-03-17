using System;
using System.Collections;
using System.Collections.Generic;
using CySoft.Frame.Core;

namespace CySoft.Utility.Mvc.Html
{
    /// <summary>
    /// 分页传输对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageList<T> : IList<T>
    {
        //private int _PageIndex = 1;
        private int _PageIndex = 0;     //YZQ TEST

        private IList<T> list;

        public PageList(int pageSize)
        {
            this.PageSize = pageSize;
            this.list = new List<T>();
        }

        public PageList(PageNavigate pn, int pageIndex, int pageSize)
        {
            this.ItemCount = pn.TotalCount;
            this.PageSize = pageSize > 0 ? pageSize : 1;
            //this.PageIndex = pageIndex > 0 ? pageIndex : 1;
            this.PageIndex = pageIndex > -1 ? pageIndex : 0;        //YZQ TEST
            if (pn.Data != null && (pn.Data is IList<T> || pn.Data is List<T>))
            {
                this.list = (IList<T>)pn.Data;
            }
            else
            {
                this.list = new List<T>();
            }
            this.PageCount = (int)Math.Ceiling((double)(((double)pn.TotalCount) / ((double)pageSize)));
        }

        public IList<T> Items { get { return list; } }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int ItemCount { get; private set; }

        public int PageIndex
        {
            get
            {
                return this._PageIndex;
            }
            set
            {
                this._PageIndex = value;
            }
        }

        public int IndexOf(T item)
        {
            return this.list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.list.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list[index] = value;
            }
        }

        public void Add(T item)
        {
            this.list.Add(item);
        }

        public void Clear()
        {
            this.list.Clear();
        }

        public bool Contains(T item)
        {
            return this.list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.list.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.list.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return this.list.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }
    }
}
