using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Controllers.Filters;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Td;
using CySoft.Model.Ts;
using CySoft.Model.Flags;

#region 订单统计
#endregion

namespace CySoft.Controllers.OrderCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class OrderStatisticsController : BaseController
    {
        /// <summary>
        /// 获取订单统计 cxb 2015-6-2 
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            DateTime dt = DateTime.Now;
            var flag_state_List = new List<int>();
            try
            {
                param["start_rq_create"] = dt.AddMonths(-dt.Month + 1).AddDays(-dt.Day + 1).ToString("yyyy-MM-dd");                
                param["end_rq_create"] = dt.ToString("yyyy-MM-dd");
                ViewData["end_rq_create"] = param["end_rq_create"];
                ViewData["start_rq_create"] = param["start_rq_create"];
                param.Add("not_flag_state",0);
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                flag_state_List.AddRange(new List<int> {(int)OrderFlag.FinanceCheck, (int)OrderFlag.WaitOutputCheck, (int)OrderFlag.WaitDelivery,(int)OrderFlag.Delivered,(int)OrderFlag.Outbounded,(int)OrderFlag.Receipted,(int)OrderFlag.WaitDelivery,(int)OrderFlag.WaitOutputCheck });
                param.Add("flag_state_List", flag_state_List);
                br = BusinessFactory.Report.GetOrderStatistics(param);
            }
            catch (CySoftException ex)
            {

                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(br.Data);
        }

        /// <summary>
        /// 获取订单统计详情 cxb 2015-6-2
        /// </summary>
        /// <returns></returns>
        public ActionResult Details()
        {
            BaseResult br = new BaseResult();
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Td_Sale_Order_Head_Query> list = new PageList<Td_Sale_Order_Head_Query>(limit);
            var flag_state_List = new List<int>();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("status", 0, HandleType.DefaultValue);//排序
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("flag_delete", 0, HandleType.DefaultValue);
                p.Add("flag_show", true, HandleType.Remove);
                p.Add("dh", string.Empty, HandleType.Remove);
                p.Add("alias_gys", string.Empty, HandleType.Remove);
                p.Add("start_rq_create", String.Empty, HandleType.Remove);
                p.Add("end_rq_create", String.Empty, HandleType.Remove);
                p.Add("flag_tj", string.Empty, HandleType.Remove);
                p.Add("flag_fh", string.Empty, HandleType.Remove);
                p.Add("month", string.Empty, HandleType.Remove);
                param = param.Trim(p);
                DateTime dt=DateTime.Now;
                if (param.Contains("start_rq_create"))
                {
                    ViewData["start_rq_create"] = param["start_rq_create"];
                }
                else {
                    int month = 0;
                    if (param["month"] != null)
                    {
                        month = int.Parse(param["month"].ToString());
                    }
                    else {
                        month = dt.Month;
                    }
                    ViewData["start_rq_create"] = new DateTime(dt.Year, month, 1).ToString("yyyy-MM-dd");
                    param["start_rq_create"] = ViewData["start_rq_create"];
                    
                }
                if (param.Contains("end_rq_create"))
                {
                    ViewData["end_rq_create"] = param["end_rq_create"];
                }
                else {
                    int month = 0;
                    if (param["month"] != null)
                    {
                        month = int.Parse(param["month"].ToString());
                    }
                    else {
                        month = dt.Month;
                    }
                    if (dt.Month == month)
                    {
                        ViewData["end_rq_create"] = dt.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        ViewData["end_rq_create"] = new DateTime(dt.Year, month, DateTime.DaysInMonth(dt.Year, month)).ToString("yyyy-MM-dd");
                    }
                    param["end_rq_create"] = ViewData["end_rq_create"];
                }
                flag_state_List.AddRange(new List<int> { (int)OrderFlag.FinanceCheck, (int)OrderFlag.WaitOutputCheck, (int)OrderFlag.WaitDelivery, (int)OrderFlag.Delivered, (int)OrderFlag.Outbounded, (int)OrderFlag.Receipted, (int)OrderFlag.WaitDelivery, (int)OrderFlag.WaitOutputCheck });
                param.Add("flag_state_List", flag_state_List);
                param.Remove("month");
                param.Add("not_flag_state", 0);
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                br = BusinessFactory.Order.GetAccount(param);
                ViewData["model"] = br.Data;
                ViewData["ordercount"] = pn.TotalCount;
                param.Add("sort", "rq_create");
                param.Add("dir", "desc");

                ViewData["pageIndex"] = pageIndex;
                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                param.Add("flag_up", YesNoFlag.Yes);
                pn = BusinessFactory.Order.GetPage(param);

                #region 订单操作日志
                List<Td_Sale_Order_Head_Query> query = pn.Data as List<Td_Sale_Order_Head_Query>;
                if (query != null && query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        param.Clear();
                        param.Add("dh", item.dh);
                        param.Add("sort", "rq");
                        param.Add("dir", "desc");
                        br = BusinessFactory.OrderLog.GetAll(param);

                        if (br.Data != null)
                        {
                            item.order_Log = (br.Data as List<Ts_Sale_Order_Log_Query>);
                        }
                        else
                        {
                            item.order_Log = new List<Ts_Sale_Order_Log_Query>();
                        }

                    }
                }
                pn.Data = query;
                #endregion


                list = new PageList<Td_Sale_Order_Head_Query>(pn, pageIndex, limit);


            }
            catch (CySoftException ex)
            {

                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListControl", list);
            }
            return View(list);
        }
    }
}
