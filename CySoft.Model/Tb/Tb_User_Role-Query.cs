#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region 用户角色
#endregion

namespace CySoft.Model.Tb
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [Serializable]
    [DebuggerDisplay("id_role = {id_role}, isChecked = {isChecked}, name_role = {name_role}")]
    public class Tb_User_Role_Query
    {
        public string id_role { get; set; }
        public bool isChecked { get; set; }
        public string name_role { get; set; }
    }
}