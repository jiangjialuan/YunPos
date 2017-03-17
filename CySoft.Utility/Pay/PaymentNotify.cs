using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.IO;

#region 支付通知
#endregion

namespace CySoft.Utility.Pay
{
   public  class PaymentNotify
    {
       public virtual bool Verify(Hashtable parms)
       {
           NameValueCollection parameters = new NameValueCollection();
           foreach (DictionaryEntry item in parms)
           {
               parameters.Add(item.Key.ToString(), item.Value.ToString());
           }
           return false;
       }

       protected virtual string GetResponse(string url, int timeout)
       {
           string strResult;

           try
           {
               HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
               myReq.Timeout = timeout;
               HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();

               using (Stream myStream = response.GetResponseStream())
               {
                   using (StreamReader sr = new StreamReader(myStream, Encoding.Default))
                   {
                       StringBuilder strBuilder = new StringBuilder();

                       while (-1 != sr.Peek())
                       {
                           strBuilder.Append(sr.ReadLine());
                       }

                       strResult = strBuilder.ToString();
                   }
               }
           }
           catch (Exception ex)
           {
               strResult = "Error:" + ex.Message;
           }

           return strResult;
       }
    }
}
