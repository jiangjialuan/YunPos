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
using CySoft.Utility;
using CySoft.Model.Tb;
using CySoft.Model.Flags;
using System.Web.Caching;

namespace CySoft.BLL.SystemBLL.SettingBLL
{
    public class ProcessFunBLL:BaseBLL,IProcessFunBLL
    {
        protected object lock1 = new object();

        /// <summary>
        /// 在缓存中读取业务流程 cxb 2015-7-15
        /// </summary>
        /// <param name="id_user_master"></param>
        /// <returns></returns>
        public BaseResult GetProcess(long id_user_master)
        {
            BaseResult br = new BaseResult();
            br.Data = GetProcessList(id_user_master);
            br.Success = true;
            return br;
        }

        public IList<Ts_Param_Business> GetProcessList(long id_user_master)
        {
            IList<Ts_Param_Business> processList = DataCache.Get<IList<Ts_Param_Business>>(id_user_master + "_process");
            if (processList == null)
            {
                lock (lock1)
                {
                    processList = DataCache.Get<IList<Ts_Param_Business>>(id_user_master + "_process");
                    if (processList == null)
                    {
                        Hashtable param = new Hashtable();
                        param.Add("id_user_master", id_user_master);

                        IList<Ts_Param_Business> processlist = DAL.QueryList<Ts_Param_Business>(typeof(Ts_Param_Business), param);
                        if (processlist != null)
                        {
                            DataCache.Add(id_user_master + "_process", processList, CacheItemPriority.High);
                        }
                    }
                }
            }

            return processList;
        }
    }
}
