using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.IBLL;
using CySoft.Frame.Attributes;
using CySoft.Model.Flags;

namespace CySoft.BLL.GoodsBLL
{
    public class GoodsRecommendBll : BaseBLL, IGoodsRecommendBll
    {

        public override PageNavigate GetPage(Hashtable param)
        {
            PageNavigate pn = new PageNavigate() { TotalCount = DAL.QueryCount(typeof(Tb_Sp_Sku_Push), param) };

            if (pn.TotalCount > 0)
            {
                List<SpSkuPushData> list = DAL.QueryPage<SpSkuPushData>(typeof(Tb_Sp_Sku_Push), param).ToList();

                //param = new Hashtable();
                //foreach (var item in list)
                //{


                //    param.Add("id_sku", item.id);
                //    item.sp_expand_query = DAL.QueryList<Tb_Sp_Expand_Query>(typeof(Tb_Sp_Expand), param).ToList();

                //    //组合【尺码2：300，重量2：30kg，颜色2：黑色】
                //    foreach (var expand in item.sp_expand_query)
                //    {
                //        item.name_spec_zh += String.Format("{0}:{1},", expand.mc, expand.val);
                //    }
                //    param.Remove("id_sku");

                //}

                pn.Data = list;
            }
            else
            {
                pn.Data = new List<SpSkuPushData>();
            }

            pn.Success = true;
            return pn;
        }
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();

            IList<Tb_Sp_Sku_Push> list = (List<Tb_Sp_Sku_Push>)entity;

            var param = new Hashtable();

            param.Add("id_sku_List", list.GroupBy(d => d.id_sku).Select(d => d.Key).ToArray());
            param.Add("id_gys_List", list.GroupBy(d => d.id_gys).Select(d => d.Key).ToArray());

            var list2 = DAL.QueryList<Tb_Sp_Sku_Push>(typeof(Tb_Sp_Sku_Push), param);

            //判断已推荐过的就不重复推荐
            if (list2 != null && list2.Count > 0)
            {
                list = list.Where(d => !list2.Select(e => new { id_sku = e.id_sku, id_gys = e.id_gys }).Contains(new { id_sku = d.id_sku, id_gys = d.id_gys })).ToList<Tb_Sp_Sku_Push>();
            }

            DAL.AddRange(list);

            br.Message.Add("推荐商品成功！");
            br.Success = true;
            return br;
        }
        //审核通过
        [Transaction]
        public BaseResult Pass(Tb_Sp_Sku_Push info)
        {
            BaseResult br = new BaseResult();
            var param = new Hashtable();
            var now = DateTime.Now;

            param.Add("id", info.id);
            var pushInfo = DAL.GetItem<SpSkuPushData>(typeof(Tb_Sp_Sku_Push), param);

            if (pushInfo == null)
            {
                br.Message.Add("推荐商品信息丢失！");
                br.Success = false;
                return br;
            }

            //判断输入的商品编码是否重复
            param.Clear();
            param.Add("id_gys", pushInfo.id_gys);
            param.Add("bm_Interface", info.bm_Interface);
            if (DAL.GetCount(typeof(Tb_Gys_Sp), param) > 0)
            {
                br.Success = false;
                br.Data = "bm";
                br.Message.Add(string.Format("商品编码[{0}]重复,请重新输入商品编码", info.bm_Interface));
                throw new CySoftException(br);
            }

            //判断当前商品是否已在分单表中
            param.Clear();
            param.Add("id_gys_fd", pushInfo.id_gys_push);
            param.Add("id_sku", pushInfo.id_sku);
            var spFdInfo = DAL.GetCount(typeof(Tb_sp_sku_fd), param);
            if (spFdInfo >0)
            {
                br.Message.Add("该商品已被推荐或应用，请选择其它商品审核！");
                br.Success = false;
                return br;
            }

            param.Clear();
            param.Add("id_gys", pushInfo.id_gys);
            param.Add("id_sku", pushInfo.id_sku);
            //获取当前商品信息 判断是否已存在该供应商的列表中
            var spInfo = DAL.GetCount(typeof(Tb_Gys_Sp), param);
            if (spInfo >0)
            {
                br.Message.Add("该商品已存在列表！");
                br.Success = false;
                return br;
            }

            //将商品存为自己的商品

            DAL.Add<Tb_Gys_Sp>(new Tb_Gys_Sp()
            {
                id_gys = pushInfo.id_gys,
                id_sku = pushInfo.id_sku,
                id_sp = pushInfo.id_sp,
                id_spfl = info.id_spfl,
                flag_stop = 0,
                bm_Interface = info.bm_Interface,
                dj_base = info.dj_dh,
                sort_id = 0,
                zhl = pushInfo.zhl,
                flag_up = YesNoFlag.Yes,
                sl_kc = 0,
                sl_kc_bj = 0,
                id_create = info.id_sh,
                rq_create = now,
                id_edit = info.id_sh,
                rq_edit = now
            });

            param.Clear();
            param.Add("id_gys", pushInfo.id_gys);
            //插入新的定价
            var levelList = DAL.QueryList<Tb_Cgs_Level>(typeof(Tb_Cgs_Level), param);

            var tb_sp_dj_list = levelList.Select(d => new Tb_Sp_Dj
            {
                id_gys = pushInfo.id_gys,
                id_sku = pushInfo.id_sku,
                id_cgs_level = d.id,
                id_sp = pushInfo.id_sp,
                sl_dh_min = info.sl_dh_min,
                dj_dh = info.dj_dh * d.agio / 100,
                id_create = info.id_sh,
                rq_create = now,
                id_edit = info.id_sh,
                rq_edit = now
            }).ToList();

            DAL.AddRange<Tb_Sp_Dj>(tb_sp_dj_list);

            //分单表插入关系
            DAL.Add<Tb_sp_sku_fd>(new Tb_sp_sku_fd()
            {
                id = Guid.NewGuid(),
                id_sku = pushInfo.id_sku,
                id_gys_fd = pushInfo.id_gys_push,
                id_gys = pushInfo.id_gys,
                rq_create = now
            });

            //更新推荐商品的信息
            param.Clear();
            param.Add("id", info.id);
            param.Add("new_id_spfl", info.id_spfl);
            param.Add("new_bm_Interface", info.bm_Interface);
            param.Add("new_sl_dh_min", info.sl_dh_min);
            param.Add("new_dj_dh", info.dj_dh);
            param.Add("new_id_sh", info.id_sh);
            param.Add("new_rq_sh", now);
            param.Add("new_falg_state", info.falg_state);

            DAL.UpdatePart(typeof(Tb_Sp_Sku_Push), param);

            br.Message.Add("商品已审核【通过】成功！");
            br.Success = true;
            return br;
        }
        //审核不通过
        public BaseResult NoPass(Tb_Sp_Sku_Push info)
        {
            BaseResult br = new BaseResult();
            var param = new Hashtable();
            param.Add("id", info.id);
            param.Add("new_refusereason", info.refusereason);
            param.Add("new_id_sh", info.id_sh);
            param.Add("new_rq_sh", DateTime.Now);
            param.Add("new_falg_state", info.falg_state);

            DAL.UpdatePart(typeof(Tb_Sp_Sku_Push), param);

            br.Message.Add("商品已审核【不通过】成功！");
            br.Success = true;
            return br;
        }
        //作废
        public BaseResult Invalid(Tb_Sp_Sku_Push info)
        {
            BaseResult br = new BaseResult();
            var param = new Hashtable();
            param.Add("id", info.id);
            param.Add("new_falg_state", info.falg_state);
            param.Add("new_id_sh", info.id_sh);
            param.Add("new_rq_sh", DateTime.Now);

            DAL.UpdatePart(typeof(Tb_Sp_Sku_Push), param);

            br.Message.Add("商品已【作废】成功！");
            br.Success = true;
            return br;
        }
    }
}
