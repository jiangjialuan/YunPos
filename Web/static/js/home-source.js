//; window.console && console.log("%c\n                                   *********                           \n                             *******       *******                     \n                           ***                    ***                  \n                         **                         **                 \n                *****   **                           ***               \n             ****   *****                              **              \n           **                                           **             \n          *                                              *             \n         **              **                **            *             \n      ****               **        **      **           ******         \n    **                   **        **      **                 ****     \n  ***                              **                            ***   \n  *                                **                              **  \n *                               ****                               *  \n *                                                                  *  \n**                                                                  ** \n*                            **         **                           * \n**                            ***********                            * \n *                                ***                               ** \n  **                                                               **  \n   ***                                                            **   \n      ****                                                     ****    \n         ****                                               ****       \n            *************************************************          \n \n \u6b22\u8fce\u5149\u4e34\u0021\u0020\u5982\u6709\u95ee\u9898\uff0c\u8bf7\u8054\u7cfb\n \u0059\u005a\u0051\n \u0051\u0051\uff1a\u0035\u0035\u0034\u0033\u0039\u0033\u0031\u0030\u0039\n \u004d\u0061\u0069\u006c\uff1a\u0051\u0051\u90ae\u7bb1\u4f60\u61c2\u7684 \n", "font-family: Menlo, monospace");

var ToolMan = { events: function () { if (!ToolMan._eventsFactory) throw "ToolMan Events module isn't loaded"; return ToolMan._eventsFactory }, css: function () { if (!ToolMan._cssFactory) throw "ToolMan CSS module isn't loaded"; return ToolMan._cssFactory }, coordinates: function () { if (!ToolMan._coordinatesFactory) throw "ToolMan Coordinates module isn't loaded"; return ToolMan._coordinatesFactory }, drag: function () { if (!ToolMan._dragFactory) throw "ToolMan Drag module isn't loaded"; return ToolMan._dragFactory }, dragsort: function () { if (!ToolMan._dragsortFactory) throw "ToolMan DragSort module isn't loaded"; return ToolMan._dragsortFactory }, helpers: function () { return ToolMan._helpers }, cookies: function () { if (!ToolMan._cookieOven) throw "ToolMan Cookie module isn't loaded"; return ToolMan._cookieOven }, junkdrawer: function () { return ToolMan._junkdrawer } }
ToolMan._helpers = {
    map: function (array, func) { for (var i = 0, n = array.length; i < n; i++) func(array[i]) }, nextItem: function (item, nodeName) {
        if (item == null) return
        var next = item.nextSibling
        while (next != null) {
            if (next.nodeName == nodeName) return next
            next = next.nextSibling
        }
        return null
    }, previousItem: function (item, nodeName) {
        var previous = item.previousSibling
        while (previous != null) {
            if (previous.nodeName == nodeName) return previous
            previous = previous.previousSibling
        }
        return null
    }, moveBefore: function (item1, item2) {
        var parent = item1.parentNode
        parent.removeChild(item1)
        parent.insertBefore(item1, item2)
    }, moveAfter: function (item1, item2) {
        var parent = item1.parentNode
        parent.removeChild(item1)
        parent.insertBefore(item1, item2 ? item2.nextSibling : null)
    }
}
ToolMan._junkdrawer = {
    serializeList: function (list) {
        var items = list.getElementsByTagName("li")
        var array = new Array()
        for (var i = 0, n = items.length; i < n; i++) {
            var item = items[i]
            array.push(ToolMan.junkdrawer()._identifier(item))
        }
        return array.join('|')
    }, inspectListOrder: function (id) { alert(ToolMan.junkdrawer().serializeList(document.getElementById(id))) }, restoreListOrder: function (listID) {
        var list = document.getElementById(listID)
        if (list == null) return
        var cookie = ToolMan.cookies().get("list-" + listID)
        if (!cookie) return; var IDs = cookie.split('|')
        var items = ToolMan.junkdrawer()._itemsByID(list)
        for (var i = 0, n = IDs.length; i < n; i++) {
            var itemID = IDs[i]
            if (itemID in items) {
                var item = items[itemID]
                list.removeChild(item)
                list.insertBefore(item, null)
            }
        }
    }, _identifier: function (item) {
        var trim = ToolMan.junkdrawer().trim
        var identifier
        identifier = trim(item.getAttribute("id"))
        if (identifier != null && identifier.length > 0) return identifier; identifier = trim(item.getAttribute("itemID"))
        if (identifier != null && identifier.length > 0) return identifier; return trim(item.innerHTML)
    }, _itemsByID: function (list) {
        var array = new Array()
        var items = list.getElementsByTagName('li')
        for (var i = 0, n = items.length; i < n; i++) {
            var item = items[i]
            array[ToolMan.junkdrawer()._identifier(item)] = item
        }
        return array
    }, trim: function (text) {
        if (text == null) return null
        return text.replace(/^(\s+)?(.*\S)(\s+)?$/, '$2')
    }
}
ToolMan._eventsFactory = {
    fix: function (event) {
        if (!event) event = window.event
        if (event.target) { if (event.target.nodeType == 3) event.target = event.target.parentNode } else if (event.srcElement) { event.target = event.srcElement }
        return event
    },
    register: function (element, type, func) {
        if (element.addEventListener) { element.addEventListener(type, func, false) } else if (element.attachEvent) {
            if (!element._listeners) element._listeners = new Array()
            if (!element._listeners[type]) element._listeners[type] = new Array()
            var workaroundFunc = function () { func.apply(element, new Array()) }
            element._listeners[type][func] = workaroundFunc
            element.attachEvent('on' + type, workaroundFunc)
        }
    },
    unregister: function (element, type, func) { if (element.removeEventListener) { element.removeEventListener(type, func, false) } else if (element.detachEvent) { if (element._listeners && element._listeners[type] && element._listeners[type][func]) { element.detachEvent('on' + type, element._listeners[type][func]) } } }
}
ToolMan._cssFactory = {
    readStyle: function (element, property) {
        if (element.style[property]) { return element.style[property] } else if (element.currentStyle) { return element.currentStyle[property] } else if (document.defaultView && document.defaultView.getComputedStyle) {
            var style = document.defaultView.getComputedStyle(element, null)
            return style.getPropertyValue(property)
        } else { return null }
    }
}
ToolMan._coordinatesFactory = {
    create: function (x, y) { return new _ToolManCoordinate(this, x, y) }, origin: function () { return this.create(0, 0) }, topLeftPosition: function (element) {
        var left = parseInt(ToolMan.css().readStyle(element, "left"))
        var left = isNaN(left) ? 0 : left
        var top = parseInt(ToolMan.css().readStyle(element, "top"))
        var top = isNaN(top) ? 0 : top
        return this.create(left, top)
    }, bottomRightPosition: function (element) { return this.topLeftPosition(element).plus(this._size(element)) }, topLeftOffset: function (element) {
        var offset = this._offset(element)
        var parent = element.offsetParent
        while (parent) {
            offset = offset.plus(this._offset(parent))
            parent = parent.offsetParent
        }
        return offset
    }, bottomRightOffset: function (element) { return this.topLeftOffset(element).plus(this.create(element.offsetWidth, element.offsetHeight)) }, scrollOffset: function () { if (window.pageXOffset) { return this.create(window.pageXOffset, window.pageYOffset) } else if (document.documentElement) { return this.create(document.body.scrollLeft + document.documentElement.scrollLeft, document.body.scrollTop + document.documentElement.scrollTop) } else if (document.body.scrollLeft >= 0) { return this.create(document.body.scrollLeft, document.body.scrollTop) } else { return this.create(0, 0) } }, clientSize: function () { if (window.innerHeight >= 0) { return this.create(window.innerWidth, window.innerHeight) } else if (document.documentElement) { return this.create(document.documentElement.clientWidth, document.documentElement.clientHeight) } else if (document.body.clientHeight >= 0) { return this.create(document.body.clientWidth, document.body.clientHeight) } else { return this.create(0, 0) } }, mousePosition: function (event) {
        event = ToolMan.events().fix(event)
        return this.create(event.clientX, event.clientY)
    }, mouseOffset: function (event) {
        event = ToolMan.events().fix(event)
        if (event.pageX >= 0 || event.pageX < 0) { return this.create(event.pageX, event.pageY) } else if (event.clientX >= 0 || event.clientX < 0) { return this.mousePosition(event).plus(this.scrollOffset()) }
    }, _size: function (element) { return this.create(element.offsetWidth, element.offsetHeight) }, _offset: function (element) { return this.create(element.offsetLeft, element.offsetTop) }
}
function _ToolManCoordinate(factory, x, y) {
    this.factory = factory
    this.x = isNaN(x) ? 0 : x
    this.y = isNaN(y) ? 0 : y
}
_ToolManCoordinate.prototype = {
    toString: function () { return "(" + this.x + "," + this.y + ")" }, plus: function (that) { return this.factory.create(this.x + that.x, this.y + that.y) }, minus: function (that) { return this.factory.create(this.x - that.x, this.y - that.y) }, min: function (that) { return this.factory.create(Math.min(this.x, that.x), Math.min(this.y, that.y)) }, max: function (that) { return this.factory.create(Math.max(this.x, that.x), Math.max(this.y, that.y)) }, constrainTo: function (one, two) {
        var min = one.min(two)
        var max = one.max(two)
        return this.max(min).min(max)
    }, distance: function (that) { return Math.sqrt(Math.pow(this.x - that.x, 2) + Math.pow(this.y - that.y, 2)) }, reposition: function (element) {
        element.style["top"] = this.y + "px"
        element.style["left"] = this.x + "px"
    }
}
ToolMan._dragFactory = {
    createSimpleGroup: function (element, handle) {
        handle = handle ? handle : element
        var group = this.createGroup(element)
        group.setHandle(handle)
        group.transparentDrag()
        group.onTopWhileDragging()
        return group
    }, createGroup: function (element) {
        var group = new _ToolManDragGroup(this, element)
        var position = ToolMan.css().readStyle(element, 'position')
        if (position == 'static') { element.style["position"] = 'relative' } else if (position == 'absolute') { ToolMan.coordinates().topLeftOffset(element).reposition(element) }
        group.register('draginit', this._showDragEventStatus)
        group.register('dragmove', this._showDragEventStatus)
        group.register('dragend', this._showDragEventStatus)
        return group
    }, _showDragEventStatus: function (dragEvent) { window.status = dragEvent.toString() }, constraints: function () { return this._constraintFactory }, _createEvent: function (type, event, group) { return new _ToolManDragEvent(type, event, group) }
}
function _ToolManDragGroup(factory, element) {
    this.factory = factory
    this.element = element
    this._handle = null
    this._thresholdDistance = 0
    this._transforms = new Array()
    this._listeners = new Array()
    this._listeners['draginit'] = new Array()
    this._listeners['dragstart'] = new Array()
    this._listeners['dragmove'] = new Array()
    this._listeners['dragend'] = new Array()
}
_ToolManDragGroup.prototype = {
    setHandle: function (handle) {
        var events = ToolMan.events()
        handle.toolManDragGroup = this
        events.register(handle, 'mousedown', this._dragInit)
        handle.onmousedown = function () { return false }
        if (this.element != handle)
            events.unregister(this.element, 'mousedown', this._dragInit)
    }, register: function (type, func) { this._listeners[type].push(func) }, addTransform: function (transformFunc) { this._transforms.push(transformFunc) }, verticalOnly: function () { this.addTransform(this.factory.constraints().vertical()) }, horizontalOnly: function () { this.addTransform(this.factory.constraints().horizontal()) }, setThreshold: function (thresholdDistance) { this._thresholdDistance = thresholdDistance }, transparentDrag: function (opacity) {
        var opacity = typeof (opacity) != "undefined" ? opacity : 0.75; var originalOpacity = ToolMan.css().readStyle(this.element, "opacity")
        this.register('dragstart', function (dragEvent) {
            var element = dragEvent.group.element
            element.style.opacity = opacity
            element.style.filter = 'alpha(opacity=' + (opacity * 100) + ')'
        })
        this.register('dragend', function (dragEvent) {
            var element = dragEvent.group.element
            element.style.opacity = originalOpacity
            element.style.filter = 'alpha(opacity=100)'
        })
    }, onTopWhileDragging: function (zIndex) {
        var zIndex = typeof (zIndex) != "undefined" ? zIndex : 100000; var originalZIndex = ToolMan.css().readStyle(this.element, "z-index")
        this.register('dragstart', function (dragEvent) { dragEvent.group.element.style.zIndex = zIndex })
        this.register('dragend', function (dragEvent) { dragEvent.group.element.style.zIndex = originalZIndex })
    }, _dragInit: function (event) {
        event = ToolMan.events().fix(event)
        var group = document.toolManDragGroup = this.toolManDragGroup
        var dragEvent = group.factory._createEvent('draginit', event, group)
        group._isThresholdExceeded = false
        group._initialMouseOffset = dragEvent.mouseOffset
        group._grabOffset = dragEvent.mouseOffset.minus(dragEvent.topLeftOffset)
        ToolMan.events().register(document, 'mousemove', group._drag)
        document.onmousemove = function () { return false }
        ToolMan.events().register(document, 'mouseup', group._dragEnd)
        group._notifyListeners(dragEvent)
    }, _drag: function (event) {
        event = ToolMan.events().fix(event)
        var coordinates = ToolMan.coordinates()
        var group = this.toolManDragGroup
        if (!group) return
        var dragEvent = group.factory._createEvent('dragmove', event, group)
        var newTopLeftOffset = dragEvent.mouseOffset.minus(group._grabOffset)
        if (!group._isThresholdExceeded) {
            var distance = dragEvent.mouseOffset.distance(group._initialMouseOffset)
            if (distance < group._thresholdDistance) return
            group._isThresholdExceeded = true
            group._notifyListeners(group.factory._createEvent('dragstart', event, group))
        }
        for (i in group._transforms) {
            var transform = group._transforms[i]
            newTopLeftOffset = transform(newTopLeftOffset, dragEvent)
        }
        var dragDelta = newTopLeftOffset.minus(dragEvent.topLeftOffset)
        var newTopLeftPosition = dragEvent.topLeftPosition.plus(dragDelta)
        newTopLeftPosition.reposition(group.element)
        dragEvent.transformedMouseOffset = newTopLeftOffset.plus(group._grabOffset)
        group._notifyListeners(dragEvent)
        var errorDelta = newTopLeftOffset.minus(coordinates.topLeftOffset(group.element))
        if (errorDelta.x != 0 || errorDelta.y != 0) { coordinates.topLeftPosition(group.element).plus(errorDelta).reposition(group.element) }
    }, _dragEnd: function (event) {
        event = ToolMan.events().fix(event)
        var group = this.toolManDragGroup
        var dragEvent = group.factory._createEvent('dragend', event, group)
        group._notifyListeners(dragEvent)
        this.toolManDragGroup = null
        ToolMan.events().unregister(document, 'mousemove', group._drag)
        document.onmousemove = null
        ToolMan.events().unregister(document, 'mouseup', group._dragEnd)
    }, _notifyListeners: function (dragEvent) {
        var listeners = this._listeners[dragEvent.type]
        for (i in listeners) { listeners[i](dragEvent) }
    }
}
function _ToolManDragEvent(type, event, group) {
    this.type = type
    this.group = group
    this.mousePosition = ToolMan.coordinates().mousePosition(event)
    this.mouseOffset = ToolMan.coordinates().mouseOffset(event)
    this.transformedMouseOffset = this.mouseOffset
    this.topLeftPosition = ToolMan.coordinates().topLeftPosition(group.element)
    this.topLeftOffset = ToolMan.coordinates().topLeftOffset(group.element)
}
_ToolManDragEvent.prototype = { toString: function () { return "mouse: " + this.mousePosition + this.mouseOffset + "    " + "xmouse: " + this.transformedMouseOffset + "    " + "left,top: " + this.topLeftPosition + this.topLeftOffset } }
ToolMan._dragFactory._constraintFactory = {
    vertical: function () {
        return function (coordinate, dragEvent) {
            var x = dragEvent.topLeftOffset.x
            return coordinate.x != x ? coordinate.factory.create(x, coordinate.y) : coordinate
        }
    }, horizontal: function () {
        return function (coordinate, dragEvent) {
            var y = dragEvent.topLeftOffset.y
            return coordinate.y != y ? coordinate.factory.create(coordinate.x, y) : coordinate
        }
    }
}
ToolMan._dragsortFactory = {
    makeSortable: function (item) {
        var group = ToolMan.drag().createSimpleGroup(item)
        group.register('dragstart', this._onDragStart)
        group.register('dragmove', this._onDragMove)
        group.register('dragend', this._onDragEnd)
        return group
    }, makeListSortable: function (list) {
        var helpers = ToolMan.helpers()
        var coordinates = ToolMan.coordinates()
        var items = list.getElementsByTagName("li")
        helpers.map(items, function (item) {
            var dragGroup = dragsort.makeSortable(item)
            dragGroup.setThreshold(4)
            var min, max
            dragGroup.addTransform(function (coordinate, dragEvent) { return coordinate.constrainTo(min, max) })
            dragGroup.register('dragstart', function () {
                var items = list.getElementsByTagName("li")
                min = max = coordinates.topLeftOffset(items[0])
                for (var i = 1, n = items.length; i < n; i++) {
                    var offset = coordinates.topLeftOffset(items[i])
                    min = min.min(offset)
                    max = max.max(offset)
                }
            })
        })
        for (var i = 1, n = arguments.length; i < n; i++)
            helpers.map(items, arguments[i])
    }, _onDragStart: function (dragEvent) { }, _onDragMove: function (dragEvent) {
        var helpers = ToolMan.helpers()
        var coordinates = ToolMan.coordinates()
        var item = dragEvent.group.element
        var xmouse = dragEvent.transformedMouseOffset
        var moveTo = null
        var previous = helpers.previousItem(item, item.nodeName)
        while (previous != null) {
            var bottomRight = coordinates.bottomRightOffset(previous)
            if (xmouse.y <= bottomRight.y && xmouse.x <= bottomRight.x) { moveTo = previous }
            previous = helpers.previousItem(previous, item.nodeName)
        }
        if (moveTo != null) {
            helpers.moveBefore(item, moveTo)
            return
        }
        var next = helpers.nextItem(item, item.nodeName)
        while (next != null) {
            var topLeft = coordinates.topLeftOffset(next)
            if (topLeft.y <= xmouse.y && topLeft.x <= xmouse.x) { moveTo = next }
            next = helpers.nextItem(next, item.nodeName)
        }
        if (moveTo != null) {
            helpers.moveBefore(item, helpers.nextItem(moveTo, item.nodeName))
            return
        }
    }, _onDragEnd: function (dragEvent) { ToolMan.coordinates().create(0, 0).reposition(dragEvent.group.element) }
}
ToolMan._cookieOven = {
    set: function (name, value, expirationInDays) {
        if (expirationInDays) {
            var date = new Date()
            date.setTime(date.getTime() + (expirationInDays * 24 * 60 * 60 * 1000))
            var expires = "; expires=" + date.toGMTString()
        } else { var expires = "" }
        document.cookie = name + "=" + value + expires + "; path=/"
    }, get: function (name) {
        var namePattern = name + "="
        var cookies = document.cookie.split(';')
        for (var i = 0, n = cookies.length; i < n; i++) {
            var c = cookies[i]
            while (c.charAt(0) == ' ') c = c.substring(1, c.length)
            if (c.indexOf(namePattern) == 0)
                return c.substring(namePattern.length, c.length)
        }
        return null
    }, eraseCookie: function (name) { createCookie(name, "", -1) }
}






;/*! jQuery v1.9.1 | (c) 2005, 2012 jQuery Foundation, Inc. | jquery.org/license
//@ sourceMappingURL=jquery.min.map
*/(function (e, t) {
    var n, r, i = typeof t, o = e.document, a = e.location, s = e.jQuery, u = e.$, l = {}, c = [], p = "1.9.1", f = c.concat, d = c.push, h = c.slice, g = c.indexOf, m = l.toString, y = l.hasOwnProperty, v = p.trim, b = function (e, t) { return new b.fn.init(e, t, r) }, x = /[+-]?(?:\d*\.|)\d+(?:[eE][+-]?\d+|)/.source, w = /\S+/g, T = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g, N = /^(?:(<[\w\W]+>)[^>]*|#([\w-]*))$/, C = /^<(\w+)\s*\/?>(?:<\/\1>|)$/, k = /^[\],:{}\s]*$/, E = /(?:^|:|,)(?:\s*\[)+/g, S = /\\(?:["\\\/bfnrt]|u[\da-fA-F]{4})/g, A = /"[^"\\\r\n]*"|true|false|null|-?(?:\d+\.|)\d+(?:[eE][+-]?\d+|)/g, j = /^-ms-/, D = /-([\da-z])/gi, L = function (e, t) { return t.toUpperCase() }, H = function (e) { (o.addEventListener || "load" === e.type || "complete" === o.readyState) && (q(), b.ready()) }, q = function () { o.addEventListener ? (o.removeEventListener("DOMContentLoaded", H, !1), e.removeEventListener("load", H, !1)) : (o.detachEvent("onreadystatechange", H), e.detachEvent("onload", H)) }; b.fn = b.prototype = { jquery: p, constructor: b, init: function (e, n, r) { var i, a; if (!e) return this; if ("string" == typeof e) { if (i = "<" === e.charAt(0) && ">" === e.charAt(e.length - 1) && e.length >= 3 ? [null, e, null] : N.exec(e), !i || !i[1] && n) return !n || n.jquery ? (n || r).find(e) : this.constructor(n).find(e); if (i[1]) { if (n = n instanceof b ? n[0] : n, b.merge(this, b.parseHTML(i[1], n && n.nodeType ? n.ownerDocument || n : o, !0)), C.test(i[1]) && b.isPlainObject(n)) for (i in n) b.isFunction(this[i]) ? this[i](n[i]) : this.attr(i, n[i]); return this } if (a = o.getElementById(i[2]), a && a.parentNode) { if (a.id !== i[2]) return r.find(e); this.length = 1, this[0] = a } return this.context = o, this.selector = e, this } return e.nodeType ? (this.context = this[0] = e, this.length = 1, this) : b.isFunction(e) ? r.ready(e) : (e.selector !== t && (this.selector = e.selector, this.context = e.context), b.makeArray(e, this)) }, selector: "", length: 0, size: function () { return this.length }, toArray: function () { return h.call(this) }, get: function (e) { return null == e ? this.toArray() : 0 > e ? this[this.length + e] : this[e] }, pushStack: function (e) { var t = b.merge(this.constructor(), e); return t.prevObject = this, t.context = this.context, t }, each: function (e, t) { return b.each(this, e, t) }, ready: function (e) { return b.ready.promise().done(e), this }, slice: function () { return this.pushStack(h.apply(this, arguments)) }, first: function () { return this.eq(0) }, last: function () { return this.eq(-1) }, eq: function (e) { var t = this.length, n = +e + (0 > e ? t : 0); return this.pushStack(n >= 0 && t > n ? [this[n]] : []) }, map: function (e) { return this.pushStack(b.map(this, function (t, n) { return e.call(t, n, t) })) }, end: function () { return this.prevObject || this.constructor(null) }, push: d, sort: [].sort, splice: [].splice }, b.fn.init.prototype = b.fn, b.extend = b.fn.extend = function () { var e, n, r, i, o, a, s = arguments[0] || {}, u = 1, l = arguments.length, c = !1; for ("boolean" == typeof s && (c = s, s = arguments[1] || {}, u = 2), "object" == typeof s || b.isFunction(s) || (s = {}), l === u && (s = this, --u) ; l > u; u++) if (null != (o = arguments[u])) for (i in o) e = s[i], r = o[i], s !== r && (c && r && (b.isPlainObject(r) || (n = b.isArray(r))) ? (n ? (n = !1, a = e && b.isArray(e) ? e : []) : a = e && b.isPlainObject(e) ? e : {}, s[i] = b.extend(c, a, r)) : r !== t && (s[i] = r)); return s }, b.extend({ noConflict: function (t) { return e.$ === b && (e.$ = u), t && e.jQuery === b && (e.jQuery = s), b }, isReady: !1, readyWait: 1, holdReady: function (e) { e ? b.readyWait++ : b.ready(!0) }, ready: function (e) { if (e === !0 ? !--b.readyWait : !b.isReady) { if (!o.body) return setTimeout(b.ready); b.isReady = !0, e !== !0 && --b.readyWait > 0 || (n.resolveWith(o, [b]), b.fn.trigger && b(o).trigger("ready").off("ready")) } }, isFunction: function (e) { return "function" === b.type(e) }, isArray: Array.isArray || function (e) { return "array" === b.type(e) }, isWindow: function (e) { return null != e && e == e.window }, isNumeric: function (e) { return !isNaN(parseFloat(e)) && isFinite(e) }, type: function (e) { return null == e ? e + "" : "object" == typeof e || "function" == typeof e ? l[m.call(e)] || "object" : typeof e }, isPlainObject: function (e) { if (!e || "object" !== b.type(e) || e.nodeType || b.isWindow(e)) return !1; try { if (e.constructor && !y.call(e, "constructor") && !y.call(e.constructor.prototype, "isPrototypeOf")) return !1 } catch (n) { return !1 } var r; for (r in e); return r === t || y.call(e, r) }, isEmptyObject: function (e) { var t; for (t in e) return !1; return !0 }, error: function (e) { throw Error(e) }, parseHTML: function (e, t, n) { if (!e || "string" != typeof e) return null; "boolean" == typeof t && (n = t, t = !1), t = t || o; var r = C.exec(e), i = !n && []; return r ? [t.createElement(r[1])] : (r = b.buildFragment([e], t, i), i && b(i).remove(), b.merge([], r.childNodes)) }, parseJSON: function (n) { return e.JSON && e.JSON.parse ? e.JSON.parse(n) : null === n ? n : "string" == typeof n && (n = b.trim(n), n && k.test(n.replace(S, "@").replace(A, "]").replace(E, ""))) ? Function("return " + n)() : (b.error("Invalid JSON: " + n), t) }, parseXML: function (n) { var r, i; if (!n || "string" != typeof n) return null; try { e.DOMParser ? (i = new DOMParser, r = i.parseFromString(n, "text/xml")) : (r = new ActiveXObject("Microsoft.XMLDOM"), r.async = "false", r.loadXML(n)) } catch (o) { r = t } return r && r.documentElement && !r.getElementsByTagName("parsererror").length || b.error("Invalid XML: " + n), r }, noop: function () { }, globalEval: function (t) { t && b.trim(t) && (e.execScript || function (t) { e.eval.call(e, t) })(t) }, camelCase: function (e) { return e.replace(j, "ms-").replace(D, L) }, nodeName: function (e, t) { return e.nodeName && e.nodeName.toLowerCase() === t.toLowerCase() }, each: function (e, t, n) { var r, i = 0, o = e.length, a = M(e); if (n) { if (a) { for (; o > i; i++) if (r = t.apply(e[i], n), r === !1) break } else for (i in e) if (r = t.apply(e[i], n), r === !1) break } else if (a) { for (; o > i; i++) if (r = t.call(e[i], i, e[i]), r === !1) break } else for (i in e) if (r = t.call(e[i], i, e[i]), r === !1) break; return e }, trim: v && !v.call("\ufeff\u00a0") ? function (e) { return null == e ? "" : v.call(e) } : function (e) { return null == e ? "" : (e + "").replace(T, "") }, makeArray: function (e, t) { var n = t || []; return null != e && (M(Object(e)) ? b.merge(n, "string" == typeof e ? [e] : e) : d.call(n, e)), n }, inArray: function (e, t, n) { var r; if (t) { if (g) return g.call(t, e, n); for (r = t.length, n = n ? 0 > n ? Math.max(0, r + n) : n : 0; r > n; n++) if (n in t && t[n] === e) return n } return -1 }, merge: function (e, n) { var r = n.length, i = e.length, o = 0; if ("number" == typeof r) for (; r > o; o++) e[i++] = n[o]; else while (n[o] !== t) e[i++] = n[o++]; return e.length = i, e }, grep: function (e, t, n) { var r, i = [], o = 0, a = e.length; for (n = !!n; a > o; o++) r = !!t(e[o], o), n !== r && i.push(e[o]); return i }, map: function (e, t, n) { var r, i = 0, o = e.length, a = M(e), s = []; if (a) for (; o > i; i++) r = t(e[i], i, n), null != r && (s[s.length] = r); else for (i in e) r = t(e[i], i, n), null != r && (s[s.length] = r); return f.apply([], s) }, guid: 1, proxy: function (e, n) { var r, i, o; return "string" == typeof n && (o = e[n], n = e, e = o), b.isFunction(e) ? (r = h.call(arguments, 2), i = function () { return e.apply(n || this, r.concat(h.call(arguments))) }, i.guid = e.guid = e.guid || b.guid++, i) : t }, access: function (e, n, r, i, o, a, s) { var u = 0, l = e.length, c = null == r; if ("object" === b.type(r)) { o = !0; for (u in r) b.access(e, n, u, r[u], !0, a, s) } else if (i !== t && (o = !0, b.isFunction(i) || (s = !0), c && (s ? (n.call(e, i), n = null) : (c = n, n = function (e, t, n) { return c.call(b(e), n) })), n)) for (; l > u; u++) n(e[u], r, s ? i : i.call(e[u], u, n(e[u], r))); return o ? e : c ? n.call(e) : l ? n(e[0], r) : a }, now: function () { return (new Date).getTime() } }), b.ready.promise = function (t) { if (!n) if (n = b.Deferred(), "complete" === o.readyState) setTimeout(b.ready); else if (o.addEventListener) o.addEventListener("DOMContentLoaded", H, !1), e.addEventListener("load", H, !1); else { o.attachEvent("onreadystatechange", H), e.attachEvent("onload", H); var r = !1; try { r = null == e.frameElement && o.documentElement } catch (i) { } r && r.doScroll && function a() { if (!b.isReady) { try { r.doScroll("left") } catch (e) { return setTimeout(a, 50) } q(), b.ready() } }() } return n.promise(t) }, b.each("Boolean Number String Function Array Date RegExp Object Error".split(" "), function (e, t) { l["[object " + t + "]"] = t.toLowerCase() }); function M(e) { var t = e.length, n = b.type(e); return b.isWindow(e) ? !1 : 1 === e.nodeType && t ? !0 : "array" === n || "function" !== n && (0 === t || "number" == typeof t && t > 0 && t - 1 in e) } r = b(o); var _ = {}; function F(e) { var t = _[e] = {}; return b.each(e.match(w) || [], function (e, n) { t[n] = !0 }), t } b.Callbacks = function (e) { e = "string" == typeof e ? _[e] || F(e) : b.extend({}, e); var n, r, i, o, a, s, u = [], l = !e.once && [], c = function (t) { for (r = e.memory && t, i = !0, a = s || 0, s = 0, o = u.length, n = !0; u && o > a; a++) if (u[a].apply(t[0], t[1]) === !1 && e.stopOnFalse) { r = !1; break } n = !1, u && (l ? l.length && c(l.shift()) : r ? u = [] : p.disable()) }, p = { add: function () { if (u) { var t = u.length; (function i(t) { b.each(t, function (t, n) { var r = b.type(n); "function" === r ? e.unique && p.has(n) || u.push(n) : n && n.length && "string" !== r && i(n) }) })(arguments), n ? o = u.length : r && (s = t, c(r)) } return this }, remove: function () { return u && b.each(arguments, function (e, t) { var r; while ((r = b.inArray(t, u, r)) > -1) u.splice(r, 1), n && (o >= r && o--, a >= r && a--) }), this }, has: function (e) { return e ? b.inArray(e, u) > -1 : !(!u || !u.length) }, empty: function () { return u = [], this }, disable: function () { return u = l = r = t, this }, disabled: function () { return !u }, lock: function () { return l = t, r || p.disable(), this }, locked: function () { return !l }, fireWith: function (e, t) { return t = t || [], t = [e, t.slice ? t.slice() : t], !u || i && !l || (n ? l.push(t) : c(t)), this }, fire: function () { return p.fireWith(this, arguments), this }, fired: function () { return !!i } }; return p }, b.extend({ Deferred: function (e) { var t = [["resolve", "done", b.Callbacks("once memory"), "resolved"], ["reject", "fail", b.Callbacks("once memory"), "rejected"], ["notify", "progress", b.Callbacks("memory")]], n = "pending", r = { state: function () { return n }, always: function () { return i.done(arguments).fail(arguments), this }, then: function () { var e = arguments; return b.Deferred(function (n) { b.each(t, function (t, o) { var a = o[0], s = b.isFunction(e[t]) && e[t]; i[o[1]](function () { var e = s && s.apply(this, arguments); e && b.isFunction(e.promise) ? e.promise().done(n.resolve).fail(n.reject).progress(n.notify) : n[a + "With"](this === r ? n.promise() : this, s ? [e] : arguments) }) }), e = null }).promise() }, promise: function (e) { return null != e ? b.extend(e, r) : r } }, i = {}; return r.pipe = r.then, b.each(t, function (e, o) { var a = o[2], s = o[3]; r[o[1]] = a.add, s && a.add(function () { n = s }, t[1 ^ e][2].disable, t[2][2].lock), i[o[0]] = function () { return i[o[0] + "With"](this === i ? r : this, arguments), this }, i[o[0] + "With"] = a.fireWith }), r.promise(i), e && e.call(i, i), i }, when: function (e) { var t = 0, n = h.call(arguments), r = n.length, i = 1 !== r || e && b.isFunction(e.promise) ? r : 0, o = 1 === i ? e : b.Deferred(), a = function (e, t, n) { return function (r) { t[e] = this, n[e] = arguments.length > 1 ? h.call(arguments) : r, n === s ? o.notifyWith(t, n) : --i || o.resolveWith(t, n) } }, s, u, l; if (r > 1) for (s = Array(r), u = Array(r), l = Array(r) ; r > t; t++) n[t] && b.isFunction(n[t].promise) ? n[t].promise().done(a(t, l, n)).fail(o.reject).progress(a(t, u, s)) : --i; return i || o.resolveWith(l, n), o.promise() } }), b.support = function () { var t, n, r, a, s, u, l, c, p, f, d = o.createElement("div"); if (d.setAttribute("className", "t"), d.innerHTML = "  <link/><table></table><a href='/a'>a</a><input type='checkbox'/>", n = d.getElementsByTagName("*"), r = d.getElementsByTagName("a")[0], !n || !r || !n.length) return {}; s = o.createElement("select"), l = s.appendChild(o.createElement("option")), a = d.getElementsByTagName("input")[0], r.style.cssText = "top:1px;float:left;opacity:.5", t = { getSetAttribute: "t" !== d.className, leadingWhitespace: 3 === d.firstChild.nodeType, tbody: !d.getElementsByTagName("tbody").length, htmlSerialize: !!d.getElementsByTagName("link").length, style: /top/.test(r.getAttribute("style")), hrefNormalized: "/a" === r.getAttribute("href"), opacity: /^0.5/.test(r.style.opacity), cssFloat: !!r.style.cssFloat, checkOn: !!a.value, optSelected: l.selected, enctype: !!o.createElement("form").enctype, html5Clone: "<:nav></:nav>" !== o.createElement("nav").cloneNode(!0).outerHTML, boxModel: "CSS1Compat" === o.compatMode, deleteExpando: !0, noCloneEvent: !0, inlineBlockNeedsLayout: !1, shrinkWrapBlocks: !1, reliableMarginRight: !0, boxSizingReliable: !0, pixelPosition: !1 }, a.checked = !0, t.noCloneChecked = a.cloneNode(!0).checked, s.disabled = !0, t.optDisabled = !l.disabled; try { delete d.test } catch (h) { t.deleteExpando = !1 } a = o.createElement("input"), a.setAttribute("value", ""), t.input = "" === a.getAttribute("value"), a.value = "t", a.setAttribute("type", "radio"), t.radioValue = "t" === a.value, a.setAttribute("checked", "t"), a.setAttribute("name", "t"), u = o.createDocumentFragment(), u.appendChild(a), t.appendChecked = a.checked, t.checkClone = u.cloneNode(!0).cloneNode(!0).lastChild.checked, d.attachEvent && (d.attachEvent("onclick", function () { t.noCloneEvent = !1 }), d.cloneNode(!0).click()); for (f in { submit: !0, change: !0, focusin: !0 }) d.setAttribute(c = "on" + f, "t"), t[f + "Bubbles"] = c in e || d.attributes[c].expando === !1; return d.style.backgroundClip = "content-box", d.cloneNode(!0).style.backgroundClip = "", t.clearCloneStyle = "content-box" === d.style.backgroundClip, b(function () { var n, r, a, s = "padding:0;margin:0;border:0;display:block;box-sizing:content-box;-moz-box-sizing:content-box;-webkit-box-sizing:content-box;", u = o.getElementsByTagName("body")[0]; u && (n = o.createElement("div"), n.style.cssText = "border:0;width:0;height:0;position:absolute;top:0;left:-9999px;margin-top:1px", u.appendChild(n).appendChild(d), d.innerHTML = "<table><tr><td></td><td>t</td></tr></table>", a = d.getElementsByTagName("td"), a[0].style.cssText = "padding:0;margin:0;border:0;display:none", p = 0 === a[0].offsetHeight, a[0].style.display = "", a[1].style.display = "none", t.reliableHiddenOffsets = p && 0 === a[0].offsetHeight, d.innerHTML = "", d.style.cssText = "box-sizing:border-box;-moz-box-sizing:border-box;-webkit-box-sizing:border-box;padding:1px;border:1px;display:block;width:4px;margin-top:1%;position:absolute;top:1%;", t.boxSizing = 4 === d.offsetWidth, t.doesNotIncludeMarginInBodyOffset = 1 !== u.offsetTop, e.getComputedStyle && (t.pixelPosition = "1%" !== (e.getComputedStyle(d, null) || {}).top, t.boxSizingReliable = "4px" === (e.getComputedStyle(d, null) || { width: "4px" }).width, r = d.appendChild(o.createElement("div")), r.style.cssText = d.style.cssText = s, r.style.marginRight = r.style.width = "0", d.style.width = "1px", t.reliableMarginRight = !parseFloat((e.getComputedStyle(r, null) || {}).marginRight)), typeof d.style.zoom !== i && (d.innerHTML = "", d.style.cssText = s + "width:1px;padding:1px;display:inline;zoom:1", t.inlineBlockNeedsLayout = 3 === d.offsetWidth, d.style.display = "block", d.innerHTML = "<div></div>", d.firstChild.style.width = "5px", t.shrinkWrapBlocks = 3 !== d.offsetWidth, t.inlineBlockNeedsLayout && (u.style.zoom = 1)), u.removeChild(n), n = d = a = r = null) }), n = s = u = l = r = a = null, t }(); var O = /(?:\{[\s\S]*\}|\[[\s\S]*\])$/, B = /([A-Z])/g; function P(e, n, r, i) { if (b.acceptData(e)) { var o, a, s = b.expando, u = "string" == typeof n, l = e.nodeType, p = l ? b.cache : e, f = l ? e[s] : e[s] && s; if (f && p[f] && (i || p[f].data) || !u || r !== t) return f || (l ? e[s] = f = c.pop() || b.guid++ : f = s), p[f] || (p[f] = {}, l || (p[f].toJSON = b.noop)), ("object" == typeof n || "function" == typeof n) && (i ? p[f] = b.extend(p[f], n) : p[f].data = b.extend(p[f].data, n)), o = p[f], i || (o.data || (o.data = {}), o = o.data), r !== t && (o[b.camelCase(n)] = r), u ? (a = o[n], null == a && (a = o[b.camelCase(n)])) : a = o, a } } function R(e, t, n) { if (b.acceptData(e)) { var r, i, o, a = e.nodeType, s = a ? b.cache : e, u = a ? e[b.expando] : b.expando; if (s[u]) { if (t && (o = n ? s[u] : s[u].data)) { b.isArray(t) ? t = t.concat(b.map(t, b.camelCase)) : t in o ? t = [t] : (t = b.camelCase(t), t = t in o ? [t] : t.split(" ")); for (r = 0, i = t.length; i > r; r++) delete o[t[r]]; if (!(n ? $ : b.isEmptyObject)(o)) return } (n || (delete s[u].data, $(s[u]))) && (a ? b.cleanData([e], !0) : b.support.deleteExpando || s != s.window ? delete s[u] : s[u] = null) } } } b.extend({ cache: {}, expando: "jQuery" + (p + Math.random()).replace(/\D/g, ""), noData: { embed: !0, object: "clsid:D27CDB6E-AE6D-11cf-96B8-444553540000", applet: !0 }, hasData: function (e) { return e = e.nodeType ? b.cache[e[b.expando]] : e[b.expando], !!e && !$(e) }, data: function (e, t, n) { return P(e, t, n) }, removeData: function (e, t) { return R(e, t) }, _data: function (e, t, n) { return P(e, t, n, !0) }, _removeData: function (e, t) { return R(e, t, !0) }, acceptData: function (e) { if (e.nodeType && 1 !== e.nodeType && 9 !== e.nodeType) return !1; var t = e.nodeName && b.noData[e.nodeName.toLowerCase()]; return !t || t !== !0 && e.getAttribute("classid") === t } }), b.fn.extend({ data: function (e, n) { var r, i, o = this[0], a = 0, s = null; if (e === t) { if (this.length && (s = b.data(o), 1 === o.nodeType && !b._data(o, "parsedAttrs"))) { for (r = o.attributes; r.length > a; a++) i = r[a].name, i.indexOf("data-") || (i = b.camelCase(i.slice(5)), W(o, i, s[i])); b._data(o, "parsedAttrs", !0) } return s } return "object" == typeof e ? this.each(function () { b.data(this, e) }) : b.access(this, function (n) { return n === t ? o ? W(o, e, b.data(o, e)) : null : (this.each(function () { b.data(this, e, n) }), t) }, null, n, arguments.length > 1, null, !0) }, removeData: function (e) { return this.each(function () { b.removeData(this, e) }) } }); function W(e, n, r) { if (r === t && 1 === e.nodeType) { var i = "data-" + n.replace(B, "-$1").toLowerCase(); if (r = e.getAttribute(i), "string" == typeof r) { try { r = "true" === r ? !0 : "false" === r ? !1 : "null" === r ? null : +r + "" === r ? +r : O.test(r) ? b.parseJSON(r) : r } catch (o) { } b.data(e, n, r) } else r = t } return r } function $(e) { var t; for (t in e) if (("data" !== t || !b.isEmptyObject(e[t])) && "toJSON" !== t) return !1; return !0 } b.extend({ queue: function (e, n, r) { var i; return e ? (n = (n || "fx") + "queue", i = b._data(e, n), r && (!i || b.isArray(r) ? i = b._data(e, n, b.makeArray(r)) : i.push(r)), i || []) : t }, dequeue: function (e, t) { t = t || "fx"; var n = b.queue(e, t), r = n.length, i = n.shift(), o = b._queueHooks(e, t), a = function () { b.dequeue(e, t) }; "inprogress" === i && (i = n.shift(), r--), o.cur = i, i && ("fx" === t && n.unshift("inprogress"), delete o.stop, i.call(e, a, o)), !r && o && o.empty.fire() }, _queueHooks: function (e, t) { var n = t + "queueHooks"; return b._data(e, n) || b._data(e, n, { empty: b.Callbacks("once memory").add(function () { b._removeData(e, t + "queue"), b._removeData(e, n) }) }) } }), b.fn.extend({ queue: function (e, n) { var r = 2; return "string" != typeof e && (n = e, e = "fx", r--), r > arguments.length ? b.queue(this[0], e) : n === t ? this : this.each(function () { var t = b.queue(this, e, n); b._queueHooks(this, e), "fx" === e && "inprogress" !== t[0] && b.dequeue(this, e) }) }, dequeue: function (e) { return this.each(function () { b.dequeue(this, e) }) }, delay: function (e, t) { return e = b.fx ? b.fx.speeds[e] || e : e, t = t || "fx", this.queue(t, function (t, n) { var r = setTimeout(t, e); n.stop = function () { clearTimeout(r) } }) }, clearQueue: function (e) { return this.queue(e || "fx", []) }, promise: function (e, n) { var r, i = 1, o = b.Deferred(), a = this, s = this.length, u = function () { --i || o.resolveWith(a, [a]) }; "string" != typeof e && (n = e, e = t), e = e || "fx"; while (s--) r = b._data(a[s], e + "queueHooks"), r && r.empty && (i++, r.empty.add(u)); return u(), o.promise(n) } }); var I, z, X = /[\t\r\n]/g, U = /\r/g, V = /^(?:input|select|textarea|button|object)$/i, Y = /^(?:a|area)$/i, J = /^(?:checked|selected|autofocus|autoplay|async|controls|defer|disabled|hidden|loop|multiple|open|readonly|required|scoped)$/i, G = /^(?:checked|selected)$/i, Q = b.support.getSetAttribute, K = b.support.input; b.fn.extend({ attr: function (e, t) { return b.access(this, b.attr, e, t, arguments.length > 1) }, removeAttr: function (e) { return this.each(function () { b.removeAttr(this, e) }) }, prop: function (e, t) { return b.access(this, b.prop, e, t, arguments.length > 1) }, removeProp: function (e) { return e = b.propFix[e] || e, this.each(function () { try { this[e] = t, delete this[e] } catch (n) { } }) }, addClass: function (e) { var t, n, r, i, o, a = 0, s = this.length, u = "string" == typeof e && e; if (b.isFunction(e)) return this.each(function (t) { b(this).addClass(e.call(this, t, this.className)) }); if (u) for (t = (e || "").match(w) || []; s > a; a++) if (n = this[a], r = 1 === n.nodeType && (n.className ? (" " + n.className + " ").replace(X, " ") : " ")) { o = 0; while (i = t[o++]) 0 > r.indexOf(" " + i + " ") && (r += i + " "); n.className = b.trim(r) } return this }, removeClass: function (e) { var t, n, r, i, o, a = 0, s = this.length, u = 0 === arguments.length || "string" == typeof e && e; if (b.isFunction(e)) return this.each(function (t) { b(this).removeClass(e.call(this, t, this.className)) }); if (u) for (t = (e || "").match(w) || []; s > a; a++) if (n = this[a], r = 1 === n.nodeType && (n.className ? (" " + n.className + " ").replace(X, " ") : "")) { o = 0; while (i = t[o++]) while (r.indexOf(" " + i + " ") >= 0) r = r.replace(" " + i + " ", " "); n.className = e ? b.trim(r) : "" } return this }, toggleClass: function (e, t) { var n = typeof e, r = "boolean" == typeof t; return b.isFunction(e) ? this.each(function (n) { b(this).toggleClass(e.call(this, n, this.className, t), t) }) : this.each(function () { if ("string" === n) { var o, a = 0, s = b(this), u = t, l = e.match(w) || []; while (o = l[a++]) u = r ? u : !s.hasClass(o), s[u ? "addClass" : "removeClass"](o) } else (n === i || "boolean" === n) && (this.className && b._data(this, "__className__", this.className), this.className = this.className || e === !1 ? "" : b._data(this, "__className__") || "") }) }, hasClass: function (e) { var t = " " + e + " ", n = 0, r = this.length; for (; r > n; n++) if (1 === this[n].nodeType && (" " + this[n].className + " ").replace(X, " ").indexOf(t) >= 0) return !0; return !1 }, val: function (e) { var n, r, i, o = this[0]; { if (arguments.length) return i = b.isFunction(e), this.each(function (n) { var o, a = b(this); 1 === this.nodeType && (o = i ? e.call(this, n, a.val()) : e, null == o ? o = "" : "number" == typeof o ? o += "" : b.isArray(o) && (o = b.map(o, function (e) { return null == e ? "" : e + "" })), r = b.valHooks[this.type] || b.valHooks[this.nodeName.toLowerCase()], r && "set" in r && r.set(this, o, "value") !== t || (this.value = o)) }); if (o) return r = b.valHooks[o.type] || b.valHooks[o.nodeName.toLowerCase()], r && "get" in r && (n = r.get(o, "value")) !== t ? n : (n = o.value, "string" == typeof n ? n.replace(U, "") : null == n ? "" : n) } } }), b.extend({ valHooks: { option: { get: function (e) { var t = e.attributes.value; return !t || t.specified ? e.value : e.text } }, select: { get: function (e) { var t, n, r = e.options, i = e.selectedIndex, o = "select-one" === e.type || 0 > i, a = o ? null : [], s = o ? i + 1 : r.length, u = 0 > i ? s : o ? i : 0; for (; s > u; u++) if (n = r[u], !(!n.selected && u !== i || (b.support.optDisabled ? n.disabled : null !== n.getAttribute("disabled")) || n.parentNode.disabled && b.nodeName(n.parentNode, "optgroup"))) { if (t = b(n).val(), o) return t; a.push(t) } return a }, set: function (e, t) { var n = b.makeArray(t); return b(e).find("option").each(function () { this.selected = b.inArray(b(this).val(), n) >= 0 }), n.length || (e.selectedIndex = -1), n } } }, attr: function (e, n, r) { var o, a, s, u = e.nodeType; if (e && 3 !== u && 8 !== u && 2 !== u) return typeof e.getAttribute === i ? b.prop(e, n, r) : (a = 1 !== u || !b.isXMLDoc(e), a && (n = n.toLowerCase(), o = b.attrHooks[n] || (J.test(n) ? z : I)), r === t ? o && a && "get" in o && null !== (s = o.get(e, n)) ? s : (typeof e.getAttribute !== i && (s = e.getAttribute(n)), null == s ? t : s) : null !== r ? o && a && "set" in o && (s = o.set(e, r, n)) !== t ? s : (e.setAttribute(n, r + ""), r) : (b.removeAttr(e, n), t)) }, removeAttr: function (e, t) { var n, r, i = 0, o = t && t.match(w); if (o && 1 === e.nodeType) while (n = o[i++]) r = b.propFix[n] || n, J.test(n) ? !Q && G.test(n) ? e[b.camelCase("default-" + n)] = e[r] = !1 : e[r] = !1 : b.attr(e, n, ""), e.removeAttribute(Q ? n : r) }, attrHooks: { type: { set: function (e, t) { if (!b.support.radioValue && "radio" === t && b.nodeName(e, "input")) { var n = e.value; return e.setAttribute("type", t), n && (e.value = n), t } } } }, propFix: { tabindex: "tabIndex", readonly: "readOnly", "for": "htmlFor", "class": "className", maxlength: "maxLength", cellspacing: "cellSpacing", cellpadding: "cellPadding", rowspan: "rowSpan", colspan: "colSpan", usemap: "useMap", frameborder: "frameBorder", contenteditable: "contentEditable" }, prop: function (e, n, r) { var i, o, a, s = e.nodeType; if (e && 3 !== s && 8 !== s && 2 !== s) return a = 1 !== s || !b.isXMLDoc(e), a && (n = b.propFix[n] || n, o = b.propHooks[n]), r !== t ? o && "set" in o && (i = o.set(e, r, n)) !== t ? i : e[n] = r : o && "get" in o && null !== (i = o.get(e, n)) ? i : e[n] }, propHooks: { tabIndex: { get: function (e) { var n = e.getAttributeNode("tabindex"); return n && n.specified ? parseInt(n.value, 10) : V.test(e.nodeName) || Y.test(e.nodeName) && e.href ? 0 : t } } } }), z = { get: function (e, n) { var r = b.prop(e, n), i = "boolean" == typeof r && e.getAttribute(n), o = "boolean" == typeof r ? K && Q ? null != i : G.test(n) ? e[b.camelCase("default-" + n)] : !!i : e.getAttributeNode(n); return o && o.value !== !1 ? n.toLowerCase() : t }, set: function (e, t, n) { return t === !1 ? b.removeAttr(e, n) : K && Q || !G.test(n) ? e.setAttribute(!Q && b.propFix[n] || n, n) : e[b.camelCase("default-" + n)] = e[n] = !0, n } }, K && Q || (b.attrHooks.value = { get: function (e, n) { var r = e.getAttributeNode(n); return b.nodeName(e, "input") ? e.defaultValue : r && r.specified ? r.value : t }, set: function (e, n, r) { return b.nodeName(e, "input") ? (e.defaultValue = n, t) : I && I.set(e, n, r) } }), Q || (I = b.valHooks.button = { get: function (e, n) { var r = e.getAttributeNode(n); return r && ("id" === n || "name" === n || "coords" === n ? "" !== r.value : r.specified) ? r.value : t }, set: function (e, n, r) { var i = e.getAttributeNode(r); return i || e.setAttributeNode(i = e.ownerDocument.createAttribute(r)), i.value = n += "", "value" === r || n === e.getAttribute(r) ? n : t } }, b.attrHooks.contenteditable = { get: I.get, set: function (e, t, n) { I.set(e, "" === t ? !1 : t, n) } }, b.each(["width", "height"], function (e, n) { b.attrHooks[n] = b.extend(b.attrHooks[n], { set: function (e, r) { return "" === r ? (e.setAttribute(n, "auto"), r) : t } }) })), b.support.hrefNormalized || (b.each(["href", "src", "width", "height"], function (e, n) { b.attrHooks[n] = b.extend(b.attrHooks[n], { get: function (e) { var r = e.getAttribute(n, 2); return null == r ? t : r } }) }), b.each(["href", "src"], function (e, t) { b.propHooks[t] = { get: function (e) { return e.getAttribute(t, 4) } } })), b.support.style || (b.attrHooks.style = { get: function (e) { return e.style.cssText || t }, set: function (e, t) { return e.style.cssText = t + "" } }), b.support.optSelected || (b.propHooks.selected = b.extend(b.propHooks.selected, { get: function (e) { var t = e.parentNode; return t && (t.selectedIndex, t.parentNode && t.parentNode.selectedIndex), null } })), b.support.enctype || (b.propFix.enctype = "encoding"), b.support.checkOn || b.each(["radio", "checkbox"], function () { b.valHooks[this] = { get: function (e) { return null === e.getAttribute("value") ? "on" : e.value } } }), b.each(["radio", "checkbox"], function () { b.valHooks[this] = b.extend(b.valHooks[this], { set: function (e, n) { return b.isArray(n) ? e.checked = b.inArray(b(e).val(), n) >= 0 : t } }) }); var Z = /^(?:input|select|textarea)$/i, et = /^key/, tt = /^(?:mouse|contextmenu)|click/, nt = /^(?:focusinfocus|focusoutblur)$/, rt = /^([^.]*)(?:\.(.+)|)$/; function it() { return !0 } function ot() { return !1 } b.event = { global: {}, add: function (e, n, r, o, a) { var s, u, l, c, p, f, d, h, g, m, y, v = b._data(e); if (v) { r.handler && (c = r, r = c.handler, a = c.selector), r.guid || (r.guid = b.guid++), (u = v.events) || (u = v.events = {}), (f = v.handle) || (f = v.handle = function (e) { return typeof b === i || e && b.event.triggered === e.type ? t : b.event.dispatch.apply(f.elem, arguments) }, f.elem = e), n = (n || "").match(w) || [""], l = n.length; while (l--) s = rt.exec(n[l]) || [], g = y = s[1], m = (s[2] || "").split(".").sort(), p = b.event.special[g] || {}, g = (a ? p.delegateType : p.bindType) || g, p = b.event.special[g] || {}, d = b.extend({ type: g, origType: y, data: o, handler: r, guid: r.guid, selector: a, needsContext: a && b.expr.match.needsContext.test(a), namespace: m.join(".") }, c), (h = u[g]) || (h = u[g] = [], h.delegateCount = 0, p.setup && p.setup.call(e, o, m, f) !== !1 || (e.addEventListener ? e.addEventListener(g, f, !1) : e.attachEvent && e.attachEvent("on" + g, f))), p.add && (p.add.call(e, d), d.handler.guid || (d.handler.guid = r.guid)), a ? h.splice(h.delegateCount++, 0, d) : h.push(d), b.event.global[g] = !0; e = null } }, remove: function (e, t, n, r, i) { var o, a, s, u, l, c, p, f, d, h, g, m = b.hasData(e) && b._data(e); if (m && (c = m.events)) { t = (t || "").match(w) || [""], l = t.length; while (l--) if (s = rt.exec(t[l]) || [], d = g = s[1], h = (s[2] || "").split(".").sort(), d) { p = b.event.special[d] || {}, d = (r ? p.delegateType : p.bindType) || d, f = c[d] || [], s = s[2] && RegExp("(^|\\.)" + h.join("\\.(?:.*\\.|)") + "(\\.|$)"), u = o = f.length; while (o--) a = f[o], !i && g !== a.origType || n && n.guid !== a.guid || s && !s.test(a.namespace) || r && r !== a.selector && ("**" !== r || !a.selector) || (f.splice(o, 1), a.selector && f.delegateCount--, p.remove && p.remove.call(e, a)); u && !f.length && (p.teardown && p.teardown.call(e, h, m.handle) !== !1 || b.removeEvent(e, d, m.handle), delete c[d]) } else for (d in c) b.event.remove(e, d + t[l], n, r, !0); b.isEmptyObject(c) && (delete m.handle, b._removeData(e, "events")) } }, trigger: function (n, r, i, a) { var s, u, l, c, p, f, d, h = [i || o], g = y.call(n, "type") ? n.type : n, m = y.call(n, "namespace") ? n.namespace.split(".") : []; if (l = f = i = i || o, 3 !== i.nodeType && 8 !== i.nodeType && !nt.test(g + b.event.triggered) && (g.indexOf(".") >= 0 && (m = g.split("."), g = m.shift(), m.sort()), u = 0 > g.indexOf(":") && "on" + g, n = n[b.expando] ? n : new b.Event(g, "object" == typeof n && n), n.isTrigger = !0, n.namespace = m.join("."), n.namespace_re = n.namespace ? RegExp("(^|\\.)" + m.join("\\.(?:.*\\.|)") + "(\\.|$)") : null, n.result = t, n.target || (n.target = i), r = null == r ? [n] : b.makeArray(r, [n]), p = b.event.special[g] || {}, a || !p.trigger || p.trigger.apply(i, r) !== !1)) { if (!a && !p.noBubble && !b.isWindow(i)) { for (c = p.delegateType || g, nt.test(c + g) || (l = l.parentNode) ; l; l = l.parentNode) h.push(l), f = l; f === (i.ownerDocument || o) && h.push(f.defaultView || f.parentWindow || e) } d = 0; while ((l = h[d++]) && !n.isPropagationStopped()) n.type = d > 1 ? c : p.bindType || g, s = (b._data(l, "events") || {})[n.type] && b._data(l, "handle"), s && s.apply(l, r), s = u && l[u], s && b.acceptData(l) && s.apply && s.apply(l, r) === !1 && n.preventDefault(); if (n.type = g, !(a || n.isDefaultPrevented() || p._default && p._default.apply(i.ownerDocument, r) !== !1 || "click" === g && b.nodeName(i, "a") || !b.acceptData(i) || !u || !i[g] || b.isWindow(i))) { f = i[u], f && (i[u] = null), b.event.triggered = g; try { i[g]() } catch (v) { } b.event.triggered = t, f && (i[u] = f) } return n.result } }, dispatch: function (e) { e = b.event.fix(e); var n, r, i, o, a, s = [], u = h.call(arguments), l = (b._data(this, "events") || {})[e.type] || [], c = b.event.special[e.type] || {}; if (u[0] = e, e.delegateTarget = this, !c.preDispatch || c.preDispatch.call(this, e) !== !1) { s = b.event.handlers.call(this, e, l), n = 0; while ((o = s[n++]) && !e.isPropagationStopped()) { e.currentTarget = o.elem, a = 0; while ((i = o.handlers[a++]) && !e.isImmediatePropagationStopped()) (!e.namespace_re || e.namespace_re.test(i.namespace)) && (e.handleObj = i, e.data = i.data, r = ((b.event.special[i.origType] || {}).handle || i.handler).apply(o.elem, u), r !== t && (e.result = r) === !1 && (e.preventDefault(), e.stopPropagation())) } return c.postDispatch && c.postDispatch.call(this, e), e.result } }, handlers: function (e, n) { var r, i, o, a, s = [], u = n.delegateCount, l = e.target; if (u && l.nodeType && (!e.button || "click" !== e.type)) for (; l != this; l = l.parentNode || this) if (1 === l.nodeType && (l.disabled !== !0 || "click" !== e.type)) { for (o = [], a = 0; u > a; a++) i = n[a], r = i.selector + " ", o[r] === t && (o[r] = i.needsContext ? b(r, this).index(l) >= 0 : b.find(r, this, null, [l]).length), o[r] && o.push(i); o.length && s.push({ elem: l, handlers: o }) } return n.length > u && s.push({ elem: this, handlers: n.slice(u) }), s }, fix: function (e) { if (e[b.expando]) return e; var t, n, r, i = e.type, a = e, s = this.fixHooks[i]; s || (this.fixHooks[i] = s = tt.test(i) ? this.mouseHooks : et.test(i) ? this.keyHooks : {}), r = s.props ? this.props.concat(s.props) : this.props, e = new b.Event(a), t = r.length; while (t--) n = r[t], e[n] = a[n]; return e.target || (e.target = a.srcElement || o), 3 === e.target.nodeType && (e.target = e.target.parentNode), e.metaKey = !!e.metaKey, s.filter ? s.filter(e, a) : e }, props: "altKey bubbles cancelable ctrlKey currentTarget eventPhase metaKey relatedTarget shiftKey target timeStamp view which".split(" "), fixHooks: {}, keyHooks: { props: "char charCode key keyCode".split(" "), filter: function (e, t) { return null == e.which && (e.which = null != t.charCode ? t.charCode : t.keyCode), e } }, mouseHooks: { props: "button buttons clientX clientY fromElement offsetX offsetY pageX pageY screenX screenY toElement".split(" "), filter: function (e, n) { var r, i, a, s = n.button, u = n.fromElement; return null == e.pageX && null != n.clientX && (i = e.target.ownerDocument || o, a = i.documentElement, r = i.body, e.pageX = n.clientX + (a && a.scrollLeft || r && r.scrollLeft || 0) - (a && a.clientLeft || r && r.clientLeft || 0), e.pageY = n.clientY + (a && a.scrollTop || r && r.scrollTop || 0) - (a && a.clientTop || r && r.clientTop || 0)), !e.relatedTarget && u && (e.relatedTarget = u === e.target ? n.toElement : u), e.which || s === t || (e.which = 1 & s ? 1 : 2 & s ? 3 : 4 & s ? 2 : 0), e } }, special: { load: { noBubble: !0 }, click: { trigger: function () { return b.nodeName(this, "input") && "checkbox" === this.type && this.click ? (this.click(), !1) : t } }, focus: { trigger: function () { if (this !== o.activeElement && this.focus) try { return this.focus(), !1 } catch (e) { } }, delegateType: "focusin" }, blur: { trigger: function () { return this === o.activeElement && this.blur ? (this.blur(), !1) : t }, delegateType: "focusout" }, beforeunload: { postDispatch: function (e) { e.result !== t && (e.originalEvent.returnValue = e.result) } } }, simulate: function (e, t, n, r) { var i = b.extend(new b.Event, n, { type: e, isSimulated: !0, originalEvent: {} }); r ? b.event.trigger(i, null, t) : b.event.dispatch.call(t, i), i.isDefaultPrevented() && n.preventDefault() } }, b.removeEvent = o.removeEventListener ? function (e, t, n) { e.removeEventListener && e.removeEventListener(t, n, !1) } : function (e, t, n) { var r = "on" + t; e.detachEvent && (typeof e[r] === i && (e[r] = null), e.detachEvent(r, n)) }, b.Event = function (e, n) { return this instanceof b.Event ? (e && e.type ? (this.originalEvent = e, this.type = e.type, this.isDefaultPrevented = e.defaultPrevented || e.returnValue === !1 || e.getPreventDefault && e.getPreventDefault() ? it : ot) : this.type = e, n && b.extend(this, n), this.timeStamp = e && e.timeStamp || b.now(), this[b.expando] = !0, t) : new b.Event(e, n) }, b.Event.prototype = { isDefaultPrevented: ot, isPropagationStopped: ot, isImmediatePropagationStopped: ot, preventDefault: function () { var e = this.originalEvent; this.isDefaultPrevented = it, e && (e.preventDefault ? e.preventDefault() : e.returnValue = !1) }, stopPropagation: function () { var e = this.originalEvent; this.isPropagationStopped = it, e && (e.stopPropagation && e.stopPropagation(), e.cancelBubble = !0) }, stopImmediatePropagation: function () { this.isImmediatePropagationStopped = it, this.stopPropagation() } }, b.each({ mouseenter: "mouseover", mouseleave: "mouseout" }, function (e, t) {
        b.event.special[e] = {
            delegateType: t, bindType: t, handle: function (e) {
                var n, r = this, i = e.relatedTarget, o = e.handleObj;
                return (!i || i !== r && !b.contains(r, i)) && (e.type = o.origType, n = o.handler.apply(this, arguments), e.type = t), n
            }
        }
    }), b.support.submitBubbles || (b.event.special.submit = { setup: function () { return b.nodeName(this, "form") ? !1 : (b.event.add(this, "click._submit keypress._submit", function (e) { var n = e.target, r = b.nodeName(n, "input") || b.nodeName(n, "button") ? n.form : t; r && !b._data(r, "submitBubbles") && (b.event.add(r, "submit._submit", function (e) { e._submit_bubble = !0 }), b._data(r, "submitBubbles", !0)) }), t) }, postDispatch: function (e) { e._submit_bubble && (delete e._submit_bubble, this.parentNode && !e.isTrigger && b.event.simulate("submit", this.parentNode, e, !0)) }, teardown: function () { return b.nodeName(this, "form") ? !1 : (b.event.remove(this, "._submit"), t) } }), b.support.changeBubbles || (b.event.special.change = { setup: function () { return Z.test(this.nodeName) ? (("checkbox" === this.type || "radio" === this.type) && (b.event.add(this, "propertychange._change", function (e) { "checked" === e.originalEvent.propertyName && (this._just_changed = !0) }), b.event.add(this, "click._change", function (e) { this._just_changed && !e.isTrigger && (this._just_changed = !1), b.event.simulate("change", this, e, !0) })), !1) : (b.event.add(this, "beforeactivate._change", function (e) { var t = e.target; Z.test(t.nodeName) && !b._data(t, "changeBubbles") && (b.event.add(t, "change._change", function (e) { !this.parentNode || e.isSimulated || e.isTrigger || b.event.simulate("change", this.parentNode, e, !0) }), b._data(t, "changeBubbles", !0)) }), t) }, handle: function (e) { var n = e.target; return this !== n || e.isSimulated || e.isTrigger || "radio" !== n.type && "checkbox" !== n.type ? e.handleObj.handler.apply(this, arguments) : t }, teardown: function () { return b.event.remove(this, "._change"), !Z.test(this.nodeName) } }), b.support.focusinBubbles || b.each({ focus: "focusin", blur: "focusout" }, function (e, t) { var n = 0, r = function (e) { b.event.simulate(t, e.target, b.event.fix(e), !0) }; b.event.special[t] = { setup: function () { 0 === n++ && o.addEventListener(e, r, !0) }, teardown: function () { 0 === --n && o.removeEventListener(e, r, !0) } } }), b.fn.extend({ on: function (e, n, r, i, o) { var a, s; if ("object" == typeof e) { "string" != typeof n && (r = r || n, n = t); for (a in e) this.on(a, n, r, e[a], o); return this } if (null == r && null == i ? (i = n, r = n = t) : null == i && ("string" == typeof n ? (i = r, r = t) : (i = r, r = n, n = t)), i === !1) i = ot; else if (!i) return this; return 1 === o && (s = i, i = function (e) { return b().off(e), s.apply(this, arguments) }, i.guid = s.guid || (s.guid = b.guid++)), this.each(function () { b.event.add(this, e, i, r, n) }) }, one: function (e, t, n, r) { return this.on(e, t, n, r, 1) }, off: function (e, n, r) { var i, o; if (e && e.preventDefault && e.handleObj) return i = e.handleObj, b(e.delegateTarget).off(i.namespace ? i.origType + "." + i.namespace : i.origType, i.selector, i.handler), this; if ("object" == typeof e) { for (o in e) this.off(o, n, e[o]); return this } return (n === !1 || "function" == typeof n) && (r = n, n = t), r === !1 && (r = ot), this.each(function () { b.event.remove(this, e, r, n) }) }, bind: function (e, t, n) { return this.on(e, null, t, n) }, unbind: function (e, t) { return this.off(e, null, t) }, delegate: function (e, t, n, r) { return this.on(t, e, n, r) }, undelegate: function (e, t, n) { return 1 === arguments.length ? this.off(e, "**") : this.off(t, e || "**", n) }, trigger: function (e, t) { return this.each(function () { b.event.trigger(e, t, this) }) }, triggerHandler: function (e, n) { var r = this[0]; return r ? b.event.trigger(e, n, r, !0) : t } }), function (e, t) { var n, r, i, o, a, s, u, l, c, p, f, d, h, g, m, y, v, x = "sizzle" + -new Date, w = e.document, T = {}, N = 0, C = 0, k = it(), E = it(), S = it(), A = typeof t, j = 1 << 31, D = [], L = D.pop, H = D.push, q = D.slice, M = D.indexOf || function (e) { var t = 0, n = this.length; for (; n > t; t++) if (this[t] === e) return t; return -1 }, _ = "[\\x20\\t\\r\\n\\f]", F = "(?:\\\\.|[\\w-]|[^\\x00-\\xa0])+", O = F.replace("w", "w#"), B = "([*^$|!~]?=)", P = "\\[" + _ + "*(" + F + ")" + _ + "*(?:" + B + _ + "*(?:(['\"])((?:\\\\.|[^\\\\])*?)\\3|(" + O + ")|)|)" + _ + "*\\]", R = ":(" + F + ")(?:\\(((['\"])((?:\\\\.|[^\\\\])*?)\\3|((?:\\\\.|[^\\\\()[\\]]|" + P.replace(3, 8) + ")*)|.*)\\)|)", W = RegExp("^" + _ + "+|((?:^|[^\\\\])(?:\\\\.)*)" + _ + "+$", "g"), $ = RegExp("^" + _ + "*," + _ + "*"), I = RegExp("^" + _ + "*([\\x20\\t\\r\\n\\f>+~])" + _ + "*"), z = RegExp(R), X = RegExp("^" + O + "$"), U = { ID: RegExp("^#(" + F + ")"), CLASS: RegExp("^\\.(" + F + ")"), NAME: RegExp("^\\[name=['\"]?(" + F + ")['\"]?\\]"), TAG: RegExp("^(" + F.replace("w", "w*") + ")"), ATTR: RegExp("^" + P), PSEUDO: RegExp("^" + R), CHILD: RegExp("^:(only|first|last|nth|nth-last)-(child|of-type)(?:\\(" + _ + "*(even|odd|(([+-]|)(\\d*)n|)" + _ + "*(?:([+-]|)" + _ + "*(\\d+)|))" + _ + "*\\)|)", "i"), needsContext: RegExp("^" + _ + "*[>+~]|:(even|odd|eq|gt|lt|nth|first|last)(?:\\(" + _ + "*((?:-\\d)?\\d*)" + _ + "*\\)|)(?=[^-]|$)", "i") }, V = /[\x20\t\r\n\f]*[+~]/, Y = /^[^{]+\{\s*\[native code/, J = /^(?:#([\w-]+)|(\w+)|\.([\w-]+))$/, G = /^(?:input|select|textarea|button)$/i, Q = /^h\d$/i, K = /'|\\/g, Z = /\=[\x20\t\r\n\f]*([^'"\]]*)[\x20\t\r\n\f]*\]/g, et = /\\([\da-fA-F]{1,6}[\x20\t\r\n\f]?|.)/g, tt = function (e, t) { var n = "0x" + t - 65536; return n !== n ? t : 0 > n ? String.fromCharCode(n + 65536) : String.fromCharCode(55296 | n >> 10, 56320 | 1023 & n) }; try { q.call(w.documentElement.childNodes, 0)[0].nodeType } catch (nt) { q = function (e) { var t, n = []; while (t = this[e++]) n.push(t); return n } } function rt(e) { return Y.test(e + "") } function it() { var e, t = []; return e = function (n, r) { return t.push(n += " ") > i.cacheLength && delete e[t.shift()], e[n] = r } } function ot(e) { return e[x] = !0, e } function at(e) { var t = p.createElement("div"); try { return e(t) } catch (n) { return !1 } finally { t = null } } function st(e, t, n, r) { var i, o, a, s, u, l, f, g, m, v; if ((t ? t.ownerDocument || t : w) !== p && c(t), t = t || p, n = n || [], !e || "string" != typeof e) return n; if (1 !== (s = t.nodeType) && 9 !== s) return []; if (!d && !r) { if (i = J.exec(e)) if (a = i[1]) { if (9 === s) { if (o = t.getElementById(a), !o || !o.parentNode) return n; if (o.id === a) return n.push(o), n } else if (t.ownerDocument && (o = t.ownerDocument.getElementById(a)) && y(t, o) && o.id === a) return n.push(o), n } else { if (i[2]) return H.apply(n, q.call(t.getElementsByTagName(e), 0)), n; if ((a = i[3]) && T.getByClassName && t.getElementsByClassName) return H.apply(n, q.call(t.getElementsByClassName(a), 0)), n } if (T.qsa && !h.test(e)) { if (f = !0, g = x, m = t, v = 9 === s && e, 1 === s && "object" !== t.nodeName.toLowerCase()) { l = ft(e), (f = t.getAttribute("id")) ? g = f.replace(K, "\\$&") : t.setAttribute("id", g), g = "[id='" + g + "'] ", u = l.length; while (u--) l[u] = g + dt(l[u]); m = V.test(e) && t.parentNode || t, v = l.join(",") } if (v) try { return H.apply(n, q.call(m.querySelectorAll(v), 0)), n } catch (b) { } finally { f || t.removeAttribute("id") } } } return wt(e.replace(W, "$1"), t, n, r) } a = st.isXML = function (e) { var t = e && (e.ownerDocument || e).documentElement; return t ? "HTML" !== t.nodeName : !1 }, c = st.setDocument = function (e) { var n = e ? e.ownerDocument || e : w; return n !== p && 9 === n.nodeType && n.documentElement ? (p = n, f = n.documentElement, d = a(n), T.tagNameNoComments = at(function (e) { return e.appendChild(n.createComment("")), !e.getElementsByTagName("*").length }), T.attributes = at(function (e) { e.innerHTML = "<select></select>"; var t = typeof e.lastChild.getAttribute("multiple"); return "boolean" !== t && "string" !== t }), T.getByClassName = at(function (e) { return e.innerHTML = "<div class='hidden e'></div><div class='hidden'></div>", e.getElementsByClassName && e.getElementsByClassName("e").length ? (e.lastChild.className = "e", 2 === e.getElementsByClassName("e").length) : !1 }), T.getByName = at(function (e) { e.id = x + 0, e.innerHTML = "<a name='" + x + "'></a><div name='" + x + "'></div>", f.insertBefore(e, f.firstChild); var t = n.getElementsByName && n.getElementsByName(x).length === 2 + n.getElementsByName(x + 0).length; return T.getIdNotName = !n.getElementById(x), f.removeChild(e), t }), i.attrHandle = at(function (e) { return e.innerHTML = "<a href='#'></a>", e.firstChild && typeof e.firstChild.getAttribute !== A && "#" === e.firstChild.getAttribute("href") }) ? {} : { href: function (e) { return e.getAttribute("href", 2) }, type: function (e) { return e.getAttribute("type") } }, T.getIdNotName ? (i.find.ID = function (e, t) { if (typeof t.getElementById !== A && !d) { var n = t.getElementById(e); return n && n.parentNode ? [n] : [] } }, i.filter.ID = function (e) { var t = e.replace(et, tt); return function (e) { return e.getAttribute("id") === t } }) : (i.find.ID = function (e, n) { if (typeof n.getElementById !== A && !d) { var r = n.getElementById(e); return r ? r.id === e || typeof r.getAttributeNode !== A && r.getAttributeNode("id").value === e ? [r] : t : [] } }, i.filter.ID = function (e) { var t = e.replace(et, tt); return function (e) { var n = typeof e.getAttributeNode !== A && e.getAttributeNode("id"); return n && n.value === t } }), i.find.TAG = T.tagNameNoComments ? function (e, n) { return typeof n.getElementsByTagName !== A ? n.getElementsByTagName(e) : t } : function (e, t) { var n, r = [], i = 0, o = t.getElementsByTagName(e); if ("*" === e) { while (n = o[i++]) 1 === n.nodeType && r.push(n); return r } return o }, i.find.NAME = T.getByName && function (e, n) { return typeof n.getElementsByName !== A ? n.getElementsByName(name) : t }, i.find.CLASS = T.getByClassName && function (e, n) { return typeof n.getElementsByClassName === A || d ? t : n.getElementsByClassName(e) }, g = [], h = [":focus"], (T.qsa = rt(n.querySelectorAll)) && (at(function (e) { e.innerHTML = "<select><option selected=''></option></select>", e.querySelectorAll("[selected]").length || h.push("\\[" + _ + "*(?:checked|disabled|ismap|multiple|readonly|selected|value)"), e.querySelectorAll(":checked").length || h.push(":checked") }), at(function (e) { e.innerHTML = "<input type='hidden' i=''/>", e.querySelectorAll("[i^='']").length && h.push("[*^$]=" + _ + "*(?:\"\"|'')"), e.querySelectorAll(":enabled").length || h.push(":enabled", ":disabled"), e.querySelectorAll("*,:x"), h.push(",.*:") })), (T.matchesSelector = rt(m = f.matchesSelector || f.mozMatchesSelector || f.webkitMatchesSelector || f.oMatchesSelector || f.msMatchesSelector)) && at(function (e) { T.disconnectedMatch = m.call(e, "div"), m.call(e, "[s!='']:x"), g.push("!=", R) }), h = RegExp(h.join("|")), g = RegExp(g.join("|")), y = rt(f.contains) || f.compareDocumentPosition ? function (e, t) { var n = 9 === e.nodeType ? e.documentElement : e, r = t && t.parentNode; return e === r || !(!r || 1 !== r.nodeType || !(n.contains ? n.contains(r) : e.compareDocumentPosition && 16 & e.compareDocumentPosition(r))) } : function (e, t) { if (t) while (t = t.parentNode) if (t === e) return !0; return !1 }, v = f.compareDocumentPosition ? function (e, t) { var r; return e === t ? (u = !0, 0) : (r = t.compareDocumentPosition && e.compareDocumentPosition && e.compareDocumentPosition(t)) ? 1 & r || e.parentNode && 11 === e.parentNode.nodeType ? e === n || y(w, e) ? -1 : t === n || y(w, t) ? 1 : 0 : 4 & r ? -1 : 1 : e.compareDocumentPosition ? -1 : 1 } : function (e, t) { var r, i = 0, o = e.parentNode, a = t.parentNode, s = [e], l = [t]; if (e === t) return u = !0, 0; if (!o || !a) return e === n ? -1 : t === n ? 1 : o ? -1 : a ? 1 : 0; if (o === a) return ut(e, t); r = e; while (r = r.parentNode) s.unshift(r); r = t; while (r = r.parentNode) l.unshift(r); while (s[i] === l[i]) i++; return i ? ut(s[i], l[i]) : s[i] === w ? -1 : l[i] === w ? 1 : 0 }, u = !1, [0, 0].sort(v), T.detectDuplicates = u, p) : p }, st.matches = function (e, t) { return st(e, null, null, t) }, st.matchesSelector = function (e, t) { if ((e.ownerDocument || e) !== p && c(e), t = t.replace(Z, "='$1']"), !(!T.matchesSelector || d || g && g.test(t) || h.test(t))) try { var n = m.call(e, t); if (n || T.disconnectedMatch || e.document && 11 !== e.document.nodeType) return n } catch (r) { } return st(t, p, null, [e]).length > 0 }, st.contains = function (e, t) { return (e.ownerDocument || e) !== p && c(e), y(e, t) }, st.attr = function (e, t) { var n; return (e.ownerDocument || e) !== p && c(e), d || (t = t.toLowerCase()), (n = i.attrHandle[t]) ? n(e) : d || T.attributes ? e.getAttribute(t) : ((n = e.getAttributeNode(t)) || e.getAttribute(t)) && e[t] === !0 ? t : n && n.specified ? n.value : null }, st.error = function (e) { throw Error("Syntax error, unrecognized expression: " + e) }, st.uniqueSort = function (e) { var t, n = [], r = 1, i = 0; if (u = !T.detectDuplicates, e.sort(v), u) { for (; t = e[r]; r++) t === e[r - 1] && (i = n.push(r)); while (i--) e.splice(n[i], 1) } return e }; function ut(e, t) { var n = t && e, r = n && (~t.sourceIndex || j) - (~e.sourceIndex || j); if (r) return r; if (n) while (n = n.nextSibling) if (n === t) return -1; return e ? 1 : -1 } function lt(e) { return function (t) { var n = t.nodeName.toLowerCase(); return "input" === n && t.type === e } } function ct(e) { return function (t) { var n = t.nodeName.toLowerCase(); return ("input" === n || "button" === n) && t.type === e } } function pt(e) { return ot(function (t) { return t = +t, ot(function (n, r) { var i, o = e([], n.length, t), a = o.length; while (a--) n[i = o[a]] && (n[i] = !(r[i] = n[i])) }) }) } o = st.getText = function (e) { var t, n = "", r = 0, i = e.nodeType; if (i) { if (1 === i || 9 === i || 11 === i) { if ("string" == typeof e.textContent) return e.textContent; for (e = e.firstChild; e; e = e.nextSibling) n += o(e) } else if (3 === i || 4 === i) return e.nodeValue } else for (; t = e[r]; r++) n += o(t); return n }, i = st.selectors = { cacheLength: 50, createPseudo: ot, match: U, find: {}, relative: { ">": { dir: "parentNode", first: !0 }, " ": { dir: "parentNode" }, "+": { dir: "previousSibling", first: !0 }, "~": { dir: "previousSibling" } }, preFilter: { ATTR: function (e) { return e[1] = e[1].replace(et, tt), e[3] = (e[4] || e[5] || "").replace(et, tt), "~=" === e[2] && (e[3] = " " + e[3] + " "), e.slice(0, 4) }, CHILD: function (e) { return e[1] = e[1].toLowerCase(), "nth" === e[1].slice(0, 3) ? (e[3] || st.error(e[0]), e[4] = +(e[4] ? e[5] + (e[6] || 1) : 2 * ("even" === e[3] || "odd" === e[3])), e[5] = +(e[7] + e[8] || "odd" === e[3])) : e[3] && st.error(e[0]), e }, PSEUDO: function (e) { var t, n = !e[5] && e[2]; return U.CHILD.test(e[0]) ? null : (e[4] ? e[2] = e[4] : n && z.test(n) && (t = ft(n, !0)) && (t = n.indexOf(")", n.length - t) - n.length) && (e[0] = e[0].slice(0, t), e[2] = n.slice(0, t)), e.slice(0, 3)) } }, filter: { TAG: function (e) { return "*" === e ? function () { return !0 } : (e = e.replace(et, tt).toLowerCase(), function (t) { return t.nodeName && t.nodeName.toLowerCase() === e }) }, CLASS: function (e) { var t = k[e + " "]; return t || (t = RegExp("(^|" + _ + ")" + e + "(" + _ + "|$)")) && k(e, function (e) { return t.test(e.className || typeof e.getAttribute !== A && e.getAttribute("class") || "") }) }, ATTR: function (e, t, n) { return function (r) { var i = st.attr(r, e); return null == i ? "!=" === t : t ? (i += "", "=" === t ? i === n : "!=" === t ? i !== n : "^=" === t ? n && 0 === i.indexOf(n) : "*=" === t ? n && i.indexOf(n) > -1 : "$=" === t ? n && i.slice(-n.length) === n : "~=" === t ? (" " + i + " ").indexOf(n) > -1 : "|=" === t ? i === n || i.slice(0, n.length + 1) === n + "-" : !1) : !0 } }, CHILD: function (e, t, n, r, i) { var o = "nth" !== e.slice(0, 3), a = "last" !== e.slice(-4), s = "of-type" === t; return 1 === r && 0 === i ? function (e) { return !!e.parentNode } : function (t, n, u) { var l, c, p, f, d, h, g = o !== a ? "nextSibling" : "previousSibling", m = t.parentNode, y = s && t.nodeName.toLowerCase(), v = !u && !s; if (m) { if (o) { while (g) { p = t; while (p = p[g]) if (s ? p.nodeName.toLowerCase() === y : 1 === p.nodeType) return !1; h = g = "only" === e && !h && "nextSibling" } return !0 } if (h = [a ? m.firstChild : m.lastChild], a && v) { c = m[x] || (m[x] = {}), l = c[e] || [], d = l[0] === N && l[1], f = l[0] === N && l[2], p = d && m.childNodes[d]; while (p = ++d && p && p[g] || (f = d = 0) || h.pop()) if (1 === p.nodeType && ++f && p === t) { c[e] = [N, d, f]; break } } else if (v && (l = (t[x] || (t[x] = {}))[e]) && l[0] === N) f = l[1]; else while (p = ++d && p && p[g] || (f = d = 0) || h.pop()) if ((s ? p.nodeName.toLowerCase() === y : 1 === p.nodeType) && ++f && (v && ((p[x] || (p[x] = {}))[e] = [N, f]), p === t)) break; return f -= i, f === r || 0 === f % r && f / r >= 0 } } }, PSEUDO: function (e, t) { var n, r = i.pseudos[e] || i.setFilters[e.toLowerCase()] || st.error("unsupported pseudo: " + e); return r[x] ? r(t) : r.length > 1 ? (n = [e, e, "", t], i.setFilters.hasOwnProperty(e.toLowerCase()) ? ot(function (e, n) { var i, o = r(e, t), a = o.length; while (a--) i = M.call(e, o[a]), e[i] = !(n[i] = o[a]) }) : function (e) { return r(e, 0, n) }) : r } }, pseudos: { not: ot(function (e) { var t = [], n = [], r = s(e.replace(W, "$1")); return r[x] ? ot(function (e, t, n, i) { var o, a = r(e, null, i, []), s = e.length; while (s--) (o = a[s]) && (e[s] = !(t[s] = o)) }) : function (e, i, o) { return t[0] = e, r(t, null, o, n), !n.pop() } }), has: ot(function (e) { return function (t) { return st(e, t).length > 0 } }), contains: ot(function (e) { return function (t) { return (t.textContent || t.innerText || o(t)).indexOf(e) > -1 } }), lang: ot(function (e) { return X.test(e || "") || st.error("unsupported lang: " + e), e = e.replace(et, tt).toLowerCase(), function (t) { var n; do if (n = d ? t.getAttribute("xml:lang") || t.getAttribute("lang") : t.lang) return n = n.toLowerCase(), n === e || 0 === n.indexOf(e + "-"); while ((t = t.parentNode) && 1 === t.nodeType); return !1 } }), target: function (t) { var n = e.location && e.location.hash; return n && n.slice(1) === t.id }, root: function (e) { return e === f }, focus: function (e) { return e === p.activeElement && (!p.hasFocus || p.hasFocus()) && !!(e.type || e.href || ~e.tabIndex) }, enabled: function (e) { return e.disabled === !1 }, disabled: function (e) { return e.disabled === !0 }, checked: function (e) { var t = e.nodeName.toLowerCase(); return "input" === t && !!e.checked || "option" === t && !!e.selected }, selected: function (e) { return e.parentNode && e.parentNode.selectedIndex, e.selected === !0 }, empty: function (e) { for (e = e.firstChild; e; e = e.nextSibling) if (e.nodeName > "@" || 3 === e.nodeType || 4 === e.nodeType) return !1; return !0 }, parent: function (e) { return !i.pseudos.empty(e) }, header: function (e) { return Q.test(e.nodeName) }, input: function (e) { return G.test(e.nodeName) }, button: function (e) { var t = e.nodeName.toLowerCase(); return "input" === t && "button" === e.type || "button" === t }, text: function (e) { var t; return "input" === e.nodeName.toLowerCase() && "text" === e.type && (null == (t = e.getAttribute("type")) || t.toLowerCase() === e.type) }, first: pt(function () { return [0] }), last: pt(function (e, t) { return [t - 1] }), eq: pt(function (e, t, n) { return [0 > n ? n + t : n] }), even: pt(function (e, t) { var n = 0; for (; t > n; n += 2) e.push(n); return e }), odd: pt(function (e, t) { var n = 1; for (; t > n; n += 2) e.push(n); return e }), lt: pt(function (e, t, n) { var r = 0 > n ? n + t : n; for (; --r >= 0;) e.push(r); return e }), gt: pt(function (e, t, n) { var r = 0 > n ? n + t : n; for (; t > ++r;) e.push(r); return e }) } }; for (n in { radio: !0, checkbox: !0, file: !0, password: !0, image: !0 }) i.pseudos[n] = lt(n); for (n in { submit: !0, reset: !0 }) i.pseudos[n] = ct(n); function ft(e, t) { var n, r, o, a, s, u, l, c = E[e + " "]; if (c) return t ? 0 : c.slice(0); s = e, u = [], l = i.preFilter; while (s) { (!n || (r = $.exec(s))) && (r && (s = s.slice(r[0].length) || s), u.push(o = [])), n = !1, (r = I.exec(s)) && (n = r.shift(), o.push({ value: n, type: r[0].replace(W, " ") }), s = s.slice(n.length)); for (a in i.filter) !(r = U[a].exec(s)) || l[a] && !(r = l[a](r)) || (n = r.shift(), o.push({ value: n, type: a, matches: r }), s = s.slice(n.length)); if (!n) break } return t ? s.length : s ? st.error(e) : E(e, u).slice(0) } function dt(e) { var t = 0, n = e.length, r = ""; for (; n > t; t++) r += e[t].value; return r } function ht(e, t, n) { var i = t.dir, o = n && "parentNode" === i, a = C++; return t.first ? function (t, n, r) { while (t = t[i]) if (1 === t.nodeType || o) return e(t, n, r) } : function (t, n, s) { var u, l, c, p = N + " " + a; if (s) { while (t = t[i]) if ((1 === t.nodeType || o) && e(t, n, s)) return !0 } else while (t = t[i]) if (1 === t.nodeType || o) if (c = t[x] || (t[x] = {}), (l = c[i]) && l[0] === p) { if ((u = l[1]) === !0 || u === r) return u === !0 } else if (l = c[i] = [p], l[1] = e(t, n, s) || r, l[1] === !0) return !0 } } function gt(e) { return e.length > 1 ? function (t, n, r) { var i = e.length; while (i--) if (!e[i](t, n, r)) return !1; return !0 } : e[0] } function mt(e, t, n, r, i) { var o, a = [], s = 0, u = e.length, l = null != t; for (; u > s; s++) (o = e[s]) && (!n || n(o, r, i)) && (a.push(o), l && t.push(s)); return a } function yt(e, t, n, r, i, o) { return r && !r[x] && (r = yt(r)), i && !i[x] && (i = yt(i, o)), ot(function (o, a, s, u) { var l, c, p, f = [], d = [], h = a.length, g = o || xt(t || "*", s.nodeType ? [s] : s, []), m = !e || !o && t ? g : mt(g, f, e, s, u), y = n ? i || (o ? e : h || r) ? [] : a : m; if (n && n(m, y, s, u), r) { l = mt(y, d), r(l, [], s, u), c = l.length; while (c--) (p = l[c]) && (y[d[c]] = !(m[d[c]] = p)) } if (o) { if (i || e) { if (i) { l = [], c = y.length; while (c--) (p = y[c]) && l.push(m[c] = p); i(null, y = [], l, u) } c = y.length; while (c--) (p = y[c]) && (l = i ? M.call(o, p) : f[c]) > -1 && (o[l] = !(a[l] = p)) } } else y = mt(y === a ? y.splice(h, y.length) : y), i ? i(null, a, y, u) : H.apply(a, y) }) } function vt(e) { var t, n, r, o = e.length, a = i.relative[e[0].type], s = a || i.relative[" "], u = a ? 1 : 0, c = ht(function (e) { return e === t }, s, !0), p = ht(function (e) { return M.call(t, e) > -1 }, s, !0), f = [function (e, n, r) { return !a && (r || n !== l) || ((t = n).nodeType ? c(e, n, r) : p(e, n, r)) }]; for (; o > u; u++) if (n = i.relative[e[u].type]) f = [ht(gt(f), n)]; else { if (n = i.filter[e[u].type].apply(null, e[u].matches), n[x]) { for (r = ++u; o > r; r++) if (i.relative[e[r].type]) break; return yt(u > 1 && gt(f), u > 1 && dt(e.slice(0, u - 1)).replace(W, "$1"), n, r > u && vt(e.slice(u, r)), o > r && vt(e = e.slice(r)), o > r && dt(e)) } f.push(n) } return gt(f) } function bt(e, t) { var n = 0, o = t.length > 0, a = e.length > 0, s = function (s, u, c, f, d) { var h, g, m, y = [], v = 0, b = "0", x = s && [], w = null != d, T = l, C = s || a && i.find.TAG("*", d && u.parentNode || u), k = N += null == T ? 1 : Math.random() || .1; for (w && (l = u !== p && u, r = n) ; null != (h = C[b]) ; b++) { if (a && h) { g = 0; while (m = e[g++]) if (m(h, u, c)) { f.push(h); break } w && (N = k, r = ++n) } o && ((h = !m && h) && v--, s && x.push(h)) } if (v += b, o && b !== v) { g = 0; while (m = t[g++]) m(x, y, u, c); if (s) { if (v > 0) while (b--) x[b] || y[b] || (y[b] = L.call(f)); y = mt(y) } H.apply(f, y), w && !s && y.length > 0 && v + t.length > 1 && st.uniqueSort(f) } return w && (N = k, l = T), x }; return o ? ot(s) : s } s = st.compile = function (e, t) { var n, r = [], i = [], o = S[e + " "]; if (!o) { t || (t = ft(e)), n = t.length; while (n--) o = vt(t[n]), o[x] ? r.push(o) : i.push(o); o = S(e, bt(i, r)) } return o }; function xt(e, t, n) { var r = 0, i = t.length; for (; i > r; r++) st(e, t[r], n); return n } function wt(e, t, n, r) { var o, a, u, l, c, p = ft(e); if (!r && 1 === p.length) { if (a = p[0] = p[0].slice(0), a.length > 2 && "ID" === (u = a[0]).type && 9 === t.nodeType && !d && i.relative[a[1].type]) { if (t = i.find.ID(u.matches[0].replace(et, tt), t)[0], !t) return n; e = e.slice(a.shift().value.length) } o = U.needsContext.test(e) ? 0 : a.length; while (o--) { if (u = a[o], i.relative[l = u.type]) break; if ((c = i.find[l]) && (r = c(u.matches[0].replace(et, tt), V.test(a[0].type) && t.parentNode || t))) { if (a.splice(o, 1), e = r.length && dt(a), !e) return H.apply(n, q.call(r, 0)), n; break } } } return s(e, p)(r, t, d, n, V.test(e)), n } i.pseudos.nth = i.pseudos.eq; function Tt() { } i.filters = Tt.prototype = i.pseudos, i.setFilters = new Tt, c(), st.attr = b.attr, b.find = st, b.expr = st.selectors, b.expr[":"] = b.expr.pseudos, b.unique = st.uniqueSort, b.text = st.getText, b.isXMLDoc = st.isXML, b.contains = st.contains }(e); var at = /Until$/, st = /^(?:parents|prev(?:Until|All))/, ut = /^.[^:#\[\.,]*$/, lt = b.expr.match.needsContext, ct = { children: !0, contents: !0, next: !0, prev: !0 }; b.fn.extend({ find: function (e) { var t, n, r, i = this.length; if ("string" != typeof e) return r = this, this.pushStack(b(e).filter(function () { for (t = 0; i > t; t++) if (b.contains(r[t], this)) return !0 })); for (n = [], t = 0; i > t; t++) b.find(e, this[t], n); return n = this.pushStack(i > 1 ? b.unique(n) : n), n.selector = (this.selector ? this.selector + " " : "") + e, n }, has: function (e) { var t, n = b(e, this), r = n.length; return this.filter(function () { for (t = 0; r > t; t++) if (b.contains(this, n[t])) return !0 }) }, not: function (e) { return this.pushStack(ft(this, e, !1)) }, filter: function (e) { return this.pushStack(ft(this, e, !0)) }, is: function (e) { return !!e && ("string" == typeof e ? lt.test(e) ? b(e, this.context).index(this[0]) >= 0 : b.filter(e, this).length > 0 : this.filter(e).length > 0) }, closest: function (e, t) { var n, r = 0, i = this.length, o = [], a = lt.test(e) || "string" != typeof e ? b(e, t || this.context) : 0; for (; i > r; r++) { n = this[r]; while (n && n.ownerDocument && n !== t && 11 !== n.nodeType) { if (a ? a.index(n) > -1 : b.find.matchesSelector(n, e)) { o.push(n); break } n = n.parentNode } } return this.pushStack(o.length > 1 ? b.unique(o) : o) }, index: function (e) { return e ? "string" == typeof e ? b.inArray(this[0], b(e)) : b.inArray(e.jquery ? e[0] : e, this) : this[0] && this[0].parentNode ? this.first().prevAll().length : -1 }, add: function (e, t) { var n = "string" == typeof e ? b(e, t) : b.makeArray(e && e.nodeType ? [e] : e), r = b.merge(this.get(), n); return this.pushStack(b.unique(r)) }, addBack: function (e) { return this.add(null == e ? this.prevObject : this.prevObject.filter(e)) } }), b.fn.andSelf = b.fn.addBack; function pt(e, t) { do e = e[t]; while (e && 1 !== e.nodeType); return e } b.each({ parent: function (e) { var t = e.parentNode; return t && 11 !== t.nodeType ? t : null }, parents: function (e) { return b.dir(e, "parentNode") }, parentsUntil: function (e, t, n) { return b.dir(e, "parentNode", n) }, next: function (e) { return pt(e, "nextSibling") }, prev: function (e) { return pt(e, "previousSibling") }, nextAll: function (e) { return b.dir(e, "nextSibling") }, prevAll: function (e) { return b.dir(e, "previousSibling") }, nextUntil: function (e, t, n) { return b.dir(e, "nextSibling", n) }, prevUntil: function (e, t, n) { return b.dir(e, "previousSibling", n) }, siblings: function (e) { return b.sibling((e.parentNode || {}).firstChild, e) }, children: function (e) { return b.sibling(e.firstChild) }, contents: function (e) { return b.nodeName(e, "iframe") ? e.contentDocument || e.contentWindow.document : b.merge([], e.childNodes) } }, function (e, t) { b.fn[e] = function (n, r) { var i = b.map(this, t, n); return at.test(e) || (r = n), r && "string" == typeof r && (i = b.filter(r, i)), i = this.length > 1 && !ct[e] ? b.unique(i) : i, this.length > 1 && st.test(e) && (i = i.reverse()), this.pushStack(i) } }), b.extend({ filter: function (e, t, n) { return n && (e = ":not(" + e + ")"), 1 === t.length ? b.find.matchesSelector(t[0], e) ? [t[0]] : [] : b.find.matches(e, t) }, dir: function (e, n, r) { var i = [], o = e[n]; while (o && 9 !== o.nodeType && (r === t || 1 !== o.nodeType || !b(o).is(r))) 1 === o.nodeType && i.push(o), o = o[n]; return i }, sibling: function (e, t) { var n = []; for (; e; e = e.nextSibling) 1 === e.nodeType && e !== t && n.push(e); return n } }); function ft(e, t, n) { if (t = t || 0, b.isFunction(t)) return b.grep(e, function (e, r) { var i = !!t.call(e, r, e); return i === n }); if (t.nodeType) return b.grep(e, function (e) { return e === t === n }); if ("string" == typeof t) { var r = b.grep(e, function (e) { return 1 === e.nodeType }); if (ut.test(t)) return b.filter(t, r, !n); t = b.filter(t, r) } return b.grep(e, function (e) { return b.inArray(e, t) >= 0 === n }) } function dt(e) { var t = ht.split("|"), n = e.createDocumentFragment(); if (n.createElement) while (t.length) n.createElement(t.pop()); return n } var ht = "abbr|article|aside|audio|bdi|canvas|data|datalist|details|figcaption|figure|footer|header|hgroup|mark|meter|nav|output|progress|section|summary|time|video", gt = / jQuery\d+="(?:null|\d+)"/g, mt = RegExp("<(?:" + ht + ")[\\s/>]", "i"), yt = /^\s+/, vt = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:]+)[^>]*)\/>/gi, bt = /<([\w:]+)/, xt = /<tbody/i, wt = /<|&#?\w+;/, Tt = /<(?:script|style|link)/i, Nt = /^(?:checkbox|radio)$/i, Ct = /checked\s*(?:[^=]|=\s*.checked.)/i, kt = /^$|\/(?:java|ecma)script/i, Et = /^true\/(.*)/, St = /^\s*<!(?:\[CDATA\[|--)|(?:\]\]|--)>\s*$/g, At = { option: [1, "<select multiple='multiple'>", "</select>"], legend: [1, "<fieldset>", "</fieldset>"], area: [1, "<map>", "</map>"], param: [1, "<object>", "</object>"], thead: [1, "<table>", "</table>"], tr: [2, "<table><tbody>", "</tbody></table>"], col: [2, "<table><tbody></tbody><colgroup>", "</colgroup></table>"], td: [3, "<table><tbody><tr>", "</tr></tbody></table>"], _default: b.support.htmlSerialize ? [0, "", ""] : [1, "X<div>", "</div>"] }, jt = dt(o), Dt = jt.appendChild(o.createElement("div")); At.optgroup = At.option, At.tbody = At.tfoot = At.colgroup = At.caption = At.thead, At.th = At.td, b.fn.extend({ text: function (e) { return b.access(this, function (e) { return e === t ? b.text(this) : this.empty().append((this[0] && this[0].ownerDocument || o).createTextNode(e)) }, null, e, arguments.length) }, wrapAll: function (e) { if (b.isFunction(e)) return this.each(function (t) { b(this).wrapAll(e.call(this, t)) }); if (this[0]) { var t = b(e, this[0].ownerDocument).eq(0).clone(!0); this[0].parentNode && t.insertBefore(this[0]), t.map(function () { var e = this; while (e.firstChild && 1 === e.firstChild.nodeType) e = e.firstChild; return e }).append(this) } return this }, wrapInner: function (e) { return b.isFunction(e) ? this.each(function (t) { b(this).wrapInner(e.call(this, t)) }) : this.each(function () { var t = b(this), n = t.contents(); n.length ? n.wrapAll(e) : t.append(e) }) }, wrap: function (e) { var t = b.isFunction(e); return this.each(function (n) { b(this).wrapAll(t ? e.call(this, n) : e) }) }, unwrap: function () { return this.parent().each(function () { b.nodeName(this, "body") || b(this).replaceWith(this.childNodes) }).end() }, append: function () { return this.domManip(arguments, !0, function (e) { (1 === this.nodeType || 11 === this.nodeType || 9 === this.nodeType) && this.appendChild(e) }) }, prepend: function () { return this.domManip(arguments, !0, function (e) { (1 === this.nodeType || 11 === this.nodeType || 9 === this.nodeType) && this.insertBefore(e, this.firstChild) }) }, before: function () { return this.domManip(arguments, !1, function (e) { this.parentNode && this.parentNode.insertBefore(e, this) }) }, after: function () { return this.domManip(arguments, !1, function (e) { this.parentNode && this.parentNode.insertBefore(e, this.nextSibling) }) }, remove: function (e, t) { var n, r = 0; for (; null != (n = this[r]) ; r++) (!e || b.filter(e, [n]).length > 0) && (t || 1 !== n.nodeType || b.cleanData(Ot(n)), n.parentNode && (t && b.contains(n.ownerDocument, n) && Mt(Ot(n, "script")), n.parentNode.removeChild(n))); return this }, empty: function () { var e, t = 0; for (; null != (e = this[t]) ; t++) { 1 === e.nodeType && b.cleanData(Ot(e, !1)); while (e.firstChild) e.removeChild(e.firstChild); e.options && b.nodeName(e, "select") && (e.options.length = 0) } return this }, clone: function (e, t) { return e = null == e ? !1 : e, t = null == t ? e : t, this.map(function () { return b.clone(this, e, t) }) }, html: function (e) { return b.access(this, function (e) { var n = this[0] || {}, r = 0, i = this.length; if (e === t) return 1 === n.nodeType ? n.innerHTML.replace(gt, "") : t; if (!("string" != typeof e || Tt.test(e) || !b.support.htmlSerialize && mt.test(e) || !b.support.leadingWhitespace && yt.test(e) || At[(bt.exec(e) || ["", ""])[1].toLowerCase()])) { e = e.replace(vt, "<$1></$2>"); try { for (; i > r; r++) n = this[r] || {}, 1 === n.nodeType && (b.cleanData(Ot(n, !1)), n.innerHTML = e); n = 0 } catch (o) { } } n && this.empty().append(e) }, null, e, arguments.length) }, replaceWith: function (e) { var t = b.isFunction(e); return t || "string" == typeof e || (e = b(e).not(this).detach()), this.domManip([e], !0, function (e) { var t = this.nextSibling, n = this.parentNode; n && (b(this).remove(), n.insertBefore(e, t)) }) }, detach: function (e) { return this.remove(e, !0) }, domManip: function (e, n, r) { e = f.apply([], e); var i, o, a, s, u, l, c = 0, p = this.length, d = this, h = p - 1, g = e[0], m = b.isFunction(g); if (m || !(1 >= p || "string" != typeof g || b.support.checkClone) && Ct.test(g)) return this.each(function (i) { var o = d.eq(i); m && (e[0] = g.call(this, i, n ? o.html() : t)), o.domManip(e, n, r) }); if (p && (l = b.buildFragment(e, this[0].ownerDocument, !1, this), i = l.firstChild, 1 === l.childNodes.length && (l = i), i)) { for (n = n && b.nodeName(i, "tr"), s = b.map(Ot(l, "script"), Ht), a = s.length; p > c; c++) o = l, c !== h && (o = b.clone(o, !0, !0), a && b.merge(s, Ot(o, "script"))), r.call(n && b.nodeName(this[c], "table") ? Lt(this[c], "tbody") : this[c], o, c); if (a) for (u = s[s.length - 1].ownerDocument, b.map(s, qt), c = 0; a > c; c++) o = s[c], kt.test(o.type || "") && !b._data(o, "globalEval") && b.contains(u, o) && (o.src ? b.ajax({ url: o.src, type: "GET", dataType: "script", async: !1, global: !1, "throws": !0 }) : b.globalEval((o.text || o.textContent || o.innerHTML || "").replace(St, ""))); l = i = null } return this } }); function Lt(e, t) { return e.getElementsByTagName(t)[0] || e.appendChild(e.ownerDocument.createElement(t)) } function Ht(e) { var t = e.getAttributeNode("type"); return e.type = (t && t.specified) + "/" + e.type, e } function qt(e) { var t = Et.exec(e.type); return t ? e.type = t[1] : e.removeAttribute("type"), e } function Mt(e, t) { var n, r = 0; for (; null != (n = e[r]) ; r++) b._data(n, "globalEval", !t || b._data(t[r], "globalEval")) } function _t(e, t) { if (1 === t.nodeType && b.hasData(e)) { var n, r, i, o = b._data(e), a = b._data(t, o), s = o.events; if (s) { delete a.handle, a.events = {}; for (n in s) for (r = 0, i = s[n].length; i > r; r++) b.event.add(t, n, s[n][r]) } a.data && (a.data = b.extend({}, a.data)) } } function Ft(e, t) { var n, r, i; if (1 === t.nodeType) { if (n = t.nodeName.toLowerCase(), !b.support.noCloneEvent && t[b.expando]) { i = b._data(t); for (r in i.events) b.removeEvent(t, r, i.handle); t.removeAttribute(b.expando) } "script" === n && t.text !== e.text ? (Ht(t).text = e.text, qt(t)) : "object" === n ? (t.parentNode && (t.outerHTML = e.outerHTML), b.support.html5Clone && e.innerHTML && !b.trim(t.innerHTML) && (t.innerHTML = e.innerHTML)) : "input" === n && Nt.test(e.type) ? (t.defaultChecked = t.checked = e.checked, t.value !== e.value && (t.value = e.value)) : "option" === n ? t.defaultSelected = t.selected = e.defaultSelected : ("input" === n || "textarea" === n) && (t.defaultValue = e.defaultValue) } } b.each({ appendTo: "append", prependTo: "prepend", insertBefore: "before", insertAfter: "after", replaceAll: "replaceWith" }, function (e, t) { b.fn[e] = function (e) { var n, r = 0, i = [], o = b(e), a = o.length - 1; for (; a >= r; r++) n = r === a ? this : this.clone(!0), b(o[r])[t](n), d.apply(i, n.get()); return this.pushStack(i) } }); function Ot(e, n) { var r, o, a = 0, s = typeof e.getElementsByTagName !== i ? e.getElementsByTagName(n || "*") : typeof e.querySelectorAll !== i ? e.querySelectorAll(n || "*") : t; if (!s) for (s = [], r = e.childNodes || e; null != (o = r[a]) ; a++) !n || b.nodeName(o, n) ? s.push(o) : b.merge(s, Ot(o, n)); return n === t || n && b.nodeName(e, n) ? b.merge([e], s) : s } function Bt(e) { Nt.test(e.type) && (e.defaultChecked = e.checked) } b.extend({
        clone: function (e, t, n) { var r, i, o, a, s, u = b.contains(e.ownerDocument, e); if (b.support.html5Clone || b.isXMLDoc(e) || !mt.test("<" + e.nodeName + ">") ? o = e.cloneNode(!0) : (Dt.innerHTML = e.outerHTML, Dt.removeChild(o = Dt.firstChild)), !(b.support.noCloneEvent && b.support.noCloneChecked || 1 !== e.nodeType && 11 !== e.nodeType || b.isXMLDoc(e))) for (r = Ot(o), s = Ot(e), a = 0; null != (i = s[a]) ; ++a) r[a] && Ft(i, r[a]); if (t) if (n) for (s = s || Ot(e), r = r || Ot(o), a = 0; null != (i = s[a]) ; a++) _t(i, r[a]); else _t(e, o); return r = Ot(o, "script"), r.length > 0 && Mt(r, !u && Ot(e, "script")), r = s = i = null, o }, buildFragment: function (e, t, n, r) {
            var i, o, a, s, u, l, c, p = e.length, f = dt(t), d = [], h = 0; for (; p > h; h++) if (o = e[h], o || 0 === o) if ("object" === b.type(o)) b.merge(d, o.nodeType ? [o] : o); else if (wt.test(o)) {
                s = s || f.appendChild(t.createElement("div")), u = (bt.exec(o) || ["", ""])[1].toLowerCase(), c = At[u] || At._default, s.innerHTML = c[1] + o.replace(vt, "<$1></$2>") + c[2], i = c[0]; while (i--) s = s.lastChild; if (!b.support.leadingWhitespace && yt.test(o) && d.push(t.createTextNode(yt.exec(o)[0])), !b.support.tbody) {
                    o = "table" !== u || xt.test(o) ? "<table>" !== c[1] || xt.test(o) ? 0 : s : s.firstChild, i = o && o.childNodes.length; while (i--) b.nodeName(l = o.childNodes[i], "tbody") && !l.childNodes.length && o.removeChild(l)
                } b.merge(d, s.childNodes), s.textContent = ""; while (s.firstChild) s.removeChild(s.firstChild); s = f.lastChild
            } else d.push(t.createTextNode(o)); s && f.removeChild(s), b.support.appendChecked || b.grep(Ot(d, "input"), Bt), h = 0; while (o = d[h++]) if ((!r || -1 === b.inArray(o, r)) && (a = b.contains(o.ownerDocument, o), s = Ot(f.appendChild(o), "script"), a && Mt(s), n)) { i = 0; while (o = s[i++]) kt.test(o.type || "") && n.push(o) } return s = null, f
        }, cleanData: function (e, t) { var n, r, o, a, s = 0, u = b.expando, l = b.cache, p = b.support.deleteExpando, f = b.event.special; for (; null != (n = e[s]) ; s++) if ((t || b.acceptData(n)) && (o = n[u], a = o && l[o])) { if (a.events) for (r in a.events) f[r] ? b.event.remove(n, r) : b.removeEvent(n, r, a.handle); l[o] && (delete l[o], p ? delete n[u] : typeof n.removeAttribute !== i ? n.removeAttribute(u) : n[u] = null, c.push(o)) } }
    });
    var Pt, Rt, Wt, $t = /alpha\([^)]*\)/i, It = /opacity\s*=\s*([^)]*)/, zt = /^(top|right|bottom|left)$/, Xt = /^(none|table(?!-c[ea]).+)/, Ut = /^margin/, Vt = RegExp("^(" + x + ")(.*)$", "i"), Yt = RegExp("^(" + x + ")(?!px)[a-z%]+$", "i"), Jt = RegExp("^([+-])=(" + x + ")", "i"), Gt = { BODY: "block" }, Qt = { position: "absolute", visibility: "hidden", display: "block" }, Kt = { letterSpacing: 0, fontWeight: 400 }, Zt = ["Top", "Right", "Bottom", "Left"], en = ["Webkit", "O", "Moz", "ms"]; function tn(e, t) { if (t in e) return t; var n = t.charAt(0).toUpperCase() + t.slice(1), r = t, i = en.length; while (i--) if (t = en[i] + n, t in e) return t; return r } function nn(e, t) { return e = t || e, "none" === b.css(e, "display") || !b.contains(e.ownerDocument, e) } function rn(e, t) { var n, r, i, o = [], a = 0, s = e.length; for (; s > a; a++) r = e[a], r.style && (o[a] = b._data(r, "olddisplay"), n = r.style.display, t ? (o[a] || "none" !== n || (r.style.display = ""), "" === r.style.display && nn(r) && (o[a] = b._data(r, "olddisplay", un(r.nodeName)))) : o[a] || (i = nn(r), (n && "none" !== n || !i) && b._data(r, "olddisplay", i ? n : b.css(r, "display")))); for (a = 0; s > a; a++) r = e[a], r.style && (t && "none" !== r.style.display && "" !== r.style.display || (r.style.display = t ? o[a] || "" : "none")); return e } b.fn.extend({ css: function (e, n) { return b.access(this, function (e, n, r) { var i, o, a = {}, s = 0; if (b.isArray(n)) { for (o = Rt(e), i = n.length; i > s; s++) a[n[s]] = b.css(e, n[s], !1, o); return a } return r !== t ? b.style(e, n, r) : b.css(e, n) }, e, n, arguments.length > 1) }, show: function () { return rn(this, !0) }, hide: function () { return rn(this) }, toggle: function (e) { var t = "boolean" == typeof e; return this.each(function () { (t ? e : nn(this)) ? b(this).show() : b(this).hide() }) } }), b.extend({ cssHooks: { opacity: { get: function (e, t) { if (t) { var n = Wt(e, "opacity"); return "" === n ? "1" : n } } } }, cssNumber: { columnCount: !0, fillOpacity: !0, fontWeight: !0, lineHeight: !0, opacity: !0, orphans: !0, widows: !0, zIndex: !0, zoom: !0 }, cssProps: { "float": b.support.cssFloat ? "cssFloat" : "styleFloat" }, style: function (e, n, r, i) { if (e && 3 !== e.nodeType && 8 !== e.nodeType && e.style) { var o, a, s, u = b.camelCase(n), l = e.style; if (n = b.cssProps[u] || (b.cssProps[u] = tn(l, u)), s = b.cssHooks[n] || b.cssHooks[u], r === t) return s && "get" in s && (o = s.get(e, !1, i)) !== t ? o : l[n]; if (a = typeof r, "string" === a && (o = Jt.exec(r)) && (r = (o[1] + 1) * o[2] + parseFloat(b.css(e, n)), a = "number"), !(null == r || "number" === a && isNaN(r) || ("number" !== a || b.cssNumber[u] || (r += "px"), b.support.clearCloneStyle || "" !== r || 0 !== n.indexOf("background") || (l[n] = "inherit"), s && "set" in s && (r = s.set(e, r, i)) === t))) try { l[n] = r } catch (c) { } } }, css: function (e, n, r, i) { var o, a, s, u = b.camelCase(n); return n = b.cssProps[u] || (b.cssProps[u] = tn(e.style, u)), s = b.cssHooks[n] || b.cssHooks[u], s && "get" in s && (a = s.get(e, !0, r)), a === t && (a = Wt(e, n, i)), "normal" === a && n in Kt && (a = Kt[n]), "" === r || r ? (o = parseFloat(a), r === !0 || b.isNumeric(o) ? o || 0 : a) : a }, swap: function (e, t, n, r) { var i, o, a = {}; for (o in t) a[o] = e.style[o], e.style[o] = t[o]; i = n.apply(e, r || []); for (o in t) e.style[o] = a[o]; return i } }), e.getComputedStyle ? (Rt = function (t) { return e.getComputedStyle(t, null) }, Wt = function (e, n, r) { var i, o, a, s = r || Rt(e), u = s ? s.getPropertyValue(n) || s[n] : t, l = e.style; return s && ("" !== u || b.contains(e.ownerDocument, e) || (u = b.style(e, n)), Yt.test(u) && Ut.test(n) && (i = l.width, o = l.minWidth, a = l.maxWidth, l.minWidth = l.maxWidth = l.width = u, u = s.width, l.width = i, l.minWidth = o, l.maxWidth = a)), u }) : o.documentElement.currentStyle && (Rt = function (e) { return e.currentStyle }, Wt = function (e, n, r) { var i, o, a, s = r || Rt(e), u = s ? s[n] : t, l = e.style; return null == u && l && l[n] && (u = l[n]), Yt.test(u) && !zt.test(n) && (i = l.left, o = e.runtimeStyle, a = o && o.left, a && (o.left = e.currentStyle.left), l.left = "fontSize" === n ? "1em" : u, u = l.pixelLeft + "px", l.left = i, a && (o.left = a)), "" === u ? "auto" : u }); function on(e, t, n) { var r = Vt.exec(t); return r ? Math.max(0, r[1] - (n || 0)) + (r[2] || "px") : t } function an(e, t, n, r, i) { var o = n === (r ? "border" : "content") ? 4 : "width" === t ? 1 : 0, a = 0; for (; 4 > o; o += 2) "margin" === n && (a += b.css(e, n + Zt[o], !0, i)), r ? ("content" === n && (a -= b.css(e, "padding" + Zt[o], !0, i)), "margin" !== n && (a -= b.css(e, "border" + Zt[o] + "Width", !0, i))) : (a += b.css(e, "padding" + Zt[o], !0, i), "padding" !== n && (a += b.css(e, "border" + Zt[o] + "Width", !0, i))); return a } function sn(e, t, n) { var r = !0, i = "width" === t ? e.offsetWidth : e.offsetHeight, o = Rt(e), a = b.support.boxSizing && "border-box" === b.css(e, "boxSizing", !1, o); if (0 >= i || null == i) { if (i = Wt(e, t, o), (0 > i || null == i) && (i = e.style[t]), Yt.test(i)) return i; r = a && (b.support.boxSizingReliable || i === e.style[t]), i = parseFloat(i) || 0 } return i + an(e, t, n || (a ? "border" : "content"), r, o) + "px" } function un(e) { var t = o, n = Gt[e]; return n || (n = ln(e, t), "none" !== n && n || (Pt = (Pt || b("<iframe frameborder='0' width='0' height='0'/>").css("cssText", "display:block !important")).appendTo(t.documentElement), t = (Pt[0].contentWindow || Pt[0].contentDocument).document, t.write("<!doctype html><html><body>"), t.close(), n = ln(e, t), Pt.detach()), Gt[e] = n), n } function ln(e, t) { var n = b(t.createElement(e)).appendTo(t.body), r = b.css(n[0], "display"); return n.remove(), r } b.each(["height", "width"], function (e, n) { b.cssHooks[n] = { get: function (e, r, i) { return r ? 0 === e.offsetWidth && Xt.test(b.css(e, "display")) ? b.swap(e, Qt, function () { return sn(e, n, i) }) : sn(e, n, i) : t }, set: function (e, t, r) { var i = r && Rt(e); return on(e, t, r ? an(e, n, r, b.support.boxSizing && "border-box" === b.css(e, "boxSizing", !1, i), i) : 0) } } }), b.support.opacity || (b.cssHooks.opacity = { get: function (e, t) { return It.test((t && e.currentStyle ? e.currentStyle.filter : e.style.filter) || "") ? .01 * parseFloat(RegExp.$1) + "" : t ? "1" : "" }, set: function (e, t) { var n = e.style, r = e.currentStyle, i = b.isNumeric(t) ? "alpha(opacity=" + 100 * t + ")" : "", o = r && r.filter || n.filter || ""; n.zoom = 1, (t >= 1 || "" === t) && "" === b.trim(o.replace($t, "")) && n.removeAttribute && (n.removeAttribute("filter"), "" === t || r && !r.filter) || (n.filter = $t.test(o) ? o.replace($t, i) : o + " " + i) } }), b(function () { b.support.reliableMarginRight || (b.cssHooks.marginRight = { get: function (e, n) { return n ? b.swap(e, { display: "inline-block" }, Wt, [e, "marginRight"]) : t } }), !b.support.pixelPosition && b.fn.position && b.each(["top", "left"], function (e, n) { b.cssHooks[n] = { get: function (e, r) { return r ? (r = Wt(e, n), Yt.test(r) ? b(e).position()[n] + "px" : r) : t } } }) }), b.expr && b.expr.filters && (b.expr.filters.hidden = function (e) { return 0 >= e.offsetWidth && 0 >= e.offsetHeight || !b.support.reliableHiddenOffsets && "none" === (e.style && e.style.display || b.css(e, "display")) }, b.expr.filters.visible = function (e) { return !b.expr.filters.hidden(e) }), b.each({ margin: "", padding: "", border: "Width" }, function (e, t) { b.cssHooks[e + t] = { expand: function (n) { var r = 0, i = {}, o = "string" == typeof n ? n.split(" ") : [n]; for (; 4 > r; r++) i[e + Zt[r] + t] = o[r] || o[r - 2] || o[0]; return i } }, Ut.test(e) || (b.cssHooks[e + t].set = on) }); var cn = /%20/g, pn = /\[\]$/, fn = /\r?\n/g, dn = /^(?:submit|button|image|reset|file)$/i, hn = /^(?:input|select|textarea|keygen)/i; b.fn.extend({ serialize: function () { return b.param(this.serializeArray()) }, serializeArray: function () { return this.map(function () { var e = b.prop(this, "elements"); return e ? b.makeArray(e) : this }).filter(function () { var e = this.type; return this.name && !b(this).is(":disabled") && hn.test(this.nodeName) && !dn.test(e) && (this.checked || !Nt.test(e)) }).map(function (e, t) { var n = b(this).val(); return null == n ? null : b.isArray(n) ? b.map(n, function (e) { return { name: t.name, value: e.replace(fn, "\r\n") } }) : { name: t.name, value: n.replace(fn, "\r\n") } }).get() } }), b.param = function (e, n) { var r, i = [], o = function (e, t) { t = b.isFunction(t) ? t() : null == t ? "" : t, i[i.length] = encodeURIComponent(e) + "=" + encodeURIComponent(t) }; if (n === t && (n = b.ajaxSettings && b.ajaxSettings.traditional), b.isArray(e) || e.jquery && !b.isPlainObject(e)) b.each(e, function () { o(this.name, this.value) }); else for (r in e) gn(r, e[r], n, o); return i.join("&").replace(cn, "+") }; function gn(e, t, n, r) { var i; if (b.isArray(t)) b.each(t, function (t, i) { n || pn.test(e) ? r(e, i) : gn(e + "[" + ("object" == typeof i ? t : "") + "]", i, n, r) }); else if (n || "object" !== b.type(t)) r(e, t); else for (i in t) gn(e + "[" + i + "]", t[i], n, r) } b.each("blur focus focusin focusout load resize scroll unload click dblclick mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave change select submit keydown keypress keyup error contextmenu".split(" "), function (e, t) { b.fn[t] = function (e, n) { return arguments.length > 0 ? this.on(t, null, e, n) : this.trigger(t) } }), b.fn.hover = function (e, t) { return this.mouseenter(e).mouseleave(t || e) }; var mn, yn, vn = b.now(), bn = /\?/, xn = /#.*$/, wn = /([?&])_=[^&]*/, Tn = /^(.*?):[ \t]*([^\r\n]*)\r?$/gm, Nn = /^(?:about|app|app-storage|.+-extension|file|res|widget):$/, Cn = /^(?:GET|HEAD)$/, kn = /^\/\//, En = /^([\w.+-]+:)(?:\/\/([^\/?#:]*)(?::(\d+)|)|)/, Sn = b.fn.load, An = {}, jn = {}, Dn = "*/".concat("*"); try { yn = a.href } catch (Ln) { yn = o.createElement("a"), yn.href = "", yn = yn.href } mn = En.exec(yn.toLowerCase()) || []; function Hn(e) { return function (t, n) { "string" != typeof t && (n = t, t = "*"); var r, i = 0, o = t.toLowerCase().match(w) || []; if (b.isFunction(n)) while (r = o[i++]) "+" === r[0] ? (r = r.slice(1) || "*", (e[r] = e[r] || []).unshift(n)) : (e[r] = e[r] || []).push(n) } } function qn(e, n, r, i) { var o = {}, a = e === jn; function s(u) { var l; return o[u] = !0, b.each(e[u] || [], function (e, u) { var c = u(n, r, i); return "string" != typeof c || a || o[c] ? a ? !(l = c) : t : (n.dataTypes.unshift(c), s(c), !1) }), l } return s(n.dataTypes[0]) || !o["*"] && s("*") } function Mn(e, n) { var r, i, o = b.ajaxSettings.flatOptions || {}; for (i in n) n[i] !== t && ((o[i] ? e : r || (r = {}))[i] = n[i]); return r && b.extend(!0, e, r), e } b.fn.load = function (e, n, r) { if ("string" != typeof e && Sn) return Sn.apply(this, arguments); var i, o, a, s = this, u = e.indexOf(" "); return u >= 0 && (i = e.slice(u, e.length), e = e.slice(0, u)), b.isFunction(n) ? (r = n, n = t) : n && "object" == typeof n && (a = "POST"), s.length > 0 && b.ajax({ url: e, type: a, dataType: "html", data: n }).done(function (e) { o = arguments, s.html(i ? b("<div>").append(b.parseHTML(e)).find(i) : e) }).complete(r && function (e, t) { s.each(r, o || [e.responseText, t, e]) }), this }, b.each(["ajaxStart", "ajaxStop", "ajaxComplete", "ajaxError", "ajaxSuccess", "ajaxSend"], function (e, t) { b.fn[t] = function (e) { return this.on(t, e) } }), b.each(["get", "post"], function (e, n) { b[n] = function (e, r, i, o) { return b.isFunction(r) && (o = o || i, i = r, r = t), b.ajax({ url: e, type: n, dataType: o, data: r, success: i }) } }), b.extend({ active: 0, lastModified: {}, etag: {}, ajaxSettings: { url: yn, type: "GET", isLocal: Nn.test(mn[1]), global: !0, processData: !0, async: !0, contentType: "application/x-www-form-urlencoded; charset=UTF-8", accepts: { "*": Dn, text: "text/plain", html: "text/html", xml: "application/xml, text/xml", json: "application/json, text/javascript" }, contents: { xml: /xml/, html: /html/, json: /json/ }, responseFields: { xml: "responseXML", text: "responseText" }, converters: { "* text": e.String, "text html": !0, "text json": b.parseJSON, "text xml": b.parseXML }, flatOptions: { url: !0, context: !0 } }, ajaxSetup: function (e, t) { return t ? Mn(Mn(e, b.ajaxSettings), t) : Mn(b.ajaxSettings, e) }, ajaxPrefilter: Hn(An), ajaxTransport: Hn(jn), ajax: function (e, n) { "object" == typeof e && (n = e, e = t), n = n || {}; var r, i, o, a, s, u, l, c, p = b.ajaxSetup({}, n), f = p.context || p, d = p.context && (f.nodeType || f.jquery) ? b(f) : b.event, h = b.Deferred(), g = b.Callbacks("once memory"), m = p.statusCode || {}, y = {}, v = {}, x = 0, T = "canceled", N = { readyState: 0, getResponseHeader: function (e) { var t; if (2 === x) { if (!c) { c = {}; while (t = Tn.exec(a)) c[t[1].toLowerCase()] = t[2] } t = c[e.toLowerCase()] } return null == t ? null : t }, getAllResponseHeaders: function () { return 2 === x ? a : null }, setRequestHeader: function (e, t) { var n = e.toLowerCase(); return x || (e = v[n] = v[n] || e, y[e] = t), this }, overrideMimeType: function (e) { return x || (p.mimeType = e), this }, statusCode: function (e) { var t; if (e) if (2 > x) for (t in e) m[t] = [m[t], e[t]]; else N.always(e[N.status]); return this }, abort: function (e) { var t = e || T; return l && l.abort(t), k(0, t), this } }; if (h.promise(N).complete = g.add, N.success = N.done, N.error = N.fail, p.url = ((e || p.url || yn) + "").replace(xn, "").replace(kn, mn[1] + "//"), p.type = n.method || n.type || p.method || p.type, p.dataTypes = b.trim(p.dataType || "*").toLowerCase().match(w) || [""], null == p.crossDomain && (r = En.exec(p.url.toLowerCase()), p.crossDomain = !(!r || r[1] === mn[1] && r[2] === mn[2] && (r[3] || ("http:" === r[1] ? 80 : 443)) == (mn[3] || ("http:" === mn[1] ? 80 : 443)))), p.data && p.processData && "string" != typeof p.data && (p.data = b.param(p.data, p.traditional)), qn(An, p, n, N), 2 === x) return N; u = p.global, u && 0 === b.active++ && b.event.trigger("ajaxStart"), p.type = p.type.toUpperCase(), p.hasContent = !Cn.test(p.type), o = p.url, p.hasContent || (p.data && (o = p.url += (bn.test(o) ? "&" : "?") + p.data, delete p.data), p.cache === !1 && (p.url = wn.test(o) ? o.replace(wn, "$1_=" + vn++) : o + (bn.test(o) ? "&" : "?") + "_=" + vn++)), p.ifModified && (b.lastModified[o] && N.setRequestHeader("If-Modified-Since", b.lastModified[o]), b.etag[o] && N.setRequestHeader("If-None-Match", b.etag[o])), (p.data && p.hasContent && p.contentType !== !1 || n.contentType) && N.setRequestHeader("Content-Type", p.contentType), N.setRequestHeader("Accept", p.dataTypes[0] && p.accepts[p.dataTypes[0]] ? p.accepts[p.dataTypes[0]] + ("*" !== p.dataTypes[0] ? ", " + Dn + "; q=0.01" : "") : p.accepts["*"]); for (i in p.headers) N.setRequestHeader(i, p.headers[i]); if (p.beforeSend && (p.beforeSend.call(f, N, p) === !1 || 2 === x)) return N.abort(); T = "abort"; for (i in { success: 1, error: 1, complete: 1 }) N[i](p[i]); if (l = qn(jn, p, n, N)) { N.readyState = 1, u && d.trigger("ajaxSend", [N, p]), p.async && p.timeout > 0 && (s = setTimeout(function () { N.abort("timeout") }, p.timeout)); try { x = 1, l.send(y, k) } catch (C) { if (!(2 > x)) throw C; k(-1, C) } } else k(-1, "No Transport"); function k(e, n, r, i) { var c, y, v, w, T, C = n; 2 !== x && (x = 2, s && clearTimeout(s), l = t, a = i || "", N.readyState = e > 0 ? 4 : 0, r && (w = _n(p, N, r)), e >= 200 && 300 > e || 304 === e ? (p.ifModified && (T = N.getResponseHeader("Last-Modified"), T && (b.lastModified[o] = T), T = N.getResponseHeader("etag"), T && (b.etag[o] = T)), 204 === e ? (c = !0, C = "nocontent") : 304 === e ? (c = !0, C = "notmodified") : (c = Fn(p, w), C = c.state, y = c.data, v = c.error, c = !v)) : (v = C, (e || !C) && (C = "error", 0 > e && (e = 0))), N.status = e, N.statusText = (n || C) + "", c ? h.resolveWith(f, [y, C, N]) : h.rejectWith(f, [N, C, v]), N.statusCode(m), m = t, u && d.trigger(c ? "ajaxSuccess" : "ajaxError", [N, p, c ? y : v]), g.fireWith(f, [N, C]), u && (d.trigger("ajaxComplete", [N, p]), --b.active || b.event.trigger("ajaxStop"))) } return N }, getScript: function (e, n) { return b.get(e, t, n, "script") }, getJSON: function (e, t, n) { return b.get(e, t, n, "json") } }); function _n(e, n, r) { var i, o, a, s, u = e.contents, l = e.dataTypes, c = e.responseFields; for (s in c) s in r && (n[c[s]] = r[s]); while ("*" === l[0]) l.shift(), o === t && (o = e.mimeType || n.getResponseHeader("Content-Type")); if (o) for (s in u) if (u[s] && u[s].test(o)) { l.unshift(s); break } if (l[0] in r) a = l[0]; else { for (s in r) { if (!l[0] || e.converters[s + " " + l[0]]) { a = s; break } i || (i = s) } a = a || i } return a ? (a !== l[0] && l.unshift(a), r[a]) : t } function Fn(e, t) { var n, r, i, o, a = {}, s = 0, u = e.dataTypes.slice(), l = u[0]; if (e.dataFilter && (t = e.dataFilter(t, e.dataType)), u[1]) for (i in e.converters) a[i.toLowerCase()] = e.converters[i]; for (; r = u[++s];) if ("*" !== r) { if ("*" !== l && l !== r) { if (i = a[l + " " + r] || a["* " + r], !i) for (n in a) if (o = n.split(" "), o[1] === r && (i = a[l + " " + o[0]] || a["* " + o[0]])) { i === !0 ? i = a[n] : a[n] !== !0 && (r = o[0], u.splice(s--, 0, r)); break } if (i !== !0) if (i && e["throws"]) t = i(t); else try { t = i(t) } catch (c) { return { state: "parsererror", error: i ? c : "No conversion from " + l + " to " + r } } } l = r } return { state: "success", data: t } } b.ajaxSetup({ accepts: { script: "text/javascript, application/javascript, application/ecmascript, application/x-ecmascript" }, contents: { script: /(?:java|ecma)script/ }, converters: { "text script": function (e) { return b.globalEval(e), e } } }), b.ajaxPrefilter("script", function (e) { e.cache === t && (e.cache = !1), e.crossDomain && (e.type = "GET", e.global = !1) }), b.ajaxTransport("script", function (e) { if (e.crossDomain) { var n, r = o.head || b("head")[0] || o.documentElement; return { send: function (t, i) { n = o.createElement("script"), n.async = !0, e.scriptCharset && (n.charset = e.scriptCharset), n.src = e.url, n.onload = n.onreadystatechange = function (e, t) { (t || !n.readyState || /loaded|complete/.test(n.readyState)) && (n.onload = n.onreadystatechange = null, n.parentNode && n.parentNode.removeChild(n), n = null, t || i(200, "success")) }, r.insertBefore(n, r.firstChild) }, abort: function () { n && n.onload(t, !0) } } } }); var On = [], Bn = /(=)\?(?=&|$)|\?\?/; b.ajaxSetup({ jsonp: "callback", jsonpCallback: function () { var e = On.pop() || b.expando + "_" + vn++; return this[e] = !0, e } }), b.ajaxPrefilter("json jsonp", function (n, r, i) { var o, a, s, u = n.jsonp !== !1 && (Bn.test(n.url) ? "url" : "string" == typeof n.data && !(n.contentType || "").indexOf("application/x-www-form-urlencoded") && Bn.test(n.data) && "data"); return u || "jsonp" === n.dataTypes[0] ? (o = n.jsonpCallback = b.isFunction(n.jsonpCallback) ? n.jsonpCallback() : n.jsonpCallback, u ? n[u] = n[u].replace(Bn, "$1" + o) : n.jsonp !== !1 && (n.url += (bn.test(n.url) ? "&" : "?") + n.jsonp + "=" + o), n.converters["script json"] = function () { return s || b.error(o + " was not called"), s[0] }, n.dataTypes[0] = "json", a = e[o], e[o] = function () { s = arguments }, i.always(function () { e[o] = a, n[o] && (n.jsonpCallback = r.jsonpCallback, On.push(o)), s && b.isFunction(a) && a(s[0]), s = a = t }), "script") : t }); var Pn, Rn, Wn = 0, $n = e.ActiveXObject && function () { var e; for (e in Pn) Pn[e](t, !0) }; function In() { try { return new e.XMLHttpRequest } catch (t) { } } function zn() { try { return new e.ActiveXObject("Microsoft.XMLHTTP") } catch (t) { } } b.ajaxSettings.xhr = e.ActiveXObject ? function () { return !this.isLocal && In() || zn() } : In, Rn = b.ajaxSettings.xhr(), b.support.cors = !!Rn && "withCredentials" in Rn, Rn = b.support.ajax = !!Rn, Rn && b.ajaxTransport(function (n) {
        if (!n.crossDomain || b.support.cors) {
            var r; return {
                send: function (i, o) {
                    var a, s, u = n.xhr();
                    if (n.username ? u.open(n.type, n.url, n.async, n.username, n.password) : u.open(n.type, n.url, n.async), n.xhrFields)
                        for (s in n.xhrFields)
                            u[s] = n.xhrFields[s];
                    n.mimeType && u.overrideMimeType && u.overrideMimeType(n.mimeType), n.crossDomain || i["X-Requested-With"] || (i["X-Requested-With"] = "XMLHttpRequest");
                    try {
                        for (s in i) u.setRequestHeader(s, i[s])
                    } catch (l) { }
                    u.send(n.hasContent && n.data || null),
                    r = function (e, i) {
                        var s, l, c, p; try {
                            if (r && (i || 4 === u.readyState)) if (r = t, a && (u.onreadystatechange = b.noop, $n && delete Pn[a]), i) 4 !== u.readyState && u.abort();
                            else { p = {}, s = u.status, l = u.getAllResponseHeaders(), "string" == typeof u.responseText && (p.text = u.responseText); try { c = u.statusText } catch (f) { c = "" } s || !n.isLocal || n.crossDomain ? 1223 === s && (s = 204) : s = p.text ? 200 : 404 }
                        } catch (d) { i || o(-1, d) } p && o(s, c, p, l)
                    },
                    n.async ? 4 === u.readyState ? setTimeout(r) : (a = ++Wn, $n && (Pn || (Pn = {}, b(e).unload($n)), Pn[a] = r), u.onreadystatechange = r) : r()
                }, abort: function () { r && r(t, !0) }
            }
        }
    }); var Xn, Un, Vn = /^(?:toggle|show|hide)$/, Yn = RegExp("^(?:([+-])=|)(" + x + ")([a-z%]*)$", "i"), Jn = /queueHooks$/, Gn = [nr], Qn = { "*": [function (e, t) { var n, r, i = this.createTween(e, t), o = Yn.exec(t), a = i.cur(), s = +a || 0, u = 1, l = 20; if (o) { if (n = +o[2], r = o[3] || (b.cssNumber[e] ? "" : "px"), "px" !== r && s) { s = b.css(i.elem, e, !0) || n || 1; do u = u || ".5", s /= u, b.style(i.elem, e, s + r); while (u !== (u = i.cur() / a) && 1 !== u && --l) } i.unit = r, i.start = s, i.end = o[1] ? s + (o[1] + 1) * n : n } return i }] }; function Kn() { return setTimeout(function () { Xn = t }), Xn = b.now() } function Zn(e, t) { b.each(t, function (t, n) { var r = (Qn[t] || []).concat(Qn["*"]), i = 0, o = r.length; for (; o > i; i++) if (r[i].call(e, t, n)) return }) } function er(e, t, n) { var r, i, o = 0, a = Gn.length, s = b.Deferred().always(function () { delete u.elem }), u = function () { if (i) return !1; var t = Xn || Kn(), n = Math.max(0, l.startTime + l.duration - t), r = n / l.duration || 0, o = 1 - r, a = 0, u = l.tweens.length; for (; u > a; a++) l.tweens[a].run(o); return s.notifyWith(e, [l, o, n]), 1 > o && u ? n : (s.resolveWith(e, [l]), !1) }, l = s.promise({ elem: e, props: b.extend({}, t), opts: b.extend(!0, { specialEasing: {} }, n), originalProperties: t, originalOptions: n, startTime: Xn || Kn(), duration: n.duration, tweens: [], createTween: function (t, n) { var r = b.Tween(e, l.opts, t, n, l.opts.specialEasing[t] || l.opts.easing); return l.tweens.push(r), r }, stop: function (t) { var n = 0, r = t ? l.tweens.length : 0; if (i) return this; for (i = !0; r > n; n++) l.tweens[n].run(1); return t ? s.resolveWith(e, [l, t]) : s.rejectWith(e, [l, t]), this } }), c = l.props; for (tr(c, l.opts.specialEasing) ; a > o; o++) if (r = Gn[o].call(l, e, c, l.opts)) return r; return Zn(l, c), b.isFunction(l.opts.start) && l.opts.start.call(e, l), b.fx.timer(b.extend(u, { elem: e, anim: l, queue: l.opts.queue })), l.progress(l.opts.progress).done(l.opts.done, l.opts.complete).fail(l.opts.fail).always(l.opts.always) } function tr(e, t) { var n, r, i, o, a; for (i in e) if (r = b.camelCase(i), o = t[r], n = e[i], b.isArray(n) && (o = n[1], n = e[i] = n[0]), i !== r && (e[r] = n, delete e[i]), a = b.cssHooks[r], a && "expand" in a) { n = a.expand(n), delete e[r]; for (i in n) i in e || (e[i] = n[i], t[i] = o) } else t[r] = o } b.Animation = b.extend(er, { tweener: function (e, t) { b.isFunction(e) ? (t = e, e = ["*"]) : e = e.split(" "); var n, r = 0, i = e.length; for (; i > r; r++) n = e[r], Qn[n] = Qn[n] || [], Qn[n].unshift(t) }, prefilter: function (e, t) { t ? Gn.unshift(e) : Gn.push(e) } }); function nr(e, t, n) { var r, i, o, a, s, u, l, c, p, f = this, d = e.style, h = {}, g = [], m = e.nodeType && nn(e); n.queue || (c = b._queueHooks(e, "fx"), null == c.unqueued && (c.unqueued = 0, p = c.empty.fire, c.empty.fire = function () { c.unqueued || p() }), c.unqueued++, f.always(function () { f.always(function () { c.unqueued--, b.queue(e, "fx").length || c.empty.fire() }) })), 1 === e.nodeType && ("height" in t || "width" in t) && (n.overflow = [d.overflow, d.overflowX, d.overflowY], "inline" === b.css(e, "display") && "none" === b.css(e, "float") && (b.support.inlineBlockNeedsLayout && "inline" !== un(e.nodeName) ? d.zoom = 1 : d.display = "inline-block")), n.overflow && (d.overflow = "hidden", b.support.shrinkWrapBlocks || f.always(function () { d.overflow = n.overflow[0], d.overflowX = n.overflow[1], d.overflowY = n.overflow[2] })); for (i in t) if (a = t[i], Vn.exec(a)) { if (delete t[i], u = u || "toggle" === a, a === (m ? "hide" : "show")) continue; g.push(i) } if (o = g.length) { s = b._data(e, "fxshow") || b._data(e, "fxshow", {}), "hidden" in s && (m = s.hidden), u && (s.hidden = !m), m ? b(e).show() : f.done(function () { b(e).hide() }), f.done(function () { var t; b._removeData(e, "fxshow"); for (t in h) b.style(e, t, h[t]) }); for (i = 0; o > i; i++) r = g[i], l = f.createTween(r, m ? s[r] : 0), h[r] = s[r] || b.style(e, r), r in s || (s[r] = l.start, m && (l.end = l.start, l.start = "width" === r || "height" === r ? 1 : 0)) } } function rr(e, t, n, r, i) { return new rr.prototype.init(e, t, n, r, i) } b.Tween = rr, rr.prototype = { constructor: rr, init: function (e, t, n, r, i, o) { this.elem = e, this.prop = n, this.easing = i || "swing", this.options = t, this.start = this.now = this.cur(), this.end = r, this.unit = o || (b.cssNumber[n] ? "" : "px") }, cur: function () { var e = rr.propHooks[this.prop]; return e && e.get ? e.get(this) : rr.propHooks._default.get(this) }, run: function (e) { var t, n = rr.propHooks[this.prop]; return this.pos = t = this.options.duration ? b.easing[this.easing](e, this.options.duration * e, 0, 1, this.options.duration) : e, this.now = (this.end - this.start) * t + this.start, this.options.step && this.options.step.call(this.elem, this.now, this), n && n.set ? n.set(this) : rr.propHooks._default.set(this), this } }, rr.prototype.init.prototype = rr.prototype, rr.propHooks = { _default: { get: function (e) { var t; return null == e.elem[e.prop] || e.elem.style && null != e.elem.style[e.prop] ? (t = b.css(e.elem, e.prop, ""), t && "auto" !== t ? t : 0) : e.elem[e.prop] }, set: function (e) { b.fx.step[e.prop] ? b.fx.step[e.prop](e) : e.elem.style && (null != e.elem.style[b.cssProps[e.prop]] || b.cssHooks[e.prop]) ? b.style(e.elem, e.prop, e.now + e.unit) : e.elem[e.prop] = e.now } } }, rr.propHooks.scrollTop = rr.propHooks.scrollLeft = { set: function (e) { e.elem.nodeType && e.elem.parentNode && (e.elem[e.prop] = e.now) } }, b.each(["toggle", "show", "hide"], function (e, t) { var n = b.fn[t]; b.fn[t] = function (e, r, i) { return null == e || "boolean" == typeof e ? n.apply(this, arguments) : this.animate(ir(t, !0), e, r, i) } }), b.fn.extend({ fadeTo: function (e, t, n, r) { return this.filter(nn).css("opacity", 0).show().end().animate({ opacity: t }, e, n, r) }, animate: function (e, t, n, r) { var i = b.isEmptyObject(e), o = b.speed(t, n, r), a = function () { var t = er(this, b.extend({}, e), o); a.finish = function () { t.stop(!0) }, (i || b._data(this, "finish")) && t.stop(!0) }; return a.finish = a, i || o.queue === !1 ? this.each(a) : this.queue(o.queue, a) }, stop: function (e, n, r) { var i = function (e) { var t = e.stop; delete e.stop, t(r) }; return "string" != typeof e && (r = n, n = e, e = t), n && e !== !1 && this.queue(e || "fx", []), this.each(function () { var t = !0, n = null != e && e + "queueHooks", o = b.timers, a = b._data(this); if (n) a[n] && a[n].stop && i(a[n]); else for (n in a) a[n] && a[n].stop && Jn.test(n) && i(a[n]); for (n = o.length; n--;) o[n].elem !== this || null != e && o[n].queue !== e || (o[n].anim.stop(r), t = !1, o.splice(n, 1)); (t || !r) && b.dequeue(this, e) }) }, finish: function (e) { return e !== !1 && (e = e || "fx"), this.each(function () { var t, n = b._data(this), r = n[e + "queue"], i = n[e + "queueHooks"], o = b.timers, a = r ? r.length : 0; for (n.finish = !0, b.queue(this, e, []), i && i.cur && i.cur.finish && i.cur.finish.call(this), t = o.length; t--;) o[t].elem === this && o[t].queue === e && (o[t].anim.stop(!0), o.splice(t, 1)); for (t = 0; a > t; t++) r[t] && r[t].finish && r[t].finish.call(this); delete n.finish }) } }); function ir(e, t) { var n, r = { height: e }, i = 0; for (t = t ? 1 : 0; 4 > i; i += 2 - t) n = Zt[i], r["margin" + n] = r["padding" + n] = e; return t && (r.opacity = r.width = e), r } b.each({ slideDown: ir("show"), slideUp: ir("hide"), slideToggle: ir("toggle"), fadeIn: { opacity: "show" }, fadeOut: { opacity: "hide" }, fadeToggle: { opacity: "toggle" } }, function (e, t) { b.fn[e] = function (e, n, r) { return this.animate(t, e, n, r) } }), b.speed = function (e, t, n) { var r = e && "object" == typeof e ? b.extend({}, e) : { complete: n || !n && t || b.isFunction(e) && e, duration: e, easing: n && t || t && !b.isFunction(t) && t }; return r.duration = b.fx.off ? 0 : "number" == typeof r.duration ? r.duration : r.duration in b.fx.speeds ? b.fx.speeds[r.duration] : b.fx.speeds._default, (null == r.queue || r.queue === !0) && (r.queue = "fx"), r.old = r.complete, r.complete = function () { b.isFunction(r.old) && r.old.call(this), r.queue && b.dequeue(this, r.queue) }, r }, b.easing = { linear: function (e) { return e }, swing: function (e) { return .5 - Math.cos(e * Math.PI) / 2 } }, b.timers = [], b.fx = rr.prototype.init, b.fx.tick = function () { var e, n = b.timers, r = 0; for (Xn = b.now() ; n.length > r; r++) e = n[r], e() || n[r] !== e || n.splice(r--, 1); n.length || b.fx.stop(), Xn = t }, b.fx.timer = function (e) { e() && b.timers.push(e) && b.fx.start() }, b.fx.interval = 13, b.fx.start = function () { Un || (Un = setInterval(b.fx.tick, b.fx.interval)) }, b.fx.stop = function () { clearInterval(Un), Un = null }, b.fx.speeds = { slow: 600, fast: 200, _default: 400 }, b.fx.step = {}, b.expr && b.expr.filters && (b.expr.filters.animated = function (e) { return b.grep(b.timers, function (t) { return e === t.elem }).length }), b.fn.offset = function (e) { if (arguments.length) return e === t ? this : this.each(function (t) { b.offset.setOffset(this, e, t) }); var n, r, o = { top: 0, left: 0 }, a = this[0], s = a && a.ownerDocument; if (s) return n = s.documentElement, b.contains(n, a) ? (typeof a.getBoundingClientRect !== i && (o = a.getBoundingClientRect()), r = or(s), { top: o.top + (r.pageYOffset || n.scrollTop) - (n.clientTop || 0), left: o.left + (r.pageXOffset || n.scrollLeft) - (n.clientLeft || 0) }) : o }, b.offset = { setOffset: function (e, t, n) { var r = b.css(e, "position"); "static" === r && (e.style.position = "relative"); var i = b(e), o = i.offset(), a = b.css(e, "top"), s = b.css(e, "left"), u = ("absolute" === r || "fixed" === r) && b.inArray("auto", [a, s]) > -1, l = {}, c = {}, p, f; u ? (c = i.position(), p = c.top, f = c.left) : (p = parseFloat(a) || 0, f = parseFloat(s) || 0), b.isFunction(t) && (t = t.call(e, n, o)), null != t.top && (l.top = t.top - o.top + p), null != t.left && (l.left = t.left - o.left + f), "using" in t ? t.using.call(e, l) : i.css(l) } }, b.fn.extend({ position: function () { if (this[0]) { var e, t, n = { top: 0, left: 0 }, r = this[0]; return "fixed" === b.css(r, "position") ? t = r.getBoundingClientRect() : (e = this.offsetParent(), t = this.offset(), b.nodeName(e[0], "html") || (n = e.offset()), n.top += b.css(e[0], "borderTopWidth", !0), n.left += b.css(e[0], "borderLeftWidth", !0)), { top: t.top - n.top - b.css(r, "marginTop", !0), left: t.left - n.left - b.css(r, "marginLeft", !0) } } }, offsetParent: function () { return this.map(function () { var e = this.offsetParent || o.documentElement; while (e && !b.nodeName(e, "html") && "static" === b.css(e, "position")) e = e.offsetParent; return e || o.documentElement }) } }), b.each({ scrollLeft: "pageXOffset", scrollTop: "pageYOffset" }, function (e, n) { var r = /Y/.test(n); b.fn[e] = function (i) { return b.access(this, function (e, i, o) { var a = or(e); return o === t ? a ? n in a ? a[n] : a.document.documentElement[i] : e[i] : (a ? a.scrollTo(r ? b(a).scrollLeft() : o, r ? o : b(a).scrollTop()) : e[i] = o, t) }, e, i, arguments.length, null) } }); function or(e) { return b.isWindow(e) ? e : 9 === e.nodeType ? e.defaultView || e.parentWindow : !1 } b.each({ Height: "height", Width: "width" }, function (e, n) { b.each({ padding: "inner" + e, content: n, "": "outer" + e }, function (r, i) { b.fn[i] = function (i, o) { var a = arguments.length && (r || "boolean" != typeof i), s = r || (i === !0 || o === !0 ? "margin" : "border"); return b.access(this, function (n, r, i) { var o; return b.isWindow(n) ? n.document.documentElement["client" + e] : 9 === n.nodeType ? (o = n.documentElement, Math.max(n.body["scroll" + e], o["scroll" + e], n.body["offset" + e], o["offset" + e], o["client" + e])) : i === t ? b.css(n, r, s) : b.style(n, r, i, s) }, n, a ? i : t, a, null) } }) }), e.jQuery = e.$ = b, "function" == typeof define && define.amd && define.amd.jQuery && define("jquery", [], function () { return b })
})(window);



(function (jQuery) {

    if (jQuery.browser) return;

    jQuery.browser = {};
    jQuery.browser.mozilla = false;
    jQuery.browser.webkit = false;
    jQuery.browser.opera = false;
    jQuery.browser.msie = false;

    var nAgt = navigator.userAgent;
    jQuery.browser.name = navigator.appName;
    jQuery.browser.fullVersion = '' + parseFloat(navigator.appVersion);
    jQuery.browser.majorVersion = parseInt(navigator.appVersion, 10);
    var nameOffset, verOffset, ix;

    // In Opera, the true version is after "Opera" or after "Version"
    if ((verOffset = nAgt.indexOf("Opera")) != -1) {
        jQuery.browser.opera = true;
        jQuery.browser.name = "Opera";
        jQuery.browser.fullVersion = nAgt.substring(verOffset + 6);
        if ((verOffset = nAgt.indexOf("Version")) != -1)
            jQuery.browser.fullVersion = nAgt.substring(verOffset + 8);
    }
        // In MSIE, the true version is after "MSIE" in userAgent
    else if ((verOffset = nAgt.indexOf("MSIE")) != -1) {
        jQuery.browser.msie = true;
        jQuery.browser.name = "Microsoft Internet Explorer";
        jQuery.browser.fullVersion = nAgt.substring(verOffset + 5);
    }
        // In Chrome, the true version is after "Chrome"
    else if ((verOffset = nAgt.indexOf("Chrome")) != -1) {
        jQuery.browser.webkit = true;
        jQuery.browser.name = "Chrome";
        jQuery.browser.fullVersion = nAgt.substring(verOffset + 7);
    }
        // In Safari, the true version is after "Safari" or after "Version"
    else if ((verOffset = nAgt.indexOf("Safari")) != -1) {
        jQuery.browser.webkit = true;
        jQuery.browser.name = "Safari";
        jQuery.browser.fullVersion = nAgt.substring(verOffset + 7);
        if ((verOffset = nAgt.indexOf("Version")) != -1)
            jQuery.browser.fullVersion = nAgt.substring(verOffset + 8);
    }
        // In Firefox, the true version is after "Firefox"
    else if ((verOffset = nAgt.indexOf("Firefox")) != -1) {
        jQuery.browser.mozilla = true;
        jQuery.browser.name = "Firefox";
        jQuery.browser.fullVersion = nAgt.substring(verOffset + 8);
    }
        // In most other browsers, "name/version" is at the end of userAgent
    else if ((nameOffset = nAgt.lastIndexOf(' ') + 1) <
    (verOffset = nAgt.lastIndexOf('/'))) {
        jQuery.browser.name = nAgt.substring(nameOffset, verOffset);
        jQuery.browser.fullVersion = nAgt.substring(verOffset + 1);
        if (jQuery.browser.name.toLowerCase() == jQuery.browser.name.toUpperCase()) {
            jQuery.browser.name = navigator.appName;
        }
    }
    // trim the fullVersion string at semicolon/space if present
    if ((ix = jQuery.browser.fullVersion.indexOf(";")) != -1)
        jQuery.browser.fullVersion = jQuery.browser.fullVersion.substring(0, ix);
    if ((ix = jQuery.browser.fullVersion.indexOf(" ")) != -1)
        jQuery.browser.fullVersion = jQuery.browser.fullVersion.substring(0, ix);

    jQuery.browser.majorVersion = parseInt('' + jQuery.browser.fullVersion, 10);
    if (isNaN(jQuery.browser.majorVersion)) {
        jQuery.browser.fullVersion = '' + parseFloat(navigator.appVersion);
        jQuery.browser.majorVersion = parseInt(navigator.appVersion, 10);
    }
    jQuery.browser.version = jQuery.browser.majorVersion;
})(jQuery);








;/*
 *  Bootstrap TouchSpin - v3.0.1
 *  A mobile and touch friendly input spinner component for Bootstrap 3.
 *  http://www.virtuosoft.eu/code/bootstrap-touchspin/
 *
 *  Made by István Ujj-Mészáros
 *  Under Apache License v2.0 License
 */
!function (a) { "use strict"; function b(a, b) { return a + ".touchspin_" + b } function c(c, d) { return a.map(c, function (a) { return b(a, d) }) } var d = 0; a.fn.TouchSpin = function (b) { if ("destroy" === b) return void this.each(function () { var b = a(this), d = b.data(); a(document).off(c(["mouseup", "touchend", "touchcancel", "mousemove", "touchmove", "scroll", "scrollstart"], d.spinnerid).join(" ")) }); var e = { min: 0, max: 100, initval: "", step: 1, decimals: 0, stepinterval: 100, forcestepdivisibility: "round", stepintervaldelay: 500, verticalbuttons: !1, verticalupclass: "glyphicon glyphicon-chevron-up", verticaldownclass: "glyphicon glyphicon-chevron-down", prefix: "", postfix: "", prefix_extraclass: "", postfix_extraclass: "", booster: !0, boostat: 10, maxboostedstep: !1, mousewheel: !0, buttondown_class: "btn btn-default", buttonup_class: "btn btn-default" }, f = { min: "min", max: "max", initval: "init-val", step: "step", decimals: "decimals", stepinterval: "step-interval", verticalbuttons: "vertical-buttons", verticalupclass: "vertical-up-class", verticaldownclass: "vertical-down-class", forcestepdivisibility: "force-step-divisibility", stepintervaldelay: "step-interval-delay", prefix: "prefix", postfix: "postfix", prefix_extraclass: "prefix-extra-class", postfix_extraclass: "postfix-extra-class", booster: "booster", boostat: "boostat", maxboostedstep: "max-boosted-step", mousewheel: "mouse-wheel", buttondown_class: "button-down-class", buttonup_class: "button-up-class" }; return this.each(function () { function g() { if (!J.data("alreadyinitialized")) { if (J.data("alreadyinitialized", !0), d += 1, J.data("spinnerid", d), !J.is("input")) return void console.log("Must be an input."); j(), h(), u(), m(), p(), q(), r(), s(), D.input.css("display", "block") } } function h() { "" !== B.initval && "" === J.val() && J.val(B.initval) } function i(a) { l(a), u(); var b = D.input.val(); "" !== b && (b = Number(D.input.val()), D.input.val(b.toFixed(B.decimals))) } function j() { B = a.extend({}, e, K, k(), b) } function k() { var b = {}; return a.each(f, function (a, c) { var d = "bts-" + c; J.is("[data-" + d + "]") && (b[a] = J.data(d)) }), b } function l(b) { B = a.extend({}, B, b) } function m() { var a = J.val(), b = J.parent(); "" !== a && (a = Number(a).toFixed(B.decimals)), J.data("initvalue", a).val(a), J.addClass("form-control"), b.hasClass("input-group") ? n(b) : o() } function n(b) { b.addClass("bootstrap-touchspin"); var c, d, e = J.prev(), f = J.next(), g = '<span class="input-group-addon bootstrap-touchspin-prefix">' + B.prefix + "</span>", h = '<span class="input-group-addon bootstrap-touchspin-postfix">' + B.postfix + "</span>"; e.hasClass("input-group-btn") ? (c = '<button class="' + B.buttondown_class + ' bootstrap-touchspin-down" type="button">-</button>', e.append(c)) : (c = '<span class="input-group-btn"><button class="' + B.buttondown_class + ' bootstrap-touchspin-down" type="button">-</button></span>', a(c).insertBefore(J)), f.hasClass("input-group-btn") ? (d = '<button class="' + B.buttonup_class + ' bootstrap-touchspin-up" type="button">+</button>', f.prepend(d)) : (d = '<span class="input-group-btn"><button class="' + B.buttonup_class + ' bootstrap-touchspin-up" type="button">+</button></span>', a(d).insertAfter(J)), a(g).insertBefore(J), a(h).insertAfter(J), C = b } function o() { var b; b = B.verticalbuttons ? '<div class="input-group bootstrap-touchspin"><span class="input-group-addon bootstrap-touchspin-prefix">' + B.prefix + '</span><span class="input-group-addon bootstrap-touchspin-postfix">' + B.postfix + '</span><span class="input-group-btn-vertical"><button class="' + B.buttondown_class + ' bootstrap-touchspin-up" type="button"><i class="' + B.verticalupclass + '"></i></button><button class="' + B.buttonup_class + ' bootstrap-touchspin-down" type="button"><i class="' + B.verticaldownclass + '"></i></button></span></div>' : '<div class="input-group bootstrap-touchspin"><span class="input-group-btn"><button class="' + B.buttondown_class + ' bootstrap-touchspin-down" type="button">-</button></span><span class="input-group-addon bootstrap-touchspin-prefix">' + B.prefix + '</span><span class="input-group-addon bootstrap-touchspin-postfix">' + B.postfix + '</span><span class="input-group-btn"><button class="' + B.buttonup_class + ' bootstrap-touchspin-up" type="button">+</button></span></div>', C = a(b).insertBefore(J), a(".bootstrap-touchspin-prefix", C).after(J), J.hasClass("input-sm") ? C.addClass("input-group-sm") : J.hasClass("input-lg") && C.addClass("input-group-lg") } function p() { D = { down: a(".bootstrap-touchspin-down", C), up: a(".bootstrap-touchspin-up", C), input: a("input", C), prefix: a(".bootstrap-touchspin-prefix", C).addClass(B.prefix_extraclass), postfix: a(".bootstrap-touchspin-postfix", C).addClass(B.postfix_extraclass) } } function q() { "" === B.prefix && D.prefix.hide(), "" === B.postfix && D.postfix.hide() } function r() { J.on("keydown", function (a) { var b = a.keyCode || a.which; 38 === b ? ("up" !== M && (w(), z()), a.preventDefault()) : 40 === b && ("down" !== M && (x(), y()), a.preventDefault()) }), J.on("keyup", function (a) { var b = a.keyCode || a.which; 38 === b ? A() : 40 === b && A() }), J.on("blur", function () { u() }), D.down.on("keydown", function (a) { var b = a.keyCode || a.which; (32 === b || 13 === b) && ("down" !== M && (x(), y()), a.preventDefault()) }), D.down.on("keyup", function (a) { var b = a.keyCode || a.which; (32 === b || 13 === b) && A() }), D.up.on("keydown", function (a) { var b = a.keyCode || a.which; (32 === b || 13 === b) && ("up" !== M && (w(), z()), a.preventDefault()) }), D.up.on("keyup", function (a) { var b = a.keyCode || a.which; (32 === b || 13 === b) && A() }), D.down.on("mousedown.touchspin", function (a) { D.down.off("touchstart.touchspin"), J.is(":disabled") || (x(), y(), a.preventDefault(), a.stopPropagation()) }), D.down.on("touchstart.touchspin", function (a) { D.down.off("mousedown.touchspin"), J.is(":disabled") || (x(), y(), a.preventDefault(), a.stopPropagation()) }), D.up.on("mousedown.touchspin", function (a) { D.up.off("touchstart.touchspin"), J.is(":disabled") || (w(), z(), a.preventDefault(), a.stopPropagation()) }), D.up.on("touchstart.touchspin", function (a) { D.up.off("mousedown.touchspin"), J.is(":disabled") || (w(), z(), a.preventDefault(), a.stopPropagation()) }), D.up.on("mouseout touchleave touchend touchcancel", function (a) { M && (a.stopPropagation(), A()) }), D.down.on("mouseout touchleave touchend touchcancel", function (a) { M && (a.stopPropagation(), A()) }), D.down.on("mousemove touchmove", function (a) { M && (a.stopPropagation(), a.preventDefault()) }), D.up.on("mousemove touchmove", function (a) { M && (a.stopPropagation(), a.preventDefault()) }), a(document).on(c(["mouseup", "touchend", "touchcancel"], d).join(" "), function (a) { M && (a.preventDefault(), A()) }), a(document).on(c(["mousemove", "touchmove", "scroll", "scrollstart"], d).join(" "), function (a) { M && (a.preventDefault(), A()) }), J.on("mousewheel DOMMouseScroll", function (a) { if (B.mousewheel && J.is(":focus")) { var b = a.originalEvent.wheelDelta || -a.originalEvent.deltaY || -a.originalEvent.detail; a.stopPropagation(), a.preventDefault(), 0 > b ? x() : w() } }) } function s() { J.on("touchspin.uponce", function () { A(), w() }), J.on("touchspin.downonce", function () { A(), x() }), J.on("touchspin.startupspin", function () { z() }), J.on("touchspin.startdownspin", function () { y() }), J.on("touchspin.stopspin", function () { A() }), J.on("touchspin.updatesettings", function (a, b) { i(b) }) } function t(a) { switch (B.forcestepdivisibility) { case "round": return (Math.round(a / B.step) * B.step).toFixed(B.decimals); case "floor": return (Math.floor(a / B.step) * B.step).toFixed(B.decimals); case "ceil": return (Math.ceil(a / B.step) * B.step).toFixed(B.decimals); default: return a } } function u() { var a, b, c; a = J.val(), "" !== a && (B.decimals > 0 && "." === a || (b = parseFloat(a), isNaN(b) && (b = 0), c = b, b.toString() !== a && (c = b), b < B.min && (c = B.min), b > B.max && (c = B.max), c = t(c), Number(a).toString() !== c.toString() && (J.val(c), J.trigger("change")))) } function v() { if (B.booster) { var a = Math.pow(2, Math.floor(L / B.boostat)) * B.step; return B.maxboostedstep && a > B.maxboostedstep && (a = B.maxboostedstep, E = Math.round(E / a) * a), Math.max(B.step, a) } return B.step } function w() { u(), E = parseFloat(D.input.val()), isNaN(E) && (E = 0); var a = E, b = v(); E += b, E > B.max && (E = B.max, J.trigger("touchspin.on.max"), A()), D.input.val(Number(E).toFixed(B.decimals)), a !== E && J.trigger("change") } function x() { u(), E = parseFloat(D.input.val()), isNaN(E) && (E = 0); var a = E, b = v(); E -= b, E < B.min && (E = B.min, J.trigger("touchspin.on.min"), A()), D.input.val(E.toFixed(B.decimals)), a !== E && J.trigger("change") } function y() { A(), L = 0, M = "down", J.trigger("touchspin.on.startspin"), J.trigger("touchspin.on.startdownspin"), H = setTimeout(function () { F = setInterval(function () { L++, x() }, B.stepinterval) }, B.stepintervaldelay) } function z() { A(), L = 0, M = "up", J.trigger("touchspin.on.startspin"), J.trigger("touchspin.on.startupspin"), I = setTimeout(function () { G = setInterval(function () { L++, w() }, B.stepinterval) }, B.stepintervaldelay) } function A() { switch (clearTimeout(H), clearTimeout(I), clearInterval(F), clearInterval(G), M) { case "up": J.trigger("touchspin.on.stopupspin"), J.trigger("touchspin.on.stopspin"); break; case "down": J.trigger("touchspin.on.stopdownspin"), J.trigger("touchspin.on.stopspin") } L = 0, M = !1 } var B, C, D, E, F, G, H, I, J = a(this), K = J.data(), L = 0, M = !1; g() }) } }(jQuery);
;/*!
 * Bootstrap-select v1.9.3 (http://silviomoreto.github.io/bootstrap-select)
 *
 * Copyright 2013-2015 bootstrap-select
 * Licensed under MIT (https://github.com/silviomoreto/bootstrap-select/blob/master/LICENSE)
 */
(function (root, factory) { if (typeof define === 'function' && define.amd) { define(["jquery"], function (a0) { return (factory(a0)); }); } else if (typeof exports === 'object') { module.exports = factory(require("jquery")); } else { factory(jQuery); } }(this, function (jQuery) {
    (function ($) {
        'use strict'; if (!String.prototype.includes) {
            (function () {
                'use strict'; var toString = {}.toString; var defineProperty = (function () {
                    try { var object = {}; var $defineProperty = Object.defineProperty; var result = $defineProperty(object, object, object) && $defineProperty; } catch (error) { }
                    return result;
                }()); var indexOf = ''.indexOf; var includes = function (search) {
                    if (this == null) { throw new TypeError(); }
                    var string = String(this); if (search && toString.call(search) == '[object RegExp]') { throw new TypeError(); }
                    var stringLength = string.length; var searchString = String(search); var searchLength = searchString.length; var position = arguments.length > 1 ? arguments[1] : undefined; var pos = position ? Number(position) : 0; if (pos != pos) { pos = 0; }
                    var start = Math.min(Math.max(pos, 0), stringLength); if (searchLength + start > stringLength) { return false; }
                    return indexOf.call(string, searchString, pos) != -1;
                }; if (defineProperty) { defineProperty(String.prototype, 'includes', { 'value': includes, 'configurable': true, 'writable': true }); } else { String.prototype.includes = includes; }
            }());
        }
        if (!String.prototype.startsWith) {
            (function () {
                'use strict'; var defineProperty = (function () {
                    try { var object = {}; var $defineProperty = Object.defineProperty; var result = $defineProperty(object, object, object) && $defineProperty; } catch (error) { }
                    return result;
                }()); var toString = {}.toString; var startsWith = function (search) {
                    if (this == null) { throw new TypeError(); }
                    var string = String(this); if (search && toString.call(search) == '[object RegExp]') { throw new TypeError(); }
                    var stringLength = string.length; var searchString = String(search); var searchLength = searchString.length; var position = arguments.length > 1 ? arguments[1] : undefined; var pos = position ? Number(position) : 0; if (pos != pos) { pos = 0; }
                    var start = Math.min(Math.max(pos, 0), stringLength); if (searchLength + start > stringLength) { return false; }
                    var index = -1; while (++index < searchLength) { if (string.charCodeAt(start + index) != searchString.charCodeAt(index)) { return false; } }
                    return true;
                }; if (defineProperty) { defineProperty(String.prototype, 'startsWith', { 'value': startsWith, 'configurable': true, 'writable': true }); } else { String.prototype.startsWith = startsWith; }
            }());
        }
        if (!Object.keys) {
            Object.keys = function (o, k, r) {
                r = []; for (k in o)
                    r.hasOwnProperty.call(o, k) && r.push(k); return r;
            };
        }
        $.fn.triggerNative = function (eventName) {
            var el = this[0], event; if (el.dispatchEvent) {
                if (typeof Event === 'function') { event = new Event(eventName, { bubbles: true }); } else { event = document.createEvent('Event'); event.initEvent(eventName, true, false); }
                el.dispatchEvent(event);
            } else {
                if (el.fireEvent) { event = document.createEventObject(); event.eventType = eventName; el.fireEvent('on' + eventName, event); }
                this.trigger(eventName);
            }
        }; $.expr[':'].icontains = function (obj, index, meta) { var $obj = $(obj); var haystack = ($obj.data('tokens') || $obj.text()).toUpperCase(); return haystack.includes(meta[3].toUpperCase()); }; $.expr[':'].ibegins = function (obj, index, meta) { var $obj = $(obj); var haystack = ($obj.data('tokens') || $obj.text()).toUpperCase(); return haystack.startsWith(meta[3].toUpperCase()); }; $.expr[':'].aicontains = function (obj, index, meta) { var $obj = $(obj); var haystack = ($obj.data('tokens') || $obj.data('normalizedText') || $obj.text()).toUpperCase(); return haystack.includes(meta[3].toUpperCase()); }; $.expr[':'].aibegins = function (obj, index, meta) { var $obj = $(obj); var haystack = ($obj.data('tokens') || $obj.data('normalizedText') || $obj.text()).toUpperCase(); return haystack.startsWith(meta[3].toUpperCase()); }; function normalizeToBase(text) { var rExps = [{ re: /[\xC0-\xC6]/g, ch: "A" }, { re: /[\xE0-\xE6]/g, ch: "a" }, { re: /[\xC8-\xCB]/g, ch: "E" }, { re: /[\xE8-\xEB]/g, ch: "e" }, { re: /[\xCC-\xCF]/g, ch: "I" }, { re: /[\xEC-\xEF]/g, ch: "i" }, { re: /[\xD2-\xD6]/g, ch: "O" }, { re: /[\xF2-\xF6]/g, ch: "o" }, { re: /[\xD9-\xDC]/g, ch: "U" }, { re: /[\xF9-\xFC]/g, ch: "u" }, { re: /[\xC7-\xE7]/g, ch: "c" }, { re: /[\xD1]/g, ch: "N" }, { re: /[\xF1]/g, ch: "n" }]; $.each(rExps, function () { text = text.replace(this.re, this.ch); }); return text; }
        function htmlEscape(html) { var escapeMap = { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#x27;', '`': '&#x60;' }; var source = '(?:' + Object.keys(escapeMap).join('|') + ')', testRegexp = new RegExp(source), replaceRegexp = new RegExp(source, 'g'), string = html == null ? '' : '' + html; return testRegexp.test(string) ? string.replace(replaceRegexp, function (match) { return escapeMap[match]; }) : string; }
        var Selectpicker = function (element, options, e) {
            if (e) { e.stopPropagation(); e.preventDefault(); }
            this.$element = $(element); this.$newElement = null; this.$button = null; this.$menu = null; this.$lis = null; this.options = options; if (this.options.title === null) { this.options.title = this.$element.attr('title'); }
            this.val = Selectpicker.prototype.val; this.render = Selectpicker.prototype.render; this.refresh = Selectpicker.prototype.refresh; this.setStyle = Selectpicker.prototype.setStyle; this.selectAll = Selectpicker.prototype.selectAll; this.deselectAll = Selectpicker.prototype.deselectAll; this.destroy = Selectpicker.prototype.destroy; this.remove = Selectpicker.prototype.remove; this.show = Selectpicker.prototype.show; this.hide = Selectpicker.prototype.hide; this.init();
        }; Selectpicker.VERSION = '1.9.3'; Selectpicker.DEFAULTS = { noneSelectedText: 'Nothing selected', noneResultsText: 'No results matched {0}', countSelectedText: function (numSelected, numTotal) { return (numSelected == 1) ? "{0} item selected" : "{0} items selected"; }, maxOptionsText: function (numAll, numGroup) { return [(numAll == 1) ? 'Limit reached ({n} item max)' : 'Limit reached ({n} items max)', (numGroup == 1) ? 'Group limit reached ({n} item max)' : 'Group limit reached ({n} items max)']; }, selectAllText: 'Select All', deselectAllText: 'Deselect All', doneButton: false, doneButtonText: 'Close', multipleSeparator: ', ', styleBase: 'btn btn-sm', style: 'btn-default', size: 'auto', title: null, selectedTextFormat: 'values', width: false, container: false, hideDisabled: false, showSubtext: false, showIcon: true, showContent: true, dropupAuto: true, header: false, liveSearch: false, liveSearchPlaceholder: null, liveSearchNormalize: false, liveSearchStyle: 'contains', actionsBox: false, iconBase: 'glyphicon', tickIcon: 'glyphicon-ok', template: { caret: '<span class="caret"></span>' }, maxOptions: false, mobile: false, selectOnTab: false, dropdownAlignRight: false }; Selectpicker.prototype = {
            constructor: Selectpicker, init: function () {
                var that = this, id = this.$element.attr('id'); this.liObj = {}; this.multiple = this.$element.prop('multiple'); this.autofocus = this.$element.prop('autofocus'); this.$newElement = this.createView(); this.$element.after(this.$newElement).appendTo(this.$newElement); this.$button = this.$newElement.children('button'); this.$menu = this.$newElement.children('.dropdown-menu'); this.$menuInner = this.$menu.children('.inner'); this.$searchbox = this.$menu.find('input'); if (this.options.dropdownAlignRight)
                    this.$menu.addClass('dropdown-menu-right'); if (typeof id !== 'undefined') { this.$button.attr('data-id', id); $('label[for="' + id + '"]').click(function (e) { e.preventDefault(); that.$button.focus(); }); }
                this.checkDisabled(); this.clickListener(); if (this.options.liveSearch) this.liveSearchListener(); this.render(); this.setStyle(); this.setWidth(); if (this.options.container) this.selectPosition(); this.$menu.data('this', this); this.$newElement.data('this', this); if (this.options.mobile) this.mobile(); this.$newElement.on({ 'hide.bs.dropdown': function (e) { that.$element.trigger('hide.bs.select', e); }, 'hidden.bs.dropdown': function (e) { that.$element.trigger('hidden.bs.select', e); }, 'show.bs.dropdown': function (e) { that.$element.trigger('show.bs.select', e); }, 'shown.bs.dropdown': function (e) { that.$element.trigger('shown.bs.select', e); } }); if (that.$element[0].hasAttribute('required')) { this.$element.on('invalid', function () { that.$button.addClass('bs-invalid').focus(); that.$element.on({ 'focus.bs.select': function () { that.$button.focus(); that.$element.off('focus.bs.select'); }, 'shown.bs.select': function () { that.$element.val(that.$element.val()).off('shown.bs.select'); }, 'rendered.bs.select': function () { if (this.validity.valid) that.$button.removeClass('bs-invalid'); that.$element.off('rendered.bs.select'); } }); }); }
                setTimeout(function () { that.$element.trigger('loaded.bs.select'); });
            }, createDropdown: function () {
                var multiple = this.multiple ? ' show-tick' : '', inputGroup = this.$element.parent().hasClass('input-group') ? ' input-group-btn' : '', autofocus = this.autofocus ? ' autofocus' : ''; var header = this.options.header ? '<div class="popover-title"><button type="button" class="close" aria-hidden="true">&times;</button>' + this.options.header + '</div>' : ''; var searchbox = this.options.liveSearch ? '<div class="bs-searchbox">' + '<input type="text" class="form-control" autocomplete="off"' +
                (null === this.options.liveSearchPlaceholder ? '' : ' placeholder="' + htmlEscape(this.options.liveSearchPlaceholder) + '"') + '>' + '</div>' : ''; var actionsbox = this.multiple && this.options.actionsBox ? '<div class="bs-actionsbox">' + '<div class="btn-group btn-group-sm btn-block">' + '<button type="button" class="actions-btn bs-select-all btn btn-default">' +
                this.options.selectAllText + '</button>' + '<button type="button" class="actions-btn bs-deselect-all btn btn-default">' +
                this.options.deselectAllText + '</button>' + '</div>' + '</div>' : ''; var donebutton = this.multiple && this.options.doneButton ? '<div class="bs-donebutton">' + '<div class="btn-group btn-block">' + '<button type="button" class="btn btn-sm btn-default">' +
                this.options.doneButtonText + '</button>' + '</div>' + '</div>' : ''; var drop = '<div class="btn-group bootstrap-select user-input ' + multiple + inputGroup + '">' + '<button type="button" class="' + this.options.styleBase + ' dropdown-toggle" data-toggle="dropdown"' + autofocus + '>' + '<span class="filter-option pull-left d-i-b cut-out"></span>&nbsp;' + '<span class="bs-caret">' +
                this.options.template.caret + '</span>' + '</button>' + '<div class="dropdown-menu open">' +
                header +
                searchbox +
                actionsbox + '<ul class="dropdown-menu inner" role="menu">' + '</ul>' +
                donebutton + '</div>' + '</div>'; return $(drop);
            }, createView: function () { var $drop = this.createDropdown(), li = this.createLi(); $drop.find('ul')[0].innerHTML = li; return $drop; }, reloadLi: function () { this.destroyLi(); var li = this.createLi(); this.$menuInner[0].innerHTML = li; }, destroyLi: function () { this.$menu.find('li').remove(); }, createLi: function () {
                var that = this, _li = [], optID = 0, titleOption = document.createElement('option'), liIndex = -1; var generateLI = function (content, index, classes, optgroup) {
                    return '<li' +
                    ((typeof classes !== 'undefined' & '' !== classes) ? ' class="' + classes + '"' : '') +
                    ((typeof index !== 'undefined' & null !== index) ? ' data-original-index="' + index + '"' : '') +
                    ((typeof optgroup !== 'undefined' & null !== optgroup) ? 'data-optgroup="' + optgroup + '"' : '') + '>' + content + '</li>';
                }; var generateA = function (text, classes, inline, tokens) {
                    return '<a tabindex="0"' +
                    (typeof classes !== 'undefined' ? ' class="' + classes + '"' : '') +
                    (typeof inline !== 'undefined' ? ' style="' + inline + '"' : '') +
                    (that.options.liveSearchNormalize ? ' data-normalized-text="' + normalizeToBase(htmlEscape(text)) + '"' : '') +
                    (typeof tokens !== 'undefined' || tokens !== null ? ' data-tokens="' + tokens + '"' : '') + '>' + text + '<span class="' + that.options.iconBase + ' ' + that.options.tickIcon + ' check-mark"></span>' + '</a>';
                }; if (this.options.title && !this.multiple) { liIndex--; if (!this.$element.find('.bs-title-option').length) { var element = this.$element[0]; titleOption.className = 'bs-title-option'; titleOption.appendChild(document.createTextNode(this.options.title)); titleOption.value = ''; element.insertBefore(titleOption, element.firstChild); if ($(element.options[element.selectedIndex]).attr('selected') === undefined) titleOption.selected = true; } }
                this.$element.find('option').each(function (index) {
                    var $this = $(this); liIndex++; if ($this.hasClass('bs-title-option')) return; var optionClass = this.className || '', inline = this.style.cssText, text = $this.data('content') ? $this.data('content') : $this.html(), tokens = $this.data('tokens') ? $this.data('tokens') : null, subtext = typeof $this.data('subtext') !== 'undefined' ? '<small class="text-muted">' + $this.data('subtext') + '</small>' : '', icon = typeof $this.data('icon') !== 'undefined' ? '<span class="' + that.options.iconBase + ' ' + $this.data('icon') + '"></span> ' : '', isDisabled = this.disabled || (this.parentNode.tagName === 'OPTGROUP' && this.parentNode.disabled); if (icon !== '' && isDisabled) { icon = '<span>' + icon + '</span>'; }
                    if (that.options.hideDisabled && isDisabled) { liIndex--; return; }
                    if (!$this.data('content')) { text = icon + '<span class="text">' + text + subtext + '</span>'; }
                    if (this.parentNode.tagName === 'OPTGROUP' && $this.data('divider') !== true) {
                        var optGroupClass = ' ' + this.parentNode.className || ''; if ($this.index() === 0) {
                            optID += 1; var label = this.parentNode.label, labelSubtext = typeof $this.parent().data('subtext') !== 'undefined' ? '<small class="text-muted">' + $this.parent().data('subtext') + '</small>' : '', labelIcon = $this.parent().data('icon') ? '<span class="' + that.options.iconBase + ' ' + $this.parent().data('icon') + '"></span> ' : ''; label = labelIcon + '<span class="text">' + label + labelSubtext + '</span>'; if (index !== 0 && _li.length > 0) { liIndex++; _li.push(generateLI('', null, 'divider', optID + 'div')); }
                            liIndex++; _li.push(generateLI(label, null, 'dropdown-header' + optGroupClass, optID));
                        }
                        _li.push(generateLI(generateA(text, 'opt ' + optionClass + optGroupClass, inline, tokens), index, '', optID));
                    } else if ($this.data('divider') === true) { _li.push(generateLI('', index, 'divider')); } else if ($this.data('hidden') === true) { _li.push(generateLI(generateA(text, optionClass, inline, tokens), index, 'hidden is-hidden')); } else {
                        if (this.previousElementSibling && this.previousElementSibling.tagName === 'OPTGROUP') { liIndex++; _li.push(generateLI('', null, 'divider', optID + 'div')); }
                        _li.push(generateLI(generateA(text, optionClass, inline, tokens), index));
                    }
                    that.liObj[index] = liIndex;
                }); if (!this.multiple && this.$element.find('option:selected').length === 0 && !this.options.title) { this.$element.find('option').eq(0).prop('selected', true).attr('selected', 'selected'); }
                return _li.join('');
            }, findLis: function () { if (this.$lis == null) this.$lis = this.$menu.find('li'); return this.$lis; }, render: function (updateLi) {
                var that = this, notDisabled; if (updateLi !== false) { this.$element.find('option').each(function (index) { var $lis = that.findLis().eq(that.liObj[index]); that.setDisabled(index, this.disabled || this.parentNode.tagName === 'OPTGROUP' && this.parentNode.disabled, $lis); that.setSelected(index, this.selected, $lis); }); }
                this.tabIndex(); var selectedItems = this.$element.find('option').map(function () {
                    if (this.selected) {
                        if (that.options.hideDisabled && (this.disabled || this.parentNode.tagName === 'OPTGROUP' && this.parentNode.disabled)) return; var $this = $(this), icon = $this.data('icon') && that.options.showIcon ? '<i class="' + that.options.iconBase + ' ' + $this.data('icon') + '"></i> ' : '', subtext; if (that.options.showSubtext && $this.data('subtext') && !that.multiple) { subtext = ' <small class="text-muted">' + $this.data('subtext') + '</small>'; } else { subtext = ''; }
                        if (typeof $this.attr('title') !== 'undefined') { return $this.attr('title'); } else if ($this.data('content') && that.options.showContent) { return $this.data('content'); } else { return icon + $this.html() + subtext; }
                    }
                }).toArray(); var title = !this.multiple ? selectedItems[0] : selectedItems.join(this.options.multipleSeparator); if (this.multiple && this.options.selectedTextFormat.indexOf('count') > -1) { var max = this.options.selectedTextFormat.split('>'); if ((max.length > 1 && selectedItems.length > max[1]) || (max.length == 1 && selectedItems.length >= 2)) { notDisabled = this.options.hideDisabled ? ', [disabled]' : ''; var totalCount = this.$element.find('option').not('[data-divider="true"], [data-hidden="true"]' + notDisabled).length, tr8nText = (typeof this.options.countSelectedText === 'function') ? this.options.countSelectedText(selectedItems.length, totalCount) : this.options.countSelectedText; title = tr8nText.replace('{0}', selectedItems.length.toString()).replace('{1}', totalCount.toString()); } }
                if (this.options.title == undefined) { this.options.title = this.$element.attr('title'); }
                if (this.options.selectedTextFormat == 'static') { title = this.options.title; }
                if (!title) { title = typeof this.options.title !== 'undefined' ? this.options.title : this.options.noneSelectedText; }
                this.$button.children('.filter-option').html(title); this.$element.trigger('rendered.bs.select');
            }, setStyle: function (style, status) {
                if (this.$element.attr('class')) { this.$newElement.addClass(this.$element.attr('class').replace(/selectpicker|mobile-device|bs-select-hidden|validate\[.*\]/gi, '')); }
                if (this.$element.attr('style')) { this.$newElement.attr('style', this.$element.attr('style')); }
                if (this.$element.attr('onclick')) { this.$newElement.attr('onclick', this.$element.attr('onclick')); }
                if (this.$element.attr('onmouseleave')) { this.$newElement.attr('onmouseleave', this.$element.attr('onmouseleave')); }
                if (this.$element.attr('onmouseenter')) { this.$newElement.attr('onmouseenter', this.$element.attr('onmouseenter')); }
                var buttonClass = style ? style : this.options.style; if (status == 'add') { this.$button.addClass(buttonClass); } else if (status == 'remove') { this.$button.removeClass(buttonClass); } else { this.$button.removeClass(this.options.style); this.$button.addClass(buttonClass); }
            }, liHeight: function (refresh) {
                if (!refresh && (this.options.size === false || this.sizeInfo)) return; var newElement = document.createElement('div'), menu = document.createElement('div'), menuInner = document.createElement('ul'), divider = document.createElement('li'), li = document.createElement('li'), a = document.createElement('a'), text = document.createElement('span'), header = this.options.header && this.$menu.find('.popover-title').length > 0 ? this.$menu.find('.popover-title')[0].cloneNode(true) : null, search = this.options.liveSearch ? document.createElement('div') : null, actions = this.options.actionsBox && this.multiple && this.$menu.find('.bs-actionsbox').length > 0 ? this.$menu.find('.bs-actionsbox')[0].cloneNode(true) : null, doneButton = this.options.doneButton && this.multiple && this.$menu.find('.bs-donebutton').length > 0 ? this.$menu.find('.bs-donebutton')[0].cloneNode(true) : null; text.className = 'text'; newElement.className = this.$menu[0].parentNode.className + ' open'; menu.className = 'dropdown-menu open'; menuInner.className = 'dropdown-menu inner'; divider.className = 'divider'; text.appendChild(document.createTextNode('Inner text')); a.appendChild(text); li.appendChild(a); menuInner.appendChild(li); menuInner.appendChild(divider); if (header) menu.appendChild(header); if (search) { var input = document.createElement('span'); search.className = 'bs-searchbox'; input.className = 'form-control'; search.appendChild(input); menu.appendChild(search); }
                if (actions) menu.appendChild(actions); menu.appendChild(menuInner); if (doneButton) menu.appendChild(doneButton); newElement.appendChild(menu); document.body.appendChild(newElement); var liHeight = a.offsetHeight, headerHeight = header ? header.offsetHeight : 0, searchHeight = search ? search.offsetHeight : 0, actionsHeight = actions ? actions.offsetHeight : 0, doneButtonHeight = doneButton ? doneButton.offsetHeight : 0, dividerHeight = $(divider).outerHeight(true), menuStyle = typeof getComputedStyle === 'function' ? getComputedStyle(menu) : false, $menu = menuStyle ? null : $(menu), menuPadding = parseInt(menuStyle ? menuStyle.paddingTop : $menu.css('paddingTop')) +
                parseInt(menuStyle ? menuStyle.paddingBottom : $menu.css('paddingBottom')) +
                parseInt(menuStyle ? menuStyle.borderTopWidth : $menu.css('borderTopWidth')) +
                parseInt(menuStyle ? menuStyle.borderBottomWidth : $menu.css('borderBottomWidth')), menuExtras = menuPadding +
                parseInt(menuStyle ? menuStyle.marginTop : $menu.css('marginTop')) +
                parseInt(menuStyle ? menuStyle.marginBottom : $menu.css('marginBottom')) + 2; document.body.removeChild(newElement); this.sizeInfo = { liHeight: liHeight, headerHeight: headerHeight, searchHeight: searchHeight, actionsHeight: actionsHeight, doneButtonHeight: doneButtonHeight, dividerHeight: dividerHeight, menuPadding: menuPadding, menuExtras: menuExtras };
            }, setSize: function () {
                this.findLis(); this.liHeight(); if (this.options.header) this.$menu.css('padding-top', 0); if (this.options.size === false) return; var that = this, $menu = this.$menu, $menuInner = this.$menuInner, $window = $(window), selectHeight = this.$newElement[0].offsetHeight, liHeight = this.sizeInfo['liHeight'], headerHeight = this.sizeInfo['headerHeight'], searchHeight = this.sizeInfo['searchHeight'], actionsHeight = this.sizeInfo['actionsHeight'], doneButtonHeight = this.sizeInfo['doneButtonHeight'], divHeight = this.sizeInfo['dividerHeight'], menuPadding = this.sizeInfo['menuPadding'], menuExtras = this.sizeInfo['menuExtras'], notDisabled = this.options.hideDisabled ? '.disabled' : '', menuHeight, getHeight, selectOffsetTop, selectOffsetBot, posVert = function () { selectOffsetTop = that.$newElement.offset().top - $window.scrollTop(); selectOffsetBot = $window.height() - selectOffsetTop - selectHeight; }; posVert(); if (this.options.size === 'auto') {
                    var getSize = function () {
                        var minHeight, hasClass = function (className, include) { return function (element) { if (include) { return (element.classList ? element.classList.contains(className) : $(element).hasClass(className)); } else { return !(element.classList ? element.classList.contains(className) : $(element).hasClass(className)); } }; }, lis = that.$menuInner[0].getElementsByTagName('li'), lisVisible = Array.prototype.filter ? Array.prototype.filter.call(lis, hasClass('hidden', false)) : that.$lis.not('.hidden'), optGroup = Array.prototype.filter ? Array.prototype.filter.call(lisVisible, hasClass('dropdown-header', true)) : lisVisible.filter('.dropdown-header'); posVert(); menuHeight = selectOffsetBot - menuExtras; if (that.options.container) { if (!$menu.data('height')) $menu.data('height', $menu.height()); getHeight = $menu.data('height'); } else { getHeight = $menu.height(); }
                        if (that.options.dropupAuto) { that.$newElement.toggleClass('dropup', selectOffsetTop > selectOffsetBot && (menuHeight - menuExtras) < getHeight); }
                        if (that.$newElement.hasClass('dropup')) { menuHeight = selectOffsetTop - menuExtras; }
                        if ((lisVisible.length + optGroup.length) > 3) { minHeight = liHeight * 3 + menuExtras - 2; } else { minHeight = 0; }
                        $menu.css({ 'z-index': '999999', 'max-height': (menuHeight <= 260 ? menuHeight : 260) + 'px', 'overflow': 'hidden', 'min-height': minHeight + headerHeight + searchHeight + actionsHeight + doneButtonHeight + 'px' }); $menuInner.css({ 'max-height': (menuHeight <= 260 ? menuHeight : 260) - headerHeight - searchHeight - actionsHeight - doneButtonHeight - menuPadding + 'px', 'overflow-y': 'auto', 'min-height': Math.max(minHeight - menuPadding, 0) + 'px' });
                    }; getSize(); this.$searchbox.off('input.getSize propertychange.getSize').on('input.getSize propertychange.getSize', getSize); $window.off('resize.getSize scroll.getSize').on('resize.getSize scroll.getSize', getSize);
                } else if (this.options.size && this.options.size != 'auto' && this.$lis.not(notDisabled).length > this.options.size) {
                    var optIndex = this.$lis.not('.divider').not(notDisabled).children().slice(0, this.options.size).last().parent().index(), divLength = this.$lis.slice(0, optIndex + 1).filter('.divider').length; menuHeight = liHeight * this.options.size + divLength * divHeight + menuPadding; if (that.options.container) { if (!$menu.data('height')) $menu.data('height', $menu.height()); getHeight = $menu.data('height'); } else { getHeight = $menu.height(); }
                    if (that.options.dropupAuto) { this.$newElement.toggleClass('dropup', selectOffsetTop > selectOffsetBot && (menuHeight - menuExtras) < getHeight); }
                    $menu.css({ 'max-height': menuHeight + headerHeight + searchHeight + actionsHeight + doneButtonHeight + 'px', 'overflow': 'hidden', 'min-height': '' }); $menuInner.css({ 'max-height': menuHeight - menuPadding + 'px', 'overflow-y': 'auto', 'min-height': '' });
                }
            }, setWidth: function () {
                if (this.options.width === 'auto') { this.$menu.css('min-width', '0'); var $selectClone = this.$menu.parent().clone().appendTo('body'), $selectClone2 = this.options.container ? this.$newElement.clone().appendTo('body') : $selectClone, ulWidth = $selectClone.children('.dropdown-menu').outerWidth(), btnWidth = $selectClone2.css('width', 'auto').children('button').outerWidth(); $selectClone.remove(); $selectClone2.remove(); this.$newElement.css('width', Math.max(ulWidth, btnWidth) + 'px'); } else if (this.options.width === 'fit') { this.$menu.css('min-width', ''); this.$newElement.css('width', '').addClass('fit-width'); } else if (this.options.width) { this.$menu.css('min-width', ''); this.$newElement.css('width', this.options.width); } else { this.$menu.css('min-width', ''); }
                if (this.$newElement.hasClass('fit-width') && this.options.width !== 'fit') { this.$newElement.removeClass('fit-width'); }
            }, selectPosition: function () {
                this.$bsContainer = $('<div class="bs-container" />'); var that = this, pos, actualHeight, getPlacement = function ($element) { that.$bsContainer.addClass($element.attr('class').replace(/form-control|fit-width/gi, '')).toggleClass('dropup', $element.hasClass('dropup')); pos = $element.offset(); actualHeight = $element.hasClass('dropup') ? 0 : $element[0].offsetHeight; that.$bsContainer.css({ 'top': pos.top + actualHeight, 'left': pos.left, 'width': $element[0].offsetWidth }); }; this.$button.on('click', function () {
                    var $this = $(this); if (that.isDisabled()) { return; }
                    getPlacement(that.$newElement); that.$bsContainer.appendTo(that.options.container).toggleClass('open', !$this.hasClass('open')).append(that.$menu);
                }); $(window).on('resize scroll', function () { getPlacement(that.$newElement); }); this.$element.on('hide.bs.select', function () { that.$menu.data('height', that.$menu.height()); that.$bsContainer.detach(); });
            }, setSelected: function (index, selected, $lis) {
                if (!$lis) { $lis = this.findLis().eq(this.liObj[index]); }
                $lis.toggleClass('selected', selected);
            }, setDisabled: function (index, disabled, $lis) {
                if (!$lis) { $lis = this.findLis().eq(this.liObj[index]); }
                if (disabled) { $lis.addClass('disabled').children('a').attr('href', '#').attr('tabindex', -1); } else { $lis.removeClass('disabled').children('a').removeAttr('href').attr('tabindex', 0); }
            }, isDisabled: function () { return this.$element[0].disabled; }, checkDisabled: function () {
                var that = this; if (this.isDisabled()) { this.$newElement.addClass('disabled'); this.$button.addClass('disabled').attr('tabindex', -1); } else {
                    if (this.$button.hasClass('disabled')) { this.$newElement.removeClass('disabled'); this.$button.removeClass('disabled'); }
                    if (this.$button.attr('tabindex') == -1 && !this.$element.data('tabindex')) { this.$button.removeAttr('tabindex'); }
                }
                this.$button.click(function () { return !that.isDisabled(); });
            }, tabIndex: function () {
                if (this.$element.data('tabindex') !== this.$element.attr('tabindex') && (this.$element.attr('tabindex') !== -98 && this.$element.attr('tabindex') !== '-98')) { this.$element.data('tabindex', this.$element.attr('tabindex')); this.$button.attr('tabindex', this.$element.data('tabindex')); }
                this.$element.attr('tabindex', -98);
            }, clickListener: function () {
                var that = this, $document = $(document); this.$newElement.on('touchstart.dropdown', '.dropdown-menu', function (e) { e.stopPropagation(); }); $document.data('spaceSelect', false); this.$button.on('keyup', function (e) { if (/(32)/.test(e.keyCode.toString(10)) && $document.data('spaceSelect')) { e.preventDefault(); $document.data('spaceSelect', false); } }); this.$button.on('click', function () { that.setSize(); that.$element.on('shown.bs.select', function () { if (!that.options.liveSearch && !that.multiple) { that.$menuInner.find('.selected a').focus(); } else if (!that.multiple) { var selectedIndex = that.liObj[that.$element[0].selectedIndex]; if (typeof selectedIndex !== 'number' || that.options.size === false) return; var offset = that.$lis.eq(selectedIndex)[0].offsetTop - that.$menuInner[0].offsetTop; offset = offset - that.$menuInner[0].offsetHeight / 2 + that.sizeInfo.liHeight / 2; that.$menuInner[0].scrollTop = offset; } }); }); this.$menuInner.on('click', 'li a', function (e) {
                    var $this = $(this), clickedIndex = $this.parent().data('originalIndex'), prevValue = that.$element.val(), prevIndex = that.$element.prop('selectedIndex'); if (that.multiple) { e.stopPropagation(); }
                    e.preventDefault(); if (!that.isDisabled() && !$this.parent().hasClass('disabled')) {
                        var $options = that.$element.find('option'), $option = $options.eq(clickedIndex), state = $option.prop('selected'), $optgroup = $option.parent('optgroup'), maxOptions = that.options.maxOptions, maxOptionsGrp = $optgroup.data('maxOptions') || false; if (!that.multiple) { $options.prop('selected', false); $option.prop('selected', true); that.$menuInner.find('.selected').removeClass('selected'); that.setSelected(clickedIndex, true); } else {
                            $option.prop('selected', !state); that.setSelected(clickedIndex, !state); $this.blur(); if (maxOptions !== false || maxOptionsGrp !== false) {
                                var maxReached = maxOptions < $options.filter(':selected').length, maxReachedGrp = maxOptionsGrp < $optgroup.find('option:selected').length; if ((maxOptions && maxReached) || (maxOptionsGrp && maxReachedGrp)) {
                                    if (maxOptions && maxOptions == 1) { $options.prop('selected', false); $option.prop('selected', true); that.$menuInner.find('.selected').removeClass('selected'); that.setSelected(clickedIndex, true); } else if (maxOptionsGrp && maxOptionsGrp == 1) { $optgroup.find('option:selected').prop('selected', false); $option.prop('selected', true); var optgroupID = $this.parent().data('optgroup'); that.$menuInner.find('[data-optgroup="' + optgroupID + '"]').removeClass('selected'); that.setSelected(clickedIndex, true); } else {
                                        var maxOptionsArr = (typeof that.options.maxOptionsText === 'function') ? that.options.maxOptionsText(maxOptions, maxOptionsGrp) : that.options.maxOptionsText, maxTxt = maxOptionsArr[0].replace('{n}', maxOptions), maxTxtGrp = maxOptionsArr[1].replace('{n}', maxOptionsGrp), $notify = $('<div class="notify"></div>'); if (maxOptionsArr[2]) { maxTxt = maxTxt.replace('{var}', maxOptionsArr[2][maxOptions > 1 ? 0 : 1]); maxTxtGrp = maxTxtGrp.replace('{var}', maxOptionsArr[2][maxOptionsGrp > 1 ? 0 : 1]); }
                                        $option.prop('selected', false); that.$menu.append($notify); if (maxOptions && maxReached) { $notify.append($('<div>' + maxTxt + '</div>')); that.$element.trigger('maxReached.bs.select'); }
                                        if (maxOptionsGrp && maxReachedGrp) { $notify.append($('<div>' + maxTxtGrp + '</div>')); that.$element.trigger('maxReachedGrp.bs.select'); }
                                        setTimeout(function () { that.setSelected(clickedIndex, false); }, 10); $notify.delay(750).fadeOut(300, function () { $(this).remove(); });
                                    }
                                }
                            }
                        }
                        if (!that.multiple) { that.$button.focus(); } else if (that.options.liveSearch) { that.$searchbox.focus(); }
                        if ((prevValue != that.$element.val() && that.multiple) || (prevIndex != that.$element.prop('selectedIndex') && !that.multiple)) { that.$element.triggerNative('change'); that.$element.trigger('changed.bs.select', [clickedIndex, $option.prop('selected'), state]); }
                    }
                }); this.$menu.on('click', 'li.disabled a, .popover-title, .popover-title :not(.close)', function (e) { if (e.currentTarget == this) { e.preventDefault(); e.stopPropagation(); if (that.options.liveSearch && !$(e.target).hasClass('close')) { that.$searchbox.focus(); } else { that.$button.focus(); } } }); this.$menuInner.on('click', '.divider, .dropdown-header', function (e) { e.preventDefault(); e.stopPropagation(); if (that.options.liveSearch) { that.$searchbox.focus(); } else { that.$button.focus(); } }); this.$menu.on('click', '.popover-title .close', function () { that.$button.click(); }); this.$searchbox.on('click', function (e) { e.stopPropagation(); }); this.$menu.on('click', '.actions-btn', function (e) {
                    if (that.options.liveSearch) { that.$searchbox.focus(); } else { that.$button.focus(); }
                    e.preventDefault(); e.stopPropagation(); if ($(this).hasClass('bs-select-all')) { that.selectAll(); } else { that.deselectAll(); }
                    that.$element.triggerNative('change');
                }); this.$element.change(function () { that.render(false); });
            }, liveSearchListener: function () {
                var that = this, $no_results = $('<li class="no-results"></li>'); this.$button.on('click.dropdown.data-api touchstart.dropdown.data-api', function () {
                    that.$menuInner.find('.active').removeClass('active'); if (!!that.$searchbox.val()) { that.$searchbox.val(''); that.$lis.not('.is-hidden').removeClass('hidden'); if (!!$no_results.parent().length) $no_results.remove(); }
                    if (!that.multiple) that.$menuInner.find('.selected').addClass('active'); setTimeout(function () { that.$searchbox.focus(); }, 10);
                }); this.$searchbox.on('click.dropdown.data-api focus.dropdown.data-api touchend.dropdown.data-api', function (e) { e.stopPropagation(); }); this.$searchbox.on('input propertychange', function () {
                    if (that.$searchbox.val()) {
                        var $searchBase = that.$lis.not('.is-hidden').removeClass('hidden').children('a'); if (that.options.liveSearchNormalize) { $searchBase = $searchBase.not(':a' + that._searchStyle() + '("' + normalizeToBase(that.$searchbox.val()) + '")'); } else { $searchBase = $searchBase.not(':' + that._searchStyle() + '("' + that.$searchbox.val() + '")'); }
                        $searchBase.parent().addClass('hidden'); that.$lis.filter('.dropdown-header').each(function () { var $this = $(this), optgroup = $this.data('optgroup'); if (that.$lis.filter('[data-optgroup=' + optgroup + ']').not($this).not('.hidden').length === 0) { $this.addClass('hidden'); that.$lis.filter('[data-optgroup=' + optgroup + 'div]').addClass('hidden'); } }); var $lisVisible = that.$lis.not('.hidden'); $lisVisible.each(function (index) { var $this = $(this); if ($this.hasClass('divider') && ($this.index() === $lisVisible.first().index() || $this.index() === $lisVisible.last().index() || $lisVisible.eq(index + 1).hasClass('divider'))) { $this.addClass('hidden'); } }); if (!that.$lis.not('.hidden, .no-results').length) {
                            if (!!$no_results.parent().length) { $no_results.remove(); }
                            $no_results.html(that.options.noneResultsText.replace('{0}', '"' + htmlEscape(that.$searchbox.val()) + '"')).show(); that.$menuInner.append($no_results);
                        } else if (!!$no_results.parent().length) { $no_results.remove(); }
                    } else { that.$lis.not('.is-hidden').removeClass('hidden'); if (!!$no_results.parent().length) { $no_results.remove(); } }
                    that.$lis.filter('.active').removeClass('active'); if (that.$searchbox.val()) that.$lis.not('.hidden, .divider, .dropdown-header').eq(0).addClass('active').children('a').focus(); $(this).focus();
                });
            }, _searchStyle: function () { var styles = { begins: 'ibegins', startsWith: 'ibegins' }; return styles[this.options.liveSearchStyle] || 'icontains'; }, val: function (value) { if (typeof value !== 'undefined') { this.$element.val(value); this.render(); return this.$element; } else { return this.$element.val(); } }, changeAll: function (status) {
                if (typeof status === 'undefined') status = true; this.findLis(); var $options = this.$element.find('option'), $lisVisible = this.$lis.not('.divider, .dropdown-header, .disabled, .hidden').toggleClass('selected', status), lisVisLen = $lisVisible.length, selectedOptions = []; for (var i = 0; i < lisVisLen; i++) { var origIndex = $lisVisible[i].getAttribute('data-original-index'); selectedOptions[selectedOptions.length] = $options.eq(origIndex)[0]; }
                $(selectedOptions).prop('selected', status); this.render(false);
            },
            selectAll: function () { return this.changeAll(true); },
            deselectAll: function () { return this.changeAll(false); },
            keydown: function (e) {
                var $this = $(this),
                    $parent = $this.is('input') ? $this.parent().parent() : $this.parent(), $items,
                    that = $parent.data('this'), index, next, first, last, prev, nextPrev, prevIndex, isActive, selector = ':not(.disabled, .hidden, .dropdown-header, .divider)',
                    keyCodeMap = { 32: ' ', 48: '0', 49: '1', 50: '2', 51: '3', 52: '4', 53: '5', 54: '6', 55: '7', 56: '8', 57: '9', 59: ';', 65: 'a', 66: 'b', 67: 'c', 68: 'd', 69: 'e', 70: 'f', 71: 'g', 72: 'h', 73: 'i', 74: 'j', 75: 'k', 76: 'l', 77: 'm', 78: 'n', 79: 'o', 80: 'p', 81: 'q', 82: 'r', 83: 's', 84: 't', 85: 'u', 86: 'v', 87: 'w', 88: 'x', 89: 'y', 90: 'z', 96: '0', 97: '1', 98: '2', 99: '3', 100: '4', 101: '5', 102: '6', 103: '7', 104: '8', 105: '9' };
                if (that.options.liveSearch)
                    $parent = $this.parent().parent();
                if (that.options.container)
                    $parent = that.$menu;
                $items = $('[role=menu] li', $parent);
                isActive = that.$newElement.hasClass('open');
                if (!isActive && (e.keyCode >= 48 && e.keyCode <= 57 || e.keyCode >= 96 && e.keyCode <= 105 || e.keyCode >= 65 && e.keyCode <= 90)) {
                    if (!that.options.container) { that.setSize(); that.$menu.parent().addClass('open'); isActive = true; } else { that.$button.trigger('click'); }
                    that.$searchbox.focus();
                }
                if (that.options.liveSearch) {
                    if (/(^9$|27)/.test(e.keyCode.toString(10)) && isActive && that.$menu.find('.active').length === 0) { e.preventDefault(); that.$menu.parent().removeClass('open'); if (that.options.container) that.$newElement.removeClass('open'); that.$button.focus(); }
                    $items = $('[role=menu] li' + selector, $parent); if (!$this.val() && !/(38|40)/.test(e.keyCode.toString(10))) {
                        if ($items.filter('.active').length === 0) {
                            $items = that.$menuInner.find('li'); if (that.options.liveSearchNormalize) { $items = $items.filter(':a' + that._searchStyle() + '(' + normalizeToBase(keyCodeMap[e.keyCode]) + ')'); } else { $items = $items.filter(':' + that._searchStyle() + '(' + keyCodeMap[e.keyCode] + ')'); }
                        }
                    }
                }
                if (!$items.length) return; if (/(38|40)/.test(e.keyCode.toString(10))) {
                    index = $items.index($items.find('a').filter(':focus').parent()); first = $items.filter(selector).first().index(); last = $items.filter(selector).last().index(); next = $items.eq(index).nextAll(selector).eq(0).index(); prev = $items.eq(index).prevAll(selector).eq(0).index(); nextPrev = $items.eq(next).prevAll(selector).eq(0).index(); if (that.options.liveSearch) {
                        $items.each(function (i) { if (!$(this).hasClass('disabled')) { $(this).data('index', i); } }); index = $items.index($items.filter('.active')); first = $items.first().data('index'); last = $items.last().data('index'); next = $items.eq(index).nextAll().eq(0).data('index'); prev = $items.eq(index).prevAll().eq(0).data('index'); nextPrev = $items.eq(next).prevAll().eq(0).data('index');
                    }
                    prevIndex = $this.data('prevIndex'); if (e.keyCode == 38) { if (that.options.liveSearch) index--; if (index != nextPrev && index > prev) index = prev; if (index < first) index = first; if (index == prevIndex) index = last; } else if (e.keyCode == 40) { if (that.options.liveSearch) index++; if (index == -1) index = 0; if (index != nextPrev && index < next) index = next; if (index > last) index = last; if (index == prevIndex) index = first; }
                    $this.data('prevIndex', index); if (!that.options.liveSearch) { $items.eq(index).children('a').focus(); } else {
                        e.preventDefault(); if (!$this.hasClass('dropdown-toggle')) {
                            $items.removeClass('active').eq(index).addClass('active').children('a').focus(); $this.focus();
                        }
                    }
                } else if (!$this.is('input')) {
                    var keyIndex = [], count, prevKey; $items.each(function () { if (!$(this).hasClass('disabled')) { if ($.trim($(this).children('a').text().toLowerCase()).substring(0, 1) == keyCodeMap[e.keyCode]) { keyIndex.push($(this).index()); } } }); count = $(document).data('keycount'); count++; $(document).data('keycount', count); prevKey = $.trim($(':focus').text().toLowerCase()).substring(0, 1); if (prevKey != keyCodeMap[e.keyCode]) { count = 1; $(document).data('keycount', count); } else if (count >= keyIndex.length) { $(document).data('keycount', 0); if (count > keyIndex.length) count = 1; }
                    $items.eq(keyIndex[count - 1]).children('a').focus();
                }
                if ((/(13|32)/.test(e.keyCode.toString(10)) || (/(^9$)/.test(e.keyCode.toString(10)) && that.options.selectOnTab)) && isActive) {
                    if (!/(32)/.test(e.keyCode.toString(10))) e.preventDefault(); if (!that.options.liveSearch) { var elem = $(':focus'); elem.click(); elem.focus(); e.preventDefault(); $(document).data('spaceSelect', true); } else if (!/(32)/.test(e.keyCode.toString(10))) {
                        that.$menuInner.find('.active a').click(); $this.focus();
                    }
                    $(document).data('keycount', 0);
                }
                if ((/(^9$|27)/.test(e.keyCode.toString(10)) && isActive && (that.multiple || that.options.liveSearch)) || (/(27)/.test(e.keyCode.toString(10)) && !isActive)) {
                    that.$menu.parent().removeClass('open'); if (that.options.container) that.$newElement.removeClass('open'); that.$button.focus();
                }
            }, mobile: function () {
                this.$element.addClass('mobile-device');
            },
            refresh: function () {
                this.$lis = null;
                this.liObj = {};
                this.reloadLi();
                this.render();
                this.checkDisabled();
                this.liHeight(true);
                this.setStyle();
                this.setWidth();
                if (this.$lis)
                    this.$searchbox.trigger('propertychange'); this.$element.trigger('refreshed.bs.select');
            },
            hide: function () { this.$newElement.hide(); }, show: function () { this.$newElement.show(); }, remove: function () { this.$newElement.remove(); this.$element.remove(); }, destroy: function () {
                this.$newElement.remove(); if (this.$bsContainer) { this.$bsContainer.remove(); } else { this.$menu.remove(); }
                this.$element.off('.bs.select').removeData('selectpicker').removeClass('bs-select-hidden selectpicker');
            }
        }; function Plugin(option, event) {
            var args = arguments; var _option = option, _event = event;[].shift.apply(args); var value; var chain = this.each(function () {
                var $this = $(this); if ($this.is('select')) {
                    var data = $this.data('selectpicker'), options = typeof _option == 'object' && _option; if (!data) { var config = $.extend({}, Selectpicker.DEFAULTS, $.fn.selectpicker.defaults || {}, $this.data(), options); config.template = $.extend({}, Selectpicker.DEFAULTS.template, ($.fn.selectpicker.defaults ? $.fn.selectpicker.defaults.template : {}), $this.data().template, options.template); $this.data('selectpicker', (data = new Selectpicker(this, config, _event))); } else if (options) {
                        for (var i in options) {
                            if (options.hasOwnProperty(i)) {
                                data.options[i] = options[i];
                            }
                        }
                    }
                    if (typeof _option == 'string') { if (data[_option] instanceof Function) { value = data[_option].apply(data, args); } else { value = data.options[_option]; } }
                }
            }); if (typeof value !== 'undefined') { return value; } else { return chain; }
        }
        var old = $.fn.selectpicker; $.fn.selectpicker = Plugin; $.fn.selectpicker.Constructor = Selectpicker; $.fn.selectpicker.noConflict = function () { $.fn.selectpicker = old; return this; }; $(document).data('keycount', 0).on('keydown.bs.select', '.bootstrap-select [data-toggle=dropdown], .bootstrap-select [role="menu"], .bs-searchbox input', Selectpicker.prototype.keydown).on('focusin.modal', '.bootstrap-select [data-toggle=dropdown], .bootstrap-select [role="menu"], .bs-searchbox input', function (e) { e.stopPropagation(); }); $(window).on('load.bs.select.data-api', function () {
            $('.selectpicker').each(function () {
                var $selectpicker = $(this); Plugin.call($selectpicker, $selectpicker.data());
            })
        });
    })(jQuery);
}));
/*!
 * Bootstrap-select v1.9.3 (http://silviomoreto.github.io/bootstrap-select)
 *
 * Copyright 2013-2015 bootstrap-select
 * Licensed under MIT (https://github.com/silviomoreto/bootstrap-select/blob/master/LICENSE)
 */
(function (root, factory) { if (typeof define === 'function' && define.amd) { define(["jquery"], function (a0) { return (factory(a0)); }); } else if (typeof exports === 'object') { module.exports = factory(require("jquery")); } else { factory(jQuery); } }(this, function (jQuery) { (function ($) { $.fn.selectpicker.defaults = { noneSelectedText: '没有选中任何项', noneResultsText: '没有找到匹配项', countSelectedText: '选中{1}中的{0}项', maxOptionsText: ['超出限制 (最多选择{n}项)', '组选择超出限制(最多选择{n}组)'], multipleSeparator: ', ' }; })(jQuery); }));

; jQuery.cookie = function (name, value, options) {
    if (typeof value != 'undefined') {
        options = options || {}; if (value === null) {
            value = ''; options.expires = -1;
        }
        var expires = ''; if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
            var date; if (typeof options.expires == 'number') { date = new Date(); date.setTime(date.getTime() + (options.expires * 1000)); } else { date = options.expires; }
            expires = '; expires=' + date.toUTCString();
        }
        var path = options.path ? '; path=' + options.path : ''; var domain = options.domain ? '; domain=' + options.domain : ''; var secure = options.secure ? '; secure' : ''; document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
    }
    else {
        var cookieValue = null; if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = jQuery.trim(cookies[i]);
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    }
};

; (function (jQuery) {
    jQuery.fn.__bind__ = jQuery.fn.bind; jQuery.fn.__unbind__ = jQuery.fn.unbind; jQuery.fn.__find__ = jQuery.fn.find; var hotkeys = { version: '0.7.8', override: /keydown|keypress|keyup/g, triggersMap: {}, specialKeys: { 27: 'esc', 9: 'tab', 32: 'space', 13: 'return', 8: 'backspace', 145: 'scroll', 20: 'capslock', 144: 'numlock', 19: 'pause', 45: 'insert', 36: 'home', 46: 'del', 35: 'end', 33: 'pageup', 34: 'pagedown', 37: 'left', 38: 'up', 39: 'right', 40: 'down', 112: 'f1', 113: 'f2', 114: 'f3', 115: 'f4', 116: 'f5', 117: 'f6', 118: 'f7', 119: 'f8', 120: 'f9', 121: 'f10', 122: 'f11', 123: 'f12' }, shiftNums: { "`": "~", "1": "!", "2": "@", "3": "#", "4": "$", "5": "%", "6": "^", "7": "&", "8": "*", "9": "(", "0": ")", "-": "_", "=": "+", ";": ":", "'": "\"", ",": "<", ".": ">", "/": "?", "\\": "|" }, newTrigger: function (type, combi, callback) { var result = {}; result[type] = {}; result[type][combi] = { cb: callback, disableInInput: false }; return result; } }; if (jQuery.browser.mozilla) {
        hotkeys.specialKeys = jQuery.extend(hotkeys.specialKeys, { 96: '0', 97: '1', 98: '2', 99: '3', 100: '4', 101: '5', 102: '6', 103: '7', 104: '8', 105: '9' });
    }
    jQuery.fn.find = function (selector) { this.query = selector; return jQuery.fn.__find__.apply(this, arguments); }; jQuery.fn.unbind = function (type, combi, fn) {
        if (jQuery.isFunction(combi)) { fn = combi; combi = null; }
        if (combi && typeof combi === 'string') {
            var selectorId = ((this.prevObject && this.prevObject.query) || (this[0].id && this[0].id) || this[0]).toString(); var hkTypes = type.split(' '); for (var x = 0; x < hkTypes.length; x++) {
                delete hotkeys.triggersMap[selectorId][hkTypes[x]][combi];
            }
        }
        return this.__unbind__(type, fn);
    }; jQuery.fn.bind = function (type, data, fn) {
        var handle = type.match(hotkeys.override); if (jQuery.isFunction(data) || !handle) {
            return this.__bind__(type, data, fn);
        }
        else {
            var result = null, pass2jq = jQuery.trim(type.replace(hotkeys.override, '')); if (pass2jq) {
                result = this.__bind__(pass2jq, data, fn);
            }
            if (typeof data === "string") {
                data = { 'combi': data };
            }
            if (data.combi) {
                for (var x = 0; x < handle.length; x++) {
                    var eventType = handle[x]; var combi = data.combi.toLowerCase(), trigger = hotkeys.newTrigger(eventType, combi, fn), selectorId = ((this.prevObject && this.prevObject.query) || (this[0].id && this[0].id) || this[0]).toString(); trigger[eventType][combi].disableInInput = data.disableInInput; if (!hotkeys.triggersMap[selectorId]) {
                        hotkeys.triggersMap[selectorId] = trigger;
                    }
                    else if (!hotkeys.triggersMap[selectorId][eventType]) {
                        hotkeys.triggersMap[selectorId][eventType] = trigger[eventType];
                    }
                    var mapPoint = hotkeys.triggersMap[selectorId][eventType][combi]; if (!mapPoint) {
                        hotkeys.triggersMap[selectorId][eventType][combi] = [trigger[eventType][combi]];
                    }
                    else if (mapPoint.constructor !== Array) {
                        hotkeys.triggersMap[selectorId][eventType][combi] = [mapPoint];
                    }
                    else {
                        hotkeys.triggersMap[selectorId][eventType][combi][mapPoint.length] = trigger[eventType][combi];
                    }
                    this.each(function () {
                        var jqElem = jQuery(this); if (jqElem.attr('hkId') && jqElem.attr('hkId') !== selectorId) {
                            selectorId = jqElem.attr('hkId') + ";" + selectorId;
                        }
                        jqElem.attr('hkId', selectorId);
                    }); result = this.__bind__(handle.join(' '), data, hotkeys.handler)
                }
            }
            return result;
        }
    }; hotkeys.findElement = function (elem) {
        if (!jQuery(elem).attr('hkId')) {
            if (jQuery.browser.opera || jQuery.browser.safari) {
                while (!jQuery(elem).attr('hkId') && elem.parentNode) {
                    elem = elem.parentNode;
                }
            }
        }
        return elem;
    }; hotkeys.handler = function (event) {
        var target = hotkeys.findElement(event.currentTarget), jTarget = jQuery(target), ids = jTarget.attr('hkId'); if (ids) {
            ids = ids.split(';'); var code = event.which, type = event.type, special = hotkeys.specialKeys[code], character = !special && String.fromCharCode(code).toLowerCase(), shift = event.shiftKey, ctrl = event.ctrlKey, alt = event.altKey || event.originalEvent.altKey, mapPoint = null; for (var x = 0; x < ids.length; x++) {
                if (hotkeys.triggersMap[ids[x]][type]) {
                    mapPoint = hotkeys.triggersMap[ids[x]][type]; break;
                }
            }
            if (mapPoint) {
                var trigger; if (!shift && !ctrl && !alt) {
                    trigger = mapPoint[special] || (character && mapPoint[character]);
                }
                else {
                    var modif = ''; if (alt) modif += 'alt+'; if (ctrl) modif += 'ctrl+'; if (shift) modif += 'shift+'; trigger = mapPoint[modif + special]; if (!trigger) {
                        if (character) {
                            trigger = mapPoint[modif + character] || mapPoint[modif + hotkeys.shiftNums[character]] || (modif === 'shift+' && mapPoint[hotkeys.shiftNums[character]]);
                        }
                    }
                }
                if (trigger) {
                    var result = false; for (var x = 0; x < trigger.length; x++) {
                        if (trigger[x].disableInInput) {
                            var elem = jQuery(event.target); if (jTarget.is("input") || jTarget.is("textarea") || elem.is("input") || elem.is("textarea")) {
                                return true;
                            }
                        }
                        result = result || trigger[x].cb.apply(this, [event]);
                    }
                    return result;
                }
            }
        }
    }; window.hotkeys = hotkeys; return jQuery;
})(jQuery);;
/*! Copyright (c) 2013 Brandon Aaron (http://brandon.aaron.sh)
 * Licensed under the MIT License (LICENSE.txt).
 *
 * Version: 3.1.9
 *
 * Requires: jQuery 1.2.2+
 */
(function (factory) { if (typeof define === 'function' && define.amd) { define(['jquery'], factory); } else if (typeof exports === 'object') { module.exports = factory; } else { factory(jQuery); } }(function ($) {
    var toFix = ['wheel', 'mousewheel', 'DOMMouseScroll', 'MozMousePixelScroll'], toBind = ('onwheel' in document || document.documentMode >= 9) ? ['wheel'] : ['mousewheel', 'DomMouseScroll', 'MozMousePixelScroll'], slice = Array.prototype.slice, nullLowestDeltaTimeout, lowestDelta; if ($.event.fixHooks) {
        for (var i = toFix.length; i;) {
            $.event.fixHooks[toFix[--i]] = $.event.mouseHooks;
        }
    }
    var special = $.event.special.mousewheel = {
        version: '3.1.9', setup: function () {
            if (this.addEventListener) { for (var i = toBind.length; i;) { this.addEventListener(toBind[--i], handler, false); } } else { this.onmousewheel = handler; }
            $.data(this, 'mousewheel-line-height', special.getLineHeight(this)); $.data(this, 'mousewheel-page-height', special.getPageHeight(this));
        }, teardown: function () { if (this.removeEventListener) { for (var i = toBind.length; i;) { this.removeEventListener(toBind[--i], handler, false); } } else { this.onmousewheel = null; } }, getLineHeight: function (elem) { return parseInt($(elem)['offsetParent' in $.fn ? 'offsetParent' : 'parent']().css('fontSize'), 10); }, getPageHeight: function (elem) { return $(elem).height(); }, settings: { adjustOldDeltas: true }
    }; $.fn.extend({ mousewheel: function (fn) { return fn ? this.bind('mousewheel', fn) : this.trigger('mousewheel'); }, unmousewheel: function (fn) { return this.unbind('mousewheel', fn); } }); function handler(event) {
        var orgEvent = event || window.event, args = slice.call(arguments, 1), delta = 0, deltaX = 0, deltaY = 0, absDelta = 0; event = $.event.fix(orgEvent); event.type = 'mousewheel'; if ('detail' in orgEvent) {
            deltaY = orgEvent.detail * -1;
        }
        if ('wheelDelta' in orgEvent) {
            deltaY = orgEvent.wheelDelta;
        }
        if ('wheelDeltaY' in orgEvent) {
            deltaY = orgEvent.wheelDeltaY;
        }
        if ('wheelDeltaX' in orgEvent) {
            deltaX = orgEvent.wheelDeltaX * -1;
        }
        if ('axis' in orgEvent && orgEvent.axis === orgEvent.HORIZONTAL_AXIS) {
            deltaX = deltaY * -1; deltaY = 0;
        }
        delta = deltaY === 0 ? deltaX : deltaY; if ('deltaY' in orgEvent) {
            deltaY = orgEvent.deltaY * -1; delta = deltaY;
        }
        if ('deltaX' in orgEvent) {
            deltaX = orgEvent.deltaX; if (deltaY === 0) {
                delta = deltaX * -1;
            }
        }
        if (deltaY === 0 && deltaX === 0) {
            return;
        }
        if (orgEvent.deltaMode === 1) { var lineHeight = $.data(this, 'mousewheel-line-height'); delta *= lineHeight; deltaY *= lineHeight; deltaX *= lineHeight; } else if (orgEvent.deltaMode === 2) { var pageHeight = $.data(this, 'mousewheel-page-height'); delta *= pageHeight; deltaY *= pageHeight; deltaX *= pageHeight; }
        absDelta = Math.max(Math.abs(deltaY), Math.abs(deltaX)); if (!lowestDelta || absDelta < lowestDelta) {
            lowestDelta = absDelta; if (shouldAdjustOldDeltas(orgEvent, absDelta)) {
                lowestDelta /= 40;
            }
        }
        if (shouldAdjustOldDeltas(orgEvent, absDelta)) {
            delta /= 40; deltaX /= 40; deltaY /= 40;
        }
        delta = Math[delta >= 1 ? 'floor' : 'ceil'](delta / lowestDelta); deltaX = Math[deltaX >= 1 ? 'floor' : 'ceil'](deltaX / lowestDelta); deltaY = Math[deltaY >= 1 ? 'floor' : 'ceil'](deltaY / lowestDelta); event.deltaX = deltaX; event.deltaY = deltaY; event.deltaFactor = lowestDelta; event.deltaMode = 0; args.unshift(event, delta, deltaX, deltaY); if (nullLowestDeltaTimeout) {
            clearTimeout(nullLowestDeltaTimeout);
        }
        nullLowestDeltaTimeout = setTimeout(nullLowestDelta, 200); return ($.event.dispatch || $.event.handle).apply(this, args);
    }
    function nullLowestDelta() {
        lowestDelta = null;
    }
    function shouldAdjustOldDeltas(orgEvent, absDelta) {
        return special.settings.adjustOldDeltas && orgEvent.type === 'mousewheel' && absDelta % 120 === 0;
    }
}));

; (function ($) {
    $.extend($.fn, {
        validate: function (options) {
            if (!this.length) {
                options && options.debug && window.console && console.warn("nothing selected, can't validate, returning nothing"); return;
            }
            var validator = $.data(this[0], 'validator');
            if (validator) {
                return validator;
            }
            if (typeof (Worker) !== "undefined") {
                this.attr('novalidate', 'novalidate');
            }
            validator = new $.validator(options, this[0]);
            $.data(this[0], 'validator', validator);
            if (validator.settings.onsubmit) {
                var inputsAndButtons = this.find("input, button");
                inputsAndButtons.filter(".cancel").click(function () { validator.cancelSubmit = true; }); if (validator.settings.submitHandler) {
                    inputsAndButtons.filter(":submit").click(function () {
                        validator.submitButton = this;
                    });
                }
                this.submit(function (event) {
                    if (validator.settings.debug)
                        event.preventDefault(); function handle() {
                            if (validator.settings.submitHandler) {
                                if (validator.submitButton) { var hidden = $("<input type='hidden'/>").attr("name", validator.submitButton.name).val(validator.submitButton.value).appendTo(validator.currentForm); }
                                validator.settings.submitHandler.call(validator, validator.currentForm); if (validator.submitButton) {
                                    hidden.remove();
                                }
                                return false;
                            }
                            return true;
                        }
                    if (validator.cancelSubmit) {
                        validator.cancelSubmit = false; return handle();
                    }
                    if (validator.form()) {
                        if (validator.pendingRequest) {
                            validator.formSubmitted = true; return false;
                        }
                        return handle();
                    } else {
                        validator.focusInvalid(); return false;
                    }
                });
            }
            return validator;
        }, valid: function () { if ($(this[0]).is('form')) { return this.validate().form(); } else { var valid = true; var validator = $(this[0].form).validate(); this.each(function () { valid &= validator.element(this); }); return valid; } }, removeAttrs: function (attributes) { var result = {}, $element = this; $.each(attributes.split(/\s/), function (index, value) { result[value] = $element.attr(value); $element.removeAttr(value); }); return result; }, rules: function (command, argument) {
            var element = this[0]; if (command) {
                var settings = $.data(element.form, 'validator').settings; var staticRules = settings.rules; var existingRules = $.validator.staticRules(element); switch (command) {
                    case "add": $.extend(existingRules, $.validator.normalizeRule(argument)); staticRules[element.name] = existingRules; if (argument.messages)
                        settings.messages[element.name] = $.extend(settings.messages[element.name], argument.messages); break; case "remove": if (!argument) {
                            delete staticRules[element.name]; return existingRules;
                        }
                            var filtered = {}; $.each(argument.split(/\s/), function (index, method) { filtered[method] = existingRules[method]; delete existingRules[method]; }); return filtered;
                }
            }
            var data = $.validator.normalizeRules($.extend({}, $.validator.metadataRules(element), $.validator.classRules(element), $.validator.attributeRules(element), $.validator.staticRules(element)), element); if (data.required) {
                var param = data.required; delete data.required; data = $.extend({ required: param }, data);
            }
            return data;
        }
    });
    $.extend($.expr[":"], { blank: function (a) { return !$.trim("" + a.value); }, filled: function (a) { return !!$.trim("" + a.value); }, unchecked: function (a) { return !a.checked; } });
    $.validator = function (options, form) { this.settings = $.extend(true, {}, $.validator.defaults, options); this.currentForm = form; this.init(); };
    $.validator.format = function (source, params) {
        if (arguments.length == 1)
            return function () { var args = $.makeArray(arguments); args.unshift(source); return $.validator.format.apply(this, args); }; if (arguments.length > 2 && params.constructor != Array) {
                params = $.makeArray(arguments).slice(1);
            }
        if (params.constructor != Array) {
            params = [params];
        }
        $.each(params, function (i, n) { source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n); }); return source;
    };
    $.extend($.validator, {
        defaults: {
            messages: {}, groups: {}, rules: {}, errorClass: "error", validClass: "valid", errorElement: "label", focusInvalid: true, errorContainer: $([]), errorLabelContainer: $([]), onsubmit: true, ignore: ":hidden", ignoreTitle: false, onfocusin: function (element, event) { this.lastActive = element; if (this.settings.focusCleanup && !this.blockFocusCleanup) { this.settings.unhighlight && this.settings.unhighlight.call(this, element, this.settings.errorClass, this.settings.validClass); this.addWrapper(this.errorsFor(element)).hide(); } }, onfocusout: function (element, event) { if (!this.checkable(element) && (element.name in this.submitted || !this.optional(element))) { this.element(element); } }, onkeyup: function (element, event) { if (element.name in this.submitted || element == this.lastElement) { this.element(element); } }, onclick: function (element, event) {
                if (element.name in this.submitted)
                    this.element(element); else if (element.parentNode.name in this.submitted)
                        this.element(element.parentNode);
            }, highlight: function (element, errorClass, validClass) { if (element.type === 'radio') { this.findByName(element.name).addClass(errorClass).removeClass(validClass); } else { $(element).addClass(errorClass).removeClass(validClass); } }, unhighlight: function (element, errorClass, validClass) { if (element.type === 'radio') { this.findByName(element.name).removeClass(errorClass).addClass(validClass); } else { $(element).removeClass(errorClass).addClass(validClass); } }
        }, setDefaults: function (settings) { $.extend($.validator.defaults, settings); }, messages: { required: "This field is required.", remote: "Please fix this field.", email: "Please enter a valid email address.", url: "Please enter a valid URL.", date: "Please enter a valid date.", dateISO: "Please enter a valid date (ISO).", number: "Please enter a valid number.", digits: "Please enter only digits.", creditcard: "Please enter a valid credit card number.", equalTo: "Please enter the same value again.", notEqualTo: "Please enter the different value again.", largerThan: "Please enter the larger value again.", lessThan: "Please enter the less value again.", accept: "Please enter a value with a valid extension.", maxlength: $.validator.format("Please enter no more than {0} characters."), minlength: $.validator.format("Please enter at least {0} characters."), rangelength: $.validator.format("Please enter a value between {0} and {1} characters long."), range: $.validator.format("Please enter a value between {0} and {1}."), max: $.validator.format("Please enter a value less than or equal to {0}."), min: $.validator.format("Please enter a value greater than or equal to {0}.") }, autoCreateRanges: false, prototype: {
            init: function () {
                this.labelContainer = $(this.settings.errorLabelContainer); this.errorContext = this.labelContainer.length && this.labelContainer || $(this.currentForm); this.containers = $(this.settings.errorContainer).add(this.settings.errorLabelContainer); this.submitted = {}; this.valueCache = {}; this.pendingRequest = 0; this.pending = {}; this.invalid = {}; this.reset(); var groups = (this.groups = {}); $.each(this.settings.groups, function (key, value) { $.each(value.split(/\s/), function (index, name) { groups[name] = key; }); }); var rules = this.settings.rules; $.each(rules, function (key, value) { rules[key] = $.validator.normalizeRule(value); }); function delegate(event) {
                    var validator = $.data(this[0].form, "validator"), eventType = "on" + event.type.replace(/^validate/, ""); validator.settings[eventType] && validator.settings[eventType].call(validator, this[0], event);
                }
                $(this.currentForm).validateDelegate("[type='text'], [type='password'], [type='file'], select, textarea, " + "[type='number'], [type='search'] ,[type='tel'], [type='url'], " + "[type='email'], [type='datetime'], [type='date'], [type='month'], " + "[type='week'], [type='time'], [type='datetime-local'], " + "[type='range'], [type='color'] ", "focusin focusout keyup", delegate).validateDelegate("[type='radio'], [type='checkbox'], select, option", "click", delegate); if (this.settings.invalidHandler)
                    $(this.currentForm).bind("invalid-form.validate", this.settings.invalidHandler);
            }, form: function () {
                
                this.checkForm(); $.extend(this.submitted, this.errorMap); this.invalid = $.extend({}, this.errorMap); if (!this.valid())
                    $(this.currentForm).triggerHandler("invalid-form", [this]); this.showErrors(); return this.valid();
            },
            checkForm: function () {
                
                this.prepareForm(); for (var i = 0, elements = (this.currentElements = this.elements()) ; elements[i]; i++) {
                    this.check(elements[i]);
                }
                return this.valid();
            }, element: function (element) {
                element = this.validationTargetFor(this.clean(element)); this.lastElement = element; this.prepareElement(element); this.currentElements = $(element); var result = this.check(element); if (result) { delete this.invalid[element.name]; } else { this.invalid[element.name] = true; }
                if (!this.numberOfInvalids()) {
                    this.toHide = this.toHide.add(this.containers);
                }
                this.showErrors(); return result;
            }, showErrors: function (errors) {
                if (errors) {
                    $.extend(this.errorMap, errors); this.errorList = []; for (var name in errors) {
                        this.errorList.push({ message: errors[name], element: this.findByName(name)[0] });
                    }
                    this.successList = $.grep(this.successList, function (element) {
                        return !(element.name in errors);
                    });
                }
                this.settings.showErrors ? this.settings.showErrors.call(this, this.errorMap, this.errorList) : this.defaultShowErrors();
            }, resetForm: function () {
                if ($.fn.resetForm)
                    $(this.currentForm).resetForm(); this.submitted = {}; this.lastElement = null; this.prepareForm(); this.hideErrors(); this.elements().removeClass(this.settings.errorClass);
            }, numberOfInvalids: function () { return this.objectLength(this.invalid); }, objectLength: function (obj) {
                var count = 0; for (var i in obj)
                    count++; return count;
            }, hideErrors: function () { this.addWrapper(this.toHide).hide(); }, valid: function () { return this.size() == 0; }, size: function () { return this.errorList.length; }, focusInvalid: function () { if (this.settings.focusInvalid) { try { $(this.findLastActive() || this.errorList.length && this.errorList[0].element || []).filter(":visible").focus().trigger("focusin"); } catch (e) { } } }, findLastActive: function () { var lastActive = this.lastActive; return lastActive && $.grep(this.errorList, function (n) { return n.element.name == lastActive.name; }).length == 1 && lastActive; }, elements: function () {
                var validator = this, rulesCache = {}; return $(this.currentForm).find("input, select, textarea").not(":submit, :reset, :image, [disabled]").not(this.settings.ignore).filter(function () {
                    !this.name && validator.settings.debug && window.console && console.error("%o has no name assigned", this); if (this.name in rulesCache || !validator.objectLength($(this).rules()))
                        return false; rulesCache[this.name] = true; return true;
                });
            }, clean: function (selector) { return $(selector)[0]; }, errors: function () { return $(this.settings.errorElement + "." + this.settings.errorClass, this.errorContext); }, reset: function () { this.successList = []; this.errorList = []; this.errorMap = {}; this.toShow = $([]); this.toHide = $([]); this.currentElements = $([]); }, prepareForm: function () { this.reset(); this.toHide = this.errors().add(this.containers); }, prepareElement: function (element) { this.reset(); this.toHide = this.errorsFor(element); }, check: function (element) {
                element = this.validationTargetFor(this.clean(element)); var rules = $(element).rules(); var dependencyMismatch = false; for (var method in rules) {
                    var rule = { method: method, parameters: rules[method] }; try {
                        if (typeof ($.validator.methods[method]) != 'undefined') {
                            var result = $.validator.methods[method].call(this, element.value.replace(/\r/g, ""), element, rule.parameters); if (result == "dependency-mismatch") {
                                dependencyMismatch = true; continue;
                            }
                            dependencyMismatch = false; if (result == "pending") {
                                this.toHide = this.toHide.not(this.errorsFor(element)); return;
                            }
                            if (!result) {
                                this.formatAndAdd(element, rule); return false;
                            }
                        }
                    } catch (e) {
                        this.settings.debug && window.console && console.log("exception occured when checking element " + element.id
                        + ", check the '" + rule.method + "' method", e); throw e;
                    }
                }
                if (dependencyMismatch)
                    return; if (this.objectLength(rules))
                        this.successList.push(element); return true;
            }, customMetaMessage: function (element, method) {
                if (!$.metadata)
                    return; var meta = this.settings.meta ? $(element).metadata()[this.settings.meta] : $(element).metadata(); return meta && meta.messages && meta.messages[method];
            }, customMessage: function (name, method) { var m = this.settings.messages[name]; return m && (m.constructor == String ? m : m[method]); }, findDefined: function () {
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i] !== undefined)
                        return arguments[i];
                }
                return undefined;
            }, defaultMessage: function (element, method) { return this.findDefined(this.customMessage(element.name, method), this.customMetaMessage(element, method), !this.settings.ignoreTitle && element.title || undefined, $.validator.messages[method], "<strong>Warning: No message defined for " + element.name + "</strong>"); }, formatAndAdd: function (element, rule) {
                var message = this.defaultMessage(element, rule.method), theregex = /\$?\{(\d+)\}/g; if (typeof message == "function") { message = message.call(this, rule.parameters, element); } else if (theregex.test(message)) {
                    message = jQuery.format(message.replace(theregex, '{$1}'), rule.parameters);
                }
                this.errorList.push({ message: message, element: element }); this.errorMap[element.name] = message; this.submitted[element.name] = message;
            }, addWrapper: function (toToggle) {
                if (this.settings.wrapper)
                    toToggle = toToggle.add(toToggle.parent(this.settings.wrapper)); return toToggle;
            }, defaultShowErrors: function () {
                for (var i = 0; this.errorList[i]; i++) {
                    var error = this.errorList[i]; this.settings.highlight && this.settings.highlight.call(this, error.element, this.settings.errorClass, this.settings.validClass); this.showLabel(error.element, error.message);
                }
                if (this.errorList.length) {
                    this.toShow = this.toShow.add(this.containers);
                }
                if (this.settings.success) {
                    for (var i = 0; this.successList[i]; i++) {
                        this.showLabel(this.successList[i]);
                    }
                }
                if (this.settings.unhighlight) {
                    for (var i = 0, elements = this.validElements() ; elements[i]; i++) {
                        this.settings.unhighlight.call(this, elements[i], this.settings.errorClass, this.settings.validClass);
                    }
                }
                this.toHide = this.toHide.not(this.toShow); this.hideErrors(); this.addWrapper(this.toShow).show();
            }, validElements: function () { return this.currentElements.not(this.invalidElements()); }, invalidElements: function () { return $(this.errorList).map(function () { return this.element; }); }, showLabel: function (element, message) {
                var label = this.errorsFor(element); if (label.length) { label.removeClass(this.settings.validClass).addClass(this.settings.errorClass); label.attr("generated") && label.html(message); } else {
                    label = $("<" + this.settings.errorElement + "/>").attr({ "for": this.idOrName(element), generated: true }).addClass(this.settings.errorClass).html(message || ""); if (this.settings.wrapper) {
                        label = label.hide().show().wrap("<" + this.settings.wrapper + "/>").parent();
                    }
                    if (!this.labelContainer.append(label).length)
                        this.settings.errorPlacement ? this.settings.errorPlacement(label, $(element)) : label.insertAfter(element);
                }
                if (!message && this.settings.success) {
                    label.text(""); typeof this.settings.success == "string" ? label.addClass(this.settings.success) : this.settings.success(label);
                }
                this.toShow = this.toShow.add(label);
            }, errorsFor: function (element) { var name = this.idOrName(element); return this.errors().filter(function () { return $(this).attr('for') == name; }); }, idOrName: function (element) { return this.groups[element.name] || (this.checkable(element) ? element.name : element.id || element.name); }, validationTargetFor: function (element) {
                if (this.checkable(element)) {
                    element = this.findByName(element.name).not(this.settings.ignore)[0];
                }
                return element;
            }, checkable: function (element) { return /radio|checkbox/i.test(element.type); }, findByName: function (name) { var form = this.currentForm; return $(document.getElementsByName(name)).map(function (index, element) { return element.form == form && element.name == name && element || null; }); }, getLength: function (value, element) {
                switch (element.nodeName.toLowerCase()) {
                    case 'select': return $("option:selected", element).length; case 'input': if (this.checkable(element))
                        return this.findByName(element.name).filter(':checked').length;
                }
                return value.length;
            }, depend: function (param, element) { return this.dependTypes[typeof param] ? this.dependTypes[typeof param](param, element) : true; }, dependTypes: { "boolean": function (param, element) { return param; }, "string": function (param, element) { return !!$(param, element.form).length; }, "function": function (param, element) { return param(element); } }, optional: function (element) { return !$.validator.methods.required.call(this, $.trim(element.value), element) && "dependency-mismatch"; }, startRequest: function (element) { if (!this.pending[element.name]) { this.pendingRequest++; this.pending[element.name] = true; } }, stopRequest: function (element, valid) {
                this.pendingRequest--; if (this.pendingRequest < 0)
                    this.pendingRequest = 0; delete this.pending[element.name]; if (valid && this.pendingRequest == 0 && this.formSubmitted && this.form()) { $(this.currentForm).submit(); this.formSubmitted = false; } else if (!valid && this.pendingRequest == 0 && this.formSubmitted) { $(this.currentForm).triggerHandler("invalid-form", [this]); this.formSubmitted = false; }
            }, previousValue: function (element) {
                return $.data(element, "previousValue") || $.data(element, "previousValue", { old: null, valid: true, message: this.defaultMessage(element, "remote") });
            }
        }, classRuleSettings: { required: { required: true }, email: { email: true }, url: { url: true }, date: { date: true }, dateISO: { dateISO: true }, dateDE: { dateDE: true }, number: { number: true }, numberDE: { numberDE: true }, digits: { digits: true }, creditcard: { creditcard: true } }, addClassRules: function (className, rules) { className.constructor == String ? this.classRuleSettings[className] = rules : $.extend(this.classRuleSettings, className); }, classRules: function (element) { var rules = {}; var classes = $(element).attr('class'); classes && $.each(classes.split(' '), function () { if (this in $.validator.classRuleSettings) { $.extend(rules, $.validator.classRuleSettings[this]); } }); return rules; }, attributeRules: function (element) {
            var rules = {};
            var $element = $(element);
            for (var method in $.validator.methods) {
                var value;
                if (method === 'required' && typeof $.fn.prop === 'function') { value = $element.prop(method); } else { value = $element.attr(method); }
                if (value) { rules[method] = value; } else if ($element[0].getAttribute("type") === method) {
                    rules[method] = true;
                }
            }
            if (rules.maxlength && /-1|2147483647|524288/.test(rules.maxlength)) {
                delete rules.maxlength;
            }
            return rules;
        }, metadataRules: function (element) { if (!$.metadata) return {}; var meta = $.data(element.form, 'validator').settings.meta; return meta ? $(element).metadata()[meta] : $(element).metadata(); }, staticRules: function (element) {
            var rules = {}; var validator = $.data(element.form, 'validator'); if (validator.settings.rules) {
                rules = $.validator.normalizeRule(validator.settings.rules[element.name]) || {};
            }
            return rules;
        }, normalizeRules: function (rules, element) {
            $.each(rules, function (prop, val) {
                if (val === false) {
                    delete rules[prop]; return;
                }
                if (val.param || val.depends) {
                    var keepRule = true; switch (typeof val.depends) {
                        case "string": keepRule = !!$(val.depends, element.form).length; break; case "function": keepRule = val.depends.call(element, element); break;
                    }
                    if (keepRule) { rules[prop] = val.param !== undefined ? val.param : true; } else { delete rules[prop]; }
                }
            }); $.each(rules, function (rule, parameter) { rules[rule] = $.isFunction(parameter) ? parameter(element) : parameter; }); $.each(['minlength', 'maxlength', 'min', 'max'], function () { if (rules[this]) { rules[this] = Number(rules[this]); } }); $.each(['rangelength', 'range'], function () { if (rules[this]) { rules[this] = [Number(rules[this][0]), Number(rules[this][1])]; } }); if ($.validator.autoCreateRanges) {
                if (rules.min && rules.max) {
                    rules.range = [rules.min, rules.max]; delete rules.min; delete rules.max;
                }
                if (rules.minlength && rules.maxlength) {
                    rules.rangelength = [rules.minlength, rules.maxlength]; delete rules.minlength; delete rules.maxlength;
                }
            }
            if (rules.messages) {
                delete rules.messages;
            }
            return rules;
        }, normalizeRule: function (data) {
            if (typeof data == "string") { var transformed = {}; $.each(data.split(/\s/), function () { transformed[this] = true; }); data = transformed; }
            return data;
        }, addMethod: function (name, method, message) { $.validator.methods[name] = method; $.validator.messages[name] = message != undefined ? message : $.validator.messages[name]; if (method.length < 3) { $.validator.addClassRules(name, $.validator.normalizeRule(name)); } }, methods: {
            required: function (value, element, param) {
                if (!this.depend(param, element))
                    return "dependency-mismatch"; switch (element.nodeName.toLowerCase()) {
                        case 'select': var val = $(element).val(); return val && val.length > 0; case 'input': if (this.checkable(element))
                            return this.getLength(value, element) > 0; default: return $.trim(value).length > 0;
                    }
            }, remote: function (value, element, param) {
                if (this.optional(element))
                    return "dependency-mismatch"; var previous = this.previousValue(element); if (!this.settings.messages[element.name])
                        this.settings.messages[element.name] = {}; previous.originalMessage = this.settings.messages[element.name].remote; this.settings.messages[element.name].remote = previous.message; param = typeof param == "string" && { url: param } || param; if (this.pending[element.name]) {
                            return "pending";
                        }
                if (previous.old === value) {
                    return previous.valid;
                }
                previous.old = value; var validator = this; this.startRequest(element); var data = {}; data[element.name] = value; $.ajax($.extend(true, {
                    url: param, mode: "abort", port: "validate" + element.name, dataType: "json", data: data, success: function (response) {
                        validator.settings.messages[element.name].remote = previous.originalMessage; var valid = response === true; if (valid) { var submitted = validator.formSubmitted; validator.prepareElement(element); validator.formSubmitted = submitted; validator.successList.push(element); validator.showErrors(); } else { var errors = {}; var message = response || validator.defaultMessage(element, "remote"); errors[element.name] = previous.message = $.isFunction(message) ? message(value) : message; validator.showErrors(errors); }
                        previous.valid = valid; validator.stopRequest(element, valid);
                    }
                }, param)); return "pending";
            }, minlength: function (value, element, param) { return this.optional(element) || this.getLength($.trim(value), element) >= param; }, maxlength: function (value, element, param) { return this.optional(element) || this.getLength($.trim(value), element) <= param; }, rangelength: function (value, element, param) { var length = this.getLength($.trim(value), element); return this.optional(element) || (length >= param[0] && length <= param[1]); }, min: function (value, element, param) { return this.optional(element) || value >= param; }, max: function (value, element, param) { return this.optional(element) || value <= param; }, range: function (value, element, param) { return this.optional(element) || (value >= param[0] && value <= param[1]); }, email: function (value, element) { return this.optional(element) || /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/i.test(value); }, url: function (value, element) { return this.optional(element) || /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(value); }, date: function (value, element) { return this.optional(element) || !/Invalid|NaN/.test(new Date(value)); }, dateISO: function (value, element) { return this.optional(element) || /^\d{4}[\/-]\d{1,2}[\/-]\d{1,2}$/.test(value); }, number: function (value, element) { return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/.test(value); }, digits: function (value, element) { return this.optional(element) || /^\d+$/.test(value); }, creditcard: function (value, element) {
                if (this.optional(element))
                    return "dependency-mismatch"; if (/[^0-9 -]+/.test(value))
                        return false; var nCheck = 0, nDigit = 0, bEven = false; value = value.replace(/\D/g, ""); for (var n = value.length - 1; n >= 0; n--) {
                            var cDigit = value.charAt(n); var nDigit = parseInt(cDigit, 10); if (bEven) {
                                if ((nDigit *= 2) > 9)
                                    nDigit -= 9;
                            }
                            nCheck += nDigit; bEven = !bEven;
                        }
                return (nCheck % 10) == 0;
            }, accept: function (value, element, param) { param = typeof param == "string" ? param.replace(/,/g, '|') : "png|jpe?g|gif"; return this.optional(element) || value.match(new RegExp(".(" + param + ")$", "i")); }, equalTo: function (value, element, param) { var target = $(param).unbind(".validate-equalTo").bind("blur.validate-equalTo", function () { $(element).valid(); }); return value == target.val(); }, notEqualTo: function (value, element, param) { var target = $(param).unbind(".validate-notEqualTo").bind("blur.validate-notEqualTo", function () { $(element).valid(); }); return !(value == target.val()); }, largerThan: function (value, element, param) {
                var target = $(param).unbind(".validate-largerThan").bind("blur.validate-largerThan", function () { $(element).valid(); }); var targetVal = target.val(); if (isNaN(value) || value == '' || value == '0')
                    return true; if (isNaN(targetVal) || targetVal == '')
                        return true; return eval(value) > eval(targetVal);
            }, lessThan: function (value, element, param) {
                var target = $(param).unbind(".validate-lessThan").bind("blur.validate-lessThan", function () { $(element).valid(); }); var targetVal = target.val(); if (isNaN(value) || value == '' || value == '0')
                    return true; if (isNaN(targetVal) || targetVal == '')
                        return true; return eval(value) < eval(targetVal);
            }
        }
    });
    $.format = $.validator.format;
})(jQuery);
; (function ($) {
    var pendingRequests = {}; if ($.ajaxPrefilter) {
        $.ajaxPrefilter(function (settings, _, xhr) {
            var port = settings.port; if (settings.mode == "abort") {
                if (pendingRequests[port]) {
                    pendingRequests[port].abort();
                }
                pendingRequests[port] = xhr;
            }
        });
    } else {
        var ajax = $.ajax; $.ajax = function (settings) {
            var mode = ("mode" in settings ? settings : $.ajaxSettings).mode, port = ("port" in settings ? settings : $.ajaxSettings).port; if (mode == "abort") {
                if (pendingRequests[port]) {
                    pendingRequests[port].abort();
                }
                return (pendingRequests[port] = ajax.apply(this, arguments));
            }
            return ajax.apply(this, arguments);
        };
    }
})(jQuery);
; (function ($) {
    if (!jQuery.event.special.focusin && !jQuery.event.special.focusout && document.addEventListener) {
        $.each({ focus: 'focusin', blur: 'focusout' },
            function (original, fix) {
                $.event.special[fix] = {
                    setup: function () {
                        this.addEventListener(original, handler, true);
                    },
                    teardown: function () { this.removeEventListener(original, handler, true); },
                    handler: function (e) { arguments[0] = $.event.fix(e); arguments[0].type = fix; return $.event.handle.apply(this, arguments); }
                };
                function handler(e) { e = $.event.fix(e); e.type = fix; if (typeof ($.event.handle) != 'undefined') { return $.event.handle.call(this, e); } else { return null; } }
            });
    };
    $.extend($.fn, { validateDelegate: function (delegate, type, handler) { return this.bind(type, function (event) { var target = $(event.target); if (target.is(delegate)) { return handler.apply(target, arguments); } }); } });
})(jQuery);
jQuery.validator.addMethod("stringCheck", function (value, element) { return this.optional(element) || /^[\u0391-\uFFE5\w]+$/.test(value); }, "只能包括中文字、英文字母、数字和下划线");
jQuery.validator.addMethod("stringCheckTwo", function (value, element) { return this.optional(element) || /^[a-zA-Z0-9_]{1,}$/.test(value); }, "只能包括英文字母、数字和下划线");
jQuery.validator.addMethod("stringCheckThree", function (value, element) { return this.optional(element) || /^[a-zA-Z]*$/.test(value); }, "只能是英文字母");
jQuery.validator.addMethod("stringCheckFour", function (value, element) { return this.optional(element) || /^[\u4E00-\u9FA5]+$/.test(value); }, "只能是中文");
jQuery.validator.addMethod("stringCheckFive", function (value, element) { return this.optional(element) || /^[a-zA-Z0-9_./]{1,}$/.test(value); }, "只能包括英文字母、数字和下划线、点、反斜杠");
jQuery.validator.addMethod("byteRangeLength", function (value, element, param) {
    var length = value.length; for (var i = 0; i < value.length; i++) {
        if (value.charCodeAt(i) > 127) {
            length++;
        }
    }
    return this.optional(element) || (length >= param[0] && length <= param[1]);
}, jQuery.validator.format("请确保输入的值在{0}-{1}个字节之间(一个中文字算2个字节)"));
jQuery.validator.addMethod("isIdCardNo", function (value, element) { return this.optional(element) || /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/.test(value); }, "请正确输入您的身份证号码");
jQuery.validator.addMethod("isWebSite", function (value, element) { return this.optional(element) || /^[a-z0-9-]{1,}$/.test(value); }, "只能包括英文字母、数字和中横线");
jQuery.validator.addMethod("isMobile", function (value, element) { var length = value.length; var mobile = /^((1[0-9]{1})+\d{9})$/; return this.optional(element) || (length == 11 && mobile.test(value)); }, "请输入合法的手机号码");
jQuery.validator.addMethod("isTel", function (value, element) { var tel = /^\d{3,4}-?\d{7,9}$/; return this.optional(element) || (tel.test(value)); }, "请正确填写您的电话号码");
jQuery.validator.addMethod("isPhone", function (value, element) {
    var length = value.length; var mobile = /^((1[3|5|7|8][0-9]{1})+\d{8})$/;
    var tel = /^\d{3,4}-?\d{7,9}$/; return this.optional(element) || (tel.test(value) || (length == 11 && mobile.test(value)));
}, "请正确填写您的联系电话");
jQuery.validator.addMethod("isZipCode", function (value, element) { var tel = /^[0-9]{6}$/; return this.optional(element) || (tel.test(value)); }, "请正确填写您的邮政编码");
jQuery.validator.addMethod("isQQ", function (value, element) { var qq = /^[1-9]\d{4,11}$/; return this.optional(element) || (qq.test(value)); }, "QQ号码格式错误");
jQuery.validator.addMethod("ints", function (value, element) { var ints = /^[-]{0,1}[0-9]{1,}$/; return this.optional(element) || (ints.test(value)); }, "请输入整数，包括负数、0、整数");
jQuery.validator.addMethod("check_common_num", function (value, element) { value = $.trim(value); if (!value) { return true; } else { return /^[a-zA-Z0-9_\-\.]{1,}$/.test(value); } }, "编号只允许出现字母、数字和分割符（- _ .）");
(function ($) {
    $.extend({
        metadata: {
            defaults: {
                type: 'class',
                name: 'metadata',
                cre: /({.*})/,
                single: 'metadata'
            },
            setType: function (type, name) {
                this.defaults.type = type;
                this.defaults.name = name;
            },
            get: function (elem, opts) {
                var settings = $.extend({}, this.defaults, opts); if (!settings.single.length) settings.single = 'metadata'; var data = $.data(elem, settings.single); if (data) return data; data = "{}"; if (settings.type == "class") {
                    var m = settings.cre.exec(elem.className); if (m)
                        data = m[1];
                } else if (settings.type == "elem") {
                    if (!elem.getElementsByTagName)
                        return undefined; var e = elem.getElementsByTagName(settings.name); if (e.length)
                            data = $.trim(e[0].innerHTML);
                } else if (elem.getAttribute != undefined) {
                    var attr = elem.getAttribute(settings.name); if (attr)
                        data = attr;
                }
                if (data.indexOf('{') < 0)
                    data = "{" + data + "}"; data = eval("(" + data + ")"); $.data(elem, settings.single, data); return data;
            }
        }
    });
    $.fn.metadata = function (opts) {
        return $.metadata.get(this[0], opts);
    };
})(jQuery);

jQuery.extend(jQuery.validator.messages, {
    required: "必选字段",
    remote: "请修正该字段",
    email: "请输入正确格式的电子邮件",
    url: "请输入合法的网址",
    date: "请输入合法的日期",
    dateISO: "请输入合法的日期 (ISO).",
    number: "请输入合法的数字",
    digits: "只能输入整数",
    creditcard: "请输入合法的信用卡号",
    equalTo: "请再次输入相同的值",
    accept: "请输入拥有合法后缀名的字符串",
    maxlength: jQuery.validator.format("请输入一个长度最多是 {0} 的字符串"),
    minlength: jQuery.validator.format("请输入一个长度最少是 {0} 的字符串"),
    rangelength: jQuery.validator.format("请输入一个长度介于 {0} 和 {1} 之间的字符串"),
    range: jQuery.validator.format("请输入一个介于 {0} 和 {1} 之间的值"),
    max: jQuery.validator.format("请输入一个最大为 {0} 的值"),
    min: jQuery.validator.format("请输入一个最小为 {0} 的值"),
    stringCheck: "只能包括中文字、英文字母、数字和下划线",
    stringCheckTwo: "只能包括英文字母、数字和下划线",
    byteRangeLength: jQuery.validator.format("请确保输入的值在{0}-{1}个字节之间(一个中文字算2个字节)"),
    isIdCardNo: "请正确输入您的身份证号码",
    isMobile: "请正确填写您的手机号码",
    isTel: "请正确填写您的电话号码",
    isPhone: "请正确填写您的联系电话",
    isZipCode: "请正确填写您的邮政编码",
    isQQ: "QQ号码格式错误",
    ints: "请输入整数，包括负数、0、整数",
    stringCheckThree: "只能是英文字母",
    stringCheckFour: "只能是中文"
});

jQuery.fn.pagination = function (maxentries, opts) {
    
    opts = jQuery.extend({
        items_per_page: 10,
        num_display_entries: 10,
        current_page: 0,
        num_edge_entries: 0,
        link_to: "#",
        prev_text: "<i class=\"fa fa-chevron-left\"></i>",
        next_text: "<i class=\"fa fa-chevron-right\"></i>",
        ellipse_text: "...",
        prev_show_always: true,
        next_show_always: true,
        init_apply: false,
        callback: function () {
            return false;
        }
    }, opts || {});
    
    
    return this.each(function () {
        function numPages(add_n) {
            
            if (add_n) {
                return Math.ceil(maxentries / add_n);
            } else {
                if ($.cookie('pagesize_cookie')) {
                    opts.items_per_page = $.cookie('pagesize_cookie');
                }
                return Math.ceil(maxentries / opts.items_per_page);
            }
            //return Math.ceil(maxentries / opts.items_per_page);
        };
        //计算区间
        function getInterval(add_n) {
            
            var ne_half = Math.ceil(opts.num_display_entries / 2);
            var np = numPages(add_n);
            var upper_limit = np - opts.num_display_entries;
            var start = current_page > ne_half ? Math.max(Math.min(current_page - ne_half, upper_limit), 0) : 0;
            var end = current_page > ne_half ? Math.min(current_page + ne_half, np) : Math.min(opts.num_display_entries, np);
            return [start, end];
        };
        function pageSelected(page_id, evt) {
            
            current_page = page_id;
            drawLinks();
            var continuePropagation = opts.callback(page_id, panel);
            if (!continuePropagation) {
                if (evt.stopPropagation) {
                    evt.stopPropagation();
                } else {
                    evt.cancelBubble = true;
                }
            }
            return continuePropagation;
        };
        //绘制分页DOM
        function drawLinks(add_n) {
            
            panel.empty();      //清空li
            var interval = getInterval(add_n);
            var np = numPages(add_n);
            var getClickHandler = function (page_id) {
                
                return function (evt) {
                    return pageSelected(page_id, evt);
                }
            }
            var appendItem = function (page_id, appendopts) {
                
                page_id = page_id < 0 ? 0 : (page_id < np ? page_id : np - 1);
                appendopts = jQuery.extend({
                    text: page_id + 1,
                    classes: ""
                },
                appendopts || {});
                
                if (page_id == current_page) {
                    var lnk = jQuery("<li class='" + (opts.prev_text != appendopts.text && opts.next_text != appendopts.text ? '' : 'active') + "active'><a>" + (appendopts.text) + "</a></li>").bind("click", getClickHandler(page_id)).attr('href', opts.link_to.replace(/__id__/, page_id));
                } else {
                    var lnk = jQuery("<li><a>" + (appendopts.text) + "</a></li>").bind("click", getClickHandler(page_id)).attr('href', opts.link_to.replace(/__id__/, page_id));
                }
                if (appendopts.classes) {
                    lnk.addClass(appendopts.classes);
                }
                panel.append(lnk);
            };

            if (typeof (opts.dialog_pagesize) === 'undefined' || !opts.dialog_pagesize) {
                //TODO：修改分页Size
                var setpageSize = function (nPage) {
                    $.cookie(
                        'pagesize_cookie',
                        nPage,
                        {
                            expires: 86400 * 30 * 365,
                            path: '/'
                        });

                    $(_ + '#pageSize_').attr('data-value', nPage);
                    $(_ + '#pageSize_ strong').text(nPage);
                    $(_ + '#pageSize_').next().find('li').each(function () {
                        if ($(this).text() == nPage) {
                            $(this).attr('class', 'active');
                        }
                        else {
                            $(this).attr('class', '');
                        }
                    });
                    // $.DHB.refresh();
                    drawLinks(nPage);
                    opts.callback(0, panel, nPage);
                    
                };
            }
            maxentries = ($.trim(maxentries).length == 0 || maxentries < 0)
                ? 1
                : maxentries;

            var totalCount = jQuery("<li><a>共 <strong>" + (maxentries) + " </strong>条</a></li>");
            panel.append(totalCount);       //DOM插入总记录数

            if (typeof (opts.dialog_pagesize) === 'undefined' || !opts.dialog_pagesize) {
                
                var nPageSizeCookie = $.cookie('pagesize_cookie');
                if (!nPageSizeCookie) {
                    nPageSizeCookie = publicSettings.pageSize;
                    $.cookie('pagesize_cookie', nPageSizeCookie, {
                        expires: 86400 * 30 * 365,
                        path: '/'
                    });
                }
                var pageSize = '<li class="dropdown dropup" style="width:100px;max-width:100;min-width:100;"><a data-toggle="dropdown" id="pageSize_" data-value="' + nPageSizeCookie + '">每页 <strong style="text-align:center;width:23px;display:inline-block;">' + nPageSizeCookie + '</strong> 条 <span class="caret"></span></a><ul class="dropdown-menu">';
                //可选分页Size
                var pageSizes = new Array(10, 20, 30, 50, 80, 100);
                for (var i = 0; i < pageSizes.length; i++) {
                    pageSize += '<li' + (nPageSizeCookie == pageSizes[i] ? ' class="active"' : '') + '><a>' + pageSizes[i] + '</a></li>';
                }
                pageSize += '</ul></li>';
                panel.append($(pageSize));      //DOM插入分页Size

                //绑定分页Size事件
                $(_ + '#pageSize_').next().find('li').unbind('click').bind("click", function (e) {
                    setpageSize($(this).text());
                });
            }

            //上页
            if (opts.prev_text && maxentries > 0 && (current_page > 0 || opts.prev_show_always)) {
                appendItem(current_page - 1, {
                    text: opts.prev_text,
                    classes: "prev"
                });
            }

            if (interval[0] > 0 && opts.num_edge_entries > 0) {
                var end = Math.min(opts.num_edge_entries, interval[0]);
                for (var i = 0; i < end; i++) {
                    appendItem(i);
                }
                if (opts.num_edge_entries < interval[0] && opts.ellipse_text) {
                    jQuery("<li><a>" + opts.ellipse_text + "</a></li>").appendTo(panel);
                }
            }

            for (var i = interval[0]; i < interval[1]; i++) {
                appendItem(i);
            }

            if (interval[1] < np && opts.num_edge_entries > 0) {
                if (np - opts.num_edge_entries > interval[1] && opts.ellipse_text) {
                    jQuery("<li><a>" + opts.ellipse_text + "</a></li>").appendTo(panel);
                }
                var begin = Math.max(np - opts.num_edge_entries, interval[1]);
                for (var i = begin; i < np; i++) {
                    appendItem(i);
                }
            }

            //下页
            if (opts.next_text && maxentries > 0 && (current_page < np - 1 || opts.next_show_always)) {
                appendItem(current_page + 1, {
                    text: opts.next_text,
                    classes: "next"
                });
            }
        };
        var current_page = opts.current_page;

        opts.items_per_page = (!opts.items_per_page || opts.items_per_page < 0)
            ? 1
            : opts.items_per_page;

        var panel = jQuery(this);
        //选页
        this.selectPage = function (page_id) {
            pageSelected(page_id);
        };
        //上页
        this.prevPage = function () {
            if (current_page > 0) {
                pageSelected(current_page - 1);
                return true;
            } else {
                return false;
            }
        };
        //下页
        this.nextPage = function () {
            if (current_page < numPages() - 1) {
                pageSelected(current_page + 1);
                return true;
            }
            else {
                return false;
            }
        };

        drawLinks();

        opts.init_apply && opts.callback(current_page, this);
    });
};

(function ($) {
    $.fn.niceTitle = function (options) {
        var opts = $.extend({}, $.fn.niceTitle.defaults, options); var _self = this, _imgAlt = "", _imgTitle = "", _hasImg = false, _imgObj, _winWidth = $(window).width(), _winHeight = $(window).height(), _scrollTop = $(document).scrollTop(), _domHeight = $(document).height(); this.initialize = function (_opts) {
            $(window).scroll(function () { _scrollTop = $(document).scrollTop(); }); var htmlStr = ""; if (jQuery.browser.msie) { htmlStr = '<div id="niceTitle" class="popover">' + '<span>' + '<span class="r1"></span>' + '<span class="r2"></span>' + '<span class="r3"></span>' + '<span class="r4"></span>' + '</span>' + '<div id="niceTitle-ie"><p><em></em></p></div>' + '<span>' + '<span class="r4"></span>' + '<span class="r3"></span>' + '<span class="r2"></span>' + '<span class="r1"></span>' + '</span>' + '</div>'; } else { htmlStr = '<div id="niceTitle" class="popover"><p><em></em></p></div>'; }
            $(_self).mouseover(function (e) {
                if (typeof ($(this).attr('data-title')) != 'undefined') { var tempTitle = $(this).attr('data-title'); } else { if (typeof ($(this).attr('title')) != 'undefined') { $(this).attr('data-title', $(this).attr('title')); var tempTitle = $(this).attr('data-title'); $(this).attr('title', ''); } }
                if (typeof (tempTitle) != 'undefined' && tempTitle != 'undefined' && tempTitle) {
                    if ($(this).is("a")) { this.tmpHref = this.href; } else if ($(this).is("img")) { this.tmpHref = this.src; } else { this.tmpHref = "" }; _imgObj = $(this).find("img"); if (_imgObj.length > 0) {
                        _imgAlt = _imgObj.attr("alt"); _imgObj.attr("alt", ""); _imgTitle = _imgObj.attr("title"); _imgObj.attr("title", ""); _hasImg = true;
                    }
                    var _length = _opts.urlSize; if (tempTitle) { }
                    $(this).attr("title", ""); if (this.tmpHref.length > 0 && _opts.showLink) { this.tmpHref = (this.tmpHref.length > _length ? this.tmpHref.toString().substring(0, _length) + "..." : this.tmpHref); $(htmlStr).appendTo("body").find("p").prepend($(this).data('title')).css({ "color": _opts.titleColor }).find("em").text(this.tmpHref).css({ "color": _opts.urlColor }); } else { $(htmlStr).appendTo("body").find("p").prepend($(this).data('title')).css({ "color": _opts.titleColor }).find("em").remove(); }
                    var obj = $('#niceTitle'); obj.css({ "position": "absolute", "text-align": "left", "padding": "5px", "opacity": _opts.opacity, "top": (_winHeight + _scrollTop - e.pageY - _opts.y) - 10 < obj.height() ? (e.pageY - obj.height() - _opts.y) + "px" : (e.pageY + _opts.y) + "px", "left": (_winWidth - e.pageX - _opts.x) - 10 < obj.width() ? (e.pageX - obj.width() - _opts.x) + "px" : (e.pageX + _opts.x) + "px", "z-index": _opts.zIndex, "max-width": _opts.maxWidth + "px", "width": "auto !important", "min-height": _opts.minHeight + "px", "-moz-border-radius": _opts.radius + "px", "-webkit-border-radius": _opts.radius + "px", "word-wrap": 'break-word', "white-space": 'normal', "word-break": 'break-all' }); if (!jQuery.browser.msie) { obj.css({ "background": _opts.bgColor, "border": "1px solid " + _opts.borColor }); } else { $('#niceTitle span').css({ "background-color": _opts.bgColor, "border": "1px solid " + _opts.borColor }); $('#niceTitle-ie').css({ "background": _opts.bgColor, "border": "1px solid " + _opts.borColor }); }
                    obj.show('fast');
                }
                return false;
            }).mouseout(function (e) {
                $('#niceTitle').remove(); if (_hasImg) {
                    _imgObj.attr("alt", _imgAlt); _imgObj.attr("title", _imgTitle);
                }
                return false;
            }).mousemove(function (e) { var obj = $('#niceTitle'); obj.css({ "top": (_winHeight + _scrollTop - e.pageY - _opts.y) - 10 < obj.height() ? (e.pageY - obj.height() - _opts.y) + "px" : (e.pageY + _opts.y) + "px", "left": (_winWidth - e.pageX - _opts.x) - 10 < obj.width() ? (e.pageX - obj.width() - _opts.x) + "px" : (e.pageX + _opts.x) + "px" }); return false; }); return _self;
        }; this.initialize(opts);
    }; $.fn.niceTitle.defaults = { x: 10, y: 10, urlSize: 30, bgColor: "#fff", borColor: "#CDCDCD", titleColor: "#58666e", urlColor: "#F60", zIndex: 999, maxWidth: 450, minHeight: 30, opacity: 1, radius: 6, showLink: true };
})(jQuery)

; (function ($, window, document, undefined) {
    'use strict'; var pluginName = 'treeview'; var _default = {}; _default.settings = { injectStyle: true, levels: 2, expandIcon: 'glyphicon glyphicon-plus', collapseIcon: 'glyphicon glyphicon-minus', emptyIcon: 'glyphicon', nodeIcon: '', selectedIcon: '', checkedIcon: 'glyphicon-check', uncheckedIcon: 'glyphicon-unchecked', color: undefined, backColor: undefined, borderColor: undefined, onhoverColor: '#F5F5F5', selectedColor: '#428bca', selectedBackColor: '#edf1f2', searchResultColor: '#D9534F', searchResultBackColor: undefined, enableLinks: false, highlightSelected: true, highlightSearchResults: true, showBorder: true, showIcon: true, showCheckbox: false, showTags: false, multiSelect: false, onNodeChecked: undefined, onNodeCollapsed: undefined, onNodeDisabled: undefined, onNodeEnabled: undefined, onNodeExpanded: undefined, onNodeSelected: undefined, onNodeUnchecked: undefined, onNodeUnselected: undefined, onSearchComplete: undefined, onSearchCleared: undefined }; _default.options = { silent: false, ignoreChildren: false }; _default.searchOptions = { ignoreCase: true, exactMatch: false, revealResults: true }; var Tree = function (element, options) { this.$element = $(element); this.elementId = element.id; this.styleId = this.elementId + '-style'; this.init(options); return { options: this.options, init: $.proxy(this.init, this), remove: $.proxy(this.remove, this), getNode: $.proxy(this.getNode, this), getParent: $.proxy(this.getParent, this), getSiblings: $.proxy(this.getSiblings, this), getSelected: $.proxy(this.getSelected, this), getUnselected: $.proxy(this.getUnselected, this), getExpanded: $.proxy(this.getExpanded, this), getCollapsed: $.proxy(this.getCollapsed, this), getChecked: $.proxy(this.getChecked, this), getUnchecked: $.proxy(this.getUnchecked, this), getDisabled: $.proxy(this.getDisabled, this), getEnabled: $.proxy(this.getEnabled, this), selectNode: $.proxy(this.selectNode, this), unselectNode: $.proxy(this.unselectNode, this), toggleNodeSelected: $.proxy(this.toggleNodeSelected, this), collapseAll: $.proxy(this.collapseAll, this), collapseNode: $.proxy(this.collapseNode, this), expandAll: $.proxy(this.expandAll, this), expandNode: $.proxy(this.expandNode, this), toggleNodeExpanded: $.proxy(this.toggleNodeExpanded, this), revealNode: $.proxy(this.revealNode, this), checkAll: $.proxy(this.checkAll, this), checkNode: $.proxy(this.checkNode, this), uncheckAll: $.proxy(this.uncheckAll, this), uncheckNode: $.proxy(this.uncheckNode, this), toggleNodeChecked: $.proxy(this.toggleNodeChecked, this), disableAll: $.proxy(this.disableAll, this), disableNode: $.proxy(this.disableNode, this), enableAll: $.proxy(this.enableAll, this), enableNode: $.proxy(this.enableNode, this), toggleNodeDisabled: $.proxy(this.toggleNodeDisabled, this), search: $.proxy(this.search, this), clearSearch: $.proxy(this.clearSearch, this) }; }; Tree.prototype.init = function (options) {
        this.tree = []; this.nodes = []; if (options.data) {
            if (typeof options.data === 'string') {
                options.data = $.parseJSON(options.data);
            }
            this.tree = $.extend(true, [], options.data); delete options.data;
        }
        this.options = $.extend({}, _default.settings, options); this.destroy(); this.subscribeEvents(); this.setInitialStates({ nodes: this.tree }, 0); this.render();
    }; Tree.prototype.remove = function () { this.destroy(); $.removeData(this, pluginName); $('#' + this.styleId).remove(); }; Tree.prototype.destroy = function () { if (!this.initialized) return; this.$wrapper.remove(); this.$wrapper = null; this.unsubscribeEvents(); this.initialized = false; }; Tree.prototype.unsubscribeEvents = function () { this.$element.off('click'); this.$element.off('nodeChecked'); this.$element.off('nodeCollapsed'); this.$element.off('nodeDisabled'); this.$element.off('nodeEnabled'); this.$element.off('nodeExpanded'); this.$element.off('nodeSelected'); this.$element.off('nodeUnchecked'); this.$element.off('nodeUnselected'); this.$element.off('searchComplete'); this.$element.off('searchCleared'); }; Tree.prototype.subscribeEvents = function () {
        this.unsubscribeEvents(); this.$element.on('click', $.proxy(this.clickHandler, this)); if (typeof (this.options.onNodeChecked) === 'function') {
            this.$element.on('nodeChecked', this.options.onNodeChecked);
        }
        if (typeof (this.options.onNodeCollapsed) === 'function') {
            this.$element.on('nodeCollapsed', this.options.onNodeCollapsed);
        }
        if (typeof (this.options.onNodeDisabled) === 'function') {
            this.$element.on('nodeDisabled', this.options.onNodeDisabled);
        }
        if (typeof (this.options.onNodeEnabled) === 'function') {
            this.$element.on('nodeEnabled', this.options.onNodeEnabled);
        }
        if (typeof (this.options.onNodeExpanded) === 'function') {
            this.$element.on('nodeExpanded', this.options.onNodeExpanded);
        }
        if (typeof (this.options.onNodeSelected) === 'function') {
            this.$element.on('nodeSelected', this.options.onNodeSelected);
        }
        if (typeof (this.options.onNodeUnchecked) === 'function') {
            this.$element.on('nodeUnchecked', this.options.onNodeUnchecked);
        }
        if (typeof (this.options.onNodeUnselected) === 'function') {
            this.$element.on('nodeUnselected', this.options.onNodeUnselected);
        }
        if (typeof (this.options.onSearchComplete) === 'function') {
            this.$element.on('searchComplete', this.options.onSearchComplete);
        }
        if (typeof (this.options.onSearchCleared) === 'function') {
            this.$element.on('searchCleared', this.options.onSearchCleared);
        }
    }; Tree.prototype.setInitialStates = function (node, level) {
        if (!node.nodes) return; level += 1; var parent = node; var _this = this; $.each(node.nodes, function checkStates(index, node) {
            node.nodeId = _this.nodes.length; node.parentId = parent.nodeId; if (!node.hasOwnProperty('selectable')) {
                node.selectable = true;
            }
            node.state = node.state || {}; if (!node.state.hasOwnProperty('checked')) {
                node.state.checked = false;
            }
            if (!node.state.hasOwnProperty('disabled')) {
                node.state.disabled = false;
            }
            if (!node.state.hasOwnProperty('expanded')) {
                if (!node.state.disabled && (level < _this.options.levels) && (node.nodes && node.nodes.length > 0)) {
                    node.state.expanded = true;
                }
                else {
                    node.state.expanded = false;
                }
            }
            if (!node.state.hasOwnProperty('selected')) {
                node.state.selected = false;
            }
            _this.nodes.push(node); if (node.nodes) {
                _this.setInitialStates(node, level);
            }
        });
    }; Tree.prototype.clickHandler = function (event) {
        if (!this.options.enableLinks) event.preventDefault(); var target = $(event.target); var node = this.findNode(target); if (!node || node.state.disabled) return; var classList = target.attr('class') ? target.attr('class').split(' ') : []; if ((classList.indexOf('expand-icon') !== -1)) {
            this.toggleExpandedState(node, _default.options); this.render();
        }
        else if ((classList.indexOf('check-icon') !== -1)) {
            this.toggleCheckedState(node, _default.options); this.render();
        }
        else {
            if (node.selectable) { this.toggleSelectedState(node, _default.options); } else { this.toggleExpandedState(node, _default.options); }
            this.render();
        }
        if (target.hasClass('icon')) {
            event.stopPropagation();
        }
    }; Tree.prototype.findNode = function (target) {
        var nodeId = target.closest('li.list-group-item').attr('data-nodeid'); var node = this.nodes[nodeId]; if (!node) {
            console.log('Error: node does not exist');
        }
        return node;
    }; Tree.prototype.toggleExpandedState = function (node, options) { if (!node) return; this.setExpandedState(node, !node.state.expanded, options); }; Tree.prototype.setExpandedState = function (node, state, options) {
        if (state === node.state.expanded) return; if (state && node.nodes) {
            node.state.expanded = true; if (!options.silent) {
                this.$element.trigger('nodeExpanded', $.extend(true, {}, node));
            }
        }
        else if (!state) {
            node.state.expanded = false; if (!options.silent) {
                this.$element.trigger('nodeCollapsed', $.extend(true, {}, node));
            }
            if (node.nodes && !options.ignoreChildren) {
                $.each(node.nodes, $.proxy(function (index, node) { this.setExpandedState(node, false, options); }, this));
            }
        }
    }; Tree.prototype.toggleSelectedState = function (node, options) { if (!node) return; this.setSelectedState(node, !node.state.selected, options); }; Tree.prototype.setSelectedState = function (node, state, options) {
        if (state === node.state.selected) return; if (state) {
            if (!this.options.multiSelect) {
                $.each(this.findNodes('true', 'g', 'state.selected'), $.proxy(function (index, node) { this.setSelectedState(node, false, options); }, this));
            }
            node.state.selected = true; if (!options.silent) {
                this.$element.trigger('nodeSelected', $.extend(true, {}, node));
            }
        }
        else {
            node.state.selected = false; if (!options.silent) {
                this.$element.trigger('nodeUnselected', $.extend(true, {}, node));
            }
        }
    }; Tree.prototype.toggleCheckedState = function (node, options) { if (!node) return; this.setCheckedState(node, !node.state.checked, options); }; Tree.prototype.setCheckedState = function (node, state, options) {
        if (state === node.state.checked) return; if (state) {
            node.state.checked = true; if (!options.silent) {
                this.$element.trigger('nodeChecked', $.extend(true, {}, node));
            }
        }
        else {
            node.state.checked = false; if (!options.silent) {
                this.$element.trigger('nodeUnchecked', $.extend(true, {}, node));
            }
        }
    }; Tree.prototype.setDisabledState = function (node, state, options) {
        if (state === node.state.disabled) return; if (state) {
            node.state.disabled = true; this.setExpandedState(node, false, options); this.setSelectedState(node, false, options); this.setCheckedState(node, false, options); if (!options.silent) {
                this.$element.trigger('nodeDisabled', $.extend(true, {}, node));
            }
        }
        else {
            node.state.disabled = false; if (!options.silent) {
                this.$element.trigger('nodeEnabled', $.extend(true, {}, node));
            }
        }
    }; Tree.prototype.render = function () {
        if (!this.initialized) {
            this.$element.addClass(pluginName); this.$wrapper = $(this.template.list); this.injectStyle(); this.initialized = true;
        }
        this.$element.empty().append(this.$wrapper.empty()); this.buildTree(this.tree, 0);
    }; Tree.prototype.buildTree = function (nodes, level) {
        if (!nodes) return; level += 1; var _this = this; $.each(nodes, function addNodes(id, node) {
            var treeItem = $(_this.template.item).addClass('node-' + _this.elementId).addClass(node.state.checked ? 'node-checked' : '').addClass(node.state.disabled ? 'node-disabled' : '').addClass(node.state.selected ? 'node-selected' : '').addClass(node.searchResult ? 'search-result' : '').attr('data-nodeid', node.nodeId).attr('data-href', node.href).attr('style', _this.buildStyleOverride(node)); for (var i = 0; i < (level - 1) ; i++) {
                treeItem.append(_this.template.indent);
            }
            var classList = []; if (node.nodes) {
                classList.push('expand-icon'); if (node.state.expanded) {
                    classList.push(_this.options.collapseIcon);
                }
                else {
                    classList.push(_this.options.expandIcon);
                }
            }
            else {
                classList.push(_this.options.emptyIcon);
            }
            treeItem.append($(_this.template.icon).addClass(classList.join(' '))); if (_this.options.showIcon) {
                var classList = ['node-icon']; classList.push(node.icon || _this.options.nodeIcon); if (node.state.selected) {
                    classList.pop(); classList.push(node.selectedIcon || _this.options.selectedIcon || node.icon || _this.options.nodeIcon);
                }
                treeItem.append($(_this.template.icon).addClass(classList.join(' ')));
            }
            if (_this.options.showCheckbox) {
                var classList = ['check-icon']; if (node.state.checked) {
                    classList.push(_this.options.checkedIcon);
                }
                else {
                    classList.push(_this.options.uncheckedIcon);
                }
                treeItem.append($(_this.template.icon).addClass(classList.join(' ')));
            }
            if (_this.options.enableLinks) {
                treeItem.append($(_this.template.link).attr('href', node.href).append(node.text));
            }
            else {
                treeItem.append(node.text);
            }
            if (_this.options.showTags && node.tags) {
                $.each(node.tags, function addTag(id, tag) {
                    treeItem.append($(_this.template.badge).append(tag));
                });
            }
            _this.$wrapper.append(treeItem); treeItem.find('span:not(":last")').css({ 'color': 'rgb(213, 211, 213)' }); if (node.nodes && node.state.expanded && !node.state.disabled) {
                return _this.buildTree(node.nodes, level);
            }
        });
    }; Tree.prototype.buildStyleOverride = function (node) {
        if (node.state.disabled) return ''; var color = node.color; var backColor = node.backColor; if (this.options.highlightSelected && node.state.selected) {
            if (this.options.selectedColor) {
                color = this.options.selectedColor;
            }
            if (this.options.selectedBackColor) {
                backColor = this.options.selectedBackColor;
            }
        }
        if (this.options.highlightSearchResults && node.searchResult && !node.state.disabled) {
            if (this.options.searchResultColor) {
                color = this.options.searchResultColor;
            }
            if (this.options.searchResultBackColor) {
                backColor = this.options.searchResultBackColor;
            }
        }
        return 'color:' + color + ';background-color:' + backColor + ';';
    }; Tree.prototype.injectStyle = function () { if (this.options.injectStyle && !document.getElementById(this.styleId)) { $('<style type="text/css" id="' + this.styleId + '"> ' + this.buildStyle() + ' </style>').appendTo('head'); } }; Tree.prototype.buildStyle = function () {
        var style = '.node-' + this.elementId + '{'; if (this.options.color) {
            style += 'color:' + this.options.color + ';';
        }
        if (this.options.backColor) {
            style += 'background-color:' + this.options.backColor + ';';
        }
        if (!this.options.showBorder) {
            style += 'border:none;';
        }
        else if (this.options.borderColor) {
            style += 'border:1px solid ' + this.options.borderColor + ';';
        }
        style += '}'; if (this.options.onhoverColor) {
            style += '.node-' + this.elementId + ':not(.node-disabled):hover{' + 'background-color:' + this.options.onhoverColor + ';' + '}';
        }
        return this.css + style;
    }; Tree.prototype.template = { list: '<ul class="list-group"></ul>', item: '<li class="list-group-item"></li>', indent: '<span class="indent"></span>', icon: '<span class="icon"></span>', link: '<a href="#" style="color:inherit;"></a>', badge: '<span class="badge"></span>' }; Tree.prototype.css = '.treeview .list-group-item{cursor:pointer}.treeview span.indent{margin-left:10px;margin-right:10px}.treeview span.icon{width:12px;margin-right:2px}.treeview span.check-icon{width:12px;margin-right:3px}.treeview .node-disabled{color:silver;cursor:not-allowed}'
    Tree.prototype.getNode = function (nodeId) { return this.nodes[nodeId]; }; Tree.prototype.getParent = function (identifier) { var node = this.identifyNode(identifier); return this.nodes[node.parentId]; }; Tree.prototype.getSiblings = function (identifier) { var node = this.identifyNode(identifier); var parent = this.getParent(node); var nodes = parent ? parent.nodes : this.tree; return nodes.filter(function (obj) { return obj.nodeId !== node.nodeId; }); }; Tree.prototype.getSelected = function () { return this.findNodes('true', 'g', 'state.selected'); }; Tree.prototype.getUnselected = function () { return this.findNodes('false', 'g', 'state.selected'); }; Tree.prototype.getExpanded = function () { return this.findNodes('true', 'g', 'state.expanded'); }; Tree.prototype.getCollapsed = function () { return this.findNodes('false', 'g', 'state.expanded'); }; Tree.prototype.getChecked = function () { return this.findNodes('true', 'g', 'state.checked'); }; Tree.prototype.getUnchecked = function () { return this.findNodes('false', 'g', 'state.checked'); }; Tree.prototype.getDisabled = function () { return this.findNodes('true', 'g', 'state.disabled'); }; Tree.prototype.getEnabled = function () { return this.findNodes('false', 'g', 'state.disabled'); }; Tree.prototype.selectNode = function (identifiers, options) { this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setSelectedState(node, true, options); }, this)); this.render(); }; Tree.prototype.unselectNode = function (identifiers, options) { this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setSelectedState(node, false, options); }, this)); this.render(); }; Tree.prototype.toggleNodeSelected = function (identifiers, options) { this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.toggleSelectedState(node, options); }, this)); this.render(); }; Tree.prototype.collapseAll = function (options) { var identifiers = this.findNodes('true', 'g', 'state.expanded'); this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setExpandedState(node, false, options); }, this)); this.render(); }; Tree.prototype.collapseNode = function (identifiers, options) { this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setExpandedState(node, false, options); }, this)); this.render(); }; Tree.prototype.expandAll = function (options) {
        options = $.extend({}, _default.options, options); if (options && options.levels) {
            this.expandLevels(this.tree, options.levels, options);
        }
        else {
            var identifiers = this.findNodes('false', 'g', 'state.expanded'); this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setExpandedState(node, true, options); }, this));
        }
        this.render();
    }; Tree.prototype.expandNode = function (identifiers, options) { this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setExpandedState(node, true, options); if (node.nodes && (options && options.levels)) { this.expandLevels(node.nodes, options.levels - 1, options); } }, this)); this.render(); }; Tree.prototype.expandLevels = function (nodes, level, options) { options = $.extend({}, _default.options, options); $.each(nodes, $.proxy(function (index, node) { this.setExpandedState(node, (level > 0) ? true : false, options); if (node.nodes) { this.expandLevels(node.nodes, level - 1, options); } }, this)); }; Tree.prototype.revealNode = function (identifiers, options) { this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { var parentNode = this.getParent(node); while (parentNode) { this.setExpandedState(parentNode, true, options); parentNode = this.getParent(parentNode); }; }, this)); this.render(); }; Tree.prototype.toggleNodeExpanded = function (identifiers, options) { this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.toggleExpandedState(node, options); }, this)); this.render(); }; Tree.prototype.checkAll = function (options) { var identifiers = this.findNodes('false', 'g', 'state.checked'); this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setCheckedState(node, true, options); }, this)); this.render(); }; Tree.prototype.checkNode = function (identifiers, options) { this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setCheckedState(node, true, options); }, this)); this.render(); }; Tree.prototype.uncheckAll = function (options) { var identifiers = this.findNodes('true', 'g', 'state.checked'); this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setCheckedState(node, false, options); }, this)); this.render(); }; Tree.prototype.uncheckNode = function (identifiers, options) { this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setCheckedState(node, false, options); }, this)); this.render(); }; Tree.prototype.toggleNodeChecked = function (identifiers, options) { this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.toggleCheckedState(node, options); }, this)); this.render(); }; Tree.prototype.disableAll = function (options) { var identifiers = this.findNodes('false', 'g', 'state.disabled'); this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setDisabledState(node, true, options); }, this)); this.render(); }; Tree.prototype.disableNode = function (identifiers, options) { this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setDisabledState(node, true, options); }, this)); this.render(); }; Tree.prototype.enableAll = function (options) { var identifiers = this.findNodes('true', 'g', 'state.disabled'); this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setDisabledState(node, false, options); }, this)); this.render(); }; Tree.prototype.enableNode = function (identifiers, options) { this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setDisabledState(node, false, options); }, this)); this.render(); }; Tree.prototype.toggleNodeDisabled = function (identifiers, options) { this.forEachIdentifier(identifiers, options, $.proxy(function (node, options) { this.setDisabledState(node, !node.state.disabled, options); }, this)); this.render(); }; Tree.prototype.forEachIdentifier = function (identifiers, options, callback) {
        options = $.extend({}, _default.options, options); if (!(identifiers instanceof Array)) {
            identifiers = [identifiers];
        }
        $.each(identifiers, $.proxy(function (index, identifier) { callback(this.identifyNode(identifier), options); }, this));
    }; Tree.prototype.identifyNode = function (identifier) { return ((typeof identifier) === 'number') ? this.nodes[identifier] : identifier; }; Tree.prototype.search = function (pattern, options) {
        options = $.extend({}, _default.searchOptions, options); this.clearSearch({ render: false }); var results = []; if (pattern && pattern.length > 0) {
            if (options.exactMatch) {
                pattern = '^' + pattern + '$';
            }
            var modifier = 'g'; if (options.ignoreCase) {
                modifier += 'i';
            }
            results = this.findNodes(pattern, modifier); $.each(results, function (index, node) {
                node.searchResult = true;
            })
        }
        if (options.revealResults) {
            this.revealNode(results);
        }
        else {
            this.render();
        }
        this.$element.trigger('searchComplete', $.extend(true, {}, results)); return results;
    }; Tree.prototype.clearSearch = function (options) {
        options = $.extend({}, { render: true }, options); var results = $.each(this.findNodes('true', 'g', 'searchResult'), function (index, node) { node.searchResult = false; }); if (options.render) {
            this.render();
        }
        this.$element.trigger('searchCleared', $.extend(true, {}, results));
    }; Tree.prototype.findNodes = function (pattern, modifier, attribute) { modifier = modifier || 'g'; attribute = attribute || 'text'; var _this = this; return $.grep(this.nodes, function (node) { var val = _this.getNodeValue(node, attribute); if (typeof val === 'string') { return val.match(new RegExp(pattern, modifier)); } }); }; Tree.prototype.getNodeValue = function (obj, attr) {
        var index = attr.indexOf('.'); if (index > 0) {
            var _obj = obj[attr.substring(0, index)]; var _attr = attr.substring(index + 1, attr.length); return this.getNodeValue(_obj, _attr);
        }
        else {
            if (obj.hasOwnProperty(attr)) {
                return obj[attr].toString();
            }
            else {
                return undefined;
            }
        }
    }; var logError = function (message) { if (window.console) { window.console.error(message); } }; $.fn[pluginName] = function (options, args) {
        var result; this.each(function () {
            var _this = $.data(this, pluginName); if (typeof options === 'string') {
                if (!_this) {
                    logError('Not initialized, can not call method : ' + options);
                }
                else if (!$.isFunction(_this[options]) || options.charAt(0) === '_') {
                    logError('No such method : ' + options);
                }
                else {
                    if (!(args instanceof Array)) {
                        args = [args];
                    }
                    result = _this[options].apply(_this, args);
                }
            }
            else if (typeof options === 'boolean') {
                result = _this;
            }
            else {
                $.data(this, pluginName, new Tree(this, $.extend(true, {}, options)));
            }
        }); return result || this;
    };
})(jQuery, window, document);;
/*!
 * Bootstrap v3.3.0 (http://getbootstrap.com)
 * Copyright 2011-2014 Twitter, Inc.
 * Licensed under MIT (https://github.com/twbs/bootstrap/blob/master/LICENSE)
 */
if (typeof jQuery === 'undefined') {
    throw new Error('Bootstrap\'s JavaScript requires jQuery')
}
+function ($) {
    var version = $.fn.jquery.split(' ')[0].split('.')
    if ((version[0] < 2 && version[1] < 9) || (version[0] == 1 && version[1] == 9 && version[2] < 1)) {
        throw new Error('Bootstrap\'s JavaScript requires jQuery version 1.9.1 or higher')
    }
}(jQuery); +function ($) {
    'use strict'; function transitionEnd() {
        var el = document.createElement('bootstrap')
        var transEndEventNames = { WebkitTransition: 'webkitTransitionEnd', MozTransition: 'transitionend', OTransition: 'oTransitionEnd otransitionend', transition: 'transitionend' }
        for (var name in transEndEventNames) {
            if (el.style[name] !== undefined) {
                return { end: transEndEventNames[name] }
            }
        }
        return false
    }
    $.fn.emulateTransitionEnd = function (duration) {
        var called = false
        var $el = this
        $(this).one('bsTransitionEnd', function () {
            called = true
        })
        var callback = function () {
            if (!called) $($el).trigger($.support.transition.end)
        }
        setTimeout(callback, duration)
        return this
    }
    $(function () {
        $.support.transition = transitionEnd()
        if (!$.support.transition) return
        $.event.special.bsTransitionEnd = {
            bindType: $.support.transition.end, delegateType: $.support.transition.end, handle: function (e) {
                if ($(e.target).is(this)) return e.handleObj.handler.apply(this, arguments)
            }
        }
    })
}(jQuery); +function ($) {
    'use strict';
    var dismiss = '[data-dismiss="alert"]'
    var Alert = function (el) {
        $(el).on('click', dismiss, this.close)
    }
    Alert.VERSION = '3.3.0'
    Alert.TRANSITION_DURATION = 150
    Alert.prototype.close = function (e) {
        var $this = $(this)
        var selector = $this.attr('data-target');
        if (!selector) {
            selector = $this.attr('href');
            selector = selector && selector.replace(/.*(?=#[^\s]*$)/, '');
        }
        var $parent = $(selector)
        if (e)
            e.preventDefault();
        if (!$parent.length) {
            $parent = $this.closest('.alert');
        }
        $parent.trigger(e = $.Event('close.bs.alert'))
        if (e.isDefaultPrevented()) return
        $parent.removeClass('in')
        function removeElement() {
            $parent.detach().trigger('closed.bs.alert').remove();
        }
        $.support.transition && $parent.hasClass('fade') ? $parent.one('bsTransitionEnd', removeElement).emulateTransitionEnd(Alert.TRANSITION_DURATION) : removeElement()
    }
    function Plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('bs.alert')
            if (!data) $this.data('bs.alert', (data = new Alert(this)))
            if (typeof option == 'string') data[option].call($this)
        })
    }
    var old = $.fn.alert
    $.fn.alert = Plugin
    $.fn.alert.Constructor = Alert
    $.fn.alert.noConflict = function () {
        $.fn.alert = old
        return this
    }
    $(document).on('click.bs.alert.data-api', dismiss, Alert.prototype.close)
}(jQuery); +function ($) {
    'use strict'; var Button = function (element, options) {
        this.$element = $(element)
        this.options = $.extend({}, Button.DEFAULTS, options)
        this.isLoading = false
    }
    Button.VERSION = '3.3.0'
    Button.DEFAULTS = { loadingText: 'loading...' }
    Button.prototype.setState = function (state) {
        var d = 'disabled'
        var $el = this.$element
        var val = $el.is('input') ? 'val' : 'html'
        var data = $el.data()
        state = state + 'Text'
        if (data.resetText == null) $el.data('resetText', $el[val]())
        setTimeout($.proxy(function () {
            $el[val](data[state] == null ? this.options[state] : data[state])
            if (state == 'loadingText') {
                this.isLoading = true
                $el.addClass(d).attr(d, d)
            } else if (this.isLoading) {
                this.isLoading = false
                $el.removeClass(d).removeAttr(d)
            }
        }, this), 0)
    }
    Button.prototype.toggle = function () {
        var changed = true
        var $parent = this.$element.closest('[data-toggle="buttons"]')
        if ($parent.length) {
            var $input = this.$element.find('input')
            if ($input.prop('type') == 'radio') {
                if ($input.prop('checked') && this.$element.hasClass('active')) changed = false
                else $parent.find('.active').removeClass('active')
            }
            if (changed) $input.prop('checked', !this.$element.hasClass('active')).trigger('change')
        } else {
            this.$element.attr('aria-pressed', !this.$element.hasClass('active'))
        }
        if (changed) this.$element.toggleClass('active')
    }
    function Plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('bs.button')
            var options = typeof option == 'object' && option
            if (!data) $this.data('bs.button', (data = new Button(this, options)))
            if (option == 'toggle') data.toggle()
            else if (option) data.setState(option)
        })
    }
    var old = $.fn.button
    $.fn.button = Plugin
    $.fn.button.Constructor = Button
    $.fn.button.noConflict = function () {
        $.fn.button = old
        return this
    }
    $(document).on('click.bs.button.data-api', '[data-toggle^="button"]', function (e) {
        var $btn = $(e.target)
        if (!$btn.hasClass('btn')) $btn = $btn.closest('.btn')
        Plugin.call($btn, 'toggle')
        e.preventDefault()
    }).on('focus.bs.button.data-api blur.bs.button.data-api', '[data-toggle^="button"]', function (e) {
        $(e.target).closest('.btn').toggleClass('focus', e.type == 'focus')
    })
}(jQuery); +function ($) {
    'use strict'; var Carousel = function (element, options) {
        this.$element = $(element)
        this.$indicators = this.$element.find('.carousel-indicators')
        this.options = options
        this.paused = this.sliding = this.interval = this.$active = this.$items = null
        this.options.keyboard && this.$element.on('keydown.bs.carousel', $.proxy(this.keydown, this))
        this.options.pause == 'hover' && !('ontouchstart' in document.documentElement) && this.$element.on('mouseenter.bs.carousel', $.proxy(this.pause, this)).on('mouseleave.bs.carousel', $.proxy(this.cycle, this))
    }
    Carousel.VERSION = '3.3.0'
    Carousel.TRANSITION_DURATION = 600
    Carousel.DEFAULTS = { interval: 5000, pause: 'hover', wrap: true, keyboard: true }
    Carousel.prototype.keydown = function (e) {
        switch (e.which) {
            case 37: this.prev(); break
            case 39: this.next(); break
            default: return
        }
        e.preventDefault()
    }
    Carousel.prototype.cycle = function (e) {
        e || (this.paused = false)
        this.interval && clearInterval(this.interval)
        this.options.interval && !this.paused && (this.interval = setInterval($.proxy(this.next, this), this.options.interval))
        return this
    }
    Carousel.prototype.getItemIndex = function (item) {
        this.$items = item.parent().children('.item')
        return this.$items.index(item || this.$active)
    }
    Carousel.prototype.getItemForDirection = function (direction, active) {
        var delta = direction == 'prev' ? -1 : 1
        var activeIndex = this.getItemIndex(active)
        var itemIndex = (activeIndex + delta) % this.$items.length
        return this.$items.eq(itemIndex)
    }
    Carousel.prototype.to = function (pos) {
        var that = this
        var activeIndex = this.getItemIndex(this.$active = this.$element.find('.item.active'))
        if (pos > (this.$items.length - 1) || pos < 0) return
        if (this.sliding) return this.$element.one('slid.bs.carousel', function () {
            that.to(pos)
        })
        if (activeIndex == pos) return this.pause().cycle()
        return this.slide(pos > activeIndex ? 'next' : 'prev', this.$items.eq(pos))
    }
    Carousel.prototype.pause = function (e) {
        e || (this.paused = true)
        if (this.$element.find('.next, .prev').length && $.support.transition) {
            this.$element.trigger($.support.transition.end)
            this.cycle(true)
        }
        this.interval = clearInterval(this.interval)
        return this
    }
    Carousel.prototype.next = function () {
        if (this.sliding) return
        return this.slide('next')
    }
    Carousel.prototype.prev = function () {
        if (this.sliding) return
        return this.slide('prev')
    }
    Carousel.prototype.slide = function (type, next) {
        var $active = this.$element.find('.item.active')
        var $next = next || this.getItemForDirection(type, $active)
        var isCycling = this.interval
        var direction = type == 'next' ? 'left' : 'right'
        var fallback = type == 'next' ? 'first' : 'last'
        var that = this
        if (!$next.length) {
            if (!this.options.wrap) return
            $next = this.$element.find('.item')[fallback]()
        }
        if ($next.hasClass('active')) return (this.sliding = false)
        var relatedTarget = $next[0]
        var slideEvent = $.Event('slide.bs.carousel', {
            relatedTarget: relatedTarget, direction: direction
        })
        this.$element.trigger(slideEvent)
        if (slideEvent.isDefaultPrevented()) return
        this.sliding = true
        isCycling && this.pause()
        if (this.$indicators.length) {
            this.$indicators.find('.active').removeClass('active')
            var $nextIndicator = $(this.$indicators.children()[this.getItemIndex($next)])
            $nextIndicator && $nextIndicator.addClass('active')
        }
        var slidEvent = $.Event('slid.bs.carousel', {
            relatedTarget: relatedTarget, direction: direction
        })
        if ($.support.transition && this.$element.hasClass('slide')) {
            $next.addClass(type)
            $next[0].offsetWidth
            $active.addClass(direction)
            $next.addClass(direction)
            $active.one('bsTransitionEnd', function () {
                $next.removeClass([type, direction].join(' ')).addClass('active')
                $active.removeClass(['active', direction].join(' '))
                that.sliding = false
                setTimeout(function () {
                    that.$element.trigger(slidEvent)
                }, 0)
            }).emulateTransitionEnd(Carousel.TRANSITION_DURATION)
        } else {
            $active.removeClass('active')
            $next.addClass('active')
            this.sliding = false
            this.$element.trigger(slidEvent)
        }
        isCycling && this.cycle()
        return this
    }
    function Plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('bs.carousel')
            var options = $.extend({}, Carousel.DEFAULTS, $this.data(), typeof option == 'object' && option)
            var action = typeof option == 'string' ? option : options.slide
            if (!data) $this.data('bs.carousel', (data = new Carousel(this, options)))
            if (typeof option == 'number') data.to(option)
            else if (action) data[action]()
            else if (options.interval) data.pause().cycle()
        })
    }
    var old = $.fn.carousel
    $.fn.carousel = Plugin
    $.fn.carousel.Constructor = Carousel
    $.fn.carousel.noConflict = function () {
        $.fn.carousel = old
        return this
    }
    var clickHandler = function (e) {
        var href
        var $this = $(this)
        var $target = $($this.attr('data-target') || (href = $this.attr('href')) && href.replace(/.*(?=#[^\s]+$)/, ''))
        if (!$target.hasClass('carousel')) return
        var options = $.extend({}, $target.data(), $this.data())
        var slideIndex = $this.attr('data-slide-to')
        if (slideIndex) options.interval = false
        Plugin.call($target, options)
        if (slideIndex) {
            $target.data('bs.carousel').to(slideIndex)
        }
        e.preventDefault()
    }
    $(document).on('click.bs.carousel.data-api', '[data-slide]', clickHandler).on('click.bs.carousel.data-api', '[data-slide-to]', clickHandler)
    $(window).on('load', function () {
        $('[data-ride="carousel"]').each(function () {
            var $carousel = $(this)
            Plugin.call($carousel, $carousel.data())
        })
    })
}(jQuery); +function ($) {
    'use strict'; var Collapse = function (element, options) {
        this.$element = $(element)
        this.options = $.extend({}, Collapse.DEFAULTS, options)
        this.$trigger = $(this.options.trigger).filter('[href="#' + element.id + '"], [data-target="#' + element.id + '"]')
        this.transitioning = null
        if (this.options.parent) { this.$parent = this.getParent() } else { this.addAriaAndCollapsedClass(this.$element, this.$trigger) }
        if (this.options.toggle) this.toggle()
    }
    Collapse.VERSION = '3.3.0'
    Collapse.TRANSITION_DURATION = 350
    Collapse.DEFAULTS = { toggle: true, trigger: '[data-toggle="collapse"]' }
    Collapse.prototype.dimension = function () {
        var hasWidth = this.$element.hasClass('width')
        return hasWidth ? 'width' : 'height'
    }
    Collapse.prototype.show = function () {
        if (this.transitioning || this.$element.hasClass('in')) return
        var activesData
        var actives = this.$parent && this.$parent.find('> .panel').children('.in, .collapsing')
        if (actives && actives.length) {
            activesData = actives.data('bs.collapse')
            if (activesData && activesData.transitioning) return
        }
        var startEvent = $.Event('show.bs.collapse')
        this.$element.trigger(startEvent)
        if (startEvent.isDefaultPrevented()) return
        if (actives && actives.length) {
            Plugin.call(actives, 'hide')
            activesData || actives.data('bs.collapse', null)
        }
        var dimension = this.dimension()
        this.$element.removeClass('collapse').addClass('collapsing')[dimension](0).attr('aria-expanded', true)
        this.$trigger.removeClass('collapsed').attr('aria-expanded', true)
        this.transitioning = 1
        var complete = function () {
            this.$element.removeClass('collapsing').addClass('collapse in')[dimension]('')
            this.transitioning = 0
            this.$element.trigger('shown.bs.collapse')
        }
        if (!$.support.transition) return complete.call(this)
        var scrollSize = $.camelCase(['scroll', dimension].join('-'))
        this.$element.one('bsTransitionEnd', $.proxy(complete, this)).emulateTransitionEnd(Collapse.TRANSITION_DURATION)[dimension](this.$element[0][scrollSize])
    }
    Collapse.prototype.hide = function () {
        if (this.transitioning || !this.$element.hasClass('in')) return
        var startEvent = $.Event('hide.bs.collapse')
        this.$element.trigger(startEvent)
        if (startEvent.isDefaultPrevented()) return
        var dimension = this.dimension()
        this.$element[dimension](this.$element[dimension]())[0].offsetHeight
        this.$element.addClass('collapsing').removeClass('collapse in').attr('aria-expanded', false)
        this.$trigger.addClass('collapsed').attr('aria-expanded', false)
        this.transitioning = 1
        var complete = function () {
            this.transitioning = 0
            this.$element.removeClass('collapsing').addClass('collapse').trigger('hidden.bs.collapse')
        }
        if (!$.support.transition) return complete.call(this)
        this.$element
        [dimension](0).one('bsTransitionEnd', $.proxy(complete, this)).emulateTransitionEnd(Collapse.TRANSITION_DURATION)
    }
    Collapse.prototype.toggle = function () {
        this[this.$element.hasClass('in') ? 'hide' : 'show']()
    }
    Collapse.prototype.getParent = function () {
        return $(this.options.parent).find('[data-toggle="collapse"][data-parent="' + this.options.parent + '"]').each($.proxy(function (i, element) {
            var $element = $(element)
            this.addAriaAndCollapsedClass(getTargetFromTrigger($element), $element)
        }, this)).end()
    }
    Collapse.prototype.addAriaAndCollapsedClass = function ($element, $trigger) {
        var isOpen = $element.hasClass('in')
        $element.attr('aria-expanded', isOpen)
        $trigger.toggleClass('collapsed', !isOpen).attr('aria-expanded', isOpen)
    }
    function getTargetFromTrigger($trigger) {
        var href
        var target = $trigger.attr('data-target') || (href = $trigger.attr('href')) && href.replace(/.*(?=#[^\s]+$)/, '')
        return $(target)
    }
    function Plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('bs.collapse')
            var options = $.extend({}, Collapse.DEFAULTS, $this.data(), typeof option == 'object' && option)
            if (!data && options.toggle && option == 'show') options.toggle = false
            if (!data) $this.data('bs.collapse', (data = new Collapse(this, options)))
            if (typeof option == 'string') data[option]()
        })
    }
    var old = $.fn.collapse
    $.fn.collapse = Plugin
    $.fn.collapse.Constructor = Collapse
    $.fn.collapse.noConflict = function () {
        $.fn.collapse = old
        return this
    }
    $(document).on('click.bs.collapse.data-api', '[data-toggle="collapse"]', function (e) {
        var $this = $(this)
        if (!$this.attr('data-target')) e.preventDefault()
        var $target = getTargetFromTrigger($this)
        var data = $target.data('bs.collapse')
        var option = data ? 'toggle' : $.extend({}, $this.data(), {
            trigger: this
        })
        Plugin.call($target, option)
    })
}(jQuery); +function ($) {
    'use strict'; var backdrop = '.dropdown-backdrop'
    var toggle = '[data-toggle="dropdown"]'
    var Dropdown = function (element) { $(element).on('click.bs.dropdown', this.toggle) }
    Dropdown.VERSION = '3.3.0'
    Dropdown.prototype.toggle = function (e) {
        var $this = $(this)
        if ($this.is('.disabled, :disabled')) return
        var $parent = getParent($this)
        var isActive = $parent.hasClass('open')
        clearMenus()
        if (!isActive) {
            if ('ontouchstart' in document.documentElement && !$parent.closest('.navbar-nav').length) { $('<div class="dropdown-backdrop"/>').insertAfter($(this)).on('click', clearMenus) }
            var relatedTarget = { relatedTarget: this }
            $parent.trigger(e = $.Event('show.bs.dropdown', relatedTarget))
            if (e.isDefaultPrevented()) return
            $this.trigger('focus').attr('aria-expanded', 'true')
            $parent.toggleClass('open').trigger('shown.bs.dropdown', relatedTarget)
        }
        return false
    }
    Dropdown.prototype.keydown = function (e) {
        if (!/(38|40|27|32)/.test(e.which)) return
        var $this = $(this)
        e.preventDefault()
        e.stopPropagation()
        if ($this.is('.disabled, :disabled')) return
        var $parent = getParent($this)
        var isActive = $parent.hasClass('open')
        if ((!isActive && e.which != 27) || (isActive && e.which == 27)) {
            if (e.which == 27) $parent.find(toggle).trigger('focus')
            return $this.trigger('click')
        }
        var desc = ' li:not(.divider):visible a'
        var $items = $parent.find('[role="menu"]' + desc + ', [role="listbox"]' + desc)
        if (!$items.length) return
        var index = $items.index(e.target)
        if (e.which == 38 && index > 0) index--
        if (e.which == 40 && index < $items.length - 1) index++
        if (!~index) index = 0
        $items.eq(index).trigger('focus')
    }
    function clearMenus(e) {
        if (e && e.which === 3) return
        $(backdrop).remove()
        $(toggle).each(function () {
            var $this = $(this)
            var $parent = getParent($this)
            var relatedTarget = { relatedTarget: this }
            if (!$parent.hasClass('open')) return
            $parent.trigger(e = $.Event('hide.bs.dropdown', relatedTarget))
            if (e.isDefaultPrevented()) return
            $this.attr('aria-expanded', 'false')
            $parent.removeClass('open').trigger('hidden.bs.dropdown', relatedTarget)
        })
    }
    function getParent($this) {
        var selector = $this.attr('data-target')
        if (!selector) {
            selector = $this.attr('href')
            selector = selector && /#[A-Za-z]/.test(selector) && selector.replace(/.*(?=#[^\s]*$)/, '')
        }
        var $parent = selector && $(selector)
        return $parent && $parent.length ? $parent : $this.parent()
    }
    function Plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('bs.dropdown')
            if (!data) $this.data('bs.dropdown', (data = new Dropdown(this)))
            if (typeof option == 'string') data[option].call($this)
        })
    }
    var old = $.fn.dropdown
    $.fn.dropdown = Plugin
    $.fn.dropdown.Constructor = Dropdown
    $.fn.dropdown.noConflict = function () {
        $.fn.dropdown = old
        return this
    }
    $(document).on('click.bs.dropdown.data-api', clearMenus).on('click.bs.dropdown.data-api', '.dropdown form', function (e) { e.stopPropagation() }).on('click.bs.dropdown.data-api', toggle, Dropdown.prototype.toggle).on('keydown.bs.dropdown.data-api', toggle, Dropdown.prototype.keydown).on('keydown.bs.dropdown.data-api', '[role="menu"]', Dropdown.prototype.keydown).on('keydown.bs.dropdown.data-api', '[role="listbox"]', Dropdown.prototype.keydown)
}(jQuery);

//Modal
+function ($) {
    'use strict';
    var Modal = function (element, options) {
        this.options = options
        this.$body = $(document.body)
        this.$element = $(element)
        this.$backdrop = this.isShown = null
        this.scrollbarWidth = 0
        if (this.options.remote) {
            this.$element.find('.modal-content').load(this.options.remote, $.proxy(function () { this.$element.trigger('loaded.bs.modal') }, this))
        }
    }
    Modal.VERSION = '3.3.0'
    Modal.TRANSITION_DURATION = 300
    Modal.BACKDROP_TRANSITION_DURATION = 150
    Modal.DEFAULTS = { backdrop: true, keyboard: true, show: true, showBodyScroll: true, }
    Modal.prototype.toggle = function (_relatedTarget) {
        return this.isShown ? this.hide() : this.show(_relatedTarget)
    }
    
    //显示弹出层
    Modal.prototype.show = function (_relatedTarget) {
        
        var that = this;
        var e = $.Event('show.bs.modal', {
            relatedTarget: _relatedTarget
        })
        this.$element.trigger(e);
        if (this.isShown || e.isDefaultPrevented())
            return;
        this.isShown = true;
        this.checkScrollbar();
        this.setScrollbar();
        this.escape();
        this.$element.on('click.dismiss.bs.modal', '[data-dismiss="modal"]', $.proxy(this.hide, this));
        this.backdrop(function () {
            var transition = $.support.transition && that.$element.hasClass('fade');
            if (!that.$element.parent().length) {
                that.$element.appendTo(that.$body);
            }
            that.$element.show().scrollTop(0)
            if (transition) {
                that.$element[0].offsetWidth;
            }
            that.$element.addClass('in').attr('aria-hidden', false);
            that.enforceFocus();
            var e = $.Event('shown.bs.modal', {
                relatedTarget: _relatedTarget
            });
            transition
            ? that.$element.find('.modal-dialog').one('bsTransitionEnd', function () { that.$element.trigger('focus').trigger(e) }).emulateTransitionEnd(Modal.TRANSITION_DURATION)
            : that.$element.trigger('focus').trigger(e);
        });
        if (!that.options.showBodyScroll) {
            $(document.body).css({ "overflow-x": "hidden", "overflow-y": "hidden" });
        }
    }
    //关闭弹出层
    Modal.prototype.hide = function (e) {
        
        if (this.$element.attr("id") == "dialog-shopmanager-set") {
            return false;
        }else {
            if (e)
                e.preventDefault()
            e = $.Event('hide.bs.modal')
            this.$element.trigger(e)
            if (!this.isShown || e.isDefaultPrevented())
                return;
            this.isShown = false;
            this.escape()
            $(document).off('focusin.bs.modal');
            this.$element.removeClass('in').attr('aria-hidden', true).off('click.dismiss.bs.modal');

            $.support.transition && this.$element.hasClass('fade')
            ? this.$element.one('bsTransitionEnd', $.proxy(this.hideModal, this)).emulateTransitionEnd(Modal.TRANSITION_DURATION)
            : this.hideModal();
        }
        
    }
    Modal.prototype.enforceFocus = function () {
        $(document).off('focusin.bs.modal').on('focusin.bs.modal', $.proxy(function (e) { if (this.$element[0] !== e.target && !this.$element.has(e.target).length) { this.$element.trigger('focus') } }, this))
    }
    Modal.prototype.escape = function () {
        
        if (this.isShown && this.options.keyboard) { this.$element.on('keydown.dismiss.bs.modal', $.proxy(function (e) { e.which == 27 && this.hide() }, this)) } else if (!this.isShown) {
            this.$element.off('keydown.dismiss.bs.modal')
        }
    }
    Modal.prototype.hideModal = function () {
        
        var that = this
        this.$element.hide()
        this.backdrop(function () {
            that.resetScrollbar()
            that.$element.trigger('hidden.bs.modal')
        })
        if (!that.options.showBodyScroll) { $(document.body).css({ "overflow-x": "auto", "overflow-y": "auto" }); }
    }
    Modal.prototype.removeBackdrop = function () {
        this.$backdrop && this.$backdrop.remove()
        this.$backdrop = null
    }
    Modal.prototype.backdrop = function (callback) {
        var that = this
        var animate = this.$element.hasClass('fade') ? 'fade' : ''
        if (this.isShown && this.options.backdrop) {
            
            var doAnimate = $.support.transition && animate
            this.$backdrop = $('<div class="modal-backdrop ' + animate + '" />').prependTo(this.$element)
            //    .on('click.dismiss.bs.modal', $.proxy(function (e) {
            //    if (e.target !== e.currentTarget) return
            //    this.options.backdrop == 'static' ? this.$element[0].focus.call(this.$element[0]) : this.hide.call(this)
            //}, this))
            if (doAnimate) this.$backdrop[0].offsetWidth
            this.$backdrop.addClass('in')
            if (!callback) return
            doAnimate ? this.$backdrop.one('bsTransitionEnd', callback).emulateTransitionEnd(Modal.BACKDROP_TRANSITION_DURATION) : callback()
        } else if (!this.isShown && this.$backdrop) {
            this.$backdrop.removeClass('in')
            var callbackRemove = function () {
                that.removeBackdrop()
                callback && callback()
            }
            $.support.transition && this.$element.hasClass('fade') ? this.$backdrop.one('bsTransitionEnd', callbackRemove).emulateTransitionEnd(Modal.BACKDROP_TRANSITION_DURATION) : callbackRemove()
        } else if (callback) {
            callback()
        }
    }
    Modal.prototype.checkScrollbar = function () {
        this.scrollbarWidth = this.measureScrollbar()
    }
    Modal.prototype.setScrollbar = function () { }
    Modal.prototype.resetScrollbar = function () { }
    Modal.prototype.measureScrollbar = function () {
        if (document.body.clientWidth >= window.innerWidth) return 0
        var scrollDiv = document.createElement('div')
        scrollDiv.className = 'modal-scrollbar-measure'
        this.$body.append(scrollDiv)
        var scrollbarWidth = scrollDiv.offsetWidth - scrollDiv.clientWidth
        this.$body[0].removeChild(scrollDiv)
        return scrollbarWidth
    }
    function Plugin(option, _relatedTarget) {
        return this.each(function () {
            var $this = $(this);
            var data = $this.data('bs.modal');
            //;
            var options = $.extend({}, Modal.DEFAULTS, $this.data(), typeof option == 'object' && option)
            if (!data)
                $this.data('bs.modal', (data = new Modal(this, options)));
            if (typeof option == 'string')
                data[option](_relatedTarget)
            else if (options.show)
                data.show(_relatedTarget)
        })
    }
    var old = $.fn.modal
    $.fn.modal = Plugin
    $.fn.modal.Constructor = Modal
    $.fn.modal.noConflict = function () {
        $.fn.modal = old
        return this
    }
    $(document).on('click.bs.modal.data-api', '[data-toggle="modal"]', function (e) {
        var $this = $(this)
        var href = $this.attr('href')
        var $target = $($this.attr('data-target') || (href && href.replace(/.*(?=#[^\s]+$)/, '')))
        var option = $target.data('bs.modal') ? 'toggle' : $.extend({ remote: !/#/.test(href) && href }, $target.data(), $this.data())
        if ($this.is('a')) e.preventDefault()
        $target.one('show.bs.modal', function (showEvent) {
            if (showEvent.isDefaultPrevented()) return
            $target.one('hidden.bs.modal', function () {
                $this.is(':visible') && $this.trigger('focus')
            })
        })
        Plugin.call($target, option, this)
    })
}(jQuery);

//Tooltip
+function ($) {
    'use strict'; var Tooltip = function (element, options) {
        this.type = this.options = this.enabled = this.timeout = this.hoverState = this.$element = null
        this.init('tooltip', element, options)
    }
    Tooltip.VERSION = '3.3.0'
    Tooltip.TRANSITION_DURATION = 150
    Tooltip.DEFAULTS = { animation: true, placement: 'top', selector: false, template: '<div class="tooltip" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>', trigger: 'hover focus', title: '', delay: 0, html: false, container: false, viewport: { selector: 'body', padding: 0 } }
    Tooltip.prototype.init = function (type, element, options) {
        this.enabled = true
        this.type = type
        this.$element = $(element)
        this.options = this.getOptions(options)
        this.$viewport = this.options.viewport && $(this.options.viewport.selector || this.options.viewport)
        var triggers = this.options.trigger.split(' ')
        for (var i = triggers.length; i--;) {
            var trigger = triggers[i]
            if (trigger == 'click') { this.$element.on('click.' + this.type, this.options.selector, $.proxy(this.toggle, this)) } else if (trigger != 'manual') {
                var eventIn = trigger == 'hover' ? 'mouseenter' : 'focusin'
                var eventOut = trigger == 'hover' ? 'mouseleave' : 'focusout'
                this.$element.on(eventIn + '.' + this.type, this.options.selector, $.proxy(this.enter, this))
                this.$element.on(eventOut + '.' + this.type, this.options.selector, $.proxy(this.leave, this))
            }
        }
        this.options.selector ? (this._options = $.extend({}, this.options, { trigger: 'manual', selector: '' })) : this.fixTitle()
    }
    Tooltip.prototype.getDefaults = function () {
        return Tooltip.DEFAULTS
    }
    Tooltip.prototype.getOptions = function (options) {
        options = $.extend({}, this.getDefaults(), this.$element.data(), options)
        if (options.delay && typeof options.delay == 'number') {
            options.delay = { show: options.delay, hide: options.delay }
        }
        return options
    }
    Tooltip.prototype.getDelegateOptions = function () {
        var options = {}
        var defaults = this.getDefaults()
        this._options && $.each(this._options, function (key, value) { if (defaults[key] != value) options[key] = value })
        return options
    }
    Tooltip.prototype.enter = function (obj) {
        var self = obj instanceof this.constructor ? obj : $(obj.currentTarget).data('bs.' + this.type)
        if (self && self.$tip && self.$tip.is(':visible')) {
            self.hoverState = 'in'
            return
        }
        if (!self) {
            self = new this.constructor(obj.currentTarget, this.getDelegateOptions())
            $(obj.currentTarget).data('bs.' + this.type, self)
        }
        clearTimeout(self.timeout)
        self.hoverState = 'in'
        if (!self.options.delay || !self.options.delay.show) return self.show()
        self.timeout = setTimeout(function () { if (self.hoverState == 'in') self.show() }, self.options.delay.show)
    }
    Tooltip.prototype.leave = function (obj) {
        var self = obj instanceof this.constructor ? obj : $(obj.currentTarget).data('bs.' + this.type)
        if (!self) {
            self = new this.constructor(obj.currentTarget, this.getDelegateOptions())
            $(obj.currentTarget).data('bs.' + this.type, self)
        }
        clearTimeout(self.timeout)
        self.hoverState = 'out'
        if (!self.options.delay || !self.options.delay.hide) return self.hide()
        self.timeout = setTimeout(function () { if (self.hoverState == 'out') self.hide() }, self.options.delay.hide)
    }
    Tooltip.prototype.show = function () {
        var e = $.Event('show.bs.' + this.type)
        if (this.hasContent() && this.enabled) {
            this.$element.trigger(e)
            var inDom = $.contains(this.$element[0].ownerDocument.documentElement, this.$element[0])
            if (e.isDefaultPrevented() || !inDom) return
            var that = this
            var $tip = this.tip()
            var tipId = this.getUID(this.type)
            this.setContent()
            $tip.attr('id', tipId)
            this.$element.attr('aria-describedby', tipId)
            if (this.options.animation) $tip.addClass('fade')
            var placement = typeof this.options.placement == 'function' ? this.options.placement.call(this, $tip[0], this.$element[0]) : this.options.placement
            var autoToken = /\s?auto?\s?/i
            var autoPlace = autoToken.test(placement)
            if (autoPlace) placement = placement.replace(autoToken, '') || 'top'
            $tip.detach().css({ top: 0, left: 0, display: 'block' }).addClass(placement).data('bs.' + this.type, this)
            this.options.container ? $tip.appendTo(this.options.container) : $tip.insertAfter(this.$element)
            var pos = this.getPosition()
            var actualWidth = $tip[0].offsetWidth
            var actualHeight = $tip[0].offsetHeight
            if (autoPlace) {
                var orgPlacement = placement
                var $container = this.options.container ? $(this.options.container) : this.$element.parent()
                var containerDim = this.getPosition($container)
                placement = placement == 'bottom' && pos.bottom + actualHeight > containerDim.bottom ? 'top' : placement == 'top' && pos.top - actualHeight < containerDim.top ? 'bottom' : placement == 'right' && pos.right + actualWidth > containerDim.width ? 'left' : placement == 'left' && pos.left - actualWidth < containerDim.left ? 'right' : placement
                $tip.removeClass(orgPlacement).addClass(placement)
            }
            var calculatedOffset = this.getCalculatedOffset(placement, pos, actualWidth, actualHeight)
            this.applyPlacement(calculatedOffset, placement)
            var complete = function () {
                var prevHoverState = that.hoverState
                that.$element.trigger('shown.bs.' + that.type)
                that.hoverState = null
                if (prevHoverState == 'out') that.leave(that)
            }
            $.support.transition && this.$tip.hasClass('fade') ? $tip.one('bsTransitionEnd', complete).emulateTransitionEnd(Tooltip.TRANSITION_DURATION) : complete()
        }
    }
    Tooltip.prototype.applyPlacement = function (offset, placement) {
        var $tip = this.tip()
        var width = $tip[0].offsetWidth
        var height = $tip[0].offsetHeight
        var marginTop = parseInt($tip.css('margin-top'), 10)
        var marginLeft = parseInt($tip.css('margin-left'), 10)
        if (isNaN(marginTop)) marginTop = 0
        if (isNaN(marginLeft)) marginLeft = 0
        offset.top = offset.top + marginTop
        offset.left = offset.left + marginLeft
        $.offset.setOffset($tip[0], $.extend({ using: function (props) { $tip.css({ top: Math.round(props.top), left: Math.round(props.left) }) } }, offset), 0)
        $tip.addClass('in')
        var actualWidth = $tip[0].offsetWidth
        var actualHeight = $tip[0].offsetHeight
        if (placement == 'top' && actualHeight != height) {
            offset.top = offset.top + height - actualHeight
        }
        var delta = this.getViewportAdjustedDelta(placement, offset, actualWidth, actualHeight)
        if (delta.left) offset.left += delta.left
        else offset.top += delta.top
        var isVertical = /top|bottom/.test(placement)
        var arrowDelta = isVertical ? delta.left * 2 - width + actualWidth : delta.top * 2 - height + actualHeight
        var arrowOffsetPosition = isVertical ? 'offsetWidth' : 'offsetHeight'
        $tip.offset(offset)
        this.replaceArrow(arrowDelta, $tip[0][arrowOffsetPosition], isVertical)
    }
    Tooltip.prototype.replaceArrow = function (delta, dimension, isHorizontal) {
        this.arrow().css(isHorizontal ? 'left' : 'top', 50 * (1 - delta / dimension) + '%').css(isHorizontal ? 'top' : 'left', '')
    }
    Tooltip.prototype.setContent = function () {
        var $tip = this.tip()
        var title = this.getTitle()
        $tip.find('.tooltip-inner')[this.options.html ? 'html' : 'text'](title)
        $tip.removeClass('fade in top bottom left right')
    }
    Tooltip.prototype.hide = function (callback) {
        var that = this
        var $tip = this.tip()
        var e = $.Event('hide.bs.' + this.type)
        function complete() {
            if (that.hoverState != 'in') $tip.detach()
            that.$element.removeAttr('aria-describedby').trigger('hidden.bs.' + that.type)
            callback && callback()
        }
        this.$element.trigger(e)
        if (e.isDefaultPrevented()) return
        $tip.removeClass('in')
        $.support.transition && this.$tip.hasClass('fade') ? $tip.one('bsTransitionEnd', complete).emulateTransitionEnd(Tooltip.TRANSITION_DURATION) : complete()
        this.hoverState = null
        return this
    }
    Tooltip.prototype.fixTitle = function () {
        var $e = this.$element
        if ($e.attr('title') || typeof ($e.attr('data-original-title')) != 'string') { $e.attr('data-original-title', $e.attr('title') || '').attr('title', '') }
    }
    Tooltip.prototype.hasContent = function () {
        return this.getTitle()
    }
    Tooltip.prototype.getPosition = function ($element) {
        $element = $element || this.$element
        var el = $element[0]
        var isBody = el.tagName == 'BODY'
        var elRect = el.getBoundingClientRect()
        if (elRect.width == null) {
            elRect = $.extend({}, elRect, {
                width: elRect.right - elRect.left, height: elRect.bottom - elRect.top
            })
        }
        var elOffset = isBody ? { top: 0, left: 0 } : $element.offset()
        var scroll = { scroll: isBody ? document.documentElement.scrollTop || document.body.scrollTop : $element.scrollTop() }
        var outerDims = isBody ? { width: $(window).width(), height: $(window).height() } : null
        return $.extend({}, elRect, scroll, outerDims, elOffset)
    }
    Tooltip.prototype.getCalculatedOffset = function (placement, pos, actualWidth, actualHeight) {
        return placement == 'bottom' ? { top: pos.top + pos.height, left: pos.left + pos.width / 2 - actualWidth / 2 } : placement == 'top' ? { top: pos.top - actualHeight, left: pos.left + pos.width / 2 - actualWidth / 2 } : placement == 'left' ? { top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left - actualWidth } : { top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left + pos.width }
    }
    Tooltip.prototype.getViewportAdjustedDelta = function (placement, pos, actualWidth, actualHeight) {
        var delta = {
            top: 0, left: 0
        }
        if (!this.$viewport) return delta
        var viewportPadding = this.options.viewport && this.options.viewport.padding || 0
        var viewportDimensions = this.getPosition(this.$viewport)
        if (/right|left/.test(placement)) {
            var topEdgeOffset = pos.top - viewportPadding - viewportDimensions.scroll
            var bottomEdgeOffset = pos.top + viewportPadding - viewportDimensions.scroll + actualHeight
            if (topEdgeOffset < viewportDimensions.top) { delta.top = viewportDimensions.top - topEdgeOffset } else if (bottomEdgeOffset > viewportDimensions.top + viewportDimensions.height) {
                delta.top = viewportDimensions.top + viewportDimensions.height - bottomEdgeOffset
            }
        } else {
            var leftEdgeOffset = pos.left - viewportPadding
            var rightEdgeOffset = pos.left + viewportPadding + actualWidth
            if (leftEdgeOffset < viewportDimensions.left) { delta.left = viewportDimensions.left - leftEdgeOffset } else if (rightEdgeOffset > viewportDimensions.width) {
                delta.left = viewportDimensions.left + viewportDimensions.width - rightEdgeOffset
            }
        }
        return delta
    }
    Tooltip.prototype.getTitle = function () {
        var title
        var $e = this.$element
        var o = this.options
        title = $e.attr('data-original-title') || (typeof o.title == 'function' ? o.title.call($e[0]) : o.title)
        return title
    }
    Tooltip.prototype.getUID = function (prefix) {
        do prefix += ~~(Math.random() * 1000000)
        while (document.getElementById(prefix))
        return prefix
    }
    Tooltip.prototype.tip = function () {
        return (this.$tip = this.$tip || $(this.options.template))
    }
    Tooltip.prototype.arrow = function () {
        return (this.$arrow = this.$arrow || this.tip().find('.tooltip-arrow'))
    }
    Tooltip.prototype.enable = function () {
        this.enabled = true
    }
    Tooltip.prototype.disable = function () {
        this.enabled = false
    }
    Tooltip.prototype.toggleEnabled = function () {
        this.enabled = !this.enabled
    }
    Tooltip.prototype.toggle = function (e) {
        var self = this
        if (e) {
            self = $(e.currentTarget).data('bs.' + this.type)
            if (!self) {
                self = new this.constructor(e.currentTarget, this.getDelegateOptions())
                $(e.currentTarget).data('bs.' + this.type, self)
            }
        }
        self.tip().hasClass('in') ? self.leave(self) : self.enter(self)
    }
    Tooltip.prototype.destroy = function () {
        var that = this
        clearTimeout(this.timeout)
        this.hide(function () {
            that.$element.off('.' + that.type).removeData('bs.' + that.type)
        })
    }
    function Plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('bs.tooltip')
            var options = typeof option == 'object' && option
            var selector = options && options.selector
            if (!data && option == 'destroy') return
            if (selector) {
                if (!data) $this.data('bs.tooltip', (data = {}))
                if (!data[selector]) data[selector] = new Tooltip(this, options)
            } else {
                if (!data) $this.data('bs.tooltip', (data = new Tooltip(this, options)))
            }
            if (typeof option == 'string') data[option]()
        })
    }
    var old = $.fn.tooltip
    $.fn.tooltip = Plugin
    $.fn.tooltip.Constructor = Tooltip
    $.fn.tooltip.noConflict = function () {
        $.fn.tooltip = old
        return this
    }
}(jQuery);

//Popover
+function ($) {
    'use strict'; var Popover = function (element, options) {
        this.init('popover', element, options)
    }
    if (!$.fn.tooltip) throw new Error('Popover requires tooltip.js')
    Popover.VERSION = '3.3.0'
    Popover.DEFAULTS = $.extend({}, $.fn.tooltip.Constructor.DEFAULTS, {
        placement: 'right', trigger: 'click', content: '', template: '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'
    })
    Popover.prototype = $.extend({}, $.fn.tooltip.Constructor.prototype)
    Popover.prototype.constructor = Popover
    Popover.prototype.getDefaults = function () {
        return Popover.DEFAULTS
    }
    Popover.prototype.setContent = function () {
        var $tip = this.tip()
        var title = this.getTitle()
        var content = this.getContent()
        $tip.find('.popover-title')[this.options.html ? 'html' : 'text'](title)
        $tip.find('.popover-content').children().detach().end()[this.options.html ? (typeof content == 'string' ? 'html' : 'append') : 'text'](content)
        $tip.removeClass('fade top bottom left right in')
        if (!$tip.find('.popover-title').html()) $tip.find('.popover-title').hide()
    }
    Popover.prototype.hasContent = function () {
        return this.getTitle() || this.getContent()
    }
    Popover.prototype.getContent = function () {
        var $e = this.$element
        var o = this.options
        return $e.attr('data-content') || (typeof o.content == 'function' ? o.content.call($e[0]) : o.content)
    }
    Popover.prototype.arrow = function () {
        return (this.$arrow = this.$arrow || this.tip().find('.arrow'))
    }
    Popover.prototype.tip = function () {
        if (!this.$tip) this.$tip = $(this.options.template)
        return this.$tip
    }
    function Plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('bs.popover')
            var options = typeof option == 'object' && option
            var selector = options && options.selector
            if (!data && option == 'destroy') return
            if (selector) {
                if (!data) $this.data('bs.popover', (data = {}))
                if (!data[selector]) data[selector] = new Popover(this, options)
            } else {
                if (!data) $this.data('bs.popover', (data = new Popover(this, options)))
            }
            if (typeof option == 'string') data[option]()
        })
    }
    var old = $.fn.popover
    $.fn.popover = Plugin
    $.fn.popover.Constructor = Popover
    $.fn.popover.noConflict = function () {
        $.fn.popover = old
        return this
    }
}(jQuery);

//ScrollSpy
+function ($) {
    'use strict'; function ScrollSpy(element, options) {
        var process = $.proxy(this.process, this)
        this.$body = $('body')
        this.$scrollElement = $(element).is('body') ? $(window) : $(element)
        this.options = $.extend({}, ScrollSpy.DEFAULTS, options)
        this.selector = (this.options.target || '') + ' .nav li > a'
        this.offsets = []
        this.targets = []
        this.activeTarget = null
        this.scrollHeight = 0
        this.$scrollElement.on('scroll.bs.scrollspy', process)
        this.refresh()
        this.process()
    }
    ScrollSpy.VERSION = '3.3.0'
    ScrollSpy.DEFAULTS = { offset: 10 }
    ScrollSpy.prototype.getScrollHeight = function () {
        return this.$scrollElement[0].scrollHeight || Math.max(this.$body[0].scrollHeight, document.documentElement.scrollHeight)
    }
    ScrollSpy.prototype.refresh = function () {
        var offsetMethod = 'offset'
        var offsetBase = 0
        if (!$.isWindow(this.$scrollElement[0])) {
            offsetMethod = 'position'
            offsetBase = this.$scrollElement.scrollTop()
        }
        this.offsets = []
        this.targets = []
        this.scrollHeight = this.getScrollHeight()
        var self = this
        this.$body.find(this.selector).map(function () {
            var $el = $(this)
            var href = $el.data('target') || $el.attr('href')
            var $href = /^#./.test(href) && $(href)
            return ($href && $href.length && $href.is(':visible') && [[$href[offsetMethod]().top + offsetBase, href]]) || null
        }).sort(function (a, b) { return a[0] - b[0] }).each(function () {
            self.offsets.push(this[0])
            self.targets.push(this[1])
        })
    }
    ScrollSpy.prototype.process = function () {
        var scrollTop = this.$scrollElement.scrollTop() + this.options.offset
        var scrollHeight = this.getScrollHeight()
        var maxScroll = this.options.offset + scrollHeight - this.$scrollElement.height()
        var offsets = this.offsets
        var targets = this.targets
        var activeTarget = this.activeTarget
        var i
        if (this.scrollHeight != scrollHeight) {
            this.refresh()
        }
        if (scrollTop >= maxScroll) {
            return activeTarget != (i = targets[targets.length - 1]) && this.activate(i)
        }
        if (activeTarget && scrollTop < offsets[0]) {
            this.activeTarget = null
            return this.clear()
        }
        for (i = offsets.length; i--;) {
            activeTarget != targets[i] && scrollTop >= offsets[i] && (!offsets[i + 1] || scrollTop <= offsets[i + 1]) && this.activate(targets[i])
        }
    }
    ScrollSpy.prototype.activate = function (target) {
        this.activeTarget = target
        this.clear()
        var selector = this.selector + '[data-target="' + target + '"],' +
        this.selector + '[href="' + target + '"]'
        var active = $(selector).parents('li').addClass('active')
        if (active.parent('.dropdown-menu').length) {
            active = active.closest('li.dropdown').addClass('active')
        }
        active.trigger('activate.bs.scrollspy')
    }
    ScrollSpy.prototype.clear = function () {
        $(this.selector).parentsUntil(this.options.target, '.active').removeClass('active')
    }
    function Plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('bs.scrollspy')
            var options = typeof option == 'object' && option
            if (!data) $this.data('bs.scrollspy', (data = new ScrollSpy(this, options)))
            if (typeof option == 'string') data[option]()
        })
    }
    var old = $.fn.scrollspy
    $.fn.scrollspy = Plugin
    $.fn.scrollspy.Constructor = ScrollSpy
    $.fn.scrollspy.noConflict = function () {
        $.fn.scrollspy = old
        return this
    }
    $(window).on('load.bs.scrollspy.data-api', function () {
        $('[data-spy="scroll"]').each(function () {
            var $spy = $(this)
            Plugin.call($spy, $spy.data())
        })
    })
}(jQuery);

//Tab
+function ($) {
    'use strict'; var Tab = function (element) {
        this.element = $(element)
    }
    Tab.VERSION = '3.3.0'
    Tab.TRANSITION_DURATION = 150
    Tab.prototype.show = function () {
        var $this = this.element
        var $ul = $this.closest('ul:not(.dropdown-menu)')
        var selector = $this.data('target')
        if (!selector) {
            selector = $this.attr('href')
            selector = selector && selector.replace(/.*(?=#[^\s]*$)/, '')
        }
        if ($this.parent('li').hasClass('active')) return
        var $previous = $ul.find('.active:last a')
        var hideEvent = $.Event('hide.bs.tab', {
            relatedTarget: $this[0]
        })
        var showEvent = $.Event('show.bs.tab', {
            relatedTarget: $previous[0]
        })
        $previous.trigger(hideEvent)
        $this.trigger(showEvent)
        if (showEvent.isDefaultPrevented() || hideEvent.isDefaultPrevented()) return
        var $target = $(selector)
        this.activate($this.closest('li'), $ul)
        this.activate($target, $target.parent(), function () {
            $previous.trigger({ type: 'hidden.bs.tab', relatedTarget: $this[0] })
            $this.trigger({ type: 'shown.bs.tab', relatedTarget: $previous[0] })
        })
    }
    Tab.prototype.activate = function (element, container, callback) {
        var $active = container.find('> .active')
        var transition = callback && $.support.transition && (($active.length && $active.hasClass('fade')) || !!container.find('> .fade').length)
        function next() {
            $active.removeClass('active').find('> .dropdown-menu > .active').removeClass('active').end().find('[data-toggle="tab"]').attr('aria-expanded', false)
            element.addClass('active').find('[data-toggle="tab"]').attr('aria-expanded', true)
            if (transition) {
                element[0].offsetWidth
                element.addClass('in')
            } else {
                element.removeClass('fade')
            }
            if (element.parent('.dropdown-menu')) {
                element.closest('li.dropdown').addClass('active').end().find('[data-toggle="tab"]').attr('aria-expanded', true)
            }
            callback && callback()
        }
        $active.length && transition ? $active.one('bsTransitionEnd', next).emulateTransitionEnd(Tab.TRANSITION_DURATION) : next()
        $active.removeClass('in')
    }
    function Plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('bs.tab')
            if (!data) $this.data('bs.tab', (data = new Tab(this)))
            if (typeof option == 'string') data[option]()
        })
    }
    var old = $.fn.tab
    $.fn.tab = Plugin
    $.fn.tab.Constructor = Tab
    $.fn.tab.noConflict = function () {
        $.fn.tab = old
        return this
    }
    var clickHandler = function (e) {
        e.preventDefault()
        Plugin.call($(this), 'show')
    }
    $(document).on('click.bs.tab.data-api', '[data-toggle="tab"]', clickHandler).on('click.bs.tab.data-api', '[data-toggle="pill"]', clickHandler)
}(jQuery);

//Affix
+function ($) {
    'use strict'; var Affix = function (element, options) {
        this.options = $.extend({}, Affix.DEFAULTS, options)
        this.$target = $(this.options.target).on('scroll.bs.affix.data-api', $.proxy(this.checkPosition, this)).on('click.bs.affix.data-api', $.proxy(this.checkPositionWithEventLoop, this))
        this.$element = $(element)
        this.affixed = this.unpin = this.pinnedOffset = null
        this.checkPosition()
    }
    Affix.VERSION = '3.3.0'
    Affix.RESET = 'affix affix-top affix-bottom'
    Affix.DEFAULTS = { offset: 0, target: window }
    Affix.prototype.getState = function (scrollHeight, height, offsetTop, offsetBottom) {
        var scrollTop = this.$target.scrollTop()
        var position = this.$element.offset()
        var targetHeight = this.$target.height()
        if (offsetTop != null && this.affixed == 'top') return scrollTop < offsetTop ? 'top' : false
        if (this.affixed == 'bottom') {
            if (offsetTop != null) return (scrollTop + this.unpin <= position.top) ? false : 'bottom'
            return (scrollTop + targetHeight <= scrollHeight - offsetBottom) ? false : 'bottom'
        }
        var initializing = this.affixed == null
        var colliderTop = initializing ? scrollTop : position.top
        var colliderHeight = initializing ? targetHeight : height
        if (offsetTop != null && colliderTop <= offsetTop) return 'top'
        if (offsetBottom != null && (colliderTop + colliderHeight >= scrollHeight - offsetBottom)) return 'bottom'
        return false
    }
    Affix.prototype.getPinnedOffset = function () {
        if (this.pinnedOffset) return this.pinnedOffset
        this.$element.removeClass(Affix.RESET).addClass('affix')
        var scrollTop = this.$target.scrollTop()
        var position = this.$element.offset()
        return (this.pinnedOffset = position.top - scrollTop)
    }
    Affix.prototype.checkPositionWithEventLoop = function () {
        setTimeout($.proxy(this.checkPosition, this), 1)
    }
    Affix.prototype.checkPosition = function () {
        if (!this.$element.is(':visible')) return
        var height = this.$element.height()
        var offset = this.options.offset
        var offsetTop = offset.top
        var offsetBottom = offset.bottom
        var scrollHeight = $('body').height()
        if (typeof offset != 'object') offsetBottom = offsetTop = offset
        if (typeof offsetTop == 'function') offsetTop = offset.top(this.$element)
        if (typeof offsetBottom == 'function') offsetBottom = offset.bottom(this.$element)
        var affix = this.getState(scrollHeight, height, offsetTop, offsetBottom)
        if (this.affixed != affix) {
            if (this.unpin != null) this.$element.css('top', '')
            var affixType = 'affix' + (affix ? '-' + affix : '')
            var e = $.Event(affixType + '.bs.affix')
            this.$element.trigger(e)
            if (e.isDefaultPrevented()) return
            this.affixed = affix
            this.unpin = affix == 'bottom' ? this.getPinnedOffset() : null
            this.$element.removeClass(Affix.RESET).addClass(affixType).trigger(affixType.replace('affix', 'affixed') + '.bs.affix')
        }
        if (affix == 'bottom') {
            this.$element.offset({ top: scrollHeight - height - offsetBottom })
        }
    }
    function Plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('bs.affix')
            var options = typeof option == 'object' && option
            if (!data) $this.data('bs.affix', (data = new Affix(this, options)))
            if (typeof option == 'string') data[option]()
        })
    }
    var old = $.fn.affix
    $.fn.affix = Plugin
    $.fn.affix.Constructor = Affix
    $.fn.affix.noConflict = function () {
        $.fn.affix = old
        return this
    }
    $(window).on('load', function () {
        $('[data-spy="affix"]').each(function () {
            var $spy = $(this)
            var data = $spy.data()
            data.offset = data.offset || {}
            if (data.offsetBottom != null) data.offset.bottom = data.offsetBottom
            if (data.offsetTop != null) data.offset.top = data.offsetTop
            Plugin.call($spy, data)
        })
    })
}(jQuery);



String.prototype.format || (String.prototype.format = function () {
    var e = arguments;
    return this.replace(/{(\d+)}/g, function (t, n) { return typeof e[n] != "undefined" ? e[n] : t });
}), function (e) {
    e.fn.dialog = function (t) {
        var n = this,
            r = e(n),
            i = e(document.body),
            s = r.closest(".dialog"),
            o = "dialog-parent",
            u = arguments[1],
            a = arguments[2],
            f = function () { var t = '<div class="dialog modal fade"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><button type="button" class="close">&times;</button><h4 class="modal-title"></h4></div><div class="modal-body"></div><div class="modal-footer"></div></div></div></div>'; s = e(t), e(document.body).append(s), s.find(".modal-body").append(r) }, l = function (r) {
                var i = (r || t || {}).buttons || {}, o = s.find(".modal-footer"); o.html(""); for (var u in i) {
                    var a = i[u], f = "", l = "", c = "btn-default", h = ""; a.constructor == Object && (f = a.id, l = a.text, c = a["class"] || a.classed || c, h = a.click), a.constructor == Function && (l = u, h = a), $button = e('<button type="button" class="btn {1}">{0}</button>'.format(l, c)), f && $button.attr("id", f), h && function (e) { $button.click(function () { e.call(n) }) }(h), o.append($button)
                }
                o.data("buttons", i)
            }, c = function () { s.modal("show") }, h = function (e) { s.modal("hide").on("hidden.bs.modal", function () { e && (r.data(o).append(r), s.remove()) }) }; t.constructor == Object && (!r.data(o) && r.data(o, r.parent()), s.size() < 1 && f(), l(), e(".modal-title", s).html(t.title || ""), e(".modal-dialog", s).addClass(t.dialogClass || ""), e(".modal-header .close", s).click(function () { var e = t.onClose || h; e.call(n) }), (t["class"] || t.classed) && s.addClass(t["class"] || t.classed), t.autoOpen !== !1 && c()), t == "destroy" && h(!0), t == "close" && h(), t == "open" && c(); if (t == "option" && u == "buttons") {
                if (!a) return s.find(".modal-footer").data("buttons"); l({
                    buttons: a
                }), c()
            }
        return n
    };
}(jQuery),



$.messager = function () {
    //alert
    var e = function (e, t, time, reload, settings) {
        var n = $.messager.model;
        arguments.length < 2 && (t = e || "", e = "&nbsp;"),
        $("<div>" + t + "</div>").dialog({
            title: e,
            onClose: function () {
                $(this).dialog("destroy");
            },
            buttons: [{
                text: n.ok.text,
                classed: n.ok.classed || "btn-success",
                click: function () {
                    $(this).dialog("destroy");
                }
            }]
        });

        if (time && $.isNumeric(time)) {
            clearTimeout(i);
            i = setTimeout(function () {
                $('.dialog').css("display", 'none');

                //YZQ Inject CallBack
                if (settings && settings.closed && $.isFunction(settings.closed))
                    settings.closed();
            }, time);
        }
    },
    //confirm
    t = function (e, t, n,m) {
        var r = $.messager.model; $("<h4 class='text-center'>" + t + "</h4>").dialog({
            title: e,
            onClose: function () { $(this).dialog("destroy"); },
            buttons: [{
                text: r.ok.text, classed: r.ok.classed || "btn-success",
                click: function () { $(this).dialog("destroy"); n && n() }
            },
            {
                text: r.cancel.text, classed: r.cancel.classed || "btn-danger",
                click: function () { $(this).dialog("destroy");m && m() }
            }]
        })
    },
    n = '<div class="dialog modal fade msg-popup"><div class="modal-dialog modal-sm"><div class="modal-content"><div class="modal-body text-center"></div></div></div></div>',
    r,
    i,
    //popup
    s = function (arrData) {
        arrDefaultData = { 'content': '', 'time': 1500, 'modal': 1, 'close': 0, 'css': 's' };
        var settings = $.extend({}, arrDefaultData, arrData);
        r || (r = $(n), $("body").append(r));
        if (settings.css) {
            if (settings.css == 's') { r.removeClass('msg-popup-i'); }
            else { r.removeClass('msg-popup-s'); }
            r.addClass('msg-popup-' + settings.css);
        }
        if (arrData.close == 1) {
            r.modal("hide");
        }
        else {
            r.find(".modal-body").html(settings.content), r.modal({ show: !0, backdrop: settings.modal == '1' ? !0 : 0 });
            if (settings.time) {
                clearTimeout(i);
                i = setTimeout(function () { r.modal("hide"); }, settings.time);
            }
        }
    };
    return {
        alert: e,
        popup: s,
        confirm: t
    }
}(),
$.messager.model = { ok: { text: "确定", classed: "btn-info w-xs" }, cancel: { text: "取消", classed: "btn-default w-xs" } };;
var _ = '[contentID="index"] ';
var config = { rowcolors: { 0: ['b-info', 'alert-info'], 1: ['b-success', 'alert-success'], 2: ['b-warning', 'alert-warning'], 3: ['b-default', 'alert-default'] } }

app.c = {
    public_data: {},
    init_menu: false,
    is_ctrl: false,
    load_menu: [],
    is_history: false,
    needFooterFixed: function () {
        $(window).ready(function () {
            $(_ + '.need-footer-fixed').addClass('need-footer-fixed-box');
        });
    },
    style: function (objCurrent) {
        $('#extend_style').attr('href', publicSettings.publicUrl + 'static/css/style/' + objCurrent.attr('dataType') + '.css'); $.cookie('style_cookie', objCurrent.attr('dataType'), { expires: 86400 * 30 * 365, path: '/' }); $('#style-select a .pull-right').remove(); objCurrent.append('<span class="pull-right"><i class="glyphicon glyphicon-ok"></i></span>');
    },
    resizePage: function () {
        $('.wrap-content').css({ 'min-height': ($(window).height() - 100) + 'px' }); $('.wrap-tab-contentbox').css({ 'min-height': ($(window).height() - 115) + 'px' }); $('.need-footer-fixed-box').css('width', ($(window).width() - 115) + 'px');
    },
    menucenter: function (booClose) {
        if (!booClose) {
            booClose = false;
        }
        if ($('#menucenter-btn').attr('class') == '' && booClose === false) { $("#menucenter-mainbox").css('display', 'block'); $('#menucenter-btn').attr('class', 'ok'); $('#menucenter-btn span').eq(0).removeClass('fa-angle-double-down').addClass('fa-angle-double-up'); } else { $("#menucenter-mainbox").css('display', 'none'); $('#menucenter-btn').attr('class', ''); $('#menucenter-btn span').eq(0).removeClass('fa-angle-double-up').addClass('fa-angle-double-down'); }
    },
    fullscreen: function () {
        $("#fullscreen-btn").click(function () {
            if (!window.fullScreenApi.supportsFullScreen) { alert("您的浏览器不支持全屏API哦，请换高版本的Chrome或者Firefox！"); }
            else {
                if ($(this).attr('class') == '') {
                    $("body").requestFullScreen(); $(this).attr('class', 'ok');
                    $(this).find('span').removeClass('fa-expand').addClass('fa-compress');
                }
                else {
                    window.fullScreenApi.cancelFullScreen(); $(this).attr('class', '');
                    $(this).find('span').removeClass('fa-compress').addClass('fa-expand');
                }
            }
        });
    }, service: function () {
        $('.service-menu').click(function (e) {
            if ($('#service-mainbox').attr('class') == 'service-close') { $('.service-bgbox,.service-content-box').animate({ 'right': 0 }, 300); $('#service-mainbox').attr('class', 'service-open'); } else { $('.service-bgbox,.service-content-box').animate({ 'right': -325 }, 300); $('#service-mainbox').attr('class', 'service-close'); }
            e.stopPropagation();
        }); $('#service-mainbox').click(function (e) {
            e.stopPropagation();
        });
    },
    menuTab: function () {
        
        $('div.wrap-header-tabbox ul.first-level').on('mouseenter mouseleave click', 'li', function (e) {            
            if (e.type == 'mouseenter') {
                if ($(this).attr('class_') == 'menu-more') { $(this).children('ul').show(); $(this).css('height', '50px'); } else {
                    if ($(this).next().attr('class_') == 'menu-more') { $(this).next().children('ul').show(); $(this).next().css('height', '50px'); }
                    if ($(this).attr('menuID') == 'index') { if ($("div.wrap-header-tabbox ul.first-level li[menuID!='index']").length > 0) { $(this).children('.wrap-header-navdel').show(); } else { $(this).children('.wrap-header-navdel').hide(); } } else { $(this).children('.wrap-header-navdel').show(); }
                }
                e.stopPropagation();
            }
            else if (e.type == 'mouseleave') {
                var menumore = $("div.wrap-header-tabbox ul.first-level li[class_='menu-more']"); if (menumore.length > 0) {
                    var booYes = false; if ($(this).parents('ul').attr('class') == 'more-tab') { booYes = true; } else if ($(this).next().length < 1) { booYes = true; } else if ($(this).next().attr('class_') == 'menu-more') {
                        booYes = true;
                    }
                    if (booYes === false) {
                        menumore.children('ul').hide(); menumore.css('height', '50px');
                    }
                }
                $(this).children('.wrap-header-navdel').hide(); e.stopPropagation();
            }
            else if (e.type == 'click') {
                if ($(this).attr('menuID') != 'index') { var strHistory = publicSettings.rootUrl + '?url=' + encodeURIComponent($(this).find('a').attr('_url')); } else { var strHistory = publicSettings.rootUrl; }
                if (strHistory != window.location.href) {
                    strHistory = publicSettings.rootUrl + publicSettings.moduleName + '#' + $.DHB.thinkUrl(strHistory);
                    app.c.is_history = false;
                    window.history.pushState('forward', null, strHistory);
                }
                var menumore = $('div.wrap-content-header .wrap-header-tabbox ul.more-tab'); if (menumore.length > 0) {
                    var booYes = false; if ($(this).parents('ul').attr('class') != 'more-tab' && $(this).next().length < 1) {
                        booYes = true;
                    }
                    if (booYes === true) { menumore.toggle(); } else { menumore.hide(); }
                }
                $(this).siblings().children('.wrap-header-navdel').hide(); if ($(this).parent().attr('class').indexOf('more-tab') >= 0) { var cloneThis = $(this).clone(true); var cloneLast = $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:not([class_]):last()').clone(true); $(this).remove(); $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:not([class_]):last()').remove(); $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:not([class_]):last()').after(cloneThis); $('div.wrap-content-header .wrap-header-tabbox ul.more-tab').prepend(cloneLast); }
                $.DHB.menuData[1] = $("div.wrap-header-tabbox ul.first-level li.active").attr('menuID');
                $.cookie('dhb_' + publicSettings.moduleName + '_menutab', JSON.stringify($.DHB.menuData), { expires: 86400, path: '/' });
                if ($(this).data('init') == '0') {
                    $(this).data('init', '1');
                    $.DHB.refresh();
                }
                else {
                    jQuery.DHB.ready();
                }
                e.stopPropagation();
            }
        });
    },
    sidebar: function () {
        var aside = { timer: null, timer_: null }; $('.aside-wrap').on('mouseenter mouseleave click', '.navi-item', function (e) {
            if (e.type == 'mouseenter' || e.type == 'click') {
                var _this = this; if (aside.timer_) {
                    clearTimeout(aside.timer_);
                }
                $(_this).find('i').addClass($(_this).find('i').attr('class_') + '_'); $(_this).siblings('.active').removeClass('active'); $(_this).addClass('active'); aside.timer = setTimeout(function () { $(".aside-wrap .navi-item .nav-sub").hide(); $(_this).find('.nav-sub').show(); var intTop = $(_this).children('a:eq(0)').offset().top - $(document).scrollTop() + $(_this).find('.nav-sub').height() - $(window).height(); if (intTop > 0) { $(_this).find('.nav-sub').css({ 'top': '-' + (intTop - 2) + 'px' }); } }, 50);
            } else {
                if (aside.timer) {
                    clearTimeout(aside.timer);
                }
                var _this = this; $(_this).find('i').removeClass($(_this).find('i').attr('class_') + '_'); $(_this).find(".nav-sub").parents(".active").removeClass("active"); aside.timer_ = setTimeout(function () { $(".aside-wrap .navi-item .nav-sub").hide(); }, 50);
            }
        });
        $('.aside-wrap .navi-item ul').on('click', '.nav-sub-header', function (e) {
            $(this).parents('.nav-sub').hide(); e.stopPropagation();
        });
    },
    footer: function () { $('.sidebar-footer').mouseover(function () { $('.footer').css('display', 'block'); }).mouseout(function () { $('.footer').css('display', 'none'); }); },
    index: function () { $.DHB.index('init'); },
    cookieMenu: function (hash) {
        if (hash != '') {
            var curNode = hash.split('?');
            var curNode = publicSettings.moduleName + '@' + curNode[0].replace('/', '@');
            var strTitle = '';
            var strTabID = '';
            if (typeof (app.menutab[curNode]) != 'undefined') {
                if (!strTitle) {
                    strTitle = app.menutab[curNode].title;
                }
                if (!strTabID) {
                    strTabID = app.menutab[curNode].id;
                }
            }
            var menu = [{}, '']; var newMenu = []; newMenu.push({ url: $.DHB.U(hash), 'title': strTitle, 'id': strTabID, nocache: '0' }); app.c.load_menu = newMenu; app.c.cookieMenu_();
        } else {
            var menu = $.cookie('dhb_' + publicSettings.moduleName + '_menutab'); menu = menu ? JSON.parse(menu) : [{}, '']; if ($.isEmptyObject(menu[0]) === false) {
                var newMenu = []; var cur = {}; for (var id in menu[0]) {
                    if (id == menu[1]) {
                        cur = {
                            url: menu[0][id][1], 'title': menu[0][id][0], 'id': id, nocache: 0
                        };
                    }
                    newMenu.push({ url: menu[0][id][1], 'title': menu[0][id][0], 'id': id, nocache: 0, firstinit: '1' });
                }
                if (!$.isEmptyObject(cur)) { newMenu.push(cur); } else { var indexA = $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:first()>a'); newMenu.push({ url: indexA.attr('_url'), 'title': indexA.attr('_title'), 'id': indexA.attr('_id'), nocache: 0 }); }
                app.c.load_menu = newMenu; app.c.cookieMenu_();
            } else {
                app.c.cookieMenu__();
            }
            $.DHB.menuData = menu;
        }
    },
    cookieMenu_: function (newID) {
        if (app.c.load_menu.length < 1) { app.c.cookieMenu__(); }
        else { var now = app.c.load_menu.shift(); if (typeof (now) != 'undefined') { $.fn.menuTab.load(now); } else { app.c.cookieMenu__(); } }
    },
    cookieMenu__: function () {
        $('body #menutabload_box').fadeOut(100); setTimeout(function () { $('body').attr('style', ''); }, 150);
        app.c.bindMenuMove(); app.c.init_menu = true;
    },
    //删除菜单
    clearMenu: function ($strAct) {
        if ($strAct == 'other') {
            var otherLi = $("div.wrap-header-tabbox ul.first-level li[menuID!='index'][class!='active']");
            otherLi.each(function () {
                $.DHB.menuCookie({ id: $(this).attr('menuID') }, 'del');
                $("div.wrap-content-body .wrap-tab-contentbox[contentID='" + $(this).attr('menuID') + "']").remove();
            });
            otherLi.remove();
        }
        else if ($strAct == 'right') {
            var rightLi = $("div.wrap-header-tabbox ul.first-level>li:gt(" + $("div.wrap-header-tabbox ul.first-level li[class='active']").index() + ")");
            rightLi.each(function () {
                $.DHB.menuCookie({ id: $(this).attr('menuID') }, 'del');
                $("div.wrap-content-body .wrap-tab-contentbox[contentID='" + $(this).attr('menuID') + "']").remove();
            });
            rightLi.remove();
        }
        else {
            var allLi = $("div.wrap-header-tabbox ul.first-level li[menuID!='index']");
            allLi.each(function () {
                $.DHB.menuCookie({ id: $(this).attr('menuID') }, 'del');
            });
            allLi.remove();
            $("div.wrap-content-body .wrap-tab-contentbox[contentID!='index']").remove();
            $.DHB.index(true);
        }
    },
    bodyClick: function () {
        $("body").click(function () {
            $('.aside-wrap .nav-sub').css('display', 'none');
            app.c.menucenter(true); $('div.more-operate > div').hide(); $('div.wrap-content-header .wrap-header-tabbox ul.more-tab').css({ 'display': 'none', 'height': 'auto' }); $('.show-li,.show-li2,#niceTitle').hide(); $('.dhb-citybox-box .city-box').hide();
        });
    },
    resize: function () { $(window).resize(function () { app.c.resizePage(); $.fn.menuTab.bindMenuMore(); app.c.sidebar(); }); },
    bindMenuMove: function () {
        if (false) {
            $("ul#wrap-header-menu-move").dragsort({
                dragSelector: '', dragBetween: true, dragEnd: function () {
                    if ($("div.wrap-header-tabbox ul.first-level li:eq(0)").attr('menuID') != 'index') { var cloneIndex = $("div.wrap-header-tabbox ul.first-level li[menuID='index']").clone(true); $("div.wrap-header-tabbox ul.first-level li[menuID='index']").remove(); $("div.wrap-header-tabbox ul.first-level").prepend(cloneIndex); }
                    if ($('div.wrap-content-header .wrap-header-tabbox ul.first-level>li[class_="menu-more"]').length > 0) { var cloneMore = $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li[class_="menu-more"]').clone(true); $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li[class_="menu-more"]').remove(); $('div.wrap-content-header .wrap-header-tabbox ul.first-level').append(cloneMore); }
                    var menu = [{}, '']; var temp = []; $("div.wrap-header-tabbox ul.first-level li[menuID!='index']").each(function () { if ($(this).attr('class_') != 'menu-more') { var curA = $(this).find('a'); temp.unshift(new Array(curA.attr('_id'), curA.attr('_title'), curA.attr('_url'))); } }); for (var i = 0; i < temp.length; i++) {
                        menu[0][temp[i][0]] = new Array(temp[i][1], temp[i][2]);
                    }
                    menu[1] = $("div.wrap-header-tabbox ul.first-level li.active").attr('menuID'); $.DHB.menuData = menu; $.cookie('dhb_' + publicSettings.moduleName + '_menutab', JSON.stringify(menu), {
                        expires: 86400, path: '/'
                    });
                }, dragSelectorExclude: 'li[class_="more"]', placeHolderTemplate: "<li class='placeHolder'></li>", scrollSpeed: 15
            });
        }
    },
    extend: function () {
        $('html,body').animate({ scrollTop: 0 }, 10); var iconTimer = null; $(document).on('mouseenter mouseleave', '.showIcon', function (e) { var _self = this; if (iconTimer) { clearTimeout(iconTimer); }; $(".iconImg").css("visibility", "hidden"); $(".popover").css("display", "none"); $(_self).find(".iconImg").css("visibility", "visible"); if (e.type == "mouseleave") { iconTimer = setTimeout(function () { $(_self).find(".iconImg").css("visibility", "hidden"); }, 50); }; }); $(document).bind('keydown', function (e) { if (e.which == 17) { } else if (e.which == 116) { $.DHB.refresh(); return false; } else { } }); $(document).bind('keyup', function (e) { }); $(document).ready(function (e) {
            if (window.history && window.history.pushState) {
                $(window).on('popstate', function () {
                    var hash = window.location.hash;
                    hash = hash.replace(/#/g, '');
                    if (hash) {
                        if (publicSettings.homeNode != hash) {
                            app.c.is_history = true;
                            $.DHB.url(hash);
                        }
                    }
                    else {
                        return false;
                    }
                });
            }
        });
        window.onbeforeunload = function () { }
    },
    chooseHover: function (e) {
        $("body").on("mouseenter mouseleave click", ".show-list", function (e) {
            var _self = this;
            var timer = null;
            if (timer) { clearTimeout(timer); };
            $('.show-li').hide();
            $('.show-li').find('.need-hide').hide();
            if ($(_self).next().hasClass("dropdown-menu")) { $(_self).next().show(); }
        });
        $("body").on("mouseenter mouseleave click", ".show-li", function (e) {
            var _self = this; $(_self).show(); $(_self).find('.need-hide').hide();
            //打印
            //console.log($(_self));
            if (e.type == "mouseleave") { $(_self).hide(); $(_self).find('.need-hide').hide(); }; if (e.type == 'click') { $(_self).hide(); $(_self).find('.need-hide').hide(); }
        });
    },
    preview_print: function (title, url, module, id) {
        module = module || '';
        $.DHB.dialog({ 'title': title, 'url': $.DHB.U('Public/printDialog?type=' + module + '&did=' + id + '&url=' + encodeURIComponent($.DHB.U(url))), 'id': 'dialog-print' });
        $(_ + "#dialog-print #submit-button").remove();
    },
    auxiliaryCopy: function (el, content) {
        var str = '<label class="copy d-i-b  m-l-none"  id="copy" style="position: relative;"><i class="icon-question tool iconImg"  style="position: relative;"></i>' + '<div class="popover fade bottom in tool-box">' + '<div class="arrow"></div>' + '<div class="popover-content">' + content + '</div></div></label>';
        $(el).append(str);
    },
    setWordsWidth: function (el) { el = el || {}; function getTextWidth(str) { var w = $('body').append($('<span stlye="display:none;" id="textWidth"/>')).find('#textWidth').html(str).width(); $('#textWidth').remove(); return w; }; el.find("p").each(function (index, dom) { var widthArray = []; var pEl = $(dom); var html = pEl.html().split('<br>'); html.forEach(function (text, index) { var text = text; widthArray.push(getTextWidth(text)); }); var maxWidthArray = []; maxWidthArray.push(widthArray.sort(function (a, b) { return b - a })[0]); var width = (maxWidthArray > 700 ? 700 : maxWidthArray); $(dom).css('width', width + 'px'); }); }, setHelpPlace: function (el) {
        app.c.setWordsWidth($(el).find(".popover-content"));
        var toolWidth = $(el).find(".tool").width();
        var contentWidth = $(el).find(".tool-box").width();
        var left;
        left = (toolWidth - contentWidth) / 2;
        var toolOffsetLeft = $(el).find(".tool").offset().left;
        var toolOffset = toolOffsetLeft - 110;
        var windowWidth = $(window).width();
        var toolOffsetRight = windowWidth - toolOffsetLeft - 15;
        if (Math.abs(left) > toolOffset) {
            left = -toolOffset;
        }
        else if (Math.abs(left) > toolOffsetRight) {
            left = -(contentWidth - toolOffsetRight + 20);
        }
        $(el).find(".tool-box").css("left", left + "px");
        $(el).find(".arrow").css("left", -left + "px");
    },
    showHelpPlace: function () {
        var timer = null; $(document).on("mouseenter mouseleave", ".tool-box", function (e) { var _this = this; if (timer) { clearTimeout(timer); }; app.c.setHelpPlace($(_this).parents(".copy")); $(".tool-box").hide(); $(_this).parents(".copy").find(".tool-box").show(); if (e.type == "mouseleave") { timer = setTimeout(function () { $(_this).parents(".copy").find(".tool-box").hide(); }, 200); } }); $(document).on("mouseenter mouseleave", ".icon-question", function (e) {
            var _this = this; if (timer) { clearTimeout(timer); }; app.c.setHelpPlace($(_this).parents(".copy")); $(".tool-box").hide(); $(_this).parents(".copy").find(".tool-box").show(); if (e.type == "mouseleave") { timer = setTimeout(function () { $(_this).parents(".copy").find(".tool-box").hide(); }, 200); }
        });
    },
    toolLink: function () {
        $(document).on("click", "#help-link", function () {
            window.open($(this).attr("href"));
        })
    },
    //TODO：验证登录 已屏蔽
    checkLogin: function () {
        //$.post($.DHB.U('Public/checkLogin?time=' + new Date().getTime()), function (data) {
        //    if (typeof (data.status) == 'undefined' || data.status != "success")
        //    { $.DHB.message({ content: '认证过期请重新登录！', 'type': 'e', time: 0 }); setTimeout(function () { window.location.href = $.DHB.U(publicSettings.moduleName == 'Manager' ? 'Auth/Index/index' : 'Auth/Index/platform'); }, 1500); }
        //}, 'json');
    }
};


$(document).ready(function () { app.c.checkLogin(); app.c.extend(); app.c.index(); app.c.resizePage(); app.c.menuTab(); app.c.sidebar(); app.c.footer(); app.c.bodyClick(); app.c.needFooterFixed(); app.c.resize(); app.c.chooseHover(); app.c.showHelpPlace(); app.c.toolLink(); }); (function ($) {
    var option; var methods = {
        init: function (settings) { console.log('==============================='); console.log('==== DHBTree Jquery Plugin ===='); console.log('==============================='); option = $.extend({ data: [], outerWrapper: '<ul class="p-l-none m-b-none first-menu-type choose-client">', innerWrapper: '<ul class="two-menu-type">', id: 'rule_id', value: 'rule_name', name: 'rule[]' }, settings); var html = methods.render(option.data, 1); this.append(html); return this; }, render: function (data, level) {
            var item; var html = ''; html += (level == 1) ? option.outerWrapper : option.innerWrapper; for (var i = 0; i < data.length; i++) {
                item = data[i]; var tmp = item.children ? methods.render(item.children) : ''; var collapseDom = item.children ? '<i class="get-area glyphicon-plus no-italic"></i>' : '<i style="margin-left: 10px"></i>'; html += '<li>' +
                collapseDom + '<label class="i-checks m-l-none">' + '<input name="' + option.name + '" type="checkbox" fullname="' + item[option.value] + '" value="' + item[option.id] + '">' + '<i></i>' + item[option.value] + '</label>' +
                tmp + '</li>'
            }
            html += '</ul>'
            return html;
        }, getCheckedValue: function () { var val = []; this.find('input[name="' + option.name + '"]:checked').each(function () { val.push($(this).val()); }); return val; }, uncheckAll: function () { this.find('input[name="' + option.name + '"]:checked').parents('li').find('i.glyphicon-minus').click(); this.find('input[name="' + option.name + '"]:checked').prop('checked', false); }, checkBellow: function (element) { element.parent().next('ul').children().find('input').prop('checked', true); }, unCheckBellow: function (element) { element.parent().next('ul').children().find('input').prop('checked', false); }, checkLeafToRoot: function (element) { element.parents('li').children('label').children('input').prop('checked', true); }, checkValue: function (ruleIds) {
            if (!ruleIds) {
                return this;
            }
            this.find('input[name="' + option.name + '"]:checked').prop('checked', false); for (var i = 0; i < ruleIds.length; i++) {
                this.find('input[value="' + ruleIds[i] + '"]').prop('checked', true); this.find('input[value="' + ruleIds[i] + '"]').parents('li').find('i.glyphicon-plus').click();
            }
            return this;
        }
    }; $.fn.DHBTree = function (method) { if (methods[method]) { return methods[method].apply(this, Array.prototype.slice.call(arguments, 1)); } else if (typeof method === 'object' || !method) { return methods.init.apply(this, arguments); } else { $.error('Method ' + method + ' does not exist on jQuery.DHBTree'); } }
})(jQuery);;



app.edit_orders = app.edit_orders || {};
app.edit_orders.check = function (t) {
    return app.edit.check("gys_id", t)
}

app.edit = app.edit || {}; { }

app.edit.get_goodsid = function (goodID) {
    var returnGoodsID = {}; if (goodID.indexOf('-') >= 0) { var tempGoodsID = goodID.split('-'); returnGoodsID['goods_id'] = tempGoodsID[0]; returnGoodsID['options_id'] = tempGoodsID[1]; returnGoodsID['extend_id'] = typeof (tempGoodsID[2]) != 'undefined' ? tempGoodsID[2] : ''; } else { returnGoodsID['goods_id'] = goodID; returnGoodsID['options_id'] = ''; returnGoodsID['extend_id'] = ''; }
    return returnGoodsID;
}

app.edit_orders.goods = function (t, e, i, a, d, n) {
    //;
    if (a === !0) {
        var r = parseFloat(i.find('td[field="number"] div').text());
        r += 1,
		i.find('td[field="number"] div').text($.DHB.format_number(r)),
		i.find('td[field="number"]').click()
    } else {
        if (i.find('td[field="category"] div').html(t.data("category")), i.find('td[field="options"]').length > 0) if (n.indexOf("-") >= 0) {
            var o = {
                name: "goods-multioptions-id",
                value: t.data("optionsid")
            },
			s = $.DHB.isAjaxJson(t.data("multioptions"), !0);
            if (s.length > 1) {
                o.item = [];
                for (var l = 0; l < s.length; l++) o.item[s[l].id] = {
                    0: s[l].name,
                    1: s[l].price,
                    2: s[l].number_price,
                    3: s[l].whole_inventory
                }
            } else o.item = s[0].name;
            i.find('td[field="options"]').html($.DHB.table.list(o));
            var p = i.find('td[field="options"] .goods-multioptions-id>button .cut-out'),
			m = i.find('td[field="options"] div.list-single');
            p.length > 0 && p.html().indexOf(",") >= 0 && p.html(p.html().replace(",", "<br/>")),
			m.length > 0 && m.html().indexOf(",") >= 0 && m.html(m.html().replace(",", "<br/>"))
        } else i.find('td[field="options"]').html("");
        var f = {
            name: "goods-units-id"
        };
        f.item = t.data("baseunits"),
		f.value = "0",
		i.find('td[field="units"]').html($.DHB.table.list(f)).attr("data-base", t.data("baseunits")).data("base", t.data("baseunits")).attr("data-container", t.data("containerunits")).data("container", t.data("containerunits")).attr("data-units", f.value).data("units", f.value),
		i.find('td[field="units"]').length > 0 && t.data("containerunits") && i.find('td[field="units"] div').append('<p style="color: rgb(153, 153, 153);font-size:12px;">1' + t.data("containerunits") + " = " + parseInt(t.data("conversionnumber")) + t.data("baseunits") + "</p>"),
		i.find('td[field="shipednumber"]').length > 0 && i.find('td[field="shipednumber"] div').html(t.data("shipednumber")),
		i.find('td[field="inventory"]').length > 0 && i.find('td[field="inventory"] div').html(t.data("wholeinventory"));
        //;

        i.find('td[field="barcode"]').length > 0 && i.find('td[field="barcode"] div').html(t.data("orderunits"));

        i.find('td[field="id_kcsp"]').length > 0 && i.find('td[field="id_kcsp"] div').html(t.data("id_kcsp"));
        i.find('td[field="id_sp"]').length > 0 && i.find('td[field="id_sp"] div').html(t.data("id_sp"));
        i.find('td[field="zhl"]').length > 0 && i.find('td[field="zhl"] div').html(t.data("zhl"));

        var u = parseFloat(t.data("minorder")),
		c = u;
        "container_units" == t.data("orderunits") && (u *= parseFloat(t.data("conversionnumber")), c = u),
		"undefined" != typeof t.data("ordersnumber") && (u = parseFloat(t.data("ordersnumber"))),
		i.find('td[field="number"] div').html($.DHB.format_number(u)),
		i.find('td[field="number"]').data("wholeinventory", t.data("wholeinventory")).attr("data-wholeinventory", t.data("wholeinventory")),
		i.find('td[field="number"]').data("minorder", c).attr("data-minorder", c),
		i.find('td[field="number"]').data("stocknumber", t.data("stocknumber")).attr("data-stocknumber", t.data("stocknumber")),
		parseFloat(t.data("stocknumber")) > 0 && i.find('td[field="operate"] a:eq(1)').remove(),
		i.find('td[field="number"]').data("oldnumber", $.DHB.format_number(u)).attr("data-oldnumber", $.DHB.format_number(u)).data("translation", $.DHB.format_number(t.data("translation"))).attr("data-translation", $.DHB.format_number(t.data("translation")));
        var h = parseFloat(t.data("wholeprice"));
        if (i.find('td[field="price"] div').html($.DHB.format_price(h)), i.find('td[field="price"]').data("number", JSON.stringify(t.data("numberprice"))).attr("data-number", JSON.stringify(t.data("numberprice"))).data("old", h).attr("data-old", h),
            app.edit_orders.format_price(t.data("numberprice"), t), i.find('td[field="count"] div').html($.DHB.format_price(parseFloat(i.find('td[field="price"] div').html()) * parseFloat(i.find('td[field="number"] div').html()))),
            "undefined" != typeof t.data("remark")) {
            var v = $.trim(t.data("remark"));
            v || (v = "--"),
            i.find('td[field="remark"]').attr("title", v),
            i.find('td[field="remark"] p').html(v)
        }

        app.edit_orders.table_result(e),

    i.find('td[field="number"]').click(function () {

        $(this).attr("class").indexOf("editChange") < 0 &&
        (

        $(this).html('<input type="text" class="input-change-field" value="' + $.trim($(this).find("div").html()) + '" />').removeClass("editChange").addClass("editChange"),
        $(this).find("input").focus().select().blur(function () {

            var i = goodsnum = parseFloat($(this).val()),
            a = parseFloat($(this).parents("td").data("minorder")),
            d = parseFloat($(this).parents("td").data("wholeinventory")),
            n = parseFloat($(this).parents("td").data("oldnumber"));
            if ("T" == publicSettings.order_approval)
                var r = parseFloat($(this).parents("td").data("stocknumber"));

            //;

            if (goodsnum.toString() == "NaN") {
                $.DHB.message({ content: "格式不正确！", time: 1500, type: "i" });
                $(this).val($.DHB.format_number(n));
                goodsnum = parseFloat($(this).val());
            }
            else if (goodsnum <= 0) {
                $.DHB.message({ content: "商品数量必须大于0！", time: 1500, type: "i" });
                $(this).val($.DHB.format_number(n));
                goodsnum = parseFloat($(this).val());
            }

            var o = $(t).parents("tr");
            $(this).parents("td").data("oldnumber", goodsnum).attr("data-oldnumber", goodsnum),
            $(this).parent().html("<div>" + $.DHB.format_number(goodsnum) + "</div>").removeClass("editChange"), app.edit_orders.chang_price(o, !1, e)


            //$.trim($(this).val()) ? goodsnum < 0 ?
            //        (
            //        $.DHB.message({ content: "商品数量必须大于0！", time: 1500, type: "i" }),
            //        $(this).val($.DHB.format_number(a)), goodsnum = parseFloat($(this).val())
            //        )

            //        : 
            //        "F" == publicSettings.inventory_control

            //        && i > d ?
            //    (
            //    //$.DHB.message({ content: "最大库存数量不足，库存数量为(" + d + ")！", time: 1500, type: "i" }),
            //       $(this).val($.DHB.format_number(a)),
            //        goodsnum = parseFloat($(this).val())
            //    )

            //        : "T" == publicSettings.order_approval && "undefined" != typeof r && i < r &&
            //        (
            //        //$.DHB.message({ content: "订货数量不能小于出库数量，已出库数量为(" + r + ")！", time: 1500, type: "i" }),
            //        $(this).val($.DHB.format_number(n)),
            //          goodsnum = parseFloat($(this).val())
            //        )

            //        :
            //        (
            //        //$.DHB.message({ content: "订货数量不能为空！", time: 1500, type: "i" }),
            //          $(this).val($.DHB.format_number(a)),
            //           goodsnum = parseFloat($(this).val())
            //        );




        }))

    }),
    i.find('td[field="price"]').click(function () {
        $(this).attr("class").indexOf("editChange") < 0 &&
        (
        $(this).html('<input type="text" class="input-change-field" value="' + $.trim($(this).find("div").html()) + '" />').removeClass("editChange").addClass("editChange"),
        $(this).find("input").focus().select().blur(function () {
            var i = parseFloat($(this).parents("td").data("old")),
            a = parseFloat($(this).val());

            //;
            if (!$.trim($(this).val())) {
                $.DHB.message({ content: "单价不能为空！", time: 1500, type: "i" });
                $(this).val($.DHB.format_price(i))
                a = parseFloat($(this).val());
            }
            else if (a.toString() == "NaN") {
                $.DHB.message({ content: "格式不正确！", time: 1500, type: "i" });
                $(this).val($.DHB.format_price(i));
                a = parseFloat($(this).val());
            }

            var d = $(t).parents("tr");
            $(this).parents("td").data("old", a).attr("data-old", a),
            $(this).parent().html("<div>" + $.DHB.format_price(a) + "</div>").removeClass("editChange"),
            app.edit_orders.chang_price(d, !0, e)


        }))
    }),
    i.find('td[field="options"]').find(".goods-multioptions-id .dropdown-menu li").each(function () {
        $(this).click(function () {
            var i = $(this).parents("tr"),
            a = app.edit.get_goodsid(i.data("item")),
            d = a.goods_id + "-" + $(this).attr("value");
            if ($(this).attr("class").indexOf("active") < 0) if (e.find('tr[data-item="' + d + '"]').length >= 1) $.DHB.message({
                content: "已经存在多选项属性了！",
                time: 1500,
                type: "i"
            });
            else {
                var n = i,
                r = JSON.stringify($(this).data("extend2"));
                i.data("item", d).attr("data-item", d),
                $(this).parent().prev().find("div:eq(0)").html($(this).find("a").html().replace(",", "<br/>")),
                $(this).siblings().removeClass("active"),
                $(this).addClass("active"),
                i.find('td[field="price"]').attr("data-number", r).data("number", r),
                i.find('td[field="inventory"] div').text($(this).data("extend3")),
                app.edit_orders.format_price($(this).data("extend2"), t),
                app.edit_orders.chang_price(n, !1, e)
            }
        })
    })
    }
}

app.edit_orders.chang_price = function (t, e, i) {
    //;
    var a = parseFloat($.trim(t.find('td[field="number"] div').html())),
	d = parseFloat($.trim(t.find('td[field="price"] div').html()));
    if ("number_price" == publicSettings.goods_price_type) var n = $.DHB.isAjaxJson($.trim(t.find('td[field="price"]').data("number")), !0);
    e !== !0 && t.find('td[field="options"] ul.dropdown-menu').length > 0 && (d = parseFloat(t.find('td[field="options"] ul.dropdown-menu li.active').data("extend1")), "number_price" == publicSettings.goods_price_type && (n =

$.DHB.isAjaxJson(t.find('td[field="options"] ul.dropdown-menu li.active').data("extend2"), !0))),
	"number_price" == publicSettings.goods_price_type && (d = app.edit_orders.get_price(a, n, d)),
	t.find('td[field="number"] div').html($.DHB.format_number(a)),
	t.find('td[field="price"] div').html($.DHB.format_price(d)),
	t.find('td[field="count"] div').html($.DHB.format_price(parseFloat(a) * d)),
	app.edit_orders.table_result(i)
}

app.edit_orders.get_price = function (t, e, i) {
    if ($.isEmptyObject(e)) return parseFloat(i);
    i = 0;
    for (var a in e) parseFloat(t) >= parseFloat(a) && (i = e[a]);
    return parseFloat(i)
}

app.edit_orders.format_price = function (t, e) {
    if ("number_price" == publicSettings.goods_price_type) {
        var i = e.parents("tr");
        if (i.find('td[field="price"]').attr("title", ""), !$.isEmptyObject(t)) {
            var a = "";
            for (var d in t) a += 1 == d ? "商品单价 (￥" + t[d] + " / " + e.data("baseunits") + ")" : " | 满 " + d + " " + e.data("baseunits") + " 优惠价 (￥" + t[d] + " / " + e.data("baseunits") + ")";
            i.find('td[field="price"]').attr("title", a).niceTitle({
                showLink: !1
            })
        }
    }
}

app.edit_orders.table_result = function (t) {
    var e = [],
     shopsp = [],
	i = 0;
    t.find('tbody tr[data-item!=""]').each(function () {
        var t = {},
		a = app.edit.get_goodsid($(this).data("item"));
        if (
            t.goods_id = a.goods_id,
            t.options_id = a.options_id ? a.options_id : "0",
            t.num = parseFloat($.trim($(this).find('td[field="number"] div').text())),
            t.units = "0",
            t.base_units = $.trim($(this).find('td[field="units"]').data("base")),
            t.container_units = $.trim($(this).find('td[field="units"]').data("container")),
            t.price = parseFloat($.trim($(this).find('td[field="price"] div').text())),
            t.total_price = parseFloat($.trim($(this).find('td[field="count"] div').text())),
            t.remark = $.trim($(this).find('td[field="remark"] p').text()),
            t.conversion_number = 1,
            i += t.total_price,
            t.num = $.DHB.format_number(t.num),
            t.price = $.DHB.format_price(t.price),
            t.conversion_number = $.DHB.format_number(t.conversion_number),
            t.goods_name = $.trim($(this).find('td[field="name"] div.goods-name').text()),
            t.goods_num = $.trim($(this).find('td[field="name"] div.goods-num').text()),
            $(this).find('td[field="options"]').length > 0
            ) {
            if ($(this).find('td[field="options"] .goods-multioptions-id').length > 0)
                t.options_name = $.trim($(this).find('td[field="options"] .goods-multioptions-id button .cut-out').html().replace("<br>", ","));
            else {
                var d = $.trim($(this).find('td[field="options"] div').html());
                d ? t.options_name = d.replace("<br>", ",") : t.options_name = ""
            }
        }
        else
            t.options_name = "";
        e.push(t);

        var shopsp_e = {};
        shopsp_e.id_shopsp = $(this).data("item");//商品id
        shopsp_e.id_kcsp = $.trim($(this).find('td[field="id_kcsp"] div').text());//仓库id
        shopsp_e.sl = $.trim($(this).find('td[field="number"] div').text());//数量
        shopsp_e.barcode = $.trim($(this).find('td[field="barcode"] div').text());//条码
        shopsp_e.dw = $.trim($(this).find('td[field="units"] div').text());//单位
        shopsp_e.dj = $.trim($(this).find('td[field="price"] div').text());
        shopsp_e.bz = $.trim($(this).find('td[field="remark"] p').text());//备注

        shopsp.push(shopsp_e);

    }),

    //t.parent().find('input[name="table_result"]').val(JSON.stringify(e)),
    t.parent().find('input[name="table_result"]').val(JSON.stringify(shopsp)),
    $(_ + "#edit_goods_total").text($.DHB.format_price(i)),
    app.jh.addshopsp_extendmoney_callback()

};













app.edit.check = function (field, el) {
    if (!$(_ + "#" + field).val()) { return false; } else { return true; }
}

{
}
app.edit.clear = function (newItem, type) {
    var table = $(_ + ".dhb-table table"); if (table.data('new') != newItem) {
        var len = table.find("tbody>tr[data-item!='']").length; table.find("tbody>tr[data-item!='']").remove(); for (var i = 0; i < len; i++) {
            table.find("tbody [field='operate']:eq(0) a:eq(0)").click();
        }
    }
    table.data('new', newItem).attr('data-new', newItem); table.data('init', '0').attr('data-init', '0'); table.find('td[data-init="1"]').data('init', '0').attr('data-init', '0'); eval('app.edit_' + type + '.table_result(table);');
};


//TODO：DHB函数
jQuery.DHB = {    
    isShowMore: false, menuData: [{}, ''],
    //TODO：★加载完成调用此函数
    ready: function (focus, jsonce) {
        
        focus = focus || false;
        jsonce = jsonce || false;
        $(document).ready(function () {
            if (typeof (focus) == 'string') {
                _ = focus;
                //若有弹框刷新弹框里面的数据
                if ($(_ + ".modal[style='display: block;'] #btn-search")) {
                    //;
                    $(_ + ".modal[style='display: block;'] #btn-search").click();
                }
            }
            else {
                _ = $.fn.menuTab.current(1);
                //若有弹框刷新弹框里面的数据
                if ($(_ + ".modal[style='display: block;'] #btn-search")) {
                    //;
                    $(_ + ".modal[style='display: block;'] #btn-search").click();
                }
            }
            
            
            if (focus !== false) {
                jQuery.DHB._();
                jQuery.DHB._search();
                jQuery.DHB._threeMenu();
                if (jsonce === false) {
                    var objBoby = $(_);
                    if (objBoby.length > 0 && objBoby.attr('controller')) {
                        //;

                        var curInit = ('app.' + objBoby.attr('controller') + '.' + objBoby.attr('action') + '_ready').toLowerCase();
                        var curListcallback = ('app.' + objBoby.attr('controller') + '.' + objBoby.attr('action') + '_listready').toLowerCase();
                        eval('try {if(' + curInit + ' && typeof(' + curInit + ')=="function"){' + curInit + '(); }}catch(e){}');
                        eval('try {if(' + curListcallback + ' && typeof(' + curListcallback + ')=="function"){' + curListcallback + '(); }}catch(e){}');
                    }
                }
            }
        });
    },
    _: function () { },
    _search: function () { },
    _threeMenu: function () { },
    propagation: function () {
        $('.for-stoppropagation').removeClass('open');
    },
    propagation_: function (booCur) {
        $(_ + '.for-propagation .dropdown-menu').click(function (e) {
            e.stopPropagation();
        });
    },
    //TODO：显示进度条
    showButterbar: function () {
        $('.butterbar').addClass('active');
    },
    //TODO：隐藏进度条
    closeButterbar: function (leaveTime) {
        leaveTime = leaveTime || 500; setTimeout(function () { $('.butterbar').removeClass('active'); }, leaveTime);
    },
    //TODO：弹出层
    dialog: function (arrData) {
        
        var defaultData = {
            'show': 1,
            'url': '',
            'id': '',
            'focus': true,
            'nocache': 0,
            'title': '',
            'action': '',
            'body': '',
            'footer': '',
            'modal_ok': '确定',
            'modal_load': '提交中...',
            'modal_cancel': '取消',
            'modal_disabled': false,
            'modal_id': 'submit-button',
            'modal_type': 'submit',
            'modal_extend': '',

            //YZQ Inject CallBack
            'confirm': null
        };
        var settings = $.extend({}, defaultData, arrData);
        if (settings.id && settings.action) {
            if ($(_ + '#' + settings.id).length > 0) {
                switch (settings.action) {
                    case 'show': $(_ + '#' + settings.id + ' .modal').modal('show'); break;
                    case 'hide': $(_ + '#' + settings.id + ' .modal').modal('hide'); break;
                    case 'toggle': $(_ + '#' + settings.id + ' .modal').modal('toggle'); break;
                    case 'destroy': $(_ + '#' + settings.id).remove(); break;
                    default: $.DHB.message({ 'content': '错误的动作', 'time': 0, 'type': 'e' }); break;
                }
            }
            
        }
        else if (settings.id) {
            if (settings.url) {
                $.DHB.showButterbar();
                if ($(_ + '#' + settings.id).length <= 0 || settings.focus === true) {
                    if ($(_ + '#' + settings.id).length > 0) {
                        $(_ + "#" + settings.id).remove();
                    }
                    var options = {
                        url: settings.url,
                        data:{
                                    dialog_id: settings.id,
                                    _dialog_: 1
                        },
                        dataType: 'html',
                        async: false,
                        complete: function (xhr, ts) {
                            if (xhr.responseText) {
                                var tryJson = $.DHB.isAjaxJson(xhr.responseText, true);
                                if (typeof (tryJson) != 'object') {
                                    $(_).append('<div id="' + settings.id + '">' + xhr.responseText + '</div>');
                                    if (settings.title) { $(_ + '#' + settings.id + ' .modal-title').text(settings.title); }

                                    if (settings.show == '1') {
                                        setTimeout(function () {
                                            $(_ + '#' + settings.id + ' .modal').modal('show');
                                        }, 100);
                                        $(_ + '#' + settings.id + ' .modal').modal('show');
                                    }
                                    $.DHB.contentbind(_ + '#' + settings.id, false);
                                    //;
                                    //YZQ Inject CallBack
                                    $('#' + settings.id, _).delegate('button[data-confirm]', 'click', function (e) {
                                        //;
                                        if (settings.confirm && $.isFunction(settings.confirm)) {
                                            var $array_form = $($('#' + settings.id + ' form', _)).serializeArray();
                                            settings.confirm($array_form);
                                        }
                                    });

                                } else {
                                    $.DHB.message({ 'content': tryJson.message, 'time': 0, 'type': tryJson.status == 'success' ? 's' : 'e' });
                                }
                                $.DHB.closeButterbar();
                            } else {
                                $.DHB.closeButterbar();
                                $.DHB.message({ 'content': '加载失败', 'time': 0, 'type': 'e' });
                            }
                        }

                    }
                    app.httpAjax.post(options);
                    //$.ajax({
                    //    url: settings.url,
                    //    data: {
                    //        dialog_id: settings.id,
                    //        _dialog_: 1
                    //    },
                    //    async: false,
                    //    cache: (settings.nocache == '1' ? false : true),
                    //    dataType: 'html',
                    //    complete: function (xhr, ts) {
                    //        if (xhr.responseText) {
                    //            var tryJson = $.DHB.isAjaxJson(xhr.responseText, true);
                    //            if (typeof (tryJson) != 'object') {
                    //                $(_).append('<div id="' + settings.id + '">' + xhr.responseText + '</div>');
                    //                if (settings.title) { $(_ + '#' + settings.id + ' .modal-title').text(settings.title); }

                    //                if (settings.show == '1') {
                    //                    setTimeout(function () {
                    //                        $(_ + '#' + settings.id + ' .modal').modal('show');
                    //                    }, 100);
                    //                    $(_ + '#' + settings.id + ' .modal').modal('show');
                    //                }
                    //                $.DHB.contentbind(_ + '#' + settings.id, false);
                    //                //;
                    //                //YZQ Inject CallBack
                    //                $('#' + settings.id, _).delegate('button[data-confirm]', 'click', function (e) {
                    //                    //;
                    //                    if (settings.confirm && $.isFunction(settings.confirm)) {
                    //                        var $array_form = $($('#' + settings.id + ' form', _)).serializeArray();
                    //                        settings.confirm($array_form);
                    //                    }
                    //                });

                    //            } else {
                    //                $.DHB.message({ 'content': tryJson.message, 'time': 0, 'type': tryJson.status == 'success' ? 's' : 'e' });
                    //            }
                    //            $.DHB.closeButterbar();
                    //        } else {
                    //            $.DHB.closeButterbar();
                    //            $.DHB.message({ 'content': '加载失败', 'time': 0, 'type': 'e' });
                    //        }
                    //    }
                    //});
                }
            }
            else if (settings.body) {
                if ($(_ + '#' + settings.id).length <= 0 || settings.focus === true) {
                    var dialogHtml = '<div class="dialog modal fade">' + '<div class="modal-dialog">' + '<div class="modal-content">' + '<div class="modal-header" onmouseenter="$(this).find(\'.close\').css(\'display\',\'block\');" onmouseleave="$(this).find(\'.close\').css(\'display\',\'none\');">' + '<button type="button" class="close" style="display:none;">&times;</button>' + '<h4 class="modal-title"></h4>' + '</div>' + '<div class="modal-body"></div>' + '<div class="modal-footer">' + '<button type="' + settings.modal_type + '" class="modal-ok btn btn-info w-xs" id="' + settings.modal_id + '" data-loading-text="' + settings.modal_load + '" ' + (settings.modal_type === true ? ' disabled="disabled" ' : '') + ' ' + settings.modal_extend + '>' + settings.modal_ok + '</button>' + '<button type="button" class="modal-cancel btn btn-default w-xs" data-dismiss="modal">' + settings.modal_cancel + '</button>' + '</div>' + '</div>' + '</div>' + '</div>'; if ($(_ + '#' + settings.id).length > 0) { $(_ + "#" + settings.id).html(dialogHtml); } else { $(_).append('<div id="' + settings.id + '">' + dialogHtml + '</div>'); }
                    if ($(_ + settings.body).length > 0) { $(_ + "#" + settings.id + " .modal-body").html($(_ + settings.body).html()); }
                    if ($(_ + settings.footer).length > 0) { $(_ + "#" + settings.id + " .modal-footer").html($(_ + settings.footer).html()); }
                    if (settings.title) { $(_ + '#' + settings.id + ' .modal-title').text(settings.title); }
                    if (settings.show == '1') { $(_ + '#' + settings.id + ' .modal').modal('show'); }
                    $.DHB.contentbind(_ + '#' + settings.id, false);
                }
            }
            else {
                if (settings.title) { $(_ + '#' + settings.id + ' .modal-title').text(settings.title); }
                if (settings.show == '1') { $(_ + '#' + settings.id + ' .modal').modal('show'); }
            }
            $.DHB.ready(true, true);
        }
        else {
            $.DHB.message({ 'content': '未设置载入的URL地址或者ID值', 'time': 0, 'type': 'e' });
        }
    },
    //加载js
    loadJs: function (arrData, callFunc) {
        arrData = arrData || [];
        var doJs = function () {
            var now = arrData.shift();
            if (typeof (now) != 'undefined') {
                var sID = 'script-' + now['id'];
                if ($('script#' + sID).length <= 0) {
                    var objScript = document.createElement("script");
                    objScript.setAttribute("type", "text/javascript");
                    objScript.setAttribute("id", sID);
                    objScript.setAttribute("src", now['url']);
                    var heads = document.getElementsByTagName("head");
                    heads[0].appendChild(objScript);
                    setTimeout(function () { doJs(); }, 50);
                } else {
                    doJs();
                }
            }
            else {
                if (typeof (callFunc) == 'function') {
                    setTimeout(function () { callFunc(); }, 500);
                }
            }
        }
        doJs();
    },
    tree: function (arrData) {
        
        var defaultData = { 'title': '', 'width': '350', 'width_units': 'px', 'el': '', 'name': name || '', 'value': '', 'data': [], 'has_title': true, 'levels': 1, 'callback': function (cur, el, type, value) { }, 'class': '', 'more': false, 'min_width': 350, 'checkbox': false, 'checked_child': false, 'open': false }; var settings = $.extend({}, defaultData, arrData); var groups = {}; var zNodes = settings.data; var selectText = ''; if (zNodes.length > 0) {
            for (var i = 0; i < zNodes.length; i++) {
                if (selectText == '' && settings.value && settings.value == zNodes[i].id) {
                    selectText = zNodes[i].name;
                }
                if (groups[zNodes[i].pid]) { var tem = {}; tem['text'] = zNodes[i].name; tem['href'] = zNodes[i].id; tem['nodes'] = []; groups[zNodes[i].pid]['nodes'].push(tem); } else { groups[zNodes[i].pid] = {}; groups[zNodes[i].pid]['text'] = zNodes[i].name; groups[zNodes[i].pid]['href'] = zNodes[i].id; groups[zNodes[i].pid]['nodes'] = []; var tem = {}; tem['text'] = zNodes[i].name; tem['href'] = zNodes[i].id; tem['nodes'] = []; groups[zNodes[i].pid]['nodes'].push(tem); }
            }
            defaultData = groups;
        } else {
            defaultData = {};
        }
        var getDom = function (a) {
            if (typeof (a) == 'undefined' || typeof (a.nodes) == 'undefined' || $.isEmptyObject(a.nodes)) {
                return {};
            }
            var result = []; for (var i = 0; i < a.nodes.length; i++) {
                var temp = {}; temp['text'] = a.nodes[i]['text']; temp['href'] = a.nodes[i]['href']; var node = getDom(defaultData[a.nodes[i].href]); if (!$.isEmptyObject(node)) {
                    temp['nodes'] = node;
                }
                result.push(temp);
            }; return result;
        }
        var getSelectText = function (now) {
            var select = ''; if (zNodes.length > 0) {
                for (var i = 0; i < zNodes.length; i++) {
                    if (select == '' && now && now == zNodes[i].id) {
                        select = zNodes[i].name; return select;
                    }
                }
            }
            return select;
        }
        var clearSearch = function () { }
        if (!$.isEmptyObject(defaultData)) { defaultData = getDom(defaultData[0]); } else { defaultData = []; }
        if (settings.has_title === true) {
            defaultData.unshift({ href: '', text: settings.title });
        }
        if (settings.checkbox === true) {
            var shtml = '<div class="dhb-treeview-multi" style="width:' + settings.width + settings.width_units + ';" id="dhb-treeview-' + settings.name + '">' + '<div></div>' + '<input type="hidden" class="' + settings['class'] + ' valid" name="' + settings.name + '" value="' + settings.value + '" />' + '</div>';
        } else {
            var shtml = '<div class="btn-group bootstrap-select form-control input-sm dhb-treeview box-shawn search-result select2' + (settings.open === true ? ' open' : '') + '" style="width:' + settings.width + settings.width_units + ';" id="dhb-treeview-' + settings.name + '">' + '<button ' + (settings.open === false ? ' data-toggle="dropdown" ' : '') + ' class="btn btn-sm dropdown-toggle btn-default" type="button">' + '<span class="filter-option pull-left d-i-b cut-out">' + settings.title + '</span>&nbsp;' +
                                    (settings.open === false ? '<span class="bs-caret"><span class="caret"></span></span>' : '') + '</button>' + '<div class="dropdown-menu open" style="overflow: hidden;min-width:' + settings.min_width + 'px;width:100%;">' + '<div role="menu" class="dropdown-menu inner" style="max-height: 250px; overflow-y: auto;">' + '<div></div>' + '</div>' +
                                    (settings.more === true ? '<div class="list-group dhb-more"><a class="media list-group-item list-group-item-more" style="padding-left:13px;">+ 新增更多</a></div>' : '') + '</div>' + '<input type="hidden" class="' + settings['class'] + ' valid" name="' + settings.name + '" value="' + settings.value + '" />' + '</div>';
        }
        $(_ + settings.el).html(shtml); if (settings.checkbox === true) { var nodeTree = $(_ + settings.el).find('.dhb-treeview-multi>div'); } else { var nodeTree = $(_ + settings.el).find('.dropdown-menu>div:first()'); }
        if (settings.checked_child === true) { var checkIsOver = []; var bindTime = 0; }
        var $expandibleTree = nodeTree.treeview({
            data: defaultData, expandIcon: 'fa fa-angle-down', collapseIcon: 'fa fa-angle-up', checkedIcon: 'fa fa-check-square-o', uncheckedIcon: 'fa fa-square-o', levels: settings.levels, showCheckbox: settings.checkbox, searchResultColor: '', selectedColor: settings.checkbox === false ? '#428bca' : '', selectedBackColor: settings.checkbox === false ? '#edf1f2' : '', onNodeSelected: function (el, cur) {
                if (settings.checkbox === false) {
                    $(_ + settings.el).find('button .pull-left').text(cur.text); $(_ + settings.el).find('input').attr('value', cur.href).val(cur.href);
                    setTimeout(function () {
                        clearSearch();
                    }, 10); settings.callback(cur, $(_ + settings.el), cur.href);
                }
            }, onNodeChecked: function (event, node) {
                if (settings.checked_child === false) {
                    setTimeout(function () {
                        var selectID = []; nodeTree.find('li').each(function () {
                            if ($(this).find('span.check-icon').hasClass('fa-check-square-o')) {
                                selectID.push($(this).data('href'));
                            }
                        }); $(_ + settings.el).find('input').attr('value', selectID.join(',')).val(selectID.join(','));
                        settings.callback(node, $(_ + settings.el), 'checked', selectID.join(','));
                    }, 10);
                } else {
                    if (typeof (areaValue) != 'undefined' && areaValue.length > 0) {
                        var now = areaValue.shift(); if (typeof (now) != 'undefined') {
                            setTimeout(function () {
                                if ($(_ + settings.el).find('li[data-href="' + now + '"]').length <= 0) {
                                    var selectText2 = getSelectText(now);
                                    var findExpandibleNodess = function () { return $expandibleTree.treeview('search', [selectText2, { ignoreCase: false, exactMatch: true }]); };
                                    var expandibleNodes = findExpandibleNodess();
                                }
                                $(_ + settings.el).find('li[data-href="' + now + '"] span.check-icon').click();
                            }, 50);
                        } else {
                            areaValue = [];
                        }
                    } else {
                        var selectText = node.text;
                        var findExpandibleNodess = function () { return $expandibleTree.treeview('search', [selectText, { ignoreCase: false, exactMatch: true }]); };
                        var expandibleNodes = findExpandibleNodess(); $expandibleTree.treeview('expandNode', [expandibleNodes, { levels: 99, silent: true }]); if (bindTime != 0) { var now = checkIsOver.shift(); if (typeof (now) != 'undefined') { bindTime++; setTimeout(function () { nodeTree.find('li[data-href="' + now + '"] span.check-icon').click(); }, 50); } else { bindTime = 0; checkIsOver = []; setTimeout(function () { var selectID = []; nodeTree.find('li').each(function () { if ($(this).find('span.check-icon').hasClass('fa-check-square-o')) { selectID.push($(this).data('href')); } }); $(_ + settings.el).find('input').attr('value', selectID.join(',')).val(selectID.join(',')); settings.callback(node, $(_ + settings.el), 'checked', selectID.join(',')); }, 10); } } else {
                            bindTime++; var curLi = nodeTree.find('li[data-href="' + node.href + '"]'); var indentLength = curLi.find('span.indent').length; var lastIndex = null; if (curLi.next().length > 0) {
                                curLi.nextAll().each(function () { if (lastIndex === null && $(this).find('span.indent').length <= indentLength) { lastIndex = $(this).index(); return false; } else { if ($(this).index() == nodeTree.find('li').length - 1) { lastIndex = $(this).index(); if ($(this).find('span.fa-check-square-o').length <= 0) { checkIsOver.push($(this).data('href')); } } } }); if (lastIndex !== null) {
                                    curLi.nextAll().each(function () { if ($(this).index() >= lastIndex) { return false; } else { if ($(this).find('span.fa-check-square-o').length <= 0) { checkIsOver.push($(this).data('href')); } } });
                                }
                            }
                            var now = checkIsOver.shift(); if (typeof (now) != 'undefined') { setTimeout(function () { nodeTree.find('li[data-href="' + now + '"] span.check-icon').click(); }, 50); } else {
                                bindTime = 0; checkIsOver = []; setTimeout(function () { var selectID = []; nodeTree.find('li').each(function () { if ($(this).find('span.check-icon').hasClass('fa-check-square-o')) { selectID.push($(this).data('href')); } }); $(_ + settings.el).find('input').attr('value', selectID.join(',')).val(selectID.join(',')); settings.callback(node, $(_ + settings.el), 'checked', selectID.join(',')); }, 10);
                            }
                        }
                    }
                }
            }, onNodeUnchecked: function (event, node) {
                if (settings.checked_child === false) {
                    setTimeout(function () {
                        var selectID = []; nodeTree.find('li').each(function () {
                            if ($(this).find('span.check-icon').hasClass('fa-check-square-o')) {
                                selectID.push($(this).data('href'));
                            }
                        }); $(_ + settings.el).find('input').attr('value', selectID.join(',')).val(selectID.join(','));
                        settings.callback(node, $(_ + settings.el), 'checked', selectID.join(','));
                    }, 10);
                } else {
                    var selectText = node.text; var findExpandibleNodess = function () { return $expandibleTree.treeview('search', [selectText, { ignoreCase: false, exactMatch: true }]); }; var expandibleNodes = findExpandibleNodess(); $expandibleTree.treeview('expandNode', [expandibleNodes, { levels: 99, silent: true }]); if (bindTime != 0) { var now = checkIsOver.shift(); if (typeof (now) != 'undefined') { bindTime++; setTimeout(function () { nodeTree.find('li[data-href="' + now + '"] span.check-icon').click(); }, 50); } else { bindTime = 0; checkIsOver = []; setTimeout(function () { var selectID = []; nodeTree.find('li').each(function () { if ($(this).find('span.check-icon').hasClass('fa-square-o')) { selectID.push($(this).data('href')); } }); $(_ + settings.el).find('input').attr('value', selectID.join(',')).val(selectID.join(',')); settings.callback(node, $(_ + settings.el), 'checked', selectID.join(',')); }, 10); } } else {
                        bindTime++; var curLi = nodeTree.find('li[data-href="' + node.href + '"]'); var indentLength = curLi.find('span.indent').length; var lastIndex = null; if (curLi.next().length > 0) {
                            curLi.nextAll().each(function () { if (lastIndex === null && $(this).find('span.indent').length <= indentLength) { lastIndex = $(this).index(); return false; } else { if ($(this).index() == nodeTree.find('li').length - 1) { lastIndex = $(this).index(); if ($(this).find('span.fa-square-o').length <= 0) { checkIsOver.push($(this).data('href')); } } } }); if (lastIndex !== null) {
                                curLi.nextAll().each(function () { if ($(this).index() >= lastIndex) { return false; } else { if ($(this).find('span.fa-square-o').length <= 0) { checkIsOver.push($(this).data('href')); } } });
                            }
                        }
                        var now = checkIsOver.shift(); if (typeof (now) != 'undefined') { setTimeout(function () { nodeTree.find('li[data-href="' + now + '"] span.check-icon').click(); }, 50); } else {
                            bindTime = 0; checkIsOver = []; setTimeout(function () { var selectID = []; nodeTree.find('li').each(function () { if ($(this).find('span.check-icon').hasClass('fa-check-square-o')) { selectID.push($(this).data('href')); } }); $(_ + settings.el).find('input').attr('value', selectID.join(',')).val(selectID.join(',')); settings.callback(node, $(_ + settings.el), 'checked', selectID.join(',')); }, 10);
                        }
                    }
                }
            }, onNodeCollapsed: function () { setTimeout(function () { clearSearch(); }, 1); }, onNodeExpanded: function () {
                setTimeout(function () { clearSearch(); }, 1);
            }
        }); if (settings.value) {
            if (settings.checkbox === true) {
                if (settings.value) {
                    var areaValue = settings.value.split(','); var now = areaValue.shift(); if (typeof (now) != 'undefined') {
                        setTimeout(function () {
                            if ($(_ + settings.el).find('li[data-href="' + now + '"]').length <= 0) { var selectText2 = getSelectText(now); var findExpandibleNodess = function () { return $expandibleTree.treeview('search', [selectText2, { ignoreCase: false, exactMatch: true }]); }; var expandibleNodes = findExpandibleNodess(); }
                            $(_ + settings.el).find('li[data-href="' + now + '"] span.check-icon').click();
                        }, 50);
                    } else {
                        areaValue = [];
                    }
                }
            } else {
                var findExpandibleNodess = function () { return $expandibleTree.treeview('search', [selectText, { ignoreCase: false, exactMatch: true }]); }; var expandibleNodes = findExpandibleNodess(); $(_ + settings.el).find('li[data-href="' + settings.value + '"]').click();
            }
        }
    },
    dialog_: function () { app.c.resizePage(); $('.dialog').remove(); $('#global-msg').css('display', 'none'); },
    menuCookie: function (data, act) {
        
        act = act || 'add';
        if (data.id != 'index') {
            if (act == 'add') {
                $.DHB.menuData[0][data.id] = new Array(data.title, data.url);
            }
            else if (act == 'upd') {
                if (typeof ($.DHB.menuData[0][data.id]) != 'undefined') { $.DHB.menuData[0][data.id][1] = data.url; }
            }
            else {
                //;
                delete $.DHB.menuData[0][data.id];
            }
            $.DHB.menuData[1] = $("div.wrap-header-tabbox ul.first-level li.active").attr('menuID');
            $.cookie('dhb_' + publicSettings.moduleName + '_menutab', JSON.stringify($.DHB.menuData), {
                expires: 86400, path: '/'
            });
        }
    },
    //刷新
    refresh: function (isCache, extend1) {
        
        if (isCache == 'page') {
            var isPage = true;
            isCache = extend1 == 'cache' ? '1' : '0';
        }
        else {
            isCache = isCache == 'cache' ? '1' : '0'; var isPage = false;
        }
        var currentObjA = $('div.wrap-header-tabbox li.active a');
        if (currentObjA.attr('_id') == 'index') {
            ;
            //如果是首页
            //$.DHB.index(currentObjA.attr('_url'), '1');
            var homedata = { type: 0 };
                $.ajax({
                    url: '/Manager/QueryPageShowData',
                    data: homedata,
                    type: 'post',
                    contentType: "application/x-www-form-urlencoded;charset=utf-8",
                    crossDomain: true,
                    success: function (data) {

                        $('div[contentid=index]').html('');
                        $('div[contentid=index]').append(data);
                        // console.log(app.home.picfull());
                        if (app.home.picfull() == 'false') {

                            $('#container').parent().parent().addClass('homebg')
                        } else {
                            $('#container').highcharts(app.home.picfull());
                        }

                        //return data;
                    },
                    error: function () {
                        //console.log("error");
                    }
                });
            ////饼状图
                app.home.picdata = function () {
                    var payStr = $('#cartdata').val();
                    var picdatad = JSON.parse(payStr);
                    var picdata = [];
                    var kkaa = [];
                    var f = false, h = false;
                    var totalje = 0;
                    for (var i = 0; i < picdatad.length; i++) {
                        totalje += parseFloat(picdatad[i].je);

                        if (i == picdatad.length - 1) {
                            f = true;
                        }
                    }


                    for (var i = 0; i < picdatad.length; i++) {
                        var aa = [];
                        aa.push(picdatad[i].mc);
                        var kk = parseFloat(picdatad[i].je);
                        var jj = parseFloat(kk / totalje) * 100;
                        aa.push(parseFloat(jj.toFixed(2)));
                        picdata.push(aa);
                        if (i == picdatad.length - 1) {
                            h = true;
                        }
                    }
                    if (f == true && h == true) {
                        kkaa.push(picdata);
                        totalje = totalje.toFixed(2);
                        kkaa.push(totalje);
                        return kkaa;
                    }


                }

                app.home.picfull = function () {
                    var picdata = [];
                    picdata = app.home.picdata();
                    if (picdata == undefined) {
                        return 'false'
                    } else {
                        var pie_new = {
                            chart: {
                                type: 'pie',
                                options3d: {
                                    enabled: true,
                                    alpha: 45
                                }
                            },
                            title: {
                                text: '入帐总额' + picdata[1]
                            },
                            colors: [
                                '#71cf3b',
                                '#2abbe5',
                                '#fe8a5d',
                                '#fdb45d',
                                '#492970',
                                '#77a1e5',
                                '#c42525',
                                '#a6c96a',
                                '#f28f43'
                            ],
                            subtitle: {
                                //text: '3D donut in Highcharts'
                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            exporting: {
                                buttons: {
                                    exportButton: {
                                        enabled: false
                                    },
                                    printButton: {
                                        enabled: false
                                    }
                                }
                            },
                            labels: {                  //图表标签
                                exporting: {
                                    enabled: false  //设置导出按钮不可用
                                },
                            },
                            series: [{
                                name: '占总额百分比为',
                                data: picdata[0]
                            }]
                        }
                        return pie_new;
                    }

                };

           
        }
        else {
            //;
            var url = '';
            var objContent = $('[contentID="' + currentObjA.attr('_id') + '"] ');
            var strNode = objContent.attr('controller') + '/' + objContent.attr('action');
            if (isPage === true && typeof (app.c.public_data[strNode]['_current_page_']) != 'undefined') {
                url = currentObjA.attr('_url') + (objContent.find(".search-box-container").length > 0 ? '&' + objContent.find(".search-box-container").parents('form').serialize() : '') + '&pageSize=' + app.c.public_data[strNode]['_page_size_'] + '&page=' + app.c.public_data[strNode]['_current_page_']; isCache = '-1';
            } else {
                url = currentObjA.attr('_url');
            }
            $.fn.menuTab.load_({ 'url': url, 'id': currentObjA.attr('_id'), 'nocache': isCache, 'title': currentObjA.attr('_title') });
        }
        $.DHB.dialog_();
    },
    close: function (id) {
        if (!id) {
            var cur = $.fn.menuTab.current(); id = cur.attr('contentID');
        }
        //;
        $("#wrap-header-menu-move li[menuID=\"" + id + "\"]").find(".wrap-header-navdel").click();
    },
    loadMessage: function (id) {
        $.fn.menuTab.loadMessage(id);
    },
    load: function (settings) { $.fn.menuTab.load(settings); },
    center: function (url) {
        var urls = [];
        if (typeof (url) != 'object') { urls.push(url); } else { urls = url; }
        var refreshTab = function () {
            var nowUrl = urls.pop(); if (typeof (nowUrl) != 'undefined') {
                var strUrl = $.DHB.U(nowUrl, {});
                var tryTab = $('div.wrap-content-header .wrap-header-tabbox ul.first-level li>a[_url^="' + strUrl + '"]');
                if (tryTab.length > 0) {
                    var url = '';
                    var objContent = $('[contentID="' + tryTab.attr('_id') + '"] ');
                    var strNode = objContent.attr('controller') + '/' + objContent.attr('action');
                    if (typeof (app.c.public_data[strNode]['_current_page_']) != 'undefined') {
                        url = tryTab.attr('_url') + (objContent.find(".search-box-container").length > 0 ? '&' + objContent.find(".search-box-container").parents('form').serialize() : '') + '&pageSize=' + app.c.public_data[strNode]['_page_size_'] + '&page=' + app.c.public_data[strNode]['_current_page_'];
                    } else {
                        url = tryTab.attr('_url');
                    }
                    htmlobj = $.ajax({
                        url: url,
                        async: false,
                        cache: false,
                        dataType: 'html',
                        complete: function (xhr, ts) {
                            var id = tryTab.attr('_id');
                            _ = '[contentID="' + id + '"] ';
                            $('[contentID="' + id + '"]').html(xhr.responseText);
                            jQuery.DHB.ready(_);
                            $.DHB.contentbind(_);
                            setTimeout(function () {
                                refreshTab();
                            }, 50);
                        }
                    });
                } else {
                    refreshTab();
                }
            } else {
                jQuery.DHB.ready();
            }
        }
        refreshTab();
    },
    load_: function (settings) { $.fn.menuTab.load_(settings); },
    //TODO：消息提示
    message: function (arrData) {
        //;
        //var defaultData = { 'content': '页面载入中，请稍后...', 'time': 1500, 'modal': 0, 'close': 0, 'type': 'i' };
        var defaultData = { 'content': '页面载入中，请稍后...', 'time': 2000, 'modal': 0, 'close': 0, 'type': 'i', 'closed': null };
        var settings = $.extend({}, defaultData, arrData);
        if (settings.type == 's') {
            //settings.time = 2000;
            var sMessage = 'message-success';
            var sIcon = 'fa-check-circle';
        }
        else if (settings.type == 'e') {
            //settings.time = 5000;
            var sMessage = 'message-error';
            var sIcon = 'fa-times-circle';
        }
        else {
            //settings.time = 5000;
            var sMessage = 'message-info';
            var sIcon = 'fa-exclamation-circle';
        }

        if (settings.type == 'e') {
            $.messager.alert(
                '信息提示',
                "<div class=\"message-loading\"><p class=\"" + sMessage + "\"><i class=\"fa " + sIcon + "\"></i> " + settings.content + "</p></div>",
                settings.time,
                settings.reload,
                settings);
        }
        else {
            var msg = $('#global-msg');
            msg[0].className = settings.type == 's' ? 'msg-popup-s' : 'msg-popup-i';
            $('#global-msg i')[0].className = 'fa ' + sIcon;
            $('#global-msg .msg-content').html(settings.content);
            if (settings.time) {
                if (this.p_timer) {
                    clearTimeout(this.p_timer);
                }
                this.p_timer = setTimeout(function () {
                    msg.hide();
                    //YZQ Inject CallBack
                    if (settings && settings.closed && $.isFunction(settings.closed))
                        settings.closed();
                }, settings.time);
            }
            msg.show();
        }
    },
    //首页
    index: function (url, nocache) {
        if (url == 'init') {
            $.DHB.closeButterbar();
            $.fn.menuTab.bindMenuAdd();
            $.DHB.contentbind('');
            setTimeout(function () { app.c.cookieMenu(''); }, 200);
        }
        else if (url === true) {
            $("div.wrap-header-tabbox ul.first-level li[menuID='index']").click();
            $('html,body').animate({ scrollTop: 0 }, 10);
        }
        else {
            var tabHome = $("div.wrap-header-tabbox ul.first-level li[menuID='index']>a");
            $.DHB.showButterbar();
            htmlobj = $.ajax({
                url: tabHome.attr('_url'),
               // url:'/Manager/QueryPageShowData',
                async: false,
                cache: false,
                dataType: 'html',
                complete: function (xhr, ts) {
                    $.DHB.closeButterbar();
                    var id = tabHome.attr('_id');
                    _ = '[contentID="' + id + '"] ';
                    $('[contentID="' + id + '"]').html(xhr.responseText);
                    //console.log(xhr.responseText);
                    jQuery.DHB.ready(_);
                    $.DHB.contentbind(_);
                }
            });
        }
    },
    contentbind: function (curEl, footerFix) {
        
        curEl = $(curEl).length > 0 ? curEl + ' ' : ''; $(curEl + '.menuTab').menuTab();
        $(curEl + 'select.select2_').selectpicker({ liveSearch: true });
        $(curEl + 'select.select2').selectpicker();
        $(curEl + '[data-toggle="tooltip"]').tooltip({ html: true });
        $(curEl + '[data-toggle="popover"]').each(function () {
            if ($(this).parents('.showIcon').length > 0) {
                app.c.auxiliaryCopy($(this).parents('.showIcon').get(0), $(this).data('content'));
                $(this).remove();
            } else {
                $(this).popover({ trigger: $(this).attr('data-trigger'), html: true });
            }
        });
        if (footerFix !== false) {
            app.c.needFooterFixed();
        }
        $(curEl + '.niceTitle').niceTitle({ showLink: false });
        //;
        //var firstOperate = $(curEl + '.list-operate:first()');
        //var firstOperate = $(curEl + '.list-operate');
        //if (firstOperate.length > 0) {
        //    firstOperate.parents('tr').attr('data-init', '1').data('init', '1').find('.list-operate').each(function () {
        //        if ($(this).find('div:first()').css("display") == "none") {
        //            $(this).find('div:first()').show();
        //            $(this).find("p").css("visibility", "hidden");
        //        }
        //    });
        //}
    },
    checkForm: function () {

        ;
        var _defaultForm = $(_ + "form.validate"),
            checkRules = checkMessages = {},
            submitFun = function () {
                alert('提交成功!');
            },
            errorFun,
            succesFun; if (typeof (arguments[0]) == 'undefined') {
                alert('参数错误');
                return false;
            }
            else {
                var intIndex = 0; if (typeof (arguments[0]) == 'string') {
                    _defaultForm = $(_ + "form#" + arguments[0]); intIndex = 1;
                }
                if (typeof (arguments[intIndex]) != 'undefined') {
                    submitFun = arguments[intIndex]; intIndex++;
                }
                if (typeof (arguments[intIndex]) != 'undefined') {
                    checkRules = arguments[intIndex]; intIndex++;
                }
                if (typeof (arguments[intIndex]) != 'undefined') {
                    checkMessages = arguments[intIndex]; intIndex++;
                }
                if (typeof (arguments[intIndex]) != 'undefined') {
                    errorFun = arguments[intIndex]; intIndex++;
                }
                if (typeof (arguments[intIndex]) != 'undefined') {
                    succesFun = arguments[intIndex];
                }
            }
        $(document).ready(function () {
            _defaultForm.validate({
                ignore: ".bootstrap-select",
                rules: checkRules,
                messages: checkMessages,
                errorPlacement: function (error, element) {
                    if (!errorFun) {
                        if ($(element).hasClass('error') && !$(element).hasClass('valid')) {
                            if ($(element).hasClass('select2') || $(element).hasClass('select2_'))
                            {
                                var currentElement = $(element).parents('div.bootstrap-select').find('button');
                                currentElement.addClass('error');
                            } else if ($(element).parent().hasClass('dhb-treeview')) {
                                var currentElement = $(element).parent().find('button'); currentElement.addClass('error');
                            } else if ($(element).hasClass('city-validate'))
                            {
                                var currentElement = $(element).parent();
                                currentElement.addClass('error');
                            } else {
                                var currentElement = $(element);
                            }
                            currentElement.popover('destroy'); currentElement.popover({ 'trigger': 'hover', 'content': '<span class="_red">' + $(error).html() + '</span>', 'placement': 'bottom', 'html': true });
                        } else {
                            if ($(element).hasClass('select2') || $(element).hasClass('select2_')) {
                                var currentElement = $(element).parents('div.bootstrap-select').find('button');
                                currentElement.removeClass('error');
                            } else if ($(element).parent().hasClass('dhb-treeview')) {
                                var currentElement = $(element).parent().find('button');
                                currentElement.addClass('error');
                            } else if ($(element).hasClass('city-validate'))
                            {
                                var currentElement = $(element).parent();
                            } else {
                                var currentElement = $(element);
                            }
                            currentElement.popover('destroy');
                        }
                    }
                    else {
                        errorFun(error, element);
                    }
                },
                submitHandler: function () {
                    //;
                    submitFun();
                },
                success: function (label) {
                    ;
                    if (!succesFun) {
                        _defaultForm.find('.valid').popover('destroy');
                        _defaultForm.find('.valid').each(function () {
                            if ($(this).parent().hasClass('error')) {
                                $(this).parent().removeClass('error');
                            }
                            else {
                                $(this).parents('div.bootstrap-select').find('button').removeClass('error');
                            }
                        });
                    }
                    else {
                        succesFun(label);
                    }
                }
            });
        });
    },
    checkForm_: function (str) { $(str, _).change(function () { if ($(this).val()) { $(this).parent().find('button').removeClass('error').popover('destroy'); } }); },
    checkForm__: function (strNode) { $('body').on('click', function () { var arrNode = strNode.split(","); for (var i in arrNode) { var temNode = arrNode[i]; if ($(_ + "select[name='" + temNode + "']").val() != '') { $(_ + "select[name='" + temNode + "']").removeClass('error'); $(_ + "select[name='" + temNode + "']").parents('.bootstrap-select').find('button').removeClass('error'); } } }); },
    city: function (arrData, el) {
        var defaultData = { 'id': '', 'type': 'val', 'seg': '-', 'level': 2, 'field': '', 'value': '', 'position': 'auto', 'callback': '' };
        var settings = $.extend({}, defaultData, arrData);
        var currentCity = $(el).parents('.dhb-citybox-box');
        var tabChange = function (currentObj) { var index = currentObj.index(); var classs = currentCity.find(".city-tab-content"); currentObj.parent().children("li").attr("class", ""); currentObj.attr("class", "active"); classs.hide(); classs.eq(index).show(); }; var city_callback = function (code) { var cityCallback = settings.callback; if (cityCallback) { eval(cityCallback + '(\'' + code + '\');'); } }; var setValue = function (currentObj) {
            var strCity = ''; if (settings.level >= 2) {
                strCity += currentCity.find(".city-province a.active").text() + settings.seg;
            }
            if (settings.level == 3) {
                strCity += currentCity.find(".city-city a.active").text() + settings.seg;
            }
            strCity += currentObj.text(); if (settings.type == 'val') { currentCity.find('#' + settings.id).val(strCity); } else { currentCity.find('#' + settings.id).text(strCity); }
            currentCity.find('input[name="' + settings.field + '"]').val(currentObj.data('code')); city_callback(currentObj.data('code')); cityDetailBox.css('display', 'none');
        };
        var cityBind = function () {
            currentCity.find(".city-city").on('click', 'a', function () {
                if (!$(this).hasClass('active')) {
                    tabChange(currentCity.find(".city-tab-header li").eq(2)); if (settings.level == 3) { htmlobj = $.ajax({ url: $.DHB.U('Quote/Area/getCounty', $.extend({}, settings, { 'city_id': $(this).data('code') })), async: false, cache: true, dataType: 'html' }); currentCity.find(".city-county").html(htmlobj.responseText); countyBind(); } else { setValue($(this)); }
                    $(this).siblings().removeClass('active'); $(this).addClass('active'); currentCity.find(".city-tab-header li:eq(1) a").text($(this).text()); var countryTab = currentCity.find(".city-tab-header li:eq(2) a"); if (countryTab.length > 0) {
                        countryTab.text(countryTab.data('name'));
                    }
                }
            });
        };
        var countyBind = function () { currentCity.find(".city-county").on('click', 'a', function () { if (!$(this).hasClass('active')) { setValue($(this)); $(this).siblings().removeClass('active'); $(this).addClass('active'); currentCity.find(".city-tab-header li:eq(2) a").text($(this).text()); } }); };
        var cityDetailBox = currentCity.find('#citybox-' + settings.id);
        if (cityDetailBox.length > 0) {
            var inputName = currentCity.find('#' + settings.id); if (settings.position == 'auto') {
                if ($(window).height() - 270 - inputName.offset().top + $(document).scrollTop() < 0) {
                    settings.position = 'top';
                }
            }
            if (settings.position == 'top') { var topHeight = -273; } else { var topHeight = parseInt(inputName.height()) + 3; }
            cityDetailBox.css('top', topHeight + 'px'); cityDetailBox.toggle();
        }
        else {
            htmlobj = $.ajax({ url: $.DHB.U('Quote/Area/index', settings), async: false, cache: true, dataType: 'html' }); var inputName = currentCity.find('#' + settings.id); inputName.after(htmlobj.responseText); cityDetailBox = currentCity.find('#citybox-' + settings.id); if (settings.position == 'auto') {
                if ($(window).height() - 270 - inputName.offset().top + $(document).scrollTop() < 0) {
                    settings.position = 'top';
                }
            }
            if (settings.position == 'top') { var topHeight = -273; } else { var topHeight = parseInt(inputName.height()) + 3; }
            cityDetailBox.css('top', topHeight + 'px'); var nProvinceID = 0; var nCityID = 0; var nCountyID = 0; if (settings.value > 0) {
                if (settings.level == 2) { nCityID = settings.value; nProvinceID = nCityID.substring(0, 2) + '0000'; tabChange(currentCity.find(".city-tab-header li").eq(1)); } else if (settings.level == 3) { nCountyID = settings.value; nProvinceID = nCountyID.substring(0, 2) + '0000'; nCityID = nCountyID.substring(0, 4) + '00'; tabChange(currentCity.find(".city-tab-header li").eq(2)); } else { nProvinceID = settings.value; }
                if (nProvinceID > 0) { var curPro = currentCity.find(".city-province a[data-code='" + nProvinceID + "']"); curPro.addClass('active'); currentCity.find(".city-tab-header li:first() a").text(curPro.text()); }
                if (settings.level >= 2 && nProvinceID > 0) {
                    htmlobj = $.ajax({ url: $.DHB.U('Quote/Area/getCity', $.extend({}, settings, { 'province_id': nProvinceID })), async: false, cache: true, dataType: 'html' }); currentCity.find(".city-city").html(htmlobj.responseText); cityBind(); if (nCityID > 0) { var curCity = currentCity.find(".city-city a[data-code='" + nCityID + "']"); curCity.addClass('active'); currentCity.find(".city-tab-header li:eq(1) a").text(curCity.text()); }
                    if (settings.level == 3 && nCityID > 0) {
                        currentCity.find(".city-county").html('请先选择市'); htmlobj = $.ajax({ url: $.DHB.U('Quote/Area/getCounty', $.extend({}, settings, { 'city_id': nCityID })), async: false, cache: true, dataType: 'html' }); currentCity.find(".city-county").html(htmlobj.responseText); countyBind(); if (nCountyID > 0) { var curCounty = currentCity.find(".city-county a[data-code='" + nCountyID + "']"); curCounty.addClass('active'); currentCity.find(".city-tab-header li:eq(2) a").text(curCounty.text()); }
                    }
                }
            }
        }
        currentCity.on('click', '.city-del', function () { $(this).parents('.city-box').css('display', 'none'); });
        currentCity.on('click', '.city-box', function (e) { e.stopPropagation(); });
        currentCity.find(".city-tab-header").on('click', 'li', function () { tabChange($(this)); });
        currentCity.find(".city-province").on('click', 'a', function () {
            if (!$(this).hasClass('active')) {
                if (settings.level >= 2) { htmlobj = $.ajax({ url: $.DHB.U('Quote/Area/getCity', $.extend({}, settings, { 'province_id': $(this).data('code') })), async: false, cache: true, dataType: 'html' }); currentCity.find(".city-city").html(htmlobj.responseText); tabChange(currentCity.find(".city-tab-header li").eq(1)); cityBind(); if (settings.level == 3) { currentCity.find(".city-county").html('请先选择市'); } } else { setValue($(this)); }
                $(this).parents('.city-province').find('a').removeClass('active'); $(this).addClass('active'); currentCity.find(".city-tab-header li:first() a").text($(this).text()); var cityTab = currentCity.find(".city-tab-header li:eq(1) a"); var countryTab = currentCity.find(".city-tab-header li:eq(2) a"); if (cityTab.length > 0) {
                    cityTab.text(cityTab.data('name'));
                }
                if (countryTab.length > 0) {
                    countryTab.text(countryTab.data('name'));
                }
            }
        });
    },
    city_name: function (arrData) {
        var defaultData = { 'area_id': '', 'parent': 0, 'seg': '-', };
        var settings = $.extend({}, defaultData, arrData);
        htmlobj = $.ajax({ url: $.DHB.U('Quote/Area/getName', settings), async: false, cache: true, dataType: 'html' });
        return htmlobj.responseText;
    },
    clear_select_city: function (el, callback) {
        var par = $(el).parent(); par.find('input').val('');
        par.find('.city-box a.active').removeClass('active');
        par.find('.city-box ul.city-tab-header li:first()').click();
        var proTab = par.find(".city-tab-header li:first() a");
        var cityTab = par.find(".city-tab-header li:eq(1) a");
        var countryTab = par.find(".city-tab-header li:eq(2) a");
        proTab.text(proTab.data('name'));
        if (cityTab.length > 0) {
            cityTab.text(cityTab.data('name')); par.find(".city-city").html("");
        }
        if (countryTab.length > 0) {
            countryTab.text(countryTab.data('name')); par.find(".city-county").html("");
        }
        var city_callback = function (callbackArgs) { if (callbackArgs) { eval(callbackArgs + '(\'\');'); } };
        city_callback(callback);
    },
    //TODO：构造URL
    U: function (sUrl, extend, all) {
        
        sUrl = sUrl || '';
        extend = extend || {};
        var sReturnUrl = publicSettings.rootUrl;
        //;
        if (sUrl) {
            sExtendUrl = '';
            if (sUrl.indexOf('?') > 0) {
                arrTemp = sUrl.split('?');
                sUrl = arrTemp[0];
                sExtendUrl = arrTemp[1];
            }
            arrTemp = sUrl.split('/');
            var sModule = sController = sAction = '';
            if (arrTemp.length > 0) {
                sAction = arrTemp.pop();
            }
            else {
                sAction = $(_).attr('action');
            }
            if (arrTemp.length > 0) {
                sController = arrTemp.pop();
            }
            else {
                sController = $(_).attr('controller');
            }
            if (
                arrTemp.length > 0) {
                sModule = arrTemp.pop();
            }
            else {
                sModule = publicSettings.moduleName;
            }
            //sReturnUrl += '?module=' + sModule;
            //sReturnUrl += '&controller=' + sController;
            //sReturnUrl += '&action=' + sAction;
            sReturnUrl += sController;
            sReturnUrl += '/' + sAction;
            if (sExtendUrl) {
                //sReturnUrl += '&' + sExtendUrl;
                sReturnUrl += '?' + sExtendUrl;
            }
        }
        if (extend && $.param(extend)) {
            if (sReturnUrl.indexOf('?') > 0) {
                sReturnUrl += '&' + $.param(extend);
            }
            else {
                sReturnUrl += '?' + $.param(extend);
            }
        }
        if (all === true) { var returnData = { node: sModule + '@' + sController + '@' + sAction, url: sReturnUrl }; }
        else { var returnData = sReturnUrl; }
        return returnData;
    },
    //
    url: function (sUrl, extend, strTitle, strTabID, strExtend) {
        
        if (typeof (extend) == 'object') {
            var objUrl = $.DHB.U(sUrl, extend, true);
            if (strTitle == 'cache') { var cache = '1'; strTitle = strTabID; strTabID = strExtend; }
            else { var cache = '0'; }
        }
        else {
            var objUrl = $.DHB.U(sUrl, {}, true);
            if (extend == 'cache') { var cache = '1'; }
            else { var cache = '0'; }
        }
        if (typeof (app.menutab[objUrl.node]) != 'undefined') {
            if (!strTitle) {
                strTitle = app.menutab[objUrl.node].title;
            }
            if (!strTabID) {
                strTabID = app.menutab[objUrl.node].id;
            }
        }
        $.fn.menuTab.load({ url: objUrl.url, title: strTitle, id: strTabID, nocache: cache });
    },
    thinkUrl: function (sUrl) {
        var url = $.DHB.parse_url(sUrl);
        url = decodeURIComponent(url.search);
        url = url.replace('?url=/', '');
        if ($.trim(url).length == 0) {
            return publicSettings.homeNode;
        }
        //url = $.DHB.parse_url(url);
        //url = url.params;
        //;
        //if (typeof (url.controller) == 'undefined') {
        //    return publicSettings.homeNode;
        //}
        //else {
        //    var controller = url.controller;
        //    var action = url.action;
        //    delete url.module;
        //    delete url.controller;
        //    delete url.action;
        //    return controller.replace('controller=', '') + '/' + action.replace('action=', '') + (!$.isEmptyObject(url) ? '?' + $.param(url) : '');
        //}
        return url;
    },
    pagination_node: {},

    pagination: function (setting) {
        //
        var options = $.extend({
            link_to: "",
            row_total: 0,
            current_page: 0,
            filter_form: ".filter-form",
            page_box: "#Pagination",
            refresh_box: '',
            current_node: '',
            ready_once: false,
            callback: function (idx, jq) { },
            callback_: function (data, textStatus, idx, jq) { },
            dialog_pagesize: false
        },
        setting || {});
        //;
        if (options.ready_once === true) {
            var currentNode = '';
            if (options.current_node) {
                currentNode = '_' + options.current_node + '_';
            }
            else {
                currentNode = '_' + $(_).attr('controller') + '_' + $(_).attr('action') + '_';
            }
        }
        if (typeof (options.dialog_pagesize) === 'undefined' || !options.dialog_pagesize) {
        }
        else {
            if (options.dialog_pagesize === true) {
                options.page_size = parseInt(publicSettings.minPageSize);
            }
            else {
                options.page_size = parseInt(options.dialog_pagesize);
            }
        }

        $(document).ready(function () {
            //TODO：绘制分页控件UI
            
            $(options.page_box, _).pagination(options.row_total, {
                num_edge_entries: 1,
                num_display_entries: 4,
                current_page: parseInt(options.current_page),
                link_to: options.link_to,
                callback: function (idx, jq,add_n) {
                    
                    idx = parseInt(idx);
                    var $fm;
                    if (options.filter_form instanceof jQuery) {
                        $fm = options.filter_form;
                    }
                    else {
                        $fm = $(options.filter_form, _);
                    }
                    
                    if (this.link_to) {
                        if (add_n) {
                            //TODO：分页拼接url
                            var url = this.link_to + '?_search_=1&_page_=1' + ($fm.length > 0 ? '&' + $fm.serialize() : '')
                                + '&pageSize=' + add_n +
                                '&' + publicSettings.pageVar + '=' + idx;
                            //console.log(url);
                            var pagesize = add_n;
                        } else {
                        
                            //TODO：分页拼接url
                            var url = this.link_to + '?_search_=1&_page_=1' + ($fm.length > 0 ? '&' + $fm.serialize() : '')
                                + '&pageSize=' + this.items_per_page +
                                '&' + publicSettings.pageVar + '=' + idx;
                                //console.log(url);
                            var pagesize = this.items_per_page;
                        }
                        
                        if (options.refresh_box) {
                            $.DHB.showButterbar();
                            $.get(url,
                            function (data, textStatus) {
                                $.DHB.closeButterbar();
                                //TODO：此处影响分页数据绑定
                                //;
                                //console.log($(options.refresh_box, _));
                                $(options.refresh_box, _).html(data);
                                
                                $.DHB.contentbind(_ + options.refresh_box, false);
                                if (options.ready_once === true) {
                                    eval(currentNode + " = false;");
                                    $.DHB._(true);
                                }
                                if ($(_ + "[pagesize]").length > 0) {
                                    $(_ + "[pagesize]").val(pagesize);
                                }
                                if ($(_ + "[pagesize]").length > 0) {
                                    $(_ + "[page]").val(idx);
                                }
                                options.callback_(data, textStatus, idx, jq);
                            },
                            'html');
                        } else {
                            $.fn.menuTab.load_({
                                url: url,
                                nocache: 1
                            });
                            options.callback_(idx, jq);
                        }
                    }
                    return false;
                },
                items_per_page: options.page_size,
                prev_text: "<i class='fa fa-chevron-left'></i>",
                next_text: "<i class='fa fa-chevron-right'></i>",
                init_apply: false,
                dialog_pagesize: options.dialog_pagesize
            });
        });
    },
    singleUpload: function (arrData) {
        //;
        var defaultData = { 'config': 'base', 'callback': '', 'value': '', 'id': 'single-upload', 'title': '', };
        var settings = $.extend({}, defaultData, arrData);
        var objContent = $(_);
        //;
        settings['_controller_'] = objContent.attr('controller').toLowerCase();
        settings['_action_'] = objContent.attr('action');
        $.DHB.dialog({ url: $.DHB.U('/utility/uploadfiles', settings), id: settings.id, title: settings.title, show: '1' });
    },
    multiUpload: function (arrData) {
        var defaultData = { 'config': 'base', 'callback': '', 'calltype': 'all', 'type': 'other', 'id': 'multi-upload', 'title': '', 'maxnum': 0, 'already': 0 };
        var settings = $.extend({}, defaultData, arrData);
        var objContent = $(_);
        settings['_controller_'] = objContent.attr('controller').toLowerCase();
        settings['_action_'] = objContent.attr('action');
        $.DHB.dialog({ url: $.DHB.U('Quote/Upload/multi', settings), id: settings.id, title: settings.title, show: '1' })
    },
    is_float: function (inNumber) { return parseFloat(inNumber).toString() == inNumber; },
    is_int: function (inNumber) { return parseInt(inNumber).toString() == inNumber; },
    serializeForm: function (formID) {
        var felements = $(formID).map(function () { var elements = $.prop(this, "elements"); return elements ? $.makeArray(elements) : this; }).filter(function () { return this.name; }).map(function (i, elem) {
            switch (elem.type) {
                case "checkbox": var $this = $(this); if ($this.attr('trueValue') && $this.attr('falseValue')) { val = $this.prop('checked') ? $.trim($this.attr('trueValue')) : $.trim($this.attr('falseValue')); } else { val = $this.prop('checked') ? $this.val() : ''; }
                    break; default: val = $(this).val(); break;
            }
            return { name: elem.name, value: val };
        }).get(); return $.param(felements);
    },
    parse_url: function (url) {
        var targetA = document.createElement('a');
        targetA.href = url;
        return {
            host: targetA.host,
            port: targetA.port,
            hash: targetA.hash,
            search: targetA.search,
            hostname: targetA.hostname,
            pathname: targetA.pathname,
            protocol: targetA.protocol.replace(':', ''),
            params: (function () {
                var myparam = {},
                    search = targetA.search.replace('?', '').split('&'),
                    length = search.length;
                var current;
                for (var i = 0; i < length; i++) {
                    if (!search[i])
                        continue;
                    current = search[i].split('=');
                    myparam[current[0]] = decodeURIComponent(current[1]).replace(/\+/g, ' ');
                }
                return myparam;
            })()
        }
    },
    isAjaxJson: function (CheckData, booReturnData) {
        var booIsJson = false;
        if (typeof (CheckData) == 'object') { booIsJson = true; }
        else {
            CheckData = $.trim(CheckData);
            try {
                CheckData = eval('(' + CheckData + ')');
                booIsJson = true;
            } catch (e) { }
        }
        if (booReturnData === true) { return CheckData; }
        else { return booIsJson; }
    },
    number_format: function (number) {
        var decimals = typeof (arguments[1]) != 'undefined' ? arguments[1] : 2; var decimalpoint = typeof (arguments[2]) != 'undefined' ? arguments[2] : '.'; var separator = typeof (arguments[3]) != 'undefined' ? arguments[3] : ','; number = parseFloat(number); number = number.toFixed(decimals); if (separator == '') { return number; } else {
            var temNumber = number.split('.'); var number1 = temNumber[0]; var arrNumber1 = number1.split(''); var str = ''; for (var i = 1; i <= arrNumber1.length; i++) {
                str = arrNumber1[arrNumber1.length - i] + str; if (i % 3 == 0) { str = separator + str; }
            }
            str = number1.length % 3 == 0 ? str.substr(1) : str; number1 = str + (typeof (temNumber[1]) != 'undefined' ? decimalpoint + temNumber[1] : ''); return number1;
        }
    },
    set_number_format: function (strData, decimals, comma) { if (typeof (decimals) == 'undefined') decimals = 2; if (typeof (comma) == 'undefined') comma = true; return $.DHB.number_format(strData, decimals, ".", comma === true ? "," : ""); },
    format_price: function (price, comma) { if (typeof (comma) == 'undefined') comma = false; return $.DHB.set_number_format(price, publicSettings.set_accuracy_price, comma); },
    format_number: function (number, comma) { if (typeof (comma) == 'undefined') comma = false; return $.DHB.set_number_format(number, publicSettings.set_accuracy_number, comma); },

    client: {
        client: function (el) {
            var searchClient = function () {
                var searchObj = $(el).parent().find('.dropdown-menu .list-group'); $.fn.menuTab.loadMessage(searchObj); $.get($.DHB.U('gys/search?_search_dropdown_=1'), {
                    value: $(el).val(), client_type: $(el).data('client_type')
                }, function (data, textStatus) {
                    data = $.DHB.isAjaxJson(data, true);
                    if (typeof (data) == 'object') {
                        $(el).parent().addClass('open');
                        searchObj.html('<p class="media list-group-item">' + data.one.client_name + '</p>');
                        $(_ + '#' + $(el).data('id')).val(data.one.client_id);
                        $(el).val(data.one.client_name);
                    } else {
                        searchObj.html(data);
                        searchSize = searchObj.find('a').size();
                    }
                });
            }
            var selectResult = function (intIndex) {
                var searchObj = $(el).parent().find('.dropdown-menu .list-group');
                searchObj.find('a').removeClass('active').find('div>div').css('color', '#999');
                searchObj.find('a').eq(intIndex).addClass('active').find('div>div').css('color', '#fff');
                currentIndex = intIndex;
                var scrollToObj = searchObj.find('a').eq(intIndex);
                searchObj.animate({ scrollTop: scrollToObj.offset().top - searchObj.offset().top + searchObj.scrollTop() }, 100); selectItem = true;
            }
            var select_callback = function (client_id) {
                var selectCallback = $(el).data('callback');
                if (selectCallback) {
                    eval(selectCallback + '(\'' + client_id + '\');');
                }
            }
            if ($(window).height() - 250 - $(el).offset().top + $(document).scrollTop() < 0) { $(el).parents('.input-group').addClass('dropup'); } else { $(el).parents('.input-group').removeClass('dropup'); }
            if ($(el).data('init') == '0') {
                searchClient(); var searchObj = $(el).parent().find('.dropdown-menu .list-group'); searchObj.on('click', 'a', function (e) { $(_ + '#' + $(el).data('id')).val($(this).attr('dataId')); $(el).val($(this).attr('dataName')); $(el).parent().removeClass('open'); select_callback($(this).attr('dataId')); }); searchObj.on('click', '.moreQ', function (e) { $(el).parents('.input-group').removeClass('open'); $.DHB.client.select_client({ 'title': $(el).data('dialog_title'), 'client_name': $(el).data('name'), 'client_id': $(el).data('id'), 'client_callback': $(el).data('callback'), 'client_type': $(el).data('client_type') }); }); $(el).data('init', '1');
            }
            var searchSize = 0; var currentIndex = 0; var selectItem = false; if ($(el).data('initenter') == '0') {
                $(el).parent().unbind('keydown').bind('keydown', 'return', function (e) {
                    if ($(el).is(':focus')) { if ($(el).parent().find('.dropdown-menu .list-group a.active').length <= 0) { searchClient(); } else { var searchObj = $(el).parent().find('.dropdown-menu .list-group'); curSel = searchObj.find('a').eq(currentIndex); $(_ + '#' + $(el).data('id')).val(curSel.attr('dataId')); $(el).val(curSel.attr('dataName')); $(el).parent().removeClass('open'); searchObj.find('a').removeClass('active').find('div>div').css('color', '#999'); select_callback(curSel.attr('dataId')); } }
                    $(el).data('initenter', '1'); return false;
                });
            }
            if ($(el).data('initkeydown') == '0') {
                $(el).parent().unbind('keydown').bind('keydown', 'up', function (e) {
                    if ($(el).is(':focus')) {
                        if (currentIndex == -1 || currentIndex == 0) { selectResult(searchSize - 1); } else if (currentIndex > 0) {
                            selectResult(currentIndex - 1);
                        }
                    }
                    return false;
                }); $(el).parent().unbind('keydown').bind('keydown', 'down', function (e) {
                    if ($(el).is(':focus')) {
                        if (currentIndex == -1 || currentIndex == (searchSize - 1)) { selectResult(0); } else if (currentIndex < (searchSize - 1)) {
                            selectResult(currentIndex + 1);
                        }
                    }
                    return false;
                }); $(el).data('keydown', '1');
            }
        },
        select_client: function (arrData) {
            var defaultData = { 'title': '选择客户', 'client_name': 'edit_client_name', 'client_id': 'edit_client_id', 'client_callback': '', 'client_type': '' };
            arrData = $.extend({}, defaultData, arrData);
            $.DHB.dialog({
                'title': arrData.title,
                'url': $.DHB.U('gys/list', { pageSize: publicSettings.minPageSize, edit_client_name: arrData.client_name, edit_client_id: arrData.client_id, client_callback: arrData.client_callback, client_type: arrData.client_type }), 'id': 'client_dialog'
            });
        },
        clear_select_client: function (el) {
            var select_callback = function () {
                var selectCallback = $(el).data('callback'); if (selectCallback) {
                    eval(selectCallback + '();');
                }
            }
            $(_ + '#' + $(el).data('id')).val(''); $(_ + '#' + $(el).data('name')).val(''); select_callback();
        },

        select_gys: function (arrData) {
            var defaultData = { 'title': '选择供应商', 'gys_name': 'gys_name', 'gys_id': 'gys_id', 'gys_callback': '', 'gys_type': '', 'dialog_id': 'dialog-gys-search' };
            arrData = $.extend({}, defaultData, arrData);
            $.DHB.dialog({
                'title': arrData.title,
                'url': $.DHB.U('gys/search', { pageSize: publicSettings.minPageSize, gys_name: arrData.gys_name, gys_id: arrData.gys_id, gys_callback: arrData.gys_callback, gys_type: arrData.gys_type }),
                'id': arrData.dialog_id
            });
        },

        clear_select_gys: function (el) {
            var select_callback = function () {
                var selectCallback = $(el).data('callback');
                if (selectCallback) {
                    eval(selectCallback + '();');
                }
            }
            $(_ + '#' + $(el).data('id')).val('');
            $(_ + '#' + $(el).data('name')).val('');
            select_callback();
        },


        select_kh: function (arrData) {
            var defaultData = { 'title': '选择客户', 'kh_name': 'kh_name', 'kh_id': 'kh_id', 'kh_callback': '', 'kh_type': '', 'dialog_id': 'dialog-kh-search' };
            arrData = $.extend({}, defaultData, arrData);
            $.DHB.dialog({
                'title': arrData.title,
                'url': $.DHB.U('kh/search', { pageSize: publicSettings.minPageSize, kh_name: arrData.kh_name, kh_id: arrData.kh_id, kh_callback: arrData.kh_callback, kh_type: arrData.kh_type }),
                'id': arrData.dialog_id
            });
        },

        


        select_dh_list: function (arrData) {
            //;

            var defaultData = { 'title': '选择订货单', 'dh_origin': 'dh_origin', 'id_bill_origin': 'id_bill_origin', 'bm_djlx_origin': 'bm_djlx_origin', 'dh_callback': '', 'jh_type': '', 'dialog_id': 'dialog-dh-search' };
            arrData = $.extend({}, defaultData, arrData);
            $.DHB.dialog({
                'title': arrData.title,
                'url': $.DHB.U('Dh/Search', { pageSize: publicSettings.minPageSize, dh_origin: arrData.dh_origin, id_bill_origin: arrData.id_bill_origin, bm_djlx_origin: arrData.bm_djlx_origin, dh_callback: arrData.dh_callback, jh_type: arrData.jh_type }),
                'id': arrData.dialog_id
            });
        },



        clear_select_dh: function (el) {

            var select_callback = function () {
                var selectCallback = $(el).data('callback');
                if (selectCallback) {
                    eval(selectCallback + '();');
                }
            }
            $(_ + '#' + $(el).data('id')).val('');
            $(_ + '#' + $(el).data('name')).val('');

            select_callback();
        },



        select_xs_list: function (arrData) {
            var defaultData = { 'title': '选择销售订单', 'xs_origin': 'xs_origin', 'id_bill_origin': 'id_bill_origin', 'bm_djlx_origin': 'bm_djlx_origin', 'xs_callback': '', 'xs_type': '', 'dialog_id': 'dialog-xs-show-search' };
            arrData = $.extend({}, defaultData, arrData);
            $.DHB.dialog({
                'title': arrData.title,
                'url': $.DHB.U('xs/search', { pageSize: publicSettings.minPageSize, xs_origin: arrData.xs_origin, id_bill_origin: arrData.id_bill_origin, bm_djlx_origin: arrData.bm_djlx_origin, xs_callback: arrData.xs_callback, xs_type: arrData.xs_type }),
                'id': arrData.dialog_id
            });
        },


        clear_select_xs: function (el) {
            var select_callback = function () {
                var selectCallback = $(el).data('callback');
                if (selectCallback) {
                    eval(selectCallback + '();');
                }
            }
            $(_ + '#' + $(el).data('id')).val('');
            $(_ + '#' + $(el).data('name')).val('');
            select_callback();
        },


        select_xsck_list: function (arrData) {
            var defaultData = { 'title': '选择销售出库单', 'xs_origin': 'xs_origin', 'id_bill_origin': 'id_bill_origin', 'bm_djlx_origin': 'bm_djlx_origin', 'xs_callback': '', 'xs_type': '', 'dialog_id': 'dialog-xsck-show-search' };
            arrData = $.extend({}, defaultData, arrData);
            $.DHB.dialog({
                'title': arrData.title,
                'url': $.DHB.U('xsck/search', { pageSize: publicSettings.minPageSize, xs_origin: arrData.xs_origin, id_bill_origin: arrData.id_bill_origin, bm_djlx_origin: arrData.bm_djlx_origin, xs_callback: arrData.xs_callback, xs_type: arrData.xs_type }),
                'id': arrData.dialog_id
            });
        },






        select_jh_list: function (arrData) {
            //;

            var defaultData = { 'title': '选择进货单', 'dh_origin': 'dh_origin', 'id_bill_origin': 'id_bill_origin', 'bm_djlx_origin': 'bm_djlx_origin', 'jh_callback': '', 'jh_type': '', 'dialog_id': 'dialog-jh-search' };
            arrData = $.extend({}, defaultData, arrData);
            $.DHB.dialog({
                'title': arrData.title,
                'url': $.DHB.U('Jh/Search', { pageSize: publicSettings.minPageSize, dh_origin: arrData.dh_origin, id_bill_origin: arrData.id_bill_origin, bm_djlx_origin: arrData.bm_djlx_origin, jh_callback: arrData.jh_callback, jh_type: arrData.jh_type }),
                'id': arrData.dialog_id
            });
        },

        clear_select_jh: function (el) {

            //;
                var select_callback = function () {
                    var selectCallback = $(el).data('callback');
                    if (selectCallback) {
                        eval(selectCallback + '();');
                    }
                }
                $(_ + '#' + $(el).data('id')).val('');
                $(_ + '#' + $(el).data('name')).val('');

                select_callback();
            },

        gys: function (el) {
            var searchGys = function () {
                var searchObj = $(el).parent().find('.dropdown-menu .list-group');
                $.fn.menuTab.loadMessage(searchObj);

                //$.get($.DHB.U('gys/search?_search_dropdown_=1'), {
                //    value: $(el).val(), client_type: $(el).data('gys_type')
                //},
                //function (data, textStatus) {
                //    data = $.DHB.isAjaxJson(data, true);
                //    if (typeof (data) == 'object') {
                //        $(el).parent().addClass('open');
                //        searchObj.html('<p class="media list-group-item">' + data.one.gys_name + '</p>');
                //        $(_ + '#' + $(el).data('id')).val(data.one.client_id);
                //        $(el).val(data.one.gys_name);
                //    } else {
                //        searchObj.html(data);
                //        searchSize = searchObj.find('a').size();
                //    }
                //});
            }
            var selectResult = function (intIndex) {
                var searchObj = $(el).parent().find('.dropdown-menu .list-group');
                searchObj.find('a').removeClass('active').find('div>div').css('color', '#999');
                searchObj.find('a').eq(intIndex).addClass('active').find('div>div').css('color', '#fff');
                currentIndex = intIndex;
                var scrollToObj = searchObj.find('a').eq(intIndex);
                searchObj.animate({ scrollTop: scrollToObj.offset().top - searchObj.offset().top + searchObj.scrollTop() }, 100); selectItem = true;
            }
            var select_callback = function (client_id) {
                var selectCallback = $(el).data('callback');
                if (selectCallback) {
                    eval(selectCallback + '(\'' + client_id + '\');');
                }
            }
            if ($(window).height() - 250 - $(el).offset().top + $(document).scrollTop() < 0) { $(el).parents('.input-group').addClass('dropup'); } else { $(el).parents('.input-group').removeClass('dropup'); }
            if ($(el).data('init') == '0') {
                searchGys();
                var searchObj = $(el).parent().find('.dropdown-menu .list-group');
                searchObj.on('click', 'a', function (e) {
                    $(_ + '#' + $(el).data('id')).val($(this).attr('dataId'));
                    $(el).val($(this).attr('dataName'));
                    $(el).parent().removeClass('open');
                    select_callback($(this).attr('dataId') + '|' + $(this).attr('dataName'));
                });
                searchObj.on('click', '.moreQ', function (e) {
                    $(el).parents('.input-group').removeClass('open');
                    $.DHB.client.select_gys({ 'title': $(el).data('dialog_title'), 'gys_name': $(el).data('name'), 'gys_id': $(el).data('id'), 'gys_callback': $(el).data('callback'), 'gys_type': $(el).data('gys_type') });
                }); $(el).data('init', '1');
            }
            var searchSize = 0; var currentIndex = 0; var selectItem = false; if ($(el).data('initenter') == '0') {
                $(el).parent().unbind('keydown').bind('keydown', 'return', function (e) {
                    if ($(el).is(':focus')) {
                        if ($(el).parent().find('.dropdown-menu .list-group a.active').length <= 0) {
                            searchGys();
                        } else {
                            var searchObj = $(el).parent().find('.dropdown-menu .list-group');
                            curSel = searchObj.find('a').eq(currentIndex); $(_ + '#' + $(el).data('id')).val(curSel.attr('dataId'));
                            $(el).val(curSel.attr('dataName'));
                            $(el).parent().removeClass('open');
                            searchObj.find('a').removeClass('active').find('div>div').css('color', '#999');
                            select_callback(curSel.attr('dataId') + '|' + curSel.attr('dataName'));
                        }
                    }
                    $(el).data('initenter', '1'); return false;
                });
            }
            if ($(el).data('initkeydown') == '0') {
                $(el).parent().unbind('keydown').bind('keydown', 'up', function (e) {
                    if ($(el).is(':focus')) {
                        if (currentIndex == -1 || currentIndex == 0) { selectResult(searchSize - 1); } else if (currentIndex > 0) {
                            selectResult(currentIndex - 1);
                        }
                    }
                    return false;
                }); $(el).parent().unbind('keydown').bind('keydown', 'down', function (e) {
                    if ($(el).is(':focus')) {
                        if (currentIndex == -1 || currentIndex == (searchSize - 1)) { selectResult(0); } else if (currentIndex < (searchSize - 1)) {
                            selectResult(currentIndex + 1);
                        }
                    }
                    return false;
                }); $(el).data('keydown', '1');
            }
        }

    },
    staff: {
        staff: function (el) {
            var searchStaff = function () {
                var searchObj = $(el).parent().find('.dropdown-menu .list-group'); $.fn.menuTab.loadMessage(searchObj); $.get($.DHB.U('Quote/Public/staffInput'), {
                    value: $(el).val(), staff_type: $(el).data('stafftype')
                }, function (data, textStatus) {
                    data = $.DHB.isAjaxJson(data, true); if (typeof (data) == 'object') { $(el).parent().addClass('open'); searchObj.html('<p class="media list-group-item">' + data.one.staff_name + '</p>'); $(_ + '#' + $(el).data('id')).val(data.one.staff_id); $(el).val(data.one.staff_name); } else { searchObj.html(data); searchSize = searchObj.find('a').size(); }
                });
            }
            var selectResult = function (intIndex) { var searchObj = $(el).parent().find('.dropdown-menu .list-group'); searchObj.find('a').removeClass('active').find('div>div').css('color', '#999'); searchObj.find('a').eq(intIndex).addClass('active').find('div>div').css('color', '#fff'); currentIndex = intIndex; var scrollToObj = searchObj.find('a').eq(intIndex); searchObj.animate({ scrollTop: scrollToObj.offset().top - searchObj.offset().top + searchObj.scrollTop() }, 100); selectItem = true; }
            var select_callback = function (staff_id) {
                var selectCallback = $(el).data('callback'); if (selectCallback) {
                    eval(selectCallback + '(\'' + staff_id + '\');');
                }
            }
            if ($(window).height() - 250 - $(el).offset().top + $(document).scrollTop() < 0) { $(el).parents('.input-group').addClass('dropup'); } else { $(el).parents('.input-group').removeClass('dropup'); }
            if ($(el).data('init') == '0') {
                searchStaff(); var searchObj = $(el).parent().find('.dropdown-menu .list-group'); searchObj.on('click', 'a', function (e) { $(_ + '#' + $(el).data('id')).val($(this).attr('dataId')); $(el).val($(this).attr('dataName')); $(el).parent().removeClass('open'); select_callback($(this).attr('dataId')); }); searchObj.on('click', '.moreQ', function (e) { $(el).parents('.input-group').removeClass('open'); $.DHB.staff.select_staff({ 'title': $(el).data('dialog_title'), 'staff_name': $(el).data('name'), 'staff_id': $(el).data('id'), 'staff_type': $(el).data('stafftype'), 'staff_callback': $(el).data('callback'), 'type': $(el).data('type') }); }); $(el).data('init', '1');
            }
            var searchSize = 0; var currentIndex = 0; var selectItem = false; if ($(el).data('initenter') == '0') {
                $(el).parent().unbind('keydown').bind('keydown', 'return', function (e) {
                    if ($(el).is(':focus')) { if ($(el).parent().find('.dropdown-menu .list-group a.active').length <= 0) { searchStaff(); } else { var searchObj = $(el).parent().find('.dropdown-menu .list-group'); curSel = searchObj.find('a').eq(currentIndex); $(_ + '#' + $(el).data('id')).val(curSel.attr('dataId')); $(el).val(curSel.attr('dataName')); $(el).parent().removeClass('open'); searchObj.find('a').removeClass('active').find('div>div').css('color', '#999'); select_callback(curSel.attr('dataId')); } }
                    $(el).data('initenter', '1'); return false;
                });
            }
            if ($(el).data('initkeydown') == '0') {
                $(el).parent().unbind('keydown').bind('keydown', 'up', function (e) {
                    if ($(el).is(':focus')) {
                        if (currentIndex == -1 || currentIndex == 0) { selectResult(searchSize - 1); } else if (currentIndex > 0) {
                            selectResult(currentIndex - 1);
                        }
                    }
                    return false;
                }); $(el).parent().unbind('keydown').bind('keydown', 'down', function (e) {
                    if ($(el).is(':focus')) {
                        if (currentIndex == -1 || currentIndex == (searchSize - 1)) { selectResult(0); } else if (currentIndex < (searchSize - 1)) {
                            selectResult(currentIndex + 1);
                        }
                    }
                    return false;
                }); $(el).data('keydown', '1');
            }
        },
        select_staff: function (arrData) { var defaultData = { 'title': '选择员工', 'staff_name': 'edit_staff_name', 'staff_id': 'edit_staff_id', 'staff_type': '', 'staff_callback': '', 'type': '' }; arrData = $.extend({}, defaultData, arrData); $.DHB.dialog({ 'title': arrData.title, 'url': $.DHB.U('Quote/Public/staffDialog', { pageSize: publicSettings.minPageSize, edit_staff_name: arrData.staff_name, edit_staff_id: arrData.staff_id, staff_callback: arrData.staff_callback, staff_type: arrData.staff_type, type: arrData.type }), 'id': 'select_staff_dialog' }); },
        clear_select_staff: function (el) {
            var select_callback = function () {
                var selectCallback = $(el).data('callback'); if (selectCallback) {
                    eval(selectCallback + '();');
                }
            }
            $(_ + '#' + $(el).data('id')).val(''); $(_ + '#' + $(el).data('name')).val(''); select_callback();
        }
    },



    table: {
        init: function (el) {
            if ($(el).data('init') == '0') {
                if ($.DHB.table.check(el)) {

                    $(el).data('init', '1').attr('data-init', '1');
                    $('body').click(function () {
                        $(el).find('td[field="name"]').each(function (e) {
                            if ($(this).parent().data('item') == '') {
                                if ($(this).data('init') == '1') {
                                    $(this).find('.goods1').show();
                                    $(this).find('.goods2').hide();
                                    $(this).find('.goods3').hide();
                                }
                            }
                            else {
                                $(this).find('.goods1').hide(); $(this).find('.goods2').show();
                                $(this).find('.goods3').hide();
                            }
                        });
                    });
                    $(el).find('[field="name"]').click(function (e) {
                        if ($.DHB.table.check(el)) {
                            $.DHB.table.goods($(this));
                            e.stopPropagation();
                        }
                    });
                    $.DHB.func.listOperate(el);
                }
                else {
                    if ($(el).data('enterinit') == '0') {
                        $(el).find('[field="name"]').click(function (e) {
                            if (!$.DHB.table.check(el)) {
                                $.DHB.message({ content: $(el).data('type') == 'orders' || $(el).data('type') == 'returns' ? '请先选择供应商' : '请先选择供应商', time: 1000, type: 'i' });
                                e.stopPropagation();
                            }
                        });
                        $(el).data('enterinit', '1').attr('data-enterinit', '1');
                    }
                }
            }
        },
        check: function (el) {
            var data = false;
            eval('data=app.edit_' + $(el).data('type') + '.check(el);');
            return data;
        },
        add: function (el) {

            var cur = $(el).parents('tr');
            cur.after(cur.siblings('[hide="1"]').clone(true).removeAttr('hide').attr('style', ''));
            $.DHB.table.reset(cur);
        },
        del: function (el) {
            var table = $(el).parents('table');
            if (table.find('tbody>tr').length < 3) {
                $.DHB.message({ content: '至少保留一项', time: '1000', type: 'i' });
            } else {
                $(el).parents('tr').remove();
                $.DHB.table.reset(table.find('tbody>tr:eq(0)'));
                eval('app.edit_' + table.data('type') + '.table_result(table);');
            }
        },
        reset: function (el) {

            var i = 0;
            $(el).parents('table').find('tbody tr').each(function () {
                $(this).find('td[field="index"]').text(i + 1);
                $(this).attr('data-index', i).data('index', i);
                i++;
            });
        },
        goods: function (el) {
            var searchGoods = function () {
                var searchObj = $(el).find('.dropdown-menu .list-group');
                $.fn.menuTab.loadMessage(searchObj);
                var goodsInputData = {};
                goodsInputData.value = $.trim($(el).find('input').val());

                if ($(el).parents('table').data('client')) {
                    goodsInputData.client_id = $(_ + "#" + $(el).parents('table').data('client')).val();
                }
                if ($(el).parents('table').data('stock')) {
                    goodsInputData.stock_id = $(_ + "#" + $(el).parents('table').data('stock')).val();
                    goodsInputData.stock_name = $(_ + "#" + $(el).parents('table').data('stock')).find('option:selected').text();
                }
                if ($(el).parents('table').data('type') == 'returns') {
                    var inputAction = 'returnsGoodsInput';
                } else {
                    var inputAction = 'goodsInput';
                }
                goodsInputData.for_type = $(el).parents('table').data('type');


                //$.get($.DHB.U('/shopsp/search?_search_dropdown_=1&keyword=' + goodsInputData.value), goodsInputData, function (data, textStatus) {
                //    $(el).find('.input-group').addClass('open');
                //    searchObj.html(data);
                //    searchSize = searchObj.find('a').size();
                //});
            }
            var selectResult = function (intIndex) {
                var searchObj = $(el).find('.dropdown-menu .list-group');
                searchObj.find('a').removeClass('active').find('div>div').css('color', '#999');
                searchObj.find('a').eq(intIndex).addClass('active').find('div>div').css('color', '#fff');
                currentIndex = intIndex;
                var scrollToObj = searchObj.find('a').eq(intIndex);
                searchObj.animate({ scrollTop: scrollToObj.offset().top - searchObj.offset().top + searchObj.scrollTop() }, 100);
                selectItem = true;
            }
            if ($(window).height() - 294 - $(el).offset().top + $(document).scrollTop() < 0) { $(el).find('.input-group').addClass('dropup'); } else { $(el).find('.input-group').removeClass('dropup'); }
            $(el).parents('table').find('td[field="name"]').each(function (e) {
                if ($(this).parent().data('item') == '') {
                    if ($(this).data('init') == '1') {
                        $(this).find('.goods1').show(); $(this).find('.goods2').hide();
                        $(this).find('.goods3').hide();
                    }
                } else {
                    $(this).find('.goods1').hide();
                    $(this).find('.goods2').show();
                    $(this).find('.goods3').hide();
                }
            }); if ($(el).data('init') == '0') {
                var nextTr = $(el).parents('tr').next().next(); if (nextTr.length < 1) { $(el).parents('tr').find('[field="operate"] a:eq(0)').click(); }
                $(el).find('.goods1').hide();
                $(el).find('.goods2').hide();
                $(el).find('.goods3').show();
                var searchSize = 0;
                var currentIndex = 0;
                var selectItem = false;
                if ($(el).find('input').data('initenter') == '0') {
                    $(el).find('input').unbind('keydown').bind('keydown', 'return', function (e) {
                        if ($(el).find('input').is(':focus')) { if ($(el).find('.dropdown-menu .list-group a.active').length <= 0) { searchGoods(); } else { var searchObj = $(el).find('.dropdown-menu .list-group'); $(el).find('.input-group').removeClass('open'); $(el).find('.dropdown-menu .list-group a.active').removeClass('active'); } }
                        $(el).find('input').data('initenter', '1').attr('data-initenter', '1'); return false;
                    });
                }
                if ($(el).find('input').data('initkeydown') == '0') {
                    $(el).find('input').unbind('keydown').bind('keydown', 'up', function (e) {
                        if ($(el).find('input').is(':focus')) {
                            if (currentIndex == -1 || currentIndex == 0) { selectResult(searchSize - 1); } else if (currentIndex > 0) {
                                selectResult(currentIndex - 1);
                            }
                        }
                        return false;
                    }); $(el).find('input').unbind('keydown').bind('keydown', 'down', function (e) {
                        if ($(el).find('input').is(':focus')) {
                            if (currentIndex == -1 || currentIndex == (searchSize - 1)) { selectResult(0); } else if (currentIndex < (searchSize - 1)) {
                                selectResult(currentIndex + 1);
                            }
                        }
                        return false;
                    }); $(el).find('input').data('keydown', '1').attr('data-keydown', '1');
                }
                searchGoods();
                $(el).find('.goods3 input').focus().select();
                var searchObj = $(el).find('.dropdown-menu .list-group');
                searchObj.on('click', 'a', function (e) {
                    if (!$(this).hasClass('moreQ')) {
                        //;
                        $(this).find('.input-group').removeClass('open');
                        $.DHB.table.item($(this));
                        $(el).find('.goods1').hide();
                        $(el).find('.goods2').show();
                        $(el).find('.goods3').hide();
                    }
                    e.stopPropagation();
                });
                searchObj.on('click', '.moreQ', function (e) {
                    $(this).parents('.input-group').removeClass('open');
                    $.DHB.table.select_shopsp(this);
                    e.stopPropagation();
                });
                $(el).data('init', '1').attr('data-init', '1');
            } else {
                $(el).find('.goods1').hide();
                $(el).find('.goods2').hide();
                $(el).find('.goods3').show();
                $(el).find('.goods3 input').focus().select();
            }
        },
        goods_orders: function (el) {
            //;
            var searchGoods = function () {
                var searchObj = $(el).parent().find('.dropdown-menu .list-group'); $.fn.menuTab.loadMessage(searchObj);
                $.get($.DHB.U('/shopsp/search'), {
                    value: $(el).val()
                }, function (data, textStatus) {
                    data = $.DHB.isAjaxJson(data, true);
                    if (typeof (data) == 'object') {
                        $(el).parent().addClass('open');
                        searchObj.html('<p class="media list-group-item">' + data.one.goods_name + '</p>');
                        $(_ + '#' + $(el).data('id')).val(data.one.goods_id);
                        $(el).val(data.one.goods_name);
                    } else {
                        searchObj.html(data);
                        searchSize = searchObj.find('a').size();
                    }
                });
            }
            var selectResult = function (intIndex) { var searchObj = $(el).parent().find('.dropdown-menu .list-group'); searchObj.find('a').removeClass('active').find('div>div').css('color', '#999'); searchObj.find('a').eq(intIndex).addClass('active').find('div>div').css('color', '#fff'); currentIndex = intIndex; var scrollToObj = searchObj.find('a').eq(intIndex); searchObj.animate({ scrollTop: scrollToObj.offset().top - searchObj.offset().top + searchObj.scrollTop() }, 100); selectItem = true; }
            var select_callback = function (goods_id) {
                var selectCallback = $(el).data('callback'); if (selectCallback) {
                    eval(selectCallback + '(\'' + goods_id + '\');');
                }
            }
            if ($(window).height() - 250 - $(el).offset().top + $(document).scrollTop() < 0) { $(el).parents('.input-group').addClass('dropup'); } else { $(el).parents('.input-group').removeClass('dropup'); }
            if ($(el).data('init') == '0') {
                searchGoods();
                var searchObj = $(el).parent().find('.dropdown-menu .list-group');
                searchObj.on('click', 'a', function (e) {
                    $(_ + '#' + $(el).data('id')).val($(this).attr('dataId'));
                    $(el).val($(this).attr('dataName')); $(el).parent().removeClass('open'); select_callback($(this).attr('dataId'));
                });
                searchObj.on('click', '.moreQ', function (e) {
                    $(el).parents('.input-group').removeClass('open');
                    $.DHB.table.select_goods('', { 'title': $(el).data('dialog_title'), 'goods_name': $(el).data('name'), 'goods_id': $(el).data('id'), 'goods_callback': $(el).data('callback'), 'goods_type': 'orders', 'type': '1' });
                }); $(el).data('init', '1');
            }
            var searchSize = 0; var currentIndex = 0; var selectItem = false; if ($(el).data('initenter') == '0') {
                $(el).parent().unbind('keydown').bind('keydown', 'return', function (e) {
                    if ($(el).is(':focus')) { if ($(el).parent().find('.dropdown-menu .list-group a.active').length <= 0) { searchGoods(); } else { var searchObj = $(el).parent().find('.dropdown-menu .list-group'); curSel = searchObj.find('a').eq(currentIndex); $(_ + '#' + $(el).data('id')).val(curSel.attr('dataId')); $(el).val(curSel.attr('dataName')); $(el).parent().removeClass('open'); searchObj.find('a').removeClass('active').find('div>div').css('color', '#999'); select_callback(curSel.attr('dataId')); } }
                    $(el).data('initenter', '1'); return false;
                });
            }
            if ($(el).data('initkeydown') == '0') {
                $(el).parent().unbind('keydown').bind('keydown', 'up', function (e) {
                    if ($(el).is(':focus')) {
                        if (currentIndex == -1 || currentIndex == 0) { selectResult(searchSize - 1); } else if (currentIndex > 0) {
                            selectResult(currentIndex - 1);
                        }
                    }
                    return false;
                }); $(el).parent().unbind('keydown').bind('keydown', 'down', function (e) {
                    if ($(el).is(':focus')) {
                        if (currentIndex == -1 || currentIndex == (searchSize - 1)) { selectResult(0); } else if (currentIndex < (searchSize - 1)) {
                            selectResult(currentIndex + 1);
                        }
                    }
                    return false;
                }); $(el).data('keydown', '1');
            }
        },
        select_goods: function (el, arrData) {

            var defaultData = { 'title': '选择商品', 'index': '0', 'client_id': '0', 'stock_id': '0', 'type': '0', 'goods_type': '', 'goods_name': 'edit_goods_name', 'goods_id': 'edit_goods_id', 'goods_callback': '', };
            var goodsData = {};
            if (typeof (arrData) == 'undefined') {
                arrData = {}; arrData.type = '0';
            }

            if (arrData.type != '1') {
                var table = $(el).parents('table'); if (typeof (table.data('client')) != 'undefined') {
                    defaultData['client_id'] = $(_ + '#' + table.data('client')).val();
                }
                if (typeof (table.data('stock')) != 'undefined') {
                    defaultData['stock_id'] = $(_ + '#' + table.data('stock')).val();
                }
            }
            arrData = $.extend({}, defaultData, arrData); var dialogData = { pageSize: publicSettings.minPageSize }; if (arrData.type != '1') {
                if (arrData.client_id) {
                    dialogData.client_id = arrData.client_id;
                }
                if (arrData.stock_id) {
                    dialogData.stock_id = arrData.stock_id;
                }
                dialogData.for_type = $(el).parents('table').data('type');
                if ($(el).parents('table').data('type') == 'returns') {
                    var inputAction = 'returnsGoodsDialog';
                } else {
                    var inputAction = 'goodsDialog';
                }
            } else {
                if (arrData.goods_type == 'orders') {
                    var inputAction = 'ordersGoodsDialog';
                }
                else {
                    var inputAction = 'ordersGoodsDialog';
                }
                dialogData.edit_goods_name = arrData.goods_name; dialogData.edit_goods_id = arrData.goods_id; dialogData.goods_callback = arrData.goods_callback;
            }
            $.DHB.dialog({
                'title': arrData.title, 'url': $.DHB.U('Quote/Public/' + inputAction + '?index=' + arrData.index, dialogData), 'id': 'goods_dialog'
            });
        },
        clear_select_goods: function (el) {
            var select_callback = function () {
                var selectCallback = $(el).data('callback'); if (selectCallback) {
                    eval(selectCallback + '();');
                }
            }
            $(_ + '#' + $(el).data('id')).val(''); $(_ + '#' + $(el).data('name')).val(''); select_callback();
        },
        shopsp_dropmenu: function (el) {
            if ($(el).parent().attr('class').indexOf('open') >= 0) {
                $(el).parent().removeClass('open');
            } else { $(el).parent().addClass('open'); }
        },
        list: function (data) {
            var strResult = ''; if (typeof (data.item) == 'object') {
                strResult = '<div class="btn-group ' + data.name + '" style="width:100%;height:60px;padding:0;border:none;">' + '<button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="width:100%;height:60px;border:none;text-align:left;position: relative"><div class="cut-out" style="padding-right: 10px">' +
                data.item[data.value]['0'] + '</div> <div class="caret" style="position: absolute;top: 28px;right:12px"></div>' + '</button>' + '<ul class="dropdown-menu" role="menu" style="width:200px;box-shadow:none;overflow-y:auto;max-height:180px;">'; for (var it in data.item) {
                    strResult += '<li value="' + it + '" class="' + (data.value == it ? 'active' : '') + '" ' + (typeof (data.item[it][1]) != 'undefined' ? 'data-extend1="' + data.item[it][1] + '"' : '') + ' ' + (typeof (data.item[it][2]) != 'undefined' ? 'data-extend2=\'' + JSON.stringify(data.item[it][2]) + '\'' : '') + '><a style="padding-top:6px;padding-bottom:6px;" class="cut-out" title=' + data.item[it][0] + ' >' + data.item[it][0] + '</a></li>';
                }
                strResult += '</ul>' + '</div>';
            } else {
                strResult = '<div class="list-single">' + data.item + '</div>';
            }
            return strResult;
        },
        item: function (el, isDialog, index) {
            //;
            isDialog = isDialog || false;
            if (isDialog === true) {
                var table = $(_ + ".dhb-table table"); index = parseInt(index);
            } else {
                var table = el.parents('table');
                var index = parseInt(el.parents('tr').data('index'));
            }
            var strID = String(el.data('id'));
            var objTr = table.find('tr[hide!="1"][data-index="' + index + '"]');
            var objTrItem = table.find('tr[hide!="1"][data-item="' + strID + '"]');
            var findTrue = false; if (objTr.length > 0 && objTr.data('item') == strID) {
                findTrue = true;
            } else if (objTrItem.length > 0 && objTrItem.data('index') != index) {
                findTrue = true; objTr = objTrItem;
            }
            if (!findTrue) {
                if (isDialog === true || objTr.length < 1) {
                    objTr = table.find('tr[hide!="1"][data-item=""]').eq(0); if (objTr.length < 1) {
                        if (isDialog === true) {
                            var curTr = table.find('tr:eq(' + (table.find('tbody>tr').length - 1) + ')');
                        } else {
                            var curTr = el.parents('tr');
                        }
                        curTr.find('[field="operate"] a:eq(0)').click();
                        objTr = table.find('tr[hide!="1"][data-item=""]').eq(0);
                    }
                }
                if (isDialog === true) {
                    objTr.find('[field="name"] .goods4').html(el.clone());
                    el = objTr.find('[field="name"] .goods4').find('input:checkbox');
                }
                objTr.data('item', strID).attr('data-item', strID);
            } else {
                setTimeout(function () {
                    var par = el.parents('td');
                    par.parent().attr('data-init', '1').data('init', '0');
                    par.find('.goods1').show();
                    par.find('.goods2').hide();
                    par.find('.goods3').hide();
                }, 100);
            }
            var objName = objTr.find('td[field="name"]');
            objName.attr('title', el.data('goodsname'));
            objName.find('.goods-num').text(el.data('goodsnum'));
            objName.find('.goods-name').text(el.data('goodsname')).attr('title', el.data('goodsname')).niceTitle({
                showLink: false
            });
            objName.find('input[name="goods-post[]"]').val(el.data('goodsname')).data('conversion', el.data('conversionnumber')).attr('data-conversion', el.data('conversionnumber'));
            objTr.find('td[field="name"] .goods-img img').attr('src', el.data('goodspicture'));
            eval('app.edit_' + table.data('type') + '.goods(el,table,objTr,findTrue,index,strID);');
        },

        remark: function (el) {
            if ($(el).parents('tr').data('item') == '') {
                $.DHB.message({ content: '请先选择列数据！', time: 1000, type: 'i' });
            } else {
                var strHtml = '<textarea class="form-control p-t-xs" cols="50" rows="5" name="table_remark_value" placeholder="添加备注">' + $.trim($(el).parents('td').find('p').text()) + '</textarea><div style="clear:both"></div>';
                $.messager.confirm("添加备注", strHtml, function () {
                    var strRemark = $.trim($("textarea[name='table_remark_value']").val());
                    $(el).parents('td').attr('title', strRemark).find('p').text(strRemark); $(el).parents('td').find('div');
                    var table = $(el).parents('table'); eval('app.edit_' + table.data('type') + '.table_result(table);');
                });
            }
        },

        select_shopsp: function (arrData) {
            var defaultData = { 'title': '选择商品', 'shopsp_name': 'shopsp_name', 'shopsp_id': 'shopsp_id', 'shopsp_callback': '', 'shopsp_type': '', 'dialog_id': 'dialog-shopsp-search' };
            arrData = $.extend({}, defaultData, arrData);

            //;

            $.DHB.dialog({
                'title': arrData.title,
                'url': $.DHB.U('shopsp/search', { pageSize: publicSettings.minPageSize, gys_name: arrData.gys_name, gys_id: arrData.gys_id, gys_callback: arrData.gys_callback, gys_type: arrData.gys_type }),
                'id': arrData.dialog_id
            });
        },

        clear_select_shopsp: function (el) {
            var select_callback = function () {
                var selectCallback = $(el).data('callback');
                if (selectCallback) {
                    eval(selectCallback + '();');
                }
            }
            $(_ + '#' + $(el).data('id')).val('');
            $(_ + '#' + $(el).data('name')).val('');
            select_callback();
        },

        shopsp: function (el) {
            var searchGys = function () {
                var searchObj = $(el).parent().find('.dropdown-menu .list-group');
                $.fn.menuTab.loadMessage(searchObj);
                $.get($.DHB.U('gys/search?_search_dropdown_=1'), {
                    value: $(el).val(), client_type: $(el).data('gys_type')
                }, function (data, textStatus) {
                    data = $.DHB.isAjaxJson(data, true);
                    if (typeof (data) == 'object') {
                        $(el).parent().addClass('open');
                        searchObj.html('<p class="media list-group-item">' + data.one.gys_name + '</p>');
                        $(_ + '#' + $(el).data('id')).val(data.one.client_id);
                        $(el).val(data.one.gys_name);
                    } else {
                        searchObj.html(data);
                        searchSize = searchObj.find('a').size();
                    }
                });
            }
            var selectResult = function (intIndex) {
                var searchObj = $(el).parent().find('.dropdown-menu .list-group');
                searchObj.find('a').removeClass('active').find('div>div').css('color', '#999');
                searchObj.find('a').eq(intIndex).addClass('active').find('div>div').css('color', '#fff');
                currentIndex = intIndex;
                var scrollToObj = searchObj.find('a').eq(intIndex);
                searchObj.animate({ scrollTop: scrollToObj.offset().top - searchObj.offset().top + searchObj.scrollTop() }, 100); selectItem = true;
            }
            var select_callback = function (client_id) {
                var selectCallback = $(el).data('callback');
                if (selectCallback) {
                    eval(selectCallback + '(\'' + client_id + '\');');
                }
            }
            if ($(window).height() - 250 - $(el).offset().top + $(document).scrollTop() < 0) { $(el).parents('.input-group').addClass('dropup'); } else { $(el).parents('.input-group').removeClass('dropup'); }
            if ($(el).data('init') == '0') {
                searchGys();
                var searchObj = $(el).parent().find('.dropdown-menu .list-group');
                searchObj.on('click', 'a', function (e) {
                    $(_ + '#' + $(el).data('id')).val($(this).attr('dataId'));
                    $(el).val($(this).attr('dataName'));
                    $(el).parent().removeClass('open');
                    select_callback($(this).attr('dataId') + '|' + $(this).attr('dataName'));
                });
                searchObj.on('click', '.moreQ', function (e) {
                    $(el).parents('.input-group').removeClass('open');
                    $.DHB.client.select_gys({ 'title': $(el).data('dialog_title'), 'gys_name': $(el).data('name'), 'gys_id': $(el).data('id'), 'gys_callback': $(el).data('callback'), 'gys_type': $(el).data('gys_type') });
                }); $(el).data('init', '1');
            }
            var searchSize = 0; var currentIndex = 0; var selectItem = false; if ($(el).data('initenter') == '0') {
                $(el).parent().unbind('keydown').bind('keydown', 'return', function (e) {
                    if ($(el).is(':focus')) {
                        if ($(el).parent().find('.dropdown-menu .list-group a.active').length <= 0) {
                            searchGys();
                        } else {
                            var searchObj = $(el).parent().find('.dropdown-menu .list-group');
                            curSel = searchObj.find('a').eq(currentIndex); $(_ + '#' + $(el).data('id')).val(curSel.attr('dataId'));
                            $(el).val(curSel.attr('dataName'));
                            $(el).parent().removeClass('open');
                            searchObj.find('a').removeClass('active').find('div>div').css('color', '#999');
                            select_callback(curSel.attr('dataId') + '|' + curSel.attr('dataName'));
                        }
                    }
                    $(el).data('initenter', '1'); return false;
                });
            }
            if ($(el).data('initkeydown') == '0') {
                $(el).parent().unbind('keydown').bind('keydown', 'up', function (e) {
                    if ($(el).is(':focus')) {
                        if (currentIndex == -1 || currentIndex == 0) { selectResult(searchSize - 1); } else if (currentIndex > 0) {
                            selectResult(currentIndex - 1);
                        }
                    }
                    return false;
                }); $(el).parent().unbind('keydown').bind('keydown', 'down', function (e) {
                    if ($(el).is(':focus')) {
                        if (currentIndex == -1 || currentIndex == (searchSize - 1)) { selectResult(0); } else if (currentIndex < (searchSize - 1)) {
                            selectResult(currentIndex + 1);
                        }
                    }
                    return false;
                }); $(el).data('keydown', '1');
            }
        }


    },
    func: {
        //TODO：分页
        pagination: function (setting) {
            
            var curContent = $(_);
            var options = $.extend({
                //count: '0',
                //page_size: '0',
                //current_page: '0',
                count: 0,
                page_size: 10,
                current_page: 0,
                ready_once: false,
                controller_name: curContent.attr('controller'),
                action_name: curContent.attr('action'),
                module_name: publicSettings.moduleName,
                filter_form: ".filter-form",
                page_box: "#Pagination",
                dialog_pagesize: false,
                page_extendurl: ''
            },
            setting || {});
            var node = options.controller_name + '/' + options.action_name;
            options.count = parseInt(app.c.public_data[node]['_row_total_']);
            options.page_size = parseInt(app.c.public_data[node]['_page_size_']);
            options.current_page = parseInt(app.c.public_data[node]['_current_page_']);

            $.DHB.pagination({
                link_to: $.DHB.U(options.module_name + '/' + options.controller_name + '/' + options.action_name + (options.page_extendurl ? '?' + options.page_extendurl : '')),
                row_total: options.count,
                page_size: options.page_size,
                current_page: options.current_page,
                filter_form: options.filter_form,
                page_box: options.page_box,
                refresh_box: '#' + options.controller_name + '-' + options.action_name + '-list-fresh-box',
                ready_once: options.ready_once,
                dialog_pagesize: options.dialog_pagesize,
                callback_: function (data, textStatus, idx, jq) {
                    
                    //TODO：分页调用回调
                    var curListcallback = ('app.' + options.controller_name + '.' + options.action_name + '_listready').toLowerCase();
                    var curPaginationcallback = ('app.' + options.controller_name + '.' + options.action_name + '_pagination_callback').toLowerCase();
                    eval('try {if(' + curListcallback + ' && typeof(' + curListcallback + ')=="function"){' + curListcallback + '(); }}catch(e){}');
                    eval('try {if(' + curPaginationcallback + ' && typeof(' + curPaginationcallback + ')=="function"){' + curPaginationcallback + '(data, textStatus,idx,jq); }}catch(e){}');
                }
            });
        },
        selectAll: function (setting) {
            var options = $.extend({
                select: '.selectAll-table',
                callback: function (intNum) { }
            }, setting || {}); $(_ + options.select + " th input:checkbox").click(function () { $.DHB.func.selectAllThis(this, options); });
        },
        selectAllThis: function (el, setting) {
            var options = $.extend({
                select: '.selectAll-table', footerSelect: '.footer-selectAllThis',
                callback: function (intNum) { }
            }, setting || {}); var table = $(el).parents(options.select); var checked = $(el).get(0).checked; var disabled = $(el).get(0).disabled; table.find("td input:checkbox").each(function () {
                if (this.disabled == disabled)
                    this.checked = checked ? 'checked' : false;
            }); if ($(_ + options.footerSelect).length > 0 && $(_ + options.footerSelect).get(0).disabled == disabled)
                $(_ + options.footerSelect).prop('checked', checked); options.callback($.DHB.func.getSelectedAttr(options.select).length);
        },
        selectSingle: function (el, setting) {
            var options = $.extend({
                select: '.selectAll-table', headerAndFooterSelect: '#check-btn-header, #check-btn-footer',
                callback: function (intNum) { }
            }, setting || {}); var isChecked = el.checked
            var table; if (!isChecked) { $(options.headerAndFooterSelect).prop('checked', false); } else { var isAllCheck = true; table = $(el).parents(options.select); table.find("td input:checkbox").each(function () { if (!this.checked) { isAllCheck = false; return false; } }); if (isAllCheck) { $(options.headerAndFooterSelect).prop('checked', true); } }
        },
        defaultAjax: function () { $("[data-ajax='default']", _).on('click', function () { $.DHB.func.ajaxThis(this); return false; }); },
        ajaxThis: function (el) {
            var confirm = $(el).data("confirm"); var $this = $(el); if (confirm) { $.messager.confirm("系统提示", confirm, function () { defaultAjax(); }); } else { defaultAjax(); }
            function defaultAjax() {
                $.post($this.attr('href'), function (json) {
                    $.DHB.message({
                        content: json.message,
                        'type': (json.status == 'success' ? 's' : 'e')
                    });
                    if (json.status == 'success') {
                        $.DHB.refresh();
                    }
                }, 'json');
            }
            return false;
        },
        defaultDialog: function (setting) { var options = $.extend({ title: 'dialog-title', url: 'dialog-url', id: 'dialog-id' }, setting || {}); $("[data-dialog-url]", _).on('click', function () { $.DHB.func.dialogThis(this, options); }); },
        dialogThis: function (el, setting) { var options = $.extend({ title: 'dialog-title', url: 'dialog-url', id: 'dialog-id' }, setting || {}); $.DHB.dialog({ title: $(el).data(options.title), url: $(el).data(options.url), id: $(el).data(options.id) }); },
        getSelectedAttr: function (selector, type) { var vals = []; type = type || 'value'; $(selector + " td input:checkbox:checked", _).each(function () { vals.push($(this).attr(type)); }); return vals; },
        inArray: function (val, arr, isStrict) {
            var result = -1; isStrict = isStrict || 'F'; if (arr) {
                for (var i = 0; i < arr.length; i++) {
                    if (isStrict == 'T' && val === arr[i]) {
                        result = i; break;
                    }
                    if (isStrict == 'F' && val == arr[i]) {
                        result = i; break;
                    }
                }
            }
            return result;
        },
        arrayFilter: function (arr) {
            var tmp = []; for (var i = 0, l = arr.length; i < l; i++) {
                if (arr[i]) { tmp.push(arr[i]); }
            }
            return tmp;
        },
        //TODO：拖拽树
        nestable: function (setting) {
            var options = $.extend({
                node: '.dd',
                node_handle: '.dd-handle',
                result_input: '#nestable-result',
                submit_button: '#nestable-submit-button',
                menu: '#nestable-menu',
                save_url: '',
                save_on: false,
                level: 5
            }, setting || {});
            var node = _ + options.node;
            setTimeout(function () {
                var node_handle = _ + options.node_handle;
                var result = _ + options.result_input;
                var submit_button = _ + options.submit_button;
                $(node).nestable({ maxDepth: options.level });
                $(node_handle + ' a').on('mousedown', function (e) { e.stopPropagation(); });
                $(node).nestable().on('change', function () {
                    $(submit_button).removeAttr('disabled');
                    $(result).val(JSON.stringify($(node).nestable('serialize')));
                });
                $(node).nestable('collapseAll');
                $(result).val(JSON.stringify($(node).nestable('serialize')));
                if ($(_ + options.menu).length > 0) {
                    $(_ + options.menu).on('click', function (e) {
                        var target = $(e.target);
                        var action = target.data('action');
                        if (action === 'expand-all') {
                            $(node).nestable('expandAll');
                            target.data('action', 'collapse-all').find('i').attr('class', 'fa fa-angle-double-up');
                        }
                        else if (action === 'collapse-all') {
                            $(node).nestable('collapseAll');
                            target.data('action', 'expand-all').find('i').attr('class', 'fa fa-angle-double-down');
                        }
                    });
                }
                if (options.save_on === true) {
                    $(submit_button).on('click', function (e) {
                        if (!$(result).val()) {
                            $.DHB.message({ 'content': '没有可提交的数据！', 'type': 'i' });
                        }
                        else {
                            var btn = $(this).button('loading');
                            $.post($.DHB.U(options.save_url),
                                {
                                    'order_data': $(result).val()
                                },
                                function (data) {
                                    if (data.status == "success") {
                                        $.DHB.message({ 'content': data.message, 'time': 1000, 'type': 's' });
                                        $(submit_button).removeClass('disabled').text('确定');
                                    }
                                    else {
                                        $.DHB.message({ 'content': data.message, 'time': 0, 'type': 'e' });
                                        btn.button('reset');
                                    }
                                }, 'JSON');
                        }
                    });
                }
            }, 50);
            return $(node);
        },
        panelClose: function (el) { $(el).parents('.dropdown-menu').parent().removeClass('open'); },
        listOperate: function (el) {
            if ($(el).attr('data-operateinit') !== '1') {
                $(el).attr('data-operateinit', '1').data('operateinit', '1');
                //$(el).find("tbody td.list-operate").each(function () {
                //    var tr = $(this).parents('tr');
                //    var ts = $(this);
                //    tr.find('td:gt(' + (tr.find('td').size() - 4) + ')').hover(function () {
                //        //进入函数
                //        tr.siblings().find('.list-operate').each(function () {
                //            $(this).find('p').css("visibility", "visible");
                //            $(this).find('div:first()').hide();
                //        });
                //        ts.children("div").css("display", "block");
                //        ts.children("p").css("visibility", "hidden");
                //    }, function () {
                //        //离开函数
                //        ts.children("div").css("display", "none");
                //        ts.children("p").css("visibility", "visible");
                //    });
                //});
            }
        },
        tab: function (el, index) {
            $(el).siblings('.selected-tab').removeClass('selected-tab'); $(el).addClass('selected-tab'); $(el).parents(".main-head").find(".copy").hide()
            $(el).parents(".main-head").find(".copy").eq(index).show(); var tabID = $(el).parent().attr('tab'); $(_ + 'div[tabcontent="' + tabID + '"] > div').hide(); $(_ + 'div[tabcontent="' + tabID + '"] > div').eq($(el).index()).show();
        },
        tab_: function (strTab, intEq) { var curTab = $(_ + 'ul[tab="' + strTab + '"] li:eq(' + intEq + ')'); if (curTab.length > 0) { curTab.click(); } },
        nestableHover: function (el, act) { if (act == 'leave') { $(el).css('background', '#fff'); } else { $(el).css('background', '#fff6ed'); } },
        editor: function (setting) {
            setting = $.extend({ type: 'full', textarea: '', width: '' }, setting || {}); var item = []; if (setting.type == 'simple') { item = ['fontname', 'fontsize', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline', 'removeformat', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist', 'insertunorderedlist', 'emoticons', 'dhbupload', 'link']; } else { item = ['formatblock', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline', 'strikethrough', 'lineheight', 'dhbupload', 'link', 'unlink', 'plainpaste', 'wordpaste', 'insertorderedlist', 'insertunorderedlist', 'table', 'fullscreen', 'dhbshowmore', '/', 'justifyleft', 'justifycenter', 'justifyright', 'justifyfull', 'subscript', 'superscript', 'clearhtml', 'removeformat', 'flash', 'preview', 'source']; }
            setTimeout(function () {
                var editor = KindEditor.create(_ + setting.textarea, { resizeType: 1, themeType: 'simple', minHeight: 460, minWidth: setting.type == 'full' ? 960 : 500, items: item, allowFileUpload: false, allowFlashUpload: false, allowImageUpload: false, allowMediaUpload: false, afterBlur: function () { this.sync(); } }); if (typeof (editor) != 'undefined' && typeof (editor.container) != 'undefined') { $(editor.container[0]).find('.ke-toolbar .ke-outline').tooltip({ 'trigger': 'hover', 'placement': 'bottom' }); if (setting.width != '') { $(editor.container[0]).find('.ke-edit').css('width', setting.width); } }
                return editor;
            }, 50);
        },
        checkInt: function (el) {
            var val = $.trim($(el).val()); if (val == '') {
                if (typeof ($(el).attr('data-default')) != 'undefined') { $(el).val($(el).data('default')); }
                return true;
            } else {
                val = parseInt(val); if (isNaN(val)) {
                    if (typeof ($(el).attr('data-default')) != 'undefined') { $(el).val($(el).data('default')); } else { $(el).val(''); }
                    return false;
                } else {
                    $(el).val(val); return true;
                }
            }
        },
        checkFloat: function (el) {
            var val = $.trim($(el).val()); if (val == '') {
                if (typeof ($(el).attr('data-default')) != 'undefined') { $(el).val($(el).data('default')); }
                return true;
            } else {
                val = parseFloat(val); if (isNaN(val)) {
                    if (typeof ($(el).attr('data-default')) != 'undefined') { $(el).val($(el).data('default')); } else { $(el).val(''); }
                    return false;
                } else {
                    $(el).val(val); return true;
                }
            }
        },
        edit: function (el) {
            var oldValue = $.trim($(el).text()); if (!$(el).hasClass('editChange')) { $(el).removeClass("p-r"); var strHtml = '<input type="text" class="form-control ' + (typeof ($(el).data('class')) != 'undefined' ? $(el).data('class') : '') + '" value="' + $.trim($(el).text()) + '" style="width:' + ($(el).data('width') ? $(el).data('width') : '100%') + ';min-width:0;' + $(el).data('style') + '" />'; $(el).addClass('editChange').html(strHtml).find('input').focus().select().blur(function (e) { var now = $.trim($(this).val()); if (oldValue != now) { $.get($(el).data('url'), { value: now }, function (result) { if (typeof (result.status) != "undefined") { if (result.status == 'success') { $(el).removeClass('editChange'); $(el).html(result.data); $(el).addClass("p-r"); } else { $(el).html(oldValue); $(el).addClass("p-r"); $.DHB.message({ 'content': result.message, 'time': 0, 'type': 'e' }); } } else { $(el).html(oldValue); $(el).addClass("p-r"); $.DHB.message({ 'content': '返回数据格式错误', 'time': 0, 'type': 'e' }); } }, 'json'); } else { $(el).removeClass('editChange'); $(el).html(oldValue); $(el).addClass("p-r"); } }); }
        }
    }
};




(function ($) {
    _self = $(this);
    var defaultSettings = { 'url': '', 'title': '', 'id': '', 'nocache': '0', 'firstinit': '0' };
    //加载菜单
    $.fn.menuTab = function (settings) {
        //;
        $(this).click(function () {
            var objSettings = objSettings || {};
            settings = settings || {};
            //;
            if (typeof ($(this).attr("tabUrl")) != "undefined") {
                objSettings.url = $(this).attr("tabUrl");
            }
            if (typeof ($(this).attr("tabTitle")) != "undefined") {
                objSettings.title = $(this).attr("tabTitle");
            }
            else if (typeof ($(this).attr("title")) != "undefined") {
                objSettings.title = $(this).attr("title");
            }
            if (typeof ($(this).attr("tabNocache")) != "undefined") {
                objSettings.nocache = $(this).attr("tabNocache");
            }
            if (typeof ($(this).attr("tabId")) != "undefined") {
                objSettings.id = $(this).attr("tabId");
            }
            var settings = $.extend({}, defaultSettings, objSettings, settings);
            loadMenu(settings);
        });
        return this;
    };
    //TODO：内页加载入口
    $.fn.menuTab.load = function (settings) {
        
        settings = settings || {};
        var settings = $.extend({}, defaultSettings, settings);
        loadMenu(settings);
    }
    $.fn.menuTab.load_ = function (settings) {
        
        settings = settings || {}; var settings = $.extend({}, defaultSettings, settings);
        loadMenu(settings, 1);
    }
    $.fn.menuTab.bindMenuAdd = function () {
        bindMenuAdd();
    };
    $.fn.menuTab.bindMenuMore = function () {
        bindMenuMore();
    };
    $.fn.menuTab.loadMessage = function (id) {
        if (typeof (id) == 'object') { loadMessage(id); } else {
            loadMessage($('.wrap-content-body [contentid="' + id + '"]'));
        }
    };
    $.fn.menuTab.current = function (nID) {
        return currentTab(nID);
    };
    var currentTab = function (nID) {
        var currentObj = $(".wrap-content-body .wrap-tab-contentbox[contentID='" + $("div.wrap-header-tabbox ul.first-level li.active").attr('menuID') + "']")
        if (nID == 1) {
            return '[contentid="' + currentObj.attr('contentid') + '"]' + ' ';
        }
        else {
            return currentObj;
        }
    };
    //TODO：内页加载
    var loadMenu = function (settings, nCurrentObj) {
        
        var currentActive = $("div.wrap-header-tabbox ul.first-level li.active");
        if (nCurrentObj == 1) {
            if (app.c.is_ctrl === true) {
                var newUrl = publicSettings.rootUrl + publicSettings.moduleName + '#' + $.DHB.thinkUrl(settings.url);
                window.open(newUrl);
                app.c.is_ctrl = false;
            }
            else {
                if (app.c.is_history === false) {
                    var strHistory = publicSettings.rootUrl + '?url=' + encodeURIComponent(settings.url);
                    if (strHistory != window.location.href) {
                        strHistory = publicSettings.rootUrl + publicSettings.moduleName + '#' + $.DHB.thinkUrl(strHistory);
                        app.c.is_history = false;
                        window.history.pushState('forward', null, strHistory);
                    }
                }
                var currentObj = currentTab();
                $.DHB.showButterbar();
                options = {
                    url: settings.url,
                    data: {
                        //'_m_': '1'
                    },
                    type: "GET",
                    cache: (settings.nocache == '1' ? false : true),
                    dataType: 'html',
                    complete: function (xhr, ts) {
                        $.DHB.closeButterbar();
                        currentObj.html(xhr.responseText);
                        var currentMenuLi = $("div.wrap-header-tabbox ul.first-level li.active");
                        if (currentMenuLi.parent().attr('class') == 'more-tab') {
                            currentMenuLi.parent().show().css({ 'height': 'auto' });
                        } else {
                            $('div.wrap-content-header .wrap-header-tabbox ul.more-tab').css({ 'display': 'none', 'height': 'auto' });
                        }
                        //;
                        if (settings.nocache != '-1') {
                            currentMenuLi.find('a').attr('_url', settings.url);
                        }
                        if (currentMenuLi.attr('menuID') != 'index') {
                            $.DHB.menuCookie({
                                id: currentMenuLi.attr('menuID'),
                                url: (settings.nocache != '-1' ? settings.url : currentMenuLi.find('a').attr('_url'))
                            }, 'upd');
                        }
                        jQuery.DHB.ready(true);
                        
                        $.DHB.contentbind(_);
                    }
                };
                htmlobj = app.httpAjax.post(options);
                //htmlobj = $.ajax({
                //    url: settings.url,
                //    data: {
                //        '_m_': '1'
                //    },
                //    async: false,
                //    cache: (settings.nocache == '1' ? false : true),
                //    dataType: 'html',
                //    complete: function (xhr, ts) {
                //        $.DHB.closeButterbar();
                //        currentObj.html(xhr.responseText);
                //        var currentMenuLi = $("div.wrap-header-tabbox ul.first-level li.active");
                //        if (currentMenuLi.parent().attr('class') == 'more-tab') {
                //            currentMenuLi.parent().show().css({ 'height': 'auto' });
                //        } else {
                //            $('div.wrap-content-header .wrap-header-tabbox ul.more-tab').css({ 'display': 'none', 'height': 'auto' });
                //        }
                //        ;
                //        if (settings.nocache != '-1') {
                //            currentMenuLi.find('a').attr('_url', settings.url);
                //        }
                //        if (currentMenuLi.attr('menuID') != 'index') {
                //            $.DHB.menuCookie({
                //                id: currentMenuLi.attr('menuID'),
                //                url: (settings.nocache != '-1' ? settings.url : currentMenuLi.find('a').attr('_url'))
                //            }, 'upd');
                //        }
                //        jQuery.DHB.ready(true);
                //        $.DHB.contentbind(_);
                //    }
                //});
            }
        }
        else {
            if (!settings.id) {
                $.DHB.message({ 'content': '菜单配置错误，请检查访问 URL 是否正确以及元素上是否同时存在 class=menuTab 和 $.fn.menuTab.load|load_ 方法，解决办法为删除 meanTab它们之中的一个或者请在后台配置 Title 为（' + settings.title + '）、URL 为（' + settings.url + '）的访问菜单！', 'time': 0, 'type': 'e' });
            }
            else {
                if (app.c.is_ctrl === true) {
                    var newUrl = publicSettings.rootUrl + publicSettings.moduleName + '#' + $.DHB.thinkUrl(settings.url);
                    window.open(newUrl);
                    app.c.is_ctrl = false;
                }
                else {
                    sUrlID = settings.id;
                    var oldCurrentObj = currentTab();
                    if (app.c.is_history === false) {
                        //url历史
                        var strHistory = publicSettings.rootUrl + '?url=' + encodeURIComponent(settings.url);
                        if (strHistory != window.location.href) {
                            strHistory = publicSettings.rootUrl + publicSettings.moduleName + '#' + $.DHB.thinkUrl(strHistory);
                            app.c.is_history = false;
                            window.history.pushState('forward', null, strHistory);
                        }
                    }
                    //;
                    if ($('[contentID="' + sUrlID + '"]').length > 0) {
                        //;
                        //若已打开页面
                        if (app.c.init_menu === false) {
                            app.c.cookieMenu__();
                        }
                        var currentContent = $('[contentID="' + sUrlID + '"]');
                        if (currentContent.attr('contentID') != oldCurrentObj.attr('contentID')) {
                            currentContent.show();
                            oldCurrentObj.hide();
                        }
                        var currentMenuLi = $("div.wrap-content-header .wrap-header-tabbox ul.first-level li[menuID='" + sUrlID + "']")
                        var currentMenuA = currentMenuLi.find('a');
                        if (settings.nocache == '1' || currentMenuLi.data('init') == '0' || currentMenuA.attr('_url') != settings.url) {
                            $.DHB.showButterbar();
                            var options = {
                                url: settings.url,
                                data: {
                                    //'_m_': '1'
                                },
                                cache: (settings.nocache == '1' ? false : true),
                                type: "GET",
                                dataType: 'html',
                                complete: function (xhr, ts) {
                                    var regjson = /^{([\s\S]*)}$/;
                                    if (regjson.test(xhr.responseText)) {
                                        AjaxLoginCheck(JSON.parse(xhr.responseText));
                                    } else {
                                        $.DHB.closeButterbar();
                                        currentContent.html(xhr.responseText);
                                        //
                                        _ = '[contentID="' + settings.id + '"] ';
                                        currentMenuA.attr('_url', settings.url);
                                        if (settings.title) {
                                            currentMenuA.attr('_title', settings.title).attr('title', settings.title).html('<span>' + settings.title + '</span>');
                                        }
                                        if (currentMenuLi.attr('menuID') != currentActive.attr('menuID')) {
                                            currentMenuLi.attr("class", "active"); currentActive.removeClass("active");
                                        }
                                        if (currentMenuLi.parent().attr('class') == 'more-tab') { currentMenuLi.parent().show().css({ 'height': 'auto' }); } else {
                                            $('div.wrap-content-header .wrap-header-tabbox ul.more-tab').css({ 'display': 'none', 'height': 'auto' });
                                        }
                                        $.DHB.closeButterbar();
                                        jQuery.DHB.ready(true);
                                        
                                        $.DHB.contentbind(_);

                                        if (currentMenuLi.parent().attr('class').indexOf('more-tab') >= 0) {
                                            var cloneThis = currentMenuLi.clone(true);
                                            var cloneLast = $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:not([class_]):last()').clone(true);
                                            currentMenuLi.remove();
                                            $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:not([class_]):last()').remove();
                                            $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:not([class_]):last()').after(cloneThis);
                                            $('div.wrap-content-header .wrap-header-tabbox ul.more-tab').prepend(cloneLast);
                                        }
                                        $.DHB.menuCookie({ id: sUrlID, title: settings.title, url: settings.url });
                                    }

                                }
                            }
                            htmlobj = app.httpAjax.post(options);
                            //htmlobj = $.ajax({
                            //    url: settings.url,
                            //    data: {
                            //        //'_m_': '1'
                            //    },
                            //    async: false,
                            //    cache: (settings.nocache == '1' ? false : true),
                            //    dataType: 'html',
                            //    complete: function (xhr, ts) {
                            //        var regjson = /^{([\s\S]*)}$/;
                            //        if (regjson.test(xhr.responseText)) {
                            //            AjaxLoginCheck(JSON.parse(xhr.responseText));
                            //        } else {
                            //            $.DHB.closeButterbar();
                            //            currentContent.html(xhr.responseText);
                            //            //
                            //            _ = '[contentID="' + settings.id + '"] ';
                            //            currentMenuA.attr('_url', settings.url);
                            //            if (settings.title) {
                            //                currentMenuA.attr('_title', settings.title).attr('title', settings.title).html('<span>' + settings.title + '</span>');
                            //            }
                            //            if (currentMenuLi.attr('menuID') != currentActive.attr('menuID')) {
                            //                currentMenuLi.attr("class", "active"); currentActive.removeClass("active");
                            //            }
                            //            if (currentMenuLi.parent().attr('class') == 'more-tab') { currentMenuLi.parent().show().css({ 'height': 'auto' }); } else {
                            //                $('div.wrap-content-header .wrap-header-tabbox ul.more-tab').css({ 'display': 'none', 'height': 'auto' });
                            //            }
                            //            $.DHB.closeButterbar();
                            //            jQuery.DHB.ready(true);
                            //            $.DHB.contentbind(_);
                                        
                            //            if (currentMenuLi.parent().attr('class').indexOf('more-tab') >= 0) {
                            //                var cloneThis = currentMenuLi.clone(true);
                            //                var cloneLast = $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:not([class_]):last()').clone(true);
                            //                currentMenuLi.remove();
                            //                $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:not([class_]):last()').remove();
                            //                $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:not([class_]):last()').after(cloneThis);
                            //                $('div.wrap-content-header .wrap-header-tabbox ul.more-tab').prepend(cloneLast);
                            //            }
                            //            $.DHB.menuCookie({ id: sUrlID, title: settings.title, url: settings.url });
                            //        }
                                    
                            //    }
                            //});
                        } else {
                            if (currentMenuLi.attr('menuID') != currentActive.attr('menuID')) {
                                currentMenuLi.attr("class", "active"); currentActive.removeClass("active");
                            }
                            if (currentMenuLi.parent().attr('class') == 'more-tab') { currentMenuLi.parent().show().css({ 'height': 'auto' }); } else {
                                $('div.wrap-content-header .wrap-header-tabbox ul.more-tab').css({ 'display': 'none', 'height': 'auto' });
                            }
                            if (currentMenuLi.parent().attr('class').indexOf('more-tab') >= 0) {
                                var cloneThis = currentMenuLi.clone(true); var cloneLast = $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:not([class_]):last()').clone(true); currentMenuLi.remove(); $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:not([class_]):last()').remove(); $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:not([class_]):last()').after(cloneThis); $('div.wrap-content-header .wrap-header-tabbox ul.more-tab').prepend(cloneLast);
                            }
                            $.DHB.menuCookie({ id: sUrlID, title: settings.title, url: settings.url });
                            jQuery.DHB.ready();
                        }
                    }
                    else {
                        
                        //未打开页面
                        var oldCurrentObj = currentTab();   //获取当前显示Tab内容
                        if (settings.firstinit == '1') {
                            var currentMenuLi = $('div.wrap-content-header .wrap-header-tabbox ul > li[class="active"]');
                            
                            $('div.wrap-content-header .wrap-header-tabbox ul > li' + (app.c.init_menu === false ? ':first()' : '[class="active"]')).after('<li data-init="0" class="active" menuID="' + sUrlID + '"><a title="' + settings.title + '" _url="' + settings.url + '" _title="' + settings.title + '" _id="' + settings.id + '"><span>' + settings.title + '</span></a><span class="wrap-header-navdel" tabId="' + settings.id + '"><i class="fa caretnew_del">&times;</i></span></li>');
                            $('div.wrap-content-body .wrap-tab-contentbox[contentID="index"]').after('<div class="wrap-tab-contentbox" contentID="' + sUrlID + '" controller="" action=""></div>');
                            oldCurrentObj.hide();
                            currentMenuLi.removeClass("active");
                            if (typeof (currentMenuLi) == 'undefined') {
                                currentMenuLi.parent().show().css({ 'height': 'auto' });
                            }
                            else {
                                $('div.wrap-content-header .wrap-header-tabbox ul.more-tab').css({ 'display': 'none', 'height': 'auto' });
                            }
                            bindMenuMore();
                            bindMenuAdd();
                            //
                            bindMenuDel();
                            setTimeout(function () {
                                app.c.cookieMenu_(settings.id);
                            }, 1);
                        }
                        else {
                            $.DHB.showButterbar();

                            
                            //加载核心
                            var options = {
                                url: settings.url,
                                data: {
                                    //'_m_': '1'
                                },
                                type: "GET",
                                cache: (settings.nocache == '1' ? false : true),
                                async:false,
                                dataType: 'html',
                                complete: function (xhr, ts) {
                                   // 
                                    var regjson = /^{([\s\S]*)}$/;

                                    if (regjson.test(xhr.responseText)) {
                                        AjaxLoginCheck(JSON.parse(xhr.responseText));
                                    } else {
                                        _ = '[contentID="' + settings.id + '"] ';
                                        $.DHB.closeButterbar();
                                        var currentMenuLi = $('div.wrap-content-header .wrap-header-tabbox ul > li[class="active"]');
                                        $('div.wrap-content-header .wrap-header-tabbox ul > li' + (app.c.init_menu === false ? ':first()' : '[class="active"]'))
                                            .after('<li data-init="1" class="active" menuID="' + sUrlID + '"><a title="' + settings.title + '" _url="' + settings.url + '" _title="' + settings.title + '" _id="' + settings.id + '"><span>' + settings.title + '</span></a><span class="wrap-header-navdel" tabId="' + settings.id + '"><i class="fa caretnew_del">&times;</i></span></li>');
                                       
                                        $('div.wrap-content-body .wrap-tab-contentbox[contentID="index"]')
                                            .after('<div class="wrap-tab-contentbox" contentID="' + sUrlID + '" controller="" action="">' + xhr.responseText + '</div>');
                                        oldCurrentObj.hide();
                                        currentMenuLi.removeClass("active");
                                        if (typeof (currentMenuLi) == 'undefined') {
                                            currentMenuLi.parent().show().css({ 'height': 'auto' });
                                        }
                                        else {
                                            $('div.wrap-content-header .wrap-header-tabbox ul.more-tab').css({ 'display': 'none', 'height': 'auto' });
                                        }
                                        if ($('div.wrap-content-header .wrap-header-tabbox ul.first-level li').length > 13) {
                                            //
                                            $('div.wrap-content-header .wrap-header-tabbox ul.first-level li[class=""]:last() .wrap-header-navdel').click();
                                        }
                                        bindMenuMore();
                                        bindMenuAdd();
                                        //
                                        bindMenuDel();
                                        jQuery.DHB.ready(true);
                                        
                                        $.DHB.contentbind(_);
                                        $.DHB.menuCookie({ id: sUrlID, title: settings.title, url: settings.url });
                                    }
                                }
                            }
                            //setTimeout(function () {
                                htmlobj = app.httpAjax.post(options);
                            //},500);
                            //htmlobj = app.httpAjax.post(options);

                            //htmlobj = $.ajax({
                            //    url: settings.url,
                            //    data: {
                            //        //'_m_': '1'
                            //    },
                            //    async: false,
                            //    cache: (settings.nocache == '1' ? false : true),
                            //    dataType: 'html',
                            //    complete: function (xhr, ts) {
                            //        //
                            //        var regjson = /^{([\s\S]*)}$/;

                            //        if (regjson.test(xhr.responseText)) {
                            //            AjaxLoginCheck(JSON.parse(xhr.responseText));
                            //        } else {
                            //            _ = '[contentID="' + settings.id + '"] ';
                            //            $.DHB.closeButterbar();
                            //            var currentMenuLi = $('div.wrap-content-header .wrap-header-tabbox ul > li[class="active"]');
                            //            $('div.wrap-content-header .wrap-header-tabbox ul > li' + (app.c.init_menu === false ? ':first()' : '[class="active"]'))
                            //                .after('<li data-init="1" class="active" menuID="' + sUrlID + '"><a title="' + settings.title + '" _url="' + settings.url + '" _title="' + settings.title + '" _id="' + settings.id + '"><span>' + settings.title + '</span></a><span class="wrap-header-navdel" tabId="' + settings.id + '"><i class="fa caretnew_del">&times;</i></span></li>');
                            //            $('div.wrap-content-body .wrap-tab-contentbox[contentID="index"]')
                            //                .after('<div class="wrap-tab-contentbox" contentID="' + sUrlID + '" controller="" action="">' + xhr.responseText + '</div>');
                            //            oldCurrentObj.hide();
                            //            currentMenuLi.removeClass("active");
                            //            if (typeof (currentMenuLi) == 'undefined') {
                            //                currentMenuLi.parent().show().css({ 'height': 'auto' });
                            //            }
                            //            else {
                            //                $('div.wrap-content-header .wrap-header-tabbox ul.more-tab').css({ 'display': 'none', 'height': 'auto' });
                            //            }
                            //            if ($('div.wrap-content-header .wrap-header-tabbox ul.first-level li').length > 10) {
                            //                //
                            //                $('div.wrap-content-header .wrap-header-tabbox ul.first-level li[class=""]:last() .wrap-header-navdel').click();
                            //            }
                            //            bindMenuMore();
                            //            bindMenuAdd();
                            //            //
                            //            bindMenuDel();
                            //            jQuery.DHB.ready(true);
                            //            $.DHB.contentbind(_);
                            //            $.DHB.menuCookie({ id: sUrlID, title: settings.title, url: settings.url });
                            //        }
                            //    }


                            //    //,success: function(dataInfo) {
                            //    //},
                            //    //error: function (dataInfo) {
                            //    //    //
                            //    //}

                            //});
                        }
                        
                    }
                }
            }
        }
    };
    //删除内页
    var deleteMenu = function (id) {
       
        if ($('[contentID="' + id + '"]').length > 0) {
            var currentBool = $('[menuID="' + id + '"]').attr('class') == 'active';
            var next = $('[menuID="' + id + '"]').next();
            var prev = $('[menuID="' + id + '"]').prev();
            if (prev.length==0) {
                prev = $('[menuID="' + id + '"]').parents('li').prev();
            }
            $('[contentID="' + id + '"]').remove(); $('[menuID="' + id + '"]').remove();
            if (currentBool) {
                if (next.length > 0 && typeof (next.attr('class_')) == 'undefined') {
                    next.click();
                } else {
                    if (next.length <= 0 || typeof (next.attr('class_')) != 'undefined') {
                        
                        prev.click();
                    } else {
                        $('div.wrap-header-tabbox li').eq(0).attr('class', 'active'); $("div.wrap-tab-contentbox").eq(0).show();
                    }
                }
            }
            $.DHB.menuCookie({ id: id }, 'del'); bindMenuMore();
        }
    };

    //外面删除menuTab  lz add 2016-10-10
    $.fn.menuTab.deleteMenu = function (id) {
        deleteMenu(id);
    };


    //
    var bindMenuAdd = function () {
        $("div.wrap-header-tabbox ul.first-level li.active").bind("click", function (e) {
            if ($(this).hasClass('active') === false) {
                $('div.wrap-header-tabbox ul.first-level li').attr("class", ""); $(this).attr("class", "active"); if ($(this).parent().attr('class') != 'more-tab') {
                    $('div.wrap-content-header .wrap-header-tabbox ul.more-tab').css({ 'display': 'none', 'height': 'auto' });
                }
                $("div.wrap-tab-contentbox").hide(); $("div[contentID='" + $(this).attr('menuID') + "']").show(); $('html,body').animate({ scrollTop: 0 }, 10);
            }
        });
    };
    //
    var bindMenuDel = function () {
        //
        $("div.wrap-header-tabbox li.active .wrap-header-navdel").bind("click", function () {
            //
            deleteMenu($(this).attr("tabId"));
        });
    };
    var bindMenuMore = function () {
        //
        var tabParentWidth = Math.floor(($('div.wrap-content-header .wrap-header-tabbox ul.first-level').parent().width() - 140) / 90) - 1, tabWidth = $('div.wrap-content-header .wrap-header-tabbox ul.first-level li').length; if (tabWidth > tabParentWidth) {
            var numMore = tabParentWidth - 1; if ($('div.wrap-content-header .wrap-header-tabbox ul.first-level>li[class_="menu-more"]').length > 0) {
                var cloneTab = $('div.wrap-content-header .wrap-header-tabbox ul.more-tab>li').clone(true); $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li[class_="menu-more"]').remove(); $('div.wrap-content-header .wrap-header-tabbox ul.first-level').append(cloneTab);
            }
            var cloneTab = $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:gt(' + numMore + ')').clone(true); $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li:gt(' + numMore + ')').remove(); $('div.wrap-content-header .wrap-header-tabbox ul.first-level').append('<li class="" class_="menu-more" style="width: 30px;min-width: 30px;position: relative;"><a title="更多"><i style="font-size: 16px;" class="fa fa-caret-down"></i></a><ul class="more-tab"></ul></li>'); $('div.wrap-content-header .wrap-header-tabbox ul.first-level > li[class_="menu-more"] > ul').append(cloneTab); if ($('div.wrap-content-header .wrap-header-tabbox ul.more-tab>li').length < 1) {
                $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li[class_="menu-more"]').remove();
            }
        } else {
            if ($('div.wrap-content-header .wrap-header-tabbox ul.first-level>li[class_="menu-more"]').length > 0) { var cloneTab = $('div.wrap-content-header .wrap-header-tabbox ul.more-tab>li').clone(true); $('div.wrap-content-header .wrap-header-tabbox ul.first-level>li[class_="menu-more"]').remove(); $('div.wrap-content-header .wrap-header-tabbox ul.first-level').append(cloneTab); }
        }
    };
    var loadMessage = function (objCurrent) {
        objCurrent.html('<div class="message-loading" style="position: relative;top: 35%"><img src="' + publicSettings.publicUrl + 'static/images/load.gif" alt="Load..." style="display:block;margin:auto"><p class="text-center">正在努力加载...</p></div>');
    };
})(jQuery);

; !function (e) {
    function n() { var e = arguments[0], r = n.cache; return r[e] && r.hasOwnProperty(e) || (r[e] = n.parse(e)), n.format.call(null, r[e], arguments) }
    function r(e) { return Object.prototype.toString.call(e).slice(8, -1).toLowerCase() }
    function t(e, n) { return Array(n + 1).join(e) } var i = { not_string: /[^s]/, number: /[dief]/, text: /^[^\x25]+/, modulo: /^\x25{2}/, placeholder: /^\x25(?:([1-9]\d*)\$|\(([^\)]+)\))?(\+)?(0|'[^$])?(-)?(\d+)?(?:\.(\d+))?([b-fiosuxX])/, key: /^([a-z_][a-z_\d]*)/i, key_access: /^\.([a-z_][a-z_\d]*)/i, index_access: /^\[(\d+)\]/, sign: /^[\+\-]/ };
    n.format = function (e, s) {
        var a, o, l, f, c, p, u, d = 1, g = e.length, h = "", x = [], b = !0, y = "";
        for (o = 0; g > o; o++) if (h = r(e[o]), "string" === h) x[x.length] = e[o]; else if ("array" === h) { if (f = e[o], f[2]) for (a = s[d], l = 0; l < f[2].length; l++) { if (!a.hasOwnProperty(f[2][l])) throw new Error(n("[sprintf] property '%s' does not exist", f[2][l])); a = a[f[2][l]] } else a = f[1] ? s[f[1]] : s[d++]; if ("function" == r(a) && (a = a()), i.not_string.test(f[8]) && "number" != r(a) && isNaN(a)) throw new TypeError(n("[sprintf] expecting number but found %s", r(a))); switch (i.number.test(f[8]) && (b = a >= 0), f[8]) { case "b": a = a.toString(2); break; case "c": a = String.fromCharCode(a); break; case "d": case "i": a = parseInt(a, 10); break; case "e": a = f[7] ? a.toExponential(f[7]) : a.toExponential(); break; case "f": a = f[7] ? parseFloat(a).toFixed(f[7]) : parseFloat(a); break; case "o": a = a.toString(8); break; case "s": a = (a = String(a)) && f[7] ? a.substring(0, f[7]) : a; break; case "u": a >>>= 0; break; case "x": a = a.toString(16); break; case "X": a = a.toString(16).toUpperCase() } !i.number.test(f[8]) || b && !f[3] ? y = "" : (y = b ? "+" : "-", a = a.toString().replace(i.sign, "")), p = f[4] ? "0" === f[4] ? "0" : f[4].charAt(1) : " ", u = f[6] - (y + a).length, c = f[6] && u > 0 ? t(p, u) : "", x[x.length] = f[5] ? y + a + c : "0" === p ? y + c + a : c + y + a } return x.join("")
    },
    n.cache = {},
    n.parse = function (e) {
        for (var n = e, r = [], t = [], s = 0; n;) {
            if (null !== (r = i.text.exec(n))) t[t.length] = r[0];
            else if (null !== (r = i.modulo.exec(n))) t[t.length] = "%";
            else {
                if (null === (r = i.placeholder.exec(n))) throw new SyntaxError("[sprintf] unexpected placeholder");
                if (r[2]) {
                    s |= 1; var a = [], o = r[2], l = []; if (null === (l = i.key.exec(o)))
                        throw new SyntaxError("[sprintf] failed to parse named argument key"); for (a[a.length] = l[1]; "" !== (o = o.substring(l[0].length)) ;)
                            if (null !== (l = i.key_access.exec(o))) a[a.length] = l[1];
                            else {
                                if (null === (l = i.index_access.exec(o)))
                                    throw new SyntaxError("[sprintf] failed to parse named argument key"); a[a.length] = l[1]
                            } r[2] = a
                }
                else s |= 2;
                if (3 === s)
                    throw new Error("[sprintf] mixing positional and named placeholders is not (yet) supported"); t[t.length] = r
            } n = n.substring(r[0].length)
        }
        return t
    }; var s = function (e, r, t) {
        return t = (r || []).slice(0), t.splice(0, 0, e), n.apply(null, t)
    };
    "undefined" != typeof exports ? (exports.sprintf = n, exports.vsprintf = s) : (e.sprintf = n, e.vsprintf = s, "function" == typeof define && define.amd && define(function () { return { sprintf: n, vsprintf: s } }))
}("undefined" == typeof window ? this : window);




var $dp, WdatePicker;
!function () {
    function e() {
        try {
            m[D],
			m.$dp = m.$dp || {}
        } catch (e) {
            m = M,
			$dp = $dp || {}
        }
        var e = {
            win: M,
            $: function ($) {
                var e = jQuery.fn.menuTab.current(1);
                return "string" == typeof $ ? "undefined" != typeof e ? jQuery(e + "#" + $).get(0) : M[D].getElementById($) : $
            },
            $D: function ($, e) {
                return this.$DV(this.$($).value, e)
            },
            $DV: function (e, $) {
                if ("" != e) {
                    if (this.dt = $dp.cal.splitDate(e, $dp.cal.dateFmt), $) for (var t in $) if (void 0 === this.dt[t]) this.errMsg = "invalid property:" + t;
                    else if (this.dt[t] += $[t], "M" == t) {
                        var n = $.M > 0 ? 1 : 0,
						a = new Date(this.dt.y, this.dt.M, 0).getDate();
                        this.dt.d = Math.min(a + n, this.dt.d)
                    }
                    if (this.dt.refresh()) return this.dt
                }
                return ""
            },
            show: function () {
                for (var e = m[D].getElementsByTagName("div"), $ = 1e5, t = 0; t < e.length; t++) {
                    var n = parseInt(e[t].style.zIndex);
                    n > $ && ($ = n)
                }
                this.dd.style.zIndex = $ + 2,
				h(this.dd, "block"),
				h(this.dd.firstChild, "")
            },
            unbind: function ($) {
                $ = this.$($),
				$.initcfg && (n($, "onclick",
				function () {
				    p($.initcfg)
				}), n($, "onfocus",
				function () {
				    p($.initcfg)
				}))
            },
            hide: function () {
                h(this.dd, "none")
            },
            attachEvent: t
        };
        for (var a in e) m.$dp[a] = e[a];
        $dp = m.$dp
    }
    function t(e, t, n, $) {
        if (e.addEventListener) {
            var a = t.replace(/on/, "");
            n._ieEmuEventHandler = function ($) {
                return n($)
            },
			e.addEventListener(a, n._ieEmuEventHandler, $)
        } else e.attachEvent(t, n)
    }
    function n(e, $, t) {
        if (e.removeEventListener) {
            var n = $.replace(/on/, "");
            t._ieEmuEventHandler = function ($) {
                return t($)
            },
			e.removeEventListener(n, t._ieEmuEventHandler, !1)
        } else e.detachEvent($, t)
    }
    function a(e, $, t) {
        if (typeof e != typeof $) return !1;
        if ("object" == typeof e) {
            if (!t) for (var n in e) {
                if ("undefined" == typeof $[n]) return !1;
                if (!a(e[n], $[n], !0)) return !1
            }
            return !0
        }
        return "function" == typeof e && "function" == typeof $ ? e.toString() == $.toString() : e == $
    }
    function i() {
        for (var e, t, $ = M[D][L]("script"), n = 0; n < $.length && (e = $[n].getAttribute("src") || "", e = e.substr(0, e.toLowerCase().indexOf("wdatepicker.js")), t = e.lastIndexOf("/"), t > 0 && (e = e.substring(0, t + 1)), !e) ; n++);
        return e
    }
    function r(e, $, t) {
        var n = M[D][L]("HEAD").item(0),
		a = M[D].createElement("link");
        //
        n && (a.href = e, a.rel = "stylesheet", a.type = "text/css", $ && (a.title = $), t && (a.charset = t), n.appendChild(a))
    }
    function o($) {
        $ = $ || m;
        for (var e = 0,
		t = 0; $ != m;) {
            for (var n = $.parent[D][L]("iframe"), a = 0; a < n.length; a++) try {
                if (n[a].contentWindow == $) {
                    var i = l(n[a]);
                    e += i.left,
					t += i.top;
                    break
                }
            } catch (e) { }
            $ = $.parent
        }
        return {
            leftM: e,
            topM: t
        }
    }
    function l(e, t) {
        if (e.getBoundingClientRect) return e.getBoundingClientRect();
        var n = {
            ROOT_TAG: /^body|html$/i,
            OP_SCROLL: /^(?:inline|table-row)$/i
        },
		a = !1,
		i = null,
		r = e.offsetTop,
		o = e.offsetLeft,
		l = e.offsetWidth,
		d = e.offsetHeight,
		c = e.offsetParent;
        if (c != e) for (; c;) o += c.offsetLeft,
		r += c.offsetTop,
		"fixed" == u(c, "position").toLowerCase() ? a = !0 : "body" == c.tagName.toLowerCase() && (i = c.ownerDocument.defaultView),
		c = c.offsetParent;
        for (c = e.parentNode; c.tagName && !n.ROOT_TAG.test(c.tagName) ;) (c.scrollTop || c.scrollLeft) && (n.OP_SCROLL.test(h(c)) || w && "visible" === c.style.overflow || (o -= c.scrollLeft, r -= c.scrollTop)),
		c = c.parentNode;
        if (!a) {
            var $ = s(i);
            o -= $.left,
			r -= $.top
        }
        return l += o,
		d += r,
		{
		    left: o,
		    top: r,
		    right: l,
		    bottom: d
		}
    }
    function d($) {
        $ = $ || m;
        var e = $[D],
		t = $.innerWidth ? $.innerWidth : e[E] && e[E].clientWidth ? e[E].clientWidth : e.body.offsetWidth,
		n = $.innerHeight ? $.innerHeight : e[E] && e[E].clientHeight ? e[E].clientHeight : e.body.offsetHeight;
        return {
            width: t,
            height: n
        }
    }
    function s($) {
        $ = $ || m;
        var e = $[D],
		t = e[E],
		n = e.body;
        return e = t && null != t.scrollTop && (t.scrollTop > n.scrollTop || t.scrollLeft > n.scrollLeft) ? t : n,
		{
		    top: e.scrollTop,
		    left: e.scrollLeft
		}
    }
    function c($) {
        try {
            var e = $ ? $.srcElement || $.target : null;
            $dp.cal && !$dp.eCont && $dp.dd && e != $dp.el && "block" == $dp.dd.style.display && $dp.cal.close()
        } catch (e) { }
    }
    function f() {
        $dp.status = 2
    }
    function p(n, i) {
        function r() {
            return !y || m == M || "complete" == m[D].readyState
        }
        function o() {
            if (b) {
                for (func = o.caller; null != func;) {
                    var $ = func.arguments[0];
                    if ($ && ($ + "").indexOf("Event") >= 0) return $;
                    func = func.caller
                }
                return null
            }
            return event
        }
        if ($dp) {
            e();
            var l = {};
            for (var d in n) l[d] = n[d];
            for (d in $) "$" != d.substring(0, 1) && void 0 === l[d] && (l[d] = $[d]);
            if (i) {
                if (!r()) return void (S = S || setInterval(function () {
                    "complete" == m[D].readyState && clearInterval(S),
					p(null, !0)
                },
				50));
                if (0 != $dp.status) return;
                $dp.status = 1,
				l.el = k,
				g(l, !0)
            } else if (l.eCont) l.eCont = $dp.$(l.eCont),
			l.el = k,
			l.autoPickDate = !0,
			l.qsEnabled = !1,
			g(l);
            else {
                if ($.$preLoad && 2 != $dp.status) return;
                var s = o();
                if ((M.event === s || s) && (l.srcEl = s.srcElement || s.target, s.cancelBubble = !0), l.el = l.el = $dp.$(l.el || l.srcEl), !l.el || l.el.My97Mark === !0 || l.el.disabled || $dp.dd && "none" != h($dp.dd) && "-970px" != $dp.dd.style.left) {
                    try {
                        l.el.My97Mark && (l.el.My97Mark = !1)
                    } catch (e) { }
                    return
                }
                s && 1 == l.el.nodeType && !a(l.el.initcfg, n) && ($dp.unbind(l.el), t(l.el, "focus" == s.type ? "onclick" : "onfocus",
				function () {
				    p(n)
				}), l.el.initcfg = n),
				g(l)
            }
        }
    }
    function u(e, $) {
        return e.currentStyle ? e.currentStyle[$] : document.defaultView.getComputedStyle(e, !1)[$]
    }
    function h(e, $) {
        if (e) {
            if (null == $) return u(e, "display");
            e.style.display = $
        }
    }
    function g(e, t) {
        function n(e, t) {
            function n() {
                var n = t.getRealLang();
                e.lang = n.name,
				e.skin = t.skin;
                var $ = ["<head><script>", "", "var doc=document, $d, $dp, $cfg=doc.cfg, $pdp = parent.$dp, $dt, $tdt, $sdt, $lastInput, $IE=$pdp.ie, $FF = $pdp.ff,$OPERA=$pdp.opera, $ny, $cMark = false;", "if($cfg.eCont){$dp = {};for(var p in $pdp)$dp[p]=$pdp[p];}else{$dp=$pdp;};for(var p in $cfg){$dp[p]=$cfg[p];}", "doc.oncontextmenu=function(){try{$c._fillQS(!$dp.has.d,1);showB($d.qsDivSel);}catch(e){};return false;};", "</script>"];
                r && ($[1] = 'document.domain="' + i + '";');
                for (var o = 0; o < d.length; o++) d[o].name == t.skin && $.push('<link rel="stylesheet" type="text/css" href="' + publicSettings.publicUrl + "static/js/My97DatePicker/skin/" + d[o].name + "/datepicker.css?" + publicSettings.version + '" charset="' + d[o].charset + '"/>');
                $.push('<script src="' + publicSettings.publicUrl + 'static/js/My97DatePicker/calendar.js"></script>'),
				$.push('</head><body leftmargin="0" topmargin="0" tabindex=0></body></html>'),
				$.push("<script>var t;t=t||setInterval(function(){if(doc.ready){new My97DP();$cfg.onload();$c.autoSize();$cfg.setPos($dp);clearInterval(t);}},20);</script>"),
				t.setPos = a,
				t.onload = f,
				l.write("<html>"),
				l.cfg = t,
				l.write($.join("")),
				l.close()
            }
            var i = m[D].domain,
			r = !1,
			o = '<iframe hideFocus=true width=9 height=7 frameborder=0 border=0 scrolling=no src="about:blank"></iframe>';
            e.innerHTML = o;
            var l, d = ($.$langList, $.$skinList);
            try {
                l = e.lastChild.contentWindow[D]
            } catch (t) {
                r = !0,
				e.removeChild(e.lastChild);
                var s = m[D].createElement("iframe");
                return s.hideFocus = !0,
				s.frameBorder = 0,
				s.scrolling = "no",
				s.src = "javascript:(function(){var d=document;d.open();d.domain='" + i + "';})()",
				e.appendChild(s),
				void setTimeout(function () {
				    l = e.lastChild.contentWindow[D],
					n()
				},
				97)
            }
            n()
        }
        function a(e) {
            var t = e.position.left,
			n = e.position.top,
			a = e.el;
            if (a != k) {
                a == e.srcEl || "none" != h(a) && "hidden" != a.type || (a = e.srcEl);
                var i = l(a),
				$ = o(M),
				r = d(m),
				c = s(m),
				f = $dp.dd.offsetHeight,
				p = $dp.dd.offsetWidth;
                if (isNaN(n) && (n = 0), $.topM + i.bottom + f > r.height && $.topM + i.top - f > 0) n += c.top + $.topM + i.top - f - 2;
                else {
                    n += c.top + $.topM + i.bottom;
                    var u = n - c.top + f - r.height;
                    u > 0 && (n -= u)
                }
                isNaN(t) && (t = 0),
				t += c.left + Math.min($.leftM + i.left, r.width - p - 5) - (y ? 2 : 0),
				e.dd.style.top = n + "px",
				e.dd.style.left = t + "px"
            }
        }
        var i = e.el ? e.el.nodeName : "INPUT";
        if (t || e.eCont || new RegExp(/input|textarea|div|span|p|a/gi).test(i)) {
            if (e.elProp = "INPUT" == i ? "value" : "innerHTML", "auto" == e.lang && (e.lang = y ? navigator.browserLanguage.toLowerCase() : navigator.language.toLowerCase()), !e.eCont) for (var r in e) $dp[r] = e[r]; !$dp.dd || e.eCont || $dp.dd && (e.getRealLang().name != $dp.dd.lang || e.skin != $dp.dd.skin) ? e.eCont ? n(e.eCont, e) : ($dp.dd = m[D].createElement("DIV"), $dp.dd.style.cssText = "position:absolute", m[D].body.appendChild($dp.dd), n($dp.dd, e), t ? $dp.dd.style.left = $dp.dd.style.top = "-970px" : ($dp.show(), a($dp))) : $dp.cal && ($dp.show(), $dp.cal.init(), $dp.eCont || a($dp))
        }
    }
    var $ = {
        $langList: [{
            name: "en",
            charset: "UTF-8"
        },
		{
		    name: "zh-cn",
		    charset: "UTF-8"
		},
		{
		    name: "zh-tw",
		    charset: "UTF-8"
		}],
        $skinList: [{
            name: "default",
            charset: "UTF-8"
        },
		{
		    name: "whyGreen",
		    charset: "gb2312"
		}],
        $wdate: !0,
        $crossFrame: !0,
        $preLoad: !1,
        $dpPath: "",
        doubleCalendar: !1,
        enableKeyboard: !0,
        enableInputMask: !0,
        autoUpdateOnChanged: null,
        weekMethod: "ISO8601",
        position: {},
        lang: "zh-cn",
        skin: "default",
        dateFmt: "yyyy-MM-dd",
        realDateFmt: "yyyy-MM-dd",
        realTimeFmt: "HH:mm:ss",
        realFullFmt: "%Date %Time",
        minDate: "1900-01-01 00:00:00",
        maxDate: "2099-12-31 23:59:59",
        startDate: "",
        alwaysUseStartDate: !1,
        yearOffset: 1911,
        firstDayOfWeek: 0,
        isShowWeek: !1,
        highLineWeekDay: !0,
        isShowClear: !0,
        isShowToday: !0,
        isShowOK: !0,
        isShowOthers: !0,
        readOnly: !1,
        errDealMode: 0,
        autoPickDate: null,
        qsEnabled: !0,
        autoShowQS: !1,
        opposite: !1,
        hmsMenuCfg: {
            H: [1, 6],
            m: [5, 6],
            s: [15, 4]
        },
        opposite: !1,
        specialDates: null,
        specialDays: null,
        disabledDates: null,
        disabledDays: null,
        onpicking: null,
        onpicked: null,
        onclearing: null,
        oncleared: null,
        ychanging: null,
        ychanged: null,
        Mchanging: null,
        Mchanged: null,
        dchanging: null,
        dchanged: null,
        Hchanging: null,
        Hchanged: null,
        mchanging: null,
        mchanged: null,
        schanging: null,
        schanged: null,
        eCont: null,
        vel: null,
        elProp: "",
        errMsg: "",
        quickSel: [],
        has: {},
        getRealLang: function () {
            for (var e = $.$langList,
			t = 0; t < e.length; t++) if (e[t].name == this.lang) return e[t];
            return e[0]
        }
    };
    WdatePicker = p;
    var m, v, y, b, w, M = window,
	k = {
	    innerHTML: ""
	},
	D = "document",
	E = "documentElement",
	L = "getElementsByTagName",
	C = navigator.appName;
    //if ("Microsoft Internet Explorer" == C ? y = !0 : "Opera" == C ? w = !0 : b = !0, v = $.$dpPath || i(), $.$wdate && r(publicSettings.publicUrl + "static/js/My97DatePicker/skin/WdatePicker.css?" + publicSettings.version), m = M, $.$crossFrame) try {
    if ("Microsoft Internet Explorer" == C ? y = !0 : "Opera" == C ? w = !0 : b = !0, v = $.$dpPath || i(), $.$wdate && r("/static/js/My97DatePicker/skin/WdatePicker.css"), m = M, $.$crossFrame) try {
        for (; m.parent != m && 0 == m.parent[D][L]("frameset").length;) m = m.parent
    } catch (e) { }
    m.$dp || (m.$dp = {
        ff: b,
        ie: y,
        opera: w,
        status: 0,
        defMinDate: $.minDate,
        defMaxDate: $.maxDate
    }),
	e(),
	$.$preLoad && 0 == $dp.status && t(M, "onload",
	function () {
	    p(null, !0)
	}),
	M[D].docMD || (t(M[D], "onmousedown", c, !0), M[D].docMD = !0),
	m[D].docMD || (t(m[D], "onmousedown", c, !0), m[D].docMD = !0),
	t(M, "onunload",
	function () {
	    $dp.dd && h($dp.dd, "none")
	});
    var S
}();









////小数位验证 lz 2016-09-06
//function check_digit(obj, num) {
//    var median = num;
//    var value = new String($(obj).val().replace(/\s+/g, ""));
//    $(obj).val(value);
//    if (isNaN(value)) {
//        $.DHB.message({ 'content': '仅允许输入数值', 'time': 4000, 'type': 'e' });
//        value = value + "";
//        value = value.substring(0, value.length - 1);
//        value = value.substring(0, 9);
//        $(obj).val(value);
//    }
//    if (value >= 1000000000 || value < -1000000000) {
//        alert("输入值需在-1000000000-1000000000之间");
//        value = value + "";
//        value = value.substring(0, value.length - 1);
//        value = value.toString().substring(0, 9);
//        $(obj).val(value);
//    }
//    value = value + "";
//    median = parseInt(median);
//    if (value.indexOf(".") != -1) {
//        value = value.substring(0, value.indexOf(".") + median + 1);
//        $(obj).val(value);
//    }
//}


//小数位验证 lz 2016-09-06
function check_digit(obj, num) {
    //
    //如果是上下左右Tab按键 回车 不处理
    var event = arguments.callee.caller.arguments[0] || window.event;//消除浏览器差异
    if (typeof (event) != 'undefined') {
        var keyCode = event.keyCode;

        if (keyCode == 37 || keyCode == 38 || keyCode == 39 || keyCode == 40 || keyCode == 9 || keyCode == 13 || keyCode == 8) {
            return false;
        }
    }
    var median = num;
    
    var value = new String($(obj).val().replace(/\s+/g, ""));
    //value=parseFloat(value);
    //value = Math.abs(value);
    //$(obj).val(value);
    if (isNaN(value)) {
        $.DHB.message({ 'content': '仅允许输入数字', 'time': 4000, 'type': 'i' });
        //
        var old_data = $(obj).attr('old-data');
        if (typeof (old_data) == 'undefined') { old_data = '0'; }
        value = old_data;
        value = value.substring(0, 9);
        $(obj).val(value);
    }
    if (value >= 1000000000 || value < -1000000000) {
        alert("输入值需在-1000000000-1000000000之间");
        value = value + "";
        value = value.substring(0, value.length - 1);
        value = value.toString().substring(0, 9);
        $(obj).val(value);
    }
    value = value + "";
    median = parseInt(median);
    if (value.indexOf(".") != -1) {
        value = value.substring(0, value.indexOf(".") + median + 1);
        //$(obj).val(value);
    }
    //else {
    //    value = value + ".0000000000";
    //    if (value.indexOf(".") != -1) {
    //        value = value.substring(0, value.indexOf(".") + median + 1);
    //        $(obj).val(value);
    //    }
    //}
    $(obj).attr('old-data', value);
}

function limit_num(value, num) {
    //如果是上下左右Tab按键 回车 不处理
    var event = arguments.callee.caller.arguments[0] || window.event;//消除浏览器差异

    if (typeof (event) != 'undefined') {
        var keyCode = event.keyCode;

        if (keyCode == 37 || keyCode == 38 || keyCode == 39 || keyCode == 40 || keyCode == 9 || keyCode == 13 || keyCode == 8) {
            return false;
        }
    }


    num = parseInt(num);
    value = parseFloat(value).toString();

    if (value.indexOf("NaN") != -1) {
        value = "0";
    }

    if (value.indexOf(".") != -1) {
        value = value + "0000000000";
        //value = value.substring(0, value.indexOf(".") + num + 1);
        //value = parseFloat(value).toFixed(num)
        value = round2(parseFloat(value), num);
        return value;
    } else {
        value = value + ".0000000000";
        if (value.indexOf(".") != -1) {
            //value = value.substring(0, value.indexOf(".") + num + 1);
            //value = parseFloat(value).toFixed(num)
            value = round2(parseFloat(value), num);
            return value;
        }
    }
    return value;
}


function round2(number, fractionDigits) {
    with (Math) {
        var result = (round(number * pow(10, fractionDigits)) / pow(10, fractionDigits)).toString();
        if (result.indexOf(".") != -1) {
            result = result + "0000000000";
            result = result.substring(0, result.indexOf(".") + fractionDigits + 1);
        } else {
            result = result + ".0000000000";
            if (result.indexOf(".") != -1) {
                result = result.substring(0, result.indexOf(".") + fractionDigits + 1);
            }
        }
        return result;
    }
}



//加法函数   lz 2016-10-11
function accAdd(arg1, arg2) {
    var r1, r2, m;
    try {
        r1 = arg1.toString().split(".")[1].length;
    }
    catch (e) {
        r1 = 0;
    }
    try {
        r2 = arg2.toString().split(".")[1].length;
    }
    catch (e) {
        r2 = 0;
    }
    m = Math.pow(10, Math.max(r1, r2));
    return (arg1 * m + arg2 * m) / m;
}

//减法函数   lz 2016-10-11
function Subtr(arg1, arg2) {
    var r1, r2, m, n;
    try {
        r1 = arg1.toString().split(".")[1].length;
    }
    catch (e) {
        r1 = 0;
    }
    try {
        r2 = arg2.toString().split(".")[1].length;
    }
    catch (e) {
        r2 = 0;
    }
    m = Math.pow(10, Math.max(r1, r2));
    //last modify by deeka  
    //动态控制精度长度  
    n = (r1 >= r2) ? r1 : r2;
    return ((arg1 * m - arg2 * m) / m).toFixed(n);
}


//乘法函数   lz 2016-10-11
function accMul(arg1, arg2) {
    var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
    try {
        m += s1.split(".")[1].length;
    }
    catch (e) {
    }
    try {
        m += s2.split(".")[1].length;
    }
    catch (e) {
    }
    return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m);
}


//除法函数   lz 2016-10-11
function accDiv(arg1, arg2) {
    var t1 = 0, t2 = 0, r1, r2;
    try {
        t1 = arg1.toString().split(".")[1].length;
    }
    catch (e) {
    }
    try {
        t2 = arg2.toString().split(".")[1].length;
    }
    catch (e) {
    }
    with (Math) {
        r1 = Number(arg1.toString().replace(".", ""));
        r2 = Number(arg2.toString().replace(".", ""));
        return (r1 / r2) * pow(10, t2 - t1);
    }
}

/* 表单数据格式化为json串 */
$.fn.serializeJson = function () {
    var serializeObj = {};
    var array = this.serializeArray();
    var str = this.serialize();
    $(array).each(function () {
        if (serializeObj[this.name]) {
            if ($.isArray(serializeObj[this.name])) {
                serializeObj[this.name].push(this.value);
            } else {
                serializeObj[this.name] = [serializeObj[this.name], this.value];
            }
        } else {
            serializeObj[this.name] = this.value;
        }
    });
    return serializeObj;
};
/*
调用接口的方法
接口使用post方式,数据以json格式字符串提交
 */
app.request = function (url, data, success, error) {
    
    $.ajax({
        url: url,
        data:data,
        type: 'post',
        contentType: "application/x-www-form-urlencoded;charset=utf-8",
        dataType: "json",
        crossDomain: true,
        success: success,
        error: error
    });

}
//ajax统一方法
app.httpAjax = (function ($) {
    //
    var fun_post = function (options) {
        
        options = $.extend({            
            data: {},                    //请求参数
            headers: {},                 //请求头
            async: true,
            url: '',                     //请求地址，必填
            type: "GET",
            cache: false,
            dataType: 'json',             //服务器返回的数据类型
            datatype: 'json',
            beforeSend: null,             //发送请求前
            success: null,               //成功回调
            error: null,                 //异常回调
            complete: null              //请求完成后回调函数
        }, options || {});
        if(options.datatype!='json') options.dataType=options.datatype;
        
        $.ajax({
            async: options.async,             //true：异步请求
            cache: options.cache,           //false：不缓存当前页
            crossDomain: false,     //false：同域请求
            data: options.data,
            headers: options.headers,
            dataType:options.dataType,
            url: options.url,
            type: options.type,
            processData: true,
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            timeout: 10000,
            beforeSend: function (XHR) {
                if (options.beforeSend && $.isFunction(options.beforeSend))
                    options.beforeSend(XHR);
            },
            success: function (data, textStatus, jqXHR) {
                
                var ret = null;                
                if (data != undefined && jqXHR.status == 200 && $.trim(data).length > 0) {
                    

                    if (options.dataType == 'json') {                        
                        var type = typeof (data);
                        if (type == "object")
                            ret = data;
                        else
                            ret = JSON.parse(data);
                        if (data != null && data.Success == false) {
                            
                            if (data.Message != null && data.Message.length>0 && data.Message[0].indexOf("您未登录或登录已超时") != -1) {
                                $.DHB.message({ "content": data.Message[0], "type": "i" });
                                $.DHB.closeButterbar();
                                setTimeout(function () { location.replace('/Account/Login'); }, 1000);
                            }
                            else {
                                if (options.success && $.isFunction(options.success))
                                    options.success(ret, textStatus, jqXHR);
                            }

                        } else {
                            if (options.success && $.isFunction(options.success))
                                options.success(ret, textStatus, jqXHR);
                        }
                    }
                    else {
                        ret = data;
                        if (data != null) {
                            if (data.responseText) {
                                var reg = /^{([\s\S]*)}$/;
                                if (reg.test(XHR.responseText)) {
                                    var dataJson = JSON.parse(data.responseText);
                                    if (dataJson.Success == false) {
                                        if (data.Message != null && data.Message.length > 0 && dataJson.Message[0].indexOf("您未登录或登录已超时") != -1) {
                                            $.DHB.message({ "content": dataJson.Message[0], "type": "i" });
                                            $.DHB.closeButterbar();
                                            setTimeout(function () { location.replace('/Account/Login'); }, 1000);
                                        }
                                        else {
                                            if (options.success && $.isFunction(options.success))
                                                options.success(ret, textStatus, jqXHR);
                                        }

                                    }
                                }
                                else {
                                    if (options.success && $.isFunction(options.success))
                                        options.success(ret, textStatus, jqXHR);
                                }
                            } else {
                                if (options.success && $.isFunction(options.success))
                                    options.success(ret, textStatus, jqXHR);
                            }
                            
                            
                        }
                    }

                }
                
                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                
                console.error(XMLHttpRequest);
                if (options.error && $.isFunction(options.error))
                    options.error(XMLHttpRequest, textStatus, errorThrown)
            },
            complete: function (XHR, TS) {
                //
                var ret=null
                if (XHR) {
                    if (options.dataType == 'json') {
                        var type = typeof (XHR);
                        if (type == "object")
                            ret = XHR;
                        else
                            ret = JSON.parse(XHR);
                        if (XHR != null && XHR.Success == false) {
                            $.DHB.message({ "content": XHR.Message[0], "type": "i" });
                            if (XHR.Message[0].indexOf("您未登录或登录已超时") != -1) {
                                $.DHB.closeButterbar();
                                setTimeout(function () { location.replace('/Account/Login'); }, 1000);
                            }
                        } else {
                            if (options.complete && $.isFunction(options.complete))
                                options.complete(ret, TS);
                        }
                    } else {
                        
                        ret = XHR;
                        if (XHR != null) {
                            if (XHR.responseText) {
                                var reg = /^{([\s\S]*)}$/;
                            if (reg.test(XHR.responseText)) {
                                var XHRJson = JSON.parse(XHR.responseText);
                                if (XHRJson.Success == false) {
                                    $.DHB.message({ "content": XHRJson.Message[0], "type": "i" });
                                    if (XHRJson.Message[0].indexOf("您未登录或登录已超时") != -1) {
                                        $.DHB.closeButterbar();
                                        setTimeout(function () { location.replace('/Account/Login'); }, 1000);
                                    }
                                } else {
                                    if (options.complete && $.isFunction(options.complete))
                                        options.complete(ret, TS);
                                }
                            }else {
                                if (options.complete && $.isFunction(options.complete))
                                    options.complete(ret, TS);
                            }
                            } else {
                                if (options.complete && $.isFunction(options.complete))
                                    options.complete(ret, TS);
                            }
                            

                        }
                    }
                }
            }
        });
    }
    return { post: fun_post };
})(jQuery);



//GUID   lz 2016-11-09
function NewGuid() {
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
            guid += "-";
    }
    return guid;
}
//点击门店确认触发事件
app.shopmanagerdialogCallBack = function () {

}

function AjaxLoginCheck(dataInfo) {
    //
    if (dataInfo.Level=="0") {
        $.DHB.message({ "content": dataInfo.Message[0].toString(), "type": "i" });
        window.location.href = $.DHB.U('/Account/Login');
        
    }
   else if (dataInfo.toString().indexOf("获取购买服务信息失败") >= 0) {
        var dataObj = JSON.parse(dataInfo.toString());
        if (dataObj.Level.toString() == '4' && dataObj.Message.toString().indexOf("获取购买服务信息失败") >= 0) {
            alert(dataObj.Message.toString());
            window.location.href = $.DHB.U('/Account/Login');
        }
    }
   else if (dataInfo.Level == "2") {
       //$.DHB.message({ "content": dataInfo.Message[0].toString(), "type": "i" });
       if (dataInfo.Data != null && dataInfo.Data != '') {
           var url = dataInfo.Data;
           $.DHB.dialog({ 'title': '选择门店', 'url': $.DHB.U('shopmanager/set?url=' + url), 'id': 'dialog-shopmanager-set', 'confirm': app.shopmanagerdialogCallBack });
       } else {
           $.DHB.dialog({ 'title': '选择门店', 'url': $.DHB.U('shopmanager/set'), 'id': 'dialog-shopmanager-set', 'confirm': app.shopmanagerdialogCallBack });
       }
   } else if (dataInfo.Level == "3") {
       $.DHB.message({ "content": dataInfo.Message[0].toString(), "type": "i" });
   }
   else if (dataInfo.Level == "5") {
       $.DHB.message({ "content": dataInfo.Message[0].toString(), "type": "i" });
       setTimeout(function () {
           if (dataInfo.Data != null && dataInfo.Data != '') {
               var url = dataInfo.Data;
               window.location.href = $.DHB.U('/iframe/index?url=' + dataInfo.url);
           } else {
               window.location.href = $.DHB.U('/iframe/index')
           }
       },5000)
       
    }
}



function NewWin(url, id) {
    var a = document.createElement('a');
    a.setAttribute('href', url);
    a.setAttribute('target', '_blank');
    a.setAttribute('id', id);
    // 防止反复添加
    if (!document.getElementById(id)) document.body.appendChild(a);
    a.click();
}

//时间戳转y-m-d
Date.prototype.Format = function (fmt) {
    //
    var o = {
        "M+": this.getMonth() + 1,                 //月份
        "d+": this.getDate(),                    //日
        "h+": this.getHours(),                   //小时
        "m+": this.getMinutes(),                 //分
        "s+": this.getSeconds(),                 //秒
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度
        "S": this.getMilliseconds()             //毫秒
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
//开始时间不能大于结束时间
app.endThanStart=function(startTime, endTime) {
    //
    var startdate = new Date((startTime).replace(/-/g, "/"));
    var enddate = new Date((endTime).replace(/-/g, "/"));
    if (enddate < startdate) {
        return false;
    }
    else {
        return true;
    }
}
//去除字符串前后空格
app.str_del_bank = function (str) {
   return str.replace(/(^\s*)|(\s*$)/g, "");
}




