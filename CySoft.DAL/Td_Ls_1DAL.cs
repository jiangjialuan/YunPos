using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.DAL.Base;
using CySoft.IDAL;
using CySoft.Model.Other;
using CySoft.Model.Td;
using CySoft.Model.Tz;

namespace CySoft.DAL
{
    public class Td_Ls_1DAL : BaseDAL, ITd_Ls_1DAL
    {

        /// <summary>
        /// 查询首页销售数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public object GetHomePageData(Hashtable param)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            try
            {
                param["lgrq"] = (param["lgrq"] is DateTime && (((DateTime)param["lgrq"]) > new DateTime(2000, 01, 01)) ? (DateTime)param["lgrq"] : new DateTime(2000, 01, 01)).ToString("yyyy-MM-dd");
                var bgrq = (param["bgrq"] is DateTime ? (DateTime)param["bgrq"] : new DateTime(2000, 01, 01)).ToString("yyyy-MM-dd");
                var date = DateTime.Now.ToString("yyyy-MM-dd");
                // if (lgrq == bgrq && bgrq == date)
                //{
                //    return dataMapper.QueryForObject(String.Format("{0}.home_page_data", GetMapName(typeof(Td_Ls_1))), param);
                //}
                return dataMapper.QueryForObject(String.Format("{0}.home_page_data1", GetMapName(typeof(Td_Ls_1))), param);
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
        /// 各支付方式金额统计
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<HomePagePay> QueryHomePagePays(Hashtable param)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            try
            {
                return dataMapper.QueryForList<HomePagePay>(String.Format("{0}.QueryHomePagePays", GetMapName(typeof(Td_Ls_1))), param).ToList();
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
        /// 查询店铺库存金
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public decimal QueryHomePageShopKcj(Hashtable param)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            try
            {
                var ht = dataMapper.QueryForObject<Hashtable>(String.Format("{0}.QueryHomePageShopKcj", GetMapName(typeof(Td_Ls_1))), param);
                if (ht == null)
                {
                    return 0;
                }
                decimal v = 0;
                decimal.TryParse(ht["value"] + "", out v);
                return v;
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
        /// 首页查询畅销商品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<HomePageCxspModel> QueryHomePageCxsp(Hashtable param)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            try
            {
                return dataMapper.QueryForList<HomePageCxspModel>(String.Format("{0}.QueryHomePageCxsp", GetMapName(typeof(Td_Ls_1))), param).ToList();
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




        public List<HomePageZxspModel> QueryHomePageZxsp(Hashtable param)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            try
            {
                return dataMapper.QueryForList<HomePageZxspModel>(String.Format("{0}.QueryHomePageZxsp", GetMapName(typeof(Td_Ls_1))), param).ToList();
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
