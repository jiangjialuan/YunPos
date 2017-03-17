using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text.RegularExpressions;
using CySoft.Utility;

namespace CySoft.Utility.Mvc.Html
{
    public static class NumberExtensions
    {

        public static MvcHtmlString Number(this HtmlHelper html, object value, NumberType numberType)
        {
            return Number(html, value, numberType, false);
        }

        public static MvcHtmlString Number(this HtmlHelper html, object value, NumberType numberType, bool removeendzero)
        {
            return Number(html, value, 2, removeendzero);
            //return Number(html, value, GetDecimalsFromat(html, numberType), removeendzero);
        }

        internal static string GetDecimalsFromat(HtmlHelper html, NumberType numberType)
        {
            ViewDataDictionary viewData = html.ViewContext.ViewData;
            int digit = -1;
            switch (numberType)
            {
                case NumberType.Number:
                    digit = Convert.ToInt32(viewData["loginInfo.sl_digit"] ?? 2);
                    break;
                case NumberType.Price:
                    digit = Convert.ToInt32(viewData["loginInfo.dj_digit"] ?? 2); 
                    break;
                case NumberType.Money:
                    digit = Convert.ToInt32(viewData["loginInfo.je_digit"] ?? 2);
                    break;
                case NumberType.Agio:
                    digit = Convert.ToInt32(viewData["loginInfo.zk_digit"] ?? 2);
                    break;
                case NumberType.TaxRate:
                    digit = Convert.ToInt32(viewData["loginInfo.slv_digit"] ?? 2);
                    break;
                default:
                    break;
            }
            return GetFromatString(digit);
        }

        internal static string GetFromatString(int digit)
        {
            if (digit < 0)
            {
                return null;
            }
            if (digit < 1)
            {
                return "0";
            }
            if (digit > 7)
            {
                digit = 7;
            }
            return ("{0:" + "0.".PadRight(digit + 2, '0') + "}");
        }

        public static MvcHtmlString Number(this HtmlHelper html, object value, string fromatString)
        {
            return Number(html, value, fromatString, false);
        }

        public static MvcHtmlString Number(this HtmlHelper html, object value, string fromatString, bool removeendzero)
        {
            return NumberHelper(html, value, fromatString, removeendzero);
        }

        public static MvcHtmlString Number(this HtmlHelper html, object value, int decimals)
        {
            return NumberHelper(html, value, decimals, false);
        }

        public static MvcHtmlString Number(this HtmlHelper html, object value, int decimals, bool removeendzero)
        {
            return NumberHelper(html, value, decimals, removeendzero);
        }

        internal static MvcHtmlString NumberHelper(HtmlHelper html, object value, string fromatString, bool removeendzero)
        {
            string stringValue;
            stringValue = value == null ? "" : (CyVerify.IsNumeric(stringValue = value.ToString().Trim()) ? String.Format(fromatString, Convert.ToDecimal(stringValue)) : stringValue);
            if (removeendzero)
            {
                stringValue = RemoveEndZero(stringValue);
            }
            return MvcHtmlString.Create(stringValue);
        }

        internal static MvcHtmlString NumberHelper(HtmlHelper html, object value, int decimals, bool removeendzero)
        {
            string stringValue;
            stringValue = value == null ? "" : (CyVerify.IsNumeric(stringValue = value.ToString().Trim()) ? Decimal.Round(Convert.ToDecimal(stringValue),decimals).ToString() : stringValue);
            if (removeendzero)
            {
                stringValue = RemoveEndZero(stringValue);
            }
            return MvcHtmlString.Create(stringValue);
        }

        internal static string RemoveEndZero(string stringValue)
        {
            while (stringValue.Contains("."))
            {
                if (stringValue.EndsWith("0"))
                {
                    stringValue = stringValue.Remove(stringValue.Length - 1);
                }
                if (stringValue.EndsWith("."))
                {
                    stringValue = stringValue.Remove(stringValue.Length - 1);
                }
                if (!stringValue.EndsWith("0"))
                {
                    break;
                }
            }

            return stringValue;
        }
    }

    public enum NumberType
    {
        /// <summary>
        /// 无类型
        /// </summary>
        Normal,
        /// <summary>
        /// 数量
        /// </summary>
        Number,
        /// <summary>
        /// 单价
        /// </summary>
        Price,
        /// <summary>
        /// 金额
        /// </summary>
        Money,
        /// <summary>
        /// 折扣
        /// </summary>
        Agio,
        /// <summary>
        /// 税率
        /// </summary>
        TaxRate
    }
}
