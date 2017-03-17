using System;

namespace CySoft.Model.Tb
{
    /// <summary>
    /// 采购关系状态
    /// </summary>
    public static class Gys_Cgs_Status
    {
        /// <summary>
        /// 创建采购商
        /// </summary>
        public static string Create = "create";//创建采购商
        /// <summary>
        /// 供应商为采购商创建用户
        /// </summary>
        public static string AddUser = "adduser";//供应商为采购商创建用户
        /// <summary>
        /// 暂停关系
        /// </summary>
        public static string Stop = "stop";//暂停关系
        /// <summary>
        /// 恢复关系
        /// </summary>
        public static string Active = "Active";//恢复关系
        /// <summary>
        /// 申请成为采购商
        /// </summary>
        public static string Apply = "apply";//申请成为采购商
        /// <summary>
        /// 通过申请，已关注
        /// </summary>
        public static string Accept = "accept";//通过申请，已关注
        /// <summary>
        /// 拒绝申请
        /// </summary>
        public static string Refuse = "refuse";//拒绝申请
        /// <summary>
        /// 撤消申请
        /// </summary>
        public static string Cancel = "cancel";//撤消申请
        /// <summary>
        /// 采购商结束关注
        /// </summary>
        public static string CallBack = "callback";//采购商结束关注
        /// <summary>
        /// 供应商结束关注
        /// </summary>
        public static string End = "end";//供应商结束关注
    }
}
