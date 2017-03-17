using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

#region 财付通支付通知
#endregion

namespace CySoft.Utility.Pay.TenPay
{
   public class TenPayNotify:PaymentNotify
    {
       public override bool Verify(Hashtable parms)
       {
           NameValueCollection parameters = new NameValueCollection();
           foreach (DictionaryEntry item in parms)
           {
               parameters.Add(item.Key.ToString(), item.Value.ToString());
           }
           string cmdno = parameters["cmdno"];
           string pay_result = parameters["pay_result"];
           string pay_info = UrlDecode(parameters["pay_info"]);
           string date = parameters["date"];
           string bargainor_id = parameters["bargainor_id"];
           string transaction_id = parameters["transaction_id"];
           string sp_billno = parameters["sp_billno"];
           string total_fee = parameters["total_fee"];
           string fee_type = parameters["fee_type"];
           string attach = parameters["attach"];
           string sign = parameters["sign"];

           // 参数检查
           if (cmdno == null || pay_result == null || pay_info == null || date == null || bargainor_id == null || transaction_id == null ||
               sp_billno == null || total_fee == null || fee_type == null || attach == null || sign == null
               )
           {
               return false;
           }
           // 交易状态
           if (!pay_result.Equals("0"))
           {
               return false;
           }
           //签名验证
           string s="cmdno="+ cmdno + "&pay_result=" + pay_result + "&date=" + date + "&transaction_id=" +
                    transaction_id
                    + "&sp_billno=" + sp_billno + "&total_fee=" + total_fee + "&fee_type=" + fee_type + "&attach=" +
                    attach + "&key=8934e7d15453e97507ef794cf7b0519d";
           if (!sign.Equals(MD5Encrypt.Md5(s).ToUpper()))
           {
               return false;
           }
           return true;
       }

       /// <summary>
       /// 对字符串进行URL解码
       /// </summary>
       /// <param name="instr">待解码的字符串</param>
       /// <returns>解码结果</returns>
       private string UrlDecode(string instr)
       {
           if (instr == null || instr.Trim() == "")
               return "";

           return instr.Replace("%3d", "=").Replace("%26", "&").Replace("%22", "\"").Replace("%3f", "?")
               .Replace("%27", "'").Replace("%20", " ").Replace("%25", "%");
       }
    }
}
