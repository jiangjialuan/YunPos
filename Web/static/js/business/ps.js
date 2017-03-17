app.ps = app.ps || {};
//根据原单号判断用户是否可以执行此操作(true:可执行,false:不可执行)
app.ps.is_can_exec_op_by_dh = function (_, inputId) {
    
    var val = $(_ + "#" + inputId).val();
    if (val && val.length > 1) {
        $.DHB.message({ 'content': '有引单,不能执行此操作', 'time': 1000, 'type': 'i' });
        return false;
    }
    return true;
}
//商品选择弹框
app.ps.select_sp_dialog = function (data) { 
    $.DHB.dialog({
        'title': '选择商品',
        'url': data.url,
        'id': 'dialog-shopsp-search',
        'confirm': data.callback
    });
}
//单位选择下拉
app.ps.dw_select_onclick = function (data) { 
    app.httpAjax.post({
        data: data.data,
        headers: {},
        url: data.url,
        type: "POST",
        dataType: 'json',
        beforeSend: null,
        success: function (ret) {
            data.callback(ret);
        },
        error: null,
        complete: null
    });

}
