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

namespace CySoft.Controllers.Buyers
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class RecieverAddressController : BaseController
    {
        /// <summary>
        /// 不分页收货地址列表
        /// znt 2015-03-24
        /// </summary>
        public ActionResult List()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            try
            {
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                param.Add("sort", "flag_default");
                param.Add("dir", "desc");
                br = BusinessFactory.RecieverAddress.GetAll(param);

                if (br.Data == null)
                {
                    br.Data = new List<Tb_Cgs_Shdz_Query>();
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

        /// <summary>
        /// 新增 
        /// znt 2015-03-25
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Add()
        {
            Tb_Cgs_Shdz_Query model = new Tb_Cgs_Shdz_Query();

            ViewData["action"] = "AddJson";
            return PartialView("_EditControl", model);
        }

        /// <summary>
        /// 新增 业务
        /// znt 2015-03-25
        /// </summary>
        [HttpPost]
        public ActionResult AddJson(Tb_Cgs_Shdz model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();

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
            if (string.IsNullOrEmpty(model.address))
            {
                br.Success = false;
                br.Message.Add("收货人详细地址不能为空");
                br.Data = "shr";
                return Json(br);
            }

            try
            {
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
        /// 删除 收货信息
        /// znt 2015-03-25
        /// </summary>
        [HttpPost]
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();

            if (string.IsNullOrEmpty(param["id"].ToString()))
            {
                br.Success = false;
                br.Message.Add("少了删除的关键信息！请联系管理员！");
                br.Data = "id";
                return Json(br);
            }

            try
            {
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
        /// 修改 收货信息 
        /// znt 2015-03-25
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Update()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();

            try
            {

                br = BusinessFactory.RecieverAddress.Get(param);
                Tb_Cgs_Shdz_Query model = (Tb_Cgs_Shdz_Query)br.Data;
                if (model == null)
                {
                    return Json(br);
                }
                ViewData["action"] = "UpdateJson";
                return PartialView("_EditControl", model);
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
        /// 修改 收货信息  
        /// znt 2015-03-25
        /// </summary>
        [HttpPost]
        public ActionResult UpdateJson(Tb_Cgs_Shdz model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            try
            {
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
        /// 设置 默认地址
        /// znt 2015-03-26
        /// </summary>
        [HttpPost]
        public ActionResult SettingDefault(Tb_Cgs_Shdz model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            try
            {
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

        /// <summary>
        /// 编辑 
        /// znt 2015-03-26
        /// </summary>
        public ActionResult EditList()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();

            if (string.IsNullOrEmpty(param["id_cgs"].ToString()))
            {
                br.Success = false;
                br.Data = "none";
                br.Message.Add("校验失败！请重新刷新页面。");
                return Json(br);
            }

            try
            {
                param.Add("sort", "flag_default");
                param.Add("dir", "desc");
                br = BusinessFactory.RecieverAddress.GetAll(param);

                if (br.Data == null)
                {
                    br.Data = new List<Tb_Cgs_Shdz_Query>();
                }
                List<Tb_Cgs_Shdz_Query> list = br.Data as List<Tb_Cgs_Shdz_Query>;

                return PartialView("_EditListControl",list);
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
