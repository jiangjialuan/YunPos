using CySoft.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Pay
{
    /// <summary>
    /// 微信账户信息表
    /// </summary>
    [Serializable]
    [Table("WeChatAccount", "WeChatAccount")]
    public class WeChatAccount
    {
        /// <summary>
        /// 主用户
        /// </summary>
        public long id_user_master { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mchid { get; set; }
        /// <summary>
        /// 应用id
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 应用密钥
        /// </summary>
        public string appsecret { get; set; }
        /// <summary>
        /// api密钥
        /// </summary>
        public string appkey { get; set; }
        /// <summary>
        /// 启用状态 0停用  1启用
        /// </summary>
        public int flag_state { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        public long id_edit { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? rq_edit { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        public long id_create { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? rq_create { get; set; }
        /// <summary>
        /// 令牌
        /// </summary>
        public string token { get; set; }
    }
}
