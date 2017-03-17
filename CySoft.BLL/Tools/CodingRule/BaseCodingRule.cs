using System;
using System.Collections;
using CySoft.Frame.Core;
using CySoft.Frame.Utility;
using CySoft.Model.Ts;

namespace CySoft.BLL.Tools.CodingRule
{
    public class BaseCodingRule : AbstractCodingRule
    {
        public override BaseResult SetCoding(object entity, Type type = null, string setPropertyName = "bm")
        {
            BaseResult br = new BaseResult();
            try
            {
                if (type == null)
                {
                    type = entity.GetType();
                }
                string tableName = GetTableName(type);
                bool flag_add = true;
                bool flag_hasxh = false;
                if (entity.HasProperty("xh"))
                {
                    flag_hasxh = true;
                }
                long id = Convert.ToInt64(entity.GetValue("id"));
                long xh = 0;
                Hashtable param = new Hashtable();
                param.Add("id", id);
                if (DAL.GetCount(type,param) > 0)
                {
                    flag_add = false;
                    if (flag_hasxh)
                    {
                        xh = Convert.ToInt64(entity.GetValue("xh"));
                    }
                }
                string coding = entity.GetValue(setPropertyName).ToString().Trim();
                
                param.Clear();
                param.Add("coding", tableName);
                param.Add("lx", "1");
                Ts_Codingrule codingrule = DAL.GetItem<Ts_Codingrule>(typeof(Ts_Codingrule), param);
                if (codingrule == null)
                {
                    br.Success = false;
                    br.Message.Clear();
                    br.Message.Add(string.Format("对象'{0}'没有编码规则！请联系管理员！", tableName));
                    br.Level = ErrorLevel.Warning;
                    return br;
                }
                int codeLength = GetCodeLength(codingrule.prefix, codingrule.length.Value, 1);
                if (!String.IsNullOrEmpty(coding))
                {
                    if (flag_add && !coding.StartsWith(codingrule.prefix))
                    {
                        coding = codingrule.prefix + coding;
                    }
                    if (coding.Length != codeLength)
                    {
                        br.Success = false;
                        br.Message.Clear();
                        br.Message.Add(string.Format("本级的编码长度为 '{0}' 位,当前编码 '{1}' 不满足系统定义！请检查后再试！", codeLength, coding));
                        br.Level = ErrorLevel.Warning;
                        return br;
                    }
                    param.Clear();
                    if (!flag_add)
                    {
                        param.Add("not_id", id);
                    }
                    if (flag_hasxh)
                    {
                        object id_gys = entity.GetValue("id_gys");
                        if (id_gys != null)
                        {
                            param.Add("id_gys", entity.GetValue("id_gys"));
                        }
                    }
                    param.Add(setPropertyName, coding);
                    if (DAL.GetCount(type, param) >= 1)
                    {
                        br.Success = false;
                        br.Message.Clear();
                        br.Message.Add(string.Format("编码[{0}]已存在，请使用其他编码！", coding));
                        br.Level = ErrorLevel.Warning;
                        return br;
                    }
                }
                else if (flag_add)
                {
                    if (flag_hasxh)
                    {
                        xh = DAL.GetNextXh(type) - 1;
                        xh += codingrule.step.Value;
                    }
                    else
                    {
                        param.Clear();
                        param.Add("table_name", tableName);
                        if (DAL.GetCount(typeof(Ts_Number_Max), param) > 0)
                        {
                            param.Clear();
                            param.Add("table_name", tableName);
                            Ts_Number_Max model = DAL.GetItem<Ts_Number_Max>(typeof(Ts_Number_Max), param);
                            xh = model.xh_max.Value + codingrule.step.Value;
                        }
                        else
                        {
                            Ts_Number_Max newItem = new Ts_Number_Max();
                            newItem.table_name = tableName;
                            newItem.xh_max = 1;
                            DAL.Add(newItem);
                            xh = 1;
                        }
                    }
                    string format = this.GetFormat(codingrule.prefix, codeLength);
                    param.Clear();
                    if (flag_hasxh)
                    {
                        object id_gys = entity.GetValue("id_gys");
                        if (id_gys != null)
                        {
                            param.Add("id_gys", entity.GetValue("id_gys"));
                        }
                    }
                    while (true)
                    {
                        coding = String.Format(format, xh);
                        if (coding.Length != codeLength)
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add("系统的编码资源已用完, 请检查编码规则或手工录入编码");
                            br.Level = ErrorLevel.Warning;
                            return br;
                        }
                        param[setPropertyName] = coding;
                        if (DAL.GetCount(type, param) < 1)
                        {
                            break;
                        }
                        xh += codingrule.step.Value;
                    }
                }
                else
                {
                    //br.Success = false;
                    //br.Message.Clear();
                    //br.Message.Add("编码不能为空！");
                    //br.Level = ErrorLevel.Warning;
                    //return br;
                }
                entity.SetValue("id", id);
                if (flag_hasxh)
                {
                    entity.SetValue("xh", xh);
                }
                else
                {
                    param.Clear();
                    param.Add("table_name", tableName);
                    param.Add("new_xh_max", xh);
                    DAL.UpdatePart(typeof(Ts_Number_Max), param);
                }
                entity.SetValue(setPropertyName, coding);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            br.Success = true;
            return br;
        }
    }
}
