/**
 * Created by rskf on 2016/5/20.
 */
function initPlaceholder($input, msg, classname){
    var isIE = !!window.ActiveXObject || 'ActiveXObject' in window;
    var isPlaceholder = 'placeholder' in document.createElement('input');
    var isIE6=isIE&&!window.XMLHttpRequest;
    var isIE8=isIE&&!!document.documentMode;
    var isIE7=isIE&&!isIE6&&!isIE8;
    if(isPlaceholder && !isIE7 || !isIE8){
        $input.attr('placeholder', msg);
    }else{
        var $tip = $('<span class="' + classname + '">' + msg + '</span>');
        $input.after($tip);
        $.data($input[0], 'tip', $tip);
        if($input.val() != ''){
            hidePHTip($input);
        }
        dealPHTip($input, $tip);
    }
}
function hidePHTip($input){
    var $tip = $.data($input[0], 'tip');
    if($tip){
        $tip.hide();
    }
}
function dealPHTip($input, $tip){
    var _deal = function(){
        var val = $input.val();
        if(val == ''){
            $tip.show();
        }else{
            $tip.hide();
        }
    };
    $tip.click(function(){
        $input.focus();
    });
    $input.on('input propertychange', function(){
        clearTimeout(timeout);
        var timeout = setTimeout(_deal, 0);
    });
}