using System;
using System.Collections;
using System.Linq;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.IDAL;
using CySoft.Model;
using CySoft.Model.Enums;
using CySoft.Model.Tb;

namespace CySoft.BLL.MemberBLL
{
    public class Tb_HyflBLL : BaseBLL
    {
        #region 获取分页数据
        /// <summary>
        /// 获取分页数据
        /// lz
        /// 2016-09-18
        /// </summary>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Tb_Hyfl), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_Hyfl>(typeof(Tb_Hyfl), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        } 
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// lz
        /// 2016-09-18
        /// </summary>
        public override BaseResult Add(dynamic entity)
        {
            #region 获取数据
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            var tb_Hyfl = this.TurnTb_HyflModel(param);
            #endregion


            #region 验证折扣
            if (tb_Hyfl.zk < 0 || tb_Hyfl.zk > 1)
            {
                br.Success = false;
                br.Message.Add("折扣必须在 0 和 1 之间!");
                return br;
            } 
            #endregion

            #region 验证是否重复
            ht.Add("id_masteruser", tb_Hyfl.id_masteruser);
            ht.Add("mc", tb_Hyfl.mc);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            if (DAL.GetCount(typeof(Tb_Hyfl), ht) > 0)
            {
                br.Success = false;
                br.Message.Add("输入名称已存在!");
                return br;
            }

            //ht.Clear();
            //ht.Add("id_masteruser", tb_Hyfl.id_masteruser);
            //ht.Add("bm", tb_Hyfl.bm);
            //ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            //if (DAL.GetCount(typeof(Tb_Hyfl), ht) > 0)
            //{
            //    br.Success = false;
            //    br.Message.Add("输入编码已存在!");
            //    return br;
            //}
            #endregion

            #region 插入数据库
            DAL.Add(tb_Hyfl);
            #endregion
            #region 返回
            br.Message.Add(String.Format("新增成功。流水号：{0}", tb_Hyfl.id));
            br.Success = true;
            return br; 
            #endregion
        } 
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// lz
        /// 2016-09-18
        /// </summary>
        public override BaseResult Update(dynamic entity)
        {
            #region 获取参数
            BaseResult result = new BaseResult() { Success = true };
            Tb_Hyfl model = entity as Tb_Hyfl;
            if (model == null)
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            #endregion

            #region 验证折扣
            if (model.zk < 0 || model.zk > 1)
            {
                result.Success = false;
                result.Message.Add("折扣必须在 0 和 1 之间!");
                return result;
            }
            #endregion

            #region 验证参数
            Hashtable ht = new Hashtable();
            //ht.Add("id_masteruser", model.id_masteruser);
            //ht.Add("bm", model.bm);
            //ht.Add("not_id", model.id);
            //ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            //if (DAL.GetCount(typeof(Tb_Hyfl), ht) > 0)
            //{
            //    result.Success = false;
            //    result.Message.Add("输入编码重复!");
            //    return result;
            //}

            ht.Clear();
            ht.Add("id_masteruser", model.id_masteruser);
            ht.Add("mc", model.mc);
            ht.Add("not_id", model.id);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            if (DAL.GetCount(typeof(Tb_Hyfl), ht) > 0)
            {
                result.Success = false;
                result.Message.Add("输入名称重复!");
                return result;
            } 

            #endregion
            #region 执行更新
            ht.Clear();
            ht.Add("id", model.id);
            ht.Add("id_masteruser", model.id_masteruser);
            ht.Add("new_bm", model.bm);
            ht.Add("new_mc", model.mc);
            ht.Add("new_sort_id", model.sort_id);
            ht.Add("new_rq_edit", DateTime.Now);
            ht.Add("new_id_edit", model.id_edit);
            ht.Add("new_flag_yhlx", model.flag_yhlx);
            ht.Add("new_zk", model.zk);
            try
            {
                DAL.UpdatePart(typeof(Tb_Hyfl), ht);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message.Add("更新操作异常!");
            } 
            #endregion
            #region 返回
            return result; 
            #endregion
        } 
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// lz
        /// 2016-09-18
        /// </summary>
        public override BaseResult Delete(Hashtable param)
        {
            #region 获取参数
            BaseResult result = new BaseResult() { Success = true };
            if (param == null || param.Count < 2)
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            var id = param["id"].ToString();
            var id_masteruser = param["id_masteruser"].ToString();
            if (string.IsNullOrEmpty(id))
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            if (string.IsNullOrEmpty(id_masteruser))
            {
                result.Success = false;
                result.Message.Add("请登录!");
                return result; 
            }
            #endregion
            #region 执行删除
            Hashtable ht = new Hashtable();
            ht.Add("id_hyfl", id);
            ht.Add("hy_flag_delete", (byte)Enums.FlagDelete.NoDelete);
            ht.Add("id_masteruser", id_masteruser);
            var totalCount = DAL.GetCount(typeof(Tb_Hy_Shop), ht);
            if (totalCount > 0)
            {
                result.Success = false;
                result.Message.Add("本分类已被使用 不允许删除!");
                return result;

            }

            ht.Clear();
            ht.Add("id", id);
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);
            try
            {
                if (DAL.UpdatePart(typeof(Tb_Hyfl), ht) <= 0)
                {
                    result.Success = false;
                    result.Message.Add("删除操作失败!");
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message.Add("删除操作异常!");
            } 
            #endregion
            #region 返回
            return result; 
            #endregion
        }
        
        #endregion

        #region 获取单个数据
        /// <summary>
        /// 获取单个数据
        /// lz
        /// 2016-09-18
        /// </summary>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            try
            {
                res.Data = DAL.GetItem<Tb_Hyfl>(typeof(Tb_Hyfl), param);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message.Add("操作异常!");
            }
            return res;
        }
        
        #endregion

        #region 获取所有符合条件的数据
        /// <summary>
        /// 获取所有符合条件的数据
        /// lz
        /// 2016-09-19
        /// </summary>
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
            result.Data = DAL.QueryList<Tb_Hyfl>(typeof(Tb_Hyfl), param).ToList();
            return result;
        } 
        #endregion

        #region TurnTb_HyflModel
        /// <summary>
        /// 将Hashtable转换为Model
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private Tb_Hyfl TurnTb_HyflModel(Hashtable param)
        {
            Tb_Hyfl model = new Tb_Hyfl();
            model.id_masteruser = param["id_masteruser"].ToString();
            model.id = Guid.NewGuid().ToString();
            model.bm = param["bm"].ToString();
            model.mc = param["mc"].ToString();
            int sort_id=0;
            int.TryParse(param["sort_id"].ToString(),out sort_id);
            model.sort_id = sort_id;
            model.id_create = Guid.NewGuid().ToString();
            model.rq_create = DateTime.Now;
            model.flag_delete = (byte)Enums.FlagDelete.NoDelete;

            byte flag_yhlx = 0;
            byte.TryParse(param["flag_yhlx"].ToString(), out flag_yhlx);
            model.flag_yhlx = flag_yhlx;

            decimal zk = 0;
            decimal.TryParse(param["zk"].ToString(), out zk);
            model.zk = zk;

            return model;
        }
        #endregion
    }
}
