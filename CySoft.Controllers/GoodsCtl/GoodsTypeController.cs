using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using System.Web.UI;
using CySoft.Model;
using CySoft.Model.Tb;
using CySoft.Model.Flags;
using CySoft.Utility;

namespace CySoft.Controllers.GoodsCtl
{
    [LoginActionFilter]
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class GoodsTypeController : BaseController
    {
        ///// <summary>
        ///// 显示商品类别树
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult List()
        //{
        //    BaseResult br = new BaseResult();
        //    try
        //    {

        //        Hashtable param = new Hashtable();
        //        param["sort"] = "xh";
        //        param["dir"] = "asc";
        //        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        br = BusinessFactory.GoodsTpye.GetAll(param);
        //        ViewData["SaveUrl"] = Url.RouteUrl("Default", new { action = "Add", controller = "GoodsType" });
        //        ViewData["DeleteUrl"] = Url.RouteUrl("Default", new { action = "Delete", controller = "Goodstype" });
        //        ViewData["ModifyUrl"] = Url.RouteUrl("Default", new { action = "Update", controller = "Goodstype" });
        //        ViewData["LoadhyNameUrl"] = Url.RouteUrl("Default", new { action = "_HyListControl", controller = "Hangye" });
        //        ViewData["LoadListUrl"] = Url.RouteUrl("Default", new { action = "_GdtListControl", controller = "Goodstype" });

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

        ///// <summary>
        ///// 保存商品类别
        ///// 修改为批量保存
        ///// hjb
        ///// 2016-5-19
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult Add(string info)
        //{

        //    BaseResult br = new BaseResult();
        //    List<Tb_Spfl> spfl_list = JSON.Deserialize<List<Tb_Spfl>>(info);
        //    //Hashtable param = base.GetParameters();
        //    //ParamVessel p = new ParamVessel();
        //    //p.Add("name", string.Empty, HandleType.ReturnMsg);//商品类别
        //    //p.Add("fatherId", (long)0, HandleType.ReturnMsg);//商品类别名称 
        //    //p.Add("id_sp_expand_template", (long)0, HandleType.DefaultValue);//商品属性名称
        //    //param = param.Trim(p);


        //    try
        //    {
        //        if (spfl_list == null && spfl_list.Count <= 0)
        //        {
        //            br.Success = false;
        //            br.Message.Add("至少新增一个商品类别！");
        //            br.Level = ErrorLevel.Warning;
        //            br.Data = "name";
        //            return Json(br);
        //        }

        //        //获取相同商品类别名集合
        //        var sameName = spfl_list.Select(d => new { name = d.name.Trim() }).GroupBy(d => new { d.name }).Where(g => g.Count() > 1);

        //        if (sameName.Count() > 0)
        //        {
        //            br.Success = false;
        //            br.Message.Add(string.Format("商品类别名称【{0}】重复！", sameName.FirstOrDefault().Key.name));
        //            br.Level = ErrorLevel.Warning;
        //            br.Data = "name";
        //            return Json(br);
        //        }

        //        //Tb_Spfl model = new Tb_Spfl();
        //        //添加商品类型
        //        //model.name = param["name"].ToString();
        //        //model.id = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Spfl));
        //        //model.id_gys = GetLoginInfo<long>("id_supplier");
        //        //model.id_create = GetLoginInfo<long>("id_user");
        //        //model.id_edit = model.id_create;
        //        //model.rq_create = DateTime.Now;
        //        //model.rq_edit = DateTime.Now;

        //        DateTime now = DateTime.Now;
        //        var id_supplier = GetLoginInfo<long>("id_supplier");
        //        var id_user = GetLoginInfo<long>("id_user");
        //        //model.fatherId = int.Parse(param["fatherId"].ToString());
        //        //model.id_sp_expand_template = long.Parse(param["id_sp_expand_template"].ToString());

        //        //获取id值

        //        foreach (var item in spfl_list)
        //        {
        //            //item.id = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Spfl));
        //            //item.id_gys = id_supplier;
        //            //item.id_create = id_user;
        //            //item.id_edit = id_user;
        //            //item.rq_create = DateTime.Now;
        //            //item.rq_edit = DateTime.Now;
        //        }

        //        br = BusinessFactory.GoodsTpye.Add(spfl_list);
        //        br.Data = spfl_list.Select(d => new { idList = d.id });
        //        if (br.Success)
        //        {
        //            WriteDBLog(LogFlag.Base, br.Message);
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
        //    return Json(br);
        //}

        ///// <summary>
        ///// 修改商品类别
        ///// </summary>
        //public ActionResult Update(Tb_Spfl model)
        //{
        //    BaseResult br = new BaseResult();
        //    try
        //    {
        //        //if (model.id < 1)
        //        //{
        //        //    br.Success = false;
        //        //    br.Message.Add("商品类别信息丢失，请刷新页面后再试！");
        //        //    br.Level = ErrorLevel.Warning;
        //        //    br.Data = "101";
        //        //    return Json(br);
        //        //}
        //        //if (model.name.IsEmpty())
        //        //{
        //        //    br.Success = false;
        //        //    br.Message.Add("商品类别名称不能为空");
        //        //    br.Level = ErrorLevel.Warning;
        //        //    br.Data = "102";
        //        //    return Json(br);
        //        //}
        //        //if (model.fatherId < 0)
        //        //{
        //        //    br.Success = false;
        //        //    br.Message.Add("商品上级类别不能为空！");
        //        //    br.Level = ErrorLevel.Warning;
        //        //    br.Data = "101";
        //        //    return Json(br);
        //        //}
        //        ////添加商品类型
        //        //model.id_gys = GetLoginInfo<long>("id_supplier");
        //        //model.id_edit = GetLoginInfo<long>("id_user");
        //        //br = BusinessFactory.GoodsTpye.Update(model);
        //        //if (br.Success)
        //        //{
        //        //    WriteDBLog(LogFlag.Base, br.Message);
        //        //}
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
        ///// 加载列表
        ///// </summary>
        //[HttpPost]
        //[ActionPurview(false)]
        //public ActionResult _GdtListControl()
        //{
        //    try
        //    {
        //        BaseResult br = new BaseResult();
        //        Hashtable param = new Hashtable();
        //        param["sort"] = "id";
        //        param["dir"] = "desc";
        //        br = BusinessFactory.GoodsTpye.GetAll(param);
        //        return View(br.Data);
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
        ///// 删除
        ///// </summary>
        //[ValidateInput(false)]
        //public ActionResult Delete()
        //{
        //    BaseResult br = new BaseResult();
        //    Hashtable param = GetParameters();
        //    ParamVessel p = new ParamVessel();
        //    p.Add("id", (long)0, HandleType.ReturnMsg);//商品类别
        //    p.Add("backupId", (long)0, HandleType.ReturnMsg);//商品类别名称          
        //    param = param.Trim(p);
        //    param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //    param.Add("id_user", GetLoginInfo<long>("id_user"));

        //    try
        //    {
        //        if (param["id"].ToString().Equals("0"))
        //        {
        //            br.Success = false;
        //            br.Message.Add("请选择商品要删除的类别");
        //            br.Level = ErrorLevel.Warning;
        //            br.Data = "102";
        //            return Json(br);
        //        }
        //        if (param["backupId"].ToString().Equals("0"))
        //        {
        //            br.Success = false;
        //            br.Message.Add("请选择商品要转移的类别");
        //            br.Level = ErrorLevel.Warning;
        //            br.Data = "102";
        //            return Json(br);
        //        }
        //        br = BusinessFactory.GoodsTpye.Delete(param);
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
        ///// 获取json格式数据
        ///// znt 
        ///// 2015-03-05
        ///// lxt 2015-03-18 改
        ///// znt 2015-04-08 改
        ///// cxb 2015-7-31 改
        ///// </summary>
        //[HttpPost]
        //[ActionPurview(false)]
        //public ActionResult JsonData()
        //{
        //    BaseResult br = new BaseResult();
        //    try
        //    {
        //        Hashtable param = GetParameters();
        //        ParamVessel p = new ParamVessel();
        //        p.Add("childId", 0l, HandleType.Remove);
        //        p.Add("id_gys", GetLoginInfo<long>("id_supplier"), HandleType.DefaultValue);
        //        param = param.Trim(p);
        //        br = BusinessFactory.GoodsTpye.GetTree(param);
        //        if (!br.Success)
        //        {
        //            throw new CySoftException(br);
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
        //    return Json(br);
        //}

        ///// <summary>
        ///// 显示商品类别树
        ///// </summary>
        ///// <returns></returns>
        // [ActionPurview(false)]
        //public ActionResult GoodsTypeToAdv()
        //{
        //    BaseResult br = new BaseResult();
        //    try
        //    {

        //        Hashtable param = new Hashtable();
        //        param["sort"] = "xh";
        //        param["dir"] = "asc";
        //        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
        //        br = BusinessFactory.GoodsTpye.GetAll(param);
        //        ViewData["SaveUrl"] = Url.RouteUrl("Default", new { action = "Add", controller = "GoodsType" });
        //        ViewData["DeleteUrl"] = Url.RouteUrl("Default", new { action = "Delete", controller = "Goodstype" });
        //        ViewData["ModifyUrl"] = Url.RouteUrl("Default", new { action = "Update", controller = "Goodstype" });
        //        ViewData["LoadhyNameUrl"] = Url.RouteUrl("Default", new { action = "_HyListControl", controller = "Hangye" });
        //        ViewData["LoadListUrl"] = Url.RouteUrl("Default", new { action = "_GdtListControl", controller = "Goodstype" });

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
    }
}
