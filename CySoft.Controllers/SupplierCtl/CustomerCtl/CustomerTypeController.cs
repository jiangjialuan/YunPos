using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Utility;
using CySoft.Model.Tb;
using CySoft.Controllers.Filters;

#region 客户级别管理
#endregion

namespace CySoft.Controllers.SupplierCtl.CustomerCtl
{
    [LoginActionFilter]
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class CustomerTypeController : BaseController
    {
        /// <summary>
        /// 客户级别管理页面
        /// lxt
        /// 2015-02-11
        /// </summary>
        public ActionResult List()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("name", String.Empty, HandleType.Remove);
                p.Add("remark", String.Empty, HandleType.Remove);
                param = param.Trim(p);
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                br = BusinessFactory.CustomerType.GetAll(param);
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
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListControl", br.Data);
            }
            return View(br.Data);
        }

        /// <summary>
        /// 客户级别编辑页面
        /// lxt
        /// 2015-02-12
        /// </summary>
        [HttpPost]
        public ActionResult Edit()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("flag_edit", "Add", HandleType.DefaultValue);
                p.Add("id", (long)0, HandleType.Remove);
                param = param.Trim(p);
                ViewData["flag_edit"] = param["flag_edit"];
                if (param["flag_edit"].ToString() == "Update")
                {
                    param.Remove("flag_edit");
                    if (!param.ContainsKey("id"))
                    {
                        br.Success = false;
                        br.Message.Add("参数错误，请联系管理员！");
                        br.Level = ErrorLevel.Warning;
                        return Json(br);
                    }
                    br = BusinessFactory.CustomerType.Get(param);
                }
                else
                {
                    br.Data = new Tb_Cgs_Level();
                    br.Success = true;
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
            if (!br.Success)
            {
                return Json(br);
            }
            return PartialView("_EditControl", br.Data);
        }

        /// <summary>
        /// 添加用户
        /// lxt
        /// 2015-02-12
        /// </summary>
        [HttpPost]
        public ActionResult Add(Tb_Cgs_Level model)
        {
            BaseResult br = new BaseResult();
            try
            {
                if (model.name.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("请输入级别名称！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "name";
                    return Json(br);
                }
                if (model.agio < 1m)
                {
                    br.Success = false;
                    br.Message.Add("请输入正确的折扣！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "agio";
                    return Json(br);
                }
                model.id = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Cgs_Level));
                model.id_gys = GetLoginInfo<long>("id_supplier");
                model.id_create = GetLoginInfo<long>("id_user");
                model.id_edit = model.id_create;
                br = BusinessFactory.CustomerType.Add(model);
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
        /// 修改用户
        /// lxt
        /// 2015-02-12
        /// </summary>
        [HttpPost]
        public ActionResult Update(Tb_Cgs_Level model)
        {
            BaseResult br = new BaseResult();
            try
            {
                if (model.id < 1)
                {
                    br.Success = false;
                    br.Message.Add("请刷新页面后再试！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "id";
                    return Json(br);
                }
                if (model.name.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("请输入级别名称！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "name";
                    return Json(br);
                }
                if (model.agio < 1m)
                {
                    br.Success = false;
                    br.Message.Add("请输入正确的折扣！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "agio";
                    return Json(br);
                }
                model.id_edit = GetLoginInfo<long>("id_user");
                model.id_gys = GetLoginInfo<long>("id_supplier");
                br = BusinessFactory.CustomerType.Update(model);
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
        /// 删除
        /// lxt
        /// 2015-02-12
        /// </summary>
        [HttpPost]
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                param = param.Trim(p);
                br = BusinessFactory.CustomerType.Delete(param);
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
        /// 获取不分页json数据
        /// lxt
        /// 2015-02-27
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult JsonData()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("name", String.Empty, HandleType.Remove);
                p.Add("remark", String.Empty, HandleType.Remove);
                param = param.Trim(p);
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                br = BusinessFactory.CustomerType.GetAll(param);
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
