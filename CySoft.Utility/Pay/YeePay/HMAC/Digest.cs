using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CySoft.Utility.Pay.YeePay
{
    /// <summary>
    /// Digest 类 
    /// </summary>
    public abstract class Digest
    {
        public Digest() { }

        public static string GetHMAC(Dictionary<string, string> dir, string key)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string item in dir.Values)
            {
                sb.Append((item == null) ? "" : item.Trim());
            }
            string hmac = Digest.HmacSign(sb.ToString(), key);
            return hmac;
        }
        public static byte[] hex2byte(string s)
        {
            byte[] src = Encoding.UTF8.GetBytes(s.ToLower());
            byte[] ret = new byte[src.Length / 2];
            for (int i = 0; i < src.Length; i += 2)
            {
                byte hi = src[i];
                byte low = src[i + 1];
                hi = (byte)((hi >= 'a' && hi <= 'f') ? 0x0a + (hi - 'a') : hi - '0');
                low = (byte)((low >= 'a' && low <= 'f') ? 0x0a + (low - 'a') : low - '0');
                ret[i / 2] = (byte)(hi << 4 | low);
            }
            return ret;
        }
        /// <summary>
        /// 支付请求签名
        /// </summary>
        /// <param name="customernumber"></param>
        /// <param name="requestid"></param>
        /// <param name="amount"></param>
        /// <param name="assure"></param>
        /// <param name="productname"></param>
        /// <param name="productcat"></param>
        /// <param name="productdesc"></param>
        /// <param name="divideinfo"></param>
        /// <param name="callbackurl"></param>
        /// <param name="webcallbackurl"></param>
        /// <param name="bankid"></param>
        /// <param name="period"></param>
        /// <param name="memo"></param>
        /// <param name="hmacKey"></param>
        /// <returns></returns>
        public static string GetHMAC(string customernumber, string requestid, string amount, string assure, string productname, string productcat, string productdesc, string divideinfo, string callbackurl, string webcallbackurl, string bankid, string period, string memo, string hmacKey)
        {
            string sign = "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}";


            sign = string.Format(sign, customernumber, requestid, amount, assure, productname, productcat, productdesc, divideinfo, callbackurl, webcallbackurl, bankid, period, memo);

            return HmacSign(sign, hmacKey);
        }


        /// <summary>
        /// 支付请求回调验签
        /// </summary>
        /// <param name="customernumber"></param>
        /// <param name="requestid"></param>
        /// <param name="code"></param>
        /// <param name="externalid"></param>
        /// <param name="amount"></param>
        /// <param name="payurl"></param>
        /// <returns></returns>
        public static bool PayRequestVerifyHMAC(string customernumber, string requestid, int code, string externalid, string amount, string payurl, string hmacKey, string VSign)
        {
            string sign = "{0}{1}{2}{3}{4}{5}";


            sign = string.Format(sign, customernumber, requestid, code, externalid, amount, payurl);

            sign = HmacSign(sign, hmacKey);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(sign, VSign))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 支付成功回调结果验签
        /// </summary>
        /// <param name="customernumber"></param>
        /// <param name="requestid"></param>
        /// <param name="code"></param>
        /// <param name="notifytype"></param>
        /// <param name="externalid"></param>
        /// <param name="amount"></param>
        /// <param name="cardno"></param>
        /// <param name="hmacKey"></param>
        /// <param name="VSign"></param>
        /// <returns></returns>
        public static bool PayResultVerifyHMAC(string customernumber, string requestid, int code, string notifytype, string externalid, string amount, string cardno, string hmacKey, string VSign)
        {
            string sign = "{0}{1}{2}{3}{4}{5}{6}";
            sign = string.Format(sign, customernumber, requestid, code, notifytype, externalid, amount, cardno);
            sign = HmacSign(sign, hmacKey);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(sign, VSign))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static string HmacSign(string aValue, string aKey)
        {
            byte[] k_ipad = new byte[64];
            byte[] k_opad = new byte[64];
            byte[] keyb;
            byte[] Value;
            keyb = Encoding.UTF8.GetBytes(aKey);
            Value = Encoding.UTF8.GetBytes(aValue);

            for (int i = keyb.Length; i < 64; i++)
                k_ipad[i] = 54;

            for (int i = keyb.Length; i < 64; i++)
                k_opad[i] = 92;

            for (int i = 0; i < keyb.Length; i++)
            {
                k_ipad[i] = (byte)(keyb[i] ^ 0x36);
                k_opad[i] = (byte)(keyb[i] ^ 0x5c);
            }

            HmacMD5 md = new HmacMD5();

            md.update(k_ipad, (uint)k_ipad.Length);
            md.update(Value, (uint)Value.Length);
            byte[] dg = md.finalize();
            md.init();
            md.update(k_opad, (uint)k_opad.Length);
            md.update(dg, 16);
            dg = md.finalize();

            return toHex(dg);
        }

        public static string toHex(byte[] input)
        {
            if (input == null)
                return null;

            StringBuilder output = new StringBuilder(input.Length * 2);

            for (int i = 0; i < input.Length; i++)
            {
                int current = input[i] & 0xff;
                if (current < 16)
                    output.Append("0");
                output.Append(current.ToString("x"));
            }

            return output.ToString();
        }
    }
}
