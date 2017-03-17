using System;
using System.IO;

namespace CySoft.Utility
{
    public class LogHelper
    {
        #region 取配置

        //private static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");               //选择<logger name="logerror">的配置

        //private static readonly log4net.ILog logwarn = log4net.LogManager.GetLogger("logwarn");                 //选择<logger name="logwarn">的配置

        private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");                 //选择<logger name="loginfo">的配置

        //private static readonly log4net.ILog logdebug = log4net.LogManager.GetLogger("logdebug");               //选择<logger name="logdebug">的配置

        private static readonly log4net.ILog logweixin = log4net.LogManager.GetLogger("logweixin");             //选择<logger name="logwechat">的配置

        //private static readonly log4net.ILog logalipay = log4net.LogManager.GetLogger("logalipay");             //选择<logger name="logalipay">的配置

        #endregion 取配置

        public LogHelper()
        {
        }


        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void SetConfig(FileInfo configFile)
        {
            log4net.Config.XmlConfigurator.Configure(configFile);
        }

        ///// <summary>
        ///// 错误
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="se"></param>
        //public static void Error(string error)
        //{
        //    if (logerror.IsErrorEnabled)
        //        logerror.Error(error);
        //}
        ///// <summary>
        ///// 错误
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="se"></param>
        //public static void Error(Exception ex)
        //{
        //    if (logerror.IsErrorEnabled)
        //        logerror.Error(ex.Message, ex);
        //}
        ///// <summary>
        ///// 错误
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="se"></param>
        //public static void Error(string error, Exception ex)
        //{
        //    if (logerror.IsErrorEnabled)
        //        logerror.Error(error, ex);
        //}
        ///// <summary>
        ///// 警告
        ///// </summary>
        ///// <param name="warn"></param>
        //public static void Warn(string warn)
        //{
        //    if (logwarn.IsWarnEnabled)
        //        logwarn.Warn(warn);
        //}
        ///// <summary>
        ///// 警告
        ///// </summary>
        ///// <param name="warn"></param>
        //public static void Warn(string warn, Exception ex)
        //{
        //    if (logwarn.IsWarnEnabled)
        //        logwarn.Warn(warn, ex);
        //}
        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="info"></param>
        public static void Info(string info)
        {
            if (loginfo.IsInfoEnabled)
                loginfo.Info(info);
        }
        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void InfoFormat(string format, params object[] args)
        {
            if (loginfo.IsInfoEnabled)
                loginfo.Info(string.Format(format, args));
        }
        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="info"></param>
        public static void Info(string info, Exception ex)
        {
            if (loginfo.IsInfoEnabled)
                loginfo.Info(info, ex);
        }
        ///// <summary>
        ///// 调试
        ///// </summary>
        ///// <param name="debug"></param>
        //public static void Debug(string debug)
        //{
        //    if (logdebug.IsDebugEnabled)
        //        logdebug.Debug(debug);
        //}



        /// <summary>
        /// 微信日志
        /// </summary>
        /// <param name="info"></param>
        public static void WeixinInfo(string info)
        {
            if (logweixin.IsInfoEnabled)
                logweixin.Info(info);
        }

        ///// <summary>
        ///// 支付宝日志
        ///// </summary>
        ///// <param name="info"></param>
        //public static void AlipayInfo(string info)
        //{
        //    if (logalipay.IsInfoEnabled)
        //        logalipay.Info(info);
        //}
    }
}