/**
 * Created by rskf on 2016/5/26.
 */

function showError(err, forcusName) {
    if (typeof err === "string" && err.length > 0) {
        var $thisInput = $("input[name='" + forcusName + "']");
        if (typeof forcusName == "string") {
            $thisInput.siblings('.error').text(err);
            $thisInput.focus().parent('.login-block').addClass('focus');
        }
    }
}
function hide(ele) {
    ele.parent('.login-block').removeClass('focus');
}
function show(ele) {
    if (ele.siblings('.error').text() != '') {
        ele.parent('.login-block').addClass('focus');
    }
}