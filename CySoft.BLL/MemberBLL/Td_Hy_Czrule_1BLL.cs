using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.IDAL;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using CySoft.Model.Td;
using CySoft.Model.Ts;
using CySoft.Model.Tz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.BLL.MemberBLL
{
    public class Td_Hy_Czrule_1BLL : BaseBLL
    {
        public ITd_Hy_Czrule_1DAL Td_Hy_Czrule_1DAL { get; set; }

        #region 会员充值金额设置
        /// <summary>
        /// 会员充值金额设置
        /// lz 2016-11-14
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
                || !param.ContainsKey("je_obj")
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
            var je_obj = (Ts_HykDbjf)param["je_obj"];
            Td_Hy_Czrule_1DAL.AddWithExists(je_obj);
            #endregion

            #region 返回结果
            br.Success = true;
            br.Message.Add("操作成功.");
            #endregion

            return br;
        }


        #endregion

        #region 新增充值规则
        /// <summary>
        /// 新增充值规则
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
            List<Td_Hy_Czrule_1_ReqModel> list = (List<Td_Hy_Czrule_1_ReqModel>)param["list"];
            string id_masteruser = param["id_masteruser"].ToString();
            string id_user = param["id_user"].ToString();
            string id_shop= param["id_shop"].ToString();
            #endregion

            List<Td_Hy_Czrule_1> addList1 = new List<Td_Hy_Czrule_1>();
            List<Td_Hy_Czrule_2> addList2 = new List<Td_Hy_Czrule_2>();


            foreach (var item in list)
            {
                Td_Hy_Czrule_1 model = new Td_Hy_Czrule_1();
                model.id_masteruser = id_masteruser;
                model.id = GetGuid;
                model.id_shop = id_shop;
                model.id_shop_cz = item.id_shop_cz;
                model.flag_cancel = (byte)Enums.FlagCancel.NoCancel;
                model.day_b = item.day_b;
                model.day_e = item.day_e;
                model.id_hyfl = item.id_hyfl;
                model.je_cz = item.je_cz;
                model.je_cz_zs = item.je_cz_zs;
                model.flag_delete = (byte)Enums.FlagDelete.NoDelete;
                model.bz = item.bz;
                model.id_create = id_user;
                model.rq_create = DateTime.Now;
                model.id_rule = "";
                addList1.Add(model);

                int sort_id = 1;
                foreach (var item2 in item.czrule_2_list)
                {
                    item2.id_masteruser = id_masteruser;
                    item2.id= GetGuid;
                    item2.id_bill = model.id;
                    item2.sort_id = sort_id;
                    item2.rq_create = model.rq_create;
                    addList2.Add(item2);
                    sort_id++;

                }
            }

            #region 新增数据
            DAL.AddRange<Td_Hy_Czrule_1>(addList1);
            DAL.AddRange<Td_Hy_Czrule_2>(addList2);
            #endregion


            #region 执行存储过程并返回结果
            foreach (var item in addList1)
            {
                ht.Clear();
                ht.Add("proname", "p_czrule_sh");
                ht.Add("errorid", "-1");
                ht.Add("errormessage", "未知错误！");
                ht.Add("id_bill", item.id);
                ht.Add("id_user", id_user);
                DAL.RunProcedure(ht);

                if (!ht.ContainsKey("errorid") || !ht.ContainsKey("errormessage"))
                {
                    br.Success = false;
                    br.Message.Add("过账失败,执行审核出现异常!");
                    throw new CySoftException(br);
                }

                if (!string.IsNullOrEmpty(ht["errorid"].ToString()) || !string.IsNullOrEmpty(ht["errormessage"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add(ht["errormessage"].ToString());
                    throw new CySoftException(br);
                }
            }
            #endregion


            br.Message.Add(String.Format("操作成功!!"));
            br.Success = true;
            return br;
        }
        #endregion

        #region 更新充值规则
        /// <summary>
        /// 更新充值规则
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            #region 获取数据
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            List<Td_Hy_Czrule_1_ReqModel> list = (List<Td_Hy_Czrule_1_ReqModel>)param["list"];
            string id_masteruser = param["id_masteruser"].ToString();
            string id_user = param["id_user"].ToString();
            string id_shop = param["id_shop"].ToString();
            #endregion

            #region 构建Model
            if (list.Count() != 1 || string.IsNullOrEmpty(list.FirstOrDefault().id))
            {
                br.Success = false;
                br.Message.Add("参数不符合要求!");
                throw new CySoftException(br);
            }

            var newModel = list.FirstOrDefault();

            List<Td_Hy_Czrule_1> tdAddList1 = new List<Td_Hy_Czrule_1>();
            List<Td_Hy_Czrule_2> tdAddList2 = new List<Td_Hy_Czrule_2>();
            List<Tb_Hy_Czrule_Zssp> tbAddList2 = new List<Tb_Hy_Czrule_Zssp>();


            ht.Clear();
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("id", list.FirstOrDefault().id);
            var head = DAL.GetItem<Tb_Hy_Czrule_Query>(typeof(Tb_Hy_Czrule), ht);
            if (head == null || string.IsNullOrEmpty(head.id))
            {
                br.Success = false;
                br.Message.Add("未找到此规则信息!");
                throw new CySoftException(br);
            }

            foreach (var item in list)
            {
                Td_Hy_Czrule_1 model = new Td_Hy_Czrule_1();
                model.id_masteruser = id_masteruser;
                model.id = GetGuid;
                model.id_shop = head.id_shop;
                model.id_shop_cz = item.id_shop_cz;
                model.flag_cancel = (byte)Enums.FlagCancel.NoCancel;
                model.day_b = item.day_b;
                model.day_e = item.day_e;
                model.id_hyfl = item.id_hyfl;
                model.je_cz = item.je_cz;
                model.je_cz_zs = item.je_cz_zs;
                model.flag_delete = (byte)Enums.FlagDelete.NoDelete;
                model.bz = item.bz;
                model.id_create = id_user;
                model.rq_create = DateTime.Now;
                model.id_rule = head.id;
                tdAddList1.Add(model);

                int sort_id = 1;
                foreach (var item2 in item.czrule_2_list)
                {
                    item2.id_masteruser = id_masteruser;
                    item2.id = GetGuid;
                    item2.id_bill = model.id;
                    item2.sort_id = sort_id;
                    item2.rq_create = model.rq_create;
                    tdAddList2.Add(item2);
                    sort_id++;
                }
            }

            foreach (var item in tdAddList2)
            {
                Tb_Hy_Czrule_Zssp zsspModel = new Tb_Hy_Czrule_Zssp();
                zsspModel.id_masteruser = head.id_masteruser;
                zsspModel.id = GetGuid;
                zsspModel.id_bill = head.id_bill;
                zsspModel.id_sp = item.id_sp;
                zsspModel.sl = item.sl;
                zsspModel.id_create = head.id_create;
                zsspModel.rq_create = head.rq_create;
                zsspModel.flag_delete = head.flag_delete;
                tbAddList2.Add(zsspModel);
            } 
            #endregion

            #region 操作数据库

            ht.Clear();
            ht.Add("id", head.id);
            ht.Add("new_id_shop", newModel.id_shop_cz);
            ht.Add("new_day_b", newModel.day_b);
            ht.Add("new_day_e", newModel.day_e);
            ht.Add("new_id_hyfl", newModel.id_hyfl);
            ht.Add("new_je_cz", newModel.je_cz);
            ht.Add("new_je_cz_zs", newModel.je_cz_zs);
            DAL.UpdatePart(typeof(Tb_Hy_Czrule), ht);

            ht.Clear();
            ht.Add("id_bill", head.id_bill);
            ht.Add("id_masteruser", id_masteruser);
            DAL.Delete(typeof(Tb_Hy_Czrule_Zssp), ht);
            DAL.AddRange<Tb_Hy_Czrule_Zssp>(tbAddList2);

            DAL.AddRange<Td_Hy_Czrule_1>(tdAddList1);
            DAL.AddRange<Td_Hy_Czrule_2>(tdAddList2);

            #endregion

            br.Message.Add(String.Format("操作成功!!"));
            br.Success = true;
            return br;
        }
        #endregion

        #region 积分规则作废
        /// <summary>
        /// 积分规则作废
        /// lz 2016-11-16
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Stop(Hashtable param)
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
                br.Message.Add("作废参数错误.");
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

                var rs = StopOne(ht);
                if (!rs.Success) br.Message.Add(rs.Message[0]);
            }
            #endregion

            #region 返回结果

            if (ids.Length.Equals(br.Message.Count))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Insert(0, "作废不成功，因为以下原因：");
            }
            else if (br.Message.Count > 0)
            {
                br.Success = true;
                br.Level = ErrorLevel.Question;
                br.Message.Insert(0, "作废部分成功，未删除的规则存在以下原因：");
            }
            else
            {
                br.Success = true;
                br.Message.Add("作废成功.");
            }
            #endregion

            return br;
        }

        /// <summary>
        /// 积分规则作废单例模式
        /// lz 2016-11-16
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private BaseResult StopOne(Hashtable param)
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

            #region 获取信息
            string id = param["id"].ToString();
            string id_user = param["id_user"].ToString();
            string id_masteruser = param["id_masteruser"].ToString();

            Hashtable ht = new Hashtable();

            ht.Clear();
            ht.Add("id", id);
            ht.Add("id_masteruser", id_masteruser);
            var czrule_d = DAL.GetItem<Td_Hy_Czrule_1>(typeof(Td_Hy_Czrule_1), ht);

            if (czrule_d == null)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("要作废的规则D不存在!"));
                return br;
            }


            ht.Clear();
            ht.Add("id_bill", id);
            ht.Add("id_masteruser", id_masteruser);

            var czrule = DAL.GetItem<Tb_Hy_Czrule>(typeof(Tb_Hy_Czrule), ht);

            if (czrule == null)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("要作废的规则不存在!"));
                return br;
            }

            br.Success = true;
            br.Message.Add("规则作废成功.");
            #endregion


            #region 执行作废并返回结果
            ht.Clear();
            ht.Add("id", id);
            ht.Add("new_flag_cancel", (int)Enums.FlagCancel.Canceled);
            DAL.UpdatePart(typeof(Td_Hy_Czrule_1), ht);

            ht.Clear();
            ht.Add("id_bill", id);
            ht.Add("new_flag_stop", (byte)Enums.FlagStop.Stopped);
            DAL.UpdatePart(typeof(Tb_Hy_Czrule), ht);

            #endregion
            return br;
        }
        #endregion

        #region GetAll

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
            
            var count = DAL.UpdatePart(typeof(Model.Tb.Tb_Hy_Jfrule), ht);
            if (count <= 0)
            {
                br.Success = false;
                br.Message.Add(string.Format("积分规则[{0}]删除操作失败！", gz.sort_id));
            }

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
