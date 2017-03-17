using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Other
{
    public class KspdModel
    {
        public string id { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public string dh { get; set; }
        /// <summary>
        /// 出库门店ID
        /// </summary>
        public string id_shop { get; set; }
        /// <summary>
        /// 经办人ID
        /// </summary>
        public string id_jbr { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string bz { get; set; }
        /// <summary>
        /// JSON Data
        /// </summary>
        public string json_data { get; set; }

        /// <summary>
        /// 主用户ID
        /// </summary>
        public string id_masteruser { get; set; }
        /// <summary>
        /// 创建用户ID
        /// </summary>
        public string id_create { get; set; }
        /// <summary>
        /// 编辑用户ID
        /// </summary>
        public string id_edit { get; set; }
        /// <summary>
        /// 盈亏明细
        /// </summary>
        public decimal je_yk_mxtotal { get; set; }
        /// <summary>
        /// 开单日期
        /// </summary>
        public DateTime rq { get; set; }

        public bool AutoAudit { get; set; }
    }
}
