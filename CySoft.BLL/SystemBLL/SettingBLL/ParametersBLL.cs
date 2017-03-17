using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Model.Ts;

#region 系统参数
#endregion

namespace CySoft.BLL.SystemBLL.SettingBLL
{
    public class ParametersBLL : BaseBLL
    {
        /// <summary>
        /// 不分页查询
        /// lxt
        /// 2015-04-10
        /// </summary>
        #region public override BaseResult GetAll(Hashtable param)
        public override BaseResult GetAll(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Ts_Param>(typeof(Ts_Param), param);
            br.Success = true;
            return br;
        }
        #endregion
    }
}
