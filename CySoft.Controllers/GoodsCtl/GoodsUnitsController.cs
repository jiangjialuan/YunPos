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
using CySoft.Model.Tb;
using CySoft.Model.Flags;
using CySoft.Utility;

namespace CySoft.Controllers.GoodsCtl
{
    [LoginActionFilter]
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class GoodsUnitsController : BaseController
    {
        /// <summary>
        /// 显示商品类别树
        /// 
        /// 2015-03-23 lxt 改
        /// </summary>
        public ActionResult List()
        {
            BaseResult br = new BaseResult();
            try
            {
               
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("sort", "id", HandleType.DefaultValue);
                p.Add("dir", "desc", HandleType.DefaultValue);
                param = param.Trim(p);
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                br = BusinessFactory.GoodsUnits.GetAll(param);
                ViewData["AddUrl"] = Url.RouteUrl("Default", new { action = "Add", controller = "GoodsUnits" });
                ViewData["DeleteUrl"] = Url.RouteUrl("Default", new { action = "Delete", controller = "GoodsUnits" });
                
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
        /// 新增UI
        /// lxt
        /// 2015-03-25
        /// </summary>
        [ActionPurview(false)]
        public ActionResult AddView()
        {
            return PartialView("_AddControl");
        }

        /// <summary>
        /// 添加加量单位 cxb 2015-3-3 
        /// 2015-03-25 lxt 修改
        /// </summary>
        public ActionResult Add(Tb_Unit model)
        {
            BaseResult br = new BaseResult();
            try
            {
                if (model.name.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("计量单位名称不能为空");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "101";
                    return Json(br);
                }
                //添加商品类型
                //model.id = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Spfl));
                model.id_gys = GetLoginInfo<long>("id_supplier");
                br = BusinessFactory.GoodsUnits.Add(model);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
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
        /// 删除计量单位
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                br = BusinessFactory.GoodsUnits.Delete(param);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
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
        /// 获取json格式数据
        /// znt 
        /// 2015-03-05
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult JsonData()
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            try
            {
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                br = BusinessFactory.GoodsUnits.GetAll(param);
                if (!br.Success)
                {
                    throw new CySoftException(br);
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
