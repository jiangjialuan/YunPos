using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Core;
using CySoft.Model;
using CySoft.Model.Tb;
using CySoft.Utility;

namespace CySoft.Controllers.Service
{
    public class AccountServiceController : ServiceBaseController
    {
        #region 用户修改密码
        /// <summary>
        /// 用户修改密码
        /// 2016-11-24
        /// </summary>
        //[HttpPost]
        [ActionPurview(false)]
        public ActionResult ChangePwd()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("id_user", string.Empty, HandleType.ReturnMsg);//id
                p.Add("old_pwd", string.Empty, HandleType.ReturnMsg);//old_pwd
                p.Add("new_pwd", string.Empty, HandleType.ReturnMsg);//new_pwd
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = "部份必要参数缺失!";
                    return res;
                }
                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = "主用户或门店ID缺失!";
                    return res;
                } 
                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = "登录票据已不存在，请重新登录!";
                    return res;
                } 
                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "id_user", "old_pwd", "new_pwd" });

                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = "数据签名验证失败!";
                    return res;
                }
                #endregion

                #region 更新密码
                param.Remove("sign");
                param.Add("id", param["id_user"]);
                param.Remove("id_user");
                var br = BusinessFactory.Account.ChangeUserPwd(param);
                #endregion

                #region 返回
                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = "";
                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion
    }
}
