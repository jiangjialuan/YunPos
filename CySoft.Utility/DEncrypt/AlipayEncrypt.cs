using System;
using System.Text;
using CySoft.Frame.Common;

namespace CySoft.Utility.DEncrypt
{
    /// <summary>
    /// 来自支付宝SDK的AES加解密
    /// </summary>
    public class AlipayEncrypt
    {

        /// <summary>
        /// 128位0向量
        /// </summary>
        private static byte[] AES_IV = initIv(16);

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="encryptKey"></param>
        /// <param name="bizContent"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string AesEncrypt(string encryptKey, string bizContent, string charset = null)
        {
            if (string.IsNullOrWhiteSpace(encryptKey))
                encryptKey = AppConfig.GetValue("AesKey");

            Byte[] keyArray = Convert.FromBase64String(encryptKey);
            Byte[] toEncryptArray = null;

            if (string.IsNullOrWhiteSpace(charset))
            {
                toEncryptArray = Encoding.UTF8.GetBytes(bizContent);
            }
            else
            {
                toEncryptArray = Encoding.GetEncoding(charset).GetBytes(bizContent);
            }

            System.Security.Cryptography.RijndaelManaged rDel = new System.Security.Cryptography.RijndaelManaged();

            try
            {
                rDel.Key = keyArray;
                rDel.Mode = System.Security.Cryptography.CipherMode.CBC;
                rDel.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                rDel.IV = AES_IV;

                System.Security.Cryptography.ICryptoTransform cTransform = rDel.CreateEncryptor(rDel.Key, rDel.IV);
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray);
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                return null;
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptKey"></param>
        /// <param name="bizContent"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string AesDencrypt(string encryptKey, string bizContent, string charset = null)
        {
            if (string.IsNullOrWhiteSpace(encryptKey))
                encryptKey = AppConfig.GetValue("AesKey");

            Byte[] keyArray = Convert.FromBase64String(encryptKey);
            Byte[] toEncryptArray = Convert.FromBase64String(bizContent);

            System.Security.Cryptography.RijndaelManaged rDel = new System.Security.Cryptography.RijndaelManaged();

            try
            {
                rDel.Key = keyArray;
                rDel.Mode = System.Security.Cryptography.CipherMode.CBC;
                rDel.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                rDel.IV = AES_IV;

                System.Security.Cryptography.ICryptoTransform cTransform = rDel.CreateDecryptor(rDel.Key, rDel.IV);
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                if (string.IsNullOrWhiteSpace(charset))
                    return Encoding.UTF8.GetString(resultArray);
                else
                    return Encoding.GetEncoding(charset).GetString(resultArray);
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 初始化向量
        /// </summary>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        private static byte[] initIv(int blockSize)
        {
            byte[] iv = new byte[blockSize];
            for (int i = 0; i < blockSize; i++)
            {
                iv[i] = (byte)0x0;
            }
            return iv;
        }
    }
}
