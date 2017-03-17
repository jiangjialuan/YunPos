#region Imports
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Caching;
using CySoft.IDAL;
using CySoft.IMapHelper;
using CySoft.Model.Mapping;
using CySoft.Model.Sys;
using CySoft.Utility;
using IBatisNet.DataMapper.Exceptions;
using Spring.Context.Support;
#endregion

namespace CySoft.MapHelper.Base
{
    public class BaseMapHelper : IBaseMapHelper
    {
        protected static readonly Type tableAttributeType = typeof(TableAttribute);

        #region protected IDALBase DAL
        private IDALBase _DAL;
        protected IDALBase DAL
        {
            get
            {
                if (_DAL == null)
                {
                    _DAL = (IDALBase)ContextRegistry.GetContext().GetObject("Dao");
                }
                return _DAL;
            }
        }
        #endregion

        public TResult GetNextKey<TResult>(Type type, IDictionary param)
        {
            return DAL.QueryForObject<TResult>(String.Format("{0}.GetNextKey", GetMapName(type)), param);
        }

        public int GetNextXh(Type type, IDictionary param)
        {
            return DAL.QueryForObject<int>(String.Format("{0}.GetNextXh", GetMapName(type)), param);
        }

        public IList<TResult> QueryList<TResult>(Type type, IDictionary param)
        {
            try
            {
                return DAL.QueryForList<TResult>(String.Format("{0}.QueryList", GetMapName(type)), param);
            }
            catch (DataMapperException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<TResult> QueryObjectList<TResult>(Type type, IDictionary param)
        {
            try
            {
                return DAL.QueryForList<TResult>(String.Format("{0}.QueryObjectList", GetMapName(type)), param);
            }
            catch (DataMapperException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IList QueryList(Type type, IDictionary param)
        {
            try
            {
                return DAL.QueryForList(String.Format("{0}.QueryList", GetMapName(type)), param);
            }
            catch (DataMapperException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<TResult> QueryPage<TResult>(Type type, IDictionary param)
        {
            try
            {
                return DAL.QueryForList<TResult>(String.Format("{0}.QueryPage", GetMapName(type)), param);
            }
            catch (DataMapperException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IList<TResult> QueryPage2<TResult>(Type type, IDictionary param)
        {
            try
            {
                return DAL.QueryForList<TResult>(String.Format("{0}.QueryPage2", GetMapName(type)), param);
            }
            catch (DataMapperException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IList QueryPage(Type type, IDictionary param)
        {
            try
            {
                return DAL.QueryForList(String.Format("{0}.QueryPage", GetMapName(type)), param);
            }
            catch (DataMapperException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TResult QueryItem<TResult>(Type type, IDictionary param)
        {
            try
            {
                return DAL.QueryForObject<TResult>(String.Format("{0}.QueryItem", GetMapName(type)), param);
            }
            catch (DataMapperException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object QueryItem(Type type, IDictionary param)
        {
            try
            {
                return DAL.QueryForObject(String.Format("{0}.QueryItem", GetMapName(type)), param);
            }
            catch (DataMapperException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetCount(Type type, IDictionary param)
        {
            return DAL.QueryForObject<int>(String.Format("{0}.GetCount", GetMapName(type)), param);
        }
        public int GetCount2(Type type, IDictionary param)
        {
            return DAL.QueryForObject<int>(String.Format("{0}.GetCount2", GetMapName(type)), param);
        }
        public int UpdatePart(Type type, IDictionary param)
        {
            try
            {
                return DAL.Update(String.Format("{0}.UpdatePart", GetMapName(type)), param);
            }
            catch (DataMapperException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete(Type type, IDictionary param)
        {
            try
            {
                return DAL.Delete(String.Format("{0}.Delete", GetMapName(type)), param);
            }
            catch (DataMapperException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<TResult> GetTree<TResult>(Type type, IDictionary param)
        {
            try
            {
                return DAL.QueryForList<TResult>(String.Format("{0}.GetTree", GetMapName(type)), param);
            }
            catch (DataMapperException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IList<TResult> GetTree2<TResult>(Type type, IDictionary param)
        {
            try
            {
                return DAL.QueryForList<TResult>(String.Format("{0}.GetTree2", GetMapName(type)), param);
            }
            catch (DataMapperException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TResult GetItem<TResult>(Type type, IDictionary param)
        {
            try
            {
                return DAL.QueryForObject<TResult>(String.Format("{0}.GetItem", GetMapName(type)), param);
            }
            catch (DataMapperException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual int Update<T>(Type type, T item)
        {
            Hashtable param = new Hashtable();
            //Type type = item.GetType();
            Table table = GetTable(type);

            IList setList = new ArrayList();
            var query = from column in table.columnList where !column.is_computed && !column.is_identity && !column.is_primary_key select column;
            foreach (Column column in query)
            {
                string value = GetColumnValue(column, item);
                setList.Add(String.Format("{0}={1}", column.name, value));
            }

            if (setList.Count < 1)
            {
                throw new ArgumentException(String.Format("表 {0} 没有可修改的列！", table.name));
            }

            IList queryList = new ArrayList();
            query = from column in table.columnList where column.is_primary_key select column;
            foreach (Column column in query)
            {
                string value = GetColumnValue(column, item);
                queryList.Add(String.Format("{0}={1}", column.name, value));
            }

            if (queryList.Count < 1)
            {
                throw new ArgumentException(String.Format("表 {0} 未设置主键！", table.name));
            }

            param.Add("tableName", table.name);
            param.Add("setList", setList);
            param.Add("queryList", queryList);
            return DAL.Update("DataTools.Update", param);
        }

        public virtual void Add<T>(Type type, T item)
        {
            Hashtable param = new Hashtable();
            //Type type = item.GetType();
            Table tableInfo = GetTable(type);
            IList<Column> columnInfos = tableInfo.columnList.Where(col => !col.is_identity && !col.is_computed).ToList();
            IList valueList = new ArrayList();
            foreach (Column columnInfo in columnInfos)
            {
                object value = type.GetProperty(columnInfo.name).GetValue(item, null);
                valueList.Add(value);
            }
            param.Add("tableName", tableInfo.name);
            param.Add("columns", columnInfos.Join(col => col.name, ","));
            param.Add("valueList", valueList);

            DAL.Insert("DataTools.Insert", param);
        }

        public virtual void AddRange<T>(IList<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            Type type = typeof(T);
            Table tableInfo = GetTable(type);
            IList<Column> columnList = tableInfo.columnList;
            string sqlFormat = "insert into " + tableInfo.name + "(" + tableInfo.columnList.Where(item => !item.is_identity && !item.is_computed).Join(item => item.name, ",") + ") values {0}";

            StringBuilder statement = new StringBuilder();
            StringBuilder values = new StringBuilder();
            int rowIndex = 0;
            foreach (var item in list)
            {
                foreach (Column column in columnList)
                {
                    string value = GetColumnValue(column, item);
                    values.Append("," + value);
                }
                statement.AppendFormat("({0}),", values.Remove(0, 1));

                values.Length = 0;
                rowIndex++;
                if (rowIndex % 300 == 0 || rowIndex == list.Count)
                {
                    statement.Length = statement.Length - 1;
                    string sql = String.Format(sqlFormat, statement);
                    DAL.Insert("DataTools.ExecuteSql", sql);

                    statement.Length = 0;
                }
            }
        }

        public virtual DateTime GetDbDateTime()
        {
            return DAL.QueryForObject<DateTime>("DataTools.GetDbDateTime", null);
        }

        public virtual string GetDataBaseName()
        {
            return DAL.QueryForObject<string>("DataTools.GetDataBaseName", null);
        }

        public virtual int GetBlockedCount(IDictionary param)
        {
            return DAL.QueryForObject<int>("DataTools.GetBlockedCount", param);
        }

        /// <summary>
        /// 获得表信息
        /// </summary>
        #region protected Table GetTable(Type type)
        private static object getTableLock = new object();
        protected Table GetTable(Type type)
        {
            Dictionary<Type, Table> dicTableInfo = DataCache.Get<Dictionary<Type, Table>>("TableCache");
            TableAttribute tableAttr = GetTableAttr(type);
            if (dicTableInfo != null && dicTableInfo.ContainsKey(type))
            {
                return dicTableInfo[type];
            }
            lock (getTableLock)
            {
                dicTableInfo = DataCache.Get<Dictionary<Type, Table>>("TableCache");
                if (dicTableInfo == null)
                {
                    dicTableInfo = new Dictionary<Type, Table>();
                }
                if (dicTableInfo.ContainsKey(type))
                {
                    return dicTableInfo[type];
                }

                Hashtable param = new Hashtable();
                param.Add("name", tableAttr.Name);
                Table table = DAL.QueryForObject<Table>("DataTools.GetTable", param);

                PropertyInfo[] properties = type.GetProperties();
                param.Clear();
                param.Add("table_id", table.id);
                param.Add("columnList", properties.ToList(item => item.Name));
                IList<Column> columnList = DAL.QueryForList<Column>("DataTools.GetColumns", param);

                foreach (Column column in columnList)
                {
                    foreach (PropertyInfo propertie in properties)
                    {
                        if (column.name.ToLower() == propertie.Name.ToLower())
                        {
                            column.property = propertie;
                        }
                    }
                }

                table.columnList = columnList;
                dicTableInfo.Add(type, table);
                DataCache.Add("TableCache", dicTableInfo, CacheItemPriority.NotRemovable);

                return table;
            }
        }
        #endregion

        /// <summary>
        /// 获得 SqlMap 名称
        /// </summary>
        #region protected string GetMapName(Type type)
        protected string GetMapName(Type type)
        {
            try
            {
                TableAttribute TableAttr = GetTableAttr(type);
                return TableAttr.MapName;
            }
            catch (TypeLoadException ex)
            {
                throw ex;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 获得Model的Table属性
        /// </summary>
        #region  protected TableAttribute GetTableAttr(Type type)
        private static object lock_TableAttribute = new object();
        protected TableAttribute GetTableAttr(Type type)
        {
            try
            {
                Dictionary<Type, TableAttribute> dic = DataCache.Get<Dictionary<Type, TableAttribute>>("TableAttributeCache");
                if (dic == null || !dic.ContainsKey(type) || dic[type] == null)
                {
                    lock (lock_TableAttribute)
                    {
                        dic = DataCache.Get<Dictionary<Type, TableAttribute>>("TableAttributeCache");
                        if (dic == null || !dic.ContainsKey(type) || dic[type] == null)
                        {
                            object[] attrs = type.GetCustomAttributes(tableAttributeType, true);
                            if (attrs.Length < 1)
                            {
                                throw new ArgumentException(String.Format("类 {0} 中未找到属性 {1}", type.FullName, tableAttributeType.FullName));
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
                            if (dic == null)
                            {
                                dic = new Dictionary<Type, TableAttribute>();
                            }
                            dic.Add(type, tableAttr);
                            DataCache.Add("TableAttributeCache", dic, CacheItemPriority.NotRemovable);
                        }
                    }
                }
                return dic[type];
            }
            catch (TypeLoadException ex)
            {
                throw ex;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 获得列值
        /// </summary>
        #region protected string GetColumnValue<T>(Column column, T item)
        protected string GetColumnValue<T>(Column column, T item)
        {
            string value;
            object objectValue = column.property.GetValue(item, null);
            if (objectValue != null && !Convert.IsDBNull(objectValue))
            {
                string dbType = column.dbType.ToString().ToLower();
                if (",bigint,timestamp,".Contains("," + dbType + ","))
                {
                    objectValue = (objectValue is long ? objectValue : Convert.ToInt64(objectValue));
                    value = String.Format("{0}", objectValue);
                }
                else if (",int,tinyint,smallint,".Contains("," + dbType + ","))
                {
                    objectValue = (objectValue is int ? objectValue : Convert.ToInt32(objectValue));
                    value = String.Format("{0}", objectValue);
                }
                else if (",decimal,numeric,".Contains("," + dbType + ","))
                {
                    objectValue = (objectValue is decimal ? objectValue : Convert.ToDecimal(objectValue));
                    value = String.Format("{0}", objectValue);
                }
                else if (",datetime,datetime2,smalldatetime,".Contains("," + dbType + ","))
                {
                    objectValue = (objectValue is DateTime ? objectValue : Convert.ToDateTime(objectValue));
                    value = String.Format("'{0:yyyy-MM-dd HH:mm:ss.fff}'", objectValue);
                }
                else if (",date,".Contains("," + dbType + ","))
                {
                    objectValue = (objectValue is DateTime ? objectValue : Convert.ToDateTime(objectValue));
                    value = String.Format("'{0:yyyy-MM-dd}'", objectValue);
                }
                else if (",char,varchar,text,".Contains("," + dbType + ","))
                {
                    value = String.Format("'{0}'", SQLFilter(objectValue));
                }
                else
                {
                    value = String.Format("N'{0}'", SQLFilter(objectValue));
                }
            }
            else
            {
                value = "null";
            }
            return value;
        }
        #endregion

        /// <summary>
        /// SQL注入过滤
        /// </summary>
        #region protected string SQLFilter(object input)
        protected string SQLFilter(object input)
        {
            string inputSQL = String.Empty;
            if (input == null || String.IsNullOrEmpty(inputSQL = input.ToString()) || String.IsNullOrEmpty(inputSQL.Trim()))
            {
                return inputSQL;
            }
            inputSQL = inputSQL.Replace("'", "''");
            return inputSQL;
        }
        #endregion
    }
}
