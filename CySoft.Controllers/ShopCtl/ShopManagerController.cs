using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using CySoft.Utility.Mvc.Html;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using CySoft.Utility;
using System.IO;
using System.Data;
using System.Web;

namespace CySoft.Controllers.ShopCtl
{
    public class ShopManagerController : BaseController
    {
        /// <summary>
        /// ShopManager 设置门店
        /// lz
        /// 2017-01-04
        /// </summary>
        [ActionPurview(true)]
        public ActionResult Set()
        {
            string url = Request["url"] == null ? "" : HttpUtility.UrlDecode(Request["url"], Encoding.GetEncoding("UTF-8"));//url
            ViewData["url"] = url;

            if (Session["LoginInfo"] == null)
            {
                ViewData["is_sysmanager"] = "0";
            }
            else
            {

                var loginInfo = (Hashtable)Session["LoginInfo"];
                var checkBr = BusinessFactory.Account.CheckServiceForLogin(loginInfo);
                if (!checkBr.Success)
                {
                    if (checkBr.Level == ErrorLevel.Error)
                    {
                        ViewData["is_sysmanager"] = "0";
                    }
                    else if (checkBr.Level == ErrorLevel.Question)
                    {
                        //服务门店超过总门店数 需要管理员角色设置
                        if (is_sysmanager == "1")
                        {
                            ViewData["is_sysmanager"] = "1";
                            ViewData["shopList"] = GetShop(Enums.ShopDataType.All_State);
                            //用户管理门店
                            ViewData["userShopList"] = GetShop(Enums.ShopDataType.UserShopOnlyNone, id_user);
                            ViewData["id_shop_master"] = id_shop_master;
                            ViewData["id_shop"] = id_shop;
                            ViewData["id_user_master"] = id_user_master;
                        }
                    }
                    else
                    {
                        //服务不够 需要跳转购买才可以进行的
                        return RedirectToAction("iframe", "index", new { url = checkBr.Data.ToString() });
                    }
                }
                else
                {
                    return RedirectToAction("manager", "home");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Set(string ids)
        {
            BaseResult br = new BaseResult();
            try
            {
                if (string.IsNullOrEmpty(ids))
                {
                    br.Success = false;
                    br.Message.Add("请选择门店先");
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message),
                        level = 3
                    });
                }
                else
                {
                    #region 数据处理
                    if (ids.EndsWith(","))
                    {
                        ids = ids.Substring(0, ids.Length - 1);
                    }

                    var arr = ids.Split(',');
                    if (arr.Length <= 0)
                    {
                        br.Success = false;
                        br.Message.Add("请选择门店先");
                        return base.JsonString(new
                        {
                            status = "error",
                            message = string.Join(";", br.Message),
                            level = 3
                        });
                    }

                    if (!arr.Contains(id_shop_master))
                    {
                        ids += "," + id_shop_master;
                    }
                    #endregion
                    #region 更新开启门店设置

                    var bm = BusinessFactory.Account.GetServiceBM(version);
                    if (string.IsNullOrEmpty(bm))
                    {
                        return base.JsonString(new
                        {
                            status = "error",
                            message = "操作失败 获取服务编码异常 请检查版本是否正常！",
                            level = 3
                        });
                    }

                    Hashtable ht = new Hashtable();
                    ht.Add("id_cyuser", id_cyuser);
                    ht.Add("bm", bm);
                    ht.Add("service", "GetService");
                    ht.Add("id_masteruser", id_user_master);
                    ht.Add("rq_create_master_shop", rq_create_master_shop.ToString());

                    var cyServiceHas = BusinessFactory.Account.GetCYService(ht);
                    if (cyServiceHas == null || !cyServiceHas.ContainsKey("cyServiceList"))
                    {
                        br.Message.Clear();
                        br.Message.Add(String.Format("获取购买服务信息失败 请重试！"));
                        br.Success = false;
                        br.Level = ErrorLevel.Warning;
                        return base.JsonString(new
                        {
                            status = "error",
                            message = string.Join(";", br.Message),
                            level = 3
                        });
                    }
                    var cyServiceList = (List<Schedule_UserService>)cyServiceHas["cyServiceList"];
                    var allowNum = cyServiceList.FirstOrDefault().sl;
                    ht.Clear();
                    ht.Add("id_masteruser", id_user_master);
                    ht.Add("not_id_shop", id_shop_master);
                    ht.Add("opened_ids", ids);
                    ht.Add("allow_number", allowNum);
                    br = BusinessFactory.Tb_Shop.ResetOpenShop(ht);
                    if (br.Success)
                    {
                        //修改成功后 刷新缓存
                        var loginInfo = (Hashtable)Session["LoginInfo"];
                        if (loginInfo.ContainsKey("is_sysmanager"))
                        {
                            if (loginInfo["is_sysmanager"].ToString() != "1")
                            {
                                loginInfo["is_sysmanager"] = "1";
                            }
                        }
                        else
                            loginInfo.Add("is_sysmanager", "1");
                        var checkBr = BusinessFactory.Account.CheckServiceForLogin(loginInfo);

                        return base.JsonString(new
                        {
                            status = "success",
                            message = string.Join(";", br.Message),
                            level = 3
                        });

                    } 
                    #endregion
                }

                return base.JsonString(new
                {
                    status = "error",
                    message = "操作失败",
                    level = 3
                });

            }
            catch (CySoftException ex)
            {
                br.Success = false;
                br.Data = "";
                br.Message.Clear();
                br.Message.Add(ex.Message);
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message),
                    level = 3
                });
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Data = "";
                br.Message.Add("系统异常 请重新登录后重试!");
                br.Level = ErrorLevel.Warning;
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }

        }


        [ActionPurview(true)]
        public ActionResult Test()
        {
            return View();
        }


    }
}
