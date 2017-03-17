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
using CySoft.Model.Td;
using System.Linq;
using System.Collections.Generic;
using CySoft.Controllers.Service.Base;

#region 客户管理
#endregion

namespace CySoft.Controllers.SupplierCtl.CustomerCtl
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceReportController : ServiceBaseController
    {
        /// <summary>
        /// 订单统计报表 cxb
        /// </summary>
        [HttpPost]
        public ActionResult OrderStatistic(string obj)
        {
            BaseResult br = new BaseResult();
            PageNavigate pn = new PageNavigate();
            int limit = 100;
            PageList<Td_Report> list = new PageList<Td_Report>(limit);
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                DateTime dt = DateTime.Now;
                DateTime startYear = new DateTime(dt.Year, 1, 1);
                param["start_rq_create"] = startYear;
                param["end_rq_create"] = dt;
                param["id_gys"] = GetLoginInfo<long>("id_supplier");
                param["checktype"]=2;
                int pageIndex = 1;
                param.Add("sort", "rq_create");
                param.Add("dir", "desc");
                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                pn = BusinessFactory.Report.GetPage(param);
             
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
        /// 地区统计报表
        /// </summary>
       [HttpPost]
        public ActionResult AreaStatistic(string obj) 
        {
            BaseResult br = new BaseResult();
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Td_Report> list = new PageList<Td_Report>(limit);
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("checktype", String.Empty, HandleType.Remove);
                p.Add("start_rq_create", string.Empty, HandleType.Remove);
                p.Add("end_rq_create", string.Empty, HandleType.Remove);
                param = param.Trim(p);
                param["id_gys"] = GetLoginInfo<long>("id_supplier");
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                param.Add("sort", "rq_create");
                param.Add("dir", "desc");
                if (param["checktype"] == null) {
                    param["checktype"] = 5;
                }
                if (param["limit"] != null)
                {
                    limit = int.Parse(param["limit"].ToString());
                }
                else
                {
                    param.Add("limit", limit);
                }
                param.Add("start", (pageIndex - 1) * limit);
                pn = BusinessFactory.Report.GetPage(param);
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
        /// 客户订单统计报表
        /// </summary>
        [HttpPost]
       public ActionResult Order(string obj)
        {
            BaseResult br = new BaseResult();
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Td_Report> list = new PageList<Td_Report>(limit);
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("cgs_level", string.Empty, HandleType.Remove);
                p.Add("start_rq_create", string.Empty, HandleType.Remove);
                p.Add("end_rq_create", string.Empty, HandleType.Remove);
                p.Add("limit", string.Empty, HandleType.Remove);
                param = param.Trim(p);
                param["checktype"] = 10;
                param["id_gys"] = GetLoginInfo<long>("id_supplier");
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                param.Add("sort", "rq_create");
                param.Add("dir", "desc");
                if (param["limit"] != null)
                {
                    limit = int.Parse(param["limit"].ToString());
                }
                else {
                    param.Add("limit", limit);
                }
                //param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                pn = BusinessFactory.Report.GetPage(param);
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
        /// 商品销售报表
        /// </summary>
        [HttpPost]
        public ActionResult Sales(string obj) 
        {
            BaseResult br = new BaseResult();
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Td_Report> list = new PageList<Td_Report>(limit);
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("id_spfl_father", String.Empty, HandleType.Remove);
                p.Add("start_rq_create_forstatic", string.Empty, HandleType.Remove);
                p.Add("end_rq_create_forstatic", string.Empty, HandleType.Remove);
                p.Add("limit", string.Empty, HandleType.Remove);
                param = param.Trim(p);
                param["checktype"] = 8;
                param["id_gys_body"] = GetLoginInfo<long>("id_supplier");
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                param.Add("sort", "rq_create");
                param.Add("dir", "desc");
                if (param["limit"] != null)
                {
                    limit = int.Parse(param["limit"].ToString());
                }
                else
                {
                    param.Add("limit", limit);
                }
                param.Add("start", (pageIndex - 1) * limit);
                pn = BusinessFactory.Report.GetPage(param);
                
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
    }
}