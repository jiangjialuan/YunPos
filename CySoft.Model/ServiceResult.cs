using System;

namespace CySoft.Model
{ /// <summary>
    /// 云服务返回客户端信息
    /// </summary>
    [Serializable]
    public class ServiceResult
    {
        private string _state = string.Empty;
        private string _message = string.Empty;
        private string _number = string.Empty;
        private object _data = null;
        /// <summary>
        /// 状态：Done,Fail,Err
        /// </summary>
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }
        /// <summary>
        /// 信息描述或其他用途

        /// </summary>
        public string Message
        {
            set { _message = value; }
            get { return _message; }
        }
        /// <summary>
        /// 错误代码或其他用途
        /// </summary>
        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }
        /// <summary>
        /// 数据
        /// </summary>
        public object Data
        {
            //get { return _data==null?string.Empty:_data; }
            get { return _data; }
            set { _data = value; }
            
        }
    }
}

