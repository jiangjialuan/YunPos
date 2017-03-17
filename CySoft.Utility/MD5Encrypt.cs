using System;
using System.Security.Cryptography;
using System.Text;

namespace CySoft.Utility
{
    public static class MD5Encrypt
    {
        public static string Encrypt(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException();
            }
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            foreach (byte item in buffer)
            {
                builder.Append(String.Format("{0:x}", item));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding">编码格式</param>
        /// <param name="str"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Encode(Encoding encoding, string str, int code = 32)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = encoding.GetBytes(str);
            byte[] output = md5.ComputeHash(result);
            //string str_result = BitConverter.ToString(output).Replace("-", string.Empty);  //tbMd5pass为输出加密文本

            StringBuilder str_result = new StringBuilder();
            for (int i = 0; i < output.Length; i++)
                str_result.Append(output[i].ToString("x").PadLeft(2, '0'));

            if (code == 16)
                return str_result.ToString().Substring(8, 16);
            else
                return str_result.ToString();
        }
        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string Md5(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                input = "";
                //throw new ArgumentNullException();
            }
            MD5 md5 = new MD5CryptoServiceProvider();
            var data = Encoding.UTF8.GetBytes(input);
            var encs = md5.ComputeHash(data);
            return BitConverter.ToString(encs).Replace("-", "");
        }
        #endregion
    }
}
