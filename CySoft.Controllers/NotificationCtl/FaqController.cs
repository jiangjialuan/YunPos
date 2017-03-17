//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using CySoft.Controllers.Filters;
//using System.Web.Mvc;
//using System.Web.UI;
//using CySoft.Controllers.Base;
//using CySoft.Frame.Core;
//using CySoft.Model.Tb;
//using System.Collections;
//using CySoft.Utility.Mvc.Html;

//#region 反馈管理
//#endregion

//namespace CySoft.Controllers.NotificationCtl
//{
//    [LoginActionFilter]
//    [ValidateInput(false)]
//    [OutputCache(Location = OutputCacheLocation.None)]
//    public class FaqController : BaseController
//    {
//        /// <summary>
//        /// 客户反馈列表,返回的树状结构数据
//        /// znt 2015-05-07
//        /// </summary>
//        [ActionPurview(false)]
//        public ActionResult Client()
//        {
//            int totalCount = 0;
//            int anwserCount = 0;
//            bool b = false;
//            Hashtable param = GetParameters();
//            ParamVessel p = new ParamVessel();

//            p.Add("pageIndex", 1, HandleType.DefaultValue);
//            p.Add("likeContents", string.Empty, HandleType.DefaultValue);
//            p.Add("start_rq_create", string.Empty, HandleType.DefaultValue);
//            p.Add("end_rq_create", string.Empty, HandleType.DefaultValue);
//            p.Add("state", string.Empty, HandleType.Remove);
//            p.Add("isAnwser", -1, HandleType.DefaultValue);
//            param = param.Trim(p);
//            //搜索条件
//            ViewData["isAnwser"] = param["isAnwser"];
//            ViewData["likeContents"] = param["likeContents"];
//            ViewData["start_rq_create"] = param["start_rq_create"];
//            ViewData["end_rq_create"] = param["end_rq_create"];
            
//            param.Add("isKeFu", 1);
//            param.Add("fatherId", 0);
//            param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
//            //搜索内容
//            if (!param["likeContents"].ToString().Equals(""))
//            {
//                param["likeContents"] = "%" + param["likeContents"].ToString() + "%";
//            }
//            else
//            {
//                param.Remove("likeContents");
//            }
//            //搜索日期
//            if (!param["start_rq_create"].ToString().Equals("") && !param["end_rq_create"].ToString().Equals(""))
//            {
//                b = true;
//            }
//            else
//            {
//                param.Remove("start_rq_create");
//                param.Remove("end_rq_create");
//            }
//            totalCount = GetPageCount(param);//查询总条数
//            param.Add("state1", 0);
//            anwserCount = GetPageCount(param);//查询已回复条数
//            ViewData["totalCount"] = totalCount;
//            ViewData["anwserCount"] = anwserCount;
//            ViewData["unCount"] = totalCount - anwserCount;
//            param.Remove("state1");
//            if (param.ContainsKey("isAnwser"))
//            {
//                if (param["isAnwser"].ToString().Equals("0"))
//                {
//                    param.Add("state0", 0);
//                    totalCount = totalCount - anwserCount;
//                }
//                else if (param["isAnwser"].ToString().Equals("1"))
//                {
//                    param.Add("state1", 0);
//                    totalCount = anwserCount;
//                }
//                param.Remove("isAnwser");
//            }
//            if (b)
//            {
//                param.Add("byDate", "");
//            }
//            param.Remove("fatherId");
//            param.Remove("id_user_master");
//            if (Request.IsAjaxRequest())
//            {
//                return PartialView("_FaqControl", GetPageData(param, totalCount, "Client"));
//            }
//            else
//            {
//                return View(GetPageData(param, totalCount, "Client"));
//            }
//        }
        
//        /// <summary>
//        /// 供应商 回复客户
//        /// znt 2015-05-15
//        /// </summary>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult ReplyClient(Faq model)
//        {
            
//            long id =(long) model.fatherId;//记录问题Id
//            BaseResult br = new BaseResult();
//            if (string.IsNullOrEmpty(model.contents))
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("请输入提交的内容！"));
//                return Json(br);
//            }
//            else if (model.contents.Length > 2000)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("提交内容请限定在2000个字内！"));
//                return Json(br);
//            }
//            if (model.fatherId==0 || model.id_user==0)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("系统繁忙，请稍后重试！"));
//                return Json(br);
//            }
//            try
//            {
//                model.id = BusinessFactory.Utilety.GetNextKey(typeof(Faq));
//                model.id_user_master = GetLoginInfo<long>("id_user_master");
//                model.flag_state = 1; // 1 已回复
//                model.flag_type = 1; // 1 记录为回复内容
//                model.flag_delete = 0; // 0 未删除
//                model.flag_from = "pc";
//                model.id_user_receive = GetLoginInfo<long>("id_user");//回答人
//                br = BusinessFactory.Faq.ReplyClient(model);                
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
//        ///  供应商反馈
//        ///  znt 2015-05-07
//        ///  wzp修改：2015-5-28
//        /// </summary>
//        [ActionPurview(false)]
//        public ActionResult Supplier()
//        {
//            Hashtable param = GetParameters();
//            int totalCount = 0;
//            int anwserCount = 0;
//            try
//            {
//                ViewData["id_gys"] = param.ContainsKey("id_gys") ? param["id_gys"].ToString() : "0";
//                ViewData["isAnwser"] = param.ContainsKey("isAnwser") ? param["isAnwser"].ToString() : "-1";
//                param = Get_cgsPageParam(param, 0);
//                //查询总条数（只通过供应商来确定总数目,不考虑状态）
//                totalCount = GetPageCount(param);
//                //查询已回复条数
//                param.Add("state1", 0);
//                anwserCount = GetPageCount(param);
//                param.Remove("state1");

//                ViewData["totalCount"] = totalCount;
//                ViewData["anwserCount"] = anwserCount;
//                ViewData["unCount"] = totalCount - anwserCount;
//                param.Clear();
//                param = GetParameters();
//                param = Get_cgsPageParam(param, 1);
//                //根据状态的条件确定查询分页内容的记录数
//                if (param.ContainsKey("isAnwser"))
//                {
//                    if (param["isAnwser"].ToString().Equals("0"))
//                    {
//                        totalCount = totalCount - anwserCount;
//                        param.Add("state0", 0);
//                    }
//                    else if (param["isAnwser"].ToString().Equals("1"))
//                    {
//                        totalCount = anwserCount;
//                        param.Add("state1", 0);
//                    }
//                }
//                string name = "Supplier";
//                if (param.ContainsKey("id_user_master"))
//                {
//                    name = "id_userAndMaster";
//                }
//                if (Request.IsAjaxRequest())
//                {
//                    return PartialView("_FaqControl", GetPageData(param, totalCount, name));
//                }
//                else
//                {
//                    return View(GetPageData(param, totalCount, name));
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
//        }

//        /// <summary>
//        /// 客服反馈（平台反馈）(获取所有反馈到平台的信息内容)
//        /// 修改 wzp:2015-6-2
//        /// </summary>
//        /// <returns></returns>
//        [ActionPurview(false)]
//        public ActionResult Platform()
//        {
//            Hashtable param = GetParameters();
//            int totalCount = 0;
//            int anwserCount = 0;
//            long id_user = GetLoginInfo<long>("id_user");
//            //判断是否是平台管理员
//            if (id_user != 1)
//            {
//                param.Add("id_user", id_user);
//            }
//            //记录id_user的角色（此处只在意是否是平台管理员）
//            ViewData["id_user_js"] = id_user;
//            param.Add("id_user_master", 1);
//            param.Add("fatherId", 0);
//            try
//            {
//                //查询总条数
//                totalCount = GetPageCount(param);
//                //查询已回复条数
//                param.Add("state1", 0);
//                anwserCount = GetPageCount(param);

//                param.Remove("state1");
//                param.Remove("fatherId");

//                ViewData["totalCount"] = totalCount;
//                ViewData["anwserCount"] = anwserCount;
//                ViewData["unCount"] = totalCount - anwserCount;
//                //根据状态的条件确定查询分页内容的记录数
//                if (param.ContainsKey("isAnwser"))
//                {
//                    if (param["isAnwser"].ToString().Equals("0"))
//                    {
//                        totalCount = totalCount - anwserCount;
//                        param.Add("state0", 0);
//                    }
//                    else if (param["isAnwser"].ToString().Equals("1"))
//                    {
//                        totalCount = anwserCount;
//                        param.Add("state1", 0);
//                    }
//                    ViewData["isAnwser"] = param["isAnwser"].ToString();
//                }
//                else
//                {
//                    ViewData["isAnwser"] = -1;
//                }
               
//                param.Remove("fatherId");
//                //判定请求方式
//                if (Request.IsAjaxRequest())
//                {
//                    return PartialView("_FaqControl", GetPageData(param, totalCount, "Platform"));
//                }
//                else
//                {
//                    return View(GetPageData(param, totalCount, "Platform"));
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
//        }

//        /// <summary>
//        ///  新增反馈到平台
//        ///  wzp修改 2015-7-28 
//        /// </summary>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult AddToPlatform(Faq model)
//        {
//            BaseResult br = new BaseResult();
//            long fatherId =long.Parse(model.fatherId.ToString());
//            if (string.IsNullOrEmpty(model.contents))
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("请输入提交的内容！"));
//                return Json(br);
//            }
//            else if (model.contents.Length > 2000)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("提交内容请限定在2000个字内！"));
//                return Json(br);
//            }
//            try
//            {
//                model.id = BusinessFactory.Utilety.GetNextKey(typeof(Faq));
//                //客户提交继续反馈
//                if (model.id_user == 0)
//                {
//                    model.id_user = GetLoginInfo<long>("id_user");
//                    model.flag_state = 0; // 0 未回复
//                    model.flag_type = 0;
//                }
//                //平台回复客户
//                else
//                {
//                    model.flag_state = 1; // 0 未回复
//                    model.flag_type = 1;
//                }
//                model.id_user_master =1;
//                model.flag_delete = 0; // 0 未删除
//                model.id_user_receive = 0; // 0 默认到平台
//                model.flag_from = "pc";
//                br=BusinessFactory.Faq.Add(model);
//                int state = Convert.ToInt32(model.flag_state);
//                //添加反馈成功后获取最新的数据源集合
//                if (br.Success)
//                {
//                    if (fatherId > 0)
//                    {
//                        br = UpdateFaqState(fatherId, state);
//                    }                   
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
//            return Json(br);
//        }

//        /// <summary>
//        ///  新增反馈到供应商
//        ///  znt 2015-05-11
//        /// </summary>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult AddToSupplier()
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param =GetParameters();
//            Faq model = new Faq();
//            if (String.IsNullOrEmpty(param["contents"].ToString()))
//            {
//                br.Message.Add(string.Format("反馈内容不允许为空！"));
//                br.Success = false;
//                return Json(br);
//            }
//            else if (param["contents"].ToString().Length > 2000)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("提交内容请限定在2000个字内！"));
//                return Json(br);
//            }
//            model.contents = param["contents"].ToString();
//            param.Remove("contents");
//            bool b = false;
//            long fatherId = 0;
//            long id_gys = 0;
//            //判断是否为继续反馈
//            if (param.ContainsKey("contain"))
//            {
//                ParamVessel p = new ParamVessel();
//                b = true;
//                fatherId = long.Parse(param["fatherId"].ToString());
//                p.Add("fatherId", 0, HandleType.ReturnMsg);
//                p.Add("id_user_master", 0, HandleType.ReturnMsg);
//                param = param.Trim(p);

//                model.fatherId = fatherId;
//                model.id_user_master = long.Parse(param["id_user_master"].ToString());
//                param.Remove("contain");
//            }
//            else
//            {
//                id_gys = long.Parse(param["id"].ToString());
//                //根据供应商ID获取供应商对象
//                br = BusinessFactory.Supplier.Get(param);
//                if (br.Data == null)
//                {
//                    br.Success = false;
//                    br.Message.Add(string.Format("不存在该供应商或供应商资料有误，请检查后重试！"));
//                    return Json(br);
//                }
//                Tb_Gys gys = (Tb_Gys)br.Data;
//                model.flag_state = 0;
//                model.fatherId = 0;
//                model.id_user_master = gys.id_user_master;
//                param.Remove("id");
//            }
//            try
//            {
//                model.id = BusinessFactory.Utilety.GetNextKey(typeof(Faq));
//                model.id_user = GetLoginInfo<long>("id_user");
//                model.id_user_receive = 0;
//                model.flag_delete = 0; // 0 未删除
//                model.flag_from = "pc";
//                br = BusinessFactory.Faq.Add(model);
//                if (!br.Success)
//                {
//                    return Json(br);
//                }
//                if (br.Success && b)
//                {
//                    br= UpdateFaqState(fatherId, 0);                    
//                }
//                return Json(br);
                
//            }
//            catch (CySoftException ex)
//            {

//                throw ex;
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }

//        /// <summary>
//        /// 采购商-》已解决问题
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult Resolved()
//        {
//            BaseResult br = new BaseResult();
//            ParamVessel p = new ParamVessel();
//            p.Add("fatherId", (long)0, HandleType.ReturnMsg);
            
//            Hashtable param = GetParameters();
//            param = param.Trim(p);
//            param.Add("orFatherId",param["fatherId"]);
//            param.Add("new_flag_state", 2);
//            param.Remove("fatherId");
//            br = BusinessFactory.Faq.Update(param);
//            return Json(br);
//        }

//        #region 方法

//        /// <summary>
//        /// 获取分页结果集
//        /// </summary>
//        /// <param name="param"></param>
//        /// <returns></returns>
//        public PageList<Faq_Tree> GetPageData(Hashtable param, int count, string actionName)
//        {
//            int limit = 10;
//            ParamVessel p = new ParamVessel();
//            PageList<Faq_Tree> lst = new PageList<Faq_Tree>(limit);

//            p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
//            int pageIndex = Convert.ToInt32(param["pageIndex"]);
//            pageIndex = !(pageIndex > 0) ? 1 : pageIndex;
//            PageNavigate pn = new PageNavigate();
//            switch (actionName)
//            {
//                case "Client":
//                    param.Add("id_user_masterFlag", GetLoginInfo<long>("id_user_master"));
//                    break;
//                case "Supplier":
//                    param.Add("id_userFlag", GetLoginInfo<long>("id_user"));
//                    break;
//                case "id_userAndMaster":
//                    param.Add("id_user", GetLoginInfo<long>("id_user"));
//                    break;
//                case "Platform":
//                    if (param.ContainsKey("id_user"))
//                    {
//                        param.Add("id_userAndMaster", "");
//                    }
//                    else
//                    {
//                        param.Add("id_user_masterFlag", 1);
//                    }
//                    break;
//            }

//            param.Add("limit", limit);
//            param.Add("start", (pageIndex - 1) * limit);
//            param.Add("sort", "id");
//            param.Add("dir", "desc");
//            pn = BusinessFactory.Faq.GetPage(param);
//            pn.TotalCount = count;
//            lst = new PageList<Faq_Tree>(pn, pageIndex, limit);
//            return lst;
//        }

//        /// <summary>
//        /// 获取总记录数
//        /// </summary>
//        /// <param name="param"></param>
//        /// <returns></returns>
//        private int GetPageCount(Hashtable param)
//        {
//            try
//            {
//                BaseResult br = BusinessFactory.Faq.GetCount(param);
//                return Convert.ToInt32(br.Data);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        /// <summary>
//        /// 获取查询条件参数（采购商/客户角色）
//        /// </summary>
//        /// <param name="param"></param>
//        /// <param name="flag">0 获取分页总条数的参数 1获取分页数据源参数</param>
//        /// <returns></returns>
//        private Hashtable Get_cgsPageParam(Hashtable param, int flag)
//        {
//            //当选择供应商过滤时
//            if (param.ContainsKey("id_gys") && long.Parse(param["id_gys"].ToString()) != 0)
//            {
//                BaseResult br = new BaseResult();
//                param.Add("id", param["id_gys"]);
//                //根据供应商ID获取供应商对象
//                br = BusinessFactory.Supplier.Get(param);
//                if (br.Data == null)
//                {
//                    br.Success = false;
//                    br.Message.Add(string.Format("不存在该供应商或供应商资料有误，请检查后重试！"));
//                    return null;
//                }
//                Tb_Gys gys = (Tb_Gys)br.Data;

//                if (flag == 1)
//                {
//                    param.Add("id_userAndMaster", "");
//                }
//                param.Add("id_user_master", (long)gys.id_user_master);
//                param.Remove("id_gys");
//                param.Remove("id");
//            }
//            if (flag == 0)
//            {
//                param.Add("fatherId", 0);
//                param.Add("id_user", GetLoginInfo<long>("id_user"));
//            }
//            param.Add("isKeFu", 1);
//            return param;
//        }

//        /// <summary>
//        /// 更改问题主记录Id的状态
//        /// </summary>
//        /// <param name="fatherId">问题主记录Id</param>
//        /// <param name="stateId">0未回复 1已回复</param>
//        /// <returns></returns>
//        private BaseResult UpdateFaqState(long fatherId, int stateId)
//        {
//            Hashtable param = new Hashtable();
//            param.Add("id", fatherId);
//            param.Add("new_flag_state", stateId);
//            BaseResult br = new BaseResult();
//            br = BusinessFactory.Faq.Update(param);
//            return br;
//        }     

//        #endregion
//    }
//}
