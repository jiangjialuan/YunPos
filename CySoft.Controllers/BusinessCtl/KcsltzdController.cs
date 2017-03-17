using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Model.Td;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace CySoft.Controllers.BusinessCtl
{
    [LoginActionFilter]
    public class KcsltzdController:BaseController
    {
        [ActionPurview(true)]
        public ActionResult List()
        {
            Hashtable param = base.GetParameters();
            try
            {
                var s_id_shop = string.Format("{0}", param["s_id_shop"]);
                int limit = base.PageSizeFromCookie;
                param.Add("id_masteruser", id_user_master);
                ParamVessel pv = new ParamVessel();
                pv.Add("_search_", "0", HandleType.DefaultValue);
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                pv.Add("s_dh", "", HandleType.Remove, true);
                pv.Add("s_flag_sh", "", HandleType.Remove);
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("sort", "rq_create desc", HandleType.DefaultValue);
                pv.Add("start_time", String.Empty, HandleType.Remove);
                pv.Add("start_time_end", String.Empty, HandleType.Remove);
                param = param.Trim(pv);
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);
                if (param.ContainsKey("start_time"))
                {
                    param.Add("start_rq", param["start_time"].ToString());
                    param.Remove("start_time");
                }
                if (param.ContainsKey("start_time_end"))
                {
                    param.Add("end_rq", param["start_time_end"].ToString());
                    param.Remove("start_time_end");
                }
                //param.Add("id_shop_array", GetCurrentUserMgrShopIdArray());
                if (string.IsNullOrEmpty(s_id_shop)) //s_id_shop
                {
                    param.Add("id_shop_array", GetCurrentUserMgrShopIdArray());
                }
                else
                {
                    param.Add("id_shop_array", new string[] { s_id_shop });
                }
                PageNavigate pn = new PageNavigate();
                pn = BusinessFactory.Td_Kc_Sltz_1.GetPage<Td_Kc_Sltz_1, Td_Kc_Sltz_1_Query>(param);

                var plist = new PageList<Td_Kc_Sltz_1_Query>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"];//排序规则需要返回前台
                ViewBag.DigitHashtable = GetParm();
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            if (param["_search_"].ToString().Equals("1"))
            {
                return PartialView("_List");
            }
            else
            {
                ViewData["MgrShop"] = GetCurrentUserMgrShop(id_user,id_shop);
                return View();
            }
        }

        [HttpGet]
        [ActionPurview(true)]
        public ActionResult Add()
        {
            ViewData["DigitHashtable"] = GetParm();
            ViewData["option"] = "add";
            var date = DateTime.Now;
            ViewData["dh"] = GetNewDH(Enums.FlagDJLX.DHJH);
            //经办人
            ViewData["userList"] = GetUser();
            //用户管理门店
            ViewData["userShopList"] = GetCurrentUserMgrShop(id_user, id_shop); //GetShop(Enums.ShopDataType.UserShop);
            ViewData["id_shop"] = id_shop;
            return View("Edit");
        }

        [HttpPost]
        [ActionPurview(true)]
        public ActionResult Add(KcsltzModel model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                model.id_create = id_user;
                model.AutoAudit = AutoAudit();
                return BusinessFactory.Td_Kc_Sltz_1.Add(model);
            });
            return JsonString(res, 1);
        }
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult Edit(string id)
        {
            ViewData["DigitHashtable"] = GetParm();
            ViewData["option"] = "edit";
            var date = DateTime.Now;
            //经办人
            ViewData["userList"] = GetUser();
            //用户管理门店
            ViewData["userShopList"] = GetCurrentUserMgrShop(id_user, id_shop); //GetShop(Enums.ShopDataType.UserShop);
            Hashtable param = new Hashtable();
            param.Add("id", id);
            param.Add("id_masteruser", id_user_master);
            var model = BusinessFactory.Td_Kc_Sltz_1.Get(param).Data as KcsltzdQueryModel;
            if (model != null)
            {
                ViewData["dh"] = model.model1.dh;// date.ToString("yyyyMMddHHmmss") + date.Millisecond;
                ViewData["zdr_name"] = model.zdr;
                ViewData["shr_name"] = model.shr;
            }
            ViewData["item_edit"] = model;
            return View();
        }

        [HttpGet]
        [ActionPurview(true)]
        public ActionResult Detial(string id)
        {
            ViewData["DigitHashtable"] = GetParm();
            ViewData["option"] = "edit";
            var date = DateTime.Now;
            //经办人
            ViewData["userList"] = GetUser();
            //用户管理门店
            ViewData["userShopList"] = GetCurrentUserMgrShop(id_user, id_shop); //GetShop(Enums.ShopDataType.UserShop);
            Hashtable param = new Hashtable();
            param.Add("id", id);
            param.Add("id_masteruser", id_user_master);
            var model = BusinessFactory.Td_Kc_Sltz_1.Get(param).Data as KcsltzdQueryModel;
            if (model != null)
            {
                ViewData["dh"] = model.model1.dh;// date.ToString("yyyyMMddHHmmss") + date.Millisecond;
                ViewData["zdr_name"] = model.zdr;
                ViewData["shr_name"] = model.shr;
            }
            ViewData["item_edit"] = model;
            return View();
        }

        [HttpPost]
        [ActionPurview(true)]
        public ActionResult Edit(KcsltzModel model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                model.id_edit = id_user;
                return BusinessFactory.Td_Kc_Sltz_1.Update(model);
            });
            return JsonString(res, 1);
        }

        [HttpGet]
        [ActionPurview(true)]
        public ActionResult Copy(string id)
        {
            ViewData["DigitHashtable"] = GetParm();
            ViewData["option"] = "copy";
            var date = DateTime.Now;
            //经办人
            ViewData["userList"] = GetUser();
            //用户管理门店
            ViewData["userShopList"] = GetCurrentUserMgrShop(id_user, id_shop); //GetShop(Enums.ShopDataType.UserShop);
            Hashtable param = new Hashtable();
            param.Add("id", id);
            param.Add("id_masteruser", id_user_master);
            param.Add("copy", "copy");
            var model = BusinessFactory.Td_Kc_Sltz_1.Get(param).Data as KcsltzdQueryModel;
            if (model != null)
            {
                ViewData["dh"] = GetNewDH(Enums.FlagDJLX.DHJH);
                model.model1.id = "";
            }
            ViewData["item_edit"] = model; 
            return View("Edit");
        }

        [HttpPost]
        [ActionPurview(true)]
        public ActionResult Copy(KcsltzModel model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                model.id_create = id_user;
                model.AutoAudit = AutoAudit();
                return BusinessFactory.Td_Kc_Sltz_1.Add(model);
            });
            return JsonString(res, 1);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult Sh(string id)
        {
            var res = HandleResult(() =>
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                return BusinessFactory.Td_Kc_Sltz_1.Active(param);
            });
            return JsonString(res, 1);
        }
        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult ZF(string id)
        {
            var res = HandleResult(() =>
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                return BusinessFactory.Td_Kc_Sltz_1.Stop(param);
            });
            return JsonString(res, 1);
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult Delete(string id)
        {
            var res = HandleResult(() =>
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                return BusinessFactory.Td_Kc_Sltz_1.Delete(param);
            });
            return JsonString(res, 1);
        }

        [ActionPurview(false)]
        [HttpGet]
        public ActionResult ImportIn()
        {
            Hashtable param = base.GetParameters();
            ViewData["callback"] = param["callback"];
            ParamVessel p = new ParamVessel();
            p.Add("id_shop", string.Empty, HandleType.Remove);//id_shop
            param = param.Trim(p);
            if (param.ContainsKey("id_shop"))
                ViewData["id_shop"] = param["id_shop"].ToString();

            return View("ImportIn");
        }

        [ActionPurview(false)]
        [HttpPost]
        public ActionResult ImportIn(string filePath)
        {
            Hashtable param = base.GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("id_shop", string.Empty, HandleType.DefaultValue);//id_shop
            param = param.Trim(p);


            var savePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\" + filePath;
            DataTable table = NPOIHelper.ImportExcelFile(savePath);
            BaseResult br = new BaseResult() { Level = ErrorLevel.Alert };
            List<KcsltzShopsp_Import> list = new List<KcsltzShopsp_Import>();
            List<KcsltzShopsp_Import> successList = new List<KcsltzShopsp_Import>();
            List<KcsltzShopsp_Import> failList = new List<KcsltzShopsp_Import>();
            try
            {
                if (!table.Columns.Contains("系统备注"))
                    table.Columns.Add("系统备注", typeof(System.String));
                if (!table.Columns.Contains("id"))
                    table.Columns.Add("id", typeof(System.String));
                list = this.TurnImportList(table);
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Data = "数据格式有误，请重新下载导入模版，再导入";
                br.Message.Add(" 数据格式有误，请重新下载导入模版，再导入 ");
                return Json(br);
            }

            if (!list.Any())
            {
                br.Success = false;
                br.Data = "文件中数据不符合!";
                br.Message.Add(" 文件中数据不符合，请重新下载导入模版，再导入 ");
                return Json(br);
            }

            ProccessData(filePath, ref list, ref successList, ref failList, param["id_shop"].ToString());

            if (failList != null && failList.Any())
            {
                br.Success = true;
                string failFilePath = SaveFailFile(failList);
                br.Data = failFilePath;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(" 共" + table.Rows.Count + "条 符合条件" + successList.Count() + "条  不符合" + failList.Count() + "条 本次未处理 ");
                return Json(br);
            }
            else
            {
                br.Success = true;
                br.Message.Clear();
                br.Message.Add(" 导入成功 ");
                br.Data = JSON.Serialize(successList);
                return Json(br);
            }
        }

        private List<KcsltzShopsp_Import> TurnImportList(DataTable table)
        {
            List<KcsltzShopsp_Import> list = new List<KcsltzShopsp_Import>();
            foreach (DataRow item in table.Rows)
            {
                KcsltzShopsp_Import model = new KcsltzShopsp_Import();
                model.barcode = string.Format("{0}", item["商品条码"]);
                model.mc = string.Format("{0}", item["商品名称"]);
                model.dw = string.Format("{0}", item["单位"]);
                decimal sl = 0;
                decimal.TryParse(string.Format("{0}", item["数量"]), out sl);
                model.sl = sl;
                model.bz = string.Format("{0}", item["备注"]);  
                list.Add(model);
            }
            return list;
        }

        private void ProccessData(string filePath, ref List<KcsltzShopsp_Import> list, ref List<KcsltzShopsp_Import> successList, ref List<KcsltzShopsp_Import> failList, string id_shop_sel = "")
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            param.Add("filePath", filePath);
            param.Add("list", list);
            param.Add("id_masteruser", id_user_master);
            param.Add("id_user", id_user);
            //param.Add("id_shop", id_shop);
            if (!string.IsNullOrEmpty(id_shop_sel))
                param.Add("id_shop", id_shop_sel);
            else
                param.Add("id_shop", id_shop);
            br = BusinessFactory.Td_Kc_Sltz_1.Export(param);
            if (br.Success)
            {
                KcsltzShopsp_Import_All rModel = (KcsltzShopsp_Import_All)br.Data;
                successList = rModel.SuccessList;
                failList = rModel.FailList;
                list = rModel.AllList;
            }
            else
            {
                failList = list;
                foreach (var item in failList)
                    item.sysbz = br.Message[0].ToString();
            }
        }

        private string SaveFailFile(List<KcsltzShopsp_Import> failList)
        {
            try
            {
                string fileName = "盘点导入失败数据" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                string url = "/UpLoad/Temp/" + fileName;
                string fileFullName = ApplicationInfo.TempPath + "\\" + fileName;

                HSSFWorkbook book = new HSSFWorkbook();
                Dictionary<string, int> e_param = new Dictionary<string, int> {
                   {"商品条码",40},
                   {"商品名称",40},
                   {"单位",20},
                   {"数量",20},
                   {"库存数量",20},
                   {"备注",20},
                   {"系统备注",20}
                };

                Dictionary<string, string> e_param_comment = new Dictionary<string, string> {
                   {"商品条码","规则:\r\n1.必填 "},
                   {"数量","规则:\r\n1.必填 "}
                };

                ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "库存调整商品资料模版");
                int i = 1;
                foreach (var item in failList)
                {
                    IRow rowtemp = sheet1.CreateRow(i);
                    rowtemp.CreateCell(0).SetCellValue(item.barcode);
                    rowtemp.CreateCell(1).SetCellValue(item.mc);
                    rowtemp.CreateCell(2).SetCellValue(item.dw);
                    rowtemp.CreateCell(3).SetCellValue(string.Format("{0}", item.sl));
                    rowtemp.CreateCell(4).SetCellValue(string.Format("{0}", item.sl_qm));
                    rowtemp.CreateCell(5).SetCellValue(item.bz);
                    rowtemp.CreateCell(6).SetCellValue(item.sysbz);
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


        [ActionPurview(false)]
        public FileResult DownloadExcelTemp()
        {
            try
            {
                HSSFWorkbook book = new HSSFWorkbook();
                Dictionary<string, int> e_param = new Dictionary<string, int> {
                   {"商品条码",40},
                   {"商品名称",40},
                   {"单位",20},
                   {"数量",20},
                   {"库存数量",20},
                   {"备注",20}
                };

                Dictionary<string, string> e_param_comment = new Dictionary<string, string> {
                   {"商品条码","规则:\r\n1.必填 "},
                   {"实盘数量","规则:\r\n1.必填 "}
                };

                ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "库存调整商品资料模板");

                IRow rowtemp = sheet1.CreateRow(1);
                rowtemp.CreateCell(0).SetCellValue("1612261035461");
                rowtemp.CreateCell(1).SetCellValue("食品0001(KG)");
                rowtemp.CreateCell(2).SetCellValue("KG");
                rowtemp.CreateCell(3).SetCellValue(50);
                rowtemp.CreateCell(4).SetCellValue(40);
                rowtemp.CreateCell(5).SetCellValue("库存调整商品");
                sheet1.GetRow(1).Height = 28 * 20;

                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "库存调整商品资料模板-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");

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

        #region 导出
        /// <summary>
        /// 
        /// </summary>
        [ActionPurview(false)]
        public FileResult ImportOut()
        {
            #region Excel基类数据
            HSSFWorkbook book = new HSSFWorkbook();
            Dictionary<string, int> e_param = new Dictionary<string, int> {
                   {"商品条码",20},
                   {"商品名称",40},
                   {"单位",40},
                   {"数量",20},
                   {"总数量",20},
                   {"库存数量",20},
                   {"单价",20},
                   {"进货价",20},
                   {"零售价",20},
                   {"金额",20},
                   {"成本金额",20},
                   {"备注",20}
                };

            Dictionary<string, string> e_param_comment = new Dictionary<string, string> {
                   {"商品条码","规则:\r\n1.必填 "},
                   {"实盘数量","规则:\r\n1.必填 "}
                };

            ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "库存调整商品资料导出");
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            #endregion

            try
            {
                #region 获取数据
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("shopspList", string.Empty, HandleType.ReturnMsg);//商品List
                param = param.Trim(p);
                List<Td_Kc_Sltz_2_Query> shopspList = JSON.Deserialize<List<Td_Kc_Sltz_2_Query>>(param["shopspList"].ToString()) ?? new List<Td_Kc_Sltz_2_Query>();
                #endregion
                #region 验证数据
                if (shopspList == null || !shopspList.Any())
                {
                    IRow rowtemp = sheet1.CreateRow(1);
                    rowtemp.CreateCell(0).SetCellValue("");
                    rowtemp.CreateCell(1).SetCellValue("");
                    rowtemp.CreateCell(2).SetCellValue("");
                    rowtemp.CreateCell(3).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(0), 2)));
                    rowtemp.CreateCell(4).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(0), 2)));
                    rowtemp.CreateCell(5).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(0), 2)));
                    rowtemp.CreateCell(6).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(0), 2)));
                    rowtemp.CreateCell(7).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(0), 2)));
                    rowtemp.CreateCell(8).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(0), 2)));
                    rowtemp.CreateCell(9).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(0), 2)));
                    rowtemp.CreateCell(10).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(0), 2)));
                    rowtemp.CreateCell(11).SetCellValue("");
                    sheet1.GetRow(1).Height = 28 * 20;
                    // 写入到客户端 
                    book.Write(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    return File(ms, "application/vnd.ms-excel", "库存调整商品资料导出-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
                }
                #endregion
                #region 数量、单价、金额小数点
                var digit = GetParm();
                #endregion
                #region 构建Excel数据内容
                int i = 1;
                foreach (var item in shopspList)
                {
                    IRow rowtempdata = sheet1.CreateRow(i);
                    rowtempdata.CreateCell(0).SetCellValue(item.barcode);
                    rowtempdata.CreateCell(1).SetCellValue(item.shopsp_name);
                    rowtempdata.CreateCell(2).SetCellValue(item.dw);
                    rowtempdata.CreateCell(3).SetCellValue(Decimal.Round((decimal)(item.sl), int.Parse(digit["sl_digit"].ToString())).ToString());
                    rowtempdata.CreateCell(4).SetCellValue(Decimal.Round((decimal)(item.sl_total), int.Parse(digit["sl_digit"].ToString())).ToString());
                    rowtempdata.CreateCell(5).SetCellValue(Decimal.Round((decimal)(item.sl_kc), int.Parse(digit["sl_digit"].ToString())).ToString());
                    rowtempdata.CreateCell(6).SetCellValue(Decimal.Round((decimal)(item.dj), int.Parse(digit["sl_digit"].ToString())).ToString());
                    rowtempdata.CreateCell(7).SetCellValue(Decimal.Round((decimal)(item.dj_jh), int.Parse(digit["je_digit"].ToString())).ToString());
                    rowtempdata.CreateCell(8).SetCellValue(Decimal.Round((decimal)(item.dj_ls), int.Parse(digit["je_digit"].ToString())).ToString());

                    rowtempdata.CreateCell(9).SetCellValue(Decimal.Round((decimal)(item.je), int.Parse(digit["je_digit"].ToString())).ToString());
                    rowtempdata.CreateCell(10).SetCellValue(Decimal.Round((decimal)(item.je_cb), int.Parse(digit["je_digit"].ToString())).ToString());
                    rowtempdata.CreateCell(11).SetCellValue(item.bz);
                    sheet1.GetRow(i).Height = 28 * 20;
                    i++;
                }
                #endregion
                #region 写入到客户端
                // 写入到客户端 
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "库存调整商品资料导出-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
                #endregion
            }
            catch (Exception ex)
            {
                #region 异常处理
                IRow rowtempe = sheet1.CreateRow(1);
                rowtempe.CreateCell(0).SetCellValue("");
                rowtempe.CreateCell(1).SetCellValue("");
                rowtempe.CreateCell(2).SetCellValue(Decimal.Round((decimal)(0), 2).ToString());
                rowtempe.CreateCell(3).SetCellValue(Decimal.Round((decimal)(0), 2).ToString());
                rowtempe.CreateCell(4).SetCellValue(Decimal.Round((decimal)(0), 2).ToString());
                rowtempe.CreateCell(5).SetCellValue("");
                rowtempe.CreateCell(6).SetCellValue("库存调整导出出现异常 请重试");
                sheet1.GetRow(1).Height = 28 * 20;
                // 写入到客户端 
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "库存调整商品资料导出-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
                #endregion
            }
        }
        #endregion


    }
}
