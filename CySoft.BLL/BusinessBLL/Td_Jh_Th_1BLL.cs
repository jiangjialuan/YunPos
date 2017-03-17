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
using CySoft.Model.Tb;


namespace CySoft.BLL.BusinessBLL
{

    public class Td_Jh_Th_1BLL : BaseBLL
    {

        #region 获取分页数据
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Td_Jh_Th_1), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Td_Jh_Th_1_QueryModel>(typeof(Td_Jh_Th_1), param);
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
            var td_Jh_Th_2_List = (List<Td_Jh_Th_2>)param["shopspList"];
            var digit = param["DigitHashtable"] as System.Collections.Hashtable;//小数点控制 

            var td_Jh_Th_1_Model = this.TurnTd_Jh_1Model(param);
            int xh = 1;

            foreach (var item in td_Jh_Th_2_List)
            {
                item.id = Guid.NewGuid().ToString();
                item.id_masteruser = td_Jh_Th_1_Model.id_masteruser;
                item.id_bill = td_Jh_Th_1_Model.id;
                item.xh = item.sort_id = xh;
                item.rq_create = td_Jh_Th_1_Model.rq_create;
                item.dh = td_Jh_Th_1_Model.dh;
                xh++;
            }
            td_Jh_Th_1_Model.je_mxtotal = td_Jh_Th_2_List.Sum(d => d.je);
            #endregion

            #region 检查单号是否重复

            ht.Clear();
            ht.Add("id_masteruser", td_Jh_Th_1_Model.id_masteruser);
            ht.Add("dh", td_Jh_Th_1_Model.dh);
            if (DAL.GetCount(typeof(Td_Jh_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在进货单中,请核实!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", td_Jh_Th_1_Model.id_masteruser);
            ht.Add("dh", td_Jh_Th_1_Model.dh);
            if (DAL.GetCount(typeof(Td_Fk_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在付款单中,请核实!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", td_Jh_Th_1_Model.id_masteruser);
            ht.Add("dh", td_Jh_Th_1_Model.dh);
            if (DAL.GetCount(typeof(Td_Jh_Th_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在退货单中,请核实!");
                return br;
            }


            var shopModel = QueryShopById(td_Jh_Th_1_Model.id_shop);
            if (shopModel == null || shopModel.id.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("该门店已停用或删除 不允许操作!");
                return br;
            }


            #endregion

            #region 插入数据库
            DAL.Add(td_Jh_Th_1_Model);
            DAL.AddRange<Td_Jh_Th_2>(td_Jh_Th_2_List);
            #endregion

            #region 审核
            //审核
            if (param["autoAudit"].ToString().ToLower() == "true")
            {
                ht.Clear();
                ht.Add("id", td_Jh_Th_1_Model.id);
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
            br.Data = td_Jh_Th_1_Model.id;
            return br;
        }
        #endregion

        #region 获取单条
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            try
            {
                Td_Jh_Th_1_Query_DetailModel result = new Td_Jh_Th_1_Query_DetailModel();
                var head = DAL.GetItem<Td_Jh_Th_1_QueryModel>(typeof(Td_Jh_Th_1), param);
                if (head != null && !string.IsNullOrEmpty(head.id))
                {

                    result.head = head;
                    param.Clear();
                    param.Add("id_bill", head.id);
                    param.Add("dh", head.dh);
                    var body = DAL.QueryList<Td_Jh_Th_2_QueryModel>(typeof(Td_Jh_Th_2), param);
                    result.body = body.ToList();
                    res.Data = result;
                }
                else
                {
                    res.Success = false;
                    res.Message.Add("未找到此退货单信息!");
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
        public BaseResult ActiveWork(Hashtable param)
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
            var id_user = param["id_user"].ToString();
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
            #region 数据操作
            Hashtable ht = new Hashtable();
            ht.Add("id", id);
            var brJh = this.Get(ht);
            if (!brJh.Success)
            {
                return brJh;
            }
            else
            {
                #region 校验商品合法性
                Td_Jh_Th_1_Query_DetailModel dbModel = (Td_Jh_Th_1_Query_DetailModel)brJh.Data;
                if (dbModel == null || dbModel.head == null || string.IsNullOrEmpty(dbModel.head.id) || dbModel.body == null || dbModel.body.Count() <= 0)
                {
                    result.Success = false;
                    result.Message.Add("获取进货单信息不符合要求!");
                    return result;
                }

                ht.Clear();
                ht.Add("id_masteruser", id_masteruser);
                ht.Add("id_shop", dbModel.head.id_shop);
                var shopspList = DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), ht);
                if (shopspList == null || shopspList.Count() <= 0)
                {
                    result.Success = false;
                    result.Message.Add("该门店下不存在商品!");
                    return result;
                }


                //ht.Clear();
                //ht.Add("id_masteruser", id_masteruser);
                //ht.Add("id_shop", dbModel.head.id_shop);
                //var kcList = DAL.QueryList<Tz_Sp_Kc>(typeof(Tz_Sp_Kc), ht);
                //if (kcList == null || kcList.Count() <= 0)
                //{
                //    result.Success = false;
                //    result.Message.Add("未查询到库存信息!");
                //    return result;
                //}


                var noShopspList = (from body in dbModel.body
                                    where shopspList.Count(d => d.id.ToString().Trim() == body.id_shopsp.ToString().Trim()) == 0
                                    select body).ToList();

                if (noShopspList != null && noShopspList.Count > 0)
                {
                    result.Success = false;
                    result.Message.Add(string.Format("过账失败,门店[{0}]不存在商品[{1}]", dbModel.head.shop_name, noShopspList.FirstOrDefault().shopsp_name));
                    throw new CySoftException(result);
                }

                var stopShopspList = (from body in dbModel.body
                                      where shopspList.Count(d => d.id.ToString().Trim() == body.id_shopsp.ToString().Trim() && d.flag_state == (byte)Enums.FlagShopspStop.Stoped) > 0
                                      select body).ToList();
                if (stopShopspList != null && stopShopspList.Count > 0)
                {
                    result.Success = false;
                    result.Message.Add(string.Format("过账失败,商品为[{0}]已停用！", stopShopspList.FirstOrDefault().shopsp_name));
                    throw new CySoftException(result);
                }

                var noSpflShopspList = (from body in dbModel.body
                                        where shopspList.Count(d => d.id.ToString().Trim() == body.id_shopsp.ToString().Trim() && d.id_spfl == "0") > 0
                                        select body).ToList();
                if (noSpflShopspList != null && noSpflShopspList.Count > 0)
                {
                    result.Success = false;
                    result.Message.Add(string.Format("过账失败,商品为[{0}]未设置分类", noSpflShopspList.FirstOrDefault().shopsp_name));
                    throw new CySoftException(result);
                }


                var noId_kcspShopspList = (from body in dbModel.body
                                           where shopspList.Count(d => d.id.ToString().Trim() == body.id_shopsp.ToString().Trim() && (d.id_kcsp == null || d.id_kcsp == "")) > 0
                                           select body).ToList();
                if (noId_kcspShopspList != null && noId_kcspShopspList.Count > 0)
                {
                    result.Success = false;
                    result.Message.Add(string.Format("过账失败,商品为[{0}] 库存ID 为空！", noId_kcspShopspList.FirstOrDefault().shopsp_name));
                    throw new CySoftException(result);
                }


                var noDwShopspList = (from body in dbModel.body
                                      where shopspList.Count(d => d.id.ToString().Trim() == body.id_shopsp.ToString().Trim() && (d.dw == null || d.dw == "")) > 0
                                      select body).ToList();
                if (noDwShopspList != null && noDwShopspList.Count > 0)
                {
                    result.Success = false;
                    result.Message.Add(string.Format("过账失败,商品为[{0}] 单位 为空！", noDwShopspList.FirstOrDefault().shopsp_name));
                    throw new CySoftException(result);
                }

                var noIdSpList = (from body in dbModel.body
                                  where shopspList.Count(d => d.id_sp.ToString().Trim() == body.id_sp.ToString().Trim()) == 0
                                  select body).ToList();
                if (noIdSpList != null && noIdSpList.Count > 0)
                {
                    result.Success = false;
                    result.Message.Add(string.Format("过账失败,门店[{0}]不存在商品[{1}]的id_sp", dbModel.head.shop_name, noIdSpList.FirstOrDefault().shopsp_name));
                    throw new CySoftException(result);
                }

                var bodyInfo = dbModel.body.GroupBy(d => d.id_kcsp).Select(d => new { id_kcsp = d.Key, sl_qm = d.Sum(s => s.sl_total), je_qm = d.Sum(s => s.je) });


                #endregion

                ht.Clear();
                ht.Add("proname", "p_jh_th_sh");
                ht.Add("errorid", "-1");
                ht.Add("errormessage", "未知错误！");
                ht.Add("id_bill", dbModel.head.id);
                ht.Add("id_user", id_user);
                DAL.RunProcedure(ht);

                if (!ht.ContainsKey("errorid") || !ht.ContainsKey("errormessage"))
                {
                    result.Success = false;
                    result.Message.Add("过账失败,执行审核出现异常!");
                    throw new CySoftException(result);
                }

                if (!string.IsNullOrEmpty(ht["errorid"].ToString()) || !string.IsNullOrEmpty(ht["errormessage"].ToString()))
                {
                    result.Success = false;
                    result.Message.Add(ht["errormessage"].ToString());
                    throw new CySoftException(result);
                }

                result.Success = true;
                result.Message.Add("过账成功,审核成功!");
                return result;

            } 
            #endregion
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
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("new_flag_cancel", (int)Enums.FlagCancel.Canceled);
            try
            {
                if (DAL.UpdatePart(typeof(Td_Jh_Th_1), ht) <= 0)
                {
                    result.Success = false;
                    result.Message.Add("作废操作失败!");
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message.Add("作废操作异常!");
            }
            #endregion
            return result;
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

                Td_Jh_Th_1_Query_DetailModel dbModel = (Td_Jh_Th_1_Query_DetailModel)brJh.Data;
                if (dbModel == null || dbModel.head == null || string.IsNullOrEmpty(dbModel.head.id) || dbModel.body == null || dbModel.body.Count() <= 0)
                {
                    result.Success = false;
                    result.Message.Add("获取退货单信息不符合要求!");
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
                    if (DAL.UpdatePart(typeof(Td_Jh_Th_1), ht) <= 0)
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
            var td_Jh_Th_2_List = (List<Td_Jh_Th_2>)param["shopspList"];

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

            if (DAL.GetCount(typeof(Td_Jh_Th_1), ht) <= 0)
            {
                br.Success = false;
                br.Message.Add("未查询到此id订单,请重试!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("dh", param["dh"].ToString());
            ht.Add("not_id", param["id"].ToString());
            if (DAL.GetCount(typeof(Td_Jh_Th_1), ht) > 0)
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
            if (DAL.GetCount(typeof(Td_Fk_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在付款单中,请核实!");
                return br;
            }

            var shopModel = QueryShopById(param["id_shop"].ToString());
            if (shopModel == null || shopModel.id.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("该门店已停用或删除 不允许操作!");
                return br;
            }

            #endregion


            int xh = 1;

            foreach (var item in td_Jh_Th_2_List)
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
            ht.Add("new_je_mxtotal", td_Jh_Th_2_List.Sum(d => d.je));
            ht.Add("new_id_edit", param["id_user"].ToString());
            ht.Add("new_rq_edit", DateTime.Now);
            DAL.UpdatePart(typeof(Td_Jh_Th_1), ht);

            ht.Clear();
            ht.Add("id_bill", param["id"].ToString());
            DAL.Delete(typeof(Td_Jh_Th_2), ht);
            DAL.AddRange<Td_Jh_Th_2>(td_Jh_Th_2_List);
            #endregion
            br.Message.Add(String.Format("操作成功!"));
            br.Success = true;
            br.Data = param["id"].ToString();
            return br;
        }
        #endregion

        #region 进货过账 更新货插入库存
        /// <summary>
        /// 进货过账 更新货插入库存
        /// </summary>
        /// <param name="Td_Jh_Th_1_Model"></param>
        /// <param name="td_Jh_Th_2_List"></param>
        /// <param name="digit"></param>
        public void JH_ChangeKC(Td_Jh_Th_1 Td_Jh_Th_1_Model, List<Td_Jh_Th_2> td_Jh_Th_2_List, Hashtable digit)
        {
            Hashtable ht = new Hashtable();
            foreach (var item in td_Jh_Th_2_List)
            {
                var id_kcsp = item.id_kcsp;
                var id_masteruser = item.id_masteruser;
                var id_shop = Td_Jh_Th_1_Model.id_shop;
                ht.Clear();
                ht.Add("id_masteruser", id_masteruser);
                ht.Add("id_shop", id_shop);
                ht.Add("id_shopsp", id_kcsp);
                var dbModel = DAL.GetItem<Tz_Sp_Kc>(typeof(Tz_Sp_Kc), ht);
                if (dbModel != null)
                {
                    var sl_qm = dbModel.sl_qm.Digit((int)digit["sl_digit"]) + item.sl_total.Digit((int)digit["sl_digit"]);
                    var je_qm = dbModel.je_qm.Digit((int)digit["je_digit"]) + item.je.Digit((int)digit["je_digit"]);
                    var dj_cb = decimal.Parse("0").Digit((int)digit["dj_digit"]);
                    if (sl_qm != 0)
                    {
                        dj_cb = (decimal)((je_qm / sl_qm).Digit((int)digit["dj_digit"]));
                    }
                    ht.Add("new_sl_qm", sl_qm);
                    ht.Add("new_je_qm", je_qm);
                    ht.Add("new_dj_cb", dj_cb);
                    DAL.UpdatePart(typeof(Tz_Sp_Kc), ht);
                }
                else
                {

                    var kcModel = new Tz_Sp_Kc()
                    {
                        id_masteruser = id_masteruser,
                        id = Guid.NewGuid().ToString(),
                        id_shop = id_shop,
                        id_kcsp = id_kcsp
                    };

                    kcModel.sl_qm = item.sl_total.Digit((int)digit["sl_digit"]);
                    kcModel.je_qm = item.je.Digit((int)digit["je_digit"]);
                    kcModel.dj_cb = decimal.Parse("0").Digit((int)digit["dj_digit"]);
                    if (kcModel.je_qm != 0)
                    {
                        kcModel.dj_cb = (kcModel.je_qm / kcModel.sl_qm).Digit((int)digit["dj_digit"]);
                    }
                    DAL.Add(kcModel);
                }
            }
        }
        #endregion

        #region TurnTd_Jh_1Model
        /// <summary>
        /// 将Hashtable转换为Model
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private Td_Jh_Th_1 TurnTd_Jh_1Model(Hashtable param)
        {
            Td_Jh_Th_1 model = new Td_Jh_Th_1();
            model.id_masteruser = param["id_masteruser"].ToString();
            model.id = Guid.NewGuid().ToString();
            model.dh = param["dh"].ToString();
            model.rq = DateTime.Parse(param["rq"].ToString());
            model.id_shop = param["id_shop"].ToString();
            model.id_gys = param["id_gys"].ToString();
            model.id_jbr = param["id_jbr"].ToString();
            model.je_mxtotal = 0m;
            model.je_sf = decimal.Parse(param["je_sf"].ToString());
            model.bz = param["remark"].ToString();
            model.id_create = param["id_user"].ToString();
            model.rq_create = DateTime.Now;
            model.flag_delete = (byte)Enums.FlagDelete.NoDelete;
            model.flag_sh = (byte)Enums.FlagSh.UnSh;
            model.bm_djlx = "JH030";
            model.flag_cancel = (byte)Enums.FlagCancel.NoCancel;

            model.dh_origin = param["dh_origin"].ToString();
            model.bm_djlx_origin = param["bm_djlx_origin"].ToString();
            model.id_bill_origin = param["id_bill_origin"].ToString();

            return model;
        }
        #endregion


    }
}
