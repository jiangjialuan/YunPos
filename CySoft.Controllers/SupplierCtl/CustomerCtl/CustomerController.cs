//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using System.Web.UI;
//using CySoft.Controllers.Base;
//using CySoft.Controllers.Filters;
//using CySoft.Frame.Core;
//using CySoft.Frame.Common;
//using CySoft.Model.Flags;
//using CySoft.Model.Tb;
//using CySoft.Utility;
//using CySoft.Utility.Mvc.Html;
//using System.IO;
//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;
//#region 客户管理
//#endregion

//namespace CySoft.Controllers.SupplierCtl.CustomerCtl
//{
//    [LoginActionFilter]
//    [ValidateInput(false)]
//    [OutputCache(Location = OutputCacheLocation.None)]
//    public class CustomerController : BaseController
//    {
//        //客户列表
//        public ActionResult List()
//        {
//            PageNavigate pn = new PageNavigate();
//            int limit = 10;
//            PageList<Tb_Gys_Cgs_Query> list = new PageList<Tb_Gys_Cgs_Query>(limit);
//            try
//            {
//                Hashtable param = base.GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("orderby", 2, HandleType.DefaultValue);//排序
//                p.Add("status", 1, HandleType.Remove);//是否开通
//                p.Add("from", String.Empty, HandleType.Remove);//来源
//                p.Add("level", (long)0, HandleType.Remove);//客户级别Id
//                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
//                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
//                p.Add("id_province", 0, HandleType.Remove);//省份
//                p.Add("id_city", 0, HandleType.Remove);//城市
//                p.Add("id_county", 0, HandleType.Remove);//镇
//                p.Add("flag_show", true, HandleType.Remove);//是否展开高级搜索
//                param = param.Trim(p);
//                if (param["flag_show"] != null)
//                {
//                    ViewData["flag_show"] = true;
//                }
//                switch (param["orderby"].ToString())
//                {
//                    case "1":
//                        param.Add("sort", "db.rq_create");
//                        param.Add("dir", "asc");
//                        break;
//                    case "2":
//                        param.Add("sort", "db.rq_create");
//                        param.Add("dir", "desc");
//                        break;
//                    case "3":
//                        param.Add("sort", "db.alias_cgs");
//                        param.Add("dir", "asc");
//                        break;
//                    case "4":
//                        param.Add("sort", "db.alias_cgs");
//                        param.Add("dir", "desc");
//                        break;
//                    case "5":
//                        param.Add("sort", "db.id_cgs_level");
//                        param.Add("dir", "asc");
//                        break;
//                    case "6":
//                        param.Add("sort", "db.id_cgs_level");
//                        param.Add("dir", "desc");
//                        break;
//                    case "7":
//                        param.Add("sort", "um.id_province desc,um.id_city desc,um.id_county");
//                        param.Add("dir", "asc");
//                        break;
//                    case "8":
//                        param.Add("sort", "um.id_province asc,um.id_city asc,um.id_county");
//                        param.Add("dir", "desc");
//                        break;
//                    default:
//                        param["orderby"] = 2;
//                        param.Add("sort", "db.rq_create");
//                        param.Add("dir", "desc");
//                        break;
//                }

//                int pageIndex = Convert.ToInt32(param["pageIndex"]);
//                if (param.ContainsKey("level"))
//                {
//                    ViewData["level"] = param["level"];
//                    param.Add("id_cgs_level", param["level"]);
//                    param.Remove("level");
//                }
//                if (param.ContainsKey("status"))
//                {
//                    ViewData["status"] = param["status"];
//                    param.Add("flag_actived", param["status"]);
//                    param.Remove("status");
//                }
//                if (param.ContainsKey("from"))
//                {
//                    ViewData["from"] = param["from"];
//                    param.Add("flag_from", param["from"]);
//                    param.Remove("from");
//                }

//                ViewData["keyword"] = GetParameter("keyword");
//                ViewData["orderby"] = param["orderby"];
//                ViewData["pageIndex"] = pageIndex;
//                ViewData["limit"] = limit;

//                param.Add("limit", limit);
//                param.Add("start", (pageIndex - 1) * limit);
//                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
//                param.Add("not_stop", 1);
//                pn = BusinessFactory.Customer.GetPage(param);
//                list = new PageList<Tb_Gys_Cgs_Query>(pn, pageIndex, limit);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//            if (Request.IsAjaxRequest())
//            {
//                return PartialView("_ListControl", list);
//            }

//            try
//            {
//                Hashtable param = new Hashtable();
//                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
//                BaseResult br = BusinessFactory.CustomerType.GetAll(param);
//                ViewData["typeList"] = br.Data;
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return View(list);
//        }

//        [ActionPurview(false)]
//        public FileResult Export()
//        {

//            #region 数据
//            PageNavigate pn = new PageNavigate();
//            int limit = 5000;
//            PageList<Tb_Gys_Cgs_Query> list = new PageList<Tb_Gys_Cgs_Query>(limit);
//            try
//            {
//                Hashtable param = base.GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("orderby", 1, HandleType.DefaultValue);//排序
//                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
//                param = param.Trim(p);
//                param["orderby"] = 1;
//                param.Add("sort", "db.rq_create");
//                param.Add("dir", "desc");
//                int pageIndex = Convert.ToInt32(param["pageIndex"]);
//                param.Add("limit", limit);
//                param.Add("start", (pageIndex - 1) * limit);
//                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
//                param.Add("not_stop", 1);
//                pn = BusinessFactory.Customer.GetPage(param);
//                list = new PageList<Tb_Gys_Cgs_Query>(pn, pageIndex, limit);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//            try
//            {
//                Hashtable param = new Hashtable();
//                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
//                BaseResult br = BusinessFactory.CustomerType.GetAll(param);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            #endregion

//            var fileName = Server.MapPath("~/Template/kh_template.xls");
//            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
//            IWorkbook hssfworkbook = new HSSFWorkbook(file);
//            ISheet sheet1 = hssfworkbook.GetSheet("客户数据");

//            for (int i = 0; i < list.Count; i++)
//            {
//                IRow rowtemp = sheet1.CreateRow(i + 2);
//                string name = list[i].companyname;
//                rowtemp.CreateCell(0).SetCellValue(name);
//                rowtemp.CreateCell(1).SetCellValue(list[i].bm_Interface);
//                rowtemp.CreateCell(2).SetCellValue(list[i].name_province + list[i].name_city + list[i].name_county);
//                rowtemp.CreateCell(3).SetCellValue(list[i].name_cgs_level);
//                rowtemp.CreateCell(9).SetCellValue(list[i].name);
//                rowtemp.CreateCell(11).SetCellValue(list[i].phone);
//            }
//            System.IO.MemoryStream ms = new System.IO.MemoryStream();
//            hssfworkbook.Write(ms);
//            ms.Seek(0, SeekOrigin.Begin);
//            file.Close();
//            return File(ms, "application/vnd.ms-excel", "客户数据_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");

//        }

//        /// <summary>
//        /// 拒绝申请
//        /// cxb
//        /// 2015-4-15
//        /// </summary>
//        [HttpPost]
//        public ActionResult RefusalAttention(Tb_Gys_Cgs_Check model)
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = new Hashtable();
//                param.Add("model", model);
//                param.Add("flag_from", "browser");
//                param.Add("id_user", GetLoginInfo<long>("id_user"));
//                br = BusinessFactory.SupplierAttention.Update(param);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        /// 供应商通过关注
//        /// cxb
//        /// 2015-4-15
//        /// </summary>
//        [HttpPost]
//        public ActionResult AllowsAttention(Tb_Cgs_Edit model)
//        {
//            BaseResult br = new BaseResult();
//            if (!model.id_cgs_level.HasValue || model.id_cgs_level < 1)
//            {
//                br.Success = false;
//                br.Message.Add("请选择客户级别");
//                br.Level = ErrorLevel.Warning;
//                br.Data = "customerLevel";
//                return Json(br);
//            }
//            try
//            {
//                model.id_create = GetLoginInfo<long>("id_user");
//                model.id_edit = model.id_create;
//                Hashtable param = new Hashtable();
//                param.Add("model", model);
//                param.Add("flag_from", "browser");
//                param.Add("id_supplier", GetLoginInfo<long>("id_supplier"));
//                br = BusinessFactory.SupplierAttention.Add(param);
//                if (br.Success)
//                {
//                    WriteDBLog(LogFlag.Base, br.Message);
//                }
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        /// 删除记录
//        /// cxb
//        /// 2015-4-15
//        /// </summary>
//        //[HttpPost]
//        //public ActionResult DeleteAttention() {
//        //    BaseResult br = new BaseResult();
//        //    try
//        //    {
//        //        Hashtable param = GetParameters();                
//        //        br = BusinessFactory.SupplierAttention.Delete(param);
//        //    }
//        //    catch (CySoftException ex)
//        //    {
//        //        throw ex;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        throw ex;
//        //    }
//        //    return Json(br);
//        //}

//        /// <summary>
//        /// 未审核客户列表
//        /// cxb
//        /// 2015-4-8
//        /// </summary>
//        public ActionResult NoreList()
//        {
//            PageNavigate pn = new PageNavigate();
//            int limit = 10;
//            PageList<Tb_Gys_Cgs_Check_Query> list = new PageList<Tb_Gys_Cgs_Check_Query>(limit);
//            try
//            {
//                Hashtable param = base.GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("orderby", 2, HandleType.DefaultValue);//排序
//                p.Add("status", string.Empty, HandleType.Remove);// 审批状态                
//                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
//                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
//                param = param.Trim(p);
//                switch (param["orderby"].ToString())
//                {
//                    case "1":
//                        param.Add("sort", "rq_sq");
//                        param.Add("dir", "asc");
//                        break;
//                    case "2":
//                        param.Add("sort", "rq_sq");
//                        param.Add("dir", "desc");
//                        break;
//                    default:
//                        param["orderby"] = 2;
//                        param.Add("sort", "rq_sq");
//                        param.Add("dir", "desc");
//                        break;
//                }
//                int pageIndex = Convert.ToInt32(param["pageIndex"]);

//                if (param.ContainsKey("status"))
//                {
//                    ViewData["status"] = param["status"];
//                    param.Add("flag_state", param["status"]);
//                    param.Remove("status");
//                }

//                ViewData["keyword"] = GetParameter("keyword");
//                ViewData["orderby"] = param["orderby"];
//                ViewData["pageIndex"] = pageIndex;
//                ViewData["limit"] = limit;

//                param.Add("limit", limit);
//                param.Add("start", (pageIndex - 1) * limit);
//                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));

//                pn = BusinessFactory.BuyerAttention.GetNorevPage(param);
//                list = new PageList<Tb_Gys_Cgs_Check_Query>(pn, pageIndex, limit);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//            if (Request.IsAjaxRequest())
//            {
//                return PartialView("_NoreListControl", list);
//            }

//            try
//            {
//                Hashtable param = new Hashtable();
//                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
//                BaseResult br = BusinessFactory.CustomerType.GetAll(param);
//                ViewData["typeList"] = br.Data;
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return View(list);
//        }



//        public ActionResult Edit(long? id)
//        {
//            //BaseResult br = new BaseResult();
//            //try
//            //{
//            //    id = TypeConvert.ToInt64(id, 0);
//            //    ViewData["flag_edit"] = id > 0 ? "Update" : "Add";
//            //    if (id > 0)
//            //    {
//            //        Hashtable param = new Hashtable();
//            //        param.Add("id", id);
//            //        param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
//            //        br = BusinessFactory.Customer.Get(param);

//            //        if (br.Data == null)
//            //        { br.Data = new Tb_Cgs_Edit(); ViewData["flag_edit"] = "Add"; }

//            //    }
//            //    else
//            //    {
//            //        br.Data = new Tb_Cgs_Edit();
//            //    }
//            //    if (Request.IsAjaxRequest())
//            //    {
//            //        return PartialView("_AllowAttentionControl", br.Data);
//            //    }
//            //    if (br.Data != null)
//            //    {
//            //        Hashtable param = new Hashtable();
//            //        param.Clear();
//            //        param["id_user"] = ((Tb_Cgs_Edit)br.Data).id_user_master;
//            //        param.Add("id_roleList", new long[] { 1, 3, 4 });
//            //        var role = BusinessFactory.UserRole.GetAll(param);
//            //        IList<Tb_User_Role> user_role = (IList<Tb_User_Role>)role.Data;
//            //        if (user_role != null && user_role.Count > 0)
//            //        {
//            //            var isSupplier = user_role.Where(d => d.id_role == 3).ToList().Count > 0;
//            //            var isBuyer = user_role.Where(d => d.id_role == 4).ToList().Count > 0;
//            //            if (isSupplier && isBuyer)
//            //            {
//            //                //平台商
//            //                ViewData["RoleType"] = "0";
//            //            }
//            //            else if (isSupplier)
//            //            {
//            //                //供应商
//            //                ViewData["RoleType"] = "0";
//            //            }
//            //            else if (isBuyer)
//            //            {
//            //                //采购商
//            //                ViewData["RoleType"] = "1";
//            //            }
//            //        }
//            //        else
//            //        {
//            //            ViewData["RoleType"] = "0";
//            //        }
//            //    }
//            //}
//            //catch (CySoftException ex)
//            //{
//            //    throw ex;
//            //}
//            //catch (Exception ex)
//            //{
//            //    throw ex;
//            //}
//            //return View(br.Data);
//            return View();
//        }

//        public ActionResult GetAttention(long? id)
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                id = TypeConvert.ToInt64(id, 0);

//                if (id > 0)
//                {
//                    Hashtable param = new Hashtable();
//                    param.Add("id_cgs", id);
//                    param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
//                    br = BusinessFactory.SupplierAttention.Get(param);
//                    //2015-7-23 wzp 修改：验证采购商是否存在图册
//                    param.Clear();
//                    Tb_Cgs_Attention cgs = (Tb_Cgs_Attention)br.Data;
//                    param.Add("id_master", cgs.id_user_master);
//                    int count = Convert.ToInt32(BusinessFactory.UserPic.GetCount(param).Data);
//                    int pageCount = (int)Math.Ceiling((double)(((double)count) / ((double)5)));
//                    ViewData["picCount"] = pageCount;

//                }
//                else
//                {
//                    br.Success = false;
//                    br.Message.Add("提交参数错误.");
//                    br.Level = ErrorLevel.Warning;
//                }
//                if (Request.IsAjaxRequest())
//                {
//                    if (br.Success)
//                        return PartialView("_AllowAttentionControl", br.Data);
//                }
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            if (br.Success)
//                return View(br.Data);
//            return Json(br);
//        }

//        /// <summary>
//        /// 分页获取采购商图册
//        /// wzp 2015-7-24
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult GetPicResult()
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = GetParameters();
//            ParamVessel p = new ParamVessel();
//            p.Add("pageIndex", 1, HandleType.DefaultValue);
//            p.Add("id_master", (long)0, HandleType.ReturnMsg);
//            param = param.Trim(p);
//            int limit = 5;
//            int pageIndex = int.Parse(param["pageIndex"].ToString());
//            PageList<Tb_User_Pic> lst = new PageList<Tb_User_Pic>(limit);
//            PageNavigate pn = new PageNavigate();
//            param.Add("start", (pageIndex - 1) * limit);
//            param.Add("limit", limit);
//            try
//            {
//                pn = BusinessFactory.UserPic.GetPage(param);
//                lst = new PageList<Tb_User_Pic>(pn, pageIndex, limit);
//                ViewData["pageCount"] = lst.PageCount;
//                br.Data = lst;
//                br.Success = true;
//                return Json(br);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        /// <summary>
//        /// 添加客户
//        /// lxt
//        /// 2015-02-26
//        /// </summary>
//        [HttpPost]
//        public ActionResult Add(Tb_Cgs_Edit model)
//        {
//            BaseResult br = new BaseResult();
//            if (model.companyname.IsEmpty())
//            {
//                br.Success = false;
//                br.Message.Add("客户名称不能为空");
//                br.Level = ErrorLevel.Warning;
//                br.Data = "companyName";
//                return Json(br);
//            }
//            if (model.id_cgs_level < 1)
//            {
//                br.Success = false;
//                br.Message.Add("请选择客户级别");
//                br.Level = ErrorLevel.Warning;
//                br.Data = "customerLevel";
//                return Json(br);
//            }
//            if (model.name.IsEmpty())
//            {
//                br.Success = false;
//                br.Message.Add("姓名不能为空");
//                br.Level = ErrorLevel.Warning;
//                br.Data = "name";
//                return Json(br);
//            }
//            if (model.phone.IsEmpty())
//            {
//                br.Success = false;
//                br.Message.Add("手机号不能为空");
//                br.Level = ErrorLevel.Warning;
//                br.Data = "phone";
//                return Json(br);
//            }
//            if (model.rq_treaty_start > new DateTime(1900, 1, 1) && model.rq_treaty_end > new DateTime(1900, 1, 1) && model.rq_treaty_start > model.rq_treaty_end)
//            {
//                br.Success = false;
//                br.Message.Add("合约有效期有误");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//            }
//            if (model.flag_active == YesNoFlag.Yes)
//            {
//                if (model.username.IsEmpty())
//                {
//                    br.Success = false;
//                    br.Message.Add("用户名不能为空");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "username";
//                    return Json(br);
//                }
//                if (model.password.IsEmpty())
//                {
//                    br.Success = false;
//                    br.Message.Add("密码不能为空");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "username";
//                    return Json(br);
//                }
//            }
//            try
//            {
//                model.id = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Cgs));
//                model.id_user_master = BusinessFactory.Utilety.GetNextKey(typeof(Tb_User));
//                model.id_cgs_shdz = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Cgs_Shdz));
//                model.id_gys = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Gys));
//                model.id_create = GetLoginInfo<long>("id_user");
//                model.id_edit = model.id_create;

//                Hashtable param = new Hashtable();
//                param.Add("model", model);
//                param.Add("id_supplier", GetLoginInfo<long>("id_supplier"));
//                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
//                param.Add("name_gys", GetLoginInfo<string>("companyname"));
//                param.Add("id_user", GetLoginInfo<long>("id_user"));
//                param.Add("flag_from", "browser");
//                br = BusinessFactory.Customer.Add(param);
//                if (br.Success)
//                {
//                    WriteDBLog(LogFlag.Base, br.Message);
//                }
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        /// 修改
//        /// lxt
//        /// 2015-02-27
//        /// </summary>
//        [HttpPost]
//        public ActionResult Update(Tb_Cgs_Edit model)
//        {
//            BaseResult br = new BaseResult();
//            if (model.id < 1 || model.id_user_master < 1)
//            {
//                br.Success = false;
//                br.Message.Add("提交的数据不完整，请刷新后再试");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//            }
//            if (model.companyname.IsEmpty())
//            {
//                br.Success = false;
//                br.Message.Add("客户名称不能为空");
//                br.Level = ErrorLevel.Warning;
//                br.Data = "companyName";
//                return Json(br);
//            }
//            if (model.id_cgs_level < 1)
//            {
//                br.Success = false;
//                br.Message.Add("请选择客户级别");
//                br.Level = ErrorLevel.Warning;
//                br.Data = "customerLevel";
//                return Json(br);
//            }

//            if (model.rq_treaty_start > new DateTime(1900, 1, 1) && model.rq_treaty_end > new DateTime(1900, 1, 1) && model.rq_treaty_start > model.rq_treaty_end)
//            {
//                br.Success = false;
//                br.Message.Add("合约有效期有误");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//            }
//            if (model.flag_activeed == YesNoFlag.No && model.flag_active == YesNoFlag.Yes)
//            {
//                if (model.name.IsEmpty())
//                {
//                    br.Success = false;
//                    br.Message.Add("姓名不能为空");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "name";
//                    return Json(br);
//                }
//                if (model.phone.IsEmpty())
//                {
//                    br.Success = false;
//                    br.Message.Add("手机号不能为空");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "phone";
//                    return Json(br);
//                }
//                if (model.username.IsEmpty())
//                {
//                    br.Success = false;
//                    br.Message.Add("用户名不能为空");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "username";
//                    return Json(br);
//                }
//                if (model.password.IsEmpty())
//                {
//                    br.Success = false;
//                    br.Message.Add("密码不能为空");
//                    br.Level = ErrorLevel.Warning;
//                    br.Data = "username";
//                    return Json(br);
//                }
//            }
//            try
//            {
//                model.id_edit = GetLoginInfo<long>("id_user");
//                model.id_cgs_shdz = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Cgs_Shdz));
//                Hashtable param = new Hashtable();
//                param.Add("model", model);
//                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
//                param.Add("name_gys", GetLoginInfo<string>("companyname"));
//                param.Add("id_user", GetLoginInfo<long>("id_user"));
//                param.Add("flag_from", "browser");
//                br = BusinessFactory.Customer.Update(param);
//                if (br.Success)
//                {
//                    WriteDBLog(LogFlag.Base, br.Message);
//                }
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }


//        /// <summary>
//        /// 取消关注
//        /// lxt
//        /// 2015-02-27
//        /// </summary>
//        [HttpPost]
//        public ActionResult CancelAttention()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("id", (long)0, HandleType.DefaultValue);
//                param = param.Trim(p);
//                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
//                param.Add("id_user", GetLoginInfo<long>("id_user"));
//                param.Add("flag_from", "browser");
//                br = BusinessFactory.Customer.Delete(param);
//                if (br.Success)
//                {
//                    WriteDBLog(LogFlag.Base, br.Message);
//                }
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        /// 停用
//        /// lxt
//        /// 2015-02-27
//        /// </summary>
//        [HttpPost]
//        public ActionResult Stop()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("id", (long)0, HandleType.ReturnMsg);
//                param = param.Trim(p);
//                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
//                param.Add("id_user", GetLoginInfo<long>("id_user"));
//                param.Add("flag_from", "browser");
//                br = BusinessFactory.Customer.Stop(param);
//                if (br.Success)
//                {
//                    WriteDBLog(LogFlag.Base, br.Message);
//                }
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        /// 启用
//        /// lxt
//        /// 2015-02-27
//        /// </summary>
//        [HttpPost]
//        public ActionResult Active()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("id", (long)0, HandleType.ReturnMsg);
//                param = param.Trim(p);
//                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
//                param.Add("id_user", GetLoginInfo<long>("id_user"));
//                param.Add("flag_from", "browser");
//                br = BusinessFactory.Customer.Active(param);
//                if (br.Success)
//                {
//                    WriteDBLog(LogFlag.Base, br.Message);
//                }
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        ///  供应商关注客户列表（弹出窗选择）
//        ///  znt 2015-04-13
//        /// </summary>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult FrameList()
//        {
//            PageNavigate pn = new PageNavigate();
//            int limit = 10;
//            PageList<Tb_Gys_Cgs_Query> list = new PageList<Tb_Gys_Cgs_Query>(limit);
//            try
//            {
//                Hashtable param = GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("level", (long)0, HandleType.Remove); //客户级别Id
//                p.Add("keyword", String.Empty, HandleType.Remove, true); //搜素关键字
//                p.Add("pageIndex", 1, HandleType.DefaultValue); //当前页码
//                param = param.Trim(p);
//                if (param["keyword"] != null)
//                {
//                    ViewData["keyword"] = GetParameter("keyword");
//                }
//                param.Add("status", 0); // 只查询可用的

//                int pageIndex = Convert.ToInt32(param["pageIndex"]);
//                if (param.ContainsKey("level"))
//                {
//                    ViewData["level"] = param["level"];
//                    param.Add("id_cgs_level", param["level"]);
//                    param.Remove("level");
//                }
//                if (param.ContainsKey("status"))
//                {
//                    ViewData["status"] = param["status"];
//                    param.Add("flag_stop", param["status"]);
//                    param.Remove("status");
//                }

//                //ViewData["keyword"] = GetParameter("keyword");
//                ViewData["pageIndex"] = pageIndex;
//                ViewData["limit"] = limit;

//                param.Add("limit", limit);
//                param.Add("start", (pageIndex - 1) * limit);
//                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
//                param.Add("flag", 1);
//                pn = BusinessFactory.Customer.GetPage(param);
//                list = new PageList<Tb_Gys_Cgs_Query>(pn, pageIndex, limit);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//            return PartialView("_FrameListControl", list);
//        }

//        /// <summary>
//        ///  客户单体 
//        /// </summary>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult GetItem()
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = GetParameters();

//            try
//            {
//                if (string.IsNullOrEmpty(param["id"].ToString()))
//                {
//                    br.Success = false;
//                    br.Message.Add(string.Format("校验失败！请重新刷新页面。"));
//                    br.Data = "none";
//                    return Json(br);
//                }
//                var id = Convert.ToInt32(param["id"]);
//                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
//                br = BusinessFactory.Customer.Get(param);

//                if (br.Data == null)
//                {
//                    br.Data = new Tb_Cgs_Edit();
//                }
//                else
//                {
//                    Tb_Cgs_Edit model = (Tb_Cgs_Edit)br.Data;

//                    #region 获取 采购商收货地址
//                    br = BusinessFactory.Buyer.RecieverAddress(id);
//                    List<Tb_Cgs_Shdz_Query> list_shdz = br.Data as List<Tb_Cgs_Shdz_Query>;
//                    if (list_shdz != null && list_shdz.Count > 0)
//                    {

//                        Tb_Cgs_Shdz_Query Shdz = list_shdz.Where(m => m.flag_default == YesNoFlag.Yes).FirstOrDefault();
//                        if (Shdz != null)
//                        {
//                            model.shr = Shdz.shr;
//                            model.phone = Shdz.phone != String.Empty ? Shdz.phone : Shdz.tel;
//                            model.id_province = Shdz.id_province;
//                            model.id_city = Shdz.id_city;
//                            model.id_county = Shdz.id_county;
//                            model.address = Shdz.address;
//                            model.name_province = Shdz.province_name;
//                            model.name_city = Shdz.city_name;
//                            model.name_county = Shdz.county_name;
//                        }
//                        else
//                        {
//                            if (list_shdz.Count == 1)
//                            {
//                                Shdz = list_shdz.FirstOrDefault();

//                                model.shr = Shdz.shr;
//                                model.phone = Shdz.phone != String.Empty ? Shdz.phone : Shdz.tel;
//                                model.id_province = Shdz.id_province;
//                                model.id_city = Shdz.id_city;
//                                model.id_county = Shdz.id_county;
//                                model.address = Shdz.address;
//                                model.name_province = Shdz.province_name;
//                                model.name_city = Shdz.city_name;
//                                model.name_county = Shdz.county_name;
//                            }
//                        }
//                    }
//                    #endregion

//                    br.Data = model;
//                }

//                return Json(br);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult JsonData()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("keyword", String.Empty, HandleType.Remove);//查询关键字
//                p.Add("flag_stop", (byte)0, HandleType.Remove);
//                param = param.Trim(p);
//                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
//                br = BusinessFactory.Customer.GetAll(param);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        /// 生成json 
//        /// </summary>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public ActionResult Createjson()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                BusinessFactory.Config.InitArea();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        /// 高级查询获取数据源
//        /// 2016-7-1 wzp
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult GetJsonData()
//        {
//            PageList<Tb_Gys_Cgs_Query> list;
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = base.GetParameters();

//                string cName = string.Empty;//采购商
//                int limit = 0;

//                if (param.ContainsKey("cname"))
//                {
//                    cName = param["cname"].ToString();
//                }

//                ParamVessel p = new ParamVessel();
//                p.Add("orderby", 1, HandleType.DefaultValue);//排序
//                p.Add("status", 1, HandleType.Remove);//是否开通
//                p.Add("from", String.Empty, HandleType.Remove);//来源
//                p.Add("level", (long)0, HandleType.Remove);//客户级别Id
//                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
//                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
//                p.Add("id_province", 0, HandleType.Remove);//省份
//                p.Add("id_city", 0, HandleType.Remove);//城市
//                p.Add("id_county", 0, HandleType.Remove);//镇
//                p.Add("flag_show", true, HandleType.Remove);//是否展开高级搜索
//                p.Add("limit", 0, HandleType.Remove);//获取的数据行数
//                param = param.Trim(p);
//                if (param["flag_show"] != null)
//                {
//                    ViewData["flag_show"] = true;
//                }
//                switch (param["orderby"].ToString())
//                {
//                    case "2":
//                        param.Add("sort", "db.rq_create");
//                        param.Add("dir", "asc");
//                        break;
//                    case "3":
//                        param.Add("sort", "db.alias_cgs");
//                        param.Add("dir", "desc");
//                        break;
//                    case "4":
//                        param.Add("sort", "db.alias_cgs");
//                        param.Add("dir", "asc");
//                        break;
//                    case "5":
//                        param.Add("sort", "db.id_cgs_level");
//                        param.Add("dir", "desc");
//                        break;
//                    case "6":
//                        param.Add("sort", "db.id_cgs_level");
//                        param.Add("dir", "asc");
//                        break;
//                    case "7":
//                        param.Add("sort", "um.id_province desc,um.id_city desc,um.id_county");
//                        param.Add("dir", "desc");
//                        break;
//                    case "8":
//                        param.Add("sort", "um.id_province asc,um.id_city asc,um.id_county");
//                        param.Add("dir", "asc");
//                        break;
//                    default:
//                        param["orderby"] = 1;
//                        param.Add("sort", "db.rq_create");
//                        param.Add("dir", "desc");
//                        break;
//                }

//                int pageIndex = Convert.ToInt32(param["pageIndex"]);
//                if (param.ContainsKey("level"))
//                {
//                    ViewData["level"] = param["level"];
//                    param.Add("id_cgs_level", param["level"]);
//                    param.Remove("level");
//                }
//                if (param.ContainsKey("status"))
//                {
//                    ViewData["status"] = param["status"];
//                    param.Add("flag_actived", param["status"]);
//                    param.Remove("status");
//                }
//                if (param.ContainsKey("from"))
//                {
//                    ViewData["from"] = param["from"];
//                    param.Add("flag_from", param["from"]);
//                    param.Remove("from");
//                }

//                if (!string.IsNullOrEmpty(cName))
//                {
//                    param.Add("customer_name", cName);
//                }

//                ViewData["keyword"] = GetParameter("keyword");
//                ViewData["orderby"] = param["orderby"];
//                ViewData["pageIndex"] = pageIndex;

//                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
//                param.Add("not_stop", 1);

//                if (param.ContainsKey("limit"))
//                {
//                    limit = Convert.ToInt32(param["limit"]);
//                    param.Add("start", (pageIndex - 1) * limit);
//                }

//                PageNavigate pn = BusinessFactory.Customer.GetPage(param);
//                list = new PageList<Tb_Gys_Cgs_Query>(pn, 1, pn.TotalCount);
//                list = new PageList<Tb_Gys_Cgs_Query>(pn, 1, limit == 0 ? pn.TotalCount : limit);
//                br.Data = list;
//                br.Success = true;

//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }
//    }
//}