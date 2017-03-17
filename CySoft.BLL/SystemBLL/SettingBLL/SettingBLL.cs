using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Ts;
using System.Collections;
using CySoft.IBLL;
using CySoft.Frame.Attributes;
using CySoft.IDAL;
using CySoft.Model.Td;
using CySoft.Utility;

namespace CySoft.BLL.SystemBLL.SettingBLL
{
    public class SettingBLL : BaseBLL
    {
        private readonly static Type _type = typeof(Ts_Param_Business);
        private readonly static string key_caceh = "UserMasterParams_{0}";
        protected object lock1 = new object();
        /// <summary>
        /// 获取所有用户参数
        /// tim 
        /// 2015-7-23
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetAll(Hashtable param = null)
        {
            //IList<Ts_Param_Business> list;
            //if (param.ContainsKey("id_user_master") && !param["id_user_master"].ToString().IsEmpty())
            //{
            //    var id_user_master = param["id_user_master"].ToString();                ;
            //    list = DataCache.Get<List<Ts_Param_Business>>(key_caceh.toFormat(id_user_master));
            //    if (list == null || list.Count < 1)
            //    {
            //        lock (lock1)
            //        {
            //            list = DataCache.Get<List<Ts_Param_Business>>(key_caceh.toFormat(id_user_master));
            //            if (list == null || list.Count < 1)
            //            {
            //                var ht = new Hashtable();
            //                ht.Add("id_user_master", id_user_master);
            //                list = DAL.QueryList<Ts_Param_Business>(_type, ht);
            //                if (list != null && list.Count > 0) DataCache.Add(key_caceh.toFormat(id_user_master), list, DateTime.Now.AddDays(1));
            //            }
            //        }
            //    }
            //}else
            //    list = DAL.QueryList<Ts_Param_Business>(_type, param);

            var list = DAL.QueryList<Ts_Param_Business>(_type, param);

            BaseResult br = new BaseResult() { Data = list, Success = true };
            return br;
        }

        /// <summary>
        /// 初始化数据
        /// tim
        /// 2015-7-23
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Init(Hashtable param)
        {            
            if (param.ContainsKey("id_user_master") && !param["id_user_master"].ToString().IsEmpty())
            {
                var id_user_master = param["id_user_master"].ToString();
                param.Remove("id_user_master");
                DataCache.Remove(key_caceh.toFormat(id_user_master));
            }
            BaseResult br = new BaseResult() { Data = DAL.Procedure("Data_reset", _type, param), Success = true };
            return br;
        }
        /// <summary>
        /// 保存参数
        /// tim
        /// 2015-7-23
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Save(dynamic entity)
        {
            BaseResult br = new BaseResult();
            var param_list = entity as List<Ts_Param_Business>;
            int id_master = 0;
            string[] bmlist = new string[param_list.Count];
            for (var i = 0; i < param_list.Count;i++ )
            {
                id_master = (int)param_list[i].id_user_master;
                bmlist[i] = param_list[i].bm;
            }
          
            var ht = new Hashtable();
            ht.Add("id_user_master",id_master);
            ht.Add("bmList", bmlist);
            var list = DAL.QueryList(_type,ht);
            DAL.Delete(_type, ht);

            DAL.AddRange<Ts_Param_Business>(param_list);

            DataCache.Remove(key_caceh.toFormat(id_master));

            br.Success = true;
            return br;
        }

        public override BaseResult GetCount(Hashtable param = null)
        {
            BaseResult br = new BaseResult() { Data = DAL.GetCount(_type, param), Success = true };
            return br;
        }
    }
}
