using Aop.Api.Util;
using CYSDK.Utils;
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.IBLL.Base;
using CySoft.Model.CySoft.Model.Td;
using CySoft.Model.Tb;
using CySoft.Model.Ts;
using CySoft.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

//商业服务
namespace CySoft.BLL.PayBLL
{
    public class PayBLL : BaseBLL, IPayBLL
    {
        public IBaseBLL Tz_Hy_JeBLL { get; set; }

        #region OutOrderPay 订单支付
        /// <summary>
        /// 订单支付
        /// lz
        /// 2016-12-07
        /// </summary>
        [Transaction]
        public BaseResult OutOrderPay(Hashtable param)
        {
            #region 定义参数
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            string dh_ec = "";
            string ec_tradeno = "";
            decimal add_je = 0;
            decimal add_je_zs = 0;
            decimal je_qm = 0;
            decimal je_qm_zs = 0;
            #endregion

            #region 验证参数
            if (param == null ||
                 param["id_shop"] == null || string.IsNullOrEmpty(param["id_shop"].ToString()) ||
                  param["id_masteruser"] == null || string.IsNullOrEmpty(param["id_masteruser"].ToString()) ||
               param["json_param"] == null || string.IsNullOrEmpty(param["json_param"].ToString()) ||
               param["dh"] == null || string.IsNullOrEmpty(param["dh"].ToString()) ||
               param["payList"] == null ||
                param["sign"] == null || string.IsNullOrEmpty(param["sign"].ToString())
                )
            {
                br.Success = false;
                br.Message.Add("必要参数不可以为空.");
                return br;
            }

            var payList = (List<Money_OrderPayList>)param["payList"];

            if (payList.Count() != 1)
            {
                br.Success = false;
                br.Message.Add("支付参数不符合要求.");
                return br;
            }

            if (payList.Where(d => d.type != "wx" && d.type != "zfb" && d.type != "hy").Count() > 0)
            {
                br.Success = false;
                br.Message.Add("支付类型错误 目前只支持微信/支付宝/储值卡支付.");
                return br;
            }

            if (payList.Sum(d => d.money) <= 0)
            {
                br.Success = false;
                br.Message.Add("金额出现错误.");
                return br;
            }

            #endregion

            #region 获取支付配置信息
            int i = 1;
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("id_shop", param["id_shop"].ToString());
            var payConfigList = DAL.QueryList<Tb_Pay_Config>(typeof(Tb_Pay_Config), ht);
            #endregion

            #region 调用接口处理
            //遍历执行支付
            foreach (var item in payList)
            {
                if (item.money == 0) continue;
                switch (item.type)
                {
                    case "hy"://会员支付
                        #region 会员支付
                        HY_OrderPayModel hyCostModel = new HY_OrderPayModel();
                        try
                        {
                            hyCostModel = Utility.JSON.Deserialize<HY_OrderPayModel>(item.data);

                            if (string.IsNullOrEmpty(hyCostModel.id_hy))
                            {
                                br.Success = false;
                                br.Message.Add(string.Format("会员支付数据 data 的 id_hy 不符合."));
                                return br;
                            }
                        }
                        catch (Exception ex)
                        {
                            br.Success = false;
                            br.Message.Add(string.Format("会员支付数据 data 不符合."));
                            br.Level = ErrorLevel.Alert;
                            throw new CySoftException(br);
                        }

                        ht.Clear();
                        ht.Add("id_shop", param["id_shop"].ToString());
                        ht.Add("id_masteruser", param["id_masteruser"].ToString());
                        ht.Add("id_hy", hyCostModel.id_hy);
                        ht.Add("id_bill", hyCostModel.id_bill);
                        ht.Add("bm_djlx", hyCostModel.bm_djlx);
                        ht.Add("rq", DateTime.Now);
                        ht.Add("je", item.money);
                        ht.Add("bz", hyCostModel.bz);
                        ht.Add("password", hyCostModel.password);
                        ht.Add("Type", "XF");
                        br = Tz_Hy_JeBLL.Active(ht);
                        if (!br.Success)
                        {
                            br.Level = ErrorLevel.Alert;
                            if (br.Message.Count() > 0 && br.Message[0].ToString().Contains("密码不正确"))
                                br.Level = ErrorLevel.Question;
                            throw new CySoftException(br);
                        }

                        decimal.TryParse(br.Data.GetType().GetProperty("add_je").GetValue(br.Data).ToString(), out add_je);
                        decimal.TryParse(br.Data.GetType().GetProperty("add_je_zs").GetValue(br.Data).ToString(), out add_je_zs);
                        decimal.TryParse(br.Data.GetType().GetProperty("je_qm").GetValue(br.Data).ToString(), out je_qm);
                        decimal.TryParse(br.Data.GetType().GetProperty("je_qm_zs").GetValue(br.Data).ToString(), out je_qm_zs);
                        #endregion
                        break;
                    case "zfb"://支付宝
                        #region 检查有没有设置支付类型
                        if (payConfigList == null || payConfigList.Where(d => d.flag_type == 4).Count() <= 0)
                        {
                            br.Success = false;
                            br.Message.Add(string.Format("没有设置支付宝支付类型,不能支付."));
                            return br;
                        }

                        if (payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_is_use" && d.parmvalue == "1").Count() <= 0)
                        {
                            br.Success = false;
                            br.Message.Add(string.Format("没有启用支付宝支付,不能支付."));
                            return br;
                        }
                        #endregion

                        #region 序列化Model
                        AliPayModel aliModel = new AliPayModel();
                        try
                        {
                            aliModel = Utility.JSON.Deserialize<AliPayModel>(item.data);
                        }
                        catch (Exception ex)
                        {
                            br.Success = false;
                            br.Message.Add(string.Format("支付宝支付数据 data 不符合."));
                            throw new CySoftException(br);
                        }
                        #endregion

                        #region 旧的支付----已注释
                        //#region 获取私钥和APP_ID信息

                        //if (payConfigList == null || payConfigList.Count() <= 0
                        //    || payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_rsakey1" && d.parmvalue != "").Count() <= 0
                        //    || payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_rsakey2" && d.parmvalue != "").Count() <= 0
                        //    || payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_rsakey3" && d.parmvalue != "").Count() <= 0
                        //    || payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_rsakey4" && d.parmvalue != "").Count() <= 0
                        //    )
                        //{
                        //    br.Success = false;
                        //    br.Message.Add(string.Format("设置的支付宝私钥错误,不能支付."));
                        //    return br;
                        //}

                        //if (payConfigList == null || payConfigList.Count() <= 0 || payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_partner" && d.parmvalue != "").Count() <= 0)
                        //{
                        //    br.Success = false;
                        //    br.Message.Add(string.Format("没有设置支付宝开发帐号,不能支付."));
                        //    return br;
                        //}


                        //string privateKey = payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_rsakey1").FirstOrDefault().parmvalue
                        //    + payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_rsakey2").FirstOrDefault().parmvalue
                        //    + payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_rsakey3").FirstOrDefault().parmvalue
                        //    + payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_rsakey4").FirstOrDefault().parmvalue;


                        //#endregion

                        //#region 调接口处理
                        ////开发者帐号
                        //aliModel.app_id = payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_partner").FirstOrDefault().parmvalue;
                        //aliModel.method = "alipay.trade.pay";
                        //aliModel.charset = "utf-8";
                        //aliModel.sign_type = "RSA";
                        //aliModel.timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //aliModel.version = "1.0";
                        //aliModel.sign = privateKey;

                        //aliModel.out_trade_no = param["dh"].ToString();
                        //aliModel.scene = "bar_code";

                        //aliModel.total_amount = (decimal)Frame.Common.OperationHelper.Digit(item.money, 2);//总金额
                        //aliModel.discountable_amount = 0;//优惠金额
                        //aliModel.undiscountable_amount = aliModel.total_amount;//不参与优惠计算的金额

                        //br = this.AliPay(aliModel);
                        //if (br.Success == false)
                        //{
                        //    throw new CySoftException(br);
                        //}
                        //AliPayResultModel aliPayResultModel = (AliPayResultModel)br.Data;
                        //dh_ec = aliModel.out_trade_no;
                        //ec_tradeno = aliPayResultModel.trade_no;
                        //br.Data = new { dh = param["dh"].ToString(), dh_ec = aliModel.out_trade_no, ec_tradeno = aliPayResultModel.trade_no };
                        //#endregion 
                        #endregion

                        #region 获取私钥和APP_ID信息

                        if (payConfigList == null || payConfigList.Count() <= 0 || payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_partner" && d.parmvalue != "").Count() <= 0)
                        {
                            br.Success = false;
                            br.Message.Add(string.Format("没有设置支付宝商户身份ID,不能支付."));
                            return br;
                        }

                        if (payConfigList == null || payConfigList.Count() <= 0 || payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_store_id" && d.parmvalue != "").Count() <= 0)
                        {
                            br.Success = false;
                            br.Message.Add(string.Format("没有设置支付宝商户门店编号,不能支付."));
                            return br;
                        }

                        if (string.IsNullOrEmpty(aliModel.auth_code))
                        {
                            br.Success = false;
                            br.Message.Add(string.Format("没有支付授权码,不能支付."));
                            return br;
                        }

                        if (string.IsNullOrEmpty(aliModel.subject))
                        {
                            br.Success = false;
                            br.Message.Add(string.Format("没有订单标题,不能支付."));
                            return br;
                        }

                        #endregion

                        #region 调接口处理
                        //开发者帐号
                        aliModel.m_code = payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_partner").FirstOrDefault().parmvalue;
                        aliModel.scene = "bar_code";
                        aliModel.total_amount = (decimal)Frame.Common.OperationHelper.Digit(item.money, 2);//总金额
                        aliModel.store_id = payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_store_id").FirstOrDefault().parmvalue;
                        aliModel.out_trade_no = param["dh"].ToString();

                        //terminal_id
                        //auth_code
                        //operator_id
                        //body
                        //subject

                        br = this.AliPayNew(aliModel);
                        if (br.Success == false)
                        {
                            throw new CySoftException(br);
                        }
                        AliPayResultModel aliPayResultModel = (AliPayResultModel)br.Data;
                        dh_ec = aliModel.out_trade_no;
                        ec_tradeno = aliPayResultModel.trade_no;
                        br.Data = new { dh = param["dh"].ToString(), dh_ec = aliModel.out_trade_no, ec_tradeno = aliPayResultModel.trade_no };
                        #endregion



                        break;
                    case "wx"://微信
                        #region 检查有没有设置支付类型
                        if (payConfigList == null || payConfigList.Where(d => d.flag_type == 5).Count() <= 0)
                        {
                            br.Success = false;
                            br.Message.Add(string.Format("没有设置微信支付类型,不能支付."));
                            return br;
                        }

                        if (payConfigList.Where(d => d.flag_type == 5 && d.parmcode == "pay_wx_is_use" && d.parmvalue == "1").Count() <= 0)
                        {
                            br.Success = false;
                            br.Message.Add(string.Format("没有启用微信支付,不能支付."));
                            return br;
                        }

                        #endregion

                        #region 序列化Model
                        WXPayModel wxModel = new WXPayModel();
                        try
                        {
                            wxModel = Utility.JSON.Deserialize<WXPayModel>(item.data);
                            wxModel.out_trade_no = param["dh"].ToString();
                            wxModel.total_fee = (int)((decimal)(item.money * 100));
                            wxModel.body = "Pos";
                        }
                        catch (Exception ex)
                        {
                            br.Success = false;
                            br.Message.Add(string.Format("微信类型支付数据不符合"));
                            throw new CySoftException(br);
                        }
                        #endregion

                        #region 获取子商户号信息
                        if (payConfigList == null || payConfigList.Count() <= 0 || payConfigList.Where(d => d.flag_type == 5 && d.parmcode == "pay_wx_mch_id_child" && d.parmvalue != "").Count() <= 0)
                        {
                            br.Success = false;
                            br.Message.Add(string.Format("没有设置微信支付子商户号,不能支付."));
                            return br;
                        }
                        #endregion

                        #region 调接口处理
                        wxModel.sub_mch_id = payConfigList.Where(d => d.flag_type == 5 && d.parmcode == "pay_wx_mch_id_child").FirstOrDefault().parmvalue;
                        wxModel.out_trade_no = param["dh"].ToString();

                        br = this.WXPay(wxModel);
                        if (br.Success == false)
                        {
                            throw new CySoftException(br);
                        }
                        WXPayResultModel wxPayResultModel = (WXPayResultModel)br.Data;

                        dh_ec = wxPayResultModel.out_trade_no;
                        ec_tradeno = wxPayResultModel.transaction_id;

                        br.Data = new { dh = param["dh"].ToString(), dh_ec = wxPayResultModel.out_trade_no, ec_tradeno = wxPayResultModel.transaction_id };
                        #endregion

                        break;
                    default:
                        br.Success = false;
                        br.Message.Add(string.Format("含有未知的支付类型 不予处理!"));
                        return br;
                }
                i++;
            }
            #endregion

            br.Success = true;
            br.Data = new { dh = param["dh"].ToString(), dh_ec = dh_ec, ec_tradeno = ec_tradeno, msg = "", add_je = add_je, add_je_zs = add_je_zs, je_qm = je_qm, je_qm_zs = je_qm_zs };
            return br;

        }
        #endregion





        #region 支付宝支付
        public BaseResult AliPay(AliPayModel aliModel)
        {
            BaseResult br = new BaseResult();
            try
            {
                #region 构建支付宝调用数据
                var paramters = new Dictionary<string, string>();
                paramters.Add("app_id", aliModel.app_id); //开发者帐号
                paramters.Add("method", aliModel.method); //接口名称
                paramters.Add("charset", aliModel.charset); //请求使用的编码格式，如utf - 8,gbk,gb2312等
                paramters.Add("sign_type", aliModel.sign_type);//商户生成签名字符串所使用的签名算法类型，目前支持RSA
                paramters.Add("timestamp", aliModel.timestamp);//发送请求的时间，格式"yyyy-MM-dd HH:mm:ss"
                paramters.Add("version", aliModel.version);//调用的接口版本，固定为：1.0

                if (string.IsNullOrEmpty(aliModel.store_id))
                    aliModel.store_id = "-999";

                string bizContent = "{" +
            "    \"out_trade_no\":\"" + aliModel.out_trade_no + "\"," +
            "    \"scene\":\"" + aliModel.scene + "\"," +
            "    \"auth_code\":\"" + aliModel.auth_code + "\"," +
            "    \"total_amount\":" + aliModel.total_amount + "," +
            "    \"subject\":\"" + aliModel.subject + "\"," +
            "    \"store_id\":\"" + aliModel.store_id + "\"" +
            " }";
                bizContent = bizContent.Trim();
                paramters.Add("biz_content", bizContent);

                string mySign = AlipaySignature.RSASign(paramters, aliModel.sign, "utf-8", false, "RSA");//生成签名 其中aliModel.sign为定义的私钥值
                paramters.Add("sign", mySign);

                #endregion
                #region 构建调用内部接口数据
                var dataContent = AlipaySignature.GetSignContent(paramters);
                paramters.Clear();
                //paramters.Add("data",   dataContent);
                var eData = HttpUtility.UrlEncode(dataContent, Encoding.UTF8);
                paramters.Add("data", dataContent);
                string myNewSign = CYUtils.SignRequestNew(paramters, PublicSign.localKey);
                paramters.Add("sign", myNewSign);
                paramters.Remove("data");
                paramters.Add("data", eData);
                paramters.Add("amount", aliModel.total_amount.ToString());

                #endregion

                var result = new WebUtils().DoPost(PublicSign.aliPayUrl, paramters, 200000);
                AliPayResultModel retPayModel = Utility.JSON.Deserialize<AliPayResultModel>(result);
                if (retPayModel.alipay_trade_pay_response.code == "10000" && retPayModel.alipay_trade_pay_response.msg == "Success")
                {
                    retPayModel.out_trade_no = retPayModel.alipay_trade_pay_response.out_trade_no;
                    retPayModel.trade_no = retPayModel.alipay_trade_pay_response.trade_no;
                    br.Success = true;
                    br.Data = retPayModel;
                    return br;
                }
                else
                {
                    if (retPayModel.alipay_trade_pay_response.sub_code == "ACQ.TRADE_HAS_SUCCESS")
                    {
                        br.Success = true;
                        br.Message.Add(string.Format("订单已被支付 描述:" + retPayModel.alipay_trade_pay_response.sub_msg));
                        retPayModel.out_trade_no = retPayModel.alipay_trade_pay_response.out_trade_no;
                        retPayModel.trade_no = retPayModel.alipay_trade_pay_response.trade_no;
                        br.Success = true;
                        br.Data = retPayModel;
                        return br;
                    }
                    else if (retPayModel.alipay_trade_pay_response.sub_code == "ACQ.SYSTEM_ERROR")
                    {
                        br.Success = false;
                        br.Message.Add(string.Format("支付宝异常接口返回错误 需要查询订单结果 描述:" + retPayModel.alipay_trade_pay_response.sub_msg));
                        br.Level = ErrorLevel.Warning;
                        return br;
                    }
                    else if (retPayModel.alipay_trade_pay_response.sub_code == "ACQ.USER_FACE_PAYMENT_SWITCH_OFF")
                    {
                        br.Success = false;
                        br.Message.Add(string.Format("用户当面付付款开关关闭 需要让用户在手机上打开当面付付款开关 描述:" + retPayModel.alipay_trade_pay_response.sub_msg));
                        br.Level = ErrorLevel.Warning;
                        return br;
                    }
                    else
                    {
                        br.Success = false;
                        br.Message.Add(string.Format("支付宝支付 操作返回失败! 描述:" + retPayModel.alipay_trade_pay_response.sub_msg));
                        return br;
                    }
                }
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Message.Add(string.Format("支付宝支付时发生异常 单号:" + aliModel.out_trade_no + " 请查询确定结果 以免带来损失!"));
                br.Level = ErrorLevel.Warning;
                return br;
            }
        }

        public BaseResult AliPayNew(AliPayModel aliModel)
        {
            BaseResult br = new BaseResult();
            try
            {
                #region 构建支付宝调用数据
                var paramters = new Dictionary<string, string>();
                paramters.Add("m_code", aliModel.m_code); //m_code
                paramters.Add("scene", aliModel.scene); //scene
                paramters.Add("terminal_id", aliModel.terminal_id); //terminal_id
                paramters.Add("total_amount", aliModel.total_amount.ToString());//total_amount
                paramters.Add("store_id", aliModel.store_id);//store_id
                paramters.Add("out_trade_no", aliModel.out_trade_no);//out_trade_no
                paramters.Add("auth_code", aliModel.auth_code);//auth_code
                paramters.Add("operator_id", aliModel.operator_id);//operator_id
                paramters.Add("body", aliModel.body);//body
                paramters.Add("subject", aliModel.subject);//subject
                string mySign = CYUtils.SignRequestNew(paramters, PublicSign.localKey);
                paramters.Add("sign", mySign);
                #endregion

                var urlNow = "请求报文:" + "\r\n" + PublicSign.aliPayUrl + "?" + WebUtils.BuildQuery2(paramters) + "\r\n\r\n";


                var result = new WebUtils().DoPost(PublicSign.aliPayUrl, paramters, 200000);
                AliPayResultModel retPayModel = Utility.JSON.Deserialize<AliPayResultModel>(result);
                if (retPayModel.alipay_trade_pay_response.code == "10000" && retPayModel.alipay_trade_pay_response.msg == "Success")
                {
                    retPayModel.out_trade_no = retPayModel.alipay_trade_pay_response.out_trade_no;
                    retPayModel.trade_no = retPayModel.alipay_trade_pay_response.trade_no;
                    br.Success = true;
                    br.Data = retPayModel;
                    return br;
                }
                else
                {
                    if (retPayModel.alipay_trade_pay_response.sub_code == "ACQ.TRADE_HAS_SUCCESS")
                    {
                        br.Success = true;
                        br.Message.Add(string.Format("订单已被支付 描述:" + retPayModel.alipay_trade_pay_response.sub_msg));
                        retPayModel.out_trade_no = retPayModel.alipay_trade_pay_response.out_trade_no;
                        retPayModel.trade_no = retPayModel.alipay_trade_pay_response.trade_no;
                        br.Success = true;
                        br.Data = retPayModel;
                        return br;
                    }
                    else if (retPayModel.alipay_trade_pay_response.sub_code == "ACQ.SYSTEM_ERROR")
                    {
                        br.Success = false;
                        br.Message.Add(string.Format("支付宝异常接口返回错误 需要查询订单结果 描述:" + retPayModel.alipay_trade_pay_response.sub_msg));
                        br.Level = ErrorLevel.Warning;
                        return br;
                    }
                    else if (retPayModel.alipay_trade_pay_response.sub_code == "ACQ.USER_FACE_PAYMENT_SWITCH_OFF")
                    {
                        br.Success = false;
                        br.Message.Add(string.Format("用户当面付付款开关关闭 需要让用户在手机上打开当面付付款开关 描述:" + retPayModel.alipay_trade_pay_response.sub_msg));
                        br.Level = ErrorLevel.Warning;
                        return br;
                    }
                    else
                    {
                        br.Success = false;
                        br.Message.Add(string.Format("支付宝支付 操作返回失败! 描述:" + retPayModel.alipay_trade_pay_response.sub_msg));
                        return br;
                    }
                }
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Message.Add(string.Format("支付宝支付时发生异常 单号:" + aliModel.out_trade_no + " 请查询确定结果 以免带来损失!"));
                br.Level = ErrorLevel.Warning;
                return br;
            }
        }
        #endregion

        #region 微信支付

        public BaseResult WXPay(WXPayModel wxModel)
        {
            BaseResult br = new BaseResult();
            try
            {
                var paramters = new Dictionary<string, string>();
                paramters.Add("sub_mch_id", wxModel.sub_mch_id);
                paramters.Add("body", wxModel.body);
                paramters.Add("out_trade_no", wxModel.out_trade_no);
                paramters.Add("total_fee", wxModel.total_fee.ToString());
                paramters.Add("auth_code", wxModel.auth_code);
                string mySign = CYUtils.SignRequestNew(paramters, PublicSign.localKey);
                paramters.Add("sign", mySign);
                var result = new WebUtils().DoPost(PublicSign.wxPayUrl, paramters, 200000);

                WXPayResultModel reqMicropay = XmlUtil.Deserialize(typeof(WXPayResultModel), result.Replace("</xml>", "</WXPayResultModel>").Replace("<xml>", "<WXPayResultModel>")) as WXPayResultModel;
                if (reqMicropay.result_code == "SUCCESS" && reqMicropay.return_code == "SUCCESS")
                {
                    br.Success = true;
                    br.Data = reqMicropay;
                    return br;
                }
                else
                {
                    if (reqMicropay.err_code == "BANKERROR" || reqMicropay.err_code == "SYSTEMERROR")
                    {
                        br.Success = false;
                        br.Message.Add(string.Format("支付结果未知 需要查询结果 描述:" + reqMicropay.err_code_des));
                        br.Level = ErrorLevel.Warning;
                        return br;
                    }
                    else if (reqMicropay.err_code == "USERPAYING")
                    {
                        br.Success = false;
                        br.Message.Add(string.Format("用户支付中，需要输入密码 描述:" + reqMicropay.err_code_des));
                        br.Level = ErrorLevel.Warning;
                        return br;
                    }
                    else if (reqMicropay.err_code == "ORDERPAID")
                    {
                        br.Success = true;
                        br.Message.Add(string.Format("订单已被支付 描述:" + reqMicropay.err_code_des));
                        br.Data = reqMicropay;
                        return br;
                    }
                    else
                    {
                        br.Success = false;
                        br.Message.Add(string.Format("微信返回失败 描述:" + reqMicropay.err_code_des));
                        return br;
                    }
                }
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Message.Add(string.Format("微信支付时发生异常 单号:" + wxModel.out_trade_no + "  请查询确定结果 以免带来损失!"));
                return br;
            }
        }
        #endregion

        #region AliPayQuery 支付宝订单查询
        /// <summary>
        /// AliPayQuery 支付宝订单查询
        /// lz
        /// 2016-12-08
        /// </summary>
        [Transaction]
        public BaseResult AliPayQuery(Hashtable param)
        {
            #region 定义参数
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            #endregion

            #region 验证参数
            if (param == null ||
                  param["id_shop"] == null || string.IsNullOrEmpty(param["id_shop"].ToString()) ||
                  param["id_masteruser"] == null || string.IsNullOrEmpty(param["id_masteruser"].ToString()) ||
                  param["sign"] == null || string.IsNullOrEmpty(param["sign"].ToString()) ||
                  !param.ContainsKey("out_trade_no") ||
                  !param.ContainsKey("trade_no")
                )
            {
                br.Success = false;
                br.Message.Add("必要参数不可以为空.");
                return br;
            }

            #endregion

            #region 获取支付配置信息
            int i = 1;
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("id_shop", param["id_shop"].ToString());
            var payConfigList = DAL.QueryList<Tb_Pay_Config>(typeof(Tb_Pay_Config), ht);
            #endregion

            #region 检查有没有设置支付类型
            if (payConfigList == null || payConfigList.Where(d => d.flag_type == 4).Count() <= 0)
            {
                br.Success = false;
                br.Message.Add(string.Format("没有设置支付宝支付类型,不能查询."));
                return br;
            }

            if (payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_is_use" && d.parmvalue == "2").Count() <= 0)
            {
                br.Success = false;
                br.Message.Add(string.Format("没有启用支付宝支付,不能查询."));
                return br;
            }

            if (payConfigList == null || payConfigList.Count() <= 0 || payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_partner" && d.parmvalue != "").Count() <= 0)
            {
                br.Success = false;
                br.Message.Add(string.Format("没有设置支付宝商户身份ID,不能查询."));
                return br;
            }

            if (payConfigList == null || payConfigList.Count() <= 0 || payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_store_id" && d.parmvalue != "").Count() <= 0)
            {
                br.Success = false;
                br.Message.Add(string.Format("没有设置支付宝商户门店编号,不能查询."));
                return br;
            }

            #endregion


            var paramters = new Dictionary<string, string>();
            paramters.Add("m_code", payConfigList.Where(d => d.flag_type == 4 && d.parmcode == "pay_alipay_partner").FirstOrDefault().parmvalue); //m_code
            paramters.Add("out_trade_no", param["out_trade_no"].ToString()); //out_trade_no
            paramters.Add("trade_no", param["trade_no"].ToString()); //trade_no
            string mySign = CYUtils.SignRequestNew(paramters, PublicSign.localKey);
            paramters.Add("sign", mySign);

            var result = new WebUtils().DoPost(PublicSign.aliQueryUrl, paramters, 200000);

            AliPayResultModel retPayModel = Utility.JSON.Deserialize<AliPayResultModel>(result);
            if (retPayModel.alipay_trade_query_response.code == "10000" && retPayModel.alipay_trade_query_response.msg == "Success")
            {
                retPayModel.out_trade_no = retPayModel.alipay_trade_query_response.out_trade_no;
                retPayModel.trade_no = retPayModel.alipay_trade_query_response.trade_no;

                var aliPayResultModel_Ret = new AliPayResultModel_Ret()
                {
                    out_trade_no = retPayModel.out_trade_no,
                    trade_no = retPayModel.trade_no,
                    alipay_trade_query_response = retPayModel.alipay_trade_query_response
                };
                br.Success = true;
                br.Data = aliPayResultModel_Ret;
                return br;
            }
            else
            {
                br.Success = false;
                br.Message.Add(string.Format("支付宝查询 请稍后再试 操作返回 非支付状态!!"));
                return br;
            }

        }
        #endregion

        #region WXPayQuery 微信订单查询
        /// <summary>
        /// WXPayQuery 微信订单查询
        /// lz
        /// 2016-12-08
        /// </summary>
        [Transaction]
        public BaseResult WXPayQuery(Hashtable param)
        {
            #region 定义参数
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            #endregion

            #region 验证参数
            if (param == null ||
                  param["id_shop"] == null || string.IsNullOrEmpty(param["id_shop"].ToString()) ||
                  param["id_masteruser"] == null || string.IsNullOrEmpty(param["id_masteruser"].ToString()) ||
                  param["sign"] == null || string.IsNullOrEmpty(param["sign"].ToString()) ||
                  !param.ContainsKey("out_trade_no")
                )
            {
                br.Success = false;
                br.Message.Add("必要参数不可以为空.");
                return br;
            }

            #endregion

            #region 获取支付配置信息
            int i = 1;
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("id_shop", param["id_shop"].ToString());
            var payConfigList = DAL.QueryList<Tb_Pay_Config>(typeof(Tb_Pay_Config), ht);
            #endregion

            #region 检查有没有设置支付类型
            if (payConfigList == null || payConfigList.Where(d => d.flag_type == 5).Count() <= 0)
            {
                br.Success = false;
                br.Message.Add(string.Format("没有设置微信支付类型,不能查询."));
                return br;
            }

            if (payConfigList.Where(d => d.flag_type == 5 && d.parmcode == "pay_wx_is_use" && d.parmvalue == "1").Count() <= 0)
            {
                br.Success = false;
                br.Message.Add(string.Format("没有启用微信支付,不能查询."));
                return br;
            }

            if (payConfigList == null || payConfigList.Count() <= 0 || payConfigList.Where(d => d.flag_type == 5 && d.parmcode == "pay_wx_mch_id_child" && d.parmvalue != "").Count() <= 0)
            {
                br.Success = false;
                br.Message.Add(string.Format("没有设置微信支付子商户号,不能查询."));
                return br;
            }

            #endregion

            #region 调用接口查询
            try
            {
                var paramters = new Dictionary<string, string>();
                paramters.Add("sub_mch_id", payConfigList.Where(d => d.flag_type == 5 && d.parmcode == "pay_wx_mch_id_child").FirstOrDefault().parmvalue); //sub_mch_id
                paramters.Add("out_trade_no", param["out_trade_no"].ToString()); //out_trade_no
                string mySign = CYUtils.SignRequestNew(paramters, PublicSign.localKey);
                paramters.Add("sign", mySign);

                var result = new WebUtils().DoPost(PublicSign.wxQueryUrl, paramters, 200000);

                WXPayResultModel reqMicropay = XmlUtil.Deserialize(typeof(WXPayResultModel), result.Replace("</xml>", "</WXPayResultModel>").Replace("<xml>", "<WXPayResultModel>")) as WXPayResultModel;
                if (reqMicropay.result_code == "SUCCESS" && reqMicropay.return_code == "SUCCESS")
                {
                    if (reqMicropay.trade_state == "SUCCESS")
                    {
                        br.Success = true;
                        br.Data = reqMicropay;
                        return br;
                    }
                    else
                    {
                        br.Success = false;
                        br.Message.Add(string.Format("订单未未支付状态 描述:" + reqMicropay.trade_state_desc));
                        if (reqMicropay.trade_state == "USERPAYING")
                            br.Level = ErrorLevel.Warning;
                        return br;
                    }
                }
                else
                {
                    br.Success = false;
                    br.Message.Add(string.Format("微信查询 操作返回 非支付状态!"));
                    return br;
                }
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Message.Add(string.Format("微信查询时发生异常 请稍后再试 单号:" + param["out_trade_no"].ToString() + " !"));
                return br;
            } 
            #endregion

        }
        #endregion



    }
}
