using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.IDAL;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
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
    public class Tz_Hy_JfBLL : BaseBLL
    {
        #region IDAL
        public ITz_Hy_Jf_FlowDAL Tz_Hy_Jf_FlowDAL { get; set; }
        #endregion

        #region 获取会员积分
        /// <summary>
        /// 获取会员积分
        /// lz
        /// 2016-10-18
        /// </summary>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id_hy", param["id_hy"].ToString());
                var brJf = this.GetList(ht);

                if (!brJf.Success)
                {
                    res.Success = false;
                    res.Message.Add("查询积分数据异常 请重试!");
                }
                else
                {
                    var dbList = (List<Tz_Hy_Jf>)brJf.Data;
                    if (dbList == null || dbList.Count() <= 0)
                    {
                        res.Success = true;
                        res.Data = new Tz_Hy_Jf() { jf_qm = 0, id_masteruser = param["id_masteruser"].ToString(), id_hy = param["id_hy"].ToString() };
                        res.Message.Add("查询成功!");
                    }
                    else
                    {

                        foreach (var dbInfo in dbList)
                        {
                            if (RecordKeyError(dbInfo))
                            {
                                res.Success = false;
                                res.Message.Add("会员积分数据非法!");
                                return res;
                            }
                        }
                        var rModel = dbList.FirstOrDefault();
                        rModel.jf_qm = dbList.Sum(d => d.jf_qm);
                        if (ZBError(rModel))
                        {
                            res.Success = false;
                            res.Message.Add("会员积分账本数据非法!");
                            return res;
                        }
                        else
                        {
                            res.Data = new Tz_Hy_Jf() { jf_qm = rModel.jf_qm, id_masteruser = param["id_masteruser"].ToString(), id_hy = param["id_hy"].ToString() };
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

        #region 加/减积分
        /// <summary>
        /// 加/减积分
        /// lz
        /// 2016-10-21
        /// </summary>
        [Transaction]
        public override BaseResult Add(dynamic entity)
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

            if (decimal.Parse(param["jf"].ToString()) == 0)
            {
                br.Success = false;
                br.Message.Add("操作积分不能为0!");
                return br;
            }

            #endregion

            #region 检查会员是否有效
            br = base.CheckHY(param);
            if (!br.Success)
                throw new CySoftException(br);

            var tempModel = new Tz_Hy_Jf() { jf_qm = decimal.Parse(param["jf"].ToString()) };

            #endregion

            if (param["Type"].ToString() == "Add")
            {
                #region 加

                #region 更新/插入Tz_Hy_Jf表
                ht.Clear();
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id_hy", param["id_hy"].ToString());
                var brJf = this.GetList(ht);
                if (!brJf.Success)
                    throw new CySoftException(br);
                else
                {
                    var dbList = (List<Tz_Hy_Jf>)brJf.Data;
                    if (dbList == null || dbList.Count() <= 0)
                    {
                        #region 插入数据
                        Tz_Hy_Jf addModel = new Tz_Hy_Jf()
                        {
                            id_masteruser = param["id_masteruser"].ToString(),
                            id = GetGuid,
                            id_shop = param["id_shop"].ToString(),
                            id_hy = param["id_hy"].ToString(),
                            jf_qm = 0
                        };

                        if (ZBError(addModel))
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add("会员积分账本数据非法!");
                            throw new CySoftException(br);
                        }

                        addModel.jf_qm = decimal.Parse(param["jf"].ToString());
                        addModel.recodekey = GetRecordKey(addModel);
                        DAL.Add(addModel);
                        #endregion
                    }
                    else
                    {
                        #region 检验 RecordKey
                        foreach (var dbInfo in dbList)
                        {
                            if (RecordKeyError(dbInfo))
                            {
                                br.Success = false;
                                br.Message.Clear();
                                br.Message.Add("会员积分数据非法!");
                                throw new CySoftException(br);
                            }
                        }
                        #endregion

                        #region 检验 积分账本
                        var rModel = dbList.FirstOrDefault().Clone();
                        rModel.jf_qm = dbList.Sum(d => d.jf_qm);
                        if (ZBError(rModel))
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add("会员积分账本数据非法!");
                            throw new CySoftException(br);
                        }
                        #endregion

                        #region 执行更新或插入本 id_shop 的数据
                        var dbJfModel = dbList.Where(d => d.id_shop == param["id_shop"].ToString()).FirstOrDefault();
                        if (dbJfModel == null || string.IsNullOrEmpty(dbJfModel.id))
                        {
                            #region 插入数据
                            Tz_Hy_Jf addModel = new Tz_Hy_Jf()
                            {
                                id_masteruser = param["id_masteruser"].ToString(),
                                id = GetGuid,
                                id_shop = param["id_shop"].ToString(),
                                id_hy = param["id_hy"].ToString(),
                                jf_qm = decimal.Parse(param["jf"].ToString())
                            };
                            addModel.recodekey = GetRecordKey(addModel);
                            DAL.Add(addModel);
                            #endregion
                        }
                        else
                        {
                            #region 更新数据
                            dbJfModel.jf_qm = dbJfModel.jf_qm + decimal.Parse(param["jf"].ToString());
                            dbJfModel.recodekey = GetRecordKey(dbJfModel);
                            DAL.Update(dbJfModel);
                            #endregion
                        }
                        #endregion
                    }
                }
                #endregion

                #region 插入流水表
                ht.Clear();
                ht.Add("id", GetGuid);
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id_bill", param["id_bill"].ToString());
                ht.Add("bm_djlx", param["bm_djlx"].ToString());
                ht.Add("rq", DateTime.Parse(param["rq"].ToString()));
                ht.Add("id_shop", param["id_shop"].ToString());
                ht.Add("id_hy", param["id_hy"].ToString());
                ht.Add("jf", decimal.Parse(param["jf"].ToString()));
                ht.Add("bz", param["bz"].ToString());
                var addFlowNum = Tz_Hy_Jf_FlowDAL.AddWithExists(typeof(Tz_Hy_Jf_Flow), ht);
                if (addFlowNum == 0)
                {
                    br.Success = false;
                    br.Message.Clear();
                    br.Message.Add("积分单据已存在!");
                    throw new CySoftException(br);
                }
                #endregion

                #region 读取操作后积分 并验证
                ht.Clear();
                ht.Add("id_shop", param["id_shop"].ToString());
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id_hy", param["id_hy"].ToString());
                var brJfNow = this.Get(ht);
                if (!brJfNow.Success)
                {
                    br.Success = false;
                    br.Message.Add(string.Format("操作失败 查询积分失败 !"));
                    throw new CySoftException(br);
                }
                else
                {
                    var dbJfModel = (Tz_Hy_Jf)brJfNow.Data;
                    if (ZBError(dbJfModel))
                    {
                        br.Success = false;
                        br.Message.Clear();
                        br.Message.Add("操作失败 会员积分账本数据非法 !");
                        throw new CySoftException(br);
                    }
                }
                #endregion

                #region 返回
                var jfNowModel = (Tz_Hy_Jf)brJfNow.Data;
                br.Message.Add(String.Format("操作成功!"));
                br.Success = true;
                br.Data = new { add_jf = decimal.Parse(param["jf"].ToString()), jf_qm = jfNowModel.jf_qm };
                return br;
                #endregion 

                #endregion
            }
            else if (param["Type"].ToString() == "Del")
            {
                #region 减

                #region 更新Tz_Hy_Jf表
                ht.Clear();
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id_hy", param["id_hy"].ToString());
                var brJf = this.GetList(ht);
                if (!brJf.Success)
                    throw new CySoftException(br);
                else
                {

                    var dbList = (List<Tz_Hy_Jf>)brJf.Data;
                    if (dbList == null || dbList.Count() <= 0)
                    {
                        #region 操作失败 会员会员积分不足
                        br.Success = false;
                        br.Message.Clear();
                        br.Message.Add(string.Format("操作失败 会员积分为0 不允许消费积分!"));
                        throw new CySoftException(br);
                        #endregion
                    }
                    else
                    {
                        #region 检验 RecordKey
                        foreach (var dbInfo in dbList)
                        {
                            if (RecordKeyError(dbInfo))
                            {
                                br.Success = false;
                                br.Message.Clear();
                                br.Message.Add("会员积分数据非法!");
                                throw new CySoftException(br);
                            }
                        }
                        #endregion

                        #region 检验 积分账本
                        var rModel = dbList.FirstOrDefault().Clone();
                        rModel.jf_qm = dbList.Sum(d => d.jf_qm);
                        if (ZBError(rModel))
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add("会员积分账本数据非法!");
                            throw new CySoftException(br);
                        }
                        #endregion

                        #region 插入流水表
                        ht.Clear();
                        ht.Add("id", GetGuid);
                        ht.Add("id_masteruser", param["id_masteruser"].ToString());
                        ht.Add("id_bill", param["id_bill"].ToString());
                        ht.Add("bm_djlx", param["bm_djlx"].ToString());
                        ht.Add("rq", DateTime.Parse(param["rq"].ToString()));
                        ht.Add("id_shop", param["id_shop"].ToString());
                        ht.Add("id_hy", param["id_hy"].ToString());
                        ht.Add("jf", decimal.Parse(param["jf"].ToString()));
                        ht.Add("bz", param["bz"].ToString());
                        var addFlowNum = Tz_Hy_Jf_FlowDAL.AddWithExists(typeof(Tz_Hy_Jf_Flow), ht);
                        if (addFlowNum == 0)
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add("积分单据已存在!");
                            throw new CySoftException(br);
                        }
                        #endregion

                        #region 验证账户积分是否充足
                        if (rModel.jf_qm + tempModel.jf_qm < 0)
                        {
                            br.Success = false;
                            br.Message.Clear();
                            br.Message.Add(String.Format("操作失败，账户积分(" + rModel.jf_qm + ") 不足！"));
                            throw new CySoftException(br);
                        }
                        #endregion

                        var dbJfModel = dbList.Where(d => d.id_shop == param["id_shop"].ToString()).FirstOrDefault();
                        if (dbJfModel == null || string.IsNullOrEmpty(dbJfModel.id))
                        {
                            #region 插入数据
                            Tz_Hy_Jf addModel = new Tz_Hy_Jf()
                            {
                                id_masteruser = param["id_masteruser"].ToString(),
                                id = GetGuid,
                                id_shop = param["id_shop"].ToString(),
                                id_hy = param["id_hy"].ToString(),
                                jf_qm = decimal.Parse(param["jf"].ToString())
                            };
                            addModel.recodekey = GetRecordKey(addModel);
                            DAL.Add(addModel);
                            #endregion
                        }
                        else
                        {
                            #region 更新积分数据
                            dbJfModel.jf_qm = dbJfModel.jf_qm + decimal.Parse(param["jf"].ToString());
                            dbJfModel.recodekey = GetRecordKey(dbJfModel);
                            DAL.Update(dbJfModel);
                            #endregion
                        }
                    }
                }
                #endregion

                #region 读取操作后积分 并验证
                ht.Clear();
                ht.Add("id_shop", param["id_shop"].ToString());
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id_hy", param["id_hy"].ToString());
                var brJfNow = this.Get(ht);
                if (!brJfNow.Success)
                {
                    br.Success = false;
                    br.Message.Clear();
                    br.Message.Add(string.Format("操作失败 查询积分失败 !"));
                    throw new CySoftException(br);
                }
                else
                {
                    var dbJfModel = (Tz_Hy_Jf)brJfNow.Data;
                    if (ZBError(dbJfModel))
                    {
                        br.Success = false;
                        br.Message.Clear();
                        br.Message.Add("操作失败 会员积分账本数据非法 !");
                        throw new CySoftException(br);
                    }
                }
                #endregion

                #region 返回
                var jfNowModel = (Tz_Hy_Jf)brJfNow.Data;
                br.Message.Add(String.Format("操作成功!"));
                br.Success = true;
                br.Data = new { add_jf = decimal.Parse(param["jf"].ToString()), jf_qm = jfNowModel.jf_qm };
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

        #region 注释

        //#region 获取会员积分
        ///// <summary>
        ///// 获取会员积分
        ///// lz
        ///// 2016-10-18
        ///// </summary>
        //public override BaseResult Get(Hashtable param)
        //{
        //    BaseResult res = new BaseResult() { Success = true };
        //    try
        //    {
        //        var brJf = this.GetOne(param);
        //        if (!brJf.Success)
        //        {
        //            res.Success = false;
        //            res.Message.Add("会员积分数据不存在!");
        //        }
        //        else
        //        {
        //            var dbInfo = (Tz_Hy_Jf)brJf.Data;
        //            if (dbInfo == null || string.IsNullOrEmpty(dbInfo.id))
        //            {
        //                res.Success = true;
        //                res.Data = new Tz_Hy_Jf() { jf_qm = 0 };
        //                res.Message.Add("查询成功!");
        //            }
        //            else
        //            {
        //                if (RecordKeyError(dbInfo))
        //                {
        //                    res.Success = false;
        //                    res.Message.Add("会员积分数据非法!");
        //                }
        //                else if (ZBError(dbInfo))
        //                {
        //                    res.Success = false;
        //                    res.Message.Add("会员积分账本数据非法!");
        //                }
        //                else
        //                {
        //                    res.Data = dbInfo;
        //                    res.Message.Add("查询成功!");
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        res.Success = false;
        //        res.Message.Add("系统异常!");
        //    }
        //    return res;
        //}

        //#endregion 
        #endregion

        #region 注释
        //#region 加/减积分
        ///// <summary>
        ///// 加/减积分
        ///// lz
        ///// 2016-10-21
        ///// </summary>
        //[Transaction]
        //public override BaseResult Add(dynamic entity)
        //{
        //    #region 获取数据
        //    Hashtable param = (Hashtable)entity;
        //    BaseResult br = new BaseResult();
        //    Hashtable ht = new Hashtable();
        //    #endregion

        //    #region 验证参数
        //    if (!param.ContainsKey("Type") || string.IsNullOrEmpty(param["Type"].ToString()))
        //    {
        //        br.Success = false;
        //        br.Message.Add("业务数据异常!");
        //        return br;
        //    }

        //    if (decimal.Parse(param["jf"].ToString()) == 0)
        //    {
        //        br.Success = false;
        //        br.Message.Add("操作积分不能为0!");
        //        return br;
        //    }

        //    #endregion

        //    #region 检查会员是否有效
        //    br = base.CheckHY(param);
        //    if (!br.Success)
        //        throw new CySoftException(br);

        //    var tempModel = new Tz_Hy_Jf() { jf_qm = decimal.Parse(param["jf"].ToString()) };

        //    #endregion

        //    if (param["Type"].ToString() == "Add")
        //    {
        //        #region 加

        //        #region 更新/插入Tz_Hy_Jf表
        //        ht.Clear();
        //        ht.Add("id_shop", param["id_shop"].ToString());
        //        ht.Add("id_masteruser", param["id_masteruser"].ToString());
        //        ht.Add("id_hy", param["id_hy"].ToString());
        //        var brJe = this.GetOne(ht);
        //        if (!brJe.Success)
        //            throw new CySoftException(br);
        //        else
        //        {
        //            var dbJfModel = (Tz_Hy_Jf)brJe.Data;
        //            if (dbJfModel != null && !string.IsNullOrEmpty(dbJfModel.id))
        //            {
        //                #region 更新数据
        //                if (RecordKeyError(dbJfModel))
        //                {
        //                    br.Success = false;
        //                    br.Message.Clear();
        //                    br.Message.Add("会员积分数据非法!");
        //                    throw new CySoftException(br);
        //                }
        //                else if (ZBError(dbJfModel))
        //                {
        //                    br.Success = false;
        //                    br.Message.Clear();
        //                    br.Message.Add("会员积分账本数据非法!");
        //                    throw new CySoftException(br);
        //                }
        //                else
        //                {
        //                    dbJfModel.jf_qm = dbJfModel.jf_qm + decimal.Parse(param["jf"].ToString());
        //                    dbJfModel.recodekey = GetRecordKey(dbJfModel);
        //                    DAL.Update(dbJfModel);
        //                }
        //                #endregion
        //            }
        //            else
        //            {
        //                #region 插入数据
        //                Tz_Hy_Jf addModel = new Tz_Hy_Jf()
        //                {
        //                    id_masteruser = param["id_masteruser"].ToString(),
        //                    id = GetGuid,
        //                    id_shop = param["id_shop"].ToString(),
        //                    id_hy = param["id_hy"].ToString(),
        //                    jf_qm = decimal.Parse(param["jf"].ToString())
        //                };
        //                addModel.recodekey = GetRecordKey(addModel);
        //                DAL.Add(addModel);
        //                #endregion
        //            }
        //        }
        //        #endregion

        //        #region 插入流水表
        //        ht.Clear();
        //        ht.Add("id", GetGuid);
        //        ht.Add("id_masteruser", param["id_masteruser"].ToString());
        //        ht.Add("id_bill", param["id_bill"].ToString());
        //        ht.Add("bm_djlx", param["bm_djlx"].ToString());
        //        ht.Add("rq", DateTime.Parse(param["rq"].ToString()));
        //        ht.Add("id_shop", param["id_shop"].ToString());
        //        ht.Add("id_hy", param["id_hy"].ToString());
        //        ht.Add("jf", decimal.Parse(param["jf"].ToString()));
        //        ht.Add("bz", param["bz"].ToString());
        //        var addFlowNum = Tz_Hy_Jf_FlowDAL.AddWithExists(typeof(Tz_Hy_Jf_Flow), ht);
        //        if (addFlowNum == 0)
        //        {
        //            br.Success = false;
        //            br.Message.Clear();
        //            br.Message.Add("积分单据已存在!");
        //            throw new CySoftException(br);
        //        }
        //        #endregion

        //        #region 读取操作后积分 并验证
        //        ht.Clear();
        //        ht.Add("id_shop", param["id_shop"].ToString());
        //        ht.Add("id_masteruser", param["id_masteruser"].ToString());
        //        ht.Add("id_hy", param["id_hy"].ToString());
        //        var brJfNow = this.Get(ht);
        //        if (!brJfNow.Success)
        //        {
        //            br.Success = false;
        //            br.Message.Add(string.Format("操作失败 查询积分失败 !"));
        //            throw new CySoftException(br);
        //        }
        //        else
        //        {
        //            var dbJfModel = (Tz_Hy_Jf)brJfNow.Data;
        //            if (ZBError(dbJfModel))
        //            {
        //                br.Success = false;
        //                br.Message.Clear();
        //                br.Message.Add("操作失败 会员积分账本数据非法 !");
        //                throw new CySoftException(br);
        //            }
        //        }
        //        #endregion

        //        #region 返回
        //        var jfNowModel = (Tz_Hy_Jf)brJfNow.Data;
        //        br.Message.Add(String.Format("操作成功!"));
        //        br.Success = true;
        //        br.Data = new { add_jf = decimal.Parse(param["jf"].ToString()), jf_qm = jfNowModel.jf_qm };
        //        return br;
        //        #endregion 

        //        #endregion
        //    }
        //    else if (param["Type"].ToString() == "Del")
        //    {
        //        #region 减

        //        #region 更新Tz_Hy_Jf表
        //        ht.Clear();
        //        ht.Add("id_shop", param["id_shop"].ToString());
        //        ht.Add("id_masteruser", param["id_masteruser"].ToString());
        //        ht.Add("id_hy", param["id_hy"].ToString());
        //        var brJe = this.GetOne(ht);
        //        if (!brJe.Success)
        //            throw new CySoftException(br);
        //        else
        //        {
        //            var dbJfModel = (Tz_Hy_Jf)brJe.Data;
        //            if (dbJfModel != null && !string.IsNullOrEmpty(dbJfModel.id))
        //            {
        //                #region 更新数据
        //                #region 验证会员积分数据是否非法
        //                if (RecordKeyError(dbJfModel))
        //                {
        //                    br.Success = false;
        //                    br.Message.Clear();
        //                    br.Message.Add("会员积分数据非法!");
        //                    throw new CySoftException(br);
        //                }
        //                #endregion
        //                #region 验证会员积分账本数据是否非法

        //                if (ZBError(dbJfModel))
        //                {
        //                    br.Success = false;
        //                    br.Message.Clear();
        //                    br.Message.Add("会员积分账本数据非法!");
        //                    throw new CySoftException(br);
        //                }

        //                #endregion
        //                #region 插入流水表
        //                ht.Clear();
        //                ht.Add("id", GetGuid);
        //                ht.Add("id_masteruser", param["id_masteruser"].ToString());
        //                ht.Add("id_bill", param["id_bill"].ToString());
        //                ht.Add("bm_djlx", param["bm_djlx"].ToString());
        //                ht.Add("rq", DateTime.Parse(param["rq"].ToString()));
        //                ht.Add("id_shop", param["id_shop"].ToString());
        //                ht.Add("id_hy", param["id_hy"].ToString());
        //                ht.Add("jf", decimal.Parse(param["jf"].ToString()));
        //                ht.Add("bz", param["bz"].ToString());
        //                var addFlowNum = Tz_Hy_Jf_FlowDAL.AddWithExists(typeof(Tz_Hy_Jf_Flow), ht);
        //                if (addFlowNum == 0)
        //                {
        //                    br.Success = false;
        //                    br.Message.Clear();
        //                    br.Message.Add("积分单据已存在!");
        //                    throw new CySoftException(br);
        //                }
        //                #endregion
        //                #region 验证账户积分是否充足
        //                if (dbJfModel.jf_qm + tempModel.jf_qm < 0)
        //                {
        //                    br.Success = false;
        //                    br.Message.Clear();
        //                    br.Message.Add(String.Format("操作失败，账户积分(" + dbJfModel.jf_qm + ") 不足！"));
        //                    throw new CySoftException(br);
        //                }
        //                #endregion
        //                #region 更新积分数据
        //                dbJfModel.jf_qm = dbJfModel.jf_qm + decimal.Parse(param["jf"].ToString());
        //                dbJfModel.recodekey = GetRecordKey(dbJfModel);
        //                DAL.Update(dbJfModel);
        //                #endregion
        //                #endregion
        //            }
        //            else
        //            {
        //                #region 操作失败 会员会员积分不足
        //                br.Success = false;
        //                br.Message.Clear();
        //                br.Message.Add(string.Format("操作失败 会员积分为0 不允许消费积分!"));
        //                throw new CySoftException(br);
        //                #endregion
        //            }
        //        }
        //        #endregion

        //        #region 读取操作后积分 并验证
        //        ht.Clear();
        //        ht.Add("id_shop", param["id_shop"].ToString());
        //        ht.Add("id_masteruser", param["id_masteruser"].ToString());
        //        ht.Add("id_hy", param["id_hy"].ToString());
        //        var brJfNow = this.Get(ht);
        //        if (!brJfNow.Success)
        //        {
        //            br.Success = false;
        //            br.Message.Clear();
        //            br.Message.Add(string.Format("操作失败 查询积分失败 !"));
        //            throw new CySoftException(br);
        //        }
        //        else
        //        {
        //            var dbJfModel = (Tz_Hy_Jf)brJfNow.Data;
        //            if (ZBError(dbJfModel))
        //            {
        //                br.Success = false;
        //                br.Message.Clear();
        //                br.Message.Add("操作失败 会员积分账本数据非法 !");
        //                throw new CySoftException(br);
        //            }
        //        }
        //        #endregion

        //        #region 返回
        //        var jfNowModel = (Tz_Hy_Jf)brJfNow.Data;
        //        br.Message.Add(String.Format("操作成功!"));
        //        br.Success = true;
        //        br.Data = new { add_jf = decimal.Parse(param["jf"].ToString()), jf_qm = jfNowModel.jf_qm };
        //        return br;
        //        #endregion  

        //        #endregion
        //    }
        //    else
        //    {
        //        #region 返回
        //        br.Message.Add(String.Format("操作失败,无此操作类型!"));
        //        br.Success = false;
        //        return br;
        //        #endregion 
        //    }
        //}
        //#endregion 
        #endregion

        #region GetOne
        /// <summary>
        /// GetOne
        /// lz
        /// 2016-10-20
        /// </summary>
        public BaseResult GetOne(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            try
            {
                var dbInfo = DAL.GetItem<Tz_Hy_Jf>(typeof(Tz_Hy_Jf), param);
                if (dbInfo == null || string.IsNullOrEmpty(dbInfo.id))
                {
                    res.Data = new Tz_Hy_Jf();
                    res.Message.Add("会员积分数据不存在!");
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

        #region GetList
        /// <summary>
        /// GetList
        /// lz
        /// 2016-12-22
        /// </summary>
        public BaseResult GetList(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            try
            {
                var dbList = DAL.QueryList<Tz_Hy_Jf>(typeof(Tz_Hy_Jf), param);
                if (dbList == null || dbList.Count() <= 0)
                {
                    res.Data = new List<Tz_Hy_Jf>();
                    res.Message.Add("会员积分数据不存在!");
                }
                else
                {
                    res.Data = dbList;
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
        public bool ZBError(Tz_Hy_Jf model)
        {
            try
            {
                bool result = true;
                if (model != null && !string.IsNullOrEmpty(model.id_masteruser) && !string.IsNullOrEmpty(model.id_hy))
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("id_masteruser", model.id_masteruser);
                    ht.Add("id_hy", model.id_hy);
                    var flowList = DAL.QueryList<Tz_Hy_Jf_Flow>(typeof(Tz_Hy_Jf_Flow), ht);
                    if (flowList != null && flowList.Count() > 0)
                    {
                        var flowJf = flowList.Sum(d => d.jf);
                        if (model.jf_qm == flowJf)
                            return false;
                    }
                    else
                    {
                        if (model.jf_qm == 0)
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
        /// 2016-09-20
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool RecordKeyError(Tz_Hy_Jf model)
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
        /// 2016-09-20
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetRecordKey(Tz_Hy_Jf model)
        {
            try
            {
                string RecordKey = "";
                if (model != null)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(model.id_masteruser);
                    strSql.Append(model.id);
                    strSql.Append(model.id_shop);
                    strSql.Append(model.id_hy);
                    strSql.Append(double.Parse(model.jf_qm.ToString()).ToString("0.0000000"));
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

    }
}
