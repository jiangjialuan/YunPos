using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IDAL;
using CySoft.DAL.Base;
using System.Collections;
using Apache.Ibatis.DataMapper.Exceptions;

namespace CySoft.DAL
{
    public class Info_UserDAL:BaseDAL,IInfo_UserDAL
    {
        /// <summary>
        /// 批量插入接收公告信息的用户人群
        /// 2015-6-24 wzp
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public int BatchInsert_User(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForObject<int>(String.Format("{0}.BatchInsert_User", GetMapName(type)), param);
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
        public int QueryCountOfGG(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForObject<int>(String.Format("{0}.QueryCountOfGG", GetMapName(type)), param);
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
