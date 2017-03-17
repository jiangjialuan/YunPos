using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Flags
{
    /// <summary>
    /// 发票类型
    /// </summary>
    [Flags]
    public enum InvoiceFlag
    {
        /// <summary>
        /// 不开发票
        /// </summary>
        None=1,

        /// <summary>
        /// 普通发票
        /// </summary>
        General=2,

        /// <summary>
        /// 增值税发票
        /// </summary>
        Vat=3
    }
}
