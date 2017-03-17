using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Utility;
using CySoft.Frame.Common;
using CySoft.Model;
using CySoft.Model.Ts;
using CySoft.Model.Tb;
using System.Threading;
using Spring.Core.IO;
using CySoft.Model.Enums;

namespace CySoft.Controllers.Service.Base
{
    //public class ServiceResult
    //{
    //    public object Data { get; set; }
    //    public bool Success { get; set; }
    //    public string Message { get; set; }
    //}
    public abstract class ServiceBaseController : Controller
    {
        private string _logAPIUrl = System.Configuration.ConfigurationManager.AppSettings["LogAPIUrl"];

        public string Ip
        {
            get { return ControllerContext.HttpContext.Request.UserHostAddress; }
        }


        protected ServiceResult RequestResult(Func<ServiceResult, ServiceResult> func)
        {
            ServiceResult sr = new ServiceResult() { State = ServiceState.Done };
            Hashtable param = new Hashtable();
            try
            {
                param = this.GetParameters();
                //检查用户购买服务系统
                var br = CheckUserService(param);
                if (br.Success)
                {
                    sr = func(sr);
                }
                else
                {
                    sr.Message = string.Join(",", br.Message);
                    sr.State = ServiceState.Fail;
                    sr.Number = "BuyService";
                    sr.Data = br.Data;
                }
            }
            catch (CySoftException br)
            {
                sr.Message = string.Join(",", br.Message);
                sr.State = ServiceState.Fail;
            }
            catch (Exception ex)
            {
                sr.Message = "系统错误";
                sr.State = ServiceState.Error;
            }
            RetAddLog(param, sr);
            return sr;
        }

        /// <summary>
        /// 获得所有请求参数
        /// </summary>
        protected virtual Hashtable GetParameters()
        {
            Hashtable param = new Hashtable();
            foreach (string item in Request.QueryString.AllKeys)
            {
                param[item] = Request.QueryString[item].Trim();
            }
            foreach (string item in Request.Form.AllKeys)
            {
                param[item] = Request.Form[item].Trim();
            }
            return param;
        }


        private string GetGetParametersStr()
        {
            try
            {
                string str = Request.QueryString.ToString();
                if (string.IsNullOrEmpty(str))
                    str = Request.Form.ToString();
                if (string.IsNullOrEmpty(str))
                    str = JSON.Serialize(this.GetParameters());
                return str;
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        protected virtual IDictionary<string, string> GetParameters(string[] keys)
        {
            IDictionary<string, string> param = new Dictionary<string, string>();
            if (keys != null && keys.Any())
            {
                foreach (var key in keys)
                {
                    string value = Request.Form[key] ?? Request.QueryString[key];
                    if (!string.IsNullOrEmpty(value))
                    {
                        param[key] = value;
                    }
                    else
                    {
                        param[key] = "";
                    }
                }
            }
            return param;
        }

        protected string GetValidToken()
        {
            var token = System.Configuration.ConfigurationManager.AppSettings["ServiceToken"];
            return token;
        }

        protected ActionResult JsonString(dynamic obj)
        {
            return Content(content: Utility.JSON.Serialize(obj), contentType: "application/json",
                contentEncoding: Encoding.UTF8);
        }

        /// <summary>
        /// 获得指定请求参数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">当值为null时使用的默认值。</param>
        /// <returns>值</returns>
        protected string GetParameter(string key, string defaultValue = null)
        {
            string value = Request.Form[key] ?? Request.QueryString[key];
            if (value == null)
            {
                return defaultValue;
            }
            return value.Trim();
        }

        public ServiceResult CheckParamNull(ServiceResult res)
        {
            res.State = ServiceState.Done;
            if (GetParameter("sign") == null)
            {
                res.State = ServiceState.Fail;
                res.Message = string.Format("缺少必要参数");
                return res;
            }
            return res;
        }

        private string TurnParametersStr(Hashtable param)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                foreach (var item in param.Keys)
                {
                    string key = item.ToString();
                    string value = param[item.ToString()].ToString();
                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                    {
                        if (str.Length != 0)
                            str.Append("&");
                        str.Append(key).Append("=").Append(value);
                    }
                }
                return str.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }



        /// <summary>
        /// ts_parm表 配置参数
        /// lz 2016-09-06
        /// </summary>
        /// <returns></returns>
        public Hashtable GetParm(string id_user_master)
        {
            return BusinessFactory.Account.GetParm(id_user_master);
            #region 注释
            //if (DataCache.Get(id_user_master + "_GetParm") != null && ((Hashtable)DataCache.Get(id_user_master + "_GetParm")).Keys.Count > 0)
            //    return (Hashtable)DataCache.Get(id_user_master + "_GetParm");
            //else
            //{
            //    Hashtable result = new Hashtable();
            //    result.Add("je_digit", 2);
            //    result.Add("sl_digit", 2);
            //    result.Add("dj_digit", 2);
            //    result.Add("zk_digit", 2);
            //    result.Add("hy_shopshare", 0);
            //    Hashtable ht = new Hashtable();
            //    ht.Add("get_self_defaul", "1");
            //    ht.Add("self_id_masteruser", id_user_master);
            //    var parmListBr = BusinessFactory.Ts_Parm.GetAll(ht);
            //    if (parmListBr.Success)
            //    {
            //        var parmList = (List<Ts_Parm>)parmListBr.Data;
            //        var jeDigitModel = parmList.Where(d => d.parmcode == "je_digit").OrderByDescending(d => d.id_masteruser).FirstOrDefault() ?? new Ts_Parm();
            //        int jeDigit = 2;
            //        int.TryParse(jeDigitModel.parmvalue, out jeDigit);
            //        var slDigitModel = parmList.Where(d => d.parmcode == "sl_digit").OrderByDescending(d => d.id_masteruser).FirstOrDefault() ?? new Ts_Parm();
            //        int slDigit = 2;
            //        int.TryParse(slDigitModel.parmvalue, out slDigit);
            //        var djDigitModel = parmList.Where(d => d.parmcode == "dj_digit").OrderByDescending(d => d.id_masteruser).FirstOrDefault() ?? new Ts_Parm();
            //        int djDigit = 2;
            //        int.TryParse(djDigitModel.parmvalue, out djDigit);
            //        var zkDigitModel = parmList.Where(d => d.parmcode == "zk_digit").OrderByDescending(d => d.id_masteruser).FirstOrDefault() ?? new Ts_Parm();
            //        int zkDigit = 2;
            //        int.TryParse(zkDigitModel.parmvalue, out zkDigit);
            //        var shopShareModel = parmList.Where(d => d.parmcode == "hy_shopshare").OrderByDescending(d => d.id_masteruser).FirstOrDefault() ?? new Ts_Parm();
            //        int shopShare = 0;
            //        int.TryParse(shopShareModel.parmvalue, out shopShare);
            //        result["je_digit"] = jeDigit;
            //        result["sl_digit"] = slDigit;
            //        result["dj_digit"] = djDigit;
            //        result["zk_digit"] = zkDigit;
            //        result["hy_shopshare"] = shopShare;
            //        DataCache.Add(id_user_master + "_GetParm", result, DateTime.Now.AddDays(1));
            //    }
            //    return result;
            //} 
            #endregion
        }



        protected void WriteDBLog(LogFlag flag_lx, IList<string> messageList)
        {
            Hashtable loginInfo = new Hashtable();
            BusinessFactory.Log.Add(loginInfo, flag_lx, messageList);
        }



        /// <summary>
        /// 获取接口签名Ticket
        /// lz 2016-10-25
        /// </summary>
        /// <returns></returns>
        public BaseResult GetTicketInfo(string key_y)
        {
            BaseResult res = new BaseResult() { Success = false };
            if (DataCache.Get(key_y + "_RegisterTicket") != null && !string.IsNullOrEmpty(((Tb_Ticket)DataCache.Get(key_y + "_RegisterTicket")).id))
            {
                res.Success = true;
                res.Data = (Tb_Ticket)DataCache.Get(key_y + "_RegisterTicket");
                return res;
            }
            else
            {
                Hashtable ht = new Hashtable();
                ht.Add("key_y", key_y);
                var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
                if (ticketBr.Success && ((Tb_Ticket)ticketBr.Data) != null && !string.IsNullOrEmpty(((Tb_Ticket)ticketBr.Data).id))
                {
                    res.Success = true;
                    res.Data = ticketBr.Data;
                    DataCache.Add(key_y + "_RegisterTicket", (Tb_Ticket)ticketBr.Data, DateTime.Now.AddDays(1));
                    return res;
                }
                else
                {
                    res.Success = false;
                    return res;
                }
            }
        }


        public void ReqAddLog(Hashtable param)
        {
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("context", System.Web.HttpContext.Current);
                string id_user = "";
                if (param["id_shop"] != null)
                    id_user = param["id_shop"].ToString();
                else if (param["id_masteruser"] != null)
                    id_user = param["id_masteruser"].ToString();
                ht.Add("id_user", id_user);
                ht.Add("content", "接口" + Request.RawUrl + "  接受参数:" + TurnParametersStr(param));
                ThreadPool.QueueUserWorkItem(new WaitCallback(HandOutDataToSvr), ht);
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
            }
        }

        public void RetAddLog(Hashtable param, ServiceResult sr)
        {
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("context", System.Web.HttpContext.Current);
                string id_user = "";
                if(param!=null&& param.ContainsKey("id_masteruser") &&!string.IsNullOrEmpty(param["id_masteruser"].ToString()))
                    id_user = param["id_masteruser"].ToString();
                else if (param != null && param.ContainsKey("id_shop") && !string.IsNullOrEmpty(param["id_shop"].ToString()))
                    id_user = param["id_shop"].ToString();

                //if (param["id_shop"] != null)
                //    id_user = param["id_shop"].ToString();
                //else if (param["id_masteruser"] != null)
                //    id_user = param["id_masteruser"].ToString();

                ht.Add("id_user", id_user);
                ht.Add("content", "接口" + Request.RawUrl + "  接受参数:" + TurnParametersStr(param) + "  返回参数:" + JSON.Serialize(sr));
                ht.Add("ip", Ip);
                ThreadPool.QueueUserWorkItem(new WaitCallback(HandOutDataToSvr), ht);
            }
            catch (Exception ex)
            {

            }
        }


        #region 线程池方法处理
        /// <summary>
        /// 线程池方法处理
        /// lz
        /// 2016-10-31
        /// </summary>
        /// <param name="obj"></param>
        public void HandOutDataToSvr(object obj)
        {
            try
            {
                Hashtable param = (Hashtable)obj;
                //System.Web.HttpContext.Current = (System.Web.HttpContext)param["context"];
                //Hashtable loginInfo = new Hashtable();
                //loginInfo.Add("id_user", param["id_user"].ToString());
                //loginInfo.Add("flag_from", 3);
                //BusinessFactory.Log.Add(loginInfo, LogFlag.Bill, param["content"].ToString());

                var paramters = new Dictionary<string, string>();
                paramters.Add("id_user", HttpUtility.UrlEncode(param["id_user"].ToString(), Encoding.UTF8));
                paramters.Add("flag_from", HttpUtility.UrlEncode("computer", Encoding.UTF8));
                paramters.Add("flag_lx", HttpUtility.UrlEncode("Bill", Encoding.UTF8));
                string mySign = SignUtils.SignRequest(paramters, "CY2016");
                paramters.Add("content", HttpUtility.UrlEncode("[" + param["content"].ToString() + "]", Encoding.UTF8));
                paramters.Add("sign", mySign);
                paramters.Add("ip", HttpUtility.UrlEncode(param["ip"].ToString(), Encoding.UTF8));
                var result = new WebUtils().DoPost(_logAPIUrl, paramters, 20000);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion


        protected BaseResult GetFlagList(string listcode)
        {
            BaseResult res = new BaseResult() { Success = false };
            try
            {
                Hashtable param = new Hashtable();
                param.Add("listcode", listcode);
                param.Add("sort", "listsort");
                param.Add("dir", "asc");
                res = BusinessFactory.Ts_Flag.GetAll(param);
            }
            catch (CySoftException ex)
            {
                res.Message.Add(ex.Message);
                res.Level = ErrorLevel.Warning;
            }
            catch (Exception ex)
            {
                res.Message.Add(ex.Message);
            }
            return res;
        }

        public BaseResult CheckUserService(Hashtable param)
        {
            BaseResult br = new BaseResult() { Success = true };
            //return br;

            if (PublicSign.flagCheckService == "1" && param.ContainsKey("id_masteruser"))
            {
                if (!DoNotCheckService())
                {
                    br = CheckUserServiceWork(param);
                }
            }
            return br;
        }

        private bool DoNotCheckService()
        {
            bool result = false;
            var url = Request.RawUrl.ToLower();
            var apiConfig = PublicSign.doNotCheckAPI;
            if (!string.IsNullOrEmpty(apiConfig))
            {
                var arr = apiConfig.Split('|');
                foreach (var item in arr)
                {
                    if (url.Contains(item.ToLower()))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }


        public BaseResult CheckUserServiceWork(Hashtable param)
        {
            BaseResult br = new BaseResult() { Success = true };

            var brUser = GetUserInfo(param);
            if (brUser.Success)
            {
                var userHas = (Hashtable)brUser.Data;
                if (userHas != null && userHas.ContainsKey("user_master"))
                {
                    var user_master = (Tb_User)userHas["user_master"];

                    if (user_master == null)
                    {
                        br.Message.Clear();
                        br.Message.Add("操作失败 未查询到对应的主用户信息！");
                        br.Level = ErrorLevel.Error;
                        br.Success = false;
                        return br;
                    }

                    Hashtable ht = new Hashtable();
                    ht.Add("id_cyuser", user_master.id_cyuser);
                    ht.Add("id_user_master", user_master.id_masteruser);
                    ht.Add("phone_master", user_master.phone);
                    ht.Add("rq_create_master_shop", user_master.rq_create);

                    var bm = BusinessFactory.Account.GetServiceBM(user_master.version.ToString());
                    if (string.IsNullOrEmpty(bm))
                    {
                        br.Message.Clear();
                        br.Message.Add("操作失败 获取服务编码异常 请检查版本是否正常！");
                        br.Level = ErrorLevel.Error;
                        br.Success = false;
                        return br;
                    }

                    ht.Add("bm", bm);
                    var brCheck = CheckService(ht);
                    if (!brCheck.Success)
                    {
                        br.Success = false;
                        br.Message = brCheck.Message;
                        br.Data = brCheck.Data;
                        br.Level = brCheck.Level;
                    }
                    else
                    {
                        #region 符合条件 检验是否 将要过期
                        Hashtable cyServiceHas = (Hashtable)brCheck.Data;
                        if (cyServiceHas != null && cyServiceHas.ContainsKey("cyServiceList") && cyServiceHas.ContainsKey("isExpire") && cyServiceHas.ContainsKey("isStop") && cyServiceHas.ContainsKey("buyUrl") && cyServiceHas.ContainsKey("endTime"))
                        {
                            var vEndData = cyServiceHas["endTime"].ToString();
                            DateTime d1 = DateTime.Parse(vEndData);
                            DateTime d2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                            if (d1 > DateTime.Parse("1900-01-01"))
                            {
                                int cyDay = ((int)(d1 - d2).TotalDays);//天数差     
                                if (cyDay <= PublicSign.showMsgServiceDay)
                                {
                                    var buyUrl = cyServiceHas["buyUrl"].ToString();
                                    br.Message.Clear();
                                    br.Message.Add(" 您购买的服务有效期剩余 " + cyDay + " 天 即将到期！");
                                    br.Data = buyUrl;
                                    br.Level = ErrorLevel.Question;
                                }
                            }
                        }
                        #endregion
                    }
                }
            }

            return br;
        }

        public BaseResult CheckService(Hashtable userinfo)
        {
            //id_cyuser
            //id_user_master
            //phone_master
            //rq_create_master_shop
            //bm

            BaseResult br = new BaseResult() { Success = true };
            Hashtable ht = new Hashtable();
            ht.Add("id_cyuser", userinfo["id_cyuser"].ToString());
            ht.Add("bm", userinfo["bm"].ToString());
            ht.Add("service", "GetService");
            ht.Add("id_masteruser", userinfo["id_user_master"].ToString());
            ht.Add("rq_create_master_shop", userinfo["rq_create_master_shop"].ToString());

            ht.Add("check_db_count", "1");

            var cyServiceHas = BusinessFactory.Account.GetCYService(ht);
            if (cyServiceHas == null || !cyServiceHas.ContainsKey("cyServiceList") || !cyServiceHas.ContainsKey("isExpire") || !cyServiceHas.ContainsKey("isStop"))
            {
                br.Message.Clear();
                br.Message.Add("获取购买服务异常,请重试！");
                br.Level = ErrorLevel.Error;
                br.Success = false;
                return br;
            }

            if (DataCache.Get(userinfo["id_user_master"].ToString() + "_GetCYService") != null && ((Hashtable)DataCache.Get(userinfo["id_user_master"].ToString() + "_GetCYService")).Keys.Count > 0)
                cyServiceHas = (Hashtable)DataCache.Get(userinfo["id_user_master"].ToString() + "_GetCYService");

            if (cyServiceHas == null || !cyServiceHas.ContainsKey("cyServiceList") || !cyServiceHas.ContainsKey("isExpire") || !cyServiceHas.ContainsKey("isStop") || !cyServiceHas.ContainsKey("isOutLimit"))
            {
                br.Message.Clear();
                br.Success = false;
                br.Message.Add("获取购买服务信息失败，不允许操作!");
                br.Level = ErrorLevel.Error;
                return br;
            }

            string buyUrl = this.GetUrl(userinfo);


            if (cyServiceHas["isStop"].ToString() == "1")
            {
                br.Message.Clear();
                br.Success = false;
                br.Message.Add("您购买的服务信息已停用 不允许操作!");
                br.Level = ErrorLevel.Drump;
                br.Data = buyUrl;
                return br;
            }

            if (cyServiceHas["isExpire"].ToString() == "1")
            {
                br.Message.Clear();
                br.Success = false;
                br.Message.Add("您还未购买服务信息或已超过有效期 不允许操作!");
                br.Level = ErrorLevel.Drump;
                br.Data = buyUrl;
                return br;
            }

            if (cyServiceHas["isOutLimit"].ToString() == "1")
            {
                br.Message.Clear();
                br.Success = false;
                br.Message.Add("您的门店设置已经超过购买的服务 不允许操作!");
                br.Level = ErrorLevel.Question;
                br.Data = buyUrl;
                return br;
            }

            if (br.Success)
            {
                if (!cyServiceHas.ContainsKey("buyUrl"))
                    cyServiceHas.Add("buyUrl", buyUrl);
                br.Data = cyServiceHas;
            }

            return br;
        }

        public string GetUrl(Hashtable param)
        {
            Hashtable ht = new Hashtable();
            ht.Add("id_cyuser", param["id_cyuser"].ToString());
            ht.Add("id", param["bm"].ToString());
            ht.Add("phone", param["phone_master"].ToString());
            ht.Add("service", "Detail");
            ht.Add("id_masteruser", param["id_user_master"].ToString());
            string buyUrl = BusinessFactory.Tb_Shop.GetBuyServiceUrl(ht);
            if (string.IsNullOrEmpty(buyUrl))
                buyUrl = PublicSign.cyBuyServiceUrl;
            buyUrl = HttpUtility.UrlEncode(buyUrl, Encoding.UTF8);
            return buyUrl;
        }

        public BaseResult GetUserInfo(Hashtable param)
        {
            BaseResult br = new BaseResult() { Success = false };

            string id_masteruser = param["id_masteruser"].ToString();

            if (DataCache.Get(id_masteruser.ToString() + "_GetServiceBaseInfo") != null && ((Hashtable)DataCache.Get(id_masteruser.ToString() + "_GetServiceBaseInfo")).Keys.Count > 0)
            {
                var serviceBaseInfo = (Hashtable)DataCache.Get(id_masteruser.ToString() + "_GetServiceBaseInfo");
                br.Success = true;
                br.Data = serviceBaseInfo;
            }
            else
            {
                Hashtable result = new Hashtable();
                Hashtable ht = new Hashtable();
                ht.Add("id_masteruser", id_masteruser);
                ht.Add("id", id_masteruser);
                var brUser = BusinessFactory.Account.GetUserInfo(ht);
                if (brUser.Success)
                {
                    var user = (Tb_User)brUser.Data;
                    //此处需要赋值给缓存
                    result.Add("user_master", user);
                    br.Success = true;
                    br.Data = result;

                    if (DataCache.Get(id_masteruser.ToString() + "_GetServiceBaseInfo") == null || ((Hashtable)DataCache.Get(id_masteruser.ToString() + "_GetServiceBaseInfo")).Keys.Count > 0)
                    {
                        DataCache.Add(id_masteruser.ToString() + "_GetServiceBaseInfo", result);
                    }
                }
            }
            return br;
        }



        #region GetUserSelfShop
        protected Tb_User_ShopWithShopMc GetUserSelfShop(string id_user_master,string id_shop,string id_user)
        {
            Tb_User_ShopWithShopMc model = null;
            #region 添加登陆者当前门店信息
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            param.Add("id", id_shop);
            var brSelfShop = BusinessFactory.Tb_Shop.Get(param);
            if (brSelfShop.Success)
            {
                Tb_Shop selfShop = (Tb_Shop)brSelfShop.Data;
                if (selfShop != null && !string.IsNullOrEmpty(selfShop.id))
                {
                    model = new Tb_User_ShopWithShopMc()
                    {
                        id_masteruser = id_user_master,
                        id_shop = id_shop,
                        id_user = id_user,
                        mc = selfShop.mc,
                        bm = selfShop.bm,
                        rq_create = selfShop.rq_create
                    };
                }
            }
            #endregion
            return model;
        }
        #endregion

        #region 是否自动审核
        public bool AutoAudit(string id_user_master,string id_shop,string id_shop_master)
        {
            var autoAudit = GetAutoAudit(id_user_master, id_shop, id_shop_master);
            if (int.Parse(autoAudit["bill_auto_audit"].ToString()) == 1)
                return true;
            else
                return false;
        }
        #endregion

        #region 是否自动审核 实现方法
        /// <summary>
        /// 是否自动审核
        /// lz 2016-09-06
        /// </summary>
        /// <returns></returns>
        public Hashtable GetAutoAudit(string id_user_master, string id_shop, string id_shop_master)
        {
            return BusinessFactory.Account.GetAutoAudit(id_user_master, id_shop, id_shop_master);
        }
        #endregion

        #region GetNewDH
        public string GetNewDH(Enums.FlagDJLX type, string id_user_master, string id_shop)
        {
            if (type == Enums.FlagDJLX.BMShop)
            {
                //门店编码
                string dh = "";
                Hashtable ht = new Hashtable();
                ht.Add("id_masteruser", id_user_master);
                var br = BusinessFactory.Tb_Shop.GetMaxBMInfo(ht);
                if (br.Success)
                {
                    dh = ((Tb_Shop)br.Data).bm.ToString();
                    dh = dh.GetNextStr();
                }
                return dh;
            }
            if (type == Enums.FlagDJLX.BMShopspCZFS)
            {
                //商品编码 称重方式的
                string dh = "";
                Hashtable ht = new Hashtable();
                ht.Add("id_masteruser", id_user_master);
                var br = BusinessFactory.Tb_Shopsp.GetMaxBarcodeInfo(ht);
                if (br.Success)
                {
                    dh = br.Data.ToString();
                    dh = dh.GetNextStr() + "00000";
                }

                return dh.GetJYStr();
            }
            else
            {
                string dh = BusinessFactory.Account.GetNewDH(id_user_master, id_shop, type);
                return dh;
            }
        }
        #endregion


    }
}
