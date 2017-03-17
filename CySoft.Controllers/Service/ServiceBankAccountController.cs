using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Controllers.Service.Base;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Frame.Core;
using CySoft.Controllers.Base;
using CySoft.Model.Tb;
using CySoft.Utility.Mvc.Html;
using System.Collections;
using CySoft.Utility;

namespace CySoft.Controllers.Service
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceBankAccountController : ServiceBaseController
    {
        /// <summary>
        /// 列表
        /// znt 2015-04-24
        /// </summary>
        [HttpPost]
        public ActionResult List(string obj)
        {
            PageNavigate pn = new PageNavigate();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("id_gys",String.Empty, HandleType.ReturnMsg);
                p.Add("pageIndex", 1, HandleType.DefaultValue);
                p.Add("limit", 20, HandleType.DefaultValue);
                p.Add("sort", "flag_default",HandleType.DefaultValue);
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
                param.Add("start", (pageIndex - 1) * limit);
                pn = BusinessFactory.BankAccount.GetPage(param);
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
        /// 供应商默认银行账号
        /// znt 2015-05-08
        /// </summary>
        [HttpPost]
        public ActionResult GetDefault(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("id_gys", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                br = BusinessFactory.BankAccount.GetDefault(param);
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
