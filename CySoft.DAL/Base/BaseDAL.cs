#region Imports
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Caching;
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.IDAL.Base;
using CySoft.Model.Base;
using CySoft.Model.Mapping;
using CySoft.Utility;
using System.Data;
using CySoft.Model.Other;
#endregion

namespace CySoft.DAL.Base
{
    public class BaseDAL : IBatisDAL, IBaseDAL
    {
        public BaseDAL()
        {

        }

        protected static readonly Type tableAttributeType = typeof(TableAttribute);
        protected static readonly Type columnAttributeType = typeof(ColumnAttribute);

        public TResult GetNextKey<TResult>(Type type, IDictionary param = null, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            return dataMapper.QueryForObject<TResult>(String.Format("{0}.GetNextKey", GetMapName(type)), param);
        }

        public int GetNextXh(Type type, IDictionary param = null, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            return dataMapper.QueryForObject<int>(String.Format("{0}.GetNextXh", GetMapName(type)), param);
        }

        public IList<TResult> QueryList<TResult>(Type type, IDictionary param = null, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<TResult>(String.Format("{0}.QueryList", GetMapName(type)), param);
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
        public IList<TResult> ExportAll<TResult>(Type type, IDictionary param = null, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<TResult>(String.Format("{0}.ExportSpAllList", GetMapName(type)), param);
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
        public IList QueryList(Type type, IDictionary param = null, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.QueryForList(String.Format("{0}.QueryList", GetMapName(type)), param);
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

        public IList<TResult> QueryPage<TResult>(Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.QueryForList<TResult>(String.Format("{0}.QueryPage", GetMapName(type)), param);
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

        public IList QueryPage(Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.QueryForList(String.Format("{0}.QueryPage", GetMapName(type)), param);
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

        public IList<TResult> QueryTree<TResult>(Type type, IDictionary param = null, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.QueryForList<TResult>(String.Format("{0}.QueryTree", GetMapName(type)), param);
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

        public TResult QueryItem<TResult>(Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.QueryForObject<TResult>(String.Format("{0}.QueryItem", GetMapName(type)), param);
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

        public object QueryItem(Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.QueryForObject(String.Format("{0}.QueryItem", GetMapName(type)), param);
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

        public int GetCount(Type type, IDictionary param = null, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            return dataMapper.QueryForObject<int>(String.Format("{0}.GetCount", GetMapName(type)), param);
        }

        public int QueryCount(Type type, IDictionary param = null, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            return dataMapper.QueryForObject<int>(String.Format("{0}.QueryCount", GetMapName(type)), param);
        }

        public int UpdatePart(Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.Update(String.Format("{0}.UpdatePart", GetMapName(type)), param);
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

        public int Save(Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.Update(String.Format("{0}.Save", GetMapName(type)), param);
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

        public int Delete(Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.Delete(String.Format("{0}.Delete", GetMapName(type)), param);
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

        public int Procedure(string procedureName, Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.Delete(String.Format("{0}.{1}", GetMapName(type), procedureName), param);
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

        public IList<TResult> GetTree<TResult>(Type type, IDictionary param = null, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.QueryForList<TResult>(String.Format("{0}.GetTree", GetMapName(type)), param);
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

        public TResult CopyItem<TResult>(Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.QueryForObject<TResult>(String.Format("{0}.CopyItem", GetMapName(type)), param);
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

        public IList<TResult> CopyList<TResult>(Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.QueryForList<TResult>(String.Format("{0}.CopyList", GetMapName(type)), param);
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

        public TResult GetItem<TResult>(Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.QueryForObject<TResult>(String.Format("{0}.GetItem", GetMapName(type)), param);
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

        public object GetItem(Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.QueryForObject(String.Format("{0}.GetItem", GetMapName(type)), param);
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

        public virtual int Update<T>(T item, string database = null)
        {
            Hashtable param = new Hashtable();
            Type type = item.GetType();
            Table table = GetTable(type, database);

            IList setList = new ArrayList();
            var query = from column in table.columnList where !column.is_computed && !column.is_identity && !column.is_primary_key && column.update select column;
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
            if (!String.IsNullOrEmpty(database))
            {
                param.Add("database", database);
            }
            param.Add("tableName", table.name);
            param.Add("setList", setList);
            param.Add("queryList", queryList);
            return dataMapper.Update("DataTools.Update", param);
        }

        public virtual void Check(Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            if (!String.IsNullOrEmpty(database))
            {
                param.Add("database", database);
            }
            try
            {
                dataMapper.Update(String.Format("{0}.Check", GetMapName(type)), param);
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

        public virtual void Add<T>(T item, string database = null)
        {
            Type type = typeof(T);
            Table tableInfo = GetTable(type, database);
            IList<Column> columnList = tableInfo.columnList.Where(m => !m.is_identity && !m.is_computed && m.insert && m.dbType != SqlDbType.Timestamp).ToList();
            if (columnList.Count > 0)
            {
                StringBuilder values = new StringBuilder();
                foreach (Column column in columnList)
                {
                    string value = GetColumnValue(column, item);
                    values.Append("," + value);
                }
                string sql = "insert into" + (String.IsNullOrEmpty(database) ? "" : " [" + database + "]..") + "[" + tableInfo.name + "](" + columnList.Join(m => m.name, ",") + ") values (" + values.Remove(0, 1) + ")";
                dataMapper.Insert("DataTools.ExecuteSql", sql);
            }
        }

        public virtual void ExecuteSql(string sql)
        {
            dataMapper.Insert("DataTools.ExecuteSql", sql);
        }
        public virtual decimal ExecuteSqlWithBack(Hashtable param)
        {
            return dataMapper.QueryForObject<decimal>("DataTools.ExecuteSqlWithBack", param);
        }


        public virtual decimal ExecuteFunctionWithName(Hashtable param)
        {
            return dataMapper.QueryForObject<decimal>("DataTools.ExecuteFunctionWithName", param);
        }

        public virtual void AddRange<T>(IList<T> list, string database = null)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            Type type = typeof(T);
            Table tableInfo = GetTable(type, database);
            IList<Column> columnList = tableInfo.columnList.Where(item => !item.is_identity && !item.is_computed && item.insert && item.dbType != SqlDbType.Timestamp).ToList();
            string sqlFormat = "insert into" + (String.IsNullOrEmpty(database) ? "" : " [" + database + "]..") + "[" + tableInfo.name + "](" + columnList.Join(item => item.name, ",") + ") values {0}";

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
                    dataMapper.Insert("DataTools.ExecuteSql", sql);

                    statement.Length = 0;
                }
            }
        }

        public IList<T> QueryListByStatementName<T>(Type type, System.Collections.IDictionary param, string statementName = "QueryList", string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<T>(String.Format("{0}.{1}", GetMapName(type), statementName), param);
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

        public DateTime GetDbDateTime()
        {
            return DateTime.Now;
            //return dataMapper.QueryForObject<DateTime>("DataTools.GetDbDateTime", null);
        }

        public int GetBlockedCount(string database)
        {
            return dataMapper.QueryForObject<int>("DataTools.GetBlockedCount", database);
        }

        /// <summary>
        /// 获得表信息
        /// </summary>
        #region protected Table  GetTable(Type type, string database = null)
        private static object getTableLock = new object();
        protected Table GetTable(Type type, string database = null)
        {
            string key = database + type.FullName;
            Dictionary<string, Table> dicTableInfo = DataCache.Get<Dictionary<string, Table>>("TableCache");
            TableAttribute tableAttr = GetTableAttr(type);
            Hashtable param = new Hashtable();
            param.Add("name", tableAttr.Name);
            if (!String.IsNullOrEmpty(database))
            {
                param.Add("database", database);
            }
            Table table = dataMapper.QueryForObject<Table>("DataTools.GetTable", param);
            if (dicTableInfo != null && dicTableInfo.ContainsKey(key))
            {
                Table cacheTable = dicTableInfo[key];
                if (cacheTable.modify_date == table.modify_date)
                {
                    return cacheTable;
                }
            }
            lock (getTableLock)
            {
                dicTableInfo = DataCache.Get<Dictionary<string, Table>>("TableCache");
                if (dicTableInfo == null)
                {
                    dicTableInfo = new Dictionary<string, Table>();
                }
                if (dicTableInfo.ContainsKey(key))
                {
                    Table cacheTable = dicTableInfo[key];
                    if (cacheTable.modify_date == table.modify_date)
                    {
                        return cacheTable;
                    }
                }

                PropertyInfo[] properties = type.GetProperties();
                param.Clear();
                param.Add("database", database);
                param.Add("table_id", table.id);
                param.Add("columnList", properties.ToList(item => item.Name));
                IList<Column> columnList = dataMapper.QueryForList<Column>("DataTools.GetColumns", param);

                foreach (Column column in columnList)
                {
                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        if (column.name.ToLower() == propertyInfo.Name.ToLower())
                        {
                            var columnAttr = GetColumnAttr(propertyInfo);
                            column.property = propertyInfo;
                            column.insert = columnAttr.Insert;
                            column.update = columnAttr.Update;
                        }
                    }
                }

                table.columnList = columnList;
                dicTableInfo[key] = table;
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
        /// 获得Model的Column属性
        /// </summary>
        #region protected ColumnAttribute GetColumnAttr(PropertyInfo propertyInfo)
        private static object lock_ColumnAttribute = new object();
        protected ColumnAttribute GetColumnAttr(PropertyInfo propertyInfo)
        {
            object[] attrs = propertyInfo.GetCustomAttributes(columnAttributeType, true);
            if (attrs.Length < 1)
            {
                attrs = new object[] { new ColumnAttribute() };
            }
            return (ColumnAttribute)attrs[0];
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

        public void RunProcedure(Hashtable param)
        {
            dataMapper.Insert("DataTools.RunProcedure", param);
        }
        public void RunProcedure2(Hashtable param)
        {
            dataMapper.Insert("DataTools.RunProcedure2", param);
        }

        public IList ProcedureQuery(Hashtable param)
        {
            var result = dataMapper.QueryForList("DataTools.ProcedureQuery", param);
            return result;
        }

        public IList ProcedureOutQuery(Hashtable param)
        {
            var result = dataMapper.QueryForList("DataTools.RunProcedureOutQuery", param);
            return result;

        }
    }
}
