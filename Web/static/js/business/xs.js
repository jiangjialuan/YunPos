


app.xs = app.xs || {};

app.xs.div_name="xs_add";
debugger;
app.xs.sl_digit=$(_+"#"+app.xs.div_name+'  #sl_digit').val();
app.xs.dj_digit = $(_ + "#" + app.xs.div_name + '  #dj_digit').val();
app.xs.je_digit = $(_ + "#" + app.xs.div_name + '  #je_digit').val();
app.xs.type = $(_ + "#" + app.xs.div_name + '  #type').val();
app.xs.head_id = $(_ + "#" + app.xs.div_name + '  #head_id').val();
app.xs.head_dh = $(_ + "#" + app.xs.div_name + '  #head_dh').val();
app.xs.default_dj = $(_ + "#" + app.xs.div_name + '  #default_dj').val();
app.xs.default_sl = $(_ + "#" + app.xs.div_name + '  #default_sl').val();
app.xs.default_je = $(_ + "#" + app.xs.div_name + '  #default_je').val();
app.xs.id_shop_master = $(_ + "#" + app.xs.div_name + '  #id_shop_master').val();


$.DHB._ = function () {
    app.c.public_data['xs/add'] = app.c.public_data['xs/add'] || {};
    if (app.c.public_data['xs/add']['once'] === false) {
        app.c.public_data['xs/add']['once'] = true;
        $.DHB.checkForm(function () {
            if (!$(_ + "#" + app.xs.div_name + " #kh_id").val()) {
                $.DHB.message({ 'content': '请选择客户！', 'type': 'i' });
            }
            else if ($(_ + "#" + app.xs.div_name + " #shopsp_table>tbody>tr[data-item!='']").length <= 0) {
                $.DHB.message({ 'content': '请至少选择一个商品！', 'type': 'i' });
            }
            else {
                app.xs.setresult();
                var objData = {};
                objData.shopspList = $(_ + 'input[name="table_result"]').val();
                objData.dh = $(_ + "#" + app.xs.div_name + ' #dh').val();
                objData.id_shop = $(_ + "#" + app.xs.div_name + ' #id_shop').val();
                objData.rq = $(_ + "#" + app.xs.div_name + ' #rq').val();
                //objData.rq_jh = $(_ + "#" + app.xs.div_name + ' #rq_jh').val();
                objData.flag_ddlx = $(_ + "#" + app.xs.div_name + ' #flag_ddlx').val();
                objData.flag_dhtype = $(_ + "#" + app.xs.div_name + ' #flag_dhtype').val();
                objData.id_kh = $(_ + "#" + app.xs.div_name + ' #kh_id').val();
                objData.id_jbr = $(_ + "#" + app.xs.div_name + ' #id_jbr').val();
                objData.remark = $(_ + "#" + app.xs.div_name + ' #remark').val();

                objData.type = app.xs.type;
                objData.id = app.xs.head_id;

                var options = {
                    data: objData,
                    url: $.DHB.U('xs/add'),
                    type: "POST",
                    datatype: 'json',
                    beforeSend: function () { },
                    success: function (data, textStatus, jqXHR) {
                        if (data.status == "success") {
                            $.DHB.message({ 'content': data.message, 'time': 4000, 'type': 's' });
                            if ($(_ + "#" + app.xs.div_name + ' #add_new').val() == '0') {
                                $.fn.menuTab.deleteMenu('xs/add');
                                $.DHB.url('xs/list', 'cache', '销售订单');
                            }
                            else {
                                $.fn.menuTab.deleteMenu('xs/add');
                                $.fn.menuTab.load({ url: '/xs/add', 'title': '新增销售订单', id: 'xs/add', nocache: '0' });
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
            $(_ + "#" + app.xs.div_name +  ' #submit-button').removeAttr('disabled');
        });
    }
};


var options_xs_add_createdh= {
    data: {},
    url: $.DHB.U('/xs/createdh'),
    type: "POST",
    datatype: 'json',
    beforeSend: function () { },
    success: function (ret, textStatus, jqXHR) {
        if (ret.Success) {
            if (app.xs.type != 'edit') {
                $(_ + "#" + app.xs.div_name + " #dh").val(ret.Data);
                $(_ + "#" + app.xs.div_name + " #dh").attr('old-data', ret.Data);
            } else {
                $(_ + "#" + app.xs.div_name + " #dh").val(app.xs.head_dh);
                $(_ + "#" + app.xs.div_name + " #dh").attr('old-data', app.xs.head_dh);
            }
        }
    },
    complete: function (XHR, TS) { }
};
app.httpAjax.post(options_xs_add_createdh);


//选择客户回调
app.xs.add__add_kh_callback = function (selectVal) {
    selectVal = selectVal || '';
    var selectValTem = selectVal.split('|');
    if (selectValTem[0] != 'undefined' && selectValTem[0] != '') {
        $("#" + app.xs.div_name + ' #kh_id', _).val(selectValTem[0]);
    }
    if (selectValTem[1] != 'undefined' && selectValTem[1] != '') {
        $("#" + app.xs.div_name + ' #kh_name', _).val(selectValTem[1]);
    }
}



function dialogCallBack(array) {
    
    //var jsonStr = array[5].value;

    var jsonStr = "";
    $.each(array, function (index, item) {
        if (item.name == "shopsp_table_json") {
            jsonStr = item.value;
        }
    });

    dialogCallBackWork(jsonStr);
    //关闭dialog
    $.DHB.dialog({ 'id': 'dialog-shopsp-search', 'action': 'destroy' });
    app.xs.setresult(true);
}

//选择商品回调
function dialogCallBackWork(jsonStr) {
    var data_item_last = '';
    var objList = jQuery.parseJSON(jsonStr);
    $(objList).each(function (i, obj) {
        var guid = parseInt($(_ + "#" + app.xs.div_name + " #shopspnum").val()) + 1;
        var option = '';
        var resultHtml = '';
        resultHtml += '<tr data-item="' + obj.id_shopsp + '">';
        resultHtml += '<td class="align_center">  <div name="xh">' + guid + '</div>  <input class="form-control user-input" style="width:110px;display:none;" placeholder="商品ID" name="shopsp_obj" type="text" value="' + obj.id_shopsp + '"  data-id_kcsp="' + obj.id_kcsp + '"  data-id_sp="' + obj.id_sp + '"></td>';

        var sl = limit_num(obj.sl.toString(), app.xs.sl_digit);
        debugger;

        //var dj = limit_num(obj.dj_jh.toString(), app.xs.dj_digit);//修改成按批发价来
        var dj = limit_num(obj.dj_pf.toString(), app.xs.dj_digit);
        var je = accMul(parseFloat(dj), parseFloat(sl));
        je = limit_num(je.toString(), app.xs.je_digit);
        var dj_ls = limit_num(obj.dj_ls.toString(), app.xs.dj_digit);
        var sl_kc = limit_num(obj.sl_qm.toString(), app.xs.dj_digit);

        resultHtml += ' <td><input class="form-control user-input col-md-2" style="border:0px;" onmouseover="app.xs.onmouseover(this)" onmouseout="app.xs.onmouseout(this)"  type="text" value="' + obj.barcode + '" name="barcode" old-data="' + obj.barcode + '"></td>';
        resultHtml += '<td name="mc"> <div style="width: 100% !important;text-align:left;padding:0 0;"> <span>' + obj.mc + '</span><a href="javascript:void(0)" onclick="app.xs.showshopsp();" class="btn btn-info f-r">  选择</a></div> </td>';

        debugger;

        if (typeof (obj.dw) != 'undefined' && obj.dw != 'undefined' && obj.dw != '') {
            resultHtml += '<td name="dw">    <div id="dw_select" name="dw_select"  class="common_select" style="display:block;"><div onclick="app.xs.dw_select_xs_add_onclick(this)"><span class="inline" data-zhl="' + limit_num(obj.zhl.toString(), app.xs.sl_digit) + '">' + obj.dw + '</span> <i class="fa fa-caret-down"></i></div><ul class="common_select_list"><li value="' + limit_num(obj.dj_pf.toString(), app.xs.dj_digit) + '"  data-zhl="' + limit_num(obj.zhl.toString(), app.xs.sl_digit) + '"    data-id_shopsp="' + obj.id_shopsp + '"   data-id_kcsp="' + obj.id_kcsp + '"  data-id_sp="' + obj.id_sp + '" data-dw="' + obj.dw + '" data-dj_ls="' + dj_ls + '"     data-mc="' + obj.mc + '" data-barcode="' + obj.barcode + '" data-sl_kc="' + sl_kc + '" >' + obj.dw + '</li></ul></div></td>';
        } else {
            resultHtml += '<td name="dw">    <div id="dw_select" name="dw_select"   class="common_select"><div onclick="app.xs.dw_select_xs_add_onclick(this)"><span class="inline">单位</span> <i class="fa fa-caret-down"></i></div><ul value="" class="common_select_list"></ul></div></td>';
        }


        resultHtml += '<td><input class="form-control user-input" style="width:96%;border:0px;text-align:right;" onmouseover="app.xs.onmouseover(this)" onmouseout="app.xs.onmouseout(this)" placeholder="数量" type="text" value="' + sl + '" name="sl" onkeyup="check_digit(this,\'' + app.xs.sl_digit + '\')" onafterpaste="check_digit(this,\'' + app.xs.sl_digit + '\')" old-data="' + sl + '">    <input name="sl_zhl" style="width:95%;display:none;text-align:right;" placeholder="转换率" type="text" value="1">  <input name="sl_total" style="width:95%;display:none;text-align:right;" placeholder="数量总数" type="text" value="' + app.xs.default_sl + '"></td>';
        resultHtml += ' <td><input class="form-control user-input" style="width:96%;border:0px;text-align:right;" onmouseover="app.xs.onmouseover(this)" onmouseout="app.xs.onmouseout(this)" placeholder="单价(元)" type="text" value="' + dj + '" name="dj_jh" old-data="' + dj + '"   onkeyup="check_digit(this,\''+app.xs.dj_digit+'\')" onafterpaste="check_digit(this,\''+app.xs.dj_digit+'\')" ></td>';
        resultHtml += ' <td name="je"> <input class="form-control user-input" style="width:96%;border:0px;text-align:right;" onmouseover="app.xs.onmouseover(this)" onmouseout="app.xs.onmouseout(this)" placeholder="金额(元)" type="text" value="' + je + '" name="je" old-data="' + je + '"   onkeyup="check_digit(this,\''+app.xs.je_digit+'\')" onafterpaste="check_digit(this,\''+app.xs.je_digit+'\')" ></td>';



        resultHtml += ' <td name="dj_ls"><div style="width: 100% !important;text-align:right;padding:0 0;">' + dj_ls + '</div></td> ';
        resultHtml += ' <td name="sl_kc"><div style="width: 100% !important;text-align:right;padding:0 0;">' + sl_kc + '</div></td> ';


        if (typeof (obj.bz) != 'undefined' && obj.bz != 'undefined' && obj.bz != '') {
            resultHtml += ' <td><input class="form-control user-input" style="width:96%;border:0px;" onmouseover="app.xs.onmouseover(this)" onmouseout="app.xs.onmouseout(this)" type="text" value="' + obj.bz + '" name="bz"></td>';
        } else {
            resultHtml += ' <td><input class="form-control user-input" style="width:96%;border:0px;" onmouseover="app.xs.onmouseover(this)" onmouseout="app.xs.onmouseout(this)" type="text" value="" name="bz"></td>';
        }
        resultHtml += '<td class="align_center"><a onclick="app.xs.choice_spec_del(this);" class="bg-state bg-state-info blockbtn fa fa-trash" title="删除"></a></td>';
        resultHtml += ' </tr>';
        $(_ + "#" + app.xs.div_name +  " #shopspnum").val(guid);
        $(_ + "#" + app.xs.div_name +  " #shopsp_table>tbody").append(resultHtml);
        app.xs.bindkeypresslast();
        data_item_last = obj.id_shopsp;
    });
}

app.xs.addshopsp_row = function (obj) {
    app.xs.addshopsp();
    reset_xh();
};
   

//添加商品至table
app.xs.addshopsp = function (obj) {
    var shopsp = [];
    var shopsp_e = {};
    shopsp_e.id_shopsp = "";
    shopsp_e.id_kcsp = "";
    shopsp_e.id_sp = "";
    shopsp_e.barcode = "";
    shopsp_e.mc = "";
    shopsp_e.dw = "";
    shopsp_e.zhl = "1";
    shopsp_e.dj_jh = app.xs.default_dj;
    shopsp_e.sl = app.xs.default_sl;
    shopsp_e.dj_ls = app.xs.default_dj;
    shopsp_e.sl_qm = app.xs.default_sl;
    shopsp_e.dj_pf = app.xs.default_dj;

    shopsp.push(shopsp_e);
    var jsonStr = JSON.stringify(shopsp);
    dialogCallBackWork(jsonStr);
}

//删除商品的数据
app.xs.choice_spec_del = function (el) {
    $(el).parents('tr').remove();
    app.xs.setresult(false);
}

//改动后重新计算和赋值
app.xs.setresult = function (ifRemove) {
    var e = [];
    var shopsp = [];
    var totleMoney = 0;
    var xh = 1;

    if (ifRemove) {
        $("#" + app.xs.div_name + ' #shopsp_table>tbody').find('tr[data-item=""]').each(function () {
            $(this).remove();
        });
    }

    $("#" + app.xs.div_name +  ' #shopsp_table>tbody').find('tr').each(function () {
        $(this).find('div[name="xh"]').text(xh);
        var shopsp_e = {};
        var shopsp_obj = $(this).find('input[name="shopsp_obj"]');
        shopsp_e.id_shopsp = shopsp_obj.val();
        shopsp_e.id_kcsp = $.trim(shopsp_obj.attr("data-id_kcsp"));//仓库id
        shopsp_e.id_sp = $.trim(shopsp_obj.attr("data-id_sp"));//id_sp
        shopsp_e.sl = $.trim($(this).find('input[name="sl"]').val()); //数量
        shopsp_e.dj = $.trim($(this).find('input[name="dj_jh"]').val()); //单价
        shopsp_e.barcode = $.trim($(this).find('input[name="barcode"]').val());//条码
        shopsp_e.dw = $(this).find('div[name=dw_select]>div>span').html();//单位
        shopsp_e.bz = $.trim($(this).find('input[name="bz"]').val());//备注
        shopsp_e.zhl = $.trim($(this).find('div[name=dw_select]>div>span').attr('data-zhl')); //转换率
        shopsp_e.shopsp_name = $(this).find('td[name="mc"] div span').text();;//商品名称
        shopsp_e.dj_ls = $(this).find('td[name="dj_ls"] div').text();
        shopsp_e.sl_kc = $(this).find('td[name="sl_kc"] div').text();

        //数量总数
        var sl_total = accMul(parseFloat(shopsp_e.zhl), parseFloat(shopsp_e.sl));                     //(parseFloat(shopsp_e.zhl) * parseFloat(shopsp_e.sl));
        sl_total = limit_num(sl_total.toString(), app.xs.sl_digit)
        $(this).find('input[name="sl_total"]').val(sl_total);
        shopsp_e.sl_total = $.trim(sl_total);

        shopsp_e.je = $.trim($(this).find('input[name="je"]').val()); //金额

        if (shopsp_e.id_shopsp != '') {
            shopsp.push(shopsp_e);
        }
        xh++;

        //当前金额
        var moneyTotal = limit_num(shopsp_e.je.toString(), app.xs.dj_digit)
        //总金额
        totleMoney = accAdd(totleMoney, parseFloat($.trim(moneyTotal)));
    });

    if (ifRemove) {
        $(_ + "#" + app.xs.div_name + " #shopspnum").val(parseInt(xh) - 1);
        app.xs.addshopsp();
        app.xs.addshopsp();
        app.xs.addshopsp();
        app.xs.addshopsp();
        app.xs.addshopsp();
        app.xs.addshopsp();
        app.xs.addshopsp();
        app.xs.addshopsp();
        app.xs.addshopsp();
        app.xs.addshopsp();
        reset_xh();
    }


    totleMoney = limit_num(totleMoney.toString(), app.xs.je_digit)
    $(_ + "#" + app.xs.div_name + ' #edit_goods_total').text(totleMoney);
    var jsonStr = JSON.stringify(shopsp);
    $(_ + "#" + app.xs.div_name + " #table_result").val(jsonStr);

}

//时间控件
app.xs.wdatepicker_xs_list = function (el) {
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
        app.search.do_search_xs_list();
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
        app.search.do_search_xs_list();
    };

    WdatePicker(option);
}




//绑定事件
app.xs.bindkeypress = function (obj) {
    $("#" + app.xs.div_name + ' #shopsp_table>tbody',_).find('tr').each(function () {
        bindEvent($(this));
    });
}

//默认绑定事件
app.xs.bindkeypress();

//绑定最后一条tr的事件
app.xs.bindkeypresslast = function () {
    $("#" + app.xs.div_name + " #shopsp_table>tbody>tr:last",_).each(function () {
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
                                tr.attr("data-item", ret.Data.id);
                                tr.find('td[name="mc"] div span').text(ret.Data.mc);
                                tr.find('input[name="sl"]').val(limit_num('1', app.xs.sl_digit));

                                //tr.find('input[name="dj_jh"]').val(limit_num(ret.Data.dj_jh.toString(), app.xs.dj_digit));//修改成按批发价
                                tr.find('input[name="dj_jh"]').val(limit_num(ret.Data.dj_pf.toString(), app.xs.dj_digit));
                                tr.find('input[name="je"]').val(limit_num(ret.Data.dj_pf.toString(), app.xs.dj_digit));
                                //设置old-data
                                barcode_obj.attr('old-data', barcode_obj.val());
                                sl_obj.attr('old-data', limit_num('1', app.xs.sl_digit));
                                dj_ls_obj.attr('old-data', limit_num(ret.Data.dj_jh.toString(), app.xs.dj_digit));
                                je_obj.attr('old-data', limit_num(ret.Data.dj_jh.toString(), app.xs.dj_digit));

                                tr.find('input[name="shopsp_obj"]').attr("value", ret.Data.id);

                                tr.find('input[name="sl"]').focus().select();

                                tr.find('td[name="dj_ls"] div').text(limit_num(ret.Data.dj_ls.toString(),app.xs.dj_digit));
                                tr.find('td[name="sl_kc"] div').text(limit_num(ret.Data.sl_qm.toString(),app.xs.sl_digit));


                                //转换率
                                var zhl = ret.Data.zhl.toString();
                                zhl = limit_num(zhl, app.xs.sl_digit)
                                //var dj_jh = ret.Data.dj_jh.toString();//用批发价
                                var dj_jh = ret.Data.dj_pf.toString();
                                dj_jh = limit_num(dj_jh,app.xs.dj_digit)

                                var dj_ls = ret.Data.dj_ls.toString();
                                dj_ls = limit_num(dj_ls, app.xs.dj_digit)
                                    

                                //dw_select.append('<option value=' + dj_jh + '  data-zhl=' + zhl + ' data-id_shopsp=' + ret.Data.id + ' data-id_kcsp=' + ret.Data.id_kcsp + '  data-id_sp=' + ret.Data.id_sp + ' data-dw=' + ret.Data.dw + ' data-dj_ls="' + dj_ls + '" > ' + ret.Data.dw + '(' + zhl + ')' + '</option> ');
                                tr.find('input[name="sl_zhl"]').val(zhl);

                                tr.find('input[name="shopsp_obj"]').attr("data-id_kcsp", ret.Data.id_kcsp);
                                tr.find('input[name="shopsp_obj"]').attr("data-id_sp", ret.Data.id_sp);
                                //绑定下拉
                                tr.find('div[name=dw_select]>div>span').html(ret.Data.dw);
                                tr.find('div[name=dw_select]>div>span').attr("data-zhl", ret.Data.zhl);
                                tr.find('div[name=dw_select]').css('display', 'block');
                                tr.find('ul').append('<li  value=' + dj_jh + ' data-mc=' + ret.Data.mc + ' data-barcode=' + ret.Data.barcode + ' data-sl_kc=' + ret.Data.sl_kc + '    data-zhl=' + zhl + ' data-id_shopsp=' + ret.Data.id + ' data-id_kcsp=' + ret.Data.id_kcsp + '   data-id_sp=' + ret.Data.id_sp + '  data-dj_ls="' + dj_ls + '"   > ' + ret.Data.dw + '</li>');


                                var tbody = tr.parents("tbody");
                                var data_item = tbody.find("tr:last").attr("data-item");
                                if (data_item == ret.Data.id) {
                                    app.xs.addshopsp();//新增最后一条新记录
                                    reset_xh();
                                }
                                app.xs.setresult(false);
                            } else {
                                $.DHB.dialog({ 'title': '选择商品', 'url': $.DHB.U('shopsp/search?keyword=' + barcode_obj.val() + '&id_shop=' + $(_ + '#id_shop').val()), 'id': 'dialog-shopsp-search', 'confirm': dialogCallBack });
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
                    tr_next.find('input[name="barcode"]').focus().select();
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
                    tr_next.find('input[name="barcode"]').focus().select();
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
                    tr_next.find('input[name="barcode"]').focus().select();
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
                        app.xs.addshopsp();
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
            var tr = sl_obj.parents("tr");
            var data_item = tr.attr('data-item');
            if (typeof (data_item) != 'undefined' && data_item != 'undefined' && data_item != '') {
                //数量变了金额变
                var dj = limit_num($.trim(tr.find('input[name="dj_jh"]').val()), app.xs.dj_digit); //单价
                var sl = limit_num($.trim(tr.find('input[name="sl"]').val()), app.xs.sl_digit); //数量
                var old_je = limit_num($.trim(tr.find('input[name="je"]').val()), app.xs.je_digit); //旧金额
                var je = accMul(parseFloat(dj), parseFloat(sl));//金额
                //如果旧金额除以数量取小数后等于现在的单价 金额则不变
                if (parseFloat(sl) != 0) {
                    var dj_temp = accDiv(parseFloat(old_je), parseFloat(sl));
                    dj_temp = limit_num($.trim(dj_temp), app.xs.dj_digit);
                    if (parseFloat(dj_temp) == parseFloat(dj)) {
                        je = old_je;
                    }
                }
                je = limit_num($.trim(je), app.xs.je_digit);
                tr.find('input[name="je"]').val(je);
                tr.find('input[name="je"]').attr('old-data', je);
                tr.find('input[name="sl"]').val(sl);
                tr.find('input[name="sl"]').attr('old-data', sl);
                app.xs.setresult(false);
            } else {
                sl_obj.val(app.xs.default_sl);
            }

        });
        dj_ls_obj.blur(function () {
            var tr = dj_ls_obj.parents("tr");
            var data_item = tr.attr('data-item');
            if (typeof (data_item) != 'undefined' && data_item != 'undefined' && data_item != '') {
                //单价变了金额变
                var dj = limit_num($.trim(tr.find('input[name="dj_jh"]').val()), app.xs.dj_digit); //单价
                var sl = limit_num($.trim(tr.find('input[name="sl"]').val()), app.xs.sl_digit); //数量
                var old_je = limit_num($.trim(tr.find('input[name="je"]').val()), app.xs.je_digit); //旧金额
                var je = accMul(parseFloat(dj), parseFloat(sl));//金额



                //如果旧金额除以数量取小数后等于现在的单价 金额则不变
                if (parseFloat(sl) != 0) {

            

                    var dj_temp = accDiv(parseFloat(old_je), parseFloat(sl));
                    dj_temp = limit_num($.trim(dj_temp), app.xs.dj_digit);
                    if (parseFloat(dj_temp) == parseFloat(dj)) {
                        je = old_je;
                    }
                }
                je = limit_num($.trim(je),app.xs.je_digit);
                tr.find('input[name="je"]').val(je);
                tr.find('input[name="je"]').attr('old-data', je);
                tr.find('input[name="dj_jh"]').val(dj);
                tr.find('input[name="dj_jh"]').attr('old-data', dj);
                app.xs.setresult(false);
            } else {
                dj_ls_obj.val(app.xs.default_dj); 
            }
        });
        je_obj.blur(function () {
            var tr = je_obj.parents("tr");
            var data_item = tr.attr('data-item');
            if (typeof (data_item) != 'undefined' && data_item != 'undefined' && data_item != '') {
                //金额变了单价变

                var old_dj = limit_num($.trim(tr.find('input[name="dj_jh"]').val()), app.xs.dj_digit); //旧单价
                var je = limit_num($.trim(tr.find('input[name="je"]').val()), app.xs.je_digit); //金额
                var sl = limit_num($.trim(tr.find('input[name="sl"]').val()), app.xs.sl_digit); //数量
                var dj = app.xs.default_dj;//单价

                if (parseFloat(sl) != 0) {
                    var temp_je = accMul(parseFloat(old_dj), parseFloat(sl));//金额
                    temp_je = limit_num($.trim(temp_je), app.xs.je_digit);
                    if (je == temp_je) {
                        dj = old_dj;
                    } else {
                        var dj_temp = accDiv(parseFloat(je), parseFloat(sl));
                        dj_temp = limit_num($.trim(dj_temp), app.xs.dj_digit);
                        dj = dj_temp;
                    }

                }

                tr.find('input[name="dj_jh"]').val(dj);
                tr.find('input[name="dj_jh"]').attr('old-data', dj);
                tr.find('input[name="je"]').val(je);
                tr.find('input[name="je"]').attr('old-data', je);
                app.xs.setresult(false);
            } else {
                je_obj.val(app.xs.default_je);
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
app.xs.dw_select_onchange = function (e) {
    debugger;
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

    var dj = limit_num($(e).attr('value').toString(), app.xs.dj_digit);//单价
    var sl = limit_num($.trim(tr.find('input[name="sl"]').val()).toString(), app.xs.sl_digit); //数量

    var dj_ls = $(e).attr('data-dj_ls');
    if (typeof (dj_ls) != 'undefined' && dj_ls != 'undefined' && dj_ls != '') {
        dj_ls = limit_num($.trim(dj_ls).toString(), app.xs.dj_digit);
        tr.find('td[name="dj_ls"] div').html(dj_ls);
    }

    var je = accMul(parseFloat(dj), parseFloat(sl));
    je = limit_num($.trim(je).toString(), app.xs.je_digit);//金额


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

    app.xs.setresult(false);
};








app.xs.onmouseover = function onmouseover(e) {
    var input = $(e);
    input.css({ border: "1px solid #ccc" })
};

app.xs.onmouseout = function onmouseout(e) {
    var input = $(e);
    input.css({ border: "0px solid #ccc" })
};


app.xs.sm = function (e) {
    var div_sm = $(_ + "#" + app.xs.div_name +  ' #div_sm');
    if (div_sm.css('display') == 'none') {
        $(_ + "#" + app.xs.div_name + ' #div_sm').css('display', 'block');
    } else {
        $(_ + "#" + app.xs.div_name + ' #div_sm').css('display', 'none');
    }
}
app.xs.add_ready = function () {

    $(_ + "#" + app.xs.div_name + " #id_shop_hid").val($(_ + "#" + app.xs.div_name + ' #id_shop').val());
    //点击单位下拉事件
    app.xs.dw_select_xs_add_onclick = function (e) {
        var id_shop = $('#id_shop', _).val();
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
                    data: { select_id_sp: data_item, ps_id_shop: id_shop },
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
                                        zhl = limit_num(zhl, app.xs.sl_digit)
                                        //var dj_jh = ret.Data[item].dj_jh.toString();//改用批发价
                                        debugger;
                                        var dj_jh = ret.Data[item].dj_pf.toString();
                                        dj_jh = limit_num(dj_jh, app.xs.dj_digit)

                                        var dj_ls = ret.Data[item].dj_ls.toString();
                                        dj_ls = limit_num(dj_ls, app.xs.dj_digit)

                                        

                                        var sl_kc = ret.Data[item].sl_qm.toString();
                                        sl_kc = limit_num(sl_kc, app.xs.sl_digit);

                                        str_li+='<li  value=' + dj_jh + ' data-mc=' + ret.Data[item].mc + ' data-barcode=' + ret.Data[item].barcode + ' data-sl_kc=' + sl_kc + '    data-zhl=' + zhl + ' data-id_shopsp=' + ret.Data[item].id + ' data-id_kcsp=' + ret.Data[item].id_kcsp + '   data-id_sp=' + ret.Data[item].id_sp + '  data-dj_ls="' + dj_ls + '"   > ' + ret.Data[item].dw + '</li> ';

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


    app.xs.sl_digit = $(_ + "#" + app.xs.div_name + '  #sl_digit').val();
    app.xs.dj_digit = $(_ + "#" + app.xs.div_name + '  #dj_digit').val();
    app.xs.je_digit = $(_ + "#" + app.xs.div_name + '  #je_digit').val();
    app.xs.type = $(_ + "#" + app.xs.div_name + '  #type').val();
    app.xs.head_id = $(_ + "#" + app.xs.div_name + '  #head_id').val();
    app.xs.head_dh = $(_ + "#" + app.xs.div_name + '  #head_dh').val();
    app.xs.default_dj = $(_ + "#" + app.xs.div_name + '  #default_dj').val();
    app.xs.default_sl = $(_ + "#" + app.xs.div_name + '  #default_sl').val();
    app.xs.default_je = $(_ + "#" + app.xs.div_name + '  #default_je').val();
    app.xs.id_shop_master = $(_ + "#" + app.xs.div_name + '  #id_shop_master').val();


    $("#" + app.xs.div_name + ' .table_list', _).on('mouseleave', '.common_select', function () {
        $('.common_select_list', _).hide();
    });
    $("#" + app.xs.div_name + ' .table_list', _).on('click', '.common_select_list li', function () {
        var li_txt = $(this).html();
        $(this).parents('.common_select_list').hide();
        $(this).parents('.common_select').find('span').html(li_txt);
        $(this).parents('.common_select').find('span').attr('data-zhl', $(this).attr('data-zhl'));
        app.xs.dw_select_onchange(this);
    });
    app.xs.setresult(true);

$(_ + "#" + app.xs.div_name + ' #div_sm').keyup(function (e) {
    if (e.keyCode == 13) {
        var scan = $(_ + "#" + app.xs.div_name + ' #barcode_search').val();
        $(_ + "#" + app.xs.div_name + ' #barcode_search').val('');

        var id_shop = $(_ + "#id_shop").val();
                //Post读取数据赋值
                cy.http.Post({
                    url: '/shopsp/GetShopspList',
                    data: { keyword: scan, id_shop: id_shop },
                    beforeSend: function () {
                    },
                    callback: function (ret) {
                        if (ret.Success) {
                            
                            $("#" + app.xs.div_name + ' #shopsp_table>tbody tr').each(function () {
                                var tr = $(this);
                            if (tr.attr('data-item') == ret.Data.id) {
                                var old_sl = tr.find('input[name=sl]').val();
                                tr.find('input[name=sl]').val((parseFloat(old_sl) + 1).toFixed(2));
                                tr.find('input[name=sl]').blur();
                                return false;
                            } else if (tr.attr('data-item') == "") {
                                tr.attr("data-item", ret.Data.id);
                                tr.find('td[name="mc"] div span').text(ret.Data.mc);
                                tr.find('input[name="sl"]').val(limit_num('1', app.xs.sl_digit));
                                //tr.find('input[name="dj_jh"]').val(limit_num(ret.Data.dj_jh.toString(), app.xs.dj_digit));//修改成按批发价
                                debugger;
                                tr.find('input[name="dj_jh"]').val(limit_num(ret.Data.dj_pf.toString(), app.xs.dj_digit));

                                tr.find('input[name="je"]').val(limit_num(ret.Data.dj_pf.toString(), app.xs.je_digit));
                                //设置old-data
                                tr.find('input[name="barcode"]').attr('old-data', scan);
                                tr.find('input[name="barcode"]').val(scan);
                                tr.find('input[name="sl"]').attr('old-data', limit_num('1', app.xs.sl_digit));
                                tr.find('input[name="dj_jh"]').attr('old-data', limit_num(ret.Data.dj_jh.toString(), app.xs.dj_digit));
                                tr.find('input[name="je"]').attr('old-data', limit_num(ret.Data.dj_jh.toString(), app.xs.je_digit));
                                tr.find('input[name="shopsp_obj"]').attr("value", ret.Data.id);
                                tr.find('td[name="dj_ls"] div').text(limit_num(ret.Data.dj_ls.toString(), app.xs.dj_digit));
                                tr.find('td[name="sl_kc"] div').text(limit_num(ret.Data.sl_qm.toString(), app.xs.dj_digit));
                               //转换率
                                var zhl = ret.Data.zhl.toString();
                                zhl = limit_num(zhl, app.xs.sl_digit)
                                //var dj_jh = ret.Data.dj_jh.toString();//按批发价

                                var dj_jh = ret.Data.dj_pf.toString();
                                dj_jh = limit_num(dj_jh, app.xs.dj_digit)
                                var dj_ls = ret.Data.dj_ls.toString();
                                dj_ls = limit_num(dj_ls, app.xs.dj_digit)
                                //dw_select.append('<option value=' + dj_jh + '  data-zhl=' + zhl + ' data-id_shopsp=' + ret.Data.id + ' data-id_kcsp=' + ret.Data.id_kcsp + ' data-id_sp=' + ret.Data.id_sp + '  data-dw=' + ret.Data.dw + ' data-dj_ls="' + dj_ls + '" > ' + ret.Data.dw + '(' + zhl + ')' + '</option> ');
                                tr.find('input[name="sl_zhl"]').val(zhl);
                                tr.find('input[name="shopsp_obj"]').attr("data-id_kcsp", ret.Data.id_kcsp);
                                tr.find('input[name="shopsp_obj"]').attr("data-id_sp", ret.Data.id_sp);
                                //绑定下拉
                                tr.find('div[name=dw_select]>div>span').html(ret.Data.dw);
                                tr.find('div[name=dw_select]>div>span').attr("data-zhl", ret.Data.zhl);
                                tr.find('div[name=dw_select]').css('display', 'block');
                                tr.find('ul').append('<li  value=' + dj_jh + ' data-mc=' + ret.Data.mc + ' data-barcode=' + ret.Data.barcode + ' data-sl_kc=' + ret.Data.sl_kc + '    data-zhl=' + zhl + ' data-id_shopsp=' + ret.Data.id + ' data-id_kcsp=' + ret.Data.id_kcsp + '   data-id_sp=' + ret.Data.id_sp + '  data-dj_ls="' + dj_ls + '"   > ' + ret.Data.dw + '</li>');
                                
                                var tbody = tr.parents("tbody");
                                var data_item = tbody.find("tr:last").attr("data-item");
                                if (data_item == ret.Data.id) {
                                    app.xs.addshopsp();//新增最后一条新记录
                                    reset_xh();
                                }
                                app.xs.setresult(false);
                                return false;
                            }
                            
                            });
                        } else {
                            $.DHB.dialog({ 'title': '选择商品', 'url': $.DHB.U('shopsp/search?keyword=' + scan), 'id': 'dialog-shopsp-search', 'confirm': dialogCallBack });
                        }
                    },
                    complete: function () { }
                
                
            
            
        });
    }
});

}

app.xs.importin = function () {
    var id_shop = $(_ + "#id_shop").val();
    $.DHB.dialog({ 'title': '销售导入', 'url': $.DHB.U('xs/importin?id_shop=' + id_shop), 'id': 'dialog-dhdr-search' });
}

function dialogxsDRCallBack(jsonStr) {

    dialogCallBackWork(jsonStr);
    app.xs.setresult(true);
    $.DHB.message({ 'content': '操作完毕', 'time': 4000, 'type': 's' });
}

//导出
app.xs.importout = function () {
    app.xs.setresult(false);
    if ($(_ + "#" + app.xs.div_name + " #shopsp_table>tbody>tr[data-item!='']").length <= 0) {
        $.DHB.message({ 'content': '无有效的数据！', 'type': 'i' });
    }
    else {
        var shopspList = $(_ + "#" + app.xs.div_name + ' input[name="table_result"]').val();

        var turnForm_dh = document.createElement("form");
        //一定要加入到body中！！   
        document.body.appendChild(turnForm_dh);
        turnForm_dh.method = 'post';
        turnForm_dh.action = '/xs/importout';
        turnForm_dh.target = '_blank';
        //创建隐藏表单
        var newElement_dh = document.createElement("input");
        newElement_dh.setAttribute("name", "shopspList");
        newElement_dh.setAttribute("type", "hidden");
        newElement_dh.setAttribute("value", shopspList);
        turnForm_dh.appendChild(newElement_dh);
        turnForm_dh.submit();

    }

}

//定位
app.xs.dw = function (obj) {
    $(_ + "#define-xs-dw").modal('show');
};

//定位确定
app.xs.dwsub = function (obj) {
    var czwz = $(_ + "#" + app.xs.div_name + " #id_czwz").val();
    var cznr = $(_ + "#" + app.xs.div_name + " #id_cznr").val();
    if (cznr == '') {
        $.DHB.message({ 'content': '请填写查找内容！', 'type': 'i' });
        return false;
    }

    var tbody = $("#" + app.xs.div_name + ' #shopsp_table>tbody');

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


app.xs.onblur = function (e, num) {
    var data = $(e).val();
    $(e).val(limit_num($.trim(data), num));
};

app.xs.onfocus = function (e) {
    $(e).select();
};

app.xs.showshopsp = function () {
    $.DHB.dialog({ 'title': '选择商品', 'url': $.DHB.U('shopsp/search?id_shop=' + $(_ + '#id_shop').val()), 'id': 'dialog-shopsp-search', 'confirm': dialogCallBack });
};


function reset_xh(e) {
    var xh = 1;
    $("#" + app.xs.div_name + ' #shopsp_table>tbody').find('tr').each(function () {
        $(this).find('div[name="xh"]').text(xh);
        xh++;
    });
}

app.xs.checkbarcode = function (obj) {
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


app.xs.flag1 = true;

//制单门店值发生改变触发事件
app.xs.id_shop_onchange = function (e) {
    if ($(_ + "#" + app.xs.div_name + " #shopsp_table>tbody>tr[data-item!='']").length <= 0) {
        app.xs.change_shop_work(e);
    }
    else {
        if (app.xs.flag1 == true) {
            $.messager.confirm("提示", "更改门店将刷新并删除非此门店的商品是否继续?", function () {
                app.xs.change_shop_work(e);
            }, function () {
                app.xs.flag1 = false;
                $(e).prev().find('li').eq(app.xs.select_index1).find('a').click();
            });
        } else {
            app.xs.change_shop_work(e);
        }
    }
};

//处理门店改变
app.xs.change_shop_work = function (e) {
    debugger;
    var id_shop_hid = $(_ + "#" + app.xs.div_name + ' #id_shop_hid').val();
    var id_shop = $(_ + "#" + app.xs.div_name + ' #id_shop').val();
    $(_ + "#" + app.xs.div_name + " #id_shop_hid").val(id_shop);
    if (id_shop_hid != id_shop && id_shop_hid!='') {
        $(_ + "#" + app.xs.div_name + ' #shopsp_table>tbody').find('tr').each(function () {
            $(this).remove();
        });
        app.xs.setresult(true);
    }
    app.xs.flag1 = true;
};




app.xs.option = function (e) {
    app.xs.flag1 = true;
    app.xs.select_index1 = $(e).find('li[class="selected"]').index();
};




