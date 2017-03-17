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
using CySoft.Model.Tb;
using CySoft.Utility.Mvc.Html;
using CySoft.Utility;

#region 收藏夹
#endregion

namespace CySoft.Controllers.GoodsCtl
{
    [LoginActionFilter]
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class FavoritesController : BaseController
    {
        /// <summary>
        /// 收藏商品列表
        /// </summary>
        [ActionPurview(false)]
        public ActionResult Index()
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
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                param = param.Trim(p);
                switch (param["orderby"].ToString())
                {
                    case "2":
                        param["orderby"] = 2;
                        param.Add("sort", "db.id_sp");
                        param.Add("dir", "asc");
                        break;
                    case "3":
                        param.Add("sort", "gsp.bm_Interface");
                        param.Add("dir", "asc");
                        break;
                    case "4":
                        param.Add("sort", "gsp.bm_Interface");
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
                        param.Add("sort", "gsp.rq_create");
                        param.Add("dir", "asc");
                        break;
                    case "8":
                        param.Add("sort", "gsp.rq_create");
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
                ViewData["keyword"] = GetParameter("keyword");
                if (Request.IsAjaxRequest())
                {
                    param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                    param.Add("id_user", GetLoginInfo<long>("id_user"));
                    param.Add("limit", limit);
                    param.Add("start", (pageIndex - 1) * limit);
                    param.Add("flag_up", YesNoFlag.Yes);
                    param.Add("flag_stop", YesNoFlag.No);
                    pn = BusinessFactory.GoodsFavorites.GetPage(param);
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
                return PartialView("_ListControl", list);
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
            return View();
        }        

        /// <summary>
        /// 添加收藏商品
        /// tim
        /// 2015-05-12
        /// </summary>
        [ValidateInput(false)]
        [ActionPurview(false)]
        public ActionResult Add()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                //p.Add("supplierid", (long)0, HandleType.ReturnMsg);//供应商Id
                //p.Add("id_sp", (long)0, HandleType.ReturnMsg);//商品id
                p.Add("obj", string.Empty, HandleType.ReturnMsg);//商品
                param = param.Trim(p);

                var list = JSON.Deserialize <List<Tb_Sp_Favorites>>(param["obj"].ToString());

                if (list != null && list.Count > 0)
                {
                    foreach (var model in list)
                    {
                        model.id_user = GetLoginInfo<long>("id_user");
                        model.id_create = model.id_user;
                        model.id_edit = model.id_user;
                    }

                    br = BusinessFactory.GoodsFavorites.Add(list);
                }
                else {
                    br.Success = false;
                    br.Level = ErrorLevel.Question;
                    br.Message.Add("请选择商品收藏.");
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
            return Json(br);
        }

        /// <summary>
        /// 取消收藏商品
        /// tim
        /// 2015-05-12
        /// </summary>
        [ValidateInput(false)]
        [ActionPurview(false)]
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("obj", string.Empty, HandleType.ReturnMsg);//商品
                param = param.Trim(p);

                var list = JSON.Deserialize<List<Tb_Sp_Favorites>>(param["obj"].ToString());

                if (list != null && list.Count > 0)
                {
                    foreach (var model in list)
                    {
                        model.id_user = GetLoginInfo<long>("id_user");                       
                    }
                    param.Clear();
                    param.Add("sp",list);
                    br = BusinessFactory.GoodsFavorites.Delete(param);
                }
                else
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Question;
                    br.Message.Add("请选择收藏商品.");
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
            return Json(br);
        }
    }
}
