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
using CySoft.Utility.Mvc.Html;

#region 行业管理
#endregion


namespace CySoft.Controllers.IndustrieCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class IndustrieController : BaseController
    {
        /// <summary>
        /// 获取json格式数据
        /// lxt
        /// 2015-03-03
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        [ActionPurview(false)]
        public ActionResult JsonData()
        {
            BaseResult br = new BaseResult();
            try
            {
                br = BusinessFactory.Industrie.GetTree();
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
