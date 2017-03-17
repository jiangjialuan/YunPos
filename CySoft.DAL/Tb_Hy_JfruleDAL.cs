using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.DAL.Base;
using CySoft.IDAL;
using CySoft.Model.Ts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.DAL.Tb
{
    public class Tb_Hy_JfruleDAL : BaseDAL, ITb_Hy_JfruleDAL
    {
        public void AddWithExists(Ts_HykDbjf model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (model == null) { return; }

                //会员阳历生日
                if (!string.IsNullOrEmpty(model.hy_jfsz_hysr_nbjf))
                {
                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_hysr_nbjf') ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_hysr_nbjf' ", model.hy_jfsz_hysr_nbjf, model.id_masteruser, model.id_shop));
                    sb.Append(" END ");
                    sb.Append(" ELSE BEGIN ");
                    sb.Append(" INSERT INTO ts_parm_shop (id_masteruser,id,id_shop,parmcode,parmname,parmvalue,regex,version,parmdescribe,sort_id,flag_editstyle ,flag_type) ");
                    sb.Append(string.Format(" SELECT TOP 1 '{0}',NEWID() id,'{1}',parmcode,parmname,'{2}',regex,version,parmdescribe,sort_id,flag_editstyle,'20' as flag_type from ts_parm_shop  ", model.id_masteruser, model.id_shop, model.hy_jfsz_hysr_nbjf));
                    sb.Append(" WHERE id_masteruser='0' and id_shop='0' and parmcode='hy_jfsz_hysr_nbjf' ");
                    sb.Append(" END ");
                    sb.Append("  ");

                    //请选择会员生日期间类型
                    if (!string.IsNullOrEmpty(model.hy_jfsz_hysr_lx))
                    {
                        sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_hysr_lx') ", model.id_masteruser, model.id_shop));
                        sb.Append(" BEGIN ");
                        sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_hysr_lx' ", model.hy_jfsz_hysr_lx, model.id_masteruser, model.id_shop));
                        sb.Append(" END ");
                        sb.Append(" ELSE BEGIN ");
                        sb.Append(" INSERT INTO ts_parm_shop (id_masteruser,id,id_shop,parmcode,parmname,parmvalue,regex,version,parmdescribe,sort_id,flag_editstyle,flag_type) ");
                        sb.Append(string.Format(" SELECT TOP 1 '{0}',NEWID() id,'{1}',parmcode,parmname,'{2}',regex,version,parmdescribe,sort_id,flag_editstyle,'20' as flag_type from ts_parm_shop  ", model.id_masteruser, model.id_shop, model.hy_jfsz_hysr_lx));
                        sb.Append(" WHERE id_masteruser='0' and id_shop='0' and parmcode='hy_jfsz_hysr_lx' ");
                        sb.Append(" END ");
                        sb.Append("  ");
                    }
                }
                else
                {
                    //判断如果数据库中存在有值的数据 清空
                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_hysr_nbjf' and isnull(parmvalue,'')!='' ) ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_hysr_nbjf' and  isnull(parmvalue,'')!=''  ", "", model.id_masteruser, model.id_shop));
                    sb.Append(" END ");

                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_hysr_lx' and isnull(parmvalue,'')!='' ) ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_hysr_lx' and  isnull(parmvalue,'')!=''  ", "", model.id_masteruser, model.id_shop));
                    sb.Append(" END ");

                }



                //星期
                if (!string.IsNullOrEmpty(model.hy_jfsz_week_nbjf))
                {
                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_week_nbjf') ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_week_nbjf' ", model.hy_jfsz_week_nbjf, model.id_masteruser, model.id_shop));
                    sb.Append(" END ");
                    sb.Append(" ELSE BEGIN ");
                    sb.Append(" INSERT INTO ts_parm_shop (id_masteruser,id,id_shop,parmcode,parmname,parmvalue,regex,version,parmdescribe,sort_id,flag_editstyle ,flag_type) ");
                    sb.Append(string.Format(" SELECT TOP 1 '{0}',NEWID() id,'{1}',parmcode,parmname,'{2}',regex,version,parmdescribe,sort_id,flag_editstyle ,'20' as flag_type from ts_parm_shop  ", model.id_masteruser, model.id_shop, model.hy_jfsz_week_nbjf));
                    sb.Append(" WHERE id_masteruser='0' and id_shop='0' and parmcode='hy_jfsz_week_nbjf' ");
                    sb.Append(" END ");
                    sb.Append("  ");

                    //请选择星期积分倍数的星期
                    if (!string.IsNullOrEmpty(model.hy_jfsz_week_val))
                    {
                        sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_week_val') ", model.id_masteruser, model.id_shop));
                        sb.Append(" BEGIN ");
                        sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_week_val' ", model.hy_jfsz_week_val, model.id_masteruser, model.id_shop));
                        sb.Append(" END ");
                        sb.Append(" ELSE BEGIN ");
                        sb.Append(" INSERT INTO ts_parm_shop (id_masteruser,id,id_shop,parmcode,parmname,parmvalue,regex,version,parmdescribe,sort_id,flag_editstyle,flag_type) ");
                        sb.Append(string.Format(" SELECT TOP 1 '{0}',NEWID() id,'{1}',parmcode,parmname,'{2}',regex,version,parmdescribe,sort_id,flag_editstyle ,'20' as flag_type from ts_parm_shop  ", model.id_masteruser, model.id_shop, model.hy_jfsz_week_val));
                        sb.Append(" WHERE id_masteruser='0' and id_shop='0' and parmcode='hy_jfsz_week_val' ");
                        sb.Append(" END ");
                        sb.Append("  ");
                    }
                    
                }
                else
                {
                    //判断如果数据库中存在有值的数据 清空
                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_week_nbjf' and isnull(parmvalue,'')!='' ) ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_week_nbjf' and  isnull(parmvalue,'')!=''  ", "", model.id_masteruser, model.id_shop));
                    sb.Append(" END ");

                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_week_val' and isnull(parmvalue,'')!='' ) ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_week_val' and  isnull(parmvalue,'')!=''  ", "", model.id_masteruser, model.id_shop));
                    sb.Append(" END ");

                }



                //日期
                if (!string.IsNullOrEmpty(model.hy_jfsz_day_nbjf))
                {
                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_day_nbjf') ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_day_nbjf' ", model.hy_jfsz_day_nbjf, model.id_masteruser, model.id_shop));
                    sb.Append(" END ");
                    sb.Append(" ELSE BEGIN ");
                    sb.Append(" INSERT INTO ts_parm_shop (id_masteruser,id,id_shop,parmcode,parmname,parmvalue,regex,version,parmdescribe,sort_id,flag_editstyle,flag_type) ");
                    sb.Append(string.Format(" SELECT TOP 1 '{0}',NEWID() id,'{1}',parmcode,parmname,'{2}',regex,version,parmdescribe,sort_id,flag_editstyle,'20' as flag_type from ts_parm_shop  ", model.id_masteruser, model.id_shop, model.hy_jfsz_day_nbjf));
                    sb.Append(" WHERE id_masteruser='0' and id_shop='0' and parmcode='hy_jfsz_day_nbjf' ");
                    sb.Append(" END ");
                    sb.Append("  ");

                    //请选择日期积分倍数的日期
                    if (!string.IsNullOrEmpty(model.hy_jfsz_day_val))
                    {
                        sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_day_val') ", model.id_masteruser, model.id_shop));
                        sb.Append(" BEGIN ");
                        sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_day_val' ", model.hy_jfsz_day_val, model.id_masteruser, model.id_shop));
                        sb.Append(" END ");
                        sb.Append(" ELSE BEGIN ");
                        sb.Append(" INSERT INTO ts_parm_shop (id_masteruser,id,id_shop,parmcode,parmname,parmvalue,regex,version,parmdescribe,sort_id,flag_editstyle,flag_type) ");
                        sb.Append(string.Format(" SELECT TOP 1 '{0}',NEWID() id,'{1}',parmcode,parmname,'{2}',regex,version,parmdescribe,sort_id,flag_editstyle ,'20' as flag_type from ts_parm_shop  ", model.id_masteruser, model.id_shop, model.hy_jfsz_day_val));
                        sb.Append(" WHERE id_masteruser='0' and id_shop='0' and parmcode='hy_jfsz_day_val' ");
                        sb.Append(" END ");
                        sb.Append("  ");
                    }
                }
                else
                {
                    //判断如果数据库中存在有值的数据 清空
                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_day_nbjf' and isnull(parmvalue,'')!='' ) ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_day_nbjf' and  isnull(parmvalue,'')!=''  ", "", model.id_masteruser, model.id_shop));
                    sb.Append(" END ");

                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_day_val' and isnull(parmvalue,'')!='' ) ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_day_val' and  isnull(parmvalue,'')!=''  ", "", model.id_masteruser, model.id_shop));
                    sb.Append(" END ");

                }




                //满金额
                if (!string.IsNullOrEmpty(model.hy_jfsz_xs_nbjf))
                {
                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_xs_nbjf') ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_xs_nbjf' ", model.hy_jfsz_xs_nbjf, model.id_masteruser, model.id_shop));
                    sb.Append(" END ");
                    sb.Append(" ELSE BEGIN ");
                    sb.Append(" INSERT INTO ts_parm_shop (id_masteruser,id,id_shop,parmcode,parmname,parmvalue,regex,version,parmdescribe,sort_id,flag_editstyle,flag_type) ");
                    sb.Append(string.Format(" SELECT TOP 1 '{0}',NEWID() id,'{1}',parmcode,parmname,'{2}',regex,version,parmdescribe,sort_id,flag_editstyle,'20' as flag_type from ts_parm_shop  ", model.id_masteruser, model.id_shop, model.hy_jfsz_xs_nbjf));
                    sb.Append(" WHERE id_masteruser='0' and id_shop='0' and parmcode='hy_jfsz_xs_nbjf' ");
                    sb.Append(" END ");
                    sb.Append("  ");

                    //请选择金额积分倍数的开始时间
                    if (!string.IsNullOrEmpty(model.hy_jfsz_xs_rq_b))
                    {
                        sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_xs_rq_b') ", model.id_masteruser, model.id_shop));
                        sb.Append(" BEGIN ");
                        sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_xs_rq_b' ", model.hy_jfsz_xs_rq_b, model.id_masteruser, model.id_shop));
                        sb.Append(" END ");
                        sb.Append(" ELSE BEGIN ");
                        sb.Append(" INSERT INTO ts_parm_shop (id_masteruser,id,id_shop,parmcode,parmname,parmvalue,regex,version,parmdescribe,sort_id,flag_editstyle,flag_type) ");
                        sb.Append(string.Format(" SELECT TOP 1 '{0}',NEWID() id,'{1}',parmcode,parmname,'{2}',regex,version,parmdescribe,sort_id,flag_editstyle,'20' as flag_type from ts_parm_shop  ", model.id_masteruser, model.id_shop, model.hy_jfsz_xs_rq_b));
                        sb.Append(" WHERE id_masteruser='0' and id_shop='0' and parmcode='hy_jfsz_xs_rq_b' ");
                        sb.Append(" END ");
                        sb.Append("  ");
                    }


                    //请选择金额积分倍数的结束时间
                    if (!string.IsNullOrEmpty(model.hy_jfsz_xs_rq_e))
                    {
                        sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_xs_rq_e') ", model.id_masteruser, model.id_shop));
                        sb.Append(" BEGIN ");
                        sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_xs_rq_e' ", model.hy_jfsz_xs_rq_e, model.id_masteruser, model.id_shop));
                        sb.Append(" END ");
                        sb.Append(" ELSE BEGIN ");
                        sb.Append(" INSERT INTO ts_parm_shop (id_masteruser,id,id_shop,parmcode,parmname,parmvalue,regex,version,parmdescribe,sort_id,flag_editstyle,flag_type) ");
                        sb.Append(string.Format(" SELECT TOP 1 '{0}',NEWID() id,'{1}',parmcode,parmname,'{2}',regex,version,parmdescribe,sort_id,flag_editstyle ,'20' as flag_type from ts_parm_shop  ", model.id_masteruser, model.id_shop, model.hy_jfsz_xs_rq_e));
                        sb.Append(" WHERE id_masteruser='0' and id_shop='0' and parmcode='hy_jfsz_xs_rq_e' ");
                        sb.Append(" END ");
                        sb.Append("  ");
                    }

                    //请选择金额积分倍数的消费满金额
                    if (!string.IsNullOrEmpty(model.hy_jfsz_xs_je))
                    {
                        sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_xs_je') ", model.id_masteruser, model.id_shop));
                        sb.Append(" BEGIN ");
                        sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_xs_je' ", model.hy_jfsz_xs_je, model.id_masteruser, model.id_shop));
                        sb.Append(" END ");
                        sb.Append(" ELSE BEGIN ");
                        sb.Append(" INSERT INTO ts_parm_shop (id_masteruser,id,id_shop,parmcode,parmname,parmvalue,regex,version,parmdescribe,sort_id,flag_editstyle,flag_type) ");
                        sb.Append(string.Format(" SELECT TOP 1 '{0}',NEWID() id,'{1}',parmcode,parmname,'{2}',regex,version,parmdescribe,sort_id,flag_editstyle ,'20' as flag_type from ts_parm_shop  ", model.id_masteruser, model.id_shop, model.hy_jfsz_xs_je));
                        sb.Append(" WHERE id_masteruser='0' and id_shop='0' and parmcode='hy_jfsz_xs_je' ");
                        sb.Append(" END ");
                        sb.Append("  ");
                    }

                }
                else
                {
                    //判断如果数据库中存在有值的数据 清空
                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_xs_nbjf' and isnull(parmvalue,'')!='' ) ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_xs_nbjf' and  isnull(parmvalue,'')!=''  ", "", model.id_masteruser, model.id_shop));
                    sb.Append(" END ");

                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_xs_rq_b' and isnull(parmvalue,'')!='' ) ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_xs_rq_b' and  isnull(parmvalue,'')!=''  ", "", model.id_masteruser, model.id_shop));
                    sb.Append(" END ");

                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_xs_rq_e' and isnull(parmvalue,'')!='' ) ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_xs_rq_e' and  isnull(parmvalue,'')!=''  ", "", model.id_masteruser, model.id_shop));
                    sb.Append(" END ");

                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_jfsz_xs_je' and isnull(parmvalue,'')!='' ) ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_jfsz_xs_je' and  isnull(parmvalue,'')!=''  ", "", model.id_masteruser, model.id_shop));
                    sb.Append(" END ");

                }



                dataMapper.Insert("DataTools.ExecuteSql", sb.ToString());

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
