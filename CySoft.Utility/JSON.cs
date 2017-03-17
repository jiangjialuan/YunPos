using System;
using System.Web.Script.Serialization;
using CySoft.Frame.Common;
using Newtonsoft.Json;

namespace CySoft.Utility
{
    public static class JSON
    {
        /// <summary>
        /// 序列化及范序列化提供程序
        /// </summary>
        #region protected JavaScriptSerializer Serializer
        private static object javaScriptSerializerLock = new object();
        private static volatile JavaScriptSerializer _Serializer;
        private static JavaScriptSerializer Serializer
        {
            get
            {
                if (_Serializer == null)
                {
                    lock (javaScriptSerializerLock)
                    {
                        if (_Serializer == null)
                        {
                            _Serializer = new JavaScriptSerializer();
                        }
                    }
                }
                return _Serializer;
            }
        }
        #endregion

        /// <summary>
        /// 将指定的 JSON 字符串转换为 T 类型的对象。
        /// </summary>
        /// <typeparam name="T">所生成对象的类型。</typeparam>
        /// <param name="input">要进行反序列化的 JSON 字符串。</param>
        /// <returns>反序列化的对象。</returns>
        public static T Deserialize<T>(string input)
        {
            try
            {
                //return Serializer.Deserialize<T>(input);
                return JsonConvert.DeserializeObject<T>(input);
            }
            catch (ArgumentNullException ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw ex;
            }
            catch (ArgumentException ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 将对象转换为 JSON 字符串。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns> 序列化的 JSON 字符串。</returns>
        public static string Serialize(object obj)
        {
            try
            {
                return Serializer.Serialize(obj);
                //return JsonConvert.SerializeObject(obj, Formatting.None);
            }
            catch (InvalidOperationException ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw ex;
            }
            catch (ArgumentException ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="formatting">None = 0, Indented = 1</param>
        /// <returns></returns>
        public static string Serialize(object obj, int formatting)
        {
            try
            {
                //return Serializer.Serialize(obj);
                return JsonConvert.SerializeObject(obj, (Formatting)formatting);
            }
            catch (InvalidOperationException ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw ex;
            }
            catch (ArgumentException ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 将给定对象转换为指定类型。
        /// </summary>
        /// <typeparam name="T">obj 将转换成的类型。</typeparam>
        /// <param name="obj">序列化的 JSON 字符串。</param>
        /// <returns>已转换为目标类型的对象。</returns>
        public static T ConvertToType<T>(object obj)
        {
            try
            {
                return Serializer.ConvertToType<T>(obj);
            }
            catch (InvalidOperationException ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw ex;
            }
            catch (ArgumentException ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw ex;
            }
        }


        /// <summary>
        /// Json校验
        /// </summary>
        /// <param name="schemaJson"></param>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static bool IsValid(string schemaJson, string strJson)
        {
            bool isValid = false;

            //JSchema schema = JSchema.Parse(schemaJson);
            //JObject jobj = new JObject();
            //try
            //{
            //    jobj = JsonConvert.DeserializeObject<JObject>(strJson);
            //}
            //catch (Exception ex)
            //{
            //    TextLogHelper.WriterExceptionLog(ex);
            //}

            //IList<string> messages;
            //isValid = jobj.IsValid(schema, out messages);

            //if (!isValid || messages.Count > 0)
            //{
            //    TextLogHelper.WriterExceptionLog(string.Join("；", messages));
            //}

            return isValid;
        }



        public static T Cast<T>(object obj, T type)
        {
           return (T)obj;
        }

    }
}
