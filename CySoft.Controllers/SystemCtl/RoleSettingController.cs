using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Frame.Common;
using CySoft.Model.Enums;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;

#region 角色权限管理
#endregion

namespace CySoft.Controllers.SystemCtl
{
    [LoginActionFilter]
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class RoleSettingController : BaseController
    {
        /// <summary>
        /// 角色权限管理
        /// lxt
        /// 2015-03-24
        /// </summary>
        public ActionResult Index()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("flag_master", 1);
                param.Add("id_user_master", GetLoginInfo<string>("id_user_master"));
                br = BusinessFactory.RoleSetting.GetAll(param);
                if (br.Success)
                {
                    //BaseResult br1 = BusinessFactory.RoleSetting.GetAllModule();
                    param.Clear();
                    param.Add("id_platform_role", UserRole);
                    BaseResult br1 = BusinessFactory.RoleSetting.GetAllModuleByPlatformRole(param);
                    ViewData["modules"] = br1.Data;
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
            return View(br.Data);
        }

        /// <summary>
        /// 修改名称UI
        /// lxt
        /// 2015-04-14
        /// </summary>
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult ChangeNameView(ChangeRoleName model)
        {
            return PartialView("_ChangeNameControl", model);
        }

        /// <summary>
        /// 修改名称
        /// lxt
        /// 2015-04-14
        /// </summary>
        [HttpPost]
        public ActionResult ChangeName()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                p.Add("name", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                param.Add("id_user_master", GetLoginInfo<string>("id_user_master"));
                br = BusinessFactory.RoleSetting.ChangeName(param);
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

        [HttpPost]
        [ActionPurview(true)]
        public ActionResult GetFunctionsJSON()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                param = param.Trim(p);
                br = BusinessFactory.RoleSetting.Get(param);
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
        /// 保存角色权限
        /// lxt
        /// 2015-04-16
        /// </summary>
        [HttpPost]
        public ActionResult Save()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                p.Add("id_functionList", String.Empty, HandleType.DefaultValue);
                param = param.Trim(p);
                string id_functionStr = param["id_functionList"].ToString().Trim();
                if (!id_functionStr.IsEmpty())
                {
                    param["id_functionList"] = param["id_functionList"].ToString().Trim().Split(',').ToInt32Array();
                }
                else
                {
                    param["id_functionList"] = new int[0];
                }
                param.Add("id_user", GetLoginInfo<string>("id_user"));
                param.Add("id_user_master", GetLoginInfo<string>("id_user_master"));
                br = BusinessFactory.RoleSetting.Save(param);
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
        /// 升级为平台商 视图
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult ChangeToPlatformView(int curRole)
        {
            var br = new BaseResult();
            var param = new Hashtable();

            var flag_master = (YesNoFlag)GetLoginInfo<long>("flag_master");

            //非master账号不得使用该功能
            if (flag_master == YesNoFlag.No)
            {
                br.Message.Add("您没有使用该功能的权限！");
                br.Success = false;
                br.Level = ErrorLevel.Error;
                return Json(br);
            }

            param.Add("id", GetLoginInfo<string>("id_user"));
            br = BusinessFactory.Account.Get(param);
            var info = br.Data as Tb_User_Query;
            return View("_UpgradeToPlatform",info);
        }

        /// <summary>
        /// 采购商 供应商 转换为平台商
        /// </summary>
        /// <param name="currentRole">标志：3供应商转平台商，标志：4 采购商转平台商</param>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(true)]
        public ActionResult ChangeToPlatform(UpgradeToPlatform data)
        {
            try
            {
                var curRole = Convert.ToInt64(data.currole);

                BaseResult br = new BaseResult();

                var flag_master = (YesNoFlag)GetLoginInfo<long>("flag_master");

                //非master账号不得使用该功能
                if (flag_master == YesNoFlag.No)
                {
                    br.Message.Add("您没有使用该功能的权限！");
                    br.Success = false;
                    br.Level = ErrorLevel.Error;
                    return Json(br);
                }

                var id_user = GetLoginInfo<string>("id_user");
                //var id_cgs = GetLoginInfo<string>("id_buyer");

                if (curRole == 3 || curRole == 4)
                {
                    if (string.IsNullOrEmpty(data.phone))
                    {
                        br.Message.Add("异常的手机验证！");
                        br.Success = false;
                        br.Level = ErrorLevel.Error;
                        return Json(br);
                    }

                    if (string.IsNullOrEmpty(data.companyname))
                    {
                        br.Message.Add("公司名称不能为空！");
                        br.Success = false;
                        br.Level = ErrorLevel.Error;
                        return Json(br);
                    }

                    if (string.IsNullOrEmpty(data.linkman))
                    {
                        br.Message.Add("联系人不能为空！");
                        br.Success = false;
                        br.Level = ErrorLevel.Error;
                        return Json(br);
                    }


                    if (string.IsNullOrEmpty(data.detaillocation))
                    {
                        br.Message.Add("所在地详细地址不能为空！");
                        br.Success = false;
                        br.Level = ErrorLevel.Error;
                        return Json(br);
                    }

                    //if (string.IsNullOrEmpty(data.defaultman))
                    //{
                    //    br.Message.Add("默认收货人不能为空！");
                    //    br.Success = false;
                    //    br.Level = ErrorLevel.Error;
                    //    return Json(br);
                    //}

                    if (string.IsNullOrEmpty(data.detailaddress))
                    {
                        br.Message.Add("默认收货详细地址不能为空！");
                        br.Success = false;
                        br.Level = ErrorLevel.Error;
                        return Json(br);
                    }

                    data.id_create = id_user;
                    data.id_user = id_user;
                    //data.id_cgs = id_cgs;

                    //data.id_shdz = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Cgs_Shdz));

                    br = BusinessFactory.RoleSetting.ChangeToPlatform(data);

                    return Json(br);
                }
                else
                {
                    br.Message.Add("非法提交！");
                    br.Success = false;
                    br.Level = ErrorLevel.Error;
                    return Json(br);
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
        }


        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult List()
        {
            PageNavigate pn = new PageNavigate();
            int limit = base.PageSizeFromCookie;
            PageList<Tb_Role_Query> list = new PageList<Tb_Role_Query>(limit);
            Hashtable param = GetParameters();
            try
            {
                ParamVessel p = new ParamVessel();
                p.Add("s_role", String.Empty, HandleType.Remove, true);
                p.Add("companyname", String.Empty, HandleType.Remove, true);
                p.Add("rq_start_reg", DateTime.Now, HandleType.Remove);//排序
                p.Add("rq_end_reg", DateTime.Now, HandleType.Remove);//排序
                p.Add("page", 0, HandleType.DefaultValue);//当前页码
                p.Add("sort", "id", HandleType.DefaultValue);
                p.Add("dir", "asc", HandleType.DefaultValue);
                p.Add("_search_", "0", HandleType.DefaultValue);
                param = param.Trim(p);
                param.Add("id_masteruser", "0");

                int pageIndex = Convert.ToInt32(param["page"]);
                ViewData["pageindex"] = pageIndex;
                ViewData["sort"] = param["sort"];
                ViewData["dir"] = param["dir"];
                ViewData["limit"] = limit;
                param.Add("start", pageIndex* limit);
                param.Add("limit", limit);
                pn = BusinessFactory.RoleSetting.GetPage(param);
                list = new PageList<Tb_Role_Query>(pn, pageIndex, limit);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ViewData["List"] = list;
            if (param["_search_"] != null && param["_search_"].ToString()=="1")
            {
                return PartialView("_List");
            }
            return View();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult Delete(string id)
        {
            var br = new BaseResult();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var param = new Hashtable();
                    param.Add("id", id);
                    br = BusinessFactory.RoleSetting.Delete(param);
                }
            }
            catch (CySoftException ex)
            {
                br.Message.Add(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return JsonString(br, 1);
        }

        [ActionPurview(true)]
        public ActionResult AddRole(Tb_Role info)
        {
            try
            {
                var result = new BaseResult();

                var id_user = GetLoginInfo<string>("id_user");
                info.id = Guid.NewGuid().ToString();//(int)BusinessFactory.Utilety.GetNextKey(typeof(Tb_Role));
                info.id_create = id_user;
                info.id_edit = id_user;
                info.id_masteruser = "0";
                info.rq_create = DateTime.Now;
                info.rq_edit = DateTime.Now;

                result = BusinessFactory.RoleSetting.Add(info);

                return Json(result);
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

        [ActionPurview(true)]
        public ActionResult EditRole(Tb_Role info)
        {
            try
            {
                var result = new BaseResult();

                var id_user = GetLoginInfo<string>("id_user");
                info.id_create = id_user;
                info.id_edit = id_user;
                info.id_masteruser = "0";
                info.rq_create = DateTime.Now;
                info.rq_edit = DateTime.Now;

                result = BusinessFactory.RoleSetting.Update(info);

                return Json(result);
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
        /// 系统用户角色管理
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult RoleList()
        {
            Hashtable param = base.GetParameters();
            int limit = 10;
            param.Add("_id_masteruser", id_user_master);
            ParamVessel p = new ParamVessel();
            p.Add("_search_", "0", HandleType.DefaultValue);
            p.Add("_id_masteruser", String.Empty, HandleType.ReturnMsg);
            p.Add("s_role", "", HandleType.Remove, true);
            p.Add("page", 0, HandleType.DefaultValue);
            p.Add("pageSize", limit, HandleType.DefaultValue);
            param = param.Trim(p);
            param.Add("flag_master",1);
            int.TryParse(param["pageSize"].ToString(), out limit);
            PageNavigate pn = new PageNavigate();
            int pageIndex = Convert.ToInt32(param["page"]);
            param.Add("limit", limit);
            param.Add("start", pageIndex * limit);
            pn = BusinessFactory.RoleSetting.GetPage(param);
            var plist = new PageList<Tb_Role_Query>(pn, pageIndex, limit);
            plist.PageIndex = pageIndex;
            plist.PageSize = limit;
            ViewData["List"] = plist;
            if (param["_search_"].ToString().Equals("1"))
            {
                return PartialView("_RoleList");
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 系统用户编辑角色
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Edit()
        {
            Hashtable param = base.GetParameters();
            param.Add("id_masteruser", id_user_master);
            ParamVessel p = new ParamVessel();
            p.Add("id_masteruser", id_user_master, HandleType.DefaultValue);
            p.Add("id", String.Empty, HandleType.ReturnMsg);
            var entity = BusinessFactory.RoleSetting.GetRoleModel(param).Data as Tb_Role;
            if (entity != null && entity.flag_update == 0)
            {
                entity=new Tb_Role();
            }
            ViewData["item_edit"] = entity;
            ViewData["option"] = "edit";
            return View("_RoleSetting_Edit");
        }
        /// <summary>
        /// 保存角色
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Edit(Tb_Role model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("name", string.Empty, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            pv.Add("role_describe","",HandleType.DefaultValue);
            try
            {
                param_model = param.Trim(pv);
                br = BusinessFactory.RoleSetting.UpdateRoleModel(new Tb_Role()
                {
                    id = param_model["id"].ToString(),
                    name = param_model["name"].ToString(),
                    id_edit = id_user,
                    role_describe = param_model["role_describe"].ToString(),
                    id_masteruser = id_user_master
                });
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            return JsonString(br, 1);
        }

        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Add()
        {
            ViewData["option"] = "add";
            return View("_RoleSetting_Edit");
        }

        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Add(Tb_Role model)
        {
            BaseResult res=new BaseResult();
            model.flag_type =10;
            model.id_masteruser = id_user_master;
            model.rq_create = model.rq_edit = DateTime.Now;
            model.id_create = model.id_edit =id_user;
            model.flag_update = 1;
            res=BusinessFactory.RoleSetting.Add(model);
            return JsonString(res, 1);
        }
        /// <summary>
        /// 平台角色编辑
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult PlatformRoleEdit()
        {
            Hashtable param = base.GetParameters();
            param.Add("id_masteruser", '0');
            ParamVessel p = new ParamVessel();
            p.Add("id_masteruser", '0', HandleType.DefaultValue);
            p.Add("id", String.Empty, HandleType.ReturnMsg);
            var entity = BusinessFactory.RoleSetting.GetRoleModel(param).Data as Tb_Role ?? new Tb_Role();
            ViewData["item_edit"] = entity;
            ViewData["option"] = "edit";
            List<SelectFunctionType> slist=new List<SelectFunctionType>()
            {
                new SelectFunctionType(){DisplayName = "平台角色",Value="1"},
                new SelectFunctionType(){DisplayName = "系统角色",Value="2"},
                new SelectFunctionType(){DisplayName = "模板角色",Value="9"},
            };
            SelectList selectList = new SelectList(slist, "Value", "DisplayName", entity.flag_type);
            ViewData["selectList"] = selectList;
            return View("_Platform_Role_Edit");
        }
        /// <summary>
        /// 平台角色添加 
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult PlatformRoleAdd()
        {
            Hashtable param = base.GetParameters();
            param.Add("id_masteruser", id_user_master);
            ParamVessel p = new ParamVessel();
            p.Add("id_masteruser", id_user_master, HandleType.DefaultValue);
            var entity = new Tb_Role();
            ViewData["item_edit"] = entity;
            ViewData["option"] = "add";
            List<SelectFunctionType> slist = new List<SelectFunctionType>()
            {
                new SelectFunctionType(){DisplayName = "平台角色",Value="1"},
                new SelectFunctionType(){DisplayName = "系统角色",Value="2"},
                new SelectFunctionType(){DisplayName = "模板角色",Value="9"},
            };
            SelectList selectList = new SelectList(slist, "Value", "DisplayName", entity.flag_type);
            ViewData["selectList"] = selectList;
            return View("_Platform_Role_Edit");
        }
        /// <summary>
        /// 平台角色添加
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult savePlatformRoleAdd()
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("name", string.Empty, HandleType.ReturnMsg);
            pv.Add("flag_type", 0, HandleType.ReturnMsg);
            pv.Add("role_describe", "", HandleType.DefaultValue);
            try
            {
                param_model = param.Trim(pv);
                br = BusinessFactory.RoleSetting.Add(new Tb_Role()
                {
                    name = param_model["name"].ToString(),
                    id_edit = id_user,
                    flag_type = Convert.ToByte(param_model["flag_type"].ToString()),
                    id_create = id_user,
                    rq_create = DateTime.Now,
                    rq_edit = DateTime.Now,
                    id_masteruser = "0",
                    role_describe = param_model["role_describe"].ToString()
                });
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            return JsonString(br, 1);
        }
        /// <summary>
        /// 平台角色编辑
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult savePlatformRoleEdit()
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("name", string.Empty, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            pv.Add("flag_type", 0, HandleType.ReturnMsg);
            pv.Add("role_describe","",HandleType.DefaultValue);
            try
            {
                param_model = param.Trim(pv);
                br = BusinessFactory.RoleSetting.UpdateRoleModel(new Tb_Role()
                {
                    id = param_model["id"].ToString(),
                    name = param_model["name"].ToString(),
                    id_edit = id_user,
                    flag_type = Convert.ToByte(param_model["id"].ToString()),
                    role_describe = param_model["role_describe"].ToString()
                });
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            return JsonString(br, 1);
        }
        

        /// <summary>
        /// 编辑角色模块
        /// LD
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult EditRoleModule()
        {
            Hashtable param = base.GetParameters();
            param.Add("id_platform_role", -1);
            var roleId = string.Format("{0}", param["id"]);
            var rolename = string.Format("{0}", param["rolename"]);// param["rolename"].ToString();
            ParamVessel pv = new ParamVessel();
            pv.Add("id_platform_role", -1,HandleType.ReturnMsg);
            param = param.Trim(pv);
            BaseResult br1 = BusinessFactory.RoleSetting.GetAllModuleByPlatformRole(param);
            Hashtable ht=new Hashtable();
            ht.Add("id_role", roleId);
            ViewData["roleFunList"] = BusinessFactory.RoleSetting.GetRoleFunction(ht).Data;
            ViewData["option"] = "edit";
            ViewData["item_edit"] = br1.Data;
            ViewData["roleId"] = roleId;
            ViewData["rolename"] = rolename;
            param.Clear();
            param.Add("id_role", roleId);
            ViewData["rolePosFuncList"] = BusinessFactory.Tb_Role_Pos_Function.GetAll(param).Data;
            param.Clear();
            param.Add("flag_stop",(int)Enums.FlagStop.Start);
            ViewData["posFuncList"]= BusinessFactory.Tb_Post_Function.GetAll(param).Data;
            param.Clear();
            param.Add("flag_stop",(int)Enums.FlagStop.Start);
            ViewData["funList"]= BusinessFactory.Function.GetAll(param).Data;
            ViewData["id_role"] = roleId;
            return View("RoleModuleEdit");
        }
        /// <summary>
        /// 保存角色模块
        /// LD
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult EditRoleModule(string id)
        {
            Hashtable param = base.GetParameters();
            ParamVessel pv = new ParamVessel();
            pv.Add("roleId", String.Empty, HandleType.ReturnMsg);
            pv.Add("moduleIds", "", HandleType.DefaultValue);
            BaseResult br = new BaseResult();
            try
            {
                param = param.Trim(pv);
                param.Add("id_user", id_user);
                param.Add("id_masteruser", id_user_master);
                br=BusinessFactory.RoleSetting.UpdateRoleFunction(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            if (br.Success)
            {
                return base.JsonString(new
                {
                    status = "success",
                    message = "执行成功,正在载入页面..."
                });
            }
            else
            {
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message),
                });
            }
        }
        /// <summary>
        /// 角色绑定前台功能
        /// LD
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult BindPosFunction(string roleid)
        {
            var res = HandleResult(() =>
            {
                var param= new Hashtable();
                param.Add("id_role",roleid);
                param.Add("flag_use",1);
                ViewData["rolePosFuncList"]=BusinessFactory.Tb_Role_Pos_Function.GetAll(param).Data;
                param.Clear();
                return BusinessFactory.Tb_Post_Function.GetAll(param);
            });
            ViewData["posFuncList"] = res.Data;
            ViewData["roleid"] = roleid;
            return View("_Bind_Pos_Function");
        }
        /// <summary>
        /// 保存角色绑定的前台功能
        /// LD
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult BindPosFunction()
        {
            var br= HandleResult(() =>
            {
                BaseResult res = new BaseResult();
                var param = base.GetParameters();
                var posmoduleIds = param["posmoduleIds"] + "";
                var roleid = param["roleid"] + "";
                if (string.IsNullOrEmpty(roleid))
                {
                    res.Success = false;
                    res.Message.Add("参数异常!");
                    return res;
                }
                var selectPosFuncList= JSON.Deserialize<List<SetPosFuncModel>>(posmoduleIds);
                List<Tb_Role_Pos_Function> list = new List<Tb_Role_Pos_Function>();
                if (selectPosFuncList.Any())
                {
                    selectPosFuncList.ForEach(s =>
                    {
                        list.Add(new Tb_Role_Pos_Function()
                        {
                            id_pos_function = s.id_pos_func,
                            id_create = id_user,
                            id_role = roleid,
                            rq_create = DateTime.Now,
                            minvalue = s.minvalue,
                            maxvalue = s.maxvalue,
                            flag_use = 1,
                            id_masteruser = id_user_master
                        });
                    });
                }
                if (list.Any())
                {
                    return BusinessFactory.Tb_Role_Pos_Function.Add(list);
                }
                else
                {
                    //param.Clear();
                    //param.Add("id_role", roleid);
                    //return BusinessFactory.Tb_Role_Pos_Function.Delete(param);
                    ChangePosFunctionFlagUseModel flagUseModel=new ChangePosFunctionFlagUseModel()
                    {
                        id_role = roleid,
                        flag_use = 0
                    };
                    return BusinessFactory.Tb_Role_Pos_Function.Update(flagUseModel);
                }
            });
            return JsonString(br, 1);
        }


    }
}
