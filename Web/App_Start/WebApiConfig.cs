using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web;
using System.IO;
using CySoft.Utility;

namespace Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static void Init(HttpConfiguration config)
        {
            //YZQ注释
            Register(config);

            //// 判断 存储图片的文件夹是否存在  
            //if (!Directory.Exists(ApplicationInfo.GoodsPath))//如果不存在就创建file文件夹
            //{
            //    Directory.CreateDirectory(ApplicationInfo.GoodsPath);
            //}
            //if (!Directory.Exists(ApplicationInfo.GoodsThumbPath))
            //{
            //    Directory.CreateDirectory(ApplicationInfo.GoodsThumbPath);
            //}
            //if (!Directory.Exists(ApplicationInfo.TempPath))
            //{
            //    Directory.CreateDirectory(ApplicationInfo.TempPath);
            //}
            //if (!Directory.Exists(ApplicationInfo.UserPath))
            //{
            //    Directory.CreateDirectory(ApplicationInfo.UserPath);
            //}
            //if (!Directory.Exists(ApplicationInfo.UserMePath))
            //{
            //    Directory.CreateDirectory(ApplicationInfo.UserMePath);
            //}
            //if (!Directory.Exists(ApplicationInfo.UserMasterPath))
            //{
            //    Directory.CreateDirectory(ApplicationInfo.UserMasterPath);
            //}

            //if (!Directory.Exists(ApplicationInfo.InfoPath))
            //{
            //    Directory.CreateDirectory(ApplicationInfo.InfoPath);
            //}
        }
    }
}
