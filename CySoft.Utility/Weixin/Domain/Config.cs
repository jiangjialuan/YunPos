namespace CySoft.Utility.Weixin.Domain
{
    public static class Config
    {
        private static int _token_expires = 6600;
        private static int _ticket_expires = 6000;


        /// <summary>
        /// Token缓存有效期，单位秒
        /// </summary>
        public static int Token_Expires
        {
            get
            {
                return _token_expires;
            }
            set
            {
                if (_token_expires == value)
                    return;
                _token_expires = value;
            }
        }

        /// <summary>
        /// Ticket缓存有效期，单位秒
        /// </summary>
        public static int Ticket_Expires
        {
            get
            {
                return _ticket_expires;
            }
            set
            {
                if (_ticket_expires == value)
                    return;
                _ticket_expires = value;
            }
        }
    }
}