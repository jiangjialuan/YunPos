using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text.RegularExpressions;

namespace CySoft.Utility
{
    public class PublicModth
    {
        static ManualResetEvent signal = new ManualResetEvent(false);
        [DllImport("cydes.dll")]
        static extern int b64_size(int size, int flag);
        [DllImport("cydes.dll")]
        static extern int b64_des(string txt, byte[] mm, string key, int size, int flag);

        [DllImport("cymd.dll")]
        static extern string MDString(string arg1, int arg2);

        public static string MDString_New(string arg1, int arg2)
        {
            return MDString(arg1, arg2);
        }



        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="prestr"></param>
        /// <param name="sign_type"></param>
        /// <param name="_input_charset"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string prestr)
        {
            StringBuilder sb = new StringBuilder(32);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.ASCII.GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }


        /// <summary>
        /// 加密（金额）
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string Encrypt(string txt)
        {
            int size = b64_size(txt.Length, 1);

            byte[] bt = Encoding.Default.GetBytes(new string(' ', size));
            size = txt.Length;
            int flag = b64_des(txt, bt, "chaoying2009", size, 1);
            string str = Encoding.Default.GetString(bt);
            str = str.Replace("\0", "");
            return str;
        }

        /// <summary>
        /// 加密（金额）
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string Encrypt_Key(string txt, string key)
        {
            int size = b64_size(txt.Length, 1);

            byte[] bt = Encoding.Default.GetBytes(new string(' ', size));
            size = txt.Length;
            int flag = b64_des(txt, bt, key, size, 1);
            string str = Encoding.Default.GetString(bt);
            str = str.Replace("\0", "");
            return str;
        }


        /// <summary>
        /// 解密（金额）
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string unicode(string txt)
        {
            byte[] by = Encoding.Default.GetBytes(txt);
            System.IO.Stream ms = new System.IO.MemoryStream();
            ms.Write(by, 0, by.Length);

            System.IO.StreamReader rr = new System.IO.StreamReader(ms, Encoding.Unicode);
            string t = rr.ReadToEnd();
            int size = b64_size(txt.Length, 0);

            byte[] mbt = Encoding.Default.GetBytes(new string(' ', size));
            size = txt.Length;
            int flag = b64_des(txt, mbt, "chaoying2009", size, 0);
            string source = Encoding.Default.GetString(mbt);
            source = source.Trim();
            source = source.Replace("\0", "");
            return source;
        }


        public static string unicode_key(string txt, string key)
        {
            byte[] by = Encoding.Default.GetBytes(txt);
            System.IO.Stream ms = new System.IO.MemoryStream();
            ms.Write(by, 0, by.Length);

            System.IO.StreamReader rr = new System.IO.StreamReader(ms, Encoding.Unicode);
            string t = rr.ReadToEnd();
            int size = b64_size(txt.Length, 0);

            byte[] mbt = Encoding.Default.GetBytes(new string(' ', size));
            size = txt.Length;
            int flag = b64_des(txt, mbt, key, size, 0);
            string source = Encoding.Default.GetString(mbt);
            source = source.Trim();
            source = source.Replace("\0", "");
            return source;
        }

        /// <summary>
        /// 中文转unicode
        /// </summary>
        /// <returns></returns>
        public static string unicode_0(string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    outStr += "/u" + ((int)str[i]).ToString("x");
                }
            }
            return outStr;
        }
        /// <summary>
        /// unicode转中文
        /// </summary>
        /// <returns></returns>
        public static string unicode_1(string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("/", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符  
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outStr = ex.Message;
                }
            }
            return outStr;

        }


        public static string GetNextStr(string str)
        {
            
            string rStr = string.Empty;
            if (string.IsNullOrEmpty(str))
                return "1";

            if (CyVerify.IsInt32(str))
                return (int.Parse(str) + 1).ToString();

            //所有非数字字符串
            string strString = Regex.Replace(str, @"\d", "");
            //所有数字字符串
            string strInt = Regex.Replace(str, @"[^\d]*", "");
            if (!string.IsNullOrEmpty(strInt))
                strInt = (int.Parse(strInt) + 1).ToString().PadLeft(strInt.Length, '0');
            else

                strInt = "1";

            if (strString.Length + strInt.Length > str.Length)
            {
                if (strInt.Length > 0)
                {
                    rStr += strInt.Substring(0, 1);
                    strInt = strInt.Remove(0, 1);
                }
            }

            foreach (var c in str)
            {
                if (CyVerify.IsInt32(c.ToString()))
                {
                    if (strInt.Length > 0)
                    {
                        rStr += strInt.Substring(0, 1);
                        strInt = strInt.Remove(0, 1);
                    }
                }
                else
                    rStr += c.ToString();
            }

            return rStr;
        }





     












       

        

      
    }
}






