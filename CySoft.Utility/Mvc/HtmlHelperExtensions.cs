using System;
using System.Web.Mvc;

namespace CySoft.Utility.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static string CyEncode(this HtmlHelper html, object value)
        {
            return html.Encode(value).Replace(" ", "&nbsp;");
        }

        public static string CyEncode(this HtmlHelper html, string value)
        {
            return html.Encode(value).Replace(" ", "&nbsp;");
        }
    }
}
