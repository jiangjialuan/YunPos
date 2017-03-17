using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Controllers.Base;
using System.Web.Mvc;
using CySoft.Controllers.Filters;
using System.Web.UI;
using System.Collections;
using System.Web.Routing;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Utility;
using CySoft.Model;
using System.Web;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Ts;

namespace CySoft.Controllers.ManagerCtl
{
    //[LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ManagerController : BaseController
    {
        [LoginActionFilter]
        [ActionPurview(false)]
        public ActionResult Home()
        {
            var v = version;
            Hashtable param = base.GetParameters();

            //if (!param.ContainsKey("controller") || !param.ContainsKey("action"))
            //{
            //TODO：菜单查库
            #region 后期查库

            int index = 0;
            List<Tb_Menu> list_menu = new List<Tb_Menu>();



            #endregion 后期查库

            //查询用户所有的菜单权限树
            var userPurview = BusinessFactory.AccountFunction.GetUserMenu(id_user);
            if (userPurview != null)
            {
                var tree = userPurview.Data as List<Tb_Function_Tree>;
                var role_str = GetLoginInfo<string>("role_str");
                var isManager = role_str.Split(',').ToList().Any(a => a == "2");
                if (tree != null && tree.Any())
                {
                    var menus = tree[0].children;
                    //遍历菜单权限树
                    foreach (var menu in menus)
                    {
                        if ((menu.version + "").Split(',').All(a => a != v)) continue;
                        List<Tb_Menu_Item> menuItems = new List<Tb_Menu_Item>();
                        if (menu.children.Any())
                        {
                            foreach (var subItem in menu.children)
                            {
                                if ((subItem.version + "").Split(',').All(a => a != v)) continue;
                                 
                                //if (!string.IsNullOrEmpty(id_shop)
                                //    && id_shop != id_shop_master
                                //    && (
                                //        subItem.controller_name.ToLower() == "psck"
                                //        || subItem.controller_name.ToLower() == "psfprk"
                                //        || subItem.controller_name.ToLower() == "pscktzd"
                                //        || subItem.controller_name.ToLower() == "psfptzd"
                                //        ) //&& !isManager
                                //    )
                                //{
                                //    continue;
                                //}
                                if (!(id_shop_info.flag_type == 1 || id_shop_info.flag_type == 2)
                                    && (
                                        subItem.controller_name.ToLower() == "psck"
                                        || subItem.controller_name.ToLower() == "psfprk"
                                        || subItem.controller_name.ToLower() == "pscktzd"
                                        || subItem.controller_name.ToLower() == "psfptzd"
                                    )
                                    )
                                {
                                    continue;
                                }

                                if (id_shop_info.flag_type != 1
                                    && subItem.controller_name.ToLower() == "shop" && subItem.action_name.ToLower() == "shopinfo") { continue; }

                                if (!(id_shop_info.flag_type == 1 || id_shop_info.flag_type == 2)
                                    && subItem.controller_name.ToLower() == "shop" &&  subItem.action_name.ToLower() == "list") { continue; }


                                menuItems.Add(new Tb_Menu_Item()
                                {
                                    ActionName = subItem.action_name,
                                    ControllerName = subItem.controller_name,
                                    HasRemark = false,
                                    ID = subItem.id,
                                    Name = subItem.name,
                                    Title = subItem.name,
                                    TabTitle = subItem.name,
                                    Remark = subItem.name,
                                    TagName = subItem.tag_name,
                                    sort_id = subItem.sort_id == null ? 10000000 : subItem.sort_id.Value
                                });
                            }
                        }
                        list_menu.Add(new Tb_Menu()
                        {
                            ID = menu.id,
                            Icon = menu.icon,
                            Name = menu.name,
                            Title = menu.name,
                            Items = menuItems
                        });
                    }
                }
            }

            ViewData["list_menu"] = list_menu;
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", id_user_master);
            ht.Add("id_shop", id_shop);
            var date = DateTime.Now;
            ht.Add("lgrq", new DateTime(date.Year, date.Month, date.Day));
            ht.Add("bgrq", new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999));
            ViewData["ShowData"] = BusinessFactory.Td_Ls_1.Get(ht).Data;
            ViewData["date_type"] = "0";

            param.Clear();
            param.Add("flag_stop", 0);
            param.Add("flag_type", "action");
            ViewData["dialog_tab_ids"] = BusinessFactory.Function.GetAll(param).Data;

            #region 验服务以及获取购买服务地址

            if (PublicSign.flagCheckService == "1")
            {
                var bm = BusinessFactory.Account.GetServiceBM(version);
                if (!string.IsNullOrEmpty(bm))
                {
                    ht.Clear();
                    ht.Add("id_cyuser", id_cyuser);
                    ht.Add("bm", bm);
                    ht.Add("service", "GetService");
                    ht.Add("id_masteruser", id_user_master);
                    ht.Add("rq_create_master_shop", rq_create_master_shop.ToString());
                    var cyServiceHas = BusinessFactory.Account.GetCYService(ht);
                    if (cyServiceHas != null && cyServiceHas.ContainsKey("cyServiceList") && cyServiceHas.ContainsKey("endTime"))
                    {
                        var cyServiceList = (List<Schedule_UserService>)cyServiceHas["cyServiceList"];
                        if (cyServiceList.Count() == 0)
                        {
                            ViewData["vEndData"] = cyServiceHas["endTime"].ToString();
                        }
                        else
                        {
                            DateTime dt = DateTime.Now;
                            if (DateTime.TryParse(cyServiceHas["endTime"].ToString(), out dt))
                            {
                                if (dt > DateTime.Parse("1900-01-01 00:00:00"))
                                {
                                    ViewData["vEndData"] = cyServiceHas["endTime"].ToString();
                                }
                            }
                        }
                    }

                    ht.Clear();
                    ht.Add("id_cyuser", id_cyuser);
                    ht.Add("id", bm);
                    ht.Add("phone", phone_master);
                    ht.Add("service", "Detail");
                    ht.Add("id_masteruser", id_user_master);
                    string buyUrl = BusinessFactory.Tb_Shop.GetBuyServiceUrl(ht);
                    if (string.IsNullOrEmpty(buyUrl))
                        buyUrl = PublicSign.cyBuyServiceUrl;
                    ViewData["buyUrl"] = buyUrl;
                }
            }
            #endregion

            ViewData["downUrl"] = PublicSign.downUrl;
            ViewData["cusName"] = System.Configuration.ConfigurationManager.AppSettings["CUSTOMER_YUNPOS_CUS"];
            ViewData["cusPhone"] = System.Configuration.ConfigurationManager.AppSettings["CUSTOMER_YUNPOS_PHONE"];
            ViewData["cusTel"] = System.Configuration.ConfigurationManager.AppSettings["CUSTOMER_YUNPOS_TEL"];
            ViewData["cusQQ"] = System.Configuration.ConfigurationManager.AppSettings["CUSTOMER_YUNPOS_QQ"];
            ViewData["cusEamil"] = System.Configuration.ConfigurationManager.AppSettings["CUSTOMER_YUNPOS_EMAIL"];
           
            //生成购买历史的 登录地址
            var paramters = new Dictionary<string, string>();
            paramters.Add("uid", id_cyuser);
            string ps = MD5Encrypt.Encode(Encoding.UTF8, "cy.$" + id_cyuser + "+#" + phone_master + "*" + DateTime.Now.ToString("yyyyMMddHH"));
            paramters.Add("ps", ps);
            string mySign = SignUtils.SignRequestForCyUserSys(paramters, PublicSign.md5KeyBusiness);
            paramters.Add("sign", mySign);
            string url = PublicSign.cyBuyServiceHistoryUrl + "?" + WebUtils.BuildQuery2(paramters);
            ViewData["cyBuyServiceHistoryUrl"] = url;


            int pageIndex = 0;
            int limit = 10;
            param.Clear();
            param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            param.Add("page", pageIndex);
            param.Add("pageSize", limit);
            param.Add("sort", "rq_create desc");
            param.Add("limit", limit);
            param.Add("start", pageIndex * limit);
            PageNavigate pn = new PageNavigate();
            pn = BusinessFactory.Ts_Notice.GetPage(param);
            var plist = new PageList<Ts_Notice_View>(pn, pageIndex, limit);
            ViewData["notice_list"] = plist;
            ViewData["cyLoginOutUrl"] = PublicSign.cyLoginOutUrl;

            return View("Home");

        }

        private void HandleFunctionTree(Tb_Function_Tree node)
        {
            if (node != null)
            {
                if (node.flag_type == "module")
                {

                }
            }
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult Register()
        {
            return View("Register");
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Shop()
        {
            return View("Shop");
        }

        public ActionResult ShopSP()
        {
            return View("ShopSP");
        }

        public ActionResult ProfileInfo()
        {
            return View("ProfileInfo");
        }
        /// <summary>
        /// 查询首页显示数据
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryPageShowData()
        {
            Hashtable ht = base.GetParameters();
            var date = string.Format("{0}", ht["date"]);
            var type = string.Format("{0}", ht["type"]);//今天0，昨天1，近7天7，本月-1，其它日期-2
            ViewData["date_type"] = type;
            ht.Clear();
            DateTime lgrq = new DateTime(2000, 1, 1);
            DateTime bgrq = new DateTime(2000, 1, 1);
            var nowdate = DateTime.Now;
            if (true)
            {
                switch (type)
                {
                    case "0":
                        lgrq = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day);
                        bgrq = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day, 23, 59, 59, 999);
                        break;
                    case "1":
                        nowdate = nowdate.AddDays(-1);
                        lgrq = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day);
                        bgrq = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day, 23, 59, 59, 999);
                        break;
                    case "7":
                        var begindate = nowdate.AddDays(-6);
                        lgrq = new DateTime(begindate.Year, begindate.Month, begindate.Day);
                        bgrq = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day, 23, 59, 59, 999);
                        break;
                    case "-1":
                        lgrq = new DateTime(nowdate.Year, nowdate.Month, 1);
                        bgrq = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day, 23, 59, 59, 999);
                        break;
                    case "-2":
                        if (!string.IsNullOrEmpty(date))
                        {
                            var rqmodel = JSON.Deserialize<RqModel>(date);
                            if (rqmodel != null)
                            {
                                lgrq = new DateTime(rqmodel.rq_begin.Year, rqmodel.rq_begin.Month, rqmodel.rq_begin.Day);
                                if (rqmodel.rq_begin == new DateTime(1, 1, 1))
                                {
                                    ViewData["rq_begin"] ="";
                                }
                                else
                                {
                                    ViewData["rq_begin"] = lgrq.ToString("yyyy-MM-dd");
                                }
                                var tempDate = new DateTime(2000, 01, 01);
                                var currentDate = DateTime.Now;
                                if (lgrq<tempDate)
                                {
                                    lgrq = tempDate;
                                }
                                if (rqmodel.rq_end == new DateTime(1, 1, 1))
                                {
                                    ViewData["rq_end"] = "";
                                }
                                else
                                {
                                    ViewData["rq_end"] = rqmodel.rq_end.ToString("yyyy-MM-dd");
                                }
                                bgrq = new DateTime(rqmodel.rq_end.Year, rqmodel.rq_end.Month, rqmodel.rq_end.Day, 23, 59, 59, 999);
                                if (bgrq < tempDate)
                                {
                                    bgrq = currentDate;
                                    ViewData["rq_end"] = currentDate.ToString("yyyy-MM-dd");
                                }
                            }
                        }
                        break;
                }
            }
            ht.Add("id_shop", id_shop);
            ht.Add("lgrq", lgrq);
            ht.Add("bgrq", bgrq);
            ht.Add("id_masteruser", id_user_master);
            ViewData["ShowData"] = BusinessFactory.Td_Ls_1.Get(ht).Data;


            int pageIndex = 0;
            int limit = 10;
            ht.Clear();
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            ht.Add("page", pageIndex);
            ht.Add("pageSize", limit);
            ht.Add("sort", "rq_create desc");
            ht.Add("limit", limit);
            ht.Add("start", pageIndex * limit);
            PageNavigate pn = new PageNavigate();
            pn = BusinessFactory.Ts_Notice.GetPage(ht);
            var plist = new PageList<Ts_Notice_View>(pn, pageIndex, limit);
            ViewData["notice_list"] = plist;
            ViewData["cyLoginOutUrl"] = PublicSign.cyLoginOutUrl;



            return View("_Index");
        }

    }
}
