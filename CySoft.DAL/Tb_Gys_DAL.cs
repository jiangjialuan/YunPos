using System;
using System.Collections;
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.DAL.Base;
using CySoft.IDAL;
using CySoft.Model.Tb;

namespace CySoft.DAL
{
    public class Tb_Gys_DAL : BaseDAL, ITb_Gys_DAL
    {

        public Tb_Cgs QueryGysOfCgs(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForObject<Tb_Cgs>(String.Format("{0}.QueryGysOfCgs", GetMapName(type)), param);
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
