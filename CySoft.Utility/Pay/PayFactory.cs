using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Utility.Pay
{
   public class PayFactory
    {
       public static PaymentRequest CreatePaymentRequest(PaymentParms param)
       {
           PaymentRequest payRequest = null;
           switch(param.name)
           {
               case "hishop.plugins.payment.alipaydirect.directrequest":
                   payRequest = new Alipay.AlipayRequest(param);
                   break;
               case "tenpay":
                   payRequest = new TenPay.TenPayRequest(param);
                   break;
               case"yeepay":
                   payRequest = new YeePay.YeePayRequest(param);
                   break;
           }
           return payRequest;
       }
       public static PaymentNotify CreatePaymentNotify(string name)
       {
           PaymentNotify payNotify = null;
           switch(name)
           {
               case "alipay":
                   payNotify = new Alipay.AlipayNotify();
                   break;
               case"tenpay":
                   payNotify = new TenPay.TenPayNotify();
                   break;
               case "yeepay":
                   payNotify = new YeePay.YeePayNotify();
                   break;
           }
           return payNotify;
       }
    }
}
