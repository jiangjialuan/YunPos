
//
if (app.report.spxsranklistreport) {
    delete app.report.spxsranklistreport.tb;
}
$(function () {
    $('div[contentID="report/spxsranklistreport"]').attr({ controller: 'report', action: 'spxsranklistreport' });
    app.c.public_data['report/spxsranklistreport'] = app.c.public_data['report/spxsranklistreport'] || {};
    app.c.public_data['report/spxsranklistreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.spxsranklistreport.vue = new Vue({
        el: "#spxsranklistreport",
        data: {
            selected: app.report.spxsranklistreport.report_idshop,
            getshop: [],
            wayselect: "je_xs"

        },       
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    //console.log(data);
                    app.report.spxsranklistreport.vue.getshop = data;
                }, function () {});
            },
            
            
        }
    });
    //门店名称初始化
    app.report.spxsranklistreport.vue.getshopfunc();
    
})



app.report.spxsranklistreport_ready = function () {

    //日期插件初始化
    $('#spxsranklistreport #rq_begin,#spxsranklistreport #rq_end', _).click(function () {
        app.report.spxsranklistreport.wdatepicker_report_spxsranklistreport(this);
    });
    app.report.spxsranklistreport.wdatepicker_report_spxsranklistreport = function (el) {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd ',
        });

    }
    app.report.spxsranklistreport.first_Day = function () {
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
    app.report.spxsranklistreport.last_Day = function () {
        
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
    
    $('#spxsranklistreport #rq_end', _).val(app.report.spxsranklistreport.last_Day());
    $('#spxsranklistreport #rq_begin', _).val(app.report.spxsranklistreport.first_Day());
    //input显示商品分类，选择商品删除按钮
    $('#spxsranklistreport #shopfl_box,#spxsranklistreport #id_sp_box', _).hover(function () {
        $(this).parent().find('span:last()').show();
    }, function () {
        $(this).parent().find('span:last()').hide();
    });
    $('#spxsranklistreport .clear_button', _).click(function () {
        app.report.spxsranklistreport.report_spxsmx(this);
    });

    //商品分类弹框
    $('#spxsranklistreport #shopfl', _).click(function () {
        app.report.spxsranklistreport.spfl(this)
    });
    app.report.spxsranklistreport.spfl = function (el) {
        $.DHB.dialog({ 'title': '商品分类', 'url': $.DHB.U('promote/shopfl'), 'id': 'report-shopfl', 'confirm': app.report.spxsranklistreport.dialogCallBack_shopfl });
    }
    app.report.spxsranklistreport.dialogCallBack_shopfl = function () {
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

    app.report.spxsranklistreport.report_spxsmx = function (el) {
        var par = $(el).parent();
        par.find('input[type=text]').val('');
        par.find('input[type=hidden]').val('');

    }
    

    //替换非数字日期
    app.report.spxsranklistreport.renumdou = function (str) {
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
    app.report.spxsranklistreport.sl_xs = 0;
    app.report.spxsranklistreport.je_xs = 0;
    app.report.spxsranklistreport.je_ml = 0;
    app.report.spxsranklistreport.table_draw = function () {
        
        
        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.spxsranklistreport.tb = $('#report-spxsranklistreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('/report/spxsranklistreportapi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form', _).serializeJson();
                    dt.rankno = app.str_del_bank(dt.rankno);
                    //dt = app.report.spxsranklistreport.vue.amDate;
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
                    "data": "xh_rank", "sClass": "text-center w-40"
                },
                
                { "data": "mc_sp", "sClass": "text-center width200" },
                              
                { "data": "dw", "sClass": "text-center width55" },
                
                {
                    "data": "sl_xs", "sClass": "text-right w-100", "render": function (data, type, row) {
                        ////console.log(row);
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spxsranklistreport.sl_xs += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spxsranklistreport.digit);
                        }
                    }
                },
                
                {
                    "data": "je_xs", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spxsranklistreport.je_xs += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spxsranklistreport.digit);
                        }
                    }
                },
                {
                    "data": "je_ml", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.spxsranklistreport.je_ml += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.spxsranklistreport.digit);
                        }
                    }
                },
                
                {
                    "data": null, "sClass": "text-right w-70", "render": function (data, type, row) {
                        //"mll"=je_ml/je_xs
                        if (row["je_ml"] != "" && row["je_xs"] != undefined && row["je_ml"] != undefined && row["je_xs"] != "") {
                            return parseFloat(row["je_ml"] / row["je_xs"] * 100).toFixed(app.report.spxsranklistreport.digit) + '%';
                            
                        } else {
                            return ""
                        }
                        
                    }
                },
                { "data": "barcode", "sClass": "text-center w-100" },
                { "data": "mc_spfl", "sClass": "text-center w-100" }
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                app.report.spxsranklistreport.rowCallback();
                
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
        
        //app.report.spxsranklistreport.fncallback();
        
    }
    //app.report.spxsranklistreport.table_draw();
    app.report.spxsranklistreport.rowCallback = function () {
        $('#spxsranklistreport .sl_xs', _).html(app.report.spxsranklistreport.sl_xs.toFixed(app.report.spxsranklistreport.digit));
        $('#spxsranklistreport .je_xs', _).html(app.report.spxsranklistreport.je_xs.toFixed(app.report.spxsranklistreport.digit));
        $('#spxsranklistreport .je_ml', _).html(app.report.spxsranklistreport.je_ml.toFixed(app.report.spxsranklistreport.digit));
    }
    //datatables设置
    
    $('#spxsranklistreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        
        app.report.spxsranklistreport.sl_xs = 0;
        app.report.spxsranklistreport.je_xs = 0;
        app.report.spxsranklistreport.je_ml = 0;
    });
    $("#spxsranklistreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
            
            if (json.outstr == 0) {
                //alert("没有数据");
                //json.data = {};
                //json.draw = draw;
                
                json.data = {};
                json.recordsTotal = 0;
                json.recordsFiltered = 0;
                app.report.spxsranklistreport.rowCallback();
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
        $('#spxsranklistreport #btn-search', _).click(function () {
            $(this).attr("disabled", "disabled");
            _button = $(this);
            setTimeout(function () {
                _button.removeAttr("disabled");
            }, 500);
            if (app.endThanStart($('input[name=rq_begin]', _).val(), $('input[name=rq_end]', _).val())) {
                app.report.spxsranklistreport.table_search();
            } else {
                $.DHB.message({ "content": "开始时间不能大于结束时间" });
            }
            
        });
        app.report.spxsranklistreport.table_search = function () {
            
            if (app.report.spxsranklistreport.tb) {
                app.report.spxsranklistreport.tb.ajax.reload();
                return false;
            } else {
                app.report.spxsranklistreport.table_draw();
          }
           
            
        }
    
}

