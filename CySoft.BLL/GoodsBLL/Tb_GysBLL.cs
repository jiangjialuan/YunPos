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
using CySoft.Model.Td;
using CySoft.Frame.Attributes;
using System.Collections.Generic;
using CySoft.Utility;

namespace CySoft.BLL.GoodsBLL
{
    public class Tb_GysBLL : BaseBLL
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
            result.Data = DAL.QueryList<Tb_Gys_User_QueryModel>(typeof(Tb_Gys), param).ToList();
            return result;
        }

        public override BaseResult GetCount(Hashtable param = null)
        {
            BaseResult res = new BaseResult() { Success = true };
            res.Data = DAL.GetCount(typeof(Tb_Gys), param);
            return res;
        }

        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Tb_Gys), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_Gys>(typeof(Tb_Gys), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }


        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            try
            {
                res.Data = DAL.GetItem<Tb_Gys>(typeof(Tb_Gys), param);
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
            #region 验证参数
            BaseResult result = new BaseResult() { Success = true };
            Tb_Gys gysModel = entity as Tb_Gys;
            if (gysModel == null)
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            if (string.IsNullOrEmpty(gysModel.mc))
            {
                result.Success = false;
                result.Message.Add("名称不允许为空!");
                return result;
            }
            //if (string.IsNullOrEmpty(gysModel.bm))
            //{
            //    result.Success = false;
            //    result.Message.Add("编码不允许为空!");
            //    return result;
            //}
            #endregion
            #region 验证是否重复
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", gysModel.id_masteruser);
            ht.Add("mc", gysModel.mc);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            if (DAL.GetCount(typeof(Tb_Gys), ht) > 0)
            {
                result.Success = false;
                result.Message.Add("输入名称已存在!");
                return result;
            }

            //ht.Clear();
            //ht.Add("id_masteruser", gysModel.id_masteruser);
            //ht.Add("bm", gysModel.bm);
            //ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            //if (DAL.GetCount(typeof(Tb_Gys), ht) > 0)
            //{
            //    result.Success = false;
            //    result.Message.Add("输入编码已存在!");
            //    return result;
            //}
            #endregion
            #region 执行操作
            try
            {
                gysModel.id = Guid.NewGuid().ToString();
                gysModel.flag_delete = (int)Enums.FlagDelete.NoDelete;
                gysModel.rq_create = DateTime.Now;
                gysModel.zjm = CySoft.Frame.Utility.MnemonicCode.chinesecap(gysModel.mc);
                DAL.Add(gysModel);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message.Add("新增操作异常!");
            }
            #endregion
            return result;
        }

        public override BaseResult Update(dynamic entity)
        {
            #region 验证参数
            BaseResult result = new BaseResult() { Success = true };
            Tb_Gys gysModel = entity as Tb_Gys;
            if (gysModel == null || string.IsNullOrEmpty(gysModel.id))
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            if (string.IsNullOrEmpty(gysModel.mc))
            {
                result.Success = false;
                result.Message.Add("名称不允许为空!");
                return result;
            }
            //if (string.IsNullOrEmpty(gysModel.bm))
            //{
            //    result.Success = false;
            //    result.Message.Add("编码不允许为空!");
            //    return result;
            //}
            #endregion
            #region 验证是否重复
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", gysModel.id_masteruser);
            ht.Add("mc", gysModel.mc);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            ht.Add("not_id", gysModel.id);
            if (DAL.GetCount(typeof(Tb_Gys), ht) > 0)
            {
                result.Success = false;
                result.Message.Add("输入名称已存在!");
                return result;
            }

            //ht.Clear();
            //ht.Add("id_masteruser", gysModel.id_masteruser);
            //ht.Add("bm", gysModel.bm);
            //ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            //ht.Add("not_id", gysModel.id);
            //if (DAL.GetCount(typeof(Tb_Gys), ht) > 0)
            //{
            //    result.Success = false;
            //    result.Message.Add("输入编码已存在!");
            //    return result;
            //}
            #endregion
            #region 更新操作
            ht.Clear();
            ht.Add("id", gysModel.id);
            ht.Add("id_masteruser", gysModel.id_masteruser);
            ht.Add("new_bm", gysModel.bm);
            ht.Add("new_mc", gysModel.mc);
            ht.Add("new_lxr", gysModel.lxr);
            ht.Add("new_flag_state", gysModel.flag_state);
            ht.Add("new_tel", gysModel.tel);
            ht.Add("new_companytel", gysModel.companytel);
            ht.Add("new_email", gysModel.email);
            ht.Add("new_zipcode", gysModel.zipcode);
            ht.Add("new_address", gysModel.address);
            ht.Add("new_bz", gysModel.bz);
            ht.Add("new_rq_edit", DateTime.Now);
            ht.Add("new_id_edit", gysModel.id_edit);
            ht.Add("new_id_gysfl", gysModel.id_gysfl);

            try
            {
                DAL.UpdatePart(typeof(Tb_Gys), ht);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message.Add("更新操作异常!");
            }
            #endregion
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

            #region 验证此供应商是否存在进货单
            ht.Clear();
            ht.Add("id_gys", id);
            int jh_count = DAL.GetCount(typeof(Td_Jh_1), ht);

            if (jh_count > 0)
            {
                result.Success = false;
                result.Message.Add("此供应商存在进货单,不允许删除!");
                return result;
            }

            #endregion



            #region 验证此供应商是否在商品中引用
            ht.Clear();
            ht.Add("id_gys", id);
            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            int sp_count = DAL.GetCount(typeof(Tb_Shopsp), ht);

            if (sp_count > 0)
            {
                result.Success = false;
                result.Message.Add("有商品使用此供应商,不允许删除!");
                return result;
            }
            #endregion



            ht.Clear();
            ht.Add("id", id);
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);
            try
            {
                if (DAL.UpdatePart(typeof(Tb_Gys), ht) <= 0)
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

        private bool CheckParam(BaseResult result, Tb_Gys dwModel, string addOrUpdate = "")
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
            //if (string.IsNullOrEmpty(dwModel.bm))
            //{
            //    result.Success = false;
            //    result.Message.Add("请输入编码!");
            //    return false;
            //}
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", dwModel.id_masteruser);
            ht.Add("dw", dwModel.bm);
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
            if (DAL.GetCount(typeof(Tb_Gys), ht) > 0)
            {
                result.Success = false;
                result.Message.Add("输入单位已存在!");
                return false;
            }
            return true;
        }




        #region 供应商导入
        /// <summary>
        /// 供应商导入
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Init(Hashtable param)
        {
            #region 获取数据
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            string FilePath = param["filePath"].ToString();
            string id_masteruser = param["id_masteruser"].ToString();
            string id_user = param["id_user"].ToString();
            string id_shop = param["id_shop"].ToString();
            List<Tb_Gys_Import> list = (List<Tb_Gys_Import>)param["list"];
            List<Tb_Gys_Import> successList = new List<Tb_Gys_Import>();
            List<Tb_Gys_Import> failList = new List<Tb_Gys_Import>();
            List<Tb_Gys> addGYSList = new List<Tb_Gys>();
            List<Tb_Gysfl> addGYSFLList = new List<Tb_Gysfl>();
            #endregion
            #region 获取供应商分类List
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            var gysflList = DAL.QueryList<Tb_Gysfl>(typeof(Tb_Gysfl), ht);
            #endregion
            #region 获取所有供应商
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            var allGysList = DAL.QueryList<Tb_Gys_User_QueryModel>(typeof(Tb_Gys), ht);
            #endregion
            #region 数据处理
            foreach (var item in list)
            {
                br = this.CheckImportInfo(item);
                if (!br.Success)
                {
                    item.bz_sys = br.Message.Count > 0 ? br.Message[0].ToString() : "数据不符合要求";
                    failList.Add(item);
                }
                else
                {
                    #region 验证数据
                    if (list.Where(d => d.mc == item.mc).Count() >= 2)
                    {
                        item.bz_sys = String.Format("导入数据 名称:{0}重复!", item.mc);
                        failList.Add(item);
                        continue;
                    }

                    //if (list.Where(d => d.bm == item.bm).Count() >= 2)
                    //{
                    //    item.bz_sys = String.Format("导入数据 编码:{0}重复!", item.bm);
                    //    failList.Add(item);
                    //    continue;
                    //}

                    if (allGysList.Where(d => d.mc == item.mc).Count() > 0)
                    {
                        item.bz_sys = String.Format("导入数据 名称:{0} 已被占用!", item.mc);
                        failList.Add(item);
                        continue;
                    }

                    //if (allGysList.Where(d => d.bm == item.bm).Count() > 0)
                    //{
                    //    item.bz_sys = String.Format("导入数据 编码:{0} 已被占用!", item.bm);
                    //    failList.Add(item);
                    //    continue;
                    //}

                    #endregion
                    #region 供应商分类 如果不存在 则添加一条
                    string idgysfl = Guid.NewGuid().ToString();
                    if (!string.IsNullOrEmpty(item.id_gysfl) && gysflList != null && gysflList.Where(d => d.mc == item.id_gysfl && d.flag_delete == 0).Count() > 0)
                    {
                        idgysfl = gysflList.Where(d => d.mc == item.id_gysfl && d.flag_delete == 0).FirstOrDefault().id;
                    }
                    else
                    {
                        Tb_Gysfl gysflModel = new Tb_Gysfl()
                        {
                            id_masteruser = id_masteruser,
                            id = idgysfl,
                            bm = DateTime.Now.ToString("yyyyMMddHHmmss"),
                            mc = item.id_gysfl,
                            path = "0/" + idgysfl,
                            id_farther = "0",
                            id_create = id_user,
                            rq_create = DateTime.Now,
                            id_edit = id_user,
                            rq_edit = DateTime.Now,
                            flag_delete = 0,
                            sort_id = 0
                        };
                        gysflList.Add(gysflModel);
                        addGYSFLList.Add(gysflModel);
                    }
                    #endregion
                    #region 供应商model
                    Tb_Gys model = new Tb_Gys()
                    {
                        id = GetGuid,
                        id_masteruser = id_masteruser,
                        bm = item.bm,
                        mc = item.mc,
                        id_gysfl = idgysfl,
                        companytel = item.companytel,
                        zjm = CySoft.Utility.PinYin.GetChineseSpell(item.mc),
                        tel = item.tel,
                        lxr = item.lxr,
                        email = item.email,
                        zipcode = item.zipcode,
                        address = item.address,
                        flag_state = byte.Parse(item.flag_state),
                        bz = item.bz,
                        id_create = id_user,
                        rq_create = DateTime.Now,
                        id_edit = id_user,
                        rq_edit = DateTime.Now,
                        flag_delete = 0
                    };

                    addGYSList.Add(model);
                    #endregion
                    successList.Add(item);
                }
            }
            #endregion
            #region 保存供应商相关信息
            DAL.AddRange(addGYSList);
            DAL.AddRange(addGYSFLList);
            #endregion
            br.Message.Add(String.Format("导入供应商成功!"));
            br.Success = true;
            br.Data = new Tb_Gys_Import_All() { SuccessList = successList, FailList = failList };
            return br;
        }
        #endregion
        

        #region CheckImportInfo
        public BaseResult CheckImportInfo(Tb_Gys_Import model)
        {
            BaseResult br = new BaseResult();
            if (string.IsNullOrEmpty(model.id_gysfl))
            {
                br.Success = false;
                br.Data = "id_gysfl";
                br.Message.Add("供应商分类不能为空");
                return br;
            }

            //if (string.IsNullOrEmpty(model.bm))
            //{
            //    br.Success = false;
            //    br.Data = "bm";
            //    br.Message.Add("编码不能为空");
            //    return br;
            //}

            if (string.IsNullOrEmpty(model.mc))
            {
                br.Success = false;
                br.Data = "mc";
                br.Message.Add("名称不能为空");
                return br;
            }

            if (string.IsNullOrEmpty(model.flag_state) || (model.flag_state != "0" && model.flag_state != "1"))
            {
                br.Success = false;
                br.Data = "flag_state";
                br.Message.Add("状态不符合要求");
                return br;
            }

            br.Success = true;
            return br;
        }
        #endregion



    }
}
