using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using CySoft.Model.Tz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.BLL.MemberBLL
{
    public class Tb_Hy_JfconvertspBLL : BaseBLL
    {

        #region IBLL
        public IBaseBLL Tz_Hy_JfBLL { get; set; }
        #endregion

        #region 获取积分兑换商品
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult result = new BaseResult() { Success = true };
            if (param == null)
            {
                result.Success = false;
                result.Level = ErrorLevel.Warning;
                result.Message.Add("参数有误!");
                return result;
            }
            param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            result.Data = DAL.QueryList<Tb_Hy_Jfconvertsp_Query>(typeof(Tb_Hy_Jfconvertsp), param).ToList();
            return result;
        }
        #endregion

        #region 积分兑换商品删除
        /// <summary>
        /// 积分兑换商品删除
        /// lz 2016-11-21
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            #region 获取参数
            BaseResult br = new BaseResult();
            if (param == null
                || param.Count == 0
                || !param.ContainsKey("ids")
                || !param.ContainsKey("id_user")
                || !param.ContainsKey("id_masteruser")
                )
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("删除参数错误.");
                return br;
            }
            #endregion

            #region 遍历处理
            string[] ids = param["ids"].ToString().Split(',');

            Hashtable ht = new Hashtable();
            foreach (var p in ids)
            {
                ht.Clear();
                ht.Add("id", p);
                ht.Add("id_user", param["id_user"]);
                ht.Add("id_masteruser", param["id_masteruser"]);

                var rs = DeleteOne(ht);
                if (!rs.Success) br.Message.Add(rs.Message[0]);
            }
            #endregion

            #region 返回结果

            if (ids.Length.Equals(br.Message.Count))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Insert(0, "删除不成功，因为以下原因：");
            }
            else if (br.Message.Count > 0)
            {
                br.Success = true;
                br.Level = ErrorLevel.Question;
                br.Message.Insert(0, "删除部分成功，未删除的规则存在以下原因：");
            }
            else
            {
                br.Success = true;
                br.Message.Add("删除成功.");
            }
            #endregion

            return br;
        }

        /// <summary>
        /// 积分兑换商品删除单例模式
        /// lz 2016-11-21
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private BaseResult DeleteOne(Hashtable param)
        {
            #region 验证参数
            BaseResult br = new BaseResult();
            if (param == null
                || param.Count == 0
                || !param.ContainsKey("id")
                || string.IsNullOrWhiteSpace(param["id"].ToString())
                 || !param.ContainsKey("id_user")
                || !param.ContainsKey("id_masteruser")

                )
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("删除参数错误.");
                return br;
            }
            #endregion

            #region 获取商品
            string id = param["id"].ToString();
            string id_user = param["id_user"].ToString();
            string id_masteruser = param["id_masteruser"].ToString();

            Hashtable ht = new Hashtable();
            ht.Add("id", id);
            ht.Add("id_masteruser", id_masteruser);

            var gz = DAL.GetItem<Model.Tb.Tb_Hy_Jfconvertsp>(typeof(Model.Tb.Tb_Hy_Jfconvertsp), ht);

            if (gz == null)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("要删除的数据不存在,ID为:{0}", id));
                return br;
            }
            br.Success = true;
            br.Message.Add("删除成功.");
            #endregion

            #region 执行删除并返回结果

            ht.Clear();
            ht.Add("id", id);
            ht.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);

            var count = DAL.UpdatePart(typeof(Model.Tb.Tb_Hy_Jfconvertsp), ht);
            if (count <= 0)
            {
                br.Success = false;
                br.Message.Add(string.Format("数据 id [{0}]删除操作失败！", gz.sort_id));
            }

            #endregion
            return br;
        }
        #endregion

        #region 新增积分兑换商品
        /// <summary>
        /// 新增积分兑换商品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            #region 获取数据
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            List<Model.Tb.Tb_Hy_Jfconvertsp> list = (List<Model.Tb.Tb_Hy_Jfconvertsp>)param["list"];
            string id_masteruser = param["id_masteruser"].ToString();
            string id_user = param["id_user"].ToString();
            #endregion

            #region 获取DB的数据
            ht.Clear();
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            var dbList = DAL.QueryList<Tb_Hy_Jfconvertsp_Query>(typeof(Tb_Hy_Jfconvertsp), ht).ToList();
            #endregion


            #region 删除此类型的数据
            ht.Clear();
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            DAL.Delete(typeof(Model.Tb.Tb_Hy_Jfconvertsp), ht);
            #endregion

            int sort_id = 1;
            foreach (var item in list)
            {
                item.id_masteruser = id_masteruser;
                item.sort_id = sort_id;
                item.day_b = DateTime.Parse(((DateTime)item.day_b).ToString("yyyy-MM-dd 00:00:00"));
                item.day_e = DateTime.Parse(((DateTime)item.day_e).ToString("yyyy-MM-dd 23:59:59"));
                item.flag_delete = (byte)Enums.FlagDelete.NoDelete;
                item.id_create = id_user;
                item.rq_create = DateTime.Now;

                if (!string.IsNullOrEmpty(item.id) && dbList.Where(d => d.id == item.id).Count() > 0)
                {
                    var oldModel = dbList.Where(d => d.id == item.id).FirstOrDefault();
                    if (oldModel != null)
                    {
                        item.id_create = oldModel.id_create;
                        item.rq_create = oldModel.rq_create;
                    }
                }
                else
                {
                    item.id = GetGuid;
                }

                sort_id++;
            }

            #region 新增数据
            DAL.AddRange<Model.Tb.Tb_Hy_Jfconvertsp>(list);
            #endregion

            br.Message.Add(String.Format("操作成功!!"));
            br.Success = true;
            return br;
        }
        #endregion

        #region 积分兑换商品查询 API
        /// <summary>
        /// 积分兑换商品查询
        /// lz
        /// 2016-12-15
        /// </summary>
        public override BaseResult Active(Hashtable entity)
        {
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult() { Success = true };

            #region 检查会员状态
            br = base.CheckHY(param);
            if (!br.Success)
            {
                br.Success = false;
                br.Data = null;
                return br;
            }
            Tb_Hy_Detail hy_detail = (Tb_Hy_Detail)br.Data;
            #endregion

            #region 检查是否符合
            if (hy_detail == null || hy_detail.Tb_Hy_Shop == null || string.IsNullOrEmpty(hy_detail.Tb_Hy_Shop.id_hyfl))
            {
                br.Success = false;
                br.Data = null;
                br.Message.Clear();
                br.Message.Add(String.Format("未查询到会员相关分类信息!"));
                return br;
            }

            hy_detail.Tb_Hy_Shop.id_shop = param["id_shop"].ToString();

            #endregion

            #region 获取会员积分
            Hashtable ht = new Hashtable();
            ht.Add("id_hy", hy_detail.Tb_Hy_Shop.id_hy);
            ht.Add("id_shop", hy_detail.Tb_Hy_Shop.id_shop);
            ht.Add("id_masteruser", hy_detail.Tb_Hy_Shop.id_masteruser);
            var brHYJF = Tz_Hy_JfBLL.Get(ht);
            if (!brHYJF.Success)
            {
                br.Success = false;
                br.Data = null;
                br.Message.Clear();
                br.Message.Add(brHYJF.Message.FirstOrDefault());
                return br;
            }
            var dhHYJF = (Tz_Hy_Jf)brHYJF.Data ?? new Tz_Hy_Jf() { jf_qm = 0 };

            #endregion

            #region 获取相关信息
            //由于机构和会员类别都有所有值,有四种情况按下列顺序查找
            //1、指定机构和指定会员类别
            //2、所有机构和指定会员类别
            //3、指定机构和所有会员类别
            //4、所有机构和所有会员类别
            #region 注释
            //var list = new List<Tb_Hy_Jfconvertsp_Query>();
            //ht.Clear();
            //ht.Add("id_masteruser", hy_detail.Tb_Hy_Shop.id_masteruser);
            //ht.Add("id_shop", hy_detail.Tb_Hy_Shop.id_shop);
            //ht.Add("id_hyfl", hy_detail.Tb_Hy_Shop.id_hyfl);
            //ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            //list = DAL.QueryList<Tb_Hy_Jfconvertsp_Query>(typeof(Tb_Hy_Jfconvertsp), ht).ToList();
            //if (list == null || list.Count() <= 0)
            //{
            //    ht.Clear();
            //    ht.Add("id_masteruser", hy_detail.Tb_Hy_Shop.id_masteruser);
            //    ht.Add("id_shop", "0");
            //    ht.Add("id_hyfl", hy_detail.Tb_Hy_Shop.id_hyfl);
            //    ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            //    list = DAL.QueryList<Tb_Hy_Jfconvertsp_Query>(typeof(Tb_Hy_Jfconvertsp), ht).ToList();
            //}
            //if (list == null || list.Count() <= 0)
            //{
            //    ht.Clear();
            //    ht.Add("id_masteruser", hy_detail.Tb_Hy_Shop.id_masteruser);
            //    ht.Add("id_shop", hy_detail.Tb_Hy_Shop.id_shop);
            //    ht.Add("id_hyfl", "0");
            //    ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            //    list = DAL.QueryList<Tb_Hy_Jfconvertsp_Query>(typeof(Tb_Hy_Jfconvertsp), ht).ToList();
            //}
            //if (list == null || list.Count() <= 0)
            //{
            //    ht.Clear();
            //    ht.Add("id_masteruser", hy_detail.Tb_Hy_Shop.id_masteruser);
            //    ht.Add("id_shop", "0");
            //    ht.Add("id_hyfl", "0");
            //    ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            //    list = DAL.QueryList<Tb_Hy_Jfconvertsp_Query>(typeof(Tb_Hy_Jfconvertsp), ht).ToList();
            //} 
            #endregion

            var list = new List<Tb_Hy_Jfconvertsp_Query>();
            var wList = new List<Tb_Hy_Jfconvertsp_Query>();
            ht.Clear();
            ht.Add("id_masteruser", hy_detail.Tb_Hy_Shop.id_masteruser);
            ht.Add("id_shop", hy_detail.Tb_Hy_Shop.id_shop);
            ht.Add("id_hyfl", hy_detail.Tb_Hy_Shop.id_hyfl);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            list = DAL.QueryList<Tb_Hy_Jfconvertsp_Query>(typeof(Tb_Hy_Jfconvertsp), ht).ToList();
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list.Where(d => !(from r in wList select r.id_sp).ToArray().Contains(d.id_sp)))
                {
                    wList.Add(item);
                }
            }

            ht.Clear();
            ht.Add("id_masteruser", hy_detail.Tb_Hy_Shop.id_masteruser);
            ht.Add("id_shop", "0");
            ht.Add("id_hyfl", hy_detail.Tb_Hy_Shop.id_hyfl);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            list = DAL.QueryList<Tb_Hy_Jfconvertsp_Query>(typeof(Tb_Hy_Jfconvertsp), ht).ToList();

            if (list != null && list.Count() > 0)
            {
                foreach (var item in list.Where(d => !(from r in wList select r.id_sp).ToArray().Contains(d.id_sp)))
                {
                    wList.Add(item);
                }
            }

            ht.Clear();
            ht.Add("id_masteruser", hy_detail.Tb_Hy_Shop.id_masteruser);
            ht.Add("id_shop", hy_detail.Tb_Hy_Shop.id_shop);
            ht.Add("id_hyfl", "0");
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            list = DAL.QueryList<Tb_Hy_Jfconvertsp_Query>(typeof(Tb_Hy_Jfconvertsp), ht).ToList();

            if (list != null && list.Count() > 0)
            {
                foreach (var item in list.Where(d => !(from r in wList select r.id_sp).ToArray().Contains(d.id_sp)))
                {
                    wList.Add(item);
                }
            }

            ht.Clear();
            ht.Add("id_masteruser", hy_detail.Tb_Hy_Shop.id_masteruser);
            ht.Add("id_shop", "0");
            ht.Add("id_hyfl", "0");
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            list = DAL.QueryList<Tb_Hy_Jfconvertsp_Query>(typeof(Tb_Hy_Jfconvertsp), ht).ToList();

            if (list != null && list.Count() > 0)
            {
                foreach (var item in list.Where(d => !(from r in wList select r.id_sp).ToArray().Contains(d.id_sp)))
                {
                    wList.Add(item);
                }
            }

            #endregion

            #region 结果处理
            if (wList == null)
                wList = new List<Tb_Hy_Jfconvertsp_Query>();

            var rList = from r in wList
                        select new
                        {
                            id_masteruser = r.id_masteruser,
                            id_shop = r.id_shop,
                            day_b = (r.day_b == null ? "" : ((DateTime)r.day_b).ToString("yyyy-MM-dd HH:mm:ss")),
                            day_e = (r.day_e == null ? "" : ((DateTime)r.day_e).ToString("yyyy-MM-dd HH:mm:ss")),
                            id_hyfl = r.id_hyfl,
                            weeks = r.weeks,
                            id_sp = r.id_sp,
                            je = r.je,
                            jf = r.jf,
                            dj_ls = r.dj_ls,
                            id_create = r.id_create,
                            rq_create = (r.rq_create == null ? "" : ((DateTime)r.rq_create).ToString("yyyy-MM-dd HH:mm:ss"))
                        };
            #endregion

            #region 返回
            br.Message.Add(String.Format("操作成功!"));
            br.Success = true;
            br.Data = new { rList = rList, jf_qm = dhHYJF.jf_qm };
            return br;
            #endregion

        }
        #endregion


    }
}
