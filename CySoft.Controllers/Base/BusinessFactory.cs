#region Imports
using CySoft.IBLL;
using Spring.Context;
using Spring.Context.Support;
using Spring.Objects;
using Spring.Objects.Factory;
using System;
using CySoft.IBLL.Base;
using CySoft.IDAL;
#endregion

#region 业务层工厂
#endregion

namespace CySoft.Controllers.Base
{
    public sealed class BusinessFactory
    {
        private BusinessFactory() { }

        /// <summary>
        /// Return an instance (possibly shared or independent) of the given object name.
        /// </summary>
        #region  private static T GetObject<T>(string name)
        private static T GetObject<T>(string name)
        {
            try
            {
                return (T)ContextRegistry.GetContext().GetObject(name);
            }
            catch (NoSuchObjectDefinitionException ex)
            {
                throw ex;
            }
            catch (ObjectsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 日志
        /// </summary>
        #region public static ILogBLL Log
        private volatile static ILogBLL _Log;
        private static object lock_Log = new object();
        public static ILogBLL Log
        {
            get
            {
                if (_Log == null)
                {
                    lock (lock_Log)
                    {
                        if (_Log == null)
                        {
                            _Log = GetObject<ILogBLL>("LogService");
                        }
                    }
                }
                return _Log;
            }
        }
        #endregion

        /// <summary>
        /// 公共业务
        /// </summary>
        #region public static IUtiletyBLL Utilety
        private volatile static IUtiletyBLL _Utilety;
        private static object lock_Utilety = new object();
        public static IUtiletyBLL Utilety
        {
            get
            {
                if (_Utilety == null)
                {
                    lock (lock_Utilety)
                    {
                        if (_Utilety == null)
                        {
                            _Utilety = GetObject<IUtiletyBLL>("UtiletyService");
                        }
                    }
                }
                return _Utilety;
            }
        }
        #endregion

        /// <summary>
        /// 用户
        /// </summary>
        #region public static IAccountBLL Account
        private volatile static IAccountBLL _Account;
        private static object lock_Account = new object();
        public static IAccountBLL Account
        {
            get
            {
                if (_Account == null)
                {
                    lock (lock_Account)
                    {
                        if (_Account == null)
                        {
                            _Account = GetObject<IAccountBLL>("AccountService");
                        }
                    }
                }
                return _Account;
            }
        }
        #endregion

        /// <summary>
        /// 用户功能
        /// </summary>
        #region public static IAccountFunctionBLL AccountFunction
        private volatile static IAccountFunctionBLL _AccountFunction;
        private static object lock_AccountFunction = new object();
        public static IAccountFunctionBLL AccountFunction
        {
            get
            {
                if (_AccountFunction == null)
                {
                    lock (lock_AccountFunction)
                    {
                        if (_AccountFunction == null)
                        {
                            _AccountFunction = GetObject<IAccountFunctionBLL>("AccountFunctionService");
                        }
                    }
                }
                return _AccountFunction;
            }
        }
        #endregion

        /// <summary>
        /// 用户参数设置
        /// </summary>
        #region public static IBaseBLL Setting
        private volatile static IBaseBLL _SettingService;
        private static object lock_setting = new object();
        public static IBaseBLL Setting
        {
            get
            {
                if (_SettingService == null)
                {
                    lock (lock_setting)
                    {
                        if (_SettingService == null)
                        {
                            _SettingService = GetObject<IBaseBLL>("SettingService");
                        }
                    }
                }
                return _SettingService;
            }
        }
        #endregion
        /// <summary>
        /// 系统后台主用户管理
        /// </summary>
        #region public static IBaseBLL _UserMasterService
        private volatile static IBaseBLL _UserMasterService;
        private static object lock_UserMaster = new object();
        public static IBaseBLL UserMaster
        {
            get
            {
                if (_UserMasterService == null)
                {
                    lock (lock_UserMaster)
                    {
                        if (_UserMasterService == null)
                        {
                            _UserMasterService = GetObject<IBaseBLL>("UserMasterService");
                        }
                    }
                }
                return _UserMasterService;
            }
        }
        #endregion

        /// <summary>
        /// 系统后台主功能管理
        /// </summary>
        #region public static IFunctionBll _FunctionService
        private volatile static IFunctionBll _FunctionService;
        private static object lock_Function = new object();
        public static IFunctionBll Function
        {
            get
            {
                if (_FunctionService == null)
                {
                    lock (lock_Function)
                    {
                        if (_FunctionService == null)
                        {
                            _FunctionService = GetObject<IFunctionBll>("FunctionService");
                        }
                    }
                }
                return _FunctionService;
            }
        }
        #endregion


        /// <summary>
        /// 业务流程功能
        /// </summary>
        #region public static IBaseBLL ProcessFun
        private volatile static IProcessFunBLL _ProcessFun;
        private static object lock_ProcessFun = new object();
        public static IProcessFunBLL ProcessFun
        {
            get
            {
                if (_ProcessFun == null)
                {
                    lock (lock_ProcessFun)
                    {
                        if (_ProcessFun == null)
                        {
                            _ProcessFun = GetObject<IProcessFunBLL>("ProcessFunService");
                        }
                    }
                }
                return _ProcessFun;
            }
        }
        #endregion

        /// <summary>
        /// 系统设置
        /// </summary>
        #region public static IBaseBLL System
        private volatile static IBaseBLL _System;
        private static object lock_System = new object();
        public static IBaseBLL System
        {
            get
            {
                if (_System == null)
                {
                    lock (lock_System)
                    {
                        if (_System == null)
                        {
                            _System = GetObject<IBaseBLL>("SystemService");
                        }
                    }
                }
                return _System;
            }
        }
        #endregion

        /// <summary>
        /// 客户分类
        /// </summary>
        #region public static IBaseBLL CustomerType
        private volatile static IBaseBLL _CustomerType;
        private static object lock_CustomerType = new object();
        public static IBaseBLL CustomerType
        {
            get
            {
                if (_CustomerType == null)
                {
                    lock (lock_CustomerType)
                    {
                        if (_CustomerType == null)
                        {
                            _CustomerType = GetObject<IBaseBLL>("CustomerTypeService");
                        }
                    }
                }
                return _CustomerType;
            }
        }
        #endregion

        /// <summary>
        /// 客户分类
        /// </summary>
        #region public static IBaseBLL GoodsTag
        private volatile static IBaseBLL _GoodsTag;
        private static object lock_GoodsTag = new object();
        public static IBaseBLL GoodsTag
        {
            get
            {
                if (_GoodsTag == null)
                {
                    lock (lock_GoodsTag)
                    {
                        if (_GoodsTag == null)
                        {
                            _GoodsTag = GetObject<IBaseBLL>("GoodsTagService");
                        }
                    }
                }
                return _GoodsTag;
            }
        }
        #endregion

        /// <summary>
        /// 客户
        /// </summary>
        #region public static IBaseBLL Customer
        private volatile static IBaseBLL _Customer;
        private static object lock_Customer = new object();
        public static IBaseBLL Customer
        {
            get
            {
                if (_Customer == null)
                {
                    lock (lock_Customer)
                    {
                        if (_Customer == null)
                        {
                            _Customer = GetObject<IBaseBLL>("CustomerService");
                        }
                    }
                }
                return _Customer;
            }
        }
        #endregion

        /// <summary>
        /// 供应商
        /// </summary>
        #region public static IBaseBLL Supplier
        private volatile static ISupplierBLL _Supplier;
        private static object lock_Supplier = new object();
        public static ISupplierBLL Supplier
        {
            get
            {
                if (_Supplier == null)
                {
                    lock (lock_Supplier)
                    {
                        if (_Supplier == null)
                        {
                            _Supplier = GetObject<ISupplierBLL>("SupplierService");
                        }
                    }
                }
                return _Supplier;
            }
        }
        #endregion

        /// <summary>
        /// 关注供应商
        /// </summary>
        #region public static IBaseBLL BuyerAttention
        private volatile static IBuyerAttentionBLL _BuyerAttention;
        private static object lock_BuyerAttention = new object();
        public static IBuyerAttentionBLL BuyerAttention
        {
            get
            {
                if (_BuyerAttention == null)
                {
                    lock (lock_BuyerAttention)
                    {
                        if (_BuyerAttention == null)
                        {
                            _BuyerAttention = GetObject<IBuyerAttentionBLL>("BuyerAttentionService");
                        }
                    }
                }
                return _BuyerAttention;
            }
        }
        #endregion

        /// <summary>
        /// 通过采购商
        /// </summary>
        #region public static IBaseBLL SupplierAttention
        private volatile static IBaseBLL _SupplierAttention;
        private static object lock_SupplierAttention = new object();
        public static IBaseBLL SupplierAttention
        {
            get
            {
                if (_SupplierAttention == null)
                {
                    lock (lock_SupplierAttention)
                    {
                        if (_SupplierAttention == null)
                        {
                            _SupplierAttention = GetObject<IBaseBLL>("SupplierAttentionService");
                        }
                    }
                }
                return _SupplierAttention;
            }
        }
        #endregion

        /// <summary>
        /// 采购商
        /// </summary>
        #region public static IBaseBLL Buyer
        private volatile static IBuyerBLL _Buyer;
        private static object lock_Buyer = new object();
        public static IBuyerBLL Buyer
        {
            get
            {
                if (_Buyer == null)
                {
                    lock (lock_Buyer)
                    {
                        if (_Buyer == null)
                        {
                            _Buyer = GetObject<IBuyerBLL>("BuyerService");
                        }
                    }
                }
                return _Buyer;
            }
        }
        #endregion

        /// <summary>
        /// 商品
        /// </summary>
        #region public static ITreeBLL Industrie
        private volatile static IGoodsBLL _Goods;
        private static object lock_Goods = new object();
        public static IGoodsBLL Goods
        {
            get
            {
                if (_Goods == null)
                {
                    lock (lock_Industrie)
                    {
                        if (_Industrie == null)
                        {
                            _Goods = GetObject<IGoodsBLL>("GoodsService");
                        }
                    }
                }
                return _Goods;
            }
        }
        #endregion

        /// <summary>
        /// 商品库存
        /// </summary>
        #region public static ITreeBLL GoodsInventory
        private volatile static IBaseBLL _GoodsInventory;
        private static object lock_GoodsInventory = new object();
        public static IBaseBLL GoodsInventory
        {
            get
            {
                if (_GoodsInventory == null)
                {
                    lock (lock_GoodsInventory)
                    {
                        if (_GoodsInventory == null)
                        {
                            _GoodsInventory = GetObject<IBaseBLL>("GoodsInventoryService");
                        }
                    }
                }
                return _GoodsInventory;
            }
        }
        #endregion

        /// <summary>
        /// 商品分类
        /// </summary>
        #region public static IBaseBLL GoodsTpye
        private volatile static ITreeBLL _GoodsType;
        private static object lock_GoodsType = new object();
        public static ITreeBLL GoodsTpye
        {
            get
            {
                if (_GoodsType == null)
                {
                    lock (lock_GoodsType)
                    {
                        if (_GoodsType == null)
                        {
                            _GoodsType = GetObject<ITreeBLL>("GoodsTypeService");
                        }
                    }
                }
                return _GoodsType;
            }
        }
        #endregion

        /// <summary>
        /// 商品收藏
        /// </summary>
        #region public static IBaseBLL GoodsFavorites
        private volatile static IBaseBLL _GoodsFavorites;
        private static object lock_GoodsFavorites = new object();
        public static IBaseBLL GoodsFavorites
        {
            get
            {
                if (_GoodsFavorites == null)
                {
                    lock (lock_GoodsFavorites)
                    {
                        if (_GoodsFavorites == null)
                        {
                            _GoodsFavorites = GetObject<IBaseBLL>("GoodsFavoritesService");
                        }
                    }
                }
                return _GoodsFavorites;
            }
        }
        #endregion

        /// <summary>
        /// 计量单位
        /// </summary>
        #region public static IBaseBLL GoodsUnits
        private volatile static IBaseBLL _GoodsUnits;
        private static object lock_GoodsUnits = new object();
        public static IBaseBLL GoodsUnits
        {
            get
            {
                if (_GoodsUnits == null)
                {
                    lock (lock_GoodsUnits)
                    {
                        if (_GoodsUnits == null)
                        {
                            _GoodsUnits = GetObject<IBaseBLL>("GoodsUnitsService");
                        }
                    }
                }
                return _GoodsUnits;
            }
        }
        #endregion
        /// <summary>
        /// 商品规格
        /// </summary>
        #region public static IBaseBLL GoodsSpec
        private volatile static IBaseBLL _GoodsSpec;
        private static object lock_GoodsSpec = new object();
        public static IBaseBLL GoodsSpec
        {
            get
            {
                if (_GoodsSpec == null)
                {
                    lock (lock_GoodsSpec)
                    {
                        if (_GoodsSpec == null)
                        {
                            _GoodsSpec = GetObject<IBaseBLL>("GoodsSpecService");
                        }
                    }
                }
                return _GoodsSpec;
            }
        }
        #endregion

        /// <summary>
        /// 商品规格
        /// </summary>
        #region public static IBaseBLL GoodsCart
        private volatile static IBaseBLL _GoodsCart;
        private static object lock_GoodsCart = new object();
        public static IBaseBLL GoodsCart
        {
            get
            {
                if (_GoodsCart == null)
                {
                    lock (lock_GoodsSpec)
                    {
                        if (_GoodsCart == null)
                        {
                            _GoodsCart = GetObject<IBaseBLL>("GoodsCartService");
                        }
                    }
                }
                return _GoodsCart;
            }
        }
        #endregion

        /// <summary>
        /// 配置信息
        /// </summary>
        #region public static IConfigBLL Config
        private volatile static IConfigBLL _Config;
        private static object lock_Config = new object();
        public static IConfigBLL Config
        {
            get
            {
                if (_Config == null)
                {
                    lock (lock_Config)
                    {
                        if (_Config == null)
                        {
                            _Config = GetObject<IConfigBLL>("ConfigService");
                        }
                    }
                }
                return _Config;
            }
        }
        #endregion

        /// <summary>
        /// 行业
        /// </summary>
        #region public static ITreeBLL Industrie
        private volatile static ITreeBLL _Industrie;
        private static object lock_Industrie = new object();
        public static ITreeBLL Industrie
        {
            get
            {
                if (_Industrie == null)
                {
                    lock (lock_Industrie)
                    {
                        if (_Industrie == null)
                        {
                            _Industrie = GetObject<ITreeBLL>("IndustrieService");
                        }
                    }
                }
                return _Industrie;
            }
        }
        #endregion

        /// <summary>
        /// 所在地
        /// </summary>
        #region public static ITreeBLL Districts
        private volatile static ITreeBLL _Districts;
        private static object lock_Districts = new object();
        public static ITreeBLL Districts
        {
            get
            {
                if (_Districts == null)
                {
                    lock (lock_Districts)
                    {
                        if (_Districts == null)
                        {
                            _Districts = GetObject<ITreeBLL>("DistrictsService");
                        }
                    }
                }
                return _Districts;
            }
        }
        #endregion

        /// <summary>
        /// 规则
        /// </summary>
        #region public static IBaseBLL CodingRule
        private volatile static IBaseBLL _CodingRule;
        private static object lock_CodingRule = new object();
        public static IBaseBLL CodingRule
        {
            get
            {
                if (_CodingRule == null)
                {
                    lock (lock_CodingRule)
                    {
                        if (_CodingRule == null)
                        {
                            _CodingRule = GetObject<IBaseBLL>("CodingRuleService");
                        }
                    }
                }
                return _CodingRule;
            }
        }
        #endregion

        /// <summary>
        /// 报表
        /// </summary>
        #region public static IBaseBLL Report
        private volatile static IReportBLL _Report;
        private static object lock_Report = new object();
        public static IReportBLL Report
        {
            get
            {
                if (_Report == null)
                {
                    lock (lock_Report)
                    {
                        if (_Report == null)
                        {
                            _Report = GetObject<IReportBLL>("ReportService");
                        }
                    }
                }
                return _Report;
            }
        }
        #endregion

        /// <summary>
        /// 出库/发货记录
        /// </summary>
        #region public static IBaseBLL ShippingRecord
        private volatile static IShippingRecordBLL _ShippingRecord;
        private static object lock_ShippingRecord = new object();
        public static IShippingRecordBLL ShippingRecord
        {
            get
            {
                if (_ShippingRecord == null)
                {
                    lock (lock_ShippingRecord)
                    {
                        if (_ShippingRecord == null)
                        {
                            _ShippingRecord = GetObject<IShippingRecordBLL>("ShippingRecordService");
                        }
                    }
                }
                return _ShippingRecord;
            }
        }
        #endregion

        /// <summary>
        /// 订单
        /// </summary>
        #region public static IBaseBLL Order
        private volatile static IOrderBLL _Order;
        private static object lock_Order = new object();
        public static IOrderBLL Order
        {
            get
            {
                if (_Order == null)
                {
                    lock (lock_Order)
                    {
                        if (_Order == null)
                        {
                            _Order = GetObject<IOrderBLL>("OrderService");
                        }
                    }
                }
                return _Order;
            }
        }
        #endregion

        /// <summary>
        /// 订单日志
        /// </summary>
        #region public static IBaseBLL OrderLog
        private volatile static IBaseBLL _OrderLog;
        private static object lock_OrderLog = new object();
        public static IBaseBLL OrderLog
        {
            get
            {
                if (_OrderLog == null)
                {
                    lock (lock_OrderLog)
                    {
                        if (_OrderLog == null)
                        {
                            _OrderLog = GetObject<IBaseBLL>("OrderLogService");
                        }
                    }
                }
                return _OrderLog;
            }
        }
        #endregion

        /// <summary>
        /// 商品 by Service
        /// </summary>
        #region public static IOrderServiceBLL OrderService
        private volatile static IOrderServiceBLL _OrderService;
        private static object lock_OrderService = new object();
        public static IOrderServiceBLL OrderService
        {
            get
            {
                if (_OrderService == null)
                {
                    lock (lock_OrderService)
                    {
                        if (_OrderService == null)
                        {
                            _OrderService = GetObject<IOrderServiceBLL>("OrderServiceService");
                        }
                    }
                }
                return _OrderService;
            }
        }
        #endregion


        /// <summary>
        /// 收货地址管理
        /// </summary>
        #region public static IRecieverAddressBLL RecieverAddress
        private volatile static IRecieverAddressBLL _RecieverAddress;
        private static object lock_RecieverAddress = new object();
        public static IRecieverAddressBLL RecieverAddress
        {
            get
            {
                if (_RecieverAddress == null)
                {
                    lock (lock_RecieverAddress)
                    {
                        if (_RecieverAddress == null)
                        {
                            _RecieverAddress = GetObject<IRecieverAddressBLL>("RecieverAddressService");
                        }
                    }
                }
                return _RecieverAddress;
            }
        }
        #endregion

        /// <summary>
        /// 公司信息
        /// </summary>
        #region public static IBaseBLL Company
        private volatile static IBaseBLL _Company;
        private static object lock_Company = new object();
        public static IBaseBLL Company
        {
            get
            {
                if (_Company == null)
                {
                    lock (lock_Company)
                    {
                        if (_Company == null)
                        {
                            _Company = GetObject<IBaseBLL>("CompanyService");
                        }
                    }
                }
                return _Company;
            }
        }
        #endregion

        /// <summary>
        /// 角色权限
        /// </summary>
        #region public static IRoleSettingBLL RoleSetting
        private volatile static IRoleSettingBLL _RoleSetting;
        private static object lock_RoleSetting = new object();
        public static IRoleSettingBLL RoleSetting
        {
            get
            {
                if (_RoleSetting == null)
                {
                    lock (lock_RoleSetting)
                    {
                        if (_RoleSetting == null)
                        {
                            _RoleSetting = GetObject<IRoleSettingBLL>("RoleSettingService");
                        }
                    }
                }
                return _RoleSetting;
            }
        }
        #endregion

        /// <summary>
        /// 资金管理
        /// </summary>
        #region public static IFundsBLL Funds
        private volatile static IFundsBLL _Funds;
        private static object lock_Funds = new object();
        public static IFundsBLL Funds
        {
            get
            {
                if (_Funds == null)
                {
                    lock (lock_Funds)
                    {
                        if (_Funds == null)
                        {
                            _Funds = GetObject<IFundsBLL>("FundsService");
                        }
                    }
                }
                return _Funds;
            }
        }
        #endregion

        /// <summary>
        /// 销售订单
        /// </summary>
        #region public static IBillBLL SaleOrder
        private volatile static IBillBLL _SaleOrder;
        private static object lock_SaleOrder = new object();
        public static IBillBLL SaleOrder
        {
            get
            {
                if (_SaleOrder == null)
                {
                    lock (lock_SaleOrder)
                    {
                        if (_SaleOrder == null)
                        {
                            _SaleOrder = GetObject<IBillBLL>("SaleOrderService");
                        }
                    }
                }
                return _SaleOrder;
            }
        }
        #endregion

        /// <summary>
        /// 销售出库
        /// </summary>
        #region public static IBillBLL SaleOut
        private volatile static IBillBLL _SaleOut;
        private static object lock_SaleOut = new object();
        public static IBillBLL SaleOut
        {
            get
            {
                if (_SaleOut == null)
                {
                    lock (lock_SaleOut)
                    {
                        if (_SaleOut == null)
                        {
                            _SaleOut = GetObject<IBillBLL>("SaleOutService");
                        }
                    }
                }
                return _SaleOut;
            }
        }
        #endregion

        /// <summary>
        /// 银行账号
        /// </summary>
        #region public static IBankAccountBLL BankAccount
        private volatile static IBankAccountBLL _BankAccount;
        private static object lock_BankAccount = new object();
        public static IBankAccountBLL BankAccount
        {
            get
            {
                if (_BankAccount == null)
                {
                    lock (lock_BankAccount)
                    {
                        if (_BankAccount == null)
                        {
                            _BankAccount = GetObject<IBankAccountBLL>("BankAccountService");
                        }
                    }
                }
                return _BankAccount;
            }
        }
        #endregion

        /// <summary>
        /// 反馈
        /// </summary>
        #region public static IFaqBLL Faq
        private volatile static IFaqBLL _Faq;
        private static object lock_Faq = new object();
        public static IFaqBLL Faq
        {
            get
            {
                if (_Faq == null)
                {
                    lock (lock_Faq)
                    {
                        if (_Faq == null)
                        {
                            _Faq = GetObject<IFaqBLL>("FaqService");
                        }
                    }
                }
                return _Faq;
            }
        }
        #endregion


        /// <summary>
        /// 用户
        /// </summary>
        #region public static IBaseBLL UserAccredit
        private volatile static IBaseBLL _UserAccredit;
        private static object lock_UserAccredit = new object();
        public static IBaseBLL UserAccredit
        {
            get
            {
                if (_UserAccredit == null)
                {
                    lock (lock_UserAccredit)
                    {
                        if (_UserAccredit == null)
                        {
                            _UserAccredit = GetObject<IBaseBLL>("UserAccreditService");
                        }
                    }
                }
                return _UserAccredit;
            }
        }
        #endregion

        /// <summary>
        /// 通知公告
        /// </summary>
        #region public static IBaseBLL Info
        private volatile static IBaseBLL _Info;
        private static object lock_Info = new object();
        public static IBaseBLL Info
        {
            get
            {
                if (_Info == null)
                {
                    lock (lock_Info)
                    {
                        if (_Info == null)
                        {
                            _Info = GetObject<IBaseBLL>("InfoService");
                        }
                    }
                }
                return _Info;
            }
        }
        #endregion

        /// <summary>
        /// 通知公告收信
        /// </summary>
        #region public static IBaseBLL Info_User
        private volatile static IBaseBLL _InfoUser;
        private static object lock_InfoUser = new object();
        public static IBaseBLL InfoUser
        {
            get
            {
                if (_InfoUser == null)
                {
                    lock (lock_InfoUser)
                    {
                        if (_InfoUser == null)
                        {
                            _InfoUser = GetObject<IBaseBLL>("InfoUserService");
                        }
                    }
                }
                return _InfoUser;
            }
        }
        #endregion

        /// <summary>
        /// 通知公告类型
        /// </summary>
        #region public static IBaseBLL InfoType
        private volatile static IBaseBLL _InfoType;
        private static object lock_InfoType = new object();
        public static IBaseBLL InfoType
        {
            get
            {
                if (_InfoType == null)
                {
                    lock (lock_InfoType)
                    {
                        if (_InfoType == null)
                        {
                            _InfoType = GetObject<IBaseBLL>("InfoTypeService");
                        }
                    }
                }
                return _InfoType;
            }
        }
        #endregion

        /// <summary>
        /// 用户图册管理类
        /// </summary>
        #region public static IBaseBLL UserPic
        private volatile static IBaseBLL _UserPic;
        private static object lock_UserPic = new object();
        public static IBaseBLL UserPic
        {
            get
            {
                if (_UserPic == null)
                {
                    lock (lock_UserPic)
                    {
                        if (_UserPic == null)
                        {
                            _UserPic = GetObject<IBaseBLL>("UserPicService");
                        }
                    }
                }
                return _UserPic;
            }
        }
        #endregion

        /// <summary>
        /// 用户图册管理类
        /// </summary>
        #region public static IBaseBLL UserCheckIn
        private volatile static IBaseBLL _UserCheckIn;
        private static object lock_UserCheckIn = new object();
        public static IBaseBLL UserCheckIn
        {
            get
            {
                if (_UserCheckIn == null)
                {
                    lock (lock_UserCheckIn)
                    {
                        if (_UserPic == null)
                        {
                            _UserCheckIn = GetObject<IBaseBLL>("UserCheckInService");
                        }
                    }
                }
                return _UserCheckIn;
            }
        }
        #endregion

        /// <summary>
        /// 用户角色
        /// </summary>
        #region public static IBaseBLL UserRole
        private volatile static IBaseBLL _userRole;
        private static object lock_userRole = new object();
        public static IBaseBLL UserRole
        {
            get
            {
                if (_userRole == null)
                {
                    lock (lock_userRole)
                    {
                        if (_userRole == null)
                        {
                            _userRole = GetObject<IBaseBLL>("UserRoleService");
                        }
                    }
                }
                return _userRole;
            }
        }
        #endregion

        /// <summary>
        /// 商品推荐
        /// </summary>
        #region public static IBaseBLL GoodsRecommend
        private volatile static IGoodsRecommendBll _goodsRecommend;
        private static object lock_goodsRecommend = new object();
        public static IGoodsRecommendBll GoodsRecommend
        {
            get
            {
                if (_goodsRecommend == null)
                {
                    lock (lock_goodsRecommend)
                    {
                        if (_goodsRecommend == null)
                        {
                            _goodsRecommend = GetObject<IGoodsRecommendBll>("GoodsRecommendService");
                        }
                    }
                }
                return _goodsRecommend;
            }
        }
        #endregion

        /// <summary>
        /// 商品分单关系
        /// </summary>
        #region public static IGoodsSkuFdBLL GoodsSkuFd
        private volatile static IGoodsSkuFdBLL _GoodsSkuFd;
        private static object lock_GoodsSkuFd = new object();
        public static IGoodsSkuFdBLL GoodsSkuFd
        {
            get
            {
                if (_GoodsSkuFd == null)
                {
                    lock (lock_GoodsSkuFd)
                    {
                        if (_GoodsSkuFd == null)
                        {
                            _GoodsSkuFd = GetObject<IGoodsSkuFdBLL>("GoodsSkuFdService");
                        }
                    }
                }
                return _GoodsSkuFd;
            }
        }
        #endregion


        /// <summary>
        /// 广告
        /// </summary>
        #region public static IBaseBLL _Advertis
        private volatile static IBaseBLL _Advertis;
        private static object lock_Advertis = new object();
        public static IBaseBLL Advertis
        {
            get
            {
                if (_Advertis == null)
                {
                        lock (lock_Advertis)
                        {
                            if (_Advertis == null)
                            {
                                _Advertis = GetObject<IBaseBLL>("AdvertisService");
                            }
                        }
                }
                return _Advertis;
            }
        }
        #endregion

        /// <summary>
        /// 角色模块
        /// </summary>
        #region public static IRoleModuleBll _RoleModule
        private volatile static IRoleModuleBll _RoleModule;
        private static object lock_RoleModule = new object();
        public static IRoleModuleBll RoleModule
        {
            get
            {
                if (_RoleModule == null)
                {
                    lock (lock_Advertis)
                    {
                        if (_RoleModule == null)
                        {
                            _RoleModule = GetObject<IRoleModuleBll>("RoleModuleService");
                        }
                    }
                }
                return _RoleModule;
            }
        }
        #endregion
        /// <summary>
        /// 广告点击用户
        /// </summary>
        #region public static IBaseBLL _Advertis_Log
        private volatile static IBaseBLL _Advertis_Log;
        private static object lock_Advertis_Log = new object();
        public static IBaseBLL Advertis_Log
        {
            get
            {
                if (_Advertis_Log == null)
                {
                    lock (lock_Advertis_Log)
                    {
                        if (_Advertis_Log == null)
                        {
                            _Advertis_Log = GetObject<IBaseBLL>("AdvertisLogService");
                        }
                    }
                }
                return _Advertis_Log;
            }
        }
        #endregion

        /// <summary>
        /// Tb_Dw
        /// </summary>
        #region public static ITb_DwBLL _Tb_Dw
        private volatile static IBaseBLL _Tb_Dw;
        private static object lock_Tb_Dw = new object();
        public static IBaseBLL Tb_Dw
        {
            get
            {
                if (_Tb_Dw == null)
                {
                    lock (lock_Tb_Dw)
                    {
                        if (_Tb_Dw == null)
                        {
                            _Tb_Dw = GetObject<IBaseBLL>("Tb_DwService");
                        }
                    }
                }
                return _Tb_Dw;
            }
        }
        #endregion

        /// <summary>
        /// Tb_Spfl
        /// </summary>
        #region public static ITb_SpflBLL _Tb_Spfl
        private volatile static ITb_SpflBLL _Tb_Spfl;
        private static object lock_Tb_Spfl = new object();
        public static ITb_SpflBLL Tb_Spfl
        {
            get
            {
                if (_Tb_Spfl == null)
                {
                    lock (lock_Tb_Spfl)
                    {
                        if (_Tb_Spfl == null)
                        {
                            _Tb_Spfl = GetObject<ITb_SpflBLL>("Tb_SpflService");
                        }
                    }
                }
                return _Tb_Spfl;
            }
        }
        #endregion

        /// <summary>
        /// Tb_Pay
        /// </summary>
        #region public static ITb_PayBLL _Tb_Pay
        private volatile static IBaseBLL _Tb_Pay;
        private static object lock_Tb_Pay = new object();
        public static IBaseBLL Tb_Pay
        {
            get
            {
                if (_Tb_Pay == null)
                {
                    lock (lock_Tb_Pay)
                    {
                        if (_Tb_Pay == null)
                        {
                            _Tb_Pay = GetObject<IBaseBLL>("Tb_PayService");
                        }
                    }
                }
                return _Tb_Pay;
            }
        }
        #endregion


        /// <summary>
        /// Ts_Flag
        /// </summary>
        #region public static ITs_FlagBLL _Ts_Flag
        private volatile static IBaseBLL _Ts_Flag;
        private static object lock_Ts_Flag = new object();
        public static IBaseBLL Ts_Flag
        {
            get
            {
                if (_Ts_Flag == null)
                {
                    lock (lock_Ts_Flag)
                    {
                        if (_Ts_Flag == null)
                        {
                            _Ts_Flag = GetObject<IBaseBLL>("Ts_FlagService");
                        }
                    }
                }
                return _Ts_Flag;
            }
        }
        #endregion



        /// <summary>
        /// Tb_Shopsp
        /// </summary>
        #region public static ITb_ShopspBLL _Tb_Shopsp
        private volatile static ITb_ShopspBLL _Tb_Shopsp;
        private static object lock_Tb_Shopsp = new object();
        public static ITb_ShopspBLL Tb_Shopsp
        {
            get
            {
                if (_Tb_Shopsp == null)
                {
                    lock (lock_Tb_Shopsp)
                    {
                        if (_Tb_Shopsp == null)
                        {
                            _Tb_Shopsp = GetObject<ITb_ShopspBLL>("Tb_ShopspService");
                        }
                    }
                }
                return _Tb_Shopsp;
            }
        }
        #endregion

        /// <summary>
        /// Tb_Shopsp_Exportdata
        /// </summary>
        #region public static IBaseBLL _Tb_Shopsp_Exportdata
        private volatile static IBaseBLL _Tb_Shopsp_Exportdata;
        private static object lock_Tb_Shopsp_Exportdata = new object();
        public static IBaseBLL Tb_Shopsp_Exportdata
        {
            get
            {
                if (_Tb_Shopsp_Exportdata == null)
                {
                    lock (lock_Tb_Shopsp_Exportdata)
                    {
                        if (_Tb_Shopsp_Exportdata == null)
                        {
                            _Tb_Shopsp_Exportdata = GetObject<IBaseBLL>("Tb_Shopsp_ExportdataService");
                        }
                    }
                }
                return _Tb_Shopsp_Exportdata;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Role_Pos_Function
        /// </summary>
        #region public static ITb_Role_Pos_FunctionBLL _Tb_Role_Pos_Function
        private volatile static IBaseBLL _Tb_Role_Pos_Function;
        private static object lock_Tb_Role_Pos_Function = new object();
        public static IBaseBLL Tb_Role_Pos_Function
        {
            get
            {
                if (_Tb_Role_Pos_Function == null)
                {
                    lock (lock_Tb_Role_Pos_Function)
                    {
                        if (_Tb_Role_Pos_Function == null)
                        {
                            _Tb_Role_Pos_Function = GetObject<IBaseBLL>("Tb_Role_Pos_FunctionService");
                        }
                    }
                }
                return _Tb_Role_Pos_Function;
            }
        }
        #endregion

        /// <summary>
        /// Tb_Post_Function
        /// </summary>
        #region public static ITb_Post_FunctionBLL _Tb_Post_Function
        private volatile static ITb_Pos_FunctionBLL _Tb_Post_Function;
        private static object lock_Tb_Post_Function = new object();
        public static ITb_Pos_FunctionBLL Tb_Post_Function
        {
            get
            {
                if (_Tb_Post_Function == null)
                {
                    lock (lock_Tb_Post_Function)
                    {
                        if (_Tb_Post_Function == null)
                        {
                            _Tb_Post_Function = GetObject<ITb_Pos_FunctionBLL>("Tb_Pos_FunctionService");
                        }
                    }
                }
                return _Tb_Post_Function;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Pos_Role_Module
        /// </summary>
        #region public static ITb_Pos_Role_ModuleBLL _Tb_Pos_Role_Module
        private volatile static ITb_Pos_Role_ModuleBLL _Tb_Pos_Role_Module;
        private static object lock_Tb_Pos_Role_Module = new object();
        public static ITb_Pos_Role_ModuleBLL Tb_Pos_Role_Module
        {
            get
            {
                if (_Tb_Pos_Role_Module == null)
                {
                    lock (lock_Tb_Pos_Role_Module)
                    {
                        if (_Tb_Pos_Role_Module == null)
                        {
                            _Tb_Pos_Role_Module = GetObject<ITb_Pos_Role_ModuleBLL>("Tb_Pos_Role_ModuleService");
                        }
                    }
                }
                return _Tb_Pos_Role_Module;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Shopsp_Qc_Md
        /// </summary>
        #region public static IBaseBLL _Tb_Shopsp_Qc_Md
        private volatile static IBaseBLL _Tb_Shopsp_Qc_Md;
        private static object lock_Tb_Shopsp_Qc_Md = new object();
        public static IBaseBLL Tb_Shopsp_Qc_Md
        {
            get
            {
                if (_Tb_Shopsp_Qc_Md == null)
                {
                    lock (lock_Tb_Shopsp_Qc_Md)
                    {
                        if (_Tb_Shopsp_Qc_Md == null)
                        {
                            _Tb_Shopsp_Qc_Md = GetObject<IBaseBLL>("Tb_Shopsp_Qc_MdService");
                        }
                    }
                }
                return _Tb_Shopsp_Qc_Md;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Shop
        /// </summary>
        #region public static ITb_ShopBLL _Tb_Shop
        private volatile static ITb_ShopBLL _Tb_Shop;
        private static object lock_Tb_Shop = new object();
        public static ITb_ShopBLL Tb_Shop
        {
            get
            {
                if (_Tb_Shop == null)
                {
                    lock (lock_Tb_Shop)
                    {
                        if (_Tb_Shop == null)
                        {
                            _Tb_Shop = GetObject<ITb_ShopBLL>("Tb_ShopService");
                        }
                    }
                }
                return _Tb_Shop;
            }
        }
        #endregion


        /// <summary>
        /// Ts_Parm
        /// </summary>
        #region public static IBaseBLL _Ts_Parm
        private volatile static IBaseBLL _Ts_Parm;
        private static object lock_Ts_Parm = new object();
        public static IBaseBLL Ts_Parm
        {
            get
            {
                if (_Ts_Parm == null)
                {
                    lock (lock_Ts_Parm)
                    {
                        if (_Ts_Parm == null)
                        {
                            _Ts_Parm = GetObject<IBaseBLL>("Ts_ParmService");
                        }
                    }
                }
                return _Ts_Parm;
            }
        }
        #endregion

        /// <summary>
        /// Ts_Parm_Shop
        /// </summary>
        #region public static ITs_Parm_ShopBLL _Ts_Parm_Shop
        private volatile static IBaseBLL _Ts_Parm_Shop;
        private static object lock_Ts_Parm_Shop = new object();
        public static IBaseBLL Ts_Parm_Shop
        {
            get
            {
                if (_Ts_Parm_Shop == null)
                {
                    lock (lock_Ts_Parm_Shop)
                    {
                        if (_Ts_Parm_Shop == null)
                        {
                            _Ts_Parm_Shop = GetObject<IBaseBLL>("Ts_Parm_ShopService");
                        }
                    }
                }
                return _Ts_Parm_Shop;
            }
        }
        #endregion

        /// <summary>
        /// Tb_User_Shop
        /// </summary>
        #region public static ITb_User_ShopBLL _Tb_User_Shop
        private volatile static IBaseBLL _Tb_User_Shop;
        private static object lock_Tb_User_Shop = new object();
        public static IBaseBLL Tb_User_Shop
        {
            get
            {
                if (_Tb_User_Shop == null)
                {
                    lock (lock_Tb_User_Shop)
                    {
                        if (_Tb_User_Shop == null)
                        {
                            _Tb_User_Shop = GetObject<IBaseBLL>("Tb_User_ShopService");
                        }
                    }
                }
                return _Tb_User_Shop;
            }
        }
        #endregion

        /// <summary>
        /// Tb_Pay_Config
        /// </summary>
        #region public static ITb_Pay_ConfigBLL _Tb_Pay_Config
        private volatile static IBaseBLL _Tb_Pay_Config;
        private static object lock_Tb_Pay_Config = new object();
        public static IBaseBLL Tb_Pay_Config
        {
            get
            {
                if (_Tb_Pay_Config == null)
                {
                    lock (lock_Tb_Pay_Config)
                    {
                        if (_Tb_Pay_Config == null)
                        {
                            _Tb_Pay_Config = GetObject<IBaseBLL>("Tb_Pay_ConfigService");
                        }
                    }
                }
                return _Tb_Pay_Config;
            }
        }
        #endregion

        /// <summary>
        /// Ts_Lszd
        /// </summary>
        #region public static ITs_LszdBLL _Ts_Lszd
        private volatile static IBaseBLL _Ts_Lszd;
        private static object lock_Ts_Lszd = new object();
        public static IBaseBLL Ts_Lszd
        {
            get
            {
                if (_Ts_Lszd == null)
                {
                    lock (lock_Ts_Lszd)
                    {
                        if (_Ts_Lszd == null)
                        {
                            _Ts_Lszd = GetObject<IBaseBLL>("Ts_LszdService");
                        }
                    }
                }
                return _Ts_Lszd;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Promote
        /// </summary>
        #region public static ITb_PromoteBLL _Tb_Promote
        private volatile static IBaseBLL _Tb_Promote;
        private static object lock_Tb_Promote = new object();
        public static IBaseBLL Tb_Promote
        {
            get
            {
                if (_Tb_Promote == null)
                {
                    lock (lock_Tb_Promote)
                    {
                        if (_Tb_Promote == null)
                        {
                            _Tb_Promote = GetObject<IBaseBLL>("Tb_PromoteService");
                        }
                    }
                }
                return _Tb_Promote;
            }
        }
        #endregion

        /// <summary>
        /// Td_Promote_Shop
        /// </summary>
        #region public static ITd_Promote_ShopBLL _Td_Promote_Shop
        private volatile static IBaseBLL _Td_Promote_Shop;
        private static object lock_Td_Promote_Shop = new object();
        public static IBaseBLL Td_Promote_Shop
        {
            get
            {
                if (_Td_Promote_Shop == null)
                {
                    lock (lock_Td_Promote_Shop)
                    {
                        if (_Td_Promote_Shop == null)
                        {
                            _Td_Promote_Shop = GetObject<IBaseBLL>("Td_Promote_ShopService");
                        }
                    }
                }
                return _Td_Promote_Shop;
            }
        }
        #endregion
        /// <summary>
        /// Td_Promote_1
        /// </summary>
        #region public static ITd_Promote_1BLL _Td_Promote_1
        private volatile static IBaseBLL _Td_Promote_1;
        private static object lock_Td_Promote_1 = new object();
        public static IBaseBLL Td_Promote_1
        {
            get
            {
                if (_Td_Promote_1 == null)
                {
                    lock (lock_Td_Promote_1)
                    {
                        if (_Td_Promote_1 == null)
                        {
                            _Td_Promote_1 = GetObject<IBaseBLL>("Td_Promote_1Service");
                        }
                    }
                }
                return _Td_Promote_1;
            }
        }
        #endregion

        /// <summary>
        /// Td_Promote_2
        /// </summary>
        #region public static ITd_Promote_2BLL _Td_Promote_2
        private volatile static IBaseBLL _Td_Promote_2;
        private static object lock_Td_Promote_2 = new object();
        public static IBaseBLL Td_Promote_2
        {
            get
            {
                if (_Td_Promote_2 == null)
                {
                    lock (lock_Td_Promote_2)
                    {
                        if (_Td_Promote_2 == null)
                        {
                            _Td_Promote_2 = GetObject<IBaseBLL>("Td_Promote_2Service");
                        }
                    }
                }
                return _Td_Promote_2;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Promote_Sort
        /// </summary>
        #region public static ITb_Promote_SortBLL _Tb_Promote_Sort
        private volatile static IBaseBLL _Tb_Promote_Sort;
        private static object lock_Tb_Promote_Sort = new object();
        public static IBaseBLL Tb_Promote_Sort
        {
            get
            {
                if (_Tb_Promote_Sort == null)
                {
                    lock (lock_Tb_Promote_Sort)
                    {
                        if (_Tb_Promote_Sort == null)
                        {
                            _Tb_Promote_Sort = GetObject<IBaseBLL>("Tb_Promote_SortService");
                        }
                    }
                }
                return _Tb_Promote_Sort;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Promote_Largess
        /// </summary>
        #region public static ITb_Promote_LargessBLL _Tb_Promote_Largess
        private volatile static IBaseBLL _Tb_Promote_Largess;
        private static object lock_Tb_Promote_Largess = new object();
        public static IBaseBLL Tb_Promote_Largess
        {
            get
            {
                if (_Tb_Promote_Largess == null)
                {
                    lock (lock_Tb_Promote_Largess)
                    {
                        if (_Tb_Promote_Largess == null)
                        {
                            _Tb_Promote_Largess = GetObject<IBaseBLL>("Tb_Promote_LargessService");
                        }
                    }
                }
                return _Tb_Promote_Largess;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Gysfl
        /// </summary>
        #region public static ITb_GysflBLL _Tb_Gysfl
        private volatile static ITb_GysflBLL _Tb_Gysfl;
        private static object lock_Tb_Gysfl = new object();
        public static ITb_GysflBLL Tb_Gysfl
        {
            get
            {
                if (_Tb_Gysfl == null)
                {
                    lock (lock_Tb_Gysfl)
                    {
                        if (_Tb_Gysfl == null)
                        {
                            _Tb_Gysfl = GetObject<ITb_GysflBLL>("Tb_GysflService");
                        }
                    }
                }
                return _Tb_Gysfl;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Gys
        /// </summary>
        #region public static ITb_GysBLL _Tb_Gys
        private volatile static IBaseBLL _Tb_Gys;
        private static object lock_Tb_Gys = new object();
        public static IBaseBLL Tb_Gys
        {
            get
            {
                if (_Tb_Gys == null)
                {
                    lock (lock_Tb_Gys)
                    {
                        if (_Tb_Gys == null)
                        {
                            _Tb_Gys = GetObject<IBaseBLL>("Tb_GysService");
                        }
                    }
                }
                return _Tb_Gys;
            }
        }
        #endregion

        /// <summary>
        /// Tb_Shop_Shop
        /// </summary>
        #region public static ITb_Shop_ShopBLL _Tb_Shop_Shop
        private volatile static IBaseBLL _Tb_Shop_Shop;
        private static object lock_Tb_Shop_Shop = new object();
        public static IBaseBLL Tb_Shop_Shop
        {
            get
            {
                if (_Tb_Shop_Shop == null)
                {
                    lock (lock_Tb_Shop_Shop)
                    {
                        if (_Tb_Shop_Shop == null)
                        {
                            _Tb_Shop_Shop = GetObject<IBaseBLL>("Tb_Shop_ShopService");
                        }
                    }
                }
                return _Tb_Shop_Shop;
            }
        }
        #endregion

        /// <summary>
        /// Td_Jh_1
        /// </summary>
        #region public static ITd_Jh_1BLL _Td_Jh_1
        private volatile static ITd_Jh_1BLL _Td_Jh_1;
        private static object lock_Td_Jh_1 = new object();
        public static ITd_Jh_1BLL Td_Jh_1
        {
            get
            {
                if (_Td_Jh_1 == null)
                {
                    lock (lock_Td_Jh_1)
                    {
                        if (_Td_Jh_1 == null)
                        {
                            _Td_Jh_1 = GetObject<ITd_Jh_1BLL>("Td_Jh_1Service");
                        }
                    }
                }
                return _Td_Jh_1;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Ticket
        /// </summary>
        #region public static ITb_TicketBLL _Tb_Ticket
        private volatile static IBaseBLL _Tb_Ticket;
        private static object lock_Tb_Ticket = new object();
        public static IBaseBLL Tb_Ticket
        {
            get
            {
                if (_Tb_Ticket == null)
                {
                    lock (lock_Tb_Ticket)
                    {
                        if (_Tb_Ticket == null)
                        {
                            _Tb_Ticket = GetObject<IBaseBLL>("Tb_TicketService");
                        }
                    }
                }
                return _Tb_Ticket;
            }
        }
        #endregion



        /// <summary>
        /// Tb_Hyfl
        /// </summary>
        #region public static IBaseBLL _Tb_Hyfl
        private volatile static IBaseBLL _Tb_Hyfl;
        private static object lock_Tb_Hyfl = new object();
        public static IBaseBLL Tb_Hyfl
        {
            get
            {
                if (_Tb_Hyfl == null)
                {
                    lock (lock_Tb_Hyfl)
                    {
                        if (_Tb_Hyfl == null)
                        {
                            _Tb_Hyfl = GetObject<IBaseBLL>("Tb_HyflService");
                        }
                    }
                }
                return _Tb_Hyfl;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Hy
        /// </summary>
        #region public static IBaseBLL _Tb_Hy
        private volatile static IBaseBLL _Tb_Hy;
        private static object lock_Tb_Hy = new object();
        public static IBaseBLL Tb_Hy
        {
            get
            {
                if (_Tb_Hy == null)
                {
                    lock (lock_Tb_Hy)
                    {
                        if (_Tb_Hy == null)
                        {
                            _Tb_Hy = GetObject<IBaseBLL>("Tb_HyService");
                        }
                    }
                }
                return _Tb_Hy;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Hy_Shop
        /// </summary>
        #region public static IBaseBLL _Tb_Hy_Shop
        private volatile static IBaseBLL _Tb_Hy_Shop;
        private static object lock_Tb_Hy_Shop = new object();
        public static IBaseBLL Tb_Hy_Shop
        {
            get
            {
                if (_Tb_Hy_Shop == null)
                {
                    lock (lock_Tb_Hy_Shop)
                    {
                        if (_Tb_Hy_Shop == null)
                        {
                            _Tb_Hy_Shop = GetObject<IBaseBLL>("Tb_Hy_ShopService");
                        }
                    }
                }
                return _Tb_Hy_Shop;
            }
        }
        #endregion


        /// <summary>
        /// Td_Jh_Th_1
        /// </summary>
        #region public static ITd_Jh_Th_1BLL _Td_Jh_Th_1
        private volatile static IBaseBLL _Td_Jh_Th_1;
        private static object lock_Td_Jh_Th_1 = new object();
        public static IBaseBLL Td_Jh_Th_1
        {
            get
            {
                if (_Td_Jh_Th_1 == null)
                {
                    lock (lock_Td_Jh_Th_1)
                    {
                        if (_Td_Jh_Th_1 == null)
                        {
                            _Td_Jh_Th_1 = GetObject<IBaseBLL>("Td_Jh_Th_1Service");
                        }
                    }
                }
                return _Td_Jh_Th_1;
            }
        }
        #endregion


        /// <summary>
        /// Td_Fk_1
        /// </summary>
        #region public static IBaseBLL _Td_Fk_1
        private volatile static IBaseBLL _Td_Fk_1;
        private static object lock_Td_Fk_1 = new object();
        public static IBaseBLL Td_Fk_1
        {
            get
            {
                if (_Td_Fk_1 == null)
                {
                    lock (lock_Td_Fk_1)
                    {
                        if (_Td_Fk_1 == null)
                        {
                            _Td_Fk_1 = GetObject<IBaseBLL>("Td_Fk_1Service");
                        }
                    }
                }
                return _Td_Fk_1;
            }
        }
        #endregion


        /// <summary>
        /// Tz_Yf_Jsc_Gx
        /// </summary>
        #region public static IBaseBLL _Tz_Yf_Jsc_Gx
        private volatile static IBaseBLL _Tz_Yf_Jsc_Gx;
        private static object lock_Tz_Yf_Jsc_Gx = new object();
        public static IBaseBLL Tz_Yf_Jsc_Gx
        {
            get
            {
                if (_Tz_Yf_Jsc_Gx == null)
                {
                    lock (lock_Tz_Yf_Jsc_Gx)
                    {
                        if (_Tz_Yf_Jsc_Gx == null)
                        {
                            _Tz_Yf_Jsc_Gx = GetObject<IBaseBLL>("Tz_Yf_Jsc_GxService");
                        }
                    }
                }
                return _Tz_Yf_Jsc_Gx;
            }
        }
        #endregion


        /// <summary>
        /// Td_Ls_Jb_1
        /// </summary>
        #region public static ITd_Ls_Jb_1BLL _Td_Ls_Jb_1
        private volatile static IBaseBLL _Td_Ls_Jb_1;
        private static object lock_Td_Ls_Jb_1 = new object();
        public static IBaseBLL Td_Ls_Jb_1
        {
            get
            {
                if (_Td_Ls_Jb_1 == null)
                {
                    lock (lock_Td_Ls_Jb_1)
                    {
                        if (_Td_Ls_Jb_1 == null)
                        {
                            _Td_Ls_Jb_1 = GetObject<IBaseBLL>("Td_Ls_Jb_1Service");
                        }
                    }
                }
                return _Td_Ls_Jb_1;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ls_Jb_2
        /// </summary>
        #region public static ITd_Ls_Jb_2BLL _Td_Ls_Jb_2
        private volatile static IBaseBLL _Td_Ls_Jb_2;
        private static object lock_Td_Ls_Jb_2 = new object();
        public static IBaseBLL Td_Ls_Jb_2
        {
            get
            {
                if (_Td_Ls_Jb_2 == null)
                {
                    lock (lock_Td_Ls_Jb_2)
                    {
                        if (_Td_Ls_Jb_2 == null)
                        {
                            _Td_Ls_Jb_2 = GetObject<IBaseBLL>("Td_Ls_Jb_2Service");
                        }
                    }
                }
                return _Td_Ls_Jb_2;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ls_1
        /// </summary>
        #region public static ITd_Ls_1BLL _Td_Ls_1
        private volatile static IBaseBLL _Td_Ls_1;
        private static object lock_Td_Ls_1 = new object();
        public static IBaseBLL Td_Ls_1
        {
            get
            {
                if (_Td_Ls_1 == null)
                {
                    lock (lock_Td_Ls_1)
                    {
                        if (_Td_Ls_1 == null)
                        {
                            _Td_Ls_1 = GetObject<IBaseBLL>("Td_Ls_1Service");
                        }
                    }
                }
                return _Td_Ls_1;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ls_2
        /// </summary>
        #region public static ITd_Ls_2BLL _Td_Ls_2
        private volatile static IBaseBLL _Td_Ls_2;
        private static object lock_Td_Ls_2 = new object();
        public static IBaseBLL Td_Ls_2
        {
            get
            {
                if (_Td_Ls_2 == null)
                {
                    lock (lock_Td_Ls_2)
                    {
                        if (_Td_Ls_2 == null)
                        {
                            _Td_Ls_2 = GetObject<IBaseBLL>("Td_Ls_2Service");
                        }
                    }
                }
                return _Td_Ls_2;
            }
        }
        #endregion


        /// <summary>
        /// Tz_Jxc_Flow
        /// </summary>
        #region public static IBaseBLL _Tz_Jxc_Flow
        private volatile static IBaseBLL _Tz_Jxc_Flow;
        private static object lock_Tz_Jxc_Flow = new object();
        public static IBaseBLL Tz_Jxc_Flow
        {
            get
            {
                if (_Tz_Jxc_Flow == null)
                {
                    lock (lock_Tz_Jxc_Flow)
                    {
                        if (_Tz_Jxc_Flow == null)
                        {
                            _Tz_Jxc_Flow = GetObject<IBaseBLL>("Tz_Jxc_FlowService");
                        }
                    }
                }
                return _Tz_Jxc_Flow;
            }
        }
        #endregion


        /// <summary>
        /// Td_Ps_Ck_1
        /// </summary>
        #region public static ITd_Ps_Ck_1BLL _Td_Ps_Ck_1
        private volatile static IBaseBLL _Td_Ps_Ck_1;
        private static object lock_Td_Ps_Ck_1 = new object();
        public static IBaseBLL Td_Ps_Ck_1
        {
            get
            {
                if (_Td_Ps_Ck_1 == null)
                {
                    lock (lock_Td_Ps_Ck_1)
                    {
                        if (_Td_Ps_Ck_1 == null)
                        {
                            _Td_Ps_Ck_1 = GetObject<IBaseBLL>("Td_Ps_Ck_1Service");
                        }
                    }
                }
                return _Td_Ps_Ck_1;
            }
        }
        #endregion


        /// <summary>
        /// Td_Ps_Ck_2
        /// </summary>
        #region public static ITd_Ps_Ck_2BLL _Td_Ps_Ck_2
        private volatile static IBaseBLL _Td_Ps_Ck_2;
        private static object lock_Td_Ps_Ck_2 = new object();
        public static IBaseBLL Td_Ps_Ck_2
        {
            get
            {
                if (_Td_Ps_Ck_2 == null)
                {
                    lock (lock_Td_Ps_Ck_2)
                    {
                        if (_Td_Ps_Ck_2 == null)
                        {
                            _Td_Ps_Ck_2 = GetObject<IBaseBLL>("Td_Ps_Ck_2Service");
                        }
                    }
                }
                return _Td_Ps_Ck_2;
            }
        }
        #endregion


        /// <summary>
        /// Tz_Hy_Je
        /// </summary>
        #region public static IBaseBLL _Tz_Hy_Je
        private volatile static IBaseBLL _Tz_Hy_Je;
        private static object lock_Tz_Hy_Je = new object();
        public static IBaseBLL Tz_Hy_Je
        {
            get
            {
                if (_Tz_Hy_Je == null)
                {
                    lock (lock_Tz_Hy_Je)
                    {
                        if (_Tz_Hy_Je == null)
                        {
                            _Tz_Hy_Je = GetObject<IBaseBLL>("Tz_Hy_JeService");
                        }
                    }
                }
                return _Tz_Hy_Je;
            }
        }
        #endregion


        /// <summary>
        /// Tz_Hy_Je_Flow
        /// </summary>
        #region public static ITz_Hy_Je_FlowDAL _Tz_Hy_Je_Flow
        private volatile static IBaseBLL _Tz_Hy_Je_Flow;
        private static object lock_Tz_Hy_Je_Flow = new object();
        public static IBaseBLL Tz_Hy_Je_Flow
        {
            get
            {
                if (_Tz_Hy_Je_Flow == null)
                {
                    lock (lock_Tz_Hy_Je_Flow)
                    {
                        if (_Tz_Hy_Je_Flow == null)
                        {
                            _Tz_Hy_Je_Flow = GetObject<IBaseBLL>("Tz_Hy_Je_FlowService");
                        }
                    }
                }
                return _Tz_Hy_Je_Flow;
            }
        }
        #endregion



        /// <summary>
        /// Tz_Hy_Jf
        /// </summary>
        #region public static ITz_Hy_JfBLL _Tz_Hy_Jf
        private volatile static IBaseBLL _Tz_Hy_Jf;
        private static object lock_Tz_Hy_Jf = new object();
        public static IBaseBLL Tz_Hy_Jf
        {
            get
            {
                if (_Tz_Hy_Jf == null)
                {
                    lock (lock_Tz_Hy_Jf)
                    {
                        if (_Tz_Hy_Jf == null)
                        {
                            _Tz_Hy_Jf = GetObject<IBaseBLL>("Tz_Hy_JfService");
                        }
                    }
                }
                return _Tz_Hy_Jf;
            }
        }
        #endregion

        /// <summary>
        /// Td_Hy_Cz_1
        /// </summary>
        #region public static IBaseBLL _Td_Hy_Cz_1
        private volatile static IBaseBLL _Td_Hy_Cz_1;
        private static object lock_Td_Hy_Cz_1 = new object();
        public static IBaseBLL Td_Hy_Cz_1
        {
            get
            {
                if (_Td_Hy_Cz_1 == null)
                {
                    lock (lock_Td_Hy_Cz_1)
                    {
                        if (_Td_Hy_Cz_1 == null)
                        {
                            _Td_Hy_Cz_1 = GetObject<IBaseBLL>("Td_Hy_Cz_1Service");
                        }
                    }
                }
                return _Td_Hy_Cz_1;
            }
        }
        #endregion




        /// <summary>
        /// BusinessBLL
        /// </summary>
        #region public static IBusinessBLL _Business
        private volatile static IBusinessBLL _Business;
        private static object lock_Business = new object();
        public static IBusinessBLL Business
        {
            get
            {
                if (_Business == null)
                {
                    lock (lock_Business)
                    {
                        if (_Business == null)
                        {
                            _Business = GetObject<IBusinessBLL>("BusinessBLLService");
                        }
                    }
                }
                return _Business;
            }
        }
        #endregion


        /// <summary>
        /// Td_Kc_Kspd_1
        /// </summary>
        #region public static IBaseBLL _Td_Kc_Kspd_1
        private volatile static IBaseBLL _Td_Kc_Kspd_1;
        private static object lock_Td_Kc_Kspd_1 = new object();
        public static IBaseBLL Td_Kc_Kspd_1
        {
            get
            {
                if (_Td_Kc_Kspd_1 == null)
                {
                    lock (lock_Td_Kc_Kspd_1)
                    {
                        if (_Td_Kc_Kspd_1 == null)
                        {
                            _Td_Kc_Kspd_1 = GetObject<IBaseBLL>("Td_Kc_Kspd_1Service");
                        }
                    }
                }
                return _Td_Kc_Kspd_1;
            }
        }
        #endregion



        /// <summary>
        /// Td_Jh_Dd_1
        /// </summary>
        #region public static IBaseBLL _Td_Jh_Dd_1
        private volatile static IBaseBLL _Td_Jh_Dd_1;
        private static object lock_Td_Jh_Dd_1 = new object();
        public static IBaseBLL Td_Jh_Dd_1
        {
            get
            {
                if (_Td_Jh_Dd_1 == null)
                {
                    lock (lock_Td_Jh_Dd_1)
                    {
                        if (_Td_Jh_Dd_1 == null)
                        {
                            _Td_Jh_Dd_1 = GetObject<IBaseBLL>("Td_Jh_Dd_1Service");
                        }
                    }
                }
                return _Td_Jh_Dd_1;
            }
        }
        #endregion



        /// <summary>
        /// Td_Ps_Rk_1
        /// </summary>
        #region public static ITd_Ps_Rk_1BLL _Td_Ps_Rk_1
        private volatile static IBaseBLL _Td_Ps_Rk_1;
        private static object lock_Td_Ps_Rk_1 = new object();
        public static IBaseBLL Td_Ps_Rk_1
        {
            get
            {
                if (_Td_Ps_Rk_1 == null)
                {
                    lock (lock_Td_Ps_Rk_1)
                    {
                        if (_Td_Ps_Rk_1 == null)
                        {
                            _Td_Ps_Rk_1 = GetObject<IBaseBLL>("Td_Ps_Rk_1Service");
                        }
                    }
                }
                return _Td_Ps_Rk_1;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ps_Rk_2
        /// </summary>
        #region public static ITd_Ps_Rk_2BLL _Td_Ps_Rk_2
        private volatile static IBaseBLL _Td_Ps_Rk_2;
        private static object lock_Td_Ps_Rk_2 = new object();
        public static IBaseBLL Td_Ps_Rk_2
        {
            get
            {
                if (_Td_Ps_Rk_2 == null)
                {
                    lock (lock_Td_Ps_Rk_2)
                    {
                        if (_Td_Ps_Rk_2 == null)
                        {
                            _Td_Ps_Rk_2 = GetObject<IBaseBLL>("Td_Ps_Rk_2Service");
                        }
                    }
                }
                return _Td_Ps_Rk_2;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ps_Fpck_1
        /// </summary>
        #region public static ITd_Ps_Fpck_1BLL _Td_Ps_Fpck_1
        private volatile static IBaseBLL _Td_Ps_Fpck_1;
        private static object lock_Td_Ps_Fpck_1 = new object();
        public static IBaseBLL Td_Ps_Fpck_1
        {
            get
            {
                if (_Td_Ps_Fpck_1 == null)
                {
                    lock (lock_Td_Ps_Fpck_1)
                    {
                        if (_Td_Ps_Fpck_1 == null)
                        {
                            _Td_Ps_Fpck_1 = GetObject<IBaseBLL>("Td_Ps_Fpck_1Service");
                        }
                    }
                }
                return _Td_Ps_Fpck_1;
            }
        }

        #endregion

        /// <summary>
        /// Td_Ps_Fpck_2
        /// </summary>
        #region public static ITd_Ps_Fpck_2BLL _Td_Ps_Fpck_2
        private volatile static IBaseBLL _Td_Ps_Fpck_2;
        private static object lock_Td_Ps_Fpck_2 = new object();
        public static IBaseBLL Td_Ps_Fpck_2
        {
            get
            {
                if (_Td_Ps_Fpck_2 == null)
                {
                    lock (lock_Td_Ps_Fpck_2)
                    {
                        if (_Td_Ps_Fpck_2 == null)
                        {
                            _Td_Ps_Fpck_2 = GetObject<IBaseBLL>("Td_Ps_Fpck_2Service");
                        }
                    }
                }
                return _Td_Ps_Fpck_2;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ps_Fprk_1
        /// </summary>
        #region public static ITd_Ps_Fprk_1BLL _Td_Ps_Fprk_1
        private volatile static IBaseBLL _Td_Ps_Fprk_1;
        private static object lock_Td_Ps_Fprk_1 = new object();
        public static IBaseBLL Td_Ps_Fprk_1
        {
            get
            {
                if (_Td_Ps_Fprk_1 == null)
                {
                    lock (lock_Td_Ps_Fprk_1)
                    {
                        if (_Td_Ps_Fprk_1 == null)
                        {
                            _Td_Ps_Fprk_1 = GetObject<IBaseBLL>("Td_Ps_Fprk_1Service");
                        }
                    }
                }
                return _Td_Ps_Fprk_1;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ps_Fprk_2
        /// </summary>
        #region public static ITd_Ps_Fprk_2BLL _Td_Ps_Fprk_2
        private volatile static IBaseBLL _Td_Ps_Fprk_2;
        private static object lock_Td_Ps_Fprk_2 = new object();
        public static IBaseBLL Td_Ps_Fprk_2
        {
            get
            {
                if (_Td_Ps_Fprk_2 == null)
                {
                    lock (lock_Td_Ps_Fprk_2)
                    {
                        if (_Td_Ps_Fprk_2 == null)
                        {
                            _Td_Ps_Fprk_2 = GetObject<IBaseBLL>("Td_Ps_Fprk_2Service");
                        }
                    }
                }
                return _Td_Ps_Fprk_2;
            }
        }
        #endregion




        /// <summary>
        /// Td_Hy_Czrule_1
        /// </summary>
        #region public static IBaseBLL _Td_Hy_Czrule_1
        private volatile static IBaseBLL _Td_Hy_Czrule_1;
        private static object lock_Td_Hy_Czrule_1 = new object();
        public static IBaseBLL Td_Hy_Czrule_1
        {
            get
            {
                if (_Td_Hy_Czrule_1 == null)
                {
                    lock (lock_Td_Hy_Czrule_1)
                    {
                        if (_Td_Hy_Czrule_1 == null)
                        {
                            _Td_Hy_Czrule_1 = GetObject<IBaseBLL>("Td_Hy_Czrule_1Service");
                        }
                    }
                }
                return _Td_Hy_Czrule_1;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Hy_Czrule
        /// </summary>
        #region public static IBaseBLL _Tb_Hy_Czrule
        private volatile static IBaseBLL _Tb_Hy_Czrule;
        private static object lock_Tb_Hy_Czrule = new object();
        public static IBaseBLL Tb_Hy_Czrule
        {
            get
            {
                if (_Tb_Hy_Czrule == null)
                {
                    lock (lock_Tb_Hy_Czrule)
                    {
                        if (_Tb_Hy_Czrule == null)
                        {
                            _Tb_Hy_Czrule = GetObject<IBaseBLL>("Tb_Hy_CzruleService");
                        }
                    }
                }
                return _Tb_Hy_Czrule;
            }
        }
        #endregion


        /// <summary>
        /// Td_Ps_Sq_1
        /// </summary>
        #region public static ITd_Ps_Sq_1BLL _Td_Ps_Sq_1
        private volatile static IBaseBLL _Td_Ps_Sq_1;
        private static object lock_Td_Ps_Sq_1 = new object();
        public static IBaseBLL Td_Ps_Sq_1
        {
            get
            {
                if (_Td_Ps_Sq_1 == null)
                {
                    lock (lock_Td_Ps_Sq_1)
                    {
                        if (_Td_Ps_Sq_1 == null)
                        {
                            _Td_Ps_Sq_1 = GetObject<IBaseBLL>("Td_Ps_Sq_1Service");
                        }
                    }
                }
                return _Td_Ps_Sq_1;
            }
        }
        #endregion


        /// <summary>
        /// Td_Ps_Sq_2
        /// </summary>
        #region public static ITd_Ps_Sq_2BLL _Td_Ps_Sq_2
        private volatile static IBaseBLL _Td_Ps_Sq_2;
        private static object lock_Td_Ps_Sq_2 = new object();
        public static IBaseBLL Td_Ps_Sq_2
        {
            get
            {
                if (_Td_Ps_Sq_2 == null)
                {
                    lock (lock_Td_Ps_Sq_2)
                    {
                        if (_Td_Ps_Sq_2 == null)
                        {
                            _Td_Ps_Sq_2 = GetObject<IBaseBLL>("Td_Ps_Sq_2Service");
                        }
                    }
                }
                return _Td_Ps_Sq_2;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ps_Dbck_1
        /// </summary>
        #region public static ITd_Ps_Dbck_1BLL _Td_Ps_Dbck_1
        private volatile static IBaseBLL _Td_Ps_Dbck_1;
        private static object lock_Td_Ps_Dbck_1 = new object();
        public static IBaseBLL Td_Ps_Dbck_1
        {
            get
            {
                if (_Td_Ps_Dbck_1 == null)
                {
                    lock (lock_Td_Ps_Dbck_1)
                    {
                        if (_Td_Ps_Dbck_1 == null)
                        {
                            _Td_Ps_Dbck_1 = GetObject<IBaseBLL>("Td_Ps_Dbck_1Service");
                        }
                    }
                }
                return _Td_Ps_Dbck_1;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ps_Dbck_2
        /// </summary>
        #region public static ITd_Ps_Dbck_2BLL _Td_Ps_Dbck_2
        private volatile static IBaseBLL _Td_Ps_Dbck_2;
        private static object lock_Td_Ps_Dbck_2 = new object();
        public static IBaseBLL Td_Ps_Dbck_2
        {
            get
            {
                if (_Td_Ps_Dbck_2 == null)
                {
                    lock (lock_Td_Ps_Dbck_2)
                    {
                        if (_Td_Ps_Dbck_2 == null)
                        {
                            _Td_Ps_Dbck_2 = GetObject<IBaseBLL>("Td_Ps_Dbck_2Service");
                        }
                    }
                }
                return _Td_Ps_Dbck_2;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ps_Dbrk_1
        /// </summary>
        #region public static ITd_Ps_Dbrk_1BLL _Td_Ps_Dbrk_1
        private volatile static IBaseBLL _Td_Ps_Dbrk_1;
        private static object lock_Td_Ps_Dbrk_1 = new object();
        public static IBaseBLL Td_Ps_Dbrk_1
        {
            get
            {
                if (_Td_Ps_Dbrk_1 == null)
                {
                    lock (lock_Td_Ps_Dbrk_1)
                    {
                        if (_Td_Ps_Dbrk_1 == null)
                        {
                            _Td_Ps_Dbrk_1 = GetObject<IBaseBLL>("Td_Ps_Dbrk_1Service");
                        }
                    }
                }
                return _Td_Ps_Dbrk_1;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ps_Dbrk_2
        /// </summary>
        #region public static ITd_Ps_Dbrk_2BLL _Td_Ps_Dbrk_2
        private volatile static IBaseBLL _Td_Ps_Dbrk_2;
        private static object lock_Td_Ps_Dbrk_2 = new object();
        public static IBaseBLL Td_Ps_Dbrk_2
        {
            get
            {
                if (_Td_Ps_Dbrk_2 == null)
                {
                    lock (lock_Td_Ps_Dbrk_2)
                    {
                        if (_Td_Ps_Dbrk_2 == null)
                        {
                            _Td_Ps_Dbrk_2 = GetObject<IBaseBLL>("Td_Ps_Dbrk_2Service");
                        }
                    }
                }
                return _Td_Ps_Dbrk_2;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ps_Cktzd_1
        /// </summary>
        #region public static ITd_Ps_Cktzd_1BLL _Td_Ps_Cktzd_1
        private volatile static IBaseBLL _Td_Ps_Cktzd_1;
        private static object lock_Td_Ps_Cktzd_1 = new object();
        public static IBaseBLL Td_Ps_Cktzd_1
        {
            get
            {
                if (_Td_Ps_Cktzd_1 == null)
                {
                    lock (lock_Td_Ps_Cktzd_1)
                    {
                        if (_Td_Ps_Cktzd_1 == null)
                        {
                            _Td_Ps_Cktzd_1 = GetObject<IBaseBLL>("Td_Ps_Cktzd_1Service");
                        }
                    }
                }
                return _Td_Ps_Cktzd_1;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ps_Cktzd_2
        /// </summary>
        #region public static ITd_Ps_Cktzd_2BLL _Td_Ps_Cktzd_2
        private volatile static IBaseBLL _Td_Ps_Cktzd_2;
        private static object lock_Td_Ps_Cktzd_2 = new object();
        public static IBaseBLL Td_Ps_Cktzd_2
        {
            get
            {
                if (_Td_Ps_Cktzd_2 == null)
                {
                    lock (lock_Td_Ps_Cktzd_2)
                    {
                        if (_Td_Ps_Cktzd_2 == null)
                        {
                            _Td_Ps_Cktzd_2 = GetObject<IBaseBLL>("Td_Ps_Cktzd_2Service");
                        }
                    }
                }
                return _Td_Ps_Cktzd_2;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ps_Fpsq_1
        /// </summary>
        #region public static ITd_Ps_Fpsq_1BLL _Td_Ps_Fpsq_1
        private volatile static IBaseBLL _Td_Ps_Fpsq_1;
        private static object lock_Td_Ps_Fpsq_1 = new object();
        public static IBaseBLL Td_Ps_Fpsq_1
        {
            get
            {
                if (_Td_Ps_Fpsq_1 == null)
                {
                    lock (lock_Td_Ps_Fpsq_1)
                    {
                        if (_Td_Ps_Fpsq_1 == null)
                        {
                            _Td_Ps_Fpsq_1 = GetObject<IBaseBLL>("Td_Ps_Fpsq_1Service");
                        }
                    }
                }
                return _Td_Ps_Fpsq_1;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ps_Fpsq_2
        /// </summary>
        #region public static ITd_Ps_Fpsq_2BLL _Td_Ps_Fpsq_2
        private volatile static IBaseBLL _Td_Ps_Fpsq_2;
        private static object lock_Td_Ps_Fpsq_2 = new object();
        public static IBaseBLL Td_Ps_Fpsq_2
        {
            get
            {
                if (_Td_Ps_Fpsq_2 == null)
                {
                    lock (lock_Td_Ps_Fpsq_2)
                    {
                        if (_Td_Ps_Fpsq_2 == null)
                        {
                            _Td_Ps_Fpsq_2 = GetObject<IBaseBLL>("Td_Ps_Fpsq_2Service");
                        }
                    }
                }
                return _Td_Ps_Fpsq_2;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ps_Fptzd_1
        /// </summary>
        #region public static ITd_Ps_Fptzd_1BLL _Td_Ps_Fptzd_1
        private volatile static IBaseBLL _Td_Ps_Fptzd_1;
        private static object lock_Td_Ps_Fptzd_1 = new object();
        public static IBaseBLL Td_Ps_Fptzd_1
        {
            get
            {
                if (_Td_Ps_Fptzd_1 == null)
                {
                    lock (lock_Td_Ps_Fptzd_1)
                    {
                        if (_Td_Ps_Fptzd_1 == null)
                        {
                            _Td_Ps_Fptzd_1 = GetObject<IBaseBLL>("Td_Ps_Fptzd_1Service");
                        }
                    }
                }
                return _Td_Ps_Fptzd_1;
            }
        }
        #endregion

        /// <summary>
        /// Td_Ps_Fptzd_2
        /// </summary>
        #region public static ITd_Ps_Fptzd_2BLL _Td_Ps_Fptzd_2
        private volatile static IBaseBLL _Td_Ps_Fptzd_2;
        private static object lock_Td_Ps_Fptzd_2 = new object();
        public static IBaseBLL Td_Ps_Fptzd_2
        {
            get
            {
                if (_Td_Ps_Fptzd_2 == null)
                {
                    lock (lock_Td_Ps_Fptzd_2)
                    {
                        if (_Td_Ps_Fptzd_2 == null)
                        {
                            _Td_Ps_Fptzd_2 = GetObject<IBaseBLL>("Td_Ps_Fptzd_2Service");
                        }
                    }
                }
                return _Td_Ps_Fptzd_2;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Hy_Jfrule
        /// </summary>
        #region public static ITb_Hy_JfruleBLL _Tb_Hy_Jfrule
        private volatile static IBaseBLL _Tb_Hy_Jfrule;
        private static object lock_Tb_Hy_Jfrule = new object();
        public static IBaseBLL Tb_Hy_Jfrule
        {
            get
            {
                if (_Tb_Hy_Jfrule == null)
                {
                    lock (lock_Tb_Hy_Jfrule)
                    {
                        if (_Tb_Hy_Jfrule == null)
                        {
                            _Tb_Hy_Jfrule = GetObject<IBaseBLL>("Tb_Hy_JfruleService");
                        }
                    }
                }
                return _Tb_Hy_Jfrule;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Hy_Czrule_Zssp
        /// </summary>
        #region public static IBaseBLL _Tb_Hy_Czrule_Zssp
        private volatile static IBaseBLL _Tb_Hy_Czrule_Zssp;
        private static object lock_Tb_Hy_Czrule_Zssp = new object();
        public static IBaseBLL Tb_Hy_Czrule_Zssp
        {
            get
            {
                if (_Tb_Hy_Czrule_Zssp == null)
                {
                    lock (lock_Tb_Hy_Czrule_Zssp)
                    {
                        if (_Tb_Hy_Czrule_Zssp == null)
                        {
                            _Tb_Hy_Czrule_Zssp = GetObject<IBaseBLL>("Tb_Hy_Czrule_ZsspService");
                        }
                    }
                }
                return _Tb_Hy_Czrule_Zssp;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Hy_Jfconvertsp
        /// </summary>
        #region public static IBaseBLL _Tb_Hy_Jfconvertsp
        private volatile static IBaseBLL _Tb_Hy_Jfconvertsp;
        private static object lock_Tb_Hy_Jfconvertsp = new object();
        public static IBaseBLL Tb_Hy_Jfconvertsp
        {
            get
            {
                if (_Tb_Hy_Jfconvertsp == null)
                {
                    lock (lock_Tb_Hy_Jfconvertsp)
                    {
                        if (_Tb_Hy_Jfconvertsp == null)
                        {
                            _Tb_Hy_Jfconvertsp = GetObject<IBaseBLL>("Tb_Hy_JfconvertspService");
                        }
                    }
                }
                return _Tb_Hy_Jfconvertsp;
            }
        }
        #endregion


        #region public static IPayBLL Pay
        private volatile static IPayBLL _Pay;
        private static object lock_Pay = new object();
        public static IPayBLL Pay
        {
            get
            {
                if (_Pay == null)
                {
                    lock (lock_Pay)
                    {
                        if (_Pay == null)
                        {
                            _Pay = GetObject<IPayBLL>("PayService");
                        }
                    }
                }
                return _Pay;
            }
        }
        #endregion


        /// <summary>
        /// Td_Kc_Sltz_1
        /// </summary>
        #region public static ITd_Kc_Sltz_1BLL _Td_Kc_Sltz_1
        private volatile static IBaseBLL _Td_Kc_Sltz_1;
        private static object lock_Td_Kc_Sltz_1 = new object();
        public static IBaseBLL Td_Kc_Sltz_1
        {
            get
            {
                if (_Td_Kc_Sltz_1 == null)
                {
                    lock (lock_Td_Kc_Sltz_1)
                    {
                        if (_Td_Kc_Sltz_1 == null)
                        {
                            _Td_Kc_Sltz_1 = GetObject<IBaseBLL>("Td_Kc_Sltz_1Service");
                        }
                    }
                }
                return _Td_Kc_Sltz_1;
            }
        }
        #endregion



        /// <summary>
        /// Ts_Notice
        /// </summary>
        #region public static ITs_NoticeBLL _Ts_Notice
        private volatile static IBaseBLL _Ts_Notice;
        private static object lock_Ts_Notice = new object();
        public static IBaseBLL Ts_Notice
        {
            get
            {
                if (_Ts_Notice == null)
                {
                    lock (lock_Ts_Notice)
                    {
                        if (_Ts_Notice == null)
                        {
                            _Ts_Notice = GetObject<IBaseBLL>("Ts_NoticeService");
                        }
                    }
                }
                return _Ts_Notice;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Khfl
        /// </summary>
        #region public static IBaseBLL _Tb_Khfl
        private volatile static ITb_KhflBLL _Tb_Khfl;
        private static object lock_Tb_Khfl = new object();
        public static ITb_KhflBLL Tb_Khfl
        {
            get
            {
                if (_Tb_Khfl == null)
                {
                    lock (lock_Tb_Khfl)
                    {
                        if (_Tb_Khfl == null)
                        {
                            _Tb_Khfl = GetObject<ITb_KhflBLL>("Tb_KhflService");
                        }
                    }
                }
                return _Tb_Khfl;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Kh
        /// </summary>
        #region public static ITb_KhBLL _Tb_Kh
        private volatile static IBaseBLL _Tb_Kh;
        private static object lock_Tb_Kh = new object();
        public static IBaseBLL Tb_Kh
        {
            get
            {
                if (_Tb_Kh == null)
                {
                    lock (lock_Tb_Kh)
                    {
                        if (_Tb_Kh == null)
                        {
                            _Tb_Kh = GetObject<IBaseBLL>("Tb_KhService");
                        }
                    }
                }
                return _Tb_Kh;
            }
        }
        #endregion


        /// <summary>
        /// Td_Sk_1
        /// </summary>
        #region public static IBaseBLL _Td_Sk_1
        private volatile static IBaseBLL _Td_Sk_1;
        private static object lock_Td_Sk_1 = new object();
        public static IBaseBLL Td_Sk_1
        {
            get
            {
                if (_Td_Sk_1 == null)
                {
                    lock (lock_Td_Sk_1)
                    {
                        if (_Td_Sk_1 == null)
                        {
                            _Td_Sk_1 = GetObject<IBaseBLL>("Td_Sk_1Service");
                        }
                    }
                }
                return _Td_Sk_1;
            }
        }
        #endregion



        /// <summary>
        /// Td_Sk_2
        /// </summary>
        #region public static IBaseBLL _Td_Sk_2
        private volatile static IBaseBLL _Td_Sk_2;
        private static object lock_Td_Sk_2 = new object();
        public static IBaseBLL Td_Sk_2
        {
            get
            {
                if (_Td_Sk_2 == null)
                {
                    lock (lock_Td_Sk_2)
                    {
                        if (_Td_Sk_2 == null)
                        {
                            _Td_Sk_2 = GetObject<IBaseBLL>("Td_Sk_2Service");
                        }
                    }
                }
                return _Td_Sk_2;
            }
        }
        #endregion



        /// <summary>
        /// Tz_Ys
        /// </summary>
        #region public static ITz_YsBLL _Tz_Ys
        private volatile static IBaseBLL _Tz_Ys;
        private static object lock_Tz_Ys = new object();
        public static IBaseBLL Tz_Ys
        {
            get
            {
                if (_Tz_Ys == null)
                {
                    lock (lock_Tz_Ys)
                    {
                        if (_Tz_Ys == null)
                        {
                            _Tz_Ys = GetObject<IBaseBLL>("Tz_YsService");
                        }
                    }
                }
                return _Tz_Ys;
            }
        }
        #endregion



        /// <summary>
        /// Tz_Ys_Jsc
        /// </summary>
        #region public static IBaseBLL _Tz_Ys_Jsc
        private volatile static IBaseBLL _Tz_Ys_Jsc;
        private static object lock_Tz_Ys_Jsc = new object();
        public static IBaseBLL Tz_Ys_Jsc
        {
            get
            {
                if (_Tz_Ys_Jsc == null)
                {
                    lock (lock_Tz_Ys_Jsc)
                    {
                        if (_Tz_Ys_Jsc == null)
                        {
                            _Tz_Ys_Jsc = GetObject<IBaseBLL>("Tz_Ys_JscService");
                        }
                    }
                }
                return _Tz_Ys_Jsc;
            }
        }
        #endregion


        /// <summary>
        /// Tz_Ys_Flow
        /// </summary>
        #region public static ITz_Ys_FlowBLL _Tz_Ys_Flow
        private volatile static IBaseBLL _Tz_Ys_Flow;
        private static object lock_Tz_Ys_Flow = new object();
        public static IBaseBLL Tz_Ys_Flow
        {
            get
            {
                if (_Tz_Ys_Flow == null)
                {
                    lock (lock_Tz_Ys_Flow)
                    {
                        if (_Tz_Ys_Flow == null)
                        {
                            _Tz_Ys_Flow = GetObject<IBaseBLL>("Tz_Ys_FlowService");
                        }
                    }
                }
                return _Tz_Ys_Flow;
            }
        }
        #endregion



        /// <summary>
        /// Td_Xs_Dd_1
        /// </summary>
        #region public static IBaseBLL _Td_Xs_Dd_1
        private volatile static IBaseBLL _Td_Xs_Dd_1;
        private static object lock_Td_Xs_Dd_1 = new object();
        public static IBaseBLL Td_Xs_Dd_1
        {
            get
            {
                if (_Td_Xs_Dd_1 == null)
                {
                    lock (lock_Td_Xs_Dd_1)
                    {
                        if (_Td_Xs_Dd_1 == null)
                        {
                            _Td_Xs_Dd_1 = GetObject<IBaseBLL>("Td_Xs_Dd_1Service");
                        }
                    }
                }
                return _Td_Xs_Dd_1;
            }
        }
        #endregion


        /// <summary>
        /// Td_Xs_1
        /// </summary>
        #region public static IBaseBLL _Td_Xs_1
        private volatile static IBaseBLL _Td_Xs_1;
        private static object lock_Td_Xs_1 = new object();
        public static IBaseBLL Td_Xs_1
        {
            get
            {
                if (_Td_Xs_1 == null)
                {
                    lock (lock_Td_Xs_1)
                    {
                        if (_Td_Xs_1 == null)
                        {
                            _Td_Xs_1 = GetObject<IBaseBLL>("Td_Xs_1Service");
                        }
                    }
                }
                return _Td_Xs_1;
            }
        }
        #endregion

        /// <summary>
        /// Td_Xs_Th_1
        /// </summary>
        #region public static IBaseBLL _Td_Xs_Th_1
        private volatile static IBaseBLL _Td_Xs_Th_1;
        private static object lock_Td_Xs_Th_1 = new object();
        public static IBaseBLL Td_Xs_Th_1
        {
            get
            {
                if (_Td_Xs_Th_1 == null)
                {
                    lock (lock_Td_Xs_Th_1)
                    {
                        if (_Td_Xs_Th_1 == null)
                        {
                            _Td_Xs_Th_1 = GetObject<IBaseBLL>("Td_Xs_Th_1Service");
                        }
                    }
                }
                return _Td_Xs_Th_1;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Sp_Jgzh
        /// </summary>
        #region public static ITb_Sp_JgzhBLL _Tb_Sp_Jgzh
        private volatile static IBaseBLL _Tb_Sp_Jgzh;
        private static object lock_Tb_Sp_Jgzh = new object();
        public static IBaseBLL Tb_Sp_Jgzh
        {
            get
            {
                if (_Tb_Sp_Jgzh == null)
                {
                    lock (lock_Tb_Sp_Jgzh)
                    {
                        if (_Tb_Sp_Jgzh == null)
                        {
                            _Tb_Sp_Jgzh = GetObject<IBaseBLL>("Tb_Sp_JgzhService");
                        }
                    }
                }
                return _Tb_Sp_Jgzh;
            }
        }
        #endregion


        /// <summary>
        /// Tb_Sp_Fgcf
        /// </summary>
        #region public static ITb_Sp_FgcfBLL _Tb_Sp_Fgcf
        private volatile static IBaseBLL _Tb_Sp_Fgcf;
        private static object lock_Tb_Sp_Fgcf = new object();
        public static IBaseBLL Tb_Sp_Fgcf
        {
            get
            {
                if (_Tb_Sp_Fgcf == null)
                {
                    lock (lock_Tb_Sp_Fgcf)
                    {
                        if (_Tb_Sp_Fgcf == null)
                        {
                            _Tb_Sp_Fgcf = GetObject<IBaseBLL>("Tb_Sp_FgcfService");
                        }
                    }
                }
                return _Tb_Sp_Fgcf;
            }
        }
        #endregion
        

    }
}
