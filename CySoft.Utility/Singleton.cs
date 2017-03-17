
namespace CySoft.Utility
{
    /// <summary>
    /// Singleton单例泛型类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Singleton<T>
        where T : new()
    {
        private static T instance = new T();

        private static object lockHelper = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        private Singleton()
        { }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="value"></param>
        public static T GetInstance()
        {
            if (instance == null)
            {
                lock (lockHelper)
                {
                    //此处必须要判断
                    if (instance == null)
                        instance = new T();
                }
            }
            return instance;
        }

        /// <summary>
        /// 设置实例
        /// </summary>
        /// <param name="value"></param>
        public void SetInstance(T value)
        {
            instance = value;
        }
    }
}