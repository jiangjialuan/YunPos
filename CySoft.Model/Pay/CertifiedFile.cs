using CySoft.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Pay
{
    /// <summary>
    /// 认证资料表
    /// </summary>
    [Serializable]
    [Table("CertifiedFile", "CertifiedFile")]
    public class CertifiedFile
    {
        /// <summary>
        /// 子账户编号
        /// </summary>
        public string ledgerno { get; set; }
        /// <summary>
        /// 主用户
        /// </summary>
        public long id_user_master { get; set; }
        /// <summary>
        /// 身份证正面
        /// </summary>
        public string id_card_front { get; set; }
        /// <summary>
        /// 身份证背面
        /// </summary>
        public string id_card_back { get; set; }
        /// <summary>
        /// 银行卡正面   
        /// </summary>
        public string bank_card_front { get; set; }
        /// <summary>
        /// 银行卡背面
        /// </summary>
        public string bank_card_back { get; set; }
        /// <summary>
        /// 手持身份证照
        /// </summary>
        public string person_photo { get; set; }
        /// <summary>
        /// 营业执照
        /// </summary>
        public string bussiness_license { get; set; }
        /// <summary>
        /// 工商证
        /// </summary>
        public string bussiness_certificates { get; set; }
        /// <summary>
        /// 组织机构代码证
        /// </summary>
        public string organization_code { get; set; }
        /// <summary>
        /// 税务登记证
        /// </summary>
        public string tax_registration { get; set; }
        /// <summary>
        /// 银行开户许可证 
        /// </summary>
        public string bank_account_licence { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime rq_create { get; set; }
    }
}
