using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IDAL;
using CySoft.DAL.Base;
using CySoft.Model.Tb;
using Apache.Ibatis.DataMapper.Exceptions;
using System.Collections;
namespace CySoft.DAL
{
    public class FaqDAL : BaseDAL,IFaqDAL
    {
        /// <summary>
        /// 反馈管理：接口分页
        /// wzp 2015-6-30
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public IList<Faq_Tree> QueryServicePage(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<Faq_Tree>(String.Format("{0}.QueryServicePage", GetMapName(type)), param);
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
    }
}
