using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CySoft.Controllers.AccountCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class OpenAPIController : BaseController
    {
        /// <summary>      
        /// </summary>
        public ActionResult Index()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            try
            {
                param.Add("id_master", GetLoginInfo<long>("id_user_master"));
                br = BusinessFactory.UserAccredit.GetAll(param);

                if (br.Data == null)
                {
                    br.Data = new List<Tb_User_Accredit>();
                }

                return View(br.Data);
            }
            catch (CySoftException ex)
            {

                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public ActionResult Add(Tb_User_Accredit model)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("name", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                model.id_master = GetLoginInfo<long>("id_user_master");
                br = BusinessFactory.UserAccredit.Add(model);              
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

        [HttpPost]
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("appkey", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                param.Add("id_master", GetLoginInfo<long>("id_user_master"));
                br = BusinessFactory.UserAccredit.Delete(param);
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
