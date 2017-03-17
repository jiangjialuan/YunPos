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
//using CySoft.Utility;
//using System.Web;
//using CySoft.Model.Other;
//using CySoft.Utility.Pay;
//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;
//using NPOI.SS.Util;
//using System.IO;
//using System.Dynamic;
//using CySoft.Model.Pay;
//using CySoft.Utility.Pay.YeePay;
//using CySoft.Utility.Pay.WxPay;
//using System.Drawing;
//using ThoughtWorks.QRCode.Codec;
//using System.Drawing.Imaging;
//using CySoft.Frame.Common;
//using CySoft.Model.Enums;
//namespace CySoft.Controllers.FundsCtl
//{
//    [LoginActionFilter]
//    [OutputCache(Location = OutputCacheLocation.None)]
//    public class FundsController : BaseController
//    {

//        /// <summary>
//        /// 收款记录 cxb 2015-3-10
//        /// </summary>
//        /// <returns></returns>
//        [ValidateInput(false)]
//        public ActionResult List()
//        {

//            //BaseResult br = new BaseResult();
//            //PageNavigate pn = new PageNavigate();
//            //int limit = 10;
//            //PageList<Td_sale_pay_Query> list = new PageList<Td_sale_pay_Query>(limit);
//            //try
//            //{
//            //    Hashtable param = base.GetParameters();
//            //    ParamVessel p = new ParamVessel();
//            //    p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
//            //    p.Add("start_rq_create", String.Empty, HandleType.Remove);
//            //    p.Add("end_rq_create", String.Empty, HandleType.Remove);
//            //    p.Add("searchValuebycgs", String.Empty, HandleType.Remove, true);
//            //    param = param.Trim(p);

//            //    int pageIndex = Convert.ToInt32(param["pageIndex"]);
//            //    ViewData["pageIndex"] = pageIndex;
//            //    param.Add("sort", "rq_create");
//            //    param.Add("dir", "desc");
//            //    param.Add("limit", limit);
//            //    param.Add("start", (pageIndex - 1) * limit);
//            //    param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
//            //    param["not_flag_state"] = 1;
//            //    param["not_flag_delete"] = 1;
//            //    pn = BusinessFactory.Funds.GetPage(param);
//            //    list = new PageList<Td_sale_pay_Query>(pn, pageIndex, limit);
//            //    param.Clear();
//            //    param["id_user"] = GetLoginInfo<long>("id_user_master");
//            //    param.Add("id_roleList", new long[] { 1, 3, 4 });
//            //    var role = BusinessFactory.UserRole.GetAll(param);
//            //    IList<Tb_User_Role> user_role = (IList<Tb_User_Role>)role.Data;
//            //    if (user_role != null && user_role.Count > 0)
//            //    {
//            //        var isSupplier = user_role.Where(d => d.id_role == ((int)Enums.TbRoleFixedRoleId.BusinessMan).ToString()).ToList().Count > 0;
//            //        var isBuyer = user_role.Where(d => d.id_role == "4").ToList().Count > 0;
//            //        if (isSupplier && isBuyer)
//            //        {
//            //            //平台商
//            //            ViewData["RoleType"] = "0";
//            //        }
//            //        else if (isSupplier)
//            //        {
//            //            //供应商
//            //            ViewData["RoleType"] = "0";
//            //        }
//            //        else if (isBuyer)
//            //        {
//            //            //采购商
//            //            ViewData["RoleType"] = "1";
//            //        }
//            //    }
//            //    else
//            //    {
//            //        ViewData["RoleType"] = "0";
//            //    }
//            //}
//            //catch (CySoftException ex)
//            //{
//            //    throw ex;
//            //}
//            //catch (Exception ex)
//            //{
//            //    throw ex;
//            //}
//            //if (Request.IsAjaxRequest())
//            //{
//            //    return PartialView("_ListControl", list);
//            //}
//            //return View(list);
//            return View();
//        }

//        /// <summary>
//        /// 付款记录 cxb 2015-4-22 19:31:18
//        /// </summary>
//        /// <returns></returns>
//        public ActionResult PayList()
//        {
//            BaseResult br = new BaseResult();
//            PageNavigate pn = new PageNavigate();
//            int limit = 10;
//            PageList<Td_sale_pay_Query> list = new PageList<Td_sale_pay_Query>(limit);
//            try
//            {
//                Hashtable param = base.GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
//                p.Add("start_rq_create", String.Empty, HandleType.Remove);
//                p.Add("end_rq_create", String.Empty, HandleType.Remove);
//                p.Add("searchValuebygys", String.Empty, HandleType.Remove, true);
//                param = param.Trim(p);
//                int pageIndex = Convert.ToInt32(param["pageIndex"]);
//                ViewData["pageIndex"] = pageIndex;
//                param.Add("sort", "rq_create");
//                param.Add("dir", "desc");
//                param.Add("limit", limit);
//                param.Add("start", (pageIndex - 1) * limit);
//                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
//                param["not_flag_delete"] = 1;
//                pn = BusinessFactory.Funds.GetPage(param);
//                list = new PageList<Td_sale_pay_Query>(pn, pageIndex, limit);
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
//                return PartialView("_PayListControl", list);
//            }
//            return View(list);
//        }

//        /// <summary>
//        /// 删除收款记录 cxb 2015-4-22
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public ActionResult DeleteRecord(Td_sale_pay_Query model)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = new Hashtable();
//            try
//            {
//                param["dh"] = model.dh;
//                param["new_flag_delete"] = 1;
//                param["new_id_delete"] = GetLoginInfo<long>("id_user");
//                param["new_rq_delete"] = DateTime.Now;
//                br = BusinessFactory.Funds.Update(param);
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
//        /// 收款统计 cxb 2015-3-13 
//        /// </summary>
//        /// <returns></returns>
//        public ActionResult Statistics(Td_Sale_Order_Head_Query model)
//        {
//            BaseResult br = new BaseResult();
//            PageNavigate pn = new PageNavigate();
//            int limit = 10;
//            decimal? CopeSum = 0;
//            decimal? PaidSum = 0;
//            decimal? WaidSum = 0;
//            PageList<Td_Sale_Order_Head_Query> list = new PageList<Td_Sale_Order_Head_Query>(limit);
//            var flag_state_List = new List<int>();
//            try
//            {
//                Hashtable param = base.GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
//                p.Add("start_rq_create", String.Empty, HandleType.Remove);
//                p.Add("end_rq_create", String.Empty, HandleType.Remove);
//                p.Add("searchValuebygys", String.Empty, HandleType.Remove, true);
//                param = param.Trim(p);
//                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
//                param.Add("not_flag_state", 0);
//                pn = BusinessFactory.Order.GetPage(param);
//                foreach (Td_Sale_Order_Head_Query item in (IList<Td_Sale_Order_Head_Query>)pn.Data)
//                {
//                    CopeSum = CopeSum + item.je_pay;
//                    PaidSum = PaidSum + item.je_payed;
//                    WaidSum = CopeSum - PaidSum;
//                }

//                ViewData["Cope"] = CopeSum;
//                ViewData["Paid"] = PaidSum;
//                ViewData["Wait"] = WaidSum;
//                int pageIndex = Convert.ToInt32(param["pageIndex"]);
//                ViewData["pageIndex"] = pageIndex;

//                param.Add("sort", "rq_create");
//                param.Add("dir", "desc");
//                param.Add("limit", limit);
//                param.Add("start", (pageIndex - 1) * limit);

//                pn = BusinessFactory.Order.GetPage(param);
//                list = new PageList<Td_Sale_Order_Head_Query>(pn, pageIndex, limit);
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
//                return PartialView("_StatisticsControl", list);
//            }
//            return View(list);
//        }
//        [ActionPurview(false)]
//        public ActionResult Export(string type = "", string dh = "")
//        {
//            PageNavigate pn = new PageNavigate();
//            decimal? CopeSum = 0;
//            decimal? PaidSum = 0;
//            decimal? WaidSum = 0;
//            Hashtable param = base.GetParameters();
//            PageList<Td_Sale_Order_Head_Query> list = new PageList<Td_Sale_Order_Head_Query>(10);
//            param.Remove("pageIndex");
//            ParamVessel p = new ParamVessel();
//            p.Add("start_rq_create", String.Empty, HandleType.Remove);
//            p.Add("end_rq_create", String.Empty, HandleType.Remove);
//            p.Add("searchValuebygys", String.Empty, HandleType.Remove, true);
//            param = param.Trim(p);
//            if (type == "Statistics")
//            {
//                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
//            }
//            else if (type == "PaymentStatistics")
//            {
//                param["id_cgs"] = GetLoginInfo<long>("id_buyer");
//            }
//            param.Add("not_flag_state", 0);
//            if (!string.IsNullOrEmpty(dh))
//            {
//                string[] dhList = dh.Split(',');
//                param.Add("dhList", dhList);
//            }
//            pn = BusinessFactory.Order.GetPage(param);
//            foreach (Td_Sale_Order_Head_Query item in (IList<Td_Sale_Order_Head_Query>)pn.Data)
//            {
//                CopeSum = CopeSum + item.je_pay;
//                PaidSum = PaidSum + item.je_payed;
//                WaidSum = CopeSum - PaidSum;
//            }
//            param.Add("sort", "rq_create");
//            param.Add("dir", "desc");
//            pn = BusinessFactory.Order.GetPage(param);
//            list = new PageList<Td_Sale_Order_Head_Query>(pn, 1, 10);
//            string str = type == "Statistics" ? "收款统计" : "付款统计";
//            //创建Excel文件的对象
//            HSSFWorkbook book = new HSSFWorkbook();
//            //添加一个sheet
//            ISheet sheet1 = book.CreateSheet(str);
//            //设置列宽
//            sheet1.SetColumnWidth(0, 14 * 256);
//            sheet1.SetColumnWidth(1, 25 * 256);
//            sheet1.SetColumnWidth(2, 25 * 256);
//            sheet1.SetColumnWidth(3, 32 * 256);
//            sheet1.SetColumnWidth(4, 14 * 256);
//            sheet1.SetColumnWidth(5, 14 * 256);
//            sheet1.SetColumnWidth(6, 14 * 256);
//            sheet1.SetColumnWidth(7, 14 * 256);
//            //初始化样式
//            ICellStyle mStyle = book.CreateCellStyle();
//            mStyle.Alignment = HorizontalAlignment.Center;
//            mStyle.VerticalAlignment = VerticalAlignment.Center;
//            mStyle.BorderBottom = BorderStyle.Thin;
//            mStyle.BorderTop = BorderStyle.Thin;
//            mStyle.BorderLeft = BorderStyle.Thin;
//            mStyle.BorderRight = BorderStyle.Thin;
//            IFont mfont = book.CreateFont();
//            mfont.FontHeight = 10 * 20;
//            mStyle.SetFont(mfont);
//            sheet1.SetDefaultColumnStyle(0, mStyle);
//            sheet1.SetDefaultColumnStyle(1, mStyle);
//            sheet1.SetDefaultColumnStyle(2, mStyle);
//            sheet1.SetDefaultColumnStyle(3, mStyle);
//            sheet1.SetDefaultColumnStyle(4, mStyle);
//            sheet1.SetDefaultColumnStyle(5, mStyle);
//            sheet1.SetDefaultColumnStyle(6, mStyle);
//            sheet1.SetDefaultColumnStyle(7, mStyle);
//            //第一行
//            IRow row = sheet1.CreateRow(0);
//            ICell cell = row.CreateCell(0);
//            cell.SetCellValue(str);
//            ICellStyle style = book.CreateCellStyle();
//            style.Alignment = HorizontalAlignment.Center;
//            style.VerticalAlignment = VerticalAlignment.Center;
//            IFont font = book.CreateFont();
//            font.FontHeight = 20 * 20;
//            style.SetFont(font);
//            cell.CellStyle = style;
//            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 7));
//            sheet1.GetRow(0).Height = 40 * 20;
//            //第二行
//            IRow row1 = sheet1.CreateRow(1);
//            row1.CreateCell(0).SetCellValue("应付金额总计");
//            row1.CreateCell(1).SetCellValue(CopeSum == null ? 0 : Convert.ToDouble(CopeSum));
//            row1.CreateCell(2).SetCellValue("已付金额总计");
//            row1.CreateCell(3).SetCellValue(PaidSum == null ? 0 : Convert.ToDouble(PaidSum));
//            row1.CreateCell(4).SetCellValue("待付金额总计");
//            row1.CreateCell(6).SetCellValue(WaidSum == null ? 0 : Convert.ToDouble(WaidSum));
//            sheet1.AddMergedRegion(new CellRangeAddress(1, 1, 4, 5));
//            sheet1.AddMergedRegion(new CellRangeAddress(1, 1, 6, 7));
//            sheet1.GetRow(1).Height = 28 * 20;
//            //第三行
//            IRow row2 = sheet1.CreateRow(2);
//            ICell cell3 = row2.CreateCell(0);
//            cell3.SetCellValue("明细");
//            cell3.CellStyle = style;
//            sheet1.AddMergedRegion(new CellRangeAddress(2, 2, 0, 7));
//            sheet1.GetRow(2).Height = 40 * 20;
//            //第四行
//            IRow row3 = sheet1.CreateRow(3);
//            row3.CreateCell(0).SetCellValue("序号");
//            row3.CreateCell(1).SetCellValue(type == "Statistics" ? "客户名称" : "供应商名称");
//            row3.CreateCell(2).SetCellValue("订单号");
//            row3.CreateCell(3).SetCellValue("下单日期");
//            row3.CreateCell(4).SetCellValue("应付金额");
//            row3.CreateCell(5).SetCellValue("已付金额");
//            row3.CreateCell(6).SetCellValue("待付金额");
//            sheet1.AddMergedRegion(new CellRangeAddress(3, 3, 6, 7));
//            sheet1.GetRow(3).Height = 28 * 20;
//            if (type == "Statistics")
//            {
//                for (int i = 0; i < list.Count; i++)
//                {
//                    IRow rowtemp = sheet1.CreateRow(i + 4);
//                    rowtemp.CreateCell(0).SetCellValue(i + 1);
//                    rowtemp.CreateCell(1).SetCellValue(list[i].alias_cgs);
//                    rowtemp.CreateCell(2).SetCellValue(list[i].dh);
//                    rowtemp.CreateCell(3).SetCellValue(list[i].rq_create.Value.ToString("yyyy-MM-dd"));
//                    rowtemp.CreateCell(4).SetCellValue(Decimal.Round((decimal)list[i].je_pay, 2).ToString());
//                    rowtemp.CreateCell(5).SetCellValue(Decimal.Round((decimal)list[i].je_payed, 2).ToString());
//                    rowtemp.CreateCell(6).SetCellValue(Decimal.Round((decimal)list[i].je_pay - (decimal)list[i].je_payed, 2).ToString());
//                    sheet1.AddMergedRegion(new CellRangeAddress(i + 4, i + 4, 6, 7));
//                    sheet1.GetRow(i + 4).Height = 28 * 20;
//                }
//            }
//            else if (type == "PaymentStatistics")
//            {
//                for (int i = 0; i < list.Count; i++)
//                {
//                    IRow rowtemp = sheet1.CreateRow(i + 4);
//                    rowtemp.CreateCell(0).SetCellValue(i + 1);
//                    rowtemp.CreateCell(1).SetCellValue(list[i].alias_gys);
//                    rowtemp.CreateCell(2).SetCellValue(list[i].dh);
//                    rowtemp.CreateCell(3).SetCellValue(list[i].rq_create.Value.ToString("yyyy-MM-dd"));
//                    rowtemp.CreateCell(4).SetCellValue(Decimal.Round((decimal)list[i].je_pay, 2).ToString());
//                    rowtemp.CreateCell(5).SetCellValue(Decimal.Round((decimal)list[i].je_payed, 2).ToString());
//                    rowtemp.CreateCell(6).SetCellValue(Decimal.Round((decimal)list[i].je_pay - (decimal)list[i].je_payed, 2).ToString());
//                    sheet1.AddMergedRegion(new CellRangeAddress(i + 4, i + 4, 6, 7));
//                    sheet1.GetRow(i + 4).Height = 28 * 20;
//                }
//            }

//            System.IO.MemoryStream ms = new System.IO.MemoryStream();
//            book.Write(ms);
//            ms.Seek(0, SeekOrigin.Begin);

//            return File(ms, "application/vnd.ms-excel", str + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
//        }
//        /// <summary>
//        /// 付款统计 cxb 2015-3-13 
//        /// </summary>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public ActionResult PaymentStatistics(Td_Sale_Order_Head_Query model)
//        {
//            BaseResult br = new BaseResult();
//            PageNavigate pn = new PageNavigate();
//            int limit = 10;
//            decimal? CopeSum = 0;
//            decimal? PaidSum = 0;
//            decimal? WaidSum = 0;
//            PageList<Td_Sale_Order_Head_Query> list = new PageList<Td_Sale_Order_Head_Query>(limit);
//            try
//            {
//                Hashtable param = base.GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
//                p.Add("start_rq_create", String.Empty, HandleType.Remove);
//                p.Add("end_rq_create", String.Empty, HandleType.Remove);
//                p.Add("searchValuebygys", String.Empty, HandleType.Remove);
//                param = param.Trim(p);
//                param["id_cgs"] = GetLoginInfo<long>("id_buyer");
//                param.Add("not_flag_state", 0);
//                pn = BusinessFactory.Order.GetPage(param);
//                foreach (Td_Sale_Order_Head_Query item in (IList<Td_Sale_Order_Head_Query>)pn.Data)
//                {
//                    CopeSum = CopeSum + item.je_pay;
//                    PaidSum = PaidSum + item.je_payed;
//                    WaidSum = CopeSum - PaidSum;
//                }

//                ViewData["Cope"] = CopeSum;
//                ViewData["Paid"] = PaidSum;
//                ViewData["Wait"] = WaidSum;
//                int pageIndex = Convert.ToInt32(param["pageIndex"]);
//                ViewData["pageIndex"] = pageIndex;

//                param.Add("sort", "rq_create");
//                param.Add("dir", "desc");
//                param.Add("limit", limit);
//                param.Add("start", (pageIndex - 1) * limit);
//                pn = BusinessFactory.Order.GetPage(param);
//                list = new PageList<Td_Sale_Order_Head_Query>(pn, pageIndex, limit);
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
//                return PartialView("_PaymentStatisticsControl", list);
//            }
//            return View(list);
//        }

//        /// <summary>
//        /// 在线支付 cxb 2015-3-13 
//        /// </summary>
//        [ActionPurview(false)]
//        public ActionResult Payment()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = new Hashtable();
//                long user_master = GetLoginInfo<long>("id_user_master");
//                param["id_user_master"] = user_master;
//                //br = BusinessFactory.Pay.CheckPayAccount(param, "yeePay");
//                bool yee_state = false;
//                bool wx_state = false;
//                int yee_flag = 1;
//                int wx_flag = 1;
//                //if (br.Data != null)
//                //{
//                //    RegisterPay reg = (RegisterPay)br.Data;
//                //    yee_state = true;
//                //    ViewBag.ledgerno = reg.ledgerno;

//                //    //ViewBag.yee_state = true;
//                //    //string url = APIURLConfig.merchantPrefix + APIURLConfig.queryBalance;
//                //    //RegisterPay reg = (RegisterPay)br.Data;
//                //    //Dictionary<string, string> dir = new Dictionary<string, string>();
//                //    //dir.Add("customernumber", Config.merchantAccount.Trim());
//                //    //dir.Add("ledgerno",reg.ledgerno);
//                //    //string hmac = Digest.GetHMAC(dir, Config.merchantKey);
//                //    //dir.Add("hmac", hmac);
//                //    //br = YeePayResult<QueryBalanceJson>.GetResult(url, dir);
//                //    //if (br.Success)
//                //    //{
//                //    //    dynamic result = br.Data;
//                //    //    if (result.code == "1")
//                //    //    {
//                //    //        balance = result.ledgerbalance.Substring(result.ledgerbalance.LastIndexOf(':') + 1);
//                //    //    }
//                //    //}
//                //    //PageNavigate pn = new PageNavigate();
//                //    //int limit = 10;
//                //    //PageList<YeePay_Trade> list = new PageList<YeePay_Trade>(limit);
//                //    //ParamVessel p = new ParamVessel();
//                //    //p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
//                //    //param = param.Trim(p);

//                //    //int pageIndex = Convert.ToInt32(param["pageIndex"]);
//                //    //ViewData["pageIndex"] = pageIndex;
//                //    //param.Add("sort", "rq_create");
//                //    //param.Add("dir", "desc");
//                //    //param.Add("limit", limit);
//                //    //param.Add("start", (pageIndex - 1) * limit);
//                //    //param.Add("id_user_master", reg.id_user_master);
//                //    //param.Add("ledgerno", reg.ledgerno);
//                //    //pn = BusinessFactory.Pay.GetPage(param);
//                //    //list = new PageList<YeePay_Trade>(pn, pageIndex, limit);
//                //    //ViewBag.balance = balance;
//                //    //return View(list);
//                //}
//                br = BusinessFactory.Pay.CheckPayAccount(param, "wxPay");
//                if (br.Data != null)
//                {
//                    WeChatAccount wx = (WeChatAccount)br.Data;
//                    wx_flag = wx.flag_state;
//                    ViewBag.mchid = wx.mchid;
//                    wx_state = true;
//                }
//                ViewBag.wx_state = wx_state;
//                ViewBag.yee_state = yee_state;
//                ViewBag.yee_flag = yee_flag;
//                ViewBag.wx_flag = wx_flag;
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return View();
//        }
//        /// <summary>
//        /// 开通在线支付
//        /// </summary>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public ActionResult OpenPay()
//        {
//            try
//            {

//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return View();
//        }

//        /// <summary>
//        /// 易宝账户注册
//        /// mq 2016-06-16
//        /// </summary>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        [HttpPost]
//        public ActionResult RegYeePay(RegisterPay model)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = new Hashtable();
//            long user_master = GetLoginInfo<long>("id_user_master");
//            try
//            {
//                param["id_user_master"] = user_master;
//                br = BusinessFactory.Pay.CheckPayAccount(param, "yeePay");
//                if (br.Data != null)
//                {
//                    br.Success = true;
//                    br.Message.Add("您已经注册过！");
//                    br.Level = ErrorLevel.Warning;
//                    model = (RegisterPay)br.Data;
//                    br.Data = model.customertype;
//                    return Json(br);
//                }
//                if (string.IsNullOrEmpty(model.bindmobile))
//                {
//                    br.Success = false;
//                    br.Message.Add("请填写手机号！");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "bindmobile";
//                    return Json(br);
//                }
//                if (string.IsNullOrEmpty(model.customertype))
//                {
//                    br.Success = false;
//                    br.Message.Add("请选择注册类型！");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "customertype";
//                    return Json(br);
//                }
//                if (model.customertype == "PERSON")
//                {
//                    if (string.IsNullOrEmpty(model.signedname))
//                    {
//                        br.Success = false;
//                        br.Message.Add("请填写个人姓名！");
//                        br.Level = ErrorLevel.Warning;
//                        br.Data = "signedname";
//                        return Json(br);
//                    }
//                    if (string.IsNullOrEmpty(model.idcard))
//                    {
//                        br.Success = false;
//                        br.Message.Add("请填写身份证号！");
//                        br.Level = ErrorLevel.Warning;
//                        br.Data = "idcard";
//                        return Json(br);
//                    }
//                    model.linkman = model.signedname;
//                    model.bankaccounttype = "PrivateCash";
//                }
//                else if (model.customertype == "ENTERPRISE")
//                {
//                    if (string.IsNullOrEmpty(model.signedname))
//                    {
//                        br.Success = false;
//                        br.Message.Add("请填写企业名称！");
//                        br.Level = ErrorLevel.Warning;
//                        br.Data = "signedname";
//                        return Json(br);
//                    }
//                    if (string.IsNullOrEmpty(model.linkman))
//                    {
//                        br.Success = false;
//                        br.Message.Add("请填写联系人！");
//                        br.Level = ErrorLevel.Warning;
//                        br.Data = "linkman";
//                        return Json(br);
//                    }
//                    if (string.IsNullOrEmpty(model.businesslicence))
//                    {
//                        br.Success = false;
//                        br.Message.Add("请填写营业执照号！");
//                        br.Level = ErrorLevel.Warning;
//                        br.Data = "businesslicence";
//                        return Json(br);
//                    }
//                    if (string.IsNullOrEmpty(model.legalperson))
//                    {
//                        br.Success = false;
//                        br.Message.Add("法人姓名！");
//                        br.Level = ErrorLevel.Warning;
//                        br.Data = "legalperson";
//                        return Json(br);
//                    }
//                    model.bankaccounttype = "PublicCash";
//                }
//                if (string.IsNullOrEmpty(model.bankaccountnumber))
//                {
//                    br.Success = false;
//                    br.Message.Add("请填写银行卡号！");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "bankaccountnumber";
//                    return Json(br);
//                }
//                if (string.IsNullOrEmpty(model.bankname))
//                {
//                    br.Success = false;
//                    br.Message.Add("请填写开户行！");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "bankname";
//                    return Json(br);
//                }

//                if (string.IsNullOrEmpty(model.bankprovince))
//                {
//                    br.Success = false;
//                    br.Message.Add("请选择开户省！");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "bankprovince";
//                    return Json(br);
//                }
//                if (string.IsNullOrEmpty(model.bankcity))
//                {
//                    br.Success = false;
//                    br.Message.Add("请选择开户市！");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "bankcity";
//                    return Json(br);
//                }
//                model.minsettleamount = "0.01";
//                model.riskreserveday = "1";
//                model.accountname = model.signedname;
//                model.manualsettle = 'N';
//                model.feetype = "TARGET";
//                string url = APIURLConfig.merchantPrefix + APIURLConfig.registerURL;
//                Dictionary<string, string> sd = new Dictionary<string, string>();
//                sd.Add("customernumber", Config.merchantAccount);
//                sd.Add("requestid", DateTime.Now.ToString("yyyyMMddHHmmss"));
//                sd.Add("bindmobile", model.bindmobile.Trim());
//                sd.Add("customertype", model.customertype.Trim());
//                sd.Add("signedname", model.signedname.Trim());
//                sd.Add("linkman", model.linkman.Trim());
//                if (model.customertype.Trim() == "PERSON")
//                {
//                    sd.Add("idcard", model.idcard.Trim());
//                    sd.Add("businesslicence", "");
//                    sd.Add("legalperson", "");
//                    model.businesslicence = "";
//                    model.legalperson = "";
//                }
//                else if (model.customertype.Trim() == "ENTERPRISE")
//                {
//                    sd.Add("idcard", "");
//                    sd.Add("businesslicence", model.businesslicence.Trim());
//                    sd.Add("legalperson", model.legalperson.Trim());
//                    model.idcard = "";
//                }
//                sd.Add("minsettleamount", model.minsettleamount.Trim());
//                sd.Add("riskreserveday", model.riskreserveday.Trim());
//                sd.Add("bankaccountnumber", model.bankaccountnumber.Trim());
//                sd.Add("bankname", model.bankname.Trim());
//                sd.Add("accountname", model.accountname.Trim());
//                sd.Add("bankaccounttype", model.bankaccounttype.Trim());
//                sd.Add("bankprovince", model.bankprovince.Trim());
//                sd.Add("bankcity", model.bankcity.Trim());
//                string hmac = Digest.GetHMAC(sd, Config.merchantKey);
//                sd.Add("manualsettle", model.manualsettle.ToString());
//                sd.Add("hmac", hmac);
//                br = YeePayResult<QueryBalanceJson>.GetResult(url, sd);
//                if (br.Success == false)
//                {
//                    return Json(br);
//                }
//                dynamic result = br.Data;
//                RegisterPayLog log = new RegisterPayLog();
//                log.id = BusinessFactory.Utilety.GetNextKey(typeof(RegisterPayLog));
//                log.id_user_master = user_master;
//                log.type_action = PayFlag.register;
//                if (result.code == "1")
//                {
//                    log.flag_action = PayFlag.Done;
//                    log.ledgerno = result.ledgerno;
//                    log.des_action = "支付账户注册成功";
//                    log.bm_error = "";
//                    log.msg_error = "";
//                    model.ledgerno = result.ledgerno;
//                    model.id_user_master = user_master;
//                    BusinessFactory.Pay.AddPayData(model, "reg");
//                }
//                else
//                {
//                    log.ledgerno = "";
//                    log.flag_action = PayFlag.Fail;
//                    log.des_action = "支付账户注册失败";
//                    log.bm_error = result.code;
//                    log.msg_error = result.msg;
//                }
//                br = BusinessFactory.Pay.AddPayData(log, "log");
//            }
//            catch (Exception)
//            {
//                RegisterPayLog log = new RegisterPayLog();
//                log.id = BusinessFactory.Utilety.GetNextKey(typeof(RegisterPayLog));
//                log.id_user_master = user_master;
//                log.type_action = PayFlag.register;
//                log.flag_action = PayFlag.Error;
//                log.des_action = "服务器内部错误";
//                BusinessFactory.Pay.AddPayData(log, "log");
//                br.Message.Add("内部服务器错误，请联系客服！");
//                br.Success = false;
//            }
//            return Json(br);
//        }
//        /// <summary>
//        /// 加载城市数据
//        /// </summary>
//        /// <param name="type"></param>
//        /// <param name="keyword"></param>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult JsonData(string type = "", string keyword = "", string value = "")
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = new Hashtable();
//                param.Add(type, type);
//                if (keyword != "" && value != "")
//                {
//                    param.Add(keyword, value);
//                }
//                br = BusinessFactory.Pay.GetCity(param);
//                br.Success = true;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }
//        /// <summary>
//        /// 加载银行数据
//        /// </summary>
//        /// <param name="province"></param>
//        /// <param name="city"></param>
//        /// <returns></returns>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult BanksData(string province = "", string city = "")
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = new Hashtable();
//                if (province != "" && city != "")
//                {
//                    param.Add("province", province);
//                    param.Add("city", city);
//                }
//                br = BusinessFactory.Pay.GetBanks(param);
//                br.Success = true;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }
//        /// <summary>
//        /// 易宝实名认证
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        [HttpPost]
//        public ActionResult Certified(CertifiedFile model)
//        {

//            BaseResult br = new BaseResult();
//            Hashtable param = new Hashtable();
//            long user_master = GetLoginInfo<long>("id_user_master");
//            string regType = "";
//            string[] str = null;
//            bool active = false;
//            try
//            {
//                param["id_user_master"] = user_master;
//                br = BusinessFactory.Pay.CheckPayAccount(param, "yeePay");
//                if (br.Data == null)
//                {
//                    br.Success = false;
//                    br.Message.Add("您尚未注册过！");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "not_reg";
//                    return Json(br);
//                }
//                else
//                {
//                    if (string.IsNullOrEmpty(model.id_card_front))
//                    {
//                        br.Success = false;
//                        br.Message.Add("请上传身份证正面照！");
//                        br.Level = ErrorLevel.Warning;
//                        return Json(br);
//                    }
//                    if (string.IsNullOrEmpty(model.id_card_back))
//                    {
//                        br.Success = false;
//                        br.Message.Add("请上传身份证背面照！");
//                        br.Level = ErrorLevel.Warning;
//                        return Json(br);
//                    }
//                    RegisterPay reg = (RegisterPay)br.Data;
//                    regType = reg.customertype;
//                    model.ledgerno = reg.ledgerno;
//                    model.id_user_master = user_master;
//                    if (regType == "PERSON")
//                    {
//                        if (string.IsNullOrEmpty(model.bank_card_front))
//                        {
//                            br.Success = false;
//                            br.Message.Add("请上传银行卡正面照！");
//                            br.Level = ErrorLevel.Warning;
//                            return Json(br);
//                        }
//                        if (string.IsNullOrEmpty(model.person_photo))
//                        {
//                            br.Success = false;
//                            br.Message.Add("请上传手持身份证照！");
//                            br.Level = ErrorLevel.Warning;
//                            return Json(br);
//                        }
//                        str = new string[] { "ID_CARD_FRONT", "ID_CARD_BACK", "BANK_CARD_FRONT", "PERSON_PHOTO" };
//                    }
//                    else if (regType == "ENTERPRISE")
//                    {
//                        if (string.IsNullOrEmpty(model.bussiness_license))
//                        {
//                            br.Success = false;
//                            br.Message.Add("请上传营业执照！");
//                            br.Level = ErrorLevel.Warning;
//                            return Json(br);
//                        }
//                        if (string.IsNullOrEmpty(model.organization_code))
//                        {
//                            br.Success = false;
//                            br.Message.Add("请上传组织机构代码证！");
//                            br.Level = ErrorLevel.Warning;
//                            return Json(br);
//                        }
//                        if (string.IsNullOrEmpty(model.tax_registration))
//                        {
//                            br.Success = false;
//                            br.Message.Add("请上传税务登记证！");
//                            br.Level = ErrorLevel.Warning;
//                            return Json(br);
//                        }
//                        if (string.IsNullOrEmpty(model.bank_account_licence))
//                        {
//                            br.Success = false;
//                            br.Message.Add("请上传银行开户许可证！");
//                            br.Level = ErrorLevel.Warning;
//                            return Json(br);
//                        }
//                        str = new string[] { "ID_CARD_FRONT", "ID_CARD_BACK", "BUSSINESS_LICENSE", "ORGANIZATION_CODE", "TAX_REGISTRATION", "BANK_ACCOUNT_LICENCE" };
//                    }
//                }
//                if (str != null)
//                {
//                    Dictionary<string, string> dir = new Dictionary<string, string>();
//                    string url = APIURLConfig.merchantPrefix + APIURLConfig.verifyURL;
//                    string imgPath = "";
//                    string imgName = "";
//                    foreach (string item in str)
//                    {
//                        dir.Clear();
//                        dir.Add("customernumber", Config.merchantAccount.Trim());
//                        dir.Add("ledgerno", model.ledgerno.Trim());
//                        dir.Add("filetype", item.Trim());
//                        string hmac = Digest.GetHMAC(dir, Config.merchantKey);
//                        dir.Add("hmac", hmac);
//                        string info_json = JSON.Serialize(dir);
//                        string data = AESUtil.Encrypt(info_json, Config.AescKey);
//                        dir.Clear();
//                        dir.Add("data", data);
//                        dir.Add("customernumber", Config.merchantAccount);
//                        switch (item.Trim())
//                        {
//                            case "ID_CARD_FRONT":
//                                imgPath = model.id_card_front;
//                                imgName = " 身份证正面";
//                                break;
//                            case "ID_CARD_BACK":
//                                imgPath = model.id_card_back;
//                                imgName = " 身份证背面";
//                                break;
//                            case "BANK_CARD_FRONT":
//                                imgPath = model.bank_card_front;
//                                imgName = " 银行卡正面";
//                                break;
//                            case "BANK_CARD_BACK":
//                                imgPath = model.bank_card_back;
//                                imgName = " 银行卡背面";
//                                break;
//                            case "PERSON_PHOTO":
//                                imgPath = model.person_photo;
//                                imgName = " 手持身份证照片";
//                                break;
//                            case "BUSSINESS_LICENSE":
//                                imgPath = model.bussiness_license;
//                                imgName = " 营业执照";
//                                break;
//                            case "BUSSINESS_CERTIFICATES":
//                                imgPath = model.bussiness_certificates;
//                                imgName = " 工商证";
//                                break;
//                            case "ORGANIZATION_CODE":
//                                imgPath = model.organization_code;
//                                imgName = " 组织机构代码证";
//                                break;
//                            case "TAX_REGISTRATION":
//                                imgPath = model.tax_registration;
//                                imgName = " 税务登记证";
//                                break;
//                            case "BANK_ACCOUNT_LICENCE":
//                                imgPath = model.bank_account_licence;
//                                imgName = " 银行开户许可证";
//                                break;
//                            default:
//                                break;
//                        }
//                        string fileName = imgPath.Substring(imgPath.LastIndexOf('/') + 1);
//                        string ybResult = HttpHelp.PostFile(url, dir, ApplicationInfo.PathRoot + imgPath, fileName);
//                        dynamic result = YeePayResult<CertifiedResultJson>.GetResult(ybResult);
//                        RegisterPayLog log = new RegisterPayLog();
//                        log.id = BusinessFactory.Utilety.GetNextKey(typeof(RegisterPayLog));
//                        log.id_user_master = user_master;
//                        log.type_action = PayFlag.certified;
//                        log.ledgerno = model.ledgerno;
//                        if (result.code == "1")
//                        {
//                            log.flag_action = PayFlag.Done;
//                            if (result.active)
//                            {
//                                active = result.active;
//                                log.des_action = "资质认证成功,账户已激活";
//                            }
//                            else
//                            {
//                                log.des_action = imgName + "认证";
//                            }

//                            log.bm_error = "";
//                            log.msg_error = "";
//                        }
//                        else
//                        {
//                            log.flag_action = PayFlag.Fail;
//                            log.des_action = imgName + "认证失败";
//                            log.bm_error = result.code;
//                            log.msg_error = result.msg;
//                        }
//                        BusinessFactory.Pay.AddPayData(log, "log");
//                    }
//                    if (active)
//                    {
//                        param["ledgerno"] = model.ledgerno;
//                        param["new_active"] = 0;
//                        BusinessFactory.Pay.Update(param, "pay");
//                        br.Success = true;
//                        br.Message.Add("资质认证成功,账户已激活");
//                    }
//                    else
//                    {
//                        br.Success = false;
//                        br.Message.Add("认证失败");
//                    }
//                    BusinessFactory.Pay.AddPayData(model, "certified");
//                }

//            }
//            catch (Exception)
//            {
//                RegisterPayLog log = new RegisterPayLog();
//                log.id = BusinessFactory.Utilety.GetNextKey(typeof(RegisterPayLog));
//                log.id_user_master = user_master;
//                log.type_action = PayFlag.certified;
//                log.flag_action = PayFlag.Error;
//                log.des_action = "服务器内部错误";
//                BusinessFactory.Pay.AddPayData(log, "log");
//                br.Message.Add("内部服务器错误，请联系客服！");
//                br.Success = false;
//            }

//            return Json(br);
//        }
//        /// <summary>
//        /// 易宝提现
//        /// </summary>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public ActionResult CashTransferView()
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = new Hashtable();
//            long user_master = GetLoginInfo<long>("id_user_master");
//            string bankname = "获取失败";
//            string bankNo = "获取失败";
//            string feetype = "SOURCE";
//            param.Add("id_user_master", user_master);
//            br = BusinessFactory.Pay.CheckPayAccount(param, "yeePay");
//            if (br.Data != null)
//            {
//                RegisterPay pay = (RegisterPay)br.Data;
//                bankNo = pay.bankaccountnumber.Substring(pay.bankaccountnumber.Length - 4);
//                feetype = pay.feetype;
//                param.Clear();
//                param.Add("sub_bank", pay.bankname);
//                br = BusinessFactory.Pay.GetBanks(param);
//                if (br.Data != null)
//                {
//                    List<YeePay_Banks> list = (List<YeePay_Banks>)br.Data;
//                    bankname = list[0].parent_bank;
//                }
//            }
//            ViewBag.bankname = bankname;
//            ViewBag.bankNo = bankNo;
//            ViewBag.feetype = feetype;
//            return PartialView("_CashTransfer");
//        }
//        [ActionPurview(false)]
//        [HttpPost]
//        public ActionResult CashTransfer()
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = GetParameters();
//            try
//            {
//                if (!param.ContainsKey("amount") || string.IsNullOrEmpty(param["amount"].ToString()))
//                {
//                    br.Success = false;
//                    br.Message.Add("请填写提现金额");
//                    br.Level = ErrorLevel.Warning;
//                    return Json(br);
//                }
//                if (Convert.ToDouble(param["amount"]) <= 0)
//                {
//                    br.Success = false;
//                    br.Message.Add("请填写正确的提现金额");
//                    br.Level = ErrorLevel.Warning;
//                    return Json(br);
//                }
//            }
//            catch (Exception)
//            {
//                br.Success = false;
//                br.Message.Add("请填写正确的提现金额");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//            }
//            long user_master = GetLoginInfo<long>("id_user_master");
//            param.Add("id_user_master", user_master);
//            br = BusinessFactory.Pay.CheckPayAccount(param, "yeePay");
//            if (br.Data != null)
//            {
//                string balance = "";
//                string url = APIURLConfig.merchantPrefix + APIURLConfig.queryBalance;
//                RegisterPay pay = (RegisterPay)br.Data;
//                Dictionary<string, string> dir = new Dictionary<string, string>();
//                dir.Add("customernumber", Config.merchantAccount.Trim());
//                dir.Add("ledgerno", pay.ledgerno.Trim());
//                string hmac = Digest.GetHMAC(dir, Config.merchantKey);
//                dir.Add("hmac", hmac);
//                br = YeePayResult<QueryBalanceJson>.GetResult(url, dir);
//                if (br.Success == false)
//                {
//                    return Json(br);
//                }
//                dynamic result = br.Data;
//                if (result.code == "1")
//                {
//                    balance = result.ledgerbalance.Substring(result.ledgerbalance.LastIndexOf(':') + 1);
//                }
//                if (Convert.ToDouble(param["amount"]) > Convert.ToDouble(balance))
//                {
//                    br.Success = false;
//                    br.Message.Add("提现金额不得大于账户余额！");
//                    br.Level = ErrorLevel.Warning;
//                    return Json(br);
//                }
//                dir.Clear();
//                url = APIURLConfig.merchantPrefix + APIURLConfig.cashTransfer;
//                dir.Add("customernumber", Config.merchantAccount.Trim());
//                dir.Add("requestid", user_master.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss"));
//                dir.Add("ledgerno", pay.ledgerno.Trim());
//                dir.Add("amount", param["amount"].ToString());
//                dir.Add("callbackurl", "");
//                hmac = Digest.GetHMAC(dir, Config.merchantKey);
//                if (param.ContainsKey("feetype") || !string.IsNullOrEmpty(param["feetype"].ToString()))
//                {
//                    dir.Add("feetype", param["feetype"].ToString());
//                }
//                dir.Add("hmac", hmac);
//                br = YeePayResult<QueryBalanceJson>.GetResult(url, dir);
//                if (br.Success == false)
//                {
//                    return Json(br);
//                }
//                dynamic result2 = br.Data;
//            }
//            else
//            {
//                br.Success = false;
//                br.Message.Add("网络错误，请稍后重试！");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//            }
//            return Json(br);
//        }
//        #region 易宝支付请求
//        /// <summary>
//        /// 易宝支付请求
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        [HttpPost]
//        public ActionResult YeePay(YeePayJson model)
//        {
//            BaseResult br = new BaseResult();
//            try
//            {

//                //if (string.IsNullOrEmpty(model.payproducttype))
//                //{
//                //    br.Success = false;
//                //    br.Message.Add("请选择支付方式!");
//                //    br.Level = ErrorLevel.Warning;
//                //    return Json(br);
//                //}
//                if (string.IsNullOrEmpty(model.assure) || model.assure == "0")
//                {
//                    model.assure = "0";
//                    model.period = "";
//                }
//            }
//            catch (Exception)
//            {
//                br.Success = false;
//                br.Message.Add("您输入的担保期限不正确");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//            }
//            Hashtable param = new Hashtable();
//            param["id_user_master"] = model.id_user_master_gys;
//            br = BusinessFactory.Pay.CheckPayAccount(param, "yeePay");
//            if (br.Data == null)
//            {
//                br.Success = false;
//                br.Message.Add("网络繁忙，请稍后重试！");
//                return Json(br);
//            }
//            RegisterPay reg = (RegisterPay)br.Data;
//            Td_Sale_Pay sale_pay = new Td_Sale_Pay();
//            sale_pay.dh_order = model.requestid;
//            sale_pay.flag_pay = "onlink";
//            sale_pay.flag_state = 1;
//            sale_pay.je = Convert.ToDecimal(model.amount);
//            sale_pay.rq_create = DateTime.Now;
//            BusinessFactory.Utilety.GetNextDH(sale_pay, typeof(Td_Sale_Pay));
//            var returnUrl = string.Format("{0}://{1}/Funds/YeePayNotify", Request.Url.Scheme, Request.Url.Authority);
//            var returnWebUrl = string.Format("{0}://{1}/Order/List", Request.Url.Scheme, Request.Url.Authority);
//            string url = APIURLConfig.merchantPrefix + APIURLConfig.webpayURL;
//            Dictionary<string, string> dir = new Dictionary<string, string>();
//            dir.Add("customernumber", Config.merchantAccount);
//            dir.Add("requestid", sale_pay.dh);
//            dir.Add("amount", "0.1");
//            dir.Add("assure", "0");
//            dir.Add("productname", model.requestid);
//            dir.Add("productcat", "");
//            dir.Add("productdesc", "支付单号：" + model.requestid);
//            dir.Add("divideinfo", reg.ledgerno + ":0.95");
//            dir.Add("callbackurl", returnUrl);
//            dir.Add("webcallbackurl", returnUrl);
//            dir.Add("bankid", "");
//            dir.Add("period", "");
//            dir.Add("memo", "");
//            string hmac = Digest.GetHMAC(dir, Config.merchantKey);
//            dir.Add("hmac", hmac);
//            dir.Add("payproducttype", "SALES");
//            br = YeePayResult<PayRequestJson>.GetResult(url, dir);
//            if (br.Success == false)
//            {
//                return Json(br);
//            }
//            dynamic result = br.Data;
//            if (result.code == "1")
//            {
//                br = BusinessFactory.Funds.Add(sale_pay);
//                YeePay_Trade trade = new YeePay_Trade();
//                trade.ledgerno = reg.ledgerno;
//                trade.id_user_master = model.id_user_master_gys;
//                trade.flag_trade = PayFlag.receivables;
//                trade.rq_trade = DateTime.Now;
//                trade.tradeid = sale_pay.dh;
//                trade.je_trade = decimal.Round((decimal)0.1, 2); //Convert.ToDecimal(model.amount);
//                trade.flag_state = PayFlag.INIT;
//                trade.rq_create = DateTime.Now;
//                BusinessFactory.Pay.AddPayData(trade, "trade");
//                if (br.Success)
//                {
//                    WriteDBLog(LogFlag.Bill, br.Message);
//                }
//                br.Success = true;
//                br.Message.Add(sale_pay.dh);
//                br.Data = result.payurl;
//            }
//            else
//            {
//                br.Success = false;
//                br.Message.Add("网络繁忙，请稍后重试！");
//            }

//            return Json(br);
//        }
//        #endregion

//        /// <summary>
//        /// 查询订单支付结果
//        /// </summary>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        [HttpPost]
//        public ActionResult QueryOrder(string dh_pay)
//        {
//            BaseResult br = new BaseResult();
//            if (string.IsNullOrEmpty(dh_pay))
//            {
//                br.Success = false;
//                br.Message.Add("订单号丢失，请稍后重试！");
//                return Json(br);
//            }
//            string url = APIURLConfig.merchantPrefix + APIURLConfig.queryOrder;
//            Dictionary<string, string> dir = new Dictionary<string, string>();
//            dir.Add("customernumber", Config.merchantAccount);
//            dir.Add("requestid", dh_pay);
//            string hmac = Digest.GetHMAC(dir, Config.merchantKey);
//            dir.Add("hmac", hmac);
//            br = YeePayResult<queryOrderJson>.GetResult(url, dir);
//            if (br.Success == false)
//            {
//                return Json(br);
//            }
//            dynamic result = br.Data;
//            if (result.code == "1")
//            {
//                br.Success = true;
//                br.Data = result.status;
//                Hashtable param = new Hashtable();
//                param["tradeid"] = dh_pay;
//                param["new_flag_state"] = result.status;
//                BusinessFactory.Pay.Update(param, "trade");
//            }
//            else
//            {
//                br.Success = false;
//                br.Message.Add("网络繁忙，请重试！");
//            }
//            return Json(br);
//        }
//        [ActionPurview(false)]
//        [HttpGet]
//        public ActionResult YeePayNotify()
//        {
//            Hashtable param = GetParameters();
//            return Json("SUCCESS");
//        }
//        /// <summary>
//        /// 加载微信账户设置视图
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult WXInfo()
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = new Hashtable();
//            long user_master = GetLoginInfo<long>("id_user_master");
//            param.Add("item_mchid", "");
//            param["id_user_master"] = user_master;
//            br = BusinessFactory.Pay.CheckPayAccount(param, "wxPay");
//            WeChatAccount wx = new WeChatAccount();
//            if (br.Data != null)
//            {
//                wx = (WeChatAccount)br.Data;
//            }
//            return PartialView(wx);
//        }
//        /// <summary>
//        /// 添加/修改微信账户信息
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult AddWXInfo(WeChatAccount model, string type)
//        {
//            BaseResult br = new BaseResult();
//            //if (string.IsNullOrEmpty(model.appid.Trim()))
//            //{
//            //    br.Success = false;
//            //    br.Message.Add("请填写AppID(应用ID)！");
//            //    br.Level = ErrorLevel.Warning;
//            //    return Json(br);
//            //}
//            //if (string.IsNullOrEmpty(model.appsecret.Trim()))
//            //{
//            //    br.Success = false;
//            //    br.Message.Add("请填写AppSecret(应用密钥)！");
//            //    br.Level = ErrorLevel.Warning;
//            //    return Json(br);
//            //}
//            if (string.IsNullOrEmpty(model.mchid.Trim()))
//            {
//                br.Success = false;
//                br.Message.Add("请填写Mchid(商户号)！");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//            }
//            //if (string.IsNullOrEmpty(model.appkey.Trim()))
//            //{
//            //    br.Success = false;
//            //    br.Message.Add("请填写APPkey(API密钥)！");
//            //    br.Level = ErrorLevel.Warning;
//            //    return Json(br);
//            //}
//            long user_master = GetLoginInfo<long>("id_user_master");
//            if (type == "add")
//            {
//                model.id_user_master = user_master;
//                model.flag_state = 1;
//                model.id_create = user_master;
//                model.rq_create = DateTime.Now;
//                //model.id_edit = user_master;
//                //model.rq_edit = DateTime.Now;
//                br = BusinessFactory.Pay.AddPayData(model, "wxAccount");
//            }
//            if (type == "updata")
//            {
//                Hashtable param = new Hashtable();
//                param["id_user_master"] = user_master;
//                br = BusinessFactory.Pay.CheckPayAccount(param, "wxPay");
//                if (br.Data != null)
//                {
//                    WeChatAccount wx = (WeChatAccount)br.Data;
//                    //wx.appid = model.appid;
//                    //wx.appsecret = model.appsecret;
//                    wx.mchid = model.mchid;
//                    //wx.appkey = model.appkey;
//                    wx.id_edit = user_master;
//                    wx.rq_edit = DateTime.Now;
//                    br = BusinessFactory.Pay.Update(wx, "wxInfo");
//                    if ((int)br.Data == 1)
//                    {
//                        br.Success = true;
//                        br.Message.Add("修改成功！");
//                    }
//                }

//            }
//            return Json(br);
//        }

//        /// <summary>
//        /// 停用/启用在线支付
//        /// </summary>
//        /// <param name="state"></param>
//        /// <param name="type"></param>
//        /// <returns></returns>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult ClosePay(int state, string type)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = new Hashtable();
//            long user_master = GetLoginInfo<long>("id_user_master");
//            param["id_user_master"] = user_master;
//            if (type == "wxPay")
//            {
//                br = BusinessFactory.Pay.CheckPayAccount(param, "wxPay");
//                if (br.Data != null)
//                {
//                    WeChatAccount wx = (WeChatAccount)br.Data;
//                    wx.flag_state = state;
//                    br = BusinessFactory.Pay.Update(wx, "wxInfo");
//                    if ((int)br.Data == 1)
//                    {
//                        br.Success = true;
//                        if (state == 0)
//                        {
//                            br.Message.Add("已停用微信支付！");
//                        }
//                        else
//                        {
//                            br.Message.Add("已开启微信支付！");
//                        }
//                    }
//                    else
//                    {
//                        br.Success = false;
//                        br.Message.Add("失败，请稍后重试！");
//                    }
//                }
//                else
//                {
//                    br.Success = false;
//                    br.Message.Add("网络繁忙，请稍后重试！");
//                }
//            }
//            else if (type == "yeePay")
//            {

//            }

//            return Json(br);
//        }

//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult WxPay(string md5Gys, string dh, string je)
//        {
//            BaseResult br = new BaseResult();

//            if (string.IsNullOrEmpty(md5Gys))
//            {
//                br.Success = false;
//                br.Message.Add("请求支付参数丢失，请重试！");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//            }
//            if (string.IsNullOrEmpty(dh))
//            {
//                br.Success = false;
//                br.Message.Add("请求支付参数丢失，请重试！");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//            }
//            if (string.IsNullOrEmpty(je))
//            {
//                br.Success = false;
//                br.Message.Add("请输入支付金额！");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//            }
//            double total_fee = 1;
//            try 
//	        {
//                total_fee = Convert.ToDouble(je) * 100;
//	        }
//	        catch (Exception)
//	        {
//                br.Success = false;
//                br.Message.Add("支付金额格式错误！");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//	        }
//            Hashtable param = new Hashtable();
//            param.Add("dh", dh);
//            br = BusinessFactory.Order.Get(param);
//            if (br.Data != null)
//            {
//                Td_Sale_Order_Head_Query head = (Td_Sale_Order_Head_Query)br.Data;
//                if (md5Gys == MD5Encrypt.Encrypt(head.id_user_master_gys.ToString()))
//                {
//                    param.Clear();
//                    param["id_user_master"] = head.id_user_master_gys;
//                    br = BusinessFactory.Pay.CheckPayAccount(param, "wxPay");
//                    if (br.Data != null)
//                    {
//                        var returnUrl = string.Format("{0}://{1}/WxNotify.aspx", Request.Url.Scheme, Request.Url.Authority);
//                        Td_Sale_Pay sale_pay = new Td_Sale_Pay();
//                        BusinessFactory.Utilety.GetNextDH(sale_pay, typeof(Td_Sale_Pay));
//                        WeChatAccount wx = (WeChatAccount)br.Data;
//                        WxPayData data = new WxPayData();
//                        data.SetValue("sub_mch_id", wx.mchid);
//                        data.SetValue("body", dh);//商品描述
//                        data.SetValue("out_trade_no", sale_pay.dh);
//                        data.SetValue("total_fee",Convert.ToInt32(total_fee));//总金额
//                        data.SetValue("notify_url", returnUrl);
//                        data.SetValue("trade_type", "NATIVE");
//                        data.SetValue("product_id", sale_pay.dh);
//                        data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
//                        data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
//                        WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
//                        string return_code = result.GetValue("return_code").ToString();
//                        if (return_code == "SUCCESS")
//                        {
//                            Console.WriteLine(return_code);
//                            string result_code = result.GetValue("result_code").ToString();
//                            if (result_code == "SUCCESS")
//                            {
//                                string code_url = result.GetValue("code_url").ToString();
//                                Bitmap image = new QRCodeEncoder
//                                {
//                                    QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
//                                    QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M,
//                                    QRCodeVersion = 0,
//                                    QRCodeScale = 4
//                                }.Encode(code_url, Encoding.Default);
//                                MemoryStream ms = new MemoryStream();
//                                string imgPath = ApplicationInfo.TempPath + "/" + sale_pay.dh + ".png";
//                                image.Save(imgPath, ImageFormat.Png);
//                                image.Dispose();
//                                Dictionary<string, string> dir = new Dictionary<string, string>();
//                                dir.Add("mchid", wx.mchid);
//                                dir.Add("out_trade_no", sale_pay.dh);
//                                dir.Add("path", "/UpLoad/Temp/" + sale_pay.dh + ".png");
//                                dir.Add("dh",dh);
//                                dir.Add("je", je);
//                                br.Success = true;
//                                br.Message.Add("请扫码支付");
//                                br.Data = dir;
//                                return Json(br);
//                            }
//                            else
//                            {
//                                string err_code = result.GetValue("err_code").ToString();
//                                if (err_code == "ORDERPAID")
//                                {
//                                    br.Success = false;
//                                    br.Message.Add("订单已支付，无需重复操作！");
//                                    br.Level = ErrorLevel.Warning;
//                                    return Json(br);
//                                }
//                                if (err_code == "ORDERCLOSED")
//                                {
//                                    br.Success = false;
//                                    br.Message.Add("当前订单已关闭，请重新下单！");
//                                    br.Level = ErrorLevel.Warning;
//                                    return Json(br);
//                                }
//                                if (err_code == "OUT_TRADE_NO_USED")
//                                {
//                                    br.Success = false;
//                                    br.Message.Add("同一笔交易不能多次提交！");
//                                    br.Level = ErrorLevel.Warning;
//                                    return Json(br);
//                                }
//                                if (err_code == "MCHID_NOT_EXIST")
//                                {
//                                    br.Success = false;
//                                    br.Message.Add("供应商微信商户号设置错误！");
//                                    br.Level = ErrorLevel.Warning;
//                                    return Json(br);
//                                }
//                                if (err_code == "APPID_MCHID_NOT_MATCH")
//                                {
//                                    br.Success = false;
//                                    br.Message.Add("供应商微信商户号设置错误！");
//                                    br.Level = ErrorLevel.Warning;
//                                    return Json(br);
//                                }
//                            }
//                        }
//                        else
//                        {
//                            br.Success = false;
//                            br.Message.Add("支付请求失败，请稍后重试！");
//                            br.Level = ErrorLevel.Warning;
//                            return Json(br);
//                        }
//                    }
//                    else
//                    {
//                        br.Success = false;
//                        br.Message.Add("供应商未开通微信支付！");
//                        br.Level = ErrorLevel.Warning;
//                        return Json(br);
//                    }


//                }
//            }
//            else
//            {
//                br.Success = false;
//                br.Message.Add("支付单号丢失，请稍后重试！");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//            }
//            return Json(br);
//        }
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult WxNotify(string mchid, string out_trade_no, string dh,string je)
//        {
//            BaseResult br = new BaseResult();
//            br.Success = false;
//            WxPayData data = new WxPayData();
//            data.SetValue("sub_mch_id", mchid);
//            data.SetValue("out_trade_no", out_trade_no);
//            WxPayData result = WxPayApi.OrderQuery(data);
//            string return_code = result.GetValue("return_code").ToString();
//            if (return_code == "SUCCESS")
//            {
//                if (result.GetValue("result_code").ToString() == "SUCCESS")
//                {
//                    string trade_state = result.GetValue("trade_state").ToString();
//                    string msg = "";
//                    switch (trade_state)
//                    {
//                        case "SUCCESS":
//                            msg = "该订单已支付成功";
//                            br.Success = true;
//                            Td_Sale_Pay sale_pay = new Td_Sale_Pay();
//                            sale_pay.dh = out_trade_no;
//                            sale_pay.dh_order = dh;
//                            sale_pay.flag_pay = "onlink";
//                            sale_pay.flag_state = 1;
//                            sale_pay.je = Convert.ToDecimal(je);
//                            sale_pay.rq_create = DateTime.Now;
//                            BusinessFactory.Funds.Add(sale_pay);
//                            break;
//                        case "CLOSED":
//                            msg = "该订单交易已关闭";
//                            br.Success = true;
//                            break;
//                        case "PAYERROR":
//                            msg = "订单支付失败";
//                            br.Success = true;
//                            break;
//                        default:
//                            break;
//                    }
//                    br.Data = trade_state;
//                    br.Message.Add(msg);
//                }
//            }
//            return Json(br);
//        }
//        [ActionPurview(false)]
//        public void Alipay()
//        {
//            var returnUrl = string.Format("{0}://{1}/Funds/AlipayNotify", Request.Url.Scheme, Request.Url.Authority);
//            PaymentParms param = new PaymentParms
//            {
//                name = "hishop.plugins.payment.alipaydirect.directrequest",
//                subject = "xxx",
//                body = "xxx",
//                orderId = "cy" + DateTime.Now.ToString("yyyyMMddHHmmss"),
//                returnUrl = returnUrl,
//                notifyUrl = returnUrl,
//                showUrl = returnUrl,
//                amount = Convert.ToDecimal(0.01),
//                sellerEmail = "wellsco@yeah.net",
//                partner = "2088511231930430",
//                key = "w5wcogx3f1a0zjtyazvh5g8cqy7eu1gj"
//            };
//            var payRequest = PayFactory.CreatePaymentRequest(param);
//            payRequest.SendRequest();
//        }
//        [ActionPurview(false)]
//        public void AlipayNotify()
//        {
//            PaymentNotify notify = PayFactory.CreatePaymentNotify("alipay");
//            Hashtable param = GetParameters();
//            var result = notify.Verify(param);
//            if (result)
//            {
//                switch (param["trade_status"].ToString())
//                {
//                    // 支付宝的买家已付款表示买家已付款给支付宝
//                    case "WAIT_SELLER_SEND_GOODS":
//                        break;
//                    // 支付宝的交易结束就是打款到商家账户上了
//                    case "TRADE_FINISHED":
//                        break;
//                }
//            }
//        }
//        [ActionPurview(false)]
//        public void TenPay()
//        {
//            var returnUrl = string.Format("{0}://{1}/Funds/TenpayNotify", Request.Url.Scheme, Request.Url.Authority);
//            PaymentParms param = new PaymentParms
//            {
//                name = "tenpay",
//                subject = "xxx",
//                orderId = "cy" + DateTime.Now.ToString("yyyyMMddHHmmss"),
//                returnUrl = returnUrl,
//                amount = Convert.ToDecimal(0.01),
//                partner = "1900000109",
//                key = "8934e7d15453e97507ef794cf7b0519d"
//            };
//            var payRequest = PayFactory.CreatePaymentRequest(param);
//            payRequest.SendRequest();
//        }
//        [ActionPurview(false)]
//        public void TenpayNotify()
//        {
//            PaymentNotify notify = PayFactory.CreatePaymentNotify("tenpay");
//            Hashtable param = GetParameters();
//            var result = notify.Verify(param);

//        }


//        /// <summary>
//        /// 取消收款记录 cxb
//        /// </summary>
//        [ActionPurview(false)]
//        [HttpPost]
//        public ActionResult CancelRecord()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("dh", string.Empty, HandleType.ReturnMsg);
//                param = param.Trim(p);

//                param.Add("id_edit", GetLoginInfo<long>("id_user"));
//                br = BusinessFactory.Funds.CancelRecord(param);
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
//        /// 采购商 付款记录
//        /// znt 2015-04-27
//        /// </summary>
//        [ActionPurview(false)]
//        [HttpPost]
//        public ActionResult Add(Td_Sale_Pay model)
//        {
//            BaseResult br = new BaseResult();
//            if (model.dh_order.IsEmpty())
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("订单号校验失败，请重新刷新页面。"));
//                br.Data = "dh_order";
//                return Json(br);
//            }
//            if (model.rq_create.Value == null)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("付款日期不能为空。"));
//                br.Data = "rq_create";
//                return Json(br);
//            }
//            if (model.khr.IsEmpty())
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("开户人不能为空。"));
//                br.Data = "khr";
//                return Json(br);
//            }
//            if (model.name_bank.IsEmpty())
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("开户银行不能为空。"));
//                br.Data = "name_bank";
//                return Json(br);
//            }
//            if (model.account_bank.IsEmpty())
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("银行账号不能为空。"));
//                br.Data = "account_bank";
//                return Json(br);
//            }
//            if (model.je <= 0)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("支付失败，请正确填写付款金额!"));
//                br.Data = "je";
//                return Json(br);
//            }
//            try
//            {
//                br = BusinessFactory.Utilety.GetNextDH(model, typeof(Td_Sale_Pay));
//                if (!br.Success)
//                {
//                    return Json(br);
//                }

//                switch (model.flag_pay)
//                {
//                    case "onlink":      // 线上付款
//                        break;
//                    case "platform":    // 平台付款
//                        break;
//                    default:            // 默认线下付款
//                        model.dh_pay = "";
//                        break;
//                }
//                model.id_create = GetLoginInfo<long>("id_user");
//                model.id_edit = GetLoginInfo<long>("id_user");
//                model.flag_state = 1; // 待审核
//                br = BusinessFactory.Funds.Add(model);
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
//        /// 供应商 添加付款记录
//        /// znt 2015-04-27
//        /// </summary>
//        [ActionPurview(false)]
//        [HttpPost]
//        public ActionResult AddForGys(Td_Sale_Pay model)
//        {
//            BaseResult br = new BaseResult();
//            if (model.dh_order.IsEmpty())
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("订单号校验失败，请重新刷新页面。"));
//                br.Data = "dh_order";
//                return Json(br);
//            }
//            if (model.rq_create.Value == null)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("付款日期不能为空。"));
//                br.Data = "rq_create";
//                return Json(br);
//            }
//            try
//            {
//                br = BusinessFactory.Utilety.GetNextDH(model, typeof(Td_Sale_Pay));
//                if (!br.Success)
//                {
//                    return Json(br);
//                }

//                // 待完善
//                switch (model.flag_pay)
//                {
//                    case "onlink":      // 线上付款
//                        break;
//                    case "platform":    // 平台付款
//                        break;
//                    default:            // 默认线下付款
//                        model.dh_pay = "";
//                        break;
//                }
//                model.id_create = GetLoginInfo<long>("id_user");
//                model.id_edit = GetLoginInfo<long>("id_user");
//                br = BusinessFactory.Funds.PayForGys(model);
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

//    }
//}
