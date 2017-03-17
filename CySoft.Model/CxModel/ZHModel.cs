using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Td;
using CySoft.Utility;

namespace CySoft.Model.CxModel
{
    public class ZHModel:BaseCx
    {

        public override void CheckParam()
        {
            Handle();
            base.CheckParam();
            #region
            if (condition_1 == 0)
            {
                Errors.Add("请按A、B、C组号填对应的任选数量!");
                return ;
            }
            if (condition_2 == 0 && condition_3 > 0)
            {
                Errors.Add("请按A、B、C组号填对应的任选数量!");
                return ;
            }
            if (condition_1 > 0 && (string.IsNullOrEmpty(zh_sp_a) || !zh_sp_a.Contains("id_object")))
            {
                Errors.Add("请选择A组商品!");
                return ;
            }
            if (condition_1 <= 0 && !string.IsNullOrEmpty(zh_sp_a) && zh_sp_a.Contains("id_object"))
            {
                Errors.Add("请设置A组组合条件!");
                return;
            }
            if (condition_2 > 0 && (string.IsNullOrEmpty(zh_sp_b) || !zh_sp_b.Contains("id_object")))
            {
                Errors.Add("请选择B组商品!");
                return ;
            }
            if (condition_2 <= 0 && !string.IsNullOrEmpty(zh_sp_b) && zh_sp_b.Contains("id_object"))
            {
                Errors.Add("请设置B组组合条件!");
                return ;
            }
            if (condition_3 > 0 && (string.IsNullOrEmpty(zh_sp_c) || !zh_sp_c.Contains("id_object")))
            {
                Errors.Add("请选择C组商品!");
                return ;
            }
            if (condition_3 <= 0 && !string.IsNullOrEmpty(zh_sp_c) && zh_sp_c.Contains("id_object"))
            {
                Errors.Add("请设置C组组合条件!");
                return ;
            }
            if (condition_3 > 0 && condition_2 <= 0)
            {
                Errors.Add("请按A、B、C组号顺序设置商品组合!");
                return ;
            }
            if (result_1 == 0)
            {
                Errors.Add("请填写组合售价!");
                return ;
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
            Promote1.style = "zh";
            Promote1.bm_djlx = "CX020";
            Promote1.preferential = "zhj";
            //Promote1.condition_1 = condition_1;
            //Promote1.condition_2 = condition_2;
            // Promote1.condition_3 = condition_3;
            Promote1.result_1 = result_1;
            //Promote1.result_2 = result_2;
            //Promote1.result_3 = result_3;
            if (condition_1 > 0)
            {
                Promote1.zh_condition_1 = condition_1 + "A";
            }
            if (condition_2 > 0)
            {
                Promote1.zh_condition_1 += "+" + condition_2 + "B";
            }
            if (condition_3 > 0)
            {
                Promote1.zh_condition_1 += "+" + condition_3 + "C";
            }
        }

        public override void HandlePromote2List()
        {
            var date = DateTime.Now;
            var zhaList = JSON.Deserialize<List<Td_Promote_2>>(zh_sp_a);
            if (zhaList.Any())
            {
                for (int i = 0; i < zhaList.Count; i++)
                {
                    var p = zhaList[i];
                    p.id = Guid.NewGuid().ToString();
                    p.zh_group = "A";
                    p.id_masteruser = id_masteruser;
                    p.rq_create = date;
                    p.sort_id = Promote2List.Count+i + 1;
                    p.id_bill = id;
                }
                Promote2List.AddRange(zhaList);
            }

            var zhbList = JSON.Deserialize<List<Td_Promote_2>>(zh_sp_b);
            if (zhbList.Any())
            {
                for (int i = 0; i < zhbList.Count; i++)
                {
                    var p = zhbList[i];
                    p.id = Guid.NewGuid().ToString();
                    p.zh_group = "B";
                    p.id_masteruser = id_masteruser;
                    p.rq_create = date;
                    p.sort_id = Promote2List.Count + i + 1;
                    p.id_bill = id;
                }
                Promote2List.AddRange(zhbList);
            }

            var zhcList = JSON.Deserialize<List<Td_Promote_2>>(zh_sp_c);
            if (zhcList.Any())
            {
                for (int i = 0; i < zhcList.Count; i++)
                {
                    var p = zhcList[i];
                    p.id = Guid.NewGuid().ToString();
                    p.zh_group = "C";
                    p.id_masteruser = id_masteruser;
                    p.rq_create = date;
                    p.sort_id = Promote2List.Count + i + 1;
                    p.id_bill = id;
                }
                Promote2List.AddRange(zhcList);
            }

        }



    }
}
