using System;

namespace CySoft.Model.Flags
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogFlag
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown,
        /// <summary>
        /// 登录
        /// </summary>
        LogOn,
        /// <summary>
        /// 退出/注销
        /// </summary>
        LogOff,
        /// <summary>
        /// 基础资料
        /// </summary>
        Base,
        /// <summary>
        /// 业务数据
        /// </summary>
        Bill
    }
}
