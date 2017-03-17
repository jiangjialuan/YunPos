using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Controllers.Base;
using System.Web.Mvc;
using CySoft.Controllers.Filters;
using System.Web.UI;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Tb;
using CySoft.Model.Flags;
using CySoft.Controllers.Service.Base;
using CySoft.Utility;

namespace CySoft.Controllers.Buyers
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceRecieverAddressController : ServiceBaseController
    {
        /// <summary>
        /// 收货地址列表
        /// cxb
        /// 2015-4-2
        /// </summary>
        [HttpPost]
        public ActionResult List()
        {

            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                param.Add("sort", "flag_default");
                param.Add("dir", "desc");
                br = BusinessFactory.RecieverAddress.GetAll(param);

                if (br.Data == null)
                {
                    br.Data = new List<Tb_Cgs_Shdz_Query>();
                }

                return Json(br);
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

        /// <summary>
        /// 采购商收货地址列表
        /// tim
        /// 2015-7-29
        /// </summary>
        [HttpPost]
        public ActionResult GetAll(string obj)
        {

            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                if (!param.ContainsKey("id_cgs") || param["id_cgs"].ToString().IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("采购商不能为空");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                param.Add("sort", "flag_default");
                param.Add("dir", "desc");
                br = BusinessFactory.RecieverAddress.GetAll(param);

                if (br.Data == null)
                {
                    br.Data = new List<Tb_Cgs_Shdz_Query>();
                }
            }
            catch (CySoftException ex)
            {

                br.Success = false;
                br.Message.Add(ex.Message);
                br.Level = ErrorLevel.Warning;                
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Message.Add(ex.Message);
                br.Level = ErrorLevel.Warning;  
            }
            return Json(br);
        }

        /// <summary>
        /// 新增收货地址
        /// cxb
        /// 2015-4-2
        /// </summary>
        [HttpPost]
        public ActionResult AddJson(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
             Tb_Cgs_Shdz model = JSON.Deserialize<Tb_Cgs_Shdz>(obj);
            if (string.IsNullOrEmpty(model.shr))
            {
                br.Success = false;
                br.Message.Add("收货人不能为空");
                br.Data = "shr";
                return Json(br);
            }
            if (string.IsNullOrEmpty(model.phone) && string.IsNullOrEmpty(model.tel))
            {
                br.Success = false;
                br.Message.Add("手机或电话号码，必填一项");
                br.Data = "shr";
                return Json(br);
            }
            //if (string.IsNullOrEmpty(model.address))
            //{
            //    br.Success = false;
            //    br.Message.Add("收货人详细地址不能为空");
            //    br.Data = "shr";
            //    return Json(br);
            //}

          
                model.id = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Cgs_Shdz));
                model.id_cgs = GetLoginInfo<long>("id_buyer");
                model.id_create = GetLoginInfo<long>("id_user");
                model.id_edit = GetLoginInfo<long>("id_user");
                model.rq_create = DateTime.Now;
                model.rq_edit = DateTime.Now;
                br = BusinessFactory.RecieverAddress.Add(model);
                return Json(br);

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

        /// <summary>
        ///删除收货地址
        ///cxb
        ///2015-4-2
        /// </summary>
        [HttpPost]
        public ActionResult Delete(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
            Hashtable param = JSON.Deserialize<Hashtable>(obj);
            if (string.IsNullOrEmpty(param["id"].ToString()))
            {
                br.Success = false;
                br.Message.Add("少了删除的关键信息！请联系管理员！");
                br.Data = "id";
                return Json(br);
            }
                br = BusinessFactory.RecieverAddress.Delete(param);
                return Json(br);
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

        /// <summary>
        /// 修改收货地址
        /// cxb
        /// 2015-4-2
        /// </summary>
        [HttpPost]
        public ActionResult UpdateJson(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Tb_Cgs_Shdz model = JSON.Deserialize<Tb_Cgs_Shdz>(obj);
                model.id_create = GetLoginInfo<long>("id_user");
                model.id_edit = GetLoginInfo<long>("id_user");
                model.rq_create = DateTime.Now;
                model.rq_edit = DateTime.Now;
                br = BusinessFactory.RecieverAddress.Update(model);
                return Json(br);
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

        /// <summary>
        /// 设置为默认收货地址
        /// cxb
        /// 2015-4-2
        /// </summary>
        [HttpPost]
        public ActionResult SettingDefault(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Tb_Cgs_Shdz model = JSON.Deserialize<Tb_Cgs_Shdz>(obj);
                model.id_cgs = GetLoginInfo<long>("id_buyer");
                model.flag_default =YesNoFlag.Yes;
                model.id_create = GetLoginInfo<long>("id_user");
                model.id_edit = GetLoginInfo<long>("id_user");
                model.rq_create = DateTime.Now;
                model.rq_edit = DateTime.Now;
                br= BusinessFactory.RecieverAddress.SettingDefault(model);
                return Json(br);
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

    
    }
}
