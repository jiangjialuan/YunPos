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
using CySoft.IDAL;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Utility;

//快速盘点
namespace CySoft.BLL.BusinessBLL
{

    public class Td_Kc_Kspd_1BLL : BaseBLL
    {
        public ITb_ShopspDAL Tb_ShopspDAL { get; set; }
        public ITd_Kc_Kspd_2DAL Td_Kc_Kspd_2DAL { get; set; }
        private void AdminAdd(BaseResult br,KspdModel model)
        {
            #region 参数验证
            if (model == null || string.IsNullOrEmpty(model.id_masteruser))
            {
                br.Success = false;
                br.Message.Add("参数有误!");
                return;
            }
            if (string.IsNullOrEmpty(model.dh))
            {
                br.Success = false;
                br.Message.Add("单号不能为空!");
                return;
            }
            if (string.IsNullOrEmpty(model.id_shop))
            {
                br.Success = false;
                br.Message.Add("请选择制单门店!");
                return;
            }
            if (string.IsNullOrEmpty(model.id_jbr))
            {
                br.Success = false;
                br.Message.Add("请选择经办人!");
                return;
            }
            if (string.IsNullOrWhiteSpace(model.json_data))
            {
                br.Success = false;
                br.Message.Add("请选择商品!");
                return;
            } 
            #endregion
            Td_Kc_Kspd_1 entity=new Td_Kc_Kspd_1();
            #region 
            entity.id_masteruser = model.id_masteruser;
            entity.id = GetGuid;
            entity.dh = model.dh;
            entity.rq = model.rq;
            entity.id_shop = model.id_shop;
            entity.bm_djlx = Enums.Kspd.KC010.ToString();
            entity.id_jbr = model.id_jbr;
            entity.flag_sh = (byte)Enums.FlagSh.UnSh;
            entity.flag_cancel = (byte)Enums.FlagCancel.NoCancel;
            entity.je_yk_mxtotal = model.je_yk_mxtotal;
            entity.bz = model.bz;
            entity.id_create = model.id_create;
            entity.rq_create = DateTime.Now;
            entity.flag_delete = (byte)Enums.FlagDelete.NoDelete; 
            #endregion

            var kspdList2= JSON.Deserialize<List<Td_Kc_Kspd_2>>(model.json_data);
            #region 验证表体数据
            if (!kspdList2.Any())
            {
                br.Success = false;
                br.Message.Add("请选择商品!");
                return;
            }
            var sort_id = 1;
            kspdList2.ForEach(a =>
            {
                a.id_masteruser = entity.id_masteruser;
                a.id_bill = entity.id;
                a.id = GetGuid;
                a.sort_id = sort_id;
                a.rq_create = entity.rq_create;
                CheckKspd2(a, br);
                sort_id++;
            });
            if (br.Message.Any())
            {
                var firstError = br.Message.FirstOrDefault();
                br.Message.Clear();
                br.Message.Add(firstError);
                return;
            }
            var groupList= kspdList2.GroupBy(a => a.id_kcsp,a=>a).ToList();
            if (groupList.Any())
            {
                var item= groupList.FirstOrDefault(a => a.Count() > 1);
                if (item!=null)
                {
                    var sameSpList= kspdList2.Where(k => k.id_kcsp == item.Key).ToList();
                    br.Success = false;
                    br.Message.Clear();
                    br.Message.Add(string.Format("第{0}行商品与第{1}行商品是同一库存商品，不能重复盘点!", sameSpList[0].sort_id, sameSpList[1].sort_id));
                    return;
                }
            }
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", model.id_masteruser);
            param.Add("id_shop", model.id_shop);
            var id_kcsp_list= (from k in kspdList2 select k.id_kcsp).ToArray();
            param.Add("id_kcsp_list", id_kcsp_list);
            var list= Td_Kc_Kspd_2DAL.QureyKspd2LeftJoinKspd1(typeof(Td_Kc_Kspd_2), param);
            if (list.Any())
            {
                var first= list.FirstOrDefault();
                if (first!=null)
                {
                    var hadsp= kspdList2.FirstOrDefault(k => k.id_kcsp == first.id_kcsp);
                    if (hadsp!=null)
                    {
                        br.Success = false;
                        br.Message.Clear();
                        br.Message.Add(string.Format("在其他未审核盘点单中已存在第{0}行商品!", hadsp.sort_id));
                        return;
                    }
                }
            }
            #endregion
            DAL.Add(entity);
            DAL.AddRange(kspdList2);
            if (model.AutoAudit)
            {
                param.Clear();
                param.Add("id_masteruser", model.id_masteruser);
                param.Add("id_user", model.id_create);
                param.Add("id", entity.id);
                Sh<Td_Kc_Kspd_1>(br, param, "p_kc_kspd_sh");
            }
            br.Success = true;
        }

        private void CheckKspd2(Td_Kc_Kspd_2 model,BaseResult br)
        {
            if (
                string.IsNullOrEmpty(model.id_shopsp)
                || string.IsNullOrEmpty(model.id_sp)
                || string.IsNullOrEmpty(model.id_kcsp)
                ||model.zhl<=0
                ||string.IsNullOrEmpty(model.barcode))
            {
                br.Success = false;
                br.Message.Add(string.Format("第{0}行商品数据异常",model.sort_id));
                return;
            }
            if (model.sl<0)
            {
                br.Success = false;
                br.Message.Add(string.Format("第{0}行商品实盘数量不能为0", model.sort_id));
                return;
            }
            if (model.sl_total <0)
            {
                br.Success = false;
                br.Message.Add(string.Format("第{0}行商品总数量不能为0", model.sort_id));
                return;
            }
        }


        public override BaseResult Export(Hashtable param)
        {
            //BaseResult res =new BaseResult(){Success = true};
            #region 获取数据
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            string FilePath = param["filePath"].ToString();
            string id_masteruser = param["id_masteruser"].ToString();
            string id_user = param["id_user"].ToString();
            string id_shop = param["id_shop"].ToString();
            List<KspdShopsp_Import> list = (List<KspdShopsp_Import>)param["list"];
            List<KspdShopsp_Import> successList = new List<KspdShopsp_Import>();
            List<KspdShopsp_Import> failList = new List<KspdShopsp_Import>();
            #endregion
            #region 验证导入数据是否空
            if (list == null || !list.Any())
            {
                br.Message.Add(String.Format("操作失败,没有数据!"));
                br.Success = false;
                br.Data = new KspdShopsp_Import_All() { SuccessList = successList, FailList = list, AllList = list };
                return br;
            }
            #endregion
            #region
            ht.Clear();
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("id_shop",id_shop);
            var shopspList = Tb_ShopspDAL.GetShopspList(typeof(Tb_Shopsp), ht);
            if (shopspList == null || !shopspList.Any())
            {
                br.Message.Add(String.Format("操作失败,未查询到用户的商品数据!"));
                br.Success = false;
                br.Data = new KspdShopsp_Import_All() { SuccessList = successList, FailList = list, AllList = list };
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

                    #region 
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
                    item.id_kcsp = dbInfo.id_kcsp;
                    if (dbInfo.zhl != null) item.zhl = dbInfo.zhl.Value;
                    item.sl_qm = dbInfo.sl_qm;
                    item.id_sp = dbInfo.id_sp;
                    item.sysbz = ""; 
                    #endregion
                }
                #endregion
               
            }
            #endregion
            failList = list.Where(d => !d.sysbz.IsEmpty()).ToList();
            successList = list.Where(d => d.sysbz.IsEmpty()).ToList();

            br.Message.Add(String.Format("操作完毕!"));
            br.Success = true;
            br.Data = new KspdShopsp_Import_All() { SuccessList = successList, FailList = failList, AllList = list };
            return br;
        }

        /// <summary>
        /// 快速盘点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            #region 获取参数
            BaseResult br = new BaseResult();
            var kspdModel= entity as KspdModel;
            if (kspdModel!=null)
            {
                AdminAdd(br, kspdModel);
                return br;
            }

            Hashtable param = entity as Hashtable;

            DateTime rq = DateTime.Now;

            if (param == null ||
                param["id_shop"] == null || string.IsNullOrEmpty(param["id_shop"].ToString()) ||
                param["id_masteruser"] == null || string.IsNullOrEmpty(param["id_masteruser"].ToString()) ||
                param["json_param"] == null || string.IsNullOrEmpty(param["json_param"].ToString()) ||
                param["id_jbr"] == null || string.IsNullOrEmpty(param["id_jbr"].ToString()) ||
                param["id_create"] == null || string.IsNullOrEmpty(param["id_create"].ToString()) ||
                param["rq"] == null || string.IsNullOrEmpty(param["rq"].ToString()) || !DateTime.TryParse(param["rq"].ToString(), out rq) ||
                param["sign"] == null || string.IsNullOrEmpty(param["sign"].ToString())
               )
            {
                br.Success = false;
                br.Message.Add("必要参数不可以为空.");
                return br;
            }
            #endregion

            #region 构建表体
            var body = Utility.JSON.Deserialize<List<Td_Kc_Kspd_2>>(param["json_param"].ToString());

            if (body == null || body.Count() <= 0)
            {
                br.Success = false;
                br.Message.Add("必要参数不符合要求.");
                return br;
            }
            #endregion

            #region 构建表头
            var head = new Td_Kc_Kspd_1()
            {
                id_masteruser = param["id_masteruser"].ToString(),
                id = GetGuid,
                dh = GetNewDH(param["id_masteruser"].ToString(), param["id_shop"].ToString(), Enums.FlagDJLX.DHKSPD),
                rq = rq,
                id_shop = param["id_shop"].ToString(),
                bm_djlx = "KC010",
                id_jbr = param["id_jbr"].ToString(),
                je_yk_mxtotal = 0,
                rq_sh = null,
                flag_sh = 0,
                id_user_sh = "",
                flag_cancel = 0,
                bz = param["bz"] == null ? "" : param["bz"].ToString(),
                id_create = param["id_create"].ToString(),
                rq_create = DateTime.Now,
                id_edit = "",
                rq_edit = null,
                flag_delete = 0
            };
            #endregion

            #region 表体验证以及赋值
            foreach (var item in body)
            {
                string message = string.Empty;
                if (this.ParamError(item, out message) || !string.IsNullOrEmpty(message))
                {
                    br.Success = false;
                    br.Message.Clear();
                    br.Message.Add(message);
                    return br;
                }
                else
                {
                    item.id_masteruser = head.id_masteruser;
                    item.id = GetGuid;
                    item.id_bill = head.id;
                    item.rq_create = head.rq_create;
                    if (item.sl == null || item.zhl == null)
                        item.sl_total = null;
                    else
                        item.sl_total = item.zhl * item.sl;

                }
            }

            head.je_yk_mxtotal = body.Sum(d => d.je_yk);

            #endregion

            #region 插入数据
            DAL.Add(head);
            DAL.AddRange(body);
            #endregion

            #region 执行存储过程并返回结果
            Hashtable ht = new Hashtable();
            ht.Clear();
            ht.Add("proname", "p_kc_kspd_sh");
            ht.Add("errorid", "-1");
            ht.Add("errormessage", "未知错误！");
            ht.Add("id_bill", head.id);
            ht.Add("id_user", head.id_create);
            DAL.RunProcedure(ht);

            if (!ht.ContainsKey("errorid") || !ht.ContainsKey("errormessage"))
            {
                br.Success = false;
                br.Message.Clear();
                br.Message.Add("审核失败,执行审核出现异常!");
                throw new CySoftException(br);
            }

            if (!string.IsNullOrEmpty(ht["errorid"].ToString()) || !string.IsNullOrEmpty(ht["errormessage"].ToString()))
            {
                br.Success = false;
                br.Message.Clear();
                br.Message.Add("审核失败,  " + ht["errormessage"].ToString());
                throw new CySoftException(br);
            }

            #endregion

            #region 返回
            br.Success = true;
            br.Message.Clear();
            br.Message.Add(string.Format("操作成功!"));
            br.Data = new { id = head.id, dh = head.dh };
            return br;
            #endregion
        }


        #region 初步检验表体数据
        /// <summary>
        /// 初步检验表体数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ParamError(Td_Kc_Kspd_2 model, out string message)
        {
            message = "";
            if (model == null)
            {
                message = "参数不能为空!";
                return true;
            }
            else
            {
                if (!CyVerify.IsNumeric(model.sort_id) || model.sort_id <= 0)
                {
                    message = "序号不符合要求!";
                    return true;
                }
                else if (string.IsNullOrEmpty(model.id_shopsp))
                {
                    message = "商品id不符合要求!";
                    return true;
                }
                else if (string.IsNullOrEmpty(model.id_kcsp))
                {
                    message = "库存id不符合要求!";
                    return true;
                }
                else if (string.IsNullOrEmpty(model.barcode))
                {
                    message = "条码不符合要求!";
                    return true;
                }
                else if (string.IsNullOrEmpty(model.dw))
                {
                    message = "单位不符合要求!";
                    return true;
                }
                return false;
            }
        }
        #endregion

        public override BaseResult Get(Hashtable param)
        {
            BaseResult res=new BaseResult(){Success = true};
            if (
                param == null 
                || !param.Contains("id_masteruser"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            var copy = string.Format("{0}", param["copy"]);
            KspdQueryModel queryModel=new KspdQueryModel();
            queryModel.model1= DAL.GetItem<Td_Kc_Kspd_1>(typeof(Td_Kc_Kspd_1), param);
            if (queryModel.model1 != null)
            {
                param.Clear();
                param.Add("id_bill", queryModel.model1.id);
                param.Add("id_masteruser", queryModel.model1.id_masteruser);
                queryModel.model2List = DAL.QueryList<Td_Kc_Kspd_2_Query>(typeof(Td_Kc_Kspd_2), param).ToList();

                if (!string.IsNullOrEmpty(queryModel.model1.id_create))
                {
                    #region 
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
                    #endregion
                }
                if (copy=="copy")
                {
                    param.Clear();
                    param.Add("id_masteruser", queryModel.model1.id_masteruser);
                    param.Add("id_shop",queryModel.model1.id_shop);
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
                                    if (kc.id_kcsp==pd.id_kcsp)
                                    {
                                        pd.sl_kc = kc.sl_qm;
                                        pd.sl_yk = pd.sl_total - kc.sl_qm;
                                        pd.je_yk = pd.dj_jh*pd.sl_yk;
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
            BaseResult res = new BaseResult() {Success = true};
            var model= entity as KspdModel;

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

            Hashtable param=new Hashtable();
            var date = DateTime.Now;
            param.Add("id",model.id);
            param.Add("id_masteruser", model.id_masteruser);
            param.Add("new_id_shop", model.id_shop);
            param.Add("new_id_jbr", model.id_jbr);
            param.Add("new_je_yk_mxtotal", model.je_yk_mxtotal);
            param.Add("new_bz", model.bz);
            param.Add("new_rq_edit", date);
            param.Add("new_id_edit", model.id_edit);
            var kspdList2 = JSON.Deserialize<List<Td_Kc_Kspd_2>>(model.json_data);
            if (!kspdList2.Any())
            {
                res.Success = false;
                res.Message.Add("请选择商品!");
                return res;
            }
            var sort_id = 1;
            kspdList2.ForEach(a =>
            {
                a.id_masteruser = model.id_masteruser;
                a.id_bill = model.id;
                a.id = GetGuid;
                a.sort_id = sort_id;
                a.rq_create = date;
                CheckKspd2(a, res);
                sort_id++;
            });
            if (res.Message.Any())
            {
                var firstError = res.Message.FirstOrDefault();
                res.Message.Clear();
                res.Message.Add(firstError);
                return res;
            }
            DAL.UpdatePart(typeof(Td_Kc_Kspd_1),param);
            Hashtable ht=new Hashtable();
            ht.Add("id_bill",model.id);
            ht.Add("id_masteruser", model.id_masteruser);
            DAL.Delete(typeof(Td_Kc_Kspd_2), ht);
            DAL.AddRange(kspdList2);
            return res;
        }


        public override BaseResult Delete(Hashtable param)
        {
            BaseResult res=new BaseResult(){Success = true};
            if (
                param==null
                ||!param.Contains("id")
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
            param.Add("id",id);
            param.Add("id_masteruser", id_masteruser);
            param.Add("new_flag_delete",(int)Enums.FlagDelete.Deleted);
            DAL.UpdatePart(typeof(Td_Kc_Kspd_1), param);
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
            Sh<Td_Kc_Kspd_1>(res, param, "p_kc_kspd_sh");
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
            Sh<Td_Kc_Kspd_1>(res, param, "p_ps_ck_zf");
            return res;
        }

    }
}
