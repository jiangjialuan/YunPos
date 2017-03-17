
if (app.report.shopysreport) {
    delete app.report.shopysreport.tb;
}
$(function () {
    $('div[contentID="report/shopysreport"]').attr({ controller: 'report', action: 'shopysreport' });
    app.c.public_data['report/shopysreport'] = app.c.public_data['report/shopysreport'] || {};
    app.c.public_data['report/shopysreport']['once'] = false;
    app.report = app.report || {};
});
$(function () {
    app.report.shopysreport.vue = new Vue({
        el: "#shopysreport",
        data: {
            selected: app.report.shopysreport.report_idshop,
            getshop: [],
            getuser: []

        },       
        methods: {
            getshopfunc: function () {
                app.request($.DHB.U('SearchCondition/GetShop'), {}, function (data) {
                    app.report.shopysreport.vue.getshop = data;
                }, function () {});
            },
            //getuserfunc: function () {
            //    app.request($.DHB.U('Gys/GetGysApi'), {}, function (data) {
            //        app.report.shopysreport.vue.getuser = data;
            //    }, function () { });
            //},
            
        }
    });
    //门店名称初始化
    app.report.shopysreport.vue.getshopfunc();
    //收银员初始化
    //app.report.shopysreport.vue.getuserfunc()
})



app.report.shopysreport_ready = function () {

    
    //表格初始化 "mc_shop","mc_gys","lxr","tel","je_yf"
    app.report.shopysreport.je_qm = 0;
    app.report.shopysreport.je_pre_qm = 0;
    app.report.shopysreport.table_draw = function () {
        
        
        jQuery.fn.dataTable.ext.errMode = "none";
        app.report.shopysreport.tb = $('#report-shopysreport-lis', _).DataTable({
            ajax: {
                url: $.DHB.U('ShopysReportReportApi'),
                type: 'post',
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                crossDomain: true,
                data: function (d) {
                    
                    var dt = {};
                    dt = $('.filter-form', _).serializeJson();
                    
                    //dt = app.report.shopysreport.vue.amDate;
                    ////console.log(dt);
                    dt.draw = d.draw;
                    dt.page = d.start / d.length;
                    dt.pageSize = d.length;
                    console.log(dt);
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
                
                { "data": "mc_shop", "sClass": "w-100" },
                { "data": "mc_kh", "sClass": "w-100" },
                { "data": "tel", "sClass": "text-center w-100" },
                { "data": "email", "sClass": "text-center width200" },
                {
                    "data": "je_qm", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.shopysreport.je_qm += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.shopysreport.digit);
                        }
                    }
                    
                },
                {
                    "data": "je_pre_qm", "sClass": "text-right w-100", "render": function (data, type, row) {
                        if (data == '' || data == null) {
                            return data;
                        } else {
                            app.report.shopysreport.je_pre_qm += parseFloat(data);
                            return parseFloat(data).toFixed(app.report.shopysreport.digit);
                        }
                    }
                }
                
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {                
                app.report.shopysreport.rowCallback();
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
        
        //app.report.shopysreport.fncallback();
        
    }
    app.report.shopysreport.rowCallback = function () {
        $('#shopysreport .je_qm', _).html(app.report.shopysreport.je_qm.toFixed(app.report.shopysreport.digit));
        $('#shopysreport .je_pre_qm', _).html(app.report.shopysreport.je_pre_qm.toFixed(app.report.shopysreport.digit));
    }
    //app.report.shopysreport.table_draw();
    //datatables设置
    
    $('#shopysreport  .table', _).on('preXhr.dt', function (e, settings, data) {
        app.report.shopysreport.je_qm = 0;
        app.report.shopysreport.je_pre_qm = 0;
    });
    $("#shopysreport .table", _).on('xhr.dt', function (e, settings, json, xhr) {
        
            if (json.outstr == 0) {
                //alert("没有数据");
                //json.data = {};
                //json.draw = draw;
                
                json.data = {};
                json.recordsTotal = 0;
                json.recordsFiltered = 0;
                app.report.shopysreport.rowCallback();
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
        $('#shopysreport #btn-search', _).click(function () {
            
            $(this).attr("disabled", "disabled");
            _button = $(this);
            setTimeout(function () {
                _button.removeAttr("disabled");
            }, 500)
            
            app.report.shopysreport.table_search();
            
        });
        app.report.shopysreport.table_search = function () {
            
            if (app.report.shopysreport.tb) {
                app.report.shopysreport.tb.ajax.reload();
                return false;
            } else {
                app.report.shopysreport.table_draw();
          }
           
            
        }
    
}

