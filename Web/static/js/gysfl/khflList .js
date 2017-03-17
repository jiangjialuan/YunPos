
$('div[contentID="khfl/list"]').attr({ controller: 'khfl', action: 'list' });
app.khfl = app.khfl || {};
app.khfl.list = app.khfl.list || {};

$(function () {
    app.khfl.vue = new Vue({
        el: '#khflList',
        data: {
            Get_Left_Tree: [],
            default_Html: '',
            khfl_chirld_data: [],
        },
        watch: {
            //Get_Left_Tree: function () {
            //    this.get_left_tree();
            //},

        },
        created: function () {
            // 
            this.get_gysfl_data();
        },
        methods: {
            //获取供应商子类
            gysflchildren: function (child_id) {
                var _this = this;
                var options = {
                    url: $.DHB.U('khfl/GetChildInfo'),
                    data: { id: child_id },
                    success: function (data) {
                        if (data.Success == true) {
                            $('#clicktree', _).val(child_id).attr('xh', data.Data.length);
                            _this.khfl_chirld_data = data.Data;
                        }
                    }
                }
                app.httpAjax.post(options);

            },
            //绘树
            get_left_tree: function () {
                _this = this;

                var tree_selector = '#tree_left';
                $(_ + tree_selector)
                    .jstree("destroy")
                    .jstree({
                        'plugins': ['dnd'],
                        'core': {
                            'data': _this.Get_Left_Tree,
                            "themes": {
                                "theme": "proton",
                                "dots": true,
                                "icons": true,
                            },
                        },
                    }).on('ready.jstree', function (e, obj) {

                        obj.instance.open_node({ "id": "0" });
                    }).on('changed.jstree', function (e, data) {

                        if (data.selected && data.selected.length) {
                            var i, j, r = [];
                            for (i = 0, j = data.selected.length; i < j; i++) {
                                r.push(data.instance.get_node(data.selected[i]).id);
                            }
                            //TODO：赋值到hidden，触发查询事件等
                            var child_id = r.join('_');
                            _this.gysflchildren(child_id);
                        }
                    });
            },
            //初始化
            get_gysfl_data: function () {
                _this = this;
                $.when($.ajax($.DHB.U('khfl/Get_Left_Tree')), $.ajax({ url: $.DHB.U('khfl/GetChildInfo'), data: { id: 0 } })).then(function (data1, data2) {
                    
                    if (data1[0] != null && data1[0].Success == true && data1[0].Data != null) {
                        _this.Get_Left_Tree = data1[0].Data;
                    } else {
                        $.DHB.message({ "content": data1[0].Message, "type": "i" });
                    }
                    if (data2[0] != null && data2[0].Success == true && data2[0].Data != null) {
                        _this.khfl_chirld_data = data2[0].Data;
                    } else {
                        $.DHB.message({ "content": data1[0].Message, "type": "i" });
                    }
                    app.khfl.vue.get_left_tree();
                }, function () {

                });

            }

        }
    });

});

app.khfl.list_ready = function () {

    //重置序号
    app.khfl.list.xhset = function () {
        var xh = 1;
        $('#spfl_list>table>tbody>tr', _).each(function (i, e) {
            $(this).find('td[name=xh]').html(xh);
            xh++;
        });
    }
    //表格行添加
    app.khfl.list.htmladd = function (data) {

        var trhtml = '<tr id="' + data.id + ' " id_father="' + $('#clicktree', _).val() + '">';
        trhtml += '<td class="align_center" name="xh"></td>';
        trhtml += '<td><input value="' + data.bm + '" type="text" name="bm"  class="form-control user-input"  onkeyup="app.khfl.list.bmKeyUp(this)"/></td>';
        trhtml += '<td><input value="' + data.mc + '" type="text" name="mc" class="width200 form-control user-input"  onkeyup="app.khfl.list.mcKeyUp(this)"/></td>';
        trhtml += '<td><div class="look-out1 supernatant position_static" style="width:140px;"><div class="second-height-operate position_static">';
        trhtml += '<a class="bg-state bg-state-info fa fa-floppy-o" title="保存" onclick="app.khfl.list.addpost(this)"></a>';
        trhtml += '<a class="bg-state bg-state-info fa fa-trash" title="删除" onclick="app.khfl.list.delspfl(this)"></a>';
        trhtml += '</div></div></td>';
        trhtml += "</tr>";
        $('#spfl_list>table>tbody', _).append(trhtml);
    }
    //新增分类
    app.khfl.add_khfl = function () {
        debugger;
        var arr = {
            id: '',
            id_farther: $('#clicktree', _).val(),
            bm: '',
            mc: '',
        }
        app.khfl.vue.khfl_chirld_data.push(arr);

    }

    //编码输入框按enter键触发事件
    app.khfl.list.bmKeyUp = function (obj, e) {
        var $e = e ? e : window.event;
        if ($e.keyCode == 13) {
            $(obj).parents('tr').find('input[name=mc]').focus();
        }
    }
    //名称输入框按enter键触发事件
    app.khfl.list.mcKeyUp = function (obj, e) {
        var $e = e ? e : window.event;
        if ($e.keyCode == 13) {

            if ($(obj).parents('tr').next().length <= 0) {
                    app.khfl.add_khfl();
                
            }
            $(obj).parents('tr').next().find('input[name=bm]').focus();
        }
    }

    //点击保存
    app.khfl.list.addpost = function (e) {
        debugger;
        console.log(app.khfl.vue.khfl_chirld_data[app.khfl.vue.khfl_chirld_data.length - 1]);
        var id = $(e).parents('tr').attr('id');
        //if ($(e).parents('tr').find('input[name=bm]').val() == "" || $(e).parents('tr').find('input[name=mc]').val() == "") {
        if ($(e).parents('tr').find('input[name=mc]').val() == "") {
            $.DHB.message({ 'content': '名称不能为空', 'type': 'i' });
        } else {
            var url;
            var smDate = {};
            smDate.bm = $(e).parents('tr').find('input[name=bm]').val();
            smDate.mc = $(e).parents('tr').find('input[name=mc]').val();
            //
            if (id == "" || id == undefined) {
                url = $.DHB.U('khfl/Add');
                smDate.parent_id = $(e).parents('tr').attr('id_father');
            } else {
                url = $.DHB.U('khfl/Edit');
                smDate.category_id = $(e).parents('tr').attr('id');

            };

            var options = {
                data: smDate,
                url: url,
                type: "POST",
                datatype: 'json',
                beforeSend: function () { },
                success: function (data, textStatus, jqXHR) {

                    if (data.status == "success") {
                        $.DHB.message({ 'content': data.message, 'time': 1000, 'type': 's' });
                        $(e).parents('tr').attr('id', data.khfl.id);
                        var arr = {
                            bm: smDate.bm,
                            id: data.khfl.id,
                            parent: smDate.parent_id,
                            text: smDate.mc,
                        }
                        app.khfl.vue.Get_Left_Tree.push(arr);
                        app.khfl.vue.get_left_tree();

                    }
                    else {
                        $.DHB.message({ 'content': data.message, 'time': 0, 'type': 'e' });
                    }
                },
                complete: function (XHR, TS) { }
            };
            app.httpAjax.post(options)
        }
    }
    //删除供应商分类
    app.khfl.list.delspfl = function (e) {
        var id = $(e).parents('tr').attr('id');
        if (id == "" || id == undefined) {
            $(e).parents('tr').remove();
            //app.spfl.list.xhset();
        } else {
            $.messager.confirm("提示", "确定删除吗?", function () {
                var options = {
                    data: {
                        id: id
                    },
                    url: $.DHB.U('/khfl/delete'),
                    type: "POST",
                    datatype: 'json',
                    beforeSend: function () { },
                    success: function (data, textStatus, jqXHR) {
                        if (data.status == "success") {
                            $.DHB.message({ 'content': data.message, 'time': 1000, 'type': 's' });
                            $(e).parents('tr').remove();
                            app.khfl.list.xhset();
                        }
                        else {
                            $.DHB.message({ 'content': data.message, 'time': 0, 'type': 'e' });
                        }
                    },
                    complete: function (XHR, TS) { }
                };
                app.httpAjax.post(options)

            });
        }

    }

}

