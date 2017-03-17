using System;
using System.Collections;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Frame.Common;
using CySoft.Model.Flags;
using CySoft.Model.Tb;
using CySoft.Utility;
using System.Collections.Generic;
using CySoft.Frame.Attributes;
using CySoft.IBLL;
using CySoft.IDAL;
using CySoft.Model.Ts;

#region 客户
#endregion

namespace CySoft.BLL.SupplierBLL
{
    public class SupplierBLL : BaseBLL,ISupplierBLL
    {
        protected ITb_Gys_CgsDAL Tb_Gys_CgsDAL { get; set; }

        protected static readonly Type gysType = typeof(Tb_Gys);
        /// <summary>
        /// 分页查询
        /// lxt
        /// 2015-02-26
        /// </summary>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = Tb_Gys_CgsDAL.QueryCountOfBuyer(typeof(Tb_Gys_Cgs), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = Tb_Gys_CgsDAL.QueryPageOfBuyer(typeof(Tb_Gys_Cgs), param);
            }
            else
            {
                pn.Data = new List<Tb_Gys_Cgs_Query>();
            }
            pn.Success = true;
            return pn;
        }

        ///// <summary>
        ///// 添加关注
        ///// cxb
        ///// 2015-4-13
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public override BaseResult Add(dynamic entity)
        //{
        //    BaseResult br = new BaseResult();
        //    Tb_Gys_Cgs gyscgs = (Tb_Gys_Cgs)entity;
        //    DAL.Add(gyscgs);
        //    br.Success = true;
        //    br.Message.Add(String.Format("取消关注[{1}]。信息：供应商流水号:{0}, 供应商名称:{1}", gyscgs.id_gys, gyscgs.alias_gys));
        //    return br;
        //}

        /// <summary>
        /// 取消关注
        /// lxt
        /// 2015-02-28
        /// </summary>
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();

            long id = TypeConvert.ToInt64(param["id"], 0);
            long id_user_master_cgs = TypeConvert.ToInt64(param["id_user_master_cgs"], 0);
            long id_user = TypeConvert.ToInt64(param["id_user"], 0);
            string flag_from = TypeConvert.ToString(param["flag_from"], "pc");

            param.Clear();
            param.Add("id", id);
            Tb_Gys gys = DAL.GetItem<Tb_Gys>(gysType, param);
            if (gys == null)
            {
                br.Success = false;
                br.Message.Add("取消关注失败，该供应商不存在或资料已缺失！");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            param.Clear();
            param.Add("id_user_master_gys", gys.id_user_master);
            param.Add("id_user_master_cgs", id_user_master_cgs);
            Tb_Gys_Cgs gysCgs = DAL.GetItem<Tb_Gys_Cgs>(typeof(Tb_Gys_Cgs), param);
            if (gysCgs == null)
            {
                br.Success = true;
                return br;
            }
            param.Clear();
            param.Add("id_user_master_gys", gys.id_user_master);
            param.Add("id_user_master_cgs", id_user_master_cgs);
            param.Add("new_flag_stop", YesNoFlag.Yes);
            DAL.UpdatePart(typeof(Tb_Gys_Cgs), param);
            
            param.Clear();
            param.Add("id_gys", gysCgs.id_gys);
            param.Add("id_cgs", gysCgs.id_cgs);
            //成功后删除申请记录         
            DAL.Delete(typeof(Tb_Gys_Cgs_Check), param);

            //记录供采关系日志
            var Loggcgx = new Tb_Gys_Cgs_Log();
            Loggcgx.id_cgs = gysCgs.id_gys.Value;
            Loggcgx.id_gys = gysCgs.id_cgs.Value;
            Loggcgx.id_user = id_user;
            Loggcgx.flag_state = Gys_Cgs_Status.CallBack;
            Loggcgx.flag_form = flag_from;
            Loggcgx.contents = string.Format("[{0}]撤销对[{1}]的关注.", gysCgs.mc_cgs, gysCgs.mc_gys);
            DAL.Add(Loggcgx);

            br.Success = true;
            br.Message.Add(string.Format("[{0}]撤销对[{1}]的关注.", gysCgs.mc_cgs, gysCgs.mc_gys));
            return br;
        }

        /// <summary>
        /// 修改
        /// lxt
        /// 2015-02-26
        /// </summary>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            Tb_Gys_Edit model = (Tb_Gys_Edit)param["model"];
            int id_user_master_cgs = Convert.ToInt32(param["id_user_master_cgs"]);

            param.Clear();
            param.Add("not_id_user_master_gys", model.id_user_master);
            param.Add("alias_gys", model.companyname);
            param.Add("id_user_master_cgs", id_user_master_cgs);
            if (DAL.GetCount(typeof(Tb_Gys_Cgs), param) > 0)
            {
                br.Success = false;
                br.Message.Add("供应商名已被占用");
                br.Level = ErrorLevel.Warning;
                br.Data = "companyName";
                return br;
            }
            if (!model.bm_cgs_Interface.IsEmpty())
            {
                param.Clear();
                param.Add("not_id_user_master_gys", model.id_user_master);
                param.Add("bm_cgs_Interface", model.bm_cgs_Interface);
                param.Add("id_user_master_cgs", id_user_master_cgs);
                if (DAL.GetCount(typeof(Tb_Gys_Cgs), param) > 0)
                {
                    br.Success = false;
                    br.Message.Add("编码已被占用");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "bm_cgs_Interface";
                    return br;
                }
            }


            DateTime dbDateTime = DAL.GetDbDateTime();

            param.Clear();
            param.Add("id_user_master_cgs", id_user_master_cgs);
            param.Add("id_user_master_gys", model.id_user_master);
            param.Add("new_alias_gys", model.companyname);
            param.Add("new_bm_cgs_Interface", model.bm_cgs_Interface);
            param.Add("new_id_edit", model.id_edit);
            param.Add("new_rq_edit", dbDateTime);
            DAL.UpdatePart(typeof(Tb_Gys_Cgs), param);

            br.Message.Add(String.Format("修改客户。流水号：{0}，名称:{1}", model.id, model.companyname));
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 不分页查询
        /// lxt
        /// 2015-03-07
        /// </summary>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = Tb_Gys_CgsDAL.QueryListOfBuyer(typeof(Tb_Gys_Cgs), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获得单个完整对象
        /// lxt
        /// 2015-03-16
        /// </summary>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            var gys = DAL.QueryItem<Tb_Gys_Edit>(gysType, param);
            if (gys == null)
            {
                br.Success = false;
                br.Message.Add("未找到该客户信息。");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            br.Data = gys;
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获取单个供应商
        /// cxb
        /// 2015-4-10
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseResult GetGys(Hashtable param) 
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryItem<Tb_Gys_Edit>(typeof(Tb_Gys), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获取关注供应商数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetCount(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data=DAL.GetCount(typeof(Tb_Gys_Cgs), param);
            return br;
        }

        /// <summary>
        /// 获取供应商列表
        /// cxb
        /// 2015-4-9
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseResult GetGysAll(Hashtable param) {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Tb_Gys_Query>(typeof(Tb_Gys), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获取要关注的供应商列表
        /// tim
        /// 2015-5-20
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseResult GetFindGys(Hashtable param)
        {
            BaseResult br = new BaseResult();
            if (!param.ContainsKey("id_cgs"))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("确少采购商参数.");
                return br;
            }
            br.Data = Tb_Gys_CgsDAL.FindSupplier(typeof(Tb_Gys), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获取 供应商采购商关系
        /// znt 2013-03-18
        /// </summary>
        public BaseResult GetGysCgsRelation(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.GetItem(typeof(Tb_Gys_Cgs), param);
            if (br.Data == null)
            {
                br.Success = false;
                br.Message.Add("未找到该供应商采购商关系");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            br.Success = true;
            return br;
        }

        ///// <summary>
        ///// 货单单个供应商资料
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //public Tb_Gys GetGys2(Hashtable param)
        //{
        //    return DAL.GetItem<Tb_Gys>(typeof(Tb_Gys), param);
        //}

    }
}
