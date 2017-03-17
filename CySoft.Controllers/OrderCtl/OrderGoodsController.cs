using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CySoft.Controllers.Filters;
using System.Web.UI;
using CySoft.Controllers.Base;
using System.Collections;
using CySoft.Model.Flags;
using CySoft.Frame.Core;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;

#region 订货商品
#endregion

namespace CySoft.Controllers.OrderCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class OrderGoodsController : BaseController
    {


        /// <summary>
        /// 订货商品列表
        /// lxt
        /// 2015-04-17
        /// mq  2016-04-22修改
        /// </summary>
        //[ActionPurview(false)]
        public ActionResult List()
        {
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            var list = new PageList<SkuData>(limit);
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("supplierid", (long)0, HandleType.Remove);//供应商Id
                p.Add("orderby", 8, HandleType.DefaultValue);//排序
                p.Add("typeid", (long)0, HandleType.Remove);//商品类别 Id
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("flag_show", String.Empty, HandleType.Remove);//是否展开高级搜索
                p.Add("flag_search", String.Empty, HandleType.Remove);//来源-是否是来自高级查询
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("mc", string.Empty, HandleType.Remove, true);//商品名称
                p.Add("bm_Interface", String.Empty, HandleType.Remove, true);//商品编码
                p.Add("barcode", String.Empty, HandleType.Remove, true);//商品条码
                param = param.Trim(p);
                switch (param["orderby"].ToString())
                {
                    case "2":
                        param["orderby"] = 2;
                        param.Add("sort", "db.id_sp");
                        param.Add("dir", "asc");
                        break;
                    case "3":
                        param.Add("sort", "db.bm_Interface");
                        param.Add("dir", "asc");
                        break;
                    case "4":
                        param.Add("sort", "db.bm_Interface");
                        param.Add("dir", "desc");
                        break;
                    case "5":
                        param.Add("sort", "sp.mc");
                        param.Add("dir", "asc");
                        break;
                    case "6":
                        param.Add("sort", "sp.mc");
                        param.Add("dir", "desc");
                        break;
                    case "7":
                        param.Add("sort", "db.rq_create");
                        param.Add("dir", "asc");
                        break;
                    case "8":
                        param.Add("sort", "db.rq_create");
                        param.Add("dir", "desc");
                        break;
                    default:
                        param["orderby"] = 1;
                        param.Add("sort", "db.id_sp");
                        param.Add("dir", "desc");
                        break;
                }


                ViewData["orderby"] = param["orderby"];
                param.Remove("orderby");
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                ViewData["pageIndex"] = pageIndex;
                if (param.ContainsKey("typeid"))
                {
                    ViewData["typeid"] = param["typeid"];
                    param.Add("id_spfl", param["typeid"]);
                    param.Remove("typeid");
                }
                if (param.ContainsKey("supplierid"))
                {
                    if (!string.IsNullOrWhiteSpace(param["supplierid"].ToString()) && !param["supplierid"].ToString().Equals("0"))
                    {
                        ViewData["supplierid"] = param["supplierid"];
                        param.Add("id_gys", param["supplierid"]);
                    }
                    param.Remove("supplierid");
                }
                if (param.ContainsKey("flag_show"))
                {
                    ViewData["flag_show"] = param["flag_show"];
                    param.Remove("flag_show");
                }
                if (param.ContainsKey("mc"))
                {
                    ViewData["mc"] = param["mc"];
                }
                if (param.ContainsKey("bm_Interface"))
                {
                    ViewData["bm_Interface"] = param["bm_Interface"];
                }
                if (param.ContainsKey(""))
                {
                    ViewData["barcode"] = param["barcode"];
                }
                if (param.ContainsKey("flag_search"))
                {
                    ViewData["flag_search"] = param["flag_search"];
                    param.Remove("id_gys");
                }
                ViewData["keyword"] = GetParameter("keyword");
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                if (Request.IsAjaxRequest())
                {
                    param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                    param.Add("limit", limit);
                    param.Add("start", (pageIndex - 1) * limit);
                    param.Add("flag_up", YesNoFlag.Yes);
                    param.Add("flag_stop", YesNoFlag.No);
                    //pn = BusinessFactory.Goods.GetPageSkn(param);
                    pn = BusinessFactory.Goods.GetPage(param);
                    list = new PageList<SkuData>(pn, pageIndex, limit);
                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (Request.IsAjaxRequest())
            {
                //return PartialView("_ListControl", list);
                return PartialView("_OrderListControl", list);
            }
            try
            {
                Hashtable param = new Hashtable();
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                param.Add("flag_stop", YesNoFlag.No);
                BaseResult br = BusinessFactory.Supplier.GetAll(param);
                Hashtable spparam = new Hashtable();
                ViewData["supplierList"] = br.Data ?? new List<Tb_Gys_Cgs_Query>();
                if (br.Data != null)
                {
                    spparam.Add("id_gys", param["supplierid"]);
                    ViewData["spflList"] = BusinessFactory.GoodsTpye.GetAll(param).Data ?? new List<Tb_Spfl_Query>();
                }
            }
            catch (CySoftException ex)
            {

                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return View("List");
            return View("OrderList");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult OrderList()
        {
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Tb_Sp_Query> list = new PageList<Tb_Sp_Query>(limit);
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("supplierid", (long)0, HandleType.Remove);//供应商Id
                p.Add("orderby", 8, HandleType.DefaultValue);//排序
                p.Add("typeid", (long)0, HandleType.Remove);//商品类别 Id
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("flag_show", String.Empty, HandleType.Remove);//是否展开高级搜索
                p.Add("flag_search", String.Empty, HandleType.Remove);//来源-是否是来自高级查询
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("mc", string.Empty, HandleType.Remove, true);//商品名称
                p.Add("bm_Interface", String.Empty, HandleType.Remove, true);//商品编码

                p.Add("barcode", String.Empty, HandleType.Remove, true);//商品条码
                param = param.Trim(p);
                switch (param["orderby"].ToString())
                {
                    case "2":
                        param["orderby"] = 2;
                        param.Add("sort", "spsku.id_sp");
                        param.Add("dir", "asc");
                        break;
                    case "3":
                        param.Add("sort", "gyssp.bm_Interface");
                        param.Add("dir", "asc");
                        break;
                    case "4":
                        param.Add("sort", "gyssp.bm_Interface");
                        param.Add("dir", "desc");
                        break;
                    case "5":
                        param.Add("sort", "sp.mc");
                        param.Add("dir", "asc");
                        break;
                    case "6":
                        param.Add("sort", "sp.mc");
                        param.Add("dir", "desc");
                        break;
                    case "7":
                        param.Add("sort", "gyssp.rq_create");
                        param.Add("dir", "asc");
                        break;
                    case "8":
                        param.Add("sort", "gyssp.rq_create");
                        param.Add("dir", "desc");
                        break;
                    default:
                        param["orderby"] = 1;
                        param.Add("sort", "spsku.id_sp");
                        param.Add("dir", "desc");
                        break;
                }


                ViewData["orderby"] = param["orderby"];
                param.Remove("orderby");

                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                ViewData["pageIndex"] = pageIndex;
                if (param.ContainsKey("typeid"))
                {
                    ViewData["typeid"] = param["typeid"];
                    param.Add("id_spfl", param["typeid"]);
                    param.Remove("typeid");
                }
                if (param.ContainsKey("supplierid"))
                {
                    if (!string.IsNullOrWhiteSpace(param["supplierid"].ToString()) && !param["supplierid"].ToString().Equals("0"))
                    {
                        ViewData["supplierid"] = param["supplierid"];
                        param.Add("id_gys", param["supplierid"]);
                    }
                    param.Remove("supplierid");
                }
                if (param.ContainsKey("flag_show"))
                {
                    ViewData["flag_show"] = param["flag_show"];
                    param.Remove("flag_show");
                }
                if (param.ContainsKey("mc"))
                {
                    ViewData["mc"] = param["mc"];
                }
                if (param.ContainsKey("bm_Interface"))
                {
                    ViewData["bm_Interface"] = param["bm_Interface"];
                }
                if (param.ContainsKey(""))
                {
                    ViewData["barcode"] = param["barcode"];
                }
                if (param.ContainsKey("flag_search"))
                {
                    ViewData["flag_search"] = param["flag_search"];
                    param.Remove("id_gys");
                }
                ViewData["keyword"] = GetParameter("keyword");
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                if (Request.IsAjaxRequest())
                {
                    param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                    param.Add("limit", limit);
                    param.Add("start", (pageIndex - 1) * limit);
                    param.Add("flag_up", YesNoFlag.Yes);
                    param.Add("flag_stop", YesNoFlag.No);
                    pn = BusinessFactory.Goods.GetOrderPage(param);
                    list = new PageList<Tb_Sp_Query>(pn, pageIndex, limit);
                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_OrderListControl", list);
            }
            try
            {
                Hashtable param = new Hashtable();
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                param.Add("flag_stop", YesNoFlag.No);
                BaseResult br = BusinessFactory.Supplier.GetAll(param);
                ViewData["supplierList"] = br.Data ?? new List<Tb_Gys_Cgs_Query>();
            }
            catch (CySoftException ex)
            {

                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("OrderList");
        }
        /// <summary>
        /// 商品详细页
        /// lxt
        /// 2015-03-18
        /// </summary>
        [ValidateInput(false)]
        [ActionPurview(false)]
        public ActionResult Item(long id)
        {
            BaseResult br = new BaseResult();
            try
            {

                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("supplierid", (long)0, HandleType.ReturnMsg);//商品类别 Id
                p.Add("sku", (long)0, HandleType.Remove);//商品SKU
                param = param.Trim(p);
                param.Add("id", id);
                if (param.ContainsKey("sku"))
                {
                    ViewData["id_sku"] = param["sku"];
                    param.Add("id_sku", param["sku"]);
                    param.Remove("sku");
                }
                ViewData["supplierid"] = param["supplierid"];
                param.Add("id_gys", param["supplierid"]);
                param.Remove("supplierid");
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                br = BusinessFactory.Goods.Get(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(br.Data);
        }

        /// <summary>
        /// 引用商品_调出引用界面
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [ActionPurview(false)]
        public ActionResult CiteList(string id)
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();

            var model = new List<Model.Other.CiteGoodsData>();
            //当前用户的 供应商编码
            int id_supplier = (int)GetLoginInfo<long>("id_supplier");
            try
            {
                #region 获得引用商品资料

                var list = JSON.Deserialize<List<Tb_Sp_Favorites>>(id);
                if (list != null && list.Count > 0)
                {
                    foreach (var M in list)
                    {
                        //查询供应商——商品资料
                        param.Clear();
                        param.Add("id_sku", M.id_sp);
                        param.Add("id_gys", M.id_gys);
                        var sp = BusinessFactory.Goods.Get_Gys_sp(param).Data as Tb_Gys_Sp_Query;

                        if (sp.id_sku != null)
                        {
                            var id_sku = Convert.ToInt32(sp.id_sku);
                            //判断是否已存在关联
                            if (!BusinessFactory.GoodsSkuFd.Check_Sdk_Fd(id_supplier, id_sku))
                            {
                                model.Add(new CiteGoodsData()
                                {
                                    id_mc = sp.mc,
                                    id_mc_zh = sp.mc_zh,
                                    id_sku = sp.id_sku,
                                    bm_Interface = sp.bm_Interface,
                                    dj_dh = sp.dj_base,
                                    id_gys = M.id_gys,
                                    sl_dh_min = 0
                                });
                            }
                        }
                    }
                }
                #endregion

                //获取分类
                param.Clear();
                param.Add("id_gys", id_supplier);
                ViewData["GoodsTpye"] = BusinessFactory.GoodsTpye.GetAll(param).Data;
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }
    }
}
