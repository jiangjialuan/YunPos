using System;
using System.Collections;
using CySoft.Frame.Core;
using CySoft.Frame.Utility;
using CySoft.Model.Ts;

namespace CySoft.BLL.Tools.CodingRule
{
    public class TreeCodingRule : AbstractCodingRule
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
                int id = Convert.ToInt32(entity.GetValue("id"));
                int xh = 0;
                Hashtable param = new Hashtable();
                param.Add("id", id);
                if (DAL.GetCount(type, param) > 0)
                {
                    flag_add = false;
                    xh = Convert.ToInt32(entity.GetValue("xh"));
                }
                string coding = entity.GetValue(setPropertyName).ToString().Trim();
                int fatherId = Convert.ToInt32(entity.GetValue("fatherId"));
                string path = string.Empty;
                param.Clear();
                param.Add("id", fatherId);
                object father = DAL.GetItem(type, param) ?? new { path = "/0" };
                path = father.GetValue("path") + "/" + id;
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
                int curlevel = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Length - 1;
                if (curlevel > codingrule.level)
                {
                    br.Success = false;
                    br.Message.Clear();
                    br.Message.Add(string.Format("编码规则限制为'{0}'级！请检查后再试！", codingrule.level));
                    br.Level = ErrorLevel.Warning;
                    return br;
                }
                int codeLength = GetCodeLength(codingrule.prefix, codingrule.length.Value, curlevel);
                if (!string.IsNullOrEmpty(coding))
                {
                    string fatherBm = (father.GetValue(setPropertyName) ?? "").ToString();
                    if (String.IsNullOrEmpty(fatherBm))
                    {
                        fatherBm = codingrule.prefix;
                    }
                    if (flag_add)
                    {
                        coding = fatherBm + coding;
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
                    object id_gys = entity.GetValue("id_gys");
                    if (id_gys != null)
                    {
                        param.Add("id_gys", entity.GetValue("id_gys"));
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
                    xh = DAL.GetNextXh(type, param) - 1;
                    xh += codingrule.step.Value;
                    string fatherBm = (father.GetValue(setPropertyName) ?? "").ToString();
                    if (String.IsNullOrEmpty(fatherBm))
                    {
                        fatherBm = codingrule.prefix;
                    }
                    string format = this.GetFormat(fatherBm, codeLength);
                    param.Clear();
                    object id_gys = entity.GetValue("id_gys");
                    if (id_gys != null)
                    {
                        param.Add("id_gys", entity.GetValue("id_gys"));
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
                if (entity.GetValue("xh") is Nullable<long>)
                {
                    entity.SetValue("xh", (long?)xh);
                }
                else if (entity.GetValue("xh") is Nullable<int>)
                {
                    entity.SetValue("xh", (int?)xh);
                }
                else if (entity.GetValue("xh") is long)
                {
                    entity.SetValue("xh", (long)xh);
                }
                else
                {
                    entity.SetValue("xh", xh);
                }
                entity.SetValue(setPropertyName, coding);
                entity.SetValue("path", path);
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
