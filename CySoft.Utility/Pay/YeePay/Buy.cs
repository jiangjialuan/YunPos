using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Utility.Pay.YeePay
{
    public class Buy
    {
        private static string nodeAuthorizationURL = @"https://www.yeepay.com/app-merchant-proxy/node";

        // 创建在线支付URL
        internal static string CreateUrl(
                                            string merchantId,
                                            string keyValue,
                                            string orderId,
                                            string amount,
                                            string cur,

                                            string productId,
                                            string merchantCallbackURL,
                                            string addressFlag,

                                            string sMctProperties,
                                            string frpId)
        {
            string messageType = "Buy";
            string needResponse = "1";

            string sbOld = "";
            sbOld = sbOld + messageType;
            sbOld = sbOld + merchantId;
            sbOld = sbOld + orderId;
            sbOld = sbOld + amount;

            sbOld = sbOld + cur;
            sbOld = sbOld + productId;
            sbOld = sbOld + merchantCallbackURL;

            sbOld = sbOld + addressFlag;
            sbOld = sbOld + sMctProperties;
            sbOld = sbOld + frpId;
            sbOld = sbOld + needResponse;

            string sNewString = Digest.HmacSign(sbOld, keyValue);

            string html = "";

            html += nodeAuthorizationURL;
            html += "?p0_Cmd=" + messageType;
            html += "&p1_MerId=" + merchantId;
            html += "&p2_Order=" + orderId;
            html += "&p3_Amt=" + amount;

            html += "&p4_Cur=" + cur;
            html += "&p5_Pid=" + productId;
            html += "&p8_Url=" + System.Web.HttpUtility.UrlEncode(merchantCallbackURL, System.Text.Encoding.GetEncoding("gb2312"));

            html += "&p9_SAF=" + addressFlag;
            html += "&pa_MP=" + sMctProperties;
            html += "&pd_FrpId=" + frpId;
            html += "&pr_NeedResponse=" + needResponse;

            html += "&hmac=" + sNewString;

            return html;
        }

        // 返回url检查md5
        internal static bool VerifyCallback(string merchantId,
                                            string keyValue,
                                            string sCmd,
                                            string sErrorCode,
                                            string sTrxId,

                                            string amount,
                                            string cur,
                                            string productId,
                                            string orderId,
                                            string userId,

                                            string mp,
                                            string bType,
                                            string hmac
                                            )
        {

            string sbOld = "";

            sbOld = sbOld + merchantId;
            sbOld = sbOld + sCmd;
            sbOld = sbOld + sErrorCode;
            sbOld = sbOld + sTrxId;
            sbOld = sbOld + amount;

            sbOld = sbOld + cur;
            sbOld = sbOld + productId;
            sbOld = sbOld + orderId;
            sbOld = sbOld + userId;
            sbOld = sbOld + mp;

            sbOld = sbOld + bType;

            string sNewString = Digest.HmacSign(sbOld, keyValue);

            if (hmac == sNewString)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
