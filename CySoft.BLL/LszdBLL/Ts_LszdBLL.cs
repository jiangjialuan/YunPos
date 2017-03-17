using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Model.Other;
using CySoft.Model.Ts;
using Spring.Collections;

namespace CySoft.BLL.LszdBLL
{
    public class Ts_LszdBLL:BaseBLL
    {
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult res=new BaseResult(){Success = true};
            var model= entity as Ts_Lszd;
            if (model == null 
                || string.IsNullOrEmpty(model.id_masteruser)
                || string.IsNullOrEmpty(model.id_shop)
                || string.IsNullOrEmpty(model.mac))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            Hashtable param = new Hashtable();
            param.Add("mac", model.mac);
            param.Add("id_shop", model.id_shop);
            param.Add("id_masteruser", model.id_masteruser);
            param.Add("flag_delete",(byte)Enums.FlagDelete.NoDelete);
            var lszdModel= DAL.GetItem<Ts_Lszd>(typeof(Ts_Lszd), param);
            if (lszdModel == null)
            {
                model.flag_delete = (byte) Enums.FlagDelete.NoDelete;
                model.id = GetGuid;
                res.Data = model.id;
                DAL.Add(model);
                lszdModel=DAL.GetItem<Ts_Lszd>(typeof(Ts_Lszd), param);
                if (lszdModel!=null)
                {
                    res.Data = new {id = lszdModel.id, discern_no = lszdModel.discern_no};
                }
            }
            else
            {
                res.Data = new {id = lszdModel.id, discern_no = lszdModel.discern_no};
            }
            return res;
        }
    }
}
