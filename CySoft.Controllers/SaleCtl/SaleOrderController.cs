using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Td;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Ts;
using CySoft.Utility;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.IO;
using System.Text;
using CySoft.Controllers.GoodsCtl;

#region 销售订单
#endregion

namespace CySoft.Controllers.SaleCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class SaleOrderController : BaseController
    {
        /// <summary>
        /// 订单列表
        /// znt 2015-03-27
        /// </summary>
        public ActionResult List()
        {
            BaseResult br = new BaseResult();
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Td_Sale_Order_Head_Query> list = new PageList<Td_Sale_Order_Head_Query>(limit);
            Hashtable param = new Hashtable();
            try
            {
                param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                var flag_state_List = new List<int>();
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("status", 0, HandleType.DefaultValue);//订单状态筛选
                p.Add("orderby", 1, HandleType.DefaultValue);// 排序
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("flag_delete", 0, HandleType.DefaultValue);
                p.Add("flag_show", true, HandleType.Remove);
                p.Add("flag_state_List", string.Empty, HandleType.Remove);
                p.Add("dh", string.Empty, HandleType.Remove);
                p.Add("alias_cgs", string.Empty, HandleType.Remove, true);
                p.Add("start_rq_create_ss", String.Empty, HandleType.Remove);
                p.Add("end_rq_create_ss", String.Empty, HandleType.Remove);
                p.Add("flag_out", string.Empty, HandleType.Remove);
                p.Add("flag_tj", string.Empty, HandleType.Remove);
                p.Add("flag_fh", string.Empty, HandleType.Remove);
                param = param.Trim(p);
                if (param["keyword"] != null)
                {
                    ViewData["keyword"] = param["keyword"];
                }
                if (param["flag_state_List"] != null)
                {
                    ViewData["flag_state_List"] = param["flag_state_List"].ToString();
                    string[] flag_stateListStr = param["flag_state_List"].ToString().Split(',');
                    //flag_stateListStr.ToList<string, int>(s =>Convert.ToInt32(s)).ToArray();
                    int[] flag_stateListInt = Array.ConvertAll<string, int>(flag_stateListStr, s => Convert.ToInt32(s));
                    param["flag_state_List"] = flag_stateListInt;

                }
                if (param["flag_out"] != null)
                {
                    ViewData["flag_out"] = param["flag_out"];
                }
                if (param["flag_fh"] != null)
                {
                    ViewData["flag_fh"] = param["flag_fh"];
                }
                if (param["flag_tj"] != null)
                {
                    ViewData["flag_tj"] = param["flag_tj"];
                }
                if (param["start_rq_create"] != null)
                {
                    ViewData["start_rq_create"] = param["start_rq_create"];
                }
                if (param["end_rq_create"] != null)
                {
                    ViewData["end_rq_create"] = param["end_rq_create"];
                }
                if (param["dh"] != null)
                {
                    ViewData["dh"] = param["dh"];
                    ViewData["Search_dh"] = param["dh"];
                }
                if (param["alias_cgs"] != null)
                {
                    ViewData["alias_cgs"] = param["alias_cgs"];
                }
                if (param["flag_show"] != null)
                {
                    ViewData["flag_show"] = true;
                }
                switch (param["status"].ToString())
                {
                    case "1": //待处理订单 -- 订单审核、财务审核、待出库审核、待发货审核
                        flag_state_List.AddRange(new List<int> { (int)OrderFlag.Submitted, (int)OrderFlag.OrderCheck, (int)OrderFlag.FinanceCheck, (int)OrderFlag.WaitOutputCheck, (int)OrderFlag.WaitDelivery });
                        param.Add("flag_state_List", flag_state_List);
                        param.Remove("status");
                        ViewData["status"] = 1;
                        break;
                    case "2": //已完成订单  -- 已收货
                        flag_state_List.Add((int)OrderFlag.Receipted);
                        param.Add("flag_state_List", flag_state_List);
                        param.Remove("status");
                        ViewData["status"] = 2;
                        break;
                    case "3": //已作废订单 -- 已作废
                        flag_state_List.Add((int)OrderFlag.Invalided);
                        param.Add("flag_state_List", flag_state_List);
                        param.Remove("status");
                        ViewData["status"] = 3;
                        break;
                    case "4"://未完成订单 -- -- 订单审核、财务审核、待出库审核、待发货审核、已发货
                        flag_state_List.AddRange(new List<int> { (int)OrderFlag.Submitted, (int)OrderFlag.OrderCheck, (int)OrderFlag.FinanceCheck, (int)OrderFlag.WaitOutputCheck, (int)OrderFlag.WaitDelivery, (int)OrderFlag.Delivered });
                        param.Add("flag_state_List", flag_state_List);
                        param.Remove("status");
                        ViewData["status"] = 4;
                        break;
                    default:
                        param.Remove("status");
                        ViewData["status"] = 0;
                        break;
                }

                switch (param["orderby"].ToString())
                {
                    case "0":
                        param.Remove("orderby");
                        param.Add("sort", "rq_create");
                        param.Add("dir", "asc");
                        ViewData["orderby"] = 0;
                        break;
                    case "1":
                        param.Remove("orderby");
                        param.Add("sort", "rq_create");
                        param.Add("dir", "desc");
                        ViewData["orderby"] = 1;
                        break;
                    case "2":
                        param.Remove("orderby");
                        param.Add("sort", "flag_state");
                        param.Add("dir", "asc");
                        ViewData["orderby"] = 2;
                        break;
                    case "3":
                        param.Remove("orderby");
                        param.Add("sort", "flag_state");
                        param.Add("dir", "desc");
                        ViewData["orderby"] = 3;
                        break;
                    case "4":
                        param.Remove("orderby");
                        param.Add("sort", "shr");
                        param.Add("dir", "asc");
                        ViewData["orderby"] = 4;
                        break;
                    case "5":
                        param.Remove("orderby");
                        param.Add("sort", "shr");
                        param.Add("dir", "desc");
                        ViewData["orderby"] = 5;
                        break;
                    default:
                        param.Remove("orderby");
                        param.Add("sort", "rq_create");
                        param.Add("dir", "desc");
                        ViewData["orderby"] = 1;
                        break;
                }


                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));


                ViewData["pageIndex"] = pageIndex;
                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                param.Add("flag_up", YesNoFlag.Yes);
                pn = BusinessFactory.Order.GetPage(param);

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
            return View("List", list);
        }

        /// <summary>
        /// 新增
        /// lxt
        /// 2015-04-15
        /// </summary>
        //[ActionPurview(false)]
        public ActionResult Add()
        {
            BaseResult br = new BaseResult();
            var bm_List = new List<string>();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("source", "PcNew", HandleType.DefaultValue);
                param = param.Trim(p);

                long id_gys = GetLoginInfo<long>("id_supplier");
                ViewData["GridLoadListUrl"] = Url.RouteUrl("Default", new { action = "GetBody", controller = "SaleOrder", gys = id_gys, source = param["source"].ToString().Trim() });
                ViewData["id_gys"] = id_gys;
                ViewData["tax"] = GetLoginInfo<decimal>("tax");
                ViewData["vat"] = GetLoginInfo<decimal>("vat");
                //获取系统参数设置值
                param.Clear();
                param["id_user_master"] = GetLoginInfo<long>("id_user_master");

                br = BusinessFactory.Setting.GetAll(param);

                ViewData["order_settingList"] = br.Data;
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

        /// <summary>
        /// 提交 新增销售单
        /// znt 2015-04-18
        /// </summary>
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult Submit()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                int cgs_send = Convert.ToInt32(param["cgs_send"]);
                param.Remove("id_cgs_send");
                ParamVessel p = new ParamVessel();
                p.Add("invoiceFlag", String.Empty, HandleType.ReturnMsg);
                p.Add("orderData", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                Hashtable param2 = new Hashtable();
                //备份原入参
                param2.Add("orderData", param["orderData"]);
                param2.Add("invoiceFlag", param["invoiceFlag"]);
                param2.Add("soure", param["soure"]);

                Td_Sale_Order_Head_Query model = JSON.Deserialize<Td_Sale_Order_Head_Query>(param["orderData"].ToString());
                if (string.IsNullOrEmpty(model.id_cgs.ToString()))
                {
                    br.Success = false;
                    br.Message.Add("校验失败！请重新刷新页面。");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "none";
                    return Json(br);
                }

                if (model.order_body.Count <= 0)
                {
                    br.Success = false;
                    br.Message.Add("商品不能为空！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "sku";
                    return Json(br);
                }
                if (string.IsNullOrEmpty(model.shr))
                {
                    br.Success = false;
                    br.Message.Add("收货人不能为空！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "shr";
                    return Json(br);
                }
                if (string.IsNullOrEmpty(model.phone))
                {
                    br.Success = false;
                    br.Message.Add("联系号码不能为空！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "phone";
                    return Json(br);
                }

                // 详细地址
                //if (string.IsNullOrEmpty(model.address))
                //{
                //    br.Success = false;
                //    br.Message.Add("收货地址不能为空！");
                //    br.Data = "address";
                //    return Json(br);
                //}

                model.id_gys = GetLoginInfo<long>("id_supplier");
                model.id_user_bill = GetLoginInfo<long>("id_user");
                model.id_user_master = GetLoginInfo<long>("id_user_master");
                model.id_create = GetLoginInfo<long>("id_user");
                model.id_edit = GetLoginInfo<long>("id_user");

                // 单号 赋值 
                br = BusinessFactory.Utilety.GetNextDH(model, typeof(Td_Sale_Order_Head));

                if (!br.Success)
                {
                    return Json(br);
                }

                // 发票类型 
                string invoiceFlag = param["invoiceFlag"].ToString();
                switch (invoiceFlag)
                {
                    case "2":
                        model.invoiceFlag = InvoiceFlag.General;
                        break;
                    case "3":
                        model.invoiceFlag = InvoiceFlag.Vat;
                        break;
                    default:
                        model.invoiceFlag = InvoiceFlag.None;
                        break;
                }

                if (model.invoiceFlag == InvoiceFlag.General && model.slv == 0)
                {
                    model.slv = GetLoginInfo<decimal>("tax");
                }
                if (model.invoiceFlag == InvoiceFlag.Vat && model.slv == 0)
                {
                    model.slv = GetLoginInfo<decimal>("vat");
                }
                long newOrderLogId = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));


                param2.Add("newOrderLogId", newOrderLogId);
                param2.Add("id_user", GetLoginInfo<long>("id_user"));
                param2.Add("model", model);
                param2.Add("OrderSource", OrderSourceFlag.PcNew);

                br = BusinessFactory.SaleOrder.Add(param2);
                if (br.Success)
                {
                    br.Data = model.dh;
                    WriteDBLog(LogFlag.Bill, br.Message);
                    param.Clear();
                    BaseResult br1 = new BaseResult();

                    long id_info = BusinessFactory.Utilety.GetNextKey(typeof(Info));//获取下一个Id自增值
                    param.Add("cgs_send", cgs_send);
                    param.Add("id", id_info);
                    param.Add("Title", GetLoginInfo<string>("companyname") + "代您下单了，" + model.dh);
                    param.Add("content", "order," + model.dh);
                    param.Add("filename", "");
                    param.Add("fileSize", "");
                    param.Add("id_info_type", 0);
                    param.Add("id_create", GetLoginInfo<long>("id_user"));
                    param.Add("id_master", GetLoginInfo<long>("id_user_master"));
                    param.Add("flag_from", "pc");
                    param.Add("bm", "business");
                    br1 = BusinessFactory.Info.Add(param);
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

        /// <summary>
        /// 订单详情
        /// lxt 2015-04-15
        /// znt 2015-05-05 修改 
        /// </summary>
        [ActionPurview(false)]
        public ActionResult Item(string id)
        {
            BaseResult br = new BaseResult();
            try
            {
                // 获取单头
                Hashtable param = new Hashtable();
                param = GetParameters();
                param.Add("dh", id);
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                p.Add("pageIndex", String.Empty, HandleType.Remove);
                p.Add("flag_show", String.Empty, HandleType.Remove);
                p.Add("alias_cgs", String.Empty, HandleType.Remove);
                p.Add("dh", String.Empty, HandleType.Remove);
                p.Add("Search_dh", String.Empty, HandleType.Remove);
                p.Add("flag_state_List", String.Empty, HandleType.Remove);
                p.Add("flag_out", String.Empty, HandleType.Remove);
                p.Add("flag_fh", String.Empty, HandleType.Remove);
                p.Add("flag_tj", String.Empty, HandleType.Remove);
                p.Add("start_rq_create", String.Empty, HandleType.Remove);
                p.Add("end_rq_create", String.Empty, HandleType.Remove);
                p.Add("id_info", String.Empty, HandleType.Remove);
                param = param.Trim(p);
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                ViewData["dh"] = param["dh"];
                if (param["pageIndex"] != null)
                {
                    ViewData["pageIndex"] = param["pageIndex"];
                }
                if (param["flag_out"] != null)
                {
                    ViewData["flag_out"] = param["flag_out"];
                    param.Remove("flag_out");
                }
                if (param["start_rq_create"] != null)
                {
                    ViewData["start_rq_create"] = param["start_rq_create"];
                    param.Remove("start_rq_create");
                }
                if (param["end_rq_create"] != null)
                {
                    ViewData["end_rq_create"] = param["end_rq_create"];
                    param.Remove("end_rq_create");
                }
                if (param["flag_fh"] != null)
                {
                    ViewData["flag_fh"] = param["flag_fh"];
                    param.Remove("flag_fh");
                }
                if (param["flag_tj"] != null)
                {
                    ViewData["flag_tj"] = param["flag_tj"];
                    param.Remove("flag_tj");
                }
                if (param["flag_state_List"] != null)
                {
                    ViewData["flag_state_List"] = param["flag_state_List"];
                    param.Remove("flag_state_List");
                }
                if (param["flag_show"] != null)
                {
                    ViewData["flag_show"] = param["flag_show"];
                    param.Remove("flag_show");
                }
                if (param["alias_cgs"] != null)
                {
                    ViewData["alias_cgs"] = param["alias_cgs"];
                    param.Remove("alias_cgs");
                }
                if (param["Search_dh"] != null)
                {
                    ViewData["Search_dh"] = param["Search_dh"];
                    param.Remove("Search_dh");
                }
                br = BusinessFactory.SaleOrder.Get(param);
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
        /// 修改
        /// znt 2015-05-04
        /// </summary>
        [ActionPurview(false)]
        public ActionResult Edit()
        {
            Td_Sale_Order_Head_Query model = new Td_Sale_Order_Head_Query();
            try
            {
                BaseResult br = new BaseResult();
                Hashtable param = new Hashtable();
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("actionName", "EditDj");
                param.Add("controllerName", "SaleOrder");
                br = BusinessFactory.AccountFunction.Check(param);
                ViewBag.isEditDj = br.Success;
                param.Clear();
                param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dh", string.Empty, HandleType.DefaultValue);
                p.Add("source", "PcClone", HandleType.DefaultValue);
                param = param.Trim(p);

                string dh = param["dh"].ToString();
                long id_gys = GetLoginInfo<long>("id_supplier");
                string source = param["source"].ToString();

                param.Clear();
                param.Add("dh", dh);
                model = BusinessFactory.SaleOrder.Get(param).Data as Td_Sale_Order_Head_Query;
                if (model != null)
                {
                    ViewData["id_gys"] = id_gys;
                    ViewData["GridLoadListUrl"] = Url.RouteUrl("Default", new { action = "GetBody", controller = "SaleOrder", dh = dh.ToString(), source = source.Trim() });
                }
                else
                {
                    model = new Td_Sale_Order_Head_Query();
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


            return View(model);
        }

        /// <summary>
        /// 修改保存
        /// znt 2015-05-05
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult SaveEdit()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("orderData", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                Td_Sale_Order_Head_Query model = JSON.Deserialize<Td_Sale_Order_Head_Query>(param["orderData"].ToString());
                if (string.IsNullOrEmpty(model.dh))
                {
                    br.Success = false;
                    br.Message.Add("校验失败！请重新刷新页面。");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "dh";
                    return Json(br);
                }

                if (model.order_body.Count <= 0)
                {
                    br.Success = false;
                    br.Message.Add("商品不能为空！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "sku";
                    return Json(br);
                }

                long newOrderLogId = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                param.Clear();
                param.Add("newOrderLogId", newOrderLogId);
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("model", model);
                param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                br = BusinessFactory.SaleOrder.Edit(param);

                if (br.Success)
                {
                    br.Data = model.dh;
                    WriteDBLog(LogFlag.Bill, br.Message);
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

        /// <summary>
        /// 审核
        /// lxt
        /// 2015-04-15
        /// </summary>
        [HttpPost]
        public ActionResult Check()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                p.Add("new_flag_state", (byte)0, HandleType.ReturnMsg);
                p.Add("jsonarray", String.Empty, HandleType.Remove);

                param = param.Trim(p);
                OrderFlag new_flag_state;
                if (!Enum.TryParse<OrderFlag>(param["new_flag_state"].ToString(), out new_flag_state))
                {
                    br.Success = false;
                    br.Message.Add(" 数据验证失败！请重新刷新页面。");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                param["newOrderLogId"] = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                param["new_flag_state"] = new_flag_state;
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
                br = BusinessFactory.SaleOrder.Check(param);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Bill, br.Message);
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

        /// <summary>
        /// 订单审核
        /// znt 2015-04-17
        /// </summary>
        [HttpPost]
        public ActionResult CheckOrder()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                p.Add("jsonarray", String.Empty, HandleType.Remove);
                param = param.Trim(p);

                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param["newOrderLogId"] = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                //IList<Ts_Param_Business> listq = (IList<Ts_Param_Business>)BusinessFactory.ProcessFun.GetProcess((long)param["id_user_master"]).Data;
                br = BusinessFactory.SaleOrder.CheckOrder(param);

                if (br.Success)
                {
                    WriteDBLog(LogFlag.Bill, br.Message);
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
        /// <summary>
        /// 批量订单审核
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult CheckOrderList(string obj)
        {
            BaseResult br = new BaseResult();
            if (string.IsNullOrEmpty(obj) || obj == "[]")
            {
                br.Success = false;
                br.Message.Add("操作失败！没有符合的订单。");
                br.Level = ErrorLevel.Warning;
                return Json(br);
            }
            try
            {
                List<string> list = JSON.Deserialize<List<string>>(obj);
                Td_Sale_Order_Head_Query saleOrder = new Td_Sale_Order_Head_Query();
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                Hashtable param = new Hashtable();
                long id_user = GetLoginInfo<long>("id_user");
                long id_user_master = GetLoginInfo<long>("id_user_master");
                long id_gys = GetLoginInfo<long>("id_supplier");
                foreach (string item in list)
                {
                    param.Clear();
                    param.Add("dh", item);
                    br = BusinessFactory.SaleOrder.Get(param);
                    saleOrder = JSON.Deserialize<Td_Sale_Order_Head_Query>(JSON.Serialize(br.Data));
                    if (saleOrder.flag_tj == 1 && saleOrder.flag_state == 10)
                    {
                        for (int i = 0; i < saleOrder.order_body.Count; i++)
                        {
                            if (i == 0)
                            {
                                sb.AppendFormat("{xh:{0},je_pay:{1}}", saleOrder.order_body[i].xh, saleOrder.order_body[i].je_pay);
                            }
                            else
                            {
                                sb.AppendFormat(",{xh:{0},je_pay:{1}}", saleOrder.order_body[i].xh, saleOrder.order_body[i].je_pay);
                            }

                        }
                        sb.Append("]");
                    }
                    param.Add("jsonarray", sb.ToString());
                    param["newOrderLogId"] = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                    param.Add("id_user", id_user);
                    param.Add("id_user_master", id_user_master);
                    param.Add("id_gys", id_gys);
                    br = BusinessFactory.SaleOrder.CheckOrder(param);
                    if (br.Success)
                    {
                        WriteDBLog(LogFlag.Bill, br.Message);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Json(br);
        }
        /// <summary>
        /// 财务审核
        /// znt 2015-04-17
        /// </summary>
        [HttpPost]
        public ActionResult CheckFinance()
        {
            BaseResult br = new BaseResult();

            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param["newOrderLogId"] = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                br = BusinessFactory.SaleOrder.CheckFinance(param);

                if (br.Success)
                {
                    WriteDBLog(LogFlag.Bill, br.Message);
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
        /// <summary>
        /// 批量财务审核
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult CheckFinanceList(string orders)
        {
            BaseResult br = new BaseResult();

            if (string.IsNullOrEmpty(orders) || orders == "[]")
            {
                br.Success = false;
                br.Message.Add("操作失败！没有符合的订单。");
                br.Level = ErrorLevel.Warning;
                return Json(br);
            }
            try
            {
                List<string> list = JSON.Deserialize<List<string>>(orders);

                if (list != null && list.Count > 0)
                {
                    Hashtable param = new Hashtable();
                    int success = 0;
                    long id_user = GetLoginInfo<long>("id_user");
                    param["newOrderLogId"] = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                    long id_user_master = GetLoginInfo<long>("id_user_master");
                    long id_gys = GetLoginInfo<long>("id_supplier");
                    foreach (string item in list)
                    {
                        param.Clear();
                        param.Add("dh", item);
                        param.Add("id_user", id_user);
                        param.Add("id_user_master", id_user_master);
                        param.Add("id_gys", id_gys);
                        br = BusinessFactory.SaleOrder.CheckFinance(param);
                        if (br.Success)
                        {
                            success++;
                        }
                    }
                    br.Message.Add(string.Format("共处理{0}项订单，成功{1}项。", list.Count, success));
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Json(br);
        }
        /// <summary>
        /// 退回
        /// lxt
        /// 2015-04-15
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Back()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                p.Add("remark", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                param["newOrderLogId"] = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
                param.Add("id_supplier", GetLoginInfo<long>("id_supplier"));
                br = BusinessFactory.SaleOrder.UnCheck(param);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Bill, br.Message);
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

        /// <summary>
        /// 作废
        /// lxt
        /// 2015-04-15
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Invalid()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                p.Add("remark", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                param["newOrderLogId"] = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
                param.Add("id_supplier", GetLoginInfo<long>("id_supplier"));
                string dh = param["dh"].ToString();
                br = BusinessFactory.SaleOrder.Invalid(param);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Bill, br.Message);
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

        /// <summary>
        /// 删除订单 
        /// znt 2015-04-20
        /// </summary>
        /// flag_delete 状态为0
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dh", string.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                param.Add("new_id_edit", GetLoginInfo<long>("id_user"));
                param.Add("new_rq_edit", DateTime.Now);
                string dh = param["dh"].ToString();
                br = BusinessFactory.Order.Stop(param);

                if (br.Success)
                {
                    WriteDBLog(LogFlag.Bill, br.Message);
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

        /// <summary>
        /// 设置 订单收货地址
        /// znt 2015-04-21
        /// </summary>
        [HttpPost]
        public ActionResult SetOrderAddress(Td_Sale_Order_Head model)
        {
            BaseResult br = new BaseResult();
            if (model.dh.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("提交的数据不完整，请刷新后再试");
                br.Level = ErrorLevel.Warning;
                return Json(br);
            }
            try
            {
                Hashtable param = new Hashtable();
                model.id_edit = GetLoginInfo<long>("id_user");
                model.rq_edit = DateTime.Now;

                param.Add("model", model);
                br = BusinessFactory.SaleOrder.SetOrderAddress(param);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Bill, br.Message);
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

        /// <summary>
        /// 设置 订单订单发票信息
        /// znt 2015-04-21
        /// </summary>
        [HttpPost]
        public ActionResult SetOrderInvoice(Td_Sale_Order_Head model)
        {
            BaseResult br = new BaseResult();
            if (model.dh.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("提交的数据不完整，请刷新后再试");
                br.Level = ErrorLevel.Warning;
                return Json(br);
            }
            try
            {
                Hashtable param = new Hashtable();
                model.id_edit = GetLoginInfo<long>("id_user");
                model.rq_edit = DateTime.Now;

                param.Add("model", model);
                br = BusinessFactory.SaleOrder.SetOrderInvoice(param);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Bill, br.Message);
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

        /// <summary>
        /// 添加付款记录 视图
        /// znt 2015-04-28
        /// </summary>
        [ActionPurview(false)]
        public ActionResult Pay()
        {
            BaseResult br = new BaseResult();
            Td_sale_pay_Query model = new Td_sale_pay_Query();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                string dh = param["dh"].ToString();
                ViewData["je_pay"] = (long)0; // 订单金额
                ViewData["je_payed"] = (long)0; // 已付金额
                ViewData["id_gys"] = (long)0; // 供应商id 选择银行账号标识

                param.Clear();
                param.Add("dh", dh);
                br = BusinessFactory.Order.Get(param);
                if (br.Data != null)
                {
                    Td_Sale_Order_Head head = (Td_Sale_Order_Head)br.Data;
                    model.dh_order = head.dh;
                    ViewData["je_pay"] = head.je_pay;
                    ViewData["je_payed"] = head.je_payed;
                    ViewData["id_gys"] = head.id_gys;
                    model.je = (head.je_pay - head.je_payed) < 0 ? (long)0 : (head.je_pay - head.je_payed); // 尚未付款

                    // 收款账号
                    param.Clear();
                    param.Add("id_gys", head.id_gys);
                    Tb_Gys_Account Bank = BusinessFactory.BankAccount.GetDefault(param).Data as Tb_Gys_Account;
                    if (Bank != null)
                    {
                        model.khr = Bank.khr;
                        model.name_bank = Bank.name_bank;
                        model.account_bank = Bank.account_bank;

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
            return View(model);
        }

        /// <summary>
        /// 采购商 请求记录（购物车或复制订单）
        /// znt 2015-03-24
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult GetBody()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("source", "PcNew", HandleType.DefaultValue);
                p.Add("dh", String.Empty, HandleType.Remove);
                param = param.Trim(p);
                JqGrid model = new JqGrid();

                OrderSourceFlag Source = (OrderSourceFlag)Enum.Parse(typeof(OrderSourceFlag), param["source"].ToString(), true);
                switch (Source)
                {
                    case OrderSourceFlag.PcClone:
                        Hashtable param2 = new Hashtable();
                        param2.Add("dh", param["dh"]);
                        br = BusinessFactory.Order.Get(param2);

                        if (br.Data != null)
                        {
                            Td_Sale_Order_Head_Query orderHead = br.Data as Td_Sale_Order_Head_Query;
                            List<Td_Sale_Order_Body_Query> orderBodyList = BusinessFactory.Order.GetAll(param).Data as List<Td_Sale_Order_Body_Query>;
                            model.rows = new List<JqGrid_Body>();
                            if (orderBodyList != null)
                            {

                                foreach (var item in orderBodyList)
                                {
                                    JqGrid_Body jqBody = new JqGrid_Body();

                                    jqBody.dj = item.dj;
                                    jqBody.dj_base = item.dj_base;
                                    jqBody.dj_old = item.dj;
                                    jqBody.gg = string.Format("{0} {1}【{2}】", item.bm, item.GoodsName, item.formatname.TrimEnd(','));
                                    jqBody.id_cgs = orderHead.id_cgs.Value;
                                    jqBody.id_gys = orderHead.id_gys.Value;
                                    jqBody.id_sku = item.id_sku.Value;
                                    jqBody.id_sp = item.id_sp.Value;
                                    jqBody.sl = item.sl.Value;
                                    jqBody.sl_dh_min = item.sl_dh_min;
                                    jqBody.unit = item.unit;
                                    jqBody.xj = item.sl.Value * item.dj;
                                    model.rows.Add(jqBody);
                                }

                                model.rows.Add(new JqGrid_Body());
                                model.page = 1;
                                model.total = 1;
                                model.records = orderBodyList.Count;
                            }

                        }

                        break;
                    default:
                        model.total = 3;
                        model.records = 3;
                        var none_list = new List<JqGrid_Body>();
                        none_list.Add(new JqGrid_Body());
                        none_list.Add(new JqGrid_Body());
                        none_list.Add(new JqGrid_Body());
                        model.rows = none_list;
                        break;
                }

                return Json(model);
            }
            catch (CySoftException ex)
            {

                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            //return Content(jsonStr);
        }

    }
}
