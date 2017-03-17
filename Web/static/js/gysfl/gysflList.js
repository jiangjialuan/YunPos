
    $('div[contentID="gysfl/list"]').attr({ controller: 'gysfl', action: 'list' });
    app.gysfl = app.gysfl || {};
    app.gysfl.list = app.gysfl.list || {};
    
$(function () {
    app.gysfl.vue = new Vue({
        el: '#gysflList',
        data: {
            Get_Left_Tree:[],            
            default_Html:'',
            gysfl_chirld_data: [],
            tree_defalut:"1",
            add_child_data:''
        },
        watch: {
            //Get_Left_Tree: function () {
            //    this.get_left_tree();
            //},
            
        },
        created:function(){
            this.get_gysfl_data();
        },
        updated:function(){
            if (this.add_child_data=='1') {
                $('#spfl_list tbody tr').eq(app.gysfl.vue.gysfl_chirld_data.length - 1).find('input[name=bm]').focus();
                this.add_child_data = '2';
            }
            
        },
        methods: {
            //获取供应商子类
            gysflchildren: function (child_id) {
                var _this = this;
                var options = {
                    url: $.DHB.U('gysfl/GetChildInfo'),
                    data: { id: child_id },
                    success: function (data) {
                        if (data.Success == true) {                            
                            $('#clicktree', _).val(child_id).attr('xh', data.Data.length);                            
                            _this.gysfl_chirld_data =data.Data;
                        }
                    }
                }
                app.httpAjax.post(options);                
            },
            //绘树
            get_left_tree: function (idselect) {
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
                   // if (_this.tree_defalut == '1') {
                    obj.instance.select_node({ "id": idselect });
                    //    _this.tree_defalut = '2';
                    //}
                    
                    
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
                $.when($.ajax($.DHB.U('gysfl/Get_Left_Tree')), $.ajax({ url: $.DHB.U('gysfl/GetChildInfo'), data: { id: 0 } })).then(function (data1, data2) {
                    
                    if (data1[0] != null && data1[0].Success == true && data1[0].Data!=null) {
                        _this.Get_Left_Tree = usMin.sortBy(data1[0].Data, function (item) { return item.text });

                    } else {
                        $.DHB.message({"content":data1[0].Message,"type":"i"});
                    }
                    if (data2[0] != null && data2[0].Success == true && data2[0].Data != null) {
                        //console.log(usMin.sortBy(data2[0].Data, function (item) { return item.text }));
                        _this.gysfl_chirld_data = usMin.sortBy(data2[0].Data, function (item) { return item.mc });
                    } else {
                        $.DHB.message({ "content": data1[0].Message, "type": "i" });
                    }
                    app.gysfl.vue.get_left_tree("0");
                }, function () {
                    
                });
                
            }

        }
    });

});

app.gysfl.list_ready = function () {
   
    //新增分类
    app.gysfl.add_Gysfl = function () {
        app.gysfl.vue.add_child_data = '1';
        if ($('#clicktree', _).val() == '') {
            $.DHB.message({ 'content': '请选择分类', 'type': 'i' });
            return;
        }
        var arr={
            id: '',
            id_farther: $('#clicktree', _).val(),
            bm: '',
            mc: '',
            }
        app.gysfl.vue.gysfl_chirld_data.push(arr);
        
        
    }
   
    //编码输入框按enter键触发事件
    app.gysfl.list.bmKeyUp = function (obj, e) {
        var $e = e ? e : window.event;
        if ($e.keyCode == 13) {
            $(obj).parents('tr').find('input[name=mc]').focus();
        }
    }
    //名称输入框按enter键触发事件
    app.gysfl.list.mcKeyUp = function (obj, e) {
        var $e = e ? e : window.event;
        if ($e.keyCode == 13) {
            
            if ($(obj).parents('tr').next().length <= 0) {
                app.gysfl.add_Gysfl();
            } else {
                $(obj).parents('tr').next().find('input[name=bm]').focus();
            }
            
        }
    }

    //点击保存
    app.gysfl.list.addpost = function (e) {
        var idDefault = $(e).parents('tr').attr('id');
        if ($(e).parents('tr').find('input[name=mc]').val() == "") {
            $.DHB.message({ 'content': '名称不能为空', 'type': 'i' });
        } else {
            var url;
            var smDate = {};
            smDate.bm = $(e).parents('tr').find('input[name=bm]').val();
            smDate.mc = $(e).parents('tr').find('input[name=mc]').val();
            //
            if (idDefault == "" || idDefault == undefined) {
                url = $.DHB.U('gysfl/Add');
                smDate.parent_id = $(e).parents('tr').attr('id_father');
            } else {
                url = $.DHB.U('gysfl/Edit');
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
                        
                        if (idDefault == "" || idDefault == undefined) {
                            $(e).parents('tr').attr('id', data.gysfl.id);
                            var arr = {
                                bm: smDate.bm,
                                id: data.gysfl.id,
                                parent: smDate.parent_id,
                                text: smDate.mc,
                            }
                            app.gysfl.vue.Get_Left_Tree.push(arr);
                            app.gysfl.vue.get_left_tree(smDate.parent_id);
                        } else {
                           // var gettreeIndex = function () {
                                for (var i = 0; i < app.gysfl.vue.Get_Left_Tree.length; i++) {
                                    if (idDefault == app.gysfl.vue.Get_Left_Tree[i].id) {
                                        app.gysfl.vue.Get_Left_Tree[i].text = smDate.mc;
                                        app.gysfl.vue.Get_Left_Tree[i].bm = smDate.bm;
                                        app.gysfl.vue.get_left_tree(smDate.parent_id);
                                        //return i;
                                        break;
                                    }
                                }
                            // };
                                


                            

                        }
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
    app.gysfl.list.delspfl = function (e) {
        
        var idDefault = $(e).parents('tr').attr('id');
        var index = $(e).parents('tr').index();
        if (idDefault == "" || idDefault == undefined) {     
            app.gysfl.vue.gysfl_chirld_data.splice(index, 1);
        } else {
            $.messager.confirm("提示", "确定删除吗?", function () {
                var options = {
                    data: {
                        id: idDefault
                    },
                    url: $.DHB.U('/gysfl/delete'),
                    type: "POST",
                    datatype: 'json',
                    beforeSend: function () { },
                    success: function (data, textStatus, jqXHR) {
                        if (data.status == "success") {
                            $.DHB.message({ 'content': data.message, 'time': 1000, 'type': 's' });
                            app.gysfl.vue.gysfl_chirld_data.splice(index, 1);
                            var index1;
                            for (var i = 0; i <= app.gysfl.vue.Get_Left_Tree.length; i++) {
                                if (idDefault== app.gysfl.vue.Get_Left_Tree[i].id) {
                                    index1 = i;
                                    break;
                                }
                            }
                            app.gysfl.vue.Get_Left_Tree.splice(index1, 1);
                            app.gysfl.vue.get_left_tree($(e).parents('tr').attr('id_father'));
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

