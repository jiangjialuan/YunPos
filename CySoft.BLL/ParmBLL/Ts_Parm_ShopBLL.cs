using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Model.Ts;
using CySoft.Utility;

namespace CySoft.BLL.ParmBLL
{
    public class Ts_Parm_ShopBLL : BaseBLL
    {
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult res = new BaseResult() { Success = true };
            if (param == null || param.Count <= 0 || (!param.ContainsKey("id_masteruser") && !param.ContainsKey("get_self_defaul")))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            res.Data= DAL.QueryList<Ts_Parm_Shop>(typeof(Ts_Parm_Shop), param);
            return res;
        }
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Ts_Parm_Shop), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Ts_Parm_ShopWithShopMc>(typeof(Ts_Parm_Shop), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }
        public override BaseResult Add(dynamic entity)
        {
            BaseResult res = new BaseResult() { Success = true };
            Ts_Parm_Shop model = entity as Ts_Parm_Shop;
            if (model == null)
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (string.IsNullOrEmpty(model.parmname))
            {
                res.Success = false;
                res.Message.Add("参数名称不能为空!");
                return res;
            }
            if (string.IsNullOrEmpty(model.parmcode))
            {
                res.Success = false;
                res.Message.Add("参数编码不能为空!");
                return res;
            }
            if (string.IsNullOrEmpty(model.id_shop))
            {
                res.Success = false;
                res.Message.Add("所属门店不能为空!");
                return res;
            }
            int newvalue = -1;
            int.TryParse(model.parmvalue, out newvalue);
            if (newvalue < 0 || newvalue > 7)
            {
                res.Success = false;
                res.Message.Add("设定值在0-7之间!");
                return res;
            }
            Hashtable param = new Hashtable();
            param.Add("parmcode", model.parmcode);
            param.Add("id_masteruser", model.id_masteruser);
            if (DAL.GetCount(typeof(Ts_Parm_Shop), param) > 0)
            {
                res.Success = false;
                res.Message.Add("参数编码已存在!");
                return res;
            }
            model.id = Guid.NewGuid().ToString();
            DAL.Add(model);
            return res;
        }
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult();
            res.Data = DAL.GetItem<Ts_Parm_Shop>(typeof(Ts_Parm_Shop), param);
            return res;
        }
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult res = new BaseResult() { Success = true };
            UpdateParmShop model = entity as UpdateParmShop;
            if (model == null 
                || string.IsNullOrEmpty(model.id_masteruser)
                ||string.IsNullOrEmpty(model.id))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (string.IsNullOrEmpty(model.parmvalue))
            {
                res.Success = false;
                res.Message.Add("参数值不能为空!");
                return res;
            }
            if (string.IsNullOrEmpty(model.id_shop)||string.IsNullOrEmpty(model.id_shop_master))
            {
                res.Success = false;
                res.Message.Add("操作失败!");
                return res;
            }

            var id_shop= model.id_shop;
            var parmvalue = model.parmvalue;
            var id_masteruser = model.id_masteruser;
            var id = model.id;

            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("id", id);
            var oldModel = DAL.GetItem<Ts_Parm_Shop>(typeof(Ts_Parm_Shop), ht);
            if (oldModel == null)
            {
                res.Success = false;
                res.Message.Add("数据已不存在!");
                return res;
            }
            if (!ParmValidation.Valid(oldModel.regex, parmvalue, res)&&oldModel.flag_editstyle!=1)
            {
                return res;
            }
            if (ParmValidation.NeedValidParmCode(oldModel.parmcode) && oldModel.flag_editstyle != 1)
            {
                if (!ParmValidation.ValidDigit(parmvalue, oldModel.parmvalue, res))
                {
                    return res;
                }
            }
            Hashtable param=new Hashtable();
            if (model.id_shop == oldModel.id_shop && model.id_shop==model.id_shop_master)
            {
                param.Clear();
                param.Add("id_masteruser", model.id_masteruser);
                param.Add("id", model.id);
                param.Add("new_parmvalue",model.parmvalue);
                DAL.UpdatePart(typeof(Ts_Parm_Shop), param);
            }
            else
            {
                param.Clear();
                param.Add("id_masteruser", model.id_masteruser);
                param.Add("id_shop", id_shop);
                param.Add("parmcode", oldModel.parmcode);
                var count = DAL.GetCount(typeof(Ts_Parm_Shop), param);
                if (count == 1)
                {
                    param.Add("new_parmvalue", model.parmvalue);
                    DAL.UpdatePart(typeof(Ts_Parm_Shop), param);
                }
                else if (count==0)
                {
                    Ts_Parm_Shop parmShop=new Ts_Parm_Shop()
                    {
                        id=GetGuid,
                        flag_editstyle = oldModel.flag_editstyle,
                        id_masteruser = id_masteruser,
                        id_shop = id_shop,
                        parmcode = oldModel.parmcode,
                        parmvalue = model.parmvalue,
                        parmdescribe = oldModel.parmdescribe,
                        regex = oldModel.regex,
                        sort_id = oldModel.sort_id,
                        version = oldModel.version,
                        parmname = oldModel.parmname
                    };
                    DAL.Add(parmShop);
                }
                else
                {
                    res.Success = false;
                    res.Message.Add("数据有误，请联系管理员!");
                    return res;
                }

            }
            DataCache.Remove(id_masteruser + "_" + id_shop + "_GetShopParm");
            return res;
        }
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            var id = param["id"].ToString();
            if (string.IsNullOrEmpty(id))
            {
                res.Success = false;
                res.Message.Add("删除失败");
                return res;
            }
            param.Clear();
            param.Add("id", id);
            DAL.Delete(typeof(Ts_Parm_Shop), param);
            return res;
        }

    }
}
