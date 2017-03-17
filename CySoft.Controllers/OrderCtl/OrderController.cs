//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web.Mvc;
//using CySoft.Frame.Core;
//using System.Collections;
//using CySoft.Controllers.Base;
//using CySoft.Controllers.Filters;
//using System.Web.UI;
//using CySoft.Model.Tb;
//using CySoft.Model.Flags;
//using CySoft.Utility.Mvc.Html;
//using CySoft.Model.Td;
//using CySoft.Model.Other;
//using CySoft.Utility;
//using CySoft.Model.Ts;
//using CySoft.Frame.Utility;
//using System.IO;
//using NPOI;
//using NPOI.HSSF;
//using NPOI.HSSF.UserModel;
//using NPOI.HSSF.Util;
//using NPOI.POIFS;
//using NPOI.Util;
//using NPOI.SS.UserModel;
//using NPOI.SS.Util;
//using ICSharpCode.SharpZipLib.Zip;
//using System.Web;
//using CySoft.Model.Pay;
//using System.Security.Cryptography;
//namespace CySoft.Controllers.OrderCtl
//{
//    [LoginActionFilter]
//    [OutputCache(Location = OutputCacheLocation.None)]
//    public class OrderController : BaseController
//    {
//        /// <summary>
//        ///  采购商 订单列表 
//        ///  znt 2015-03-20
//        ///  mq 2016-05-24 修改：增加已发货订单逾期7天后系统自动收货
//        /// </summary>
//        public ActionResult List()
//        {
//            BaseResult br = new BaseResult();
//            PageNavigate pn = new PageNavigate();
//            int limit = 10;
//            PageList<Td_Sale_Order_Head_Query> list = new PageList<Td_Sale_Order_Head_Query>(limit);
//            Hashtable param = new Hashtable();
//            try
//            {
//                param = base.GetParameters();
//                ParamVessel p = new ParamVessel();
//                var flag_state_List = new List<int>();
//                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
//                p.Add("status", 0, HandleType.DefaultValue);//排序
//                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
//                p.Add("flag_delete", 0, HandleType.DefaultValue);
//                p.Add("flag_show", true, HandleType.Remove);
//                p.Add("flag_state_List", string.Empty, HandleType.Remove);
//                p.Add("dh", string.Empty, HandleType.Remove);
//                p.Add("alias_gys", string.Empty, HandleType.Remove, true);
//                p.Add("start_rq_create", String.Empty, HandleType.Remove);
//                p.Add("end_rq_create", String.Empty, HandleType.Remove);
//                p.Add("flag_out", string.Empty, HandleType.Remove);
//                p.Add("flag_tj", string.Empty, HandleType.Remove);
//                p.Add("flag_fh", string.Empty, HandleType.Remove);
//                param = param.Trim(p);
//                if (param["keyword"] != null)
//                {
//                    ViewData["keyword"] = param["keyword"];
//                }
//                if (param["flag_state_List"] != null)
//                {
//                    ViewData["flag_state_List"] = param["flag_state_List"].ToString();
//                    string[] flag_stateListStr = param["flag_state_List"].ToString().Split(',');
//                    //flag_stateListStr.ToList<string, int>(s =>Convert.ToInt32(s)).ToArray();
//                    int[] flag_stateListInt = Array.ConvertAll<string, int>(flag_stateListStr, s => Convert.ToInt32(s));
//                    param["flag_state_List"] = flag_stateListInt;

//                }
//                if (param["dh"] != null)
//                {
//                    ViewData["dh"] = param["dh"];
//                    ViewData["Search_dh"] = param["dh"];
//                }
//                if (param["flag_out"] != null)
//                {
//                    ViewData["flag_out"] = param["flag_out"];
//                }
//                if (param["flag_fh"] != null)
//                {
//                    ViewData["flag_fh"] = param["flag_fh"];
//                }
//                if (param["flag_tj"] != null)
//                {
//                    ViewData["flag_tj"] = param["flag_tj"];
//                }
//                if (param["start_rq_create"] != null)
//                {
//                    ViewData["start_rq_create"] = param["start_rq_create"];
//                }
//                if (param["end_rq_create"] != null)
//                {
//                    ViewData["end_rq_create"] = param["end_rq_create"];
//                }
//                if (param["dh"] != null)
//                {
//                    ViewData["dh"] = param["dh"];
//                }
//                if (param["alias_cgs"] != null)
//                {
//                    ViewData["alias_cgs"] = param["alias_cgs"];
//                }
//                if (param["flag_show"] != null)
//                {
//                    ViewData["flag_show"] = true;
//                }
//                switch (param["status"].ToString())
//                {
//                    case "1"://待处理订单 -- 已发货 待定 （已发货-》待签收-》已完成）
//                        flag_state_List.AddRange(new List<int> { (int)OrderFlag.Delivered });
//                        param.Add("flag_state_List", flag_state_List);
//                        param.Remove("status");
//                        ViewData["status"] = 1;
//                        break;
//                    case "2"://已完成订单  -- 已收货
//                        flag_state_List.Add((int)OrderFlag.Receipted);
//                        param.Add("flag_state_List", flag_state_List);
//                        param.Remove("status");
//                        ViewData["status"] = 2;
//                        break;
//                    case "3"://已作废订单 -- 已作废
//                        flag_state_List.Add((int)OrderFlag.Invalided);
//                        param.Add("flag_state_List", flag_state_List);
//                        param.Remove("status");
//                        ViewData["status"] = 3;
//                        break;
//                    default:
//                        param.Remove("status");
//                        ViewData["status"] = 0;
//                        break;
//                }


//                int pageIndex = Convert.ToInt32(param["pageIndex"]);
//                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));

//                param.Add("sort", "rq_create");
//                param.Add("dir", "desc");

//                ViewData["pageIndex"] = pageIndex;
//                param.Add("limit", limit);
//                param.Add("start", (pageIndex - 1) * limit);
//                param.Add("flag_up", YesNoFlag.Yes);
//                pn = BusinessFactory.Order.GetPage(param);

//                #region 订单操作日志
//                List<Td_Sale_Order_Head_Query> query = pn.Data as List<Td_Sale_Order_Head_Query>;
//                if (query != null && query.Count > 0)
//                {
//                    foreach (var item in query)
//                    {
//                        param.Clear();
//                        param.Add("dh", item.dh);
//                        param.Add("sort", "rq");
//                        param.Add("dir", "desc");
//                        br = BusinessFactory.OrderLog.GetAll(param);

//                        if (br.Data != null)
//                        {
//                            item.order_Log = (br.Data as List<Ts_Sale_Order_Log_Query>);
//                        }
//                        else
//                        {
//                            item.order_Log = new List<Ts_Sale_Order_Log_Query>();
//                        }

//                    }
//                }
//                pn.Data = query;
//                #endregion


//                list = new PageList<Td_Sale_Order_Head_Query>(pn, pageIndex, limit);

//                //有上级母单的订单列表
//                List<string> Fd_ls = new List<string>();
//                foreach (var item in list)
//                {
//                    var Order_Fd = BusinessFactory.GoodsSkuFd.Query_Sale_Order_Fd(item.dh);
//                    if (Order_Fd != null)
//                    {
//                        Fd_ls.Add(Order_Fd.dh);
//                    }
//                }
//                ViewData["Fd_ls"] = Fd_ls;
//            }
//            catch (CySoftException ex)
//            {

//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            if (Request.IsAjaxRequest())
//            {
//                return PartialView("_ListControl", list);
//            }
//            return View(list);

//        }

//        /// <summary>
//        /// 采购商 新增订单 
//        /// znt 2015-03-17
//        /// </summary>
//        /// <returns>视图</returns>
//        [ActionPurview(false)]
//        public ActionResult Add()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("id_gys", (long)0, HandleType.Remove);
//                p.Add("source", 0, HandleType.DefaultValue);
//                p.Add("dh", String.Empty, HandleType.Remove);
//                param = param.Trim(p);

//                string dh = (param["dh"] ?? "").ToString();
//                long id_gys = Convert.ToInt64(param["id_gys"] ?? 0);
//                long id_cgs = GetLoginInfo<long>("id_buyer");

//                Td_Sale_Order_Head_Query order_head = new Td_Sale_Order_Head_Query();

//                OrderSourceFlag Source = (OrderSourceFlag)Enum.Parse(typeof(OrderSourceFlag), param["source"].ToString());

//                switch (Source)
//                {
//                    //1 PC端购物车
//                    case OrderSourceFlag.PcCart:

//                        ViewData["source"] = param["source"];
//                        ViewData["order_source"] = (int)OrderSourceFlag.PcCart;
//                        ViewData["GridLoadListUrl"] = Url.RouteUrl("Default", new { action = "GridLoadList", controller = "Order", id_gys = id_gys, source = param["source"] });

//                        param.Add("id_cgs", id_cgs);
//                        br = BusinessFactory.Supplier.GetGysCgsRelation(param);
//                        order_head.supplier_name = ((Tb_Gys_Cgs)br.Data).alias_gys;
//                        break;

//                    //2 Pc端 复制
//                    case OrderSourceFlag.PcClone:

//                        ViewData["order_source"] = (int)OrderSourceFlag.PcClone;
//                        ViewData["GridLoadListUrl"] = Url.RouteUrl("Default", new { action = "GridLoadList", controller = "Order", dh = dh.ToString(), source = param["source"] });

//                        param.Clear();
//                        param.Add("dh", dh);
//                        br = BusinessFactory.Order.Get(param);
//                        order_head.supplier_name = ((Td_Sale_Order_Head_Query)br.Data).alias_gys;
//                        order_head.id_gys = ((Td_Sale_Order_Head_Query)br.Data).id_gys;
//                        id_gys = order_head.id_gys.Value;

//                        break;
//                    default:

//                        ViewData["order_source"] = (int)OrderSourceFlag.PcNew;
//                        ViewData["GridLoadListUrl"] = Url.RouteUrl("Default", new { action = "GridLoadList", controller = "Order", id_gys = id_gys, source = param["source"] });
//                        break;
//                }

//                ViewData["id_cgs"] = id_cgs;
//                ViewData["id_gys"] = id_gys;

//                #region 获取 供应商发票类型
//                param.Clear();
//                param.Add("id", id_gys);
//                br = BusinessFactory.Supplier.Get(param);

//                Tb_Gys_Edit tb_gys = br.Data as Tb_Gys_Edit;
//                if (tb_gys != null)
//                {
//                    order_head.tax = tb_gys.tax; // 普通税
//                    order_head.vat = tb_gys.vat; // 增值税
//                }
//                #endregion

//                #region 获取 采购商收货地址
//                br = BusinessFactory.Buyer.RecieverAddress(Convert.ToInt32(GetLoginInfo<long>("id_buyer")));
//                List<Tb_Cgs_Shdz_Query> list_shdz = br.Data as List<Tb_Cgs_Shdz_Query>;
//                if (list_shdz != null && list_shdz.Count > 0)
//                {

//                    Tb_Cgs_Shdz_Query Shdz = list_shdz.Where(m => m.flag_default == YesNoFlag.Yes).FirstOrDefault();
//                    if (Shdz != null)
//                    {
//                        order_head.shr = Shdz.shr;
//                        order_head.phone = Shdz.phone != string.Empty ? Shdz.phone : Shdz.tel;
//                        order_head.id_province = Shdz.id_province;
//                        order_head.id_city = Shdz.id_city;
//                        order_head.id_county = Shdz.id_county;
//                        order_head.address = Shdz.address;
//                        order_head.province_name = Shdz.province_name;
//                        order_head.city_name = Shdz.city_name;
//                        order_head.county_name = Shdz.county_name;
//                    }
//                    else
//                    {
//                        if (list_shdz.Count == 1)
//                        {
//                            Shdz = list_shdz.FirstOrDefault();

//                            order_head.shr = Shdz.shr;
//                            order_head.phone = Shdz.phone != string.Empty ? Shdz.phone : Shdz.tel;
//                            order_head.id_province = Shdz.id_province;
//                            order_head.id_city = Shdz.id_city;
//                            order_head.id_county = Shdz.id_county;
//                            order_head.address = Shdz.address;
//                            order_head.province_name = Shdz.province_name;
//                            order_head.city_name = Shdz.city_name;
//                            order_head.county_name = Shdz.county_name;
//                        }
//                    }
//                }
//                #endregion

//                #region  获取 采购商发票信息
//                param.Clear();
//                param.Add("id", GetLoginInfo<long>("id_buyer"));

//                Tb_Cgs cgs = BusinessFactory.Buyer.Get(param).Data as Tb_Cgs;
//                order_head.title_invoice = cgs.title_invoice;
//                order_head.name_bank = cgs.name_bank;
//                order_head.account_bank = cgs.account_bank;
//                order_head.no_tax = cgs.no_tax;
//                #endregion

//                return View(order_head);
//            }
//            catch (CySoftException)
//            {

//                throw;
//            }
//            catch (Exception)
//            {

//                throw;
//            }

//        }

//        /// <summary>
//        /// 采购商 提交新增
//        /// znt 2015-03-19
//        /// </summary>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult Submit()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("orderData", String.Empty, HandleType.ReturnMsg); // 订单来源
//                p.Add("invoiceFlag", String.Empty, HandleType.ReturnMsg); // 发票类型
//                p.Add("soure", (long)0, HandleType.DefaultValue); // 订单来源 默认PcNew
//                param = param.Trim(p);

//                //订单头信息
//                Td_Sale_Order_Head_Query model = JSON.Deserialize<Td_Sale_Order_Head_Query>(param["orderData"].ToString());

//                if (string.IsNullOrEmpty(model.id_gys.ToString()))
//                {
//                    br.Success = false;
//                    br.Message.Add("校验失败！请重新刷新页面。");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "none";
//                    return Json(br);
//                }

//                if (model.order_body.Count <= 0)
//                {
//                    br.Success = false;
//                    br.Message.Add("商品不能为空！");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "sku";
//                    return Json(br);
//                }
//                if (string.IsNullOrEmpty(model.shr))
//                {
//                    br.Success = false;
//                    br.Message.Add("收货人不能为空！");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "shr";
//                    return Json(br);
//                }
//                if (string.IsNullOrEmpty(model.phone))
//                {
//                    br.Success = false;
//                    br.Message.Add("联系号码不能为空！");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "phone";
//                    return Json(br);
//                }

//                // 单号 赋值 
//                br = BusinessFactory.Utilety.GetNextDH(model, typeof(Td_Sale_Order_Head));

//                if (!br.Success)
//                {
//                    return Json(br);
//                }

//                #region 发票类型

//                string invoiceFlag = param["invoiceFlag"].ToString();
//                int soure = Convert.ToInt32(param["soure"]);
//                switch (invoiceFlag)
//                {
//                    case "2":
//                        model.invoiceFlag = InvoiceFlag.General;
//                        break;
//                    case "3":
//                        model.invoiceFlag = InvoiceFlag.Vat;
//                        break;
//                    default:
//                        model.invoiceFlag = InvoiceFlag.None;
//                        break;
//                }

//                #endregion


//                model.id_cgs = GetLoginInfo<long>("id_buyer");
//                model.id_user_bill = GetLoginInfo<long>("id_user");
//                model.id_user_master = GetLoginInfo<long>("id_user_master");

//                model.id_create = GetLoginInfo<long>("id_user");
//                model.id_edit = GetLoginInfo<long>("id_user");

//                //订单日志 编码
//                long newOrderLogId = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));

//                Hashtable param2 = new Hashtable();
//                param2.Clear();
//                //下单入参
//                param2.Add("newOrderLogId", newOrderLogId);
//                param2.Add("id_user", GetLoginInfo<long>("id_user"));
//                param2.Add("model", model);
//                //备份原入参
//                param2.Add("orderData", param["orderData"]);
//                param2.Add("invoiceFlag", param["invoiceFlag"]);
//                param2.Add("soure", param["soure"]);

//                switch (soure)
//                {        //PC端 购物车
//                    case (int)OrderSourceFlag.PcCart:
//                        param2.Add("OrderSource", OrderSourceFlag.PcCart);
//                        break;
//                    //PC端 复制订单
//                    case (int)OrderSourceFlag.PcClone:
//                        param2.Add("OrderSource", OrderSourceFlag.PcClone);
//                        break;
//                    default:
//                        param2.Add("OrderSource", OrderSourceFlag.PcNew);
//                        break;
//                }

//                //新增普通订单
//                br = BusinessFactory.Order.Add(param2);

//                if (br.Success)
//                {
//                    br.Data = model.dh;
//                    WriteDBLog(LogFlag.Bill, br.Message);
//                }
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//            return Json(br);

//        }

//        /// <summary>
//        /// 采购商 订单详情
//        /// znt 2015-04-01
//        /// cxb 2015-6-17 修改
//        /// </summary>
//        public ActionResult Item(string dh)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = new Hashtable();
//            Hashtable param0 = new Hashtable();
//            Hashtable param1 = new Hashtable();
//            param = GetParameters();

//            try
//            {

//                ParamVessel p = new ParamVessel();
//                p.Add("dh", String.Empty, HandleType.ReturnMsg);
//                p.Add("pageIndex", String.Empty, HandleType.Remove);
//                p.Add("flag_show", String.Empty, HandleType.Remove);
//                p.Add("alias_cgs", String.Empty, HandleType.Remove);
//                p.Add("dh", String.Empty, HandleType.Remove);
//                p.Add("Search_dh", String.Empty, HandleType.Remove);
//                p.Add("flag_state_List", String.Empty, HandleType.Remove);
//                p.Add("flag_out", String.Empty, HandleType.Remove);
//                p.Add("flag_fh", String.Empty, HandleType.Remove);
//                p.Add("flag_tj", String.Empty, HandleType.Remove);
//                p.Add("start_rq_create", String.Empty, HandleType.Remove);
//                p.Add("end_rq_create", String.Empty, HandleType.Remove);
//                p.Add("id_info", String.Empty, HandleType.Remove);
//                param = param.Trim(p);

//                ViewData["dh"] = param["dh"];
//                if (param["pageIndex"] != null)
//                {
//                    ViewData["pageIndex"] = param["pageIndex"];
//                }
//                if (param["flag_out"] != null)
//                {
//                    ViewData["flag_out"] = param["flag_out"];
//                    param.Remove("flag_out");
//                }
//                if (param["start_rq_create"] != null)
//                {
//                    ViewData["start_rq_create"] = param["start_rq_create"];
//                    param.Remove("start_rq_create");
//                }
//                if (param["end_rq_create"] != null)
//                {
//                    ViewData["end_rq_create"] = param["end_rq_create"];
//                    param.Remove("end_rq_create");
//                }
//                if (param["flag_fh"] != null)
//                {
//                    ViewData["flag_fh"] = param["flag_fh"];
//                    param.Remove("flag_fh");
//                }
//                if (param["flag_tj"] != null)
//                {
//                    ViewData["flag_tj"] = param["flag_tj"];
//                    param.Remove("flag_tj");
//                }
//                if (param["flag_state_List"] != null)
//                {
//                    ViewData["flag_state_List"] = param["flag_state_List"];
//                    param.Remove("flag_state_List");
//                }
//                if (param["flag_show"] != null)
//                {
//                    ViewData["flag_show"] = param["flag_show"];
//                    param.Remove("flag_show");
//                }
//                if (param["alias_cgs"] != null)
//                {
//                    ViewData["alias_cgs"] = param["alias_cgs"];
//                    param.Remove("alias_cgs");
//                }
//                if (param["Search_dh"] != null)
//                {
//                    ViewData["Search_dh"] = param["Search_dh"];
//                    param.Remove("Search_dh");
//                }
//                // 获取单头
//                br = BusinessFactory.Order.Get(param);
//                Td_Sale_Order_Head_Query model_head = br.Data as Td_Sale_Order_Head_Query;
//                param.Remove("id_info");

//                if (model_head != null)
//                {
//                    // 获取单体
//                    List<Td_Sale_Order_Body_Query> model_body = BusinessFactory.Order.GetAll(param).Data as List<Td_Sale_Order_Body_Query>;
//                    if (model_body != null)
//                    {
//                        model_head.order_body = model_body;
//                    }

//                    // 获得订单操作日志
//                    param.Add("sort", "rq");
//                    param.Add("dir", "desc");
//                    List<Ts_Sale_Order_Log_Query> model_log = BusinessFactory.OrderLog.GetAll(param).Data as List<Ts_Sale_Order_Log_Query>;

//                    if (model_log != null)
//                    {
//                        model_head.order_Log = model_log;
//                    }
//                }
//                else
//                {
//                    model_head = new Td_Sale_Order_Head_Query();
//                }

//                param1["id"] = model_head.id_gys;
//                br = BusinessFactory.Supplier.Get(param1);
//                Tb_Gys gys = (Tb_Gys)br.Data;

//                param0["val"] = 1;
//                param0["id_user_master"] = gys.id_user_master;
//                br = BusinessFactory.Setting.GetAll(param0);
//                ViewData["process"] = br.Data;

//                param0.Clear();
//                param0["dh_order"] = param["dh"];
//                param0["flag_state"] = OrderFlag.Delivered;
//                br = BusinessFactory.Order.GetCount(param0);

//                ViewData["outordercount"] = br.Data;
//                ViewData["dh"] = model_head.dh;

//                var Order_Fd = BusinessFactory.GoodsSkuFd.Query_Sale_Order_Fd(dh);
//                ViewData["FD"] = "0";
//                if (Order_Fd != null) ViewData["FD"] = "1";



//                return View(model_head);
//            }

//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        /// <summary>
//        /// 收货去人越过发货出库 cxb 2015-6-17
//        /// </summary>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public ActionResult ConfirmCrossingFhck()
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = base.GetParameters();
//            ParamVessel p = new ParamVessel();
//            p.Add("dh", string.Empty, HandleType.ReturnMsg);//出库单单号
//            param = param.Trim(p);
//            br = BusinessFactory.ShippingRecord.ConfirmSh(param);
//            return Json(br);
//        }

//        /// <summary>
//        /// 订单状态 作废操作
//        /// znt 2015-04-02
//        /// </summary>
//        [HttpPost]
//        public ActionResult Invalid()
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = GetParameters();
//            string dh = param["dh"].ToString();
//            if (string.IsNullOrEmpty(dh))
//            {
//                br.Success = false;
//                br.Message.Add(" 数据验证失败！请重新刷新页面。");
//                br.Data = "dh";
//                return Json(br);
//            }
//            if (string.IsNullOrEmpty(param["flag_state"].ToString()))
//            {
//                br.Success = false;
//                br.Message.Add(" 数据验证失败！请重新刷新页面。");
//                br.Data = "flag_state";
//                return Json(br);
//            }
//            try
//            {
//                param.Add("id_user", GetLoginInfo<long>("id_user"));
//                param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
//                br = BusinessFactory.Order.Invalid(param);
//                if (br.Success)
//                {
//                    WriteDBLog(LogFlag.Bill, br.Message);
//                }
//                return Json(br);
//            }
//            catch (CySoftException ex)
//            {

//                throw ex;
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }

//        /// <summary>
//        /// 商品sku备注 添加或修改  采购商
//        /// znt 2015-4-7
//        /// </summary>
//        [HttpPost]
//        [ActionPurview("Update")]
//        public ActionResult SpSkuRemark()
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = GetParameters();
//            string dh = param["dh"].ToString();
//            if (string.IsNullOrEmpty(param["dh"].ToString()))
//            {
//                br.Success = false;
//                br.Message.Add(" 数据验证失败！请重新刷新页面。");
//                br.Data = "dh";
//                return Json(br);
//            }
//            try
//            {
//                br = BusinessFactory.Order.SpSkuRemark(param);
//                if (br.Success)
//                {
//                    WriteDBLog(LogFlag.Bill, br.Message);
//                }
//                return Json(br);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }

//        /// <summary>
//        /// 删除订单 
//        /// znt 2015-04-20
//        /// </summary>
//        /// flag_delete 状态为0
//        public ActionResult Delete()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("dh", String.Empty, HandleType.ReturnMsg);
//                param = param.Trim(p);

//                param.Add("new_id_edit", GetLoginInfo<long>("id_user"));
//                param.Add("new_rq_edit", DateTime.Now);
//                br = BusinessFactory.Order.Stop(param);
//                if (br.Success)
//                {
//                    WriteDBLog(LogFlag.Bill, br.Message);
//                }
//            }
//            catch (CySoftException ex)
//            {

//                throw ex;
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//            return Json(br);
//        }

//        #region 导出订单
//        /// <summary>
//        /// 导出订单
//        /// </summary>
//        /// <param name="dh"></param>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public FileResult Export(string dh)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = new Hashtable();
//            try
//            {
//                string[] dhList = dh.Split(',');
//                Guid guid = new Guid();
//                guid = Guid.NewGuid();
//                string path = ApplicationInfo.TempPath + "/" + guid;
//                string FileName = "";
//                if (dhList.Length > 1)
//                {
//                    if (!Directory.Exists(path))
//                    {
//                        Directory.CreateDirectory(path);
//                    }
//                }
//                for (int index = 0; index < dhList.Length; index++)
//                {
//                    param.Clear();
//                    param.Add("dh", dhList[index]);
//                    ParamVessel p = new ParamVessel();
//                    p.Add("dh", String.Empty, HandleType.ReturnMsg);
//                    param = param.Trim(p);
//                    // 获取单头
//                    br = BusinessFactory.Order.Get(param);
//                    Td_Sale_Order_Head_Query model_head = br.Data as Td_Sale_Order_Head_Query;

//                    if (model_head != null)
//                    {
//                        // 获取单体
//                        List<Td_Sale_Order_Body_Query> model_body = BusinessFactory.Order.GetAll(param).Data as List<Td_Sale_Order_Body_Query>;
//                        if (model_body != null)
//                        {
//                            model_head.order_body = model_body;
//                        }
//                        // 获得订单操作日志
//                        param.Add("sort", "rq");
//                        param.Add("dir", "desc");

//                        List<Ts_Sale_Order_Log_Query> model_log = BusinessFactory.OrderLog.GetAll(param).Data as List<Ts_Sale_Order_Log_Query>;
//                        if (model_log != null)
//                        {
//                            model_head.order_Log = model_log;
//                        }
//                    }
//                    else
//                    {
//                        model_head = new Td_Sale_Order_Head_Query();
//                    }

//                    #region 导出订单

//                    //创建Excel文件的对象
//                    HSSFWorkbook book = new HSSFWorkbook();
//                    //添加一个sheet
//                    ISheet sheet1 = book.CreateSheet(dhList[index]);

//                    //设置列宽
//                    sheet1.SetColumnWidth(0, 14 * 256);
//                    sheet1.SetColumnWidth(1, 25 * 256);
//                    sheet1.SetColumnWidth(2, 25 * 256);
//                    sheet1.SetColumnWidth(3, 32 * 256);
//                    sheet1.SetColumnWidth(4, 14 * 256);
//                    sheet1.SetColumnWidth(5, 14 * 256);
//                    sheet1.SetColumnWidth(6, 14 * 256);
//                    sheet1.SetColumnWidth(7, 14 * 256);
//                    sheet1.SetColumnWidth(8, 14 * 256);
//                    sheet1.SetColumnWidth(9, 14 * 256);

//                    //初始化样式
//                    ICellStyle mStyle = book.CreateCellStyle();
//                    mStyle.Alignment = HorizontalAlignment.Center;
//                    mStyle.VerticalAlignment = VerticalAlignment.Center;
//                    mStyle.BorderBottom = BorderStyle.Thin;
//                    mStyle.BorderTop = BorderStyle.Thin;
//                    mStyle.BorderLeft = BorderStyle.Thin;
//                    mStyle.BorderRight = BorderStyle.Thin;
//                    IFont mfont = book.CreateFont();
//                    mfont.FontHeight = 10 * 20;
//                    mStyle.SetFont(mfont);
//                    sheet1.SetDefaultColumnStyle(0, mStyle);
//                    sheet1.SetDefaultColumnStyle(1, mStyle);
//                    sheet1.SetDefaultColumnStyle(2, mStyle);
//                    sheet1.SetDefaultColumnStyle(3, mStyle);
//                    sheet1.SetDefaultColumnStyle(4, mStyle);
//                    sheet1.SetDefaultColumnStyle(5, mStyle);
//                    sheet1.SetDefaultColumnStyle(6, mStyle);
//                    sheet1.SetDefaultColumnStyle(7, mStyle);
//                    sheet1.SetDefaultColumnStyle(8, mStyle);
//                    sheet1.SetDefaultColumnStyle(9, mStyle);

//                    ICellStyle lstyle = book.CreateCellStyle();
//                    lstyle.Alignment = HorizontalAlignment.Left;
//                    lstyle.VerticalAlignment = VerticalAlignment.Center;
//                    lstyle.BorderBottom = BorderStyle.Thin;
//                    lstyle.BorderTop = BorderStyle.Thin;
//                    lstyle.BorderLeft = BorderStyle.Thin;
//                    lstyle.BorderRight = BorderStyle.Thin;

//                    ICellStyle rstyle = book.CreateCellStyle();
//                    rstyle.Alignment = HorizontalAlignment.Right;
//                    rstyle.VerticalAlignment = VerticalAlignment.Center;
//                    rstyle.BorderBottom = BorderStyle.Thin;
//                    rstyle.BorderTop = BorderStyle.Thin;
//                    rstyle.BorderLeft = BorderStyle.Thin;
//                    rstyle.BorderRight = BorderStyle.Thin;

//                    //第一行
//                    IRow row = sheet1.CreateRow(0);
//                    ICell cell = row.CreateCell(0);
//                    cell.SetCellValue("订货单");
//                    ICellStyle style = book.CreateCellStyle();
//                    style.Alignment = HorizontalAlignment.Center;
//                    style.VerticalAlignment = VerticalAlignment.Center;
//                    IFont font = book.CreateFont();
//                    font.FontHeight = 20 * 20;
//                    style.SetFont(font);
//                    /*style.FillForegroundColor = HSSFColor.BlueGrey.Index;
//                    style.FillPattern = FillPattern.NoFill;
//                    style.FillForegroundColor = HSSFColor.BlueGrey.Index;*/
//                    cell.CellStyle = style;
//                    NPOIHelper.MergeCell(sheet1, 0, 0, 0, 9);
//                    sheet1.GetRow(0).Height = 40 * 20;

//                    //第二行
//                    IRow row1 = sheet1.CreateRow(1);
//                    row1.CreateCell(0).SetCellValue("订单编号");
//                    row1.CreateCell(1).SetCellValue(model_head.dh);
//                    row1.CreateCell(2).SetCellValue("下单日期");
//                    row1.CreateCell(3).SetCellValue(Convert.ToDateTime(model_head.rq_create).ToString("yyyy-MM-dd HH:mm"));
//                    row1.CreateCell(4).SetCellValue("预交货日期");
//                    var jh = string.Empty;
//                    if (Convert.ToDateTime(model_head.rq_jh).ToString() == "" || Convert.ToDateTime(model_head.rq_jh) < Convert.ToDateTime("1901-01-01"))
//                    {
//                        jh = "无交货日期";
//                    }
//                    else
//                        jh = Convert.ToDateTime(model_head.rq_jh).ToString("yyyy-MM-dd HH:mm");
//                    row1.CreateCell(6).SetCellValue(jh);
//                    NPOIHelper.MergeCell(sheet1, 1, 1, 4, 5);
//                    NPOIHelper.MergeCell(sheet1, 1, 1, 6, 9);
//                    sheet1.GetRow(1).Height = 28 * 20;

//                    //第三行
//                    IRow row2 = sheet1.CreateRow(2);
//                    row2.CreateCell(0).SetCellValue("公司名称");
//                    row2.CreateCell(1).SetCellValue(model_head.CustomeName);//
//                    row2.GetCell(1).CellStyle = lstyle;
//                    row2.CreateCell(2).SetCellValue("客户名");
//                    row2.CreateCell(3).SetCellValue(model_head.alias_cgs);
//                    row2.CreateCell(4).SetCellValue("客户编码");
//                    row2.CreateCell(5).SetCellValue(model_head.bm_gys_Interface);
//                    row2.CreateCell(7).SetCellValue("联系人");
//                    row2.CreateCell(8).SetCellValue(model_head.shr);
//                    //NPOIHelper.MergeCell(sheet1, 2, 2, 1, 3);
//                    //NPOIHelper.MergeCell(sheet1, 2, 2, 4, 5);
//                    NPOIHelper.MergeCell(sheet1, 2, 2, 5, 6);
//                    NPOIHelper.MergeCell(sheet1, 2, 2, 8, 9);
//                    sheet1.GetRow(2).Height = 28 * 20;

//                    //第四行
//                    IRow row3 = sheet1.CreateRow(3);
//                    row3.CreateCell(0).SetCellValue("收货地址");
//                    row3.CreateCell(1).SetCellValue(model_head.province_name + model_head.city_name + model_head.county_name + model_head.address);
//                    row3.GetCell(1).CellStyle = lstyle;
//                    row3.CreateCell(4).SetCellValue("联系电话");
//                    row3.CreateCell(6).SetCellValue(model_head.phone);
//                    NPOIHelper.MergeCell(sheet1, 3, 3, 1, 3);
//                    NPOIHelper.MergeCell(sheet1, 3, 3, 4, 5);
//                    NPOIHelper.MergeCell(sheet1, 3, 3, 6, 9);
//                    sheet1.GetRow(3).Height = 28 * 20;

//                    //第五行
//                    IRow row4 = sheet1.CreateRow(4);
//                    ICell cell4 = row4.CreateCell(0);
//                    cell4.SetCellValue("商品明细");
//                    cell4.CellStyle = style;
//                    NPOIHelper.MergeCell(sheet1, 4, 4, 0, 9);
//                    sheet1.GetRow(4).Height = 40 * 20;

//                    //第六行
//                    IRow row5 = sheet1.CreateRow(5);
//                    row5.CreateCell(0).SetCellValue("序号");
//                    row5.CreateCell(1).SetCellValue("商品编码");
//                    row5.CreateCell(2).SetCellValue("商品条码");
//                    row5.CreateCell(3).SetCellValue("商品名称");
//                    row5.CreateCell(4).SetCellValue("商品规格");
//                    row5.CreateCell(5).SetCellValue("单价(元)");
//                    row5.CreateCell(6).SetCellValue("数量");
//                    row5.CreateCell(7).SetCellValue("计量单位");
//                    row5.CreateCell(8).SetCellValue("金额小计(元)");
//                    row5.CreateCell(9).SetCellValue("备注");
//                    sheet1.GetRow(5).Height = 28 * 20;

//                    //将数据逐步写入sheet1各个行
//                    var item = model_head.order_body;
//                    var k = item.Count;
//                    decimal total_sl = 0;
//                    for (int i = 0; i < item.Count; i++)
//                    {
//                        IRow rowtemp = sheet1.CreateRow(i + 6);
//                        rowtemp.CreateCell(0).SetCellValue(i + 1);
//                        rowtemp.CreateCell(1).SetCellValue(item[i].bm);
//                        rowtemp.CreateCell(2).SetCellValue(item[i].barcode);
//                        rowtemp.CreateCell(3).SetCellValue(item[i].GoodsName);
//                        rowtemp.GetCell(3).CellStyle = lstyle;
//                        rowtemp.CreateCell(4).SetCellValue(item[i].formatname);
//                        rowtemp.GetCell(4).CellStyle = lstyle;
//                        rowtemp.CreateCell(5).SetCellValue(Decimal.Round((decimal)item[i].dj_hs, 2).ToString());//Decimal.Round((decimal)item[i].dj_hs,2
//                        rowtemp.CreateCell(6).SetCellValue(Decimal.Round((decimal)item[i].sl, 0).ToString());
//                        total_sl += (decimal)item[i].sl;
//                        rowtemp.GetCell(5).CellStyle = rstyle;
//                        rowtemp.GetCell(6).CellStyle = rstyle;
//                        rowtemp.CreateCell(7).SetCellValue(item[i].unit);
//                        rowtemp.CreateCell(8).SetCellValue(Decimal.Round((decimal)item[i].je_pay, 2).ToString());
//                        rowtemp.GetCell(8).CellStyle = rstyle;
//                        rowtemp.CreateCell(9).SetCellValue(item[i].remark);
//                        sheet1.GetRow(i + 6).Height = 28 * 20;
//                    }
//                    if (item.Count < 4)
//                    {
//                        for (int x = 0; x < 4 - item.Count; x++)
//                        {
//                            IRow rowtemp = sheet1.CreateRow(item.Count + 6 + x);
//                            rowtemp.CreateCell(0).SetCellValue("");
//                            rowtemp.CreateCell(1).SetCellValue("");
//                            rowtemp.CreateCell(2).SetCellValue("");
//                            rowtemp.CreateCell(3).SetCellValue("");
//                            rowtemp.CreateCell(4).SetCellValue("");
//                            rowtemp.CreateCell(5).SetCellValue("");
//                            rowtemp.CreateCell(6).SetCellValue("");
//                            rowtemp.CreateCell(7).SetCellValue("");
//                            rowtemp.CreateCell(8).SetCellValue("");
//                            rowtemp.CreateCell(9).SetCellValue("");
//                            sheet1.GetRow(item.Count + 6 + x).Height = 28 * 20;
//                        }
//                        k = 4;
//                    }
//                    ICellStyle style2 = book.CreateCellStyle();
//                    style2.Alignment = HorizontalAlignment.Left;
//                    style2.VerticalAlignment = VerticalAlignment.Center;
//                    style2.BorderBottom = BorderStyle.Thin;
//                    style2.BorderTop = BorderStyle.Thin;
//                    style2.BorderLeft = BorderStyle.Thin;
//                    style2.BorderRight = BorderStyle.Thin;
//                    /* IRow row6 = sheet1.CreateRow(k + 6);
//                     ICell cell6 = row6.CreateCell(0);
//                     cell6.SetCellValue("订单运费（元）：");
//                     cell6.CellStyle = style2;
//                     row6.CreateCell(7).SetCellValue("0.00");
//                     row6.GetCell(7).CellStyle = rstyle;
//                     row6.CreateCell(8).SetCellValue("");
//                     NPOIHelper.MergeCell(sheet1, k + 6, k + 6, 0, 6);
//                     sheet1.GetRow(k + 6).Height = 28 * 20;*/

//                    IRow row7 = sheet1.CreateRow(k + 6);
//                    row7.CreateCell(0).SetCellValue("合计：");
//                    row7.CreateCell(1).SetCellValue("");
//                    row7.CreateCell(2).SetCellValue("");
//                    row7.CreateCell(3).SetCellValue("");
//                    row7.CreateCell(4).SetCellValue("");
//                    row7.CreateCell(5).SetCellValue("");
//                    row7.CreateCell(6).SetCellValue(Decimal.Round(total_sl, 0).ToString());
//                    row7.GetCell(6).CellStyle = rstyle;
//                    row7.CreateCell(7).SetCellValue("");
//                    row7.CreateCell(8).SetCellValue(Decimal.Round((decimal)model_head.je_pay, 2).ToString());
//                    row7.CreateCell(9).SetCellValue("");
//                    row7.GetCell(8).CellStyle = rstyle;
//                    row7.CreateCell(9).SetCellValue("");
//                    sheet1.GetRow(k + 6).Height = 28 * 20;

//                    //清除边框
//                    ICellStyle endStyle = book.CreateCellStyle();
//                    endStyle.BorderBottom = BorderStyle.None;
//                    endStyle.BorderTop = BorderStyle.None;
//                    endStyle.BorderLeft = BorderStyle.None;
//                    endStyle.BorderRight = BorderStyle.None;

//                    sheet1.SetDefaultColumnStyle(0, endStyle);
//                    sheet1.SetDefaultColumnStyle(1, endStyle);
//                    sheet1.SetDefaultColumnStyle(2, endStyle);
//                    sheet1.SetDefaultColumnStyle(3, endStyle);
//                    sheet1.SetDefaultColumnStyle(4, endStyle);
//                    sheet1.SetDefaultColumnStyle(5, endStyle);
//                    sheet1.SetDefaultColumnStyle(6, endStyle);
//                    sheet1.SetDefaultColumnStyle(7, endStyle);
//                    sheet1.SetDefaultColumnStyle(8, endStyle);
//                    sheet1.SetDefaultColumnStyle(9, endStyle);
//                    FileName = "订货单-" + dhList[index] + ".xls";
//                    if (dhList.Length == 1)
//                    {
//                System.IO.MemoryStream ms = new System.IO.MemoryStream();
//                book.Write(ms);
//                ms.Seek(0, SeekOrigin.Begin);
//                        FileName = "订货单-" + dhList[index] + ".xls";
//                        return File(ms, "application/vnd.ms-excel", FileName);
//                    }
//                    using (FileStream fs = System.IO.File.OpenWrite(path + "/" + FileName))
//                {
//                        book.Write(fs);
//                }

//                }
//                FileName = "批量导出订货单-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".zip";
//                ZipHelper.ZipFileDirectory(path, path + "/" + FileName, "xls");
//                return File(path + "/" + FileName, "application/x-zip-compressed", FileName);
//                    #endregion

//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }
//        #endregion
//        /// <summary>
//        /// 导出明细
//        /// </summary>
//        /// <param name="dh"></param>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public FileResult ExportInfo(string dh)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = new Hashtable();
//            try
//            {
//                System.IO.MemoryStream ms = new System.IO.MemoryStream();
//                string[] dhList = dh.Split(',');
//                string Name = "";
//                string cgs_name = "";
//                string cgs_bm = "";
//                Guid guid = new Guid();
//                guid = Guid.NewGuid();
//                string path = ApplicationInfo.TempPath +"/"+ guid;
//                if (dhList.Length > 1)
//                {
//                    if (!Directory.Exists(path))
//                    {
//                        Directory.CreateDirectory(path);
//                    }
//                }
               
//                for (int index = 0; index < dhList.Length; index++)
//                {
//                    param.Clear();
//                    param.Add("dh", dhList[index]);
//                    ParamVessel p = new ParamVessel();
//                    p.Add("dh", String.Empty, HandleType.ReturnMsg);
//                    param = param.Trim(p);
//                    // 获取单头
//                    br = BusinessFactory.Order.Get(param);
//                    Td_Sale_Order_Head_Query model_head = br.Data as Td_Sale_Order_Head_Query;

//                    if (model_head != null)
//                    {
//                        cgs_name = model_head.alias_cgs == "" ? "" : "-"+model_head.alias_cgs;
//                        cgs_bm = model_head.bm_gys_Interface == "" ? "-" : "-" + model_head.bm_gys_Interface;
//                        // 获取单体
//                        List<Td_Sale_Order_Body_Query> model_body = BusinessFactory.Order.GetAll(param).Data as List<Td_Sale_Order_Body_Query>;
//                        if (model_body != null)
//                        {
//                            model_head.order_body = model_body;
//                        }
//                        // 获得订单操作日志
//                        param.Add("sort", "rq");
//                        param.Add("dir", "desc");
//                    }
//                    else
//                    {
//                        model_head = new Td_Sale_Order_Head_Query();
//                    }

//                    #region 导出订单

//                    var item = model_head.order_body;
//                    var k = item.Count;
//                    //创建Excel文件的对象
//                    HSSFWorkbook book = new HSSFWorkbook();
//                    ISheet sheet1 = book.CreateSheet("订单明细");

//                    //设置列宽
//                    sheet1.SetColumnWidth(0, 25 * 256);
//                    sheet1.SetColumnWidth(1, 25 * 256);
//                    //初始化样式
//                    ICellStyle mStyle = book.CreateCellStyle();
//                    mStyle.Alignment = HorizontalAlignment.Center;
//                    mStyle.VerticalAlignment = VerticalAlignment.Center;
//                    mStyle.BorderBottom = BorderStyle.Thin;
//                    mStyle.BorderTop = BorderStyle.Thin;
//                    mStyle.BorderLeft = BorderStyle.Thin;
//                    mStyle.BorderRight = BorderStyle.Thin;
//                    IFont mfont = book.CreateFont();
//                    mfont.FontHeight = 10 * 20;
//                    mStyle.SetFont(mfont);
//                    sheet1.SetDefaultColumnStyle(0, mStyle);
//                    sheet1.SetDefaultColumnStyle(1, mStyle);
//                    for (int i = 0; i < item.Count; i++)
//                    {
//                        IRow rowtemp = sheet1.CreateRow(i);
//                        rowtemp.CreateCell(0).SetCellValue(item[i].barcode);
//                        rowtemp.CreateCell(1).SetCellValue(Decimal.Round((decimal)item[i].sl, 0).ToString());
//                        sheet1.GetRow(i).Height = 28 * 20;
//                    }
//                //清除边框
//                ICellStyle endStyle = book.CreateCellStyle();
//                endStyle.BorderBottom = BorderStyle.None;
//                endStyle.BorderTop = BorderStyle.None;
//                endStyle.BorderLeft = BorderStyle.None;
//                endStyle.BorderRight = BorderStyle.None;

//                sheet1.SetDefaultColumnStyle(0, endStyle);
//                sheet1.SetDefaultColumnStyle(1, endStyle);
//                    Name = "订货单明细" + cgs_bm + cgs_name + "-" + dhList[index] + ".xls";
//                    if (dhList.Length == 1)
//                    {
//                book.Write(ms);
//                ms.Seek(0, SeekOrigin.Begin);
//                        return File(ms, "application/vnd.ms-excel", Name);
//                    }
                    
//                    using (FileStream fs = System.IO.File.OpenWrite(path + "/" + Name))
//                {
//                        book.Write(fs);
//                    }
//                }
//                Name = "批量导出订货单明细-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".zip";
//                ZipHelper.ZipFileDirectory(path, path + "/" + Name, "xls");
//                return File(path + "/" + Name, "application/x-zip-compressed", Name);
//                #endregion

//            }
//            catch (Exception)
//            {

//                throw;
//            }

//        }

//        #region 打印订单
//        /// <summary>
//        /// 打印订单
//        /// </summary>
//        /// <param name="dh"></param>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public ActionResult Print(string dh)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = new Hashtable();
//            param.Add("dh", dh);

//            try
//            {

//                ParamVessel p = new ParamVessel();
//                p.Add("dh", String.Empty, HandleType.ReturnMsg);
//                param = param.Trim(p);
//                // 获取单头
//                br = BusinessFactory.Order.Get(param);
//                Td_Sale_Order_Head_Query model_head = br.Data as Td_Sale_Order_Head_Query;

//                if (model_head != null)
//                {
//                    // 获取单体
//                    List<Td_Sale_Order_Body_Query> model_body = BusinessFactory.Order.GetAll(param).Data as List<Td_Sale_Order_Body_Query>;
//                    if (model_body != null)
//                    {
//                        model_head.order_body = model_body;
//                    }
//                    // 获得订单操作日志
//                    param.Add("sort", "rq");
//                    param.Add("dir", "desc");

//                    List<Ts_Sale_Order_Log_Query> model_log = BusinessFactory.OrderLog.GetAll(param).Data as List<Ts_Sale_Order_Log_Query>;
//                    if (model_log != null)
//                    {
//                        model_head.order_Log = model_log;
//                    }
//                }
//                else
//                {
//                    model_head = new Td_Sale_Order_Head_Query();
//                }

//                return View(model_head);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }
//        #endregion

//        /// <summary>
//        /// 设置 订单收货地址
//        /// znt 2015-04-21
//        /// </summary>
//        [HttpPost]
//        [ActionPurview("Update")]
//        public ActionResult SetOrderAddress(Td_Sale_Order_Head model)
//        {
//            BaseResult br = new BaseResult();
//            if (model.dh.IsEmpty())
//            {
//                br.Success = false;
//                br.Message.Add("提交的数据不完整，请刷新后再试");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//            }
//            try
//            {
//                Hashtable param = new Hashtable();
//                model.id_edit = GetLoginInfo<long>("id_user");
//                model.rq_edit = DateTime.Now;

//                param.Add("model", model);
//                br = BusinessFactory.Order.SetOrderAddress(param);
//                if (br.Success)
//                {
//                    WriteDBLog(LogFlag.Bill, br.Message);
//                }

//            }
//            catch (CySoftException ex)
//            {

//                throw ex;
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        /// 设置 订单订单发票信息
//        /// znt 2015-04-21
//        /// </summary>
//        [HttpPost]
//        [ActionPurview("Update")]
//        public ActionResult SetOrderInvoice(Td_Sale_Order_Head model)
//        {
//            BaseResult br = new BaseResult();
//            if (model.dh.IsEmpty())
//            {
//                br.Success = false;
//                br.Message.Add("提交的数据不完整，请刷新后再试");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//            }
//            try
//            {
//                Hashtable param = new Hashtable();
//                model.id_edit = GetLoginInfo<long>("id_user");
//                model.rq_edit = DateTime.Now;

//                param.Add("model", model);
//                br = BusinessFactory.Order.SetOrderInvoice(param);
//                if (br.Success)
//                {
//                    WriteDBLog(LogFlag.Bill, br.Message);
//                }

//            }
//            catch (CySoftException ex)
//            {

//                throw ex;
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        /// 付款
//        /// znt 2015-04-27
//        /// </summary>
//        [ActionPurview(false)]
//        public ActionResult Pay()
//        {
//            BaseResult br = new BaseResult();
//            Td_sale_pay_Query model = new Td_sale_pay_Query();
//            try
//            {
//                Hashtable param = GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("dh", String.Empty, HandleType.ReturnMsg);
//                param = param.Trim(p);

//                string dh = param["dh"].ToString();
//                ViewData["je_pay"] = (long)0; // 订单金额
//                ViewData["je_payed"] = (long)0; // 已付金额
//                ViewData["id_gys"] = (long)0; // 供应商id 选择银行账号标识

//                param.Clear();
//                param.Add("dh", dh);
//                br = BusinessFactory.Order.Get(param);
//                if (br.Data != null)
//                {
//                    Td_Sale_Order_Head_Query head = (Td_Sale_Order_Head_Query)br.Data;
//                    model.dh_order = head.dh;
//                    ViewData["je_pay"] = head.je_pay;
//                    ViewData["je_payed"] = head.je_payed;
//                    ViewData["id_gys"] = head.id_gys;
//                    model.je = (head.je_pay - head.je_payed) < 0 ? (long)0 : (head.je_pay - head.je_payed); // 尚未付款
//                    model.id_user_master_cgs = head.id_user_master_cgs;
//                    model.id_user_master_gys = head.id_user_master_gys;
//                    // 收款账号
//                    param.Clear();
//                    param.Add("id_gys", head.id_gys);
//                    Tb_Gys_Account Bank = BusinessFactory.BankAccount.GetDefault(param).Data as Tb_Gys_Account;
//                    if (Bank != null)
//                    {
//                        model.khr = Bank.khr;
//                        model.name_bank = Bank.name_bank;
//                        model.account_bank = Bank.account_bank;
//                    }
//                    #region 判断供应商是否开通在线支付
//                    param.Clear();
//                    bool yeePay = false;
//                    bool wxPay = false;
//                    param["id_user_master"] = head.id_user_master_gys;
//                    //br = BusinessFactory.Pay.CheckPayAccount(param, "yeePay");
//                    //if (br.Data != null)
//                    //{

//                    //}
//                    br = BusinessFactory.Pay.CheckPayAccount(param, "wxPay");
//                    if (br.Data != null)
//                    {
//                        WeChatAccount wx = (WeChatAccount)br.Data;
//                        if (wx.flag_state == 1)
//                        {
//                            wxPay = true;
//                        }
//                    }
//                    string md5Gys = MD5Encrypt.Encrypt(head.id_user_master_gys.ToString());
//                    ViewBag.dh = dh;
//                    ViewBag.md5Gys = md5Gys;
//                    ViewBag.yeePay = yeePay;
//                    ViewBag.wxPay = wxPay; 
//                    #endregion
//                }

//            }
//            catch (CySoftException ex)
//            {

//                throw ex;
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//            return View(model);
//        }

//        /// <summary>
//        /// 采购商 请求记录（购物车或复制订单）
//        /// znt 2015-03-24
//        /// </summary>
//        [ActionPurview(false)]
//        public ActionResult GridLoadList()
//        {

//            BaseResult br = new BaseResult();
//            Hashtable param = GetParameters();
//            var id_gys = param["id_gys"];
//            var dh = param["dh"];
//            JqGrid model = new JqGrid();

//            OrderSourceFlag Source = (OrderSourceFlag)Enum.Parse(typeof(OrderSourceFlag), param["source"].ToString());


//            try
//            {

//                switch (Source)
//                {
//                    case OrderSourceFlag.PcCart:
//                        param.Clear();
//                        param.Add("id_gys", id_gys);
//                        param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
//                        br = BusinessFactory.GoodsCart.GetAll(param);
//                        if (br.Data != null)
//                        {
//                            List<Td_Sale_Cart_Query> list = br.Data as List<Td_Sale_Cart_Query>;
//                            model.rows = new List<JqGrid_Body>();
//                            foreach (var item in list)
//                            {
//                                JqGrid_Body jqBody = new JqGrid_Body();

//                                jqBody.dj = item.dj;
//                                jqBody.dj_base = item.dj_base;
//                                jqBody.dj_old = item.dj_old;
//                                jqBody.gg = string.Format("{0} {1}{2}", item.bm_Interface, item.mc, string.IsNullOrWhiteSpace(item.gg) ? "" : "【" + item.gg.TrimEnd(',') + "】");
//                                jqBody.id_cgs = item.id_cgs.Value;
//                                jqBody.id_gys = item.id_gys.Value;
//                                jqBody.id_sku = item.id_sku.Value;
//                                jqBody.id_sp = item.id_sp.Value;
//                                jqBody.sl = item.sl.Value;
//                                jqBody.sl_dh_min = item.sl_dh_min;
//                                jqBody.unit = item.unit;
//                                jqBody.xj = item.sl.Value * item.dj;
//                                model.rows.Add(jqBody);
//                            }
//                            model.rows.Add(new JqGrid_Body());
//                            model.page = 1;
//                            model.total = 1;
//                            model.records = list.Count;
//                        }

//                        break;
//                    case OrderSourceFlag.PcClone:
//                        param.Clear();
//                        param.Add("dh", dh);
//                        br = BusinessFactory.Order.Get(param);

//                        if (br.Data != null)
//                        {
//                            Td_Sale_Order_Head_Query orderHead = br.Data as Td_Sale_Order_Head_Query;
//                            id_gys = orderHead.id_gys;

//                            List<Td_Sale_Order_Body_Query> orderBodyList = BusinessFactory.Order.GetAll(param).Data as List<Td_Sale_Order_Body_Query>;
//                            model.rows = new List<JqGrid_Body>();
//                            if (orderBodyList != null)
//                            {

//                                foreach (var item in orderBodyList)
//                                {
//                                    JqGrid_Body jqBody = new JqGrid_Body();

//                                    jqBody.dj = item.dj;
//                                    jqBody.dj_base = item.dj_base;
//                                    jqBody.dj_old = item.dj;
//                                    jqBody.gg = string.Format("{0} {1}{2}", item.bm, item.GoodsName, string.IsNullOrWhiteSpace(item.formatname) ? "" : "【" + item.formatname.TrimEnd(',') + "】");
//                                    jqBody.id_cgs = orderHead.id_cgs.Value;
//                                    jqBody.id_gys = orderHead.id_gys.Value;
//                                    jqBody.id_sku = item.id_sku.Value;
//                                    jqBody.id_sp = item.id_sp.Value;
//                                    jqBody.sl = item.sl.Value;
//                                    jqBody.sl_dh_min = item.sl_dh_min;
//                                    jqBody.unit = item.unit;
//                                    jqBody.xj = item.sl.Value * item.dj;
//                                    model.rows.Add(jqBody);
//                                }

//                                model.rows.Add(new JqGrid_Body());
//                                model.page = 1;
//                                model.total = 1;
//                                model.records = orderBodyList.Count;
//                            }

//                        }

//                        break;
//                    default:
//                        model.total = 3;
//                        model.records = 3;
//                        var none_list = new List<JqGrid_Body>();
//                        none_list.Add(new JqGrid_Body());
//                        none_list.Add(new JqGrid_Body());
//                        none_list.Add(new JqGrid_Body());
//                        model.rows = none_list;
//                        break;
//                }

//                return Json(model);
//            }
//            catch (CySoftException ex)
//            {

//                throw ex;
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }

//            //return Content(jsonStr);
//        }
//    }
//}
