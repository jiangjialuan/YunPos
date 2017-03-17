using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        /// <summary>
        /// 路由映射
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //异常
            routes.MapRoute(
                name: "Error",
                url: "Error/{id}",
                defaults: new { controller = "Error", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CySoft.Controllers.OtherCtl" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Manager", action = "Home" },
                //defaults: new { controller = "Account", action = "Login" },
                constraints: new { controller = @"\w+", action = @"\w+" },
                namespaces: new[] { "CySoft.Controllers.ManagerCtl" }
                //namespaces: new[] { "CySoft.Controllers.AccountCtl" }
            );

            routes.MapRoute(
                name: "Login",
                url: "{controller}/{action}",
                defaults: new { controller = "Account", action = "Login" },
                constraints: new { controller = @"\w+", action = @"\w+" },
                namespaces: new[] { "CySoft.Controllers.AccountCtl" }
            );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}",
            //    defaults: new { controller = "manager", action = "index" },
            //    constraints: new { controller = @"\w+", action = @"\w+" },
            //    namespaces: new[] { "CySoft.Controllers.ManagerCtl" }
            //);



            #region 默认路由

            ////已由WxLogin覆盖
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
            //    constraints: new { controller = @"\w+", action = @"\w+" }
            //);

            #endregion 默认路由
        }
    }
}