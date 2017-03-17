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

//会员积分规则
namespace CySoft.Controllers.MemberCtl
{
    [LoginActionFilter]
    public class HYJFRuleController : BaseController
    {
        #region 会员积分规则
        /// <summary>
        /// 会员积分规则
        /// lz
        /// 2016-11-10
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult JFRuleSet()
        {
            SetViewData();

            #region 获取会员积分规则设置
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            ViewData["db_data"] = BusinessFactory.Tb_Hy_Jfrule.GetAll(param).Data;
            #endregion

            #region 获取会员积分兑换商品设置
            param.Clear();
            param.Add("id_masteruser", id_user_master);
            ViewData["db_dbjf_dhsp_data"] = BusinessFactory.Tb_Hy_Jfconvertsp.GetAll(param).Data;
            #endregion

            #region 获取会员积分 多倍积分设置
            var dbjf = new Ts_HykDbjf();
            if(id_shop_master!=id_shop)
                dbjf=this.GetDBJFModel(id_shop);
            else
                dbjf = this.GetDBJFModel("0");
            ViewData["db_dbjf_data"] = dbjf;
            #endregion

            return View();
        }

        /// <summary>
        /// 会员积分规则
        /// lz
        /// 2016-11-10
        /// </summary>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult JFRuleSet(Model.Tb.Tb_Hy_Jfrule model)
        {
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            try
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("tab", string.Empty, HandleType.ReturnMsg);//tab
                p.Add("table_info", string.Empty, HandleType.ReturnMsg);//table_info
                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                oldParam = (Hashtable)param.Clone();
                #endregion
                #region 参数验证

                if (string.IsNullOrEmpty(param["tab"].ToString()) || string.IsNullOrEmpty(param["table_info"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add("参数不符合要求!");
                    WriteDBLog("会员积分规则-设置", oldParam, br);
                    return base.JsonString(br, 1);
                }

                var list = JSON.Deserialize<List<Model.Tb.Tb_Hy_Jfrule>>(param["table_info"].ToString()) ?? new List<Model.Tb.Tb_Hy_Jfrule>();
                foreach (var item in list)
                {
                    if (item.jf <= 0)
                    {
                        br.Success = false;
                        br.Message.Add("积分不允许小于等于0!");
                        WriteDBLog("会员积分规则-设置", oldParam, br);
                        return base.JsonString(br, 1);
                    }
                }
                param.Add("list", list);

                #endregion

                #region 新增
                br = BusinessFactory.Tb_Hy_Jfrule.Add(param);
                #endregion
                #region 返回
                WriteDBLog("会员积分规则-设置", oldParam, br);
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
                WriteDBLog("会员积分规则-设置", oldParam, br);
                return base.JsonString(br, 1);
                #endregion
            }
        }
        #endregion

        #region 会员积分规则-删除
        /// <summary>
        /// 会员积分规则-删除
        /// lz
        /// 2016-11-10
        /// </summary>
        [HttpPost]
        public ActionResult Delete()
        {
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("ids", string.Empty, HandleType.ReturnMsg);//会员积分规则
                param = param.Trim(p);
                param.Add("id_user", id_user);
                param.Add("id_masteruser", id_user_master);
                oldParam = (Hashtable)param.Clone();
                br = BusinessFactory.Tb_Hy_Jfrule.Delete(param);
                WriteDBLog("会员积分规则-删除", oldParam, br);
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

        #region 会员多倍积分
        /// <summary>
        /// 会员多倍积分
        /// lz
        /// 2016-11-11
        /// </summary>
        [HttpPost]
        public ActionResult SetDBJF()
        {
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dbjf", string.Empty, HandleType.ReturnMsg);//会员多倍积分
                param = param.Trim(p);
                param.Add("id_user", id_user);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_shop", id_shop);
                oldParam = (Hashtable)param.Clone();
                var dbjf = JSON.Deserialize<Ts_HykDbjf>(param["dbjf"].ToString()) ?? new Ts_HykDbjf();
                dbjf.id_masteruser = id_user_master;
                param.Remove("dbjf");
                param.Add("dbjf", dbjf);

                #region 验证参数

                if (!string.IsNullOrEmpty(dbjf.hy_jfsz_hysr_nbjf))
                {
                    if (string.IsNullOrEmpty(dbjf.hy_jfsz_hysr_lx))
                    {
                        return base.JsonString(new { status = "error", message = string.Join(";", "请选择会员生日期间类型") });
                    }
                    
                }

                if (!string.IsNullOrEmpty(dbjf.hy_jfsz_week_nbjf))
                {
                    if (string.IsNullOrEmpty(dbjf.hy_jfsz_week_val))
                    {
                        return base.JsonString(new { status = "error", message = string.Join(";", "请选择星期积分倍数的星期") });
                    }
                }

                if (!string.IsNullOrEmpty(dbjf.hy_jfsz_day_nbjf))
                {
                    if (string.IsNullOrEmpty(dbjf.hy_jfsz_day_val))
                    {
                        return base.JsonString(new { status = "error", message = string.Join(";", "请选择日期积分倍数的日期") });
                    }
                }


                if (!string.IsNullOrEmpty(dbjf.hy_jfsz_xs_nbjf))
                {
                    if (string.IsNullOrEmpty(dbjf.hy_jfsz_xs_rq_b))
                    {
                        return base.JsonString(new { status = "error", message = string.Join(";", "请选择金额积分倍数的开始时间") });
                    }
                    else if (string.IsNullOrEmpty(dbjf.hy_jfsz_xs_rq_e))
                    {
                        return base.JsonString(new { status = "error", message = string.Join(";", "请选择金额积分倍数的结束时间") });

                    }
                    else if (string.IsNullOrEmpty(dbjf.hy_jfsz_xs_je) || !CyVerify.IsNumeric(dbjf.hy_jfsz_xs_je))
                    {
                        return base.JsonString(new { status = "error", message = string.Join(";", "请选择金额积分倍数的消费满金额") });

                    }
                }

                #endregion

                br = BusinessFactory.Tb_Hy_Jfrule.Active(param);
                WriteDBLog("会员多倍积分-设置", oldParam, br);
                if (br.Success)
                {
                    base.ClearShopParm(dbjf.id_masteruser, dbjf.id_shop);
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

        #region 会员多倍积分--按门店查询
        /// <summary>
        /// 会员多倍积分--按门店查询
        /// lz
        /// 2016-11-14
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult GetDBJF()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//门店
                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);

                var model = this.GetDBJFModel(param["id_shop"].ToString());

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
        public Ts_HykDbjf GetDBJFModel(string id_shop_select)
        {
            try
            {
                Ts_HykDbjf model = new Ts_HykDbjf() { id_masteruser = id_user_master, id_shop = id_shop_select };
                var shopParm = BusinessFactory.Tb_Hy_Jfrule.GetShopParm(model.id_masteruser, model.id_shop);
                if (shopParm != null && shopParm.ContainsKey("success") && shopParm["success"].ToString() == "1")
                {
                    model.hy_jfsz_week_val = shopParm["hy_jfsz_week_val"].ToString();
                    
                    model.hy_jfsz_xs_rq_b = shopParm["hy_jfsz_xs_rq_b"].ToString();
                    model.hy_jfsz_xs_rq_e = shopParm["hy_jfsz_xs_rq_e"].ToString();
                    model.hy_jfsz_day_nbjf = shopParm["hy_jfsz_day_nbjf"].ToString();
                    model.hy_jfsz_xs_je = shopParm["hy_jfsz_xs_je"].ToString();
                    model.hy_jfsz_day_val = shopParm["hy_jfsz_day_val"].ToString();
                    model.hy_jfsz_xs_nbjf = shopParm["hy_jfsz_xs_nbjf"].ToString();
                    model.hy_jfsz_hysr_lx = shopParm["hy_jfsz_hysr_lx"].ToString();
                    model.hy_jfsz_hysr_nbjf = shopParm["hy_jfsz_hysr_nbjf"].ToString();
                    model.hy_jfsz_week_nbjf = shopParm["hy_jfsz_week_nbjf"].ToString();
                }
                else
                {
                    shopParm = BusinessFactory.Tb_Hy_Jfrule.GetShopParm(model.id_masteruser, "0");
                    if (shopParm != null && shopParm.ContainsKey("success") && shopParm["success"].ToString() == "1")
                    {
                        model.hy_jfsz_week_val = shopParm["hy_jfsz_week_val"].ToString();
                        
                        model.hy_jfsz_xs_rq_b = shopParm["hy_jfsz_xs_rq_b"].ToString();
                        model.hy_jfsz_xs_rq_e = shopParm["hy_jfsz_xs_rq_e"].ToString();
                        model.hy_jfsz_day_nbjf = shopParm["hy_jfsz_day_nbjf"].ToString();
                        model.hy_jfsz_xs_je = shopParm["hy_jfsz_xs_je"].ToString();
                        model.hy_jfsz_day_val = shopParm["hy_jfsz_day_val"].ToString();
                        model.hy_jfsz_xs_nbjf = shopParm["hy_jfsz_xs_nbjf"].ToString();
                        model.hy_jfsz_hysr_lx = shopParm["hy_jfsz_hysr_lx"].ToString();
                        model.hy_jfsz_hysr_nbjf = shopParm["hy_jfsz_hysr_nbjf"].ToString();
                        model.hy_jfsz_week_nbjf = shopParm["hy_jfsz_week_nbjf"].ToString();
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

        #region 会员积分兑换商品-删除
        /// <summary>
        /// 会员积分兑换商品-删除
        /// lz
        /// 2016-11-21
        /// </summary>
        [HttpPost]
        public ActionResult DeleteDHSP()
        {
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("ids", string.Empty, HandleType.ReturnMsg);//会员积分兑换商品
                param = param.Trim(p);
                param.Add("id_user", id_user);
                param.Add("id_masteruser", id_user_master);
                oldParam = (Hashtable)param.Clone();
                br = BusinessFactory.Tb_Hy_Jfconvertsp.Delete(param);
                WriteDBLog("会员积分兑换商品-删除", oldParam, br);

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

        #region 会员积分兑换商品-保存
        /// <summary>
        /// 会员积分兑换商品-保存
        /// lz
        /// 2016-11-21
        /// </summary>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult JFRuleSetDHSP(Model.Tb.Tb_Hy_Jfconvertsp model)
        {
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            try
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("tab", string.Empty, HandleType.ReturnMsg);//tab
                p.Add("table_info", string.Empty, HandleType.ReturnMsg);//table_info
                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                oldParam = (Hashtable)param.Clone();
                #endregion
                #region 参数验证

                if (string.IsNullOrEmpty(param["tab"].ToString()) || string.IsNullOrEmpty(param["table_info"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add("参数不符合要求!");
                    WriteDBLog("会员积分兑换商品-保存", oldParam, br);
                    return base.JsonString(br, 1);
                }

                var list = JSON.Deserialize<List<Model.Tb.Tb_Hy_Jfconvertsp>>(param["table_info"].ToString()) ?? new List<Model.Tb.Tb_Hy_Jfconvertsp>();

                if (list.Where(d => d.jf == 0).Count() > 0)
                {
                    br.Success = false;
                    br.Message.Add("积分不允许小于等于0!");
                    WriteDBLog("会员积分兑换商品-保存", oldParam, br);
                    return base.JsonString(br, 1);
                }

                param.Add("list", list);

                #endregion

                #region 新增
                br = BusinessFactory.Tb_Hy_Jfconvertsp.Add(param);
                #endregion
                #region 返回
                WriteDBLog("会员积分兑换商品-保存", oldParam, br);
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
                WriteDBLog("会员积分兑换商品-保存", oldParam, br);
                return base.JsonString(br, 1);
                #endregion
            }
        } 
        #endregion


    }
}
