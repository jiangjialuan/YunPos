using System;
using System.Collections.Generic;
using System.Reflection;

namespace CySoft.Model.Mapping
{
    /// <summary>
    /// 枚举扩展方法
    /// </summary>
    public static class EnumExtension
    {
        private static Dictionary<string, string> dic_EnumShowText_Cache = new Dictionary<string, string>();

        /// <summary>
        /// 获取标签文本 [EnumText("文本")]
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static string GetText(this Enum em)
        {
            string str_text = string.Empty;
            string enFullName = GetEnumFullName(em);

            // 先在缓存中找
            if (!dic_EnumShowText_Cache.TryGetValue(enFullName, out str_text))
            {
                // 获取枚举类型
                Type enumType = em.GetType();

                // 找字段
                FieldInfo fi = enumType.GetField(em.ToString());
                if (fi == null)
                {
                    //throw new InvalidOperationException(string.Format("非完整枚举{0}", enFullName));
                    //Utility.LogHelper.Error(string.Format("非完整枚举{0}", enFullName));
                }
                else
                {
                    // 找定制特性
                    object[] atts = fi.GetCustomAttributes(typeof(EnumTextAttribute), false);
                    if (atts != null && atts.Length > 0)
                    {
                        // 找到，设置
                        str_text = (atts[0] as EnumTextAttribute).Text;
                        lock (dic_EnumShowText_Cache)
                        {
                            dic_EnumShowText_Cache[enFullName] = str_text;
                        }
                    }
                    else
                    {
                        // 没找到定制特性，参数指定抛出异常则抛出异常
                        //throw new InvalidOperationException(string.Format("此枚举{0}未定义EnumShowTextAttribute", enFullName));
                        //Utility.LogHelper.Error(string.Format("此枚举{0}未定义EnumTextAttribute", enFullName));
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(str_text))
                str_text = "-";

            return str_text;
        }

        /// <summary>
        /// 获取枚举名
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static string GetName(this Enum em)
        {
            return em.ToString();
        }

        /// <summary>
        /// 获取枚举值 不填则返回零
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static int GetNum(this Enum em)
        {
            return Convert.ToInt32(em);
        }





        /// <summary>
        /// 获取枚举的全名
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        private static string GetEnumFullName(Enum en)
        {
            return string.Format("{0}.{1}", en.GetType().FullName, en.ToString());
        }
    }
}
