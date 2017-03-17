using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.IDAL;
using CySoft.Model.CxModel;
using CySoft.Model.Enums;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Model.Td;
using CySoft.Utility;

namespace CySoft.BLL.PromoteBLL
{
    public class Td_Promote_1BLL : BaseBLL
    {
        public ITd_Promote_2DAL Td_Promote_2DAL { get; set; }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Td_Promote_1), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Td_Promote_1WithUserName>(typeof(Td_Promote_1), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult res = new BaseResult() { Success = true };
            var model = entity as PromoteViewModel;
            Hashtable param = new Hashtable();
            if (model!=null&&model.op=="copy"&&!string.IsNullOrEmpty(model.id))
            {
                param.Add("id",model.id);
                var old_model = DAL.GetItem<Td_Promote_1>(typeof(Td_Promote_1), param);
                if (old_model!=null)
                {
                    model.preferential = old_model.preferential;
                    model.spxz = old_model.style;
                    model.bm_djlx = old_model.bm_djlx;
                    if (old_model.bm_djlx=="CX020")
                    {
                        model.jsfs = "zhsp";
                        model.jsgz = "mei";
                    }
                }
            }
            if (!CheckParam(model, res))
            {
                return res;
            }
            var promoteModel = CreateTdPromote1(model);
            HandleStyle(res, model, promoteModel);
            if (res.Success)
            {
                HandleShop(promoteModel.id, model.id_shops, model.id_masteruser, model.id_user);
                DAL.Add(promoteModel);

                if (model.AutoAudit)
                {
                    param.Clear();
                    param.Add("id_masteruser", model.id_masteruser);
                    param.Add("id_user", model.id_user);
                    param.Add("id", model.id);
                    Sh<Td_Promote_1>(res, param, "p_promote_sh");
                }
            }
            return res;
        }

        //[Transaction]
        //public override BaseResult Add(dynamic entity)
        //{
        //    BaseResult res = new BaseResult() { Success = true };
        //    var model = entity as BaseCx;
        //    if (model==null)
        //    {
        //        res.Success = false;
        //        res.Message.Add("参数有误!");
        //        return res;
        //    }
        //    if (model.CheckState())
        //    {
        //        DAL.Add(model.Promote1);
        //        if (model.Promote2List.Any())
        //        {
        //            DAL.AddRange(model.Promote2List);
        //        }
        //        if (model.PromoteShopList.Any())
        //        {
        //            DAL.AddRange(model.PromoteShopList);
        //        }
        //    }
        //    else
        //    {
        //        res.Success = false;
        //        res.Message.Add(model.Errors.FirstOrDefault());
        //    }
        //    return res;
        //}
        #region 辅助方法
        /// <summary>
        /// 创建Td_Promote_1实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Td_Promote_1 CreateTdPromote1(PromoteViewModel model)
        {
            Td_Promote_1 promoteModel = new Td_Promote_1();
            promoteModel.id_masteruser = model.id_masteruser;
            promoteModel.id = GetGuid;
            var date = DateTime.Now;
            promoteModel.rq = date;
            promoteModel.id_shop = model.id_shop;
            promoteModel.id_jbr = model.id_user;
            promoteModel.day_b = model.day_b;
            promoteModel.day_e = new DateTime(model.day_e.Year, model.day_e.Month, model.day_e.Day, 23, 59, 59);
            promoteModel.time_b = model.time_b.Replace(":", "").Replace("：", "");
            promoteModel.time_e = model.time_e.Replace(":", "").Replace("：", "");
            promoteModel.flag_rqfw = model.flag_rqfw;
            promoteModel.weeks = model.weeks;//1,2,3,4....
            promoteModel.days = model.days;
            promoteModel.rule_name = model.cxzt;
            promoteModel.style = model.spxz;//dp单品/dzsp单组/zh组合/bill整单/spfl整类
            promoteModel.examine = model.jsfs;//sl数量/je金额/xl限量/zhj
            promoteModel.bm_djlx = model.bm_djlx;
            promoteModel.preferential = model.preferential;//zk打折/tj特价/yh优惠/zs赠送/jjhg加价换购
            promoteModel.strategy = model.jsgz;//mei每\man满
            promoteModel.dh = GetNewDH(model.id_masteruser, model.id_shop, Enums.FlagDJLX.DHJH);//date.ToString("yyyyMMddHHmmss") + date.Millisecond;
            promoteModel.flag_cancel = (byte)Enums.FlagCancel.NoCancel;
            if (model.bm_djlx == "CX020")
            {
                if (model.condition_1 > 0)
                {
                    promoteModel.zh_condition_1 = model.condition_1 + "A";
                }
                if (model.condition_2 > 0)
                {
                    promoteModel.zh_condition_1 += "+" + model.condition_2 + "B";
                }
                if (model.condition_3 > 0)
                {
                    promoteModel.zh_condition_1 += "+" + model.condition_3 + "C";
                }
                promoteModel.result_1 = model.result_1;
            }
            else
            {
                promoteModel.condition_1 = model.condition_1;
                promoteModel.condition_2 = model.condition_2;
                promoteModel.condition_3 = model.condition_3;
                promoteModel.result_1 = model.result_1;
                promoteModel.result_2 = model.result_2;
                promoteModel.result_3 = model.result_3;
            }
            promoteModel.sl_largess_1 = model.sl_largess_1;
            promoteModel.sl_largess_2 = model.sl_largess_2;
            promoteModel.sl_largess_3 = model.sl_largess_3;
            promoteModel.flag_sh = (byte)Enums.FlagSh.UnSh;
            promoteModel.id_hyfl_list = model.hylx;
            Regex regex = new Regex(@",.+,");
            if (regex.IsMatch(model.hylx) && model.hylx != ",all,")
            {
                promoteModel.yxj_id = 3;
            }
            else
            {
                promoteModel.yxj_id = 7;
            }
            promoteModel.flag_delete = (byte)Enums.FlagDelete.NoDelete;
            promoteModel.id_create = model.id_user;
            promoteModel.rq_create = DateTime.Now;
            promoteModel.sd1 = string.Format("{0}", model.sd1 < 0 ? "" : model.sd1.ToString());
            promoteModel.sd2 = string.Format("{0}", model.sd2 < 0 ? "" : model.sd2.ToString());
            promoteModel.sd3 = string.Format("{0}", model.sd3 < 0 ? "" : model.sd3.ToString());
            promoteModel.bz = "";

            promoteModel.flag_zsz = model.flag_zsz;
            model.id = promoteModel.id;
            return promoteModel;
        }

        private string GetPreferential(string bm_djlx)
        {
            //zk打折/tj特价/yh优惠/zs赠送/jjhg加价换购/zhj组合价
            switch (bm_djlx)
            {
                case "CX001":
                    return "zk";
                case "CX002":
                    return "tj";
                case "CX003":
                    return "yh";
                case "CX004":
                    return "zhj";
            }
            return "";
        }
        /// <summary>
        /// 检查Td_Promote_1对象的参数合法性
        /// </summary>
        /// <param name="model"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        private bool CheckParam(PromoteViewModel model, BaseResult res)
        {

            #region 公共参数
            if (model == null)
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return false;
            }
            if (string.IsNullOrEmpty(model.cxzt))
            {
                res.Success = false;
                res.Message.Add("促销主题不能为空!");
                return false;
            }
            if (model.day_b == DateTime.MinValue)
            {
                res.Success = false;
                res.Message.Add("促销开始日期不能为空!");
                return false;
            }
            if (model.day_e == DateTime.MinValue)
            {
                res.Success = false;
                res.Message.Add("促销结束日期不能为空!");
                return false;
            }
            var nowdate = DateTime.Now;
            if (model.day_b < new DateTime(nowdate.Year, nowdate.Month, nowdate.Day))
            {
                res.Success = false;
                res.Message.Add("促销开始日期已过，请重新选择大于今天的日期!");
                return false;
            }
            if (model.day_b > model.day_e)
            {
                res.Success = false;
                res.Message.Add("促销开始日期必须小于结束日期!");
                return false;
            }
            if (string.IsNullOrEmpty(model.time_b))
            {
                res.Success = false;
                res.Message.Add("促销开始时间不能为空!");
                return false;
            }
            if (string.IsNullOrEmpty(model.time_e))
            {
                res.Success = false;
                res.Message.Add("促销结束时间不能为空!");
                return false;
            }
            var te = model.time_e.Replace(":", "").Replace("：", "");
            var tb = model.time_b.Replace(":", "").Replace("：", "");
            int int_te = 0;
            int int_tb = 0;
            int.TryParse(te, out int_te);
            int.TryParse(tb, out int_tb);
            if (int_tb > int_te)
            {
                res.Success = false;
                res.Message.Add("促销开始时间不能大于结束时间!");
                return false;
            }
            Regex regex = new Regex(@",.+,");
            if (model.flag_rqfw == 1)
            {
                if (string.IsNullOrEmpty(model.days) || !regex.IsMatch(model.days))
                {
                    res.Success = false;
                    res.Message.Add("请选择指定日期!");
                    return false;
                }
            }
            if (model.flag_rqfw == 2)
            {
                if (string.IsNullOrEmpty(model.weeks) || !regex.IsMatch(model.weeks))
                {
                    res.Success = false;
                    res.Message.Add("请选择指定星期!");
                    return false;
                }
            }
            #endregion

            if (string.IsNullOrEmpty(model.spxz) && string.IsNullOrEmpty(model.id))
            {
                res.Success = false;
                res.Message.Add("未设置商品组!");
                return false;
            }
            if (model.bm_djlx == "CX010")//特价促销单
            {
                #region
                if (string.IsNullOrEmpty(model.sp))
                {
                    res.Success = false;
                    res.Message.Add("请选择特价商品!");
                    return false;
                }
                #endregion
            }
            else if (model.bm_djlx == "CX020")//组合促销单
            {
                #region
                if (model.condition_1 == 0)
                {
                    res.Success = false;
                    res.Message.Add("请按A、B、C组号填对应的任选数量!");
                    return false;
                }
                if (model.condition_2 == 0 && model.condition_3 > 0)
                {
                    res.Success = false;
                    res.Message.Add("请按A、B、C组号填对应的任选数量!");
                    return false;
                }
                if (model.condition_1 > 0 && (string.IsNullOrEmpty(model.zh_sp_a) || !model.zh_sp_a.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择A组商品!");
                    return false;
                }
                if (model.condition_1 <= 0 && !string.IsNullOrEmpty(model.zh_sp_a) && model.zh_sp_a.Contains("id_object"))
                {
                    res.Success = false;
                    res.Message.Add("请设置A组组合条件!");
                    return false;
                }
                if (model.condition_2 > 0 && (string.IsNullOrEmpty(model.zh_sp_b) || !model.zh_sp_b.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择B组商品!");
                    return false;
                }
                if (model.condition_2 <= 0 && !string.IsNullOrEmpty(model.zh_sp_b) && model.zh_sp_b.Contains("id_object"))
                {
                    res.Success = false;
                    res.Message.Add("请设置B组组合条件!");
                    return false;
                }
                if (model.condition_3 > 0 && (string.IsNullOrEmpty(model.zh_sp_c) || !model.zh_sp_c.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择C组商品!");
                    return false;
                }
                if (model.condition_3 <= 0 && !string.IsNullOrEmpty(model.zh_sp_c) && model.zh_sp_c.Contains("id_object"))
                {
                    res.Success = false;
                    res.Message.Add("请设置C组组合条件!");
                    return false;
                }
                if (model.condition_3 > 0 && model.condition_2 <= 0)
                {
                    res.Success = false;
                    res.Message.Add("请按A、B、C组号顺序设置商品组合!");
                    return false;
                }
                if (model.result_1 == 0)
                {
                    res.Success = false;
                    res.Message.Add("请填写组合售价!");
                    return false;
                }
                #endregion
            }
            else if (model.bm_djlx == "CX110")//单品折扣促销单
            {
                if (string.IsNullOrEmpty(model.sp))
                {
                    res.Success = false;
                    res.Message.Add("请选择商品!");
                    return false;
                }
            }
            else if (model.bm_djlx == "CX120")//品类折扣促销单
            {
                if (string.IsNullOrEmpty(model.sp) || !model.sp.Contains("id_object"))
                {
                    res.Success = false;
                    res.Message.Add("请选择商品分类!");
                    return false;
                }
            }
            else if (model.bm_djlx == "CX130")//单组折扣促销单
            {
                #region
                if (model.condition_1 == 0 || model.result_1 == 0)
                {
                    res.Success = false;
                    res.Message.Add("请按顺序一、二、三设置打折条件!");
                    return false;
                }
                if ((model.condition_2 == 0 && model.condition_3 != 0) || (model.result_2 == 0 && model.result_3 != 0))
                {
                    res.Success = false;
                    res.Message.Add("请按顺序一、二、三设置打折条件!");
                    return false;
                }
                if ((model.condition_2 == 0 && model.result_2 != 0) || (model.condition_2 != 0 && model.result_2 == 0))
                {
                    res.Success = false;
                    res.Message.Add("规则二设置不完整!");
                    return false;
                }
                if ((model.condition_3 == 0 && model.result_3 != 0) || (model.condition_3 != 0 && model.result_3 == 0))
                {
                    res.Success = false;
                    res.Message.Add("规则三设置不完整!");
                    return false;
                }
                if (model.condition_2 != 0 && model.condition_2 <= model.condition_1)
                {
                    res.Success = false;
                    res.Message.Add("规则二的条件必需大于规则一的条件!");
                    return false;
                }
                if (model.condition_3 != 0 && model.condition_3 <= model.condition_2)
                {
                    res.Success = false;
                    res.Message.Add("规则三的条件必需大于规则二的条件!");
                    return false;
                }
                if (model.result_2 > 0 && model.result_2 >= model.result_1)
                {
                    res.Success = false;
                    res.Message.Add("规则二的折扣必需小于规则一的折扣!");
                    return false;
                }
                if (model.result_3 > 0 && model.result_3 >= model.result_2)
                {
                    res.Success = false;
                    res.Message.Add("规则三的折扣必需小于规则二的折扣!");
                    return false;
                }
                if (string.IsNullOrEmpty(model.jsgz))
                {
                    res.Success = false;
                    res.Message.Add("请选择结算规则!");
                    return false;
                }
                if (model.result_1 > 1 || model.result_1 < 0)
                {
                    res.Success = false;
                    res.Message.Add("折扣1只能设置0-1的数!");
                    return false;
                }
                if (model.result_2 > 1 || model.result_2 < 0)
                {
                    res.Success = false;
                    res.Message.Add("折扣2只能设置0-1的数!");
                    return false;
                }
                if (model.result_3 > 1 || model.result_3 < 0)
                {
                    res.Success = false;
                    res.Message.Add("折扣3只能设置0-1的数!");
                    return false;
                }
                if (string.IsNullOrEmpty(model.sp))
                {
                    res.Success = false;
                    res.Message.Add("请选择商品!");
                    return false;
                }
                #endregion
            }
            else if (model.bm_djlx == "CX140")//时段折扣促销单
            {
                #region 时段促销
                if (model.jsfs != "sl")
                {
                    res.Success = false;
                    res.Message.Add("时段促销结算方式只能为数量");
                    return false;
                }
                if (model.jsgz != "man")
                {
                    res.Success = false;
                    res.Message.Add("时段促销结算规则只能为满");
                    return false;
                }
                if (model.sd1 == -1)
                {
                    res.Success = false;
                    res.Message.Add("规则一的时点未设置!");
                    return false;
                }
                if (model.result_1 == 0)
                {
                    res.Success = false;
                    res.Message.Add("规则一的折扣未设置!");
                    return false;
                }
                if (model.sd3 > -1 && model.sd2 == -1)
                {
                    res.Success = false;
                    res.Message.Add("按规则一、二、三顺序设置时点!");
                    return false;
                }
                if (model.sd2 > -1 && model.result_2 == 0)
                {
                    res.Success = false;
                    res.Message.Add("规则二的折扣未设置!");
                    return false;
                }
                if (model.sd3 > -1 && model.result_3 == 0)
                {
                    res.Success = false;
                    res.Message.Add("规则三的折扣未设置!");
                    return false;
                }
                if (model.result_1 > 1 || model.result_1 < 0)
                {
                    res.Success = false;
                    res.Message.Add("规则一的折扣只能设置0-1的数!");
                    return false;
                }
                if (model.result_1 > 2 || model.result_2 < 0)
                {
                    res.Success = false;
                    res.Message.Add("规则二的折扣只能设置0-1的数!");
                    return false;
                }
                if (model.result_3 > 2 || model.result_3 < 0)
                {
                    res.Success = false;
                    res.Message.Add("规则三的折扣只能设置0-1的数!");
                    return false;
                }
                if (model.sd2 <= 0 && int_te <= model.sd1)
                {
                    res.Success = false;
                    res.Message.Add("结束时间必需大于规则一的时点!");
                    return false;
                }
                if (model.sd2 > 0 && model.sd2 <= model.sd1)
                {
                    res.Success = false;
                    res.Message.Add("规则二的时点必需大于规则一的时点!");
                    return false;
                }
                if (model.sd3 <= 0 && model.sd2 > 0 && int_te <= model.sd2)
                {
                    res.Success = false;
                    res.Message.Add("结束时间必需大于规则二的时点!");
                    return false;
                }
                if (model.sd3 > 0 && model.sd3 <= model.sd2)
                {
                    res.Success = false;
                    res.Message.Add("规则三的时点必需大于规则二的时点!");
                    return false;
                }
                if (model.sd3 > 0 && int_te <= model.sd3)
                {
                    res.Success = false;
                    res.Message.Add("结束时间必需大于规则三的时点!");
                    return false;
                }
                if (string.IsNullOrEmpty(model.sp))
                {
                    res.Success = false;
                    res.Message.Add("请选择促销商品!");
                    return false;
                }
                #endregion
            }
            else if (model.bm_djlx == "CX150")//整单折扣促销单
            {
                #region
                if (model.condition_1 == 0 || model.result_1 == 0)
                {
                    res.Success = false;
                    res.Message.Add("请按顺序一、二、三设置打折条件!");
                    return false;
                }
                if ((model.condition_2 == 0 && model.condition_3 != 0) || (model.result_2 == 0 && model.result_3 != 0))
                {
                    res.Success = false;
                    res.Message.Add("请按顺序一、二、三设置打折条件!");
                    return false;
                }
                if ((model.condition_2 == 0 && model.result_2 != 0) || (model.condition_2 != 0 && model.result_2 == 0))
                {
                    res.Success = false;
                    res.Message.Add("规则二的条件设置不完整!");
                    return false;
                }
                if ((model.condition_3 == 0 && model.result_3 != 0) || (model.condition_3 != 0 && model.result_3 == 0))
                {
                    res.Success = false;
                    res.Message.Add("规则三的条件设置不完整!");
                    return false;
                }
                if (model.condition_2 != 0 && model.condition_2 <= model.condition_1)
                {
                    res.Success = false;
                    res.Message.Add("规则二的条件必需大于规则一的条件!");
                    return false;
                }
                if (model.condition_3 != 0 && model.condition_3 <= model.condition_2)
                {
                    res.Success = false;
                    res.Message.Add("规则三的条件必需大于规则二的条件!");
                    return false;
                }
                if (model.result_2 > 0 && model.result_2 >= model.result_1)
                {
                    res.Success = false;
                    res.Message.Add("规则二的折扣必需小于规则一的折扣!");
                    return false;
                }
                if (model.result_3 > 0 && model.result_3 >= model.result_2)
                {
                    res.Success = false;
                    res.Message.Add("规则三的折扣必需小于规则二的折扣!");
                    return false;
                }
                if (model.result_1 > 1 || model.result_1 < 0)
                {
                    res.Success = false;
                    res.Message.Add("规则一的折扣只能设置0-1的数!");
                    return false;
                }
                if (model.result_1 > 2 || model.result_2 < 0)
                {
                    res.Success = false;
                    res.Message.Add("规则二的折扣只能设置0-1的数!");
                    return false;
                }
                if (model.result_3 > 2 || model.result_3 < 0)
                {
                    res.Success = false;
                    res.Message.Add("规则三的折扣只能设置0-1的数!");
                    return false;
                }
                if (string.IsNullOrEmpty(model.jsgz))
                {
                    res.Success = false;
                    res.Message.Add("请选择结算规则!");
                    return false;
                }

                #endregion
            }
            else if (model.bm_djlx == "CX210")//单品买减促销单
            {
                #region
                if (string.IsNullOrEmpty(model.sp) || !model.sp.Contains("id_object"))
                {
                    res.Success = false;
                    res.Message.Add("请选择商品!");
                    return false;
                }
                if (string.IsNullOrEmpty(model.jsfs))
                {
                    res.Success = false;
                    res.Message.Add("请选择结算方式!");
                    return false;
                }
                if (string.IsNullOrEmpty(model.jsgz))
                {
                    res.Success = false;
                    res.Message.Add("请选择结算规则!");
                    return false;
                }
                #endregion
            }
            else if (model.bm_djlx == "CX220")//单组买送促销单
            {
                #region
                if (model.condition_1 == 0)
                {
                    res.Success = false;
                    res.Message.Add("请按顺序一、二、三设置促销规则!");
                    return false;
                }
                if ((model.condition_2 == 0 && model.condition_3 > 0))
                {
                    res.Success = false;
                    res.Message.Add("请按顺序一、二、三设置促销规则!");
                    return false;
                }
                if (model.result_1 <= 0 && model.sl_largess_1 <= 0)
                {
                    res.Success = false;
                    res.Message.Add("规则一设置不完整!");
                    return false;
                }
                if ((model.condition_2 > 0 && model.result_2 <= 0 && model.sl_largess_2 <= 0)
                    || (model.condition_2 <= 0 && (model.result_2 > 0 || model.sl_largess_2 > 0)))
                {
                    res.Success = false;
                    res.Message.Add("规则二设置不完整!");
                    return false;
                }
                if ((model.condition_3 > 0 && model.result_3 <= 0 && model.sl_largess_3 <= 0)
                    || (model.condition_3 <= 0 && (model.result_3 > 0 || model.sl_largess_3 > 0)))
                {
                    res.Success = false;
                    res.Message.Add("规则三设置不完整!");
                    return false;
                }
                if (model.sl_largess_1 > 0 && (string.IsNullOrEmpty(model.zs_sp_1) || !model.zs_sp_1.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择赠送商品1");
                    return false;
                }
                if (model.sl_largess_2 > 0 && (string.IsNullOrEmpty(model.zs_sp_2) || !model.zs_sp_2.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择赠送商品2");
                    return false;
                }
                if (model.sl_largess_3 > 0 && (string.IsNullOrEmpty(model.zs_sp_3) || !model.zs_sp_3.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择赠送商品3");
                    return false;
                }
                if (model.condition_2 > 0 && model.condition_2 <= model.condition_1)
                {
                    res.Success = false;
                    res.Message.Add("规则二的条件数值必需大于规则一的条件数值!");
                    return false;
                }
                if (model.condition_3 > 0 && model.condition_3 <= model.condition_2)
                {
                    res.Success = false;
                    res.Message.Add("规则三的条件数值必需大于规则二的条件数值!");
                    return false;
                }
                if (model.sl_largess_3 == 0
                    && model.sl_largess_2 == 0
                    && model.sl_largess_1 == 0)
                {
                    if (model.result_2 > 0 && model.result_2 <= model.result_1)
                    {
                        res.Success = false;
                        res.Message.Add("规则二的优惠金额必需大于规则一的优惠金额!");
                        return false;
                    }
                    if (model.result_3 > 0 && model.result_3 <= model.result_2)
                    {
                        res.Success = false;
                        res.Message.Add("规则三的优惠金额必需大于规则二的优惠金额!");
                        return false;
                    }
                }
                #endregion
            }
            else if (model.bm_djlx == "CX230" || model.bm_djlx == "CX240")//整类买送促销单//整单买送促销单
            {
                #region
                if (model.jsfs != "je")
                {
                    res.Success = false;
                    res.Message.Add("只能以金额做为结算方式!");
                    return false;
                }
                if (model.condition_1 == 0)
                {
                    res.Success = false;
                    res.Message.Add("请按顺序一、二、三设置促销规则!");
                    return false;
                }
                if ((model.condition_2 == 0 && model.condition_3 != 0))
                {
                    res.Success = false;
                    res.Message.Add("请按顺序一、二、三设置促销规则!");
                    return false;
                }
                if (model.result_1 == 0 && model.sl_largess_1 == 0)
                {
                    res.Success = false;
                    res.Message.Add("规则一设置不完整!");
                    return false;
                }
                if ((model.condition_2 > 0 && model.result_2 <= 0 && model.sl_largess_2 <= 0)
                    || (model.condition_2 <= 0 && (model.result_2 > 0 || model.sl_largess_2 > 0)))
                {
                    res.Success = false;
                    res.Message.Add("规则二设置不完整!");
                    return false;
                }
                if ((model.condition_3 > 0 && model.result_3 <= 0 && model.sl_largess_3 <= 0)
                    || (model.condition_3 <= 0 && (model.result_3 > 0 || model.sl_largess_3 > 0)))
                {
                    res.Success = false;
                    res.Message.Add("规则三设置不完整!");
                    return false;
                }
                if (model.sl_largess_1 > 0 && (string.IsNullOrEmpty(model.zs_sp_1) || !model.zs_sp_1.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择赠送商品1");
                    return false;
                }
                if (model.sl_largess_2 > 0 && (string.IsNullOrEmpty(model.zs_sp_2) || !model.zs_sp_2.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择赠送商品2");
                    return false;
                }
                if (model.sl_largess_3 > 0 && (string.IsNullOrEmpty(model.zs_sp_3) || !model.zs_sp_3.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择赠送商品3");
                    return false;
                }
                if (model.condition_2 > 0 && model.condition_2 <= model.condition_1)
                {
                    res.Success = false;
                    res.Message.Add("规则二的条件数值必需大于规则一的条件数值!");
                    return false;
                }
                if (model.condition_3 > 0 && model.condition_3 <= model.condition_2)
                {
                    res.Success = false;
                    res.Message.Add("规则三的条件数值必需大于规则二的条件数值!");
                    return false;
                }
                if (model.sl_largess_3 == 0
                    && model.sl_largess_2 == 0
                    && model.sl_largess_1 == 0)
                {
                    if (model.result_2 > 0 && model.result_2 <= model.result_1)
                    {
                        res.Success = false;
                        res.Message.Add("规则二的优惠金额必需大于规则一的优惠金额!");
                        return false;
                    }
                    if (model.result_3 > 0 && model.result_3 <= model.result_2)
                    {
                        res.Success = false;
                        res.Message.Add("规则三的优惠金额必需大于规则二的优惠金额!");
                        return false;
                    }
                }
                #endregion
            }
            else if (model.bm_djlx == "CX310")//单组加价换购促销单
            {
                #region
                if (model.condition_1 == 0)
                {
                    res.Success = false;
                    res.Message.Add("请设置规则一!");
                    return false;
                }
                if (model.result_1 == 0 || model.sl_largess_1 == 0)
                {
                    res.Success = false;
                    res.Message.Add("规则一设置不完整!");
                    return false;
                }
                if (model.condition_2 == 0 && model.condition_3 != 0)
                {
                    res.Success = false;
                    res.Message.Add("请按顺序一、二、三设置规则!");
                    return false;
                }
                if (model.condition_2 > 0 && (model.result_2 <= 0 || model.sl_largess_2 <= 0))
                {
                    res.Success = false;
                    res.Message.Add("规则二设置不完整!");
                    return false;
                }
                if (model.condition_3 > 0 && (model.result_3 <= 0 || model.sl_largess_3 <= 0))
                {
                    res.Success = false;
                    res.Message.Add("规则三设置不完整!");
                    return false;
                }
                if (model.condition_2 > 0 && model.condition_2 <= model.condition_1)
                {
                    res.Success = false;
                    res.Message.Add("规则二的条件必需大于规则一的条件数值!");
                    return false;
                }
                if (model.condition_3 > 0 && model.condition_3 <= model.condition_2)
                {
                    res.Success = false;
                    res.Message.Add("规则三的条件数值必需大于规则二的条件数值!");
                    return false;
                }
                if (string.IsNullOrEmpty(model.sp) || !model.sp.Contains("id_object"))
                {
                    res.Success = false;
                    res.Message.Add("请选择商品!");
                    return false;
                }
                if (model.sl_largess_1 > 0 && (string.IsNullOrEmpty(model.zs_sp_1) || !model.zs_sp_1.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择换购商品1!");
                    return false;
                }
                if (model.sl_largess_2 > 0 && (string.IsNullOrEmpty(model.zs_sp_2) || !model.zs_sp_2.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择换购商品2!");
                    return false;
                }
                if (model.sl_largess_3 > 0 && (string.IsNullOrEmpty(model.zs_sp_3) || !model.zs_sp_3.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择换购商品3!");
                    return false;
                }
                #endregion
            }
            else if (model.bm_djlx == "CX320")//整单加价换购促销单
            {
                #region
                if (model.jsfs != "je")
                {
                    res.Success = false;
                    res.Message.Add("加价换购整单只能以金额做为结算方式!");
                    return false;
                }
                if (model.condition_1 == 0)
                {
                    res.Success = false;
                    res.Message.Add("请设置规则一!");
                    return false;
                }
                if (model.result_1 == 0 || model.sl_largess_1 == 0)
                {
                    res.Success = false;
                    res.Message.Add("规则一设置不完整!");
                    return false;
                }
                if (model.condition_2 == 0 && model.condition_3 != 0)
                {
                    res.Success = false;
                    res.Message.Add("请按顺序一、二、三设置规则!");
                    return false;
                }
                if (model.condition_2 > 0 && (model.result_2 <= 0 || model.sl_largess_2 <= 0))
                {
                    res.Success = false;
                    res.Message.Add("规则二设置不完整!");
                    return false;
                }
                if (model.condition_3 > 0 && (model.result_3 <= 0 || model.sl_largess_3 <= 0))
                {
                    res.Success = false;
                    res.Message.Add("规则三设置不完整!");
                    return false;
                }
                if (model.condition_2 > 0 && model.condition_2 <= model.condition_1)
                {
                    res.Success = false;
                    res.Message.Add("规则二的条件必需大于规则一的条件数值!");
                    return false;
                }
                if (model.condition_3 > 0 && model.condition_3 <= model.condition_2)
                {
                    res.Success = false;
                    res.Message.Add("规则三的条件数值必需大于规则二的条件数值!");
                    return false;
                }
                if (model.sl_largess_1 > 0 && (string.IsNullOrEmpty(model.zs_sp_1) || !model.zs_sp_1.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择换购商品1!");
                    return false;
                }
                if (model.sl_largess_2 > 0 && (string.IsNullOrEmpty(model.zs_sp_2) || !model.zs_sp_2.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择换购商品2!");
                    return false;
                }
                if (model.sl_largess_3 > 0 && (string.IsNullOrEmpty(model.zs_sp_3) || !model.zs_sp_3.Contains("id_object")))
                {
                    res.Success = false;
                    res.Message.Add("请选择换购商品3!");
                    return false;
                }
                #endregion
            }

            #region 公共参数

            //if (string.IsNullOrEmpty(model.hylx))
            //{
            //    res.Success = false;
            //    res.Message.Add("请选择会员类型!");
            //    return false;
            //}
            if (string.IsNullOrEmpty(model.id_shops))
            {
                res.Success = false;
                res.Message.Add("请选择门店!");
                return false;
            }
            if (string.IsNullOrEmpty(model.id_shop) && string.IsNullOrEmpty(model.id))
            {
                res.Success = false;
                res.Message.Add("开单门店丢失!");
                return false;
            }
            #endregion

            return true;
        }
        /// <summary>
        /// 处理促销类型(暂留，之后删除)
        /// </summary>
        private bool HandleStyle1(BaseResult res, PromoteViewModel model, Td_Promote_1 promoteModel, string addOrUpdate = "add")
        {
            if (string.IsNullOrEmpty(promoteModel.id) || string.IsNullOrEmpty(promoteModel.style))
            {
                return false;
            }
            List<Td_Promote_2> promote2S;
            var currentXH = 0;
            if (promoteModel.bm_djlx == "CX010" || promoteModel.bm_djlx == "CX020")//折扣 //特价
            {
                #region
                switch (promoteModel.style)
                {
                    case "dp"://部份商品:只用处理商品列表
                        promote2S = GetSpList(res, model, promoteModel);
                        if (promote2S.Any() && res.Success)
                        {
                            DAL.AddRange(promote2S);
                        }
                        break;
                    case "dzsp"://分组商品
                        promote2S = GetSpList(res, model, promoteModel);
                        if (promote2S.Any() && res.Success)
                        {
                            DAL.AddRange(promote2S);
                        }
                        break;
                    case "zh":
                        break;
                    case "bill"://全部商品:只用处理例外商品列表
                        promote2S = GetLwspList(res, model, promoteModel);
                        if (promote2S.Any() && res.Success)
                        {
                            DAL.AddRange(promote2S);
                        }
                        break;
                    case "spfl"://分类商品:处理选中分类以及例外商品
                        promote2S = GetSpList(res, model, promoteModel);
                        if (promote2S.Any() && res.Success)
                        {
                            currentXH = promote2S.Count;
                            DAL.AddRange(promote2S);
                        }
                        promote2S = GetLwspList(res, model, promoteModel, currentXH);
                        if (promote2S.Any() && res.Success)
                        {
                            DAL.AddRange(promote2S);
                        }
                        break;
                    default:
                        break;
                }
                #endregion
            }
            else if (promoteModel.bm_djlx == "CX030") //买送促销单
            {
                switch (promoteModel.style)
                {
                    #region
                    case "spfl"://分类商品:处理选中分类以及例外商品
                        promote2S = GetZsspList(res, model, promoteModel);
                        if (promote2S.Any() && res.Success)
                        {
                            currentXH = promote2S.Count;
                            DAL.AddRange(promote2S);
                        }
                        promote2S = GetLwspList(res, model, promoteModel, currentXH);
                        if (promote2S.Any() && res.Success)
                        {
                            DAL.AddRange(promote2S);
                        }
                        break;
                    case "dzsp":
                        promote2S = GetSpList(res, model, promoteModel);
                        if (promote2S.Any() && res.Success)
                        {
                            currentXH = promote2S.Count;
                            DAL.AddRange(promote2S);
                        }
                        promote2S = GetZsspList(res, model, promoteModel, currentXH);
                        if (promote2S.Any() && res.Success)
                        {
                            DAL.AddRange(promote2S);
                        }
                        break;
                    case "bill":
                        promote2S = GetLwspList(res, model, promoteModel);
                        if (promote2S.Any() && res.Success)
                        {
                            currentXH = promote2S.Count;
                            DAL.AddRange(promote2S);
                        }
                        promote2S = GetZsspList(res, model, promoteModel, currentXH);
                        if (promote2S.Any() && res.Success)
                        {
                            DAL.AddRange(promote2S);
                        }
                        break;
                    default:
                        break;
                    #endregion
                }
            }
            else if (promoteModel.bm_djlx == "CX050")//组合
            {
                promote2S = GetZhspList(res, model, promoteModel, currentXH);
                if (promote2S.Any() && res.Success)
                {
                    DAL.AddRange(promote2S);
                }
            }
            else if (promoteModel.bm_djlx == "CX040")//加价换购促销单
            {
                #region
                switch (promoteModel.style)
                {
                    case "bill":
                        promote2S = GetLwspList(res, model, promoteModel);
                        if (promote2S.Any() && res.Success)
                        {
                            currentXH = promote2S.Count();
                            DAL.AddRange(promote2S);
                        }
                        promote2S = GetZsspList(res, model, promoteModel, currentXH);
                        if (promote2S.Any() && res.Success)
                        {
                            DAL.AddRange(promote2S);
                        }
                        break;
                    case "dzsp":
                        promote2S = GetSpList(res, model, promoteModel);
                        if (promote2S.Any() && res.Success)
                        {
                            currentXH = promote2S.Count();
                            DAL.AddRange(promote2S);
                        }
                        promote2S = GetZsspList(res, model, promoteModel, currentXH);
                        if (promote2S.Any() && res.Success)
                        {
                            DAL.AddRange(promote2S);
                        }
                        break;
                }
                #endregion
            }
            else if (promoteModel.bm_djlx == "CX060")//时段促销
            {

            }
            return true;
        }
        /// <summary>
        /// 处理促销类型
        /// </summary>
        private bool HandleStyle(BaseResult res, PromoteViewModel model, Td_Promote_1 promoteModel, string addOrUpdate = "add")
        {
            if (string.IsNullOrEmpty(promoteModel.id) || string.IsNullOrEmpty(promoteModel.style))
            {
                return false;
            }
            List<Td_Promote_2> promote2S;
            var currentXH = 0;
            if (model.bm_djlx == "CX010")//特价促销单
            {
                promote2S = GetSpList(res, model, promoteModel);
                if (promote2S.Any() && res.Success)
                {
                    DAL.AddRange(promote2S);
                }
            }
            else if (model.bm_djlx == "CX020")//组合促销单
            {
                promote2S = GetZhspList(res, model, promoteModel, currentXH);
                if (promote2S.Any() && res.Success)
                {
                    DAL.AddRange(promote2S);
                }
            }
            else if (model.bm_djlx == "CX110")//单品折扣促销单
            {
                promote2S = GetSpList(res, model, promoteModel);
                if (promote2S.Any() && res.Success)
                {
                    DAL.AddRange(promote2S);
                }
            }
            else if (model.bm_djlx == "CX120")//品类折扣促销单
            {
                promote2S = GetSpList(res, model, promoteModel);
                if (promote2S.Any() && res.Success)
                {
                    currentXH = promote2S.Count;
                    DAL.AddRange(promote2S);
                }
                promote2S = GetLwspList(res, model, promoteModel, currentXH);
                if (promote2S.Any() && res.Success)
                {
                    DAL.AddRange(promote2S);
                }
            }
            else if (model.bm_djlx == "CX130")//单组折扣促销单
            {
                promote2S = GetSpList(res, model, promoteModel);
                if (promote2S.Any() && res.Success)
                {
                    DAL.AddRange(promote2S);
                }
            }
            else if (model.bm_djlx == "CX140")//时段折扣促销单
            {
                promote2S = GetSpList(res, model, promoteModel);
                if (promote2S.Any() && res.Success)
                {
                    DAL.AddRange(promote2S);
                }
            }
            else if (model.bm_djlx == "CX150")//整单折扣促销单
            {
                promote2S = GetLwspList(res, model, promoteModel);
                if (promote2S.Any() && res.Success)
                {
                    DAL.AddRange(promote2S);
                }
            }
            else if (model.bm_djlx == "CX210")//单品买减促销单
            {
                promote2S = GetSpList(res, model, promoteModel);
                if (promote2S.Any() && res.Success)
                {
                    DAL.AddRange(promote2S);
                }
            }
            else if (model.bm_djlx == "CX220")//单组买送促销单
            {
                promote2S = GetSpList(res, model, promoteModel);
                if (promote2S.Any() && res.Success)
                {
                    currentXH = promote2S.Count;
                    DAL.AddRange(promote2S);
                }
                promote2S = GetZsspList(res, model, promoteModel, currentXH);
                if (promote2S.Any() && res.Success)
                {
                    DAL.AddRange(promote2S);
                }
            }
            else if (model.bm_djlx == "CX230")//整类买送促销单
            {
                promote2S = GetSpList(res, model, promoteModel);
                if (promote2S.Any() && res.Success)
                {
                    currentXH = promote2S.Count;
                    DAL.AddRange(promote2S);
                }
                promote2S = GetZsspList(res, model, promoteModel);
                if (promote2S.Any() && res.Success)
                {
                    currentXH = promote2S.Count;
                    DAL.AddRange(promote2S);
                }
                promote2S = GetLwspList(res, model, promoteModel, currentXH);
                if (promote2S.Any() && res.Success)
                {
                    DAL.AddRange(promote2S);
                }
            }
            else if (model.bm_djlx == "CX240")//整单买送促销单
            {
                promote2S = GetLwspList(res, model, promoteModel);
                if (promote2S.Any() && res.Success)
                {
                    currentXH = promote2S.Count;
                    DAL.AddRange(promote2S);
                }
                promote2S = GetZsspList(res, model, promoteModel, currentXH);
                if (promote2S.Any() && res.Success)
                {
                    DAL.AddRange(promote2S);
                }
            }
            else if (model.bm_djlx == "CX310")//单组加价换购促销单
            {
                promote2S = GetSpList(res, model, promoteModel);
                if (promote2S.Any() && res.Success)
                {
                    currentXH = promote2S.Count();
                    DAL.AddRange(promote2S);
                }
                promote2S = GetZsspList(res, model, promoteModel, currentXH);
                if (promote2S.Any() && res.Success)
                {
                    DAL.AddRange(promote2S);
                }
            }
            else if (model.bm_djlx == "CX320")//整单加价换购促销单
            {
                promote2S = GetLwspList(res, model, promoteModel);
                if (promote2S.Any() && res.Success)
                {
                    currentXH = promote2S.Count();
                    DAL.AddRange(promote2S);
                }
                promote2S = GetZsspList(res, model, promoteModel, currentXH);
                if (promote2S.Any() && res.Success)
                {
                    DAL.AddRange(promote2S);
                }
            }
            return true;
        }
        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="sps"></param>
        /// <param name="id_masteruser"></param>
        /// <param name="id_bill"></param>
        /// <param name="dh"></param>
        /// <returns></returns>
        private List<Td_Promote_2> GetSpList(BaseResult res, PromoteViewModel model, Td_Promote_1 promoteModel)
        {
            List<Td_Promote_2> promote2S = new List<Td_Promote_2>();
            var date = DateTime.Now;
            if (!string.IsNullOrEmpty(model.sp))//sps:[{"id_object":"xxxx","condition_1":"","condition_2":"","condition_3":"","result_1":"","result_2":"","result_3":""}]
            {
                promote2S = JSON.Deserialize<List<Td_Promote_2>>(model.sp);
                if (promote2S.Any())
                {
                    for (int i = 0; i < promote2S.Count; i++)
                    {
                        var p = promote2S[i];
                        p.id = GetGuid;
                        p.zh_group = "A";
                        p.id_masteruser = model.id_masteruser;
                        p.rq_create = date;
                        p.sort_id = i + 1;
                        p.id_bill = promoteModel.id;
                        CheckTdPromote2(p, res, model);
                    }
                }
                else
                {
                    res.Success = false;
                    res.Message.Add(string.Format("{0}", (model.bm_djlx == "CX230" || model.bm_djlx == "CX120") ? "请选择商品类别!" : "请选择商品"));
                    return promote2S;
                }
            }
            return promote2S;
        }

        private bool CheckTdPromote2(Td_Promote_2 promote2, BaseResult res, PromoteViewModel model)
        {
            if (promote2 != null)
            {
                if (model.bm_djlx == "CX130"
                    || model.bm_djlx == "CX140"
                    || model.bm_djlx == "CX150"
                    || model.bm_djlx == "CX220"
                    || model.bm_djlx == "CX230"
                    || model.bm_djlx == "CX240"
                    || model.bm_djlx == "CX310"
                    || model.bm_djlx == "CX320"
                    || model.bm_djlx == "CX020")
                {
                    return true;
                }
                #region
                if (promote2.condition_1 == 0 || promote2.result_1 == 0)
                {
                    res.Success = false;
                    res.Message.Add(string.Format("商品{0}行,请按顺序一、二、三设置规则!", promote2.sort_id));
                    return false;
                }
                if ((promote2.condition_2 == 0 && promote2.condition_3 != 0) || (promote2.result_2 == 0 && promote2.result_3 != 0))
                {
                    res.Success = false;
                    res.Message.Add(string.Format("商品{0}行,请按顺序一、二、三设置规则!", promote2.sort_id));
                    return false;
                }
                if ((promote2.condition_2 == 0 && promote2.result_2 != 0) || (promote2.condition_2 != 0 && promote2.result_2 == 0))
                {
                    res.Success = false;
                    res.Message.Add(string.Format("商品{0}行,规则二设置不完整!", promote2.sort_id));
                    return false;
                }
                if ((promote2.condition_3 == 0 && promote2.result_3 != 0) || (promote2.condition_3 != 0 && promote2.result_3 == 0))
                {
                    res.Success = false;
                    res.Message.Add(string.Format("商品{0}行,规则三设置不完整!", promote2.sort_id));
                    return false;
                }
                #endregion
                if (model.bm_djlx == "CX110")//单品折扣促销单
                {
                    #region 折扣
                    if (promote2.condition_2 > 0 && promote2.condition_2 <= promote2.condition_1)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品{0}行,规则二的条件必需大于规则一的条件!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.condition_3 > 0 && promote2.condition_3 <= promote2.condition_2)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品{0}行,规则三的条件不能小于规则二的条件!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.result_2 > 0 && promote2.result_2 >= promote2.result_1)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品{0}行,规则二折扣必需小于规则一折扣!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.result_3 > 0 && promote2.result_3 >= promote2.result_2)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品{0}行,规则三折扣必需小于规则二折扣!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.result_1 > 1 || promote2.result_1 < 0)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品{0}行,规则一的折扣只能设置0-1的数!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.result_2 > 1 || promote2.result_2 < 0)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品{0}行,规则二的折扣只能设置0-1的数!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.result_3 > 1 || promote2.result_3 < 0)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品{0}行,规则三的折扣只能设置0-1的数!", promote2.sort_id));
                        return false;
                    }
                    #endregion
                }
                if (model.bm_djlx == "CX120")//品类折扣促销单
                {
                    #region 折扣
                    if (promote2.condition_2 > 0 && promote2.condition_2 <= promote2.condition_1)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品分类{0}行,规则二的条件必需大于规则一的条件!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.condition_3 > 0 && promote2.condition_3 <= promote2.condition_2)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品分类{0}行,规则三的条件必需大于规则二的条件!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.result_2 > 0 && promote2.result_2 >= promote2.result_1)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品分类{0}行,规则二折扣必需小于规则一折扣!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.result_3 > 0 && promote2.result_3 >= promote2.result_2)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品分类{0}行,规则三折扣必需小于规则二折扣!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.result_1 > 1 || promote2.result_1 < 0)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品分类{0}行,规则一的折扣只能设置0-1的数!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.result_2 > 1 || promote2.result_2 < 0)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品分类{0}行,规则二的折扣只能设置0-1的数!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.result_3 > 1 || promote2.result_3 < 0)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品分类{0}行,规则三的折扣只能设置0-1的数!", promote2.sort_id));
                        return false;
                    }
                    #endregion
                }
                if (model.bm_djlx == "CX210")//单品买减
                {
                    #region
                    if (promote2.condition_2 > 0 && promote2.condition_2 <= promote2.condition_1)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品{0}行,规则二的条件必需大于规则一的条件!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.condition_3 > 0 && promote2.condition_3 <= promote2.condition_2)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品{0}行,规则三的条件必需大于规则二的条件!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.result_2 > 0 && promote2.result_2 <= promote2.result_1)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品{0}行,规则二的优惠必需大于规则一的优惠!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.result_3 > 0 && promote2.result_3 <= promote2.result_2)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品{0}行,规则三的优惠必需大于规则二的优惠!", promote2.sort_id));
                        return false;
                    }
                    #endregion
                }

                if (model.bm_djlx == "CX010")
                {
                    #region 特价
                    if (promote2.condition_2 > 0 && promote2.condition_2 <= promote2.condition_1)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品:第{0}行,规则二的条件必需大于规则一的条件!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.condition_3 > 0 && promote2.condition_3 <= promote2.condition_2)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品:第{0}行,规则三的条件必需大于规则二的条件!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.result_2 > 0 && promote2.result_2 >= promote2.result_1)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品:第{0}行,规则二的特价必需小规则一的特价!", promote2.sort_id));
                        return false;
                    }
                    if (promote2.result_3 > 0 && promote2.result_3 >= promote2.result_2)
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("商品:第{0}行,规则三的特价必需小规则二的特价!", promote2.sort_id));
                        return false;
                    }
                    #endregion
                }
            }
            return true;
        }
        /// <summary>
        /// 获取例外商品列表
        /// </summary>
        /// <param name="lwspIds"></param>
        /// <param name="id_masteruser"></param>
        /// <param name="id_bill"></param>
        /// <param name="dh"></param>
        /// <returns></returns>
        private List<Td_Promote_2> GetLwspList(BaseResult res, PromoteViewModel model, Td_Promote_1 promoteModel, int begin_xh = 0)
        {
            List<Td_Promote_2> promote2S = new List<Td_Promote_2>();
            var date = DateTime.Now;
            if (!string.IsNullOrEmpty(model.lwsp))
            {
                promote2S = JSON.Deserialize<List<Td_Promote_2>>(model.lwsp);
                if (promote2S.Any())
                {
                    for (int i = 0; i < promote2S.Count; i++)
                    {
                        var item = promote2S[i];
                        item.id_masteruser = model.id_masteruser;
                        item.id = GetGuid;
                        item.id_bill = promoteModel.id;
                        item.sort_id = i + 1;
                        item.zh_group = "lw";
                        item.rq_create = date;
                        begin_xh++;
                    }
                }
            }
            return promote2S;
        }

        private List<Td_Promote_2> GetZhspList(BaseResult res, PromoteViewModel model, Td_Promote_1 promoteModel, int begin_xh = 0)
        {
            List<Td_Promote_2> promote2S = new List<Td_Promote_2>();
            var date = DateTime.Now;
            if (!string.IsNullOrEmpty(model.zh_sp_a))
            {
                var _promote2S = JSON.Deserialize<List<Td_Promote_2>>(model.zh_sp_a);
                if (_promote2S.Any())
                {
                    for (int i = 0; i < _promote2S.Count; i++)
                    {
                        var p = _promote2S[i];
                        p.id = GetGuid;
                        p.zh_group = "A";
                        p.id_masteruser = model.id_masteruser;
                        p.rq_create = date;
                        p.sort_id = i + 1;
                        p.id_bill = promoteModel.id;
                        CheckTdPromote2(p, res, model);
                    }
                    promote2S.AddRange(_promote2S);
                }
                if (model.condition_1 > 0 && _promote2S.Count <= 0)
                {
                    res.Success = false;
                    res.Message.Add("请选择商品A!");
                    return promote2S;
                }
                if (model.condition_1 <= 0 && _promote2S.Count > 0)
                {
                    res.Success = false;
                    res.Message.Add("请设置组A的任选数量!");
                    return promote2S;
                }
            }
            if (!string.IsNullOrEmpty(model.zh_sp_b))
            {
                var _promote2S = JSON.Deserialize<List<Td_Promote_2>>(model.zh_sp_b);
                if (_promote2S.Any())
                {
                    for (int i = 0; i < _promote2S.Count; i++)
                    {
                        var p = _promote2S[i];
                        p.id = GetGuid;
                        p.zh_group = "B";
                        p.id_masteruser = model.id_masteruser;
                        p.rq_create = date;
                        p.sort_id = i + 1;
                        p.id_bill = promoteModel.id;
                        CheckTdPromote2(p, res, model);
                    }
                    promote2S.AddRange(_promote2S);
                }
                if (model.condition_2 > 0 && _promote2S.Count <= 0)
                {
                    res.Success = false;
                    res.Message.Add("请选择商品B!");
                    return promote2S;
                }
                if (model.condition_2 <= 0 && _promote2S.Count > 0)
                {
                    res.Success = false;
                    res.Message.Add("请设置组B的任选数量!");
                    return promote2S;
                }
            }
            if (!string.IsNullOrEmpty(model.zh_sp_c))
            {
                var _promote2S = JSON.Deserialize<List<Td_Promote_2>>(model.zh_sp_c);
                if (_promote2S.Any())
                {
                    for (int i = 0; i < _promote2S.Count; i++)
                    {
                        var p = _promote2S[i];
                        p.id = GetGuid;
                        p.zh_group = "C";
                        p.id_masteruser = model.id_masteruser;
                        p.rq_create = date;
                        p.sort_id = i + 1;
                        p.id_bill = promoteModel.id;
                        CheckTdPromote2(p, res, model);
                    }
                    promote2S.AddRange(_promote2S);
                }
                if (model.condition_3 > 0 && _promote2S.Count <= 0)
                {
                    res.Success = false;
                    res.Message.Add("请选择商品C!");
                    return promote2S;
                }
                if (model.condition_3 <= 0 && _promote2S.Count > 0)
                {
                    res.Success = false;
                    res.Message.Add("请设置组C的任选数量!");
                    return promote2S;
                }
            }
            return promote2S;
        }
        private Td_Promote_2 CreateTdPromote2(PromoteViewModel model, Td_Promote_1 promoteModel, int sort_id, int xh, string id_object, DateTime date, string zh_group)
        {
            Td_Promote_2 promote2 = new Td_Promote_2();
            promote2.id_masteruser = model.id_masteruser;
            promote2.id = GetGuid;
            promote2.id_bill = promoteModel.id;
            promote2.sort_id = sort_id;
            promote2.id_object = id_object;
            promote2.sl_largess_1 = promoteModel.sl_largess_1;
            promote2.sl_largess_2 = promoteModel.sl_largess_2;
            promote2.sl_largess_3 = promoteModel.sl_largess_3;
            promote2.zh_group = zh_group;
            promote2.rq_create = date;
            return promote2;
        }
        private List<Td_Promote_2> GetZsspList(BaseResult res, PromoteViewModel model, Td_Promote_1 promoteModel, int begin_xh = 0)
        {
            List<Td_Promote_2> promote2S = new List<Td_Promote_2>();
            var date = DateTime.Now;
            if (!string.IsNullOrEmpty(model.zs_sp_1))
            {
                var _promote2S = JSON.Deserialize<List<Td_Promote_2>>(model.zs_sp_1);
                if (_promote2S.Any())
                {
                    for (int i = 0; i < _promote2S.Count; i++)
                    {
                        var item = _promote2S[i];
                        item.id_masteruser = model.id_masteruser;
                        item.id = GetGuid;
                        item.id_bill = promoteModel.id;
                        item.sort_id = i + 1;
                        item.zh_group = "zs1";
                        item.rq_create = date;
                        begin_xh++;
                    }
                    promote2S.AddRange(_promote2S);
                }
                if (promoteModel.sl_largess_1 > 0 && _promote2S.Count <= 0)
                {
                    res.Success = false;
                    res.Message.Add("请选择赠送商品1");
                    return promote2S;
                }
                if (promoteModel.sl_largess_1 <= 0 && _promote2S.Count > 0)
                {
                    res.Success = false;
                    res.Message.Add("请填写赠送数据1");
                    return promote2S;
                }
            }
            if (!string.IsNullOrEmpty(model.zs_sp_2))
            {
                var _promote2S = JSON.Deserialize<List<Td_Promote_2>>(model.zs_sp_2);
                if (_promote2S.Any())
                {
                    for (int i = 0; i < _promote2S.Count; i++)
                    {
                        var item = _promote2S[i];
                        item.id_masteruser = model.id_masteruser;
                        item.id = GetGuid;
                        item.id_bill = promoteModel.id;
                        item.sort_id = i + 1;
                        item.zh_group = "zs2";
                        item.rq_create = date;
                        begin_xh++;
                    }
                    promote2S.AddRange(_promote2S);
                }
                if (promoteModel.sl_largess_2 > 0 && _promote2S.Count <= 0)
                {
                    res.Success = false;
                    res.Message.Add("请选择赠送商品2");
                    return promote2S;
                }
                if (promoteModel.sl_largess_2 <= 0 && _promote2S.Count > 0)
                {
                    res.Success = false;
                    res.Message.Add("请填写赠送数据2");
                    return promote2S;
                }
            }
            if (!string.IsNullOrEmpty(model.zs_sp_3))
            {
                var _promote2S = JSON.Deserialize<List<Td_Promote_2>>(model.zs_sp_3);
                if (_promote2S.Any())
                {
                    for (int i = 0; i < _promote2S.Count; i++)
                    {
                        var item = _promote2S[i];
                        item.id_masteruser = model.id_masteruser;
                        item.id = GetGuid;
                        item.id_bill = promoteModel.id;
                        item.sort_id = i + 1;
                        item.zh_group = "zs3";
                        item.rq_create = date;
                        begin_xh++;
                    }
                    promote2S.AddRange(_promote2S);
                }
                if (promoteModel.sl_largess_3 > 0 && _promote2S.Count <= 0)
                {
                    res.Success = false;
                    res.Message.Add("请选择赠送商品3");
                    return promote2S;
                }
                if (promoteModel.sl_largess_3 <= 0 && _promote2S.Count > 0)
                {
                    res.Success = false;
                    res.Message.Add("请填写赠送数据3");
                    return promote2S;
                }
            }
            return promote2S;
        }
        /// <summary>
        /// 处理促销门店
        /// </summary>
        /// <param name="id_bill"></param>
        /// <param name="id_shops"></param>
        /// <param name="id_masteruser"></param>
        /// <param name="id_user"></param>
        private void HandleShop(string id_bill, string id_shops, string id_masteruser, string id_user, string addOrUpdate = "add")
        {
            if (string.IsNullOrWhiteSpace(id_bill) || string.IsNullOrEmpty(id_shops) || string.IsNullOrEmpty(id_masteruser))
            {
                return;
            }
            var arr = id_shops.Split(',');
            List<Td_Promote_Shop> promoteShops = new List<Td_Promote_Shop>();
            var date = DateTime.Now;
            foreach (var a in arr)
            {
                promoteShops.Add(new Td_Promote_Shop()
                {
                    flag_delete = (byte)Enums.FlagDelete.NoDelete,
                    id = GetGuid,
                    id_bill = id_bill,
                    id_create = id_user,
                    id_masteruser = id_masteruser,
                    id_shop = a,
                    rq_create = date
                });
            }
            if (promoteShops.Any())
            {
                if (addOrUpdate == "update")
                {
                    Hashtable param = new Hashtable();
                    param.Add("id_masteruser", id_masteruser);
                    param.Add("id_bill", id_bill);
                    DAL.Delete(typeof(Td_Promote_Shop), param);
                }
                DAL.AddRange(promoteShops);
            }
        }
        #endregion

        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            if (param != null 
                && !param.ContainsKey("id_masteruser") 
                && !param.ContainsKey("id")
                && !param.ContainsKey("id_shop"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            var id_shop = string.Format("{0}", param["id_shop"]);
            param.Remove("id_shop");
            UpdatePromoteViewModel updatePromote = new UpdatePromoteViewModel();
            updatePromote.Promote1 = DAL.GetItem<Td_Promote_1>(typeof(Td_Promote_1), param);
            if (updatePromote.Promote1 != null)
            {
                //var id_shop = updatePromote.Promote1.id_shop;//string.Format("{0}", param["id_shop"]);
                param.Clear();
                param.Add("id_masteruser", updatePromote.Promote1.id_masteruser);
                param.Add("id_bill", updatePromote.Promote1.id);
                //updatePromote.Promote2S = DAL.QueryList<Td_Promote_2>(typeof(Td_Promote_2), param).ToList();
                updatePromote.Promote2S = new List<Td_Promote_2_Query>();
                updatePromote.Shops = DAL.QueryList<Td_Promote_Shop>(typeof(Td_Promote_Shop), param).ToList();
                if (updatePromote.Promote1.bm_djlx == "CX120"
                    || updatePromote.Promote1.bm_djlx == "CX230")
                {
                    param.Clear();
                    param.Add("id_bill", updatePromote.Promote1.id);
                    param.Add("zh_group", "A");
                    var list = Td_Promote_2DAL.QueryListWithSpfl(param);
                    if (list != null && list.Any())
                    {
                        updatePromote.Promote2S.AddRange(list);
                    }
                    param.Clear();
                    param.Add("id_bill", updatePromote.Promote1.id);
                    param.Add("not_zh_group", "A");
                    param.Add("sp_id_shop", id_shop);
                    var list1 = Td_Promote_2DAL.QueryListWithSp(param);
                    if (list1 != null && list1.Any())
                    {
                        updatePromote.Promote2S.AddRange(list1);
                    }
                }
                else
                {
                    param.Clear();
                    param.Add("id_bill", updatePromote.Promote1.id);
                    param.Add("sp_id_shop",id_shop);
                    //param.Add("zh_group", "A");
                    var list1 = Td_Promote_2DAL.QueryListWithSp(param);
                    if (list1 != null && list1.Any())
                    {
                        updatePromote.Promote2S.AddRange(list1);
                    }
                }
            }
            res.Data = updatePromote;
            return res;
        }
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult res = new BaseResult() { Success = true };
            var model = entity as PromoteViewModel;
            if (!CheckParam(model, res))
            {
                return res;
            }
            if (string.IsNullOrEmpty(model.id))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            Hashtable param = new Hashtable();
            param.Add("id", model.id);
            param.Add("id_masteruser", model.id_masteruser);
            var promoteModel = DAL.GetItem<Td_Promote_1>(typeof(Td_Promote_1), param);
            param.Clear();
            param.Add("new_id_jbr", model.id_jbr);
            param.Add("new_day_b", model.day_b);
            param.Add("new_day_e", new DateTime(model.day_e.Year, model.day_e.Month, model.day_e.Day, 23, 59, 59));
            param.Add("new_time_b", model.time_b.Replace(":", "").Replace("：", ""));
            param.Add("new_time_e", model.time_e.Replace(":", "").Replace("：", ""));
            param.Add("new_flag_rqfw", model.flag_rqfw);
            param.Add("new_flag_zsz", model.flag_zsz);
            param.Add("new_weeks", model.weeks ?? "");
            param.Add("new_days", model.days ?? "");
            param.Add("new_rule_name", model.cxzt);
            param.Add("new_examine", model.jsfs);
            param.Add("new_strategy", model.jsgz);
            param.Add("new_sl_largess_1", model.sl_largess_1);
            param.Add("new_sl_largess_2", model.sl_largess_2);
            param.Add("new_sl_largess_3", model.sl_largess_3);
            param.Add("new_sd1", string.Format("{0}", model.sd1 < 0 ? "" : model.sd1.ToString()));
            param.Add("new_sd2", string.Format("{0}", model.sd2 < 0 ? "" : model.sd2.ToString()));
            param.Add("new_sd3", string.Format("{0}", model.sd3 < 0 ? "" : model.sd3.ToString()));
            param.Add("new_condition_1", model.condition_1);
            param.Add("new_condition_2", model.condition_2);
            param.Add("new_condition_3", model.condition_3);
            param.Add("new_result_1", model.result_1);
            param.Add("new_result_2", model.result_2);
            param.Add("new_result_3", model.result_3);
            param.Add("new_id_hyfl_list", model.hylx);
            param.Add("new_id_edit", model.id_user);
            param.Add("new_rq_edit", DateTime.Now);
            param.Add("id", model.id);
            param.Add("id_masteruser", model.id_masteruser);
            DAL.UpdatePart(typeof(Td_Promote_1), param);
            param.Clear();
            param.Add("id_bill", model.id);
            DAL.Delete(typeof(Td_Promote_2), param);
            HandleStyle(res, model, promoteModel);
            HandleShop(model.id, model.id_shops, model.id_masteruser, model.id_user, "update");
            return res;
        }

        /// <summary>
        /// 审核促销单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Active(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            Sh<Td_Promote_1>(res, param, "p_promote_sh");
            return res;
        }

        private bool ShCxd(BaseResult res, Hashtable param)
        {
            if (param == null || !param.ContainsKey("id") || !param.ContainsKey("id_masteruser") || !param.ContainsKey("id_user"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return false;
            }
            var id = param["id"];
            var id_masteruser = param["id_masteruser"];
            var id_user = param["id_user"];
            param.Clear();
            param.Add("id", id);
            param.Add("id_masteruser", id_masteruser);
            var promoteModel = DAL.GetItem<Td_Promote_1>(typeof(Td_Promote_1), param);
            if (promoteModel == null)
            {
                res.Success = false;
                res.Message.Add("数据已不存在!");
                return false;
            }
            param.Add("proname", "p_promote_sh");
            param.Add("errorid", "-1");
            param.Add("errormessage", "未知错误！");
            param.Add("id_bill", promoteModel.id);
            param.Add("id_user", id_user);
            DAL.RunProcedure(param);
            if (!param.ContainsKey("errorid") || !param.ContainsKey("errormessage"))
            {
                res.Success = false;
                res.Message.Add("操作出现异常!");
                throw new CySoftException(res);
            }

            if (!string.IsNullOrEmpty(param["errorid"].ToString()) || !string.IsNullOrEmpty(param["errormessage"].ToString()))
            {
                res.Success = false;
                res.Message.Add(param["errormessage"].ToString());
                throw new CySoftException(res);
            }
            res.Success = true;
            res.Message.Add("操作成功!");
            return true;
        }
        /// <summary>
        /// 作废促销单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult Stop(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            Sh<Td_Promote_1>(res, param, "p_promote_cancel");
            return res;
        }
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            if (param == null || !param.ContainsKey("id") || !param.ContainsKey("id_user") || !param.ContainsKey("id_masteruser"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            var id = param["id"];
            var id_user = param["id_user"];
            var id_masteruser = param["id_masteruser"];
            param.Clear();
            param.Add("id", id);
            param.Add("id_masteruser", id_masteruser);
            param.Add("new_flag_delete", (byte)Enums.FlagDelete.Deleted);
            param.Add("flag_sh", (byte)Enums.FlagSh.UnSh);
            if (DAL.UpdatePart(typeof(Td_Promote_1), param) > 0)
            {
                param.Clear();
                param.Add("id_bill", id);
                param.Add("id_masteruser", id_masteruser);
                param.Add("new_flag_delete", (byte)Enums.FlagDelete.Deleted);
                DAL.UpdatePart(typeof(Td_Promote_Shop), param);
            }
            else
            {
                res.Success = false;
                res.Message.Add("操作失败!");
                return res;
            }
            return res;
        }

    }
}
