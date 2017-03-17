
//
if (app.report.xshzshopreport) {
    delete app.report.xshzshopreport.tb;
}
$(function () {
    $('div[contentID="report/xshzshopreport"]').attr({ controller: 'report', action: 'xshzshopreport' });
    app.c.public_data['report/xshzshopreport'] = app.c.public_data['report/xshzshopreport'] || {};
    app.c.public_data['report/xshzshopreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.xshzshopreport.vue = new Vue({
        el: "#xshzshopreport",
        data: {
            selected: app.report.xshzshopreport.report_idshop,
            getshop: [],
            

        },       
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.xshzshopreport.vue.getshop = data;
                }, function () {});
            },
            
            
        }
    });
    //门店名称初始化
    app.report.xshzshopreport.vue.getshopfunc();
   
})



app.report.xshzshopreport_ready = function () {

    //日期插件初始化
    $('#xshzshopreport #rq_begin,#xshzshopreport #rq_end', _).click(function () {
        app.report.xshzshopreport.wdatepicker_report_xshzshopreport(this);
    });
    app.report.xshzshopreport.wdatepicker_report_xshzshopreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.xshzshopreport.first_Day = function () {
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
    app.report.xshzshopreport.last_Day = function () {
        
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
    
    $('#xshzshopreport #rq_end', _).val(app.report.xshzshopreport.last_Day());
    $('#xshzshopreport #rq_begin', _).val(app.report.xshzshopreport.first_Day());
    

    //替换非数字日期
    app.report.xshzshopreport.renumdou = function (str) {
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
    app.report.xshzshopreport.sl_cust = 0;
    app.report.xshzshopreport.je_ml = 0;
    app.report.xshzshopreport.je_cb = 0;
    app.report.xshzshopreport.je_xs_sj = 0;
    app.report.xshzshopreport.je_zk = 0;
    app.report.xshzshopreport.je_xs = 0;
    app.report.xshzshopreport.table_draw = function () {
        
        
        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.xshzshopreport.tb = $('#report-xshzshopreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/xshzshopreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form',_).serializeJson();
                    //dt = app.report.xshzshopreport.vue.amDate;
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
                
                {
                    "data": "je_xs", "sClass": "text-right w-100", "render": function (data, type, row) {
                        ////console.log(row);
                        
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.xshzshopreport.je_xs += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.xshzshopreport.digit);
                        }
                    }
                },
                {
                    "data": "je_yh", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.xshzshopreport.je_zk += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.xshzshopreport.digit);
                        }
                    }
                },
                {
                    "data": "je_ys", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.xshzshopreport.je_xs_sj += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.xshzshopreport.digit);
                        }
                    }
                },
                {
                    "data": "je_cb", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.xshzshopreport.je_cb += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.xshzshopreport.digit);
                        }
                    }
                },
                {
                    "data": "je_ml", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.xshzshopreport.je_ml += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.xshzshopreport.digit);
                        }
                    }
                },
                {
                    "data": "mll", "sClass": "text-right w-70", "render": function (data, type, row) {
                        //mll=je_ml/je_ys
                        if (row["je_ml"] != "" && row["je_ml"] != undefined && row["je_ys"] != "" && row["je_ys"]!=undefined) {
                            return parseFloat(row["je_ml"] / row["je_ys"] * 100).toFixed(app.report.xshzshopreport.digit) + '%'; 
                        } else {
                            return "";
                        }
                            
                        
                    }
                },
                
                {
                    "data": "sl_cust", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.xshzshopreport.sl_cust += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.xshzshopreport.digit);
                        }
                    }
                },
                
                {
                    "data": null, "sClass": "text-right w-100", "render": function (data, type, row) {
                        //dj_cust=je_xs/sl_cust
                        
                        return parseFloat(row["je_xs"] / row["sl_cust"]).toFixed(app.report.xshzshopreport.digit);
                       
                    }
                },
                
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                
                app.report.xshzshopreport.rowCallback();
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
        
        //app.report.xshzshopreport.fncallback();
        
    }
    //app.report.xshzshopreport.table_draw();
    app.report.xshzshopreport.rowCallback = function () {
        $('#xshzshopreport .sl_cust', _).html(app.report.xshzshopreport.sl_cust.toFixed(app.report.xshzshopreport.digit));
        $('#xshzshopreport .je_ml', _).html(app.report.xshzshopreport.je_ml.toFixed(app.report.xshzshopreport.digit));
        $('#xshzshopreport .je_cb', _).html(app.report.xshzshopreport.je_cb.toFixed(app.report.xshzshopreport.digit));
        $('#xshzshopreport .je_xs_sj', _).html(app.report.xshzshopreport.je_xs_sj.toFixed(app.report.xshzshopreport.digit));
        $('#xshzshopreport .je_zk', _).html(app.report.xshzshopreport.je_zk.toFixed(app.report.xshzshopreport.digit));
        $('#xshzshopreport .je_xs', _).html(app.report.xshzshopreport.je_xs.toFixed(app.report.xshzshopreport.digit));
    }
    //datatables设置
    
    $('#xshzshopreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.xshzshopreport.sl_cust = 0;
        app.report.xshzshopreport.je_ml = 0;
        app.report.xshzshopreport.je_cb = 0;
        app.report.xshzshopreport.je_xs_sj = 0;
        app.report.xshzshopreport.je_zk = 0;
        app.report.xshzshopreport.je_xs = 0;
    });
    $("#xshzshopreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
            
            if (json.outstr == 0) {
                //alert("没有数据");
                //json.data = {};
                //json.draw = draw;
                
                json.data = {};
                json.recordsTotal = 0;
                json.recordsFiltered = 0;
                app.report.xshzshopreport.rowCallback();
                
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
        $('#xshzshopreport #btn-search', _).click(function () {
            $(this).attr("disabled", "disabled");
            _button = $(this);
            setTimeout(function () {
                _button.removeAttr("disabled");
            }, 500);
            if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {
                app.report.xshzshopreport.table_search();
            } else {
                $.DHB.message({ "content": "开始时间不能大于结束时间" });
            }
            
        });
        app.report.xshzshopreport.table_search = function () {
            
            if (app.report.xshzshopreport.tb) {
                app.report.xshzshopreport.tb.ajax.reload();
                return false;
            } else {
                app.report.xshzshopreport.table_draw();
          }
           
            
        }
    
}

