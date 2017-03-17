using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Service.Base;
using CySoft.Model;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Model.Ts;
using CySoft.Utility;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Enums;
using CySoft.Model.Tz;
using CySoft.Controllers.Filters;

//会员
namespace CySoft.Controllers.Service
{
    public class MemberServiceController : ServiceBaseController
    {
        #region 新增会员
        /// <summary>
        /// 新增会员
        /// lz
        /// 2016-09-26
        /// </summary>
        [HttpPost]
        public ActionResult Add()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop_create
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("membercard", string.Empty, HandleType.DefaultValue);//membercard
                p.Add("phone", string.Empty, HandleType.ReturnMsg);//phone
                p.Add("name", string.Empty, HandleType.ReturnMsg);//name
                p.Add("id_hyfl", string.Empty, HandleType.ReturnMsg);//id_hyfl
                p.Add("rq_b", string.Empty, HandleType.Remove);//rq_b
                p.Add("rq_e", string.Empty, HandleType.Remove);//rq_e
                p.Add("qq", string.Empty, HandleType.DefaultValue);//qq
                p.Add("email", string.Empty, HandleType.DefaultValue);//email
                p.Add("tel", string.Empty, HandleType.DefaultValue);//tel
                p.Add("address", string.Empty, HandleType.DefaultValue);//address
                p.Add("mmno", string.Empty, HandleType.DefaultValue);//mmno
                p.Add("zipcode", string.Empty, HandleType.DefaultValue);//zipcode
                p.Add("birthday", string.Empty, HandleType.DefaultValue);//birthday
                p.Add("hysr", "", HandleType.DefaultValue);//hysr

                p.Add("zk", "0.00", HandleType.Remove);//zk
                p.Add("flag_nl", "0", HandleType.DefaultValue);//flag_nl 是否农历
                p.Add("flag_sex", "1", HandleType.Remove);//flag_sex
                p.Add("password", "", HandleType.DefaultValue);//password
                p.Add("flag_yhlx", "1", HandleType.Remove);//flag_yhlx


                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }
                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                //var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);

                var ticketBr = base.GetTicketInfo(param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());

                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }

                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }

                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "membercard", "phone", "name", "id_hyfl", "qq", "email", "tel", "address", "mmno", "zipcode", "birthday", "flag_nl", "hysr", "rq_b", "rq_e", "zk", "flag_sex", "password", "flag_yhlx" });

                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 验证参数是否符合
                //验证参数是否符合
                //if (string.IsNullOrEmpty(param["phone"].ToString()) || !CyVerify.IsPhone(param["phone"].ToString()) || (param["flag_nl"].ToString() != "0" && param["flag_nl"].ToString() != "1"))
                if (string.IsNullOrEmpty(param["phone"].ToString()) || (param["flag_nl"].ToString() != "0" && param["flag_nl"].ToString() != "1"))
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0006;
                    return res;
                }
                #endregion
                #region 参数处理
                param.Add("id_shop_create", param["id_shop"].ToString());
                param.Add("MMno", param["mmno"].ToString());
                param.Remove("mmno");

                if (!param.ContainsKey("rq_b")) param.Add("rq_b", DateTime.Now.ToString("yyyy-MM-dd 00:00:01"));
                if (!param.ContainsKey("rq_e")) param.Add("rq_e", DateTime.Parse(param["rq_b"].ToString()).AddYears(1).ToString("yyyy-MM-dd 23:59:59"));
                if (!param.ContainsKey("flag_nl")) param.Add("flag_nl", "0");
                if (!param.ContainsKey("flag_sex")) param.Add("flag_sex", "1");


                if (!param.ContainsKey("zk") || !param.ContainsKey("flag_yhlx"))
                {
                    Hashtable ht_hyfl = new Hashtable();
                    ht_hyfl.Add("id", param["id_hyfl"].ToString());
                    var hyflBr = BusinessFactory.Tb_Hyfl.Get(ht_hyfl);
                    if (!hyflBr.Success || hyflBr.Data == null)
                    {
                        res.State = ServiceState.Fail;
                        res.Message = "操作失败 查询会员类别失败!";
                        return res;
                    }

                    Tb_Hyfl hyflModel = (Tb_Hyfl)hyflBr.Data;
                    if (hyflModel == null || string.IsNullOrEmpty(hyflModel.id))
                    {
                        res.State = ServiceState.Fail;
                        res.Message = "操作失败 会员类别不存在!";
                        return res;
                    }

                    if (!param.ContainsKey("zk")) param.Add("zk", hyflModel.zk);
                    if (!param.ContainsKey("flag_yhlx")) param.Add("flag_yhlx", hyflModel.flag_yhlx);

                }

                #endregion
                #region 计算生日
                if (!string.IsNullOrEmpty(param["birthday"].ToString()))
                {
                    //计算生日
                    DateTime birthday = DateTime.Parse(param["birthday"].ToString());
                    string hysr = birthday.ToString("MMdd");
                    if (param.ContainsKey("hysr"))
                        param.Remove("hysr");
                    param.Add("hysr", hysr);
                }
                else
                {
                    if (!string.IsNullOrEmpty(param["hysr"].ToString()))
                    {
                        var hysr = param["hysr"].ToString();
                        if (hysr.Length != 4)
                        {
                            res.State = ServiceState.Fail;
                            res.Message = ServiceFailCode.A0006;
                            return res;
                        }
                        else
                        {
                            var month = hysr.Substring(0, 2);
                            var day = hysr.Substring(2, 2);
                            if (!this.CheckMonthDay(month, day))
                            {
                                res.State = ServiceState.Fail;
                                res.Message = ServiceFailCode.A0006;
                                return res;
                            }
                        }
                    }
                }
                #endregion
                #region 判断是否共享的处理
                var br_Hy_ShopShare = BusinessFactory.Account.GetHy_ShopShare(param["id_shop_create"].ToString(), param["id_masteruser"].ToString()); //GetHy_ShopShare(param["id_shop_create"].ToString(), param["id_masteruser"].ToString());
                if (!br_Hy_ShopShare.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = br_Hy_ShopShare.Message.FirstOrDefault();
                    return res;
                }
                var param_Hy_ShopShare = (Hashtable)br_Hy_ShopShare.Data;
                param["id_shop"] = param_Hy_ShopShare["id_shop"].ToString();
                #endregion
                #region 新增
                var br = BusinessFactory.Tb_Hy_Shop.Add(param);
                #endregion
                #region 返回
                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = br.Data;
                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion

        #region CheckMonthDay
        private bool CheckMonthDay(string month, string day)
        {
            if (!CyVerify.IsInt32(month) || !CyVerify.IsInt32(day))
            {
                return false;
            }
            else if (int.Parse(month) <= 0 || int.Parse(month) > 12)
            {
                return false;
            }
            else if (int.Parse(day) <= 0 || int.Parse(day) > 31)
            {
                return false;
            }
            else
            {
                var intMonth = int.Parse(month);
                var intDay = int.Parse(day);
                if ((intMonth == 4 || intMonth == 6 || intMonth == 9 || intMonth == 11) && intDay > 30)
                    return false;
                if ((intMonth == 2 && intDay > 29))
                    return false;
            }
            return true;
        }
        #endregion

        #region 会员金额读取
        /// <summary>
        /// 会员金额读取
        /// lz
        /// 2016-10-18
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult JeQuery()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("id_hy", string.Empty, HandleType.ReturnMsg);//id_hy
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }
                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }

                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }

                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "id_hy" });

                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion

                #region 读取数据
                param.Remove("sign");
                var br = BusinessFactory.Tz_Hy_Je.Get(param);
                #endregion

                #region 返回

                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = "";

                if (br.Success)
                {
                    var tz_Hy_Je = (Tz_Hy_Je)br.Data;
                    res.Data = new { je_qm = tz_Hy_Je.je_qm, je_qm_zs = tz_Hy_Je.je_qm_zs };
                    return res;
                }

                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion

        #region 会员金额充值
        /// <summary>
        /// 会员金额充值
        /// lz
        /// 2016-10-19
        /// </summary>
        //[HttpPost]
        [ActionPurview(false)]
        public ActionResult JeCZ()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("id_hy", string.Empty, HandleType.ReturnMsg);//id_hy
                p.Add("rq", string.Empty, HandleType.ReturnMsg);//rq
                p.Add("je", string.Empty, HandleType.ReturnMsg);//je
                p.Add("bz", string.Empty, HandleType.DefaultValue);//bz
                p.Add("id_pay", string.Empty, HandleType.ReturnMsg);//id_pay
                p.Add("id_create", string.Empty, HandleType.ReturnMsg);//id_create
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                p.Add("dh", string.Empty, HandleType.DefaultValue);//dh
                p.Add("dh_pay", string.Empty, HandleType.DefaultValue);//dh_pay
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }

                if (!CyVerify.IsNumeric(param["je"].ToString()))
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }

                DateTime timeTemp = DateTime.Now;
                if (!DateTime.TryParse(param["rq"].ToString(), out timeTemp))
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }

                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }

                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }

                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "id_hy", "rq", "je", "bz", "id_pay", "id_create", "dh", "dh_pay" });


                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 执行操作
                param.Remove("sign");
                param.Add("Type", "CZ");
                var br = BusinessFactory.Tz_Hy_Je.Add(param);
                #endregion
                #region 返回

                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = br.Data;
                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion

        #region 会员金额消费
        /// <summary>
        /// 会员金额消费
        /// lz
        /// 2016-10-20
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult JeConsume()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("id_hy", string.Empty, HandleType.ReturnMsg);//id_hy
                p.Add("id_bill", string.Empty, HandleType.ReturnMsg);//id_bill
                p.Add("bm_djlx", string.Empty, HandleType.ReturnMsg);//bm_djlx
                p.Add("rq", string.Empty, HandleType.ReturnMsg);//rq
                p.Add("je", string.Empty, HandleType.ReturnMsg);//je
                p.Add("bz", string.Empty, HandleType.DefaultValue);//bz
                p.Add("password", string.Empty, HandleType.ReturnMsg);//password
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }

                if (!CyVerify.IsNumeric(param["je"].ToString()))
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }

                DateTime timeTemp = DateTime.Now;
                if (!DateTime.TryParse(param["rq"].ToString(), out timeTemp))
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }

                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }

                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }

                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "id_hy", "id_bill", "bm_djlx", "rq", "je", "bz", "password"});

                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 执行操作
                param.Remove("sign");
                param.Add("Type", "XF");
                var br = BusinessFactory.Tz_Hy_Je.Add(param);
                #endregion
                #region 返回

                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = br.Data;
                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion

        #region 会员积分读取
        /// <summary>
        /// 会员积分读取
        /// lz
        /// 2016-10-20
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult JfQuery()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("id_hy", string.Empty, HandleType.ReturnMsg);//id_hy
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }
                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }

                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }

                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "id_hy" });

                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 读取数据
                param.Remove("sign");
                var br = BusinessFactory.Tz_Hy_Jf.Get(param);
                #endregion
                #region 返回

                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = "";

                if (br.Success)
                {
                    var tz_Hy_Jf = (Tz_Hy_Jf)br.Data;
                    res.Data = new { jf_qm = tz_Hy_Jf.jf_qm };
                    return res;
                }

                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion

        #region 会员积分加/减
        /// <summary>
        /// 会员积分加/减
        /// lz
        /// 2016-10-21
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult JfEdit()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("id_hy", string.Empty, HandleType.ReturnMsg);//id_hy
                p.Add("id_bill", string.Empty, HandleType.ReturnMsg);//id_bill
                p.Add("bm_djlx", string.Empty, HandleType.ReturnMsg);//bm_djlx
                p.Add("rq", string.Empty, HandleType.ReturnMsg);//rq
                p.Add("jf", string.Empty, HandleType.ReturnMsg);//je
                p.Add("bz", string.Empty, HandleType.DefaultValue);//bz
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }

                if (!CyVerify.IsNumeric(param["jf"].ToString()))
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }

                DateTime timeTemp = DateTime.Now;
                if (!DateTime.TryParse(param["rq"].ToString(), out timeTemp))
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }

                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }
                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }
                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "id_hy", "id_bill", "bm_djlx", "rq", "jf", "bz" });

                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 执行操作
                param.Remove("sign");

                if (decimal.Parse(param["jf"].ToString()) > 0)
                    param.Add("Type", "Add");
                else
                    param.Add("Type", "Del");

                var br = BusinessFactory.Tz_Hy_Jf.Add(param);
                #endregion
                #region 返回

                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = br.Data;
                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion

        #region 会员修改消费密码
        /// <summary>
        /// 会员修改消费密码
        /// lz
        /// 2016-11-24
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult ChangePwd()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("id_hy", string.Empty, HandleType.ReturnMsg);//id_hy
                p.Add("old_pwd", string.Empty, HandleType.ReturnMsg);//old_pwd
                p.Add("new_pwd", string.Empty, HandleType.ReturnMsg);//new_pwd
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }
                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }

                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }

                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "id_hy", "old_pwd", "new_pwd" });

                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion

                #region 更新密码
                param.Add("id",param["id_hy"].ToString());
                param.Remove("sign");
                param.Remove("id_hy");
                var br = BusinessFactory.Tb_Hy.Update(param);
                #endregion

                #region 返回
                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = "";
                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion

        #region 会员充值赠送金额读取
        /// <summary>
        /// 会员充值赠送金额读取
        /// lz
        /// 2016-12-12
        /// </summary>
        //[HttpPost]
        [ActionPurview(false)]
        public ActionResult CZZSJeQuery()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("id_hy", string.Empty, HandleType.ReturnMsg);//id_hy
                p.Add("je", string.Empty, HandleType.ReturnMsg);//je
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }
                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }

                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }

                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "id_hy", "je" });

                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion

                #region 读取数据
                param.Remove("sign");
                var br = BusinessFactory.Tz_Hy_Je.Init(param);
                #endregion

                #region 返回

                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = "";

                if (br.Success)
                {
                    res.Data = br.Data;
                    return res;
                }

                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion

        #region 会员验证
        /// <summary>
        /// 会员验证
        /// lz
        /// 2016-12-15
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult CheckHY()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("id_hy", string.Empty, HandleType.ReturnMsg);//id_hy
                p.Add("password", string.Empty, HandleType.ReturnMsg);//password
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }

                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }

                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }

                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "id_hy", "password" });


                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 执行操作
                param.Remove("sign");
                param.Add("Type", "XF");
               
                var br = BusinessFactory.Tz_Hy_Je.CheckStock(param);
                #endregion
                #region 返回

                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = br.Data;
                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion

        #region 积分兑换商品查询
        /// <summary>
        /// 积分兑换商品查询
        /// lz
        /// 2016-12-15
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult JfConvertSpQuery()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("id_hy", string.Empty, HandleType.ReturnMsg);//id_hy
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }

                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }

                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }

                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "id_hy", "password" });


                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 执行操作
                param.Remove("sign");
                var br = BusinessFactory.Tb_Hy_Jfconvertsp.Active(param);
                #endregion
                #region 返回

                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = br.Data;
                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion


    }
}
