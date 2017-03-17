//using System;
//using System.Text;
//using System.Collections;
//using System.Collections.Generic;
//using System.Web.Mvc;
//using System.Web.UI;
//using CySoft.Controllers.Base;
//using CySoft.Controllers.Filters;
//using CySoft.Frame.Core;
//using CySoft.Model.Flags;
//using CySoft.Model.Tb;
//using CySoft.Utility;
//using CySoft.Utility.Mvc.Html;

//#region 通知管理
//#endregion

//namespace CySoft.Controllers.SupplierCtl.CustomerCtl
//{
//    [LoginActionFilter]
//    [ValidateInput(false)]
//    [OutputCache(Location = OutputCacheLocation.None)]
//    public class NotificationController : BaseController
//    {
//        /// <summary>
//        /// 我的通知
//        /// </summary>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public ActionResult MyInform()
//        {
//            Hashtable param = GetParameters();
//            if (param.Count == 0)
//            {
//                param.Add("flag_reade", 0);
//            }
//            ParamVessel p = new ParamVessel();
//            p.Add("id_gys", string.Empty, HandleType.Remove);
//            p.Add("flag_reade", string.Empty, HandleType.Remove);
//            p.Add("pageIndex", (long)1, HandleType.DefaultValue);
//            param = param.Trim(p);
//            //发布来源
//            ViewData["flag_reade"] = param.ContainsKey("flag_reade") ? param["flag_reade"].ToString() : "";
//            if (param.ContainsKey("id_gys"))
//            {
//                long id_gys = long.Parse(param["id_gys"].ToString());
//                ViewData["id_gys"] = id_gys;
//                param.Remove("id_gys");
//                switch (id_gys)
//                {
//                    //查全部通知
//                    case 0:
//                        break;
//                    //查公司内部公告
//                    case -1:
//                        param.Add("id_master_from", GetLoginInfo<long>("id_user_master"));
//                        break;
//                    //根据供应商查询
//                    default:
//                        BaseResult br = new BaseResult();
//                        param.Add("id", id_gys);
//                        //根据供应商ID获取供应商对象
//                        br = BusinessFactory.Supplier.Get(param);
//                        if (br.Data == null)
//                        {
//                            br.Success = false;
//                            br.Message.Add(string.Format("不存在该供应商或供应商资料有误，请检查后重试！"));
//                            return null;
//                        }
//                        Tb_Gys gys = (Tb_Gys)br.Data;
//                        param.Add("id_master_from", (long)gys.id_user_master);
//                        param.Remove("id");
//                        break;
//                }
//            }
//            PageList<Info_Query> lst = GetPageData(param, 0);
//            if (Request.IsAjaxRequest())
//            {
//                return PartialView("_ListControl", lst);
//            }
//            return View(lst);
//        }

//        /// <summary>
//        /// 我的公告
//        /// </summary>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public ActionResult MyNotice()
//        {
//            Hashtable param = GetParameters();
//            ParamVessel p = new ParamVessel();
//            p.Add("selectTime", string.Empty, HandleType.Remove);
//            p.Add("pageIndex", (long)1, HandleType.DefaultValue);
//            p.Add("id_info_type", string.Empty, HandleType.Remove);
//            param = param.Trim(p);
//            //按时间查询
//            if (param.ContainsKey("selectTime"))
//            {
//                long flag = long.Parse(param["selectTime"].ToString());
//                param.Remove("selectTime");
//                ViewData["selectTime"] = flag;
//                switch (flag)
//                {
//                    //全部时间
//                    case 0:
//                        break;
//                    //最近一天
//                    case 1:
//                        param.Add("rq_create_day", "");
//                        break;
//                    //最近一个星期
//                    case 2:
//                        param.Add("rq_create_week", "");
//                        break;
//                    //最近一个月
//                    case 3:
//                        param.Add("rq_create_month", "");
//                        break;
//                    //最近一个年
//                    case 4:
//                        param.Add("rq_create_year", "");
//                        break;
//                }
//            }
//            param.Add("id_create", GetLoginInfo<long>("id_user"));
//            PageList<Info_Query> lst = GetPageData(param, 1);
//            if (Request.IsAjaxRequest())
//            {
//                return PartialView("_ListControl", lst);
//            }
//            //获取公告类型
//            param.Clear();
//            param.Add("id_master", GetLoginInfo<long>("id_user_master"));
//            BaseResult br = BusinessFactory.InfoType.GetAll(param);
//            ViewData["InfoType"] = br.Data;
//            return View(lst);
//        }

//        /// <summary>
//        /// 获取数据源
//        /// mq 2016-05-27 修改 排除升级，系统，业务类型通知
//        /// </summary>
//        /// <param name="param"></param>
//        /// <param name="flag">0 查询我的通知 1查询我的公告</param>
//        /// <returns></returns>
//        private PageList<Info_Query> GetPageData(Hashtable param, int flag)
//        {
//            int limit = 8;
//            PageList<Info_Query> lst = new PageList<Info_Query>(limit);
//            //页码
//            int pageIndex = param.ContainsKey("pageIndex") ? Convert.ToInt32(param["pageIndex"]) : 1;
//            ViewData["pageIndex"] = pageIndex;
//            param.Add("limit", limit);
//            param.Add("start", (pageIndex - 1) * limit);
//            try
//            {
//                //查询我的公告
//                if (flag == 1)
//                {
//                    ViewData["contentType"] = 1;
//                    //根据公告类型获取数据
//                    if (param.ContainsKey("id_info_type") && !param["id_info_type"].ToString().Equals("0"))
//                    {
//                        ViewData["id_info_type"] = param["id_info_type"].ToString();
//                    }
//                    else
//                    {
//                        ViewData["id_info_type"] = "0";
//                        param.Remove("id_info_type");
//                    }
//                    param.Add("sort", "id");
//                    param.Add("dir", "desc");
//                    PageNavigate pn = BusinessFactory.Info.GetPage(param);
//                    lst = new PageList<Info_Query>(pn, pageIndex, limit);
//                }
//                //我的通知
//                else
//                {
//                    string[] bm = new string[] { "update", "system", "business" };
//                    param.Add("bmList", bm);
//                    param.Add("id_user", GetLoginInfo<long>("id_user"));
//                    ViewData["contentType"] = 0;
//                    PageNavigate pn = BusinessFactory.InfoUser.GetPage(param);
//                    lst = new PageList<Info_Query>(pn, pageIndex, limit);
//                }

//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return lst;
//        }

//        /// <summary>
//        /// 查询详情
//        /// </summary>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public ActionResult NotificationItem()
//        {
//            Hashtable param = GetParameters();
//            ParamVessel p = new ParamVessel();
//            p.Add("id", 0, HandleType.ReturnMsg);
//            p.Add("flag", 1, HandleType.DefaultValue);
//            param = param.Trim(p);
//            BaseResult br = new BaseResult();

//            if (int.Parse(param["flag"].ToString()) == 1)
//            {
//                param.Add("NoticeFlag", "");
//            }
//            else
//            {
//                param.Add("id_user", GetLoginInfo<long>("id_user"));
//            }
//            param.Remove("flag");
//            br = BusinessFactory.Info.Get(param);
//            param.Clear();
//            param["bm"] = "update";
//            int update = BusinessFactory.InfoType.GetByID(param);
//            param["bm"] = "system";
//            int system = BusinessFactory.InfoType.GetByID(param);
//            ViewBag.update = update;
//            ViewBag.system = system;
//            return View(br.Data);
//        }

//        [ActionPurview(false)]
//        public ActionResult FileDownLoad()
//        {
//            Hashtable param = GetParameters();
//            ParamVessel p = new ParamVessel();
//            p.Add("pathUrl", string.Empty, HandleType.ReturnMsg);
//            param = param.Trim(p);
//            string filename = param["pathUrl"].ToString();
//            filename = filename.Substring(filename.LastIndexOf("/") + 1, filename.Length - filename.LastIndexOf("/") - 1);
//            return File(param["pathUrl"].ToString(), "application/octet-stream", Url.Encode(filename));
//        }

//        /// <summary>
//        /// 新增公告
//        /// </summary>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public ActionResult AddInfo()
//        {
//            ViewData["id_user_js"] = GetLoginInfo<long>("id_user");
//            Hashtable param = new Hashtable();
//            param["bm"] = "update";
//            int update = BusinessFactory.InfoType.GetByID(param);
//            param["bm"] = "system";
//            int system = BusinessFactory.InfoType.GetByID(param);
//            ViewBag.update = update;
//            ViewBag.system = system;
//            return View();
//        }

//        /// <summary>
//        /// 平台下的用户列表
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult UserDialogList()
//        {
//            int limit = 10;
//            string likeCon = string.Empty;
//            Hashtable param = GetParameters();
//            ParamVessel p = new ParamVessel();
//            p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码

//            //搜索关键字
//            p.Add("username", string.Empty, HandleType.Remove, true);
//            p.Add("name", string.Empty, HandleType.Remove, true);
//            p.Add("companyname", string.Empty, HandleType.Remove, true);
//            p.Add("email", string.Empty, HandleType.Remove, true);
//            p.Add("phone", string.Empty, HandleType.Remove, true);
//            param = param.Trim(p);
//            //页码索引
//            int pageIndex = Convert.ToInt32(param["pageIndex"]);
//            param.Add("have_account", 1);
//            param.Add("not_id", 1);
//            param.Add("limit", limit);
//            param.Add("start", (pageIndex - 1) * limit);
//            param.Add("dir", "asc");

//            ViewData["username"] = Convert.ToString(param["username"]).Replace('%', ' ');
//            ViewData["name"] = Convert.ToString(param["name"]).Replace('%', ' ');
//            ViewData["companyname"] = Convert.ToString(param["companyname"]).Replace('%', ' ');
//            ViewData["email"] = Convert.ToString(param["email"]).Replace('%', ' ');
//            ViewData["phone"] = Convert.ToString(param["phone"]).Replace('%', ' ');
//            ViewData["pageIndex"] = pageIndex;
//            PageList<Tb_User_Query> lst = new PageList<Tb_User_Query>(limit);
//            try
//            {
//                PageNavigate pn = BusinessFactory.Account.QueryUserPage(param);

//                lst = new PageList<Tb_User_Query>(pn, pageIndex, limit);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//            return PartialView("_UserDialogControl", lst);


//        }

//        /// <summary>
//        /// 获取所有员工
//        /// 2015-6-26 wzp
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult GetAllYg()
//        {
//            Hashtable param = new Hashtable();
//            BaseResult br = new BaseResult();
//            param.Add("have_account", 1);
//            param.Add("fatherId", GetLoginInfo<long>("id_user_master"));
//            try
//            {
//                br = BusinessFactory.InfoUser.GetAll(param);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);

//        }

//        /// <summary>
//        /// 保存公告信息 2015-6-24 wzp
//        /// 发送对象：1代表客户（每个客户中存在多个用户），2代表员工
//        /// 特殊处理（自定义发送对象）：发送对象（sendObj）以字符串数组存储，对其中的每个字符串进行验证：
//        /// 1开头：1,客户Id
//        /// 2开头：2,员工Id
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        [ActionPurview(false)]
//        [ValidateInput(false)]
//        public ActionResult Save()
//        {
//            StringBuilder kh = new StringBuilder();//客户
//            StringBuilder yg = new StringBuilder();//员工
//            string khStr = string.Empty;
//            string ygStr = string.Empty;
//            BaseResult br = new BaseResult();
//            Hashtable param = GetParameters();
//            ParamVessel p = new ParamVessel();
//            p.Add("Title", string.Empty, HandleType.ReturnMsg);
//            p.Add("content", string.Empty, HandleType.ReturnMsg);
//            p.Add("id_info_type", (long)0, HandleType.ReturnMsg);
//            p.Add("filename", string.Empty, HandleType.DefaultValue);
//            p.Add("fileSize", string.Empty, HandleType.DefaultValue);
//            p.Add("id_gys", (long)-1, HandleType.DefaultValue);//全部客户
//            p.Add("id_gys_yg", (long)0, HandleType.Remove);//全部员工
//            p.Add("all_account", string.Empty, HandleType.Remove);//所有用户
//            //发送对象
//            p.Add("sendList[]", null, HandleType.Remove);
//            param = param.Trim(p);

//            //判断是否是自定义发送对象
//            if (param.ContainsKey("sendList[]"))
//            {
//                param.Remove("id_gys");
//                param.Remove("id_gys_yg");
//                string strLst = param["sendList[]"].ToString();
//                string[] lst = strLst.Split(',');
//                string[] val = new string[] { };
//                foreach (string str in lst)
//                {
//                    val = str.Split('-');
//                    //发送给客户
//                    if (val[0].Equals("1"))
//                    {
//                        kh.Append(val[1] + ",");//获取客户（采购商）Id集合
//                    }
//                    //发送给员工
//                    else
//                    {
//                        yg.Append(val[1] + ",");//获取员工Id集合
//                    }
//                }
//                khStr = kh.ToString();
//                ygStr = yg.ToString();
//                khStr = khStr.Length > 0 ? khStr.Substring(0, khStr.Length - 1) : "-1";
//                ygStr = ygStr.Length > 0 ? ygStr.Substring(0, ygStr.Length - 1) : "-1";
//                param.Add("id_user_master", khStr);
//                param.Add("id_account", ygStr);
//                param.Remove("sendList[]");
//            }
//            //（平台管理员）发送全部用户
//            else if (param.ContainsKey("all_account"))
//            {
//                if (param.ContainsKey("id_gys")) param.Remove("id_gys");
//                if (param.ContainsKey("id_gys_yg")) param.Remove("id_gys_yg");
//                if (!param.ContainsKey("yg_flag")) param.Add("yg_flag", -1); //不插入员工数据标示
//                param["yg_flag"] = -1;
//            }
//            else
//            {
//                //全部客户
//                if (param["id_gys"].ToString().Equals("1"))
//                {
//                    param["id_gys"] = GetLoginInfo<long>("id_user_master");
//                }
//                //全部员工
//                if (param.ContainsKey("id_gys_yg") && param["id_gys_yg"].ToString().Equals("2"))
//                {
//                    param["id_gys_yg"] = GetLoginInfo<long>("id_user_master");
//                    param.Add("yg_flag", 1); //插入员工数据标示
//                }
//                else
//                {
//                    param.Add("yg_flag", -1); //不插入员工数据标示
//                }
//            }
//            try
//            {
//                param.Add("id_create", GetLoginInfo<long>("id_user"));
//                param.Add("id_master", GetLoginInfo<long>("id_user_master"));
//                param.Add("flag_from", "pc");
//                long id_info = BusinessFactory.Utilety.GetNextKey(typeof(Info));//获取下一个Id自增值
//                param.Add("id", id_info);
//                br = BusinessFactory.Info.Add(param);
//                param.Clear();
//                //更新发送数量
//                param.Add("id_info", id_info);
//                br = BusinessFactory.Info.Active(param);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        /// 删除公告
//        /// 2015-6-29 wzp
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult Delete()
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = GetParameters();
//            ParamVessel p = new ParamVessel();
//            p.Add("id", 0, HandleType.ReturnMsg);
//            param = param.Trim(p);
//            br = BusinessFactory.Info.Delete(param);
//            param.Clear();
//            param.Add("id_create", GetLoginInfo<long>("id_user"));
//            return PartialView("_ListControl", GetPageData(param, 1));
//        }

//        /// <summary>
//        /// 获取已读/未读用户
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult GetReadUser()
//        {
//            Hashtable param = GetParameters();
//            ParamVessel p = new ParamVessel();
//            p.Add("id_info", (long)0, HandleType.ReturnMsg);
//            p.Add("flag_reade", string.Empty, HandleType.Remove);
//            param = param.Trim(p);

//            BaseResult br = new BaseResult();
//            br = BusinessFactory.Info.GetAll(param);
//            return Json(br);
//        }


//        /// <summary>
//        /// 获取我的通知
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult GetInformCount()
//        {
//            Hashtable param = GetParameters();
//            ParamVessel p = new ParamVessel();
//            p.Add("flag_reade", string.Empty, HandleType.ReturnMsg);
//            param = param.Trim(p);
//            param.Add("id_user", GetLoginInfo<long>("id_user"));
//            BaseResult br = new BaseResult();
//            try
//            {
//                PageNavigate pn = BusinessFactory.InfoUser.GetPage(param);
//                //ViewData["context.InformCount"] = pn.TotalCount;//未读总数量
//                //ViewData["context.Inform"] = lst;
//                br.Data = (IList<CySoft.Model.Tb.Info_Query>)pn.Data ?? new List<CySoft.Model.Tb.Info_Query>();//未读消息
//                br.Success = true;
//                return Json(br);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        /// <summary>
//        /// 我的公告
//        /// </summary>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public ActionResult ListToAdv()
//        {
//            Hashtable param = GetParameters();
//            ParamVessel p = new ParamVessel();
//            p.Add("pageIndex", (long)1, HandleType.DefaultValue);
//            param = param.Trim(p);
//            if (param.ContainsKey("id_gys"))
//            {
//                param.Remove("id_gys");
//            }
//            param.Add("id_create", GetLoginInfo<long>("id_user"));
//            PageList<Info_Query> lst = GetPageData(param, 1);
//            return PartialView("ListToAdv", lst);
//        }
//    }
//}