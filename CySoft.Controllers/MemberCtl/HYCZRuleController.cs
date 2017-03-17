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
using CySoft.Model.Ts;

//会员充值规则
namespace CySoft.Controllers.MemberCtl
{
    [LoginActionFilter]
    public class HYCZRuleController : BaseController
    {
        #region 会员充值规则-总页面
        /// <summary>
        /// 会员充值规则-总页面
        /// lz
        /// 2016-11-14
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult CZRuleSet()
        {
            SetViewData();
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            param.Add("query_body", "1");
            param.Add("digit", GetParm());
            ViewData["db_data"] = BusinessFactory.Tb_Hy_Czrule.GetAll(param).Data;

            #region 获取会员积分 多倍积分设置
            var dbjf = new Ts_HykDbjf();
            if (id_shop_master != id_shop)
                dbjf = this.GetCZModel(id_shop);
            else
                dbjf = this.GetCZModel("0");
            ViewData["db_dbjf_data"] = dbjf;
            #endregion

            return View();
        }

        #endregion

        #region 会员充值规则-新增/编辑页面
        /// <summary>
        /// 会员充值规则-新增页面
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Add()
        {
            SetViewData();
            Hashtable param = base.GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("id", string.Empty, HandleType.Remove);//id
            p.Add("type", "add", HandleType.DefaultValue);//type
            param = param.Trim(p);
            param.Add("id_masteruser", id_user_master);

            ViewData["type"] = param["type"].ToString();

            if (param.ContainsKey("id") && !string.IsNullOrEmpty(param["id"].ToString()))
            {
                ViewData["db_data"] = BusinessFactory.Tb_Hy_Czrule.Get(param).Data;
            }

            return View();
        }
        #endregion

        #region 会员充值规则-新增/编辑页面Post
        /// <summary>
        /// 会员充值规则-新增/编辑页面Post
        /// lz
        /// 2016-11-14
        /// </summary>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult CZRuleSet(Model.Tb.Tb_Hy_Jfrule model)
        {
            BaseResult br = new BaseResult();
            var oldParam = new Hashtable();


            try
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("table_info", string.Empty, HandleType.ReturnMsg);//table_info
                p.Add("type", string.Empty, HandleType.Remove);//type
                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                param.Add("id_shop", id_shop);
                oldParam = (Hashtable)param.Clone();
                #endregion
                #region 参数验证

                if (string.IsNullOrEmpty(param["table_info"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add("参数不符合要求!");
                    WriteDBLog("会员充值规则-新增/编辑页面Post", oldParam, br);
                    return base.JsonString(br, 1);
                }

                var list = JSON.Deserialize<List<Td_Hy_Czrule_1_ReqModel>>(param["table_info"].ToString()) ?? new List<Td_Hy_Czrule_1_ReqModel>();

                if (list == null || list.Count() <= 0)
                {
                    br.Success = false;
                    br.Message.Add("请先添加需要新增的充值规则!");
                    WriteDBLog("会员充值规则-新增/编辑页面Post", oldParam, br);
                    return base.JsonString(br, 1);
                }

                foreach (var item in list)
                {
                    var tempList = JSON.Deserialize<List<Td_Hy_Czrule_2>>(item.sp_list) ?? new List<Td_Hy_Czrule_2>();
                    item.czrule_2_list = tempList;

                    if (item.je_cz <= 0)
                    {
                        br.Success = false;
                        br.Message.Add("充值金额必须大于0!");
                        WriteDBLog("会员充值规则-新增/编辑页面Post", oldParam, br);
                        return base.JsonString(br, 1);
                    }

                    if (item.je_cz_zs < 0)
                    {
                        br.Success = false;
                        br.Message.Add("赠送金额不允许小于0!");
                        WriteDBLog("会员充值规则-新增/编辑页面Post", oldParam, br);
                        return base.JsonString(br, 1);
                    }

                    if (item.je_cz_zs == 0 && item.czrule_2_list.Count() <= 0)
                    {
                        br.Success = false;
                        br.Message.Add("赠送金额为0时 必须选择赠送商品!");
                        WriteDBLog("会员充值规则-新增/编辑页面Post", oldParam, br);
                        return base.JsonString(br, 1);
                    }

                }

                param.Add("list", list);

                #endregion


                if (param.ContainsKey("type") && param["type"].ToString() == "edit")
                {
                    #region 更新
                    br = BusinessFactory.Td_Hy_Czrule_1.Update(param);
                    #endregion
                }
                else
                {
                    #region 新增
                    br = BusinessFactory.Td_Hy_Czrule_1.Add(param);
                    #endregion
                }

                #region 返回
                WriteDBLog("会员充值规则-新增/编辑页面Post", oldParam, br);

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
                WriteDBLog("会员充值规则-新增/编辑页面Post", oldParam, br);
                return base.JsonString(br, 1);
                #endregion
            }
        }

        #endregion

        #region 会员充值规则--获取商品
        /// <summary>
        /// 会员充值规则--获取商品
        /// lz
        /// 2016-11-16
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult GetCZRule_ZSSP()
        {
            BaseResult br = new BaseResult();
            try
            {
                //Tb_Hy_Czrule_Zssp
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_bill", string.Empty, HandleType.ReturnMsg);//id_bill
                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);

                br = BusinessFactory.Tb_Hy_Czrule_Zssp.GetAll(param);

                if (br.Success)
                {
                    return base.JsonString(new
                    {
                        status = "success",
                        message = "执行成功",
                        data = br.Data
                    });
                }
                else
                {
                    return base.JsonString(new
                    {
                        status = "false",
                        message = "执行成功",
                        data = br.Data
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

        #region 会员充值规则-作废
        /// <summary>
        /// 会员充值规则-作废
        /// lz
        /// 2016-11-16
        /// </summary>
        [HttpPost]
        public ActionResult Stop()
        {
            BaseResult br = new BaseResult();

            var oldParam = new Hashtable();

            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("ids", string.Empty, HandleType.ReturnMsg);//ids
                param = param.Trim(p);
                param.Add("id_user", id_user);
                param.Add("id_masteruser", id_user_master);
                oldParam = (Hashtable)param.Clone();
                br = BusinessFactory.Td_Hy_Czrule_1.Stop(param);
                WriteDBLog("会员充值规则-作废", oldParam, br);
                if (br.Success)
                {
                    return base.JsonString(new
                    {
                        status = "success",
                        message = "执行成功..."
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
                WriteDBLog("会员充值规则-作废", oldParam, br);
                throw ex;
            }
            catch (Exception ex)
            {
                WriteDBLog("会员充值规则-作废", oldParam, br);
                throw ex;
            }
        }
        #endregion

        #region 会员充值规则-获取详细
        /// <summary>
        /// 会员充值规则-获取详细
        /// lz
        /// 2016-11-16
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult GetCZRule_Detail()
        {
            BaseResult br = new BaseResult();

            var oldParam = new Hashtable();


            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_bill", string.Empty, HandleType.ReturnMsg);//id_bill
                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);
                oldParam = (Hashtable)param.Clone();
                br = BusinessFactory.Tb_Hy_Czrule.Get(param);

                if (br.Success)
                {
                    WriteDBLog("会员充值规则-获取详细", oldParam, br);

                    return base.JsonString(new
                    {
                        status = "success",
                        message = "执行成功,正在载入页面...",
                        data = br.Data
                    });
                }
                else
                {
                    WriteDBLog("会员充值规则-获取详细", oldParam, br);

                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
            }
            catch (CySoftException ex)
            {
                WriteDBLog("会员充值规则-获取详细", oldParam, br);
                throw ex;
            }
            catch (Exception ex)
            {
                WriteDBLog("会员充值规则-获取详细", oldParam, br);
                throw ex;
            }
        }
        #endregion

        #region 会员充值 充值设置金额参数
        /// <summary>
        /// 会员充值 充值设置金额参数
        /// lz
        /// 2016-11-11
        /// </summary>
        [HttpPost]
        public ActionResult SetJeMinMax()
        {
            BaseResult br = new BaseResult();

            var oldParam = new Hashtable();


            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("je_obj", string.Empty, HandleType.ReturnMsg);//je_obj
                param = param.Trim(p);
                param.Add("id_user", id_user);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_shop", id_shop);
                oldParam = (Hashtable)param.Clone();
                var je_obj = JSON.Deserialize<Ts_HykDbjf>(param["je_obj"].ToString()) ?? new Ts_HykDbjf();
                je_obj.id_masteruser = id_user_master;
                param.Remove("je_obj");
                param.Add("je_obj", je_obj);

                #region 验证参数

                if (!string.IsNullOrEmpty(je_obj.hy_czje_min_onec) && !CyVerify.IsNumeric(je_obj.hy_czje_min_onec))
                {
                    return base.JsonString(new { status = "error", message = string.Join(";", "每次最小金额格式错误") });
                }

                if (!string.IsNullOrEmpty(je_obj.hy_czje_max_onec) && !CyVerify.IsNumeric(je_obj.hy_czje_max_onec))
                {
                    return base.JsonString(new { status = "error", message = string.Join(";", "每次最大金额格式错误") });
                }

                if (!string.IsNullOrEmpty(je_obj.hy_czje_max_month) && !CyVerify.IsNumeric(je_obj.hy_czje_max_month))
                {
                    return base.JsonString(new { status = "error", message = string.Join(";", "每月最大金额格式错误") });
                }


                if (!string.IsNullOrEmpty(je_obj.hy_czje_min_onec) && !string.IsNullOrEmpty(je_obj.hy_czje_max_onec))
                {
                    if (decimal.Parse(je_obj.hy_czje_min_onec) > decimal.Parse(je_obj.hy_czje_max_onec))
                    {
                        return base.JsonString(new { status = "error", message = string.Join(";", "每次最小金额 不能大于 每次最大金额！") });
                    }
                }

                if (!string.IsNullOrEmpty(je_obj.hy_czje_min_onec) && !string.IsNullOrEmpty(je_obj.hy_czje_max_month))
                {
                    if (decimal.Parse(je_obj.hy_czje_min_onec) > decimal.Parse(je_obj.hy_czje_max_month))
                    {
                        return base.JsonString(new { status = "error", message = string.Join(";", "每次最小金额 不能大于 每月最大金额！") });
                    }
                }

                if (!string.IsNullOrEmpty(je_obj.hy_czje_max_onec) && !string.IsNullOrEmpty(je_obj.hy_czje_max_month))
                {
                    if (decimal.Parse(je_obj.hy_czje_max_onec) > decimal.Parse(je_obj.hy_czje_max_month))
                    {
                        return base.JsonString(new { status = "error", message = string.Join(";", "每次最大金额 不能大于 每月最大金额！") });
                    }
                }


                #endregion

                br = BusinessFactory.Td_Hy_Czrule_1.Active(param);

                if (br.Success)
                {
                    WriteDBLog("会员充值-充值设置金额参数", oldParam, br);
                    base.ClearShopParm(je_obj.id_masteruser, je_obj.id_shop);
                    return base.JsonString(new
                    {
                        status = "success",
                        message = "执行成功,正在载入页面..."
                    });
                }
                else
                {
                    WriteDBLog("会员充值-充值设置金额参数", oldParam, br);
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
            }
            catch (CySoftException ex)
            {
                WriteDBLog("会员充值-充值设置金额参数", oldParam, br);
                throw ex;
            }
            catch (Exception ex)
            {
                WriteDBLog("会员充值-充值设置金额参数", oldParam, br);
                throw ex;
            }
        }
        #endregion

        #region 会员充值--按门店查询
        /// <summary>
        /// 会员充值--按门店查询
        /// lz
        /// 2016-11-14
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult GetJeMinMax()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//门店
                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);

                var model = this.GetCZModel(param["id_shop"].ToString());

                return base.JsonString(new
                {
                    status = "success",
                    message = "执行成功",
                    data = model
                });

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

        #region SetViewData
        /// <summary>
        /// 设置通用页面data
        /// </summary>
        private void SetViewData()
        {
            //用户管理门店
            ViewData["shopList"] = GetShop(Enums.ShopDataType.ShopShop);
            ViewData["version"] = version;
            ViewData["hyfls"] = GetHyfl();
            ViewData["id_shop"] = id_shop;
            ViewData["is_master"] = (id_user_master == id_user ? true : false);

            var digitlxBr = base.GetFlagList(Enums.TsFlagListCode.digitlx.ToString());
            if (digitlxBr.Success)
                ViewData["digitlx"] = digitlxBr.Data;

            //获取前台控制小数点
            ViewBag.DigitHashtable = GetParm();
        }
        #endregion

        #region GetDBJFModel
        public Ts_HykDbjf GetCZModel(string id_shop_select)
        {
            try
            {
                Ts_HykDbjf model = new Ts_HykDbjf() { id_masteruser = id_user_master, id_shop = id_shop_select };
                var shopParm = BusinessFactory.Tb_Hy_Jfrule.GetShopParm(model.id_masteruser, model.id_shop);
                if (shopParm != null && shopParm.ContainsKey("success") && shopParm["success"].ToString() == "1")
                {
                    model.hy_czje_min_onec = shopParm["hy_czje_min_onec"].ToString();
                    model.hy_czje_max_onec = shopParm["hy_czje_max_onec"].ToString();
                    model.hy_czje_max_month = shopParm["hy_czje_max_month"].ToString();
                }
                else
                {
                    shopParm = BusinessFactory.Tb_Hy_Jfrule.GetShopParm(model.id_masteruser, "0");
                    if (shopParm != null && shopParm.ContainsKey("success") && shopParm["success"].ToString() == "1")
                    {
                        model.hy_czje_min_onec = shopParm["hy_czje_min_onec"].ToString();
                        model.hy_czje_max_onec = shopParm["hy_czje_max_onec"].ToString();
                        model.hy_czje_max_month = shopParm["hy_czje_max_month"].ToString();
                    }
                }
                return model;
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

    }
}
