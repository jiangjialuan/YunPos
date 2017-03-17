#region Imports
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.DAL.Base;
using CySoft.IDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace CySoft.DAL
{
    public class Tz_Hy_Je_FlowDAL : BaseDAL, ITz_Hy_Je_FlowDAL
    {
        
        public int AddFlowWithExists(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForObject<int>(String.Format("{0}.AddFlowWithExists", GetMapName(type)), param);
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


        public int AddFlowForXFWithExists(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForObject<int>(String.Format("{0}.AddFlowForXFWithExists", GetMapName(type)), param);
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