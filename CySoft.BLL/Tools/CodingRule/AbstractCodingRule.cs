using System;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.IDAL.Base;
using CySoft.Model.Mapping;
using Spring.Context.Support;

namespace CySoft.BLL.Tools.CodingRule
{
    public abstract class AbstractCodingRule : AbstractBaseBLL, ICodingRule
    {
        public AbstractCodingRule()
        {
            DAL = (IBaseDAL)ContextRegistry.GetContext().GetObject("BaseDAL");
        }

        protected static readonly Type tableAttributeType = typeof(TableAttribute);

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
        /// 获得 Table 名称
        /// </summary>
        #region protected string GetTableName(Type type)
        protected string GetTableName(Type type)
        {
            try
            {
                TableAttribute TableAttr = GetTableAttr(type);
                return TableAttr.Name;
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

        protected int GetCodeLength(string prefix, int length, int curlevel)
        {
            int num = prefix.Length;
            string str = length.ToString();
            for (int i = 0; i < curlevel; i++)
            {
                num += Convert.ToInt32(str.Substring(i, 1));
            }
            return num;
        }

        protected string GetFormat(string prefix, int codeLength)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(prefix);
            builder.Append("{0:");
            int num = codeLength - prefix.Length;
            for (int i = 0; i < num; i++)
            {
                builder.Append("0");
            }
            builder.Append("}");
            return builder.ToString();
        }

        public virtual BaseResult SetCoding(object entity, Type type = null, string setPropertyName = "bm")
        {
            throw new NotImplementedException();
        }
    }
}
