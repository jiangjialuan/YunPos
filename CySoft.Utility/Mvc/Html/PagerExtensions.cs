using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace CySoft.Utility.Mvc.Html
{
    public static class PagerExtensions
    {
        public static MvcHtmlString Pager<T>(this HtmlHelper helper, PageList<T> pageList, string actionName, string controllerName, int numericPagerItemCount, object routeValues, object htmlAttributes)
        {
            return Pager<T>(helper, pageList, actionName, controllerName, numericPagerItemCount, false, routeValues, htmlAttributes);
        }

        public static MvcHtmlString Pager<T>(this HtmlHelper helper, PageList<T> pageList, string actionName, string controllerName, int numericPagerItemCount, bool autoHide, object routeValues, object htmlAttributes)
        {
            return Pager<T>(helper, pageList, actionName, controllerName, (string)null, numericPagerItemCount, false, routeValues, htmlAttributes);
        }

        public static MvcHtmlString Pager<T>(this HtmlHelper helper, PageList<T> pageList, string actionName, string controllerName, string routeName, int numericPagerItemCount, object routeValues, object htmlAttributes)
        {
            return Pager<T>(helper, pageList, actionName, controllerName, routeName, numericPagerItemCount, false, routeValues, htmlAttributes);
        }

        public static MvcHtmlString Pager<T>(this HtmlHelper helper, PageList<T> pageList, string actionName, string controllerName, string routeName, int numericPagerItemCount, bool autoHide, object routeValues, object htmlAttributes)
        {
            if (pageList == null)
            {
                pageList = new PageList<T>(20);
            }
            var mvcHtml = helper.Pager(pageList.ItemCount, pageList.PageSize, pageList.PageIndex, new PagerOptions()
            {
                Id = "mvcPager",
                ActionName = actionName,
                ControllerName = controllerName,
                RouteName = routeName,
                RouteValues = new System.Web.Routing.RouteValueDictionary(routeValues),
                HtmlAttributes = htmlAttributes as System.Collections.Generic.Dictionary<string, object>,
                GoToButtonId = "mvcGoToButtonId",
                PageIndexBoxId = "mvcPageIndexBoxId",
                PageIndexParameterName = "pageIndex",
                InvalidPageIndexErrorMessage = "索引值不是有效的数值类型！",
                PageIndexOutOfRangeErrorMessage = "页索引超出范围！",
                OnPageIndexError = @"cy.message([errMsg], { level: 4 });",
                ContainerTagName = "ul",
                PrevPageText = "上页",
                NextPageText = "下页",
                FirstPageText = "首页",
                LastPageText = "尾页",
                CssClass = "pager",
                CurrentPagerItemTemplate = "<li class=\"active\"><a href=\"javascript:;\">{0}</a></li>",
                PagerItemTemplate = "<li>{0}</li>",
                DisabledPagerItemTemplate = "<li><a disabled=\"disabled\" href=\"javascript:;\">{0}</a></li>",
                AutoHide = autoHide,
                NumericPagerItemCount = numericPagerItemCount,
            });

            helper.RegisterMvcPagerScriptResource();

            return new MvcHtmlString(
                "<div class=\"pagerbar\"><span class=\"pagerinfo\">总计"
                + pageList.PageCount + "页/"
                + pageList.ItemCount + "条记录</span>"
                + mvcHtml.ToHtmlString().Replace("<!--MvcPager v2.0 for ASP.NET MVC 4.0+ © 2009-2013 Webdiyer (http://www.webdiyer.com)-->", "").Replace("\r\n", "")
                + "<span class=\"pagergoto\">跳转到第<input id=\"mvcPageIndexBoxId\" value=\"" + pageList.PageIndex + "\"></input>页<button id=\"mvcGoToButtonId\">跳转</button></span>"
                + "</div>"
                + @"<script type='text/javascript'>
                    $(function () {
                        $('#mvcPageIndexBoxId').keydown(function(e){
                            if(e.keyCode==13){
                                $('#mvcGoToButtonId').click();
                            }
                        });
                    });
                    </script>");
        }
    }
}