using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Other
{
    public class SpzhcfModel
    {
        public string id_masteruser { get; set; }

        public string id_user { get; set; }
        /// <summary>
        /// 成品ID_SP
        /// </summary>
        public string id_sp_cp{get; set; }
        /// <summary>
        /// 表体数据
        /// </summary>
        public string jsonData { get; set; }
        /// <summary>
        /// 自动日结处理
        /// </summary>
        public int flag_rjauto { get; set; }
    }
}
