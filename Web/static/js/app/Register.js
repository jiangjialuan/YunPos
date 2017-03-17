'use strict';

function refreshCode(node) {
    var time = new Date().getTime();
    node.src = "/utility/vaildcode" + '?t=' + time;
}
var cce;

$.fn.extend({
    // 表单序列化为对象
    serializeObject: function () {
        var serializeArr = this.serializeArray(),
                tempArr = {},
                i = 0,
                len = serializeArr.length,
                filedName;

        for (; i < len; i += 1) {

            filedName = serializeArr[i].name;

            if (tempArr[filedName]) {
                if ($.isArray(tempArr[filedName])) {
                    tempArr[filedName].push(serializeArr[i].value);
                } else {
                    tempArr[filedName] = [tempArr[filedName], serializeArr[i].value];
                }
            }
            else {
                tempArr[filedName] = serializeArr[i].value;
            }
        }

        return tempArr;
    }
});

var sign = {
    init: function () {
        this.initPage();
        this.initEvent();
    },
    initPage: function () {
        var initHeight = $(window).height() - $(".header").height() - $(".footer").height()-50;
        $('.con-row').height(initHeight);
        $(window).resize(function () {
            var $winH = $(window).height() - $(".header").height() - $(".footer").height()-50;
            $(window).height() > 830 ? $('.con-row').height($winH) : initHeight;
        });
    },
    verificatCode: function (obj) {
        //debugger;
        var _this = this;
            
            
        var $this = obj;
        var bool_style=_this.getCodeCheck();
        if ( bool_style!=false) {
            var data = {};
            data.vaildphone = $("#phone").val();
            data.vaildcode = $(".recognition-code").val();

            var options = {
                url: '/utility/phonevaildcode',
                data: data,
                callback: function (ret) {
                   
                    if (ret && ret.Success.toString().toLowerCase() === "true") {
                        
                        if (ret.Level == 5) {
                            _this.count($this, parseInt(ret.Data));
                        } else {
                            _this.count($this, 90);
                        }
                        $($this).off('click').addClass('disabled');
                    } else {
                        $('.btn_img_code').click();
                        $('.recognition-code').val('');
                        //$('.verification-code').val('');
                        $('.recognition-code').focus();
                        var msg = '';
                        $.each(ret.Message, function (i, obj) {
                            msg += obj;
                            if (i != ret.Message.length - 1)
                                msg += '；';
                        });
                        if (msg == "您操作太过频繁，请稍后再试。") {
                            clearTimeout(cce);
                            $('.verification-code-btn').html('获取验证码').removeClass('disabled').click(function () {
                                _this.verificatCode(this);
                            });
                        }
                        
                        if (msg.indexOf('该手机号已注册') >= 0) {
                            $("#phone").parent('.login-block').prev('.error').find('label').html(msg);
                            $("#phone").parent('.login-block').prev('.error').show()
                            $("#phone").focus();
                        }
                        else {
                            $('.recognition-code').parent('.login-block').prev('.error').find('label').html(msg);
                            $('.recognition-code').parent('.login-block').prev('.error').show()
                        }

                    }
                },
                error: function (ret) {
                    //
                    $('.btn_img_code').click();
                    $('.recognition-code').val('');
                    //$('.verification-code').val('');
                    $('.recognition-code').focus();

                    var msg = '';
                    $.each(ret.Message, function (i, obj) {
                        msg += obj;
                        if (i != ret.Message.length - 1)
                            msg += '<br />';
                    });
                    $('.recognition-code').parent('.login-block').prev('.error').find('label').html(msg);
                    $('.recognition-code').parent('.login-block').prev('.error').show()
                    if (msg.indexOf('该手机号已注册') >= 0) {
                        $("#phone").focus();
                    }

                }
            };
            cy.http.Post(options);
        }
        },
    initEvent: function () {
        var _this = this;

        //短信验证码
        $('.verification-code-btn').on('click', function () {
            _this.verificatCode(this);
        });
    
        $('#phone')

        //注册
        $(".next-step").on("click", function (e) {
            
            
            var $this = this;
            
            if (_this.getCodeCheck() != false && _this.checkCode() != false) {
                
            
                var data = $("#register-first").serializeObject();

                if (!$('#rules').is(':checked')) {                    
                    //$('#success').css('color', 'red').html("请勾选我接受并同意用户协议");
                    $('#success').css('display', 'block');
                    $('.warn').css('display', 'block');
                    return false;
                }
                if (data.isAgree == 'on') {
                    data.isAgree = 'true';
                }
                //$(this).attr('disabled', 'disabled').html('正在处理...');

                var options = {
                    url: '/account/register',
                    data: data,
                    beforeSend: function (XHR) {
                        $(this).attr('disabled', 'disabled').html('正在处理...');
                    },
                    callback: function (ret) {
                        
                        if (ret && ret.status == 0 && $.isEmptyObject(ret.redirect_uri) == false) {
                            //
                            var history = '[{},"index"]';
                            cy.cookie.set('dhb_manager/home_menutab', history, {
                                expires: 86400, path: '/'
                            });
                            location.replace(ret.redirect_uri);
                        }
                        else {
                            //
                            $('.next-step').removeAttr('disabled').html('立即注册');
                            $('.btn_img_code').click();
                            $('.recognition-code').val('');
                            $('.verification-code').val('');
                            var msg = '';
                            $.each(ret.message, function (i, obj) {
                                msg += obj;
                                if (i != ret.message.length - 1)
                                    msg += '；';
                            });
                            if (msg == "图片识别码错误") {
                                $('.recognition-code').parent('.login-block').prev('.error').find('label').html(msg);
                                $('.recognition-code').parent('.login-block').prev('.error').show();
                                $('.recognition-code').focus();
                            } else {
                                if (msg.indexOf("注册成功") != -1) {
                                    alert(msg);
                                } else {
                                    $('.verification-code').parent('.login-block').prev('.error').find('label').html(msg);
                                    $('.verification-code').parent('.login-block').prev('.error').show();
                                    $('.verification-code').focus();
                                }
                                
                                
                            }
                            //clearTimeout(cce);
                            //$('.verification-code-btn').html('获取验证码').removeClass('disabled').click(function(){
                            //    _this.verificatCode(this);
                            //});
                        }
                    },
                    error: function (ret) {
                        //
                        $('.next-step').removeAttr('disabled').html('立即注册');
                        $('.btn_img_code').click();
                        $('.recognition-code').val('');
                        $('.verification-code').val('');

                        var msg = '';
                        $.each(ret.message, function (i, obj) {
                            msg += obj;
                            if (i != ret.message.length - 1)
                                msg += '；';
                        });

                        $("#error").html(msg).removeClass('hide');
                    }
                };
                cy.http.Post(options);
            }
        });

        var $phone = $('#phone'),
        $password = $('.register-password'),
        $password_confirm = $('.register-password-confirm'),
        $recognition = $('.recognition-code'),
        $verification = $('.verification-code'),
        $warnArea = $('#error');

        $phone.on('keypress', function (e) {
            //console.log(e.type);
            $(this).parent('.login-block').prev('.error').hide();
            //$warnArea.html('').removeClass('hide');
            e = e || event;
            if (e.charCode === 13 || e.keyCode === 13) {
                $password.focus();
            }
        });

        $password.on('keypress', function (e) {
            $(this).parent('.login-block').prev('.error').hide();
            e = e || event;
            if (e.charCode === 13 || e.keyCode === 13) {
                $password_confirm.focus();
            }
        });

        $password_confirm.on('keypress', function (e) {
            $(this).parent('.login-block').prev('.error').hide();
            e = e || event;
            if (e.charCode === 13 || e.keyCode === 13) {
                $recognition.focus();
            }
        });

        $recognition.on('keypress', function (e) {
            $(this).parent('.login-block').prev('.error').hide();
            e = e || event;
            if (e.charCode === 13 || e.keyCode === 13) {
                $verification.focus();
            }
        });

        $verification.on('keypress', function (e) {
            $(this).parent('.login-block').prev('.error').hide();
            e = e || event;
            if (e.charCode === 13 || e.keyCode === 13) {
                $(".next-step").click();
            }
        });

        $phone.focus();
    },
    
    //倒计时
    count: function (obj, countdown) {
        var _this = this;

        if (countdown == 0) {
            $(obj).removeAttr("disabled");
            $(obj).html("获取验证码");
            $(".verification-code-btn").parent('.login-block').prev('.error').hide();
            $("#success").html("未收到短信？可再次发送！");
            //
            $('.verification-code-btn').removeClass('disabled').on('click',function(){
                _this.verificatCode(this);
            });
            return;
        }
        else {
            $(obj).prop("disabled", true);
            $(obj).html("获取验证码 (" + countdown + ")");
            countdown--;
        }

        cce=setTimeout(function () {
            _this.count(obj, countdown)
        }, 1000)

    },
    getCodeCheck: function () {
        //
        var phoneReg = /^0?1[1|2|3|4|5|6|7|8|9][0-9]\d{8}$/g;
        var passwordReg = /^(\w){6,20}$/;
        //var companynameReg = /^(\w){5,30}$/;
        //var shop_mcReg = /^(\w){5,30}$/;
        //var shop_lxrReg = /^(\w){2,30}$/;
        //var recognitionReg = /[a-zA-Z0-9]{4}/g;
        var recognitionReg = /[0-9]{4}/g;
        //var usernameReg = /^[a-zA-Z][a-zA-Z0-9]{4,30}$/;

        var array = [
            $("#phone"),
            //$(".register-username"),
            $('.register-password'),
            $('.register-password-confirm'),
            $('.register-companyname'),
            $('.register-shop_mc'),
            $('.register-shop_lxr'),
            $(".recognition-code")
        ];
        //$("#error").html("");
        //$("#success").html("");
        $("#error").hide();
        $("#success").hide();
        for (var i = 0; i < array.length; i++) {
            
            if (array[i].hasClass("phone")) {
                if (!phoneReg.test(array[i].val())) {
                    array[i].focus();
                    //var error_str = '<p class="warn-info " id="error"></p>';
                    array[i].parent('.login-block').prev('.error').find('label').html('必须填写正确的11位手机号');
                    array[i].parent('.login-block').prev('.error').show();
                    //$("#error").html("必须填写正确的11位手机号");
                    return false;
                }
                //console.log(phoneReg.test(array[i].val()));
            }
                //else if (array[i].hasClass("register-username")) {
                //    if (!usernameReg.test(array[i].val())) {
                //        array[i].focus();
                //        $("#error").html("登录帐号长度5~30个英文或数字字符");
                //        return false;
                //    }
                //}
            else if (array[i].hasClass("register-password")) {
                if (!passwordReg.test(array[i].val())) {
                    array[i].focus();
                    array[i].parent('.login-block').prev('.error').find('label').html('密码长度6~20个英文或数字字符');
                    array[i].parent('.login-block').prev('.error').show();
                    //$("#error").html("密码长度6~20个英文或数字字符");
                    return false;
                }
            }
            else if (array[i].hasClass("register-password-confirm")) {
                var $pwd = $('.register-password').val();
                if ($pwd != array[i].val()) {
                    array[i].focus();
                    array[i].parent('.login-block').prev('.error').find('label').html('两次密码必须相同');
                    array[i].parent('.login-block').prev('.error').show();
                    //$("#error").html("两次密码必须相同");
                    return false;
                }
            }
            else if (array[i].hasClass("register-companyname")) {
                //if (!companynameReg.test(array[i].val())) {
                if (array[i].val().length < 5 || array[i].val().length > 30) {
                    array[i].focus();
                    array[i].parent('.login-block').prev('.error').find('label').html('公司名称长度5~30个字符');
                    array[i].parent('.login-block').prev('.error').show();
                    //$("#error").html("公司名称长度5~30个字符");
                    return false;
                }
            }
            else if (array[i].hasClass("register-shop_mc")) {
                //if (!shop_mcReg.test(array[i].val())) {
                if (array[i].val().length < 5 || array[i].val().length > 30) {
                    array[i].focus();
                    array[i].parent('.login-block').prev('.error').find('label').html('门店名称长度5~30个字符');
                    array[i].parent('.login-block').prev('.error').show();
                    //$("#error").html("门店名称长度5~30个字符");
                    return false;
                }
            }
            else if (array[i].hasClass("register-shop_lxr")) {
                //if (!shop_lxrReg.test(array[i].val())) {
                if (array[i].val().length < 2 || array[i].val().length > 30) {
                    array[i].focus();
                    array[i].parent('.login-block').prev('.error').find('label').html('联系人长度2~30个字符');
                    array[i].parent('.login-block').prev('.error').show();
                    //$("#error").html("联系人长度2~30个字符");
                    return false;
                }
            }
            else if (array[i].hasClass("recognition-code")) {
                if (!recognitionReg.test(array[i].val())) {
                    array[i].focus();
                    array[i].parent('.login-block').prev('.error').html('请输入正确的识别码').show();
                    array[i].parent('.login-block').prev('.error').show();
                    //$("#error").html("请输入正确的识别码");
                    return false;
                }
            }
        }
    },
    checkCode: function () {
        var verificationReg = /[0-9]{4}/g;
        if (!verificationReg.test($(".verification-code").val())) {
            $(".verification-code").focus();
            $(".verification-code-btn").parent('.login-block').prev('.error').find('label').html('请输入正确的验证码');
            $(".verification-code-btn").parent('.login-block').prev('.error').show();
            return false;
        }
    }
};
$('.login_checkbox a').click(function () {
    $('#pagelogin').hide();
    $('#page_login2').show();
});
$('#ty').click(function () {
    $('#pagelogin').show();
    $('#page_login2').hide();
    if (!$('#rules').is(":checked")) {
        $('#rules').click();
    }
});
$('#bty').click(function () {
    $('#pagelogin').show();
    $('#page_login2').hide();
    if ($('#rules').is(":checked")) {
        $('#rules').click();
    }
});
sign.init();
$('#phone').blur(function () {
    var phoneReg = /^0?1[1|2|3|4|5|6|7|8|9][0-9]\d{8}$/g;
    if (!phoneReg.test($("#phone").val())) {
        $(this).parent('.login-block').prev('.error').find('label').html('必须填写正确的11位手机号');
        $(this).parent('.login-block').prev('.error').show();
    }
})
$('.register-password').blur(function () {
    var passwordReg = /^(\w){6,20}$/;
    if (!passwordReg.test($(this).val())) {
        $(this).parent('.login-block').prev('.error').find('label').html('密码长度6~20个英文或数字字符');
        $(this).parent('.login-block').prev('.error').show();
    }
})
$('.register-password-confirm').blur(function () {
    if ($('.register-password').val() != $(this).val()||$(this).val()==''||$(this).val()==undefined||$(this).val()==null) {
        $(this).parent('.login-block').prev('.error').find('label').html('两次密码必须相同');
        $(this).parent('.login-block').prev('.error').show();
    } 
})
