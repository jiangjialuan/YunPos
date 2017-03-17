using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CySoft.Model.Base
{
    [Serializable]
    [DebuggerDisplay("name = {name}, create_date = {create_date}, modify_date = {modify_date}, column_count = {columnList.Count}")]
    public sealed class Table
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime create_date { get; set; }
        /// <summary>
        /// 最后修改日期
        /// </summary>
        public DateTime modify_date { get; set; }
        /// <summary>
        /// 列信息
        /// </summary>
        public IList<Column> columnList { get; set; }
    }
}
