using System.Web;

namespace CySoft.Utility
{
    public class ApplicationInfo
    {
        private static readonly string _PathRoot = HttpContext.Current.Server.MapPath("~");

        public static string PathRoot
        {
            get
            {
                return _PathRoot;
            }
        }

        public static string TempPath
        {
            get
            {
                return _PathRoot + "\\UpLoad\\Temp";
            }
        }
        public static string ImagesPath
        {
            get
            {
                return _PathRoot + "\\UpLoad\\MsgImages";
            }
        }
        public static string QualificationPath
        {
            get
            {
                return _PathRoot + "\\UpLoad\\Qualifications";
            }
        }
        public static string PayPath
        {
            get
            {
                return _PathRoot + "\\UpLoad\\Pay";
            }
        }
        public static string OrderZipPath
        {
            get
            {
                return _PathRoot + "\\UpLoad\\OrderZip";
            }
        }
        public static string UserPath
        {
            get
            {
                return _PathRoot + "\\UpLoad\\User";
            }
        }
        public static string UserMePath
        {
            get
            {
                return _PathRoot + "\\UpLoad\\User\\Me";
            }
        }
        public static string UserMasterPath
        {
            get
            {
                return _PathRoot + "\\UpLoad\\User\\Master";
            }
        }
        public static string UserMeUri
        {
            get
            {
                return "/UpLoad/User/Me";
            }
        }
        public static string UserMasterUri
        {
            get
            {
                return "/UpLoad/User/Master";
            }
        }
        public static string GoodsPath
        {
            get
            {
                return _PathRoot + "\\UpLoad\\Goods";
            }
        }
        public static string GoodsThumbPath
        {
            get
            {
                return _PathRoot + "\\UpLoad\\Goods\\thumb";
            }
        }
        public static string ConfigPath
        {
            get
            {
                return _PathRoot + "\\CYApp.config";
            }
        }

        public static string InfoPath
        {
            get
            {
                return _PathRoot + "\\UpLoad\\Info";
            }
        }

        /// <summary>
        /// 用于保存条码库图片本地地址
        /// </summary>
        public static string ShopSpPic
        {
            get
            {
                return _PathRoot + "\\UpLoad\\ShopSpPic";
            }
        }

    }
}
