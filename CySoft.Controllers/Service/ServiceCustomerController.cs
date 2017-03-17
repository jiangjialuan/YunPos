using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Core;
using CySoft.Utility;
using CySoft.Model.Tb;
using CySoft.Model.Flags;
using CySoft.Utility.Mvc.Html;

namespace CySoft.Controllers.ServiceCtl
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceCustomerController : ServiceBaseController
    {
        /// <summary>
        /// 分页查询客户列表
        /// lxt
        /// 2015-03-06
        /// </summary>
      
        public ActionResult GetPage(string obj)
        {
            PageNavigate pn = new PageNavigate();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("flag_actived", (byte)0, HandleType.Remove);//是否开通
                p.Add("id_cgs_level", (long)0, HandleType.Remove);//客户级别Id
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("limit", 20, HandleType.DefaultValue);//当前页码
                p.Add("sort", "db.rq_create", HandleType.DefaultValue);
                p.Add("dir", "desc", HandleType.DefaultValue);
                param = param.Trim(p);
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                int limit = Convert.ToInt32(param["limit"]);
                if (limit < 1)
                {
                    limit = 20;
                } 
                if (limit > 200)
                {
                    limit = 200;
                }
                param.Add("flag_stop", YesNoFlag.No);
                param.Add("start", (pageIndex - 1) * limit);
                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
                param.Add("not_stop", 1);
                pn = BusinessFactory.Customer.GetPage(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(pn);
        }

        /// <summary>
        /// 获得单个完整对象
        /// lxt
        /// 2015-03-06
        /// </summary>
        [HttpPost]
        public ActionResult GetItem(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                param = param.Trim(p);
                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
                br = BusinessFactory.Customer.Get(param);
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

        /// <summary>
        /// 未审核客户列表
        /// cxb
        /// 2015-4-8
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NoreList(string obj)
        {
            PageNavigate pn = new PageNavigate();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                int limit = int.Parse(param["limit"].ToString());
                PageList<Tb_Gys_Cgs_Check_Query> list = new PageList<Tb_Gys_Cgs_Check_Query>(limit);
                ParamVessel p = new ParamVessel();
                p.Add("orderby", 1, HandleType.DefaultValue);//排序
                p.Add("status", String.Empty, HandleType.Remove);//是否开通
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                param = param.Trim(p);
                switch (param["orderby"].ToString())
                {
                    case "2":
                        param.Add("sort", "rq_sq");
                        param.Add("dir", "asc");
                        break;
                    default:
                        param["orderby"] = 1;
                        param.Add("sort", "rq_sq");
                        param.Add("dir", "desc");
                        break;
                }
                int pageIndex = Convert.ToInt32(param["pageIndex"]);

                if (param.ContainsKey("status"))
                {
                    param.Add("flag_state", param["status"]);
                    param.Remove("status");
                }

                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));

                pn = BusinessFactory.BuyerAttention.GetNorevPage(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(pn);
        }

        /// <summary>
        /// 添加客户
        /// cxb
        /// 2015-4-16
        /// </summary>
        [HttpPost]
        public ActionResult Add(string obj)
        {
            Tb_Cgs_Edit model = JSON.Deserialize<Tb_Cgs_Edit>(obj);
            BaseResult br = new BaseResult();
            if (model.companyname.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("客户名称不能为空");
                br.Level = ErrorLevel.Warning;
                br.Data = "companyName";
                return Json(br);
            }
            //if (model.id_province < 1 || model.id_city < 1)
            //{
            //    br.Success = false;
            //    br.Message.Add("至少选择省份和城市");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = "area";
            //    return Json(br);
            //}
            if (model.id_cgs_level < 1)
            {
                br.Success = false;
                br.Message.Add("请选择客户级别");
                br.Level = ErrorLevel.Warning;
                br.Data = "customerLevel";
                return Json(br);
            }
            if (model.name.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("姓名不能为空");
                br.Level = ErrorLevel.Warning;
                br.Data = "name";
                return Json(br);
            }
            if (model.phone.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("手机号不能为空");
                br.Level = ErrorLevel.Warning;
                br.Data = "phone";
                return Json(br);
            }
            if (model.rq_treaty_start > new DateTime(1900, 1, 1) && model.rq_treaty_end > new DateTime(1900, 1, 1) && model.rq_treaty_start > model.rq_treaty_end)
            {
                br.Success = false;
                br.Message.Add("合约有效期有误");
                br.Level = ErrorLevel.Warning;
                return Json(br);
            }
            if (!model.username.IsEmpty() && model.password.IsEmpty())
            {
                model.flag_active = YesNoFlag.Yes;

                br = CyVerify.CheckUserName(model.username);
                if (!br.Success) return Json(br);
            }
           
            try
            {
                model.id = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Cgs));
                model.id_user_master = BusinessFactory.Utilety.GetNextKey(typeof(Tb_User));
                model.id_cgs_shdz = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Cgs_Shdz));
                model.id_gys = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Gys));
                model.id_create = GetLoginInfo<long>("id_user");
                model.id_edit = model.id_create;
                Hashtable param = new Hashtable();
                param.Add("model", model);
                param.Add("id_supplier", GetLoginInfo<long>("id_supplier"));
                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
                param.Add("name_gys", GetLoginInfo<string>("companyname"));
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("flag_from", model.flag_from);
                br = BusinessFactory.Customer.Add(param);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
                }
            }
            catch (CySoftException ex)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(ex.Message);
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(ex.Message);
            }
            return Json(br);
        }

        /// <summary>
        /// 修改
        /// cxb
        /// 2015-4-16
        /// </summary>
        [HttpPost]
        public ActionResult Update(string obj)
        {
            BaseResult br = new BaseResult();
            
            try
            {
                Tb_Cgs_Edit model = JSON.Deserialize<Tb_Cgs_Edit>(obj);
                if (model.id < 1 || model.id_user_master < 1)
                {
                    br.Success = false;
                    br.Message.Add("提交的数据不完整，请刷新后再试");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (model.companyname.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("客户名称不能为空");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "companyName";
                    return Json(br);
                }
                if (model.id_cgs_level < 1)
                {
                    br.Success = false;
                    br.Message.Add("请选择客户级别");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "customerLevel";
                    return Json(br);
                }

                if (model.rq_treaty_start > new DateTime(1900, 1, 1) && model.rq_treaty_end > new DateTime(1900, 1, 1) && model.rq_treaty_start > model.rq_treaty_end)
                {
                    br.Success = false;
                    br.Message.Add("合约有效期有误");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (model.flag_activeed == YesNoFlag.No && model.flag_active == YesNoFlag.Yes)
                {
                    if (model.id_province < 1 || model.id_city < 1)
                    {
                        br.Success = false;
                        br.Message.Add("至少选择省份和城市");
                        br.Level = ErrorLevel.Warning;
                        br.Data = "area";
                        return Json(br);
                    }
                    if (model.name.IsEmpty())
                    {
                        br.Success = false;
                        br.Message.Add("姓名不能为空");
                        br.Level = ErrorLevel.Warning;
                        br.Data = "name";
                        return Json(br);
                    }
                    if (model.phone.IsEmpty())
                    {
                        br.Success = false;
                        br.Message.Add("手机号不能为空");
                        br.Level = ErrorLevel.Warning;
                        br.Data = "phone";
                        return Json(br);
                    }
                    br = CyVerify.CheckUserName(model.username);
                    if (!br.Success) return Json(br);
                    if (model.password.IsEmpty())
                    {
                        br.Success = false;
                        br.Message.Add("密码不能为空");
                        br.Level = ErrorLevel.Warning;
                        br.Data = "username";
                        return Json(br);
                    }
                }
                model.id_edit = GetLoginInfo<long>("id_user");
                model.id_cgs_shdz = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Cgs_Shdz));
                Hashtable param = new Hashtable();
                param.Add("model", model);
                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
                param.Add("name_gys", GetLoginInfo<string>("companyname"));
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("flag_from", model.flag_from);
                br = BusinessFactory.Customer.Update(param);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
                }
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

        /// <summary>
        /// 取消关注
        /// cxb
        /// 2015-4-16
        /// </summary>
        [HttpPost]
        public ActionResult CancelAttention(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                p.Add("flag_from", string.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                param.Add("id_user_master_gys", GetLoginInfo<long>("id_user_master"));
                param.Add("id_user", GetLoginInfo<long>("id_user"));            
                br = BusinessFactory.Customer.Delete(param);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
                }
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

        /// <summary>
        /// 供应商通过关注
        /// cxb
        /// 2015-4-15
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AllowsAttention(string obj)
        {
            BaseResult br = new BaseResult();
           
            try
            {
                Tb_Cgs_Edit model = JSON.Deserialize<Tb_Cgs_Edit>(obj);
                if (model.companyname.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("客户名称不能为空");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "companyName";
                    return Json(br);
                }
                if (model.id_cgs_level < 1)
                {
                    br.Success = false;
                    br.Message.Add("请选择客户级别");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "customerLevel";
                    return Json(br);
                }
             
                model.id_create = GetLoginInfo<long>("id_user");
                model.id_edit = model.id_create;
                Hashtable param = new Hashtable();
                param.Add("model", model);
                param.Add("flag_from", model.flag_from);
                param.Add("id_supplier", GetLoginInfo<long>("id_supplier"));
                br = BusinessFactory.SupplierAttention.Add(param);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
                }
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

        /// <summary>
        /// 拒绝申请
        /// cxb
        /// 2015-4-15
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RefusalAttention(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                var model = JSON.Deserialize<Tb_Gys_Cgs_Check>(obj);
                Hashtable param = new Hashtable();
                param.Add("model", model);
                param.Add("flag_from", model.flag_form);
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                br = BusinessFactory.SupplierAttention.Update(param);
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
        /// <summary>
        /// 采购商扫一扫成客户
        /// tim
        /// 2015-7-22
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        // [HttpPost]
        public ActionResult Scan(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                var id_des = Base64Encrypt.DecodeBase64(obj);
                var id_master = DESEncrypt.DecryptDES(id_des);
                if (!string.IsNullOrWhiteSpace(obj))
                {
                    //获取供应商信息
                    var ht = new Hashtable();
                    ht.Add("id_user_master", id_master);
                    br = BusinessFactory.Supplier.Get(ht);
                    if (!br.Success || br.Data == null)
                    {
                        br.Success = false;
                        br.Message.Add("您所扫描的供应商不存在.");
                        br.Level = ErrorLevel.Warning;
                        return Json(br);
                    }

                    var gys = br.Data as Tb_Gys_Edit;

                    if (gys.id_user_master.Equals(GetLoginInfo<long>("id_user_master")))
                    {
                        br.Success = false;
                        br.Message.Add("自己不能扫描关注自己.");
                        br.Level = ErrorLevel.Warning;
                        return Json(br);
                    }
                    //采购关系对象
                    Tb_Gys_Cgs_Check model = new Tb_Gys_Cgs_Check()
                    {
                        flag_form = GetLoginInfo<string>("flag_from"),
                        flag_state = Gys_Cgs_Status.Apply,
                        id_cgs = GetLoginInfo<long>("id_buyer"),
                        id_gys = gys.id,
                        rq_sq = DateTime.Now,
                        id_user = GetLoginInfo<long>("id_user"),
                    };

                    //删除关注关系
                    ht.Clear();
                    ht.Add("id_gys", model.id_gys);
                    ht.Add("id_cgs", model.id_cgs);
                    ht.Add("id_user", model.id_user);
                    ht.Add("flag_from", model.flag_form);
                    BusinessFactory.SupplierAttention.Delete(ht);

                    //申请关注
                    br = BusinessFactory.BuyerAttention.Add(model);
                    if (br.Success)
                    {
                        WriteDBLog(LogFlag.Base, br.Message);

                        //获取采购商级别
                        ht.Clear();
                        ht.Add("id_gys", gys.id);
                        ht.Add("flag_sys", 1);
                        br = BusinessFactory.CustomerType.Get(ht);
                        if (br.Success && br.Data != null)
                        {
                            var cgs_level = br.Data as Tb_Cgs_Level;

                            //通过关注申请，直接成为客户
                            var gysmodel = new Tb_Cgs_Edit()
                            {
                                id = model.id_cgs,
                                companyname = GetLoginInfo<string>("companyname"),
                                id_cgs_level = cgs_level.id,
                                flag_pay = 0,
                                id_create = GetLoginInfo<long>("id_user"),
                                id_edit = GetLoginInfo<long>("id_user")
                            };
                            ht.Clear();
                            ht.Add("model", gysmodel);
                            ht.Add("flag_from", model.flag_form);
                            ht.Add("id_supplier", gys.id);
                            br = BusinessFactory.SupplierAttention.Add(ht);
                            if (br.Success)
                            {
                                WriteDBLog(LogFlag.Base, br.Message);
                            }
                        }
                        else
                        {
                            br.Message.Clear();
                            br.Message.Add("供应商没有设置默认客户级别，无法扫描自动关注，请登录网站手动设置。");
                            br.Success = false;
                            br.Level = ErrorLevel.Warning;
                        }
                    }
                }
                else
                {
                    br.Message.Clear();
                    br.Message.Add("扫描的二维码不正确。");
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                }
            }
            catch (CySoftException ex)
            {
                br.Message.Clear();
                br.Message.Add(ex.Message);
                br.Success = false;
                br.Level = ErrorLevel.Warning;
            }
            catch (Exception ex)
            {
                br.Message.Clear();
                br.Message.Add(ex.Message);
                br.Success = false;
                br.Level = ErrorLevel.Warning;
            }
            return Json(br);
        }
    }
}
