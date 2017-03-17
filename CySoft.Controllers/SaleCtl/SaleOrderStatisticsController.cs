using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Filters;
using CySoft.Controllers.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Td;
using CySoft.Model.Flags;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using CySoft.Utility;
using NPOI.HSSF.UserModel;

#region 销售订单统计
#endregion

namespace CySoft.Controllers.SaleCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class SaleOrderStatisticsController : BaseController
    {
        public ActionResult List()
        {
            BaseResult br = new BaseResult();
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Td_Sale_Order_Body_Query> list = new PageList<Td_Sale_Order_Body_Query>(limit);
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("dh", string.Empty, HandleType.Remove);
                p.Add("alias_cgs", string.Empty, HandleType.Remove);
                p.Add("start_rq_create", string.Empty, HandleType.Remove);
                p.Add("end_rq_create",string.Empty,HandleType.Remove);
                p.Add("flag_state_List", string.Empty, HandleType.Remove);
                p.Add("sp_mc", string.Empty, HandleType.Remove,true);
                p.Add("alias_cgs", string.Empty, HandleType.Remove,true);
                p.Add("level", (long)0, HandleType.Remove);//客户级别Id
                p.Add("id_spfl", string.Empty, HandleType.Remove);
                p.Add("flag_search", string.Empty, HandleType.Remove);
                param = param.Trim(p);
                if (param.Contains("flag_state_List"))
                {
                    ViewData["flag_state_List"] = param["flag_state_List"].ToString();
                    string[] flag_stateListStr = param["flag_state_List"].ToString().Split(',');
                    int[] flag_stateListInt = Array.ConvertAll<string, int>(flag_stateListStr, s => Convert.ToInt32(s));
                    param["flag_state_List"] = flag_stateListInt;
                }
                if (param["start_rq_create"] != null)
                {
                    ViewData["start_rq_create"] = param["start_rq_create"];
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    ViewData["start_rq_create"] = Convert.ToDateTime(dt.AddDays(1 - (dt.Day))).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    param["start_rq_create"] = ViewData["start_rq_create"];
                }
                if (param["end_rq_create"] != null)
                {
                    ViewData["end_rq_create"] = param["end_rq_create"];
                }
                else
                {
                    ViewData["end_rq_create"] = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    param["end_rq_create"] = ViewData["end_rq_create"];
                }
                if (param.ContainsKey("level"))
                {
                    ViewData["level"] = param["level"];
                    param.Add("id_cgs_level", param["level"]);
                    param.Remove("level");
                }
                if (param.Contains("alias_cgs")) {
                    ViewData["alias_cgs"] = param["alias_cgs"];
                }
                if (param.Contains("sp_mc")) {
                    ViewData["sp_mc"] = param["sp_mc"];
                }
                if (param.Contains("start_rq_create")) {
                    ViewData["start_rq_create"] = param["start_rq_create"];
                }
                if (param.Contains("end_rq_create"))
                {
                    ViewData["end_rq_create"] = param["end_rq_create"];
                }
                if (param.Contains("id_spfl")) {
                    ViewData["id_spfl"] = param["id_spfl"];
                }
                param.Add("not_flag_state", 0);
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                br = BusinessFactory.SaleOrder.GetStatisticsInfo(param);
                ViewData["Collect"] = br.Data;
                ViewData["OrderCount"] = pn.TotalCount;
                ViewData["pageIndex"] = pageIndex;
                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                if (param["flag_search"]!=null)
                {
                    pn = BusinessFactory.SaleOrder.GetStatistics(param);
                }
                else {
                    pn.Data = new List<Td_Sale_Order_Body_Query>();
                    pn.Success = true;
                }
                list = new PageList<Td_Sale_Order_Body_Query>(pn, pageIndex, limit);
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

        /// <summary>
        /// 订单商品统计导出 xdh
        /// </summary>
        /// <returns></returns>
        public ActionResult Export()
        {
            BaseResult br = new BaseResult();
            PageNavigate pn = new PageNavigate();
            int limit = 50000;
            PageList<Td_Sale_Order_Body_Query> list = new PageList<Td_Sale_Order_Body_Query>(limit);
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("start_rq_create", string.Empty, HandleType.Remove);
                p.Add("end_rq_create", string.Empty, HandleType.Remove);
                p.Add("flag_search", string.Empty, HandleType.Remove);
                param = param.Trim(p);
                param.Add("not_flag_state", 0);
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                br = BusinessFactory.SaleOrder.GetStatisticsInfo(param);
                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                if (param["flag_search"] != null)
                {
                    pn = BusinessFactory.SaleOrder.GetStatistics(param);
                }
                list = new PageList<Td_Sale_Order_Body_Query>(pn, pageIndex, limit);

                #region 导出订单商品统计
                HSSFWorkbook book = new HSSFWorkbook();
                Dictionary<string, int> e_param = new Dictionary<string, int> { 
                   {"客户名称",18},
                   {"订单号",18},
                   {"订单状态",18},
                   {"下单日期",18},
                   {"商品编码",18},
                   {"商品名称",22},
                   {"数量",14},
                   {"单价",18},
                   {"小计金额",18}
                };
                ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param);

                ICellStyle lstyle = book.CreateCellStyle();
                lstyle.Alignment = HorizontalAlignment.Left;
                lstyle.VerticalAlignment = VerticalAlignment.Center;

                ICellStyle rstyle = book.CreateCellStyle();
                rstyle.Alignment = HorizontalAlignment.Right;
                rstyle.VerticalAlignment = VerticalAlignment.Center;
               
                for (int i = 0; i < list.Count; i++)
                {
                    IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(list[i].alias_cgs);
                    rowtemp.GetCell(0).CellStyle = lstyle;
                    rowtemp.CreateCell(1).SetCellValue(list[i].dh);
                    rowtemp.GetCell(1).CellStyle = lstyle;
                    rowtemp.CreateCell(2).SetCellValue(list[i].mc_flag_state);
                    rowtemp.GetCell(2).CellStyle = lstyle;
                    rowtemp.CreateCell(3).SetCellValue(list[i].rq_create.Value.ToString("yyyy-MM-dd HH:mm"));
                    rowtemp.GetCell(3).CellStyle = lstyle;
                    rowtemp.CreateCell(4).SetCellValue(list[i].GoodsBm);
                    rowtemp.GetCell(4).CellStyle = lstyle;
                    rowtemp.CreateCell(5).SetCellValue(list[i].GoodsName);
                    rowtemp.GetCell(5).CellStyle = lstyle;
                    rowtemp.CreateCell(6).SetCellValue(Decimal.Round((decimal)list[i].sl,2).ToString());
                    rowtemp.GetCell(6).CellStyle = rstyle;
                    rowtemp.CreateCell(7).SetCellValue(Decimal.Round((decimal)list[i].dj_hs, 2).ToString());
                    rowtemp.GetCell(7).CellStyle = rstyle;
                    rowtemp.CreateCell(8).SetCellValue(Decimal.Round((decimal)list[i].je_pay,2).ToString());
                    rowtemp.GetCell(8).CellStyle = rstyle;
                }
                sheet1.CreateFreezePane(0, 2);
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "订单商品统计报表_" + Convert.ToDateTime(param["start_rq_create"]).ToString("yyyyMMdd") + "-" + Convert.ToDateTime(param["end_rq_create"]).ToString("yyyyMMdd") + ".xls");
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
            return Content("导出");
        }
    }
}
