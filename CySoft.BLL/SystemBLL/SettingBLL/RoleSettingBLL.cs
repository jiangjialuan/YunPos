#region Imports
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Utility;
using CySoft.Frame.Attributes;
using CySoft.BLL.Tools.CodingRule;
using CySoft.Model.Enums;
using CySoft.Model.Ts;
#endregion

#region 角色管理
#endregion

namespace CySoft.BLL.SystemBLL.SettingBLL
{
    public class RoleSettingBLL : BaseBLL, IRoleSettingBLL
    {
        protected static readonly Type userRoleType = typeof(Tb_User_Role);
        protected static readonly Type roleType = typeof(Tb_Role);

        /// <summary>
        /// 修改角色名
        /// lxt
        /// 2015-04-13
        /// </summary>
        public BaseResult ChangeName(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long id = Convert.ToInt64(param["id"]);
            string id_user_master = param["id_user_master"].ToString();
            string name = param["name"].ToString();

            param.Clear();
            param.Add("id", id);
            var role = DAL.GetItem<Tb_Role>(roleType, param);
            if (role == null)
            {
                br.Success = false;
                br.Message.Add(String.Format("修改失败！该角色不存在。", name));
                br.Level = ErrorLevel.Warning;
                return br;
            }
            if (role.flag_update != (byte)YesNoFlag.Yes)
            {
                br.Success = false;
                br.Message.Add(String.Format("修改失败！该角色不允许修改。", name));
                br.Level = ErrorLevel.Warning;
                return br;
            }
            param.Clear();
            param.Add("not_id", id);
            param.Add("id_masteruser", id_user_master);
            param.Add("name", name);
            if (DAL.GetCount(roleType, param) > 0)
            {
                br.Success = false;
                br.Message.Add(String.Format("修改失败！角色名【{0}】已存在。", name));
                br.Level = ErrorLevel.Warning;
                return br;
            }
            param.Clear();
            param.Add("id", id);
            param.Add("id_masteruser", id_user_master);
            param.Add("new_name", name);
            DAL.UpdatePart(roleType, param);

            br.Message.Add(String.Format("修改角色名称。信息：流水号:{0},名称:{1}", id, name));
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 不分页查询
        /// lxt
        /// 2015-04-13
        /// </summary>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Tb_Role>(roleType, param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获得的单个角色权限值
        /// </summary>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long id_role = Convert.ToInt64(param["id"]);
            param.Clear();
            param.Add("id_roleList", new long[] { id_role });
            param.Add("flag_stop", YesNoFlag.No);
            param.Add("flag_type", "module");
            var list = DAL.QueryList<Tb_Function>(typeof(Tb_Function), param);

            br.Data = list.ToList(m => m.id);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获取权限模版
        /// lxt
        /// 2015-04-13
        /// </summary>
        public BaseResult GetAllModule()
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            param.Add("sort", "sort_id");
            param.Add("id_role_sysList", new long[] { 3, 4 });
            param.Add("flag_stop", YesNoFlag.No);
            param.Add("flag_type", "module");
            br.Data = DAL.QueryList<Tb_Function>(typeof(Tb_Function), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 根据平台角色 获取权限模板
        /// </summary>
        /// <returns></returns>
        public BaseResult GetAllModuleByPlatformRole(Hashtable param)
        {
            param.Add("sort", "sort_id");
            param.Add("dir", "asc");
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Tb_Role_Module>(typeof(Tb_Role_Module), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 保存角色权限
        /// lxt
        /// 2015-04-16
        /// </summary>
        [Transaction]
        public override BaseResult Save(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            string id = param["id"].ToString();
            string id_user = param["id_user"].ToString();
            string id_user_master = param["id_user_master"].ToString();
            string[] id_functions = (string[])param["id_functionList"];

            param.Clear();
            param.Add("id", id);
            var role = DAL.GetItem<Tb_Role>(roleType, param);
            if (role == null)
            {
                br.Success = false;
                br.Message.Add("修改失败！该角色不存在。");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            if (role.id_masteruser != id_user_master)
            {
                br.Success = false;
                br.Message.Add("修改失败！您无权修改该角色。");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            param.Clear();
            param.Add("id_role", id);
            DAL.Delete(typeof(Tb_Role_Function), param);

            if (id_functions.Length > 0)
            {
                List<Tb_Role_Function> roleFunctionList = new List<Tb_Role_Function>(id_functions.Length);
                foreach (var item in id_functions)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        roleFunctionList.Add(new Tb_Role_Function() { id_create = id_user, id_function = item, id_role = id });
                    }
                }
                DAL.AddRange(roleFunctionList);
            }

            param.Clear();
            param.Add("id_role", id);
            param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            var userRoleList = DAL.QueryList<Tb_User_Role>(typeof(Tb_User_Role), param);
            foreach (var item in userRoleList)
            {
                DataCache.Remove(item.id_user + "_purview");
            }


            br.Success = true;
            br.Message.Add(String.Format("保存角色权限。信息：流水号:{0}, 角色名称：{1}", role.id, role.name));
            return br;
        }

        /// <summary>
        /// 采购商 供应商 转平台商
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Transaction]
        public BaseResult ChangeToPlatform(UpgradeToPlatform model)
        {
            var br = new BaseResult(); return br;
            //var param = new Hashtable();

            //param.Add("id_user", model.id_user);
            //param.Add("id_roleList", new string[] { "3", "4" });
            //var userRoleList = DAL.QueryList<Tb_User_Role>(typeof(Tb_User_Role), param);

            //var isCgs = userRoleList.Where(d => d.id_role == 4).ToList().Count > 0;//当前用户是否为采购商
            //var isGys = userRoleList.Where(d => d.id_role == 3).ToList().Count > 0;//当前用户是否为供应商

            //if (isCgs && isGys)
            //{
            //    br.Level = ErrorLevel.Warning;
            //    br.Success = true;
            //    br.Message.Add("已成功启用该功能，请重新登录系统！");
            //    return br;
            //}
            //else if (isCgs || isGys)
            //{
            //    //检测当前手机号是否已被绑定 无绑定的话绑定当前用户
            //    param.Clear();
            //    param.Add("username", model.phone);
            //    var count = DAL.GetCount(typeof(Tb_Account), param);
            //    if (count <= 0)
            //    {
            //        DAL.Add<Tb_Account>(new Tb_Account
            //        {
            //            flag_lx = AccountFlag.standard,
            //            id_edit = model.id_user,
            //            rq_edit = DateTime.Now,
            //            username = model.phone,
            //            id_user = model.id_user
            //        });
            //    }
            //    // 更新 公司名称 联系人 所在地 手机号
            //    param.Clear();
            //    param.Add("new_name", model.linkman);
            //    param.Add("new_companyname", model.companyname);
            //    param.Add("new_id_province", model.location_id_province);
            //    param.Add("new_id_city", model.location_id_city);
            //    param.Add("new_id_county", model.location_id_county);
            //    param.Add("new_address", model.detaillocation);
            //    param.Add("new_phone", model.phone);
            //    param.Add("id", model.id_user);
            //    DAL.UpdatePart(typeof(Tb_User), param);

            //    //无论有无默认收货地址 先将其他的更新为非默认的 在插入一条默认的收货地址
            //    param.Clear();
            //    //param.Add("id_cgs", model.id_cgs);
            //    //param.Add("flag_default", 1);
            //    //param.Add("new_flag_default", 0);
            //    //DAL.UpdatePart(typeof(Tb_Cgs_Shdz), param);

            //    //插入默认收货地址
            //    //DAL.Add<Tb_Cgs_Shdz>(new Tb_Cgs_Shdz
            //    //{
            //    //    id = model.id_shdz,
            //    //    id_cgs = model.id_cgs,
            //    //    shr = model.linkman,
            //    //    phone = model.phone,
            //    //    id_province = model.address_id_province,
            //    //    id_city = model.address_id_city,
            //    //    id_county = model.address_id_county,
            //    //    address = model.detailaddress,
            //    //    id_create = model.id_create,
            //    //    rq_create = DateTime.Now,
            //    //    id_edit = model.id_create,
            //    //    rq_edit = DateTime.Now,
            //    //    flag_default = YesNoFlag.Yes
            //    //});

            //    if (isCgs)
            //    {
            //        DAL.Add<Tb_User_Role>(new Tb_User_Role
            //        {
            //            id_user = model.id_user,
            //            id_role = 3,
            //            id_create = model.id_create,
            //            rq_create = DateTime.Now
            //        });

            //        br.Success = true;
            //        br.Message.Add("已成功开启供货功能，请重新登录系统！");
            //        return br;
            //    }
            //    else
            //    {
            //        DAL.Add<Tb_User_Role>(new Tb_User_Role
            //        {
            //            id_user = model.id_user,
            //            id_role = 4,
            //            id_create = model.id_create,
            //            rq_create = DateTime.Now
            //        });

            //        br.Success = true;
            //        br.Message.Add("已成功开启采购功能，请重新登录系统！");
            //        return br;
            //    }

            //}
            //else
            //{
            //    br.Level = ErrorLevel.Warning;
            //    br.Success = false;
            //    br.Message.Add("非法提交！");
            //    return br;
            //}
        }


        /// <summary>
        /// 获取角色列表 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.GetCount(typeof(Tb_Role), param);
            if (pn.TotalCount > 0)
            {
                var lst = DAL.QueryPage<Tb_Role_Query>(typeof(Tb_Role), param) ?? new List<Tb_Role_Query>();
                pn.Data = lst;
            }
            else
            {
                pn.Data = new List<Tb_Role_Query>();
            }
            pn.Success = true;
            return pn;
        }

        public override BaseResult GetCount(Hashtable param = null)
        {
            BaseResult res=new BaseResult(){Success = true};
            res.Data= DAL.GetCount(typeof(Tb_Role), param);
            return res;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            var br = new BaseResult();

            if (!param.ContainsKey("id"))
            {
                br.Message.Add("删除的节点id不能为空！");
                br.Success = false;
                return br;
            }

            string id = string.Format("{0}", param["id"]);

            if (string.IsNullOrEmpty(id))
            {
                br.Message.Add("传入参数错误！");
                br.Success = false;
                return br;
            }
            param.Clear();
            param.Add("id_role", id);
            param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            if (DAL.GetCount(typeof(Tb_User_Role), param)>0)
            {
                br.Message.Add("此角色正被使用中,不能删除！");
                br.Success = false;
                return br;
            }
            //删除角色
            param.Clear();
            param.Add("id", id);
            DAL.Delete(typeof(Tb_Role), param);

            //删除用户角色
            param.Clear();
            param.Add("id_role", id);
            param.Add("new_flag_delete", (byte)Enums.FlagDelete.Deleted);

            //DAL.Delete(typeof(Tb_User_Role), param);
            DAL.UpdatePart(typeof(Tb_User_Role), param);



            //删除角色权限
            param.Clear();
            param.Add("id_role", id);
            DAL.Delete(typeof(Tb_Role_Function), param);

            br.Message.Add("删除成功！");
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            var br = new BaseResult();

            var model = (Tb_Role)entity;

            if (string.IsNullOrEmpty(model.name))
            {
                br.Message.Add("角色名称不能为空！");
                br.Success = false;
                return br;
            }
            if (model.name == "管理员" || model.name == "收银员")
            {
                br.Message.Add("角色名已存在！");
                br.Success = false;
                return br;
            }
            Hashtable param=new Hashtable();
            param.Add("id_masteruser", model.id_masteruser);
            param.Add("name", model.name);
            if (DAL.GetCount(typeof(Tb_Role), param)>0)
            {
                br.Success = false;
                br.Message.Add("角色名已存在!");
                return br;
            }
            model.id = Guid.NewGuid().ToString();  //new UtiletyBLL().GetNextKey(typeof(Tb_Role),DAL);
            DAL.Add<Tb_Role>(model);
            br.Success = true;
            br.Message.Add("添加成功！");
            return br;
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            var br = new BaseResult();
            var param = new Hashtable();

            var model = (Tb_Role)entity;

            if (string.IsNullOrEmpty(model.id))
            {
                br.Message.Add("异常的角色id！");
                br.Success = false;
                return br;
            }

            if (string.IsNullOrEmpty(model.name))
            {
                br.Message.Add("角色名称不能为空！");
                br.Success = false;
                return br;
            }

            param.Add("id", model.id);
            param.Add("new_name", model.name);
            param.Add("new_flag_type", model.flag_type);
            param.Add("new_id_edit", model.id_edit);

            DAL.UpdatePart(typeof(Tb_Role),param);
            br.Success = true;
            br.Message.Add("保存成功！");
            return br;
        }



        public BaseResult GetRoleModel(Hashtable param)
        {
            BaseResult res=new BaseResult(){Success = true};
            res.Data= DAL.GetItem<Tb_Role>(typeof(Tb_Role), param);
            return res;
        }




        public BaseResult UpdateRoleModel(dynamic entity)
        {
            var br = new BaseResult();
            var param = new Hashtable();

            var model = (Tb_Role)entity;

            if (string.IsNullOrEmpty(model.id))
            {
                br.Message.Add("异常的角色id！");
                br.Success = false;
                return br;
            }
            if (string.IsNullOrEmpty(model.id_masteruser))
            {
                br.Message.Add("参数有误！");
                br.Success = false;
                return br;
            }
            if (string.IsNullOrEmpty(model.name))
            {
                br.Message.Add("角色名称不能为空！");
                br.Success = false;
                return br;
            }
            param.Add("id",model.id);
            var oldModel= DAL.GetItem<Tb_Role>(typeof(Tb_Role), param);
            if (oldModel==null)
            {
                br.Message.Add("数据已不存在！");
                br.Success = false;
                return br;
            }
            if ((oldModel.name != "管理员" || oldModel.name != "收银员") && (model.name == "管理员" || model.name == "收银员"))
            {
                br.Success = false;
                br.Message.Add("角色名已存在!");
                return br;
            }
            param.Clear();
            param.Add("id_masteruser", model.id_masteruser);
            param.Add("name", model.name);
            param.Add("not_id",model.id);
            if (DAL.GetCount(typeof(Tb_Role), param) > 0)
            {
                br.Success = false;
                br.Message.Add("角色名已存在!");
                return br;
            }
            if (oldModel.flag_update==(byte)YesNoFlag.No)
            {
                br.Message.Add("此数据不能编辑！");
                br.Success = false;
                return br;
            }
            param.Clear();
            param.Add("id", model.id);
            param.Add("new_name", model.name);
            param.Add("new_id_edit", model.id_edit);
            param.Add("new_role_describe", model.role_describe);
            DAL.UpdatePart(typeof(Tb_Role), param);
            br.Success = true;
            br.Message.Add("编辑成功！");
            return br;
        }


        public BaseResult GetRoleFunction(Hashtable param)
        {
            BaseResult res=new BaseResult();
            res.Data= DAL.QueryList<Tb_Role_Function>(typeof(Tb_Role_Function), param);
            return res;
        }



        [Transaction]
        public BaseResult UpdateRoleFunction(Hashtable param)
        {
            BaseResult res=new BaseResult(){Success = true};
            if (param == null || param.Count<=0)
            {
                res.Success = false;
                res.Message.Add("参数有误");
                return res;
            }
            var moduleIds= param["moduleIds"].ToString();
            var roleId = param["roleId"] == null ? "" : param["roleId"].ToString();
            var id_user = param["id_user"].ToString() ?? "";//"_purview"
            var id_masteruser = string.Format("{0}", param["id_masteruser"]);
            if (string.IsNullOrEmpty(roleId))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (roleId == "2" || roleId=="3")
            {
                res.Success = false;
                res.Message.Add("此角色不允许编辑!");
                return res;
            }
            param.Clear();
            param.Add("id_role", roleId);
            DAL.Delete(typeof(Tb_Role_Function), param);
            var arr= moduleIds.Split(',');
            List<Tb_Role_Function> rfList = new List<Tb_Role_Function>();
            if (arr.Any())
            {
                param.Clear();
                param.Add("id_platform_role", "-1");
                var roleModuleList = DAL.QueryList<Tb_Role_Module>(typeof(Tb_Role_Module), param).ToList();
                var roleModules = new List<Tb_Role_Module>();
                param.Clear();
                var functionList = DAL.QueryList<Tb_Function>(typeof(Tb_Function), param).ToList();
                foreach (var s in arr)
                {
                    var moduleId = s;
                    if (string.IsNullOrEmpty(moduleId))
                    {
                        continue;
                    }
                    FindParentModule(moduleId, roleModuleList, roleModules);
                    if (roleModules.Any() && functionList.Any())
                    {
                        roleModules.ForEach(rm =>
                        {
                            var funcModel= functionList.FirstOrDefault(fl => fl.id == rm.id_function);
                            if (funcModel != null && funcModel.path!=null)
                            {
                                var funcIds= funcModel.path.Split('/');
                                foreach (var funcId in funcIds)
                                {
                                    if (!string.IsNullOrEmpty(funcId) && funcId!="0")
                                    {
                                        if (rfList.All(a => a.id_function != funcId))
                                        {
                                            rfList.Add(new Tb_Role_Function()
                                            {
                                                id_role = roleId,
                                                rq_create = DateTime.Now,
                                                id_function = funcId,
                                                id_create = id_user
                                            });
                                        }
                                        
                                    }
                                }
                            }
                        });
                    }
                }
            }
            DAL.AddRange(rfList);
            if (!string.IsNullOrEmpty(roleId)&&!string.IsNullOrEmpty(id_masteruser))
            {
                ClearPurview(roleId,id_masteruser);
            }
            return res;
        }
        /// <summary>
        /// 清除用户权限缓存
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="id_masteruser"></param>
        private void ClearPurview(string roleId, string id_masteruser)
        {
            if (!string.IsNullOrEmpty(roleId) && !string.IsNullOrEmpty(id_masteruser))
            {
                Hashtable param=new Hashtable();
                param.Add("id_role", roleId);
                param.Add("id_masteruser", id_masteruser);
                param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                var userRoleList = DAL.QueryList<Tb_User_Role>(typeof(Tb_User_Role), param).ToList();
                if (userRoleList.Any())
                {
                    string[] _arr = (from d in userRoleList select d.id_user + "_purview").ToArray();
                    DataCache.Remove(_arr);
                }
            }
        }

        private void FindParentModule(string moduleId, List<Tb_Role_Module> source, List<Tb_Role_Module> result)
        {
            if (string.IsNullOrEmpty(moduleId) || !source.Any()) return;
            var currnetModule = source.FirstOrDefault(s => s.id_module == moduleId);
            if (currnetModule == null || currnetModule.id_module_father==null||currnetModule.id_module_father == "0") return;
            result.Add(currnetModule);
            FindParentModule(currnetModule.id_module_father, source, result);
        }

    }
}
