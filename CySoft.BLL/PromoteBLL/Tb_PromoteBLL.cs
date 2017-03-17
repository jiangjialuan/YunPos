using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Model.Other;
using CySoft.Model.Tb;

namespace CySoft.BLL.PromoteBLL
{
    public class Tb_PromoteBLL : BaseBLL
    {

        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Tb_Promote), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_Promote_Query>(typeof(Tb_Promote), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }

        public override BaseResult Add(dynamic entity)
        {
            BaseResult res=new BaseResult(){Success = true};
            var model= entity as PromoteViewModel;
            if (model==null)
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (string.IsNullOrEmpty(model.cxzt))
            {
                res.Success = false;
                res.Message.Add("促销主题不能为空!");
                return res;
            }
            if (model.day_b==DateTime.MinValue)
            {
                res.Success = false;
                res.Message.Add("促销开始日期不能为空!");
                return res;
            }
            if (model.day_e == DateTime.MinValue)
            {
                res.Success = false;
                res.Message.Add("促销结束日期不能为空!");
                return res;
            }
            if (model.day_b>model.day_e)
            {
                res.Success = false;
                res.Message.Add("促销开始日期必须小于结束日期!");
                return res;
            }
            if (string.IsNullOrEmpty(model.spxz))
            {
                res.Success = false;
                res.Message.Add("商品选择必选!");
                return res;
            }
            if (string.IsNullOrEmpty(model.hylx))
            {
                res.Success = false;
                res.Message.Add("请选择会员类型!");
                return res;
            }
            if (string.IsNullOrEmpty(model.id_shops))
            {
                res.Success = false;
                res.Message.Add("请选择门店!");
                return res;
            }
            Tb_Promote promoteModel=new Tb_Promote();
            promoteModel.id_masteruser = model.id_masteruser;
            promoteModel.id = GetGuid;
            promoteModel.id_bill = GetGuid;
            promoteModel.id_billmx = GetGuid;
            promoteModel.sort_id = 1;

            promoteModel.rule_name = model.cxzt;
            promoteModel.day_b = model.day_b;
            promoteModel.day_e = model.day_e;


            promoteModel.time_b = model.time_b;
            promoteModel.time_e = model.time_e;
            promoteModel.yxj = 1;
            //promoteModel.flag_rqfw = model.flag_rqfw;
            promoteModel.weeks = model.weeks;
            promoteModel.days = model.days;
            
            promoteModel.examine = model.jsfs;
            promoteModel.strategy = model.jsgz;
            promoteModel.condition_1 = string.Format("{0}", model.condition_1);
            promoteModel.condition_2 = string.Format("{0}", model.condition_2);
            promoteModel.condition_3 = string.Format("{0}", model.condition_3);
            promoteModel.result_1 = string.Format("{0}", model.result_1);
            promoteModel.result_2 = string.Format("{0}", model.result_1);
            promoteModel.result_3 = string.Format("{0}", model.result_3);
            //TODO 确认style存什么
            promoteModel.style = "";
            //TODO 确认id_object存什么
            promoteModel.id_object = "";
            promoteModel.preferential = model.preferential;
            promoteModel.flag_stop = model.flag_stop;
            promoteModel.id_hyfl_list = model.hylx;
            promoteModel.flag_delete = (byte)Enums.FlagDelete.NoDelete;
            promoteModel.id_create = model.id_user;
            promoteModel.rq_create = DateTime.Now;
            return res;
        }

        public override BaseResult Stop(Hashtable param)
        {
            BaseResult res = new BaseResult() {Success = true};
            if (param == null || !param.ContainsKey("id") || !param.ContainsKey("id_masteruser"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            var id = param["id"];
            var id_masteruser = param["id_masteruser"];
            param.Clear();
            param.Add("id",id);
            param.Add("id_masteruser", id_masteruser);
            param.Add("new_flag_stop", (int)Enums.FlagStop.Stopped);
            DAL.UpdatePart(typeof(Tb_Promote), param);
            return res;
        }

    }
}
