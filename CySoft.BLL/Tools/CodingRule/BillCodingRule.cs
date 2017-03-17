using System;
using System.Collections;
using CySoft.Frame.Core;
using CySoft.Frame.Utility;
using CySoft.Model.Ts;

namespace CySoft.BLL.Tools.CodingRule
{
    public class BillCodingRule : AbstractCodingRule
    {
        public override BaseResult SetCoding(object entity, Type type = null, string setPropertyName = "dh")
        {
            BaseResult br = new BaseResult();
            try
            {
                if (type == null)
                {
                    type = entity.GetType();
                }
                string tableName = GetTableName(type);
                long xh = 1;
                long id_master = CySoft.Frame.Common.TypeConvert.ToInt64((entity.GetValue("id_gys") ?? "0"),0);
                Hashtable param = new Hashtable();
                param.Add("coding", tableName);
                param.Add("lx", "2");
                Ts_Codingrule codingrule = DAL.GetItem<Ts_Codingrule>(typeof(Ts_Codingrule), param);
                if (codingrule == null)
                {
                    br.Success = false;
                    br.Message.Clear();
                    br.Message.Add(string.Format("对象'{0}'没有编码规则！请联系管理员！", tableName));
                    br.Level = ErrorLevel.Warning;
                    return br;
                }
                string ymd = DAL.GetDbDateTime().ToString("yyyyMMdd");
                param.Clear();
                param.Add("billcode", tableName);
                param.Add("ymd", ymd);
                param.Add("id_master", id_master);

                if (DAL.GetCount(typeof(Ts_Max_Billcode), param) > 0)
                {                   
                    Ts_Max_Billcode model = DAL.GetItem<Ts_Max_Billcode>(typeof(Ts_Max_Billcode), param);
                    xh = model.max_dh.Value + codingrule.step.Value;
                    param.Add("new_max_dh", xh);
                    DAL.UpdatePart(typeof(Ts_Max_Billcode), param);
                }
                else
                {
                    Ts_Max_Billcode newItem = new Ts_Max_Billcode();
                    newItem.billcode = tableName;
                    newItem.ymd = ymd;
                    newItem.max_dh = xh;
                    newItem.id_master = id_master;
                    DAL.Add(newItem);
                }                

                int codeLength = GetCodeLength("", codingrule.length.Value, 1) + 10;
                string bm = string.Empty;
                if (!id_master.Equals(0)) 
                {
                    bm = id_master.ToString().Length > 7 ? id_master.ToString() : string.Format("{0:0000000}", id_master);
                    codeLength += bm.Length;
                }
                string format = this.GetFormat(codingrule.prefix + ymd + bm, codeLength);
                string coding = String.Format(format, xh);
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
