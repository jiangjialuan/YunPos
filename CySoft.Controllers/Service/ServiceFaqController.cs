using System;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Utility;
using CySoft.Model.Tb;
using CySoft.Controllers.Base;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Utility.Mvc.Html;

#region 反馈管理接口
#endregion

namespace CySoft.Controllers.Service
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceFaqController : ServiceBaseController
    {
        /// <summary>
        /// 客户反馈列表(返回的是非树状结构数据，与PC端不同)
        /// 修改：wzp 2015-6-30
        /// </summary>
        [HttpPost]
        public ActionResult Client(string obj)
        {
            Hashtable param = JSON.Deserialize<Hashtable>(obj);
            ParamVessel p = new ParamVessel();
            try
            {
                p.Add("pageIndex", (long)1, HandleType.DefaultValue);
                p.Add("flag_state", string.Empty, HandleType.Remove);
                param = param.Trim(p);
                param.Add("isKeFu", 1);//过滤客服数据
                param.Add("fatherId", 0);
                param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(GetPageData(param));
        }

        /// <summary>
        /// 供应商 回复客户
        /// znt 2015-05-15
        /// </summary>
        [HttpPost]
        public ActionResult ReplyClient(string obj)
        {
            Hashtable param = JSON.Deserialize<Hashtable>(obj);
            ParamVessel p = new ParamVessel();
            p.Add("fatherId", (long)0, HandleType.ReturnMsg);
            p.Add("contents", string.Empty, HandleType.ReturnMsg);
            p.Add("id_user",(long)0,HandleType.ReturnMsg);
            param = param.Trim(p);
            BaseResult br = new BaseResult();
            Faq model = new Faq();
            try
            {
                model.fatherId = long.Parse(param["fatherId"].ToString());
                model.contents = param["contents"].ToString();
                model.id_user = long.Parse(param["id_user"].ToString());
                model.id = BusinessFactory.Utilety.GetNextKey(typeof(Faq));
                model.id_user_master = GetLoginInfo<long>("id_user_master");
                model.flag_state = 1; // 1 已回复
                model.flag_type = 1; // 1 记录为回复内容
                model.flag_delete = 0; // 0 未删除
                model.flag_from = "android";
                model.id_user_receive = GetLoginInfo<long>("id_user");//回答人
                br = BusinessFactory.Faq.ReplyClient(model);
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
        ///  供应商反馈
        ///  znt 2015-05-07
        ///  wzp修改：2015-5-28
        /// </summary>
        [HttpPost]
        public ActionResult Supplier(string obj)
        {
            BaseResult br = new BaseResult();
            Hashtable param = JSON.Deserialize<Hashtable>(obj);            
            try
            {
                //根据供应商获取反馈信息
                if (param.ContainsKey("id_gys") && int.Parse(param["id_gys"].ToString())!=0)
                {
                    param.Add("id", param["id_gys"]);
                    param.Remove("id_gys");
                    br = BusinessFactory.Supplier.Get(param);
                    if (br.Data == null)
                    {
                        br.Success = false;
                        br.Message.Add(string.Format("不存在该供应商或供应商资料有误，请检查后重试！"));
                        return null;
                    }
                    Tb_Gys gys = (Tb_Gys)br.Data;                    
                    param.Add("id_user_master", (long)gys.id_user_master);
                    param.Remove("id");
                }
                ParamVessel p = new ParamVessel();
                p.Add("pageIndex", (long)1, HandleType.DefaultValue);
                p.Add("id_user_master", string.Empty, HandleType.Remove);
                p.Add("flag_state", string.Empty, HandleType.Remove);
                param = param.Trim(p);
                param.Add("isKeFu", 1);//过滤客服数据
                param.Add("fatherId", 0);
                param.Add("id_user", GetLoginInfo<long>("id_user"));
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(GetPageData(param));
        }

        /// <summary>
        /// 客服反馈（平台反馈）(获取所有反馈到平台的信息内容)
        /// 修改 wzp:2015-6-2
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Platform(string obj)
        {
            Hashtable param = JSON.Deserialize<Hashtable>(obj);
            ParamVessel p = new ParamVessel();
            try
            {
                p.Add("pageIndex", (long)1, HandleType.DefaultValue);
                p.Add("flag_state", string.Empty, HandleType.Remove);
                param = param.Trim(p);
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("id_user_master", 1);
                param.Add("fatherId", 0);               
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Json(GetPageData(param));
        }

        /// <summary>
        /// 根据父节点Id获取整个反馈内容
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ActionResult GetFaqInfo(string obj)
        {
            BaseResult br = new BaseResult();
            Hashtable param = JSON.Deserialize<Hashtable>(obj);
            ParamVessel p = new ParamVessel();
            try
            {
                p.Add("orFatherId", (long)0, HandleType.ReturnMsg);
                p.Add("id_user", string.Empty, HandleType.Remove);
                p.Add("id_user_master", string.Empty, HandleType.Remove);
                param = param.Trim(p);
                br=BusinessFactory.Faq.GetAll(param);
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
        ///  新增反馈到平台
        ///  2015-7-9 wzp修改
        /// </summary>
        [HttpPost]
        public ActionResult AddToPlatform(string obj)
        {
            BaseResult br = new BaseResult();
            Hashtable param = JSON.Deserialize<Hashtable>(obj);
            long fatherId =param.ContainsKey("fatherId")? long.Parse(param["fatherId"].ToString()):0;
            string content = param.ContainsKey("contents") ? param["contents"].ToString() : "";
            if (string.IsNullOrEmpty(content))
            {
                br.Success = false;
                br.Message.Add(string.Format("请输入提交的内容！"));
                return Json(br);
            }
            else if (content.Length > 2000)
            {
                br.Success = false;
                br.Message.Add(string.Format("提交内容请限定在2000个字内！"));
                return Json(br);
            }
            try
            {
                Faq model = new Faq();
                model.fatherId = fatherId;
                model.contents = content;
                model.id = BusinessFactory.Utilety.GetNextKey(typeof(Faq));
                model.id_user = GetLoginInfo<long>("id_user");
                model.id_user_master = 1;
                model.flag_state = 0; // 0 未回复
                model.flag_delete = 0; // 0 未删除
                model.id_user_receive = 0; // 0 默认到平台
                model.flag_from = "android";
                br = BusinessFactory.Faq.Add(model);
                if (br.Success)
                {
                    if (fatherId > 0)
                    {
                        br = UpdateFaqState(fatherId, 0);
                    }
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
        ///  新增反馈到供应商
        ///  znt 2015-05-11
        /// </summary>
        [HttpPost]
        public ActionResult AddToSupplier(string obj)
        {
            BaseResult br = new BaseResult();
            Hashtable param = JSON.Deserialize<Hashtable>(obj);
            Faq model = new Faq();
            if (String.IsNullOrEmpty(param["contents"].ToString()))
            {
                br.Message.Add(string.Format("反馈内容不允许为空！"));
                br.Success = false;
                return Json(br);
            }
            else if (param["contents"].ToString().Length > 2000)
            {
                br.Success = false;
                br.Message.Add(string.Format("提交内容请限定在2000个字内！"));
                return Json(br);
            }
            model.contents = param["contents"].ToString();
            param.Remove("contents");
            bool b = false;
            long fatherId = 0;
            long id_gys = 0;
            //判断是否为继续反馈
            if (param.ContainsKey("contain"))
            {
                ParamVessel p = new ParamVessel();
                b = true;
                fatherId = long.Parse(param["fatherId"].ToString());
                p.Add("fatherId", 0, HandleType.ReturnMsg);
                p.Add("id_user_master", 0, HandleType.ReturnMsg);
                param = param.Trim(p);

                model.fatherId = fatherId;
                model.id_user_master = long.Parse(param["id_user_master"].ToString());
                param.Remove("contain");
            }
            else
            {
                id_gys = long.Parse(param["id"].ToString());
                //根据供应商ID获取供应商对象
                br = BusinessFactory.Supplier.Get(param);
                if (br.Data == null)
                {
                    br.Success = false;
                    br.Message.Add(string.Format("不存在该供应商或供应商资料有误，请检查后重试！"));
                    return Json(br);
                }
                Tb_Gys gys = (Tb_Gys)br.Data;
                model.flag_state = 0;
                model.fatherId = 0;
                model.id_user_master = gys.id_user_master;
                param.Remove("id");
            }
            try
            {
                model.id = BusinessFactory.Utilety.GetNextKey(typeof(Faq));
                model.id_user = GetLoginInfo<long>("id_user");
                model.id_user_receive = 0;
                model.flag_delete = 0; // 0 未删除
                model.flag_from = "android";
                br = BusinessFactory.Faq.Add(model);
                //当继续提交反馈成功后修改主问题Id状态为0
                if (!br.Success)
                {
                    return Json(br);
                }
                if (br.Success && b)
                {
                    br = UpdateFaqState(fatherId, 0);
                }
                return Json(br);

            }
            catch (CySoftException ex)
            {

                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 采购商-》已解决问题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Resolved(string obj)
        {
            BaseResult br = new BaseResult();
            ParamVessel p = new ParamVessel();
            p.Add("fatherId", (long)0, HandleType.ReturnMsg);

            Hashtable param = JSON.Deserialize<Hashtable>(obj);
            param = param.Trim(p);
            param.Add("orFatherId", param["fatherId"]);
            param.Add("new_flag_state", 2);
            param.Remove("fatherId");
            br = BusinessFactory.Faq.Update(param);
            return Json(br);
        }

        #region 方法

        /// <summary>
        /// 获取分页结果集
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PageList<Faq_Tree> GetPageData(Hashtable param)
        {
            int limit = 15;
            PageList<Faq_Tree> lst = new PageList<Faq_Tree>(limit);
            int pageIndex = Convert.ToInt32(param["pageIndex"]);
            pageIndex = !(pageIndex > 0) ? 1 : pageIndex;
            PageNavigate pn = new PageNavigate();
            param.Add("limit", limit);
            param.Add("start", (pageIndex - 1) * limit);
            param.Add("sort", "id");
            param.Add("dir", "desc");
            pn = BusinessFactory.Faq.QueryServicePage(param);
            lst = new PageList<Faq_Tree>(pn, pageIndex, limit);
            return lst;
        }

        /// <summary>
        /// 获取查询条件参数（采购商/客户角色）
        /// </summary>
        /// <param name="param"></param>
        /// <param name="flag">0 获取分页总条数的参数 1获取分页数据源参数</param>
        /// <returns></returns>
        private Hashtable Get_cgsPageParam(Hashtable param, int flag)
        {
            //当选择供应商过滤时
            if (param.ContainsKey("id_gys") && long.Parse(param["id_gys"].ToString()) != 0)
            {
                BaseResult br = new BaseResult();
                param.Add("id", param["id_gys"]);
                //根据供应商ID获取供应商对象
                br = BusinessFactory.Supplier.Get(param);
                if (br.Data == null)
                {
                    br.Success = false;
                    br.Message.Add(string.Format("不存在该供应商或供应商资料有误，请检查后重试！"));
                    return null;
                }
                Tb_Gys gys = (Tb_Gys)br.Data;

                if (flag == 1)
                {
                    param.Add("id_userAndMaster", "");
                }
                param.Add("id_user_master", (long)gys.id_user_master);
                param.Remove("id_gys");
                param.Remove("id");
            }
            if (flag == 0)
            {
                param.Add("fatherId", 0);
                param.Add("id_user", GetLoginInfo<long>("id_user"));
            }
            param.Add("isKeFu", 1);
            return param;
        }

        /// <summary>
        /// 更改问题主记录Id的状态
        /// </summary>
        /// <param name="fatherId">问题主记录Id</param>
        /// <param name="stateId">0未回复 1已回复</param>
        /// <returns></returns>
        private BaseResult UpdateFaqState(long fatherId, long stateId)
        {
            Hashtable param = new Hashtable();
            param.Add("id", fatherId);
            param.Add("new_flag_state", stateId);
            BaseResult br = new BaseResult();
            br = BusinessFactory.Faq.Update(param);
            return br;
        }

        #endregion
    }
}
