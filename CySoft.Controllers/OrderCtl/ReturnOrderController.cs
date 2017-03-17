using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using System.Web.UI;
using CySoft.Model.Tb;
using CySoft.Model.Flags;
using CySoft.Utility.Mvc.Html;

namespace CySoft.Controllers.GoodsCtl
{
    [LoginActionFilter]
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ReturnOrderController : BaseController
    {
        public ActionResult List()
        {
            return View();
        }
    }
}
