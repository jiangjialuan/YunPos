
//
if (app.report.xshzshopdayreport) {
    delete app.report.xshzshopdayreport.tb;
}
$(function () {
    $('div[contentID="report/xshzshopdayreport"]').attr({ controller: 'report', action: 'xshzshopdayreport' });
    app.c.public_data['report/xshzshopdayreport'] = app.c.public_data['report/xshzshopdayreport'] || {};
    app.c.public_data['report/xshzshopdayreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.xshzshopdayreport.vue = new Vue({
        el: "#xshzshopdayreport",
        data: {
            selected: app.report.xshzshopdayreport.report_idshop,
            getshop: []

        },       
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.xshzshopdayreport.vue.getshop = data;
                }, function () {});
            },
            
            
        }
    });
    //门店名称初始化
    app.report.xshzshopdayreport.vue.getshopfunc();
    
})



app.report.xshzshopdayreport_ready = function () {

    //日期插件初始化
    $('#xshzshopdayreport #rq_begin,#xshzshopdayreport #rq_end', _).click(function () {
        app.report.xshzshopdayreport.wdatepicker_report_xshzshopdayreport(this);
    });
    app.report.xshzshopdayreport.wdatepicker_report_xshzshopdayreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.xshzshopdayreport.first_Day = function () {
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
    app.report.xshzshopdayreport.last_Day = function () {
        
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
    app.report.xshzshopdayreport.last_Day()
    $('#xshzshopdayreport #rq_end', _).val(app.report.xshzshopdayreport.last_Day());
    $('#xshzshopdayreport #rq_begin', _).val(app.report.xshzshopdayreport.first_Day());
    
    //替换非数字日期
    app.report.xshzshopdayreport.renumdou = function (str) {
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
    app.report.xshzshopdayreport.je_xs = 0;
    app.report.xshzshopdayreport.je_zk = 0;
    app.report.xshzshopdayreport.je_xs_sj = 0;
    app.report.xshzshopdayreport.je_cb = 0;
    app.report.xshzshopdayreport.je_ml = 0;
    app.report.xshzshopdayreport.sl_cust = 0;
    app.report.xshzshopdayreport.table_draw = function () {
        
        
        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.xshzshopdayreport.tb = $('#report-xshzshopdayreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/xshzshopdayreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form',_).serializeJson();
                    //dt = app.report.xshzshopdayreport.vue.amDate;
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
                        ////console.log(row);
                        //var dd = parseInt(app.report.xshzshopdayreport.renumdou(data));
                        //return (new Date(parseInt(dd)).Format("yyyy-MM-dd"));;
                        return data;
                    }
                },
                {
                    "data": "weekday", "sClass": "text-center", "render": function (data, type, row) {
                        
                        if (row["rq"] == "") {
                            return "";
                        } else {
                            var dateStr = row["rq"].split("-").join("/");
                            var weekday = new Date(dateStr).getDay();
                            switch (weekday) {
                                case 0: return "星期日"; break;
                                case 1: return "星期一"; break;
                                case 2: return "星期二"; break;
                                case 3: return "星期三"; break;
                                case 4: return "星期四"; break;
                                case 5: return "星期五"; break;
                                case 6: return "星期六"; break;
                            }
                        }
                        
                    }
                },
                { "data": "mc_shop", "sClass": "width200" },
                
                {
                    "data": "je_xs", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.xshzshopdayreport.je_xs += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.xshzshopdayreport.digit);
                        }
                    }
                },
                {
                    "data": "je_yh", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.xshzshopdayreport.je_zk += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.xshzshopdayreport.digit);
                        }
                    }
                },
                {
                    "data": "je_ys", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.xshzshopdayreport.je_xs_sj += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.xshzshopdayreport.digit);
                        }
                    }
                },
                {
                    "data": "je_cb", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.xshzshopdayreport.je_cb += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.xshzshopdayreport.digit);
                        }
                    }
                },
                {
                    "data": "je_ml", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.xshzshopdayreport.je_ml += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.xshzshopdayreport.digit);
                        }
                    }
                },
                {
                    "data": null, "sClass": "text-right width55", "render": function (data, type, row) {
                        //mll=je_ml/je_ys
                        if (row["je_ml"] != "" && row["je_ml"] != undefined && row["je_ys"] != '' && row["je_ys"]!=undefined) {
                            return parseFloat(row["je_ml"] / row["je_ys"] * 100).toFixed(app.report.xshzshopdayreport.digit) + '%';
                        } else {
                            return ""
                        }
                    }
                },
                {
                    "data": "sl_cust", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.xshzshopdayreport.sl_cust += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.xshzshopdayreport.digit);
                        }
                    }
                },
                
                {
                    "data": null, "sClass": "text-right w-100", "render": function (data, type, row) {
                        //dj_cust=je_ys/sl_cust
                        if (row["je_ys"] == "" || row["je_ys"] == undefined) {
                            return "";
                        } else {
                            return parseFloat(row["je_ys"] / row["sl_cust"]).toFixed(app.report.xshzshopdayreport.digit);
                        }
                        
                    }
                },
                
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                app.report.xshzshopdayreport.rowCallback();
                
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
        
        //app.report.xshzshopdayreport.fncallback();
        
    }
    //app.report.xshzshopdayreport.table_draw();
    app.report.xshzshopdayreport.rowCallback = function () {
        $('#xshzshopdayreport .je_xs', _).html(app.report.xshzshopdayreport.je_xs.toFixed(app.report.xshzshopdayreport.digit));
        $('#xshzshopdayreport .je_zk', _).html(app.report.xshzshopdayreport.je_zk.toFixed(app.report.xshzshopdayreport.digit));
        $('#xshzshopdayreport .je_xs_sj', _).html(app.report.xshzshopdayreport.je_xs_sj.toFixed(app.report.xshzshopdayreport.digit));
        $('#xshzshopdayreport .je_cb', _).html(app.report.xshzshopdayreport.je_cb.toFixed(app.report.xshzshopdayreport.digit));
        $('#xshzshopdayreport .je_ml', _).html(app.report.xshzshopdayreport.je_ml.toFixed(app.report.xshzshopdayreport.digit));
        $('#xshzshopdayreport .sl_cust', _).html(app.report.xshzshopdayreport.sl_cust.toFixed(app.report.xshzshopdayreport.digit));
    }
    //datatables设置
    
    $('#xshzshopdayreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.xshzshopdayreport.je_xs = 0;
        app.report.xshzshopdayreport.je_zk = 0;
        app.report.xshzshopdayreport.je_xs_sj = 0;
        app.report.xshzshopdayreport.je_cb = 0;
        app.report.xshzshopdayreport.je_ml = 0;
        app.report.xshzshopdayreport.sl_cust = 0;
    });
    $("#xshzshopdayreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
            
            if (json.outstr == 0) {
                //alert("没有数据");
                //json.data = {};
                //json.draw = draw;
                
                json.data = {};
                json.recordsTotal = 0;
                json.recordsFiltered = 0;
                app.report.xshzshopdayreport.rowCallback();
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
        $('#xshzshopdayreport #btn-search', _).click(function () {
            $(this).attr("disabled", "disabled");
            _button = $(this);
            setTimeout(function () {
                _button.removeAttr("disabled");
            }, 500);
            if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {
                app.report.xshzshopdayreport.table_search();
            } else {
                $.DHB.message({ "content": "开始时间不能大于结束时间" });
            }
            
        });
        app.report.xshzshopdayreport.table_search = function () {
            
            if (app.report.xshzshopdayreport.tb) {
                app.report.xshzshopdayreport.tb.ajax.reload();
                return false;
            } else {
                app.report.xshzshopdayreport.table_draw();
          }
           
            
        }
    
}

