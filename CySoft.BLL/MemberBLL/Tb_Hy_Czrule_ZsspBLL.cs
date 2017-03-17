#region Imports
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.IDAL;
using CySoft.Model.Tb;
using System.Collections;
using System.Linq;
#endregion

namespace CySoft.BLL.MemberBLL
{
    public class Tb_Hy_Czrule_ZsspBLL : BaseBLL
    {


        #region 获取所有符合条件的数据
        /// <summary>
        /// 获取所有符合条件的数据
        /// lz
        /// 2016-11-16
        /// </summary>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult result = new BaseResult() { Success = true };
            if (param == null)
            {
                result.Success = false;
                result.Level = ErrorLevel.Warning;
                result.Message.Add("参数有误!");
                return result;
            }
            result.Data = DAL.QueryList<Tb_Hy_Czrule_Zssp_Query>(typeof(Tb_Hy_Czrule_Zssp), param).ToList();
            return result;
        }
        #endregion



    }
}