using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Controllers.Base;
using System.Web.Mvc;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Tb;
using CySoft.Controllers.Filters;
using System.Web.UI;
using CySoft.Model.Flags;
using CySoft.Utility;
using CySoft.Model.Td;
using CySoft.Model.Ts;

namespace CySoft.Controllers
{

    [LoginActionFilter]
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class MainController : BaseController
    {
        /// <summary>
        /// mq 2016-05-30 修改:我的最新通知排除业务，系统，升级公告
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult Index()
        {
            try
            {
                if (UserRole == RoleFlag.Other)
                {
                    //PageNavigate pn = GetSupplierIndex();

                    //if (Request.IsAjaxRequest())
                    //{
                    //    return PartialView("_IndexSupplierControl", pn.Data);
                    //}
                }
                else
                {
                    //PageNavigate pnBuyer = GetBuyerIndex();
                    //PageNavigate pnSupplier = GetSupplierIndex();

                    if (Request.IsAjaxRequest())
                    {
                        Hashtable param = base.GetParameters();

                        //if (param.ContainsKey("reporttype"))
                        //{
                        //    var type = param["reporttype"].ToString();

                        //    if (type.Equals("supplier"))
                        //    {
                        //        return PartialView("_IndexSupplierControl", pnSupplier.Data);
                        //    }
                        //    else if (type.Equals("buyer"))
                        //    {
                        //        return PartialView("_IndexBuyerControl", pnBuyer.Data);
                        //    }
                        //}
                    }

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

            return View();
        }

        private PageNavigate GetBuyerIndex()
        {
            PageNavigate pn = new PageNavigate();
            string checktype = "";
            DateTime dt = DateTime.Now;
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                var flag_state_List = new List<int>();
                p.Add("checktype", String.Empty, HandleType.Remove);
                param = param.Trim(p);
                if (param["checktype"] != null)
                {
                    checktype = param["checktype"].ToString();
                }
                else
                {
                    checktype = null;
                }
                //long id_cgs = GetLoginInfo<long>("id_buyer");
                string id_user_master = GetLoginInfo<string>("id_user_master");
                param.Add("id_user", GetLoginInfo<string>("id_user"));
                param.Add("sort", "rq");
                param.Add("dir", "desc");
                br = BusinessFactory.Log.Get(param);
                ViewData["buyer_Log"] = (Ts_Log)br.Data ?? new Ts_Log() { rq = DateTime.Now };
                param.Clear();
                param["day"] = dt.Day;
                param["month"] = dt.Month;
                param["year"] = dt.Year;
                //param["id_cgs"] = id_cgs;
                param.Add("not_flag_state", 0);
                br = BusinessFactory.Report.GetAll(param);
                ViewData["buyer_day"] = br.Data;
                param.Clear();
                param["month"] = dt.Month;
                param["year"] = dt.Year;
                //param["id_cgs"] = id_cgs;
                param.Add("not_flag_state", 0);
                br = BusinessFactory.Report.GetAll(param);
                ViewData["buyer_month"] = br.Data;
                param.Clear();
                param["year"] = dt.Year;
                //param["id_cgs"] = id_cgs;
                param.Add("not_flag_state", 0);
                br = BusinessFactory.Report.GetAll(param);
                ViewData["buyer_year"] = br.Data;
                param.Clear();
                param["flag_state"] = 10;
                flag_state_List.AddRange(new List<int> { (int)OrderFlag.Submitted, (int)OrderFlag.OrderCheck, (int)OrderFlag.FinanceCheck, (int)OrderFlag.WaitOutputCheck, (int)OrderFlag.WaitDelivery });
                param.Add("flag_state_List", flag_state_List);
                //param["id_cgs"] = id_cgs;
                br = BusinessFactory.Report.GetAll(param);
                ViewData["buyer_Pending"] = br.Data;
                param.Clear();
                if (checktype == null)
                {
                    param["rq_create"] = Convert.ToDateTime(dt.AddDays(1 - (dt.Day))).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    param["end_create"] = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    param["checktype"] = checktype;
                }
                if (checktype == "2")
                {
                    param["rq_create"] = Convert.ToDateTime(dt.AddMonths(-6)).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    param["end_create"] = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    param["checktype"] = "2";
                }
                else if (checktype == "3")
                {
                    param["rq_create"] = Convert.ToDateTime(dt.AddYears(-1)).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    param["end_create"] = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    param["checktype"] = "2";
                }
                //param["id_cgs"] = id_cgs;
                pn = BusinessFactory.Report.GetDataforImg(param);
                param.Clear();
                //我的通知信息(只获取最新的10条数据)
                PageList<Info_Query> lst = new PageList<Info_Query>(10);
                param.Add("limit", 10);
                param.Add("start", 0);
                param.Add("id_user", GetLoginInfo<string>("id_user"));
                string[] bm = new string[] { "update", "system", "business" };
                param.Add("bmList", bm);
                PageNavigate pn1 = BusinessFactory.InfoUser.GetPage(param);
                lst = new PageList<Info_Query>(pn1, 1, 10);
                ViewData["buyer_infoList"] = lst;
                ViewData["buyer_id_cgs"] = GetLoginInfo<string>("id_buyer");
                ViewData["buyer_report_data"] = pn.Data;
                return pn;
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private PageNavigate GetSupplierIndex()
        {
            PageNavigate pn = new PageNavigate();
            string checktype = "";
            DateTime dt = DateTime.Now;
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                var flag_state_List = new List<int>();
                p.Add("checktype", String.Empty, HandleType.Remove);
                param = param.Trim(p);
                if (param["checktype"] != null)
                {
                    checktype = param["checktype"].ToString();
                }
                else
                {
                    checktype = null;
                }
                //long id_gys = GetLoginInfo<long>("id_supplier");
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("sort", "rq");
                param.Add("dir", "desc");
                br = BusinessFactory.Log.Get(param);
                ViewData["supplier_Log"] = (Ts_Log)br.Data ?? new Ts_Log() { rq = DateTime.Now };
                param.Clear();
                param["day"] = dt.Day;
                param["month"] = dt.Month;
                param["year"] = dt.Year;
                //param["id_gys"] = id_gys;
                param.Add("not_flag_state", 0);
                br = BusinessFactory.Report.GetAll(param);
                ViewData["supplier_day"] = br.Data;
                param.Clear();
                param["month"] = dt.Month;
                param["year"] = dt.Year;
                //param["id_gys"] = id_gys;
                param.Add("not_flag_state", 0);
                //br = BusinessFactory.Report.GetAll(param);
                ViewData["supplier_month"] = br.Data;
                param.Clear();
                param["year"] = dt.Year;
                //param["id_gys"] = id_gys;
                param.Add("not_flag_state", 0);
                //br = BusinessFactory.Report.GetAll(param);
                ViewData["supplier_year"] = br.Data;
                param.Clear();
                param["flag_state"] = 10;
                flag_state_List.AddRange(new List<int> { (int)OrderFlag.Submitted, (int)OrderFlag.OrderCheck, (int)OrderFlag.FinanceCheck, (int)OrderFlag.WaitOutputCheck, (int)OrderFlag.WaitDelivery });
                param.Add("flag_state_List", flag_state_List);
                //param["id_gys"] = id_gys;
                //br = BusinessFactory.Report.GetAll(param);
                ViewData["supplier_Pending"] = br.Data;
                param.Clear();
                if (checktype == null)
                {
                    param["rq_create"] = Convert.ToDateTime(dt.AddDays(1 - (dt.Day))).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    param["end_create"] = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    param["checktype"] = checktype;
                }
                if (checktype == "2")
                {
                    param["rq_create"] = Convert.ToDateTime(dt.AddMonths(-6)).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    param["end_create"] = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    param["checktype"] = "2";
                }
                else if (checktype == "3")
                {
                    param["rq_create"] = Convert.ToDateTime(dt.AddYears(-1)).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    param["end_create"] = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    param["checktype"] = "2";
                }
                //param["id_gys"] = id_gys;
                //pn = BusinessFactory.Report.GetDataforImg(param);
                param.Clear();
                //我的通知信息(只获取最新的10条数据)
                PageList<Info_Query> lst = new PageList<Info_Query>(10);
                param.Add("limit", 10);
                param.Add("start", 0);
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                string[] bm = new string[] { "update", "system", "business" };
                param.Add("bmList", bm);
                PageNavigate pn1 = BusinessFactory.InfoUser.GetPage(param);
                lst = new PageList<Info_Query>(pn1, 1, 10);
                ViewData["supplier_infoList"] = lst;
                ViewData["supplier_id_cgs"] = GetLoginInfo<long>("id_buyer");
                ViewData["supplier_report_data"] = pn.Data;
                return pn;
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
