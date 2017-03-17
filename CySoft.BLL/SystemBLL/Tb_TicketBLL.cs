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
using CySoft.Utility;

namespace CySoft.BLL.SystemBLL
{
    public class Tb_TicketBLL : BaseBLL
    {
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult res = new BaseResult() { Success = true };
            var model = entity as Tb_Ticket;
            if (model == null
                || string.IsNullOrEmpty(model.ticket)
                || string.IsNullOrEmpty(model.key_y))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            Hashtable param = new Hashtable();
            param.Add("key_y", model.key_y);
            var ticketModel = DAL.GetItem<Tb_Ticket>(typeof(Tb_Ticket), param);
            if (ticketModel == null)
            {
                ticketModel = model;
                DAL.Add(model);
            }
            else
            {
                res.Data = ticketModel.ticket;
            }

            if (DataCache.Get(model.key_y + "_RegisterTicket") != null && !string.IsNullOrEmpty(((Tb_Ticket)DataCache.Get(model.key_y + "_RegisterTicket")).id))
                DataCache.Remove(model.key_y + "_RegisterTicket");

            return res;
        }

        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };

            if (param == null || param.Count <= 0)
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            res.Data = DAL.GetItem<Tb_Ticket>(typeof(Tb_Ticket), param);
            return res;
        }
    }
}
