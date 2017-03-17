using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;
using System.Collections.Specialized;

#region 支付通知
#endregion
namespace CySoft.Utility.Pay.Alipay
{
   public class AlipayNotify:PaymentNotify
    {
        #region 字段
        private string _partner = "";               //合作身份者ID
        private string _key = "";                   //商户的私钥
        private string _charset = "";               //编码格式
        private string _sign_type = "";             //签名方式

        //支付宝消息验证地址
        private string Https_veryfy_url = "";
        #endregion

       public AlipayNotify()
       {
           //初始化基本配置信息
           _partner = "2088511231930430";
           _sign_type = "MD5";
           _key = "w5wcogx3f1a0zjtyazvh5g8cqy7eu1gj";
           _charset = "utf-8";
           Https_veryfy_url = "https://mapi.alipay.com/gateway.do?service=notify_verify&";

       }
       public override bool Verify(Hashtable parms)
       {
           NameValueCollection parameters=new NameValueCollection();
           foreach (DictionaryEntry item in parms)
           {
               parameters.Add(item.Key.ToString(),item.Value.ToString());
           }
           var sign = parms["sign"].ToString();
           var notify_id = parms["notify_id"].ToString();

           //验证签名
           bool isSign = false;
           string[] requestarr = parameters.AllKeys;
           // 参数排序
           string[] sortedstr = AlipayRequest.BubbleSort(requestarr);

           // 构造待md5摘要字符串
           string prestr = "";
           for (int i = 0; i < sortedstr.Length; i++)
           {
               if (
                   !string.IsNullOrEmpty(parameters[sortedstr[i]]) &&
                   (sortedstr[i] != "sign") &&
                   (sortedstr[i] != "sign_type")
                   )
               {
                   if (i == sortedstr.Length - 1)
                   {
                       prestr = prestr + sortedstr[i] + "=" + parameters[sortedstr[i]];
                   }
                   else
                   {
                       prestr = prestr + sortedstr[i] + "=" + parameters[sortedstr[i]] + "&";
                   }
               }

           }

           prestr = prestr + _key;
           if (sign.Equals(AlipayRequest.GetMD5(prestr.ToString(), _charset)))
           {
               isSign = true;
           }
           
           //获取是否是支付宝服务器发来的请求的验证结果
           string responseTxt = "true";
           if (notify_id != null && notify_id != "") { responseTxt = GetResponseTxt(notify_id); }

           if (responseTxt == "true" && isSign)//验证成功
           {
               return true;
           }
           else//验证失败
           {
               return false;
           }
       }


       /// <summary>
       /// 获取是否是支付宝服务器发来的请求的验证结果
       /// </summary>
       /// <param name="notify_id">通知验证ID</param>
       /// <returns>验证结果</returns>
       private string GetResponseTxt(string notify_id)
       {
           string veryfy_url = Https_veryfy_url + "partner=" + _partner + "&notify_id=" + notify_id;

           //获取远程服务器ATN结果，验证是否是支付宝服务器发来的请求
           string responseTxt = GetResponse(veryfy_url, 120000);

           return responseTxt;
       }

    }
}
