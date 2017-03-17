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
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Model.Ts;
using CySoft.Utility;
using CySoft.Frame.Core;
using CySoft.Controllers.Filters;

namespace CySoft.Controllers.Service
{
    public class SystemServiceController : ServiceBaseController
    {
        /// <summary>
        /// 主用户登录
        /// 2016-9-14 修改LD
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult ShopRegister()
        {
            var sr = RequestResult(res =>
            {
                if (CheckParamNull(res).State == ServiceState.Fail)
                {
                    return res;
                }
                Hashtable ht = new Hashtable();
                ht.Add("username", base.GetParameter("username"));
                ht.Add("id_masteruser", "0");
                IDictionary<string, string> param = base.GetParameters(new string[] { "username", "yzm" });
                //WriteDBLog(LogFlag.Bill, new List<string>() { "接口SystemService/ShopRegister,username:" + param["username"] });
                var userModel = BusinessFactory.Account.ShopRegister(ht).Data as Tb_User;
                if (userModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = "主帐号或企业号错误!";
                    return res;
                }

                var validSign = SignUtils.SignRequest(param, "CY2016");
                var sign = base.GetParameter("sign");
                //WriteDBLog(LogFlag.Bill, new List<string>() { "接口SystemService/ShopRegister,sign:" + sign + ";服务器生成sign:" + validSign + ";username:" + param["username"] });
                if (sign != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = "数据签名验证失败!";
                    return res;
                }
                ht.Clear();
                ht.Add("username", base.GetParameter("username"));
                ht.Add("yzm", base.GetParameter("yzm"));
                var br = BusinessFactory.Tb_Shop.GetPosShopInfo(ht);
                var posShopInfoModel = br.Data as PosShopInfoModel;
                if (posShopInfoModel == null)
                {
                    res.State = ServiceState.Fail;
                    if (userModel.version == 10)
                    {
                        res.Message = "密码错误!";
                    }
                    else
                    {
                        res.Message = "门店验证码错误!";
                    }
                    return res;
                }
                var key = userModel.id_masteruser + "_" + posShopInfoModel.id_shop;
                var ticket = Guid.NewGuid().ToString();
                posShopInfoModel.ticket = ticket;
                Tb_Ticket ticketModel = new Tb_Ticket()
                {
                    ticket = ticket,
                    key_y = key,
                    id = Guid.NewGuid().ToString()
                };
                var ticketRes = BusinessFactory.Tb_Ticket.Add(ticketModel);
                if (!string.IsNullOrEmpty(ticketRes.Data + ""))
                {
                    posShopInfoModel.ticket = ticketRes.Data.ToString();
                }
                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                posShopInfoModel.version = userModel.version;
                res.Data = posShopInfoModel;
                res.Message = br.Message.FirstOrDefault();
                return res;
            });
            return JsonString(sr);
        }

        /// <summary>
        /// 零售终端注册
        /// 2016-9-14 修改LD
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult LszdRegister(Ts_Lszd model)
        {
            var sr = RequestResult(res =>
            {
                if (CheckParamNull(res).State == ServiceState.Fail)
                {
                    return res;
                }
                model.ip = Ip;
                model.rq_create = DateTime.Now;

                var br = BusinessFactory.Ts_Lszd.Add(model);
                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Data = br.Data;
                res.Message = br.Message.FirstOrDefault();
                return res;
            });
            return JsonString(sr);
        }


        public ActionResult GetSystemTime(string phone)
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
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
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser" });

                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 返回

                res.State = ServiceState.Done;
                res.Message = "操作成功!!";
                res.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                return res;
                #endregion
            });
            return JsonString(sr);
            #region 注释
            //#region 获取参数
            //ServiceResult res = new ServiceResult();
            //Hashtable param = base.GetParameters();
            //ParamVessel p = new ParamVessel();
            //p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
            //p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
            //p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
            //#endregion
            //#region 验证参数
            //try
            //{
            //    param = param.Trim(p);
            //}
            //catch (Exception ex)
            //{
            //    res.State = ServiceState.Fail;
            //    res.Message = ServiceFailCode.A0001;
            //    return JsonString(res);
            //}
            //#endregion
            //#region 读取ticket
            ////读取ticket
            //Hashtable ht = new Hashtable();
            //ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
            //var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
            //if (!ticketBr.Success)
            //{
            //    res.State = ServiceState.Fail;
            //    res.Message = ServiceFailCode.S0001;
            //    return JsonString(res);
            //}

            //var ticketModel = (Tb_Ticket)ticketBr.Data;
            //if (ticketModel == null)
            //{
            //    res.State = ServiceState.Fail;
            //    res.Message = ServiceFailCode.A0003;
            //    return JsonString(res);
            //}

            //var ticket = ticketModel.ticket;
            //#endregion
            //#region 验证签名
            //IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser" });

            //var validSign = SignUtils.SignRequest(dic, ticket);
            ////验证签名
            //if (param["sign"].ToString() != validSign)
            //{
            //    res.State = ServiceState.Fail;
            //    res.Message = ServiceFailCode.A0002;
            //    return JsonString(res);
            //}
            //#endregion
            //#region 返回

            //res.State = ServiceState.Done;
            //res.Message = "操作成功!!";
            //res.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //return JsonString(res);
            //#endregion

            #endregion
        }




        public ActionResult Test(string phone)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("username", "18888888888");
            parameters.Add("yzm", "1235");
            parameters.Add("id_masteruser", "007a0a55-054e-454f-b6df-214c1a158037");
            parameters.Add("id_shop", "007a0a55-054e-454f-b6df-214c1a158037");
            //Hashtable param=new Hashtable();
            //param.Add("id_masteruser","007a0a55-054e-454f-b6df-214c1a158037");
            //param.Add("childId","f533d519-33b6-4b85-a1d0-b7fbdce53727");
            //var d= BusinessFactory.Tb_Spfl.GetTree(param);
            var md5 = MD5Encrypt.Md5(phone);
            return Json(MD5Encrypt.Md5("123456") + "|||" + MD5Encrypt.Md5("654321"), JsonRequestBehavior.AllowGet);
        }


        #region 检验购买服务接口
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult CheckCYService()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
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
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser" });
                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 获取商品信息
                var brCheck = CheckUserServiceWork(param);
                #endregion
                #region 返回
                res.State = brCheck.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = brCheck.Message.FirstOrDefault();
                res.Data = brCheck.Data;

                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion





    }
}
