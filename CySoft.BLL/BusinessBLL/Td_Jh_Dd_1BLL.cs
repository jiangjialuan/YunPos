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
using CySoft.IBLL;

//订货
namespace CySoft.BLL.BusinessBLL
{

    public class Td_Jh_Dd_1BLL : BaseBLL
    {

        #region 获取分页数据
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Td_Jh_Dd_1), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Td_Jh_Dd_1_QueryModel>(typeof(Td_Jh_Dd_1), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }
        #endregion

        #region 获取单条
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            try
            {
                Td_Jh_Dd_1_Query_DetailModel result = new Td_Jh_Dd_1_Query_DetailModel();
                var head = DAL.GetItem<Td_Jh_Dd_1_QueryModel>(typeof(Td_Jh_Dd_1), param);
                if (head != null && !string.IsNullOrEmpty(head.id))
                {

                    result.head = head;
                    param.Clear();
                    param.Add("id_bill", head.id);
                    param.Add("dh", head.dh);
                    var body = DAL.QueryList<Td_Jh_Dd_2_QueryModel>(typeof(Td_Jh_Dd_2), param).OrderBy(d=>d.sort_id);
                    result.body = body.ToList();
                    res.Data = result;
                }
                else
                {
                    res.Success = false;
                    res.Message.Add("未找到此订货单信息!");
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message.Add("获取单条数据操作异常!");
            }
            return res;
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
            var td_Jh_Dd_2_List = (List<Td_Jh_Dd_2>)param["shopspList"];

            var digit = param["DigitHashtable"] as System.Collections.Hashtable;//小数点控制 
            var td_Jh_Dd_1_Model = this.TurnTd_Jh_Dd_1Model(param);

            #region 检测此单号是否已经存在
            ht.Clear();
            ht.Add("id_masteruser", td_Jh_Dd_1_Model.id_masteruser);
            ht.Add("dh", td_Jh_Dd_1_Model.dh);
            if (DAL.GetCount(typeof(Td_Jh_Dd_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在订货单中,请核实!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", td_Jh_Dd_1_Model.id_masteruser);
            ht.Add("dh", td_Jh_Dd_1_Model.dh);
            if (DAL.GetCount(typeof(Td_Jh_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在订货单中,请核实!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", td_Jh_Dd_1_Model.id_masteruser);
            ht.Add("dh", td_Jh_Dd_1_Model.dh);
            if (DAL.GetCount(typeof(Td_Fk_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在付款单中,请核实!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", td_Jh_Dd_1_Model.id_masteruser);
            ht.Add("dh", td_Jh_Dd_1_Model.dh);
            if (DAL.GetCount(typeof(Td_Jh_Th_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在退货单中,请核实!");
                return br;
            }

            var shopModel = QueryShopById(td_Jh_Dd_1_Model.id_shop);
            if (shopModel == null || shopModel.id.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("该门店已停用或删除 不允许操作!");
                return br;
            }
            #endregion

            int xh = 1;

            foreach (var item in td_Jh_Dd_2_List)
            {
                item.id = Guid.NewGuid().ToString();
                item.id_masteruser = td_Jh_Dd_1_Model.id_masteruser;
                item.id_bill = td_Jh_Dd_1_Model.id;
                item.sort_id = xh;
                item.rq_create = td_Jh_Dd_1_Model.rq_create;
                //item.dh = td_Jh_1_Model.dh;
                xh++;
            }

            td_Jh_Dd_1_Model.je_mxtotal = td_Jh_Dd_2_List.Sum(d => d.je);


            #endregion

            #region 插入数据库
            DAL.Add(td_Jh_Dd_1_Model);
            DAL.AddRange<Td_Jh_Dd_2>(td_Jh_Dd_2_List);
            #endregion

            #region 审核
            //审核
            if (param["autoAudit"].ToString().ToLower()=="true")
            {
                ht.Clear();
                ht.Add("id", td_Jh_Dd_1_Model.id);
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
            br.Data = td_Jh_Dd_1_Model.id;
            return br;
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
            var td_Jh_Dh_2_List = (List<Td_Jh_Dd_2>)param["shopspList"];

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
            if (DAL.GetCount(typeof(Td_Jh_Dd_1), ht) <= 0)
            {
                br.Success = false;
                br.Message.Add("未查询到此id订单,请重试!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString() );
            ht.Add("dh", param["dh"].ToString());
            ht.Add("not_id", param["id"].ToString());
            if (DAL.GetCount(typeof(Td_Jh_Dd_1), ht) > 0)
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
                br.Message.Add("此单号已经存在订货单中,请核实!");
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

            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("dh", param["dh"].ToString());
            if (DAL.GetCount(typeof(Td_Jh_Th_1), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("此单号已经存在退货单中,请核实!");
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

            foreach (var item in td_Jh_Dh_2_List)
            {
                item.id = Guid.NewGuid().ToString();
                item.id_masteruser = param["id_masteruser"].ToString();
                item.id_bill = param["id"].ToString();
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
            ht.Add("new_id_gys", param["id_gys"].ToString());
            ht.Add("new_id_jbr", param["id_jbr"].ToString());
            ht.Add("new_bz", param["remark"].ToString());
            ht.Add("new_je_mxtotal", td_Jh_Dh_2_List.Sum(d => d.je));
            ht.Add("new_id_edit", param["id_user"].ToString());
            ht.Add("new_rq_edit", DateTime.Now);


            DAL.UpdatePart(typeof(Td_Jh_Dd_1), ht);

            ht.Clear();
            ht.Add("id_bill", param["id"].ToString());
            DAL.Delete(typeof(Td_Jh_Dd_2), ht);
            DAL.AddRange<Td_Jh_Dd_2>(td_Jh_Dh_2_List);
            #endregion
            br.Message.Add(String.Format("操作成功!"));
            br.Success = true;
            br.Data = param["id"].ToString();
            return br;
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
                Td_Jh_Dd_1_Query_DetailModel dbModel = (Td_Jh_Dd_1_Query_DetailModel)brJh.Data;
                if (dbModel == null || dbModel.head == null || string.IsNullOrEmpty(dbModel.head.id) || dbModel.body == null || dbModel.body.Count() <= 0)
                {
                    result.Success = false;
                    result.Message.Add("获取订货单信息不符合要求!");
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
                ht.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);

                try
                {
                    if (DAL.UpdatePart(typeof(Td_Jh_Dd_1), ht) <= 0)
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
            var brJh = this.Get(ht);
            if (!brJh.Success)
            {
                return brJh;
            }
            else
            {
                #region 校验商品合法性
                Td_Jh_Dd_1_Query_DetailModel dbModel = (Td_Jh_Dd_1_Query_DetailModel)brJh.Data;
                if (dbModel == null || dbModel.head == null || string.IsNullOrEmpty(dbModel.head.id) || dbModel.body == null || dbModel.body.Count() <= 0)
                {
                    result.Success = false;
                    result.Message.Add("获取订货单信息不符合要求!");
                    return result;
                }

                ht.Clear();
                ht.Add("id_masteruser", id_masteruser);
                ht.Add("id", dbModel.head.id_shop);
                var tb_shop = DAL.GetItem<Tb_Shop>(typeof(Tb_Shop), ht);
                if (tb_shop == null || string.IsNullOrEmpty(tb_shop.id))
                {
                    result.Success = false;
                    result.Message.Add("获取收货门店信息失败!");
                    return result;
                }

                ht.Clear();
                ht.Add("id_masteruser", id_masteruser);
                ht.Add("id_shop", dbModel.head.id_shop);
                var shopspList = DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), ht);
                if (shopspList == null || shopspList.Count() <= 0)
                {
                    result.Success = false;
                    result.Message.Add("该收货门店下不存在商品!");
                    return result;
                }




                var noShopspList = (from body in dbModel.body
                                    where shopspList.Count(d => d.id.ToString().Trim() == body.id_shopsp.ToString().Trim()) == 0
                                    select body).ToList();
                if (noShopspList != null && noShopspList.Count > 0)
                {

                    ht.Clear();
                    ht.Add("id_masteruser", id_masteruser);
                    ht.Add("id", noShopspList.FirstOrDefault().id_shopsp);
                    var sp = DAL.GetItem<Tb_Shopsp>(typeof(Tb_Shopsp), ht);

                    result.Success = false;
                    if (sp != null && !string.IsNullOrEmpty(sp.id))
                        result.Message.Add(string.Format("审核失败,门店[{0}]不存在商品[{1}]", tb_shop.mc, sp.bm));
                    else
                        result.Message.Add(string.Format("审核失败,门店[{0}]不存在商品[{1}]", tb_shop.mc, noShopspList.FirstOrDefault().id_shopsp));
                    throw new CySoftException(result);
                }

                var stopShopspList = (from body in dbModel.body
                                      where shopspList.Count(d => d.id.ToString().Trim() == body.id_shopsp.ToString().Trim() && d.flag_state == (byte)Enums.FlagShopspStop.Stoped) > 0
                                      select body).ToList();
                if (stopShopspList != null && stopShopspList.Count > 0)
                {
                    result.Success = false;
                    result.Message.Add(string.Format("审核失败,商品编码为[{0}]已停用！", shopspList.Where(d => d.id == stopShopspList.FirstOrDefault().id_shopsp).FirstOrDefault().bm));
                    throw new CySoftException(result);
                }

                var noSpflShopspList = (from body in dbModel.body
                                        where shopspList.Count(d => d.id.ToString().Trim() == body.id_shopsp.ToString().Trim() && d.id_spfl == "0") > 0
                                        select body).ToList();
                if (noSpflShopspList != null && noSpflShopspList.Count > 0)
                {
                    result.Success = false;
                    result.Message.Add(string.Format("审核失败,商品编码为[{0}]未设置分类", shopspList.Where(d => d.id == noSpflShopspList.FirstOrDefault().id_shopsp).FirstOrDefault().bm));
                    throw new CySoftException(result);
                }


                var noId_kcspShopspList = (from body in dbModel.body
                                           where shopspList.Count(d => d.id.ToString().Trim() == body.id_shopsp.ToString().Trim() && (d.id_kcsp == null || d.id_kcsp == "")) > 0
                                           select body).ToList();
                if (noId_kcspShopspList != null && noId_kcspShopspList.Count > 0)
                {
                    result.Success = false;
                    result.Message.Add(string.Format("审核失败,商品编码为[{0}] 库存ID 为空！", shopspList.Where(d => d.id == noId_kcspShopspList.FirstOrDefault().id_shopsp).FirstOrDefault().bm));
                    throw new CySoftException(result);
                }


                var noDwShopspList = (from body in dbModel.body
                                      where shopspList.Count(d => d.id.ToString().Trim() == body.id_shopsp.ToString().Trim() && (d.dw == null || d.dw == "")) > 0
                                      select body).ToList();
                if (noDwShopspList != null && noDwShopspList.Count > 0)
                {
                    result.Success = false;
                    result.Message.Add(string.Format("审核失败,商品编码为[{0}] 单位 为空！", shopspList.Where(d => d.id == noDwShopspList.FirstOrDefault().id_shopsp).FirstOrDefault().bm));
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

                #endregion
                #region 执行存储过程并返回结果
                ht.Clear();
                ht.Add("proname", "p_jh_dd_sh");
                ht.Add("errorid", "-1");
                ht.Add("errormessage", "未知错误！");
                ht.Add("id_bill", dbModel.head.id);
                ht.Add("id_user", id_user);
                DAL.RunProcedure(ht);

                if (!ht.ContainsKey("errorid") || !ht.ContainsKey("errormessage"))
                {
                    result.Success = false;
                    result.Message.Add("审核失败,执行审核出现异常!");
                    throw new CySoftException(result);
                }

                if (!string.IsNullOrEmpty(ht["errorid"].ToString()) || !string.IsNullOrEmpty(ht["errormessage"].ToString()))
                {
                    result.Success = false;
                    result.Message.Add(ht["errormessage"].ToString());
                    throw new CySoftException(result);
                }

                result.Success = true;
                result.Message.Add("审核成功,审核成功!");
                return result;
                #endregion
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
            #region 之前的
            //Hashtable ht = new Hashtable();
            //ht.Add("id", id);
            //ht.Add("id_masteruser", id_masteruser);
            //ht.Add("new_flag_cancel", (int)Enums.FlagCancel.Canceled);
            //try
            //{
            //    if (DAL.UpdatePart(typeof(Td_Jh_1), ht) <= 0)
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
                Td_Jh_Dd_1_Query_DetailModel dbModel = (Td_Jh_Dd_1_Query_DetailModel)brJh.Data;
                if (dbModel == null || dbModel.head == null || string.IsNullOrEmpty(dbModel.head.id))
                {
                    result.Success = false;
                    result.Message.Add("获取进货单信息不符合要求!");
                    return result;
                }
                #endregion
                #region 执行存储过程并返回结果
                ht.Clear();
                ht.Add("proname", "p_jh_dd_zf");
                ht.Add("errorid", "-1");
                ht.Add("errormessage", "未知错误！");
                ht.Add("id_bill", dbModel.head.id);
                ht.Add("id_user", id_user);
                DAL.RunProcedure(ht);

                if (!ht.ContainsKey("errorid") || !ht.ContainsKey("errormessage"))
                {
                    result.Success = false;
                    result.Message.Add("作废失败,返回参数出现异常!");
                    throw new CySoftException(result);
                }

                if (!string.IsNullOrEmpty(ht["errorid"].ToString()) || !string.IsNullOrEmpty(ht["errormessage"].ToString()))
                {
                    result.Success = false;
                    result.Message.Add(ht["errormessage"].ToString());
                    throw new CySoftException(result);
                }

                result.Success = true;
                result.Message.Add("作废操作成功!");
                return result;
                #endregion
            }
            #endregion
        }
        #endregion

        #region TurnTd_Jh_Dd_1Model
        /// <summary>
        /// 将Hashtable转换为Model
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private Td_Jh_Dd_1 TurnTd_Jh_Dd_1Model(Hashtable param)
        {
            Td_Jh_Dd_1 model = new Td_Jh_Dd_1();
            model.id_masteruser = param["id_masteruser"].ToString();
            model.id = Guid.NewGuid().ToString();
            model.dh = param["dh"].ToString();
            model.rq = DateTime.Parse(param["rq"].ToString());
            model.id_shop = param["id_shop"].ToString();
            model.id_gys = param["id_gys"].ToString();
            model.id_jbr = param["id_jbr"].ToString();
            model.je_mxtotal = 0m;

            model.bz = param["remark"].ToString();
            model.id_create = param["id_user"].ToString();
            model.rq_create = DateTime.Now;
            model.flag_delete = (byte)Enums.FlagDelete.NoDelete;
            model.flag_sh = (byte)Enums.FlagSh.UnSh;
            model.bm_djlx = "JH010";
            model.flag_cancel = (byte)Enums.FlagCancel.NoCancel;

            model.flag_jyfs = 0;
            model.rq_jh = DateTime.Now.AddYears(1);
            
            return model;
        }
        #endregion

        #region 是否允许进货
        /// <summary>
        ///是否允许进货
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult Export(Hashtable param)
        {
            #region 参数验证
            BaseResult result = new BaseResult() { Success = false };
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
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("start", 0);
            ht.Add("limit", 10);
            var data = DAL.QueryPage<Td_Jh_Dd_1_QueryModel>(typeof(Td_Jh_Dd_1), ht).FirstOrDefault();
            if (data == null || string.IsNullOrEmpty(data.id))
            {
                result.Success = false;
                result.Message.Add("未查询到符合的数据!");
                return result;
            }

            var shopModel = QueryShopById(data.id_shop);
            if (shopModel == null || shopModel.id.IsEmpty())
            {
                result.Success = false;
                result.Message.Add("该门店已停用或删除 不允许操作!");
                return result;
            }

            ht.Clear();
            ht.Add("id_bill_origin", id);
            ht.Add("flag_sh", (byte)Enums.FlagSh.UnSh);
            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            if (DAL.GetCount(typeof(Td_Jh_1), ht) > 0)
            {
                result.Success = false;
                result.Message.Add("该订单存在引单未审核进货订单 不允许操作!");
                return result;
            }


            if (data.finish_jh == 0 && data.flag_sh == 1 && data.flag_cancel == 0)
            {
                result.Success = true;
                result.Message.Add("允许进货操作!");
                return result;
            }
            else
            {
                if (data.finish_jh==1)
                {
                    result.Success = false;
                    result.Message.Add("本单据已完成进货 请刷新数据后重试!");
                    return result;
                }
                if (data.flag_sh != 1)
                {
                    result.Success = false;
                    result.Message.Add("本单据还未审核 不允许进货!");
                    return result;
                }
                if (data.flag_cancel != 0)
                {
                    result.Success = false;
                    result.Message.Add("本单据已作废 不允许进货!");
                    return result;
                }
            }

            #endregion
            result.Success = false;
            result.Message.Add("本单据不允许进货!");
            return result;
        }
        #endregion


        #region GetAll
        public override BaseResult GetAll(Hashtable param)
        {
            BaseResult br = new BaseResult();
            var sp = DAL.QueryList<Td_Jh_Dd_1_QueryModel>(typeof(Td_Jh_Dd_1), param);
            br.Data = sp;
            br.Success = true;
            return br;
        }
        #endregion



    }
}
