
//
if (app.report.spxshzreport) {
    delete app.report.spxshzreport.tb;
}
$(function () {
    $('div[contentID="report/spxshzreport"]').attr({ controller: 'report', action: 'spxshzreport' });
    app.c.public_data['report/spxshzreport'] = app.c.public_data['report/spxshzreport'] || {};
    app.c.public_data['report/spxshzreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.spxshzreport.vue = new Vue({
        el: "#spxshzreport",
        data: {
            selected: app.report.spxshzreport.report_idshop,
            getshop: [],

        },       
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.spxshzreport.vue.getshop = data;
                }, function () {});
            },
            
            
        }
    });
    //门店名称初始化
    app.report.spxshzreport.vue.getshopfunc();
    
})



app.report.spxshzreport_ready = function () {

    //日期插件初始化
    $('#spxshzreport #rq_begin,#spxshzreport #rq_end', _).click(function () {
        app.report.spxshzreport.wdatepicker_report_spxshzreport(this);
    });
    app.report.spxshzreport.wdatepicker_report_spxshzreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.spxshzreport.first_Day = function () {
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
    app.report.spxshzreport.last_Day = function () {
        
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
    app.report.spxshzreport.last_Day()
    $('#spxshzreport #rq_end', _).val(app.report.spxshzreport.last_Day());
    $('#spxshzreport #rq_begin', _).val(app.report.spxshzreport.first_Day());
    //input显示商品分类，选择商品删除按钮
    $('#spxshzreport #shopfl_box,#spxshzreport #id_sp_box', _).hover(function () {
        $(this).parent().find('span:last()').show();
    }, function () {
        $(this).parent().find('span:last()').hide();
    });
    $('#spxshzreport .clear_button', _).click(function () {
        app.report.spxshzreport.report_spxsmx(this);
    });

    //商品分类弹框
    $('#spxshzreport #shopfl', _).click(function () {
        app.report.spxshzreport.spfl(this)
    });
    app.report.spxshzreport.spfl = function (el) {
        $.DHB.dialog({ 'title': '商品分类', 'url': $.DHB.U('promote/shopfl'), 'id': 'report-shopfl', 'confirm': app.report.spxshzreport.dialogCallBack_shopfl });
    }
    app.report.spxshzreport.dialogCallBack_shopfl = function () {
        var fldata = {}, str = "", str_id = "";
        fldata = JSON.parse($('#fldata', _).val());
        for (var i = 0; i < fldata.length; i++) {
            if (i == fldata.length - 1) {
                str += fldata[i].text;
                str_id += fldata[i].id;
            } else {
                str += fldata[i].text + ",";
                str_id += fldata[i].id + ",";
            }

        }
        $('#shopfl').val(str);
        $('#fldata').val(str_id);
        $.DHB.dialog({ 'id': 'report-shopfl', 'action': 'destroy' });
    }
    app.report.spxshzreport.report_spxsmx = function (el) {
        var par = $(el).parent();
        par.find('input[type=text]').val('');
        par.find('input[type=hidden]').val('');

    }
    
    

    //替换非数字日期
    app.report.spxshzreport.renumdou = function (str) {
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
    app.report.spxshzreport.sl_xs = 0;
    app.report.spxshzreport.je_yxs = 0;
    app.report.spxshzreport.je_zk = 0;
    app.report.spxshzreport.je_xs = 0;
    app.report.spxshzreport.je_cb = 0;
    app.report.spxshzreport.je_ml = 0;
    app.report.spxshzreport.table_draw = function () {
        
        
        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.spxshzreport.tb = $('#report-spxshzreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/spxshzreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form',_).serializeJson();
                    //dt = app.report.spxshzreport.vue.amDate;
                    dt.barcode = app.str_del_bank(dt.barcode);
                    dt.mc_sp = app.str_del_bank(dt.mc_sp);
                    //console.log(dt);
                    dt.draw = d.draw;
                    dt.page = d.start / d.length;
                    dt.pageSize = d.length;
                    //console.log(dt);
                    return dt;
                }
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
                        ////console.log(row)
                        var dd = parseInt(app.report.spxshzreport.renumdou(data));
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
                { "data": "mc_shop", "sClass": "width200" },
                
                { "data": "mc_sp", "sClass": "width200" },
                { "data": "dw", "sClass": "text-center width55" },
                {
                    "data": "sl_xs", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spxshzreport.sl_xs += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spxshzreport.digit);
                        }
                    }
                },
                {
                    "data": "je_yxs", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spxshzreport.je_yxs += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spxshzreport.digit);
                        }
                    }
                },
                {
                    "data": "je_zk", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spxshzreport.je_zk += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spxshzreport.digit);
                        }
                    }
                },
                {
                    "data": "je_xs", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spxshzreport.je_xs += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spxshzreport.digit);
                        }
                    }
                },
                {
                    "data": "je_cb", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spxshzreport.je_cb += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spxshzreport.digit);
                        }
                    }
                },
                {
                    "data": "je_ml", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spxshzreport.je_ml += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spxshzreport.digit);
                        }
                    }
                },
                
                {
                    "data": "mll", "sClass": "text-right width55", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            return parseFloat(data*100).toFixed(app.report.spxshzreport.digit)+'%';
                        }
                    }
                },
                { "data": "barcode", "sClass": "text-center w-100" },
                { "data": "mc_spfl", "sClass": "w-100" }
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                app.report.spxshzreport.rowCallback();
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
        
        //app.report.spxshzreport.fncallback();
        
    }
    //app.report.spxshzreport.table_draw();
    app.report.spxshzreport.rowCallback = function () {
        $('#spxshzreport .sl_xs', _).html(app.report.spxshzreport.sl_xs.toFixed(app.report.spxshzreport.digit));
        $('#spxshzreport .je_yxs', _).html(app.report.spxshzreport.je_yxs.toFixed(app.report.spxshzreport.digit));
        $('#spxshzreport .je_zk', _).html(app.report.spxshzreport.je_zk.toFixed(app.report.spxshzreport.digit));
        $('#spxshzreport .je_xs', _).html(app.report.spxshzreport.je_xs.toFixed(app.report.spxshzreport.digit));
        $('#spxshzreport .je_cb', _).html(app.report.spxshzreport.je_cb.toFixed(app.report.spxshzreport.digit));
        $('#spxshzreport .je_ml', _).html(app.report.spxshzreport.je_ml.toFixed(app.report.spxshzreport.digit));
    }
    //datatables设置
    
    $('#spxshzreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.spxshzreport.sl_xs = 0;
        app.report.spxshzreport.je_yxs = 0;
        app.report.spxshzreport.je_zk = 0;
        app.report.spxshzreport.je_xs = 0;
        app.report.spxshzreport.je_cb = 0;
        app.report.spxshzreport.je_ml = 0;
    });
    $("#spxshzreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
            
            if (json.outstr == 0) {
                //alert("没有数据");
                //json.data = {};
                //json.draw = draw;
                
                json.data = {};
                json.recordsTotal = 0;
                json.recordsFiltered = 0;
                app.report.spxshzreport.rowCallback();
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
        $('#spxshzreport #btn-search', _).click(function () {
            $(this).attr("disabled", "disabled");
            _button = $(this);
            setTimeout(function () {
                _button.removeAttr("disabled");
            }, 500);
            if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {
                app.report.spxshzreport.table_search();
            } else {
                $.DHB.message({ "content": "开始时间不能大于结束时间" });
            }
            
        });
        app.report.spxshzreport.table_search = function () {
            
            if (app.report.spxshzreport.tb) {
                app.report.spxshzreport.tb.ajax.reload();
                return false;
            } else {
                app.report.spxshzreport.table_draw();
          }
           
            
        }
    
}

