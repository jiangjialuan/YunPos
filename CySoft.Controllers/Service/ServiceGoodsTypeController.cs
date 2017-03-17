using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Core;
using CySoft.Utility;

#region 商品分类
#endregion

namespace CySoft.Controllers.ServiceCtl
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceGoodsTypeController : ServiceBaseController
    {
        /// <summary>
        /// 获得树结构数据
        /// lxt
        /// 2015-03-11
        /// </summary>
        [HttpPost]
        public ActionResult GetTree(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("name", String.Empty, HandleType.Remove);//名称
                p.Add("id_gys", GetLoginInfo<long>("id_supplier"), HandleType.DefaultValue);//供应商
                param = param.Trim(p);
                
                br = BusinessFactory.GoodsTpye.GetTree(param);
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
