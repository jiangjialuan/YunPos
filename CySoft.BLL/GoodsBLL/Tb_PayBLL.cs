using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.IDAL;
using CySoft.Model.Enums;
using CySoft.Model.Tb;

namespace CySoft.BLL.GoodsBLL
{
    public class Tb_PayBLL : BaseBLL
    {

        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult result=new BaseResult();
            if (param != null)
            {
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                result.Data = DAL.QueryList(typeof(Tb_Pay), param);
            }
            return result;
        }

        public override BaseResult Add(dynamic entity)
        {
            BaseResult res=new BaseResult(){Success = true};
            Tb_Pay payModel=entity as Tb_Pay;
            try
            {
                if (payModel == null)
                {
                    res.Success = false;
                    res.Message.Add("参数有误!");
                    res.Level = ErrorLevel.Warning;
                    return res;
                }
                Hashtable param=new Hashtable();
                param.Add("id_masteruser", payModel.id_masteruser);
                param.Add("s_mc", payModel.mc);
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                if (DAL.GetCount(typeof(Tb_Pay), param)>0)
                {
                    res.Success = false;
                    res.Message.Add("名称已存在!");
                    res.Level = ErrorLevel.Warning;
                    return res;
                }
                payModel.id = Guid.NewGuid().ToString();
                payModel.flag_delete = (int)Enums.FlagDelete.NoDelete;
                payModel.rq_create = payModel.rq_edit = DateTime.Now;
                DAL.Add(payModel);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Level=ErrorLevel.Warning;
                res.Message.Add("新增操作异常!");
            }
            return res;
        }

        public override BaseResult Update(dynamic entity)
        {
            BaseResult res = new BaseResult() { Success = true };
            Tb_Pay payModel=entity as Tb_Pay;
            try
            {
                if (payModel== null)
                {
                    res.Success = false;
                    res.Message.Add("参数有误!");
                    res.Level = ErrorLevel.Warning;
                    return res;
                }
                Hashtable param = new Hashtable();
                param.Add("id_masteruser", payModel.id_masteruser);
                param.Add("s_mc", payModel.mc);
                param.Add("not_id", payModel.id);
                param.Add("flag_delete",(int)Enums.FlagDelete.NoDelete);
                if (DAL.GetCount(typeof(Tb_Pay), param) > 0)
                {
                    res.Success = false;
                    res.Message.Add("名称已存在!");
                    res.Level = ErrorLevel.Warning;
                    return res;
                }
                Hashtable ht=new Hashtable();
                ht.Add("id", payModel.id);
                ht.Add("id_masteruser", payModel.id_masteruser);
                ht.Add("new_mc", payModel.mc);
                ht.Add("new_bz", payModel.bz);
                ht.Add("new_id_edit", payModel.id_edit);
                ht.Add("new_rq_edit", DateTime.Now);
                ht.Add("new_flag_stop", payModel.flag_stop);
                ht.Add("new_flag_type", payModel.flag_type);
                if (DAL.UpdatePart(typeof(Tb_Pay), ht)<=0)
                {
                    res.Success = false;
                    res.Message.Add("更新操作失败");
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Level=ErrorLevel.Warning;
                res.Message.Add("更新操作异常!");
            }
            return res;
        }

        public override BaseResult Get(Hashtable param)
        {
            BaseResult res=new BaseResult(){Success = true};
            try
            {
                res.Data = DAL.GetItem<Tb_Pay>(typeof(Tb_Pay), param);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message.Add("操作异常!");
            }
            return res;
        }

        public override BaseResult Delete(Hashtable param)
        {
            BaseResult res=new BaseResult(){Success = true};
            if (param==null)
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                res.Level=ErrorLevel.Warning;
                return res;
            }
            try
            {
                param.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);
                if (DAL.UpdatePart(typeof(Tb_Pay), param) <= 0)
                {
                    res.Success = false;
                    res.Message.Add("删除操作失败!");
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Level=ErrorLevel.Error;
                res.Message.Add("删除操作异常!");
            }
            return res;
        }


        public override BaseResult GetCount(Hashtable param = null)
        {
            BaseResult res = new BaseResult() { Success = true };
            res.Data= DAL.GetCount(typeof(Tb_Pay), param);
            return res;
        }

        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Tb_Pay), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_Pay>(typeof(Tb_Pay), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }



    }
}
