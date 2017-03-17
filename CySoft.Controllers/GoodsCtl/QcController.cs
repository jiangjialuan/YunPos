using System;
using System.Collections;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using CySoft.Utility.Mvc.Html;

namespace CySoft.Controllers.GoodsCtl
{
    public class QcController : BaseController
    {
        #region 期初列表

        [LoginActionFilter]
        /// <summary>
        /// 期初列表
        /// </summary>
        [ValidateInput(false)]
        public ActionResult List()
        {
            #region 获取数据
            try
            {
                #region 获取参数
                PageNavigate pn = new PageNavigate();
                BaseResult br = new BaseResult();
                Hashtable param = base.GetParameters();
                #endregion

                #region 构建param
                ParamVessel p = new ParamVessel();
                p.Add("_search_", string.Empty, HandleType.Remove);
                p.Add("orderby", 0, HandleType.DefaultValue);                                       //排序
                p.Add("keyword", string.Empty, HandleType.Remove, true);                            //搜素关键字
                p.Add("page", 0, HandleType.DefaultValue);                                        //当前页码
                p.Add("pageSize", base.PageSizeFromCookie, HandleType.DefaultValue);                //每页多少条
                param = param.Trim(p);
                ViewData["keyword"] = param["keyword"];

                int page = Convert.ToInt32(param["page"]);
                int pageSize = Convert.ToInt32(param["pageSize"]);

                PageList<Tb_Shopsp_Qc_Md> list = new PageList<Tb_Shopsp_Qc_Md>(pageSize);

                #region 排序

                switch (param["orderby"].ToString())
                {
                    case "1"://条形码 正序
                        param.Add("sort", "barcode");
                        param.Add("dir", "asc");
                        break;
                    case "2"://条形码 降序
                        param.Add("sort", "barcode");
                        param.Add("dir", "desc");
                        break;
                    case "3"://名称 正序
                        param.Add("sort", "mc");
                        param.Add("dir", "asc");
                        break;
                    case "4"://名称 降序
                        param.Add("sort", "mc");
                        param.Add("dir", "desc");
                        break;
                    case "5"://单位 正序
                        param.Add("sort", "dw");
                        param.Add("dir", "asc");
                        break;
                    case "6"://单位 降序
                        param.Add("sort", "dw");
                        param.Add("dir", "desc");
                        break;
                    case "7"://期初库存数量 正序
                        param.Add("sort", "sl_qc");
                        param.Add("dir", "asc");
                        break;
                    case "8"://期初库存数量 降序
                        param.Add("sort", "sl_qc");
                        param.Add("dir", "desc");
                        break;
                    case "9"://期初库存金额 正序
                        param.Add("sort", "je_qc");
                        param.Add("dir", "asc");
                        break;
                    case "10"://期初库存金额 降序
                        param.Add("sort", "je_qc");
                        param.Add("dir", "desc");
                        break;
                    default:
                        param["orderby"] = 0;
                        param.Add("sort", "id");
                        param.Add("dir", "desc");
                        break;
                }

                #endregion 排序

                ViewData["orderby"] = param["orderby"];
                param.Remove("orderby");

                param.Add("id_masteruser", id_user_master);
                param.Add("limit", pageSize);
                param.Add("start", page * pageSize);

                param.Remove("page");
                param.Remove("pageSize");
                #endregion

                param.Add("flag_delete", (byte)CySoft.Model.Enums.Enums.FlagDelete.NoDelete);

                param.Add("id_shop", id_shop);

                pn = BusinessFactory.Tb_Shopsp_Qc_Md.GetPage(param);
                list = new PageList<Tb_Shopsp_Qc_Md>(pn, page, pageSize);

                //获取前台控制小数点
                ViewBag.DigitHashtable = GetParm();

                //数据PageList
                ViewData["PageList"] = list;
                ViewData["version"] = version;

                if (param.ContainsKey("_search_") && param["_search_"].ToString() == "1")
                {
                    return PartialView("_List");
                }
                else
                {
                    return View("List");
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
            #endregion
        }
        #endregion
    }
}
