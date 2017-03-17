using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CySoft.Model.Other;
using CySoft.Model.Td;
using CySoft.Utility;

namespace CySoft.Model.CxModel
{
    public  class BaseCx : PromoteViewModel
    {

        public BaseCx()
        {
            Errors = new List<string>();
            Promote1=new Td_Promote_1();
            Promote2List = new List<Td_Promote_2>();
            PromoteShopList=new List<Td_Promote_Shop>();
        }

        public bool CheckState()
        {
            CheckParam();
            return !Errors.Any();
        }
        

        public List<string> Errors { get; set; }

        public Td_Promote_1 Promote1 { get; set; }

        public List<Td_Promote_2> Promote2List { get; set; }

        public List<Td_Promote_Shop> PromoteShopList { get; set; }

        public virtual void Handle()
        {
            HandlePromote1();
            HandlePromote2List();
            HandleShopList();
        }
        /// <summary>
        /// 验证参数方法
        /// </summary>
        /// <returns></returns>
        public virtual void CheckParam()
        {
            if (string.IsNullOrEmpty(id_masteruser))
            {
                Errors.Add("主用户ID丢失,请重新登录!");
                return;
            }
            if (string.IsNullOrEmpty(cxzt))
            {
                Errors.Add("促销主题不能为空!");
                return;
            }
            if (day_b == DateTime.MinValue)
            {
                Errors.Add("促销开始日期不能为空!");
                return;
            }
            if (day_e == DateTime.MinValue)
            {
                Errors.Add("促销结束日期不能为空!");
                return;
            }
            var nowdate = DateTime.Now;
            if (day_b < new DateTime(nowdate.Year, nowdate.Month, nowdate.Day))
            {
                Errors.Add("促销开始日期已过!");
                return;
            }
            if (day_b > day_e)
            {
                Errors.Add("促销开始日期必须小于结束日期!");
                return;
            }
            if (string.IsNullOrEmpty(time_b))
            {
                Errors.Add("促销开始时间不能为空!");
                return;
            }
            if (string.IsNullOrEmpty(time_b))
            {
                Errors.Add("促销结束时间不能为空!");
                return;
            }
            var te = time_e.Replace(":", "").Replace("：", "");
            var tb = time_b.Replace(":", "").Replace("：", "");
            int int_te = 0;
            int int_tb = 0;
            int.TryParse(te, out int_te);
            int.TryParse(tb, out int_tb);
            if (int_tb > int_te)
            {
                Errors.Add("促销开始时间不能大于结束时间!");
                return;
            }
        }

        public virtual void HandlePromote1()
        {
            Promote1.flag_delete = (byte)Enums.Enums.FlagDelete.NoDelete;
            Promote1.id_masteruser = id_masteruser;
            id = Promote1.id = Guid.NewGuid().ToString();
            Promote1.id_shop = id_shop;
            Promote1.id_jbr = id_user;
            Promote1.day_b = day_b;
            Promote1.time_b = time_b.Replace(":", "").Replace("：", "");
            Promote1.time_e = time_e.Replace(":", "").Replace("：", "");
            Promote1.flag_rqfw = flag_rqfw;
            Promote1.weeks = weeks;
            Promote1.days = days;
            Promote1.rule_name = cxzt;
            Promote1.strategy = jsgz;
            var date = DateTime.Now;
            Promote1.rq = date;
            Promote1.day_e = new DateTime(day_e.Year, day_e.Month, day_e.Day, 23, 59, 59);
            Promote1.dh = string.Format("{0}{1}", date.ToString("yyyyMMddHHmmss"), date.Millisecond);
            Promote1.id_create = id_user;
            Promote1.rq_create = date;
            Promote1.bz = "";
            Promote1.flag_sh = (byte)Enums.Enums.FlagSh.UnSh;
            Promote1.id_hyfl_list = hylx;
            Regex regex = new Regex(@",.+,");
            if (regex.IsMatch(hylx) && hylx != ",all,")
            {
                Promote1.yxj_id = 3;
            }
            else
            {
                Promote1.yxj_id = 7;
            }
            Promote1.flag_cancel = (byte)Enums.Enums.FlagCancel.NoCancel;
            Promote1.examine = jsfs;
        }

        public virtual void HandlePromote2List(){}
        public virtual void HandleShopList()
        {
            var arr = id_shops.Split(',');
            var date = DateTime.Now;
            foreach (var a in arr)
            {
                PromoteShopList.Add(new Td_Promote_Shop()
                {
                    flag_delete = (byte)Enums.Enums.FlagDelete.NoDelete,
                    id = Guid.NewGuid().ToString(),
                    id_bill = id,
                    id_create = id_user,
                    id_masteruser = id_masteruser,
                    id_shop = a,
                    rq_create = date
                });
            }
        }


    }

    public class DPZKModel : BaseCx
    {

        public override void CheckParam()
        {
            Handle();
            base.CheckParam();
            if (Promote2List.Any())
            {
                Promote2List.ForEach(promote2 =>
                {
                    #region 折扣
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
                    if (promote2.condition_2 > 0 && promote2.condition_2 <= promote2.condition_1)
                    {
                        Errors.Add(string.Format("商品:第{0}行,规则二的条件必需大于规则一的条件!", promote2.sort_id));
                    }
                    if (promote2.condition_3 > 0 && promote2.condition_3 <= promote2.condition_2)
                    {
                        Errors.Add(string.Format("商品:第{0}行,规则三的条件不能小于规则二的条件!", promote2.sort_id));
                    }
                    if (promote2.result_2 > 0 && promote2.result_2 >= promote2.result_1)
                    {
                        Errors.Add(string.Format("商品:第{0}行,规则二折扣必需小于规则一折扣!", promote2.sort_id));
                    }
                    if (promote2.result_3 > 0 && promote2.result_3 >= promote2.result_2)
                    {
                        Errors.Add(string.Format("商品:第{0}行,规则三折扣必需小于规则二折扣!", promote2.sort_id));
                    }
                    if (promote2.result_1 > 1 || promote2.result_1 < 0)
                    {
                        Errors.Add(string.Format("商品:第{0}行,规则一的折扣只能设置0-1的数!", promote2.sort_id));
                    }
                    if (promote2.result_2 > 1 || promote2.result_2 < 0)
                    {
                        Errors.Add(string.Format("商品:第{0}行,规则二的折扣只能设置0-1的数!", promote2.sort_id));
                    }
                    if (promote2.result_3 > 1 || promote2.result_3 < 0)
                    {
                        Errors.Add(string.Format("商品:第{0}行,规则三的折扣只能设置0-1的数!", promote2.sort_id));
                    }
                    #endregion
                });
            }
            else
            {
                Errors.Add("请选择商品!");
                return;
            }
            if (!PromoteShopList.Any())
            {
                Errors.Add("请选择促销门店!");
            }
        }

        public override void HandlePromote1()
        {
            base.HandlePromote1();
            Promote1.style = "dp";
            Promote1.bm_djlx = "CX110";
            Promote1.preferential = "zk";
            Promote1.condition_1 = condition_1;
            Promote1.condition_2 = condition_2;
            Promote1.condition_3 = condition_3;
            Promote1.result_1 = result_1;
            Promote1.result_2 = result_2;
            Promote1.result_3 = result_3;
        }

        public override void HandlePromote2List()
        {
            Promote2List = JSON.Deserialize<List<Td_Promote_2>>(sp);
            var date = DateTime.Now;
            if (Promote2List.Any())
            {
                for (int i = 0; i < Promote2List.Count; i++)
                {
                    var p = Promote2List[i];
                    p.id = Guid.NewGuid().ToString();
                    p.zh_group = "A";
                    p.id_masteruser = id_masteruser;
                    p.rq_create = date;
                    p.sort_id = i + 1;
                    p.id_bill = id;
                }
            }
        }

    }

}
