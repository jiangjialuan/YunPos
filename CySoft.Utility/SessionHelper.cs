using System.Web;
using System.Web.SessionState;
using CySoft.Frame.Common;

namespace CySoft.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class SessionHelper
    {
        /// <summary>
        /// 是否启用Redis
        /// </summary>
        private static readonly bool isRedis = AppConfig.ExistNode("RedisHost");
        private static HttpSessionState _session = HttpContext.Current.Session;

        public static string Get(string str_key)
        {
            string result = null;
            try
            {
                object obj = null;
                if (isRedis)
                {
                    string sid = _session.SessionID;        //此处代替Session功能，需要记录SessionID
                    if (RedisHelper.Exists(string.Format("{0}_{1}", sid, str_key)))
                        obj = RedisHelper.Get(string.Format("{0}_{1}", sid, str_key));
                }
                else
                {
                    obj = _session[str_key];
                }

                if (null != obj)
                    result = obj.ToString();
            }
            catch (System.Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
            }

            return result;
        }

        public static T Get<T>(string obj_key)
            where T : class, new()
        {
            T result = null;
            try
            {
                if (isRedis)
                {
                    string sid = _session.SessionID;        //此处代替Session功能，需要记录SessionID
                    if (RedisHelper.Exists(string.Format("{0}_{1}", sid, obj_key)))
                        result = RedisHelper.Get<T>(string.Format("{0}_{1}", sid, obj_key));
                }
                else
                {
                    object obj = _session[obj_key];
                    if (null != obj)
                        result = (obj as T);
                }
            }
            catch (System.Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
            }

            return result;
        }


        public static bool Set(string key, dynamic str_json)
        {
            bool success = true;

            if (null != str_json)
            {
                try
                {
                    if (isRedis)
                    {
                        string sid = _session.SessionID;        //此处代替Session功能，需要记录SessionID
                        RedisHelper.Set(string.Format("{0}_{1}", sid, key), str_json);
                    }
                    else
                    {
                        _session[key] = str_json;
                    }
                }
                catch (System.Exception ex)
                {
                    success = false;
                    TextLogHelper.WriterExceptionLog(ex);
                }
            }

            return success;
        }
    }
}
