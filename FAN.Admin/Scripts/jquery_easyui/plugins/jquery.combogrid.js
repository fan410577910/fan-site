(function ($) {
    function _82b(_82c) {
        var _82d = $.data(_82c, "combogrid");
        var opts = _82d.options;
        var grid = _82d.grid;
        $(_82c).addClass("combogrid-f").combo(opts);
        var _82e = $(_82c).combo("panel");
        if (!grid) {
            grid = $("<table></table>").appendTo(_82e);
            _82d.grid = grid;
        }
        grid.datagrid($.extend({}, opts, {
            border: false, fit: true, singleSelect: (!opts.multiple), onLoadSuccess: function (data) {
                var _82f = $(_82c).combo("getValues");
                var _830 = opts.onSelect;
                opts.onSelect = function () {
                };
                _83a(_82c, _82f, _82d.remainText);
                opts.onSelect = _830;
                opts.onLoadSuccess.apply(_82c, arguments);
            }, onClickRow: _831
            , onSelect: function (_832, row) {
                //_833();
                opts.onSelect.call(this, _832, row);
            }, onUnselect: function (_834, row) {
                _833();
                opts.onUnselect.call(this, _834, row);
            }, onSelectAll: function (rows) {
                _833();
                opts.onSelectAll.call(this, rows);
            }, onUnselectAll: function (rows) {
                if (opts.multiple) {
                    _833();
                }
                opts.onUnselectAll.call(this, rows);
            }
        }));
        function _831(_835, row) {
            _82d.remainText = false;
            _833();
            if (!opts.multiple) {
                $(_82c).combo("hidePanel");
            }
            opts.onClickRow.call(this, _835, row);
        };
        function _833() {
            var rows = grid.datagrid("getSelections");
            var vv = [], ss = [];
            for (var i = 0; i < rows.length; i++) {
                vv.push(rows[i][opts.idField]);
                ss.push(rows[i][opts.textField]);
            }
            if (!opts.multiple) {
                $(_82c).combo("setValues", (vv.length ? vv : [""]));
            } else {
                $(_82c).combo("setValues", vv);
            }
            alert("");
            //if (!_82d.remainText) {
            //    $(_82c).combo("setText", ss.join(opts.separator));
            //}
            //：孙亮修改 扩展了 formatter函数
            if (!_82d.remainText && rows.length > 0) {
                var text = opts.formatter.call(this, vv, ss, rows) || ss.join(opts.separator);
                $(_82c).combo("setText", text);
            }
        };
    };
    function nav(_836, dir) {
        var _837 = $.data(_836, "combogrid");
        var opts = _837.options;
        var grid = _837.grid;
        var _838 = grid.datagrid("getRows").length;
        if (!_838) {
            return;
        }
        var tr = opts.finder.getTr(grid[0], null, "highlight");
        if (!tr.length) {
            tr = opts.finder.getTr(grid[0], null, "selected");
        }
        var _839;
        if (!tr.length) {
            _839 = (dir == "next" ? 0 : _838 - 1);
        } else {
            var _839 = parseInt(tr.attr("datagrid-row-index"));
            _839 += (dir == "next" ? 1 : -1);
            if (_839 < 0) {
                _839 = _838 - 1;
            }
            if (_839 >= _838) {
                _839 = 0;
            }
        }
        grid.datagrid("highlightRow", _839);
        if (opts.selectOnNavigation) {
            _837.remainText = false;
            grid.datagrid("selectRow", _839);
        }
    };
    function _83a(_83b, _83c, _83d) {
        var _83e = $.data(_83b, "combogrid");
        var opts = _83e.options;
        var grid = _83e.grid;
        var rows = grid.datagrid("getRows");
        var ss = [], selectRows = [];
        var _83f = $(_83b).combo("getValues");
        var _840 = $(_83b).combo("options");
        var _841 = _840.onChange;
        _840.onChange = function () {
        };
        grid.datagrid("clearSelections");
        var m = null;
        for (var i = 0; i < _83c.length; i++) {
            var _842 = grid.datagrid("getRowIndex", _83c[i]);
            if (_842 >= 0) {
                grid.datagrid("selectRow", _842);
                m = rows[_842];
                selectRows.push(m);
                ss.push(m[opts.textField]);
            }
            //else {
            //    ss.push(_83c[i]);
            //}
        }
        $(_83b).combo("setValues", _83f);
        _840.onChange = _841;
        $(_83b).combo("setValues", _83c);
        if (!_83d && selectRows.length > 0) {

            // var s = ss.join(opts.separator);
            alert(selectRows);
            //孙亮修改
            var text = opts.formatter.call(this, _83f, ss, selectRows) || ss.join(opts.separator);
            if ($(_83b).combo("getText") != text) {
                $(_83b).combo("setText", text);
            }
        }
    };
    function _843(_844, q) {
        var _845 = $.data(_844, "combogrid");
        var opts = _845.options;
        var grid = _845.grid;
        _845.remainText = true;
        if (opts.multiple && !q) {
            _83a(_844, [], true);
        } else {
            _83a(_844, [q], true);
        }
        if (opts.mode == "remote") {
            grid.datagrid("clearSelections");
            grid.datagrid("load", $.extend({}, opts.queryParams, { q: q }));
        } else {
            if (!q) {
                return;
            }
            var rows = grid.datagrid("getRows");
            for (var i = 0; i < rows.length; i++) {
                if (opts.filter.call(_844, q, rows[i])) {
                    grid.datagrid("clearSelections");
                    grid.datagrid("selectRow", i);
                    return;
                }
            }
        }
    };
    function _846(_847) {
        var _848 = $.data(_847, "combogrid");
        var opts = _848.options;
        var grid = _848.grid;
        var tr = opts.finder.getTr(grid[0], null, "highlight");
        if (!tr.length) {
            tr = opts.finder.getTr(grid[0], null, "selected");
        }
        if (!tr.length) {
            return;
        }
        _848.remainText = false;
        var _849 = parseInt(tr.attr("datagrid-row-index"));
        if (opts.multiple) {
            if (tr.hasClass("datagrid-row-selected")) {
                grid.datagrid("unselectRow", _849);
            } else {
                grid.datagrid("selectRow", _849);
            }
        } else {
            grid.datagrid("selectRow", _849);
            $(_847).combogrid("hidePanel");
        }
    };
    $.fn.combogrid = function (_84a, _84b) {
        if (typeof _84a == "string") {
            var _84c = $.fn.combogrid.methods[_84a];
            if (_84c) {
                return _84c(this, _84b);
            } else {
                return $.fn.combo.methods[_84a](this, _84b);
            }
        }
        _84a = _84a || {};
        return this.each(function () {
            var _84d = $.data(this, "combogrid");
            if (_84d) {
                $.extend(_84d.options, _84a);
            } else {
                _84d = $.data(this, "combogrid", { options: $.extend({}, $.fn.combogrid.defaults, $.fn.combogrid.parseOptions(this), _84a) });
            }
            _82b(this);
        });
    };
    $.fn.combogrid.methods = {
        options: function (jq) {
            var _84e = jq.combo("options");
            return $.extend($.data(jq[0], "combogrid").options, { originalValue: _84e.originalValue, disabled: _84e.disabled, readonly: _84e.readonly });
        }, grid: function (jq) {
            return $.data(jq[0], "combogrid").grid;
        }, setValues: function (jq, _84f) {
            return jq.each(function () {
                _83a(this, _84f);
            });
        }, setValue: function (jq, _850) {
            return jq.each(function () {
                _83a(this, [_850]);
            });
        }, clear: function (jq) {
            return jq.each(function () {
                $(this).combogrid("grid").datagrid("clearSelections");
                $(this).combo("clear");
            });
        }, reset: function (jq) {
            return jq.each(function () {
                var opts = $(this).combogrid("options");
                if (opts.multiple) {
                    $(this).combogrid("setValues", opts.originalValue);
                } else {
                    $(this).combogrid("setValue", opts.originalValue);
                }
            });
        }, reload: function (jq, url) {
            return jq.each(function () {
                $(this).combo("clear");
                var _21 = $.data(this, "combogrid").options;
                var _22 = $.data(this, "combogrid").grid;
                _22.datagrid("clearSelections");
                if (url) {
                    _21.url = url;
                }
                _22.datagrid({ url: _21.url });
            });
        }
    };
    $.fn.combogrid.parseOptions = function (_851) {
        var t = $(_851);
        return $.extend({}, $.fn.combo.parseOptions(_851), $.fn.datagrid.parseOptions(_851), $.parser.parseOptions(_851, ["idField", "textField", "mode"]));
    };
    $.fn.combogrid.defaults = $.extend({}, $.fn.combo.defaults, $.fn.datagrid.defaults, {
        loadMsg: null, idField: null, textField: null, mode: "local", keyHandler: {
            up: function () {
                nav(this, "prev");
            }, down: function () {
                nav(this, "next");
            }, enter: function () {
                _846(this);
            }, query: function (q) {
                _843(this, q);
            }
        }, filter: function (q, row) {
            var opts = $(this).combogrid("options");
            return row[opts.textField].indexOf(q) == 0;
        },
        //孙亮修改
        formatter: function (values, texts, rows) {
            return texts;
        }
    });
})(jQuery);
