using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using CySoft.Model.Td;
using CySoft.Utility;

#region 购物车
#endregion

namespace CySoft.BLL.GoodsBLL
{
    public class GoodsCartBLL : BaseBLL
    {

        /// <summary>
        /// 新增
        /// znt 2015-03-20
        /// 2015-03-14 lxt 修改
        /// </summary>
        public override BaseResult Save(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Td_Sale_Cart model = (Td_Sale_Cart)entity;
            Hashtable param = new Hashtable();
            param.Add("id_cgs", model.id_cgs);
            param.Add("id_sp", model.id_sp);
            param.Add("id_sku", model.id_sku);
            param.Add("id_gys", model.id_gys);
            if (DAL.GetCount(typeof(Td_Sale_Cart), param) > 0)
            {
                param.Add("new_sl", model.sl);
                DAL.UpdatePart(typeof(Td_Sale_Cart), param);
                br.Success = true;
                return br;
            }
            else
            {
                DAL.Add(model);
            }

            br.Success = true;
            return br;
        }

        /// <summary>
        /// 删除 
        /// znt 2015-03-13
        /// </summary>
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            DAL.Delete(typeof(Td_Sale_Cart), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 购物车数量
        /// znt 2015-03-14
        /// </summary>
        public override BaseResult GetCount(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.GetCount(typeof(Td_Sale_Cart), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 查询 单体
        /// znt 2015-03-14 
        /// </summary>
        public override BaseResult Get(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            Td_Sale_Cart_Query model = DAL.GetItem<Td_Sale_Cart_Query>(typeof(Td_Sale_Cart), param);
            br.Data = model;
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 不分页查询
        /// znt 2015-03-14 
        /// 2015-03-14 lxt 修改
        /// </summary>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            long id_buyer = Convert.ToInt64(param["id_cgs"]);
            IList<Td_Sale_Cart_Query> list = DAL.QueryList<Td_Sale_Cart_Query>(typeof(Td_Sale_Cart), param);
            param.Clear();
            param.Add("flag_cart", 1);
            param.Add("id_cgs", id_buyer);
            IList<Tb_Sp_Expand_Query> list_Expand = DAL.QueryList<Tb_Sp_Expand_Query>(typeof(Tb_Sp_Expand), param);

            foreach (var item in list)
            {
                var query = from m in list_Expand where m.id_sku == item.id_sku select m;
                item.gg = query.Join(m => m.mc + "：" + m.val, "，");
            }
            br.Data = list;
            br.Success = true;
            return br;
        }

    }
}
