#region Imports
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.Model.Flags;
using CySoft.Model.Ts;
using CySoft.Frame.Attributes;
#endregion

#region 系统日志
#endregion

namespace CySoft.BLL.SystemBLL.LogBLL
{
    public class LogBLL : BaseBLL, ILogBLL
    {
        /// <summary>
        /// 写数据库操作日志
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <param name="flag_lx">日志类型</param>
        /// <param name="content">内容</param>
        public void Add(Hashtable loginInfo, LogFlag flag_lx, string content)
        {
            HttpContext context = HttpContext.Current;
            Ts_Log log = new Ts_Log();
            log.id = Guid.NewGuid().ToString();
            log.IP = context.Request.UserHostAddress;
            log.id_user = string.Format("{0}", loginInfo["id_user"]);
            log.flag_lx = flag_lx.ToString();
            log.content = content;
            log.flag_from = ((FromFlag)(loginInfo["flag_from"] ?? 0)).ToString();
            log.rq=DateTime.Now;
            if (loginInfo.ContainsKey("ip") && !string.IsNullOrEmpty(loginInfo["ip"].ToString()))
                log.IP = loginInfo["ip"].ToString();
            DAL.Add(log);
        }

        /// <summary>
        /// 写数据库操作日志
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <param name="flag_lx">日志类型</param>
        /// <param name="content">内容</param>
        public void Add(Hashtable loginInfo, string flag_lx, string content)
        {
            HttpContext context = HttpContext.Current;
            Ts_Log log = new Ts_Log();
            log.id = Guid.NewGuid().ToString();
            log.IP = context.Request.UserHostAddress;
            log.id_user = string.Format("{0}", loginInfo["id_user"]);
            log.flag_lx = flag_lx.ToString();
            log.content = content;
            log.flag_from = loginInfo["flag_from"].ToString();
            log.rq = DateTime.Now;
            if (loginInfo.ContainsKey("ip") && !string.IsNullOrEmpty(loginInfo["ip"].ToString()))
                log.IP = loginInfo["ip"].ToString();
            DAL.Add(log);
        }
        /// <summary>
        /// 写数据库操作日志
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <param name="flag_lx">日志类型</param>
        /// <param name="messageList">消息</param>
        [Transaction]
        public void Add(Hashtable loginInfo, LogFlag flag_lx, IList<string> messageList)
        {
            if (messageList != null && messageList.Count > 0)
            {
                foreach (string message in messageList)
                {
                    Add(loginInfo, flag_lx, message);
                }
            }
        }

        /// <summary>
        /// 分页查询
        /// 日期：2015-01-26
        /// </summary>
        public override PageNavigate GetPage(Hashtable param)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.GetCount(typeof(Ts_Log), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = DAL.QueryPage<Ts_Log_Query>(typeof(Ts_Log), param);
            }
            pn.Success = true;
            return pn;
        }

        /// <summary>
        /// 获取上次登录时间
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryItem(typeof(Ts_Log),param);
            br.Success = true;
            return br;
        }








    }
}
