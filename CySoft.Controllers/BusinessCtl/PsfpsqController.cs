using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Model.Other;
using CySoft.Model.Td;
using CySoft.Utility.Mvc.Html;

namespace CySoft.Controllers.BusinessCtl
{
     [LoginActionFilter]
    public class PsfpsqController:BaseController
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult List()
        {

            Hashtable param = base.GetParameters();
            try
            {
                #region 获取参数
                int limit = base.PageSizeFromCookie;
                param.Add("id_masteruser", id_user_master);
                ParamVessel pv = new ParamVessel();
                pv.Add("_search_", "0", HandleType.DefaultValue);
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("keyword", "", HandleType.Remove, true);
                pv.Add("s_id_shop", "", HandleType.Remove);
                pv.Add("sort", "rq_create desc", HandleType.DefaultValue);
                pv.Add("start_time", String.Empty, HandleType.Remove);
                pv.Add("start_time_end", String.Empty, HandleType.Remove);
                param = param.Trim(pv);
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);
                if (param.ContainsKey("start_time"))
                {
                    param.Add("start_rq", param["start_time"].ToString());
                    param.Remove("start_time");
                }
                if (param.ContainsKey("start_time_end"))
                {
                    param.Add("end_rq", param["start_time_end"].ToString());
                    param.Remove("start_time_end");
                }
                var userShops = GetCurrentUserMgrShop(id_user, id_shop); 
                if (!param.Contains("s_id_shop")) 
                {
                    if (userShops.Any())
                    {
                        param.Add("user_id_shops", (from s in userShops select s.id_shop).ToArray());
                    }
                }
                #endregion
                PageNavigate pn = new PageNavigate();
                pn = BusinessFactory.Td_Ps_Fpsq_1.GetPage(param);
                var plist = new PageList<Td_Ps_Fpsq_1_Query>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"]; //排序规则需要返回前台  
                ViewBag.DigitHashtable = GetParm();
                ViewData["userShopList"] = userShops;
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            if (param["_search_"].ToString().Equals("1"))
            {
                return PartialView("_List");
            }
            else
            {
                return View();
            }

        }

        #region 新增/复制
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult Add(string id)
        {
            ViewData["option"] = "add";
            //用户管理门店
            ViewData["userShopList"] =GetCurrentUserMgrShop(id_user,id_shop);//GetShop(Enums.ShopDataType.UserShopOnly);
            ViewData["AllShopList"] = GetShop(Enums.ShopDataType.All);
            ViewData["id_shop_master"] = id_shop_master;
            ViewData["IsMasterShop"] = id_shop == id_shop_master;
            //获取前台控制小数点
            ViewBag.DigitHashtable = GetParm();
            //经办人
            ViewData["userList"] = GetUser();
            ViewData["id_user"] = id_user;
              
            ViewData["dh"] = GetNewDH(Enums.FlagDJLX.DHJH);
            Hashtable param = new Hashtable();
            if (!string.IsNullOrEmpty(id))
            {

                param.Clear();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                ViewData["item_edit"] = BusinessFactory.Td_Ps_Fpsq_1.Get(param).Data;
                ViewData["option"] = "copy";
            }
            return View("Edit");
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult Add(FpsqModel model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                model.id_create = id_user;
                //model.id_shop = id_shop;
                model.AutoAudit = AutoAudit();
                var br = BusinessFactory.Td_Ps_Fpsq_1.Add(model);
                return br;
            });
            return JsonString(res, 1);
        }
        #endregion

        #region 编辑
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult Edit(string id)
        {
            ViewData["option"] = "edit";
            //用户管理门店
            ViewData["userShopList"] =GetCurrentUserMgrShop(id_user,id_shop);//GetShop(Enums.ShopDataType.UserShopOnly); //GetUserShop();
            ViewData["AllShopList"] = GetShop(Enums.ShopDataType.All);
            ViewData["id_shop_master"] = id_shop_master;
            ViewData["IsMasterShop"] = id_shop == id_shop_master;
            //获取前台控制小数点
            ViewBag.DigitHashtable = GetParm();
            //经办人
            ViewData["userList"] = GetUser();
            ViewData["id"] = id;
            if (!string.IsNullOrEmpty(id))
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                var model = BusinessFactory.Td_Ps_Fpsq_1.Get(param).Data as FpsqQueryModel;
                if (model != null)
                {
                    ViewData["dh"] = model.Fpsq1.dh;
                    ViewData["zdr_name"] = model.zdr;
                    ViewData["shr_name"] = model.shr;
                }
                ViewData["item_edit"] = model;
                //param.Clear();
                //param.Add("id", id_shop_master);
                //ViewData["shop_master_item"] = BusinessFactory.Tb_Shop.Get(param).Data;
            }
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult Edit(FpsqModel model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                model.id_create = id_user;
                //model.id_shop = id_shop;
                return BusinessFactory.Td_Ps_Fpsq_1.Update(model);
            });
            return JsonString(res, 1);
        }
        #endregion

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult Sh(string id)
        {
            var res = HandleResult(() =>
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                return BusinessFactory.Td_Ps_Fpsq_1.Active(param);
            });
            return JsonString(res, 1);
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult ZF(string id)
        {
            var res = HandleResult(() =>
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                return BusinessFactory.Td_Ps_Fpsq_1.Stop(param);
            });
            return JsonString(res, 1);
        }
         [HttpPost]
         [ActionPurview(true)]
        public ActionResult Delete(string id)
        {
            var res = HandleResult(() =>
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                return BusinessFactory.Td_Ps_Fpsq_1.Delete(param);
            });
            return JsonString(res, 1);
        }
        /// <summary>
        /// 弹框选择查询
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult SearchList()
        {
            Hashtable param = base.GetParameters();
            try
            {
                #region 获取参数
                int limit = base.PageSizeFromCookie;
                param.Add("id_masteruser", id_user_master);
                ParamVessel pv = new ParamVessel();
                pv.Add("_search_", "0", HandleType.DefaultValue);
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("keyword", "", HandleType.Remove, true);
                pv.Add("id_shop_sq", String.Empty, HandleType.Remove);
                pv.Add("s_id_shop_sq", String.Empty, HandleType.Remove);
                pv.Add("sort", "rq_create desc", HandleType.DefaultValue);
                pv.Add("start_time", String.Empty, HandleType.Remove);
                pv.Add("start_time_end", String.Empty, HandleType.Remove);
                param = param.Trim(pv);
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                param.Add("flag_sh", (int)Enums.FlagSh.HadSh);
                param.Add("flag_cancel", (int)Enums.FlagCancel.NoCancel);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);
                if (param.ContainsKey("start_time"))
                {
                    param.Add("start_rq", param["start_time"].ToString());
                    param.Remove("start_time");
                }
                if (param.ContainsKey("start_time_end"))
                {
                    param.Add("end_rq", param["start_time_end"].ToString());
                    param.Remove("start_time_end");
                }
                param.Add("no_th", "1");
                var userShops = GetCurrentUserMgrShop(id_user, id_shop);
                if (param.ContainsKey("s_id_shop_sq"))
                {
                    ViewData["id_shop_sq"] = param["s_id_shop_sq"];
                    param["id_shop_sq"] = param["s_id_shop_sq"];
                    param.Remove("s_id_shop_sq");
                }
                if (!param.Contains("id_shop_sq"))
                {
                    if (userShops.Any())
                    {
                        param.Add("user_id_shops", (from s in userShops select s.id_shop).ToArray());
                    }
                }
                #endregion
                PageNavigate pn = new PageNavigate();
                pn = BusinessFactory.Td_Ps_Fpsq_1.GetPage(param);
                var plist = new PageList<Td_Ps_Fpsq_1_Query>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"]; //排序规则需要返回前台  
                ViewBag.DigitHashtable = GetParm();
                ViewData["id_shop_master"] = id_shop_master;
                ViewData["userShopList"] = userShops;// GetShop(Enums.ShopDataType.UserShop); //GetUserShop();
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            if (param["_search_"].ToString().Equals("1"))
            {
                return PartialView("_SearchList");
            }
            else
            {
                return View();
            }

        }
        /// <summary>
        /// 根据ID查询申请单商品明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult QuerFpsqdSpmx(string id)
        {
            var res = HandleResult(() =>
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                param.Add("forps","");
                return BusinessFactory.Td_Ps_Fpsq_1.Get(param);
            });
            return JsonString(res);
        }

        [HttpGet]
        [ActionPurview(true)]
        [ActionAlias("psfpsq","list")]
        public ActionResult Detial(string id)
        {
            ViewData["option"] = "edit";
            //用户管理门店
            ViewData["userShopList"] = GetCurrentUserMgrShop(id_user, id_shop);//GetUserShop();
            ViewData["AllShopList"] = GetShop(Enums.ShopDataType.All);
            ViewData["id_shop_master"] = id_shop_master;
            ViewData["IsMasterShop"] = id_shop == id_shop_master;
            //获取前台控制小数点
            ViewBag.DigitHashtable = GetParm();
            //经办人
            ViewData["userList"] = GetUser();
            ViewData["id"] = id;
            if (!string.IsNullOrEmpty(id))
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                var model = BusinessFactory.Td_Ps_Fpsq_1.Get(param).Data as FpsqQueryModel;
                if (model != null)
                {
                    ViewData["dh"] = model.Fpsq1.dh;
                    ViewData["zdr_name"] = model.zdr;
                    ViewData["shr_name"] = model.shr;
                }
                ViewData["item_edit"] = model;
            }
            return View();
        }


    }
}
