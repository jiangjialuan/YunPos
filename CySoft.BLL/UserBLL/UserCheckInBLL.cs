using System.Collections.Generic;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Model.Tb;
using CySoft.Frame.Attributes;

namespace CySoft.BLL.UserBLL
{
    public class UserCheckInBLL:BaseBLL
    {
        /// <summary>
        /// 业务员签到
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Tb_User_Checkin checkIn = (Tb_User_Checkin)entity;

            if (checkIn != null)
            {
                DAL.Add(checkIn);
                br.Success = true;
                br.Message.Add("业务员签到成功！");
            }
            return br;
        }

        /// <summary>
        /// 分页获取签到记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.GetCount(typeof(Tb_User_Checkin), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_User_Checkin>(typeof(Tb_User_Checkin), param) ?? new List<Tb_User_Checkin>();
                pn.Success = true;
            }
            return pn;
        }

        /// <summary>
        /// 获取签到记录详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.GetItem(typeof(Tb_User_Checkin), param);
            br.Success = true;
            return br;
        }
    }
}
