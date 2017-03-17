#region Imports
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.IDAL;
using CySoft.Model;
using CySoft.Model.Tb;
using CySoft.Model.Td;
using CySoft.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using CySoft.Frame.Utility;
using CySoft.Model.Tz;
using CySoft.Model.Enums;
using System.Data;
using CySoft.Model.Other;
using CySoft.Model.Ts;

#endregion

namespace CySoft.BLL.GoodsBLL
{
    public class Tb_ShopspBLL : BaseBLL, ITb_ShopspBLL
    {
        #region IDAL
        public ITb_ShopspDAL Tb_ShopspDAL { get; set; }
        #endregion

        #region 商品新增
        /// <summary>
        /// 商品新增
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
            List<Tb_Shopsp> list = new List<Tb_Shopsp>();
            Td_Sp_Qc cqModel = (Td_Sp_Qc)param["sp_qc"];
            var id_shop = param["id_shop"].ToString();
            var digitHashtable = (Hashtable)param["DigitHashtable"];
            int digitDj = 2;
            int.TryParse(digitHashtable["dj_digit"].ToString(), out digitDj);


            #region 获取店铺信息 遍历每个店铺都要插一份数据

            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());

            var shopList = (List<Tb_User_ShopWithShopMc>)param["shop_shop"] ?? new List<Tb_User_ShopWithShopMc>();

            var q = from p in shopList group p by p.id_shop into g select new { g.Key };

            if (q == null || q.Count() <= 0)
            {
                br.Data = new { id = "", name = "", value = "" };
                br.Success = false;
                br.Message.Add(String.Format("缺少门店信息数据"));
                return br;
            }

            if (q.Where(d => d.Key == id_shop).Count() <= 0)
            {
                br.Data = new { id = "", name = "", value = "" };
                br.Success = false;
                br.Message.Add(String.Format("门店信息数据出错"));
                return br;
            }

            foreach (var item in q)
            {
                if (param.ContainsKey("id_shop"))
                    param["id_shop"] = item.Key;
                else
                    param.Add("id_shop", item.Key);
                var tempList = TurnShopSPList(param);
                list.AddRange(tempList);
            }

            #endregion

            #endregion

            #region 获取所有商品
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            //ht.Add("flag_delete", 0);
            var allShopSpList = DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), ht);
            var allSpList = allShopSpList.Where(d => d.flag_delete == (byte)Enums.FlagDelete.NoDelete);
            #endregion

            #region 数据处理

            var id_shop_temp = id_shop;
            List<Tb_Shopsp> updateList = new List<Tb_Shopsp>();

            #region 处理商品List的 id_sp 和 id_kcsp 

            #region 处理1

            foreach (var item in list.Where(d => d.id_shop == id_shop_temp))
            {
                var tempModel = allSpList.Where(d => d.barcode == item.barcode).FirstOrDefault();

                if (item.id == item.id_kcsp)
                {
                    if (tempModel != null)
                    {
                        if (tempModel.id_sp != tempModel.id_kcsp)
                        {
                            br.Data = new { id = "", name = "", value = "" };
                            br.Message.Add(String.Format("新增商品失败,您填写的主商品条码已经是多包装子商品了" + "!"));
                            br.Success = false;
                            return br;
                        }
                        else
                        {
                            item.id_sp = item.id_kcsp = tempModel.id_sp;
                        }
                    }
                    else
                    {
                        item.id_sp = item.id_kcsp;
                    }
                }
                else
                {
                    if (tempModel != null)
                    {
                        if (tempModel.id_sp == tempModel.id_kcsp)
                        {
                            br.Data = new { id = "", name = "", value = "" };
                            br.Message.Add(String.Format("新增商品失败,您填写的子商品条码已经是多包装主商品了" + "!"));
                            br.Success = false;
                            return br;
                        }
                        else
                        {
                            item.id_sp = tempModel.id_sp;
                            item.id_kcsp = tempModel.id_kcsp;
                        }
                    }
                    else
                    {
                        item.id_sp = GetGuid;
                    }
                }
            }
            #endregion

            #region 处理2

            var spg = list.Where(d => d.id_shop == id_shop_temp).GroupBy(a => a.id_kcsp).Select(g => (new { name = g.Key }));

            if (spg.Count() != 1)
            {
                br.Data = new { id = "", name = "", value = "" };
                br.Message.Add(String.Format("新增商品失败,您填写的商品条码已经是多包装商品了" + "!"));
                br.Success = false;
                return br;
            }
            #endregion

            #region 处理3
            foreach (var item in list.Where(d => d.id_shop != id_shop_temp))
            {
                var id_sp_obj = list.Where(d => d.id_shop == id_shop_temp && d.barcode == item.barcode).FirstOrDefault();
                if (id_sp_obj == null)
                {
                    br.Data = new { id = "", name = "", value = "" };
                    br.Message.Add(String.Format("新增商品失败,处理商品id异常!"));
                    br.Success = false;
                    return br;
                }
                item.id_sp = id_sp_obj.id_sp;
                item.id_kcsp = id_sp_obj.id_kcsp;
            }
            #endregion

            #region 处理4
            foreach (var item in list)
            {
                //如果存在已删除的商品
                var delModel = allShopSpList.Where(d => d.id_shop == item.id_shop && d.id_sp == item.id_sp).FirstOrDefault();
                if (delModel != null && !string.IsNullOrEmpty(delModel.id))
                {
                    updateList.Add(item);
                }
            }
            #endregion

            #endregion

            #region 2017-02-15注释
            //var masterModel = list.Where(d => d.id_shop == id_shop_temp && d.id == d.id_kcsp).FirstOrDefault();

            //Tb_Shopsp sameModel = null;//数据库中存在的 条码重复的商品
            //List<Tb_Shopsp> sameDBZList = new List<Tb_Shopsp>();
            //if (allSpList.Where(d => masterModel.barcode == d.barcode && d.id_sp == d.id_kcsp).Count() > 0)
            //{
            //    sameModel = allSpList.Where(d => masterModel.barcode == d.barcode && d.id_sp == d.id_kcsp).FirstOrDefault();
            //    sameDBZList = allSpList.Where(d => d.id_masteruser == sameModel.id_masteruser && d.id_kcsp == sameModel.id_kcsp).ToList();
            //}

            //List<Tb_Shopsp> needUpdateModel = new List<Tb_Shopsp>();

            //if (sameModel != null && !string.IsNullOrEmpty(sameModel.id_sp) && !string.IsNullOrEmpty(sameModel.id_kcsp) && sameDBZList.Count() > 0)
            //{
            //    #region 如果有商品 取原商品的 id_sp 和 id_kcsp
            //    foreach (var item in list)
            //    {
            //        var tempModel = sameDBZList.Where(d => d.barcode == item.barcode && d.dw == item.dw).FirstOrDefault();
            //        if (tempModel != null && allShopSpList.Where(d => d.id_sp == tempModel.id_sp && d.id_shop == item.id_shop).Count() <= 0)
            //        {
            //            item.id_sp = tempModel.id_sp;
            //            item.id_kcsp = sameModel.id_kcsp;
            //        }
            //        else
            //        {
            //            item.id_sp = GetGuid;
            //            item.id_kcsp = sameModel.id_kcsp;
            //        }
            //        if (allShopSpList.Where(d => d.id_sp == item.id_sp && d.id_shop == item.id_shop).Count() > 0)
            //        {
            //            needUpdateModel.Add(item);
            //        }
            //    }
            //    #endregion
            //}
            //else
            //{
            //    #region 如果没有商品  按主商品处理（ id_sp 和 id_kcsp 都是重新生成的）
            //    //如果没有商品  按主商品处理
            //    foreach (var item in list.Where(d => d.id_shop == id_shop_temp))
            //    {
            //        if (item.id == item.id_kcsp)
            //        {
            //            item.id_sp = item.id;
            //        }
            //        else
            //            item.id_sp = GetGuid;
            //    }

            //    foreach (var item in list.Where(d => d.id_shop != id_shop_temp))
            //    {
            //        var id_sp_obj = list.Where(d => d.id_shop == id_shop_temp && d.barcode == item.barcode && d.bm == item.bm && d.dw == item.dw).FirstOrDefault();
            //        if (id_sp_obj == null)
            //        {
            //            br.Data = new { id = "", name = "", value = "" };
            //            br.Message.Add(String.Format("新增商品失败,处理商品id异常!"));
            //            br.Success = false;
            //            return br;
            //        }

            //        item.id_sp = id_sp_obj.id_sp;
            //        item.id_kcsp = id_sp_obj.id_kcsp;
            //    }
            //    #endregion
            //} 
            #endregion

            #region 注释

            //foreach (var item in list.Where(d => d.id_shop == id_shop_temp))
            //{
            //    if (item.id == item.id_kcsp)
            //    {
            //        if (sameModel != null && !string.IsNullOrEmpty(sameModel.id_sp) && !string.IsNullOrEmpty(sameModel.id_kcsp))
            //        {
            //            item.id_sp = sameModel.id_sp;
            //        }
            //        else
            //        {
            //            item.id_sp = item.id;
            //        }
            //    }
            //    else
            //    {
            //        if (sameModel != null && !string.IsNullOrEmpty(sameModel.id_sp) && !string.IsNullOrEmpty(sameModel.id_kcsp))
            //        {

            //            item.id_sp = sameModel.id_sp;
            //        }
            //        else
            //        {
            //            item.id_sp = GetGuid;
            //        }
            //    }
            //}



            ////id_sp 处理
            //foreach (var item in list.Where(d => d.id_shop == id_shop_temp))
            //{
            //    if (item.id == item.id_kcsp)
            //    {
            //        item.id_sp = item.id;
            //    }
            //    else
            //        item.id_sp = GetGuid;
            //}



            //foreach (var item in list.Where(d => d.id_shop != id_shop_temp))
            //{
            //    var id_sp_obj = list.Where(d => d.id_shop == id_shop_temp && d.barcode == item.barcode && d.bm == item.bm && d.dw == item.dw).FirstOrDefault();
            //    if (id_sp_obj == null)
            //    {
            //        br.Data = new { id = "", name = "", value = "" };
            //        br.Message.Add(String.Format("新增商品失败,处理商品id异常!"));
            //        br.Success = false;
            //        return br;
            //    }

            //    item.id_sp = id_sp_obj.id_sp;
            //    item.id_kcsp = id_sp_obj.id_kcsp;
            //} 
            #endregion

            #endregion

            #region 验证条码

            List<string> rShopList = new List<string>();
            foreach (var shop in q)
            {
                foreach (var item in list.GroupBy(d => d.barcode).Select(d => new { barcode = d.Key }))
                {
                    #region 验条码在提交数据中是否重复
                    var id_view = "";
                    if (list.Where(d => d.barcode == item.barcode && d.id_shop == shop.Key).Count() >= 2)
                    {
                        var item_temp = list.Where(d => d.barcode == item.barcode && d.id_shop == shop.Key).FirstOrDefault();
                        id_view = item_temp.id_view;
                        br.Data = new { id = id_view, name = "barcode", value = item_temp.barcode };
                        br.Message.Add(String.Format("提交的数据中 条码:{0} 存在重复数据!", item.barcode));
                        br.Success = false;
                        return br;
                    }
                    #endregion

                    #region 注释
                    //ht.Clear();
                    //ht.Add("barcode", item.barcode);
                    //ht.Add("flag_delete", "0");
                    //ht.Add("id_masteruser", param["id_masteruser"].ToString());
                    //ht.Add("id_shop", shop.Key);
                    //if (DAL.GetCount(typeof(Tb_Shopsp), ht) > 0)
                    //{
                    //    var item_temp = list.Where(d => d.barcode == item.barcode && d.id_shop == shop.Key).FirstOrDefault();
                    //    id_view = item_temp.id_view;
                    //    br.Data = new { id = id_view, name = "barcode", value = item_temp.barcode };
                    //    br.Message.Add(String.Format("提交的数据中 条码:{0} 已被占用!", item.barcode));
                    //    br.Success = false;
                    //    return br;
                    //}
                    #endregion

                    #region 验条码在数据库数据中是否被占用
                    if (allSpList != null && allSpList.Where(d => d.id_shop == shop.Key && d.barcode == item.barcode).Count() > 0)
                    {
                        if (shop.Key == id_shop)
                        {
                            //如果是自己门店重复 提示 如果是管理门店重复则不处理
                            var item_temp = list.Where(d => d.barcode == item.barcode && d.id_shop == shop.Key).FirstOrDefault();
                            id_view = item_temp.id_view;
                            br.Data = new { id = id_view, name = "barcode", value = item_temp.barcode };
                            br.Message.Add(String.Format("提交的数据中 条码:{0} 已被占用!", item.barcode));
                            br.Success = false;
                            return br;
                        }
                        else
                        {
                            rShopList.Add(shop.Key);
                        }
                    }
                    #endregion
                }

                #region 验编码在提交数据中是否重复
                foreach (var item in list.GroupBy(d => d.bm).Select(d => new { bm = d.Key }))
                {
                    var id_view = "";
                    if (list.Where(d => d.bm == item.bm && d.id_shop == shop.Key).Count() >= 2)
                    {
                        var item_temp = list.Where(d => d.bm == item.bm && d.id_shop == shop.Key).FirstOrDefault();
                        id_view = item_temp.id_view;
                        br.Data = new { id = id_view, name = "barcode", value = item_temp.barcode };
                        br.Message.Add(String.Format("提交的数据中 编码:{0} 存在重复数据!", item.bm));
                        br.Success = false;
                        return br;
                    }
                }
                #endregion
            }

            #endregion

            #region 保存至商品表
            var addList = list.Where(d => !rShopList.Contains(d.id_shop) && !(from a in updateList select a.id).Contains(d.id)).ToList();
            DAL.AddRange<Tb_Shopsp>(addList);
            #endregion

            #region 更新已存在的商品
            foreach (var item in updateList)
            {
                var delModel = allShopSpList.Where(d => d.id_shop == item.id_shop && d.id_sp == item.id_sp).FirstOrDefault();
                ht.Clear();
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id", delModel.id);
                #region 要更新的参数

                ht.Add("new_flag_delete", "0");
                if (delModel.barcode != item.barcode)
                {
                    ht.Add("new_barcode", item.barcode);//条码
                }

                if (delModel.bm != item.bm)
                {
                    ht.Add("new_bm", item.bm);//编码
                }

                if (delModel.mc != item.mc)
                {
                    ht.Add("new_mc", item.mc);//名称
                    ht.Add("new_zjm", CySoft.Utility.PinYin.GetChineseSpell(item.mc.ToString()));//助记码
                }

                if (delModel.cd != item.cd)
                {
                    ht.Add("new_cd", item.cd);//产地
                }

                if (delModel.dj_jh != item.dj_jh)
                {
                    ht.Add("new_dj_jh", item.dj_jh);//进货价
                }

                if (delModel.dj_ls != item.dj_ls)
                {
                    ht.Add("new_dj_ls", item.dj_ls);//零售价
                }

                if (delModel.dw != item.dw)
                {
                    ht.Add("new_dw", item.dw);//单位
                }

                if (delModel.sl_kc_min != item.sl_kc_min)
                {
                    ht.Add("new_sl_kc_min", item.sl_kc_min);//最底库存量
                }

                if (delModel.sl_kc_max != item.sl_kc_max)
                {
                    ht.Add("new_sl_kc_max", item.sl_kc_max);//最高库存量
                }

                if (delModel.pic_path != item.pic_path)
                {
                    ht.Add("new_pic_path", item.pic_path);//图片路径
                }

                if (delModel.id_spfl != item.id_spfl)
                {
                    ht.Add("new_id_spfl", item.id_spfl);//分类ID
                }

                if (delModel.dj_hy != item.dj_hy)
                {
                    ht.Add("new_dj_hy", item.dj_hy);//会员价
                }

                if (delModel.flag_czfs != item.flag_czfs)
                {
                    ht.Add("new_flag_czfs", item.flag_czfs);//计价方式
                }

                if (delModel.dj_ps != item.dj_ps)
                {
                    ht.Add("new_dj_ps", item.dj_ps);//配送价
                }

                if (delModel.dj_pf != item.dj_pf)
                {
                    ht.Add("new_dj_pf", item.dj_pf);//批发价
                }

                if (delModel.yxq != item.yxq)
                {
                    ht.Add("new_yxq", item.yxq);//有效期
                }

                if (delModel.id_gys != item.id_gys)
                {
                    ht.Add("new_id_gys", item.id_gys);//默认供应商
                }

                if (delModel.bz != item.bz)
                {
                    ht.Add("new_bz", item.bz);//备注
                }

                if (delModel.flag_state != item.flag_state)
                {
                    ht.Add("new_flag_state", item.flag_state);//flag_state
                }

                ht.Add("new_rq_edit", DateTime.Now);//修改日期 
                #endregion
                if (DAL.UpdatePart(typeof(Tb_Shopsp), ht) <= 0)
                {
                    br.Data = new { id = "", name = "", value = "" };
                    br.Success = false;
                    br.Message.Add("操作失败[更新数据库失败]");
                    throw new CySoftException(br);
                }
            }
            #endregion


            #region 保存单位
            foreach (var item in list.GroupBy(d => d.dw).Select(d => new { dw = d.Key }))
            {
                ht.Clear();
                ht.Add("dw", item.dw);
                ht.Add("flag_delete", "0");
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                if (DAL.GetCount(typeof(Tb_Dw), ht).Equals(0))
                {
                    var tb_dw = new Tb_Dw() { id = Guid.NewGuid().ToString(), dw = item.dw, id_create = param["id_user"].ToString(), id_masteruser = param["id_masteruser"].ToString(), rq_create = DateTime.Now, id_edit = param["id_user"].ToString(), rq_edit = DateTime.Now, flag_delete = 0 };
                    DAL.Add<Tb_Dw>(tb_dw);
                }
            }
            #endregion

            #region 保存期初和库存
            List<Td_Sp_Qc> qcList = new List<Td_Sp_Qc>();
            List<Tz_Sp_Kc> kcList = new List<Tz_Sp_Kc>();
            List<Tz_Jxc_Flow> flowList = new List<Tz_Jxc_Flow>();

            foreach (var item in q.Where(d => d.Key == id_shop))
            {
                if (cqModel != null && cqModel.sl_qc != null && cqModel.sl_qc != 0)
                {
                    var id_kcsp = list.OrderBy(d => d.rq_create).FirstOrDefault().id_kcsp;

                    Td_Sp_Qc addQCModel = new Td_Sp_Qc();
                    addQCModel.id_masteruser = param["id_masteruser"].ToString();
                    addQCModel.id = Guid.NewGuid().ToString();
                    addQCModel.id_shop = item.Key;
                    addQCModel.id_shopsp = id_kcsp;
                    addQCModel.je_qc = cqModel.je_qc;
                    addQCModel.sl_qc = cqModel.sl_qc;
                    addQCModel.dj_cb = (cqModel.je_qc / cqModel.sl_qc).Digit(digitDj);
                    addQCModel.id_kcsp = id_kcsp;
                    qcList.Add(addQCModel);

                    Tz_Sp_Kc addKCModel = new Tz_Sp_Kc();
                    addKCModel.id_masteruser = param["id_masteruser"].ToString();
                    addKCModel.id = Guid.NewGuid().ToString();
                    addKCModel.id_shop = addQCModel.id_shop;
                    addKCModel.id_kcsp = addQCModel.id_kcsp;
                    addKCModel.sl_qm = addQCModel.sl_qc;
                    addKCModel.je_qm = addQCModel.je_qc;
                    addKCModel.dj_cb = (addKCModel.je_qm / addKCModel.sl_qm).Digit(digitDj);
                    kcList.Add(addKCModel);


                    Tz_Jxc_Flow flowModel = new Tz_Jxc_Flow();
                    flowModel.id_masteruser = param["id_masteruser"].ToString();
                    flowModel.id = Guid.NewGuid().ToString();
                    flowModel.id_billmx = "qc";
                    flowModel.bm_djlx = "qc";
                    flowModel.id_shop = addQCModel.id_shop;
                    flowModel.id_shopsp = addQCModel.id_shopsp;
                    flowModel.sl = addQCModel.sl_qc;
                    flowModel.je = addQCModel.je_qc;
                    flowModel.rq = DateTime.Now;
                    flowModel.id_kcsp = addQCModel.id_kcsp;
                    flowList.Add(flowModel);

                }
            }

            DAL.AddRange<Td_Sp_Qc>(qcList);
            DAL.AddRange<Tz_Sp_Kc>(kcList);
            DAL.AddRange<Tz_Jxc_Flow>(flowList);
            #endregion

            #region 检验库存账本
            //检验库存账本
            ht.Clear();
            ht.Add("proname", "p_rzh_jykc");
            ht.Add("errorid", "-1");
            ht.Add("errormessage", "未知错误！");
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("id_shop", "0");
            DAL.RunProcedure2(ht);

            if (!ht.ContainsKey("errorid") || !ht.ContainsKey("errormessage"))
            {
                br.Success = false;
                br.Message.Add("检验库存账本失败,执行检验库存账本出现异常!");
                throw new CySoftException(br);
            }

            if (!string.IsNullOrEmpty(ht["errorid"].ToString()) || !string.IsNullOrEmpty(ht["errormessage"].ToString()))
            {
                br.Success = false;
                br.Message.Add(ht["errormessage"].ToString());
                throw new CySoftException(br);
            }

            #endregion

            var dbModel = list.Where(d => d.id_shop == id_shop && d.barcode == param["barcode"].ToString()).FirstOrDefault();
            if (dbModel == null || string.IsNullOrEmpty(dbModel.id))
                dbModel = list.Where(d => d.id_shop == id_shop).FirstOrDefault();
            br.Data = new { id = dbModel.id, id_kcsp = dbModel.id_kcsp, id_sp = dbModel.id_sp, zjm = dbModel.zjm };
            br.Message.Add(String.Format("新增商品。流水号：{0}，名称:{1}", list.FirstOrDefault().id, list.FirstOrDefault().mc));
            br.Success = true;
            return br;
        }
        #endregion

        #region 商品修改

        /// <summary>
        /// 商品修改  //
        /// lz 2016-08-04
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Save(dynamic entity)
        {
            #region 获取数据
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            Td_Sp_Qc qcModel = (Td_Sp_Qc)param["sp_qc"];
            var shopList = (List<Tb_User_ShopWithShopMc>)param["shop_shop"] ?? new List<Tb_User_ShopWithShopMc>();
            var shopDistinct = from shop in shopList group shop by shop.id_shop into g select new { g.Key };
            List<Tb_Shopsp_DBZ> dbzList = (List<Tb_Shopsp_DBZ>)param["dbzList"];
            List<Tb_Shopsp> addList = new List<Tb_Shopsp>();//用于存储多包装新增的商品信息
            var id_shop = param["id_shop"].ToString();
            var id_shop_master = param["id_shop_master"].ToString();
            var id_masteruser = param["id_masteruser"].ToString();
            bool updateQC = false;//是否新增期初
            bool updateZSP = false;//是否更新主商品
            bool updateDBZ = false;//是否更新多包装商品
            #endregion

            #region 获取商品原信息
            #region 获取所有商品
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            //ht.Add("flag_delete", 0);
            var allShopSpList = DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), ht).ToList();
            var allSpList = allShopSpList.Where(d => d.flag_delete == (byte)Enums.FlagDelete.NoDelete).ToList();
            List<Tb_Shopsp> updateList = new List<Tb_Shopsp>();
            #endregion
            #region 获取本修改商品的detail信息
            ht.Clear();
            ht.Add("id", param["id"].ToString());
            ht.Add("need_query_spfl", "No");
            ht.Add("need_query_shop", "No");
            ht.Add("need_query_kc", "No");
            ht.Add("need_query_dbz", "Yes");
            ht.Add("need_query_qc", "Yes");

            ht.Add("id_user", param["id_user"].ToString());
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            var copySPBr = this.Get(ht);
            if (!copySPBr.Success)
            {
                br.Data = new { id = "", name = "", value = "" };
                br.Success = false;
                br.Message.Add("查询商品相关信息异常");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            var data = (Tb_Shopsp_Get)copySPBr.Data;
            if (data == null || data.ShopSP == null || string.IsNullOrEmpty(data.ShopSP.id))
            {
                br.Data = new { id = "", name = "", value = "" };
                br.Success = false;
                br.Message.Add("商品不存在");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            var sp = data.ShopSP;
            #endregion
            #endregion

            #region 更新操作
            //try
            //{
            #region 验证期初是否变动
            if (data.Qc != null && qcModel != null && qcModel.sl_qc != null)
            {
                if (data.Qc.sl_qc != qcModel.sl_qc)
                {
                    br.Data = new { id = "", name = "sl_qc", value = "" };
                    br.Success = false;
                    br.Message.Add("商品期初数量已经存在 不允许修改");
                    br.Level = ErrorLevel.Warning;
                    return br;
                }
                if (data.Qc.je_qc != qcModel.je_qc)
                {
                    br.Data = new { id = "", name = "je_qc", value = "" };
                    br.Success = false;
                    br.Message.Add("商品期初金额已经存在 不允许修改");
                    br.Level = ErrorLevel.Warning;
                    return br;
                }
            }
            if (data.Qc == null && qcModel != null && qcModel.sl_qc != null && qcModel.sl_qc != 0)
            {
                updateQC = true;
            }
            #endregion

            #region 验证主商品是否需要修改
            #region 获取主商品修改的字段
            //获取主商品修改的字段
            ht.Clear();
            if (param.ContainsKey("barcode") && sp.barcode != param["barcode"].ToString())
            {
                ht.Add("new_barcode", param["barcode"].ToString());//条码
                ht.Add("barcode", sp.barcode);//条码
            }

            if (param.ContainsKey("bm") && sp.bm != param["bm"].ToString())
            {
                ht.Add("new_bm", param["bm"].ToString());//编码
                ht.Add("bm", sp.bm);//编码
            }

            if (param.ContainsKey("mc") && sp.mc != param["mc"].ToString())
            {
                ht.Add("new_mc", param["mc"].ToString());//名称
                ht.Add("mc", sp.mc);//名称
                ht.Add("new_zjm", CySoft.Utility.PinYin.GetChineseSpell(param["mc"].ToString()));//助记码
            }

            if (param.ContainsKey("cd") && sp.cd != param["cd"].ToString())
            {
                ht.Add("new_cd", param["cd"].ToString());//产地
                ht.Add("cd", sp.cd);//产地
            }

            if (param.ContainsKey("dj_jh") && !string.IsNullOrEmpty(param["dj_jh"].ToString()) && sp.dj_jh != decimal.Parse(param["dj_jh"].ToString()))
            {
                ht.Add("new_dj_jh", param["dj_jh"].ToString());//进货价
                ht.Add("dj_jh", sp.dj_jh);//进货价
            }

            if (param.ContainsKey("dj_ls") && !string.IsNullOrEmpty(param["dj_ls"].ToString()) && sp.dj_ls != decimal.Parse(param["dj_ls"].ToString()))
            {
                ht.Add("new_dj_ls", param["dj_ls"].ToString());//零售价
                ht.Add("dj_ls", sp.dj_ls);//零售价
            }

            if (param.ContainsKey("dw") && sp.dw != param["dw"].ToString())
            {
                ht.Add("new_dw", param["dw"].ToString());//单位
                ht.Add("dw", sp.dw);//单位
            }
            if (param.ContainsKey("sl_kc_min") && !string.IsNullOrEmpty(param["sl_kc_min"].ToString()) && sp.sl_kc_min != decimal.Parse(param["sl_kc_min"].ToString()))
            {
                ht.Add("new_sl_kc_min", param["sl_kc_min"].ToString());//最底库存量
                ht.Add("sl_kc_min", sp.sl_kc_min);//最底库存量
            }
            if (param.ContainsKey("sl_kc_max") && !string.IsNullOrEmpty(param["sl_kc_min"].ToString()) && sp.sl_kc_max != decimal.Parse(param["sl_kc_max"].ToString()))
            {
                ht.Add("new_sl_kc_max", param["sl_kc_max"].ToString());//最高库存量
                ht.Add("sl_kc_max", sp.sl_kc_max);//最高库存量
            }
            if (param.ContainsKey("pic_path") && sp.pic_path != param["pic_path"].ToString())
            {
                ht.Add("new_pic_path", param["pic_path"].ToString());//图片路径
                ht.Add("pic_path", sp.pic_path);//图片路径
            }

            if (param.ContainsKey("id_spfl") && sp.id_spfl != param["id_spfl"].ToString())
            {
                ht.Add("new_id_spfl", param["id_spfl"].ToString());//分类ID
                ht.Add("id_spfl_where", sp.id_spfl);//分类ID
            }
            if (param.ContainsKey("dj_hy") && !string.IsNullOrEmpty(param["dj_hy"].ToString()) && sp.dj_hy != decimal.Parse(param["dj_hy"].ToString()))
            {
                ht.Add("new_dj_hy", param["dj_hy"].ToString());//会员价
                ht.Add("dj_hy", sp.dj_hy);//会员价
            }
            if (param.ContainsKey("flag_czfs") && !string.IsNullOrEmpty(param["flag_czfs"].ToString()) && sp.flag_czfs != byte.Parse(param["flag_czfs"].ToString()))
            {
                ht.Add("new_flag_czfs", param["flag_czfs"].ToString());//计价方式
                ht.Add("flag_czfs", sp.flag_czfs);//计价方式
            }
            if (param.ContainsKey("dj_ps") && !string.IsNullOrEmpty(param["dj_ps"].ToString()) && sp.dj_ps != decimal.Parse(param["dj_ps"].ToString()))
            {
                ht.Add("new_dj_ps", param["dj_ps"].ToString());//配送价
                ht.Add("dj_ps", sp.dj_ps);//配送价
            }
            if (param.ContainsKey("dj_pf") && !string.IsNullOrEmpty(param["dj_pf"].ToString()) && sp.dj_pf != decimal.Parse(param["dj_pf"].ToString()))
            {
                ht.Add("new_dj_pf", param["dj_pf"].ToString());//批发价
                ht.Add("dj_pf", sp.dj_pf);//批发价
            }
            if (param.ContainsKey("yxq") && !string.IsNullOrEmpty(param["yxq"].ToString()) && sp.yxq != int.Parse(param["yxq"].ToString()))
            {
                ht.Add("new_yxq", param["yxq"].ToString());//有效期
                ht.Add("yxq", sp.yxq);//有效期
            }
            if (param.ContainsKey("id_gys") && sp.id_gys != param["id_gys"].ToString())
            {
                ht.Add("new_id_gys", param["id_gys"].ToString());//默认供应商
                ht.Add("id_gys", sp.id_gys);//默认供应商
            }
            if (param.ContainsKey("bz") && sp.bz != param["bz"].ToString())
            {
                ht.Add("new_bz", param["bz"].ToString());
                ht.Add("bz", sp.bz);
            }

            if (param.ContainsKey("flag_state") && sp.flag_state != byte.Parse(param["flag_state"].ToString()))
            {
                ht.Add("new_flag_state", param["flag_state"].ToString());
                ht.Add("flag_state", sp.flag_state);
            }

            #endregion
            if (ht.Keys.Count > 0)
            {
                updateZSP = true;
            }
            #endregion

            #region 验证多包装是否有变动
            //验证多包装是否有变动
            if (dbzList.Where(d => d.info_type == "add").Count() > 0)
                updateDBZ = true;
            foreach (var item in dbzList.Where(d => d.info_type == "edit"))
            {
                var oldItem = data.ShopSP_DBZ.Where(d => d.id == item.id).FirstOrDefault();
                if (oldItem == null)
                {
                    item.info_type = "add";
                    updateDBZ = true;
                    continue;
                }

                if (oldItem.barcode != item.barcode || oldItem.bm != item.bm || oldItem.mc != item.mc || oldItem.dw != item.dw || oldItem.zhl != item.zhl || oldItem.dj_jh != item.dj_jh || oldItem.dj_ls != item.dj_ls || oldItem.dj_ps != item.dj_ps || oldItem.dj_pf != item.dj_pf)
                {
                    updateDBZ = true;
                    item.need_edit = "1";
                }
            }
            #endregion

            #region 如果主商品 期初 多包装 都不需要更新 直接返回 不做处理
            if (!updateZSP && !updateQC && !updateDBZ)
            {
                br.Data = new { id = "", name = "", value = "" };
                br.Success = true;
                //br.Message.Add("操作失败 检测到商品未修改任何内容 ");
                br.Message.Add("操作成功 检测到商品未修改任何内容 ");
                return br;

            }
            #endregion

            if (updateZSP)
            {
                Hashtable htzsp = new Hashtable();
                #region 主商品信息有改动 修改主商品信息
                if (param.ContainsKey("id_user") && sp.id_edit != param["id_user"].ToString()) ht.Add("new_id_edit", param["id_user"].ToString());//修改人
                ht.Add("id_masteruser", sp.id_masteruser);
                //if (id_shop != id_shop_master || ht.ContainsKey("new_flag_state"))
                if (id_shop != id_shop_master)
                {
                    ht.Add("id", sp.id);
                    #region 验证条码是否被占用
                    if (ht.ContainsKey("new_barcode"))
                    {
                        if (allSpList.Where(d => d.barcode == ht["new_barcode"].ToString() && d.flag_delete == 0 && d.id != sp.id && d.id_shop == sp.id_shop).Count() > 0)
                        {
                            br.Data = new { id = "", name = "barcode", value = ht["new_barcode"].ToString() };
                            br.Message.Clear();
                            br.Message.Add(String.Format("提交的数据中 条码:{0} 已被占用!", ht["new_barcode"].ToString()));
                            br.Success = false;
                            return br;
                        }
                    }
                    #endregion
                }
                else
                {
                    ht.Add("id_sp", sp.id_sp);
                    if (!ht.ContainsKey("barcode"))
                        ht.Add("barcode", sp.barcode);
                    if (!ht.ContainsKey("bm"))
                        ht.Add("bm", sp.bm);
                    if (!ht.ContainsKey("dw"))
                        ht.Add("dw", sp.dw);
                    #region 验证条码是否被占用
                    if (ht.ContainsKey("new_barcode"))
                    {
                        if (allSpList.Where(d => d.barcode == ht["new_barcode"].ToString() && d.flag_delete == 0 && d.id_sp != sp.id_sp && d.bm != sp.bm && d.dw != sp.dw && d.barcode != sp.barcode).Count() > 0)
                        {
                            br.Data = new { id = "", name = "barcode", value = ht["new_barcode"].ToString() };
                            br.Message.Clear();
                            br.Message.Add(String.Format("提交的数据中 条码:{0} 已被占用!", ht["new_barcode"].ToString()));
                            br.Success = false;
                            return br;
                        }
                    }
                    #endregion
                }

                ht.Add("new_rq_edit", DateTime.Now);//修改日期

                if (DAL.UpdatePart(typeof(Tb_Shopsp), ht) <= 0)
                {
                    br.Data = new { id = "", name = "", value = "" };
                    br.Success = false;
                    br.Message.Clear();
                    br.Message.Add("更新主商品 数据库操作失败.");
                    throw new CySoftException(br);
                }
                #endregion
            }

            if (updateDBZ)
            {
                #region 如果包装数据存在新增数据
                if (dbzList.Where(d => d.info_type == "add").Count() > 0)
                {
                    #region 验证 处理多包装新增商品时检测到缺少门店信息数据
                    if (shopDistinct == null || shopDistinct.Count() <= 0)
                    {
                        br.Data = new { id = "", name = "", value = "" };
                        br.Success = false;
                        br.Message.Add(String.Format("处理多包装新增商品时检测到缺少门店信息数据 "));
                        throw new CySoftException(br);
                    }
                    #endregion

                    #region 获取该主用户 id_sp等于主商品的信息 后面门店取库存商品id时用到
                    //ht.Clear();
                    //ht.Add("id_sp", sp.id_sp);
                    //ht.Add("id_masteruser", sp.id_masteruser);
                    //ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                    //var shopsp_id_sp = (List<Tb_Shopsp>)this.GetAll(ht).Data ?? new List<Tb_Shopsp>();

                    var shopsp_id_sp = allSpList.Where(d => d.id_sp == sp.id_sp && d.flag_delete == (byte)Enums.FlagDelete.NoDelete).ToList() ?? new List<Tb_Shopsp>();

                    #endregion

                    #region 辅助数据
                    byte flag_czfs = 0;
                    byte.TryParse(param["flag_czfs"] == null ? "" : param["flag_czfs"].ToString(), out flag_czfs);
                    int yxq = 0;
                    int.TryParse(param["yxq"].ToString(), out yxq);
                    #endregion

                    #region 构建新增的数据List


                    List<string> rShopList = new List<string>();
                    //按用户管理门店新增 主门店拥有所有自门店的管理权限 shopshop关系表对应
                    foreach (var shop in shopDistinct)
                    {
                        var shopsp_model_id_sp = shopsp_id_sp.Where(d => d.id_shop == shop.Key).FirstOrDefault();
                        if (shopsp_model_id_sp == null || string.IsNullOrEmpty(shopsp_model_id_sp.id))
                            continue;
                        foreach (var item in dbzList.Where(d => d.info_type == "add"))
                        {
                            var id_sp = GetGuid;

                            #region 验证条码是否被占用
                            if (allSpList.Where(d => d.barcode == item.barcode && d.flag_delete == 0 && d.id_shop == shop.Key && d.id_masteruser == sp.id_masteruser).Count() > 0)
                            {
                                //如果是添加账户门店 则提示被占用 不然就排除
                                if (shop.Key == id_shop)
                                {
                                    //如果是自己门店重复 提示 如果是管理门店重复则不处理
                                    br.Success = false;
                                    br.Data = new { id = item.id, name = "barcode", value = item.barcode };
                                    br.Message.Clear();
                                    br.Message.Add(String.Format("提交的数据中 条码:{0} 已被占用!", item.barcode));
                                    throw new CySoftException(br);
                                }
                                else
                                {
                                    rShopList.Add(shop.Key);
                                }
                            }
                            #endregion

                            #region 如果 其他门店商品存在此条码的 取已存在的 id_sp
                            //如果 其他门店商品存在此条码的
                            Tb_Shopsp sameModel = null;//重复的商品
                            List<Tb_Shopsp> sameDBZList = new List<Tb_Shopsp>();
                            if (allSpList.Where(d => item.barcode == d.barcode).Count() > 0)
                            {
                                sameModel = allSpList.Where(d => item.barcode == d.barcode).FirstOrDefault();
                                sameDBZList = allSpList.Where(d => d.id_masteruser == sameModel.id_masteruser && d.id_kcsp == sameModel.id_kcsp).ToList();
                                if (sameDBZList.Where(d => d.barcode == item.barcode).Count() > 0)
                                {
                                    id_sp = sameDBZList.Where(d => d.barcode == item.barcode).FirstOrDefault().id_sp;
                                }
                            }
                            #endregion

                            #region 构建新增多包装Model
                            Tb_Shopsp modelDBZ = new Tb_Shopsp()
                            {
                                id_view = item.id,
                                id = GetGuid,
                                id_masteruser = id_masteruser,
                                id_shop = shop.Key,
                                id_sp = id_sp,
                                dw = item.dw,
                                bm = item.bm,
                                mc = item.mc,
                                id_kcsp = shopsp_model_id_sp.id_kcsp,
                                id_spfl = param["id_spfl"].ToString(),
                                barcode = item.barcode,
                                zjm = CySoft.Utility.PinYin.GetChineseSpell(item.mc),
                                zhl = item.zhl,
                                cd = param["cd"].ToString(),
                                flag_state = 1,
                                flag_czfs = 0,
                                dj_ls = item.dj_ls,
                                dj_jh = item.dj_jh,
                                sl_kc_min = decimal.Parse(param["sl_kc_min"].ToString()),
                                sl_kc_max = decimal.Parse(param["sl_kc_max"].ToString()),
                                bz = param["bz"] == null ? "" : param["bz"].ToString(),
                                pic_path = param["pic_path"] == null ? "" : param["pic_path"].ToString(),
                                id_create = param["id_user"].ToString(),
                                rq_create = DateTime.Now,
                                id_edit = param["id_user"].ToString(),
                                rq_edit = DateTime.Now,
                                flag_delete = 0,
                                dj_hy = item.dj_hy,
                                id_gys = param["id_gys"].ToString(),
                                yxq = yxq,
                                dj_ps = item.dj_ps,
                                dj_pf = item.dj_pf
                            };

                            if (!string.IsNullOrEmpty(item.pic_path))
                                modelDBZ.pic_path = item.pic_path;

                            #endregion
                            addList.Add(modelDBZ);
                            allSpList.Add(modelDBZ);
                        }
                    }
                    #endregion
                    addList = addList.Where(d => !rShopList.Contains(d.id_shop)).ToList();

                    //2017-02-15 add lz
                    foreach (var item in addList)
                    {
                        //如果存在id_sp的门店商品
                        var delModel = allShopSpList.Where(d => d.id_shop == item.id_shop && d.barcode == item.barcode).FirstOrDefault();
                        if (delModel != null && !string.IsNullOrEmpty(delModel.id))
                        {
                            var delModelMaster = allShopSpList.Where(d => d.id_kcsp == delModel.id_kcsp && d.id_sp == d.id_kcsp).FirstOrDefault();
                            if (delModelMaster.id_kcsp != sp.id_kcsp)
                            {
                                br.Success = false;
                                br.Data = new { id = item.id_view, name = "barcode", value = item.barcode };
                                br.Message.Clear();
                                br.Message.Add(String.Format("条码:{0} 已是{1}[{2}] 的多包装商品!", item.barcode, delModelMaster.mc, delModelMaster.barcode));
                                throw new CySoftException(br);
                            }
                            updateList.Add(item);
                        }
                    }

                    addList = addList.Where(d => !(from a in updateList select a.id).Contains(d.id)).ToList();

                    DAL.AddRange(addList);

                    //更新已删除的数据
                    #region 更新已存在的商品
                    foreach (var item_dbz in updateList)
                    {
                        var delModel = allShopSpList.Where(d => d.id_shop == item_dbz.id_shop && d.barcode == item_dbz.barcode).FirstOrDefault();
                        ht.Clear();
                        ht.Add("id_masteruser", param["id_masteruser"].ToString());
                        ht.Add("id", delModel.id);
                        #region 要更新的参数

                        ht.Add("new_flag_delete", "0");
                        if (delModel.barcode != item_dbz.barcode)
                        {
                            ht.Add("new_barcode", item_dbz.barcode);//条码
                        }

                        if (delModel.bm != item_dbz.bm)
                        {
                            ht.Add("new_bm", item_dbz.bm);//编码
                        }

                        if (delModel.mc != item_dbz.mc)
                        {
                            ht.Add("new_mc", item_dbz.mc);//名称
                            ht.Add("new_zjm", CySoft.Utility.PinYin.GetChineseSpell(item_dbz.mc.ToString()));//助记码
                        }

                        if (delModel.dw != item_dbz.dw)
                        {
                            ht.Add("new_dw", item_dbz.dw);//单位
                        }

                        if (delModel.zhl != item_dbz.zhl)
                        {
                            ht.Add("new_zhl", item_dbz.zhl);//转换率
                        }

                        if (delModel.dj_jh != item_dbz.dj_jh)
                        {
                            ht.Add("new_dj_jh", item_dbz.dj_jh);//进货价
                        }

                        if (delModel.dj_ls != item_dbz.dj_ls)
                        {
                            ht.Add("new_dj_ls", item_dbz.dj_ls);//零售价
                        }


                        if (delModel.dj_hy != item_dbz.dj_hy)
                        {
                            ht.Add("new_dj_hy", item_dbz.dj_hy);//会员价
                        }

                        if (delModel.dj_ps != item_dbz.dj_ps)
                        {
                            ht.Add("new_dj_ps", item_dbz.dj_ps);//配送价
                        }

                        if (delModel.dj_pf != item_dbz.dj_pf)
                        {
                            ht.Add("new_dj_pf", item_dbz.dj_pf);//批发价
                        }

                        ht.Add("new_rq_edit", DateTime.Now);//修改日期 
                        #endregion
                        if (DAL.UpdatePart(typeof(Tb_Shopsp), ht) <= 0)
                        {
                            br.Data = new { id = "", name = "", value = "" };
                            br.Success = false;
                            br.Message.Add("操作失败[更新数据库失败]");
                            throw new CySoftException(br);
                        }
                    }
                    #endregion
                }
                #endregion

                #region 如果包装数据存在更新数据
                if (dbzList.Where(d => d.info_type == "edit" && d.need_edit == "1").Count() > 0)
                {
                    foreach (var item in dbzList.Where(d => d.info_type == "edit" && d.need_edit == "1"))
                    {
                        #region 获取多包装商品 id 对应的 db数据
                        var oldItem = data.ShopSP_DBZ.Where(d => d.id == item.id).FirstOrDefault();
                        if (oldItem == null)
                            continue;
                        #endregion

                        #region 获取多包装商品修改的字段
                        ht.Clear();
                        if (item.barcode != oldItem.barcode)
                        {
                            ht.Add("new_barcode", item.barcode);//条码
                            ht.Add("barcode", oldItem.barcode);
                        }

                        if (item.bm != oldItem.bm)
                        {
                            ht.Add("new_bm", item.bm);//编码
                            ht.Add("bm", oldItem.bm);
                        }

                        if (item.mc != oldItem.mc)
                        {
                            ht.Add("new_mc", item.mc);//名称
                            ht.Add("mc", oldItem.mc);
                            ht.Add("new_zjm", CySoft.Utility.PinYin.GetChineseSpell(item.mc));//助记码
                        }

                        if (item.dw != oldItem.dw)
                        {
                            ht.Add("new_dw", item.dw);//单位
                            ht.Add("dw", oldItem.dw);
                        }

                        if (item.zhl != oldItem.zhl)
                        {
                            ht.Add("new_zhl", item.zhl);//转换率
                            ht.Add("zhl", oldItem.zhl);
                        }

                        if (item.dj_jh != oldItem.dj_jh)
                        {
                            ht.Add("new_dj_jh", item.dj_jh);//进货价
                            ht.Add("dj_jh", oldItem.dj_jh);
                        }

                        if (item.dj_ls != oldItem.dj_ls)
                        {
                            ht.Add("new_dj_ls", item.dj_ls);//零售价
                            ht.Add("dj_ls", oldItem.dj_ls);
                        }

                        if (item.dj_hy != oldItem.dj_hy)
                        {
                            ht.Add("new_dj_hy", item.dj_hy);//会员价
                            ht.Add("dj_hy", oldItem.dj_hy);
                        }

                        if (item.dj_ps != oldItem.dj_ps)
                        {
                            ht.Add("new_dj_ps", item.dj_ps);//配送价
                            ht.Add("dj_ps", oldItem.dj_ps);
                        }

                        if (item.dj_pf != oldItem.dj_pf)
                        {
                            ht.Add("new_dj_pf", item.dj_pf);//批发价
                            ht.Add("dj_pf", oldItem.dj_pf);
                        }
                        #endregion

                        #region 多包装商品信息如果有改动 修改多包装商品信息
                        if (ht.Keys.Count <= 0)
                            continue;
                        else
                        {
                            #region 执行更新
                            if (param.ContainsKey("id_user") && oldItem.id_edit != param["id_user"].ToString()) ht.Add("new_id_edit", param["id_user"].ToString());//修改人
                            ht.Add("id_masteruser", id_masteruser);
                            if (id_shop != id_shop_master)
                            {
                                ht.Add("id", oldItem.id);
                                #region 验证条码是否被占用
                                if (ht.ContainsKey("new_barcode"))
                                {
                                    if (allSpList.Where(d => d.barcode == ht["new_barcode"].ToString() && d.flag_delete == 0 && d.id != oldItem.id && d.id_shop == oldItem.id_shop).Count() > 0)
                                    {
                                        br.Data = new { id = item.id, name = "barcode", value = ht["new_barcode"].ToString() };
                                        br.Message.Clear();
                                        br.Message.Add(String.Format("提交的数据中 条码:{0} 已被占用!", ht["new_barcode"].ToString()));
                                        br.Success = false;
                                        return br;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                ht.Add("id_sp", oldItem.id_sp);
                                if (!ht.ContainsKey("barcode"))
                                    ht.Add("barcode", oldItem.barcode);
                                if (!ht.ContainsKey("bm"))
                                    ht.Add("bm", oldItem.bm);
                                if (!ht.ContainsKey("dw"))
                                    ht.Add("dw", oldItem.dw);
                                #region 验证条码是否被占用
                                if (ht.ContainsKey("new_barcode"))
                                {
                                    if (allSpList.Where(d => d.barcode == ht["new_barcode"].ToString() && d.flag_delete == 0 && d.id_sp != oldItem.id_sp && d.bm != oldItem.bm && d.dw != oldItem.dw && d.barcode != oldItem.barcode).Count() > 0)
                                    {
                                        br.Data = new { id = item.id, name = "barcode", value = ht["new_barcode"].ToString() };
                                        br.Message.Clear();
                                        br.Message.Add(String.Format("提交的数据中 条码:{0} 已被占用!", ht["new_barcode"].ToString()));
                                        br.Success = false;
                                        return br;
                                    }
                                }
                                #endregion
                            }

                            ht.Add("new_rq_edit", DateTime.Now);//修改日期
                            if (DAL.UpdatePart(typeof(Tb_Shopsp), ht) <= 0)
                            {
                                br.Data = new { id = "", name = "", value = "" };
                                br.Success = false;
                                br.Message.Add("多包装商品 更新操作失败.");
                                throw new CySoftException(br);
                            }
                            else
                            {
                                br.Data = new { id = "", name = "", value = "" };
                                br.Success = true;
                                br.Message.Add("更新操作成功.");
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
                #endregion
            }

            if (updateQC)
            {
                #region 更新期初库存等操作
                if (qcModel != null && qcModel.sl_qc != null && qcModel.sl_qc != 0)
                {
                    Td_Sp_Qc addQCModel = new Td_Sp_Qc();
                    addQCModel.id_masteruser = param["id_masteruser"].ToString();
                    addQCModel.id = Guid.NewGuid().ToString();
                    addQCModel.id_shop = data.ShopSP.id_shop;
                    addQCModel.id_shopsp = data.ShopSP.id_kcsp;
                    addQCModel.je_qc = qcModel.je_qc;
                    addQCModel.sl_qc = qcModel.sl_qc;
                    addQCModel.dj_cb = (qcModel.je_qc / qcModel.sl_qc);
                    addQCModel.id_kcsp = data.ShopSP.id_kcsp;
                    DAL.Add(addQCModel);

                    Tz_Sp_Kc addKCModel = new Tz_Sp_Kc();
                    addKCModel.id_masteruser = param["id_masteruser"].ToString();
                    addKCModel.id = Guid.NewGuid().ToString();
                    addKCModel.id_shop = addQCModel.id_shop;
                    addKCModel.id_kcsp = addQCModel.id_kcsp;
                    addKCModel.sl_qm = addQCModel.sl_qc;
                    addKCModel.je_qm = addQCModel.je_qc;
                    addKCModel.dj_cb = (addKCModel.je_qm / addKCModel.sl_qm);
                    DAL.Add(addKCModel);


                    Tz_Jxc_Flow flowModel = new Tz_Jxc_Flow();
                    flowModel.id_masteruser = param["id_masteruser"].ToString();
                    flowModel.id = Guid.NewGuid().ToString();
                    flowModel.id_billmx = "qc";
                    flowModel.bm_djlx = "qc";
                    flowModel.id_shop = addQCModel.id_shop;
                    flowModel.id_shopsp = addQCModel.id_shopsp;
                    flowModel.sl = addQCModel.sl_qc;
                    flowModel.je = addQCModel.je_qc;
                    flowModel.rq = DateTime.Now;
                    flowModel.id_kcsp = addQCModel.id_kcsp;
                    DAL.Add(flowModel);

                }
                #endregion
            }

            #region 检验库存账本
            //检验库存账本
            ht.Clear();
            ht.Add("proname", "p_rzh_jykc");
            ht.Add("errorid", "-1");
            ht.Add("errormessage", "未知错误！");
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("id_shop", "0");
            DAL.RunProcedure2(ht);

            if (!ht.ContainsKey("errorid") || !ht.ContainsKey("errormessage"))
            {
                br.Success = false;
                br.Message.Add("检验库存账本失败,执行检验库存账本出现异常!");
                throw new CySoftException(br);
            }

            if (!string.IsNullOrEmpty(ht["errorid"].ToString()) || !string.IsNullOrEmpty(ht["errormessage"].ToString()))
            {
                br.Success = false;
                br.Message.Add(ht["errormessage"].ToString());
                throw new CySoftException(br);
            }

            #endregion

            //}
            //catch (Exception ex)
            //{
            //    br.Data = new { id = "", name = "", value = "" };
            //    br.Success = false;
            //    br.Message.Add("更新操作异常!");
            //    throw new CySoftException(br);
            //}

            br.Data = new { id = "", name = "", value = "" };
            br.Success = true;
            br.Message.Add("更新操作成功.");
            return br;
            #endregion
        }


        public BaseResult SaveWork(Hashtable param)
        {
            #region 定义参数
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            #endregion

            #region 获取商品原信息
            ht.Add("id", param["id"].ToString());
            var sp = DAL.GetItem<Tb_Shopsp>(typeof(Tb_Shopsp), ht);
            if (sp == null)
            {
                br.Data = new { id = "", name = "", value = "" };
                br.Success = false;
                br.Message.Add("商品不存在");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            var id_shop = param["id_shop"].ToString();
            var id_shop_master = param["id_shop_master"].ToString();
            var id_masteruser = param["id_masteruser"].ToString();
            #endregion


            #region 更新操作
            try
            {
                //更新商品
                ht.Clear();
                if (param.ContainsKey("barcode") && sp.barcode != param["barcode"].ToString())
                {
                    ht.Add("new_barcode", param["barcode"].ToString());//条码
                    ht.Add("barcode", sp.barcode);//条码
                }

                if (param.ContainsKey("bm") && sp.bm != param["bm"].ToString())
                {
                    ht.Add("new_bm", param["bm"].ToString());//编码
                    ht.Add("bm", sp.bm);//编码
                }

                if (param.ContainsKey("mc") && sp.mc != param["mc"].ToString())
                {
                    ht.Add("new_mc", param["mc"].ToString());//名称
                    ht.Add("mc", sp.mc);//名称
                    ht.Add("new_zjm", CySoft.Utility.PinYin.GetChineseSpell(param["mc"].ToString()));//助记码
                }

                if (param.ContainsKey("cd") && sp.cd != param["cd"].ToString())
                {
                    ht.Add("new_cd", param["cd"].ToString());//产地
                    ht.Add("cd", sp.cd);//产地
                }

                if (param.ContainsKey("dj_jh") && !string.IsNullOrEmpty(param["dj_jh"].ToString()) && sp.dj_jh != decimal.Parse(param["dj_jh"].ToString()))
                {
                    ht.Add("new_dj_jh", param["dj_jh"].ToString());//进货价
                    ht.Add("dj_jh", sp.dj_jh);//进货价
                }

                if (param.ContainsKey("dj_ls") && !string.IsNullOrEmpty(param["dj_ls"].ToString()) && sp.dj_ls != decimal.Parse(param["dj_ls"].ToString()))
                {
                    ht.Add("new_dj_ls", param["dj_ls"].ToString());//零售价
                    ht.Add("dj_ls", sp.dj_ls);//零售价
                }

                if (param.ContainsKey("dw") && sp.dw != param["dw"].ToString())
                {
                    ht.Add("new_dw", param["dw"].ToString());//单位
                    ht.Add("dw", sp.dw);//单位
                }
                if (param.ContainsKey("sl_kc_min") && !string.IsNullOrEmpty(param["sl_kc_min"].ToString()) && sp.sl_kc_min != decimal.Parse(param["sl_kc_min"].ToString()))
                {
                    ht.Add("new_sl_kc_min", param["sl_kc_min"].ToString());//最底库存量
                    ht.Add("sl_kc_min", sp.sl_kc_min);//最底库存量
                }
                if (param.ContainsKey("sl_kc_max") && !string.IsNullOrEmpty(param["sl_kc_min"].ToString()) && sp.sl_kc_max != decimal.Parse(param["sl_kc_max"].ToString()))
                {
                    ht.Add("new_sl_kc_max", param["sl_kc_max"].ToString());//最高库存量
                    ht.Add("sl_kc_max", sp.sl_kc_max);//最高库存量
                }
                if (param.ContainsKey("pic_path") && sp.pic_path != param["pic_path"].ToString())
                {
                    ht.Add("new_pic_path", param["pic_path"].ToString());//图片路径
                    ht.Add("pic_path", sp.pic_path);//图片路径
                }

                if (param.ContainsKey("id_spfl") && sp.id_spfl != param["id_spfl"].ToString())
                {
                    ht.Add("new_id_spfl", param["id_spfl"].ToString());//分类ID
                    ht.Add("id_spfl", sp.id_spfl);//分类ID
                }
                if (param.ContainsKey("dj_hy") && !string.IsNullOrEmpty(param["dj_hy"].ToString()) && sp.dj_hy != decimal.Parse(param["dj_hy"].ToString()))
                {
                    ht.Add("new_dj_hy", param["dj_hy"].ToString());//会员价
                    ht.Add("dj_hy", sp.dj_hy);//会员价
                }
                if (param.ContainsKey("flag_czfs") && !string.IsNullOrEmpty(param["flag_czfs"].ToString()) && sp.flag_czfs != byte.Parse(param["flag_czfs"].ToString()))
                {
                    ht.Add("new_flag_czfs", param["flag_czfs"].ToString());//计价方式
                    ht.Add("flag_czfs", sp.flag_czfs);//计价方式
                }
                if (param.ContainsKey("dj_ps") && !string.IsNullOrEmpty(param["dj_ps"].ToString()) && sp.dj_ps != decimal.Parse(param["dj_ps"].ToString()))
                {
                    ht.Add("new_dj_ps", param["dj_ps"].ToString());//配送价
                    ht.Add("dj_ps", sp.dj_ps);//配送价
                }
                if (param.ContainsKey("dj_pf") && !string.IsNullOrEmpty(param["dj_pf"].ToString()) && sp.dj_pf != decimal.Parse(param["dj_pf"].ToString()))
                {
                    ht.Add("new_dj_pf", param["dj_pf"].ToString());//批发价
                    ht.Add("dj_pf", sp.dj_pf);//批发价
                }
                if (param.ContainsKey("yxq") && !string.IsNullOrEmpty(param["yxq"].ToString()) && sp.yxq != int.Parse(param["yxq"].ToString()))
                {
                    ht.Add("new_yxq", param["yxq"].ToString());//有效期
                    ht.Add("yxq", sp.yxq);//有效期
                }
                if (param.ContainsKey("id_gys") && sp.id_gys != param["id_gys"].ToString())
                {
                    ht.Add("new_id_gys", param["id_gys"].ToString());//默认供应商
                    ht.Add("id_gys", sp.id_gys);//默认供应商
                }
                if (param.ContainsKey("bz") && sp.bz != param["bz"].ToString())
                {
                    ht.Add("new_bz", param["bz"].ToString());
                    ht.Add("bz", sp.bz);
                }

                if (ht.Keys.Count <= 0)
                {
                    br.Data = new { id = "", name = "", value = "" };
                    br.Success = false;
                    br.Message.Add("操作失败 检测到未修改任何内容 ");
                    return br;
                }

                if (param.ContainsKey("id_user") && sp.id_edit != param["id_user"].ToString()) ht.Add("new_id_edit", param["id_user"].ToString());//修改人
                ht.Add("id_masteruser", sp.id_masteruser);
                if (id_shop != id_shop_master)
                {
                    ht.Add("id", sp.id);
                }
                else
                {
                    ht.Add("id_sp", sp.id_sp);
                    if (!ht.ContainsKey("dw"))
                        ht.Add("dw", sp.dw);
                }
                ht.Add("new_rq_edit", DateTime.Now);//修改日期

                if (DAL.UpdatePart(typeof(Tb_Shopsp), ht) <= 0)
                {
                    br.Data = new { id = "", name = "", value = "" };
                    br.Success = false;
                    br.Message.Add("更新操作失败.");

                }
                else
                {
                    br.Data = new { id = "", name = "", value = "" };
                    br.Success = true;
                    br.Message.Add("更新操作成功.");
                }

            }
            catch (Exception ex)
            {
                br.Data = new { id = "", name = "", value = "" };
                br.Success = false;
                br.Message.Add("更新操作异常!");

            }

            return br;
            #endregion
        }



        /// <summary>
        /// 批量修改商品
        /// lz 2016-08-04
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseResult BatchSave(Hashtable param)
        {
            #region 获取参数
            BaseResult br = new BaseResult();
            if (param == null
                || param.Count < 3
                || !param.ContainsKey("spIDs")
                || !param.ContainsKey("id_user")
                 || !param.ContainsKey("id_shop_master")
                  || !param.ContainsKey("id_shop")
                || (!param.ContainsKey("flag_state")
                && !param.ContainsKey("sl_kc_min")
                && !param.ContainsKey("sl_kc_max")
                && !param.ContainsKey("id_spfl")

                && !param.ContainsKey("dj_ls")
                && !param.ContainsKey("dj_jh")
                && !param.ContainsKey("dj_hy")
                && !param.ContainsKey("dj_ps")
                && !param.ContainsKey("dj_pf")

                )
                )
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("批量修改商品参数错误.");
                return br;
            }
            #endregion

            #region 遍历处理
            var arr = param["spIDs"].ToString().Split('|');
            if (arr == null || arr.Length != 2 || string.IsNullOrEmpty(arr[0].ToString()) || string.IsNullOrEmpty(arr[1].ToString()))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("批量修改商品参数错误.");
                return br;
            }

            if (param.ContainsKey("sl_kc_min") && param.ContainsKey("sl_kc_max") && CyVerify.IsNumeric(param["sl_kc_min"].ToString()) && CyVerify.IsNumeric(param["sl_kc_max"].ToString()))
            {
                if (decimal.Parse(param["sl_kc_min"].ToString()) > decimal.Parse(param["sl_kc_max"].ToString()))
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    br.Message.Add("最低库存不能大于最高库存.");
                    return br;
                }
            }



            string[] spIDs = arr[0].ToString().Split(',');
            string[] id_spIDs = arr[1].ToString().Split(',');
            Hashtable ht = new Hashtable();
            ht.Clear();

            if (param["id_shop"].ToString() != param["id_shop_master"].ToString())
            {
                ht.Add("idList", spIDs.ToArray());
            }
            else
            {
                ht.Add("id_spList", id_spIDs.ToArray());
            }

            ht.Add("new_id_edit", param["id_user"].ToString());
            ht.Add("new_rq_edit", DateTime.Now);
            if (param.ContainsKey("flag_state")) ht.Add("new_flag_state", param["flag_state"].ToString());
            if (param.ContainsKey("sl_kc_min")) ht.Add("new_sl_kc_min", param["sl_kc_min"].ToString());
            if (param.ContainsKey("sl_kc_max")) ht.Add("new_sl_kc_max", param["sl_kc_max"].ToString());
            if (param.ContainsKey("id_spfl")) ht.Add("new_id_spfl", param["id_spfl"].ToString());

            if (param.ContainsKey("dj_ls")) ht.Add("new_dj_ls", param["dj_ls"].ToString());
            if (param.ContainsKey("dj_jh")) ht.Add("new_dj_jh", param["dj_jh"].ToString());
            if (param.ContainsKey("dj_hy")) ht.Add("new_dj_hy", param["dj_hy"].ToString());
            if (param.ContainsKey("dj_ps")) ht.Add("new_dj_ps", param["dj_ps"].ToString());
            if (param.ContainsKey("dj_pf")) ht.Add("new_dj_pf", param["dj_pf"].ToString());


            if (DAL.UpdatePart(typeof(Tb_Shopsp), ht) > 0)
            {
                br.Success = true;
                br.Message.Clear();
                br.Message.Add(string.Format("操作成功!"));
            }
            else
            {
                br.Success = false;
                br.Message.Clear();
                br.Message.Add(string.Format("更新数据库失败!"));

            }



            #endregion


            return br;
        }

        /// <summary>
        /// 批量修改商品单例模式
        /// lz 2016-08-04
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private BaseResult BatchSaveOne(Hashtable param)
        {
            #region 验证参数
            BaseResult br = new BaseResult();
            if (param == null
                || param.Count == 0
                || !param.ContainsKey("id")
                || string.IsNullOrWhiteSpace(param["id"].ToString())
                 || !param.ContainsKey("id_user")
                 || (!param.ContainsKey("flag_state") && !param.ContainsKey("sl_kc_min") && !param.ContainsKey("sl_kc_max"))
                )
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("删除商品参数错误.");
                return br;
            }
            #endregion

            #region 获取商品
            Hashtable ht = new Hashtable();
            ht.Add("id", param["id"].ToString());
            var sp = DAL.GetItem<Tb_Shopsp>(typeof(Tb_Shopsp), ht);
            if (sp == null)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("商品不存在,ID为:{0}", param["id"].ToString()));
                return br;
            }
            br.Success = true;
            br.Message.Add("商品修改成功.");
            #endregion

            #region 执行修改并返回结果
            ht.Clear();
            ht.Add("id", param["id"].ToString());
            ht.Add("new_id_edit", param["id_user"].ToString());
            ht.Add("new_rq_edit", DateTime.Now);
            if (param.ContainsKey("flag_state")) ht.Add("new_flag_state", param["flag_state"].ToString());
            if (param.ContainsKey("sl_kc_min")) ht.Add("new_sl_kc_min", param["sl_kc_min"].ToString());
            if (param.ContainsKey("sl_kc_max")) ht.Add("new_sl_kc_max", param["sl_kc_max"].ToString());
            int count = DAL.UpdatePart(typeof(Tb_Shopsp), ht);
            if (count <= 0)
            {
                br.Success = false;
                br.Message.Add(string.Format("商品[{0}]修改操作失败！", sp.mc));
            }
            return br;
            #endregion
        }

        #endregion

        #region 商品删除
        /// <summary>
        /// 商品删除
        /// lz 2016-08-03
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            #region 获取参数
            BaseResult br = new BaseResult();
            if (param == null
                || param.Count == 0
                || !param.ContainsKey("spIDs")
                || !param.ContainsKey("id_user")
                || !param.ContainsKey("id_masteruser")
                )
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("商品删除参数错误.");
                return br;
            }
            #endregion

            #region 遍历处理
            string[] spIDs = param["spIDs"].ToString().Split(',');

            Hashtable ht = new Hashtable();
            foreach (var p in spIDs)
            {
                ht.Clear();
                ht.Add("id", p);
                ht.Add("id_user", param["id_user"]);
                ht.Add("id_masteruser", param["id_masteruser"]);

                var rs = DeleteOne(ht);
                if (!rs.Success) br.Message.Add(rs.Message[0]);
            }
            #endregion

            #region 返回结果

            if (spIDs.Length.Equals(br.Message.Count))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Insert(0, "商品删除不成功，因为以下原因：");
            }
            else if (br.Message.Count > 0)
            {
                br.Success = true;
                br.Level = ErrorLevel.Question;
                br.Message.Insert(0, "商品删除部分成功，未删除的商品存在以下原因：");
            }
            else
            {
                br.Success = true;
                br.Message.Add("商品删除成功.");
            }
            #endregion

            return br;
        }

        /// <summary>
        /// 商品删除单例模式
        /// lz 2016-08-03
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private BaseResult DeleteOne(Hashtable param)
        {
            #region 验证参数
            BaseResult br = new BaseResult();
            if (param == null
                || param.Count == 0
                || !param.ContainsKey("id")
                || string.IsNullOrWhiteSpace(param["id"].ToString())
                 || !param.ContainsKey("id_user")
                || !param.ContainsKey("id_masteruser")

                )
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("删除商品参数错误.");
                return br;
            }
            #endregion

            #region 获取商品
            string id = param["id"].ToString();
            string id_user = param["id_user"].ToString();
            string id_masteruser = param["id_masteruser"].ToString();

            Hashtable ht = new Hashtable();
            ht.Add("id", id);
            ht.Add("id_masteruser", id_masteruser);

            var sp = DAL.GetItem<Tb_Shopsp>(typeof(Tb_Shopsp), ht);

            if (sp == null)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("要删除的商品不存在,ID为:{0}", id));
                return br;
            }

            #endregion

            #region 验证库存
            ht.Clear();
            ht.Add("id_kcsp", sp.id_kcsp);
            ht.Add("id_shop", sp.id_shop);
            // var kcModel= DAL.QueryList<Tz_Sp_Kc>(typeof(Tz_Sp_Kc), ht).FirstOrDefault();
            var kcModel = DAL.GetItem<Tz_Sp_Kc>(typeof(Tz_Sp_Kc), ht);
            if (kcModel != null && !string.IsNullOrEmpty(kcModel.id) && kcModel.sl_qm != 0)
            {
                br.Message.Clear();
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("商品[{0}]有库存，无法删除！", sp.mc));
                return br;
            }

            ht.Clear();
            ht.Add("id_shopsp", sp.id);
            int lsCount = DAL.GetCount(typeof(Td_Ls_2), ht);
            if (lsCount > 0)
            {
                br.Message.Clear();
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("商品[{0}]有在零售表存在数据，不允许删除！", sp.mc));
                return br;
            }

            #endregion

            br.Success = true;
            br.Message.Add("商品删除成功.");

            #region 执行删除并返回结果
            ht.Clear();
            ht.Add("id", id);
            ht.Add("new_id_edit", id_user);
            ht.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);
            ht.Add("new_rq_edit", DateTime.Now);

            var count = DAL.UpdatePart(typeof(Tb_Shopsp), ht);
            if (count <= 0)
            {
                br.Success = false;
                br.Message.Add(string.Format("商品[{0}]删除操作失败！", sp.mc));
            }

            #region 备注
            //if (db_count.Equals(0))
            //{
            //    ht.Clear();
            //    ht.Add("id", id);
            //    ht.Add("new_id_edit", id_user);
            //    ht.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);
            //    ht.Add("new_rq_edit", DateTime.Now);

            //    var count = DAL.UpdatePart(typeof(Tb_Shopsp), ht);
            //    if (count <= 0)
            //    {
            //        br.Success = false;
            //        br.Message.Add(string.Format("商品[{0}]删除操作失败！", sp.mc));
            //    }
            //}
            //else
            //{
            //    br.Message.Clear();
            //    br.Success = false;
            //    br.Level = ErrorLevel.Warning;
            //    br.Message.Add(string.Format("商品[{0}]有库存，无法删除！", sp.mc));
            //} 
            #endregion

            #endregion
            return br;
        }
        #endregion

        #region 商品复制
        /// <summary>
        /// 商品复制
        /// lz 2016-08-04
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseResult Copy(Hashtable param)
        {
            #region 获取参数
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            if (param == null
                || param.Count == 0
                || !param.ContainsKey("id")
                || string.IsNullOrWhiteSpace(param["id"].ToString())
                || !param.ContainsKey("id_masteruser")
                || !param.ContainsKey("id_user")
                )
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("商品复制参数错误.");
                return br;
            }
            #endregion

            #region 获取商品原信息
            ht.Add("id", param["id"].ToString());
            var sp = DAL.GetItem<Tb_Shopsp>(typeof(Tb_Shopsp), ht);
            if (sp == null) { br.Success = false; br.Message.Add("商品不存在"); br.Level = ErrorLevel.Warning; return br; }
            #endregion

            #region 执行复制操作并返回结果

            sp.id = Guid.NewGuid().ToString();
            //sp.dw_xh = 1;
            sp.id_masteruser = param["id_masteruser"].ToString();
            //sp.id_shop = param["id_shop"].ToString();
            sp.id_kcsp = sp.id;
            sp.id_create = param["id_user"].ToString();
            sp.rq_create = DateTime.Now;
            sp.id_edit = param["id_user"].ToString();
            sp.rq_edit = DateTime.Now;

            DAL.Add(sp);

            br.Success = true;
            br.Message.Add("商品复制成功.");
            br.Data = sp;
            return br;
            #endregion
        }
        #endregion

        #region 商品导入
        /// <summary>
        /// 商品导入
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public BaseResult ImportIn(Hashtable param)
        {
            #region 获取数据
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            string FilePath = param["filePath"].ToString();
            string id_masteruser = param["id_masteruser"].ToString();
            string id_user = param["id_user"].ToString();
            string id_shop = param["id_shop"].ToString();
            var digitHashtable = (Hashtable)param["digitHashtable"];

            List<Tb_Shopsp_Import> list = (List<Tb_Shopsp_Import>)param["list"];
            List<Tb_Shopsp_Import> successList = new List<Tb_Shopsp_Import>();
            List<Tb_Shopsp_Import> failList = new List<Tb_Shopsp_Import>();

            List<Tb_Shopsp> addSPList = new List<Tb_Shopsp>();
            List<Td_Sp_Qc> addQCList = new List<Td_Sp_Qc>();
            List<Tz_Sp_Kc> addKCList = new List<Tz_Sp_Kc>();
            List<Tz_Jxc_Flow> addFlowList = new List<Tz_Jxc_Flow>();
            List<Tb_Dw> addDWList = new List<Tb_Dw>();
            List<Tb_Spfl> addSPFLList = new List<Tb_Spfl>();
            List<Tb_Gys> addGYSList = new List<Tb_Gys>();
            #endregion
            #region 获取单位List
            ht.Add("", "");
            var dbDWList = DAL.QueryList<Tb_Dw>(typeof(Tb_Dw), ht);
            #endregion
            #region 获取管理店铺List
            var shopList = (List<Tb_User_ShopWithShopMc>)param["shop_shop"] ?? new List<Tb_User_ShopWithShopMc>();
            var q = from p in shopList group p by p.id_shop into g select new { g.Key };

            if (q == null || q.Count() <= 0)
            {
                br.Success = false;
                br.Message.Add(String.Format("缺少门店信息数据"));
                return br;
            }
            #endregion
            #region 获取商品分类List
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("flag_delete", 0);
            var spflList = DAL.QueryList<Tb_Spfl>(typeof(Tb_Spfl), ht);
            #endregion
            #region 获取所有商品
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            //ht.Add("flag_delete", 0);
            var allSpList_All = DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), ht);
            var allSpList = allSpList_All.Where(d=>d.flag_delete==0).ToList();
            #endregion
            #region 获取供应商List
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("flag_delete", 0);
            var gysList = DAL.QueryList<Tb_Gys_User_QueryModel>(typeof(Tb_Gys), ht);
            #endregion

            #region 获取供应商List
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("flag_delete", 0);
            var gysflList = DAL.QueryList<Tb_Gysfl>(typeof(Tb_Gysfl), ht);
            if (gysflList == null || gysflList.Count() <= 0)
            {
                var gysflModel = new Tb_Gysfl()
                {
                    id_masteruser = param["id_masteruser"].ToString(),
                    id = GetGuid,
                    bm = "默认",
                    mc = "默认",
                    path = "0",
                    id_farther = "0",
                    sort_id = 0,
                    id_create = id_user,
                    rq_create = DateTime.Now,
                    flag_delete = 0
                };
                gysflModel.path = string.Format("/0/{0}", gysflModel.id);
                DAL.Add(gysflModel);
                gysflList.Add(gysflModel);
            }
            #endregion

            #region 数据处理
            foreach (var item in list)
            {
                br = this.CheckImportInfo(item, digitHashtable);
                if (!br.Success)
                {
                    item.bz = br.Message.Count > 0 ? br.Message[0].ToString() : "数据不符合要求";
                    failList.Add(item);
                }
                else
                {
                    #region 验证条码
                    if (list.Where(d => d.barcode == item.barcode).Count() >= 2)
                    {
                        item.bz = String.Format("导入数据 条码:{0}重复!", item.barcode);
                        failList.Add(item);
                        continue;
                    }

                    if (list.Where(d => d.bm == item.bm).Count() >= 2)
                    {
                        item.bz = String.Format("导入数据 编码:{0}重复!", item.bm);
                        failList.Add(item);
                        continue;
                    }

                    //if (allSpList.Where(d => (from shop in q select shop.Key).ToArray().Contains(d.id_shop) && d.barcode == item.barcode).Count() > 0)
                    //{
                    //    item.bz = String.Format("导入数据 条码:{0} 已被占用!", item.barcode);
                    //    failList.Add(item);
                    //    continue;
                    //}

                    if (allSpList.Where(d => d.id_shop == id_shop && d.barcode == item.barcode).Count() > 0)
                    {
                        item.bz = String.Format("导入数据 条码:{0} 已被占用!", item.barcode);
                        failList.Add(item);
                        continue;
                    }
                    #endregion

                    #region 遍历门店插入商品/期初/库存表
                    string zID = Guid.NewGuid().ToString();
                    string temp_id_shop = id_shop;
                    if (allSpList.Where(d => (from shop in q select shop.Key).ToArray().Contains(d.id_shop) && d.barcode == item.barcode).Count() > 0)
                    {
                        var tempModel = allSpList.Where(d => (from shop in q select shop.Key).ToArray().Contains(d.id_shop) && d.barcode == item.barcode).FirstOrDefault();
                        zID = tempModel.id_kcsp;
                        temp_id_shop = tempModel.id_shop;
                    }

                    string id_shopsp = zID;
                    string id_sp = zID;

                    foreach (var shopitem in q)
                    {
                        if (allSpList.Where(d => d.id_shop == shopitem.Key && d.barcode == item.barcode).Count() > 0)
                        {
                            continue;
                        }

                        #region Tb_Shopsp
                        byte flag_czfs = 0;

                        if (shopitem.Key == temp_id_shop)
                        {
                            id_shopsp = zID;
                        }
                        else
                        {
                            id_shopsp = Guid.NewGuid().ToString();
                        }

                        #region 商品分类 如果不存在 则添加一条
                        string idspfl = Guid.NewGuid().ToString();
                        if (!string.IsNullOrEmpty(item.id_spfl) && spflList != null && spflList.Where(d => d.mc == item.id_spfl && d.flag_delete == 0).Count() > 0)
                        {
                            idspfl = spflList.Where(d => d.mc == item.id_spfl && d.flag_delete == 0).FirstOrDefault().id;
                        }
                        else
                        {
                            Tb_Spfl spflModel = new Tb_Spfl()
                            {
                                id_masteruser = id_masteruser,
                                id = idspfl,
                                bm = DateTime.Now.ToString("yyyyMMddHHmmss"),
                                mc = item.id_spfl,
                                path = "/0/" + idspfl,
                                id_father = "0",
                                id_create = id_user,
                                rq_create = DateTime.Now,
                                id_edit = id_user,
                                rq_edit = DateTime.Now,
                                flag_delete = 0,
                                sort_id = 0
                            };
                            spflList.Add(spflModel);
                            addSPFLList.Add(spflModel);
                        }
                        #endregion

                        #region id_gys
                        string id_gys = "";
                        if (!string.IsNullOrEmpty(item.id_gys) && gysList != null && gysList.Where(d => d.mc == item.id_gys && d.flag_delete == 0).Count() > 0)
                        {
                            id_gys = gysList.Where(d => d.mc == item.id_gys && d.flag_delete == 0).FirstOrDefault().id;
                        }
                        else
                        {
                            Tb_Gys gysModel = new Tb_Gys();
                            gysModel.id_masteruser = id_masteruser;
                            gysModel.id = GetGuid;
                            gysModel.bm = "";
                            gysModel.mc = item.id_gys;
                            gysModel.companytel = "";
                            gysModel.zjm = CySoft.Utility.PinYin.GetChineseSpell(gysModel.mc);
                            gysModel.tel = "";
                            gysModel.lxr = "";
                            gysModel.email = "";
                            gysModel.zipcode = "";
                            gysModel.address = "";
                            gysModel.flag_state = (byte)Enums.TbUserFlagState.Yes;
                            gysModel.bz = "";
                            gysModel.id_create = id_user;
                            gysModel.rq_create = DateTime.Now;
                            gysModel.flag_delete = (byte)Enums.FlagDelete.NoDelete;
                            gysModel.id_gysfl = gysflList.FirstOrDefault().id;
                            addGYSList.Add(gysModel);
                            gysList.Add(new Tb_Gys_User_QueryModel()
                            {
                                flag_delete = gysModel.flag_delete,
                                id = gysModel.id,
                                mc = gysModel.mc,
                                id_masteruser = gysModel.id_masteruser
                            });
                        }
                        #endregion

                        if (allSpList_All.Where(d => d.id_shop == shopitem.Key && d.barcode == item.barcode && d.flag_delete == (byte)Enums.FlagDelete.Deleted).Count() > 0)
                        {
                            //此处是要更新已删除的商品为正常商品
                            #region 更新已存在被删除的商品
                            var delModel = allSpList_All.Where(d => d.id_shop == shopitem.Key && d.barcode == item.barcode && d.flag_delete == (byte)Enums.FlagDelete.Deleted).FirstOrDefault();

                            ht.Clear();
                            ht.Add("id_masteruser", id_masteruser);
                            ht.Add("id", delModel.id);
                            #region 要更新的参数

                            ht.Add("new_flag_delete", "0");
                            if (delModel.barcode != item.barcode)
                            {
                                ht.Add("new_barcode", item.barcode);//条码
                            }

                            if (delModel.bm != item.bm)
                            {
                                ht.Add("new_bm", item.bm);//编码
                            }

                            if (delModel.mc != item.mc)
                            {
                                ht.Add("new_mc", item.mc);//名称
                                ht.Add("new_zjm", CySoft.Utility.PinYin.GetChineseSpell(item.mc.ToString()));//助记码
                            }

                            if (delModel.cd != item.cd)
                            {
                                ht.Add("new_cd", item.cd);//产地
                            }

                            if (delModel.dj_jh != decimal.Parse(item.dj_jh))
                            {
                                ht.Add("new_dj_jh", decimal.Parse(item.dj_jh));//进货价
                            }

                            if (delModel.dj_ls != decimal.Parse(item.dj_ls))
                            {
                                ht.Add("new_dj_ls", decimal.Parse(item.dj_ls));//零售价
                            }

                            if (delModel.dw != item.dw)
                            {
                                ht.Add("new_dw", item.dw);//单位
                            }

                            if (delModel.sl_kc_min != decimal.Parse(item.sl_kc_min))
                            {
                                ht.Add("new_sl_kc_min", item.sl_kc_min);//最底库存量
                            }

                            if (delModel.sl_kc_max != decimal.Parse(item.sl_kc_max))
                            {
                                ht.Add("new_sl_kc_max", item.sl_kc_max);//最高库存量
                            }

                            if (delModel.id_spfl != idspfl)
                            {
                                ht.Add("new_id_spfl", idspfl);//分类ID
                            }

                            if (delModel.dj_hy != decimal.Parse(item.dj_hy) )
                            {
                                ht.Add("new_dj_hy", item.dj_hy);//会员价
                            }

                            if (delModel.flag_czfs != byte.Parse(item.flag_czfs))
                            {
                                ht.Add("new_flag_czfs", item.flag_czfs);//计价方式
                            }

                            if (delModel.dj_ps != decimal.Parse(item.dj_ps) )
                            {
                                ht.Add("new_dj_ps", item.dj_ps);//配送价
                            }

                            if (delModel.dj_pf != decimal.Parse(item.dj_pf) )
                            {
                                ht.Add("new_dj_pf", item.dj_pf);//批发价
                            }

                            if (delModel.yxq != int.Parse(item.yxq))
                            {
                                ht.Add("new_yxq", item.yxq);//有效期
                            }

                            if (delModel.id_gys != id_gys)
                            {
                                ht.Add("new_id_gys", id_gys);//默认供应商
                            }


                            if (delModel.flag_state != byte.Parse(item.flag_state))
                            {
                                ht.Add("new_flag_state", item.flag_state);//flag_state
                            }

                            ht.Add("new_rq_edit", DateTime.Now);//修改日期 
                            #endregion

                            DAL.UpdatePart(typeof(Tb_Shopsp), ht);

                            //if (DAL.UpdatePart(typeof(Tb_Shopsp), ht) <= 0)
                            //{
                            //    br.Data = new { id = "", name = "", value = "" };
                            //    br.Success = false;
                            //    br.Message.Add("操作失败[更新数据库失败]");
                            //    throw new CySoftException(br);
                            //}
                            #endregion

                            continue;
                        }

                        Tb_Shopsp model = new Tb_Shopsp()
                        {
                            id = id_shopsp,
                            id_masteruser = id_masteruser,
                            id_shop = shopitem.Key,
                            id_sp = id_sp,
                            dw = item.dw,
                            bm = item.bm,
                            mc = item.mc,
                            id_kcsp = zID,
                            id_spfl = idspfl,
                            barcode = item.barcode,
                            zjm = CySoft.Utility.PinYin.GetChineseSpell(item.mc),  //MnemonicCode.chinesecap(item.mc),
                            zhl = 1,
                            cd = item.cd,
                            flag_state = byte.Parse(item.flag_state),
                            flag_czfs = byte.Parse(item.flag_czfs),
                            dj_ls = decimal.Parse(item.dj_ls),
                            dj_jh = decimal.Parse(item.dj_jh),
                            dj_hy = decimal.Parse(item.dj_hy),
                            dj_ps = decimal.Parse(item.dj_ps),
                            dj_pf = decimal.Parse(item.dj_pf),
                            sl_kc_min = decimal.Parse(item.sl_kc_min),
                            sl_kc_max = decimal.Parse(item.sl_kc_max),
                            bz = "批量导入...",
                            pic_path = "",
                            id_create = id_user,
                            rq_create = DateTime.Now,
                            id_edit = id_user,
                            rq_edit = DateTime.Now,
                            flag_delete = 0,
                            id_gys = id_gys,
                            yxq = int.Parse(item.yxq),
                        };

                        if (model.zjm.Length > 30)
                        {
                            model.zjm = model.zjm.Substring(0, 29);
                        }

                        addSPList.Add(model);
                        allSpList.Add(model);
                        allSpList_All.Add(model);
                        #endregion

                        #region Td_Sp_Qc AND Tz_Sp_Kc
                        if (item.sl_qc != null && item.sl_qc != 0 && shopitem.Key == id_shop)
                        {
                            Td_Sp_Qc qcModel = new Td_Sp_Qc();
                            qcModel.id_masteruser = id_masteruser;
                            qcModel.id = Guid.NewGuid().ToString();
                            qcModel.id_shop = shopitem.Key;
                            qcModel.id_shopsp = model.id_kcsp;
                            qcModel.je_qc = item.je_qc;
                            qcModel.sl_qc = item.sl_qc;
                            qcModel.dj_cb = (item.je_qc / item.sl_qc);
                            qcModel.id_kcsp = model.id_kcsp;
                            addQCList.Add(qcModel);

                            Tz_Sp_Kc kcModel = new Tz_Sp_Kc();
                            kcModel.id_masteruser = id_masteruser;
                            kcModel.id = Guid.NewGuid().ToString();
                            kcModel.id_shop = shopitem.Key;
                            kcModel.id_kcsp = qcModel.id_kcsp;
                            kcModel.sl_qm = qcModel.sl_qc;
                            kcModel.je_qm = qcModel.je_qc;
                            kcModel.dj_cb = (kcModel.je_qm / kcModel.sl_qm);
                            addKCList.Add(kcModel);

                            Tz_Jxc_Flow flowModel = new Tz_Jxc_Flow();
                            flowModel.id_masteruser = param["id_masteruser"].ToString();
                            flowModel.id = Guid.NewGuid().ToString();
                            flowModel.id_billmx = "qc";
                            flowModel.bm_djlx = "qc";
                            flowModel.id_shop = shopitem.Key;
                            flowModel.id_shopsp = qcModel.id_shopsp;
                            flowModel.sl = qcModel.sl_qc;
                            flowModel.je = qcModel.je_qc;
                            flowModel.rq = DateTime.Now;
                            flowModel.id_kcsp = qcModel.id_kcsp;
                            addFlowList.Add(flowModel);
                        }
                        #endregion
                    }
                    #endregion

                    #region 新增单位
                    if (dbDWList == null || dbDWList.Count() <= 0 || dbDWList.Where(d => d.dw == item.dw).Count() <= 0)
                    {
                        var addDw = new Tb_Dw() { id = Guid.NewGuid().ToString(), dw = item.dw, id_create = id_user, id_masteruser = id_masteruser, rq_create = DateTime.Now, id_edit = id_user, rq_edit = DateTime.Now, flag_delete = 0 };
                        addDWList.Add(addDw);
                        dbDWList.Add(addDw);
                    }
                    #endregion

                    successList.Add(item);
                }
            }
            #endregion
            #region 保存商品相关信息

            DAL.AddRange(addSPFLList);
            DAL.AddRange(addSPList);
            DAL.AddRange(addQCList);
            DAL.AddRange(addKCList);
            DAL.AddRange(addDWList);
            DAL.AddRange(addFlowList);
            DAL.AddRange(addGYSList);

            #endregion
            #region 检验库存账本
            //检验库存账本
            ht.Clear();
            ht.Add("proname", "p_rzh_jykc");
            ht.Add("errorid", "-1");
            ht.Add("errormessage", "未知错误！");
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("id_shop", "0");
            DAL.RunProcedure2(ht);

            if (!ht.ContainsKey("errorid") || !ht.ContainsKey("errormessage"))
            {
                br.Success = false;
                br.Message.Add("检验库存账本失败,执行检验库存账本出现异常!");
                throw new CySoftException(br);
            }

            if (!string.IsNullOrEmpty(ht["errorid"].ToString()) || !string.IsNullOrEmpty(ht["errormessage"].ToString()))
            {
                br.Success = false;
                br.Message.Add(ht["errormessage"].ToString());
                throw new CySoftException(br);
            }

            #endregion
            br.Message.Add(String.Format("导入商品成功!"));
            br.Success = true;
            br.Data = new Tb_Shopsp_Import_All() { SuccessList = successList, FailList = failList };
            return br;
        }
        #endregion

        #region 商品列表
        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(Hashtable param)
        {
            PageNavigate pn;
            List<Tb_Shopsp_Query> list = new List<Tb_Shopsp_Query>();

            if (param.ContainsKey("id_spfl") && !string.IsNullOrEmpty(param["id_spfl"].ToString()) && param["id_spfl"].ToString() != "0")
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", param["id_spfl"].ToString());
                var spflModel = DAL.GetItem<Tb_Spfl>(typeof(Tb_Spfl), ht);
                if (spflModel != null && !string.IsNullOrEmpty(spflModel.id) && !string.IsNullOrEmpty(spflModel.path))
                {
                    param.Remove("id_spfl");
                    param.Add("spfl_path", spflModel.path + "%/");
                }
            }
            if (param.ContainsKey("id_shop_ck"))
                param["id_shop_ck"] = string.Format("'{0}'", param["id_shop_ck"].ToString().SQLFilterStr());

            //param["id_shop_ck"] = string.Format("'{0}'", param["id_shop_ck"]);

            pn = new PageNavigate() { TotalCount = DAL.GetCount(typeof(Tb_Shopsp), param) };

            if (pn.TotalCount > 0)
            {
                list = DAL.QueryPage<Tb_Shopsp_Query>(typeof(Tb_Shopsp), param).ToList() ?? new List<Tb_Shopsp_Query>();
            }
            pn.Data = list;
            pn.Success = true;
            return pn;
        }
        #endregion

        #region Get
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string id = param.ContainsKey("id") ? param["id"].ToString() : string.Empty;
            string mc = param.ContainsKey("mc") ? param["mc"].ToString() : string.Empty;
            string id_masteruser = param["id_masteruser"].ToString();
            string id_user = param["id_user"].ToString();

            var model = new Tb_Shopsp_Get();

            Hashtable ht = new Hashtable();
            if (param.ContainsKey("id")) ht.Add("id", param["id"]);
            if (param.ContainsKey("mc")) ht.Add("mc", param["mc"]);
            if (param.ContainsKey("barcode")) ht.Add("barcode", param["barcode"]);
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            var sp = DAL.GetItem<Tb_Shopsp>(typeof(Tb_Shopsp), ht);
            if (sp == null)
            {
                br.Success = false;
                br.Message.Add("商品不存在.");
                return br;
            }
            model.ShopSP = sp;

            if (param.ContainsKey("need_query_spfl") && param["need_query_spfl"].ToString() == "Yes")
            {
                ht.Clear();
                ht.Add("id", sp.id_spfl);
                ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                model.Spfl = DAL.GetItem<Tb_Spfl>(typeof(Tb_Spfl), ht) ?? new Tb_Spfl();
            }

            if (param.ContainsKey("need_query_shop") && param["need_query_shop"].ToString() == "Yes")
            {
                ht.Clear();
                ht.Add("id", sp.id_shop);
                ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                model.Shop = DAL.GetItem<Tb_Shop>(typeof(Tb_Shop), ht) ?? new Tb_Shop();
            }

            if (param.ContainsKey("need_query_kc") && param["need_query_kc"].ToString() == "Yes")
            {
                ht.Clear();
                ht.Add("id_kcsp", sp.id_kcsp);
                ht.Add("id_shop", sp.id_shop);
                ht.Add("id_masteruser", sp.id_masteruser);
                model.Kc = DAL.GetItem<Tz_Sp_Kc>(typeof(Tz_Sp_Kc), ht);
            }

            if (param.ContainsKey("need_query_qc") && param["need_query_qc"].ToString() == "Yes")
            {
                ht.Clear();
                ht.Add("id_masteruser", sp.id_masteruser);
                ht.Add("id_shop", sp.id_shop);
                ht.Add("id_shopsp", sp.id);
                model.Qc = DAL.GetItem<Td_Sp_Qc>(typeof(Td_Sp_Qc), ht);
            }

            if (param.ContainsKey("need_query_dbz") && param["need_query_dbz"].ToString() == "Yes")
            {
                model.ShopSP_DBZ = new List<Tb_Shopsp>();
                if (sp.id_sp == sp.id_kcsp)
                {
                    ht.Clear();
                    ht.Add("id_kcsp", sp.id_kcsp);
                    ht.Add("id_masteruser", sp.id_masteruser);
                    ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                    var allSp = DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), ht).ToList();
                    model.ShopSP_DBZ = allSp.Where(d => d.id_shop == sp.id_shop && d.id_sp != sp.id_sp).ToList();
                }
                //ht.Clear();
                //ht.Add("id_kcsp", sp.id_kcsp);
                //ht.Add("id_masteruser", sp.id_masteruser);
                //var allSp = DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), ht).ToList();
                //var masterSp = allSp.Where(d => d.id_sp == sp.id_kcsp && d.id_shop == sp.id_shop).FirstOrDefault();
                //if (masterSp != null)
                //{
                //    if (masterSp.id == sp.id)
                //    {
                //        model.ShopSP_DBZ = allSp.Where(d => d.id_shop == sp.id_shop && d.id_sp != masterSp.id_sp).ToList();
                //    }
                //}
            }

            br.Data = model;
            br.Success = true;
            return br;
        }
        #endregion

        #region GetAll
        public override BaseResult GetAll(Hashtable param)
        {
            BaseResult br = new BaseResult();
            var sp = DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), param);
            br.Data = sp;
            br.Success = true;
            return br;
        }
        #endregion

        #region TurnShopSPList
        /// <summary>
        /// 将Hashtable转换为Model
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private List<Tb_Shopsp> TurnShopSPList(Hashtable param)
        {
            List<Tb_Shopsp> list = new List<Tb_Shopsp>();
            List<Tb_Shopsp_DBZ> dbzList = (List<Tb_Shopsp_DBZ>)param["dbzList"];
            string id_sp = Guid.NewGuid().ToString();
            string zID = Guid.NewGuid().ToString();
            string id_shop = param["id_shop"].ToString();
            string id_masteruser = param["id_masteruser"].ToString();
            byte flag_czfs = 0;
            byte.TryParse(param["flag_czfs"] == null ? "0" : param["flag_czfs"].ToString(), out flag_czfs);
            int yxq = 0;
            int.TryParse(param["yxq"].ToString(), out yxq);
            byte flag_state = 1;
            if (param["flag_state"] != null && !string.IsNullOrEmpty(param["flag_state"].ToString()))
                byte.TryParse(param["flag_state"] == null ? "1" : param["flag_state"].ToString(), out flag_state);

            int i = 1;
            Tb_Shopsp model = new Tb_Shopsp()
            {
                id_view = "",
                id = zID,
                id_masteruser = id_masteruser,
                id_shop = id_shop,
                id_sp = id_sp,
                //dw_xh = i,
                dw = param["dw"].ToString(),
                bm = param["bm"].ToString(),
                mc = param["mc"].ToString(),
                id_kcsp = zID,
                id_spfl = param["id_spfl"].ToString(),
                barcode = param["barcode"].ToString(),
                zjm = CySoft.Utility.PinYin.GetChineseSpell(param["mc"].ToString()),// MnemonicCode.chinesecap(param["mc"].ToString()),
                zhl = 1,
                cd = param["cd"].ToString(),
                //flag_state = (byte)Enums.FlagShopspStop.NoStop,
                flag_state = flag_state,
                flag_czfs = flag_czfs,
                dj_ls = decimal.Parse(param["dj_ls"].ToString()),
                dj_jh = decimal.Parse(param["dj_jh"].ToString()),
                sl_kc_min = decimal.Parse(param["sl_kc_min"].ToString()),
                sl_kc_max = decimal.Parse(param["sl_kc_max"].ToString()),
                bz = param["bz"] == null ? "" : param["bz"].ToString(),
                pic_path = param["pic_path"] == null ? "" : param["pic_path"].ToString(),
                id_create = param["id_user"].ToString(),
                rq_create = DateTime.Now,
                id_edit = param["id_user"].ToString(),
                rq_edit = DateTime.Now,
                flag_delete = 0,
                dj_hy = decimal.Parse(param["dj_hy"].ToString()),

                id_gys = param["id_gys"].ToString(),
                yxq = yxq,
                dj_ps = decimal.Parse(param["dj_ps"].ToString()),
                dj_pf = decimal.Parse(param["dj_pf"].ToString())



            };
            list.Add(model);
            i++;

            foreach (var item in dbzList)
            {
                Tb_Shopsp modelDBZ = new Tb_Shopsp()
                {
                    id_view = item.id,
                    id = Guid.NewGuid().ToString(),
                    id_masteruser = id_masteruser,
                    id_shop = id_shop,
                    id_sp = id_sp,
                    //dw_xh = i,
                    dw = item.dw,
                    bm = item.bm,
                    mc = item.mc,
                    id_kcsp = zID,
                    id_spfl = param["id_spfl"].ToString(),
                    barcode = item.barcode,
                    zjm = CySoft.Utility.PinYin.GetChineseSpell(item.mc),//MnemonicCode.chinesecap(item.mc),
                    zhl = item.zhl,
                    cd = param["cd"].ToString(),
                    flag_state = flag_state,
                    flag_czfs = 0,
                    dj_ls = item.dj_ls,
                    dj_jh = item.dj_jh,
                    sl_kc_min = decimal.Parse(param["sl_kc_min"].ToString()),
                    sl_kc_max = decimal.Parse(param["sl_kc_max"].ToString()),
                    bz = param["bz"] == null ? "" : param["bz"].ToString(),
                    pic_path = param["pic_path"] == null ? "" : param["pic_path"].ToString(),
                    id_create = param["id_user"].ToString(),
                    rq_create = DateTime.Now,
                    id_edit = param["id_user"].ToString(),
                    rq_edit = DateTime.Now,
                    flag_delete = 0,
                    dj_hy = item.dj_hy,

                    id_gys = param["id_gys"].ToString(),
                    yxq = yxq,
                    dj_ps = item.dj_ps,
                    dj_pf = item.dj_pf

                };

                if (!string.IsNullOrEmpty(item.pic_path))
                    modelDBZ.pic_path = item.pic_path;

                list.Add(modelDBZ);
                i++;
            }
            return list;
        }
        #endregion

        #region CheckImportInfo
        public BaseResult CheckImportInfo(Tb_Shopsp_Import model, Hashtable digitHashtable)
        {
            BaseResult br = new BaseResult();
            if (string.IsNullOrEmpty(model.barcode))
            {
                br.Success = false;
                br.Data = "barcode";
                br.Message.Add("条码不能为空");
                return br;
            }

            if (string.IsNullOrEmpty(model.bm))
            {
                br.Success = false;
                br.Data = "bm";
                br.Message.Add("编码不能为空");
                return br;
            }

            if (string.IsNullOrEmpty(model.mc))
            {
                br.Success = false;
                br.Data = "mc";
                br.Message.Add("名称不能为空");
                return br;
            }

            if (string.IsNullOrEmpty(model.dj_jh) || !CyVerify.IsNumeric(model.dj_jh) || decimal.Parse(model.dj_jh) < 0)
            {
                br.Success = false;
                br.Data = "dj_jh";
                br.Message.Add("进货价不符合要求");
                return br;
            }
            if (string.IsNullOrEmpty(model.dj_ls) || !CyVerify.IsNumeric(model.dj_ls) || decimal.Parse(model.dj_ls) < 0)
            {
                br.Success = false;
                br.Data = "dj_ls";
                br.Message.Add("零售价不符合要求");
                return br;
            }

            if (string.IsNullOrEmpty(model.dw))
            {
                br.Success = false;
                br.Data = "dw";
                br.Message.Add("单位不能为空");
                return br;
            }
            if (string.IsNullOrEmpty(model.sl_kc_min) || !CyVerify.IsNumeric(model.sl_kc_min) || decimal.Parse(model.sl_kc_min) < 0)
            {
                br.Success = false;
                br.Data = "sl_kc_min";
                br.Message.Add("最底库存量不符合要求");
                return br;
            }
            if (string.IsNullOrEmpty(model.sl_kc_max) || !CyVerify.IsNumeric(model.sl_kc_max) || decimal.Parse(model.sl_kc_max) < 0)
            {
                br.Success = false;
                br.Data = "sl_kc_max";
                br.Message.Add("最高库存量不符合要求");
                return br;
            }

            if (decimal.Parse(model.sl_kc_max) < decimal.Parse(model.sl_kc_max))
            {
                br.Success = false;
                br.Data = "sl_kc_max";
                br.Message.Add("最高库存量不应小于最底库存量.");
                return br;
            }

            if (string.IsNullOrEmpty(model.id_spfl) && string.IsNullOrEmpty(model.id_spfl))
            {
                br.Success = false;
                br.Data = "id_spfl";
                br.Message.Add("商品类别不符合要求");
                return br;
            }

            if (string.IsNullOrEmpty(model.dj_hy) || !CyVerify.IsNumeric(model.dj_hy) || decimal.Parse(model.dj_hy) < 0)
            {
                br.Success = false;
                br.Data = "dj_hy";
                br.Message.Add("会员价不符合要求");
                return br;
            }

            if (string.IsNullOrEmpty(model.dj_ps) || !CyVerify.IsNumeric(model.dj_ps) || decimal.Parse(model.dj_ps) < 0)
            {
                br.Success = false;
                br.Data = "dj_ps";
                br.Message.Add("配送价不符合要求");
                return br;
            }

            if (string.IsNullOrEmpty(model.dj_pf) || !CyVerify.IsNumeric(model.dj_pf) || decimal.Parse(model.dj_pf) < 0)
            {
                br.Success = false;
                br.Data = "dj_pf";
                br.Message.Add("批发价不符合要求");
                return br;
            }

            if (string.IsNullOrEmpty(model.flag_czfs) || !CyVerify.IsNumeric(model.flag_czfs) || (byte.Parse(model.flag_czfs) != 0 && byte.Parse(model.flag_czfs) != 1 && byte.Parse(model.flag_czfs) != 2))
            {
                br.Success = false;
                br.Data = "flag_czfs";
                br.Message.Add("计价方式不符合要求");
                return br;
            }

            if (byte.Parse(model.flag_czfs) == 1 || byte.Parse(model.flag_czfs) == 2)
            {
                if (!model.barcode.StartsWith(PublicSign.dh_cz))
                {
                    br.Success = false;
                    br.Data = "barcode";
                    br.Message.Add("非普通商品 条码 不合要求 必须以 " + PublicSign.dh_cz + " 开头");
                    return br;
                }
            }


            if (string.IsNullOrEmpty(model.flag_state) || !CyVerify.IsNumeric(model.flag_state) || (byte.Parse(model.flag_state) != 0 && byte.Parse(model.flag_state) != 1))
            {
                br.Success = false;
                br.Data = "flag_state";
                br.Message.Add("商品状态不符合要求");
                return br;
            }

            model.sl_qc = model.sl_qc == null ? 0 : model.sl_qc;
            model.je_qc = model.je_qc == null ? 0 : model.je_qc;

            if ((model.sl_qc > 0 && model.je_qc < 0) || (model.sl_qc < 0 && model.je_qc > 0))
            {
                br.Success = false;
                br.Data = "sp_qc";
                br.Message.Add("期初不允许数量和金额一正一负");
                return br;
            }


            model.dj_jh = CySoft.Utility.DecimalExtension.Digit(decimal.Parse(model.dj_jh), int.Parse(digitHashtable["dj_digit"].ToString())).ToString();
            model.dj_ls = CySoft.Utility.DecimalExtension.Digit(decimal.Parse(model.dj_ls), int.Parse(digitHashtable["dj_digit"].ToString())).ToString();
            model.dj_hy = CySoft.Utility.DecimalExtension.Digit(decimal.Parse(model.dj_hy), int.Parse(digitHashtable["dj_digit"].ToString())).ToString();
            model.sl_kc_min = CySoft.Utility.DecimalExtension.Digit(decimal.Parse(model.sl_kc_min), int.Parse(digitHashtable["sl_digit"].ToString())).ToString();
            model.sl_kc_max = CySoft.Utility.DecimalExtension.Digit(decimal.Parse(model.sl_kc_max), int.Parse(digitHashtable["sl_digit"].ToString())).ToString();
            model.je_qc = CySoft.Utility.DecimalExtension.Digit(model.je_qc, int.Parse(digitHashtable["je_digit"].ToString()));
            model.sl_qc = CySoft.Utility.DecimalExtension.Digit(model.sl_qc, int.Parse(digitHashtable["sl_digit"].ToString()));


            if (model.mc.Length > 80)
            {
                br.Success = false;
                br.Data = "mc";
                br.Message.Add("名称最长80位");
                return br;
            }


            if (!string.IsNullOrEmpty(model.bz) && model.bz.Length > 200)
            {
                br.Success = false;
                br.Data = "bz";
                br.Message.Add("备注最长200位");
                return br;
            }


            br.Success = true;
            return br;
        }
        #endregion

        #region GetShopspList
        public BaseResult GetShopspList(Hashtable param)
        {
            BaseResult br = new BaseResult();

            if (param.ContainsKey("id_spfl") && !string.IsNullOrEmpty(param["id_spfl"].ToString()) && param["id_spfl"].ToString() != "0")
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", param["id_spfl"].ToString());
                var spflModel = DAL.GetItem<Tb_Spfl>(typeof(Tb_Spfl), ht);
                if (spflModel != null && !string.IsNullOrEmpty(spflModel.id) && !string.IsNullOrEmpty(spflModel.path))
                {
                    param.Remove("id_spfl");
                    param.Add("spfl_path", spflModel.path + "%/");
                }
            }
            var id_spfls = string.Format("{0}", param["id_spfls"]);
            List<Tb_Spfl> spflList = new List<Tb_Spfl>();
            if (!id_spfls.IsEmpty() && id_spfls != "0")
            {
                var arr_spfl = id_spfls.Split(',');
                Hashtable ht = new Hashtable();
                ht.Add("flList", arr_spfl.Where(a => !a.IsEmpty()).ToArray());
                spflList = DAL.QueryList<Tb_Spfl>(typeof(Tb_Spfl), ht).ToList();
                param.Remove("id_spfls");
            }

            if (param.ContainsKey("id_shop_ck"))
                param["id_shop_ck"] = string.Format("'{0}'", param["id_shop_ck"].ToString().SQLFilterStr());

            var sp = Tb_ShopspDAL.GetShopspList(typeof(Tb_Shopsp), param);
            if (spflList.Any())
            {
                sp = sp.Where(s => spflList.Any(f => f.id == s.id_spfl)).ToList();
            }
            br.Data = sp;
            br.Success = true;
            return br;
        }
        public BaseResult GetShopspListForPs(Hashtable param)
        {
            BaseResult br = new BaseResult();

            if (param.ContainsKey("id_spfl") && !string.IsNullOrEmpty(param["id_spfl"].ToString()) && param["id_spfl"].ToString() != "0")
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", param["id_spfl"].ToString());
                var spflModel = DAL.GetItem<Tb_Spfl>(typeof(Tb_Spfl), ht);
                if (spflModel != null && !string.IsNullOrEmpty(spflModel.id) && !string.IsNullOrEmpty(spflModel.path))
                {
                    param.Remove("id_spfl");
                    param.Add("spfl_path", spflModel.path + "%/");
                }
            }

            if (param.ContainsKey("id_shop_ck"))
                param["id_shop_ck"] = string.Format("'{0}'", param["id_shop_ck"].ToString().SQLFilterStr());

            var sp = Tb_ShopspDAL.GetShopspListForPs(typeof(Tb_Shopsp), param);
            br.Data = sp;
            br.Success = true;
            return br;
        }
        #endregion

        #region GetPageList
        public PageNavigate GetPageList(Hashtable param)
        {
            PageNavigate pn;
            List<SelectSpModel> list = new List<SelectSpModel>();

            if (param.ContainsKey("id_spfl") && !string.IsNullOrEmpty(param["id_spfl"].ToString()) && param["id_spfl"].ToString() != "0")
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", param["id_spfl"].ToString());
                var spflModel = DAL.GetItem<Tb_Spfl>(typeof(Tb_Spfl), ht);
                if (spflModel != null && !string.IsNullOrEmpty(spflModel.id) && !string.IsNullOrEmpty(spflModel.path))
                {
                    param.Remove("id_spfl");
                    param.Add("spfl_path", spflModel.path + "%/");
                }
            }

            if (param.ContainsKey("id_shop_ck"))
                param["id_shop_ck"] = string.Format("'{0}'", param["id_shop_ck"].ToString().SQLFilterStr());


            pn = new PageNavigate() { TotalCount = DAL.GetCount(typeof(Tb_Shopsp), param) };
            if (pn.TotalCount > 0)
            {
                list = Tb_ShopspDAL.GetPageList(typeof(Tb_Shopsp), param).ToList() ?? new List<SelectSpModel>(); ;
            }
            pn.Data = list;
            pn.Success = true;
            return pn;
        }
        #endregion

        #region 检测新增/编辑的数据是否合法
        /// <summary>
        /// 检测新增/编辑的数据是否合法
        /// </summary>
        /// <param name="param"></param>
        /// <param name="qcModel"></param>
        /// <param name="dbzList"></param>
        /// <returns></returns>
        public BaseResult CheckParam(Hashtable param, Td_Sp_Qc qcModel, List<Tb_Shopsp_DBZ> dbzList)
        {
            BaseResult br = new BaseResult();

            #region 验证数据
            //控制层验证数据
            if (string.IsNullOrEmpty(param["barcode"].ToString()))
            {
                br.Success = false;
                br.Message.Add("条码不能为空!");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "barcode", value = param["barcode"].ToString() };
                return br;
            }

            if (string.IsNullOrEmpty(param["bm"].ToString()))
            {
                br.Success = false;
                br.Message.Add("编码不能为空!");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "bm", value = param["bm"].ToString() };
                return br;
            }

            if (string.IsNullOrEmpty(param["mc"].ToString()))
            {
                br.Success = false;
                br.Message.Add("名称不能为空");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "mc", value = param["mc"].ToString() };
                return br;
            }

            if (string.IsNullOrEmpty(param["dj_jh"].ToString()) || !CyVerify.IsNumeric(param["dj_jh"].ToString()) || decimal.Parse(param["dj_jh"].ToString()) < 0)
            {
                br.Success = false;
                br.Message.Add("进货价不符合要求");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "dj_jh", value = param["dj_jh"].ToString() };
                return br;
            }

            if (string.IsNullOrEmpty(param["dj_ls"].ToString()) || !CyVerify.IsNumeric(param["dj_ls"].ToString()) || decimal.Parse(param["dj_ls"].ToString()) < 0)
            {
                br.Success = false;
                br.Message.Add("零售价不符合要求");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "dj_ls", value = param["dj_ls"].ToString() };
                return br;
            }

            if (string.IsNullOrEmpty(param["dj_hy"].ToString()) || !CyVerify.IsNumeric(param["dj_hy"].ToString()) || decimal.Parse(param["dj_hy"].ToString()) < 0)
            {
                br.Success = false;
                br.Message.Add("会员价不符合要求");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "dj_hy", value = param["dj_hy"].ToString() };
                return br;
            }

            if (string.IsNullOrEmpty(param["dj_ps"].ToString()) || !CyVerify.IsNumeric(param["dj_ps"].ToString()) || decimal.Parse(param["dj_ps"].ToString()) < 0)
            {
                br.Success = false;
                br.Message.Add("配送价不符合要求");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "dj_ps", value = param["dj_ps"].ToString() };
                return br;
            }

            if (!string.IsNullOrEmpty(param["dj_pf"].ToString()) && !CyVerify.IsNumeric(param["dj_pf"].ToString()) || decimal.Parse(param["dj_pf"].ToString()) < 0)
            {
                br.Success = false;
                br.Message.Add("批发价不符合要求");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "dj_pf", value = param["dj_pf"].ToString() };
                return br;
            }

            if (string.IsNullOrEmpty(param["dw"].ToString()))
            {
                br.Success = false;
                br.Message.Add("单位不能为空");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "dw", value = param["dw"].ToString() };
                return br;
            }
            if (string.IsNullOrEmpty(param["sl_kc_min"].ToString()) || !CyVerify.IsNumeric(param["sl_kc_min"].ToString()) || decimal.Parse(param["sl_kc_min"].ToString()) < 0)
            {
                br.Success = false;
                br.Message.Add("最底库存量不符合要求");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "sl_kc_min", value = param["sl_kc_min"].ToString() };
                return br;
            }
            if (string.IsNullOrEmpty(param["sl_kc_max"].ToString()) || !CyVerify.IsNumeric(param["sl_kc_max"].ToString()) || decimal.Parse(param["sl_kc_max"].ToString()) < 0)
            {
                br.Success = false;
                br.Message.Add("最高库存量不符合要求");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "sl_kc_max", value = param["sl_kc_max"].ToString() };
                return br;
            }

            if (decimal.Parse(param["sl_kc_max"].ToString()) < decimal.Parse(param["sl_kc_min"].ToString()))
            {
                br.Success = false;
                br.Message.Add("最高库存量不应小于最底库存量.");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "sl_kc_max", value = param["sl_kc_max"].ToString() };
                return br;
            }

            if ((param["id_spfl"] == null || string.IsNullOrEmpty(param["id_spfl"].ToString())))
            {
                br.Success = false;
                br.Message.Add("商品类别不符合要求");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "id_spfl", value = param["id_spfl"].ToString() };
                return br;
            }
            else
            {
                Hashtable ht = new Hashtable();
                ht.Clear();
                ht.Add("id", param["id_spfl"].ToString());
                ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                if (DAL.GetCount(typeof(Tb_Spfl), ht).Equals(0))
                {
                    br.Success = false;
                    br.Message.Add("商品类别不存在或已删除");
                    br.Level = ErrorLevel.Warning;
                    br.Data = new { id = "", name = "id_spfl", value = param["id_spfl"].ToString() };
                    return br;
                }
            }

            if (param.ContainsKey("id_gys") && !string.IsNullOrEmpty(param["id_gys"].ToString()))
            {
                Hashtable ht = new Hashtable();
                ht.Clear();
                ht.Add("id", param["id_gys"].ToString());
                ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                if (DAL.GetCount(typeof(Tb_Gys), ht).Equals(0))
                {
                    br.Success = false;
                    br.Message.Add("默认供应商不存在或已删除");
                    br.Level = ErrorLevel.Warning;
                    br.Data = new { id = "", name = "id_gys", value = param["id_gys"].ToString() };
                    return br;
                }
            }


            if (param.ContainsKey("flag_czfs") && !string.IsNullOrEmpty(param["flag_czfs"].ToString()))
            {
                if (!CyVerify.IsInt32(param["flag_czfs"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add("计价方式格式不正确");
                    br.Level = ErrorLevel.Warning;
                    br.Data = new { id = "", name = "flag_czfs", value = param["flag_czfs"].ToString() };
                    return br;
                }

                Hashtable ht = new Hashtable();
                ht.Add("listcode", "spczfs");
                ht.Add("sort", "listsort");
                ht.Add("dir", "asc");
                var czfsList = DAL.QueryList<Ts_Flag>(typeof(Ts_Flag), ht);
                if (czfsList == null || czfsList.Count() <= 0 || czfsList.Where(d => d.listdata == int.Parse(param["flag_czfs"].ToString())).Count() <= 0)
                {
                    br.Success = false;
                    br.Message.Add("计价方式不存在或已删除");
                    br.Level = ErrorLevel.Warning;
                    br.Data = new { id = "", name = "flag_czfs", value = param["flag_czfs"].ToString() };
                    return br;
                }

                if (param["flag_czfs"].ToString() == "1" || param["flag_czfs"].ToString() == "2")
                {
                    //如果新增称重和计件商品，条码必须21开头
                    if (!param["barcode"].ToString().StartsWith(PublicSign.dh_cz) || param["barcode"].ToString().Length < (PublicSign.dh_cz.Length + 10) || param["barcode"].ToString().Substring(PublicSign.dh_cz.Length + 5, 5) != "00000")
                    {
                        br.Success = false;
                        br.Message.Add("称重和计件商品，条码必须 " + PublicSign.dh_cz + " 开头 +5位数字(不能重复)+5个0 ");
                        br.Level = ErrorLevel.Warning;
                        br.Data = new { id = "", name = "barcode", value = param["barcode"].ToString() };
                        return br;
                    }


                    if (PublicSign.dh_cz.Length >= 1)
                    {
                        var checkBarcode = param["barcode"].ToString().Substring(0, PublicSign.dh_cz.Length + 10) + "%";
                        if (param.ContainsKey("id") && !string.IsNullOrEmpty(param["id"].ToString()))
                        {
                            //修改
                            ht.Clear();
                            ht.Add("id_masteruser", param["id_masteruser"].ToString());
                            ht.Add("barcode_like", checkBarcode);
                            ht.Add("not_id_id_kcsp", param["id"].ToString());
                            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                            var sp = DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), ht);
                            //新增
                            if (sp != null && sp.Count() > 0)
                            {
                                br.Success = false;
                                br.Message.Add("称重和计件商品，条码必须 " + PublicSign.dh_cz + " 开头 +5位数字(不能重复)+5个0  条码重复 ");
                                br.Level = ErrorLevel.Warning;
                                br.Data = new { id = "", name = "barcode", value = param["barcode"].ToString() };
                                return br;
                            }
                        }
                        else
                        {
                            //新增
                            ht.Clear();
                            ht.Add("id_masteruser", param["id_masteruser"].ToString());
                            ht.Add("barcode_like", checkBarcode);
                            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                            var sp = DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), ht);
                            //新增
                            if (sp != null && sp.Count() > 0)
                            {
                                br.Success = false;
                                br.Message.Add("称重和计件商品，条码必须 " + PublicSign.dh_cz + " 开头 +5位数字(不能重复)+5个0  条码重复 ");
                                br.Level = ErrorLevel.Warning;
                                br.Data = new { id = "", name = "barcode", value = param["barcode"].ToString() };
                                return br;
                            }
                        }
                    }


                }
            }


            if (param.ContainsKey("flag_state") && !string.IsNullOrEmpty(param["flag_state"].ToString()))
            {
                if (!CyVerify.IsInt32(param["flag_state"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add("商品状态格式不正确");
                    br.Level = ErrorLevel.Warning;
                    br.Data = new { id = "", name = "flag_state", value = param["flag_state"].ToString() };
                    return br;
                }

                Hashtable ht = new Hashtable();
                ht.Add("listcode", "spstate");
                ht.Add("sort", "listsort");
                ht.Add("dir", "asc");
                var stateList = DAL.QueryList<Ts_Flag>(typeof(Ts_Flag), ht);
                if (stateList == null || stateList.Count() <= 0 || stateList.Where(d => d.listdata == int.Parse(param["flag_state"].ToString())).Count() <= 0)
                {
                    br.Success = false;
                    br.Message.Add("商品状态不存在或已删除");
                    br.Level = ErrorLevel.Warning;
                    br.Data = new { id = "", name = "flag_state", value = param["flag_state"].ToString() };
                    return br;
                }
            }


            if (param["bz"] != null && !string.IsNullOrWhiteSpace(param["bz"].ToString()) && param["bz"].ToString().Length > 200)
            {
                br.Success = false;
                br.Message.Add("商品备注应在200字以内.");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "bz", value = param["bz"].ToString() };
                return br;
            }

            #region 期初数据验证
            if (qcModel == null)
            {
                br.Success = false;
                br.Message.Add("期初不能为空");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "sl_qc", value = param["sl_qc"].ToString() };
                return br;
            }

            qcModel.sl_qc = qcModel.sl_qc == null ? 0 : qcModel.sl_qc;
            qcModel.je_qc = qcModel.je_qc == null ? 0 : qcModel.je_qc;

            if ((qcModel.sl_qc > 0 && qcModel.je_qc < 0) || (qcModel.sl_qc < 0 && qcModel.je_qc > 0))
            {
                br.Success = false;
                br.Message.Add("期初不允许数量和金额一正一负");
                br.Level = ErrorLevel.Warning;
                br.Data = new { id = "", name = "sl_qc", value = param["sl_qc"].ToString() };
                return br;
            }
            #endregion

            #region 多包装数据验证
            foreach (var item in dbzList)
            {
                if (string.IsNullOrEmpty(item.barcode))
                {
                    br.Success = false;
                    br.Message.Add("多包装商品条码不能为空");
                    br.Level = ErrorLevel.Warning;
                    br.Data = new { id = item.id, name = "barcode", value = item.barcode };
                    return br;
                }

                if (string.IsNullOrEmpty(item.bm))
                {
                    br.Success = false;
                    br.Message.Add("多包装商品编码不能为空");
                    br.Level = ErrorLevel.Warning;
                    br.Data = new { id = item.id, name = "bm", value = item.bm };
                    return br;
                }

                //多包装不验证称重和计件商品，条码必须21开头
                //if (param["flag_czfs"].ToString() == "1" || param["flag_czfs"].ToString() == "2")
                //{
                //    //如果新增称重和计件商品，条码必须21开头
                //    if (!item.barcode.ToString().StartsWith("21"))
                //    {
                //        br.Success = false;
                //        br.Message.Add("称重和计件商品，条码必须21开头!");
                //        br.Level = ErrorLevel.Warning;
                //        br.Data = new { id = item.id, name = "barcode", value = item.barcode };
                //        return br;
                //    }
                //}

                if (string.IsNullOrEmpty(item.mc))
                {
                    br.Success = false;
                    br.Message.Add("多包装商品名称不能为空");
                    br.Level = ErrorLevel.Warning;
                    br.Data = new { id = item.id, name = "mc", value = item.mc };
                    return br;
                }


                if (item.barcode == param["barcode"].ToString())
                {
                    br.Success = false;
                    br.Message.Add("多包装商品条码不能与主商品条码重复");
                    br.Level = ErrorLevel.Warning;
                    br.Data = new { id = item.id, name = "barcode", value = item.barcode };
                    return br;
                }

                if (item.bm == param["bm"].ToString())
                {
                    br.Success = false;
                    br.Message.Add("多包装商品编码不能与主商品编码重复");
                    br.Level = ErrorLevel.Warning;
                    br.Data = new { id = item.id, name = "bm", value = item.bm };
                    return br;
                }


                if (dbzList.Where(d => d.barcode == item.barcode).Count() >= 2)
                {
                    br.Success = false;
                    br.Message.Add("多包装商品条码重复");
                    br.Level = ErrorLevel.Warning;
                    br.Data = new { id = item.id, name = "barcode", value = item.barcode };
                    return br;
                }

                if (dbzList.Where(d => d.bm == item.bm).Count() >= 2)
                {
                    br.Success = false;
                    br.Message.Add("多包装商品编码重复");
                    br.Level = ErrorLevel.Warning;
                    br.Data = new { id = item.id, name = "bm", value = item.bm };
                    return br;
                }

                if (item.dw == param["dw"].ToString())
                {
                    br.Success = false;
                    br.Message.Add("多包装商品单位不能与主商品单位重复");
                    br.Level = ErrorLevel.Warning;
                    //br.Data = new { id = item.id, name = "dw", value = item.dw };
                    br.Data = new { id = "", name = "", value = "" };
                    return br;
                }

                if (dbzList.Where(d => d.dw == item.dw).Count() >= 2)
                {
                    br.Success = false;
                    br.Message.Add("多包装商品单位重复");
                    br.Level = ErrorLevel.Warning;
                    //br.Data = new { id = item.id, name = "dw", value = item.dw };
                    br.Data = new { id = "", name = "", value = "" };
                    return br;
                }

            }
            #endregion

            #endregion

            br.Message.Clear();
            br.Success = true;
            return br;
        }
        #endregion

        #region GetMaxBarcodeInfo

        public BaseResult GetMaxBarcodeInfo(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            if (!param.ContainsKey("dh_cz"))
                param.Add("dh_cz", PublicSign.dh_cz);
            var data = Tb_ShopspDAL.GetMaxBarcodeInfo(typeof(Tb_Shopsp), param);
            if (data == 0)
            {
                res.Data = PublicSign.dh_cz + "00000";
                return res;
            }
            res.Data = data.ToString();
            return res;
        }
        #endregion

        #region GetPageListForPs

        public PageNavigate GetPageListForPs(Hashtable param)
        {
            PageNavigate pn;
            List<Tb_Shopsp_Query_For_Ps> list = new List<Tb_Shopsp_Query_For_Ps>();


            if (param.ContainsKey("id_spfl") && !string.IsNullOrEmpty(param["id_spfl"].ToString()) && param["id_spfl"].ToString() != "0")
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", param["id_spfl"].ToString());
                var spflModel = DAL.GetItem<Tb_Spfl>(typeof(Tb_Spfl), ht);
                if (spflModel != null && !string.IsNullOrEmpty(spflModel.id) && !string.IsNullOrEmpty(spflModel.path))
                {
                    param.Remove("id_spfl");
                    param.Add("spfl_path", spflModel.path + "%/");
                }
            }

            param["id_shop_ck"] = string.Format("'{0}'", param["id_shop_ck"].ToString().SQLFilterStr());

            pn = new PageNavigate() { TotalCount = DAL.GetCount(typeof(Tb_Shopsp), param) };
            if (pn.TotalCount > 0)
            {
                list = Tb_ShopspDAL.GetPageListForPs(typeof(Tb_Shopsp), param).ToList();
            }
            pn.Data = list;
            pn.Success = true;
            return pn;
        }
        #endregion

        #region GetShopspDwList
        public BaseResult GetShopspDwList(Hashtable param)
        {
            BaseResult br = new BaseResult();
            var sp = Tb_ShopspDAL.GetShopspDwList(typeof(Tb_Shopsp), param);
            br.Data = sp;
            br.Success = true;
            return br;
        }
        #endregion


        #region 商品列表
        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPageSel(Hashtable param)
        {
            PageNavigate pn;
            List<Tb_Shopsp_Query> list = new List<Tb_Shopsp_Query>();

            if (param.ContainsKey("id_spfl") && !string.IsNullOrEmpty(param["id_spfl"].ToString()) && param["id_spfl"].ToString() != "0")
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", param["id_spfl"].ToString());
                var spflModel = DAL.GetItem<Tb_Spfl>(typeof(Tb_Spfl), ht);
                if (spflModel != null && !string.IsNullOrEmpty(spflModel.id) && !string.IsNullOrEmpty(spflModel.path))
                {
                    param.Remove("id_spfl");
                    param.Add("spfl_path", spflModel.path + "%/");
                }
            }

            pn = new PageNavigate() { TotalCount = DAL.GetCount(typeof(Tb_Shopsp), param) };

            if (pn.TotalCount > 0)
            {
                list = Tb_ShopspDAL.QueryPageSel<Tb_Shopsp_Query>(typeof(Tb_Shopsp), param).ToList() ?? new List<Tb_Shopsp_Query>();
            }
            pn.Data = list;
            pn.Success = true;
            return pn;
        }
        #endregion



    }
}