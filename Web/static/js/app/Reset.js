'use strict';

function refreshCode(node) {
    var time = new Date().getTime();
    node.src = "/utility/vaildcode" + '?t=' + time;
}

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
                }
                else {
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

var Recover = {
    init: function () {
        this.initPage();
        this.initEvent();
    },
    initPage: function () {
        var initHeight = $(window).height() - $(".header").height() - $(".footer").height()-100;
        $('.con-row').height(initHeight);

        $(window).resize(function () {
            var $winH = $(window).height() - $(".header").height() - $(".footer").height()-100;
            $(window).height() > 830 ? $('.con-row').height($winH) : initHeight;
        });
    },
    initEvent: function () {
        var _this = this;

        $("body").on("click", ".verification-code-btn,.reset", function (e) {
            var $this = this;
            if (e.type == "click") {
                $("#error").html("");
                $("#success").html("");

                if ($($this).hasClass("verification-code-btn")) {
                    if (_this.getCodeCheck() == false) {
                        return;
                    }

                    var data = {
                        vaildphone: $(".phone").val(),
                        vaildcode: $(".img-code").val(),
                        type: 'reset',
                    };

                    //短信验证码
                    $.ajax({
                        type: "post",
                        dataType: "json",
                        data: data,
                        url: "/utility/phonevaildcode",
                        success: function (ret) {
                            
                            if (ret && ret.Success.toString().toLowerCase() === "true") {
                                if (ret.Level == 5) {
                                    _this.count($this, parseInt(ret.Data));
                                } else {
                                    _this.count($this, 90);
                                }
                                $($this).off('click').addClass('disabled');
                            }
                            else {
                                $('.btn_img_code').click();
                                $('.img-code').val('');
                                $('.verification-code').val('');

                                var msg = '';
                                $.each(ret.Message, function (i, obj) {
                                    msg += obj;
                                    if (i != ret.Message.length - 1)
                                        msg += '；';
                                });
                                $("#error4 label").html(msg).removeClass('hide').show();
                                $("#error4 ").show();
                            }
                        },
                        fail: function (ret) {
                            $('.btn_img_code').click();
                            $('.img-code').val('');
                            $('.verification-code').val('');

                            var msg = '';
                            $.each(ret.message, function (i, obj) {
                                msg += obj;
                                if (i != ret.message.length - 1)
                                    msg += '；';
                            });
                            $("#error4  label").html(msg).removeClass('hide').show();
                            $("#error4 ").show();
                        }
                    })
                }
                else if ($($this).hasClass("reset")) {
                    if (_this.getCodeCheck() == false || _this.checkCode() == false) {
                        return;
                    }

                    var data = $("#recover").serializeObject();

                    $('.reset').attr('disabled', 'disabled').html('正在处理...');
                    $.ajax({
                        type: "post",
                        data: data,
                        dataType: "json",
                        url: "/account/reset",
                        success: function (ret) {
                            
                            if (ret && ret.status == 0 && $.isEmptyObject(ret.redirect_uri) == false) {
                                $("#register-ok").addClass("show");
                                $(".modal-content").find(".modal-header").remove();
                                $(".modal-content").find(".modal-footer").remove();
                                $(".modal").prepend("<div class='modal-backdrop fade in'></div>");
                                setTimeout(function () {
                                    $("#register-ok").addClass("hide");
                                    location.replace(ret.redirect_uri);
                                }, 1000);
                            }
                            else {
                                $('.reset').removeAttr('disabled').html('立即找回');
                                $('.btn_img_code').click();
                                $('.img-code').val('');
                                $('.verification-code').val('');

                                var msg = '';
                                $.each(ret.message, function (i, obj) {
                                    msg += obj;
                                    if (i != ret.message.length - 1)
                                        msg += '；';
                                });
                                $("#error4  label").html(msg).removeClass('hide').show();
                                $("#error4 ").show();
                            }
                        },
                        fail: function (ret) {
                            $('.reset').removeAttr('disabled').html('立即找回');
                            $('.btn_img_code').click();
                            $('.img-code').val('');
                            $('.verification-code').val('');

                            var msg = '';
                            $.each(ret.message, function (i, obj) {
                                msg += obj;
                                if (i != ret.message.length - 1)
                                    msg += '；';
                            });
                            $("#error4  label").html(msg).removeClass('hide').show();
                            $("#error4 ").show();
                        }
                    })
                }
            }
        });

        var $phone = $('.phone'),
        $password = $('.password'),
        $confirm_password = $('.confirm-password'),
        $img_code = $('.img-code'),
        $verification = $('.verification-code'),
        $warnArea = $('#error');


        $phone.on('keypress', function (e) {
            $(this).parent('.login-block').prev('.error').hide();
            e = e || event;
            if (e.charCode === 13 || e.keyCode === 13) {
                $password.focus();
            }
        });

        $password.on('keypress', function (e) {
            $(this).parent('.login-block').prev('.error').hide();
            e = e || event;
            if (e.charCode === 13 || e.keyCode === 13) {
                $confirm_password.focus();
            }
        });

        $confirm_password.on('keypress', function (e) {
            $(this).parent('.login-block').prev('.error').hide();
            e = e || event;
            if (e.charCode === 13 || e.keyCode === 13) {
                $img_code.focus();
            }
        });

        $img_code.on('keypress', function (e) {
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
                $(".reset").click();
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
            $("#error4").html("");
            $("#success").html("未收到短信？可再次发送！");
            return;
        }
        else {
            $(obj).prop("disabled", true);
            $(obj).html("获取验证码 (" + countdown + ")");
            countdown--;

        }

        setTimeout(function () {
            _this.count(obj, countdown)
        }, 1000)

    },
    //获取短信验证码校验
    getCodeCheck: function () {
        var phoneReg = /^0?1[1|2|3|4|5|6|7|8|9][0-9]\d{8}$/g;
        var passwordReg = /^(\w){6,20}$/;
        var imgcodeReg = /[0-9]{4}/g;
        $("#error").html("");
        $("#success").html("");

        var array = [
            $(".phone"),
            $(".password"),
            $('.confirm-password'),
            $('.img-code')
        ];

        for (var i = 0; i < array.length; i++) {
            if (array[i].hasClass("phone")) {
                if (!phoneReg.test(array[i].val())) {
                    array[i].focus();
                    //$("#error").html("必须填写正确的11位手机号");
                    array[i].parent('.login-block').prev('.error').find('label').html('必须填写正确的11位手机号');
                    array[i].parent('.login-block').prev('.error').show();
                    return false;
                }
            }
            else if (array[i].hasClass("password")) {
                if (!passwordReg.test(array[i].val())) {
                    array[i].focus();
                    //$("#error").html("密码长度6~20个英文或数字字符");
                    array[i].parent('.login-block').prev('.error').find('label').html('密码长度6~20个英文或数字字符');
                    array[i].parent('.login-block').prev('.error').show();
                    return false;
                }
            }
            else if (array[i].hasClass("confirm-password")) {
                var $pwd = $('.password').val();
                if ($pwd != array[i].val()) {
                    array[i].focus();
                    //$("#error").html("两次密码必须相同");
                    array[i].parent('.login-block').prev('.error').find('label').html('两次密码必须相同');
                    array[i].parent('.login-block').prev('.error').show();
                    return false;
                }
            }
            else if (array[i].hasClass("img-code")) {
                if (!imgcodeReg.test(array[i].val())) {
                    array[i].focus();
                    //$("#error").html("请输入正确的识别码");
                    array[i].parent('.login-block').prev('.error').find('label').html('请输入正确的识别码');
                    array[i].parent('.login-block').prev('.error').show();
                    return false;
                }
            }
        }
    },
    checkCode: function () {
        var array = [$(".verification-code")];
        var verificationcodeReg = /[0-9]{4}/g;

        $("#error").html("");
        $("#success").html("");

        for (var i = 0; i < array.length; i++) {
            if (array[i].hasClass("verification-code")) {
                if (!verificationcodeReg.test(array[i].val())) {
                    array[i].focus();
                    $("#error3").html("请输入正确的验证码");
                    return false;
                }
            }
        }
    }
};

Recover.init();
$('.phone').blur(function () {
    var phoneReg = /^0?1[1|2|3|4|5|6|7|8|9][0-9]\d{8}$/g;
    
    if (!phoneReg.test($(".phone").val())) {
        $(this).parent('.login-block').prev('.error').find('label').html('必须填写正确的11位手机号');
        $(this).parent('.login-block').prev('.error').show();
    }
})
$('.password').blur(function () {
    var passwordReg = /^(\w){6,20}$/;
    if (!passwordReg.test($(this).val())) {
        $(this).parent('.login-block').prev('.error').find('label').html('密码长度6~20个英文或数字字符');
        $(this).parent('.login-block').prev('.error').show();
    }
})
$('.confirm-password').blur(function () {
    if ($('.password').val() != $(this).val() || $(this).val() == '' || $(this).val() == undefined || $(this).val() == null) {
        $(this).parent('.login-block').prev('.error').find('label').html('两次密码必须相同');
        $(this).parent('.login-block').prev('.error').show();
    }
})