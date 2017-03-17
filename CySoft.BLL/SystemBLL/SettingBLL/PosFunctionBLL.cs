using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using Spring.Collections;

namespace CySoft.BLL.SystemBLL.SettingBLL
{
    public class PosFunctionBLL : BaseBLL, ITb_Pos_FunctionBLL
    {

        public override BaseResult Add(dynamic entity)
        {
            BaseResult res=new BaseResult(){Success = true};
            try
            {
                Tb_Pos_Function model = entity as Tb_Pos_Function;
                if (model == null)
                {
                    res.Success = false;
                    res.Message.Add("参数有误!");
                    res.Level = ErrorLevel.Warning;
                    return res;
                }
                model.rq_create = DateTime.Now;
                model.id = Guid.NewGuid().ToString();
                model.flag_stop = (int)Enums.FlagStop.Start;
                Hashtable ht = new Hashtable();
                ht.Add("bm", model.bm);
                if (DAL.GetCount(typeof(Tb_Pos_Function), ht)>0)
                {
                    res.Success = false;
                    res.Message.Add("功能编码已存在，请重新输入!");
                    return res;
                }
                DAL.Add(model);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Level=ErrorLevel.Error;
                res.Message.Add("新增操作异常!");
            }
            return res;
        }

        public override BaseResult Update(dynamic entity)
        {
            BaseResult res = new BaseResult(){Success = true};
            try
            {
                Tb_Pos_Function model = entity as Tb_Pos_Function;
                if (model == null)
                {
                    res.Success = false;
                    res.Message.Add("参数有误!");
                    res.Level = ErrorLevel.Warning;
                    return res;
                }
                Hashtable param = new Hashtable();
                param.Add("bm", model.bm);
                param.Add("not_id", model.id);
                if (DAL.GetCount(typeof(Tb_Pos_Function), param )> 0)
                {
                    res.Success = false;
                    res.Message.Add("功能编码已存在，请重新输入!");
                    return res;
                }
                param.Clear();
                param.Add("new_mc", model.mc);
                param.Add("new_flag_system",model.flag_system);
                param.Add("new_functiondescribe", model.functiondescribe);
                param.Add("new_flag_type", model.flag_type);
                param.Add("new_sort_id", model.sort_id);
                param.Add("new_flag_stop", model.flag_stop);
                param.Add("new_bm", model.bm);
                param.Add("new_regex", model.regex);
                param.Add("id", model.id);
                if (DAL.UpdatePart(typeof(Tb_Pos_Function), param)<= 0)
                {
                    res.Success = false;
                    res.Level=ErrorLevel.Warning;
                    res.Message.Add("更新操作失败!");
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Level=ErrorLevel.Error;
                res.Message.Add("更新操作异常!");
            }
            return res;
        }

        public override BaseResult Delete(Hashtable param)
        {
            BaseResult res = new BaseResult(){Success = true};
            try
            {
                //if (!param.ContainsKey("id_pos_function") || !param.ContainsKey("id_role"))
                //{
                //    res.Success = false;
                //    res.Message.Add("参数有误!");
                //    return res;
                //}
                Hashtable ht = new Hashtable();
                ht.Add("id_pos_function", param["id"]);
                if (DAL.GetCount(typeof(Tb_Role_Pos_Function), ht)>0)
                {
                    res.Success = false;
                    res.Message.Add("此功能已有使用，不能删除!");
                    return res;
                }
                if (DAL.Delete(typeof(Tb_Pos_Function), param)<=0)
                {
                    res.Success = false;
                    res.Message.Add("删除操作失败!");
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Level = ErrorLevel.Error;
                res.Message.Add("删除操作异常!");
            }
            return res;
        }

        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult res=new BaseResult();
            try
            {
                res.Data = DAL.QueryList<Tb_Pos_Function>(typeof(Tb_Pos_Function), param);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Level = ErrorLevel.Error;
                res.Message.Add("操作异常!");
            }
            
            return res;
        }


        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult(){Success = true};
            if (param==null||param.Count<=0)
            {
                res.Success = false;
                res.Level=ErrorLevel.Warning;
                res.Message.Add("参数有误!");
                return res;
            }
            try
            {
                res.Data = DAL.GetItem(typeof(Tb_Pos_Function), param);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Level = ErrorLevel.Error;
                res.Message.Add("操作异常!");
            }
            return res;
        }

        public IList<Tb_Pos_Function_Tree> GetFunctionTree(Hashtable param)
        {
            param.Add("flag_stop", 0);
            param.Add("sort", "sort_id");
            param.Add("dir", "asc");

            var list = DAL.QueryList<Tb_Pos_Function>(typeof(Tb_Pos_Function), param);

            var sourceList = list.Select(d => new Tb_Pos_Function_Tree
            {
                id = d.id,
                flag_stop = d.flag_stop,
                flag_type = d.flag_type,
                id_create = d.id_create,
                mc = d.mc,
                rq_create = d.rq_create,
                sort_id = d.sort_id,
                version = d.version,
                isclose = true,
                functiondescribe = d.functiondescribe,
                flag_system = d.flag_system,
                bm=d.bm,
                regex = d.regex
            }).ToList();


            var root = new Tb_Pos_Function_Tree
            {
                id = "0",
                flag_type = (byte)Enums.TbPosFunctionFlagType.module,
                mc = "根目录",
                children = sourceList,
                isclose = false
            };
            var rootList = new List<Tb_Pos_Function_Tree>();
            rootList.Add(root);
            return rootList;
        }

        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Tb_Pos_Function), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_Pos_Function>(typeof(Tb_Pos_Function), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }


    }

}
