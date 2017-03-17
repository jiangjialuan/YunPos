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

namespace CySoft.DAL
{
    public class Td_Promote_2DAL : BaseDAL, ITd_Promote_2DAL
    {
        /// <summary>
        /// 关联商品分类表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Td_Promote_2_Query> QueryListWithSpfl(Hashtable param)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            try
            {
                return dataMapper.QueryForList<Td_Promote_2_Query>(String.Format("{0}.QueryListWithSpfl", GetMapName(typeof(Td_Promote_2))), param).ToList();
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
        /// <summary>
        /// 关联商品表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Td_Promote_2_Query> QueryListWithSp(Hashtable param)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            try
            {
                return dataMapper.QueryForList<Td_Promote_2_Query>(String.Format("{0}.QueryListWithSp", GetMapName(typeof(Td_Promote_2))), param).ToList();
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
