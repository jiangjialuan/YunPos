using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
#region 支付请求
#endregion

namespace CySoft.Utility.Pay
{
  public abstract class PaymentRequest
    {
      
      /// <summary>
      /// 支付请求(GET方式)
      /// </summary>
      /// <param name="url"></param>
      protected virtual void RedirectToGateway(string url)
      {
          HttpContext.Current.Response.Redirect(url,true);
      }

      /// <summary>
      /// 发送支付请求
      /// </summary>
      public abstract void SendRequest();
      
    }
}
