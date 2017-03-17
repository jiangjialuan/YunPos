
//
if (app.report.skhzreport) {
    delete app.report.skhzreport.tb;
}
$(function () {
    $('div[contentID="report/skhzreport"]').attr({ controller: 'report', action: 'skhzreport' });
    app.c.public_data['report/skhzreport'] = app.c.public_data['report/skhzreport'] || {};
    app.c.public_data['report/skhzreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.skhzreport.vue = new Vue({
        el: "#skhzreport",
        data: {
            selected: app.report.skhzreport.report_idshop,
            getshop: [],
            getuser: []

        },       
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.skhzreport.vue.getshop = data;
                }, function () {});
            },
            getuserfunc: function () {
                app.request($.DHB.U('SearchCondition/GetUser'), {}, function (data) {
                    app.report.skhzreport.vue.getuser = data;
                }, function () { });
            },
            
        }
    });
    //门店名称初始化
    app.report.skhzreport.vue.getshopfunc();
    //收银员初始化
    app.report.skhzreport.vue.getuserfunc()
})



app.report.skhzreport_ready = function () {

    //日期插件初始化
    $('#skhzreport #rq_begin,#skhzreport #rq_end', _).click(function () {
        app.report.skhzreport.wdatepicker_report_skhzreport(this);
    });
    app.report.skhzreport.wdatepicker_report_skhzreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.skhzreport.first_Day = function () {
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
    app.report.skhzreport.last_Day = function () {
        
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
    app.report.skhzreport.last_Day()
    $('#skhzreport #rq_end', _).val(app.report.skhzreport.last_Day());
    $('#skhzreport #rq_begin', _).val(app.report.skhzreport.first_Day());

    

    //替换非数字日期
    app.report.skhzreport.renumdou = function (str) {
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
    app.report.skhzreport.je_cash = 0;
    app.report.skhzreport.je_bank = 0;
    app.report.skhzreport.je_yhq = 0;
    app.report.skhzreport.je_hy = 0;
    app.report.skhzreport.je_alipay = 0;
    app.report.skhzreport.je_wxpay = 0;
    app.report.skhzreport.je_sum = 0;
    app.report.skhzreport.table_draw = function () {
        
        
        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.skhzreport.tb = $('#report-skhzreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/skhzreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form',_).serializeJson();
                    //dt = app.report.skhzreport.vue.amDate;
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
                        ////console.log(row);
                        var dd = parseInt(app.report.skhzreport.renumdou(data));
                        return (new Date(parseInt(dd)).Format("yyyy-MM-dd"));;
                    }
                },

                { "data": "mc_shop", "sClass": "w-200" },

                { "data": "mc_user" ,"sClass":"w-70"},

                {
                    "data": "je_cash", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.skhzreport.je_cash += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.skhzreport.digit);
                        }
                    }
                },

                {
                    "data": "je_bank", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.skhzreport.je_bank += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.skhzreport.digit);
                        }
                    }
                },
                {
                    "data": "je_yhq", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.skhzreport.je_yhq += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.skhzreport.digit);
                        }
                    }
                },
                {
                    "data": "je_hy", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.skhzreport.je_hy += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.skhzreport.digit);
                        }
                    }
                },
                {
                    "data": "je_alipay", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.skhzreport.je_alipay += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.skhzreport.digit);
                        }
                    }
                },
                {
                    "data": "je_wxpay", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.skhzreport.je_wxpay += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.skhzreport.digit);
                        }
                    }
                },
                {
                    "data": "je_sum", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.skhzreport.je_sum += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.skhzreport.digit);
                        }
                    }
                },
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                app.report.skhzreport.rowCallback();
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
        
        //app.report.skhzreport.fncallback();
        
    }
    //app.report.skhzreport.table_draw();
    app.report.skhzreport.rowCallback = function () {
        ////console.log(app.report.skhzreport.je_cashs);
        $('#skhzreport .je_cash', _).html(app.report.skhzreport.je_cash.toFixed(app.report.skhzreport.digit));
        $('#skhzreport .je_bank', _).html(app.report.skhzreport.je_bank.toFixed(app.report.skhzreport.digit));
        $('#skhzreport .je_yhq', _).html(app.report.skhzreport.je_yhq.toFixed(app.report.skhzreport.digit));
        $('#skhzreport .je_hy', _).html(app.report.skhzreport.je_hy.toFixed(app.report.skhzreport.digit));
        $('#skhzreport .je_alipay', _).html(app.report.skhzreport.je_alipay.toFixed(app.report.skhzreport.digit));
        $('#skhzreport .je_wxpay', _).html(app.report.skhzreport.je_wxpay.toFixed(app.report.skhzreport.digit));
        $('#skhzreport .je_sum', _).html(app.report.skhzreport.je_sum.toFixed(app.report.skhzreport.digit));
    }
    //datatables设置
    
    $('#skhzreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.skhzreport.je_cash = 0;
        app.report.skhzreport.je_bank = 0;
        app.report.skhzreport.je_yhq = 0;
        app.report.skhzreport.je_hy = 0;
        app.report.skhzreport.je_alipay = 0;
        app.report.skhzreport.je_wxpay = 0;
        app.report.skhzreport.je_sum = 0;
    });
    $("#skhzreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
            
            if (json.outstr == 0) {
                //alert("没有数据");
                //json.data = {};
                //json.draw = draw;
                
                json.data = {};
                json.recordsTotal = 0;
                json.recordsFiltered = 0;
                app.report.skhzreport.rowCallback();
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
        $('#skhzreport #btn-search', _).click(function () {
            $(this).attr("disabled", "disabled");
            _button = $(this);
            setTimeout(function () {
                _button.removeAttr("disabled");
            }, 500)
            if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {
                app.report.skhzreport.table_search();
            } else {
                $.DHB.message({ "content": "开始时间不能大于结束时间" });
            }
            
        });
        app.report.skhzreport.table_search = function () {
            
            if (app.report.skhzreport.tb) {
                app.report.skhzreport.tb.ajax.reload();
                return false;
            } else {
                app.report.skhzreport.table_draw();
          }
           
            
        }
    
}

