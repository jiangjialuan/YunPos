using System;

namespace CySoft.Model.Flags
{
    /// <summary>
    /// 帐号类型
    /// </summary>
    [Flags]
    public enum AccountFlag
    {
        /// <summary>
        /// 未知
        /// </summary>
        unknown = 0,
        /// <summary>
        /// 标准账户(账户名/手机号码/电子邮箱)
        /// </summary>
        standard = 1
    }
}
