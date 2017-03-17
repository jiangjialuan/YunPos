using System;
using System.Collections;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Enums;
using CySoft.Model.Td;
using CySoft.Utility;
using System.Collections.Generic;
using CySoft.Model.Flags;
using System.Linq;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

//订货
namespace CySoft.Controllers.BusinessCtl
{
    [LoginActionFilter]
    public class DhController : BaseController
    {

        #region 订货-查询
        /// <summary>
        /// 订货-查询
        /// lz 2016-10-27
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
                pv.Add("id_shop", String.Empty, HandleType.Remove);
                pv.Add("id_gys", String.Empty, HandleType.Remove);
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



                if (id_shop != id_shop_master)//user_id_shops
                {
                    var userShops = GetShop(Enums.ShopDataType.UserShopOnly); //GetUserShop();

                    if (userShops.Any())
                    {
                        param.Add("user_id_shops", (from s in userShops select s.id_shop).ToArray());
                    }
                }

                #endregion
                #region 获取数据
                PageNavigate pn = new PageNavigate();

                pn = BusinessFactory.Td_Jh_Dd_1.GetPage(param);
                var plist = new PageList<Td_Jh_Dd_1_QueryModel>(pn, pageIndex, limit);

                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"]; //排序规则需要返回前台  

                //用户管理门店
                ViewData["userShopList"] = GetShop(Enums.ShopDataType.UserShop);
                //用户管理供应商
                ViewData["userGysList"] = GetUserGys();
                //获取前台控制小数点
                ViewBag.DigitHashtable = GetParm();
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

        #region 订货-新增
        /// <summary>
        /// 订货-新增 
        /// lz
        /// 2016-09-09
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Add()
        {
            ViewData["option"] = "add";
            //制单门店
            ViewData["userShopList"] = GetCurrentUserMgrShop(id_user, id_shop);// GetShop(Enums.ShopDataType.UserShop);
            //用户管理门店
            ViewData["userShopShopList"] = GetShop(Enums.ShopDataType.ShopShop);

            //用户管理供应商
            ViewData["userGysList"] = GetUserGys();
            //获取前台控制小数点
            ViewBag.DigitHashtable = GetParm();
            //经办人
            ViewData["userList"] = GetUser();

            Hashtable param = base.GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("id", string.Empty, HandleType.Remove);//是否需要复制
            p.Add("data_json", string.Empty, HandleType.Remove);//是否需要复制
            p.Add("type", string.Empty, HandleType.Remove);//type
            param = param.Trim(p);

            if (param.ContainsKey("type"))
                ViewData["type"] = param["type"].ToString();

            if (param.ContainsKey("data_json") && !string.IsNullOrEmpty(param["data_json"].ToString()))
                ViewData["data_json"] = param["data_json"].ToString();

            ViewData["zdr_name"] = ViewData["shr_name"] = GetLoginInfo<string>("name");

            if (param.ContainsKey("id") && !string.IsNullOrEmpty(param["id"].ToString()))
            {
                var br = BusinessFactory.Td_Jh_Dd_1.Get(param);
                if (br.Success)
                {
                    ViewBag.CopyInfo = br.Data;
                }
            }
            ViewData["id_user"] = id_user;
            return View();
        }
        #endregion

        #region 订货-Post新增
        /// <summary>
        /// 订货-Post新增
        /// lz
        /// 2016-09-09
        /// </summary>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Add(Tb_Shopsp model)
        {
            BaseResult br = new BaseResult();
            var oldParam = new Hashtable();
            try
            {
                #region 获取参数
                Hashtable param = base.GetParameters();

                ParamVessel p = new ParamVessel();
                p.Add("shopspList", string.Empty, HandleType.ReturnMsg);//商品List
                p.Add("remark", string.Empty, HandleType.DefaultValue);//备注
                p.Add("id_gys", string.Empty, HandleType.ReturnMsg);//id_gys
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_jbr", string.Empty, HandleType.ReturnMsg);//id_jbr
                p.Add("dh", string.Empty, HandleType.ReturnMsg);
                p.Add("rq", string.Empty, HandleType.ReturnMsg);
                //p.Add("je_sf", string.Empty, HandleType.ReturnMsg);//je_sf
                p.Add("type", string.Empty, HandleType.Remove);//type
                p.Add("id", string.Empty, HandleType.Remove);//id
                //p.Add("id_shop_sh", string.Empty, HandleType.ReturnMsg);//id_shop_sh

                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                oldParam = (Hashtable)param.Clone();
                #endregion

                List<Td_Jh_Dd_2> shopspList = JSON.Deserialize<List<Td_Jh_Dd_2>>(param["shopspList"].ToString()) ?? new List<Td_Jh_Dd_2>();

                #region 验证数据
                if (shopspList == null || shopspList.Count() <= 0)
                {
                    br.Success = false;
                    br.Message.Add("商品不能为空!");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "shopspList";
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }

                var digitHashtable = GetParm();
                foreach (var item in shopspList)
                {
                    item.sl = CySoft.Utility.DecimalExtension.Digit(item.sl, int.Parse(digitHashtable["sl_digit"].ToString()));
                    item.dj = CySoft.Utility.DecimalExtension.Digit(item.dj, int.Parse(digitHashtable["dj_digit"].ToString()));
                    item.je = CySoft.Utility.DecimalExtension.Digit(item.je, int.Parse(digitHashtable["je_digit"].ToString()));

                    if (item.sl == 0)
                    {
                        br.Success = false;
                        br.Message.Add("商品[" + item.barcode + "]数量不允许为0!");
                        br.Level = ErrorLevel.Warning;
                        br.Data = "shopspList";
                        return base.JsonString(new
                        {
                            status = "error",
                            message = string.Join(";", br.Message)
                        });

                        //item.je = 0;
                        //continue;
                    }

                    //此处验证数据是否符合
                    var tempJe = CySoft.Utility.DecimalExtension.Digit(item.sl * item.dj, int.Parse(digitHashtable["je_digit"].ToString()));
                    var tempDj = CySoft.Utility.DecimalExtension.Digit(item.je / item.sl, int.Parse(digitHashtable["dj_digit"].ToString()));
                    if (tempJe == item.je || tempDj == item.dj)
                    {
                        continue;
                    }
                    else
                    {
                        br.Success = false;
                        br.Message.Add("商品中存在 单价*数量不等于金额的数据!");
                        return base.JsonString(new
                        {
                            status = "error",
                            message = string.Join(";", br.Message)
                        });
                    }
                }

                param.Remove("shopspList");
                param.Add("shopspList", shopspList);
                param.Add("DigitHashtable", digitHashtable);
                param.Add("autoAudit", AutoAudit());
                #endregion

                if (param.ContainsKey("type") && param["type"].ToString() == "edit")
                {
                    br = BusinessFactory.Td_Jh_Dd_1.Update(param);
                    WriteDBLog("订货-编辑", oldParam, br);
                }
                else
                {
                    //插入表
                    br = BusinessFactory.Td_Jh_Dd_1.Add(param);
                    WriteDBLog("订货-新增", oldParam, br);
                }

                if (br.Success)
                {
                    return base.JsonString(new
                    {
                        status = "success",
                        message = string.Join(";", br.Message)
                    });

                }
                else
                {
                    return base.JsonString(br, 1);
                }
            }
            catch (CySoftException brEx)
            {
                br.Success = false;
                br.Data = "";
                br.Message.Clear();
                br.Message.Add(brEx.Message.ToString());
                br.Level = ErrorLevel.Warning;
                WriteDBLog("订货-新增/编辑", oldParam, br);
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Data = "";
                br.Message.Add("数据不符合要求!");
                br.Level = ErrorLevel.Warning;
                WriteDBLog("订货-新增/编辑", oldParam, br);
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }
        }

        #endregion

        #region 订货-删除
        /// <summary>
        ///订货-删除
        /// lz 2016-10-28
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
                br = BusinessFactory.Td_Jh_Dd_1.Delete(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("订货-删除", oldParam, br);
            return JsonString(br, 1);
        }
        #endregion

        #region 订货-审核
        /// <summary>
        ///订货-审核
        /// lz 2016-10-28
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult Sh()
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            ParamVessel pv = new ParamVessel();
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            var oldParam = new Hashtable();
            try
            {
                param = param.Trim(pv);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                oldParam = (Hashtable)param.Clone();
                br = BusinessFactory.Td_Jh_Dd_1.Active(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("订货-审核", oldParam, br);
            return JsonString(br, 1);
        }
        #endregion

        #region 订货-作废
        /// <summary>
        ///订货-作废
        /// lz 2016-09-28
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult Stop()
        {
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            param.Add("id_masteruser", id_user_master);
            param.Add("id_user", id_user);
            pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
            pv.Add("id_user", string.Empty, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            try
            {
                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                br = BusinessFactory.Td_Jh_Dd_1.Stop(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("订货-作废", oldParam, br);
            return JsonString(br, 1);
        }
        #endregion

        #region 订货-选择查询
        /// <summary>
        /// 订货-选择查询
        /// lz 2016-10-14
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
                pv.Add("id_shop", String.Empty, HandleType.Remove);
                pv.Add("id_shop_sh", String.Empty, HandleType.Remove);
                pv.Add("id_gys", String.Empty, HandleType.Remove);
                pv.Add("start_time", String.Empty, HandleType.Remove);
                pv.Add("start_time_end", String.Empty, HandleType.Remove);

                pv.Add("dh_callback", "", HandleType.Remove);

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

                param.Add("no_th", 1);
                param.Add("no_close_shop", 1);
                

                if (id_shop != id_shop_master)//user_id_shops
                {
                    var userShops = GetShop(Enums.ShopDataType.UserShopOnly); //GetUserShop();

                    if (userShops.Any())
                    {
                        param.Add("user_id_shops", (from s in userShops select s.id_shop).ToArray());
                    }
                }

                #endregion
                #region 获取数据
                PageNavigate pn = new PageNavigate();

                pn = BusinessFactory.Td_Jh_Dd_1.GetPage(param);
                var plist = new PageList<Td_Jh_Dd_1_QueryModel>(pn, pageIndex, limit);

                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"]; //排序规则需要返回前台  

                //用户管理门店
                ViewData["userShopList"] = GetShop(Enums.ShopDataType.UserShop);
                //用户管理供应商
                ViewData["userGysList"] = GetUserGys();
                //获取前台控制小数点
                ViewBag.DigitHashtable = GetParm();

                if (param.ContainsKey("dh_callback") && !string.IsNullOrEmpty(param["dh_callback"].ToString()))
                    ViewData["dh_callback"] = param["dh_callback"].ToString();

                #endregion

            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            if (param["_search_"].ToString().Equals("1"))
                return PartialView("_Search");
            else
                return View();

        }
        #endregion

        #region 根据id获取详细
        [ActionPurview(false)]
        public ActionResult GetDhDetailList()
        {
            BaseResult br = new BaseResult() { Success = false };
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", string.Empty, HandleType.Remove);//是否需要复制
                param = param.Trim(p);
                if (param.ContainsKey("id") && !string.IsNullOrEmpty(param["id"].ToString()))
                {
                    br = BusinessFactory.Td_Jh_Dd_1.Get(param);
                }
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

        #region 订货详细
        /// <summary>
        /// 订货详细
        /// lz
        /// 2016-11-08
        /// </summary>
        //[ActionPurview(false)]
        [ActionAlias("dh", "list")]
        public ActionResult Detail()
        {
            try
            {
                ViewData["option"] = "add";
                //制单门店
                ViewData["userShopList"] = GetCurrentUserMgrShop(id_user, id_shop);// GetShop(Enums.ShopDataType.UserShop);
                //用户管理门店
                ViewData["userShopShopList"] = GetShop(Enums.ShopDataType.ShopShop);
                //用户管理供应商
                ViewData["userGysList"] = GetUserGys();
                //获取前台控制小数点
                ViewBag.DigitHashtable = GetParm();
                //经办人
                ViewData["userList"] = GetUser();

                Hashtable param = base.GetParameters();
                param.Add("id_masteruser", id_user_master);
                ParamVessel p = new ParamVessel();
                p.Add("id_masteruser", id_user_master, HandleType.DefaultValue);
                p.Add("id", String.Empty, HandleType.ReturnMsg);
                var br = BusinessFactory.Td_Jh_Dd_1.Get(param);
                if (br.Success)
                    ViewBag.CopyInfo = br.Data;
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

        #region 生成条码
        [ActionPurview(false)]
        public ActionResult CreateDH()
        {
            BaseResult br = new BaseResult();
            try
            {
                br.Success = true;
                br.Data = GetNewDH(Enums.FlagDJLX.DHDH);
                return JsonString(br);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 订货-根据id_gys获取订货单
        ///// <summary>
        /////订货-根据id_gys获取订货单
        ///// lz 2016-09-22
        ///// </summary>
        ///// <returns></returns>
        //[ActionPurview(true)]
        //public ActionResult Search()
        //{
        //    Hashtable param = base.GetParameters();
        //    try
        //    {
        //        #region 获取参数
        //        int limit = base.PageSizeFromCookie;
        //        ParamVessel pv = new ParamVessel();
        //        pv.Add("id_gys", string.Empty, HandleType.ReturnMsg);
        //        pv.Add("page", 0, HandleType.DefaultValue);
        //        pv.Add("pageSize", limit, HandleType.DefaultValue);
        //        pv.Add("sort", "rq_create desc", HandleType.DefaultValue);
        //        param = param.Trim(pv);
        //        param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
        //        param.Add("flag_cancel", (int)Enums.FlagCancel.NoCancel);
        //        param.Add("flag_sh", (int)Enums.FlagSh.UnSh);
        //        int pageIndex = Convert.ToInt32(param["page"]);
        //        param.Add("limit", limit);
        //        param.Add("start", pageIndex * limit);

        //        #endregion
        //        #region 获取数据
        //        PageNavigate pn = new PageNavigate();

        //        pn = BusinessFactory.Td_Jh_1.GetPage(param);
        //        var plist = new PageList<Td_Jh_1_QueryModel>(pn, pageIndex, limit);

        //        plist.PageIndex = pageIndex;
        //        plist.PageSize = limit;
        //        ViewData["List"] = plist;
        //        ViewData["sort"] = param["sort"]; //排序规则需要返回前台  

        //        //用户管理门店
        //        ViewData["userShopList"] = GetUserShop();
        //        //用户管理供应商
        //        ViewData["userGysList"] = GetUserGys();
        //        //获取前台控制小数点
        //        ViewBag.DigitHashtable = GetParm();
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {

        //    }



        //    BaseResult br = new BaseResult();

        //    Hashtable param_model = null;

        //    try
        //    {

        //        br = BusinessFactory.Td_Jh_1.Delete(param);
        //    }
        //    catch (Exception ex)
        //    {
        //        br.Message.Add(ex.Message);
        //    }
        //    return JsonString(br, 1);
        //}
        #endregion

        #region 订货-批量导入商品 UI
        /// <summary>
        /// 订货-批量导入商品 UI 
        /// lz
        /// 2016-09-26
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult ImportIn()
        {
            Hashtable param = base.GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("id_shop", string.Empty, HandleType.Remove);//id_shop
            param = param.Trim(p);
            if (param.ContainsKey("id_shop"))
                ViewData["id_shop"] = param["id_shop"].ToString();

            return View("ImportIn");
        }
        #endregion

        #region 订货-Post批量导入商品
        /// <summary>
        /// 订货-Post批量导入商品
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [ActionPurview(true)]
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
            List<Tb_JhShopsp_Import> list = new List<Tb_JhShopsp_Import>();
            List<Tb_JhShopsp_Import> successList = new List<Tb_JhShopsp_Import>();
            List<Tb_JhShopsp_Import> failList = new List<Tb_JhShopsp_Import>();
            try
            {
                if (!table.Columns.Contains("系统备注"))
                    table.Columns.Add("系统备注", typeof(System.String));
                if (!table.Columns.Contains("id"))
                    table.Columns.Add("id", typeof(System.String));
                list = this.TurnJhShopSPImportList(table);
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

            ProccessData(filePath, ref list, ref successList, ref failList, param["id_shop"].ToString());

            if (failList != null && failList.Count() > 0)
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
        #endregion

        #region 订货-Post批量导入商品-辅助方法

        #region TurnJhShopSPImportList
        private List<Tb_JhShopsp_Import> TurnJhShopSPImportList(DataTable table)
        {
            List<Tb_JhShopsp_Import> list = new List<Tb_JhShopsp_Import>();
            foreach (DataRow item in table.Rows)
            {
                Tb_JhShopsp_Import model = new Tb_JhShopsp_Import();
                model.barcode = item["条形码"] == null ? "" : item["条形码"].ToString();
                model.mc = item["商品名称"] == null ? "" : item["商品名称"].ToString();
                decimal sl = 0;
                decimal.TryParse(item["数量"] == null ? "" : item["数量"].ToString(), out sl);
                model.sl = sl;
                decimal dj = 0;
                decimal.TryParse(item["单价"] == null ? "" : item["单价"].ToString(), out dj);
                model.dj = dj;
                model.bz = item["备注"] == null ? "" : item["备注"].ToString();
                model.sysbz = "";
                list.Add(model);
            }
            return list;
        }
        #endregion

        #region 保存本地临时文件
        /// <summary>
        /// 保存本地临时文件
        /// </summary>
        /// <param name="failList"></param>
        /// <returns></returns>
        private string SaveFailFile(List<Tb_JhShopsp_Import> failList)
        {
            try
            {
                string fileName = "订货商品导入失败数据" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
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

                ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "订货商品资料模版");
                int i = 1;
                foreach (var item in failList)
                {
                    IRow rowtemp = sheet1.CreateRow(i);
                    rowtemp.CreateCell(0).SetCellValue(item.barcode);
                    rowtemp.CreateCell(1).SetCellValue(item.mc);
                    rowtemp.CreateCell(2).SetCellValue(Decimal.Round((decimal)(item.sl), 2).ToString());
                    rowtemp.CreateCell(3).SetCellValue(Decimal.Round((decimal)(item.dj), 2).ToString());
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

        #region 订货商品数据处理
        /// <summary>
        /// 订货商品数据处理
        /// </summary>
        /// <param name="list"></param>
        /// <param name="successList"></param>
        /// <param name="failList"></param>
        private void ProccessData(string filePath, ref List<Tb_JhShopsp_Import> list, ref List<Tb_JhShopsp_Import> successList, ref List<Tb_JhShopsp_Import> failList, string id_shop_sel = "")
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            param.Add("filePath", filePath);
            param.Add("list", list);
            param.Add("id_masteruser", id_user_master);
            param.Add("id_user", id_user);
            if (!string.IsNullOrEmpty(id_shop_sel))
                param.Add("id_shop", id_shop_sel);
            else
                param.Add("id_shop", id_shop);
            br = BusinessFactory.Td_Jh_1.ImportIn(param);
            if (br.Success)
            {
                Tb_JhShopsp_Import_All rModel = (Tb_JhShopsp_Import_All)br.Data;
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
        #endregion

        #endregion

        #region 订货-商品导出模板
        /// <summary>
        /// 订货-商品导出模板
        /// lz
        /// 2016-09-25
        /// </summary>
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
                   {"单价",20},
                   {"备注",20}
                };

                Dictionary<string, string> e_param_comment = new Dictionary<string, string> {
                   {"条形码","规则:\r\n1.必填 "},
                   {"数量","规则:\r\n1.必填 "}
                };

                ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "订货商品资料模板");

                IRow rowtemp = sheet1.CreateRow(1);
                rowtemp.CreateCell(0).SetCellValue("16092318171654");
                rowtemp.CreateCell(1).SetCellValue("卡夫王子脆脆多膨化食品甜橙味35G");
                rowtemp.CreateCell(2).SetCellValue(Decimal.Round((decimal)(1), 2).ToString());
                rowtemp.CreateCell(3).SetCellValue(Decimal.Round((decimal)(100), 2).ToString());
                rowtemp.CreateCell(4).SetCellValue("2016-09-24订货商品");
                sheet1.GetRow(1).Height = 28 * 20;

                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "订货商品资料模板-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");

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

        #region 订货-导出
        /// <summary>
        /// 订货-导出
        /// lz
        /// 2016-09-26
        /// </summary>
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

            ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "订货商品资料导出");
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
                    rowtemp.CreateCell(2).SetCellValue(Decimal.Round((decimal)(0), 2).ToString());
                    rowtemp.CreateCell(3).SetCellValue(Decimal.Round((decimal)(0), 2).ToString());
                    rowtemp.CreateCell(4).SetCellValue(Decimal.Round((decimal)(0), 2).ToString());
                    rowtemp.CreateCell(5).SetCellValue("");
                    rowtemp.CreateCell(6).SetCellValue("无有效数据");
                    sheet1.GetRow(1).Height = 28 * 20;
                    // 写入到客户端 
                    book.Write(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    return File(ms, "application/vnd.ms-excel", "订货商品资料导出-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
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
                    rowtempdata.CreateCell(2).SetCellValue(Decimal.Round((decimal)(item.sl), int.Parse(digit["sl_digit"].ToString())).ToString());
                    rowtempdata.CreateCell(3).SetCellValue(Decimal.Round((decimal)(item.dj), int.Parse(digit["dj_digit"].ToString())).ToString());
                    rowtempdata.CreateCell(4).SetCellValue(Decimal.Round((decimal)(item.je), int.Parse(digit["je_digit"].ToString())).ToString());
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
                return File(ms, "application/vnd.ms-excel", "订货商品资料导出-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
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
                rowtempe.CreateCell(6).SetCellValue("导出出现异常 请重试");
                sheet1.GetRow(1).Height = 28 * 20;
                // 写入到客户端 
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "商品资料导出-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
                #endregion
            }
        }
        #endregion

        #region 订货-是否允许进货
        /// <summary>
        ///订货-是否允许进货
        /// lz 2016-12-26
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult AllowJH()
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            param.Add("id_masteruser", id_user_master);
            param.Add("id_user", id_user);
            pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
            pv.Add("id_user", string.Empty, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            try
            {
                param_model = param.Trim(pv);
                br = BusinessFactory.Td_Jh_Dd_1.Export(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            return JsonString(br, 1);
        }
        #endregion

    }
}
