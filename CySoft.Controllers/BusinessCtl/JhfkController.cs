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
using CySoft.Model.Tz;

//进货付款
namespace CySoft.Controllers.BusinessCtl
{
    [LoginActionFilter]
    public class JhfkController : BaseController
    {

        #region 进货付款-查询
        /// <summary>
        /// 进货付款-查询
        /// lz 2016-09-20
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
                pv.Add("flag_cancel", String.Empty, HandleType.Remove);
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
                    param.Add("start_rq_create", param["start_time"].ToString());
                    param.Remove("start_time");
                }
                if (param.ContainsKey("start_time_end"))
                {
                    param.Add("end_rq_create", param["start_time_end"].ToString());
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

                pn = BusinessFactory.Td_Fk_1.GetPage(param);
                var plist = new PageList<Td_Fk_1_QueryModel>(pn, pageIndex, limit);

                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"]; //排序规则需要返回前台  

                //用户管理门店
                ViewData["userShopList"] = GetShop(Enums.ShopDataType.UserShop); //GetUserShop();
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

        #region 进货付款-新增
        /// <summary>
        /// 进货付款-新增 
        /// lz
        /// 2016-09-20
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Add()
        {
            ViewData["option"] = "add";
            //制单门店
            ViewData["userShopList"] = GetCurrentUserMgrShop(id_user, id_shop);//GetShop(Enums.ShopDataType.UserShop); //GetUserShop();
            //用户管理供应商
            ViewData["userGysList"] = GetUserGys();
            //获取前台控制小数点
            ViewBag.DigitHashtable = GetParm();
            //经办人
            ViewData["userList"] = GetUser();

            Hashtable param = base.GetParameters();
            PageNavigate pn = new PageNavigate();
            int limit = base.PageSizeFromCookie;
            ParamVessel pv = new ParamVessel();
            pv.Add("id_gys", string.Empty, HandleType.DefaultValue);
            pv.Add("gys_name", string.Empty, HandleType.DefaultValue);
            pv.Add("page", 0, HandleType.DefaultValue);
            pv.Add("pageSize", limit, HandleType.DefaultValue);
            pv.Add("sort", "rq desc", HandleType.DefaultValue);
            pv.Add("_search_", "0", HandleType.DefaultValue);
            pv.Add("id", "", HandleType.Remove);
            pv.Add("type", string.Empty, HandleType.Remove);//type
            pv.Add("id_shop", "", HandleType.Remove);//id_shop
            param = param.Trim(pv);
            param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            param.Add("flag_cancel", (int)Enums.FlagCancel.NoCancel);
            param.Add("flag_sh", (int)Enums.FlagSh.HadSh);
            int pageIndex = Convert.ToInt32(param["page"]);
            param.Add("limit", limit);
            param.Add("start", pageIndex * limit);


            if (param.ContainsKey("type"))
                ViewData["type"] = param["type"].ToString();

            if (param.ContainsKey("id_shop") &&  string.IsNullOrEmpty(param["id_shop"].ToString()))
                param.Remove("id_shop");

            if (param.ContainsKey("id_gys") && !string.IsNullOrEmpty(param["id_gys"].ToString()))
            {
                param.Add("je_wf_more_zero", "0");
                pn = BusinessFactory.Tz_Yf_Jsc_Gx.GetPage(param);
            }
            else if (param.ContainsKey("id") && !string.IsNullOrEmpty(param["id"].ToString()))
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", param["id"].ToString());
                var br = BusinessFactory.Td_Fk_1.Get(ht);
                if (br.Success)
                {
                    ViewBag.CopyInfo = br.Data;
                }
            }

            var plist = new PageList<Tz_Yf_Jsc_Gx_QueryModel>(pn, pageIndex, limit);
            plist.PageIndex = pageIndex;
            plist.PageSize = limit;
            ViewData["List"] = plist;
            ViewData["sort"] = param["sort"]; //排序规则需要返回前台  
            ViewData["id_gys"] = param["id_gys"].ToString();
            ViewData["gys_name"] = param["gys_name"].ToString();

            ViewData["zdr_name"] = ViewData["shr_name"] = GetLoginInfo<string>("name");
            ViewData["id_user"] = id_user;
            if (param["_search_"].ToString().Equals("1"))
                return PartialView("_Add");
            else
                return View();

        }
        #endregion

        #region 进货付款-Post新增
        /// <summary>
        /// 进货付款-Post新增
        /// lz
        /// 2016-09-20
        /// </summary>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Add(Td_Fk_1 model)
        {
            BaseResult br = new BaseResult();

            var oldParam = new Hashtable();

            
            try
            {
                #region 获取参数
                Hashtable param = base.GetParameters();

                ParamVessel p = new ParamVessel();
                p.Add("jhList", string.Empty, HandleType.ReturnMsg);//jhList
                p.Add("remark", string.Empty, HandleType.DefaultValue);//备注
                p.Add("id_gys", string.Empty, HandleType.ReturnMsg);//id_gys
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_jbr", string.Empty, HandleType.ReturnMsg);//id_jbr
                p.Add("dh", string.Empty, HandleType.ReturnMsg);//dh
                p.Add("rq", string.Empty, HandleType.ReturnMsg);//rq

                p.Add("type", string.Empty, HandleType.Remove);//type
                p.Add("id", string.Empty, HandleType.Remove);//id

                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                oldParam = (Hashtable)param.Clone();
                #endregion

                List<Td_Fk_2> jhList = JSON.Deserialize<List<Td_Fk_2>>(param["jhList"].ToString()) ?? new List<Td_Fk_2>();

                #region 验证数据
                if (jhList == null || jhList.Count() <= 0)
                {
                    br.Success = false;
                    br.Message.Add("付款体不能为空!");
                    WriteDBLog("进货付款-Post新增/编辑", oldParam, br);
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }



                foreach (var item in jhList)
                {
                    #region 付款体参数验证
                    if (item.je_origin == null
                                    || item.je_wf == null
                                    || item.je_yf == null
                                     || item.je_fk == null
                                    )
                    {
                        br.Success = false;
                        br.Message.Add("必要金额不能为空 请重新刷新页面!");
                        WriteDBLog("进货付款-Post新增/编辑", oldParam, br);
                        return base.JsonString(new
                        {
                            status = "error",
                            message = string.Join(";", br.Message)
                        });
                    }

                    

                    if (item.je_wf + item.je_yf != item.je_origin)
                    {
                        br.Success = false;
                        br.Message.Add("已付金额 + 未付金额不等于原单总金额 请重新刷新页面!");
                        WriteDBLog("进货付款-Post新增/编辑", oldParam, br);
                        return base.JsonString(new
                        {
                            status = "error",
                            message = string.Join(";", br.Message)
                        });
                    }

                    if (item.je_fk + item.je_yh > item.je_wf)
                    {
                        br.Success = false;
                        br.Message.Add("优惠金额 + 本次付款金额不能超过未付款金额!");
                        WriteDBLog("进货付款-Post新增/编辑", oldParam, br);
                        return base.JsonString(new
                        {
                            status = "error",
                            message = string.Join(";", br.Message)
                        });
                    }
                    #endregion
                }

                var digitHashtable = GetParm();

                param.Remove("jhList");
                param.Add("jhList", jhList);
                param.Add("DigitHashtable", digitHashtable);
                param.Add("autoAudit", AutoAudit());
                #endregion

                if (param.ContainsKey("type") && param["type"].ToString() == "edit")
                {
                    //插入表
                    br = BusinessFactory.Td_Fk_1.Update(param);
                    WriteDBLog("进货付款-Post编辑", oldParam, br);
                }
                else
                {
                    //插入表
                    br = BusinessFactory.Td_Fk_1.Add(param);
                    WriteDBLog("进货付款-Post新增", oldParam, br);
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
            catch (Exception ex)
            {
                br.Success = false;
                br.Data = "";
                br.Message.Add("数据不符合要求_e!");
                br.Level = ErrorLevel.Warning;
                WriteDBLog("进货付款-Post新增/编辑", oldParam, br);
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }
        }

        #endregion

        #region 进货付款-详细
        /// <summary>
        /// 进货付款-详细
        /// lz
        /// 2016-09-20
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Detail()
        {
            ViewData["option"] = "add";
            //用户管理门店
            ViewData["userShopList"] = GetCurrentUserMgrShop(id_user, id_shop);//GetShop(Enums.ShopDataType.UserShop); //GetUserShop();
            //用户管理供应商
            ViewData["userGysList"] = GetUserGys();
            //获取前台控制小数点
            ViewBag.DigitHashtable = GetParm();
            //经办人
            ViewData["userList"] = GetUser();

            Hashtable param = base.GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("id", string.Empty, HandleType.Remove);//是否需要复制
            param = param.Trim(p);
            if (param.ContainsKey("id") && !string.IsNullOrEmpty(param["id"].ToString()))
            {
                var br = BusinessFactory.Td_Jh_1.Get(param);
                if (br.Success)
                {
                    ViewBag.CopyInfo = br.Data;
                }
            }
            return View();
        }
        #endregion

        #region 进货付款-作废
        /// <summary>
        ///进货付款-作废
        /// lz 2016-09-20
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
            pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            try
            {
                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                br = BusinessFactory.Td_Fk_1.Stop(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("进货付款-作废", oldParam, br);

            return JsonString(br, 1);
        }
        #endregion

        #region 进货付款-删除
        /// <summary>
        ///进货付款-删除
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
                br = BusinessFactory.Td_Fk_1.Delete(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("进货付款-删除", oldParam, br);
            return JsonString(br, 1);
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
                br.Data = GetNewDH(Enums.FlagDJLX.DHJHFK);
                return JsonString(br);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 进货付款-审核
        /// <summary>
        ///进货付款-审核
        /// lz 2016-10-21
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult Sh()
        {
            BaseResult br = new BaseResult();
            var oldParam = new Hashtable();
            Hashtable param = base.GetParameters();
            ParamVessel pv = new ParamVessel();
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            try
            {
                param = param.Trim(pv);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                oldParam = (Hashtable)param.Clone();
                br = BusinessFactory.Td_Fk_1.Active(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("进货付款-审核", oldParam, br);
            return JsonString(br, 1);
        }
        #endregion



    }
}
