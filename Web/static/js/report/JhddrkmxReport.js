
//
if (app.report.jhddrkmxreport) {
    delete app.report.jhddrkmxreport.tb;
}
$(function () {
    $('div[contentID="report/jhddrkmxreport"]').attr({ controller: 'report', action: 'jhddrkmxreport' });
    app.c.public_data['report/jhddrkmxreport'] = app.c.public_data['report/jhddrkmxreport'] || {};
    app.c.public_data['report/jhddrkmxreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.jhddrkmxreport.vue = new Vue({
        el: "#jhddrkmxreport",
        data: {
            selected: app.report.jhddrkmxreport.report_idshop,
            getshop: [],
            getuser: []

        },       
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.jhddrkmxreport.vue.getshop = data;
                }, function () {});
            },
            getuserfunc: function () {
                app.request($.DHB.U('Gys/GetGysApi'), {}, function (data) {
                    //console.log(data);
                    app.report.jhddrkmxreport.vue.getuser = data;
                }, function () { });
            },
            
        }
    });
    //门店名称初始化
    app.report.jhddrkmxreport.vue.getshopfunc();
    //收银员初始化
    app.report.jhddrkmxreport.vue.getuserfunc()
})



app.report.jhddrkmxreport_ready = function () {

    //日期插件初始化
    $('#jhddrkmxreport #rq_begin,#jhddrkmxreport #rq_end', _).click(function () {
        app.report.jhddrkmxreport.wdatepicker_report_jhddrkmxreport(this);
    });
    app.report.jhddrkmxreport.wdatepicker_report_jhddrkmxreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.jhddrkmxreport.first_Day = function () {
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
    app.report.jhddrkmxreport.last_Day = function () {
        
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
    app.report.jhddrkmxreport.last_Day()
    $('#jhddrkmxreport #rq_end', _).val(app.report.jhddrkmxreport.last_Day());
    $('#jhddrkmxreport #rq_begin', _).val(app.report.jhddrkmxreport.first_Day());


    

    //替换非数字日期
    app.report.jhddrkmxreport.renumdou = function (str) {
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
    app.report.jhddrkmxreport.sl_dd = 0;
    app.report.jhddrkmxreport.je_dd = 0;
    app.report.jhddrkmxreport.sl_sh = 0;
    app.report.jhddrkmxreport.sl_cy = 0;
    app.report.jhddrkmxreport.je_sh = 0;
    app.report.jhddrkmxreport.je_cy = 0;
    app.report.jhddrkmxreport.table_draw = function () {
        
        
        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.jhddrkmxreport.tb = $('#report-jhddrkmxreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/jhddrkmxreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form', _).serializeJson();
                    dt.dh_dd = app.str_del_bank(dt.dh_dd);
                    //dt = app.report.jhddrkmxreport.vue.amDate;
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
                { "data": null, "sClass": "text-center width55" },
                {
                    "data": "rq", "sClass": "text-center w-100", "render": function (data, type, row) {
                        var dd = parseInt(app.report.jhddrkmxreport.renumdou(data));
                        return (new Date(parseInt(dd)).Format("yyyy-MM-dd"));;
                    }
                },
                
                { "data": "dh_dd", "sClass": "text-center w-124" },
                { "data": "mc_gys", "sClass": "text-center w-100" },
                {
                    "data": "mc_shop_sh", "sClass": "text-center w-160"
                },
                
                { "data": "dh_sh", "sClass": "text-center w-124" },
                { "data": "mc_sp", "sClass": "width200" },
                { "data": "dw", "sClass": "text-center w-70" },
                {
                    "data": "sl_dd", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.jhddrkmxreport.sl_dd += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.jhddrkmxreport.digit);
                        }
                    }
                },
                {
                    "data": "dj_dd", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            return parseFloat(data).toFixed(app.report.jhddrkmxreport.digit);
                        }
                    }
                },
                {
                    "data": "je_dd", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.jhddrkmxreport.je_dd += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.jhddrkmxreport.digit);
                        }
                    }
                },
                {
                    "data": "sl_sh", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.jhddrkmxreport.sl_sh += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.jhddrkmxreport.digit);
                        }
                    }
                },
                {
                    "data": "sl_cy", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.jhddrkmxreport.sl_cy += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.jhddrkmxreport.digit);
                        }
                    }
                },
                {
                    "data": "je_sh", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.jhddrkmxreport.je_sh += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.jhddrkmxreport.digit);
                        }
                    }
                },
                {
                    "data": "je_cy", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.jhddrkmxreport.je_cy += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.jhddrkmxreport.digit);
                        }
                    }
                },
                { "data": "barcode", "sClass": "text-center w-124" },
                { "data": "mc_spfl", "sClass": "text-center w-100" }
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                
                app.report.jhddrkmxreport.rowCallback();
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
        
        //app.report.jhddrkmxreport.fncallback();
        
    }
    //app.report.jhddrkmxreport.table_draw();
    app.report.jhddrkmxreport.rowCallback = function () {
        $('#jhddrkmxreport .sl_dd', _).html(app.report.jhddrkmxreport.sl_dd.toFixed(app.report.jhddrkmxreport.digit));
        $('#jhddrkmxreport .je_dd', _).html(app.report.jhddrkmxreport.je_dd.toFixed(app.report.jhddrkmxreport.digit));
        $('#jhddrkmxreport .sl_sh', _).html(app.report.jhddrkmxreport.sl_sh.toFixed(app.report.jhddrkmxreport.digit));
        $('#jhddrkmxreport .sl_cy', _).html(app.report.jhddrkmxreport.sl_cy.toFixed(app.report.jhddrkmxreport.digit));
        $('#jhddrkmxreport .je_sh', _).html(app.report.jhddrkmxreport.je_sh.toFixed(app.report.jhddrkmxreport.digit));
        $('#jhddrkmxreport .je_cy', _).html(app.report.jhddrkmxreport.je_cy.toFixed(app.report.jhddrkmxreport.digit));
    }
    //datatables设置
    
    $('#jhddrkmxreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.jhddrkmxreport.sl_dd = 0;
        app.report.jhddrkmxreport.je_dd = 0;
        app.report.jhddrkmxreport.sl_sh = 0;
        app.report.jhddrkmxreport.sl_cy = 0;
        app.report.jhddrkmxreport.je_sh = 0;
        app.report.jhddrkmxreport.je_cy = 0;
    });
    $("#jhddrkmxreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
            
            if (json.outstr == 0) {
                //alert("没有数据");
                //json.data = {};
                //json.draw = draw;
                
                json.data = {};
                json.recordsTotal = 0;
                json.recordsFiltered = 0;
                app.report.jhddrkmxreport.rowCallback();
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
        $('#jhddrkmxreport #btn-search', _).click(function () {
            $(this).attr("disabled", "disabled");
            _button = $(this);
            setTimeout(function () {
                _button.removeAttr("disabled");
            }, 500)
            if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {
                app.report.jhddrkmxreport.table_search();
            } else {
                $.DHB.message({ "content": "开始时间不能大于结束时间" });
            }
            
        });
        app.report.jhddrkmxreport.table_search = function () {
            
            if (app.report.jhddrkmxreport.tb) {
                app.report.jhddrkmxreport.tb.ajax.reload();
                return false;
            } else {
                app.report.jhddrkmxreport.table_draw();
          }
           
            
        }
    
}

