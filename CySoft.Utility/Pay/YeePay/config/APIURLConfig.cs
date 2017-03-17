using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Utility.Pay.YeePay
{
    /// <summary>
    /// 地址配置文件
    /// </summary>
    public class APIURLConfig
    {

        static APIURLConfig()
        {
            //移动终端网页收银台前缀
            mobilePrefix = "https://o2o.yeepay.com";

            //商户通用接口前缀
            merchantPrefix = "https://o2o.yeepay.com";

            //账号注册地址
            registerURL = "/zgt-api/api/register";

            //分账方资质上传地址
            verifyURL = "/zgt-api/api/uploadLedgerQualifications";

            //账户余额查询地址
            queryBalance = "/zgt-api/api/queryBalance";

            //余额提现地址
            cashTransfer = "/zgt-api/api/cashTransfer";

            //订单查询地址
            queryOrder = "/zgt-api/api/queryOrder";

            //移动终端网页收银台支付地址 https://o2o.yeepay.com/zgt-api/api/pay
            webpayURL = "/zgt-api/api/pay";

            //提现请求url地址 https://ok.yeepay.com/payapi/api/tzt/withdraw
            withdrwURL = "/payapi/api/tzt/withdraw";

            //提现查询接口https://ok.yeepay.com/payapi/api/tzt/drawrecord
            drawRecordURL = "/payapi/api/tzt/drawrecord";

            //绑卡结果查询 https://ok.yeepay.com/payapi/api/bankcard/authbind/list
            bankCardListURL = "/payapi/api/bankcard/authbind/list";

            //解除银行卡绑定关系https://ok.yeepay.com/payapi/api/tzt/unbind
            unbindBankCardURL = "/payapi/api/tzt/unbind";

            //银行卡绑定信息查询 https://ok.yeepay.com/payapi/api/bankcard/check
            queryBankCardURL = "/payapi/api/bankcard/check";


            //支付结果查询接口
            queryPayResultURL = "/api/query/order";

            //直接退款
            directFundURL = "/query_server/direct_refund";

            //交易记录查询
            queryOrderURL = "/query_server/pay_single";

            //退款订单查询
            queryRefundURL = "/query_server/refund_single";

            //获取消费清算对账单
            clearPayDataURL = "/query_server/pay_clear_data";

            //获取退款清算对账单
            clearRefundDataURL = "/query_server/refund_clear_data";


        }
        /// <summary>
        /// 一键支付前缀
        /// </summary>
        public static string mobilePrefix
        { get; set; }

        /// <summary>
        /// 商户地址前缀
        /// </summary>
        public static string merchantPrefix
        { get; set; }

        /// <summary>
        /// 账号注册地址
        /// </summary>
        public static string registerURL { get; set; }
        /// <summary>
        /// 分账方资质上传地址
        /// </summary>
        public static string verifyURL { get; set; }
        /// <summary>
        /// 账户余额查询地址
        /// </summary>
        public static string queryBalance { get; set; }
        /// <summary>
        /// 余额提现地址
        /// </summary>
        public static string cashTransfer { get; set; }
        /// <summary>
        /// 订单查询地址
        /// </summary>
        public static string queryOrder { get; set; }
        /// <summary>
        /// 网页一键支付地址
        /// </summary>
        public static string webpayURL
        { get; set; }

        /// <summary>
        /// 提现接口地址
        /// </summary>
        public static string withdrwURL
        { get; set; }

        /// <summary>
        /// 提现结果查询
        /// </summary>
        public static string drawRecordURL
        { get; set; }

        /// <summary>
        /// 绑卡列表查看
        /// </summary>
        public static string bankCardListURL { get; set; }


        /// <summary>
        /// 查询银行卡信息接口
        /// </summary>
        public static string queryBankCardURL { get; set; }


        /// <summary>
        /// 银行卡解绑
        /// </summary>
        public static string unbindBankCardURL { get; set; }


        /// <summary>
        /// 查询支付结果
        /// </summary>
        public static string queryPayResultURL
        { get; set; }

        /// <summary>
        /// 直接退款
        /// </summary>
        public static string directFundURL
        { get; set; }

        /// <summary>
        /// 订单支付接口
        /// </summary>
        public static string queryOrderURL
        { get; set; }

        /// <summary>
        /// 退款订单查询
        /// </summary>
        public static string queryRefundURL
        { get; set; }

        /// <summary>
        /// 获取消费清算对账单
        /// </summary>
        public static string clearPayDataURL
        { get; set; }

        /// <summary>
        /// 获取退款账单
        /// </summary>
        public static string clearRefundDataURL
        { get; set; }
    }
}
