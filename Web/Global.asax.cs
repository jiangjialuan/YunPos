using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CySoft.Frame.Common;
using CySoft.Frame.Core;
using CySoft.Utility;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //YZQ_Add
            log4net.Config.XmlConfigurator.Configure();

            //CySoft.Controllers.Base.BusinessFactory.Config.InitArea();

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Init(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //int resultCode =  MSDPHelper.Init();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //MSDPHelper.End();
        }


        protected void Session_Start(object sender, EventArgs e)
        {
            //LogHelper.Info(string.Format("Session_Start"));
        }


        protected void Session_End(object sender, EventArgs e)
        {
            string sid = Session.SessionID;
            LogHelper.Info(string.Format("Session_End sid:{0}", sid));
        }




        /// <summary>
        /// 应用程序出错时执行
        /// </summary>
        #region protected void Application_Error(object sender, EventArgs e)
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception lastError = Server.GetLastError();

            if (lastError != null)
            {
                Exception ex = lastError.GetBaseException();
                Server.ClearError();
                //过滤浏览器请求图片所产生的错误
                if (ex is HttpException)
                {
                    string absolutePath = Request.Url.AbsolutePath.ToLower();
                    string ext = Path.HasExtension(absolutePath) ? Path.GetExtension(absolutePath) : "";
                    if (new string[] { ".ico", ".gif", ".jpg", ".png", ".bmp", ".cur", ".map" }.Contains(ext.ToLower()))
                    {
                        return;
                    }
                }
                if (ex is CySoftException)
                {
                    SystemError(ex);
                    return;
                }
                TextLogHelper.WriterExceptionLog(ex);
                SystemError(ex);
            }
        }
        #endregion

        /// <summary>
        /// 处理错误
        /// </summary>
        #region protected void SystemError(Exception ex)
        protected void SystemError(Exception ex)
        {
            bool isAjaxRequest = IsAjaxRequest();
            if (isAjaxRequest)
            {
                Response.Expires = -1;
                Response.CacheControl = "no-cache";
                Response.Charset = "utf-8";
                Response.ContentType = "application/json";
            }
            if (ex is HttpRequestValidationException)
            {
                if (isAjaxRequest)
                {
                    BaseResult br = new BaseResult();
                    br.Success = false;
                    br.Message.Add("操作失败！");
                    br.Message.Add("请求的内容中包含不安全的字符！（如：html代码，脚本语言)");
                    br.Level = ErrorLevel.Error;

                    Response.Write(JSON.Serialize(br));
                }
                else
                {
                        Response.Redirect("/Error/hazard");
                }
            }
            else if (ex is HttpCompileException)
            {
                if (isAjaxRequest)
                {
                    BaseResult br = new BaseResult();
                    br.Success = false;
                    br.Message.Add("系统错误！");
                    br.Message.Add("服务器发生错误，请联系管理员！");
                    br.Level = ErrorLevel.Error;

                    Response.Write(JSON.Serialize(br));
                }
                else
                {
                        Response.Redirect("/Error/500");
                }
            }
            else if (ex is SqlException)
            {
                if (isAjaxRequest)
                {
                    BaseResult br = new BaseResult();
                    br.Success = false;
                    br.Message.Add("系统错误！");
                    br.Message.Add("数据库服务器发生错误，请联系管理员！");
                    br.Level = ErrorLevel.Error;

                    Response.Write(JSON.Serialize(br));
                }
                else
                {
                        Response.Redirect("/Error/sql");
                }
            }
            else if (ex is ExternalException)
            {
                if (isAjaxRequest)
                {
                    BaseResult br = new BaseResult();
                    br.Success = false;
                    br.Message.Add("系统错误！");
                    br.Message.Add("请求的服务器地址不存在，请联系管理员！");
                    br.Level = ErrorLevel.Error;

                    Response.Write(JSON.Serialize(br));
                }
                else
                {
                   
                        Response.Redirect("/Error/404");
                }
            }
            else if (ex is CySoftException)
            {
                CySoftException cyex = (CySoftException)ex;
                if (isAjaxRequest)
                {
                    Response.Write(JSON.Serialize(cyex.Result));
                }
                else
                {
                    if (cyex.Message.Contains("您未登录或登录已超时"))
                        Response.Redirect("/Account/Login");
                    else
                        Response.Redirect("/Error/cy");
                }
            }
            else
            {
                if (isAjaxRequest)
                {
                    BaseResult br = new BaseResult();
                    br.Success = false;
                    br.Message.Add("系统错误！");
                    br.Message.Add("服务器发生错误，请联系管理员！");
                    br.Level = ErrorLevel.Error;
                    Response.Redirect("/Account/Login");
                    Response.Write(JSON.Serialize(br));
                }
                else
                {
                    Response.Redirect("/Error/500");
                }
            }
        }
        #endregion

        /// <summary>
        /// 判断是否Ajax请求
        /// </summary>
        #region protected bool IsAjaxRequest()
        protected bool IsAjaxRequest()
        {
            if (Request == null)
            {
                return false;
            }
            return ((Request["X-Requested-With"] == "XMLHttpRequest") || ((Request.Headers != null) && (Request.Headers["X-Requested-With"] == "XMLHttpRequest")));
        }
        #endregion
    }
}