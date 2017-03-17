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
using CySoft.Model.Tb;
using CySoft.Model.Td;
using CySoft.Model.Ts;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;
using NPOI.POIFS.Storage;

namespace CySoft.Controllers.SalesPromotionCtl
{
    [LoginActionFilter]
    public class PromoteController : BaseController
    {
        #region 辅助方法
        /// <summary>
        /// 获取当前主用户的会员分类
        /// </summary>
        /// <returns></returns>
        private List<Tb_Hyfl> GetHyfl()
        {
            List<Tb_Hyfl> hyfls = new List<Tb_Hyfl>();
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            hyfls = BusinessFactory.Tb_Hyfl.GetAll(param).Data as List<Tb_Hyfl>;
            return hyfls;
        }
        /// <summary>
        /// 获取促销类型
        /// </summary>
        /// <returns></returns>
        private List<Ts_Flag> GetCxlx()
        {
            List<Ts_Flag> flags = new List<Ts_Flag>()
            {
                new Ts_Flag(){listdisplay = "特价促销单",listcode = "CX010"},
                new Ts_Flag(){listdisplay = "组合促销单",listcode = "CX020"},
                new Ts_Flag(){listdisplay = "单品折扣促销单",listcode = "CX110"},
                new Ts_Flag(){listdisplay = "品类折扣促销单",listcode = "CX120"},
                new Ts_Flag(){listdisplay = "单组折扣促销单",listcode = "CX130"},
                new Ts_Flag(){listdisplay = "时段折扣促销单",listcode = "CX140"},
                new Ts_Flag(){listdisplay = "整单折扣促销单",listcode = "CX150"},
                new Ts_Flag(){listdisplay = "单品买减促销单",listcode = "CX210"},
                new Ts_Flag(){listdisplay = "单组买送促销单",listcode = "CX220"},
                new Ts_Flag(){listdisplay = "整类买送促销单",listcode = "CX230"},
                new Ts_Flag(){listdisplay = "整单买送促销单",listcode = "CX240"},
                new Ts_Flag(){listdisplay = "单组加价换购促销单",listcode = "CX310"},
                new Ts_Flag(){listdisplay = "整单加价换购促销单",listcode = "CX320"}
            };
            return flags;
        }

        private List<string> GetShopIds()
        {
            List<Tb_User_ShopWithShopMc> allShop = new List<Tb_User_ShopWithShopMc>();
            //var userShops = GetShop(Enums.ShopDataType.UserShopOnly).OrderBy(a=>a.bm).ToList(); //GetUserShop();
            //if (id_shop == id_shop_master)
            //{
            //    userShops = GetShop(Enums.ShopDataType.UserShop).OrderBy(a => a.bm).ToList();
            //}
            //if (userShops.Any())
            //{
            //    allShop.AddRange(userShops.ToList());
            //}
             allShop = GetCurrentUserMgrShop(id_user, id_shop);
            return (from item in allShop select item.id_shop).ToList();
        }
        private void AddQurryParam(string search_cxd_state, Hashtable param)
        {
            switch (search_cxd_state)
            {
                case "0":
                    param.Add("s_flag_sh", (int)Enums.FlagSh.UnSh);
                    break;
                case "1":
                    param.Add("s_flag_sh", (int)Enums.FlagSh.HadSh);
                    param.Add("not_begin", DateTime.Now);
                    param.Add("flag_cancel", (int)Enums.FlagCancel.NoCancel);
                    break;
                case "2":
                    param.Add("s_flag_sh", (int)Enums.FlagSh.HadSh);
                    param.Add("current_day", DateTime.Now);
                    param.Add("flag_cancel", (int)Enums.FlagCancel.NoCancel);
                    break;
                case "3":
                    param.Add("s_flag_sh", (int)Enums.FlagSh.HadSh);
                    param.Add("had_end", DateTime.Now);
                    param.Add("flag_cancel", (int)Enums.FlagCancel.NoCancel);
                    break;
                case "4":
                    param.Add("s_flag_sh", (int)Enums.FlagSh.HadSh);
                    param.Add("flag_cancel", (int)Enums.FlagCancel.Canceled);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 处理促销单状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected string HandlePromote1State(Td_Promote_1WithUserName model)
        {
            if (model == null) return String.Empty;
            if (model.flag_sh != null && model.flag_sh.Value == (byte)Enums.FlagSh.UnSh)
            {
                return "未审核";
            }
            else if (model.flag_sh != null
                && model.flag_sh.Value == (byte)Enums.FlagSh.HadSh
                && model.flag_cancel.Value == (byte)Enums.FlagCancel.NoCancel)
            {
                var nowdate = DateTime.Now;
                if (nowdate < model.day_b.Value)
                {
                    return "未开始";
                }
                else if (nowdate > model.day_e.Value)
                {
                    return "已过期";
                }
                else
                {
                    return "进行中";
                }
            }
            else if (model.flag_cancel.Value == (byte)Enums.FlagCancel.Canceled)
            {
                return "已作废";
            }
            return String.Empty;
        }
        #endregion


        [ActionPurview(true)]
        public ActionResult List()
        {
            Hashtable param = base.GetParameters();
            try
            {
                int limit = base.PageSizeFromCookie;
                param.Add("id_masteruser", id_user_master);
                ParamVessel pv = new ParamVessel();
                pv.Add("_search_", "0", HandleType.DefaultValue);
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                pv.Add("s_bm_djlx", "", HandleType.Remove);
                //pv.Add("s_flag_sh", "", HandleType.Remove);
                pv.Add("s_rule_name","",HandleType.Remove,true);
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("sort", "rq_create desc", HandleType.DefaultValue);
                var search_cxd_state = string.Format("{0}", param["s_cx_flag"]);
                param = param.Trim(pv);
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);
                AddQurryParam(search_cxd_state,param);

                var id_shopList = GetShopIds();
                //id_shopList.Add(id_shop_master);
                if (id_shopList.Any())
                {
                    param.Add("id_shopList", id_shopList.ToArray());
                }

                PageNavigate pn = new PageNavigate();
                param.Add("id_shop_current",id_shop);
                pn = BusinessFactory.Td_Promote_1.GetPage(param);
                var plist = new PageList<Td_Promote_1WithUserName>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"];//排序规则需要返回前台
                ViewData["cxlist"] = GetCxlx();
                ViewData["GetBackViewName"] = new Func<string, string>(GetBackViewName);
                ViewData["HandlePromote1State"] = new Func<Td_Promote_1WithUserName, string>(HandlePromote1State);
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

        /// <summary>
        /// 当前促销
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult CurrentList()
        {
            Hashtable param = base.GetParameters();
            try
            {
                int limit = base.PageSizeFromCookie;
                param.Add("id_masteruser", id_user_master);
                ParamVessel pv = new ParamVessel();
                pv.Add("_search_", "0", HandleType.DefaultValue);
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                pv.Add("s_bm_djlx", "", HandleType.Remove);
                pv.Add("s_rule_name", "", HandleType.Remove, true);
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("s_id_shop", "", HandleType.Remove);
                pv.Add("sort", "rq_create desc", HandleType.DefaultValue);
                param = param.Trim(pv);
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                param.Add("flag_stop", (int)Enums.FlagStop.Start);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);
                PageNavigate pn = new PageNavigate();
                param.Add("page_bm_djlx","");
                param.Add("now_date", DateTime.Now);
                param.Add("page_zh_group","");
                param.Add("sp_id_shop",id_shop);
                string s_id_shop = string.Format("{0}", param["s_id_shop"]);
                param.Remove("s_id_shop");
                if (string.IsNullOrEmpty(s_id_shop))
                {
                    var id_shopList = GetShopIds();
                    if (id_shopList.Any())
                    {
                        param.Add("id_shopList", id_shopList.ToArray());
                    }
                }
                else
                {
                    var id_shopList=new List<string>(){s_id_shop}.ToArray();
                    param.Add("id_shopList", id_shopList);
                }
                pn = BusinessFactory.Tb_Promote.GetPage(param);
                var plist = new PageList<Tb_Promote_Query>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"];       //排序规则需要返回前台
                ViewData["cxlist"] = GetCxlx();
                ViewData["GetBackViewName"] = new Func<string, string>(GetBackViewName);
                //if (id_shop == id_shop_master)
                //{
                //    ViewData["userShopList"] = GetShop(Enums.ShopDataType.UserShop).OrderBy(a => a.bm).ToList(); ;
                //}
                //else
                //{
                //    ViewData["userShopList"] = GetShop(Enums.ShopDataType.UserShopOnly).OrderBy(a => a.bm).ToList(); ;
                //}
                ViewData["userShopList"] = GetCurrentUserMgrShop(id_user, id_shop);
                
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            if (param["_search_"].ToString().Equals("1"))
            {
                return PartialView("_CurrentList");
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="bm_djlx"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult Add(string bm_djlx)
        {
            ViewData["shopList"] = GetCurrentUserMgrShop(id_user,id_shop);//GetShop(Enums.ShopDataType.UserShop).OrderBy(a => a.bm).ToList(); ; //GetUserShop();
            ViewData["version"] = version;
            ViewData["bm_djlx"] = bm_djlx;
            return View("Edit");
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult Add(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                return BusinessFactory.Td_Promote_1.Add(model);
            });
            return JsonString(null, 1);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult Edit(string id,string bm_djlx)
        {
            Hashtable param = new Hashtable();
            param.Add("id", id);
            param.Add("id_masteruser", id_user_master);
            param.Add("id_shop", id_shop);
            ViewData["item_edit"] = BusinessFactory.Td_Promote_1.Get(param).Data;
            //ViewData["selectShop"]=BusinessFactory.Td_Promote_Shop.GetAll(param).Data;
            ViewData["shopList"] = GetCurrentUserMgrShop(id_user, id_shop);//GetShop(Enums.ShopDataType.UserShop).OrderBy(a => a.bm).ToList(); ; //GetUserShop();
            ViewData["version"] = version;
            ViewData["hyfls"] = GetHyfl();
            ViewData["id_shop"] = id_shop;
            ViewData["option"] = "edit";
            ViewData["bm_djlx"] = bm_djlx;
            //获取前台控制小数点
            ViewBag.DigitHashtable = GetParm();
            return View(GetBackViewName(bm_djlx));
        }
        /// <summary>
        /// 详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionPurview(true)]
        [ActionAlias("promote","list")]
        [ActionAlias("promote", "currentlist")]
        public ActionResult Detial(string id, string bm_djlx, string form)
        {
            Hashtable param = new Hashtable();
            param.Add("id", id);
            param.Add("id_masteruser", id_user_master);
            param.Add("id_shop", id_shop);
            ViewData["item_edit"] = BusinessFactory.Td_Promote_1.Get(param).Data;
            ViewData["shopList"] = GetCurrentUserMgrShop(id_user, id_shop);//GetShop(Enums.ShopDataType.UserShop).OrderBy(a => a.bm).ToList(); ; //GetUserShop();
            ViewData["version"] = version;
            ViewData["hyfls"] = GetHyfl();
            ViewData["id_shop"] = id_shop;
            ViewData["option"] = "detial";
            ViewData["bm_djlx"] = bm_djlx;
            ViewData["form"] = form;
            //获取前台控制小数点
            ViewBag.DigitHashtable = GetParm();
            return View(GetBackViewName(bm_djlx));
        }
        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionPurview(true)]
        //[ActionAlias("promote", "add")]
        public ActionResult Copy(string id, string bm_djlx)
        {
            Hashtable param = new Hashtable();
            param.Add("id", id);
            param.Add("id_masteruser", id_user_master);
            param.Add("id_shop", id_shop);
            ViewData["item_edit"] = BusinessFactory.Td_Promote_1.Get(param).Data;
            ViewData["shopList"] = GetCurrentUserMgrShop(id_user, id_shop); //GetShop(Enums.ShopDataType.UserShop).OrderBy(a=>a.bm).ToList(); //GetUserShop();
            ViewData["version"] = version;
            ViewData["hyfls"] = GetHyfl();
            ViewData["id_shop"] = id_shop;
            ViewData["option"] = "copy";
            ViewData["bm_djlx"] = bm_djlx;
            //获取前台控制小数点
            ViewBag.DigitHashtable = GetParm();
            return View(GetBackViewName(bm_djlx));
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult Copy(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                model.op = "copy";
                model.id_shop = id_shop;
                model.AutoAudit = AutoAudit();
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                return BusinessFactory.Td_Promote_1.Add(model);
            });
            return JsonString(res, 1);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult Edit(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                return BusinessFactory.Td_Promote_1.Update(model);
            });
            return JsonString(res, 1);
        }
        /// <summary>
        /// 查询商品
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult SearchSp()
        {
            Hashtable param = base.GetParameters();
            int limit = base.PageSizeFromCookie;
            param.Add("id_masteruser", id_user_master);
            ParamVessel pv = new ParamVessel();
            pv.Add("_search_", "0", HandleType.DefaultValue);
            pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
            pv.Add("page", 0, HandleType.DefaultValue);
            pv.Add("pageSize", limit, HandleType.DefaultValue);
            pv.Add("sort", "rq_create desc", HandleType.DefaultValue);
            param = param.Trim(pv);
            param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            int pageIndex = Convert.ToInt32(param["page"]);
            param.Add("limit", limit);
            param.Add("start", pageIndex * limit);
            PageNavigate pn = new PageNavigate();

            param.Add("id_shop", id_shop);
            pn = BusinessFactory.Tb_Shopsp.GetPageList(param);

            var plist = new PageList<SelectSpModel>(pn, pageIndex, limit);
            plist.PageIndex = pageIndex;
            plist.PageSize = limit;
            ViewData["List"] = plist;
            ViewData["sort"] = param["sort"];       //排序规则需要返回前台
            ViewData["pn"] = plist;

            if (param["_search_"].ToString().Equals("1"))
            {
                return PartialView("_searchsp");
            }
            else
            {
                return View("SearchSp");
            }
        }

        
        private void SetViewData()
        {
            //if (id_shop == id_shop_master)
            //{
            //    ViewData["shopList"] = GetShop(Enums.ShopDataType.UserShop).OrderBy(a=>a.bm).ToList();
            //}
            //else
            //{
            //    ViewData["shopList"] = GetShop(Enums.ShopDataType.UserShopOnly).OrderBy(a => a.bm).ToList(); ; //GetUserShop();
            //}
            var shoplist=GetCurrentUserMgrShop(id_user, id_shop);
            ViewData["shopList"] = shoplist;
            ViewData["stringShopList"] = JSON.Serialize(shoplist);
            ViewData["version"] = version;
            ViewData["hyfls"] = GetHyfl();
            ViewData["id_shop"] = id_shop;
            //ViewData["option"] = "add";
            //获取前台控制小数点
            ViewBag.DigitHashtable = GetParm();
        }

        #region 折扣
        #region 单品折扣
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult DPZKAdd()
        {
            SetViewData();
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult DPZKAdd(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.preferential = "zk";
                model.spxz = "dp";
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                model.bm_djlx = "CX110";
                model.id_shop = id_shop;
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                model.AutoAudit = AutoAudit();
                var br= BusinessFactory.Td_Promote_1.Add(model);
                return br;
            });
            return JsonString(res, 1);
        }
        #endregion

        #region 品类折扣
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult SPFLZKAdd()
        {
            SetViewData();
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult SPFLZKAdd(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.preferential = "zk";
                model.spxz = "spfl";
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                model.bm_djlx = "CX120";
                model.id_shop = id_shop;
                model.AutoAudit = AutoAudit();
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                var br = BusinessFactory.Td_Promote_1.Add(model);
                return br;
            });
            return JsonString(res, 1);
        }
        #endregion

        #region 单组折扣
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult DZZKAdd()
        {
            SetViewData();
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult DZZKAdd(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.preferential = "zk";
                model.spxz = "dzsp";
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                model.bm_djlx = "CX130";
                model.id_shop = id_shop;
                model.AutoAudit = AutoAudit();
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                var br = BusinessFactory.Td_Promote_1.Add(model);
                return br;
            });
            return JsonString(res, 1);
        }
        #endregion

        #region 时段折扣
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult SDZKAdd()
        {
            SetViewData();
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult SDZKAdd(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.preferential = "zk";
                model.spxz = "dp";
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                model.bm_djlx = "CX140";
                model.id_shop = id_shop;
                model.AutoAudit = AutoAudit();
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                var br = BusinessFactory.Td_Promote_1.Add(model);
                return br;
            });
            return JsonString(res, 1);
        }
        #endregion

        #region 整单折扣
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult BillZKAdd()
        {
            SetViewData();
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult BillZKAdd(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.preferential = "zk";
                model.spxz = "bill";
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                model.bm_djlx = "CX150";
                model.id_shop = id_shop;
                model.AutoAudit = AutoAudit();
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                var br = BusinessFactory.Td_Promote_1.Add(model);
                return br;
            });
            return JsonString(res, 1);
        }
        #endregion 
        #endregion

        #region 买送

        #region 单品买减
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult DPMSAdd()
        {
            SetViewData();
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult DPMSAdd(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.preferential = "yh";
                model.spxz = "dp";
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                model.bm_djlx = "CX210";
                model.id_shop = id_shop;
                model.AutoAudit = AutoAudit();
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                var br = BusinessFactory.Td_Promote_1.Add(model);
                return br;
            });
            return JsonString(res, 1);
        }
        #endregion

        #region 单组买送
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult DZMSAdd()
        {
            SetViewData();
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult DZMSAdd(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.preferential = "yh";
                model.spxz = "dzsp";
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                model.bm_djlx = "CX220";
                model.id_shop = id_shop;
                model.AutoAudit = AutoAudit();
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                var br = BusinessFactory.Td_Promote_1.Add(model);
                return br;
            });
            return JsonString(res, 1);
        }
        #endregion

        #region 整类买送
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult SPFLMSAdd()
        {
            SetViewData();
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult SPFLMSAdd(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.preferential = "yh";
                model.spxz = "spfl";
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                model.bm_djlx = "CX230";
                model.id_shop = id_shop;
                model.AutoAudit = AutoAudit();
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                var br = BusinessFactory.Td_Promote_1.Add(model);
                return br;
            });
            return JsonString(res, 1);
        }
        #endregion

        #region 整单买送
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult BillMSAdd()
        {
            SetViewData();
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult BillMSAdd(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.preferential = "yh";
                model.spxz = "bill";
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                model.bm_djlx = "CX240";
                model.id_shop = id_shop;
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                model.AutoAudit = AutoAudit();
                var br = BusinessFactory.Td_Promote_1.Add(model);
                return br;
            });
            return JsonString(res, 1);
        }
        #endregion

        #endregion

        #region 加价换够

        #region 单组加价换够
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult DZHGAdd()
        {
            SetViewData();
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult DZHGAdd(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.preferential = "jj";
                model.spxz = "dzsp";
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                model.bm_djlx = "CX310";
                model.id_shop = id_shop;
                model.AutoAudit = AutoAudit();
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                var br = BusinessFactory.Td_Promote_1.Add(model);
                return br;
            });
            return JsonString(res, 1);
        }
        #endregion

        #region 整单加价换够
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult BillHGAdd()
        {
            ViewData["shopList"] = GetShop(Enums.ShopDataType.UserShop).OrderBy(a=>a.bm).ToList(); //GetUserShop();
            ViewData["version"] = version;
            ViewData["hyfls"] = GetHyfl();
            ViewData["id_shop"] = id_shop;
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult BillHGAdd(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.preferential = "jj";
                model.spxz = "bill";
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                model.bm_djlx = "CX320";
                model.id_shop = id_shop;
                model.AutoAudit = AutoAudit();
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                var br = BusinessFactory.Td_Promote_1.Add(model);
                return br;
            });
            return JsonString(res, 1);
        }
        #endregion

        #endregion

        #region 特价促销
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult TJAdd()
        {
            SetViewData();
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult TJAdd(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.preferential = "tj";
                model.spxz = "dp";
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                model.bm_djlx = "CX010";
                model.id_shop = id_shop;
                model.AutoAudit = AutoAudit();
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                var br = BusinessFactory.Td_Promote_1.Add(model);
                return br;
            });
            return JsonString(res, 1);
        }
        #endregion

        #region 组合促销
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult ZHAdd()
        {
            SetViewData();
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult ZHAdd(PromoteViewModel model)
        {
            var res = HandleResult(() =>
            {
                model.preferential = "zhj";
                model.spxz = "zh";
                model.id_masteruser = id_user_master;
                model.id_user = id_user;
                model.bm_djlx = "CX020";
                model.id_shop = id_shop;
                model.jsfs = "zhsp";
                model.jsgz = "mei";
                model.AutoAudit = AutoAudit();
                if (version == string.Format("{0}", (int)Enums.FuncVersion.SingleShop))
                {
                    model.id_shops = id_shop;
                }
                return BusinessFactory.Td_Promote_1.Add(model);
            });
            return JsonString(res, 1);
        }
        #endregion
        /// <summary>
        /// 获取返回视图名称
        /// </summary>
        /// <param name="bm_djlx"></param>
        /// <returns></returns>
        protected string GetBackViewName(string bm_djlx)
        {
            switch (bm_djlx)
            {
                case "CX010": return "TJAdd";
                case "CX020": return "ZHAdd";
                case "CX110": return "DPZKAdd";
                case "CX120": return "SPFLZKAdd";
                case "CX130": return "DZZKAdd";
                case "CX140": return "SDZKAdd";
                case "CX150": return "BillZKAdd";
                case "CX210": return "DPMSAdd";
                case "CX220": return "DZMSAdd";
                case "CX230": return "SPFLMSAdd";
                case "CX240": return "BillMSAdd";
                case "CX310": return "DZHGAdd";
                case "CX320": return "BillHGAdd";
            }
            return "";
        }
        
        
        [HttpGet]
        [ActionPurview(false)]
        public ActionResult ShopFl()
        {
            return View();
        }
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
                Hashtable param=new Hashtable();
                param.Add("id",id);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                return BusinessFactory.Td_Promote_1.Active(param);
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
        public ActionResult Zf(string id)
        {
            var res = HandleResult(() =>
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                return BusinessFactory.Td_Promote_1.Stop(param);
            });
            return JsonString(res, 1);
        }
        /// <summary>
        /// 终止
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult Stop(string id)
        {
            var res = HandleResult(() =>
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                return BusinessFactory.Tb_Promote.Stop(param);
            });
            return JsonString(res, 1);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                return BusinessFactory.Td_Promote_1.Delete(param);
            });
            return JsonString(res, 1);
        }

        

    }
}
