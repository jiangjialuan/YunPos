using System.Collections.Generic;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using System.Collections;
using CySoft.Frame.Attributes;
using System.Linq;
using System.IO;
using CySoft.Model.Enums;

namespace CySoft.BLL.InfoBLL
{
    /// <summary>
    /// 通知分类BLL
    /// </summary>
    public class InfoTypeBLL : BaseBLL
    {
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(Hashtable param)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.GetCount(typeof(Info_Type), param);
            if (pn.TotalCount > 0)
            {
                IList<Info_Type> lst = DAL.QueryPage<Info_Type>(typeof(Info_Type), param);
                pn.Data = lst;
            }
            return pn;
        }

        /// <summary>
        /// 获取所有通知分类
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Info_Type>(typeof(Info_Type), param) ?? new List<Info_Type>();
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 添加通知分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Info_Type model = (Info_Type)entity;
            Hashtable param = new Hashtable();
            param.Add("mc", model.mc);
            param.Add("id_master", model.id_master);
            if (string.IsNullOrEmpty(model.id.ToString()))
            {
                br.Data = "id";
                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add(string.Format("添加失败！"));
                return br;
            }
            //判断是否已存在相同名称的类型
            else if (DAL.GetCount(typeof(Info_Type), param) > 0)
            {
                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add(string.Format("已存在相同的名称的类型，添加失败！"));
                return br;
            }

            DAL.Add(model);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 更新通知分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            if (DAL.UpdatePart(typeof(Info_Type), param) < 0)
            {
                br.Success = true;
                br.Message.Add(string.Format("修改通知分类信息成功！"));
            }
            else
            {
                br.Success = false;
                br.Message.Add(string.Format("修改通知分类信息失败！"));
            }
            return br;
        }

        /// <summary>
        /// 删除通知分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long typeId = param.ContainsKey("typeId") ? long.Parse(param["typeId"].ToString()) : 0;
            long id = param.ContainsKey("id") ? long.Parse(param["id"].ToString()) : 0;//删除类型
            param.Clear();
            //判断是否将被删除的通知分类的公告内容转移到其他分类
            if (typeId != 0)
            {
                param.Add("new_id_info_type", typeId);
                param["id_info_type"] = id;
                if (DAL.UpdatePart(typeof(Info), param) < 0)
                {
                    param.Clear();
                    param.Add("id", id);
                    br = DeleteInfo(param);
                }
            }
            else
            {
                param.Add("id_info_type", id);
                //获取被删除的类型的公告信息集合
                IList<Info> lst = DAL.QueryList<Info>(typeof(Info), param) ?? new List<Info>();
                var deleteList = (from m in lst select m.id).ToList();
                //删除发送对象
                param.Add("id_infoList", deleteList);
                DAL.Delete(typeof(Info_User), param);

                //删除公告内容
                DAL.Delete(typeof(Info), param);
                param.Clear();
                param.Add("id", id);
                //删除公告类型
                br = DeleteInfo(param);
                try
                {
                    FileInfo file;
                    //删除附件
                    foreach (Info item in lst)
                    {
                        if (!string.IsNullOrEmpty(item.filename))
                        {
                            string hostUrl = System.Web.HttpContext.Current.Server.MapPath("~/");
                            file = new FileInfo(hostUrl + item.filename);
                            if (file.Exists)
                            {
                                file.Delete();
                            }
                        }
                    }
                }
                catch (System.Exception)
                {
                    br.Success = false;
                    br.Message.Add("删除附件内容失败！");
                }
            }

            return br;
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private BaseResult DeleteInfo(Hashtable param)
        {
            BaseResult br = new BaseResult();
            if (DAL.Delete(typeof(Info_Type), param) < 0)
            {
                br.Success = true;
                br.Message.Add(string.Format("删除通知分类信息成功！"));
            }
            else
            {
                br.Success = false;
                br.Message.Add(string.Format("删除通知分类信息失败！"));
            }
            return br;
        }

        /// <summary>
        /// 置顶
        /// 置顶思路：将置顶类型的序号改为1，其他的序号统一加1
        /// 2015-6-29 wzp
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Active(Hashtable param)
        {
            BaseResult br = new BaseResult();
            if (DAL.UpdatePart(typeof(Info_Type), param) == -1)
            {
                //其他的序号统一加1
                param.Add("new_xh_next", "");
                param.Add("not_id", param["id"].ToString());
                param.Remove("id");
                param.Remove("new_xh");
                if (DAL.UpdatePart(typeof(Info_Type), param) == -1)
                {
                    br.Success = true;
                    br.Message.Add(string.Format("置顶成功！"));
                }
            }
            else
            {
                br.Success = false;
                br.Message.Add(string.Format("置顶失败！"));
            }
            return br;
        }
        public override int isAdmin(Hashtable param = null)
        {
            return DAL.GetCount(typeof(Tb_User_Role), param);
        }
        public override int GetByID(Hashtable param)
        {
            Info_Type item = DAL.GetItem<Info_Type>(typeof(Info_Type), param);
            int id = 0;
            if (item != null)
            {
                id = (int)item.id;
            }
            return id;
        }
    }
}
