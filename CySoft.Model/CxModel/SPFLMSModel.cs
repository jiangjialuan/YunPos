using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Td;
using CySoft.Utility;

namespace CySoft.Model.CxModel
{
    public class SPFLMSModel:BaseCx
    {
        public override void CheckParam()
        {
            Handle();
            base.CheckParam();
            #region
            if (jsfs != "je")
            {
                Errors.Add("只能以金额做为结算方式!");
                return ;
            }
            if (condition_1 == 0)
            {
                Errors.Add("请按顺序一、二、三设置促销规则!");
                return ;
            }
            if ((condition_2 == 0 && condition_3 != 0))
            {
                Errors.Add("请按顺序一、二、三设置促销规则!");
                return ;
            }
            if (result_1 == 0 && sl_largess_1 == 0)
            {
                Errors.Add("规则一设置不完整!");
                return ;
            }
            if ((condition_2 != 0 && result_2 == 0 && sl_largess_2 == 0)
                || (condition_2 == 0 && (result_2 != 0 || sl_largess_2 != 0)))
            {
                Errors.Add("规则二设置不完整!");
                return ;
            }
            if ((condition_3 != 0 && result_3 == 0 && sl_largess_3 == 0)
                || (condition_3 == 0 && (result_3 != 0 || sl_largess_3 != 0)))
            {
                Errors.Add("规则三设置不完整!");
                return ;
            }
            if (string.IsNullOrEmpty(sp) || !sp.Contains("id_object"))
            {
                Errors.Add("请选择商品分类!");
                return;
            }
            if (sl_largess_1 > 0 && (string.IsNullOrEmpty(zs_sp_1) || !zs_sp_1.Contains("id_object")))
            {
                Errors.Add("请选择赠送商品1");
                return ;
            }
            if (sl_largess_1 <= 0 && !string.IsNullOrEmpty(zs_sp_1) && zs_sp_1.Contains("id_object"))
            {
                Errors.Add("请设置规则一的赠送数量");
                return;
            }
            if (sl_largess_2 > 0 && (string.IsNullOrEmpty(zs_sp_2) || !zs_sp_2.Contains("id_object")))
            {
                Errors.Add("请选择赠送商品2");
                return ;
            }
            if (sl_largess_2 <= 0 && !string.IsNullOrEmpty(zs_sp_2) && zs_sp_2.Contains("id_object"))
            {
                Errors.Add("请设置规则二的赠送数量");
                return;
            }
            if (sl_largess_3 > 0 && (string.IsNullOrEmpty(zs_sp_3) || !zs_sp_3.Contains("id_object")))
            {
                Errors.Add("请选择赠送商品3");
                return ;
            }
            if (sl_largess_3 <= 0 && !string.IsNullOrEmpty(zs_sp_3) && zs_sp_3.Contains("id_object"))
            {
                Errors.Add("请设置规则三的赠送数量");
                return;
            }
            if (sl_largess_3 == 0
                && sl_largess_2 == 0
                && sl_largess_1 == 0)
            {
                if (condition_2 > 0 && condition_2 <= condition_1)
                {
                    Errors.Add("规则二的条件数值必需大于规则一的条件数值!");
                    return ;
                }
                if (condition_3 > 0 && condition_3 <= condition_2)
                {
                    Errors.Add("规则三的条件数值必需大于规则二的条件数值!");
                    return ;
                }
                if (result_2 > 0 && result_2 <= result_1)
                {
                    Errors.Add("规则二的优惠金额必需大于规则一的优惠金额!");
                    return ;
                }
                if (result_3 > 0 && result_3 <= result_2)
                {
                    Errors.Add("规则三的优惠金额必需大于规则二的优惠金额!");
                    return ;
                }
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
            Promote1.style = "spfl";
            Promote1.bm_djlx = "CX230";
            Promote1.preferential = "yh";
            Promote1.condition_1 = condition_1;
            Promote1.condition_2 = condition_2;
            Promote1.condition_3 = condition_3;
            Promote1.result_1 = result_1;
            Promote1.result_2 = result_2;
            Promote1.result_3 = result_3;
            Promote1.sl_largess_1 = sl_largess_1;
            Promote1.sl_largess_2 = sl_largess_2;
            Promote1.sl_largess_3 = sl_largess_3;
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
            var lwList = JSON.Deserialize<List<Td_Promote_2>>(lwsp);
            if (lwList.Any())
            {
                for (int i = 0; i < lwList.Count; i++)
                {
                    var p = lwList[i];
                    p.id = Guid.NewGuid().ToString();
                    p.zh_group = "lw";
                    p.id_masteruser = id_masteruser;
                    p.rq_create = date;
                    p.sort_id = Promote2List.Count+i + 1;
                    p.id_bill = id;
                }
                Promote2List.AddRange(lwList);
            }
            var zs1List = JSON.Deserialize<List<Td_Promote_2>>(zs_sp_1);
            if (zs1List.Any())
            {
                for (int i = 0; i < zs1List.Count; i++)
                {
                    var item = zs1List[i];
                    item.id_masteruser = id_masteruser;
                    item.id = Guid.NewGuid().ToString();
                    item.id_bill = id;
                    item.sort_id = Promote2List.Count + i + 1;
                    item.zh_group = "zs1";
                    item.rq_create = date;
                }
                Promote2List.AddRange(zs1List);
            }
            var zs2List = JSON.Deserialize<List<Td_Promote_2>>(zs_sp_2);
            if (zs2List.Any())
            {
                for (int i = 0; i < zs2List.Count; i++)
                {
                    var item = zs2List[i];
                    item.id_masteruser = id_masteruser;
                    item.id = Guid.NewGuid().ToString();
                    item.id_bill = id;
                    item.sort_id = Promote2List.Count + i + 1;
                    item.zh_group = "zs2";
                    item.rq_create = date;
                }
                Promote2List.AddRange(zs2List);
            }
            var zs3List = JSON.Deserialize<List<Td_Promote_2>>(zs_sp_3);
            if (zs3List.Any())
            {
                for (int i = 0; i < zs3List.Count; i++)
                {
                    var item = zs3List[i];
                    item.id_masteruser = id_masteruser;
                    item.id = Guid.NewGuid().ToString();
                    item.id_bill = id;
                    item.sort_id = Promote2List.Count + i + 1;
                    item.zh_group = "zs3";
                    item.rq_create = date;
                }
                Promote2List.AddRange(zs3List);
            }


        }

    }
}
