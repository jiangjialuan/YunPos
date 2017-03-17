using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Utility;
using Spring.Collections;

namespace CySoft.BLL.SystemBLL.SettingBLL
{
    public class PosRoleFuncSettingBLL : BaseBLL
    {
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult res=new BaseResult(){Success = true};
            List<Tb_Role_Pos_Function> roleFuncIds = entity as List<Tb_Role_Pos_Function>;
            Hashtable param=new Hashtable();
            if (!roleFuncIds.Any())
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            var roleIds = (from item in roleFuncIds select item.id_role).Distinct().ToList();
            param.Clear();
            var funcIds = (from item in roleFuncIds select item.id_pos_function).Distinct().ToList();
            param.Add("idList", funcIds.ToArray());
            param.Add("flag_type", 2);
            var funcList = DAL.QueryList<Tb_Pos_Function>(typeof(Tb_Pos_Function), param).ToList();
            if (funcList.Any())
            {
                funcList.ForEach(f =>
                {
                    Regex regex=new Regex(f.regex);
                    var rfmodel= roleFuncIds.FirstOrDefault(rf => rf.id_pos_function == f.id);
                    if (rfmodel != null)
                    {
                        if (!string.IsNullOrEmpty(rfmodel.minvalue)&&!regex.IsMatch(rfmodel.minvalue))
                        {
                            res.Success = false;
                            res.Message.Add(string.Format("{0},输入参数有误!",f.mc));
                        }
                        if (!string.IsNullOrEmpty(rfmodel.maxvalue)&&!regex.IsMatch(rfmodel.maxvalue))
                        {
                            res.Success = false;
                            res.Message.Add(string.Format("{0},输入参数有误!", f.mc));
                        }
                    }
                });
            }
            param.Clear();
            param.Add("idList", funcIds.ToArray());
            param.Add("flag_type", 3);
            var funcList3 = DAL.QueryList<Tb_Pos_Function>(typeof(Tb_Pos_Function), param).ToList();
            if (funcList3.Any())
            {
                funcList3.ForEach(f =>
                {
                    Regex regex = new Regex(f.regex);
                    var rfmodel = roleFuncIds.FirstOrDefault(rf => rf.id_pos_function == f.id);
                    if (rfmodel != null&&!string.IsNullOrEmpty(rfmodel.maxvalue))
                    {
                        if (!regex.IsMatch(rfmodel.maxvalue))
                        {
                            res.Success = false;
                            res.Message.Add(string.Format("{0},输入参数有误!", f.mc));
                        }
                    }
                });
            }
            if (!res.Success)
            {
                return res;
            }
            if (roleIds.Any())
            {
                param.Add("id_roleList", roleIds.ToArray());

                var list = DAL.QueryList<Tb_Role_Pos_FunctionWithName>(typeof(Tb_Role_Pos_Function), param).ToList();
                if (list.Any())
                {
                    StringBuilder sb=new StringBuilder();
                    StringBuilder sb1 = new StringBuilder();
                    sb.Append(@" update tb_role_pos_function set flag_use=1 where ");
                    sb1.Append(@" update tb_role_pos_function set flag_use=0 where ");
                    var _sb = 0;
                    var _sb1 = 0;
                    list.ForEach(d =>
                    {
                        var _newModel= roleFuncIds.FirstOrDefault(rfd =>
                            rfd.id_pos_function == d.id_pos_function 
                            && rfd.id_role == d.id_role);
                        if (_newModel != null &&
                            d.id_role == _newModel.id_role &&
                            d.id_pos_function == _newModel.id_pos_function)
                        {
                            sb.AppendFormat("(id_role='{0}' and id_pos_function='{1}') or ", d.id_role, d.id_pos_function);
                            roleFuncIds.Remove(_newModel);
                            _sb++;
                        }
                        else
                        {
                            sb1.AppendFormat("(id_role='{0}' and id_pos_function='{1}') or ", d.id_role, d.id_pos_function);
                            _sb1++;
                        }
                    });
                    sb.Append(" 1!=1 ");
                    sb1.Append(" 1!=1 ");
                    if (_sb>0)
                    {
                        DAL.ExecuteSql(sb.ToString());
                    }
                    if (_sb1>0)
                    {
                        DAL.ExecuteSql(sb1.ToString());
                    }
                }
                //param.Add("id_roleList", roleIds.ToArray());
                //DAL.Delete(typeof(Tb_Role_Pos_Function), param);
            }
            if (roleFuncIds.Any())
            {
                DAL.AddRange(roleFuncIds);
            }
            return res;
        }

        public override BaseResult Delete(Hashtable param)
        {
            BaseResult res = new BaseResult(){Success = true};
            if (!param.ContainsKey("id_role") && !param.ContainsKey("id_pos_function"))
            {
                res.Message.Add("参数有误!");
                return res;
            }
            DAL.Delete(typeof(Tb_Role_Pos_Function), param);
            return res;
        }


        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult res=new BaseResult(){Success = true};
            res.Data = DAL.QueryList<Tb_Role_Pos_FunctionWithName>(typeof(Tb_Role_Pos_Function), param);
            return res;
        }

        /// <summary>
        /// 修改前台权限启用状态
        /// </summary>
        /// <param name="res"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool ChangePosFunctionFlagUse(BaseResult res, ChangePosFunctionFlagUseModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.id_role))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return false;
            }
            Hashtable param = new Hashtable();
            param.Add("id_role", model.id_role);
            param.Add("new_flag_use", model.flag_use);
            DAL.UpdatePart(typeof(Tb_Role_Pos_Function), param);
            return true;
        }

        public override BaseResult Update(dynamic entity)
        {
            BaseResult res = new BaseResult(){Success = true};
            try
            {
               ChangePosFunctionFlagUseModel model= entity as ChangePosFunctionFlagUseModel;
               ChangePosFunctionFlagUse(res, model);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Level = ErrorLevel.Error;
                res.Message.Add("操作异常!");
            }
            return res;
        }
    }

    
}
