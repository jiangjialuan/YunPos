using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CySoft.Frame.Core;
using CySoft.Frame.Utility;
using CySoft.IBLL.Base;

namespace CySoft.BLL.Base
{
    public class TreeBLL : BaseBLL, ITreeBLL
    {
        public virtual BaseResult GetTree(Hashtable param = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 构建树形结构
        /// </summary>
        #region internal virtual IList<T> CreateTree<T>(IList<T> list)
        internal virtual IList<T> CreateTree<T>(IList<T> list)
        {
            return CreateTree(list, 0);
        }
        #endregion

        /// <summary>
        /// 构建树形结构
        /// </summary>
        #region internal IList<T> CreateTree<T>(IList<T> list, long fatherId)
        internal IList<T> CreateTree<T>(IList<T> list, long fatherId)
        {
            IList<T> noteList = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                T model = list[i];
                if (Convert.ToInt64(model.GetValue("fatherId")) == fatherId)
                {
                    noteList.Add(model);
                    list.RemoveAt(i);
                    i -= 1;
                    long id = Convert.ToInt64(model.GetValue("id"));
                    model.SetValue("children", CreateTree(list.ToList(), id));
                }
            }
            return noteList;
        }
        #endregion

        /// <summary>
        /// 构建树形结构
        /// </summary>
        #region internal IList<T> CreateTree<T>(IList<T> list, string fatherId)
        internal IList<T> CreateTree<T>(IList<T> list, string fatherId)
        {
            IList<T> noteList = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                T model = list[i];
                var v = model.GetValue("fatherId").ToString();
                if (v == fatherId)
                {
                    noteList.Add(model);
                    list.RemoveAt(i);
                    i -= 1;
                    var id = model.GetValue("id").ToString();
                    model.SetValue("children", CreateTree(list.ToList(), id));
                }
            }
            return noteList;
        }
        #endregion


    }
}
