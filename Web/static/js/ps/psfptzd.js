
app.psfptzd = app.psfptzd || {};
//点击返配申请单号触发事件
app.psfptzd.select_cktzd = function () {
    var id_shop = $(_ + '#id_shop_ck').val();
    $.DHB.dialog({ 'title': '选择返配申请单', 'url': $.DHB.U('psfpsq/searchlist?s_id_shop_sq=' + id_shop), 'id': 'dialog-psfpsq-search', 'confirm': app.psfptzd.select_d_callback });
}
//点击返配申请单号触发事件回调方法
app.psfptzd.select_d_callback = function (array) {
    $("#dh_origin").val(array.dh);
    $("#bm_djlx_origin").val(array.bm_djlx);
    $("#id_bill_origin").val(array.id);
    if (array.id) {
        //Post读取数据赋值 
        app.httpAjax.post({
            data: { id: array.id },
            headers: {},
            url: '/psfpsq/querfpsqdspmx',
            type: "POST",
            dataType: 'json',
            beforeSend: null,
            success: function (ret) {

                if (ret.Success) {
                    var shopsp = [];
                    if (ret.Data.Fpsq2List.length > 0) {
                        for (var item in ret.Data.Fpsq2List) {
                            var obj = ret.Data.Fpsq2List[item];
                            var shopsp_e = {};
                            shopsp_e.id_shopsp = obj.id_shopsp;
                            shopsp_e.id_kcsp = obj.id_kcsp;
                            shopsp_e.id_sp = obj.id_sp;
                            shopsp_e.barcode = obj.barcode;
                            shopsp_e.mc = obj.shopsp_name;
                            shopsp_e.dw = obj.dw;
                            shopsp_e.zhl = obj.zhl;
                            shopsp_e.dj_jh = obj.dj;
                            shopsp_e.dj_ps = obj.dj;
                            shopsp_e.sl = obj.sl;
                            shopsp_e.je = obj.je;
                            shopsp_e.bz = obj.bz;
                            shopsp.push(shopsp_e);
                        }
                    }
                    if (ret.Data.Fpsq1) { 
                        var index = $('#id_shop_ck', _).find('option[value="' + ret.Data.Fpsq1.id_shop + '"]').index();
                        $('#id_shop_ck', _).siblings('.dropdown-menu').find('ul li').eq(index).find('a').click();
                        $(_ + "#remark").val(ret.Data.Fpsq1.bz);
                    }
                    var jsonStr = JSON.stringify(shopsp);
                    app.psfptzd.removeAll();
                    app.psfptzd.dialogCallBackWork(jsonStr);
                    app.psfptzd.setresult(true);
                    $("button:contains('导入')", _).addClass('disabled').removeAttr('onclick');
                    $("button:contains('扫描')", _).addClass('disabled').removeAttr('onclick');
                    $("#psfptzd_shopsp_table a:contains('选择')", _).addClass('disabled').removeAttr('onclick');
                    $("button:contains('增加行')", _).addClass('disabled').removeAttr('onclick');
                    $('#id_shop_ck', _).attr('disabled', 'disabled');
                    $('#id_shop_rk_mc', _).attr('disabled', 'disabled');
                }
            },
            error: null,
            complete: null
        });

    }
}
//删除返配申请单号触发事件
app.psfptzd.clear_select_sqd = function (el) {
    var id_bill_origin = $(_ + "#id_bill_origin").val();
    if (id_bill_origin != '') {
        app.psfptzd.removeAll();
        app.psfptzd.setresult(true);
    }
    $(_ + "#dh_origin").val("");
    $(_ + "#bm_djlx_origin").val("");
    $(_ + "#id_bill_origin").val("");
    $(_ + "#remark").val("");
    $(_ + "#id_shop_rk").removeAttr("disabled");
    $(_ + "#id_shop_ck").removeAttr("disabled");
    $("button:contains('导入')", _).removeClass('disabled').attr('onclick', 'app.psfptzd.importin()');
    $("button:contains('扫描')", _).removeClass('disabled').attr('onclick', 'app.psfptzd.sm(this)');
    $("#psfptzd_shopsp_table a:contains('选择')", _).removeClass('disabled').attr('onclick', 'app.psfptzd.showshopsp();');
    $("button:contains('增加行')", _).removeClass('disabled').attr('onclick', 'app.psfptzd.addshopsp_row(this)');
    $('#id_shop_ck', _).removeAttr('disabled');
    $('#id_shop_rk_mc', _).removeAttr('disabled');

}
//退货门店值改变触发事件
app.psfptzd.shop_change_clear = function () {
    app.psfptzd.query_psrk_shop();
    app.psfptzd.removeAll();
    app.psfptzd.setresult(true);
}
app.psfptzd.query_psrk_shop = function () {
    
    var id_shop_ck = $(_ + "#id_shop_ck").val();
    if (id_shop_ck != '') {
        cy.http.Post({
            url: '/SearchCondition/GetPsShop',
            data: { id: id_shop_ck },
            beforeSend: function () {
            },
            callback: function (ret) {
                if (ret.Success) {
                    $(_ + "#id_shop_rk_mc").val(ret.Data.mc);
                    $(_ + "#id_shop_rk").val(ret.Data.id_shop);
                } else {
                    $(_ + "#id_shop_rk_mc").val("");
                    $(_ + "#id_shop_rk").val("");
                    $.DHB.message({ 'content': ret.Data.Message[0], 'time': 4000, 'type': 's' });
                }
            },
            complete: function () { }
        });
    } else {
        $('#id_shop_rk_mc', _).val('');
        $('#id_shop_rk', _).val('');
        
    }
    
}
if (app.psfptzd.option == 'edit' || app.psfptzd.option == 'copy') {
    app.psfptzd.query_psrk_shop();
}