using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Td;
using CySoft.Model.Ts;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Flags;
using System.Linq;
using CySoft.Model.Other;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using CySoft.Controllers.GoodsCtl;
using NPOI.SS.Util;

#region 销售出库单
#endregion

namespace CySoft.Controllers.SaleCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class SaleOutController : BaseController
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
                p.Add("from", String.Empty, HandleType.Remove);//来源
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("beginDate_ck", String.Empty, HandleType.Remove);
                p.Add("endDate_ck", String.Empty, HandleType.Remove);
                p.Add("beginDate_fh", String.Empty, HandleType.Remove);
                p.Add("endDate_fh", String.Empty, HandleType.Remove);
                param = param.Trim(p);
                if (param["beginDate_ck"] != null)
                {
                    ViewData["beginDate_ck"] = param["beginDate_ck"];
                }
                if (param["endDate_ck"] != null)
                {
                    ViewData["endDate_ck"] = param["endDate_ck"];
                }
                if (param["beginDate_fh"] != null)
                {
                    ViewData["beginDate_fh"] = param["beginDate_fh"];
                }
                if (param["endDate_fh"] != null)
                {
                    ViewData["endDate_fh"] = param["endDate_fh"];
                }
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                ViewData["keyword"] = GetParameter("keyword");
                ViewData["orderby"] = param["orderby"];
                ViewData["pageIndex"] = pageIndex;
                ViewData["limit"] = limit;

                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
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
        /// 出库
        /// cxb
        /// 2015-3-20 
        /// </summary>
        [HttpPost]
        public ActionResult Out(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("head", null, HandleType.ReturnMsg);
                p.Add("body", null, HandleType.ReturnMsg);
                p.Add("ShoppingInfo", null, HandleType.Remove);
                param = param.Trim(p);
                Td_Sale_Out_Head head = JSON.ConvertToType<Td_Sale_Out_Head>(param["head"]);
                head.id_gys = GetLoginInfo<long>("id_supplier");
                List<Td_Sale_Out_Body> body = JSON.ConvertToType<List<Td_Sale_Out_Body>>(param["body"]);
                ShoppingInfo shoppinginfo = JSON.ConvertToType<ShoppingInfo>(param["ShoppingInfo"]);
                Ts_Sale_Order_Log ts = new Ts_Sale_Order_Log();//日志
                if (shoppinginfo != null)
                {
                    if (shoppinginfo.company_logistics == "")
                    {
                        shoppinginfo.company_logistics = "暂无";
                    }
                    if (shoppinginfo.no_logistics == "")
                    {
                        shoppinginfo.no_logistics = "暂无";
                    }
                    if (shoppinginfo.rq_fh == "")
                    {
                        shoppinginfo.rq_fh = DateTime.Now.ToString();
                    }
                }
                // 单号 赋值 
                if (head.id_cgs <= 0)
                {
                    br.Success = false;
                    br.Message.Add("未知采购商，请刷新后再试！");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (head.dh_order.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("订单号丢失，请刷新后再试！");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (head.id_gys <= 0)
                {
                    br.Success = false;
                    br.Message.Add("未知供应商，请刷新后再试！");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (body.Count < 1)
                {
                    br.Success = false;
                    br.Message.Add("<h5>参数错误</h5>");
                    br.Message.Add("没有可保存的商品！");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                int xh = 1;
                foreach (var item in body)
                {
                    if (item != null)
                    {
                        if (item.id_sku < 1)
                        {
                            br.Success = false;
                            br.Message.Add("<h5>参数错误</h5>");
                            br.Message.Add(String.Format("[代码:303]，第{0}行数据中商品id有误！", xh));
                            br.Level = ErrorLevel.Warning;
                            return Json(br);
                        }
                        if (item.sl <= 0m)
                        {
                            br.Success = false;
                            br.Message.Add("<h5>参数错误</h5>");
                            br.Message.Add(String.Format("[代码:303]，第{0}行数据中商品数量有误！", xh));
                            br.Level = ErrorLevel.Warning;
                            return Json(br);
                        }
                        //if (item.zhl <= 0m)
                        //{
                        //    br.Success = false;
                        //    br.Message.Add("<h5>参数错误</h5>");
                        //    br.Message.Add(String.Format("[代码:303]，第{0}行数据中包装数有误！", xh));
                        //    br.Level = ErrorLevel.Warning;
                        //    return Json(br);
                        //}
                        xh++;
                    }
                }
                br = BusinessFactory.Utilety.GetNextDH(head, typeof(Td_Sale_Out_Head));

                if (!br.Success)
                {
                    return Json(br);
                }
                ts.id = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                ts.id_user = GetLoginInfo<long>("id_user");
                ts.id_user_master = GetLoginInfo<long>("id_user_master");
                ts.flag_type = 4;
                head.id_create = GetLoginInfo<long>("id_user");
                head.id_edit = GetLoginInfo<long>("id_user");

                param.Clear();
                param.Add("head", head);
                param.Add("body", body);
                param.Add("Ts", ts);
                param.Add("ShoppingInfo", shoppinginfo);
                param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));

                br = BusinessFactory.ShippingRecord.Add(param);

                //#region 跳入分单批量插入出库单

                //var rs = BusinessFactory.GoodsSkuFd.Query_Sale_Order_Head(head.dh_order);
                //if (rs.flag_fd != 1)
                //{
                //    BusinessFactory.GoodsSkuFd.SaleOut_Out_Fd(head.dh_order, head.dh);
                //}

                //#endregion

                return Json(br);
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
        /// <summary>
        /// 批量出库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult OutList(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                if (!string.IsNullOrEmpty(obj) && obj != "[]")
                {
                    Hashtable param = new Hashtable();
                    List<Hashtable> paramList = JSON.Deserialize<List<Hashtable>>(obj);
                    List<Td_Sale_Out_Body> body = new List<Td_Sale_Out_Body>();
                    List<Td_Sale_Out_Body> bodyQuery = new List<Td_Sale_Out_Body>();
                    Ts_Sale_Order_Log ts = new Ts_Sale_Order_Log();//日志
                    ShoppingInfo shoppinginfo = new ShoppingInfo();
                    if (shoppinginfo != null)
                    {
                        if (shoppinginfo.company_logistics == "")
                        {
                            shoppinginfo.company_logistics = "暂无";
                        }
                        if (shoppinginfo.no_logistics == "")
                        {
                            shoppinginfo.no_logistics = "暂无";
                        }
                        if (shoppinginfo.rq_fh == "")
                        {
                            shoppinginfo.rq_fh = DateTime.Now.ToString();
                        }
                    }
                    long gys = GetLoginInfo<long>("id_supplier");
                    long id_user = GetLoginInfo<long>("id_user");
                    long id_user_master = GetLoginInfo<long>("id_user_master");
                    foreach (Hashtable item in paramList)
                    {
                        Td_Sale_Out_Head head = new Td_Sale_Out_Head();
                        head.dh_order = item["dh_order"].ToString();
                        head.id_cgs = Convert.ToInt32(item["id_cgs"]);
                        head.id_gys = gys;
                        param.Clear();
                        param["id_gys"] = gys;
                        param["sort"] = "sl_ck";
                        param["dir"] = "desc";
                        param.Add("dh", head.dh_order);
                        br = BusinessFactory.Order.GetAll(param);
                        if (br.Data != null)
                        {
                            body.Clear();
                            body = JSON.Deserialize<List<Td_Sale_Out_Body>>(JSON.Serialize(br.Data));
                            bodyQuery = JSON.Deserialize<List<Td_Sale_Out_Body>>(JSON.Serialize(br.Data));
                            if (head.id_cgs > 0 && !head.dh_order.IsEmpty() && head.id_gys > 0 || body.Count > 0)
                            {
                                int index = 0;
                                foreach (Td_Sale_Out_Body bq in bodyQuery)
                                {
                                    bq.dh_order = bq.dh;
                                    bq.dh = "";
                                    bq.xh_order = Convert.ToInt16(index++);
                                    bq.sl = bq.sl - bq.sl_ck;
                                }
                                //for (int i = 0; i < bodyQuery.Count; i++)
                                //{
                                //    //if (bodyQuery[i].sl - bodyQuery[i].sl_ck <= 0)
                                //    //{
                                //    //    bodyQuery.RemoveAt(i);
                                //    //}
                                //    //else
                                //    //{
                                //    //    bodyQuery[i].dh_order = bodyQuery[i].dh;
                                //    //    bodyQuery[i].dh = "";
                                //    //    bodyQuery[i].xh_order = Convert.ToInt16(i++);
                                //    //    bodyQuery[i].sl = bodyQuery[i].sl - bodyQuery[i].sl_ck;
                                //    //    index++;
                                //    //}
                                //    bodyQuery[i].dh_order = bodyQuery[i].dh;
                                //    bodyQuery[i].dh = "";
                                //    bodyQuery[i].xh_order = Convert.ToInt16(i++);
                                //    bodyQuery[i].sl = bodyQuery[i].sl - bodyQuery[i].sl_ck;
                                //}
                                br = BusinessFactory.Utilety.GetNextDH(head, typeof(Td_Sale_Out_Head));
                                if (br.Success)
                                {
                                    ts.id = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                                    ts.id_user = id_user;
                                    ts.id_user_master = id_user_master;
                                    ts.flag_type = 4;
                                    head.id_create = id_user;
                                    head.id_edit = id_user;
                                    param.Clear();
                                    param.Add("head", head);
                                    param.Add("body", bodyQuery);
                                    param.Add("ShoppingInfo", shoppinginfo);
                                    param.Add("Ts", ts);
                                    param.Add("id_user_master", id_user_master);

                                    br = BusinessFactory.ShippingRecord.Add(param);

                                }
                            }
                        }
                    }
                }
                else
                {
                    br.Success = false;
                    br.Message.Add("无可出库的订单！");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                return Json(br);
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
        /// <summary>
        /// 发货UI
        /// lxt
        /// 2015-04-24
        /// </summary>
        [HttpPost]
        [ActionPurview("Deliver")]
        public ActionResult DeliverView()
        {
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                p.Add("orderdh", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                ViewData["dh"] = param["dh"];
                ViewData["orderdh"] = param["orderdh"];
                ViewData["dh_order"] = param["orderdh"];
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return PartialView("_DeliverControl");
        }

        /// <summary>
        /// 发货
        /// cxb
        /// 2015-3-25
        /// 2015-04-16 lxt 改
        /// </summary>
        [HttpPost]
        public ActionResult Deliver(Td_Sale_Out_Head model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            DateTime dt = new DateTime(1900, 1, 1);

            Ts_Sale_Order_Log ts = new Ts_Sale_Order_Log();//日志
            try
            {
                ts.id = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                ts.id_user = GetLoginInfo<long>("id_user");
                ts.id_user_master = GetLoginInfo<long>("id_user_master");
                ts.flag_type = 5;
                ts.dh = model.dh_order;
                ts.content = "订单已通过发货确认";
                if (model.rq_fh == dt)
                {
                    model.rq_fh = DateTime.Now;
                }
                model.id_fh = GetLoginInfo<long>("id_user");
                model.id_gys = GetLoginInfo<long>("id_supplier");
                param["id_user_master"] = GetLoginInfo<long>("id_user_master");
                param["model"] = model;
                br = BusinessFactory.ShippingRecord.Update(param);
                if (br.Success)
                {
                    BusinessFactory.OrderLog.Add(ts);
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
        /// 批量发货
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult DeliverList(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                if (!string.IsNullOrEmpty(obj) && obj != "[]")
                {

                    Hashtable param = new Hashtable();
                    List<Td_Sale_Out_Head_Query> ck = new List<Td_Sale_Out_Head_Query>();
                    List<Hashtable> paramList = JSON.Deserialize<List<Hashtable>>(obj);
                    DateTime dt = new DateTime(1900, 1, 1);
                    Ts_Sale_Order_Log ts = new Ts_Sale_Order_Log();//日志
                    Td_Sale_Out_Head model = new Td_Sale_Out_Head();
                    long gys = GetLoginInfo<long>("id_supplier");
                    long id_user = GetLoginInfo<long>("id_user");
                    long id_user_master = GetLoginInfo<long>("id_user_master");
                    ts.id_user = id_user;
                    ts.id_user_master = id_user_master;
                    ts.flag_type = 5;
                    ts.content = "订单已通过发货确认";
                    foreach (Hashtable item in paramList)
                    {
                        //获取出库单头
                        param.Clear();
                        param["dh_order"] = item["dh_order"].ToString();
                        param["sort"] = "flag_state asc, rq_fh desc  , rq_create";
                        param["dir"] = "desc";
                        param["flag_state"] = OrderFlag.WaitDelivery;
                        br = BusinessFactory.ShippingRecord.Get(param);
                        ck = JSON.Deserialize<List<Td_Sale_Out_Head_Query>>(JSON.Serialize(br.Data));
                        foreach (Td_Sale_Out_Head_Query it in ck)
                        {
                            model.dh = it.dh;
                            model.flag_state = it.flag_state;
                            ts.id = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                            model.rq_fh = DateTime.Now;
                            model.id_fh = id_user;
                            model.id_gys = gys;
                            model.dh_order = item["dh_order"].ToString();
                            param.Clear();
                            param["id_user_master"] = id_user_master;
                            param["model"] = model;
                            br = BusinessFactory.ShippingRecord.Update(param);
                            if (br.Success)
                            {
                                BusinessFactory.OrderLog.Add(ts);
                            }

                        }
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
        /// 作废UI
        /// lxt
        /// 2015-04-24
        /// </summary>
        [HttpPost]
        [ActionPurview("Invalid")]
        public ActionResult InvalidView()
        {
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                p.Add("orderdh", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                ViewData["dh"] = param["dh"];
                ViewData["orderdh"] = param["orderdh"];
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return PartialView("_InvalidControl");
        }

        /// <summary>
        /// 作废
        /// cxb
        /// 2015-3-26 
        /// </summary>
        [HttpPost]
        public ActionResult Invalid(Td_Sale_Out_Head model)
        {

            BaseResult br = new BaseResult();
            Ts_Sale_Order_Log ts = new Ts_Sale_Order_Log();//日志
            try
            {
                ts.id = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                ts.id_user = GetLoginInfo<long>("id_user");
                ts.id_user_master = GetLoginInfo<long>("id_user_master");
                ts.flag_type = 6;
                ts.dh = model.dh_order;
                ts.content = "出库单【" + model.dh + "】作废";
                model.id_edit = GetLoginInfo<long>("id_user_master");
                br = BusinessFactory.ShippingRecord.Save(model);
                if (br.Success)
                {
                    BusinessFactory.OrderLog.Add(ts);
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
        /// 删除
        /// cxb
        /// 2015-3-26
        /// </summary>
        [HttpPost]
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                param["id_edit"] = GetLoginInfo<long>("id_user_master");
                br = BusinessFactory.ShippingRecord.Delete(param);
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
            Hashtable param0 = new Hashtable();
            try
            {

                param.Add("dh", id);
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                br = BusinessFactory.Order.Get(param);
                ViewData["OrderHead"] = br.Data;
                //获取订单单体
                param["id_gys"] = GetLoginInfo<long>("id_supplier");
                param["sort"] = "sl_ck";
                param["dir"] = "desc";
                br = BusinessFactory.Order.GetAll(param);
                ViewData["OrderbodyList"] = br.Data;
                //获取出库单头
                paramli["dh_order"] = id;
                paramli["sort"] = "flag_state asc, rq_fh desc  , rq_create";
                paramli["dir"] = "desc";
                paramli["not_flag_state"] = OrderFlag.Deleted;
                br = BusinessFactory.ShippingRecord.Get(paramli);
                ViewData["OrderOutHead"] = br.Data;
                List<Td_Sale_Out_Head_Query> list2 = (List<Td_Sale_Out_Head_Query>)br.Data;
                //获取业务流程
                param0["id_user_master"] = GetLoginInfo<long>("id_user_master");
                br = BusinessFactory.Setting.GetAll(param0);
                ViewData["process"] = br.Data;
                //获取出库单单体
                paramli["sort"] = "dh";
                paramli["idgys"] = GetLoginInfo<long>("id_supplier");
                br = BusinessFactory.ShippingRecord.GetAll(paramli);
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
        /// mq 2016-05-24 修改：增加批量导出
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dh"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        public FileResult Export(string id, string dh)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            Hashtable paramli = new Hashtable();
            Hashtable param0 = new Hashtable();
            try
            {

                string[] idList = id.Split(',');
                string[] dhList = dh.Split(',');
                //创建Excel文件的对象
                HSSFWorkbook book = new HSSFWorkbook();
                for (int i = 0; i < dhList.Length; i++)
                {
                    param.Clear();
                    param.Add("dh", idList[i]);
                    ParamVessel p = new ParamVessel();
                    p.Add("dh", String.Empty, HandleType.ReturnMsg);
                    param = param.Trim(p);
                    br = BusinessFactory.Order.Get(param);
                    //ViewData["OrderHead"] = br.Data;
                    //获取订单单体
                    param["sort"] = "sl_ck";
                    param["dir"] = "desc";
                    br = BusinessFactory.Order.GetAll(param);
                    //ViewData["OrderbodyList"] = br.Data;
                    //获取出库单头
                    paramli["dh_order"] = idList[i];
                    paramli["sort"] = "flag_state asc, rq_fh desc  , rq_create";
                    paramli["dir"] = "desc";
                    paramli["not_flag_state"] = OrderFlag.Deleted;
                    br = BusinessFactory.ShippingRecord.Get(paramli);
                    var OrderOutHead = br.Data as IList<CySoft.Model.Td.Td_Sale_Out_Head_Query>;
                    var itemhead = OrderOutHead.Where(m => m.flag_delete == 0 && m.dh == dhList[i]).FirstOrDefault();
                    List<Td_Sale_Out_Head_Query> list2 = (List<Td_Sale_Out_Head_Query>)br.Data;
                    //获取业务流程
                    param0["val"] = 1;
                    param0["id_user_master"] = GetLoginInfo<long>("id_user_master");
                    br = BusinessFactory.Setting.GetAll(param0);
                    //ViewData["process"] = br.Data;
                    //获取出库单单体
                    paramli["sort"] = "dh";
                    br = BusinessFactory.ShippingRecord.GetAll(paramli);
                    var Orderbody = br.Data as IList<CySoft.Model.Td.Td_Sale_Out_Body_Query>;
                    var item = Orderbody.Where(m => m.dh == dhList[i]).ToList();

                    #region 导出订单

                    //添加一个sheet
                    ISheet sheet1 = book.CreateSheet(dhList[i]);

                    //设置列宽
                    sheet1.SetColumnWidth(0, 14 * 256);
                    sheet1.SetColumnWidth(1, 25 * 256);
                    sheet1.SetColumnWidth(2, 25 * 256);
                    sheet1.SetColumnWidth(3, 32 * 256);
                    sheet1.SetColumnWidth(4, 14 * 256);
                    sheet1.SetColumnWidth(5, 14 * 256);
                    sheet1.SetColumnWidth(6, 14 * 256);
                    sheet1.SetColumnWidth(7, 14 * 256);

                    //初始化样式
                    ICellStyle mStyle = book.CreateCellStyle();
                    mStyle.Alignment = HorizontalAlignment.Center;
                    mStyle.VerticalAlignment = VerticalAlignment.Center;
                    mStyle.BorderBottom = BorderStyle.Thin;
                    mStyle.BorderTop = BorderStyle.Thin;
                    mStyle.BorderLeft = BorderStyle.Thin;
                    mStyle.BorderRight = BorderStyle.Thin;
                    IFont mfont = book.CreateFont();
                    mfont.FontHeight = 10 * 20;
                    mStyle.SetFont(mfont);
                    sheet1.SetDefaultColumnStyle(0, mStyle);
                    sheet1.SetDefaultColumnStyle(1, mStyle);
                    sheet1.SetDefaultColumnStyle(2, mStyle);
                    sheet1.SetDefaultColumnStyle(3, mStyle);
                    sheet1.SetDefaultColumnStyle(4, mStyle);
                    sheet1.SetDefaultColumnStyle(5, mStyle);
                    sheet1.SetDefaultColumnStyle(6, mStyle);
                    sheet1.SetDefaultColumnStyle(7, mStyle);

                    //第一行
                    IRow row = sheet1.CreateRow(0);
                    ICell cell = row.CreateCell(0);
                    cell.SetCellValue("出库/发货记录");
                    ICellStyle style = book.CreateCellStyle();
                    style.Alignment = HorizontalAlignment.Center;
                    style.VerticalAlignment = VerticalAlignment.Center;
                    IFont font = book.CreateFont();
                    font.FontHeight = 20 * 20;
                    style.SetFont(font);

                    cell.CellStyle = style;
                    sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 7));
                    sheet1.GetRow(0).Height = 40 * 20;

                    //第二行
                    IRow row1 = sheet1.CreateRow(1);
                    row1.CreateCell(0).SetCellValue("出库编号");
                    row1.CreateCell(1).SetCellValue(dh);
                    row1.CreateCell(2).SetCellValue("出库日期");
                    row1.CreateCell(3).SetCellValue(Convert.ToDateTime(itemhead.rq_create).ToString("yyyy-MM-dd HH:mm"));
                    row1.CreateCell(4).SetCellValue("发货日期");
                    var rq_fh_logistics = Convert.ToDateTime(itemhead.rq_fh_logistics);//;
                    row1.CreateCell(6).SetCellValue(rq_fh_logistics < Convert.ToDateTime("1900-01-02") ? "无发货日期" : rq_fh_logistics.ToString("yyyy-MM-dd HH:mm"));
                    sheet1.AddMergedRegion(new CellRangeAddress(1, 1, 4, 5));
                    sheet1.AddMergedRegion(new CellRangeAddress(1, 1, 6, 7));
                    sheet1.GetRow(1).Height = 28 * 20;

                    //第三行
                    IRow row2 = sheet1.CreateRow(2);
                    row2.CreateCell(0).SetCellValue("物流公司");
                    row2.CreateCell(1).SetCellValue(itemhead.company_logistics);//
                    row2.CreateCell(2).SetCellValue("状态");
                    var flag_state = string.Empty;

                    switch (itemhead.flag_state)
                    {
                        case OrderFlag.Invalided:
                            flag_state = "已作废";
                            break;
                        case OrderFlag.Submitted:
                            flag_state = "已提交";
                            break;
                        case OrderFlag.OrderCheck:
                            flag_state = "订单审核";
                            break;
                        case OrderFlag.FinanceCheck:
                            flag_state = "财务审核";
                            break;
                        case OrderFlag.WaitOutputCheck:
                            flag_state = "待出库审核";
                            break;
                        case OrderFlag.Outbounded:
                            flag_state = "已出库";
                            break;
                        case OrderFlag.WaitDelivery:
                            flag_state = "待发货";
                            break;
                        case OrderFlag.Delivered:
                            flag_state = "已发货";
                            break;
                        case OrderFlag.Receipted:
                            flag_state = "已收货";
                            break;
                        case OrderFlag.Deleted:
                            flag_state = "已删除";
                            break;

                    }

                    /* if (itemhead.flag_state == CySoft.Model.Flags.OrderFlag.Delivered)
                     {
                         flag_state = "未签收";
                     }
                     else
                         flag_state = "已签收";*/
                    row2.CreateCell(3).SetCellValue(flag_state);
                    row2.CreateCell(4).SetCellValue("物流单号");
                    row2.CreateCell(6).SetCellValue(itemhead.no_logistics);
                    sheet1.AddMergedRegion(new CellRangeAddress(2, 2, 4, 5));
                    sheet1.AddMergedRegion(new CellRangeAddress(2, 2, 6, 7));
                    sheet1.GetRow(2).Height = 28 * 20;

                    //第四行
                    IRow row3 = sheet1.CreateRow(3);
                    ICell cell3 = row3.CreateCell(0);
                    cell3.SetCellValue("商品明细");
                    cell3.CellStyle = style;
                    sheet1.AddMergedRegion(new CellRangeAddress(3, 3, 0, 7));
                    sheet1.GetRow(3).Height = 40 * 20;

                    //第五行
                    IRow row4 = sheet1.CreateRow(4);
                    row4.CreateCell(0).SetCellValue("序号");
                    row4.CreateCell(1).SetCellValue("商品编码");
                    row4.CreateCell(2).SetCellValue("商品名称");
                    row4.CreateCell(3).SetCellValue("商品规格");
                    row4.CreateCell(4).SetCellValue("数量");
                    row4.CreateCell(5).SetCellValue("计量单位");
                    row4.CreateCell(6).SetCellValue("备注");
                    sheet1.AddMergedRegion(new CellRangeAddress(4, 4, 6, 7));
                    sheet1.GetRow(4).Height = 28 * 20;

                    //将数据逐步写入sheet1各个行

                    for (int k = 0; k < item.Count; k++)
                    {
                        IRow rowtemp = sheet1.CreateRow(k + 5);
                        rowtemp.CreateCell(0).SetCellValue(k + 1);
                        rowtemp.CreateCell(1).SetCellValue(item[k].GoodsBm);
                        rowtemp.CreateCell(2).SetCellValue(item[k].GoodsName);
                        rowtemp.CreateCell(3).SetCellValue(item[k].formatname);
                        rowtemp.CreateCell(4).SetCellValue(Decimal.Round((decimal)item[k].sl, 2).ToString());
                        rowtemp.CreateCell(5).SetCellValue(item[k].unit);
                        rowtemp.CreateCell(6).SetCellValue(item[k].remark);
                        sheet1.AddMergedRegion(new CellRangeAddress(k + 5, k + 5, 6, 7));
                        sheet1.GetRow(k + 5).Height = 28 * 20;
                    }
                }


                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                string title = "出库/发货记录-" + dh;
                if (dhList.Length > 1)
                {
                    title = "批量导出出库/发货记录" + DateTime.Now.ToString("yyyyMMddHHmmss");
                }
                return File(ms, "application/vnd.ms-excel", title + ".xls");
                #endregion
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
        [ActionPurview(false)]
        public ActionResult Print(string id, string dh)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            Hashtable paramli = new Hashtable();
            Hashtable param0 = new Hashtable();
            try
            {
                param.Add("dh", id);
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                br = BusinessFactory.Order.Get(param);
                //ViewData["OrderHead"] = br.Data;
                //获取订单单体
                param["sort"] = "sl_ck";
                param["dir"] = "desc";
                br = BusinessFactory.Order.GetAll(param);
                //ViewData["OrderbodyList"] = br.Data;
                //获取出库单头
                paramli["dh_order"] = id;
                paramli["sort"] = "flag_state asc, rq_fh desc  , rq_create";
                paramli["dir"] = "desc";
                paramli["not_flag_state"] = OrderFlag.Deleted;
                br = BusinessFactory.ShippingRecord.Get(paramli);
                var OrderOutHead = br.Data as IList<CySoft.Model.Td.Td_Sale_Out_Head_Query>;
                var itemhead = OrderOutHead.Where(m => m.flag_delete == 0 && m.dh == dh).FirstOrDefault();
                ViewData["itemhead"] = itemhead;
                List<Td_Sale_Out_Head_Query> list2 = (List<Td_Sale_Out_Head_Query>)br.Data;
                //获取业务流程
                param0["val"] = 1;
                param0["id_user_master"] = GetLoginInfo<long>("id_user_master");
                br = BusinessFactory.Setting.GetAll(param0);
                //ViewData["process"] = br.Data;
                //获取出库单单体
                paramli["sort"] = "dh";
                br = BusinessFactory.ShippingRecord.GetAll(paramli);
                var Orderbody = br.Data as IList<CySoft.Model.Td.Td_Sale_Out_Body_Query>;
                var item = Orderbody.Where(m => m.dh == dh).ToList();

                return View(item);
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
