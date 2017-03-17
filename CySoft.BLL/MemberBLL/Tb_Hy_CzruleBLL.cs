#region Imports
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.IDAL;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using CySoft.Model.Ts;
using CySoft.Model.Tz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Utility;

#endregion

namespace CySoft.BLL.MemberBLL
{
    public class Tb_Hy_CzruleBLL : BaseBLL
    {

        #region Get
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            try
            {
                Tb_Hy_Czrule_Query_DetailModel result = new Tb_Hy_Czrule_Query_DetailModel();
                var head = DAL.GetItem<Tb_Hy_Czrule_Query>(typeof(Tb_Hy_Czrule), param);
                if (head != null && !string.IsNullOrEmpty(head.id))
                {
                    head.day_b_str = head.day_b == null ? "" : ((DateTime)head.day_b).ToString("yyyy-MM-dd");
                    head.day_e_str = head.day_e == null ? "" : ((DateTime)head.day_e).ToString("yyyy-MM-dd");
                    result.head = head;
                    param.Clear();
                    param.Add("id_bill", head.id_bill);
                    var body = DAL.QueryList<Tb_Hy_Czrule_Zssp_Query>(typeof(Tb_Hy_Czrule_Zssp), param);
                    result.body = body.ToList();
                    res.Data = result;
                }
                else
                {
                    res.Success = false;
                    res.Message.Add("未找到此规则信息!");
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message.Add("操作异常!");
            }
            return res;
        }
        #endregion

        #region GetAll
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
            param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            var head_list = DAL.QueryList<Tb_Hy_Czrule_Query>(typeof(Tb_Hy_Czrule), param).ToList();

            if (param.ContainsKey("query_body") && param["query_body"].ToString() == "1")
            {
                string id_bills = "";

                var q = from p in head_list group p by p.id_bill into g select new { g.Key };

                if (q == null || q.Count() <= 0)
                {
                    result.Data = head_list;
                    return result;
                }

                foreach (var item in q)
                {
                    if (!string.IsNullOrEmpty(id_bills))
                        id_bills += "," + item.Key;
                    else
                        id_bills = item.Key;
                }

                string[] billIDs = id_bills.Split(',');

                param.Clear();
                param.Add("id_billList", billIDs.ToArray());
                param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                var body_list = DAL.QueryList<Tb_Hy_Czrule_Zssp_Query>(typeof(Tb_Hy_Czrule_Zssp), param).ToList();

                int sl_digit = 2;
                if (param.ContainsKey("digit"))
                {
                    var digit = (Hashtable)param["digit"] ?? new Hashtable();
                    if (digit.ContainsKey("sl_digit"))
                        int.TryParse(digit["sl_digit"].ToString(), out sl_digit);
                }

                foreach (var head in head_list)
                {
                    var splist = body_list.Where(d => d.id_bill == head.id_bill).ToList();
                    foreach (var body in splist)
                    {
                        head.spmc += body.mc + "[" + body.sl.Digit(sl_digit) + " " + body.dw + "],";
                    }
                    head.body_list = splist;
                }
            }
            result.Data = head_list;
            return result;
        }
        #endregion



      





    }
}