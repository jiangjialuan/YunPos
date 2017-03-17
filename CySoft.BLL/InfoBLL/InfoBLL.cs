using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Tb;
using CySoft.Frame.Attributes;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.IDAL;
using CySoft.Utility;

namespace CySoft.BLL.InfoBLL
{
    /// <summary>
    /// 通知公告内容BLL wzp:2015-6-24
    /// </summary>
    public class InfoBLL : BaseBLL
    {
        public IInfo_UserDAL Info_UserDAL { get; set; }

        /// <summary>
        /// 添加公告内容
        /// mq 2016-05-27 修改 增加下单时通知对方
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            Info info = new Info();
            info.id =long.Parse(param["id"].ToString());
            
            if (info.id==0)
            {
                br.Data = "id";
                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add(string.Format("添加失败！标识列Id不能为空"));
                return br;
            }
            if (param.ContainsKey("cgs_send"))
            {
                Hashtable param1 = new Hashtable();
                param1.Add("id_cgs", param["cgs_send"]);
                param1.Add("id_user_master_gys", param["id_master"]);
                Tb_Gys_Cgs gys_cgs = DAL.GetItem<Tb_Gys_Cgs>(typeof(Tb_Gys_Cgs), param1);
                param["id_user_master"] = gys_cgs.id_user_cgs;
            }
            if (param.ContainsKey("gys_send"))
            {
                Hashtable param2 = new Hashtable();
                param2.Add("id_gys", param["gys_send"]);
                param2.Add("id_user_master_cgs", param["id_master"]);
                Tb_Gys_Cgs gys_cgs = DAL.GetItem<Tb_Gys_Cgs>(typeof(Tb_Gys_Cgs), param2);
                param["id_user_master"] = gys_cgs.id_user_gys;
            }
            info.Title = param["Title"].ToString();
            info.content = param["content"].ToString();
            if (param.ContainsKey("bm"))
            {
                Hashtable param3 =new Hashtable();
                param3.Add("bm", param["bm"]);
                Info_Type type = DAL.GetItem<Info_Type>(typeof(Info_Type), param3);
                param["id_info_type"] = type.id;
            }
            info.id_info_type = int.Parse(param["id_info_type"].ToString());
            info.id_create = long.Parse(param["id_create"].ToString());
            info.id_master = long.Parse(param["id_master"].ToString());
            info.flag_from =param["flag_from"].ToString();
            //上传内容处理
            if (!String.IsNullOrEmpty(param["filename"].ToString()))
            {
                info.filename = param["filename"].ToString();
                info.filename = info.filename.Replace("Temp", "Info");
                info.fileSize = param["fileSize"].ToString();
                // 复制 原图 到 Info 文件夹下
                FileExtension.Copy(System.Web.HttpContext.Current.Server.MapPath( param["filename"].ToString()), System.Web.HttpContext.Current.Server.MapPath(info.filename));
            }
            info.sl_read = 0;

            DAL.Add(info);

            param.Remove("id");
            param.Remove("Title");
            param.Remove("content");
            param.Remove("id_info_type");
            param.Remove("flag_from");
            param.Remove("filename");
            //记录发送对象(数据插入info_user)
            param["rq_new"] = DateTime.Now;
            param.Add("flag_stop", 0);
            param.Add("_from", info.flag_from);
            param.Add("infoId", info.id);
            Info_UserDAL.BatchInsert_User(typeof(Info_User), param);
            
            //获取发送数量
            //long infoId = long.Parse(param["infoId"].ToString());
            //param.Clear();
            //param.Add("id_info", infoId);
            //info.sl_send = DAL.GetCount(typeof(Info_User), param);
            
            
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 记录发送数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult  Active(Hashtable param)
        {
 	        BaseResult br = new BaseResult();
            param.Add("new_sl_send", DAL.GetCount(typeof(Info_User), param));//获取发送数量
            param.Add("id", param["id_info"]);
            param.Remove("id_info");
            br.Data= DAL.UpdatePart(typeof(Info), param);
            
            br.Success = true;
            return br;
        }


        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            IList<Info_Query> lst = new List<Info_Query>();
            if (param.ContainsKey("bm"))
            {
                Info_Type info = new Info_Type();
                string id_user = param["id_user"].ToString();
                param.Remove("id_user");
                info = DAL.GetItem<Info_Type>(typeof(Info_Type), param);
                param["id_info_type"] = info.id;
                if (param["bm"].ToString() == "business")
                {
                    param["id_user_yw"] = id_user;
                }
                pn.TotalCount = DAL.GetCount(typeof(Info), param);
                if (pn.TotalCount > 0)
                {
                    lst = DAL.QueryPage<Info_Query>(typeof(Info), param);
                }
                param.Clear();
                param["id_user"] = id_user;
                foreach (Info_Query item in lst)
                {
                    param["id_info"] = item.id;
                    param["flag_reade"] = 1;
                    item.reade= DAL.GetCount(typeof(Info_User), param);
                }
            }
            else
            {
                pn.TotalCount = DAL.GetCount(typeof(Info), param);
                if (pn.TotalCount > 0)
                {
                    lst = DAL.QueryPage<Info_Query>(typeof(Info), param);
                }
            }
            pn.Data = lst;
            return pn;
        }

        /// <summary>
        /// 依据公告Id获取阅读信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Tb_User>(typeof(Tb_User), param) ?? new List<Tb_User>();
            br.Success = true;
            return br;
        }

        /// <summary>
        ///  查询公告详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            Info info = new Info();
            if (param.ContainsKey("bm"))
            {
                Hashtable hb = new Hashtable();
                hb["bm"] = param["bm"];
                Info_Type infoType = new Info_Type();
                infoType = DAL.GetItem<Info_Type>(typeof(Info_Type), hb);
                param["id_info_type"] = infoType.id;
                info = (Info)DAL.GetItem(typeof(Info), param) ?? new Info();

            }
            else
            {
                info = (Info)DAL.GetItem(typeof(Info), param) ?? new Info();
            }
            br.Data = info;
            //当查询我的公告详情时，直接return结果
            if (param.ContainsKey("NoticeFlag"))
            {
                br.Success = true;
                return br;
            }
            //获取info_User对象
            param.Add("id_info", param["id"].ToString());
            Info_User user = (Info_User)DAL.GetItem(typeof(Info_User), param) ?? new Info_User();
            if (user.flag_reade == 0)
            {
                //修改阅读标示
                param.Add("new_rq", DateTime.Now);
                param.Add("new_flag_reade", 1);
                int flag = DAL.UpdatePart(typeof(Info_User), param);
                if (flag == -1)
                {
                    //info公告阅读数量加1
                    int readNum = (int)info.sl_read+1;
                    param.Remove("id_user");
                    param.Add("new_sl_read", readNum);
                    DAL.UpdatePart(typeof(Info), param);
                }
            }
            
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            if (DAL.UpdatePart(typeof(Info), param) < 0)
            {
                br.Success = true;
                br.Message.Add(string.Format("修改阅读标示成功！"));
            }
            else
            {
                br.Success = false;
                br.Message.Add(string.Format("修改阅读标示成功！"));
            }
            return br;
        }

        /// <summary>
        /// 删除公告信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            if (DAL.Delete(typeof(Info), param) ==-1)
            {
                param.Add("id_info", param["id"].ToString());
                param.Remove("id");
                if (DAL.Delete(typeof(Info_User), param) == -1)
                {
                    br.Success = true;
                    br.Message.Add(string.Format("删除公告信息成功！"));
                }
                else
                {
                    br.Success = false;
                    br.Message.Add(string.Format("删除公告接收人群信息失败！"));
                }                                
            }
            else
            {
                br.Success = false;
                br.Message.Add(string.Format("删除公告信息失败！"));
            }
            return br;
        }
    }
}
