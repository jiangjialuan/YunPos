using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.DAL.Base;
using CySoft.IDAL;
using CySoft.Model.Td;

namespace CySoft.DAL.Td
{
    public class Td_Kc_Kspd_2DAL : BaseDAL, ITd_Kc_Kspd_2DAL
    {

        public IList<Td_Kc_Kspd_2> QureyKspd2LeftJoinKspd1(Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            try
            {
                return dataMapper.QueryForList<Td_Kc_Kspd_2>(String.Format("{0}.QureyKspd2LeftJoinKspd1", GetMapName(typeof(Td_Kc_Kspd_2))), param).ToList();
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
