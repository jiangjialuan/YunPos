using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Td;
using CySoft.Utility;

namespace CySoft.Model.CxModel
{
    public class SDZKModel:BaseCx
    {
        public override void CheckParam()
        {
            Handle();
            base.CheckParam();
            #region 时段促销
            if (jsfs != "sl")
            {
                Errors.Add("时段促销结算方式只能为数量");
                return ;
            }
            if (jsgz != "man")
            {
                Errors.Add("时段促销结算规则只能为满");
                return ;
            }
            if (sd1 == -1)
            {
                Errors.Add("规则一的时点未设置!");
            }
            if (result_1 == 0)
            {
                Errors.Add("规则一的折扣未设置!");
                return ;
            }
            if (sd3 > -1 && sd2 == -1)
            {
                Errors.Add("按规则一、二、三顺序设置时点!");
                return ;
            }
            if (sd2 > -1 && result_2 == 0)
            {
                Errors.Add("规则二的折扣未设置!");
                return ;
            }
            if (sd3 > -1 && result_3 == 0)
            {
                Errors.Add("规则三的折扣未设置!");
                return ;
            }
            if (result_1 > 1 || result_1 < 0)
            {
                Errors.Add("规则一的折扣只能设置0-1的数!");
                return ;
            }
            if (result_1 > 2 || result_2 < 0)
            {
                Errors.Add("规则二的折扣只能设置0-1的数!");
                return ;
            }
            if (result_3 > 2 || result_3 < 0)
            {
                Errors.Add("规则三的折扣只能设置0-1的数!");
                return ;
            }
            var te = time_e.Replace(":", "").Replace("：", "");
            var tb = time_b.Replace(":", "").Replace("：", "");
            int int_te = 0;
            int int_tb = 0;
            int.TryParse(te, out int_te);
            int.TryParse(tb, out int_tb);
            if (sd2 <= 0 && int_te <= sd1)
            {
                Errors.Add("结束时间必需大于规则一的时点!");
                return ;
            }
            if (sd2 > 0 && sd2 <= sd1)
            {
                Errors.Add("规则二的时点必需大于规则一的时点!");
                return ;
            }
            if (sd3 <= 0 && sd2 > 0 && int_te <= sd2)
            {
                Errors.Add("结束时间必需大于规则二的时点!");
                return ;
            }
            if (sd3 > 0 && sd3 <= sd2)
            {
                Errors.Add("规则三的时点必需大于规则二的时点!");
                return ;
            }
            if (sd3 > 0 && int_te <= sd3)
            {
                Errors.Add("结束时间必需大于规则三的时点!");
                return ;
            }
            if (string.IsNullOrEmpty(sp)||!sp.Contains("id_object"))
            {
                Errors.Add("请选择商品!");
            }
            #endregion
            if (!PromoteShopList.Any())
            {
                Errors.Add("请选择促销门店!");
            }
        }

        public override void HandlePromote1()
        {
            base.HandlePromote1();
            Promote1.style = "dp";
            Promote1.bm_djlx = "CX140";
            Promote1.preferential = "zk";
            Promote1.condition_1 = condition_1;
            Promote1.condition_2 = condition_2;
            Promote1.condition_3 = condition_3;
            Promote1.result_1 = result_1;
            Promote1.result_2 = result_2;
            Promote1.result_3 = result_3;
            Promote1.sd1 = string.Format("{0}", sd1 < 0 ? "" : sd1.ToString());
            Promote1.sd2 = string.Format("{0}", sd2 < 0 ? "" : sd2.ToString());
            Promote1.sd3 = string.Format("{0}", sd3 < 0 ? "" : sd3.ToString());
        }

        public override void HandlePromote2List()
        {
            var list = JSON.Deserialize<List<Td_Promote_2>>(sp);
            var date = DateTime.Now;
            if (list.Any())
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var p = list[i];
                    p.id = Guid.NewGuid().ToString();
                    p.zh_group = "A";
                    p.id_masteruser = id_masteruser;
                    p.rq_create = date;
                    p.sort_id = i + 1;
                    p.id_bill = id;
                }
                Promote2List.AddRange(list);
            }
        }


    }
}
