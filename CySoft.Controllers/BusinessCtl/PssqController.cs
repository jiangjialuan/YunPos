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
    public class PssqController:BaseController
    {

        /// <summary>
        /// 查询
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
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("keyword", "", HandleType.Remove, true);
                pv.Add("s_id_shop", "", HandleType.Remove);//入库门店
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
                var userShops =GetCurrentUserMgrShop(id_user,id_shop);
                if (!param.Contains("s_id_shop"))
                {
                    if (userShops.Any())
                    {
                        param.Add("id_shops", (from s in userShops select s.id_shop).ToArray());
                    }
                } 
                #endregion
                PageNavigate pn = new PageNavigate();
                pn = BusinessFactory.Td_Ps_Sq_1.GetPage(param);
                var plist = new PageList<Td_Ps_Sq_1_Query>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"]; //排序规则需要返回前台  
                ViewBag.DigitHashtable = GetParm();
                ViewData["userShopList"] = userShops; 
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
                return View();
            }

        }

        #region 新增/复制
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult Add(string id)
        {
            ViewData["option"] = "add";
            //用户管理门店
            ViewData["userShopList"] =GetCurrentUserMgrShop(id_user,id_shop);
            //ViewData["AllShopList"] = GetShop(Enums.ShopDataType.All);
            //获取前台控制小数点
            ViewBag.DigitHashtable = GetParm();
            //经办人
            ViewData["userList"] = GetUser();
            ViewData["id_user"] = id_user;
            //ViewData["IsMasterShop"] = id_shop_master == id_shop;
            ViewData["dh"] = GetNewDH(Enums.FlagDJLX.DHJH);
            ViewData["id_shop"] = id_shop;
            ViewData["id_shop_master"] = id_shop_master;
            if (!string.IsNullOrEmpty(id))
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                ViewData["item_edit"] = BusinessFactory.Td_Ps_Sq_1.Get(param).Data;
                ViewData["option"] = "copy";
            }
            return View("Edit");
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult Add(PssqModel model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                model.id_create = id_user; 
                model.AutoAudit = AutoAudit();
                var br = BusinessFactory.Td_Ps_Sq_1.Add(model); 
                return br;
            });
            return JsonString(res, 1);
        }
        #endregion

        #region 编辑
        [HttpGet]
        [ActionPurview(true)]
        public ActionResult Edit(string id)
        {
            ViewData["option"] = "edit";
            //用户管理门店
            ViewData["userShopList"] = GetCurrentUserMgrShop(id_user, id_shop); 
            ViewData["AllShopList"] = GetShop(Enums.ShopDataType.All);
            ViewData["IsMasterShop"] = id_shop_master == id_shop;
            //获取前台控制小数点
            ViewBag.DigitHashtable = GetParm();
            //经办人
            ViewData["userList"] = GetUser();
            ViewData["id"] = id;
            ViewData["id_shop"] = id_shop;
            ViewData["id_shop_master"] = id_shop_master;

            if (!string.IsNullOrEmpty(id))
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                var model = BusinessFactory.Td_Ps_Sq_1.Get(param).Data as PssqQueryModel;
                if (model != null)
                {
                    ViewData["dh"] = model.Pssq1.dh;
                    ViewData["zdr_name"] = model.zdr;
                    ViewData["shr_name"] = model.shr;
                }
                ViewData["item_edit"] = model;
            }
            return View();
        }
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult Edit(PssqModel model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                model.id_create = id_user;
                //model.id_shop = id_shop;
                return BusinessFactory.Td_Ps_Sq_1.Update(model);
            });
            return JsonString(res, 1);
        }
        #endregion

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
                return BusinessFactory.Td_Ps_Sq_1.Active(param);
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
                return BusinessFactory.Td_Ps_Sq_1.Stop(param);
            });
            return JsonString(res, 1);
        }

         [ActionPurview(true)]
         [HttpPost]
        public ActionResult Delete(string id)
        {
            var res = HandleResult(() =>
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                return BusinessFactory.Td_Ps_Sq_1.Delete(param);
            });
            return JsonString(res, 1);
        }
        /// <summary>
        /// 弹框选择查询
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult SearchList()
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
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("keyword", "", HandleType.Remove, true);
                pv.Add("id_shop_sq", String.Empty, HandleType.Remove);
                pv.Add("id_shop_pszx", String.Empty, HandleType.Remove);
                pv.Add("sort", "rq_create desc", HandleType.DefaultValue);
                pv.Add("start_time", String.Empty, HandleType.Remove);
                pv.Add("start_time_end", String.Empty, HandleType.Remove);
                pv.Add("_callback_", String.Empty, HandleType.Remove);
                pv.Add("bm_djlx_t", "PS110", HandleType.DefaultValue);
                param = param.Trim(pv);
                if (param.ContainsKey("_callback_"))
                {
                    ViewData["dh_callback"] = param["_callback_"].ToString();
                }; 
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                param.Add("flag_sh", (int)Enums.FlagSh.HadSh);
                param.Add("flag_cancel", (int)Enums.FlagCancel.NoCancel);
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
                if (param.ContainsKey("bm_djlx_t"))
                {
                    ViewData["bm_djlx_t"] = param["bm_djlx_t"];
                }
                param.Add("no_th","1");
                #region 门店处理
                //申请门店
                if (param.Contains("id_shop_sq"))
                {
                    ViewData["id_shop_sq"] = param["id_shop_sq"];
                }
                var userShops = GetCurrentUserMgrShop(id_user, id_shop);
                var id_shop_pszx = string.Format("{0}", param["id_shop_pszx"]);
                //配送中心
                if (!param.Contains("id_shop_pszx"))
                {
                    var defaultPSZX = userShops.FirstOrDefault(a => a.flag_type == 1 || a.flag_type == 2 || a.flag_type == 3);
                    if (defaultPSZX!=null)
	                {
                        id_shop_pszx = defaultPSZX.id_shop;//设置默认配送中心
	                } 
                }
                ViewData["id_shop_pszx"] = id_shop_pszx;
                param["id_shop_pszx"] = id_shop_pszx;
                #endregion   
                #endregion
                PageNavigate pn = new PageNavigate();
                pn = BusinessFactory.Td_Ps_Sq_1.GetPage(param);
                var plist = new PageList<Td_Ps_Sq_1_Query>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"]; //排序规则需要返回前台  
                ViewBag.DigitHashtable = GetParm();
                ViewData["userShopList"] = userShops; 
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            if (param["_search_"].ToString().Equals("1"))
            {
                return PartialView("_SearchList");
            }
            else
            {
                return View();
            }

        }
        /// <summary>
        /// 根据ID查询申请单商品明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult QuerSqdSpmx(string id)
        {
            var res = HandleResult(() =>
            {
                Hashtable param=new Hashtable();
                param.Add("id",id);
                param.Add("id_masteruser", id_user_master);
                return BusinessFactory.Td_Ps_Sq_1.Get(param);
            });
             
            return JsonString(res);
        }
         [ActionPurview(false)]
        public ActionResult QueryShop()
        {
            var shoplist= GetShop(Enums.ShopDataType.All);
            return JsonString(null);
        }


         [HttpGet]
         [ActionPurview(true)]
         [ActionAlias("pssq","list")]
         public ActionResult Detial(string id)
         {
             ViewData["option"] = "edit";
             //用户管理门店
             ViewData["userShopList"] = GetCurrentUserMgrShop(id_user, id_shop);
             //ViewData["AllShopList"] = GetShop(Enums.ShopDataType.All);
             //ViewData["IsMasterShop"] = id_shop_master == id_shop;
             //获取前台控制小数点
             ViewBag.DigitHashtable = GetParm();
             //经办人
             ViewData["userList"] = GetUser();
             ViewData["id"] = id;
             if (!string.IsNullOrEmpty(id))
             {
                 Hashtable param = new Hashtable();
                 param.Add("id", id);
                 param.Add("id_masteruser", id_user_master);
                 var model = BusinessFactory.Td_Ps_Sq_1.Get(param).Data as PssqQueryModel;
                 if (model != null)
                 {
                     ViewData["dh"] = model.Pssq1.dh;
                     ViewData["zdr_name"] = model.zdr;
                     ViewData["shr_name"] = model.shr;
                 }
                 ViewData["item_edit"] = model;
             }
             ViewData["id_shop_master"] = id_shop_master;
             ViewData["id_shop"] = id_shop;
             return View();
         }

         [ActionPurview(false)]
         [HttpGet]
         public ActionResult ImportIn()
         {
             Hashtable param = base.GetParameters();
             ViewData["callback"] = param["callback"];
             ParamVessel p = new ParamVessel();
             p.Add("id_shop", string.Empty, HandleType.Remove);//id_shop
             p.Add("id_shop_ck", string.Empty, HandleType.Remove);//id_shop_ck
             param = param.Trim(p);
             if (param.ContainsKey("id_shop"))
             {
                 ViewData["id_shop"] = param["id_shop"];
             }
             if (param.ContainsKey("id_shop_ck"))
             {
                 ViewData["id_shop_ck"] = param["id_shop_ck"];
             }
             return View("ImportIn");
         }

         [ActionPurview(false)]
         [HttpPost]
         public ActionResult ImportIn(string filePath)
         {
             Hashtable param = base.GetParameters();
             ParamVessel p = new ParamVessel();
             p.Add("id_shop", string.Empty, HandleType.DefaultValue);//id_shop
             p.Add("id_shop_ck", string.Empty, HandleType.DefaultValue);//id_shop
             param = param.Trim(p);

             var savePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\" + filePath;
             DataTable table = NPOIHelper.ImportExcelFile(savePath);
             BaseResult br = new BaseResult() { Level = ErrorLevel.Alert };
             List<Ps_Shopsp_Import> list = new List<Ps_Shopsp_Import>();
             List<Ps_Shopsp_Import> successList = new List<Ps_Shopsp_Import>();
             List<Ps_Shopsp_Import> failList = new List<Ps_Shopsp_Import>();
             try
             {
                 if (!table.Columns.Contains("系统备注"))
                     table.Columns.Add("系统备注", typeof(System.String));
                 if (!table.Columns.Contains("id"))
                     table.Columns.Add("id", typeof(System.String));
                 list = this.TurnShopSPImportList(table);
             }
             catch (Exception ex)
             {
                 br.Success = false;
                 br.Data = "数据格式有误，请重新下载导入模版，再导入";
                 br.Message.Add(" 数据格式有误，请重新下载导入模版，再导入 ");
                 return Json(br);
             }

             if (list == null || list.Count() <= 0)
             {
                 br.Success = false;
                 br.Data = "文件中数据不符合!";
                 br.Message.Add(" 文件中数据不符合，请重新下载导入模版，再导入 ");
                 return Json(br);
             }

             ProccessData(
                 filePath, 
                 ref list, 
                 ref successList, 
                 ref failList, 
                 string.Format("{0}", param["id_shop"]), 
                 string.Format("{0}", param["id_shop_ck"]));

             if (failList != null && failList.Any())
             {
                 br.Success = true;
                 string failFilePath = SaveFailFile(list);
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
         #region 进货商品数据处理
         /// <summary>
         /// 进货商品数据处理
         /// </summary>
         /// <param name="list"></param>
         /// <param name="successList"></param>
         /// <param name="failList"></param>
         private void ProccessData(string filePath, ref List<Ps_Shopsp_Import> list, ref List<Ps_Shopsp_Import> successList, ref List<Ps_Shopsp_Import> failList, string id_shop_sel = "",string id_shop_ck="")
         {
             BaseResult br = new BaseResult();
             Hashtable param = new Hashtable();
             param.Add("filePath", filePath);
             param.Add("list", list);
             param.Add("id_masteruser", id_user_master);
             param.Add("id_user", id_user);
             param.Add("id_shop", id_shop_sel);
             param.Add("id_shop_ck", id_shop_ck);
             br = BusinessFactory.Td_Ps_Sq_1.Export(param);
             if (br.Success)
             {
                 PsShopsp_Import_All rModel = (PsShopsp_Import_All)br.Data;
                 successList = rModel.SuccessList;
                 failList = rModel.FailList;
                 list = rModel.AllList;
             }
             else
             {
                 failList = list;
                 foreach (var item in failList)
                 {
                     item.sysbz = string.Format("{0}", br.Message[0]);
                 }
             }
         }
         #endregion
         #region 保存本地临时文件
         /// <summary>
         /// 保存本地临时文件
         /// </summary>
         /// <param name="failList"></param>
         /// <returns></returns>
         private string SaveFailFile(List<Ps_Shopsp_Import> failList)
         {
             try
             {
                 string fileName = "配送商品导入失败数据" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                 string url = "/UpLoad/Temp/" + fileName;
                 string fileFullName = ApplicationInfo.TempPath + "\\" + fileName;

                 HSSFWorkbook book = new HSSFWorkbook();
                 Dictionary<string, int> e_param = new Dictionary<string, int> {
                   {"条形码",20},
                   {"商品名称",40},
                   {"数量",20},
                   {"单价",20},
                   {"备注",20},
                   {"系统备注",20}
                };

                 Dictionary<string, string> e_param_comment = new Dictionary<string, string> {
                   {"条形码","规则:\r\n1.必填 "},
                   {"数量","规则:\r\n1.必填 "}
                };

                 ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "配送商品资料模版");
                 int i = 1;
                 foreach (var item in failList)
                 {
                     IRow rowtemp = sheet1.CreateRow(i);
                     rowtemp.CreateCell(0).SetCellValue(item.barcode);
                     rowtemp.CreateCell(1).SetCellValue(item.mc);
                     rowtemp.CreateCell(2).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(item.sl), 2)));
                     rowtemp.CreateCell(3).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(item.dj), 2)));
                     rowtemp.CreateCell(4).SetCellValue(item.bz);
                     rowtemp.CreateCell(5).SetCellValue(item.sysbz);
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

         #region TurnShopSPImportList
         private List<Ps_Shopsp_Import> TurnShopSPImportList(DataTable table)
         {
             List<Ps_Shopsp_Import> list = new List<Ps_Shopsp_Import>();
             foreach (DataRow item in table.Rows)
             {
                 Ps_Shopsp_Import model = new Ps_Shopsp_Import();
                 model.barcode = string.Format("{0}", item["条形码"]) ;
                 model.mc = string.Format("{0}", item["商品名称"]);// item["商品名称"] == null ? "" : item["商品名称"].ToString();
                 decimal sl = 0;
                 decimal.TryParse(string.Format("{0}", item["数量"]),out sl);//item["数量"] == null ? "" : item["数量"].ToString(), out sl)
                 model.sl = sl;
                 //decimal dj = 0;
                 //decimal.TryParse(string.Format("{0}", item["单价"]), out dj);//)item["单价"] == null ? "" : item["单价"].ToString()
                 //model.dj = dj;
                 model.bz = string.Format("{0}", item["备注"]);// item["备注"] == null ? "" : item["备注"].ToString();
                 model.sysbz = "";
                 list.Add(model);
             }
             return list;
         }
         #endregion


         [ActionPurview(false)]
         public FileResult DownloadExcelTemp()
         {
             try
             {
                 HSSFWorkbook book = new HSSFWorkbook();
                 Dictionary<string, int> e_param = new Dictionary<string, int> {
                   {"条形码",20},
                   {"商品名称",40},
                   {"数量",20},
                   //{"单价",20},
                   {"备注",20}
                };

                 Dictionary<string, string> e_param_comment = new Dictionary<string, string> {
                   {"条形码","规则:\r\n1.必填 "},
                   {"数量","规则:\r\n1.必填 "}
                };

                 ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "配送商品资料模板");

                 IRow rowtemp = sheet1.CreateRow(1);
                 rowtemp.CreateCell(0).SetCellValue("16092318171654");
                 rowtemp.CreateCell(1).SetCellValue("卡夫王子脆脆多膨化食品甜橙味35G");
                 //rowtemp.CreateCell(2).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(1), 2)));
                 rowtemp.CreateCell(2).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(100), 2)));
                 rowtemp.CreateCell(3).SetCellValue("2016-09-24商品");
                 sheet1.GetRow(1).Height = 28 * 20;

                 // 写入到客户端 
                 System.IO.MemoryStream ms = new System.IO.MemoryStream();
                 book.Write(ms);
                 ms.Seek(0, SeekOrigin.Begin);
                 return File(ms, "application/vnd.ms-excel", "配送商品资料模板-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");

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
         [ActionPurview(false)]
         public FileResult ImportOut()
         {
             #region Excel基类数据
             HSSFWorkbook book = new HSSFWorkbook();
             Dictionary<string, int> e_param = new Dictionary<string, int> {
                   {"条形码",20},
                   {"商品名称",40},
                   {"数量",20},
                   {"单价",20},
                   {"金额",20},
                   {"备注",20},
                   {"系统备注",20}
                };

             Dictionary<string, string> e_param_comment = new Dictionary<string, string> {
                   {"条形码","规则:\r\n1.必填 "},
                   {"数量","规则:\r\n1.必填 "}
                };

             ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "配送商品资料导出");
             System.IO.MemoryStream ms = new System.IO.MemoryStream();
             #endregion

             try
             {
                 #region 获取数据
                 Hashtable param = base.GetParameters();
                 ParamVessel p = new ParamVessel();
                 p.Add("shopspList", string.Empty, HandleType.ReturnMsg);//商品List
                 param = param.Trim(p);
                 List<Td_Jh_2_QueryModel> shopspList = JSON.Deserialize<List<Td_Jh_2_QueryModel>>(param["shopspList"].ToString()) ?? new List<Td_Jh_2_QueryModel>();
                 #endregion
                 #region 验证数据
                 if (shopspList == null || shopspList.Count() <= 0)
                 {
                     IRow rowtemp = sheet1.CreateRow(1);
                     rowtemp.CreateCell(0).SetCellValue("");
                     rowtemp.CreateCell(1).SetCellValue("");
                     rowtemp.CreateCell(2).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(0), 2)));
                     rowtemp.CreateCell(3).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(0), 2)));
                     rowtemp.CreateCell(4).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(0), 2)));
                     rowtemp.CreateCell(5).SetCellValue("");
                     rowtemp.CreateCell(6).SetCellValue("无有效数据");
                     sheet1.GetRow(1).Height = 28 * 20;
                     // 写入到客户端 
                     book.Write(ms);
                     ms.Seek(0, SeekOrigin.Begin);
                     return File(ms, "application/vnd.ms-excel", "配送商品资料导出-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
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
                     rowtempdata.CreateCell(2).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(item.sl), int.Parse(digit["sl_digit"].ToString()))));
                     rowtempdata.CreateCell(3).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(item.dj), int.Parse(digit["dj_digit"].ToString()))));
                     rowtempdata.CreateCell(4).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(item.je), int.Parse(digit["je_digit"].ToString()))));
                     rowtempdata.CreateCell(5).SetCellValue(item.bz);
                     rowtempdata.CreateCell(6).SetCellValue("");
                     sheet1.GetRow(i).Height = 28 * 20;
                     i++;
                 }
                 #endregion
                 #region 写入到客户端
                 // 写入到客户端 
                 book.Write(ms);
                 ms.Seek(0, SeekOrigin.Begin);
                 return File(ms, "application/vnd.ms-excel", "配送商品资料导出-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
                 #endregion
             }
             catch (Exception ex)
             {
                 #region 异常处理
                 IRow rowtempe = sheet1.CreateRow(1);
                 rowtempe.CreateCell(0).SetCellValue("");
                 rowtempe.CreateCell(1).SetCellValue("");
                 rowtempe.CreateCell(2).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(0), 2)));
                 rowtempe.CreateCell(3).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(0), 2)));
                 rowtempe.CreateCell(4).SetCellValue(string.Format("{0}", Decimal.Round((decimal)(0), 2)));
                 rowtempe.CreateCell(5).SetCellValue("");
                 rowtempe.CreateCell(6).SetCellValue("配送导出出现异常 请重试");
                 sheet1.GetRow(1).Height = 28 * 20;
                 // 写入到客户端 
                 book.Write(ms);
                 ms.Seek(0, SeekOrigin.Begin);
                 return File(ms, "application/vnd.ms-excel", "配送商品资料导出-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
                 #endregion
             }
         }
         #endregion


    }

}
