using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using CySoft.Model.Ts;
using CySoft.Model.Enums;

namespace CySoft.BLL.UserBLL
{
    public class Tb_User_ShopBLL:BaseBLL
    {
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult res=new BaseResult(){Success = true};
            if (param==null||param.Count<=0||!param.ContainsKey("id_masteruser"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }

            //if (!param.ContainsKey("flag_delete"))
            //{
            //    param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            //}

            res.Data = DAL.QueryList<Tb_User_ShopWithShopMc>(typeof(Tb_User_Shop), param);
            return res;
        }
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult res=new BaseResult(){Success = true};
            var param= entity as Hashtable;
            if (param == null 
                || param.Count <= 0 
                || param.ContainsKey("id_masteruser")
                || param.ContainsKey("id_user")
                || param.ContainsKey("id_shop_list"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            var id_masteruser = param["id_masteruser"] + "";
            var id_user = param["id_user"] + "";
            var id_shop_list = param["id_shop_list"] + "";
            if (string.IsNullOrWhiteSpace(id_masteruser) || string.IsNullOrEmpty(id_user) || string.IsNullOrEmpty(id_shop_list))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            var arr = id_shop_list.Split(',');
            List<Tb_User_Shop> userShops=new List<Tb_User_Shop>();
            foreach (var s in arr)
            {
                userShops.Add(new Tb_User_Shop()
                {
                    id_masteruser = id_masteruser,
                    id_user = id_user,
                    id_shop = s,
                    id_create = param["id_edit"] == null ? id_masteruser : param["id_edit"].ToString(),
                    id_edit = param["id_edit"] == null ? id_masteruser : param["id_edit"].ToString(),
                    rq_create = DateTime.Now,
                    rq_edit = DateTime.Now
                    //flag_delete=(byte)Enums.FlagDelete.NoDelete
                });
            }
            DAL.AddRange(userShops);
            return res;
        }

        //public override BaseResult Update(dynamic entity)
        //{
        //    BaseResult res = new BaseResult() { Success = true };
        //    var model = entity as Tb_User_Shop;
        //    if (model == null
        //        || string.IsNullOrEmpty(model.id_user)
        //        || string.IsNullOrEmpty(model.id_shop)
        //        )
        //    {
        //        res.Success = false;
        //        res.Message.Add("参数有误!");
        //        return res;
        //    }
        //    Hashtable param=new Hashtable();
        //    param.Add("id_user", model.id_user);
        //    param.Add("id_user", model.id_shop);
        //    return res;
        //}

    }
}
