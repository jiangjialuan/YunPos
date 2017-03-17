using CySoft.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CySoft.Model.Pay
{
    /// <summary>
    /// 开通在线支付模型
    /// </summary>
    [Serializable]
    [Table("RegisterPay", "RegisterPay")]
    public class RegisterPay
    {
        public RegisterPay()
        {
            manualsettle = 'N';
            rq_create = DateTime.Now;
            active = 1;
            feetype = "SOURCE";
        }
        /// <summary>
        /// 子账户编号
        /// </summary>
        public string ledgerno { get; set; }
        /// <summary>
        /// 主用户
        /// </summary>
        public long id_user_master { get; set; }
        /// <summary>
        ///  绑定手机
        /// </summary>
        public string bindmobile { get; set; }
        /// <summary>
        /// 注册类型,个人PERSON，企业ENTERPRISE
        /// </summary>
        public string customertype { get; set; }
        /// <summary>
        /// 签约名，姓名或企业名
        /// </summary>
        public string signedname { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string linkman { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string idcard { get; set; }
        /// <summary>
        /// 营业执照号
        /// </summary>
        public string businesslicence { get; set; }
        /// <summary>
        /// 法人姓名
        /// </summary>
        public string legalperson { get; set; }
        /// <summary>
        /// 起结金额
        /// </summary>
        public string minsettleamount { get; set; }
        /// <summary>
        /// 结算周期
        /// </summary>
        public string riskreserveday { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        public string bankaccountnumber { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string bankname { get; set; }
        /// <summary>
        /// 开户名
        /// </summary>
        public string accountname { get; set; }
        /// <summary>
        /// 银行卡类型,对私PrivateCash，对公PublicCash
        /// </summary>
        public string bankaccounttype { get; set; }
        /// <summary>
        /// 开户省
        /// </summary>
        public string bankprovince { get; set; }
        /// <summary>
        /// 开户市
        /// </summary>
        public string bankcity { get; set; }
        /// <summary>
        /// 自助结算  Y/N
        /// </summary>
        public char manualsettle { get; set; }
        /// <summary>
        /// 账户状态  0激活1冻结
        /// </summary>
        public int active { get; set; }
        /// <summary>
        /// 手续费扣除类型SOURCE （ 手续费从户余额扣除） 或者 TARGET（从出款金额扣除）
        /// </summary>
        public string feetype { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime rq_create { get; set; }
    }
}
