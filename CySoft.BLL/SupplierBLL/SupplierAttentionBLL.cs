using System;
using System.Collections;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Tb;
using CySoft.Utility;
using System.Collections.Generic;
using CySoft.Frame.Attributes;
using CySoft.IBLL;
using CySoft.IDAL;
using CySoft.BLL.Tools.CodingRule;
using CySoft.Frame.Common;

namespace CySoft.BLL.SupplierBLL
{
    public class SupplierAttentionBLL : BaseBLL
    {
        /// <summary>
        /// 通过申请
        /// cxb
        /// 2015-4-15 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            Tb_Cgs_Edit model = (Tb_Cgs_Edit)param["model"];            
            long id_supplier = TypeConvert.ToInt64(param["id_supplier"],0);
            string flag_from = param.ContainsKey("flag_from") ? param["flag_from"].ToString() : "PC";
            if (model == null || !model.id.HasValue || !(model.id>0) || id_supplier.Equals(0))
            {
                br.Success = false;
                br.Message.Add("关注参数错误.");
                br.Level = ErrorLevel.Warning;
                br.Data = "companyName";
                return br;
            }


            Hashtable ht = new Hashtable();
            ht.Add("id_gys", id_supplier);
            ht.Add("id_cgs", model.id);
            var gcgx = DAL.GetItem<Tb_Gys_Cgs_Check>(typeof(Tb_Gys_Cgs_Check), ht);
            if (gcgx == null)
            {
                br.Success = false;
                br.Message.Add("客户的申请已处理过了.");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            if (gcgx.flag_form.Equals(Gys_Cgs_Status.Refuse))
            {
                br.Success = false;
                br.Message.Add("客户的申请已被拒绝过了.");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            //采购商
            param.Clear();
            param["id"] = model.id;
            var cgs = (Tb_Cgs)DAL.GetItem<Tb_Cgs>(typeof(Tb_Cgs), param);
            if (cgs == null || !cgs.id.HasValue || !(cgs.id>0))
            {
                //成功后删除申请记录         
                DAL.Delete(typeof(Tb_Gys_Cgs_Check), ht);
                br.Success = false;
                br.Message.Add("客户不存在.");
                br.Level = ErrorLevel.Warning;
                br.Data = "cgs";
                return br;
            }
            param.Clear();
            param["id"] = id_supplier;
            var gys = (Tb_Gys)DAL.GetItem<Tb_Gys>(typeof(Tb_Gys), param);
            if (gys == null || !gys.id.HasValue || !(gys.id > 0))
            {
                //成功后删除申请记录         
                DAL.Delete(typeof(Tb_Gys_Cgs_Check), ht);
                br.Success = false;
                br.Message.Add("供应商不存在.");
                br.Level = ErrorLevel.Warning;
                br.Data = "gys";
                return br;
            }           

            string name_gys = gys.companyname;

            param.Clear();
            param.Add("alias_cgs", model.companyname);
            param.Add("id_user_master_gys", gys.id_user_master);
            param.Add("not_id_user_master_cgs", cgs.id_user_master);
            if (DAL.GetCount(typeof(Tb_Gys_Cgs), param) > 0)
            {
                br.Success = false;
                br.Message.Add("客户名已被使用");
                br.Level = ErrorLevel.Warning;
                br.Data = "companyName";
                return br;
            }
            if (!model.bm_gys_Interface.IsEmpty())
            {
                param.Clear();
                param.Add("bm_gys_Interface", model.bm_gys_Interface);
                param.Add("id_user_master_gys", gys.id_user_master);
                param.Add("not_id_user_master_cgs", cgs.id_user_master);
                if (DAL.GetCount(typeof(Tb_Gys_Cgs), param) > 0)
                {
                    br.Success = false;
                    br.Message.Add("客户编码已被使用");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "bm_gys_Interface";
                    return br;
                }
            }

            //获取供应商的公司名称
            param.Clear();
            param.Add("id", gys.id_user_master);
            var TbUser = DAL.GetItem<Tb_User>(typeof (Tb_User), param);
            string gys_companyname = string.Empty;
            if (TbUser != null)
            {
                gys_companyname = TbUser.companyname;
            }


            param.Clear();
            param["id_user_master_gys"] = gys.id_user_master;
            param["id_user_master_cgs"] = cgs.id_user_master;           
            if (DAL.GetCount(typeof(Tb_Gys_Cgs), param) > 0)
            {
                if (gys_companyname != string.Empty) param.Add("new_alias_gys", gys_companyname);
                param.Add("new_alias_cgs", model.companyname);
                param.Add("new_id_cgs_level", model.id_cgs_level);
                param.Add("new_bm_gys_Interface", model.bm_gys_Interface);
                param.Add("new_rq_treaty_end", model.rq_treaty_end);
                param.Add("new_rq_treaty_start", model.rq_treaty_start);
                param.Add("new_flag_stop", YesNoFlag.No);
                param.Add("new_rq_edit", model.rq_edit);
                param.Add("new_id_edit", model.id_edit);
                DAL.UpdatePart(typeof(Tb_Gys_Cgs), param);
            }
            else
            {
                Tb_Gys_Cgs gysCgs = new Tb_Gys_Cgs();
                gysCgs.id_gys = id_supplier;
                gysCgs.id_cgs = cgs.id;
                gysCgs.alias_cgs = model.companyname;
                gysCgs.alias_gys = name_gys;
                gysCgs.flag_from = flag_from;
                gysCgs.flag_pay = model.flag_pay;
                gysCgs.flag_stop = YesNoFlag.No;
                gysCgs.id_cgs_level = model.id_cgs_level;
                gysCgs.id_create = model.id_create;
                gysCgs.id_edit = model.id_edit;
                gysCgs.id_user_cgs = cgs.id_user_master;
                gysCgs.id_user_gys = gys.id_user_master;
                gysCgs.id_user_master_cgs = cgs.id_user_master;
                gysCgs.id_user_master_gys = gys.id_user_master;
                gysCgs.rq_treaty_end = model.rq_treaty_end;
                gysCgs.rq_treaty_start = model.rq_treaty_start;
                gysCgs.bm_gys_Interface = model.bm_gys_Interface;
                gysCgs.rq_create = DateTime.Now;
                DAL.Add(gysCgs);
            }

            //成功后删除申请记录         
            DAL.Delete(typeof(Tb_Gys_Cgs_Check), ht);

            //记录供采关系日志
            var Loggcgx = new Tb_Gys_Cgs_Log();
            Loggcgx.id_cgs = cgs.id.Value;
            Loggcgx.id_gys = gys.id.Value;
            Loggcgx.id_user = model.id_create.Value;
            Loggcgx.flag_state = Gys_Cgs_Status.Accept;
            Loggcgx.flag_form = flag_from;
            Loggcgx.contents = string.Format("[{0}]接受[{1}]成为客户.", gys.companyname, cgs.companyname);
            DAL.Add(Loggcgx);

            br.Message.Add(string.Format("[{0}]接受[{1}]成为客户.", gys.companyname, cgs.companyname));
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 拒绝关注
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>        
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            if (param == null || !param.ContainsKey("model") || !param.ContainsKey("id_user"))
            {
                br.Success = false;
                br.Message.Add("拒绝关注参数错误.");
                br.Level = ErrorLevel.Warning;
                br.Data = "companyName";
                return br;
            }
            long id_user = TypeConvert.ToInt64(param["id_user"], 0);
            Tb_Gys_Cgs_Check gyscgscheck = (Tb_Gys_Cgs_Check)param["model"];
            string flag_from = param.ContainsKey("flag_from")?param["flag_from"].ToString(): "pc";
            if (gyscgscheck == null || !gyscgscheck.id.HasValue || !(gyscgscheck.id > 0))
            {
                br.Success = false;
                br.Message.Add("拒绝关注参数错误.");
                br.Level = ErrorLevel.Warning;
                br.Data = "companyName";
                return br;
            }

            Hashtable ht = new Hashtable();
            ht.Add("id", gyscgscheck.id);
            var gcgx = DAL.GetItem<Tb_Gys_Cgs_Check>(typeof(Tb_Gys_Cgs_Check), ht);
            if (gcgx == null)
            {
                br.Success = false;
                br.Message.Add("客户的申请已处理过了，请刷新页面.");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            if (gcgx.flag_form.Equals(Gys_Cgs_Status.Refuse))
            {
                br.Success = false;
                br.Message.Add("客户的申请已被拒绝过了，请刷新页面.");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            param["id"] = gyscgscheck.id;
            param["new_flag_state"] = Gys_Cgs_Status.Refuse;
            param["new_refuse"] = gyscgscheck.refuse;
            DAL.UpdatePart(typeof(Tb_Gys_Cgs_Check),param);

            //记录供采关系日志
            var Loggcgx = new Tb_Gys_Cgs_Log();
            Loggcgx.id_cgs = gcgx.id_cgs.Value;
            Loggcgx.id_gys = gcgx.id_gys.Value;
            Loggcgx.id_user = id_user;
            Loggcgx.flag_state = Gys_Cgs_Status.Refuse;
            Loggcgx.flag_form = flag_from;
            Loggcgx.contents = string.Format("[{0}]拒绝[{1}]的申请.", gcgx.mc_gys, gcgx.mc_cgs);
            DAL.Add(Loggcgx);

            br.Message.Add(string.Format("[{0}]拒绝[{1}]的申请.", gcgx.mc_gys, gcgx.mc_cgs));
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 采购商删除关注申请
        /// tim
        /// 2015-5-21 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            if (param == null || !(param.ContainsKey("id") || param.ContainsKey("id_gys")) || !param.ContainsKey("id_cgs") || !param.ContainsKey("id_user"))
            {
                br.Success = false;
                br.Message.Add("删除关注申请参数错误.");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            long id_user = TypeConvert.ToInt64(param["id_user"], 0);
            
            string flag_from = param.ContainsKey("flag_from") ? param["flag_from"].ToString() : "pc";

            Hashtable ht = new Hashtable();
            ht.Add("id", param.ContainsKey("id_gys") ? param["id_gys"] : param["id"]);
            ht.Add("id_csg",param["id_cgs"]);
            var gcgx = DAL.GetItem<Tb_Gys_Cgs_Check>(typeof(Tb_Gys_Cgs_Check), ht);
            if (gcgx == null)
            {
                br.Success = true;
                br.Message.Add("关注申请已删除了.");
                return br;
            }

            //记录供采关系日志
            var Loggcgx = new Tb_Gys_Cgs_Log();
            Loggcgx.id_cgs = gcgx.id_cgs.Value;
            Loggcgx.id_gys = gcgx.id_gys.Value;
            Loggcgx.id_user = id_user;
            Loggcgx.flag_state = Gys_Cgs_Status.Cancel;
            Loggcgx.flag_form = flag_from;
            Loggcgx.contents = string.Format("[{0}]撤销对[{1}]的关注申请.", gcgx.mc_cgs, gcgx.mc_gys);
            DAL.Add(Loggcgx);

            DAL.Delete(typeof(Tb_Gys_Cgs_Check), ht);
            br.Message.Add(string.Format("[{0}]撤销对[{1}]的关注申请.", gcgx.mc_cgs, gcgx.mc_gys));
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 读取关注采购商
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryItem<Tb_Cgs_Attention>(typeof(Tb_Gys_Cgs_Check), param);
            if (br.Data == null)
            {
                br.Success = false;
                br.Message.Add("客户已取消关注申请。");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            br.Success = true;
            return br;
        }
    }
}
