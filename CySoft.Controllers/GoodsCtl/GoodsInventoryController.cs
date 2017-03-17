using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Utility.Mvc.Html;
/**/
using System.Collections.Generic;
using CySoft.Utility;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using System.Data;

#region 库存管理
#endregion

namespace CySoft.Controllers.GoodsCtl
{
    [LoginActionFilter]
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class GoodsInventoryController : BaseController
    {
        /// <summary>
        /// 库存列表
        /// znt 2015-03-13
        /// 2015-04-09 lxt 改
        /// </summary>
        public ActionResult List()
        {

            PageNavigate pn = new PageNavigate();
            int limit = 15;
            PageList<SkuData> list = new PageList<SkuData>(limit);
            try
            {
                BaseResult br = new BaseResult();
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("orderby", 8, HandleType.DefaultValue);//排序
                p.Add("up", 0, HandleType.Remove);//是否上架
                p.Add("typeid", (long)0, HandleType.Remove);//商品类别 Id
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                param = param.Trim(p);

                switch (param["orderby"].ToString())
                {
                    case "3":
                        param.Add("sort", "db.dj_base");
                        param.Add("dir", "asc");
                        break;
                    case "4":
                        param.Add("sort", "db.dj_base");
                        param.Add("dir", "desc");
                        break;
                    case "5":
                        param.Add("sort", "db.rq_edit");
                        param.Add("dir", "asc");
                        break;
                    case "6":
                        param.Add("sort", "db.rq_edit");
                        param.Add("dir", "desc");
                        break;
                    case "7":
                        param.Add("sort", "db.rq_create");
                        param.Add("dir", "asc");
                        break;
                    case "8":
                        param.Add("sort", "db.rq_create");
                        param.Add("dir", "desc");
                        break;
                    case "9":
                        param.Add("sort", "db.sl_kc");
                        param.Add("dir","asc");
                        break;
                    case "10":
                        param.Add("sort", "db.sl_kc");
                        param.Add("dir","desc");
                        break;
                    default:
                        param["orderby"] = 8;
                        param.Add("sort", "db.rq_create");
                        param.Add("dir", "desc");
                        break;
                }

                ViewData["orderby"] = param["orderby"];
                param.Remove("orderby");

                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                ViewData["pageIndex"] = pageIndex;
                if (param.ContainsKey("up"))
                {
                    ViewData["up"] = param["up"];
                    param.Add("flag_up", param["up"]);
                    param.Remove("up");
                }
                if (param.ContainsKey("typeid"))
                {
                    ViewData["typeid"] = param["typeid"];
                    param.Add("id_spfl", param["typeid"]);
                    param.Remove("typeid");
                }

                ViewData["keyword"] = GetParameter("keyword");
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                pn = BusinessFactory.Goods.GetPage(param);
                list = new PageList<SkuData>(pn, pageIndex, limit);
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
        ///  修改
        ///  znt 2015-03-13
        /// </summary>
        [HttpPost]
        public ActionResult Update()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            try
            {
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));

                br=BusinessFactory.GoodsInventory.Update(param); 
               
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
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
        /// 导出
        /// </summary>
        public FileResult Export()
        {
            PageNavigate pn = new PageNavigate();
            int limit = 5000;
            PageList<SkuData> list = new PageList<SkuData>(limit);
            try
            {
                BaseResult br = new BaseResult();
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("orderby", 2, HandleType.DefaultValue);//排序
                p.Add("up", 0, HandleType.Remove);//是否上架
                p.Add("typeid", (long)0, HandleType.Remove);//商品类别 Id
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                param = param.Trim(p);

                switch (param["orderby"].ToString())
                {
                    case "3":
                        param.Add("sort", "db.dj_base");
                        param.Add("dir", "asc");
                        break;
                    case "4":
                        param.Add("sort", "db.dj_base");
                        param.Add("dir", "desc");
                        break;
                    case "5":
                        param.Add("sort", "db.rq_edit");
                        param.Add("dir", "asc");
                        break;
                    case "6":
                        param.Add("sort", "db.rq_edit");
                        param.Add("dir", "desc");
                        break;
                    case "7":
                        param.Add("sort", "db.rq_create");
                        param.Add("dir", "asc");
                        break;
                    case "8":
                        param.Add("sort", "db.rq_create");
                        param.Add("dir", "desc");
                        break;
                    case "9":
                        param.Add("sort", "db.sl_kc");
                        param.Add("dir", "asc");
                        break;
                    case "10":
                        param.Add("sort", "db.sl_kc");
                        param.Add("dir", "desc");
                        break;
                    default:
                        param["orderby"] = 2;
                        param.Add("sort", "db.sort_id");
                        param.Add("dir", "desc");
                        break;
                }
                param.Remove("orderby");
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                if (param.ContainsKey("up"))
                {
                    param.Add("flag_up", param["up"]);
                    param.Remove("up");
                }
                if (param.ContainsKey("typeid"))
                {
                    param.Add("id_spfl", param["typeid"]);
                    param.Remove("typeid");
                }
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                pn = BusinessFactory.Goods.GetPage(param);
                list = new PageList<SkuData>(pn, pageIndex, limit);
                #region 导出库存
                HSSFWorkbook book = new HSSFWorkbook();
                Dictionary<string, int> e_param = new Dictionary<string, int> { 
                   {"序号",12},
                   {"类别",18},
                   {"商品编码",18},
                   {"商品名称",18},
                   {"库存",18},
                   {"预警数量",18},
                   {"单位",18}
                };
                ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param,"商品库存");
                ICellStyle lstyle = book.CreateCellStyle();
                lstyle.Alignment = HorizontalAlignment.Left;
                lstyle.VerticalAlignment = VerticalAlignment.Center;

                ICellStyle rstyle = book.CreateCellStyle();
                rstyle.Alignment = HorizontalAlignment.Right;
                rstyle.VerticalAlignment = VerticalAlignment.Center;
                for (int i = 0; i < list.Count; i++)
                {
                    IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(i + 1);
                    rowtemp.GetCell(0).CellStyle = rstyle;
                    rowtemp.CreateCell(1).SetCellValue(list[i].name_fl);
                    rowtemp.GetCell(1).CellStyle = lstyle;
                    rowtemp.CreateCell(2).SetCellValue(list[i].bm);
                    rowtemp.CreateCell(3).SetCellValue(list[i].name_sp);
                    rowtemp.GetCell(3).CellStyle = lstyle;
                    rowtemp.CreateCell(4).SetCellValue(Decimal.Round((decimal)list[i].sl_kc, 2).ToString());
                    rowtemp.GetCell(4).CellStyle = rstyle;
                    rowtemp.CreateCell(5).SetCellValue(Decimal.Round((decimal)list[i].sl_kc_bj, 2).ToString());
                    rowtemp.GetCell(5).CellStyle = rstyle;
                    rowtemp.CreateCell(6).SetCellValue(list[i].unit);
                    sheet1.GetRow(i + 1).Height = 16 * 20;
                   
                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "商品库存_" +DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
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

           
            return null;
        }
        [ActionPurview(false)]
        public FileResult ExportAll()
        {
            List<SkuData> list = new List<SkuData>();
            try
            {
                Hashtable param = new Hashtable();
                BaseResult br = new BaseResult();
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                list = BusinessFactory.Goods.GetExportAll(param);
                HSSFWorkbook book = new HSSFWorkbook();
                Dictionary<string, int> e_param = new Dictionary<string, int> { 
                   {"序号",12},
                   {"类别",18},
                   {"SKU编码",18},
                   {"商品编码",18},
                   {"商品名称",18},
                   {"规格",30},
                   {"库存",18},
                   {"预警数量",18},
                   {"单位",18}
                };
                ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, "商品库存");
                ICellStyle lstyle = book.CreateCellStyle();
                lstyle.Alignment = HorizontalAlignment.Left;
                lstyle.VerticalAlignment = VerticalAlignment.Center;
                lstyle.IsLocked = true;

                ICellStyle rstyle = book.CreateCellStyle();
                rstyle.Alignment = HorizontalAlignment.Right;
                rstyle.VerticalAlignment = VerticalAlignment.Center;

                ICellStyle locked= book.CreateCellStyle();
                locked.IsLocked = true;
                for (int i = 0; i < list.Count; i++)
                {
                    IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(i + 1);
                    rowtemp.GetCell(0).CellStyle = rstyle;
                    rowtemp.CreateCell(1).SetCellValue(list[i].name_fl);
                    rowtemp.GetCell(1).CellStyle = lstyle;
                    rowtemp.CreateCell(2).SetCellValue(list[i].id.ToString());
                    rowtemp.GetCell(2).CellStyle.IsLocked = true;
                    rowtemp.CreateCell(3).SetCellValue(list[i].bm);
                    rowtemp.GetCell(3).CellStyle.IsLocked = true;
                    rowtemp.CreateCell(4).SetCellValue(list[i].name_sp);
                    rowtemp.GetCell(4).CellStyle = lstyle;
                    rowtemp.CreateCell(5).SetCellValue(list[i].name_spec_zh);
                    rowtemp.CreateCell(6).SetCellValue(Decimal.Round((decimal)list[i].sl_kc, 2).ToString());
                    rowtemp.GetCell(6).CellStyle = rstyle;
                    rowtemp.CreateCell(7).SetCellValue(Decimal.Round((decimal)list[i].sl_kc_bj, 2).ToString());
                    rowtemp.GetCell(7).CellStyle = rstyle;
                    rowtemp.CreateCell(8).SetCellValue(list[i].unit);
                    sheet1.GetRow(i + 1).Height = 16 * 20;
                }
                
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "商品库存_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        /// <summary>
        /// 导入库存
        /// </summary>
        public static DataTable table;//数据
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult Import(string filePath)
        {
            BaseResult br = new BaseResult();
            int success = 0;
            int fail = 0;
            try
            {
                ProccessData(filePath,out success,out fail,out table);
            }
            catch (Exception ex)
            {
                br.Success = true;
                br.Data = "数据格式有误，请重新下载导入模版，再导入";
                return Json(br);
            }
            br.Success = true;
            br.Data = " 共" + table.Rows.Count + "数据，成功导入" + success + "条，异常数据<span id=\"failCount\">" + fail + "</span> 条";
            return Json(br);
        }
        /// <summary>
        /// 导入库存数据处理
        /// </summary>
        /// <param name="filePath">表格文件路径</param>
        /// <param name="success">成功数</param>
        /// <param name="fail">失败数</param>
        /// <param name="table">数据</param>
        private void ProccessData(string filePath,out int success,out int fail, out DataTable table)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            success = 0;
            fail = 0;
            //读取数据
            var savePath = Server.MapPath(filePath);
            table = NPOIHelper.ImportExcelFile(savePath);
            if (!table.Columns.Contains("备注"))
                table.Columns.Add("备注", typeof(System.String));
            foreach (DataRow item in table.Rows)
            {
                string sku = item["SKU编码"].ToString();
                if (!string.IsNullOrEmpty(sku))
                {
                    param.Clear();
                    param.Add("id", sku);
                    br = BusinessFactory.GoodsInventory.CheckStock(param);
                    if (br.Success == true && br.Data != null)
                    {
                        param.Clear();
                        string sl_kc = item["库存"].ToString();
                        string sl_kc_bj = item["预警数量"].ToString();
                        param.Add("id_sku", br.Data);
                        if (!string.IsNullOrEmpty(sl_kc) && !string.IsNullOrEmpty(sl_kc_bj))
                        {
                            if (!string.IsNullOrEmpty(sl_kc_bj))
                            {
                                param.Add("sl_kc_bj", Convert.ToDouble(sl_kc_bj));
                            }
                            if (!string.IsNullOrEmpty(sl_kc))
                            {
                                param.Add("sl_kc", Convert.ToDouble(sl_kc));
                                br = BusinessFactory.GoodsInventory.Update(param);
                                success++;
                            }
                        }
                        else
                        {
                            fail++;
                            item["备注"] = "库存和预警数量都为空，至少填写一项";
                        }
                    }
                    else
                    {
                        fail++;
                        item["备注"] = "系统无法识别该商品SKU编码";
                    }
                }
                else 
                {
                    fail++;
                    item["备注"] = "SKU编码为空";
                }
            }
        }
        /// <summary>
        /// 下载导入失败数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        public FileResult DownloadFail(string filePath)
        {
            try
            {
                if (table == null || table.Rows.Count == 0)
                {
                    return null;
                }
                var dr = table.Select("备注 <> ''");
                var fileName = Server.MapPath("~/Template/stock_import.xls");
                FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                IWorkbook book = new HSSFWorkbook(file);
                ISheet sheet1 = book.GetSheet("商品库存");
                for (int i = 0; i < dr.Length; i++)
                {
                    IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(dr[i]["序号"].ToString());
                    rowtemp.CreateCell(1).SetCellValue(dr[i]["类别"].ToString());
                    rowtemp.CreateCell(2).SetCellValue(dr[i]["SKU编码"].ToString());
                    rowtemp.CreateCell(3).SetCellValue(dr[i]["商品编码"].ToString());
                    rowtemp.CreateCell(4).SetCellValue(dr[i]["商品名称"].ToString());
                    rowtemp.CreateCell(5).SetCellValue(dr[i]["规格"].ToString());
                    rowtemp.CreateCell(6).SetCellValue(dr[i]["库存"].ToString());
                    rowtemp.CreateCell(7).SetCellValue(dr[i]["预警数量"].ToString());
                    rowtemp.CreateCell(8).SetCellValue(dr[i]["单位"].ToString());
                    rowtemp.CreateCell(8).SetCellValue(dr[i]["备注"].ToString());
                }
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "商品库存失败数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
