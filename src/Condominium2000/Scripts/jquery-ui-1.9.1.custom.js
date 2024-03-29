/*! jQuery UI - v1.9.1 - 2012-11-03
* http://jqueryui.com
* Includes: jquery.ui.core.js, jquery.ui.widget.js, jquery.ui.mouse.js, jquery.ui.position.js, jquery.ui.tabs.js, jquery.ui.tooltip.js
* Copyright (c) 2012 jQuery Foundation and other contributors Licensed MIT */

(function($, undefined) {

    var uuid = 0,
        runiqueId = /^ui-id-\d+$/;

// prevent duplicate loading
// this is only a problem because we proxy existing functions
// and we don't want to double proxy them
    $.ui = $.ui || {};
    if ($.ui.version) {
        return;
    }

    $.extend($.ui,
    {
        version: "1.9.1",

        keyCode: {
            BACKSPACE: 8,
            COMMA: 188,
            DELETE: 46,
            DOWN: 40,
            END: 35,
            ENTER: 13,
            ESCAPE: 27,
            HOME: 36,
            LEFT: 37,
            NUMPAD_ADD: 107,
            NUMPAD_DECIMAL: 110,
            NUMPAD_DIVIDE: 111,
            NUMPAD_ENTER: 108,
            NUMPAD_MULTIPLY: 106,
            NUMPAD_SUBTRACT: 109,
            PAGE_DOWN: 34,
            PAGE_UP: 33,
            PERIOD: 190,
            RIGHT: 39,
            SPACE: 32,
            TAB: 9,
            UP: 38
        }
    });

// plugins
    $.fn.extend({
        _focus: $.fn.focus,
        focus: function(delay, fn) {
            return typeof delay === "number"
                ? this.each(function() {
                    var elem = this;
                    setTimeout(function() {
                            $(elem).focus();
                            if (fn) {
                                fn.call(elem);
                            }
                        },
                        delay);
                })
                : this._focus.apply(this, arguments);
        },

        scrollParent: function() {
            var scrollParent;
            if (($.ui
                .ie &&
                (/(static|relative)/).test(this.css("position"))) ||
            (/absolute/).test(this.css("position"))) {
                scrollParent = this.parents()
                    .filter(function() {
                        return (/(relative|absolute|fixed)/).test($.css(this, "position")) &&
                        (/(auto|scroll)/).test($.css(this, "overflow") +
                            $.css(this, "overflow-y") +
                            $.css(this, "overflow-x"));
                    })
                    .eq(0);
            } else {
                scrollParent = this.parents()
                    .filter(function() {
                        return (/(auto|scroll)/).test($.css(this, "overflow") +
                            $.css(this, "overflow-y") +
                            $.css(this, "overflow-x"));
                    })
                    .eq(0);
            }

            return (/fixed/).test(this.css("position")) || !scrollParent.length ? $(document) : scrollParent;
        },

        zIndex: function(zIndex) {
            if (zIndex !== undefined) {
                return this.css("zIndex", zIndex);
            }

            if (this.length) {
                var elem = $(this[0]), position, value;
                while (elem.length && elem[0] !== document) {
                    // Ignore z-index if position is set to a value where z-index is ignored by the browser
                    // This makes behavior of this function consistent across browsers
                    // WebKit always returns auto if the element is positioned
                    position = elem.css("position");
                    if (position === "absolute" || position === "relative" || position === "fixed") {
                        // IE returns 0 when zIndex is not specified
                        // other browsers return a string
                        // we ignore the case of nested elements with an explicit value of 0
                        // <div style="z-index: -10;"><div style="z-index: 0;"></div></div>
                        value = parseInt(elem.css("zIndex"), 10);
                        if (!isNaN(value) && value !== 0) {
                            return value;
                        }
                    }
                    elem = elem.parent();
                }
            }

            return 0;
        },

        uniqueId: function() {
            return this.each(function() {
                if (!this.id) {
                    this.id = "ui-id-" + (++uuid);
                }
            });
        },

        removeUniqueId: function() {
            return this.each(function() {
                if (runiqueId.test(this.id)) {
                    $(this).removeAttr("id");
                }
            });
        }
    });

// support: jQuery <1.8
    if (!$("<a>").outerWidth(1).jquery) {
        $.each(["Width", "Height"],
            function(i, name) {
                var side = name === "Width" ? ["Left", "Right"] : ["Top", "Bottom"],
                    type = name.toLowerCase(),
                    orig = {
                        innerWidth: $.fn.innerWidth,
                        innerHeight: $.fn.innerHeight,
                        outerWidth: $.fn.outerWidth,
                        outerHeight: $.fn.outerHeight
                    };

                function reduce(elem, size, border, margin) {
                    $.each(side,
                        function() {
                            size -= parseFloat($.css(elem, "padding" + this)) || 0;
                            if (border) {
                                size -= parseFloat($.css(elem, "border" + this + "Width")) || 0;
                            }
                            if (margin) {
                                size -= parseFloat($.css(elem, "margin" + this)) || 0;
                            }
                        });
                    return size;
                }

                $.fn["inner" + name] = function(size) {
                    if (size === undefined) {
                        return orig["inner" + name].call(this);
                    }

                    return this.each(function() {
                        $(this).css(type, reduce(this, size) + "px");
                    });
                };

                $.fn["outer" + name] = function(size, margin) {
                    if (typeof size !== "number") {
                        return orig["outer" + name].call(this, size);
                    }

                    return this.each(function() {
                        $(this).css(type, reduce(this, size, true, margin) + "px");
                    });
                };
            });
    }

// selectors
    function focusable(element, isTabIndexNotNaN) {
        var map,
            mapName,
            img,
            nodeName = element.nodeName.toLowerCase();
        if ("area" === nodeName) {
            map = element.parentNode;
            mapName = map.name;
            if (!element.href || !mapName || map.nodeName.toLowerCase() !== "map") {
                return false;
            }
            img = $("img[usemap=#" + mapName + "]")[0];
            return !!img && visible(img);
        }
        return (/input|select|textarea|button|object/.test(nodeName)
                ? !element.disabled
                : "a" === nodeName ? element.href || isTabIndexNotNaN : isTabIndexNotNaN) &&
            // the element and all of its ancestors must be visible
            visible(element);
    }

    function visible(element) {
        return $.expr.filters.visible(element) &&
            !$(element)
            .parents()
            .andSelf()
            .filter(function() {
                return $.css(this, "visibility") === "hidden";
            })
            .length;
    }

    $.extend($.expr[":"],
    {
        data: $.expr.createPseudo
            ? $.expr.createPseudo(function(dataName) {
                return function(elem) {
                    return !!$.data(elem, dataName);
                };
            })
            :
            // support: jQuery <1.8
            function(elem, i, match) {
                return !!$.data(elem, match[3]);
            },

        focusable: function(element) {
            return focusable(element, !isNaN($.attr(element, "tabindex")));
        },

        tabbable: function(element) {
            var tabIndex = $.attr(element, "tabindex"),
                isTabIndexNaN = isNaN(tabIndex);
            return (isTabIndexNaN || tabIndex >= 0) && focusable(element, !isTabIndexNaN);
        }
    });

// support
    $(function() {
        var body = document.body,
            div = body.appendChild(div = document.createElement("div"));

        // access offsetHeight before setting the style to prevent a layout bug
        // in IE 9 which causes the element to continue to take up space even
        // after it is removed from the DOM (#8026)
        div.offsetHeight;

        $.extend(div.style,
        {
            minHeight: "100px",
            height: "auto",
            padding: 0,
            borderWidth: 0
        });

        $.support.minHeight = div.offsetHeight === 100;
        $.support.selectstart = "onselectstart" in div;

        // set display to none to avoid a layout bug in IE
        // http://dev.jquery.com/ticket/4014
        body.removeChild(div).style.display = "none";
    });


// deprecated

    (function() {
        var uaMatch = /msie ([\w.]+)/.exec(navigator.userAgent.toLowerCase()) || [];
        $.ui.ie = uaMatch.length ? true : false;
        $.ui.ie6 = parseFloat(uaMatch[1], 10) === 6;
    })();

    $.fn.extend({
        disableSelection: function() {
            return this.bind(($.support.selectstart ? "selectstart" : "mousedown") +
                ".ui-disableSelection",
                function(event) {
                    event.preventDefault();
                });
        },

        enableSelection: function() {
            return this.unbind(".ui-disableSelection");
        }
    });

    $.extend($.ui,
    {
        // $.ui.plugin is deprecated.  Use the proxy pattern instead.
        plugin: {
            add: function(module, option, set) {
                var i,
                    proto = $.ui[module].prototype;
                for (i in set) {
                    proto.plugins[i] = proto.plugins[i] || [];
                    proto.plugins[i].push([option, set[i]]);
                }
            },
            call: function(instance, name, args) {
                var i,
                    set = instance.plugins[name];
                if (!set || !instance.element[0].parentNode || instance.element[0].parentNode.nodeType === 11) {
                    return;
                }

                for (i = 0; i < set.length; i++) {
                    if (instance.options[set[i][0]]) {
                        set[i][1].apply(instance.element, args);
                    }
                }
            }
        },

        contains: $.contains,

        // only used by resizable
        hasScroll: function(el, a) {

            //If overflow is hidden, the element might have extra content, but the user wants to hide it
            if ($(el).css("overflow") === "hidden") {
                return false;
            }

            var scroll = (a && a === "left") ? "scrollLeft" : "scrollTop",
                has = false;

            if (el[scroll] > 0) {
                return true;
            }

            // TODO: determine which cases actually cause this to happen
            // if the element doesn't have the scroll set, see if it's possible to
            // set the scroll
            el[scroll] = 1;
            has = (el[scroll] > 0);
            el[scroll] = 0;
            return has;
        },

        // these are odd functions, fix the API or move into individual plugins
        isOverAxis: function(x, reference, size) {
            //Determines when x coordinate is over "b" element axis
            return (x > reference) && (x < (reference + size));
        },
        isOver: function(y, x, top, left, height, width) {
            //Determines when x, y coordinates is over "b" element
            return $.ui.isOverAxis(y, top, height) && $.ui.isOverAxis(x, left, width);
        }
    });

})(jQuery);
(function($, undefined) {

    var uuid = 0,
        slice = Array.prototype.slice,
        _cleanData = $.cleanData;
    $.cleanData = function(elems) {
        for (var i = 0, elem; (elem = elems[i]) != null; i++) {
            try {
                $(elem).triggerHandler("remove");
                // http://bugs.jquery.com/ticket/8235
            } catch (e) {
            }
        }
        _cleanData(elems);
    };

    $.widget = function(name, base, prototype) {
        var fullName,
            existingConstructor,
            constructor,
            basePrototype,
            namespace = name.split(".")[0];

        name = name.split(".")[1];
        fullName = namespace + "-" + name;

        if (!prototype) {
            prototype = base;
            base = $.Widget;
        }

        // create selector for plugin
        $.expr[":"][fullName.toLowerCase()] = function(elem) {
            return !!$.data(elem, fullName);
        };

        $[namespace] = $[namespace] || {};
        existingConstructor = $[namespace][name];
        constructor = $[namespace][name] = function(options, element) {
            // allow instantiation without "new" keyword
            if (!this._createWidget) {
                return new constructor(options, element);
            }

            // allow instantiation without initializing for simple inheritance
            // must use "new" keyword (the code above always passes args)
            if (arguments.length) {
                this._createWidget(options, element);
            }
        };
        // extend with the existing constructor to carry over any static properties
        $.extend(constructor,
            existingConstructor,
            {
                version: prototype.version,
                // copy the object used to create the prototype in case we need to
                // redefine the widget later
                _proto: $.extend({}, prototype),
                // track widgets that inherit from this widget in case this widget is
                // redefined after a widget inherits from it
                _childConstructors: []
            });

        basePrototype = new base();
        // we need to make the options hash a property directly on the new instance
        // otherwise we'll modify the options hash on the prototype that we're
        // inheriting from
        basePrototype.options = $.widget.extend({}, basePrototype.options);
        $.each(prototype,
            function(prop, value) {
                if ($.isFunction(value)) {
                    prototype[prop] = (function() {
                        var _super = function() {
                                return base.prototype[prop].apply(this, arguments);
                            },
                            _superApply = function(args) {
                                return base.prototype[prop].apply(this, args);
                            };
                        return function() {
                            var __super = this._super,
                                __superApply = this._superApply,
                                returnValue;

                            this._super = _super;
                            this._superApply = _superApply;

                            returnValue = value.apply(this, arguments);

                            this._super = __super;
                            this._superApply = __superApply;

                            return returnValue;
                        };
                    })();
                }
            });
        constructor.prototype = $.widget.extend(basePrototype,
            {
                // TODO: remove support for widgetEventPrefix
                // always use the name + a colon as the prefix, e.g., draggable:start
                // don't prefix for widgets that aren't DOM-based
                widgetEventPrefix: basePrototype.widgetEventPrefix || name
            },
            prototype,
            {
                constructor: constructor,
                namespace: namespace,
                widgetName: name,
                // TODO remove widgetBaseClass, see #8155
                widgetBaseClass: fullName,
                widgetFullName: fullName
            });

        // If this widget is being redefined then we need to find all widgets that
        // are inheriting from it and redefine all of them so that they inherit from
        // the new version of this widget. We're essentially trying to replace one
        // level in the prototype chain.
        if (existingConstructor) {
            $.each(existingConstructor._childConstructors,
                function(i, child) {
                    var childPrototype = child.prototype;

                    // redefine the child widget using the same prototype that was
                    // originally used, but inherit from the new version of the base
                    $.widget(childPrototype.namespace + "." + childPrototype.widgetName, constructor, child._proto);
                });
            // remove the list of existing child constructors from the old constructor
            // so the old child constructors can be garbage collected
            delete existingConstructor._childConstructors;
        } else {
            base._childConstructors.push(constructor);
        }

        $.widget.bridge(name, constructor);
    };

    $.widget.extend = function(target) {
        var input = slice.call(arguments, 1),
            inputIndex = 0,
            inputLength = input.length,
            key,
            value;
        for (; inputIndex < inputLength; inputIndex++) {
            for (key in input[inputIndex]) {
                value = input[inputIndex][key];
                if (input[inputIndex].hasOwnProperty(key) && value !== undefined) {
                    // Clone objects
                    if ($.isPlainObject(value)) {
                        target[key] = $.isPlainObject(target[key])
                            ? $.widget.extend({}, target[key], value)
                            :
                            // Don't extend strings, arrays, etc. with objects
                            $.widget.extend({}, value);
                        // Copy everything else by reference
                    } else {
                        target[key] = value;
                    }
                }
            }
        }
        return target;
    };

    $.widget.bridge = function(name, object) {
        var fullName = object.prototype.widgetFullName;
        $.fn[name] = function(options) {
            var isMethodCall = typeof options === "string",
                args = slice.call(arguments, 1),
                returnValue = this;

            // allow multiple hashes to be passed on init
            options = !isMethodCall && args.length ? $.widget.extend.apply(null, [options].concat(args)) : options;

            if (isMethodCall) {
                this.each(function() {
                    var methodValue,
                        instance = $.data(this, fullName);
                    if (!instance) {
                        return $.error("cannot call methods on " +
                            name +
                            " prior to initialization; " +
                            "attempted to call method '" +
                            options +
                            "'");
                    }
                    if (!$.isFunction(instance[options]) || options.charAt(0) === "_") {
                        return $.error("no such method '" + options + "' for " + name + " widget instance");
                    }
                    methodValue = instance[options].apply(instance, args);
                    if (methodValue !== instance && methodValue !== undefined) {
                        returnValue = methodValue && methodValue.jquery
                            ? returnValue.pushStack(methodValue.get())
                            : methodValue;
                        return false;
                    }
                });
            } else {
                this.each(function() {
                    var instance = $.data(this, fullName);
                    if (instance) {
                        instance.option(options || {})._init();
                    } else {
                        new object(options, this);
                    }
                });
            }

            return returnValue;
        };
    };

    $.Widget = function(/* options, element */ ) {};
    $.Widget._childConstructors = [];

    $.Widget.prototype = {
        widgetName: "widget",
        widgetEventPrefix: "",
        defaultElement: "<div>",
        options: {
            disabled: false,

            // callbacks
            create: null
        },
        _createWidget: function(options, element) {
            element = $(element || this.defaultElement || this)[0];
            this.element = $(element);
            this.uuid = uuid++;
            this.eventNamespace = "." + this.widgetName + this.uuid;
            this.options = $.widget.extend({},
                this.options,
                this._getCreateOptions(),
                options);

            this.bindings = $();
            this.hoverable = $();
            this.focusable = $();

            if (element !== this) {
                // 1.9 BC for #7810
                // TODO remove dual storage
                $.data(element, this.widgetName, this);
                $.data(element, this.widgetFullName, this);
                this._on(this.element,
                {
                    remove: function(event) {
                        if (event.target === element) {
                            this.destroy();
                        }
                    }
                });
                this.document = $(element.style
                    ?
                    // element within the document
                    element.ownerDocument
                    :
                    // element is window or document
                    element.document || element);
                this.window = $(this.document[0].defaultView || this.document[0].parentWindow);
            }

            this._create();
            this._trigger("create", null, this._getCreateEventData());
            this._init();
        },
        _getCreateOptions: $.noop,
        _getCreateEventData: $.noop,
        _create: $.noop,
        _init: $.noop,

        destroy: function() {
            this._destroy();
            // we can probably remove the unbind calls in 2.0
            // all event bindings should go through this._on()
            this.element
                .unbind(this.eventNamespace)
                // 1.9 BC for #7810
                // TODO remove dual storage
                .removeData(this.widgetName)
                .removeData(this.widgetFullName)
                // support: jquery <1.6.3
                // http://bugs.jquery.com/ticket/9413
                .removeData($.camelCase(this.widgetFullName));
            this.widget()
                .unbind(this.eventNamespace)
                .removeAttr("aria-disabled")
                .removeClass(
                    this.widgetFullName +
                    "-disabled " +
                    "ui-state-disabled");

            // clean up events and states
            this.bindings.unbind(this.eventNamespace);
            this.hoverable.removeClass("ui-state-hover");
            this.focusable.removeClass("ui-state-focus");
        },
        _destroy: $.noop,

        widget: function() {
            return this.element;
        },

        option: function(key, value) {
            var options = key,
                parts,
                curOption,
                i;

            if (arguments.length === 0) {
                // don't return a reference to the internal hash
                return $.widget.extend({}, this.options);
            }

            if (typeof key === "string") {
                // handle nested keys, e.g., "foo.bar" => { foo: { bar: ___ } }
                options = {};
                parts = key.split(".");
                key = parts.shift();
                if (parts.length) {
                    curOption = options[key] = $.widget.extend({}, this.options[key]);
                    for (i = 0; i < parts.length - 1; i++) {
                        curOption[parts[i]] = curOption[parts[i]] || {};
                        curOption = curOption[parts[i]];
                    }
                    key = parts.pop();
                    if (value === undefined) {
                        return curOption[key] === undefined ? null : curOption[key];
                    }
                    curOption[key] = value;
                } else {
                    if (value === undefined) {
                        return this.options[key] === undefined ? null : this.options[key];
                    }
                    options[key] = value;
                }
            }

            this._setOptions(options);

            return this;
        },
        _setOptions: function(options) {
            var key;

            for (key in options) {
                this._setOption(key, options[key]);
            }

            return this;
        },
        _setOption: function(key, value) {
            this.options[key] = value;

            if (key === "disabled") {
                this.widget()
                    .toggleClass(this.widgetFullName + "-disabled ui-state-disabled", !!value)
                    .attr("aria-disabled", value);
                this.hoverable.removeClass("ui-state-hover");
                this.focusable.removeClass("ui-state-focus");
            }

            return this;
        },

        enable: function() {
            return this._setOption("disabled", false);
        },
        disable: function() {
            return this._setOption("disabled", true);
        },

        _on: function(element, handlers) {
            var delegateElement,
                instance = this;
            // no element argument, shuffle and use this.element
            if (!handlers) {
                handlers = element;
                element = this.element;
                delegateElement = this.widget();
            } else {
                // accept selectors, DOM elements
                element = delegateElement = $(element);
                this.bindings = this.bindings.add(element);
            }

            $.each(handlers,
                function(event, handler) {
                    function handlerProxy() {
                        // allow widgets to customize the disabled handling
                        // - disabled as an array instead of boolean
                        // - disabled class as method for disabling individual parts
                        if (instance.options.disabled === true ||
                            $(this).hasClass("ui-state-disabled")) {
                            return;
                        }
                        return (typeof handler === "string" ? instance[handler] : handler)
                            .apply(instance, arguments);
                    }

                    // copy the guid so direct unbinding works
                    if (typeof handler !== "string") {
                        handlerProxy.guid = handler.guid =
                            handler.guid || handlerProxy.guid || $.guid++;
                    }

                    var match = event.match(/^(\w+)\s*(.*)$/),
                        eventName = match[1] + instance.eventNamespace,
                        selector = match[2];
                    if (selector) {
                        delegateElement.delegate(selector, eventName, handlerProxy);
                    } else {
                        element.bind(eventName, handlerProxy);
                    }
                });
        },

        _off: function(element, eventName) {
            eventName = (eventName || "").split(" ").join(this.eventNamespace + " ") + this.eventNamespace;
            element.unbind(eventName).undelegate(eventName);
        },

        _delay: function(handler, delay) {
            function handlerProxy() {
                return (typeof handler === "string" ? instance[handler] : handler)
                    .apply(instance, arguments);
            }

            var instance = this;
            return setTimeout(handlerProxy, delay || 0);
        },

        _hoverable: function(element) {
            this.hoverable = this.hoverable.add(element);
            this._on(element,
            {
                mouseenter: function(event) {
                    $(event.currentTarget).addClass("ui-state-hover");
                },
                mouseleave: function(event) {
                    $(event.currentTarget).removeClass("ui-state-hover");
                }
            });
        },

        _focusable: function(element) {
            this.focusable = this.focusable.add(element);
            this._on(element,
            {
                focusin: function(event) {
                    $(event.currentTarget).addClass("ui-state-focus");
                },
                focusout: function(event) {
                    $(event.currentTarget).removeClass("ui-state-focus");
                }
            });
        },

        _trigger: function(type, event, data) {
            var prop,
                orig,
                callback = this.options[type];

            data = data || {};
            event = $.Event(event);
            event.type = (type === this.widgetEventPrefix ? type : this.widgetEventPrefix + type).toLowerCase();
            // the original event may come from any element
            // so we need to reset the target on the new event
            event.target = this.element[0];

            // copy original event properties over to the new event
            orig = event.originalEvent;
            if (orig) {
                for (prop in orig) {
                    if (!(prop in event)) {
                        event[prop] = orig[prop];
                    }
                }
            }

            this.element.trigger(event, data);
            return !($.isFunction(callback) &&
                callback.apply(this.element[0], [event].concat(data)) === false ||
                event.isDefaultPrevented());
        }
    };

    $.each({ show: "fadeIn", hide: "fadeOut" },
        function(method, defaultEffect) {
            $.Widget.prototype["_" + method] = function(element, options, callback) {
                if (typeof options === "string") {
                    options = { effect: options };
                }
                var hasOptions,
                    effectName = !options
                        ? method
                        : options === true || typeof options === "number"
                        ? defaultEffect
                        : options.effect || defaultEffect;
                options = options || {};
                if (typeof options === "number") {
                    options = { duration: options };
                }
                hasOptions = !$.isEmptyObject(options);
                options.complete = callback;
                if (options.delay) {
                    element.delay(options.delay);
                }
                if (hasOptions &&
                    $.effects &&
                    ($.effects.effect[effectName] || $.uiBackCompat !== false && $.effects[effectName])) {
                    element[method](options);
                } else if (effectName !== method && element[effectName]) {
                    element[effectName](options.duration, options.easing, callback);
                } else {
                    element.queue(function(next) {
                        $(this)[method]();
                        if (callback) {
                            callback.call(element[0]);
                        }
                        next();
                    });
                }
            };
        });

// DEPRECATED
    if ($.uiBackCompat !== false) {
        $.Widget.prototype._getCreateOptions = function() {
            return $.metadata && $.metadata.get(this.element[0])[this.widgetName];
        };
    }

})(jQuery);
(function($, undefined) {

    var mouseHandled = false;
    $(document)
        .mouseup(function(e) {
            mouseHandled = false;
        });

    $.widget("ui.mouse",
    {
        version: "1.9.1",
        options: {
            cancel: "input,textarea,button,select,option",
            distance: 1,
            delay: 0
        },
        _mouseInit: function() {
            var that = this;

            this.element
                .bind("mousedown." + this.widgetName,
                    function(event) {
                        return that._mouseDown(event);
                    })
                .bind("click." + this.widgetName,
                    function(event) {
                        if (true === $.data(event.target, that.widgetName + ".preventClickEvent")) {
                            $.removeData(event.target, that.widgetName + ".preventClickEvent");
                            event.stopImmediatePropagation();
                            return false;
                        }
                    });

            this.started = false;
        },

        // TODO: make sure destroying one instance of mouse doesn't mess with
        // other instances of mouse
        _mouseDestroy: function() {
            this.element.unbind("." + this.widgetName);
            if (this._mouseMoveDelegate) {
                $(document)
                    .unbind("mousemove." + this.widgetName, this._mouseMoveDelegate)
                    .unbind("mouseup." + this.widgetName, this._mouseUpDelegate);
            }
        },

        _mouseDown: function(event) {
            // don't let more than one widget handle mouseStart
            if (mouseHandled) {
                return;
            }

            // we may have missed mouseup (out of window)
            (this._mouseStarted && this._mouseUp(event));

            this._mouseDownEvent = event;

            var that = this,
                btnIsLeft = (event.which === 1),
                // event.target.nodeName works around a bug in IE 8 with
                // disabled inputs (#7620)
                elIsCancel = (typeof this.options.cancel === "string" && event.target.nodeName
                    ? $(event.target).closest(this.options.cancel).length
                    : false);
            if (!btnIsLeft || elIsCancel || !this._mouseCapture(event)) {
                return true;
            }

            this.mouseDelayMet = !this.options.delay;
            if (!this.mouseDelayMet) {
                this._mouseDelayTimer = setTimeout(function() {
                        that.mouseDelayMet = true;
                    },
                    this.options.delay);
            }

            if (this._mouseDistanceMet(event) && this._mouseDelayMet(event)) {
                this._mouseStarted = (this._mouseStart(event) !== false);
                if (!this._mouseStarted) {
                    event.preventDefault();
                    return true;
                }
            }

            // Click event may never have fired (Gecko & Opera)
            if (true === $.data(event.target, this.widgetName + ".preventClickEvent")) {
                $.removeData(event.target, this.widgetName + ".preventClickEvent");
            }

            // these delegates are required to keep context
            this._mouseMoveDelegate = function(event) {
                return that._mouseMove(event);
            };
            this._mouseUpDelegate = function(event) {
                return that._mouseUp(event);
            };
            $(document)
                .bind("mousemove." + this.widgetName, this._mouseMoveDelegate)
                .bind("mouseup." + this.widgetName, this._mouseUpDelegate);

            event.preventDefault();

            mouseHandled = true;
            return true;
        },

        _mouseMove: function(event) {
            // IE mouseup check - mouseup happened when mouse was out of window
            if ($.ui.ie && !(document.documentMode >= 9) && !event.button) {
                return this._mouseUp(event);
            }

            if (this._mouseStarted) {
                this._mouseDrag(event);
                return event.preventDefault();
            }

            if (this._mouseDistanceMet(event) && this._mouseDelayMet(event)) {
                this._mouseStarted =
                (this._mouseStart(this._mouseDownEvent, event) !== false);
                (this._mouseStarted ? this._mouseDrag(event) : this._mouseUp(event));
            }

            return !this._mouseStarted;
        },

        _mouseUp: function(event) {
            $(document)
                .unbind("mousemove." + this.widgetName, this._mouseMoveDelegate)
                .unbind("mouseup." + this.widgetName, this._mouseUpDelegate);

            if (this._mouseStarted) {
                this._mouseStarted = false;

                if (event.target === this._mouseDownEvent.target) {
                    $.data(event.target, this.widgetName + ".preventClickEvent", true);
                }

                this._mouseStop(event);
            }

            return false;
        },

        _mouseDistanceMet: function(event) {
            return (Math.max(
                        Math.abs(this._mouseDownEvent.pageX - event.pageX),
                        Math.abs(this._mouseDownEvent.pageY - event.pageY)
                    ) >=
                    this.options.distance
            );
        },

        _mouseDelayMet: function(event) {
            return this.mouseDelayMet;
        },

        // These are placeholder methods, to be overriden by extending plugin
        _mouseStart: function(event) {},
        _mouseDrag: function(event) {},
        _mouseStop: function(event) {},
        _mouseCapture: function(event) { return true; }
    });

})(jQuery);
(function($, undefined) {

    $.ui = $.ui || {};

    var cachedScrollbarWidth,
        max = Math.max,
        abs = Math.abs,
        round = Math.round,
        rhorizontal = /left|center|right/,
        rvertical = /top|center|bottom/,
        roffset = /[\+\-]\d+%?/,
        rposition = /^\w+/,
        rpercent = /%$/,
        _position = $.fn.position;

    function getOffsets(offsets, width, height) {
        return [
            parseInt(offsets[0], 10) * (rpercent.test(offsets[0]) ? width / 100 : 1),
            parseInt(offsets[1], 10) * (rpercent.test(offsets[1]) ? height / 100 : 1)
        ];
    }

    function parseCss(element, property) {
        return parseInt($.css(element, property), 10) || 0;
    }

    $.position = {
        scrollbarWidth: function() {
            if (cachedScrollbarWidth !== undefined) {
                return cachedScrollbarWidth;
            }
            var w1,
                w2,
                div =
                    $("<div style='display:block;width:50px;height:50px;overflow:hidden;'><div style='height:100px;width:auto;'></div></div>"),
                innerDiv = div.children()[0];

            $("body").append(div);
            w1 = innerDiv.offsetWidth;
            div.css("overflow", "scroll");

            w2 = innerDiv.offsetWidth;

            if (w1 === w2) {
                w2 = div[0].clientWidth;
            }

            div.remove();

            return (cachedScrollbarWidth = w1 - w2);
        },
        getScrollInfo: function(within) {
            var overflowX = within.isWindow ? "" : within.element.css("overflow-x"),
                overflowY = within.isWindow ? "" : within.element.css("overflow-y"),
                hasOverflowX = overflowX === "scroll" ||
                (overflowX === "auto" && within.width < within.element[0].scrollWidth),
                hasOverflowY = overflowY === "scroll" ||
                (overflowY === "auto" && within.height < within.element[0].scrollHeight);
            return {
                width: hasOverflowX ? $.position.scrollbarWidth() : 0,
                height: hasOverflowY ? $.position.scrollbarWidth() : 0
            };
        },
        getWithinInfo: function(element) {
            var withinElement = $(element || window),
                isWindow = $.isWindow(withinElement[0]);
            return {
                element: withinElement,
                isWindow: isWindow,
                offset: withinElement.offset() || { left: 0, top: 0 },
                scrollLeft: withinElement.scrollLeft(),
                scrollTop: withinElement.scrollTop(),
                width: isWindow ? withinElement.width() : withinElement.outerWidth(),
                height: isWindow ? withinElement.height() : withinElement.outerHeight()
            };
        }
    };

    $.fn.position = function(options) {
        if (!options || !options.of) {
            return _position.apply(this, arguments);
        }

        // make a copy, we don't want to modify arguments
        options = $.extend({}, options);

        var atOffset,
            targetWidth,
            targetHeight,
            targetOffset,
            basePosition,
            target = $(options.of),
            within = $.position.getWithinInfo(options.within),
            scrollInfo = $.position.getScrollInfo(within),
            targetElem = target[0],
            collision = (options.collision || "flip").split(" "),
            offsets = {};

        if (targetElem.nodeType === 9) {
            targetWidth = target.width();
            targetHeight = target.height();
            targetOffset = { top: 0, left: 0 };
        } else if ($.isWindow(targetElem)) {
            targetWidth = target.width();
            targetHeight = target.height();
            targetOffset = { top: target.scrollTop(), left: target.scrollLeft() };
        } else if (targetElem.preventDefault) {
            // force left top to allow flipping
            options.at = "left top";
            targetWidth = targetHeight = 0;
            targetOffset = { top: targetElem.pageY, left: targetElem.pageX };
        } else {
            targetWidth = target.outerWidth();
            targetHeight = target.outerHeight();
            targetOffset = target.offset();
        }
        // clone to reuse original targetOffset later
        basePosition = $.extend({}, targetOffset);

        // force my and at to have valid horizontal and vertical positions
        // if a value is missing or invalid, it will be converted to center
        $.each(["my", "at"],
            function() {
                var pos = (options[this] || "").split(" "),
                    horizontalOffset,
                    verticalOffset;

                if (pos.length === 1) {
                    pos = rhorizontal.test(pos[0])
                        ? pos.concat(["center"])
                        : rvertical.test(pos[0]) ? ["center"].concat(pos) : ["center", "center"];
                }
                pos[0] = rhorizontal.test(pos[0]) ? pos[0] : "center";
                pos[1] = rvertical.test(pos[1]) ? pos[1] : "center";

                // calculate offsets
                horizontalOffset = roffset.exec(pos[0]);
                verticalOffset = roffset.exec(pos[1]);
                offsets[this] = [
                    horizontalOffset ? horizontalOffset[0] : 0,
                    verticalOffset ? verticalOffset[0] : 0
                ];

                // reduce to just the positions without the offsets
                options[this] = [
                    rposition.exec(pos[0])[0],
                    rposition.exec(pos[1])[0]
                ];
            });

        // normalize collision option
        if (collision.length === 1) {
            collision[1] = collision[0];
        }

        if (options.at[0] === "right") {
            basePosition.left += targetWidth;
        } else if (options.at[0] === "center") {
            basePosition.left += targetWidth / 2;
        }

        if (options.at[1] === "bottom") {
            basePosition.top += targetHeight;
        } else if (options.at[1] === "center") {
            basePosition.top += targetHeight / 2;
        }

        atOffset = getOffsets(offsets.at, targetWidth, targetHeight);
        basePosition.left += atOffset[0];
        basePosition.top += atOffset[1];

        return this.each(function() {
            var collisionPosition,
                using,
                elem = $(this),
                elemWidth = elem.outerWidth(),
                elemHeight = elem.outerHeight(),
                marginLeft = parseCss(this, "marginLeft"),
                marginTop = parseCss(this, "marginTop"),
                collisionWidth = elemWidth + marginLeft + parseCss(this, "marginRight") + scrollInfo.width,
                collisionHeight = elemHeight + marginTop + parseCss(this, "marginBottom") + scrollInfo.height,
                position = $.extend({}, basePosition),
                myOffset = getOffsets(offsets.my, elem.outerWidth(), elem.outerHeight());

            if (options.my[0] === "right") {
                position.left -= elemWidth;
            } else if (options.my[0] === "center") {
                position.left -= elemWidth / 2;
            }

            if (options.my[1] === "bottom") {
                position.top -= elemHeight;
            } else if (options.my[1] === "center") {
                position.top -= elemHeight / 2;
            }

            position.left += myOffset[0];
            position.top += myOffset[1];

            // if the browser doesn't support fractions, then round for consistent results
            if (!$.support.offsetFractions) {
                position.left = round(position.left);
                position.top = round(position.top);
            }

            collisionPosition = {
                marginLeft: marginLeft,
                marginTop: marginTop
            };

            $.each(["left", "top"],
                function(i, dir) {
                    if ($.ui.position[collision[i]]) {
                        $.ui.position[collision[i]][dir](position,
                        {
                            targetWidth: targetWidth,
                            targetHeight: targetHeight,
                            elemWidth: elemWidth,
                            elemHeight: elemHeight,
                            collisionPosition: collisionPosition,
                            collisionWidth: collisionWidth,
                            collisionHeight: collisionHeight,
                            offset: [atOffset[0] + myOffset[0], atOffset[1] + myOffset[1]],
                            my: options.my,
                            at: options.at,
                            within: within,
                            elem: elem
                        });
                    }
                });

            if ($.fn.bgiframe) {
                elem.bgiframe();
            }

            if (options.using) {
                // adds feedback as second argument to using callback, if present
                using = function(props) {
                    var left = targetOffset.left - position.left,
                        right = left + targetWidth - elemWidth,
                        top = targetOffset.top - position.top,
                        bottom = top + targetHeight - elemHeight,
                        feedback = {
                            target: {
                                element: target,
                                left: targetOffset.left,
                                top: targetOffset.top,
                                width: targetWidth,
                                height: targetHeight
                            },
                            element: {
                                element: elem,
                                left: position.left,
                                top: position.top,
                                width: elemWidth,
                                height: elemHeight
                            },
                            horizontal: right < 0 ? "left" : left > 0 ? "right" : "center",
                            vertical: bottom < 0 ? "top" : top > 0 ? "bottom" : "middle"
                        };
                    if (targetWidth < elemWidth && abs(left + right) < targetWidth) {
                        feedback.horizontal = "center";
                    }
                    if (targetHeight < elemHeight && abs(top + bottom) < targetHeight) {
                        feedback.vertical = "middle";
                    }
                    if (max(abs(left), abs(right)) > max(abs(top), abs(bottom))) {
                        feedback.important = "horizontal";
                    } else {
                        feedback.important = "vertical";
                    }
                    options.using.call(this, props, feedback);
                };
            }

            elem.offset($.extend(position, { using: using }));
        });
    };

    $.ui.position = {
        fit: {
            left: function(position, data) {
                var within = data.within,
                    withinOffset = within.isWindow ? within.scrollLeft : within.offset.left,
                    outerWidth = within.width,
                    collisionPosLeft = position.left - data.collisionPosition.marginLeft,
                    overLeft = withinOffset - collisionPosLeft,
                    overRight = collisionPosLeft + data.collisionWidth - outerWidth - withinOffset,
                    newOverRight;

                // element is wider than within
                if (data.collisionWidth > outerWidth) {
                    // element is initially over the left side of within
                    if (overLeft > 0 && overRight <= 0) {
                        newOverRight = position.left + overLeft + data.collisionWidth - outerWidth - withinOffset;
                        position.left += overLeft - newOverRight;
                        // element is initially over right side of within
                    } else if (overRight > 0 && overLeft <= 0) {
                        position.left = withinOffset;
                        // element is initially over both left and right sides of within
                    } else {
                        if (overLeft > overRight) {
                            position.left = withinOffset + outerWidth - data.collisionWidth;
                        } else {
                            position.left = withinOffset;
                        }
                    }
                    // too far left -> align with left edge
                } else if (overLeft > 0) {
                    position.left += overLeft;
                    // too far right -> align with right edge
                } else if (overRight > 0) {
                    position.left -= overRight;
                    // adjust based on position and margin
                } else {
                    position.left = max(position.left - collisionPosLeft, position.left);
                }
            },
            top: function(position, data) {
                var within = data.within,
                    withinOffset = within.isWindow ? within.scrollTop : within.offset.top,
                    outerHeight = data.within.height,
                    collisionPosTop = position.top - data.collisionPosition.marginTop,
                    overTop = withinOffset - collisionPosTop,
                    overBottom = collisionPosTop + data.collisionHeight - outerHeight - withinOffset,
                    newOverBottom;

                // element is taller than within
                if (data.collisionHeight > outerHeight) {
                    // element is initially over the top of within
                    if (overTop > 0 && overBottom <= 0) {
                        newOverBottom = position.top + overTop + data.collisionHeight - outerHeight - withinOffset;
                        position.top += overTop - newOverBottom;
                        // element is initially over bottom of within
                    } else if (overBottom > 0 && overTop <= 0) {
                        position.top = withinOffset;
                        // element is initially over both top and bottom of within
                    } else {
                        if (overTop > overBottom) {
                            position.top = withinOffset + outerHeight - data.collisionHeight;
                        } else {
                            position.top = withinOffset;
                        }
                    }
                    // too far up -> align with top
                } else if (overTop > 0) {
                    position.top += overTop;
                    // too far down -> align with bottom edge
                } else if (overBottom > 0) {
                    position.top -= overBottom;
                    // adjust based on position and margin
                } else {
                    position.top = max(position.top - collisionPosTop, position.top);
                }
            }
        },
        flip: {
            left: function(position, data) {
                var within = data.within,
                    withinOffset = within.offset.left + within.scrollLeft,
                    outerWidth = within.width,
                    offsetLeft = within.isWindow ? within.scrollLeft : within.offset.left,
                    collisionPosLeft = position.left - data.collisionPosition.marginLeft,
                    overLeft = collisionPosLeft - offsetLeft,
                    overRight = collisionPosLeft + data.collisionWidth - outerWidth - offsetLeft,
                    myOffset = data.my[0] === "left" ? -data.elemWidth : data.my[0] === "right" ? data.elemWidth : 0,
                    atOffset = data
                        .at[0] ===
                        "left"
                        ? data.targetWidth
                        : data.at[0] === "right" ? -data.targetWidth : 0,
                    offset = -2 * data.offset[0],
                    newOverRight,
                    newOverLeft;

                if (overLeft < 0) {
                    newOverRight = position.left +
                        myOffset +
                        atOffset +
                        offset +
                        data.collisionWidth -
                        outerWidth -
                        withinOffset;
                    if (newOverRight < 0 || newOverRight < abs(overLeft)) {
                        position.left += myOffset + atOffset + offset;
                    }
                } else if (overRight > 0) {
                    newOverLeft = position.left -
                        data.collisionPosition.marginLeft +
                        myOffset +
                        atOffset +
                        offset -
                        offsetLeft;
                    if (newOverLeft > 0 || abs(newOverLeft) < overRight) {
                        position.left += myOffset + atOffset + offset;
                    }
                }
            },
            top: function(position, data) {
                var within = data.within,
                    withinOffset = within.offset.top + within.scrollTop,
                    outerHeight = within.height,
                    offsetTop = within.isWindow ? within.scrollTop : within.offset.top,
                    collisionPosTop = position.top - data.collisionPosition.marginTop,
                    overTop = collisionPosTop - offsetTop,
                    overBottom = collisionPosTop + data.collisionHeight - outerHeight - offsetTop,
                    top = data.my[1] === "top",
                    myOffset = top ? -data.elemHeight : data.my[1] === "bottom" ? data.elemHeight : 0,
                    atOffset = data.at[1] === "top"
                        ? data.targetHeight
                        : data.at[1] === "bottom" ? -data.targetHeight : 0,
                    offset = -2 * data.offset[1],
                    newOverTop,
                    newOverBottom;
                if (overTop < 0) {
                    newOverBottom = position.top +
                        myOffset +
                        atOffset +
                        offset +
                        data.collisionHeight -
                        outerHeight -
                        withinOffset;
                    if ((position.top + myOffset + atOffset + offset) > overTop &&
                    (newOverBottom < 0 || newOverBottom < abs(overTop))) {
                        position.top += myOffset + atOffset + offset;
                    }
                } else if (overBottom > 0) {
                    newOverTop = position.top -
                        data.collisionPosition.marginTop +
                        myOffset +
                        atOffset +
                        offset -
                        offsetTop;
                    if ((position.top + myOffset + atOffset + offset) > overBottom &&
                    (newOverTop > 0 || abs(newOverTop) < overBottom)) {
                        position.top += myOffset + atOffset + offset;
                    }
                }
            }
        },
        flipfit: {
            left: function() {
                $.ui.position.flip.left.apply(this, arguments);
                $.ui.position.fit.left.apply(this, arguments);
            },
            top: function() {
                $.ui.position.flip.top.apply(this, arguments);
                $.ui.position.fit.top.apply(this, arguments);
            }
        }
    };

// fraction support test
    (function() {
        var testElement,
            testElementParent,
            testElementStyle,
            offsetLeft,
            i,
            body = document.getElementsByTagName("body")[0],
            div = document.createElement("div");

        //Create a "fake body" for testing based on method used in jQuery.support
        testElement = document.createElement(body ? "div" : "body");
        testElementStyle = {
            visibility: "hidden",
            width: 0,
            height: 0,
            border: 0,
            margin: 0,
            background: "none"
        };
        if (body) {
            $.extend(testElementStyle,
            {
                position: "absolute",
                left: "-1000px",
                top: "-1000px"
            });
        }
        for (i in testElementStyle) {
            testElement.style[i] = testElementStyle[i];
        }
        testElement.appendChild(div);
        testElementParent = body || document.documentElement;
        testElementParent.insertBefore(testElement, testElementParent.firstChild);

        div.style.cssText = "position: absolute; left: 10.7432222px;";

        offsetLeft = $(div).offset().left;
        $.support.offsetFractions = offsetLeft > 10 && offsetLeft < 11;

        testElement.innerHTML = "";
        testElementParent.removeChild(testElement);
    })();

// DEPRECATED
    if ($.uiBackCompat !== false) {
        // offset option
        (function($) {
            var _position = $.fn.position;
            $.fn.position = function(options) {
                if (!options || !options.offset) {
                    return _position.call(this, options);
                }
                var offset = options.offset.split(" "),
                    at = options.at.split(" ");
                if (offset.length === 1) {
                    offset[1] = offset[0];
                }
                if (/^\d/.test(offset[0])) {
                    offset[0] = "+" + offset[0];
                }
                if (/^\d/.test(offset[1])) {
                    offset[1] = "+" + offset[1];
                }
                if (at.length === 1) {
                    if (/left|center|right/.test(at[0])) {
                        at[1] = "center";
                    } else {
                        at[1] = at[0];
                        at[0] = "center";
                    }
                }
                return _position.call(this,
                    $.extend(options,
                    {
                        at: at[0] + offset[0] + " " + at[1] + offset[1],
                        offset: undefined
                    }));
            };
        }(jQuery));
    }

}(jQuery));
(function($, undefined) {

    var tabId = 0,
        rhash = /#.*$/;

    function getNextTabId() {
        return ++tabId;
    }

    function isLocal(anchor) {
        return anchor.hash.length > 1 &&
            anchor.href.replace(rhash, "") === location.href.replace(rhash, "");
    }

    $.widget("ui.tabs",
    {
        version: "1.9.1",
        delay: 300,
        options: {
            active: null,
            collapsible: false,
            event: "click",
            heightStyle: "content",
            hide: null,
            show: null,

            // callbacks
            activate: null,
            beforeActivate: null,
            beforeLoad: null,
            load: null
        },

        _create: function() {
            var that = this,
                options = this.options,
                active = options.active,
                locationHash = location.hash.substring(1);

            this.running = false;

            this.element
                .addClass("ui-tabs ui-widget ui-widget-content ui-corner-all")
                .toggleClass("ui-tabs-collapsible", options.collapsible)
                // Prevent users from focusing disabled tabs via click
                .delegate(".ui-tabs-nav > li",
                    "mousedown" + this.eventNamespace,
                    function(event) {
                        if ($(this).is(".ui-state-disabled")) {
                            event.preventDefault();
                        }
                    })
                // support: IE <9
                // Preventing the default action in mousedown doesn't prevent IE
                // from focusing the element, so if the anchor gets focused, blur.
                // We don't have to worry about focusing the previously focused
                // element since clicking on a non-focusable element should focus
                // the body anyway.
                .delegate(".ui-tabs-anchor",
                    "focus" + this.eventNamespace,
                    function() {
                        if ($(this).closest("li").is(".ui-state-disabled")) {
                            this.blur();
                        }
                    });

            this._processTabs();

            if (active === null) {
                // check the fragment identifier in the URL
                if (locationHash) {
                    this.tabs.each(function(i, tab) {
                        if ($(tab).attr("aria-controls") === locationHash) {
                            active = i;
                            return false;
                        }
                    });
                }

                // check for a tab marked active via a class
                if (active === null) {
                    active = this.tabs.index(this.tabs.filter(".ui-tabs-active"));
                }

                // no active tab, set to false
                if (active === null || active === -1) {
                    active = this.tabs.length ? 0 : false;
                }
            }

            // handle numbers: negative, out of range
            if (active !== false) {
                active = this.tabs.index(this.tabs.eq(active));
                if (active === -1) {
                    active = options.collapsible ? false : 0;
                }
            }
            options.active = active;

            // don't allow collapsible: false and active: false
            if (!options.collapsible && options.active === false && this.anchors.length) {
                options.active = 0;
            }

            // Take disabling tabs via class attribute from HTML
            // into account and update option properly.
            if ($.isArray(options.disabled)) {
                options.disabled = $.unique(options.disabled.concat(
                        $.map(this.tabs.filter(".ui-state-disabled"),
                            function(li) {
                                return that.tabs.index(li);
                            })
                    ))
                    .sort();
            }

            // check for length avoids error when initializing empty list
            if (this.options.active !== false && this.anchors.length) {
                this.active = this._findActive(this.options.active);
            } else {
                this.active = $();
            }

            this._refresh();

            if (this.active.length) {
                this.load(options.active);
            }
        },

        _getCreateEventData: function() {
            return {
                tab: this.active,
                panel: !this.active.length ? $() : this._getPanelForTab(this.active)
            };
        },

        _tabKeydown: function(event) {
            var focusedTab = $(this.document[0].activeElement).closest("li"),
                selectedIndex = this.tabs.index(focusedTab),
                goingForward = true;

            if (this._handlePageNav(event)) {
                return;
            }

            switch (event.keyCode) {
            case $.ui.keyCode.RIGHT:
            case $.ui.keyCode.DOWN:
                selectedIndex++;
                break;
            case $.ui.keyCode.UP:
            case $.ui.keyCode.LEFT:
                goingForward = false;
                selectedIndex--;
                break;
            case $.ui.keyCode.END:
                selectedIndex = this.anchors.length - 1;
                break;
            case $.ui.keyCode.HOME:
                selectedIndex = 0;
                break;
            case $.ui.keyCode.SPACE:
                // Activate only, no collapsing
                event.preventDefault();
                clearTimeout(this.activating);
                this._activate(selectedIndex);
                return;
            case $.ui.keyCode.ENTER:
                // Toggle (cancel delayed activation, allow collapsing)
                event.preventDefault();
                clearTimeout(this.activating);
                // Determine if we should collapse or activate
                this._activate(selectedIndex === this.options.active ? false : selectedIndex);
                return;
            default:
                return;
            }

            // Focus the appropriate tab, based on which key was pressed
            event.preventDefault();
            clearTimeout(this.activating);
            selectedIndex = this._focusNextTab(selectedIndex, goingForward);

            // Navigating with control key will prevent automatic activation
            if (!event.ctrlKey) {
                // Update aria-selected immediately so that AT think the tab is already selected.
                // Otherwise AT may confuse the user by stating that they need to activate the tab,
                // but the tab will already be activated by the time the announcement finishes.
                focusedTab.attr("aria-selected", "false");
                this.tabs.eq(selectedIndex).attr("aria-selected", "true");

                this.activating = this._delay(function() {
                        this.option("active", selectedIndex);
                    },
                    this.delay);
            }
        },

        _panelKeydown: function(event) {
            if (this._handlePageNav(event)) {
                return;
            }

            // Ctrl+up moves focus to the current tab
            if (event.ctrlKey && event.keyCode === $.ui.keyCode.UP) {
                event.preventDefault();
                this.active.focus();
            }
        },

        // Alt+page up/down moves focus to the previous/next tab (and activates)
        _handlePageNav: function(event) {
            if (event.altKey && event.keyCode === $.ui.keyCode.PAGE_UP) {
                this._activate(this._focusNextTab(this.options.active - 1, false));
                return true;
            }
            if (event.altKey && event.keyCode === $.ui.keyCode.PAGE_DOWN) {
                this._activate(this._focusNextTab(this.options.active + 1, true));
                return true;
            }
        },

        _findNextTab: function(index, goingForward) {
            var lastTabIndex = this.tabs.length - 1;

            function constrain() {
                if (index > lastTabIndex) {
                    index = 0;
                }
                if (index < 0) {
                    index = lastTabIndex;
                }
                return index;
            }

            while ($.inArray(constrain(), this.options.disabled) !== -1) {
                index = goingForward ? index + 1 : index - 1;
            }

            return index;
        },

        _focusNextTab: function(index, goingForward) {
            index = this._findNextTab(index, goingForward);
            this.tabs.eq(index).focus();
            return index;
        },

        _setOption: function(key, value) {
            if (key === "active") {
                // _activate() will handle invalid values and update this.options
                this._activate(value);
                return;
            }

            if (key === "disabled") {
                // don't use the widget factory's disabled handling
                this._setupDisabled(value);
                return;
            }

            this._super(key, value);

            if (key === "collapsible") {
                this.element.toggleClass("ui-tabs-collapsible", value);
                // Setting collapsible: false while collapsed; open first panel
                if (!value && this.options.active === false) {
                    this._activate(0);
                }
            }

            if (key === "event") {
                this._setupEvents(value);
            }

            if (key === "heightStyle") {
                this._setupHeightStyle(value);
            }
        },

        _tabId: function(tab) {
            return tab.attr("aria-controls") || "ui-tabs-" + getNextTabId();
        },

        _sanitizeSelector: function(hash) {
            return hash ? hash.replace(/[!"$%&'()*+,.\/:;<=>?@\[\]\^`{|}~]/g, "\\$&") : "";
        },

        refresh: function() {
            var options = this.options,
                lis = this.tablist.children(":has(a[href])");

            // get disabled tabs from class attribute from HTML
            // this will get converted to a boolean if needed in _refresh()
            options.disabled = $.map(lis.filter(".ui-state-disabled"),
                function(tab) {
                    return lis.index(tab);
                });

            this._processTabs();

            // was collapsed or no tabs
            if (options.active === false || !this.anchors.length) {
                options.active = false;
                this.active = $();
                // was active, but active tab is gone
            } else if (this.active.length && !$.contains(this.tablist[0], this.active[0])) {
                // all remaining tabs are disabled
                if (this.tabs.length === options.disabled.length) {
                    options.active = false;
                    this.active = $();
                    // activate previous tab
                } else {
                    this._activate(this._findNextTab(Math.max(0, options.active - 1), false));
                }
                // was active, active tab still exists
            } else {
                // make sure active index is correct
                options.active = this.tabs.index(this.active);
            }

            this._refresh();
        },

        _refresh: function() {
            this._setupDisabled(this.options.disabled);
            this._setupEvents(this.options.event);
            this._setupHeightStyle(this.options.heightStyle);

            this.tabs.not(this.active)
                .attr({
                    "aria-selected": "false",
                    tabIndex: -1
                });
            this.panels.not(this._getPanelForTab(this.active))
                .hide()
                .attr({
                    "aria-expanded": "false",
                    "aria-hidden": "true"
                });

            // Make sure one tab is in the tab order
            if (!this.active.length) {
                this.tabs.eq(0).attr("tabIndex", 0);
            } else {
                this.active
                    .addClass("ui-tabs-active ui-state-active")
                    .attr({
                        "aria-selected": "true",
                        tabIndex: 0
                    });
                this._getPanelForTab(this.active)
                    .show()
                    .attr({
                        "aria-expanded": "true",
                        "aria-hidden": "false"
                    });
            }
        },

        _processTabs: function() {
            var that = this;

            this.tablist = this._getList()
                .addClass("ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all")
                .attr("role", "tablist");

            this.tabs = this.tablist.find(" > li:has(a[href])")
                .addClass("ui-state-default ui-corner-top")
                .attr({
                    role: "tab",
                    tabIndex: -1
                });

            this.anchors = this.tabs.map(function() {
                    return $("a", this)[0];
                })
                .addClass("ui-tabs-anchor")
                .attr({
                    role: "presentation",
                    tabIndex: -1
                });

            this.panels = $();

            this.anchors.each(function(i, anchor) {
                var selector,
                    panel,
                    panelId,
                    anchorId = $(anchor).uniqueId().attr("id"),
                    tab = $(anchor).closest("li"),
                    originalAriaControls = tab.attr("aria-controls");

                // inline tab
                if (isLocal(anchor)) {
                    selector = anchor.hash;
                    panel = that.element.find(that._sanitizeSelector(selector));
                    // remote tab
                } else {
                    panelId = that._tabId(tab);
                    selector = "#" + panelId;
                    panel = that.element.find(selector);
                    if (!panel.length) {
                        panel = that._createPanel(panelId);
                        panel.insertAfter(that.panels[i - 1] || that.tablist);
                    }
                    panel.attr("aria-live", "polite");
                }

                if (panel.length) {
                    that.panels = that.panels.add(panel);
                }
                if (originalAriaControls) {
                    tab.data("ui-tabs-aria-controls", originalAriaControls);
                }
                tab.attr({
                    "aria-controls": selector.substring(1),
                    "aria-labelledby": anchorId
                });
                panel.attr("aria-labelledby", anchorId);
            });

            this.panels
                .addClass("ui-tabs-panel ui-widget-content ui-corner-bottom")
                .attr("role", "tabpanel");
        },

        // allow overriding how to find the list for rare usage scenarios (#7715)
        _getList: function() {
            return this.element.find("ol, ul").eq(0);
        },

        _createPanel: function(id) {
            return $("<div>")
                .attr("id", id)
                .addClass("ui-tabs-panel ui-widget-content ui-corner-bottom")
                .data("ui-tabs-destroy", true);
        },

        _setupDisabled: function(disabled) {
            if ($.isArray(disabled)) {
                if (!disabled.length) {
                    disabled = false;
                } else if (disabled.length === this.anchors.length) {
                    disabled = true;
                }
            }

            // disable tabs
            for (var i = 0, li; (li = this.tabs[i]); i++) {
                if (disabled === true || $.inArray(i, disabled) !== -1) {
                    $(li)
                        .addClass("ui-state-disabled")
                        .attr("aria-disabled", "true");
                } else {
                    $(li)
                        .removeClass("ui-state-disabled")
                        .removeAttr("aria-disabled");
                }
            }

            this.options.disabled = disabled;
        },

        _setupEvents: function(event) {
            var events = {
                click: function(event) {
                    event.preventDefault();
                }
            };
            if (event) {
                $.each(event.split(" "),
                    function(index, eventName) {
                        events[eventName] = "_eventHandler";
                    });
            }

            this._off(this.anchors.add(this.tabs).add(this.panels));
            this._on(this.anchors, events);
            this._on(this.tabs, { keydown: "_tabKeydown" });
            this._on(this.panels, { keydown: "_panelKeydown" });

            this._focusable(this.tabs);
            this._hoverable(this.tabs);
        },

        _setupHeightStyle: function(heightStyle) {
            var maxHeight,
                overflow,
                parent = this.element.parent();

            if (heightStyle === "fill") {
                // IE 6 treats height like minHeight, so we need to turn off overflow
                // in order to get a reliable height
                // we use the minHeight support test because we assume that only
                // browsers that don't support minHeight will treat height as minHeight
                if (!$.support.minHeight) {
                    overflow = parent.css("overflow");
                    parent.css("overflow", "hidden");
                }
                maxHeight = parent.height();
                this.element.siblings(":visible")
                    .each(function() {
                        var elem = $(this),
                            position = elem.css("position");

                        if (position === "absolute" || position === "fixed") {
                            return;
                        }
                        maxHeight -= elem.outerHeight(true);
                    });
                if (overflow) {
                    parent.css("overflow", overflow);
                }

                this.element.children()
                    .not(this.panels)
                    .each(function() {
                        maxHeight -= $(this).outerHeight(true);
                    });

                this.panels.each(function() {
                        $(this)
                            .height(Math.max(0,
                                maxHeight -
                                $(this).innerHeight() +
                                $(this).height()));
                    })
                    .css("overflow", "auto");
            } else if (heightStyle === "auto") {
                maxHeight = 0;
                this.panels.each(function() {
                        maxHeight = Math.max(maxHeight, $(this).height("").height());
                    })
                    .height(maxHeight);
            }
        },

        _eventHandler: function(event) {
            var options = this.options,
                active = this.active,
                anchor = $(event.currentTarget),
                tab = anchor.closest("li"),
                clickedIsActive = tab[0] === active[0],
                collapsing = clickedIsActive && options.collapsible,
                toShow = collapsing ? $() : this._getPanelForTab(tab),
                toHide = !active.length ? $() : this._getPanelForTab(active),
                eventData = {
                    oldTab: active,
                    oldPanel: toHide,
                    newTab: collapsing ? $() : tab,
                    newPanel: toShow
                };

            event.preventDefault();

            if (tab.hasClass("ui-state-disabled") ||
                // tab is already loading
                tab.hasClass("ui-tabs-loading") ||
                // can't switch durning an animation
                this.running ||
                // click on active header, but not collapsible
                (clickedIsActive && !options.collapsible) ||
                // allow canceling activation
                (this._trigger("beforeActivate", event, eventData) === false)) {
                return;
            }

            options.active = collapsing ? false : this.tabs.index(tab);

            this.active = clickedIsActive ? $() : tab;
            if (this.xhr) {
                this.xhr.abort();
            }

            if (!toHide.length && !toShow.length) {
                $.error("jQuery UI Tabs: Mismatching fragment identifier.");
            }

            if (toShow.length) {
                this.load(this.tabs.index(tab), event);
            }
            this._toggle(event, eventData);
        },

        // handles show/hide for selecting tabs
        _toggle: function(event, eventData) {
            var that = this,
                toShow = eventData.newPanel,
                toHide = eventData.oldPanel;

            this.running = true;

            function complete() {
                that.running = false;
                that._trigger("activate", event, eventData);
            }

            function show() {
                eventData.newTab.closest("li").addClass("ui-tabs-active ui-state-active");

                if (toShow.length && that.options.show) {
                    that._show(toShow, that.options.show, complete);
                } else {
                    toShow.show();
                    complete();
                }
            }

            // start out by hiding, then showing, then completing
            if (toHide.length && this.options.hide) {
                this._hide(toHide,
                    this.options.hide,
                    function() {
                        eventData.oldTab.closest("li").removeClass("ui-tabs-active ui-state-active");
                        show();
                    });
            } else {
                eventData.oldTab.closest("li").removeClass("ui-tabs-active ui-state-active");
                toHide.hide();
                show();
            }

            toHide.attr({
                "aria-expanded": "false",
                "aria-hidden": "true"
            });
            eventData.oldTab.attr("aria-selected", "false");
            // If we're switching tabs, remove the old tab from the tab order.
            // If we're opening from collapsed state, remove the previous tab from the tab order.
            // If we're collapsing, then keep the collapsing tab in the tab order.
            if (toShow.length && toHide.length) {
                eventData.oldTab.attr("tabIndex", -1);
            } else if (toShow.length) {
                this.tabs.filter(function() {
                        return $(this).attr("tabIndex") === 0;
                    })
                    .attr("tabIndex", -1);
            }

            toShow.attr({
                "aria-expanded": "true",
                "aria-hidden": "false"
            });
            eventData.newTab.attr({
                "aria-selected": "true",
                tabIndex: 0
            });
        },

        _activate: function(index) {
            var anchor,
                active = this._findActive(index);

            // trying to activate the already active panel
            if (active[0] === this.active[0]) {
                return;
            }

            // trying to collapse, simulate a click on the current active header
            if (!active.length) {
                active = this.active;
            }

            anchor = active.find(".ui-tabs-anchor")[0];
            this._eventHandler({
                target: anchor,
                currentTarget: anchor,
                preventDefault: $.noop
            });
        },

        _findActive: function(index) {
            return index === false ? $() : this.tabs.eq(index);
        },

        _getIndex: function(index) {
            // meta-function to give users option to provide a href string instead of a numerical index.
            if (typeof index === "string") {
                index = this.anchors.index(this.anchors.filter("[href$='" + index + "']"));
            }

            return index;
        },

        _destroy: function() {
            if (this.xhr) {
                this.xhr.abort();
            }

            this.element.removeClass("ui-tabs ui-widget ui-widget-content ui-corner-all ui-tabs-collapsible");

            this.tablist
                .removeClass("ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all")
                .removeAttr("role");

            this.anchors
                .removeClass("ui-tabs-anchor")
                .removeAttr("role")
                .removeAttr("tabIndex")
                .removeData("href.tabs")
                .removeData("load.tabs")
                .removeUniqueId();

            this.tabs.add(this.panels)
                .each(function() {
                    if ($.data(this, "ui-tabs-destroy")) {
                        $(this).remove();
                    } else {
                        $(this)
                            .removeClass("ui-state-default ui-state-active ui-state-disabled " +
                                "ui-corner-top ui-corner-bottom ui-widget-content ui-tabs-active ui-tabs-panel")
                            .removeAttr("tabIndex")
                            .removeAttr("aria-live")
                            .removeAttr("aria-busy")
                            .removeAttr("aria-selected")
                            .removeAttr("aria-labelledby")
                            .removeAttr("aria-hidden")
                            .removeAttr("aria-expanded")
                            .removeAttr("role");
                    }
                });

            this.tabs.each(function() {
                var li = $(this),
                    prev = li.data("ui-tabs-aria-controls");
                if (prev) {
                    li.attr("aria-controls", prev);
                } else {
                    li.removeAttr("aria-controls");
                }
            });

            if (this.options.heightStyle !== "content") {
                this.panels.css("height", "");
            }
        },

        enable: function(index) {
            var disabled = this.options.disabled;
            if (disabled === false) {
                return;
            }

            if (index === undefined) {
                disabled = false;
            } else {
                index = this._getIndex(index);
                if ($.isArray(disabled)) {
                    disabled = $.map(disabled,
                        function(num) {
                            return num !== index ? num : null;
                        });
                } else {
                    disabled = $.map(this.tabs,
                        function(li, num) {
                            return num !== index ? num : null;
                        });
                }
            }
            this._setupDisabled(disabled);
        },

        disable: function(index) {
            var disabled = this.options.disabled;
            if (disabled === true) {
                return;
            }

            if (index === undefined) {
                disabled = true;
            } else {
                index = this._getIndex(index);
                if ($.inArray(index, disabled) !== -1) {
                    return;
                }
                if ($.isArray(disabled)) {
                    disabled = $.merge([index], disabled).sort();
                } else {
                    disabled = [index];
                }
            }
            this._setupDisabled(disabled);
        },

        load: function(index, event) {
            index = this._getIndex(index);
            var that = this,
                tab = this.tabs.eq(index),
                anchor = tab.find(".ui-tabs-anchor"),
                panel = this._getPanelForTab(tab),
                eventData = {
                    tab: tab,
                    panel: panel
                };

            // not remote
            if (isLocal(anchor[0])) {
                return;
            }

            this.xhr = $.ajax(this._ajaxSettings(anchor, event, eventData));

            // support: jQuery <1.8
            // jQuery <1.8 returns false if the request is canceled in beforeSend,
            // but as of 1.8, $.ajax() always returns a jqXHR object.
            if (this.xhr && this.xhr.statusText !== "canceled") {
                tab.addClass("ui-tabs-loading");
                panel.attr("aria-busy", "true");

                this.xhr
                    .success(function(response) {
                        // support: jQuery <1.8
                        // http://bugs.jquery.com/ticket/11778
                        setTimeout(function() {
                                panel.html(response);
                                that._trigger("load", event, eventData);
                            },
                            1);
                    })
                    .complete(function(jqXHR, status) {
                        // support: jQuery <1.8
                        // http://bugs.jquery.com/ticket/11778
                        setTimeout(function() {
                                if (status === "abort") {
                                    that.panels.stop(false, true);
                                }

                                tab.removeClass("ui-tabs-loading");
                                panel.removeAttr("aria-busy");

                                if (jqXHR === that.xhr) {
                                    delete that.xhr;
                                }
                            },
                            1);
                    });
            }
        },

        // TODO: Remove this function in 1.10 when ajaxOptions is removed
        _ajaxSettings: function(anchor, event, eventData) {
            var that = this;
            return {
                url: anchor.attr("href"),
                beforeSend: function(jqXHR, settings) {
                    return that._trigger("beforeLoad",
                        event,
                        $.extend({ jqXHR: jqXHR, ajaxSettings: settings }, eventData));
                }
            };
        },

        _getPanelForTab: function(tab) {
            var id = $(tab).attr("aria-controls");
            return this.element.find(this._sanitizeSelector("#" + id));
        }
    });

// DEPRECATED
    if ($.uiBackCompat !== false) {

        // helper method for a lot of the back compat extensions
        $.ui.tabs.prototype._ui = function(tab, panel) {
            return {
                tab: tab,
                panel: panel,
                index: this.anchors.index(tab)
            };
        };

        // url method
        $.widget("ui.tabs",
            $.ui.tabs,
            {
                url: function(index, url) {
                    this.anchors.eq(index).attr("href", url);
                }
            });

        // TODO: Remove _ajaxSettings() method when removing this extension
        // ajaxOptions and cache options
        $.widget("ui.tabs",
            $.ui.tabs,
            {
                options: {
                    ajaxOptions: null,
                    cache: false
                },

                _create: function() {
                    this._super();

                    var that = this;

                    this._on({
                        tabsbeforeload: function(event, ui) {
                            // tab is already cached
                            if ($.data(ui.tab[0], "cache.tabs")) {
                                event.preventDefault();
                                return;
                            }

                            ui.jqXHR.success(function() {
                                if (that.options.cache) {
                                    $.data(ui.tab[0], "cache.tabs", true);
                                }
                            });
                        }
                    });
                },

                _ajaxSettings: function(anchor, event, ui) {
                    var ajaxOptions = this.options.ajaxOptions;
                    return $.extend({},
                        ajaxOptions,
                        {
                            error: function(xhr, status) {
                                try {
                                    // Passing index avoid a race condition when this method is
                                    // called after the user has selected another tab.
                                    // Pass the anchor that initiated this request allows
                                    // loadError to manipulate the tab content panel via $(a.hash)
                                    ajaxOptions.error(
                                        xhr,
                                        status,
                                        ui.tab.closest("li").index(),
                                        ui.tab[0]);
                                } catch (error) {
                                }
                            }
                        },
                        this._superApply(arguments));
                },

                _setOption: function(key, value) {
                    // reset cache if switching from cached to not cached
                    if (key === "cache" && value === false) {
                        this.anchors.removeData("cache.tabs");
                    }
                    this._super(key, value);
                },

                _destroy: function() {
                    this.anchors.removeData("cache.tabs");
                    this._super();
                },

                url: function(index) {
                    this.anchors.eq(index).removeData("cache.tabs");
                    this._superApply(arguments);
                }
            });

        // abort method
        $.widget("ui.tabs",
            $.ui.tabs,
            {
                abort: function() {
                    if (this.xhr) {
                        this.xhr.abort();
                    }
                }
            });

        // spinner
        $.widget("ui.tabs",
            $.ui.tabs,
            {
                options: {
                    spinner: "<em>Loading&#8230;</em>"
                },
                _create: function() {
                    this._super();
                    this._on({
                        tabsbeforeload: function(event, ui) {
                            // Don't react to nested tabs or tabs that don't use a spinner
                            if (event.target !== this.element[0] ||
                                !this.options.spinner) {
                                return;
                            }

                            var span = ui.tab.find("span"),
                                html = span.html();
                            span.html(this.options.spinner);
                            ui.jqXHR.complete(function() {
                                span.html(html);
                            });
                        }
                    });
                }
            });

        // enable/disable events
        $.widget("ui.tabs",
            $.ui.tabs,
            {
                options: {
                    enable: null,
                    disable: null
                },

                enable: function(index) {
                    var options = this.options,
                        trigger;

                    if (index && options.disabled === true ||
                    ($.isArray(options.disabled) && $.inArray(index, options.disabled) !== -1)) {
                        trigger = true;
                    }

                    this._superApply(arguments);

                    if (trigger) {
                        this._trigger("enable", null, this._ui(this.anchors[index], this.panels[index]));
                    }
                },

                disable: function(index) {
                    var options = this.options,
                        trigger;

                    if (index && options.disabled === false ||
                    ($.isArray(options.disabled) && $.inArray(index, options.disabled) === -1)) {
                        trigger = true;
                    }

                    this._superApply(arguments);

                    if (trigger) {
                        this._trigger("disable", null, this._ui(this.anchors[index], this.panels[index]));
                    }
                }
            });

        // add/remove methods and events
        $.widget("ui.tabs",
            $.ui.tabs,
            {
                options: {
                    add: null,
                    remove: null,
                    tabTemplate: "<li><a href='#{href}'><span>#{label}</span></a></li>"
                },

                add: function(url, label, index) {
                    if (index === undefined) {
                        index = this.anchors.length;
                    }

                    var doInsertAfter,
                        panel,
                        options = this.options,
                        li = $(options.tabTemplate
                            .replace(/#\{href\}/g, url)
                            .replace(/#\{label\}/g, label)),
                        id = !url.indexOf("#") ? url.replace("#", "") : this._tabId(li);

                    li.addClass("ui-state-default ui-corner-top").data("ui-tabs-destroy", true);
                    li.attr("aria-controls", id);

                    doInsertAfter = index >= this.tabs.length;

                    // try to find an existing element before creating a new one
                    panel = this.element.find("#" + id);
                    if (!panel.length) {
                        panel = this._createPanel(id);
                        if (doInsertAfter) {
                            if (index > 0) {
                                panel.insertAfter(this.panels.eq(-1));
                            } else {
                                panel.appendTo(this.element);
                            }
                        } else {
                            panel.insertBefore(this.panels[index]);
                        }
                    }
                    panel.addClass("ui-tabs-panel ui-widget-content ui-corner-bottom").hide();

                    if (doInsertAfter) {
                        li.appendTo(this.tablist);
                    } else {
                        li.insertBefore(this.tabs[index]);
                    }

                    options.disabled = $.map(options.disabled,
                        function(n) {
                            return n >= index ? ++n : n;
                        });

                    this.refresh();
                    if (this.tabs.length === 1 && options.active === false) {
                        this.option("active", 0);
                    }

                    this._trigger("add", null, this._ui(this.anchors[index], this.panels[index]));
                    return this;
                },

                remove: function(index) {
                    index = this._getIndex(index);
                    var options = this.options,
                        tab = this.tabs.eq(index).remove(),
                        panel = this._getPanelForTab(tab).remove();

                    // If selected tab was removed focus tab to the right or
                    // in case the last tab was removed the tab to the left.
                    // We check for more than 2 tabs, because if there are only 2,
                    // then when we remove this tab, there will only be one tab left
                    // so we don't need to detect which tab to activate.
                    if (tab.hasClass("ui-tabs-active") && this.anchors.length > 2) {
                        this._activate(index + (index + 1 < this.anchors.length ? 1 : -1));
                    }

                    options.disabled = $.map(
                        $.grep(options.disabled,
                            function(n) {
                                return n !== index;
                            }),
                        function(n) {
                            return n >= index ? --n : n;
                        });

                    this.refresh();

                    this._trigger("remove", null, this._ui(tab.find("a")[0], panel[0]));
                    return this;
                }
            });

        // length method
        $.widget("ui.tabs",
            $.ui.tabs,
            {
                length: function() {
                    return this.anchors.length;
                }
            });

        // panel ids (idPrefix option + title attribute)
        $.widget("ui.tabs",
            $.ui.tabs,
            {
                options: {
                    idPrefix: "ui-tabs-"
                },

                _tabId: function(tab) {
                    var a = tab.is("li") ? tab.find("a[href]") : tab;
                    a = a[0];
                    return $(a).closest("li").attr("aria-controls") ||
                        a.title && a.title.replace(/\s/g, "_").replace(/[^\w\u00c0-\uFFFF\-]/g, "") ||
                        this.options.idPrefix + getNextTabId();
                }
            });

        // _createPanel method
        $.widget("ui.tabs",
            $.ui.tabs,
            {
                options: {
                    panelTemplate: "<div></div>"
                },

                _createPanel: function(id) {
                    return $(this.options.panelTemplate)
                        .attr("id", id)
                        .addClass("ui-tabs-panel ui-widget-content ui-corner-bottom")
                        .data("ui-tabs-destroy", true);
                }
            });

        // selected option
        $.widget("ui.tabs",
            $.ui.tabs,
            {
                _create: function() {
                    var options = this.options;
                    if (options.active === null && options.selected !== undefined) {
                        options.active = options.selected === -1 ? false : options.selected;
                    }
                    this._super();
                    options.selected = options.active;
                    if (options.selected === false) {
                        options.selected = -1;
                    }
                },

                _setOption: function(key, value) {
                    if (key !== "selected") {
                        return this._super(key, value);
                    }

                    var options = this.options;
                    this._super("active", value === -1 ? false : value);
                    options.selected = options.active;
                    if (options.selected === false) {
                        options.selected = -1;
                    }
                },

                _eventHandler: function() {
                    this._superApply(arguments);
                    this.options.selected = this.options.active;
                    if (this.options.selected === false) {
                        this.options.selected = -1;
                    }
                }
            });

        // show and select event
        $.widget("ui.tabs",
            $.ui.tabs,
            {
                options: {
                    show: null,
                    select: null
                },
                _create: function() {
                    this._super();
                    if (this.options.active !== false) {
                        this._trigger("show",
                            null,
                            this._ui(
                                this.active.find(".ui-tabs-anchor")[0],
                                this._getPanelForTab(this.active)[0]));
                    }
                },
                _trigger: function(type, event, data) {
                    var ret = this._superApply(arguments);
                    if (!ret) {
                        return false;
                    }
                    if (type === "beforeActivate" && data.newTab.length) {
                        ret = this._super("select",
                            event,
                            {
                                tab: data.newTab.find(".ui-tabs-anchor")[0],
                                panel: data.newPanel[0],
                                index: data.newTab.closest("li").index()
                            });
                    } else if (type === "activate" && data.newTab.length) {
                        ret = this._super("show",
                            event,
                            {
                                tab: data.newTab.find(".ui-tabs-anchor")[0],
                                panel: data.newPanel[0],
                                index: data.newTab.closest("li").index()
                            });
                    }
                    return ret;
                }
            });

        // select method
        $.widget("ui.tabs",
            $.ui.tabs,
            {
                select: function(index) {
                    index = this._getIndex(index);
                    if (index === -1) {
                        if (this.options.collapsible && this.options.selected !== -1) {
                            index = this.options.selected;
                        } else {
                            return;
                        }
                    }
                    this.anchors.eq(index).trigger(this.options.event + this.eventNamespace);
                }
            });

        // cookie option
        (function() {

            var listId = 0;

            $.widget("ui.tabs",
                $.ui.tabs,
                {
                    options: {
                        cookie: null // e.g. { expires: 7, path: '/', domain: 'jquery.com', secure: true }
                    },
                    _create: function() {
                        var options = this.options,
                            active;
                        if (options.active == null && options.cookie) {
                            active = parseInt(this._cookie(), 10);
                            if (active === -1) {
                                active = false;
                            }
                            options.active = active;
                        }
                        this._super();
                    },
                    _cookie: function(active) {
                        var cookie = [
                            this.cookie ||
                            (this.cookie = this.options.cookie.name || "ui-tabs-" + (++listId))
                        ];
                        if (arguments.length) {
                            cookie.push(active === false ? -1 : active);
                            cookie.push(this.options.cookie);
                        }
                        return $.cookie.apply(null, cookie);
                    },
                    _refresh: function() {
                        this._super();
                        if (this.options.cookie) {
                            this._cookie(this.options.active, this.options.cookie);
                        }
                    },
                    _eventHandler: function() {
                        this._superApply(arguments);
                        if (this.options.cookie) {
                            this._cookie(this.options.active, this.options.cookie);
                        }
                    },
                    _destroy: function() {
                        this._super();
                        if (this.options.cookie) {
                            this._cookie(null, this.options.cookie);
                        }
                    }
                });

        })();

        // load event
        $.widget("ui.tabs",
            $.ui.tabs,
            {
                _trigger: function(type, event, data) {
                    var _data = $.extend({}, data);
                    if (type === "load") {
                        _data.panel = _data.panel[0];
                        _data.tab = _data.tab.find(".ui-tabs-anchor")[0];
                    }
                    return this._super(type, event, _data);
                }
            });

        // fx option
        // The new animation options (show, hide) conflict with the old show callback.
        // The old fx option wins over show/hide anyway (always favor back-compat).
        // If a user wants to use the new animation API, they must give up the old API.
        $.widget("ui.tabs",
            $.ui.tabs,
            {
                options: {
                    fx: null // e.g. { height: "toggle", opacity: "toggle", duration: 200 }
                },

                _getFx: function() {
                    var hide,
                        show,
                        fx = this.options.fx;

                    if (fx) {
                        if ($.isArray(fx)) {
                            hide = fx[0];
                            show = fx[1];
                        } else {
                            hide = show = fx;
                        }
                    }

                    return fx ? { show: show, hide: hide } : null;
                },

                _toggle: function(event, eventData) {
                    var that = this,
                        toShow = eventData.newPanel,
                        toHide = eventData.oldPanel,
                        fx = this._getFx();

                    if (!fx) {
                        return this._super(event, eventData);
                    }

                    that.running = true;

                    function complete() {
                        that.running = false;
                        that._trigger("activate", event, eventData);
                    }

                    function show() {
                        eventData.newTab.closest("li").addClass("ui-tabs-active ui-state-active");

                        if (toShow.length && fx.show) {
                            toShow
                                .animate(fx.show,
                                    fx.show.duration,
                                    function() {
                                        complete();
                                    });
                        } else {
                            toShow.show();
                            complete();
                        }
                    }

                    // start out by hiding, then showing, then completing
                    if (toHide.length && fx.hide) {
                        toHide.animate(fx.hide,
                            fx.hide.duration,
                            function() {
                                eventData.oldTab.closest("li").removeClass("ui-tabs-active ui-state-active");
                                show();
                            });
                    } else {
                        eventData.oldTab.closest("li").removeClass("ui-tabs-active ui-state-active");
                        toHide.hide();
                        show();
                    }
                }
            });
    }

})(jQuery);
(function($) {

    var increments = 0;

    function addDescribedBy(elem, id) {
        var describedby = (elem.attr("aria-describedby") || "").split(/\s+/);
        describedby.push(id);
        elem
            .data("ui-tooltip-id", id)
            .attr("aria-describedby", $.trim(describedby.join(" ")));
    }

    function removeDescribedBy(elem) {
        var id = elem.data("ui-tooltip-id"),
            describedby = (elem.attr("aria-describedby") || "").split(/\s+/),
            index = $.inArray(id, describedby);
        if (index !== -1) {
            describedby.splice(index, 1);
        }

        elem.removeData("ui-tooltip-id");
        describedby = $.trim(describedby.join(" "));
        if (describedby) {
            elem.attr("aria-describedby", describedby);
        } else {
            elem.removeAttr("aria-describedby");
        }
    }

    $.widget("ui.tooltip",
    {
        version: "1.9.1",
        options: {
            content: function() {
                return $(this).attr("title");
            },
            hide: true,
            // Disabled elements have inconsistent behavior across browsers (#8661)
            items: "[title]:not([disabled])",
            position: {
                my: "left top+15",
                at: "left bottom",
                collision: "flipfit flipfit"
            },
            show: true,
            tooltipClass: null,
            track: false,

            // callbacks
            close: null,
            open: null
        },

        _create: function() {
            this._on({
                mouseover: "open",
                focusin: "open"
            });

            // IDs of generated tooltips, needed for destroy
            this.tooltips = {};
            // IDs of parent tooltips where we removed the title attribute
            this.parents = {};

            if (this.options.disabled) {
                this._disable();
            }
        },

        _setOption: function(key, value) {
            var that = this;

            if (key === "disabled") {
                this[value ? "_disable" : "_enable"]();
                this.options[key] = value;
                // disable element style changes
                return;
            }

            this._super(key, value);

            if (key === "content") {
                $.each(this.tooltips,
                    function(id, element) {
                        that._updateContent(element);
                    });
            }
        },

        _disable: function() {
            var that = this;

            // close open tooltips
            $.each(this.tooltips,
                function(id, element) {
                    var event = $.Event("blur");
                    event.target = event.currentTarget = element[0];
                    that.close(event, true);
                });

            // remove title attributes to prevent native tooltips
            this.element.find(this.options.items)
                .andSelf()
                .each(function() {
                    var element = $(this);
                    if (element.is("[title]")) {
                        element
                            .data("ui-tooltip-title", element.attr("title"))
                            .attr("title", "");
                    }
                });
        },

        _enable: function() {
            // restore title attributes
            this.element.find(this.options.items)
                .andSelf()
                .each(function() {
                    var element = $(this);
                    if (element.data("ui-tooltip-title")) {
                        element.attr("title", element.data("ui-tooltip-title"));
                    }
                });
        },

        open: function(event) {
            var that = this,
                target = $(event ? event.target : this.element)
                    // we need closest here due to mouseover bubbling,
                    // but always pointing at the same event target
                    .closest(this.options.items);

            // No element to show a tooltip for
            if (!target.length) {
                return;
            }

            // If the tooltip is open and we're tracking then reposition the tooltip.
            // This makes sure that a tracking tooltip doesn't obscure a focused element
            // if the user was hovering when the element gained focused.
            if (this.options.track && target.data("ui-tooltip-id")) {
                this._find(target)
                    .position($.extend({
                            of: target
                        },
                        this.options.position));
                // Stop tracking (#8622)
                this._off(this.document, "mousemove");
                return;
            }

            if (target.attr("title")) {
                target.data("ui-tooltip-title", target.attr("title"));
            }

            target.data("tooltip-open", true);

            // kill parent tooltips, custom or native, for hover
            if (event && event.type === "mouseover") {
                target.parents()
                    .each(function() {
                        var blurEvent;
                        if ($(this).data("tooltip-open")) {
                            blurEvent = $.Event("blur");
                            blurEvent.target = blurEvent.currentTarget = this;
                            that.close(blurEvent, true);
                        }
                        if (this.title) {
                            $(this).uniqueId();
                            that.parents[this.id] = {
                                element: this,
                                title: this.title
                            };
                            this.title = "";
                        }
                    });
            }

            this._updateContent(target, event);
        },

        _updateContent: function(target, event) {
            var content,
                contentOption = this.options.content,
                that = this;

            if (typeof contentOption === "string") {
                return this._open(event, target, contentOption);
            }

            content = contentOption.call(target[0],
                function(response) {
                    // ignore async response if tooltip was closed already
                    if (!target.data("tooltip-open")) {
                        return;
                    }
                    // IE may instantly serve a cached response for ajax requests
                    // delay this call to _open so the other call to _open runs first
                    that._delay(function() {
                        this._open(event, target, response);
                    });
                });
            if (content) {
                this._open(event, target, content);
            }
        },

        _open: function(event, target, content) {
            var tooltip,
                events,
                delayedShow,
                positionOption = $.extend({}, this.options.position);

            if (!content) {
                return;
            }

            // Content can be updated multiple times. If the tooltip already
            // exists, then just update the content and bail.
            tooltip = this._find(target);
            if (tooltip.length) {
                tooltip.find(".ui-tooltip-content").html(content);
                return;
            }

            // if we have a title, clear it to prevent the native tooltip
            // we have to check first to avoid defining a title if none exists
            // (we don't want to cause an element to start matching [title])
            //
            // We use removeAttr only for key events, to allow IE to export the correct
            // accessible attributes. For mouse events, set to empty string to avoid
            // native tooltip showing up (happens only when removing inside mouseover).
            if (target.is("[title]")) {
                if (event && event.type === "mouseover") {
                    target.attr("title", "");
                } else {
                    target.removeAttr("title");
                }
            }

            tooltip = this._tooltip(target);
            addDescribedBy(target, tooltip.attr("id"));
            tooltip.find(".ui-tooltip-content").html(content);

            function position(event) {
                positionOption.of = event;
                if (tooltip.is(":hidden")) {
                    return;
                }
                tooltip.position(positionOption);
            }

            if (this.options.track && event && /^mouse/.test(event.originalEvent.type)) {
                this._on(this.document,
                {
                    mousemove: position
                });
                // trigger once to override element-relative positioning
                position(event);
            } else {
                tooltip.position($.extend({
                        of: target
                    },
                    this.options.position));
            }

            tooltip.hide();

            this._show(tooltip, this.options.show);
            // Handle tracking tooltips that are shown with a delay (#8644). As soon
            // as the tooltip is visible, position the tooltip using the most recent
            // event.
            if (this.options.show && this.options.show.delay) {
                delayedShow = setInterval(function() {
                        if (tooltip.is(":visible")) {
                            position(positionOption.of);
                            clearInterval(delayedShow);
                        }
                    },
                    $.fx.interval);
            }

            this._trigger("open", event, { tooltip: tooltip });

            events = {
                keyup: function(event) {
                    if (event.keyCode === $.ui.keyCode.ESCAPE) {
                        var fakeEvent = $.Event(event);
                        fakeEvent.currentTarget = target[0];
                        this.close(fakeEvent, true);
                    }
                },
                remove: function() {
                    this._removeTooltip(tooltip);
                }
            };
            if (!event || event.type === "mouseover") {
                events.mouseleave = "close";
            }
            if (!event || event.type === "focusin") {
                events.focusout = "close";
            }
            this._on(target, events);
        },

        close: function(event) {
            var that = this,
                target = $(event ? event.currentTarget : this.element),
                tooltip = this._find(target);

            // disabling closes the tooltip, so we need to track when we're closing
            // to avoid an infinite loop in case the tooltip becomes disabled on close
            if (this.closing) {
                return;
            }

            // only set title if we had one before (see comment in _open())
            if (target.data("ui-tooltip-title")) {
                target.attr("title", target.data("ui-tooltip-title"));
            }

            removeDescribedBy(target);

            tooltip.stop(true);
            this._hide(tooltip,
                this.options.hide,
                function() {
                    that._removeTooltip($(this));
                });

            target.removeData("tooltip-open");
            this._off(target, "mouseleave focusout keyup");
            // Remove 'remove' binding only on delegated targets
            if (target[0] !== this.element[0]) {
                this._off(target, "remove");
            }
            this._off(this.document, "mousemove");

            if (event && event.type === "mouseleave") {
                $.each(this.parents,
                    function(id, parent) {
                        parent.element.title = parent.title;
                        delete that.parents[id];
                    });
            }

            this.closing = true;
            this._trigger("close", event, { tooltip: tooltip });
            this.closing = false;
        },

        _tooltip: function(element) {
            var id = "ui-tooltip-" + increments++,
                tooltip = $("<div>")
                    .attr({
                        id: id,
                        role: "tooltip"
                    })
                    .addClass("ui-tooltip ui-widget ui-corner-all ui-widget-content " +
                    (this.options.tooltipClass || ""));
            $("<div>")
                .addClass("ui-tooltip-content")
                .appendTo(tooltip);
            tooltip.appendTo(this.document[0].body);
            if ($.fn.bgiframe) {
                tooltip.bgiframe();
            }
            this.tooltips[id] = element;
            return tooltip;
        },

        _find: function(target) {
            var id = target.data("ui-tooltip-id");
            return id ? $("#" + id) : $();
        },

        _removeTooltip: function(tooltip) {
            tooltip.remove();
            delete this.tooltips[tooltip.attr("id")];
        },

        _destroy: function() {
            var that = this;

            // close open tooltips
            $.each(this.tooltips,
                function(id, element) {
                    // Delegate to close method to handle common cleanup
                    var event = $.Event("blur");
                    event.target = event.currentTarget = element[0];
                    that.close(event, true);

                    // Remove immediately; destroying an open tooltip doesn't use the
                    // hide animation
                    $("#" + id).remove();

                    // Restore the title
                    if (element.data("ui-tooltip-title")) {
                        element.attr("title", element.data("ui-tooltip-title"));
                        element.removeData("ui-tooltip-title");
                    }
                });
        }
    });

}(jQuery));