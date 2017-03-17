using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Other
{
    [Serializable]
    public class SpecMul
    {
        //id_sp_expand_template
        //mc
        //val

        private long _id_sp_expand_template;
        private string _mc;
        private string[] _val;
        /// <summary>
        /// 模板规格id
        /// </summary>
        public Nullable<long> id_sp_expand_template
        {
            get { return _id_sp_expand_template; }
            set
            {
                if (value.HasValue)
                {
                    _id_sp_expand_template = value.Value;
                }
                else
                {
                    this._id_sp_expand_template = 0;
                }
            }
        }

        
        /// <summary>
        /// 规格名称
        /// </summary>
        public string mc
        {
            get
            {
                if (this._mc == null)
                {
                    return String.Empty;
                }
                return _mc;
            }
            set { _mc = value; }
        }

        /// <summary>
        /// 规格名称
        /// </summary>
        public string[] val
        {
            get
            {
                return _val;
            }
            set { _val = value; }
        }


    }
}
