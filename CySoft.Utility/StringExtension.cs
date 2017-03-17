#region Imports
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
#endregion

namespace CySoft.Utility
{
 

    public static class StringExtension
    {

        /// <summary>
        /// SQL注入过滤
        /// </summary>
        #region public static string SQLFilterStr(object input)
        public static string SQLFilterStr(this object input)
        {
            string inputSQL = String.Empty;
            if (input == null || String.IsNullOrEmpty(inputSQL = input.ToString()) || String.IsNullOrEmpty(inputSQL.Trim()))
            {
                return inputSQL;
            }
            inputSQL = inputSQL.Replace("'", "''");
            return inputSQL;
        }
        #endregion

        public static string Left(this string value, int length)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            if (length < 1)
            {
                return String.Empty;
            }
            value = value.Length > length ? value.Substring(0, length) : value;
            return value;
        }

        public static string Right(this string value, int length)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            if (length < 1)
            {
                return String.Empty;
            }
            value = value.Length > length ? value.Substring(value.Length - length, length) : value;
            return value;
        }

        public static string IsNullOrEmpty(string value, string defaultValue)
        {
            if (String.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            return value;
        }

        public static bool IsEmpty(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return true;
            }
            return false;
        }

        public static string toFormat(this string value, params object[] args)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            return String.Format(value, args);
        }

        public static string Join(this string[] array, string separator)
        {
            if (array != null || array.Length > 0)
            {
                return String.Join(separator, array);
            }
            return String.Empty;
        }

        public static string Join(this int[] array, string separator)
        {
            if (array != null || array.Length > 0)
            {
                string[] strArray = new string[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    strArray[i] = array[i].ToString();
                }
                return String.Join(separator, strArray);
            }
            return String.Empty;
        }

        public static string Join<TSource>(this IEnumerable<TSource> source, Func<TSource, string> keySelector, string separator)
        {
            string result = String.Empty;
            if (source != null && source.Count() > 0)
            {
                string[] value = source.Select(keySelector).ToArray();
                result = String.Join(separator, value);
            }
            return result;
        }

        public static bool Contains(this string[] array, string value)
        {
            if (array != null && array.Count() > 0)
            {
                foreach (string item in array)
                {
                    if (item.Equals(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static int GetBytesLength(this string value)
        {
            return GetBytesLength(value, Encoding.Default);
        }

        public static int GetBytesLength(this string value, Encoding encoding)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            return encoding.GetBytes(value).Length;
        }

        public static int ToInt32(this string value)
        {
            return Convert.ToInt32(value);
        }


        public static string GetNextStr(this string str)
        {
            string rStr = string.Empty;
            if (string.IsNullOrEmpty(str))
                return "0001";

            //if (CyVerify.IsInt32(str))
            //    return (int.Parse(str) + 1).ToString();

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

        public static string GetJYStr(this string str)
        {
            string rStr = string.Empty;
            if (string.IsNullOrEmpty(str))
                return "0";
            var hashCode = Math.Abs(MD5Encrypt.Md5(str+PublicSign.sign).GetHashCode() % 10);
            return str + hashCode;
        }

    }
}
