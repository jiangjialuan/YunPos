using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.IDAL;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using CySoft.Model.Td;
using CySoft.Model.Tz;
using CySoft.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.BLL.MemberBLL
{
    public class Tz_Hy_JeBLL : BaseBLL
    {
        #region IDAL
        public ITz_Hy_JeDAL Tz_Hy_JeDAL { get; set; }
        public ITz_Hy_Je_FlowDAL Tz_Hy_Je_FlowDAL { get; set; }
        public ITd_Hy_Cz_1DAL Td_Hy_Cz_1DAL { get; set; }

        #endregion

        #region 获取会员金额
        /// <summary>
        /// 获取会员金额
        /// lz
        /// 2016-10-18
        /// </summary>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };

            #region 检查会员状态
            var br = base.CheckHY(param);
            if (!br.Success)
                throw new CySoftException(br);
            #endregion

            try
            {
                var brJe = this.GetOne(param);
                if (!brJe.Success)
                {
                    res.Success = false;
                    res.Message.Add("会员金额数据不存在!");
                }
                else
                {
                    var dbInfo = (Tz_Hy_Je)brJe.Data;
                    if (dbInfo == null || string.IsNullOrEmpty(dbInfo.id))
                    {
                        res.Success = true;
                        res.Data = new Tz_Hy_Je() { je_qm = 0, je_qm_zs = 0 };
                        res.Message.Add("查询成功!");

                    }
                    else
                    {
                        if (RecordKeyError(dbInfo))
                        {
                            res.Success = false;
                            res.Message.Add("会员金额数据非法!");
                        }
                        else if (ZBError(dbInfo))
                        {
                            res.Success = false;
                            res.Message.Add("会员金额账本数据非法!");
                        }
                        else
                        {
                            res.Data = dbInfo;
                            res.Message.Add("查询成功!");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message.Add("系统异常!");
            }
            return res;
        }

        #endregion

        #region 充值/消费
        /// <summary>
        /// 新增
        /// lz
        /// 2016-09-19
        /// </summary>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            return this.AddWork(entity);
        }

        public BaseResult AddWork(dynamic entity)
        {
            #region 获取数据
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            #endregion

            #region 验证参数
            if (!param.ContainsKey("Type") || string.IsNullOrEmpty(param["Type"].ToString()))
            {
                br.Success = false;
                br.Message.Add("业务数据异常!");
                return br;
            }

            if (decimal.Parse(param["je"].ToString()) == 0)
            {
                br.Success = false;
                br.Message.Add("操作金额不能等于0!");
                return br;
            }
            #endregion

            if (param["Type"].ToString() == "CZ")
            {
                #region 充值
                #region 检查会员是否有效
                br = base.CheckHY(param);
                if (!br.Success)
                    throw new CySoftException(br);
                Tb_Hy_Detail hy_detail = (Tb_Hy_Detail)br.Data;
                #endregion
                #region 检查是否符合充值设置的金额
                if (decimal.Parse(param["je"].ToString()) > 0)
                {
                    br = this.CheckCZMaxMoney(param);
                    if (!br.Success)
                        throw new CySoftException(br);
                }
                #endregion
                #region 获取赠送金额等信息
                hy_detail.Tb_Hy_Shop.id_shop = param["id_shop"].ToString();
                var czrule_zssp_list = new List<Tb_Hy_Czrule_Zssp_Api_Query>();
                decimal je_zs = GetJe_Qm_Zs(hy_detail, decimal.Parse(param["je"].ToString()), ref czrule_zssp_list);
                string dh = GetNewDH(param["id_masteruser"].ToString(), param["id_shop"].ToString(), Enums.FlagDJLX.DHCZ);
                string dh_pay = "";
                if (param.ContainsKey("dh") && !string.IsNullOrEmpty(param["dh"].ToString()))
                    dh = param["dh"].ToString();
                if (param.ContainsKey("dh_pay") && !string.IsNullOrEmpty(param["dh_pay"].ToString()))
                    dh_pay = param["dh_pay"].ToString();
                string id = GetGuid;
                decimal je_ye = 0;
                #endregion
                #region 更新/插入Tz_Hy_Je表
                ht.Clear();
                ht.Add("id_shop", param["id_shop"].ToString());
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id_hy", param["id_hy"].ToString());
                var brJe = this.GetOne(ht);
                if (!brJe.Success)
                    throw new CySoftException(br);
                else
                {
                    var dbJeModel = (Tz_Hy_Je)brJe.Data;
                    if (dbJeModel != null && !string.IsNullOrEmpty(dbJeModel.id))
                    {
                        #region 更新数据
                        if (RecordKeyError(dbJeModel))
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add("会员金额数据非法!");
                            throw new CySoftException(br);
                        }
                        else if (ZBError(dbJeModel))
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add("会员金额账本数据非法!");
                            throw new CySoftException(br);
                        }
                        else
                        {
                            if (decimal.Parse(param["je"].ToString()) < 0 && (dbJeModel.je_qm + decimal.Parse(param["je"].ToString())) < 0)
                            {
                                br.Success = false;
                                br.Message.Clear();
                                br.Message.Add(String.Format("操作失败，账户余额(" + dbJeModel.je_qm + ") + 充值金额(" + decimal.Parse(param["je"].ToString()) + ")  不能为负！"));
                                throw new CySoftException(br);
                            }
                            je_ye = (decimal)dbJeModel.je_qm+ (decimal)dbJeModel.je_qm_zs;
                            dbJeModel.je_qm = dbJeModel.je_qm + decimal.Parse(param["je"].ToString());
                            dbJeModel.je_qm_zs = dbJeModel.je_qm_zs + je_zs;
                            dbJeModel.recodekey = GetRecordKey(dbJeModel);
                            DAL.Update(dbJeModel);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 插入数据
                        Tz_Hy_Je addModel = new Tz_Hy_Je()
                        {
                            id_masteruser = param["id_masteruser"].ToString(),
                            id = GetGuid,
                            id_hy = param["id_hy"].ToString(),
                            je_qm = 0,
                            je_qm_zs = 0
                        };

                        if (ZBError(addModel))
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add("会员金额账本数据非法!");
                            throw new CySoftException(br);
                        }

                        if (decimal.Parse(param["je"].ToString()) < 0)
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add(String.Format("操作失败，账户余额(0) + 充值金额(" + decimal.Parse(param["je"].ToString()) + ")  不能为负！"));
                            throw new CySoftException(br);
                        }

                        addModel.je_qm = decimal.Parse(param["je"].ToString());
                        addModel.je_qm_zs = je_zs;
                        addModel.recodekey = GetRecordKey(addModel);
                        DAL.Add(addModel);
                        #endregion
                    }
                }
                #endregion
                #region 插入流水表
                ht.Clear();
                ht.Add("id", GetGuid);
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id_bill", id);
                ht.Add("bm_djlx", "HY020");
                ht.Add("rq", DateTime.Parse(param["rq"].ToString()));
                ht.Add("id_shop", param["id_shop"].ToString());
                ht.Add("id_hy", param["id_hy"].ToString());
                ht.Add("je", decimal.Parse(param["je"].ToString()));
                ht.Add("je_zs", je_zs);
                ht.Add("bz", param["bz"].ToString());
                var addFlowNum = Tz_Hy_Je_FlowDAL.AddFlowWithExists(typeof(Tz_Hy_Je_Flow), ht);
                if (addFlowNum == 0)
                {
                    br.Success = false;
                    br.Message.Clear();
                    br.Message.Add(string.Format("充值单据已存在!"));
                    throw new CySoftException(br);
                }
                #endregion
                #region 插入表头
                ht.Clear();
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id", id);
                ht.Add("dh", dh);
                ht.Add("rq", DateTime.Parse(param["rq"].ToString()));
                ht.Add("id_shop", param["id_shop"].ToString());
                ht.Add("bm_djlx", "HY020");
                ht.Add("id_pay", param["id_pay"].ToString());
                ht.Add("id_jbr", param["id_create"].ToString());
                ht.Add("je_mxtotal", decimal.Parse(param["je"].ToString()));
                ht.Add("flag_sh", (byte)Enums.FlagSh.HadSh);
                ht.Add("flag_cancel", (byte)Enums.FlagCancel.NoCancel);
                ht.Add("bz", param["bz"].ToString());
                ht.Add("id_create", param["id_create"].ToString());
                ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                ht.Add("id_user_sh", param["id_create"].ToString());
                ht.Add("rq_sh", DateTime.Parse(param["rq"].ToString()));
                ht.Add("dh_pay", dh_pay);

                var addHeadNum = Td_Hy_Cz_1DAL.AddWithExists(typeof(Td_Hy_Cz_1), ht);
                if (addHeadNum == 0)
                {
                    br.Success = false;
                    br.Message.Clear();
                    br.Message.Add(string.Format("充值单据已存在!"));
                    throw new CySoftException(br);
                }
                #endregion
                #region 插入表体
                var td_Hy_Cz_2 = new Td_Hy_Cz_2()
                {
                    id_masteruser = param["id_masteruser"].ToString(),
                    id = GetGuid,
                    id_hy = param["id_hy"].ToString(),
                    id_bill = id,
                    sort_id = 1,
                    je = decimal.Parse(param["je"].ToString()),
                    je_zs = je_zs,
                    bz = "接口充值",
                    rq_create = DateTime.Now,
                    je_ye= je_ye
                };
                DAL.Add(td_Hy_Cz_2);
                #endregion
                #region 读取充值后金额 并验证
                ht.Clear();
                ht.Add("id_shop", param["id_shop"].ToString());
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id_hy", param["id_hy"].ToString());
                var brJeNow = this.Get(ht);
                if (!brJeNow.Success)
                {
                    br.Success = false;
                    br.Message.Add(string.Format("操作失败 查询金额失败 !"));
                    throw new CySoftException(br);
                }
                else
                {
                    //回写 Td_Hy_Cz_2 的 je_ye 为当前的余额 目前取消掉了
                    var dbJeModel = (Tz_Hy_Je)brJeNow.Data;
                    //ht.Clear();
                    //ht.Add("id", td_Hy_Cz_2.id);
                    //ht.Add("id_masteruser", param["id_masteruser"].ToString());
                    //ht.Add("new_je_ye", dbJeModel.je_qm);
                    //DAL.UpdatePart(typeof(Td_Hy_Cz_2), ht);

                    if (ZBError(dbJeModel))
                    {
                        br.Success = false;
                        br.Message.Clear();
                        br.Message.Add("操作失败 会员金额账本数据非法 !");
                        throw new CySoftException(br);
                    }
                }
                #endregion
                #region 返回
                var jeNowModel = (Tz_Hy_Je)brJeNow.Data;
                br.Message.Add(String.Format("操作成功!"));
                br.Success = true;
                br.Data = new { add_je = decimal.Parse(param["je"].ToString()), add_je_zs = je_zs, je_qm = jeNowModel.je_qm, je_qm_zs = jeNowModel.je_qm_zs, id = id, czrule_zssp = czrule_zssp_list };
                return br;
                #endregion 
                #endregion
            }
            else if (param["Type"].ToString() == "XF")
            {
                #region 消费

                #region 检查会员是否有效
                br = base.CheckHY(param);
                if (!br.Success)
                    throw new CySoftException(br);

                var tempModel = new Tz_Hy_Je() { je_qm = decimal.Parse(param["je"].ToString()), je_qm_zs = 0 };
                #endregion

                #region 验证会员金额是否非法 并更新账户以及插入流水帐
                ht.Clear();
                ht.Add("id_shop", param["id_shop"].ToString());
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id_hy", param["id_hy"].ToString());
                var brJe = this.GetOne(ht);
                if (!brJe.Success)
                    throw new CySoftException(br);
                else
                {
                    var dbJeModel = (Tz_Hy_Je)brJe.Data;
                    if (dbJeModel != null && !string.IsNullOrEmpty(dbJeModel.id))
                    {
                        #region 验证会员金额数据是否非法
                        if (RecordKeyError(dbJeModel))
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add("会员金额数据非法!");
                            throw new CySoftException(br);
                        }
                        #endregion
                        #region 验证会员金额账本数据是否非法
                        if (ZBError(dbJeModel))
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add("会员金额账本数据非法!");
                            throw new CySoftException(br);
                        }
                        #endregion
                        #region 插入流水表
                        tempModel = GetMonenyCost(decimal.Parse(param["je"].ToString()), dbJeModel);
                        ht.Clear();
                        ht.Add("id", GetGuid);
                        ht.Add("id_masteruser", param["id_masteruser"].ToString());
                        ht.Add("id_bill", param["id_bill"].ToString());
                        ht.Add("bm_djlx", param["bm_djlx"].ToString());
                        ht.Add("rq", DateTime.Parse(param["rq"].ToString()));
                        ht.Add("id_shop", param["id_shop"].ToString());
                        ht.Add("id_hy", param["id_hy"].ToString());
                        ht.Add("je", tempModel.je_qm * (-1));
                        ht.Add("je_zs", tempModel.je_qm_zs * (-1));
                        ht.Add("bz", param["bz"].ToString());
                        var addFlowNum = Tz_Hy_Je_FlowDAL.AddFlowForXFWithExists(typeof(Tz_Hy_Je_Flow), ht);
                        if (addFlowNum == 0)
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add("消费单据已存在!");
                            throw new CySoftException(br);
                        }

                        #endregion
                        #region 验证账户余额是否充足
                        if (dbJeModel.je_qm < tempModel.je_qm)
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add(String.Format("操作失败，正常账户余额(" + dbJeModel.je_qm + ") < 正常消费金额(" + tempModel.je_qm + ")！"));
                            throw new CySoftException(br);
                        }

                        if (dbJeModel.je_qm_zs < tempModel.je_qm_zs)
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add(String.Format("操作失败，赠送账户余额(" + dbJeModel.je_qm_zs + ") < 赠送消费金额(" + tempModel.je_qm_zs + ")！"));
                            throw new CySoftException(br);
                        }
                        #endregion
                        #region 更新数据
                        dbJeModel.je_qm = dbJeModel.je_qm - tempModel.je_qm;
                        dbJeModel.je_qm_zs = dbJeModel.je_qm_zs - tempModel.je_qm_zs;
                        dbJeModel.recodekey = GetRecordKey(dbJeModel);
                        DAL.Update(dbJeModel);
                        #endregion
                    }
                    else
                    {
                        #region 操作失败 会员还未充值过
                        br.Success = false;
                        br.Message.Clear();
                        br.Message.Add(string.Format("操作失败 会员还未充值过!"));
                        throw new CySoftException(br);
                        #endregion
                    }
                }
                #endregion

                #region 查询消费后会员的金额
                ht.Clear();
                ht.Add("id_shop", param["id_shop"].ToString());
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id_hy", param["id_hy"].ToString());
                var brJeNow = this.Get(ht);
                if (!brJeNow.Success)
                {
                    br.Success = false;
                    br.Message.Clear();
                    br.Message.Add(string.Format("操作完毕 查询金额失败!"));
                    throw new CySoftException(br);
                }
                else
                {
                    var dbJeModel = (Tz_Hy_Je)brJeNow.Data;
                    if (ZBError(dbJeModel))
                    {
                        br.Success = false;
                        br.Message.Clear();
                        br.Message.Add("操作失败 会员金额账本数据非法 !");
                        throw new CySoftException(br);
                    }
                }
                #endregion

                #region 返回
                var jeNowModel = (Tz_Hy_Je)brJeNow.Data;
                br.Message.Add(String.Format("操作成功!"));
                br.Success = true;
                br.Data = new { add_je = tempModel.je_qm, add_je_zs = tempModel.je_qm_zs, je_qm = jeNowModel.je_qm, je_qm_zs = jeNowModel.je_qm_zs };
                return br;
                #endregion 

                #endregion
            }
            else
            {
                #region 返回
                br.Message.Add(String.Format("操作失败,无此操作类型!"));
                br.Success = false;
                return br;
                #endregion 
            }
        }

        #endregion

        #region 获取充值赠送金额
        /// <summary>
        /// 获取充值赠送金额
        /// lz
        /// 2016-09-19
        /// </summary>
        /// <returns></returns>
        public decimal GetJe_Qm_Zs(Tb_Hy_Detail hy_detail, decimal je_cz, ref List<Tb_Hy_Czrule_Zssp_Api_Query> czrule_zssp)
        {
            #region 初始过滤条件
            if (hy_detail == null || hy_detail.Tb_Hy_Shop == null)
                return 0;

            var abs_je_cz = Math.Abs(je_cz);

            #endregion
            #region 获取符合规则的表头信息
            Hashtable ht = new Hashtable();
            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            ht.Add("flag_stop", (byte)Enums.FlagStop.Start);
            ht.Add("id_masteruser", hy_detail.Tb_Hy_Shop.id_masteruser);
            var head_list_all = DAL.QueryList<Tb_Hy_Czrule_Query>(typeof(Tb_Hy_Czrule), ht).ToList();
            if (head_list_all == null || head_list_all.Count() <= 0)
                return 0;
            var head_list = head_list_all.Where(d => d.day_b <= DateTime.Now && d.day_e >= DateTime.Now && d.je_cz <= abs_je_cz && (d.id_shop == "0" || d.id_shop == hy_detail.Tb_Hy_Shop.id_shop) && (d.id_hyfl == "0" || d.id_hyfl == hy_detail.Tb_Hy_Shop.id_hyfl)).ToList();

            if (head_list == null || head_list.Count() <= 0)
                return 0;

            var czrule_head = head_list.Where(d => d.id_shop == hy_detail.Tb_Hy_Shop.id_shop && d.id_hyfl == hy_detail.Tb_Hy_Shop.id_hyfl).OrderByDescending(d => d.je_cz).FirstOrDefault();
            if (czrule_head == null || string.IsNullOrEmpty(czrule_head.id))
                czrule_head = head_list.Where(d => d.id_shop == "0" && d.id_hyfl == hy_detail.Tb_Hy_Shop.id_hyfl).OrderByDescending(d => d.je_cz).FirstOrDefault();
            if (czrule_head == null || string.IsNullOrEmpty(czrule_head.id))
                czrule_head = head_list.Where(d => d.id_shop == hy_detail.Tb_Hy_Shop.id_shop && d.id_hyfl == "0").OrderByDescending(d => d.je_cz).FirstOrDefault();
            if (czrule_head == null || string.IsNullOrEmpty(czrule_head.id))
                czrule_head = head_list.Where(d => d.id_shop == "0" && d.id_hyfl == "0").OrderByDescending(d => d.je_cz).FirstOrDefault();

            if (czrule_head == null || string.IsNullOrEmpty(czrule_head.id))
                return 0;

            //var czrule_head = head_list.OrderByDescending(d => d.je_cz).FirstOrDefault();

            #endregion
            #region 获取符合规则的表体信息
            ht.Clear();
            ht.Add("id_bill", czrule_head.id_bill);
            var czrule_body = DAL.QueryList<Tb_Hy_Czrule_Zssp_Query>(typeof(Tb_Hy_Czrule_Zssp), ht).ToList() ?? new List<Tb_Hy_Czrule_Zssp_Query>();
            czrule_zssp = new List<Tb_Hy_Czrule_Zssp_Api_Query>();
            foreach (var item in czrule_body)
            {
                czrule_zssp.Add(new Tb_Hy_Czrule_Zssp_Api_Query() { id_sp = item.id_sp, sl = (decimal)item.sl });
            }
            #endregion
            #region 获取赠送的金额
            decimal zsje = 0;
            czrule_head.je_cz_zs = czrule_head.je_cz_zs == null ? 0 : (decimal)czrule_head.je_cz_zs;
            czrule_head.je_cz = czrule_head.je_cz == null ? 0 : (decimal)czrule_head.je_cz;
            if (czrule_head.je_cz_zs != 0 && czrule_head.je_cz != 0)
            {
                int ruleInt = (int)((decimal)(abs_je_cz / czrule_head.je_cz));
                zsje = (decimal)czrule_head.je_cz_zs * ruleInt;
                if (je_cz < 0)
                    zsje = -1 * zsje;
            }
            #endregion
            return zsje;
        }
        #endregion

        #region GetOne
        /// <summary>
        /// GetOne
        /// lz
        /// 2016-10-18
        /// </summary>
        public BaseResult GetOne(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            try
            {
                var dbInfo = DAL.GetItem<Tz_Hy_Je>(typeof(Tz_Hy_Je), param);
                if (dbInfo == null || string.IsNullOrEmpty(dbInfo.id))
                {
                    res.Data = new Tz_Hy_Je();
                    res.Message.Add("会员金额数据不存在!");
                }
                else
                {
                    res.Data = dbInfo;
                    res.Message.Add("查询成功!");
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message.Add("系统异常!");
            }
            return res;
        }
        #endregion

        #region 检查账本是否错误
        /// <summary>
        /// 检查账本是否错误
        /// lz
        /// 2016-09-20
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ZBError(Tz_Hy_Je model)
        {
            try
            {
                bool result = true;
                if (model != null && !string.IsNullOrEmpty(model.id_masteruser) && !string.IsNullOrEmpty(model.id_hy))
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("id_masteruser", model.id_masteruser);
                    ht.Add("id_hy", model.id_hy);
                    var flowList = DAL.QueryList<Tz_Hy_Je_Flow>(typeof(Tz_Hy_Je_Flow), ht);
                    if (flowList != null && flowList.Count() > 0)
                    {
                        var flowJe = flowList.Sum(d => d.je);
                        var flowJeZs = flowList.Sum(d => d.je_zs);
                        if (model.je_qm == flowJe && model.je_qm_zs == flowJeZs)
                            return false;
                    }
                    else
                    {
                        if (model.je_qm == 0 && model.je_qm_zs == 0)
                            return false;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        #endregion

        #region 检查数据秘钥是否有效
        /// <summary>
        /// 检查数据秘钥是否有效
        /// lz
        /// 2016-09-19
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool RecordKeyError(Tz_Hy_Je model)
        {
            try
            {
                bool result = true;
                if (model != null)
                {
                    string recordKey = this.GetRecordKey(model);
                    if (recordKey == model.recodekey)
                        return false;
                }
                return result;
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        #endregion

        #region 获取指定model的秘钥
        /// <summary>
        /// 获取指定model的秘钥
        /// lz
        /// 2016-09-19
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetRecordKey(Tz_Hy_Je model)
        {
            try
            {
                string RecordKey = "";
                if (model != null)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(model.id_masteruser);
                    strSql.Append(model.id);
                    strSql.Append(model.id_hy);
                    strSql.Append(double.Parse(model.je_qm.ToString()).ToString("0.0000000"));
                    strSql.Append(double.Parse(model.je_qm_zs.ToString()).ToString("0.0000000"));
                    strSql.Append(PublicSign.sign.ToString());
                    RecordKey = MD5Encrypt.Md5(strSql.ToString());
                }
                return RecordKey;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

        #region 根据消费金额和余额获取消费的赠送金额
        /// <summary>
        /// 根据消费金额和余额获取消费的赠送金额
        /// </summary>
        /// <param name="money"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public Tz_Hy_Je GetMonenyCost(decimal money, Tz_Hy_Je balance)
        {
            try
            {
                Tz_Hy_Je jeModel = new Tz_Hy_Je();
                jeModel.je_qm = money;
                jeModel.je_qm_zs = 0;

                if (balance != null)
                {
                    if (balance.je_qm > 0 && balance.je_qm_zs <= 0)
                    {
                        jeModel.je_qm = money;
                        jeModel.je_qm_zs = 0;
                    }
                    else if (balance.je_qm <= 0 && balance.je_qm_zs > 0)
                    {
                        jeModel.je_qm = 0;
                        jeModel.je_qm_zs = money;
                    }
                    else if (balance.je_qm > 0 && balance.je_qm_zs > 0)
                    {
                        jeModel.je_qm = Frame.Common.OperationHelper.Digit(((money / (balance.je_qm + balance.je_qm_zs)) * balance.je_qm), 2);
                        jeModel.je_qm_zs = money - jeModel.je_qm;
                    }
                }

                return jeModel;
            }
            catch (Exception ex)
            {
                Tz_Hy_Je jeModel = new Tz_Hy_Je();
                jeModel.je_qm = money;
                jeModel.je_qm_zs = 0;
                return jeModel;
            }
        }
        #endregion

        #region 检查是否符合充值设置的金额
        /// <summary>
        /// 检查是否符合充值设置的金额
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseResult CheckCZMaxMoney(Hashtable param)
        {
            BaseResult br = new BaseResult();
            try
            {
                Tb_Hy_Detail hyDetailModel = new Tb_Hy_Detail();
                Hashtable ht = new Hashtable();
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id_shop", param["id_shop"].ToString());
                ht.Add("id_hy", param["id_hy"].ToString());
                ht.Add("bm_djlx", "HY020");
                var yczList = DAL.QueryList<Tz_Hy_Je_Flow>(typeof(Tz_Hy_Je_Flow), ht).ToList();

                decimal moneyMonthYCZ = 0;
                if (yczList != null && yczList.Count() > 0)
                    moneyMonthYCZ = (decimal)yczList.Sum(d => d.je);

                var shopParm = GetShopParm(param["id_masteruser"].ToString(), param["id_shop"].ToString());
                var shopParmAll = GetShopParm(param["id_masteruser"].ToString(), "0");

                if (shopParm != null && shopParm.ContainsKey("success") && shopParm["success"].ToString() == "1")
                {
                    #region hy_czje_min_onec
                    var hy_czje_min_onec = shopParm["hy_czje_min_onec"].ToString();
                    if (!string.IsNullOrEmpty(hy_czje_min_onec) && CyVerify.IsNumeric(hy_czje_min_onec))
                    {
                        if (decimal.Parse(param["je"].ToString()) < decimal.Parse(hy_czje_min_onec))
                        {
                            br.Success = false;
                            br.Message.Add("操作失败:每次最小金额至少为: " + hy_czje_min_onec + "  ");
                            return br;
                        }
                    }
                    else if (shopParmAll != null && shopParmAll.ContainsKey("success") && shopParmAll["success"].ToString() == "1")
                    {
                        var hy_czje_min_onec_all = shopParmAll["hy_czje_min_onec"].ToString();
                        if (!string.IsNullOrEmpty(hy_czje_min_onec_all) && CyVerify.IsNumeric(hy_czje_min_onec_all))
                        {
                            if (decimal.Parse(param["je"].ToString()) < decimal.Parse(hy_czje_min_onec_all))
                            {
                                br.Success = false;
                                br.Message.Add("操作失败:每次充值金额最小为: " + hy_czje_min_onec_all + " ");
                                return br;
                            }
                        }
                    }
                    #endregion

                    #region hy_czje_max_onec
                    var hy_czje_max_onec = shopParm["hy_czje_max_onec"].ToString();
                    if (!string.IsNullOrEmpty(hy_czje_max_onec) && CyVerify.IsNumeric(hy_czje_max_onec))
                    {
                        if (decimal.Parse(param["je"].ToString()) > decimal.Parse(hy_czje_max_onec))
                        {
                            br.Success = false;
                            br.Message.Add("操作失败:每次充值金额最大为: " + hy_czje_max_onec + "  ");
                            return br;
                        }
                    }
                    else if (shopParmAll != null && shopParmAll.ContainsKey("success") && shopParmAll["success"].ToString() == "1")
                    {
                        var hy_czje_max_onec_all = shopParmAll["hy_czje_max_onec"].ToString();
                        if (!string.IsNullOrEmpty(hy_czje_max_onec_all) && CyVerify.IsNumeric(hy_czje_max_onec_all))
                        {
                            if (decimal.Parse(param["je"].ToString()) > decimal.Parse(hy_czje_max_onec_all))
                            {
                                br.Success = false;
                                br.Message.Add("操作失败:每次充值金额最大为: " + hy_czje_max_onec_all + " ");
                                return br;
                            }
                        }
                    }
                    #endregion

                    #region hy_czje_max_month
                    var hy_czje_max_month = shopParm["hy_czje_max_month"].ToString();
                    if (!string.IsNullOrEmpty(hy_czje_max_month) && CyVerify.IsNumeric(hy_czje_max_month))
                    {
                        if ((moneyMonthYCZ + decimal.Parse(param["je"].ToString())) > decimal.Parse(hy_czje_max_month))
                        {
                            br.Success = false;
                            br.Message.Add("操作失败:每月充值最大金额: " + hy_czje_max_month + "  本月已充值: " + moneyMonthYCZ + "  ");
                            return br;
                        }
                    }
                    else if (shopParmAll != null && shopParmAll.ContainsKey("success") && shopParmAll["success"].ToString() == "1")
                    {
                        var hy_czje_max_month_all = shopParmAll["hy_czje_max_month"].ToString();
                        if (!string.IsNullOrEmpty(hy_czje_max_month_all) && CyVerify.IsNumeric(hy_czje_max_month_all))
                        {
                            if (moneyMonthYCZ + decimal.Parse(param["je"].ToString()) > decimal.Parse(hy_czje_max_month_all))
                            {
                                br.Success = false;
                                br.Message.Add("操作失败:每月充值最大金额: " + hy_czje_max_month_all + "  本月已充值: " + moneyMonthYCZ + "  ");
                                return br;
                            }
                        }
                    }
                    #endregion

                }
                else if (shopParmAll != null && shopParmAll.ContainsKey("success") && shopParmAll["success"].ToString() == "1")
                {
                    #region hy_czje_min_onec  hy_czje_max_onec  hy_czje_max_month
                    var hy_czje_min_onec = shopParm["hy_czje_min_onec"].ToString();
                    var hy_czje_max_onec = shopParm["hy_czje_max_onec"].ToString();
                    var hy_czje_max_month = shopParm["hy_czje_max_month"].ToString();

                    if (!string.IsNullOrEmpty(hy_czje_min_onec) && CyVerify.IsNumeric(hy_czje_min_onec))
                    {
                        if (decimal.Parse(param["je"].ToString()) < decimal.Parse(hy_czje_min_onec))
                        {
                            br.Success = false;
                            br.Message.Add("操作失败:每次充值金额最小为: " + hy_czje_min_onec + "  ");
                            return br;
                        }
                    }

                    if (!string.IsNullOrEmpty(hy_czje_max_onec) && CyVerify.IsNumeric(hy_czje_max_onec))
                    {
                        if (decimal.Parse(param["je"].ToString()) > decimal.Parse(hy_czje_max_onec))
                        {
                            br.Success = false;
                            br.Message.Add("操作失败:每次充值金额最大为: " + hy_czje_max_onec + "  ");
                            return br;
                        }
                    }

                    if (!string.IsNullOrEmpty(hy_czje_max_month) && CyVerify.IsNumeric(hy_czje_max_month))
                    {
                        if ((moneyMonthYCZ + decimal.Parse(param["je"].ToString())) > decimal.Parse(hy_czje_max_month))
                        {
                            br.Success = false;
                            br.Message.Add("操作失败:每月充值最大金额: " + hy_czje_max_month + "  本月已充值: " + moneyMonthYCZ + "  ");
                            return br;
                        }
                    }
                    #endregion
                }

                br.Success = true;
                return br;
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Message.Add("操作失败:检查是否符合充值设置的金额获取到异常 ");
                return br;
            }
        }
        #endregion

        #region 接口调用消费
        public override BaseResult Active(Hashtable entity)
        {
            return this.AddWork(entity);
        }
        #endregion

        #region 会员充值赠送金额读取
        /// <summary>
        /// 会员充值赠送金额读取
        /// lz
        /// 2016-12-12
        /// </summary>
        public override BaseResult Init(Hashtable entity)
        {
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult() { Success = true };

            #region 检查会员状态
            br = base.CheckHY(param);
            if (!br.Success)
                throw new CySoftException(br);
            Tb_Hy_Detail hy_detail = (Tb_Hy_Detail)br.Data;
            #endregion

            #region 检查是否符合充值设置的金额
            br = this.CheckCZMaxMoney(param);
            if (!br.Success)
                throw new CySoftException(br);
            #endregion

            #region 获取赠送金额等信息
            var czrule_zssp_list = new List<Tb_Hy_Czrule_Zssp_Api_Query>();
            hy_detail.Tb_Hy_Shop.id_shop = param["id_shop"].ToString();
            decimal je_zs = GetJe_Qm_Zs(hy_detail, decimal.Parse(param["je"].ToString()), ref czrule_zssp_list);
            #endregion

            #region 返回
            br.Message.Add(String.Format("操作成功!"));
            br.Success = true;
            br.Data = new { add_je = decimal.Parse(param["je"].ToString()), add_je_zs = je_zs, czrule_zssp = czrule_zssp_list };
            return br;
            #endregion

        }
        #endregion

        #region 会员密码验证
        /// <summary>
        /// 会员密码验证
        /// lz
        /// 2016-12-15
        /// </summary>
        public override BaseResult CheckStock(Hashtable entity)
        {
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult() { Success = true };

            #region 检查会员状态
            br = base.CheckHY(param);
            if (br.Success)
            {
                br.Message.Clear();
                br.Message.Add("查询成功 会员状态/密码正常");
                br.Data = null;
            }
            return br;
            #endregion

        }
        #endregion



    }
}
