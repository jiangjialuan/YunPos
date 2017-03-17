using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.DAL.Base;
using CySoft.IDAL;
using System.Collections;
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.Model.Td;

namespace CySoft.DAL
{
    public class Td_Sale_Order_BodyDAL : BaseDAL, ITd_Sale_Order_BodyDAL
    {
        public IList<Td_Sale_Order_Body_Query> QueryStatisticsPage(Type type, System.Collections.IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<Td_Sale_Order_Body_Query>(String.Format("{0}.QueryStatisticsPage", GetMapName(type)), param);
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

        public Td_Report QueryStatisticsInfo(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForObject<Td_Report>(String.Format("{0}.QueryStatisticsInfo", GetMapName(type)), param);
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
        public int QueryStatisticsCount(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForObject<int>(String.Format("{0}.QueryStatisticsCount", GetMapName(type)), param);
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

        public int Data_reset(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.Update(String.Format("{0}.Data_reset", GetMapName(type)), param);
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
