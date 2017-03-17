
//
if (app.report.kckspdmxreport) {
    delete app.report.kckspdmxreport.tb;
}
$(function () {
    $('div[contentID="report/kckspdmxreport"]').attr({ controller: 'report', action: 'kckspdmxreport' });
    app.c.public_data['report/kckspdmxreport'] = app.c.public_data['report/kckspdmxreport'] || {};
    app.c.public_data['report/kckspdmxreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.kckspdmxreport.vue = new Vue({
        el: "#kckspdmxreport",
        data: {
            selected: app.report.kckspdmxreport.report_idshop,
            getshop: [],

        },       
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.kckspdmxreport.vue.getshop = data;
                }, function () {});
            },
            
            
        }
    });
    //门店名称初始化
    app.report.kckspdmxreport.vue.getshopfunc();
    
})



app.report.kckspdmxreport_ready = function () {

    //日期插件初始化
    $('#kckspdmxreport #rq_begin,#kckspdmxreport #rq_end', _).click(function () {
        app.report.kckspdmxreport.wdatepicker_report_kckspdmxreport(this);
    });
    app.report.kckspdmxreport.wdatepicker_report_kckspdmxreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.kckspdmxreport.first_Day = function () {
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
    app.report.kckspdmxreport.last_Day = function () {
        
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
    
    $('#kckspdmxreport #rq_end', _).val(app.report.kckspdmxreport.last_Day());
    $('#kckspdmxreport #rq_begin', _).val(app.report.kckspdmxreport.first_Day());
  

    //替换非数字日期
    app.report.kckspdmxreport.renumdou = function (str) {
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
    app.report.kckspdmxreport.sl_kc = 0;
    app.report.kckspdmxreport.sl_pd = 0; 
    app.report.kckspdmxreport.sl_yk = 0;
    app.report.kckspdmxreport.je_yk = 0;
    app.report.kckspdmxreport.table_draw = function () {
        
        
        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.kckspdmxreport.tb = $('#report-kckspdmxreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/kckspdmxreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form',_).serializeJson();
                    //dt = app.report.kckspdmxreport.vue.amDate;
                    dt.barcode = app.str_del_bank(dt.barcode);
                    dt.mc_sp = app.str_del_bank(dt.mc_sp);
                    dt.dh = app.str_del_bank(dt.dh);
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
                    "data": "rq", "sClass": "text-center w-100", "render": function (data, type, row) {
                        var dd = parseInt(app.report.kckspdmxreport.renumdou(data));
                        return (new Date(parseInt(dd)).Format("yyyy-MM-dd"));;
                    }
                },
                
                { "data": "mc_shop", "sClass": "text-center width200" },
                //{ "data": "dh" },
                { "data": "dh", "sClass": "text-center width200" },
                { "data": "mc_sp", "sClass": "width200" },
                { "data": "dw", "sClass": "text-center w-70" },
                
                {
                    "data": "sl_kc", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.kckspdmxreport.sl_kc += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.kckspdmxreport.digit);
                        }
                    }
                },
                {
                    "data": "sl_pd", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.kckspdmxreport.sl_pd += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.kckspdmxreport.digit);
                        }
                    }
                },
                {
                    "data": "sl_yk", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.kckspdmxreport.sl_yk += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.kckspdmxreport.digit);
                        }
                    }
                },
                {
                    "data": "dj_jh", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            return parseFloat(data).toFixed(app.report.kckspdmxreport.digit);
                        }
                    }
                },
                
                {
                    "data": "je_yk", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.kckspdmxreport.je_yk += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.kckspdmxreport.digit);
                        }
                    }
                },
                { "data": "barcode", "sClass": "text-center w-100" },
                { "data": "mc_spfl", "sClass": "text-center w-100" }
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                app.report.kckspdmxreport.rowCallback();
                
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
        
        //app.report.kckspdmxreport.fncallback();
        
    }
    //app.report.kckspdmxreport.table_draw();
    app.report.kckspdmxreport.rowCallback = function () {
        $('#kckspdmxreport .sl_kc', _).html(app.report.kckspdmxreport.sl_kc.toFixed(app.report.kckspdmxreport.digit));
        $('#kckspdmxreport .sl_pd', _).html(app.report.kckspdmxreport.sl_pd.toFixed(app.report.kckspdmxreport.digit));
        $('#kckspdmxreport .sl_yk', _).html(app.report.kckspdmxreport.sl_yk.toFixed(app.report.kckspdmxreport.digit));
        $('#kckspdmxreport .je_yk', _).html(app.report.kckspdmxreport.je_yk.toFixed(app.report.kckspdmxreport.digit));
    }
    //datatables设置
    
    $('#kckspdmxreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.kckspdmxreport.sl_kc = 0;
        app.report.kckspdmxreport.sl_pd = 0;
        app.report.kckspdmxreport.sl_yk = 0; 
        app.report.kckspdmxreport.je_yk = 0;

    });
    $("#kckspdmxreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
            
            if (json.outstr == 0) {
                //alert("没有数据");
                //json.data = {};
                //json.draw = draw;
                
                json.data = {};
                json.recordsTotal = 0;
                json.recordsFiltered = 0;
                app.report.kckspdmxreport.rowCallback();
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
        $('#kckspdmxreport #btn-search', _).click(function () {
            $(this).attr("disabled", "disabled");
            _button = $(this);
            setTimeout(function () {
                _button.removeAttr("disabled");
            }, 500)
            if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {
                app.report.kckspdmxreport.table_search();
            } else {
                $.DHB.message({ "content": "开始时间不能大于结束时间" });
            }
            
        });
        app.report.kckspdmxreport.table_search = function () {
            
            if (app.report.kckspdmxreport.tb) {
                app.report.kckspdmxreport.tb.ajax.reload();
                return false;
            } else {
                app.report.kckspdmxreport.table_draw();
          }
           
            
        }
    
}

