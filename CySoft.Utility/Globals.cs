using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CySoft.Utility
{
    public class Globals
    {
        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string ClientIP
        {
            get
            {
                string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (string.IsNullOrWhiteSpace(result))
                    result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrWhiteSpace(result))
                    result = HttpContext.Current.Request.UserHostAddress;

                if (string.IsNullOrWhiteSpace(result) || !IsIP(result))
                    return "127.0.0.1";

                return result;
            }
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }



        /// <summary>
        /// IList -> List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list_old"></param>
        /// <returns></returns>
        public static List<T> ConvertToCollectionList<T>(IList<T> list_old)
            where T : new()
        {
            List<T> list_new = new List<T>();
            if (list_old == null)
                return list_new;

            list_new.AddRange(list_old);
            return list_new;
        }



        #region Url编解码

        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str, Encoding.UTF8);
        }

        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str, Encoding.UTF8);
        }

        #endregion Url编解码

        #region 类型转换

        /// <summary>
        /// String -> Int
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns></returns>
        public static int ConvertStringToInt(string str, int defaultvalue = 0)
        {
            int result = 0;

            if (string.IsNullOrWhiteSpace(str))
                return defaultvalue;
            else if (int.TryParse(str, out result))
                return result;
            else
                return defaultvalue;
        }
        /// <summary>
        /// String -> Long
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns></returns>
        public static long ConvertStringToLong(string str, long defaultvalue = 0)
        {
            long result = 0;

            if (string.IsNullOrWhiteSpace(str))
                return defaultvalue;
            else if (long.TryParse(str, out result))
                return result;
            else
                return defaultvalue;
        }
        /// <summary>
        /// String -> Double
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns></returns>
        public static double ConvertStringToDouble(string str, double defaultvalue = 0.00)
        {
            double result = 0.00;

            if (string.IsNullOrWhiteSpace(str))
                return defaultvalue;
            else if (double.TryParse(str, out result))
                return result;
            else
                return defaultvalue;
        }
        /// <summary>
        /// String -> Decimal
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns></returns>
        public static decimal ConvertStringToDecimal(string str, decimal defaultvalue = 0.00M)
        {
            decimal result = 0.00M;

            if (string.IsNullOrWhiteSpace(str))
                return defaultvalue;
            else if (decimal.TryParse(str, out result))
                return result;
            else
                return defaultvalue;
        }

        #endregion 类型转换

        #region 类型判断

        /// <summary>
        /// 判断是否Int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsInt(object obj)
        {
            bool isCorrect = false;
            int val = 0;

            if (obj != null)
                isCorrect = int.TryParse(obj.ToString(), out val);

            return isCorrect;
        }

        /// <summary>
        /// 判断是否Decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsDecimal(object obj)
        {
            bool isCorrect = false;
            Decimal val = 0;

            if (obj != null)
                isCorrect = Decimal.TryParse(obj.ToString(), out val);

            return isCorrect;
        }

        /// <summary>
        /// 判断是否DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsDateTime(object obj)
        {
            bool isCorrect = false;
            DateTime val;

            if (obj != null)
                isCorrect = DateTime.TryParse(obj.ToString(), out val);

            return isCorrect;
        }

        #endregion 类型判断
    }
}
