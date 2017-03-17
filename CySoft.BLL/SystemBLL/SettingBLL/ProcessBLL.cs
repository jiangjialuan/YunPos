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

namespace CySoft.BLL.SystemBLL.SettingBLL
{
    public class ProcessBLL : BaseBLL,IProcessBLL
    {
        public ITs_param_businessDAL Ts_param_businessDAL { get; set; }
        /// <summary>
        /// 获取业务流程的对象 cxb 2015-6-9 13:49:07
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetAll(System.Collections.Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Ts_Param_Business>(typeof(Ts_Param_Business), param);
            br.Success = true;
            return br;
        }
        /// <summary> 
        /// 获取在业务流程中的个数 cxb 2015-6-25
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetCount(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.GetCount(typeof(Ts_Param_Business), param);
            br.Success=true;
            return br;
        }
        /// <summary>
        /// 新增订单业务流程 cxb 2015-6-25
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult();
            br.Data = Ts_param_businessDAL.Insert_yw(typeof(Ts_Param_Business), param);
            //DataCache.Remove(param["id_user_master"] + "_process");
            br.Success = true;
            br.Message.Add(String.Format("新增订单业务流程。信息：用户主ID:{0}", param["id_user_master"]));
            return br;
        }
        /// <summary>
        /// 修改业务流程 cxb 2015-6-16
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            Hashtable param0 = (Hashtable)entity;

            Ts_Param_Business model = (Ts_Param_Business)param0["model"];
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            param.Clear();
            string[] Process_List=param0["Process_List"].ToString().Split(',');
            string[] bm_list = { "order_ywlc_cwsh", "order_ywlc_cksh", "order_ywlc_fhqr", "order_ywlc_shqr" };
            for (int i = 0; i < Process_List.Length; i++)
            {
                param["bm"] = bm_list[i];
                param["new_val"] = Process_List[i];
                param["id_user_master"] = model.id_user_master;
                br.Data = DAL.UpdatePart(typeof(Ts_Param_Business), param);
            }
            DataCache.Remove(param["id_user_master"] + "_process");
            br.Success = true;
            br.Message.Add(String.Format("更新业务流程。信息：用户主ID:{0}，流程:{1}", model.id_user_master, model.bm));
            return br;
        }
        /// <summary>
        /// 比较是否用户删减某个流程 cxb 2015-6-16
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseResult CompareProcess(Hashtable param) {
            BaseResult br = new BaseResult();
            int flag =-1;
            List<Ts_Param_Business> list = (List<Ts_Param_Business>)DAL.QueryList<Ts_Param_Business>(typeof(Ts_Param_Business), param);
            if (list.Count > 0)
            {
                string[] Process_List = param["Process_List"].ToString().Split(',');
                for (int i = 0; i < Process_List.Length; i++)
                {
                    if (int.Parse(list[i].val) != int.Parse(Process_List[i]))
                    {
                        if (int.Parse(list[i].val) > 0)
                        {
                            if (int.Parse(Process_List[i]) == 0 && i != 3)
                            {
                                flag = 0;
                            }
                        }
                    }
                    else if (flag != 0)
                    {
                        flag = 1;
                    }
                }
                if (flag == null)
                {
                    flag = 2;
                }
            }
            else {
                flag = 0;
            }
            br.Data = flag;
            br.Success = true;
            return br;
        }
    }
}
