
//
if (app.report.pssqcrkmxreport) {
    delete app.report.pssqcrkmxreport.tb;
}
$(function () {
    $('div[contentID="report/pssqcrkmxreport"]').attr({ controller: 'report', action: 'pssqcrkmxreport' });
    app.c.public_data['report/pssqcrkmxreport'] = app.c.public_data['report/pssqcrkmxreport'] || {};
    app.c.public_data['report/pssqcrkmxreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.pssqcrkmxreport.vue = new Vue({
        el: "#pssqcrkmxreport",
        data: {
            selected: "",
            getshop: [],
            //getuser: []

        },
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.pssqcrkmxreport.vue.getshop = data;
                }, function () { });
            },
            

        }
    });
    //门店名称初始化
    app.report.pssqcrkmxreport.vue.getshopfunc();
})



app.report.pssqcrkmxreport_ready = function () {

    //日期插件初始化
    $('#pssqcrkmxreport #rq_begin,#pssqcrkmxreport #rq_end', _).click(function () {
        app.report.pssqcrkmxreport.wdatepicker_report_pssqcrkmxreport(this);
    });
    app.report.pssqcrkmxreport.wdatepicker_report_pssqcrkmxreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.pssqcrkmxreport.first_Day = function () {
        var nowdate = new Date();
        var y = nowdate.getFullYear();
        var m = nowdate.getMonth() + 1;
        var d = nowdate.getDate();
        if (parseInt(m) < 10) {
            m = "0" + m;
        }
        var formatnowdate = y + '-' + m + '-' + '01';
        return formatnowdate;
    }
    app.report.pssqcrkmxreport.last_Day = function () {

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
    app.report.pssqcrkmxreport.last_Day()
    $('#pssqcrkmxreport #rq_end', _).val(app.report.pssqcrkmxreport.last_Day());
    $('#pssqcrkmxreport #rq_begin', _).val(app.report.pssqcrkmxreport.first_Day());




    //替换非数字日期
    app.report.pssqcrkmxreport.renumdou = function (str) {
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
    app.report.pssqcrkmxreport.sl_sq = 0;
    app.report.pssqcrkmxreport.je_sq = 0;
    app.report.pssqcrkmxreport.sl_ck = 0;
    app.report.pssqcrkmxreport.sl_cy_ck = 0;
    app.report.pssqcrkmxreport.sl_rk = 0;
    app.report.pssqcrkmxreport.sl_cy_rk = 0;
    app.report.pssqcrkmxreport.table_draw = function () {
        

        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.pssqcrkmxreport.tb = $('#report-pssqcrkmxreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/pssqcrkmxreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form', _).serializeJson();
                    dt.dh_sq = app.str_del_bank(dt.dh_sq);
                    //dt = app.report.pssqcrkmxreport.vue.amDate;
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

                    'next': '>',
                    'previous': '<'
                },
            },
            serverSide: true,
            processing: false,
            lengthMenu: [25, 10, 20, 30, 50, 80, 100],
            ////stateSave: true,    //该参数会将分页状态保存, 影响到每页行数
            pageLength: 25,
            autoWidth:false,
            columns: [
                { "data": null, "sClass": "text-center w-40" },
                {
                    "data": "rq", "sClass": "text-center w-100", "render": function (data, type, row) {
                       // //console.log(row);
                        var dd = parseInt(app.report.pssqcrkmxreport.renumdou(data));
                        return (new Date(parseInt(dd)).Format("yyyy-MM-dd"));;
                    }
                },
                
                { "data": "dh_sq", "sClass": "text-center w-124" },
                { "data": "mc_shop_sq", "sClass": "text-center w-100" },
                //{ "data": "dh" },
                { "data": "mc_shop_ck", "sClass": "text-center w-100" },
                { "data": "mc_sp", "sClass": "width200" },
                { "data": "dw", "sClass": "text-center width55" },
                
                {
                    "data": "sl_sq", "sClass": "text-right w-100", "render": function (data, type, row) {
                        
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.pssqcrkmxreport.sl_sq += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.pssqcrkmxreport.digit);
                        }
                    }
                },
                {
                    "data": "dj_sq", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return 0;
                        } else {
                            return parseFloat(data).toFixed(app.report.pssqcrkmxreport.digit);
                        }
                    }
                },
                {
                    "data": "je_sq", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return 0;
                        } else {
                            app.report.pssqcrkmxreport.je_sq += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.pssqcrkmxreport.digit);
                        }
                    }
                },

                { "data": "dh_ck", "sClass": "text-center w-124" },
                {
                    "data": "sl_ck", "sClass": "text-right", "render": function (data, type, row) {
                        
                        if (data == '' || data == null) {
                            return 0;
                        } else {
                            app.report.pssqcrkmxreport.sl_ck += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.pssqcrkmxreport.digit);
                        }
                    }
                },
                {
                    "data": null, "sClass": "text-right w-100", "render": function (data, type, row) {
                        //sl_cy_ck=sl_sq-sl_ck
                        if (row["sl_sq"] != undefined && row["sl_sq"] != '' && row["sl_ck"] != undefined && row["sl_ck"] != '') {
                            app.report.pssqcrkmxreport.sl_cy_ck += parseFloat(row["sl_sq"] - row["sl_ck"]);
                            return parseFloat(row["sl_sq"] - row["sl_ck"]).toFixed(app.report.pssqcrkmxreport.digit);
                        } else {
                            return 0
                        }
                        
                    }
                },
                { "data": "dh_rk", "sClass": "text-center w-124" },
                {
                    "data": "sl_rk", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return 0;
                        } else {
                            app.report.pssqcrkmxreport.sl_rk += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.pssqcrkmxreport.digit);
                        }
                    }
                },
                {
                    "data": null, "sClass": "text-right w-100", "render": function (data, type, row) {
                        //sl_cy_rk=sl_ck-sl_rk
                        if (row["sl_rk"] != undefined && row["sl_rk"] != '' && row["sl_ck"] != undefined && row["sl_ck"] != '') {
                            app.report.pssqcrkmxreport.sl_cy_rk += parseFloat(row["sl_ck"] - row["sl_rk"]);
                            return parseFloat(row["sl_ck"] - row["sl_rk"]).toFixed(app.report.pssqcrkmxreport.digit);
                        } else {
                            return 0
                        }
                        if (data == '' || data == null) {
                            return 0;
                        } else {
                            app.report.pssqcrkmxreport.sl_cy_rk += parseFloat(row["sl_ck"] - row["sl_rk"]);
                            return parseFloat(data).toFixed(app.report.pssqcrkmxreport.digit);
                        }
                    }
                },
                { "data": "barcode", "sClass": "text-center w-124" },
                { "data": "mc_spfl", "sClass": "text-center w-100" }
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                app.report.pssqcrkmxreport.rowCallback();
                
            },
            "fnDrawCallback": function () {
                var api = this.api();
                var startIndex = api.context[0]._iDisplayStart;//获取到本页开始的条数
                api.column(0).nodes().each(function (cell, i) {
                    cell.innerHTML = startIndex + i + 1;
                });
            },
            "dom": 'rt<"pagecss"ilp><"clear">',


        });

        //app.report.pssqcrkmxreport.fncallback();

    }
    //app.report.pssqcrkmxreport.table_draw();
    app.report.pssqcrkmxreport.rowCallback = function () {
        $('#pssqcrkmxreport .sl_sq', _).html(app.report.pssqcrkmxreport.sl_sq.toFixed(app.report.pssqcrkmxreport.digit));
        $('#pssqcrkmxreport .je_sq', _).html(app.report.pssqcrkmxreport.je_sq.toFixed(app.report.pssqcrkmxreport.digit));
        $('#pssqcrkmxreport .sl_ck', _).html(app.report.pssqcrkmxreport.sl_ck.toFixed(app.report.pssqcrkmxreport.digit));
        $('#pssqcrkmxreport .sl_cy_ck', _).html(app.report.pssqcrkmxreport.sl_cy_ck.toFixed(app.report.pssqcrkmxreport.digit));
        $('#pssqcrkmxreport .sl_rk', _).html(app.report.pssqcrkmxreport.sl_rk.toFixed(app.report.pssqcrkmxreport.digit));
        $('#pssqcrkmxreport .sl_cy_rk', _).html(app.report.pssqcrkmxreport.sl_cy_rk.toFixed(app.report.pssqcrkmxreport.digit));
    }
    //datatables设置

    $('#pssqcrkmxreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.pssqcrkmxreport.sl_sq = 0;
        app.report.pssqcrkmxreport.je_sq = 0;
        app.report.pssqcrkmxreport.sl_ck = 0;
        app.report.pssqcrkmxreport.sl_cy_ck = 0;
        app.report.pssqcrkmxreport.sl_rk = 0;
        app.report.pssqcrkmxreport.sl_cy_rk = 0;
    });
    $("#pssqcrkmxreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
        
        if (json.outstr == 0) {
            //alert("没有数据");
            //json.data = {};
            //json.draw = draw;

            json.data = {};
            json.recordsTotal = 0;
            json.recordsFiltered = 0;
            app.report.pssqcrkmxreport.rowCallback();
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
    $('#pssqcrkmxreport #btn-search', _).click(function () {
        $(this).attr("disabled", "disabled");
        _button = $(this);
        setTimeout(function () {
            _button.removeAttr("disabled");
        }, 500)
        if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {
            app.report.pssqcrkmxreport.table_search();
        } else {
            $.DHB.message({ "content": "开始时间不能大于结束时间" });
        }
        
    });
    app.report.pssqcrkmxreport.table_search = function () {
        
        if (app.report.pssqcrkmxreport.tb) {
            app.report.pssqcrkmxreport.tb.ajax.reload();
            return false;
        } else {
            app.report.pssqcrkmxreport.table_draw();
        }


    }

}

