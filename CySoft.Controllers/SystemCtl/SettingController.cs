using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Tb;
using CySoft.Model.Ts;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;
using System.IO;
/**/
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web;
using System.Text;
using System.Linq;

#region 设置
#endregion

namespace CySoft.Controllers.SystemCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class SettingController : BaseController
    {
        /// <summary>
        /// 日志查看
        /// lxt
        /// 2015-03-24
        /// </summary>
        [ValidateInput(false)]
        public ActionResult Log()
        {
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Ts_Log_Query> list = new PageList<Ts_Log_Query>(limit);
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("flag_time", 0, HandleType.DefaultValue);//0全部
                p.Add("flag", 0, HandleType.DefaultValue);//0全部  1本人
                p.Add("orderby", 1, HandleType.DefaultValue);//排序
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("limit", 10, HandleType.DefaultValue);//每页大小
                param = param.Trim(p);
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                limit = Convert.ToInt32(param["limit"]);
                string flag = param["flag"].ToString();
                string orderby =  param["orderby"].ToString();

                ViewData["pageindex"] = pageIndex;
                ViewData["flag_time"] = param["flag_time"];
                ViewData["flag"] = param["flag"];
                ViewData["orderby"] = param["orderby"];
                ViewData["keyword"] = GetParameter("keyword");
                ViewData["limit"] = GetParameter("limit");

                param.Remove("flag");
                switch (orderby)
                {
                    case "2":
                        param.Add("sort", "id");
                        break;
                    default:
                        param.Add("sort", "id");
                        param.Add("dir", "desc");
                        break;
                }
                switch (flag)
                {
                    case "1":
                        param.Add("id_user", GetLoginInfo<long>("id_user"));
                        break;
                    default:
                        param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
                        break;
                }
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                if (limit < 1)
                {
                    param["limit"] = limit = 1;
                }
                else if (limit > 50)
                {
                    param["limit"] = limit = 50;
                }
                param.Add("start", (pageIndex - 1) * limit);
                pn = BusinessFactory.Log.GetPage(param);
                list = new PageList<Ts_Log_Query>(pn, pageIndex, limit);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_LogListControl", list);
            }
            return View(list);
        }

        /// <summary>
        /// 公司信息
        /// lxt
        /// 2015-03-24
        /// </summary>
        [ValidateInput(false)]
        public ActionResult Company()
        {
            BaseResult br = new BaseResult();
            try
            {
                //Hashtable param = new Hashtable();
                //param.Add("id", GetLoginInfo<int>("id_user_master"));
                //param.Add("flag_pc", "");
                //ViewData["bm_gys"] = GetLoginInfo<string>("bm");
                //br = BusinessFactory.Company.Get(param);
                //Tb_User_Query m_User = br.Data as Tb_User_Query;
                //var id_master = GetLoginInfo<long>("id_user_master");
                //var url = String.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority);
                //var destImg = string.Empty;
                // if (!string.IsNullOrEmpty(m_User.pic))
                // destImg = Server.MapPath(m_User.pic); //@"~\Images\\eweilogo.png");
                // else
                //     destImg = Server.MapPath(@"~\Images\\eweilogo.png");
                //if (string.IsNullOrEmpty(m_User.pic_erwei))
                //{
                //    var id_des = DESEncrypt.EncryptDES(id_master.ToString());
                //    var id = Base64Encrypt.EncodeBase64(id_des);
                //    var data = url + "/ServiceCustomer/Scan/" + id;
                //    string filename = "erwei_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                //    string filepath = Server.MapPath("~\\UpLoad\\User\\Master") + "\\" + filename;
                //    QRCode.CreatQRCode(destImg,data,filepath);
                //    m_User.pic_erwei = "/UpLoad/User/Master/"+filename;
                //    //保存二维码
                //    param.Clear();
                //    param.Add("id", GetLoginInfo<long>("id_user"));
                //    param.Add("id_user", GetLoginInfo<long>("id_user"));
                //    param.Add("id_user_master", GetLoginInfo<string>("id_user_master"));
                //    param.Add("flag_from", GetLoginInfo<string>("flag_from"));
                //    param.Add("pic_erwei", m_User.pic_erwei);
                //    var result=BusinessFactory.Account.Save(param); 
                //   // ViewData["filename"] = filename;
                //}
               
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

            return View(br.Data);
        }

        /// <summary>
        /// 更新公司信息
        /// lxt
        /// 2015-03-24
        /// 2015-7-15 wzp 修改
        /// </summary>
        [HttpPost]
        [ActionPurview("Company")]
        public ActionResult UpdateCompany(string obj)
        {
            BaseResult br = new BaseResult();
            var model = JSON.Deserialize<Tb_User_Query>(obj);
            if (model.companyname.IsEmpty())
            {
                br.Message.Add("公司名称不能为空！");
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Data = "companyname";
                return Json(br);
            }
            if (model.id_province == 0 || model.id_city == 0)
            {
                br.Message.Add("省市区不能为空！");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                br.Data = "area";
                return Json(br);
            }
            //if (model.name.IsEmpty())
            //{
            //    br.Message.Add("联系人不能为空！");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = "name";
            //    br.Success = false;
            //    br.Data = null;
            //    return Json(br);
            //}
            //if (model.phone.IsEmpty())
            //{
            //    br.Message.Add("手机号不能为空！");
            //    br.Level = ErrorLevel.Warning;
            //    br.Success = false;
            //    br.Data = "phone";
            //    return Json(br);
            //}
            try
            {
                model.id = GetLoginInfo<string>("id_user");
                model.id_master = GetLoginInfo<int>("id_user_master");
                model.id_edit = model.id;
                br = BusinessFactory.Company.Update(model);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return Json(br);
        }
        
        /// <summary>
        /// 业务流程设置
        /// lxt
        /// 2015-03-24
        /// cxb
        /// 2015-6-16
        /// </summary>
        [ValidateInput(false)]
        public ActionResult Process()
        {
            BaseResult br = new BaseResult();
            Hashtable param =new Hashtable();    
            try {
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                param.Add("start_flag_state",OrderFlag.OrderCheck);
                param.Add("end_flag_state", OrderFlag.WaitDelivery);
                br = BusinessFactory.SaleOrder.GetCount(param);
                var order = (int)br.Data;
                if (order > 0)
                {
                    ViewData["Confirm"] = 1;
                }
                else
                {
                    ViewData["Confirm"] = 0;
                }
                param.Clear();
                param.Add("id_user_master",GetLoginInfo<long>("id_user_master"));
                br=BusinessFactory.Setting.GetAll(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(br.Data);
        }

        /// <summary>
        /// 修改业务流程
        /// cxb 2015-6-16 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       [ActionPurview(false)]
        public ActionResult UpdateProcess(string obj)
        {
            BaseResult br = new BaseResult();          
            try {

                var list = JSON.Deserialize<List<Ts_Param_Business>>(obj);

                if (list == null || list.Count < 1)
                {
                    br.Level = ErrorLevel.Warning;
                    br.Success = false;
                    br.Message.Add("订单业务流程没有修改内容。");
                    return Json(br);
                }
              
                    Hashtable param = new Hashtable();                      

                    var status = false;
                    var is_order_ywlc_shqr = false;
                    var id_user_master = GetLoginInfo<long>("id_user_master");
                    var id_user = GetLoginInfo<long>("id_user");

                    param.Add("id_user_master", id_user_master);
                    br = BusinessFactory.Setting.GetAll(param);
                    if (br.Data == null)
                    {
                        br.Level = ErrorLevel.Warning;
                        br.Success = false;
                        br.Message.Add("订单业务流程未定义。");
                        return Json(br);
                    }
                    var param_b = br.Data as List<Ts_Param_Business>;
                    if (param_b == null || param_b.Count<1)
                    {
                        br.Level = ErrorLevel.Warning;
                        br.Success = false;
                        br.Message.Add("订单业务流程未定义。");
                        return Json(br);
                    }
                //过滤是否有不符合条件的修改
                    foreach (var p in list)
                    {
                        var k = "1";
                        var val = from item in param_b where item.bm.Equals(p.bm) select item;
                        if (val != null&&val.Count()==1)
                        {
                            k = val.First().val;
                        }
                        if (p.bm.ToLower().Equals("order_ywlc_shqr") && p.val == "0" && !p.val.Equals(k)) is_order_ywlc_shqr = true;
                        if (!p.bm.ToLower().Equals("order_ywlc_shqr") && k=="1" &&!p.val.Equals(k)) status = true;
                    }
                //有未处理的单，不能修改某些特定流程，必须处理完订单后才能修改
                    if (status)
                    {
                        param.Clear();
                        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                        param.Add("start_flag_state", OrderFlag.OrderCheck);
                        param.Add("end_flag_state", OrderFlag.WaitDelivery);
                        br = BusinessFactory.SaleOrder.GetCount(param);
                        var order = (int)br.Data;
                        if (order > 0)
                        {
                            br.Level = ErrorLevel.Warning;
                            br.Success = false;
                            br.Message.Add("请处理完所有处在可调整订单业务流程范围内的订单后，再修改订单业务流程。");
                            return Json(br);
                        }
                    }
                   //批量收货确认
                    if (is_order_ywlc_shqr)
                    {
                        param.Clear();
                        param.Add("id_user", GetLoginInfo<long>("id_user"));
                        param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                        br = BusinessFactory.ShippingRecord.ConfirmBatch(param);
                        if (!br.Success)
                        {
                            br.Level = ErrorLevel.Warning;
                            br.Success = false;
                            br.Message.Add("批量收货确认未成功，请重新再试。");
                            return Json(br);
                        }
                    }

                    foreach (var m in list)
                    {
                        m.id_user_master = id_user_master;
                        m.id_create = id_user;
                    }
                    br = BusinessFactory.Setting.Save(list);

               
                //Hashtable param = GetParameters();
                //ParamVessel p = new ParamVessel();
                //p.Add("List", string.Empty, HandleType.ReturnMsg);//业务流程列表                
                //param = param.Trim(p);
                ////判断是否存在模板，没有就插入模板
                //param0["id_user_master"] = GetLoginInfo<long>("id_user_master");
                //param0["not_id_user_master"] = 0;
                //param0["ywlc_bm"] = 0;
                //br=BusinessFactory.Process.GetCount(param0);
                //if ((int)br.Data==0) {
                //    br=BusinessFactory.Process.Add(param0);
                //}
                //param0.Clear();
                ////订单是否在这个流程上
                //flag_state_List.AddRange(new List<int> { (int)OrderFlag.OrderCheck,(int)OrderFlag.FinanceCheck, (int)OrderFlag.WaitOutputCheck, (int)OrderFlag.WaitDelivery });
                //param0["flag_state_List"] = flag_state_List;
                //param0["id_gys"] = GetLoginInfo<long>("id_supplier");
                //br=BusinessFactory.SaleOrder.GetCount(param0);
                //flag0 =(int)br.Data;
                ////订单不在此流程但这个流程上有订单
                //param0["flag_out"] = 1;
                //param0.Remove("flag_state_List");
                //br = BusinessFactory.SaleOrder.GetCount(param0);
                //flag1 = (int)br.Data;
                //param0.Clear();
                //param0["id_user_master"] = GetLoginInfo<long>("id_user_master");
                //param0["Process_List"] = param["Process_List"];
                //param0.Add("sort", "sort_id");
                //param0.Add("dir", "asc");
                //br = BusinessFactory.Process.CompareProcess(param0);
                //flag2 = (int)br.Data;
                //if (flag2==0)//表示在此过程中除了确认订单流程是否有删减流程
                //{
                //    if (flag0 > 0 || flag1 > 0)
                //    {
                //        if (flag0 > 0)//有删减 但是订单在此流程上
                //        {
                //            br.Data = "false";
                //            br.Success = true;
                //            br.Message.Add("订单完成财务审核、出库审核、发货审核流程才能修改业务流程");
                //        }
                //        else if (flag1 > 0)//订单不在此流程上 但是订单里面有发货单
                //        {
                //            br.Data = "false";
                //            br.Success = true;
                //            br.Message.Add("必须处理已经出库发货的订单才能修改订单流程,请到销售列表,出库/发货查看！");
                //        }
                //    }
                //    else//有删减但是合法
                //    {
                //        model.id_user_master = GetLoginInfo<long>("id_user_master");
                //        param["model"] = model;
                //        br = BusinessFactory.Process.Update(param);
                //        if (br.Success == true && param["isConfirmOrder"]=="1")
                //        {
                //            param.Clear();
                //            param["id_gys"] = GetLoginInfo<long>("id_supplier");
                //            br = BusinessFactory.ShippingRecord.ConfirmBatch(param);
                //        }
                //    }
                //}
                //else//无删减
                //{
                //    model.id_user_master = GetLoginInfo<long>("id_user_master");
                //    param["model"] = model;
                //    br = BusinessFactory.Process.Update(param);
                //    if (br.Success == true && param["isConfirmOrder"].ToString() == "1")
                //    {
                //        param.Clear();
                //        param["id_gys"] = GetLoginInfo<long>("id_supplier");
                //        br = BusinessFactory.ShippingRecord.ConfirmBatch(param);
                //    }
                //}
               
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(br);
        }

        /// <summary>
        /// 系统设置
        /// cxb
        /// 2015-6-23
        /// </summary>
        [ValidateInput(false)]
        public ActionResult System()
        {
            Hashtable param = new Hashtable();
            BaseResult br = new BaseResult();
            try {
                param["id_user_master"] = GetLoginInfo<long>("id_user_master");
                br = BusinessFactory.Setting.GetAll(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(br.Data);
        }

        /// <summary>
        /// 保存系统设置 cxb 2015-6-25 
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult SaveSystem(string obj) {
            BaseResult br = new BaseResult();
            Hashtable param = JSON.Deserialize<Hashtable>(obj);
            ParamVessel p = new ParamVessel();
            p.Add("OrderSettingList",null,HandleType.ReturnMsg);//设置列表
            param = param.Trim(p);
            try {
               var id_user_master = GetLoginInfo<long>("id_user_master");
               var id_user = GetLoginInfo<long>("id_user");
               var list = JSON.ConvertToType<List<Ts_Param_Business>>(param["OrderSettingList"]);
               if (list == null || list.Count < 1)
               {
                   br.Success = false;
                   br.Level = ErrorLevel.Warning;
                   br.Message.Add("设置参数不能为空.");
               }
               else
               {
                   foreach (var m in list) {
                       m.id_user_master = id_user_master;
                       m.id_create = id_user;
                   }           
                   br = BusinessFactory.Setting.Save(list);
               }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(br);
        }

        /// <summary>
        /// 初始化数据 cxb 2015-6-23
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult Initialization()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("init_type", string.Empty, HandleType.ReturnMsg);
            param = param.Trim(p);
            try {
                if (param["init_type"].ToString() == "1")
                {
                    param["del_lx"] = "YWDATA";
                }
                else {
                    param["del_lx"] = "All";
                }
                param["id_gys"] = GetLoginInfo<long>("id_supplier");
                param["id_user_master"] = GetLoginInfo<long>("id_user_master");
                br = BusinessFactory.Setting.Init(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(br);
        }
        ///// <summary>
        ///// 订单设置 cxb 2015-6-25 
        ///// </summary>
        ///// <returns></returns>
        //[ActionPurview(false)]
        //public ActionResult OrderSetting() {
        //    BaseResult br = new BaseResult();
        //    try {

        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Json(br);
        //}

        /// <summary>
        /// 分页获取结果集图册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult GetPicResult()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            bool b = false;
            if (param.Count == 0)
            {
                b = true;
            }
            ParamVessel p = new ParamVessel();
            p.Add("pageIndex", 1, HandleType.DefaultValue);
            param = param.Trim(p);
            int limit =5;
            int pageIndex = int.Parse(param["pageIndex"].ToString());
            PageList<Tb_User_Pic> lst = new PageList<Tb_User_Pic>(limit);
            PageNavigate pn = new PageNavigate();
            param.Add("id_master", GetLoginInfo<long>("id_user_master"));
            param.Add("start", (pageIndex - 1) * limit);
            param.Add("limit", limit);
            try
            {
                pn = BusinessFactory.UserPic.GetPage(param);               
                lst = new PageList<Tb_User_Pic>(pn, pageIndex, limit);
                ViewData["pageCount"] = lst.PageCount;
                if (b)
                {
                    return PartialView("_PicDialogListControl", lst);
                }
                else
                {
                    br.Data = lst;
                    br.Success = true;
                    return Json(br);
                }
                //
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取全部结果集图册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult GetAllsPic()
        {
            Hashtable param = new Hashtable();
            BaseResult br = new BaseResult();
            param.Add("id_master", GetLoginInfo<long>("id_user_master"));
            try
            {
                br = BusinessFactory.UserPic.GetAll(param);
                br.Success = true;
                return PartialView("_PicDialogListControl", br.Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除图片
        /// wzp 2015-7-16
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult DeletePic()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            
            ParamVessel p = new ParamVessel();
            p.Add("xhList", string.Empty, HandleType.DefaultValue);
            p.Add("picUrl", string.Empty, HandleType.DefaultValue);
            param = param.Trim(p);
            //删除临时的图片
            if (param["picUrl"].ToString().Contains("Temp"))
            {
                try
                {
                    string url = Server.MapPath("~/") + param["picUrl"].ToString();
                    FileInfo delFile = new FileInfo(url);
                    if (delFile.Exists)
                    {
                        delFile.Delete();
                        br.Success = true;
                        br.Message.Add("删除图片成功！");
                        return Json(br);
                    }
                }
                catch (Exception)
                {
                    br.Success = false;
                    br.Message.Add("删除图片失败！");
                    return Json(br);
                }
            }
            //删除已存在的图片
            else
            {
                try
                {
                    param.Remove("picUrl");
                    string[] lst = param["xhList"].ToString().Split(',');

                    param["xhList"] = lst;
                    param.Add("id_master", GetLoginInfo<long>("id_user_master"));
                    br = BusinessFactory.UserPic.Delete(param);
                }
                catch (CySoftException ex)
                {
                    br.Data = ex.Result;
                }
                catch (Exception ex)
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Error;
                    br.Message.Add("<h5>删除失败</h5>");
                    br.Message.Add("");
                    br.Message.Add("请重试或与管理员联系！");
                }
            }
            
            return Json(br);
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="filename"></param>
        [ActionPurview(false)]
        public void SaveImage(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return;
            var list_file=filename.Split('/');
            string fileName =list_file[list_file.Length-1]; //客户端保存的文件名
            string filepath = Server.MapPath(filename); ; //Server.MapPath("~\\UpLoad\\User\\Master") + "\\" + filename;
            //以字符流的形式下载文件
            FileStream fs = new FileStream(filepath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.ClearContent();
            Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开
            Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
            Response.BinaryWrite(bytes);
            // Response.Flush();
            //  Response.End();
        }
    }
}
