app.pssq = app.pssq || {};

app.pssq.SearchList_vue=new Vue({
    el: '#pssqSearchList',
    data: {
        getshop: [],
        rk_select: app.pssq.SearchList_rkmd,
        pszx: app.pssq.SearchList_pszx,

    },
    methods: {
        getshopFun: function () {
            _this = this;
            app.httpAjax.post({
                data: { id: _this.pszx },
                headers: {},
                url: '/SearchCondition/GetPzxSubShop',
                type: "POST",
                dataType: 'json',
                beforeSend: null,
                success: function (res) {
                    if (res.Success && res.Data.length > 0) {
                        app.pssq.SearchList_vue.getshop = res.Data;
                    }
                    else {
                        app.pssq.SearchList_vue.getshop = [];
                    }
                },
                error: null,
                complete: null
            });
        }
    }

});
app.pssq.SearchList_vue.getshopFun();
//配送中心发生改变触发事件
app.pssq.psck_pszx_select = function (obj) {
    var id_shop = $(obj).val();
    app.httpAjax.post({
        data: { id: id_shop },
        headers: {},
        url: '/SearchCondition/GetPzxSubShop',
        type: "POST",
        dataType: 'json',
        beforeSend: null,
        success: function (res) {
            
            if (res.Success && res.Data.length > 0) {
                app.pssq.SearchList_vue.getshop = res.Data;                
            }
            else {
                app.pssq.SearchList_vue.getshop = [];
                app.pssq.SearchList_vue.rk_select = '';
            }
        },
        error: null,
        complete: null
    });

}