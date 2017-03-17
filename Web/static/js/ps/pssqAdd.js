app.pssq = app.pssq || {};

//app.pssq.vue = new Vue({
//    el: '#pssq_add',
//    data: {
//        message: 'Hello Vue!'
//    }
//})

//制单门店值改变事件
app.pssq.id_shop_change = function (e) {
    
    var options = {
        url: '/SearchCondition/GetSubShop',
        data: { id: $(e).val() },
        datatype: "json",
        type:"post",
        success: function (data) {
            console.log(data);
            
            
            if (data.Data.length > 0) {
                var li_str = '', options_str = '';
                for (var i = 0; i < data.Data.length; i++) {
                    var selected = '';
                    if (i == 0) {  selected = 'class="selected"'; $('#id_shop_sq', _).siblings('button').find('span').eq(0).html(data.Data[i].mc); }
                    li_str += '<li data-original-index="' + i + '"' + selected + '>';
                    li_str += '<a tabindex="0" class="" style="" data-tokens="null"><span class="text">' + data.Data[i].mc + '</span><span class="glyphicon glyphicon-ok check-mark"></span></a>';
                    li_str += '</li>';
                    options_str += '<option value="' + data.Data[i].id_shop + '">' + data.Data[i].mc + '</option>';
                }
                $('#id_shop_sq', _).siblings('.dropdown-menu').find('ul').html(li_str);
                $('#id_shop_sq', _).html(options_str);
                $('#id_shop_sq', _).find('options').eq(0).attr("selected", true);
                app.pssq.shop_change_clear();
            } else {
                $('#id_shop_sq', _).siblings('button').find('span').eq(0).html('');
                $('#id_shop_sq', _).siblings('.dropdown-menu').find('ul').html('');
                $('#id_shop_sq', _).html('');
                $('#id_shop_ck_mc', _).val('');
            }
            

        },
        error: function () {
            console.log("i");
        }
    }
    app.httpAjax.post(options);
}
//申请门店值改变事件
app.pssq.shop_change_clear = function () {
    app.pssq.query_psck_shop();
    app.pssq.removeAll();
    app.pssq.setresult(true);
}