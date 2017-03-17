using System;
using System.Collections;
using CySoft.BLL.Base;
using System.Linq;
using CySoft.Frame.Core;
using CySoft.Frame.Common;
using CySoft.Model.Flags;
using CySoft.Model.Tb;
using CySoft.Utility;
using System.Collections.Generic;
using CySoft.Frame.Attributes;
using CySoft.IBLL;
using CySoft.IDAL;
using CySoft.BLL.Tools.CodingRule;
using CySoft.BLL.SystemBLL;

#region 用户接口凭证
#endregion
namespace CySoft.BLL.UserBLL
{
    public class UserAccreditBLL : BaseBLL
    {
        protected static readonly Type classType = typeof(Tb_User_Accredit);

        /// <summary>
        /// 新增
        /// tim
        /// 2015-06-17
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();

            var model = (Tb_User_Accredit)entity;
            if (model == null || !model.id_master.HasValue || model.id_master.Value.Equals(0))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("用户信息丢失，请重新登录。");
                return br;
            }

            if (model.name.IsEmpty())
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("凭证名称不能为空。");
                return br;
            }
            Hashtable ht = new Hashtable();
            ht.Add("id_master",model.id_master);
            if (!(DAL.GetCount(classType, ht) < 5))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("每个用户只允许产生5个凭证，请先删除不用的凭证再新增。");
                return br;
            }

            model.appkey = Guid.NewGuid().ToString().Replace("-","");

            ht.Clear();
            ht.Add("appkey", model.appkey);
            while (!DAL.GetCount(classType,ht).Equals(0))
            {
                model.appkey = Guid.NewGuid().ToString().Replace("-", "");
                ht.Clear();
                ht.Add("appkey", model.appkey);
            }

            model.secret = CySoft.Frame.Common.Rand.RandCode(8);

            DAL.Add<Tb_User_Accredit>(model);
            br.Success = true;
            br.Message.Add("用户接口凭证产生成功");
            return br;
        }

        /// <summary>
        /// 删除
        /// tim
        /// 2015-06-17
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();

            if (param == null || !param.ContainsKey("appkey") || !param.ContainsKey("id_master"))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("删除凭证的信息不能为空。");
                return br;
            }

            DAL.Delete(classType,param);
            br.Success = true;
            br.Message.Add("用户接口凭证删除成功");
            return br;
        }

        /// <summary>
        /// 不分页查询
        /// tim
        /// 2015-06-17
        /// </summary>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Tb_User_Accredit>(classType,param);
            br.Success = true;
            return br;
        }
    }
}
