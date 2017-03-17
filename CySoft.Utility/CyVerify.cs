#region Imports
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using CySoft.Frame.Core;
#endregion

namespace CySoft.Utility
{
    public sealed class CyVerify
    {
        public static readonly Regex URLRegex = new Regex(@"^[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>'\'])*$", RegexOptions.Compiled);
        public static readonly Regex NumericRegex = new Regex(@"^(\-|\+)?\d+(\.\d+)?$", RegexOptions.Compiled);
        public static readonly Regex TimeRegex = new Regex(@"^(([0,1]?[0-9])|(2[0-3])):[0-5]?[0-9](:[0-5]?[0-9](\.\d+)?)?$", RegexOptions.Compiled);

        private CyVerify() { }

        public static bool IsURL(object value)
        {
            if (IsNull(value))
            {
                return false;
            }
            string strValue;
            if (String.IsNullOrEmpty(strValue = value.ToString()))
            {
                return false;
            }
            return URLRegex.IsMatch(strValue);
        }

        public static bool IsURL(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return false;
            }
            return true; //URLRegex.IsMatch(value);
        }

        public static bool IsNumeric(object value)
        {
            if (IsNull(value))
            {
                return false;
            }
            string strValue;
            if (String.IsNullOrEmpty(strValue = value.ToString()))
            {
                return false;
            }
            return NumericRegex.IsMatch(strValue);
        }

        public static bool IsNumeric(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return false;
            }
            return NumericRegex.IsMatch(value);
        }

        public static bool IsInt32(object value)
        {
            if (IsNull(value))
            {
                return false;
            }
            string strValue;
            if (String.IsNullOrEmpty(strValue = value.ToString()))
            {
                return false;
            }
            int t;
            return Int32.TryParse(strValue, out t);
        }

        public static bool IsInt32(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return false;
            }
            int t;
            return Int32.TryParse(value, out t);
        }

        public static bool IsTime(object value)
        {
            if (IsNull(value))
            {
                return false;
            }
            string strValue;
            if (String.IsNullOrEmpty(strValue = value.ToString()))
            {
                return false;
            }
            return TimeRegex.IsMatch(strValue);
        }

        public static bool IsTime(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return false;
            }
            return TimeRegex.IsMatch(value);
        }

        public static bool IsNull(object value)
        {
            if (value == null
                || value == Stream.Null
                || value == StreamReader.Null || value == StreamWriter.Null
                || value == TextReader.Null || value == TextWriter.Null
                || value == BinaryWriter.Null
                )
            {
                return true;
            }
            return false;
        }

        public static bool IsNull(string value)
        {
            if (value == null)
            {
                return true;
            }
            return false;
        }

        public static bool IsNull(Stream value)
        {
            if (value == null || value == Stream.Null
                )
            {
                return true;
            }
            return false;
        }

        public static bool IsNull(StreamReader value)
        {
            if (value == null || value == StreamReader.Null
                )
            {
                return true;
            }
            return false;
        }

        public static bool IsNull(StreamWriter value)
        {
            if (value == null || value == StreamWriter.Null
                )
            {
                return true;
            }
            return false;
        }

        public static bool IsNull(TextReader value)
        {
            if (value == null || value == TextReader.Null
                )
            {
                return true;
            }
            return false;
        }

        public static bool IsNull(TextWriter value)
        {
            if (value == null || value == TextWriter.Null
                )
            {
                return true;
            }
            return false;
        }

        public static bool IsNull(BinaryWriter value)
        {
            if (value == null || value == BinaryWriter.Null
                )
            {
                return true;
            }
            return false;
        }

        public static bool IsImage(string fileName)
        {
            if (IsNull(fileName))
            {
                return false;
            }
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("文件未创建或没有足够的访问权限！", fileName);
            }
            try
            {
                Image.FromFile(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsImage(Stream stream)
        {
            if (IsNull(stream))
            {
                return false;
            }
            try
            {
                Image.FromStream(stream);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary >
        /// 判断是否有中文
        /// </summary >
        /// <param name="str" ></param >
        /// <returns ></returns >
        public static bool IsIncludeChinese(string str)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]");
            Match m = regex.Match(str);
            return m.Success;
        }

        /// <summary>
        /// 验证是否是手机格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPhone(string str)
        {
            Regex regex = new Regex(@"^(1+\d{10})$");
            Match m = regex.Match(str);
            if (str.Length == 11 && regex.IsMatch(str))
            {
                return true;
            }
            else
            {
                return false;
            }
                
        }



        public static BaseResult CheckUserName(string username)
        {
            BaseResult br = new BaseResult();
            if (username.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("用户名不能为空");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            Regex rx = new Regex(@"^([a-zA-Z0-9]{4,20})$");
            if (!rx.IsMatch(username))
            {
                br.Success = false;
                br.Message.Add("用户名必须由4-20位数字和字母组成");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            rx = new Regex(@"^(1+\d{10})$");
            if (username.Length == 11 && rx.IsMatch(username))
            {
                br.Success = false;
                br.Message.Add("用户名不能用手机号，如果要用手机号登录，请使用手机验证功能");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            rx = new Regex(@"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$");
            if (rx.IsMatch(username))
            {
                br.Success = false;
                br.Message.Add("用户名不能用邮箱，如果要用邮箱号登录，请使用邮箱验证功能");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            br.Success = true;
            return br;
        }
    }
}
