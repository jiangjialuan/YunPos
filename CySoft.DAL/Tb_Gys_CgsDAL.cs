using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.DAL.Base;
using CySoft.IDAL;
using CySoft.Model.Tb;

namespace CySoft.DAL
{
    public class Tb_Gys_CgsDAL : BaseDAL, ITb_Gys_CgsDAL
    {
        public IList<Tb_Gys_Cgs_Query> QueryPageOfBuyer(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<Tb_Gys_Cgs_Query>(String.Format("{0}.QueryPageOfBuyer", GetMapName(type)), param);
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

        public int QueryCountOfBuyer(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForObject<int>(String.Format("{0}.QueryCountOfBuyer", GetMapName(type)), param);
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

        public IList<Tb_Gys_Cgs_Query> QueryListOfBuyer(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<Tb_Gys_Cgs_Query>(String.Format("{0}.QueryListOfBuyer", GetMapName(type)), param);
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

        public IList<Tb_Gys_Cgs_Query> QueryListOfSupplier(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<Tb_Gys_Cgs_Query>(String.Format("{0}.QueryListOfSupplier", GetMapName(type)), param);
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

        public IList<Tb_Gys_Query> FindSupplier(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<Tb_Gys_Query>(String.Format("{0}.FindSupplier", GetMapName(type)), param);
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
