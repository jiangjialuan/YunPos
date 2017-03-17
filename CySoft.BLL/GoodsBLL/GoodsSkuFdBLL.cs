using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CySoft.BLL.Base;
using CySoft.BLL.SaleBLL;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.IDAL;
using CySoft.Model.Flags;
using CySoft.Model.Tb;
using CySoft.Model.Td;
using CySoft.Model.Ts;
using CySoft.Utility;

namespace CySoft.BLL.GoodsBLL
{
    public class GoodsSkuFdBLL : BaseBLL, IGoodsSkuFdBLL
    {
        ITb_Gys_DAL Tb_Gys_DAL { get; set; }
       public IInfo_UserDAL Info_UserDAL { get; set; }

        /// <summary>
        /// 检查SKU分单商品 是否 已存在关联
        /// </summary>
        /// <param name="id_gys_fd">当前用户的 供应商编码</param>
        /// <param name="id_sku">商品的sku编码</param>
        /// <returns>true/false 存在/不存在</returns>
        public bool Check_Sdk_Fd(int id_gys_fd, int id_sku)
        {
            Hashtable param = new Hashtable();
            param.Add("id_gys_fd", id_gys_fd);
            param.Add("id_sku", id_sku);
            bool br;
            //判断是否存在重复 id_sku
            br = DAL.GetCount(typeof(Tb_sp_sku_fd), param) > 0;
            return br;
        }

        /// <summary>
        /// 检查订单是否存在 分单商品【列表】 
        /// </summary>
        /// <param name="id_skuList">sku商品列表</param>
        /// <param name="id_gys_fd">分单者 的 供应商编码</param>
        /// <returns>包含不需要分单与分单商品的数据 (本地商品id_gys=0)</returns>
        private List<Tb_sp_sku_fd> Check_Goods_Fd_List(List<long?> id_skuList, long id_gys_fd)
        {
            var br = new List<Tb_sp_sku_fd>();

            Hashtable param = new Hashtable();
            foreach (var id_sku in id_skuList)
            {
                param.Clear();
                param.Add("id_sku", id_sku);
                param.Add("id_gys_fd", id_gys_fd);
                var sp = DAL.GetItem<Tb_sp_sku_fd>(typeof(Tb_sp_sku_fd), param);

                if (sp != null)
                {
                    //异地
                    br.Add(new Tb_sp_sku_fd()
                    {
                        id = sp.id,
                        id_gys = sp.id_gys,
                        id_gys_fd = sp.id_gys_fd,
                        id_sku = sp.id_sku,
                        rq_create = sp.rq_create
                    });
                }
                else
                {
                    //本地
                    br.Add(new Tb_sp_sku_fd()
                    {
                        id_gys_fd = id_gys_fd,
                        id_gys = 0,
                        id_sku = id_sku,
                    });
                }
            }
            return br;
        }

        /// <summary>
        /// 更新销售单的分单状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        private bool Update_Sale_Order_Head_Flag_Fd(Hashtable param)
        {
            DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);
            return true;
        }

        /// <summary>
        /// 写入订单分单记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        private bool Add_sale_order_fd(Td_Sale_Order_Fd param)
        {
            DAL.Add(param);
            return true;
        }

        /// <summary>
        /// 通过 供应商编码 查询 对应用户的 采购商编码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private Tb_Cgs Query_GysOfCgs(Hashtable param)
        {
            param.Add("id", param["id_gys"]);
            param.Remove("id_gys");
            var gys = DAL.GetItem<Tb_Gys>(typeof(Tb_Gys), param);

            param.Clear();
            param.Add("id_user_master", gys.id_user_master);
            var br = DAL.GetItem<Tb_Cgs>(typeof(Tb_Cgs), param);

            return br;
        }

        /// <summary>
        /// 按单号 查询订单 子单列表
        /// 【单个】
        /// </summary>
        /// <param name="dh">订单号</param>
        /// <returns></returns>
        private IList<Td_Sale_Order_Fd> Query_Sale_Order_Fd_List(string dh)
        {
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("dh_father", dh);
            return DAL.QueryList<Td_Sale_Order_Fd>(typeof(Td_Sale_Order_Fd), param);
        }

        /// <summary>
        /// 查询订单头内容
        /// </summary>
        /// <param name="dh">订单号</param>
        /// <returns></returns>
        public Td_Sale_Order_Head Query_Sale_Order_Head(string dh)
        {
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("dh", dh);
            return DAL.GetItem<Td_Sale_Order_Head>(typeof(Td_Sale_Order_Head), param);
        }

        /// <summary>
        /// 更新订单头内容
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        private void Update_Sale_Order_Head(Hashtable param)
        {
            DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);
        }

        /// <summary>
        /// 插入出库关系表
        /// </summary>
        /// <param name="dh">出库单号</param>
        /// <param name="dh_order">订单号</param>
        [Transaction]
        private void Add_Saler_Out_relation(string dh, string dh_order)
        {
            Td_Saler_Out_Relation model = new Td_Saler_Out_Relation()
            {
                dh = dh,
                dh_order = dh_order
            };
            DAL.Add(model);
        }

        /// <summary>
        /// 根据订单号 获取所有子单单号
        /// </summary>
        /// <param name="dh">母单单号</param>
        /// <returns>子单列表</returns>
        public List<string> Get_Fd_Sale_Order_List(string dh)
        {
            if (dh != string.Empty) dh = "%" + dh + "%";
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("like_path", dh);
            var ls = DAL.QueryList<Td_Sale_Order_Fd>(typeof(Td_Sale_Order_Fd), param);

            return ls.Select(item => item.dh).ToList();
        }

        /// <summary>
        /// 查询发货头
        /// </summary>
        /// <param name="dh">订单号</param>
        /// <returns></returns>
        public Td_Sale_Out_Head Get_Sale_Out_Head_Query(string dh)
        {
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("dh", dh);
            return DAL.GetItem<Td_Sale_Out_Head>(typeof(Td_Sale_Out_Head), param);
        }

        /// <summary>
        /// 查询销售内容列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private IList<Td_Sale_Order_Body_Query> Get_Sale_Order_Body_Query(Hashtable param)
        {
            return DAL.QueryList<Td_Sale_Order_Body_Query>(typeof(Td_Sale_Order_Body), param);
        }

        /// <summary>
        /// 引用 SKU 分单商品
        /// </summary>
        /// <param name="entity">"id_gys","id_gys_fd","id_mc","id_sku","bm_Interface","id_spfl","dj_dh","sl_dh_min"</param>
        /// <returns></returns>
        [Transaction]
        public BaseResult Add_Sdk_Fd(dynamic entity)
        {
            BaseResult br = new BaseResult();

            var model = (Hashtable)entity;
            Hashtable param = new Hashtable();

            #region 分单商品关系表
            //判断是否存在重复 id_sku
            param.Clear();
            param.Add("id_gys_fd", model["id_gys_fd"]);
            param.Add("id_sku", model["id_sku"]);
            if (DAL.GetCount(typeof(Tb_sp_sku_fd), param) > 0)
            {
                //param.Add("id_gys", fd_model.id_gys);
                //DAL.UpdatePart(typeof(Tb_sp_sku_fd), param);
                br.Success = true;
                return br;
            }
            ////获取分单商品数据
            var fd_model = new Tb_sp_sku_fd()
            {
                id_gys = Convert.ToInt32(model["id_gys"]),
                id_gys_fd = Convert.ToInt32(model["id_gys_fd"]),
                id_sku = Convert.ToInt32(model["id_sku"])
            };
            //插入 分单关系表
            DAL.Add(fd_model);

            #endregion

            #region 供应商商品表
            //查询 sku商品的 供应商商品资料
            param.Clear();
            param.Add("id_sku", Convert.ToInt32(model["id_sku"]));
            var Gys_sp = DAL.GetItem<Tb_Gys_Sp>(typeof(Tb_Gys_Sp), param);

            //获取供应商商品数据
            var gys_sp_model = new Tb_Gys_Sp()
            {
                id_gys = Convert.ToInt32(model["id_gys_fd"]),
                bm_Interface = model["bm_Interface"].ToString(),
                dj_base = Convert.ToDecimal(model["dj_dh"]),
                flag_stop = 0,
                flag_up = YesNoFlag.Yes,
                id_sku = Convert.ToInt32(model["id_sku"]),
                id_sp = Gys_sp.id_sp,
                id_spfl = Convert.ToInt32(model["id_spfl"]),
                sl_kc = 0,
                sl_kc_bj = 0,
                zhl = 1,
                sort_id = 0,
                id_create = Convert.ToInt32(model["id_user"]),
            };
            //插入 供应商商品表
            DAL.Add(gys_sp_model);
            #endregion

            #region 商品单价表
            //获取 供应商 等级
            param.Clear();
            param.Add("id_gys", fd_model.id_gys_fd);
            var cgs_level = DAL.QueryList<Tb_Cgs_Level>(typeof(Tb_Cgs_Level), param);
            foreach (var item in cgs_level)
            {
                //获取 商品单价数据
                var dj_sp_model = new Tb_Sp_Dj()
                {
                    id_cgs_level = item.id,
                    dj_dh = Convert.ToDecimal(model["dj_dh"]) * item.agio / 100,

                    id_gys = Convert.ToInt32(model["id_gys_fd"]),
                    id_sp = Gys_sp.id_sp,
                    id_sku = Convert.ToInt32(model["id_sku"]),
                    sl_dh_min = Convert.ToDecimal(model["sl_dh_min"]),

                    id_create = Convert.ToInt32(model["id_user"]),
                    rq_create = DateTime.Now
                };
                //插入 商品单价表
                DAL.Add(dj_sp_model);
            }

            #endregion

            br.Success = true;
            return br;
        }

        /// <summary>
        /// 检查订单是否存在 分单商品 【单个对象】
        /// </summary>
        /// <param name="id_skuList">sku商品列表</param>
        /// <param name="id_gys_fd">分单者 的 供应商编码</param>
        /// <returns>需要分单的商品数据</returns>
        public List<Tb_sp_sku_fd> Check_Order_Goods_Fd(List<long?> id_skuList, long id_gys_fd)
        {
            var br = new List<Tb_sp_sku_fd>();

            Hashtable param = new Hashtable();

            foreach (var id_sku in id_skuList)
            {
                param.Clear();
                param.Add("id_sku", id_sku);
                param.Add("id_gys_fd", id_gys_fd);

                var sp = DAL.GetItem<Tb_sp_sku_fd>(typeof(Tb_sp_sku_fd), param);

                if (sp != null)
                {
                    br.Add(new Tb_sp_sku_fd()
                    {
                        id = sp.id,
                        id_gys = sp.id_gys,
                        id_gys_fd = sp.id_gys_fd,
                        id_sku = sp.id_sku,
                        rq_create = sp.rq_create
                    });
                }
            }
            return br;
        }

        /// <summary>
        /// 查询订单分单关系
        /// 【单个】
        /// </summary>
        /// <param name="dh"></param>
        /// <returns></returns>
        public Td_Sale_Order_Fd Query_Sale_Order_Fd(string dh)
        {
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("dh", dh);
            return DAL.GetItem<Td_Sale_Order_Fd>(typeof(Td_Sale_Order_Fd), param);
        }



        /// <summary>
        /// 分单函数 采购商-订货下单
        /// </summary>
        /// <param name="dh">母单ID</param>
        /// <param name="path">订单路径</param>
        /// <param name="id_gys">当前供应商</param>
        /// <param name="OrderData">对应的商品资料</param>
        /// <param name="PresentParam">提交的数据</param>
        /// <param name="SourceParam">原入参</param>
        /// <returns></returns>
        [Transaction]
        public BaseResult Order_Fd_Add(string dh, string path, long id_gys, Td_Sale_Order_Head_Query OrderData, Hashtable PresentParam, Hashtable SourceParam)
        {
            var Utilety = new SystemBLL.UtiletyBLL();
            Utilety.DAL = DAL;
            var Order = new OrderBLL.OrderBLL();
            Order.DAL = DAL;
            var Supplier = new SupplierBLL.SupplierBLL();
            Supplier.DAL = DAL;
            var Goods = new GoodsBLL();
            Goods.DAL = DAL;
            var Fd = new GoodsSkuFdBLL();
            Fd.DAL = DAL;
            Fd.Info_UserDAL = Info_UserDAL;

            var info = new InfoBLL.InfoBLL();
            info.DAL = DAL;
            info.Info_UserDAL = Info_UserDAL;
            BaseResult br = new BaseResult();

            #region 插入 母单

            if (dh == path)
            {
                // 获取 供应商信息
                Hashtable param = new Hashtable();
                param.Add("id_gys", OrderData.id_gys);
                param.Add("id_cgs", OrderData.id_cgs);
                var gys_cgs = DAL.GetItem<Tb_Gys_Cgs>(typeof(Tb_Gys_Cgs), param);

                if (gys_cgs == null)
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    br.Message.Add("采购关系不存在，无法订货");
                    return br;
                }

                if (gys_cgs.flag_stop.Equals(YesNoFlag.Yes))
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    br.Message.Add(string.Format("[{0}]和[{1}]关注已取消，无法订货", gys_cgs.mc_cgs, gys_cgs.mc_gys));
                    return br;
                }
                //新增普通订单
                br = Order.Add_Fd(PresentParam);
                path = "/" + path;

                #region 修改母单状态

                Hashtable h1 = new Hashtable();

                h1.Clear();
                h1.Add("new_flag_fd", 1);
                h1.Add("dh", dh);

                Update_Sale_Order_Head_Flag_Fd(h1);

                #endregion
            }

            #endregion

            //是否存在本地数据  0/1
            var my_op = 0;
            //是否存在异地数据 0/1
            var other_op = 0;

            //临时变量
            Hashtable pm_cgs = new Hashtable();
            Hashtable pm_gys = new Hashtable();

            //本地销售数据赋值
            var my_order = JSON.Deserialize<Td_Sale_Order_Head_Query>(SourceParam["orderData"].ToString());
            //异地销售数据赋值
            var fd_order = JSON.Deserialize<Td_Sale_Order_Head_Query>(SourceParam["orderData"].ToString());

            my_order.order_body.Clear();
            fd_order.order_body.Clear();
            foreach (var i in OrderData.order_body)
            {
                my_order.order_body.Add(i);
                fd_order.order_body.Add(i);
            }

            #region 数据赋值
            //获取 相关商品 sku 列表
            var id_sku_list = OrderData.order_body.Select(item => item.id_sku).ToList();
            var fd_ls = Check_Goods_Fd_List(id_sku_list, id_gys);

            //按照商品分单列表，区分商品关系;
            foreach (var item in fd_ls)
            {
                //如果是 0 表示本地的商品
                if (item.id_gys == 0)
                {
                    for (int i = fd_order.order_body.Count - 1; i >= 0; i--)
                    {
                        if (fd_order.order_body[i].id_sku == item.id_sku)
                        {
                            //在【异地数据】里面 删除 本地的资料
                            fd_order.order_body.Remove(fd_order.order_body[i]);
                            //fd_ls.Remove(item);
                            my_op = 1;
                        }
                    }
                }
                else //异地的商品
                {
                    for (int i = my_order.order_body.Count - 1; i >= 0; i--)
                    {
                        if (my_order.order_body[i].id_sku == item.id_sku)
                        {
                            //在【本地数据】里面 删除 异地的资料
                            my_order.order_body.Remove(my_order.order_body[i]);
                            other_op = 1;
                        }
                    }
                }
            }
            #endregion

            #region 本地数据下单
            // 给本地数据下单 
            if (my_op == 1)
            {

                #region 发票类型
                string invoiceFlag = SourceParam["invoiceFlag"].ToString();
                int soure = Convert.ToInt32(SourceParam["soure"]);
                switch (invoiceFlag)
                {
                    case "2":
                        my_order.invoiceFlag = InvoiceFlag.General;
                        break;
                    case "3":
                        my_order.invoiceFlag = InvoiceFlag.Vat;
                        break;
                    default:
                        my_order.invoiceFlag = InvoiceFlag.None;
                        break;
                }

                #endregion
                #region 购物车
                PresentParam.Remove("OrderSource");
                switch (soure)
                {
                    //PC端 购物车
                    case (int)OrderSourceFlag.PcCart:
                        PresentParam.Add("OrderSource", OrderSourceFlag.PcCart);
                        break;
                    //PC端 复制订单
                    case (int)OrderSourceFlag.PcClone:
                        PresentParam.Add("OrderSource", OrderSourceFlag.PcClone);
                        break;
                    default:
                        PresentParam.Add("OrderSource", OrderSourceFlag.PcNew);
                        break;
                }

                #endregion

                //获取供应商的 采购商编码
                pm_cgs.Clear();
                pm_cgs.Add("id_gys", id_gys);
                var sp_cgs = Query_GysOfCgs(pm_cgs);

                //获取供应商的 主用户编码
                pm_gys.Clear();
                pm_gys.Add("id", id_gys);
                var sp_gys = Supplier.GetGys2(pm_gys);

                my_order.id_user_bill = sp_gys.id_user_master;
                my_order.id_create = sp_gys.id_user_master;
                my_order.id_edit = sp_gys.id_user_master;
                PresentParam["id_user"] = sp_gys.id_user_master;
                my_order.id_user_master = sp_gys.id_user_master;

                //my_order.id_cgs = GetLoginInfo<long>("id_buyer");
                my_order.id_cgs = sp_cgs.id;
                my_order.id_gys = id_gys;

                //新增采购的订单号;
                br = Utilety.GetNextDH(my_order, typeof(Td_Sale_Order_Head));
                //获取日志编号
                long newOrderLogId = Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                PresentParam["newOrderLogId"] = newOrderLogId;

                PresentParam["model"] = my_order;

                //新增普通订单
                br = Order.Add_Fd(PresentParam);

                #region 写入分单关系

                var My_order_fd = new Td_Sale_Order_Fd()
                {
                    dh = my_order.dh,
                    dh_father = dh,
                    path = path + "/" + my_order.dh
                };
                Add_sale_order_fd(My_order_fd);

                #endregion
            }

            #endregion

            #region 异地数据下单

            //有分单关系的商品, 给异地订单下单
            if (other_op == 1)
            {
                //按照 供应商分组
                var fdls = fd_ls.Where(p => p.id_gys != 0).GroupBy(p => new { p.id_gys, p.id_gys_fd }).Select(g => g);
                Hashtable param = new Hashtable();
                
                foreach (var item in fdls)
                {
                    var new_fd_ls = new List<Tb_sp_sku_fd>();//new_fd_ls.AddRange(fd_ls);
                    var new_fd_order = JSON.Deserialize<Td_Sale_Order_Head_Query>(SourceParam["orderData"].ToString());
                    new_fd_order.order_body.Clear();

                    #region 数据赋值

                    //获得当前供应商的 商品 
                    foreach (var item_ls in fd_ls)
                    {
                        if (item_ls.id_gys != 0)
                        {
                            //如果供应商是对应的
                            if (item_ls.id_gys == item.Key.id_gys)
                            {
                                //插入
                                new_fd_ls.Add(item_ls);
                            }
                        }
                    }
                    //筛选出当前供应商的 model 数据
                    foreach (var model_ls in fd_order.order_body)
                    {
                        foreach (var item_ls in new_fd_ls)
                        {
                            if (model_ls.id_sku == item_ls.id_sku)
                            {
                                new_fd_order.order_body.Add(model_ls);
                            }
                        }
                    }
                    #endregion

                    #region 发票类型
                    string invoiceFlag = SourceParam["invoiceFlag"].ToString();
                    int soure = Convert.ToInt32(SourceParam["soure"]);
                    switch (invoiceFlag)
                    {
                        case "2":
                            new_fd_order.invoiceFlag = InvoiceFlag.General;
                            break;
                        case "3":
                            new_fd_order.invoiceFlag = InvoiceFlag.Vat;
                            break;
                        default:
                            new_fd_order.invoiceFlag = InvoiceFlag.None;
                            break;
                    }
                    #endregion

                    #region 购物车

                    PresentParam.Remove("OrderSource");
                    switch (soure)
                    {
                        //PC端 购物车
                        case (int)OrderSourceFlag.PcCart:
                            PresentParam.Add("OrderSource", OrderSourceFlag.PcCart);
                            break;
                        //PC端 复制订单
                        case (int)OrderSourceFlag.PcClone:
                            PresentParam.Add("OrderSource", OrderSourceFlag.PcClone);
                            break;
                        default:
                            PresentParam.Add("OrderSource", OrderSourceFlag.PcNew);
                            break;
                    }

                    #endregion

                    //获取供应商的 采购商编码
                    pm_cgs.Clear();
                    pm_cgs.Add("id_gys", item.Key.id_gys_fd);
                    var sp_cgs = Query_GysOfCgs(pm_cgs);

                    //获取供应商的 主用户编码
                    pm_gys.Clear();
                    pm_gys.Add("id", item.Key.id_gys_fd);
                    var sp_gys = Supplier.GetGys2(pm_gys);
                    pm_gys.Clear();
                    pm_gys.Add("id", item.Key.id_gys);
                    var sp_gys2 = Supplier.GetGys2(pm_gys);

                    pm_gys.Clear();
                    pm_gys.Add("id_user_master_cgs", sp_gys.id_user_master); //item.Key.id_gys_fd);
                    pm_gys.Add("id_user_master_gys", sp_gys2.id_user_master);//item.Key.id_gys);
                    var gys_cgs = Supplier.GetGysCgsRelation(pm_gys).Data as Tb_Gys_Cgs;

                    if (gys_cgs != null)
                    {
                        var cgs_lv = gys_cgs.id_cgs_level;
                        foreach (var sp in new_fd_order.order_body)
                        {
                            var Cgs_dj = Goods.Get_Sp_dj(item.Key.id_gys, cgs_lv, sp.id_sku);
                            sp.dj = Convert.ToDecimal(Cgs_dj.dj_dh);
                            sp.dj_base = Convert.ToDecimal(Cgs_dj.dj_dh);
                            sp.dj_hs = Convert.ToDecimal(Cgs_dj.dj_dh);
                            sp.dj_bhs = Convert.ToDecimal(Cgs_dj.dj_dh);
                        }
                    }
                    new_fd_order.id_user_bill = sp_gys.id_user_master;
                    new_fd_order.id_create = sp_gys.id_user_master;
                    new_fd_order.id_edit = sp_gys.id_user_master;
                    PresentParam["id_user"] = sp_gys.id_user_master;
                    new_fd_order.id_user_master = sp_gys.id_user_master;

                    //my_order.id_cgs = GetLoginInfo<long>("id_buyer");
                    new_fd_order.id_cgs = sp_cgs.id;
                    new_fd_order.id_gys = item.Key.id_gys;

                    //新增采购B的订单
                    br = Utilety.GetNextDH(new_fd_order, typeof(Td_Sale_Order_Head));
                    long newOrderLogId = Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                    PresentParam["newOrderLogId"] = newOrderLogId;

                    PresentParam["model"] = new_fd_order;

                    //新增普通订单
                    br = Order.Add_Fd(PresentParam);

                    #region 写入分单关系

                    var My_order_fd = new Td_Sale_Order_Fd()
                    {
                        dh = new_fd_order.dh,
                        dh_father = dh,
                        path = path + "/" + new_fd_order.dh
                    };
                    Add_sale_order_fd(My_order_fd);

                    #endregion
                    param.Clear();
                    param["id_user_master"] = gys_cgs.id_user_gys;
                    param["id"] = Utilety.GetNextKey(typeof(Info));
                    param["Title"] = "客户【" + gys_cgs.alias_cgs + "】下单了，" + new_fd_order.dh;
                    param["content"] = "sales," + new_fd_order.dh;
                    param["id_create"] = gys_cgs.id_user_cgs;
                    param["id_master"] = gys_cgs.id_user_master_cgs;
                    param["filename"] = "";
                    param["fileSize"] = "";
                    param["bm"] = "business";
                    param["id_info_type "] = 0;
                    param["flag_from"] = "pc";
                    info.Add(param);
                    #region 检查与向下级下单

                    //检查是否存在分单 商品
                    id_sku_list = new_fd_order.order_body.Select(item2 => item2.id_sku).ToList();
                    var goods_fd = Fd.Check_Order_Goods_Fd(id_sku_list, Convert.ToInt32(item.Key.id_gys));

                    if (goods_fd.Count > 0)
                    {
                        //分单函数
                        br = Fd.Order_Fd_Add(new_fd_order.dh, path + "/" + new_fd_order.dh, Convert.ToInt32(item.Key.id_gys), new_fd_order, PresentParam, SourceParam);

                        #region 修改母单状态
                        Hashtable h1 = new Hashtable();
                        h1.Clear();
                        h1.Add("new_flag_fd", 1);
                        h1.Add("dh", new_fd_order.dh);
                        Update_Sale_Order_Head_Flag_Fd(h1);
                        #endregion
                    }
                    #endregion
                }
            }
            #endregion

            return br;
        }


        /// <summary>
        /// 分单函数 供应商-代客下单
        /// </summary>
        /// <param name="dh">母单ID</param>
        /// <param name="path">订单路径</param>
        /// <param name="id_gys">当前供应商</param>
        /// <param name="OrderData">对应的商品资料</param>
        /// <param name="PresentParam">提交的数据</param>
        /// <param name="SourceParam">原入参</param>
        /// <returns></returns>
        [Transaction]
        public BaseResult Order_Fd_Add2(string dh, string path, long id_gys, Td_Sale_Order_Head_Query OrderData, Hashtable PresentParam, Hashtable SourceParam)
        {
            var Utilety = new SystemBLL.UtiletyBLL();
            Utilety.DAL = DAL;
            var SaleOrder = new SaleOrderBLL();
            SaleOrder.DAL = DAL;
            var Supplier = new SupplierBLL.SupplierBLL();
            Supplier.DAL = DAL;
            var Goods = new GoodsBLL();
            Goods.DAL = DAL;
            var Fd = new GoodsSkuFdBLL();
            Fd.DAL = DAL;

            BaseResult br = new BaseResult();

            #region 插入 母单

            if (dh == path)
            {
                // 获取 供应商信息
                Hashtable param = new Hashtable();
                param.Add("id_gys", OrderData.id_gys);
                param.Add("id_cgs", OrderData.id_cgs);
                var gys_cgs = DAL.GetItem<Tb_Gys_Cgs>(typeof(Tb_Gys_Cgs), param);

                if (gys_cgs == null)
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    br.Message.Add("采购关系不存在，无法订货");
                    return br;
                }

                if (gys_cgs.flag_stop.Equals(YesNoFlag.Yes))
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    br.Message.Add(string.Format("[{0}]和[{1}]关注已取消，无法订货", gys_cgs.mc_cgs, gys_cgs.mc_gys));
                    return br;
                }
                //新增普通订单
                br = SaleOrder.Add_Fd(PresentParam);
                path = "/" + path;

                #region 修改母单状态

                Hashtable h1 = new Hashtable();

                h1.Clear();
                h1.Add("new_flag_fd", 1);
                h1.Add("dh", dh);

                Update_Sale_Order_Head_Flag_Fd(h1);

                #endregion
            }

            #endregion

            //是否存在本地数据  0/1
            var my_op = 0;
            //是否存在异地数据 0/1
            var other_op = 0;

            //临时变量
            Hashtable pm_cgs = new Hashtable();
            Hashtable pm_gys = new Hashtable();

            //本地销售数据赋值
            var my_order = JSON.Deserialize<Td_Sale_Order_Head_Query>(SourceParam["orderData"].ToString());
            //异地销售数据赋值
            var fd_order = JSON.Deserialize<Td_Sale_Order_Head_Query>(SourceParam["orderData"].ToString());

            my_order.order_body.Clear();
            fd_order.order_body.Clear();
            foreach (var i in OrderData.order_body)
            {
                my_order.order_body.Add(i);
                fd_order.order_body.Add(i);
            }

            #region 数据赋值
            //获取 相关商品 sku 列表
            var id_sku_list = OrderData.order_body.Select(item => item.id_sku).ToList();
            var fd_ls = Check_Goods_Fd_List(id_sku_list, id_gys);

            //按照商品分单列表，区分商品关系;
            foreach (var item in fd_ls)
            {
                //如果是 0 表示本地的商品
                if (item.id_gys == 0)
                {
                    for (int i = fd_order.order_body.Count - 1; i >= 0; i--)
                    {
                        if (fd_order.order_body[i].id_sku == item.id_sku)
                        {
                            //在【异地数据】里面 删除 本地的资料
                            fd_order.order_body.Remove(fd_order.order_body[i]);
                            //fd_ls.Remove(item);
                            my_op = 1;
                        }
                    }
                }
                else //异地的商品
                {
                    for (int i = my_order.order_body.Count - 1; i >= 0; i--)
                    {
                        if (my_order.order_body[i].id_sku == item.id_sku)
                        {
                            //在【本地数据】里面 删除 异地的资料
                            my_order.order_body.Remove(my_order.order_body[i]);
                            other_op = 1;
                        }
                    }
                }
            }
            #endregion

            #region 本地数据下单
            // 给本地数据下单 
            if (my_op == 1)
            {

                #region 发票类型
                string invoiceFlag = SourceParam["invoiceFlag"].ToString();
                int soure = Convert.ToInt32(SourceParam["soure"]);
                switch (invoiceFlag)
                {
                    case "2":
                        my_order.invoiceFlag = InvoiceFlag.General;
                        break;
                    case "3":
                        my_order.invoiceFlag = InvoiceFlag.Vat;
                        break;
                    default:
                        my_order.invoiceFlag = InvoiceFlag.None;
                        break;
                }

                #endregion
                #region 购物车
                PresentParam.Remove("OrderSource");
                switch (soure)
                {
                    //PC端 购物车
                    case (int)OrderSourceFlag.PcCart:
                        PresentParam.Add("OrderSource", OrderSourceFlag.PcCart);
                        break;
                    //PC端 复制订单
                    case (int)OrderSourceFlag.PcClone:
                        PresentParam.Add("OrderSource", OrderSourceFlag.PcClone);
                        break;
                    default:
                        PresentParam.Add("OrderSource", OrderSourceFlag.PcNew);
                        break;
                }

                #endregion

                //获取供应商的 采购商编码
                pm_cgs.Clear();
                pm_cgs.Add("id_gys", id_gys);
                var sp_cgs = Query_GysOfCgs(pm_cgs);

                //获取供应商的 主用户编码
                pm_gys.Clear();
                pm_gys.Add("id", id_gys);
                var sp_gys = Supplier.GetGys2(pm_gys);

                my_order.id_user_bill = sp_gys.id_user_master;
                my_order.id_create = sp_gys.id_user_master;
                my_order.id_edit = sp_gys.id_user_master;
                PresentParam["id_user"] = sp_gys.id_user_master;
                my_order.id_user_master = sp_gys.id_user_master;

                //my_order.id_cgs = GetLoginInfo<long>("id_buyer");
                my_order.id_cgs = sp_cgs.id;
                my_order.id_gys = id_gys;

                //新增采购的订单号;
                br = Utilety.GetNextDH(my_order, typeof(Td_Sale_Order_Head));
                //获取日志编号
                long newOrderLogId = Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                PresentParam["newOrderLogId"] = newOrderLogId;

                PresentParam["model"] = my_order;

                //新增普通订单
                br = SaleOrder.Add_Fd(PresentParam);

                #region 写入分单关系

                var My_order_fd = new Td_Sale_Order_Fd()
                {
                    dh = my_order.dh,
                    dh_father = dh,
                    path = path + "/" + my_order.dh
                };
                Add_sale_order_fd(My_order_fd);

                #endregion
            }

            #endregion

            #region 异地数据下单

            //有分单关系的商品, 给异地订单下单
            if (other_op == 1)
            {
                //按照 供应商分组
                var fdls = fd_ls.Where(p => p.id_gys != 0).GroupBy(p => new { p.id_gys, p.id_gys_fd }).Select(g => g);

                foreach (var item in fdls)
                {
                    var new_fd_ls = new List<Tb_sp_sku_fd>();//new_fd_ls.AddRange(fd_ls);
                    var new_fd_order = JSON.Deserialize<Td_Sale_Order_Head_Query>(SourceParam["orderData"].ToString());
                    new_fd_order.order_body.Clear();

                    #region 数据赋值

                    //获得当前供应商的 商品 
                    foreach (var item_ls in fd_ls)
                    {
                        if (item_ls.id_gys != 0)
                        {
                            //如果供应商是对应的
                            if (item_ls.id_gys == item.Key.id_gys)
                            {
                                //插入
                                new_fd_ls.Add(item_ls);
                            }
                        }
                    }
                    //筛选出当前供应商的 model 数据
                    foreach (var model_ls in fd_order.order_body)
                    {
                        foreach (var item_ls in new_fd_ls)
                        {
                            if (model_ls.id_sku == item_ls.id_sku)
                            {
                                new_fd_order.order_body.Add(model_ls);
                            }
                        }
                    }
                    #endregion

                    #region 发票类型
                    string invoiceFlag = SourceParam["invoiceFlag"].ToString();
                    int soure = Convert.ToInt32(SourceParam["soure"]);
                    switch (invoiceFlag)
                    {
                        case "2":
                            new_fd_order.invoiceFlag = InvoiceFlag.General;
                            break;
                        case "3":
                            new_fd_order.invoiceFlag = InvoiceFlag.Vat;
                            break;
                        default:
                            new_fd_order.invoiceFlag = InvoiceFlag.None;
                            break;
                    }
                    #endregion

                    #region 购物车
                    PresentParam.Remove("OrderSource");
                    switch (soure)
                    {
                        //PC端 购物车
                        case (int)OrderSourceFlag.PcCart:
                            PresentParam.Add("OrderSource", OrderSourceFlag.PcCart);
                            break;
                        //PC端 复制订单
                        case (int)OrderSourceFlag.PcClone:
                            PresentParam.Add("OrderSource", OrderSourceFlag.PcClone);
                            break;
                        default:
                            PresentParam.Add("OrderSource", OrderSourceFlag.PcNew);
                            break;
                    }

                    #endregion

                    //获取供应商的 采购商编码
                    pm_cgs.Clear();
                    pm_cgs.Add("id_gys", item.Key.id_gys_fd);
                    var sp_cgs = Query_GysOfCgs(pm_cgs);

                    //获取供应商的 主用户编码
                    pm_gys.Clear();
                    pm_gys.Add("id", item.Key.id_gys_fd);
                    var sp_gys = Supplier.GetGys2(pm_gys);
                    pm_gys.Clear();
                    pm_gys.Add("id", item.Key.id_gys);
                    var sp_gys2 = Supplier.GetGys2(pm_gys);

                    pm_gys.Clear();
                    pm_gys.Add("id_user_master_cgs", sp_gys.id_user_master); //item.Key.id_gys_fd);
                    pm_gys.Add("id_user_master_gys", sp_gys2.id_user_master);//item.Key.id_gys);
                    var gys_cgs = Supplier.GetGysCgsRelation(pm_gys).Data as Tb_Gys_Cgs;

                    if (gys_cgs != null)
                    {
                        var cgs_lv = gys_cgs.id_cgs_level;
                        foreach (var sp in new_fd_order.order_body)
                        {
                            var Cgs_dj = Goods.Get_Sp_dj(item.Key.id_gys, cgs_lv, sp.id_sku);
                            sp.dj = Convert.ToDecimal(Cgs_dj.dj_dh);
                            sp.dj_base = Convert.ToDecimal(Cgs_dj.dj_dh);
                            sp.dj_hs = Convert.ToDecimal(Cgs_dj.dj_dh);
                            sp.dj_bhs = Convert.ToDecimal(Cgs_dj.dj_dh);
                        }
                    }
                    new_fd_order.id_user_bill = sp_gys.id_user_master;
                    new_fd_order.id_create = sp_gys.id_user_master;
                    new_fd_order.id_edit = sp_gys.id_user_master;
                    PresentParam["id_user"] = sp_gys.id_user_master;
                    new_fd_order.id_user_master = sp_gys.id_user_master;

                    //my_order.id_cgs = GetLoginInfo<long>("id_buyer");
                    new_fd_order.id_cgs = sp_cgs.id;
                    new_fd_order.id_gys = item.Key.id_gys;

                    //新增采购B的订单
                    br = Utilety.GetNextDH(new_fd_order, typeof(Td_Sale_Order_Head));
                    long newOrderLogId = Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                    PresentParam["newOrderLogId"] = newOrderLogId;

                    PresentParam["model"] = new_fd_order;

                    //新增普通订单
                    br = SaleOrder.Add_Fd(PresentParam);

                    #region 写入分单关系

                    var My_order_fd = new Td_Sale_Order_Fd()
                    {
                        dh = new_fd_order.dh,
                        dh_father = dh,
                        path = path + "/" + new_fd_order.dh
                    };
                    Add_sale_order_fd(My_order_fd);

                    #endregion

                    #region 检查与向下级下单

                    //检查是否存在分单 商品
                    id_sku_list = new_fd_order.order_body.Select(item2 => item2.id_sku).ToList();
                    var goods_fd = Fd.Check_Order_Goods_Fd(id_sku_list, Convert.ToInt32(item.Key.id_gys));

                    if (goods_fd.Count > 0)
                    {
                        //分单函数
                        br = Fd.Order_Fd_Add2(new_fd_order.dh, path + "/" + new_fd_order.dh, Convert.ToInt32(item.Key.id_gys), new_fd_order, PresentParam, SourceParam);

                        #region 修改母单状态
                        Hashtable h1 = new Hashtable();
                        h1.Clear();
                        h1.Add("new_flag_fd", 1);
                        h1.Add("dh", new_fd_order.dh);
                        Update_Sale_Order_Head_Flag_Fd(h1);
                        #endregion
                    }
                    #endregion
                }
            }
            #endregion

            return br;
        }


        /// <summary>
        /// 分单-出库-更新状态
        /// </summary>
        /// <param name="dh">当前订单号</param>
        /// <param name="ck_dh">出库单号</param>
        /// <returns></returns>
        [Transaction]
        public BaseResult SaleOut_Out_Fd(string dh, string ck_dh)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = new Hashtable();
                //检查当前单号是否存在上级单号
                var Order_Fd = Query_Sale_Order_Fd(dh);
                //存在上级订单
                if (Order_Fd != null)
                {
                    //写入 出库关系列表
                    if (ck_dh != string.Empty)
                    {
                        Add_Saler_Out_relation(ck_dh, dh);
                    }

                    //上级单 单号
                    var dh_order = Order_Fd.dh_father;

                    //查询子单列表
                    var Fd_list = Query_Sale_Order_Fd_List(dh_order);

                    int i = 0;
                    //统计已发货
                    int Delivered = 0;
                    //最高级别操作状态
                    //最高级别操作状态
                    short? i_v = 0;
                    short? i_fh = 0;
                    short? i_ck = 0;

                    foreach (var item in Fd_list)
                    {
                        //获取订单头内容
                        var head = Query_Sale_Order_Head(item.dh);
                        if (head.flag_state < 61 )
                        {
                            if (head.flag_state != 0)
                            {
                                i++;
                                //获取最高级的操作状态
                                //获取最高级的操作状态
                                if (i_v < head.flag_state)
                                {
                                    i_v = head.flag_state;
                                }
                                if (i_fh < head.flag_fh)
                                {
                                    i_fh = head.flag_fh;
                                }
                                if (i_ck < head.flag_out)
                                {
                                    i_ck = head.flag_out;
                                }
                            }
                        }
                    }
                    param.Clear();
                    param.Add("dh", dh_order);
                    if (i > 0)
                    {
                        //更新上级单 状态：发货中 1
                        param.Add("new_flag_fh", i_fh);
                        param.Add("new_flag_state", i_v);
                        param.Add("new_flag_out", i_ck);
                        //更新上级订单 出库状态
                        Update_Sale_Order_Head(param);
                    }
                    //检查当前单号是否存在上级单号
                    var Query_Fd = Query_Sale_Order_Fd(dh_order);
                    //存在上级订单
                    if (Query_Fd != null)
                    {
                        br = SaleOut_Out_Fd(dh_order, string.Empty);
                    }
                    else
                    {
                        return br;
                    }

                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return br;
        }

        /// <summary>
        /// 分单-发货-更新状态 **
        /// </summary>
        /// <param name="dh">当前订单号</param>
        /// <returns></returns>
        [Transaction]
        public BaseResult SaleOut_FH_Fd(string dh)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();

            try
            {
                //检查当前单号是否存在上级单号
                var Order_Fd = Query_Sale_Order_Fd(dh);
                //存在上级订单
                if (Order_Fd != null)
                {
                    //上级单 单号
                    var dh_order = Order_Fd.dh_father;
                    //查询子 单列表
                    var Fd_list = Query_Sale_Order_Fd_List(dh_order);

                    //统计未发货子单数量
                    int i = 0;
                    //最高级别操作状态
                    short? i_v = 0;
                    short? i_fh = 0;
                    short? i_ck = 0;

                    //循环获取子单
                    foreach (var item in Fd_list)
                    {
                        //获取订单头内容
                        var head = Query_Sale_Order_Head(item.dh);
                        if (head.flag_state < 80 ) 
                        {
                            if (head.flag_state != 0)
                            {
                                i++;
                                //获取最高级的操作状态
                                //获取最高级的操作状态
                                if (i_v < head.flag_state)
                                {
                                    i_v = head.flag_state;
                                }
                                if (i_fh < head.flag_fh)
                                {
                                    i_fh = head.flag_fh;
                                }
                                if (i_ck < head.flag_out)
                                {
                                    i_ck = head.flag_out;
                                }
                            }
                        }
                    }
                    param.Clear();
                    param.Add("dh", dh_order);
                    if (i > 0)
                    {
                        //更新上级单 状态：发货中 1
                        param.Add("new_flag_fh", i_fh);
                        param.Add("new_flag_state", i_v);
                        param.Add("new_flag_out", i_ck);
                    }
                    else
                    {
                        //更新上级单 状态：已出库 2
                        param.Add("new_flag_fh", 2);
                        param.Add("new_flag_state", OrderFlag.Delivered);
                    }
                    //更新上级订单 出库状态
                    Update_Sale_Order_Head(param);
                    br = SaleOut_FH_Fd(dh_order);

                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return br;
        }

        /// <summary>
        /// 分单--更新--收货状态
        /// </summary>
        /// <param name="dh">收货单号</param>
        /// <returns></returns>
        [Transaction]
        public BaseResult SaleOut_ShouHuo_Fd(string dh)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();

            try
            {
                //检查当前单号是否存在上级单号
                var Order_Fd = Query_Sale_Order_Fd(dh);
                //存在上级订单
                if (Order_Fd != null)
                {
                    //上级单 单号
                    var dh_order = Order_Fd.dh_father;
                    //查询子单列表
                    var Fd_list = Query_Sale_Order_Fd_List(dh_order);

                    //统计子单数量
                    int e = 0;
                    //统计未结束子单数量
                    int i = 0;
                    short? i_v = 0; //最高级别操作状态
                    short? i_fh = 0;
                    short? i_ck = 0;


                    //统计作废数量
                    int f = 0;
                    //统计删除数量
                    int d = 0;
                    //统计已发货
                    int Delivered = 0;

                    //本单循环子单
                    foreach (var item in Fd_list)
                    {
                        //统计子单总数量
                        e++;
                        //获取订单头内容
                        var head = Query_Sale_Order_Head(item.dh);

                        if (head.flag_state < 80 ) 
                        {
                            if (head.flag_state != 0)
                            {
                                //统计未结束子单数量
                                i++;
                                //获取最高级的操作状态
                                if (i_v < head.flag_state)
                                {
                                    i_v = head.flag_state;
                                }
                                if (i_fh < head.flag_fh)
                                {
                                    i_fh = head.flag_fh;
                                }
                                if (i_ck < head.flag_out)
                                {
                                    i_ck = head.flag_out;
                                }
                            }
                        }

                        if (head.flag_state == 70) Delivered++; //统计已发货
                        if (head.flag_state == 90) f++; //统计作废数量
                        if (head.flag_state == 100) d++; //统计删除数量
                    }
                    param.Clear();
                    param.Add("dh", dh_order);
                    if (i > 0)
                    {
                        //更新为 子单最高级的操作状态
                        param.Add("new_flag_state", i_v);
                        param.Add("new_flag_fh", i_fh);
                        param.Add("new_flag_out", i_ck);
                    }
                    else
                    {
                        //更新为已发货
                        param.Add("new_flag_state", OrderFlag.Receipted);
                        if (Fd_list.Count > 0)
                        {
                            //如果存在全单作废
                            if (f == e)
                            {
                                param["new_flag_state"] = OrderFlag.Invalided;

                            }
                            //如果存在全单删除
                            if (d == e)
                            {
                                param["new_flag_state"] = OrderFlag.Deleted;
                            }
                            //如果有子单是已发货的状态
                            if (Delivered > 0)
                            {
                                param["new_flag_state"] = OrderFlag.Delivered;
                            }
                        }
                    }
                    //更新上级订单 出库状态
                    Update_Sale_Order_Head(param);

                    br = SaleOut_ShouHuo_Fd(dh_order);

                }
                else
                {
                    //查询子单列表
                    var Fd_list = Query_Sale_Order_Fd_List(dh);
                    //统计子单数量
                    int e = 0;
                    //统计未结束子单数量
                    int i = 0;
                    short? i_v = 0; //最高级别操作状态
                    short? i_fh = 0;
                    short? i_ck = 0;

                    //统计作废数量
                    int f = 0;
                    //统计删除数量
                    int d = 0;
                    //统计已发货
                    int Delivered = 0;

                    //本单循环子单
                    foreach (var item in Fd_list)
                    {
                        //统计子单总数量
                        e++;
                        //获取订单头内容
                        var head = Query_Sale_Order_Head(item.dh);

                        if (head.flag_state < 80 )
                        {
                            if (head.flag_state != 0)
                            {
                                //统计未结束子单数量
                                i++;
                                //获取最高级的操作状态
                                //获取最高级的操作状态
                                if (i_v < head.flag_state)
                                {
                                    i_v = head.flag_state;
                                }
                                if (i_fh < head.flag_fh)
                                {
                                    i_fh = head.flag_fh;
                                }
                                if (i_ck < head.flag_out)
                                {
                                    i_ck = head.flag_out;
                                }
                            }
                        }
                        if (head.flag_state == 90) f++; //统计作废数量
                        if (head.flag_state == 100) d++; //统计删除数量
                        if (head.flag_state == 70) Delivered++; //统计已发货
                    }
                    param.Clear();
                    param.Add("dh", dh);
                    if (i > 0)
                    {
                        //更新为 子单最高级的操作状态
                        param.Add("new_flag_state", i_v);
                        param.Add("new_flag_fh", i_fh);
                        param.Add("new_flag_out", i_ck);
                        //更新上级订单 出库状态
                        Update_Sale_Order_Head(param);
                    }
                    //else
                    //{
                    //    //更新为已发货
                    //    param.Add("new_flag_state", OrderFlag.Receipted);
                    //    if (Fd_list.Count > 0)
                    //    {
                    //        //如果存在全单作废
                    //        if (f == e)
                    //        {
                    //            param["new_flag_state"] = OrderFlag.Invalided;

                    //        }
                    //        //如果存在全单删除
                    //        if (f == e)
                    //        {
                    //            param["new_flag_state"] = OrderFlag.Deleted;
                    //        }
                    //        //如果有子单是已发货的状态
                    //        if (Delivered > 0)
                    //        {
                    //            param["new_flag_state"] = OrderFlag.Delivered;
                    //        }
                    //    }
                    //}

                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return br;
        }

        /// <summary>
        /// 主要用途 更新 作废/删除 状态
        /// </summary>
        /// <param name="dh">单号</param>
        /// <param name="op">状态</param>
        /// <returns></returns>
        [Transaction]
        public BaseResult SaleOut_UpdateFlag_Fd(string dh, OrderFlag op)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            try
            {
                //检查当前单号是否存在上级单号
                var Order_Fd = Query_Sale_Order_Fd(dh);
                //存在上级订单
                if (Order_Fd != null)
                {
                    //上级单 单号
                    var dh_order = Order_Fd.dh_father;
                    //查询子 单列表
                    var Fd_list = Query_Sale_Order_Fd_List(dh_order);

                    //统计子单数量
                    int e = 0;
                    //统计未结束子单数量
                    int i = 0;
                    short? i_v = 0; //最高级别操作状态
                    short? i_fh = 0;
                    short? i_ck = 0;
                    short? flag_state = 0;
                    //统计作废数量
                    int f = 0;
                    //统计删除数量
                    int d = 0;
                    //统计已发货
                    int Delivered = 0;

                    //循环获取子单
                    foreach (var item in Fd_list)
                    {
                        e++;
                        //获取订单头内容
                        var head = Query_Sale_Order_Head(item.dh);

                        if (head.flag_state < (short?) op + 1)
                        {
                            if (head.flag_state != 0)
                            {
                                i++;
                                //获取最高级的操作状态
                                //获取最高级的操作状态
                                if (i_v < head.flag_state)
                                {
                                    i_v = head.flag_state;
                                }
                                if (i_fh < head.flag_fh)
                                {
                                    i_fh = head.flag_fh;
                                }
                                if (i_ck < head.flag_out)
                                {
                                    i_ck = head.flag_out;
                                }
                            }
                        }
                        
                        if (flag_state < head.flag_state) flag_state = head.flag_state;

                        if (head.flag_state == 70) Delivered++; //统计已发货
                        if (head.flag_state == 90) f++; //统计作废数量
                        if (head.flag_state == 100) d++; //统计删除数量
                        
                    }
                    param.Clear();
                    param.Add("dh", dh_order);
                    if (i > 0)
                    {
                        //更新为 子单最高级的操作状态
                        param.Add("new_flag_state", i_v);
                        param.Add("new_flag_fh", i_fh);
                        param.Add("new_flag_out", i_ck);
                    }
                    else
                    {
                        //更新为已发货
                        if (op == OrderFlag.Invalided)
                        {
                            param.Add("new_flag_state", flag_state);
                        }
                        else
                        {
                            param.Add("new_flag_state", OrderFlag.Receipted);
                        }
                        if (Fd_list.Count > 0)
                        {
                            //如果存在全单作废
                            if (f == e)
                            {
                                param["new_flag_state"] = OrderFlag.Invalided;

                            }
                            //如果存在全单删除
                            if (d == e)
                            {
                                param["new_flag_state"] = OrderFlag.Deleted;
                            }
                            //如果有子单是已发货的状态
                            if (Delivered > 0)
                            {
                                param["new_flag_state"] = OrderFlag.Delivered;
                            }
                        }
                    }
                    //更新上级订单 出库状态
                    Update_Sale_Order_Head(param);
                    br = SaleOut_UpdateFlag_Fd(dh_order, op);
                }
                else
                {
                    //查询子 单列表
                    var Fd_list = Query_Sale_Order_Fd_List(dh);

                    //统计子单数量
                    int e = 0;
                    //统计未结束子单数量
                    int i = 0;
                    short? i_v = 0; //最高级别操作状态
                    short? i_fh = 0;
                    short? i_ck = 0;

                    //统计作废数量
                    int f = 0;
                    //统计删除数量
                    int d = 0;
                    //统计已发货
                    int Delivered = 0;

                    //循环获取子单
                    foreach (var item in Fd_list)
                    {
                        e++;
                        //获取订单头内容
                        var head = Query_Sale_Order_Head(item.dh);

                        if (head.flag_state < (short?)op + 1 )
                        {
                            if (head.flag_state != 0)
                            {
                                i++;
                                //获取最高级的操作状态
                                //获取最高级的操作状态
                                if (i_v < head.flag_state)
                                {
                                    i_v = head.flag_state;
                                }
                                if (i_fh < head.flag_fh)
                                {
                                    i_fh = head.flag_fh;
                                }
                                if (i_ck < head.flag_out)
                                {
                                    i_ck = head.flag_out;
                                }
                            }
                        }
                        if (head.flag_state == 70) Delivered++; //统计已发货
                        if (head.flag_state == 90) f++; //统计作废数量
                        if (head.flag_state == 100) d++; //统计删除数量
                    }
                    param.Clear();
                    param.Add("dh", dh);
                    if (i > 0)
                    {
                        //更新为 子单最高级的操作状态
                        param.Add("new_flag_state", i_v);
                        param.Add("new_flag_fh", i_fh);
                        param.Add("new_flag_out", i_ck);

                        //更新上级订单 出库状态
                        Update_Sale_Order_Head(param);
                    }
                    //else
                    //{
                    //    //更新为已发货
                    //    param.Add("new_flag_state", OrderFlag.Receipted);
                    //    if (Fd_list.Count > 0)
                    //    {
                    //        //如果存在全单作废
                    //        if (f == e)
                    //        {
                    //            param["new_flag_state"] = OrderFlag.Invalided;

                    //        }
                    //        //如果存在全单删除
                    //        if (f == e)
                    //        {
                    //            param["new_flag_state"] = OrderFlag.Deleted;
                    //        }
                    //        //如果有子单是已发货的状态
                    //        if (Delivered > 0)
                    //        {
                    //            param["new_flag_state"] = OrderFlag.Delivered;
                    //        }
                    //    }
                    //}

                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return br;
        }









    }
}
