using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Model.Td;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.BLL.BusinessBLL
{
    public class Td_Sk_1BLL : BaseBLL
    {
        #region GetPage
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Td_Sk_1), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Td_Sk_1_QueryModel>(typeof(Td_Sk_1), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }
        #endregion

        #region Get
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            try
            {
                Td_Sk_1_Query_DetailModel result = new Td_Sk_1_Query_DetailModel();
                var head = DAL.GetItem<Td_Sk_1_QueryModel>(typeof(Td_Sk_1), param);
                if (head != null && !string.IsNullOrEmpty(head.id))
                {
                    result.head = head;
                    param.Clear();
                    param.Add("id_bill", head.id);
                    var body = DAL.QueryList<Td_Sk_2_QueryModel>(typeof(Td_Sk_2), param);
                    result.body = body.ToList();
                    res.Data = result;
                }
                else
                {
                    res.Success = false;
                    res.Message.Add("未找到此收款单信息!");
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

        #region Add
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            #region 获取数据
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            var td_Sk_2_List = (List<Td_Sk_2>)param["skList"];
            var digit = param["DigitHashtable"] as System.Collections.Hashtable;//小数点控制 

            var td_Sk_1_Model = this.TurnTd_Sk_1Model(param);
            int xh = 1;

            foreach (var item in td_Sk_2_List)
            {
                item.id = Guid.NewGuid().ToString();
                item.id_bill = td_Sk_1_Model.id;
                item.id_masteruser = td_Sk_1_Model.id_masteruser;
                item.sort_id = xh;
                item.rq_create = td_Sk_1_Model.rq_create;
                xh++;
            }

            td_Sk_1_Model.je_sk_mxtotal = td_Sk_2_List.Sum(d => d.je_sk);//明细总金额
            td_Sk_1_Model.je_yh_mxtotal = td_Sk_2_List.Sum(d => d.je_yh);//明细优惠总金额
            td_Sk_1_Model.je_total = td_Sk_1_Model.je_sk_mxtotal + td_Sk_1_Model.je_yh_mxtotal;//收款金额

            #endregion

            #region 检查单号是否重复

            ht.Clear();
            ht.Add("id_masteruser", td_Sk_1_Model.id_masteruser);
            ht.Add("dh", td_Sk_1_Model.dh);
            if (DAL.GetCount(typeof(Td_Ps_Ck_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在配送出库单中,请核实!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", td_Sk_1_Model.id_masteruser);
            ht.Add("dh", td_Sk_1_Model.dh);
            if (DAL.GetCount(typeof(Td_Ps_Fprk_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在返配入库单中,请核实!");
                return br;
            }


            #endregion

            #region 插入数据库
            DAL.Add(td_Sk_1_Model);
            DAL.AddRange<Td_Sk_2>(td_Sk_2_List);
            #endregion

            #region 审核
            //审核
            if (param["autoAudit"].ToString().ToLower() == "true")
            {
                ht.Clear();
                ht.Add("id", td_Sk_1_Model.id);
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id_user", param["id_user"].ToString());
                var brSh = this.ActiveWork(ht);
                if (!brSh.Success)
                {
                    br.Message.Clear();
                    br.Message.Add("审核操作失败,请重试！");
                    br.Success = false;
                    throw new CySoftException(br);
                }
            }
            #endregion

            br.Message.Add(String.Format("操作成功!"));
            br.Success = true;
            br.Data = td_Sk_1_Model.id;
            return br;
        }
        #endregion

        #region Active
        /// <summary>
        /// Active
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Active(Hashtable param)
        {
            return this.ActiveWork(param);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseResult ActiveWork(Hashtable param)
        {
            #region 参数验证
            BaseResult result = new BaseResult() { Success = true };
            if (param == null || param.Count < 3)
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            var id = param["id"].ToString();
            var id_masteruser = param["id_masteruser"].ToString();
            var id_user = param["id_user"].ToString();
            if (string.IsNullOrEmpty(id))
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            if (string.IsNullOrEmpty(id_masteruser) || string.IsNullOrEmpty(id_user))
            {
                result.Success = false;
                result.Message.Add("请登录!");
                return result;
            }
            #endregion
            #region 更新数据
            Hashtable ht = new Hashtable();
            ht.Add("id", id);
            var brSk = this.Get(ht);
            if (!brSk.Success)
            {
                return brSk;
            }
            else
            {
                #region 校验商品合法性
                Td_Sk_1_Query_DetailModel dbModel = (Td_Sk_1_Query_DetailModel)brSk.Data;
                if (dbModel == null || dbModel.head == null || string.IsNullOrEmpty(dbModel.head.id) || (dbModel.head.flag_sklx == (byte)Enums.FlagSKLX.YingShou && (dbModel.body == null || dbModel.body.Count() <= 0)))
                {
                    result.Success = false;
                    result.Message.Add("获取收款单信息不符合要求!");
                    return result;
                }
                #endregion
                #region 执行存储过程并返回结果
                ht.Clear();
                ht.Add("proname", "p_sk_sh");
                ht.Add("errorid", "-1");
                ht.Add("errormessage", "未知错误！");
                ht.Add("id_bill", dbModel.head.id);
                ht.Add("id_user", id_user);
                DAL.RunProcedure(ht);

                if (!ht.ContainsKey("errorid") || !ht.ContainsKey("errormessage"))
                {
                    result.Success = false;
                    result.Message.Add("执行审核出现异常!");
                    throw new CySoftException(result);
                }

                if (!string.IsNullOrEmpty(ht["errorid"].ToString()) || !string.IsNullOrEmpty(ht["errormessage"].ToString()))
                {
                    result.Success = false;
                    result.Message.Add(ht["errormessage"].ToString());
                    throw new CySoftException(result);
                }

                result.Success = true;
                result.Message.Add("审核成功!");
                return result;
                #endregion

            }
            #endregion
        }
        #endregion

        #region Delete
        /// <summary>
        ///Delete
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult Delete(Hashtable param)
        {
            #region 参数验证
            BaseResult result = new BaseResult() { Success = true };
            if (param == null || param.Count < 2)
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            var id = param["id"].ToString();
            var id_masteruser = param["id_masteruser"].ToString();
            if (string.IsNullOrEmpty(id))
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            if (string.IsNullOrEmpty(id_masteruser))
            {
                result.Success = false;
                result.Message.Add("请登录!");
                return result;
            }
            #endregion
            #region 更新数据
            Hashtable ht = new Hashtable();

            ht.Add("id", id);
            var brSk = this.Get(ht);
            if (!brSk.Success)
            {
                return brSk;
            }
            else
            {
                Td_Sk_1_Query_DetailModel dbModel = (Td_Sk_1_Query_DetailModel)brSk.Data;
                if (dbModel == null || dbModel.head == null || string.IsNullOrEmpty(dbModel.head.id))
                {
                    result.Success = false;
                    result.Message.Add("获取收款单信息不符合要求!");
                    return result;
                }

                if (dbModel.head.flag_sh == (byte)Enums.FlagSh.HadSh)
                {
                    result.Success = false;
                    result.Message.Add("该单据已经审核,不允许删除!");
                    return result;
                }

                ht.Clear();
                ht.Add("id", id);
                ht.Add("id_masteruser", id_masteruser);
                ht.Add("flag_sh", (byte)Enums.FlagSh.UnSh);
                ht.Add("new_flag_delete", (int)Enums.FlagCancel.Canceled);
                try
                {
                    if (DAL.UpdatePart(typeof(Td_Sk_1), ht) <= 0)
                    {
                        result.Success = false;
                        result.Message.Add("删除操作失败!");
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Message.Add("删除操作异常!");
                }

                return result;
            }
            #endregion
        }
        #endregion

        #region TurnTd_Sk_1Model
        /// <summary>
        /// 将Hashtable转换为Model
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private Td_Sk_1 TurnTd_Sk_1Model(Hashtable param)
        {
            Td_Sk_1 model = new Td_Sk_1();
            model.id_masteruser = param["id_masteruser"].ToString();
            model.id = Guid.NewGuid().ToString();
            model.dh = param["dh"].ToString();
            model.rq = DateTime.Parse(param["rq"].ToString());
            model.id_shop = param["id_shop"].ToString();
            model.bm_djlx = Enums.Ps.CW002.ToString();
            model.id_kh = param["id_kh"].ToString();
            model.id_jbr = param["id_jbr"].ToString();
            model.je_sk_mxtotal = 0m;//明细总金额
            model.je_yh_mxtotal = 0m;//明细优惠总金额
            model.je_total = 0m;//总金额
            model.flag_sh = (byte)Enums.FlagSh.UnSh;
            model.flag_cancel = (byte)Enums.FlagCancel.NoCancel;
            model.bz = param["remark"].ToString();
            model.id_create = param["id_user"].ToString();
            model.rq_create = DateTime.Now;
            model.flag_delete = (byte)Enums.FlagDelete.NoDelete;

            model.flag_sklx = byte.Parse(param["flag_sklx"].ToString());
            model.je_pre = decimal.Parse(param["je_pre"].ToString());//预收款

            //if (model.flag_sklx == (byte)Enums.FlagSKLX.YingShou)
            //    model.je_pre = (-1) * model.je_pre;

            model.id_bill_origin = "";
            model.dh_origin = "";
            model.bm_djlx_origin = "";

            return model;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            #region 获取数据
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            var td_Sk_2_List = (List<Td_Sk_2>)param["skList"];

            var digit = param["DigitHashtable"] as System.Collections.Hashtable;//小数点控制 

            if (!param.ContainsKey("id") || string.IsNullOrEmpty(param["id"].ToString()))
            {
                br.Message.Add(String.Format("操作失败 缺少必要参数!"));
                br.Success = false;
                return br;
            }

            #region 检测此单号是否已经存在
            ht.Clear();
            ht.Add("id", param["id"].ToString());

            if (DAL.GetCount(typeof(Td_Sk_1), ht) <= 0)
            {
                br.Success = false;
                br.Message.Add("未查询到此id订单,请重试!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("dh", param["dh"].ToString());
            ht.Add("not_id", param["id"].ToString());
            if (DAL.GetCount(typeof(Td_Sk_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("新单号已经重复,请核实!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("dh", param["dh"].ToString());
            if (DAL.GetCount(typeof(Td_Ps_Ck_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在配送出库单中,请核实!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("dh", param["dh"].ToString());
            if (DAL.GetCount(typeof(Td_Ps_Fprk_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在返配入库单中,请核实!");
                return br;
            }

            #endregion


            int xh = 1;

            foreach (var item in td_Sk_2_List)
            {
                item.id = Guid.NewGuid().ToString();
                item.id_bill = param["id"].ToString();
                item.id_masteruser = param["id_masteruser"].ToString();
                item.sort_id = xh;
                item.rq_create = DateTime.Parse(param["rq"].ToString());
                xh++;
            }


            #endregion
            #region 操作数据库
            ht.Clear();
            ht.Add("id", param["id"].ToString());
            ht.Add("new_dh", param["dh"].ToString());
            ht.Add("new_rq", DateTime.Parse(param["rq"].ToString()));
            ht.Add("new_id_shop", param["id_shop"].ToString());
            ht.Add("new_id_kh", param["id_kh"].ToString());//id_kh
            ht.Add("new_id_jbr", param["id_jbr"].ToString());
            ht.Add("new_bz", param["remark"].ToString());
            ht.Add("new_je_sk_mxtotal", td_Sk_2_List.Sum(d => d.je_sk));//明细总金额
            ht.Add("new_je_yh_mxtotal", td_Sk_2_List.Sum(d => d.je_yh));//明细优惠总金额
            ht.Add("new_je_total", (td_Sk_2_List.Sum(d => d.je_sk) + td_Sk_2_List.Sum(d => d.je_yh)));//收款金额

            ht.Add("new_flag_sklx", byte.Parse(param["flag_sklx"].ToString()));//收款类型
            ht.Add("new_id_edit", param["id_user"].ToString());
            ht.Add("new_rq_edit", DateTime.Now);


            ht.Add("new_je_pre", decimal.Parse(param["je_pre"].ToString()));
           

            DAL.UpdatePart(typeof(Td_Sk_1), ht);

            ht.Clear();
            ht.Add("id_bill", param["id"].ToString());
            DAL.Delete(typeof(Td_Sk_2), ht);
            DAL.AddRange<Td_Sk_2>(td_Sk_2_List);
            #endregion
            br.Message.Add(String.Format("操作成功!"));
            br.Success = true;
            br.Data = param["id"].ToString();
            return br;
        }
        #endregion




    }
}
