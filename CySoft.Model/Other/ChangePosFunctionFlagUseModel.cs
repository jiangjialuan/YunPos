using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Other
{
    public class ChangePosFunctionFlagUseModel
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string id_role { get; set; }
        /// <summary>
        /// 使用/停用
        /// </summary>
        public byte flag_use { get; set; }
    }
}
