
//
if (app.report.spxsmxreport) {
    delete app.report.spxsmxreport.tb;
}
$(function () {
    $('div[contentID="report/spxsmxreport"]').attr({ controller: 'report', action: 'spxsmxreport' });
    app.c.public_data['report/spxsmxreport'] = app.c.public_data['report/spxsmxreport'] || {};
    app.c.public_data['report/spxsmxreport']['once'] = false;
    app.report = app.report || {};
});

$(function () {
    app.report.spxsmxreport.vue = new Vue({
        el: "#spxsmxreport",
        data: {
            selected: app.report.spxsmxreport.report_idshop,
            getshop: [],
            //getuser: []

        },       
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.spxsmxreport.vue.getshop = data;
                }, function () {});
            },
            //getuserfunc: function () {
            //    app.request($.DHB.U('SearchCondition/GetUser'), {}, function (data) {
            //        app.report.spxsmxreport.vue.getuser = data;
            //    }, function () { });
            //},
            
        }
    });
    //门店名称初始化
    app.report.spxsmxreport.vue.getshopfunc();
    //收银员初始化
    //app.report.spxsmxreport.vue.getuserfunc()
})



app.report.spxsmxreport_ready = function () {

    //日期插件初始化
    $('#spxsmxreport #rq_begin,#spxsmxreport #rq_end', _).click(function () {
        app.report.spxsmxreport.wdatepicker_report_spxsmxreport(this);
    });
    app.report.spxsmxreport.wdatepicker_report_spxsmxreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.spxsmxreport.first_Day = function () {
        var nowdate = new Date();
        var y = nowdate.getFullYear();
        var m = nowdate.getMonth() + 1;
        var d = nowdate.getDate();
        if (parseInt(m) < 10) {
            m = "0" + m;
        }
        var formatnowdate = y + '-' + m + '-' +'01';
        return formatnowdate;
    }
    app.report.spxsmxreport.last_Day = function () {
        
        var date = new Date();
        var currentMonth = date.getMonth();
        var nextMonth = ++currentMonth;
        var nextMonthFirstDay = new Date(date.getFullYear(), nextMonth, 1);
        var oneDay = 1000 * 60 * 60 * 24;
        var nowdate = new Date(nextMonthFirstDay - oneDay);
        var y = nowdate.getFullYear();
        var m = nowdate.getMonth() + 1;
        var d = nowdate.getDate();
        if (parseInt(m) < 10) {
            m = "0" + m;
        }
        if (parseInt(d) < 10) {
            d = "0" + d;
        }
        var formatnowdate = y + '-' + m + '-' + d;
        return formatnowdate;
    }
    app.report.spxsmxreport.last_Day()
    $('#spxsmxreport #rq_end', _).val(app.report.spxsmxreport.last_Day());
    $('#spxsmxreport #rq_begin', _).val(app.report.spxsmxreport.first_Day());
    //input显示商品分类，选择商品删除按钮
    $('#spxsmxreport #shopfl_box,#spxsmxreport #id_sp_box', _).hover(function () {
        $(this).parent().find('span:last()').show();
    }, function () {
        $(this).parent().find('span:last()').hide();
    });
    $('#spxsmxreport .clear_button', _).click(function () {
        app.report.spxsmxreport.report_spxsmx(this);
    });

    //商品分类弹框
    //$('#spxsmxreport #shopfl', _).click(function () {
    //    app.report.spxsmxreport.spfl(this)
    //});
    //app.report.spxsmxreport.spfl = function (el) {
    //    $.DHB.dialog({ 'title': '商品分类', 'url': $.DHB.U('promote/shopfl'), 'id': 'report-shopfl', 'confirm': app.report.spxsmxreport.dialogCallBack_shopfl });
    //}
    //app.report.spxsmxreport.dialogCallBack_shopfl = function () {
    //    var fldata = {}, str = "", str_id = "";
    //    fldata = JSON.parse($('#fldata', _).val());
    //    for (var i = 0; i < fldata.length; i++) {
    //        if (i == fldata.length - 1) {
    //            str += fldata[i].text;
    //            str_id += fldata[i].id;
    //        } else {
    //            str += fldata[i].text + ",";
    //            str_id += fldata[i].id + ",";
    //        }

    //    }
    //    $('#shopfl').val(str);
    //    //$('#fldata').attr('flstr', str_id);
    //    $('#fldata').val(str_id);
    //    $.DHB.dialog({ 'id': 'report-shopfl', 'action': 'destroy' });
    //}

    //选择商品
    //$('#spxsmxreport #s_id_shopsp_mc', _).click(function () {
    //    app.report.spxsmxreport.get_select_xsmx()
    //});
    //app.report.spxsmxreport.get_select_xsmx = function () {
    //    //
    //    var id_shop = "";
    //    $.DHB.dialog({ 'title': '选择商品', 'url': $.DHB.U('shopsp/search?searchType=report&count=1&id_shop=' + id_shop), 'id': 'dialog-shopsp-search', 'confirm': app.report.spxsmxreport.dialogCallBack_xsmx });
    //}
    //app.report.spxsmxreport.dialogCallBack_xsmx = function (array) {
    //    var jsonStr = "";
    //    $.each(array, function (index, item) {
    //        if (item.name == "shopsp_table_json") {
    //            jsonStr = item.value;
    //        }
    //    });
    //    var objList = jQuery.parseJSON(jsonStr);
    //    var str_name = "", str_id = "";
    //    for (var i = 0; i < objList.length; i++) {
    //        if (i == objList.length - 1) {
    //            str_name += objList[i].mc;
    //            str_id += objList[i].id_shopsp;
    //        } else {
    //            str_name += objList[i].mc + ",";
    //            str_id += objList[i].id_shopsp + ",";
    //        }

    //    }
    //    //
    //    $(_ + "#spxsmxreport  #s_id_shopsp_mc").val(str_name);
    //    $(_ + "#spxsmxreport  #id_sp").val(str_id);

    //    $.DHB.dialog({ 'id': 'dialog-shopsp-search', 'action': 'destroy' });
    //    //app.search.do_search_report_spxsmxreport();
    //}
    //app.report.spxsmxreport.report_spxsmx = function (el) {
    //    var par = $(el).parent();
    //    par.find('input[type=text]').val('');

    //}
    

    //替换非数字日期
    app.report.spxsmxreport.renumdou = function (str) {
        if (str == "" || str == undefined) {
            var newstr = "";
            
        } else {
            var regexp = /[^\d]]*/g;
            newstr = str.replace(regexp, "");
            
        }
        return newstr;
        
    }
    
    //
    
    //表格初始化
    app.report.spxsmxreport.sl_xs = 0;
    app.report.spxsmxreport.je = 0;
    app.report.spxsmxreport.je_yh = 0;
    app.report.spxsmxreport.je_xs = 0;
    app.report.spxsmxreport.je_cb = 0;
    app.report.spxsmxreport.je_ml = 0;
    app.report.spxsmxreport.sl_cx = 0;
    app.report.spxsmxreport.table_draw = function () {
        
        
        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.spxsmxreport.tb = $('#report-spxsmxreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/spxsmxreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form',_).serializeJson();
                    //dt = app.report.spxsmxreport.vue.amDate;
                    dt.dh = app.str_del_bank(dt.dh);
                    dt.barcode = app.str_del_bank(dt.barcode);
                    dt.mc_sp = app.str_del_bank(dt.mc_sp);
                    //console.log(dt);
                    dt.draw = d.draw;
                    dt.page = d.start / d.length;
                    dt.pageSize = d.length;
                    //console.log(dt);
                    return dt;
                }
                //success: function (data) {
                //    //console.log(data);
                //}
            },
            "language": {
                "lengthMenu": "每页_MENU_条",
                "info": "共 _PAGES_页",
                'paginate': {  
                      
                    'next':       '>',  
                    'previous':   '<'  
                },
            },
            serverSide: true,
            processing: false,
            lengthMenu: [25,10, 20,30,50,80,100],
            ////stateSave: true,    //该参数会将分页状态保存, 影响到每页行数
            pageLength: 25,
            autoWidth:false,
            columns: [
                { "data": null, "sClass": "text-center w-40" },
                {
                    "data": "rq_xs", "sClass": "text-center w-100", "render": function (data, type, row) {
                        var dd = parseInt(app.report.spxsmxreport.renumdou(data));
                        return (new Date(parseInt(dd)).Format("yyyy-MM-dd"));;
                    }
                },
                {
                    "data": "rq_wd", "sClass": "text-center w-70", "render": function (data, type, row) {
                        
                        switch (data) {
                            case 1: return "星期日"; break;
                            case 2: return "星期一"; break;
                            case 3: return "星期二"; break;
                            case 4: return "星期三"; break;
                            case 5: return "星期四"; break;
                            case 6: return "星期五"; break;
                            case 7: return "星期六"; break;
                        }
                    }
                },
                { "data": "mc_shop", "sClass": "width200", },
                { "data": "dh", "sClass": "text-center width200" },
                { "data": "mc_sp", "sClass": "width200" },
                { "data": "dw", "sClass": "text-center width55" },
                {
                    "data": "sl_xs", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spxsmxreport.sl_xs += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spxsmxreport.digit);
                        }
                    }
                },
                {
                    "data": "dj", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            return parseFloat(data).toFixed(app.report.spxsmxreport.digit);
                        }
                    }
                },
                {
                    "data": "je", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spxsmxreport.je += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spxsmxreport.digit);
                        }
                    }
                },
                {
                    "data": "zk", "sClass": "text-right width55", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            return parseFloat(data).toFixed(app.report.spxsmxreport.digit);
                        }
                    }
                },
                {
                    "data": "je_yh", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spxsmxreport.je_yh += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spxsmxreport.digit);
                        }
                    }
                },
                {
                    "data": "je_xs", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spxsmxreport.je_xs += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spxsmxreport.digit);
                        }
                    }
                },
                
                { "data": "mc_hy", "sClass": "text-center w-70" },
                { "data": "phone", "sClass": "text-center w-89" },
                {
                    "data": "jf", "sClass": "text-right w-70", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            return parseFloat(data).toFixed(app.report.spxsmxreport.digit);
                        }
                    }
                },
                {
                    "data": "sl_cx", "sClass": "text-right", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spxsmxreport.sl_cx += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spxsmxreport.digit);
                        }
                    }
                },
                { "data": "barcode", "sClass": "text-center w-100" },
                { "data": "mc_spfl", "sClass": "text-center w-100" }
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                app.report.spxsmxreport.rowCallback();
            },
            "fnDrawCallback":function(){
                var api = this.api();
                var startIndex = api.context[0]._iDisplayStart;//获取到本页开始的条数
                api.column(0).nodes().each(function (cell, i) {
                    cell.innerHTML = startIndex + i + 1;
                });
            },
            "dom": 'rt<"pagecss"ilp><"clear">',
            

        });
        
        //app.report.spxsmxreport.fncallback();
        
    }
    //app.report.spxsmxreport.table_draw();
    app.report.spxsmxreport.rowCallback = function () {
        $('#spxsmxreport .sl_xs', _).html(app.report.spxsmxreport.sl_xs.toFixed(app.report.spxsmxreport.digit));
        $('#spxsmxreport .je', _).html(app.report.spxsmxreport.je.toFixed(app.report.spxsmxreport.digit));
        $('#spxsmxreport .je_yh', _).html(app.report.spxsmxreport.je_yh.toFixed(app.report.spxsmxreport.digit));
        $('#spxsmxreport .je_xs', _).html(app.report.spxsmxreport.je_xs.toFixed(app.report.spxsmxreport.digit));
        //$('#spxsmxreport .je_cb', _).html(app.report.spxsmxreport.je_cb.toFixed(app.report.spxsmxreport.digit));
        //$('#spxsmxreport .je_ml', _).html(app.report.spxsmxreport.je_ml.toFixed(app.report.spxsmxreport.digit));
        $('#spxsmxreport .sl_cs', _).html(app.report.spxsmxreport.sl_cx.toFixed(app.report.spxsmxreport.digit));
    }
    //datatables设置
    
    $('#spxsmxreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.spxsmxreport.sl_xs = 0;
        app.report.spxsmxreport.je = 0;
        app.report.spxsmxreport.je_yh = 0;
        app.report.spxsmxreport.je_xs = 0;
        //app.report.spxsmxreport.je_cb = 0;
        //app.report.spxsmxreport.je_ml = 0;
        app.report.spxsmxreport.sl_cx = 0;
    });
    $("#spxsmxreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
            
            if (json.outstr == 0) {
                //alert("没有数据");
                //json.data = {};
                //json.draw = draw;
                
                json.data = {};
                json.recordsTotal = 0;
                json.recordsFiltered = 0;
                app.report.spxsmxreport.rowCallback();
                
                return false;
            } else {
                try {

                    json.data = json.rList;
                    json.recordsTotal = json.outstr;
                    json.recordsFiltered = json.outstr;

                    //}

                }
                catch (e) {
                    json.data = {};
                    //json.draw = draw;
                    json.recordsTotal = 0;
                    json.recordsFiltered = 0;
                    ////console.log(e.toString());
                }
            }
            
        });

        try {
            //$.fn.dataTable.ext.errMode = "none";   //屏蔽dataTable的错误提示
        }
        catch (e) {
            ////console.log(e.message);
        }

        try {
            Dropzone.autoDiscover = false;  //屏蔽Dropzone的自动初始化功能
        }
        catch (e) {
            ////console.log(e.message);
        }
        

    //表格查询
        $('#spxsmxreport #btn-search', _).click(function () {
            $(this).attr("disabled", "disabled");
            _button = $(this);
            setTimeout(function () {
                _button.removeAttr("disabled");
            }, 500);
            if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {
                app.report.spxsmxreport.table_search();
            } else {
                $.DHB.message({ "content": "开始时间不能大于结束时间" });
            }
            
        });
        app.report.spxsmxreport.table_search = function () {
            
            if (app.report.spxsmxreport.tb) {
                app.report.spxsmxreport.tb.ajax.reload();
                return false;
            } else {
                app.report.spxsmxreport.table_draw();
          }
           
            
        }
    
}

