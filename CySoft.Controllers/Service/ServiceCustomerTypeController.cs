using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Core;
using CySoft.Utility;

namespace CySoft.Controllers.ServiceCtl
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceCustomerTypeController : ServiceBaseController
    {
        /// <summary>
        /// 不分页查询客户级别
        /// lxt
        /// 2015-03-06
        /// </summary>
        [HttpPost]
        public ActionResult GetAll(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("name", String.Empty, HandleType.Remove);//名称
                p.Add("remark", String.Empty, HandleType.Remove);//备注
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
