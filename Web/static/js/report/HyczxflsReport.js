
//
if (app.report.hyczxflsreport) {
    delete app.report.hyczxflsreport.tb;
}
$(function () {
    $('div[contentID="report/hyczxflsreport"]').attr({ controller: 'report', action: 'hyczxflsreport' });
    app.c.public_data['report/hyczxflsreport'] = app.c.public_data['report/hyczxflsreport'] || {};
    app.c.public_data['report/hyczxflsreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.hyczxflsreport.vue = new Vue({
        el: "#hyczxflsreport",
        data: {
            selected: app.report.hyczxflsreport.report_idshop,
            getshop: [],
            

        },       
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.hyczxflsreport.vue.getshop = data;
                }, function () {});
            },
            
            
        }
    });
    //门店名称初始化
    app.report.hyczxflsreport.vue.getshopfunc();
    
})



app.report.hyczxflsreport_ready = function () {

    //日期插件初始化
    $('#hyczxflsreport #rq_begin,#hyczxflsreport #rq_end', _).click(function () {
        app.report.hyczxflsreport.wdatepicker_report_hyczxflsreport(this);
    });
    app.report.hyczxflsreport.wdatepicker_report_hyczxflsreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.hyczxflsreport.first_Day = function () {
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
    app.report.hyczxflsreport.last_Day = function () {
        
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
    app.report.hyczxflsreport.last_Day()
    $('#hyczxflsreport #rq_end', _).val(app.report.hyczxflsreport.last_Day());
    $('#hyczxflsreport #rq_begin', _).val(app.report.hyczxflsreport.first_Day());
   

    

    //替换非数字日期
    app.report.hyczxflsreport.renumdou = function (str) {
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
    
    app.report.hyczxflsreport.je = 0;
    app.report.hyczxflsreport.je_zs = 0;
    app.report.hyczxflsreport.je_ye = 0;
    app.report.hyczxflsreport.table_draw = function () {
        
        
        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.hyczxflsreport.tb = $('#report-hyczxflsreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/hyczxflsreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form', _).serializeJson();
                    dt.name = app.str_del_bank(dt.name);
                    dt.phone = app.str_del_bank(dt.phone);
                    dt.dh = app.str_del_bank(dt.dh);
                    //dt = app.report.hyczxflsreport.vue.amDate;
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
                        var dd = parseInt(app.report.hyczxflsreport.renumdou(data));
                        return (new Date(parseInt(dd)).Format("yyyy-MM-dd"));;
                    }
                },
               
                { "data": "mc_shop", "sClass": "text-center w-100" },
                
                {
                    "data": "mc_djlx", "sClass": "text-center w-100"
                },
                
                { "data": "dh", "sClass": "text-center w-124" },
                { "data": "name", "sClass": "text-center w-70" },
                {
                    "data": "je", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.hyczxflsreport.je += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.hyczxflsreport.digit);
                        }
                    }
                },
                {
                    "data": "je_zs", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.hyczxflsreport.je_zs += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.hyczxflsreport.digit);
                        }
                    }
                },
                {
                    "data": "je_ye", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.hyczxflsreport.je_ye += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.hyczxflsreport.digit);
                        }
                    }
                },
                { "data": "phone", "sClass": "text-center w-100" },
                { "data": "membercard", "sClass": "text-center w-100" }
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                app.report.hyczxflsreport.rowCallback();
                
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
        
        //app.report.hyczxflsreport.fncallback();
        
    }

    //app.report.hyczxflsreport.table_draw();
    app.report.hyczxflsreport.rowCallback = function () {
        $('#hyczxflsreport .je', _).html(app.report.hyczxflsreport.je.toFixed(app.report.hyczxflsreport.digit));
        $('#hyczxflsreport .je_zs', _).html(app.report.hyczxflsreport.je_zs.toFixed(app.report.hyczxflsreport.digit));
        $('#hyczxflsreport .je_ye', _).html(app.report.hyczxflsreport.je_ye.toFixed(app.report.hyczxflsreport.digit));
    }
    //datatables设置
    
    $('#hyczxflsreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.hyczxflsreport.je = 0;
        app.report.hyczxflsreport.je_zs = 0;
        app.report.hyczxflsreport.je_ye = 0;
    });
    $("#hyczxflsreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
            
            if (json.outstr == 0) {
                //alert("没有数据");
                //json.data = {};
                //json.draw = draw;
                
                json.data = {};
                json.recordsTotal = 0;
                json.recordsFiltered = 0;
                app.report.hyczxflsreport.rowCallback();
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
        $('#hyczxflsreport #btn-search', _).click(function () {
            //
            $(this).attr("disabled", "disabled");
            _button = $(this);
            setTimeout(function () {
                _button.removeAttr("disabled");
            }, 500)
            
            if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {
                app.report.hyczxflsreport.table_search();
                
            } else {
                $.DHB.message({ "content": "开始时间不能大于结束时间" });
            }
            
            
        });
        app.report.hyczxflsreport.table_search = function () {
            
            if (app.report.hyczxflsreport.tb) {
                app.report.hyczxflsreport.tb.ajax.reload();
                return false;
            } else {
                app.report.hyczxflsreport.table_draw();
          }
           
            
        }
    
}

