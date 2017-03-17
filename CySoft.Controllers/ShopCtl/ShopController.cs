using System.Collections;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Model.Tb;
using System;
using CySoft.Frame.Core;
using System.Text;
using System.Collections.Generic;
using CySoft.Controllers.Filters;
using CySoft.Frame.Common;
using CySoft.Model.Enums;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Flags;
using System.Linq;
using CySoft.Utility;
using System.IO;
using CySoft.Model;
using System.Web;
using CySoft.Model.Mapping;

namespace CySoft.Controllers.ShopCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ShopController : BaseController
    {

        #region 门店列表
        /// <summary>
        /// 门店列表
        /// </summary>
        [ActionPurview(false)]
        public ActionResult List()
        {
            try
            {
                #region 辅助参数
                BaseResult br = new BaseResult();
                Hashtable param = base.GetParameters();
                if (param.ContainsKey("fromType") && !string.IsNullOrEmpty(param["fromType"].ToString()))
                    ViewBag.fromType = param["fromType"].ToString();
                #endregion

                #region 构建param
                int limit = base.PageSizeFromCookie;
                param.Add("id_masteruser", id_user_master);
                ParamVessel pv = new ParamVessel();
                pv.Add("_search_", "0", HandleType.DefaultValue);
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                pv.Add("keyword", "", HandleType.DefaultValue, true);
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("sort", " bm  asc", HandleType.DefaultValue);
                pv.Add("_view_page_", "1", HandleType.DefaultValue);
                param = param.Trim(pv);
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);
                #endregion

                #region 获取数据

                #region 获取分页数据
                ViewData["_view_page_"] = param["_view_page_"].ToString();

                param.Add("query_self_child", id_shop);

                var pn = BusinessFactory.Tb_Shop.GetPage(param);
                var plist = new PageList<Tb_Shop>(pn, pageIndex, limit);
                ViewData["List"] = plist;
                #endregion

                #region 查询主门店id
                Hashtable ht = new Hashtable();
                ht.Clear();
                ht.Add("id_masteruser", id_user_master);
                ht.Add("id", id_shop_master);
                var brShop = BusinessFactory.Tb_Shop.Get(ht);

                if (brShop.Success)
                    ViewBag.ShopMaster = brShop.Data;

                #endregion

                #region 获取开启门店数量
                ht.Clear();
                ht.Add("id_masteruser", id_user_master);
                ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                ht.Add("flag_state", (byte)Enums.TbShopFlagState.Opened);
                var openNum = BusinessFactory.Tb_Shop.GetUserShopCount(ht);
                ViewData["openNum"] = openNum;
                #endregion

                #region 获取购买的服务
                if (PublicSign.flagCheckService == "1")
                {
                    var bm = BusinessFactory.Account.GetServiceBM(version);
                    if (!string.IsNullOrEmpty(bm))
                    {
                        ht.Clear();
                        ht.Add("id_cyuser", id_cyuser);
                        ht.Add("bm", bm);
                        ht.Add("service", "GetService");
                        ht.Add("id_masteruser", id_user_master);
                        ht.Add("rq_create_master_shop", rq_create_master_shop.ToString());
                        var cyServiceHas = BusinessFactory.Account.GetCYService(ht);
                        if (cyServiceHas != null && cyServiceHas.ContainsKey("cyServiceList") && cyServiceHas.ContainsKey("endTime"))
                        {
                            ViewData["vEndData"] = cyServiceHas["endTime"].ToString();
                            var cyServiceList = (List<Schedule_UserService>)cyServiceHas["cyServiceList"];
                            if (cyServiceList.Count() == 1)
                            {
                                ViewData["vBuyNum"] = cyServiceList.FirstOrDefault().sl;
                            }
                        }
                    }
                }
                #endregion

                if (param["_search_"].ToString().Equals("1"))
                    return PartialView("_List");
                else
                    return View();
                #endregion

            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        }
        #endregion

        #region 新增门店
        /// <summary>
        /// 新增门店 UI 
        /// lz
        /// 2016-09-01
        /// </summary>
        [ActionPurview(false)]
        [HttpGet]
        public ActionResult Add()
        {
            Hashtable ht = new Hashtable();
            Hashtable param = base.GetParameters();
            if (param != null && param.ContainsKey("id"))
            {
                ht.Add("id", param["id"].ToString());
                var brUserShop = BusinessFactory.Tb_Shop.GetAll(ht);
                var copyShopModel= ((List<CySoft.Model.Tb.Tb_Shop>)brUserShop.Data).FirstOrDefault();
                ViewBag.CopyShopModel = copyShopModel;
            }

            if (param.ContainsKey("fromType") && !string.IsNullOrEmpty(param["fromType"].ToString()))
                ViewBag.fromType = param["fromType"].ToString();

            #region 获取门店信息
            ht.Clear();
            ht.Add("id_masteruser", id_user_master);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            ViewData["shopList"] = BusinessFactory.Tb_Shop.QueryShopSelectModels(ht).Data;


            ViewData["userShopList"] = GetShop(Enums.ShopDataType.GetBJXJList,id_shop); //本机加下级

            #endregion

            #region 查询主门店id
            //string id_shop_master = "";
            //ht.Clear();
            //ht.Add("id_masteruser", id_user_master);
            //ht.Add("flag_master", (int)Enums.TbUserFlagMaster.Master);
            //var brAccount = BusinessFactory.Account.GetAllUser(ht);

            //if (brAccount.Success && ((List<Tb_User>)brAccount.Data).FirstOrDefault() != null && !string.IsNullOrEmpty(((List<Tb_User>)brAccount.Data).FirstOrDefault().id_shop))
            //    id_shop_master = (((List<Tb_User>)brAccount.Data).FirstOrDefault()).id_shop;

            ht.Clear();
            ht.Add("id_masteruser", id_user_master);
            ht.Add("id", id_shop_master);
            var brShop = BusinessFactory.Tb_Shop.Get(ht);

            if (brShop.Success)
                ViewBag.ShopMaster = brShop.Data;

            #endregion

            #region 门店类型 下拉值
            var czfsBr = base.GetFlagList(Enums.TsFlagListCode.shoptype.ToString());
            if (czfsBr.Success)
                ViewBag.selectListTYPE = czfsBr.Data;
            #endregion
            //登录用户的门店类型
            ViewBag.flagTypeShop = flag_type_shop;


            //配送中心
            var pszxList = GetShop(Enums.ShopDataType.GetPSZXListForAdd, id_shop);
            ViewBag.selectListPSZX = pszxList;

            

            //客户






            return View();
        }


        /// <summary>
        /// 新增门店 
        /// lz
        /// 2016-09-01
        /// <returns></returns>
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult Add(Tb_Shop model)
        {
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            BaseResult br = new BaseResult();
            var oldParam = new Hashtable();
            var jumpUrl = "";

            try
            {
                ParamVessel pv = new ParamVessel();
                pv.Add("yzm", string.Empty, HandleType.ReturnMsg);//编码
                pv.Add("bm", string.Empty, HandleType.ReturnMsg);//编码
                pv.Add("mc", string.Empty, HandleType.ReturnMsg);//名称
                pv.Add("email", string.Empty, HandleType.DefaultValue);//电子邮箱
                pv.Add("phone", string.Empty, HandleType.DefaultValue);//移动电话
                pv.Add("tel", string.Empty, HandleType.DefaultValue);//联系电话
                pv.Add("fax", string.Empty, HandleType.DefaultValue);
                pv.Add("lxr", string.Empty, HandleType.DefaultValue);
                pv.Add("zipcode", string.Empty, HandleType.DefaultValue);
                pv.Add("address", string.Empty, HandleType.DefaultValue);
                pv.Add("bz", string.Empty, HandleType.DefaultValue);
                pv.Add("id_cloneshop", string.Empty, HandleType.DefaultValue);
                pv.Add("pic_path", string.Empty, HandleType.DefaultValue);//门店图片
                pv.Add("qq", string.Empty, HandleType.DefaultValue);//qq
                pv.Add("flag_type", (int)Enums.FlagShopType.直营店, HandleType.DefaultValue);//门店类型
                pv.Add("kh_id", string.Empty, HandleType.DefaultValue);//kh_id
                pv.Add("id_shop_ps", string.Empty, HandleType.DefaultValue);//id_shop_ps  //配送中心 
                param = param.Trim(pv);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                param.Add("id_shop_master", id_shop_master);
                param.Add("id_cyuser", id_cyuser);
                param.Add("rq_create_master_shop", rq_create_master_shop.ToString());
                param.Add("version", version);
                param.Add("id_shop_user", id_shop);
                oldParam = (Hashtable)param.Clone();

                // 图片 
                if (param["pic_path"] != null && !string.IsNullOrEmpty(param["pic_path"].ToString()))
                {
                    CheckImgPath();
                    string[] url_img = param["pic_path"].ToString().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    string guid = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(url_img[url_img.Length - 1]);
                    string fileName = guid + extension;
                    ImgExtension.MakeThumbnail(param["pic_path"].ToString(), "/UpLoad/Shops/thumb/_819x306_" + fileName, 819, 306, ImgCreateWay.Cut, false);
                    string newPath = string.Format("/UpLoad/Shops/thumb/_819x306_{0}", fileName);//480x480
                    param.Remove("pic_path");
                    param.Add("pic_path", newPath);
                }

                #region 获取购买服务的地址
                var bm = BusinessFactory.Account.GetServiceBM(version);
                if (string.IsNullOrEmpty(bm))
                {
                    return base.JsonString(new
                    {
                        status = "error",
                        message = "操作失败 获取服务编码异常 请检查版本是否正常！"
                    });
                }

                Hashtable ht = new Hashtable();
                ht.Clear();
                ht.Add("id_cyuser", id_cyuser);
                ht.Add("id", bm);
                ht.Add("phone", phone_master);
                ht.Add("service", "Detail");
                ht.Add("id_masteruser", id_user_master);
                string buyUrl = BusinessFactory.Tb_Shop.GetBuyServiceUrl(ht);
                if (string.IsNullOrEmpty(buyUrl))
                    buyUrl = PublicSign.cyBuyServiceUrl;

                jumpUrl = buyUrl = HttpUtility.UrlEncode(buyUrl, Encoding.UTF8);

                #endregion

                br = BusinessFactory.Tb_Shop.Add(param);
                WriteDBLog("新增门店", oldParam, br);
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
                br.Success = false;
                br.Data = "";
                br.Message.Clear();
                br.Message.Add(ex.Message);
                WriteDBLog("新增门店", oldParam, br);
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message),
                    level = 3,
                    url = jumpUrl
                });
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Data = "";
                br.Message.Add("数据不符合要求!");
                br.Level = ErrorLevel.Warning;
                WriteDBLog("新增门店", oldParam, br);
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
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
            if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/UpLoad/Shops")))//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/UpLoad/Shops"));

                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/UpLoad/Shops/thumb")))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/UpLoad/Shops/thumb"));
                }
            }
        }
        #endregion

        #region 修改门店 UI 
        /// <summary>
        /// 修改门店 UI 
        /// lz
        /// 2016-09-02
        /// </summary>
        [ActionPurview(false)]
        [HttpGet]
        public ActionResult Edit()
        {
            try
            {
                #region 获取修改的门店信息
                Hashtable param = base.GetParameters();
                Hashtable ht = new Hashtable();

                if (param.ContainsKey("fromType") && !string.IsNullOrEmpty(param["fromType"].ToString()))
                    ViewBag.fromType = param["fromType"].ToString();

                ParamVessel p = new ParamVessel();
                p.Add("id", string.Empty, HandleType.ReturnMsg);//ID
                param = param.Trim(p);
                var brShop = BusinessFactory.Tb_Shop.Get(param);
                var shopModel = (Tb_Shop)brShop.Data;
                if (brShop.Success)
                    ViewBag.ShopModel = shopModel;

                #endregion
                #region 获取绑定客户信息
                if (shopModel != null && !string.IsNullOrEmpty(shopModel.id_kh))
                {
                    ht.Clear();
                    ht.Add("id_masteruser", id_user_master);
                    ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                    ht.Add("id", shopModel.id_kh);
                    var brKH = BusinessFactory.Tb_Kh.Get(ht);
                    if (brKH.Success)
                    {
                        ViewBag.khModel = brKH.Data;
                    }
                } 
                #endregion
                #region 门店类型 下拉值
                var czfsBr = base.GetFlagList(Enums.TsFlagListCode.shoptype.ToString());
                if (czfsBr.Success)
                    ViewBag.selectListTYPE = czfsBr.Data;
                #endregion
                #region 登录用户的门店类型
                //登录用户的门店类型
                ViewBag.flagTypeShop = flag_type_shop;
                #endregion
                #region 配送中心下拉
                //配送中心下拉
                var pszxList = GetShop(Enums.ShopDataType.GetPSZXListForEdit, param["id"].ToString());
                ViewBag.selectListPSZX = pszxList; 
                #endregion

                var fatherList= GetShop(Enums.ShopDataType.GetFatherList, param["id"].ToString());
                if (fatherList != null && fatherList.Count() > 0 && fatherList.Where(d => d.id_shop.Trim() == id_shop_master.Trim()).Count() > 0)
                {
                    ViewBag.fatherIsMaster = "1";
                }

                ViewBag.version = version;
                ViewBag.is_sysmanager = is_sysmanager;

                    #region 注释
                    //Hashtable ht = new Hashtable();
                    //ht.Clear();
                    //ht.Add("id_masteruser", id_user_master);
                    //ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                    //ViewData["shopList"] = BusinessFactory.Tb_Shop.QueryShopSelectModels(ht).Data;
                    //ViewData["userShopList"] = GetUserShop(); 
                    #endregion

                    return View();
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

        #region 修改门店 
        /// <summary>
        /// 修改门店 
        /// lz
        /// 2016-09-02
        /// <returns></returns>
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult Edit(Tb_Shop model)
        {
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            br.Success = true;
            var jumpUrl = "";
            try
            {
                #region 获取数据
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", string.Empty, HandleType.ReturnMsg);//ID
                p.Add("yzm", string.Empty, HandleType.DefaultValue);//验证码
                p.Add("bm", string.Empty, HandleType.ReturnMsg);//编码
                p.Add("mc", string.Empty, HandleType.ReturnMsg);//名称
                p.Add("email", string.Empty, HandleType.DefaultValue);//电子邮箱
                p.Add("phone", string.Empty, HandleType.DefaultValue);//移动电话
                p.Add("tel", string.Empty, HandleType.DefaultValue);//联系电话
                p.Add("fax", string.Empty, HandleType.DefaultValue);
                p.Add("lxr", string.Empty, HandleType.DefaultValue);
                p.Add("zipcode", string.Empty, HandleType.DefaultValue);
                p.Add("address", string.Empty, HandleType.DefaultValue);
                p.Add("bz", string.Empty, HandleType.DefaultValue);
                p.Add("pic_path", string.Empty, HandleType.DefaultValue);//门店图片
                p.Add("flag_state", string.Empty, HandleType.Remove);
                p.Add("qq", string.Empty, HandleType.DefaultValue);
                p.Add("flag_type", string.Empty, HandleType.ReturnMsg);//门店类型
                p.Add("kh_id", string.Empty, HandleType.DefaultValue);//kh_id
                p.Add("id_shop_ps", string.Empty, HandleType.DefaultValue);//id_shop_ps  //配送中心
                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                param.Add("id_cyuser", id_cyuser);
                param.Add("rq_create_master_shop", rq_create_master_shop.ToString());
                param.Add("version", version);
                oldParam = (Hashtable)param.Clone();
                #endregion

                #region 图片
                // 图片 
                if (param["pic_path"] != null && !string.IsNullOrEmpty(param["pic_path"].ToString()))
                {
                    if (!param["pic_path"].ToString().Contains("/UpLoad/Shops/thumb/"))
                    {
                        CheckImgPath();
                        string[] url_img = param["pic_path"].ToString().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        string guid = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(url_img[url_img.Length - 1]);
                        string fileName = guid + extension;
                        ImgExtension.MakeThumbnail(param["pic_path"].ToString(), "/UpLoad/Shops/thumb/_480x480_" + fileName, 819, 306, ImgCreateWay.Cut, false);
                        string newPath = string.Format("/UpLoad/Shops/thumb/_480x480_{0}", fileName);//480x480
                        param.Remove("pic_path");
                        param.Add("pic_path", newPath);
                    }
                }
                #endregion

                if (param.ContainsKey("flag_state") && param["flag_state"].ToString() == ((byte)Enums.TbShopFlagState.Opened).ToString())
                {
                    if (PublicSign.flagCheckService == "1")
                    {

                        #region 根据版本获取 服务编码
                        var bm = BusinessFactory.Account.GetServiceBM(version);
                        if (string.IsNullOrEmpty(bm))
                        {
                            return base.JsonString(new
                            {
                                status = "error",
                                message = "操作失败 获取服务编码异常 请检查版本是否正常！"
                            });
                        }
                        #endregion

                        #region 获取购买服务的地址
                        Hashtable ht = new Hashtable();
                        ht.Add("id_cyuser", id_cyuser);
                        ht.Add("id", bm);
                        ht.Add("phone", phone_master);
                        ht.Add("service", "Detail");
                        ht.Add("id_masteruser", id_user_master);
                        string buyUrl = BusinessFactory.Tb_Shop.GetBuyServiceUrl(ht);
                        if (string.IsNullOrEmpty(buyUrl))
                            buyUrl = PublicSign.cyBuyServiceUrl;
                        buyUrl = HttpUtility.UrlEncode(buyUrl, Encoding.UTF8);
                        jumpUrl = buyUrl = HttpUtility.UrlEncode(buyUrl, Encoding.UTF8);
                        #endregion

                    }
                }
                else if (param.ContainsKey("flag_state") && param["flag_state"].ToString() == ((byte)Enums.TbShopFlagState.Closed).ToString())
                {
                    if (id_shop_master == param["id"].ToString())
                    {
                        return base.JsonString(new
                        {
                            status = "error",
                            message = "操作失败 主门店不允许停用！"
                        });
                    }
                }

                br = BusinessFactory.Tb_Shop.Save(param);
                WriteDBLog("编辑门店", oldParam, br);

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
                br.Success = false;
                br.Data = "";
                br.Message.Clear();
                br.Message.Add(ex.Message);
                WriteDBLog("编辑门店", oldParam, br);
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message),
                    level = 3,
                    url = jumpUrl
                });
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Data = "";
                br.Message.Add("数据不符合要求!");
                br.Level = ErrorLevel.Warning;
                WriteDBLog("编辑门店", oldParam, br);
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }
        }
        #endregion

        #region 门店详细
        /// <summary>
        /// 门店详细
        /// lz
        /// 2016-09-02
        /// </summary>
        [ActionPurview(false)]
        [HttpGet]
        public ActionResult Detail()
        {
            BaseResult br = new BaseResult();
            try
            {
                #region 获取并验证数据
                Hashtable ht = new Hashtable();
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", string.Empty, HandleType.ReturnMsg);//ID
                param = param.Trim(p);
                #endregion
                br = BusinessFactory.Tb_Shop.Get(param);
                if (br.Success)
                    ViewBag.ShopModel = br.Data;

                #region 获取绑定客户信息
                var shopModel = (Tb_Shop)br.Data;
                if (shopModel != null && !string.IsNullOrEmpty(shopModel.id_kh))
                {
                    ht.Clear();
                    ht.Add("id_masteruser", id_user_master);
                    ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                    ht.Add("id", shopModel.id_kh);
                    var brKH = BusinessFactory.Tb_Kh.Get(ht);
                    if (brKH.Success)
                    {
                        ViewBag.khModel = brKH.Data;
                    }
                }
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
            return View();
        }
        #endregion

        #region 生成条码
        [ActionPurview(false)]
        public ActionResult CreateBarcode()
        {
            BaseResult br = new BaseResult();
            try
            {
                br.Success = true;
                br.Data = GetNewDH(Enums.FlagDJLX.BMShop);
                return JsonString(br);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ShopList
        public ActionResult ShopList()
        {
            return View("ShopList");
        }
        #endregion

        #region ShopList2
        public ActionResult ShopList2()
        {
            Hashtable param_i = base.GetParameters();
            //PageNavigate pn = BusinessFactory.Tb_Shop.GetPage(param_i);
            Hashtable param_q = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("_search_", "0", HandleType.DefaultValue);

            try
            {
                param_q = param_i.Trim(pv);
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
            }

            if (param_q["_search_"].ToString().Equals("1"))
            {
                return PartialView("PartialView/P_ShopList");
            }
            else
            {
                return View("ShopList2");
            }
        }
        #endregion

        #region GetShopList
        public ActionResult GetShopList()
        {
            List<Model.Tb.Tb_Shop> list = new List<Tb_Shop>();
            for (int i = 0; i < 500; i++)
            {
                list.Add(new Tb_Shop()
                {
                    id = string.Format("id_{0}", Guid.NewGuid().ToString("N").Substring(0, 2)),
                    bm = string.Format("bm_{0}", Guid.NewGuid().ToString("N").Substring(0, 8)),
                    tel = string.Format("tel_{0}", Guid.NewGuid().ToString("N").Substring(0, 8))
                });
            }

            var result = new { list = list };

            return Content(content: Utility.JSON.Serialize(result),
                contentType: "application/json",
                contentEncoding: Encoding.UTF8);
        }
        #endregion

        #region 注释
        //public ActionResult GetShopList()
        //{
        //    var list = BusinessFactory.Tb_Shop.GetAll();
        //    return null;
        //}

        //public ActionResult Add()
        //{
        //    Tb_Shop shopModel = new Tb_Shop();
        //    if (TryUpdateModel(shopModel))
        //    {
        //        BusinessFactory.Tb_Shop.Add(shopModel);
        //    }
        //    return null;
        //} 
        #endregion

        #region ShopInfo
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult ShopInfo()
        {
            Hashtable param = base.GetParameters();
            param.Add("id_masteruser", id_user_master);
            param.Add("id", id_shop);
            ParamVessel pv = new ParamVessel();
            pv.Add("id_masteruser", String.Empty, HandleType.ReturnMsg);
            pv.Add("id", String.Empty, HandleType.ReturnMsg);
            param = param.Trim(pv);
            ViewData["entity"] = BusinessFactory.Tb_Shop.Get(param).Data;
            param.Clear();
            param.Add("id", id_user_master);
            var user = BusinessFactory.Account.GetUserInfo(param).Data as Tb_User;
            if (user != null)
            {
                ViewData["companyno"] = user.companyno;
                ViewData["flag_industry"] = user.flag_industry;
                ViewData["can_change"] = user.username == user.companyno && id_shop == id_shop_master;
            }
            ViewData["industryList"] = GetFlagList("industry").Data;
            return View();
        }
        #endregion

        #region ShopInfo
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult ShopInfo(string id)
        {
            var oldParam = new Hashtable();
            BaseResult res = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel pv = new ParamVessel();
                pv.Add("mc", String.Empty, HandleType.ReturnMsg);
                //pv.Add("bm", String.Empty, HandleType.ReturnMsg);
                pv.Add("email", "", HandleType.DefaultValue);
                pv.Add("phone", "", HandleType.DefaultValue);
                pv.Add("qq", String.Empty, HandleType.DefaultValue);
                pv.Add("address", String.Empty, HandleType.DefaultValue);
                pv.Add("lxr", String.Empty, HandleType.DefaultValue);
                pv.Add("bz", String.Empty, HandleType.DefaultValue);
                pv.Add("tel", String.Empty, HandleType.DefaultValue);
                pv.Add("zipcode", String.Empty, HandleType.DefaultValue);
                pv.Add("fax", String.Empty, HandleType.DefaultValue);
                pv.Add("id", String.Empty, HandleType.ReturnMsg);
                pv.Add("flag_industry", 0, HandleType.ReturnMsg);
                param = param.Trim(pv);
                param.Add("id_user", id_user);
                param.Add("id_masteruser", id_user_master);//
                oldParam = (Hashtable)param.Clone();
                res = BusinessFactory.Tb_Shop.Update(param);

            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message.Add("服务器异常!");
            }

            WriteDBLog("企业信息-保存", oldParam, res);

            if (res.Success)
            {
                return JsonString(new
                {
                    status = "success",
                    message = "执行成功,正在载入页面..."
                });
            }
            else
            {
                return JsonString(new
                {
                    status = "error",
                    message = string.Join(";", res.Message)
                });
            }
        }
        #endregion



        /// <summary>
        /// 重置验证码
        /// </summary>
        /// <returns></returns>.
        [ActionPurview(false)]
        [HttpGet]
        public ActionResult CZYzm(string id)
        {
            ViewData["id"] = id;
            return View("CZYzm");
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult CZYzm(Tb_Shop model)
        {
            var oldParam = new Hashtable();

            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            ParamVessel pv = new ParamVessel();
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            pv.Add("new_yzm", string.Empty, HandleType.ReturnMsg);
            //pv.Add("img_code", string.Empty, HandleType.ReturnMsg);

            #region 校验
            try
            {
                param = param.Trim(pv);
                oldParam.Add("id", param["id"].ToString());

                //object validCode = Session["SystemValidCode"];
                //if (validCode == null)
                //{
                //    br.Success = false;
                //    br.Message.Add("图片识别码已过期");
                //    return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                //}

                //Session.Remove("SystemValidCode");
                //if (!param["img_code"].ToString().Equals(validCode.ToString(), StringComparison.OrdinalIgnoreCase))
                //{
                //    br.Success = false;
                //    br.Message.Add("图片识别码错误");
                //    return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                //}

                if (string.IsNullOrEmpty(param["new_yzm"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add("新验证码不允许空");
                    return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                }

                if (is_sysmanager != "1")
                {
                    br.Success = false;
                    br.Message.Add("操作失败 请登录管理员角色重置");
                    return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                }

                var shop_shop= GetShop(Enums.ShopDataType.GetBJXJList,id_shop);
                if (shop_shop.Where(d => d.id_shop.Trim() == param["id"].ToString().Trim()).Count() <= 0)
                {
                    br.Success = false;
                    br.Message.Add("操作失败 你无权重置此门店信息");
                    return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                }

                br = BusinessFactory.Tb_Shop.ResetYZM(param);

            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Message.Add(ex.Message);
            }

            #endregion 校验
            WriteDBLog("重置验证码", oldParam, br);

            if (br.Success)
            {
                return base.JsonString(new
                {
                    status = "success",
                    message = "操作成功"
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



    }
}

