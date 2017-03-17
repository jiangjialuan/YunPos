using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Model.Tb;
using CySoft.Frame.Attributes;
using CySoft.IBLL;

#region 银行账号
#endregion

namespace CySoft.BLL.FundsBLL
{
    public class BankAccountBLL : BaseBLL, IBankAccountBLL
    {

        /// <summary>
        /// 单个
        /// znt 2015-04-24
        /// </summary>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long id = Convert.ToInt64(param["id"]);

            param.Clear();
            param.Add("id", id);
            Tb_Gys_Account model = DAL.GetItem<Tb_Gys_Account>(typeof(Tb_Gys_Account), param);
            if (model == null)
            {
                br.Success = false;
                br.Message.Add(string.Format("停用银行账号失败，该账号不存在或资料已缺失！"));
                br.Level = ErrorLevel.Warning;
                return br;
            }

            br.Data = model;
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 分页
        /// znt 2015-04-24
        /// </summary>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { TotalCount = DAL.GetCount(typeof(Tb_Gys_Account), param) };
            if (pn.TotalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_Gys_Account>(typeof(Tb_Gys_Account), param);
            }
            else
            {
                pn.Data = new List<Tb_Gys_Account>();
            }
            pn.Success = true;
            return pn;
        }

        /// <summary>
        /// 保存账号信息 
        /// znt 2015-04-24
        /// </summary>
        public override BaseResult Save(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Tb_Gys_Account model = (Tb_Gys_Account)entity;
            Hashtable param = new Hashtable();
            param.Add("id", model.id);
            if (DAL.GetCount(typeof(Tb_Gys_Account), param) > 0)
            {
                param.Clear();
                param.Add("id", model.id);
                param.Add("new_name_bank", model.name_bank);
                param.Add("new_account_bank", model.account_bank);
                param.Add("new_khr",model.khr);
                param.Add("new_id_edit", model.id_edit);
                param.Add("new_rq_edit", model.rq_edit);
                DAL.UpdatePart(typeof(Tb_Gys_Account), param);
                br.Success = true;
                return br;
            }
            else
            {
                param.Clear();
                param.Add("id_gys", model.id_gys);
                if (DAL.GetCount(typeof(Tb_Gys_Account), param) > 0)
                {
                    model.flag_default = 0;
                    DAL.Add(model);
                }
                else
                {
                    model.flag_default = 1;
                    DAL.Add(model);
                }
            }

            br.Success=true;
            return br;
        }

        /// <summary>
        /// 停用账号
        /// znt 2015-04-24
        /// </summary>
        public override BaseResult Stop(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long id = Convert.ToInt64(param["id"]);
            long id_edit = Convert.ToInt64(param["id_edit"]);

            param.Clear();
            param.Add("id", id);
            Tb_Gys_Account model = DAL.GetItem<Tb_Gys_Account>(typeof(Tb_Gys_Account), param);
            if (model == null)
            {
                br.Success = false;
                br.Message.Add(string.Format("停用银行账号失败，该账号不存在或资料已缺失！"));
                br.Level = ErrorLevel.Warning;
                return br;
            }

            if (model.flag_default == 1)
            {
                br.Success = false;
                br.Message.Add(string.Format("{0}【{1}】账号为默认账号，不可禁用！",model.name_bank,model.account_bank));
                br.Level = ErrorLevel.Warning;
                return br;
            }

            param.Clear();
            param.Add("id", id);
            param.Add("new_flag_stop", 1);
            param.Add("new_id_edit", id_edit);
            param.Add("new_rq_edit", DateTime.Now);
            int result= DAL.UpdatePart(typeof(Tb_Gys_Account), param);

            if (result > 0)
            {
                br.Success = false;
                br.Message.Add(String.Format("停用银行账号失败。信息：账号流水号:{0}", model.id));
                br.Level = ErrorLevel.Error;
                return br;
            }

            br.Success = true;
            br.Message.Add(String.Format("停用银行账号成功。信息：账号流水号:{0}", model.id));
            return br;

        }

        /// <summary>
        /// 启用账号
        /// znt 2015-04-24
        /// </summary>
        public override BaseResult Active(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long id = Convert.ToInt64(param["id"]);
            long id_edit = Convert.ToInt64(param["id_edit"]);

            param.Clear();
            param.Add("id", id);
            Tb_Gys_Account model = DAL.GetItem<Tb_Gys_Account>(typeof(Tb_Gys_Account), param);
            if (model == null)
            {
                br.Success = false;
                br.Message.Add(string.Format("启用银行账号失败，该账号不存在或资料已缺失！"));
                br.Level = ErrorLevel.Warning;
                return br;
            }

            param.Clear();
            param.Add("id", id);
            param.Add("new_flag_stop", 0);
            param.Add("new_id_edit", id_edit);
            param.Add("new_rq_edit", DateTime.Now);
            int result = DAL.UpdatePart(typeof(Tb_Gys_Account), param);

            if (result > 0)
            {
                br.Success = false;
                br.Message.Add(String.Format("启用银行账号失败。信息：账号流水号:{0}", model.id));
                br.Level = ErrorLevel.Error;
                return br;
            }

            br.Success = true;
            br.Message.Add(String.Format("启用银行账号成功。信息：账号流水号:{0}", model.id));
            return br;
        }

        /// <summary>
        /// 设置默认
        /// znt 2015-04-24
        /// </summary>
        [Transaction]
        public BaseResult SetDefault(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long id = Convert.ToInt64(param["id"]);
            long id_gys = Convert.ToInt64(param["id_gys"]);

            param.Clear();
            param.Add("id", id);
            if (DAL.GetCount(typeof(Tb_Gys_Account),param)<=0)
            {
                br.Success = false;
                br.Message.Add(string.Format("停用银行账号失败，该账号不存在或资料已缺失！"));
                br.Level = ErrorLevel.Warning;
                return br;
            }

            param.Clear();
            param.Add("id_gys",id_gys);
            param.Add("flag_default", 1);
            Tb_Gys_Account OldDefaultMOdel = DAL.QueryList<Tb_Gys_Account>(typeof(Tb_Gys_Account), param).FirstOrDefault();
            if (OldDefaultMOdel != null)
            {
                param.Clear();
                param.Add("id",OldDefaultMOdel.id);
                param.Add("new_flag_default", 0);
                DAL.UpdatePart(typeof(Tb_Gys_Account), param);
            }

            param.Clear();
            param.Add("id",id);
            param.Add("new_flag_default",1);
            param.Add("new_flag_stop", 0);
            int result= DAL.UpdatePart(typeof(Tb_Gys_Account),param);
            if (result > 0)
            {
                br.Success = false;
                br.Message.Add(String.Format("设置默认银行账号失败。信息：账号流水号:{0}", id));
                br.Level = ErrorLevel.Error;
                return br;
            }

            br.Success = true;
            br.Message.Add(String.Format("设置默认银行账号成功。"));
            return br;

        }

        /// <summary>
        /// 获取供应商默认付款账号
        /// znt 2015-04-27
        /// </summary>
        public BaseResult GetDefault(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long id_gys = Convert.ToInt64(param["id_gys"]);

            param.Clear();
            param.Add("id_gys", id_gys);
            param.Add("flag_default", 1);
            Tb_Gys_Account model = DAL.GetItem<Tb_Gys_Account>(typeof(Tb_Gys_Account), param);
            if (model == null)
            {
                param.Clear();
                param.Add("id_gys",id_gys);
                model = DAL.QueryList<Tb_Gys_Account>(typeof(Tb_Gys_Account), param).FirstOrDefault();
                if (model == null)
                {
                    model = new Tb_Gys_Account();
                }
            }

            br.Data = model;
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 删除 
        /// znt 2015-04-24
        /// </summary>
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long id = Convert.ToInt64(param["id"]);

            param.Clear();
            param.Add("id", id);
            Tb_Gys_Account model = DAL.GetItem<Tb_Gys_Account>(typeof(Tb_Gys_Account), param);
            if (model == null)
            {
                br.Success = false;
                br.Message.Add(string.Format("删除银行账号失败，该账号不存在或资料已缺失！"));
                br.Level = ErrorLevel.Warning;
                return br;
            }

            if (model.flag_default == 1)
            {
                br.Success = false;
                br.Message.Add(string.Format("{0}【{1}】账号为默认账号，不可删除！", model.name_bank, model.account_bank));
                br.Level = ErrorLevel.Warning;
                return br;
            }

            param.Clear();
            param.Add("id", id);
            DAL.Delete(typeof(Tb_Gys_Account), param);
            br.Success = true;
            return br;

        }

        
    }
}
