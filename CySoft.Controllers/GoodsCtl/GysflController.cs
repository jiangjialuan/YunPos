using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using AopAlliance.Intercept;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Common;
using CySoft.Frame.Core;
using CySoft.Model;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Utility;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Spring.Objects.Factory.Attributes;
using CySoft.Model.Enums;

namespace CySoft.Controllers.GoodsCtl
{
    [LoginActionFilter]
    public class GysflController : BaseController
    {
        #region 供应商分类-查询
        /// <summary>
        /// 供应商分类-查询
        /// lz 2016-09-07
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult List()
        {
            #region 获取参数
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("id_masteruser", id_user_master, HandleType.ReturnMsg);
            pv.Add("childId", 0, HandleType.DefaultValue);
            pv.Add("sort", "sort_id", HandleType.DefaultValue);
            pv.Add("dir", "asc", HandleType.DefaultValue);
            Dictionary<string, object> user = base.UserData;
            #endregion
            #region 获取数据
            try
            {
                param["id_masteruser"] = id_user_master;        //获取用户Id
                param_model = param.Trim(pv);
                br = BusinessFactory.Tb_Gysfl.GetTree(param_model);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            if (br.Success)
            {
                List<Tb_Gysfl_Tree> list_tree = br.Data as List<Tb_Gysfl_Tree>;
                ViewData["list_tree"] = list_tree;
            }
            #endregion
            return View();
        } 
        #endregion

        #region 供应商分类-新增
        /// <summary>
        /// 供应商分类-新增
        /// lz 2016-09-07
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Add()
        {
            #region 获取参数
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("sort", "mc", HandleType.DefaultValue);
            pv.Add("dir", "asc", HandleType.DefaultValue);
            pv.Add("id_masteruser", id_user_master, HandleType.ReturnMsg);
            pv.Add("parent_id", "0", HandleType.DefaultValue);
            pv.Add("other_add", "0", HandleType.DefaultValue);
            pv.Add("childId", "0", HandleType.DefaultValue);
            Dictionary<string, object> user = base.UserData;
            string pid = string.Empty;
            string other_add = string.Empty;
            #endregion
            #region 构建数据
            try
            {
                param["id_masteruser"] = id_user_master;
                param_model = param.Trim(pv);
                pid = param_model["parent_id"].ToString().Equals("0", StringComparison.OrdinalIgnoreCase) ? string.Empty : param_model["parent_id"].ToString();
                param_model.Remove("parent_id");
                other_add = param_model["other_add"].ToString().Equals("0", StringComparison.OrdinalIgnoreCase) ? string.Empty : param_model["other_add"].ToString();
                param_model.Remove("other_add");
                br = BusinessFactory.Tb_Gysfl.GetTree(param_model);      //完整分类树
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }

            ViewData["category_id"] = pid;                           //指定节点添加子分类
            ViewData["category_pid"] = string.Empty;
            ViewData["option"] = "add";
            ViewData["other_add"] = other_add;
            if (br.Success)
            {
                List<Tb_Gysfl_Tree> list_tree = br.Data as List<Tb_Gysfl_Tree>;
                ViewData["list_tree"] = list_tree;
            }
            #endregion
            return PartialView("Edit");
        } 
        #endregion

        #region 供应商分类-Post新增
        /// <summary>
        /// 供应商分类-Post新增
        /// lz 2016-09-07
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Add(Tb_Gysfl model)
        {
            #region 获取参数
            var oldParam = new Hashtable();
            Hashtable param = base.GetParameters();
            param.Add("id_masteruser", id_user_master);
            Hashtable param_model = null;
            BaseResult br = new BaseResult();
            Tb_Gysfl model_gysfl = new Tb_Gysfl();                             //新增对象

            #endregion
            #region 执行操作

            try
            {
                ParamVessel pv = new ParamVessel();
                pv.Add("mc", string.Empty, HandleType.ReturnMsg);            //名称
                pv.Add("bm", string.Empty, HandleType.DefaultValue);            //编码
                pv.Add("id_masteruser", id_user_master, HandleType.ReturnMsg);     //用户Id
                pv.Add("parent_id", string.Empty, HandleType.ReturnMsg);     //父节点Id

                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                if (TryUpdateModel(model_gysfl))
                {
                    model_gysfl.id_farther = param_model["parent_id"].ToString();
                    model_gysfl.id_masteruser = id_user_master;
                    model_gysfl.id_create = model_gysfl.id_edit = id_user;
                    br = BusinessFactory.Tb_Gysfl.Add(model_gysfl);
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

            WriteDBLog("供应商分类-新增", oldParam, br);

            if (br.Success)
            {
                return JsonString(new
                {
                    status = "success",
                    message = "执行成功,正在载入页面...",
                    gysfl = new
                    {
                        id = model_gysfl.id,
                        is_default = "F",
                        name = model_gysfl.mc,
                        pid = model_gysfl.id_farther
                    }
                });
            }
            else
            {
                return JsonString(new
                {
                    status = "false",
                    message = br.Message[0].ToString()
                });
            }

        }
        #endregion

        #region 供应商分类-修改
        /// <summary>
        /// 供应商分类-修改
        /// lz 2016-09-07
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Edit()
        {
            #region 获取参数
            BaseResult br = new BaseResult();
            BaseResult br_gysfl = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("sort", "mc", HandleType.DefaultValue);
            pv.Add("dir", "asc", HandleType.DefaultValue);
            pv.Add("id_masteruser", id_user_master, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            pv.Add("childId", "0", HandleType.DefaultValue);
            Dictionary<string, object> user = base.UserData;

            string id = string.Empty;
            #endregion
            #region 构建数据
            try
            {
                param["id_masteruser"] = id_user_master;
                param_model = param.Trim(pv);
                id = param_model["id"].ToString();
                param_model.Remove("id");
                Hashtable param_query_item = new Hashtable();
                param_query_item.Add("id", id);
                br_gysfl = BusinessFactory.Tb_Gysfl.Get(param_query_item);
                if (!br_gysfl.Success || !(br_gysfl.Data is Tb_Gysfl))
                    throw new Exception("没有相应分类");

                br = BusinessFactory.Tb_Gysfl.GetTree(param_model);
                if (!br.Success || !(br.Data is IList))
                    throw new Exception("没有父分类");
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }

            ViewData["item_edit"] = br_gysfl.Data;
            ViewData["category_id"] = id;
            ViewData["option"] = "edit";
            ViewData["other_add"] = "0";
            if (br.Success)
            {
                List<Tb_Gysfl_Tree> list_tree = br.Data as List<Tb_Gysfl_Tree>;
                ViewData["list_tree"] = list_tree;
            }
            #endregion
            return PartialView("Edit");
        } 
        #endregion

        #region 供应商分类-Post修改
        /// <summary>
        /// 供应商分类-Post修改
        /// lz 2016-09-07
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Edit(Tb_Gysfl model)
        {
            #region 获取参数
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            param.Add("id_masteruser", id_user_master);
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("mc", string.Empty, HandleType.ReturnMsg);
            pv.Add("bm", string.Empty, HandleType.DefaultValue);
            pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
            pv.Add("category_id", string.Empty, HandleType.ReturnMsg);
            #endregion
            #region 执行操作
            try
            {
                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                br = BusinessFactory.Tb_Gysfl.Update(new Tb_Gysfl()
                {
                    id_masteruser = param_model["id_masteruser"].ToString(),
                    id = param_model["category_id"].ToString(),
                    bm = param_model["bm"].ToString(),
                    mc = param_model["mc"].ToString(),
                    id_edit = id_user
                });
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            #endregion
            WriteDBLog("供应商分类-修改", oldParam, br);
            return JsonString(br, 1);
        } 
        #endregion

        #region 供应商分类-删除
        /// <summary>
        /// 供应商分类-删除
        /// lz 2016-09-07
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult Delete()
        {
            #region 执行操作
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            param.Add("id_masteruser", id_user_master);
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("id_masteruser", id_user_master, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            try
            {
                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                br = BusinessFactory.Tb_Gysfl.Delete(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            #endregion
            WriteDBLog("供应商分类-删除", oldParam, br);
            return JsonString(br, 1);
        }
        #endregion

        #region 供应商分类-拖动保存
        /// <summary>
        /// 供应商分类-拖动保存
        /// lz 2016-09-07
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult UpdateTree()
        {
            #region 获取参数
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("order_data", string.Empty, HandleType.ReturnMsg);       //新结构 
            #endregion
            #region 执行操作
            try
            {
                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                var nodeStr = param["order_data"].ToString();
                var nodeTree = Utility.JSON.Deserialize<List<SpflUpdateTreeModel>>(nodeStr);
                if (nodeTree != null && nodeTree.Any())
                {
                    br = BusinessFactory.Tb_Gysfl.UpdateTree(nodeTree, id_user_master);
                }
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            #endregion
            WriteDBLog("供应商分类-拖动保存", oldParam, br);
            return JsonString(br, 1);
        } 
        #endregion

        #region 供应商分类-导入
        /// <summary>
        /// 供应商分类-导入
        /// lz 2016-09-07
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        [HttpGet]
        public ActionResult ImportIn()
        {
            return View();
        } 
        #endregion

        #region 供应商分类-Post导入
        /// <summary>
        /// 供应商分类-Post导入
        /// lz 2016-09-07
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult ImportIn(string filePath)
        {
            #region 获取参数
            var oldParam = new Hashtable();
            oldParam.Add("filePath", filePath);
            var savePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\" + filePath;
            DataTable table = NPOIHelper.ImportExcelFile(savePath);
            BaseResult br = new BaseResult() { Level = ErrorLevel.Alert };
            List<Tb_Gysfl_Import> list = new List<Tb_Gysfl_Import>();
            List<Tb_Gysfl_Import> successList = new List<Tb_Gysfl_Import>();
            List<Tb_Gysfl_Import> failList = new List<Tb_Gysfl_Import>();
            #endregion
            #region 执行操作
            try
            {
                if (!table.Columns.Contains("备注"))
                    table.Columns.Add("备注", typeof(System.String));
                if (!table.Columns.Contains("id"))
                    table.Columns.Add("id", typeof(System.String));
                list = this.TurnShopSPImportList(table);
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Data = "数据格式有误，请重新下载导入模版，再导入";
                br.Message.Add(" 数据格式有误，请重新下载导入模版，再导入 ");
                WriteDBLog("供应商分类-导入", oldParam, br);
                return Json(br);
            }

            ProccessData(filePath, list, ref successList, ref failList);

            #endregion
            #region 返回结果

            if (failList != null && failList.Count() > 0)
            {
                br.Success = true;
                string failFilePath = SaveFailFile(failList);
                br.Data = failFilePath;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(" 共" + table.Rows.Count + "条 成功" + successList.Count() + "条 失败" + failList.Count() + "条");
                WriteDBLog("供应商分类-导入", oldParam, br);
                return Json(br);
            }
            else
            {
                br.Success = true;
                br.Message.Clear();
                br.Message.Add(" 导入成功 ");
                WriteDBLog("供应商分类-导入", oldParam, br);
                return Json(br);
            }
            #endregion
        } 
        #endregion

        #region 供应商分类-下载模版
        /// 供应商分类-下载模版
        /// lz 2016-09-07
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public FileResult DownloadExcelTemp()
        {
            try
            {
                #region 下载模版
                HSSFWorkbook book = new HSSFWorkbook();
                Dictionary<string, int> e_param = new Dictionary<string, int> {
                   {"名称",60},
                   {"编码或条码",20},
                   {"上级名称",60}
                };

                Dictionary<string, string> e_param_comment = new Dictionary<string, string> {
                    {"名称","名称规则:\r\n1.不可为空 \r\n2.长度不能超过60 \r\n3.同级中不能有同名分类"},
                   {"编码或条码","编码或条码规则：\r\n1.可为空 \r\n2.不能有\"+*%/\"等特殊字符，不能以\"_\"或\"-\"开头 \r\n3.长度不能超过40 "},
                   {"上级名称","上级名称规则:\r\n1.最顶级的分类名此项不填写 \r\n2.每一项分类必须有最顶级分类,不然数据不能加入分类中"}
                };

                ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "供应商分类模版");

                IRow rowtemp = sheet1.CreateRow(1);
                rowtemp.CreateCell(0).SetCellValue("供应商01");
                rowtemp.CreateCell(1).SetCellValue("gys100001");
                rowtemp.CreateCell(2).SetCellValue("");
                IRow rowtemp2 = sheet1.CreateRow(2);
                rowtemp2.CreateCell(0).SetCellValue("供应商011");
                rowtemp2.CreateCell(1).SetCellValue("gys1000011");
                rowtemp2.CreateCell(2).SetCellValue("供应商01");
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "供应商分类模版-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
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
        #endregion

        #region 导入辅助
        /// <summary>
        /// 供应商数据处理
        /// </summary>
        /// <param name="list"></param>
        /// <param name="successList"></param>
        /// <param name="failList"></param>
        private void ProccessData(string filePath, List<Tb_Gysfl_Import> list, ref List<Tb_Gysfl_Import> successList, ref List<Tb_Gysfl_Import> failList)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            param.Add("filePath", filePath);
            param.Add("list", list);
            param.Add("id_masteruser", id_user_master);
            param.Add("id_user", id_user);
            br = BusinessFactory.Tb_Gysfl.ImportIn(param);
            if (br.Success)
            {
                Tb_Gysfl_Import_All rModel = (Tb_Gysfl_Import_All)br.Data;
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
        /// <summary>
        /// 保存本地临时文件
        /// </summary>
        /// <param name="failList"></param>
        /// <returns></returns>
        private string SaveFailFile(List<Tb_Gysfl_Import> failList)
        {
            try
            {
                string fileName = "供应商分类导入失败数据" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                string url = "/UpLoad/Temp/" + fileName;
                string fileFullName = ApplicationInfo.TempPath + "\\" + fileName;

                HSSFWorkbook book = new HSSFWorkbook();
                Dictionary<string, int> e_param = new Dictionary<string, int> {
                    {"备注",80},
                    {"名称",40},
                   {"编码或条码",20},
                   {"上级名称",40}
                    
                };

                Dictionary<string, string> e_param_comment = new Dictionary<string, string> {
                    {"名称","名称规则:\r\n1.不可为空 \r\n2.长度不能超过60 \r\n3.同级中不能有同名分类"},
                   {"编码或条码","编码或条码规则：\r\n1.可为空 \r\n2.不能有\"+*%/\"等特殊字符，不能以\"_\"或\"-\"开头 \r\n3.长度不能超过40 "},
                   {"上级名称","上级名称规则:\r\n1.最顶级的分类名此项不填写 \r\n2.每一项分类必须有最顶级分类,不然数据不能加入分类中"}
                };

                ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "供应商分类模版");
                int i = 1;
                foreach (var item in failList)
                {
                    IRow rowtemp = sheet1.CreateRow(i);
                    rowtemp.CreateCell(0).SetCellValue(item.bz);
                    rowtemp.CreateCell(1).SetCellValue(item.mc);
                    rowtemp.CreateCell(2).SetCellValue(item.bm);
                    rowtemp.CreateCell(3).SetCellValue(item.father);
                    
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

        /// <summary>
        /// 下载供应商介绍失败数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public FileResult DownloadFailInfo(string filePath)
        {
            try
            {
                var table = NPOIHelper.ImportExcelFile(filePath);
                if (table == null || table.Rows.Count == 0)
                {
                    return null;
                }
                var dr = table.Select("备注 <> ''");

                var fileName = Server.MapPath("~/Template/info_template.xls");
                FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                IWorkbook book = new HSSFWorkbook(file);
                ISheet sheet1 = book.GetSheet("供应商分类导入");
                for (int i = 0; i < dr.Length; i++)
                {
                    IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(dr[i]["编码或条码"].ToString());
                    rowtemp.CreateCell(1).SetCellValue(dr[i]["名称"].ToString());
                    rowtemp.CreateCell(2).SetCellValue(dr[i]["上级名称"].ToString());
                }
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "供应商导入失败数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private List<Tb_Gysfl_Import> TurnShopSPImportList(DataTable table)
        {
            List<Tb_Gysfl_Import> list = new List<Tb_Gysfl_Import>();
            foreach (DataRow item in table.Rows)
            {
                Tb_Gysfl_Import model = new Tb_Gysfl_Import();
                model.bm = item["编码或条码"] == null ? "" : item["编码或条码"].ToString();
                model.mc = item["名称"] == null ? "" : item["名称"].ToString();
                model.father = item["上级名称"] == null ? "" : item["上级名称"].ToString();
                model.bz = item["备注"] == null ? "" : item["备注"].ToString();
                list.Add(model);
            }
            return list;
        }


        #endregion

        #region Get_Left_Tree
        [HttpGet]
        [ActionPurview(false)]
        public ActionResult Get_Left_Tree()
        {
            BaseResult br = new BaseResult();
            br.Success = true;
            dynamic tree = new List<dynamic>();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("id_masteruser", id_user_master);
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                param.Add("sort", "sort_id");
                param.Add("dir", "asc");

                
                br = BusinessFactory.Tb_Gysfl.GetAll(param);
                if (br.Success && br.Data is List<Tb_Gysfl>)
                {
                    List<Tb_Gysfl> list = br.Data as List<Tb_Gysfl>;
                    list.Insert(0, new Tb_Gysfl() { id = "0", id_farther = "#", mc = "全部", bm = "0" });

                    tree = (from node in list
                            select new
                            {
                                id = node.id,
                                parent = node.id_farther,
                                text = node.mc,
                                bm = node.bm
                            });
                }
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
            }
            br.Data = tree;
            return JsonString(br);
        }
        #endregion

        #region 根据供应商分类id获取子集供应商分类 只获取第一层子集
        /// <summary>
        /// 根据供应商分类id获取子集供应商分类 只获取第一层子集
        /// lz 2017-03-15
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult GetChildInfo()
        {
            BaseResult br = new BaseResult();
            br.Success = true;

            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", string.Empty, HandleType.ReturnMsg);//id
                param = param.Trim(p);
                if (string.IsNullOrEmpty(param["id"].ToString()))
                {
                    br.Success = false;
                    br.Data = "";
                    return JsonString(br);
                }

                param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_farther", param["id"].ToString());
                param.Remove("id");

                br = BusinessFactory.Tb_Gysfl.GetAll(param);
                return JsonString(br);
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Data = "";
                return JsonString(br);
            }
        }
        #endregion


    }
}
