using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Utility.Mvc.Html;

namespace CySoft.Controllers.GoodsCtl
{
    [LoginActionFilter]
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class GoodsRecommendController : BaseController
    {
        public ActionResult Add()
        {
            ViewData["id_gys"] = ((Hashtable)Session["LoginInfo"])["id_supplier"];
            return View();
        }

        [HttpPost]
        [ActionPurview(false)]
        public ActionResult FrameList()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("orderby", 1, HandleType.DefaultValue);//排序
                p.Add("keyword", string.Empty, HandleType.Remove, true);//搜素关键字
                param = param.Trim(p);
                switch (param["orderby"].ToString())
                {
                    case "2":
                        param.Add("sort", "rq_create");
                        param.Add("dir", "asc");
                        break;
                    default:
                        param["orderby"] = 1;
                        param.Add("sort", "rq_create");
                        param.Add("dir", "desc");
                        break;
                }


                ViewData["keyword"] = param["keyword"];
                ViewData["orderby"] = param["orderby"];
                param.Add("flag_state", 1);
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                param.Add("flag_stop", 0);
                br = BusinessFactory.BuyerAttention.Get(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return View(br.Data);

            return PartialView("_FrameListControl", br.Data);
        }

        [HttpPost]
        [ActionPurview(false)]
        public ActionResult GetGoods()
        {
            BaseResult br = new BaseResult();

            try
            {
                Hashtable param = base.GetParameters();

                param.Add("up", 1);//是否上架
                param.Add("stop", 0);//是否启用
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                
                //商品分类为-1时默认查询全部
                if (param["id_spfl"].ToString() == "-1")
                {
                    param.Remove("id_spfl");
                }

                br = BusinessFactory.Goods.GetAll(param);
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

        [HttpPost]
        [ActionPurview(false)]
        public ActionResult Submit()
        {

            BaseResult br = new BaseResult();

            try
            {
                Hashtable param = base.GetParameters();

                var skuIds = param["skuIds"].ToString().Trim();
                var gysIds = param["gysIds"].ToString().Trim();
                var txtReason = param["txtReason"].ToString().Trim();

                if (string.IsNullOrEmpty(skuIds))
                {
                    br.Success = false;
                    br.Message.Add("<h5>请至少选择一个推荐商品！</h5>");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }

                if (string.IsNullOrEmpty(gysIds))
                {
                    br.Success = false;
                    br.Message.Add("<h5>请选择要推荐商品供应商！</h5>");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }

                if (string.IsNullOrEmpty(txtReason))
                {
                    br.Success = false;
                    br.Message.Add("<h5>请填写推荐理由！</h5>");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }

                var id_gys_push = GetLoginInfo<long>("id_supplier");

                IList<Tb_Sp_Sku_Push> list = new List<Tb_Sp_Sku_Push>();
                var skuIdArray = skuIds.Split(',');
                for (int i = 0; i < skuIdArray.Length; i++)
                {
                    list.Add(new Tb_Sp_Sku_Push
                    {
                        id = Guid.NewGuid(),
                        id_gys_push = id_gys_push,
                        id_gys = long.Parse(gysIds),
                        id_sku = long.Parse(skuIdArray[i]),
                        pushreason = txtReason,
                        falg_state = (short)GoodsRecommendFlag.CheckPending
                    });
                }

                br = BusinessFactory.GoodsRecommend.Add(list);
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

        public ActionResult CheckList()
        {
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<SpSkuPushData> list = new PageList<SpSkuPushData>(limit);
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("pageIndex", 1, HandleType.DefaultValue); //当前页码
                param = param.Trim(p);

                int pageIndex = Convert.ToInt32(param["pageIndex"]);

                ViewData["pageIndex"] = pageIndex;
                ViewData["limit"] = limit;

                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                long id_gys = GetLoginInfo<long>("id_supplier");
                param.Add("id_gys", id_gys);

                ViewData["id_gys"] = id_gys;

                param.Add("falg_state", GoodsRecommendFlag.CheckPending);
                param.Add("sort", "rq_create");
                param.Add("dir", "desc");
                pn = BusinessFactory.GoodsRecommend.GetPage(param);
                list = new PageList<SpSkuPushData>(pn, pageIndex, limit);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(list);
        }

        [ActionPurview(false)]
        [ValidateInput(false)]
        public ActionResult Item(long id)
        {
            BaseResult br = new BaseResult();
            try
            {

                Hashtable param = base.GetParameters();

                param.Add("id", id);

                br = BusinessFactory.Goods.Get(param);
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


        [HttpPost]
        [ActionPurview(false)]
        public ActionResult Check()
        {
            BaseResult br = new BaseResult();

            try
            {
                var param = GetParameters();

                if (!param.ContainsKey("checkState"))
                {
                    br.Message.Add("非法提交！");
                    br.Level = ErrorLevel.Error;
                    br.Success = false;
                    return Json(br);
                }
                else
                {
                    int checkState = Convert.ToInt32(param["checkState"]);

                    var info = new Tb_Sp_Sku_Push
                    {
                        id = new Guid(param["id"].ToString()),
                        id_sh = GetLoginInfo<long?>("id_user")
                    };

                    if (checkState == 1)
                    {
                        if (param["fl"] == null)
                        {
                            br.Message.Add( "商品类别不能为空！");
                            br.Level = ErrorLevel.Warning;
                            br.Success = false;
                            return Json(br);
                        }

                        if (param["bm"] == null)
                        {
                            br.Message.Add("商品编码不能为空！");
                            br.Level = ErrorLevel.Warning;
                            br.Success = false;
                            return Json(br);
                        }

                        if (param["dhs"] == null)
                        {
                            br.Message.Add("最小订货数不能为空！");
                            br.Level = ErrorLevel.Warning;
                            br.Success = false;
                            return Json(br);
                        }

                        if (param["dhj"] == null)
                        {
                            br.Message.Add("订货价不能为空！");
                            br.Level = ErrorLevel.Warning;
                            br.Success = false;
                            return Json(br);
                        }

                        info.id_spfl = long.Parse(param["fl"].ToString());
                        info.bm_Interface = param["bm"].ToString();
                        info.sl_dh_min = decimal.Parse(param["dhs"].ToString());
                        info.dj_dh = decimal.Parse(param["dhj"].ToString());
                        info.falg_state = (short?)GoodsRecommendFlag.Pass;

                        br = BusinessFactory.GoodsRecommend.Pass(info);

                    }
                    else if(checkState==2)
                    {
                        //审核不通过
                        var nopass = param["nopass"];

                        if (nopass == null || string.IsNullOrEmpty(nopass.ToString().Trim()))
                        {
                            br.Message.Add("【不通过】意见不能为空！");
                            br.Success = false;
                            br.Level = ErrorLevel.Warning;
                            return Json(br);
                        }

                        info.refusereason = param["nopass"].ToString().Trim();
                        info.falg_state = (short)GoodsRecommendFlag.NoPass;

                        br = BusinessFactory.GoodsRecommend.NoPass(info);
                    }
                    else if (checkState == 3)
                    {
                        info.falg_state = (short)GoodsRecommendFlag.invalid;

                        br = BusinessFactory.GoodsRecommend.Invalid(info);
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
    }
}
