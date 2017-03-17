using System;
using System.Web;

namespace CySoft.Utility
{
    public class CookiesHelp
    {
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null
                && HttpContext.Current.Request.Cookies[strName] != null
                && HttpContext.Current.Request.Cookies[strName].Value != null)
            {
                return HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[strName].Value.ToString());
            }

            return string.Empty;
        }

        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Expires = DateTime.Now.AddDays(7);
            cookie.Value = !string.IsNullOrWhiteSpace(strValue) ? HttpUtility.UrlEncode(strValue) : string.Empty;
            HttpContext.Current.Response.AppendCookie(cookie);

        }
        public static void Clerar(string strName)
        {
            HttpContext.Current.Response.Cookies[strName].Value = null;
            HttpContext.Current.Response.Cookies[strName].Expires = new DateTime(1970, 1, 1);
            HttpContext.Current.Response.Cookies[strName].Path = "/";
        }
    }
}
