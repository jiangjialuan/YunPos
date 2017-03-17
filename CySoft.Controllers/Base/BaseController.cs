#region Imports
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CySoft.Frame.Common;
using CySoft.Frame.Core;
using CySoft.Utility;
using CySoft.Model.Flags;
using CySoft.Model.Config;
using CySoft.Model.Other;
using System.Web.Routing;
using CySoft.Model.Tb;
using CySoft.Model.Ts;
using CySoft.Model.Enums;
using System.Threading;
using System.Web;
#endregion

namespace CySoft.Controllers.Base
{
    public class BaseHostInfo
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Url { get; set; }
        public string GetUrlBase() { return string.Format("http://{0}{1}", Host, Port.Equals("80") ? "" : ":" + Port); }
    }

    public abstract class BaseController : Controller
    {
        protected static BaseHostInfo myHostInfo = new BaseHostInfo();

        private string _logAPIUrl = System.Configuration.ConfigurationManager.AppSettings["LogAPIUrl"];

        #region Ip
        protected string Ip
        {
            get { return ControllerContext.HttpContext.Request.UserHostAddress; }
        }
        #endregion

        #region Initialize
        protected override void Initialize(RequestContext requestContext)

        {
            myHostInfo.Host = requestContext.HttpContext.Request.Url.Host.ToLower();
            myHostInfo.Port = requestContext.HttpContext.Request.Url.Port.ToString();
            base.Initialize(requestContext);
        }
        #endregion

        #region 获得登录信息
        /// <summary>
        /// 获得登录信息
        /// 用户名:username
        /// 公司名：companyname
        /// 所属主用户：id_user_master
        /// 是否供应商：flag_supplier
        /// 供应商Id：id_supplier
        /// 供应商是否停用：flag_stop_supplier
        /// 是否采购商：flag_buyer
        /// 采购商Id：id_buyer
        /// </summary>
        /// <typeparam name="T">指定返回类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        protected T GetLoginInfo<T>(string key)
        {
            T result = default(T);
            if (!IsLogOn)
            {
                BaseResult br = new BaseResult();
                br.Success = false;
                br.Message.Add("您未登录或登录已超时，请重新登录！");
                br.Level = ErrorLevel.Drump;
                throw new CySoftException(br);
            }
            try
            {

                Hashtable loginInfo = (Hashtable)Session["LoginInfo"];
                if (loginInfo.ContainsKey(key))
                {
                    object value = loginInfo[key];
                    if (value != null && !Convert.IsDBNull(value))
                    {
                        Type type = typeof(T);
                        if (Type.GetTypeCode(type) != TypeCode.Object && Type.GetTypeCode(type) != TypeCode.Empty)
                        {
                            return (T)Convert.ChangeType(value, type);
                        }
                        return (T)value;
                    }
                }
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw new CySoftException("登陆信息获取失败,请联系管理员！", ErrorLevel.Error);
            }
            return result;
        }
        #endregion

        #region 设置登录信息
        /// <summary>
        /// 设置登录信息
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        protected void SetLoginInfo(string key, object value)
        {
            if (!IsLogOn)
            {
                BaseResult br = new BaseResult();
                br.Success = false;
                br.Message.Add("您未登录或登录已超时，请重新登录！");
                br.Level = ErrorLevel.Drump;
                throw new CySoftException(br);
            }
            try
            {
                Hashtable loginInfo = (Hashtable)Session["LoginInfo"];
                loginInfo[key] = value;
                Session["LoginInfo"] = loginInfo;
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw new CySoftException("登陆信息设置失败,请联系管理员！", ErrorLevel.Error);
            }
        }
        #endregion

        #region 是否已登录
        /// <summary>
        /// 是否已登录
        /// </summary>
        protected bool IsLogOn
        {
            get
            {
                object obj;
                if ((obj = Session["LoginInfo"]) == null || !(obj is Hashtable))
                {
                    return false;
                }
                return true;
            }
        }
        #endregion

        #region 获得所有请求参数
        /// <summary>
        /// 获得所有请求参数
        /// </summary>
        protected virtual Hashtable GetParameters()
        {
            Hashtable param = new Hashtable();
            foreach (string item in Request.QueryString.AllKeys)
            {
                if (!item.IsEmpty())
                {
                    param[item] = Request.QueryString[item].Trim();
                }
            }
            foreach (string item in Request.Form.AllKeys)
            {
                if (!item.IsEmpty())
                {
                    param[item] = Request.Form[item].Trim();
                }
            }
            return param;
        }
        #endregion

        #region 获得指定请求参数
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
        #endregion

        #region 页面信息 OnActionExecuted
        // 页面信息
        /// </summary>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!(filterContext.Result is JsonResult) //Ajax请求
                && !(filterContext.Result is ContentResult) //字符串内容返回
                && !(filterContext.Result is RedirectToRouteResult) //重定向
                )
            {
                Hashtable loginInfo = Session["LoginInfo"] != null ? (Hashtable)Session["LoginInfo"] : new Hashtable();
                ViewData["loginInfo.isLogOn"] = loginInfo == null ? false : true;
                ViewData["loginInfo.id_user"] = loginInfo["id_user"] ?? "";
                ViewData["loginInfo.flag_master"] = loginInfo["flag_master"] ?? YesNoFlag.No;

                if (IsLogOn)
                {
                    if (!Request.IsAjaxRequest())
                    {
                        Hashtable param = new Hashtable();
                        param.Add("id_user", loginInfo["id_user"]);
                    }
                    string id_user = loginInfo["id_user"].ToString();
                    BaseResult br2 = BusinessFactory.AccountFunction.GetPurview(id_user);
                    var purviewlist = br2.Data == null ? new List<ControllerModel>() : br2.Data as List<ControllerModel>;
                    var controllerName = ControllerContext.RouteData.Values["controller"];
                    if (purviewlist.Any())
                    {
                        var sub = purviewlist.FirstOrDefault(pl => pl.name.ToLower() == controllerName.ToString().ToLower());
                        ViewData["actionlist"] = sub == null ? new List<string>() : sub.actions;
                    }
                    ViewData["_DataPurview_"] = GetDataPurview(id_user);
                    ViewData["_IsDataShow_"] = new Func<string, List<string>, bool>(IsDataShow);
                    ViewData["_IsPermissionShow_"] = new Func<string, List<string>, bool>(IsPermissionShow);
                    ViewData["UserRole"] = UserRole;

                    ViewData["version"] = version;// 10 单店  连锁 20 
                    ViewData["yanshi"] = string.Format("{0}", loginInfo["yanshi"] ?? "false");
                    ViewData["loginInfo.id_shop"] = id_shop;// 10 单店  连锁 20 
                    ViewData["loginInfo.id_shop_master"] = id_shop_master;//主门店id
                    if (!string.IsNullOrEmpty(PublicSign.show_shop_version) && !string.IsNullOrEmpty(version) && PublicSign.show_shop_version.Contains("," + version + ","))
                        ViewData["show_shop_version"] = "1";//显示下拉门店的版本

                }
            }
            base.OnActionExecuted(filterContext);
        }
        #endregion

        #region GetDataPurview
        private List<string> GetDataPurview(string id_user)
        {
            List<string> list = new List<string>()
            {
                "phone"
            };
            return list;
        }
        #endregion

        #region 写日志

        #region WriteDBLog
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="flag_lx">日志类型</param>
        /// <param name="content">内容</param>
        protected void WriteDBLog(LogFlag flag_lx, string content)
        {
            Hashtable loginInfo = (Hashtable)(Session["LoginInfo"] ?? new Hashtable());
            BusinessFactory.Log.Add(loginInfo, flag_lx, content);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="flag_lx">日志类型</param>
        /// <param name="messageList">消息</param>
        protected void WriteDBLog(LogFlag flag_lx, IList<string> messageList)
        {
            Hashtable loginInfo = (Hashtable)(Session["LoginInfo"] ?? new Hashtable());
            BusinessFactory.Log.Add(loginInfo, flag_lx, messageList);
        }

        #endregion

        #region 备注
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="flag_lx">日志类型</param>
        /// <param name="messageList">消息</param>
        //protected void WriteDBLog(LogFlag flag_lx, IList<string> messageList)
        //{
        //    Hashtable loginInfo = (Hashtable)(Session["LoginInfo"] ?? new Hashtable());
        //    BusinessFactory.Log.Add(loginInfo, flag_lx, messageList);
        //} 
        #endregion

        protected void WriteDBLog(string name, Hashtable param, BaseResult br)
        {
            this.RetAddLog(name, param, br);
        }

        protected void WriteDBLogForNoLogin(string name, string id_user_, Hashtable param, BaseResult br)
        {
            try
            {
                string content = "业务 (" + name + ") " + Request.RawUrl + "  接受参数:" + TurnParametersStr(param) + "  返回参数:" + JSON.Serialize(br);
                Hashtable ht = new Hashtable();
                ht.Add("context", System.Web.HttpContext.Current);
                ht.Add("id_user", id_user_);
                ht.Add("content", content);
                HttpContext context = System.Web.HttpContext.Current;
                ht.Add("ip", context.Request.UserHostAddress);
                //LogHelper.Info("ip:"+context.Request.UserHostAddress);
                //LogHelper.Info("content:" + content);
                ThreadPool.QueueUserWorkItem(new WaitCallback(HandOutDataToSvr), ht);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region 调用日志接口新增业务日志 RetAddLog
        /// <summary>
        /// 调用日志接口新增业务日志
        /// </summary>
        /// <param name="param">接受的参数</param>
        /// <param name="br">返回的参数</param>
        public void RetAddLog(string name, Hashtable param, BaseResult br)
        {
            try
            {
                string content = "业务 (" + name + ") " + Request.RawUrl + "  接受参数:" + TurnParametersStr(param) + "  返回参数:" + JSON.Serialize(br);
                Hashtable ht = new Hashtable();
                ht.Add("context", System.Web.HttpContext.Current);
                ht.Add("id_user", id_user);
                ht.Add("content", content);
                HttpContext context = System.Web.HttpContext.Current;
                ht.Add("ip", context.Request.UserHostAddress);
                //LogHelper.Info("ip:"+context.Request.UserHostAddress);
                //LogHelper.Info("content:" + content);
                ThreadPool.QueueUserWorkItem(new WaitCallback(HandOutDataToSvr), ht);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region 将Hashtable转换为Str TurnParametersStr
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
        #endregion

        #region 线程池方法处理--增加日志
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

        #region 当前登录用户的角色
        /// <summary>
        /// 当前登录用户的角色
        /// </summary>
        protected RoleFlag UserRole
        {
            get
            {
                return this.GetLoginInfo<RoleFlag>("flag_user_role");//默认是供应商
            }
        }
        #endregion

        #region 默认分页每页条数 PageSizeFromCookie
        protected int PageSizeFromCookie
        {
            get
            {
                var pagesize = HttpContext.Request.Cookies["pagesize_cookie"];
                return pagesize == null
                    ? 10
                    : Utility.Globals.ConvertStringToInt(pagesize.Value, 10);
            }
        }
        #endregion

        #region YZQ Test
        /// <summary>
        /// YZQ Test
        /// </summary>
        protected Dictionary<string, object> UserData
        {
            get
            {
                return new Dictionary<string, object>
                {
                    { "ID", "11111111-1111-1111-1111-111111111111" },
                    { "Name", "YZQ" },
                };
            }
        }
        #endregion

        #region JsonString
        protected ActionResult JsonString(dynamic obj)
        {
            return Content(content: Utility.JSON.Serialize(obj), contentType: "application/json",
                contentEncoding: Encoding.UTF8);
        }
        #endregion

        #region JsonString
        protected ActionResult JsonString(BaseResult res, int type)
        {
            res = res ?? new BaseResult() { Success = false };
            if (res.Success)
            {
                return JsonString(new
                {
                    status = "success",
                    message = "执行成功,正在载入页面...",
                });
            }
            else
            {
                return JsonString(new
                {
                    status = "error",
                    message = string.Join(";", res.Message),
                });
            }
        }
        #endregion

        #region id_user
        protected string id_user
        {
            get
            {
                return GetLoginInfo<string>("id_user");
            }
        }
        #endregion

        #region companyno
        protected string companyno
        {
            get
            {
                return GetLoginInfo<string>("companyno");
            }
        }
        #endregion

        #region id_user
        protected byte? flag_industry
        {
            get
            {
                return GetLoginInfo<byte?>("flag_industry");
            }
        }
        #endregion

        protected Tb_Shop id_shop_info
        {
            get
            {
                return GetLoginInfo<Tb_Shop>("id_shop_info");
            }
        }

        #region id_user_master
        protected string id_user_master
        {
            get
            {
                return GetLoginInfo<string>("id_user_master");
            }
        }
        #endregion

        #region flag_type_shop
        protected string flag_type_shop
        {
            get
            {
                return GetLoginInfo<string>("flag_type_shop");
            }
        }
        #endregion

        #region bm_shop
        protected string bm_shop
        {
            get
            {
                return GetLoginInfo<string>("bm_shop");
            }
        }
        #endregion

        #region id_cyuser
        protected string id_cyuser
        {
            get
            {
                return GetLoginInfo<string>("id_cyuser");
            }
        }
        #endregion

        #region phone_master
        /// <summary>
        /// 主用户手机号
        /// </summary>
        protected string phone_master
        {
            get
            {
                return GetLoginInfo<string>("phone_master");
            }
        }
        #endregion

        #region rq_create_master_shop
        /// <summary>
        /// 主用户门店创建日期
        /// </summary>
        protected string rq_create_master_shop
        {
            get
            {
                return GetLoginInfo<string>("rq_create_master_shop");
            }
        }
        #endregion

        #region is_sysmanager
        protected string is_sysmanager
        {
            get
            {
                return GetLoginInfo<string>("is_sysmanager");
            }
        }
        #endregion

        #region id_shop
        protected string id_shop
        {
            get
            {
                return GetLoginInfo<string>("id_shop");
            }
        }
        #endregion

        #region id_shop_master
        protected string id_shop_master
        {
            get
            {
                return GetLoginInfo<string>("id_shop_master");
            }
        }
        #endregion

        #region version
        protected string version
        {
            get
            {
                return GetLoginInfo<string>("version");
            }
        }
        #endregion

        #region HandleResult
        protected BaseResult HandleResult(Func<BaseResult> func)
        {
            BaseResult res = new BaseResult() { Success = false };
            try
            {
                res = func();
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
        #endregion

        #region GetFlagList
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
        #endregion

        #region 获取商品分类的JsonStr
        public BaseResult GetSPFLJsonStr()
        {
            BaseResult br = new BaseResult() { Data = string.Empty };
            try
            {
                Hashtable param = new Hashtable();
                param.Add("id_masteruser", id_user_master);
                param.Add("sort", "sort_id");
                param.Add("dir", "asc");

                br = BusinessFactory.Tb_Spfl.GetAll(param);
                if (br.Success)
                {
                    var rList = (from db in (List<CySoft.Model.Tb_Spfl>)br.Data
                                 select new
                                 {
                                     id = db.id,
                                     pid = db.id_father,
                                     name = db.mc,
                                     is_default = "F"
                                 }).ToList();
                    br.Data = JSON.Serialize(rList);
                }
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            return br;
        }
        #endregion

        #region 获取供应商分类的JsonStr
        public BaseResult GetGYSFLJsonStr()
        {
            BaseResult br = new BaseResult() { Data = string.Empty };
            try
            {
                Hashtable param = new Hashtable();
                param.Add("id_masteruser", id_user_master);
                param.Add("sort", "sort_id");
                param.Add("dir", "asc");
                br = BusinessFactory.Tb_Gysfl.GetAll(param);
                if (br.Success)
                {
                    var rList = (from db in (List<Tb_Gysfl>)br.Data
                                 select new
                                 {
                                     id = db.id,
                                     pid = db.id_farther,
                                     name = db.mc,
                                     is_default = "F"
                                 }).ToList();
                    br.Data = JSON.Serialize(rList);
                }
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
                br.Message.Add(ex.Message);
            }
            return br;
        }
        #endregion

        #region 获取客户分类的JsonStr
        public BaseResult GetKHFLJsonStr()
        {
            BaseResult br = new BaseResult() { Data = string.Empty };
            try
            {
                Hashtable param = new Hashtable();
                param.Add("id_masteruser", id_user_master);
                param.Add("sort", "sort_id");
                param.Add("dir", "asc");
                br = BusinessFactory.Tb_Khfl.GetAll(param);
                if (br.Success)
                {
                    var rList = (from db in (List<Tb_Khfl>)br.Data
                                 select new
                                 {
                                     id = db.id,
                                     pid = db.id_farther,
                                     name = db.mc,
                                     is_default = "F"
                                 }).ToList();
                    br.Data = JSON.Serialize(rList);
                }
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
                br.Message.Add(ex.Message);
            }
            return br;
        }
        #endregion

        #region 注释

        //protected List<Tb_User_ShopWithShopMc> GetUserShop(string select_id_user = "")
        //{
        //    Hashtable query_user_shop = new Hashtable();
        //    query_user_shop.Add("id_masteruser", id_user_master);
        //    if (!string.IsNullOrEmpty(select_id_user))
        //        query_user_shop.Add("id_user", select_id_user);
        //    else
        //        query_user_shop.Add("id_user", id_user);
        //    var shopList = BusinessFactory.Tb_User_Shop.GetAll(query_user_shop).Data as List<Tb_User_ShopWithShopMc>;
        //    if (!shopList.Any())
        //    {
        //        Hashtable param = new Hashtable();
        //        param.Add("currentId", id_shop);
        //        var shopshopList = BusinessFactory.Tb_Shop_Shop.GetAll(param).Data as List<Tb_Shop_ShopWithMc>;
        //        if (shopshopList.Any())
        //        {
        //            shopList = new List<Tb_User_ShopWithShopMc>();
        //            shopshopList.ForEach(ssl =>
        //            {
        //                shopList.Add(new Tb_User_ShopWithShopMc()
        //                {
        //                    id_masteruser = ssl.id_masteruser,
        //                    id_shop = ssl.id,
        //                    id_user = id_user,
        //                    mc = ssl.mc,
        //                    bm = ssl.bm
        //                });
        //            });
        //        }
        //    }
        //    return shopList;
        //}

        #endregion

        #region 获取门店关系  统一入口
        /// <summary>
        /// 获取门店关系统一入口
        /// lz 2016-11-15
        /// </summary>
        /// <returns></returns>
        protected List<Tb_User_ShopWithShopMc> GetShop(Enums.ShopDataType dataType, string select_id_user = "")
        {
            List<Tb_User_ShopWithShopMc> list = new List<Tb_User_ShopWithShopMc>();
            Hashtable param = new Hashtable();
            param.Add("flag_delete", Enums.FlagDelete.NoDelete);
            param.Add("id_masteruser", id_user_master);
            switch (dataType)
            {
                case Enums.ShopDataType.All_State:
                    #region
                    var vallstate = BusinessFactory.Tb_Shop.GetAll(param).Data as IList<Tb_Shop>;
                    if (vallstate.Any())
                    {
                        vallstate.ToList().ForEach(a =>
                        {
                            list.Add(new Tb_User_ShopWithShopMc()
                            {
                                id_shop = a.id,
                                id_masteruser = a.id_masteruser,
                                bm = a.bm,
                                mc = a.mc
                            });
                        });
                    }
                    #endregion
                    break;
                case Enums.ShopDataType.All:
                    #region
                    param.Add("flag_state", (byte)Enums.TbShopFlagState.Opened);
                    var val = BusinessFactory.Tb_Shop.GetAll(param).Data as IList<Tb_Shop>;
                    if (val.Any())
                    {
                        val.ToList().ForEach(a =>
                        {
                            list.Add(new Tb_User_ShopWithShopMc()
                            {
                                id_shop = a.id,
                                id_masteruser = a.id_masteruser,
                                bm = a.bm,
                                mc = a.mc
                            });
                        });
                    }
                    #endregion
                    break;
                case Enums.ShopDataType.NotExistMaster:
                    #region
                    param.Add("not_id", id_shop_master);
                    param.Add("flag_state", (byte)Enums.TbShopFlagState.Opened);
                    val = BusinessFactory.Tb_Shop.GetAll(param).Data as IList<Tb_Shop>;
                    if (val.Any())
                    {
                        val.ToList().ForEach(a =>
                        {
                            list.Add(new Tb_User_ShopWithShopMc()
                            {
                                id_shop = a.id,
                                id_masteruser = a.id_masteruser,
                                bm = a.bm,
                                mc = a.mc
                            });
                        });
                    }
                    #endregion
                    break;
                case Enums.ShopDataType.UserShopOnly:
                    #region
                    list = this.GetUserShopOnlyNone(select_id_user);
                    //添加自己门店
                    if (list.Where(d => d.id_masteruser == id_user_master && d.id_user == id_user).Count() <= 0)
                    {
                        var selfModel = GetUserSelfShop(select_id_user);
                        if (selfModel != null)
                            list.Add(selfModel);
                    }
                    #endregion
                    break;
                case Enums.ShopDataType.UserShopOnlyNone:
                    #region
                    list = this.GetUserShopOnlyNone(select_id_user);
                    #endregion
                    break;
                case Enums.ShopDataType.UserShop:
                    #region
                    list = this.GetUserShop(select_id_user);
                    #endregion
                    break;
                case Enums.ShopDataType.ShopShop:
                    #region
                    list = this.GetUserShopShop(select_id_user);
                    #endregion
                    break;
                case Enums.ShopDataType.ShopShopAll:
                    #region
                    list = this.GetUserShopShopAll(select_id_user);
                    #endregion
                    break;
                case Enums.ShopDataType.GetPSZXListForAdd:
                    #region
                    list = this.GetPSZXListForAdd(select_id_user);
                    #endregion
                    break;
                case Enums.ShopDataType.GetPSZXListForEdit:
                    #region
                    list = this.GetPSZXListForEdit(select_id_user);
                    #endregion
                    break;
                case Enums.ShopDataType.GetBJXJList:
                    #region
                    list = this.GetBJXJList(select_id_user);
                    #endregion
                    break;
                case Enums.ShopDataType.GetFatherList:
                    #region
                    list = this.GetFatherList(select_id_user);
                    #endregion
                    break;

            }
            return list;
        }

        /// <summary>
        /// 查询当前登录用户管理门店
        /// </summary>
        /// <returns></returns>
        protected List<Tb_User_ShopWithShopMc> GetCurrentUserMgrShop()
        {
            List<Tb_User_ShopWithShopMc> list = new List<Tb_User_ShopWithShopMc>();
            if (id_shop == id_shop_master)
            {
                return GetShop(Enums.ShopDataType.UserShop);
            }
            return GetShop(Enums.ShopDataType.UserShopOnly);
        }
        /// <summary>
        /// liaodong获取用户管理门店
        /// </summary>
        /// <param name="id_user_in">用户ID</param>
        /// <param name="id_shop_in">门店ID</param>
        /// <returns></returns>
        protected List<Tb_User_ShopWithShopMc> GetCurrentUserMgrShop(string id_user_in, string id_shop_in)
        {
            List<Tb_User_ShopWithShopMc> list = new List<Tb_User_ShopWithShopMc>();
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            param.Add("id_user", id_user_in);
            var usershopList = BusinessFactory.Tb_User_Shop.GetAll(param).Data as List<Tb_User_ShopWithShopMc>;
            if (!usershopList.Any())
            {
                param.Clear();
                param.Add("id_masteruser", id_user_master);
                param.Add("tb_shop_state", (byte)Enums.TbShopFlagState.Opened);
                param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                param.Add("id_shop_father", id_shop_in);
                var shopshopList = BusinessFactory.Tb_Shop_Shop.GetAll(param).Data as List<Tb_Shop_ShopWithMc>;
                if (shopshopList.Any())
                {
                    shopshopList.ForEach(s =>
                    {
                        list.Add(new Tb_User_ShopWithShopMc()
                        {
                            id_shop = s.id_shop_child,
                            bm = s.bm,
                            mc = s.mc,
                            flag_type = s.flag_type,
                            id_shop_father = s.id_shop_father
                        });
                    });
                }

            }
            else
            {
                usershopList.ForEach(s =>
                {
                    list.Add(new Tb_User_ShopWithShopMc()
                    {
                        id_shop = s.id_shop,
                        bm = s.bm,
                        mc = s.mc,
                        flag_type = s.flag_type,
                        id_shop_father = s.id_shop_father
                    });
                });
            }


            if (list.All(l => l.id_shop != id_shop_in))
            {
                param.Clear();
                param.Add("id", id_shop_in);
                var shoplist = BusinessFactory.Tb_Shop.QueryShopListWithFatherId(param).Data as List<Tb_ShopWithFatherId>;
                if (shoplist != null && shoplist.Any())
                {
                    var selfModel = shoplist.FirstOrDefault();
                    list.Add(new Tb_User_ShopWithShopMc()
                    {
                        id_shop = selfModel.id,
                        bm = selfModel.bm,
                        mc = selfModel.mc,
                        flag_type = selfModel.flag_type.Value,
                        id_shop_father = selfModel.id_shop_father
                    });
                }
            }
            return list.OrderBy(a => a.bm).ToList();
        }

        /// <summary>
        /// 查询当前登录用户管理门店ID_Shop
        /// </summary>
        /// <returns></returns>
        protected Array GetCurrentUserMgrShopIdArray()
        {
            var list = GetCurrentUserMgrShop(id_user, id_shop);
            return (from s in list select s.id_shop).ToArray();

        }
        #endregion

        #region 获取Tb_User_Shop信息(用户管理门店)
        /// <summary>
        /// 获取Tb_User_Shop信息(用户管理门店)
        /// lz 2016-11-07
        /// </summary>
        /// <returns></returns>
        protected List<Tb_User_ShopWithShopMc> GetUserShop(string select_id_user = "")
        {
            var shopList = this.GetUserShopOnlyNone(select_id_user);
            #region 如果查询结果不存在则获取所有门店 （主门店 可选择所有的 子门店则只能选择自己的）
            if (shopList.Count() <= 0 && id_shop == id_shop_master)
            {
                Hashtable param = new Hashtable();
                param.Add("id_masteruser", id_user_master);
                param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                param.Add("flag_state", (byte)Enums.TbShopFlagState.Opened);
                var brSelfShop = BusinessFactory.Tb_Shop.GetAll(param);
                if (brSelfShop.Success)
                {
                    List<Tb_Shop> selfShopList = (List<Tb_Shop>)brSelfShop.Data;
                    foreach (var selfShop in selfShopList)
                    {
                        if (selfShop != null && !string.IsNullOrEmpty(selfShop.id))
                        {
                            shopList.Add(new Tb_User_ShopWithShopMc()
                            {
                                id_masteruser = id_user_master,
                                id_shop = selfShop.id,
                                id_user = id_user,
                                mc = selfShop.mc,
                                bm = selfShop.bm,
                                rq_create = selfShop.rq_create
                            });
                        }
                    }
                }
            }
            #endregion
            #region 添加登陆者当前门店信息
            if (shopList.Where(d => d.id_masteruser == id_user_master && d.id_shop == id_shop).Count() <= 0)
            {
                var selfModel = GetUserSelfShop(select_id_user);
                if (selfModel != null)
                    shopList.Add(selfModel);
            }
            #endregion
            return shopList;
        }

        #endregion

        #region 获取Tb_User_Shop信息
        /// <summary>
        /// 获取Tb_User_Shop信息
        /// lz 2016-11-07
        /// </summary>
        /// <returns></returns>
        protected List<Tb_User_ShopWithShopMc> GetUserShopOnlyNone(string select_id_user = "")
        {
            #region 获取Tb_User_Shop信息
            Hashtable query_user_shop = new Hashtable();
            query_user_shop.Add("id_masteruser", id_user_master);
            if (!string.IsNullOrEmpty(select_id_user))
                query_user_shop.Add("id_user", select_id_user);
            else
                query_user_shop.Add("id_user", id_user);
            var shopList = BusinessFactory.Tb_User_Shop.GetAll(query_user_shop).Data as List<Tb_User_ShopWithShopMc>;
            #endregion
            return shopList;
        }
        #endregion

        #region GetUserSelfShop
        protected Tb_User_ShopWithShopMc GetUserSelfShop(string select_id_user = "")
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
                    if (selfShop.flag_type != null)
                    {
                        model.flag_type = selfShop.flag_type.Value;
                    }
                }
            }
            #endregion
            return model;
        }
        #endregion

        #region 获取Tb_Shop_Shop信息
        /// <summary>
        /// 获取Tb_Shop_Shop信息
        /// lz 2016-11-07
        /// </summary>
        /// <returns></returns>
        protected List<Tb_User_ShopWithShopMc> GetUserShopShop(string select_id_user = "")
        {
            #region 获取Tb_Shop_Shop信息
            List<Tb_User_ShopWithShopMc> shopList = new List<Tb_User_ShopWithShopMc>();

            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            //param.Add("currentId", id_shop);
            if (id_shop_master != id_shop)
                param.Add("shopShopID", id_shop);
            else
                param.Add("shopShopID", "0");

            param.Add("tb_shop_state", (byte)Enums.TbShopFlagState.Opened);
            param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);

            var shopshopList = BusinessFactory.Tb_Shop_Shop.GetAll(param).Data as List<Tb_Shop_ShopWithMc>;
            if (shopshopList.Any())
            {
                shopshopList.ForEach(ssl =>
                {
                    shopList.Add(new Tb_User_ShopWithShopMc()
                    {
                        id_masteruser = ssl.id_masteruser,
                        id_shop = ssl.id_shop_child,
                        id_user = id_user,
                        mc = ssl.mc,
                        bm = ssl.bm,
                        rq_create = ssl.rq_create
                    });
                });
            }

            #endregion
            #region 添加登陆者当前门店信息
            if (shopList.Where(d => d.id_masteruser == id_user_master && d.id_shop == id_shop).Count() <= 0)
            {
                param.Clear();
                param.Add("id_masteruser", id_user_master);
                param.Add("id", id_shop);

                var brSelfShop = BusinessFactory.Tb_Shop.Get(param);
                if (brSelfShop.Success)
                {
                    Tb_Shop selfShop = (Tb_Shop)brSelfShop.Data;
                    if (selfShop != null && !string.IsNullOrEmpty(selfShop.id))
                    {
                        shopList.Add(new Tb_User_ShopWithShopMc()
                        {
                            id_masteruser = id_user_master,
                            id_shop = id_shop,
                            id_user = id_user,
                            mc = selfShop.mc,
                            bm = selfShop.bm,
                            rq_create = selfShop.rq_create
                        });
                    }
                }
            }
            #endregion
            return shopList;
        }
        #endregion

        #region 获取Tb_Shop_ShopALL信息
        /// <summary>
        /// 获取Tb_Shop_ShopALL信息
        /// lz 2016-11-07
        /// </summary>
        /// <returns></returns>
        protected List<Tb_User_ShopWithShopMc> GetUserShopShopAll(string select_id_user = "")
        {
            #region 获取Tb_Shop_Shop信息
            List<Tb_User_ShopWithShopMc> shopList = new List<Tb_User_ShopWithShopMc>();

            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            //param.Add("currentId", id_shop);
            if (id_shop_master != id_shop)
                param.Add("shopShopID", id_shop);
            else
                param.Add("shopShopID", "0");

            //param.Add("tb_shop_state", (byte)Enums.TbShopFlagState.Opened);
            //param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);

            var shopshopList = BusinessFactory.Tb_Shop_Shop.GetAll(param).Data as List<Tb_Shop_ShopWithMc>;
            if (shopshopList.Any())
            {
                shopshopList.ForEach(ssl =>
                {
                    shopList.Add(new Tb_User_ShopWithShopMc()
                    {
                        id_masteruser = ssl.id_masteruser,
                        id_shop = ssl.id_shop_child,
                        id_user = id_user,
                        mc = ssl.mc,
                        bm = ssl.bm,
                        rq_create = ssl.rq_create
                    });
                });
            }

            #endregion
            #region 添加登陆者当前门店信息
            if (shopList.Where(d => d.id_masteruser == id_user_master && d.id_shop == id_shop).Count() <= 0)
            {
                param.Clear();
                param.Add("id_masteruser", id_user_master);
                param.Add("id", id_shop);

                var brSelfShop = BusinessFactory.Tb_Shop.Get(param);
                if (brSelfShop.Success)
                {
                    Tb_Shop selfShop = (Tb_Shop)brSelfShop.Data;
                    if (selfShop != null && !string.IsNullOrEmpty(selfShop.id))
                    {
                        shopList.Add(new Tb_User_ShopWithShopMc()
                        {
                            id_masteruser = id_user_master,
                            id_shop = id_shop,
                            id_user = id_user,
                            mc = selfShop.mc,
                            bm = selfShop.bm,
                            rq_create = selfShop.rq_create
                        });
                    }
                }
            }
            #endregion
            return shopList;
        }
        #endregion



        #region 获取GetPSZXList信息
        #region GetPSZXListForAdd
        /// <summary>
        /// 获取本级 和 子集配送中心的门店信息
        /// lz 2017-02-21
        /// </summary>
        /// <returns></returns>
        protected List<Tb_User_ShopWithShopMc> GetPSZXListForAdd(string id_shop_select)
        {
            #region 获取Tb_Shop_Shop信息
            List<Tb_User_ShopWithShopMc> shopList = new List<Tb_User_ShopWithShopMc>();
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            param.Add("tb_shop_state", (byte)Enums.TbShopFlagState.Opened);
            param.Add("tb_shop_delete", (byte)Enums.FlagDelete.NoDelete);
            #region 获取所有门店关系
            var shopShopAllList = BusinessFactory.Tb_Shop_Shop.GetAll(param).Data as List<Tb_Shop_ShopWithMc>;
            #endregion
            #region 获取本门店信息
            var selfModel = shopShopAllList.Where(d => d.id_shop_child == id_shop_select).FirstOrDefault();
            if (selfModel == null || string.IsNullOrEmpty(selfModel.id))
            {
                return shopList;
            }
            #endregion
            #region 获取子门店
            var pingjiList = shopShopAllList.Where(d => d.id_shop_father == selfModel.id_shop_child).ToList();
            #endregion
            #region 子门店的属于配送中心的门店
            var pjpszxList = pingjiList.Where(d => d.flag_type == (int)Enums.FlagShopType.配送中心 && d.shop_flag_state == (byte)Enums.TbShopFlagState.Opened && d.shop_flag_delete == (byte)Enums.FlagDelete.NoDelete).ToList();
            if (pjpszxList.Any())
            {
                pjpszxList.ForEach(ssl =>
                {
                    shopList.Add(new Tb_User_ShopWithShopMc()
                    {
                        id_masteruser = ssl.id_masteruser,
                        id_shop = ssl.id_shop_child,
                        id_user = id_user,
                        mc = ssl.mc,
                        bm = ssl.bm,
                        rq_create = ssl.rq_create
                    });
                });
            }
            #endregion
            #region 加入本门店
            if (shopList.Where(d => d.id_masteruser == id_user_master && d.id_shop == selfModel.id_shop_child).Count() <= 0)
            {
                if (selfModel != null && !string.IsNullOrEmpty(selfModel.id_shop_child))
                {
                    shopList.Add(new Tb_User_ShopWithShopMc()
                    {
                        id_masteruser = id_user_master,
                        id_shop = selfModel.id_shop_child,
                        id_user = id_user,
                        mc = selfModel.mc,
                        bm = selfModel.bm,
                        rq_create = selfModel.rq_create
                    });
                }
            }
            #endregion
            #endregion
            return shopList;
        }
        #endregion
        #region GetPSZXListForEdit
        /// <summary>
        /// GetPSZXListForEdit
        /// lz 2017-02-21
        /// </summary>
        /// <returns></returns>
        protected List<Tb_User_ShopWithShopMc> GetPSZXListForEdit(string id_shop_select)
        {
            #region 获取Tb_Shop_Shop信息
            List<Tb_User_ShopWithShopMc> shopList = new List<Tb_User_ShopWithShopMc>();
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            #region 获取所有门店关系
            var shopShopAllList = BusinessFactory.Tb_Shop_Shop.GetAll(param).Data as List<Tb_Shop_ShopWithMc>;
            #endregion
            #region 获取本门店信息
            var selfModel = shopShopAllList.Where(d => d.id_shop_child == id_shop_select).FirstOrDefault();
            if (selfModel == null || string.IsNullOrEmpty(selfModel.id))
            {
                return shopList;
            }
            #endregion
            #region 获取父门店
            var fatherModel = new Tb_Shop_ShopWithMc();
            if (selfModel.id_shop_father == "0" || string.IsNullOrEmpty(selfModel.id_shop_father))
            {
                fatherModel = selfModel;
            }
            else
            {
                fatherModel = shopShopAllList.Where(d => d.id_shop_child == selfModel.id_shop_father).FirstOrDefault();
            }
            #endregion
            #region 平级门店
            var pingjiList = shopShopAllList.Where(d => d.id_shop_father == fatherModel.id_shop_child).ToList();
            #endregion
            #region 平级的属于配送中心的门店
            var pjpszxList = pingjiList.Where(d => d.flag_type == (int)Enums.FlagShopType.配送中心 && d.shop_flag_state == (byte)Enums.TbShopFlagState.Opened && d.shop_flag_delete == (byte)Enums.FlagDelete.NoDelete).ToList();
            if (pjpszxList.Any())
            {
                pjpszxList.ForEach(ssl =>
                {
                    shopList.Add(new Tb_User_ShopWithShopMc()
                    {
                        id_masteruser = ssl.id_masteruser,
                        id_shop = ssl.id_shop_child,
                        id_user = id_user,
                        mc = ssl.mc,
                        bm = ssl.bm,
                        rq_create = ssl.rq_create
                    });
                });
            }
            #endregion
            #region 加入父级门店
            if (shopList.Where(d => d.id_masteruser == id_user_master && d.id_shop == fatherModel.id_shop_child).Count() <= 0)
            {
                if (fatherModel != null && !string.IsNullOrEmpty(fatherModel.id_shop_child))
                {
                    shopList.Add(new Tb_User_ShopWithShopMc()
                    {
                        id_masteruser = id_user_master,
                        id_shop = fatherModel.id_shop_child,
                        id_user = id_user,
                        mc = fatherModel.mc,
                        bm = fatherModel.bm,
                        rq_create = fatherModel.rq_create
                    });
                }
            }
            #endregion
            #endregion
            return shopList;
        }
        #endregion
        #endregion

        #region GetPSZXListForAdd
        /// <summary>
        /// 获取本级 和 子集的门店信息
        /// lz 2017-02-21
        /// </summary>
        /// <returns></returns>
        protected List<Tb_User_ShopWithShopMc> GetBJXJList(string id_shop_select)
        {
            #region 获取Tb_Shop_Shop信息
            List<Tb_User_ShopWithShopMc> shopList = new List<Tb_User_ShopWithShopMc>();
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            param.Add("tb_shop_state", (byte)Enums.TbShopFlagState.Opened);
            param.Add("tb_shop_delete", (byte)Enums.FlagDelete.NoDelete);
            #region 获取所有门店关系
            var shopShopAllList = BusinessFactory.Tb_Shop_Shop.GetAll(param).Data as List<Tb_Shop_ShopWithMc>;
            #endregion
            #region 获取本门店信息
            var selfModel = shopShopAllList.Where(d => d.id_shop_child == id_shop_select).FirstOrDefault();
            if (selfModel == null || string.IsNullOrEmpty(selfModel.id))
            {
                return shopList;
            }
            #endregion
            #region 获取子门店
            var pingjiList = shopShopAllList.Where(d => d.id_shop_father == selfModel.id_shop_child).ToList();
            #endregion
            #region 子门店的属于配送中心的门店
            var pjpszxList = pingjiList.Where(d => d.shop_flag_state == (byte)Enums.TbShopFlagState.Opened && d.shop_flag_delete == (byte)Enums.FlagDelete.NoDelete).ToList();
            if (pjpszxList.Any())
            {
                pjpszxList.ForEach(ssl =>
                {
                    shopList.Add(new Tb_User_ShopWithShopMc()
                    {
                        id_masteruser = ssl.id_masteruser,
                        id_shop = ssl.id_shop_child,
                        id_user = id_user,
                        mc = ssl.mc,
                        bm = ssl.bm,
                        rq_create = ssl.rq_create
                    });
                });
            }
            #endregion
            #region 加入本门店
            if (shopList.Where(d => d.id_masteruser == id_user_master && d.id_shop == selfModel.id_shop_child).Count() <= 0)
            {
                if (selfModel != null && !string.IsNullOrEmpty(selfModel.id_shop_child))
                {
                    shopList.Add(new Tb_User_ShopWithShopMc()
                    {
                        id_masteruser = id_user_master,
                        id_shop = selfModel.id_shop_child,
                        id_user = id_user,
                        mc = selfModel.mc,
                        bm = selfModel.bm,
                        rq_create = selfModel.rq_create
                    });
                }
            }
            #endregion
            #endregion
            return shopList;
        }
        #endregion

        #region GetFatherList
        /// <summary>
        /// GetFatherList
        /// lz 2017-02-21
        /// </summary>
        /// <returns></returns>
        protected List<Tb_User_ShopWithShopMc> GetFatherList(string id_shop_select)
        {
            #region 获取Tb_Shop_Shop信息
            List<Tb_User_ShopWithShopMc> shopList = new List<Tb_User_ShopWithShopMc>();
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            #region 获取所有门店关系
            var shopShopAllList = BusinessFactory.Tb_Shop_Shop.GetAll(param).Data as List<Tb_Shop_ShopWithMc>;
            #endregion
            #region 获取本门店信息
            var selfModel = shopShopAllList.Where(d => d.id_shop_child == id_shop_select).FirstOrDefault();
            if (selfModel == null || string.IsNullOrEmpty(selfModel.id))
            {
                return shopList;
            }
            #endregion
            #region 获取父门店
            var fatherModel = new Tb_Shop_ShopWithMc();
            if (selfModel.id_shop_father == "0" || string.IsNullOrEmpty(selfModel.id_shop_father))
            {
                fatherModel = selfModel;
            }
            else
            {
                fatherModel = shopShopAllList.Where(d => d.id_shop_child == selfModel.id_shop_father).FirstOrDefault();
            }
            #endregion

            if (fatherModel != null && !string.IsNullOrEmpty(fatherModel.id_shop_child))
            {
                shopList.Add(new Tb_User_ShopWithShopMc()
                {
                    id_masteruser = id_user_master,
                    id_shop = fatherModel.id_shop_child,
                    id_user = id_user,
                    mc = fatherModel.mc,
                    bm = fatherModel.bm,
                    rq_create = fatherModel.rq_create
                });
            }
            #endregion
            return shopList;
        }
        #endregion

        #region 获取用户的供应商

        protected List<Tb_Gys_User_QueryModel> GetUserGys()
        {
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            var gysList = BusinessFactory.Tb_Gys.GetAll(param).Data as List<Tb_Gys_User_QueryModel>;
            return gysList;
        }
        #endregion

        #region 获取用户的客户
        protected List<Tb_Kh> GetUserKh(string no_bd = "")
        {
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            if(!string.IsNullOrEmpty(no_bd))
                param.Add("no_bd", 1);
            var khList = BusinessFactory.Tb_Kh.GetAll(param).Data as List<Tb_Kh>;
            return khList;
        }
        #endregion

        #region ts_parm表 配置参数
        /// <summary>
        /// ts_parm表 配置参数
        /// lz 2016-09-06
        /// </summary>
        /// <returns></returns>
        public Hashtable GetParm()
        {
            return BusinessFactory.Account.GetParm(id_user_master);
        }
        #endregion

        #region RemoveCache
        public void RemoveCache()
        {
            try
            {
                if (DataCache.Get(id_user_master + "_GetParm") != null && ((Hashtable)DataCache.Get(id_user_master + "_GetParm")).Keys.Count > 0)
                    DataCache.Remove(id_user_master + "_GetParm");
            }
            catch (Exception ex)
            {
            }
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

        #region 是否自动审核
        public bool AutoAudit()
        {
            var autoAudit = GetAutoAudit(id_user_master, id_shop, id_shop_master);
            if (int.Parse(autoAudit["bill_auto_audit"].ToString()) == 1)
                return true;
            else
                return false;
        }
        #endregion

        #region 获取接口签名Ticket
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
                if (!ticketBr.Success && ((Tb_Ticket)ticketBr.Data) != null && !string.IsNullOrEmpty(((Tb_Ticket)ticketBr.Data).id))
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
        #endregion




        protected List<Tb_User> GetUser()
        {
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            param.Add("flag_state", (byte)CySoft.Model.Enums.Enums.TbUserFlagState.Yes);
            var userList = BusinessFactory.Account.GetAllUser(param).Data as List<Tb_User>;
            return userList;
        }
        /// <summary>
        /// 控制界面显示列数据
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="dataNames"></param>
        /// <returns></returns>
        protected bool IsDataShow(string dataName, List<string> dataNames)
        {
            if (!string.IsNullOrEmpty(dataName) && dataNames.Any())
            {
                return dataNames.Any(d => d.ToLower() == dataName.ToLower());
            }
            return false;
        }
        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="permissionName"></param>
        /// <param name="PermissionNames"></param>
        /// <returns></returns>
        protected bool IsPermissionShow(string permissionName, List<string> PermissionNames)
        {
            if (!string.IsNullOrEmpty(permissionName) && PermissionNames.Any())
            {
                return PermissionNames.Any(d => d.ToLower() == permissionName.ToLower());
            }
            return false;
        }


        #region 生成条码 或 单号
        /// <summary>
        /// 生成条码 或 单号
        /// lz
        /// 2016-11-07
        /// </summary>
        /// <param name="type">用于生成单号的类型枚举 </param>
        /// <returns></returns>
        public string GetNewDH(Enums.FlagDJLX type)
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

        #region 获取当前主用户的会员分类
        /// <summary>
        /// 获取当前主用户的会员分类
        /// </summary>
        /// <returns></returns>
        protected List<Tb_Hyfl> GetHyfl()
        {
            List<Tb_Hyfl> hyfls = new List<Tb_Hyfl>();
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            hyfls = BusinessFactory.Tb_Hyfl.GetAll(param).Data as List<Tb_Hyfl>;
            return hyfls;
        }
        #endregion

        #region 统一清理缓存方法
        /// <summary>
        /// 统一清理缓存方法
        /// </summary>
        /// <param name="id_masteruser"></param>
        /// <param name="id_shop"></param>
        public void ClearShopParm(string id_masteruser, string id_shop)
        {
            BusinessFactory.Td_Hy_Czrule_1.ClearShopParm(id_masteruser, id_shop);
        }
        #endregion






    }
}
