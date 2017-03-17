
#region Imports
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.DAL.Base;
using CySoft.IDAL;
using CySoft.Model.Tb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Other;

#endregion

namespace CySoft.DAL.Tb
{
    public class Tb_ShopspDAL : BaseDAL, ITb_ShopspDAL
    {
        public IList<ShopspList_Query> GetShopspList(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<ShopspList_Query>(String.Format("{0}.GetShopspList", GetMapName(type)), param);
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
        public IList<Tb_Shopsp_Query_For_Ps> GetShopspListForPs(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<Tb_Shopsp_Query_For_Ps>(String.Format("{0}.GetShopspListForPs", GetMapName(type)), param);
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


        public IList<SelectSpModel> GetPageList(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<SelectSpModel>(String.Format("{0}.GetPageList", GetMapName(type)), param);
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


        public IList<Tb_Shopsp_Query_For_Ps> GetPageListForPs(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<Tb_Shopsp_Query_For_Ps>(String.Format("{0}.QueryPageForPs", GetMapName(type)), param);
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

        public int GetMaxBarcodeInfo(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForObject<int>(String.Format("{0}.GetMaxBarcodeInfo", GetMapName(type)), param);
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



        public IList<ShopspList_Query> GetShopspDwList(Type type, IDictionary param, string database = null)
        {
            try
            {
                if (param == null)
                {
                    param = new Hashtable();
                }
                param["database"] = database;
                return dataMapper.QueryForList<ShopspList_Query>(String.Format("{0}.GetShopspDwList", GetMapName(type)), param);
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


        public int GetCountSel(Type type, IDictionary param = null, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            return dataMapper.QueryForObject<int>(String.Format("{0}.GetCountSel", GetMapName(type)), param);
        }


        public IList<TResult> QueryPageSel<TResult>(Type type, IDictionary param, string database = null)
        {
            if (param == null)
            {
                param = new Hashtable();
            }
            param["database"] = database;
            try
            {
                return dataMapper.QueryForList<TResult>(String.Format("{0}.QueryPageSel", GetMapName(type)), param);
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
