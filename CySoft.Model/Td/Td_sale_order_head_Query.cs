#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using System.Collections.Generic;
using CySoft.Model.Flags;
using CySoft.Model.Ts;
#endregion

#region
#endregion

namespace CySoft.Model.Td
{
    /// <summary>
    /// 订单单头
    /// </summary>
    [Serializable]
    [Table("Td_sale_order_head", "Td_Sale_Order_Head")]
    [DebuggerDisplay("dh = {dh}")]
    public class Td_Sale_Order_Head_Query : Td_Sale_Order_Head
    {
       
        #region private field
        private string _customename = String.Empty;
        private string _goodsbm = String.Empty;
        private string _goodsname = String.Empty;
        private string _province_name = String.Empty;
        private string _city_name = String.Empty;
        private string _county_name = String.Empty;
        private decimal _sl_ck = 0m;
        private decimal _sl = 0m;
        private decimal _tax = 0m;
        private decimal _vat = 0m;
        private long _id_sp = 0;
        private string _mc_flag_state = String.Empty;
        private InvoiceFlag _invoiceFlag = InvoiceFlag.None;
        private long _id_user_master = 0;
        private string _flag_out_mc = String.Empty;
        private string _flag_fh_mc = String.Empty;

        public IList<Td_Sale_Order_Body_Query> order_body { get; set; }
        public IList<Ts_Sale_Order_Log_Query> order_Log { get; set; }
        public IList<Td_Sale_Order_Head_Query> children_list { get; set; }

        private string _user_name = String.Empty;
        private string _alias_cgs = String.Empty;
        private string _alias_gys = String.Empty;
        private string _level_name = String.Empty;
        private string _supplier_name = String.Empty;

       


        //未审核付款单数量
        public int count_nopay { get; set; }
        //付款状态名称
        public string mc_payment_state { get; set; }

        /// <summary>
        /// 供应商设置的采购商接口编码
        /// </summary>
        public string bm_gys_Interface { get; set; }

        #endregion

        #region public property

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string supplier_name
        {
            get
            {
                return _supplier_name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _supplier_name = value;
                }
                else
                {
                    _supplier_name = String.Empty;
                }
            }
        }

        /// <summary>
        /// 开单业务员 所属采购级别名称（普通、高级、九折客户等）
        /// </summary>
        public string level_name
        {
            get
            {
                return _level_name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _level_name = value;
                }
                else
                {
                    _level_name = String.Empty;
                }
            }
        }

        /// <summary>
        /// 开单业务员 客户别名
        /// </summary>
        public string alias_cgs
        {
            get
            {
                return _alias_cgs;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _alias_cgs = value;
                }
                else
                {
                    _alias_cgs = String.Empty;
                }
            }
        }

        /// <summary>
        ///  关注商别名
        /// </summary>
        public string alias_gys
        {
            get
            {
                return _alias_gys;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _alias_gys = value;
                }
                else
                {
                    _alias_gys = String.Empty;
                }
            }
        }

        /// <summary>
        /// 开单业务员 名称
        /// </summary>
        public string user_name
        {
            get
            {
                return _user_name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _user_name = value;
                }
                else
                {
                    _user_name = String.Empty;
                }
            }
        }


        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomeName
        {
            get
            {
                return _customename;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _customename = value;
                }
                else
                {
                    _customename = String.Empty;
                }
            }
        }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string GoodsBm
        {
            get
            {
                return _goodsbm;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _goodsbm = value;
                }
                else
                {
                    _goodsbm = String.Empty;
                }
            }
        }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName
        {
            get
            {
                return _goodsname;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _goodsname = value;
                }
                else
                {
                    _goodsname = String.Empty;
                }
            }
        }


        /// <summary>
        /// 数量
        /// </summary>
        public Nullable<decimal> Sl
        {
            get
            {
                return _sl;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl = value.Value;
                }
                else
                {
                    _sl = 0m;
                }
            }
        }

        /// <summary>
        /// 已出库数量
        /// </summary>
        public Nullable<decimal> Sl_ck
        {
            get
            {
                return _sl_ck;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_ck = value.Value;
                }
                else
                {
                    _sl_ck = 0m;
                }
            }
        }

        /// <summary>
        /// 省
        /// </summary>
        public string province_name
        {
            get
            {
                return _province_name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _province_name = value;
                }
                else
                {
                    _province_name = String.Empty;
                }
            }
        }


        /// <summary>
        /// 市
        /// </summary>
        public string city_name
        {
            get
            {
                return _city_name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _city_name = value;
                }
                else
                {
                    _city_name = String.Empty;
                }
            }
        }

        /// <summary>
        /// 区
        /// </summary>
        public string county_name
        {
            get
            {
                return _county_name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _county_name = value;
                }
                else
                {
                    _county_name = String.Empty;
                }
            }
        }

        public Nullable<decimal> tax
        {
            get
            {
                return _tax;
            }
            set
            {
                if (value.HasValue)
                {
                    _tax = value.Value;
                }
                else
                {
                    _tax = 0m;
                }
            }
        }

        public Nullable<decimal> vat
        {
            get
            {
                return _vat;
            }
            set
            {
                if (value.HasValue)
                {
                    _vat = value.Value;
                }
                else
                {
                    _vat = 0m;
                }
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
        
        /// <summary>
        /// 主用户
        /// </summary>
        public Nullable<long> id_user_master
        {
            get
            {
                return _id_user_master;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user_master = value.Value;
                }
                else
                {
                    _id_user_master = 0;
                }
            }
        }

        /// <summary>
        ///  订单整体出库状态名称
        /// </summary>
        public string flag_out_mc
        {
            get
            {
                if (_flag_out_mc == null)
                {
                    _flag_out_mc = String.Empty;
                }
                return _flag_out_mc;
            }
            set
            {

                _flag_out_mc = value;
            }
        }

        /// <summary>
        ///  订单整体发货状态名称
        /// </summary>
        public string flag_fh_mc
        {
            get
            {
                if (_flag_fh_mc == null)
                {
                    _flag_fh_mc = String.Empty;
                }
                return _flag_fh_mc;
            }
            set
            {

                _flag_fh_mc = value;
            }
        }

        /// <summary>
        /// 供应商主用户
        /// </summary>
        public long id_user_master_gys { get; set; }
        /// <summary>
        /// 采购商主用户
        /// </summary>
        public long id_user_master_cgs { get; set; }

        #endregion

    }
}