using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Utility.Mvc.Html;
using System;
using System.Collections;
using System.Web.Mvc;
using CySoft.Model.Ts;
using CySoft.Model.Tb;

namespace CySoft.Controllers.SystemCtl
{
    [LoginActionFilter]
    public class NoticeController : BaseController
    {

        #region 公告列表
        /// <summary>
        /// 公告列表
        /// lz
        /// 2017-02-06
        /// </summary>
        [ValidateInput(false)]
        [ActionPurview(false)]
        public ActionResult List()
        {
            Hashtable param = base.GetParameters();
            try
            {
                int limit = base.PageSizeFromCookie;
                //param.Add("id_masteruser", id_user_master);
                ParamVessel pv = new ParamVessel();
                pv.Add("_search_", "0", HandleType.DefaultValue);
                //pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                pv.Add("s_title", "", HandleType.Remove, true);
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("sort", "rq_create desc", HandleType.DefaultValue);
                param = param.Trim(pv);
                param.Add("flag_type", 1);
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);
                PageNavigate pn = new PageNavigate();
                pn = BusinessFactory.Ts_Notice.GetPage(param);

                var plist = new PageList<Ts_Notice_View>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"];       //排序规则需要返回前台
                ViewData["GetTypeName"] = new Func<string, string>(GetTypeName);
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
        #endregion

        #region 新增公告
        /// <summary>
        /// 新增公告
        /// lz
        /// 2017-02-06
        /// </summary>
        [ValidateInput(false)]
        [HttpGet]
        public ActionResult Add()
        {
            ViewData["option"] = "add";
            return View("_Notice_Edit");
        }

        /// <summary>
        /// 新增公告
        /// lz
        /// 2017-02-06
        /// </summary>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(Ts_Notice model)
        {
            var oldParam = new Hashtable();
            Hashtable param = base.GetParameters();
            param.Add("id_masteruser", id_user_master);
            Hashtable param_model = null;
            BaseResult br = new BaseResult();
            Ts_Notice model_notice = new Ts_Notice();                             //新增对象
            try
            {
                ParamVessel pv = new ParamVessel();
                pv.Add("id_shop_target", string.Empty, HandleType.DefaultValue);//接收公告门店
                pv.Add("id_user_target", string.Empty, HandleType.DefaultValue);//接收公告个人
                pv.Add("flag_type", string.Empty, HandleType.ReturnMsg);//类型
                pv.Add("title", string.Empty, HandleType.ReturnMsg);//标题
                pv.Add("content", string.Empty, HandleType.ReturnMsg);//内容
                param_model = param.Trim(pv);
                param_model.Add("id_masteruser", id_user_master);
                param_model.Add("id_create", id_user);
                param_model.Add("id_shop", id_shop);

                oldParam = (Hashtable)param_model.Clone();
                if (TryUpdateModel(model_notice))
                {
                    model_notice.id_masteruser = id_user_master;
                    model_notice.id_create = id_user;
                    br = BusinessFactory.Ts_Notice.Add(model_notice);
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
            WriteDBLog("新增公告", oldParam, br);
            return JsonString(br, 1);
        }

        #endregion

        #region 编辑公告
        /// <summary>
        /// 编辑公告
        /// lz
        /// 2017-02-06
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Edit()
        {
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                //param.Add("id_masteruser", id_user_master);
                //param.Add("flag_type", 1);
                ViewData["item_edit"] = BusinessFactory.Ts_Notice.Get(param).Data;
                ViewData["option"] = "edit";
                return View("_Notice_Edit");
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        }


        /// <summary>
        /// 编辑公告
        /// lz
        /// 2017-02-06
        /// </summary>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Ts_Notice model)
        {
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            pv.Add("id_shop_target", string.Empty, HandleType.DefaultValue);//接收公告门店
            pv.Add("id_user_target", string.Empty, HandleType.DefaultValue);//接收公告个人
            pv.Add("flag_type", string.Empty, HandleType.ReturnMsg);//类型
            pv.Add("title", string.Empty, HandleType.ReturnMsg);//标题
            pv.Add("content", string.Empty, HandleType.ReturnMsg);//内容

            try
            {
                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                br = BusinessFactory.Ts_Notice.Update(new Ts_Notice()
                {
                    id = param_model["id"].ToString(),
                    id_shop_target = param_model["id_shop_target"].ToString(),
                    id_user_target = param_model["id_user_target"].ToString(),
                    flag_type = byte.Parse(param_model["flag_type"].ToString()),
                    title = param_model["title"].ToString(),
                    content = param_model["content"].ToString(),
                    id_masteruser = id_user_master,
                    id_edit = id_user
                });
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("编辑公告", oldParam, br);
            return JsonString(br, 1);
        }
        #endregion

        #region 删除公告
        /// <summary>
        /// 删除公告
        /// lz
        /// 2017-02-06
        /// </summary>
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
                br = BusinessFactory.Ts_Notice.Delete(param_model);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("删除公告", oldParam, br);
            return JsonString(br, 1);
        }
        #endregion

        #region 公告详情
        /// <summary>
        /// 公告详情
        /// lz
        /// 2017-02-06
        /// </summary>
        [ActionPurview(false)]
        [HttpGet]
        public ActionResult Detail()
        {
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                //param.Add("id_masteruser", id_user_master);
                ViewData["item_edit"] = BusinessFactory.Ts_Notice.Get(param).Data;
                ViewData["option"] = "edit";
                return View();
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        } 
        #endregion



        protected string GetTypeName(string flag_type)
        {
            switch (flag_type)
            {
                case "1": return "系统公告";
            }
            return "未知";
        }


    }
}
