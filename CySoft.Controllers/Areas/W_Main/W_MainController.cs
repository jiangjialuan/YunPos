using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Model.Other;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Utility;
using System.Collections;
using CySoft.Controllers.Filters;

namespace Web.Areas.WeiWeb.Controllers
{
    [W_LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class W_MainController : BaseController
    {
        /// <summary>
        /// 登录页
        /// </summary>
        public ActionResult Main()
        {
            return View();
        }

       
    }
}
