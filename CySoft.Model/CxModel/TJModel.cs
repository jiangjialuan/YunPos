using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Td;
using CySoft.Utility;

namespace CySoft.Model.CxModel
{
    public class TJModel:BaseCx
    {

        public override void CheckParam()
        {
            Handle();
            base.CheckParam();
            #region
            if (string.IsNullOrEmpty(sp)||!sp.Contains("id_object"))
            {
                Errors.Add("请选择商品!");
                return ;
            }
            if (Promote2List.Any())
            {
                Promote2List.ForEach(promote2 =>
                {
                    #region 特价
                    #region
                    if (promote2.condition_1 == 0 || promote2.result_1 == 0)
                    {
                        Errors.Add(string.Format("商品:第{0}行,请按顺序一、二、三设置规则!", promote2.sort_id));
                    }
                    if ((promote2.condition_2 == 0 && promote2.condition_3 != 0) || (promote2.result_2 == 0 && promote2.result_3 != 0))
                    {
                        Errors.Add(string.Format("商品:第{0}行,请按顺序一、二、三设置规则!", promote2.sort_id));
                    }
                    if ((promote2.condition_2 == 0 && promote2.result_2 != 0) || (promote2.condition_2 != 0 && promote2.result_2 == 0))
                    {
                        Errors.Add(string.Format("商品:第{0}行,规则二设置不完整!", promote2.sort_id));
                    }
                    if ((promote2.condition_3 == 0 && promote2.result_3 != 0) || (promote2.condition_3 != 0 && promote2.result_3 == 0))
                    {
                        Errors.Add(string.Format("商品:第{0}行,规则三设置不完整!", promote2.sort_id));
                    }
                    #endregion
                    if (promote2.condition_2 > 0 && promote2.condition_2 <= promote2.condition_1)
                    {
                        Errors.Add(string.Format("商品:第{0}行,规则二的条件必需大于规则一的条件!", promote2.sort_id));
                    }
                    if (promote2.condition_3 > 0 && promote2.condition_3 <= promote2.condition_2)
                    {
                        Errors.Add(string.Format("商品:第{0}行,规则三的条件必需大于规则二的条件!", promote2.sort_id));
                    }
                    #endregion
                });
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
            Promote1.bm_djlx = "CX010";
            Promote1.preferential = "tj";
            Promote1.condition_1 = condition_1;
            Promote1.condition_2 = condition_2;
            Promote1.condition_3 = condition_3;
            Promote1.result_1 = result_1;
            Promote1.result_2 = result_2;
            Promote1.result_3 = result_3;
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
