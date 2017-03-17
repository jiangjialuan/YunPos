
//
if (app.report.spkcreport) {
    delete app.report.spkcreport.tb;
}
$(function () {
    $('div[contentID="report/spkcreport"]').attr({ controller: 'report', action: 'spkcreport' });
    app.c.public_data['report/spkcreport'] = app.c.public_data['report/spkcreport'] || {};
    app.c.public_data['report/spkcreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.spkcreport.vue = new Vue({
        el: "#spkcreport",
        data: {
            selected: app.report.spkcreport.report_idshop,
            getshop: [],
            //getuser: []

        },       
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.spkcreport.vue.getshop = data;
                }, function () {});
            },
            
            
        }
    });
    //门店名称初始化
    app.report.spkcreport.vue.getshopfunc();
})



app.report.spkcreport_ready = function () {

    //日期插件初始化
    $('#spkcreport #rq_begin,#spkcreport #rq_end', _).click(function () {
        app.report.spkcreport.wdatepicker_report_spkcreport(this);
    });
    app.report.spkcreport.wdatepicker_report_spkcreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.spkcreport.first_Day = function () {
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
    app.report.spkcreport.last_Day = function () {
        
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
    $('#spkcreport #rq_end', _).val(app.report.spkcreport.last_Day());
    $('#spkcreport #rq_begin', _).val(app.report.spkcreport.first_Day());
    

    
    

    //替换非数字日期
    app.report.spkcreport.renumdou = function (str) {
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
    app.report.spkcreport.sl_qm = 0;
    app.report.spkcreport.je_cb = 0;
    app.report.spkcreport.table_draw = function () {
        
        
        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.spkcreport.tb = $('#report-spkcreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/spkcreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form',_).serializeJson();
                    //dt = app.report.spkcreport.vue.amDate;
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
                { "data": "mc_shop", "sClass": "text-center width200" },
                { "data": "mc_sp", "sClass": "text-center width200" },
                { "data": "barcode", "sClass": "text-center w-100" },
                { "data": "dw", "sClass": "text-center width55" },
                {
                    "data": "sl_qm", "sClass": "text-right w-100", "render": function (data, type, row) {

                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spkcreport.sl_qm += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spkcreport.digit);
                        }
                    }
                },
                {
                    "data": "dj_cb", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            return parseFloat(data).toFixed(app.report.spkcreport.digit);
                        }
                    }
                },
                {
                    "data": "je_cb", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spkcreport.je_cb += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spkcreport.digit);
                        }
                    }
                }
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                app.report.spkcreport.rowCallback();
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
        
        //app.report.spkcreport.fncallback();
        
    }
    //app.report.spkcreport.table_draw();
    app.report.spkcreport.rowCallback = function () {
        $('#spkcreport .sl_qm', _).html(app.report.spkcreport.sl_qm.toFixed(app.report.spkcreport.digit));
        $('#spkcreport .je_cb', _).html(app.report.spkcreport.je_cb.toFixed(app.report.spkcreport.digit));
    }
    //datatables设置
    
    $('#spkcreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.spkcreport.sl_qm = 0;
        app.report.spkcreport.je_cb = 0;
    });
    $("#spkcreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
            
            if (json.outstr == 0) {
                //alert("没有数据");
                //json.data = {};
                //json.draw = draw;
                
                json.data = {};
                json.recordsTotal = 0;
                json.recordsFiltered = 0;
                app.report.spkcreport.rowCallback();
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
        $('#spkcreport #btn-search', _).click(function () {
            $(this).attr("disabled", "disabled");
            _button = $(this);
            setTimeout(function () {
                _button.removeAttr("disabled");
            }, 500);
            app.report.spkcreport.table_search();
            
        });
        app.report.spkcreport.table_search = function () {
            
            if (app.report.spkcreport.tb) {
                app.report.spkcreport.tb.ajax.reload();
                return false;
            } else {
                app.report.spkcreport.table_draw();
          }
           
            
        }
    
}

