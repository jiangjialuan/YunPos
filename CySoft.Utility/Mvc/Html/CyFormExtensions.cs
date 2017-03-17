using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CySoft.Utility.Mvc.Html
{
    public static class CyFormExtensions
    {
        public static MvcForm BeginForm(this HtmlHelper htmlHelper, FormMethod method, object htmlAttributes)
        {
            return FormExtensions.BeginForm(htmlHelper, (string)null, (string)null, method, htmlAttributes);
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, object routeValues, FormMethod method, object htmlAttributes)
        {
            return FormExtensions.BeginForm(htmlHelper, (string)null, (string)null, routeValues, method, htmlAttributes);
        }
    }
}
