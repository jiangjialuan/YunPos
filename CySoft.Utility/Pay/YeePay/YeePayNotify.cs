using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

#region 易宝支付通知
#endregion

namespace CySoft.Utility.Pay.YeePay
{
   public class YeePayNotify:PaymentNotify
    {
       public override bool Verify(Hashtable parms)
       {
           NameValueCollection parameters = new NameValueCollection();
           foreach (DictionaryEntry item in parms)
           {
               parameters.Add(item.Key.ToString(), item.Value.ToString());
           }
           string r0_Cmd = parameters["r0_Cmd"];
           string p1_MerId = parameters["p1_MerId"];
           string r1_Code = parameters["r1_Code"];
           string r2_TrxId = parameters["r2_TrxId"];
           string r3_Amt = parameters["r3_Amt"];
           string r4_Cur = parameters["r4_Cur"];
           string r5_Pid = parameters["r5_Pid"];
           string r6_Order = parameters["r6_Order"];
           string r7_Uid = parameters["r7_Uid"];
           string r8_MP = parameters["r8_MP"];
           string r9_BType = parameters["r9_BType"];
           string hmac = parameters["hmac"];
           if (r1_Code != "1")
           {
               return false;
           }
           if (!Buy.VerifyCallback(
                p1_MerId, "574584H38Msx80980026QKzbX588Zv0xh95ph8ZG67dj7x69k5091xvm0013", r0_Cmd, r1_Code,
                r2_TrxId, r3_Amt, r4_Cur, r5_Pid, r6_Order, r7_Uid, r8_MP, r9_BType, hmac)
                )
           {
               return false;
           }
           return true;
       }
    }
}
