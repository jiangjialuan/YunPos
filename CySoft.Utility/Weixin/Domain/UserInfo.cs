using System;
using System.Collections.Generic;

namespace CySoft.Utility.Weixin.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class UserInfo
    {
        public UserInfo()
        {
            this.privilege = new List<string>();
        }

        public string openid { get; set; }

        public string nickname { get; set; }

        public Nullable<int> sex { get; set; }

        public string city { get; set; }

        public string country { get; set; }

        public string province { get; set; }

        public string language { get; set; }

        public string headimgurl { get; set; }

        public string unionid { get; set; }

        public List<string> privilege { get; set; }
    }
}

