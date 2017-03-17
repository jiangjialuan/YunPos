using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IDAL.Base;
using System.Collections;

namespace CySoft.IDAL
{
    public interface IInfo_UserDAL:IBaseDAL
    {
        /// <summary>
        /// 批量插入接收公告信息的用户人群
        /// 2015-6-24 wzp
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        int BatchInsert_User(Type type, IDictionary param, string database = null);
        int QueryCountOfGG(Type type, IDictionary param, string database = null);
    }
}
