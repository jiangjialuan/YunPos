using CySoft.Model.CySoft.Model.Tb;
using CySoft.Model.Tb;
using CySoft.Model.Td;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CySoft.Model.CySoft.Model.Td
{
    //public class Td_Ls_1_Sk_OrderPayReq : Tb_User_ReqOrderQuery
    //{
    //    public string dh { set; get; }
    //    public List<Money_OrderPayList> payList { set; get; }
    //}

    public class Money_OrderPayList
    {
        public string type { set; get; }
        public decimal? money { set; get; }
        public string data { set; get; }
    }

    public class HY_OrderPayModel
    {
        public string id_hy { set; get; }
        public string id_bill { set; get; }
        public string bm_djlx { set; get; }
        public string password { set; get; }
        public string bz { set; get; }
    }

    public class WXPayModel
    {
        /// <summary>
        /// 子商户号
        /// </summary>
        public string sub_mch_id { set; get; }
        /// <summary>
        /// 商品或支付单简要描述
        /// </summary>
        public string body { set; get; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { set; get; }
        /// <summary>
        /// 订单总金额，单位为分，只能为整数
        /// </summary>
        public int? total_fee { set; get; }
        /// <summary>
        /// 授权码
        /// </summary>
        public string auth_code { set; get; }

    }

    public class WXPayResultModel
    {
        /// <summary>
        /// 返回状态码
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string return_msg { get; set; }
        /// <summary>
        /// 业务结果
        /// </summary>
        public string result_code { get; set; }

        ///// <summary>
        ///// 商户订单号
        ///// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public int? total_fee { get; set; }
        public string err_code { get; set; }
        public string err_code_des { get; set; }
        public string trade_state { get; set; }
        public string trade_state_desc { get; set; }
        

    }




    public class AliPayModel
    {
        #region 旧
        /// <summary>
        /// 支付宝分配给开发者的应用ID
        /// </summary>
        public string app_id { set; get; }
        /// <summary>
        /// 接口名称
        /// </summary>
        public string method { set; get; }
        /// <summary>
        /// 请求使用的编码格式，如utf-8,gbk,gb2312等
        /// </summary>
        public string charset { set; get; }
        /// <summary>
        /// 商户生成签名字符串所使用的签名算法类型，目前支持RSA
        /// </summary>
        public string sign_type { set; get; }
        /// <summary>
        /// 商户请求参数的签名串
        /// </summary>
        public string sign { set; get; }

        /// <summary>
        /// 发送请求的时间，格式"yyyy-MM-dd HH:mm:ss"
        /// </summary>
        public string timestamp { set; get; }
        /// <summary>
        /// 调用的接口版本，固定为：1.0
        /// </summary>
        public string version { set; get; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal? discountable_amount { set; get; }
        /// <summary>
        /// 不参与优惠计算的金额
        /// </summary>
        public decimal? undiscountable_amount { set; get; }
        #endregion

        #region 新

        /// <summary>
        /// 商户id
        /// </summary>
        public string m_code { set; get; }
        /// <summary>
        /// 支付场景 条码支付，取值：bar_code 声波支付，取值：wave_code
        /// </summary>
        public string scene { set; get; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal? total_amount { set; get; }
        /// <summary>
        /// 终端id
        /// </summary>
        public string terminal_id { set; get; }
        /// <summary>
        /// 门店编号
        /// </summary>
        public string store_id { set; get; }
        /// <summary>
        /// 商户订单号,64个字符以内、可包含字母、数字、下划线；需保证在商户端不重复
        /// </summary>
        public string out_trade_no { set; get; }
        /// <summary>
        /// 支付授权码
        /// </summary>
        public string auth_code { set; get; }
        /// <summary>
        /// 操作者id
        /// </summary>
        public string operator_id { set; get; }
        /// <summary>
        /// 订单描述
        /// </summary>
        public string body { set; get; }
        /// <summary>
        /// 订单标题
        /// </summary>
        public string subject { set; get; } 
        #endregion
    }

    public class biz_content
    {
        public string out_trade_no { set; get; }
        public string scene { set; get; }
        public string auth_code { set; get; }
        public decimal? total_amount { set; get; }
        public string subject { set; get; }
        public string store_id { set; get; }
    }




    public class AliPayResultModel
    {
        public AliTradePayResponse alipay_trade_pay_response { get; set; }
        public string sign { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 支付宝交易号 
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string store_name { get; set; }


        public AliTradePayResponse alipay_trade_query_response { get; set; }

    }

    public class AliPayResultModel_Ret
    {
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 支付宝交易号 
        /// </summary>
        public string trade_no { get; set; }

        public AliTradePayResponse alipay_trade_query_response { get; set; }

    }


    public class AliTradePayResponse
    {
        public string code { get; set; }
        public string msg { get; set; }



        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 支付宝交易号 
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string store_name { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string sub_code { get; set; }
        /// <summary>
        /// 错误码解释
        /// </summary>
        public string sub_msg { get; set; }
        
    }


    public class PayOrderQuery
    {
        public int id_gsjg { set; get; }
        public int id_user { set; get; }
        public string type { get; set; }
        public string dh { get; set; }
    }

    //public class PayOrderQueryUpdate : Td_Ls_Dd_1
    //{
    //    public HttpContext Context { get; set; }

    //    public List<Tb_Jsfs> JXFSList { get; set; }
    //}


}
