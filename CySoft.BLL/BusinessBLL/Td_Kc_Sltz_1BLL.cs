using System;
using System.Collections;
using System.Collections.Generic;
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
using CySoft.Model.Tz;
using CySoft.Utility;

namespace CySoft.BLL.BusinessBLL
{
    public class Td_Kc_Sltz_1BLL : BaseBLL 
    {
        public ITb_ShopspDAL Tb_ShopspDAL { get; set; }
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult res=new BaseResult(){Success = true};
            var model= entity as KcsltzModel;
            if (model==null
                ||string.IsNullOrEmpty(model.id_masteruser))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (string.IsNullOrEmpty(model.dh))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (string.IsNullOrEmpty(model.id_shop))
            {
                res.Success = false;
                res.Message.Add("请选择调整门店!");
                return res;
            }
            if (string.IsNullOrEmpty(model.id_jbr))
            {
                res.Success = false;
                res.Message.Add("请选择经办人!");
                return res;
            }
            Td_Kc_Sltz_1 kcSltz1=new Td_Kc_Sltz_1();
            kcSltz1.id_masteruser = model.id_masteruser;
            kcSltz1.id = GetGuid;
            kcSltz1.dh = model.dh;
            kcSltz1.rq = model.rq;
            kcSltz1.id_shop = model.id_shop;
            kcSltz1.bm_djlx =Enums.Kspd.KC020.ToString();
            kcSltz1.id_jbr = model.id_jbr;
            kcSltz1.flag_sh = (byte)Enums.FlagSh.UnSh;
            kcSltz1.flag_cancel = (byte)Enums.FlagCancel.NoCancel;
            kcSltz1.bz = model.bz;
            kcSltz1.id_create = model.id_create;
            kcSltz1.rq_create=DateTime.Now;
            kcSltz1.flag_delete = (byte)Enums.FlagDelete.NoDelete;
            model.id = kcSltz1.id;
            var List2 = JSON.Deserialize<List<Td_Kc_Sltz_2>>(model.json_data);
            if (!List2.Any())
            {
                res.Success = false;
                res.Message.Add("请选择商品!");
                return res;
            }
            var sort_id = 1;
            List2.ForEach(a =>
            {
                a.id_masteruser = model.id_masteruser;
                a.id_bill = kcSltz1.id;
                a.rq_create = kcSltz1.rq_create;
                a.sort_id = sort_id;
                a.id = GetGuid;
                CheckModel2(res,a);
                sort_id++;
            });
            if (res.Message.Any())
            {
                var firstError = res.Message.FirstOrDefault();
                res.Message.Clear();
                res.Message.Add(firstError);
                return res;
            }

            var groupList = List2.GroupBy(a => a.id_kcsp, a => a).ToList();
            if (groupList.Any())
            {
                var item = groupList.FirstOrDefault(a => a.Count() > 1);
                if (item != null)
                {
                    var sameSpList = List2.Where(k => k.id_kcsp == item.Key).ToList();
                    res.Success = false;
                    res.Message.Clear();
                    res.Message.Add(string.Format("第{0}行商品与第{1}行商品是同一库存商品，不能重复调整!", sameSpList[0].sort_id, sameSpList[1].sort_id));
                    return res;
                }
            }

            DAL.Add(kcSltz1);
            DAL.AddRange(List2);
            if (model.AutoAudit)
            {
                Hashtable param = new Hashtable();
                param.Add("id_masteruser", model.id_masteruser);
                param.Add("id_user", model.id_create);
                param.Add("id", entity.id);
                Sh<Td_Kc_Sltz_1>(res, param, "p_kc_sltz_sh");
            }
            return res;
        }

        private void CheckModel2(BaseResult br,Td_Kc_Sltz_2 model)
        {
            if (string.IsNullOrEmpty(model.id_shopsp)
                ||string.IsNullOrEmpty(model.id_kcsp)
                || string.IsNullOrEmpty(model.id_sp)
                ||(model.zhl==null||model.zhl<=0))
            {
                br.Success = false;
                br.Message.Add(string.Format("第{0}行商品数据异常",model.sort_id));
                return;
            }
            if (model.sl==null||model.sl==0)
            {
                br.Success = false;
                br.Message.Add(string.Format("请填写，第{0}行商品数量", model.sort_id));
                return;
            }

        }

        public override BaseResult Delete(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            if (
                param == null
                || !param.Contains("id")
                || !param.Contains("id_masteruser")
                )
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            var id = string.Format("{0}", param["id"]);
            var id_masteruser = string.Format("{0}", param["id_masteruser"]);
            param.Clear();
            param.Add("id", id);
            param.Add("id_masteruser", id_masteruser);
            param.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);
            DAL.UpdatePart(typeof(Td_Kc_Sltz_1), param);
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
            Sh<Td_Kc_Sltz_1>(res, param, "p_kc_sltz_sh");
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
            Sh<Td_Kc_Sltz_1>(res, param, "");
            return res;
        }

        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            if (
                param == null
                || !param.Contains("id_masteruser"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            var copy = string.Format("{0}", param["copy"]);
            KcsltzdQueryModel queryModel = new KcsltzdQueryModel();
            queryModel.model1 = DAL.GetItem<Td_Kc_Sltz_1>(typeof(Td_Kc_Sltz_1), param);
            if (queryModel.model1 != null)
            {
                param.Clear();
                param.Add("id_bill", queryModel.model1.id);
                param.Add("id_masteruser", queryModel.model1.id_masteruser);
                queryModel.model2List = DAL.QueryList<Td_Kc_Sltz_2_Query>(typeof(Td_Kc_Sltz_2), param).ToList();
                if (!string.IsNullOrEmpty(queryModel.model1.id_create))
                {
                    param.Clear();
                    List<string> arr = new List<string>();
                    arr.Add(queryModel.model1.id_create);
                    if (!string.IsNullOrEmpty(queryModel.model1.id_user_sh))
                    {
                        arr.Add(queryModel.model1.id_user_sh);
                    }
                    param.Add("id_users", arr.ToArray());
                    var users = DAL.QueryList<Tb_User>(typeof(Tb_User), param).ToList();
                    if (users.Any())
                    {
                        var user_zdr = users.FirstOrDefault(a => a.id == queryModel.model1.id_create);
                        if (user_zdr != null) queryModel.zdr = user_zdr.username;
                        if (!string.IsNullOrEmpty(queryModel.model1.id_user_sh))
                        {
                            var user_shr = users.FirstOrDefault(a => a.id == queryModel.model1.id_user_sh);
                            if (user_shr != null) queryModel.shr = user_shr.username;
                        }
                    }
                }
                if (copy == "copy")
                {
                    param.Clear();
                    param.Add("id_masteruser", queryModel.model1.id_masteruser);
                    param.Add("id_shop", queryModel.model1.id_shop);
                    if (queryModel.model2List.Any())
                    {
                        var id_kcsp_array = (from l in queryModel.model2List select l.id_kcsp).ToArray();
                        param.Add("id_kcsp_array", id_kcsp_array);
                        var kcspList = DAL.QueryList<Tz_Sp_Kc>(typeof(Tz_Sp_Kc), param).ToList();
                        if (kcspList.Any())
                        {
                            kcspList.ForEach(kc =>
                            {
                                queryModel.model2List.ForEach(pd =>
                                {
                                    if (kc.id_kcsp == pd.id_kcsp)
                                    {
                                        pd.sl_kc = kc.sl_qm;
                                        pd.je_cb = 0;
                                    }
                                });
                            });
                        }
                    }
                }
            }
            res.Data = queryModel;
            return res;
        }

        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult res = new BaseResult() { Success = true };
            var model = entity as KcsltzModel;

            #region 参数验证
            if (
                    model == null
                    || string.IsNullOrEmpty(model.id)
                    || string.IsNullOrEmpty(model.id_masteruser)
                    )
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (string.IsNullOrEmpty(model.dh))
            {
                res.Success = false;
                res.Message.Add("单号不能为空!");
                return res;
            }
            if (string.IsNullOrEmpty(model.id_shop))
            {
                res.Success = false;
                res.Message.Add("请选择制单门店!");
                return res;
            }
            if (string.IsNullOrEmpty(model.id_jbr))
            {
                res.Success = false;
                res.Message.Add("请选择经办人!");
                return res;
            }
            if (string.IsNullOrWhiteSpace(model.json_data))
            {
                res.Success = false;
                res.Message.Add("请选择商品!");
                return res;
            }
            #endregion

            Hashtable param = new Hashtable();
            var date = DateTime.Now;
            param.Add("id", model.id);
            param.Add("id_masteruser", model.id_masteruser);
            param.Add("new_id_shop", model.id_shop);
            param.Add("new_id_jbr", model.id_jbr);
            param.Add("new_bz", model.bz);
            param.Add("new_rq_edit", date);
            param.Add("new_id_edit", model.id_edit);
            var List2 = JSON.Deserialize<List<Td_Kc_Sltz_2>>(model.json_data);
            if (!List2.Any())
            {
                res.Success = false;
                res.Message.Add("请选择商品!");
                return res;
            }
            var sort_id = 1;
            List2.ForEach(a =>
            {
                a.id_masteruser = model.id_masteruser;
                a.id_bill = model.id;
                a.id = GetGuid;
                a.sort_id = sort_id;
                a.rq_create = date;
                CheckModel2(res,a);
                sort_id++;
            });
            if (res.Message.Any())
            {
                var firstError = res.Message.FirstOrDefault();
                res.Message.Clear();
                res.Message.Add(firstError);
                return res;
            }
            DAL.UpdatePart(typeof(Td_Kc_Sltz_1), param);
            Hashtable ht = new Hashtable();
            ht.Add("id_bill", model.id);
            ht.Add("id_masteruser", model.id_masteruser);
            DAL.Delete(typeof(Td_Kc_Sltz_2), ht);
            DAL.AddRange(List2);
            return res;
        }

        public override BaseResult Export(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            #region 获取数据
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            string FilePath = param["filePath"].ToString();
            string id_masteruser = param["id_masteruser"].ToString();
            string id_user = param["id_user"].ToString();
            string id_shop = param["id_shop"].ToString();
            List<KcsltzShopsp_Import> list = (List<KcsltzShopsp_Import>)param["list"];
            List<KcsltzShopsp_Import> successList = new List<KcsltzShopsp_Import>();
            List<KcsltzShopsp_Import> failList = new List<KcsltzShopsp_Import>();
            #endregion
            #region 验证导入数据是否空
            if (list == null || !list.Any())
            {
                br.Message.Add(String.Format("操作失败,没有数据!"));
                br.Success = false;
                br.Data = new KcsltzShopsp_Import_All() { SuccessList = successList, FailList = list, AllList = list };
                return br;
            }
            #endregion
            #region
            ht.Clear();
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("id_shop", id_shop);
            var shopspList = Tb_ShopspDAL.GetShopspList(typeof(Tb_Shopsp), ht);
            if (shopspList == null || !shopspList.Any())
            {
                br.Message.Add(String.Format("操作失败,未查询到用户的商品数据!"));
                br.Success = false;
                br.Data = new KcsltzShopsp_Import_All() { SuccessList = successList, FailList = list, AllList = list };
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

                if (error > 0) continue;
                #endregion

                #region 验证数据库是否存在数据并简单赋值
                var dbInfo = shopspList.FirstOrDefault(d => d.barcode == item.barcode);
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

                    item.mc = dbInfo.mc;
                    item.id_shopsp = dbInfo.id;
                    item.barcode = dbInfo.barcode;
                    item.bm = dbInfo.bm;
                    item.mc = dbInfo.mc;
                    item.id_shop = dbInfo.id_shop;
                    item.id_spfl = dbInfo.id_spfl;
                    item.dw = dbInfo.dw;
                    if (dbInfo.dj_jh != null) item.dj_jh = dbInfo.dj_jh.Value;
                    if (dbInfo.dj_ls != null) item.dj_ls = dbInfo.dj_ls.Value;
                    item.dj_cb = dbInfo.dj_cb;
                    item.id_kcsp = dbInfo.id_kcsp;
                    if (dbInfo.zhl != null) item.zhl = dbInfo.zhl.Value;
                    item.sl_qm = dbInfo.sl_qm;
                    item.id_sp = dbInfo.id_sp;
                    item.sysbz = "";
                }
                #endregion
                
            }
            #endregion
            failList = list.Where(d => !d.sysbz.IsEmpty()).ToList();
            successList = list.Where(d => d.sysbz.IsEmpty()).ToList();

            br.Message.Add(String.Format("操作完毕!"));
            br.Success = true;
            br.Data = new KcsltzShopsp_Import_All() { SuccessList = successList, FailList = failList, AllList = list };
            return br;
        }

    }
}
