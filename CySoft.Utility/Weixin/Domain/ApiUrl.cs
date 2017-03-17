namespace CySoft.Utility.Weixin.Domain
{
    public sealed class ApiUrl
    {
        /// <summary>
        /// 取Token
        /// </summary>
        public const string GetToken = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        /// <summary>
        /// 取JSSPI Ticket
        /// </summary>
        public const string GetTicket = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";

        #region 自定义菜单

        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        public const string MenuCreate = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";

        /// <summary>
        /// 查询自定义菜单
        /// </summary>
        public const string MenuGet = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}";

        /// <summary>
        /// 删除自定义菜单
        /// </summary>
        public const string MenuDelete = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";

        #endregion 自定义菜单

        /// <summary>
        /// 取 snsapi_base code
        /// </summary>
        public const string SnaApi_Base_GetCode = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect";

        /// <summary>
        /// 取 snsapi_userinfo code
        /// </summary>
        public const string SnaApi_Userinfo_GetCode = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";

        /// <summary>
        /// 取SNS OAuth2 Token
        /// </summary>
        public const string GetToken_SNS = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";

        /// <summary>
        /// 取用户基本信息（包括UnionID机制）
        /// </summary>
        public const string GetUserInfo = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}";
    }
}
