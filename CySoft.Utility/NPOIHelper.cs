using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/**/
using System.IO;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.POIFS;
using NPOI.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF;
using System.Collections;
using System.Data;
using ICSharpCode.SharpZipLib.Zip;
using System.Drawing;
using System.Drawing.Drawing2D;
using NPOI.XSSF.UserModel;
namespace CySoft.Utility
{
    public class NPOIHelper
    {

        /// <summary>
        /// 初始化导出表格样式
        /// </summary>
        /// <param name="book">Excel文件</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static ISheet CreateSheet(HSSFWorkbook book, Dictionary<string, int> param, string sheetName = "Sheet1")
        {
            //添加一个sheet
            ISheet sheet1 = book.CreateSheet(sheetName);
            IRow row = sheet1.CreateRow(0);

            //初始化样式
            ICellStyle mStyle = book.CreateCellStyle();
            mStyle.Alignment = HorizontalAlignment.Center;
            mStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont mfont = book.CreateFont();
            mfont.FontHeight = 10 * 20;
            mStyle.SetFont(mfont);

            int i = 0;
            foreach (var item in param)
            {
                //设置列宽
                sheet1.SetColumnWidth(i, item.Value * 256);
                sheet1.SetDefaultColumnStyle(i, mStyle);
                row.CreateCell(i).SetCellValue(item.Key.ToString());
                i++;
            }
            i = 0;
            sheet1.GetRow(0).Height = 28 * 20;
            return sheet1;
        }

        /// <summary>
        ///  初始化导出表格样式 --带表头
        /// </summary>
        /// <param name="book"></param>
        /// <param name="param"></param>
        /// <param name="strTitle"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static ISheet CreateSheet(HSSFWorkbook book, Dictionary<string, int> param, string strTitle, string sheetName = "Sheet1")
        {
            //添加一个sheet
            ISheet sheet1 = book.CreateSheet(sheetName);
            IRow row = sheet1.CreateRow(0);
            IRow row1 = sheet1.CreateRow(1);

            ICell cell = row.CreateCell(0);
            cell.SetCellValue(strTitle);
            ICellStyle style = book.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            IFont font = book.CreateFont();
            font.FontHeight = 16 * 20;
            style.SetFont(font);
            cell.CellStyle = style;
            var region = new CellRangeAddress(0, 0, 0, param.Count - 1);
            sheet1.AddMergedRegion(region);
            sheet1.GetRow(0).Height = 20 * 20;

            //初始化样式
            ICellStyle mStyle = book.CreateCellStyle();
            mStyle.Alignment = HorizontalAlignment.Center;
            mStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont mfont = book.CreateFont();
            mfont.FontHeight = 10 * 20;
            mStyle.SetFont(mfont);

            int i = 0;
            foreach (var item in param)
            {
                //设置列宽
                sheet1.SetColumnWidth(i, item.Value * 256);
                sheet1.SetDefaultColumnStyle(i, mStyle);
                row1.CreateCell(i).SetCellValue(item.Key.ToString());
                i++;
            }
            i = 0;
            sheet1.GetRow(1).Height = 16 * 20;
            return sheet1;
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="firstCell"></param>
        /// <param name="lastCell"></param>
        public static void MergeCell(ISheet sheet, int firstRow, int lastRow, int firstCell, int lastCell)
        {
            var region = new CellRangeAddress(firstRow, lastRow, firstCell, lastCell);
            sheet.AddMergedRegion(region);
            ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, BorderStyle.Thin, NPOI.HSSF.Util.HSSFColor.COLOR_NORMAL);
        }
        /// <summary>
        /// 根据Excel列类型获取列的值
        /// </summary>
        /// <param name="cell">Excel列</param>
        /// <returns></returns>
        public static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                return string.Empty;
                case CellType.Boolean:
                return cell.BooleanCellValue.ToString();
                case CellType.Error:
                return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                case CellType.Unknown:
                default:
                return cell.ToString();
                case CellType.String:
                return cell.StringCellValue;
                case CellType.Formula:
                try
                {
                    HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                    e.EvaluateInCell(cell);
                    return cell.ToString();
                }
                catch
                {
                    return cell.NumericCellValue.ToString();
                }
            }
        }

        public static DataTable ImportExcelFile(string filePath)
        {
            IWorkbook hssfworkbook;
            try
            {
                using (FileStream mfile = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (filePath.EndsWith(".xls") || filePath.EndsWith(".XLS"))
                    {
                        hssfworkbook = new HSSFWorkbook(mfile);
                    }
                    else
                    {
                        hssfworkbook = new XSSFWorkbook(mfile);//加入支持office2007+
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            ISheet sheet = hssfworkbook.GetSheetAt(0);
            DataTable table = new DataTable();
            IRow headerRow = sheet.GetRow(0);//第一行为标题行
            int cellCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = NPOIHelper.GetCellValue(row.GetCell(j));
                    }
                }

                table.Rows.Add(dataRow);
            }
            return table;
        }

        /// <summary>  
        /// 功能：解压zip格式的文件
        /// </summary>  
        /// <param name="zipFilePath">压缩文件路径</param>  
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>  
        /// <param name="err">出错信息</param>  
        /// <returns>解压是否成功</returns>  
        public static bool UnZipFile(string zipFilePath, string unZipDir, out List<UnZipData> list, out string err)
        {

            err = "";
            list = new List<UnZipData>();
            List<UnZipData> mlist = new List<UnZipData>();
            //img = "";
            //imgList = new List<string>();
            if (zipFilePath == string.Empty)
            {
                err = "压缩文件不能为空！";
                return false;
            }
            if (!File.Exists(zipFilePath))
            {
                err = "压缩文件不存在！";
                return false;
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹  
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDir.EndsWith("//"))
                unZipDir += "//";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);

            try
            {

                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    ZipEntry theEntry;
                    #region  解压逻辑处理
                    //List<string> list_img = new List<string>();
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);

                        var bm = string.Empty;
                        var d = directoryName.Split('\\');
                        if (d.Length > 0)
                        {
                            bm = d[d.Length - 1];
                        }
                        Directory.CreateDirectory(unZipDir + bm + "//");

                        var res = mlist.Find(u => u.Bm == bm);
                        if (res == null)
                        {
                            UnZipData model = new UnZipData();
                            model.Bm = bm;
                            mlist.Add(model);
                        }

                        if (!string.IsNullOrEmpty(fileName))
                        {
                            using (FileStream streamWriter = File.Create(unZipDir + bm + "//" + fileName))
                            {

                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                var imgName = fileName.Split('.')[0];

                                var model = mlist.Find(u => u.Bm == bm);
                                if (imgName == bm)
                                {
                                    if (model != null)
                                    {
                                        model.Img = fileName;
                                    }
                                }
                                else
                                {
                                    if (model != null && model.ImgList.Count < 21)
                                    {
                                        //list_img.Add(fileName);
                                        model.ImgList.Add(fileName);
                                    }
                                }
                            }
                        }

                    }//while 

                    #endregion

                    //
                    foreach (var d in mlist)
                    {
                        if (string.IsNullOrEmpty(d.Img) && d.ImgList.Count == 0)
                        {
                            DirectoryInfo di = new DirectoryInfo(unZipDir + d.Bm + "//");
                            di.Delete(true);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(d.Img))
                                d.Img = d.ImgList[0];
                            list.Add(d);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return false;
            }
            return true;
        }//解压结束  



        /// <summary>
        /// 初始化导出表格样式 
        /// </summary>
        /// LZ Add 2016-08-04 
        /// <param name="book">Excel文件</param>
        /// <param name="param"></param>
        /// <param name="paramComment">批注字典 注意:字典的key要与表头key一致</param>
        /// <returns></returns>
        public static ISheet CreateSheet(HSSFWorkbook book, Dictionary<string, int> param, Dictionary<string, string> paramComment, string sheetName = "Sheet1")
        {
            //添加一个sheet
            ISheet sheet1 = book.CreateSheet(sheetName);
            IRow row = sheet1.CreateRow(0);


            //初始化样式
            ICellStyle mStyle = book.CreateCellStyle();
            mStyle.Alignment = HorizontalAlignment.Center;
            mStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont mfont = book.CreateFont();
            mfont.FontHeight = 10 * 20;
            mStyle.SetFont(mfont);

            HSSFPatriarch patr = sheet1.CreateDrawingPatriarch() as HSSFPatriarch;
            NPOI.SS.UserModel.ICreationHelper facktory = book.GetCreationHelper();
            HSSFComment comment = null;
            NPOI.SS.UserModel.IClientAnchor anchor = facktory.CreateClientAnchor();

            int i = 0;
            foreach (var item in param)
            {
                //设置列宽
                sheet1.SetColumnWidth(i, item.Value * 256);
                sheet1.SetDefaultColumnStyle(i, mStyle);
                row.CreateCell(i).SetCellValue(item.Key.ToString());

                if (paramComment.ContainsKey(item.Key.ToString()))
                {
                    //设置批注
                    anchor = facktory.CreateClientAnchor();
                    anchor.Col1 = row.GetCell(i).ColumnIndex;
                    anchor.Col2 = row.GetCell(i).ColumnIndex + 1;
                    anchor.Row1 = row.RowNum;
                    anchor.Row2 = row.RowNum + 3;
                    comment = patr.CreateCellComment(anchor) as HSSFComment;
                    comment.String = new HSSFRichTextString(paramComment[item.Key.ToString()].ToString());
                    comment.Author = ("CySoft");
                    row.GetCell(i).CellComment = (comment);
                }
                i++;
            }
            i = 0;
            sheet1.GetRow(0).Height = 28 * 20;
            return sheet1;
        }

        /// <summary>
        /// Sheet中的数据转换为List集合
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IList<T> ExportToList<T>(string filePath, string[] fields) where T : class, new()
        {
            IWorkbook hssfworkbook;
            try
            {
                using (FileStream mfile = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (filePath.EndsWith(".xls"))
                    {
                        hssfworkbook = new HSSFWorkbook(mfile);
                    }
                    else
                    {
                        hssfworkbook = new XSSFWorkbook(mfile);//加入支持office2007+
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            ISheet sheet = hssfworkbook.GetSheetAt(0);
            IList<T> list = new List<T>();

            //遍历每一行数据
            for (int i = sheet.FirstRowNum + 1, len = sheet.LastRowNum + 1; i < len; i++)
            {
                T t = new T();
                IRow row = sheet.GetRow(i);

                for (int j = 0, len2 = fields.Length; j < len2; j++)
                {
                    ICell cell = row.GetCell(j);
                    object cellValue = null;

                    switch (cell.CellType)
                    {
                        case CellType.String: //文本
                        cellValue = cell.StringCellValue;
                        break;
                        case CellType.Numeric: //数值
                        cellValue = Convert.ToInt32(cell.NumericCellValue);//Double转换为int
                        break;
                        case CellType.Boolean: //bool
                        cellValue = cell.BooleanCellValue;
                        break;
                        case CellType.Blank: //空白
                        cellValue = "";
                        break;
                        default:
                        cellValue = "ERROR";
                        break;
                    }

                    typeof(T).GetProperty(fields[j]).SetValue(t, cellValue, null);
                }
                list.Add(t);
            }

            return list;
        }


        /// <summary>
        /// 保存Excel文件
        /// </summary>
        /// <param name="excelWorkBook"></param>
        /// <param name="filePath"></param>
        public static void SaveExcelFile(HSSFWorkbook excelWorkBook, string filePath)
        {
            FileStream file = null;
            try
            {
                file = new FileStream(filePath, FileMode.Create);
                excelWorkBook.Write(file);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
        }
        /// <summary>
        /// 保存Excel文件
        /// </summary>
        /// <param name="excelWorkBook"></param>
        /// <param name="filePath"></param>
        public static void SaveExcelFile(HSSFWorkbook excelWorkBook, Stream excelStream)
        {
            try
            {
                excelWorkBook.Write(excelStream);
            }
            finally
            {

            }
        }


    }
    /// <summary>
    ///解压类数据 
    /// </summary>
    public class UnZipData
    {
        /// <summary>
        /// 商品编码
        /// </summary>
        public string Bm { get; set; }

        /// <summary>
        /// 主图
        /// </summary>
        public string Img { get; set; }

        /// <summary>
        /// 图册
        /// </summary>
        public List<string> ImgList = new List<string>();

    }

}
