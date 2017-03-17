using System;
using System.Collections;
using CySoft.Frame.Core;

///短信发送接口
namespace CySoft.Utility.SMS
{
    public interface ISMSHelper
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        BaseResult Init(Hashtable param = null);
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <returns></returns>
        BaseResult Send(Hashtable param = null);
        BaseResult Send(string phone,string msg);
        /// <summary>
        /// 注册系统
        /// </summary>
        /// <returns></returns>
        BaseResult Register(Hashtable param = null);
        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        BaseResult Logout(Hashtable param = null);
        /// <summary>
        /// 查询余额
        /// </summary>
        /// <returns></returns>
        BaseResult QueryAmount(Hashtable param = null);
        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        BaseResult PayAmount(Hashtable param = null);    
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        BaseResult Status(Hashtable param = null);
        /// <summary>
        /// 报表
        /// </summary>
        /// <returns></returns>
        BaseResult Report(Hashtable param = null);
    }
}
