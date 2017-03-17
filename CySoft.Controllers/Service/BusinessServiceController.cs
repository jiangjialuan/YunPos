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
using System.Text.RegularExpressions;
using CySoft.Model.Td;
using CySoft.Model.Enums;

//业务
namespace CySoft.Controllers.Service
{
    public class BusinessServiceController : ServiceBaseController
    {

        #region 快速盘点
        /// <summary>
        /// 通用存储过程查询
        /// lz
        /// 2016-10-26
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult QuickInventory()
        {

            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel pv = new ParamVessel();
                pv.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                pv.Add("json_param", string.Empty, HandleType.ReturnMsg);//jsonParam
                pv.Add("id_jbr", string.Empty, HandleType.ReturnMsg);//id_jbr
                pv.Add("id_create", string.Empty, HandleType.ReturnMsg);//id_create
                pv.Add("bz", string.Empty, HandleType.DefaultValue);//bz
                pv.Add("rq", string.Empty, HandleType.ReturnMsg);//rq
                pv.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(pv);
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
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "json_param", "id_jbr", "id_create", "bz", "rq" });
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
                var br = BusinessFactory.Td_Kc_Kspd_1.Add(param);
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

        #region 通用存储过程查询
        /// <summary>
        /// 通用存储过程查询
        /// lz
        /// 2016-10-25
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult ProcedureQuery()
        {

            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel pv = new ParamVessel();
                pv.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                pv.Add("json_param", string.Empty, HandleType.ReturnMsg);//jsonParam
                pv.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(pv);
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
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "json_param" });
                
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
                var br = BusinessFactory.Business.ProcedureQuery(param);
                #endregion
                #region 返回
                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = br.Data;
                return res;
                #endregion

            });

            #region 数据处理
            var jsonString = JSON.Serialize(sr); ;
            string p = @"\\/Date\((\d+)\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(this.ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            string p2 = @"\\/Date\(([/+/-]\d+)\)\\/";
            MatchEvaluator matchEvaluator2 = new MatchEvaluator(this.ConvertJsonDateToDateString);
            Regex reg2 = new Regex(p2);
            jsonString = reg2.Replace(jsonString, matchEvaluator2);
            #endregion

            return Content(jsonString);
        }
        #endregion

        #region 通用存储过程查询  带out参数
        /// <summary>
        /// 通用存储过程查询  带out参数
        /// lz
        /// 2016-11-17
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult ProcedureOutQuery()
        {

            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel pv = new ParamVessel();
                pv.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                pv.Add("json_param", string.Empty, HandleType.ReturnMsg);//jsonParam
                pv.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(pv);
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
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "json_param" });

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
                var br = BusinessFactory.Business.ProcedureOutQuery(param);
                #endregion
                #region 返回
                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = br.Data;
                return res;
                #endregion

            });

            #region 数据处理
            var jsonString = JSON.Serialize(sr); ;
            string p = @"\\/Date\((\d+)\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(this.ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            string p2 = @"\\/Date\(([/+/-]\d+)\)\\/";
            MatchEvaluator matchEvaluator2 = new MatchEvaluator(this.ConvertJsonDateToDateString);
            Regex reg2 = new Regex(p2);
            jsonString = reg2.Replace(jsonString, matchEvaluator2);
            #endregion

            return Content(jsonString);
        }
        #endregion

        #region 将Json序列化的时间由/Date(1294499956278+0800)转为字符串    
        /// <summary>    
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串    
        /// lz
        /// 2016-10-25
        /// </summary>    
        private string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }
        #endregion

        #region 收货引订货单查询接口
        /// <summary>
        /// 收货引订货单查询接口
        /// lz
        /// 2017-01-22
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult JhddNoFinish()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("rq_begin", string.Empty, HandleType.DefaultValue);//rq_begin
                p.Add("rq_end", string.Empty, HandleType.DefaultValue);//rq_end
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
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "rq_begin", "rq_end" });

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
                ht.Clear();

                ht.Clear();
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                ht.Add("id_shop", param["id_shop"].ToString());
                ht.Add("no_th", 1);

                DateTime now = DateTime.Now;

                if (!string.IsNullOrEmpty(param["rq_begin"].ToString())&&DateTime.TryParse(param["rq_begin"].ToString(), out now))
                {
                    ht.Add("start_rq", DateTime.Parse(param["rq_begin"].ToString()));
                }
                if (!string.IsNullOrEmpty(param["rq_end"].ToString()) && DateTime.TryParse(param["rq_end"].ToString(), out now))
                {
                    ht.Add("end_rq", DateTime.Parse(param["rq_end"].ToString()));
                }

                var br = BusinessFactory.Business.ShydhQuery(ht);

                #endregion

                #region 返回

                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = br.Data;

                return res;
                #endregion
            });
            
            #region 数据处理
            var jsonString = JSON.Serialize(sr); ;
            string p1 = @"\\/Date\((\d+)\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(this.ConvertJsonDateToDateString);
            Regex reg = new Regex(p1);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            string p2 = @"\\/Date\(([/+/-]\d+)\)\\/";
            MatchEvaluator matchEvaluator2 = new MatchEvaluator(this.ConvertJsonDateToDateString);
            Regex reg2 = new Regex(p2);
            jsonString = reg2.Replace(jsonString, matchEvaluator2);
            #endregion

            return Content(jsonString);
        }
        #endregion

        #region 收货保存接口
        /// <summary>
        /// 收货保存接口
        /// lz
        /// 2017-01-22
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult PurchaseReceipt()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("id_user", string.Empty, HandleType.ReturnMsg);//id_user
                p.Add("shopspList", string.Empty, HandleType.ReturnMsg);//商品List
                p.Add("remark", string.Empty, HandleType.DefaultValue);//remark
                p.Add("id_gys", string.Empty, HandleType.ReturnMsg);//id_gys
                p.Add("id_jbr", string.Empty, HandleType.ReturnMsg);//id_jbr
                p.Add("rq", string.Empty, HandleType.ReturnMsg);//rq
                p.Add("je_sf", string.Empty, HandleType.ReturnMsg);//je_sf
                p.Add("dh_origin", string.Empty, HandleType.DefaultValue);//dh_origin
                p.Add("bm_djlx_origin", string.Empty, HandleType.DefaultValue);//bm_djlx_origin
                p.Add("id_bill_origin", string.Empty, HandleType.DefaultValue);//id_bill_origin
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
                IDictionary<string, string> dic = base.GetParameters(new string[] 
                {
                    "id_shop",
                    "id_masteruser",
                    "id_user",
                    "shopspList",
                    "remark",
                    "id_gys",
                    "id_jbr",
                    "rq",
                    "je_sf",
                    "dh_origin",
                    "bm_djlx_origin",
                    "id_bill_origin"
                });

                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion

                #region 验证数据操作

                List<Td_Jh_2> shopspList = new List<Td_Jh_2>();

                try
                {
                    shopspList = JSON.Deserialize<List<Td_Jh_2>>(param["shopspList"].ToString()) ?? new List<Td_Jh_2>();
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = "操作失败 shopspList不符合要求！";
                    return res;
                }

                if (shopspList == null || shopspList.Count() <= 0)
                {
                    res.State = ServiceState.Fail;
                    res.Message = "操作失败 shopspList不能为空！";
                    return res;
                }

                var digitHashtable = GetParm(param["id_masteruser"].ToString());

                foreach (var item in shopspList)
                {
                    item.sl = CySoft.Utility.DecimalExtension.Digit(item.sl, int.Parse(digitHashtable["sl_digit"].ToString()));
                    item.dj = CySoft.Utility.DecimalExtension.Digit(item.dj, int.Parse(digitHashtable["dj_digit"].ToString()));
                    item.je = CySoft.Utility.DecimalExtension.Digit(item.je, int.Parse(digitHashtable["je_digit"].ToString()));

                    if (item.sl == 0)
                    {
                        if (string.IsNullOrEmpty(param["id_bill_origin"].ToString()))
                        {
                            res.State = ServiceState.Fail;
                            res.Message = "商品[" + item.barcode + "]数量不允许为0!";
                            return res;
                        }
                        else
                        {
                            item.je = 0;
                            continue;
                        }
                    }

                    //此处验证数据是否符合
                    var tempJe = CySoft.Utility.DecimalExtension.Digit(item.sl * item.dj, int.Parse(digitHashtable["je_digit"].ToString()));
                    var tempDj = CySoft.Utility.DecimalExtension.Digit(item.je / item.sl, int.Parse(digitHashtable["dj_digit"].ToString()));
                    if (tempJe == item.je || tempDj == item.dj)
                    {
                        continue;
                    }
                    else
                    {
                        res.State = ServiceState.Fail;
                        res.Message = "商品中存在 单价*数量不等于金额的数据!";
                        return res;
                    }
                }

                param.Remove("shopspList");
                param.Add("shopspList", shopspList);
                param.Add("DigitHashtable", digitHashtable);
                param.Add("autoAudit", true);

                #endregion

                #region 执行业务保存操作
                var dh  = GetNewDH(Enums.FlagDJLX.DHJH, param["id_masteruser"].ToString(), param["id_shop"].ToString());
                param.Add("dh", dh);
                var br = BusinessFactory.Td_Jh_1.Add(param);
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
