using System;
using System.Collections.Generic;
using System.Diagnostics;
using CySoft.Model.Mapping;

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_menu", "Tb_Menu")]
    [DebuggerDisplay("ID = {ID}, Name = {Name}")]
    public class Tb_Menu
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }


        /// <summary>
        /// 子菜单列表
        /// </summary>
        public List<Tb_Menu_Item> Items { get; set; }


    }
}
