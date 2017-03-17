using System;
using System.Collections.Generic;
using CySoft.IDAL.Base;
using CySoft.Model.Tb;
using System.Collections;

namespace CySoft.IDAL
{
    public interface IFaqDAL : IBaseDAL
    {
        /// <summary>
        /// 反馈管理：接口分页
        /// wzp 2015-6-30
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        IList<Faq_Tree> QueryServicePage(Type type, IDictionary param, string database = null);
    }
}
