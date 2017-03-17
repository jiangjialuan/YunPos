using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using System.Collections;
using CySoft.IBLL;
using CySoft.IDAL;
using CySoft.Frame.Attributes;

#region 反馈管理
#endregion

namespace CySoft.BLL.FaqBLL
{
    public class FaqBLL : TreeBLL, IFaqBLL
    {
        public IFaqDAL FaqDAL { get; set; }

        /// <summary>
        /// 新增
        /// znt 2015-05-11
        /// </summary>
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Faq faq = (Faq)entity;
            if (string.IsNullOrEmpty(faq.id.ToString()))
            {
                br.Data = "id";
                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add(string.Format("添加失败！标识列Id不能为空"));
                return br;
            }
            DAL.Add(faq);
            br.Success = true;
            return br;
        }


        /// <summary>
        /// 修改
        /// wzp 2015-5-27
        /// </summary>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            //修改状态为已解决
            if (param["new_flag_state"].ToString() == "2")
            {
                if (DAL.UpdatePart(typeof(Faq), param) < 0)
                {
                    br.Success = true;
                    br.Message.Add(string.Format("修改为已解决状态成功！问题Id：{0}", param["orFatherId"]));
                }
                else
                {
                    br.Success = false;
                    br.Message.Add(string.Format("修改为已解决状态失败！问题Id：{0}", param["orFatherId"]));
                }
            }
            //修改问题记录行状态为已回复
            else
            {
                if (DAL.UpdatePart(typeof(Faq), param) < 0)
                {
                    br.Success = true;
                    br.Message.Add(string.Format("修改为已回复状态成功！问题Id：{0}", param["id"]));
                }
                else
                {
                    br.Success = false;
                    br.Message.Add(string.Format("修改为已回复状态失败！问题Id：{0}", param["id"]));
                }
            }
            return br;
            
        }

        /// <summary>
        ///  获取所有的反馈
        ///  znt 2015-05-11
        /// </summary>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Faq_Tree>(typeof(Faq), param) ?? new List<Faq_Tree>();
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 分页获取数据
        /// znt 2015-05-11
        /// </summary>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();            
            IList<Faq_Tree> list = DAL.QueryPage<Faq_Tree>(typeof(Faq), param);//
            if (list.Count > 0)
            {
                IList<Faq_Tree> lst = CreateTree(list);
                Faq_Tree f = null;
                for (int i = 0; i < lst.Count; i++)
                {
                    for (int y = i; y < lst.Count; y++)
                    {
                        if (lst[i].id < lst[y].id)
                        {
                            f = lst[i];
                            lst[i] = lst[y];
                            lst[y] = f;
                        }
                    }
                }
                pn.Data = lst;
            }
            pn.Success = true;
            return pn;
        }

        /// <summary>
        /// 数量
        /// znt 2015-05-11
        /// </summary>
        public override BaseResult GetCount(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.GetCount(typeof(Faq), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 接口分页
        /// wzp 2015-6-30
        /// </summary>
        public PageNavigate QueryServicePage(Hashtable param)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.GetCount(typeof(Faq), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = FaqDAL.QueryServicePage(typeof(Faq), param);
            }
            return pn;
        }

        /// <summary>
        /// 供应商回复客户
        /// znt 2015-02-15
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public BaseResult ReplyClient(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Faq model = (Faq)entity;

            //检查回复的主题是否存在
            Hashtable param = new Hashtable();
            param.Add("id", model.fatherId);
            Faq fatherModel = DAL.GetItem<Faq>(typeof(Faq), param);
            if (fatherModel == null)
            {
                br.Data = "none";
                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add(string.Format("回复失败！该反馈内容不存在或资料已缺失！请检查后再试。"));
                return br;
            }

            //检查状态是否符合
            if (fatherModel.flag_state == 2)
            {
                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add(string.Format("该反馈内容为已解决状态，不允许再回复内容！"));
                return br;
            }

            //跟新主题状态
            param.Clear();
            param.Add("id", fatherModel.id);
            param.Add("new_flag_state", 1); // 已回复
            DAL.UpdatePart(typeof(Faq), param);

            DAL.Add(model);
            br.Success = true;
            br.Message.Add(string.Format("回复成功，流水号：{0}", model.id));
            return br;

        }      

        /// <summary>
        /// 获取树状的通知数据结构
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetTree(Hashtable param = null)
        {
            BaseResult br = new BaseResult();         
            IList<Faq_Tree> list = DAL.QueryList<Faq_Tree>(typeof(Faq), param);
            if (list.Count > 0)
            {
                IList<Faq_Tree> lst = CreateTree(list);
                Faq_Tree f = null;
                for (int i = 0; i < lst.Count; i++)
                {
                    for (int y = i; y < lst.Count; y++)
                    {
                        if (lst[i].id < lst[y].id)
                        {
                            f = lst[i];
                            lst[i] = lst[y];
                            lst[y] = f;
                        }
                    }
                }
                br.Data = lst;
            }
            else
            {
                br.Data = CreateTree(list);
            }
            br.Success = true;
            return br;
        }
    }
}
