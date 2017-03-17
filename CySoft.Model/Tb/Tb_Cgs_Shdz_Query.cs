using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Tb
{
    [Serializable]
    public class Tb_Cgs_Shdz_Query : Tb_Cgs_Shdz
    {
        private string _province_name = String.Empty;
        private string _city_name = String.Empty;
        private string _county_name = String.Empty;

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

    }
}
