using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Model.Tb;
using CySoft.Model.Flags;
using CySoft.Frame.Attributes;
using CySoft.IBLL;

namespace CySoft.BLL.BuyerBLL
{
    public class RecieverAddressBLL : BaseBLL, IRecieverAddressBLL
    {
        /// <summary>
        /// 新增
        /// znt 2015-03-24
        /// </summary>
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Tb_Cgs_Shdz model = (Tb_Cgs_Shdz)entity;

            Hashtable param = new Hashtable();
            param.Add("id_cgs", model.id_cgs);
            if (DAL.GetCount(typeof(Tb_Cgs_Shdz), param) <= 0)
            {
                model.flag_default = YesNoFlag.Yes;
            }
            DAL.Add(model);
            br.Message.Add(String.Format("新增收货地址。流水号:{0}，详细地址:{1}", model.id, model.address));
            br.Success = true;
            return br;
        }


        /// <summary>
        /// 删除 
        /// znt 2015-03-24
        /// </summary>
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            Tb_Cgs_Shdz model = DAL.GetItem<Tb_Cgs_Shdz>(typeof(Tb_Cgs_Shdz), param);
            if (model.flag_default == YesNoFlag.Yes)
            {
                br.Message.Add(String.Format("不能删除默认的收货地址。流水号{0}，详细地址:{1}", model.id, model.address));
                br.Success = false;
                return br;
            }

            DAL.Delete(typeof(Tb_Cgs_Shdz), param);

            if (model != null)
            {
                br.Message.Add(String.Format("删除收货地址成功。流水号{0}，详细地址:{1}", model.id, model.address));
                br.Success = true;
                return br;
            }
            else
            {
                br.Message.Add(String.Format("删除收货地址失败。流水号{0}，详细地址:{1}", model.id, model.address));
                br.Success = false;
                return br;
            }

        }

        /// <summary>
        /// 修改
        /// znt 2015-03-24
        /// </summary>
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            Tb_Cgs_Shdz model = (Tb_Cgs_Shdz)entity;

            param.Clear();
            param.Add("id", model.id);
            Tb_Cgs_Shdz oldModel = DAL.GetItem<Tb_Cgs_Shdz>(typeof(Tb_Cgs_Shdz), param);

            if (oldModel == null)
            {
                br.Data = "none";
                br.Success = false;
                br.Message.Add(String.Format("收货地址不存在。流水号{0}", model.id));
                return br;
            }

            param.Clear();
            param.Add("id", model.id);
            param.Add("new_shr", model.shr);
            param.Add("new_phone", model.phone);
            param.Add("new_tel", model.tel);
            param.Add("new_id_province", model.id_province);
            param.Add("new_id_city", model.id_city);
            param.Add("new_id_county", model.id_county);
            param.Add("new_zipcode", model.zipcode);
            param.Add("new_address", model.address);
            param.Add("new_rq_edit", model.rq_edit);
            param.Add("new_id_edit", model.id_edit);

            int result = DAL.UpdatePart(typeof(Tb_Cgs_Shdz), param);


            if (result < 0)
            {
                br.Success = true;
                br.Message.Add(String.Format("修改收货地址成功。流水号{0}，详细地址:{1}", model.id, model.address));
                return br;
            }

            else
            {
                br.Success = false;
                br.Message.Add(String.Format("修改收货地址失败。流水号{0}，详细地址:{1}", model.id, model.address));
                return br;
            }

        }

        /// <summary>
        /// 获取 单个
        /// znt 2015-03-25
        /// </summary>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.GetItem<Tb_Cgs_Shdz_Query>(typeof(Tb_Cgs_Shdz), param);
            if (br.Data == null)
            {
                br.Success = false;
                br.Message.Add("未找到该地址信息。");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            br.Success = true;
            return br;
        }

        /// <summary>
        ///  不分页获取
        /// znt 2015-03-24
        /// </summary>
        /// <returns></returns>
        public override BaseResult GetAll(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Tb_Cgs_Shdz_Query>(typeof(Tb_Cgs_Shdz), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 分页
        /// znt 2015-03-24
        /// </summary>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.QueryCount(typeof(Tb_Cgs_Shdz), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_Cgs_Shdz_Query>(typeof(Tb_Cgs_Shdz), param);
            }
            else
            {
                pn.Data = new List<Tb_Cgs_Shdz_Query>();
            }
            pn.Success = true;
            return pn;
        }

        /// <summary>
        /// 设置默认地址
        /// znt 2015-03-26
        /// </summary>
        [Transaction]
        public BaseResult SettingDefault(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            Tb_Cgs_Shdz model = (Tb_Cgs_Shdz)entity;
            int result = 1;
            param.Add("id_cgs", model.id_cgs);
            param.Add("flag_default", (int)YesNoFlag.Yes);
            Tb_Cgs_Shdz old_default_model = DAL.QueryList<Tb_Cgs_Shdz>(typeof(Tb_Cgs_Shdz), param).FirstOrDefault();
            if (old_default_model != null)
            {
                param.Clear();
                param.Add("id", old_default_model.id);
                param.Add("new_flag_default", (int)YesNoFlag.No);
                result = DAL.UpdatePart(typeof(Tb_Cgs_Shdz), param);
            }

            param.Clear();
            param.Add("id", model.id);
            param.Add("new_flag_default", (int)YesNoFlag.Yes);
            result= DAL.UpdatePart(typeof(Tb_Cgs_Shdz), param);

            if (result < 0)
            {
                br.Success = true;
                br.Message.Add(String.Format("修改默认地址成功。流水号{0}，详细地址:{1}", model.id, model.address));
                return br;
            }

            else
            {
                br.Success = false;
                br.Message.Add(String.Format("修改默认地址失败。流水号{0}，详细地址:{1}", model.id, model.address));
                return br;
            }

        }
    }
}
