app.psfprk = app.psfprk || {};

//页面加载完成执行函数
app.psfprk.add_ready = function () {
    app.psfprk.vue = new Vue({
        el: "#psfprkAdd",
        data: {
            getshop: [],
            selected: '',
            id_shop_ck: $('#id_shop', _).val(),

        },
        methods: {
            getshopfunc: function () {
                _this = this;
                var options = {
                    url: $.DHB.U('/SearchCondition/GetPzxSubShop'),
                    data: { id: app.psfprk.vue.id_shop_ck },
                    type: "POST",
                    dataType: 'json',
                    success: function (data) {                        
                        console.log(app.psfprk.vue.id_shop_ck);
                        if (data.Success == true) {
                            _this.getshop = data.Data;
                            if (app.psfprk.option == 'add') {
                            } else {
                                _this.selected = app.psfprk.default_id_shop;
                            }
                        }

                    },
                    error: function () {
                        console.log("i");
                    }
                }
                app.httpAjax.post(options);
            }

        },

    });

    app.psfprk.vue.getshopfunc();
    ///////////////////////////////////////
    $('.table_list', _).on('mouseleave', '.common_select', function () {
        $('.common_select_list', _).hide();
    });
    $('.table_list', _).on('click', '.common_select_list li', function () {

        var li_txt = $(this).html();
        $(this).parents('.common_select_list').hide();
        $(this).parents('.common_select').find('span').html(li_txt);
        $(this).parents('.common_select').find('span').attr('data-zhl', $(this).attr('data-zhl'));
        app.psfprk.dw_select_onchange(this);
    });
    app.psfprk.setresult(true);
    $(_ + '#div_sm').keyup(function (e) {
        if (e.keyCode == 13) {
            var scan = $(_ + '#barcode_search').val();
            $(_ + '#barcode_search').val('');
            var id_shop = $(_ + "#id_shop").val();
            var id_shop_ck = $(_ + "#id_shop_ck").val();
            if (id_shop_ck == '') {
                $.DHB.message({ 'content': '请选择退货门店!', 'time': 1000, 'type': 'i' });
                return;
            }
            //Post读取数据赋值
            app.httpAjax.post({
                data: { keyword: scan, id_shop: id_shop, id_shop_ck: id_shop_ck },
                headers: {},
                url: '/shopsp/GetShopspListForPs',
                type: "POST",
                dataType: 'json',
                beforeSend: null,
                success: function (ret) {
                    if (ret.Success) {
                        $('#psfprk_shopsp_table>tbody tr').each(function () {
                            var tr = $(this);
                            if (tr.attr('data-item') == ret.Data.id) {
                                var old_sl = tr.find('input[name=sl]').val();
                                tr.find('input[name=sl]').val((parseFloat(old_sl) + 1).toFixed(2));
                                tr.find('input[name="sl"]').blur();
                                return false;
                            } else if (tr.attr('data-item') == "") {
                                tr.attr("data-item", ret.Data.id);
                                tr.find('td[name="mc"] div').text(ret.Data.mc);
                                tr.find('input[name="sl"]').val(limit_num('1', app.psfprk.digit_sl));
                                var dj_jh = ret.Data.dj_ps.toString() == "0" ? ret.Data.dj_jh.toString() : ret.Data.dj_ps.toString();
                                dj_jh = limit_num(dj_jh, app.psfprk.digit_dj);
                                tr.find('input[name="dj_jh"]').val(dj_jh);
                                tr.find('input[name="je"]').val(dj_jh);
                                //设置old-data
                                tr.find('input[name="barcode"]').attr('old-data', ret.Data.barcode);
                                tr.find('input[name="barcode"]').val(ret.Data.barcode);
                                tr.find('input[name="sl"]').attr('old-data', limit_num('1', app.psfprk.digit_sl));
                                tr.find('input[name="dj_jh"]').attr('old-data', dj_jh);
                                tr.find('input[name="je"]').attr('old-data', dj_jh);
                                tr.find('input[name="shopsp_obj"]').attr("value", ret.Data.id);

                                //转换率
                                var zhl = ret.Data.zhl.toString();
                                zhl = limit_num(zhl, app.psfprk.digit_sl);
                                //var dj_jh = ret.Data.dj_jh.toString();
                                // dj_jh = limit_num(dj_jh, app.psfprk.digit_dj);
                                tr.find('input[name="sl_zhl"]').val(zhl);

                                tr.find('input[name="shopsp_obj"]').attr("data-id_kcsp", ret.Data.id_kcsp);
                                tr.find('input[name="shopsp_obj"]').attr("data-id_sp", ret.Data.id_sp);

                                //绑定下拉
                                tr.find('div[name=dw_select]>div>span').html(ret.Data.dw);
                                tr.find('div[name=dw_select]>div>span').attr("data-zhl", ret.Data.zhl);
                                tr.find('div[name=dw_select]').css('display', 'block');
                                tr.find('ul').append('<li  value=' + dj_jh + '  data-zhl=' + zhl + ' data-id_shopsp=' + ret.Data.id + ' data-id_kcsp=' + ret.Data.id_kcsp + ' data-id_sp=' + ret.Data.id_sp + ' data-dw=' + ret.Data.dw + '>' + ret.Data.dw + '</li>');

                                var tbody = tr.parents("tbody");
                                var data_item = tbody.find("tr:last").attr("data-item");
                                if (data_item == ret.Data.id) {
                                    app.psfprk.addshopsp();//新增最后一条新记录
                                    app.psfprk.reset_xh();
                                }
                                app.psfprk.setresult(false);
                                return false;
                            }
                        });

                    } else {
                        $.DHB.dialog({ 'title': '选择商品', 'url': $.DHB.U('shopsp/search?keyword=' + scan + '&id_shop=' + id_shop + '&id_shop_ck=' + id_shop_ck), 'id': 'dialog-shopsp-search', 'confirm': app.psfprk.dialogCallBack });
                    }
                },
                error: null,
                complete: null
            });

        }
    });
    
}

//点击返配出库单号触发事件
app.psfprk.select_ckd = function () {
    var id_shop_ck = $(_ + '#id_shop_ck').val();
    var id_shop = $(_ + '#id_shop').val();    
    $.DHB.dialog({ 'title': '选择返配出库单', 'url': $.DHB.U('psfpck/searchlist?s_id_shop_ck=' + id_shop_ck + '&id_shop_pszx=' + id_shop), 'id': 'dialog-psfpck-search', 'confirm': app.psfprk.select_ckd_callback });
}
//点击返配出库单号回调方法
app.psfprk.select_ckd_callback = function (array) {
    $("#dh_origin").val(array.dh);
    $("#bm_djlx_origin").val(array.bm_djlx);
    $("#id_bill_origin").val(array.id);
    if (array.id) {
        //Post读取数据赋值
        app.httpAjax.post({
            data: { id: array.id },
            headers: {},
            url: '/psfpck/queryfpckdspmx',
            type: "POST",
            dataType: 'json',
            beforeSend: null,
            success: function (ret) {
                if (ret.Success) {
                    var shopsp = [];
                    if (ret.Data.model2List.length > 0) {
                        for (var item in ret.Data.model2List) {
                            var obj = ret.Data.model2List[item];
                            var shopsp_e = {};
                            shopsp_e.id_shopsp = obj.id_shopsp;
                            shopsp_e.id_kcsp = obj.id_kcsp;
                            shopsp_e.id_sp = obj.id_sp;
                            shopsp_e.barcode = obj.barcode;
                            shopsp_e.mc = obj.shopsp_name;
                            shopsp_e.dw = obj.dw;
                            shopsp_e.zhl = obj.zhl;
                            shopsp_e.dj_jh = obj.dj;
                            shopsp_e.dj_ps = obj.dj;
                            shopsp_e.sl = obj.sl;
                            shopsp_e.je = obj.je;
                            shopsp_e.bz = obj.bz;
                            shopsp.push(shopsp_e);
                        }
                    }
                    if (ret.Data.model1) {
                        //
                        app.psfprk.vue.selected = ret.Data.model1.id_shop;
                        $(_ + "#remark").val(ret.Data.model1.bz);
                    }
                    var jsonStr = JSON.stringify(shopsp);
                    app.psfprk.removeAll();
                    app.psfprk.dialogCallBackWork(jsonStr);
                    app.psfprk.setresult(true);
                    $("button:contains('导入')", _).addClass('disabled').removeAttr('onclick');
                    $("button:contains('扫描')", _).addClass('disabled').removeAttr('onclick');
                    $("#psfprk_shopsp_table a:contains('选择')", _).addClass('disabled').removeAttr('onclick');
                    $("button:contains('增加行')", _).addClass('disabled').removeAttr('onclick');
                    $('#id_shop', _).attr('disabled', 'disabled');
                    $('#id_shop_ck', _).attr('disabled', 'disabled');
                }
            },
            error: null,
            complete: null
        });

    }
}

//配送中心值发生变化触发事件
app.psfprk.select_shop_change = function (obj) {
    
    app.psfprk.clear_select_sqd();
    app.psfprk.shop_change_clear();
    var id_shop = $(obj).val();
    app.httpAjax.post({
        data: { id: id_shop },
        headers: {},
        url: '/SearchCondition/GetPzxSubShop',
        type: "POST",
        dataType: 'json',
        beforeSend: null,
        success: function (res) {
            
            if (res.Success && res.Data.length > 0) {
                app.psfprk.vue.getshop = res.Data;
                //if (app.psfprk.id_shop_rk_default != "") {
                //    app.psfprk.vue.selected = app.psfprk.id_shop_rk_default;
                //    app.psfprk.id_shop_rk_default = "";
                //} else {
                //    app.psfprk.vue.selected = res.Data[0].id_shop;
                //}
            }
            else {
                app.psfprk.vue.getshop = [];
            }
        },
        error: null,
        complete: null
    });
    app.psfprk.id_shop_change_count += 1;
}

//退货门店值发生变化触发事件
app.psfprk.shop_change_clear = function (e) {
    app.psfprk.removeAll();
    app.psfprk.setresult(true);
    
}

//删除返配出库单号
app.psfprk.clear_select_sqd = function () {
    var id_bill_origin = $(_ + "#id_bill_origin").val();
    if (id_bill_origin != '') {
        app.psfprk.removeAll();
        app.psfprk.setresult(true);
    }
    $(_ + "#dh_origin").val("");
    $(_ + "#bm_djlx_origin").val("");
    $(_ + "#id_bill_origin").val("");
    $(_ + "#remark").val("");
    $(_ + "#id_shop_ck").removeAttr("disabled");
    $(_ + "#id_shop").removeAttr("disabled");
    $("button:contains('导入')", _).removeClass('disabled').attr('onclick', 'app.jh.importin()');
    $("button:contains('扫描')", _).removeClass('disabled').attr('onclick', 'app.jh.sm(this)');
    $("#psfprk_shopsp_table a:contains('选择')", _).removeClass('disabled').attr('onclick', 'app.jh.showshopsp();');
    $("button:contains('增加行')", _).removeClass('disabled').attr('onclick', 'app.jh.addshopsp_row(this)');
    app.psfprk.vue.selected = '';
    $('#id_shop', _).removeAttr('disabled');
    $('#id_shop_ck', _).removeAttr('disabled');
    
}
