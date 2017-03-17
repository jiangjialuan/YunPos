/**************************************************
** Copyright © 2015-2016 广州市超赢信息科技有限公司
** 创建日期:   2016-06-27
** 创 建 人:   YZQ
** 修改日期:   2016-08-26
** 修 改 人:   YZQ

** 描    述:   帮助类
** 文 件 名:   util.js
** 路    径:   ~/static/js/util.js
** 模    式:   严格模式(use strict)
*****************************************************/
;; if (typeof jQuery === 'undefined')
    console.error('util\'s JavaScript requires jQuery');

'use strict';

var cy = cy || {};
//http工具
cy.http = (function ($) {
    //判断是否有Spinner
    //var hasSpinner = !(typeof Spinner === 'undefined');
    var hasSpinner = !(typeof $.fn.spin_show === 'undefined');

    // Post数据
    var fun_Post = function (options) {
        //参数合并
        
        options = $.extend({
            async: true,                //true：异步请求
            cache: false,               //false：不缓存当前页
            crossDomain: false,         //false：同域请求
            data: {},                   //请求参数
            headers: {},                //请求头
            url: '',                    //请求地址，必填
            datatype: 'json',           //服务器返回的数据类型
            beforeSend: null,           //发送请求前
            callback: null,             //成功回调
            errcallback: null,          //异常回调
            complete: null              //请求完成后回调函数
        }, options || {});
        
        $.ajax({
            async: options.async,
            cache: options.cache,
            crossDomain: options.crossDomain,
            data: options.data,
            headers: options.headers,
            url: options.url,
            type: "POST",
            processData: true,
            datatype: options.datatype,
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            //timeout: 10000,
            beforeSend: function (XHR) {
                if (hasSpinner) { $('#dv_loading').spin_show(); }

                if (options.beforeSend && $.isFunction(options.beforeSend))
                    options.beforeSend(XHR);
            },
            success: function (data, textStatus, jqXHR) {
                
                var ret = null;
                if (data != undefined && jqXHR.status == 200 && $.trim(data).length > 0) {
                    if (options.datatype == 'json') {
                        //如果datatype == 'json'
                        var type = typeof (data);
                        if (type == "object")
                            ret = data;
                        else
                            ret = JSON.parse(data);
                    }
                    else {
                        ret = data;
                    }
                }

                if (options.callback && $.isFunction(options.callback))
                    options.callback(ret, textStatus, jqXHR);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.error(XMLHttpRequest);
                if (options.errcallback && $.isFunction(options.errcallback))
                    options.errcallback(XMLHttpRequest, textStatus, errorThrown)
            },
            complete: function (XHR, TS) {
                if (hasSpinner) { $('#dv_loading').spin_hide(); }

                if (options.complete && $.isFunction(options.complete))
                    options.complete(XHR, TS);
            }
        });
    };

    // Get数据
    var fun_Get = function (options) {
        //参数合并
        options = $.extend({
            async: true,                //true：异步请求
            cache: false,               //false：不缓存当前页
            crossDomain: false,         //false：同域请求
            data: {},                   //请求参数
            headers: {},                //请求头
            url: '',                    //请求地址，必填
            datatype: 'json',           //服务器返回的数据类型
            beforeSend: null,           //发送请求前
            callback: null,             //成功回调
            errcallback: null,          //异常回调
            complete: null              //请求完成后回调函数
        }, options || {});

        $.ajax({
            async: options.async,
            cache: options.cache,
            crossDomain: options.crossDomain,
            data: options.data,
            headers: options.headers,
            url: options.url,
            type: "GET",
            datatype: options.datatype,
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            beforeSend: function (XHR) {
                if (hasSpinner) { $('#dv_loading').spin_show(); }

                if (options.beforeSend && $.isFunction(options.beforeSend))
                    options.beforeSend(XHR);
            },
            success: function (data, textStatus, jqXHR) {
                if (options.datatype == 'json') {
                    var ret = null;
                    if (data != undefined && jqXHR.status == 200 && $.trim(data).length > 0) {
                        var type = typeof (data);
                        if (type == "object")
                            ret = data;
                        else
                            ret = JSON.parse(data);
                    }
                }
                else {
                    ret = data;
                }

                if (options.callback && $.isFunction(options.callback))
                    options.callback(ret, textStatus, jqXHR);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.error(XMLHttpRequest);
                if (options.errcallback && $.isFunction(options.errcallback))
                    options.errcallback(XMLHttpRequest, textStatus, errorThrown)
            },
            complete: function (XHR, TS) {
                if (hasSpinner) { $('#dv_loading').spin_hide(); }

                if (options.complete && $.isFunction(options.complete))
                    options.complete(XHR, TS);
            }
        });
    };

    // 获取Url参数
    var fun_GetUrlParam = function (name) {
        // 构造一个含有目标参数的正则表达式对象
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        // 匹配目标参数
        var r = window.location.search.substr(1).match(reg);
        if (r != null) {
            // var url = unescape(r[2]);
            var url = decodeURIComponent(r[2]);
            if ($.trim(window.location.hash).length > 1) {
                url += window.location.hash;
            }
            return url;
            // return unescape(r[2]).toLowerCase();
            // return decodeURIComponent(r[2]).toLowerCase();
        }
        else {
            return null; // 返回参数值
        }
    };

    // 获取Cookie
    var fun_GetCookie = function (name) {
        var arr = null;
        var reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
        if (arr = document.cookie.match(reg))
            return unescape($.trim(arr[2]));
        else
            return arr;
    };

    // 对外声明函数
    return {
        Post: fun_Post,
        Get: fun_Get,
        GetUrlParam: fun_GetUrlParam,
        GetCookie: fun_GetCookie,
    };
}(jQuery));

//通用验证
cy.validator = (function ($) {

}(jQuery));

//加解密
cy.dencrypt = (function ($) {
    var fun_SHA1 = function () {

    };

    var fun_XOR = function (str, code) {
        if ($.trim(code).length == 0)
            code = 69;

        var encoded = "";
        for (i = 0; i < str.length; i++) {
            var a = str.charCodeAt(i);
            var b = a ^ code;
            encoded = encoded + String.fromCharCode(b);
        }
        return encoded;
    };

    return {
        SHA1: fun_SHA1,             //SHA1哈希
        XOR: fun_XOR,               //异或
    };
}(jQuery));

//Cookie
cy.cookie = (function ($) {
    var fun_get = function (name, value) {
        var cookieValue = null;
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');       //分割
            for (var i = 0; i < cookies.length; i++) {
                var cookie = $.trim(cookies[i]);
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    //cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    cookieValue = cookie.substring(name.length + 1);
                    break;
                }
            }
        }
        return cookieValue;
    };

    var fun_set = function (name, value, options) {
        if (typeof value != 'undefined') {
            options = options || {};
            if (value === null) {
                value = '';
                options.expires = -1;
            }
            var expires = '';
            if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
                var date;
                if (typeof options.expires == 'number') {
                    date = new Date();
                    date.setTime(date.getTime() + (options.expires * 1000));
                } else {
                    date = options.expires;
                }
                expires = '; expires=' + date.toUTCString();
            }
            var path = options.path ? '; path=' + options.path : '';
            var domain = options.domain ? '; domain=' + options.domain : '';
            var secure = options.secure ? '; secure' : '';
            //document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
            document.cookie = [name, '=', value, expires, path, domain, secure].join('');
        }
    };

    return {
        get: fun_get,
        set: fun_set,
    };
}(jQuery));

//localStorage
cy.localStorage = (function () {
    var fun_GetItem = function (key) {
        var decode_data = window.localStorage.getItem(key);
        return decodeURIComponent(decode_data);
    };

    var fun_SetItem = function (key, data) {
        var encode_data = encodeURIComponent(data);
        window.localStorage.setItem(key, encode_data);
    };

    var fun_Clear = function () {
        window.localStorage.clear();
    };

    var fun_RemoveItem = function (key) {
        window.localStorage.removeItem(key);
    };

    return {
        get: fun_GetItem,
        set: fun_SetItem,
        clear: fun_Clear,
        remove: fun_RemoveItem,
    };
}());