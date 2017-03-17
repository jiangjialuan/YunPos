using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Td;
using CySoft.Model.Flags;

namespace CySoft.Model.Other
{
    [Serializable]
    public class OrderQuery : Td_Sale_Order_Head
    {
        private string _mc_flag_state = String.Empty;
        private long _id_sp = 0;
        public List<Td_Sale_Order_Body_Query> order_body { get; set; }
        private InvoiceFlag _invoiceFlag = InvoiceFlag.None;  

        /// <summary>
        ///  订单状态名称(已提交、待审核等)
        /// </summary>
        public string mc_flag_state
        {
            get
            {
                if (_mc_flag_state == null)
                {
                    _mc_flag_state = String.Empty;
                }
                return _mc_flag_state;
            }
            set
            {

                _mc_flag_state = value;
            }
        }

        /// <summary>
        /// 商品Id
        /// </summary>
        public Nullable<long> id_sp
        {
            get
            {
                return _id_sp;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_sp = value.Value;
                }
                else
                {
                    _id_sp = 0;
                }
            }
        }

        /// <summary>
        /// 订单发票类型 默认1:不开发票
        /// </summary>
        public InvoiceFlag invoiceFlag
        {
            get
            {
                return _invoiceFlag;
            }
            set
            {
                _invoiceFlag = value;
            }
        }

    }
}
