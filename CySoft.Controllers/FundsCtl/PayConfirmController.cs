using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Controllers.Filters;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Frame.Core;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Td;
using System.Collections;

namespace CySoft.Controllers.FundsCtl
{
    //[LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class PayConfirmController : BaseController
    {
        /// <summary>
        /// 收款确认列表 cxb 2015-4-23
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            BaseResult br = new BaseResult();
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Td_sale_pay_Query> list = new PageList<Td_sale_pay_Query>(limit);
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("start_rq_create", String.Empty, HandleType.Remove);
                p.Add("end_rq_create", String.Empty, HandleType.Remove);
                p.Add("searchValuebycgs", String.Empty, HandleType.Remove,true);
                param = param.Trim(p);

                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                ViewData["pageIndex"] = pageIndex;
                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                param.Add("sort", "rq_create");
                param.Add("dir", "desc");
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                param["flag_state"] = 1;
                param["not_flag_delete"] = 1;

                pn = BusinessFactory.Funds.GetPage(param);
                
                list = new PageList<Td_sale_pay_Query>(pn, pageIndex, limit);

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
            return View(list);
        }

        /// <summary>
        /// 删除收款记录 cxb 2015-4-22
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult DeleteRecord(Td_sale_pay_Query model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            try
            {
                param["dh"] = model.dh;
                param["new_flag_delete"] = 1;
                br = BusinessFactory.Funds.Update(param);
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
        /// 确认收款 
        /// cxb 2015-4-23
        /// znt 2015-04-27
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult Submit()
        {
            BaseResult br = new BaseResult();
            
            try
            {
                Hashtable param =GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                param.Add("id_edit", GetLoginInfo<long>("id_user"));
                br = BusinessFactory.Funds.PayConfirm(param);
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
