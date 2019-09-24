window.erp = {};
var _t = top,_t$=top.$;

$.extend(erp,{
    getId: function () {
        return new Date().getTime();
    },
    tabShow: function (title, url, img) {
        if (url == ' ' || !url) return;
        var _id = erp.getId();
        url = url.indexOf("?") > -1 ? url + '&_=' + _id : url + '?_=' + _id;
        title = title || '新标签';
        
        if (_t.$tabPages.tabs('exists', title)) {
            var tabOpts = _t.$tabPages.tabs('getTabOption', title);
            _t.$tabPages.tabs('update', {
                tab: _t.$tabPages.tabs("getTab", title),
                options: {
                    title: title,
                    href: url,
                    iniframe: true,
                    closable: true,
                    fit: true,
                    iconCls: img || tabOpts.iconCls || "icon-blank",
                    selected: true,
                    showUpdateProgress: false,
                    id: _id
                }
            });
            _t.$tabPages.tabs('select', title);
        } else {
            _t.$tabPages.tabs("add", {
                title: title,
                href: url,
                iniframe: true,
                closable: true,
                fit: true,
                iconCls: img || "icon-blank",
                selected: true,
                showUpdateProgress: false,
                id: _id
            });
            var iframe = _t$("#" + _id + " iframe");
            if (iframe.length > 0) {
                iframe.get(0).contentWindow.name = title;
                iframe.parent().css("overflow", "hidden").attr("_ddd", title);
            }
        }
        return _t.$tabPages;
    },
    tabClose: function (title) {
        _t.$tabPages.tabs("close", title);
    },
    tabRefresh: function (title) {
        // &_=  ?_= TODO:添加随机数不缓存onRefresh
        _t.$tabPages.tabs("refresh", title);
    },
    tabReload: function () {
        erp.wait("请稍后,正在刷新页面...");
        self.window.location.reload(true);
    },
    show: function (content) {
        return $.easyui.messager.show({ msg: content || "", position: "bottomRight" });
    },
    showError: function (content) {
        content = content || "";
        var m = ((content.getLength() || 100) / 30) * 19 + 50;
        if (m < 100) m = 100;
        return $.easyui.messager.show({ msg: content, position: "bottomRight", icon: "error", height: m });
    },
    showError: function (content, positionText) {
        content = content || "";
        if (positionText == null || positionText == undefined || positionText == '') positionText = "bottomRight";
        var m = ((content.getLength() || 100) / 30) * 19 + 50;
        if (m < 100) m = 100;
        return $.easyui.messager.show({ msg: content, position: positionText, icon: "error", height: m });
    },
    showInfo: function (content) {
        return $.easyui.messager.show({ msg: content || "", position: "bottomRight", icon: "info" });
    },
    showInfo: function (content, positionText) {
        if (positionText == null || positionText == undefined || positionText == '') positionText = "bottomRight";
        return $.easyui.messager.show({ msg: content || "", position: positionText, icon: "info" });
    },
    open: function (url, data) {
        if (!!data)
            url = url.indexOf("?") > -1 ?
                url + "&" + $.param(data)
                : url + "?" + $.param(data);
        url = url.indexOf("?") > -1 ?
            url + '&_=' + erp.getId()
            : url + '?_=' + erp.getId();
        _OPEN.parent().attr("href", url);
        _OPEN.trigger("click");
    },
    confirm: function () { return $.messager.confirm.apply(this, arguments); },
    alert: function () { return $.messager.alert.apply(this, arguments); },
    dialog: function (title, href, top, left, width, height) {
        return $.easyui.showDialog({
            title: title,
            top: top,
            left: left,
            width: width,
            height: height,
            href: href,
            iniframe: false,
            autoVCenter: true,
            autoHCenter: true
        });
    },
    wait: function (options) {
        if (!options) {
            $.easyui.loaded();
            return;
        }
        if (!$.isPlainObject(options))
            options = { msg: options };
        options = options || {};
        $.easyui.loading(options);
    },
    sys: {},
    format: {
        booleanFormater: function (value, row, index) {
            return value == true ? "是" : "否";
        },
        enableFormater: function (value, row, index) {
            return value == true ? "启用" : "禁用";
        },
        imageFormater: function (value, row, index) {
            return "<img src='" + erp.ui.imageSmallSrcBuilder(row.SPUID, value) + "' style='width:auto;height:100px;' title='' data=''/>";
        },
        specificationFormater: function (value, row, index) {
            return (!value) ? "" : value.replace(/@/g, "<br/>");
        },
        newlineFormater: function (value, row, index) {
            return (!value) ? "" : value.replace("\r\n", "<br/>");
        },
        dateFormater: function (value, row, index) {
            return (!value || value == "/Date(-2209017600000)/") ? "" : erp.utils.toDate(value).format("yyyy-MM-dd");
        },
        datetimeFormater: function (value, row, index) {
            return (!value || value == "/Date(-2209017600000)/") ? "" : erp.utils.toDate(value).format("yyyy-MM-dd<br/>hh:mm:ss");
        },
        datetimeNo1900Formater: function (value, row, index) {
            return (!value || value == "/Date(-2209017600000)/") ? "" : erp.utils.toDate(value).format("yyyy-MM-dd<br/>hh:mm:ss");
        },
        datetimeNo0001Formater: function (value, row, index) {
            return (!value || value == "/Date(-62135596800000)/") ? "" : erp.utils.toDate(value).format("yyyy-MM-dd<br/>hh:mm:ss");
        },
        dateHourMinFormater: function (value, row, index) {
            return (!value) ? "" : erp.utils.toDate(value).format("yyyy-MM-dd<br/>hh:mm");
        },
        timeFormater: function (value, row, index) {
            return (!value) ? "" : erp.utils.toDate(value).format("hh:mm:ss");
        },
        timespanFormater: function (value, row, index) {
            var day = (value / (24 * 60 * 60)).toFixed(0).toString();
            var hour = ((value - day * (24 * 60 * 60)) / (60 * 60)).toFixed(0).toString();
            var second = ((value - day * (24 * 60 * 60) - hour * (60 * 60)) / 60).toFixed(0).toString();
            return day + "天" + ("00" + hour).substr(hour.length, 2) + "时" + ("00" + second).substr(second.length, 2) + "分";
        },
        rangeDayFromater: function (startFiled, endFiled, fixed, callback) {
            return erp.format.rangeSecondFromater(
                startFiled,
                endFiled,
                10,
                function (t) {
                    t = (t / (24 * 60 * 60)).toFixed(fixed);
                    if ($.isFunction(callback)) return callback.call(this, t) || t
                });
        },
        rangeHourFromater: function (startFiled, endFiled, fixed, callback) {
            return erp.format.rangeSecondFromater(
                startFiled,
                endFiled,
                10,
                function (t) {
                    t = (t / (60 * 60)).toFixed(fixed);
                    if ($.isFunction(callback)) return callback.call(this, t) || t
                });
        },
        rangeSecondFromater: function (startFiled, endFiled, fixed, callback) {
            if (!startFiled || !endFiled)
                return null;
            if (typeof (startFiled) !== "string" || typeof (startFiled) !== "string")
                return null;
            fixed = fixed || 2;
            return function (value, row, index) {
                var dif = erp.utils.toDate(row[endFiled]).getTime() - erp.utils.toDate(row[startFiled]).getTime();
                var t = (dif / 1000).toFixed(fixed);
                if ($.isFunction(callback)) return callback.call(row, t) || t;
                return t;
            };
        },
        exchangeRateFromater: function (value, row, index) {
            if (value == null || value == undefined) {
                return "";
            }
            return value.toFixed(2);
        },
        moneyColorfulFromater: function (value, row, index) {
            if (value == null || value == undefined) {
                return "";
            }
            if (value >= 0) {
                return "<span style='color:#00f'>" + value.toFixed(2) + "</span>";
            } else {
                return "<span style='color:#f00'>" + value.toFixed(2) + "</span>";
            }
        },
        moneyFromater: function (value, row, index) {
            if (value == null || value == undefined) {
                return "";
            }
            return value.toFixed(2);
        },
        money4Fromater: function (value, row, index) {
            if (value == null || value == undefined) {
                return "";
            }
            return value.toFixed(4);
        },
        weightFromater: function (value, row, index) {
            if (value == null || value == undefined) {
                return "";
            }
            return value.toFixed(0);
        }
    },
    styler: {
        colorCustomer: function (value, row, index) { }
    },
    parser: {

    },
    ui: {
        changeTheme: function (themeName) {
            $.easyui.theme(true, themeName, function (item) {
                $.cookie("themeName", themeName, { expires: 30 });
                erp.showInfo($.string.format("您设置了新的系统主题皮肤为：{0}，{1}。", item.name, item.path));
            });
        },
        mergeColumnCells: function (grid, rowFildName) {
            var rows = grid.datagrid('getRows');
            var startIndex = 0;
            var endIndex = 0;
            if (rows.length < 1) {
                return;
            }
            $.each(rows, function (i, row) {
                if (row[rowFildName] == rows[startIndex][rowFildName]) {
                    endIndex = i;
                } else {
                    grid.datagrid('mergeCells', {
                        index: startIndex,
                        field: rowFildName,
                        rowspan: endIndex - startIndex + 1
                    });
                    startIndex = i;
                    endIndex = i;
                }
            });
            grid.datagrid('mergeCells', {
                index: startIndex,
                field: rowFildName,
                rowspan: endIndex - startIndex + 1
            });
        },
        imageBigSrcBuilder: function (SPUID, sort) {
            if (!!SPUID) {
                return "http://erp.tidebuy.com/productimgs/" + parseInt(SPUID / 1000000) + "/" + parseInt(SPUID / 1000) + "/" + SPUID + "_" + sort + ".jpg";
            } else {
                return "";
            }
        },
        imageSmallSrcBuilder: function (SPUID, sort) {
            if (!!SPUID) {
                return "http://erp.tidebuy.com/productimgs/small/" + parseInt(SPUID / 1000000) + "/" + parseInt(SPUID / 1000) + "/" + SPUID + "_" + sort + ".jpg";
            } else {
                return "";
            }
        },
        imageBigSrcBuilder: function (productID, sort, SiteID) {
            if (!!SPUID) {
                $.post("/Resources/GetBigImg?siteID=" + SiteID + "&id=" + productID + "&fileName=" + sort + "", null, function (data) {
                    return data;
                });
            } else {
                return "";
            }
        },
        imageSmallSrcBuilderNew: function (oldproductID, sort, siteId) {
            if (!!oldproductID) {
                var size = "s";
                var url = this.GetDomain(siteId) + "/images/product/" + parseInt(oldproductID / 1000000) + "/" + parseInt(oldproductID / 1000) + "/" + oldproductID + "_" + sort + "_" + size + ".jpg";
                return url;
            } else {
                return "";
            }
        },
        imageMiddeleSrcBuilderNew: function (oldproductID, sort, siteId) {
            if (!!oldproductID) {
                var size = "m";
                var url = this.GetDomain(siteId) + "/images/product/" + parseInt(oldproductID / 1000000) + "/" + parseInt(oldproductID / 1000) + "/" + oldproductID + "_" + sort + "_" + size + ".jpg";
                return url;
            } else {
                return "";
            }
        },
        GetDomain: function (siteId) {

            var domin = "http://s.tidebuy.com";
            switch (siteId) {
                case 0:
                    domin = "http://s.tidebuy.com";
                    break;
                case 1:
                    domin = "http://s.tbdress.com";
                    break;
                case 3:
                    domin = "http://s.wigsbuy.com";
                    break;
                case 6:
                    domin = "http://s.dressv.com";
                    break;
                case 7:
                    domin = "http://s.dressvenus.com";
                    break;
                case 8:
                    domin = "http://s.dresswe.com";
                    break;
                case 11:
                    //原先的值 如果后面加/页面会造成多//
                    // domin = "http://119.254.16.153:810/";
                    //王安靖修改为
                    domin = "http://s.ericdress.com";
                    break;
                case 13:
                    // domin = "http://119.254.20.114:808/";
                    //王安靖修改为
                    domin = "http://119.254.20.114:808";
                    break;
                case 18:
                    domin = "http://s.shoespie.com";
                    break;
                case 19:
                    domin = "http://s.beddinginn.com";
                    break;
                case 21:
                    domin = "http://s.wigsu.com";
                    break;
                case 22:
                    domin = "http://s.msfairy.com";
                    break;
                case 23:
                    domin = "http://s.casasilk.com";
                    break;
                case 24:
                    domin = "http://s.swanshow.com";
                    break;
                case 25:
                    domin = "http://s.freepresale.com/";
                    break;
            }
            return domin;
        }
    },
    filter: {
        loadFilter: function (json) {
            if (!!json) {
                if (json.Success === true) {
                    return json.Data;
                }
                erp.showError(json.Message);
                return [];
            }
            return [];
        },
        comboLoadFilter: function (value) {
            if ($.isArray(value))
                return value;
            return erp.filter.loadFilter(value);
        },
        getValueFormat: function (value, formatter) {
            if ($.isFunction(formatter))
                return formatter(value, null, 0);
            return value;
        },
        barRander: function (view, filterData, text) {
            if (!view || !filterData) return;
            var t = null, html = [], col = null;
            for (var name in filterData) {
                if (!!(t = filterData[name])) {
                    col = view.datagrid("getColumnOption", name);
                    if (!col || (!!col.filterBar && col.filterBar !== false)) continue;
                    if (t.Value != null) {
                        //modify by Yang 2018-4-12 16:22:39 新增多筛选
                        html.push("<span class=\"searchbox menu-active filterBar-item\"><b>[" + col.title + "]</b>:");
                        if (text != null && text != undefined) {
                            html.push(text);
                        } else if (erp.utils.isArray(t.Value) && t.Value.length > 0) {
                            for (var m = 0; m < t.Value.length - 1; m++) {
                                html.push(erp.filter.getValueFormat(t.Value[m], col.formatter) + ",");
                            }
                            html.push(erp.filter.getValueFormat(t.Value[t.Value.length - 1], col.formatter));
                        } else {
                            html.push(erp.filter.getValueFormat(t.Value, col.formatter));
                        }
                        if (!("Close" in t) || t.Close) {
                            html.push("<a href=\"javascript:void(0);\" class=\"filterBar-close\" onclick=\"erp.filterBarHandle(this,'" + name + "',{'Value':null})\">×</a>");
                        }
                        html.push("</span>");
                    } else if (t.StartValue != null || t.EndValue != null && t.StartValue != undefined && t.EndValue != undefined) {
                        html.push("<span class=\"searchbox menu-active filterBar-item\"><b>[" + col.title + "]</b>:");
                        if (typeof (t.StartValue) != "string" && typeof (t.EndValue) != "string") {
                            html.push((t.StartValue == undefined || t.StartValue == null) ? "∞" : erp.filter.getValueFormat(t.StartValue, col.formatter));
                            html.push("至");
                            html.push((t.EndValue == undefined || t.EndValue == null) ? "∞" : erp.filter.getValueFormat(t.EndValue, col.formatter));
                        } else {
                            html.push((t.StartValue == undefined || t.StartValue == null) ? "∞" : t.StartValue);
                            html.push("至");
                            html.push((t.EndValue == undefined || t.EndValue == null) ? "∞" : t.EndValue);
                        }
                        if (!("Close" in t) || t.Close) {
                            html.push("<a href=\"javascript:void(0);\" class=\"filterBar-close\" onclick=\"erp.filterBarHandle(this,'" + name + "',{'StartValue':null,'EndValue':null})\">×</a>");
                        }
                        html.push("</span>");
                    }
                }
            }
            if (html.length == 0)
                html.push("<span class=\"searchbox menu-active filterBar-item filterBar-none\">显示全部</span>");
            return html.join('');
        },

        barOtherRander: function (view, filterData) {
            if (!view || !filterData) return;
            var t = null, html = [], col = null;
            for (var name in filterData) {
                if (!!(t = filterData[name])) {
                    col = view.datagrid("getColumnOption", name);
                    if (!col || (!!col.filterBar && col.filterBar !== false)) continue;
                    if (t.Value != null) {
                        html.push("<span class=\"searchbox menu-active filterBar-item\"><b>[" + col.title + "]</b>:");
                        if (erp.utils.isArray(t.Value) && t.Value.length > 0) {

                            for (var m = 0; m < t.Value.length - 1; m++) {
                                html.push(erp.filter.getValueFormat(t.Value[m], col.formatter) + ",");
                            }
                            html.push(erp.filter.getValueFormat(t.Value[t.Value.length - 1], col.formatter));
                        } else {
                            html.push(erp.filter.getValueFormat(t.Value, col.formatter));
                        }
                        html.push("<a href=\"javascript:void(0);\" class=\"filterBar-close\" onclick=\"erp.filterBillBarHandle(this,'" + name + "',{'Value':null})\">×</a></span>");
                    } else if (t.StartValue != null || t.EndValue != null && t.StartValue != undefined && t.EndValue != undefined) {
                        html.push("<span class=\"searchbox menu-active filterBar-item\"><b>[" + col.title + "]</b>:");
                        if (typeof (t.StartValue) != "string" && typeof (t.EndValue) != "string") {
                            html.push((t.StartValue == undefined || t.StartValue == null) ? "∞" : erp.filter.getValueFormat(t.StartValue, col.formatter));
                            html.push("至");
                            html.push((t.EndValue == undefined || t.EndValue == null) ? "∞" : erp.filter.getValueFormat(t.EndValue, col.formatter));
                        } else {
                            html.push((t.StartValue == undefined || t.StartValue == null) ? "∞" : t.StartValue);
                            html.push("至");
                            html.push((t.EndValue == undefined || t.EndValue == null) ? "∞" : t.EndValue);
                        }
                        html.push("<a href=\"javascript:void(0);\" class=\"filterBar-close\" onclick=\"erp.filterBillBarHandle(this,'" + name + "',{'StartValue':null,'EndValue':null})\">×</a></span>");
                    }
                }
            }
            if (html.length == 0)
                html.push("<span class=\"searchbox menu-active filterBar-item filterBar-none\">显示全部</span>");
            return html.join('');
        },
        barOtherMoreRander: function (view, filterData, filterBarHandle) {
            if (!view || !filterData) return;
            var t = null, html = [], col = null;
            for (var name in filterData) {
                if (!!(t = filterData[name])) {
                    col = view.datagrid("getColumnOption", name);
                    if (!col || (!!col.filterBar && col.filterBar !== false)) continue;
                    if (t.Value != null) {
                        html.push("<span class=\"searchbox menu-active filterBar-item\"><b>[" + col.title + "]</b>:");
                        if (erp.utils.isArray(t.Value) && t.Value.length > 0) {

                            for (var m = 0; m < t.Value.length - 1; m++) {
                                html.push(erp.filter.getValueFormat(t.Value[m], col.formatter) + ",");
                            }
                            html.push(erp.filter.getValueFormat(t.Value[t.Value.length - 1], col.formatter));
                        } else {
                            html.push(erp.filter.getValueFormat(t.Value, col.formatter));
                        }
                        html.push("<a href=\"javascript:void(0);\" class=\"filterBar-close\" onclick=\"erp." + filterBarHandle + "(this,'" + name + "',{'Value':null})\">×</a></span>");
                    } else if (t.StartValue != null || t.EndValue != null && t.StartValue != undefined && t.EndValue != undefined) {
                        html.push("<span class=\"searchbox menu-active filterBar-item\"><b>[" + col.title + "]</b>:");
                        if (typeof (t.StartValue) != "string" && typeof (t.EndValue) != "string") {
                            html.push((t.StartValue == undefined || t.StartValue == null) ? "∞" : erp.filter.getValueFormat(t.StartValue, col.formatter));
                            html.push("至");
                            html.push((t.EndValue == undefined || t.EndValue == null) ? "∞" : erp.filter.getValueFormat(t.EndValue, col.formatter));
                        } else {
                            html.push((t.StartValue == undefined || t.StartValue == null) ? "∞" : t.StartValue);
                            html.push("至");
                            html.push((t.EndValue == undefined || t.EndValue == null) ? "∞" : t.EndValue);
                        }
                        html.push("<a href=\"javascript:void(0);\" class=\"filterBar-close\" onclick=\"erp." + filterBarHandle + "(this,'" + name + "',{'StartValue':null,'EndValue':null})\">×</a></span>");
                    }
                }
            }
            if (html.length == 0)
                html.push("<span class=\"searchbox menu-active filterBar-item filterBar-none\">显示全部</span>");
            return html.join('');
        }
    },
    download: function (url, data) {
        if (typeof (data) === "string" && data.length > 0) {
            $.error("data must not be serialized");
        }
        var method = ($.browser.mozilla || $.browser.msie) ? "get" : method || "post";
        var form = $("<form></form>"),
            iframe = $("#___dowmload");
        if (iframe.length == 0) { //onload='erp.downloadHook()'
            $("<div style='display:none'><iframe src='javascript:false;' style='display:none' id='___dowmload' name='___dowmload' ></iframe><div>").appendTo($("body"));
            iframe = $("#___dowmload");
            iframe.contents().find("header").append("<base target='_blank'>");

        }
        if (method[0] = 'p') {
            $("<input type='hidden' value='IFrame' name='X-Requested-With' />").appendTo(form);
            $("<input type='hidden' name='X-HTTP-Accept'>").attr("value", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8").appendTo(form);
        }
        $.each(data || {}, function (name, value) {
            if ($.isPlainObject(value)) {
                name = value.name;
                value = value.value;
            }
            $("<input type='hidden' />").attr({ name: name, value: value }).appendTo(form);
        });
        iframe.contents().find("body").html(form);
        iframe.load(function () {
            var response = iframe.contents();
            try {
                response = $.parseJSON(response.html());
                if (!response) response = _errorFormat();

            } catch (e) {
                var text = response.find("title").text();
                if (text.length > 1) {
                    erp.showError(text);
                }
            }
            iframe.unbind('load');
            setTimeout(function () {
                iframe.contents().html('');
            }, 1);
        });
        form.attr({ "action": url, "method": method });
        form[0].submit();
    }
});

$.extend(Array.prototype, {
    insert: function (index, item) {
        this.splice(index, 0, item);
    },
    each: function (iterator, context) {
        if (!context) context = this;
        if (this.length === +this.length) {
            for (var i = 0, l = this.length; i < l; i++) {
                if (iterator.call(context, i, this[i], this) === false)
                    return false;
            }
        } else {
            for (var key in obj) {
                if (this.hasOwnProperty(key)) {
                    if (iterator.call(context, key, this[key], this) === false)
                        return false;
                }
            }
        }
    },
    removeAt: function (index) {
        if (index < 0)
            return;
        var newArray = this.slice(0, index).concat(this.slice(index + 1, this.length));
        this.length = 0; //clear source array
        this.push.apply(this, newArray); //add
    },
    remove: function (itemOrCallback, context) {
        if (!itemOrCallback || this.length == 0) { return this; }
        if (typeof (itemOrCallback) == "function") {
            if (!context) context = this;
            for (var i = 0; i < this.length; i++) {
                if (itemOrCallback.call(context, i, this[i]) === true) {
                    this.splice(i, 1);
                    break;
                }
            }
        } else {
            for (var i = 0; i < this.length; i++) {
                if (this[i] == itemOrCallback) {
                    this.splice(i, 1);
                    break;
                }
            }
        }
        return this;
    },
    removeAll: function (itemOrCallback, context) {
        if (!itemOrCallback || this.length == 0) { return this; }
        if (typeof (itemOrCallback) == "function") {
            if (!context) context = this;
            for (var i = 0, n = this.length; i < n;) {
                if (itemOrCallback.call(context, i, this[i]) === true) {
                    this.splice(i, 1);
                    n--;
                } else {
                    i++;
                }
            }
        } else {
            for (var i = 0, n = this.length; i < n;) {
                if (this[i] == itemOrCallback) {
                    this.splice(i, 1);
                    n--;
                } else {
                    i++;
                }
            }
        }
        return this;
    },
    contains: function (itemOrFilter, context) {
        if (this.length == 0) return false;
        if (!$.isFunction(itemOrFilter)) {
            return this.indexOf(itemOrFilter) > -1;
        }
        if (!context) context = this;
        for (var i = 0; i < this.length; i++) {
            if (itemOrFilter.call(context, i, this[i]) === true) {
                return true;
            }
        }
        return false;
    },
    firstOrDefault: function (filter, defaultValue, context) {
        if (this.length == 0) return defaultValue;
        if (!$.isFunction(filter)) { return this[0]; }
        if (!context) context = this;
        for (var i = 0; i < this.length; i++) {
            if (filter.call(context, i, this[i]) === true) {
                return this[i];
            }
        }
        return defaultValue;
    },
    first: function (filter, context) {
        if (this.length == 0) return null;
        if (!$.isFunction(filter)) { return this[0]; }
        if (!context) context = this;
        for (var i = 0; i < this.length; i++) {
            if (filter.call(context, i, this[i]) === true) {
                return this[i];
            }
        }
        return null;
    },
    lastOrDefault: function (filter, defaultValue, context) {
        if (this.length == 0) return defaultValue;
        if (!$.isFunction(filter)) { return this[this.length - 1]; }
        if (!context) context = this;
        for (var i = this.length - 1; i >= 0; i--) {
            if (filter.call(context, i, this[i]) === true) {
                return this[i];
            }
        }
        return defaultValue;
    },
    last: function (filter, context) {
        if (this.length == 0) return null;
        if (!$.isFunction(filter)) { return this[this.length - 1]; }
        if (!context) context = this;
        for (var i = this.length - 1; i >= 0; i--) {
            if (filter.call(context, i, this[i]) === true) {
                return this[i];
            }
        }
        return null;
    },
    filter: function (filter, context) {
        var arr = [], item, r;
        if (this.length == 0 || !$.isFunction(filter)) return arr;
        if (!context) context = this;
        for (var i = 0; i < this.length; i++) {
            item = this[i];
            r = filter.call(context, i, item);
            if (r == true) {
                arr.push(item);
            }
            else if (r == undefined) {
                break;
            }
        }
        return arr;
    },
    map: function (iterator, context) {
        var arr = [], item, r;
        if (this.length == 0 || !$.isFunction(iterator)) return arr;
        if (!context) context = this;
        for (var i = 0; i < this.length; i++) {
            r = iterator.call(context, i, item = this[i]);
            if (r !== null) {
                arr.push(r);
            }
        }
        return arr;
    },
    //进行迭代判定的函数,wangyunpeng
    foreach: function (fn) {
        fn = fn || Function.K;
        var a = [];
        var args = Array.prototype.slice.call(arguments, 1);
        for (var i = 0; i < this.length; i++) {
            var res = fn.apply(this, [this[i], i].concat(args));
            if (res != null) a.push(res);
        }
        return a;
    },
    //得到一个数组不重复的元素集合,wangyunpeng
    uniquelize: function () {
        var ra = new Array();
        for (var i = 0; i < this.length; i++) {
            if (!ra.contains(this[i])) {
                ra.push(this[i]);
            }
        }
        return ra;
    }
});
//两个数组的交集,wangyunpeng
Array.intersect = function (a, b) {
    return a.uniquelize().foreach(function (o) { return b.contains(o) ? o : null });
};
//两个数组的补集,wangyunpeng
Array.complement = function (a, b) {
    return Array.minus(Array.union(a, b), Array.intersect(a, b));
};
//两个数组的差集,wangyunpeng
Array.minus = function (a, b) {
    return a.uniquelize().foreach(function (o) { return b.contains(o) ? null : o });
};
//两个数组的并集,wangyunpeng
Array.union = function (a, b) {
    return a.concat(b).uniquelize();
};

Date.prototype.format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份       
        "d+": this.getDate(), //日       
        "h+": this.getHours(), // % 12 == 0 ? 12 : this.getHours() % 12, //12小时       
        "H+": this.getHours(), //小时       
        "m+": this.getMinutes(), //分       
        "s+": this.getSeconds(), //秒       
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度       
        "S": this.getMilliseconds() //毫秒       
    };
    var week = {
        "0": "日",
        "1": "一",
        "2": "二",
        "3": "三",
        "4": "四",
        "5": "五",
        "6": "六"
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    if (/(E+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "星期" : "周") : "") + week[this.getDay() + ""]);
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
};


$.extend(String.prototype, {
    //isUrl: erp.fn.isUrl,
    //isNullOrEmpty: erp.fn.isNullOrEmpty,
    //isInteger: erp.fn.isInteger,
    //isColor: erp.fn.isColor,
    //toBoolean: erp.fn.toBoolean,
    //toInteger: erp.fn.toInteger,
    //toNumber: erp.fn.toNumber,
    //toFloat: erp.fn.toFloat,
    //toObject: erp.fn.toObject,
    //toFunction: erp.fn.toFunction,
    getLength: function () {
        return this.replace(/[^\x00-\xff]/g, "aa").length;
    }
});