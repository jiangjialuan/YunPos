#region Imports
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
#endregion

namespace CySoft.BLL.KHBLL
{
    public class Tb_KhBLL : BaseBLL
    {
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
            result.Data = DAL.QueryList<Tb_Kh>(typeof(Tb_Kh), param).ToList();
            return result;
        }
        #endregion


        #region GetPage
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Tb_Kh), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_Kh_Query>(typeof(Tb_Kh), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }
        #endregion

        #region Get
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            try
            {
                res.Data = DAL.GetItem<Tb_Kh_Query>(typeof(Tb_Kh), param);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message.Add("操作异常!");
            }
            return res;
        }
        #endregion

        #region Add
        public override BaseResult Add(dynamic entity)
        {
            #region 验证参数
            BaseResult result = new BaseResult() { Success = true };
            Tb_Kh khModel = entity as Tb_Kh;
            if (khModel == null)
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            if (string.IsNullOrEmpty(khModel.mc))
            {
                result.Success = false;
                result.Message.Add("名称不允许为空!");
                return result;
            }
            //if (string.IsNullOrEmpty(khModel.bm))
            //{
            //    result.Success = false;
            //    result.Message.Add("编码不允许为空!");
            //    return result;
            //}
            if (string.IsNullOrEmpty(khModel.id_khfl))
            {
                result.Success = false;
                result.Message.Add("请选择客户分类!");
                return result;
            }
            #endregion
            #region 验证是否重复
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", khModel.id_masteruser);
            ht.Add("mc", khModel.mc);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            if (DAL.GetCount(typeof(Tb_Kh), ht) > 0)
            {
                result.Success = false;
                result.Message.Add("输入名称已存在!");
                return result;
            }

            //ht.Clear();
            //ht.Add("id_masteruser", khModel.id_masteruser);
            //ht.Add("bm", khModel.bm);
            //ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            //if (DAL.GetCount(typeof(Tb_Kh), ht) > 0)
            //{
            //    result.Success = false;
            //    result.Message.Add("输入编码已存在!");
            //    return result;
            //}
            #endregion
            #region 执行操作
            try
            {
                khModel.id = Guid.NewGuid().ToString();
                khModel.flag_delete = (int)Enums.FlagDelete.NoDelete;
                khModel.rq_create = DateTime.Now;
                khModel.zjm = CySoft.Frame.Utility.MnemonicCode.chinesecap(khModel.mc);
                khModel.id_shop_relate = "";
                DAL.Add(khModel);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message.Add("新增操作异常!");
            }
            #endregion
            return result;
        }

        #endregion

        #region Update
        public override BaseResult Update(dynamic entity)
        {
            #region 验证参数
            BaseResult result = new BaseResult() { Success = true };
            Tb_Kh khModel = entity as Tb_Kh;
            if (khModel == null || string.IsNullOrEmpty(khModel.id))
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            if (string.IsNullOrEmpty(khModel.mc))
            {
                result.Success = false;
                result.Message.Add("名称不允许为空!");
                return result;
            }
            //if (string.IsNullOrEmpty(khModel.bm))
            //{
            //    result.Success = false;
            //    result.Message.Add("编码不允许为空!");
            //    return result;
            //}
            if (string.IsNullOrEmpty(khModel.id_khfl))
            {
                result.Success = false;
                result.Message.Add("请选择客户分类!");
                return result;
            }
            #endregion
            #region 验证是否重复
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", khModel.id_masteruser);
            ht.Add("mc", khModel.mc);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            ht.Add("not_id", khModel.id);
            if (DAL.GetCount(typeof(Tb_Kh), ht) > 0)
            {
                result.Success = false;
                result.Message.Add("输入名称已存在!");
                return result;
            }

            //ht.Clear();
            //ht.Add("id_masteruser", khModel.id_masteruser);
            //ht.Add("bm", khModel.bm);
            //ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            //ht.Add("not_id", khModel.id);
            //if (DAL.GetCount(typeof(Tb_Kh), ht) > 0)
            //{
            //    result.Success = false;
            //    result.Message.Add("输入编码已存在!");
            //    return result;
            //}
            #endregion
            #region 更新操作
            ht.Clear();
            ht.Add("id", khModel.id);
            ht.Add("id_masteruser", khModel.id_masteruser);
            ht.Add("new_bm", khModel.bm);
            ht.Add("new_mc", khModel.mc);
            ht.Add("new_zjm", CySoft.Frame.Utility.MnemonicCode.chinesecap(khModel.mc));
            ht.Add("new_lxr", khModel.lxr);
            ht.Add("new_flag_state", khModel.flag_state);
            ht.Add("new_tel", khModel.tel);
            ht.Add("new_companytel", khModel.companytel);
            ht.Add("new_email", khModel.email);
            ht.Add("new_zipcode", khModel.zipcode);
            ht.Add("new_address", khModel.address);
            ht.Add("new_bz", khModel.bz);

            ht.Add("new_je_xyed", khModel.je_xyed);
            ht.Add("new_je_xyed_temp", khModel.je_xyed_temp);
            ht.Add("new_rq_xyed_temp_b", khModel.rq_xyed_temp_b);
            ht.Add("new_rq_xyed_temp_e", khModel.rq_xyed_temp_e);

            ht.Add("new_rq_edit", DateTime.Now);
            ht.Add("new_id_edit", khModel.id_edit);
            ht.Add("new_id_khfl", khModel.id_khfl);

            try
            {
                DAL.UpdatePart(typeof(Tb_Kh), ht);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message.Add("更新操作异常!");
            }
            #endregion
            return result;
        }
        #endregion

        #region Delete
        public override BaseResult Delete(Hashtable param)
        {
            #region 获取数据
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
            #region 验证此客户是否绑定门店
            Hashtable ht = new Hashtable();
            ht.Clear();
            ht.Add("id_kh", id);
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            var shop = DAL.QueryList<Tb_Shop>(typeof(Tb_Shop), ht).FirstOrDefault();

            if (shop != null && !string.IsNullOrEmpty(shop.id))
            {
                result.Success = false;
                result.Message.Add("此客户已经绑定门店: " + shop.mc + "[" + shop.bm + "] ,不允许删除!");
                return result;
            }
            #endregion
            #region 执行删除操作
            ht.Clear();
            ht.Add("id", id);
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);
            try
            {
                if (DAL.UpdatePart(typeof(Tb_Kh), ht) <= 0)
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
            return result;
        }
        #endregion

        #region GetCount
        public override BaseResult GetCount(Hashtable param = null)
        {
            BaseResult res = new BaseResult() { Success = true };
            res.Data = DAL.GetCount(typeof(Tb_Kh), param);
            return res;
        }
        #endregion

        #region Init
        /// <summary>
        /// Init 客户导入
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
            List<Tb_Kh_Import> list = (List<Tb_Kh_Import>)param["list"];
            List<Tb_Kh_Import> successList = new List<Tb_Kh_Import>();
            List<Tb_Kh_Import> failList = new List<Tb_Kh_Import>();
            List<Tb_Kh> addKHList = new List<Tb_Kh>();
            List<Tb_Khfl> addKHFLList = new List<Tb_Khfl>();
            #endregion
            #region 获取客户分类List
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            var khflList = DAL.QueryList<Tb_Khfl>(typeof(Tb_Khfl), ht);
            #endregion
            #region 获取所有客户
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            var allKhList = DAL.QueryList<Tb_Kh>(typeof(Tb_Kh), ht);
            #endregion
            #region 数据处理
            foreach (var item in list)
            {
                br = this.CheckImportInfo(item);
                if (!br.Success)
                {
                    item.bz_kh = br.Message.Count > 0 ? br.Message[0].ToString() : "数据不符合要求";
                    failList.Add(item);
                }
                else
                {
                    #region 验证数据
                    if (list.Where(d => d.mc == item.mc).Count() >= 2)
                    {
                        item.bz_kh = String.Format("导入数据 名称:{0}重复!", item.mc);
                        failList.Add(item);
                        continue;
                    }

                    //if (list.Where(d => d.bm == item.bm).Count() >= 2)
                    //{
                    //    item.bz_kh = String.Format("导入数据 编码:{0}重复!", item.bm);
                    //    failList.Add(item);
                    //    continue;
                    //}

                    if (allKhList.Where(d => d.mc == item.mc).Count() > 0)
                    {
                        item.bz_kh = String.Format("导入数据 名称:{0} 已被占用!", item.mc);
                        failList.Add(item);
                        continue;
                    }

                    //if (allKhList.Where(d => d.bm == item.bm).Count() > 0)
                    //{
                    //    item.bz_kh = String.Format("导入数据 编码:{0} 已被占用!", item.bm);
                    //    failList.Add(item);
                    //    continue;
                    //}

                    #endregion
                    #region 客户分类 如果不存在 则添加一条
                    string idkhfl = Guid.NewGuid().ToString();
                    if (!string.IsNullOrEmpty(item.id_khfl) && khflList != null && khflList.Where(d => d.mc == item.id_khfl && d.flag_delete == 0).Count() > 0)
                    {
                        idkhfl = khflList.Where(d => d.mc == item.id_khfl && d.flag_delete == 0).FirstOrDefault().id;
                    }
                    else
                    {
                        Tb_Khfl khflModel = new Tb_Khfl()
                        {
                            id_masteruser = id_masteruser,
                            id = idkhfl,
                            bm = DateTime.Now.ToString("yyyyMMddHHmmss"),
                            mc = item.id_khfl,
                            path = "0/" + idkhfl,
                            id_farther = "0",
                            id_create = id_user,
                            rq_create = DateTime.Now,
                            id_edit = id_user,
                            rq_edit = DateTime.Now,
                            flag_delete = 0,
                            sort_id = 0
                        };
                        khflList.Add(khflModel);
                        addKHFLList.Add(khflModel);
                    }
                    #endregion
                    #region 客户model
                    Tb_Kh model = new Tb_Kh()
                    {
                        id = GetGuid,
                        id_masteruser = id_masteruser,
                        bm = item.bm,
                        mc = item.mc,
                        id_khfl = idkhfl,
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
                        flag_delete = 0,
                        je_xyed=decimal.Parse( item.je_xyed),
                        je_xyed_temp= decimal.Parse(item.je_xyed_temp)
                    };

                    DateTime rq_xyed_temp_b = DateTime.Now;
                    if (item.rq_xyed_temp_b != null&&!string.IsNullOrEmpty(item.rq_xyed_temp_b) && DateTime.TryParse(item.rq_xyed_temp_b,out rq_xyed_temp_b))
                    {
                        model.rq_xyed_temp_b = rq_xyed_temp_b;
                    }

                    DateTime rq_xyed_temp_e = DateTime.Now;
                    if (item.rq_xyed_temp_e != null && !string.IsNullOrEmpty(item.rq_xyed_temp_e) && DateTime.TryParse(item.rq_xyed_temp_e, out rq_xyed_temp_e))
                    {
                        model.rq_xyed_temp_e = rq_xyed_temp_e;
                    }

                    addKHList.Add(model);
                    #endregion
                    successList.Add(item);
                }
            }
            #endregion
            #region 保存客户相关信息
            DAL.AddRange(addKHList);
            DAL.AddRange(addKHFLList);
            #endregion
            br.Message.Add(String.Format("导入客户成功!"));
            br.Success = true;
            br.Data = new Tb_Kh_Import_All() { SuccessList = successList, FailList = failList };
            return br;
        }
        #endregion

        #region CheckImportInfo
        public BaseResult CheckImportInfo(Tb_Kh_Import model)
        {
            BaseResult br = new BaseResult();
            if (string.IsNullOrEmpty(model.id_khfl))
            {
                br.Success = false;
                br.Data = "id_khfl";
                br.Message.Add("客户分类不能为空");
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

            if (!string.IsNullOrEmpty(model.je_xyed)&&!CyVerify.IsNumeric(model.je_xyed))
            {
                br.Success = false;
                br.Data = "je_xyed";
                br.Message.Add("信用额度不符合要求");
                return br;
            }

            if (!string.IsNullOrEmpty(model.je_xyed_temp) && !CyVerify.IsNumeric(model.je_xyed_temp))
            {
                br.Success = false;
                br.Data = "je_xyed_temp";
                br.Message.Add("临时额度不符合要求");
                return br;
            }


            br.Success = true;
            return br;
        }
        #endregion


    }
}
