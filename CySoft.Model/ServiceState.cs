namespace CySoft.Model
{  
    /// <summary>
    /// 云服务消息状态
    /// </summary>
    public static class ServiceState
    {
        /// <summary>
        /// 成功
        /// </summary>
        public static string Done = "Done";
        /// <summary>
        /// 失败
        /// </summary>
        public static string Fail = "Fail";
        /// <summary>
        /// 系统错误
        /// </summary>
        public static string Error = "Error";
        /// <summary>
        /// 未注册
        /// </summary>
        public static string Reg = "Reg";
        /// <summary>
        /// 过期
        /// </summary>
        public static string Exp = "Exp";
        /// <summary>
        /// 警告
        /// </summary>
        public static string Warn = "Warn"; 
        /// <summary>
        /// 欠费
        /// </summary>
        public static string Owe = "Owe";
    }
}
