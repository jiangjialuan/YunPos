using System;

namespace CySoft.Utility.Weixin.Util
{
    public class CheckSignature
    {
        /// <summary>
        /// 验证消息来自微信服务器
        /// </summary>
        /// <param name="signature">微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="token">随机字符串</param>
        /// <returns></returns>
        public static bool Check(string signature, string timestamp, string nonce, string token = "weixin_notoken")
        {
            string[] array = new string[] { timestamp, nonce, token };
            Array.Sort<string>(array);
            string str_signature = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(string.Join(string.Empty, array), "SHA1");
            return str_signature.Equals(signature, StringComparison.OrdinalIgnoreCase);
        }
    }
}
