using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;

namespace CySoft.Controllers.AdminCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class PosFunctionController:BaseController
    {
        [ActionPurview(true)]
        public ActionResult Index()
        {
            try
            {

                return View(BusinessFactory.Tb_Post_Function.GetFunctionTree(new Hashtable()));
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [ActionPurview(true)]
        public ActionResult AddPosFun()
        {
            var res = HandleResult(() =>
            {
                BaseResult result=new BaseResult(){Success = false};
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("name", String.Empty, HandleType.ReturnMsg);
                p.Add("flag_system", 0, HandleType.ReturnMsg);
                p.Add("functiondescribe", String.Empty, HandleType.DefaultValue);
                p.Add("flag_type", 0, HandleType.ReturnMsg);
                p.Add("maxvalue", String.Empty, HandleType.DefaultValue);
                p.Add("minvalue", String.Empty, HandleType.DefaultValue);
                p.Add("sort_id", 0, HandleType.DefaultValue);
                p.Add("bm", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                Tb_Pos_Function entity = new Tb_Pos_Function();
                if (TryUpdateModel(entity))
                {
                    result = BusinessFactory.Tb_Post_Function.Add(entity);
                }
                else
                {
                    result.Message.Add("参数有误!");
                }
                return result;
            });
            return JsonString(res);
        }

        [ActionPurview(true)]
        public ActionResult UpdatePosFun()
        {
            var res = HandleResult(() =>
            {
                BaseResult result=new BaseResult();
                var param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("name", String.Empty, HandleType.ReturnMsg);
                p.Add("flag_system", 0, HandleType.ReturnMsg);
                p.Add("functiondescribe", String.Empty, HandleType.DefaultValue);
                p.Add("flag_type", 0, HandleType.ReturnMsg);
                p.Add("maxvalue", String.Empty, HandleType.DefaultValue);
                p.Add("minvalue", String.Empty, HandleType.DefaultValue);
                p.Add("sort_id", 0, HandleType.DefaultValue);
                p.Add("bm", String.Empty, HandleType.ReturnMsg);
                p.Add("id", 0, HandleType.ReturnMsg);
                param = param.Trim(p);
                Tb_Pos_Function entity = new Tb_Pos_Function();
                if (TryUpdateModel(entity))
                {
                    result = BusinessFactory.Tb_Post_Function.Update(entity);
                }
                else
                {
                    result.Message.Add("参数有误!");
                }
                return result;
            });
            return JsonString(res);
        }

        public ActionResult GetAll()
        {
            var res= HandleResult(() =>
            {
                var param = base.GetParameters();
                param.Add("sort", "rq_create");
                param.Add("dir", "asc");
                return BusinessFactory.Tb_Post_Function.GetAll(param);
            });
            return JsonString(res);
        }


        [ActionPurview(true)]
        public ActionResult GetModulTree()
        {
            try
            {
                var param = new Hashtable();
                return Json(new { Success = true, Data = BusinessFactory.Tb_Post_Function.GetFunctionTree(param) }, JsonRequestBehavior.AllowGet);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionPurview(true)]
        public ActionResult List()
        {
            Hashtable param = base.GetParameters();
            try
            {
                int limit = base.PageSizeFromCookie;
                ParamVessel p = new ParamVessel();
                p.Add("_search_", "0", HandleType.DefaultValue);
                p.Add("s_mc", "", HandleType.DefaultValue, true);
                p.Add("page", 0, HandleType.DefaultValue);
                p.Add("pageSize", limit, HandleType.DefaultValue);
                param = param.Trim(p);
                int.TryParse(param["pageSize"].ToString(), out limit);
                PageNavigate pn = new PageNavigate();
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);
                pn = BusinessFactory.Tb_Post_Function.GetPage(param);
                var plist = new PageList<Tb_Pos_Function>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            ViewData["SelectListItems"]= GetSelectListItems();
            if (param["_search_"].ToString().Equals("1"))
            {
                return PartialView("_List");
            }
            else
            {
                return View();
            }
        }


        private  List<SelectListItem> GetSelectListItems()
        {
            List<SelectListItem> selectList=new List<SelectListItem>()
            {
                new SelectListItem(){Text = "PCPos",Value = "1"},
                new SelectListItem(){Text = "安卓Pos",Value = "2"},
                new SelectListItem(){Text = "手机",Value = "3"}
            };
            return selectList;
        }
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Edit()
        {
            var param = base.GetParameters();
            ParamVessel pv=new ParamVessel();
            pv.Add("id","",HandleType.ReturnMsg);
            param = param.Trim(pv);
            var enity= BusinessFactory.Tb_Post_Function.Get(param).Data as Tb_Pos_Function;
            enity = enity ?? new Tb_Pos_Function();
            SelectList selectListSys = new SelectList(GetSelectListItems(), "Value", "Text", enity.flag_system);
            ViewData["selectListSys"] = selectListSys;
            SelectList selectListType=new SelectList(new List<SelectListItem>()
            {
                new SelectListItem(){Text = "是否",Value = "1"},
                new SelectListItem(){Text = "输入值",Value = "2"}
            },"Value","Text",enity.flag_type);
            ViewData["selectListType"] = selectListType;
            SelectList selectListStop = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem(){Text = "停用",Value = "1"},
                new SelectListItem(){Text = "可用",Value = "0"}
            }, "Value", "Text", enity.flag_stop);
            ViewData["selectListStop"] = selectListStop;
            ViewData["option"] = "edit";
            ViewData["item_edit"] = enity;
            return View("_Edit");
        }
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Edit(Tb_Pos_Function model)
        {
            BaseResult res=HandleResult(() =>
            {
                return BusinessFactory.Tb_Post_Function.Update(model);
            });
            return JsonString(res, 1);
        }
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Add()
        {
            SelectList selectListSys = new SelectList(GetSelectListItems(), "Value", "Text");
            ViewData["selectListSys"] = selectListSys;
            SelectList selectListType = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem(){Text = "是否",Value = "1"},
                new SelectListItem(){Text = "输入值",Value = "2"}
            }, "Value", "Text");
            ViewData["selectListType"] = selectListType;
            SelectList selectListStop = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem(){Text = "可用",Value = "0"},
                new SelectListItem(){Text = "停用",Value = "1"}
                
            }, "Value", "Text");
            ViewData["selectListStop"] = selectListStop;
            ViewData["option"] = "add";
            return View("_Edit");
        }
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Add(Tb_Pos_Function model)
        {
            BaseResult res = HandleResult(() =>
            {
                return BusinessFactory.Tb_Post_Function.Add(model);
            });
            return JsonString(res, 1);
        }
        [ActionPurview(true)]
        public ActionResult Delete()
        {
            BaseResult res = HandleResult(() =>
            {
                var param = base.GetParameters();
                ParamVessel pv=new ParamVessel();
                pv.Add("id","0",HandleType.ReturnMsg);
                param = param.Trim(pv);
                return BusinessFactory.Tb_Post_Function.Delete(param);
            });
            return JsonString(res, 1);
        }

    }


}
