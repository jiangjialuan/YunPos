using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Tb;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;

#region 客户管理
#endregion

namespace CySoft.Controllers.SupplierCtl.CustomerCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class PromotionsController : BaseController
    {
        /// <summary>
        /// 商品促销 cxb 2015-3-10 
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult List(string role)
        {
            return View("Buyers_List");
        }
    }
}