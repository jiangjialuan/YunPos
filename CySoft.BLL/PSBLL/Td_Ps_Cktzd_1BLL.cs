﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Model.Td;
using CySoft.Utility;

namespace CySoft.BLL.PSBLL
{
    public class Td_Ps_Cktzd_1BLL : BaseBLL
    {

        #region 获取分页数据
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Td_Ps_Cktzd_1), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Td_Ps_Cktzd_1_Query>(typeof(Td_Ps_Cktzd_1), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }
        #endregion

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult res = new BaseResult() { Success = true };
            var model = entity as PscktzdModel;
            #region 参数验证
            if (model == null || string.IsNullOrEmpty(model.id_masteruser) || string.IsNullOrEmpty(model.id_shop))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (string.IsNullOrEmpty(model.dh))
            {
                res.Success = false;
                res.Message.Add("请填写单号!");
                return res;
            }
            model.dh = model.dh.Trim();
            Hashtable _ck_parm = new Hashtable();
            _ck_parm.Add("dh", model.dh);
            _ck_parm.Add("id_masteruser", model.id_masteruser);
            if (DAL.GetCount(typeof(Td_Ps_Cktzd_1), _ck_parm) > 0)
            {
                res.Success = false;
                res.Message.Add("单号已存在!");
                return res;
            }
            if (string.IsNullOrEmpty(model.id_shop_ck))
            {
                res.Success = false;
                res.Message.Add("请选择出库门店!");
                return res;
            }
            if (string.IsNullOrEmpty(model.id_jbr))
            {
                res.Success = false;
                res.Message.Add("请选择经办人!");
                return res;
            }
            if (model.rq < new DateTime(2000, 01, 01))
            {
                res.Success = false;
                res.Message.Add("请输入开单日期!");
                return res;
            }
            if (string.IsNullOrEmpty(model.id_shop_rk))
            {
                res.Success = false;
                res.Message.Add("请选择入库门店!");
                return res;
            } 
            #endregion
            Td_Ps_Cktzd_1 pscktzd1 = new Td_Ps_Cktzd_1()
            {
                #region 赋值
                id_masteruser = model.id_masteruser,
                id = GetGuid,
                dh = model.dh.Trim(),
                rq = model.rq,
                id_shop = model.id_shop,
                bm_djlx = Enums.Ps.PS110.ToString(),
                bm_djlx_origin = model.bm_djlx_origin,
                id_bill_origin = model.id_bill_origin,
                dh_origin = model.dh_origin,
                id_shop_rk = model.id_shop_rk,
                id_shop_ck = model.id_shop_ck,
                id_jbr = model.id_jbr,
                je_mxtotal = model.je_mxtotal,
                flag_sh = (byte)Enums.FlagSh.UnSh,
                flag_cancel = (byte)Enums.FlagCancel.NoCancel,
                bz = string.Format("{0}", model.remark).Trim(),
                id_create = model.id_create,
                rq_create = DateTime.Now 
                #endregion
            };
            model.id = pscktzd1.id;
            //pscktzd1.bz = pscktzd1.bz.Trim();
            List<Td_Ps_Cktzd_2> pscktzd2S = new List<Td_Ps_Cktzd_2>();
            if (!string.IsNullOrEmpty(model.json_data))
            {
                pscktzd2S = JSON.Deserialize<List<Td_Ps_Cktzd_2>>(model.json_data);
                if (pscktzd2S.Any())
                {
                    int sort = 1;
                    pscktzd2S.ForEach(a =>
                    {
                        a.id = GetGuid;
                        a.id_masteruser = pscktzd1.id_masteruser;
                        a.id_bill = pscktzd1.id;
                        a.rq_create = DateTime.Now;
                        a.sort_id = sort++;
                        CheckModel2(res, a);
                    });
                }
            }
            if (!pscktzd2S.Any())
            {
                res.Success = false;
                res.Message.Add("请选择商品!");
                return res;
            }
            if (res.Success == false)
            {
                var first = res.Message.First();
                res.Message.Clear();
                res.Message.Add(first);
                return res;
            }
            Hashtable ht = new Hashtable();
            var id_sps = (from p in pscktzd2S select p.id_sp).ToArray();
            var id_shops = new string[] { pscktzd1.id_shop_rk, pscktzd1.id_shop_ck };
            ht.Add("id_shop_array", id_shops);
            ht.Add("id_sp_array", id_sps);
            ht.Add("id_masteruser", model.id_masteruser);
            var splist = DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), ht).ToList();
            pscktzd2S.ForEach(p =>
            {
                var sps = splist.Where(s => s.id_sp == p.id_sp);
                if (sps.Any() && sps.Count() >= 2)
                {
                    if (sps.Any(a => a.flag_delete != (byte)Enums.FlagDelete.NoDelete || a.flag_state != 1))
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("第{0}行商品状态有变更,不能配货!", p.sort_id));
                    }
                }
                else
                {
                    res.Success = false;
                    res.Message.Add(string.Format("第{0}行商品状态有变更,不能配货!", p.sort_id));
                }
            });
            if (res.Success == false)
            {
                var first = res.Message.First();
                res.Message.Clear();
                res.Message.Add(first);
                return res;
            }
            DAL.Add(pscktzd1);
            DAL.AddRange(pscktzd2S);
            if (model.AutoAudit)
            {
                Hashtable param = new Hashtable();
                param.Add("id_masteruser", model.id_masteruser);
                param.Add("id_user", model.id_create);
                param.Add("id", model.id);
                Sh<Td_Ps_Cktzd_1>(res, param, "p_ps_cktzd_sh");
            }
            return res;
        }

        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            var pscktzd1 = DAL.GetItem<Td_Ps_Cktzd_1>(typeof(Td_Ps_Cktzd_1), param);
            PscktzdQueryModel model = new PscktzdQueryModel();
            if (pscktzd1 != null)
            {
                param.Clear();
                param.Add("id_bill", pscktzd1.id);
                param.Add("sort", "  sort_id  ");
                model.Pscktzd2List = DAL.QueryList<Td_Ps_Cktzd_2_Query>(typeof(Td_Ps_Cktzd_2), param).ToList();
                if (!string.IsNullOrEmpty(pscktzd1.id_create))
                {
                    param.Clear();
                    List<string> arr = new List<string>();
                    arr.Add(pscktzd1.id_create);
                    if (!string.IsNullOrEmpty(pscktzd1.id_user_sh))
                    {
                        arr.Add(pscktzd1.id_user_sh);
                    }
                    param.Add("id_users", arr.ToArray());
                    var users = DAL.QueryList<Tb_User>(typeof(Tb_User), param).ToList();
                    if (users.Any())
                    {
                        var user_zdr = users.FirstOrDefault(a => a.id == pscktzd1.id_create);
                        if (user_zdr != null) model.zdr = user_zdr.username;
                        if (!string.IsNullOrEmpty(pscktzd1.id_user_sh))
                        {
                            var user_shr = users.FirstOrDefault(a => a.id == pscktzd1.id_user_sh);
                            if (user_shr != null) model.shr = user_shr.username;
                        }
                    }
                }
            }
            model.Pscktzd1 = pscktzd1;
            res.Data = model;
            return res;
        }
        private bool CheckModel2(BaseResult res, Td_Ps_Cktzd_2 model)
        {
            if (model == null)
            {
                res.Success = false;
                res.Message.Add("数据为空!");
                return false;
            }
            if (model.sl <= 0)
            {
                res.Success = false;
                res.Message.Add(string.Format("请填写第{0}行商品的数量", model.sort_id));
                return false;
            }
            if (model.dj <= 0)
            {
                //res.Success = false;
                res.Level=ErrorLevel.Warning;
                res.Message.Add(string.Format("请填写第{0}行商品的单价", model.sort_id));
                //return false;
            }
            if (model.zhl <= 0)
            {
                res.Success = false;
                res.Message.Add(string.Format("第{0}行商品数据异常!", model.sort_id));
                return false;
            }
            if (model.sl_total <= 0)
            {
                res.Success = false;
                res.Message.Add(string.Format("第{0}行商品数据异常!", model.sort_id));
                return false;
            }
            //if (model.je <= 0)
            //{
            //    res.Success = false;
            //    res.Message.Add(string.Format("第{0}行商品数据异常!", model.sort_id));
            //    return false;
            //}
            return true;
        }

        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult res = new BaseResult() { Success = true };
            var model = entity as PscktzdModel;
            #region 参数验证
            if (model == null
                    || string.IsNullOrEmpty(model.id_masteruser)
                    || string.IsNullOrEmpty(model.id_shop)
                    || string.IsNullOrEmpty(model.id))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (string.IsNullOrEmpty(model.dh))
            {
                res.Success = false;
                res.Message.Add("请填写单号!");
                return res;
            }
            model.dh = model.dh.Trim();
            if (string.IsNullOrEmpty(model.id_shop_ck))
            {
                res.Success = false;
                res.Message.Add("请选择出库门店!");
                return res;
            }
            if (string.IsNullOrEmpty(model.id_jbr))
            {
                res.Success = false;
                res.Message.Add("请选择经办人!");
                return res;
            }
            if (model.rq < new DateTime(2000, 01, 01))
            {
                res.Success = false;
                res.Message.Add("请输入开单日期!");
                return res;
            }
            if (string.IsNullOrEmpty(model.id_shop_rk))
            {
                res.Success = false;
                res.Message.Add("请选择入库门店!");
                return res;
            } 
            #endregion
            Hashtable param = new Hashtable();
            #region 赋值
            param.Add("new_bm_djlx_origin", model.bm_djlx_origin ?? "");
            param.Add("new_id_bill_origin", model.id_bill_origin ?? "");
            param.Add("new_dh_origin", model.dh_origin ?? "");
            param.Add("new_id_shop_ck", model.id_shop_ck);
            param.Add("new_id_shop_rk", model.id_shop_rk);
            param.Add("new_id_shop", model.id_shop);
            param.Add("new_id_jbr", model.id_jbr);
            param.Add("new_je_mxtotal", model.je_mxtotal);

            param.Add("new_rq", model.rq);
            param.Add("new_remark",string.Format("{0}", model.remark).Trim());
            param.Add("id", model.id);
            param.Add("id_masteruser", model.id_masteruser);
            #endregion
            List<Td_Ps_Cktzd_2> pscktzd2S = new List<Td_Ps_Cktzd_2>();
            if (!string.IsNullOrEmpty(model.json_data))
            {
                pscktzd2S = JSON.Deserialize<List<Td_Ps_Cktzd_2>>(model.json_data);
                if (pscktzd2S.Any())
                {
                    int sort = 1;
                    pscktzd2S.ForEach(a =>
                    {
                        a.id = GetGuid;
                        a.id_masteruser = model.id_masteruser;
                        a.id_bill = model.id;
                        a.rq_create = DateTime.Now;
                        a.sort_id = sort++;
                    });
                }
            }
            if (!pscktzd2S.Any())
            {
                res.Success = false;
                res.Message.Add("请选择商品!");
                return res;
            }
            if (res.Success == false)
            {
                var first = res.Message.First();
                res.Message.Clear();
                res.Message.Add(first);
                return res;
            }
            Hashtable ht = new Hashtable();
            var id_sps = (from p in pscktzd2S select p.id_sp).ToArray();
            var id_shops = new string[] { model.id_shop_rk, model.id_shop_ck };
            ht.Add("id_shop_array", id_shops);
            ht.Add("id_sp_array", id_sps);
            ht.Add("id_masteruser", model.id_masteruser);
            var splist = DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), ht).ToList();
            pscktzd2S.ForEach(p =>
            {
                var sps = splist.Where(s => s.id_sp == p.id_sp);
                if (sps.Any() && sps.Count() >= 2)
                {
                    if (sps.Any(a => a.flag_delete != (byte)Enums.FlagDelete.NoDelete || a.flag_state != 1))
                    {
                        res.Success = false;
                        res.Message.Add(string.Format("第{0}行商品状态有变更,不能配货!", p.sort_id));
                    }
                }
                else
                {
                    res.Success = false;
                    res.Message.Add(string.Format("第{0}行商品状态有变更,不能配货!", p.sort_id));
                }
            });
            if (res.Success == false)
            {
                var first = res.Message.First();
                res.Message.Clear();
                res.Message.Add(first);
                return res;
            }
            DAL.UpdatePart(typeof(Td_Ps_Cktzd_1), param);
            param.Clear();
            param.Add("id_bill", model.id);
            param.Add("id_masteruser", model.id_masteruser);
            DAL.Delete(typeof(Td_Ps_Cktzd_2), param);
            DAL.AddRange(pscktzd2S);
            return res;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Active(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            Sh<Td_Ps_Cktzd_1>(res, param, "p_ps_cktzd_sh");
            return res;
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Stop(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            Sh<Td_Ps_Cktzd_1>(res, param, "p_ps_cktzd_zf");
            return res;
        }
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            if (param == null
                || !param.Contains("id")
                || !param.Contains("id_masteruser")
                || !param.Contains("id_user"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            var id = string.Format("{0}", param["id"]);
            var id_masteruser = string.Format("{0}", param["id_masteruser"]);
            var id_user = string.Format("{0}", param["id_user"]);
            param.Clear();
            param.Add("id_masteruser", id_masteruser);
            param.Add("id", id);
            var model = DAL.GetItem<Td_Ps_Cktzd_1>(typeof(Td_Ps_Cktzd_1), param);
            if (model == null)
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (model.flag_delete != null && model.flag_delete.Value == (byte)Enums.FlagDelete.Deleted)
            {
                res.Success = false;
                res.Message.Add("此数据已删除!");
                return res;
            }
            if (model.flag_sh != null && model.flag_sh.Value == (byte)Enums.FlagSh.HadSh)
            {
                res.Success = false;
                res.Message.Add("此数据已审核，不能删除!");
                return res;
            }
            param.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);
            param.Add("new_id_user_sh", id_user);
            param.Add("flag_sh", (int)Enums.FlagSh.UnSh);
            if (DAL.UpdatePart(typeof(Td_Ps_Cktzd_1), param) <= 0)
            {
                res.Success = false;
                res.Message.Add("此数据已被处理!");
                return res;
            }
            return res;
        }
    }
}
