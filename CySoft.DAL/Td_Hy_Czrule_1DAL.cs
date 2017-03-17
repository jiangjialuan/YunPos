
#region Imports
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
#endregion

namespace CySoft.DAL.Td
{
    public class Td_Hy_Czrule_1DAL : BaseDAL, ITd_Hy_Czrule_1DAL
    {
        public void AddWithExists(Ts_HykDbjf model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (model == null) { return; }

                //每日最小充值金额:
                if (!string.IsNullOrEmpty(model.hy_czje_min_onec))
                {
                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_czje_min_onec') ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_czje_min_onec' ", model.hy_czje_min_onec, model.id_masteruser, model.id_shop));
                    sb.Append(" END ");
                    sb.Append(" ELSE BEGIN ");
                    sb.Append(" INSERT INTO ts_parm_shop (id_masteruser,id,id_shop,parmcode,parmname,parmvalue,regex,version,parmdescribe,sort_id,flag_editstyle,flag_type) ");
                    sb.Append(string.Format(" SELECT TOP 1 '{0}',NEWID() id,'{1}',parmcode,parmname,'{2}',regex,version,parmdescribe,sort_id,flag_editstyle ,'21' as flag_type from ts_parm_shop  ", model.id_masteruser, model.id_shop, model.hy_czje_min_onec));
                    sb.Append(" WHERE id_masteruser='0' and id_shop='0' and parmcode='hy_czje_min_onec' ");
                    sb.Append(" END ");
                    sb.Append("  ");
                }
                else
                {
                    //判断如果数据库中存在有值的数据 清空
                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_czje_min_onec' and isnull(parmvalue,'')!='' ) ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_czje_min_onec' and  isnull(parmvalue,'')!=''  ", "", model.id_masteruser, model.id_shop));
                    sb.Append(" END ");
                }

                //每日最大充值金额:
                if (!string.IsNullOrEmpty(model.hy_czje_max_onec))
                {
                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_czje_max_onec') ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_czje_max_onec' ", model.hy_czje_max_onec, model.id_masteruser, model.id_shop));
                    sb.Append(" END ");
                    sb.Append(" ELSE BEGIN ");
                    sb.Append(" INSERT INTO ts_parm_shop (id_masteruser,id,id_shop,parmcode,parmname,parmvalue,regex,version,parmdescribe,sort_id,flag_editstyle,flag_type) ");
                    sb.Append(string.Format(" SELECT TOP 1 '{0}',NEWID() id,'{1}',parmcode,parmname,'{2}',regex,version,parmdescribe,sort_id,flag_editstyle ,'21' as flag_type  from ts_parm_shop  ", model.id_masteruser, model.id_shop, model.hy_czje_max_onec));
                    sb.Append(" WHERE id_masteruser='0' and id_shop='0' and parmcode='hy_czje_max_onec' ");
                    sb.Append(" END ");
                    sb.Append("  ");
                }
                else
                {
                    //判断如果数据库中存在有值的数据 清空
                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_czje_max_onec' and isnull(parmvalue,'')!='' ) ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_czje_max_onec' and  isnull(parmvalue,'')!=''  ", "", model.id_masteruser, model.id_shop));
                    sb.Append(" END ");
                }

                //每月最大充值金额:
                if (!string.IsNullOrEmpty(model.hy_czje_max_month))
                {
                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_czje_max_month') ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_czje_max_month' ", model.hy_czje_max_month, model.id_masteruser, model.id_shop));
                    sb.Append(" END ");
                    sb.Append(" ELSE BEGIN ");
                    sb.Append(" INSERT INTO ts_parm_shop (id_masteruser,id,id_shop,parmcode,parmname,parmvalue,regex,version,parmdescribe,sort_id,flag_editstyle,flag_type) ");
                    sb.Append(string.Format(" SELECT TOP 1 '{0}',NEWID() id,'{1}',parmcode,parmname,'{2}',regex,version,parmdescribe,sort_id,flag_editstyle ,'21' as flag_type from ts_parm_shop  ", model.id_masteruser, model.id_shop, model.hy_czje_max_month));
                    sb.Append(" WHERE id_masteruser='0' and id_shop='0' and parmcode='hy_czje_max_month' ");
                    sb.Append(" END ");
                    sb.Append("  ");
                }
                else
                {
                    //判断如果数据库中存在有值的数据 清空
                    sb.Append(string.Format(" IF EXISTS(SELECT ID FROM ts_parm_shop where id_masteruser='{0}' and id_shop='{1}' and parmcode='hy_czje_max_month' and isnull(parmvalue,'')!='' ) ", model.id_masteruser, model.id_shop));
                    sb.Append(" BEGIN ");
                    sb.Append(string.Format(" UPDATE ts_parm_shop set parmvalue='{0}' where id_masteruser='{1}' and id_shop='{2}' and parmcode='hy_czje_max_month' and  isnull(parmvalue,'')!=''  ", "", model.id_masteruser, model.id_shop));
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
