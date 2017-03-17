using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Tb;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.IDAL;
using CySoft.Frame.Attributes;

namespace CySoft.BLL.InfoBLL
{
    public class InfoUserBLL : BillBLL
    {
        public IInfo_UserDAL Info_UserDAL { get; set; }

        /// <summary>
        /// 2015-6-24
        /// wzp
        /// 记录接收公告新息人群
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult  Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            int count =Info_UserDAL.BatchInsert_User(typeof(Info_User), param);
            //获取发送数量
            long infoId = long.Parse(param["infoId"].ToString());
            param.Clear();
            param.Add("id_info", infoId);
            br.Data = DAL.GetCount(typeof(Info_User), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 分页获取我的通知信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.GetCount(typeof(Info_User), param);
            if (pn.TotalCount > 0)
            {
                IList<Info_Query> lst = DAL.QueryPage<Info_Query>(typeof(Info_User), param);
                pn.Data = lst;
            }
            return pn;
        }

        /// <summary>
        /// 获取通知数量
        /// mq 2016-05-27 修改 排除升级，系统，业务类型通知数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetCount(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            if (param.ContainsKey("type"))
            {
                string[] bm = new string[] { "update", "system", "business" };
                param.Add("bmList", bm);
            }
            br.Data = DAL.GetCount(typeof(Info_User), param);
            return br;
        }

        /// <summary>
        /// 获取所有员工
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Tb_User>(typeof(Tb_User), param)?? new List<Tb_User>();
            br.Success = true;
            return br;
        }
        public override void Add(Info_User info)
        {
            DAL.Add(info);
        }
        /// <summary>
        /// 获取用户未读公告数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override int GetGGCount(Hashtable param = null)
        {
            int count = 0;
            int count_user = 0;
            Info_Type info = new Info_Type();
            info = DAL.GetItem<Info_Type>(typeof(Info_Type), param);
            Hashtable hb = new Hashtable();
            if (param["bm"].ToString() == "business")
            {
                hb["flag_reade"] = 0;
                hb["id_user"] = param["id_user"];
                hb["business_id"] = info.id;
                count = DAL.GetCount(typeof(Info_User), hb);
            }
            else
            {
                hb["id_info_type"] = info.id;
                count = DAL.GetCount(typeof(Info), hb);
                hb["id_user"] = param["id_user"];
                count_user = Info_UserDAL.QueryCountOfGG(typeof(Info_User), hb);
            }
            return count - count_user;
        }
    }
}
