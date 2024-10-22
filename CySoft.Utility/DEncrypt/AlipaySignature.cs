﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CySoft.Frame.Common;

namespace CySoft.Utility.DEncrypt
{
    /// <summary>
    /// 来自支付宝SDK的RSA加解密
    /// </summary>
    public class AlipaySignature
    {
        /** 默认编码字符集 */
        private static string DEFAULT_CHARSET = "utf-8";

        public static string GetSignContent(IDictionary<string, string> parameters)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);

            return content;
        }

        public static string RSASign(IDictionary<string, string> parameters, string privateKeyPem, string charset, string signType)
        {
            string signContent = GetSignContent(parameters);

            return RSASignCharSet(signContent, privateKeyPem, charset, signType);
        }

        public static string RSASign(string data, string privateKeyPem, string charset, string signType)
        {
            return RSASignCharSet(data, privateKeyPem, charset, signType);
        }
        ///*
        public static string RSASign(IDictionary<string, string> parameters, string privateKeyPem, string charset, bool keyFromFile, string signType)
        {
            string signContent = GetSignContent(parameters);

            return RSASignCharSet(signContent, privateKeyPem, charset, keyFromFile, signType);
        }
        //*/
        public static string RSASignCharSet(string data, string privateKeyPem, string charset, string signType)
        {
            RSACryptoServiceProvider rsaCsp = LoadCertificateFile(privateKeyPem, signType);
            byte[] dataBytes = null;
            if (string.IsNullOrEmpty(charset))
            {
                dataBytes = Encoding.UTF8.GetBytes(data);
            }
            else
            {
                dataBytes = Encoding.GetEncoding(charset).GetBytes(data);
            }


            if ("RSA2".Equals(signType))
            {

                byte[] signatureBytes = rsaCsp.SignData(dataBytes, "SHA256");

                return Convert.ToBase64String(signatureBytes);

            }
            else
            {
                byte[] signatureBytes = rsaCsp.SignData(dataBytes, "SHA1");

                return Convert.ToBase64String(signatureBytes);
            }
        }

        public static string RSASignCharSet(string data, string privateKeyPem, string charset, bool keyFromFile, string signType)
        {

            byte[] signatureBytes = null;
            try
            {
                RSACryptoServiceProvider rsaCsp = null;
                if (keyFromFile)
                {//文件读取
                    rsaCsp = LoadCertificateFile(privateKeyPem, signType);
                }
                else
                {
                    //字符串获取
                    rsaCsp = LoadCertificateString(privateKeyPem, signType);
                }

                byte[] dataBytes = null;
                if (string.IsNullOrEmpty(charset))
                {
                    dataBytes = Encoding.UTF8.GetBytes(data);
                }
                else
                {
                    dataBytes = Encoding.GetEncoding(charset).GetBytes(data);
                }
                if (null == rsaCsp)
                {
                    //throw new AopException("您使用的私钥格式错误，请检查RSA私钥配置" + ",charset = " + charset);
                    throw new Exception("您使用的私钥格式错误，请检查RSA私钥配置" + ",charset = " + charset);
                }
                signatureBytes = rsaCsp.SignData(dataBytes, "SHA1");

            }
            catch (Exception ex)
            {
                //throw new AopException("您使用的私钥格式错误，请检查RSA私钥配置" + ",charset = " + charset);
                throw new Exception("您使用的私钥格式错误，请检查RSA私钥配置" + ",charset = " + charset);
            }
            return Convert.ToBase64String(signatureBytes);
        }


        public static bool RSACheckV1(IDictionary<string, string> parameters, string publicKeyPem, string charset)
        {
            string sign = parameters["sign"];

            parameters.Remove("sign");
            parameters.Remove("sign_type");
            string signContent = GetSignContent(parameters);
            return RSACheckContent(signContent, sign, publicKeyPem, charset, "RSA");
        }

        public static bool RSACheckV1(IDictionary<string, string> parameters, string publicKeyPem)
        {
            string sign = parameters["sign"];

            parameters.Remove("sign");
            parameters.Remove("sign_type");
            string signContent = GetSignContent(parameters);

            return RSACheckContent(signContent, sign, publicKeyPem, DEFAULT_CHARSET, "RSA");
        }

        public static bool RSACheckV2(IDictionary<string, string> parameters, string publicKeyPem)
        {
            string sign = parameters["sign"];

            parameters.Remove("sign");
            string signContent = GetSignContent(parameters);

            return RSACheckContent(signContent, sign, publicKeyPem, DEFAULT_CHARSET, "RSA");
        }

        public static bool RSACheckV2(IDictionary<string, string> parameters, string publicKeyPem, string charset)
        {
            string sign = parameters["sign"];

            parameters.Remove("sign");
            string signContent = GetSignContent(parameters);

            return RSACheckContent(signContent, sign, publicKeyPem, charset, "RSA");
        }

        /// <summary>
        /// 支付宝响应内容验签
        /// </summary>
        /// <param name="signContent">签名内容</param>
        /// <param name="sign">签名</param>
        /// <param name="publicKeyPem">支付宝公钥文件</param>
        /// <param name="charset">编码格式</param>
        /// <param name="signType">签名算法类型</param>
        /// <returns></returns>
        public static bool RSACheckContent(string signContent, string sign, string publicKeyPem, string charset, string signType)
        {
            try
            {
                if (string.IsNullOrEmpty(charset))
                {
                    charset = DEFAULT_CHARSET;
                }


                if ("RSA2".Equals(signType))
                {
                    string sPublicKeyPEM = File.ReadAllText(publicKeyPem);

                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    rsa.PersistKeyInCsp = false;
                    RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);

                    bool bVerifyResultOriginal = rsa.VerifyData(Encoding.GetEncoding(charset).GetBytes(signContent), "SHA256", Convert.FromBase64String(sign));
                    return bVerifyResultOriginal;

                }
                else
                {
                    string sPublicKeyPEM = File.ReadAllText(publicKeyPem);
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    rsa.PersistKeyInCsp = false;
                    RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);

                    SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                    bool bVerifyResultOriginal = rsa.VerifyData(Encoding.GetEncoding(charset).GetBytes(signContent), sha1, Convert.FromBase64String(sign));
                    return bVerifyResultOriginal;
                }
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                return false;
            }

        }

        public static bool RSACheckContent(string signContent, string sign, string publicKeyPem, string charset, bool keyFromFile)
        {
            try
            {
                string sPublicKeyPEM;
                if (keyFromFile)
                {
                    sPublicKeyPEM = File.ReadAllText(publicKeyPem);
                }
                else
                {
                    sPublicKeyPEM = "-----BEGIN PUBLIC KEY-----\r\n";
                    sPublicKeyPEM = sPublicKeyPEM + publicKeyPem;
                    sPublicKeyPEM = sPublicKeyPEM + "-----END PUBLIC KEY-----\r\n\r\n";
                }
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.PersistKeyInCsp = false;
                RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);
                SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                if (string.IsNullOrEmpty(charset))
                {
                    charset = DEFAULT_CHARSET;
                }
                bool bVerifyResultOriginal = rsa.VerifyData(Encoding.GetEncoding(charset).GetBytes(signContent), sha1, Convert.FromBase64String(sign));
                return bVerifyResultOriginal;
            }
            catch (Exception ex)
            {
                string s = ex.Message.ToString();
                return false;
            }

        }

        public static string CheckSignAndDecrypt(IDictionary<string, string> parameters, string alipayPublicKey,
                                             string cusPrivateKey, bool isCheckSign,
                                             bool isDecrypt)
        {
            string charset = parameters["charset"];
            string bizContent = parameters["biz_content"];
            if (isCheckSign)
            {
                if (!RSACheckV2(parameters, alipayPublicKey, charset))
                {
                    //throw new AopException("rsaCheck failure:rsaParams=" + parameters);
                    throw new Exception("rsaCheck failure:rsaParams=" + parameters);
                }
            }

            if (isDecrypt)
            {
                return RSADecrypt(bizContent, cusPrivateKey, charset, "RSA");
            }

            return bizContent;
        }

        public static string encryptAndSign(string bizContent, string alipayPublicKey,
                                        string cusPrivateKey, string charset, bool isEncrypt,
                                        bool isSign)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(charset))
            {
                charset = DEFAULT_CHARSET;
            }
            sb.Append("<?xml version=\"1.0\" encoding=\"" + charset + "\"?>");
            if (isEncrypt)
            {// 加密
                sb.Append("<alipay>");
                String encrypted = RSAEncrypt(bizContent, alipayPublicKey, charset);
                sb.Append("<response>" + encrypted + "</response>");
                sb.Append("<encryption_type>RSA</encryption_type>");
                if (isSign)
                {
                    String sign = RSASign(encrypted, cusPrivateKey, charset, "RSA");
                    sb.Append("<sign>" + sign + "</sign>");
                    sb.Append("<sign_type>RSA</sign_type>");
                }
                sb.Append("</alipay>");
            }
            else if (isSign)
            {// 不加密，但需要签名
                sb.Append("<alipay>");
                sb.Append("<response>" + bizContent + "</response>");
                String sign = RSASign(bizContent, cusPrivateKey, charset, "RSA");
                sb.Append("<sign>" + sign + "</sign>");
                sb.Append("<sign_type>RSA</sign_type>");
                sb.Append("</alipay>");
            }
            else
            {// 不加密，不加签
                sb.Append(bizContent);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 用私钥加密数据
        /// </summary>
        /// <param name="content"></param>
        /// <param name="publicKeyPem"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string content, string publicKeyPem, string charset)
        {
            try
            {
                string sPublicKeyPEM = File.ReadAllText(publicKeyPem);
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.PersistKeyInCsp = false;
                RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);
                if (string.IsNullOrEmpty(charset))
                {
                    charset = DEFAULT_CHARSET;
                }
                byte[] data = Encoding.GetEncoding(charset).GetBytes(content);
                int maxBlockSize = rsa.KeySize / 8 - 11; //加密块最大长度限制
                if (data.Length <= maxBlockSize)
                {
                    byte[] cipherbytes = rsa.Encrypt(data, false);
                    return Convert.ToBase64String(cipherbytes);
                }
                MemoryStream plaiStream = new MemoryStream(data);
                MemoryStream crypStream = new MemoryStream();
                Byte[] buffer = new Byte[maxBlockSize];
                int blockSize = plaiStream.Read(buffer, 0, maxBlockSize);
                while (blockSize > 0)
                {
                    Byte[] toEncrypt = new Byte[blockSize];
                    Array.Copy(buffer, 0, toEncrypt, 0, blockSize);
                    Byte[] cryptograph = rsa.Encrypt(toEncrypt, false);
                    crypStream.Write(cryptograph, 0, cryptograph.Length);
                    blockSize = plaiStream.Read(buffer, 0, maxBlockSize);
                }

                return Convert.ToBase64String(crypStream.ToArray(), Base64FormattingOptions.None);
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                //throw new AopException("EncryptContent = " + content + ",charset = " + charset, ex);
                throw new Exception("EncryptContent = " + content + ",charset = " + charset, ex);
            }
        }

        /// <summary>
        /// 用私钥解密数据
        /// </summary>
        /// <param name="content"></param>
        /// <param name="privateKeyPem"></param>
        /// <param name="charset"></param>
        /// <param name="signType"></param>
        /// <returns></returns>
        public static string RSADecrypt(string content, string privateKeyPem, string charset, string signType)
        {
            try
            {
                RSACryptoServiceProvider rsaCsp = LoadCertificateFile(privateKeyPem, signType);
                if (string.IsNullOrEmpty(charset))
                {
                    charset = DEFAULT_CHARSET;
                }
                byte[] data = Convert.FromBase64String(content);
                int maxBlockSize = rsaCsp.KeySize / 8;              //解密块最大长度限制
                if (data.Length <= maxBlockSize)
                {
                    byte[] cipherbytes = rsaCsp.Decrypt(data, false);
                    return Encoding.GetEncoding(charset).GetString(cipherbytes);
                }
                MemoryStream crypStream = new MemoryStream(data);
                MemoryStream plaiStream = new MemoryStream();
                Byte[] buffer = new Byte[maxBlockSize];
                int blockSize = crypStream.Read(buffer, 0, maxBlockSize);
                while (blockSize > 0)
                {
                    Byte[] toDecrypt = new Byte[blockSize];
                    Array.Copy(buffer, 0, toDecrypt, 0, blockSize);
                    Byte[] cryptograph = rsaCsp.Decrypt(toDecrypt, false);
                    plaiStream.Write(cryptograph, 0, cryptograph.Length);
                    blockSize = crypStream.Read(buffer, 0, maxBlockSize);
                }

                return Encoding.GetEncoding(charset).GetString(plaiStream.ToArray());
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                //throw new AopException("DecryptContent = " + content + ",charset = " + charset, ex);
                throw new Exception("DecryptContent = " + content + ",charset = " + charset, ex);
            }
        }

        /// <summary>
        /// 从pem文件中获取密钥字符串
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] GetPem(string type, byte[] data)
        {
            string pem = Encoding.UTF8.GetString(data);
            string header = String.Format("-----BEGIN {0}-----\\n", type);
            string footer = String.Format("-----END {0}-----", type);
            int start = pem.IndexOf(header) + header.Length;
            int end = pem.IndexOf(footer, start);
            string base64 = pem.Substring(start, (end - start));

            return Convert.FromBase64String(base64);
        }

        /// <summary>
        /// 加载私钥文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="signType"></param>
        /// <returns></returns>
        private static RSACryptoServiceProvider LoadCertificateFile(string filename, string signType)
        {
            using (System.IO.FileStream fs = System.IO.File.OpenRead(filename))
            {
                byte[] data = new byte[fs.Length];
                byte[] res = null;
                fs.Read(data, 0, data.Length);
                if (data[0] != 0x30)
                {
                    res = GetPem("RSA PRIVATE KEY", data);
                }
                try
                {
                    RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(res, signType);
                    return rsa;
                }
                catch (Exception ex)
                {
                    TextLogHelper.WriterExceptionLog(ex);
                }
                return null;
            }
        }
        /// <summary>
        /// 读取pem字符串
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="signType"></param>
        /// <returns></returns>
        private static RSACryptoServiceProvider LoadCertificateString(string strKey, string signType)
        {
            byte[] data = null;
            //读取带
            //ata = Encoding.Default.GetBytes(strKey);
            data = Convert.FromBase64String(strKey);
            //data = GetPem("RSA PRIVATE KEY", data);
            try
            {
                RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(data, signType);
                return rsa;
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                //    throw new AopException("EncryptContent = woshihaoren,zheshiyigeceshi,wanerde", ex);
            }
            return null;
        }

        /// <summary>
        /// 解析返回私钥操作类
        /// </summary>
        /// <param name="privkey"></param>
        /// <param name="signType"></param>
        /// <returns></returns>
        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey, string signType)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------ all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);


                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                CspParameters CspParameters = new CspParameters();
                CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;

                int bitLen = 1024;
                if ("RSA2".Equals(signType))
                {
                    bitLen = 2048;
                }

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(bitLen, CspParameters);
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        /// <summary>
        /// 获取块长度
        /// </summary>
        /// <param name="binr"></param>
        /// <returns></returns>
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
    }
}
