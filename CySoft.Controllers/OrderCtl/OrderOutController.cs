using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Controllers.GoodsCtl;
using CySoft.Frame.Core;
using CySoft.Model.Td;
using CySoft.Model.Ts;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Flags;

#region 销售出库单
#endregion

namespace CySoft.Controllers.SaleCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class OrderOutController : BaseController
    {
        /// <summary>
        /// 出库/出货记录 cxb 2015-3-3 
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Td_Sale_Out_Head_Query> list = new PageList<Td_Sale_Out_Head_Query>(limit);
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                param = param.Trim(p);

                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                ViewData["keyword"] = GetParameter("keyword");
                ViewData["orderby"] = param["orderby"];
                ViewData["pageIndex"] = pageIndex;
                ViewData["limit"] = limit;

                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                pn = BusinessFactory.ShippingRecord.GetPage(param);
                list = new PageList<Td_Sale_Out_Head_Query>(pn, pageIndex, limit);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(list);
        }

        /// <summary>
        /// 收货确认
        /// cxb 2015-5-7 修改
        /// </summary>
        public ActionResult Confirm()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);//订单单号-出库
                p.Add("dh_order", string.Empty, HandleType.ReturnMsg);//订单单号
                param = param.Trim(p);
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
        /// 出库/出货记录详
        /// cxb 2015-3-18 
        /// 2015-04-16 lxt 改
        /// </summary>
        public ActionResult Item(string id)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            Hashtable paramli = new Hashtable();
            try
            {
                ParamVessel p = new ParamVessel();
                p.Add("dh", null, HandleType.ReturnMsg);
                param.Add("dh", id);
                param = param.Trim(p);

                //获取订单单头 Td_Sale_Order_Head_Query
                br = BusinessFactory.Order.Get(param);
                ViewData["OrderHead"] = br.Data;

                //获取订单单体 Td_Sale_Order_Body_Query
                br = BusinessFactory.Order.GetAll(param);
                ViewData["OrderbodyList"] = br.Data;

                
                //获取子单列表
                List<string> Ls = BusinessFactory.GoodsSkuFd.Get_Fd_Sale_Order_List(id);
                if (Ls.Count > 0)
                {
                    Ls.Add(id);
                    paramli["dh_orderList"] = Ls;
                }
                else
                {
                //获取出库单头
                paramli["dh_order"] = id;
                }

                paramli["sort"] = "flag_state asc, rq_fh desc  , rq_create";
                paramli["dir"] = "desc";
                paramli["not_flag_state"] = OrderFlag.Deleted;

                br = BusinessFactory.ShippingRecord.Get(paramli);
                ViewData["OrderOutHead"] = br.Data;
                
                //获取出库单单体 Td_Sale_Out_Body_Query
                paramli["sort"] = "dh";
                
                br = BusinessFactory.ShippingRecord.GetAll(paramli);
                
                if (Ls.Count > 0)
                {
                    var Sale_Body = ViewData["OrderbodyList"] as List<Td_Sale_Order_Body_Query>;
                    var Out_Body = br.Data as List<Td_Sale_Out_Body_Query>;

                    if (Sale_Body != null)
                    {
                        foreach (var item in Sale_Body)
                        {
                            if (Out_Body == null) continue;

                            foreach (var i in Out_Body)
                            {
                                if (item.id_sku == i.id_sku)
                                {
                                    var dh_flag = BusinessFactory.GoodsSkuFd.Get_Sale_Out_Head_Query(i.dh);
                                    if (dh_flag != null)
                                    {
                                        if (dh_flag.flag_state == OrderFlag.Outbounded ||
                                            dh_flag.flag_state == OrderFlag.Receipted ||
                                            dh_flag.flag_state == OrderFlag.WaitDelivery ||
                                            dh_flag.flag_state == OrderFlag.WaitOutputCheck ||
                                            dh_flag.flag_state == OrderFlag.Delivered )
                                        {
                                            item.sl_ck = item.sl_ck + i.sl;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    ViewData["OrderbodyList"] = Sale_Body;
                }
                var Order_Fd = BusinessFactory.GoodsSkuFd.Query_Sale_Order_Fd(id);
                ViewData["FD"] = "0";
                if (Order_Fd != null) ViewData["FD"] = "1";

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

    }
}
