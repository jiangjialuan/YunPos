using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Core;
using CySoft.Utility;
using CySoft.Model.Flags;
using CySoft.Model.Tb;

#region 收藏夹
#endregion

namespace CySoft.Controllers.Service
{

    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceFavoritesController : ServiceBaseController
    {
        /// <summary>
        /// 分页查询
        /// tim
        /// 2015-05-13
        /// </summary>
        [HttpPost]
        public ActionResult GetPage(string obj)
        {
            PageNavigate pn = new PageNavigate();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("id_spfl", (long)0, HandleType.Remove);//商品分类Id
                p.Add("id_gys", (long)0, HandleType.Remove);//供应商Id                
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("limit", 20, HandleType.DefaultValue);//当前页码
                p.Add("sort", "gsp.rq_create", HandleType.DefaultValue);
                p.Add("dir", "desc", HandleType.DefaultValue);
                param = param.Trim(p);

                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                int limit = Convert.ToInt32(param["limit"]);
                if (limit < 1)
                {
                    limit = 20;
                }
                if (limit > 200)
                {
                    limit = 200;
                }

                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("start", (pageIndex - 1) * limit);
                param.Add("flag_up", YesNoFlag.Yes);
                param.Add("flag_stop", YesNoFlag.No);
                param.Add("baseurl", Request.Url.Scheme + "://" + Request.Url.Authority);

                pn = BusinessFactory.GoodsFavorites.GetPage(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(pn);
        }


        /// <summary>
        /// 添加收藏商品
        /// tim
        /// 2015-05-13
        /// </summary>
        [HttpPost]
        public ActionResult Add(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("id_gys", (long)0, HandleType.ReturnMsg);//供应商Id
                p.Add("id_sp", (long)0, HandleType.ReturnMsg);//商品id
                param = param.Trim(p);

                var model = new Tb_Sp_Favorites();
                model.id_sp = long.Parse(param["id_sp"].ToString());
                model.id_gys = long.Parse(param["id_gys"].ToString());
                model.id_user = GetLoginInfo<long>("id_user");

                //model.xh = 0;排序
                model.id_create = model.id_user;
                model.id_edit = model.id_user;

                var list = new List<Tb_Sp_Favorites>();
                list.Add(model);
                br = BusinessFactory.GoodsFavorites.Add(list);
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
        /// 2015-05-13
        /// </summary>
        [HttpPost]
        public ActionResult Delete(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("id_gys", (long)0, HandleType.ReturnMsg);//供应商Id
                p.Add("id_sp", (long)0, HandleType.ReturnMsg);//商品id
                param = param.Trim(p);
                //========================================================
                var model = new Tb_Sp_Favorites();
                model.id_sp = long.Parse(param["id_sp"].ToString());
                model.id_gys = long.Parse(param["id_gys"].ToString());
                model.id_user = GetLoginInfo<long>("id_user");
                //model.xh = 0;排序
                model.id_create = model.id_user;
                model.id_edit = model.id_user;

                var list = new List<Tb_Sp_Favorites>();
                list.Add(model);
                param.Clear();
                param.Add("sp", list);
                br = BusinessFactory.GoodsFavorites.Delete(param);
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
