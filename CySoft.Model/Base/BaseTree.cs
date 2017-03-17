using System;
using System.Collections.Generic;

namespace CySoft.Model.Base
{
    /// <summary>
    /// 树型机构基类
    /// </summary>
    [Serializable]
    public class BaseTree<T>
    {
        protected int _id;
        protected int _fatherId;
        protected string _path;
        protected string _text;
        protected List<T> _children;

        public BaseTree()
        {

        }

        /// <summary>
        /// 流水
        /// </summary>
        public virtual int id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 父流水
        /// </summary>
        public virtual int fatherId
        {
            get { return _fatherId; }
            set { _fatherId = value; }
        }
        /// <summary>
        /// 路径
        /// </summary>
        public virtual string path
        {
            get { return _path; }
            set { _path = value; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string text
        {
            get { return _text; }
            set { _text = value; }
        }
        /// <summary>
        /// 子节点
        /// </summary>
        public virtual List<T> children
        {
            get { return this._children; }
            set { this._children = value; }
        }
    }

    /// <summary>
    /// 树型机构基类
    /// </summary>
    [Serializable]
    public class BaseTreeGuid<T>
    {
        protected string _id;
        protected string _fatherId;
        protected string _path;
        protected string _text;
        protected List<T> _children;

        public BaseTreeGuid()
        {

        }

        /// <summary>
        /// 流水
        /// </summary>
        public virtual string id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 父流水
        /// </summary>
        public virtual string fatherId
        {
            get { return _fatherId; }
            set { _fatherId = value; }
        }
        /// <summary>
        /// 路径
        /// </summary>
        public virtual string path
        {
            get { return _path; }
            set { _path = value; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string text
        {
            get { return _text; }
            set { _text = value; }
        }
        /// <summary>
        /// 子节点
        /// </summary>
        public virtual List<T> children
        {
            get { return this._children; }
            set { this._children = value; }
        }
    }
}