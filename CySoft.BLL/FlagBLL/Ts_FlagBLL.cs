using System;
using System.Collections;
using System.Linq;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Ts;

namespace CySoft.BLL.FlagBLL
{
    public class Ts_FlagBLL : BaseBLL
    {
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult res=new BaseResult(){Success = true};
            try
            {
                res.Data = DAL.QueryList<Ts_Flag>(typeof(Ts_Flag), param).ToList();
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message.Add("操作异常");
            }
            return res;
        }
    }
}
