#region Imports
using System;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace CySoft.IMapHelper
{
    public interface IBaseMapHelper
    {
        TResult GetNextKey<TResult>(Type type, IDictionary param);
        int GetNextXh(Type type, IDictionary param);
        IList<TResult> QueryList<TResult>(Type type, IDictionary param);
        IList QueryList(Type type, IDictionary param);
        IList<TResult> QueryObjectList<TResult>(Type type, IDictionary param);
        IList<TResult> QueryPage<TResult>(Type type, IDictionary param);
        IList<TResult> QueryPage2<TResult>(Type type, IDictionary param);
        IList QueryPage(Type type, IDictionary param);
        TResult QueryItem<TResult>(Type type, IDictionary param);
        object QueryItem(Type type, IDictionary param);
        int GetCount(Type type, IDictionary param);
        TResult GetItem<TResult>(Type type, IDictionary param);
        int UpdatePart(Type type, IDictionary param);
        IList<TResult> GetTree<TResult>(Type type, IDictionary param);
        IList<TResult> GetTree2<TResult>(Type type, IDictionary param);
        int Update<T>(Type type, T item);
        int Delete(Type type, IDictionary param);
        void Add<T>(Type type, T item);
        void AddRange<T>(IList<T> list);
        DateTime GetDbDateTime();
        string GetDataBaseName();
        int GetBlockedCount(IDictionary param);
        int GetCount2(Type type, IDictionary param);
    }
}
