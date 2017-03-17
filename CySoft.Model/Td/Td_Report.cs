using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Td
{
    [Serializable]
    public class Td_Report
    {
        private int _ordercount = 0;
        private int _customercount = 0;
        private decimal _money = 0m;
        private string _time = string.Empty;
        private int _sale_count = 0;

        ////<summary>
        ////订单数
        ////</summary>
        public int ordercount
        {
            get
            {
                return _ordercount;
            }
            set
            {
                _ordercount = value;
            }
        }

        /// <summary>
        /// 客户数
        /// </summary>
        public int customercount
        {
            get
            {
                return _customercount;
            }
            set
            {
                _customercount = value;
            }
        }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal money
        {
            get
            {
                return _money;
            }
            set
            {
                _money = value;
            }
        }

        ///<summary>
        ///时间
        ///</summary>
        public string time
        {
            get
            {
                return _time;
            }
            set
            {

                _time = value;

            }
        }

        /// <summary>
        /// 销售数量
        /// </summary>
        public int sale_count {
            get { 
                return _sale_count; 
            }
            set {
                _sale_count = value;
            }
        }
    }
}
