using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.IDAL;
using CySoft.Model.Tb;
using CySoft.Model.Enums;
using CySoft.Frame.Attributes;
using CySoft.Model.Ts;
using CySoft.Model.Other;
using CySoft.Utility;
using CySoft.Model;
using CySoft.Model.Flags;

namespace CySoft.BLL.GoodsBLL
{
    public class Tb_ShopBLL : BaseBLL, ITb_ShopBLL
    {

        public ITb_ShopDAL Tb_ShopDAL { get; set; }
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            res.Data = DAL.QueryList<Tb_Shop>(typeof(Tb_Shop), param).FirstOrDefault();
            return res;
        }


        public BaseResult GetMaxBMInfo(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            res.Data = Tb_ShopDAL.GetMaxBMInfo(typeof(Tb_Shop), param);
            return res;
        }
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult res = new BaseResult() { Success = true };
            Hashtable param = entity as Hashtable;
            if (param == null || param.Count <= 0)
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            Hashtable ht = new Hashtable();
            ht.Add("new_mc", param["mc"]);
            //ht.Add("new_bm", param["bm"]);
            ht.Add("new_email", param["email"]);
            ht.Add("new_phone", param["phone"]);
            ht.Add("new_qq", param["qq"]);
            ht.Add("new_address", param["address"]);
            ht.Add("new_lxr", param["lxr"]);
            ht.Add("new_bz", param["bz"]);
            ht.Add("new_tel", param["tel"]);
            ht.Add("new_zipcode", param["zipcode"]);
            ht.Add("new_fax", param["fax"]);
            ht.Add("new_id_edit", param["id_user"]);
            ht.Add("new_rq_edit", DateTime.Now);
            ht.Add("id", param["id"]);
            if (param.ContainsKey("flag_state")) ht.Add("new_flag_state", param["flag_state"].ToString());//flag_state lz 20161011 add 
            if (DAL.UpdatePart(typeof(Tb_Shop), ht) <= 0)
            {
                res.Success = false;
                res.Message.Add("修改失败!");
            }
            else
            {
                ht.Clear();
                ht.Add("id_masteruser", param["id_masteruser"]);
                ht.Add("new_flag_industry", param["flag_industry"]);
                DAL.UpdatePart(typeof(Tb_User), ht);
            }
            return res;
        }

        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.GetCount(typeof(Tb_Shop), param);
            if (pn.TotalCount > 0)
            {
                IList<Tb_Shop> lst = DAL.QueryPage<Tb_Shop>(typeof(Tb_Shop), param) ?? new List<Tb_Shop>();
                pn.Data = lst;
            }
            else
            {
                pn.Data = new List<Tb_Shop>();
            }
            pn.Success = true;
            return pn;
        }

        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            #region 获取数据
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            #endregion

            #region 转换Model
            var tempModel = TurnShopList(param);
            if (tempModel.flag_type == (int)Enums.FlagShopType.总部)
            {
                br.Message.Add(String.Format("创建失败 不允许添加总部！", tempModel.flag_type));
                br.Success = false;
                return br;
            }

            if (tempModel.flag_type != (int)Enums.FlagShopType.配送中心 && string.IsNullOrEmpty(tempModel.id_shop_ps))
            {
                if (param.ContainsKey("id_shop_user") && !string.IsNullOrEmpty(param["id_shop_user"].ToString()))
                    tempModel.id_shop_ps = param["id_shop_user"].ToString();
                else
                {
                    br.Message.Add(String.Format("创建失败 请选择配送中心！", tempModel.flag_type));
                    br.Success = false;
                    return br;
                }
            }

            #endregion

            #region 验证是否有克隆门店
            if (string.IsNullOrEmpty(tempModel.id_cloneshop))
            {
                br.Message.Add(String.Format("创建失败 未输入克隆门店！", tempModel.mc));
                br.Success = false;
                return br;
            }
            #endregion

            #region 验证编码

            #region 验证编码格式
            if (tempModel.bm.Length > 6)
            {
                br.Message.Add(String.Format("编码：{0} 格式不正确 编码长度最大6位 ,请核实！", tempModel.bm));
                br.Success = false;
                return br;
            }
            #endregion

            #region 验证编码是否重复
            ht.Clear();
            ht.Add("id_masteruser", tempModel.id_masteruser);
            ht.Add("bm", tempModel.bm);
            var dbBMModel = DAL.GetItem<Tb_Shop>(typeof(Tb_Shop), ht);
            if (dbBMModel != null && !string.IsNullOrEmpty(dbBMModel.id))
            {
                br.Message.Add(String.Format("编码：{0} 重复 ,请核实！", tempModel.bm));
                br.Success = false;
                return br;
            }
            #endregion

            #endregion

            #region 验证名称是否存在
            ht.Clear();
            ht.Add("id_masteruser", tempModel.id_masteruser);
            ht.Add("mc", tempModel.mc);
            var dbMCModel = DAL.GetItem<Tb_Shop>(typeof(Tb_Shop), ht);
            if (dbMCModel != null && !string.IsNullOrEmpty(dbMCModel.id))
            {
                br.Message.Add(String.Format("名称：{0} 重复 ,请核实！", tempModel.mc));
                br.Success = false;
                return br;
            }
            #endregion

            #region 验证验证码是否存在
            ht.Clear();
            ht.Add("id_masteruser", tempModel.id_masteruser);
            ht.Add("yzm", tempModel.yzm);
            var dbYZMModel = DAL.GetItem<Tb_Shop>(typeof(Tb_Shop), ht);
            if (dbYZMModel != null && !string.IsNullOrEmpty(dbYZMModel.id))
            {
                br.Message.Add(String.Format("验证码：{0} 重复 ,请核实！", param["yzm"].ToString()));
                br.Success = false;
                return br;
            }
            #endregion

            #region 验证主用户门店信息
            string id_shop_father = param["id_shop_master"].ToString();
            if (string.IsNullOrEmpty(id_shop_father))
            {
                br.Message.Add(String.Format("获取主用户信息失败,请重新登录重试！"));
                br.Success = false;
                return br;
            }
            #endregion

            #region 保存至门店表
            DAL.Add<Tb_Shop>(tempModel);
            #endregion

            #region 添加门店操作员

            Hashtable query_user = new Hashtable();
            query_user.Add("id", tempModel.id_masteruser);
            var user = DAL.QueryItem<Tb_User>(typeof(Tb_User), query_user);
            if (user != null)
            {
                Tb_User subUser = new Tb_User();
                #region
                subUser.id = GetGuid;
                subUser.id_father = tempModel.id_masteruser;
                subUser.id_masteruser = tempModel.id_masteruser;
                subUser.id_cyuser = user.id_cyuser;
                subUser.username = string.Format("0{0}", tempModel.bm);
                subUser.name = string.Format("{0}门店操作员", tempModel.mc);
                subUser.password = MD5Encrypt.Md5("123456");
                subUser.flag_master = (int)Enums.TbUserFlagMaster.UnMaster;
                subUser.id_create = subUser.id_edit = user.id_create;
                subUser.rq_edit = subUser.rq_create = DateTime.Now;
                subUser.rq_start = subUser.rq_create;
                subUser.rq_stop = user.rq_stop;
                subUser.flag_state = (byte)Enums.TbUserFlagState.Yes;
                subUser.version = user.version;
                subUser.id_shop = tempModel.id;
                subUser.flag_industry = user.flag_industry;
                subUser.companyno = user.companyno;
                subUser.flag_delete = (byte)Enums.FlagDelete.NoDelete;
                #endregion
                DAL.Add(subUser);
                //添加帐号
                Tb_Account subAccount = new Tb_Account();
                #region
                subAccount.id = GetGuid;
                subAccount.flag_lx = (byte)AccountFlag.standard;
                subAccount.username = string.Format("0{0}", tempModel.bm);
                subAccount.id_user = subUser.id;
                subAccount.id_edit = subUser.id;
                subAccount.rq_edit = DateTime.Now;
                subAccount.id_masteruser = user.id;
                #endregion
                DAL.Add(subAccount);
                //添加角色
                List<Tb_User_Role> subRoles = new List<Tb_User_Role>();
                subRoles.Add(new Tb_User_Role()
                {
                    id_role = ((int)Enums.TbRoleFixedRoleId.BusinessMan).ToString(),
                    id_user = subUser.id,
                    id_create = tempModel.id_create,
                    id_masteruser = user.id_masteruser,
                    flag_delete = (byte)Enums.FlagDelete.NoDelete
                });
                subRoles.Add(new Tb_User_Role()
                {
                    id_role = ((int)Enums.TbRoleFixedRoleId.SysManager).ToString(),
                    id_user = subUser.id,
                    id_create = tempModel.id_create,
                    id_masteruser = user.id_masteruser,
                    flag_delete = (byte)Enums.FlagDelete.NoDelete
                });
                DAL.AddRange(subRoles);
            }

            #endregion

            #region 如果是加盟店 新增客户 新增前判断是否有客户分类
            if (tempModel.flag_type == (int)Enums.FlagShopType.加盟店)
            {
                ht.Clear();
                ht.Add("id_masteruser", tempModel.id_masteruser);
                ht.Add("id_shop_relate_not_null", "1");
                var ybdKhModel = DAL.QueryList<Tb_Kh>(typeof(Tb_Kh), ht).FirstOrDefault();
                if (ybdKhModel != null && !string.IsNullOrEmpty(ybdKhModel.id))
                {
                    #region 新增客户
                    Tb_Kh addKhModel = new Tb_Kh();
                    addKhModel.id_masteruser = tempModel.id_masteruser;
                    addKhModel.id = GetGuid;
                    addKhModel.bm = tempModel.bm;
                    addKhModel.mc = tempModel.mc;
                    addKhModel.id_khfl = ybdKhModel.id_khfl;
                    addKhModel.companytel = tempModel.tel;
                    addKhModel.zjm = CySoft.Utility.PinYin.GetChineseSpell(addKhModel.mc);
                    addKhModel.tel = tempModel.phone;
                    addKhModel.lxr = tempModel.lxr;
                    addKhModel.email = tempModel.email;
                    addKhModel.zipcode = tempModel.zipcode;
                    addKhModel.address = tempModel.address;
                    addKhModel.je_xyed = 0;
                    addKhModel.je_xyed_temp = 0;
                    addKhModel.id_shop_relate = tempModel.id;
                    addKhModel.flag_state = (byte)Enums.TbShopFlagState.Opened;
                    addKhModel.flag_delete = (int)Enums.FlagDelete.NoDelete;
                    addKhModel.bz = tempModel.bz;
                    addKhModel.id_create = tempModel.id_create;
                    addKhModel.rq_create = tempModel.rq_create;
                    DAL.Add(addKhModel);
                    #endregion
                    #region 更新门店绑定客户
                    ht.Clear();
                    ht.Add("id_masteruser", tempModel.id_masteruser);
                    ht.Add("id", tempModel.id);
                    ht.Add("new_id_kh", addKhModel.id);
                    if (DAL.UpdatePart(typeof(Tb_Shop), ht) <= 0)
                    {
                        br.Message.Clear();
                        br.Message.Add(String.Format("操作失败更新门店客户信息失败 请重试！"));
                        br.Success = false;
                        br.Level = ErrorLevel.Warning;
                        throw new CySoftException(br);
                    }
                    #endregion
                }
                else
                {
                    #region 新增客户分类
                    Tb_Khfl addKhflModel = new Tb_Khfl();
                    addKhflModel.id_masteruser = tempModel.id_masteruser;
                    addKhflModel.id = GetGuid;
                    addKhflModel.bm = "";
                    addKhflModel.mc = Enums.FlagShopType.加盟店.ToString();
                    addKhflModel.path = "/0/" + addKhflModel.id;
                    addKhflModel.id_farther = "0";
                    addKhflModel.sort_id = 1;
                    addKhflModel.id_create = tempModel.id_create;
                    addKhflModel.rq_create = tempModel.rq_create;
                    addKhflModel.flag_delete = (int)Enums.FlagDelete.NoDelete;
                    DAL.Add(addKhflModel);
                    #endregion
                    #region 新增客户
                    Tb_Kh addKhModel = new Tb_Kh();
                    addKhModel.id_masteruser = tempModel.id_masteruser;
                    addKhModel.id = GetGuid;
                    addKhModel.bm = tempModel.bm;
                    addKhModel.mc = tempModel.mc;
                    addKhModel.id_khfl = addKhflModel.id;
                    addKhModel.companytel = tempModel.tel;
                    addKhModel.zjm = CySoft.Utility.PinYin.GetChineseSpell(addKhModel.mc);
                    addKhModel.tel = tempModel.phone;
                    addKhModel.lxr = tempModel.lxr;
                    addKhModel.email = tempModel.email;
                    addKhModel.zipcode = tempModel.zipcode;
                    addKhModel.address = tempModel.address;
                    addKhModel.je_xyed = 0;
                    addKhModel.je_xyed_temp = 0;
                    addKhModel.id_shop_relate = tempModel.id;
                    addKhModel.flag_state = (byte)Enums.TbShopFlagState.Opened;
                    addKhModel.flag_delete = (int)Enums.FlagDelete.NoDelete;
                    addKhModel.bz = tempModel.bz;
                    addKhModel.id_create = tempModel.id_create;
                    addKhModel.rq_create = tempModel.rq_create;
                    DAL.Add(addKhModel);
                    #endregion
                    #region 更新门店绑定客户
                    ht.Clear();
                    ht.Add("id_masteruser", tempModel.id_masteruser);
                    ht.Add("id", tempModel.id);
                    ht.Add("new_id_kh", addKhModel.id);
                    if (DAL.UpdatePart(typeof(Tb_Shop), ht) <= 0)
                    {
                        br.Message.Clear();
                        br.Message.Add(String.Format("操作失败更新门店客户信息失败 请重试！"));
                        br.Success = false;
                        br.Level = ErrorLevel.Warning;
                        throw new CySoftException(br);
                    }
                    #endregion
                }
            }

            #region 备注
            //if (!string.IsNullOrEmpty(tempModel.id_kh))
            //{
            //    ht.Clear();
            //    ht.Add("id_masteruser", tempModel.id_masteruser);
            //    ht.Add("id", tempModel.id_kh);
            //    var khModel = DAL.QueryList<Tb_Kh>(typeof(Tb_Kh), ht).FirstOrDefault() ;
            //    if (khModel == null || string.IsNullOrEmpty(khModel.id))
            //    {
            //        br.Message.Clear();
            //        br.Message.Add(String.Format("操作失败未查询到本客户的信息！"));
            //        br.Success = false;
            //        br.Level = ErrorLevel.Warning;
            //        throw new CySoftException(br);
            //    }

            //    if (!string.IsNullOrEmpty(khModel.id_shop_relate))
            //    {
            //        br.Message.Clear();
            //        br.Message.Add(String.Format("操作失败本客户已经被其他门店绑定,请选择其他客户！"));
            //        br.Success = false;
            //        br.Level = ErrorLevel.Warning;
            //        throw new CySoftException(br);
            //    }

            //    ht.Clear();
            //    ht.Add("id_masteruser", tempModel.id_masteruser);
            //    ht.Add("id", tempModel.id_kh);
            //    ht.Add("flag_delete", tempModel.flag_delete);
            //    ht.Add("yes_fsyw", 1);
            //    var fsywList = DAL.QueryList<Tb_Kh>(typeof(Tb_Kh), ht);
            //    if (fsywList != null && fsywList.Count() > 0)
            //    {
            //        br.Message.Clear();
            //        br.Message.Add(String.Format("操作失败本客户已经发生业务,请选择其他客户！"));
            //        br.Success = false;
            //        br.Level = ErrorLevel.Warning;
            //        throw new CySoftException(br);
            //    }

            //    ht.Clear();
            //    ht.Add("id_masteruser", tempModel.id_masteruser);
            //    ht.Add("id_kh", tempModel.id_kh);
            //    ht.Add("flag_delete", tempModel.flag_delete);
            //    var bdList = DAL.QueryList<Tb_Shop>(typeof(Tb_Shop), ht);
            //    if (bdList != null && bdList.Count() > 1)
            //    {
            //        br.Message.Clear();
            //        br.Message.Add(String.Format("操作失败本客户已经被其他门店绑定,请选择其他客户！"));
            //        br.Success = false;
            //        br.Level = ErrorLevel.Warning;
            //        throw new CySoftException(br);
            //    }

            //    ht.Clear();
            //    ht.Add("id_masteruser", tempModel.id_masteruser);
            //    ht.Add("id", tempModel.id_kh);
            //    ht.Add("new_id_shop_relate", tempModel.id);
            //    if (DAL.UpdatePart(typeof(Tb_Kh), ht) <= 0)
            //    {
            //        br.Message.Clear();
            //        br.Message.Add(String.Format("操作失败更新客户信息绑定门店失败 请重试！"));
            //        br.Success = false;
            //        br.Level = ErrorLevel.Warning;
            //        throw new CySoftException(br);
            //    }

            //} 
            #endregion
            #endregion

            #region 验证是否已经达到购买的数量

            if (PublicSign.flagCheckService == "1")
            {

                #region 获取已存在的开启门店数
                ht.Clear();
                ht.Add("id_masteruser", tempModel.id_masteruser);
                ht.Add("flag_state", (byte)Enums.TbShopFlagState.Opened);
                ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                var dbCount = DAL.GetCount(typeof(Tb_Shop), ht);
                #endregion

                #region 取服务数据
                string bm = GetServiceBM(param["version"].ToString());
                if (string.IsNullOrEmpty(bm))
                {
                    br.Message.Clear();
                    br.Message.Add(String.Format("操作失败 获取服务编码异常 请检查版本是否正常！"));
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    throw new CySoftException(br);
                }

                ht.Clear();
                ht.Add("id_cyuser", param["id_cyuser"].ToString());
                ht.Add("bm", bm);
                ht.Add("service", "GetService");
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("rq_create_master_shop", param["rq_create_master_shop"].ToString());
                var cyServiceHas = GetCYService(ht);
                br = CheckServiceForAdd(cyServiceHas, dbCount, bm);
                if (!br.Success)
                {
                    ht.Clear();
                    ht.Add("id_cyuser", param["id_cyuser"].ToString());
                    ht.Add("bm", bm);
                    ht.Add("service", "GetService");
                    ht.Add("id_masteruser", param["id_masteruser"].ToString());
                    ht.Add("do_post", "1");
                    ht.Add("rq_create_master_shop", param["rq_create_master_shop"].ToString());
                    cyServiceHas = GetCYService(ht);
                    br = CheckServiceForAdd(cyServiceHas, dbCount, bm);
                    if (!br.Success)
                    {
                        br.Success = false;
                        br.Level = ErrorLevel.Warning;
                        throw new CySoftException(br);
                    }
                }
                #endregion

            }

            #endregion

            #region 保存至Tb_Shop_Shop表
            Tb_Shop_Shop shopShop = new Tb_Shop_Shop();
            shopShop.flag_delete = (byte)Enums.FlagDelete.NoDelete;
            shopShop.flag_state = 1;
            shopShop.id = GetGuid;
            shopShop.id_edit = shopShop.id_create = tempModel.id_create;
            shopShop.id_masteruser = tempModel.id_masteruser;
            //shopShop.id_shop_father = "0";
            //shopShop.id_shop_child = tempModel.id;
            //shopShop.path = string.Format("/0/{0}", tempModel.id);
            shopShop.rq_create = shopShop.rq_edit = DateTime.Now;
            //如果不是主用户 所属门店都在主门店下面
            //if (tempModel.id_masteruser != tempModel.id_create)
            //{
            //shopShop.id_shop_father = id_shop_father;
            //shopShop.path = string.Format("/0/{0}/{1}", id_shop_father, tempModel.id);
            //}

            ht.Clear();
            ht.Add("id_masteruser", tempModel.id_masteruser);
            ht.Add("id_shop_child", param["id_shop_user"].ToString());
            var dbFatherModel = DAL.GetItem<Tb_Shop_Shop>(typeof(Tb_Shop_Shop), ht);
            if (dbFatherModel == null || string.IsNullOrEmpty(dbFatherModel.id))
            {
                br.Message.Clear();
                br.Message.Add(String.Format("获取操作者门店信息数据异常 ,请重试！"));
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                throw new CySoftException(br);
            }

            shopShop.id_shop_child = tempModel.id;
            shopShop.id_shop_father = dbFatherModel.id_shop_child;
            shopShop.path = string.Format("{0}/{1}", dbFatherModel.path, tempModel.id);

            DAL.Add(shopShop);
            #endregion

            #region 向Syszt_Pos表添加数据 已注释
            //ht.Clear();
            //ht.Add("bm", "Cysoft_YunPos");
            //ht.Add("db_mc", "Cysoft_YunPos");
            //var syszt_db = DAL.GetItem<Syszt_Db>(typeof(Syszt_Db), ht);
            //if (syszt_db != null)
            //{
            //    Syszt_Pos syszt_pos = new Syszt_Pos();
            //    syszt_pos.id_db = syszt_db.id;
            //    syszt_pos.bm = syszt_pos.xym = syszt_pos.id_gsjg = tempModel.id;
            //    syszt_pos.rq = DateTime.Now;
            //    DAL.Add(syszt_pos);
            //}
            #endregion

            #region 新增门店的支付配置信息
            List<Tb_Pay_Config> addPayConfigList = new List<Model.Tb.Tb_Pay_Config>();
            ht.Clear();
            ht.Add("id_masteruser", "0");
            var dbPayConfigList = DAL.QueryList<Tb_Pay_Config>(typeof(Tb_Pay_Config), ht);
            if (dbPayConfigList != null && dbPayConfigList.Count() > 0)
            {
                foreach (var item in dbPayConfigList)
                {
                    Tb_Pay_Config addPayConfig = new Tb_Pay_Config()
                    {
                        id_masteruser = tempModel.id_masteruser,
                        id = Guid.NewGuid().ToString(),
                        id_shop = tempModel.id,
                        flag_type = item.flag_type,
                        parmcode = item.parmcode,
                        parmname = item.parmname,
                        parmvalue = item.parmvalue
                    };
                    addPayConfigList.Add(addPayConfig);
                }
            }
            DAL.AddRange(addPayConfigList);
            #endregion

            #region 新增门店参数 门店商品
            #region 新增门店参数(从克隆门店复制)
            List<Ts_Parm_Shop> addShopParmList = new List<Ts_Parm_Shop>();
            ht.Clear();
            ht.Add("id_shop", tempModel.id_cloneshop);
            var dbShopParmList = DAL.QueryList<Ts_Parm_Shop>(typeof(Ts_Parm_Shop), ht);
            if (dbShopParmList != null && dbShopParmList.Count() > 0)
            {
                foreach (var item in dbShopParmList)
                {
                    if (!string.IsNullOrEmpty(item.parmvalue))
                    {
                        Ts_Parm_Shop addShopParm = new Ts_Parm_Shop()
                        {
                            id_masteruser = tempModel.id_masteruser,
                            id = Guid.NewGuid().ToString(),
                            id_shop = tempModel.id,
                            parmcode = item.parmcode,
                            parmname = item.parmname,
                            parmvalue = item.parmvalue,
                            version = item.version,
                            flag_type = item.flag_type,
                            flag_editstyle = item.flag_editstyle,
                            parmdescribe = item.parmdescribe,
                            regex = item.regex
                        };
                        addShopParmList.Add(addShopParm);
                    }
                }
            }
            DAL.AddRange(addShopParmList);
            #endregion

            #region 新增门店商品(从克隆门店复制)
            List<Tb_Shopsp> addShopspList = new List<Tb_Shopsp>();
            ht.Clear();
            ht.Add("id_shop", tempModel.id_cloneshop);
            var dbShopspList = DAL.QueryList<Tb_Shopsp>(typeof(Tb_Shopsp), ht);

            if (dbShopspList != null && dbShopspList.Count() > 0)
            {
                foreach (var item in dbShopspList)
                {
                    var newItem = item.Clone();
                    newItem.id_masteruser = tempModel.id_masteruser;
                    newItem.id_shop = tempModel.id;
                    addShopspList.Add(newItem);
                }

                foreach (var item in addShopspList)
                {
                    //if (item.id == item.id_kcsp)
                    //{
                    //    var tempID = item.id.Clone().ToString();
                    //    var tempKCSP = item.id_kcsp.Clone().ToString();
                    //    item.id = Guid.NewGuid().ToString();
                    //    item.id_kcsp = tempKCSP;
                    //    foreach (var childItem in addShopspList.Where(d => d.id_kcsp == tempKCSP && d.id != tempID))
                    //    {
                    //        childItem.id = Guid.NewGuid().ToString();
                    //        childItem.id_kcsp = tempKCSP;
                    //    }
                    //}
                    //else
                    //{
                    item.id = Guid.NewGuid().ToString();
                    //}
                }
            }

            //if (addShopspList.Where(d => d.id_sp == "0" && d.id_kcsp == "0").Count() <= 0)
            //{
            //    Tb_Shopsp addModel = new Tb_Shopsp();
            //    addModel.id = GetGuid;
            //    addModel.id_sp = "0";
            //    addModel.id_kcsp = "0";
            //    addModel.mc = "无码收银商品";
            //    addModel.id_masteruser = tempModel.id_masteruser;
            //    addModel.id_shop = tempModel.id;
            //    addModel.dw = "";
            //    addModel.bm = "";
            //    addModel.id_spfl = "";
            //    addModel.barcode = "";
            //    addModel.zjm = "";
            //    addModel.zhl = 1;
            //    addModel.cd = "";
            //    addModel.flag_state = 1;
            //    addModel.flag_czfs = 0;
            //    addModel.pic_path = "";
            //    addModel.id_create = param["id_user"].ToString();
            //    addModel.rq_create = DateTime.Now;
            //    addModel.id_edit = null;
            //    addModel.rq_edit = null;
            //    addModel.flag_delete = (int)Enums.FlagDelete.NoDelete;
            //    addModel.yxq = 0;
            //    addModel.id_gys = "";
            //addShopspList.Add(addModel);
            //}

            DAL.AddRange(addShopspList);
            #endregion
            #endregion

            br.Message.Add(String.Format("新增门店。流水号：{0}，名称:{1}", tempModel.id, tempModel.mc));
            br.Success = true;
            return br;
        }

        [Transaction]
        public override BaseResult Save(dynamic entity)
        {
            #region 获取数据
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            #endregion

            #region 获取门店原信息 以及验证

            #region 获取门店原信息
            ht.Clear();
            ht.Add("id", param["id"].ToString());
            var dbShop = DAL.GetItem<Tb_Shop>(typeof(Tb_Shop), ht);
            if (dbShop == null) { br.Success = false; br.Message.Add("门店不存在"); br.Level = ErrorLevel.Warning; return br; }
            if (dbShop.flag_delete == (byte)Enums.FlagDelete.Deleted) { br.Success = false; br.Message.Add("门店已被删除"); br.Level = ErrorLevel.Warning; return br; }
            #endregion

            #region 验证

            #region 总部不允许停用
            //总部不允许停用
            if (param.ContainsKey("flag_state") && param["flag_state"].ToString() == ((byte)Enums.TbShopFlagState.Closed).ToString())
            {
                if (dbShop.flag_type == (int)Enums.FlagShopType.总部)
                {
                    br.Success = false; br.Message.Add("操作失败 总部门店不允许停用！"); br.Level = ErrorLevel.Warning; return br;
                }
            }
            #endregion

            #region 总部 分公司不允许修改门店
            //总部 分公司不允许修改门店
            if (param.ContainsKey("flag_type") && param["flag_type"].ToString() != dbShop.flag_type.ToString())
            {
                if (dbShop.flag_type == (int)Enums.FlagShopType.总部)
                {
                    br.Message.Add(String.Format("总部类型 不允许修改！"));
                    br.Success = false;
                    return br;
                }
                else if (dbShop.flag_type == (int)Enums.FlagShopType.分公司)
                {
                    br.Message.Add(String.Format("分公司类型 不允许修改！"));
                    br.Success = false;
                    return br;
                }
            }
            #endregion

            #region 如果非总部的门店修改门店类型为总部时 不允许修改
            //如果非总部的门店修改门店类型为总部时 不允许修改
            if (dbShop.flag_type != (int)Enums.FlagShopType.总部 && param.ContainsKey("flag_type") && param["flag_type"].ToString() == ((int)Enums.FlagShopType.总部).ToString())
            {
                br.Success = false; br.Message.Add("操作失败 总部类型不允许设置！"); br.Level = ErrorLevel.Warning; return br;
            }
            #endregion

            #region 修改类型为 非配送中心的门店必须设置配送中心
            //修改类型为 非配送中心的门店必须设置配送中心
            if (param.ContainsKey("flag_type") && !string.IsNullOrEmpty(param["flag_type"].ToString()) && param["flag_type"].ToString() != ((int)Enums.FlagShopType.配送中心).ToString())
            {
                if (!param.ContainsKey("id_shop_ps") || string.IsNullOrEmpty(param["id_shop_ps"].ToString()))
                {
                    br.Success = false; br.Message.Add("操作失败 该门店类型必须设置配送门店！"); br.Level = ErrorLevel.Warning; return br;
                }
            }
            #endregion

            #region 不修改类型 但是原门店类型为 非总部 非配送中心的门店必须有 配送中心
            //不修改类型 但是原门店类型为 非总部 非配送中心的门店必须有 配送中心
            if (!param.ContainsKey("flag_type") || string.IsNullOrEmpty(param["flag_type"].ToString()))
            {
                if (dbShop.flag_type != (int)Enums.FlagShopType.总部 && dbShop.flag_type != (int)Enums.FlagShopType.配送中心)
                {
                    if (!param.ContainsKey("id_shop_ps") || string.IsNullOrEmpty(param["id_shop_ps"].ToString()))
                    {
                        br.Success = false; br.Message.Add("操作失败 该门店类型必须设置配送门店！"); br.Level = ErrorLevel.Warning; return br;
                    }
                }
            }
            #endregion

            #region 验证编码
            if (param.ContainsKey("bm"))
            {
                #region 验证编码格式
                if (param["bm"].ToString().Length > 6)
                {
                    br.Message.Add(String.Format("编码：{0} 格式不正确 编码长度最大6位 ,请核实！", param["bm"].ToString()));
                    br.Success = false;
                    return br;
                }
                #endregion

                #region 验证编码是否重复
                ht.Clear();
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("bm", param["bm"].ToString());
                ht.Add("not_id", param["id"].ToString());
                var dbBMModel = DAL.GetItem<Tb_Shop>(typeof(Tb_Shop), ht);
                if (dbBMModel != null && !string.IsNullOrEmpty(dbBMModel.id))
                {
                    br.Message.Add(String.Format("编码：{0} 重复 ,请核实！", param["bm"].ToString()));
                    br.Success = false;
                    return br;
                }
                #endregion
            }
            #endregion

            #region 验证名称是否存在
            if (param.ContainsKey("mc"))
            {
                ht.Clear();
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("mc", param["mc"].ToString());
                ht.Add("not_id", param["id"].ToString());
                var dbMCModel = DAL.GetItem<Tb_Shop>(typeof(Tb_Shop), ht);
                if (dbMCModel != null && !string.IsNullOrEmpty(dbMCModel.id))
                {
                    br.Message.Add(String.Format("名称：{0} 重复 ,请核实！", param["mc"].ToString()));
                    br.Success = false;
                    return br;
                }
            }
            #endregion

            #region 验证验证码
            string new_yzm = dbShop.yzm;
            //2017-03-14 去掉验证码的修改 统一重置来修改
            //if (param.ContainsKey("yzm"))
            //{
            //    if (dbShop.yzm != param["yzm"].ToString())
            //    {
            //        ht.Clear();
            //        ht.Add("id_masteruser", param["id_masteruser"].ToString());
            //        ht.Add("yzm", CySoft.Utility.MD5Encrypt.Md5(param["yzm"].ToString()));
            //        ht.Add("not_id", param["id"].ToString());
            //        var dbYZMModel = DAL.GetItem<Tb_Shop>(typeof(Tb_Shop), ht);
            //        if (dbYZMModel != null && !string.IsNullOrEmpty(dbYZMModel.id))
            //        {
            //            br.Message.Add(String.Format("验证码：{0} 重复 ,请核实！", param["yzm"].ToString()));
            //            br.Success = false;
            //            return br;
            //        }
            //        new_yzm = CySoft.Utility.MD5Encrypt.Md5(param["yzm"].ToString());
            //    }
            //}
            #endregion

            #endregion

            #endregion

            #region 更新操作
            #region 更新字段
            //更新商品名称
            ht.Clear();
            ht.Add("id", dbShop.id);
            //if (param.ContainsKey("yzm")) ht.Add("new_yzm", param["yzm"].ToString());//yzm
            //ht.Add("new_yzm", new_yzm);//yzm 去掉验证码的修改 统一重置来修改
            if (param.ContainsKey("bm") && param["bm"].ToString() != dbShop.bm) ht.Add("new_bm", param["bm"].ToString());//bm
            if (param.ContainsKey("mc") && param["mc"].ToString() != dbShop.mc) ht.Add("new_mc", param["mc"].ToString());//mc
            if (param.ContainsKey("email") && param["email"].ToString() != dbShop.email) ht.Add("new_email", param["email"].ToString());//email
            if (param.ContainsKey("phone") && param["phone"].ToString() != dbShop.phone) ht.Add("new_phone", param["phone"].ToString());//phone
            if (param.ContainsKey("tel") && param["tel"].ToString() != dbShop.tel) ht.Add("new_tel", param["tel"].ToString());//tel
            if (param.ContainsKey("fax") && param["fax"].ToString() != dbShop.fax) ht.Add("new_fax", param["fax"].ToString());//fax
            if (param.ContainsKey("lxr") && param["lxr"].ToString() != dbShop.lxr) ht.Add("new_lxr", param["lxr"].ToString());//lxr
            if (param.ContainsKey("zipcode") && param["zipcode"].ToString() != dbShop.zipcode) ht.Add("new_zipcode", param["zipcode"].ToString());//zipcode
            if (param.ContainsKey("address") && param["address"].ToString() != dbShop.address) ht.Add("new_address", param["address"].ToString());//address
            if (param.ContainsKey("bz") && param["bz"].ToString() != dbShop.bz) ht.Add("new_bz", param["bz"].ToString());//bz
            if (param.ContainsKey("id_user") && param["id_user"].ToString() != dbShop.id_edit) ht.Add("new_id_edit", param["id_user"].ToString());//id_user
            if (param.ContainsKey("pic_path") && param["pic_path"].ToString() != dbShop.pic_path) ht.Add("new_pic_path", param["pic_path"].ToString());//pic_path
            if (param.ContainsKey("qq") && param["qq"].ToString() != dbShop.qq) ht.Add("new_qq", param["qq"].ToString());//qq

            //flag_state lz 20161011 add
            if (param.ContainsKey("flag_state") && param["flag_state"].ToString() != dbShop.flag_state.ToString()) ht.Add("new_flag_state", param["flag_state"].ToString());//

            //id_kh lz 20170220 add
            if (string.IsNullOrEmpty(dbShop.id_kh) && param.ContainsKey("kh_id") && !string.IsNullOrEmpty(param["kh_id"].ToString()))
                ht.Add("new_id_kh", param["kh_id"].ToString());//kh_id

            //flag_type lz 20170221 add
            if (param.ContainsKey("flag_type") && !string.IsNullOrEmpty(param["flag_type"].ToString()) && dbShop.flag_type != int.Parse(param["flag_type"].ToString()))
                ht.Add("new_flag_type", param["flag_type"].ToString());//flag_type

            if (param.ContainsKey("id_shop_ps") && !string.IsNullOrEmpty(param["id_shop_ps"].ToString()) && dbShop.id_shop_ps != param["id_shop_ps"].ToString())
                ht.Add("new_id_shop_ps", param["id_shop_ps"].ToString());//id_shop_ps

            ht.Add("new_rq_edit", DateTime.Now);//修改日期 
            #endregion
            #region 更新
            if (DAL.UpdatePart(typeof(Tb_Shop), ht) <= 0)
            {
                br.Success = false;
                br.Message.Add("更新操作失败.");
            }
            else
            {
                #region 加盟店->加盟店 时 修改对应客户的信息
                if (param["flag_type"].ToString() == ((int)Enums.FlagShopType.加盟店).ToString() && dbShop.flag_type == (int)Enums.FlagShopType.加盟店)
                {
                    ht.Clear();
                    ht.Add("id_masteruser", dbShop.id_masteruser);
                    ht.Add("id", dbShop.id_kh);

                    if (param.ContainsKey("bm") && param["bm"].ToString() != dbShop.bm) ht.Add("new_bm", param["bm"].ToString());//bm
                    if (param.ContainsKey("mc") && param["mc"].ToString() != dbShop.mc) ht.Add("new_mc", param["mc"].ToString());//mc
                    if (param.ContainsKey("lxr") && param["lxr"].ToString() != dbShop.lxr) ht.Add("new_lxr", param["lxr"].ToString());//lxr
                    if (param.ContainsKey("phone") && param["phone"].ToString() != dbShop.phone) ht.Add("new_tel", param["phone"].ToString());//phone
                    if (param.ContainsKey("tel") && param["tel"].ToString() != dbShop.tel) ht.Add("new_companytel", param["tel"].ToString());//tel
                    if (param.ContainsKey("flag_state") && param["flag_state"].ToString() != dbShop.flag_state.ToString()) ht.Add("new_flag_state", param["flag_state"].ToString());//flag_state

                    if (param.ContainsKey("email") && param["email"].ToString() != dbShop.email) ht.Add("new_email", param["email"].ToString());//email
                    if (param.ContainsKey("zipcode") && param["zipcode"].ToString() != dbShop.zipcode) ht.Add("new_zipcode", param["zipcode"].ToString());//zipcode
                    if (param.ContainsKey("address") && param["address"].ToString() != dbShop.address) ht.Add("new_address", param["address"].ToString());//address
                    if (param.ContainsKey("bz") && param["bz"].ToString() != dbShop.bz) ht.Add("new_bz", param["bz"].ToString());//bz

                    if (param.ContainsKey("id_user") && param["id_user"].ToString() != dbShop.id_edit) ht.Add("new_id_edit", param["id_user"].ToString());//id_user
                    ht.Add("new_rq_edit", DateTime.Now);//rq_edit 

                    DAL.UpdatePart(typeof(Tb_Kh), ht);

                }
                #endregion
                #region 如果原来类型非加盟店 现在改为加盟店时 
                if (param["flag_type"].ToString() == ((int)Enums.FlagShopType.加盟店).ToString() && dbShop.flag_type != (int)Enums.FlagShopType.加盟店)
                {
                    #region 如果原来类型非加盟店 现在改为加盟店时 
                    ht.Clear();
                    ht.Add("id_masteruser", dbShop.id_masteruser);
                    ht.Add("id_shop_relate_not_null", "1");
                    var ydbKhList = DAL.QueryList<Tb_Kh>(typeof(Tb_Kh), ht);
                    var ybdKhModel = ydbKhList.FirstOrDefault();
                    if (ybdKhModel != null && !string.IsNullOrEmpty(ybdKhModel.id))
                    {
                        if (ydbKhList.Where(d => d.id_shop_relate == dbShop.id).Count() > 0)
                        {
                            #region 如果客户表之前绑定过此门店 更新此门店id_kh为此客户id
                            //如果客户表之前绑定过此门店 更新此门店id_kh为此客户id
                            var ybdKhOldModel = ydbKhList.Where(d => d.id_shop_relate == dbShop.id).FirstOrDefault();
                            #region 更新客户状态为启用
                            ht.Clear();
                            ht.Add("id_masteruser", dbShop.id_masteruser);
                            ht.Add("id", ybdKhOldModel.id);
                            ht.Add("new_flag_state", (byte)Enums.TbShopFlagState.Opened);

                            if (param.ContainsKey("bm") && param["bm"].ToString() != ybdKhOldModel.bm) ht.Add("new_bm", param["bm"].ToString());//bm
                            if (param.ContainsKey("mc") && param["mc"].ToString() != ybdKhOldModel.mc) ht.Add("new_mc", param["mc"].ToString());//mc
                            if (param.ContainsKey("lxr") && param["lxr"].ToString() != ybdKhOldModel.lxr) ht.Add("new_lxr", param["lxr"].ToString());//lxr
                            if (param.ContainsKey("phone") && param["phone"].ToString() != ybdKhOldModel.tel) ht.Add("new_tel", param["phone"].ToString());//phone
                            if (param.ContainsKey("tel") && param["tel"].ToString() != ybdKhOldModel.companytel) ht.Add("new_companytel", param["tel"].ToString());//tel

                            if (param.ContainsKey("email") && param["email"].ToString() != ybdKhOldModel.email) ht.Add("new_email", param["email"].ToString());//email
                            if (param.ContainsKey("zipcode") && param["zipcode"].ToString() != ybdKhOldModel.zipcode) ht.Add("new_zipcode", param["zipcode"].ToString());//zipcode
                            if (param.ContainsKey("address") && param["address"].ToString() != ybdKhOldModel.address) ht.Add("new_address", param["address"].ToString());//address
                            if (param.ContainsKey("bz") && param["bz"].ToString() != ybdKhOldModel.bz) ht.Add("new_bz", param["bz"].ToString());//bz


                            DAL.UpdatePart(typeof(Tb_Kh), ht);
                            #endregion
                            #region 更新门店绑定客户
                            ht.Clear();
                            ht.Add("id_masteruser", dbShop.id_masteruser);
                            ht.Add("id", dbShop.id);
                            ht.Add("new_id_kh", ybdKhOldModel.id);
                            if (DAL.UpdatePart(typeof(Tb_Shop), ht) <= 0)
                            {
                                br.Message.Clear();
                                br.Message.Add(String.Format("操作失败更新门店客户信息失败 请重试！"));
                                br.Success = false;
                                br.Level = ErrorLevel.Warning;
                                throw new CySoftException(br);
                            }
                            #endregion
                            #endregion
                        }
                        else
                        {
                            #region 新增客户
                            Tb_Kh addKhModel = new Tb_Kh();
                            addKhModel.id_masteruser = dbShop.id_masteruser;
                            addKhModel.id = GetGuid;
                            addKhModel.bm = param["bm"].ToString();
                            addKhModel.mc = param["mc"].ToString();
                            addKhModel.id_khfl = ybdKhModel.id;
                            addKhModel.companytel = param["tel"].ToString();
                            addKhModel.zjm = CySoft.Utility.PinYin.GetChineseSpell(addKhModel.mc);
                            addKhModel.tel = param["phone"].ToString();
                            addKhModel.lxr = param["lxr"].ToString();
                            addKhModel.email = param["email"].ToString();
                            addKhModel.zipcode = param["zipcode"].ToString();
                            addKhModel.address = param["address"].ToString();
                            addKhModel.je_xyed = 0;
                            addKhModel.je_xyed_temp = 0;
                            addKhModel.id_shop_relate = dbShop.id;
                            addKhModel.flag_state = (byte)Enums.TbShopFlagState.Opened;
                            addKhModel.flag_delete = (int)Enums.FlagDelete.NoDelete;
                            addKhModel.bz = param["bz"].ToString();
                            addKhModel.id_create = dbShop.id_create;
                            addKhModel.rq_create = dbShop.rq_create;
                            DAL.Add(addKhModel);
                            #endregion
                            #region 更新门店绑定客户
                            ht.Clear();
                            ht.Add("id_masteruser", dbShop.id_masteruser);
                            ht.Add("id", dbShop.id);
                            ht.Add("new_id_kh", addKhModel.id);
                            if (DAL.UpdatePart(typeof(Tb_Shop), ht) <= 0)
                            {
                                br.Message.Clear();
                                br.Message.Add(String.Format("操作失败更新门店客户信息失败 请重试！"));
                                br.Success = false;
                                br.Level = ErrorLevel.Warning;
                                throw new CySoftException(br);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region 新增客户分类
                        Tb_Khfl addKhflModel = new Tb_Khfl();
                        addKhflModel.id_masteruser = dbShop.id_masteruser;
                        addKhflModel.id = GetGuid;
                        addKhflModel.bm = "";
                        addKhflModel.mc = Enums.FlagShopType.加盟店.ToString();
                        addKhflModel.path = "/0/" + addKhflModel.id;
                        addKhflModel.id_farther = "0";
                        addKhflModel.sort_id = 1;
                        addKhflModel.id_create = dbShop.id_create;
                        addKhflModel.rq_create = dbShop.rq_create;
                        addKhflModel.flag_delete = (int)Enums.FlagDelete.NoDelete;
                        DAL.Add(addKhflModel);
                        #endregion
                        #region 新增客户
                        Tb_Kh addKhModel = new Tb_Kh();
                        addKhModel.id_masteruser = dbShop.id_masteruser;
                        addKhModel.id = GetGuid;
                        addKhModel.bm = param["bm"].ToString();
                        addKhModel.mc = param["mc"].ToString();
                        addKhModel.id_khfl = addKhflModel.id;
                        addKhModel.companytel = param["tel"].ToString();
                        addKhModel.zjm = CySoft.Utility.PinYin.GetChineseSpell(addKhModel.mc);
                        addKhModel.tel = param["phone"].ToString();
                        addKhModel.lxr = param["lxr"].ToString();
                        addKhModel.email = param["email"].ToString();
                        addKhModel.zipcode = param["zipcode"].ToString();
                        addKhModel.address = param["address"].ToString();
                        addKhModel.je_xyed = 0;
                        addKhModel.je_xyed_temp = 0;
                        addKhModel.id_shop_relate = dbShop.id;
                        addKhModel.flag_state = (byte)Enums.TbShopFlagState.Opened;
                        addKhModel.flag_delete = (int)Enums.FlagDelete.NoDelete;
                        addKhModel.bz = param["bz"].ToString();
                        addKhModel.id_create = dbShop.id_create;
                        addKhModel.rq_create = dbShop.rq_create;
                        DAL.Add(addKhModel);
                        #endregion
                        #region 更新门店绑定客户
                        ht.Clear();
                        ht.Add("id_masteruser", dbShop.id_masteruser);
                        ht.Add("id", dbShop.id);
                        ht.Add("new_id_kh", addKhModel.id);
                        if (DAL.UpdatePart(typeof(Tb_Shop), ht) <= 0)
                        {
                            br.Message.Clear();
                            br.Message.Add(String.Format("操作失败更新门店客户信息失败 请重试！"));
                            br.Success = false;
                            br.Level = ErrorLevel.Warning;
                            throw new CySoftException(br);
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
                #region 如果原来类型加盟店 现在改为非加盟店时 
                else if (param["flag_type"].ToString() != ((int)Enums.FlagShopType.加盟店).ToString() && dbShop.flag_type == (int)Enums.FlagShopType.加盟店)
                {
                    #region 更新客户解绑门店
                    ht.Clear();
                    ht.Add("id_masteruser", dbShop.id_masteruser);
                    ht.Add("id", dbShop.id_kh);
                    ht.Add("new_flag_state", (byte)Enums.TbShopFlagState.Closed);
                    if (DAL.UpdatePart(typeof(Tb_Kh), ht) <= 0)
                    {
                        br.Message.Clear();
                        br.Message.Add(String.Format("更新客户停用失败 请重试！"));
                        br.Success = false;
                        br.Level = ErrorLevel.Warning;
                        throw new CySoftException(br);
                    }
                    #endregion
                    #region 更新门店解绑客户
                    //ht.Clear();
                    //ht.Add("id_masteruser", dbShop.id_masteruser);
                    //ht.Add("id", dbShop.id);
                    //ht.Add("new_id_kh", "");
                    //if (DAL.UpdatePart(typeof(Tb_Shop), ht) <= 0)
                    //{
                    //    br.Message.Clear();
                    //    br.Message.Add(String.Format("更新门店解绑客户失败 请重试！"));
                    //    br.Success = false;
                    //    br.Level = ErrorLevel.Warning;
                    //    throw new CySoftException(br);
                    //}
                    #endregion
                }
                #endregion
                #region 开启的门店至少留一个
                //获取现在开启的门店
                ht.Clear();
                ht.Add("id_masteruser", dbShop.id_masteruser);
                ht.Add("flag_state", (byte)Enums.TbShopFlagState.Opened);
                ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
                var dbCount = DAL.GetCount(typeof(Tb_Shop), ht);


                //开启的门店至少留一个
                if (dbCount == 0)
                {
                    br.Message.Clear();
                    br.Message.Add(String.Format("操作失败 开启的门店至少留一个！"));
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    throw new CySoftException(br);
                }

                #endregion
                #region 如果是由关闭改为开启状态 则判断购买数是否够用
                //如果是由关闭改为开启状态 则判断购买数是否够用
                if (dbShop.flag_state == (byte)Enums.TbShopFlagState.Closed && param.ContainsKey("flag_state") && param["flag_state"].ToString() == ((byte)Enums.TbShopFlagState.Opened).ToString())
                {
                    #region 验证是否已经达到购买的数量
                    if (PublicSign.flagCheckService == "1")
                    {
                        #region 缓存中取服务数据
                        string bm = GetServiceBM(param["version"].ToString());
                        if (string.IsNullOrEmpty(bm))
                        {
                            br.Message.Clear();
                            br.Message.Add(String.Format("操作失败 获取服务编码异常 请检查版本是否正常！"));
                            br.Success = false;
                            br.Level = ErrorLevel.Warning;
                            throw new CySoftException(br);
                        }
                        ht.Clear();
                        ht.Add("id_cyuser", param["id_cyuser"].ToString());
                        ht.Add("bm", bm);
                        ht.Add("service", "GetService");
                        ht.Add("id_masteruser", param["id_masteruser"].ToString());
                        ht.Add("rq_create_master_shop", param["rq_create_master_shop"].ToString());
                        var cyServiceHas = GetCYService(ht);
                        #endregion
                        #region 检验处理
                        br = CheckServiceForEdit(cyServiceHas, dbCount, bm);
                        if (!br.Success)
                        {
                            #region 调接口重新查询
                            ht.Clear();
                            ht.Add("id_cyuser", param["id_cyuser"].ToString());
                            ht.Add("bm", bm);
                            ht.Add("service", "GetService");
                            ht.Add("id_masteruser", param["id_masteruser"].ToString());
                            ht.Add("do_post", "1");
                            ht.Add("rq_create_master_shop", param["rq_create_master_shop"].ToString());
                            cyServiceHas = GetCYService(ht);
                            br = CheckServiceForEdit(cyServiceHas, dbCount, bm);
                            if (!br.Success)
                            {
                                throw new CySoftException(br);
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
                br.Success = true;
                br.Message.Add("更新操作成功.");
            }
            #endregion
            #endregion

            return br;
        }


        #region 修改门店状态的服务检查
        public BaseResult CheckServiceForEdit(Hashtable cyServiceHas, int dbCount, string bm)
        {
            BaseResult br = new BaseResult() { Success = true };

            if (cyServiceHas == null || !cyServiceHas.ContainsKey("cyServiceList"))
            {
                br.Message.Clear();
                br.Message.Add(String.Format("获取购买服务信息失败 请重试！"));
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                return br;
            }

            var cyServiceList = (List<Schedule_UserService>)cyServiceHas["cyServiceList"];

            cyServiceList = cyServiceList.Where(d => d.flag_stop == (int)Enums.CYServiceStopFlag.Used && d.bm == bm).ToList();

            if (cyServiceList == null || cyServiceList.Count() <= 0)
            {
                //如果没购买过 查看是否在试用期内
                var endtime = DateTime.Parse(cyServiceHas["endTime"].ToString());
                if (endtime < DateTime.Now)
                {
                    //如果过期了 没有购买过 就算一个
                    br.Message.Clear();
                    br.Message.Add(String.Format("已经超过试用期 不允许启用门店 请购买服务！"));
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    return br;
                }
                ////如果没有购买过 就算一个
                //if (dbCount > 1)
                //{
                //    br.Message.Clear();
                //    br.Message.Add(String.Format("已经达到购买服务的数量 [1] 不允许启用门店 请购买服务数量！"));
                //    br.Success = false;
                //    br.Level = ErrorLevel.Warning;
                //    return br;
                //}
            }
            else
            {
                var cyServiceModel = cyServiceList.FirstOrDefault();
                if (cyServiceModel.rq_begin != null && cyServiceModel.rq_begin > DateTime.Parse("1900-1-1 0:00:00") && cyServiceModel.rq_begin > DateTime.Now)
                {
                    //过期了 就只能算一个
                    if (dbCount > 1)
                    {
                        br.Message.Clear();
                        br.Message.Add(String.Format("已经达到购买服务的数量 [1] 不允许启用门店 请购买服务数量！"));
                        br.Success = false;
                        br.Level = ErrorLevel.Warning;
                        return br;
                    }
                }

                if (cyServiceModel.rq_end != null && cyServiceModel.rq_end > DateTime.Parse("1900-1-1 0:00:00") && cyServiceModel.rq_end < DateTime.Now)
                {
                    //过期了 就只能算一个
                    if (dbCount > 1)
                    {
                        br.Message.Clear();
                        br.Message.Add(String.Format("已经达到购买服务的数量 [1] 不允许启用门店 请购买服务数量！"));
                        br.Success = false;
                        br.Level = ErrorLevel.Warning;
                        return br;
                    }
                }

                //没过期 验购买的数量
                if (dbCount > cyServiceList.FirstOrDefault().sl && cyServiceList.FirstOrDefault().sl > 0)
                {
                    br.Message.Clear();
                    br.Message.Add(String.Format("已经达到购买服务的数量 [{0}] 不允许启用门店 请购买服务数量！", cyServiceList.FirstOrDefault().sl));
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    return br;
                    //throw new CySoftException(br);
                }
            }

            return br;
        }
        #endregion

        #region 修改门店状态的服务检查
        public BaseResult CheckServiceForAdd(Hashtable cyServiceHas, int dbCount, string bm)
        {
            BaseResult br = new BaseResult() { Success = true };

            if (cyServiceHas == null || !cyServiceHas.ContainsKey("cyServiceList"))
            {
                br.Message.Clear();
                br.Message.Add(String.Format("获取购买服务信息失败 请重试！"));
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                return br;
            }

            var cyServiceList = (List<Schedule_UserService>)cyServiceHas["cyServiceList"];

            cyServiceList = cyServiceList.Where(d => d.flag_stop == (int)Enums.CYServiceStopFlag.Used && d.bm == bm).ToList();

            if (cyServiceList.Count() <= 0)
            {
                //如果没购买过 查看是否在试用期内
                var endtime = DateTime.Parse(cyServiceHas["endTime"].ToString());
                if (endtime < DateTime.Now)
                {
                    br.Message.Clear();
                    br.Message.Add(String.Format("已经超过试用期 不允许新增门店 请购买服务！"));
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    return br;
                }
            }
            else
            {
                var cyServiceModel = cyServiceList.FirstOrDefault();
                if (cyServiceModel.rq_begin != null && cyServiceModel.rq_begin > DateTime.Parse("1900-1-1 0:00:00") && cyServiceModel.rq_begin > DateTime.Now)
                {
                    br.Message.Clear();
                    br.Message.Add(String.Format("您还未购买服务信息或购买信息已经失效 不允许操作！"));
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    return br;
                }

                if (cyServiceModel.rq_end != null && cyServiceModel.rq_end > DateTime.Parse("1900-1-1 0:00:00") && cyServiceModel.rq_end < DateTime.Now)
                {
                    br.Message.Clear();
                    br.Message.Add(String.Format("您还未购买服务信息或购买信息已经失效 不允许操作！"));
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    return br;
                }

                //没过期 验购买的数量
                if (dbCount > cyServiceList.FirstOrDefault().sl && cyServiceList.FirstOrDefault().sl > 0)
                {
                    br.Message.Clear();
                    br.Message.Add(String.Format("已经达到购买服务的数量 [{0}] 不允许新增门店 请购买服务！", cyServiceList.FirstOrDefault().sl));
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    return br;
                }
            }

            return br;
        }
        #endregion

        #region GetAll
        public override BaseResult GetAll(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            res.Data = DAL.QueryList<Tb_Shop>(typeof(Tb_Shop), param);
            return res;
        }
        #endregion

        #region TurnShopList
        /// <summary>
        /// 将Hashtable转换为Model
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private Tb_Shop TurnShopList(Hashtable param)
        {
            Tb_Shop model = new Tb_Shop()
            {
                id_masteruser = param["id_masteruser"].ToString(),
                id = Guid.NewGuid().ToString(),
                bm = param["bm"].ToString(),
                mc = param["mc"].ToString(),
                email = param["email"].ToString(),
                fax = param["fax"].ToString(),
                phone = param["phone"].ToString(),
                tel = param["tel"].ToString(),
                lxr = param["lxr"].ToString(),
                rq_start = DateTime.Now,
                zipcode = param["zipcode"].ToString(),
                address = param["address"].ToString(),
                flag_state = (int)Enums.TbShopFlagState.Opened,
                bz = param["bz"].ToString(),
                id_create = param["id_user"].ToString(),
                rq_create = DateTime.Now,
                id_edit = null,
                rq_edit = null,
                flag_delete = (int)Enums.FlagDelete.NoDelete,
                id_cloneshop = param["id_cloneshop"].ToString(),
                yzm = CySoft.Utility.MD5Encrypt.Md5(param["yzm"].ToString()),
                pic_path = param["pic_path"].ToString()
            };

            if (param.ContainsKey("qq") && !string.IsNullOrEmpty(param["qq"].ToString()))
                model.qq = param["qq"].ToString();

            if (param.ContainsKey("flag_type") && !string.IsNullOrEmpty(param["flag_type"].ToString()))
                model.flag_type = CySoft.Frame.Common.TypeConvert.ToInt(param["flag_type"].ToString(), (int)Enums.FlagShopType.直营店);

            if (param.ContainsKey("kh_id") && !string.IsNullOrEmpty(param["kh_id"].ToString()))
                model.id_kh = param["kh_id"].ToString();

            if (param.ContainsKey("id_shop_ps") && !string.IsNullOrEmpty(param["id_shop_ps"].ToString()))
                model.id_shop_ps = param["id_shop_ps"].ToString();

            return model;
        }
        #endregion

        #region QueryShopSelectModels
        public BaseResult QueryShopSelectModels(Hashtable param)
        {
            BaseResult res = new BaseResult();
            res.Data = Tb_ShopDAL.QueryShopSelectModels(typeof(Tb_Shop), param);
            return res;
        }
        #endregion

        #region 服务接口调用方法
        public BaseResult GetPosShopInfo(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            var model = Tb_ShopDAL.GetPosShopInfo(typeof(Tb_Shop), param);
            if (model != null)
            {
                param.Clear();
                param.Add("parmcode", "ediservice");
                var paramTerrace = DAL.GetItem<Ts_Parm_Terrace>(typeof(Ts_Parm_Terrace), param);
                if (paramTerrace != null)
                {
                    model.data_url = paramTerrace.parmvalue;
                }
            }
            res.Data = model;
            return res;
        }
        #endregion

        #region GetUserShopCount
        public int GetUserShopCount(Hashtable param)
        {
            var dbCount = DAL.GetCount(typeof(Tb_Shop), param);
            return dbCount;
        }
        #endregion

        #region CloseShopWithOutMaster
        public BaseResult CloseShopWithOutMaster(Hashtable param)
        {
            BaseResult res = new BaseResult();
            res.Data = Tb_ShopDAL.CloseShopWithOutMaster(typeof(Tb_Shop), param);
            return res;
        }
        #endregion

        #region ResetOpenShop
        [Transaction]
        public BaseResult ResetOpenShop(Hashtable param)
        {
            //ht.Add("id_masteruser", id_user_master);
            //ht.Add("not_id_shop", id_shop_master);
            //ht.Add("opened_ids", ids);
            //ht.Add("allow_number", allowNum);

            BaseResult res = new BaseResult();
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("not_id_shop", param["not_id_shop"].ToString());
            Tb_ShopDAL.CloseShopWithOutMaster(typeof(Tb_Shop), param);
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("opened_ids", param["opened_ids"].ToString().Split(',').ToArray());
            Tb_ShopDAL.ResetOpenShop(typeof(Tb_Shop), ht);
            int allowNum = int.Parse(param["allow_number"].ToString());
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("flag_state", (byte)Enums.TbShopFlagState.Opened);
            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            var dbCount = DAL.GetCount(typeof(Tb_Shop), ht);
            if (dbCount > allowNum && allowNum > 0)
            {
                res.Success = false;
                res.Message.Add("已超过购买的服务数量 目前已购买了 [" + allowNum + "] 个 如果又多购买过 请重新登录后刷新重试！");
                throw new CySoftException(res);
            }


            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("opened_id_shop_relate_ids", param["opened_ids"].ToString().Split(',').ToArray());
            ht.Add("new_flag_state", (byte)Enums.TbShopFlagState.Opened);
            DAL.UpdatePart(typeof(Tb_Kh), ht);


            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("not_id_shop_relate_ids", param["opened_ids"].ToString().Split(',').ToArray());
            ht.Add("id_shop_relate_not_null", "1");
            ht.Add("new_flag_state", (byte)Enums.TbShopFlagState.Closed);
            DAL.UpdatePart(typeof(Tb_Kh), ht);


            res.Success = true;
            return res;

        }
        #endregion

        #region QueryShopListWithFatherId
        public BaseResult QueryShopListWithFatherId(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            res.Data = Tb_ShopDAL.QueryShopListWithFatherId(typeof(Tb_Shop), param).ToList();
            return res;
        }
        #endregion

        #region ResetYZM
        public BaseResult ResetYZM(Hashtable param)
        {
            BaseResult br = new BaseResult();

            if (param == null
                || !param.ContainsKey("id")
                || string.IsNullOrEmpty("id")
                || !param.ContainsKey("new_yzm")
                || string.IsNullOrEmpty("new_yzm")
                )
            {
                br.Message.Add("操作失败 参数不符合要求！");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }

            Hashtable ht = new Hashtable();
            ht.Add("id", param["id"].ToString());
            var dbShop = DAL.GetItem<Tb_Shop>(typeof(Tb_Shop), ht);
            if (dbShop == null) { br.Success = false; br.Message.Add("门店不存在"); br.Level = ErrorLevel.Warning; return br; }
            if (dbShop.flag_delete == (byte)Enums.FlagDelete.Deleted) { br.Success = false; br.Message.Add("门店已被删除"); br.Level = ErrorLevel.Warning; return br; }

            ht.Clear();
            ht.Add("id_masteruser", dbShop.id_masteruser);
            ht.Add("yzm", CySoft.Utility.MD5Encrypt.Md5(param["new_yzm"].ToString()));
            ht.Add("not_id", param["id"].ToString());
            var dbYZMModel = DAL.GetItem<Tb_Shop>(typeof(Tb_Shop), ht);
            if (dbYZMModel != null && !string.IsNullOrEmpty(dbYZMModel.id))
            {
                br.Message.Add(String.Format("验证码：{0} 重复 ,请核实！", param["new_yzm"].ToString()));
                br.Success = false;
                return br;
            }

           var new_yzm_md5 = CySoft.Utility.MD5Encrypt.Md5(param["new_yzm"].ToString());

            ht.Clear();
            ht.Add("id", param["id"].ToString());
            ht.Add("new_yzm", new_yzm_md5);
            ht.Add("new_rq_edit", DateTime.Now);
            DAL.UpdatePart(typeof(Tb_Shop), ht);

            br.Message.Add(String.Format("重置验证码。信息：门店流水:{0}，门店名:{1}", dbShop.id, dbShop.mc));
            br.Success = true;
            return br;
        }
        #endregion


    }
}
