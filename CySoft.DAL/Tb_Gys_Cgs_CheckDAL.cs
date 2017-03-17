using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.DAL.Base;
using CySoft.IDAL;
using CySoft.Model.Tb;

namespace CySoft.DAL
{
    public class Tb_Gys_Cgs_CheckDAL : BaseDAL, ITb_Gys_Cgs_CheckDAL
    {
        public IList<Tb_Gys_Cgs_Check_Query> QueryListOfBuyerAttention(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<Tb_Gys_Cgs_Check_Query>(String.Format("{0}.QueryListOfBuyerAttention", GetMapName(type)), param);
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

        public IList<Tb_Gys_Cgs_Check_Query> QueryPageOfSupplierAttention(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<Tb_Gys_Cgs_Check_Query>(String.Format("{0}.QueryPageOfSupplierAttention", GetMapName(type)), param);
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
