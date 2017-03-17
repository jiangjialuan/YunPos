using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.IDAL;
using CySoft.Model.Tb;
using CySoft.Model.Flags;

namespace CySoft.BLL.GoodsBLL
{
    public class GoodsServiceBLL : BaseBLL, IGoodsServiceBLL
    {
        public ITb_SpDAL Tb_SpDAL { get; set; }
        public ITb_Sp_SkuDAL Tb_Sp_SkuDAL { get; set; }
        public ITb_Sp_DjDAL Tb_Sp_DjDAL { get; set; }

        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = Tb_SpDAL.QueryCountOfService(typeof(Tb_Sp), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = Tb_SpDAL.QueryPageOfService(typeof(Tb_Sp), param);
            }
            else
            {
                pn.Data = new List<Tb_Sp_Query>();
            }
            pn.Success = true;
            return pn;
        }

        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string id = param.ContainsKey("id") ? param["id"].ToString():string.Empty;
            string mc = param.ContainsKey("mc") ? param["mc"].ToString():string.Empty;
            long id_gys = Convert.ToInt64(param["id_gys"]);
            long id_cgs = Convert.ToInt64(param["id_cgs"] ?? 0);
            object baseurl = param["baseurl"];
            var model = new Tb_Sp_Get();

            Hashtable ht = new Hashtable();
            if (param.ContainsKey("id")) ht.Add("id", param["id"]);
            if (param.ContainsKey("mc")) ht.Add("mc", param["mc"]);

            var sp = DAL.GetItem<Tb_Sp>(typeof(Tb_Sp), ht);
            if (sp == null)
            {
                br.Success = false;
                br.Message.Add("商品不存在.");
                return br;
            }
            model.id = sp.id.Value;
            model.mc = sp.mc;
            model.brand = sp.brand;
            model.cd = sp.cd;            
            model.id_gys_create = sp.id_gys_create.GetValueOrDefault(0);

            ht.Clear();
            ht.Add("id_sp",sp.id);
            ht.Add("id_gys", param["id_gys"]);
            if (param.ContainsKey("baseurl") && !string.IsNullOrWhiteSpace(param["baseurl"].ToString())) ht.Add("baseurl", param["baseurl"]);
            if (id_cgs > 0)
            {
                ht.Add("id_cgs", id_cgs);
                ht.Add("flag_up", YesNoFlag.Yes);
                ht.Add("flag_stop", YesNoFlag.No);
            }
            else
            {
                model.levelPriceList = Tb_Sp_DjDAL.QueryList1(typeof(Tb_Sp_Dj), ht);
            }
            model.skuList = Tb_Sp_SkuDAL.QueryList1(typeof(Tb_Sp_Sku), ht);

            ht.Clear();
            ht.Add("id_sp", sp.id);
            var ggList = DAL.QueryList<Tb_Sp_Expand>(typeof(Tb_Sp_Expand), ht);
            foreach (var item in model.skuList)
            {
                item.specList = (from gg in ggList where gg.id_sku == item.id_sku select new Tb_Sp_Sku_Spec(gg.id_sp_expand_template.Value, gg.val)).ToList();
            }
            var id_sp_expand_templateList = (from item in ggList group item by item.id_sp_expand_template.Value into g select g.Key).ToList();
            param.Clear();
            param.Add("idList", id_sp_expand_templateList);
            var sp_expand_templateList = DAL.QueryList<Tb_Sp_Expand_Template>(typeof(Tb_Sp_Expand_Template), param);

            var sp_expandGroupList = (from item in ggList group item by new { id_sp_expand_template = item.id_sp_expand_template, val = item.val } into g select g.Key).ToList();
            foreach (var item in sp_expand_templateList)
            {
                var items = (from sp_expandGroup in sp_expandGroupList where sp_expandGroup.id_sp_expand_template == item.id select sp_expandGroup.val).ToList();
                model.specGroupList.Add(new Tb_Sp_SpecGroup(item.id.Value, item.mc, items));
            }
            model.picList = DAL.QueryList<Tb_Sp_Pic>(typeof(Tb_Sp_Pic), ht);

            if (model.specGroupList.Count > 0)
            {
                ht.Clear();
                ht.Add("id_sun", model.specGroupList[0].id);
                model.specGroup = DAL.GetItem<Tb_Sp_Expand_Template>(typeof(Tb_Sp_Expand_Template), ht);
            }

            br.Data = model;
            br.Success = true;
            return br;
        }
    }
}
