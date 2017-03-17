using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Utility
{
    public class PublicSign
    {
        public static readonly string sign = "cysoft_2016";
        //public static readonly string charset = System.Configuration.ConfigurationManager.AppSettings["Charset"];
        public static readonly string charset = System.Configuration.ConfigurationManager.AppSettings["Charset"];
        public static readonly string localKey = System.Configuration.ConfigurationManager.AppSettings["LocalKey"];
        public static readonly string wxPayUrl = System.Configuration.ConfigurationManager.AppSettings["WXMicropayUrl"];
        public static readonly string wxQueryUrl = System.Configuration.ConfigurationManager.AppSettings["WXOrderQueryUrl"];
        public static readonly string aliPayUrl = System.Configuration.ConfigurationManager.AppSettings["AliMicropayUrl"];
        public static readonly string aliQueryUrl = System.Configuration.ConfigurationManager.AppSettings["AliQueryUrl"];
        public static readonly object lock_msg = new object();//用于锁 主用户注册时 发送手机短信验证码
        public static readonly string dh_cz = "33";//电子秤码 单号开头
        public static readonly string show_shop_version = ",20,30,";//显示门店下拉的版本


        public static readonly string cyGetServiceUrl = System.Configuration.ConfigurationManager.AppSettings["CyGetServiceUrl"];
        public static readonly string cyBuyServiceUrl = System.Configuration.ConfigurationManager.AppSettings["CyBuyServiceUrl"];
        public static readonly string CyBuyServiceUrlAll = System.Configuration.ConfigurationManager.AppSettings["CyBuyServiceUrlAll"];
        
        public static readonly string cyBuyServiceHistoryUrl = System.Configuration.ConfigurationManager.AppSettings["CyBuyServiceHistoryUrl"];
        public static readonly string cyLoginOutUrl = System.Configuration.ConfigurationManager.AppSettings["CyLoginOutUrl"];


        public static readonly string md5KeyBusiness = System.Configuration.ConfigurationManager.AppSettings["Md5KeyBusiness"];

        public static readonly string flagCheckService = "1";
        public static readonly int tryDays = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TryDays"]);


        public static readonly string bm_yunpos_dd = System.Configuration.ConfigurationManager.AppSettings["BM_YUNPOS_DD"];
        public static readonly string bm_yunpos_ls = System.Configuration.ConfigurationManager.AppSettings["BM_YUNPOS_LS"];
        public static readonly string bm_yunpos_jt = System.Configuration.ConfigurationManager.AppSettings["BM_YUNPOS_JT"];

        public static readonly int showMsgServiceDay = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MsgDays"]);
        public static readonly string doNotCheckAPI = System.Configuration.ConfigurationManager.AppSettings["DoNotCheckAPI"];
        public static readonly string downUrl = System.Configuration.ConfigurationManager.AppSettings["DownUrl"];


        public static readonly string shopspCheckAPI = System.Configuration.ConfigurationManager.AppSettings["SHOPSP_CHECK"];//是否去条码库获取商品
        public static readonly string shopspUrl = System.Configuration.ConfigurationManager.AppSettings["SHOPSP_URL"];
        public static readonly string shopspMD5Key = System.Configuration.ConfigurationManager.AppSettings["SHOPSP_Md5KEY"];






    }
}
