using System;
using CySoft.Frame.Common;
using CySoft.Utility.Weixin.Domain;

namespace CySoft.Utility.Weixin.Api
{
    public class MenuApi
    {
        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="menuJson"></param>
        /// <returns></returns>
        public static string CreateMenus(string accessToken, string menuJson)
        {
            string url = string.Format(ApiUrl.MenuCreate, accessToken);
            string str_result = string.Empty;

            try
            {
                str_result = new WebUtils().DoPost(url, menuJson);
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
            }

            return str_result;
        }

        /// <summary>
        /// 查询自定义菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static string GetMenus(string accessToken)
        {
            string url = string.Format(ApiUrl.MenuGet, accessToken);
            string str_result = string.Empty;

            try
            {
                str_result = new WebUtils().DoGet(url, null);
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
            }

            return str_result;
        }

        /// <summary>
        /// 删除自定义菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static string DeleteMenus(string accessToken)
        {
            string url = string.Format(ApiUrl.MenuDelete, accessToken);
            string str_result = string.Empty;

            try
            {
                str_result = new WebUtils().DoGet(url, null);
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
            }

            return str_result;
        }
    }
}

