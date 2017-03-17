using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.DAL.Base;
using CySoft.IDAL;
using CySoft.Model.Tb;
using System.Collections;
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.Model.Td;

namespace CySoft.DAL
{
    public class Td_sale_payDAL : BaseDAL, ITd_sale_payDAL
    {
        public IList<Td_sale_pay_Query> QueryPageforView(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<Td_sale_pay_Query>(String.Format("{0}.QueryPageforView", GetMapName(type)), param);
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
