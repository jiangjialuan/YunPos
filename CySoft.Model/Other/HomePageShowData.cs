using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Other
{
    public class HomePageShowData
    {
        /// <summary>
        /// 销售额
        /// </summary>
        public decimal xse { get; set; }
        /// <summary>
        /// 销售笔数
        /// </summary>
        public decimal xsbs { get; set; }
        /// <summary>
        /// 销售毛利
        /// </summary>
        public decimal xsml { get; set; }
        /// <summary>
        /// 退货笔数
        /// </summary>
        public decimal thbs { get; set; }
        /// <summary>
        /// 退货金额
        /// </summary>
        public decimal thje { get; set; }
    }
}
