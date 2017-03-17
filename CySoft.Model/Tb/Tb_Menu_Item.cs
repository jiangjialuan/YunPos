using System;
using System.Diagnostics;
using CySoft.Model.Mapping;

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_menu_item", "Tb_Menu_Item")]
    [DebuggerDisplay("ID = {ID}, Name = {Name}")]
    public class Tb_Menu_Item
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 子菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Tab标题
        /// </summary>
        public string TabTitle { get; set; }

        /// <summary>
        /// Controller
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        public string ActionName { get; set; }

        public string Param { get; set; }
        /// <summary>
        /// 是否有备注 false：没有，true：有
        /// </summary>
        public bool HasRemark { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public string TagName { get; set; }

        public int sort_id { get; set; }
    }
}
