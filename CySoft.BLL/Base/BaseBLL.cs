using System;
using System.Collections;
using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using System.Collections.Generic;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using System.Data;
using CySoft.Model.Enums;
using System.Text;
using CySoft.Utility;
using System.Linq;
using CySoft.Model.Ts;
using CySoft.Model;
using System.Web;
using System.IO;
using CySoft.Model.Tz;

namespace CySoft.BLL.Base
{
    public abstract class BaseBLL : AbstractBaseBLL, IBaseBLL
    {
        public virtual BaseResult Add(dynamic entity)
        {
            throw new NotImplementedException();
        }

        public virtual BaseResult Delete(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual BaseResult Update(dynamic entity)
        {
            throw new NotImplementedException();
        }

        public virtual BaseResult Get(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual BaseResult GetAll(Hashtable param = null)
        {
            throw new NotImplementedException();
        }

        public virtual BaseResult GetCount(Hashtable param = null)
        {
            throw new NotImplementedException();
        }

        public virtual PageNavigate GetPage(Hashtable param = null)
        {
            throw new NotImplementedException();
        }
        public virtual PageNavigate GetPageSel(Hashtable param = null)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TType">对应的MAP文件类型</typeparam>
        /// <typeparam name="TQuery">查询返回的对象类型</typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual PageNavigate GetPage<TType, TQuery>(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(TType), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<TQuery>(typeof(TType), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }

        public virtual BaseResult Save(dynamic entity)
        {
            throw new NotImplementedException();
        }

        public virtual BaseResult Export(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual BaseResult Stop(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual BaseResult Active(Hashtable param)
        {
            throw new NotImplementedException();
        }
        public virtual BaseResult Init(Hashtable param)
        {
            throw new NotImplementedException();
        }
        public virtual BaseResult CheckStock(Hashtable param)
        {
            throw new NotImplementedException();
        }
        public virtual int isAdmin(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual void Add(Info_User info)
        {
            throw new NotImplementedException();
        }
        public virtual int GetGGCount(Hashtable param)
        {
            throw new NotImplementedException();
        }
        public virtual int GetByID(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public string GetGuid
        {
            get { return Guid.NewGuid().ToString(); }
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="res"></param>
        /// <param name="param"></param>
        /// <param name="procname">存储过程名</param>
        /// <returns></returns>
        protected bool Sh<T>(BaseResult res, Hashtable param, string procname)
        {
            if (param == null
                || !param.ContainsKey("id")
                || !param.ContainsKey("id_masteruser")
                || !param.ContainsKey("id_user")
                || string.IsNullOrEmpty(procname))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return false;
            }
            var id = param["id"];
            var id_masteruser = param["id_masteruser"];
            var id_user = param["id_user"];
            param.Clear();
            param.Add("id", id);
            param.Add("id_masteruser", id_masteruser);
            var promoteModel = DAL.GetItem<T>(typeof(T), param);
            if (promoteModel == null)
            {
                res.Success = false;
                res.Message.Add("数据已不存在!");
                return false;
            }
            param.Add("proname", procname);
            param.Add("errorid", "-1");
            param.Add("errormessage", "未知错误！");
            param.Add("id_bill", id);
            param.Add("id_user", id_user);
            DAL.RunProcedure(param);
            var a = DAL.QueryList<Tz_Ys_Jsc>(typeof(Tz_Ys_Jsc));
            if (!param.ContainsKey("errorid") || !param.ContainsKey("errormessage"))
            {
                res.Success = false;
                res.Message.Add("操作出现异常!");
                throw new CySoftException(res);
            }

            if (!string.IsNullOrEmpty(string.Format("{0}", param["errormessage"]))
                || !string.IsNullOrEmpty(string.Format("{0}", param["errormessage"])))
            {
                res.Success = false;
                res.Message.Add(string.Format("{0}", param["errormessage"]));
                throw new CySoftException(res);
            }
            res.Success = true;
            res.Message.Add("操作成功!");
            return true;
        }




        /// <summary>
        /// 检查会员是否有效
        /// 作者:lz
        /// 日期:2016-10-19
        /// </summary>
        #region public BaseResult CheckHY(object param)
        public virtual BaseResult CheckHY(Hashtable param)
        {
            BaseResult br = new BaseResult();
            Tb_Hy_Detail hyDetailModel = new Tb_Hy_Detail();


            Hashtable newparam = new Hashtable();
            newparam.Add("id", param["id_hy"].ToString());
            newparam.Add("id_masteruser", param["id_masteruser"].ToString());

            //获取会员信息
            Tb_Hy hyModel = DAL.GetItem<Tb_Hy>(typeof(Tb_Hy), newparam);
            if (hyModel == null)
            {
                br.Success = false;
                br.Message.Add(String.Format("会员不存在！"));
                br.Level = ErrorLevel.Warning;
                return br;
            }

            if (hyModel.flag_delete == (byte)Enums.FlagDelete.Deleted)
            {
                br.Success = false;
                br.Message.Add(String.Format("会员已被删除！"));
                br.Level = ErrorLevel.Warning;
                return br;
            }

            //获取会员门店信息
            newparam.Clear();
            newparam.Add("id_hy", param["id_hy"].ToString());
            newparam.Add("id_masteruser", param["id_masteruser"].ToString());


            //此处取是否共享
            var br_Hy_ShopShare = GetHy_ShopShare(param["id_shop"].ToString(), param["id_masteruser"].ToString());// GetHy_ShopShare(param["id_shop_create"].ToString());
            if (!br_Hy_ShopShare.Success)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message = br_Hy_ShopShare.Message;
                return br;
            }

            var param_Hy_ShopShare = (Hashtable)br_Hy_ShopShare.Data;
            newparam.Add("id_shop", param_Hy_ShopShare["id_shop"].ToString());


            var hyShopModel = DAL.GetItem<Tb_Hy_Shop_Query>(typeof(Tb_Hy_Shop), newparam);
            if (hyShopModel == null)
            {
                br.Success = false;
                br.Message.Add(String.Format("门店不存在此会员！"));
                br.Level = ErrorLevel.Warning;
                return br;
            }

            if (hyShopModel.flag_stop == (byte)Enums.FlagStop.Stopped)
            {
                br.Success = false;
                br.Message.Add(String.Format("会员已被停用！"));
                br.Level = ErrorLevel.Warning;
                return br;
            }

            if (!(DateTime.Now >= hyShopModel.rq_b && DateTime.Now <= hyShopModel.rq_e))
            {
                br.Success = false;
                br.Message.Add(String.Format("会员不在有效期内！"));
                br.Level = ErrorLevel.Warning;
                return br;
            }

            if (param.ContainsKey("Type") && param["Type"].ToString() == "XF")
            {
                string db_password = hyModel.password;
                string password = param["password"] == null ? null : param["password"].ToString();

                if (password == null)
                {
                    br.Success = false;
                    br.Message.Add(String.Format("密码参数错误！"));
                    br.Level = ErrorLevel.Warning;
                    return br;
                }
                else
                {
                    //if (db_password.Length == 32)
                    //{
                    //md5 验证
                    //string md5Pwd = Utility.MD5Encrypt.Md5(password.ToString());
                    if (password != db_password)
                    {
                        br.Success = false;
                        br.Message.Add(String.Format("密码不正确！"));
                        br.Level = ErrorLevel.Warning;
                        return br;
                    }
                    //}
                    //else
                    //{
                    //    if (password != db_password)
                    //    {
                    //        br.Success = false;
                    //        br.Message.Add(String.Format("密码不正确！"));
                    //        br.Level = ErrorLevel.Warning;
                    //        return br;
                    //    }
                    //}
                }



            }


            br.Success = true;
            hyDetailModel.Tb_Hy = hyModel;
            hyDetailModel.Tb_Hy_Shop = hyShopModel;
            br.Data = hyDetailModel;
            return br;
        }
        #endregion

        #region 生成条码 或 单号
        /// <summary>
        /// 生成条码 或 单号
        /// lz
        /// 2016-11-07
        /// </summary>
        /// <param name="type">用于生成单号的类型枚举 </param>
        /// <returns></returns>
        public string GetNewDH(string id_masteruser, string id_shop, Enums.FlagDJLX type)
        {
            Hashtable ht = new Hashtable();
            StringBuilder sb = new StringBuilder();
            string timeStamp = string.Format("{0:yyMMddHHmmss}", DateTime.Now);
            var shopHas = GetTbShop(id_shop);
            if (shopHas != null && shopHas.ContainsKey("success") && shopHas.ContainsKey("bm") && shopHas["success"].ToString() == "1" && !string.IsNullOrEmpty(shopHas["bm"].ToString()))
            {
                var bm = shopHas["bm"].ToString();
                Random rd = new Random();
                timeStamp = bm + string.Format("{0:yyMMddHHmmssf}", DateTime.Now) + rd.Next(0, 9).ToString();
            }

            switch (type)
            {
                case Enums.FlagDJLX.BMShop:
                    sb.Append(timeStamp);
                    break;
                case Enums.FlagDJLX.BMShopsp:
                    sb.Append(string.Format("{0:yyMMddHHmmss}", DateTime.Now).GetJYStr());
                    break;
                case Enums.FlagDJLX.DHDH:
                    sb.Append(timeStamp);
                    break;
                case Enums.FlagDJLX.DHJH:
                    sb.Append(timeStamp);
                    break;
                case Enums.FlagDJLX.DHJHFK:
                    sb.Append(timeStamp);
                    break;
                case Enums.FlagDJLX.DHTH:
                    sb.Append(timeStamp);
                    break;
                case Enums.FlagDJLX.DHCZ:
                    var bm = shopHas["bm"].ToString();
                    Random rd = new Random();
                    timeStamp = bm + string.Format("{0:yyMMddHHmmssfff}", DateTime.Now) + rd.Next(0, 9).ToString();
                    sb.Append(timeStamp);
                    break;
                case Enums.FlagDJLX.DHKSPD:
                    sb.Append(timeStamp);
                    break;
                default:
                    sb.Append(timeStamp);
                    break;
            }
            //return sb.ToString().GetJYStr();
            return sb.ToString();
        }
        #endregion

        #region tb_shop表
        /// <summary>
        /// tb_shop表 配置参数
        /// </summary>
        /// <returns></returns>
        public Hashtable GetTbShop(string id_shop)
        {
            if (DataCache.Get(id_shop + "_GetTbShop") != null && ((Hashtable)DataCache.Get(id_shop + "_GetTbShop")).Keys.Count > 0)
                return (Hashtable)DataCache.Get(id_shop + "_GetTbShop");
            else
            {
                Hashtable result = new Hashtable();
                result.Add("bm", "");
                result.Add("success", "0");
                Hashtable ht = new Hashtable();
                ht.Add("id", id_shop);
                var dbShop = DAL.QueryList<Tb_Shop>(typeof(Tb_Shop), ht).FirstOrDefault();
                if (dbShop == null)
                {
                    return result;
                }
                else
                {
                    result["bm"] = dbShop.bm;
                    result["success"] = 1;
                    DataCache.Add(id_shop + "_GetTbShop", result, DateTime.Now.AddDays(1));
                }
                return result;
            }
        }
        #endregion

        #region ClearShopParm
        public void ClearTbShop(string id_shop)
        {
            try
            {
                if (DataCache.Get(id_shop + "_GetTbShop") != null && ((Hashtable)DataCache.Get(id_shop + "_GetTbShop")).Keys.Count > 0)
                    DataCache.Remove(id_shop + "_GetTbShop");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region ts_parm表 配置参数
        /// <summary>
        /// ts_parm表 配置参数
        /// lz 2016-09-06
        /// </summary>
        /// <returns></returns>
        public Hashtable GetParm(string id_user_master)
        {
            if (DataCache.Get(id_user_master + "_GetParm") != null && ((Hashtable)DataCache.Get(id_user_master + "_GetParm")).Keys.Count > 0)
                return (Hashtable)DataCache.Get(id_user_master + "_GetParm");
            else
            {
                Hashtable result = new Hashtable();
                result.Add("je_digit", 2);
                result.Add("sl_digit", 2);
                result.Add("dj_digit", 2);
                result.Add("zk_digit", 2);
                result.Add("hy_shopshare", 0);
                result.Add("spbm_copy_barcode", 0);
                Hashtable ht = new Hashtable();
                ht.Add("get_self_defaul", "1");
                ht.Add("self_id_masteruser", id_user_master);
                var parmList = DAL.QueryList<Ts_Parm>(typeof(Ts_Parm), ht);
                if (parmList != null && parmList.Count() > 0)
                {
                    var jeDigitModel = parmList.Where(d => d.parmcode == "je_digit").OrderByDescending(d => d.id_masteruser).FirstOrDefault() ?? new Ts_Parm();
                    int jeDigit = 2;
                    int.TryParse(jeDigitModel.parmvalue, out jeDigit);
                    var slDigitModel = parmList.Where(d => d.parmcode == "sl_digit").OrderByDescending(d => d.id_masteruser).FirstOrDefault() ?? new Ts_Parm();
                    int slDigit = 2;
                    int.TryParse(slDigitModel.parmvalue, out slDigit);
                    var djDigitModel = parmList.Where(d => d.parmcode == "dj_digit").OrderByDescending(d => d.id_masteruser).FirstOrDefault() ?? new Ts_Parm();
                    int djDigit = 2;
                    int.TryParse(djDigitModel.parmvalue, out djDigit);
                    var zkDigitModel = parmList.Where(d => d.parmcode == "zk_digit").OrderByDescending(d => d.id_masteruser).FirstOrDefault() ?? new Ts_Parm();
                    int zkDigit = 2;
                    int.TryParse(zkDigitModel.parmvalue, out zkDigit);

                    var shopShareModel = parmList.Where(d => d.parmcode == "hy_shopshare").OrderByDescending(d => d.id_masteruser).FirstOrDefault() ?? new Ts_Parm();
                    int shopShare = 0;
                    int.TryParse(shopShareModel.parmvalue, out shopShare);

                    var copyBarcodeModel = parmList.Where(d => d.parmcode == "spbm_copy_barcode").OrderByDescending(d => d.id_masteruser).FirstOrDefault() ?? new Ts_Parm();
                    int spbm_copy_barcode = 0;
                    int.TryParse(copyBarcodeModel.parmvalue, out spbm_copy_barcode);

                    result["je_digit"] = jeDigit;
                    result["sl_digit"] = slDigit;
                    result["dj_digit"] = djDigit;
                    result["zk_digit"] = zkDigit;
                    result["hy_shopshare"] = shopShare;
                    result["spbm_copy_barcode"] = spbm_copy_barcode;

                    DataCache.Add(id_user_master + "_GetParm", result, DateTime.Now.AddDays(1));
                }
                return result;
            }
        }
        #endregion

        #region ts_parm_shop表 配置参数
        /// <summary>
        /// ts_parm_shop表 配置参数
        /// lz 2016-09-06
        /// </summary>
        /// <returns></returns>
        public Hashtable GetShopParm(string id_user_master, string id_shop)
        {
            if (DataCache.Get(id_user_master + "_" + id_shop + "_GetShopParm") != null && ((Hashtable)DataCache.Get(id_user_master + "_" + id_shop + "_GetShopParm")).Keys.Count > 0)
                return (Hashtable)DataCache.Get(id_user_master + "_" + id_shop + "_GetShopParm");
            else
            {
                Hashtable result = new Hashtable();
                result.Add("success", 0);
                result.Add("bill_auto_audit", 0);
                result.Add("bill_auto_audit_need_query", 0);

                result.Add("hy_jfsz_week_val", "");

                result.Add("hy_jfsz_xs_rq_b", "");
                result.Add("hy_jfsz_xs_rq_e", "");
                result.Add("hy_jfsz_day_nbjf", "");
                result.Add("hy_jfsz_xs_je", "");
                result.Add("hy_jfsz_day_val", "");
                result.Add("hy_jfsz_xs_nbjf", "");
                result.Add("hy_jfsz_hysr_lx", "");
                result.Add("hy_jfsz_hysr_nbjf", "");
                result.Add("hy_jfsz_week_nbjf", "");

                result.Add("hy_czje_min_onec", "");
                result.Add("hy_czje_max_onec", "");
                result.Add("hy_czje_max_month", "");

                Hashtable ht = new Hashtable();
                ht.Add("get_self_defaul", "1");
                ht.Add("self_id_masteruser", id_user_master);

                var parmList = DAL.QueryList<Ts_Parm_Shop>(typeof(Ts_Parm_Shop), ht);
                if (parmList != null && parmList.Count() > 0)
                {
                    var shopAutoAuditModel = parmList.Where(d => d.id_shop == id_shop && d.parmcode == "bill_auto_audit").OrderByDescending(d => d.id_masteruser).FirstOrDefault();
                    if (shopAutoAuditModel != null && !string.IsNullOrEmpty(shopAutoAuditModel.id))
                    {
                        int bill_auto_audit = 0;
                        int.TryParse(shopAutoAuditModel.parmvalue, out bill_auto_audit);
                        result["bill_auto_audit"] = bill_auto_audit;
                    }
                    else
                    {
                        result["bill_auto_audit_need_query"] = 1;
                    }


                    var hy_jfsz_week_valModel = parmList.Where(d => d.id_shop == id_shop && d.parmcode == "hy_jfsz_week_val").OrderByDescending(d => d.id_masteruser).FirstOrDefault();
                    if (hy_jfsz_week_valModel != null && !string.IsNullOrEmpty(hy_jfsz_week_valModel.id))
                    {
                        result["hy_jfsz_week_val"] = hy_jfsz_week_valModel.parmvalue;
                    }



                    var hy_jfsz_xs_rq_bModel = parmList.Where(d => d.id_shop == id_shop && d.parmcode == "hy_jfsz_xs_rq_b").OrderByDescending(d => d.id_masteruser).FirstOrDefault();
                    if (hy_jfsz_xs_rq_bModel != null && !string.IsNullOrEmpty(hy_jfsz_xs_rq_bModel.id))
                    {
                        result["hy_jfsz_xs_rq_b"] = hy_jfsz_xs_rq_bModel.parmvalue;
                    }

                    var hy_jfsz_xs_rq_eModel = parmList.Where(d => d.id_shop == id_shop && d.parmcode == "hy_jfsz_xs_rq_e").OrderByDescending(d => d.id_masteruser).FirstOrDefault();
                    if (hy_jfsz_xs_rq_eModel != null && !string.IsNullOrEmpty(hy_jfsz_xs_rq_eModel.id))
                    {
                        result["hy_jfsz_xs_rq_e"] = hy_jfsz_xs_rq_eModel.parmvalue;
                    }

                    var hy_jfsz_day_nbjfModel = parmList.Where(d => d.id_shop == id_shop && d.parmcode == "hy_jfsz_day_nbjf").OrderByDescending(d => d.id_masteruser).FirstOrDefault();
                    if (hy_jfsz_day_nbjfModel != null && !string.IsNullOrEmpty(hy_jfsz_day_nbjfModel.id))
                    {
                        result["hy_jfsz_day_nbjf"] = hy_jfsz_day_nbjfModel.parmvalue;
                    }

                    var hy_jfsz_xs_jeModel = parmList.Where(d => d.id_shop == id_shop && d.parmcode == "hy_jfsz_xs_je").OrderByDescending(d => d.id_masteruser).FirstOrDefault();
                    if (hy_jfsz_xs_jeModel != null && !string.IsNullOrEmpty(hy_jfsz_xs_jeModel.id))
                    {
                        result["hy_jfsz_xs_je"] = hy_jfsz_xs_jeModel.parmvalue;
                    }

                    var hy_jfsz_day_valModel = parmList.Where(d => d.id_shop == id_shop && d.parmcode == "hy_jfsz_day_val").OrderByDescending(d => d.id_masteruser).FirstOrDefault();
                    if (hy_jfsz_day_valModel != null && !string.IsNullOrEmpty(hy_jfsz_day_valModel.id))
                    {
                        result["hy_jfsz_day_val"] = hy_jfsz_day_valModel.parmvalue;
                    }

                    var hy_jfsz_xs_nbjfModel = parmList.Where(d => d.id_shop == id_shop && d.parmcode == "hy_jfsz_xs_nbjf").OrderByDescending(d => d.id_masteruser).FirstOrDefault();
                    if (hy_jfsz_xs_nbjfModel != null && !string.IsNullOrEmpty(hy_jfsz_xs_nbjfModel.id))
                    {
                        result["hy_jfsz_xs_nbjf"] = hy_jfsz_xs_nbjfModel.parmvalue;
                    }

                    var hy_jfsz_hysr_lxModel = parmList.Where(d => d.id_shop == id_shop && d.parmcode == "hy_jfsz_hysr_lx").OrderByDescending(d => d.id_masteruser).FirstOrDefault();
                    if (hy_jfsz_hysr_lxModel != null && !string.IsNullOrEmpty(hy_jfsz_hysr_lxModel.id))
                    {
                        result["hy_jfsz_hysr_lx"] = hy_jfsz_hysr_lxModel.parmvalue;
                    }

                    var hy_jfsz_hysr_nbjfModel = parmList.Where(d => d.id_shop == id_shop && d.parmcode == "hy_jfsz_hysr_nbjf").OrderByDescending(d => d.id_masteruser).FirstOrDefault();
                    if (hy_jfsz_hysr_nbjfModel != null && !string.IsNullOrEmpty(hy_jfsz_hysr_nbjfModel.id))
                    {
                        result["hy_jfsz_hysr_nbjf"] = hy_jfsz_hysr_nbjfModel.parmvalue;
                    }

                    var hy_jfsz_week_nbjfModel = parmList.Where(d => d.id_shop == id_shop && d.parmcode == "hy_jfsz_week_nbjf").OrderByDescending(d => d.id_masteruser).FirstOrDefault();
                    if (hy_jfsz_week_nbjfModel != null && !string.IsNullOrEmpty(hy_jfsz_week_nbjfModel.id))
                    {
                        result["hy_jfsz_week_nbjf"] = hy_jfsz_week_nbjfModel.parmvalue;
                    }

                    var hy_czje_min_onecModel = parmList.Where(d => d.id_shop == id_shop && d.parmcode == "hy_czje_min_onec").OrderByDescending(d => d.id_masteruser).FirstOrDefault();
                    if (hy_czje_min_onecModel != null && !string.IsNullOrEmpty(hy_czje_min_onecModel.id))
                    {
                        result["hy_czje_min_onec"] = hy_czje_min_onecModel.parmvalue;
                    }

                    var hy_czje_max_onecModel = parmList.Where(d => d.id_shop == id_shop && d.parmcode == "hy_czje_max_onec").OrderByDescending(d => d.id_masteruser).FirstOrDefault();
                    if (hy_czje_max_onecModel != null && !string.IsNullOrEmpty(hy_czje_max_onecModel.id))
                    {
                        result["hy_czje_max_onec"] = hy_czje_max_onecModel.parmvalue;
                    }

                    var hy_czje_max_monthModel = parmList.Where(d => d.id_shop == id_shop && d.parmcode == "hy_czje_max_month").OrderByDescending(d => d.id_masteruser).FirstOrDefault();
                    if (hy_czje_max_monthModel != null && !string.IsNullOrEmpty(hy_czje_max_monthModel.id))
                    {
                        result["hy_czje_max_month"] = hy_czje_max_monthModel.parmvalue;
                    }

                    result["success"] = 1;

                    DataCache.Add(id_user_master + "_" + id_shop + "_GetShopParm", result, DateTime.Now.AddDays(1));
                }
                return result;
            }
        }
        #endregion

        #region GetHy_ShopShare-判断是否共享并取出门店关系表对应的id_shop
        /// <summary>
        /// 判断是否共享 并取出门店关系表对应的id_shop
        /// lz
        /// 2016-09-19
        /// </summary>
        public BaseResult GetHy_ShopShare(string id_shop_select, string id_user_master)
        {
            #region 定义参数
            BaseResult br = new BaseResult() { Success = true };
            Hashtable param = new Hashtable();
            #endregion
            #region 判断是否共享的处理
            Hashtable ht = new Hashtable();
            var user_parm = GetParm(id_user_master);
            param.Add("hy_shopshare", user_parm["hy_shopshare"].ToString());
            if (user_parm["hy_shopshare"].ToString() == ((int)Enums.FlagShopShare.Shared).ToString())
            {
                //共用会员 
                #region 查询主门店id
                ht.Clear();
                ht.Add("id_masteruser", id_user_master);
                ht.Add("flag_master", (int)Enums.TbUserFlagMaster.Master);
                var accountList = DAL.QueryList<Tb_User>(typeof(Tb_User), ht);  //BusinessFactory.Account.GetAllUser(ht);
                #endregion
                #region 如果查询失败则返回
                if (accountList != null && accountList.Count() > 0 && !string.IsNullOrEmpty(accountList.FirstOrDefault().id_shop))
                    param.Add("id_shop", accountList.FirstOrDefault().id_shop);
                else
                {
                    #region 返回空数据
                    br.Success = false;
                    br.Message.Add(string.Format("查询是否共用会员数据失败,请重试!"));
                    return br;
                    #endregion
                }
                #endregion
            }
            else
            {
                param.Add("id_shop", id_shop_select);
            }
            #endregion
            #region 返回数据
            br.Data = param;
            return br;
            #endregion
        }
        #endregion

        #region GetAutoAudit
        public Hashtable GetAutoAudit(string id_user_master, string id_shop, string id_shop_master)
        {
            Hashtable result = new Hashtable();
            var shopParm = GetShopParm(id_user_master, id_shop);
            if (shopParm != null && shopParm.ContainsKey("success") && shopParm["success"].ToString() == "1" && shopParm.ContainsKey("bill_auto_audit_need_query") && shopParm["bill_auto_audit_need_query"].ToString() == "0")
                result.Add("bill_auto_audit", shopParm["bill_auto_audit"].ToString());
            else
            {
                shopParm = GetShopParm(id_user_master, "0");
                if (shopParm != null && shopParm.ContainsKey("success") && shopParm["success"].ToString() == "1" && shopParm.ContainsKey("bill_auto_audit_need_query") && shopParm["bill_auto_audit_need_query"].ToString() == "0")
                {
                    result.Add("bill_auto_audit", shopParm["bill_auto_audit"].ToString());
                    //if (DataCache.Get(id_user_master + "_" + id_shop + "_GetShopParm") == null || ((Hashtable)DataCache.Get(id_user_master + "_" + id_shop + "_GetShopParm")).Keys.Count <= 0)
                    //{
                    //    DataCache.Add(id_user_master + "_" + id_shop + "_GetShopParm", result, DateTime.Now.AddDays(1));
                    //}
                }
                else
                {
                    result.Add("bill_auto_audit", 0);

                }
            }
            return result;
        }


        #endregion

        #region ClearShopParm
        public void ClearShopParm(string id_user_master, string id_shop)
        {
            try
            {
                if (DataCache.Get(id_user_master + "_" + id_shop + "_GetShopParm") != null && ((Hashtable)DataCache.Get(id_user_master + "_" + id_shop + "_GetShopParm")).Keys.Count > 0)
                    DataCache.Remove(id_user_master + "_" + id_shop + "_GetShopParm");

                if (DataCache.Get(id_user_master + "_GetParm") != null && ((Hashtable)DataCache.Get(id_user_master + "_GetParm")).Keys.Count > 0)
                    DataCache.Remove(id_user_master + "_GetParm");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region GetCYService
        /// <summary>
        /// 获取业务系统服务
        /// </summary>
        /// <param name="param"> 需要传入id_masteruser id_cyuser  bm  service rq_create_master_shop </param>
        /// 返回 cyServiceList：List<Schedule_UserService> 服务列表
        ///      isExpire：  是否到期 1为到期 0为未到期
        ///      startTime:  到期开始时间 
        ///      endTime:    到期结束时间
        ///      isOutLimit: 是否超过站点 1为超过 0为未超过
        ///      newVersion: 调用服务接口获取到的目前最新的版本
        /// <returns></returns>
        public Hashtable GetCYService(Hashtable param)
        {
            Hashtable result = new Hashtable();

            if (!param.ContainsKey("id_masteruser"))
                return null;

            if (DataCache.Get(param["id_masteruser"].ToString() + "_GetCYService") != null && ((Hashtable)DataCache.Get(param["id_masteruser"].ToString() + "_GetCYService")).Keys.Count > 0 && (!param.ContainsKey("do_post") || param["do_post"].ToString() != "1"))
                return (Hashtable)DataCache.Get(param["id_masteruser"].ToString() + "_GetCYService");
            else
            {

                #region lz 2017-03-07 edit
                if (!param.ContainsKey("bm"))
                    return null;

                string oldBm = param["bm"].ToString();
                param.Remove("bm");
                param.Add("bm", "yunpos");
                #endregion


                var cyServiceList = this.GetCYServiceDoPost(param);
                if (cyServiceList == null)
                    return null;

                result.Add("cyServiceList", cyServiceList);
                result.Add("isStop", 0);//是否停用
                result.Add("isExpire", 0);//是否到期
                result.Add("startTime", DateTime.Parse("1900-1-1 0:00:00"));//到期时间
                result.Add("endTime", DateTime.Parse("1900-1-1 0:00:00"));//到期时间
                result.Add("isOutLimit", 0);//是否超过站点
                result.Add("newVersion", oldBm);

                #region lz 2017-03-07 edit
                ////大于1条 异常 最多为1条
                //if (cyServiceList.Count() > 1)
                //    return null;
                #endregion

                //验证是否过期 是否超过数量
                if (cyServiceList.Count() <= 0)
                {
                    //没购买过服务 看是否超过试用期
                    var expireTime = DateTime.Parse(DateTime.Parse(param["rq_create_master_shop"].ToString()).AddDays(PublicSign.tryDays).ToString("yyyy-MM-dd"));
                    result["endTime"] = expireTime;
                    if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) >= expireTime)
                    {
                        result["isExpire"] = 1;
                    }
                }
                else
                {
                    #region lz 2017-03-07 edit
                    //lz 2017-03-07 edit
                    if (cyServiceList.Where(d => d.bm != oldBm).Count() > 0)
                    {
                        if (cyServiceList.Where(d => d.bm.Trim() == PublicSign.bm_yunpos_jt.Trim()).Count() > 0 )
                        {
                            //集团版
                            result["newVersion"] = PublicSign.bm_yunpos_jt.Trim();
                        }
                        else if (cyServiceList.Where(d => d.bm.Trim() == PublicSign.bm_yunpos_ls.Trim()).Count() > 0 )
                        {
                            //连锁版
                            result["newVersion"] = PublicSign.bm_yunpos_ls.Trim();
                        }
                        else if (cyServiceList.Where(d => d.bm.Trim() == PublicSign.bm_yunpos_dd.Trim()).Count() > 0 )
                        {
                            //单店版
                            result["newVersion"] = PublicSign.bm_yunpos_dd.Trim();
                        }
                    }
                    param["bm"] = result["newVersion"].ToString();

                    if (result["newVersion"].ToString().Trim() != oldBm)
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("id_masteruser", param["id_masteruser"].ToString());
                        if (result["newVersion"].ToString().Trim() == PublicSign.bm_yunpos_dd.ToString().Trim())
                            ht.Add("new_version", 10);
                        else if (result["newVersion"].ToString().Trim() == PublicSign.bm_yunpos_ls.ToString().Trim())
                            ht.Add("new_version", 20);
                        else if (result["newVersion"].ToString().Trim() == PublicSign.bm_yunpos_jt.ToString().Trim())
                            ht.Add("new_version", 30);
                        DAL.UpdatePart(typeof(Tb_User), ht);
                    }

                    cyServiceList = cyServiceList.Where(d => d.bm == param["bm"].ToString()).ToList();
                    result["cyServiceList"] = cyServiceList;

                    #endregion


                    #region 购买过服务 是否停用
                    if (cyServiceList.Where(d => d.flag_stop == (int)Enums.CYServiceStopFlag.Used && d.bm == param["bm"].ToString()).Count() <= 0)
                    {
                        result["isStop"] = 1;
                    }
                    #endregion

                    #region 购买过服务 还未停用
                    else
                    {
                        #region 购买过数据 查看是否过期
                        //购买过数据 查看是否过期
                        var cyServiceModel = cyServiceList.FirstOrDefault();
                        if (cyServiceModel.rq_begin != null && cyServiceModel.rq_begin > DateTime.Parse("1900-1-1 0:00:00") && cyServiceModel.rq_begin > DateTime.Now)
                        {
                            result["isExpire"] = 1;
                        }
                        result["startTime"] = cyServiceModel.rq_begin;

                        if (cyServiceModel.rq_end != null && cyServiceModel.rq_end > DateTime.Parse("1900-1-1 0:00:00") && cyServiceModel.rq_end < DateTime.Now)
                        {
                            result["isExpire"] = 1;
                        }
                        result["endTime"] = cyServiceModel.rq_end;
                        #endregion

                        #region 是否需要检查库存数量
                        if (param.ContainsKey("check_db_count") && param["check_db_count"].ToString() == "1" && cyServiceModel.sl > 0)
                        {
                            Hashtable ht = new Hashtable();
                            ht.Clear();
                            ht.Add("id_masteruser", param["id_masteruser"].ToString());
                            ht.Add("flag_state", (byte)Enums.TbShopFlagState.Opened);
                            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                            var dbCount = DAL.GetCount(typeof(Tb_Shop), ht);
                            if (dbCount > cyServiceModel.sl)
                            {
                                result["isOutLimit"] = 1;
                            }
                        }
                        #endregion

                    }
                    #endregion
                }

                if (DataCache.Get(param["id_masteruser"].ToString() + "_GetCYService") != null && ((Hashtable)DataCache.Get(param["id_masteruser"].ToString() + "_GetCYService")).Keys.Count > 0)
                    DataCache.Set(param["id_masteruser"].ToString() + "_GetCYService", result);
                else
                    DataCache.Add(param["id_masteruser"].ToString() + "_GetCYService", result, DateTime.Now.AddDays(1));
                return result;
            }
        }

        #endregion

        #region GetCYServiceDoPost
        /// <summary>
        /// 调用业务系统接口 获取服务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Schedule_UserService> GetCYServiceDoPost(Hashtable param)
        {
            try
            {
                List<Schedule_UserService> list = new List<Schedule_UserService>();
                BaseResult br = new BaseResult() { Success = false };
                var paramters = new Dictionary<string, string>();

                paramters.Add("uid", param["id_cyuser"].ToString());
                paramters.Add("bm", param["bm"].ToString());//bm 
                paramters.Add("flag_from", "yunpos");
                paramters.Add("service", param["service"].ToString());//service GetService
                paramters.Add("sign", SignUtils.SignRequestForCyUserSys(paramters, PublicSign.md5KeyBusiness));
                var webutils = new CySoft.Utility.WebUtils();
                var respStr = webutils.DoPost(PublicSign.cyGetServiceUrl, paramters, 30000);
                var respModel = JSON.Deserialize<ServiceResult>(respStr);

                if (respModel != null)
                {
                    if (respModel.State != ServiceState.Done)
                    {
                        var savePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Log\\" + "ServiceLog\\ServiceLog.txt";
                        new CySoft.Frame.Common.LogText().Log2File(savePath, "调取服务接口 id_cyuser: " + param["id_cyuser"].ToString() + " RequestStr:     " + respStr);
                        return null;
                    }
                    else
                    {
                        if (respModel.Data != null)
                        {
                            var cyList = JSON.Deserialize<List<Schedule_UserService>>(respModel.Data.ToString());
                            if (cyList == null)
                            {
                                cyList = new List<Schedule_UserService>();
                            }
                            list = cyList;

                            var savePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Log\\" + "ServiceLog\\ServiceLog.txt";
                            new CySoft.Frame.Common.LogText().Log2File(savePath, "调取服务接口 id_cyuser: " + param["id_cyuser"].ToString() + " RequestStr:     " + respStr);

                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                var savePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Log\\" + "ServiceLog\\ServiceLog.txt";
                new CySoft.Frame.Common.LogText().Log2File(savePath, "调取服务接口 Error:    " + JSON.Serialize(ex.Message));
                return null;
            }
        }
        #endregion

        #region ClearCYService
        public void ClearCYService(string id_masteruser)
        {
            try
            {
                if (DataCache.Get(id_masteruser + "_GetCYService") != null && ((Hashtable)DataCache.Get(id_masteruser + "_GetCYService")).Keys.Count > 0)
                    DataCache.Remove(id_masteruser + "_GetCYService");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region 获取业务购买服务地址
        /// <summary>
        /// 获取业务购买服务地址
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetBuyServiceUrl(Hashtable param)
        {
            try
            {
                var paramters = new Dictionary<string, string>();
                paramters.Add("id", param["id"].ToString());
                paramters.Add("uid", param["id_cyuser"].ToString());
                string ps = MD5Encrypt.Encode(Encoding.UTF8, "cy.$" + param["id_cyuser"].ToString() + "+#" + param["phone"].ToString() + "*" + DateTime.Now.ToString("yyyyMMddHH"));
                paramters.Add("ps", ps);
                string mySign = SignUtils.SignRequestForCyUserSys(paramters, PublicSign.md5KeyBusiness);
                paramters.Add("sign", mySign);
                string url_parm = WebUtils.BuildQuery2(paramters);
                string url = PublicSign.cyBuyServiceUrl + "?" + url_parm;

                #region 如果没购买过服务则显示选择购买的页面
                if (param.ContainsKey("id_masteruser") && DataCache.Get(param["id_masteruser"].ToString() + "_GetCYService") != null && ((Hashtable)DataCache.Get(param["id_masteruser"].ToString() + "_GetCYService")).Keys.Count > 0)
                {
                    var serviceHas = (Hashtable)DataCache.Get(param["id_masteruser"].ToString() + "_GetCYService");
                    if (serviceHas.ContainsKey("cyServiceList"))
                    {
                        List<Schedule_UserService> buyList = (List<Schedule_UserService>)serviceHas["cyServiceList"];
                        if (buyList == null || buyList.Count() <= 0)
                        {
                            url = PublicSign.CyBuyServiceUrlAll + "?" + url_parm;
                        }
                    }
                }
                #endregion

                return url;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region CheckServiceForLogin
        public BaseResult CheckServiceForLogin(Hashtable loginInfo)
        {
            BaseResult br = new BaseResult() { Success = true };
            string bm = GetServiceBM(loginInfo["version"].ToString());
            if (string.IsNullOrEmpty(bm))
            {
                br.Message.Clear();
                br.Message.Add("登录失败！获取购买服务编码版本异常,,请重试！");
                br.Level = ErrorLevel.Error;
                br.Success = false;
                return br;
            }

            #region 获取服务信息
            Hashtable ht = new Hashtable();
            ht.Add("id_cyuser", loginInfo["id_cyuser"].ToString());
            ht.Add("bm", bm);
            ht.Add("service", "GetService");
            ht.Add("id_masteruser", loginInfo["id_user_master"].ToString());
            if (loginInfo.ContainsKey("is_sysmanager") && loginInfo["is_sysmanager"].ToString() == "1")
            {
                //如果是管理员登录 重新调接口查询
                ht.Add("do_post", "1");
            }
            ht.Add("rq_create_master_shop", loginInfo["rq_create_master_shop"].ToString());

            var cyServiceHas = GetCYService(ht);
            if (cyServiceHas == null || !cyServiceHas.ContainsKey("cyServiceList") || !cyServiceHas.ContainsKey("isExpire") || !cyServiceHas.ContainsKey("isStop"))
            {
                br.Message.Clear();
                br.Message.Add("登录失败！获取购买服务异常,请重试！");
                br.Level = ErrorLevel.Error;
                br.Success = false;
                return br;
            }

            if (cyServiceHas != null && cyServiceHas.ContainsKey("newVersion"))
            {
                bm = cyServiceHas["newVersion"].ToString().Trim();
            }

            #endregion
            #region 获取购买服务跳转地址
            ht.Clear();
            ht.Clear();
            ht.Add("id_cyuser", loginInfo["id_cyuser"].ToString());
            ht.Add("id", bm);
            ht.Add("phone", loginInfo["phone_master"].ToString());
            ht.Add("service", "Detail");
            ht.Add("id_masteruser", loginInfo["id_user_master"].ToString());
            string buyUrl = GetBuyServiceUrl(ht);
            if (string.IsNullOrEmpty(buyUrl))
                buyUrl = PublicSign.cyBuyServiceUrl;

            var jumpUrl = buyUrl = HttpUtility.UrlEncode(buyUrl, Encoding.UTF8);
            #endregion
            #region 是否停用
            if (cyServiceHas["isStop"].ToString() == "1")
            {
                br.Message.Clear();
                br.Success = false;
                br.Message.Add("登录失败！您的服务信息已停用！");
                br.Level = ErrorLevel.Drump;
                br.Data = jumpUrl;
                return br;
            }
            #endregion
            #region 是否过期
            if (cyServiceHas["isExpire"].ToString() == "1")
            {
                br.Message.Clear();
                br.Success = false;
                br.Message.Add("登录失败！您的服务信息已过期！");
                br.Level = ErrorLevel.Drump;
                br.Data = jumpUrl;
                return br;
            }
            #endregion
            #region 如果购买过服务 验证服务数量是否超过
            var cyServiceList = (List<Schedule_UserService>)cyServiceHas["cyServiceList"];
            if (cyServiceList != null && cyServiceList.Count() > 0)
            {
                var cyServiceModel = cyServiceList.FirstOrDefault();
                //获取现在门店数量
                ht.Clear();
                ht.Add("id_masteruser", loginInfo["id_user_master"].ToString());
                ht.Add("flag_state", (byte)Enums.TbShopFlagState.Opened);
                ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                var dbCount = DAL.GetCount(typeof(Tb_Shop), ht);
                if (dbCount > cyServiceModel.sl && cyServiceModel.sl > 0)
                {
                    #region 设置结果为已超过数量限制
                    if (cyServiceHas.ContainsKey("isOutLimit"))
                        cyServiceHas["isOutLimit"] = 1;
                    else
                        cyServiceHas.Add("isOutLimit", 1);

                    if (DataCache.Get(loginInfo["id_user_master"].ToString() + "_GetCYService") != null && ((Hashtable)DataCache.Get(loginInfo["id_user_master"].ToString() + "_GetCYService")).Keys.Count > 0)
                        DataCache.Set(loginInfo["id_user_master"].ToString() + "_GetCYService", cyServiceHas);
                    else
                        DataCache.Add(loginInfo["id_user_master"].ToString() + "_GetCYService", cyServiceHas, DateTime.Now.AddDays(1));

                    //如果是系统管理员角色 跳转选择开启门店的窗口
                    br.Message.Clear();
                    br.Success = false;
                    //br.Message.Add("登录失败！您的服务信息已超出购买数量！请先处理要启用的门店！");
                    br.Message.Add("登录失败！您的服务信息已超出购买数量！请先登录管理员角色处理要启用的门店！");

                    br.Level = ErrorLevel.Question;
                    br.Data = jumpUrl;
                    return br;

                    #endregion
                }
                else
                {
                    #region 设置结果为未超过数量限制
                    if (cyServiceHas.ContainsKey("isOutLimit"))
                        cyServiceHas["isOutLimit"] = 0;
                    else
                        cyServiceHas.Add("isOutLimit", 0);

                    if (DataCache.Get(loginInfo["id_user_master"].ToString() + "_GetCYService") != null && ((Hashtable)DataCache.Get(loginInfo["id_user_master"].ToString() + "_GetCYService")).Keys.Count > 0)
                        DataCache.Set(loginInfo["id_user_master"].ToString() + "_GetCYService", cyServiceHas);
                    else
                        DataCache.Add(loginInfo["id_user_master"].ToString() + "_GetCYService", cyServiceHas, DateTime.Now.AddDays(1));
                    #endregion
                }
            }
            #endregion

            if (br.Success)
            {
                if (!cyServiceHas.ContainsKey("buyUrl"))
                    cyServiceHas.Add("buyUrl", jumpUrl);
                br.Data = cyServiceHas;
            }

            return br;
        }
        #endregion

        #region GetServiceBM
        public string GetServiceBM(string version)
        {
            string bm = "";

            if (version == "10")
                bm = PublicSign.bm_yunpos_dd;

            if (version == "20")
                bm = PublicSign.bm_yunpos_ls;

            if (version == "30")
                bm = PublicSign.bm_yunpos_jt;

            return bm;
        }
        #endregion

        #region 获取网络图片保存在本地
        /// <summary>
        /// 获取网络图片保存在本地
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetBarcodePic(Tb_Shopsp_Service model)
        {
            if (model == null || string.IsNullOrEmpty(model.BarCode) || string.IsNullOrEmpty(model.Picture))
                return "/static/images/default.jpg";

            var pic = model.Picture.Split(';')[0].ToString();
            if (!pic.Contains(".")) { return "/static/images/default.jpg"; }

            try
            {
                if (!Directory.Exists(ApplicationInfo.ShopSpPic))
                {
                    Directory.CreateDirectory(ApplicationInfo.ShopSpPic);
                }

                String fileExt = Path.GetExtension(pic).ToLower();
                string fileName = model.BarCode + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExt;

                //保存图片 并返回数据
                string saveUrl = ApplicationInfo.ShopSpPic + "\\" + fileName;

                if (KeleyiImgDownloader.DownloadPicture(pic, saveUrl, 10000))
                {
                    return "/UpLoad/ShopSpPic/" + fileName;
                }
                else
                {
                    return "/static/images/default.jpg";
                }
            }
            catch (Exception ex)
            {
                return pic;
            }
        }
        #endregion

        /// <summary>
        /// 获取配送收款类型0：不做结算，1：先款后货，2先货后款
        /// </summary>
        /// <param name="id_masteruser"></param>
        /// <returns></returns>
        protected string GetPsSktype(string id_masteruser)
        {
            Hashtable param = new Hashtable();
            param.Add("parmcode", "ps_sktype");
            param.Add("id_masteruser", id_masteruser);
            var paramModel = DAL.GetItem<Ts_Parm>(typeof(Ts_Parm), param);
            if (paramModel!=null)
            {
                return paramModel.parmvalue;
            }
            return "";
        }

        /// <summary>
        /// 查询可用额度
        /// </summary>
        /// <returns></returns>
        protected decimal GetCanUseMoney(string id_masteruser, string id_shop, string notClacId = "", string bm_djlx = "", string id_bill_origin = "", string bm_djlx_origin = "")
        {

            Hashtable param=new Hashtable(); 
            param.Add("id_masteruser", id_masteruser);
            param.Add("id_shop", id_shop); 
            var shop = DAL.GetItem<Tb_Shop>(typeof(Tb_Shop), param);
            if (shop == null || shop.id_kh.IsEmpty())
            {
                return 999999999999;
            }
            param.Clear();
            param.Add("id_masteruser", id_masteruser);
            param.Add("id_shop_child", id_shop); 
            var shopshop = DAL.GetItem<Tb_Shop_Shop>(typeof(Tb_Shop_Shop), param);
            if (shopshop != null)
            {
                param.Clear();
                param.Add("id_masteruser", id_masteruser);
                param.Add("id_shop", id_shop);
                param.Add("notClacId", notClacId);
                param.Add("bm_djlx", bm_djlx);
                param.Add("id_bill_origin", id_bill_origin);
                param.Add("bm_djlx_origin", bm_djlx_origin);
                param.Add("id_shop_sk", shopshop.id_shop_father);
                var obj = string.Format("{0}", DAL.ExecuteSqlWithBack(param)); 
                decimal money = 0;
                decimal.TryParse(obj, out money);
                //money = decimal.Round(money, 1, MidpointRounding.AwayFromZero);
                return money;
            }
            return -999999999;
        }

        protected decimal GetCanUseMoneyForKH(string id_masteruser, string id_shop, string id_kh, string notClacId = "", string bm_djlx = "", string id_bill_origin = "", string bm_djlx_origin = "")
        {
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("function_name", "dbo.f_get_shop_limit_for_kh");
            param.Add("id_masteruser", id_masteruser);
            param.Add("id_shop", id_shop);
            param.Add("notClacId", notClacId);
            param.Add("bm_djlx", bm_djlx);
            param.Add("id_bill_origin", id_bill_origin);
            param.Add("bm_djlx_origin", bm_djlx_origin);
            param.Add("id_kh", id_kh);
            var obj = string.Format("{0}", DAL.ExecuteFunctionWithName(param));
            decimal money = 0;
            decimal.TryParse(obj, out money);
            //money = decimal.Round(money, 1, MidpointRounding.AwayFromZero);
            return money;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_shop"></param>
        /// <returns></returns>
        protected Tb_Shop QueryShopById(string id_shop)
        {
            Hashtable param = new Hashtable();
            param.Add("id", id_shop);
            param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            param.Add("flag_state", (int)Enums.FlagShopspStop.NoStop);
            return DAL.GetItem<Tb_Shop>(typeof(Tb_Shop), param);
        }

        protected bool NeedCheckXY_XS(string id_masteruser)
        {
            try
            {
                bool result = true;
                Hashtable ht = new Hashtable();
                ht.Add("parmcode", "xs_sktype");
                ht.Add("id_masteruser", id_masteruser);
                var parmList = DAL.QueryList<Ts_Parm>(typeof(Ts_Parm), ht).ToList();

                if (parmList != null && parmList.Count() > 0&& parmList.Where(d=>d.parmvalue=="1"|| d.parmvalue == "2").Count()>0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
