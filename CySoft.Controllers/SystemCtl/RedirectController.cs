using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Other;
using CySoft.Model.Flags;

#region 重定向控制类
#endregion

namespace CySoft.Controllers.SystemCtl
{
    [LoginActionFilter]
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class RedirectController : BaseController
    {
        [ActionPurview(false)]
        public ActionResult ToSetting()
        {
            BaseResult br = new BaseResult();
            try
            {
                IList<ControllerModel> controllerList = GetControllerList();
                if (controllerList.FirstOrDefault(m => m.name == "setting" && m.actions.FirstOrDefault(b => b == "company") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "Setting", action = "Company" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "recieveraddress" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "RecieverAddress", action = "List" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "account" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "Account", action = "List" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "setting" && m.actions.FirstOrDefault(b => b == "process") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "Setting", action = "Process" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "setting" && m.actions.FirstOrDefault(b => b == "system") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "Setting", action = "System" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "rolesetting" && m.actions.FirstOrDefault(b => b == "index") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "RoleSetting", action = "Index" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "log" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "Log", action = "List" });
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
            return RedirectToRoute("Error", new { id = "noPurview" });
        }

        [ActionPurview(false)]
        public ActionResult ToAttention()
        {
            BaseResult br = new BaseResult();
            try
            {
                IList<ControllerModel> controllerList = GetControllerList();
                if (controllerList.FirstOrDefault(m => m.name == "customer" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "Customer", action = "List" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "supplier" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "Supplier", action = "List" });
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
            return RedirectToRoute("Error", new { id = "noPurview" });
        }

        [ActionPurview(false)]
        public ActionResult ToOrder()
        {
            BaseResult br = new BaseResult();
            try
            {
                IList<ControllerModel> controllerList = GetControllerList();
                if (controllerList.FirstOrDefault(m => m.name == "ordergoods" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "OrderGoods", action = "List" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "favorites" && m.actions.FirstOrDefault(b => b == "index") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "Favorites", action = "Index" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "order" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "Order", action = "List" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "returnorder" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "ReturnOrder", action = "List" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "orderstatistics" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "OrderStatistics", action = "List" });
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
            return RedirectToRoute("Error", new { id = "noPurview" });
        }

        [ActionPurview(false)]
        public ActionResult ToSale()
        {
            BaseResult br = new BaseResult();
            try
            {
                IList<ControllerModel> controllerList = GetControllerList();
                if (controllerList.FirstOrDefault(m => m.name == "saleorder" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "SaleOrder", action = "List" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "salereturn" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "SaleReturn", action = "List" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "saleout" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "SaleOut", action = "List" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "returnorder" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "ReturnOrder", action = "List" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "saleorderstatistics" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "SaleOrderStatistics", action = "List" });
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
            return RedirectToRoute("Error", new { id = "noPurview" });
        }

        [ActionPurview(false)]
        public ActionResult ToGoods()
        {
            BaseResult br = new BaseResult();
            try
            {
                IList<ControllerModel> controllerList = GetControllerList();
                if (controllerList.FirstOrDefault(m => m.name == "goods" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "Goods", action = "List" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "goodsinventory" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "GoodsInventory", action = "List" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "goodstype" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "GoodsType", action = "List" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "goodsunits" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "GoodsUnits", action = "List" });
                }
                if (controllerList.FirstOrDefault(m => m.name == "goodsspec" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
                {
                    return RedirectToRoute("Default", new { controller = "GoodsSpec", action = "List" });
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
            return RedirectToRoute("Error", new { id = "noPurview" });
        }

        [ActionPurview(false)]
        //public ActionResult ToFunds()
        //{
        //    BaseResult br = new BaseResult();
        //    try
        //    {
        //        IList<ControllerModel> controllerList = GetControllerList();

        //        if (UserRole == RoleFlag.Platform)
        //        {
        //            if (controllerList.FirstOrDefault(m => m.name == "funds" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
        //            {
        //                return RedirectToRoute("Default", new { controller = "Funds", action = "List" });
        //            }
        //            if (controllerList.FirstOrDefault(m => m.name == "funds" && m.actions.FirstOrDefault(b => b == "paylist") != null) != null)
        //            {
        //                return RedirectToRoute("Default", new { controller = "Funds", action = "PayList" });
        //            }
        //            if (controllerList.FirstOrDefault(m => m.name == "payconfirm" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
        //            {
        //                return RedirectToRoute("Default", new { controller = "PayConfirm", action = "List" });
        //            }
        //            if (controllerList.FirstOrDefault(m => m.name == "funds" && m.actions.FirstOrDefault(b => b == "statistics") != null) != null)
        //            {
        //                return RedirectToRoute("Default", new { controller = "Funds", action = "Statistics" });
        //            }
        //            if (controllerList.FirstOrDefault(m => m.name == "funds" && m.actions.FirstOrDefault(b => b == "payment") != null) != null)
        //            {
        //                return RedirectToRoute("Default", new { controller = "Funds", action = "Payment" });
        //            }
        //            if (controllerList.FirstOrDefault(m => m.name == "bankaccount") != null)
        //            {
        //                return RedirectToRoute("Default", new { controller = "BankAccount", action = "List" });
        //            }
        //        }
        //        else
        //        {
        //            if (controllerList.FirstOrDefault(m => m.name == "funds" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
        //            {
        //                return RedirectToRoute("Default", new { controller = "Funds", action = "List" });
        //            }
        //            if (controllerList.FirstOrDefault(m => m.name == "funds" && m.actions.FirstOrDefault(b => b == "paylist") != null) != null)
        //            {
        //                return RedirectToRoute("Default", new { controller = "Funds", action = "PayList" });
        //            }
        //            if (controllerList.FirstOrDefault(m => m.name == "payconfirm" && m.actions.FirstOrDefault(b => b == "list") != null) != null)
        //            {
        //                return RedirectToRoute("Default", new { controller = "PayConfirm", action = "List" });
        //            }
        //            if (controllerList.FirstOrDefault(m => m.name == "funds" && m.actions.FirstOrDefault(b => b == "statistics") != null) != null)
        //            {
        //                return RedirectToRoute("Default", new { controller = "Funds", action = "Statistics" });
        //            }
        //            if (controllerList.FirstOrDefault(m => m.name == "funds" && m.actions.FirstOrDefault(b => b == "payment") != null) != null)
        //            {
        //                return RedirectToRoute("Default", new { controller = "Funds", action = "Payment" });
        //            }
        //            if (controllerList.FirstOrDefault(m => m.name == "bankaccount") != null)
        //            {
        //                return RedirectToRoute("Default", new { controller = "BankAccount", action = "List" });
        //            }
        //        }
        //    }
        //    catch (CySoftException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return RedirectToRoute("Error", new { id = "noPurview" });
        //}

        protected IList<ControllerModel> GetControllerList()
        {
            string id_user = GetLoginInfo<string>("id_user");
            BaseResult br2 = BusinessFactory.AccountFunction.GetPurview(id_user);
            IList<ControllerModel> controllerList = (IList<ControllerModel>)br2.Data;
            return controllerList;
        }
    }
}
