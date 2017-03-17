using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.BLL.MemberBLL
{
    public class Tb_Hy_ShopBLL : BaseBLL
    {
        #region 获取分页数据
        /// <summary>
        /// 获取分页数据
        /// lz
        /// 2016-09-18
        /// </summary>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Tb_Hy_Shop), param);
            if (totalCount > 0)
            {
                var data = DAL.QueryPage<Tb_Hy_Shop_Query>(typeof(Tb_Hy_Shop), param);
                if (data != null && data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        if (!string.IsNullOrEmpty(item.hy_hysr) && item.hy_hysr.Length == 4)
                        {
                            item.hy_hysr_cn = item.hy_hysr.Substring(0, 2) + " 月 " + item.hy_hysr.Substring(2, 2) + " 日 ";
                        }
                    }

                }
                pn.Data = data;
                pn.TotalCount = totalCount;
            }
            return pn;
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// lz
        /// 2016-09-18
        /// </summary>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            #region 获取数据
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            var tb_Hy = this.TurnTb_HyModel(param);
            #endregion

            #region 验证是否重复

            if (!string.IsNullOrEmpty(param["membercard"].ToString()))
            {
                ht.Clear();
                ht.Add("id_masteruser", tb_Hy.id_masteruser);
                ht.Add("membercard", param["membercard"].ToString());
                ht.Add("hy_flag_delete", (int)Enums.FlagDelete.NoDelete);
                if (DAL.GetCount(typeof(Tb_Hy_Shop), ht) > 0)
                {
                    br.Success = false;
                    br.Message.Add("输入会员卡已存在!");
                    return br;
                }
            }

            if (!string.IsNullOrEmpty(tb_Hy.phone))
            {
                ht.Clear();
                ht.Add("id_masteruser", tb_Hy.id_masteruser);
                ht.Add("phone", tb_Hy.phone);
                ht.Add("hy_flag_delete", (int)Enums.FlagDelete.NoDelete);
                if (DAL.GetCount(typeof(Tb_Hy_Shop), ht) > 0)
                {
                    br.Success = false;
                    br.Message.Add("输入手机已存在!");
                    return br;
                }
            }
            #endregion

            #region 构建Model
            decimal zk = 0;
            decimal.TryParse(param["zk"].ToString(), out zk);

            byte flag_yhlx = 1;
            byte.TryParse(param["flag_yhlx"].ToString(), out flag_yhlx);

            var tb_Hy_Shop = new Tb_Hy_Shop()
            {
                id = Guid.NewGuid().ToString(),
                id_masteruser = tb_Hy.id_masteruser,
                id_shop = param["id_shop"].ToString(),
                id_hy = tb_Hy.id,
                membercard = param["membercard"].ToString(),
                phone = tb_Hy.phone,
                id_hyfl = param["id_hyfl"].ToString(),
                flag_yhlx = flag_yhlx,//优惠类型
                zk = zk,//折扣
                rq_b = DateTime.Parse(param["rq_b"].ToString()),
                rq_e = DateTime.Parse(param["rq_e"].ToString()),
                flag_stop = (int)Enums.FlagStop.Start,
                rq_create = DateTime.Now
            };
            #endregion

            #region 插入数据库
            DAL.Add(tb_Hy);
            DAL.Add(tb_Hy_Shop);
            #endregion

            #region 返回
            br.Message.Add(String.Format("新增成功。流水号：{0}", tb_Hy.id));
            br.Success = true;
            br.Data = tb_Hy.id;
            return br;
            #endregion
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// lz
        /// 2016-09-20
        /// </summary>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            #region 获取数据
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();

            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("id", param["id"].ToString());
            var dbTb_Hy_Shop = DAL.GetItem<Tb_Hy_Shop_Query>(typeof(Tb_Hy_Shop), ht);
            if (dbTb_Hy_Shop == null)
            {
                br.Success = false;
                br.Message.Add("数据出错 未查询到会员关系信息 请重试!");
                return br;
            }

            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("id", dbTb_Hy_Shop.id_hy);
            var dbTb_Hy = DAL.GetItem<Tb_Hy>(typeof(Tb_Hy), ht);
            if (dbTb_Hy == null)
            {
                br.Success = false;
                br.Message.Add("数据出错 未查询到会员信息 请重试!");
                return br;
            }

            #endregion

            #region 验证是否重复
            if (!string.IsNullOrEmpty(param["membercard"].ToString()))
            {
                ht.Clear();
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("membercard", param["membercard"].ToString());
                ht.Add("hy_flag_delete", (int)Enums.FlagDelete.NoDelete);
                ht.Add("not_id", param["id"].ToString());
                if (DAL.GetCount(typeof(Tb_Hy_Shop), ht) > 0)
                {
                    br.Success = false;
                    br.Message.Add("输入会员卡已存在!");
                    return br;
                }
            }

            if (!string.IsNullOrEmpty(param["phone"].ToString()))
            {
                ht.Clear();
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("phone", param["phone"].ToString());
                ht.Add("hy_flag_delete", (int)Enums.FlagDelete.NoDelete);
                ht.Add("not_id", param["id"].ToString());
                if (DAL.GetCount(typeof(Tb_Hy_Shop), ht) > 0)
                {
                    br.Success = false;
                    br.Message.Add("输入手机已存在!");
                    return br;
                }
            }
            #endregion

            #region 更新Tb_Hy_Shop


            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("id", param["id"].ToString());
            ht.Add("new_id_shop", param["id_shop"].ToString());
            ht.Add("new_membercard", param["membercard"].ToString());
            ht.Add("new_phone", param["phone"].ToString());
            ht.Add("new_id_hyfl", param["id_hyfl"].ToString());

            byte flag_yhlx =1;
            byte.TryParse(param["flag_yhlx"].ToString(), out flag_yhlx);

            ht.Add("new_flag_yhlx", flag_yhlx);//优惠类型
            decimal zk = 0;
            decimal.TryParse(param["zk"].ToString(), out zk);
            ht.Add("new_zk", zk);//折扣
            ht.Add("new_rq_b", param["rq_b"].ToString());
            ht.Add("new_rq_e", param["rq_e"].ToString());
            DAL.UpdatePart(typeof(Tb_Hy_Shop), ht);
            #endregion

            #region 更新Tb_Hy
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("id", dbTb_Hy_Shop.id_hy);
            ht.Add("new_name", param["name"].ToString());
            ht.Add("new_qq", param["qq"].ToString());
            ht.Add("new_email", param["email"].ToString());
            ht.Add("new_phone", param["phone"].ToString());
            ht.Add("new_tel", param["tel"].ToString());
            ht.Add("new_address", param["address"].ToString());
            ht.Add("new_MMno", param["MMno"].ToString());
            ht.Add("new_zipcode", param["zipcode"].ToString());

            if(param.ContainsKey("password")&&!string.IsNullOrEmpty(param["password"].ToString()))
                ht.Add("new_password", Utility.MD5Encrypt.Md5(param["password"].ToString()));
           

            if (!string.IsNullOrEmpty(param["birthday"].ToString()))
            {
                ht.Add("new_birthday", DateTime.Parse(param["birthday"].ToString()));
                ht.Add("new_hysr", (DateTime.Parse(param["birthday"].ToString())).ToString("MMdd"));
            }
            else
            {
                if (param.ContainsKey("hysr") && !string.IsNullOrEmpty(param["hysr"].ToString()))
                {
                    if (param["hysr"].ToString() == "0000")
                        ht.Add("new_hysr", "");
                    else
                        ht.Add("new_hysr", param["hysr"].ToString());
                }
            }
            byte flag_nl = 0;
            byte.TryParse(param["flag_nl"].ToString(), out flag_nl);
            ht.Add("new_flag_nl", flag_nl);
            ht.Add("new_id_shop_create", param["id_shop_create"].ToString());
            ht.Add("new_rq_edit", DateTime.Now);
            ht.Add("new_id_edit", param["id_user"].ToString());

            byte flag_sex = 1;
            byte.TryParse(param["flag_sex"].ToString(), out flag_sex);
            ht.Add("new_flag_sex", flag_sex);

            DAL.UpdatePart(typeof(Tb_Hy), ht);
            #endregion

            #region 返回
            br.Message.Add(String.Format("更新成功。"));
            br.Success = true;
            return br;
            #endregion
        }
        #endregion

        #region 删除
        /// <summary>
        ///删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            #region 参数验证
            BaseResult result = new BaseResult() { Success = true };
            if (param == null || param.Count < 2)
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            var id = param["id"].ToString();
            var id_masteruser = param["id_masteruser"].ToString();
            if (string.IsNullOrEmpty(id))
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            if (string.IsNullOrEmpty(id_masteruser))
            {
                result.Success = false;
                result.Message.Add("请登录!");
                return result;
            }

            //获取会员门店关系信息
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("id", param["id"].ToString());
            var dbTb_Hy_Shop = DAL.GetItem<Tb_Hy_Shop_Query>(typeof(Tb_Hy_Shop), ht);
            if (dbTb_Hy_Shop == null)
            {
                result.Success = false;
                result.Message.Add("数据出错 未查询到会员关系信息 请重试!");
                return result;
            }

            #endregion
            #region 更新数据

            ht.Clear();
            ht.Add("id", dbTb_Hy_Shop.id_hy);
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);




            try
            {
                if (DAL.UpdatePart(typeof(Tb_Hy), ht) <= 0)
                {
                    result.Success = false;
                    result.Message.Add("操作数据库失败!");
                    return result;
                }
                else
                {
                    ht.Clear();
                    ht.Add("id_hy", dbTb_Hy_Shop.id_hy);
                    ht.Add("id_masteruser", id_masteruser);
                    ht.Add("new_flag_stop", (int)Enums.FlagStop.Stopped);
                    if (DAL.UpdatePart(typeof(Tb_Hy_Shop), ht) <= 0)
                    {
                        result.Success = false;
                        result.Message.Add(string.Format("操作数据库失败!"));
                        throw new CySoftException(result);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message.Add("操作异常!");
            }
            #endregion
            return result;
        }
        #endregion

        #region 获取单条数据
        /// <summary>
        /// 获取单条数据
        /// lz
        /// 2016-09-20
        /// </summary>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            try
            {
                res.Data = DAL.GetItem<Tb_Hy_Shop_Query>(typeof(Tb_Hy_Shop), param);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message.Add("操作异常!");
            }
            return res;
        }

        #endregion

        #region TurnTb_HyModel
        /// <summary>
        /// 将Hashtable转换为Model
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private Tb_Hy TurnTb_HyModel(Hashtable param)
        {
            Tb_Hy model = new Tb_Hy();
            model.id_masteruser = param["id_masteruser"].ToString();
            model.id = Guid.NewGuid().ToString();
            model.name = param["name"].ToString();
            model.qq = param["qq"].ToString();
            model.email = param["email"].ToString();
            model.phone = param["phone"].ToString();
            model.tel = param["tel"].ToString();
            model.address = param["address"].ToString();
            model.MMno = param["MMno"].ToString();
            model.zipcode = param["zipcode"].ToString();
            model.zipcode = param["zipcode"].ToString();
            if (!string.IsNullOrEmpty(param["birthday"].ToString()))
            {
                model.birthday = DateTime.Parse(param["birthday"].ToString());
                model.hysr = ((DateTime)model.birthday).ToString("MMdd");
            }
            else
            {
                if (param.ContainsKey("hysr") && !string.IsNullOrEmpty(param["hysr"].ToString()))
                {
                    if (param["hysr"].ToString() == "0000")
                        model.hysr = "";
                    else
                        model.hysr = param["hysr"].ToString();
                }
            }
            byte flag_nl = 0;
            byte.TryParse(param["flag_nl"].ToString(), out flag_nl);
            model.flag_nl = flag_nl;
            model.id_shop_create = param["id_shop_create"].ToString();
            model.id_create = Guid.NewGuid().ToString();
            model.rq_create = DateTime.Now;
            model.flag_delete = (byte)Enums.FlagDelete.NoDelete;

            byte flag_sex = 1;
            byte.TryParse(param["flag_sex"].ToString(), out flag_sex);
            model.flag_sex = flag_sex;

            if (param["password"] == null || string.IsNullOrEmpty(param["password"].ToString()))
                model.password = Utility.MD5Encrypt.Md5("");
            else if (param["password"].ToString().Length == 32)
                model.password = param["password"].ToString();
            else
                model.password = Utility.MD5Encrypt.Md5(param["password"].ToString());

            return model;
        }
        #endregion


    }
}
