using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.IDAL;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using CySoft.Frame.Attributes;
using CySoft.Model.Ts;

namespace CySoft.BLL.MemberBLL
{
    public class Tb_Hy_JfruleBLL : BaseBLL
    {
        public ITb_Hy_JfruleDAL Tb_Hy_JfruleDAL { get; set; }

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
            result.Data = DAL.QueryList<Tb_Hy_Jfrule_Query>(typeof(Tb_Hy_Jfrule), param).ToList();
            return result;
        }



        #region 新增积分规则
        /// <summary>
        /// 新增积分规则
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
            List<Model.Tb.Tb_Hy_Jfrule> list = (List<Model.Tb.Tb_Hy_Jfrule>)param["list"];
            string id_masteruser = param["id_masteruser"].ToString();
            string id_user = param["id_user"].ToString();
            string style = this.GetStyle(param["tab"].ToString());
            #endregion

            if (string.IsNullOrEmpty(style))
            {
                br.Message.Add("积分对象类型不符合要求！");
                br.Level = ErrorLevel.Warning;
                br.Success = false;
                return br;
            }

            #region 获取DB的数据
            ht.Clear();
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("style", style);
            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            var dbList = DAL.QueryList<Tb_Hy_Jfrule_Query>(typeof(Tb_Hy_Jfrule), ht).ToList();
            #endregion

            #region 删除此类型的数据
            ht.Clear();
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("style", style);
            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            DAL.Delete(typeof(Model.Tb.Tb_Hy_Jfrule), ht);
            #endregion

            int sort_id = 1;
            foreach (var item in list)
            {
                item.id_create = id_user;
                item.rq_create = DateTime.Now;
                item.sort_id = sort_id;
                item.style = style;
                item.day_b = DateTime.Parse(((DateTime)item.day_b).ToString("yyyy-MM-dd 00:00:00"));
                item.day_e = DateTime.Parse(((DateTime)item.day_e).ToString("yyyy-MM-dd 23:59:59"));
                item.flag_delete = (byte)Enums.FlagDelete.NoDelete;
                item.id_masteruser = id_masteruser;

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

                if (style == "bill")
                {
                    item.id_object = "0";
                }

                sort_id++;
            }

            #region 新增数据
            DAL.AddRange<Model.Tb.Tb_Hy_Jfrule>(list);
            #endregion

            br.Message.Add(String.Format("操作成功!!"));
            br.Success = true;
            return br;
        }
        #endregion


        #region 积分规则删除
        /// <summary>
        /// 积分规则删除
        /// lz 2016-11-10
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
        /// 积分规则删除单例模式
        /// lz 2016-11-10
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

            var gz = DAL.GetItem<Model.Tb.Tb_Hy_Jfrule>(typeof(Model.Tb.Tb_Hy_Jfrule), ht);

            if (gz == null)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("要删除的积分规则不存在,ID为:{0}", id));
                return br;
            }

            br.Success = true;
            br.Message.Add("积分规则删除成功.");
            #endregion


            #region 执行删除并返回结果

            ht.Clear();
            ht.Add("id", id);
            ht.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);
            ht.Add("style", gz.style);

            if (gz.style == "bill")
                ht.Add("keep_hasone", "1");

            var count = DAL.UpdatePart(typeof(Model.Tb.Tb_Hy_Jfrule), ht);
            if (count <= 0)
            {
                br.Success = false;
                br.Message.Clear();
                br.Message.Add(string.Format("积分规则[{0}]删除操作失败 请先确保至少保留一条数据！", gz.sort_id));
            }

            #endregion
            return br;
        }
        #endregion




        #region 会员多倍积分
        /// <summary>
        /// 会员多倍积分
        /// lz 2016-11-11
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Active(Hashtable param)
        {
            #region 获取参数
            BaseResult br = new BaseResult();
            if (param == null
                || param.Count == 0
                || !param.ContainsKey("dbjf")
                || !param.ContainsKey("id_user")
                || !param.ContainsKey("id_masteruser")
                || !param.ContainsKey("id_shop")
                )
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("多倍积分参数错误.");
                return br;
            }
            #endregion

            #region 处理
            var dbjf = (Ts_HykDbjf)param["dbjf"];
            Tb_Hy_JfruleDAL.AddWithExists(dbjf);
            #endregion

            #region 返回结果
            br.Success = true;
            br.Message.Add("操作成功.");
            #endregion

            return br;
        }


        #endregion


        #region GetStyle
        public string GetStyle(string tab)
        {
            if (tab == "xfje")
            {
                return "bill";
            }
            else if (tab == "splb")
            {
                return "spfl";
            }
            else if (tab == "sp")
            {
                return "dp";
            }
            else
                return "";
        }
        #endregion




    }
}
