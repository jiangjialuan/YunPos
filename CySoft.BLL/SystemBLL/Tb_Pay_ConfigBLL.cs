using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Frame.Utility;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Model.Ts;

namespace CySoft.BLL.SystemBLL
{
    public class Tb_Pay_ConfigBLL : BaseBLL
    {

        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult res=new BaseResult(){Success = true};
            res.Data = DAL.QueryList<Tb_Pay_Config>(typeof(Tb_Pay_Config), param).ToList();
            return res;
        }

        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Tb_Pay_ConfigWithMc), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_Pay_ConfigWithMc>(typeof(Tb_Pay_Config), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }

        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            res.Data = DAL.GetItem<Tb_Pay_Config>(typeof(Tb_Pay_Config), param);
            return res;
        }
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult res = new BaseResult() { Success = true };
            Tb_Pay_Config model = entity as Tb_Pay_Config;
            if (model != null)
            {
                return HandleTbPayConfig(res, model);
            }
            SetPayConfigModel config = entity as SetPayConfigModel;
            if (config != null)
            {
                return HandleTbPayConfig(res, config);
            }
            OpenPayConfigModel openPayConfigModel = entity as OpenPayConfigModel;
            if (openPayConfigModel != null)
            {
                return HandleOpenPayConfig(res, openPayConfigModel);
            }
            List<Tb_Pay_Config> list = entity as List<Tb_Pay_Config>;
            if (list != null)
            {
                return HandleListTbPayConfig(res, list);
            }
            res.Success = false;
            res.Message.Add("参数有误!");
            return res;
        }

        private BaseResult HandleTbPayConfig(BaseResult res, SetPayConfigModel model)
        {
            if (model == null
                || (model.flag_type != 4 && model.flag_type != 5)
                || string.IsNullOrEmpty(model.id_masteruser)
                || string.IsNullOrEmpty(model.id_shop))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", model.id_masteruser);
            param.Add("id_shop", model.id_shop);
            param.Add("flag_type", model.flag_type);
            var list = DAL.QueryList<Tb_Pay_Config>(typeof(Tb_Pay_Config), param).ToList();
            DAL.Delete(typeof(Tb_Pay_Config), param);
            List<Tb_Pay_Config> newList = new List<Tb_Pay_Config>();
            if (model.flag_type == 4)
            {
                if (string.IsNullOrWhiteSpace(model.pay_wx_appid)
                    || string.IsNullOrWhiteSpace(model.pay_wx_appid)
                    || string.IsNullOrWhiteSpace(model.pay_wx_key)
                    || string.IsNullOrWhiteSpace(model.pay_wx_mch_id)
                    || string.IsNullOrWhiteSpace(model.pay_wx_mch_id_child))
                {
                    res.Success = false;
                    res.Message.Add("参数不能为空!");
                    return res;
                }
                #region
                newList.Add(new Tb_Pay_Config()
                            {
                                id_masteruser = model.id_masteruser,
                                flag_type = model.flag_type,
                                id = Guid.NewGuid().ToString(),
                                id_shop = model.id_shop,
                                parmcode = "pay_wx_appid",
                                parmname = "AppID",
                                parmvalue = model.pay_wx_appid
                            });
                newList.Add(new Tb_Pay_Config()
                {
                    id_masteruser = model.id_masteruser,
                    flag_type = model.flag_type,
                    id = Guid.NewGuid().ToString(),
                    id_shop = model.id_shop,
                    parmcode = "pay_wx_key",
                    parmname = "key钥",
                    parmvalue = model.pay_wx_key
                });
                newList.Add(new Tb_Pay_Config()
                {
                    id_masteruser = model.id_masteruser,
                    flag_type = model.flag_type,
                    id = Guid.NewGuid().ToString(),
                    id_shop = model.id_shop,
                    parmcode = "pay_wx_mch_id",
                    parmname = "商户号钥",
                    parmvalue = model.pay_wx_mch_id
                });
                newList.Add(new Tb_Pay_Config()
                {
                    id_masteruser = model.id_masteruser,
                    flag_type = model.flag_type,
                    id = Guid.NewGuid().ToString(),
                    id_shop = model.id_shop,
                    parmcode = "pay_wx_is_use",
                    parmname = "是否启用微信支付",
                    parmvalue = model.pay_wx_is_use
                });
                newList.Add(new Tb_Pay_Config()
                {
                    id_masteruser = model.id_masteruser,
                    flag_type = model.flag_type,
                    id = Guid.NewGuid().ToString(),
                    id_shop = model.id_shop,
                    parmcode = "pay_wx_mch_id_child",
                    parmname = "子商户号",
                    parmvalue = model.pay_wx_mch_id_child
                });
                #endregion
            }
            if (model.flag_type == 5)
            {
                if (string.IsNullOrWhiteSpace(model.pay_alipay_partner)
                    || string.IsNullOrWhiteSpace(model.pay_alipay_rsakey1)
                    || string.IsNullOrWhiteSpace(model.pay_alipay_is_use)
                    || string.IsNullOrWhiteSpace(model.pay_alipay_product_code))
                {
                    res.Success = false;
                    res.Message.Add("参数不能为空!");
                    return res;
                }
                #region
                newList.Add(new Tb_Pay_Config()
                            {
                                id_masteruser = model.id_masteruser,
                                flag_type = model.flag_type,
                                id = Guid.NewGuid().ToString(),
                                id_shop = model.id_shop,
                                parmcode = "pay_alipay_partner",
                                parmname = "商户身份id",
                                parmvalue = model.pay_alipay_partner
                            });
                newList.Add(new Tb_Pay_Config()
                {
                    id_masteruser = model.id_masteruser,
                    flag_type = model.flag_type,
                    id = Guid.NewGuid().ToString(),
                    id_shop = model.id_shop,
                    parmcode = "pay_alipay_rsafile",
                    parmname = "商户私钥路径",
                    parmvalue = model.pay_alipay_rsafile
                });
                var length = model.pay_alipay_rsakey1.Length;
                var length1 = length / 4;
                var rsakey1 = model.pay_alipay_rsakey1.Substring(0, length1);
                var rsakey2 = model.pay_alipay_rsakey1.Substring(length1, length1);
                var rsakey3 = model.pay_alipay_rsakey1.Substring(2 * length1, length1);
                var rsakey4 = model.pay_alipay_rsakey1.Substring(3 * length1, length - 3 * length1);
                newList.Add(new Tb_Pay_Config()
                {
                    id_masteruser = model.id_masteruser,
                    flag_type = model.flag_type,
                    id = Guid.NewGuid().ToString(),
                    id_shop = model.id_shop,
                    parmcode = "pay_alipay_rsakey1",
                    parmname = "商户私钥文本段1",
                    parmvalue = rsakey1
                });
                newList.Add(new Tb_Pay_Config()
                {
                    id_masteruser = model.id_masteruser,
                    flag_type = model.flag_type,
                    id = Guid.NewGuid().ToString(),
                    id_shop = model.id_shop,
                    parmcode = "pay_alipay_rsakey2",
                    parmname = "商户私钥文本段2",
                    parmvalue = rsakey2
                });
                newList.Add(new Tb_Pay_Config()
                {
                    id_masteruser = model.id_masteruser,
                    flag_type = model.flag_type,
                    id = Guid.NewGuid().ToString(),
                    id_shop = model.id_shop,
                    parmcode = "pay_alipay_rsakey3",
                    parmname = "商户私钥文本段3",
                    parmvalue = rsakey3
                });
                newList.Add(new Tb_Pay_Config()
                {
                    id_masteruser = model.id_masteruser,
                    flag_type = model.flag_type,
                    id = Guid.NewGuid().ToString(),
                    id_shop = model.id_shop,
                    parmcode = "pay_alipay_rsakey4",
                    parmname = "商户私钥文本段4",
                    parmvalue = rsakey4
                });
                newList.Add(new Tb_Pay_Config()
                {
                    id_masteruser = model.id_masteruser,
                    flag_type = model.flag_type,
                    id = Guid.NewGuid().ToString(),
                    id_shop = model.id_shop,
                    parmcode = "pay_alipay_is_use",
                    parmname = "是否启用支付宝支付",
                    parmvalue = model.pay_alipay_is_use
                });
                newList.Add(new Tb_Pay_Config()
                {
                    id_masteruser = model.id_masteruser,
                    flag_type = model.flag_type,
                    id = Guid.NewGuid().ToString(),
                    id_shop = model.id_shop,
                    parmcode = "pay_alipay_product_code",
                    parmname = "支付类型",
                    parmvalue = model.pay_alipay_product_code
                });
                #endregion

            }
            if (newList.Any())
            {
                DAL.AddRange(newList);
            }
            return res;
        }

        private BaseResult HandleTbPayConfig(BaseResult res, Tb_Pay_Config model)
        {
            if (model == null || string.IsNullOrEmpty(model.id))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (string.IsNullOrEmpty(model.parmvalue))
            {
                res.Success = false;
                res.Message.Add("支付参数不能为空!");
                return res;
            }
            Hashtable param = new Hashtable();
            param.Add("id", model.id);
            param.Add("new_parmvalue", model.parmvalue.Trim());
            if (DAL.UpdatePart(typeof(Tb_Pay_Config), param) <= 0)
            {
                res.Success = false;
                res.Message.Add("操作失败!");
            }
            return res;
        }
        [Transaction]
        private BaseResult HandleOpenPayConfig(BaseResult res, OpenPayConfigModel model)
        {
            if (model==null
                ||string.IsNullOrEmpty(model.id)
                ||string.IsNullOrEmpty(model.id_is_use)
                ||string.IsNullOrEmpty(model.id_masteruser)
                ||string.IsNullOrEmpty(model.parmvalue_is_use))
            {
                res.Success = false;
                res.Message.Add("参数有误");
                return res;
            }
            if (string.IsNullOrEmpty(model.parmvalue))
            {
                res.Success = false;
                res.Message.Add("子商户号不能为空");
                return res;
            }
            Hashtable param = new Hashtable();
            param.Add("id", model.id);
            param.Add("new_parmvalue", model.parmvalue.Trim());
            DAL.UpdatePart(typeof(Tb_Pay_Config), param);
            if (model.ope_type=="open")
            {
                param.Clear();
                param.Add("id", model.id_is_use);
                param.Add("new_parmvalue", model.parmvalue_is_use.Trim());
                DAL.UpdatePart(typeof(Tb_Pay_Config), param);
            }
            return res;
        }
        [Transaction]
        private BaseResult HandleListTbPayConfig(BaseResult res, List<Tb_Pay_Config> list)
        {
            if (list==null||!list.Any())
            {
                res.Success = false;
                res.Message.Add("参数有误");
                return res;
            }
            Hashtable param = new Hashtable();
            list.ForEach(l =>
            {
                param.Clear();
                param.Add("id", l.id);
                param.Add("new_parmvalue", l.parmvalue.Trim());
                DAL.UpdatePart(typeof(Tb_Pay_Config), param);
            });
            return res;
        }
    }

}
