using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Common;
using CySoft.Frame.Core;
using CySoft.Model;
using CySoft.Model.Enums;
using CySoft.Model.Flags;
using CySoft.Model.Tb;
using CySoft.Model.Td;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Net;
using CySoft.Model.Other;

namespace CySoft.Controllers.SpCtl
{
    //门店商品
    [LoginActionFilter]
    public class ShopSpController : BaseController
    {
        #region 商品列表
        /// <summary>
        /// 商品列表
        /// </summary>
        [ValidateInput(false)]
        [ActionPurview(true)]
        public ActionResult List()
        {
            try
            {
                #region 辅助参数
                PageNavigate pn = new PageNavigate();

                var stateBr = base.GetFlagList(Enums.TsFlagListCode.spstate.ToString());
                if (stateBr.Success)
                    ViewBag.selectListState = stateBr.Data;

                var czfsBr = base.GetFlagList(Enums.TsFlagListCode.spczfs.ToString());
                if (czfsBr.Success)
                    ViewBag.selectListCZFS = czfsBr.Data;

                var spflBr = base.GetSPFLJsonStr();
                if (spflBr.Success)
                    ViewBag.selectListSPFL = spflBr.Data.ToString();

                Hashtable ht = new Hashtable();
                ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                ht.Add("id_masteruser", id_user_master);
                var shopBr = BusinessFactory.Tb_Shop.GetPage(ht);
                if (shopBr.Success)
                    ViewBag.selectListShop = shopBr.Data;

                //获取前台控制小数点
                ViewBag.DigitHashtable = GetParm();

                BaseResult br = new BaseResult();
                Hashtable param = base.GetParameters();
                #endregion

                #region 验证参数
                if (string.IsNullOrEmpty(id_user_master))
                {
                    br.Message.Add(string.Format("参数解析错误！请重新刷新页面。"));
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
                #endregion

                #region 构建param
                ParamVessel p = new ParamVessel();
                //排序字段
                p.Add("special_order_field", "rq_create", HandleType.DefaultValue);
                //排序方式
                p.Add("special_order_descasc", "desc", HandleType.DefaultValue);
                //声明周期
                p.Add("flag_state", (byte)0, HandleType.Remove);
                //商品分类
                p.Add("id_spfl", String.Empty, HandleType.Remove);
                //商品门店
                p.Add("id_shop", String.Empty, HandleType.Remove);
                //搜素关键字
                p.Add("keyword", String.Empty, HandleType.Remove, true);
                //计价方式
                p.Add("flag_czfs", (byte)0, HandleType.Remove);
                //是否页面查询标志
                p.Add("_search_", "0", HandleType.DefaultValue);
                //当前页码
                p.Add("page", 0, HandleType.DefaultValue);
                //每页大小
                p.Add("pageSize", base.PageSizeFromCookie, HandleType.DefaultValue);
                param = param.Trim(p);
                int page = Convert.ToInt32(param["page"]);
                int pageSize = Convert.ToInt32(param["pageSize"]);
                //排序方式
                param.Add("dir", param["special_order_descasc"].ToString());
                //排序字段
                param.Add("sort", param["special_order_field"].ToString());

                param.Remove("special_order_descasc");
                param.Remove("special_order_field");

                if (param.ContainsKey("id_spfl") && param["id_spfl"].ToString() == "0")
                    param.Remove("id_spfl");


                //关键词
                ViewData["keyword"] = param["keyword"];


                //主用户id
                param.Add("id_masteruser", id_user_master);
                //每页限制
                param.Add("limit", pageSize);
                //开始分页起点
                param.Add("start", page * pageSize);
                //是否删除
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                //自己门店
                param.Add("id_shop", id_shop);
                #endregion

                #region 获取数据
                PageList<Tb_Shopsp_Query> list = new PageList<Tb_Shopsp_Query>(pageSize);
                pn = BusinessFactory.Tb_Shopsp.GetPage(param);
                list = new PageList<Tb_Shopsp_Query>(pn, page, pageSize);

                ViewData["list_shopsp"] = list;

                if (param.ContainsKey("_search_") && param["_search_"].ToString() == "1")
                    return PartialView("_List");
                else
                    return View("List");
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

        #region 新增商品
        /// <summary>
        /// 新增商品 UI 
        /// lz
        /// 2016-07-29
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Add()
        {
            #region 获取参数
            Hashtable param = base.GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("id", string.Empty, HandleType.Remove);//id
            p.Add("from", string.Empty, HandleType.DefaultValue);//来源页面
            p.Add("page", "0", HandleType.DefaultValue);//页码
            p.Add("id_spfl", string.Empty, HandleType.Remove);//id_spfl
            p.Add("dw", string.Empty, HandleType.Remove);//dw
            param = param.Trim(p);
            param.Add("id_masteruser", id_user_master);
            #endregion

            #region 商品状态 下拉值
            var stateBr = base.GetFlagList(Enums.TsFlagListCode.spstate.ToString());
            if (stateBr.Success)
                ViewBag.selectListState = stateBr.Data;
            #endregion

            #region 计价方式 下拉值
            //计价方式 下拉值
            var czfsBr = base.GetFlagList(Enums.TsFlagListCode.spczfs.ToString());
            if (czfsBr.Success)
                ViewBag.selectListCZFS = czfsBr.Data;
            #endregion

            #region 商品单位 下拉值
            //商品单位 下拉值
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", id_user_master);
            var dwListBr = BusinessFactory.Tb_Dw.GetAll(ht);
            if (dwListBr.Success)
                ViewBag.selectListDW = dwListBr.Data;
            #endregion

            #region 商品分类JsonStr
            //商品分类JsonStr
            var spflBr = base.GetSPFLJsonStr();
            if (spflBr.Success)
                ViewBag.selectListSPFLJsonStr = spflBr.Data.ToString();
            #endregion

            #region 前台控制小数点参数
            //前台控制小数点参数
            ViewBag.DigitHashtable = GetParm();
            #endregion

            #region 商品门店 下拉值
            //商品门店 下拉值
            ht.Clear();
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            ht.Add("id_masteruser", id_user_master);
            var shopBr = BusinessFactory.Tb_Shop.GetPage(ht);
            if (shopBr.Success)
                ViewBag.selectListShop = shopBr.Data;
            #endregion

            #region 页面辅助参数
            ViewBag.from = param["from"].ToString();
            ViewBag.page = param["page"].ToString();
            ViewBag.showDBZ = "1";//显示多包装
            ViewBag.editDBZ = "1";//可编辑多包装
            ViewBag.editQC = "1";//可编期初
            ViewBag.viewType = "Add";//页面标识 
            if (param.ContainsKey("id_spfl")&&!string.IsNullOrEmpty(param["id_spfl"].ToString()))
                ViewBag.id_spfl = param["id_spfl"].ToString();
            if (param.ContainsKey("dw") && !string.IsNullOrEmpty(param["dw"].ToString()))
                ViewBag.dw = param["dw"].ToString();
            #endregion

            return View("Add");
        }

        /// <summary>
        /// 新增商品 
        /// lz
        /// 2016-07-29
        /// 测试:localhost:47398/ShopSp/AddGoods?barcode=111&mc=111&cd=广东&dj_jh=100&dj_ls=100&dw=包&sl_kc_min=100&sl_kc_max=10000&pic_path=../../1.jpg&id_spfl=1&dbzList=[{"barcode":"222","mc":"222","dw":"条","zhl":"10",dj_ls:"1000",dj_jh:"1000"},{"barcode":"333","mc":"333","dw":"箱","zhl":"100",dj_ls:"10000",dj_jh:"10000"}]&sp_qc={"sl_qc":"1"}&flag_czfs=1
        /// </summary>
        [ValidateInput(false)]
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
                p.Add("barcode", string.Empty, HandleType.DefaultValue);//条码 --
                p.Add("bm", string.Empty, HandleType.DefaultValue);//bm--
                p.Add("mc", string.Empty, HandleType.DefaultValue);//名称--
                p.Add("cd", string.Empty, HandleType.DefaultValue);//产地
                p.Add("dj_jh", 0m, HandleType.ReturnMsg);//进货价--
                p.Add("dj_ls", 0m, HandleType.ReturnMsg);//零售价--
                p.Add("dw", string.Empty, HandleType.DefaultValue);//单位--
                p.Add("sl_kc_min", 0m, HandleType.DefaultValue);//最底库存量--
                p.Add("sl_kc_max", 0m, HandleType.DefaultValue);//最高库存量--
                p.Add("pic_path", string.Empty, HandleType.DefaultValue);//图片路径
                p.Add("id_spfl", string.Empty, HandleType.DefaultValue);//分类ID
                p.Add("dbzList", string.Empty, HandleType.DefaultValue);//多包装    
                p.Add("sp_qc", "{\"sl_qc\":\"0\"}", HandleType.DefaultValue);//初期
                p.Add("flag_czfs", string.Empty, HandleType.ReturnMsg);//计价方式--
                p.Add("dj_hy", 0m, HandleType.DefaultValue);//会员价--
                p.Add("id_gys", string.Empty, HandleType.DefaultValue);//id_gys
                p.Add("yxq", 0, HandleType.DefaultValue);//yxq
                p.Add("dj_ps", 0m, HandleType.DefaultValue);//dj_ps
                p.Add("dj_pf", 0m, HandleType.DefaultValue);//dj_pf
                p.Add("bz", string.Empty, HandleType.DefaultValue);//bz
                p.Add("flag_state", 1, HandleType.DefaultValue);//flag_state

                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                param.Add("id_shop", id_shop);
                param.Add("id_shop_master", id_shop_master);
                oldParam = (Hashtable)param.Clone();
                #endregion

                #region 获取参数多包装和期初

                #region 多包装
                //多包装
                var dbzList = new List<Tb_Shopsp_DBZ>();
                try
                {
                    dbzList = JSON.Deserialize<List<Tb_Shopsp_DBZ>>(param["dbzList"].ToString()) ?? new List<Tb_Shopsp_DBZ>();
                }
                catch (Exception ex)
                {
                    br.Success = false;
                    br.Data = new { id = "", name = "", value = "" };
                    br.Message.Add("多包装数据不符合规则");
                    br.Level = ErrorLevel.Warning;
                    return base.JsonString(new
                    {
                        data = br.Data,
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
                #endregion

                #region 期初
                //期初
                var qcModel = new Td_Sp_Qc();
                try
                {
                    qcModel = JSON.Deserialize<Td_Sp_Qc>(param["sp_qc"].ToString()) ?? new Td_Sp_Qc();
                }
                catch (Exception ex)
                {
                    br.Success = false;
                    br.Data = new { id = "", name = "", value = "" };
                    br.Message.Add("期初数据不符合规则");
                    br.Level = ErrorLevel.Warning;
                    return base.JsonString(new
                    {
                        data = br.Data,
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
                #endregion

                #endregion

                #region 验证数据
                //控制层验证数据
                var brCheck = this.CheckParam(param, qcModel, dbzList);
                if (!brCheck.Success)
                {
                    return base.JsonString(new
                    {
                        data = brCheck.Data,
                        status = "error",
                        message = string.Join(";", brCheck.Message)
                    });
                }
                #endregion

                #region 数据处理
                // 图片 
                if (param["pic_path"] != null && !string.IsNullOrEmpty(param["pic_path"].ToString()))
                {
                    CheckImgPath();
                    string[] url_img = param["pic_path"].ToString().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    string guid = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(url_img[url_img.Length - 1]);
                    string fileName = guid + extension;
                    ImgExtension.MakeThumbnail(param["pic_path"].ToString(), "/UpLoad/Goods/thumb/_480x480_" + fileName, 480, 480, ImgCreateWay.Cut, false);
                    string newPath = string.Format("/UpLoad/Goods/thumb/_480x480_{0}", fileName);
                    ;
                    ; //480x480
                    param.Remove("pic_path");
                    param.Add("pic_path", newPath);
                }

                param.Remove("dbzList");
                param.Add("dbzList", dbzList);
                param.Remove("sp_qc");
                param.Add("sp_qc", qcModel);
                #endregion

                #region 执行添加并返回结果
                var shopShop = GetShop(Enums.ShopDataType.ShopShopAll);
                param.Add("shop_shop", shopShop);

                var DigitHashtable = GetParm();

                if (!param.ContainsKey("DigitHashtable"))
                    param.Add("DigitHashtable", DigitHashtable);

                br = BusinessFactory.Tb_Shopsp.Add(param);
                WriteDBLog("新增商品", oldParam, br);

                if (br.Success)
                {
                    return base.JsonString(new
                    {
                        data = br.Data,
                        status = "success",
                        message = "执行成功,正在载入页面..."
                    });
                }
                else
                {
                    return base.JsonString(new
                    {
                        data = br.Data,
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
                #endregion
            }
            catch (CySoftException ex)
            {
                br.Success = false;
                br.Data = new { id = "", name = "", value = "" };
                br.Message.Add("数据不符合要求!");
                br.Level = ErrorLevel.Warning;
                WriteDBLog("新增商品", oldParam, br);
                return base.JsonString(new
                {
                    data = br.Data,
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Data = new { id = "", name = "", value = "" };
                br.Message.Add("数据不符合要求!");
                br.Level = ErrorLevel.Warning;
                WriteDBLog("新增商品", oldParam, br);
                return base.JsonString(new
                {
                    data = br.Data,
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }
        }

        #endregion

        #region 复制商品
        /// <summary>
        /// 复制商品 UI 
        /// lz
        /// 2016-11-30
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Copy()
        {
            #region 获取参数
            Hashtable param = base.GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("id", string.Empty, HandleType.Remove);//id
            p.Add("from", string.Empty, HandleType.DefaultValue);//来源页面
            p.Add("page", "0", HandleType.DefaultValue);//页码
            param = param.Trim(p);
            param.Add("id_masteruser", id_user_master);
            #endregion

            #region 商品状态 下拉值
            var stateBr = base.GetFlagList(Enums.TsFlagListCode.spstate.ToString());
            if (stateBr.Success)
                ViewBag.selectListState = stateBr.Data;
            #endregion

            #region 计价方式 下拉值
            //计价方式 下拉值
            var czfsBr = base.GetFlagList(Enums.TsFlagListCode.spczfs.ToString());
            if (czfsBr.Success)
                ViewBag.selectListCZFS = czfsBr.Data;
            #endregion

            #region 商品分类JsonStr
            //商品分类JsonStr
            var spflBr = base.GetSPFLJsonStr();
            if (spflBr.Success)
                ViewBag.selectListSPFLJsonStr = spflBr.Data.ToString();
            #endregion

            #region 商品单位 下拉值
            //商品单位 下拉值
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", id_user_master);
            var dwListBr = BusinessFactory.Tb_Dw.GetAll(ht);
            if (dwListBr.Success)
                ViewBag.selectListDW = dwListBr.Data;
            #endregion

            #region 前台控制小数点参数
            //获取前台控制小数点参数
            ViewBag.DigitHashtable = GetParm();
            #endregion

            #region 商品门店 下拉值
            //商品门店 下拉值
            ht.Clear();
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            ht.Add("id_masteruser", id_user_master);
            var shopBr = BusinessFactory.Tb_Shop.GetPage(ht);
            if (shopBr.Success)
                ViewBag.selectListShop = shopBr.Data;
            #endregion

            #region 获取复制对象实体
            if (param.ContainsKey("id") && !string.IsNullOrEmpty(param["id"].ToString()))
            {
                ht.Clear();
                ht.Add("id", param["id"].ToString());
                ht.Add("need_query_spfl", "No");
                ht.Add("need_query_shop", "No");
                ht.Add("need_query_kc", "No");
                ht.Add("need_query_dbz", "Yes");
                ht.Add("need_query_qc", "Yes");

                ht.Add("id_user", id_user);
                ht.Add("id_masteruser", id_user_master);
                var copySPBr = BusinessFactory.Tb_Shopsp.Get(ht);
                if (copySPBr.Success)
                {
                    var data = (Tb_Shopsp_Get)copySPBr.Data;
                    if (data.ShopSP != null)
                    {
                        data.ShopSP.barcode = "";
                        data.ShopSP.bm = "";
                    }
                    foreach (var item in data.ShopSP_DBZ)
                    {
                        item.barcode = "";
                        item.bm = "";
                    }
                    ViewBag.Tb_Shopsp_Get = data;
                }
            }
            #endregion

            #region 页面辅助参数
            ViewBag.from = param["from"].ToString();
            ViewBag.page = param["page"].ToString();
            ViewBag.showDBZ = "1";//显示多包装
            ViewBag.editDBZ = "1";//可编辑多包装
            ViewBag.editQC = "1";//可编期初
            ViewBag.viewType = "Copy";//页面标识 
            #endregion

            return View("Add");

        }

        #endregion

        #region 修改商品

        #region 修改商品
        /// <summary>
        /// 修改商品
        /// lz
        /// 2016-08-04
        [ValidateInput(true)]
        [HttpGet]
        public ActionResult Edit()
        {
            #region 获取参数
            Hashtable param = base.GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("id", string.Empty, HandleType.Remove);//id
            p.Add("from", string.Empty, HandleType.DefaultValue);//来源页面
            p.Add("page", "0", HandleType.DefaultValue);//页码
            param = param.Trim(p);
            param.Add("id_masteruser", id_user_master);
            #endregion

            #region 商品状态 下拉值
            var stateBr = base.GetFlagList(Enums.TsFlagListCode.spstate.ToString());
            if (stateBr.Success)
                ViewBag.selectListState = stateBr.Data;
            #endregion

            #region 计价方式 下拉值
            //计价方式 下拉值
            var czfsBr = base.GetFlagList(Enums.TsFlagListCode.spczfs.ToString());
            if (czfsBr.Success)
                ViewBag.selectListCZFS = czfsBr.Data;
            #endregion

            #region 商品分类JsonStr
            //商品分类JsonStr
            var spflBr = base.GetSPFLJsonStr();
            if (spflBr.Success)
                ViewBag.selectListSPFLJsonStr = spflBr.Data.ToString();
            #endregion

            #region 商品单位 下拉值
            //商品单位 下拉值
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", id_user_master);
            var dwListBr = BusinessFactory.Tb_Dw.GetAll(ht);
            if (dwListBr.Success)
                ViewBag.selectListDW = dwListBr.Data;
            #endregion

            #region 前台控制小数点参数
            //获取前台控制小数点参数
            ViewBag.DigitHashtable = GetParm();
            #endregion

            #region 商品门店 下拉值
            //商品门店 下拉值
            ht.Clear();
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            ht.Add("id_masteruser", id_user_master);
            var shopBr = BusinessFactory.Tb_Shop.GetPage(ht);
            if (shopBr.Success)
                ViewBag.selectListShop = shopBr.Data;
            #endregion

            #region 获取复制对象实体
            if (param.ContainsKey("id") && !string.IsNullOrEmpty(param["id"].ToString()))
            {
                ht.Clear();
                ht.Add("id", param["id"].ToString());
                ht.Add("need_query_spfl", "No");
                ht.Add("need_query_shop", "No");
                ht.Add("need_query_kc", "No");
                ht.Add("need_query_dbz", "Yes");
                ht.Add("need_query_qc", "Yes");
                ht.Add("id_user", id_user);
                ht.Add("id_masteruser", id_user_master);
                var copySPBr = BusinessFactory.Tb_Shopsp.Get(ht);
                if (copySPBr.Success)
                {
                    var data = (Tb_Shopsp_Get)copySPBr.Data;
                    if (data != null && data.ShopSP != null && !string.IsNullOrEmpty(data.ShopSP.id) && data.ShopSP.id_sp == data.ShopSP.id_kcsp)
                    //if (data != null && data.ShopSP != null && !string.IsNullOrEmpty(data.ShopSP.id) && data.ShopSP_DBZ != null && data.ShopSP_DBZ.Count() > 0)
                    {
                        ViewBag.showDBZ = "1";
                        ViewBag.editDBZ = "1";

                        if (data.Qc == null || string.IsNullOrEmpty(data.Qc.id))
                            ViewBag.editQC = "1";
                    }

                    ViewBag.Tb_Shopsp_Get = data;
                }
            }
            #endregion

            #region 页面辅助参数
            ViewBag.from = param["from"].ToString();
            ViewBag.page = param["page"].ToString();
            ViewBag.viewType = "Edit";//页面标识 
            #endregion

            return View("Add");
        }

        #endregion

        #region 修改商品
        /// <summary>
        /// 修改商品
        /// lz 2016-08-04
        /// </summary>
        [ValidateInput(true)]
        [HttpPost]
        public ActionResult Edit(Tb_Shopsp model)
        {
            BaseResult br = new BaseResult();

            var oldParam = new Hashtable();

            br.Success = true;
            try
            {
                #region 获取数据
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", string.Empty, HandleType.ReturnMsg);//ID
                p.Add("barcode", string.Empty, HandleType.DefaultValue);//条码 --
                p.Add("bm", string.Empty, HandleType.DefaultValue);//bm--
                p.Add("mc", string.Empty, HandleType.DefaultValue);//名称--
                p.Add("cd", string.Empty, HandleType.DefaultValue);//产地
                p.Add("dj_jh", 0m, HandleType.ReturnMsg);//进货价--
                p.Add("dj_ls", 0m, HandleType.ReturnMsg);//零售价--
                p.Add("dw", string.Empty, HandleType.DefaultValue);//单位--
                p.Add("sl_kc_min", 0m, HandleType.DefaultValue);//最底库存量--
                p.Add("sl_kc_max", 0m, HandleType.DefaultValue);//最高库存量--
                p.Add("pic_path", string.Empty, HandleType.DefaultValue);//图片路径
                p.Add("id_spfl", string.Empty, HandleType.DefaultValue);//分类ID
                p.Add("dbzList", string.Empty, HandleType.DefaultValue);//多包装    
                p.Add("sp_qc", "{\"sl_qc\":\"0\"}", HandleType.DefaultValue);//初期
                p.Add("flag_czfs", string.Empty, HandleType.ReturnMsg);//计价方式--
                p.Add("dj_hy", 0m, HandleType.DefaultValue);//会员价--
                p.Add("id_gys", string.Empty, HandleType.DefaultValue);//id_gys
                p.Add("yxq", 0, HandleType.ReturnMsg);//yxq
                p.Add("dj_ps", 0m, HandleType.DefaultValue);//dj_ps
                p.Add("dj_pf", 0m, HandleType.DefaultValue);//dj_pf
                p.Add("bz", string.Empty, HandleType.DefaultValue);//bz
                p.Add("flag_state", 1, HandleType.DefaultValue);//flag_state
                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);

                param.Add("id_shop", id_shop);
                param.Add("id_shop_master", id_shop_master);
                oldParam = (Hashtable)param.Clone();
                #endregion

                #region 获取参数多包装和期初

                #region 多包装
                //多包装
                var dbzList = new List<Tb_Shopsp_DBZ>();
                try
                {
                    dbzList = JSON.Deserialize<List<Tb_Shopsp_DBZ>>(param["dbzList"].ToString()) ?? new List<Tb_Shopsp_DBZ>();
                }
                catch (Exception ex)
                {
                    br.Success = false;
                    br.Data = new { id = "", name = "", value = "" };
                    br.Message.Add("多包装数据不符合规则");
                    br.Level = ErrorLevel.Warning;
                    return base.JsonString(new
                    {
                        data = br.Data,
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
                #endregion

                #region 期初
                //期初
                var qcModel = new Td_Sp_Qc();
                try
                {
                    qcModel = JSON.Deserialize<Td_Sp_Qc>(param["sp_qc"].ToString()) ?? new Td_Sp_Qc();
                }
                catch (Exception ex)
                {
                    br.Success = false;
                    br.Data = new { id = "", name = "", value = "" };
                    br.Message.Add("期初数据不符合规则");
                    br.Level = ErrorLevel.Warning;
                    return base.JsonString(new
                    {
                        data = br.Data,
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
                #endregion

                #endregion

                #region 验证数据
                //控制层验证数据
                var brCheck = this.CheckParam(param, qcModel, dbzList);
                if (!brCheck.Success)
                {
                    return base.JsonString(new
                    {
                        data = brCheck.Data,
                        status = "error",
                        message = string.Join(";", brCheck.Message)
                    });
                }
                #endregion

                #region 数据处理
                // 图片 
                if (param["pic_path"] != null && !string.IsNullOrEmpty(param["pic_path"].ToString()))
                {
                    CheckImgPath();
                    if (!param["pic_path"].ToString().Contains("/UpLoad/Goods/thumb/"))
                    {
                        string[] url_img = param["pic_path"].ToString().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        string guid = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(url_img[url_img.Length - 1]);
                        string fileName = guid + extension;
                        ImgExtension.MakeThumbnail(param["pic_path"].ToString(), "/UpLoad/Goods/thumb/_480x480_" + fileName, 480, 480, ImgCreateWay.Cut, false);
                        string newPath = string.Format("/UpLoad/Goods/thumb/_480x480_{0}", fileName);
                        //480x480
                        param.Remove("pic_path");
                        param.Add("pic_path", newPath);
                    }
                }
                param.Remove("dbzList");
                param.Add("dbzList", dbzList);
                param.Remove("sp_qc");
                param.Add("sp_qc", qcModel);

                var shopShop = GetShop(Enums.ShopDataType.ShopShopAll);
                param.Add("shop_shop", shopShop);
                #endregion

                #region 更新数据库
                br = BusinessFactory.Tb_Shopsp.Save(param);
                WriteDBLog("修改商品", oldParam, br);
                if (br.Success)
                {
                    return base.JsonString(new
                    {
                        data = br.Data,
                        status = "success",
                        message = "执行成功,正在载入页面..."
                    });
                }
                else
                {
                    return base.JsonString(new
                    {
                        data = br.Data,
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
                #endregion

            }
            catch (CySoftException ex)
            {
                br.Success = false;
                br.Data = ex.Result.Data;
                br.Message.Add(ex.Result.Message.FirstOrDefault());
                br.Level = ErrorLevel.Warning;
                WriteDBLog("修改商品", oldParam, br);
                return base.JsonString(new
                {
                    data = br.Data,
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Data = new { id = "", name = "", value = "" };
                br.Message.Add("数据不符合要求!");
                br.Level = ErrorLevel.Warning;
                WriteDBLog("修改商品", oldParam, br);
                return base.JsonString(new
                {
                    data = br.Data,
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }
        }
        #endregion

        #region 批量修改商品
        /// <summary>
        /// 批量修改商品
        /// lz 2016-08-04
        /// </summary>
        //[HttpPost]
        [ActionPurview(false)]
        public ActionResult BatchEdit()
        {
            BaseResult br = new BaseResult();

            var oldParam = new Hashtable();


            br.Success = true;
            try
            {
                #region 获取并验证数据
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("spIDs", string.Empty, HandleType.ReturnMsg);//商品id
                p.Add("flag_state", string.Empty, HandleType.Remove);//flag_state
                p.Add("sl_kc_min", string.Empty, HandleType.Remove);//sl_kc_min
                p.Add("sl_kc_max", string.Empty, HandleType.Remove);//sl_kc_max
                p.Add("id_spfl", string.Empty, HandleType.Remove);//id_spfl

                p.Add("dj_ls", string.Empty, HandleType.Remove);//dj_ls
                p.Add("dj_jh", string.Empty, HandleType.Remove);//dj_jh
                p.Add("dj_hy", string.Empty, HandleType.Remove);//dj_hy
                p.Add("dj_ps", string.Empty, HandleType.Remove);//dj_ps
                p.Add("dj_pf", string.Empty, HandleType.Remove);//dj_pf

                param = param.Trim(p);

                if (param.Count < 2)
                {
                    br.Success = false;
                    br.Message.Add(string.Format("数据出现异常!"));
                    return Json(br);
                }
                param.Add("id_user", id_user);
                param.Add("id_shop_master", id_shop_master);
                param.Add("id_shop", id_shop);
                oldParam = (Hashtable)param.Clone();
                var shopShop = GetShop(Enums.ShopDataType.ShopShopAll);
                param.Add("shop_shop", shopShop);

                #endregion
                br = BusinessFactory.Tb_Shopsp.BatchSave(param);
                WriteDBLog("批量修改商品", oldParam, br);
                if (br.Success)
                {
                    return base.JsonString(new
                    {
                        status = "success",
                        message = "执行成功,正在载入页面..."
                    });
                }
                else
                {
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
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
        }
        #endregion

        #endregion

        #region 删除商品
        /// <summary>
        /// 删除商品
        /// lz 2016-08-03
        /// </summary>
        //[HttpPost]
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            var oldParam = new Hashtable();

            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("spIDs", string.Empty, HandleType.ReturnMsg);//商品id
                param = param.Trim(p);
                param.Add("id_user", id_user);
                param.Add("id_masteruser", id_user_master);
                oldParam = (Hashtable)param.Clone();
                br = BusinessFactory.Tb_Shopsp.Delete(param);
                WriteDBLog("删除商品", oldParam, br);
                if (br.Success)
                {
                    return base.JsonString(new
                    {
                        status = "success",
                        message = "执行成功,正在载入页面..."
                    });
                }
                else
                {
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
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
        }
        #endregion

        #region 商品详细
        /// <summary>
        /// 商品详细
        /// lz
        /// 2016-08-04
        /// </summary>
        //[ValidateInput(true)]
        //[ActionPurview(true)]
        [ActionAlias("shopsp", "list")]
        public ActionResult Detail()
        {
            #region 获取参数
            Hashtable param = base.GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("id", string.Empty, HandleType.Remove);//id
            p.Add("from", string.Empty, HandleType.DefaultValue);//来源页面
            p.Add("page", "0", HandleType.DefaultValue);//页码
            param = param.Trim(p);
            param.Add("id_masteruser", id_user_master);
            #endregion

            #region 计价方式 下拉值
            //计价方式 下拉值
            var czfsBr = base.GetFlagList(Enums.TsFlagListCode.spczfs.ToString());
            if (czfsBr.Success)
                ViewBag.selectListCZFS = czfsBr.Data;
            #endregion

            #region 商品分类JsonStr
            //商品分类JsonStr
            var spflBr = base.GetSPFLJsonStr();
            if (spflBr.Success)
                ViewBag.selectListSPFLJsonStr = spflBr.Data.ToString();
            #endregion

            #region 商品状态 下拉值
            var stateBr = base.GetFlagList(Enums.TsFlagListCode.spstate.ToString());
            if (stateBr.Success)
                ViewBag.selectListState = stateBr.Data;
            #endregion

            #region 商品单位 下拉值
            //商品单位 下拉值
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", id_user_master);
            var dwListBr = BusinessFactory.Tb_Dw.GetAll(ht);
            if (dwListBr.Success)
                ViewBag.selectListDW = dwListBr.Data;
            #endregion

            #region 前台控制小数点参数
            //获取前台控制小数点参数
            ViewBag.DigitHashtable = GetParm();
            #endregion

            #region 商品门店 下拉值
            //商品门店 下拉值
            ht.Clear();
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            ht.Add("id_masteruser", id_user_master);
            var shopBr = BusinessFactory.Tb_Shop.GetPage(ht);
            if (shopBr.Success)
                ViewBag.selectListShop = shopBr.Data;
            #endregion

            #region 获取复制对象实体
            if (param.ContainsKey("id") && !string.IsNullOrEmpty(param["id"].ToString()))
            {
                ht.Clear();
                ht.Add("id", param["id"].ToString());
                ht.Add("need_query_spfl", "No");
                ht.Add("need_query_shop", "No");
                ht.Add("need_query_kc", "No");
                ht.Add("need_query_dbz", "Yes");
                ht.Add("need_query_qc", "Yes");
                ht.Add("id_user", id_user);
                ht.Add("id_masteruser", id_user_master);
                var copySPBr = BusinessFactory.Tb_Shopsp.Get(ht);
                if (copySPBr.Success)
                {
                    var data = (Tb_Shopsp_Get)copySPBr.Data;
                    ViewBag.Tb_Shopsp_Get = data;
                    //if (data != null && data.ShopSP != null && !string.IsNullOrEmpty(data.ShopSP.id) && data.ShopSP.id == data.ShopSP.id_kcsp)
                    if (data != null && data.ShopSP != null && !string.IsNullOrEmpty(data.ShopSP.id) && data.ShopSP_DBZ != null && data.ShopSP_DBZ.Count() > 0)
                    {
                        ViewBag.showDBZ = "1";
                    }
                }
            }
            #endregion

            #region 页面辅助参数
            ViewBag.from = param["from"].ToString();
            ViewBag.page = param["page"].ToString();
            ViewBag.editDBZ = "0";//可编辑多包装
            ViewBag.editQC = "0";//可编期初
            ViewBag.viewType = "Detail";//页面标识 
            #endregion

            return View("Add");

            #region 注释
            //BaseResult br = new BaseResult();
            //try
            //{
            //    #region 获取并验证数据
            //    Hashtable param = base.GetParameters();
            //    ParamVessel p = new ParamVessel();
            //    p.Add("id", string.Empty, HandleType.ReturnMsg);//ID
            //    p.Add("from", string.Empty, HandleType.DefaultValue);//from
            //    p.Add("page", "0", HandleType.DefaultValue);
            //    param = param.Trim(p);
            //    param.Add("id_masteruser", id_user_master);
            //    param.Add("id_user", id_user);
            //    param.Add("need_query_spfl", "Yes");
            //    param.Add("need_query_shop", "Yes");
            //    param.Add("need_query_kc", "Yes");
            //    param.Add("need_query_qc", "Yes");
            //    param.Add("need_query_dbz", "Yes");
            //    #endregion

            //    #region MyRegion
            //    //br = BusinessFactory.Tb_Shopsp.Get(param);
            //    //if (br.Success)
            //    //    ViewBag.Tb_Shopsp_Get = br.Data;

            //    ////获取前台控制小数点
            //    //ViewBag.DigitHashtable = GetParm();
            //    ////计价方式下拉
            //    //var czfsBr = base.GetFlagList(Enums.TsFlagListCode.spczfs.ToString());
            //    //if (czfsBr.Success)
            //    //    ViewBag.selectListCZFS = czfsBr.Data; 
            //    #endregion

            //    #region 获取数据
            //    br = BusinessFactory.Tb_Shopsp.Get(param);
            //    if (br.Success)
            //    {
            //        var data = (Tb_Shopsp_Get)br.Data;
            //        if (data != null && data.ShopSP != null && !string.IsNullOrEmpty(data.ShopSP.id) && data.ShopSP.id == data.ShopSP.id_kcsp)
            //            ViewBag.showDBZ = "1";

            //        if (data.Qc == null || string.IsNullOrEmpty(data.Qc.id))
            //            ViewBag.showQC = "1";

            //        ViewBag.Tb_Shopsp_Get = data;
            //    }

            //    var stateBr = base.GetFlagList(Enums.TsFlagListCode.spstate.ToString());
            //    if (stateBr.Success)
            //        ViewBag.selectListState = stateBr.Data;

            //    var czfsBr = base.GetFlagList(Enums.TsFlagListCode.spczfs.ToString());
            //    if (czfsBr.Success)
            //        ViewBag.selectListCZFS = czfsBr.Data;

            //    Hashtable ht = new Hashtable();
            //    ht.Add("id_masteruser", id_user_master);
            //    var spflListBr = BusinessFactory.Tb_Spfl.GetAll(ht);
            //    if (spflListBr.Success)
            //        ViewBag.selectListSPFL = spflListBr.Data;

            //    ht.Clear();
            //    ht.Add("id_masteruser", id_user_master);
            //    var dwListBr = BusinessFactory.Tb_Dw.GetAll(ht);
            //    if (dwListBr.Success)
            //        ViewBag.selectListDW = dwListBr.Data;

            //    var spflBr = base.GetSPFLJsonStr();
            //    if (spflBr.Success)
            //        ViewBag.selectListSPFLJsonStr = spflBr.Data.ToString();

            //    //获取前台控制小数点
            //    ViewBag.DigitHashtable = GetParm();
            //    ViewBag.from = param["from"].ToString();
            //    ViewBag.page = param["page"].ToString();
            //    ViewBag.viewType = "Detail";//页面标识
            //    #endregion

            //}
            //catch (CySoftException ex)
            //{
            //    throw ex;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //} 

            //return View("Add");
            //return View("Item");
            #endregion

        }
        #endregion

        #region 商品导出模板
        /// <summary>
        /// 商品导出模板
        /// lz
        /// 2016-08-04
        /// </summary>
        [ActionPurview(false)]
        public FileResult DownloadExcelTemp()
        {
            try
            {
                HSSFWorkbook book = new HSSFWorkbook();
                Dictionary<string, int> e_param = new Dictionary<string, int> {
                   {"条码",20},
                   {"编码",20},
                   {"品名",40},
                   {"单位",20},
                   {"商品类别",25},
                   {"默认供应商",25},
                   {"保质期",25},
                   {"产地",40},
                   {"计价方式",20},
                   {"商品状态",20},
                   {"进货价",20},
                   {"零售价",20},
                   {"会员价",20},
                   {"配送价",20},
                   {"批发价",20},
                   {"最低库存量",20},
                   {"最高库存量",20},
                   {"期初金额",20},
                   {"期初数量",20}

                };

                Dictionary<string, string> e_param_comment = new Dictionary<string, string> {
                   {"条码","条码规则：\r\n1.不可为空 \r\n2.不能有\"+*%/\"等特殊字符，不能以\"_\"或\"-\"开头 \r\n3.长度不能超过20 "},
                   {"编码","编码规则：\r\n1.不可为空 \r\n2.不能有\"+*%/\"等特殊字符，不能以\"_\"或\"-\"开头 \r\n3.长度不能超过20 "},
                   {"品名","品名规则:\r\n1.不可为空 \r\n2.长度不能超过80 "},
                   {"单位","单位规则:\r\n1.不能为空 \r\n2.长度不能超过20 \r\n3.系统没有的单位会自动添加"},
                   {"商品类别","类别规则: \r\n1.商品类别名称\r\n2.不能为空 \r\n3.长度不能超过60 \r\n4.系统不存在的类别会自行添加到第一级"},
                   {"默认供应商","供应商规则: \r\n1.供应商名称\r\n2.长度不能超过60 \r\n3.系统不存在的供应商会自行添加"},
                   {"保质期","保质期规则:\r\n1.不可为空 \r\n2.必须为数字 单位为 天 "},
                   {"产地","产地规则:\r\n1.不可为空 \r\n2.长度不能超过40 "},
                   {"计价方式","计价方式规则: \r\n1.只能填写0-2之间的数字  0:普通  1:称重  2:计件 "},
                   {"商品状态","商品状态规则: \r\n1.只能填写0-1之间的数字  0:停用  1:正常  "},
                   {"进货价","进货价规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"零售价","零售价规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"会员价","会员价规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"配送价","配送价规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"批发价","批发价规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"最低库存量","最低库存量规则: \r\n1.只能填写0-99999999之间的数字 \r\n2.填0或不填则不处理初始库存 \r\n3.模版中可不指定该列"},
                   {"最高库存量","最高库存量规则: \r\n1.只能填写0-99999999之间的数字 \r\n2.填0或不填则不处理初始库存 \r\n3.模版中可不指定该列"},
                   {"期初金额","期初金额规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"期初数量","期初数量规则:\r\n1.只能填写0-99999999之间的数字 \r\n2.填0或不填则不处理初始库存 \r\n3.模版中可不指定该列"}
                };

                ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "商品资料模版");

                IRow rowtemp = sheet1.CreateRow(1);
                rowtemp.CreateCell(0).SetCellValue("6900873114188");
                rowtemp.CreateCell(1).SetCellValue("6900873114188");
                rowtemp.CreateCell(2).SetCellValue("卡夫王子脆脆多膨化食品甜橙味35G");
                rowtemp.CreateCell(3).SetCellValue("包");
                rowtemp.CreateCell(4).SetCellValue("零食");
                rowtemp.CreateCell(5).SetCellValue("天地集团");
                rowtemp.CreateCell(6).SetCellValue("360");
                rowtemp.CreateCell(7).SetCellValue("广东");
                rowtemp.CreateCell(8).SetCellValue(0);
                rowtemp.CreateCell(9).SetCellValue(1);
                rowtemp.CreateCell(10).SetCellValue(Decimal.Round((decimal)(3.55), 2).ToString());
                rowtemp.CreateCell(11).SetCellValue(Decimal.Round((decimal)(4.90), 2).ToString());
                rowtemp.CreateCell(12).SetCellValue(Decimal.Round((decimal)(4.11), 2).ToString());
                rowtemp.CreateCell(13).SetCellValue(Decimal.Round((decimal)(0.11), 2).ToString());
                rowtemp.CreateCell(14).SetCellValue(Decimal.Round((decimal)(3.12), 2).ToString());
                rowtemp.CreateCell(15).SetCellValue(100);
                rowtemp.CreateCell(16).SetCellValue(10000);
                rowtemp.CreateCell(17).SetCellValue(Decimal.Round((decimal)(3550), 2).ToString());
                rowtemp.CreateCell(18).SetCellValue(1000);

                sheet1.GetRow(1).Height = 28 * 20;

                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "商品资料模版-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");

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

        #region 商品导出数据
        /// <summary>
        /// 商品导出数据
        /// lz
        /// 2016-08-04
        /// </summary>
        [ActionPurview(false)]
        public FileResult Export()
        {
            try
            {
                BaseResult br = new BaseResult();
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("flag_state", (byte)0, HandleType.Remove);//声明周期
                p.Add("id_spfl", string.Empty, HandleType.Remove);//商品分类
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("flag_delete", (int)Enums.FlagDelete.NoDelete, HandleType.DefaultValue);//是否删除
                p.Add("id_masteruser", id_user_master, HandleType.DefaultValue);//主用户
                p.Add("spIDs", string.Empty, HandleType.Remove);//商品IDs
                param = param.Trim(p);

                if (param.ContainsKey("spIDs"))
                    param.Add("idList", param["spIDs"].ToString().Split(',').ToArray());

                if (param.ContainsKey("id_spfl") && param["id_spfl"].ToString() == "0")
                    param.Remove("id_spfl");

                if (!param.ContainsKey("id_shop"))
                    param.Add("id_shop", id_shop);

                if (!param.ContainsKey("flag_state"))
                    param.Add("flag_state", "1");

                br = BusinessFactory.Tb_Shopsp_Exportdata.GetAll(param);

                HSSFWorkbook book = new HSSFWorkbook();
                Dictionary<string, int> e_param = new Dictionary<string, int> {
                   {"条码",20},
                   {"编码",20},
                   {"品名",40},
                   {"单位",20},
                   {"商品类别",25},
                   {"默认供应商",25},
                   {"保质期",25},
                   {"产地",40},
                   {"计价方式",20},
                   {"商品状态",20},
                   {"进货价",20},
                   {"零售价",20},
                   {"会员价",20},
                   {"配送价",20},
                   {"批发价",20},
                   {"最低库存量",20},
                   {"最高库存量",20},
                   {"期初金额",20},
                   {"期初数量",20}
                };

                Dictionary<string, string> e_param_comment = new Dictionary<string, string> {
                   {"条码","条码规则：\r\n1.不可为空 \r\n2.不能有\"+*%/\"等特殊字符，不能以\"_\"或\"-\"开头 \r\n3.长度不能超过20 "},
                   {"编码","编码规则：\r\n1.不可为空 \r\n2.不能有\"+*%/\"等特殊字符，不能以\"_\"或\"-\"开头 \r\n3.长度不能超过20 "},
                   {"品名","品名规则:\r\n1.不可为空 \r\n2.长度不能超过80 "},
                   {"单位","单位规则:\r\n1.不能为空 \r\n2.长度不能超过20 \r\n3.系统没有的单位会自动添加"},
                   {"商品类别","类别规则: \r\n1.不能为空 \r\n2.长度不能超过60 \r\n3.系统不存在的类别会自行添加到第一级"},
                   {"默认供应商","供应商规则: \r\n1.不能为空 \r\n2.长度不能超过60 \r\n3.系统不存在的供应商会自行添加"},
                   {"保质期","保质期规则:\r\n1.不可为空 \r\n2.必须为数字 单位为 天 "},
                   {"产地","产地规则:\r\n1.不可为空 \r\n2.长度不能超过40 "},
                   {"计价方式","计价方式规则: \r\n1.只能填写0-2之间的数字  0:普通  1:称重  2:计件 "},
                   {"商品状态","商品状态规则: \r\n1.只能填写0-1之间的数字  0:停用  1:正常  "},
                   {"进货价","进货价规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"零售价","零售价规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"会员价","会员价规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"配送价","配送价规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"批发价","批发价规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"最低库存量","最低库存量规则: \r\n1.只能填写0-99999999之间的数字 \r\n2.填0或不填则不处理初始库存 \r\n3.模版中可不指定该列"},
                   {"最高库存量","最高库存量规则: \r\n1.只能填写0-99999999之间的数字 \r\n2.填0或不填则不处理初始库存 \r\n3.模版中可不指定该列"},
                   {"期初金额","期初金额规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"期初数量","期初数量规则:\r\n1.只能填写0-99999999之间的数字 \r\n2.填0或不填则不处理初始库存 \r\n3.模版中可不指定该列"}
                };


                ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "商品数据");

                if (br.Success)
                {
                    var list = (List<Tb_Shopsp_Exportdata>)br.Data;
                    for (int i = 0; i < list.Count; i++)
                    {
                        IRow rowtemp = sheet1.CreateRow(i + 1);
                        rowtemp.CreateCell(0).SetCellValue(list[i].barcode);
                        rowtemp.CreateCell(1).SetCellValue(list[i].bm);
                        rowtemp.CreateCell(2).SetCellValue(list[i].mc);
                        rowtemp.CreateCell(3).SetCellValue(list[i].dw);
                        rowtemp.CreateCell(4).SetCellValue(list[i].spfl_mc);
                        rowtemp.CreateCell(5).SetCellValue(list[i].gys_mc);
                        rowtemp.CreateCell(6).SetCellValue(list[i].yxq);
                        rowtemp.CreateCell(7).SetCellValue(list[i].cd);
                        rowtemp.CreateCell(8).SetCellValue(list[i].flag_czfs.ToString());
                        rowtemp.CreateCell(9).SetCellValue(list[i].flag_state.ToString());
                        rowtemp.CreateCell(10).SetCellValue(Convert.ToDouble(list[i].dj_jh));
                        rowtemp.CreateCell(11).SetCellValue(Convert.ToDouble(list[i].dj_ls));
                        rowtemp.CreateCell(12).SetCellValue(Convert.ToDouble(list[i].dj_hy));
                        rowtemp.CreateCell(13).SetCellValue(Convert.ToDouble(list[i].dj_ps));
                        rowtemp.CreateCell(14).SetCellValue(Convert.ToDouble(list[i].dj_pf));
                        rowtemp.CreateCell(15).SetCellValue(Convert.ToDouble(list[i].sl_kc_min));
                        rowtemp.CreateCell(16).SetCellValue(Convert.ToDouble(list[i].sl_kc_max));
                        rowtemp.CreateCell(17).SetCellValue(Convert.ToDouble(list[i].qc_je));
                        rowtemp.CreateCell(18).SetCellValue(Convert.ToDouble(list[i].qc_sl));
                        sheet1.GetRow(i + 1).Height = 28 * 20;
                    }
                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "商品数据-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
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

        #region 根据条码获取信息
        /// <summary>
        /// 根据条码获取信息
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult GetBarcodeInfo()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("barcode", string.Empty, HandleType.ReturnMsg);//条码
                param.Trim(p);
                param.Add("id_masteruser", id_user_master);
                param.Add("flag_delete", 0);
                br = BusinessFactory.Tb_Shopsp.GetAll(param);
                br.Level = ErrorLevel.Alert;
                if (br.Success)
                {
                    List<Tb_Shopsp> list = (List<Tb_Shopsp>)br.Data;
                    if (list != null && list.Count > 0)
                    {
                        #region 存在本地数据
                        if (list.Where(d => d.id_shop == id_shop).Count() > 0)
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add(string.Format("该条码已经被占用"));
                            br.Data = null;
                            return JsonString(br);
                        }
                        else
                        {
                            Tb_Shopsp shopSpMaster = null;
                            var copyShopsp = list.OrderBy(d => d.rq_create).FirstOrDefault();
                            if (list.Where(d => d.id_sp == d.id_kcsp).Count() > 0)
                            {
                                shopSpMaster = copyShopsp = list.Where(d => d.id_sp == d.id_kcsp).FirstOrDefault();
                            }


                            Hashtable ht = new Hashtable();
                            ht.Clear();
                            ht.Add("id", copyShopsp.id);
                            ht.Add("need_query_spfl", "No");
                            ht.Add("need_query_shop", "No");
                            ht.Add("need_query_kc", "No");
                            ht.Add("need_query_dbz", "Yes");
                            ht.Add("need_query_qc", "Yes");
                            ht.Add("id_user", id_user);
                            ht.Add("id_masteruser", id_user_master);
                            var copySPBr = BusinessFactory.Tb_Shopsp.Get(ht);
                            var copyData = (Tb_Shopsp_Get)copySPBr.Data;

                            if (shopSpMaster == null)
                            {
                                ht.Clear();
                                ht.Add("id_kcsp", copyShopsp.id_kcsp);
                                ht.Add("id_masteruser", id_user_master);
                                ht.Add("flag_delete", 0);
                                var brAllShopSp = BusinessFactory.Tb_Shopsp.GetAll(ht);
                                if (!brAllShopSp.Success)
                                {
                                    br.Success = false;
                                    br.Message.Clear();
                                    br.Message.Add(string.Format("获取多包装商品失败 请重试"));
                                    return JsonString(br);
                                }
                                var allShopSp = (List<Tb_Shopsp>)brAllShopSp.Data;
                                if (allShopSp != null && allShopSp.Where(d => d.id_sp == d.id_kcsp).Count() > 0)
                                {
                                    shopSpMaster = allShopSp.Where(d => d.id_sp == d.id_kcsp).FirstOrDefault();
                                }
                                else
                                {
                                    br.Success = false;
                                    br.Message.Clear();
                                    br.Message.Add(string.Format("获取多包装商品数据异常 请重试"));
                                    return JsonString(br);
                                }
                            }

                            copyData.ShopSPMaster = shopSpMaster;

                            ////测试
                            //copyData.ShopSP.pic_path= this.GetBarcodePic(new Tb_Shopsp_Service() { BarCode= copyData.ShopSP.barcode,Picture= "https://www.baidu.com/img/bd_logo1.png" });
                            //copyData.ShopSP.pic_path = this.GetBarcodePic(new Tb_Shopsp_Service() { BarCode = copyData.ShopSP.barcode, Picture = "/static/images/default.jpg" });

                            br.Success = true;
                            br.Message.Clear();
                            br.Message.Add(string.Format("查询成功"));
                            br.Data = copyData;

                            return JsonString(br);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 调用接口获取数据
                        var resultModel = new Tb_Shopsp_Get();
                        //resultModel.Spfl= new Tb_Spfl();
                        //resultModel.Shop = new Tb_Shop();
                        resultModel.ShopSP_DBZ = new List<Tb_Shopsp>();
                        try
                        {
                            if (PublicSign.shopspCheckAPI == "1")
                            {
                                #region 调用接口
                                var paramters = new Dictionary<string, string>();
                                paramters.Add("barCode", param["barcode"].ToString());
                                paramters.Add("sign", SignUtils.SignRequestForCyUserSys(paramters, PublicSign.shopspMD5Key));
                                var webutils = new CySoft.Utility.WebUtils();
                                var respStr = webutils.DoPost(PublicSign.shopspUrl, paramters, 30000);
                                var respModel = JSON.Deserialize<ServiceResult>(respStr);
                                br.Level = ErrorLevel.Question;
                                if (respModel != null)
                                {
                                    if (respModel.State != ServiceState.Done)
                                    {
                                        br.Success = false;
                                        br.Message.Clear();
                                        br.Message.Add(string.Format("查询接口失败"));
                                        return JsonString(br);
                                    }
                                    else
                                    {
                                        br.Success = true;
                                        br.Message.Clear();
                                        br.Message.Add(string.Format("查询接口成功"));
                                        br.Data = null;

                                        if (respModel.Data != null)
                                        {
                                            if (!string.IsNullOrEmpty(respModel.Data.ToString()))
                                            {
                                                var dbModel = JSON.Deserialize<Tb_Shopsp_Service>(respModel.Data.ToString());
                                                var rModel = new Tb_Shopsp()
                                                {
                                                    bm = dbModel.BarCode,
                                                    barcode = dbModel.BarCode,
                                                    mc = dbModel.ProductName,
                                                    dw = dbModel.Unit,
                                                    dj_ls = dbModel.SellingPrice
                                                };

                                                rModel.pic_path = this.GetBarcodePic(dbModel);

                                                if (rModel.bm != null)
                                                    rModel.bm = rModel.bm.Trim();
                                                if (rModel.barcode != null)
                                                    rModel.barcode = rModel.barcode.Trim();
                                                if (rModel.mc != null)
                                                    rModel.mc = rModel.mc.Trim();
                                                if (rModel.dw != null)
                                                    rModel.dw = rModel.dw.Trim();
                                                if (rModel.pic_path != null)
                                                    rModel.pic_path = rModel.pic_path.Trim();

                                                resultModel.ShopSP = rModel;
                                                br.Data = resultModel;
                                                return JsonString(br);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    br.Success = false;
                                    br.Message.Clear();
                                    br.Message.Add(string.Format("查询接口数据出现异常 请重试"));
                                    return JsonString(br);
                                }
                                #endregion
                            }
                            else
                            {
                                #region 不需要调用接口直接返回成功
                                br.Success = true;
                                br.Message.Clear();
                                br.Message.Add(string.Format("查询接口成功"));
                                br.Data = null;
                                return JsonString(br);
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add(string.Format("查询接口出现异常 请重试"));
                            return JsonString(br);
                        }
                        #endregion

                    }
                }
                else
                {
                    #region 查询本地条码信息失败
                    br.Success = false;
                    br.Message.Clear();
                    br.Message.Add(string.Format("查询本地条码信息失败 请重试"));
                    return JsonString(br);
                    #endregion
                }
                return JsonString(br);
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

        #region 获取网络图片保存在本地
        /// <summary>
        /// 获取网络图片保存在本地
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetBarcodePic(Tb_Shopsp_Service model)
        {
            return BusinessFactory.Tb_Shopsp.GetBarcodePic(model);
        }
        #endregion

        #region 缩略图
        /// <summary>
        /// 缩略图
        /// lz
        /// 2015-03-07
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult ThumpView()
        {
            ViewData["imgUrl"] = GetParameter("dataUrl");
            return PartialView("_ImgThumpControl");
        }

        #endregion

        #region 商品导入

        /// <summary>
        /// 批量导入商品 UI 
        /// lz
        /// 2016-08-25
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult ImportIn()
        {
            Hashtable param = new Hashtable();
            return View("ImportGoods");
        }

        /// <summary>
        /// 商品导入
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult ImportIn(string filePath)
        {
            var savePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\" + filePath;
            DataTable table = NPOIHelper.ImportExcelFile(savePath);
            BaseResult br = new BaseResult() { Level = ErrorLevel.Alert };
            List<Tb_Shopsp_Import> list = new List<Tb_Shopsp_Import>();
            List<Tb_Shopsp_Import> successList = new List<Tb_Shopsp_Import>();
            List<Tb_Shopsp_Import> failList = new List<Tb_Shopsp_Import>();
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
                return Json(br);
            }
            else
            {
                br.Success = true;
                br.Message.Clear();
                br.Message.Add(" 导入成功 ");
                return Json(br);
            }
        }

        #endregion

        #region 导入辅助
        /// <summary>
        /// 商品数据处理
        /// </summary>
        /// <param name="list"></param>
        /// <param name="successList"></param>
        /// <param name="failList"></param>
        private void ProccessData(string filePath, List<Tb_Shopsp_Import> list, ref List<Tb_Shopsp_Import> successList, ref List<Tb_Shopsp_Import> failList)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            param.Add("filePath", filePath);
            param.Add("list", list);
            //param.Add("id_cyuser", GetLoginInfo<string>("id_cyuser"));
            //param.Add("id_user", GetLoginInfo<string>("id_user"));
            //param.Add("id_shop", GetLoginInfo<string>("id_shop"));

            param.Add("id_masteruser", id_user_master);
            param.Add("id_user", id_user);
            param.Add("id_shop", id_shop);
            param.Add("digitHashtable", GetParm());//小数点)
            var shopShop = GetShop(Enums.ShopDataType.ShopShopAll);
            param.Add("shop_shop", shopShop);
            br = BusinessFactory.Tb_Shopsp.ImportIn(param);
            if (br.Success)
            {
                Tb_Shopsp_Import_All rModel = (Tb_Shopsp_Import_All)br.Data;
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
        private string SaveFailFile(List<Tb_Shopsp_Import> failList)
        {
            try
            {
                string fileName = "商品导入失败数据" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                string url = "/UpLoad/Temp/" + fileName;
                string fileFullName = ApplicationInfo.TempPath + "\\" + fileName;

                HSSFWorkbook book = new HSSFWorkbook();
                Dictionary<string, int> e_param = new Dictionary<string, int> {
                    {"备注",130},
                   {"条码",20},
                   {"编码",20},
                   {"品名",40},
                   {"单位",20},
                   {"商品类别",25},
                   {"默认供应商",25},
                   {"保质期",25},
                   {"产地",40},
                   {"计价方式",20},
                   {"商品状态",20},
                   {"进货价",20},
                   {"零售价",20},
                   {"会员价",20},
                   {"配送价",20},
                   {"最低库存量",20},
                   {"最高库存量",20},
                   {"期初金额",20},
                   {"期初数量",20}
                   
                };

                Dictionary<string, string> e_param_comment = new Dictionary<string, string> {
                    {"备注","导入的一些描述"},
                   {"条码","条码规则：\r\n1.不可为空 \r\n2.不能有\"+*%/\"等特殊字符，不能以\"_\"或\"-\"开头 \r\n3.长度不能超过20 "},
                   {"编码","编码规则：\r\n1.不可为空 \r\n2.不能有\"+*%/\"等特殊字符，不能以\"_\"或\"-\"开头 \r\n3.长度不能超过20 "},
                   {"品名","品名规则:\r\n1.不可为空 \r\n2.长度不能超过80 "},
                   {"单位","单位规则:\r\n1.不能为空 \r\n2.长度不能超过20 \r\n3.系统没有的单位会自动添加"},
                   {"商品类别","类别规则: \r\n1.不能为空 \r\n2.长度不能超过60 \r\n3.系统不存在的类别会自行添加到第一级"},
                   {"默认供应商","供应商规则: \r\n1.不能为空 \r\n2.长度不能超过60 \r\n3.系统不存在的供应商会自行添加"},
                   {"保质期","保质期规则:\r\n1.不可为空 \r\n2.必须为数字 单位为 天 "},
                   {"产地","产地规则:\r\n1.不可为空 \r\n2.长度不能超过40 "},
                   {"计价方式","计价方式规则: \r\n1.只能填写0-2之间的数字  0:普通  1:称重  2:计件 "},
                   {"商品状态","商品状态规则: \r\n1.只能填写0-1之间的数字  0:停用  1:正常  "},
                   {"进货价","进货价规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"零售价","零售价规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"会员价","会员价规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"配送价","配送价规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"最低库存量","最低库存量规则: \r\n1.只能填写0-99999999之间的数字 \r\n2.填0或不填则不处理初始库存 \r\n3.模版中可不指定该列"},
                   {"最高库存量","最高库存量规则: \r\n1.只能填写0-99999999之间的数字 \r\n2.填0或不填则不处理初始库存 \r\n3.模版中可不指定该列"},
                   {"期初金额","期初金额规则: \r\n1.为空或非法字符以0保存(单位:元) \r\n2.只能在0~99999999的范围之间"},
                   {"期初数量","期初数量规则:\r\n1.只能填写0-99999999之间的数字 \r\n2.填0或不填则不处理初始库存 \r\n3.模版中可不指定该列"}
                   
                };

                ISheet sheet1 = NPOIHelper.CreateSheet(book, e_param, e_param_comment, "商品资料模版");
                int i = 1;
                foreach (var item in failList)
                {
                    try
                    {
                        IRow rowtemp = sheet1.CreateRow(i);
                        rowtemp.CreateCell(0).SetCellValue(item.bz);
                        rowtemp.CreateCell(1).SetCellValue(item.barcode);
                        rowtemp.CreateCell(2).SetCellValue(item.bm);
                        rowtemp.CreateCell(3).SetCellValue(item.mc);
                        rowtemp.CreateCell(4).SetCellValue(item.dw);
                        rowtemp.CreateCell(5).SetCellValue(item.spfl_mc);
                        rowtemp.CreateCell(6).SetCellValue(item.gys_mc);
                        rowtemp.CreateCell(7).SetCellValue(item.yxq);
                        rowtemp.CreateCell(8).SetCellValue(item.cd);
                        rowtemp.CreateCell(9).SetCellValue(item.flag_czfs.ToString());
                        rowtemp.CreateCell(10).SetCellValue(item.flag_state.ToString());
                        rowtemp.CreateCell(11).SetCellValue(Convert.ToDouble(item.dj_jh));
                        rowtemp.CreateCell(12).SetCellValue(Convert.ToDouble(item.dj_ls));
                        rowtemp.CreateCell(13).SetCellValue(Convert.ToDouble(item.dj_hy));
                        rowtemp.CreateCell(14).SetCellValue(Convert.ToDouble(item.dj_ps));
                        rowtemp.CreateCell(15).SetCellValue(Convert.ToDouble(item.sl_kc_min));
                        rowtemp.CreateCell(16).SetCellValue(Convert.ToDouble(item.sl_kc_max));
                        rowtemp.CreateCell(17).SetCellValue(Convert.ToDouble(item.je_qc));
                        rowtemp.CreateCell(18).SetCellValue(Convert.ToDouble(item.sl_qc));
                        sheet1.GetRow(i).Height = 28 * 20;
                    }
                    catch (Exception ex)
                    {
                    }
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
        /// 下载商品介绍失败数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private FileResult DownloadFailInfo(string filePath)
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
                ISheet sheet1 = book.GetSheet("商品导入");
                for (int i = 0; i < dr.Length; i++)
                {
                    IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(dr[i]["备注"].ToString());
                    rowtemp.CreateCell(1).SetCellValue(dr[i]["编码或条码"].ToString());
                    rowtemp.CreateCell(2).SetCellValue(dr[i]["品名"].ToString());
                    rowtemp.CreateCell(3).SetCellValue(dr[i]["单位"].ToString());
                    rowtemp.CreateCell(4).SetCellValue(dr[i]["商品类别"].ToString());
                    rowtemp.CreateCell(5).SetCellValue(dr[i]["默认供应商"].ToString());
                    rowtemp.CreateCell(6).SetCellValue(dr[i]["保质期"].ToString());
                    rowtemp.CreateCell(7).SetCellValue(dr[i]["产地"].ToString());
                    rowtemp.CreateCell(8).SetCellValue(dr[i]["计价方式"].ToString());
                    rowtemp.CreateCell(9).SetCellValue(dr[i]["商品状态"].ToString());
                    rowtemp.CreateCell(10).SetCellValue(dr[i]["进货价"].ToString());
                    rowtemp.CreateCell(11).SetCellValue(dr[i]["零售价"].ToString());
                    rowtemp.CreateCell(12).SetCellValue(dr[i]["会员价"].ToString());
                    rowtemp.CreateCell(13).SetCellValue(dr[i]["配送价"].ToString());
                    rowtemp.CreateCell(14).SetCellValue(dr[i]["最低库存量"].ToString());
                    rowtemp.CreateCell(15).SetCellValue(dr[i]["最高库存量"].ToString());
                    rowtemp.CreateCell(16).SetCellValue(dr[i]["期初金额"].ToString());
                    rowtemp.CreateCell(17).SetCellValue(dr[i]["期初数量"].ToString());
                    
                }
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "商品导入失败数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            }
            catch (Exception)
            {
                throw;
            }
        }


        private List<Tb_Shopsp_Import> TurnShopSPImportList(DataTable table)
        {
            List<Tb_Shopsp_Import> list = new List<Tb_Shopsp_Import>();

            foreach (DataRow item in table.Rows)
            {
                Tb_Shopsp_Import model = new Tb_Shopsp_Import();
                model.barcode = item["条码"] == null ? "" : item["条码"].ToString();
                model.bm = item["编码"] == null ? "" : item["编码"].ToString();
                model.mc = item["品名"] == null ? "" : item["品名"].ToString();
                model.dw = item["单位"] == null ? "" : item["单位"].ToString();
                model.id_spfl = item["商品类别"] == null ? "" : item["商品类别"].ToString();
                model.cd = item["产地"] == null ? "" : item["产地"].ToString();
                decimal dj_jh = 0;
                decimal.TryParse(item["进货价"] == null ? "" : item["进货价"].ToString(), out dj_jh);
                model.dj_jh = dj_jh.ToString();
                decimal dj_ls = 0;
                decimal.TryParse(item["零售价"] == null ? "" : item["零售价"].ToString(), out dj_ls);
                model.dj_ls = dj_ls.ToString();
                model.sl_kc_min = item["最低库存量"] == null ? "" : item["最低库存量"].ToString();
                model.sl_kc_max = item["最高库存量"] == null ? "" : item["最高库存量"].ToString();
                decimal je_qc = 0;
                decimal.TryParse(item["期初金额"] == null ? "" : item["期初金额"].ToString(), out je_qc);
                model.je_qc = je_qc;
                decimal sl_qc = 0;
                decimal.TryParse(item["期初数量"] == null ? "" : item["期初数量"].ToString(), out sl_qc);
                model.sl_qc = sl_qc;

                byte flag_czfs = 9;
                if (!byte.TryParse(item["计价方式"] == null ? "" : item["计价方式"].ToString(), out flag_czfs))
                    model.flag_czfs = item["计价方式"] == null ? "" : item["计价方式"].ToString();
                else
                    model.flag_czfs = flag_czfs.ToString();

                byte flag_state = 9;
                if (!byte.TryParse(item["商品状态"] == null ? "" : item["商品状态"].ToString(), out flag_state))
                    model.flag_state = item["商品状态"] == null ? "" : item["商品状态"].ToString();
                else
                    model.flag_state = flag_state.ToString();

                decimal dj_hy = 0;
                decimal.TryParse(item["会员价"] == null ? "" : item["会员价"].ToString(), out dj_hy);
                model.dj_hy = dj_hy.ToString();


                decimal dj_ps = 0;
                decimal.TryParse(item["配送价"] == null ? "" : item["配送价"].ToString(), out dj_ps);
                model.dj_ps = dj_ps.ToString();

                model.id_gys = item["默认供应商"] == null ? "" : item["默认供应商"].ToString();
                model.gys_mc = model.id_gys;

                model.spfl_mc = model.id_spfl;

                decimal yxq = 0;

                decimal.TryParse(item["保质期"] == null ? "" : item["保质期"].ToString(), out yxq);
                model.yxq = yxq.ToString();

                decimal dj_pf = 0;
                decimal.TryParse(item["批发价"] == null ? "" : item["批发价"].ToString(), out dj_pf);
                model.dj_pf = dj_pf.ToString();

               
                list.Add(model);
            }
            return list;
        }
        #endregion

        #region 生成条码
        [ActionPurview(false)]
        public ActionResult CreateBarcode()
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            try
            {
                br.Success = true;
                if (param != null && param.ContainsKey("flag_czfs") && (param["flag_czfs"].ToString() == "1" || param["flag_czfs"].ToString() == "2"))
                    br.Data = GetNewDH(Enums.FlagDJLX.BMShopspCZFS);
                else
                    br.Data = GetNewDH(Enums.FlagDJLX.BMShopsp);
                return JsonString(br);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 根据条码名称获取商品信息
        [ActionPurview(false)]
        public ActionResult GetShopspList()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("keyword", string.Empty, HandleType.DefaultValue);//keyword
                p.Add("id_shop", string.Empty, HandleType.Remove);//keyword
                p.Add("id_spfls", string.Empty, HandleType.Remove);
                param = param.Trim(p);
                if (string.IsNullOrEmpty(param["keyword"].ToString()))
                {
                    br.Success = false;
                    br.Data = "";
                    return JsonString(br);
                }
                param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                param.Add("id_masteruser", id_user_master);

                if (!param.ContainsKey("id_shop"))
                    param.Add("id_shop", id_shop);

                param.Add("flag_state", (byte)Enums.FlagShopspStop.NoStop);

                //if (CyVerify.IsIncludeChinese(param["keyword"].ToString()))
                //{
                    #region 包含中文 目前查所有的
                    //包含中文 
                    string keyword = "%" + param["keyword"].ToString() + "%";
                    param.Remove("keyword");
                    param.Add("keyword", keyword);
                    br = BusinessFactory.Tb_Shopsp.GetShopspList(param);
                    if (br.Success)
                    {
                        List<ShopspList_Query> shopspList = (List<ShopspList_Query>)br.Data;
                        if (shopspList.Count() == 1)
                        {
                            var shopspModel = shopspList.FirstOrDefault();
                            param.Remove("barcode");
                            param.Add("id_sp", shopspModel.id_sp);
                            var brDW = BusinessFactory.Tb_Shopsp.GetAll(param);
                            if (brDW.Success)
                                shopspModel.dw_list = (List<Tb_Shopsp>)brDW.Data;
                            br.Success = true;
                            br.Data = shopspModel;
                        }
                        else
                        {
                            br.Success = false;
                            br.Data = "";
                        }
                    }

                    #endregion 
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

        #region 根据条码名称获取商品信息(对配送业务)
        [ActionPurview(false)]
        public ActionResult GetShopspListForPs()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("keyword", string.Empty, HandleType.DefaultValue);//keyword
                p.Add("id_shop", string.Empty, HandleType.DefaultValue);//keyword
                p.Add("id_shop_ck", string.Empty, HandleType.DefaultValue);//keyword
                param = param.Trim(p);
                if (string.IsNullOrEmpty(param["keyword"].ToString()))
                {
                    br.Success = false;
                    br.Data = "";
                    return JsonString(br);
                } 
                param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                param.Add("id_masteruser", id_user_master);
                param.Add("flag_state", (byte)Enums.FlagShopspStop.NoStop);
                if (param.Contains("keyword"))
                {
                    param["keyword"] = string.Format("%{0}%", param["keyword"]);
                }
                //param.Add("barcode", param["keyword"].ToString());
                //param.Remove("keyword");
                br = BusinessFactory.Tb_Shopsp.GetShopspListForPs(param);
                if (br.Success)
                {
                    List<Tb_Shopsp_Query_For_Ps> shopspList = (List<Tb_Shopsp_Query_For_Ps>)br.Data;
                    if (shopspList.Count() == 1)
                    {
                        var shopspModel = shopspList.FirstOrDefault();
                        param.Remove("barcode");
                        param.Add("id_sp", shopspModel.id_sp);
                        var brDW = BusinessFactory.Tb_Shopsp.GetAll(param);
                        if (brDW.Success)
                            shopspModel.dw_list = (List<Tb_Shopsp>)brDW.Data;
                        br.Success = true;
                        br.Data = shopspModel;
                    }
                    else
                    {
                        br.Success = false;
                        br.Data = "";
                    }
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

        #region 根据商品id获取商品多单位信息
        [ActionPurview(false)]
        public ActionResult GetShopspDwList()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                var ps_id_shop = string.Format("{0}", param["ps_id_shop"]);
                ParamVessel p = new ParamVessel();
                p.Add("select_id_sp", string.Empty, HandleType.DefaultValue);//keyword
                param = param.Trim(p);
                if (string.IsNullOrEmpty(param["select_id_sp"].ToString()))
                {
                    br.Success = false;
                    br.Data = "";
                    return JsonString(br);
                }

                param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                param.Add("id_masteruser", id_user_master);
                if (!string.IsNullOrEmpty(ps_id_shop))
                {
                    param.Add("id_shop", ps_id_shop);
                }
                else
                {
                    param.Add("id_shop", id_shop);
                }
                param.Add("select_id_sp_wd", param["select_id_sp"].ToString());
                param.Remove("select_id_sp");

                br = BusinessFactory.Tb_Shopsp.GetShopspDwList(param);
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

        #region 列表左侧树

        [HttpGet]
        [ActionPurview(false)]
        public ActionResult Get_Left_Tree()
        {
            Hashtable param = base.GetParameters();
            ParamVessel pv = new ParamVessel();
            pv.Add("td", string.Empty, HandleType.Remove);
            pv.Add("type", "0", HandleType.DefaultValue);
            pv.Add("sort", "sort_id asc", HandleType.DefaultValue);
            pv.Add("id_masteruser", id_user_master, HandleType.DefaultValue);

            dynamic tree = new List<dynamic>() { new { id = "0", id_farther = "#", mc = "全部" } };

            try
            {
                BaseResult br = new BaseResult();
                param = param.Trim(pv);
                string type = param["type"].ToString();

                switch (type)
                {
                    case "0":
                        {
                            #region 商品分类
                            param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                            br = BusinessFactory.Tb_Spfl.GetAll(param);
                            if (br.Success && br.Data is List<Tb_Spfl>)
                            {
                                List<Tb_Spfl> list = br.Data as List<Tb_Spfl>;
                                list.Insert(0, new Tb_Spfl() { id = "0", id_father = "#", mc = "全部", bm = "0" });

                                tree = (from node in list
                                        select new
                                        {
                                            id = node.id,
                                            parent = node.id_father,
                                            text = node.mc,
                                            bm = node.bm
                                        });
                            }

                            #endregion 商品分类
                        }
                        break;
                    case "1":
                        {
                            #region 供应商_demo

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

                            #endregion 供应商_demo
                        }
                        break;
                    case "2":
                        {
                            #region 客户分类_demo

                            br = BusinessFactory.Tb_Khfl.GetAll(param);
                            if (br.Success && br.Data is List<Tb_Khfl>)
                            {
                                List<Tb_Khfl> list = br.Data as List<Tb_Khfl>;
                                list.Insert(0, new Tb_Khfl() { id = "0", id_farther = "#", mc = "全部", bm = "0" });

                                tree = (from node in list
                                        select new
                                        {
                                            id = node.id,
                                            parent = node.id_farther,
                                            text = node.mc,
                                            bm = node.bm
                                        });
                            }

                            #endregion 客户分类_demo
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
            }

            return JsonString(tree);
        }

        #endregion 列表左侧树

        #region 商品列表
        /// <summary>
        /// 商品列表
        /// </summary>
        [ActionPurview(false)]
        public ActionResult Search()
        {
            try
            {
                #region 辅助参数
                PageNavigate pn = new PageNavigate();

                //var stateBr = base.GetFlagList(Enums.TsFlagListCode.spstate.ToString());
                //if (stateBr.Success)
                //    ViewBag.selectListState = stateBr.Data;

                //var czfsBr = base.GetFlagList(Enums.TsFlagListCode.spczfs.ToString());
                //if (czfsBr.Success)
                //    ViewBag.selectListCZFS = czfsBr.Data;

                //var spflBr = base.GetSPFLJsonStr();
                //if (spflBr.Success)
                //    ViewBag.selectListSPFL = spflBr.Data.ToString();

                Hashtable ht = new Hashtable();
                //ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                //ht.Add("id_masteruser", id_user_master);
                //var shopBr = BusinessFactory.Tb_Shop.GetPage(ht);
                //if (shopBr.Success)
                //    ViewBag.selectListShop = shopBr.Data;

                //获取前台控制小数点
                ViewBag.DigitHashtable = GetParm();

                BaseResult br = new BaseResult();
                Hashtable param = base.GetParameters();
                var searchType = string.Format("{0}", param["searchType"]);
                if (param.ContainsKey("count"))
                {
                    ViewData["count"] = param["count"];
                    ViewData["searchType"] = param["searchType"];
                    param.Remove("count");
                }
                //关键词
                if (param != null && param.ContainsKey("keyword"))
                    ViewData["keyword"] = param["keyword"];
                if (param != null && param.ContainsKey("id_shop"))
                    ViewData["id_shop"] = param["id_shop"];
                if (param != null && param.ContainsKey("id_shop_ck"))
                    ViewData["id_shop_ck"] = param["id_shop_ck"];
                #endregion

                #region 验证参数
                if (string.IsNullOrEmpty(id_user_master))
                {
                    br.Message.Add(string.Format("参数解析错误！请重新刷新页面。"));
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
                #endregion

                #region 构建param
                ParamVessel p = new ParamVessel();
                //排序字段
                p.Add("special_order_field", "rq_create", HandleType.DefaultValue);
                //排序方式
                p.Add("special_order_descasc", "desc", HandleType.DefaultValue);
                //声明周期
                p.Add("flag_state", (byte)0, HandleType.Remove);
                //商品分类
                p.Add("id_spfl", String.Empty, HandleType.Remove);
                //多个商品分类
                p.Add("id_spfls", String.Empty, HandleType.Remove);
                //商品门店
                p.Add("id_shop", String.Empty, HandleType.Remove);

                p.Add("id_shop_ck", String.Empty, HandleType.Remove);
                //搜素关键字
                p.Add("keyword", String.Empty, HandleType.Remove, true);
                //计价方式
                p.Add("flag_czfs", (byte)0, HandleType.Remove);
                //是否页面查询标志
                p.Add("_search_", "0", HandleType.DefaultValue);
                //是否页面查询标志
                p.Add("_search_dropdown_", "0", HandleType.DefaultValue);
                //当前页码
                p.Add("page", 0, HandleType.DefaultValue);
                //每页大小
                p.Add("pageSize", base.PageSizeFromCookie, HandleType.DefaultValue);
                param = param.Trim(p);
                if (param != null && param.ContainsKey("id_spfls"))
                {
                    var id_spfl_arr = string.Format("{0}", param["id_spfls"] + "").Split(',');
                    id_spfl_arr = id_spfl_arr.Where(a => !string.IsNullOrEmpty(a)).ToArray();
                    Hashtable htspfl = new Hashtable();
                    htspfl.Add("id_masteruser", id_user_master);
                    htspfl.Add("flList", id_spfl_arr);
                    htspfl.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                    var spfllist = BusinessFactory.Tb_Spfl.GetAll(htspfl).Data as List<Tb_Spfl>;
                    ViewData["id_spfls"] = param["id_spfls"];
                    if (spfllist.Any())
                    {
                        param["id_spfls"] = (from t in spfllist select t.id).ToArray();
                    }
                    else
                    {
                        param.Remove("id_spfls");
                    } 
                }
                int page = Convert.ToInt32(param["page"]);
                int pageSize = Convert.ToInt32(param["pageSize"]);
                //排序方式
                param.Add("dir", param["special_order_descasc"].ToString());
                //排序字段
                param.Add("sort", param["special_order_field"].ToString());

                param.Remove("special_order_descasc");
                param.Remove("special_order_field");

                if (param.ContainsKey("id_spfl") && param["id_spfl"].ToString() == "0")
                    param.Remove("id_spfl");

                var _id_spfl = string.Format("{0}", param["id_spfl"]);
                if (!string.IsNullOrEmpty(_id_spfl))
                {
                    if (_id_spfl.Contains(","))
                    {
                        var id_spfl_arr = _id_spfl.Split(',');
                        param["id_spfls"] = id_spfl_arr;
                        param.Remove("id_spfl");
                    }
                }
                //主用户id
                param.Add("id_masteruser", id_user_master);
                //每页限制
                param.Add("limit", pageSize);
                //开始分页起点
                param.Add("start", page * pageSize);
                //是否删除
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                #endregion

                if (!param.ContainsKey("flag_state"))
                    param.Add("flag_state", (byte)Enums.FlagShopspStop.NoStop);

                if (searchType == "report" && !param.Contains("id_shop"))
                {
                    var shopList = new List<Tb_User_ShopWithShopMc>();
                    if (id_shop == id_shop_master)
                    {
                        shopList = GetShop(Enums.ShopDataType.UserShop);
                    }
                    else
                    {
                        shopList = GetShop(Enums.ShopDataType.UserShopOnly);
                    }
                    var id_shop_array = (from s in shopList select s.id_shop).ToArray();
                    if (id_shop_array.Any())
                    {
                        param.Add("id_shop_array", id_shop_array);
                    }
                }
                #region 获取数据
                PageList<Tb_Shopsp_Query> list = new PageList<Tb_Shopsp_Query>(pageSize);
                //pn = BusinessFactory.Tb_Shopsp.GetPageSel(param);
                pn = BusinessFactory.Tb_Shopsp.GetPage(param);
                list = new PageList<Tb_Shopsp_Query>(pn, page, pageSize);

                ViewData["list_shopsp"] = list;

                if (param["_search_"].ToString().Equals("1"))
                    return PartialView("_Search");
                else if (param["_search_dropdown_"].ToString().Equals("1"))
                    return PartialView("_Search_DropDown");
                else
                    return View();
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

        /// <summary>
        /// 商品列表
        /// </summary>
        [ActionPurview(false)]
        public ActionResult SearchForPs()
        {
            try
            {
                #region 辅助参数
                PageNavigate pn = new PageNavigate();

                var stateBr = base.GetFlagList(Enums.TsFlagListCode.spstate.ToString());
                if (stateBr.Success)
                    ViewBag.selectListState = stateBr.Data;

                var czfsBr = base.GetFlagList(Enums.TsFlagListCode.spczfs.ToString());
                if (czfsBr.Success)
                    ViewBag.selectListCZFS = czfsBr.Data;

                var spflBr = base.GetSPFLJsonStr();
                if (spflBr.Success)
                    ViewBag.selectListSPFL = spflBr.Data.ToString();

                Hashtable ht = new Hashtable();
                ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                ht.Add("id_masteruser", id_user_master);
                var shopBr = BusinessFactory.Tb_Shop.GetPage(ht);
                if (shopBr.Success)
                    ViewBag.selectListShop = shopBr.Data;

                //获取前台控制小数点
                ViewBag.DigitHashtable = GetParm();

                BaseResult br = new BaseResult();
                Hashtable param = base.GetParameters();

                //关键词
                if (param != null && param.ContainsKey("keyword"))
                    ViewData["keyword"] = param["keyword"];
                if (param != null && param.ContainsKey("id_shop"))
                    ViewData["id_shop"] = param["id_shop"];
                if (param != null && param.ContainsKey("id_shop_ck"))
                    ViewData["id_shop_ck"] = param["id_shop_ck"];
                #endregion

                #region 验证参数
                if (string.IsNullOrEmpty(id_user_master))
                {
                    br.Message.Add(string.Format("参数解析错误！请重新刷新页面。"));
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
                #endregion

                #region 构建param
                ParamVessel p = new ParamVessel();
                //排序字段
                p.Add("special_order_field", "rq_create", HandleType.DefaultValue);
                //排序方式
                p.Add("special_order_descasc", "desc", HandleType.DefaultValue);
                //声明周期
                p.Add("flag_state", (byte)0, HandleType.Remove);
                //商品分类
                p.Add("id_spfl", String.Empty, HandleType.Remove);
                //多个商品分类
                p.Add("id_spfls", String.Empty, HandleType.Remove);
                //商品门店
                p.Add("id_shop", String.Empty, HandleType.Remove);

                p.Add("id_shop_ck", String.Empty, HandleType.Remove);
                //搜素关键字
                p.Add("keyword", String.Empty, HandleType.Remove, true);
                //计价方式
                p.Add("flag_czfs", (byte)0, HandleType.Remove);
                //是否页面查询标志
                p.Add("_search_", "0", HandleType.DefaultValue);
                //是否页面查询标志
                p.Add("_search_dropdown_", "0", HandleType.DefaultValue);
                //当前页码
                p.Add("page", 0, HandleType.DefaultValue);
                //每页大小
                p.Add("pageSize", base.PageSizeFromCookie, HandleType.DefaultValue);
                param = param.Trim(p);
                if (param != null && param.ContainsKey("id_spfls"))
                {
                    var id_spfl_arr = string.Format("{0}", param["id_spfls"] + "").Split(',');
                    param["id_spfls"] = id_spfl_arr;
                }
                int page = Convert.ToInt32(param["page"]);
                int pageSize = Convert.ToInt32(param["pageSize"]);
                //排序方式
                param.Add("dir", param["special_order_descasc"].ToString());
                //排序字段
                param.Add("sort", param["special_order_field"].ToString());

                param.Remove("special_order_descasc");
                param.Remove("special_order_field");

                if (param.ContainsKey("id_spfl") && param["id_spfl"].ToString() == "0")
                    param.Remove("id_spfl");

                var _id_spfl = string.Format("{0}", param["id_spfl"]);
                if (!string.IsNullOrEmpty(_id_spfl))
                {
                    if (_id_spfl.Contains(","))
                    {
                        var id_spfl_arr = _id_spfl.Split(',');
                        param["id_spfls"] = id_spfl_arr;
                        param.Remove("id_spfl");
                    }
                }
                //主用户id
                param.Add("id_masteruser", id_user_master);
                //每页限制
                param.Add("limit", pageSize);
                //开始分页起点
                param.Add("start", page * pageSize);
                //是否删除
                param.Add("flag_delete", ((int)Enums.FlagDelete.NoDelete).ToString());
                #endregion

                if (!param.ContainsKey("flag_state"))
                    param.Add("flag_state", ((byte)Enums.FlagShopspStop.NoStop).ToString());

                #region 获取数据
                PageList<Tb_Shopsp_Query_For_Ps> list = new PageList<Tb_Shopsp_Query_For_Ps>(pageSize);
                pn = BusinessFactory.Tb_Shopsp.GetPageListForPs(param);
                list = new PageList<Tb_Shopsp_Query_For_Ps>(pn, page, pageSize);

                ViewData["list_shopsp"] = list;

                if (param["_search_"].ToString().Equals("1"))
                    return PartialView("_SearchForPs");
                else if (param["_search_dropdown_"].ToString().Equals("1"))
                    return PartialView("_Search_DropDown");
                else
                    return View();
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

        #region 判断 存储图片的文件夹是否存在
        /// <summary>
        /// 判断 存储图片的文件夹是否存在  
        /// </summary>
        private void CheckImgPath()
        {
            // 判断 存储图片的文件夹是否存在  
            if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/UpLoad/Goods")))//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/UpLoad/Goods"));

                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/UpLoad/Goods/thumb")))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/UpLoad/Goods/thumb"));
                }
            }
        }
        #endregion

        #region 检测新增/编辑的数据是否合法
        /// <summary>
        /// 检测新增/编辑的数据是否合法
        /// </summary>
        /// <param name="param"></param>
        /// <param name="qcModel"></param>
        /// <param name="dbzList"></param>
        /// <returns></returns>
        public BaseResult CheckParam(Hashtable param, Td_Sp_Qc qcModel, List<Tb_Shopsp_DBZ> dbzList)
        {
            return BusinessFactory.Tb_Shopsp.CheckParam(param, qcModel, dbzList);

            #region 注释
            //BaseResult br = new BaseResult();

            //#region 验证数据
            ////控制层验证数据
            //if (string.IsNullOrEmpty(param["barcode"].ToString()))
            //{
            //    br.Success = false;
            //    br.Message.Add("条码不能为空!");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "barcode", value = param["barcode"].ToString() };
            //    return br;
            //}

            //if (string.IsNullOrEmpty(param["bm"].ToString()))
            //{
            //    br.Success = false;
            //    br.Message.Add("编码不能为空!");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "bm", value = param["bm"].ToString() };
            //    return br;
            //}

            //if (string.IsNullOrEmpty(param["mc"].ToString()))
            //{
            //    br.Success = false;
            //    br.Message.Add("名称不能为空");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "mc", value = param["mc"].ToString() };
            //    return br;
            //}

            //if (string.IsNullOrEmpty(param["dj_jh"].ToString()) || !CyVerify.IsNumeric(param["dj_jh"].ToString()) || decimal.Parse(param["dj_jh"].ToString()) < 0)
            //{
            //    br.Success = false;
            //    br.Message.Add("进货价不符合要求");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "dj_jh", value = param["dj_jh"].ToString() };
            //    return br;
            //}

            //if (string.IsNullOrEmpty(param["dj_ls"].ToString()) || !CyVerify.IsNumeric(param["dj_ls"].ToString()) || decimal.Parse(param["dj_ls"].ToString()) < 0)
            //{
            //    br.Success = false;
            //    br.Message.Add("零售价不符合要求");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "dj_ls", value = param["dj_ls"].ToString() };
            //    return br;
            //}

            //if (string.IsNullOrEmpty(param["dj_hy"].ToString()) || !CyVerify.IsNumeric(param["dj_hy"].ToString()) || decimal.Parse(param["dj_hy"].ToString()) < 0)
            //{
            //    br.Success = false;
            //    br.Message.Add("会员价不符合要求");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "dj_hy", value = param["dj_hy"].ToString() };
            //    return br;
            //}

            //if (string.IsNullOrEmpty(param["dj_ps"].ToString()) || !CyVerify.IsNumeric(param["dj_ps"].ToString()) || decimal.Parse(param["dj_ps"].ToString()) < 0)
            //{
            //    br.Success = false;
            //    br.Message.Add("配送价不符合要求");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "dj_ps", value = param["dj_ps"].ToString() };
            //    return br;
            //}

            //if (!string.IsNullOrEmpty(param["dj_pf"].ToString()) && !CyVerify.IsNumeric(param["dj_pf"].ToString()) || decimal.Parse(param["dj_pf"].ToString()) < 0)
            //{
            //    br.Success = false;
            //    br.Message.Add("批发价不符合要求");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "dj_pf", value = param["dj_pf"].ToString() };
            //    return br;
            //}

            //if (string.IsNullOrEmpty(param["dw"].ToString()))
            //{
            //    br.Success = false;
            //    br.Message.Add("单位不能为空");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "dw", value = param["dw"].ToString() };
            //    return br;
            //}
            //if (string.IsNullOrEmpty(param["sl_kc_min"].ToString()) || !CyVerify.IsNumeric(param["sl_kc_min"].ToString()) || decimal.Parse(param["sl_kc_min"].ToString()) < 0)
            //{
            //    br.Success = false;
            //    br.Message.Add("最底库存量不符合要求");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "sl_kc_min", value = param["sl_kc_min"].ToString() };
            //    return br;
            //}
            //if (string.IsNullOrEmpty(param["sl_kc_max"].ToString()) || !CyVerify.IsNumeric(param["sl_kc_max"].ToString()) || decimal.Parse(param["sl_kc_max"].ToString()) < 0)
            //{
            //    br.Success = false;
            //    br.Message.Add("最高库存量不符合要求");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "sl_kc_max", value = param["sl_kc_max"].ToString() };
            //    return br;
            //}

            //if (decimal.Parse(param["sl_kc_max"].ToString()) < decimal.Parse(param["sl_kc_min"].ToString()))
            //{
            //    br.Success = false;
            //    br.Message.Add("最高库存量不应小于最底库存量.");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "sl_kc_max", value = param["sl_kc_max"].ToString() };
            //    return br;
            //}

            //if ((param["id_spfl"] == null || string.IsNullOrEmpty(param["id_spfl"].ToString())))
            //{
            //    br.Success = false;
            //    br.Message.Add("商品类别不符合要求");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "id_spfl", value = param["id_spfl"].ToString() };
            //    return br;
            //}

            //if (qcModel == null)
            //{
            //    br.Success = false;
            //    br.Message.Add("期初不能为空");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "sl_qc", value = param["sl_qc"].ToString() };
            //    return br;
            //}

            //qcModel.sl_qc = qcModel.sl_qc == null ? 0 : qcModel.sl_qc;
            //qcModel.je_qc = qcModel.je_qc == null ? 0 : qcModel.je_qc;

            //if ((qcModel.sl_qc > 0 && qcModel.je_qc < 0) || (qcModel.sl_qc < 0 && qcModel.je_qc > 0))
            //{
            //    br.Success = false;
            //    br.Message.Add("期初不允许数量和金额一正一负");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "sl_qc", value = param["sl_qc"].ToString() };
            //    return br;
            //}

            //if (param["bz"] != null && !string.IsNullOrWhiteSpace(param["bz"].ToString()) && param["bz"].ToString().Length > 200)
            //{
            //    br.Success = false;
            //    br.Message.Add("商品备注应在200字以内.");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = new { id = "", name = "bz", value = param["bz"].ToString() };
            //    return br;
            //}

            //foreach (var item in dbzList)
            //{
            //    if (string.IsNullOrEmpty(item.barcode))
            //    {
            //        br.Success = false;
            //        br.Message.Add("多包装商品条码不能为空");
            //        br.Level = ErrorLevel.Warning;
            //        br.Data = new { id = item.id, name = "barcode", value = item.barcode };
            //        return br;
            //    }

            //    if (string.IsNullOrEmpty(item.bm))
            //    {
            //        br.Success = false;
            //        br.Message.Add("多包装商品编码不能为空");
            //        br.Level = ErrorLevel.Warning;
            //        br.Data = new { id = item.id, name = "bm", value = item.bm };
            //        return br;
            //    }

            //    if (string.IsNullOrEmpty(item.mc))
            //    {
            //        br.Success = false;
            //        br.Message.Add("多包装商品名称不能为空");
            //        br.Level = ErrorLevel.Warning;
            //        br.Data = new { id = item.id, name = "mc", value = item.mc };
            //        return br;
            //    }


            //    if (item.barcode == param["barcode"].ToString())
            //    {
            //        br.Success = false;
            //        br.Message.Add("多包装商品条码不能与主商品条码重复");
            //        br.Level = ErrorLevel.Warning;
            //        br.Data = new { id = item.id, name = "barcode", value = item.barcode };
            //        return br;
            //    }

            //    if (item.bm == param["bm"].ToString())
            //    {
            //        br.Success = false;
            //        br.Message.Add("多包装商品编码不能与主商品编码重复");
            //        br.Level = ErrorLevel.Warning;
            //        br.Data = new { id = item.id, name = "bm", value = item.bm };
            //        return br;
            //    }

            //}

            //#endregion

            //br.Message.Clear();
            //br.Success = true;
            //return br; 
            #endregion
        }
        #endregion

        #region Spzhcf
        [ActionPurview(false)]
        [HttpGet]
        public ActionResult Spzhcf()
        {
            ViewData["id_shop"] = id_shop;
            ViewBag.DigitHashtable = GetParm();
            return View();
        }
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult Spzhcf(SpzhcfModel model)//SpzhcfModel model
        {
            var res = HandleResult(() =>
            {
                model.id_user = id_user;
                model.id_masteruser = id_user_master;
                return BusinessFactory.Tb_Sp_Jgzh.Add(model);
            });
            return JsonString(res);
        } 
        #endregion

 

    }
}
