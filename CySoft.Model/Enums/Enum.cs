using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Enums
{
    public class Enums
    {
        public enum ServiceStatusCode
        {
            Success = 0,

            Error = 1
        }

        /// <summary>
        /// 停用启用状态
        /// </summary>
        public enum FlagStop
        {
            /// <summary>
            /// 启用
            /// </summary>
            Start = 0,
            /// <summary>
            /// 停用
            /// </summary>
            Stopped = 1
        }
        /// <summary>
        /// 审核状态
        /// </summary>
        public enum FlagSh
        {
            /// <summary>
            /// 未审核
            /// </summary>
            UnSh = 0,
            /// <summary>
            /// 已审核
            /// </summary>
            HadSh = 1
        }

        /// <summary>
        /// 删除状态枚举
        /// </summary>
        public enum FlagDelete
        {
            /// <summary>
            /// 未删
            /// </summary>
            NoDelete = 0,
            /// <summary>
            /// 已删
            /// </summary>
            Deleted = 1
        }

        /// <summary>
        /// 作废状态枚举
        /// </summary>
        public enum FlagCancel
        {
            /// <summary>
            /// 未作废
            /// </summary>
            NoCancel = 0,
            /// <summary>
            /// 已作废
            /// </summary>
            Canceled = 1
        }

        /// <summary>
        /// Tb_Pay表中flag_state枚举
        /// </summary>
        public enum TbPayFlagState
        {
            /// <summary>
            /// 启用
            /// </summary>
            Used = 0,
            /// <summary>
            /// 停用
            /// </summary>
            Stoped = 1
        }
        /// <summary>
        /// Ts_flag表中的listcode
        /// </summary>
        public enum TsFlagListCode
        {
            /// <summary>
            /// 支付方式表中支付类型
            /// </summary>
            paytype,
            /// <summary>
            /// Tb_Pos_Function表中所属系统
            /// </summary>
            TbPosFunctionFlagSystem,
            /// <summary>
            /// 商品生命周期 (商品状态)
            /// </summary>
            spstate,
            /// <summary>
            /// 称重方式
            /// </summary>
            spczfs,
            /// <summary>
            /// 供应商状态
            /// </summary>
            gysstate,
            /// <summary>
            /// 会员优惠类型
            /// </summary>
            hyyhlx,
            /// <summary>
            /// 截取小数类型
            /// </summary>
            digitlx,
            /// <summary>
            /// 门店类型 
            /// </summary>
            shoptype,
            /// <summary>
            /// 客户状态
            /// </summary>
            khstate


        }
        /// <summary>
        /// Tb_role表中flag_type字段
        /// </summary>
        public enum TbRoleFlagType
        {
            /// <summary>
            /// 平台角色
            /// </summary>
            PlatformRole = 1,
            /// <summary>
            /// 系统角色
            /// </summary>
            SystemRole = 2,
            /// <summary>
            /// 角色模板
            /// </summary>
            RoleTemp = 9,
            /// <summary>
            /// 从属主用户角色
            /// </summary>
            NoMasterRole = 10
        }
        /// <summary>
        /// Tb_role表中flag_update字段
        /// </summary>
        public enum TbRoleFlagUpdate
        {
            /// <summary>
            /// 可修改
            /// </summary>
            UnUpdate = 0,
            /// <summary>
            /// 不可修改
            /// </summary>
            CanUpdate = 1
        }
        /// <summary>
        /// Tb_role表中固定角色ID
        /// </summary>
        public enum TbRoleFixedRoleId
        {
            /// <summary>
            /// 1 平台管理员
            /// </summary>
            PlatformManager = 1,
            /// <summary>
            /// 2 系统管理员
            /// </summary>
            SysManager = 2,
            /// <summary>
            /// 3 业务员
            /// </summary>
            BusinessMan = 3
        }

        /// <summary>
        /// Tb_shop表中的flag_state
        /// </summary>
        public enum TbShopFlagState
        {
            /// <summary>
            /// 关闭
            /// </summary>
            Closed = 0,
            /// <summary>
            /// 正常开店
            /// </summary>
            Opened = 1
        }


        public enum CYServiceStopFlag
        {
            /// <summary>
            /// 停用
            /// </summary>
            Stoped = 1,
            /// <summary>
            /// 正常启用
            /// </summary>
            Used = 0
        }

        /// <summary>
        /// Tb_User表中flag_master字段
        /// </summary>
        public enum TbUserFlagMaster
        {
            /// <summary>
            /// 非主账户
            /// </summary>
            UnMaster = 0,
            /// <summary>
            /// 主账户
            /// </summary>
            Master = 1
        }
        /// <summary>
        /// TbUserFlagState表中的flag_state
        /// </summary>
        public enum TbUserFlagState
        {
            /// <summary>
            /// 启用
            /// </summary>
            Yes = 1,
            /// <summary>
            /// 停用
            /// </summary>
            No = 0
        }
        /// <summary>
        /// Tb_Pos_Function表中FlagType字段
        /// </summary>
        public enum TbPosFunctionFlagType
        {
            /// <summary>
            /// 模块
            /// </summary>
            module = 1,
            /// <summary>
            /// 控制器
            /// </summary>
            controller = 2,
            /// <summary>
            /// 方法
            /// </summary>
            action = 3
        }
        /// <summary>
        /// 程序功能版本
        /// </summary>
        public enum FuncVersion
        {
            /// <summary>
            /// 单店
            /// </summary>
            [Description("云POS单店版收银管理软件V1.0")]
            SingleShop = 10,
            /// <summary>
            /// 连锁
            /// </summary>
            [Description("云POS连锁版收银管理软件V1.0")]
            MultipleShop = 20,
            /// <summary>
            /// 集团
            /// </summary>
            [Description("云POS集团版收银管理软件V1.0")]
            MinmetalsShop = 30
        }
        /// <summary>
        /// tb_user_shop表中flag_default字段
        /// </summary>
        public enum TbUserShopFlagDefault
        {
            /// <summary>
            /// 默认
            /// </summary>
            Default = 0,
            /// <summary>
            /// 非默认
            /// </summary>
            UnDefault = 1
        }

        /// <summary>
        /// ts_parm表中hy_shopshare枚举
        /// </summary>
        public enum FlagShopShare
        {
            /// <summary>
            /// 未共用
            /// </summary>
            NoShare = 0,
            /// <summary>
            /// 共用
            /// </summary>
            Shared = 1
        }

        public enum TpPosMachineFlagState
        {
            /// <summary>
            /// 未用
            /// </summary>
            NoUse = 0,
            /// <summary>
            /// 已用
            /// </summary>
            Used = 1,
            /// <summary>
            /// 暂停
            /// </summary>
            Stop = 2,
            /// <summary>
            /// 停用
            /// </summary>
            Closed = 3
        }

        /// <summary>
        /// ts_shopsp表中停用枚举
        /// </summary>
        public enum FlagShopspStop
        {
            /// <summary>
            /// 正常
            /// </summary>
            NoStop = 1,
            /// <summary>
            /// 停用
            /// </summary>
            Stoped = 2,
            /// <summary>
            /// 删除
            /// </summary>
            Deleted = 9
        }



        /// <summary>
        /// 用于生成单号的类型枚举 
        /// lz
        /// 2016-11-07
        /// </summary>
        public enum FlagDJLX
        {
            /// <summary>
            /// 门店编码
            /// </summary>
            BMShop = 1,
            /// <summary>
            /// 商品编码
            /// </summary>
            BMShopsp = 2,
            /// <summary>
            /// 订货单号
            /// </summary>
            DHDH = 3,
            /// <summary>
            /// 进货单号
            /// </summary>
            DHJH = 4,
            /// <summary>
            /// 付款单号
            /// </summary>
            DHJHFK = 5,
            /// <summary>
            /// 退货单号
            /// </summary>
            DHTH = 6,
            /// <summary>
            /// 充值单号
            /// </summary>
            DHCZ = 7,
            /// <summary>
            /// 快速盘点单号
            /// </summary>
            DHKSPD = 8,
            /// <summary>
            /// 商品编码-称重方式不为普通的
            /// </summary>
            BMShopspCZFS = 9,
            /// <summary>
            /// 收款单号
            /// </summary>
            DHSK = 10,

            /// <summary>
            /// 销售订单
            /// </summary>
            XSDD=11,
            /// <summary>
            /// 销售出库
            /// </summary>
            XSCK = 12,
            /// <summary>
            /// 销售退货
            /// </summary>
            XSTH = 13



        }

        /// <summary>
        /// 门店数据类型
        /// </summary>
        public enum ShopDataType
        {
            /// <summary>
            /// 主用户下所有门店 所有门店 包括开启与未开启的
            /// </summary>
            All_State,
            /// <summary>
            /// 主用户下所有门店
            /// </summary>
            All,
            /// <summary>
            /// 除主用户门店外的所有门店
            /// </summary>
            NotExistMaster,
            /// <summary>
            /// 除当前登录用户所在门店之外的所有门店
            /// </summary>
            NotExistCurrentShop,
            /// <summary>
            /// 当前用户管理门店--如果没有则只有自己的门店
            /// </summary>
            UserShopOnly,
            /// <summary>
            /// 当前用户管理门店--如果没有则什么都没有
            /// </summary>
            UserShopOnlyNone,
            /// <summary>
            /// 当前用户管理门店-如果没有就查询所有的门店
            /// </summary>
            UserShop,
            /// <summary>
            /// ShopShop表中 flag_delete=0 shop_state=9 正常开店的
            /// </summary>
            ShopShop,
            /// <summary>
            /// ShopShop表中 所有的数据
            /// </summary>
            ShopShopAll,
            /// <summary>
            /// 根据门店id获取本门店和自己子集的配送中心
            /// </summary>
            GetPSZXListForAdd,
            /// <summary>
            /// 根据门店id获取父门店和自己平级的配送中心
            /// </summary>
            GetPSZXListForEdit,
            /// <summary>
            /// 根据门店id获取本级加下级的门店
            /// </summary>
            GetBJXJList,
            /// <summary>
            /// 根据shop_shop关系表 只获取父门店 如果为主门店则是自己 
            /// </summary>
            GetFatherList

        }
        /// <summary>
        /// 配送单类型
        /// </summary>
        public enum Ps
        {
            /// <summary>
            /// 补货申请单
            /// </summary>
            PS010,
            /// <summary>
            /// 配送通知单
            /// </summary>
            PS110,
            /// <summary>
            /// 配送出库单
            /// </summary>
            PS120,
            /// <summary>
            /// 配送入库单
            /// </summary>
            PS130,
            /// <summary>
            /// 返配申请单
            /// </summary>
            PS210,
            /// <summary>
            /// 返配通知单
            /// </summary>
            PS220,
            /// <summary>
            /// 返配出库单
            /// </summary>
            PS230,
            /// <summary>
            /// 返配入库单
            /// </summary>
            PS240,
            /// <summary>
            /// 调拨出库单
            /// </summary>
            PS310,
            /// <summary>
            /// 调拨入库单
            /// </summary>
            PS320,
            /// <summary>
            /// 收款单
            /// </summary>
            CW002,
            /// <summary>
            /// 销售订单
            /// </summary>
            XS010,
            /// <summary>
            /// 销售出库
            /// </summary>
            XS020,
            /// <summary>
            /// 销售退货
            /// </summary>
            XS030
        }

        public enum Kspd
        {
            /// <summary>
            /// 库存盘点单
            /// </summary>
            KC010,
            /// <summary>
            /// 库存调整
            /// </summary>
            KC020
        }
        /// <summary>
        /// 商品查询字段
        /// </summary>
        public enum ShopspQueryLikeFiled
        {
            barcode_like,
            mc_like
        }

        /// <summary>
        /// 门店类型枚举
        /// </summary>
        public enum FlagShopType
        {
            /// <summary>
            /// 总部
            /// </summary>
            总部 = 1,
            /// <summary>
            /// 分公司
            /// </summary>
            分公司 = 2,

            /// <summary>
            /// 配送中心
            /// </summary>
            配送中心 = 3,
            /// <summary>
            /// 直营店
            /// </summary>
            直营店 = 4,
            /// <summary>
            /// 加盟店
            /// </summary>
            加盟店 = 5

        }
        /// <summary>
        /// 门店关系表FlagState
        /// </summary>
        public enum ShopShopFlagState
        {
            /// <summary>
            /// 停用
            /// </summary>
            Stop = 0,
            /// <summary>
            /// 启用
            /// </summary>
            Use = 1
        }

        /// <summary>
        /// 收款类型枚举
        /// </summary>
        public enum FlagSKLX
        {
            /// <summary>
            /// 应收
            /// </summary>
            YingShou = 1,
            /// <summary>
            /// 预收
            /// </summary>
            YuShou = 2
        }

        /// <summary>
        /// 门店版本
        /// </summary>
        public enum ShopVersion
        {
            /// <summary>
            /// 单店
            /// </summary>
            ddb = 10,
            /// <summary>
            /// 连锁
            /// </summary>
            lsb = 20,
            /// <summary>
            /// 集团
            /// </summary>
            jtb = 30
        }


    }

}
