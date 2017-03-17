app.psck = app.psck || {};

$(function () {
    var id_shopjjl = $('#id_shop', _).val();
    app.psck.vue = new Vue({
        el: "#psck_edit",
        data: {
            id_shop_select: app.psck.id_shop_select,
            getshop: [],
            selected: '',
        },
        watch: {            
            //getshop: function () {
            //    app.psck.vue.changeSelect_rk();
            //}
        },
        methods: {
            getshopfunc: function () {
                _this = this;
                app.httpAjax.post({
                    data: { id: app.psck.id_shop_val },
                    headers: {},
                    url: $.DHB.U('/SearchCondition/GetPzxSubShop'),
                    type: "POST",
                    dataType: 'json',
                    beforeSend: null,
                    success: function (data) {
                        debugger;
                        _this.getshop = data.Data;
                        if (app.psck.option == 'add') {
                            //_this.selected = data.Data[0].id_shop;
                        } else {
                            _this.selected = app.psck.id_shop_select;
                        }
                    },
                    error: null, 
                    complete: null
                });
                //app.psck.vue.changeSelect_rk()
            },
            //changeSelect_rk: function () {
            //   debugger;
            //    console.log("aaa");
            //    $('#id_shop_rk',_).selectpicker();
            //}

        },
        

    });
    //门店名称初始化
    app.psck.vue.getshopfunc();
    
    
    
})
app.psck.add_ready = function () {
    
    $('.table_list', _).on('mouseleave', '.common_select', function () {
        $('.common_select_list', _).hide();
    });
    $('.table_list', _).on('click', '.common_select_list li', function () {

        var li_txt = $(this).html();
        $(this).parents('.common_select_list').hide();
        $(this).parents('.common_select').find('span').html(li_txt);
        $(this).parents('.common_select').find('span').attr('data-zhl', $(this).attr('data-zhl'));
        app.psck.dw_select_onchange(this);
    });
    //app.psck.select_shop_change($(_ + "#id_shop"));
    app.psck.setresult(true);
    $(_ + '#div_sm').keyup(function (e) {
        var id_shop = $(_ + "#id_shop_rk").val();
        if (id_shop=='') {
            $.DHB.message({ 'content': '请选择入库门店', 'time': 1000, 'type': 'i' });
            return;
        }
        var id_shop_ck = $(_ + "#id_shop").val();
        if (e.keyCode == 13) {
            if (!app.ps.is_can_exec_op_by_dh(_, "id_bill_origin")) {
                return;
            }
            var scan = $(_ + '#barcode_search').val();
            $(_ + '#barcode_search').val('');
            app.httpAjax.post({
                data: { keyword: scan, id_shop: id_shop, id_shop_ck: id_shop_ck },
                headers: {},
                url: '/shopsp/GetShopspListForPs',
                type: "POST",
                datatype: 'json',
                beforeSend: null,
                success: function (ret) {
                    if (ret.Success) {
                        $('#psck_shopsp_table>tbody tr').each(function () {
                            var tr = $(this);
                            if (tr.attr('data-item') == ret.Data.id_shopsp_ck) {
                                var old_sl = tr.find('input[name=sl]').val();
                                tr.find('input[name=sl]').val((parseFloat(old_sl) + 1).toFixed(2));
                                tr.find('input[name="sl"]').blur();
                                return false;
                            } else if (tr.attr('data-item') == "") {
                                tr.attr("data-item", ret.Data.id_shopsp_ck);
                                tr.find('td[name="mc"] div').text(ret.Data.mc);
                                tr.find('input[name="sl"]').val(limit_num('1', app.psck.sl_digit));
                                var dj_jh = ret.Data.dj_ps.toString() == "0" ? ret.Data.dj_jh.toString() : ret.Data.dj_ps.toString();
                                dj_jh = limit_num(dj_jh, app.psck.dj_digit);
                                tr.find('input[name="dj_jh"]').val(dj_jh);
                                tr.find('input[name="je"]').val(dj_jh);
                                //设置old-data
                                tr.find('input[name="barcode"]').attr('old-data', ret.Data.barcode);
                                tr.find('input[name="barcode"]').val(ret.Data.barcode);
                                tr.find('input[name="sl"]').attr('old-data', limit_num('1', app.psck.sl_digit));
                                tr.find('input[name="dj_jh"]').attr('old-data', dj_jh);
                                tr.find('input[name="je"]').attr('old-data', dj_jh);
                                tr.find('input[name="shopsp_obj"]').attr("value", ret.Data.id);
                                //转换率
                                var zhl = ret.Data.zhl.toString();
                                zhl = limit_num(zhl, app.psck.sl_digit);
                                //var dj_jh = ret.Data.dj_jh.toString();
                                //dj_jh = limit_num(dj_jh, app.psck.dj_digit);
                                tr.find('input[name="sl_zhl"]').val(zhl);
                                tr.find('input[name="shopsp_obj"]').attr("data-id_kcsp", ret.Data.id_kcsp);
                                tr.find('input[name="shopsp_obj"]').attr("data-id_sp", ret.Data.id_sp);
                                //绑定下拉
                                tr.find('div[name=dw_select]>div>span').html(ret.Data.dw);
                                tr.find('div[name=dw_select]>div>span').attr("data-zhl", ret.Data.zhl);
                                tr.find('div[name=dw_select]').css('display', 'block');
                                tr.find('ul').append('<li value=' + dj_jh + '  data-zhl=' + zhl + ' data-id_shopsp=' + ret.Data.id + ' data-id_kcsp=' + ret.Data.id_kcsp + ' data-id_sp=' + ret.Data.id_sp + ' data-dw=' + ret.Data.dw + '>' + ret.Data.dw + '</li>');
                                var tbody = tr.parents("tbody");
                                var data_item = tbody.find("tr:last").attr("data-item");
                                if (data_item == ret.Data.id) {
                                    app.psck.addshopsp();//新增最后一条新记录
                                    app.psck.reset_xh();
                                }
                                app.psck.setresult(false);
                                return false;
                            }
                        });
                    } else {
                        $.DHB.dialog({ 'title': '选择商品', 'url': $.DHB.U('shopsp/SearchForPs?keyword=' + scan + '&id_shop=' + id_shop + '&id_shop_ck=' + id_shop_ck), 'id': 'dialog-shopsp-searchforps', 'confirm': app.psck.dialogCallBack });
                    }
                },
                error: null,
                complete: null
            });

        }
    });

}
//选择原单单号触发事件
app.psck.select_ckd_bh = function () {
    var id_shop_rk = $(_ + "#id_shop_rk").val(); 
    var id_shop = $(_ + "#id_shop").val();
    //if (id_shop_ps=='') {
    //    $.DHB.message({ 'content': '请选择入库门店！', 'type': 'i' });
    //    return;
    //}
    app.psck.id_shop_change_source = "1";
    $.DHB.dialog({
        'title': '选择补货申请单',
        'url': $.DHB.U('pssq/searchlist?_callback_=app.psck.select_pssq_callback&bm_djlx_t=PS120&id_shop_sq=' + id_shop_rk + '&id_shop_pszx=' + id_shop),
        'id': 'dialog-pssq-search',
        'confirm': app.psck.select_pssq_callback
    });
}
//选择原单单号触发事件回调方法
app.psck.select_pssq_callback = function (array) {
    
    $("#dh_origin").val(array.dh);
    $("#bm_djlx_origin").val(array.bm_djlx);
    $("#id_bill_origin").val(array.id);
    if (array.id) {
        app.httpAjax.post({
            data: { id: array.id },
            headers: {},
            url: '/pssq/quersqdspmx',
            type: "POST",
            datatype: 'json',
            beforeSend: null,
            success: function (ret) { 
                console.log(ret);
                if (ret.Success) {
                    var shopsp = [];
                    if (ret.Data.Pssq2List.length > 0) {
                        for (var item in ret.Data.Pssq2List) {
                            var obj = ret.Data.Pssq2List[item];
                            var shopsp_e = {};
                            shopsp_e.id_shopsp = obj.id_shopsp;
                            shopsp_e.id_kcsp = obj.id_kcsp;
                            shopsp_e.id_shopsp_ck = obj.id_shopsp;
                            shopsp_e.id_kcsp_ck = obj.id_kcsp;
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
                    if (ret.Data.Pssq1) {
                        var idShop = window.localStorage.getItem('id_shop_pszx');
                       var index=$('#id_shop', _).find('option[value="' + idShop + '"]').index();
                       $('#id_shop', _).siblings('.dropdown-menu').find('ul li').eq(index).find('a').click();
                        app.psck.vue.selected = ret.Data.Pssq1.id_shop;                        
                        var bz = ret.Data.Pssq1.bz;
                        $(_ + "#remark").val(bz);
                    }
                    var jsonStr = JSON.stringify(shopsp);
                    app.psck.removeAll();
                    app.psck.dialogCallBackWork(jsonStr);
                    
                    app.psck.setresult(true);
                    $("button:contains('导入')", _).addClass('disabled').removeAttr('onclick');
                    $("button:contains('扫描')", _).addClass('disabled').removeAttr('onclick');
                    $("#psck_shopsp_table a:contains('选择')", _).addClass('disabled').removeAttr('onclick');
                    $("button:contains('增加行')", _).addClass('disabled').removeAttr('onclick');
                    $('#id_shop',_).attr('disabled', 'disabled');
                    $('#id_shop_rk',_).attr('disabled','disabled');
                }
            },
            error: null,
            complete: null
        });

    }
}

//删除原单单号触发事件
app.psck.clear_select_cktzd = function (el) { 
    var id_bill_origin = $(_ + "#id_bill_origin").val();
    if (id_bill_origin != '') {
        app.psck.removeAll();
        app.psck.setresult(true);
    }
    $(_ + "#dh_origin").val("");
    $(_ + "#bm_djlx_origin").val("");
    $(_ + "#id_bill_origin").val("");
    $(_ + "#remark").val("");
    $(_ + "#id_shop_rk").removeAttr("disabled");
    $("button:contains('导入')", _).removeClass('disabled').attr('onclick', 'app.psck.importin()');
    $("button:contains('扫描')", _).removeClass('disabled').attr('onclick', 'app.psck.sm(this)');
    $("#psck_shopsp_table a:contains('选择')", _).removeClass('disabled').attr('onclick', 'app.psck.showshopsp();');
    $("button:contains('增加行')", _).removeClass('disabled').attr('onclick', 'app.psck.addshopsp_row(this)');
    $('#id_shop', _).removeAttr("disabled");
    $('#id_shop_rk', _).removeAttr("disabled");
    app.psck.vue.selected = '';
}
//配送中心值发生变化触发事件
app.psck.select_shop_change = function (obj) {
    debugger;
    app.psck.clear_select_cktzd();
    
    app.psck.shop_change_clear();
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
                app.psck.vue.getshop = res.Data;
                if (app.psck.id_shop_rk_default != "") {
                    app.psck.vue.selected = app.psck.id_shop_rk_default;
                    app.psck.id_shop_rk_default = "";
                } else {
                    app.psck.vue.selected = res.Data[0].id_shop;
                }
            }
            else {
                app.psck.vue.getshop = [];
            }
        },
        error: null,
        complete: null
    });
    app.psck.id_shop_change_count += 1;
}
//入库门店值发生变化触发事件
app.psck.shop_change_clear = function () {
    
    if (app.psck.id_shop_rk_default == "") {
        app.psck.removeAll();
        app.psck.setresult(true);
    }
    app.psck.clear_select_cktzd();
}
//配送出库单清除表格数据
app.psck.removeAll = function () {
    if (app.psck.id_shop_change_source != "") {
        app.psck.id_shop_change_source = "";
        return;
    } else {
        $('#psck_shopsp_table>tbody', _).find('tr').each(function () {
            $(this).remove();
        });
    }

}