using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.DAL.Base;
using CySoft.IDAL;
using System.Collections;
using Apache.Ibatis.DataMapper.Exceptions;

namespace CySoft.DAL
{
    public class Ts_param_businessDAL : BaseDAL, ITs_param_businessDAL
    {
        public int Insert_yw(Type type, System.Collections.IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.Update(String.Format("{0}.Insert_yw", GetMapName(type)), param);
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
        public int Insert_SystemSet(Type type, System.Collections.IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.Update(String.Format("{0}.Insert_SystemSet", GetMapName(type)), param);
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
