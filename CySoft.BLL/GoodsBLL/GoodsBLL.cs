using System;
using System.Collections.Generic;
using System.Linq;
using CySoft.Model.Tb;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Model.Other;
using CySoft.Frame.Attributes;
using CySoft.BLL.SystemBLL;
using CySoft.Utility;
using System.IO;
using CySoft.Model.Td;
using CySoft.IBLL;
using System.Web;
using CySoft.BLL.Base;
using CySoft.IDAL;
using CySoft.Model.Flags;
using System.Data;
using CySoft.Model.Ts;

#region 商品
#endregion

namespace CySoft.BLL.GoodsBLL
{
    public class GoodsBLL : BaseBLL, IGoodsBLL
    {

        protected UtiletyBLL utilety = new UtiletyBLL();
        public ITb_SpDAL Tb_SpDAL { get; set; }
        public ITb_Sp_SkuDAL Tb_Sp_SkuDAL { get; set; }
        public ITb_Sp_DjDAL Tb_Sp_DjDAL { get; set; }

        /// <summary>
        /// 新增 商品
        /// znt
        /// 2015-03-03
        /// 修改 增加客户自定义价格
        /// hjb
        /// 2016-05-19
        /// </summary>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            GoodsData model = (GoodsData)entity;
            Hashtable param = new Hashtable();
            utilety.DAL = DAL;
            Tb_Sp model_sp = new Model.Tb.Tb_Sp();

            param.Add("id", model.id);
            param.Add("id_gys", model.id_gys_create);
            param.Add("id_gys_create", model.id_gys_create);
            br = Get(param);
            br.Message.Clear();
            if (br.Success)
            {
                var sp = br.Data as Tb_Sp_Get;
                List<Tb_Sp_Expand> list_Expand = null;
                List<List<Tb_Sp_Expand>> Sp_Expand_list = new List<List<Tb_Sp_Expand>>();

                foreach (var item in model.sku)
                {
                    list_Expand = new List<Tb_Sp_Expand>();
                    foreach (var m in item.sp_expand)
                    {
                        list_Expand.Add(new Tb_Sp_Expand() { id_sp_expand_template = m.id_sp_expand_template, val = m.val });
                    }
                    Sp_Expand_list.Add(list_Expand);
                }

                foreach (var item in Sp_Expand_list)
                {
                    bool ishas = true;
                    foreach (var p in sp.skuList)
                    {
                        string str = string.Empty;
                        ishas = true;
                        foreach (var m in p.specList)
                        {
                            str += m.val + ";";
                            var n = (from o in item where o.id_sp_expand_template.Equals(m.id_spec_group) && o.val.Equals(m.val) select item).Count();
                            if (n.Equals(0))
                            {
                                ishas = false;
                                break;
                            }
                        }
                        if (ishas)
                        {
                            br.Success = false;
                            br.Data = "expand";
                            br.Message.Add(string.Format("商品规格[{0}]已经存在", str));
                            return br;
                        };
                    }
                }
            }
            else
            {

                param.Clear();
                param.Add("not_id", model.id);
                param.Add("mc", model.mc);
                param.Add("id_gys_create", model.id_gys_create);
                if (DAL.GetCount(typeof(Tb_Sp), param) > 0)
                {
                    br.Success = false;
                    br.Data = "mc";
                    br.Message.Add("商品名称已经存在.");
                    return br;
                }

                // 添加 商品 
                model_sp.id = model.id;
                model_sp.flag_stop = 0;
                model_sp.mc = model.mc;
                model_sp.cd = model.cd;
                model_sp.keywords = model.keywords;
                model_sp.brand = model.brand;
                model_sp.id_gys_create = model.id_gys_create;
                model_sp.id_create = model.id_create;
                model_sp.rq_create = model.rq_create;
                model_sp.rq_edit = model.rq_edit;
                model_sp.id_edit = model.id_edit;

                DAL.Add(model_sp);


            }

            //用户编码是否存在
            foreach (var item in model.sku)
            {
                param.Clear();
                param.Add("bm_Interface", item.bm_Interface);
                param.Add("id_gys", model.id_gys_create);
                if (DAL.GetCount(typeof(Tb_Gys_Sp), param) > 0)
                {
                    br.Success = false;
                    br.Data = "bm";
                    br.Message.Add(string.Format("商品编码[{0}]重复,请重新输入商品编码", item.bm_Interface));
                    throw new CySoftException(br);
                }
            }

            // 校验系统编码是否存在

            foreach (var item in model.sku)
            {
                param.Clear();
                param.Add("bm", item.bm);
                if (DAL.GetCount(typeof(Tb_Sp_Sku), param) > 0)
                {
                    br.Success = false;
                    br.Data = "bm";
                    br.Message.Add(string.Format("商品系统编码[{0}]重复，请重新添加商品", item.bm));
                    throw new CySoftException(br);
                }
            }

            param.Clear();
            param.Add("id_gys", model.id_gys_create);
            param.Add("name", model.unit);
            if (DAL.GetCount(typeof(Tb_Unit), param).Equals(0))
            {
                //新增单位
                var unit = new Tb_Unit() { id_gys = model.id_gys_create, name = model.unit };
                DAL.Add<Tb_Unit>(unit);
            }


            #region 商品图册

            SaveGoodsPic(model.sp_pic, model.id, model.id_create);
            #endregion

            #region 商品 sku + 供应商 商品
            List<Tb_Sp_Sku> list_sku = new List<Tb_Sp_Sku>();
            List<Tb_Gys_Sp> list_gys_sp = new List<Tb_Gys_Sp>();
            List<Tb_Sp_Expand> list_expand = new List<Tb_Sp_Expand>();
            List<Tb_Sp_Dj> list_dj = new List<Tb_Sp_Dj>();
            List<Tb_Sp_Info> list_sp_info = new List<Tb_Sp_Info>();
            List<Tb_Sp_Cgs> list_sp_cgs = new List<Tb_Sp_Cgs>();
            List<Tb_Gys_Sp_Tag> list_sp_tag = new List<Tb_Gys_Sp_Tag>();
            foreach (var item in model.sku)
            {
                Tb_Sp_Sku model_sku = new Tb_Sp_Sku();
                Tb_Gys_Sp model_gys_sp = new Tb_Gys_Sp();

                // 添加 sku
                model_sku.id = utilety.GetNextKey(typeof(Tb_Sp_Sku));
                model_sku.id_sp = model.id;
                model_sku.bm = item.bm;
                model_sku.unit = model.unit;
                model_sku.flag_stop = 0;
                model_sku.barcode = item.barcode;
                model_sku.description = model.description;
                model_sku.keywords = model.keywords;

                // 图片 
                if (!string.IsNullOrEmpty(item.photo))
                {
                    string[] url_img = item.photo.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                    string guid = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(url_img[url_img.Length - 1]);
                    string fileName = guid + extension;

                    ImgExtension.MakeThumbnail(item.photo, "/UpLoad/Goods/thumb/_480x480_" + fileName, 480, 480, ImgCreateWay.Cut, false);
                    ImgExtension.MakeThumbnail(item.photo, "/UpLoad/Goods/thumb/_200x200_" + fileName, 200, 200, ImgCreateWay.Cut, false);
                    ImgExtension.MakeThumbnail(item.photo, "/UpLoad/Goods/thumb/_60x60_" + fileName, 60, 60, ImgCreateWay.Cut, false);

                    model_sku.photo = string.Format("/UpLoad/Goods/thumb/_480x480_{0}", fileName); ; ; //480x480
                    model_sku.photo_min = string.Format("/UpLoad/Goods/thumb/_200x200_{0}", fileName); ; ; //200x200
                    model_sku.photo_min2 = string.Format("/UpLoad/Goods/thumb/_60x60_{0}", fileName); ; ; // 60x60
                }
                model_sku.id_create = model.id_create;
                model_sku.rq_create = model.rq_create;
                model_sku.id_edit = model.id_edit;
                model_sku.rq_edit = model.rq_edit;

                list_sku.Add(model_sku);

                // 添加 供应商 商品
                model_gys_sp.id_gys = model.id_gys_create;
                model_gys_sp.id_sku = model_sku.id;
                model_gys_sp.id_sp = model.id;
                model_gys_sp.flag_stop = 0;
                model_gys_sp.bm_Interface = item.bm_Interface;
                model_gys_sp.id_spfl = model.id_spfl;
                model_gys_sp.sort_id = item.sort_id;
                model_gys_sp.dj_base = item.dj_base.Digit(DigitConfig.dj);
                model_gys_sp.zhl = item.zhl;
                model_gys_sp.flag_up = item.flag_up;
                model_gys_sp.id_create = model.id_create;
                model_gys_sp.rq_create = model.rq_create;
                model_gys_sp.id_edit = model.id_edit;

                list_gys_sp.Add(model_gys_sp);


                if (!string.IsNullOrEmpty(model.description))
                {
                    list_sp_info.Add(new Tb_Sp_Info() { description = model.description, id_sp = model.id, id_sku = model_sku.id });
                }

                #region 商品 规格属性


                foreach (var expand in item.sp_expand)
                {
                    param.Clear();
                    param.Add("id", expand.id_sp_expand_template);
                    if (!(DAL.GetCount(typeof(Tb_Sp_Expand_Template), param) == 1))
                    {
                        br.Success = false;
                        br.Data = "expand";
                        br.Message.Add(string.Format("商品规格ID[{0}]已经不存在", expand.id_sp_expand_template));
                        throw new CySoftException(br);
                    }

                    Tb_Sp_Expand model_expand = new Tb_Sp_Expand();
                    model_expand.id_sku = model_sku.id;
                    model_expand.id_sp = model.id;
                    model_expand.id_sp_expand_template = expand.id_sp_expand_template;
                    model_expand.val = expand.val;
                    model_expand.id_create = model.id_create;
                    model_expand.rq_create = model.rq_create;
                    model_expand.id_edit = model.id_edit;
                    model_expand.rq_edit = model.rq_edit;

                    list_expand.Add(model_expand);

                }


                #endregion


                #region 商品 单价

                foreach (var dj in item.sp_dj)
                {
                    Tb_Sp_Dj model_dj = new Tb_Sp_Dj();
                    model_dj.id_gys = model.id_gys_create;
                    model_dj.id_sku = model_sku.id;
                    model_dj.id_cgs_level = dj.id_cgs_level;
                    model_dj.id_sp = model.id;
                    model_dj.dj_dh = dj.dj_dh.Digit(DigitConfig.dj);
                    model_dj.sl_dh_min = dj.sl_dh_min;
                    model_dj.id_create = model.id_create;
                    model_dj.rq_create = model.rq_create;
                    model_dj.id_edit = model.id_edit;
                    model_dj.rq_edit = model.rq_edit;

                    list_dj.Add(model_dj);
                }


                #endregion

                #region 自定义客户价格
                if (model.sp_cgs != null && model.sp_cgs.Count > 0)
                {
                    foreach (var cgsdj in model.sp_cgs)
                    {
                        Tb_Sp_Cgs tsc = new Tb_Sp_Cgs();
                        tsc.id_sp = model.id;
                        tsc.id_sku = model_sku.id;
                        tsc.sl_dh_min = cgsdj.sl_dh_min;
                        tsc.dj_dh = cgsdj.dj_dh;
                        tsc.id_cgs = cgsdj.id_cgs;
                        tsc.id_gys = model.id_gys_create;
                        tsc.id_create = model.id_create;
                        tsc.rq_create = model.rq_create;
                        tsc.id_edit = model.id_edit;
                        tsc.rq_edit = model.rq_edit;

                        list_sp_cgs.Add(tsc);
                    }
                }
                #endregion
                //Tb_Gys_Tag
                #region 商品标签
                if (model.Tb_Gys_Tag != null && model.Tb_Gys_Tag.Count > 0)
                {
                    foreach (var tag in model.Tb_Gys_Tag)
                    {
                        Tb_Gys_Sp_Tag itag = new Tb_Gys_Sp_Tag();

                        itag.id_tag = tag.id_tag;
                        itag.id_sp = model.id;
                        itag.id_sku = model_sku.id;
                        itag.id_gys = model.id_gys_create;
                        itag.id_create = model.id_create;
                        itag.rq_create = model.rq_create;
                        itag.id_edit = model.id_edit;
                        itag.rq_edit = model.rq_edit;

                        list_sp_tag.Add(itag);
                    }
                }
                #endregion

            }


            if (list_dj.Count > 0)
            {
                DAL.AddRange(list_dj);
            }

            if (list_expand.Count > 0)
            {
                DAL.AddRange(list_expand);
            }

            if (list_sku.Count > 0)
            {
                DAL.AddRange(list_sku);

            }

            if (list_gys_sp.Count > 0)
            {
                DAL.AddRange(list_gys_sp);
            }

            if (list_sp_info.Count > 0)
            {
                DAL.AddRange(list_sp_info);
            }

            if (list_sp_cgs.Count > 0)
            {
                DAL.AddRange(list_sp_cgs);
            }

            if (list_sp_tag.Count > 0)
            {
                DAL.AddRange(list_sp_tag);
            }
            #endregion

            br.Message.Add(String.Format("新增商品。流水号：{0}，名称:{1}", model.id, model.mc));
            br.Success = true;
            return br;
        }


        private void SaveGoodsPic(string[] list, Nullable<long> id_sp, Nullable<long> id_user)
        {
            int xh = 0;
            List<Tb_Sp_Pic> list_pic = new List<Tb_Sp_Pic>();
            string source_file;
            string target_path = HttpContext.Current.Server.MapPath("/UpLoad/Goods/");
            if (!target_path.EndsWith("\\")) target_path += "\\";
            Hashtable ht = new Hashtable();
            ht.Add("id_sp", id_sp);
            DAL.Delete(typeof(Tb_Sp_Pic), ht);
            foreach (var item in list)
            {
                source_file = HttpContext.Current.Server.MapPath(item);
                if (!string.IsNullOrEmpty(item) && File.Exists(source_file))
                {
                    Tb_Sp_Pic model_pic = new Tb_Sp_Pic() { id_sp = id_sp, xh = Convert.ToByte(xh) };

                    string[] url_img = item.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);


                    string guid = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(url_img[url_img.Length - 1]);
                    string fileName = guid + extension;

                    // 复制 原图 到 Goods 文件夹下
                    FileExtension.Copy(source_file, target_path + fileName);

                    ImgExtension.MakeThumbnail(item, "/UpLoad/Goods/_480x480_" + fileName, 480, 480, ImgCreateWay.Cut, false);
                    model_pic.photo = string.Format("/UpLoad/Goods/{0}", fileName);
                    model_pic.photo_min = string.Format("/UpLoad/Goods/_480x480_{0}", fileName);
                    model_pic.id_create = id_user;
                    model_pic.rq_create = DateTime.Now;
                    list_pic.Add(model_pic);
                    xh++;
                }
            }
            if (list_pic.Count > 0)
            {
                DAL.AddRange(list_pic);
            }
        }

        /// <summary>
        /// 商品修改
        /// tim 2015-04-09
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Save(dynamic entity)
        {
            BaseResult br = new BaseResult();

            var model = (GoodsSave)entity;
            Hashtable ht = new Hashtable();
            ht.Add("id", model.id);
            var sp = DAL.GetItem<Tb_Sp>(typeof(Tb_Sp), ht);

            if (sp == null) { br.Success = false; br.Message.Add("商品不存在"); br.Level = ErrorLevel.Warning; return br; }

            //如果是商品的原创建供应商
            if (sp.id_gys_create.Equals(model.id_gys))
            {
                ht.Clear();
                ht.Add("not_id", model.id);
                ht.Add("id_gys_create", model.id_gys);
                ht.Add("mc", model.mc);

                if (DAL.GetCount(typeof(Tb_Sp), ht) > 0)
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    br.Message.Add("此商品名称已存在，请重命名.");
                    return br;
                }
                //更新商品名称
                ht.Clear();
                ht.Add("id", model.id);
                ht.Add("id_gys_create", model.id_gys);
                ht.Add("new_mc", model.mc);
                ht.Add("new_keywords", model.keywords);
                ht.Add("new_id_edit", model.id_edit);
                ht.Add("new_rq_edit", model.rq_edit);
                DAL.UpdatePart(typeof(Tb_Sp), ht);

                ht.Clear();
                ht.Add("id_gys", model.id_gys);
                ht.Add("name", model.unit);
                if (DAL.GetCount(typeof(Tb_Unit), ht).Equals(0))
                {
                    //新增单位
                    var unit = new Tb_Unit() { id_gys = model.id_gys, name = model.unit };
                    DAL.Add<Tb_Unit>(unit);
                }

                ht.Clear();
                //更新商品描述
                List<Tb_Sp_Info> list_sp_info = new List<Tb_Sp_Info>();
                if (model.applyallsku.Equals("1"))
                {
                    ht.Add("id_sp", model.id);
                    DAL.Delete(typeof(Tb_Sp_Info), ht);

                    if (!string.IsNullOrWhiteSpace(model.description))
                    {
                        foreach (var m in model.sku)
                        {
                            list_sp_info.Add(new Tb_Sp_Info() { id_sku = m.id, id_sp = model.id, description = model.description });
                        }
                    }
                }
                else
                {
                    ht.Add("id_sku", model.id_sku);
                    DAL.Delete(typeof(Tb_Sp_Info), ht);
                    if (!string.IsNullOrWhiteSpace(model.description))
                    {
                        list_sp_info.Add(new Tb_Sp_Info() { id_sku = model.id_sku, id_sp = model.id, description = model.description });
                    }
                }

                if (list_sp_info.Count > 0)
                {
                    DAL.AddRange(list_sp_info);
                }


                //更新SKU
                foreach (var m in model.sku)
                {
                    ht.Clear();
                    ht.Add("id", m.id);
                    ht.Add("id_sp", model.id);
                    ht.Add("new_barcode", m.barcode);
                    ht.Add("new_id_edit", model.id_edit);
                    ht.Add("new_rq_edit", model.rq_edit);
                    if (model.applyallsku.Equals("1"))
                    {
                        ht.Add("new_keywords", model.keywords);
                    }
                    else if (m.id.Equals(model.id_sku))
                    {

                        ht.Add("new_keywords", model.keywords);
                    }
                    if (model.applyunit.Equals("1"))
                    {
                        ht.Add("new_unit", model.unit);
                    }
                    else if (m.id.Equals(model.id_sku))
                    {
                        ht.Add("new_unit", model.unit);
                    }

                    // 图片 
                    if (!string.IsNullOrEmpty(m.photo))
                    {
                        string[] url_img = m.photo.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                        string guid = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(url_img[url_img.Length - 1]);
                        string fileName = guid + extension;

                        string p_min_2 = string.Format("/UpLoad/Goods/thumb/_60x60_{0}", fileName);
                        string p_min = string.Format("/UpLoad/Goods/thumb/_200x200_{0}", fileName);
                        string p = string.Format("/UpLoad/Goods/thumb/_480x480_{0}", fileName);

                        ImgExtension.MakeThumbnail(m.photo, p, 480, 480, ImgCreateWay.Cut, false);
                        ImgExtension.MakeThumbnail(m.photo, p_min, 200, 200, ImgCreateWay.Cut, false);
                        ImgExtension.MakeThumbnail(m.photo, p_min_2, 60, 60, ImgCreateWay.Cut, false);
                        ht.Add("new_photo_min2", p_min_2);// 60x60
                        ht.Add("new_photo_min", p_min);//200x200
                        ht.Add("new_photo", p); //480x480
                    }

                    DAL.UpdatePart(typeof(Tb_Sp_Sku), ht);
                }

                //更新规格
                ht.Clear();
                ht.Add("id_sp", model.id);
                DAL.Delete(typeof(Tb_Sp_Expand), ht);
                if (model.spec.Count > 0)
                {
                    List<Tb_Sp_Expand> list = new List<Tb_Sp_Expand>();
                    foreach (var m in model.spec)
                    {
                        var p = new Tb_Sp_Expand();
                        p.id_create = model.id_create;
                        p.rq_create = DateTime.Now;
                        p.id_edit = p.id_create;
                        p.rq_edit = p.rq_create;
                        p.id_sku = m.id_sku;
                        p.id_sp = model.id;
                        p.id_sp_expand_template = m.id_sp_expand_template;
                        p.val = m.val;
                        list.Add(p);
                    }
                    DAL.AddRange<Tb_Sp_Expand>(list);
                }
            }

            //更新供应商商品SKU
            foreach (var m in model.sku)
            {
                ht.Clear();
                ht.Add("id_sku", m.id);
                ht.Add("id_gys", model.id_gys);
                ht.Add("id_sp", model.id);
                ht.Add("new_dj_base", m.dj_base.Digit(DigitConfig.dj));
                ht.Add("new_flag_up", m.flag_up);
                if (model.applyallsku.Equals("1"))
                    ht.Add("new_sort_id", model.sort_id);
                else if (m.id.Equals(model.id_sku))
                    ht.Add("new_sort_id", model.sort_id);
                ht.Add("new_id_spfl", model.id_spfl);
                ht.Add("new_id_edit", model.id_edit);
                ht.Add("new_rq_edit", model.rq_edit);

                DAL.UpdatePart(typeof(Tb_Gys_Sp), ht);
            }

            //更新供应商商品单价
            ht.Clear();
            ht.Add("id_sp", model.id);
            ht.Add("id_gys", model.id_gys);
            DAL.Delete(typeof(Tb_Sp_Dj), ht);
            if (model.dj.Count > 0)
            {
                List<Tb_Sp_Dj> list = new List<Tb_Sp_Dj>();
                foreach (var m in model.dj)
                {
                    var p = new Tb_Sp_Dj();
                    p.id_sp = model.id;
                    p.id_gys = model.id_gys;
                    p.id_cgs_level = m.id_cgs_level;
                    p.id_sku = m.id_sku;
                    p.dj_dh = m.dj_dh.Digit(DigitConfig.dj);
                    p.sl_dh_min = m.sl_dh_min.Digit(DigitConfig.sl);

                    p.id_create = model.id_create;
                    p.rq_create = DateTime.Now;
                    p.id_edit = p.id_create;
                    p.rq_edit = p.rq_create;

                    list.Add(p);
                }
                DAL.AddRange<Tb_Sp_Dj>(list);
            }

            ht.Clear();

            #region 更新客户自定义价格
            //判断自定义客户价格是否应用于所有规格商品
            if (model.applyallcgs)
            {
                //是 删除该供应商 该商品下的所有sku自定义价格信息
                ht.Add("id_sp", model.id);
                ht.Add("id_gys", model.id_gys);
                DAL.Delete(typeof(Tb_Sp_Cgs), ht);

                //重新插入所有sku自定义价格信息
                IList<Tb_Sp_Cgs> cgsList = new List<Tb_Sp_Cgs>();
                foreach (var cgs in model.cgs)
                {
                    foreach (var sku in model.sku)
                    {
                        cgsList.Add(new Tb_Sp_Cgs
                        {
                            id_gys = model.id_gys,
                            id_cgs = cgs.id_cgs,
                            id_sku = sku.id,
                            id_sp = model.id,
                            sl_dh_min = cgs.sl_dh_min,
                            dj_dh = cgs.dj_dh,
                            id_create = cgs.id_create == 0 ? model.id_create : cgs.id_create,
                            id_edit = model.id_edit,
                            rq_create = cgs.rq_create,
                            rq_edit = model.rq_edit
                        });
                    }
                }

                //更新商品自定义价格
                DAL.AddRange<Tb_Sp_Cgs>(cgsList);
            }
            else
            {
                //否 删除该供应商 该sku下的所有自定义价格信息
                ht.Add("id_sp", model.id);
                ht.Add("id_gys", model.id_gys);
                ht.Add("id_sku", model.id_sku);
                DAL.Delete(typeof(Tb_Sp_Cgs), ht);

                //重新插入该sku自定义价格信息
                IList<Tb_Sp_Cgs> cgsList = new List<Tb_Sp_Cgs>();

                foreach (var cgs in model.cgs)
                {
                    cgsList.Add(new Tb_Sp_Cgs
                    {
                        id_gys = model.id_gys,
                        id_cgs = cgs.id_cgs,
                        id_sku = model.id_sku,
                        id_sp = model.id,
                        sl_dh_min = cgs.sl_dh_min,
                        dj_dh = cgs.dj_dh,
                        id_create = cgs.id_create == 0 ? model.id_create : cgs.id_create,
                        id_edit = model.id_edit,
                        rq_create = cgs.rq_create,
                        rq_edit = model.rq_edit
                    });
                }

                //更新商品自定义价格
                DAL.AddRange<Tb_Sp_Cgs>(cgsList);
            }
            #endregion

            #region 更新商品标签
            if (model.applyallsku.Equals("1"))
            {
                //更新商品标签 
                ht.Clear();
                ht.Add("id_sp", model.id);
                ht.Add("id_gys", model.id_gys);
                DAL.Delete(typeof(Tb_Gys_Sp_Tag), ht);

                //重新插入所有标签
                IList<Tb_Gys_Sp_Tag> taglist = new List<Tb_Gys_Sp_Tag>();
                foreach (var tag in model.Tb_Gys_Tag)
                {
                    foreach (var sku in model.sku)
                    {
                        taglist.Add(new Tb_Gys_Sp_Tag
                        {
                            id_tag = tag.id_tag,
                            id_gys = model.id_gys,
                            id_sku = sku.id,
                            id_sp = model.id,
                            id_create = tag.id_create == 0 ? model.id_create : tag.id_create,
                            id_edit = model.id_edit,
                            rq_create = tag.rq_create,
                            rq_edit = model.rq_edit
                        });
                    }
                }

                //更新商品标签
                DAL.AddRange<Tb_Gys_Sp_Tag>(taglist);
            }
            else
            {
                //更新商品标签 
                ht.Clear();
                ht.Add("id_sp", model.id);
                ht.Add("id_gys", model.id_gys);
                ht.Add("id_sku", model.id_sku);
                DAL.Delete(typeof(Tb_Gys_Sp_Tag), ht);

                //重新插入所有标签
                IList<Tb_Gys_Sp_Tag> taglist = new List<Tb_Gys_Sp_Tag>();
                foreach (var tag in model.Tb_Gys_Tag)
                {
                    taglist.Add(new Tb_Gys_Sp_Tag
                    {
                        id_tag = tag.id_tag,
                        id_gys = model.id_gys,
                        id_sku = model.id_sku,
                        id_sp = model.id,
                        id_create = tag.id_create == 0 ? model.id_create : tag.id_create,
                        id_edit = model.id_edit,
                        rq_create = tag.rq_create,
                        rq_edit = model.rq_edit
                    });

                }

                //更新商品标签
                DAL.AddRange<Tb_Gys_Sp_Tag>(taglist);
            }
            #endregion

            SaveGoodsPic(model.pic, model.id, model.id_edit);

            br.Success = true;
            br.Message.Add("商品保存成功.");
            return br;
        }

        #region 导入图片
        /// <summary>
        /// 导入图片
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="imgList"></param>
        /// <returns></returns>
        [Transaction]
        public BaseResult UploadImg(dynamic entity, Dictionary<string, string> imgList)
        {
            BaseResult br = new BaseResult();
            var model = (Tb_Sp_Sku)entity;
            Hashtable ht = new Hashtable();
            ht.Add("id_gys", model.id_create);
            List<Tb_Gys_Sp> list_Gys_Sp = Tb_SpDAL.QueryList1(typeof(Tb_Gys_Sp), ht) as List<Tb_Gys_Sp>;
            if (list_Gys_Sp != null && list_Gys_Sp.Count > 0)
            {
                var gys_sp = list_Gys_Sp.Find(d => d.bm_Interface == model.bm);

                if (gys_sp != null && gys_sp.id_sku > 0)
                {
                    //更新主图
                    ht.Clear();
                    ht.Add("id", gys_sp.id_sku);
                    ht.Add("new_photo_min2", model.photo_min2);// 60x60
                    ht.Add("new_photo_min", model.photo_min);//200x200
                    ht.Add("new_photo", model.photo); //480x480
                    DAL.UpdatePart(typeof(Tb_Sp_Sku), ht);

                    //更新图册
                    int xh = 0;
                    List<Tb_Sp_Pic> list_pic = new List<Tb_Sp_Pic>();
                    ht.Clear();
                    ht.Add("id_sp", gys_sp.id_sp);
                    DAL.Delete(typeof(Tb_Sp_Pic), ht);
                    foreach (var item in imgList)
                    {
                        if (!string.IsNullOrEmpty(item.Key))
                        {
                            Tb_Sp_Pic model_pic = new Tb_Sp_Pic() { id_sp = gys_sp.id_sp, xh = Convert.ToByte(xh) };
                            model_pic.photo = item.Key;
                            model_pic.photo_min = item.Value;
                            model_pic.id_create = model.id_create;
                            model_pic.rq_create = DateTime.Now;
                            list_pic.Add(model_pic);
                            xh++;
                        }

                    }
                    if (list_pic.Count > 0)
                    {
                        DAL.AddRange(list_pic);
                    }
                    br.Success = true;
                    br.Message.Add("上传完成");
                }
                else
                {
                    br.Success = false;
                    br.Message.Add("系统不存在该商品");
                }
            }
            return br;
        }
        #endregion

        public override BaseResult GetCount(Hashtable param)
        {
            BaseResult br = new BaseResult() { Success = DAL.GetCount(typeof(Tb_Sp), param) == 0 ? true : false };
            return br;
        }

        /// <summary>
        /// 供应商 商品列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(Hashtable param)
        {
            bool isAnalysis = false;//是否使用分词查询，默认是否
            PageNavigate pn;
            List<SkuData> list = new List<SkuData>();
            if (param.ContainsKey("keyword")
                && !string.IsNullOrWhiteSpace(param["keyword"].ToString())
                && param.ContainsKey("id_cgs")
                && !string.IsNullOrWhiteSpace(param["id_cgs"].ToString())
                )
            {

                //查询是否该采购商是鼎配的客户
                var ht = new Hashtable();
                ht.Add("id_cgs", param["id_cgs"]);
                ht.Add("id_gys", 13674);
                ht.Add("flag_stop", 0);

                //如果是鼎配客户，使用分词查询。
                if (DAL.GetCount(typeof(Tb_Gys_Cgs), ht) > 0) isAnalysis = true;

            }
            if (isAnalysis)
            {
                pn = new PageNavigate() { TotalCount = Tb_SpDAL.QueryAnalysisCount(typeof(Tb_Gys_Sp), param) };
                if (pn.TotalCount > 0)
                {
                    list = Tb_SpDAL.QueryAnalysisPage(typeof(Tb_Gys_Sp), param).ToList();
                }
            }
            else
            {
                pn = new PageNavigate() { TotalCount = DAL.QueryCount(typeof(Tb_Gys_Sp), param) };
                if (pn.TotalCount > 0)
                {
                    list = DAL.QueryPage<SkuData>(typeof(Tb_Gys_Sp), param).ToList();
                }
            }

            if (list != null && list.Count > 0)
            {

                var mm_exp = new List<Tb_Sp_Expand_Query>();
                var mm_tag = new List<Tb_Gys_Sp_Tag_Query>();
                var temp_exp = new List<Tb_Sp_Expand_Query>();
                var temp_tag = new List<Tb_Gys_Sp_Tag_Query>();
                long[] mm_skuid = new long[list.Count];
                long[] mm_gysid = new long[list.Count];
                int all_number = list.Count;
                int select_number = 1000;
                int select_count = 1;
                int start_index = 0;
                double qz = list.Count / select_number;
                double qm = list.Count % select_number;
                if (qm > 0)
                {
                    qz += 1;
                }
                select_count = Convert.ToInt32(qz);
                for (int i = 1; i <= select_count; i++)
                {
                    if (select_count > 1)
                    {
                        if (i == 1)
                        {
                            start_index = 0;
                            all_number = 1000;
                        }
                        else
                        {
                            start_index = select_number * (i - 1) + 1;
                            if (all_number - select_number * i > 0)
                            {
                                all_number = select_number * i;
                            }
                            else if (all_number - select_number * i < 0)
                            {
                                all_number = list.Count;
                            }
                        }
                    }
                    mm_skuid = new long[list.Count];
                    mm_gysid = new long[list.Count];
                    for (int k = start_index; k < all_number; k++)
                    {
                        mm_gysid[k] = (long)list[k].id_gys;
                        mm_skuid[k] = (long)list[k].id;
                    }
                    mm_gysid = mm_gysid.Where(s => s > 0).ToArray();
                    mm_skuid = mm_skuid.Where(s => s > 0).ToArray();
                    if (mm_skuid != null && mm_skuid.Length > 0)
                    {
                        param.Clear();
                        param.Add("id_skuList", mm_skuid);
                        temp_exp = DAL.QueryList<Tb_Sp_Expand_Query>(typeof(Tb_Sp_Expand), param).ToList();
                        mm_exp.AddRange(temp_exp);
                        if (mm_gysid != null && mm_gysid.Length > 0)
                        {
                            param.Add("id_gysList", mm_gysid.Distinct().ToList<long>());
                            temp_tag = DAL.QueryList<Tb_Gys_Sp_Tag_Query>(typeof(Tb_Gys_Sp_Tag), param).ToList();
                            mm_tag.AddRange(temp_tag);
                        }
                    }
                }





                foreach (var item in list)
                {
                    if (mm_exp != null && mm_exp.Count > 0)
                    {
                        item.sp_expand_query = mm_exp.Where(m => m.id_sku.Equals(item.id)).ToList();
                        //组合【尺码2：300，重量2：30kg，颜色2：黑色】
                        foreach (var expand in item.sp_expand_query)
                        {
                            item.name_spec_zh += String.Format("{0}:{1},", expand.mc, expand.val);
                            item.name_spec_id += String.Format("{0}/", expand.id_sp_expand_template);
                        }
                    }
                    //设置商品标签
                    item.sp_tag_list = mm_tag.Where(m => m.id_sku.Equals(item.id) && m.id_gys.Equals(item.id_gys)).ToList();
                }

            }
            else
            {
                list = new List<SkuData>();
            }
            pn.Data = list;

            pn.Success = true;
            return pn;
        }
        public BaseResult ExportAllSp(Hashtable param)
        {
            BaseResult br = new BaseResult();
            List<SkuData> list = (List<SkuData>)Tb_Sp_SkuDAL.QueryListExport(typeof(Tb_Gys_Sp), param);
            br.Data = list;
            return br;
        }
        public PageNavigate GetOrderPage(Hashtable param)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = Tb_SpDAL.QueryCountOfOrder(typeof(Tb_Sp), param);
            if (pn.TotalCount > 0)
            {
                IList<Tb_Sp_Query> list = Tb_SpDAL.QueryPageOfOrder(typeof(Tb_Sp), param);
                param = new Hashtable();
                foreach (var item in list)
                {
                    param.Add("id_sku", item.id_sku);
                    item.sp_expand_query = DAL.QueryList<Tb_Sp_Expand_Query>(typeof(Tb_Sp_Expand), param).ToList();
                    foreach (var expand in item.sp_expand_query)
                    {
                        item.name_spec_zh += String.Format("{0}:{1},", expand.mc, expand.val);
                    }
                    param.Remove("id_sku");
                    param.Add("id_user_master", item.id_user_master_gys);
                    param.Add("bm", "sp_kc_flag");
                    param.Add("val", "1");
                    item.kc_flag = DAL.GetCount(typeof(Ts_Param_Business), param);
                    param.Clear();

                    //设置商品标签
                    param.Add("id_sku", item.id_sku);
                    param.Add("id_gys", item.id_gys);
                    item.sp_tag_list = DAL.QueryList<Tb_Gys_Sp_Tag_Query>(typeof(Tb_Gys_Sp_Tag), param);
                    param.Clear();
                }
                pn.Data = list;
            }
            else
            {
                pn.Data = new List<Tb_Sp_Query>();
            }
            pn.Success = true;
            return pn;
        }
        /// <summary>
        /// 商品删除
        /// tim 2015-04-13
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            if (param == null || param.Count == 0 || !param.ContainsKey("skuList") || !param.ContainsKey("id_gys"))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("商品删除参数错误.");
                return br;
            }

            string[] skuList = param["skuList"].ToString().Split(',');

            Hashtable ht = new Hashtable();
            foreach (var p in skuList)
            {
                ht.Clear();
                ht.Add("id_sku", p);
                ht.Add("id_gys", param["id_gys"]);
                var rs = DeleteOne(ht);
                if (!rs.Success) br.Message.Add(rs.Message[0]);
            }
            if (skuList.Length.Equals(br.Message.Count))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Insert(0, "商品删除不成功，因为以下原因：");
            }
            else if (br.Message.Count > 0)
            {
                br.Success = true;
                br.Level = ErrorLevel.Question;
                br.Message.Insert(0, "商品删除部分成功，未删除的商品存在以下原因：");
            }
            else
            {
                br.Success = true;
                br.Message.Add("商品删除成功.");
            }

            return br;
        }

        /// <summary>
        /// 商品删除单例模式
        /// tim 2015-04-13
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private BaseResult DeleteOne(Hashtable param)
        {
            BaseResult br = new BaseResult();
            if (param == null
                || param.Count == 0
                || !param.ContainsKey("id_sku")
                || string.IsNullOrWhiteSpace(param["id_sku"].ToString())
                || !param.ContainsKey("id_gys")
                )
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("删除商品参数错误.");
                return br;
            }

            string id_sku = param["id_sku"].ToString();
            string id_gys = param["id_gys"].ToString();


            Hashtable ht = new Hashtable();
            ht.Add("id_sku", id_sku);

            var sp = DAL.GetItem<Tb_Sp>(typeof(Tb_Sp), ht);

            if (sp == null)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("要删除的商品不存在,SKU_ID为:{0}", id_sku));
                return br;
            }
            br.Success = true;
            br.Message.Add("商品删除成功.");


            //创建者是本人
            if (sp.id_gys_create.ToString().Equals(id_gys))
            {
                ht.Clear();
                ht.Add("id_sku", id_sku);
                var db_count = DAL.GetCount(typeof(Td_Sale_Order_Body), ht);
                //商品未开单，删除SKU商品
                if (db_count.Equals(0))
                {
                    ht.Clear();
                    ht.Add("id", id_sku);
                    DAL.Delete(typeof(Tb_Sp_Sku), ht);

                    ht.Clear();
                    ht.Add("id_sp", sp.id);
                    if (DAL.GetCount(typeof(Tb_Sp_Sku), ht).Equals(0))//如果sku全部删除，则删掉商品
                    {
                        ht.Clear();
                        ht.Add("id", sp.id);
                        DAL.Delete(typeof(Tb_Sp), ht);
                    }
                }
                else
                {
                    br.Message.Clear();
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    br.Message.Add(string.Format("商品[{0}]已被使用,不可删除！", sp.mc));
                }
            }
            else
            {
                ht.Clear();
                ht.Add("id_sku", id_sku);
                ht.Add("id_gys", id_gys);
                var db_count = DAL.GetCount(typeof(Td_Sale_Order_Body), ht);

                if (db_count > 0)
                {
                    br.Message.Clear();
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    br.Message.Add(string.Format("商品[{0}]已开单,不可删除！", sp.mc));
                }
                else
                {
                    DAL.Delete(typeof(Tb_Gys_Sp), ht);//删除供应商引用商品
                    DAL.Delete(typeof(Tb_Sp_Dj), ht);//删除供应商单价
                }
            }
            return br;
        }

        /// <summary>
        /// 商品上架
        /// tim 2015-04-13
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Stop(Hashtable param)
        {
            BaseResult br = new BaseResult();
            if (param == null || param.Count == 0 || !param.ContainsKey("skuList") || !param.ContainsKey("flag") || !param.ContainsKey("id_gys"))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("商品参数错误.");
                return br;
            }
            br = UpdateFlag(param);
            br.Message.Add(string.Format("商品{0}成功.", param["flag"].ToString().Equals("0") ? "下架" : "上架"));
            return br;
        }
        /// <summary>
        /// 商品下架
        /// tim 2015-04-13
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Active(Hashtable param)
        {

            BaseResult br = new BaseResult();
            if (param == null || param.Count == 0 || !param.ContainsKey("skuList") || !param.ContainsKey("id_gys"))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("下架商品参数错误.");
                return br;
            }
            br = UpdateFlag(param);
            br.Message.Add("商品下架成功.");
            return br;
        }

        /// <summary>
        ///  跟新 sku 
        ///  znt 2015-03-12
        /// </summary>

        public BaseResult UpdateSku(Tb_Sp_Sku model)
        {
            BaseResult br = new BaseResult();
            return br;
        }


        /// <summary>
        ///  禁用/启用 供应商 商品 
        ///  znt 2015-03-12
        /// </summary>
        private BaseResult UpdateFlag(Hashtable param)
        {
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            string[] skuList = param["skuList"].ToString().Split(',');
            string id_gys = param["id_gys"].ToString();
            ht.Add("id_skuList", skuList);
            ht.Add("id_gys", id_gys);
            ht.Add("new_flag_up", param["flag"]);
            ht.Add("new_rq_edit", DateTime.Now);
            if (param.ContainsKey("id_edit")) ht.Add("new_id_edit", param["id_edit"]);
            DAL.UpdatePart(typeof(Tb_Gys_Sp), ht);
            br.Success = true;
            return br;
        }

        public PageNavigate GetPageSkn(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = Tb_SpDAL.QueryCountOfService(typeof(Tb_Sp), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = Tb_SpDAL.QueryPageOfService(typeof(Tb_Sp), param);
                //if (param.ContainsKey("flag_search"))
                //{
                //    param.Remove("flag_search");
                //    pn.Data = Tb_SpDAL.QueryPageOfServiceForSearch(typeof(Tb_Sp), param);
                //}
                //else {
                //    pn.Data = Tb_SpDAL.QueryPageOfService(typeof(Tb_Sp), param);
                //}
            }
            else
            {
                pn.Data = new List<Tb_Sp_Query>();
            }
            pn.Success = true;
            return pn;
        }

        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string id = param.ContainsKey("id") ? param["id"].ToString() : string.Empty;
            string mc = param.ContainsKey("mc") ? param["mc"].ToString() : string.Empty;
            long id_gys = Convert.ToInt64(param["id_gys"]);
            long id_cgs = Convert.ToInt64(param["id_cgs"] ?? 0);
            object id_gys_create = param["id_gys_create"];
            object baseurl = param["baseurl"];
            var model = new Tb_Sp_Get();

            Hashtable ht = new Hashtable();
            if (param.ContainsKey("id")) ht.Add("id", param["id"]);
            if (param.ContainsKey("mc")) ht.Add("mc", param["mc"]);
            ht.Add("id_gys_create", id_gys_create);
            var sp = DAL.GetItem<Tb_Sp>(typeof(Tb_Sp), ht);
            if (sp == null)
            {
                br.Success = false;
                br.Message.Add("商品不存在.");
                return br;
            }
            model.id = sp.id.Value;
            model.mc = sp.mc;
            model.brand = sp.brand;
            model.cd = sp.cd;
            model.id_gys_create = sp.id_gys_create.GetValueOrDefault(0);

            ht.Clear();
            ht.Add("id_sp", sp.id);
            ht.Add("id_gys", param["id_gys"]);
            if (param.ContainsKey("baseurl") && !string.IsNullOrWhiteSpace(param["baseurl"].ToString())) ht.Add("baseurl", param["baseurl"]);
            if (param.ContainsKey("id_user")) ht.Add("id_user", param["id_user"]);
            if (id_cgs > 0)
            {
                ht.Add("id_cgs", id_cgs);
                // ht.Add("flag_up", YesNoFlag.Yes);
                ht.Add("flag_stop", YesNoFlag.No);
            }
            else
            {
                model.levelPriceList = Tb_Sp_DjDAL.QueryList1(typeof(Tb_Sp_Dj), ht);
            }
            model.skuList = Tb_Sp_SkuDAL.QueryList1(typeof(Tb_Sp_Sku), ht);

            //获取自定义客户价格列表
            ht.Clear();
            ht.Add("id_gys", param["id_gys"]);
            ht.Add("id_sp", sp.id);
            if (param.ContainsKey("id_sku")) { ht.Add("id_sku", param["id_sku"]); }
            model.CustomerPriceList = DAL.QueryList<Tb_Sp_Cgs_Query>(typeof(Tb_Sp_Cgs), ht);

            //获取sku设置的对应标签
            ht.Clear();
            ht.Add("id_gys", param["id_gys"]);
            ht.Add("id_sp", sp.id);
            var goodsskuTagList = DAL.QueryList<Tb_Gys_Sp_Tag_Query>(typeof(Tb_Gys_Sp_Tag), ht);

            ht.Clear();
            ht.Add("id_sp", sp.id);
            var ggList = DAL.QueryList<Tb_Sp_Expand>(typeof(Tb_Sp_Expand), ht);
            foreach (var item in model.skuList)
            {
                item.specList = (from gg in ggList where gg.id_sku == item.id_sku select new Tb_Sp_Sku_Spec(gg.id_sp_expand_template.Value, gg.val)).ToList();

                item.GoodsSkuTagList = goodsskuTagList.Where(d => d.id_sku == item.id_sku).ToList();
            }

            if (ggList != null && ggList.Count > 0)
            {
                var id_sp_expand_templateList = (from item in ggList group item by item.id_sp_expand_template.Value into g select g.Key).ToList();
                param.Clear();
                param.Add("idList", id_sp_expand_templateList);
                var sp_expand_templateList = DAL.QueryList<Tb_Sp_Expand_Template>(typeof(Tb_Sp_Expand_Template), param);


                var sp_expandGroupList = (from item in ggList group item by new { id_sp_expand_template = item.id_sp_expand_template, val = item.val } into g select g.Key).ToList();
                foreach (var item in sp_expand_templateList)
                {
                    var items = (from sp_expandGroup in sp_expandGroupList where sp_expandGroup.id_sp_expand_template == item.id select sp_expandGroup.val).ToList();
                    model.specGroupList.Add(new Tb_Sp_SpecGroup(item.id.Value, item.mc, items));
                }
            }
            model.picList = DAL.QueryList<Tb_Sp_Pic>(typeof(Tb_Sp_Pic), ht);

            if (model.specGroupList.Count > 0)
            {
                ht.Clear();
                ht.Add("id_sun", model.specGroupList[0].id);
                model.specGroup = DAL.GetItem<Tb_Sp_Expand_Template>(typeof(Tb_Sp_Expand_Template), ht);
            }

            br.Data = model;
            br.Success = true;
            return br;
        }
        public BaseResult GetPicList(List<SkuData> list, long id_gys)
        {
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            if (list != null && list.Count > 0)
            {
                var dj = new List<Tb_Sp_Dj_Query>();
                var temp_dj = new List<Tb_Sp_Dj_Query>();
                long[] array_spid = new long[list.Count];
                int all_number = list.Count;
                int select_number = 1000;
                int select_count = 1;
                int start_index = 0;
                double qz = list.Count / select_number;
                double qm = list.Count % select_number;
                if (qm > 0)
                {
                    qz += 1;
                }
                select_count = Convert.ToInt32(qz);
                for (int i = 1; i <= select_count; i++)
                {
                    if (select_count > 1)
                    {
                        if (i == 1)
                        {
                            start_index = 0;
                            all_number = 1000;
                        }
                        else
                        {
                            start_index = select_number * (i - 1) + 1;
                            if (all_number - select_number * i > 0)
                            {
                                all_number = select_number * i;
                            }
                            else if (all_number - select_number * i < 0)
                            {
                                all_number = list.Count;
                            }
                        }
                    }
                    array_spid = new long[list.Count];
                    for (int k = start_index; k < all_number; k++)
                    {
                        array_spid[k] = (long)list[k].id_gys;
                    }
                    array_spid = array_spid.Where(s => s > 0).ToArray();
                    if (array_spid != null && array_spid.Length > 0)
                    {
                        ht.Clear();
                        ht.Add("id_gys", id_gys);
                        ht.Add("id_spList", array_spid.Distinct().ToList<long>());
                        temp_dj = (List<Tb_Sp_Dj_Query>)Tb_Sp_DjDAL.QueryList1(typeof(Tb_Sp_Dj), ht);
                        dj.AddRange(temp_dj);
                    }
                }
                br.Data = dj;
            }
            return br;
        }
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<SkuData>(typeof(Tb_Gys_Sp), param) ?? new List<SkuData>();
            IList<SkuData> list = DAL.QueryList<SkuData>(typeof(Tb_Gys_Sp), param) ?? new List<SkuData>();

            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    param.Add("id_sku", item.id);
                    item.sp_expand_query = DAL.QueryList<Tb_Sp_Expand_Query>(typeof(Tb_Sp_Expand), param).ToList();

                    //组合【尺码2：300，重量2：30kg，颜色2：黑色】
                    foreach (var expand in item.sp_expand_query)
                    {
                        item.name_spec_zh += String.Format("{0}:{1},", expand.mc, expand.val);
                    }
                    if (item.name_spec_zh.Length > 0)
                    {
                        item.name_spec_zh = item.name_spec_zh.Substring(0, item.name_spec_zh.Length - 1);
                    }
                    param.Remove("id_sku");

                }
            }

            br.Data = list;
            br.Success = true;
            return br;
        }
        /// <summary>
        /// 导出所有商品库存
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<SkuData> GetExportAll(Hashtable param = null)
        {
            List<SkuData> list = new List<SkuData>(DAL.ExportAll<SkuData>(typeof(Tb_Gys_Sp), param) ?? new List<SkuData>());
            param = new Hashtable();
            foreach (var item in list)
            {
                param.Add("id_sku", item.id);
                item.sp_expand_query = DAL.QueryList<Tb_Sp_Expand_Query>(typeof(Tb_Sp_Expand), param).ToList();
                foreach (var expand in item.sp_expand_query)
                {
                    item.name_spec_zh += String.Format("{0}:{1},", expand.mc, expand.val);
                }
                param.Remove("id_sku");
            }
            return list;
        }
        /// <summary>
        /// 检查商品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseResult CheckGood(Hashtable param)
        {
            BaseResult br = new BaseResult();
            if (param == null || param.Count == 0)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("检查商品参数错误.");
                return br;
            }
            var sp = DAL.GetItem<Tb_Gys_Sp>(typeof(Tb_Gys_Sp), param);
            if (sp == null)
            {
                br.Success = false;
                return br;
            }
            br.Success = true;
            return br;
        }
        /// <summary>
        /// 查询单个供应商商品
        /// </summary>
        /// <param name="param">id_sp/id_gys</param>
        /// <returns></returns>
        public BaseResult Get_Gys_sp(Hashtable param)
        {
            BaseResult br = new BaseResult();
            //获取供应商-商品资料
            Hashtable ht = new Hashtable();
            ht.Clear();
            ht.Add("id_sku", param["id_sku"]);
            ht.Add("id_gys", param["id_gys"]);
            Tb_Gys_Sp sp = DAL.GetItem<Tb_Gys_Sp>(typeof(Tb_Gys_Sp), ht);
            var spc = new Tb_Gys_Sp_Query();
            if (sp != null)
            {
                spc.bm_Interface = sp.bm_Interface;
                spc.dj_base = sp.dj_base;
                spc.flag_stop = sp.flag_stop;
                spc.flag_up = sp.flag_up;
                spc.id_create = sp.id_create;
                spc.id_edit = sp.id_edit;
                spc.id_gys = sp.id_gys;
                spc.id_sku = sp.id_sku;
                spc.id_sp = sp.id_sp;
                spc.id_spfl = sp.id_spfl;
                spc.rq_create = sp.rq_create;
                spc.rq_edit = sp.rq_edit;
                spc.sl_kc = sp.sl_kc;
                spc.sl_kc_bj = sp.sl_kc_bj;
                spc.sort_id = sp.sort_id;
                spc.zhl = sp.zhl;

                //获取商品名称
                Hashtable ht2 = new Hashtable();
                ht2.Clear();
                ht2.Add("id", sp.id_sp);
                //ht2.Add("id_gys_create", param["id_gys"]);
                var _sp = DAL.GetItem<Tb_Sp>(typeof(Tb_Sp), ht2);
                //获取名称
                spc.mc = _sp.mc;
                spc.mc_zh = _sp.mc;
                ht2.Clear();
                ht2.Add("id_sku", param["id_sku"]);
                var ls = DAL.QueryList<Tb_Sp_Expand_Query>(typeof(Tb_Sp_Expand), ht2).ToList();
                //组合【尺码2：300，重量2：30kg，颜色2：黑色】
                foreach (var expand in ls)
                {
                    spc.mc_zh += String.Format("{0}:{1},", expand.mc, expand.val);
                }
                br.Data = spc;
            }
            br.Success = true;
            return br;
        }
        /// <summary>
        /// 获取所有商品介绍
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseResult GetInfoAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            List<Tb_Sp_Info> list = new List<Tb_Sp_Info>();
            list = Tb_SpDAL.QueryInfoAll(typeof(Tb_Sp_Info), param) as List<Tb_Sp_Info>;
            //param.Clear();
            //param.Add("id_sku", 0);
            //foreach (Tb_Sp_Info item in list)
            //{
            //    param["id_sku"] = item.id_sku;
            //    var ls = DAL.QueryList<Tb_Sp_Expand_Query>(typeof(Tb_Sp_Expand), param).ToList();
            //    foreach (var expand in ls)
            //    {
            //        item.name_spec_zh += String.Format("【{0}:{1}】", expand.mc, expand.val);
            //    }
            //}
            br.Data = list;
            br.Success = true;
            return br;
        }
        /// <summary>
        /// 添加/更新商品介绍
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public BaseResult AddOfUpInfo(Tb_Sp_Info info)
        {

            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            param["id_sp"] = info.id_sp;
            param["id_sku"] = info.id_sku;
            if (DAL.GetCount(typeof(Tb_Sp_Info), param) > 0)
            {
                param["new_description"] = info.description;
                DAL.UpdatePart(typeof(Tb_Sp_Info), param);
            }
            else
            {
                DAL.Add<Tb_Sp_Info>(info);
            }
            br.Success = true;
            return br;
        }
        public BaseResult UpSp(out int success, out int fail, out DataTable table, Hashtable ht, IList<Tb_Cgs_Level> mTb_Cgs_Level, string savePath)
        {
            BaseResult br = new BaseResult();
            success = 0;
            fail = 0;
            table = null;
            //if (!table.Columns.Contains("备注"))
            //    table.Columns.Add("备注", typeof(System.String));
            //if (table != null && table.Rows.Count > 0)
            //{
            //    long id_gys = (long)ht["id_gys"];
            //    long id_user = (long)ht["id_user"];
            //    string str = "";
            //    Tb_Gys_Sp gyssp = new Tb_Gys_Sp();
            //    Tb_Spfl fl = new Tb_Spfl();
            //    Tb_Sp_Info info = new Tb_Sp_Info();
            //    Tb_Sp_Dj dj = new Tb_Sp_Dj();
            //    Hashtable param = new Hashtable();
            //    long id_sp = 0;
            //    long id_sku = 0;
            //    try
            //    {
            //        foreach (DataRow item in table.Rows)
            //        {
            //            id_sp = item["商品id"] != null ? Convert.ToInt64(item["商品id"]) : 0;
            //            id_sku = item["商品sku"] != null ? Convert.ToInt64(item["商品sku"]) : 0;
            //            //检查商品是否存在
            //            param.Clear();
            //            param["id_sku"] = id_sku;
            //            param["id_gys"] = id_gys;
            //            param["id_sp"] = id_sp;
            //            try
            //            {
            //                gyssp = DAL.GetItem<Tb_Gys_Sp>(typeof(Tb_Gys_Sp), param);
            //            }
            //            catch (CySoftException ex)
            //            {

            //                throw new CySoftException("商品sku" + id_sku + ex.Message);
            //            }
            //            if (gyssp != null)
            //            {
            //                int fd = DAL.GetCount(typeof(Tb_sp_sku_fd), param);
            //                gyssp.dj_base = Convert.ToDecimal(item["市场价"]);
            //                if (item["状态"].ToString() == "上架")
            //                {
            //                    gyssp.flag_up = YesNoFlag.Yes;
            //                }
            //                else
            //                {
            //                    gyssp.flag_up = YesNoFlag.No;
            //                }
            //                gyssp.id_edit = id_gys;
            //                gyssp.rq_edit = DateTime.Now;
            //                param.Clear();
            //                param["id_gys"] = id_gys;
            //                param["name"] = item["商品分类"].ToString();
            //                fl = DAL.GetItem<Tb_Spfl>(typeof(Tb_Spfl), param);
            //                if (fl != null)
            //                {
            //                    param["id_gys"] = id_gys;
            //                    param["id_sku"] = id_sku;
            //                    param["id_sp"] = id_sp;
            //                    param["new_id_spfl"] = fl.id;
            //                    gyssp.id_spfl = fl.id;
            //                }
            //                else
            //                {
            //                    fail++;
            //                    success--;
            //                    item["备注"] = "商品类别更新失败，" + item["商品分类"].ToString() + "不存在。";
            //                }
            //                int gysspUp = DAL.Update<Tb_Gys_Sp>(gyssp);
            //                param.Clear();
            //                param["id"] = id_sp;
            //                param["new_mc"] = item["商品名称"].ToString();
            //                param["new_id_edit"] = id_gys;
            //                param["new_rq_edit"] = DateTime.Now;
            //                DAL.UpdatePart(typeof(Tb_Sp), param);
            //                param.Clear();
            //                int index = 0;
            //                str = item["规格模板id"].ToString();
            //                if (!string.IsNullOrEmpty(str))
            //                {
            //                    string[] name_spec = item["多规格字段设置"].ToString().Split('/');

            //                    if (str.Substring(str.Length - 1, 1) == "/")
            //                    {
            //                        str = str.Substring(0, str.Length - 1);
            //                    }
            //                    string[] id_spec = str.Split('/');
            //                    for (int i = 0; i < id_spec.Length; i++)
            //                    {
            //                        param["id"] = id_spec[i];
            //                        param["new_mc"] = name_spec[i];
            //                        DAL.UpdatePart(typeof(Tb_Sp_Expand_Template), param);
            //                        param.Clear();
            //                        param["id_sku"] = id_sku;
            //                        param["id_sp_expand_template"] = id_spec[i];
            //                        index = i + 1;
            //                        param["new_val"] = item["规格" + index + "内容"].ToString();
            //                        DAL.UpdatePart(typeof(Tb_Sp_Expand), param);
            //                    }
            //                }

            //                param.Clear();
            //                if (fd <= 0)
            //                {
            //                    param["id"] = id_sku;
            //                    param["new_unit"] = item["计量单位"].ToString();
            //                    param["new_barcode"] = item["条形码"].ToString();
            //                    param["new_id_edit"] = id_gys;
            //                    param["new_rq_edit"] = DateTime.Now;
            //                    DAL.UpdatePart(typeof(Tb_Sp_Sku), param);
            //                }
            //                if (!string.IsNullOrEmpty(item["商品标签（注：多个以中文逗号区分）"].ToString()))
            //                {
            //                    param.Clear();
            //                    string[] tagList = item["商品标签（注：多个以中文逗号区分）"].ToString().Replace('，', ',').Split(',');
            //                    br = AddGysSpTags(id_sku, id_gys, id_sp, id_user, tagList);
            //                    if (!br.Success)
            //                    {
            //                        fail++;
            //                        if (br.Message != null && br.Message.Count > 0)
            //                        {
            //                            item["备注"] = br.Message[0];
            //                        }
            //                    }
            //                }
            //                if (mTb_Cgs_Level != null && mTb_Cgs_Level.Count > 0)
            //                {
            //                    foreach (Tb_Cgs_Level le in mTb_Cgs_Level)
            //                    {
            //                        if (!string.IsNullOrEmpty(item[le.name + "起订量"].ToString()) || !string.IsNullOrEmpty(item[le.name + "订货价"].ToString()))
            //                        {
            //                            param.Clear();
            //                            param["id_gys"] = id_gys;
            //                            param["id_sku"] = id_sku;
            //                            param["id_cgs_level"] = le.id;
            //                            param["new_sl_dh_min"] = item[le.name + "起订量"].ToString();
            //                            param["new_dj_dh"] = item[le.name + "订货价"].ToString();
            //                            param["new_id_edit"] = id_gys;
            //                            param["new_rq_edit"] = DateTime.Now;
            //                            if (DAL.GetCount(typeof(Tb_Sp_Dj), param) > 0)
            //                            {
            //                                DAL.UpdatePart(typeof(Tb_Sp_Dj), param);
            //                            }
            //                            else
            //                            {
            //                                dj.id_gys = id_gys;
            //                                dj.id_sku = id_sku;
            //                                dj.id_cgs_level = le.id;
            //                                dj.id_sp = id_sp;
            //                                if (!string.IsNullOrEmpty(item[le.name + "起订量"].ToString()))
            //                                    dj.sl_dh_min = Convert.ToDecimal(item[le.name + "起订量"]);
            //                                if (!string.IsNullOrEmpty(item[le.name + "订货价"].ToString()))
            //                                    dj.dj_dh = Convert.ToDecimal(item[le.name + "订货价"]);
            //                                dj.id_create = id_gys;
            //                                dj.rq_create = DateTime.Now;
            //                                dj.id_edit = id_gys;
            //                                dj.rq_edit = DateTime.Now;
            //                                DAL.Add<Tb_Sp_Dj>(dj);

            //                            }
            //                        }
            //                    }
            //                }
            //                if (fd <= 0)
            //                {
            //                    info.id_sku = id_sku;
            //                    info.id_sp = id_sp;
            //                    info.description = item["商品介绍"].ToString();
            //                    param.Clear();
            //                    param["id_sku"] = id_sku;
            //                    param["id_sp"] = id_sp;
            //                    AddOfUpInfo(info);
            //                }
            //                success++;
            //            }
            //            else
            //            {
            //                fail++;
            //                item["备注"] = "该商品不存在";
            //            }

            //        }
            //    }
            //    catch (CySoftException ex)
            //    {
            //        throw ex;
            //    }

            //}
            //else
            //{
            //    fail++;
            //}

            return br;
        }
        /// <summary>
        /// 查 单个商品单价
        /// </summary>
        /// <param name="id_gys">货品所有者-供应商ID</param>
        /// <param name="level">下单者-供应商关系-等级</param>
        /// <param name="id_sku">商品sku</param>
        /// <returns></returns>
        public Tb_Sp_Dj Get_Sp_dj(long? id_gys, long? level, long? id_sku)
        {
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("id_gys", id_gys);
            param.Add("id_cgs_level", level);
            param.Add("id_sku", id_sku);
            return DAL.GetItem<Tb_Sp_Dj>(typeof(Tb_Sp_Dj), param);
        }

        /// <summary>
        /// 判断是否存在商品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool IsExistsGoods(Hashtable param)
        {
            return DAL.GetCount(typeof(Tb_Gys_Sp), param) > 0;
        }
        /// <summary>
        /// 获取条形码图片信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public IList<Tb_Barcode_Pic> GetBarcodePic(string barCode)
        {
            Hashtable param = new Hashtable();
            param.Add("barcode", barCode);
            return DAL.QueryList<Tb_Barcode_Pic>(typeof(Tb_Barcode_Pic), param);
        }

        /// <summary>
        /// 添加 商品标签
        /// </summary>
        /// <param name="id_sku"></param>
        /// <param name="id_gys"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        private BaseResult AddGysSpTags(long id_sku, long id_gys, long id_sp, long id_create, string[] tags)
        {
            //部分成功 也为false 
            var br = new BaseResult();
            var param = new Hashtable();
            br.Success = true;
            if (tags == null || tags.Length < 0)
            {
                br.Success = true;
                return br;
            }
            //过滤重复
            var disTags = tags.Where(d => !string.IsNullOrEmpty(d)).Distinct().ToList();
            param.Add("id_sku", id_sku);
            param.Add("id_gys", id_gys);
            param.Add("id_sp", id_sp);
            DAL.Delete(typeof(Tb_Gys_Sp_Tag), param);
            param.Clear();
            //查询该供应商所有标签
            param.Add("id_gys", id_gys);
            var sysTags = DAL.QueryList<Tb_Gys_Tag>(typeof(Tb_Gys_Tag), param);

            if (sysTags == null || sysTags.Count <= 0)
            {
                br.Success = false;
                br.Message.Add("当前无已设置的商品标签！");
                return br;
            }

            //查询当前商品 已贴标签
            param.Clear();
            param.Add("id_gys", id_gys);
            param.Add("id_sku", id_sku);
            var spTags = DAL.QueryList<Tb_Gys_Sp_Tag_Query>(typeof(Tb_Gys_Sp_Tag), param);

            //查出存在于系统标签 的 标签
            var inSysTags = disTags.Where(d => sysTags.Where(d2 => d2.mc == d).ToList().Count > 0);

            //查出不存在于系统中的标签项 设置 msg
            var notInSysTags = disTags.Where(d => sysTags.Where(d2 => d2.mc == d).ToList().Count <= 0).ToList();
            if (notInSysTags.Count > 0)
            {
                br.Message.Add(string.Format("系统中不存在标签:{0}!", string.Join(",", notInSysTags.Select(d => string.Format("【{0}】", d)))));
                br.Success = false;
            }

            //将存在于系统的标签转换为 id_tag列表
            var id_tag_list = sysTags.Where(d => inSysTags.Contains(d.mc)).Select(d => d.id_tag);

            var now = DateTime.Now;
            //获取还没设置的标签列表
            var newTags = id_tag_list.Where(d => spTags.Where(d2 => d2.id_tag == d.Value).ToList().Count <= 0).Select(d => new Tb_Gys_Sp_Tag { id_tag = d.Value, id_sku = id_sku, id_gys = id_gys, id_sp = id_sp, id_create = id_create, id_edit = id_create, rq_create = now, rq_edit = now }).ToList();

            //插入新标签
            DAL.AddRange<Tb_Gys_Sp_Tag>(newTags);

            return br;
        }
    }
}
