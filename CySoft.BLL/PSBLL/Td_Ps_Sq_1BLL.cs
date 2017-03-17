using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.IDAL;
using CySoft.Model.Enums;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Model.Td;
using CySoft.Utility;

namespace CySoft.BLL.PSBLL
{
    public class Td_Ps_Sq_1BLL : BaseBLL
    {
        public ITb_ShopspDAL Tb_ShopspDAL { get; set; }

        #region 获取分页数据
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Td_Ps_Sq_1), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Td_Ps_Sq_1_Query>(typeof(Td_Ps_Sq_1), param);
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
            var model = entity as PssqModel;
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
            if (DAL.GetCount(typeof(Td_Ps_Sq_1), _ck_parm) > 0)
            {
                res.Success = false;
                res.Message.Add("单号已存在!");
                return res;
            }
            if (string.IsNullOrEmpty(model.id_shop))
            {
                res.Success = false;
                res.Message.Add("请选择申请门店!");
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
            #endregion
            Td_Ps_Sq_1 pssq1 = new Td_Ps_Sq_1()
            {
                #region 赋值
                id_masteruser = model.id_masteruser,
                id = GetGuid,
                dh = model.dh.Trim(),
                rq = model.rq,
                id_shop = model.id_shop,
                bm_djlx = Enums.Ps.PS010.ToString(),
                bm_djlx_origin =model.bm_djlx_origin?? "",
                id_bill_origin =model.id_bill_origin?? "",
                dh_origin =model.dh_origin?? "", 
                id_jbr = model.id_jbr,
                je_mxtotal = model.je_mxtotal,
                flag_sh = (byte)Enums.FlagSh.UnSh,
                flag_cancel = (byte)Enums.FlagCancel.NoCancel,
                bz = string.Format("{0}", model.remark).Trim(),
                id_create = model.id_create,
                rq_create = DateTime.Now 
                #endregion
            };
            model.id = pssq1.id; 
            List<Td_Ps_Sq_2> pssq2S = new List<Td_Ps_Sq_2>();
            if (!string.IsNullOrEmpty(model.json_data))
            {
                pssq2S = JSON.Deserialize<List<Td_Ps_Sq_2>>(model.json_data);
                if (pssq2S.Any())
                {
                    int sort = 1;
                    pssq2S.ForEach(a =>
                    {
                        a.id = GetGuid;
                        a.id_masteruser = pssq1.id_masteruser;
                        a.id_bill = pssq1.id;
                        a.rq_create = DateTime.Now;
                        a.sort_id = sort++;
                        CheckModel2(res, a);
                    });
                }
            }
            if (!pssq2S.Any())
            {
                res.Success = false;
                res.Message.Add("请选择商品！");
                return res;
            }
            if (res.Success == false)
            {
                var first = res.Message.First();
                res.Message.Clear();
                res.Message.Add(first);
                return res;
            }
            var shopModel= QueryShopById(pssq1.id_shop);
            if (shopModel == null || shopModel.id_shop_ps.IsEmpty())
            {
                 res.Success = false;
                 res.Message.Add("申请门店已停用或删除!");
                 return res;
            }
            Hashtable param = new Hashtable(); 
            var id_sps= (from p in pssq2S select p.id_sp).ToArray();
            var id_shops = new string[] { pssq1.id_shop, shopModel.id_shop_ps};
            param.Add("id_shop_array", id_shops);
            param.Add("id_sp_array", id_sps);
            param.Add("id_masteruser", pssq1.id_masteruser);
            var splist= DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), param).ToList();
            pssq2S.ForEach(p =>
            {
                var sps=splist.Where(s => s.id_sp == p.id_sp);
                if (sps.Any()&&sps.Count()>=2)
                {
                    if (sps.Any(a=>a.flag_delete!=(byte)Enums.FlagDelete.NoDelete||a.flag_state!=1))
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
            if (GetPsSktype(model.id_masteruser)!="0")
            {
                var xyed = GetCanUseMoney(model.id_masteruser, model.id_shop);
                if (pssq1.je_mxtotal > xyed)
                {
                    res.Success = false;
                    res.Message.Add(string.Format("此单总金额超过可用信用额度[{0:F}]!", xyed));
                    return res;
                }
            } 
            DAL.Add(pssq1);
            DAL.AddRange(pssq2S);
            if (model.AutoAudit)
            {
                param.Clear();
                param.Add("id_masteruser", model.id_masteruser);
                param.Add("id_user", model.id_create);
                param.Add("id", model.id);
                Sh<Td_Ps_Sq_1>(res, param, "p_ps_sq_sh");
            }
            return res;
        }

        private bool CheckModel2(BaseResult res, Td_Ps_Sq_2 model)
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
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            var pssq1 = DAL.GetItem<Td_Ps_Sq_1>(typeof(Td_Ps_Sq_1), param);
            PssqQueryModel model = new PssqQueryModel();
            if (pssq1 != null)
            {
                param.Clear();
                param.Add("id_bill", pssq1.id);
                param.Add("sort", "  sort_id  ");
                model.Pssq2List = DAL.QueryList<Td_Ps_Sq_2_Query>(typeof(Td_Ps_Sq_2), param).ToList();
                if (!string.IsNullOrEmpty(pssq1.id_create))
                {
                    param.Clear();
                    List<string> arr = new List<string>();
                    arr.Add(pssq1.id_create);
                    if (!string.IsNullOrEmpty(pssq1.id_user_sh))
                    {
                        arr.Add(pssq1.id_user_sh);
                    }
                    param.Add("id_users", arr.ToArray());
                    var users = DAL.QueryList<Tb_User>(typeof(Tb_User), param).ToList();
                    if (users.Any())
                    {
                        var user_zdr = users.FirstOrDefault(a => a.id == pssq1.id_create);
                        if (user_zdr != null) model.zdr = user_zdr.username;
                        if (!string.IsNullOrEmpty(pssq1.id_user_sh))
                        {
                            var user_shr = users.FirstOrDefault(a => a.id == pssq1.id_user_sh);
                            if (user_shr != null) model.shr = user_shr.username;
                        }
                    }
                }
            }
            model.Pssq1 = pssq1;
            res.Data = model;
            return res;
        }

        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult res = new BaseResult() { Success = true };
            var model = entity as PssqModel;
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
            if (string.IsNullOrEmpty(model.id_shop))
            {
                res.Success = false;
                res.Message.Add("请选择申请门店!");
                return res;
            }
            //if (string.IsNullOrEmpty(model.id_shop_sq))
            //{
            //    res.Success = false;
            //    res.Message.Add("请选择申请门店!");
            //    return res;
            //}
            //if (string.IsNullOrEmpty(model.id_shop_ck))
            //{
            //    res.Success = false;
            //    res.Message.Add("请选择配送门店!");
            //    return res;
            //}
            //if (model.id_shop_ck == model.id_shop_sq)
            //{
            //    res.Success = false;
            //    res.Message.Add("申请门店、配送门店不能相同!");
            //    return res;
            //}
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
            if (GetPsSktype(model.id_masteruser)!="0")
            {
                var xyed = GetCanUseMoney(model.id_masteruser, model.id_shop, model.id, Enums.Ps.PS010.ToString());
                if (model.je_mxtotal > xyed)
                {
                    res.Success = false;
                    res.Message.Add(string.Format("此单总金额超过可用信用额度[{0:F}]!", xyed));
                    return res;
                }
            } 
            #endregion
            Hashtable param = new Hashtable();
            #region 赋值
            param.Add("new_bm_djlx_origin", model.bm_djlx_origin ?? "");
            param.Add("new_id_bill_origin", model.id_bill_origin ?? "");
            param.Add("new_dh_origin", model.dh_origin ?? ""); 
            param.Add("new_id_shop", model.id_shop);
            param.Add("new_id_jbr", model.id_jbr);
            param.Add("new_je_mxtotal", model.je_mxtotal);
            param.Add("new_remark", string.Format("{0}", model.remark).Trim());

            param.Add("new_rq", model.rq);
            param.Add("id", model.id);
            param.Add("id_masteruser", model.id_masteruser);
            
            #endregion
            List<Td_Ps_Sq_2> pssq2S = new List<Td_Ps_Sq_2>();
            if (!string.IsNullOrEmpty(model.json_data))
            {
                pssq2S = JSON.Deserialize<List<Td_Ps_Sq_2>>(model.json_data);
                if (pssq2S.Any())
                {
                    int sort = 1;
                    pssq2S.ForEach(a =>
                    {
                        a.id = GetGuid;
                        a.id_masteruser = model.id_masteruser;
                        a.id_bill = model.id;
                        a.rq_create = DateTime.Now;
                        a.sort_id = sort++;
                        CheckModel2(res, a);
                    });
                }
            }
            if (!pssq2S.Any())
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
            var shopModel= QueryShopById(model.id_shop);
            if (shopModel == null || shopModel.id_shop_ps.IsEmpty())
            {
                res.Success = false;
                res.Message.Add("申请门店已停用或删除!");
                return res;
            }
            var id_sps = (from p in pssq2S select p.id_sp).ToArray();
            var id_shops = new string[] { model.id_shop, shopModel.id_shop_ps };
            ht.Add("id_shop_array", id_shops);
            ht.Add("id_sp_array", id_sps);
            ht.Add("id_masteruser", model.id_masteruser);
            var splist = DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), ht).ToList();
            pssq2S.ForEach(p =>
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
            DAL.UpdatePart(typeof(Td_Ps_Sq_1), param);
            param.Clear();
            param.Add("id_bill", model.id);
            param.Add("id_masteruser", model.id_masteruser);
            DAL.Delete(typeof(Td_Ps_Sq_2), param);
            DAL.AddRange(pssq2S);
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
            Sh<Td_Ps_Sq_1>(res, param, "p_ps_sq_sh");
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
            Sh<Td_Ps_Sq_1>(res, param, "p_ps_sq_zf");
            return res;
        }

        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult res=new BaseResult(){Success = true};
            if (param==null
                ||!param.Contains("id")
                ||!param.Contains("id_masteruser")
                ||!param.Contains("id_user"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            var id = string.Format("{0}", param["id"]);
            var id_masteruser = string.Format("{0}", param["id_masteruser"]);
            var id_user=string.Format("{0}", param["id_user"]);
            param.Clear();
            param.Add("id_masteruser",id_masteruser);
            param.Add("id", id);
            var model = DAL.GetItem<Td_Ps_Sq_1>(typeof(Td_Ps_Sq_1), param);
            if (model==null)
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (model.flag_delete != null && model.flag_delete.Value==(byte)Enums.FlagDelete.Deleted)
            {
                res.Success = false;
                res.Message.Add("此数据已删除!");
                return res;
            }
            if (model.flag_sh != null && model.flag_sh.Value==(byte)Enums.FlagSh.HadSh)
            {
                res.Success = false;
                res.Message.Add("此数据已审核，不能删除!");
                return res;
            }
            param.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);
            param.Add("new_id_user_sh", id_user);
            param.Add("flag_sh", (int)Enums.FlagSh.UnSh);
            if (DAL.UpdatePart(typeof(Td_Ps_Sq_1), param)<=0)
            {
                res.Success = false;
                res.Message.Add("此数据已被处理!");
                return res;
            }
            return res;
        }

        public override BaseResult Export(Hashtable param)
        {
            BaseResult br = new BaseResult() {Success = true};
            Hashtable ht = new Hashtable();
            string FilePath = string.Format("{0}", param["filePath"]);
            string id_masteruser = string.Format("{0}", param["id_masteruser"]);
            string id_user = string.Format("{0}", param["id_user"]); 
            string id_shop = string.Format("{0}", param["id_shop"]);  
            string id_shop_ck = string.Format("{0}", param["id_shop_ck"]); 
            List<Ps_Shopsp_Import> list = (List<Ps_Shopsp_Import>)param["list"];
            List<Ps_Shopsp_Import> successList = new List<Ps_Shopsp_Import>();
            List<Ps_Shopsp_Import> failList = new List<Ps_Shopsp_Import>();

            #region 验证导入数据是否空
            if (list == null || list.Count() <= 0)
            {
                br.Message.Add(String.Format("操作失败,没有数据!"));
                br.Success = false;
                br.Data = new PsShopsp_Import_All() { SuccessList = successList, FailList = list, AllList = list };
                return br;
            }
            #endregion

            #region 获取属于主用户所有门店商品
            ht.Clear();
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("id_shop",id_shop);
            ht.Add("id_shop_ck", string.Format("'{0}'", id_shop_ck));
            var shopspList = Tb_ShopspDAL.GetPageListForPs(typeof(Tb_Shopsp), ht);
            if (shopspList == null || !shopspList.Any())
            {
                br.Message.Add(String.Format("操作失败,未查询到用户的商品数据!"));
                br.Success = false;
                br.Data = new PsShopsp_Import_All() { SuccessList = successList, FailList = list, AllList = list };
                return br;
            }
            #endregion
            #region 验证导入的数据是否符合要求并相应简单处理赋值

            foreach (var item in list)
            {
                #region 验证必要参数是否符合
                int error = 0;
                if (string.IsNullOrEmpty(item.barcode))
                {
                    item.sysbz += "   条形码不能为空  ";
                    error++;
                }

                if (!CyVerify.IsNumeric(item.sl) || item.sl == 0)
                {
                    item.sysbz += "   数量不符合要求  ";
                    error++;
                }

                if (item.dj != null && !CyVerify.IsNumeric(item.dj))
                {
                    item.sysbz += "   单价不符合要求  ";
                    error++;
                }

                if (error > 0) continue;
                #endregion

                #region 验证数据库是否存在数据并简单赋值
                var dbInfo = shopspList.Where(d => d.barcode == item.barcode.Trim() && d.flag_delete == (byte)Enums.FlagDelete.NoDelete && d.id_shop == id_shop).FirstOrDefault();
                if (dbInfo == null)
                {
                    item.sysbz += "   不存在此条形码的商品  ";
                    error++;
                    continue;
                }
                else
                {
                    if (dbInfo.flag_delete == (byte)Enums.FlagShopspStop.Deleted)
                    {
                        item.sysbz += "   此条形码的商品已被删除  ";
                        error++;
                        continue;
                    }
                    if (dbInfo.flag_state == (byte)Enums.FlagShopspStop.Stoped)
                    {
                        item.sysbz += "   此条形码的商品已被停用  ";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(item.mc))
                        item.mc = dbInfo.mc;
                    if (item.dj == null || item.dj == 0)
                        item.dj = dbInfo.dj_jh.Value;
                    item.id_shopsp = dbInfo.id;
                    item.barcode = dbInfo.barcode;
                    item.bm = dbInfo.bm;
                    item.mc = dbInfo.mc;
                    item.id_shop = dbInfo.id_shop;
                    item.id_spfl = dbInfo.id_spfl;
                    item.dw = dbInfo.dw;
                    item.dj_jh = item.dj;
                    if (dbInfo.dj_ls != null) item.dj_ls = dbInfo.dj_ls.Value;
                    item.id_kcsp = dbInfo.id_kcsp;
                    if (dbInfo.zhl != null) item.zhl = dbInfo.zhl.Value;
                    item.sl_qm = dbInfo.sl_qm;
                    item.id_sp = dbInfo.id_sp;
                    item.id_kcsp_ck = dbInfo.id_kcsp_ck;
                    item.id_shopsp_ck=dbInfo.id_shopsp_ck;
                    if (dbInfo.dj_ps != null) item.dj_ps = dbInfo.dj_ps.Value;
                }
                #endregion
            }
            #endregion
            failList = list.Where(d => d.sysbz != "").ToList();
            successList = list.Where(d => d.sysbz == "").ToList();

            br.Message.Add(String.Format("操作完毕!"));
            br.Success = true;
            br.Data = new PsShopsp_Import_All() { SuccessList = successList, FailList = failList, AllList = list };
            return br;
        }

    }
}
