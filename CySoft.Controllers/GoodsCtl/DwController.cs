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
using CySoft.Model;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using CySoft.Utility.Mvc.Html;

namespace CySoft.Controllers.GoodsCtl
{
    [LoginActionFilter]
    public class DwController : BaseController
    {
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
                pv.Add("s_dw", "", HandleType.Remove, true);
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("sort", "rq_create desc", HandleType.DefaultValue);
                param = param.Trim(pv);
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);
                PageNavigate pn = new PageNavigate();
                pn = BusinessFactory.Tb_Dw.GetPage(param);

                var plist = new PageList<Tb_DwWithUserName>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"];       //排序规则需要返回前台
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
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Add()
        {
            ViewData["option"] = "add";
            Hashtable param = base.GetParameters();
            ParamVessel pv = new ParamVessel();
            pv.Add("other_add", "0", HandleType.DefaultValue);
            param = param.Trim(pv);
            ViewData["other_add"] = param["other_add"].ToString();
            return View("_Dw_Edit");
        }
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Edit()
        {
            try
            {
                Hashtable param = base.GetParameters();
                param.Add("id_masteruser", id_user_master);
                ParamVessel p = new ParamVessel();
                p.Add("id_masteruser", id_user_master, HandleType.DefaultValue);
                p.Add("id", String.Empty, HandleType.ReturnMsg);
                ViewData["item_edit"] = BusinessFactory.Tb_Dw.Get(param).Data;
                ViewData["option"] = "edit";
                return View("_Dw_Edit");
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// 新增商品单位
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult Add(Tb_Dw model)
        {
            var oldParam = new Hashtable();
            Hashtable param = base.GetParameters();
            param.Add("id_masteruser", id_user_master);
            Hashtable param_model = null;
            BaseResult br = new BaseResult();
            Tb_Dw model_dw = new Tb_Dw();                             //新增对象

            try
            {
                ParamVessel pv = new ParamVessel();
                pv.Add("dw", string.Empty, HandleType.ReturnMsg);            //名称
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                if (TryUpdateModel(model_dw))
                {
                    model_dw.id_masteruser = id_user_master;
                    model.id_create = model.id_edit = id_user;
                    br = BusinessFactory.Tb_Dw.Add(model_dw);
                }
                else
                {
                    br.Message.Add("参数有误!");
                }
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("新增商品单位", oldParam, br);
            return JsonString(br, 1);
        }

        /// <summary>
        /// 编辑商品单位
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Edit(Tb_Dw model)
        {
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("dw", string.Empty, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            try
            {
                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                br = BusinessFactory.Tb_Dw.Update(new Tb_Dw()
                {
                    id = param_model["id"].ToString(),
                    dw = param_model["dw"].ToString(),
                    id_masteruser = id_user_master,
                    id_edit = id_user
                });
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("编辑商品单位", oldParam, br);
            return JsonString(br, 1);
        }

        /// <summary>
        /// 删除分类
        /// LD
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult Delete()
        {
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            param.Add("id_masteruser", id_user_master);
            pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            try
            {
                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                br = BusinessFactory.Tb_Dw.Delete(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("删除商品单位", oldParam, br);
            return JsonString(br, 1);
        }


        /// <summary>
        /// 获取单位 api
        /// lz
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult GetDW()
        {
            var dwlist = new List<Tb_Dw>();
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            var br = BusinessFactory.Tb_Dw.GetAll(param);
            if (br.Success)
            {
                dwlist = (List<Tb_Dw>)br.Data;
                if (dwlist != null && dwlist.Count() > 0)
                    dwlist = dwlist.OrderBy(d => d.dw).ToList();
            }
            return JsonString(dwlist);
        }



    }
}
