app.promote = app.promote || {};
app.promote.dpzkadd = app.promote.dpzkadd || {};
app.promote.dpzkadd_ready = function () {

    //日期插件初始化
    $.DHB.loadJs([{ id: 'WdatePicker', url: '/static/js/My97DatePicker/WdatePicker.js' }], function () {
        $('input[name=day_b],input[name=day_e]', _).click(function () {
            WdatePicker({ dateFmt: 'yyyy-MM-dd' });
        });

    });
    //日期与星期天的切换
    app.promote.dpzkadd.setDiaplay = function (input) {

        var qz = $(input).attr("value");
        $('div[data-radio]', _).each(function () {

            if ($(this).attr('data-value') == qz) {
                $(this).show();
            } else {
                $(this).hide();
            };
        })
    }
    //门店全选
    $('.selct_shop_head input', _).click(function () {

        if ($(this).is(":checked")) {
            $('.selct_shop_item .item_blank input', _).not("input:checked").click();
        } else {
            $('.selct_shop_item .item_blank input:checked', _).click();
        }
    });
    $('input[name=id_shops]').click(function () {
        if (!$(this).is(':checked')) {
            $('.selct_shop_head input', _).removeAttr('checked');
        } else {
            if ($('input[name=id_shops]:checked').length == $('input[name=id_shops]').length) {
                if (!$('.selct_shop_head input', _).is(':checked')) {
                    $('.selct_shop_head input', _).click();
                }
            }
        }
    });
    //日期全选
    app.promote.dpzkadd.setSelectAll = function (input) {

        var t = $(input).attr("data-type");
        if ($(input).is(':checked')) {
            $("input[data-type=" + t + "]", _).not("input:checked").click();
        } else {
            $("input[data-type=" + t + "]:checked", _).click();
        }
    }
    $(_ + 'input[data-type=rq]:not(:first)').click(function () {

        if (!$(this).is(':checked')) {
            $('input[data-type=rq]', _).eq(0).removeAttr('checked');
        } else {

            if ($('input[data-type=rq]:checked', _).length == 31) {
                if (!$('input[data-type=rq]', _).eq(0).is(':checked')) {
                    $('input[data-type=rq]', _).eq(0).click();
                }

            }

        }
    });



    //促销时间验证
    $('input[name=time_b],input[name=time_e]', _).blur(function () {

        var time_reg = /^(([0-1]{0,1}[0-9]|2[0-3])[:|：][0-5]){1}[0-9]{1}$/;
        if (!time_reg.test($(this).val())) {
            $.DHB.message({ 'content': '请输入格式为00:00-23:59之间的时间' });
            $(this).val('');
        };
    });


    //选择商品回调
    app.promote.dialogCallBackWork = function (jsonStr) {

        var data_item_last = '';
        var objList = jQuery.parseJSON(jsonStr);

        ////console.log(objList);
        $(objList).each(function (index, obj) {
            var guid = parseInt($(_ + "#shopspnum").val()) + 1;
            //var option = '';
            var resultHtml = '';
            var meiman;
            $('input[name=jsgz]').each(function (i, obj) {
                if ($(this).is(':checked')) {
                    meiman = $(this).attr('data-val');
                };
            });
            var guid = parseInt($(_ + "#shopspnum").val()) + 1;
            resultHtml += '<tr data-item="' + obj.id_sp + '">';
            resultHtml += ' <td class="align_center"><div name="xh">' + guid + '</div><input class="form-control user-input" placeholder="商品ID" name="shopsp_obj" type="hidden" value="' + obj.id_sp + '"  data-id_kcsp="' + obj.id_kcsp + '" ></td>';
            resultHtml += '<td class="align_left"><input type="text" class="w-120" name="bm" value="' + obj.bm + '"/></td>';
            resultHtml += '<td class="align_left"><span class="inline lineheight-30" name="mc">' + obj.mc + '</span><a class="btn btn-info f-r" onclick="app.promote.showshopsp()">选择商品</a></td>';
            if (typeof (obj.dw) != 'undefined' && obj.dw != 'undefined' && obj.dw != '') {
                resultHtml += '<td name="dw">    <div id="dw_select" name="dw_select"  class="common_select" style="display:block;"><div onclick="app.promote.dw_select_onclick(this)"><span class="inline">' + obj.dw + '</span> <i class="fa fa-caret-down"></i></div><ul class="common_select_list"><li value="' + limit_num(obj.dj_jh.toString(), app.promote.dpzkadd.digit_dj) + '"  data-zhl="' + limit_num(obj.zhl.toString(), app.promote.dpzkadd.digit_sl) + '"    data-id_shopsp="' + obj.id_sp + '"   data-id_kcsp="' + obj.id_kcsp + '"  data-dw="' + obj.dw + '"  >' + obj.dw + '</li></ul></div></td>';
            } else {
                resultHtml += '<td name="dw">    <div id="dw_select" name="dw_select"   class="common_select"><div onclick="app.promote.dw_select_onclick(this)"><span class="inline"></span> <i class="fa fa-caret-down"></i></div><ul value="" class="common_select_list"></ul></div></td>';
            }
            resultHtml += '<td class="align_center"><span data-group="span_jsgz">' + meiman + '</span><input type="text" value="" name="condition_1" onkeyup="check_digit(this,2)"/> 打折 <input type="text" value="" name="result_1"  onkeyup="check_digit(this,2)" onafterpaste="check_digit(this,2)"></td>';
            resultHtml += '<td class="align_center"><span data-group="span_jsgz">' + meiman + ' </span><input type="text" value="" name="condition_2" onkeyup="check_digit(this,2)"/> 打折 <input type="text" value="" name="result_2" onkeyup="check_digit(this,2)" onafterpaste="check_digit(this,2)"></td>';
            resultHtml += '<td class="align_center"><span data-group="span_jsgz">' + meiman + ' </span><input type="text" value="" name="condition_3"onkeyup="check_digit(this,2)" /> 打折 <input type="text" value="" name="result_3" onkeyup="check_digit(this,2)" onafterpaste="check_digit(this,2)"></td>';
            resultHtml += '<td class="align_center"><a onclick="app.promote.choice_spec_del(this);" class="bg-state bg-state-info blockbtn fa fa-trash" title="删除"></a></td>';
            resultHtml += ' </tr>';
            $(_ + "#shopspnum").val(guid);
            if (obj.id_sp == "" || obj.id_sp == undefined) {
                $(_ + "#shopsp_table>tbody").append(resultHtml);
            } else {
                $(_ + "#shopsp_table>tbody").prepend(resultHtml);
            }

            data_item_last = obj.id_sp;
        });

    }

    //下拉选项

    app.promote.dw_select = function (n_select) {
        app.httpAjax.post({
            data: { select_id_sp: n_select },
            headers: {},
            url: '/shopsp/GetShopspDwList',
            type: "POST",
            datatype: 'json',
            beforeSend: null,
            success: function (ret) {
                if (ret.Success) {
                    var aa = [];
                    $(ret.dw_list).each(function (i, e) {
                        aa.push(ret.dw_list.dw);
                    });
                    return aa;
                }
            },
            error: null,
            complete: null
        });
    }
    //添加商品至table
    app.promote.addshopsp = function (obj) {

        var shopsp = [];
        var shopsp_e = {};
        shopsp_e.id_shopsp = "";
        shopsp_e.id_sp = "";
        shopsp_e.id_kcsp = "";
        shopsp_e.bm = "";
        shopsp_e.mc = "";
        shopsp_e.dw = "";
        shopsp.push(shopsp_e);
        var jsonStr = JSON.stringify(shopsp);
        app.promote.dialogCallBackWork(jsonStr);
    }
    //增加行
    app.promote.addshopsp_row = function (obj) {
        app.promote.addshopsp();
        app.promote.reset_xh();
    };

    //删除商品的行
    app.promote.choice_spec_del = function (el) {
        $(el).parents('tr').remove();
        app.promote.setresult(false);
        app.promote.reset_xh();
    }


    //改动后重新计算和赋值
    app.promote.setresult = function (ifRemove) {
        var e = [];
        var shopsp = [];
        var totleMoney = 0;
        var xh = 1;
        if (ifRemove) {
            $('#shopsp_table>tbody', _).find('tr[data-item=""]').each(function () {
                $(this).remove();
            });
        }

        if (ifRemove) {
            app.promote.addshopsp();
            app.promote.addshopsp();
            app.promote.addshopsp();
            app.promote.addshopsp();
            app.promote.addshopsp();
            app.promote.reset_xh();
        }
    }

    //重置table序号
    app.promote.reset_xh = function (e) {
        var xh = 1;
        $('#shopsp_table>tbody', _).find('tr').each(function () {
            $(this).find('div[name="xh"]').text(xh);
            xh++;
        });
    }

    //按金额时不能选每
    if ($('input[value=je]', _).is(":checked")) {
        $('input[value=mei]+i', _).addClass('disabled');
        $('input[value=mei]', _).removeAttr('checked').attr('disabled', 'disabled');
        $('input[value=man]', _).click();
    }
    $('input[value=je]', _).click(function () {
        if ($(this).is(':checked')) {
            $('input[value=mei]+i', _).addClass('disabled');
            $('input[value=mei]', _).removeAttr('checked').attr('disabled', 'disabled');
            $('input[value=man]', _).click();
        }
    });
    $('input[value=sl]', _).click(function () {
        if ($(this).is(':checked')) {
            $('input[value=mei]+i', _).removeClass('disabled');
            $('input[value=mei]', _).removeAttr('disabled', 'disabled');
        }
    });





    //点击单位下拉事件
    app.promote.dw_select_onclick = function (e) {
        //
        var $select = $(e);
        var tr = $(e).parents("tr");
        var data_item = tr.attr('data-item');
        if (typeof (data_item) != 'undefined' && data_item != 'undefined' && data_item != '') {
            var dw_select = tr.find('td[name="dw"] div[name="dw_select"] ul');
            var data_item_select = dw_select.attr('data-item');
            if (typeof (data_item_select) == 'undefined' || data_item_select == 'undefined' || data_item_select == '' || data_item_select == '0') {
                //Post读取数据赋值
                app.httpAjax.post({
                    data: { select_id_sp: data_item, ps_id_shop: app.promote.dpzkadd.id_shop },
                    headers: {},
                    url: '/shopsp/GetShopspDwList',
                    type: "POST",
                    datatype: 'json',
                    beforeSend: null,
                    success: function (ret) {
                        if (ret.Success) {
                            var str_li = "";
                            if (ret.Data.length > 0) {
                                for (var item in ret.Data) {
                                    if (ret.Data[item].id != data_item) {
                                        var zhl = ret.Data[item].zhl.toString();
                                        zhl = limit_num(zhl, app.promote.dpzkadd.digit_sl);
                                        var dj_jh = ret.Data[item].dj_jh.toString();
                                        dj_jh = limit_num(dj_jh, app.promote.dpzkadd.digit_dj);
                                        str_li += '<li  value=' + dj_jh + '  data-zhl=' + zhl + ' data-id_shopsp=' + ret.Data[item].id + ' data-id_kcsp=' + ret.Data[item].id_kcsp + '   > ' + ret.Data[item].dw + '</li> ';
                                    }
                                }
                                dw_select.append(str_li);
                                dw_select.attr('data-item', '1');
                                $select.siblings('ul').show();
                            }
                        }
                    },
                    error: null,
                    complete: null
                });
            } else {
                $select.siblings('ul').show();
            }

        } else {
            $select.siblings('ul').show();
        }
    };
    $('.main-content', _).on('mouseleave', '.common_select', function () {
        $('.common_select_list', _).hide();
    });
    $('.main-content', _).on('click', '.common_select_list li', function () {
        var li_txt = $(this).html();
        $(this).parents('.common_select_list').hide();
        $(this).parents('.common_select').find('span').html(li_txt);
    });

    //检测相同商品
    app.promote.dpzkadd.delrepeat = function (a, arr) {
        var b = [], flag = false;
        //
        if (a == undefined) {
            b.push(flag);
            return b;
        } else {
            for (var i = 0; i < arr.length; i++) {

                if (a == arr[i].id_object) {
                    flag = true;
                    b.push(i + 1);

                }
            }
            b.push(flag);
            return b;
        }

    }
    //商品数据
    app.promote.dpzkadd.table_data = function () {
        var tableobj = []; var alldata = [], flag = true;
        $('#shopsp_table>tbody>tr', _).each(function (i, obj) {
            if ($(this).find('input[name=bm]').val() != '') {
                var sp = {}; var rezj = [];
                rezj = app.promote.dpzkadd.delrepeat($(this).find('input[name=shopsp_obj]').val(), tableobj);
                if (rezj[rezj.length - 1]) {
                    $.DHB.message({ 'content': '第' + $(this).find('div[name=xh]').html() + '行和第' + rezj[0] + '行重复，请删除其中一项！' });
                    flag = false;
                    return false;
                } else {
                    if ($(this).find('input[name=shopsp_obj]').val() != undefined) {
                        sp.id_object = $(this).find('input[name=shopsp_obj]').val();
                        sp.bm = $(this).find('input[name=bm]').val();
                        sp.mc = $(this).find('span[name=mc]').html();
                        sp.dw = $(this).find('div[name="dw_select"]>div>span').html();
                        sp.condition_1 = $(this).find('input[name=condition_1]').val();
                        sp.condition_2 = $(this).find('input[name=condition_2]').val();
                        sp.condition_3 = $(this).find('input[name=condition_3]').val();
                        sp.result_1 = $(this).find('input[name=result_1]').val();
                        sp.result_2 = $(this).find('input[name=result_2]').val();
                        sp.result_3 = $(this).find('input[name=result_3]').val();
                        tableobj.push(sp);
                    }
                }

            }
        });
        alldata.push(tableobj);
        alldata.push(flag);
        return alldata;
    }


    //表单提交
    $('#update_promote_form', _).validate({
        submitHandler: function (form) {
            var tableobj = [];
            var tableobj = app.promote.dpzkadd.table_data();
            if (tableobj[1]) {
                var smtData = $('#update_promote_form', _).serializeJson();
                smtData.sp = JSON.stringify(tableobj[0]);
                if (smtData.days) {
                    smtData.days = ',' + smtData.days.toString() + ',';
                }
                if (smtData.weeks) {
                    smtData.weeks = ',' + smtData.weeks.toString() + ',';
                }
                smtData.id = $("#promote1_id").val();
                smtData.bm_djlx = $("#promote1_bm_djlx").val();
                if (smtData.hylx == undefined) {
                    smtData.hylx = "all";
                }
                smtData.hylx = ',' + smtData.hylx.toString() + ',';
                smtData.id_shops = smtData.id_shops.toString();
                delete smtData.bm;
                delete smtData.condition_1;
                delete smtData.condition_2;
                delete smtData.condition_3;
                delete smtData.result_1;
                delete smtData.result_2;
                delete smtData.result_3;
                delete smtData.shopsp_obj;

                var url = $.DHB.U((app.promote.dpzkadd.option === app.promote.dpzkadd.option && app.promote.dpzkadd.option != "") ? 'promote/' + app.promote.dpzkadd.option : 'promote/dpzkadd');//$.DHB.U('promote/dpzkadd');
                if (smtData.flag_rqfw == '1') {
                    delete smtData.weeks;
                }
                if (smtData.flag_rqfw == '2') {
                    delete smtData.days;
                }
                smtData.id = $('#id').val();
                
                app.httpAjax.post({
                    data: smtData,
                    headers: {},
                    url: url,
                    type: "POST",
                    datatype: 'json',
                    beforeSend: null,
                    success: function (data) {
                        if (data.status == 'success') {
                            $.DHB.message({ 'content': '保存成功！' });
                            $.fn.menuTab.load({ url: '/promote/dpzkadd', 'title': '单品折扣促销', id: 'promote/dpzkadd', nocache: '1' });
                        } else if (data.status == 'error') {
                            $.DHB.message({ 'content': data.message });
                        }
                    },
                    error: null,
                    complete: null
                });
            }
        }
    });

    //当前日期初始化
    app.promote.cs_data = function () {
        var kk = [];
        var myDate = new Date();
        var now_year = myDate.getFullYear(), next_year;
        var now_month = myDate.getMonth() + 1, next_month;
        var now_day = myDate.getDate(), next_day;
        var now_hours = myDate.getHours();
        var now_minutes = myDate.getMinutes();
        var now_getseconds = myDate.getSeconds();
        var now_full_data = now_year + '-' + now_month + '-' + now_day;
        if (now_month == 12) {
            if (now_day == 1) {
                next_year = now_year + 1;
                next_month = 12;
                next_day = 31;
            } else {
                next_year = now_year + 1;
                next_month = 1;
                next_day = now_day - 1;
            }
        } else if (now_month == 1) {
            if (now_day == 1) {
                next_year = now_year;
                next_month = 1;
                next_day = 31;
            } else {
                next_year = now_year;
                next_month = now_month + 1;
                if (parseInt(now_year) % 400 == 0) {
                    if (now_day > 29) {
                        next_day = 29;
                    } else {
                        next_day = now_day - 1;
                    }
                } else {
                    if (now_day > 28) {
                        next_day = 28;
                    } else {
                        next_day = now_day - 1;
                    }
                }
            }
        } else {
            if (now_day == 1) {
                next_year = now_year;
                next_month = now_month;
                if (now_month == 2) {
                    if (parseInt(now_year) % 400 == 0) {
                        next_day = 29;
                    } else {
                        next_day = 28;
                    }
                } else if (now_month == 2 || now_month == 4 || now_month == 6 || now_month == 9 || now_month == 11) {
                    next_day = 30;
                } else {
                    next_day = 31;
                }
            } else {
                next_year = now_year;
                next_month = now_month + 1;
                next_day = now_day - 1;
            }
        }
        var next_full_data = next_year + '-' + next_month + '-' + next_day;
        kk.push(now_full_data);
        kk.push(next_full_data);
        return kk;
    }
    if (app.promote.dpzkadd.option == '' || app.promote.dpzkadd.option == undefined) {
        var data_sz = app.promote.cs_data();
        $('input[name=day_b]', _).val(data_sz[0]);
        $('input[name=day_e]', _).val(data_sz[1]);
    }


    //编码查询
    $('.cx_block', _).on('keydown', 'input[name=bm]', function (e) {
        if (e.keyCode == 13) {
            var bmstr = $(this).val();
            var mdid = app.promote.dpzkadd.id_shop;
            var $input = $(this);
            app.httpAjax.post({
                data: { keyword: bmstr },
                headers: {},
                url: '/shopsp/GetShopspList',
                type: "POST",
                datatype: 'json',
                beforeSend: null,
                success: function (data) {
                    if (data.Success) {
                        $input.parent('td').siblings('td').find('span[name=mc]').html(data.Data.mc);
                        $input.parents('tr').attr('data-item', data.Data.id_sp);
                        $input.parents('tr').find('input[name=shopsp_obj]').attr("data-id_kcsp", data.Data.id_kcsp);
                        $input.parents('tr').find('input[name=shopsp_obj]').val(data.Data.id_sp);
                        $input.parent('td').siblings('td').find('div[name=dw_select]>div>span').html(data.Data.dw);
                        $input.parent('td').siblings('td').find('div[name=dw_select]').css('display', 'block');
                        $input.parent('td').siblings('td').find('ul').append('<li>' + data.Data.dw + '</li>');
                    } else {
                        $.DHB.dialog({ 'title': '选择商品', 'url': $.DHB.U('shopsp/search?keyword=' + bmstr + '&id_shop=' + mdid), 'id': 'dialog-shopsp-search', 'confirm': app.promote.dialogCallBack });
                    }
                },
                error: null,
                complete: null
            });
        }
    });
    app.promote.cacal = function () {
        $('span[tabid=index]').click();
        $('span[tabid="promote/dpzkadd"]').show().click();
    }
    if (app.promote.dpzkadd.option == "detial") {
        $('input', _).attr('disabled', 'disabled');
        $('#update_promote_form button', _).attr('disabled', 'disabled');
        $(_ + 'a').not('.xzsp_a').attr('disabled', 'disabled');
        $('div[name=dw_select]>div,a:not(".xzsp_a")', _).removeAttr("onclick");
        $('.tab-table', _).off('click', '.fa-trash');
        //表格初始化
        app.promote.setresult(false);
    } else if (app.promote.dpzkadd.option == "edit") {
        app.promote.setresult(false);
    } else {
        //表格初始化
        app.promote.setresult(true);
    }
    if (app.promote.dpzkadd.option == '') {
        app.promote.dpzkadd.selectedShops = JSON.parse($('#mgrShops', _).val());
    } else {
        app.promote.dpzkadd.selectedShops = JSON.parse($('#selectedShops', _).val());
    }    
    app.promote.dpzkadd.mgrShops = JSON.parse($('#mgrShops', _).val());
    console.log(app.promote.dpzkadd.selectedShops);
    console.log(app.promote.dpzkadd.mgrShops);

}//ready结束括号

//判断数组Json对象中是否存在某个属性值

//构建门店树checkbox
app.get_shops = function (treeOptions) {
    var treeOptions = $.extend({
        obj: '',
        data: '',
        selectData: '',
        mrgData: '',
        changeSelectData: '',
        openId:0,
        jstreeeChange:null,
        jstreeReady: null ,
    }, treeOptions || {});
    treeOptions.obj
       .jstree("destroy")
       .jstree({
           'core': {
               'multiple': true,
               'data': treeOptions.data,

           },
           "plugins": ["themes", "json_data", "search", "checkbox"],
           "checkbox": {
               "keep_selected_style": false,//是否默认选中
               "three_state": true,//父子级别级联选择
           }
       })
       .on("changed.jstree", function (e, data) {
           if (data.selected && data.selected.length) {
               var i, j, r = [];
               if (data.selected.length == 1) {
                   treeOptions.changeSelectData = data.instance.get_node(data.selected[0]).id;
               } else {
                   for (i = 0, j = data.selected.length; i < j; i++) {

                       r.push(data.instance.get_node(data.selected[i]).id);
                   }
                   treeOptions.changeSelectData = JSON.stringify(r);
               }
           } else {
               treeOptions.changeSelectData = ''
           }
           funReturnData(treeOptions.changeSelectData);
       })
       .on('ready.jstree', function (e, obj) {
           console.log(treeOptions.openId);
           obj.instance.open_node({ "id": treeOptions.openId });
           //obj.instance.select_all();
       })
    var funReturnData=function(selectData){
        return selectData;
    }
    $('#' + treeOptions.openId, _).attr('data-jstree', '{ "disabled" : true }');
}


var options = {
    url: $.DHB.U('/SearchCondition/GetTreeShop'),
    success: function (data) {
        if (data.Success == true) {
            if (data.Data.length > 0) {
                var newDataArr = [], openIdDate;
                for (var i = 0; i < data.Data.length; i++) {
                    var newData = {},flag;
                    newData = data.Data[i];
                    if (data.Data[i].flag_type == 1) {
                        openIdDate= data.Data[i].id;                        
                    }
                    
                    newData.state = {};
                    newData.state.disabled = false;
                    newDataArr.push(newData);
                }
                if (newDataArr.length == data.Data.length) {
                    
                    app.get_shops({
                        obj: $('#tree_idshop', _),
                        data: data.Data,
                        openId: openIdDate,
                    });
                }
            }
            console.log(data);
        }
    },
    error: function () {
        console.log("i");
    }
};
app.httpAjax.post(options);
