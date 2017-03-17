
//
if (app.report.spjhhzreport) {
    delete app.report.spjhhzreport.tb;
}
$(function () {
    $('div[contentID="report/spjhhzreport"]').attr({ controller: 'report', action: 'spjhhzreport' });
    app.c.public_data['report/spjhhzreport'] = app.c.public_data['report/spjhhzreport'] || {};
    app.c.public_data['report/spjhhzreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.spjhhzreport.vue = new Vue({
        el: "#spjhhzreport",
        data: {
            selected: app.report.spjhhzreport.report_idshop,
            getshop: [],
            //getuser: []

        },
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.spjhhzreport.vue.getshop = data;
                }, function () { });
            },
           

        }
    });
    //门店名称初始化
    app.report.spjhhzreport.vue.getshopfunc();
})



app.report.spjhhzreport_ready = function () {

    //日期插件初始化
    $('#spjhhzreport #rq_begin,#spjhhzreport #rq_end', _).click(function () {
        app.report.spjhhzreport.wdatepicker_report_spjhhzreport(this);
    });
    app.report.spjhhzreport.wdatepicker_report_spjhhzreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.spjhhzreport.first_Day = function () {
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
    app.report.spjhhzreport.last_Day = function () {

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
    app.report.spjhhzreport.last_Day()
    $('#spjhhzreport #rq_end', _).val(app.report.spjhhzreport.last_Day());
    $('#spjhhzreport #rq_begin', _).val(app.report.spjhhzreport.first_Day());
    //input显示商品分类，选择商品删除按钮
    $('#spjhhzreport #shopfl_box,#spjhhzreport #id_sp_box', _).hover(function () {
        $(this).parent().find('span:last()').show();
    }, function () {
        $(this).parent().find('span:last()').hide();
    });
    $('#spjhhzreport .clear_button', _).click(function () {
        app.report.spjhhzreport.report_spxsmx(this);
    });

    //商品分类弹框
    $('#spjhhzreport #shopfl', _).click(function () {
        app.report.spjhhzreport.spfl(this)
    });
    app.report.spjhhzreport.spfl = function (el) {
        $.DHB.dialog({ 'title': '商品分类', 'url': $.DHB.U('promote/shopfl'), 'id': 'report-shopfl', 'confirm': app.report.spjhhzreport.dialogCallBack_shopfl });
    }
    app.report.spjhhzreport.dialogCallBack_shopfl = function () {
        var fldata = {}, str = "", str_id = "";
        fldata = JSON.parse($('#fldata', _).val());
        for (var i = 0; i < fldata.length; i++) {
            if (i == fldata.length - 1) {
                str += fldata[i].text;
                str_id += fldata[i].id;
            } else {
                str += fldata[i].text + ",";
                str_id += fldata[i].id + ",";
            }

        }
        $('#shopfl',_).val(str);
        //$('#fldata').attr('flstr', str_id);
        $('#fldata',_).val(str_id);
        $.DHB.dialog({ 'id': 'report-shopfl', 'action': 'destroy' });
    }
    app.report.spjhhzreport.report_spxsmx = function (el) {
        var par = $(el).parent();
        par.find('input[type=text]').val('');
        par.find('input[type=hidden]').val('');
    }

    
    


    //替换非数字日期
    app.report.spjhhzreport.renumdou = function (str) {
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
    app.report.spjhhzreport.sl_jh = 0;
    app.report.spjhhzreport.je_jh = 0;
    app.report.spjhhzreport.sl_th = 0;
    app.report.spjhhzreport.je_th = 0;
    app.report.spjhhzreport.sl_total = 0;
    app.report.spjhhzreport.je_total = 0;
    app.report.spjhhzreport.table_draw = function () {
        

        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.spjhhzreport.tb = $('#report-spjhhzreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/SpjhhzreportApi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form', _).serializeJson();
                    //dt = app.report.spjhhzreport.vue.amDate;
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
                { "data": "null", "sClass": "text-center w-40" },
                { "data": "mc_shop", "sClass": "width200" },
                { "data": "mc_sp", "sClass": "width200" },
                { "data": "dw", "sClass": "text-center" },
                {
                    "data": "sl_jh", "sClass": "text-right w-100", "render": function (data, type, row) {

                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spjhhzreport.sl_jh += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spjhhzreport.digit);
                        }

                    }
                },
                {
                    "data": "je_jh", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spjhhzreport.je_jh += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spjhhzreport.digit);
                        }
                    }
                },
                { "data": "dj_jh_avg", "sClass": "text-right w-100" },
                {
                    "data": "sl_th", "sClass": "text-right", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spjhhzreport.sl_th += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spjhhzreport.digit);
                        }
                    }
                },
                {
                    "data": "je_th", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spjhhzreport.je_th += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spjhhzreport.digit);
                        }
                    }
                },
                {
                    "data": "sl_total", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spjhhzreport.sl_total += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spjhhzreport.digit);
                        }
                    }
                },
                {
                    "data": "je_total", "sClass": "text-right", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spjhhzreport.je_total += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spjhhzreport.digit);
                        }
                    }
                },
                { "data": "barcode", "sClass": "text-center w-100"},
                { "data": "mc_spfl", "sClass": "text-center w-100" }
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                app.report.spjhhzreport.rowCallback();
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

        //app.report.spjhhzreport.fncallback();

    }
    //app.report.spjhhzreport.table_draw();
    app.report.spjhhzreport.rowCallback = function () {
        $('#spjhhzreport .sl_jh', _).html(app.report.spjhhzreport.sl_jh.toFixed(app.report.spjhhzreport.digit));
        $('#spjhhzreport .je_jh', _).html(app.report.spjhhzreport.je_jh.toFixed(app.report.spjhhzreport.digit));
        $('#spjhhzreport .sl_th', _).html(app.report.spjhhzreport.sl_th.toFixed(app.report.spjhhzreport.digit));
        $('#spjhhzreport .je_th', _).html(app.report.spjhhzreport.je_th.toFixed(app.report.spjhhzreport.digit));
        $('#spjhhzreport .sl_total', _).html(app.report.spjhhzreport.sl_total.toFixed(app.report.spjhhzreport.digit));
        $('#spjhhzreport .je_total', _).html(app.report.spjhhzreport.je_total.toFixed(app.report.spjhhzreport.digit));
    }
    //datatables设置

    $('#spjhhzreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.spjhhzreport.sl_jh = 0;
        app.report.spjhhzreport.je_jh = 0;
        app.report.spjhhzreport.sl_th = 0;
        app.report.spjhhzreport.je_th = 0;
        app.report.spjhhzreport.sl_total = 0;
        app.report.spjhhzreport.je_total = 0;
    });
    $("#spjhhzreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
        
        if (json.outstr == 0) {
            //alert("没有数据");
            //json.data = {};
            //json.draw = draw;

            json.data = {};
            json.recordsTotal = 0;
            json.recordsFiltered = 0;
            app.report.spjhhzreport.rowCallback();
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
    $('#spjhhzreport #btn-search', _).click(function () {
        $(this).attr("disabled", "disabled");
        _button = $(this);
        setTimeout(function () {
            _button.removeAttr("disabled");
        }, 500);
        if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {
            app.report.spjhhzreport.table_search();
        } else {
            $.DHB.message({ "content": "开始时间不能大于结束时间" });
        }
        
    });
    app.report.spjhhzreport.table_search = function () {
        
        if (app.report.spjhhzreport.tb) {
            app.report.spjhhzreport.tb.ajax.reload();
            return false;
        } else {
            app.report.spjhhzreport.table_draw();
        }


    }

}







