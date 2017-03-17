#region Imports
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Utility.Mail;
using CySoft.IBLL;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Utility;
using CySoft.Frame.Attributes;
using CySoft.BLL.Tools.CodingRule;
using CySoft.Model.Ts;
using CySoft.BLL.SystemBLL;
using Spring.Context.Support;
using System.Net.Mail;
using CySoft.IDAL;
using CySoft.Model.Enums;
using CySoft.Model;
using Spring.Validation;

#endregion

#region 用户管理
#endregion

namespace CySoft.BLL.AccountBLL
{
    public class AccountBLL : BaseBLL, IAccountBLL
    {
        protected static readonly Type userType = typeof(Tb_User);
        protected static readonly Type roleType = typeof(Tb_Role);
        protected UtiletyBLL utilety = new UtiletyBLL();


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model">登录信息</param>
        /// <returns>登录结果</returns>
        [Transaction]
        public BaseResult LogOn(UserLogin model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            if (model == null || string.IsNullOrEmpty(model.username))
            {
                br.Message.Add("登录失败！");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }
            if (model.loginType == "quick" && !model.username.Contains(":") && !model.username.Contains("："))
            {
                param.Add("username", model.username);
            }
            else
            {
                model.username = model.username.Replace("：", ":");
                var arr = model.username.Split(':');
                if (model.username.Contains(":"))
                {
                    param.Add("parent_name", arr[0]);
                    param.Add("username", arr[1]);
                }
                else
                {
                    param.Add("id_masteruser", "0");
                    param.Add("username", model.username);
                }
                param.Add("flag_lx", model.flag_lx);
            }
            Tb_Account account = DAL.GetItem<Tb_Account>(typeof(Tb_Account), param);
            if (account == null)
            {
                br.Message.Add("登录失败，未注册用户。");
                br.Level = ErrorLevel.NotLogin;
                br.Success = false;
                return br;
            }
            param.Clear();
            param.Add("id", account.id_user);
            Tb_User user = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
            if (user == null)
            {
                br.Message.Add("登录失败！用户资料不完整，请联系系统管理员。");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }
            string md5Password = MD5Encrypt.Md5(model.password);
            if (user.password.ToUpper() != md5Password.ToUpper())
            {
                br.Message.Add("登录失败！用户名或密码错误。");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }
            if (user.flag_state == (byte)CySoft.Model.Enums.Enums.TbUserFlagState.No)
            {
                br.Message.Add("登录失败！用户已被停用。");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }

            if (user.flag_delete != (byte)CySoft.Model.Enums.Enums.FlagDelete.NoDelete)
            {
                br.Message.Add("登录失败！用户已被删除。");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }

            Hashtable loginInfo = new Hashtable();
            loginInfo.Add("name", user.name);
            loginInfo.Add("username", user.username);
            loginInfo.Add("companyname", user.companyname);
            loginInfo.Add("id_user", user.id);
            loginInfo.Add("flag_master", user.flag_master);
            loginInfo.Add("id_cyuser", user.id_cyuser);
            loginInfo.Add("id_shop", user.id_shop);
            loginInfo.Add("flag_industry", user.flag_industry);
            loginInfo.Add("companyno", user.companyno);
            if (user.flag_master != (byte)YesNoFlag.Yes)
            {
                loginInfo.Add("id_user_master", user.id_father);
                param.Clear();
                param.Add("id", user.id_father);
                Tb_User fatherUser = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
                if (fatherUser != null)
                {
                    loginInfo.Add("version", user.version);
                    loginInfo.Add("id_shop_master", fatherUser.id_shop);
                    loginInfo.Add("phone_master", fatherUser.phone);
                    loginInfo.Add("rq_create_master_shop", fatherUser.rq_create);
                }
            }
            else
            {
                loginInfo.Add("id_user_master", user.id);
                loginInfo.Add("version", user.version);
                loginInfo.Add("id_shop_master", user.id_shop);
                loginInfo.Add("phone_master", user.phone);
                loginInfo.Add("rq_create_master_shop", user.rq_create);
            }

            //获取当前用户 id_user_master 对应的角色
            param.Clear();
            param.Add("id_user", user.id);
            param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);

            IList<Tb_User_Role> user_role = DAL.QueryList<Tb_User_Role>(typeof(Tb_User_Role), param);

            if (user_role != null && user_role.Count > 0)
            {
                var roleList = (from r in user_role select r.id_role).ToList();
                loginInfo.Add("role_str", string.Join(",", roleList));
                if (user_role.Where(d => d.id_role == ((int)Enums.TbRoleFixedRoleId.PlatformManager).ToString()).ToList().Count > 0)
                {
                    //平台管理员
                    loginInfo.Add("flag_user_role", RoleFlag.PlatformManager);
                }

                if (user_role.Where(d => d.id_role == ((int)Enums.TbRoleFixedRoleId.SysManager).ToString()).ToList().Count > 0)
                {
                    //系统管理员
                    loginInfo.Add("is_sysmanager", "1");
                }
            }
            else
            {
                loginInfo.Add("role_str", "");
                loginInfo.Add("flag_user_role", RoleFlag.Other);
            }


            //此处 检验所在门店的状态是否 删除/停用
            //     检验购买服务缓存
            //     检验购买服务是否过期 是否超过数量
            //     lz add 2017-1-05
            #region 检验所在门店的状态是否 删除/停用
            param.Clear();
            param.Add("id", user.id_shop);
            param.Add("id_masteruser", user.id_masteruser);
            var dbShop = DAL.QueryList<Tb_Shop>(typeof(Tb_Shop), param).FirstOrDefault();
            if (dbShop == null)
            {
                br.Message.Add("登录失败！获取门店信息异常。");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }
            if (dbShop.flag_delete == (byte)Enums.FlagDelete.Deleted)
            {
                br.Message.Add("登录失败！用户门店已被删除。");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }
            if (dbShop.flag_state == (byte)Enums.TbShopFlagState.Closed)
            {
                br.Message.Add("登录失败！用户门店已停用。");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }
            loginInfo.Add("id_shop_info", dbShop);
            if (!loginInfo.ContainsKey("rq_create_master_shop"))
                loginInfo.Add("rq_create_master_shop", dbShop.rq_create.ToString());

            loginInfo.Add("bm_shop", dbShop.bm);
            loginInfo.Add("flag_type_shop", dbShop.flag_type);

            #endregion


            param.Clear();
            br.Message.Add("登录成功！");
            br.Data = loginInfo;
            br.Success = true;
            return br;
        }



        /// <summary>
        /// 移动端登录
        /// </summary>
        /// <param name="model">登录信息</param>
        /// <returns>登录结果</returns>
        [Transaction]
        public BaseResult MobileLogin(UserLogin model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            param.Add("username", model.username);
            param.Add("flag_lx", model.flag_lx);
            Tb_Account account = DAL.GetItem<Tb_Account>(typeof(Tb_Account), param);
            if (account == null)
            {
                br.Message.Add("登录失败！用户名或密码错误。");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }
            param.Clear();
            param.Add("id", account.id_user);
            Tb_User user = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
            if (user == null)
            {
                br.Message.Add("登录失败！用户资料不完整，请联系系统管理员。");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }

            string md5Password = MD5Encrypt.Encrypt(model.password);

            if (user.password.ToUpper() != md5Password.ToUpper())
            {
                br.Message.Add("登录失败！用户名或密码错误。");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }
            if (user.flag_state == (byte)CySoft.Model.Enums.Enums.TbUserFlagState.No)
            {
                br.Message.Add("登录失败！用户已被停用。");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }

            //
            var user_openid_query = new Tb_User_Openid_Query
            {
                id = user.id

            };

            //还没绑定微信openid且openid不为空
            if (!model.isWxOpenidBind && !string.IsNullOrEmpty(model.wxOpenid))
            {
                //公众号供应商id
                //该账号绑定微信openid
                DAL.Add<Tb_User_Openid>(new Tb_User_Openid
                {
                    id_user = user.id,
                    id_user_master = user.id_father,
                    openid = model.wxOpenid,
                    rq_create = DateTime.Now,
                    id_gys = model.id_gys_gzh
                });


            }



            br.Message.Add("登录成功！");
            br.Data = user;
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获得单个用户信息
        /// </summary>
        /// <param name="param">查询条件</param>
        /// <returns>登录结果</returns>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            if (param.ContainsKey("username"))
            {
                param.Add("flag_lx", AccountFlag.standard);
                br.Data = DAL.GetItem<Tb_Account>(typeof(Tb_Account), param);
                br.Success = true;
                return br;
            }
            br.Data = DAL.GetCount(userType, param);
            br.Success = true;
            return br;
        }


        [Transaction]
        public override BaseResult Save(dynamic entity)
        {
            BaseResult br = new BaseResult();
            //var model = (Hashtable)entity;

            //if (!model.ContainsKey("id_user")
            //    || !model.ContainsKey("id_user_master")
            //    || !model.ContainsKey("flag_from")
            //    || !model.ContainsKey("id")
            //    )
            //{
            //    br.Success = false;
            //    br.Message.Add("参数错误.");
            //    br.Level = ErrorLevel.Warning;
            //    return br;
            //}
            //var flag_from = model["flag_from"].ToString();
            //var id_user = model["id_user"];
            //var ht = new Hashtable();
            //ht.Add("id", model["id"]);
            //var user = DAL.QueryItem<Tb_User_Query>(userType, ht);
            //if (user == null)
            //{
            //    br.Success = false;
            //    br.Message.Add("用户已不存在，请重新登录。");
            //    br.Level = ErrorLevel.Warning;
            //    return br;
            //}

            //var ht_master = new Hashtable();
            //ht.Clear();
            //if (model.ContainsKey("name"))
            //    ht.Add("new_name", model["name"]);
            //if (model.ContainsKey("flag_sex"))
            //    ht.Add("new_flag_sex", model["flag_sex"]);
            //if (model.ContainsKey("job"))
            //    ht.Add("new_job", model["job"]);
            //if (model.ContainsKey("qq"))
            //    ht.Add("new_qq", model["qq"]);
            //if (model.ContainsKey("pic"))
            //    ht.Add("new_pic", model["pic"]);
            //if (model.ContainsKey("pic_erwei"))
            //    ht.Add("new_pic_erwei", model["pic_erwei"]);
            //if (model.ContainsKey("email"))
            //{
            //    //if (!user.valid_email.Equals(0))
            //    //{
            //    //    br.Success = true;
            //    //    br.Message.Add("邮箱已被绑定。");
            //    //    br.Level = ErrorLevel.Warning;
            //    //    return br;
            //    //}
            //    ht.Add("new_email", model["email"]);
            //}
            //if (model.ContainsKey("phone"))
            //{
            //    if (!user.valid_phone.Equals(0))
            //    {
            //        br.Success = true;
            //        br.Message.Add("手机号已被绑定，不能修改。");
            //        br.Level = ErrorLevel.Warning;
            //        return br;
            //    }

            //    ht.Add("new_phone", model["phone"]);
            //}
            //if (model.ContainsKey("companyname"))
            //    ht_master.Add("new_companyname", model["companyname"]);
            //if (model.ContainsKey("tel"))
            //    ht_master.Add("new_tel", model["tel"]);
            //if (model.ContainsKey("fax"))
            //    ht_master.Add("new_fax", model["fax"]);
            //if (model.ContainsKey("zipcode"))
            //    ht_master.Add("new_zipcode", model["zipcode"]);
            //if (model.ContainsKey("address"))
            //    ht_master.Add("new_address", model["address"]);
            //if (model.ContainsKey("location_des"))
            //    ht_master.Add("new_location_des", model["location_des"]);
            //if (model.ContainsKey("location"))
            //    ht_master.Add("new_location", model["location"]);
            /////更新所在地
            //if (model.ContainsKey("districts"))
            //{
            //    var districts = model["districts"].ToString() ?? string.Empty;
            //    if (!string.IsNullOrWhiteSpace(districts))
            //    {
            //        districts = String.Format("{0}%", districts.Replace("省", "").Replace("市", "").Replace("区", "").Replace("/", "%"));
            //        var mt = new Hashtable();
            //        mt.Add("local", districts);
            //        var area = DAL.GetItem<Tb_Districts>(typeof(Tb_Districts), mt);
            //        if (area != null && !string.IsNullOrWhiteSpace(area.path))
            //        {
            //            var path = area.path.TrimStart('/').Split('/').ToArray();
            //            if (path.Length > 1)
            //            {
            //                ht_master.Add("new_id_province", path[1]);
            //                if (path.Length > 2)
            //                    ht_master.Add("new_id_city", path[2]);
            //                if (path.Length > 3)
            //                    ht_master.Add("new_id_county", path[3]);
            //            }
            //        }
            //    }
            //}

            ////更新个人
            //if (ht.Count > 0)
            //{
            //    ht.Add("id", user.id);
            //    ht.Add("new_id_edit", id_user);
            //    ht.Add("new_rq_edit", DateTime.Now);
            //    ht.Add("new_flag_from", flag_from);
            //    DAL.UpdatePart(typeof(Tb_User), ht);
            //}
            ////更新主人
            //if (ht_master.Count > 0 && (user.id_father == "0" || user.id_father == YesNoFlag.Yes))
            //{
            //    ht_master.Add("id", user.flag_master == (byte)YesNoFlag.Yes ? user.id : user.id_father);
            //    ht_master.Add("new_id_edit", id_user);
            //    ht_master.Add("new_rq_edit", DateTime.Now);
            //    ht_master.Add("new_flag_from", flag_from);
            //    DAL.UpdatePart(typeof(Tb_User), ht_master);
            //}
            br.Success = true;
            br.Message.Add("用户信息保存成功.");
            return br;
        }



        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model">注册信息</param>
        /// <returns>登录结果</returns>
        [Transaction]
        public BaseResult Register(UserRegister model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();

            if (!string.IsNullOrWhiteSpace(model.phone))
            {
                param.Add("username", model.phone);
                param.Add("flag_lx", 1);
                if (DAL.GetCount(typeof(Tb_Account), param) > 0)
                {
                    br.Success = false;
                    br.Message.Add("该手机号已注册，请先登录解除手机绑定再用该手机注册。");
                    br.Data = "mobile";
                    return br;
                }
            }
            //添加门店
            Tb_Shop shop = new Tb_Shop();
            #region
            shop.bm = "1000";
            shop.mc = model.phone;
            shop.id_masteruser = model.id_user;
            shop.id = Guid.NewGuid().ToString();
            shop.lxr = "";
            shop.tel = model.phone;
            shop.rq_create = shop.rq_start = shop.rq_edit = DateTime.Now;
            shop.id_create = shop.id_edit = model.id_user;
            shop.flag_delete = (int)Enums.FlagDelete.NoDelete;
            shop.yzm = MD5Encrypt.Md5(model.password);
            shop.bz = model.shop_bz;
            shop.flag_state = (int)Enums.TbShopFlagState.Opened;
            shop.flag_type = (int)Enums.FlagShopType.总部;
            shop.id_shop_ps = "0";//总部配送中心是 0
            #endregion
            DAL.Add(shop);



            param.Clear();
            param.Add("bm", "Cysoft_YunPos");
            param.Add("db_mc", "Cysoft_YunPos");
            #region 向Syszt_Pos表添加数据
            //var syszt_db = DAL.GetItem<Syszt_Db>(typeof(Syszt_Db), param);
            //if (syszt_db != null)
            //{
            //    Syszt_Pos syszt_pos = new Syszt_Pos();
            //    syszt_pos.id_db = syszt_db.id;
            //    syszt_pos.bm = syszt_pos.xym = syszt_pos.id_gsjg = shop.id;
            //    syszt_pos.rq = DateTime.Now;
            //    DAL.Add(syszt_pos);
            //} 
            #endregion

            //添加用户表
            Tb_User user = new Tb_User();
            #region 添加用户表
            user.id = model.id_user;
            user.id_masteruser = model.id_user;
            user.id_cyuser = model.id_cyuser;
            user.flag_state = (byte)CySoft.Model.Enums.Enums.TbUserFlagState.Yes;
            user.flag_master = (byte)YesNoFlag.Yes;
            user.companyname = model.phone;
            user.name = model.phone;
            user.phone = model.phone;
            user.email = model.email;
            user.username = model.phone;
            user.password = MD5Encrypt.Md5(model.password);
            user.id_create = user.id;
            user.id_father = "0";
            user.rq_start = DateTime.Now;
            user.version = model.version;
            user.rq_stop = DateTime.Now.AddYears(50);
            user.bz = "";
            user.id_shop = shop.id;
            user.flag_delete = (byte)Enums.FlagDelete.NoDelete;
            user.flag_industry = model.industry;
            user.companyno = model.phone;
            #endregion
            DAL.Add(user);



            //Tb_User_Shop userShop = new Tb_User_Shop();
            //#region
            //userShop.id_masteruser = user.id_masteruser;
            //userShop.id_user = user.id;
            //userShop.id_create = userShop.id_edit = user.id;
            //userShop.rq_create = userShop.rq_edit = DateTime.Now;
            //userShop.id_shop = shop.id;
            //userShop.flag_default = (byte)Enums.TbUserShopFlagDefault.Default;
            //#endregion
            //DAL.Add(userShop);

            Tb_Shop_Shop shopShop = new Tb_Shop_Shop();
            shopShop.flag_delete = (byte)Enums.FlagDelete.NoDelete;
            shopShop.flag_state = 1;
            shopShop.id = GetGuid;
            shopShop.id_edit = shopShop.id_create = user.id;
            shopShop.id_masteruser = user.id_masteruser;
            //shopShop.id_shop_father = "0";
            //shopShop.path = string.Format("/0/{0}", shopShop.id);

            shopShop.id_shop_father = "0";
            shopShop.id_shop_child = shop.id;
            shopShop.path = string.Format("/0/{0}", shop.id);

            shopShop.rq_create = shopShop.rq_edit = DateTime.Now;
            DAL.Add(shopShop);

            param.Clear();
            param.Add("id_masteruser", "0");
            var parmList = DAL.QueryList<Ts_Parm>(typeof(Ts_Parm), param).ToList();
            #region
            if (parmList.Any())
            {
                parmList.ForEach(pl =>
                {
                    pl.id_masteruser = user.id;
                    pl.id = Guid.NewGuid().ToString();
                });
                DAL.AddRange(parmList);
            }
            param.Clear();
            param.Add("id_masteruser", "0");
            var parmShopList = DAL.QueryList<Ts_Parm_Shop>(typeof(Ts_Parm_Shop), param).ToList();
            if (parmShopList.Any())
            {
                parmShopList.ForEach(pl =>
                {
                    pl.id_masteruser = user.id;
                    pl.id_shop = "0";
                    pl.id = Guid.NewGuid().ToString();
                });
                DAL.AddRange(parmShopList);
            }
            #endregion
            //如果手机被占用，则删除原有绑定
            param.Clear();
            param.Add("username", user.phone);
            param.Add("flag_lx", AccountFlag.standard);
            param.Add("id_masteruser", "0");
            DAL.Delete(typeof(Tb_Account), param);

            //添加登录帐号
            List<Tb_Account> accountList = new List<Tb_Account>();
            accountList.Add(new Tb_Account()
            {
                id = Guid.NewGuid().ToString(),
                flag_lx = (byte)AccountFlag.standard,
                username = user.phone,
                id_edit = user.id,
                id_user = user.id,
                id_masteruser = "0"
            });
            DAL.AddRange(accountList);

            //初始化角色相关
            param.Clear();
            param.Add("flag_type", Enums.TbRoleFlagType.RoleTemp);
            var roleList = DAL.QueryList<Tb_Role>(roleType, param);//查询角色列表
            var roleIdList = roleList.ToList(m => m.id);
            roleIdList.Add(((int)Enums.TbRoleFixedRoleId.SysManager).ToString());
            roleIdList.Add(((int)Enums.TbRoleFixedRoleId.BusinessMan).ToString());
            param.Clear();
            param.Add("id_roleList", roleIdList);
            var roleIdListFun = roleList.ToList(m => m.id);
            var functionList = DAL.QueryList<Tb_Role_Function>(typeof(Tb_Role_Function), param);
            UtiletyBLL utilety = new UtiletyBLL();
            utilety.DAL = DAL;
            List<Tb_Role_Function> newFunctionList = new List<Tb_Role_Function>();
            List<Tb_Role> newRoleList = new List<Tb_Role>();
            List<Tb_User_Role> userRoleList = new List<Tb_User_Role>();
            #region 遍历角色构建Model
            foreach (var role in roleList)
            {
                var newRole = role.Clone();
                newRole.id = Guid.NewGuid().ToString();
                newRole.id_masteruser = user.id;
                newRole.id_create = user.id;
                newRole.id_edit = user.id;
                newRole.flag_type = 10;
                newRole.flag_update = (byte)YesNoFlag.Yes;
                newRoleList.Add(newRole);

                var query1 = functionList.Where(m => m.id_role == role.id).ToList();
                foreach (var function in query1)
                {
                    function.id_role = newRole.id;
                    function.id_create = user.id;
                }
                newFunctionList.AddRange(query1);

            }
            #endregion


            DAL.AddRange(newRoleList);
            DAL.AddRange(newFunctionList);
            userRoleList.Add(new Tb_User_Role()
            {
                id_role = ((int)Enums.TbRoleFixedRoleId.SysManager).ToString(),
                id_user = user.id,
                id_create = user.id,
                id_masteruser = user.id_masteruser,
                flag_delete = (byte)Enums.FlagDelete.NoDelete
            });
            userRoleList.Add(new Tb_User_Role()
            {
                id_role = ((int)Enums.TbRoleFixedRoleId.BusinessMan).ToString(),
                id_user = user.id,
                id_create = user.id,
                id_masteruser = user.id_masteruser,
                flag_delete = (byte)Enums.FlagDelete.NoDelete
            });
            DAL.AddRange(userRoleList);

            //添加收银员用户
            Tb_User subUser = new Tb_User();
            #region
            subUser.id = Guid.NewGuid().ToString();
            subUser.id_father = user.id;
            subUser.id_masteruser = user.id;
            subUser.id_cyuser = user.id_cyuser;
            subUser.username = "0001";
            subUser.name = user.version == 10 ? "收银员" : "总部收银员";
            subUser.password = MD5Encrypt.Md5("123456");
            subUser.flag_master = (int)Enums.TbUserFlagMaster.UnMaster;
            subUser.id_create = subUser.id_edit = user.id_create;
            subUser.rq_edit = subUser.rq_create = DateTime.Now;
            subUser.rq_start = DateTime.Now;
            subUser.rq_stop = DateTime.Now.AddYears(50);
            subUser.flag_state = (byte)Enums.TbUserFlagState.Yes;
            subUser.version = user.version;
            subUser.id_shop = user.id_shop;
            subUser.flag_industry = user.flag_industry;
            subUser.companyno = user.companyno;
            subUser.flag_delete = (byte)Enums.FlagDelete.NoDelete;
            #endregion
            DAL.Add(subUser);

            //添加收银员帐号
            Tb_Account subAccount = new Tb_Account();
            #region
            subAccount.id = Guid.NewGuid().ToString();
            subAccount.flag_lx = (byte)AccountFlag.standard;
            subAccount.username = subUser.username;
            subAccount.id_user = subUser.id;
            subAccount.id_edit = subUser.id;
            subAccount.rq_edit = DateTime.Now;
            subAccount.id_masteruser = user.id;
            #endregion
            DAL.Add(subAccount);

            var subUserRole = new Tb_User_Role()
            {
                id_role = ((int)Enums.TbRoleFixedRoleId.BusinessMan).ToString(),
                id_user = subUser.id,
                id_create = user.id,
                id_masteruser = user.id_masteruser,
                flag_delete = (byte)Enums.FlagDelete.NoDelete
            };
            DAL.Add(subUserRole);

            //添加会员分类
            param.Clear();
            param.Add("id_masteruser", "0");
            IList<Tb_Hyfl> hyflList = DAL.QueryList<Tb_Hyfl>(typeof(Tb_Hyfl), param);
            if (hyflList.Any())
            {
                foreach (var item in hyflList)
                {
                    item.id = GetGuid;
                    item.id_masteruser = user.id_masteruser;
                    item.id_create = item.id_edit = user.id;
                    item.rq_create = item.rq_edit = DateTime.Now;
                }
                DAL.AddRange(hyflList);
            }


            //添加支付类型
            param.Clear();
            param.Add("id_masteruser", "0");
            param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            IList<Tb_Pay> payTypeList = DAL.QueryList<Tb_Pay>(typeof(Tb_Pay), param);
            if (payTypeList.Any())
            {
                foreach (var item in payTypeList)
                {
                    item.id = GetGuid;
                    item.id_masteruser = user.id_masteruser;
                    item.id_create = item.id_edit = user.id;
                    item.rq_create = item.rq_edit = DateTime.Now;
                }
                DAL.AddRange(payTypeList);
            }

            param.Clear();
            param.Add("id_masteruser", "0");
            IList<Tb_Pay_Config> payConfigList = DAL.QueryList<Tb_Pay_Config>(typeof(Tb_Pay_Config), param);
            if (payConfigList.Any())
            {
                foreach (var item in payConfigList)
                {
                    item.id = Guid.NewGuid().ToString();
                    item.id_masteruser = user.id_masteruser;
                    item.id_shop = shop.id;
                }
                DAL.AddRange(payConfigList);
            }
            param.Clear();
            param.Add("id_masteruser", "0");
            param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            var dwlist = DAL.QueryList<Tb_Dw>(typeof(Tb_Dw), param).ToList();
            if (dwlist.Any())
            {
                dwlist.ForEach(d =>
                {
                    d.id = GetGuid;
                    d.id_create = d.id_edit = user.id;
                    d.rq_create = d.rq_edit = DateTime.Now;
                    d.id_masteruser = user.id_masteruser;
                });
                DAL.AddRange(dwlist);
            }
            param.Clear();
            param.Add("id_masteruser", "0");
            var promote_sorts = DAL.QueryList<Tb_Promote_Sort>(typeof(Tb_Promote_Sort), param).ToList();
            if (promote_sorts.Any())
            {
                promote_sorts.ForEach(s =>
                {
                    s.id_masteruser = user.id_masteruser;
                    s.id = GetGuid;
                    s.rq_create = DateTime.Now;
                    s.id_create = user.id_masteruser;
                });
                DAL.AddRange(promote_sorts);
            }

            param.Clear();
            param.Add("id_masteruser", "0");
            param.Add("id_shop", "0");
            var hyJfruleList = DAL.QueryList<Tb_Hy_Jfrule>(typeof(Tb_Hy_Jfrule), param);
            if (hyJfruleList.Any())
            {
                var nowdate = DateTime.Now;
                var nowdate50 = nowdate.AddYears(50);
                foreach (var item in hyJfruleList)
                {
                    item.id = GetGuid;
                    item.id_masteruser = user.id_masteruser;
                    item.rq_create = nowdate;
                    item.day_b = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day, 0, 0, 0);
                    item.day_e = new DateTime(nowdate50.Year, nowdate50.Month, nowdate50.Day, 23, 59, 59);
                    item.id_create = user.id;
                    //item.id_shop = user.id_shop;
                }
                DAL.AddRange(hyJfruleList);
            }
            br.Message.Add("帐号:" + user.phone + " 注册成功。");
            br.Success = true;
            return br;
        }

        public BaseResult CheckHadRegister(UserRegister model)
        {
            BaseResult res = new BaseResult() { Success = true };
            Hashtable param = new Hashtable();
            if (!string.IsNullOrWhiteSpace(model.phone))
            {
                param.Add("username", model.phone);
                param.Add("flag_lx", 1);
                if (DAL.GetCount(typeof(Tb_Account), param) > 0)
                {
                    res.Success = false;
                    res.Message.Add("该手机号已注册!");
                    res.Data = "mobile";
                    return res;
                }
            }
            return res;
        }

        private void AddTs_Parm(string id_masteruser)
        {
            try
            {
                if (!string.IsNullOrEmpty(id_masteruser))
                {
                    List<Ts_Parm> addList = new List<Ts_Parm>();
                    Hashtable ht = new Hashtable();
                    ht.Add("id_masteruser", "0");
                    var dbList = DAL.QueryList<Ts_Parm>(typeof(Ts_Parm), ht);
                    if (dbList != null && dbList.Count() > 0)
                    {
                        foreach (var item in dbList)
                        {
                            item.id = Guid.NewGuid().ToString();
                            item.id_masteruser = id_masteruser;
                            addList.Add(item);
                        }
                        DAL.AddRange(addList);
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        [Transaction]
        public BaseResult ChangePassword(UserChangePWD model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            param.Add("id", model.id_user);
            Tb_User user = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
            if (user == null)
            {
                br.Message.Add("该用户不存在或资料已丢失！");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }

            string md5Password = MD5Encrypt.Md5(model.oldPassword);
            if (user.password.ToUpper() != md5Password.ToUpper())
            {
                br.Message.Add("旧密码错误！");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }

            DateTime dbDateTime = DAL.GetDbDateTime();
            string md5NewPassword = MD5Encrypt.Md5(model.newPassword);
            param.Clear();
            param.Add("id", model.id_user);
            param.Add("new_password", md5NewPassword);
            param.Add("new_id_edit", model.id_edit);
            param.Add("new_rq_edit", dbDateTime);
            param.Add("new_flag_from", model.flag_from);
            DAL.UpdatePart(userType, param);
            if (user.flag_master == 1 && user.version == 10)
            {
                param.Clear();
                param.Add("id", user.id_shop);
                param.Add("new_yzm", md5NewPassword);
                DAL.UpdatePart(typeof(Tb_Shop), param);
            }
            br.Message.Add(String.Format("修改我的密码。信息：用户流水:{0}，用户名:{1}", user.id, user.username));
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        [Transaction]
        public BaseResult ResetPassword(UserChangePWD model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            param.Add("id", model.id_user);
            Tb_User user = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
            if (user == null)
            {
                br.Message.Add("该用户不存在或资料已丢失！");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }

            //DateTime dbDateTime = DAL.GetDbDateTime();
            string md5NewPassword = MD5Encrypt.Md5(model.newPassword);
            param.Clear();
            param.Add("id", model.id_user);
            param.Add("new_password", md5NewPassword);
            param.Add("new_id_edit", model.id_edit);
            param.Add("new_rq_edit", DateTime.Now);
            param.Add("new_flag_from", model.flag_from);
            DAL.UpdatePart(userType, param);
            if (user.flag_master == 1 && user.version == 10)
            {
                param.Clear();
                param.Add("id", user.id_shop);
                param.Add("new_yzm", md5NewPassword);
                DAL.UpdatePart(typeof(Tb_Shop), param);
            }
            br.Message.Add(String.Format("重置密码。信息：用户流水:{0}，用户名:{1}", user.id, user.username));
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 更新用户信息（服务接口）
        /// </summary>
        [Transaction]
        public BaseResult UpdataPart(Tb_User model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            if (!model.phone.IsEmpty())
            {
                param.Add("id", model.id);
                var user = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
                if (user == null)
                {
                    br.Message.Add("当前用户已不存在!");
                    return br;
                }
                //param.Clear();
                //param.Add("id_user",model.id);
                //var listAccount= DAL.QueryList<Tb_Account>(typeof(Tb_Account), param);
                if (user.phone.IsEmpty())//
                {
                    param.Clear();
                    param.Add("username", model.phone);
                    if (DAL.GetCount(typeof(Tb_Account), param) > 0)
                    {
                        br.Success = false;
                        br.Message.Add("此手机号已存在!");
                        return br;
                    }
                    #region
                    Tb_Account new_account = new Tb_Account()
                    {
                        flag_lx = 1,
                        id = GetGuid,
                        id_edit = model.id,
                        id_masteruser = model.id_masteruser,
                        id_user = model.id,
                        rq_edit = DateTime.Now,
                        username = model.phone
                    };
                    #endregion
                    DAL.Add(new_account);
                    param.Clear();
                    param.Add("new_phone", model.phone);
                }
                else if (model.phone != user.phone)
                {
                    param.Clear();
                    param.Add("username", user.phone);
                    var userAccount = DAL.GetItem<Tb_Account>(typeof(Tb_Account), param);
                    if (userAccount != null)
                    {
                        //判断是否为当前主帐户下的用户登录号
                        #region 
                        if ((userAccount.id_masteruser == "0" && userAccount.id_user != model.id_masteruser)
                                            || (userAccount.id_masteruser != "0" && userAccount.id_masteruser != model.id_masteruser))
                        {
                            br.Success = false;
                            br.Message.Add("数据异常，请联系管理员!");
                            return br;
                        }
                        param.Clear();
                        param.Add("new_username", model.phone);
                        param.Add("id", userAccount.id);
                        DAL.UpdatePart(typeof(Tb_Account), param);
                        #endregion
                    }
                    else
                    {
                        #region 
                        Tb_Account new_account = new Tb_Account()
                        {
                            flag_lx = 1,
                            id = GetGuid,
                            id_edit = model.id,
                            id_masteruser = model.id_masteruser,
                            id_user = model.id,
                            rq_edit = DateTime.Now,
                            username = model.phone
                        };
                        #endregion
                        DAL.Add(new_account);
                    }
                    //判断企业号(根据主用户来处理)是否需要修改(只有在当前用户为主用户且企业号为默认手机号的情况下才修改企业号)
                    if (!model.id_masteruser.IsEmpty() && model.id_masteruser == model.id && user.username == user.companyno)
                    {
                        param.Clear();
                        param.Add("id_masteruser", model.id_masteruser);
                        param.Add("new_companyno", model.phone);
                        DAL.UpdatePart(typeof(Tb_User), param);
                    }
                    param.Clear();
                    param.Add("new_phone", model.phone);
                    param.Add("new_username", model.phone);
                }
            }
            param.Add("id", model.id);
            param.Add("new_name", model.name);
            param.Add("new_job", model.job);
            param.Add("new_email", model.email);
            param.Add("new_qq", model.qq);
            //param.Add("new_flag_from", model.flag_from);

            DAL.UpdatePart(typeof(Tb_User), param);

            br.Message.Add(String.Format("{0}", "操作成功！"));
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 更新
        /// lz
        /// 2016-08-16
        /// </summary>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            Tb_User_Edit model = (Tb_User_Edit)entity;

            DateTime dbDateTime = DAL.GetDbDateTime();

            param.Clear();
            param.Add("id", model.id);
            param.Add("new_tel", model.tel);
            param.Add("new_name", model.name);
            param.Add("new_job", model.job);
            param.Add("new_phone", model.phone);
            param.Add("new_qq", model.qq);
            param.Add("new_email", model.email);
            param.Add("new_id_edit", model.id_edit);
            param.Add("new_rq_edit", dbDateTime);
            param.Add("new_address", model.address);
            param.Add("new_companyname", model.companyname);
            param.Add("new_id_shop", model.id_shop);


            //根据city_id获取省市地区 并赋值
            if (model.id_city != null && model.id_city > 0)
            {
                var mt = new Hashtable();
                mt.Add("id", model.id_city);
                var area = DAL.GetItem<Tb_Districts>(typeof(Tb_Districts), mt);
                if (area != null && !string.IsNullOrWhiteSpace(area.path))
                {
                    var path = area.path.TrimStart('/').Split('/').ToArray();
                    if (path.Length > 1)
                    {
                        int id_province = 0;
                        int.TryParse(path[1], out id_province);

                        param.Add("new_id_province", id_province);

                        if (path.Length > 2)
                        {
                            int id_city = 0;
                            int.TryParse(path[2], out id_city);
                            param.Add("new_id_city", id_city);
                        }
                        if (path.Length > 3)
                        {
                            int id_county = 0;
                            int.TryParse(path[3], out id_county);
                            param.Add("new_id_county", id_county);
                        }
                    }
                }
            }

            DAL.UpdatePart(typeof(Tb_User), param);

            param.Clear();
            param.Add("id_user", model.id);

            if (model.flag_master == (byte)YesNoFlag.Yes)
            {
                param.Add("not_id_roleList", new string[] { "1", "2" });
            }

            //DAL.Delete(typeof(Tb_User_Role), param);
            // 2017-02-10 新增 flag_delete 字段 lz
            param.Add("new_flag_delete", (byte)Enums.FlagDelete.Deleted);
            DAL.UpdatePart(typeof(Tb_User_Role), param);



            if (!model.id_roles.IsEmpty())
            {
                string[] id_roles = model.id_roles.Split(',').Distinct().ToArray();
                List<Tb_User_Role> userRoleList = new List<Tb_User_Role>(id_roles.Length);
                if (model.flag_master == (byte)YesNoFlag.Yes)
                {
                    id_roles = id_roles.Where(d => d != "1" && d != "2").ToArray();
                }
                foreach (string id_role in id_roles)
                {
                    if (!string.IsNullOrEmpty(id_role))
                        userRoleList.Add(new Tb_User_Role() { id_role = id_role.Trim(), id_user = model.id, id_create = model.id_edit, id_masteruser = model.id_masteruser, flag_delete = (byte)Enums.FlagDelete.NoDelete });
                }

                if (userRoleList.Count() > 0)
                {
                    param.Clear();
                    param.Add("id_user", model.id);
                    var roleList = DAL.QueryList<Tb_User_Role>(typeof(Tb_User_Role), param);

                    foreach (var item in userRoleList)
                    {
                        if (roleList != null && roleList.Where(d => d.id_role == item.id_role && d.id_user == item.id_user).Count() > 0)
                        {
                            param.Clear();
                            param.Add("id_user", model.id);
                            if (model.flag_master == (byte)YesNoFlag.Yes)
                            {
                                param.Add("not_id_roleList", new string[] { "1", "2" });
                            }
                            param.Add("id_role", item.id_role);
                            param.Add("new_flag_delete", (byte)Enums.FlagDelete.NoDelete);
                            DAL.UpdatePart(typeof(Tb_User_Role), param);
                        }
                        else
                            DAL.Add(item);
                    }
                }
            }

            DataCache.Remove(model.id + "_purview");

            param.Clear();
            param.Add("id_masteruser", model.id_masteruser);
            param.Add("id_user", model.id);
            //param.Add("new_flag_delete", (byte)Enums.FlagDelete.Deleted);
            //DAL.UpdatePart(typeof(Tb_User_Shop), param);

            DAL.Delete(typeof(Tb_User_Shop), param);

            if (!string.IsNullOrWhiteSpace(model.id_shops))
            {
                List<Tb_User_Shop> userShops = new List<Tb_User_Shop>();
                var shopArr = model.id_shops.Split(',');
                foreach (var s in shopArr)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        userShops.Add(new Tb_User_Shop()
                        {
                            id_create = model.id_edit,
                            id_edit = model.id_edit,
                            id_shop = s,
                            id_user = model.id,
                            id_masteruser = model.id_masteruser,
                            rq_create = DateTime.Now,
                            rq_edit = DateTime.Now
                            //flag_delete=(byte)Enums.FlagDelete.NoDelete
                        });
                    }
                }
                if (userShops.Any())
                {
                    DAL.AddRange(userShops);
                }
            }

            br.Success = true;
            br.Message.Add(String.Format("更新员工帐号。信息：流水号:{0}，姓名:{1}", model.id, model.name));
            return br;
        }

        /// <summary>
        /// 新增
        /// </summary>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Tb_User_Edit model = (Tb_User_Edit)entity;
            Hashtable param = new Hashtable();

            br = CyVerify.CheckUserName(model.username);
            if (!br.Success)
                return br;

            param.Add("username", model.username);
            param.Add("id_masteruser", model.id_masteruser);
            var dbAccount = DAL.GetItem<Tb_Account>(typeof(Tb_Account), param);
            if (dbAccount != null && !string.IsNullOrEmpty(dbAccount.id))
            {

                param.Clear();
                param.Add("id", dbAccount.id_user);
                Tb_User user = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
                if (user == null)
                {
                    br.Message.Add(String.Format("该登录帐号异常,请更换登录名!登录帐号:{0}", model.username));
                    br.Success = false;
                    return br;
                }
                else
                {
                    if (user.flag_delete == (byte)Enums.FlagDelete.Deleted)
                    {
                        br.Message.Add("该登录帐号已被删除,是否恢复此帐号为未删除状态?");
                        br.Level = ErrorLevel.Warning;
                        br.Success = false;
                        br.Data = user.id;
                        return br;
                    }
                    else
                    {
                        br.Message.Add(String.Format("该登录帐号已存在!登录帐号:{0}", model.username));
                        br.Success = false;
                        return br;
                    }
                }
            }

            model.password = MD5Encrypt.Md5(model.password);
            model.flag_master = (int)Enums.TbUserFlagMaster.UnMaster;
            model.flag_state = (byte)Enums.TbUserFlagState.Yes;


            //根据city_id获取省市地区 并赋值
            if (model.id_city != null && model.id_city > 0)
            {
                var mt = new Hashtable();
                mt.Add("id", model.id_city);
                var area = DAL.GetItem<Tb_Districts>(typeof(Tb_Districts), mt);
                if (area != null && !string.IsNullOrWhiteSpace(area.path))
                {
                    var path = area.path.TrimStart('/').Split('/').ToArray();
                    if (path.Length > 1)
                    {
                        int id_province = 0;
                        int.TryParse(path[1], out id_province);
                        model.id_province = id_province;
                        if (path.Length > 2)
                        {
                            int id_city = 0;
                            int.TryParse(path[2], out id_city);
                            model.id_city = id_city;
                        }
                        if (path.Length > 3)
                        {
                            int id_county = 0;
                            int.TryParse(path[3], out id_county);
                            model.id_county = id_county;
                        }
                    }
                }
            }

            DAL.Add<Tb_User>(model);

            if (!model.id_roles.IsEmpty())
            {
                string[] id_roles = model.id_roles.Split(',').Distinct().ToArray();
                List<Tb_User_Role> userRoleList = new List<Tb_User_Role>(id_roles.Length);
                foreach (string id_role in id_roles)
                {
                    if (!string.IsNullOrEmpty(id_role))
                    {
                        userRoleList.Add(new Tb_User_Role() { id_role = id_role.Trim(), id_user = model.id, id_create = model.id_create, id_masteruser = model.id_masteruser, flag_delete = (byte)Enums.FlagDelete.NoDelete });
                    }
                }
                DAL.AddRange(userRoleList);
            }

            //添加登录帐号
            List<Tb_Account> accountList = new List<Tb_Account>();
            Tb_Account account = new Tb_Account()
            {
                id = Guid.NewGuid().ToString(),
                flag_lx = (byte)AccountFlag.standard,
                username = model.username,
                id_edit = model.id,
                id_user = model.id,
                id_masteruser = model.id_masteruser
            };
            accountList.Add(account);
            DAL.AddRange(accountList);

            //添加用户管理门店关系表
            if (!string.IsNullOrWhiteSpace(model.id_shops))
            {
                List<Tb_User_Shop> userShops = new List<Tb_User_Shop>();
                var shopArr = model.id_shops.Split(',');
                foreach (var s in shopArr)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        userShops.Add(new Tb_User_Shop()
                        {
                            id_create = model.id_create,
                            id_edit = model.id_create,
                            id_shop = s,
                            id_user = model.id,
                            id_masteruser = model.id_masteruser,
                            rq_create = DateTime.Now,
                            rq_edit = DateTime.Now
                            //flag_delete=(byte)Enums.FlagDelete.NoDelete
                        });
                    }
                }
                if (userShops.Any())
                {
                    DAL.AddRange(userShops);
                }
            }


            br.Message.Add(String.Format("添加用户。信息：流水号:{0}，用户名:{1}", model.id, model.username));
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.GetCount(userType, param);
            if (pn.TotalCount > 0)
            {
                IList<Tb_User_Query> lst = DAL.QueryPage<Tb_User_Query>(userType, param) ?? new List<Tb_User_Query>();
                pn.Data = lst;
            }
            else
            {
                pn.Data = new List<Tb_User_Query>();
            }
            pn.Success = true;
            return pn;
        }




        /// <summary>
        /// 删除
        /// lz
        /// 2016-08-16
        /// </summary>
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string id = param["id"].ToString();
            string id_user = param["id_user"].ToString();
            string id_masteruser = param["id_masteruser"].ToString();

            param.Clear();
            param.Add("id", id);
            param.Add("id_father", id_masteruser);
            Tb_User user = DAL.GetItem<Tb_User>(userType, param);
            if (user == null)
            {
                br.Success = false;
                br.Message.Add("删除失败！该用户不存在。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            if (user.flag_master == (byte)YesNoFlag.Yes)
            {
                br.Success = false;
                br.Message.Add("删除失败！主用户不允许删除。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            //param.Clear();
            //param.Add("id", id);
            //param.Add("id_father", id_masteruser);
            //DAL.Delete(userType, param);

            //param.Clear();
            //param.Add("id_user", id);
            //param.Add("id_masteruser", id_masteruser);
            //DAL.Delete(typeof(Tb_Account), param);

            param.Clear();
            param.Add("id", id);
            param.Add("id_father", id_masteruser);
            param.Add("new_flag_delete", (byte)Enums.FlagDelete.Deleted);
            DAL.UpdatePart(typeof(Tb_User), param);

            br.Message.Add(String.Format("删除员工帐号。信息：流水号:{0}, 用户名:{1}", user.id, user.username));
            br.Success = true;
            return br;
        }


        /// <summary>
        /// 停用
        /// lz
        /// 2016-08-16
        /// </summary>
        [Transaction]
        public override BaseResult Stop(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string id = param["id"].ToString();
            //string id_user = param["id_user"].ToString();
            string id_masteruser = param["id_masteruser"].ToString();
            param.Clear();
            param.Add("id", id);
            param.Add("id_father", id_masteruser);
            Tb_User user = DAL.GetItem<Tb_User>(userType, param);
            if (user == null)
            {
                br.Success = false;
                br.Message.Add("停用失败！该用户不存在。");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            param.Clear();
            param.Add("id", id);
            param.Add("new_flag_state", (byte)Enums.TbUserFlagState.No);
            DAL.UpdatePart(userType, param);

            br.Message.Add(String.Format("停用员工帐号。信息：流水号:{0}, 用户名:{1}", user.id, user.username));
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 启用
        /// lz
        ///  2016-08-16
        /// </summary>
        [Transaction]
        public override BaseResult Active(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string id = param["id"].ToString();
            //string id_user = param["id_user"].ToString();
            string id_masteruser = param["id_masteruser"].ToString();
            param.Clear();
            param.Add("id", id);
            param.Add("id_father", id_masteruser);
            Tb_User user = DAL.GetItem<Tb_User>(userType, param);
            if (user == null)
            {
                br.Success = false;
                br.Message.Add("启用失败！该用户不存在。");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            param.Clear();
            param.Add("id", id);
            param.Add("new_flag_state", (byte)Enums.TbUserFlagState.Yes);
            DAL.UpdatePart(userType, param);

            br.Message.Add(String.Format("启用员工帐号。信息：流水号:{0}, 用户名:{1}", user.id, user.username));
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获得用户完整信息
        /// lz
        /// 2016-08-16
        /// </summary>
        public BaseResult GetInfo(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string id = param["id"].ToString();
            string id_masteruser = param["id_masteruser"].ToString();

            param.Clear();
            param.Add("id", id);
            var user = DAL.QueryItem<Tb_User_Query>(userType, param);

            param.Clear();
            param.Add("flag_master", 1);
            param.Add("_id_masteruser", id_masteruser);
            var roleList = DAL.QueryList<Tb_Role>(roleType, param).OrderBy(d => d.flag_type);

            param.Clear();
            param.Add("id_user", id);
            param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            var userRoleList = DAL.QueryList<Tb_User_Role>(typeof(Tb_User_Role), param);
            var roles = (from item in roleList select new Tb_User_Role_Query() { id_role = item.id, isChecked = userRoleList.FirstOrDefault(m => m.id_role == item.id) != null, name_role = item.name }).ToList();

            param.Clear();
            param.Add("user", user);
            param.Add("roles", roles);
            br.Data = param;
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获得权限的模块
        /// lxt
        /// 2015-04-13
        /// </summary>
        public BaseResult GetRoleFunctions(long[] id_roleList)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            param.Add("id_roleList", id_roleList);
            param.Add("flag_stop", YesNoFlag.No);
            param.Add("flag_type", "module");
            var list = DAL.QueryList<Tb_Function>(typeof(Tb_Function), param);

            br.Data = list.ToList(m => m.id);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 取消帐号绑定
        /// tim
        /// 2015-6-8
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public BaseResult Unbing(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string id = param["id"].ToString();
            string id_user = param["id_user"].ToString();
            string username = (param["username"] ?? string.Empty).ToString();
            string password = (param["password"] ?? string.Empty).ToString();

            if (string.IsNullOrWhiteSpace(password))
            {
                br.Success = false;
                br.Message.Add("请提供用户名密码。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            var ht = new Hashtable();
            ht.Add("id", param["id"]);
            var user = DAL.QueryItem<Tb_User_Query>(userType, ht);
            if (user == null)
            {
                br.Success = false;
                br.Message.Add("用户已不存在，请刷新页面重试。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            if (param.ContainsKey("phone") && user.valid_phone.Equals(0))
            {
                br.Success = true;
                br.Message.Add("手机号已被取消绑定。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            if (param.ContainsKey("email") && user.valid_email.Equals(0))
            {
                br.Success = true;
                br.Message.Add("邮箱已被取消绑定。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            string md5Password = MD5Encrypt.Encrypt(password);
            if (!user.password.Equals(md5Password))
            {
                br.Success = false;
                br.Message.Add("用户名密码不正确。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            if (string.IsNullOrWhiteSpace(user.username) && string.IsNullOrWhiteSpace(username))
            {
                br.Success = true;
                br.Message.Add("取消绑定，必须创建一个用户名。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            if (string.IsNullOrWhiteSpace(user.username) && !string.IsNullOrWhiteSpace(username))
            {
                ht.Clear();
                ht.Add("username", username);
                if (DAL.GetCount(typeof(Tb_Account), ht) > 0)
                {
                    br.Success = false;
                    br.Message.Add("用户名已经被使用，请重新命名。");
                    br.Level = ErrorLevel.Warning;
                    return br;
                }
                var acccount = new Tb_Account() { id = Guid.NewGuid().ToString(), flag_lx = (byte)AccountFlag.standard, id_edit = id_user, id_user = id, rq_edit = DateTime.Now, username = username };
                DAL.Add(acccount);
                ht.Clear();
                ht.Add("id", id);
                ht.Add("new_username", username);
                ht.Add("new_id_edit", id_user);
                ht.Add("new_rq_edit", DateTime.Now);
                DAL.UpdatePart(typeof(Tb_User), ht);
            }


            username = (param.ContainsKey("phone") ? param["phone"] : param["email"]).ToString();
            ht.Clear();
            ht.Add("id_user", id);
            ht.Add("username", username);
            DAL.Delete(typeof(Tb_Account), ht);
            br.Success = true;
            br.Message.Add(string.Format("取消[{0}]绑定成功。", username));
            return br;
        }


        /// <summary>
        /// 手机绑定
        /// tim
        /// 2015-6-8
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public BaseResult BingPhone(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string id = param["id"].ToString();
            string id_user = param["id_user"].ToString();
            string phone = (param["phone"] ?? string.Empty).ToString();

            if (string.IsNullOrWhiteSpace(phone))
            {
                br.Success = false;
                br.Message.Add("手机号不能为空，请刷新页面重试。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            var ht = new Hashtable();
            ht.Add("id", param["id"]);
            var user = DAL.QueryItem<Tb_User_Query>(userType, ht);
            if (user == null)
            {
                br.Success = false;
                br.Message.Add("用户已不存在，请刷新页面重试。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            if (param.ContainsKey("phone") && !user.valid_phone.Equals(0))
            {
                br.Success = true;
                br.Message.Add("手机号已被绑定，请先取消绑定后再绑定该手机号。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            ht.Clear();
            ht.Add("username", phone);
            if (DAL.GetCount(typeof(Tb_Account), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("手机号已经被使用，请取消绑定后再绑定该用户。");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            var acccount = new Tb_Account() { id = Guid.NewGuid().ToString(), flag_lx = (byte)AccountFlag.standard, id_edit = id_user, id_user = id, rq_edit = DateTime.Now, username = phone };
            DAL.Add(acccount);
            ht.Clear();
            ht.Add("id", id);
            ht.Add("new_phone", phone);
            ht.Add("new_id_edit", id_user);
            ht.Add("new_rq_edit", DateTime.Now);
            DAL.UpdatePart(typeof(Tb_User), ht);

            br.Success = true;
            br.Message.Add(string.Format("[{0}]验证成功。", phone));
            return br;
        }



        /// <summary>
        /// 手机绑定
        /// tim
        /// 2015-6-8
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public BaseResult BingEmail(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string email = (param["email"] ?? string.Empty).ToString();
            string key = (param["key"] ?? string.Empty).ToString();

            if (string.IsNullOrWhiteSpace(key))
            {
                br.Success = false;
                br.Message.Add("邮件验证码不能为空。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                br.Success = false;
                br.Message.Add("邮件不能为空。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            var ht = new Hashtable();
            ht.Add("key_email", key);
            ht.Add("email", email);
            var list = DAL.QueryList<Tb_User_Query>(userType, ht);
            if (list == null || !list.Count.Equals(1))
            {
                br.Success = false;
                br.Message.Add("用户已不存在。");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            var user = list[0];
            string id = user.id;
            string id_user = id;
            //if (!user.email.Equals(email) || !user.key_email.Equals(key))
            //{
            //    br.Success = false;
            //    br.Message.Add("邮件验证码已经失效或已使用过。");
            //    br.Level = ErrorLevel.Warning;
            //    return br;
            //}

            //if (!user.rq_email_key.HasValue || user.rq_email_key.Value.AddDays(1) < DateTime.Now)
            //{
            //    br.Success = false;
            //    br.Message.Add("邮件验证码已经失效或已使用过。");
            //    br.Level = ErrorLevel.Warning;
            //    return br;
            //}

            ht.Clear();
            ht.Add("username", email);
            DAL.Delete(typeof(Tb_Account), ht);

            var acccount = new Tb_Account() { id = Guid.NewGuid().ToString(), flag_lx = (byte)AccountFlag.standard, id_edit = id_user, id_user = id, rq_edit = DateTime.Now, username = email };
            DAL.Add(acccount);
            ht.Clear();
            ht.Add("id", id);
            ht.Add("new_key_email", string.Empty);
            ht.Add("new_email", email);
            ht.Add("new_id_edit", id_user);
            ht.Add("new_rq_edit", DateTime.Now);
            DAL.UpdatePart(typeof(Tb_User), ht);

            br.Success = true;
            br.Message.Add(string.Format("[{0}]验证成功。", email));
            return br;
        }


        /// <summary>
        /// 发送邮箱验证
        /// tim
        /// 2015-6-8
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public BaseResult VaildEmail(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string id = param["id"].ToString();
            string id_user = param["id_user"].ToString();
            string email = (param["email"] ?? string.Empty).ToString();

            if (string.IsNullOrWhiteSpace(email))
            {
                br.Success = false;
                br.Message.Add("邮箱不能为空。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            var ht = new Hashtable();
            ht.Add("id", param["id"]);
            var user = DAL.QueryItem<Tb_User_Query>(userType, ht);
            if (user == null)
            {
                br.Success = false;
                br.Message.Add("用户已不存在，请刷新页面重试。");
                br.Level = ErrorLevel.Warning;
                return br;
            }



            ht.Clear();
            ht.Add("username", email);
            if (DAL.GetCount(typeof(Tb_Account), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("邮箱已经被使用，请取消绑定后再绑定该用户。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            var key = Guid.NewGuid().ToString().Replace("-", string.Empty);

            ht.Clear();
            ht.Add("id", id);
            ht.Add("new_email", email);
            ht.Add("new_key_email", key);
            ht.Add("new_rq_email_key", DateTime.Now);
            DAL.UpdatePart(typeof(Tb_User), ht);



            //MailMessage mailObj = new MailMessage();
            //mailObj.From = new MailAddress("tim@chaoying.com.cn"); //发送人邮箱地址
            //mailObj.To.Add(email);   //收件人邮箱地址
            //mailObj.Subject = "订货易邮箱验证";
            //mailObj.IsBodyHtml = true;
            //mailObj.Body = string.Format("这是订货易邮箱验证邮件，请<a href=\"http://www.dhy.hk/Account/VaildEmail?key={0}&email={1}&check=false&type=v_mail\">点击确认</a>", key, email);
            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "smtp.exmail.qq.com";
            //smtp.Port = 465;
            //smtp.EnableSsl = true;
            //smtp.Timeout = 1;
            //smtp.UseDefaultCredentials = true;
            //smtp.Credentials = new System.Net.NetworkCredential("tim@chaoying.com.cn", "tim123");  //发送人的登录名和密码
            //smtp.SendAsync(mailObj, mailObj);

            // MailHelper.Send();

            br.Success = true;
            br.Message.Add(string.Format("已发送验证邮件到[{0}]，请注意查收。", email));
            return br;
        }

        /// <summary>
        /// 创建用户名
        /// tim
        /// 2015-06-19
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public BaseResult CreateAccount(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string id = param["id"].ToString();
            string id_user = param["id_user"].ToString();
            string account = (param["account"] ?? string.Empty).ToString();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(id_user))
            {
                br.Success = false;
                br.Message.Add("请选择用户，重新登录。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            br = CyVerify.CheckUserName(account);
            if (!br.Success)
                return br;

            var ht = new Hashtable();
            ht.Add("id", id);
            var user = DAL.QueryItem<Tb_User_Query>(userType, ht);
            if (user == null)
            {
                br.Success = false;
                br.Message.Add("用户不存在，请刷新页面重试。");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            if (!user.username.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("该用户已设置了用户名，请重新登录。");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            ht.Clear();
            ht.Add("username", account);
            if (DAL.GetCount(typeof(Tb_Account), ht) > 0)
            {
                br.Success = false;
                br.Message.Add(string.Format("用户名已被使用，请另取一个用户名。用户名：{0}", account));
                br.Level = ErrorLevel.Warning;
                return br;
            }

            var model = new Tb_Account();
            model.id = Guid.NewGuid().ToString();
            model.flag_lx = (byte)AccountFlag.standard;
            model.id_user = id;
            model.id_edit = id_user;
            model.username = account;
            model.rq_edit = DateTime.Now;
            DAL.Add<Tb_Account>(model);

            ht.Clear();
            ht.Add("new_username", account);
            ht.Add("id", id);
            DAL.UpdatePart(typeof(Tb_User), ht);

            br.Success = true;
            br.Message.Add(string.Format("添加用户名成功.用户ID：{0}，用户名：{1}", id, account));
            return br;
        }

        /// <summary>
        /// 获取业务员签到记录
        /// 2015-7-20 wzp
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PageNavigate GetCheckInfo(Hashtable param)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.GetCount(typeof(Tb_User_Checkin), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_User_Checkin>(typeof(Tb_User_Checkin), param);
                pn.Success = true;
            }
            return pn;
        }

        /// <summary>
        /// 分页获取平台下的用户结果集
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PageNavigate QueryUserPage(Hashtable param)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.GetCount(typeof(Tb_User), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_User_Query>(typeof(Tb_User), param);
                pn.Success = true;
            }
            return pn;
        }


        public BaseResult ShopRegister(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            if (param == null || param.Count <= 0 || !param.ContainsKey("username") || !param.ContainsKey("id_masteruser"))
            {
                res.Success = false;
                res.Message.Add("主帐号或企业号错误!");
                return res;
            }
            var id_m = param["id_masteruser"] + "";
            var username = param["username"];
            if (id_m == "")
            {
                res.Success = false;
                res.Message.Add("主帐号或企业号错误!");
                return res;
            }
            if (id_m == "0")
            {
                var account = DAL.GetItem<Tb_Account>(typeof(Tb_Account), param);
                if (account != null)
                {
                    param.Clear();
                    param.Add("id", account.id_user);
                    param.Add("flag_master", (int)Enums.TbUserFlagMaster.Master);
                    res.Data = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
                }
            }
            else
            {
                param.Clear();
                param.Add("id", id_m);
                var masterModel = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
                if (masterModel == null)
                {
                    res.Success = false;
                    res.Message.Add("主帐号或企业号错误!");
                    return res;
                }
                if (masterModel.flag_master == (int)Enums.TbUserFlagMaster.Master)
                {
                    param.Clear();
                    param.Add("username", username);
                    param.Add("id_masteruser", "0");
                    var account = DAL.GetItem<Tb_Account>(typeof(Tb_Account), param);
                    if (account != null)
                    {
                        res.Data = masterModel;
                    }
                }
                else
                {
                    param.Clear();
                    param.Add("username", username);
                    param.Add("id_masteruser", id_m);
                    var account = DAL.GetItem<Tb_Account>(typeof(Tb_Account), param);
                    param.Clear();
                    param.Add("id", account.id_user);
                    param.Add("flag_master", (int)Enums.TbUserFlagMaster.UnMaster);
                    res.Data = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
                }
            }
            return res;
        }


        public BaseResult GetAllUser(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Tb_User>(typeof(Tb_User), param);
            br.Success = true;
            return br;
        }



        /// <summary>
        /// 恢复删除员工帐号
        /// lz
        ///  2016-10-09
        /// </summary>
        [Transaction]
        public override BaseResult Init(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string id = param["id"].ToString();
            string id_masteruser = param["id_masteruser"].ToString();
            param.Clear();
            param.Add("id", id);
            param.Add("id_father", id_masteruser);
            Tb_User user = DAL.GetItem<Tb_User>(userType, param);
            if (user == null)
            {
                br.Success = false;
                br.Message.Add("恢复失败！该用户不存在。");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            param.Clear();
            param.Add("id", id);
            param.Add("id_father", id_masteruser);
            param.Add("new_flag_delete", (byte)Enums.FlagDelete.NoDelete);
            DAL.UpdatePart(userType, param);

            br.Message.Add(String.Format("恢复删除员工帐号成功。信息：流水号:{0}, 用户名:{1}", user.id, user.username));
            br.Success = true;
            return br;
        }

        #region 接口调用
        [Transaction]
        public BaseResult ChangeUserPwd(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            if (param == null || param.Count <= 0
                || !param.ContainsKey("id")
                || !param.ContainsKey("old_pwd")
                || !param.ContainsKey("new_pwd"))
            {
                res.Success = false;
                res.Message.Add("部份必要参数缺失!");
                return res;
            }
            var id = string.Format("{0}", param["id"]);
            var old_pwd = string.Format("{0}", param["old_pwd"]);
            var new_pwd = string.Format("{0}", param["new_pwd"]);
            if (string.IsNullOrEmpty(old_pwd))
            {
                res.Success = false;
                res.Message.Add("请输入原密码!");
                return res;
            }
            param.Clear();
            param.Add("id", id);
            var model = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
            if (model == null)
            {
                res.Success = false;
                res.Message.Add("用户信息不存在!");
                return res;
            }
            if (model.password.ToLower() != old_pwd.ToLower())
            {
                res.Success = false;
                res.Message.Add("原密码不正确!");
                return res;
            }
            param.Add("new_password", new_pwd);
            DAL.UpdatePart(typeof(Tb_User), param);
            if (model.version == 10 && model.flag_master == 1 && !string.IsNullOrEmpty(model.id_shop))
            {
                param.Clear();
                param.Add("id", model.id_shop);
                param.Add("new_yzm", new_pwd);
                DAL.UpdatePart(typeof(Tb_Shop), param);
            }
            res.Message.Add("修改成功!");
            return res;
        }
        #endregion





        public BaseResult ChangeUserVersion(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            if (
                param == null
                || !param.Contains("id")
                || !param.Contains("version"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            var id = string.Format("{0}", param["id"]);
            var version = string.Format("{0}", param["version"]);
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(version))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            param.Clear();
            param.Add("id", id);
            param.Add("new_version", version);
            if (DAL.UpdatePart(typeof(Tb_User), param) <= 0)
            {
                res.Success = false;
                res.Message.Add("操作失败!");
                return res;
            }
            return res;
        }


        public BaseResult GetUserInfo(Hashtable param)
        {
            BaseResult br = new BaseResult() { Success = true };
            Tb_User user = DAL.GetItem<Tb_User>(typeof(Tb_User), param);
            br.Data = user;
            return br;
        }





        [Transaction]
        public BaseResult ChangeCompanyno(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            if (param == null ||
                !param.Contains("id_masteruser") ||
                !param.Contains("companyno"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            string id_masteruser = string.Format("{0}", param["id_masteruser"]);
            string companyno = string.Format("{0}", param["companyno"]);
            if (string.IsNullOrEmpty(companyno))
            {
                res.Success = false;
                res.Message.Add("企业号不能为空!");
                return res;
            }
            if (string.IsNullOrEmpty(id_masteruser))
            {
                res.Success = false;
                res.Message.Add("未获取到主用户ID!");
                return res;
            }
            Hashtable ht = new Hashtable();
            ht.Add("id", id_masteruser);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            Tb_User user = DAL.GetItem<Tb_User>(typeof(Tb_User), ht);
            if (user != null)
            {
                #region

                if (!string.IsNullOrEmpty(user.companyno) && user.phone != user.companyno)
                {
                    res.Success = false;
                    res.Message.Add("企业号已修改过一次，不能再次修改!");
                    return res;
                }
                ht.Clear();
                ht.Add("companyno", companyno);
                if (DAL.GetCount(typeof(Tb_User), ht) > 0)
                {
                    res.Success = false;
                    res.Message.Add("此企业号已存在,请重新输入!");
                    return res;
                }
                ht.Clear();
                ht.Add("username", companyno);
                if (DAL.GetCount(typeof(Tb_Account), ht) > 0)
                {
                    res.Success = false;
                    res.Message.Add("此企业号已存在,请重新输入!");
                    return res;
                }

                #endregion

                ht.Clear();
                ht.Add("new_companyno", companyno);
                ht.Add("id_masteruser", id_masteruser);
                DAL.UpdatePart(typeof(Tb_User), ht);
                Tb_Account account = new Tb_Account()
                {
                    id = GetGuid,
                    flag_lx = 1,
                    id_masteruser = "0",
                    id_user = id_masteruser,
                    id_edit = id_masteruser,
                    rq_edit = DateTime.Now,
                    username = companyno
                };
                DAL.Add(account);
            }
            else
            {
                res.Success = false;
                res.Message.Add("未获取到主用户ID!");
                return res;
            }
            return res;
        }



    }
}
