using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Ts;
using System.Collections;
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.DAL.Base;
using CySoft.IDAL;

namespace CySoft.DAL
{
    public class Ts_LogDAL : BaseDAL, ITs_LogDAL
    {
        public Ts_Log QueryItem(Type type, System.Collections.IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForObject<Ts_Log>(String.Format("{0}.QueryItem", GetMapName(type)), param);
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
