using System.Security.Cryptography;
using System.Text;

namespace CySoft.Utility.DEncrypt
{
    public class Md5Encrypt
    {
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
    }
}
