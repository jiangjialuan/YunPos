//using CySoft.BLL.Base;
//using CySoft.Frame.Core;
//using CySoft.IBLL;
//using CySoft.IDAL;
//using CySoft.Model.Flags;
//using CySoft.Model.Pay;
//using CySoft.Model.Tb;
//using CySoft.Model.Td;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace CySoft.BLL.FundsBLL
//{
//    public class PayBLL : BaseBLL, IPayBLL
//    {
//        public BaseResult AddPayData(dynamic entity,string dataType)
//        {
//            BaseResult br = new BaseResult();
            
//            try
//            {
//                string type = "";
//                if (dataType=="reg")
//                {
//                    RegisterPay reg_pay = (RegisterPay)entity;
//                    type=reg_pay.customertype;
//                    DAL.Add<RegisterPay>(reg_pay);
//                }
//                if (dataType=="log")
//                {
//                    RegisterPayLog log = (RegisterPayLog)entity;
//                    DAL.Add<RegisterPayLog>(log);
//                    if (log.flag_action == PayFlag.Done)
//                    {
//                        br.Success = true;
//                        br.Message.Add(log.des_action);
//                        br.Data = type;
//                    }
//                    else
//                    {
//                        br.Success = false;
//                        br.Message.Add(log.msg_error);
//                    }
//                }
//                if (dataType == "certified")
//                {
//                    Hashtable param = new Hashtable();
//                    param["ledgerno"] = entity.ledgerno;
//                    param["id_user_master"] = entity.id_user_master;
//                    int count = DAL.GetCount(typeof(CertifiedFile), param);
//                    CertifiedFile cf = cf = (CertifiedFile)entity;
//                    cf.rq_create = DateTime.Now;
//                    if (count != 1)
//                    {
//                        DAL.Add<CertifiedFile>(cf);
//                    }
//                    else
//                    {
//                        DAL.Update<CertifiedFile>(cf);
//                    }
//                }
//                if (dataType == "trade")
//                {
//                    YeePay_Trade trade = (YeePay_Trade)entity;
//                    DAL.Add<YeePay_Trade>(trade);
//                }
//                if (dataType == "wxAccount")
//                {
//                    WeChatAccount wx = (WeChatAccount)entity;
//                    Hashtable param = new Hashtable();
//                    param["id_user_master"]=wx.id_user_master;
//                    WeChatAccount item= DAL.GetItem<WeChatAccount>(typeof(WeChatAccount), param);
//                    if (item != null)
//                    {
//                        item.mchid = wx.mchid;
//                        item.flag_state = 1;
//                        item.id_edit = wx.id_user_master;
//                        item.rq_edit = DateTime.Now;
//                        DAL.Update<WeChatAccount>(item);
//                    }
//                    else
//                    {
//                        DAL.Add<WeChatAccount>(wx);
//                    }
//                    br.Success = true;
//                    br.Message.Add("设置成功！");
//                }
//            }
//            catch (Exception)
//            {
//                br.Success = false;
//                br.Message.Add("服务器内部出错,请联系客服。");
                
//            }
//            return br;
//        }
//        public BaseResult CheckPayAccount(Hashtable param,string type)
//        {
//            BaseResult br = new BaseResult();
//            if (type == "yeePay")
//            {
//                br.Data = DAL.GetItem(typeof(RegisterPay), param);
//            }
//            else if (type == "wxPay")
//            {
//                br.Data = DAL.GetItem(typeof(WeChatAccount), param);
//            }
            
//            return br;
//        }
//        public BaseResult Update(dynamic entity,string type)
//        {
//            BaseResult br = new BaseResult();
//            if(type=="pay")
//            {
//                Hashtable hb = (Hashtable)entity;
//                br.Data = DAL.UpdatePart(typeof(RegisterPay), hb);
//            }
//            if (type == "trade")
//            {
//                Hashtable hb = (Hashtable)entity;
//                br.Data = DAL.UpdatePart(typeof(YeePay_Trade), hb);
//            }
//            if (type == "wxInfo")
//            {
//                WeChatAccount wx= (WeChatAccount)entity;
//                br.Data = DAL.Update<WeChatAccount>(wx);
//            }
//            return br;
//        }
//        public BaseResult GetCity(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            br.Data = DAL.QueryList<YeePay_City>(typeof(YeePay_City), param);
//            return br;
//        }
//        public BaseResult GetBanks(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            br.Data = DAL.QueryList<YeePay_Banks>(typeof(YeePay_Banks), param);
//            return br;
//        }
//        public override PageNavigate GetPage(Hashtable param = null)
//        {
//            PageNavigate pn = new PageNavigate();
//            pn.TotalCount = DAL.QueryCount(typeof(YeePay_Trade), param);
//            if (pn.TotalCount > 0)
//            {
//                pn.Data = DAL.QueryPage<YeePay_Trade>(typeof(YeePay_Trade), param);

//            }
//            else
//            {
//                pn.Data = new List<YeePay_Trade>();
//            }
//            pn.Success = true;
//            return pn;
//        }
//        public BaseResult GetDivideinfo(string dh)
//        {
//            Hashtable param = new Hashtable();
//            BaseResult br = new BaseResult();
//            param["dh"]=dh;
//            Td_Sale_Order_Head_Query head = (Td_Sale_Order_Head_Query)DAL.GetItem<Td_Sale_Order_Head>(typeof(Td_Sale_Order_Head), param);
//            if (head == null)
//            {
//                br.Success = false;
//                br.Message.Add("该订单不存在");
//                return br;
//            }
//            decimal je_pay = (decimal)head.je_pay;//得到总单应付总额
//            long id_gys =(long)head.id_gys;//获取总单供应商
//            long id_cgs = (long)head.id_cgs;//获取总单采购商
//            param.Clear();
//            param["id_gys"] = id_gys;
//            param["id_cgs"] = id_cgs;
//            Tb_Gys_Cgs gys_cgs = DAL.GetItem<Tb_Gys_Cgs>(typeof(Tb_Gys_Cgs), param);
//            long id_cgs_level = (long)gys_cgs.id_cgs_level;
//            param.Clear();
//            param["dh_father"] = dh;
//            //根据单号查询总单是否存在子单
//            IList<Td_Sale_Order_Fd> fdList = DAL.QueryList<Td_Sale_Order_Fd>(typeof(Td_Sale_Order_Fd), param);
//            param.Clear();
//            if (fdList != null && fdList.Count > 0)
//            {
//                param["dh"] = dh;
//                //查询总单中所有商品
//                IList<Td_Sale_Order_Body> body = DAL.QueryList<Td_Sale_Order_Body>(typeof(Td_Sale_Order_Body), param);
//                param.Clear();
//                foreach (Td_Sale_Order_Body item_body in body)
//                {
//                    param["id_gys_fd"] = id_gys;
//                    param["id_sku"] = item_body.id_sku;
//                    //查询当前循坏的sku是否存在分担关系
//                    Tb_sp_sku_fd sku_fd = DAL.GetItem<Tb_sp_sku_fd>(typeof(Tb_sp_sku_fd), param);
//                    if (sku_fd != null)
//                    {
//                        param.Remove("id_gys_fd");
//                        IList<Tb_Sp_Dj> dj = DAL.QueryList<Tb_Sp_Dj>(typeof(Tb_Sp_Dj), param);
//                        foreach (Tb_Sp_Dj item_dj in dj)
//                        {
//                            if(item_dj.id_gys==id_gys&&item_dj.id_sku==sku_fd.id_sku)
//                            {

//                            }
//                        }

//                    }
//                }
//                foreach (Td_Sale_Order_Fd item in fdList)
//                {
                    
//                }
//            }
//            return br;
//        }
//    }
//}
