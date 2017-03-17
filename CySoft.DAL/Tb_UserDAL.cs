using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.DAL.Base;
using CySoft.IDAL;
using CySoft.Model.Tb;

namespace CySoft.DAL
{
    public class Tb_UserDAL : BaseDAL, IUserDAL
    {
        public IList<Tb_User_Master> PageUserMaster(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<Tb_User_Master>(String.Format("{0}.PageUserMaster", GetMapName(type)), param);
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
