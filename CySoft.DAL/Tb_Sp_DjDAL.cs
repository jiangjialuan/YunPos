using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.DAL.Base;
using CySoft.IDAL;
using CySoft.Model.Tb;
using System.Collections;
using Apache.Ibatis.DataMapper.Exceptions;

namespace CySoft.DAL
{
    public class Tb_Sp_DjDAL : BaseDAL, ITb_Sp_DjDAL
    {
        public IList<Tb_Sp_Dj_Query> QueryList1(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<Tb_Sp_Dj_Query>(String.Format("{0}.QueryList1", GetMapName(type)), param);
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
