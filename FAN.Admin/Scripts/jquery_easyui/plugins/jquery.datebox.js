/**
 * jQuery EasyUI 1.3.3
 * 
 * Copyright (c) 2009-2013 www.jeasyui.com. All rights reserved.
 *
 * Licensed under the GPL or commercial licenses
 * To use it on other terms please contact us: info@jeasyui.com
 * http://www.gnu.org/licenses/gpl.txt
 * http://www.jeasyui.com/license_commercial.php
 *
 */
(function ($) {
    function _1(_2) {
        var _3 = $.data(_2, "datebox");
        var _4 = _3.options;
        $(_2).addClass("datebox-f");
        $(_2).combo($.extend({}, _4, {
            onShowPanel: function () {
                _3.calendar.calendar("resize");
                _4.onShowPanel.call(_2);
            }
        }));
        $(_2).combo("textbox").parent().addClass("datebox");
        if (!_3.calendar) {
            _5();
        }
        function _5() {
            var _6 = $(_2).combo("panel");
            _3.calendar = $("<div></div>").appendTo(_6).wrap("<div class=\"datebox-calendar-inner\"></div>");
            _3.calendar.calendar({
                fit: true, border: false, onSelect: function (_7) {
                    var _8 = _4.formatter(_7);
                    _c(_2, _8);
                    $(_2).combo("hidePanel");
                    _4.onSelect.call(_2, _7);
                }
            });
            _c(_2, _4.value);
            //var _9 = $("<div class=\"datebox-button\"></div>").appendTo(_6);
            //$("<a href=\"javascript:void(0)\" class=\"datebox-current\"></a>").html(_4.currentText).appendTo(_9);
            //$("<a href=\"javascript:void(0)\" class=\"datebox-close\"></a>").html(_4.closeText).appendTo(_9);
            //_9.find(".datebox-current,.datebox-close").hover(function () {
            //    $(this).addClass("datebox-button-hover");
            //}, function () {
            //    $(this).removeClass("datebox-button-hover");
            //});
            //_9.find(".datebox-current").click(function () {
            //    _3.calendar.calendar({ year: new Date().getFullYear(), month: new Date().getMonth() + 1, current: new Date() });
            //});
            //_9.find(".datebox-close").click(function () {
            //    $(_2).combo("hidePanel");
            //});

            var _a = $("<div class=\"datebox-button\"><table cellspacing=\"0\" cellpadding=\"0\" style=\"width:100%\"><tr></tr></table></div>").appendTo(_6);
            var tr = _a.find("tr");
            for (var i = 0; i < _4.buttons.length; i++) {
                var td = $("<td></td>").appendTo(tr);
                var _b = _4.buttons[i];
                var t = $("<a href=\"javascript:void(0)\" class=\"datebox-current\" style='float:none;'></a>").html($.isFunction(_b.text) ? _b.text(_2) : _b.text).appendTo(td);
                t.bind("click", { target: _2, handler: _b.handler }, function (e) {
                    e.data.handler.call(this, e.data.target);
                });
            }
            tr.find("td").css("width", (100 / _4.buttons.length) + "%");

        };
    };
    function _a(_b, q) {
        _c(_b, q);
    };
    function _d(_e) {
        var _f = $.data(_e, "datebox").options;
        var c = $.data(_e, "datebox").calendar;
        var _10 = _f.formatter(c.calendar("options").current);
        _c(_e, _10);
        $(_e).combo("hidePanel");
    };
    function _c(_11, _12) {
        var _13 = $.data(_11, "datebox");
        var _14 = _13.options;
        $(_11).combo("setValue", _12).combo("setText", _12);
        _13.calendar.calendar("moveTo", _14.parser(_12));
    };
    $.fn.datebox = function (_15, _16) {
        if (typeof _15 == "string") {
            var _17 = $.fn.datebox.methods[_15];
            if (_17) {
                return _17(this, _16);
            } else {
                return this.combo(_15, _16);
            }
        }
        _15 = _15 || {};
        return this.each(function () {
            var _18 = $.data(this, "datebox");
            if (_18) {
                $.extend(_18.options, _15);
            } else {
                $.data(this, "datebox", { options: $.extend({}, $.fn.datebox.defaults, $.fn.datebox.parseOptions(this), _15) });
            }
            _1(this);
        });
    };
    $.fn.datebox.methods = {
        options: function (jq) {
            var _19 = jq.combo("options");
            return $.extend($.data(jq[0], "datebox").options, { originalValue: _19.originalValue, disabled: _19.disabled, readonly: _19.readonly });
        }, calendar: function (jq) {
            return $.data(jq[0], "datebox").calendar;
        }, setValue: function (jq, _1a) {
            return jq.each(function () {
                _c(this, _1a);
            });
        }, reset: function (jq) {
            return jq.each(function () {
                var _1b = $(this).datebox("options");
                $(this).datebox("setValue", _1b.originalValue);
            });
        }
    };
    $.fn.datebox.parseOptions = function (_1c) {
        var t = $(_1c);
        return $.extend({}, $.fn.combo.parseOptions(_1c), {});
    };
    $.fn.datebox.defaults = $.extend({}, $.fn.combo.defaults, {
        panelWidth: 180, panelHeight: "auto", keyHandler: {
            up: function () {
            }, down: function () {
            }, left: function (e) {
            }, right: function (e) {
            }, enter: function () {
                _d(this);
            }, query: function (q) {
                _a(this, q);
            }
        },
        currentText: "Today", closeText: "Close", okText: "Ok",

        buttons: [{
            text: function (_22) {
                return $(_22).datebox("options").currentText;
            }, handler: function (_23) {
                $(_23).datebox("calendar").calendar({ year: new Date().getFullYear(), month: new Date().getMonth() + 1, current: new Date() });
                _d(_23);
            }
        }, {
            text: function (_24) {
                return $(_24).datebox("options").closeText;
            }, handler: function (_25) {
                $(this).closest("div.combo-panel").panel("close");
            }
        }],
        formatter: function (_1d) {
            var y = _1d.getFullYear();
            var m = _1d.getMonth() + 1;
            var d = _1d.getDate();
            return m + "/" + d + "/" + y;
        }, parser: function (s) {
            var t = Date.parse(s);
            if (!isNaN(t)) {
                return new Date(t);
            } else {
                return new Date();
            }
        }, onSelect: function (_1e) {
        }
    });
})(jQuery);

