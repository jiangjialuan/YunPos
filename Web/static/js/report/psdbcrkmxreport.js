
//
if (app.report.psdbcrkmxreport) {
    delete app.report.psdbcrkmxreport.tb;
}
$(function () {
    $('div[contentID="report/psdbcrkmxreport"]').attr({ controller: 'report', action: 'psdbcrkmxreport' });
    app.c.public_data['report/psdbcrkmxreport'] = app.c.public_data['report/psdbcrkmxreport'] || {};
    app.c.public_data['report/psdbcrkmxreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.psdbcrkmxreport.vue = new Vue({
        el: "#psdbcrkmxreport",
        data: {
            selected: app.report.psdbcrkmxreport.report_idshop,
            getshop: [],
            getshopAll: [],
            //getuser: []

        },
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.psdbcrkmxreport.vue.getshop = data;
                }, function () { });
            },
            getAllshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), { type: "all" }, function (data) {
                    app.report.psdbcrkmxreport.vue.getshopAll = data;
                }, function () { });
            }
            

        }
    });
    //门店名称初始化
    app.report.psdbcrkmxreport.vue.getshopfunc();
    app.report.psdbcrkmxreport.vue.getAllshopfunc();

})



app.report.psdbcrkmxreport_ready = function () {

    $('#id_shop_rk', _).focus(function () {
        //if ($('#id_shop_ck', _).val() != '' && $('#id_shop_ck', _).val() != undefined) {
            $('#id_shop_rk', _).find('option[value=' + $('#id_shop_ck', _).val() + ']').hide().siblings('option').show();
        //}
        
    })

    //日期插件初始化
    $('#psdbcrkmxreport #rq_begin,#psdbcrkmxreport #rq_end', _).click(function () {
        app.report.psdbcrkmxreport.wdatepicker_report_psdbcrkmxreport(this);
    });
    app.report.psdbcrkmxreport.wdatepicker_report_psdbcrkmxreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.psdbcrkmxreport.first_Day = function () {
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
    app.report.psdbcrkmxreport.last_Day = function () {

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
    app.report.psdbcrkmxreport.last_Day()
    $('#psdbcrkmxreport #rq_end', _).val(app.report.psdbcrkmxreport.last_Day());
    $('#psdbcrkmxreport #rq_begin', _).val(app.report.psdbcrkmxreport.first_Day());
    
    //替换非数字日期
    app.report.psdbcrkmxreport.renumdou = function (str) {
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
    app.report.psdbcrkmxreport.sl_ck = 0;
    app.report.psdbcrkmxreport.je = 0;
    app.report.psdbcrkmxreport.sl_rk = 0;
    app.report.psdbcrkmxreport.sl_cy = 0;
    app.report.psdbcrkmxreport.table_draw = function () {
        

        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.psdbcrkmxreport.tb = $('#report-psdbcrkmxreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/psdbcrkmxreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form', _).serializeJson();
                    dt.dh_ck = app.str_del_bank(dt.dh_ck);
                    //dt = app.report.psdbcrkmxreport.vue.amDate;
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
                        var dd = parseInt(app.report.psdbcrkmxreport.renumdou(data));
                        return (new Date(parseInt(dd)).Format("yyyy-MM-dd"));;
                    }
                },
                {
                    "data": "dh_ck", "sClass": "text-center w-124"
                },
                { "data": "mc_shop_ck", "sClass": "text-center w-100" },
                //{ "data": "dh" },
                { "data": "mc_shop_rk", "sClass": "text-center w-100" },
                { "data": "mc_sp", "sClass": "width200" },
                { "data": "dw", "sClass": "text-center w-70" },
              
                {
                    "data": "sl_ck", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.psdbcrkmxreport.sl_ck += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.psdbcrkmxreport.digit);
                        }
                    }
                },
                {
                    "data": "dj", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                          
                            return parseFloat(data).toFixed(app.report.psdbcrkmxreport.digit);
                        }
                    }
                },
                {
                    "data": "je", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.psdbcrkmxreport.je += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.psdbcrkmxreport.digit);
                        }
                    }
                },
                {
                    "data": "dh_rk", "sClass": "text-center w-124"
                },
                {
                    "data": "sl_rk", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.psdbcrkmxreport.sl_rk += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.psdbcrkmxreport.digit);
                        }
                    }
                },

                {
                    "data": null, "sClass": "text-right w-100", "render": function (data, type, row) {
                        //sl_cy=sl_ck-sl_rk
                        if (row["sl_ck"] != undefined && row["sl_ck"] != '' && row["sl_rk"] != undefined && row["sl_rk"] != '') {
                            return parseFloat(row["sl_ck"] - row["sl_rk"]).toFixed(app.report.psdbcrkmxreport.digit);
                        } else {
                            return ''
                        }
                        
                    }
                },
                { "data": "barcode", "sClass": "text-center w-100" },
                { "data": "mc_spfl", "sClass": "text-center w-100" }
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
               
                app.report.psdbcrkmxreport.rowCallback();
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

        //app.report.psdbcrkmxreport.fncallback();

    }
    //app.report.psdbcrkmxreport.table_draw();
    app.report.psdbcrkmxreport.rowCallback = function () {
        $('#psdbcrkmxreport .sl_ck', _).html(app.report.psdbcrkmxreport.sl_ck.toFixed(app.report.psdbcrkmxreport.digit));
        $('#psdbcrkmxreport .je', _).html(app.report.psdbcrkmxreport.je.toFixed(app.report.psdbcrkmxreport.digit));
        $('#psdbcrkmxreport .sl_rk', _).html(app.report.psdbcrkmxreport.sl_rk.toFixed(app.report.psdbcrkmxreport.digit));
        $('#psdbcrkmxreport .sl_cy', _).html(app.report.psdbcrkmxreport.sl_cy.toFixed(app.report.psdbcrkmxreport.digit));
    }
    //datatables设置

    $('#psdbcrkmxreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.psdbcrkmxreport.sl_ck = 0;
        app.report.psdbcrkmxreport.je = 0;
        app.report.psdbcrkmxreport.sl_rk = 0;
        app.report.psdbcrkmxreport.sl_cy = 0;
    });
    $("#psdbcrkmxreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
        
        if (json.outstr == 0) {
            //alert("没有数据");
            //json.data = {};
            //json.draw = draw;

            json.data = {};
            json.recordsTotal = 0;
            json.recordsFiltered = 0;
            app.report.psdbcrkmxreport.rowCallback();
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
    $('#psdbcrkmxreport #btn-search', _).click(function () {
        $(this).attr("disabled", "disabled");
        _button = $(this);
        setTimeout(function () {
            _button.removeAttr("disabled");
        }, 500)
        if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {
            app.report.psdbcrkmxreport.table_search();
        } else {
            $.DHB.message({ "content": "开始时间不能大于结束时间" });
        }
        
    });
    app.report.psdbcrkmxreport.table_search = function () {
        
        if (app.report.psdbcrkmxreport.tb) {
            app.report.psdbcrkmxreport.tb.ajax.reload();
            return false;
        } else {
            app.report.psdbcrkmxreport.table_draw();
        }


    }

}

