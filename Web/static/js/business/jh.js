app.jh = app.jh || {};
app.jh.div_name = "jh_add";

app.jh.sl_digit=$(_+"#"+app.jh.div_name+'  #sl_digit').val();
app.jh.dj_digit = $(_ + "#" + app.jh.div_name + '  #dj_digit').val();
app.jh.je_digit = $(_ + "#" + app.jh.div_name + '  #je_digit').val();
app.jh.type = $(_ + "#" + app.jh.div_name + '  #type').val();
app.jh.head_id = $(_ + "#" + app.jh.div_name + '  #head_id').val();
app.jh.head_dh = $(_ + "#" + app.jh.div_name + '  #head_dh').val();
app.jh.head_je_sf = $(_ + "#" + app.jh.div_name + '  #head_je_sf').val();
app.jh.default_dj = $(_ + "#" + app.jh.div_name + '  #default_dj').val();
app.jh.default_sl = $(_ + "#" + app.jh.div_name + '  #default_sl').val();
app.jh.default_je = $(_ + "#" + app.jh.div_name + '  #default_je').val();




$.DHB._ = function () {
    app.c.public_data['jh/add'] = app.c.public_data['jh/add'] || {};
    if (app.c.public_data['jh/add']['once'] === false) {
        app.c.public_data['jh/add']['once'] = true;
        $.DHB.checkForm(function () {

            if (!$(_ + "#"+app.jh.div_name +" #gys_id").val()) {
                $.DHB.message({ 'content': '请选择供应商！', 'type': 'i' });
            }
            else if ($(_ + "#"+app.jh.div_name +" #shopsp_table>tbody>tr[data-item!='']").length <= 0) {
                $.DHB.message({ 'content': '请至少选择一个商品！', 'type': 'i' });
            }
            else if ($(_ + "#" + app.jh.div_name + ' #je_sf').val() == '' || $(_ + "#" + app.jh.div_name + ' #je_sf').val() == '') {
                $.DHB.message({ 'content': '实收金额不准为空！', 'type': 'i' });
            }
            else if (parseFloat($(_ + "#"+app.jh.div_name +' #je_sf').val()) > parseFloat($(_ + "#"+app.jh.div_name +' #edit_goods_total').text())) {
                $.DHB.message({ 'content': '实付金额不准大于商品明细总金额！', 'type': 'i' });
            }
            else {
                

                app.jh.setresult();
                var objData = {};
                objData.shopspList = $(_ + "#"+app.jh.div_name +' input[name="table_result"]').val();
                objData.remark = $(_ + "#"+app.jh.div_name +' #remark').val();
                objData.id_gys = $(_ + "#"+app.jh.div_name +' #gys_id').val();
                objData.id_shop = $(_ + "#"+app.jh.div_name +' #id_shop').val();
                objData.id_jbr = $(_ + "#"+app.jh.div_name +' #id_jbr').val();
                objData.dh = $(_ + "#"+app.jh.div_name +' #dh').val();
                objData.rq = $(_ +"#"+app.jh.div_name +' #rq').val();
                objData.je_sf = $(_ + "#"+app.jh.div_name +' #je_sf').val();
                objData.type =app.jh.type;
                objData.id = app.jh.head_id;

                objData.dh_origin = $(_ + "#"+app.jh.div_name +' #dh_origin').val();
                objData.bm_djlx_origin = $(_ +"#"+app.jh.div_name +' #bm_djlx_origin').val();
                objData.id_bill_origin = $(_ + "#"+app.jh.div_name +' #id_bill_origin').val();
                
               // $.post($.DHB.U('jh/add'), objData,
               //         function (data) {
               //             if (data.status == "success") {
               //                 $.DHB.message({ 'content': data.message, 'time': 4000, 'type': 's' });
               //                 if ($(_ + "#"+app.jh.div_name +' #add_new').val() == '0') {
               //                     $.fn.menuTab.deleteMenu('jh/add');
               //                     $.DHB.url('jh/list', 'cache', '收货管理');
               //                 }
               //                 else {
               //                     $.fn.menuTab.deleteMenu('jh/add');
               //                     $.fn.menuTab.load({ url: '/jh/add', 'title': '新增收货', id: 'jh/add', nocache: '0' });

               //                 }
               //             } else {
               //                 $.DHB.message({ 'content': data.message, 'time': 0, 'type': 'e' });
               //             }
               //         }, 'json'
               //);

                var options = {
                    data: objData,
                    url: $.DHB.U('jh/add'),
                    type: "POST",
                    datatype: 'json',
                    beforeSend: function () { },
                    success: function (data, textStatus, jqXHR) {
                        if (data.status == "success") {
                            $.DHB.message({ 'content': data.message, 'time': 4000, 'type': 's' });
                            if ($(_ + "#" + app.jh.div_name + ' #add_new').val() == '0') {
                                $.fn.menuTab.deleteMenu('jh/add');
                                $.DHB.url('jh/list', 'cache', '收货管理');
                            }
                            else {
                                $.fn.menuTab.deleteMenu('jh/add');
                                $.fn.menuTab.load({ url: '/jh/add', 'title': '新增收货', id: 'jh/add', nocache: '0' });

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
    url: '/jh/createdh',
    beforeSend: function () {
    },
    callback: function (ret) {
        if (ret.Success) {
            if (app.jh.type != 'edit') {
                $(_ + "#"+app.jh.div_name +" #dh").val(ret.Data);
                $(_ + "#"+app.jh.div_name +" #dh").attr('old-data', ret.Data);
            } else {
                $(_ + "#" + app.jh.div_name + " #dh").val(app.jh.head_dh);
                $(_ + "#" + app.jh.div_name + " #dh").attr('old-data', app.jh.head_dh);
            }
        }
    },
    complete: function () {

    }
});




app.jh.add_gys_callback = function (selectVal) {
    selectVal = selectVal || '';
    var selectValTem = selectVal.split('|');
    if (selectValTem[0] != 'undefined' && selectValTem[0] != '') {
        $("#"+app.jh.div_name +' #gys_id', _).val(selectValTem[0]);
    }
    if (selectValTem[1] != 'undefined' && selectValTem[1] != '') {
        $("#"+app.jh.div_name +' #gys_name', _).val(selectValTem[1]);
    }
}


app.jh.dialogCallBack = function (array) {
    //var jsonStr = array[5].value;

    var jsonStr = "";
    $.each(array, function (index, item) {
        if (item.name == "shopsp_table_json") {
            jsonStr = item.value;
        }
    });

    var id_bill_origin = $(_ + "#"+app.jh.div_name +" #id_bill_origin").val();
    if (id_bill_origin != '') {
        $.DHB.message({ 'content': '你已经选择关联的订货单号,不允许再添加其他商品,若需要自行添加,请删除关联的订货单号！', 'time': 4000, 'type': 'i' });
    } else {
        app.jh.dialogCallBackWork(jsonStr);
    }

    //关闭dialog
    $.DHB.dialog({ 'id': 'dialog-shopsp-search', 'action': 'destroy' });
    app.jh.setresult(true);
}

//选择商品回调
app.jh.dialogCallBackWork = function (jsonStr) {
    
    var data_item_last = '';
    var objList = jQuery.parseJSON(jsonStr);
    $(objList).each(function (i, obj) {
        var guid = parseInt($(_ + "#"+app.jh.div_name +" #shopspnum").val()) + 1;
        var option = '';
        var resultHtml = '';
        resultHtml += '<tr data-item="' + obj.id_shopsp + '">';
        resultHtml += '<td class="align_center">  <div name="xh">' + guid + '</div>  <input class="form-control user-input" style="width:110px;display:none;" placeholder="商品ID" name="shopsp_obj" type="text" value="' + obj.id_shopsp + '"  data-id_kcsp="' + obj.id_kcsp + '"   data-id_sp="' + obj.id_sp + '" ></td>';
        resultHtml += ' <td><input class="form-control user-input col-md-2" style="" onmouseover="app.jh.onmouseover(this)" onmouseout="app.jh.onmouseout(this)" type="text" value="' + obj.barcode + '" name="barcode" old-data="' + obj.barcode + '"></td>';
        resultHtml += '<td name="mc"> <div style="width: 100% !important;text-align:left;padding:0 0;"> <span>' + obj.mc + '</span><a href="javascript:void(0)" onclick="app.jh.showshopsp();" class="btn btn-info f-r">  选择</a></div> </td>';

        var sl_kc = 0;

        if (typeof (obj.dw) != 'undefined' && obj.dw != 'undefined' && obj.dw != '') {
            resultHtml += '<td name="dw">    <div id="dw_select" name="dw_select"  class="common_select" style="display:block;"><div onclick="app.jh.dw_select_onclick(this)"><span class="inline" data-zhl="' + limit_num(obj.zhl.toString(), app.jh.sl_digit) + '">' + obj.dw + '</span> <i class="fa fa-caret-down"></i></div><ul class="common_select_list"><li value="' + limit_num(obj.dj_jh.toString(), app.jh.dj_digit) + '"  data-zhl="' + limit_num(obj.zhl.toString(), app.jh.sl_digit) + '"    data-id_shopsp="' + obj.id_shopsp + '"   data-id_kcsp="' + obj.id_kcsp + '"  data-id_sp="' + obj.id_sp + '"  data-dw="' + obj.dw + '"    data-mc="' + obj.mc + '" data-barcode="' + obj.barcode + '" data-sl_kc="' + sl_kc + '"  >' + obj.dw + '</li></ul></div></td>';
        } else {
            resultHtml += '<td name="dw">    <div id="dw_select" name="dw_select"   class="common_select"><div onclick="app.jh.dw_select_onclick(this)"><span class="inline"></span> <i class="fa fa-caret-down"></i></div><ul value="" class="common_select_list"></ul></div></td>';
        }
        var sl = limit_num(obj.sl.toString(), app.jh.sl_digit);
        var dj = limit_num(obj.dj_jh.toString(), app.jh.dj_digit);

        var je = accMul(parseFloat(dj), parseFloat(sl));
        if (typeof (obj.je) != 'undefined' && obj.je != 'undefined' && obj.je != '') {
            je = obj.je;
        }
        je = limit_num(je.toString(), app.jh.je_digit);

        resultHtml += '<td><input class="form-control user-input" style="width:96%;text-align:right;" onmouseover="app.jh.onmouseover(this)" onmouseout="app.jh.onmouseout(this)" placeholder="数量" type="text" value="' + sl + '" name="sl" onkeyup="check_digit(this,\'' + app.jh.sl_digit + '\')" onafterpaste="check_digit(this,\'' + app.jh.sl_digit + '\')" old-data="' + sl + '">    <input name="sl_zhl" style="width:95%;display:none;text-align:right;" placeholder="转换率" type="text" value="1">  <input name="sl_total" style="width:95%;display:none;text-align:right;" placeholder="数量总数" type="text" value="' + app.jh.default_sl + '"></td>';
        resultHtml += ' <td><input class="form-control user-input" style="width:96%;text-align:right;" onmouseover="app.jh.onmouseover(this)" onmouseout="app.jh.onmouseout(this)" placeholder="单价(元)" type="text" value="' + dj + '" name="dj_jh" old-data="' + dj + '"   onkeyup="check_digit(this,\''+app.jh.dj_digit+'\')" onafterpaste="check_digit(this,\''+app.jh.dj_digit+'\')" ></td>';
        resultHtml += ' <td name="je"> <input class="form-control user-input" style="width:96%;text-align:right;" onmouseover="app.jh.onmouseover(this)" onmouseout="app.jh.onmouseout(this)" placeholder="金额(元)" type="text" value="' + je + '" name="je" old-data="' + je + '"   onkeyup="check_digit(this,\''+app.jh.je_digit+'\')" onafterpaste="check_digit(this,\''+app.jh.je_digit+'\')" ></td>';
        if (typeof (obj.bz) != 'undefined' && obj.bz != 'undefined' && obj.bz != '') {
            resultHtml += ' <td><input class="form-control user-input" style="width:96%;" onmouseover="app.jh.onmouseover(this)" onmouseout="app.jh.onmouseout(this)"  type="text" value="' + obj.bz + '" name="bz"></td>';
        } else {
            resultHtml += ' <td><input class="form-control user-input" style="width:96%;" onmouseover="app.jh.onmouseover(this)" onmouseout="app.jh.onmouseout(this)" type="text" value="" name="bz"></td>';
        }
        resultHtml += '<td class="align_center"><a onclick="app.jh.choice_spec_del(this);" class="bg-state bg-state-info blockbtn fa fa-trash" title="删除"></a></td>';
        resultHtml += ' </tr>';
        $(_ + "#" + app.jh.div_name + " #shopspnum").val(guid);
        $(_ + "#" + app.jh.div_name + " #shopsp_table>tbody").append(resultHtml);
        app.jh.bindkeypresslast();
        data_item_last = obj.id_shopsp;
    });
}

app.jh.addshopsp_row = function (obj) {
    app.jh.addshopsp();
    reset_xh();
};


//添加商品至table
app.jh.addshopsp = function (obj) {
    var shopsp = [];
    var shopsp_e = {};
    shopsp_e.id_shopsp = "";
    shopsp_e.id_kcsp = "";
    shopsp_e.id_sp = "";
    shopsp_e.barcode = "";
    shopsp_e.mc = "";
    shopsp_e.dw = "";
    shopsp_e.zhl = "1";
    shopsp_e.dj_jh = app.jh.default_dj;
    shopsp_e.sl = app.jh.default_sl;
    shopsp.push(shopsp_e);
    var jsonStr = JSON.stringify(shopsp);
    app.jh.dialogCallBackWork(jsonStr);
}

//删除商品的数据
app.jh.choice_spec_del = function (el) {
    $(el).parents('tr').remove();
    app.jh.setresult(false);
}

//改动后重新计算和赋值
app.jh.setresult = function (ifRemove) {

    var e = [];
    var shopsp = [];
    var totleMoney = 0;
    var xh = 1;

    if (ifRemove) {
        $("#" + app.jh.div_name + ' #shopsp_table>tbody').find('tr[data-item=""]').each(function () {
            $(this).remove();
        });
    }

    $("#" + app.jh.div_name + ' #shopsp_table>tbody').find('tr').each(function () {
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
        sl_total = limit_num(sl_total.toString(), app.jh.sl_digit)
        $(this).find('input[name="sl_total"]').val(sl_total);
        shopsp_e.sl_total = $.trim(sl_total);
        shopsp_e.je = $.trim($(this).find('input[name="je"]').val()); //金额
        if (shopsp_e.id_shopsp != '') {
            shopsp.push(shopsp_e);
        }
        xh++;
        //当前金额
        var moneyTotal = limit_num(shopsp_e.je.toString(), app.jh.dj_digit)
        //总金额
        totleMoney = accAdd(totleMoney, parseFloat($.trim(moneyTotal)));
    });

    if (ifRemove) {
        $(_ + "#" + app.jh.div_name + " #shopspnum").val(parseInt(xh) - 1);
        app.jh.addshopsp();
        app.jh.addshopsp();
        app.jh.addshopsp();
        app.jh.addshopsp();
        app.jh.addshopsp();
        app.jh.addshopsp();
        app.jh.addshopsp();
        app.jh.addshopsp();
        app.jh.addshopsp();
        app.jh.addshopsp();
        reset_xh();
    }

    totleMoney = limit_num(totleMoney.toString(), app.jh.je_digit)
    $(_ + "#" + app.jh.div_name + ' #edit_goods_total').text(totleMoney);
    var jsonStr = JSON.stringify(shopsp);
    $(_ + "#" + app.jh.div_name + " #table_result").val(jsonStr);

}

//时间控件
app.jh.wdatepicker_jh_list = function (el) {
    var booStart = $(el).data('type') == 'start';
    var option = {};
    option['el'] = $(el).data('field') + (!booStart ? '_end' : '');
    option['onpicked'] = function () {
        $(el)
            .text($dp.cal.getP('y') +
                '-' +
                $dp.cal.getP('M') +
                '-' +
                $dp.cal.getP('d'));
        if (booStart) {
            setTimeout(function () {
                $(el)
                    .parent()
                    .find('[data-type="end"]')
                    .data('position', '1')
                    .click();
            },
                5000);
        }
        app.search.do_search_dw_list();
    };

    if (booStart) {
        option['maxDate'] =
            '#F{ $dp.$D(\'' + $(el).data('field') + '_end\') }';
    } else {
        option['minDate'] = '#F{ $dp.$D(\'' + $(el).data('field') + '\') }';
        if ($(el).data('position') == '1') {
        }
    }
    option['oncleared'] = function () {
        $(el).html(booStart ? $(el).data('title') : '截至日期');
        app.search.do_search_dw_list();
    };

    WdatePicker(option);
}




//绑定事件
app.jh.bindkeypress = function (obj) {
    $("#" + app.jh.div_name + ' #shopsp_table>tbody',_).find('tr').each(function () {
        bindEvent($(this));
    });
}

//默认绑定事件
app.jh.bindkeypress();

//绑定最后一条tr的事件
app.jh.bindkeypresslast = function () {
    $("#" + app.jh.div_name + " #shopsp_table>tbody>tr:last",_).each(function () {
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
                                tr.find('input[name="sl"]').val(limit_num('1', app.jh.sl_digit));
                                tr.find('input[name="dj_jh"]').val(limit_num(ret.Data.dj_jh.toString(), app.jh.dj_digit));
                                tr.find('input[name="je"]').val(limit_num(ret.Data.dj_jh.toString(),app.jh.dj_digit));
                                //设置old-data
                                barcode_obj.attr('old-data', barcode_obj.val());
                                sl_obj.attr('old-data', limit_num('1', app.jh.sl_digit));
                                dj_ls_obj.attr('old-data', limit_num(ret.Data.dj_jh.toString(), app.jh.dj_digit));
                                je_obj.attr('old-data', limit_num(ret.Data.dj_jh.toString(), app.jh.dj_digit));
                                tr.find('input[name="shopsp_obj"]').attr("value", ret.Data.id);
                                tr.find('input[name="sl"]').focus().select();
                                //转换率
                                var zhl = ret.Data.zhl.toString();
                                zhl = limit_num(zhl, app.jh.sl_digit)
                                var dj_jh = ret.Data.dj_jh.toString();
                                dj_jh = limit_num(dj_jh, app.jh.dj_digit)
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
                                    app.jh.addshopsp();//新增最后一条新记录
                                    reset_xh();
                                }
                                app.jh.setresult(false);
                            } else {
                                $.DHB.dialog({ 'title': '选择商品', 'url': $.DHB.U('shopsp/search?keyword=' + barcode_obj.val() + '&id_shop=' + $(_ + '#id_shop').val()), 'id': 'dialog-shopsp-search', 'confirm': app.jh.dialogCallBack });
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
            //else if (e.keyCode == 37) {//左
            //    var tr_prev = tr.prev('tr');
            //    if (tr_prev.length > 0) {
            //        tr_prev.find('input[name="bz"]').focus().select();
            //    }
            //}
            //else if (e.keyCode == 39) {//右
            //    sl_obj.focus().select();
            //}
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
                    if (tr_next.attr('data-item') == '' || tr_next.attr('data-item') == undefined) {
                        tr_next.find('input[name="barcode"]').focus().select();
                    } else {
                        tr_next.find('input[name="sl"]').focus().select();
                    }
                }
            }
            //else if (e.keyCode == 37) {//左
            //    barcode_obj.focus().select();
            //}
            //else if (e.keyCode == 39) {//右
            //    dj_ls_obj.focus().select();
            //}
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
                    if (tr_next.attr('data-item') == '' || tr_next.attr('data-item') == undefined) {
                        tr_next.find('input[name="barcode"]').focus().select();
                    } else {
                        tr_next.find('input[name="dj_jh"]').focus().select();

                    }
                }
            }
            //else if (e.keyCode == 37) { //左
            //    sl_obj.focus().select();
            //}
            //else if (e.keyCode == 39) { //右
            //    je_obj.focus().select();
            //}
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
                    if (tr_next.attr('data-item') == '' || tr_next.attr('data-item') == undefined) {
                        tr_next.find('input[name="barcode"]').focus().select();
                    } else {
                        tr_next.find('input[name="je"]').focus().select();
                    }
                }
            }
            //else if (e.keyCode == 37) { //左
            //    dj_ls_obj.focus().select();
            //}
            //else if (e.keyCode == 39) { //右
            //    bz_obj.focus().select();
            //}
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
                        app.jh.addshopsp();
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
                    if (tr_next.attr('data-item') == '' || tr_next.attr('data-item') == undefined) {
                        tr_next.find('input[name="barcode"]').focus().select();
                    } else {
                        tr_next.find('input[name="bz"]').focus().select();
                    }
                }
            }
            //else if (e.keyCode == 37) { //左
            //    je_obj.focus().select();
            //}
            //else if (e.keyCode == 39) {//右
            //    var tr_next = tr.next('tr');
            //    if (tr_next.length > 0) {
            //        tr_next.find('input[name="barcode"]').focus().select();
            //    }
            //}
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
                var dj = limit_num($.trim(tr.find('input[name="dj_jh"]').val()), app.jh.dj_digit); //单价
                var sl = limit_num($.trim(tr.find('input[name="sl"]').val()),app.jh.sl_digit); //数量
                var old_je = limit_num($.trim(tr.find('input[name="je"]').val()), app.jh.je_digit); //旧金额
                var je = accMul(parseFloat(dj), parseFloat(sl));//金额
                //如果旧金额除以数量取小数后等于现在的单价 金额则不变
                if (parseFloat(sl) != 0) {
                    //var dj_temp = (parseFloat(old_je) / parseFloat(sl));
                    var dj_temp = accDiv(parseFloat(old_je), parseFloat(sl));
                    dj_temp = limit_num($.trim(dj_temp),app.jh.dj_digit);
                    if (parseFloat(dj_temp) == parseFloat(dj)) {
                        je = old_je;
                    }
                }
                je = limit_num($.trim(je), app.jh.je_digit);
                tr.find('input[name="je"]').val(je);
                tr.find('input[name="je"]').attr('old-data', je);
                tr.find('input[name="sl"]').val(sl);
                tr.find('input[name="sl"]').attr('old-data', sl);
                app.jh.setresult(false);
            } else {
                sl_obj.val(app.jh.default_sl);
            }

        });
        dj_ls_obj.blur(function () {
            var tr = dj_ls_obj.parents("tr");
            var data_item = tr.attr('data-item');
            if (typeof (data_item) != 'undefined' && data_item != 'undefined' && data_item != '') {
                //单价变了金额变
                var dj = limit_num($.trim(tr.find('input[name="dj_jh"]').val()),app.jh.dj_digit); //单价
                var sl = limit_num($.trim(tr.find('input[name="sl"]').val()), app.jh.sl_digit); //数量
                var old_je = limit_num($.trim(tr.find('input[name="je"]').val()), app.jh.je_digit); //旧金额
                var je = accMul(parseFloat(dj), parseFloat(sl));//金额



                //如果旧金额除以数量取小数后等于现在的单价 金额则不变
                if (parseFloat(sl) != 0) {



                    var dj_temp = accDiv(parseFloat(old_je), parseFloat(sl));
                    dj_temp = limit_num($.trim(dj_temp), app.jh.dj_digit);
                    if (parseFloat(dj_temp) == parseFloat(dj)) {
                        je = old_je;
                    }
                }
                je = limit_num($.trim(je), app.jh.je_digit);
                tr.find('input[name="je"]').val(je);
                tr.find('input[name="je"]').attr('old-data', je);
                tr.find('input[name="dj_jh"]').val(dj);
                tr.find('input[name="dj_jh"]').attr('old-data', dj);
                app.jh.setresult(false);
            } else {
                dj_ls_obj.val(app.jh.default_dj);
            }
        });
        je_obj.blur(function () {
            var tr = je_obj.parents("tr");
            var data_item = tr.attr('data-item');
            if (typeof (data_item) != 'undefined' && data_item != 'undefined' && data_item != '') {
                //金额变了单价变

                var old_dj = limit_num($.trim(tr.find('input[name="dj_jh"]').val()), app.jh.dj_digit); //旧单价
                var je = limit_num($.trim(tr.find('input[name="je"]').val()), app.jh.je_digit); //金额
                var sl = limit_num($.trim(tr.find('input[name="sl"]').val()), app.jh.sl_digit); //数量
                var dj = app.jh.default_sj;//单价

                if (parseFloat(sl) != 0) {
                    var temp_je = accMul(parseFloat(old_dj), parseFloat(sl));//金额
                    temp_je = limit_num($.trim(temp_je),app.jh.je_digit);
                    if (je == temp_je) {
                        dj = old_dj;
                    } else {
                        var dj_temp = accDiv(parseFloat(je), parseFloat(sl));
                        dj_temp = limit_num($.trim(dj_temp),app.jh.dj_digit);
                        dj = dj_temp;
                    }

                }

                tr.find('input[name="dj_jh"]').val(dj);
                tr.find('input[name="dj_jh"]').attr('old-data', dj);
                tr.find('input[name="je"]').val(je);
                tr.find('input[name="je"]').attr('old-data', je);
                app.jh.setresult(false);
            } else {
                je_obj.val(app.jh.default_je);
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
app.jh.dw_select_onchange = function (e) {
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

    //var dj = limit_num($.trim(e.value).toString(), app.jh.dj_digit);//单价
    var dj = limit_num($(e).attr('value').toString(), app.jh.dj_digit);//单价
    var sl = limit_num($.trim(tr.find('input[name="sl"]').val()).toString(), app.jh.sl_digit); //数量
    var je = accMul(parseFloat(dj), parseFloat(sl));
    je = limit_num($.trim(je).toString(), app.jh.je_digit);//金额
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

    app.jh.setresult(false);
};

//点击单位下拉事件
app.jh.dw_select_onclick = function (e) {
    //当选中订货单号时 不允许下拉改变单位
    var id_shop = $(_ + "#id_shop").val();
    var id_bill_origin=$("#" + app.jh.div_name + ' #id_bill_origin', _).val();
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
                                    zhl = limit_num(zhl, app.jh.sl_digit)
                                    var dj_jh = ret.Data[item].dj_jh.toString();
                                    dj_jh = limit_num(dj_jh, app.jh.dj_digit)

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

app.jh.add_ready = function () {

    app.jh.div_name = "jh_add";
    app.jh.sl_digit = $(_ + "#" + app.jh.div_name + '  #sl_digit').val();
    app.jh.dj_digit = $(_ + "#" + app.jh.div_name + '  #dj_digit').val();
    app.jh.je_digit = $(_ + "#" + app.jh.div_name + '  #je_digit').val();
    app.jh.type = $(_ + "#" + app.jh.div_name + '  #type').val();
    app.jh.head_id = $(_ + "#" + app.jh.div_name + '  #head_id').val();
    app.jh.head_dh = $(_ + "#" + app.jh.div_name + '  #head_dh').val();
    app.jh.head_je_sf = $(_ + "#" + app.jh.div_name + '  #head_je_sf').val();
    app.jh.default_dj = $(_ + "#" + app.jh.div_name + '  #default_dj').val();
    app.jh.default_sl = $(_ + "#" + app.jh.div_name + '  #default_sl').val();
    app.jh.default_je = $(_ + "#" + app.jh.div_name + '  #default_je').val();
    
    $(_ + "#" + app.jh.div_name + " #id_shop_sh_hid").val($(_ + "#" + app.jh.div_name + ' #id_shop').val());

    $("#" + app.jh.div_name + ' .table_list', _).on('mouseleave', '.common_select', function () {
        $('.common_select_list', _).hide();
    });
    $("#" + app.jh.div_name + ' .table_list', _).on('click', '.common_select_list li', function () {

        var li_txt = $(this).html();
        $(this).parents('.common_select_list').hide();
        $(this).parents('.common_select').find('span').html(li_txt);
 
        $(this).parents('.common_select').find('span').attr('data-zhl', $(this).attr('data-zhl'));
        app.jh.dw_select_onchange(this);
    });
    app.jh.setresult(true);

    $(_ + "#" + app.jh.div_name + ' #barcode_search').keyup(function (e) {
        if (e.keyCode == 13) {
            var scan = $(_ + "#" + app.jh.div_name + ' #barcode_search').val();
            $(_ + "#" + app.jh.div_name + ' #barcode_search').val('');
            var id_shop = $(_ + "#id_shop").val();
                //Post读取数据赋值
                cy.http.Post({
                    url: '/shopsp/GetShopspList',
                    data: { keyword: scan, id_shop: id_shop },
                    beforeSend: function () {
                    },
                    callback: function (ret) {
                        if (ret.Success) {
                            
                            $("#" + app.jh.div_name + ' #shopsp_table>tbody tr').each(function () {
                                var tr = $(this);
                                if (tr.attr('data-item') == ret.Data.id) {
                                    var old_sl = tr.find('input[name=sl]').val();
                                    tr.find('input[name=sl]').val((parseFloat(old_sl) + 1).toFixed(2));
                                    tr.find('input[name="sl"]').blur();
                                    return false;
                                } else if (tr.attr('data-item') == "") {
                                    tr.attr("data-item", ret.Data.id);
                                    tr.find('td[name="mc"] div span').text(ret.Data.mc);
                                    tr.find('input[name="sl"]').val(limit_num('1', app.jh.sl_digit));
                                    tr.find('input[name="dj_jh"]').val(limit_num(ret.Data.dj_jh.toString(), app.jh.dj_digit));
                                    tr.find('input[name="je"]').val(limit_num(ret.Data.dj_jh.toString(), app.jh.dj_digit));
                                    //设置old-data
                                    tr.find('input[name="barcode"]').attr('old-data', scan);
                                    tr.find('input[name="barcode"]').val(scan);
                                    tr.find('input[name="sl"]').attr('old-data', limit_num('1', app.jh.sl_digit));
                                    tr.find('input[name="dj_jh"]').attr('old-data', limit_num(ret.Data.dj_jh.toString(), app.jh.dj_digit));
                                    tr.find('input[name="je"]').attr('old-data', limit_num(ret.Data.dj_jh.toString(), app.jh.dj_digit));
                                    tr.find('input[name="shopsp_obj"]').attr("value", ret.Data.id);
                                    

                                    //转换率
                                    var zhl = ret.Data.zhl.toString();
                                    zhl = limit_num(zhl, app.jh.sl_digit)
                                    var dj_jh = ret.Data.dj_jh.toString();
                                    dj_jh = limit_num(dj_jh, app.jh.dj_digit)

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
                                        app.jh.addshopsp();//新增最后一条新记录
                                        reset_xh();
                                    }
                                    app.jh.setresult(false);
                                    return false;
                                }
                            });
                        } else {
                            $.DHB.dialog({ 'title': '选择商品', 'url': $.DHB.U('shopsp/search?keyword=' + scan), 'id': 'dialog-shopsp-search', 'confirm': app.jh.dialogCallBack });
                        }
                    },
                    complete: function () { }
                
            });
        }
    });

    if (app.jh.type == 'copy') {
        $("#" + app.jh.div_name + ' #id_bill_origin', _).val('');
        $("#" + app.jh.div_name + ' #dh_origin', _).val('');
        $("#" + app.jh.div_name + ' #bm_djlx_origin', _).val('');

    }

    if ($("#" + app.jh.div_name + ' #id_bill_origin', _).val() != '' && $("#" + app.jh.div_name + ' #id_bill_origin', _).val() != 'undefined') {
        $(_ + "#" + app.jh.div_name + " #gys_group_btn").attr("disabled", "disabled");
        $(_ + "#" + app.jh.div_name + " #gys_name").attr("disabled", "disabled");
        $(_ + "#" + app.jh.div_name + " #gys_group_span").attr("disabled", "disabled");
        $(_ + "#" + app.jh.div_name + " #id_shop").attr("disabled", "disabled");
    }




}





$(_ + "#" + app.jh.div_name + ' #je_sf').focus(function () {
    $(this).select();
});

$(_ + "#" + app.jh.div_name + ' #je_sf').blur(function (e) {

    var tr = $(_ + "#" + app.jh.div_name + " #shopsp_table>tbody>tr[data-item!='']");
    if (tr.length <= 0) {
        $(_ + "#" + app.jh.div_name + " #je_sf").val(limit_num("0", app.jh.je_digit));
    } else {
        var je_temp = $(e).val().toString();
        $(_ + "#" + app.jh.div_name + " #je_sf").val(limit_num(je_temp, app.jh.je_digit));
    }
});

$(_ + "#" + app.jh.div_name + ' #je_sf').keyup(function (e) {
    if (e.keyCode == 13) {//回车
        $(_ + "#" + app.jh.div_name + ' #remark').focus().select();
    }
});


$(_ + "#" + app.jh.div_name + ' #je_sf').val(app.jh.head_je_sf);


app.jh.onmouseover = function onmouseover(e) {
    //var input = $(e);
    //input.css({ border: "1px solid #ccc" })
};

app.jh.onmouseout = function onmouseout(e) {
    //var input = $(e);
    //input.css({ border: "0px solid #ccc" })
};


app.jh.sm = function (e) {
    var div_sm = $(_ + "#" + app.jh.div_name + ' #div_sm');
    if (div_sm.css('display') == 'none') {
        $(_ + "#" + app.jh.div_name + ' #div_sm').css('display', 'block');
    } else {
        $(_ + "#" + app.jh.div_name + ' #div_sm').css('display', 'none');
    }
}





app.jh.importin = function () {
    var id_shop = $(_ + "#id_shop").val();
    $.DHB.dialog({ 'title': '收货导入', 'url': $.DHB.U('jh/importin?id_shop=' + id_shop), 'id': 'dialog-jhdr-search' });
}

function dialogJHDRCallBack(jsonStr) {

    app.jh.dialogCallBackWork(jsonStr);
    app.jh.setresult(true);
    $.DHB.message({ 'content': '操作完毕', 'time': 4000, 'type': 's' });
}

//导出
app.jh.importout = function () {
    app.jh.setresult(false);
    if ($(_ + "#" + app.jh.div_name + " #shopsp_table>tbody>tr[data-item!='']").length <= 0) {
        $.DHB.message({ 'content': '无有效的数据！', 'type': 'i' });
    }
    else {
        var shopspList = $(_ + "#" + app.jh.div_name + ' input[name="table_result"]').val();
        //window.location.href = $.DHB.U('/jh/importout?shopspList=' + shopspList);

        debugger;
        var turnForm_jh = document.createElement("form");
        //一定要加入到body中！！   
        document.body.appendChild(turnForm_jh);
        turnForm_jh.method = 'post';
        turnForm_jh.action = '/jh/importout';
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

//定位
app.jh.dw = function (obj) {
    $(_ + "#" + app.jh.div_name + " #define-jh-dw").modal('show');
};

//定位确定
app.jh.dwsub = function (obj) {
    var czwz = $(_ + "#" + app.jh.div_name + " #id_czwz").val();
    var cznr = $(_ + "#" + app.jh.div_name + " #id_cznr").val();
    if (cznr == '') {
        $.DHB.message({ 'content': '请填写查找内容！', 'type': 'i' });
        return false;
    }

    var tbody = $("#" + app.jh.div_name + ' #shopsp_table>tbody');

    if (czwz == '行号') {
        tbody.find('tr').each(function () {
            var data = $(this).find('div[name="xh"]').text();
            if (data == cznr) {
                //$(this).find('input[name="barcode"]').val('找到此行');
                $(this).focus().select();
            }
        });
    }
    else if (czwz == '条码') {
        tbody.find('tr').each(function () {
            var data = $(this).find('input[name="barcode"]').val();
            if (data.toString().indexOf(cznr) != -1) {
                $(this).find('input[name="barcode"]').focus().select();
            }
        });
    }
    else if (czwz == '名称') {
        tbody.find('tr').each(function () {
            var data = $(this).find('td[name="mc"] div span').text();
            if (data.toString().indexOf(cznr) != -1) {
                $(this).find('input[name="barcode"]').focus().select();
            }
        });
    }
    else if (czwz == '包装数') {
        tbody.find('tr').each(function () {
            var data = $.trim($(this).find('select[name="dw_select"] option:selected').attr('data-zhl')); //转换率
            if (data.toString() == $.trim(cznr)) {
                $(this).find('input[name="barcode"]').focus().select();
            }
        });
    }
    else if (czwz == '数量') {
        tbody.find('tr').each(function () {
            var data = $(this).find('input[name="sl"]').val();
            if (data.toString() == cznr) {
                $(this).find('input[name="barcode"]').focus().select();
            }
        });
    }
    else if (czwz == '单价') {
        tbody.find('tr').each(function () {
            var data = $(this).find('input[name="dj"]').val();
            if (data.toString() == cznr) {
                $(this).find('input[name="barcode"]').focus().select();
            }
        });
    }
    else if (czwz == '金额') {
        tbody.find('tr').each(function () {
            var data = $(this).find('input[name="je"]').val();
            if (data.toString() == cznr) {
                $(this).find('input[name="barcode"]').focus().select();
            }
        });
    }
    else if (czwz == '备注') {
        tbody.find('tr').each(function () {
            var data = $(this).find('input[name="bz"]').val();
            if (data.toString().indexOf(cznr) != -1) {
                $(this).find('input[name="bz"]').focus().select();
            }
        });
    }

};


app.jh.onblur = function (e, num) {
    var data = $(e).val();
    $(e).val(limit_num($.trim(data), num));
};

app.jh.onfocus = function (e) {
    $(e).select();
};

app.jh.showshopsp = function () {
    $.DHB.dialog({ 'title': '选择商品', 'url': $.DHB.U('shopsp/search?id_shop=' + $(_ + '#id_shop').val()), 'id': 'dialog-shopsp-search', 'confirm': app.jh.dialogCallBack });
};



function reset_xh(e) {
    var xh = 1;
    $("#" + app.jh.div_name + ' #shopsp_table>tbody').find('tr').each(function () {
        $(this).find('div[name="xh"]').text(xh);
        xh++;
    });
}

app.jh.checkbarcode = function (obj) {

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



app.jh.flag1 = true;
app.jh.add_dh_list_callback = function (selectVal) {
    //
    if ($(_ + "#" + app.jh.div_name + " #shopsp_table>tbody>tr[data-item!='']").length <= 0) {
        app.jh_id_dh_callback_work(selectVal);
    }
    else {
        $.messager.confirm("提示", "更改订货单将刷新并删除非此订货单的商品是否继续?", function () {
            app.jh_id_dh_callback_work(selectVal);
        });
    }
}



app.jh.removeAll = function () {
    $("#" + app.jh.div_name + ' #shopsp_table>tbody', _).find('tr').each(function () {
        $(this).remove();
    });
}

app.jh.clear_select_dh = function (el) {
    var select_callback = function () {
        var selectCallback = $(el).data('callback');
        if (selectCallback) {
            eval(selectCallback + '();');
        }
    }
    var id_bill_origin = $(_ + "#" + app.jh.div_name + " #id_bill_origin").val();
    if (id_bill_origin != '') {
        app.jh.removeAll();
        app.jh.setresult(true);
    }
    $(_ + '#' + $(el).data('id')).val('');
    $(_ + '#' + $(el).data('name')).val('');
    select_callback();
};



app.jh.option = function (e) {
    //
    app.jh.flag1 = true;
    app.jh.select_index1 = $(e).find('li[class="selected"]').index();
};
//当选择制单门店改变时 记录新值 并清空商品信息
app.jh.id_shop_sh_onchange = function (e) {
    
    if ($(_ + "#" + app.jh.div_name + " #shopsp_table>tbody>tr[data-item!='']").length <= 0) {
        app.jh_id_shop_sh_onchange_work();
    }
    else {

        if (app.jh.flag1 == true) {
            $.messager.confirm("提示", "更改门店将刷新并删除非此门店的商品是否继续?", function () {
                app.jh_id_shop_sh_onchange_work();
            }, function () {
                //
                app.jh.flag1 = false;
                $(e).prev().find('li').eq(app.jh.select_index1).find('a').click();

            });
        } else {
            app.jh_id_shop_sh_onchange_work();
        }
    }

};
app.jh_id_shop_sh_onchange_work = function () {
    var id_shop_sh_hid = $(_ + "#" + app.jh.div_name + ' #id_shop_sh_hid').val();
    var id_shop = $(_ + "#" + app.jh.div_name + ' #id_shop').val();
    if (id_shop_sh_hid != id_shop) {
        $(_ + "#" + app.jh.div_name + " #id_shop_sh_hid").val(id_shop);
        app.jh.removeAll();
        app.jh.setresult(true);
    }
}




app.jh_id_dh_callback_work = function (selectVal) {
    
    if (typeof (selectVal) == 'undefined' || selectVal == '' || typeof (selectVal.id) == 'undefined' || selectVal.id == '') {
        $("#" + app.jh.div_name + ' #id_bill_origin', _).val('');
        $("#" + app.jh.div_name + ' #dh_origin', _).val('');
        $("#" + app.jh.div_name + ' #bm_djlx_origin', _).val('');
        $("#" + app.jh.div_name + ' #gys_id', _).val('');
        $("#" + app.jh.div_name + ' #gys_name', _).val('');
        $(_ + "#" + app.jh.div_name + " #gys_group_btn").removeAttr("disabled");
        $(_ + "#" + app.jh.div_name + " #gys_name").removeAttr("disabled");
        $(_ + "#" + app.jh.div_name + " #gys_group_span").removeAttr("disabled");
        $(_ + "#" + app.jh.div_name + " #id_shop").removeAttr("disabled");
        $(_ + "#" + app.jh.div_name + " #div_id_shop").find('button[data-id="id_shop"]').removeClass("disabled");

        $("button:contains('导入')", _).removeClass('disabled').attr('onclick', 'app.jh.importin()');
        $("button:contains('扫描')", _).removeClass('disabled').attr('onclick', 'app.jh.sm(this)');
        $("#shopsp_table a:contains('选择')", _).removeClass('disabled').attr('onclick', 'app.jh.showshopsp();');
        $("button:contains('增加行')", _).removeClass('disabled').attr('onclick', 'app.jh.addshopsp_row(this)');


    } else {
        $("#" + app.jh.div_name + ' #id_bill_origin', _).val(selectVal.id);
        $("#" + app.jh.div_name + ' #dh_origin', _).val(selectVal.dh);
        $("#" + app.jh.div_name + ' #bm_djlx_origin', _).val(selectVal.bm_djlx);
        $("#" + app.jh.div_name + ' #gys_id', _).val(selectVal.id_gys);
        $("#" + app.jh.div_name + ' #gys_name', _).val(selectVal.gys_name);
        $("#" + app.jh.div_name + ' #remark', _).val(selectVal.bz);
        $(_ + "#" + app.jh.div_name + " #id_shop").removeAttr("disabled");
        var index = $("#" + app.jh.div_name + ' #id_shop', _).find('option[value="' + selectVal.id_shop + '"]').index();
        var objShop = $(_ + "#" + app.jh.div_name + ' select[name="id_shop"]').prev();
        objShop.find('ul li').removeAttr("class");
        app.jh.flag1 = false;
        $(objShop.find('ul li')[index]).find('a').click();
        $(_ + "#" + app.jh.div_name + " #div_id_shop").find('button[data-id="id_shop"]').addClass("disabled");
        $(_ + "#" + app.jh.div_name + " #gys_group_btn").attr("disabled", "disabled");
        $(_ + "#" + app.jh.div_name + " #gys_name").attr("disabled", "disabled");
        $(_ + "#" + app.jh.div_name + " #gys_group_span").attr("disabled", "disabled");
        $(_ + "#" + app.jh.div_name + " #id_shop").attr("disabled", "disabled");
        $(_ + "#" + app.jh.div_name + " #id_shop_sh_hid").val(selectVal.id_shop);




        //Post读取数据赋值
        cy.http.Post({
            url: '/dh/GetDhDetailList',
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
                            shopsp_e.sl = ret.Data.body[item].sl;
                            shopsp_e.je = ret.Data.body[item].je;
                            
                            shopsp_e.bz = ret.Data.body[item].bz;
                            shopsp.push(shopsp_e);
                        }
                    }

                    var jsonStr = JSON.stringify(shopsp);
                    app.jh.removeAll();
                    app.jh.dialogCallBackWork(jsonStr);
                    app.jh.setresult(true);

                    
                    $("button:contains('导入')", _).addClass('disabled').removeAttr('onclick');
                    $("button:contains('扫描')", _).addClass('disabled').removeAttr('onclick');
                    $("#shopsp_table a:contains('选择')", _).addClass('disabled').removeAttr('onclick');
                    $("button:contains('增加行')", _).addClass('disabled').removeAttr('onclick');

                }
            },
            complete: function () { }
        });
    }


}












