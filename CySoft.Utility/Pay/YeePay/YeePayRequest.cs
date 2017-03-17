using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

#region 易宝支付
#endregion

namespace CySoft.Utility.Pay.YeePay
{
   public class YeePayRequest:PaymentRequest
    {
       public YeePayRequest(PaymentParms param)
       {
           this.amount = param.amount.ToString("F", CultureInfo.InvariantCulture);
           this.orderId = param.orderId;
           this.merchantCallbackURL = param.returnUrl;
           this.MerchantId = param.partner;
           this.KeyValue = param.key;
       }
       #region 常量
       private const string Cur = "CNY";
       private const string SMctProperties = "YeePay";

       /// <summary>
       /// 需要填写送货信息 0：不需要  1:需要
       /// </summary>
       private const string AddressFlag = "0";
       #endregion

       /// <summary>
       /// 商家ID
       /// </summary>
       public string MerchantId { get; set; }

       /// <summary>
       /// 商家密钥
       /// </summary>
       public string KeyValue { get; set; }

       /// <summary>
       /// 商家的交易定单号
       /// </summary>
       private readonly string orderId = "";

       /// <summary>
       /// 订阅金额
       /// </summary>
       private readonly string amount = "";

       /// <summary>
       /// 交易结果通知地址
       /// </summary>
       private readonly string merchantCallbackURL = "";

       /// <summary>
       /// 合作伙伴商户号
       /// </summary>
       public string Pid { get; set; }

       public override void SendRequest()
       {
           RedirectToGateway(Buy.CreateUrl(MerchantId,KeyValue,orderId,amount,Cur,"",merchantCallbackURL,
                                           AddressFlag,SMctProperties,""));
       }
    }
}
