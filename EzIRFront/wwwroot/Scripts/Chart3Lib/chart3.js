var Chart3UI = (function () {
    var _defaultConfig = {
        id:"tv_chart_container",
        regexSearch: /(\?s=\w+|\?s=\d+|\&s=\w+|\&s=\d+)/g,
        chart3r: "/Scripts/Chart3Lib/chart3r.html",
        chart3api: "https://chart.fpts.com.vn/chart3api",
        autosize: true,
        height: '100%',
        interval: 'D',
        symbol: "FTS",
        theme: "light",
        //toolbar_bg: '#222',
        timezone: "exchange",
        library_path: "/Scripts/Chart3Lib/charting_library/",
        //drawings_access: { type: 'black', tools: [{ name: "Trend Line" }] },
        disabled_features: [],
        //enabled_features: ["left_toolbar", "show_left_toolbar_by_default"],
        charts_storage_api_version: "1.1",
        charts_storage_url: "/chart3api",
        client_id: "",
        user_id: "",
        load_last_chart: false,
        loading_screen: { backgroundColor: '#000' },
        timeInterval: 5 * 1000,
        allow_symbol_change: true,
    };

    var _dataFeeds = {};
    var _widget = {};
    var _iframes = {};

    function Chart3UI(config) {
        if (config) mergeConfig(config);
    }

    var getDisableFeatures = function (isRoot) {
        if (_defaultConfig.disabled_features.length > 0) return _defaultConfig.disabled_features;
        return isRoot
            ? ["use_localstorage_for_settings", "header_compare", "timeframes_toolbar", "control_bar", "header_screenshot"]
            : ["use_localstorage_for_settings", "header_compare", "timeframes_toolbar", "control_bar", "left_toolbar", "header_screenshot"];
    }
    // tao datafeed
    var createDataFeed = function (name) {
        var dataFeed = new Datafeeds.UDFCompatibleDatafeed(_defaultConfig.chart3api);
        _dataFeeds[name] = dataFeed;
        window[name] = dataFeed;
        return dataFeed;
    }

    var getDataFeed = function (name) {
        var datafeed = _dataFeeds[name];
        if (datafeed) return datafeed;
        return null;
    }

    var removeDataFeed = function (name) {
        if (_dataFeeds[name]) delete _dataFeeds[name];
    }

    // tao widget
    var createWidget = function (name, opts) {
        var widget = new TradingView.widget(opts);
        _widget[name] = widget;
        return widget;
    }

    var getWidget = function (name) {
        var w = _widget[name];
        if (w) return w;
        return null;
    }

    var removeWidget = function (name) {
        if (_widget[name]) delete _widget[name];
    }

    var getEnableFutures = function (isRoot) {
        return _defaultConfig.enabled_features;
    }

    var getOptions = function (symbol, id, isRoot, datafeed, lang) {
        var disable_features = getDisableFeatures(isRoot);
      
        var enabled = getEnableFutures();
        return {
            autosize: _defaultConfig.autosize,
            height: _defaultConfig.height,
            symbol: symbol,
            interval: _defaultConfig.interval.toUpperCase(),
            //toolbar_bg: _defaultConfig.toolbar_bg,
            theme: _defaultConfig.theme,
            allow_symbol_change: true,//_defaultConfig.allow_symbol_change,
            timezone: _defaultConfig.timezone,
            container_id: id,
            datafeed: datafeed,
            library_path: _defaultConfig.library_path,
            locale: getParameterByName('lang') || lang,
            disabled_features: ["use_localstorage_for_settings", "volume_force_overlay"],
            enabled_features: ["move_logo_to_main_pane", "snapshot_trading_drawings", "side_toolbar_in_fullscreen_mode"],
            overrides: {
                "paneProperties.vertGridProperties.color": "rgb(216, 216, 216)",
                "paneProperties.vertGridProperties.style": 1,
                "paneProperties.horzGridProperties.color": "rgb(216, 216, 216)",
                "paneProperties.horzGridProperties.style": 1,
                "mainSeriesProperties.candleStyle.drawWick": true,
                "mainSeriesProperties.candleStyle.drawBorder": true,
                "mainSeriesProperties.candleStyle.borderColor": "rgb(56, 118, 29)",
                "mainSeriesProperties.candleStyle.borderUpColor": "rgb(56, 118, 29)",
                "mainSeriesProperties.candleStyle.borderDownColor": "rgb(153, 0, 0)",
                "mainSeriesProperties.candleStyle.wickColor": "rgba( 115, 115, 117, 1)",
                "mainSeriesProperties.candleStyle.wickDownColor": "rgba(153, 0, 0, 0.8)",
                "mainSeriesProperties.candleStyle.wickUpColor": "rgba(56, 118, 29, 0.79)",
                "mainSeriesProperties.pbStyle.upColor": "red",
                "mainSeriesProperties.pbStyle.downColor": "green",
            },
       
            debug: false,
            time_frames: [
                { text: "50y", resolution: "6M" },
                { text: "1d", resolution: "5" },
            ],
            charts_storage_api_version: _defaultConfig.charts_storage_api_version,
            charts_storage_url: _defaultConfig.charts_storage_url,
            drawings_access: _defaultConfig.drawings_access,
            //disabled_features: disable_features,
            enabled_features: enabled,
     
            client_id: getValueInput("xPublicID"),
            user_id: getValueInput("xUserID"),
            favorites: {
                intervals: ["1D", "3D", "3W", "1W", "1M"],
                chartTypes: ["Area", "Line"]
            },
            load_last_chart: _defaultConfig.load_last_chart,
            //loading_screen: _defaultConfig.loading_screen
        }
    }

    var mergeConfig = function (config) {
        var keys = Object.keys(config);
        for (var i = 0; i < keys.length; i++) {
            if (config[keys[i]]) _defaultConfig[keys[i]] = config[keys[i]];
        }
    }

    var CheckLocalStorage = function() {
        if (typeof (Storage) !== "undefined") {
            return true;
        } else {
            alert("Brower not support Storage");
            return false;
        }
    }

    var SetItem = function (key, value) {
        if (CheckLocalStorage()) {
            window.localStorage.setItem(key, value);
        }
    }

    var GetItem = function (key) {
        if (CheckLocalStorage()) {
            return window.localStorage.getItem(key);
        }
    }

    var createHubForChart = function (url, dataFeedName, iframeName) {
        var iframe = document.createElement("iframe");
        iframe.id = iframeName;
        iframe.name = iframeName;
        iframe.style.display = "none";
        iframe.src = url + "?datafeed=" + dataFeedName;
        _iframes[iframeName] = iframe;
        document.body.append(iframe);
    }

    var getParameterByName = function (name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }

    var getValueInput = function (id) {
        var input = document.getElementById(id);
        if (input == null) return "publish_user";
        return input.value;
    }

    var GetSearch = function () {
        var query = window.location.search;
        if (query != "") {
            var arr = query.match(_defaultConfig.regexSearch);
            if (arr != null && arr.length == 1) {
                return arr[0].replace("?s=", "").replace("&s=", "");
            }
        }
        return "VN30F1M";
    }

    var getDataFeed = function (name) {
        var datafeed = _dataFeeds[name];
        if (datafeed) return datafeed;
        return null;
    }

    Chart3UI.prototype.GetDataFeedFirst = function () {
        var keys = Object.keys(_dataFeeds);
        if (keys) {
            if (keys.length > 0) {
                _dataFeeds[keys[0]];
            }
            return null;
        }
        return null;
    }

    Chart3UI.prototype.GetWidgetFirst = function () {
        var keys = Object.keys(_widget);
        console.log(keys, keys.length, keys[0]);
        if (keys) {
            if (keys.length > 0) {
                var firstKey = keys[0];
                return getWidget(firstKey);
            }
            return null;
        }
        return null;
    }

    Chart3UI.prototype.GetDataFeed = function (name) {
        return getDataFeed(name);
    }

    Chart3UI.prototype.GetWidget = function (name) {
        return getWidget(name);
    }

    Chart3UI.prototype.Create = function (symbol, isRoot, style, lang) {        
        var root = document.getElementById(_defaultConfig.id);
       // console.log(root);
        if (root) {
            var datafeedName = "datafeed_" + symbol, widgetName = "widget_" + symbol;

            var div = document.createElement("div");
            div.id = "iframe_" + symbol;
            div.style = "height:451px";
           // div.setAttribute("style", style);
            var x = root.querySelector("[id='" + div.id + "']");
            if (x) {
                x.innerHTML = "";
            }
            else {
                root.appendChild(div);
            }
            if (TradingView) {
                var datafeed = createDataFeed(datafeedName);
                var opt = getOptions(symbol, div.id, isRoot, datafeed, lang);
                datafeed.onReady(function () {
                    window[datafeedName] = datafeed;
                    createHubForChart(_defaultConfig.chart3r, datafeedName, symbol + "_" + opt.interval);
                });
                return createWidget(widgetName, opt);
            }
        }
    }

    Chart3UI.prototype.GetFirstIframe = function () {
        var keys = Object.keys(_iframes);
        if (keys) {
            return keys.length > 0 ? keys[0] : null;
        }
        return null;
    }

    Chart3UI.prototype.Destroy = function (symbol) {
        if (!symbol) {
            var root = document.getElementById(_defaultConfig.id);
            if (root) {
                root.innerHTML = "";
            }
            for (var i in _dataFeeds) {
                delete window[i];
                delete _dataFeeds[i];
            }
            for (var i in _widget) {
                delete _widget[i];
            }
            for (var i in _iframes) {
                var f = document.getElementById(i);
                if (f) {
                    f.remove();
                }
                delete _iframes[i];
            }
        } else {
            var root = document.getElementById("iframe_" + symbol);
            if (root) {
                root.remove();
                delete _dataFeeds["datafeed_" + symbol];
                delete _widget["widget_" + symbol];
                var f = document.getElementById(symbol + "_1");
                if (f) {
                    f.remove();
                }
                delete _iframes[symbol+"_1"];
            }
        }
        
    }

    return Chart3UI;
}());

var g_Chart3UI = new Chart3UI();
