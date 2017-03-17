using System;
using System.Collections.Generic;
using System.Linq;
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
using CySoft.Model.Td;
using CySoft.Utility;
using CySoft.Model.Ts;
using CySoft.Controllers.Service.Base;

namespace CySoft.Controllers.GoodsCtl
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceShippingRecordController : ServiceBaseController
    {
        
        /// <summary>
        /// 采购商确认发货
        /// cxb
        /// 2015-4-17
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Confirm(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                br = BusinessFactory.ShippingRecord.Active(param);
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
        /// 出库/出货记录详细 
        /// cxb 
        /// 2015-3-18 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Detail(string role, string dh)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            Hashtable paramli = new Hashtable();
            try
            {
                //获取订单单头
                if (dh != null)
                {
                    param["dh"] = dh;
                    ViewData["dh"] = dh;
                }
                else {
                    param["dh"] = Request["dh"];
                    ViewData["dh"] = Request["dh"];
                    dh = Request["dh"];
                }
                br = BusinessFactory.Order.Get(param);
                ViewData["OrderHead"] = br.Data;
                //获取订单单体
                br = BusinessFactory.Order.GetAll(param);
                ViewData["OrderbodyList"] = br.Data;

                //获取出库单头
                paramli["dh_order"] = dh;
                paramli["sort"] = "flag_state asc, rq_fh desc  , rq_create";
                paramli["dir"] = "desc";
                paramli["not_flag_state"] = CySoft.Model.Flags.OrderFlag.Deleted;
                br = BusinessFactory.ShippingRecord.Get(paramli);
                ViewData["OrderOutHead"] = br.Data;

                //获取出库单单体
                paramli["sort"] = "dh";
                br=BusinessFactory.ShippingRecord.GetAll(paramli);
                
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(br.Data);
        }
    
    }
}
