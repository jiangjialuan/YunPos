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
    [DebuggerDisplay("id_function = {id_function}, isChecked = {isChecked}, name_function = {name_function}")]
    public class Tb_User_Function
    {
        public long id_function { get; set; }
        public int id_module { get; set; }
        public int id_module_fatherid { get; set; }
        public bool isChecked { get; set; }
        public string name { get; set; }
    }
}