using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Ts;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Web;
using System.Data;
using System.Net;

namespace CySoft.Controllers.Goods
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class GoodsController : BaseController
    {
        ///// <summary>
        ///// 商品列表
        ///// </summary>
        //[ValidateInput(false)]
        //public ActionResult List()
        //{
        //    PageNavigate pn = new PageNavigate();
        //    int limit = 10;
        //    long typeid = 0;
        //    PageList<SkuData> list = new PageList<SkuData>(limit);
        //    try
        //    {
        //        BaseResult br = new BaseResult();
        //        Hashtable param = base.GetParameters();
        //        ParamVessel p = new ParamVessel();
        //        p.Add("orderby", 8, HandleType.DefaultValue);//排序
        //        p.Add("up", (byte)1, HandleType.Remove);//是否上架
        //        p.Add("stop", (byte)0, HandleType.Remove);//是否启用
        //        p.Add("typeid", (long)0, HandleType.Remove);//商品类别 Id
        //        p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
        //        p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
        //        p.Add("spfl_id", string.Empty, HandleType.Remove);
        //        p.Add("flag_up", string.Empty, HandleType.Remove);
        //        p.Add("Warning", string.Empty, HandleType.Remove);
        //        p.Add("volume", string.Empty, HandleType.Remove);
        //        p.Add("flag_show", string.Empty, HandleType.Remove);
        //        param = param.Trim(p);
        //        if (param["flag_show"] != null)
        //        {
        //            ViewData["flag_show"] = true;
        //        }
        //        switch (param["orderby"].ToString())
        //        {
        //            case "3":
        //                param.Add("sort", "db.dj_base");
        //                param.Add("dir", "asc");
        //                break;
        //            case "4":
        //                param.Add("sort", "db.dj_base");
        //                param.Add("dir", "desc");
        //                break;
        //            case "5":
        //                param.Add("sort", "db.rq_edit");
        //                param.Add("dir", "asc");
        //                break;
        //            case "6":
        //                param.Add("sort", "db.rq_edit");
        //                param.Add("dir", "desc");
        //                break;
        //            case "7":
        //                param.Add("sort", "db.rq_create");
        //                param.Add("dir", "asc");
        //                break;
        //            case "8":
        //                param.Add("sort", "db.rq_create");
        //                param.Add("dir", "desc");
        //                break;
        //            default:
        //                param["orderby"] = 8;
        //                param.Add("sort", "db.rq_create");
        //                param.Add("dir", "desc");
        //                break;
        //        }


        //        ViewData["orderby"] = param["orderby"];
        //        param.Remove("orderby");

        //        int pageIndex = Convert.ToInt32(param["pageIndex"]);
        //        pageIndex = !(pageIndex > 0) ? 1 : pageIndex;
        //        ViewData["pageIndex"] = pageIndex;
        //        if (param.ContainsKey("up"))
        //        {
        //            ViewData["up"] = param["up"];
        //            param.Add("flag_up", Enum.Parse(typeof(YesNoFlag), param["up"].ToString(), true));
        //            param.Remove("up");
        //        }


        //        if (param.ContainsKey("typeid"))
        //        {
        //            ViewData["typeid"] = param["typeid"];
        //            param.Add("id_spfl", param["typeid"]);
        //            typeid = Convert.ToInt64(param["typeid"]);
        //            param.Remove("typeid");
        //        }

        //        ViewData["keyword"] = param["keyword"];
        //        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        param.Add("limit", limit);
        //        param.Add("start", (pageIndex - 1) * limit);

        //        pn = BusinessFactory.Goods.GetPage(param);
        //        list = new PageList<SkuData>(pn, pageIndex, limit);
        //    }
        //    catch (CySoftException ex)
        //    {

        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("_ListControl", list);
        //    }

        //    if (typeid > 0)
        //    {
        //        try
        //        {
        //            Hashtable param = new Hashtable();
        //            param.Add("id", typeid);
        //            BaseResult br = BusinessFactory.GoodsTpye.Get(param);
        //            if (!br.Success)
        //            {
        //                throw new CySoftException(br);
        //            }
        //            ViewData["typeName"] = ((Tb_Spfl)br.Data).name;
        //        }
        //        catch (CySoftException ex)
        //        {
        //            throw ex;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    return View(list);
        //}

        ///// <summary>
        ///// 商品选择 
        ///// znt 2015-04-08
        ///// </summary>
        //[ActionPurview(false)]
        //public ActionResult ListSelect(string role)
        //{
        //    PageNavigate pn = new PageNavigate();
        //    BaseResult br = new BaseResult();
        //    Hashtable param = base.GetParameters();
        //    long typeid = 0;
        //    int limit = 0;
        //    PageList<SkuData> list = new PageList<SkuData>(limit);
        //    try
        //    {
        //        ParamVessel p = new ParamVessel();
        //        p.Add("id_cgs", GetLoginInfo<long>("id_buyer"), HandleType.ReturnMsg);
        //        p.Add("id_gys", GetLoginInfo<long>("id_supplier"), HandleType.ReturnMsg);
        //        p.Add("typeid", (long)0, HandleType.Remove);//商品类别 Id
        //        p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
        //        p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
        //        p.Add("limit", 10, HandleType.DefaultValue);//当前页码               
        //        param = param.Trim(p);
        //        //param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        int pageIndex = CySoft.Frame.Common.TypeConvert.ToInt(param["pageIndex"], 1);

        //        if (pageIndex < 1)
        //        {
        //            pageIndex = 1;
        //        }
        //        limit = CySoft.Frame.Common.TypeConvert.ToInt(param["limit"].ToString(), 10);
        //        if (limit < 1)
        //        {
        //            limit = 1;
        //        }

        //        ViewData["pageIndex"] = pageIndex;

        //        if (param.ContainsKey("typeid"))
        //        {
        //            ViewData["typeid"] = param["typeid"];
        //            param.Add("id_spfl", param["typeid"]);
        //            typeid = CySoft.Frame.Common.TypeConvert.ToInt(param["typeid"], 0);
        //            ViewData["typeid"] = typeid;
        //            param.Remove("typeid");
        //        }

        //        ViewData["keyword"] = GetParameter("keyword");
        //        ViewData["id_gys"] = param["id_gys"]; //GetLoginInfo<long>("id_supplier");
        //        ViewData["id_cgs"] = GetLoginInfo<long>("id_buyer");
        //        param["limit"] = limit;
        //        param.Add("start", (pageIndex - 1) * limit);
        //        param.Add("flag_up", 1);

        //        pn = BusinessFactory.Goods.GetPage(param);
        //        list = new PageList<SkuData>(pn, pageIndex, limit);

        //        if (typeid > 0)
        //        {
        //            param.Clear();
        //            param.Add("id", typeid);
        //            br = BusinessFactory.GoodsTpye.Get(param);
        //            if (!br.Success)
        //            {
        //                throw new CySoftException(br);
        //            }
        //            ViewData["typeName"] = ((Tb_Spfl)br.Data).name;
        //        }

        //    }
        //    catch (CySoftException ex)
        //    {

        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return PartialView(list);
        //}

        ///// <summary>
        ///// 商品选择(发布广告使用）
        ///// </summary>
        //[ActionPurview(false)]
        //public ActionResult ListSelectToAdv()
        //{
        //    PageNavigate pn = new PageNavigate();
        //    int limit = 8;
        //    long typeid = 0;
        //    PageList<SkuData> list = new PageList<SkuData>(limit);
        //    try
        //    {
        //        BaseResult br = new BaseResult();
        //        Hashtable param = base.GetParameters();
        //        ParamVessel p = new ParamVessel();
        //        //p.Add("orderby", 8, HandleType.DefaultValue);//排序
        //        p.Add("up", (byte)1, HandleType.Remove);//是否上架
        //        p.Add("stop", (byte)0, HandleType.Remove);//是否启用
        //        p.Add("typeid", (long)0, HandleType.Remove);//商品类别 Id
        //        p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
        //        p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
        //        p.Add("spfl_id", string.Empty, HandleType.Remove);
        //        p.Add("flag_up", 1, HandleType.Remove);
        //        p.Add("Warning", string.Empty, HandleType.Remove);
        //        p.Add("volume", string.Empty, HandleType.Remove);
        //        p.Add("flag_show", string.Empty, HandleType.Remove);
        //        param = param.Trim(p);


        //        //ViewData["orderby"] = param["orderby"];
        //        //param.Remove("orderby");

        //        int pageIndex = Convert.ToInt32(param["pageIndex"]);
        //        pageIndex = !(pageIndex > 0) ? 1 : pageIndex;
        //        ViewData["pageIndex"] = pageIndex;
        //        if (param.ContainsKey("up"))
        //        {
        //            ViewData["up"] = param["up"];
        //            param.Add("flag_up", Enum.Parse(typeof(YesNoFlag), param["up"].ToString(), true));
        //            param.Remove("up");
        //        }


        //        if (param.ContainsKey("typeid"))
        //        {
        //            ViewData["typeid"] = param["typeid"];
        //            param.Add("id_spfl", param["typeid"]);
        //            typeid = Convert.ToInt64(param["typeid"]);
        //            param.Remove("typeid");
        //        }

        //        ViewData["keyword"] = param["keyword"];
        //        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                
        //        param.Add("limit", limit);
        //        param.Add("start", (pageIndex - 1) * limit);

        //        pn = BusinessFactory.Goods.GetPage(param);
        //        list = new PageList<SkuData>(pn, pageIndex, limit);
        //    }
        //    catch (CySoftException ex)
        //    {

        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return PartialView(list);
        //}

        ///// <summary>
        ///// 商品详细页
        ///// lxt
        ///// 2015-03-18
        ///// </summary>
        //[ValidateInput(false)]
        //public ActionResult Item(long id)
        //{
        //    BaseResult br = new BaseResult();
        //    try
        //    {

        //        Hashtable param = base.GetParameters();
        //        ParamVessel p = new ParamVessel();
        //        p.Add("sku", (long)0, HandleType.Remove);//商品SKU
        //        param = param.Trim(p);
        //        param.Add("id", id);
        //        if (param.ContainsKey("sku"))
        //        {
        //            ViewData["id_sku"] = param["sku"];
        //            param.Add("id_sku", param["sku"]);
        //            param.Remove("sku");
        //        }
        //        long supplierid = GetLoginInfo<long>("id_supplier");
        //        ViewData["supplierid"] = supplierid;
        //        param.Add("id_gys", supplierid);
        //        br = BusinessFactory.Goods.Get(param);
        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return View(br.Data);
        //}

        //[ValidateInput(false)]
        //public ActionResult Edit(long id)
        //{
        //    BaseResult br = new BaseResult();
        //    try
        //    {

        //        Hashtable param = base.GetParameters();
        //        ParamVessel p = new ParamVessel();
        //        p.Add("sku", (long)0, HandleType.Remove);//商品SKU
        //        param = param.Trim(p);
        //        param.Add("id", id);
        //        if (param.ContainsKey("sku"))
        //        {
        //            ViewData["id_sku"] = param["sku"];
        //            param.Add("id_sku", param["sku"]);
        //            param.Remove("sku");
        //        }
        //        long supplierid = GetLoginInfo<long>("id_supplier");
        //        ViewData["supplierid"] = supplierid;
        //        param.Add("id_gys", supplierid);
        //        br = BusinessFactory.Goods.Get(param);

        //        param.Clear();
        //        param.Add("id_gys", supplierid);
        //        ViewData["GoodsTagList"] = BusinessFactory.GoodsTag.GetAll(param).Data;
        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return View(br.Data);
        //}

        //[HttpPost]
        //[ActionPurview(false)]
        //public ActionResult GetGoodsExpand()
        //{
        //    BaseResult br = new BaseResult();
        //    try
        //    {
        //        Hashtable param = base.GetParameters();
        //        ParamVessel p = new ParamVessel();
        //        p.Add("mc", string.Empty, HandleType.ReturnMsg);//商品名称  
        //        p.Add("id_gys", GetLoginInfo<long>("id_supplier"), HandleType.DefaultValue);//供应商id
        //        param = param.Trim(p);
        //        param.Add("id_gys_create", GetLoginInfo<long>("id_supplier"));

        //        br = BusinessFactory.Goods.Get(param);
        //        return Json(br);

        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 根据商品类别id 获取商品类别属性
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[ActionPurview(false)]
        //public ActionResult GetGoodsExpandTemplate()
        //{
        //    BaseResult br = new BaseResult();
        //    try
        //    {
        //        Hashtable param = base.GetParameters();

        //        if (param.ContainsKey("typeid"))
        //        {
        //            var typeid = param["typeid"];

        //            param.Clear();
        //            param.Add("id", typeid);
        //            br = BusinessFactory.GoodsTpye.Get(param);
        //            var goodsType = br.Data as Tb_Spfl;

        //            //获取所有商品类别的父级设置
        //            var idList = goodsType.path.Split('/');
        //            param.Clear();
        //            param.Add("idList", idList);
        //            br = BusinessFactory.GoodsTpye.GetAll(param);

        //            var goodsList = br.Data as IList<Tb_Spfl_Query>;

        //            goodsList = goodsList.Where(d => d.id_sp_expand_template != 0).OrderByDescending(d => d.path.Length).ToList();

        //            if (goodsList != null && goodsList.Count > 0)
        //            {
        //                if (goodsList[0].id_sp_expand_template != null && goodsType.id_sp_expand_template != 0)
        //                {
        //                    param.Clear();
        //                    param.Add("fatherId", goodsList[0].id_sp_expand_template);
        //                    var spec = BusinessFactory.GoodsSpec.GetAll(param);
        //                    return Json(spec);
        //                }
        //            }

        //            ////0默认无规格
        //            //if (goodsType.id_sp_expand_template != 0 && goodsType.id_sp_expand_template != null)
        //            //{

        //            //}

        //        }
        //        br.Data = null;
        //        return Json(br);

        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 新增商品 
        ///// znt
        ///// 2015-03-02
        ///// </summary>
        //public ActionResult Add()
        //{
        //    Hashtable param = new Hashtable();
        //    param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //    ViewData["CustomerType"] = BusinessFactory.CustomerType.GetAll(param).Data;
        //    ViewData["GoodsTagList"] = BusinessFactory.GoodsTag.GetAll(param).Data;
        //    return View();
        //}

        ///// <summary>
        /////  下架商品
        /////  tim 2015-04-13
        ///// </summary>
        //[HttpPost]
        //public ActionResult Stop()
        //{
        //    BaseResult br = new BaseResult();
        //    try
        //    {
        //        Hashtable param = base.GetParameters();
        //        ParamVessel p = new ParamVessel();
        //        p.Add("skuList", string.Empty, HandleType.ReturnMsg);//商品SKU  
        //        p.Add("flag", 0, HandleType.ReturnMsg);
        //        param = param.Trim(p);
        //        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        param.Add("id_edit", GetLoginInfo<long>("id_user"));
        //        br = BusinessFactory.Goods.Stop(param);
        //        WriteDBLog(LogFlag.Base, br.Message);
        //        return Json(br);

        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        /////  上架商品
        /////  tim 2015-04-13
        ///// </summary>
        //[HttpPost]
        //public ActionResult Active()
        //{
        //    BaseResult br = new BaseResult();
        //    try
        //    {
        //        Hashtable param = base.GetParameters();
        //        ParamVessel p = new ParamVessel();
        //        p.Add("skuList", (long)0, HandleType.ReturnMsg);//商品SKU                 
        //        param = param.Trim(p);
        //        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        param.Add("id_edit", GetLoginInfo<long>("id_user"));
        //        br = BusinessFactory.Goods.Active(param);
        //        WriteDBLog(LogFlag.Base, br.Message);
        //        return Json(br);

        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 新增商品 
        ///// znt
        ///// 2015-03-07
        ///// </summary>
        //[HttpPost]
        //public ActionResult AddGoods(string goodsdata)
        //{
        //    BaseResult br = new BaseResult();

        //    try
        //    {
        //        GoodsData model = JSON.Deserialize<GoodsData>(goodsdata);

        //        if (string.IsNullOrEmpty(model.mc))
        //        {
        //            br.Success = false;
        //            br.Data = "sp_mc";
        //            br.Message.Add("商品名称不能为空");
        //            return Json(br);
        //        }

        //        if (model.id_spfl == 0)
        //        {
        //            br.Success = false;
        //            br.Data = "id_spfl";
        //            br.Message.Add("商品类别不能为空");
        //            return Json(br);
        //        }

        //        if (string.IsNullOrEmpty(model.unit))
        //        {
        //            br.Success = false;
        //            br.Data = "unit";
        //            br.Message.Add("商品单位不能为空");
        //            return Json(br);
        //        }

        //        if (model.sku == null || model.sku.Count.Equals(0))
        //        {
        //            br.Success = false;
        //            br.Data = "sku";
        //            br.Message.Add("必须至少有一条商品数据.");
        //            return Json(br);
        //        }

        //        if (!string.IsNullOrWhiteSpace(model.description) && model.description.Length > 2000)
        //        {
        //            br.Success = false;
        //            br.Data = "sku";
        //            br.Message.Add("商品介绍应在2000字以内.");
        //            return Json(br);
        //        }

        //        List<string> bmlist = new List<string>();
        //        foreach (var item in model.sku)
        //        {

        //            if (bmlist.Contains(item.bm_Interface) || string.IsNullOrEmpty(item.bm_Interface))
        //            {
        //                br.Success = false;
        //                br.Data = "bm";
        //                br.Message.Add("商品编码不能重复也不能为空");
        //                return Json(br);
        //            }
        //            bmlist.Add(item.bm_Interface);
        //        }


        //        if (!model.id.HasValue || model.id.Value.Equals(0))
        //            model.id = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Sp));

        //        model.rq_create = DateTime.Now;
        //        model.rq_edit = DateTime.Now;
        //        model.id_gys_create = GetLoginInfo<long>("id_supplier");
        //        model.id_edit = GetLoginInfo<long>("id_user");
        //        model.id_create = GetLoginInfo<long>("id_user");

        //        br = BusinessFactory.Goods.Add(model);

        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Json(br);

        //}

        ///// <summary>
        ///// 修改商品
        ///// tim 2015-04-09
        ///// </summary>
        //[HttpPost]
        //public ActionResult Save(string obj)
        //{
        //    BaseResult br = new BaseResult();
        //    br.Success = true;
        //    try
        //    {
        //        var model = JSON.Deserialize<GoodsSave>(obj);

        //        if (model.id == 0)
        //        {
        //            br.Success = false;
        //            br.Message.Add("请选择商品修改");
        //        }
        //        if (string.IsNullOrEmpty(model.mc))
        //        {
        //            br.Success = false;
        //            br.Message.Add("商品名称不能为空");
        //        }
        //        if (model.id_spfl == 0)
        //        {
        //            br.Success = false;
        //            br.Message.Add("商品类别不能为空");
        //        }
        //        if (string.IsNullOrEmpty(model.unit))
        //        {
        //            br.Success = false;
        //            br.Message.Add("商品单位不能为空");
        //        }
        //        if (!(model.sku.Count > 0))
        //        {
        //            br.Success = false;
        //            br.Message.Add("请选择商品修改");
        //        }
        //        if (!(model.dj.Count > 0))
        //        {
        //            br.Success = false;
        //            br.Message.Add("请设置商品价格");
        //        }
        //        if (!string.IsNullOrWhiteSpace(model.description) && model.description.Length > 2000)
        //        {
        //            br.Success = false;
        //            br.Data = "sku";
        //            br.Message.Add("商品介绍应在2000字以内.");
        //            return Json(br);
        //        }
        //        if (br.Success)
        //        {
        //            model.id_gys = GetLoginInfo<long>("id_supplier");
        //            model.rq_create = DateTime.Now;
        //            model.rq_edit = DateTime.Now;
        //            model.id_edit = GetLoginInfo<long>("id_user");
        //            model.id_create = GetLoginInfo<long>("id_user");

        //            br = BusinessFactory.Goods.Save(model);
        //        }
        //        else
        //            br.Level = ErrorLevel.Warning;

        //        WriteDBLog(LogFlag.Base, br.Message);

        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Json(br);
        //}

        ///// <summary>
        ///// 删除商品SKU
        ///// tim 2015-04-09
        ///// </summary>
        //[HttpPost]
        //public ActionResult Delete(string id)
        //{
        //    BaseResult br = new BaseResult();
        //    br.Success = true;
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(id))
        //        {
        //            br.Success = false;
        //            br.Message.Add("请选择要删除的商品.");
        //        }
        //        if (br.Success)
        //        {
        //            var ht = new Hashtable();
        //            ht.Add("skuList", id);
        //            ht.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //            br = BusinessFactory.Goods.Delete(ht);
        //        }
        //        else
        //            br.Level = ErrorLevel.Warning;

        //        WriteDBLog(LogFlag.Base, br.Message);

        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Json(br);
        //}

        ///// <summary>
        ///// 按条件查询商品,返回所有查询结果
        ///// tim 2015-04-09
        ///// </summary>
        //public ActionResult GetAll(string obj)
        //{
        //    BaseResult br = new BaseResult();
        //    br.Success = true;
        //    try
        //    {
        //        var model = JSON.Deserialize<Hashtable>(obj);
        //        if (model == null || model.Count == 0)
        //        {
        //            br.Success = false;
        //            br.Message.Add("请选择查询的商品或参数.");
        //        }

        //        if (br.Success)
        //        {
        //            br = BusinessFactory.Goods.GetAll(model);
        //        }
        //        else
        //            br.Level = ErrorLevel.Warning;

        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Json(br);
        //}

        ///// <summary>
        ///// 导出商品
        ///// </summary>
        ///// mq 2016-05-17 修改
        ///// 增加sku参数，实现可按选中商品导出或导出所有
        ///// <param name="sku"></param>
        ///// <returns></returns>
        //[ActionPurview(false)]
        //public FileResult Export(string sku)
        //{
        //    PageNavigate pn = new PageNavigate();
        //    int limit = 10;
        //    PageList<SkuData> list = new PageList<SkuData>(limit);
        //    try
        //    {
        //        #region 数据
        //        BaseResult br = new BaseResult();
        //        Hashtable param = new Hashtable();
        //        ParamVessel p = new ParamVessel();
        //        p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
        //        p.Add("orderby", 8, HandleType.DefaultValue);//排序
        //        param = param.Trim(p);
        //        int pageIndex = Convert.ToInt32(param["pageIndex"]);
        //        pageIndex = !(pageIndex > 0) ? 1 : pageIndex;
        //        param.Add("sort", "db.rq_create");
        //        param.Add("dir", "desc");
        //        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        //param.Add("limit", limit);
        //        //param.Add("start", (pageIndex - 1) * limit);

        //        if (!string.IsNullOrEmpty(sku))
        //        {
        //            string[] skulist = sku.Split(',');
        //            param.Add("sku", skulist);
        //        }
        //        pn = BusinessFactory.Goods.GetPage(param);
        //        list = new PageList<SkuData>(pn, pageIndex, limit);

        //        //客户级别
        //        Hashtable tb = new Hashtable();
        //        tb.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        var cgs = BusinessFactory.CustomerType.GetAll(tb);
        //        IList<Tb_Cgs_Level> mTb_Cgs_Level = cgs.Data as IList<Tb_Cgs_Level>;
        //        #endregion

        //        #region 导出商品
        //        var fileName = Server.MapPath("~/Template/product_template.xls");
        //        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //        IWorkbook book = new HSSFWorkbook(file);
        //        ISheet sheet1 = book.GetSheet("商品数据");
        //        sheet1.SetColumnHidden(0, true);//列隐藏
        //        IRow row1 = sheet1.GetRow(0);
        //        for (int y = 0; y < mTb_Cgs_Level.Count; y++)
        //        {
        //            row1.CreateCell(15 + 3 * y).SetCellValue(mTb_Cgs_Level[y].name + "订货价");
        //            row1.CreateCell(16 + 3 * y).SetCellValue(mTb_Cgs_Level[y].name + "起订量");
        //            row1.CreateCell(17 + 3 * y).SetCellValue(mTb_Cgs_Level[y].name + "限购量");
        //            sheet1.SetColumnWidth(15 + 3 * y, 20 * 256);
        //            sheet1.SetColumnWidth(16 + 3 * y, 20 * 256);
        //            sheet1.SetColumnWidth(17 + 3 * y, 20 * 256);
        //        }

        //        ICellStyle r_style = book.CreateCellStyle();
        //        r_style.Alignment = HorizontalAlignment.Right;
        //        for (int i = 0; i < list.Count; i++)
        //        {

        //            IRow rowtemp = sheet1.CreateRow(i + 1);
        //            rowtemp.CreateCell(0).SetCellValue(list[i].id != null ? (long)list[i].id : 0);
        //            rowtemp.CreateCell(1).SetCellValue(list[i].bm);
        //            rowtemp.CreateCell(2).SetCellValue(list[i].name_sp);
        //            rowtemp.CreateCell(3).SetCellValue(list[i].name_fl);//分类
        //            var name_spec_zh = list[i].name_spec_zh;
        //            ArrayList spec_valueList = new ArrayList();
        //            var spec_list = string.Empty;
        //            if (!string.IsNullOrEmpty(name_spec_zh))
        //            {
        //                var itemList = name_spec_zh.Split(',');
        //                for (int x = 0; x < itemList.Count() - 1; x++)
        //                {
        //                    var item = itemList[x].Split(':');
        //                    spec_list += item[0] + "/";
        //                    spec_valueList.Add(item[1]);
        //                }
        //                var spec = spec_list.Substring(0, spec_list.Length - 1);
        //                rowtemp.CreateCell(4).SetCellValue(spec);
        //            }

        //            for (int m = 0; m < spec_valueList.Count; m++)
        //            {
        //                rowtemp.CreateCell(5 + m).SetCellValue(spec_valueList[m].ToString());
        //            }

        //            //价格部分
        //            Hashtable mtable = new Hashtable();
        //            mtable.Add("id", list[i].id_sp);
        //            mtable.Add("id_sku", list[i].id);
        //            mtable.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //            var mTb_Sp = BusinessFactory.Goods.Get(mtable).Data as Tb_Sp_Get;
        //            var priceList = (from item in mTb_Sp.levelPriceList where item.id_sku == list[i].id select item).ToList();
        //            var msku = mTb_Sp.skuList.FirstOrDefault();
        //            rowtemp.CreateCell(10).SetCellValue(msku.description);
        //            rowtemp.CreateCell(11).SetCellValue(list[i].unit);
        //            rowtemp.CreateCell(12).SetCellValue(msku.barcode);
        //            rowtemp.CreateCell(13).SetCellValue(list[i].keywords);
        //            rowtemp.CreateCell(14).SetCellValue(list[i].flag_up == CySoft.Model.Flags.YesNoFlag.Yes ? "上架" : "下架");
        //            rowtemp.CreateCell(15).SetCellValue(Decimal.Round((decimal)list[i].dj_base, 2).ToString());
        //            rowtemp.GetCell(15).CellStyle = r_style;
        //            for (int n = 0; n < priceList.Count; n++)
        //            {
        //                rowtemp.CreateCell(16 + 3 * n).SetCellValue(Decimal.Round(priceList[n].dj_dh, 2).ToString());
        //                rowtemp.CreateCell(17 + 3 * n).SetCellValue(Decimal.Round(priceList[n].sl_dh_min, 2).ToString());
        //                rowtemp.CreateCell(18 + 3 * n).SetCellValue(0);
        //                rowtemp.GetCell(16 + 3 * n).CellStyle = r_style;
        //                rowtemp.GetCell(17 + 3 * n).CellStyle = r_style;
        //            }

        //        }
        //        sheet1.CreateFreezePane(0, 1);
        //        // 写入到客户端 
        //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //        book.Write(ms);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        return File(ms, "application/vnd.ms-excel", "商品数据_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
        //        #endregion
        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //[ActionPurview(false)]
        //public FileResult ExportAll()
        //{
        //    List<SkuData> list = new List<SkuData>();
        //    try
        //    {
        //        #region 数据
        //        BaseResult br = new BaseResult();
        //        Hashtable param = base.GetParameters();
        //        int pageIndex = Convert.ToInt32(param["pageIndex"]);
        //        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        br = BusinessFactory.Goods.ExportAllSp(param);
        //        list = (List<SkuData>)br.Data;
        //        long id_gys = GetLoginInfo<long>("id_supplier");
        //        br = BusinessFactory.Goods.GetPicList(list, id_gys);
        //        var mTb_Sp = (List<Tb_Sp_Dj_Query>)br.Data; 
        //        //客户级别
        //        Hashtable tb = new Hashtable();
        //        tb.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        var cgs = BusinessFactory.CustomerType.GetAll(tb);
        //        IList<Tb_Cgs_Level> mTb_Cgs_Level = cgs.Data as IList<Tb_Cgs_Level>;
        //        #endregion

        //        #region 导出商品
        //        var fileName = Server.MapPath("~/Template/UpGoods.xls");
        //        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //        IWorkbook book = new HSSFWorkbook(file);
        //        ISheet sheet1 = book.GetSheet("商品数据");
        //        sheet1.SetColumnHidden(0, true);//列隐藏
        //        sheet1.SetColumnHidden(1, true);//列隐藏
        //        sheet1.SetColumnHidden(5, true);//列隐藏
        //        IRow row1 = sheet1.GetRow(0);
        //        for (int y = 0; y < mTb_Cgs_Level.Count; y++)
        //        {

        //            row1.CreateCell(18 + 2 * y).SetCellValue(mTb_Cgs_Level[y].name + "订货价");
        //            row1.CreateCell(19 + 2 * y).SetCellValue(mTb_Cgs_Level[y].name + "起订量");
        //            sheet1.SetColumnWidth(18 + 2 * y, 20 * 256);
        //            sheet1.SetColumnWidth(19 + 2 * y, 20 * 256);

        //        }

        //        ICellStyle r_style = book.CreateCellStyle();
        //        r_style.Alignment = HorizontalAlignment.Right;
        //        string tag = "";
        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            tag = "";
        //            IRow rowtemp = sheet1.CreateRow(i + 1);
        //            rowtemp.CreateCell(0).SetCellValue(list[i].id != null ? (long)list[i].id : 0);
        //            rowtemp.CreateCell(1).SetCellValue(list[i].id != null ? (long)list[i].id_sp : 0);
        //            rowtemp.CreateCell(2).SetCellValue(list[i].bm);
        //            rowtemp.CreateCell(3).SetCellValue(list[i].name_sp);
        //            //rowtemp.CreateCell(4).SetCellValue(list[i].id_spfl != null ? (long)list[i].id_spfl : 0);//分类
        //            rowtemp.CreateCell(4).SetCellValue(list[i].name_fl);//分类
        //            var name_spec_zh = list[i].name_spec_zh;
        //            ArrayList spec_valueList = new ArrayList();
        //            var spec_list = string.Empty;
        //            if (!string.IsNullOrEmpty(name_spec_zh))
        //            {
        //                var itemList = name_spec_zh.Split(',');
        //                for (int x = 0; x < itemList.Count(); x++)
        //                {
        //                    var item = itemList[x].Split(':');
        //                    spec_list += item[0] + "/";
        //                    spec_valueList.Add(item[1]);
        //                }
        //                var spec = spec_list.Substring(0, spec_list.Length - 1);
        //                rowtemp.CreateCell(5).SetCellValue(list[i].name_spec_id);
        //                rowtemp.CreateCell(6).SetCellValue(spec);
        //            }

        //            for (int m = 0; m < spec_valueList.Count; m++)
        //            {
        //                rowtemp.CreateCell(7 + m).SetCellValue(spec_valueList[m].ToString());
        //            }

        //            //价格部分
        //            var priceList = (from item in mTb_Sp where item.id_sku == list[i].id select item).ToList();
        //            rowtemp.CreateCell(12).SetCellValue(list[i].unit);
        //            rowtemp.CreateCell(13).SetCellValue(list[i].barcode);
        //            rowtemp.CreateCell(14).SetCellValue(list[i].flag_up == CySoft.Model.Flags.YesNoFlag.Yes ? "上架" : "下架");
        //            rowtemp.CreateCell(15).SetCellValue(list[i].description);
        //            rowtemp.CreateCell(16).SetCellValue(list[i].sp_tag);
        //            rowtemp.CreateCell(17).SetCellValue(Decimal.Round((decimal)list[i].dj_base, 2).ToString());
        //            rowtemp.GetCell(17).CellStyle = r_style;
        //            for (int n = 0; n < priceList.Count; n++)
        //            {
        //                rowtemp.CreateCell(18 + 2 * n).SetCellValue(Decimal.Round(priceList[n].dj_dh, 2).ToString());
        //                rowtemp.CreateCell(19 + 2 * n).SetCellValue(Decimal.Round(priceList[n].sl_dh_min, 2).ToString());
        //                rowtemp.GetCell(18 + 2 * n).CellStyle = r_style;
        //                rowtemp.GetCell(19 + 2 * n).CellStyle = r_style;
        //            }
        //        }
        //        sheet1.CreateFreezePane(0, 1);
        //        // 写入到客户端 
        //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //        book.Write(ms);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        return File(ms, "application/vnd.ms-excel", "商品数据修改_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
        //        #endregion
        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        ///// <summary>
        ///// 导出全部商品介绍
        ///// </summary>
        ///// mq  2016-05-06
        ///// <returns></returns>
        //[ActionPurview(false)]
        //public FileResult InfoExportAll(string sku)
        //{
        //    List<Tb_Sp_Info> list = new List<Tb_Sp_Info>();
        //    try
        //    {
        //        Hashtable param = new Hashtable();
        //        BaseResult br = new BaseResult();
        //        if (string.IsNullOrEmpty(sku))
        //        {
        //            param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        }
        //        else
        //        {
        //            param.Add("sku", sku.Split(','));
        //        }
                
        //        br = BusinessFactory.Goods.GetInfoAll(param);
        //        list = JSON.ConvertToType<List<Tb_Sp_Info>>(br.Data);
        //        HSSFWorkbook book = new HSSFWorkbook();
        //        Dictionary<string, int> e_param = new Dictionary<string, int> { 
        //           {"序号",12},
        //           {"商品id",18},
        //           {"商品sku",18},
        //           {"商品编码",18},
        //           {"商品名称",18},
        //           {"商品规格",25},
        //           {"商品介绍",50},
        //        };
        //        ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, "商品介绍");
        //        sheet1.SetColumnHidden(1, true);//列隐藏
        //        sheet1.SetColumnHidden(2, true);//列隐藏
        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            IRow rowtemp = sheet1.CreateRow(i + 1);
        //            rowtemp.CreateCell(0).SetCellValue(i + 1);
        //            rowtemp.CreateCell(1).SetCellValue(list[i].id_sp.ToString());
        //            rowtemp.CreateCell(2).SetCellValue(list[i].id_sku.ToString());
        //            rowtemp.CreateCell(3).SetCellValue(list[i].bm);
        //            rowtemp.CreateCell(4).SetCellValue(list[i].name_sp);
        //            rowtemp.CreateCell(5).SetCellValue(list[i].name_spec_zh);
        //            rowtemp.CreateCell(6).SetCellValue(list[i].description);
        //            sheet1.GetRow(i + 1).Height = 16 * 20;
        //        }

        //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //        book.Write(ms);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        return File(ms, "application/vnd.ms-excel", "商品介绍_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //public static DataTable goodstable;
        ///// <summary>
        ///// 批量修改商品
        ///// </summary>
        ///// mq  2016-05-09
        ///// <param name="filePath"></param>
        ///// <returns></returns>
        //[ActionPurview(false)]
        //[HttpPost]
        //public ActionResult UpGoods(string filePath)
        //{
        //    BaseResult br = new BaseResult();
        //    Hashtable ht = new Hashtable();
        //    ht["id_gys"] = GetLoginInfo<long>("id_supplier");
        //    ht["id_user"] = GetLoginInfo<long>("id_user");
        //    int success = 0;
        //    int fail = 0;
        //    try
        //    {
        //        var savePath = Server.MapPath(filePath);
        //        var cgs = BusinessFactory.CustomerType.GetAll(ht);
        //        IList<Tb_Cgs_Level> mTb_Cgs_Level = cgs.Data as IList<Tb_Cgs_Level>;
        //        BusinessFactory.Goods.UpSp(out success, out fail, out goodstable, ht, mTb_Cgs_Level, savePath);

        //    }
        //    catch (Exception)
        //    {
        //        br.Success = true;
        //        br.Data = "数据格式有误，请重新下载导入模版，再导入";
        //        return Json(br);
        //    }
        //    br.Success = true;
        //    br.Data = " 共" + goodstable.Rows.Count + "数据，成功导入" + success + "条，异常数据<span id=\"failCount\">" + fail + "</span> 条";
        //    return Json(br);
        //}
        ///// <summary>
        ///// 下载商品修改失败数据
        ///// </summary>
        ///// mq  2016-05-09
        ///// <param name="filePath"></param>
        ///// <returns></returns>
        //[ActionPurview(false)]
        //public FileResult DownloadGoodsFail(string filePath)
        //{
        //    try
        //    {
        //        if (goodstable == null || goodstable.Rows.Count == 0)
        //        {
        //            return null;
        //        }
        //        BaseResult br;
        //        var dr = goodstable.Select("备注 <> ''");
        //        Hashtable param = new Hashtable();
        //        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        br = BusinessFactory.CustomerType.GetAll(param);
        //        IList<Tb_Cgs_Level> mTb_Cgs_Level = br.Data as IList<Tb_Cgs_Level>;

        //        var fileName = Server.MapPath("~/Template/UpGoods.xls");
        //        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //        IWorkbook book = new HSSFWorkbook(file);
        //        ISheet sheet1 = book.GetSheet("商品数据");
        //        sheet1.SetColumnHidden(0, true);//列隐藏
        //        sheet1.SetColumnHidden(1, true);//列隐藏
        //        sheet1.SetColumnHidden(5, true);//列隐藏
        //        IRow mRow = sheet1.GetRow(0);
        //        for (int i = 0; i < mTb_Cgs_Level.Count; i++)
        //        {
        //            mRow.CreateCell(17 + 2 * i).SetCellValue(mTb_Cgs_Level[i].name + "订货价");
        //            mRow.CreateCell(18 + 2 * i).SetCellValue(mTb_Cgs_Level[i].name + "起订量");
        //            sheet1.SetColumnWidth(17 + 2 * i, 20 * 256);
        //            sheet1.SetColumnWidth(18 + 2 * i, 20 * 256);
        //        }
        //        sheet1.CreateFreezePane(0, 1);
        //        mRow.CreateCell(18 + 3 * (mTb_Cgs_Level.Count - 1)).SetCellValue("备注");
        //        sheet1.SetColumnWidth(18 + 3 * (mTb_Cgs_Level.Count - 1), 30 * 256);
        //        for (int i = 0; i < dr.Length; i++)
        //        {
        //            IRow rowtemp = sheet1.CreateRow(i + 1);
        //            rowtemp.CreateCell(0).SetCellValue(dr[i]["商品sku"].ToString());
        //            rowtemp.CreateCell(1).SetCellValue(dr[i]["商品id"].ToString());
        //            rowtemp.CreateCell(2).SetCellValue(dr[i]["商品编码"].ToString());
        //            rowtemp.CreateCell(3).SetCellValue(dr[i]["商品名称"].ToString());
        //            rowtemp.CreateCell(4).SetCellValue(dr[i]["商品分类"].ToString());
        //            rowtemp.CreateCell(5).SetCellValue(dr[i]["规格模板id"].ToString());
        //            rowtemp.CreateCell(6).SetCellValue(dr[i]["多规格字段设置"].ToString());
        //            rowtemp.CreateCell(7).SetCellValue(dr[i]["规格1内容"].ToString());
        //            rowtemp.CreateCell(8).SetCellValue(dr[i]["规格2内容"].ToString());
        //            rowtemp.CreateCell(9).SetCellValue(dr[i]["规格3内容"].ToString());
        //            rowtemp.CreateCell(10).SetCellValue(dr[i]["规格4内容"].ToString());
        //            rowtemp.CreateCell(11).SetCellValue(dr[i]["规格5内容"].ToString());
        //            rowtemp.CreateCell(12).SetCellValue(dr[i]["计量单位"].ToString());
        //            rowtemp.CreateCell(13).SetCellValue(dr[i]["条形码"].ToString());
        //            rowtemp.CreateCell(14).SetCellValue(dr[i]["状态"].ToString());
        //            rowtemp.CreateCell(15).SetCellValue(dr[i]["商品介绍"].ToString());
        //            rowtemp.CreateCell(16).SetCellValue(dr[i]["商品标签（注：多个以中文逗号区分）"].ToString());
        //            rowtemp.CreateCell(17).SetCellValue(dr[i]["市场价"].ToString());
        //            for (int x = 0; x < mTb_Cgs_Level.Count; x++)
        //            {
        //                rowtemp.CreateCell(18 + 2 * x).SetCellValue(dr[i][mTb_Cgs_Level[x].name + "订货价"].ToString());
        //                rowtemp.CreateCell(19 + 2 * x).SetCellValue(dr[i][mTb_Cgs_Level[x].name + "起订量"].ToString());
        //            }
        //            rowtemp.CreateCell(18 + 3 * (mTb_Cgs_Level.Count - 1)).SetCellValue(dr[i]["备注"].ToString());
        //            //
        //        }
        //        // 写入到客户端 
        //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //        book.Write(ms);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        return File(ms, "application/vnd.ms-excel", "商品更新失败数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}
        ///// <summary>
        ///// 导入商品介绍
        ///// </summary>
        //public static DataTable infotable;//数据
        //[ActionPurview(false)]
        //[HttpPost]
        //public ActionResult ImportInfo(string filePath)
        //{
        //    BaseResult br = new BaseResult();
        //    int success = 0;
        //    int fail = 0;
        //    try
        //    {
        //        InfoData(filePath, out success, out fail, out infotable);
        //    }
        //    catch (Exception ex)
        //    {
        //        br.Success = true;
        //        br.Data = "数据格式有误，请重新下载导入模版，再导入";
        //        return Json(br);
        //    }
        //    br.Success = true;
        //    br.Data = " 共" + infotable.Rows.Count + "数据，成功导入" + success + "条，异常数据<span id=\"failCount\">" + fail + "</span> 条";
        //    return Json(br);
        //}
        ///// <summary>
        ///// 商品介绍数据处理
        ///// </summary>
        ///// <param name="filePath"></param>
        ///// <param name="success"></param>
        ///// <param name="fail"></param>
        ///// <param name="table"></param>
        //public void InfoData(string filePath, out int success, out int fail, out DataTable table)
        //{
        //    BaseResult br = new BaseResult();
        //    success = 0;
        //    fail = 0;
        //    //读取数据
        //    var savePath = Server.MapPath(filePath);
        //    table = NPOIHelper.ImportExcelFile(savePath);

        //    if (!table.Columns.Contains("备注"))
        //        table.Columns.Add("备注", typeof(System.String));
        //    Tb_Sp_Info info = new Tb_Sp_Info();
        //    long id = 0;
        //    long sku = 0;
        //    foreach (DataRow item in table.Rows)
        //    {
        //        if (!string.IsNullOrEmpty(item["商品id"].ToString()) && !string.IsNullOrEmpty(item["商品sku"].ToString()))
        //        {
        //            id = Convert.ToInt64(item["商品id"]);
        //            sku = Convert.ToInt64(item["商品sku"]);
        //            info.id_sp = id;
        //            info.id_sku = sku;
        //            info.description = item["商品介绍"].ToString();
        //            BusinessFactory.Goods.AddOfUpInfo(info);
        //        }
        //        else
        //        {
        //            item["备注"] = "无法识别该商品";
        //            fail++;
        //        }
        //        success++;
        //    }
        //}
        ///// <summary>
        ///// 下载商品介绍失败数据
        ///// </summary>
        ///// <param name="filePath"></param>
        ///// <returns></returns>
        //public FileResult DownloadFailInfo(string filePath)
        //{
        //    try
        //    {
        //        if (infotable == null || infotable.Rows.Count == 0)
        //        {
        //            return null;
        //        }
        //        var dr = table.Select("备注 <> ''");

        //        var fileName = Server.MapPath("~/Template/info_template.xls");
        //        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //        IWorkbook book = new HSSFWorkbook(file);
        //        ISheet sheet1 = book.GetSheet("商品介绍");
        //        for (int i = 0; i < dr.Length; i++)
        //        {
        //            IRow rowtemp = sheet1.CreateRow(i + 1);
        //            rowtemp.CreateCell(0).SetCellValue(dr[i]["序号"].ToString());
        //            rowtemp.CreateCell(1).SetCellValue(dr[i]["商品id"].ToString());
        //            rowtemp.CreateCell(2).SetCellValue(dr[i]["商品sku"].ToString());
        //            rowtemp.CreateCell(3).SetCellValue(dr[i]["商品编码"].ToString());
        //            rowtemp.CreateCell(4).SetCellValue(dr[i]["商品名称"].ToString());
        //            rowtemp.CreateCell(5).SetCellValue(dr[i]["商品规格"].ToString());
        //            rowtemp.CreateCell(6).SetCellValue(dr[i]["商品介绍"].ToString());
        //            rowtemp.CreateCell(7).SetCellValue(dr[i]["备注"].ToString());
        //        }
        //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //        book.Write(ms);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        return File(ms, "application/vnd.ms-excel", "商品介绍失败数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //#region 下载模板
        ///// <summary>
        ///// 下载模板
        ///// </summary>
        ///// <returns></returns>
        //[ActionPurview(false)]
        //public FileResult DownLoad(int type = 0)
        //{
        //    BaseResult br = new BaseResult();
        //    try
        //    {
        //        //数据
        //        var fileName = Server.MapPath(type == 1 ? "~/Template/product_import.xls" : "~/Template/stock_import.xls");
        //        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //        IWorkbook book = new HSSFWorkbook(file);
        //        ISheet sheet1 = book.GetSheet(type == 1 ? "商品数据" : "商品库存");
        //        IRow mRow = sheet1.GetRow(0);
        //        if (type == 1)
        //        {
        //            Hashtable param = new Hashtable();
        //            param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //            br = BusinessFactory.CustomerType.GetAll(param);
        //            IList<Tb_Cgs_Level> mTb_Cgs_Level = br.Data as IList<Tb_Cgs_Level>;
        //            if (!br.Success)
        //            {
        //                return null;
        //            }
        //            for (int i = 0; i < mTb_Cgs_Level.Count; i++)
        //            {
        //                mRow.CopyCell(14, 15 + 3 * i).SetCellValue(mTb_Cgs_Level[i].name + "订货价");
        //                mRow.CopyCell(14, 16 + 3 * i).SetCellValue(mTb_Cgs_Level[i].name + "起订量");
        //                mRow.CopyCell(14, 17 + 3 * i).SetCellValue(mTb_Cgs_Level[i].name + "限购量");
        //                sheet1.SetColumnWidth(15 + 3 * i, 20 * 256);
        //                sheet1.SetColumnWidth(16 + 3 * i, 20 * 256);
        //                sheet1.SetColumnWidth(17 + 3 * i, 20 * 256);
        //            }
        //        }
        //        sheet1.CreateFreezePane(0, 1);
        //        // 写入到客户端 
        //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //        book.Write(ms);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        return File(ms, "application/vnd.ms-excel", type == 1 ? "商品导入模板.xls" : "库存导入模板.xls");
        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        //#endregion

        //[ActionPurview(false)]
        //public ActionResult Import(int type = 0)
        //{
        //    ViewBag.type = type;
        //    return View();
        //}


        //#region 商品导入
        ///// <summary>
        ///// 商品导入
        ///// </summary>
        ///// <param name="filebase"></param>
        ///// <returns></returns>
        //public static DataTable table;//数据
        //[ActionPurview(false)]
        //[HttpPost]
        //public ActionResult Import(string filePath)
        //{

        //    var mGoodsList = new List<GoodsData>();
        //    var failGoodsList = new List<GoodsData>();
        //    BaseResult br = null;
        //    try
        //    {
        //        ProccessData(filePath, mGoodsList, failGoodsList, out br, out table);
        //    }
        //    catch (Exception ex)
        //    {
        //        br.Success = true;
        //        br.Data = "数据格式有误，请重新下载导入模版，再导入";
        //        return Json(br);
        //    }
        //    #region 注释
        //    /*  foreach (var model in mGoodsList)
        //    {
        //        br = BusinessFactory.Goods.Add(model);
        //        if (br.Success)
        //        {
        //            successGoodsList.Add(model);
        //        }
        //        else
        //        {
        //            var bm = model.sku[0].bm;
        //            var dr = table.Select("商品编码='" + bm+"'");
        //            if (dr != null && dr.Length > 0)
        //            {
        //                dr[0]["备注"] += br.Message[0].ToString() + "/";
        //            }
        //            failGoodsList.Add(model);
        //        }
        //    }*/
        //    #endregion

        //    br.Success = true;
        //    br.Data = " 共" + table.Rows.Count + "数据，成功导入" + mGoodsList.Count + "条，异常数据<span id=\"failCount\">" + failGoodsList.Count + "</span> 条";
        //    return Json(br);
        //}

        //#endregion

        //#region  导出失败处理
        //[ActionPurview(false)]
        //public FileResult DownloadFail(string filePath)
        //{
        //    try
        //    {
        //        if (table == null || table.Rows.Count == 0)
        //        {
        //            return null;
        //        }
        //        BaseResult br;
        //        var dr = table.Select("备注 <> ''");
        //        Hashtable param = new Hashtable();
        //        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        br = BusinessFactory.CustomerType.GetAll(param);
        //        IList<Tb_Cgs_Level> mTb_Cgs_Level = br.Data as IList<Tb_Cgs_Level>;

        //        var fileName = Server.MapPath("~/Template/product_import.xls");
        //        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //        IWorkbook book = new HSSFWorkbook(file);
        //        ISheet sheet1 = book.GetSheet("商品数据");

        //        IRow mRow = sheet1.GetRow(0);
        //        for (int i = 0; i < mTb_Cgs_Level.Count; i++)
        //        {
        //            mRow.CopyCell(14, 15 + 3 * i).SetCellValue(mTb_Cgs_Level[i].name + "订货价");
        //            mRow.CopyCell(14, 16 + 3 * i).SetCellValue(mTb_Cgs_Level[i].name + "起订量");
        //            mRow.CopyCell(14, 17 + 3 * i).SetCellValue(mTb_Cgs_Level[i].name + "限购量");
        //            sheet1.SetColumnWidth(15 + 3 * i, 20 * 256);
        //            sheet1.SetColumnWidth(16 + 3 * i, 20 * 256);
        //            sheet1.SetColumnWidth(17 + 3 * i, 20 * 256);
        //        }
        //        sheet1.CreateFreezePane(0, 1);
        //        mRow.CopyCell(14, 18 + 3 * (mTb_Cgs_Level.Count - 1)).SetCellValue("备注");
        //        sheet1.SetColumnWidth(18 + 3 * (mTb_Cgs_Level.Count - 1), 30 * 256);
        //        for (int i = 0; i < dr.Length; i++)
        //        {
        //            IRow rowtemp = sheet1.CreateRow(i + 1);
        //            rowtemp.CreateCell(0).SetCellValue(dr[i]["商品编码"].ToString());
        //            rowtemp.CreateCell(1).SetCellValue(dr[i]["商品名称"].ToString());
        //            rowtemp.CreateCell(2).SetCellValue(dr[i]["商品分类"].ToString());
        //            rowtemp.CreateCell(3).SetCellValue(dr[i]["多规格字段设置"].ToString());
        //            rowtemp.CreateCell(4).SetCellValue(dr[i]["规格1内容"].ToString());
        //            rowtemp.CreateCell(5).SetCellValue(dr[i]["规格2内容"].ToString());
        //            rowtemp.CreateCell(6).SetCellValue(dr[i]["规格3内容"].ToString());
        //            rowtemp.CreateCell(7).SetCellValue(dr[i]["规格4内容"].ToString());
        //            rowtemp.CreateCell(8).SetCellValue(dr[i]["规格5内容"].ToString());
        //            rowtemp.CreateCell(9).SetCellValue(dr[i]["商品介绍"].ToString());
        //            rowtemp.CreateCell(10).SetCellValue(dr[i]["计量单位"].ToString());
        //            rowtemp.CreateCell(11).SetCellValue(dr[i]["条形码"].ToString());
        //            rowtemp.CreateCell(12).SetCellValue(dr[i]["关键词"].ToString());
        //            rowtemp.CreateCell(13).SetCellValue(dr[i]["状态"].ToString());
        //            rowtemp.CreateCell(14).SetCellValue(dr[i]["市场价（元）"].ToString());
        //            for (int x = 0; x < mTb_Cgs_Level.Count; x++)
        //            {
        //                rowtemp.CreateCell(15 + 3 * x).SetCellValue(dr[i][mTb_Cgs_Level[x].name + "订货价"].ToString());
        //                rowtemp.CreateCell(16 + 3 * x).SetCellValue(dr[i][mTb_Cgs_Level[x].name + "起订量"].ToString());
        //                rowtemp.CreateCell(17 + 3 * x).SetCellValue(dr[i][mTb_Cgs_Level[x].name + "限购量"].ToString());
        //            }
        //            rowtemp.CreateCell(18 + 3 * (mTb_Cgs_Level.Count - 1)).SetCellValue(dr[i]["备注"].ToString());
        //            //
        //        }
        //        // 写入到客户端 
        //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //        book.Write(ms);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        return File(ms, "application/vnd.ms-excel", "商品导入失败数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}
        //#endregion


        //#region 数据处理
        ///// <summary>
        ///// 数据处理
        ///// </summary>
        ///// <param name="filePath"></param>
        ///// <param name="mGoodsList"></param>
        ///// <param name="failGoodsList"></param>
        ///// <param name="br"></param>
        ///// <param name="table"></param>
        //private void ProccessData(string filePath, IList<GoodsData> mGoodsList, IList<GoodsData> failGoodsList, out BaseResult br, out DataTable table)
        //{
        //    #region 数据处理
        //    br = new BaseResult();
        //    Hashtable param = new Hashtable();

        //    //读取数据
        //    var savePath = Server.MapPath(filePath);
        //    table = NPOIHelper.ImportExcelFile(savePath);
        //    if (!table.Columns.Contains("备注"))
        //        table.Columns.Add("备注", typeof(System.String));
        //    if (!table.Columns.Contains("id"))
        //        table.Columns.Add("id", typeof(System.String));

        //    //编码规则
        //    param.Add("coding", typeof(Tb_Sp_Sku).Name.ToLower());
        //    br = BusinessFactory.CodingRule.Get(param);
        //    Ts_Codingrule Codingrule = br.Data as Ts_Codingrule;

        //    //商品分类
        //    param.Clear();
        //    param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //    var fl = BusinessFactory.GoodsTpye.GetAll(param);
        //    List<Tb_Spfl_Query> list_Spfl = fl.Data as List<Tb_Spfl_Query>;

        //    //商品规格
        //    var spec = BusinessFactory.GoodsSpec.GetAll(param);
        //    var list_Sp_Expand_Template = spec.Data as List<Tb_Sp_Expand_Template>;

        //    foreach (DataRow item in table.Rows)
        //    {
        //        var model = new GoodsData();
        //        item["备注"] = "";

        //        //商品名称
        //        var mc = item["商品名称"].ToString();
        //        if (!string.IsNullOrEmpty(mc))
        //        {
        //            model.mc = mc;

        //            //判断系统是否已经存在该商品
        //            param.Clear();
        //            param.Add("mc", mc);
        //            param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //            param.Add("id_gys_create", GetLoginInfo<long>("id_supplier"));
        //            br = BusinessFactory.Goods.Get(param);
        //            if (br.Success)
        //            {
        //                var sp = br.Data as Tb_Sp_Get;
        //                model.id = sp.id;
        //            }
        //        }
        //        else
        //        {
        //            item["备注"] = "商品名称为空/";
        //        }
        //        model.description = item["商品介绍"].ToString();
        //        model.unit = item["计量单位"].ToString();
        //        model.keywords = item["关键词"].ToString();

        //        //商品分类
        //        var spfl = item["商品分类"].ToString().Trim();
        //        long? id_spfl = 0;
        //        if (string.IsNullOrEmpty(spfl))
        //        {
        //            id_spfl = list_Spfl.FirstOrDefault().id;
        //        }
        //        else
        //        {
        //            var model_Spfl = list_Spfl.Find(d => d.name == spfl);
        //            if (model_Spfl != null)
        //            {
        //                id_spfl = model_Spfl.id;
        //            }
        //            else
        //            {
        //                item["备注"] += "系统无法识别该商品分类/";
        //            }
        //        }

        //        model.id_spfl = id_spfl;
        //        model.sp_pic = new string[] { };

        //        var list_SkuData = new List<SkuData>();
        //        var model_SkuData = new SkuData();
        //        decimal dj_base = 0;

        //        //市场价
        //        if (!string.IsNullOrEmpty((item["市场价（元）"] ?? "").ToString()))
        //        {
        //            try
        //            {
        //                dj_base = Convert.ToDecimal(item["市场价（元）"]);
        //                if (dj_base > 0)
        //                    model_SkuData.dj_base = dj_base;
        //                else
        //                    item["备注"] += "市场价格式有误/";
        //            }
        //            catch (Exception ex)
        //            {
        //                item["备注"] += "市场价格式有误/";
        //            }
        //        }


        //        model_SkuData.flag_up = YesNoFlag.Yes;
        //        model_SkuData.barcode = (item["条形码"] ?? "").ToString();

        //        //商品规格
        //        List<Tb_Sp_Expand> list_Sp_Expand = new List<Tb_Sp_Expand>();
        //        var sp_Expand = item["多规格字段设置"].ToString();
        //        if (!string.IsNullOrEmpty(sp_Expand))
        //        {
        //            var sp_Expands = sp_Expand.Split('/');
        //            if (sp_Expands.Count() > 0)
        //            {
        //                for (int i = 0; i < sp_Expands.Count(); i++)
        //                {
        //                    Tb_Sp_Expand model_Sp_Expand = new Tb_Sp_Expand();
        //                    if (!string.IsNullOrEmpty((item["规格" + (i + 1) + "内容"] ?? "").ToString()))
        //                    {
        //                        var m = list_Sp_Expand_Template.Find(t => t.mc == sp_Expands[i]);
        //                        if (m != null)
        //                        {
        //                            model_Sp_Expand.id_sp_expand_template = m.id;
        //                            model_Sp_Expand.val = item["规格" + (i + 1) + "内容"].ToString();
        //                            list_Sp_Expand.Add(model_Sp_Expand);
        //                        }
        //                        else
        //                        {
        //                            item["备注"] += "商品规格[" + sp_Expands[i] + "]不存在/";
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        model_SkuData.sp_expand = list_Sp_Expand;

        //        //客户级别
        //        List<Tb_Sp_Dj> list_Sp_Dj = new List<Tb_Sp_Dj>();
        //        param.Clear();
        //        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        br = BusinessFactory.CustomerType.GetAll(param);//
        //        IList<Tb_Cgs_Level> list_Cgs_Level = br.Data as IList<Tb_Cgs_Level>;
        //        for (int i = 0; i < list_Cgs_Level.Count; i++)
        //        {
        //            Tb_Sp_Dj model_Sp_Dj = new Tb_Sp_Dj();
        //            model_Sp_Dj.id_cgs_level = list_Cgs_Level[i].id;
        //            try
        //            {
        //                var dj_dh = item[list_Cgs_Level[i].name + "订货价"] ?? "";
        //                var sl_dh_min = item[list_Cgs_Level[i].name + "起订量"] ?? "";
        //                if (!string.IsNullOrEmpty(dj_dh.ToString()))
        //                    model_Sp_Dj.dj_dh = Convert.ToDecimal(dj_dh);
        //                else
        //                    model_Sp_Dj.dj_dh = (dj_base * list_Cgs_Level[i].agio) / 100;//
        //                if (!string.IsNullOrEmpty(sl_dh_min.ToString()))
        //                    model_Sp_Dj.sl_dh_min = Convert.ToDecimal(sl_dh_min);
        //            }
        //            catch (Exception ex)
        //            {
        //                item["备注"] += list_Cgs_Level[i].name + "订货价" + "、" + list_Cgs_Level[i].name + "起订量" + "格式有误/";
        //            }

        //            list_Sp_Dj.Add(model_Sp_Dj);
        //        }

        //        model_SkuData.sp_dj = list_Sp_Dj;

        //        //商品编码
        //        var bm = (item["商品编码"] ?? "").ToString();              
         
        //        if (!string.IsNullOrEmpty(bm))
        //        {
        //            param.Clear();
        //            param.Add("bm_Interface", bm);
        //            param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //            br = BusinessFactory.GoodsInventory.GetCount(param);
        //            if (!br.Success)
        //            {
        //                item["备注"] += "商品编码[" + bm + "]重复/";
        //            }
        //        }

        //        if (item["备注"].ToString() == "")
        //        {
        //            long xh_max = BusinessFactory.Utilety.GetNextXh(typeof(Tb_Sp_Sku));
        //            if (xh_max.ToString().Length < Codingrule.length - Codingrule.prefix.Length)
        //            {
        //                model_SkuData.bm = Codingrule.prefix + xh_max.ToString().PadLeft(Codingrule.length.Value - Codingrule.prefix.Length, '0');
        //            }
        //            else
        //            {
        //                item["备注"] += "商品编码已超过有效范围/";
        //            }
        //        }
                
               
        //        item["商品编码"] = bm;
        //        item["id"] = model.id;
        //        if (item["备注"].ToString() == "")
        //        {
        //            model_SkuData.bm_Interface = bm;

        //            list_SkuData.Add(model_SkuData);
        //            model.sku = list_SkuData;
        //            //
        //            if (!model.id.HasValue || model.id.Value.Equals(0))
        //                model.id = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Sp));
        //            model.rq_create = DateTime.Now;
        //            model.rq_edit = DateTime.Now;
        //            model.id_gys_create = GetLoginInfo<long>("id_supplier");
        //            model.id_edit = GetLoginInfo<long>("id_user");
        //            model.id_create = GetLoginInfo<long>("id_user");

        //            br = BusinessFactory.Goods.Add(model);
        //            if (br.Success)
        //            {
        //                mGoodsList.Add(model);
        //            }
        //            else
        //            {
        //                var dr = table.Select("商品编码='" + bm + "'");
        //                if (dr != null && dr.Length > 0)
        //                {
        //                    dr[0]["备注"] += br.Message[0].ToString() + "/";
        //                }
        //                failGoodsList.Add(model);
        //            }
        //            // 
        //        }
        //        else
        //        {
        //            br.Message.Add(item["备注"].ToString());
        //            failGoodsList.Add(model);
        //        }
        //    }
        //    #endregion
        //}
        //#endregion

        //[ActionPurview(false)]
        //public ActionResult ImgImport()
        //{
        //    return View();
        //}

        //#region 图像导入处理
        ///// <summary>
        ///// 图像导入
        ///// </summary>
        ///// <param name="filePath"></param>
        ///// <returns></returns>
        //[ActionPurview(false)]
        //[HttpPost]
        //public ActionResult ImgImport(string filePath)
        //{
        //    BaseResult br = new BaseResult();
        //    br.Success = true;
        //    Tb_Sp_Sku model = new Tb_Sp_Sku();
        //    model.id_create = GetLoginInfo<long>("id_supplier");
        //    string err = string.Empty;
        //    List<UnZipData> list = null;


        //    //解压处理
        //    var id_user_master = GetLoginInfo<int>("id_user_master");
        //    var unZipDir = Server.MapPath("~/UpLoad/Goods/" + id_user_master);
        //    var fileName = Server.MapPath(filePath);
        //    NPOIHelper.UnZipFile(fileName, unZipDir, out list, out err);
        //    if (err != "")
        //    {
        //        br.Success = false;
        //        br.Message.Add("上传失败！");
        //        return Json(br);
        //    }
        //    //上传处理
        //    int i = 0;
        //    Dictionary<string, string> imgPathList = new Dictionary<string, string>();
        //    if (list != null && list.Count > 0)
        //    {

        //        foreach (var d in list)
        //        {
        //            imgPathList.Clear();
        //            //生成缩略图
        //            var imgPath = "/UpLoad/Goods/" + id_user_master + "/" + d.Bm + "/" + d.Img;
        //            string p_min_2 = string.Format("/UpLoad/Goods/" + id_user_master + "/" + d.Bm + "/_60x60_{0}", d.Img);
        //            string p_min = string.Format("/UpLoad/Goods/" + id_user_master + "/" + d.Bm + "/_200x200_{0}", d.Img);
        //            string p = string.Format("/UpLoad/Goods/" + id_user_master + "/" + d.Bm + "/_480x480_{0}", d.Img);

        //            ImgExtension.MakeThumbnail(imgPath, p, 480, 480, ImgCreateWay.Cut, false);
        //            ImgExtension.MakeThumbnail(imgPath, p_min, 200, 200, ImgCreateWay.Cut, false);
        //            ImgExtension.MakeThumbnail(imgPath, p_min_2, 60, 60, ImgCreateWay.Cut, false);
        //            if (d.ImgList != null && d.ImgList.Count > 0)
        //            {
        //                foreach (var item in d.ImgList)
        //                {
        //                    var imgItem = "/UpLoad/Goods/" + id_user_master + "/" + d.Bm + "/" + item;
        //                    var photo_min = string.Format("/UpLoad/Goods/" + id_user_master + "/" + d.Bm + "/_480x480_{0}", item);
        //                    ImgExtension.MakeThumbnail(imgItem, photo_min, 480, 480, ImgCreateWay.Cut, false);
        //                    imgPathList.Add(imgItem, photo_min);
        //                }
        //            }
        //            //数据处理
        //            model.photo = p;
        //            model.photo_min = p_min;
        //            model.photo_min2 = p_min_2;
        //            model.bm = d.Bm;
        //            var result = BusinessFactory.Goods.UploadImg(model, imgPathList);
        //            if (result.Success)
        //            {
        //                i++;
        //            }
        //        }

        //    }
        //    br.Success = true;
        //    br.Message.Add("成功导入商品个数：" + i + "；失败个数：" + (list.Count - i));
        //    return Json(br);

        //}
        //#endregion

        //[HttpPost]
        //[ActionPurview(false)]
        //public ActionResult CheckGoodsName(string id, string id_gys, string mc)
        //{
        //    BaseResult br = new BaseResult();
        //    br.Success = true;
        //    try
        //    {

        //        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(id_gys) || string.IsNullOrWhiteSpace(id_gys))
        //        {
        //            br.Success = false;
        //            br.Message.Add("请选择查询的商品或参数.");
        //        }

        //        if (id_gys.Equals("0"))
        //        {
        //            id_gys = GetLoginInfo<string>("id_supplier");
        //        }

        //        if (br.Success)
        //        {
        //            Hashtable ht = new Hashtable();
        //            ht.Add("mc", mc);
        //            ht.Add("not_id", id);
        //            ht.Add("id_gys_create", id_gys);
        //            br = BusinessFactory.Goods.GetCount(ht);
        //        }
        //        else
        //            br.Level = ErrorLevel.Warning;

        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Json(br.Success);
        //}

        ///// <summary>
        ///// 新增商品 业务
        ///// znt
        ///// 2015-03-02
        ///// </summary>
        //[HttpPost]
        //[ActionPurview(false)]
        //public ActionResult AddStep1()
        //{
        //    BaseResult br = new BaseResult();
        //    Hashtable param = base.GetParameters();
        //    ParamVessel tp = new ParamVessel();
        //    tp.Add("id", 0, HandleType.Remove);//商品SKU  
        //    tp.Add("mc", string.Empty, HandleType.DefaultValue);
        //    tp.Add("mulspec", string.Empty, HandleType.ReturnMsg);
        //    param = param.Trim(tp);

        //    string sp_mc = param["mc"].ToString();
        //    string mulspec = param["mulspec"].ToString();

        //    br = BusinessFactory.Goods.Stop(param);


        //    List<SpecMul> spec_list = CySoft.Utility.JSON.Deserialize<List<CySoft.Model.Other.SpecMul>>(mulspec);
        //    //屏蔽商品名称为空验证 
        //    //if (string.IsNullOrEmpty(sp_mc))
        //    //{
        //    //    br.Success = false;
        //    //    br.Data = "sp_mc";
        //    //    br.Message.Add("商品名称不能为空");
        //    //    return Json(br);
        //    //}

        //    if (spec_list == null)
        //    {
        //        br.Success = false;
        //        br.Data = "mulspec";
        //        br.Message.Add("商品规格不能为空");
        //        return Json(br);
        //    }

        //    try
        //    {
        //        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        param.Add("id_gys_create", GetLoginInfo<long>("id_supplier"));
        //        br = BusinessFactory.Goods.Get(param);

        //        Tb_Sp_Get sp = null;
        //        if (br.Success) sp = br.Data as Tb_Sp_Get;

        //        List<Tb_Sp_Expand> list_Expand = null;
        //        List<List<Tb_Sp_Expand>> Sp_Expand_list = new List<List<Model.Tb.Tb_Sp_Expand>>();

        //        foreach (var item in spec_list)
        //        {
        //            list_Expand = new List<Tb_Sp_Expand>();
        //            foreach (var val in item.val)
        //            {
        //                list_Expand.Add(new Tb_Sp_Expand() { id_sp_expand_template = item.id_sp_expand_template, val = val });
        //            }
        //            Sp_Expand_list.Add(list_Expand);
        //        }



        //        // 规格组合
        //        List<List<Tb_Sp_Expand>> Sp_Expand_Com_New = new List<List<Tb_Sp_Expand>>();
        //        foreach (var item in Sp_Expand_list)
        //        {
        //            Commbox(ref Sp_Expand_Com_New, item);
        //        }

        //        // 生成编码规则 
        //        param.Clear();
        //        param.Add("coding", typeof(Tb_Sp_Sku).Name.ToLower());
        //        br = BusinessFactory.CodingRule.Get(param);
        //        Ts_Codingrule Codingrule = br.Data as Ts_Codingrule;
        //        List<SkuList> SkuList = new List<Model.Other.SkuList>();

        //        if (Sp_Expand_Com_New.Count > 0)
        //        {
        //            foreach (var item in Sp_Expand_Com_New)
        //            {
        //                bool ishas = false;
        //                if (sp != null && sp.skuList.Count > 0)
        //                {
        //                    foreach (var p in sp.skuList)
        //                    {
        //                        ishas = true;
        //                        foreach (var m in p.specList)
        //                        {
        //                            var n = (from o in item where o.id_sp_expand_template.Equals(m.id_spec_group) && o.val.Equals(m.val) select item).Count();
        //                            if (n.Equals(0))
        //                            {
        //                                ishas = false;
        //                                break;
        //                            }
        //                        }
        //                        if (ishas) break;
        //                    }
        //                }
        //                if (!ishas)
        //                {
        //                    SkuList model = new Model.Other.SkuList();
        //                    long xh_max = BusinessFactory.Utilety.GetNextXh(typeof(Tb_Sp_Sku));
        //                    if (xh_max.ToString().Length < Codingrule.length - Codingrule.prefix.Length)
        //                    {
        //                        model.pcode = Codingrule.prefix + xh_max.ToString().PadLeft(Codingrule.length.Value - Codingrule.prefix.Length, '0');
        //                        model.sp_expand_list = item;
        //                        SkuList.Add(model);
        //                    }

        //                    else
        //                    {
        //                        br.Success = false;
        //                        br.Message.Add("商品编码已超过有效范围！请联系管理员！");
        //                        return Json(br);
        //                    }
        //                }

        //            }
        //        }
        //        else
        //        {
        //            SkuList model = new Model.Other.SkuList();
        //            long xh_max = BusinessFactory.Utilety.GetNextXh(typeof(Tb_Sp_Sku));
        //            if (xh_max.ToString().Length < Codingrule.length - Codingrule.prefix.Length)
        //            {
        //                model.pcode = Codingrule.prefix + xh_max.ToString().PadLeft(Codingrule.length.Value - Codingrule.prefix.Length, '0');
        //                model.sp_expand_list = new List<Tb_Sp_Expand>();
        //                SkuList.Add(model);
        //            }
        //        }


        //        // 客户级别
        //        param.Clear();
        //        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        ViewData["CustomerType"] = BusinessFactory.CustomerType.GetAll(param).Data;

        //        ViewData["sp"] = sp;
        //        ViewData["spec-list"] = spec_list;
        //        return PartialView("_AddStepControl", SkuList);
        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 获取商品编码
        ///// </summary>
        ///// <param name="count">获取个数</param>
        ///// <returns></returns>
        //[HttpPost]
        //[ActionPurview(false)]
        //public ActionResult GetGoodsCode(int count)
        //{
        //    try
        //    {
        //        BaseResult result = new BaseResult();

        //        Hashtable param = new Hashtable();
        //        List<SkuList> list = new List<Model.Other.SkuList>();
        //        param.Add("coding", typeof(Tb_Sp_Sku).Name.ToLower());
        //        BaseResult br = BusinessFactory.CodingRule.Get(param);
        //        Ts_Codingrule Codingrule = br.Data as Ts_Codingrule;
        //        for (int i = 0; i < count; i++)
        //        {
        //            SkuList model = new Model.Other.SkuList();
        //            long xh_max = BusinessFactory.Utilety.GetNextXh(typeof(Tb_Sp_Sku));
        //            if (xh_max.ToString().Length < Codingrule.length - Codingrule.prefix.Length)
        //            {
        //                model.pcode = Codingrule.prefix + xh_max.ToString().PadLeft(Codingrule.length.Value - Codingrule.prefix.Length, '0');
        //                model.sp_expand_list = new List<Tb_Sp_Expand>();
        //                list.Add(model);
        //            }
        //        }
        //        result.Success = true;
        //        result.Data = list;
        //        return Json(result);
        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 缩略图
        ///// znt
        ///// 2015-03-07
        ///// </summary>
        //[HttpPost]
        //[ActionPurview(false)]
        //public ActionResult ThumpView()
        //{
        //    ViewData["imgUrl"] = GetParameter("dataUrl");
        //    return PartialView("_ImgThumpControl");
        //}

        ///// <summary>
        ///// 算法 求规格交叉组合(私有成员)
        ///// znt
        ///// 2015-03-05
        ///// </summary>
        //private void Commbox(ref List<List<Tb_Sp_Expand>> list_Com, List<Tb_Sp_Expand> list_pc)
        //{
        //    if (list_Com.Count == 0)
        //    {
        //        list_Com = new List<List<Tb_Sp_Expand>>();
        //        foreach (var item in list_pc)
        //        {
        //            list_Com.Add(new List<Tb_Sp_Expand>() { item });
        //        }
        //    }
        //    else
        //    {
        //        List<List<Tb_Sp_Expand>> list_Com_new = new List<List<Tb_Sp_Expand>>();

        //        foreach (var item in list_Com)
        //        {
        //            foreach (var item_pc in list_pc)
        //            {
        //                List<Tb_Sp_Expand> list_2 = new List<Tb_Sp_Expand>();
        //                list_2.AddRange(item);
        //                list_2.Add(item_pc);
        //                list_Com_new.Add(list_2);
        //            }

        //        }
        //        list_Com = list_Com_new;
        //    }
        //}
        ///// <summary>
        ///// 检查商品
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ActionPurview(false)]
        //public ActionResult CheckGoods(string model)
        //{
        //    BaseResult br = new BaseResult();
        //    Hashtable param = new Hashtable();
        //    List<CheckGood> goods = new List<CheckGood>();
        //    List<CheckGood> list = new List<CheckGood>();
        //    goods = JSON.Deserialize<List<CheckGood>>(model);
        //    list = JSON.Deserialize<List<CheckGood>>(model);
        //    if (goods != null && goods.Count > 0)
        //    {
        //        int i = 0;
        //        foreach (CheckGood item in goods)
        //        {
        //            param.Clear();
        //            param.Add("id_sku", item.id_sku);
        //            param.Add("flag_up", 1);
        //            br = BusinessFactory.Goods.CheckGood(param);
        //            if (!br.Success)
        //            {
        //                list.RemoveAt(i);
        //            }
        //            i++;
        //        }
        //    }
        //    br.Data = list;
        //    return Json(br);
        //}

        ///// <summary>
        ///// 判断某类别下是否有商品
        ///// </summary>
        ///// <returns></returns>
        //[ActionPurview(false)]
        //public ActionResult IsGoodsTypeHasGoods(long id_spfl)
        //{
        //    var param = new Hashtable();

        //    param.Add("id_spfl", id_spfl);
        //    param.Add("id_gys", GetLoginInfo<long>("id_supplier"));

        //    return Json(new { Data = BusinessFactory.Goods.IsExistsGoods(param), Success = true });
        //}
        ///// <summary>
        ///// 获取条形码图片
        ///// </summary>
        ///// <param name="barCode"></param>
        ///// <returns></returns>
        //[ActionPurview(false)]
        //public ActionResult GetBarcodePicInfo(string barCode)
        //{
        //    return Json(new { Data = BusinessFactory.Goods.GetBarcodePic(barCode), Success = true });
        //}
    }

}
