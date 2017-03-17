using System;
using System.Collections.Generic;
using System.Text;
using CySoft.Model.Tb;

namespace CySoft.Model.Other
{
     [Serializable]
    public class GoodsSave 
    {

        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private long _id_edit = 0;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private long _id_gys_create = 0;

        private long _id_sku = 0;
        private long _id_gys = 0;
        private long _id = 0;
        private long _id_spfl = 0;

        public Nullable<long> id_sku
        {
            get
            {
                return _id_sku;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_sku = value.Value;
                }
                else
                {
                    _id_sku = 0;
                }
            }
        }
        public Nullable<long> id_gys
        {
            get
            {
                return _id_gys;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_gys = value.Value;
                }
                else
                {
                    _id_gys = 0;
                }
            }
        }
        public Nullable<long> id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value.HasValue)
                {
                    _id = value.Value;
                }
                else
                {
                    _id = 0;
                }
            }
        }
        public Nullable<long> id_spfl
        {
            get
            {
                return _id_spfl;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_spfl = value.Value;
                }
                else
                {
                    _id_spfl = 0;
                }
            }
        }
         public string mc { set; get; }         
         public string unit { set; get; }
         public string keywords { set; get; }
         public string sort_id { set; get; }
         public string applyunit { set; get; }
         public string applyallsku { set; get; }
         public string description { set; get; }
         public bool applyallcgs { get; set; }


         /// <summary>
         /// 创建供应商
         /// </summary>
         public Nullable<long> id_gys_create
         {
             get
             {
                 return _id_gys_create;
             }
             set
             {
                 if (value.HasValue)
                 {
                     _id_gys_create = value.Value;
                 }
                 else
                 {
                     _id_gys_create = 0;
                 }
             }
         }
         /// <summary>
         /// 创建人
         /// </summary>       
         public Nullable<long> id_create
         {
             get
             {
                 return _id_create;
             }
             set
             {
                 if (value.HasValue)
                 {
                     _id_create = value.Value;
                 }
                 else
                 {
                     _id_create = 0;
                 }
             }
         }

         /// <summary>
         /// 创建日期
         /// </summary>
         public Nullable<DateTime> rq_create
         {
             get
             {
                 return _rq_create;
             }
             set
             {
                 if (value.HasValue)
                 {
                     _rq_create = value.Value;
                 }
                 else
                 {
                     _rq_create = new DateTime(1900, 1, 1);
                 }
             }
         }

         /// <summary>
         /// 最后修改人
         /// </summary>
         public Nullable<long> id_edit
         {
             get
             {
                 return _id_edit;
             }
             set
             {
                 if (value.HasValue)
                 {
                     _id_edit = value.Value;
                 }
                 else
                 {
                     _id_edit = 0;
                 }
             }
         }

         /// <summary>
         /// 最后修改日期
         /// </summary>
         public Nullable<DateTime> rq_edit
         {
             get
             {
                 return _rq_edit;
             }
             set
             {
                 if (value.HasValue)
                 {
                     _rq_edit = value.Value;
                 }
                 else
                 {
                     _rq_edit = new DateTime(1900, 1, 1);
                 }
             }
         }

         public List<Tb_Sp_Dj> dj { set; get; }
         public List<Goods_SKU> sku { set; get; }
         public List<Tb_Sp_Expand> spec { set; get; }
         public List<Tb_Sp_Cgs> cgs { get; set; }
         public string[] pic { set; get; }

         /// <summary>
         /// 商品标签
         /// </summary>
         public List<Tb_Gys_Tag> Tb_Gys_Tag { get; set; }
    }

     public class Goods_SKU : Tb_Sp_Sku
     {
         private decimal _dj_base = 0m;
         private int _flag_up = 0;
         public string photo { set; get; }
         public string bm_Interface { get; set; }
         public Nullable<decimal> dj_base
         {
             get
             {
                 return _dj_base;
             }
             set
             {
                 if (value.HasValue)
                 {
                     _dj_base = value.Value;
                 }
                 else
                 {
                     _dj_base = 0m;
                 }
             }
         }

         public Nullable<int> flag_up
         {
             get
             {
                 return _flag_up;
             }
             set
             {
                 if (value.HasValue)
                 {
                     _flag_up = value.Value;
                 }
                 else
                 {
                     _flag_up = 0;
                 }
             }
         }
     }
}
