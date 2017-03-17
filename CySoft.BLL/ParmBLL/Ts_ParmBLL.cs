using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using System.Text.RegularExpressions;
using CySoft.IBLL;
using CySoft.IDAL;
using CySoft.Frame.Attributes;
using CySoft.Model.Ts;

#region 反馈管理
#endregion

namespace CySoft.BLL.ParmBLL
{
    public class Ts_ParmBLL : BaseBLL
    {
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult result = new BaseResult() { Success = true };
            if (param == null)
            {
                result.Success = false;
                result.Level = ErrorLevel.Warning;
                result.Message.Add("参数有误!");
                return result;
            }
            result.Data = DAL.QueryList<Ts_Parm>(typeof(Ts_Parm), param).ToList();
            return result;
        }

        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Ts_Parm), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Ts_Parm>(typeof(Ts_Parm), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }

        public override BaseResult Add(dynamic entity)
        {
            BaseResult res = new BaseResult() { Success = true };
            Ts_Parm model = entity as Ts_Parm;
            if (model == null)
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (string.IsNullOrEmpty(model.parmname))
            {
                res.Success = false;
                res.Message.Add("参数名称不能为空!");
                return res;
            }
            if (string.IsNullOrEmpty(model.parmcode))
            {
                res.Success = false;
                res.Message.Add("参数编码不能为空!");
                return res;
            }
            Hashtable param = new Hashtable();
            param.Add("parmcode", model.parmcode);
            param.Add("id_masteruser", model.id_masteruser);
            if (DAL.GetCount(typeof(Ts_Parm), param) > 0)
            {
                res.Success = false;
                res.Message.Add("参数编码已存在!");
                return res;
            }
            model.id = Guid.NewGuid().ToString();
            DAL.Add(model);
            return res;
        }

        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult();
            res.Data = DAL.GetItem<Ts_Parm>(typeof(Ts_Parm), param);
            return res;
        }

        public override BaseResult Update(dynamic entity)
        {
            BaseResult res = new BaseResult() { Success = true };
            Ts_Parm model = entity as Ts_Parm;
            if (model == null)
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", model.id_masteruser);
            param.Add("id", model.id);
            var oldModel = DAL.GetItem<Ts_Parm>(typeof(Ts_Parm), param);
            if (oldModel == null)
            {
                res.Success = false;
                res.Message.Add("数据已不存在!");
                return res;
            }
            if (!ParmValidation.Valid(oldModel.regex, model.parmvalue, res))
            {
                return res;
            }
            if (ParmValidation.NeedValidParmCode(oldModel.parmcode))
            {
                if (!ParmValidation.ValidDigit(model.parmvalue, oldModel.parmvalue, res))
                {
                    return res;
                }
            }
            param.Clear();
            param.Add("id", model.id);
            param.Add("new_parmvalue", model.parmvalue);
            if (DAL.UpdatePart(typeof(Ts_Parm), param) <= 0)
            {
                res.Message.Add("操作失败!");
            }
            return res;
        }

        public override BaseResult Delete(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            var id = param["id"].ToString();
            if (string.IsNullOrEmpty(id))
            {
                res.Success = false;
                res.Message.Add("删除失败");
                return res;
            }
            param.Clear();
            param.Add("id", id);
            DAL.Delete(typeof(Ts_Parm), param);
            return res;
        }
    }

    public class ParmValidation
    {
        public static bool Valid(string regex, string value, BaseResult res)
        {
            if (!string.IsNullOrEmpty(regex))
            {
                Regex reg = new Regex(regex);
                if (!reg.IsMatch(value))
                {
                    res.Success = false;
                    res.Message.Add(string.Format("参数为{0}之间的数!", regex.Replace("^[", "").Replace("]$","")));
                    return false;
                }
            }
            return true;
        }
        public static bool ValidDigit(string value, string oldValue, BaseResult res)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(oldValue))
            {
                return true;
            }
            int val = -1;
            int.TryParse(value,out val);
            int oldVal = -1;
            int.TryParse(oldValue, out oldVal);
            if (val<oldVal)
            {
                res.Success = false;
                res.Message.Add("设定数值不能小于原数值");
                return false;
            }
            return true;
        }

        private static string needValidParmCodeStr = "|je_digit|sl_digit|dj_digit|zk_digit|";

        public static bool NeedValidParmCode(string paramcode)
        {
            if (string.IsNullOrEmpty(paramcode))
            {
                return false;
            }
            return needValidParmCodeStr.Contains(string.Format("|{0}|", paramcode.ToLower()));
        }
        
    }

}
