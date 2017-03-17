using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using CySoft.Utility.Mvc.Html;
using System.Collections;
using CySoft.Utility;

namespace CySoft.Controllers.FundsCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class BankAccountController : BaseController
    {
        /// <summary>
        /// 列表
        /// znt 2015-04-24
        /// </summary>
        public ActionResult List()
        {
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Tb_Gys_Account> list = new PageList<Tb_Gys_Account>(limit);
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_gys", GetLoginInfo<long>("id_supplier"), HandleType.DefaultValue);
                p.Add("pageIndex", 1, HandleType.DefaultValue);
                param = param.Trim(p);

                int pageIndex = Convert.ToInt32(param["pageIndex"]);

                ViewData["pageIndex"] = pageIndex;
                ViewData["limit"] = limit;

                param.Add("sort", "flag_default");
                param.Add("dir", "desc");
                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                pn = BusinessFactory.BankAccount.GetPage(param);
                list = new PageList<Tb_Gys_Account>(pn, pageIndex, limit);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(list);
        }

        /// <summary>
        /// 新增 
        /// znt 2015-04-24
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult Add(Tb_Gys_Account model)
        {
            BaseResult br = new BaseResult();

            if (model.account_bank.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("银行账号不能为空");
                br.Level = ErrorLevel.Warning;
                br.Data = "account_bank";
                return Json(br);
            }
            if (model.khr.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("开户人不能为空");
                br.Level = ErrorLevel.Warning;
                br.Data = "khr";
                return Json(br);
            }
            if (model.name_bank.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("开户银行不能为空");
                br.Level = ErrorLevel.Warning;
                br.Data = "name_bank";
                return Json(br);
            }

            try
            {
                model.id=BusinessFactory.Utilety.GetNextKey(typeof(Tb_Gys_Account));
                model.id_gys = GetLoginInfo<long>("id_supplier");
                model.id_create = GetLoginInfo<long>("id_user");
                model.id_edit = GetLoginInfo<long>("id_user");
                br = BusinessFactory.BankAccount.Save(model);
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
        /// 设置默认收款账号
        /// znt 2015-04-24
        /// </summary>
        [HttpPost]
        public ActionResult setDefault() {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", string.Empty, HandleType.ReturnMsg);
                p.Add("id_gys", GetLoginInfo<long>("id_supplier"), HandleType.DefaultValue);
                param = param.Trim(p);

                br = BusinessFactory.BankAccount.SetDefault(param);
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
        /// 禁用账号
        /// znt 2015-04-24
        /// </summary>
        [HttpPost]
        public ActionResult Stop()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", string.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                param.Add("id_edit", GetLoginInfo<long>("id_user"));
                br = BusinessFactory.BankAccount.Stop(param);
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
        /// 禁用账号
        /// znt 2015-04-24
        /// </summary>
        [HttpPost]
        public ActionResult Active()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", string.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                param.Add("id_edit", GetLoginInfo<long>("id_user"));
                br = BusinessFactory.BankAccount.Active(param);
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
        /// 删除账号
        /// znt 2015-04-24
        /// </summary>
        [HttpPost]
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", string.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                br = BusinessFactory.BankAccount.Delete(param);
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
        /// 银行账号列表 对话框选择
        /// </summary>
        [ActionPurview(false)]
        public ActionResult ListDialog()
        {
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Tb_Gys_Account> list = new PageList<Tb_Gys_Account>(limit);
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_gys",string.Empty, HandleType.ReturnMsg);
                p.Add("pageIndex", 1, HandleType.DefaultValue);
                param = param.Trim(p);

                int pageIndex = Convert.ToInt32(param["pageIndex"]);

                ViewData["pageIndex"] = pageIndex;
                ViewData["limit"] = limit;

                param.Add("sort", "flag_default");
                param.Add("dir", "desc");
                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                pn = BusinessFactory.BankAccount.GetPage(param);
                list = new PageList<Tb_Gys_Account>(pn, pageIndex, limit);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(list); 
        }
    }
}
