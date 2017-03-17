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
using CySoft.Model.Enums;
#endregion

#region 用户功能
#endregion

namespace CySoft.BLL.AccountBLL
{
    public class AccountFunctionBLL : BaseBLL, IAccountFunctionBLL
    {
        protected object lock1 = new object();

        /// <summary>
        /// 校验权限
        /// lxt
        /// 2015-02-02
        /// </summary>
        public BaseResult Check(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string id_user = string.Format("{0}", param["id_user"]);
            string actionName = string.Format("{0}", param["actionName"]);
            string controllerName = param["controllerName"].ToString();
            var actionNameList= actionName.Split(',').ToList();
            var controllerNameList = controllerName.Split(',').ToList();
            IList<ControllerModel> controllerList = GetControllerList(id_user);
            if (controllerList.Any(m => controllerNameList.Any(c=>c.ToLower()==m.name)&&m.actions.Any()))
                //(controllerList.Any(m => m.name == controllerName.ToLower() && m.actions.Any()))
            {
                if (controllerList.FirstOrDefault(m => controllerNameList.Any(c => c.ToLower() == m.name)
                    && m.actions.Any(b => actionNameList.Any(a=>a.ToLower()==b))) != null)
                    //(controllerList.FirstOrDefault(m => m.name == controllerName.ToLower() && m.actions.Any(b => b == actionName.ToLower())) != null)
                {
                    br.Success = true;
                    return br;
                }
            }
            else
            {
                if (controllerList.FirstOrDefault(m => m.name == controllerName.ToLower()) != null)
                {
                    br.Success = true;
                    return br;
                }
            }
            br.Message.Add("您无权操作！");
            br.Level = ErrorLevel.Warning;
            br.Success = false;
            return br;
        }

        public BaseResult GetPurview(string id_user)
        {
            BaseResult br = new BaseResult();
            br.Data = GetControllerList(id_user);
            br.Success = true;
            return br;
        }

        public IList<ControllerModel> GetControllerList(string id_user)
        {
            IList<ControllerModel> controllerList = DataCache.Get<IList<ControllerModel>>(id_user + "_purview");
            if (controllerList == null)
            {
                lock (lock1)
                {
                    controllerList = DataCache.Get<IList<ControllerModel>>(id_user + "_purview");
                    if (controllerList == null)
                    {
                        Hashtable param = new Hashtable();
                        param.Add("id", id_user);
                        Tb_User user = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
                        if (user == null) return new List<ControllerModel>();
                        Tb_User fatherUser = user;
                        if (user.id_father!="0")
                        {
                            param.Clear();
                            param.Add("id", user.id_masteruser);
                            fatherUser = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
                        }
                        param.Clear();
                        param.Add("id_user", id_user);
                        param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                        var roleList = DAL.QueryList<Tb_User_Role>(typeof(Tb_User_Role), param);

                        if (roleList.Count > 0)
                        {
                            #region 
                            string[] roles = roleList.Select(m => m.id_role).ToArray();
                            List<int> sysRoles = new List<int>();
                            param.Clear();
                            param.Add("id_user", id_user);
                            if (user.id_father == "0")
                            {
                                param.Add("id_roleList", roles);
                            }
                            else
                            {
                                param.Add("rolelist", roles);
                            }
                            param.Add("flag_stop", (int)YesNoFlag.No);
                            param.Add("flag_typeList", new string[] { "controller", "action" });
                            var list = DAL.QueryList<Tb_Function>(typeof(Tb_Function), param);
                            list = list.Where(l => l.version.Contains(fatherUser.version + "")).ToList();
                            param.Clear();
                            param.Add("id_user", id_user);
                            param.Add("rolelist", roles);
                            var clist = list.Where(l => l.flag_type == "controller").ToList();
                            var funcList = DAL.QueryList<Tb_Function>(typeof(Tb_Function), param).ToList();
                            if (funcList.Any())
                            {
                                funcList = funcList.Where(fl => fl.version.Contains(fatherUser.version + "")).ToList();
                                clist = clist.Where(cl => funcList.Any(fl => fl.id == cl.id_father)).ToList();
                            }
                            var query = from item in clist where item.flag_type == "controller" group item by item.controller_name.ToLower() into g select g.Key;
                            controllerList = query.ToList(m => new ControllerModel() { name = m });
                            foreach (var item in controllerList)
                            {
                                var query1 = from action in list
                                             join controller in list on action.id_father equals controller.id
                                             where action.flag_type == "action" && controller.flag_type == "controller" && controller.controller_name.ToLower() == item.name
                                             group action by action.action_name.ToLower() into g
                                             select g.Key;
                                item.actions = query1.ToList(m => m);
                            } 
                            #endregion
                        }
                        else
                        {
                            controllerList = new List<ControllerModel>();
                        }
                        if (controllerList.Any())
                        {
                            DataCache.Remove(id_user + "_purview");
                            DataCache.Add(id_user + "_purview", controllerList);
                        }
                    }
                }
            }

            return controllerList;
        }

        public BaseResult GetPurview(string id_user, string noUse)
        {
            BaseResult br = new BaseResult();
            br.Data = GetControllerList(id_user, noUse);
            br.Success = true;
            return br;
        }

        private void BuildTree(Tb_Function_Tree root, IList<Tb_Function_Tree> list)
        {
            if (root == null || list==null||!list.Any()) return;
            root.children = list.Where(d => d.id_father == root.id).ToList();
            if (!root.children.Any()) return;
            foreach (var item in root.children)
            {
                BuildTree(item, list);
            }
        }
        public IList<Tb_Function_Tree> GetControllerList(string id_user, string noUse)
        {
            IList<Tb_Function_Tree> controllerList = DataCache.Get<IList<Tb_Function_Tree>>(id_user + "_purview");
            if (controllerList == null)
            {
                lock (lock1)
                {
                    controllerList = DataCache.Get<IList<Tb_Function_Tree>>(id_user + "_purview");
                    if (controllerList == null)
                    {
                        Hashtable param = new Hashtable();
                        param.Add("id", id_user);
                        Tb_User user = DAL.GetItem<Tb_User>(typeof(Tb_User), param);

                        if(user==null) return new List<Tb_Function_Tree>();

                        param.Clear();
                        param.Add("id_user", id_user);
                        param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                        var roleList = DAL.QueryList<Tb_User_Role>(typeof(Tb_User_Role), param);

                        if (roleList.Count > 0)
                        {
                            string[] roles = roleList.Select(m => m.id_role).ToArray();
                            List<int> sysRoles = new List<int>();
                            param.Clear();
                            if (user.id_father != "0")//user.fatherId != 0
                            {
                                param.Add("id_user_master", user.id_father);
                            }
                            else
                            {
                                param.Add("id_user_master", user.id);
                            }
                            param.Clear();
                            param.Add("id_user", id_user);
                            param.Add("id_roleList", roles);
                            param.Add("flag_stop", (int)YesNoFlag.No);
                            //param.Add("flag_typeList", new string[] { "controller", "action" });
                            var list = DAL.QueryList<Tb_Function>(typeof(Tb_Function), param);
                            var sourceList = list.Select(d => new Tb_Function_Tree
                            {
                                id = d.id,
                                id_father = d.id_father,
                                flag_stop = d.flag_stop,
                                flag_type = d.flag_type,
                                id_create = d.id_create,
                                id_edit = d.id_edit,
                                name = d.name,
                                path = d.path,
                                rq_create = d.rq_create,
                                rq_edit = d.rq_edit,
                                sort_id = d.sort_id,
                                version = d.version,
                                isclose = true,
                                action_name = d.action_name,
                                controller_name = d.controller_name,
                                icon = d.icon,
                                tag_name = d.tag_name
                            }).ToList();

                            var targetList = sourceList.Where(d => d.id_father == "0").ToList();
                            var root = new Tb_Function_Tree
                            {
                                id = "0",
                                flag_type = "module",
                                path = "0",
                                name = "根目录",
                                children = targetList,
                                isclose = false
                            };

                            foreach (var item in targetList)
                            {
                                BuildTree(item, sourceList);
                            }

                            var rootList = new List<Tb_Function_Tree>();
                            rootList.Add(root);
                            return rootList;
                            //var query = from item in list where item.flag_type == "controller" group item by item.name.ToLower() into g select g.Key;
                            //controllerList = query.ToList(m => new ControllerModel() { name = m });
                            //foreach (var item in controllerList)
                            //{
                            //    var query1 = from action in list
                            //                 join controller in list on action.fatherId equals controller.id
                            //                 where action.flag_type == "action" && controller.flag_type == "controller" && controller.name.ToLower() == item.name
                            //                 group action by action.name.ToLower() into g
                            //                 select g.Key;
                            //    item.actions = query1.ToList(m => m);
                            //}
                        }
                        else
                        {
                            controllerList = new List<Tb_Function_Tree>();
                        }
                    }
                }
            }

            return controllerList;
        }


        #region 新添加方法

        public BaseResult GetUserMenu(string id_user)
        {

            BaseResult br = new BaseResult();
            Hashtable param=new Hashtable();
            param.Add("id_user",id_user);
            param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            var roleList = DAL.QueryList<Tb_User_Role>(typeof(Tb_User_Role), param);
            if (roleList.Any())
            {
                var roleArray= (from role in roleList select role.id_role).ToArray();
                param.Add("rolelist",roleArray);
                param.Add("flag_stop", 0);
                param.Add("sort", "sort_id");
                param.Add("dir", "asc");
                var funcList = DAL.QueryList<Tb_Function>(typeof(Tb_Function), param);
                funcList = funcList.OrderBy(a => a.sort_id).ToList();
                var delModel = funcList.FirstOrDefault(a => a.id == "b4fbb82c-783a-4fc7-b0b2-0f7129ed8838");
                if (delModel!=null)
                {
                    funcList.Remove(delModel);
                }
                var sourceList = funcList.Select(d => new Tb_Function_Tree
                {
                    id = d.id,
                    id_father = d.id_father,
                    flag_stop = d.flag_stop,
                    flag_type = d.flag_type,
                    id_create = d.id_create,
                    id_edit = d.id_edit,
                    name = d.name,
                    path = d.path,
                    rq_create = d.rq_create,
                    rq_edit = d.rq_edit,
                    sort_id = d.sort_id,
                    version = d.version,
                    isclose = true,
                    action_name = d.action_name,
                    controller_name = d.controller_name,
                    icon = d.icon,
                    tag_name = d.tag_name
                }).ToList();
                
                
                var targetList = sourceList.Where(d => d.id_father == "0").ToList();
                targetList = targetList.OrderBy(a => a.sort_id).ToList();
                var root = new Tb_Function_Tree
                {
                    id = "0",
                    flag_type = "module",
                    path = "0",
                    name = "根目录",
                    children = targetList,
                    isclose = false
                };
                foreach (var item in targetList)
                {
                    BuildTree(item, sourceList);
                }
                var rootList = new List<Tb_Function_Tree>();
                rootList.Add(root);
                br.Data= rootList;
            }
            br.Success = true;
            return br;
        }

        #endregion

    }
}
