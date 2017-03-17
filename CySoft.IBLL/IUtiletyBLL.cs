using System;
using System.Collections;
using System.Collections.Generic;
using CySoft.Frame.Core;

namespace CySoft.IBLL
{
    public interface IUtiletyBLL
    {
        /// <summary>
        /// 获得下一个主键
        /// </summary>
        /// <param name="type">typeof(model)model类型</param>
        /// <returns>最大主键</returns>
        long GetNextKey(Type type);
        /// <summary>
        ///  获得下一个序号
        /// </summary>
        /// <param name="type">typeof(model)model类型</param>
        /// <returns>最大序号</returns>
        long GetNextXh(Type type);
        BaseResult GetNextDH(object entity, Type type = null);
    }
}
