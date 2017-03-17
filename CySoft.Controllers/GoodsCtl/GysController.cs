using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using CySoft.Utility.Mvc.Html;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using CySoft.Utility;
using System.IO;
using System.Data;

namespace CySoft.Controllers.GoodsCtl
{
    [LoginActionFilter]
    public class GysController : BaseController
    {

        #region 供应商-查询
        /// <summary>
        /// 供应商-查询
        /// lz 2016-09-08
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult List()
        {
            Hashtable param = base.GetParameters();
            try
            {
                #region 获取参数
                int limit = base.PageSizeFromCookie;
                param.Add("id_masteruser", id_user_master);
                ParamVessel pv = new ParamVessel();
                pv.Add("_search_", "0", HandleType.DefaultValue);
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                pv.Add("keyword", "", HandleType.Remove, true);
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("sort", "rq_create desc", HandleType.DefaultValue);
                pv.Add("flag_state", String.Empty, HandleType.Remove);
                pv.Add("id_gysfl", String.Empty, HandleType.Remove);
                param = param.Trim(pv);
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);
                #endregion
                #region 获取数据

                if (param.ContainsKey("id_gysfl") && param["id_gysfl"].ToString() == "0")
                    param.Remove("id_gysfl");


                PageNavigate pn = new PageNavigate();
                pn = BusinessFactory.Tb_Gys.GetPage(param);

                var plist = new PageList<Tb_Gys>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"]; //排序规则需要返回前台  

                var stateBr = base.GetFlagList(Enums.TsFlagListCode.gysstate.ToString());
                if (stateBr.Success)
                    ViewBag.selectListState = stateBr.Data;

                #endregion
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            if (param["_search_"].ToString().Equals("1"))
                return PartialView("_List");
            else
                return View();
        }
        #endregion

        #region 供应商-新增
        /// <summary>
        /// 供应商-新增
        /// lz 2016-09-08
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Add()
        {
            ViewData["option"] = "add";
            var stateBr = base.GetFlagList(Enums.TsFlagListCode.gysstate.ToString());
            if (stateBr.Success)
                ViewBag.selectListState = stateBr.Data;

            var gysflBr = base.GetGYSFLJsonStr();
            if (gysflBr.Success)
                ViewBag.selectListGYSFLJsonStr = gysflBr.Data.ToString();

            return View("_Edit");
        }
        #endregion

        #region 供应商-Post新增
        /// <summary>
        /// 供应商-Post新增
        /// lz 2016-09-08
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult Add(Tb_Gys model)
        {
            #region 获取参数
            var oldParam = new Hashtable();
            Hashtable param = base.GetParameters();
            param.Add("id_masteruser", id_user_master);
            Hashtable param_model = null;
            BaseResult br = new BaseResult();
            Tb_Gys model_gys = new Tb_Gys();
            #endregion

            #region 执行操作
            try
            {
                ParamVessel pv = new ParamVessel();
                pv.Add("mc", string.Empty, HandleType.ReturnMsg);
                pv.Add("bm", string.Empty, HandleType.DefaultValue);
                pv.Add("flag_state", string.Empty, HandleType.ReturnMsg);
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                if (TryUpdateModel(model_gys))
                {
                    model_gys.mc = param_model["mc"].ToString();
                    model_gys.bm = param_model["bm"].ToString();
                    model_gys.flag_state = byte.Parse(param_model["flag_state"].ToString());

                    model_gys.id_masteruser = id_user_master;
                    model.id_create = id_user;
                    br = BusinessFactory.Tb_Gys.Add(model_gys);
                }
                else
                {
                    br.Message.Add("参数有误!");
                }
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            #endregion
            WriteDBLog("供应商-新增", oldParam, br);
            return JsonString(br, 1);
        }

        #endregion

        #region 供应商-编辑
        /// <summary>
        /// 供应商-编辑
        /// lz 2016-09-08
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Edit()
        {
            try
            {
                Hashtable param = base.GetParameters();
                param.Add("id_masteruser", id_user_master);
                ParamVessel p = new ParamVessel();
                p.Add("id_masteruser", id_user_master, HandleType.DefaultValue);
                p.Add("id", String.Empty, HandleType.ReturnMsg);
                ViewData["item_edit"] = BusinessFactory.Tb_Gys.Get(param).Data;
                ViewData["option"] = "edit";

                var stateBr = base.GetFlagList(Enums.TsFlagListCode.gysstate.ToString());
                if (stateBr.Success)
                    ViewBag.selectListState = stateBr.Data;

                var gysflBr = base.GetGYSFLJsonStr();
                if (gysflBr.Success)
                    ViewBag.selectListGYSFLJsonStr = gysflBr.Data.ToString();

                return View("_Edit");
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        }
        #endregion

        #region 供应商-Post编辑
        /// <summary>
        /// 供应商-Post编辑
        /// lz 2016-09-08
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Edit(Tb_Gys model)
        {
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("mc", string.Empty, HandleType.ReturnMsg);
            pv.Add("bm", string.Empty, HandleType.DefaultValue);
            pv.Add("flag_state", string.Empty, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            try
            {
                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                model.id_masteruser = id_user_master;
                model.id_edit = id_user;
                br = BusinessFactory.Tb_Gys.Update(model);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("供应商-编辑", oldParam, br);
            return JsonString(br, 1);
        }
        #endregion

        #region 供应商-删除
        /// <summary>
        /// 供应商-删除
        /// lz 2016-09-08
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult Delete()
        {
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            param.Add("id_masteruser", id_user_master);
            pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            try
            {
                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                br = BusinessFactory.Tb_Gys.Delete(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("供应商-删除", oldParam, br);
            return JsonString(br, 1);
        }
        #endregion

        #region 供应商-选择查询
        /// <summary>
        /// 供应商-选择查询
        /// lz 2016-09-09
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult Search()
        {
            Hashtable param = base.GetParameters();
            try
            {
                #region 获取参数
                int limit = base.PageSizeFromCookie;
                param.Add("id_masteruser", id_user_master);
                ParamVessel pv = new ParamVessel();
                pv.Add("_search_", "0", HandleType.DefaultValue);
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                pv.Add("keyword", "", HandleType.Remove, true);
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("sort", "rq_create desc", HandleType.DefaultValue);
                pv.Add("flag_state", String.Empty, HandleType.Remove);
                pv.Add("_search_dropdown_", "0", HandleType.DefaultValue);
                pv.Add("gys_callback", "", HandleType.Remove);
                param = param.Trim(pv);
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);

                param.Add("flag_state", 1);

                #endregion
                #region 获取数据
                PageNavigate pn = new PageNavigate();
                pn = BusinessFactory.Tb_Gys.GetPage(param);

                var plist = new PageList<Tb_Gys>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"]; //排序规则需要返回前台  

                if (param.ContainsKey("gys_callback") && !string.IsNullOrEmpty(param["gys_callback"].ToString()))
                    ViewData["gys_callback"] = param["gys_callback"].ToString();



                var stateBr = base.GetFlagList(Enums.TsFlagListCode.gysstate.ToString());
                if (stateBr.Success)
                    ViewBag.selectListState = stateBr.Data;

                #endregion
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            if (param["_search_"].ToString().Equals("1"))
                return PartialView("_Search");
            else if (param["_search_dropdown_"].ToString().Equals("1"))
                return PartialView("_Search_DropDown");
            else
                return View();
        }
        #endregion

        #region 供应商详细
        /// <summary>
        /// 供应商详细
        /// lz
        /// 2016-10-12
        /// </summary>
        [ActionPurview(false)]
        public ActionResult Detail()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                param.Add("id_masteruser", id_user_master);
                ParamVessel p = new ParamVessel();
                p.Add("id_masteruser", id_user_master, HandleType.DefaultValue);
                p.Add("id", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                ViewData["item_edit"] = BusinessFactory.Tb_Gys.Get(param).Data;

                var stateBr = base.GetFlagList(Enums.TsFlagListCode.gysstate.ToString());
                if (stateBr.Success)
                    ViewBag.selectListState = stateBr.Data;

                var gysflBr = base.GetGYSFLJsonStr();
                if (gysflBr.Success)
                    ViewBag.selectListGYSFLJsonStr = gysflBr.Data.ToString();

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
        #endregion

        #region 供应商导出模板
        /// <summary>
        /// 供应商导出模板
        /// lz
        /// 2016-12-19
        /// </summary>
        [ActionPurview(false)]
        public FileResult DownloadExcelTemp()
        {
            try
            {
                HSSFWorkbook book = new HSSFWorkbook();
                Dictionary<string, int> e_param = new Dictionary<string, int> {
                   {"供应商分类",20},
                   {"编码",20},
                   {"名称",40},
                   {"状态",20},
                   {"联系人",25},
                   {"联系电话",25},
                   {"公司电话",25},
                   {"邮箱",80},
                   {"邮编",20},
                   {"地址",240},
                   {"备注",160}
                };

                Dictionary<string, string> e_param_comment = new Dictionary<string, string> {
                     {"供应商分类","类别规则: \r\n1.不能为空 \r\n2.长度不能超过60 \r\n3.系统不存在的类别会自行添加到第一级"},
                     {"编码","编码规则：\r\n1.可为空 \r\n2.不能有\"+*%/\"等特殊字符，不能以\"_\"或\"-\"开头 \r\n3.长度不能超过16 "},
                     {"名称","名称规则:\r\n1.不可为空 \r\n2.长度不能超过100 "},
                     {"状态","状态规则: \r\n1.只能填写0-1之间的数字  0:停用  1:正常 "},
                     {"联系人","名称规则:\r\n1.可为空 \r\n2.长度不能超过30 "},
                     {"联系电话","联系电话规则:\r\n1.可为空 \r\n2.长度不能超过80 "},
                     {"公司电话","公司电话规则:\r\n1.可为空 \r\n2.长度不能超过30 "},
                     {"邮箱","邮箱规则:\r\n1.可为空 \r\n2.长度不能超过100 "},
                     {"邮编","邮编规则:\r\n1.可为空 \r\n2.长度不能超过8 "},
                     {"地址","地址规则:\r\n1.可为空 \r\n2.长度不能超过300 "},
                     {"备注","备注规则:\r\n1.可为空 \r\n2.长度不能超过200 "}
                };

                ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "供应商资料模版");

                IRow rowtemp = sheet1.CreateRow(1);
                rowtemp.CreateCell(0).SetCellValue("小吃供应商");
                rowtemp.CreateCell(1).SetCellValue("01");
                rowtemp.CreateCell(2).SetCellValue("小刘");
                rowtemp.CreateCell(3).SetCellValue("1");
                rowtemp.CreateCell(4).SetCellValue("刘龙");
                rowtemp.CreateCell(5).SetCellValue("18924308070");
                rowtemp.CreateCell(6).SetCellValue("020-11111111");
                rowtemp.CreateCell(7).SetCellValue("37992810@qq.com");
                rowtemp.CreateCell(8).SetCellValue("5311111");
                rowtemp.CreateCell(9).SetCellValue("广东省天河区五山路248号金山大厦南塔303");
                rowtemp.CreateCell(10).SetCellValue("广州全区负责供货");
                sheet1.GetRow(1).Height = 28 * 20;

                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "供应商资料模版-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");

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
        #endregion

        #region 批量导入供应商
        /// <summary>
        /// 批量导入供应商 UI 
        /// lz
        /// 2016-12-19
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult ImportIn()
        {
            return View();
        }

        /// <summary>
        /// 批量导入供应商
        /// lz
        /// 2016-12-19
        /// </summary>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult ImportIn(string filePath)
        {
            var oldParam = new Hashtable();
            oldParam.Add("filePath", filePath);
            var savePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\" + filePath;
            DataTable table = NPOIHelper.ImportExcelFile(savePath);
            BaseResult br = new BaseResult() { Level = ErrorLevel.Alert };
            List<Tb_Gys_Import> list = new List<Tb_Gys_Import>();
            List<Tb_Gys_Import> successList = new List<Tb_Gys_Import>();
            List<Tb_Gys_Import> failList = new List<Tb_Gys_Import>();
            try
            {
                if (!table.Columns.Contains("系统备注"))
                    table.Columns.Add("系统备注", typeof(System.String));
                if (!table.Columns.Contains("id"))
                    table.Columns.Add("id", typeof(System.String));
                list = this.TurnGysImportList(table);
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Data = "数据格式有误，请重新下载导入模版，再导入";
                br.Message.Add(" 数据格式有误，请重新下载导入模版，再导入 ");
                WriteDBLog("供应商-导入", oldParam, br);
                return Json(br);
            }

            ProccessData(filePath, list, ref successList, ref failList);

            if (failList != null && failList.Count() > 0)
            {
                br.Success = true;
                string failFilePath = SaveFailFile(failList);
                br.Data = failFilePath;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(" 共" + table.Rows.Count + "条 成功" + successList.Count() + "条 失败" + failList.Count() + "条");
                WriteDBLog("供应商-导入", oldParam, br);
                return Json(br);
            }
            else
            {
                br.Success = true;
                br.Message.Clear();
                br.Message.Add(" 导入成功 ");
                WriteDBLog("供应商-导入", oldParam, br);
                return Json(br);
            }
        }
        #endregion

        #region TurnGysImportList
        private List<Tb_Gys_Import> TurnGysImportList(DataTable table)
        {
            List<Tb_Gys_Import> list = new List<Tb_Gys_Import>();

            foreach (DataRow item in table.Rows)
            {
                Tb_Gys_Import model = new Tb_Gys_Import();
                model.id_gysfl = item["供应商分类"] == null ? "" : item["供应商分类"].ToString();
                model.bm = item["编码"] == null ? "" : item["编码"].ToString();
                model.mc = item["名称"] == null ? "" : item["名称"].ToString();
                decimal flag_state = 0;
                decimal.TryParse(item["状态"] == null ? "" : item["状态"].ToString(), out flag_state);
                model.flag_state = flag_state.ToString();
                model.lxr = item["联系人"] == null ? "" : item["联系人"].ToString();
                model.tel = item["联系电话"] == null ? "" : item["联系电话"].ToString();
                model.companytel = item["公司电话"] == null ? "" : item["公司电话"].ToString();
                model.email = item["邮箱"] == null ? "" : item["邮箱"].ToString();
                model.zipcode = item["邮编"] == null ? "" : item["邮编"].ToString();
                model.address = item["地址"] == null ? "" : item["地址"].ToString();
                model.bz = item["备注"] == null ? "" : item["备注"].ToString();
                model.bz_sys = item["系统备注"] == null ? "" : item["系统备注"].ToString();
                list.Add(model);
            }
            return list;
        }
        #endregion

        #region ProccessData
        /// <summary>
        /// 商品数据处理
        /// </summary>
        /// <param name="list"></param>
        /// <param name="successList"></param>
        /// <param name="failList"></param>
        private void ProccessData(string filePath, List<Tb_Gys_Import> list, ref List<Tb_Gys_Import> successList, ref List<Tb_Gys_Import> failList)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            param.Add("filePath", filePath);
            param.Add("list", list);
            param.Add("id_masteruser", id_user_master);
            param.Add("id_user", id_user);
            param.Add("id_shop", id_shop);
            param.Add("digitHashtable", GetParm());//小数点)
            var shopShop = GetShop(Enums.ShopDataType.ShopShopAll);
            param.Add("shop_shop", shopShop);
            br = BusinessFactory.Tb_Gys.Init(param);
            if (br.Success)
            {
                Tb_Gys_Import_All rModel = (Tb_Gys_Import_All)br.Data;
                successList = rModel.SuccessList;
                failList = rModel.FailList;
            }
            else
            {
                failList = list;
                foreach (var item in failList)
                    item.bz = br.Message[0].ToString();
            }
        }
        #endregion

        #region 保存本地临时文件
        /// <summary>
        /// 保存本地临时文件
        /// </summary>
        /// <param name="failList"></param>
        /// <returns></returns>
        private string SaveFailFile(List<Tb_Gys_Import> failList)
        {
            try
            {
                string fileName = "供应商导入失败数据" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                string url = "/UpLoad/Temp/" + fileName;
                string fileFullName = ApplicationInfo.TempPath + "\\" + fileName;

                HSSFWorkbook book = new HSSFWorkbook();

                Dictionary<string, int> e_param = new Dictionary<string, int> {
                   {"系统备注",80},
                   {"供应商分类",20},
                   {"编码",20},
                   {"名称",40},
                   {"状态",20},
                   {"联系人",25},
                   {"联系电话",25},
                   {"公司电话",25},
                   {"邮箱",80},
                   {"邮编",20},
                   {"地址",240},
                   {"备注",160}

                };

                Dictionary<string, string> e_param_comment = new Dictionary<string, string> {
                    {"系统备注","导入失败系统描述 "},
                     {"供应商分类","类别规则: \r\n1.不能为空 \r\n2.长度不能超过60 \r\n3.系统不存在的类别会自行添加到第一级"},
                     {"编码","编码规则：\r\n1.可为空 \r\n2.不能有\"+*%/\"等特殊字符，不能以\"_\"或\"-\"开头 \r\n3.长度不能超过16 "},
                     {"名称","名称规则:\r\n1.不可为空 \r\n2.长度不能超过100 "},
                     {"状态","状态规则: \r\n1.只能填写0-1之间的数字  0:停用  1:正常 "},
                     {"联系人","名称规则:\r\n1.可为空 \r\n2.长度不能超过30 "},
                     {"联系电话","联系电话规则:\r\n1.可为空 \r\n2.长度不能超过80 "},
                     {"公司电话","公司电话规则:\r\n1.可为空 \r\n2.长度不能超过30 "},
                     {"邮箱","邮箱规则:\r\n1.可为空 \r\n2.长度不能超过100 "},
                     {"邮编","邮编规则:\r\n1.可为空 \r\n2.长度不能超过8 "},
                     {"地址","地址规则:\r\n1.可为空 \r\n2.长度不能超过300 "},
                     {"备注","备注规则:\r\n1.可为空 \r\n2.长度不能超过200 "}
                };




                ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "供应商资料模版");
                int i = 1;
                foreach (var item in failList)
                {
                    IRow rowtemp = sheet1.CreateRow(i);
                    rowtemp.CreateCell(0).SetCellValue(item.bz_sys);
                    rowtemp.CreateCell(1).SetCellValue(item.id_gysfl);
                    rowtemp.CreateCell(2).SetCellValue(item.bm);
                    rowtemp.CreateCell(3).SetCellValue(item.mc);
                    rowtemp.CreateCell(4).SetCellValue(item.flag_state);
                    rowtemp.CreateCell(5).SetCellValue(item.lxr);
                    rowtemp.CreateCell(6).SetCellValue(item.tel);
                    rowtemp.CreateCell(7).SetCellValue(item.companytel);
                    rowtemp.CreateCell(8).SetCellValue(item.email);
                    rowtemp.CreateCell(9).SetCellValue(item.zipcode);
                    rowtemp.CreateCell(10).SetCellValue(item.address);
                    rowtemp.CreateCell(11).SetCellValue(item.bz);
                    
                    sheet1.GetRow(i).Height = 28 * 20;
                    i++;
                }

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);

                NPOIHelper.SaveExcelFile(book, fileFullName);

                return url;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

        #region GetGysApi
        [ActionPurview(false)]
        public ActionResult GetGysApi()
        {
            //用户管理供应商
            var gys = GetUserGys();
            return JsonString(gys);
        }
        #endregion




    }
}
