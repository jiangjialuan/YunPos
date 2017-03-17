using System;
using System.Collections;
using System.Collections.Generic;
using CySoft.IBLL.Base;
using CySoft.Model.Flags;

namespace CySoft.IBLL
{
    public interface ILogBLL : IBaseBLL
	{
        /// <summary>
        /// 写数据库操作日志_string
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <param name="flag_lx">日志类型</param>
        /// <param name="content">内容</param>
        void Add(Hashtable loginInfo, LogFlag flag_lx, string content);

        /// <summary>
        /// 写数据库操作日志_string
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <param name="flag_lx">日志类型</param>
        /// <param name="content">内容</param>
        void Add(Hashtable loginInfo, string flag_lx, string content);
        /// <summary>
        /// 写数据库操作日志_list
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <param name="flag_lx">日志类型</param>
        /// <param name="content">内容</param>
        void Add(Hashtable loginInfo, LogFlag flag_lx, IList<string> messageList);


	}
}
