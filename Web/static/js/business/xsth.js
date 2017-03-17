app.xsth = app.xsth || {};
app.xsth.div_name = "xsth_add";



app.xsth.sl_digit=$(_+"#"+app.xsth.div_name+'  #sl_digit').val();
app.xsth.dj_digit = $(_ + "#" + app.xsth.div_name + '  #dj_digit').val();
app.xsth.je_digit = $(_ + "#" + app.xsth.div_name + '  #je_digit').val();
app.xsth.type = $(_ + "#" + app.xsth.div_name + '  #type').val();
app.xsth.head_id = $(_ + "#" + app.xsth.div_name + '  #head_id').val();
app.xsth.head_dh = $(_ + "#" + app.xsth.div_name + '  #head_dh').val();
app.xsth.head_je_st = $(_ + "#" + app.xsth.div_name + '  #head_je_st').val();
app.xsth.default_dj = $(_ + "#" + app.xsth.div_name + '  #default_dj').val();
app.xsth.default_sl = $(_ + "#" + app.xsth.div_name + '  #default_sl').val();
app.xsth.default_je = $(_ + "#" + app.xsth.div_name + '  #default_je').val();




$.DHB._ = function () {
    app.c.public_data['xsth/add'] = app.c.public_data['xsth/add'] || {};
    if (app.c.public_data['xsth/add']['once'] === false) {
        app.c.public_data['xsth/add']['once'] = true;
        $.DHB.checkForm(function () {

            if (!$(_ + "#" + app.xsth.div_name + " #kh_id").val()) {
                $.DHB.message({ 'content': '请选择客户！', 'type': 'i' });
            }
            else if ($(_ + "#"+app.xsth.div_name +" #shopsp_table_xsth>tbody>tr[data-item!='']").length <= 0) {
                $.DHB.message({ 'content': '请至少选择一个商品！', 'type': 'i' });
            }
            else if ($(_ + "#" + app.xsth.div_name + ' #je_st').val() == '' || $(_ + "#" + app.xsth.div_name + ' #je_st').val() == '') {
                $.DHB.message({ 'content': '实收金额不准为空！', 'type': 'i' });
            }
            else if (parseFloat($(_ + "#"+app.xsth.div_name +' #je_st').val()) > parseFloat($(_ + "#"+app.xsth.div_name +' #edit_goods_total').text())) {
                $.DHB.message({ 'content': '实收金额不准大于商品明细总金额！', 'type': 'i' });
            }
            else {
                

                app.xsth.setresult();
                var objData = {};
                objData.shopspList = $(_ + "#"+app.xsth.div_name +' input[name="table_result"]').val();
                objData.remark = $(_ + "#"+app.xsth.div_name +' #remark').val();
                objData.id_kh = $(_ + "#" + app.xsth.div_name + ' #kh_id').val();
                objData.id_shop = $(_ + "#"+app.xsth.div_name +' #id_shop').val();
                objData.id_jbr = $(_ + "#"+app.xsth.div_name +' #id_jbr').val();
                objData.dh = $(_ + "#"+app.xsth.div_name +' #dh').val();
                objData.rq = $(_ +"#"+app.xsth.div_name +' #rq').val();
                objData.je_st = $(_ + "#"+app.xsth.div_name +' #je_st').val();
                objData.type =app.xsth.type;
                objData.id = app.xsth.head_id;

                objData.dh_origin = $(_ + "#"+app.xsth.div_name +' #dh_origin').val();
                objData.bm_djlx_origin = $(_ +"#"+app.xsth.div_name +' #bm_djlx_origin').val();
                objData.id_bill_origin = $(_ + "#"+app.xsth.div_name +' #id_bill_origin').val();
  
                var options = {
                    data: objData,
                    url: $.DHB.U('xsth/add'),
                    type: "POST",
                    datatype: 'json',
                    beforeSend: function () { },
                    success: function (data, textStatus, jqXHR) {
                        if (data.status == "success") {
                            $.DHB.message({ 'content': data.message, 'time': 4000, 'type': 's' });
                            if ($(_ + "#" + app.xsth.div_name + ' #add_new').val() == '0') {
                                $.fn.menuTab.deleteMenu('xsth/add');
                                $.DHB.url('xsth/list', 'cache', '销售退货');
                            }
                            else {
                                $.fn.menuTab.deleteMenu('xsth/add');
                                $.fn.menuTab.load({ url: '/xsth/add', 'title': '新增销售退货', id: 'xsth/add', nocache: '0' });

                            }
                        } else {
                            $.DHB.message({ 'content': data.message, 'time': 0, 'type': 'e' });
                        }
                    },
                    complete: function (XHR, TS) { }
                };
                app.httpAjax.post(options);




            }
            return false;
        });
        $(function () {
            $(_ + '#submit-button').removeAttr('disabled');
        });
    }
};

//默认设置单号
cy.http.Post({
    url: '/xsth/createdh',
    beforeSend: function () {
    },
    callback: function (ret) {
        if (ret.Success) {
            if (app.xsth.type != 'edit') {
                $(_ + "#"+app.xsth.div_name +" #dh").val(ret.Data);
                $(_ + "#"+app.xsth.div_name +" #dh").attr('old-data', ret.Data);
            } else {
                $(_ + "#" + app.xsth.div_name + " #dh").val(app.xsth.head_dh);
                $(_ + "#" + app.xsth.div_name + " #dh").attr('old-data', app.xsth.head_dh);
            }
        }
    },
    complete: function () {

    }
});




app.xsth.add_add_kh_callback = function (selectVal) {
    selectVal = selectVal || '';
    var selectValTem = selectVal.split('|');
    if (selectValTem[0] != 'undefined' && selectValTem[0] != '') {
        $("#" + app.xsth.div_name + ' #kh_id', _).val(selectValTem[0]);
    }
    if (selectValTem[1] != 'undefined' && selectValTem[1] != '') {
        $("#" + app.xsth.div_name + ' #kh_name', _).val(selectValTem[1]);
    }
}


app.xsth.dialogCallBack = function (array) {
    debugger;
    var jsonStr = "";
    $.each(array, function (index, item) {
        if (item.name == "shopsp_table_json") {
            jsonStr = item.value;
        }
    });

    var id_bill_origin = $(_ + "#"+app.xsth.div_name +" #id_bill_origin").val();
    if (id_bill_origin != '') {
        $.DHB.message({ 'content': '你已经选择关联的销售出库单,不允许再添加其他商品,若需要自行添加,请删除关联的销售出库单！', 'time': 4000, 'type': 'i' });
    } else {
        app.xsth.dialogCallBackWork(jsonStr);
    }

    //关闭dialog
    $.DHB.dialog({ 'id': 'dialog-shopsp-search', 'action': 'destroy' });
    app.xsth.setresult(true);
}

//选择商品回调
app.xsth.dialogCallBackWork = function (jsonStr) {
    debugger;
    var data_item_last = '';
    var objList = jQuery.parseJSON(jsonStr);
    $(objList).each(function (i, obj) {

        debugger;

        var guid = parseInt($(_ + "#"+app.xsth.div_name +" #shopspnum").val()) + 1;
        var option = '';
        var resultHtml = '';
        resultHtml += '<tr data-item="' + obj.id_shopsp + '">';
        resultHtml += '<td class="align_center">  <div name="xh">' + guid + '</div>  <input class="form-control user-input" style="width:110px;display:none;" placeholder="商品ID" name="shopsp_obj" type="text" value="' + obj.id_shopsp + '"  data-id_kcsp="' + obj.id_kcsp + '"   data-id_sp="' + obj.id_sp + '" ></td>';
        resultHtml += ' <td><input class="form-control user-input col-md-2" style="border:0px;" onmouseover="app.xsth.onmouseover(this)" onmouseout="app.xsth.onmouseout(this)" type="text" value="' + obj.barcode + '" name="barcode" old-data="' + obj.barcode + '"></td>';
        resultHtml += '<td name="mc"> <div style="width: 100% !important;text-align:left;padding:0 0;"> <span>' + obj.mc + '</span><a href="javascript:void(0)" onclick="app.xsth.showshopsp();" class="btn btn-info f-r">  选择</a></div> </td>';

        var sl_kc = 0;

        if (typeof (obj.dw) != 'undefined' && obj.dw != 'undefined' && obj.dw != '') {
            resultHtml += '<td name="dw">    <div id="dw_select" name="dw_select"  class="common_select" style="display:block;"><div onclick="app.xsth.dw_select_xsck_onclick(this)"><span class="inline" data-zhl="' + limit_num(obj.zhl.toString(), app.xsth.sl_digit) + '">' + obj.dw + '</span> <i class="fa fa-caret-down"></i></div><ul class="common_select_list"><li value="' + limit_num(obj.dj_pf.toString(), app.xsth.dj_digit) + '"  data-zhl="' + limit_num(obj.zhl.toString(), app.xsth.sl_digit) + '"    data-id_shopsp="' + obj.id_shopsp + '"   data-id_kcsp="' + obj.id_kcsp + '"  data-id_sp="' + obj.id_sp + '"  data-dw="' + obj.dw + '"    data-mc="' + obj.mc + '" data-barcode="' + obj.barcode + '" data-sl_kc="' + sl_kc + '"  >' + obj.dw + '</li></ul></div></td>';
        } else {
            resultHtml += '<td name="dw">    <div id="dw_select" name="dw_select"   class="common_select"><div onclick="app.xsth.dw_select_xsck_onclick(this)"><span class="inline"></span> <i class="fa fa-caret-down"></i></div><ul value="" class="common_select_list"></ul></div></td>';
        }
        var sl = limit_num(obj.sl.toString(), app.xsth.sl_digit);
        //var dj = limit_num(obj.dj_jh.toString(), app.xsth.dj_digit);
        var dj = limit_num(obj.dj_pf.toString(), app.xsth.dj_digit);

        var je = accMul(parseFloat(dj), parseFloat(sl));
        if (typeof (obj.je) != 'undefined' && obj.je != 'undefined' && obj.je != '') {
            je = obj.je;
        }
        je = limit_num(je.toString(), app.xsth.je_digit);

        resultHtml += '<td><input class="form-control user-input" style="width:96%;border:0px;text-align:right;" onmouseover="app.xsth.onmouseover(this)" onmouseout="app.xsth.onmouseout(this)" placeholder="数量" type="text" value="' + sl + '" name="sl" onkeyup="check_digit(this,\'' + app.xsth.sl_digit + '\')" onafterpaste="check_digit(this,\'' + app.xsth.sl_digit + '\')" old-data="' + sl + '">    <input name="sl_zhl" style="width:95%;display:none;text-align:right;" placeholder="转换率" type="text" value="1">  <input name="sl_total" style="width:95%;display:none;text-align:right;" placeholder="数量总数" type="text" value="' + app.xsth.default_sl + '"></td>';
        resultHtml += ' <td><input class="form-control user-input" style="width:96%;border:0px;text-align:right;" onmouseover="app.xsth.onmouseover(this)" onmouseout="app.xsth.onmouseout(this)" placeholder="单价(元)" type="text" value="' + dj + '" name="dj_jh" old-data="' + dj + '"   onkeyup="check_digit(this,\''+app.xsth.dj_digit+'\')" onafterpaste="check_digit(this,\''+app.xsth.dj_digit+'\')" ></td>';
        resultHtml += ' <td name="je"> <input class="form-control user-input" style="width:96%;border:0px;text-align:right;" onmouseover="app.xsth.onmouseover(this)" onmouseout="app.xsth.onmouseout(this)" placeholder="金额(元)" type="text" value="' + je + '" name="je" old-data="' + je + '"   onkeyup="check_digit(this,\''+app.xsth.je_digit+'\')" onafterpaste="check_digit(this,\''+app.xsth.je_digit+'\')" ></td>';
        if (typeof (obj.bz) != 'undefined' && obj.bz != 'undefined' && obj.bz != '') {
            resultHtml += ' <td><input class="form-control user-input" style="width:96%;border:0px;" onmouseover="app.xsth.onmouseover(this)" onmouseout="app.xsth.onmouseout(this)"  type="text" value="' + obj.bz + '" name="bz"></td>';
        } else {
            resultHtml += ' <td><input class="form-control user-input" style="width:96%;border:0px;" onmouseover="app.xsth.onmouseover(this)" onmouseout="app.xsth.onmouseout(this)" type="text" value="" name="bz"></td>';
        }
        resultHtml += '<td class="align_center"><a onclick="app.xsth.choice_spec_del(this);" class="bg-state bg-state-info blockbtn fa fa-trash" title="删除"></a></td>';
        resultHtml += ' </tr>';
        $(_ + "#" + app.xsth.div_name + " #shopspnum").val(guid);
        $(_ + "#" + app.xsth.div_name + " #shopsp_table_xsth>tbody").append(resultHtml);
        app.xsth.bindkeypresslast();
        data_item_last = obj.id_shopsp;
    });
}

app.xsth.addshopsp_row = function (obj) {
    app.xsth.addshopsp();
    reset_xh();
};


//添加商品至table
app.xsth.addshopsp = function (obj) {
    var shopsp = [];
    var shopsp_e = {};
    debugger;
    shopsp_e.id_shopsp = "";
    shopsp_e.id_kcsp = "";
    shopsp_e.id_sp = "";
    shopsp_e.barcode = "";
    shopsp_e.mc = "";
    shopsp_e.dw = "";
    shopsp_e.zhl = "1";
    shopsp_e.dj_jh = app.xsth.default_dj;
    shopsp_e.sl = app.xsth.default_sl;
    shopsp_e.dj_pf = app.xsth.default_dj;
    shopsp.push(shopsp_e);
    var jsonStr = JSON.stringify(shopsp);
    app.xsth.dialogCallBackWork(jsonStr);
}

//删除商品的数据
app.xsth.choice_spec_del = function (el) {
    $(el).parents('tr').remove();
    app.xsth.setresult(false);
}

//改动后重新计算和赋值
app.xsth.setresult = function (ifRemove) {
    debugger;
    var e = [];
    var shopsp = [];
    var totleMoney = 0;
    var xh = 1;

    if (ifRemove) {
        $("#" + app.xsth.div_name + ' #shopsp_table_xsth>tbody').find('tr[data-item=""]').each(function () {
            $(this).remove();
        });
    }

    $("#" + app.xsth.div_name + ' #shopsp_table_xsth>tbody').find('tr').each(function () {
        $(this).find('div[name="xh"]').text(xh);
        var shopsp_e = {};
        var shopsp_obj = $(this).find('input[name="shopsp_obj"]');
        shopsp_e.id_shopsp = shopsp_obj.val();
        shopsp_e.id_kcsp = $.trim(shopsp_obj.attr("data-id_kcsp"));//仓库id
        shopsp_e.id_sp = $.trim(shopsp_obj.attr("data-id_sp"));//商品id
        shopsp_e.sl = $.trim($(this).find('input[name="sl"]').val()); //数量
        shopsp_e.dj = $.trim($(this).find('input[name="dj_jh"]').val()); //单价
        shopsp_e.barcode = $.trim($(this).find('input[name="barcode"]').val());//条码
        shopsp_e.dw = $(this).find('div[name=dw_select]>div>span').html();//单位
        shopsp_e.bz = $.trim($(this).find('input[name="bz"]').val());//备注
        shopsp_e.zhl = $.trim($(this).find('div[name=dw_select]>div>span').attr('data-zhl')); //转换率
        shopsp_e.shopsp_name = $(this).find('td[name="mc"] div span').text();;//商品名称
        //数量总数
        var sl_total = accMul(parseFloat(shopsp_e.zhl), parseFloat(shopsp_e.sl));                     //(parseFloat(shopsp_e.zhl) * parseFloat(shopsp_e.sl));
        sl_total = limit_num(sl_total.toString(), app.xsth.sl_digit)
        $(this).find('input[name="sl_total"]').val(sl_total);
        shopsp_e.sl_total = $.trim(sl_total);
        shopsp_e.je = $.trim($(this).find('input[name="je"]').val()); //金额
        if (shopsp_e.id_shopsp != '') {
            shopsp.push(shopsp_e);
        }
        xh++;
        //当前金额
        var moneyTotal = limit_num(shopsp_e.je.toString(), app.xsth.dj_digit)
        //总金额
        totleMoney = accAdd(totleMoney, parseFloat($.trim(moneyTotal)));
    });

    if (ifRemove) {
        $(_ + "#" + app.xsth.div_name + " #shopspnum").val(parseInt(xh) - 1);
        app.xsth.addshopsp();
        app.xsth.addshopsp();
        app.xsth.addshopsp();
        app.xsth.addshopsp();
        app.xsth.addshopsp();
        app.xsth.addshopsp();
        app.xsth.addshopsp();
        app.xsth.addshopsp();
        app.xsth.addshopsp();
        app.xsth.addshopsp();
        reset_xh();
    }

    totleMoney = limit_num(totleMoney.toString(), app.xsth.je_digit)
    $(_ + "#" + app.xsth.div_name + ' #edit_goods_total').text(totleMoney);
    var jsonStr = JSON.stringify(shopsp);
    $(_ + "#" + app.xsth.div_name + " #table_result").val(jsonStr);

}




//绑定事件
app.xsth.bindkeypress = function (obj) {
    $("#" + app.xsth.div_name + ' #shopsp_table_xsth>tbody',_).find('tr').each(function () {
        bindEvent($(this));
    });
}

//默认绑定事件
app.xsth.bindkeypress();

//绑定最后一条tr的事件
app.xsth.bindkeypresslast = function () {
    $("#" + app.xsth.div_name + " #shopsp_table_xsth>tbody>tr:last",_).each(function () {
        bindEvent($(this));
    });
};

//绑定事件执行
function bindEvent(ele) {
    ele.each(function () {
        var barcode_obj = $(this).find('input[name="barcode"]');
        var sl_obj = $(this).find('input[name="sl"]');
        var dj_ls_obj = $(this).find('input[name="dj_jh"]');
        var bz_obj = $(this).find('input[name="bz"]');
        var je_obj = $(this).find('input[name="je"]');

        barcode_obj.keyup(function (e) {
            var tr = barcode_obj.parents("tr");
            if (e.keyCode == 13) {
                var data_item_tr = tr.attr("data-item");
                if (data_item_tr != 'undefined' && data_item_tr != '') {
                    //已经获取过数据 此时找数量焦点
                    sl_obj.focus().select();
                }
                else {
                    var id_shop = $(_ + "#id_shop").val();
                    //Post读取数据赋值
                    cy.http.Post({
                        url: '/shopsp/GetShopspList',
                        data: { keyword: barcode_obj.val(), id_shop: id_shop },
                        beforeSend: function () {
                        },
                        callback: function (ret) {
                            if (ret.Success) {
                         
                                //console.log(ret);
                                tr.attr("data-item", ret.Data.id);
                                tr.find('td[name="mc"] div span').text(ret.Data.mc);
                                tr.find('input[name="sl"]').val(limit_num('1', app.xsth.sl_digit));
                                //tr.find('input[name="dj_jh"]').val(limit_num(ret.Data.dj_jh.toString(), app.xsth.dj_digit));
                                tr.find('input[name="dj_jh"]').val(limit_num(ret.Data.dj_pf.toString(), app.xsth.dj_digit));
                                tr.find('input[name="je"]').val(limit_num(ret.Data.dj_pf.toString(), app.xsth.dj_digit));
                                //设置old-data
                                barcode_obj.attr('old-data', barcode_obj.val());
                                sl_obj.attr('old-data', limit_num('1', app.xsth.sl_digit));
                                dj_ls_obj.attr('old-data', limit_num(ret.Data.dj_jh.toString(), app.xsth.dj_digit));
                                je_obj.attr('old-data', limit_num(ret.Data.dj_jh.toString(), app.xsth.dj_digit));
                                tr.find('input[name="shopsp_obj"]').attr("value", ret.Data.id);
                                tr.find('input[name="sl"]').focus().select();
                                //转换率
                                var zhl = ret.Data.zhl.toString();
                                zhl = limit_num(zhl, app.xsth.sl_digit)
                                //var dj_jh = ret.Data.dj_jh.toString();
                                var dj_jh = ret.Data.dj_pf.toString();
                                dj_jh = limit_num(dj_jh, app.xsth.dj_digit)
                                tr.find('input[name="sl_zhl"]').val(zhl);
                                tr.find('input[name="shopsp_obj"]').attr("data-id_kcsp", ret.Data.id_kcsp);
                                tr.find('input[name="shopsp_obj"]').attr("data-id_sp", ret.Data.id_sp);
                                //绑定下拉
                                tr.find('div[name=dw_select]>div>span').html(ret.Data.dw);
                                tr.find('div[name=dw_select]>div>span').attr("data-zhl", ret.Data.zhl);
                                tr.find('div[name=dw_select]').css('display', 'block');
                                tr.find('ul').append('<li value=' + dj_jh + ' data-mc=' + ret.Data.mc + ' data-barcode=' + ret.Data.barcode + '  data-zhl=' + zhl + ' data-id_shopsp=' + ret.Data.id + ' data-id_kcsp=' + ret.Data.id_kcsp + '  data-id_sp=' + ret.Data.id_sp + ' data-dw=' + ret.Data.dw + ' data-dj_ls="' + ret.Data.dj_ls + '">' + ret.Data.dw + '</li>');

                                var tbody = tr.parents("tbody");
                                var data_item = tbody.find("tr:last").attr("data-item");
                                if (data_item == ret.Data.id) {
                                    app.xsth.addshopsp();//新增最后一条新记录
                                    reset_xh();
                                }
                                app.xsth.setresult(false);
                            } else {
                                $.DHB.dialog({ 'title': '选择商品', 'url': $.DHB.U('shopsp/search?keyword=' + barcode_obj.val() + '&id_shop=' + $(_ + '#id_shop').val()), 'id': 'dialog-shopsp-search', 'confirm': app.xsth.dialogCallBack });
                            }
                        },
                        complete: function () { }
                    });
                }
            }
            else if (e.keyCode == 38) {//上
                var tr_prev = tr.prev('tr');
                if (tr_prev.length > 0) {
                    tr_prev.find('input[name="barcode"]').focus().select();
                }
            }
            else if (e.keyCode == 40) {//下
                var tr_next = tr.next('tr');
                if (tr_next.length > 0) {
                    tr_next.find('input[name="barcode"]').focus().select();
                }
            }
        });
        sl_obj.keyup(function (e) {
            var tr = sl_obj.parents("tr");
            if (e.keyCode == 13) {//回车
                dj_ls_obj.focus().select();
            }
            else if (e.keyCode == 38) {//上
                var tr_prev = tr.prev('tr');
                if (tr_prev.length > 0) {
                    tr_prev.find('input[name="sl"]').focus().select();
                }
            }
            else if (e.keyCode == 40) {//下
                var tr_next = tr.next('tr');
                if (tr_next.length > 0) {
                    tr_next.find('input[name="barcode"]').focus().select();
                }
            }
        });
        dj_ls_obj.keyup(function (e) {
            var tr = dj_ls_obj.parents("tr");

            if (e.keyCode == 13) {//回车
                je_obj.focus().select();
            }
            else if (e.keyCode == 38) {//上
                var tr_prev = tr.prev('tr');
                if (tr_prev.length > 0) {
                    tr_prev.find('input[name="dj_jh"]').focus().select();
                }
            }
            else if (e.keyCode == 40) {//下
                var tr_next = tr.next('tr');
                if (tr_next.length > 0) {
                    tr_next.find('input[name="barcode"]').focus().select();
                }
            }
        });
        je_obj.keyup(function (e) {
            var tr = je_obj.parents("tr");
            if (e.keyCode == 13) {//回车
                bz_obj.focus().select();
            }
            else if (e.keyCode == 38) {//上
                var tr_prev = tr.prev('tr');
                if (tr_prev.length > 0) {
                    tr_prev.find('input[name="je"]').focus().select();
                }
            }
            else if (e.keyCode == 40) {//下
                var tr_next = tr.next('tr');
                if (tr_next.length > 0) {
                    tr_next.find('input[name="barcode"]').focus().select();
                }
            }
            
        });
        bz_obj.keyup(function (e) {
            var tr = bz_obj.parents("tr");
            if (e.keyCode == 13) {//回车
                var tr_next = tr.next('tr');
                if (tr_next.length > 0) {
                    tr_next.find('input[name="barcode"]').focus().select();
                } else {

                    var data_item_tr = tr.attr("data-item");
                    if (data_item_tr != 'undefined' && data_item_tr != '') {
                        app.xsth.addshopsp();
                        reset_xh();
                        tr.next('tr').find('input[name="barcode"]').focus().select();
                    } else {
                        barcode_obj.focus().select();
                    }
                }
            }
            else if (e.keyCode == 38) {//上
                var tr_prev = tr.prev('tr');
                if (tr_prev.length > 0) {
                    tr_prev.find('input[name="bz"]').focus().select();
                }
            }
            else if (e.keyCode == 40) { //下
                var tr_next = tr.next('tr');
                if (tr_next.length > 0) {
                    tr_next.find('input[name="barcode"]').focus().select();
                }
            }
        });
        barcode_obj.focus(function () {
            $(this).select();
        });
        sl_obj.focus(function () {
            $(this).select();
        });
        dj_ls_obj.focus(function () {
            $(this).select();
        });
        je_obj.focus(function () {
            $(this).select();
        });
        bz_obj.focus(function () {
            $(this).select();
        });
        sl_obj.blur(function () {
            //
            var tr = sl_obj.parents("tr");
            var data_item = tr.attr('data-item');
            if (typeof (data_item) != 'undefined' && data_item != 'undefined' && data_item != '') {
                //数量变了金额变
                var dj = limit_num($.trim(tr.find('input[name="dj_jh"]').val()), app.xsth.dj_digit); //单价
                var sl = limit_num($.trim(tr.find('input[name="sl"]').val()),app.xsth.sl_digit); //数量
                var old_je = limit_num($.trim(tr.find('input[name="je"]').val()), app.xsth.je_digit); //旧金额
                var je = accMul(parseFloat(dj), parseFloat(sl));//金额
                //如果旧金额除以数量取小数后等于现在的单价 金额则不变
                if (parseFloat(sl) != 0) {
                    //var dj_temp = (parseFloat(old_je) / parseFloat(sl));
                    var dj_temp = accDiv(parseFloat(old_je), parseFloat(sl));
                    dj_temp = limit_num($.trim(dj_temp),app.xsth.dj_digit);
                    if (parseFloat(dj_temp) == parseFloat(dj)) {
                        je = old_je;
                    }
                }
                je = limit_num($.trim(je), app.xsth.je_digit);
                tr.find('input[name="je"]').val(je);
                tr.find('input[name="je"]').attr('old-data', je);
                tr.find('input[name="sl"]').val(sl);
                tr.find('input[name="sl"]').attr('old-data', sl);
                app.xsth.setresult(false);
            } else {
                sl_obj.val(app.xsth.default_sl);
            }

        });
        dj_ls_obj.blur(function () {
            var tr = dj_ls_obj.parents("tr");
            var data_item = tr.attr('data-item');
            if (typeof (data_item) != 'undefined' && data_item != 'undefined' && data_item != '') {
                //单价变了金额变
                var dj = limit_num($.trim(tr.find('input[name="dj_jh"]').val()),app.xsth.dj_digit); //单价
                var sl = limit_num($.trim(tr.find('input[name="sl"]').val()), app.xsth.sl_digit); //数量
                var old_je = limit_num($.trim(tr.find('input[name="je"]').val()), app.xsth.je_digit); //旧金额
                var je = accMul(parseFloat(dj), parseFloat(sl));//金额

                //如果旧金额除以数量取小数后等于现在的单价 金额则不变
                if (parseFloat(sl) != 0) {
                    var dj_temp = accDiv(parseFloat(old_je), parseFloat(sl));
                    dj_temp = limit_num($.trim(dj_temp), app.xsth.dj_digit);
                    if (parseFloat(dj_temp) == parseFloat(dj)) {
                        je = old_je;
                    }
                }
                je = limit_num($.trim(je), app.xsth.je_digit);
                tr.find('input[name="je"]').val(je);
                tr.find('input[name="je"]').attr('old-data', je);
                tr.find('input[name="dj_jh"]').val(dj);
                tr.find('input[name="dj_jh"]').attr('old-data', dj);
                app.xsth.setresult(false);
            } else {
                dj_ls_obj.val(app.xsth.default_dj);
            }
        });
        je_obj.blur(function () {
            var tr = je_obj.parents("tr");
            var data_item = tr.attr('data-item');
            if (typeof (data_item) != 'undefined' && data_item != 'undefined' && data_item != '') {
                //金额变了单价变

                var old_dj = limit_num($.trim(tr.find('input[name="dj_jh"]').val()), app.xsth.dj_digit); //旧单价
                var je = limit_num($.trim(tr.find('input[name="je"]').val()), app.xsth.je_digit); //金额
                var sl = limit_num($.trim(tr.find('input[name="sl"]').val()), app.xsth.sl_digit); //数量
                var dj = app.xsth.default_sj;//单价

                if (parseFloat(sl) != 0) {
                    var temp_je = accMul(parseFloat(old_dj), parseFloat(sl));//金额
                    temp_je = limit_num($.trim(temp_je),app.xsth.je_digit);
                    if (je == temp_je) {
                        dj = old_dj;
                    } else {
                        var dj_temp = accDiv(parseFloat(je), parseFloat(sl));
                        dj_temp = limit_num($.trim(dj_temp),app.xsth.dj_digit);
                        dj = dj_temp;
                    }

                }

                tr.find('input[name="dj_jh"]').val(dj);
                tr.find('input[name="dj_jh"]').attr('old-data', dj);
                tr.find('input[name="je"]').val(je);
                tr.find('input[name="je"]').attr('old-data', je);
                app.xsth.setresult(false);
            } else {
                je_obj.val(app.xsth.default_je);
            }
        });
        barcode_obj.blur(function () {
            var tr = barcode_obj.parents("tr");
            var data_item = tr.attr('data-item');
            if (typeof (data_item) != 'undefined' && data_item != 'undefined' && data_item != '') {
                var old_data = barcode_obj.attr('old-data');
                barcode_obj.val(old_data);
            }
        });

    });
}

//单位下拉选中改变事件
app.xsth.dw_select_onchange = function (e) {
    var tr = $(e).parents("tr");
    var zhl = $(e).attr('data-zhl');
    if (typeof (zhl) != 'undefined' && zhl != 'undefined' && zhl != '') {
        tr.find('input[name="sl_zhl"]').val(zhl);
    }

    var id_shopsp = $(e).attr('data-id_shopsp');
    if (typeof (id_shopsp) != 'undefined' && id_shopsp != 'undefined' && id_shopsp != '') {
        tr.attr("data-item", id_shopsp);
        tr.find('input[name="shopsp_obj"]').attr("value", id_shopsp);

    }

    var id_kcsp = $(e).attr('data-id_kcsp');
    if (typeof (id_kcsp) != 'undefined' && id_kcsp != 'undefined' && id_kcsp != '') {
        tr.find('input[name="shopsp_obj"]').attr("data-id_kcsp", id_kcsp);
    }

    var id_sp = $(e).attr('data-id_sp');
    if (typeof (id_sp) != 'undefined' && id_sp != 'undefined' && id_sp != '') {
        tr.find('input[name="shopsp_obj"]').attr("data-id_sp", id_sp);
    }

    //var dj = limit_num($.trim(e.value).toString(), app.xsth.dj_digit);//单价
    var dj = limit_num($(e).attr('value').toString(), app.xsth.dj_digit);//单价
    var sl = limit_num($.trim(tr.find('input[name="sl"]').val()).toString(), app.xsth.sl_digit); //数量
    var je = accMul(parseFloat(dj), parseFloat(sl));
    je = limit_num($.trim(je).toString(), app.xsth.je_digit);//金额
    tr.find('input[name="dj_jh"]').val(dj);
    tr.find('input[name="dj_jh"]').attr('old-data', dj);
    tr.find('input[name="je"]').val(je);
    tr.find('input[name="je"]').attr('old-data', je);

    var barcode = $(e).attr('data-barcode');
    if (typeof (barcode) != 'undefined' && barcode != 'undefined' && barcode != '') {
        tr.find('input[name="barcode"]').val(barcode);
    }

    var mc = $(e).attr('data-mc');
    if (typeof (mc) != 'undefined' && mc != 'undefined' && mc != '') {
        tr.find('td[name="mc"] div span').text(mc)
    }

    app.xsth.setresult(false);
};

//点击单位下拉事件
app.xsth.dw_select_xsck_onclick = function (e) {
    //当选中销售出库单时 不允许下拉改变单位
    var id_shop = $(_ + "#id_shop").val();
    var id_bill_origin=$("#" + app.xsth.div_name + ' #id_bill_origin', _).val();
    if (typeof (id_bill_origin) != 'undefined' && id_bill_origin != '' ) {
        return;
    }
    var $select = $(e);
    var tr = $(e).parents("tr");

    var data_item = tr.attr('data-item');
    if (typeof (data_item) != 'undefined' && data_item != 'undefined' && data_item != '') {
        var dw_select = tr.find('td[name="dw"] div[name="dw_select"] ul');
        var data_item_select = dw_select.attr('data-item');
        if (typeof (data_item_select) == 'undefined' || data_item_select == 'undefined' || data_item_select == '' || data_item_select == '0') {
            //Post读取数据赋值
            cy.http.Post({
                url: '/shopsp/GetShopspDwList',
                data: { select_id_sp: data_item,ps_id_shop: id_shop  },
                beforeSend: function () {
                },
                callback: function (ret) {
                    //console.log(ret);
                    if (ret.Success) {
                        var str_li = "";
                        if (ret.Data.length > 0) {
                            for (var item in ret.Data) {

                                if (ret.Data[item].id != data_item) {
                                    var zhl = ret.Data[item].zhl.toString();
                                    zhl = limit_num(zhl, app.xsth.sl_digit)
                                    var dj_jh = ret.Data[item].dj_pf.toString();
                                    dj_jh = limit_num(dj_jh, app.xsth.dj_digit)

                                    //var sl_kc = ret.Data[item].sl_qm.toString();
                                    //var sl_kc = limit_num(sl_kc, app.dh.sl_digit);
                                    var sl_kc = 0;

                                    str_li+='<li  value=' + dj_jh + '  data-mc=' + ret.Data[item].mc + ' data-barcode=' + ret.Data[item].barcode + ' data-sl_kc=' + sl_kc + '   data-zhl=' + zhl + ' data-id_shopsp=' + ret.Data[item].id + ' data-id_kcsp=' + ret.Data[item].id_kcsp + ' data-id_sp=' + ret.Data[item].id_sp + '  > ' + ret.Data[item].dw + '</li> ';

                                }
                            }
                            dw_select.append(str_li);
                            dw_select.attr('data-item', '1');
                            $select.siblings('ul').show();
                        }
                    }
                },
                complete: function () { }

            });
        } else {
            $select.siblings('ul').show();
        }

    } else {
        $select.siblings('ul').show();
    }
    

};

app.xsth.add_ready = function () {
    debugger;

    app.xsth.div_name = "xsth_add";
    app.xsth.sl_digit = $(_ + "#" + app.xsth.div_name + '  #sl_digit').val();
    app.xsth.dj_digit = $(_ + "#" + app.xsth.div_name + '  #dj_digit').val();
    app.xsth.je_digit = $(_ + "#" + app.xsth.div_name + '  #je_digit').val();
    app.xsth.type = $(_ + "#" + app.xsth.div_name + '  #type').val();
    app.xsth.head_id = $(_ + "#" + app.xsth.div_name + '  #head_id').val();
    app.xsth.head_dh = $(_ + "#" + app.xsth.div_name + '  #head_dh').val();
    app.xsth.head_je_st = $(_ + "#" + app.xsth.div_name + '  #head_je_st').val();
    app.xsth.default_dj = $(_ + "#" + app.xsth.div_name + '  #default_dj').val();
    app.xsth.default_sl = $(_ + "#" + app.xsth.div_name + '  #default_sl').val();
    app.xsth.default_je = $(_ + "#" + app.xsth.div_name + '  #default_je').val();


    $(_ + "#" + app.xsth.div_name + " #id_shop_sh_hid").val($(_ + "#" + app.xsth.div_name + ' #id_shop').val());

    $("#" + app.xsth.div_name + ' .table_list', _).on('mouseleave', '.common_select', function () {
        $('.common_select_list', _).hide();
    });
    $("#" + app.xsth.div_name + ' .table_list', _).on('click', '.common_select_list li', function () {

        var li_txt = $(this).html();
        $(this).parents('.common_select_list').hide();
        $(this).parents('.common_select').find('span').html(li_txt);
 
        $(this).parents('.common_select').find('span').attr('data-zhl', $(this).attr('data-zhl'));
        app.xsth.dw_select_onchange(this);
    });
    app.xsth.setresult(true);

    $(_ + "#" + app.xsth.div_name + ' #barcode_search').keyup(function (e) {
        if (e.keyCode == 13) {
            var scan = $(_ + "#" + app.xsth.div_name + ' #barcode_search').val();
            $(_ + "#" + app.xsth.div_name + ' #barcode_search').val('');
            var id_shop = $(_ + "#id_shop").val();
                //Post读取数据赋值
                cy.http.Post({
                    url: '/shopsp/GetShopspList',
                    data: { keyword: scan, id_shop: id_shop },
                    beforeSend: function () {
                    },
                    callback: function (ret) {
                        if (ret.Success) {
                            
                            $("#" + app.xsth.div_name + ' #shopsp_table_xsth>tbody tr').each(function () {
                                var tr = $(this);
                                if (tr.attr('data-item') == ret.Data.id) {
                                    var old_sl = tr.find('input[name=sl]').val();
                                    tr.find('input[name=sl]').val((parseFloat(old_sl) + 1).toFixed(2));
                                    tr.find('input[name="sl"]').blur();
                                    return false;
                                } else if (tr.attr('data-item') == "") {
                                    tr.attr("data-item", ret.Data.id);
                                    tr.find('td[name="mc"] div span').text(ret.Data.mc);
                                    tr.find('input[name="sl"]').val(limit_num('1', app.xsth.sl_digit));
                                    //tr.find('input[name="dj_jh"]').val(limit_num(ret.Data.dj_jh.toString(), app.xsth.dj_digit));
                                    tr.find('input[name="dj_jh"]').val(limit_num(ret.Data.dj_pf.toString(), app.xsth.dj_digit));
                                    tr.find('input[name="je"]').val(limit_num(ret.Data.dj_pf.toString(), app.xsth.dj_digit));
                                    //设置old-data
                                    tr.find('input[name="barcode"]').attr('old-data', scan);
                                    tr.find('input[name="barcode"]').val(scan);
                                    tr.find('input[name="sl"]').attr('old-data', limit_num('1', app.xsth.sl_digit));
                                    tr.find('input[name="dj_jh"]').attr('old-data', limit_num(ret.Data.dj_jh.toString(), app.xsth.dj_digit));
                                    tr.find('input[name="je"]').attr('old-data', limit_num(ret.Data.dj_jh.toString(), app.xsth.dj_digit));
                                    tr.find('input[name="shopsp_obj"]').attr("value", ret.Data.id);
                                    

                                    //转换率
                                    var zhl = ret.Data.zhl.toString();
                                    zhl = limit_num(zhl, app.xsth.sl_digit)
                                    //var dj_jh = ret.Data.dj_jh.toString();
                                    var dj_jh = ret.Data.dj_pf.toString();
                                    dj_jh = limit_num(dj_jh, app.xsth.dj_digit);

                                    tr.find('input[name="sl_zhl"]').val(zhl);
                                    tr.find('input[name="shopsp_obj"]').attr("data-id_kcsp", ret.Data.id_kcsp);
                                    tr.find('input[name="shopsp_obj"]').attr("data-id_sp", ret.Data.id_sp);
                                    //绑定下拉
                                    tr.find('div[name=dw_select]>div>span').html(ret.Data.dw);
                                    tr.find('div[name=dw_select]>div>span').attr("data-zhl", ret.Data.zhl);
                                    tr.find('div[name=dw_select]').css('display', 'block');
                                    tr.find('ul').append('<li value=' + dj_jh + ' data-mc=' + ret.Data.mc + ' data-barcode=' + ret.Data.barcode + '  data-zhl=' + zhl + ' data-id_shopsp=' + ret.Data.id + ' data-id_kcsp=' + ret.Data.id_kcsp + '  data-id_sp=' + ret.Data.id_sp + ' data-dw=' + ret.Data.dw + ' data-dj_ls="' + ret.Data.dj_ls + '">' + ret.Data.dw + '</li>');
                                    var tbody = tr.parents("tbody");
                                    var data_item = tbody.find("tr:last").attr("data-item");
                                    if (data_item == ret.Data.id) {
                                        app.xsth.addshopsp();//新增最后一条新记录
                                        reset_xh();
                                    }
                                    app.xsth.setresult(false);
                                    return false;
                                }
                            });
                        } else {
                            $.DHB.dialog({ 'title': '选择商品', 'url': $.DHB.U('shopsp/search?keyword=' + scan), 'id': 'dialog-shopsp-search', 'confirm': app.xsth.dialogCallBack });
                        }
                    },
                    complete: function () { }
                
            });
        }
    });

    if (app.xsth.type == 'copy') {
        $("#" + app.xsth.div_name + ' #id_bill_origin', _).val('');
        $("#" + app.xsth.div_name + ' #dh_origin', _).val('');
        $("#" + app.xsth.div_name + ' #bm_djlx_origin', _).val('');

    }

    if ($("#" + app.xsth.div_name + ' #id_bill_origin', _).val() != '' && $("#" + app.xsth.div_name + ' #id_bill_origin', _).val() != 'undefined') {
        $(_ + "#" + app.xsth.div_name + " #kh_group_btn").attr("disabled", "disabled");
        $(_ + "#" + app.xsth.div_name + " #kh_name").attr("disabled", "disabled");
        $(_ + "#" + app.xsth.div_name + " #kh_group_span").attr("disabled", "disabled");
        $(_ + "#" + app.xsth.div_name + " #id_shop").attr("disabled", "disabled");
    }

}





$(_ + "#" + app.xsth.div_name + ' #je_st').focus(function () {
    $(this).select();
});

$(_ + "#" + app.xsth.div_name + ' #je_st').blur(function (e) {

    var tr = $(_ + "#" + app.xsth.div_name + " #shopsp_table_xsth>tbody>tr[data-item!='']");
    if (tr.length <= 0) {
        $(_ + "#" + app.xsth.div_name + " #je_st").val(limit_num("0", app.xsth.je_digit));
    } else {
        var je_temp = $(e).val().toString();
        $(_ + "#" + app.xsth.div_name + " #je_st").val(limit_num(je_temp, app.xsth.je_digit));
    }
});

$(_ + "#" + app.xsth.div_name + ' #je_st').keyup(function (e) {
    if (e.keyCode == 13) {//回车
        $(_ + "#" + app.xsth.div_name + ' #remark').focus().select();
    }
});


$(_ + "#" + app.xsth.div_name + ' #je_st').val(app.xsth.head_je_st);


app.xsth.onmouseover = function onmouseover(e) {
    var input = $(e);
    input.css({ border: "1px solid #ccc" })
};

app.xsth.onmouseout = function onmouseout(e) {
    var input = $(e);
    input.css({ border: "0px solid #ccc" })
};


app.xsth.sm = function (e) {
    var div_sm = $(_ + "#" + app.xsth.div_name + ' #div_sm');
    if (div_sm.css('display') == 'none') {
        $(_ + "#" + app.xsth.div_name + ' #div_sm').css('display', 'block');
    } else {
        $(_ + "#" + app.xsth.div_name + ' #div_sm').css('display', 'none');
    }
}





app.xsth.importin = function () {
    var id_shop = $(_ + "#id_shop").val();
    $.DHB.dialog({ 'title': '销售退货导入', 'url': $.DHB.U('xsth/importin?id_shop=' + id_shop), 'id': 'dialog-dhdr-search' });
}

function dialogJHDRCallBack(jsonStr) {
    debugger;
    app.xsth.dialogCallBackWork(jsonStr);
    app.xsth.setresult(true);
    $.DHB.message({ 'content': '操作完毕', 'time': 4000, 'type': 's' });
}

//导出
app.xsth.importout = function () {
    app.xsth.setresult(false);
    if ($(_ + "#" + app.xsth.div_name + " #shopsp_table_xsth>tbody>tr[data-item!='']").length <= 0) {
        $.DHB.message({ 'content': '无有效的数据！', 'type': 'i' });
    }
    else {
        var shopspList = $(_ + "#" + app.xsth.div_name + ' input[name="table_result"]').val();
        //window.location.href = $.DHB.U('/xsth/importout?shopspList=' + shopspList);

        debugger;
        var turnForm_jh = document.createElement("form");
        //一定要加入到body中！！   
        document.body.appendChild(turnForm_jh);
        turnForm_jh.method = 'post';
        turnForm_jh.action = '/xsth/importout';
        turnForm_jh.target = '_blank';
        //创建隐藏表单
        var newElement_jh = document.createElement("input");
        newElement_jh.setAttribute("name", "shopspList");
        newElement_jh.setAttribute("type", "hidden");
        newElement_jh.setAttribute("value", shopspList);
        turnForm_jh.appendChild(newElement_jh);
        turnForm_jh.submit();
    }
}



app.xsth.onblur = function (e, num) {
    var data = $(e).val();
    $(e).val(limit_num($.trim(data), num));
};

app.xsth.onfocus = function (e) {
    $(e).select();
};

app.xsth.showshopsp = function () {
    $.DHB.dialog({ 'title': '选择商品', 'url': $.DHB.U('shopsp/search?id_shop=' + $(_ + '#id_shop').val()), 'id': 'dialog-shopsp-search', 'confirm': app.xsth.dialogCallBack });
};



function reset_xh(e) {
    var xh = 1;
    $("#" + app.xsth.div_name + ' #shopsp_table_xsth>tbody').find('tr').each(function () {
        $(this).find('div[name="xh"]').text(xh);
        xh++;
    });
}

app.xsth.checkbarcode = function (obj) {

    //如果是上下左右Tab按键 回车 不处理
    var event = arguments.callee.caller.arguments[0] || window.event;//消除浏览器差异

    if (typeof (event) != 'undefined') {
        var keyCode = event.keyCode;

        if (keyCode == 37 || keyCode == 38 || keyCode == 39 || keyCode == 40 || keyCode == 9 || keyCode == 13 || keyCode == 8) {
            return false;
        }
    }

    var value = new String($(obj).val().replace(/\s+/g, ""));
    $(obj).val(value);
    if (isNaN(value) || value.indexOf(".") != -1) {
        $.DHB.message({ 'content': '仅允许输入数字', 'time': 4000, 'type': 'i' });

        var old_data = $(obj).attr('old-data');
        if (typeof (old_data) == 'undefined') { old_data = ''; }
        value = old_data;
        $(obj).val(value);
    }
    value = value + "";
    $(obj).attr('old-data', value);
}



app.xsth.flag1 = true;
app.xsth.add_dh_list_callback = function (selectVal) {
    if ($(_ + "#" + app.xsth.div_name + " #shopsp_table_xsth>tbody>tr[data-item!='']").length <= 0) {
        app.xsth.jh_id_dh_callback_work(selectVal);
    }
    else {
        $.messager.confirm("提示", "更改销售出库单将刷新并删除非此出库单的商品是否继续?", function () {
            app.xsth.jh_id_dh_callback_work(selectVal);
        });
    }
}



app.xsth.removeAll = function () {
    $("#" + app.xsth.div_name + ' #shopsp_table_xsth>tbody', _).find('tr').each(function () {
        $(this).remove();
    });
}


app.xsth.clear_select_xs = function (el) {
    var select_callback = function () {
        var selectCallback = $(el).data('callback');
        if (selectCallback) {
            eval(selectCallback + '();');
        }
    }
    var id_bill_origin = $(_ + "#" + app.xsth.div_name + " #id_bill_origin").val();
    if (id_bill_origin != '') {
        app.xsth.removeAll();
        app.xsth.setresult(true);
    }
    $(_ + '#' + $(el).data('id')).val('');
    $(_ + '#' + $(el).data('name')).val('');
    select_callback();
};



app.xsth.option = function (e) {
    //
    app.xsth.flag1 = true;
    app.xsth.select_index1 = $(e).find('li[class="selected"]').index();
};
//当选择制单门店改变时 记录新值 并清空商品信息
app.xsth.id_shop_sh_onchange = function (e) {
    if ($(_ + "#" + app.xsth.div_name + " #shopsp_table_xsth>tbody>tr[data-item!='']").length <= 0) {
        app.jh_id_shop_sh_onchange_work();
    }
    else {
        if (app.xsth.flag1 == true) {
            $.messager.confirm("提示", "更改门店将刷新并删除非此门店的商品是否继续?", function () {
                app.jh_id_shop_sh_onchange_work();
            }, function () {
                app.xsth.flag1 = false;
                $(e).prev().find('li').eq(app.xsth.select_index1).find('a').click();
            });
        } else {
            app.jh_id_shop_sh_onchange_work();
        }
    }
};

app.jh_id_shop_sh_onchange_work = function () {

    var id_shop_sh_hid = $(_ + "#" + app.xsth.div_name + ' #id_shop_sh_hid').val();
    var id_shop = $(_ + "#" + app.xsth.div_name + ' #id_shop').val();
    $(_ + "#" + app.xsth.div_name + " #id_shop_sh_hid").val(id_shop);
    if (id_shop_sh_hid != id_shop && id_shop_sh_hid != '') {
        app.xsth.removeAll();
        app.xsth.setresult(true);
    }
    app.xsth.flag1 = true;

}




app.xsth.jh_id_dh_callback_work = function (selectVal) {
    debugger;
    if (typeof (selectVal) == 'undefined' || selectVal == '' || typeof (selectVal.id) == 'undefined' || selectVal.id == '') {
        $("#" + app.xsth.div_name + ' #id_bill_origin', _).val('');
        $("#" + app.xsth.div_name + ' #dh_origin', _).val('');
        $("#" + app.xsth.div_name + ' #bm_djlx_origin', _).val('');
        $("#" + app.xsth.div_name + ' #kh_id', _).val('');
        $("#" + app.xsth.div_name + ' #kh_name', _).val('');
        $(_ + "#" + app.xsth.div_name + " #kh_group_btn").removeAttr("disabled");
        $(_ + "#" + app.xsth.div_name + " #kh_name").removeAttr("disabled");
        $(_ + "#" + app.xsth.div_name + " #kh_group_span").removeAttr("disabled");
        $(_ + "#" + app.xsth.div_name + " #id_shop").removeAttr("disabled");
        $(_ + "#" + app.xsth.div_name + " #div_id_shop").find('button[data-id="id_shop"]').removeClass("disabled");

        $("button:contains('导入')", _).removeClass('disabled').attr('onclick', 'app.xsth.importin()');
        $("button:contains('扫描')", _).removeClass('disabled').attr('onclick', 'app.xsth.sm(this)');
        $("#shopsp_table_xsth a:contains('选择')", _).removeClass('disabled').attr('onclick', 'app.xsth.showshopsp();');
        $("button:contains('增加行')", _).removeClass('disabled').attr('onclick', 'app.xsth.addshopsp_row(this)');


    } else {
        $("#" + app.xsth.div_name + ' #id_bill_origin', _).val(selectVal.id);
        $("#" + app.xsth.div_name + ' #dh_origin', _).val(selectVal.dh);
        $("#" + app.xsth.div_name + ' #bm_djlx_origin', _).val(selectVal.bm_djlx);
        $("#" + app.xsth.div_name + ' #kh_id', _).val(selectVal.id_kh);
        $("#" + app.xsth.div_name + ' #kh_name', _).val(selectVal.kh_name);
        $("#" + app.xsth.div_name + ' #remark', _).val(selectVal.bz);
        $(_ + "#" + app.xsth.div_name + " #id_shop").removeAttr("disabled");
        var index = $("#" + app.xsth.div_name + ' #id_shop', _).find('option[value="' + selectVal.id_shop + '"]').index();
        var objShop = $(_ + "#" + app.xsth.div_name + ' select[name="id_shop"]').prev();
        objShop.find('ul li').removeAttr("class");
        app.xsth.flag1 = false;
        $(objShop.find('ul li')[index]).find('a').click();
        $(_ + "#" + app.xsth.div_name + " #div_id_shop").find('button[data-id="id_shop"]').addClass("disabled");
        $(_ + "#" + app.xsth.div_name + " #kh_group_btn").attr("disabled", "disabled");
        $(_ + "#" + app.xsth.div_name + " #kh_name").attr("disabled", "disabled");
        $(_ + "#" + app.xsth.div_name + " #kh_group_span").attr("disabled", "disabled");
        $(_ + "#" + app.xsth.div_name + " #id_shop").attr("disabled", "disabled");
        $(_ + "#" + app.xsth.div_name + " #id_shop_sh_hid").val(selectVal.id_shop);

        //Post读取数据赋值
        cy.http.Post({
            url: '/xsck/GetCkDetailList',
            data: { id: selectVal.id },
            beforeSend: function () {
            },
            callback: function (ret) {
                if (ret.Success) {
                    var shopsp = [];
                    if (ret.Data.body.length > 0) {
                        for (var item in ret.Data.body) {
                            var shopsp_e = {};
                            shopsp_e.id_shopsp = ret.Data.body[item].id_shopsp;
                            shopsp_e.id_kcsp = ret.Data.body[item].id_kcsp;
                            shopsp_e.id_sp = ret.Data.body[item].id_sp;
                            shopsp_e.barcode = ret.Data.body[item].barcode;
                            shopsp_e.mc = ret.Data.body[item].shopsp_name;
                            shopsp_e.dw = ret.Data.body[item].dw;
                            shopsp_e.zhl = ret.Data.body[item].zhl;
                            shopsp_e.dj_jh = ret.Data.body[item].dj;
                            shopsp_e.dj_pf = ret.Data.body[item].dj;
                            shopsp_e.sl = ret.Data.body[item].sl;
                            shopsp_e.je = ret.Data.body[item].je;
                            
                            shopsp_e.bz = ret.Data.body[item].bz;
                            shopsp.push(shopsp_e);
                        }
                    }

                    var jsonStr = JSON.stringify(shopsp);
                    app.xsth.removeAll();
                    app.xsth.dialogCallBackWork(jsonStr);
                    app.xsth.setresult(true);

                    
                    $("button:contains('导入')", _).addClass('disabled').removeAttr('onclick');
                    $("button:contains('扫描')", _).addClass('disabled').removeAttr('onclick');
                    $("#shopsp_table_xsth a:contains('选择')", _).addClass('disabled').removeAttr('onclick');
                    $("button:contains('增加行')", _).addClass('disabled').removeAttr('onclick');

                }
            },
            complete: function () { }
        });
    }


}



function dialogXsthImportCallBack(jsonStr) {
    debugger;
    app.xsth.dialogCallBackWork(jsonStr);
    app.xsth.setresult(true);
    $.DHB.message({ 'content': '操作完毕', 'time': 4000, 'type': 's' });
}










