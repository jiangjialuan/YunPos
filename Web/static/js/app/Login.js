'use strict';

function refreshCode(node) {
    var time = new Date().getTime();
    node.src = "/utility/vaildcode" + '?t=' + time;
}

var loginCount = 0;




$(function () {
    $('#form1 .btn_img_code').click();
    $('body').on('click', '.dialog_lose,.login_', function () {
        if ($(this).hasClass('dialog_lose')) {
            $(this).parents('#to_login').hide();
        }
        else if ($(this).hasClass('login_')) {
            $('#to_login').show();
        }
    })



    var timer = null;
    var $username1 = $('#form1 .u-name'),
        $password1 = $('#form1 .u-password'),
        $img_code1 = $('#form1 .recognition-code'),
        $warnArea1 = $('#form1 .warn-info'),
        $qynumber = $('#form2 .u-qynumber'),
        $username2 = $('#form2 .u-name'),
        $password2 = $('#form2 .u-password'),
        $img_code2 = $('#form2 .recognition-code'),
        $warnArea2 = $('#form2 .warn-info'),
        $span_is_remember = $("#span_is_remember");
    var userNameValue1 = getCookieValue("username1");
    var passwordValue1 = getCookieValue("pwd1");
    var userNameValue2 = getCookieValue("username2");
    var passwordValue2 = getCookieValue("pwd2");
    var qynumber = getCookieValue("qynumber");
    var isRememberValue = getCookieValue("isRemember");


    //绑定Cookie值
    // if ($('.loginItem .active').index() == 0) {
    $username1.val(userNameValue1);
    $password1.val(cy.dencrypt.XOR(passwordValue1, 69));
    // } else {
    $username2.val(userNameValue2);
    $password2.val(cy.dencrypt.XOR(passwordValue2, 69));
    $qynumber.val(qynumber);
    // }
    
    
    if (isRememberValue === "true") {
        $span_is_remember.addClass("add-remember");
    } else {
        $span_is_remember.removeClass("add-remember");
    }
    //记住帐号
    $("body").on("click", ".control-box .i-checks", function () {
        if ($(this).find(".remember").hasClass("add-remember")) {
            $(this).find(".remember").removeClass("add-remember")
        }
        else {
            $(this).find(".remember").addClass("add-remember")
        }
    }).on('keypress', ".control-box .i-checks", function (e) {
        e = e || event;
        if (e.charCode === 32 || e.keyCode === 32) {
            if ($(this).find(".remember").hasClass("add-remember")) {
                $(this).find(".remember").removeClass("add-remember")
            }
            else {
                $(this).find(".remember").addClass("add-remember")
            }
        }
        else if (e.charCode === 13 || e.keyCode === 13) {
            $('.login-btn').click();
        }
    });
    $('.code_text').click(function () {
        $(this).siblings('.code').find('img').click();
    });

    //登录
    $('.login-btn').on('click', function () {
        
        var _this = this;
        if ($('.loginItem .active').index() == 0) {
            var userName1 = $.trim($username1.val());
            var userPassword1 = $.trim($password1.val());
            var img_code1 = $.trim($img_code1.val());
            var $warnArea = $warnArea1;
            var usernamets = "手机号不能为空";
        } else {
            var userName1 = $.trim($username2.val());
            var userPassword1 = $.trim($password2.val());
            var img_code1 = $.trim($img_code2.val());
            var $warnArea = $warnArea2;
            var usernamets = "帐号不能为空";
            var qynumber = $.trim($qynumber.val());
            if (!qynumber) {
                $qynumber.focus();
                $warnArea.html("企业号不能为空").removeClass('hide').show();
                return;
            }
        }
        
        var isRemember = $('.remember').hasClass('add-remember');
        var recognitionReg = /[0-9]{4}/g;
        
        if (!userName1) {
            $username1.focus();
            $warnArea.html(usernamets).removeClass('hide').show();
        }
        else if (!userPassword1) {
            $password1.focus();
            $warnArea.html('密码不能为空').removeClass('hide').show();
        } 
        else if (!recognitionReg.test(img_code1) && loginCount >= 3) {
            
                $img_code1.focus();
                $warnArea.html('请输入正确的验证码').removeClass('hide').show();
                
            
        } 
        else {
            var smData = {};            
            smData.password = userPassword1;
            smData.vaildcode = img_code1;
            if ($('.loginItem .active').index() == 0) {
                smData.username = userName1;
                smData.loginType = "quick";
            } else {
                smData.username = $('.u-qynumber').val() + ":" + userName1;
            }


            var options = {
                url: '/account/login',
                data: smData,
                beforeSend: function (XHR) {
                    $(_this).attr('disabled', 'disabled').val('正在登录...');
                },
                callback: function (ret) {
                    if (ret && ret.Success) {
                        if (ret.Level == '2') {
                            //如果提示 将要过期
                            var msg = '';
                            $.each(ret.Message, function (i, obj) {
                                msg += obj;
                                if (i != ret.Message.length - 1)
                                    msg += '<br />';
                            });
                            
                            $("#MsgLogin").html(msg);
                            $('.login-btn').removeAttr('disabled').val('登 录');

                            if ($("#div_right").css('display') != 'block') {
                                $("#div_right").css('display', 'block');
                            }

                            $("#a_buy_service").attr('href', ret.Data);

                            $("#to_login").show();
                            if ($('.loginItem .active').index() == 0) {
                                setCookie("username1", userName1, 24 * 7, "/");
                                if (isRemember) {
                                    setCookie("pwd1", cy.dencrypt.XOR(userPassword1, 69), 24 * 7, "/");
                                    setCookie("isRemember", "true", 24 * 7, "/");
                                } else {
                                    setCookie("pwd1", "", 24 * 7, "/");
                                    setCookie("isRemember", "false", 24 * 7, "/");
                                }
                            } else {
                                setCookie("username2", userName1, 24 * 7, "/");
                                setCookie("qynumber", $('.u-qynumber').val(), 24 * 7, "/");
                                if (isRemember) {
                                    setCookie("pwd2", cy.dencrypt.XOR(userPassword1, 69), 24 * 7, "/");
                                    setCookie("isRemember", "true", 24 * 7, "/");
                                } else {
                                    setCookie("pwd2", "", 24 * 7, "/");
                                    setCookie("isRemember", "false", 24 * 7, "/");
                                }
                            }
                            
                            var history = '[{},"index"]';
                            cy.cookie.set('dhb_manager/home_menutab', history, {
                                expires: 86400, path: '/'
                            });
                            loginCount = 0;
                        }
                        else {
                            if ($('.loginItem .active').index() == 0) {
                                setCookie("username1", userName1, 24 * 7, "/");
                                if (isRemember) {
                                    setCookie("pwd1", cy.dencrypt.XOR(userPassword1, 69), 24 * 7, "/");
                                    setCookie("isRemember", "true", 24 * 7, "/");
                                } else {
                                    setCookie("pwd1", "", 24 * 7, "/");
                                    setCookie("isRemember", "false", 24 * 7, "/");
                                }
                            } else {
                                setCookie("qynumber", $('.u-qynumber').val(), 24 * 7, "/");
                                setCookie("username2", userName1, 24 * 7, "/");
                                if (isRemember) {
                                    setCookie("pwd2", cy.dencrypt.XOR(userPassword1, 69), 24 * 7, "/");
                                    setCookie("isRemember", "true", 24 * 7, "/");
                                } else {
                                    setCookie("pwd2", "", 24 * 7, "/");
                                    setCookie("isRemember", "false", 24 * 7, "/");
                                }
                            }
                           
                            
                            var history = '[{},"index"]';
                            cy.cookie.set('dhb_manager/home_menutab', history, {
                                expires: 86400, path: '/'
                            });
                            loginCount = 0;
                            location.replace(ret.Data);
                        }
                    }
                    else {

                        //还原登录按钮 提示错误信息
                        $('.login-btn').removeAttr('disabled').val('登 录');

               

                        if (ret.Level == '2') {
                            //服务门店超过总门店数 需要管理员角色设置
                            var msg = '';
                            $.each(ret.Message, function (i, obj) {
                                msg += obj;
                                if (i != ret.Message.length - 1)
                                    msg += '<br />';
                            });
                            alert(msg);

                        }
                        else if (ret.Level == '4')
                        {
                            alert('登录失败！您的服务信息已超出购买数量！请先处理要启用的门店！');
                            //需要设置门店
                            window.location.href = '/Account/Set';
                        }
                        else if (ret.Level == '5') {
                            //需要购买服务
                            var msg = '';
                            $.each(ret.Message, function (i, obj) {
                                msg += obj;
                                if (i != ret.Message.length - 1)
                                    msg += '<br />';
                            });

                            $("#MsgLogin").html(msg);
                            $('.login-btn').removeAttr('disabled').val('登 录');

                            if ($("#div_right").css('display') != 'none') {
                                $("#div_right").css('display', 'none');
                            }

                            $("#a_buy_service").attr('href', ret.Data);

                            $("#to_login").show();
                        }
                        else {
                            
                            //不是服务原因的登录失败 验证码的处理
                            if ($('.loginItem .active').index() == 0) {
                                $('#form1 .btn_img_code').click();
                                $('#form1 .recognition-code').val('');
                                loginCount = loginCount + 1;
                                if (loginCount >= 3) {
                                    $("#form2 #div_img_code2").show();
                                    $("#form1 #div_img_code").show();
                                    if ($('.loginItem .active').index() == 0) {
                                        
                                        $("#form1 #div_img_code img").click();
                                    } else {
                                        
                                        $("#form2 #div_img_code2 img").click();
                                    }
                                }
                                var msg = '';
                                $.each(ret.Message, function (i, obj) {
                                    msg += obj;
                                    if (i != ret.Message.length - 1)
                                        msg += '<br />';
                                });
                                $warnArea.html(msg).removeClass('hide').show();
                            } else {
                                $('#form2 .btn_img_code').click();
                                $('#form2 .recognition-code').val('');
                                loginCount = loginCount + 1;
                                if (loginCount >= 3) {
                                    $("#form1 #div_img_code").show();
                                    $("#form2 #div_img_code2").show();
                                    if ($('.loginItem .active').index() == 0) {
                                        
                                        $("#form1 #div_img_code img").click();
                                    } else {
                                        
                                        $("#form2 #div_img_code2 img").click();
                                    }
                                    
                                    
                                }
                                var msg = '';
                                $.each(ret.Message, function (i, obj) {
                                    msg += obj;
                                    if (i != ret.Message.length - 1)
                                        msg += '<br />';
                                });
                                $warnArea2.html(msg).removeClass('hide').show();
                            }
                            
                        }
                    }
                }
            };
            cy.http.Post(options);
        }
    });


    $username1.keypress(function (e) {
        
        $warnArea1.html('').removeClass('hide').hide();
        e = e || event;
        if (e.charCode === 13 || e.keyCode === 13) {
            //$('.login-btn').trigger('click');
            $password1.focus();
        }
    });
    $password1.focus(function(){
     this.select(); 
    });
    $password1.keypress(function (e) {
        $warnArea1.html('').removeClass('hide').hide();
        e = e || event;
        if (e.charCode === 13 || e.keyCode === 13) {
            //$('.login-btn').trigger('click');
            if (loginCount >= 3) {
                $img_code1.focus();
            } else {
                $('.login-btn').click();
            }
        }
    });

    $img_code1.keypress(function (e) {
        $warnArea1.html('').removeClass('hide').hide();
        e = e || event;
        if (e.charCode === 13 || e.keyCode === 13) {
            $('.login-btn').click();
        }
    });
    $qynumber.keypress(function (e) {
        
        $warnArea2.html('').removeClass('hide').hide();
        e = e || event;
        if (e.charCode === 13 || e.keyCode === 13) {
            //$('.login-btn').trigger('click');
            $username2.focus();
        }
    });
    $username2.focus(function () {
        this.select();
    });
    $username2.keypress(function (e) {
        $warnArea2.html('').removeClass('hide').hide();
        e = e || event;
        if (e.charCode === 13 || e.keyCode === 13) {
            //$('.login-btn').trigger('click');
            $password2.focus();
        }
    });
    $password2.focus(function () {
        this.select();
    });
    $password2.keypress(function (e) {
        $warnArea2.html('').removeClass('hide').hide();
        e = e || event;
        if (e.charCode === 13 || e.keyCode === 13) {
            //$('.login-btn').trigger('click');
            if (loginCount >= 3) {
                $img_code2.focus();
            } else {
                $('.login-btn').click();
            }
        }
    });

    $img_code2.keypress(function (e) {
        $warnArea2.html('').removeClass('hide').hide();
        e = e || event;
        if (e.charCode === 13 || e.keyCode === 13) {
            $('.login-btn').click();
        }
    });
    $('.loginItem div').click(function () {
        
        var index = $(this).index();
        $(this).addClass('active').siblings().removeClass('active');
        if (index == 0) {
            
            $('#form1').show();
            $("#form1 #div_img_code img").click();
            $('#form2').hide();
        } else {
            $('#form2').show();
            $("#form2 #div_img_code2 img").click();
            $('#form1').hide();
        }
    });

    $(".app-load-box").hover(function () {
        var self = this;
        timer = setTimeout(function () {
            $(self).children(".app-img").removeClass("hide").addClass("show");
        }, 200);
    }, function () {
        var _self = this;
        if (timer) {
            clearTimeout(timer);
        };
        $(_self).children(".app-img").removeClass("show").addClass("hide");
    });

    $(".weixin-load-box").hover(function () {
        var self = this;
        timer = setTimeout(function () {
            $(self).children(".weixin-img").removeClass("hide").addClass("show");
        }, 200);
    }, function () {
        var _self = this;
        if (timer) {
            clearTimeout(timer);
        };
        $(_self).children(".weixin-img").removeClass("show").addClass("hide");
    });


    var initHeight = $(window).height() - $(".header").height() - $(".footer").height()-100;
    $('.con-row').height(initHeight);
    $(window).resize(function () {
        var $winH = $(window).height() - $(".header").height() - $(".footer").height()-100;
        $(window).height() > 830 ? $('.con-row').height($winH) : initHeight;
    });

    $username1.focus();
});

//设置cookie值
function setCookie(name, value, hours, path) {
    //var name = escape(name);
    //var value = escape(value);
    var name = encodeURIComponent(name);
    var value = encodeURIComponent(value);
    var expires = new Date();
    expires.setTime(expires.getTime() + hours * 3600000);
    path = path == "" ? "" : ";path=" + path;
    var _expires = (typeof hours) == "string" ? "" : ";expires=" + expires.toUTCString();
    document.cookie = name + "=" + value + _expires + path;
}

//获取cookie值
function getCookieValue(name) {
    //var name = escape(name);
    var name = decodeURIComponent(name);

    //读cookie属性，这将返回文档的所有cookie
    var allcookies = document.cookie;
    //查找名为name的cookie的开始位置
    name += "=";
    var pos = allcookies.indexOf(name);
    //如果找到了具有该名字的cookie，那么提取并使用它的值
    if (pos != -1) {                                        //如果pos值为-1则说明搜索"version="失败
        var start = pos + name.length;                      //cookie值开始的位置
        var end = allcookies.indexOf(";", start);           //从cookie值开始的位置起搜索第一个";"的位置,即cookie值结尾的位置
        if (end == -1)
            end = allcookies.length;                        //如果end值为-1说明cookie列表里只有一个cookie
        var value = allcookies.substring(start, end);       //提取cookie的值
        return decodeURIComponent(value);                   //对它解码
    }
    else return "";                //搜索失败，返回空字符串
}

//删除cookie
function deleteCookie(name, path) {
    var name = escape(name);
    var expires = new Date(0);
    path = path == "" ? "" : ";path=" + path;
    document.cookie = name + "=" + ";expires=" + expires.toUTCString() + path;
}


//继续登录
function go_login() {
    location.replace('/');
}



//演示帐号
$('#yanshi').click(function () {
    $('#demoDiv').show();
});
function closeDemo() {
    $('#demoDiv').hide();
}
$('#ddversion').click(function () {
    var smData = {};
    smData.username = "13533285171";
    smData.password = "cysoft";
    smData.yanshi = "true";
    yanshi_login(smData);
});
$('#lsversion').click(function () {
    var smData = {};
    smData.username = "13360019127";
    smData.password = "123456";
    smData.yanshi = "true";
    yanshi_login(smData);
});
function yanshi_login(smData) {
    var options = {
        url: '/account/login',
        data: smData,
        beforeSend: function (XHR) {
            //$(_this).attr('disabled', 'disabled').val('正在登录...');
        },
        callback: function (ret) {
            
            var history = '[{},"index"]';
            cy.cookie.set('dhb_manager/home_menutab', history, {
                expires: 86400, path: '/'
            });
            location.replace(ret.Data);
        }
    }
    cy.http.Post(options);
}