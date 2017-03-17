#region Imports
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.IBLL;
using CySoft.Model.Flags;
using CySoft.Model.Mapping;
using CySoft.Model.Ts;
using CySoft.Frame.Core;
using CySoft.BLL.Tools.CodingRule;
using CySoft.IDAL.Base;

#endregion

namespace CySoft.BLL.SystemBLL
{
    /// <summary>
    /// 公共业务
    /// </summary>
    public class UtiletyBLL : AbstractBaseBLL, IUtiletyBLL
    {
        protected static readonly Type tableAttributeType = typeof(TableAttribute);

        /// <summary>
        /// 获得下一个主键
        /// </summary>
        /// <param name="type">model类型</param>
        /// <returns>下一个主键</returns>
        [Transaction]
        public long GetNextKey(Type type)
        {
            var tableAttr = GetTableAttr(type);
            Hashtable param = new Hashtable();
            param.Add("table_name", tableAttr.Name);
            if (DAL.GetCount(typeof(Ts_Number_Max), param) > 0)
            {
                param.Add("add_id_max", 1);
                DAL.UpdatePart(typeof(Ts_Number_Max), param);
            }
            else
            {
                Ts_Number_Max newItem = new Ts_Number_Max();
                newItem.table_name = tableAttr.Name;
                newItem.id_max = 1;
                DAL.Add(newItem);
            }
            param.Clear();
            param.Add("table_name", tableAttr.Name);
            Ts_Number_Max model = DAL.GetItem<Ts_Number_Max>(typeof(Ts_Number_Max), param);
            return model.id_max.Value;
        }

        [Transaction]
        public long GetNextKey(Type type,IBaseDAL dal)
        {
            var tableAttr = GetTableAttr(type);
            Hashtable param = new Hashtable();
            param.Add("table_name", tableAttr.Name);
            if (dal.GetCount(typeof(Ts_Number_Max), param) > 0)
            {
                param.Add("add_id_max", 1);
                dal.UpdatePart(typeof(Ts_Number_Max), param);
            }
            else
            {
                Ts_Number_Max newItem = new Ts_Number_Max();
                newItem.table_name = tableAttr.Name;
                newItem.id_max = 1;
                dal.Add(newItem);
            }
            param.Clear();
            param.Add("table_name", tableAttr.Name);
            Ts_Number_Max model = dal.GetItem<Ts_Number_Max>(typeof(Ts_Number_Max), param);
            return model.id_max.Value;
        }
        /// <summary>
        /// 获得下一个序号
        /// </summary>
        /// <param name="type">model类型</param>
        /// <returns>下一个序号</returns>
        [Transaction]
        public long GetNextXh(Type type)
        {
            var tableAttr = GetTableAttr(type);
            Hashtable param = new Hashtable();
            param.Add("table_name", tableAttr.Name);
            if (DAL.GetCount(typeof(Ts_Number_Max), param) > 0)
            {
                param.Add("add_xh_max", 1);
                DAL.UpdatePart(typeof(Ts_Number_Max), param);
            }
            else
            {
                Ts_Number_Max newItem = new Ts_Number_Max();
                newItem.table_name = tableAttr.Name;
                newItem.xh_max = 1;
                DAL.Add(newItem);
            }
            param.Clear();
            param.Add("table_name", tableAttr.Name);
            Ts_Number_Max model = DAL.GetItem<Ts_Number_Max>(typeof(Ts_Number_Max), param);
            return model.xh_max.Value;
        }

        /// <summary>
        /// 获得Model的Table属性
        /// </summary>
        #region  protected TableAttribute GetTableAttr(Type type)
        private static object lock_TableAttribute = new object();
        protected TableAttribute GetTableAttr(Type type)
        {
            object[] attrs = type.GetCustomAttributes(tableAttributeType, true);
            if (attrs.Length < 1)
            {
                throw new ArgumentException(String.Format("类 {0} 中未找到属性(Attribute) {1}", type.FullName, tableAttributeType.FullName));
            }
            TableAttribute tableAttr = (TableAttribute)attrs[0];
            if (tableAttr.Name == null || String.IsNullOrEmpty(tableAttr.Name.Trim()))
            {
                throw new ArgumentException(String.Format("类 {0} 中属性 {1} 的 Name 值有误", type.FullName, tableAttributeType.FullName));
            }
            if (tableAttr.MapName == null || String.IsNullOrEmpty(tableAttr.MapName.Trim()))
            {
                throw new ArgumentException(String.Format("类 {0} 中属性 {1} 的 MapName 值有误", type.FullName, tableAttributeType.FullName));
            }
            return tableAttr;
        }
        #endregion

        /// <summary>
        /// 获得下一个单号
        /// lxt
        /// 2015-03-19
        /// </summary>
        [Transaction]
        public BaseResult GetNextDH(object entity, Type type = null)
        {
            BaseResult br = new BaseResult();
            br = CodingRule.SetBaseCoding(entity, type);
            return br;
        }
    }
}