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

//会员
namespace CySoft.Controllers.MemberCtl
{
    [LoginActionFilter]
    public class HyController : BaseController
    {
        #region 会员-查询
        /// <summary>
        /// 会员-查询
        /// lz 2016-09-18
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
                param = param.Trim(pv);
                param.Add("hy_flag_delete", (int)Enums.FlagDelete.NoDelete);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);
                #endregion

                //获取前台控制小数点
                ViewBag.DigitHashtable = GetParm();

                #region 判断是否共享的处理
                var plist = new PageList<Tb_Hy_Shop_Query>(new PageNavigate() { TotalCount = 0, Data = null }, pageIndex, limit);
                var br_hy_shopshare = BusinessFactory.Account.GetHy_ShopShare(id_shop, id_user_master);// GetHy_ShopShare(id_shop);
                if (!br_hy_shopshare.Success)
                {
                    plist.PageIndex = pageIndex;
                    plist.PageSize = limit;
                    ViewData["List"] = plist;
                    ViewData["sort"] = param["sort"];
                    if (param["_search_"].ToString().Equals("1"))
                        return PartialView("_List");
                    else
                        return View();
                }

                var param_hy_shopshare = (Hashtable)br_hy_shopshare.Data;

                var hy_shopshare = param_hy_shopshare["hy_shopshare"].ToString();
                if (hy_shopshare != ((int)Enums.FlagShopShare.Shared).ToString())
                    param.Add("id_shop", param_hy_shopshare["id_shop"].ToString());

                #endregion
                #region 获取数据
                PageNavigate pn = new PageNavigate();
                pn = BusinessFactory.Tb_Hy_Shop.GetPage(param);
                plist = new PageList<Tb_Hy_Shop_Query>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"];
                var yhlxBr = base.GetFlagList(Enums.TsFlagListCode.hyyhlx.ToString());
                if (yhlxBr.Success)
                    ViewBag.YHLXSelect = yhlxBr.Data;
                #endregion
                #region 返回
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

        #region 会员-新增
        /// <summary>
        /// 会员-新增 
        /// lz
        /// 2016-09-18
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Add()
        {
            ViewData["option"] = "add";//操作类型
            ViewData["userShopList"] = GetCurrentUserMgrShop(id_user, id_shop);// GetShop(Enums.ShopDataType.UserShop); //GetUserShop();//用户管理门店
            ViewData["hyflList"] = GetHyfl();//会员类别
            ViewData["item_edit"] = new Tb_Hy_Shop_Query();
            ViewData["version"] = version;
            var yhlxBr = base.GetFlagList(Enums.TsFlagListCode.hyyhlx.ToString());
            if (yhlxBr.Success)
                ViewBag.YHLXSelect = yhlxBr.Data;
            ViewData["id_shop"] = id_shop;


            //bool isShare = false;
            //var br_hy_shopshare = BusinessFactory.Account.GetHy_ShopShare(id_shop, id_user_master);// GetHy_ShopShare(id_shop);
            //if (br_hy_shopshare.Success)
            //{
            //    var param_hy_shopshare = (Hashtable)br_hy_shopshare.Data;
            //    var hy_shopshare = param_hy_shopshare["hy_shopshare"].ToString();
            //    if (hy_shopshare == ((int)Enums.FlagShopShare.Shared).ToString())
            //        isShare = true;
            //}

            //if(isShare)
            //    ViewData["userShopList"] = GetShop(Enums.ShopDataType.UserShop);
            //else
            //    ViewData["userShopList"] = GetShop(Enums.ShopDataType.UserShop);



            return View();
        }
        #endregion

        #region 会员-Post新增
        /// <summary>
        /// 会员-Post新增
        /// lz
        /// 2016-09-18
        /// </summary>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Add(Tb_Hy model)
        {
            BaseResult br = new BaseResult();
            var oldParam = new Hashtable();
            try
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("name", string.Empty, HandleType.ReturnMsg);//name
                p.Add("membercard", string.Empty, HandleType.DefaultValue);//membercard
                p.Add("phone", string.Empty, HandleType.ReturnMsg);//phone
                p.Add("qq", string.Empty, HandleType.DefaultValue);//qq
                p.Add("email", string.Empty, HandleType.DefaultValue);//email
                p.Add("tel", string.Empty, HandleType.DefaultValue);//tel
                p.Add("address", string.Empty, HandleType.DefaultValue);//address
                p.Add("MMno", string.Empty, HandleType.DefaultValue);//MMno
                p.Add("zipcode", string.Empty, HandleType.DefaultValue);//zipcode
                p.Add("birthday", string.Empty, HandleType.DefaultValue);//birthday
                p.Add("flag_nl", "0", HandleType.DefaultValue);//flag_nl 是否农历
                p.Add("id_shop_create", string.Empty, HandleType.Remove);//id_shop_create
                p.Add("id_hyfl", string.Empty, HandleType.ReturnMsg);//id_hyfl
                p.Add("rq_b", string.Empty, HandleType.ReturnMsg);//rq_b
                p.Add("rq_b_end", string.Empty, HandleType.ReturnMsg);//rq_b_end
                p.Add("birth_month", "", HandleType.DefaultValue);//birth_month
                p.Add("birth_day", "", HandleType.DefaultValue);//birth_day
                p.Add("zk", "0.00", HandleType.DefaultValue);//zk
                p.Add("flag_sex", "1", HandleType.DefaultValue);//flag_sex
                p.Add("password", "", HandleType.DefaultValue);//password
                p.Add("flag_yhlx", "1", HandleType.DefaultValue);//flag_yhlx

                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                param.Add("rq_e", param["rq_b_end"].ToString());
                param.Remove("rq_b_end");
                oldParam = (Hashtable)param.Clone();
                #endregion
                #region 参数验证
                if (string.IsNullOrEmpty(param["membercard"].ToString()) && string.IsNullOrEmpty(param["phone"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add("会员卡号和手机号必须二选一!");
                    WriteDBLog("会员-新增", oldParam, br);
                    return base.JsonString(br, 1);
                }

                if (string.IsNullOrEmpty(param["zk"].ToString()) || !CyVerify.IsNumeric(param["zk"].ToString()) || decimal.Parse(param["zk"].ToString()) < 0 || decimal.Parse(param["zk"].ToString()) > 1)
                {
                    br.Success = false;
                    br.Message.Add("会员折扣不符合要求 折扣必须在0-1之间!");
                    WriteDBLog("会员-新增", oldParam, br);
                    return base.JsonString(br, 1);
                }

                if (!string.IsNullOrEmpty(param["birthday"].ToString()))
                {
                    //计算生日
                    DateTime birthday = DateTime.Parse(param["birthday"].ToString());
                    string hysr = birthday.ToString("MMdd");
                    param.Add("hysr", hysr);
                }
                else
                {
                    if (!string.IsNullOrEmpty(param["birth_month"].ToString()) && !string.IsNullOrEmpty(param["birth_day"].ToString()))
                    {
                        var month = param["birth_month"].ToString();
                        if (month.Length > 2 || month.Length < 1)
                            month = "00";
                        else if (month.Length == 1)
                            month = "0" + month;
                        var day = param["birth_day"].ToString();
                        if (day.Length > 2 || day.Length < 1)
                            day = "00";
                        else if (day.Length == 1)
                            day = "0" + day;
                        param.Add("hysr", month + day);
                    }
                }

                if (!string.IsNullOrEmpty(param["password"].ToString()) && !CyVerify.IsNumeric(param["password"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add("支付密码目前只允许整数!");
                    WriteDBLog("会员-新增", oldParam, br);
                    return base.JsonString(br, 1);
                }

                if (!string.IsNullOrEmpty(param["flag_yhlx"].ToString()) && param["flag_yhlx"].ToString() == "2" && decimal.Parse(param["zk"].ToString()) != 1)
                {
                    br.Success = false;
                    br.Message.Add("优惠类型为会员价 折扣只能为1!");
                    WriteDBLog("会员-新增", oldParam, br);
                    return base.JsonString(br, 1);
                }

                #endregion
                #region 判断是否共享的处理

                if (param.ContainsKey("id_shop_create"))
                {
                    var br_Hy_ShopShare = BusinessFactory.Account.GetHy_ShopShare(param["id_shop_create"].ToString(), id_user_master);// GetHy_ShopShare(param["id_shop_create"].ToString());
                    if (!br_Hy_ShopShare.Success)
                        return base.JsonString(br, 1);
                    var param_Hy_ShopShare = (Hashtable)br_Hy_ShopShare.Data;
                    param.Add("id_shop", param_Hy_ShopShare["id_shop"].ToString());
                }
                else
                {
                    param.Add("id_shop", id_shop);
                    param.Add("id_shop_create", id_shop);

                }

                #endregion
                #region 新增
                br = BusinessFactory.Tb_Hy_Shop.Add(param);
                #endregion
                #region 返回
                WriteDBLog("会员-新增", oldParam, br);
                return base.JsonString(br, 1);
                #endregion
            }
            catch (Exception ex)
            {
                #region 异常返回
                br.Success = false;
                br.Data = "";
                br.Message.Add("数据不符合要求!");
                br.Level = ErrorLevel.Warning;
                WriteDBLog("会员-新增", oldParam, br);
                return base.JsonString(br, 1);
                #endregion
            }
        }

        #endregion

        #region 会员-编辑
        /// <summary>
        /// 会员-编辑 
        /// lz
        /// 2016-09-19
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Edit()
        {
            try
            {
                ViewData["option"] = "edit";//操作类型
                //ViewData["userShopList"] = GetShop(Enums.ShopDataType.UserShop); //GetUserShop();//用户管理门店
                ViewData["hyflList"] = GetHyfl();//会员类别

                Hashtable param = base.GetParameters();
                param.Add("id_masteruser", id_user_master);
                ParamVessel p = new ParamVessel();
                p.Add("id_masteruser", id_user_master, HandleType.DefaultValue);
                p.Add("id", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                ViewData["version"] = version;

                //ViewData["item_edit"] = BusinessFactory.Tb_Hy_Shop.Get(param).Data;
                var br = BusinessFactory.Tb_Hy_Shop.Get(param);
                var data = (Tb_Hy_Shop_Query)br.Data;
                ViewData["item_edit"] = data;
                //var userShopList = GetShop(Enums.ShopDataType.All).Where(d => d.id_shop == data.hy_id_shop_create).ToList();
                ViewData["userShopList"] = GetCurrentUserMgrShop(id_user, id_shop);// userShopList;

                var yhlxBr = base.GetFlagList(Enums.TsFlagListCode.hyyhlx.ToString());
                if (yhlxBr.Success)
                    ViewBag.YHLXSelect = yhlxBr.Data;


                return View("Edit");
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        }
        #endregion

        #region 会员-Post编辑
        /// <summary>
        /// 会员-Post编辑
        /// lz 2016-09-20
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Edit(Tb_Hy model)
        {
            BaseResult br = new BaseResult();
            var oldParam = new Hashtable();
            try
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("name", string.Empty, HandleType.ReturnMsg);//name
                p.Add("membercard", string.Empty, HandleType.DefaultValue);//membercard
                p.Add("phone", string.Empty, HandleType.DefaultValue);//phone
                p.Add("qq", string.Empty, HandleType.DefaultValue);//qq
                p.Add("email", string.Empty, HandleType.DefaultValue);//email
                p.Add("tel", string.Empty, HandleType.DefaultValue);//tel
                p.Add("address", string.Empty, HandleType.DefaultValue);//address
                p.Add("MMno", string.Empty, HandleType.DefaultValue);//MMno
                p.Add("zipcode", string.Empty, HandleType.DefaultValue);//zipcode
                p.Add("birthday", string.Empty, HandleType.DefaultValue);//birthday
                p.Add("flag_nl", "0", HandleType.DefaultValue);//flag_nl 是否农历
                p.Add("id_shop_create", string.Empty, HandleType.Remove);//id_shop_create
                p.Add("id_hyfl", string.Empty, HandleType.ReturnMsg);//id_hyfl
                p.Add("rq_b", string.Empty, HandleType.ReturnMsg);//rq_b
                p.Add("rq_b_end", string.Empty, HandleType.ReturnMsg);//rq_b_end
                p.Add("birth_month", "", HandleType.DefaultValue);//birth_month
                p.Add("birth_day", "", HandleType.DefaultValue);//birth_day
                p.Add("zk", "0.00", HandleType.DefaultValue);//zk
                p.Add("flag_sex", "1", HandleType.DefaultValue);//flag_sex
                p.Add("flag_yhlx", "1", HandleType.DefaultValue);//flag_yhlx
                p.Add("password", "", HandleType.Remove);//password


                p.Add("id", "", HandleType.ReturnMsg);//id
                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                param.Add("rq_e", param["rq_b_end"].ToString());
                param.Remove("rq_b_end");
                oldParam = (Hashtable)param.Clone();
                #endregion
                #region 参数验证
                if (string.IsNullOrEmpty(param["membercard"].ToString()) && string.IsNullOrEmpty(param["phone"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add("会员卡号和手机号必须二选一!");
                    WriteDBLog("会员-编辑", oldParam, br);
                    return base.JsonString(br, 1);
                }

                if (string.IsNullOrEmpty(param["zk"].ToString()) || !CyVerify.IsNumeric(param["zk"].ToString()) || decimal.Parse(param["zk"].ToString()) < 0 || decimal.Parse(param["zk"].ToString()) > 1)
                {
                    br.Success = false;
                    br.Message.Add("会员折扣不符合要求 折扣必须在0-1之间!");
                    WriteDBLog("会员-编辑", oldParam, br);
                    return base.JsonString(br, 1);
                }

                if (!string.IsNullOrEmpty(param["flag_yhlx"].ToString()) && param["flag_yhlx"].ToString() == "2" && decimal.Parse(param["zk"].ToString()) != 1)
                {
                    br.Success = false;
                    br.Message.Add("优惠类型为会员价 折扣只能为1!");
                    WriteDBLog("会员-编辑", oldParam, br);
                    return base.JsonString(br, 1);
                }

                if (!string.IsNullOrEmpty(param["birthday"].ToString()))
                {
                    //计算生日
                    DateTime birthday = DateTime.Parse(param["birthday"].ToString());
                    string hysr = birthday.ToString("MMdd");
                    param.Add("hysr", hysr);
                }
                else
                {
                    if (!string.IsNullOrEmpty(param["birth_month"].ToString()) && !string.IsNullOrEmpty(param["birth_day"].ToString()))
                    {
                        var month = param["birth_month"].ToString();
                        if (month.Length > 2 || month.Length < 1)
                            month = "00";
                        else if (month.Length == 1)
                            month = "0" + month;
                        var day = param["birth_day"].ToString();
                        if (day.Length > 2 || day.Length < 1)
                            day = "00";
                        else if (day.Length == 1)
                            day = "0" + day;
                        param.Add("hysr", month + day);
                    }
                }
                #endregion
                #region 判断是否共享的处理
                if (param.ContainsKey("id_shop_create"))
                {
                    var br_Hy_ShopShare = BusinessFactory.Account.GetHy_ShopShare(param["id_shop_create"].ToString(), id_user_master);// GetHy_ShopShare(param["id_shop_create"].ToString());
                    if (!br_Hy_ShopShare.Success)
                        return base.JsonString(br, 1);
                    var param_Hy_ShopShare = (Hashtable)br_Hy_ShopShare.Data;
                    param.Add("id_shop", param_Hy_ShopShare["id_shop"].ToString());
                }
                else
                {
                    param.Add("id_shop", id_shop);
                    param.Add("id_shop_create", id_shop);
                }
                #endregion
                #region 新增
                br = BusinessFactory.Tb_Hy_Shop.Update(param);
                #endregion
                #region 返回
                WriteDBLog("会员-编辑", oldParam, br);
                return base.JsonString(br, 1);
                #endregion
            }
            catch (Exception ex)
            {
                #region 异常返回
                br.Success = false;
                br.Data = "";
                br.Message.Add("数据不符合要求!");
                br.Level = ErrorLevel.Warning;
                WriteDBLog("会员-编辑", oldParam, br);
                return base.JsonString(br, 1);
                #endregion
            }
        }
        #endregion

        #region 会员-删除
        /// <summary>
        /// 会员-删除
        /// lz 2016-09-20
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
                br = BusinessFactory.Tb_Hy_Shop.Delete(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("会员-删除", oldParam, br);
            return JsonString(br, 1);
        }
        #endregion

        #region GetHyfl 获取会员类别信息
        /// <summary>
        /// GetHyfl 获取会员类别信息
        /// lz
        /// 2016-09-19
        /// </summary>
        protected List<Tb_Hyfl> GetHyfl()
        {
            Hashtable ht = new Hashtable();
            var hyflList = new List<Tb_Hyfl>();
            ht.Add("id_masteruser", id_user_master);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            var br = BusinessFactory.Tb_Hyfl.GetAll(ht);
            if (br.Success)
            {
                hyflList = (List<Tb_Hyfl>)br.Data;
            }
            return hyflList;
        }
        #endregion



    }
}
