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

#region 主用户管理
#endregion

namespace CySoft.BLL.AdminBLL
{
    public class UserMasterBLL : BaseBLL
    {
        protected static readonly Type classType = typeof(Tb_User);
        private IUserDAL Tb_UserDAL { get; set; }
        /// <summary>
        /// 分页查询
        /// tim
        /// 2015-09-29
        /// </summary>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            param.Add("flag_master",1);
            pn.TotalCount = Tb_UserDAL.GetCount(classType, param);
            if (pn.TotalCount > 0)
            {

                var lst = Tb_UserDAL.PageUserMaster(classType, param) ?? new List<Tb_User_Master>();               
                pn.Data = lst;
            }
            else
            {
                pn.Data = new List<Tb_User_Master>();
            }
            pn.Success = true;
            return pn;
        }
    }
}
