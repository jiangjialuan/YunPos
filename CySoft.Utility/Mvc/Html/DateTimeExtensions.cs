using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CySoft.Utility.Mvc.Html
{
    public static class DateTimeExtensions
    {
        public static MvcHtmlString DateTime(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            if (value != null)
            {
                DateTime datetime = Convert.ToDateTime(value);
                if (datetime > new DateTime(1900, 1, 1))
                {
                    return htmlHelper.TextBox(name, datetime, "{0:yyyy-MM-dd HH:mm:ss}", htmlAttributes);
                }
            }
            return htmlHelper.TextBox(name, "", htmlAttributes);
        }

        public static MvcHtmlString DateTimeFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);
            if (metadata.Model != null)
            {
                DateTime datetime = Convert.ToDateTime(metadata.Model);
                if (datetime > new DateTime(1900, 1, 1))
                {
                    return htmlHelper.TextBox(name, datetime, "{0:yyyy-MM-dd HH:mm:ss}", htmlAttributes);
                }
            }
            return htmlHelper.TextBox(name, "", htmlAttributes);
        }

        public static MvcHtmlString Date(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            if (value != null)
            {
                DateTime datetime = Convert.ToDateTime(value);
                if (datetime > new DateTime(1900, 1, 1))
                {
                    return htmlHelper.TextBox(name, datetime, "{0:yyyy-MM-dd}", htmlAttributes);
                }
            }
            return htmlHelper.TextBox(name, "", htmlAttributes);
        }

        public static MvcHtmlString DateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);
            if (metadata.Model != null)
            {
                DateTime datetime = Convert.ToDateTime(metadata.Model);
                if (datetime > new DateTime(1900, 1, 1))
                {
                    return htmlHelper.TextBox(name, datetime, "{0:yyyy-MM-dd}", htmlAttributes);
                }
            }
            return htmlHelper.TextBox(name, "", htmlAttributes);
        }

        public static string DateTimeEncode(this HtmlHelper htmlHelper,object value)
        {
            if (value != null)
            {
                DateTime datetime = Convert.ToDateTime(value);
                if (datetime > new DateTime(1900, 1, 1))
                {
                    return htmlHelper.FormatValue(datetime, "{0:yyyy-MM-dd HH:mm:ss}");
                }
            }
            return htmlHelper.FormatValue("","");
        }
    }
}
