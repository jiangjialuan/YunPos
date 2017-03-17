using System;
using System.Collections;
using CySoft.BLL.Base;
using System.Linq;
using CySoft.Frame.Core;
using CySoft.Frame.Common;
using CySoft.Model.Flags;
using CySoft.Model.Tb;
using CySoft.Utility;
using System.Collections.Generic;
using CySoft.Frame.Attributes;
using CySoft.IBLL;
using CySoft.IDAL;
using CySoft.BLL.Tools.CodingRule;
using CySoft.BLL.SystemBLL;

#region 客户
#endregion

namespace CySoft.BLL.SupplierBLL.CustomerBLL
{
    public class CustomerBLL : BaseBLL
    {
        protected static readonly Type cgsType = typeof(Tb_Cgs);

        public ITb_Gys_CgsDAL Tb_Gys_CgsDAL { get; set; }

        /// <summary>
        /// 新增
        /// lxt
        /// 2015-02-26
        /// </summary>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            //Hashtable param = (Hashtable)entity;
            //Tb_Cgs_Edit model = (Tb_Cgs_Edit)param["model"];
            //int id_user_master_gys = Convert.ToInt32(param["id_user_master_gys"]);
            //int id_supplier = Convert.ToInt32(param["id_supplier"]);
            //long id_user = TypeConvert.ToInt64(param["id_user"], 0);
            //string flag_from = param.ContainsKey("flag_from") ? param["flag_from"].ToString() : "browser";

            //string name_gys = param["name_gys"].ToString();

            //param.Clear();
            //param.Add("alias_cgs", model.companyname);
            //param.Add("id_user_master_gys", id_user_master_gys);
            //if (DAL.GetCount(typeof(Tb_Gys_Cgs), param) > 0)
            //{
            //    br.Success = false;
            //    br.Message.Add("客户名已存在.");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = "companyName";
            //    return br;
            //}
            //if (!model.bm_gys_Interface.IsEmpty())
            //{
            //    param.Clear();
            //    param.Add("bm_gys_Interface", model.bm_gys_Interface);
            //    param.Add("id_user_master_gys", id_user_master_gys);
            //    if (DAL.GetCount(typeof(Tb_Gys_Cgs), param) > 0)
            //    {
            //        br.Success = false;
            //        br.Message.Add("编码已使用");
            //        br.Level = ErrorLevel.Warning;
            //        br.Data = "bm_gys_Interface";
            //        return br;
            //    }
            //}
            //if (!model.username.IsEmpty() && !model.password.IsEmpty()) model.flag_active = YesNoFlag.Yes;
            //if (model.flag_active == YesNoFlag.Yes)
            //{
            //    br = CyVerify.CheckUserName(model.username);
            //    if (!br.Success) return br;

            //    param.Clear();
            //    param.Add("username", model.username);
            //    if (DAL.GetCount(typeof(Tb_Account), param) > 0)
            //    {
            //        br.Success = false;
            //        br.Message.Add("该用户名已被使用");
            //        br.Level = ErrorLevel.Warning;
            //        br.Data = "username";
            //        return br;
            //    }
            //}
            //Tb_Cgs cgs = (Tb_Cgs)model;
            //cgs.id_cgs_ptjb = 1;
            //DAL.Add(cgs);

            //Tb_Gys gys = new Tb_Gys();
            //gys.id = model.id_gys;
            //gys.flag_stop = YesNoFlag.No;
            //gys.id_gys_ptjb = 1;
            //gys.tax = 0m;
            //gys.vat = 0m;
            //gys.id_user_master = model.id_user_master;
            //gys.id_create = model.id_create;
            //gys.id_edit = model.id_edit;
            //br = CodingRule.SetBaseCoding(gys);
            //if (!br.Success)
            //{
            //    throw new CySoftException(br);
            //}
            //DAL.Add(gys);


            ////记录供采关系日志
            //var Loggcgx = new Tb_Gys_Cgs_Log();
            //Loggcgx.id_cgs = cgs.id.Value;
            //Loggcgx.id_gys = gys.id.Value;
            //Loggcgx.id_user = id_user;
            //Loggcgx.flag_state = Gys_Cgs_Status.Create;
            //Loggcgx.flag_form = flag_from;
            //Loggcgx.contents = string.Format("[{0}]新增客户[{1}]资料.", name_gys, model.companyname);
            //DAL.Add(Loggcgx);

            //Tb_Cgs_Shdz shdz = new Tb_Cgs_Shdz();
            //shdz.id = model.id_cgs_shdz;
            //shdz.id_cgs = model.id;
            //shdz.id_province = model.id_province;
            //shdz.id_city = model.id_city;
            //shdz.id_county = model.id_county;
            //shdz.address = model.address;
            //shdz.zipcode = model.zipcode;
            //shdz.flag_default = YesNoFlag.Yes;
            //shdz.shr = model.name;
            //shdz.tel = model.tel;
            //shdz.phone = model.phone;
            //shdz.id_create = model.id_create;
            //shdz.id_edit = model.id_edit;

            //DAL.Add(shdz);

            //Tb_User user = new Tb_User();
            //user.id = model.id_user_master;
            //user.id_province = model.id_province;
            //user.id_city = model.id_city;
            //user.id_county = model.id_county;
            //user.address = model.address;
            //user.zipcode = model.zipcode;
            //user.id_hy = model.id_hy;
            //user.id_create = model.id_create;
            //user.id_edit = model.id_edit;
            //user.companyname = model.companyname;
            //user.email = model.email;
            //user.fatherId = 0;
            //user.fax = model.fax;
            //user.phone = model.phone;
            //user.tel = model.tel;
            //user.qq = model.qq;
            //user.job = model.job;
            //user.name = model.name;
            //user.location = model.location;
            //user.location_des = model.location_des;
            //user.flag_master = YesNoFlag.Yes;
            //user.flag_stop = YesNoFlag.No;
            //user.flag_from = flag_from;

            //if (model.flag_active == YesNoFlag.Yes)
            //{
            //    user.username = model.username;
            //    user.password = MD5Encrypt.Encrypt(model.password);

            //    //添加登录帐号
            //    Tb_Account account = new Tb_Account()
            //    {
            //        flag_lx = AccountFlag.standard,
            //        username = user.username,
            //        id_edit = user.id_create,
            //        id_user = user.id
            //    };
            //    DAL.Add(account);

            //    //初始化角色相关
            //    param.Clear();
            //    param.Add("flag_type", 3);
            //    var roleList = DAL.QueryList<Tb_Role>(typeof(Tb_Role), param);
            //    var roleIdList = roleList.ToList(m => m.id.Value);
            //    roleIdList.Add(5);
            //    roleIdList.Add(6);
            //    param.Clear();
            //    param.Add("id_roleList", roleIdList);
            //    var functionList = DAL.QueryList<Tb_Role_Function>(typeof(Tb_Role_Function), param);
            //    UtiletyBLL utilety = new UtiletyBLL();
            //    utilety.DAL = DAL;
            //    List<Tb_Role_Function> newFunctionList = new List<Tb_Role_Function>();
            //    List<Tb_Role> newRoleList = new List<Tb_Role>();
            //    List<Tb_User_Role> userRoleList = new List<Tb_User_Role>();
            //    foreach (var role in roleList)
            //    {
            //        var newRole = role.Clone();
            //        if (role.id > 6)//5:系统管理员  6:业务员
            //        {
            //            newRole.id = utilety.GetNextKey(typeof(Tb_Role));
            //            newRoleList.Add(newRole);

            //        }
            //        //默认不写入业务员和自定义n角色
            //        if (role.id != 6 && role.id != 5 && role.id < 14)
            //        {
            //            userRoleList.Add(new Tb_User_Role()
            //            {
            //                id_role = newRole.id,
            //                id_user = user.id,
            //                id_create = user.id_create,
            //            });
            //        }
            //        newRole.id_master = user.id;
            //        newRole.id_create = user.id_create;
            //        newRole.id_edit = user.id_create;
            //        newRole.flag_type = 4;
            //        var query1 = functionList.Where(m => m.id_role == role.id).ToList();
            //        foreach (var function in query1)
            //        {
            //            function.id_role = newRole.id;
            //            function.id_create = user.id;
            //        }

            //        newFunctionList.AddRange(query1);
            //    }
            //    DAL.AddRange(newRoleList);
            //    DAL.AddRange(newFunctionList);
            //    //userRoleList.Add(new Tb_User_Role()
            //    //{
            //    //    id_role = 5,
            //    //    id_user = user.id,
            //    //    id_create = model.id_create
            //    //});
            //    DAL.AddRange(userRoleList);
            //    if (!model.isFindGys)
            //    {
            //        foreach (var item in userRoleList)
            //        {
            //            if (item.id_role != 5)
            //            {
            //                //param.Clear();
            //                //param.Add("id_role", item.id_role);
            //                //param.Add("id_function", 18);
            //                //DAL.Delete(typeof(Tb_Role_Function), param);
            //                param.Clear();
            //                param.Add("id_role", item.id_role);
            //                param.Add("id_function", 19);
            //                DAL.Delete(typeof(Tb_Role_Function), param);
            //            }
            //        }
            //    }
            //    Tb_Cgs_Level cgsLevel = new Tb_Cgs_Level();
            //    cgsLevel.id = utilety.GetNextKey(typeof(Tb_Cgs_Level));
            //    cgsLevel.name = "普通";
            //    cgsLevel.flag_sys = YesNoFlag.Yes;
            //    cgsLevel.agio = 100m;
            //    cgsLevel.remark = "系统默认，不可删除，不可修改";
            //    cgsLevel.id_gys = gys.id;
            //    cgsLevel.id_create = model.id_create;
            //    cgsLevel.id_edit = model.id_create;
            //    DAL.Add(cgsLevel);

            //    Tb_Spfl spfl = new Tb_Spfl();
            //    spfl.id = utilety.GetNextKey(typeof(Tb_Spfl));
            //    spfl.fatherId = 0;
            //    br = CodingRule.SetBaseCoding(spfl, typeof(Tb_Spfl));
            //    if (!br.Success)
            //    {
            //        throw new CySoftException(br);
            //    }
            //    spfl.id_gys = model.id_gys;
            //    spfl.name = "默认分类";
            //    spfl.path = string.Format("/0/{0}", spfl.id);
            //    spfl.id_create = model.id_create;
            //    spfl.id_edit = model.id_create;
            //    DAL.Add(spfl);


            //    Loggcgx.flag_state = Gys_Cgs_Status.AddUser;
            //    Loggcgx.flag_form = flag_from;
            //    Loggcgx.contents = string.Format("[{0}]为客户[{1}]添加用户帐号：{2}.", name_gys, model.companyname, user.username);
            //    DAL.Add(Loggcgx);
            //}

            //DAL.Add(user);

            ////默认采购商角色
            //DAL.Add(new Tb_User_Role
            //{
            //    id_user = user.id,
            //    id_role = 4,
            //    id_create = model.id_create,
            //    rq_create = DateTime.Now
            //});

            //Tb_Gys_Cgs gysCgs = new Tb_Gys_Cgs();
            //gysCgs.id_gys = id_supplier;
            //gysCgs.id_cgs = model.id;
            //gysCgs.alias_cgs = model.companyname;
            //gysCgs.alias_gys = name_gys;
            //gysCgs.flag_from = flag_from;
            //gysCgs.flag_pay = model.flag_pay;
            //gysCgs.flag_stop = YesNoFlag.No;
            //gysCgs.id_cgs_level = model.id_cgs_level;
            //gysCgs.id_create = model.id_create;
            //gysCgs.id_edit = model.id_edit;
            //gysCgs.id_user_cgs = user.id;
            //gysCgs.id_user_gys = id_user_master_gys;
            //gysCgs.id_user_master_cgs = user.id;
            //gysCgs.id_user_master_gys = id_user_master_gys;
            //gysCgs.rq_treaty_end = model.rq_treaty_end;
            //gysCgs.rq_treaty_start = model.rq_treaty_start;
            //gysCgs.bm_gys_Interface = model.bm_gys_Interface;

            //DAL.Add(gysCgs);
            //br.Data = model.id_user_master; //返回新增的客户的id_user_master
            //br.Message.Add(String.Format("新增客户。流水号：{0}，名称:{1}", model.id, model.companyname));
            //br.Success = true;
            return br;
        }

        /// <summary>
        /// 修改
        /// lxt
        /// 2015-02-26
        /// cxb 改 2015-6-23 
        /// </summary>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            //Hashtable param = (Hashtable)entity;
            //Hashtable param0 = new Hashtable();
            ////Tb_Cgs_Edit model = (Tb_Cgs_Edit)param["model"];
            //int id_user_master_gys = Convert.ToInt32(param["id_user_master_gys"]);
            //long id_user = TypeConvert.ToInt64(param["id_user"], 0);
            //string flag_from = param.ContainsKey("flag_from") ? param["flag_from"].ToString() : "browser";
            //string name_gys = param["name_gys"].ToString();

            //param.Clear();
            //param.Add("id", model.id);
            //var cgs = DAL.GetItem<Tb_Cgs>(typeof(Tb_Cgs), param);
            //if (cgs == null)
            //{
            //    br.Success = false;
            //    br.Message.Add("客户资料不存在.");
            //    br.Level = ErrorLevel.Warning;
            //    return br;
            //}

           // param.Clear();
            //param.Add("id", model.id_user_master);
            //var user_master = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
            //if (user_master == null)
            //{
            //    br.Success = false;
            //    br.Message.Add("客户资料不存在.");
            //    br.Level = ErrorLevel.Warning;
            //    return br;
            //}
            //if (user_master.username.IsEmpty()) model.flag_activeed = YesNoFlag.No;
            //if (user_master.username.IsEmpty() && !model.username.IsEmpty() && !model.password.IsEmpty()) model.flag_active = YesNoFlag.Yes;

            //param.Clear();
            //param.Add("not_id_user_master_cgs", model.id_user_master);
            //param.Add("alias_cgs", model.companyname);
            //param.Add("id_user_master_gys", id_user_master_gys);
            //if (DAL.GetCount(typeof(Tb_Gys_Cgs), param) > 0)
            //{
            //    br.Success = false;
            //    br.Message.Add("客户名已被占用");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = "companyName";
            //    return br;
            //}
            //if (!model.bm_gys_Interface.IsEmpty())
            //{
            //    param.Clear();
            //    param.Add("not_id_user_master_cgs", model.id_user_master);
            //    param.Add("bm_gys_Interface", model.bm_gys_Interface);
            //    param.Add("id_user_master_gys", id_user_master_gys);
            //    if (DAL.GetCount(typeof(Tb_Gys_Cgs), param) > 0)
            //    {
            //        br.Success = false;
            //        br.Message.Add("编码已被占用");
            //        br.Level = ErrorLevel.Warning;
            //        br.Data = "bm_gys_Interface";
            //        return br;
            //    }
            //}
            //if (model.flag_activeed == YesNoFlag.No && model.flag_active == YesNoFlag.Yes)
            //{
            //    br = CyVerify.CheckUserName(model.username);
            //    if (!br.Success) return br;

            //    param.Clear();
            //    param.Add("username", model.username);
            //    if (DAL.GetCount(typeof(Tb_Account), param) > 0)
            //    {
            //        br.Success = false;
            //        br.Message.Add("该用户名已被占用");
            //        br.Level = ErrorLevel.Warning;
            //        br.Data = "username";
            //        return br;
            //    }



                //初始化角色相关
            //    param.Clear();
            //    param.Add("flag_type", 3);
            //    var roleList = DAL.QueryList<Tb_Role>(typeof(Tb_Role), param);
            //    var roleIdList = roleList.ToList(m => m.id.Value);
            //    roleIdList.Add(5);
            //    roleIdList.Add(6);
            //    param.Clear();
            //    param.Add("id_roleList", roleIdList);
            //    var functionList = DAL.QueryList<Tb_Role_Function>(typeof(Tb_Role_Function), param);
            //    UtiletyBLL utilety = new UtiletyBLL();
            //    utilety.DAL = DAL;
            //    List<Tb_Role_Function> newFunctionList = new List<Tb_Role_Function>();
            //    List<Tb_Role> newRoleList = new List<Tb_Role>();
            //    List<Tb_User_Role> userRoleList = new List<Tb_User_Role>();
            //    foreach (var role in roleList)
            //    {
            //        var newRole = role.Clone();
            //        if (role.id > 6)//5:系统管理员  6:业务员
            //        {
            //            newRole.id = utilety.GetNextKey(typeof(Tb_Role));
            //            newRoleList.Add(newRole);

            //        }
            //        //默认不写入业务员和自定义n角色
            //        if (role.id != 6 && role.id < 14 && role.id != 5)
            //        {
            //            userRoleList.Add(new Tb_User_Role()
            //            {
            //                id_role = newRole.id,
            //                id_user = model.id_user_master,
            //                id_create = model.id_edit
            //            });
            //        }
            //        newRole.id_master = model.id_user_master;
            //        newRole.id_create = model.id_edit;
            //        newRole.id_edit = model.id_edit;
            //        newRole.flag_type = 4;
            //        var query1 = functionList.Where(m => m.id_role == role.id).ToList();
            //        foreach (var function in query1)
            //        {
            //            function.id_role = newRole.id;
            //            function.id_create = model.id_edit;
            //        }

            //        newFunctionList.AddRange(query1);
            //    }
            //    DAL.AddRange(newRoleList);
            //    DAL.AddRange(newFunctionList);
            //    //userRoleList.Add(new Tb_User_Role()
            //    //{
            //    //    id_role = 5,
            //    //    id_user = model.id_user_master,
            //    //    id_create = model.id_edit
            //    //});
            //    DAL.AddRange(userRoleList);
            //    if (!model.isFindGys)
            //    {
            //        foreach (var item in userRoleList)
            //        {
            //            if (item.id_role != 5)
            //            {
            //                //param.Clear();
            //                //param.Add("id_role", item.id_role);
            //                //param.Add("id_function", 18);
            //                //DAL.Delete(typeof(Tb_Role_Function), param);
            //                param.Clear();
            //                param.Add("id_role", item.id_role);
            //                param.Add("id_function", 19);
            //                DAL.Delete(typeof(Tb_Role_Function), param);
            //            }
            //        }
            //    }
            //    param.Clear();
            //    param.Add("id_user_master", model.id_user_master);
            //    var gys = DAL.GetItem<Tb_Gys>(typeof(Tb_Gys), param);

            //    Tb_Cgs_Level cgsLevel = new Tb_Cgs_Level();
            //    cgsLevel.id = utilety.GetNextKey(typeof(Tb_Cgs_Level));
            //    cgsLevel.name = "普通";
            //    cgsLevel.flag_sys = YesNoFlag.Yes;
            //    cgsLevel.agio = 100m;
            //    cgsLevel.remark = "系统默认，不可删除，不可修改";
            //    cgsLevel.id_gys = gys.id;
            //    cgsLevel.id_create = model.id_edit;
            //    cgsLevel.id_edit = model.id_edit;
            //    DAL.Add(cgsLevel);

            //    Tb_Spfl spfl = new Tb_Spfl();
            //    spfl.id = utilety.GetNextKey(typeof(Tb_Spfl));
            //    spfl.fatherId = 0;
            //    br = CodingRule.SetBaseCoding(spfl, typeof(Tb_Spfl));
            //    if (!br.Success)
            //    {
            //        throw new CySoftException(br);
            //    }
            //    spfl.id_gys = gys.id;
            //    spfl.name = "默认分类";
            //    spfl.path = string.Format("/0/{0}", spfl.id);
            //    spfl.id_create = model.id_edit;
            //    spfl.id_edit = model.id_edit;
            //    DAL.Add(spfl);


            //    //记录供采关系日志
            //    var Loggcgx = new Tb_Gys_Cgs_Log();
            //    Loggcgx.id_cgs = model.id.Value;
            //    Loggcgx.id_gys = gys.id.Value;
            //    Loggcgx.id_user = id_user;
            //    Loggcgx.flag_state = Gys_Cgs_Status.AddUser;
            //    Loggcgx.flag_form = flag_from;
            //    Loggcgx.contents = string.Format("[{0}]为客户[{1}]添加用户帐号：{2}.", name_gys, model.companyname, model.username);
            //    DAL.Add(Loggcgx);
            //}
            //param.Clear();
            //param["id_user"] = model.id_user_master;
            //var rolelist = DAL.QueryList(typeof(Tb_User_Role), param);

            //if (!model.isFindGys)//不允许发现供应商
            //{
            //    foreach (Tb_User_Role item in rolelist)
            //    {
            //        if (item.id_role != 5)
            //        {
            //            param.Clear();
            //            param["id_role"] = item.id_role;
            //            param["id_function"] = 18;
            //            if (DAL.GetCount(typeof(Tb_Role_Function), param) > 0)
            //            {
            //                DAL.Delete(typeof(Tb_Role_Function), param);
            //            }
            //            param["id_function"] = 19;
            //            if (DAL.GetCount(typeof(Tb_Role_Function), param) > 0)
            //            {
            //                DAL.Delete(typeof(Tb_Role_Function), param);
            //            }
            //        }
            //    }
            //}
            //else//允许发现
            //{
            //    Tb_Role_Function rolef = new Tb_Role_Function();
            //    foreach (Tb_User_Role it in rolelist)
            //    {
            //        if (it.id_role != 5)
            //        {
            //            param.Clear();
            //            param["id_role"] = it.id_role;
            //            param["id_function"] = 18;
            //            rolef.id_role = it.id_role;
            //            rolef.rq_create = DateTime.Now;
            //            rolef.id_create = id_user_master_gys;
            //            if (DAL.GetCount(typeof(Tb_Role_Function), param) <= 0)
            //            {
            //                rolef.id_function = 18;
            //                DAL.Add<Tb_Role_Function>(rolef);
            //            }
            //            param["id_function"] = 19;
            //            if (DAL.GetCount(typeof(Tb_Role_Function), param) <= 0)
            //            {
            //                rolef.id_function = 19;
            //                DAL.Add<Tb_Role_Function>(rolef);
            //            }

            //        }
            //    }
            //}
            //DateTime dbDateTime = DAL.GetDbDateTime();

            //if (model.flag_activeed == YesNoFlag.No)
            //{
            //    param.Clear();
            //    param.Add("id", model.id);
            //    param.Add("new_flag_pay", model.flag_pay);
            //    param.Add("new_name_bank", model.name_bank);
            //    param.Add("new_account_bank", model.account_bank);
            //    param.Add("new_khr", model.khr);
            //    param.Add("new_no_tax", model.no_tax);
            //    param.Add("new_title_invoice", model.title_invoice);
            //    param.Add("new_id_edit", model.id_edit);
            //    param.Add("new_rq_edit", dbDateTime);

            //    DAL.UpdatePart(cgsType, param);

            //    param.Clear();
            //    param.Add("id", model.id_user_master);
            //    param.Add("new_name", model.name);
            //    param.Add("new_companyname", model.companyname);
            //    param.Add("new_job", model.job);
            //    param.Add("new_qq", model.qq);
            //    param.Add("new_email", model.email);
            //    param.Add("new_phone", model.phone);
            //    param.Add("new_tel", model.tel);
            //    param.Add("new_fax", model.fax);
            //    param.Add("new_id_hy", model.id_hy);
            //    param.Add("new_id_province", model.id_province);
            //    param.Add("new_id_city", model.id_city);
            //    param.Add("new_id_county", model.id_county);
            //    param.Add("new_zipcode", model.zipcode);
            //    param.Add("new_address", model.address);
            //    param.Add("new_id_edit", model.id_edit);
            //    param.Add("new_rq_edit", dbDateTime);
            //    param.Add("new_flag_from", flag_from);
            //    //是否激活
            //    if (model.flag_active == YesNoFlag.Yes)
            //    {
            //        string password = MD5Encrypt.Encrypt(model.password);
            //        param.Add("new_username", model.username);
            //        param.Add("new_password", password);
            //        param.Add("new_location", model.location);
            //        param.Add("new_location_des", model.location_des);

            //        //添加登录帐号
            //        Tb_Account account = new Tb_Account()
            //        {
            //            flag_lx = AccountFlag.standard,
            //            username = model.username,
            //            id_edit = model.id_edit,
            //            id_user = model.id_user_master
            //        };
            //        DAL.Add(account);
            //    }
            //    DAL.UpdatePart(typeof(Tb_User), param);
            //}

            //param0["id_cgs"] = model.id;
            //int shdzcount = DAL.GetCount(typeof(Tb_Cgs_Shdz), param0);
            //if (shdzcount == 0)
            //{
            //    Tb_Cgs_Shdz shdz = new Tb_Cgs_Shdz();
            //    shdz.id = model.id_cgs_shdz;
            //    shdz.id_cgs = model.id;
            //    shdz.id_province = model.id_province;
            //    shdz.id_city = model.id_city;
            //    shdz.id_county = model.id_county;
            //    shdz.address = model.address;
            //    shdz.zipcode = model.zipcode;
            //    shdz.flag_default = YesNoFlag.Yes;
            //    shdz.shr = model.name;
            //    shdz.tel = model.tel;
            //    shdz.phone = model.phone;
            //    shdz.id_create = model.id_create;
            //    shdz.id_edit = model.id_edit;

            //    DAL.Add(shdz);
            //}

            //param.Clear();
            //param.Add("id_user_master_gys", id_user_master_gys);
            //param.Add("id_user_master_cgs", model.id_user_master);
            //param.Add("new_flag_pay", model.flag_pay);
            //param.Add("new_alias_cgs", model.companyname);
            //param.Add("new_id_cgs_level", model.id_cgs_level);
            //param.Add("new_rq_treaty_start", model.rq_treaty_start);
            //param.Add("new_rq_treaty_end", model.rq_treaty_end);
            //param.Add("new_bm_gys_Interface", model.bm_gys_Interface);
            //param.Add("new_id_edit", model.id_edit);
            //param.Add("new_rq_edit", dbDateTime);
            //DAL.UpdatePart(typeof(Tb_Gys_Cgs), param);

            //br.Message.Add(String.Format("修改客户。流水号：{0}，名称:{1}", model.id, model.companyname));
            //br.Success = true;
            return br;
        }

        /// <summary>
        /// 分页查询
        /// lxt
        /// 2015-02-26
        /// </summary>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.QueryCount(typeof(Tb_Gys_Cgs), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_Gys_Cgs_Query>(typeof(Tb_Gys_Cgs), param);
            }
            else
            {
                pn.Data = new List<Tb_Gys_Cgs_Query>();
            }
            pn.Success = true;
            return pn;
        }

        /// <summary>
        /// 获得单个完整对象
        /// lxt
        /// 2015-02-27
        /// </summary>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult(); return br;
            //br.Data = DAL.QueryItem<Tb_Cgs_Edit>(cgsType, param);

            //if (br.Data == null)
            //{
            //    br.Success = false;
            //    br.Message.Add("未找到该客户信息。");
            //    br.Level = ErrorLevel.Warning;
            //    return br;
            //}

            //var item = br.Data as Tb_Cgs_Edit;
            //param.Clear();
            //param["id_user"] = item.id_user_master;
            //var role = DAL.QueryList(typeof(Tb_User_Role), param);

            //foreach (Tb_User_Role it in role)
            //{
            //    if (it.id_role != 5)
            //    {
            //        param.Clear();
            //        param["id_role"] = it.id_role;
            //        param["id_function"] = 18;
            //        if (DAL.GetCount(typeof(Tb_Role_Function), param) > 0)
            //        {
            //            item.isFindGys = true;
            //            br.Data = item;
            //            return br;
            //        }
            //        else
            //        {
            //            param["id_function"] = 19;
            //            if (DAL.GetCount(typeof(Tb_Role_Function), param) > 0)
            //            {
            //                item.isFindGys = true;
            //                br.Data = item;
            //                return br;
            //            }
            //        }
            //    }
            //}
            //item.isFindGys = false;
            //br.Data = item;
            //br.Success = true;
            //return br;
        }

        /// <summary>
        /// 取消关注
        /// lxt
        /// 2015-02-28
        /// </summary>
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long id = TypeConvert.ToInt64(param["id"], 0);
            long id_user_master_gys = TypeConvert.ToInt64(param["id_user_master_gys"], 0);
            long id_user = TypeConvert.ToInt64(param["id_user"], 0);
            string flag_from = TypeConvert.ToString(param["flag_from"], "pc");
            param.Clear();
            param.Add("id", id);
            Tb_Cgs cgs = DAL.GetItem<Tb_Cgs>(cgsType, param);
            if (cgs == null)
            {
                br.Success = false;
                br.Message.Add("取消关注失败，该客户不存在或资料已缺失！");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            param.Clear();
            param.Add("id_user_master_gys", id_user_master_gys);
            param.Add("id_user_master_cgs", cgs.id_user_master);
            Tb_Gys_Cgs gysCgs = DAL.GetItem<Tb_Gys_Cgs>(typeof(Tb_Gys_Cgs), param);
            if (gysCgs == null)
            {
                br.Success = true;
                return br;
            }
            param.Clear();
            param.Add("id_user_master_gys", id_user_master_gys);
            param.Add("id_user_master_cgs", cgs.id_user_master);
            param.Add("new_flag_stop", YesNoFlag.Yes);
            DAL.UpdatePart(typeof(Tb_Gys_Cgs), param);


            param.Clear();
            param.Add("id_gys", gysCgs.id_gys);
            param.Add("id_cgs", gysCgs.id_cgs);
            //成功后删除申请记录         
            DAL.Delete(typeof(Tb_Gys_Cgs_Check), param);

            //记录供采关系日志
            var Loggcgx = new Tb_Gys_Cgs_Log();
            Loggcgx.id_cgs = gysCgs.id_gys.Value;
            Loggcgx.id_gys = gysCgs.id_cgs.Value;
            Loggcgx.id_user = id_user;
            Loggcgx.flag_state = Gys_Cgs_Status.End;
            Loggcgx.flag_form = flag_from;
            Loggcgx.contents = string.Format("[{0}]取消[{1}]的客户资格.", gysCgs.mc_gys, gysCgs.mc_cgs);
            DAL.Add(Loggcgx);

            br.Success = true;
            br.Message.Add(string.Format("[{0}]取消[{1}]的客户资格.", gysCgs.mc_gys, gysCgs.mc_cgs));
            return br;
        }

        /// <summary>
        /// 停用
        /// lxt
        /// 2015-02-28
        /// </summary>
        [Transaction]
        public override BaseResult Stop(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long id = Convert.ToInt64(param["id"]);
            long id_user_master_gys = Convert.ToInt64(param["id_user_master_gys"]);
            long id_user = TypeConvert.ToInt64(param["id_user"], 0);
            string flag_from = TypeConvert.ToString(param["flag_from"], "pc");
            param.Clear();
            param.Add("id", id);
            Tb_Cgs cgs = DAL.GetItem<Tb_Cgs>(cgsType, param);
            if (cgs == null)
            {
                br.Success = false;
                br.Message.Add("停用失败，该客户不存在或资料已缺失！");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            param.Clear();
            param.Add("id_user_master_gys", id_user_master_gys);
            param.Add("id_user_master_cgs", cgs.id_user_master);
            Tb_Gys_Cgs gysCgs = DAL.GetItem<Tb_Gys_Cgs>(typeof(Tb_Gys_Cgs), param);
            if (gysCgs == null)
            {
                br.Success = true;
                return br;
            }
            param.Clear();
            param.Add("id_user_master_gys", id_user_master_gys);
            param.Add("id_user_master_cgs", cgs.id_user_master);
            param.Add("new_flag_stop", YesNoFlag.Yes);
            DAL.UpdatePart(typeof(Tb_Gys_Cgs), param);

            //记录供采关系日志
            var Loggcgx = new Tb_Gys_Cgs_Log();
            Loggcgx.id_cgs = gysCgs.id_gys.Value;
            Loggcgx.id_gys = gysCgs.id_cgs.Value;
            Loggcgx.id_user = id_user;
            Loggcgx.flag_state = Gys_Cgs_Status.Stop;
            Loggcgx.flag_form = flag_from;
            Loggcgx.contents = string.Format("[{0}]已暂停[{1}]的客户资格.", gysCgs.mc_gys, gysCgs.mc_cgs);
            DAL.Add(Loggcgx);

            br.Success = true;
            br.Message.Add(string.Format("[{0}]已暂停[{1}]的客户资格.", gysCgs.mc_gys, gysCgs.mc_cgs));
            return br;
        }

        /// <summary>
        /// 启用
        /// lxt
        /// 2015-02-28
        /// </summary>
        [Transaction]
        public override BaseResult Active(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long id = Convert.ToInt64(param["id"]);
            long id_user_master_gys = Convert.ToInt64(param["id_user_master_gys"]);
            long id_user = TypeConvert.ToInt64(param["id_user"], 0);
            string flag_from = TypeConvert.ToString(param["flag_from"], "pc");
            param.Clear();
            param.Add("id", id);
            Tb_Cgs cgs = DAL.GetItem<Tb_Cgs>(cgsType, param);
            if (cgs == null)
            {
                br.Success = false;
                br.Message.Add("停用失败，该客户不存在或资料已缺失！");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            param.Clear();
            param.Add("id_user_master_gys", id_user_master_gys);
            param.Add("id_user_master_cgs", cgs.id_user_master);
            Tb_Gys_Cgs gysCgs = DAL.GetItem<Tb_Gys_Cgs>(typeof(Tb_Gys_Cgs), param);
            if (gysCgs == null)
            {
                br.Success = true;
                return br;
            }
            param.Clear();
            param.Add("id_user_master_gys", id_user_master_gys);
            param.Add("id_user_master_cgs", cgs.id_user_master);
            param.Add("new_flag_stop", YesNoFlag.No);
            DAL.UpdatePart(typeof(Tb_Gys_Cgs), param);

            //记录供采关系日志
            var Loggcgx = new Tb_Gys_Cgs_Log();
            Loggcgx.id_cgs = gysCgs.id_gys.Value;
            Loggcgx.id_gys = gysCgs.id_cgs.Value;
            Loggcgx.id_user = id_user;
            Loggcgx.flag_state = Gys_Cgs_Status.Active;
            Loggcgx.flag_form = flag_from;
            Loggcgx.contents = string.Format("[{0}]已恢复[{1}]的客户资格.", gysCgs.mc_gys, gysCgs.mc_cgs);
            DAL.Add(Loggcgx);

            br.Success = true;
            br.Message.Add(String.Format("[{0}]已恢复[{1}]的客户资格.", gysCgs.mc_gys, gysCgs.mc_cgs));
            return br;
        }

        /// <summary>
        /// 不分页查询
        /// lxt
        /// 2015-03-07
        /// </summary>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = Tb_Gys_CgsDAL.QueryListOfSupplier(typeof(Tb_Gys_Cgs), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetCount(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList(typeof(Tb_User_Role), param);
            br.Success = true;
            return br;
        }
    }
}
