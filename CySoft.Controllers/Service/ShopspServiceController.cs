using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Service.Base;
using CySoft.Model;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Model.Ts;
using CySoft.Utility;
using CySoft.Frame.Core;
using CySoft.Controllers.Filters;
using System.Text.RegularExpressions;
using System.IO;
using CySoft.Model.Td;
using CySoft.Model.Enums;

//商品
namespace CySoft.Controllers.Service
{
    public class ShopspServiceController : ServiceBaseController
    {
        #region 新增商品
        /// <summary>
        /// 新增商品
        /// lz
        /// 2016-09-26
        /// </summary>
        //[HttpPost]
        public ActionResult Add()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//门店id *
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//主用户id *
                p.Add("id_user", string.Empty, HandleType.ReturnMsg);//用户id *
                p.Add("barcode", string.Empty, HandleType.ReturnMsg);//条码 *
                p.Add("bm", string.Empty, HandleType.ReturnMsg);//编码 *
                p.Add("mc", string.Empty, HandleType.ReturnMsg);//名称 *
                p.Add("dj_jh", 0m, HandleType.DefaultValue);//进货价 
                p.Add("dj_ls", 0m, HandleType.DefaultValue);//零售价 
                p.Add("dj_hy", 0m, HandleType.DefaultValue);//会员价 
                p.Add("dj_ps", 0m, HandleType.DefaultValue);//配送价 
                p.Add("dw", string.Empty, HandleType.ReturnMsg);//单位 *
                p.Add("sl_kc_min", 0m, HandleType.DefaultValue);//最底库存量 *
                p.Add("sl_kc_max", 0m, HandleType.DefaultValue);//最高库存量 *
                p.Add("flag_czfs", string.Empty, HandleType.ReturnMsg);//计价方式 *
                p.Add("id_spfl", string.Empty, HandleType.ReturnMsg);//分类ID *
                p.Add("yxq", 0, HandleType.DefaultValue);//保质期 天 

                p.Add("cd", string.Empty, HandleType.DefaultValue);//产地
                p.Add("pic_path", string.Empty, HandleType.DefaultValue);//图片路径
                p.Add("id_gys", string.Empty, HandleType.DefaultValue);//默认供应商
                p.Add("bz", string.Empty, HandleType.DefaultValue);//备注
                p.Add("dj_pf", 0m, HandleType.DefaultValue);//批发价  目前此字段没用

                p.Add("je_qc", 0m, HandleType.DefaultValue);//期初金额
                p.Add("sl_qc", 0m, HandleType.DefaultValue);//期初数量

                p.Add("dbzList", string.Empty, HandleType.DefaultValue);//多包装    

                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }
                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = base.GetTicketInfo(param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }
                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }
                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "id_user", "barcode", "bm", "mc", "dj_jh", "dj_ls", "dj_hy", "dj_ps", "dw", "sl_kc_min", "sl_kc_max", "flag_czfs", "id_spfl", "yxq", "cd", "pic_path", "id_gys", "bz", "je_qc", "sl_qc", "dbzList" });

                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 验证参数是否符合

                #region 多包装
                //多包装
                var dbzList = new List<Tb_Shopsp_DBZ>();
                try
                {
                    dbzList = JSON.Deserialize<List<Tb_Shopsp_DBZ>>(param["dbzList"].ToString()) ?? new List<Tb_Shopsp_DBZ>();
                    foreach (var item in dbzList)
                    {
                        item.id = Guid.NewGuid().ToString();
                        item.info_type = "add";
                    }
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = "操作失败 多包装不符合要求!";
                    return res;
                }
                #endregion

                #region 期初
                //期初
                var qcModel = new Td_Sp_Qc() { sl_qc = decimal.Parse(param["sl_qc"].ToString()), je_qc = decimal.Parse(param["je_qc"].ToString()) };
                #endregion

                #region 验证数据
                //控制层验证数据
                var brCheck = this.CheckParam(param, qcModel, dbzList);
                if (!brCheck.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = string.Join(";", brCheck.Message);
                    return res;
                }
                #endregion

                #region 数据处理

                #region 图片
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
                #endregion

                #region 多包装
                if (param.ContainsKey("dbzList"))
                    param.Remove("dbzList");
                param.Add("dbzList", dbzList);
                #endregion

                #region 期初
                if (param.ContainsKey("sp_qc"))
                    param.Remove("sp_qc");
                param.Add("sp_qc", qcModel);
                #endregion

                #region 用户管理门店
                var brShopShop = this.GetUserShopShop(param["id_masteruser"].ToString(), param["id_shop"].ToString(), param["id_user"].ToString());
                if (!brShopShop.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = string.Join(";", brShopShop.Message);
                    return res;
                }
                var shopShopList = (List<Tb_User_ShopWithShopMc>)brShopShop.Data;
                param.Add("shop_shop", shopShopList);
                #endregion

                #region 获取小数点位数
                var DigitHashtable = BusinessFactory.Account.GetParm(param["id_masteruser"].ToString());
                if (!param.ContainsKey("DigitHashtable"))
                    param.Add("DigitHashtable", DigitHashtable);
                #endregion

                #endregion

                #endregion
                #region 保存至本地图片
                if (param.ContainsKey("pic_path") && !string.IsNullOrEmpty(param["pic_path"].ToString()))
                {
                    var pic_path = BusinessFactory.Tb_Shopsp.GetBarcodePic(new Tb_Shopsp_Service() { BarCode = param["barcode"].ToString(), Picture = param["pic_path"].ToString() });
                    param["pic_path"] = pic_path;
                }
                #endregion
                #region 新增
                var br = BusinessFactory.Tb_Shopsp.Add(param);
                #endregion
                #region 返回
                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                if (br.Success)
                    res.Data = br.Data;
                return res;
                #endregion
            });
            return JsonString(sr);
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

        #region GetUserShopShop
        /// <summary>
        /// 获取Tb_Shop_Shop信息
        /// lz 2016-11-07
        /// </summary>
        /// <returns></returns>
        protected BaseResult GetUserShopShop(string id_user_master, string id_shop, string id_user)
        {
            BaseResult br = new BaseResult();
            #region 获取Tb_Shop_Shop信息
            List<Tb_User_ShopWithShopMc> shopList = new List<Tb_User_ShopWithShopMc>();

            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            var brUserList = BusinessFactory.Account.GetAllUser(param);
            if (!brUserList.Success)
            {
                br.Success = false;
                br.Message.Add(" 操作失败 查询主用户门店相关信息时发生异常 ");
                return br;
            }

            var userList = (List<Tb_User>)brUserList.Data;
            if (userList == null || userList.Count() <= 0)
            {
                br.Success = false;
                br.Message.Add(" 操作失败 未查询到主用户相关用户信息 ");
                return br;
            }

            if (userList.Where(d => d.id == id_user).Count() <= 0)
            {
                br.Success = false;
                br.Message.Add(" 操作失败 该主用户下不包含此用户id的用户 ");
                return br;
            }

            var id_shop_master_model = userList.Where(d => d.id == id_user_master).FirstOrDefault();
            if (id_shop_master_model == null || string.IsNullOrEmpty(id_shop_master_model.id))
            {
                br.Success = false;
                br.Message.Add(" 操作失败 未查询到主用户的相关信息 ");
                return br;
            }

            var id_shop_master = id_shop_master_model.id_shop;

            param.Clear();
            param.Add("id_masteruser", id_user_master);
            if (id_shop_master != id_shop)
                param.Add("shopShopID", id_shop);
            else
                param.Add("shopShopID", "0");

            var shopshopList = BusinessFactory.Tb_Shop_Shop.GetAll(param).Data as List<Tb_Shop_ShopWithMc>;
            if (shopshopList.Any())
            {
                shopshopList.ForEach(ssl =>
                {
                    shopList.Add(new Tb_User_ShopWithShopMc()
                    {
                        id_masteruser = ssl.id_masteruser,
                        id_shop = ssl.id_shop_child,
                        id_user = id_user,
                        mc = ssl.mc,
                        bm = ssl.bm,
                        rq_create = ssl.rq_create
                    });
                });
            }

            #endregion
            #region 添加登陆者当前门店信息
            if (shopList.Where(d => d.id_masteruser == id_user_master && d.id_shop == id_shop).Count() <= 0)
            {
                param.Clear();
                param.Add("id_masteruser", id_user_master);
                param.Add("id", id_shop);
                var brSelfShop = BusinessFactory.Tb_Shop.Get(param);
                if (brSelfShop.Success)
                {
                    Tb_Shop selfShop = (Tb_Shop)brSelfShop.Data;
                    if (selfShop != null && !string.IsNullOrEmpty(selfShop.id))
                    {
                        shopList.Add(new Tb_User_ShopWithShopMc()
                        {
                            id_masteruser = id_user_master,
                            id_shop = id_shop,
                            id_user = id_user,
                            mc = selfShop.mc,
                            bm = selfShop.bm,
                            rq_create = selfShop.rq_create
                        });
                    }
                }
            }
            #endregion
            br.Success = true;
            br.Data = shopList;
            return br;
        }
        #endregion

        #region 生成条码

        [ActionPurview(false)]
        [HttpPost]
        public ActionResult CreateBarcode()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("flag_czfs", string.Empty, HandleType.ReturnMsg);//flag_czfs
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }
                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = base.GetTicketInfo(param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }
                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }
                var ticket = ticketModel.ticket;
                #endregion

                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "flag_czfs" });
                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion

                #region 获取单号
                string barcode = string.Empty;
                if (param != null && param.ContainsKey("flag_czfs") && (param["flag_czfs"].ToString() == "1" || param["flag_czfs"].ToString() == "2"))
                    barcode = GetNewDH(Enums.FlagDJLX.BMShopspCZFS, param["id_masteruser"].ToString(), param["id_shop"].ToString());
                else
                    barcode = GetNewDH(Enums.FlagDJLX.BMShopsp, param["id_masteruser"].ToString(), param["id_shop"].ToString());
                #endregion
                #region 返回
                res.State = (!string.IsNullOrEmpty(barcode)) ? ServiceState.Done : ServiceState.Fail;
                res.Message = "";
                res.Data = new { barcode = barcode };
                return res;
                #endregion
            });
            return JsonString(sr);

        }

        #endregion

        #region 根据条码获取商品信息
        [ActionPurview(false)]
        //[HttpPost]
        public ActionResult GetShopspByBarcode()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("barcode", string.Empty, HandleType.ReturnMsg);//barcode
                p.Add("query_type", string.Empty, HandleType.DefaultValue);//query_type  0：查询所有 1:只查询本地2:只查询条码库 如果此字段不传 则只查询本地库
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }
                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = base.GetTicketInfo(param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }
                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }
                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "barcode", "query_type" });
                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 获取商品信息



                BaseResult br = new BaseResult();
                if (param.ContainsKey("query_type") && (param["query_type"].ToString() == "1"))
                {
                    br = GetShopSpByLocal(param);
                }
                else if (param.ContainsKey("query_type") && param["query_type"].ToString() == "2")
                {
                    br = GetShopSpByTMK(param);
                }
                else if (param.ContainsKey("query_type") && param["query_type"].ToString() == "0" || string.IsNullOrEmpty(param["query_type"].ToString()))
                {
                    br = GetShopSpByLocal(param);
                    if (br.Success)
                    {
                        if (br.Data == null)
                        {
                            //本地无数据 调用条码库接口查询
                            br = GetShopSpByTMK(param);
                        }
                    }
                }


                #region 注释
                //ht.Clear();
                //ht.Add("barcode", param["barcode"].ToString());
                //ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                //ht.Add("id_masteruser", param["id_masteruser"].ToString());
                //var br = BusinessFactory.Tb_Shopsp.GetShopspList(ht);
                //if (br.Success)
                //{
                //    List<ShopspList_Query> shopspList = (List<ShopspList_Query>)br.Data;
                //    if (shopspList.Count() > 0)
                //    {
                //        var ShopSp = shopspList.FirstOrDefault();
                //        br.Success = true;
                //        br.Data = ShopSp;
                //        br.Level = ErrorLevel.Alert;
                //    }
                //    else
                //    {
                //        br.Level = ErrorLevel.Alert;
                //        br.Success = true;
                //        br.Data = new { };

                //        if (param.ContainsKey("query_type") && param["query_type"].ToString() == "0")
                //        {
                //        }
                //    }
                //} 
                #endregion

                #endregion
                #region 返回
                if (br.Data == null)
                    br.Data = new { };
                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = "";
                res.Data = br.Data;
                res.Number = ((int)br.Level).ToString();
                return res;
                #endregion
            });


            #region 数据处理
            var jsonString = JSON.Serialize(sr); ;
            string rp = @"\\/Date\((\d+)\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(this.ConvertJsonDateToDateString);
            Regex reg = new Regex(rp);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            string p2 = @"\\/Date\(([/+/-]\d+)\)\\/";
            MatchEvaluator matchEvaluator2 = new MatchEvaluator(this.ConvertJsonDateToDateString);
            Regex reg2 = new Regex(p2);
            jsonString = reg2.Replace(jsonString, matchEvaluator2);
            #endregion

            return Content(jsonString);

        }
        #endregion

        #region 将Json序列化的时间由/Date(1294499956278+0800)转为字符串    
        /// <summary>    
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串    
        /// lz
        /// 2016-10-25
        /// </summary>    
        private string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }
        #endregion

        #region 根据条码获取服务商品信息
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult GetServiceShopspByBarcodeDB()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("barcode", string.Empty, HandleType.ReturnMsg);//barcode
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }
                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = base.GetTicketInfo(param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }
                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }
                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "barcode" });
                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 调用接口获取数据

                var paramters = new Dictionary<string, string>();
                paramters.Add("barCode", param["barcode"].ToString());
                paramters.Add("sign", SignUtils.SignRequestForCyUserSys(paramters, PublicSign.shopspMD5Key));
                var webutils = new CySoft.Utility.WebUtils();
                var respStr = webutils.DoPost(PublicSign.shopspUrl, paramters, 30000);
                var respModel = JSON.Deserialize<ServiceResult>(respStr);

                if (respModel != null)
                {
                    if (respModel.State != ServiceState.Done)
                    {
                        res.State = ServiceState.Fail;
                        res.Message = "操作失败,调用商品服务接口失败,Message:" + respModel.Message;
                        res.Data = respModel;
                        return res;
                    }
                    else
                    {
                        if (respModel.Data != null)
                        {
                            if (!string.IsNullOrEmpty(respModel.Data.ToString()))
                            {
                                var dbModel = JSON.Deserialize<Tb_Shopsp_Service>(respModel.Data.ToString());
                                if (dbModel.BarCode != null)
                                    dbModel.BarCode = dbModel.BarCode.Trim();
                                if (dbModel.ProductName != null)
                                    dbModel.ProductName = dbModel.ProductName.Trim();
                                if (dbModel.Unit != null)
                                    dbModel.Unit = dbModel.Unit.Trim();
                                if (dbModel.Picture != null)
                                    dbModel.Picture = dbModel.Picture.Trim();
                                res.Data = dbModel;
                            }
                        }
                    }
                }

                #endregion
                #region 返回
                res.State = ServiceState.Done;
                res.Message = "操作成功";

                return res;
                #endregion
            });

            #region 数据处理
            var jsonString = JSON.Serialize(sr); ;
            string rp = @"\\/Date\((\d+)\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(this.ConvertJsonDateToDateString);
            Regex reg = new Regex(rp);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            string p2 = @"\\/Date\(([/+/-]\d+)\)\\/";
            MatchEvaluator matchEvaluator2 = new MatchEvaluator(this.ConvertJsonDateToDateString);
            Regex reg2 = new Regex(p2);
            jsonString = reg2.Replace(jsonString, matchEvaluator2);
            #endregion

            return Content(jsonString);

        }
        #endregion

        #region GetShopSpByLocal
        public BaseResult GetShopSpByLocal(Hashtable param)
        {
            Hashtable ht = new Hashtable();
            ht.Clear();
            ht.Add("barcode", param["barcode"].ToString());
            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            var br = BusinessFactory.Tb_Shopsp.GetShopspList(ht);
            if (br.Success)
            {
                List<ShopspList_Query> shopspList = (List<ShopspList_Query>)br.Data;
                if (shopspList.Count() > 0)
                {
                    //if (param.ContainsKey("id_shop"))
                    //{
                    //    var ShopSp = shopspList.Where(d => d.id_shop.Trim() == param["id_shop"].ToString().Trim()).FirstOrDefault();
                    //    ShopSp.source = "server";
                    //    br.Data = ShopSp;
                    //    br.Success = true;
                    //}
                    //else
                    //{

                    var ShopSp = shopspList.FirstOrDefault();
                    ShopSp.source = "server";
                    br.Success = true;
                    br.Data = ShopSp;

                    //}

                    br.Level = ErrorLevel.Alert;

                }
                else
                {
                    br.Level = ErrorLevel.Alert;
                    br.Success = true;
                    br.Data = null;
                }
            }
            return br;
        }
        #endregion

        #region GetShopSpByTMK
        public BaseResult GetShopSpByTMK(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Level = ErrorLevel.Question;
            #region 调用接口获取数据
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

                    if (respModel != null)
                    {
                        if (respModel.State != ServiceState.Done)
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add(string.Format("查询接口失败"));
                            br.Data = null;
                            return br;
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
                                    var rModel = new ShopspList_Query()
                                    {
                                        bm = dbModel.BarCode,
                                        barcode = dbModel.BarCode,
                                        mc = dbModel.ProductName,
                                        dw = dbModel.Unit,
                                        dj_ls = dbModel.SellingPrice,
                                        pic_path = dbModel.Picture,
                                        source = "barcodedb"
                                    };
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

                                    br.Data = rModel;
                                    return br;
                                }
                            }
                        }
                    }
                    else
                    {
                        br.Success = false;
                        br.Message.Clear();
                        br.Message.Add(string.Format("查询接口数据出现异常 请重试"));
                        br.Data = null;
                        return br;
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
                    return br;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Message.Clear();
                br.Message.Add(string.Format("查询接口出现异常 请重试"));
                br.Data = null;
                return br;
            }
            #endregion
            return br;
        }
        #endregion

    }
}
