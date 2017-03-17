using System;

namespace CySoft.Model.Flags
{
    /// <summary>
    /// 收费类型
    /// </summary>
    [Flags]
    public enum ChargeFlag
    {
        /// <summary>
        /// 试用
        /// </summary>
        test,
        /// <summary>
        /// 付费
        /// </summary>
        pay,
        /// <summary>
        /// 免费
        /// </summary>
        free,
        /// <summary>
        /// 永续
        /// </summary>
        keep
    }
}
