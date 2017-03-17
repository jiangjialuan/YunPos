using System;
using System.Collections.Generic;
using System.Linq;
using CySoft.Frame.Attributes;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using CySoft.Model.Flags;
using System.Collections;
using CySoft.IDAL;
using CySoft.IBLL;

namespace CySoft.BLL.SupplierBLL
{
    public class BuyerAttentionBLL : BaseBLL,IBuyerAttentionBLL
    {
        protected ITb_Gys_Cgs_CheckDAL Tb_Gys_Cgs_CheckDAL { get; set; }
        public ITb_Gys_CgsDAL Tb_Gys_CgsDAL { get; set; }
        /// <summary>
        /// 采购商获取关注列表
        /// cxb
        /// 2015-3-27
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data=Tb_Gys_Cgs_CheckDAL.QueryListOfBuyerAttention(typeof(Tb_Gys_Cgs_Check), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 采购商获取已关注列表
        /// cxb
        /// 2015-4-16
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data = Tb_Gys_CgsDAL.QueryListOfBuyer(typeof(Tb_Gys_Cgs), param);
            //br.Data = DAL.QueryList<Tb_Gys_Cgs_Query>(typeof(Tb_Gys_Cgs), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 供应商获取关注列表
        /// cxb
        /// 2015-4-14
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PageNavigate GetNorevPage(Hashtable param)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.GetCount(typeof(Tb_Gys_Cgs_Check), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = Tb_Gys_Cgs_CheckDAL.QueryPageOfSupplierAttention(typeof(Tb_Gys_Cgs_Check), param);
            }
            else
            {
                pn.Data = new List<Tb_Gys_Cgs_Check_Query>();
            }
            pn.Success = true;
            return pn;
        }

        /// <summary>
        /// 申请关注
        /// tim
        /// 2015-05-21
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Tb_Gys_Cgs_Check model = (Tb_Gys_Cgs_Check)entity;
            if (model == null || !model.id_cgs.HasValue || !(model.id_cgs > 0) || !model.id_gys.HasValue || !(model.id_gys > 0))
            {
                br.Success = false;
                br.Message.Add("关注参数错误.");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            Hashtable ht = new Hashtable();
            ht.Add("id_gys", model.id_gys);
            ht.Add("id_cgs", model.id_cgs);
            var gcgxcheck = DAL.GetItem<Tb_Gys_Cgs_Check>(typeof(Tb_Gys_Cgs_Check), ht);
            if (gcgxcheck != null&&gcgxcheck.flag_state.Equals(Gys_Cgs_Status.Apply))
            {
                br.Success = false;
                br.Message.Add("已经提交了关注申请，请等待供应商的审核通过.");
                br.Level = ErrorLevel.Warning;
                br.Data = Gys_Cgs_Status.Apply;
                return br;
            }

            var gcgx = DAL.GetItem<Tb_Gys_Cgs>(typeof(Tb_Gys_Cgs),ht);
            if (gcgx != null && gcgx.flag_stop.Equals(YesNoFlag.No))
            {
                br.Success = false;
                br.Message.Add("您已经关注过该供应商.");
                br.Level = ErrorLevel.Warning;
                br.Data = Gys_Cgs_Status.Accept;
                return br;
            }
            //删除申请记录         
            DAL.Delete(typeof(Tb_Gys_Cgs_Check), ht);
            //采购商
            ht.Clear();
            ht["id"] = model.id_cgs;
            var cgs = (Tb_Cgs)DAL.GetItem<Tb_Cgs>(typeof(Tb_Cgs), ht);
            if (cgs == null || !cgs.id.HasValue || !(cgs.id > 0))
            {
                br.Success = false;
                br.Message.Add("采购商不存在.");
                br.Level = ErrorLevel.Warning;
                br.Data = "cgs";
                return br;
            }
            ht.Clear();
            ht["id"] = model.id_gys;
            var gys = (Tb_Gys)DAL.GetItem<Tb_Gys>(typeof(Tb_Gys), ht);
            if (gys == null || !gys.id.HasValue || !(gys.id > 0))
            {
                br.Success = false;
                br.Message.Add("供应商不存在.");
                br.Level = ErrorLevel.Warning;
                br.Data = "gys";
                return br;
            }           
            DAL.Add(model);//添加关注申请


            //记录供采关系日志
            var Loggcgx = new Tb_Gys_Cgs_Log();
            Loggcgx.id_cgs = cgs.id.Value;
            Loggcgx.id_gys = gys.id.Value;
            Loggcgx.id_user = model.id_user;
            Loggcgx.flag_state = Gys_Cgs_Status.Apply;
            Loggcgx.flag_form = model.flag_form;
            Loggcgx.contents = string.Format("[{0}]向[{1}]提交关注申请.", cgs.companyname, gys.companyname);
            DAL.Add(Loggcgx);

            br.Success = true;
            br.Message.Add(String.Format("[{0}]向[{1}]提交关注申请.", cgs.companyname, gys.companyname));
            return br;
        }

        /// <summary>
        /// 获取是否已关注该供应商
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetCount(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            if (param["id_gys"].ToString() == param["id_cgs"].ToString()) {
                br.Success = false;
                br.Message.Add("不能关注自己，请选择其他供应商！");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            param.Add("not_flag_stop", 1);
            if (DAL.GetCount(typeof(Tb_Gys_Cgs), param)>0) {
                br.Success = false;
                br.Message.Add("该供应商已关注！");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            param.Add("not_flag_state",1);
            if (DAL.GetCount(typeof(Tb_Gys_Cgs_Check), param) > 0)
            {
                br.Success = false;
                br.Message.Add("该供应商已申请关注！");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            else {
                br.Success = true;
                br.Message.Add("该供应商可以关注！");
            }
            return br;
        }

        /// <summary>
        /// 撤销关注
        /// cxb
        /// 2015-4-13
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        //public override BaseResult Delete(Hashtable param)
        //{
        //    BaseResult br = new BaseResult();
        //    DAL.Delete(typeof(Tb_Gys_Cgs_Check),param);
        //    br.Success = true;
        //    return br;
        //}
    }
}
