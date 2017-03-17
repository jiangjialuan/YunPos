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

namespace CySoft.BLL.GoodsBLL
{
    public class Tb_DwBLL : BaseBLL
    {

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
            result.Data = DAL.QueryList<Tb_Dw>(typeof(Tb_Dw), param).ToList();
            return result;
        }

        public override BaseResult GetCount(Hashtable param = null)
        {
            BaseResult res = new BaseResult() { Success = true };
            res.Data = DAL.GetCount(typeof(Tb_Dw), param);
            return res;
        }

        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Tb_Dw), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_DwWithUserName>(typeof(Tb_Dw), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }


        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            try
            {
                res.Data = DAL.GetItem<Tb_Dw>(typeof(Tb_Dw), param);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message.Add("操作异常!");
            }
            return res;
        }

        public override BaseResult Add(dynamic entity)
        {
            BaseResult result = new BaseResult() { Success = true };
            Tb_Dw dwModel = entity as Tb_Dw;
            if (dwModel == null)
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", dwModel.id_masteruser);
            ht.Add("dw", dwModel.dw);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            if (DAL.GetCount(typeof(Tb_Dw), ht) > 0)
            {
                result.Success = false;
                result.Message.Add("输入单位已存在!");
                return result;
            }
            try
            {
                dwModel.id = Guid.NewGuid().ToString();
                dwModel.flag_delete = (int)Enums.FlagDelete.NoDelete;
                dwModel.rq_create = dwModel.rq_edit = DateTime.Now;
                DAL.Add(dwModel);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message.Add("新增操作异常!");
            }
            return result;
        }

        public override BaseResult Update(dynamic entity)
        {
            BaseResult result = new BaseResult() { Success = true };
            Tb_Dw dwModel = entity as Tb_Dw;
            if (dwModel == null)
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", dwModel.id_masteruser);
            ht.Add("dw", dwModel.dw);
            ht.Add("not_id", dwModel.id);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            if (DAL.GetCount(typeof(Tb_Dw), ht) > 0)
            {
                result.Success = false;
                result.Message.Add("输入单位已存在!");
                return result;
            }
            ht.Clear();
            ht.Add("id", dwModel.id);
            ht.Add("id_masteruser", dwModel.id_masteruser);
            ht.Add("new_dw", dwModel.dw);
            ht.Add("new_rq_edit", DateTime.Now);
            ht.Add("new_id_edit", dwModel.id_edit);
            try
            {
                DAL.UpdatePart(typeof(Tb_Dw), ht);
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
            var dbInfo = DAL.GetItem<Tb_Dw>(typeof(Tb_Dw), ht);
            if (dbInfo == null)
            {
                result.Success = false;
                result.Message.Add("未查询到此单位,可能已被删除,请刷新页面重试!");
                return result;
            }

            ht.Clear();
            ht.Add("dw", dbInfo.dw);
            ht.Add("flag_delete", "0");
            ht.Add("id_masteruser", id_masteruser);
            if (DAL.GetCount(typeof(Tb_Shopsp), ht) > 0)
            {
                result.Success = false;
                result.Message.Add("有商品正在使用此单位,不允许删除!");
                return result;
            }


            ht.Clear();
            ht.Add("id", id);
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);
            try
            {
                if (DAL.UpdatePart(typeof(Tb_Dw), ht) <= 0)
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

        private bool CheckParam(BaseResult result, Tb_Dw dwModel, string addOrUpdate = "")
        {
            result = result ?? new BaseResult();
            if (dwModel == null)
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return false;
            }
            if (string.IsNullOrEmpty(dwModel.id_masteruser))
            {
                result.Success = false;
                result.Message.Add("请登录!");
                return false;
            }
            if (string.IsNullOrEmpty(dwModel.dw))
            {
                result.Success = false;
                result.Message.Add("请输入单位!");
                return false;
            }
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", dwModel.id_masteruser);
            ht.Add("dw", dwModel.dw);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            if (addOrUpdate == "update")
            {
                if (string.IsNullOrEmpty(dwModel.id))
                {
                    result.Success = false;
                    result.Message.Add("参数有误!");
                    return false;
                }
            }
            if (DAL.GetCount(typeof(Tb_Dw), ht) > 0)
            {
                result.Success = false;
                result.Message.Add("输入单位已存在!");
                return false;
            }
            return true;
        }
    }
}
