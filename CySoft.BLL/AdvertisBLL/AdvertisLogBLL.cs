using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Tb;
using CySoft.Frame.Attributes;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.IDAL;
using CySoft.Utility;

namespace CySoft.BLL.AdvertisBLL
{
    public class AdvertisLogBLL:BaseBLL
    {
        /// <summary>
        /// 添加广告内容
        /// wwc 2016-06-28
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            Tb_Advertis_Log Advertis_Log = new Tb_Advertis_Log();
            Advertis_Log.id_adv =long.Parse(param["id_adv"].ToString());
            Advertis_Log.id_user = int.Parse(param["id_user"].ToString());
            Advertis_Log.rq_create = DateTime.Now;
            DAL.Add(Advertis_Log);

            br.Success = true;
            return br;
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            IList<Tb_Advertis_Log_Query> lst = new List<Tb_Advertis_Log_Query>();
            pn.TotalCount = DAL.GetCount(typeof(Tb_Advertis_Log), param);
            if (pn.TotalCount > 0)
            {
                lst = DAL.QueryPage<Tb_Advertis_Log_Query>(typeof(Tb_Advertis_Log), param);
            }
            pn.Data = lst;
            return pn;
        }

        /// <summary>
        ///  查询广告详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            Tb_Advertis_Log_Query Advertis_Log_Query = new Tb_Advertis_Log_Query();

            Advertis_Log_Query = (Tb_Advertis_Log_Query)DAL.GetItem(typeof(Tb_Advertis_Log_Query), param) ?? new Tb_Advertis_Log_Query();
            br.Data = Advertis_Log_Query;

            br.Success = true;
            return br;
        }
    }
}
