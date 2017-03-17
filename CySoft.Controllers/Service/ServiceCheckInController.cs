using System;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Core;
using CySoft.Utility;
using CySoft.Model.Tb;
using System.Collections;
using CySoft.Utility.Mvc.Html;

namespace CySoft.Controllers.Service
{
    /// <summary>
    /// 业务员签到接口
    /// 2015-7-15 wzp
    /// </summary>
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceCheckInController : ServiceBaseController
    {
        /// <summary>
        /// 分页获取签到记录信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ActionResult GetCheckRecord(string obj)
        {
            int limit = 15;
            PageNavigate pn = new PageNavigate();
            PageList<Tb_User_Checkin> lst = new PageList<Tb_User_Checkin>(limit);
            Hashtable param = JSON.Deserialize<Hashtable>(obj);
            ParamVessel p = new ParamVessel();
            p.Add("id_user", string.Empty, HandleType.ReturnMsg);
            p.Add("pageIndex", 1, HandleType.DefaultValue);
            param = param.Trim(p);
            try
            {
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                pageIndex = !(pageIndex > 0) ? 1 : pageIndex;
                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                param.Add("sort", "id");
                param.Add("dir", "desc");
                pn = BusinessFactory.UserCheckIn.GetPage(param);
                lst = new PageList<Tb_User_Checkin>(pn, pageIndex, limit);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return Json(lst);
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ActionResult CheckIn(string obj)
        {
            Hashtable param = JSON.Deserialize<Hashtable>(obj);
            ParamVessel p = new ParamVessel();
            p.Add("id_user", (long)0, HandleType.ReturnMsg);
            p.Add("location",string.Empty,HandleType.ReturnMsg);
            p.Add("des", string.Empty, HandleType.ReturnMsg);
            p.Add("id_create",0,HandleType.DefaultValue);
            param = param.Trim(p);
            if (param["id_create"].ToString().Equals("0"))
            {
                param["id_create"] = param["id_user"];
            }
            BaseResult br = new BaseResult();
            try
            {
                Tb_User_Checkin checkIn = new Tb_User_Checkin();
                checkIn.des = param["des"].ToString();
                checkIn.id_user = param["id_user"].ToString();
                checkIn.id_create = param["id_create"].ToString();
                checkIn.location = param["location"].ToString();
                br = BusinessFactory.UserCheckIn.Add(checkIn);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(br);
        }
    }
}
