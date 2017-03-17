
//
if (app.report.usersklsreport) {
    delete app.report.usersklsreport.tb;
}

$(function () {
    $('div[contentID="report/usersklsreport"]').attr({ controller: 'report', action: 'usersklsreport' });
    app.c.public_data['report/usersklsreport'] = app.c.public_data['report/usersklsreport'] || {};
    app.c.public_data['report/usersklsreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.usersklsreport.vue = new Vue({
        el: "#usersklsreport",
        data: {
            selected: app.report.usersklsreport.report_idshop,
            getshop: [],
            getuser: []

        },       
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.usersklsreport.vue.getshop = data;
                }, function () {});
            },
            getuserfunc: function () {
                app.request($.DHB.U('SearchCondition/GetUser'), {}, function (data) {
                    app.report.usersklsreport.vue.getuser = data;
                }, function () { });
            },
            
        }
    });
    //门店名称初始化
    app.report.usersklsreport.vue.getshopfunc();
    //收银员初始化
    app.report.usersklsreport.vue.getuserfunc()
})



app.report.usersklsreport_ready = function () {

    //日期插件初始化
    $('#usersklsreport #rq_begin,#usersklsreport #rq_end', _).click(function () {
        app.report.usersklsreport.wdatepicker_report_usersklsreport(this);
    });
    app.report.usersklsreport.wdatepicker_report_usersklsreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.usersklsreport.first_Day = function () {
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
    app.report.usersklsreport.last_Day = function () {
        
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
    app.report.usersklsreport.last_Day()
    $('#usersklsreport #rq_end', _).val(app.report.usersklsreport.last_Day());
    $('#usersklsreport #rq_begin', _).val(app.report.usersklsreport.first_Day());
    //input显示商品分类，选择商品删除按钮
    $('#usersklsreport #shopfl_box,#usersklsreport #id_sp_box', _).hover(function () {
        $(this).parent().find('span:last()').show();
    }, function () {
        $(this).parent().find('span:last()').hide();
    });
    $('#usersklsreport .clear_button', _).click(function () {
        app.report.usersklsreport.report_spxsmx(this);
    });

    
    

    //替换非数字日期
    app.report.usersklsreport.renumdou = function (str) {
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
    app.report.usersklsreport.je = 0;
    app.report.usersklsreport.table_draw = function () {
        
        
        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.usersklsreport.tb = $('#report-usersklsreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/usersklsreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form', _).serializeJson();
                    dt.dh = app.str_del_bank(dt.dh);
                    //dt = app.report.usersklsreport.vue.amDate;
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
                        var dd = parseInt(app.report.usersklsreport.renumdou(data));
                        return (new Date(parseInt(dd)).Format("yyyy-MM-dd"));;
                    }
                },
                
                { "data": "mc_shop", "sClass": "w-350" },
                //{ "data": "dh" },
                { "data": "dh", "sClass": "text-center width200" },
                { "data": "name" ,"sClass":"w-70"},                
                { "data": "mc_pay", "sClass": "text-center w-100" },
                {
                    "data": "je", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.usersklsreport.je += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.usersklsreport.digit);
                        }
                    }
                },
                
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                app.report.usersklsreport.rowCallback();
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
        
        //app.report.usersklsreport.fncallback();
        
    }
    //app.report.usersklsreport.table_draw();
    app.report.usersklsreport.rowCallback = function () {
        $('#usersklsreport .je', _).html(app.report.usersklsreport.je.toFixed(app.report.usersklsreport.digit));
    }
    //datatables设置
    
    $('#usersklsreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        
        app.report.usersklsreport.je = 0;
    });
    $("#usersklsreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
            
            if (json.outstr == 0) {
                //alert("没有数据");
                //json.data = {};
                //json.draw = draw;
                
                json.data = {};
                json.recordsTotal = 0;
                json.recordsFiltered = 0;
                app.report.usersklsreport.rowCallback();
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
        $('#usersklsreport #btn-search', _).click(function () {
            $(this).attr("disabled", "disabled");
            _button = $(this);
            setTimeout(function () {
                _button.removeAttr("disabled");
            }, 500);
            if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {
                app.report.usersklsreport.table_search();
            } else {
                $.DHB.message({ "content": "开始时间不能大于结束时间" });
            }
            
        });
        app.report.usersklsreport.table_search = function () {
            
            if (app.report.usersklsreport.tb) {
                app.report.usersklsreport.tb.ajax.reload();
                return false;
            } else {
                app.report.usersklsreport.table_draw();
          }
           
            
        }
    
}

