using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Flags
{
    public enum RoleFlag
    {
        /// <summary>
        /// 系统管理员
        /// </summary>
        SysManager = 1,

        /// <summary>
        /// 平台管理员
        /// </summary>
        PlatformManager = 3,

        /// <summary>
        /// 模板角色
        /// </summary>
        Other = -1
    }
}
