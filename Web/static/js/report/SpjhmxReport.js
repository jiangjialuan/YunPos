
//
if (app.report.spjhmxreport) {
    delete app.report.spjhmxreport.tb;
}
$(function () {
    $('div[contentID="report/spjhmxreport"]').attr({ controller: 'report', action: 'spjhmxreport' });
    app.c.public_data['report/spjhmxreport'] = app.c.public_data['report/spjhmxreport'] || {};
    app.c.public_data['report/spjhmxreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.spjhmxreport.vue = new Vue({
        el: "#spjhmxreport",
        data: {
            selected: app.report.spjhmxreport.report_idshop,
            getshop: [],
            getuser: []

        },       
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.spjhmxreport.vue.getshop = data;
                }, function () {});
            },
            getuserfunc: function () {
                app.request($.DHB.U('Gys/GetGysApi'), {}, function (data) {
                    app.report.spjhmxreport.vue.getuser = data;
                }, function () { });
            },
            
        }
    });
    //门店名称初始化
    app.report.spjhmxreport.vue.getshopfunc();
    //收银员初始化
    app.report.spjhmxreport.vue.getuserfunc()
})



app.report.spjhmxreport_ready = function () {

    //日期插件初始化
    $('#spjhmxreport #rq_begin,#spjhmxreport #rq_end', _).click(function () {
        app.report.spjhmxreport.wdatepicker_report_spjhmxreport(this);
    });
    app.report.spjhmxreport.wdatepicker_report_spjhmxreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.spjhmxreport.first_Day = function () {
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
    app.report.spjhmxreport.last_Day = function () {
        
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
    app.report.spjhmxreport.last_Day()
    $('#spjhmxreport #rq_end', _).val(app.report.spjhmxreport.last_Day());
    $('#spjhmxreport #rq_begin', _).val(app.report.spjhmxreport.first_Day());
    


    //替换非数字日期
    app.report.spjhmxreport.renumdou = function (str) {
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
    app.report.spjhmxreport.sl = 0;
    app.report.spjhmxreport.je = 0;
    app.report.spjhmxreport.table_draw = function () {
        
        
        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.spjhmxreport.tb = $('#report-spjhmxreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/spjhmxreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form', _).serializeJson();
                    dt.barcode = app.str_del_bank(dt.barcode);
                    dt.mc_sp = app.str_del_bank(dt.mc_sp);
                    dt.dh = app.str_del_bank(dt.dh);
                    //dt = app.report.spjhmxreport.vue.amDate;
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
                
                { "data": "mc_shop", "sClass": "w-124" },
                { "data": "mc_gys", "sClass": "w-100" },
                {
                    "data": "rq", "sClass": "text-center w-100", "render": function (data, type, row) {
                        var dd = parseInt(app.report.spjhmxreport.renumdou(data));
                        return (new Date(parseInt(dd)).Format("yyyy-MM-dd"));;
                    }
                },
                { "data": "dh", "sClass": "text-center w-124" },
                { "data": "mc_sp", "sClass": "width200" },
                { "data": "dw", "sClass": "text-center w-70" },
                
                {
                    "data": "sl", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spjhmxreport.sl += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spjhmxreport.digit);
                        }
                    }
                },
                {
                    "data": "dj", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            return parseFloat(data).toFixed(app.report.spjhmxreport.digit);
                        }
                    }
                },
                
                {
                    "data": "je", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spjhmxreport.je += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spjhmxreport.digit);
                        }
                    }
                },
                { "data": "barcode", "sClass": "text-center w-100" },
                { "data": "mc_spfl", "sClass": "text-center w-100" }
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                app.report.spjhmxreport.rowCallback();
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
        
        //app.report.spjhmxreport.fncallback();
        
    }
    //app.report.spjhmxreport.table_draw();
    app.report.spjhmxreport.rowCallback = function () {
        $('#spjhmxreport .sl', _).html(app.report.spjhmxreport.sl.toFixed(app.report.spjhmxreport.digit));
        $('#spjhmxreport .je', _).html(app.report.spjhmxreport.je.toFixed(app.report.spjhmxreport.digit));
    }
    //datatables设置
    
    $('#spjhmxreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.spjhmxreport.sl = 0;
        app.report.spjhmxreport.je = 0;
    });
    $("#spjhmxreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
            
            if (json.outstr == 0) {
                //alert("没有数据");
                //json.data = {};
                //json.draw = draw;
                
                json.data = {};
                json.recordsTotal = 0;
                json.recordsFiltered = 0;
                app.report.spjhmxreport.rowCallback();
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
        $('#spjhmxreport #btn-search', _).click(function () {
            $(this).attr("disabled", "disabled");
            _button = $(this);
            setTimeout(function () {
                _button.removeAttr("disabled");
            }, 500);
            if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {

                app.report.spjhmxreport.table_search();
            } else {
                $.DHB.message({ "content": "开始时间不能大于结束时间" });
            }
            
        });
        app.report.spjhmxreport.table_search = function () {
            
            if (app.report.spjhmxreport.tb) {
                app.report.spjhmxreport.tb.ajax.reload();
                return false;
            } else {
                app.report.spjhmxreport.table_draw();
          }
           
            
        }
    
}

