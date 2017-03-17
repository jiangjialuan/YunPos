using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Core;
using CySoft.Model;
using CySoft.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CySoft.Controllers.Service
{
    public class LogServiceController : ServiceBaseController
    {
        #region 日志接口接口
        /// <summary>
        /// 日志接口接口
        /// lz
        /// 2016-11-18
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult Add()
        {
            ServiceResult res = new ServiceResult() { State = ServiceState.Done };
            try
            {
                #region 编码类型
                Encoding encoding = null;
                //编码类型
                string charset = System.Configuration.ConfigurationManager.AppSettings["Charset"];
                try
                {
                    encoding = Encoding.GetEncoding(charset);
                }
                catch
                {
                    encoding = Encoding.GetEncoding("UTF-8");
                }
                #endregion

                #region 获取参数
                Hashtable param = base.GetParameters();
                string id_user = Request["id_user"] == null ? "" : HttpUtility.UrlDecode(Request["id_user"], encoding);//id_user
                string flag_from = Request["flag_from"] == null ? "" : HttpUtility.UrlDecode(Request["flag_from"], encoding);//flag_from
                string flag_lx = Request["flag_lx"] == null ? "" : HttpUtility.UrlDecode(Request["flag_lx"], encoding);//flag_lx
                string content = Request["content"] == null ? "" : HttpUtility.UrlDecode(Request["content"], encoding);//content
                string sign = Request["sign"] == null ? "" : HttpUtility.UrlDecode(Request["sign"], encoding);//sign
                string ip = Request["ip"] == null ? "" : HttpUtility.UrlDecode(Request["ip"], encoding);//ip

                #endregion
                #region 验证参数
                if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(sign))
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return JsonString(res);
                }
                #endregion
                #region 验证签名
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("id_user", HttpUtility.UrlEncode(id_user, Encoding.UTF8) );
                dic.Add("flag_from", HttpUtility.UrlEncode(flag_from, Encoding.UTF8) );
                dic.Add("flag_lx", HttpUtility.UrlEncode(flag_lx, Encoding.UTF8) );
                //dic.Add("content", HttpUtility.UrlEncode(content, Encoding.UTF8)  );
                var validSign = SignUtils.SignRequest(dic, "CY2016");
                //验证签名
                if (sign != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return JsonString(res);
                }
                #endregion
                #region 读取数据
                Hashtable loginInfo = new Hashtable();
                loginInfo.Add("id_user", id_user);
                loginInfo.Add("flag_from", flag_from);

                if(!string.IsNullOrEmpty(ip))
                    loginInfo.Add("ip", ip);

                BusinessFactory.Log.Add(loginInfo, flag_lx.ToString(), content.ToString());

                #endregion
                #region 返回
                res.State = ServiceState.Done;
                res.Message = "操作成功!";
                res.Data = "";
                return JsonString(res);
                #endregion
            }
            catch (Exception ex)
            {
                res.State = ServiceState.Fail;
                res.Message = ServiceFailCode.S0001;
                return JsonString(res);
            }

        }
        #endregion
    }
}
