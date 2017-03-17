using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Model.Td;
using CySoft.Model.Tz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Utility;

//进货付款
namespace CySoft.BLL.BusinessBLL
{

    public class Td_Fk_1BLL : BaseBLL
    {

        #region 获取分页数据
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Td_Fk_1), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Td_Fk_1_QueryModel>(typeof(Td_Fk_1), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
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
            var td_Fk_2_List = (List<Td_Fk_2>)param["jhList"];
            var digit = param["DigitHashtable"] as System.Collections.Hashtable;//小数点控制 

            var td_Fk_1_Model = this.TurnTd_Fk_1Model(param);
            int xh = 1;

            foreach (var item in td_Fk_2_List)
            {
                item.id = Guid.NewGuid().ToString();
                item.id_bill = td_Fk_1_Model.id;
                item.id_masteruser = td_Fk_1_Model.id_masteruser;
                item.dh = td_Fk_1_Model.dh;
                item.xh = item.sort_id = xh;
                item.rq_create = td_Fk_1_Model.rq_create;
                xh++;
            }

            td_Fk_1_Model.je_fk_mxtotal = td_Fk_2_List.Sum(d => d.je_fk);//明细总金额
            td_Fk_1_Model.je_yh_mxtotal = td_Fk_2_List.Sum(d => d.je_yh);//明细优惠总金额
            td_Fk_1_Model.je_total = td_Fk_1_Model.je_fk_mxtotal + td_Fk_1_Model.je_yh_mxtotal;//付款金额

            #endregion

            #region 检查单号是否重复

            ht.Clear();
            ht.Add("id_masteruser", td_Fk_1_Model.id_masteruser);
            ht.Add("dh", td_Fk_1_Model.dh);
            if (DAL.GetCount(typeof(Td_Jh_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在进货单中,请核实!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", td_Fk_1_Model.id_masteruser);
            ht.Add("dh", td_Fk_1_Model.dh);
            if (DAL.GetCount(typeof(Td_Fk_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在付款单中,请核实!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", td_Fk_1_Model.id_masteruser);
            ht.Add("dh", td_Fk_1_Model.dh);
            if (DAL.GetCount(typeof(Td_Jh_Th_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在退货单中,请核实!");
                return br;
            }

            #endregion

            #region 插入数据库
            DAL.Add(td_Fk_1_Model);
            DAL.AddRange<Td_Fk_2>(td_Fk_2_List);
            #endregion

            #region 审核
            //审核
            if (param["autoAudit"].ToString().ToLower() == "true")
            {
                ht.Clear();
                ht.Add("id", td_Fk_1_Model.id);
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

            //br.Message.Add(String.Format("新增进货。流水号：{0}，单号:{1}", td_Fk_1_Model.id, td_Fk_1_Model.dh));
            br.Message.Add(String.Format("操作成功!"));
            br.Success = true;
            br.Data = td_Fk_1_Model.id;
            return br;
        }
        #endregion

        #region 获取单条
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            try
            {
                Td_Fk_1_Query_DetailModel result = new Td_Fk_1_Query_DetailModel();
                var head = DAL.GetItem<Td_Fk_1_QueryModel>(typeof(Td_Fk_1), param);
                if (head != null && !string.IsNullOrEmpty(head.id))
                {
                    result.head = head;
                    param.Clear();
                    param.Add("id_bill", head.id);
                    var body = DAL.QueryList<Td_Fk_2_QueryModel>(typeof(Td_Fk_2), param);
                    result.body = body.ToList();
                    res.Data = result;
                }
                else
                {
                    res.Success = false;
                    res.Message.Add("未找到此付款单信息!");
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

        #region 作废
        /// <summary>
        ///作废
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult Stop(Hashtable param)
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
            string id_user= param["id_user"].ToString();
            if (string.IsNullOrEmpty(id))
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            if (string.IsNullOrEmpty(id_masteruser)||string.IsNullOrEmpty(id_user))
            {
                result.Success = false;
                result.Message.Add("请登录!");
                return result;
            }
            #endregion
            #region 更新数据

            Hashtable ht = new Hashtable();
            ht.Add("proname", "p_fk_zf");
            ht.Add("errorid", "-1");
            ht.Add("errormessage", "未知错误！");
            ht.Add("id_bill", id);
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
            result.Message.Add("操作成功!");
            return result;


            //Hashtable ht = new Hashtable();
            //ht.Add("id", id);
            //ht.Add("id_masteruser", id_masteruser);
            //ht.Add("new_flag_cancel", (int)Enums.FlagCancel.Canceled);
            //try
            //{
            //    if (DAL.UpdatePart(typeof(Td_Fk_1), ht) <= 0)
            //    {
            //        result.Success = false;
            //        result.Message.Add("作废操作失败!");
            //        return result;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    result.Success = false;
            //    result.Message.Add("作废操作异常!");
            //}
            #endregion
            //return result;
        }
        #endregion

        #region 删除
        /// <summary>
        ///删除
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
            var brJh = this.Get(ht);
            if (!brJh.Success)
            {
                return brJh;
            }
            else
            {
                Td_Fk_1_Query_DetailModel dbModel = (Td_Fk_1_Query_DetailModel)brJh.Data;
                if (dbModel == null || dbModel.head == null || string.IsNullOrEmpty(dbModel.head.id) || dbModel.body == null || dbModel.body.Count() <= 0)
                {
                    result.Success = false;
                    result.Message.Add("获取付款单信息不符合要求!");
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
                    if (DAL.UpdatePart(typeof(Td_Fk_1), ht) <= 0)
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
                #endregion
                return result;
            }
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
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
            var td_Fk_2_List = (List<Td_Fk_2>)param["jhList"];

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

            if (DAL.GetCount(typeof(Td_Fk_1), ht) <= 0)
            {
                br.Success = false;
                br.Message.Add("未查询到此id订单,请重试!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("dh", param["dh"].ToString());
            ht.Add("not_id", param["id"].ToString());
            if (DAL.GetCount(typeof(Td_Fk_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("新单号已经重复,请核实!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("dh", param["dh"].ToString());
            if (DAL.GetCount(typeof(Td_Jh_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在进货单中,请核实!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("dh", param["dh"].ToString());
            if (DAL.GetCount(typeof(Td_Jh_Th_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在退货单中,请核实!");
                return br;
            }

            #endregion


            int xh = 1;

            foreach (var item in td_Fk_2_List)
            {
                item.id = Guid.NewGuid().ToString();
                item.id_masteruser = param["id_masteruser"].ToString();
                item.id_bill = param["id"].ToString();
                item.xh = item.sort_id = xh;
                item.rq_create = DateTime.Parse(param["rq"].ToString());
                item.dh = param["dh"].ToString();
                xh++;
            }


            #endregion
            #region 操作数据库
            ht.Clear();
            ht.Add("id", param["id"].ToString());
            ht.Add("new_dh", param["dh"].ToString());
            ht.Add("new_rq", DateTime.Parse(param["rq"].ToString()));
            ht.Add("new_id_shop", param["id_shop"].ToString());
            ht.Add("new_id_gys", param["id_gys"].ToString());
            ht.Add("new_id_jbr", param["id_jbr"].ToString());
            ht.Add("new_bz", param["remark"].ToString());
            ht.Add("new_je_fk_mxtotal", td_Fk_2_List.Sum(d => d.je_fk));//付款金额
            ht.Add("new_je_yh_mxtotal", td_Fk_2_List.Sum(d => d.je_yh));//优惠金额
            ht.Add("new_je_total", td_Fk_2_List.Sum(d => d.je_fk)+ td_Fk_2_List.Sum(d => d.je_yh));//付款总金额
            ht.Add("new_id_edit", param["id_user"].ToString());
            ht.Add("new_rq_edit", DateTime.Now);
            DAL.UpdatePart(typeof(Td_Fk_1), ht);


            ht.Clear();
            ht.Add("id_bill", param["id"].ToString());
            DAL.Delete(typeof(Td_Fk_2), ht);
            DAL.AddRange<Td_Fk_2>(td_Fk_2_List);
            #endregion
            br.Message.Add(String.Format("操作成功!"));
            br.Success = true;
            br.Data = param["id"].ToString();
            return br;
        }
        #endregion

        #region TurnTd_Fk_1Model
        /// <summary>
        /// 将Hashtable转换为Model
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private Td_Fk_1 TurnTd_Fk_1Model(Hashtable param)
        {
            Td_Fk_1 model = new Td_Fk_1();
            model.id_masteruser = param["id_masteruser"].ToString();
            model.id = Guid.NewGuid().ToString();
            model.dh = param["dh"].ToString();
            model.rq = DateTime.Parse(param["rq"].ToString());
            model.id_shop = param["id_shop"].ToString();
            model.bm_djlx = "CW001";
            model.id_gys = param["id_gys"].ToString();
            model.id_jbr = param["id_jbr"].ToString();
            model.je_fk_mxtotal = 0m;//明细总金额
            model.je_yh_mxtotal = 0m;//明细优惠总金额
            model.je_total = 0m;//付款金额
            model.flag_sh = (byte)Enums.FlagSh.UnSh;
            model.flag_cancel = (byte)Enums.FlagCancel.NoCancel;
            model.bz = param["remark"].ToString();
            model.id_create = param["id_user"].ToString();
            model.rq_create = DateTime.Now;
            model.flag_delete = (byte)Enums.FlagDelete.NoDelete;
            return model;
        }
        #endregion

        #region 审核
        /// <summary>
        /// 审核
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
        public  BaseResult ActiveWork(Hashtable param)
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
            var brFk = this.Get(ht);
            if (!brFk.Success)
            {
                return brFk;
            }
            else
            {
                #region 校验商品合法性
                Td_Fk_1_Query_DetailModel dbModel = (Td_Fk_1_Query_DetailModel)brFk.Data;
                if (dbModel == null || dbModel.head == null || string.IsNullOrEmpty(dbModel.head.id) || dbModel.body == null || dbModel.body.Count() <= 0)
                {
                    result.Success = false;
                    result.Message.Add("获取付款单信息不符合要求!");
                    return result;
                }
                #endregion
                #region 执行存储过程并返回结果
                ht.Clear();
                ht.Add("proname", "p_fk_sh");
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

    }
}
