using System;
using System.Collections;
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Utility;
using CySoft.Model.Enums;
using CySoft.Model.Ts;

namespace CySoft.BLL.SystemBLL
{
    public class Ts_NoticeBLL : BaseBLL
    {
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };

            if (param == null || param.Count <= 0)
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            res.Data = DAL.GetItem<Ts_Notice>(typeof(Ts_Notice), param);
            return res;
        }

        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Ts_Notice), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Ts_Notice_View>(typeof(Ts_Notice), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }

        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult result = new BaseResult() { Success = true };
            var model = entity as Ts_Notice;
            #region 验证参数
            if (model == null
                    || string.IsNullOrEmpty(model.id_masteruser)
                    || string.IsNullOrEmpty(model.id_create)
                    )
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }

            if (string.IsNullOrEmpty(model.title))
            {
                result.Success = false;
                result.Message.Add("标题不允许为空!");
                return result;
            }

            if (string.IsNullOrEmpty(model.content))
            {
                result.Success = false;
                result.Message.Add("内容不允许为空!");
                return result;
            }

            if (model.flag_type == null)
            {
                result.Success = false;
                result.Message.Add("类型不允许为空!");
                return result;
            }

            #endregion
            #region 执行插入
            try
            {
                model.id = Guid.NewGuid().ToString();
                model.rq_create = DateTime.Now;
                model.flag_delete = (byte)Enums.FlagDelete.NoDelete;
                DAL.Add(model);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message.Add("新增操作异常!");
            }
            #endregion
            return result;
        }

        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult result = new BaseResult() { Success = true };
            Ts_Notice model = entity as Ts_Notice;
            if (model == null
                || string.IsNullOrEmpty(model.id)
                    || string.IsNullOrEmpty(model.id_masteruser)
                    || string.IsNullOrEmpty(model.id_edit)
                )
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            Hashtable ht = new Hashtable();
            ht.Add("id", model.id);
            ht.Add("id_masteruser", model.id_masteruser);
            ht.Add("new_title", model.title);
            ht.Add("new_content", model.content);
            ht.Add("new_flag_type", model.flag_type);
            ht.Add("new_id_shop_target", model.id_shop_target);
            ht.Add("new_id_user_target", model.id_user_target);
            ht.Add("new_rq_edit", DateTime.Now);
            ht.Add("new_id_edit", model.id_edit);

            try
            {
                DAL.UpdatePart(typeof(Ts_Notice), ht);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message.Add("更新操作异常!");
            }
            return result;
        }

        public override BaseResult Delete(Hashtable param)
        {
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

            Hashtable ht = new Hashtable();
            ht.Add("id", id);
            var dbInfo = DAL.GetItem<Ts_Notice>(typeof(Ts_Notice), ht);
            if (dbInfo == null)
            {
                result.Success = false;
                result.Message.Add("未查询到此公告信息,可能已被删除,请刷新页面重试!");
                return result;
            }

            if (dbInfo.flag_delete == (byte)Enums.FlagDelete.Deleted)
            {
                result.Success = false;
                result.Message.Add("此公告信息已被删除,请刷新页面重试!");
                return result;
            }

            ht.Clear();
            ht.Add("id", id);

            if (dbInfo.flag_type != 1)
            {
                ht.Add("id_masteruser", id_masteruser);
            }
            
            ht.Add("new_flag_delete", (byte)Enums.FlagDelete.Deleted);
            try
            {
                if (DAL.UpdatePart(typeof(Ts_Notice), ht) <= 0)
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
            return result;
        }

    }
}
