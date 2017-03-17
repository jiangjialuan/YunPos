
//
if (app.report.spcrkhzreport) {
    delete app.report.spcrkhzreport.tb;
}
$(function () {
    $('div[contentID="report/spcrkhzreport"]').attr({ controller: 'report', action: 'spcrkhzreport' });
    app.c.public_data['report/spcrkhzreport'] = app.c.public_data['report/spcrkhzreport'] || {};
    app.c.public_data['report/spcrkhzreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.spcrkhzreport.vue = new Vue({
        el: "#spcrkhzreport",
        data: {
            selected: app.report.spcrkhzreport.report_idshop,
            getshop: [],
            //getuser: []

        },       
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.spcrkhzreport.vue.getshop = data;
                }, function () {});
            },
            
            
        }
    });
    //门店名称初始化
    app.report.spcrkhzreport.vue.getshopfunc();
})



app.report.spcrkhzreport_ready = function () {

    //日期插件初始化
    $('#spcrkhzreport #rq_begin,#spcrkhzreport #rq_end', _).click(function () {
        app.report.spcrkhzreport.wdatepicker_report_spcrkhzreport(this);
    });
    app.report.spcrkhzreport.wdatepicker_report_spcrkhzreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.spcrkhzreport.first_Day = function () {
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
    app.report.spcrkhzreport.last_Day = function () {
        
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
   
    $('#spcrkhzreport #rq_end', _).val(app.report.spcrkhzreport.last_Day());
    $('#spcrkhzreport #rq_begin', _).val(app.report.spcrkhzreport.first_Day());
    

    

    //替换非数字日期
    app.report.spcrkhzreport.renumdou = function (str) {
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
    
    app.report.spcrkhzreport.sl_rk = 0;
    app.report.spcrkhzreport.sl_ck = 0;
    app.report.spcrkhzreport.sl_qm = 0;
    app.report.spcrkhzreport.table_draw = function () {
        
        
        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.spcrkhzreport.tb = $('#report-spcrkhzreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/spcrkhzreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form',_).serializeJson();
                    //dt = app.report.spcrkhzreport.vue.amDate;
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
                
                { "data": "mc_shop", "sClass": "width200" },
                { "data": "mc_spfl", "sClass": "w-89" },
                { "data": "mc_sp", "sClass": "width200" },
                { "data": "barcode", "sClass": "text-center w-100" },
                { "data": "dw", "sClass": "text-center w-70" },
                {
                    "data": "sl_rk", "sClass": "text-right w-100", "render": function (data, type, row) {
                        ////console.log(row);
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spcrkhzreport.sl_rk += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spcrkhzreport.digit);
                        }
                    }
                },
                
                {
                    "data": "sl_ck", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spcrkhzreport.sl_ck += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spcrkhzreport.digit);
                        }
                    }
                },
                {
                    "data": "sl_qm", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spcrkhzreport.sl_qm += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spcrkhzreport.digit);
                        }
                    }
                }
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                app.report.spcrkhzreport.rowCallback();
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
        
        //app.report.spcrkhzreport.fncallback();
        
    }
    //app.report.spcrkhzreport.table_draw();
    app.report.spcrkhzreport.rowCallback = function () {
        $('#spcrkhzreport .sl_rk', _).html(app.report.spcrkhzreport.sl_rk.toFixed(app.report.spcrkhzreport.digit));
        $('#spcrkhzreport .sl_ck', _).html(app.report.spcrkhzreport.sl_ck.toFixed(app.report.spcrkhzreport.digit));
        $('#spcrkhzreport .sl_qm', _).html(app.report.spcrkhzreport.sl_qm.toFixed(app.report.spcrkhzreport.digit));
    }
    //datatables设置
    
    $('#spcrkhzreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.spcrkhzreport.sl_rk = 0;
        app.report.spcrkhzreport.sl_ck = 0;
        app.report.spcrkhzreport.sl_qm = 0;
    });
    $("#spcrkhzreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
            
            if (json.outstr == 0) {
                //alert("没有数据");
                //json.data = {};
                //json.draw = draw;
                
                json.data = {};
                json.recordsTotal = 0;
                json.recordsFiltered = 0;
                app.report.spcrkhzreport.rowCallback();
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
        $('#spcrkhzreport #btn-search', _).click(function () {
            $(this).attr("disabled", "disabled");
            _button = $(this);
            setTimeout(function () {
                _button.removeAttr("disabled");
            }, 500);
            if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {
                app.report.spcrkhzreport.table_search();
            } else {
                $.DHB.message({ "content": "开始时间不能大于结束时间" });
            }
            
        });
        app.report.spcrkhzreport.table_search = function () {
            
            if (app.report.spcrkhzreport.tb) {
                app.report.spcrkhzreport.tb.ajax.reload();
                return false;
            } else {
                app.report.spcrkhzreport.table_draw();
          }
           
            
        }
    
}

