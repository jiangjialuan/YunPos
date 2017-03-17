!
function (e, r) {
    function t(e) {
        return function (r) {
            return {}.toString.call(r) == "[object " + e + "]"
        }
    }
    function n() {
        return x++
    }
    function i(e) {
        return e.match(S)[0]
    }
    function a(e) {
        for (e = e.replace(q, "/") ; e.match(C) ;) e = e.replace(C, "/");
        return e = e.replace(I, "$1/")
    }
    function s(e) {
        var r = e.length - 1,
		t = e.charAt(r);
        return "#" === t ? e.substring(0, r) : ".js" === e.substring(r - 2) || e.indexOf("?") > 0 || ".css" === e.substring(r - 3) || "/" === t ? e : e + ".js"
    }
    function o(e) {
        var r = A.alias;
        return r && T(r[e]) ? r[e] : e
    }
    function u(e) {
        var r, t = A.paths;
        return t && (r = e.match(j)) && T(t[r[1]]) && (e = t[r[1]] + r[2]),
		e
    }
    function c(e) {
        var r = A.vars;
        return r && e.indexOf("{") > -1 && (e = e.replace(G,
		function (e, t) {
		    return T(r[t]) ? r[t] : e
		})),
		e
    }
    function f(e) {
        var r = A.map,
		t = e;
        if (r) for (var n = 0,
		i = r.length; i > n; n++) {
            var a = r[n];
            if (t = w(a) ? a(e) || e : e.replace(a[0], a[1]), t !== e) break
        }
        return t
    }
    function l(e, r) {
        var t, n = e.charAt(0);
        if (R.test(e)) t = e;
        else if ("." === n) t = a((r ? i(r) : A.cwd) + e);
        else if ("/" === n) {
            var s = A.cwd.match(L);
            t = s ? s[0] + e.substring(1) : e
        } else t = A.base + e;
        return 0 === t.indexOf("//") && (t = location.protocol + t),
		t
    }
    function d(e, r) {
        if (!e) return "";
        e = o(e),
		e = u(e),
		e = c(e),
		e = s(e);
        var t = l(e, r);
        return t = f(t)
    }
    function v(e) {
        return e.hasAttribute ? e.src : e.getAttribute("src", 4)
    }
    function h(e, r, t, n) {
        var i = W.test(e),
		a = k.createElement(i ? "link" : "script");
        t && (a.charset = t),
		U(n) || a.setAttribute("crossorigin", n),
		p(a, r, i, e),
		i ? (a.rel = "stylesheet", a.href = e) : (a.async = !0, a.src = e),
		H = a,
		P ? M.insertBefore(a, P) : M.appendChild(a),
		H = null
    }
    function p(e, t, n, i) {
        function a() {
            e.onload = e.onerror = e.onreadystatechange = null,
			n || A.debug || M.removeChild(e),
			e = null,
			t()
        }
        var s = "onload" in e;
        return !n || !Y && s ? (s ? (e.onload = a, e.onerror = function () {
            O("error", {
                uri: i,
                node: e
            }),
			a()
        }) : e.onreadystatechange = function () {
            / loaded | complete /.test(e.readyState) && a()
        },
		r) : (setTimeout(function () {
		    g(e, t)
		},
		1), r)
    }
    function g(e, r) {
        var t, n = e.sheet;
        if (Y) n && (t = !0);
        else if (n) try {
            n.cssRules && (t = !0)
        } catch (e) {
            "NS_ERROR_DOM_SECURITY_ERR" === e.name && (t = !0)
        }
        setTimeout(function () {
            t ? r() : g(e, r)
        },
		20)
    }
    function E() {
        if (H) return H;
        if (K && "interactive" === K.readyState) return K;
        for (var e = M.getElementsByTagName("script"), r = e.length - 1; r >= 0; r--) {
            var t = e[r];
            if ("interactive" === t.readyState) return K = t
        }
    }
    function m(e) {
        var r = [];
        return e.replace(J, "").replace(z,
		function (e, t, n) {
		    n && r.push(n)
		}),
		r
    }
    function y(e, r) {
        this.uri = e,
		this.dependencies = r || [],
		this.exports = null,
		this.status = 0,
		this._waitings = {},
		this._remain = 0
    }
    if (!e.seajs) {
        var b = e.seajs = {
            version: "2.2.3"
        },
		A = b.data = {},
		_ = t("Object"),
		T = t("String"),
		D = Array.isArray || t("Array"),
		w = t("Function"),
		U = t("Undefined"),
		x = 0,
		N = A.events = {};
        b.on = function (e, r) {
            var t = N[e] || (N[e] = []);
            return t.push(r),
			b
        },
		b.off = function (e, r) {
		    if (!e && !r) return N = A.events = {},
			b;
		    var t = N[e];
		    if (t) if (r) for (var n = t.length - 1; n >= 0; n--) t[n] === r && t.splice(n, 1);
		    else delete N[e];
		    return b
		};
        var O = b.emit = function (e, r) {
            var t, n = N[e];
            if (n) for (n = n.slice() ; t = n.shift() ;) t(r);
            return b
        },
		S = /[^?#]*\//,
		q = /\/\.\//g,
		C = /\/[^\/]+\/\.\.\//,
		I = /([^:\/])\/\//g,
		j = /^([^\/:]+)(\/.+)$/,
		G = /{([^{]+)}/g,
		R = /^\/\/.|:\//,
		L = /^.*?\/\/.*?\//,
		k = document,
		X = i(k.URL),
		B = k.scripts,
		F = k.getElementById("seajsnode") || B[B.length - 1],
		V = i(v(F) || X);
        b.resolve = d;
        var H, K, M = k.head || k.getElementsByTagName("head")[0] || k.documentElement,
		P = M.getElementsByTagName("base")[0],
		W = /\.css(?:\?|$)/i,
		Y = +navigator.userAgent.replace(/.*(?:AppleWebKit|AndroidWebKit)\/(\d+).*/, "$1") < 536;
        b.request = h;
        var $, z = /"(?:\\"|[^"])*"|'(?:\\'|[^'])*'|\/\*[\S\s]*?\*\/|\/(?:\\\/|[^\/\r\n])+\/(?=[^\/])|\/\/.*|\.\s*require|(?:^|[^$])\brequire\s*\(\s*(["'])(.+?)\1\s*\)/g,
		J = /\\\\/g,
		Q = b.cache = {},
		Z = {},
		ee = {},
		re = {},
		te = y.STATUS = {
		    FETCHING: 1,
		    SAVED: 2,
		    LOADING: 3,
		    LOADED: 4,
		    EXECUTING: 5,
		    EXECUTED: 6
		};
        y.prototype.resolve = function () {
            for (var e = this,
			r = e.dependencies,
			t = [], n = 0, i = r.length; i > n; n++) t[n] = y.resolve(r[n], e.uri);
            return t
        },
		y.prototype.load = function () {
		    var e = this;
		    if (!(e.status >= te.LOADING)) {
		        e.status = te.LOADING;
		        var t = e.resolve();
		        O("load", t);
		        for (var n, i = e._remain = t.length,
				a = 0; i > a; a++) n = y.get(t[a]),
				n.status < te.LOADED ? n._waitings[e.uri] = (n._waitings[e.uri] || 0) + 1 : e._remain--;
		        if (0 === e._remain) return e.onload(),
				r;
		        var s = {};
		        for (a = 0; i > a; a++) n = Q[t[a]],
				n.status < te.FETCHING ? n.fetch(s) : n.status === te.SAVED && n.load();
		        for (var o in s) s.hasOwnProperty(o) && s[o]()
		    }
		},
		y.prototype.onload = function () {
		    var e = this;
		    e.status = te.LOADED,
			e.callback && e.callback();
		    var r, t, n = e._waitings;
		    for (r in n) n.hasOwnProperty(r) && (t = Q[r], t._remain -= n[r], 0 === t._remain && t.onload());
		    delete e._waitings,
			delete e._remain
		},
		y.prototype.fetch = function (e) {
		    function t() {
		        b.request(s.requestUri, s.onRequest, s.charset, s.crossorigin)
		    }
		    function n() {
		        delete Z[o],
				ee[o] = !0,
				$ && (y.save(a, $), $ = null);
		        var e, r = re[o];
		        for (delete re[o]; e = r.shift() ;) e.load()
		    }
		    var i = this,
			a = i.uri;
		    i.status = te.FETCHING;
		    var s = {
		        uri: a
		    };
		    O("fetch", s);
		    var o = s.requestUri || a;
		    return !o || ee[o] ? (i.load(), r) : Z[o] ? (re[o].push(i), r) : (Z[o] = !0, re[o] = [i], O("request", s = {
		        uri: a,
		        requestUri: o,
		        onRequest: n,
		        charset: w(A.charset) ? A.charset(o) : A.charset,
		        crossorigin: w(A.crossorigin) ? A.crossorigin(o) : A.crossorigin
		    }), s.requested || (e ? e[s.requestUri] = t : t()), r)
		},
		y.prototype.exec = function () {
		    function e(r) {
		        return y.get(e.resolve(r)).exec()
		    }
		    var t = this;
		    if (t.status >= te.EXECUTING) return t.exports;
		    t.status = te.EXECUTING;
		    var i = t.uri;
		    e.resolve = function (e) {
		        return y.resolve(e, i)
		    },
			e.async = function (r, t) {
			    return y.use(r, t, i + "_async_" + n()),
				e
			};
		    var a = t.factory,
			s = w(a) ? a(e, t.exports = {},
			t) : a;
		    return s === r && (s = t.exports),
			delete t.factory,
			t.exports = s,
			t.status = te.EXECUTED,
			O("exec", t),
			s
		},
		y.resolve = function (e, r) {
		    var t = {
		        id: e,
		        refUri: r
		    };
		    return O("resolve", t),
			t.uri || b.resolve(t.id, r)
		},
		y.define = function (e, t, n) {
		    var i = arguments.length;
		    1 === i ? (n = e, e = r) : 2 === i && (n = t, D(e) ? (t = e, e = r) : t = r),
			!D(t) && w(n) && (t = m("" + n));
		    var a = {
		        id: e,
		        uri: y.resolve(e),
		        deps: t,
		        factory: n
		    };
		    if (!a.uri && k.attachEvent) {
		        var s = E();
		        s && (a.uri = s.src)
		    }
		    O("define", a),
			a.uri ? y.save(a.uri, a) : $ = a
		},
		y.save = function (e, r) {
		    var t = y.get(e);
		    t.status < te.SAVED && (t.id = r.id || e, t.dependencies = r.deps || [], t.factory = r.factory, t.status = te.SAVED)
		},
		y.get = function (e, r) {
		    return Q[e] || (Q[e] = new y(e, r))
		},
		y.use = function (r, t, n) {
		    var i = y.get(n, D(r) ? r : [r]);
		    i.callback = function () {
		        for (var r = [], n = i.resolve(), a = 0, s = n.length; s > a; a++) r[a] = Q[n[a]].exec();
		        t && t.apply(e, r),
				delete i.callback
		    },
			i.load()
		},
		y.preload = function (e) {
		    var r = A.preload,
			t = r.length;
		    t ? y.use(r,
			function () {
			    r.splice(0, t),
				y.preload(e)
			},
			A.cwd + "_preload_" + n()) : e()
		},
		b.use = function (e, r) {
		    return y.preload(function () {
		        y.use(e, r, A.cwd + "_use_" + n())
		    }),
			b
		},
		y.define.cmd = {},
		e.define = y.define,
		b.Module = y,
		A.fetchedList = ee,
		A.cid = n,
		b.require = function (e) {
		    var r = y.get(y.resolve(e));
		    return r.status < te.EXECUTING && (r.onload(), r.exec()),
			r.exports
		};
        var ne = /^(.+?\/)(\?\?)?(seajs\/)+/;
        A.base = (V.match(ne) || ["", V])[1],
		A.dir = V,
		A.cwd = X,
		A.charset = "utf-8",
		A.preload = function () {
		    var e = [],
			r = location.search.replace(/(seajs-\w+)(&|$)/g, "$1=1$2");
		    return r += " " + k.cookie,
			r.replace(/(seajs-\w+)=1/g,
			function (r, t) {
			    e.push(t)
			}),
			e
		}(),
		b.config = function (e) {
		    for (var r in e) {
		        var t = e[r],
				n = A[r];
		        if (n && _(n)) for (var i in t) n[i] = t[i];
		        else D(n) ? t = n.concat(t) : "base" === r && ("/" !== t.slice(-1) && (t += "/"), t = l(t)),
				A[r] = t
		    }
		    return O("config", e),
			b
		}
    }
}(this);
function _ToolManCoordinate(t, e, n) {
    this.factory = t,
	this.x = isNaN(e) ? 0 : e,
	this.y = isNaN(n) ? 0 : n
}
function _ToolManDragGroup(t, e) {
    this.factory = t,
	this.element = e,
	this._handle = null,
	this._thresholdDistance = 0,
	this._transforms = new Array,
	this._listeners = new Array,
	this._listeners.draginit = new Array,
	this._listeners.dragstart = new Array,
	this._listeners.dragmove = new Array,
	this._listeners.dragend = new Array
}
function _ToolManDragEvent(t, e, n) {
    this.type = t,
	this.group = n,
	this.mousePosition = ToolMan.coordinates().mousePosition(e),
	this.mouseOffset = ToolMan.coordinates().mouseOffset(e),
	this.transformedMouseOffset = this.mouseOffset,
	this.topLeftPosition = ToolMan.coordinates().topLeftPosition(n.element),
	this.topLeftOffset = ToolMan.coordinates().topLeftOffset(n.element)
}
var ToolMan = {
    events: function () {
        if (!ToolMan._eventsFactory) throw "ToolMan Events module isn't loaded";
        return ToolMan._eventsFactory
    },
    css: function () {
        if (!ToolMan._cssFactory) throw "ToolMan CSS module isn't loaded";
        return ToolMan._cssFactory
    },
    coordinates: function () {
        if (!ToolMan._coordinatesFactory) throw "ToolMan Coordinates module isn't loaded";
        return ToolMan._coordinatesFactory
    },
    drag: function () {
        if (!ToolMan._dragFactory) throw "ToolMan Drag module isn't loaded";
        return ToolMan._dragFactory
    },
    dragsort: function () {
        if (!ToolMan._dragsortFactory) throw "ToolMan DragSort module isn't loaded";
        return ToolMan._dragsortFactory
    },
    helpers: function () {
        return ToolMan._helpers
    },
    cookies: function () {
        if (!ToolMan._cookieOven) throw "ToolMan Cookie module isn't loaded";
        return ToolMan._cookieOven
    },
    junkdrawer: function () {
        return ToolMan._junkdrawer
    }
};
ToolMan._helpers = {
    map: function (t, e) {
        for (var n = 0,
		o = t.length; n < o; n++) e(t[n])
    },
    nextItem: function (t, e) {
        if (null != t) {
            for (var n = t.nextSibling; null != n;) {
                if (n.nodeName == e) return n;
                n = n.nextSibling
            }
            return null
        }
    },
    previousItem: function (t, e) {
        for (var n = t.previousSibling; null != n;) {
            if (n.nodeName == e) return n;
            n = n.previousSibling
        }
        return null
    },
    moveBefore: function (t, e) {
        var n = t.parentNode;
        n.removeChild(t),
		n.insertBefore(t, e)
    },
    moveAfter: function (t, e) {
        var n = t.parentNode;
        n.removeChild(t),
		n.insertBefore(t, e ? e.nextSibling : null)
    }
},
ToolMan._junkdrawer = {
    serializeList: function (t) {
        for (var e = t.getElementsByTagName("li"), n = new Array, o = 0, r = e.length; o < r; o++) {
            var i = e[o];
            n.push(ToolMan.junkdrawer()._identifier(i))
        }
        return n.join("|")
    },
    inspectListOrder: function (t) {
        alert(ToolMan.junkdrawer().serializeList(document.getElementById(t)))
    },
    restoreListOrder: function (t) {
        var e = document.getElementById(t);
        if (null != e) {
            var n = ToolMan.cookies().get("list-" + t);
            if (n) for (var o = n.split("|"), r = ToolMan.junkdrawer()._itemsByID(e), i = 0, s = o.length; i < s; i++) {
                var a = o[i];
                if (a in r) {
                    var l = r[a];
                    e.removeChild(l),
					e.insertBefore(l, null)
                }
            }
        }
    },
    _identifier: function (t) {
        var e, n = ToolMan.junkdrawer().trim;
        return e = n(t.getAttribute("id")),
		null != e && e.length > 0 ? e : (e = n(t.getAttribute("itemID")), null != e && e.length > 0 ? e : n(t.innerHTML))
    },
    _itemsByID: function (t) {
        for (var e = new Array,
		n = t.getElementsByTagName("li"), o = 0, r = n.length; o < r; o++) {
            var i = n[o];
            e[ToolMan.junkdrawer()._identifier(i)] = i
        }
        return e
    },
    trim: function (t) {
        return null == t ? null : t.replace(/^(\s+)?(.*\S)(\s+)?$/, "$2")
    }
},
ToolMan._eventsFactory = {
    fix: function (t) {
        return t || (t = window.event),
		t.target ? 3 == t.target.nodeType && (t.target = t.target.parentNode) : t.srcElement && (t.target = t.srcElement),
		t
    },
    register: function (t, e, n) {
        if (t.addEventListener) t.addEventListener(e, n, !1);
        else if (t.attachEvent) {
            t._listeners || (t._listeners = new Array),
			t._listeners[e] || (t._listeners[e] = new Array);
            var o = function () {
                n.apply(t, new Array)
            };
            t._listeners[e][n] = o,
			t.attachEvent("on" + e, o)
        }
    },
    unregister: function (t, e, n) {
        t.removeEventListener ? t.removeEventListener(e, n, !1) : t.detachEvent && t._listeners && t._listeners[e] && t._listeners[e][n] && t.detachEvent("on" + e, t._listeners[e][n])
    }
},
ToolMan._cssFactory = {
    readStyle: function (t, e) {
        if (t.style[e]) return t.style[e];
        if (t.currentStyle) return t.currentStyle[e];
        if (document.defaultView && document.defaultView.getComputedStyle) {
            var n = document.defaultView.getComputedStyle(t, null);
            return n.getPropertyValue(e)
        }
        return null
    }
},
ToolMan._coordinatesFactory = {
    create: function (t, e) {
        return new _ToolManCoordinate(this, t, e)
    },
    origin: function () {
        return this.create(0, 0)
    },
    topLeftPosition: function (t) {
        var e = parseInt(ToolMan.css().readStyle(t, "left")),
		e = isNaN(e) ? 0 : e,
		n = parseInt(ToolMan.css().readStyle(t, "top")),
		n = isNaN(n) ? 0 : n;
        return this.create(e, n)
    },
    bottomRightPosition: function (t) {
        return this.topLeftPosition(t).plus(this._size(t))
    },
    topLeftOffset: function (t) {
        for (var e = this._offset(t), n = t.offsetParent; n;) e = e.plus(this._offset(n)),
		n = n.offsetParent;
        return e
    },
    bottomRightOffset: function (t) {
        return this.topLeftOffset(t).plus(this.create(t.offsetWidth, t.offsetHeight))
    },
    scrollOffset: function () {
        return window.pageXOffset ? this.create(window.pageXOffset, window.pageYOffset) : document.documentElement ? this.create(document.body.scrollLeft + document.documentElement.scrollLeft, document.body.scrollTop + document.documentElement.scrollTop) : document.body.scrollLeft >= 0 ? this.create(document.body.scrollLeft, document.body.scrollTop) : this.create(0, 0)
    },
    clientSize: function () {
        return window.innerHeight >= 0 ? this.create(window.innerWidth, window.innerHeight) : document.documentElement ? this.create(document.documentElement.clientWidth, document.documentElement.clientHeight) : document.body.clientHeight >= 0 ? this.create(document.body.clientWidth, document.body.clientHeight) : this.create(0, 0)
    },
    mousePosition: function (t) {
        return t = ToolMan.events().fix(t),
		this.create(t.clientX, t.clientY)
    },
    mouseOffset: function (t) {
        return t = ToolMan.events().fix(t),
		t.pageX >= 0 || t.pageX < 0 ? this.create(t.pageX, t.pageY) : t.clientX >= 0 || t.clientX < 0 ? this.mousePosition(t).plus(this.scrollOffset()) : void 0
    },
    _size: function (t) {
        return this.create(t.offsetWidth, t.offsetHeight)
    },
    _offset: function (t) {
        return this.create(t.offsetLeft, t.offsetTop)
    }
},
_ToolManCoordinate.prototype = {
    toString: function () {
        return "(" + this.x + "," + this.y + ")"
    },
    plus: function (t) {
        return this.factory.create(this.x + t.x, this.y + t.y)
    },
    minus: function (t) {
        return this.factory.create(this.x - t.x, this.y - t.y)
    },
    min: function (t) {
        return this.factory.create(Math.min(this.x, t.x), Math.min(this.y, t.y))
    },
    max: function (t) {
        return this.factory.create(Math.max(this.x, t.x), Math.max(this.y, t.y))
    },
    constrainTo: function (t, e) {
        var n = t.min(e),
		o = t.max(e);
        return this.max(n).min(o)
    },
    distance: function (t) {
        return Math.sqrt(Math.pow(this.x - t.x, 2) + Math.pow(this.y - t.y, 2))
    },
    reposition: function (t) {
        t.style.top = this.y + "px",
		t.style.left = this.x + "px"
    }
},
ToolMan._dragFactory = {
    createSimpleGroup: function (t, e) {
        e = e ? e : t;
        var n = this.createGroup(t);
        return n.setHandle(e),
		n.transparentDrag(),
		n.onTopWhileDragging(),
		n
    },
    createGroup: function (t) {
        var e = new _ToolManDragGroup(this, t),
		n = ToolMan.css().readStyle(t, "position");
        return "static" == n ? t.style.position = "relative" : "absolute" == n && ToolMan.coordinates().topLeftOffset(t).reposition(t),
		e.register("draginit", this._showDragEventStatus),
		e.register("dragmove", this._showDragEventStatus),
		e.register("dragend", this._showDragEventStatus),
		e
    },
    _showDragEventStatus: function (t) {
        window.status = t.toString()
    },
    constraints: function () {
        return this._constraintFactory
    },
    _createEvent: function (t, e, n) {
        return new _ToolManDragEvent(t, e, n)
    }
},
_ToolManDragGroup.prototype = {
    setHandle: function (t) {
        var e = ToolMan.events();
        t.toolManDragGroup = this,
		e.register(t, "mousedown", this._dragInit),
		t.onmousedown = function () {
		    return !1
		},
		this.element != t && e.unregister(this.element, "mousedown", this._dragInit)
    },
    register: function (t, e) {
        this._listeners[t].push(e)
    },
    addTransform: function (t) {
        this._transforms.push(t)
    },
    verticalOnly: function () {
        this.addTransform(this.factory.constraints().vertical())
    },
    horizontalOnly: function () {
        this.addTransform(this.factory.constraints().horizontal())
    },
    setThreshold: function (t) {
        this._thresholdDistance = t
    },
    transparentDrag: function (t) {
        var t = "undefined" != typeof t ? t : .75,
		e = ToolMan.css().readStyle(this.element, "opacity");
        this.register("dragstart",
		function (e) {
		    var n = e.group.element;
		    n.style.opacity = t,
			n.style.filter = "alpha(opacity=" + 100 * t + ")"
		}),
		this.register("dragend",
		function (t) {
		    var n = t.group.element;
		    n.style.opacity = e,
			n.style.filter = "alpha(opacity=100)"
		})
    },
    onTopWhileDragging: function (t) {
        var t = "undefined" != typeof t ? t : 1e5,
		e = ToolMan.css().readStyle(this.element, "z-index");
        this.register("dragstart",
		function (e) {
		    e.group.element.style.zIndex = t
		}),
		this.register("dragend",
		function (t) {
		    t.group.element.style.zIndex = e
		})
    },
    _dragInit: function (t) {
        t = ToolMan.events().fix(t);
        var e = document.toolManDragGroup = this.toolManDragGroup,
		n = e.factory._createEvent("draginit", t, e);
        e._isThresholdExceeded = !1,
		e._initialMouseOffset = n.mouseOffset,
		e._grabOffset = n.mouseOffset.minus(n.topLeftOffset),
		ToolMan.events().register(document, "mousemove", e._drag),
		document.onmousemove = function () {
		    return !1
		},
		ToolMan.events().register(document, "mouseup", e._dragEnd),
		e._notifyListeners(n)
    },
    _drag: function (t) {
        t = ToolMan.events().fix(t);
        var e = ToolMan.coordinates(),
		n = this.toolManDragGroup;
        if (n) {
            var o = n.factory._createEvent("dragmove", t, n),
			r = o.mouseOffset.minus(n._grabOffset);
            if (!n._isThresholdExceeded) {
                var s = o.mouseOffset.distance(n._initialMouseOffset);
                if (s < n._thresholdDistance) return;
                n._isThresholdExceeded = !0,
				n._notifyListeners(n.factory._createEvent("dragstart", t, n))
            }
            for (i in n._transforms) {
                var a = n._transforms[i];
                r = a(r, o)
            }
            var l = r.minus(o.topLeftOffset),
			u = o.topLeftPosition.plus(l);
            u.reposition(n.element),
			o.transformedMouseOffset = r.plus(n._grabOffset),
			n._notifyListeners(o);
            var f = r.minus(e.topLeftOffset(n.element));
            0 == f.x && 0 == f.y || e.topLeftPosition(n.element).plus(f).reposition(n.element)
        }
    },
    _dragEnd: function (t) {
        t = ToolMan.events().fix(t);
        var e = this.toolManDragGroup,
		n = e.factory._createEvent("dragend", t, e);
        e._notifyListeners(n),
		this.toolManDragGroup = null,
		ToolMan.events().unregister(document, "mousemove", e._drag),
		document.onmousemove = null,
		ToolMan.events().unregister(document, "mouseup", e._dragEnd)
    },
    _notifyListeners: function (t) {
        var e = this._listeners[t.type];
        for (i in e) e[i](t)
    }
},
_ToolManDragEvent.prototype = {
    toString: function () {
        return "mouse: " + this.mousePosition + this.mouseOffset + "    xmouse: " + this.transformedMouseOffset + "    left,top: " + this.topLeftPosition + this.topLeftOffset
    }
},
ToolMan._dragFactory._constraintFactory = {
    vertical: function () {
        return function (t, e) {
            var n = e.topLeftOffset.x;
            return t.x != n ? t.factory.create(n, t.y) : t
        }
    },
    horizontal: function () {
        return function (t, e) {
            var n = e.topLeftOffset.y;
            return t.y != n ? t.factory.create(t.x, n) : t
        }
    }
},
ToolMan._dragsortFactory = {
    makeSortable: function (t) {
        var e = ToolMan.drag().createSimpleGroup(t);
        return e.register("dragstart", this._onDragStart),
		e.register("dragmove", this._onDragMove),
		e.register("dragend", this._onDragEnd),
		e
    },
    makeListSortable: function (t) {
        var e = ToolMan.helpers(),
		n = ToolMan.coordinates(),
		o = t.getElementsByTagName("li");
        e.map(o,
		function (e) {
		    var o = dragsort.makeSortable(e);
		    o.setThreshold(4);
		    var r, i;
		    o.addTransform(function (t, e) {
		        return t.constrainTo(r, i)
		    }),
			o.register("dragstart",
			function () {
			    var e = t.getElementsByTagName("li");
			    r = i = n.topLeftOffset(e[0]);
			    for (var o = 1,
				s = e.length; o < s; o++) {
			        var a = n.topLeftOffset(e[o]);
			        r = r.min(a),
					i = i.max(a)
			    }
			})
		});
        for (var r = 1,
		i = arguments.length; r < i; r++) e.map(o, arguments[r])
    },
    _onDragStart: function (t) { },
    _onDragMove: function (t) {
        for (var e = ToolMan.helpers(), n = ToolMan.coordinates(), o = t.group.element, r = t.transformedMouseOffset, i = null, s = e.previousItem(o, o.nodeName) ; null != s;) {
            var a = n.bottomRightOffset(s);
            r.y <= a.y && r.x <= a.x && (i = s),
			s = e.previousItem(s, o.nodeName)
        }
        if (null != i) return void e.moveBefore(o, i);
        for (var l = e.nextItem(o, o.nodeName) ; null != l;) {
            var u = n.topLeftOffset(l);
            u.y <= r.y && u.x <= r.x && (i = l),
			l = e.nextItem(l, o.nodeName)
        }
        return null != i ? void e.moveBefore(o, e.nextItem(i, o.nodeName)) : void 0
    },
    _onDragEnd: function (t) {
        ToolMan.coordinates().create(0, 0).reposition(t.group.element)
    }
},
ToolMan._cookieOven = {
    set: function (t, e, n) {
        if (n) {
            var o = new Date;
            o.setTime(o.getTime() + 24 * n * 60 * 60 * 1e3);
            var r = "; expires=" + o.toGMTString()
        } else var r = "";
        document.cookie = t + "=" + e + r + "; path=/"
    },
    get: function (t) {
        for (var e = t + "=",
		n = document.cookie.split(";"), o = 0, r = n.length; o < r; o++) {
            for (var i = n[o];
			" " == i.charAt(0) ;) i = i.substring(1, i.length);
            if (0 == i.indexOf(e)) return i.substring(e.length, i.length)
        }
        return null
    },
    eraseCookie: function (t) {
        createCookie(t, "", -1)
    }
}; !
function (e, t) {
    function n(e) {
        var t = e.length,
		n = ue.type(e);
        return !ue.isWindow(e) && (!(1 !== e.nodeType || !t) || ("array" === n || "function" !== n && (0 === t || "number" == typeof t && t > 0 && t - 1 in e)))
    }
    function r(e) {
        var t = Ne[e] = {};
        return ue.each(e.match(ce) || [],
		function (e, n) {
		    t[n] = !0
		}),
		t
    }
    function i(e, n, r, i) {
        if (ue.acceptData(e)) {
            var o, a, s = ue.expando,
			u = "string" == typeof n,
			l = e.nodeType,
			c = l ? ue.cache : e,
			f = l ? e[s] : e[s] && s;
            if (f && c[f] && (i || c[f].data) || !u || r !== t) return f || (l ? e[s] = f = Z.pop() || ue.guid++ : f = s),
			c[f] || (c[f] = {},
			l || (c[f].toJSON = ue.noop)),
			("object" == typeof n || "function" == typeof n) && (i ? c[f] = ue.extend(c[f], n) : c[f].data = ue.extend(c[f].data, n)),
			o = c[f],
			i || (o.data || (o.data = {}), o = o.data),
			r !== t && (o[ue.camelCase(n)] = r),
			u ? (a = o[n], null == a && (a = o[ue.camelCase(n)])) : a = o,
			a
        }
    }
    function o(e, t, n) {
        if (ue.acceptData(e)) {
            var r, i, o, a = e.nodeType,
			s = a ? ue.cache : e,
			u = a ? e[ue.expando] : ue.expando;
            if (s[u]) {
                if (t && (o = n ? s[u] : s[u].data)) {
                    ue.isArray(t) ? t = t.concat(ue.map(t, ue.camelCase)) : t in o ? t = [t] : (t = ue.camelCase(t), t = t in o ? [t] : t.split(" "));
                    for (r = 0, i = t.length; i > r; r++) delete o[t[r]];
                    if (!(n ? $ : ue.isEmptyObject)(o)) return
                } (n || (delete s[u].data, $(s[u]))) && (a ? ue.cleanData([e], !0) : ue.support.deleteExpando || s != s.window ? delete s[u] : s[u] = null)
            }
        }
    }
    function a(e, n, r) {
        if (r === t && 1 === e.nodeType) {
            var i = "data-" + n.replace(ke, "-$1").toLowerCase();
            if (r = e.getAttribute(i), "string" == typeof r) {
                try {
                    r = "true" === r || "false" !== r && ("null" === r ? null : +r + "" === r ? +r : Ce.test(r) ? ue.parseJSON(r) : r)
                } catch (e) { }
                ue.data(e, n, r)
            } else r = t
        }
        return r
    }
    function $(e) {
        var t;
        for (t in e) if (("data" !== t || !ue.isEmptyObject(e[t])) && "toJSON" !== t) return !1;
        return !0
    }
    function s() {
        return !0
    }
    function u() {
        return !1
    }
    function l(e, t) {
        do e = e[t];
        while (e && 1 !== e.nodeType);
        return e
    }
    function c(e, t, n) {
        if (t = t || 0, ue.isFunction(t)) return ue.grep(e,
		function (e, r) {
		    var i = !!t.call(e, r, e);
		    return i === n
		});
        if (t.nodeType) return ue.grep(e,
		function (e) {
		    return e === t === n
		});
        if ("string" == typeof t) {
            var r = ue.grep(e,
			function (e) {
			    return 1 === e.nodeType
			});
            if (Ie.test(t)) return ue.filter(t, r, !n);
            t = ue.filter(t, r)
        }
        return ue.grep(e,
		function (e) {
		    return ue.inArray(e, t) >= 0 === n
		})
    }
    function f(e) {
        var t = Xe.split("|"),
		n = e.createDocumentFragment();
        if (n.createElement) for (; t.length;) n.createElement(t.pop());
        return n
    }
    function p(e, t) {
        return e.getElementsByTagName(t)[0] || e.appendChild(e.ownerDocument.createElement(t))
    }
    function d(e) {
        var t = e.getAttributeNode("type");
        return e.type = (t && t.specified) + "/" + e.type,
		e
    }
    function h(e) {
        var t = it.exec(e.type);
        return t ? e.type = t[1] : e.removeAttribute("type"),
		e
    }
    function g(e, t) {
        for (var n, r = 0; null != (n = e[r]) ; r++) ue._data(n, "globalEval", !t || ue._data(t[r], "globalEval"))
    }
    function m(e, t) {
        if (1 === t.nodeType && ue.hasData(e)) {
            var n, r, i, o = ue._data(e),
			a = ue._data(t, o),
			s = o.events;
            if (s) {
                delete a.handle,
				a.events = {};
                for (n in s) for (r = 0, i = s[n].length; i > r; r++) ue.event.add(t, n, s[n][r])
            }
            a.data && (a.data = ue.extend({},
			a.data))
        }
    }
    function y(e, t) {
        var n, r, i;
        if (1 === t.nodeType) {
            if (n = t.nodeName.toLowerCase(), !ue.support.noCloneEvent && t[ue.expando]) {
                i = ue._data(t);
                for (r in i.events) ue.removeEvent(t, r, i.handle);
                t.removeAttribute(ue.expando)
            }
            "script" === n && t.text !== e.text ? (d(t).text = e.text, h(t)) : "object" === n ? (t.parentNode && (t.outerHTML = e.outerHTML), ue.support.html5Clone && e.innerHTML && !ue.trim(t.innerHTML) && (t.innerHTML = e.innerHTML)) : "input" === n && tt.test(e.type) ? (t.defaultChecked = t.checked = e.checked, t.value !== e.value && (t.value = e.value)) : "option" === n ? t.defaultSelected = t.selected = e.defaultSelected : ("input" === n || "textarea" === n) && (t.defaultValue = e.defaultValue)
        }
    }
    function v(e, n) {
        var r, i, o = 0,
		a = typeof e.getElementsByTagName !== U ? e.getElementsByTagName(n || "*") : typeof e.querySelectorAll !== U ? e.querySelectorAll(n || "*") : t;
        if (!a) for (a = [], r = e.childNodes || e; null != (i = r[o]) ; o++) !n || ue.nodeName(i, n) ? a.push(i) : ue.merge(a, v(i, n));
        return n === t || n && ue.nodeName(e, n) ? ue.merge([e], a) : a
    }
    function b(e) {
        tt.test(e.type) && (e.defaultChecked = e.checked)
    }
    function x(e, t) {
        if (t in e) return t;
        for (var n = t.charAt(0).toUpperCase() + t.slice(1), r = t, i = Ct.length; i--;) if (t = Ct[i] + n, t in e) return t;
        return r
    }
    function w(e, t) {
        return e = t || e,
		"none" === ue.css(e, "display") || !ue.contains(e.ownerDocument, e)
    }
    function T(e, t) {
        for (var n, r, i, o = [], a = 0, s = e.length; s > a; a++) r = e[a],
		r.style && (o[a] = ue._data(r, "olddisplay"), n = r.style.display, t ? (o[a] || "none" !== n || (r.style.display = ""), "" === r.style.display && w(r) && (o[a] = ue._data(r, "olddisplay", E(r.nodeName)))) : o[a] || (i = w(r), (n && "none" !== n || !i) && ue._data(r, "olddisplay", i ? n : ue.css(r, "display"))));
        for (a = 0; s > a; a++) r = e[a],
		r.style && (t && "none" !== r.style.display && "" !== r.style.display || (r.style.display = t ? o[a] || "" : "none"));
        return e
    }
    function N(e, t, n) {
        var r = yt.exec(t);
        return r ? Math.max(0, r[1] - (n || 0)) + (r[2] || "px") : t
    }
    function C(e, t, n, r, i) {
        for (var o = n === (r ? "border" : "content") ? 4 : "width" === t ? 1 : 0, a = 0; 4 > o; o += 2) "margin" === n && (a += ue.css(e, n + Nt[o], !0, i)),
		r ? ("content" === n && (a -= ue.css(e, "padding" + Nt[o], !0, i)), "margin" !== n && (a -= ue.css(e, "border" + Nt[o] + "Width", !0, i))) : (a += ue.css(e, "padding" + Nt[o], !0, i), "padding" !== n && (a += ue.css(e, "border" + Nt[o] + "Width", !0, i)));
        return a
    }
    function k(e, t, n) {
        var r = !0,
		i = "width" === t ? e.offsetWidth : e.offsetHeight,
		o = ct(e),
		a = ue.support.boxSizing && "border-box" === ue.css(e, "boxSizing", !1, o);
        if (0 >= i || null == i) {
            if (i = ft(e, t, o), (0 > i || null == i) && (i = e.style[t]), vt.test(i)) return i;
            r = a && (ue.support.boxSizingReliable || i === e.style[t]),
			i = parseFloat(i) || 0
        }
        return i + C(e, t, n || (a ? "border" : "content"), r, o) + "px"
    }
    function E(e) {
        var t = Y,
		n = xt[e];
        return n || (n = S(e, t), "none" !== n && n || (lt = (lt || ue("<iframe frameborder='0' width='0' height='0'/>").css("cssText", "display:block !important")).appendTo(t.documentElement), t = (lt[0].contentWindow || lt[0].contentDocument).document, t.write("<!doctype html><html><body>"), t.close(), n = S(e, t), lt.detach()), xt[e] = n),
		n
    }
    function S(e, t) {
        var n = ue(t.createElement(e)).appendTo(t.body),
		r = ue.css(n[0], "display");
        return n.remove(),
		r
    }
    function A(e, t, n, r) {
        var i;
        if (ue.isArray(t)) ue.each(t,
		function (t, i) {
		    n || Et.test(e) ? r(e, i) : A(e + "[" + ("object" == typeof i ? t : "") + "]", i, n, r)
		});
        else if (n || "object" !== ue.type(t)) r(e, t);
        else for (i in t) A(e + "[" + i + "]", t[i], n, r)
    }
    function j(e) {
        return function (t, n) {
            "string" != typeof t && (n = t, t = "*");
            var r, i = 0,
			o = t.toLowerCase().match(ce) || [];
            if (ue.isFunction(n)) for (; r = o[i++];) "+" === r[0] ? (r = r.slice(1) || "*", (e[r] = e[r] || []).unshift(n)) : (e[r] = e[r] || []).push(n)
        }
    }
    function D(e, n, r, i) {
        function o(u) {
            var l;
            return a[u] = !0,
			ue.each(e[u] || [],
			function (e, u) {
			    var c = u(n, r, i);
			    return "string" != typeof c || s || a[c] ? s ? !(l = c) : t : (n.dataTypes.unshift(c), o(c), !1)
			}),
			l
        }
        var a = {},
		s = e === It;
        return o(n.dataTypes[0]) || !a["*"] && o("*")
    }
    function L(e, n) {
        var r, i, o = ue.ajaxSettings.flatOptions || {};
        for (i in n) n[i] !== t && ((o[i] ? e : r || (r = {}))[i] = n[i]);
        return r && ue.extend(!0, e, r),
		e
    }
    function H(e, n, r) {
        var i, o, a, s, u = e.contents,
		l = e.dataTypes,
		c = e.responseFields;
        for (s in c) s in r && (n[c[s]] = r[s]);
        for (;
		"*" === l[0];) l.shift(),
		o === t && (o = e.mimeType || n.getResponseHeader("Content-Type"));
        if (o) for (s in u) if (u[s] && u[s].test(o)) {
            l.unshift(s);
            break
        }
        if (l[0] in r) a = l[0];
        else {
            for (s in r) {
                if (!l[0] || e.converters[s + " " + l[0]]) {
                    a = s;
                    break
                }
                i || (i = s)
            }
            a = a || i
        }
        return a ? (a !== l[0] && l.unshift(a), r[a]) : t
    }
    function O(e, t) {
        var n, r, i, o, a = {},
		s = 0,
		u = e.dataTypes.slice(),
		l = u[0];
        if (e.dataFilter && (t = e.dataFilter(t, e.dataType)), u[1]) for (i in e.converters) a[i.toLowerCase()] = e.converters[i];
        for (; r = u[++s];) if ("*" !== r) {
            if ("*" !== l && l !== r) {
                if (i = a[l + " " + r] || a["* " + r], !i) for (n in a) if (o = n.split(" "), o[1] === r && (i = a[l + " " + o[0]] || a["* " + o[0]])) {
                    i === !0 ? i = a[n] : a[n] !== !0 && (r = o[0], u.splice(s--, 0, r));
                    break
                }
                if (i !== !0) if (i && e.throws) t = i(t);
                else try {
                    t = i(t)
                } catch (e) {
                    return {
                        state: "parsererror",
                        error: i ? e : "No conversion from " + l + " to " + r
                    }
                }
            }
            l = r
        }
        return {
            state: "success",
            data: t
        }
    }
    function M() {
        try {
            return new e.XMLHttpRequest
        } catch (e) { }
    }
    function q() {
        try {
            return new e.ActiveXObject("Microsoft.XMLHTTP")
        } catch (e) { }
    }
    function F() {
        return setTimeout(function () {
            Gt = t
        }),
		Gt = ue.now()
    }
    function _(e, t) {
        ue.each(t,
		function (t, n) {
		    for (var r = (rn[t] || []).concat(rn["*"]), i = 0, o = r.length; o > i; i++) if (r[i].call(e, t, n)) return
		})
    }
    function B(e, t, n) {
        var r, i, o = 0,
		a = nn.length,
		s = ue.Deferred().always(function () {
		    delete u.elem
		}),
		u = function () {
		    if (i) return !1;
		    for (var t = Gt || F(), n = Math.max(0, l.startTime + l.duration - t), r = n / l.duration || 0, o = 1 - r, a = 0, u = l.tweens.length; u > a; a++) l.tweens[a].run(o);
		    return s.notifyWith(e, [l, o, n]),
			1 > o && u ? n : (s.resolveWith(e, [l]), !1)
		},
		l = s.promise({
		    elem: e,
		    props: ue.extend({},
			t),
		    opts: ue.extend(!0, {
		        specialEasing: {}
		    },
			n),
		    originalProperties: t,
		    originalOptions: n,
		    startTime: Gt || F(),
		    duration: n.duration,
		    tweens: [],
		    createTween: function (t, n) {
		        var r = ue.Tween(e, l.opts, t, n, l.opts.specialEasing[t] || l.opts.easing);
		        return l.tweens.push(r),
				r
		    },
		    stop: function (t) {
		        var n = 0,
				r = t ? l.tweens.length : 0;
		        if (i) return this;
		        for (i = !0; r > n; n++) l.tweens[n].run(1);
		        return t ? s.resolveWith(e, [l, t]) : s.rejectWith(e, [l, t]),
				this
		    }
		}),
		c = l.props;
        for (P(c, l.opts.specialEasing) ; a > o; o++) if (r = nn[o].call(l, e, c, l.opts)) return r;
        return _(l, c),
		ue.isFunction(l.opts.start) && l.opts.start.call(e, l),
		ue.fx.timer(ue.extend(u, {
		    elem: e,
		    anim: l,
		    queue: l.opts.queue
		})),
		l.progress(l.opts.progress).done(l.opts.done, l.opts.complete).fail(l.opts.fail).always(l.opts.always)
    }
    function P(e, t) {
        var n, r, i, o, a;
        for (i in e) if (r = ue.camelCase(i), o = t[r], n = e[i], ue.isArray(n) && (o = n[1], n = e[i] = n[0]), i !== r && (e[r] = n, delete e[i]), a = ue.cssHooks[r], a && "expand" in a) {
            n = a.expand(n),
			delete e[r];
            for (i in n) i in e || (e[i] = n[i], t[i] = o)
        } else t[r] = o
    }
    function R(e, t, n) {
        var r, i, o, a, s, u, l, c, f, p = this,
		d = e.style,
		h = {},
		g = [],
		m = e.nodeType && w(e);
        n.queue || (c = ue._queueHooks(e, "fx"), null == c.unqueued && (c.unqueued = 0, f = c.empty.fire, c.empty.fire = function () {
            c.unqueued || f()
        }), c.unqueued++, p.always(function () {
            p.always(function () {
                c.unqueued--,
				ue.queue(e, "fx").length || c.empty.fire()
            })
        })),
		1 === e.nodeType && ("height" in t || "width" in t) && (n.overflow = [d.overflow, d.overflowX, d.overflowY], "inline" === ue.css(e, "display") && "none" === ue.css(e, "float") && (ue.support.inlineBlockNeedsLayout && "inline" !== E(e.nodeName) ? d.zoom = 1 : d.display = "inline-block")),
		n.overflow && (d.overflow = "hidden", ue.support.shrinkWrapBlocks || p.always(function () {
		    d.overflow = n.overflow[0],
			d.overflowX = n.overflow[1],
			d.overflowY = n.overflow[2]
		}));
        for (i in t) if (a = t[i], Zt.exec(a)) {
            if (delete t[i], u = u || "toggle" === a, a === (m ? "hide" : "show")) continue;
            g.push(i)
        }
        if (o = g.length) {
            s = ue._data(e, "fxshow") || ue._data(e, "fxshow", {}),
			"hidden" in s && (m = s.hidden),
			u && (s.hidden = !m),
			m ? ue(e).show() : p.done(function () {
			    ue(e).hide()
			}),
			p.done(function () {
			    var t;
			    ue._removeData(e, "fxshow");
			    for (t in h) ue.style(e, t, h[t])
			});
            for (i = 0; o > i; i++) r = g[i],
			l = p.createTween(r, m ? s[r] : 0),
			h[r] = s[r] || ue.style(e, r),
			r in s || (s[r] = l.start, m && (l.end = l.start, l.start = "width" === r || "height" === r ? 1 : 0))
        }
    }
    function W(e, t, n, r, i) {
        return new W.prototype.init(e, t, n, r, i)
    }
    function I(e, t) {
        var n, r = {
            height: e
        },
		i = 0;
        for (t = t ? 1 : 0; 4 > i; i += 2 - t) n = Nt[i],
		r["margin" + n] = r["padding" + n] = e;
        return t && (r.opacity = r.width = e),
		r
    }
    function z(e) {
        return ue.isWindow(e) ? e : 9 === e.nodeType && (e.defaultView || e.parentWindow)
    }
    var V, X, U = typeof t,
	Y = e.document,
	J = e.location,
	Q = e.jQuery,
	G = e.$,
	K = {},
	Z = [],
	ee = "1.9.1",
	te = Z.concat,
	ne = Z.push,
	re = Z.slice,
	ie = Z.indexOf,
	oe = K.toString,
	ae = K.hasOwnProperty,
	se = ee.trim,
	ue = function (e, t) {
	    return new ue.fn.init(e, t, X)
	},
	le = /[+-]?(?:\d*\.|)\d+(?:[eE][+-]?\d+|)/.source,
	ce = /\S+/g,
	fe = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g,
	pe = /^(?:(<[\w\W]+>)[^>]*|#([\w-]*))$/,
	de = /^<(\w+)\s*\/?>(?:<\/\1>|)$/,
	he = /^[\],:{}\s]*$/,
	ge = /(?:^|:|,)(?:\s*\[)+/g,
	me = /\\(?:["\\\/bfnrt]|u[\da-fA-F]{4})/g,
	ye = /"[^"\\\r\n]*"|true|false|null|-?(?:\d+\.|)\d+(?:[eE][+-]?\d+|)/g,
	ve = /^-ms-/,
	be = /-([\da-z])/gi,
	xe = function (e, t) {
	    return t.toUpperCase()
	},
	we = function (e) {
	    (Y.addEventListener || "load" === e.type || "complete" === Y.readyState) && (Te(), ue.ready())
	},
	Te = function () {
	    Y.addEventListener ? (Y.removeEventListener("DOMContentLoaded", we, !1), e.removeEventListener("load", we, !1)) : (Y.detachEvent("onreadystatechange", we), e.detachEvent("onload", we))
	};
    ue.fn = ue.prototype = {
        jquery: ee,
        constructor: ue,
        init: function (e, n, r) {
            var i, o;
            if (!e) return this;
            if ("string" == typeof e) {
                if (i = "<" === e.charAt(0) && ">" === e.charAt(e.length - 1) && e.length >= 3 ? [null, e, null] : pe.exec(e), !i || !i[1] && n) return !n || n.jquery ? (n || r).find(e) : this.constructor(n).find(e);
                if (i[1]) {
                    if (n = n instanceof ue ? n[0] : n, ue.merge(this, ue.parseHTML(i[1], n && n.nodeType ? n.ownerDocument || n : Y, !0)), de.test(i[1]) && ue.isPlainObject(n)) for (i in n) ue.isFunction(this[i]) ? this[i](n[i]) : this.attr(i, n[i]);
                    return this
                }
                if (o = Y.getElementById(i[2]), o && o.parentNode) {
                    if (o.id !== i[2]) return r.find(e);
                    this.length = 1,
					this[0] = o
                }
                return this.context = Y,
				this.selector = e,
				this
            }
            return e.nodeType ? (this.context = this[0] = e, this.length = 1, this) : ue.isFunction(e) ? r.ready(e) : (e.selector !== t && (this.selector = e.selector, this.context = e.context), ue.makeArray(e, this))
        },
        selector: "",
        length: 0,
        size: function () {
            return this.length
        },
        toArray: function () {
            return re.call(this)
        },
        get: function (e) {
            return null == e ? this.toArray() : 0 > e ? this[this.length + e] : this[e]
        },
        pushStack: function (e) {
            var t = ue.merge(this.constructor(), e);
            return t.prevObject = this,
			t.context = this.context,
			t
        },
        each: function (e, t) {
            return ue.each(this, e, t)
        },
        ready: function (e) {
            return ue.ready.promise().done(e),
			this
        },
        slice: function () {
            return this.pushStack(re.apply(this, arguments))
        },
        first: function () {
            return this.eq(0)
        },
        last: function () {
            return this.eq(-1)
        },
        eq: function (e) {
            var t = this.length,
			n = +e + (0 > e ? t : 0);
            return this.pushStack(n >= 0 && t > n ? [this[n]] : [])
        },
        map: function (e) {
            return this.pushStack(ue.map(this,
			function (t, n) {
			    return e.call(t, n, t)
			}))
        },
        end: function () {
            return this.prevObject || this.constructor(null)
        },
        push: ne,
        sort: [].sort,
        splice: [].splice
    },
	ue.fn.init.prototype = ue.fn,
	ue.extend = ue.fn.extend = function () {
	    var e, n, r, i, o, a, s = arguments[0] || {},
		u = 1,
		l = arguments.length,
		c = !1;
	    for ("boolean" == typeof s && (c = s, s = arguments[1] || {},
		u = 2), "object" == typeof s || ue.isFunction(s) || (s = {}), l === u && (s = this, --u) ; l > u; u++) if (null != (o = arguments[u])) for (i in o) e = s[i],
		r = o[i],
		s !== r && (c && r && (ue.isPlainObject(r) || (n = ue.isArray(r))) ? (n ? (n = !1, a = e && ue.isArray(e) ? e : []) : a = e && ue.isPlainObject(e) ? e : {},
		s[i] = ue.extend(c, a, r)) : r !== t && (s[i] = r));
	    return s
	},
	ue.extend({
	    noConflict: function (t) {
	        return e.$ === ue && (e.$ = G),
			t && e.jQuery === ue && (e.jQuery = Q),
			ue
	    },
	    isReady: !1,
	    readyWait: 1,
	    holdReady: function (e) {
	        e ? ue.readyWait++ : ue.ready(!0)
	    },
	    ready: function (e) {
	        if (e === !0 ? !--ue.readyWait : !ue.isReady) {
	            if (!Y.body) return setTimeout(ue.ready);
	            ue.isReady = !0,
				e !== !0 && --ue.readyWait > 0 || (V.resolveWith(Y, [ue]), ue.fn.trigger && ue(Y).trigger("ready").off("ready"))
	        }
	    },
	    isFunction: function (e) {
	        return "function" === ue.type(e)
	    },
	    isArray: Array.isArray ||
		function (e) {
		    return "array" === ue.type(e)
		},
	    isWindow: function (e) {
	        return null != e && e == e.window
	    },
	    isNumeric: function (e) {
	        return !isNaN(parseFloat(e)) && isFinite(e)
	    },
	    type: function (e) {
	        return null == e ? e + "" : "object" == typeof e || "function" == typeof e ? K[oe.call(e)] || "object" : typeof e
	    },
	    isPlainObject: function (e) {
	        if (!e || "object" !== ue.type(e) || e.nodeType || ue.isWindow(e)) return !1;
	        try {
	            if (e.constructor && !ae.call(e, "constructor") && !ae.call(e.constructor.prototype, "isPrototypeOf")) return !1
	        } catch (e) {
	            return !1
	        }
	        var n;
	        for (n in e);
	        return n === t || ae.call(e, n)
	    },
	    isEmptyObject: function (e) {
	        var t;
	        for (t in e) return !1;
	        return !0
	    },
	    error: function (e) {
	        throw Error(e)
	    },
	    parseHTML: function (e, t, n) {
	        if (!e || "string" != typeof e) return null;
	        "boolean" == typeof t && (n = t, t = !1),
			t = t || Y;
	        var r = de.exec(e),
			i = !n && [];
	        return r ? [t.createElement(r[1])] : (r = ue.buildFragment([e], t, i), i && ue(i).remove(), ue.merge([], r.childNodes))
	    },
	    parseJSON: function (n) {
	        return e.JSON && e.JSON.parse ? e.JSON.parse(n) : null === n ? n : "string" == typeof n && (n = ue.trim(n), n && he.test(n.replace(me, "@").replace(ye, "]").replace(ge, ""))) ? Function("return " + n)() : (ue.error("Invalid JSON: " + n), t)
	    },
	    parseXML: function (n) {
	        var r, i;
	        if (!n || "string" != typeof n) return null;
	        try {
	            e.DOMParser ? (i = new DOMParser, r = i.parseFromString(n, "text/xml")) : (r = new ActiveXObject("Microsoft.XMLDOM"), r.async = "false", r.loadXML(n))
	        } catch (e) {
	            r = t
	        }
	        return r && r.documentElement && !r.getElementsByTagName("parsererror").length || ue.error("Invalid XML: " + n),
			r
	    },
	    noop: function () { },
	    globalEval: function (t) {
	        t && ue.trim(t) && (e.execScript ||
			function (t) {
			    e.eval.call(e, t)
			})(t)
	    },
	    camelCase: function (e) {
	        return e.replace(ve, "ms-").replace(be, xe)
	    },
	    nodeName: function (e, t) {
	        return e.nodeName && e.nodeName.toLowerCase() === t.toLowerCase()
	    },
	    each: function (e, t, r) {
	        var i, o = 0,
			a = e.length,
			s = n(e);
	        if (r) {
	            if (s) for (; a > o && (i = t.apply(e[o], r), i !== !1) ; o++);
	            else for (o in e) if (i = t.apply(e[o], r), i === !1) break
	        } else if (s) for (; a > o && (i = t.call(e[o], o, e[o]), i !== !1) ; o++);
	        else for (o in e) if (i = t.call(e[o], o, e[o]), i === !1) break;
	        return e
	    },
	    trim: se && !se.call("\ufeff ") ?
		function (e) {
		    return null == e ? "" : se.call(e)
		} : function (e) {
		    return null == e ? "" : (e + "").replace(fe, "")
		},
	    makeArray: function (e, t) {
	        var r = t || [];
	        return null != e && (n(Object(e)) ? ue.merge(r, "string" == typeof e ? [e] : e) : ne.call(r, e)),
			r
	    },
	    inArray: function (e, t, n) {
	        var r;
	        if (t) {
	            if (ie) return ie.call(t, e, n);
	            for (r = t.length, n = n ? 0 > n ? Math.max(0, r + n) : n : 0; r > n; n++) if (n in t && t[n] === e) return n
	        }
	        return -1
	    },
	    merge: function (e, n) {
	        var r = n.length,
			i = e.length,
			o = 0;
	        if ("number" == typeof r) for (; r > o; o++) e[i++] = n[o];
	        else for (; n[o] !== t;) e[i++] = n[o++];
	        return e.length = i,
			e
	    },
	    grep: function (e, t, n) {
	        var r, i = [],
			o = 0,
			a = e.length;
	        for (n = !!n; a > o; o++) r = !!t(e[o], o),
			n !== r && i.push(e[o]);
	        return i
	    },
	    map: function (e, t, r) {
	        var i, o = 0,
			a = e.length,
			s = n(e),
			u = [];
	        if (s) for (; a > o; o++) i = t(e[o], o, r),
			null != i && (u[u.length] = i);
	        else for (o in e) i = t(e[o], o, r),
			null != i && (u[u.length] = i);
	        return te.apply([], u)
	    },
	    guid: 1,
	    proxy: function (e, n) {
	        var r, i, o;
	        return "string" == typeof n && (o = e[n], n = e, e = o),
			ue.isFunction(e) ? (r = re.call(arguments, 2), i = function () {
			    return e.apply(n || this, r.concat(re.call(arguments)))
			},
			i.guid = e.guid = e.guid || ue.guid++, i) : t
	    },
	    access: function (e, n, r, i, o, a, s) {
	        var u = 0,
			l = e.length,
			c = null == r;
	        if ("object" === ue.type(r)) {
	            o = !0;
	            for (u in r) ue.access(e, n, u, r[u], !0, a, s)
	        } else if (i !== t && (o = !0, ue.isFunction(i) || (s = !0), c && (s ? (n.call(e, i), n = null) : (c = n, n = function (e, t, n) {
				return c.call(ue(e), n)
	        })), n)) for (; l > u; u++) n(e[u], r, s ? i : i.call(e[u], u, n(e[u], r)));
	        return o ? e : c ? n.call(e) : l ? n(e[0], r) : a
	    },
	    now: function () {
	        return (new Date).getTime()
	    }
	}),
	ue.ready.promise = function (t) {
	    if (!V) if (V = ue.Deferred(), "complete" === Y.readyState) setTimeout(ue.ready);
	    else if (Y.addEventListener) Y.addEventListener("DOMContentLoaded", we, !1),
		e.addEventListener("load", we, !1);
	    else {
	        Y.attachEvent("onreadystatechange", we),
			e.attachEvent("onload", we);
	        var n = !1;
	        try {
	            n = null == e.frameElement && Y.documentElement
	        } catch (e) { }
	        n && n.doScroll &&
			function e() {
			    if (!ue.isReady) {
			        try {
			            n.doScroll("left")
			        } catch (t) {
			            return setTimeout(e, 50)
			        }
			        Te(),
					ue.ready()
			    }
			}()
	    }
	    return V.promise(t)
	},
	ue.each("Boolean Number String Function Array Date RegExp Object Error".split(" "),
	function (e, t) {
	    K["[object " + t + "]"] = t.toLowerCase()
	}),
	X = ue(Y);
    var Ne = {};
    ue.Callbacks = function (e) {
        e = "string" == typeof e ? Ne[e] || r(e) : ue.extend({},
		e);
        var n, i, o, a, s, u, l = [],
		c = !e.once && [],
		f = function (t) {
		    for (i = e.memory && t, o = !0, s = u || 0, u = 0, a = l.length, n = !0; l && a > s; s++) if (l[s].apply(t[0], t[1]) === !1 && e.stopOnFalse) {
		        i = !1;
		        break
		    }
		    n = !1,
			l && (c ? c.length && f(c.shift()) : i ? l = [] : p.disable())
		},
		p = {
		    add: function () {
		        if (l) {
		            var t = l.length; !
					function t(n) {
					    ue.each(n,
						function (n, r) {
						    var i = ue.type(r);
						    "function" === i ? e.unique && p.has(r) || l.push(r) : r && r.length && "string" !== i && t(r)
						})
					}(arguments),
					n ? a = l.length : i && (u = t, f(i))
		        }
		        return this
		    },
		    remove: function () {
		        return l && ue.each(arguments,
				function (e, t) {
				    for (var r; (r = ue.inArray(t, l, r)) > -1;) l.splice(r, 1),
					n && (a >= r && a--, s >= r && s--)
				}),
				this
		    },
		    has: function (e) {
		        return e ? ue.inArray(e, l) > -1 : !(!l || !l.length)
		    },
		    empty: function () {
		        return l = [],
				this
		    },
		    disable: function () {
		        return l = c = i = t,
				this
		    },
		    disabled: function () {
		        return !l
		    },
		    lock: function () {
		        return c = t,
				i || p.disable(),
				this
		    },
		    locked: function () {
		        return !c
		    },
		    fireWith: function (e, t) {
		        return t = t || [],
				t = [e, t.slice ? t.slice() : t],
				!l || o && !c || (n ? c.push(t) : f(t)),
				this
		    },
		    fire: function () {
		        return p.fireWith(this, arguments),
				this
		    },
		    fired: function () {
		        return !!o
		    }
		};
        return p
    },
	ue.extend({
	    Deferred: function (e) {
	        var t = [["resolve", "done", ue.Callbacks("once memory"), "resolved"], ["reject", "fail", ue.Callbacks("once memory"), "rejected"], ["notify", "progress", ue.Callbacks("memory")]],
			n = "pending",
			r = {
			    state: function () {
			        return n
			    },
			    always: function () {
			        return i.done(arguments).fail(arguments),
					this
			    },
			    then: function () {
			        var e = arguments;
			        return ue.Deferred(function (n) {
			            ue.each(t,
						function (t, o) {
						    var a = o[0],
							s = ue.isFunction(e[t]) && e[t];
						    i[o[1]](function () {
						        var e = s && s.apply(this, arguments);
						        e && ue.isFunction(e.promise) ? e.promise().done(n.resolve).fail(n.reject).progress(n.notify) : n[a + "With"](this === r ? n.promise() : this, s ? [e] : arguments)
						    })
						}),
						e = null
			        }).promise()
			    },
			    promise: function (e) {
			        return null != e ? ue.extend(e, r) : r
			    }
			},
			i = {};
	        return r.pipe = r.then,
			ue.each(t,
			function (e, o) {
			    var a = o[2],
				s = o[3];
			    r[o[1]] = a.add,
				s && a.add(function () {
				    n = s
				},
				t[1 ^ e][2].disable, t[2][2].lock),
				i[o[0]] = function () {
				    return i[o[0] + "With"](this === i ? r : this, arguments),
					this
				},
				i[o[0] + "With"] = a.fireWith
			}),
			r.promise(i),
			e && e.call(i, i),
			i
	    },
	    when: function (e) {
	        var t, n, r, i = 0,
			o = re.call(arguments),
			a = o.length,
			s = 1 !== a || e && ue.isFunction(e.promise) ? a : 0,
			u = 1 === s ? e : ue.Deferred(),
			l = function (e, n, r) {
			    return function (i) {
			        n[e] = this,
					r[e] = arguments.length > 1 ? re.call(arguments) : i,
					r === t ? u.notifyWith(n, r) : --s || u.resolveWith(n, r)
			    }
			};
	        if (a > 1) for (t = Array(a), n = Array(a), r = Array(a) ; a > i; i++) o[i] && ue.isFunction(o[i].promise) ? o[i].promise().done(l(i, r, o)).fail(u.reject).progress(l(i, n, t)) : --s;
	        return s || u.resolveWith(r, o),
			u.promise()
	    }
	}),
	ue.support = function () {
	    var t, n, r, i, o, a, s, u, l, c, f = Y.createElement("div");
	    if (f.setAttribute("className", "t"), f.innerHTML = "  <link/><table></table><a href='/a'>a</a><input type='checkbox'/>", n = f.getElementsByTagName("*"), r = f.getElementsByTagName("a")[0], !n || !r || !n.length) return {};
	    o = Y.createElement("select"),
		s = o.appendChild(Y.createElement("option")),
		i = f.getElementsByTagName("input")[0],
		r.style.cssText = "top:1px;float:left;opacity:.5",
		t = {
		    getSetAttribute: "t" !== f.className,
		    leadingWhitespace: 3 === f.firstChild.nodeType,
		    tbody: !f.getElementsByTagName("tbody").length,
		    htmlSerialize: !!f.getElementsByTagName("link").length,
		    style: /top/.test(r.getAttribute("style")),
		    hrefNormalized: "/a" === r.getAttribute("href"),
		    opacity: /^0.5/.test(r.style.opacity),
		    cssFloat: !!r.style.cssFloat,
		    checkOn: !!i.value,
		    optSelected: s.selected,
		    enctype: !!Y.createElement("form").enctype,
		    html5Clone: "<:nav></:nav>" !== Y.createElement("nav").cloneNode(!0).outerHTML,
		    boxModel: "CSS1Compat" === Y.compatMode,
		    deleteExpando: !0,
		    noCloneEvent: !0,
		    inlineBlockNeedsLayout: !1,
		    shrinkWrapBlocks: !1,
		    reliableMarginRight: !0,
		    boxSizingReliable: !0,
		    pixelPosition: !1
		},
		i.checked = !0,
		t.noCloneChecked = i.cloneNode(!0).checked,
		o.disabled = !0,
		t.optDisabled = !s.disabled;
	    try {
	        delete f.test
	    } catch (e) {
	        t.deleteExpando = !1
	    }
	    i = Y.createElement("input"),
		i.setAttribute("value", ""),
		t.input = "" === i.getAttribute("value"),
		i.value = "t",
		i.setAttribute("type", "radio"),
		t.radioValue = "t" === i.value,
		i.setAttribute("checked", "t"),
		i.setAttribute("name", "t"),
		a = Y.createDocumentFragment(),
		a.appendChild(i),
		t.appendChecked = i.checked,
		t.checkClone = a.cloneNode(!0).cloneNode(!0).lastChild.checked,
		f.attachEvent && (f.attachEvent("onclick",
		function () {
		    t.noCloneEvent = !1
		}), f.cloneNode(!0).click());
	    for (c in {
	        submit: !0,
	        change: !0,
	        focusin: !0
	    }) f.setAttribute(u = "on" + c, "t"),
		t[c + "Bubbles"] = u in e || f.attributes[u].expando === !1;
	    return f.style.backgroundClip = "content-box",
		f.cloneNode(!0).style.backgroundClip = "",
		t.clearCloneStyle = "content-box" === f.style.backgroundClip,
		ue(function () {
		    var n, r, i, o = "padding:0;margin:0;border:0;display:block;box-sizing:content-box;-moz-box-sizing:content-box;-webkit-box-sizing:content-box;",
			a = Y.getElementsByTagName("body")[0];
		    a && (n = Y.createElement("div"), n.style.cssText = "border:0;width:0;height:0;position:absolute;top:0;left:-9999px;margin-top:1px", a.appendChild(n).appendChild(f), f.innerHTML = "<table><tr><td></td><td>t</td></tr></table>", i = f.getElementsByTagName("td"), i[0].style.cssText = "padding:0;margin:0;border:0;display:none", l = 0 === i[0].offsetHeight, i[0].style.display = "", i[1].style.display = "none", t.reliableHiddenOffsets = l && 0 === i[0].offsetHeight, f.innerHTML = "", f.style.cssText = "box-sizing:border-box;-moz-box-sizing:border-box;-webkit-box-sizing:border-box;padding:1px;border:1px;display:block;width:4px;margin-top:1%;position:absolute;top:1%;", t.boxSizing = 4 === f.offsetWidth, t.doesNotIncludeMarginInBodyOffset = 1 !== a.offsetTop, e.getComputedStyle && (t.pixelPosition = "1%" !== (e.getComputedStyle(f, null) || {}).top, t.boxSizingReliable = "4px" === (e.getComputedStyle(f, null) || {
		        width: "4px"
		    }).width, r = f.appendChild(Y.createElement("div")), r.style.cssText = f.style.cssText = o, r.style.marginRight = r.style.width = "0", f.style.width = "1px", t.reliableMarginRight = !parseFloat((e.getComputedStyle(r, null) || {}).marginRight)), typeof f.style.zoom !== U && (f.innerHTML = "", f.style.cssText = o + "width:1px;padding:1px;display:inline;zoom:1", t.inlineBlockNeedsLayout = 3 === f.offsetWidth, f.style.display = "block", f.innerHTML = "<div></div>", f.firstChild.style.width = "5px", t.shrinkWrapBlocks = 3 !== f.offsetWidth, t.inlineBlockNeedsLayout && (a.style.zoom = 1)), a.removeChild(n), n = f = i = r = null)
		}),
		n = o = a = s = r = i = null,
		t
	}();
    var Ce = /(?:\{[\s\S]*\}|\[[\s\S]*\])$/,
	ke = /([A-Z])/g;
    ue.extend({
        cache: {},
        expando: "jQuery" + (ee + Math.random()).replace(/\D/g, ""),
        noData: {
            embed: !0,
            object: "clsid:D27CDB6E-AE6D-11cf-96B8-444553540000",
            applet: !0
        },
        hasData: function (e) {
            return e = e.nodeType ? ue.cache[e[ue.expando]] : e[ue.expando],
			!!e && !$(e)
        },
        data: function (e, t, n) {
            return i(e, t, n)
        },
        removeData: function (e, t) {
            return o(e, t)
        },
        _data: function (e, t, n) {
            return i(e, t, n, !0)
        },
        _removeData: function (e, t) {
            return o(e, t, !0)
        },
        acceptData: function (e) {
            if (e.nodeType && 1 !== e.nodeType && 9 !== e.nodeType) return !1;
            var t = e.nodeName && ue.noData[e.nodeName.toLowerCase()];
            return !t || t !== !0 && e.getAttribute("classid") === t
        }
    }),
	ue.fn.extend({
	    data: function (e, n) {
	        var r, i, o = this[0],
			s = 0,
			u = null;
	        if (e === t) {
	            if (this.length && (u = ue.data(o), 1 === o.nodeType && !ue._data(o, "parsedAttrs"))) {
	                for (r = o.attributes; r.length > s; s++) i = r[s].name,
					i.indexOf("data-") || (i = ue.camelCase(i.slice(5)), a(o, i, u[i]));
	                ue._data(o, "parsedAttrs", !0)
	            }
	            return u
	        }
	        return "object" == typeof e ? this.each(function () {
	            ue.data(this, e)
	        }) : ue.access(this,
			function (n) {
			    return n === t ? o ? a(o, e, ue.data(o, e)) : null : (this.each(function () {
			        ue.data(this, e, n)
			    }), t)
			},
			null, n, arguments.length > 1, null, !0)
	    },
	    removeData: function (e) {
	        return this.each(function () {
	            ue.removeData(this, e)
	        })
	    }
	}),
	ue.extend({
	    queue: function (e, n, r) {
	        var i;
	        return e ? (n = (n || "fx") + "queue", i = ue._data(e, n), r && (!i || ue.isArray(r) ? i = ue._data(e, n, ue.makeArray(r)) : i.push(r)), i || []) : t
	    },
	    dequeue: function (e, t) {
	        t = t || "fx";
	        var n = ue.queue(e, t),
			r = n.length,
			i = n.shift(),
			o = ue._queueHooks(e, t),
			a = function () {
			    ue.dequeue(e, t)
			};
	        "inprogress" === i && (i = n.shift(), r--),
			o.cur = i,
			i && ("fx" === t && n.unshift("inprogress"), delete o.stop, i.call(e, a, o)),
			!r && o && o.empty.fire()
	    },
	    _queueHooks: function (e, t) {
	        var n = t + "queueHooks";
	        return ue._data(e, n) || ue._data(e, n, {
	            empty: ue.Callbacks("once memory").add(function () {
	                ue._removeData(e, t + "queue"),
					ue._removeData(e, n)
	            })
	        })
	    }
	}),
	ue.fn.extend({
	    queue: function (e, n) {
	        var r = 2;
	        return "string" != typeof e && (n = e, e = "fx", r--),
			r > arguments.length ? ue.queue(this[0], e) : n === t ? this : this.each(function () {
			    var t = ue.queue(this, e, n);
			    ue._queueHooks(this, e),
				"fx" === e && "inprogress" !== t[0] && ue.dequeue(this, e)
			})
	    },
	    dequeue: function (e) {
	        return this.each(function () {
	            ue.dequeue(this, e)
	        })
	    },
	    delay: function (e, t) {
	        return e = ue.fx ? ue.fx.speeds[e] || e : e,
			t = t || "fx",
			this.queue(t,
			function (t, n) {
			    var r = setTimeout(t, e);
			    n.stop = function () {
			        clearTimeout(r)
			    }
			})
	    },
	    clearQueue: function (e) {
	        return this.queue(e || "fx", [])
	    },
	    promise: function (e, n) {
	        var r, i = 1,
			o = ue.Deferred(),
			a = this,
			s = this.length,
			u = function () {
			    --i || o.resolveWith(a, [a])
			};
	        for ("string" != typeof e && (n = e, e = t), e = e || "fx"; s--;) r = ue._data(a[s], e + "queueHooks"),
			r && r.empty && (i++, r.empty.add(u));
	        return u(),
			o.promise(n)
	    }
	});
    var Ee, Se, Ae = /[\t\r\n]/g,
	je = /\r/g,
	De = /^(?:input|select|textarea|button|object)$/i,
	Le = /^(?:a|area)$/i,
	He = /^(?:checked|selected|autofocus|autoplay|async|controls|defer|disabled|hidden|loop|multiple|open|readonly|required|scoped)$/i,
	Oe = /^(?:checked|selected)$/i,
	Me = ue.support.getSetAttribute,
	qe = ue.support.input;
    ue.fn.extend({
        attr: function (e, t) {
            return ue.access(this, ue.attr, e, t, arguments.length > 1)
        },
        removeAttr: function (e) {
            return this.each(function () {
                ue.removeAttr(this, e)
            })
        },
        prop: function (e, t) {
            return ue.access(this, ue.prop, e, t, arguments.length > 1)
        },
        removeProp: function (e) {
            return e = ue.propFix[e] || e,
			this.each(function () {
			    try {
			        this[e] = t,
					delete this[e]
			    } catch (e) { }
			})
        },
        addClass: function (e) {
            var t, n, r, i, o, a = 0,
			s = this.length,
			u = "string" == typeof e && e;
            if (ue.isFunction(e)) return this.each(function (t) {
                ue(this).addClass(e.call(this, t, this.className))
            });
            if (u) for (t = (e || "").match(ce) || []; s > a; a++) if (n = this[a], r = 1 === n.nodeType && (n.className ? (" " + n.className + " ").replace(Ae, " ") : " ")) {
                for (o = 0; i = t[o++];) 0 > r.indexOf(" " + i + " ") && (r += i + " ");
                n.className = ue.trim(r)
            }
            return this
        },
        removeClass: function (e) {
            var t, n, r, i, o, a = 0,
			s = this.length,
			u = 0 === arguments.length || "string" == typeof e && e;
            if (ue.isFunction(e)) return this.each(function (t) {
                ue(this).removeClass(e.call(this, t, this.className))
            });
            if (u) for (t = (e || "").match(ce) || []; s > a; a++) if (n = this[a], r = 1 === n.nodeType && (n.className ? (" " + n.className + " ").replace(Ae, " ") : "")) {
                for (o = 0; i = t[o++];) for (; r.indexOf(" " + i + " ") >= 0;) r = r.replace(" " + i + " ", " ");
                n.className = e ? ue.trim(r) : ""
            }
            return this
        },
        toggleClass: function (e, t) {
            var n = typeof e,
			r = "boolean" == typeof t;
            return ue.isFunction(e) ? this.each(function (n) {
                ue(this).toggleClass(e.call(this, n, this.className, t), t)
            }) : this.each(function () {
                if ("string" === n) for (var i, o = 0,
				a = ue(this), s = t, u = e.match(ce) || []; i = u[o++];) s = r ? s : !a.hasClass(i),
				a[s ? "addClass" : "removeClass"](i);
                else (n === U || "boolean" === n) && (this.className && ue._data(this, "__className__", this.className), this.className = this.className || e === !1 ? "" : ue._data(this, "__className__") || "")
            })
        },
        hasClass: function (e) {
            for (var t = " " + e + " ",
			n = 0,
			r = this.length; r > n; n++) if (1 === this[n].nodeType && (" " + this[n].className + " ").replace(Ae, " ").indexOf(t) >= 0) return !0;
            return !1
        },
        val: function (e) {
            var n, r, i, o = this[0];
            return arguments.length ? (i = ue.isFunction(e), this.each(function (n) {
                var o, a = ue(this);
                1 === this.nodeType && (o = i ? e.call(this, n, a.val()) : e, null == o ? o = "" : "number" == typeof o ? o += "" : ue.isArray(o) && (o = ue.map(o,
				function (e) {
				    return null == e ? "" : e + ""
				})), r = ue.valHooks[this.type] || ue.valHooks[this.nodeName.toLowerCase()], r && "set" in r && r.set(this, o, "value") !== t || (this.value = o))
            })) : o ? (r = ue.valHooks[o.type] || ue.valHooks[o.nodeName.toLowerCase()], r && "get" in r && (n = r.get(o, "value")) !== t ? n : (n = o.value, "string" == typeof n ? n.replace(je, "") : null == n ? "" : n)) : void 0
        }
    }),
	ue.extend({
	    valHooks: {
	        option: {
	            get: function (e) {
	                var t = e.attributes.value;
	                return !t || t.specified ? e.value : e.text
	            }
	        },
	        select: {
	            get: function (e) {
	                for (var t, n, r = e.options,
					i = e.selectedIndex,
					o = "select-one" === e.type || 0 > i,
					a = o ? null : [], s = o ? i + 1 : r.length, u = 0 > i ? s : o ? i : 0; s > u; u++) if (n = r[u], !(!n.selected && u !== i || (ue.support.optDisabled ? n.disabled : null !== n.getAttribute("disabled")) || n.parentNode.disabled && ue.nodeName(n.parentNode, "optgroup"))) {
					    if (t = ue(n).val(), o) return t;
					    a.push(t)
					}
	                return a
	            },
	            set: function (e, t) {
	                var n = ue.makeArray(t);
	                return ue(e).find("option").each(function () {
	                    this.selected = ue.inArray(ue(this).val(), n) >= 0
	                }),
					n.length || (e.selectedIndex = -1),
					n
	            }
	        }
	    },
	    attr: function (e, n, r) {
	        var i, o, a, s = e.nodeType;
	        if (e && 3 !== s && 8 !== s && 2 !== s) return typeof e.getAttribute === U ? ue.prop(e, n, r) : (o = 1 !== s || !ue.isXMLDoc(e), o && (n = n.toLowerCase(), i = ue.attrHooks[n] || (He.test(n) ? Se : Ee)), r === t ? i && o && "get" in i && null !== (a = i.get(e, n)) ? a : (typeof e.getAttribute !== U && (a = e.getAttribute(n)), null == a ? t : a) : null !== r ? i && o && "set" in i && (a = i.set(e, r, n)) !== t ? a : (e.setAttribute(n, r + ""), r) : (ue.removeAttr(e, n), t))
	    },
	    removeAttr: function (e, t) {
	        var n, r, i = 0,
			o = t && t.match(ce);
	        if (o && 1 === e.nodeType) for (; n = o[i++];) r = ue.propFix[n] || n,
			He.test(n) ? !Me && Oe.test(n) ? e[ue.camelCase("default-" + n)] = e[r] = !1 : e[r] = !1 : ue.attr(e, n, ""),
			e.removeAttribute(Me ? n : r)
	    },
	    attrHooks: {
	        type: {
	            set: function (e, t) {
	                if (!ue.support.radioValue && "radio" === t && ue.nodeName(e, "input")) {
	                    var n = e.value;
	                    return e.setAttribute("type", t),
						n && (e.value = n),
						t
	                }
	            }
	        }
	    },
	    propFix: {
	        tabindex: "tabIndex",
	        readonly: "readOnly",
	        for: "htmlFor",
	        class: "className",
	        maxlength: "maxLength",
	        cellspacing: "cellSpacing",
	        cellpadding: "cellPadding",
	        rowspan: "rowSpan",
	        colspan: "colSpan",
	        usemap: "useMap",
	        frameborder: "frameBorder",
	        contenteditable: "contentEditable"
	    },
	    prop: function (e, n, r) {
	        var i, o, a, s = e.nodeType;
	        if (e && 3 !== s && 8 !== s && 2 !== s) return a = 1 !== s || !ue.isXMLDoc(e),
			a && (n = ue.propFix[n] || n, o = ue.propHooks[n]),
			r !== t ? o && "set" in o && (i = o.set(e, r, n)) !== t ? i : e[n] = r : o && "get" in o && null !== (i = o.get(e, n)) ? i : e[n]
	    },
	    propHooks: {
	        tabIndex: {
	            get: function (e) {
	                var n = e.getAttributeNode("tabindex");
	                return n && n.specified ? parseInt(n.value, 10) : De.test(e.nodeName) || Le.test(e.nodeName) && e.href ? 0 : t
	            }
	        }
	    }
	}),
	Se = {
	    get: function (e, n) {
	        var r = ue.prop(e, n),
			i = "boolean" == typeof r && e.getAttribute(n),
			o = "boolean" == typeof r ? qe && Me ? null != i : Oe.test(n) ? e[ue.camelCase("default-" + n)] : !!i : e.getAttributeNode(n);
	        return o && o.value !== !1 ? n.toLowerCase() : t
	    },
	    set: function (e, t, n) {
	        return t === !1 ? ue.removeAttr(e, n) : qe && Me || !Oe.test(n) ? e.setAttribute(!Me && ue.propFix[n] || n, n) : e[ue.camelCase("default-" + n)] = e[n] = !0,
			n
	    }
	},
	qe && Me || (ue.attrHooks.value = {
	    get: function (e, n) {
	        var r = e.getAttributeNode(n);
	        return ue.nodeName(e, "input") ? e.defaultValue : r && r.specified ? r.value : t
	    },
	    set: function (e, n, r) {
	        return ue.nodeName(e, "input") ? (e.defaultValue = n, t) : Ee && Ee.set(e, n, r)
	    }
	}),
	Me || (Ee = ue.valHooks.button = {
	    get: function (e, n) {
	        var r = e.getAttributeNode(n);
	        return r && ("id" === n || "name" === n || "coords" === n ? "" !== r.value : r.specified) ? r.value : t
	    },
	    set: function (e, n, r) {
	        var i = e.getAttributeNode(r);
	        return i || e.setAttributeNode(i = e.ownerDocument.createAttribute(r)),
			i.value = n += "",
			"value" === r || n === e.getAttribute(r) ? n : t
	    }
	},
	ue.attrHooks.contenteditable = {
	    get: Ee.get,
	    set: function (e, t, n) {
	        Ee.set(e, "" !== t && t, n)
	    }
	},
	ue.each(["width", "height"],
	function (e, n) {
	    ue.attrHooks[n] = ue.extend(ue.attrHooks[n], {
	        set: function (e, r) {
	            return "" === r ? (e.setAttribute(n, "auto"), r) : t
	        }
	    })
	})),
	ue.support.hrefNormalized || (ue.each(["href", "src", "width", "height"],
	function (e, n) {
	    ue.attrHooks[n] = ue.extend(ue.attrHooks[n], {
	        get: function (e) {
	            var r = e.getAttribute(n, 2);
	            return null == r ? t : r
	        }
	    })
	}), ue.each(["href", "src"],
	function (e, t) {
	    ue.propHooks[t] = {
	        get: function (e) {
	            return e.getAttribute(t, 4)
	        }
	    }
	})),
	ue.support.style || (ue.attrHooks.style = {
	    get: function (e) {
	        return e.style.cssText || t
	    },
	    set: function (e, t) {
	        return e.style.cssText = t + ""
	    }
	}),
	ue.support.optSelected || (ue.propHooks.selected = ue.extend(ue.propHooks.selected, {
	    get: function (e) {
	        var t = e.parentNode;
	        return t && (t.selectedIndex, t.parentNode && t.parentNode.selectedIndex),
			null
	    }
	})),
	ue.support.enctype || (ue.propFix.enctype = "encoding"),
	ue.support.checkOn || ue.each(["radio", "checkbox"],
	function () {
	    ue.valHooks[this] = {
	        get: function (e) {
	            return null === e.getAttribute("value") ? "on" : e.value
	        }
	    }
	}),
	ue.each(["radio", "checkbox"],
	function () {
	    ue.valHooks[this] = ue.extend(ue.valHooks[this], {
	        set: function (e, n) {
	            return ue.isArray(n) ? e.checked = ue.inArray(ue(e).val(), n) >= 0 : t
	        }
	    })
	});
    var Fe = /^(?:input|select|textarea)$/i,
	_e = /^key/,
	Be = /^(?:mouse|contextmenu)|click/,
	Pe = /^(?:focusinfocus|focusoutblur)$/,
	Re = /^([^.]*)(?:\.(.+)|)$/;
    ue.event = {
        global: {},
        add: function (e, n, r, i, o) {
            var a, s, u, l, c, f, p, d, h, g, m, y = ue._data(e);
            if (y) {
                for (r.handler && (l = r, r = l.handler, o = l.selector), r.guid || (r.guid = ue.guid++), (s = y.events) || (s = y.events = {}), (f = y.handle) || (f = y.handle = function (e) {
					return typeof ue === U || e && ue.event.triggered === e.type ? t : ue.event.dispatch.apply(f.elem, arguments)
                },
				f.elem = e), n = (n || "").match(ce) || [""], u = n.length; u--;) a = Re.exec(n[u]) || [],
				h = m = a[1],
				g = (a[2] || "").split(".").sort(),
				c = ue.event.special[h] || {},
				h = (o ? c.delegateType : c.bindType) || h,
				c = ue.event.special[h] || {},
				p = ue.extend({
				    type: h,
				    origType: m,
				    data: i,
				    handler: r,
				    guid: r.guid,
				    selector: o,
				    needsContext: o && ue.expr.match.needsContext.test(o),
				    namespace: g.join(".")
				},
				l),
				(d = s[h]) || (d = s[h] = [], d.delegateCount = 0, c.setup && c.setup.call(e, i, g, f) !== !1 || (e.addEventListener ? e.addEventListener(h, f, !1) : e.attachEvent && e.attachEvent("on" + h, f))),
				c.add && (c.add.call(e, p), p.handler.guid || (p.handler.guid = r.guid)),
				o ? d.splice(d.delegateCount++, 0, p) : d.push(p),
				ue.event.global[h] = !0;
                e = null
            }
        },
        remove: function (e, t, n, r, i) {
            var o, a, s, u, l, c, f, p, d, h, g, m = ue.hasData(e) && ue._data(e);
            if (m && (c = m.events)) {
                for (t = (t || "").match(ce) || [""], l = t.length; l--;) if (s = Re.exec(t[l]) || [], d = g = s[1], h = (s[2] || "").split(".").sort(), d) {
                    for (f = ue.event.special[d] || {},
					d = (r ? f.delegateType : f.bindType) || d, p = c[d] || [], s = s[2] && RegExp("(^|\\.)" + h.join("\\.(?:.*\\.|)") + "(\\.|$)"), u = o = p.length; o--;) a = p[o],
					!i && g !== a.origType || n && n.guid !== a.guid || s && !s.test(a.namespace) || r && r !== a.selector && ("**" !== r || !a.selector) || (p.splice(o, 1), a.selector && p.delegateCount--, f.remove && f.remove.call(e, a));
                    u && !p.length && (f.teardown && f.teardown.call(e, h, m.handle) !== !1 || ue.removeEvent(e, d, m.handle), delete c[d])
                } else for (d in c) ue.event.remove(e, d + t[l], n, r, !0);
                ue.isEmptyObject(c) && (delete m.handle, ue._removeData(e, "events"))
            }
        },
        trigger: function (n, r, i, o) {
            var a, s, u, l, c, f, p, d = [i || Y],
			h = ae.call(n, "type") ? n.type : n,
			g = ae.call(n, "namespace") ? n.namespace.split(".") : [];
            if (u = f = i = i || Y, 3 !== i.nodeType && 8 !== i.nodeType && !Pe.test(h + ue.event.triggered) && (h.indexOf(".") >= 0 && (g = h.split("."), h = g.shift(), g.sort()), s = 0 > h.indexOf(":") && "on" + h, n = n[ue.expando] ? n : new ue.Event(h, "object" == typeof n && n), n.isTrigger = !0, n.namespace = g.join("."), n.namespace_re = n.namespace ? RegExp("(^|\\.)" + g.join("\\.(?:.*\\.|)") + "(\\.|$)") : null, n.result = t, n.target || (n.target = i), r = null == r ? [n] : ue.makeArray(r, [n]), c = ue.event.special[h] || {},
			o || !c.trigger || c.trigger.apply(i, r) !== !1)) {
                if (!o && !c.noBubble && !ue.isWindow(i)) {
                    for (l = c.delegateType || h, Pe.test(l + h) || (u = u.parentNode) ; u; u = u.parentNode) d.push(u),
					f = u;
                    f === (i.ownerDocument || Y) && d.push(f.defaultView || f.parentWindow || e)
                }
                for (p = 0; (u = d[p++]) && !n.isPropagationStopped() ;) n.type = p > 1 ? l : c.bindType || h,
				a = (ue._data(u, "events") || {})[n.type] && ue._data(u, "handle"),
				a && a.apply(u, r),
				a = s && u[s],
				a && ue.acceptData(u) && a.apply && a.apply(u, r) === !1 && n.preventDefault();
                if (n.type = h, !(o || n.isDefaultPrevented() || c._default && c._default.apply(i.ownerDocument, r) !== !1 || "click" === h && ue.nodeName(i, "a") || !ue.acceptData(i) || !s || !i[h] || ue.isWindow(i))) {
                    f = i[s],
					f && (i[s] = null),
					ue.event.triggered = h;
                    try {
                        i[h]()
                    } catch (e) { }
                    ue.event.triggered = t,
					f && (i[s] = f)
                }
                return n.result
            }
        },
        dispatch: function (e) {
            e = ue.event.fix(e);
            var n, r, i, o, a, s = [],
			u = re.call(arguments),
			l = (ue._data(this, "events") || {})[e.type] || [],
			c = ue.event.special[e.type] || {};
            if (u[0] = e, e.delegateTarget = this, !c.preDispatch || c.preDispatch.call(this, e) !== !1) {
                for (s = ue.event.handlers.call(this, e, l), n = 0; (o = s[n++]) && !e.isPropagationStopped() ;) for (e.currentTarget = o.elem, a = 0; (i = o.handlers[a++]) && !e.isImmediatePropagationStopped() ;) (!e.namespace_re || e.namespace_re.test(i.namespace)) && (e.handleObj = i, e.data = i.data, r = ((ue.event.special[i.origType] || {}).handle || i.handler).apply(o.elem, u), r !== t && (e.result = r) === !1 && (e.preventDefault(), e.stopPropagation()));
                return c.postDispatch && c.postDispatch.call(this, e),
				e.result
            }
        },
        handlers: function (e, n) {
            var r, i, o, a, s = [],
			u = n.delegateCount,
			l = e.target;
            if (u && l.nodeType && (!e.button || "click" !== e.type)) for (; l != this; l = l.parentNode || this) if (1 === l.nodeType && (l.disabled !== !0 || "click" !== e.type)) {
                for (o = [], a = 0; u > a; a++) i = n[a],
				r = i.selector + " ",
				o[r] === t && (o[r] = i.needsContext ? ue(r, this).index(l) >= 0 : ue.find(r, this, null, [l]).length),
				o[r] && o.push(i);
                o.length && s.push({
                    elem: l,
                    handlers: o
                })
            }
            return n.length > u && s.push({
                elem: this,
                handlers: n.slice(u)
            }),
			s
        },
        fix: function (e) {
            if (e[ue.expando]) return e;
            var t, n, r, i = e.type,
			o = e,
			a = this.fixHooks[i];
            for (a || (this.fixHooks[i] = a = Be.test(i) ? this.mouseHooks : _e.test(i) ? this.keyHooks : {}), r = a.props ? this.props.concat(a.props) : this.props, e = new ue.Event(o), t = r.length; t--;) n = r[t],
			e[n] = o[n];
            return e.target || (e.target = o.srcElement || Y),
			3 === e.target.nodeType && (e.target = e.target.parentNode),
			e.metaKey = !!e.metaKey,
			a.filter ? a.filter(e, o) : e
        },
        props: "altKey bubbles cancelable ctrlKey currentTarget eventPhase metaKey relatedTarget shiftKey target timeStamp view which".split(" "),
        fixHooks: {},
        keyHooks: {
            props: "char charCode key keyCode".split(" "),
            filter: function (e, t) {
                return null == e.which && (e.which = null != t.charCode ? t.charCode : t.keyCode),
				e
            }
        },
        mouseHooks: {
            props: "button buttons clientX clientY fromElement offsetX offsetY pageX pageY screenX screenY toElement".split(" "),
            filter: function (e, n) {
                var r, i, o, a = n.button,
				s = n.fromElement;
                return null == e.pageX && null != n.clientX && (i = e.target.ownerDocument || Y, o = i.documentElement, r = i.body, e.pageX = n.clientX + (o && o.scrollLeft || r && r.scrollLeft || 0) - (o && o.clientLeft || r && r.clientLeft || 0), e.pageY = n.clientY + (o && o.scrollTop || r && r.scrollTop || 0) - (o && o.clientTop || r && r.clientTop || 0)),
				!e.relatedTarget && s && (e.relatedTarget = s === e.target ? n.toElement : s),
				e.which || a === t || (e.which = 1 & a ? 1 : 2 & a ? 3 : 4 & a ? 2 : 0),
				e
            }
        },
        special: {
            load: {
                noBubble: !0
            },
            click: {
                trigger: function () {
                    return ue.nodeName(this, "input") && "checkbox" === this.type && this.click ? (this.click(), !1) : t
                }
            },
            focus: {
                trigger: function () {
                    if (this !== Y.activeElement && this.focus) try {
                        return this.focus(),
						!1
                    } catch (e) { }
                },
                delegateType: "focusin"
            },
            blur: {
                trigger: function () {
                    return this === Y.activeElement && this.blur ? (this.blur(), !1) : t
                },
                delegateType: "focusout"
            },
            beforeunload: {
                postDispatch: function (e) {
                    e.result !== t && (e.originalEvent.returnValue = e.result)
                }
            }
        },
        simulate: function (e, t, n, r) {
            var i = ue.extend(new ue.Event, n, {
                type: e,
                isSimulated: !0,
                originalEvent: {}
            });
            r ? ue.event.trigger(i, null, t) : ue.event.dispatch.call(t, i),
			i.isDefaultPrevented() && n.preventDefault()
        }
    },
	ue.removeEvent = Y.removeEventListener ?
	function (e, t, n) {
	    e.removeEventListener && e.removeEventListener(t, n, !1)
	} : function (e, t, n) {
	    var r = "on" + t;
	    e.detachEvent && (typeof e[r] === U && (e[r] = null), e.detachEvent(r, n))
	},
	ue.Event = function (e, n) {
	    return this instanceof ue.Event ? (e && e.type ? (this.originalEvent = e, this.type = e.type, this.isDefaultPrevented = e.defaultPrevented || e.returnValue === !1 || e.getPreventDefault && e.getPreventDefault() ? s : u) : this.type = e, n && ue.extend(this, n), this.timeStamp = e && e.timeStamp || ue.now(), this[ue.expando] = !0, t) : new ue.Event(e, n)
	},
	ue.Event.prototype = {
	    isDefaultPrevented: u,
	    isPropagationStopped: u,
	    isImmediatePropagationStopped: u,
	    preventDefault: function () {
	        var e = this.originalEvent;
	        this.isDefaultPrevented = s,
			e && (e.preventDefault ? e.preventDefault() : e.returnValue = !1)
	    },
	    stopPropagation: function () {
	        var e = this.originalEvent;
	        this.isPropagationStopped = s,
			e && (e.stopPropagation && e.stopPropagation(), e.cancelBubble = !0)
	    },
	    stopImmediatePropagation: function () {
	        this.isImmediatePropagationStopped = s,
			this.stopPropagation()
	    }
	},
	ue.each({
	    mouseenter: "mouseover",
	    mouseleave: "mouseout"
	},
	function (e, t) {
	    ue.event.special[e] = {
	        delegateType: t,
	        bindType: t,
	        handle: function (e) {
	            var n, r = this,
				i = e.relatedTarget,
				o = e.handleObj;
	            return (!i || i !== r && !ue.contains(r, i)) && (e.type = o.origType, n = o.handler.apply(this, arguments), e.type = t),
				n
	        }
	    }
	}),
	ue.support.submitBubbles || (ue.event.special.submit = {
	    setup: function () {
	        return !ue.nodeName(this, "form") && (ue.event.add(this, "click._submit keypress._submit",
			function (e) {
			    var n = e.target,
				r = ue.nodeName(n, "input") || ue.nodeName(n, "button") ? n.form : t;
			    r && !ue._data(r, "submitBubbles") && (ue.event.add(r, "submit._submit",
				function (e) {
				    e._submit_bubble = !0
				}), ue._data(r, "submitBubbles", !0))
			}), t)
	    },
	    postDispatch: function (e) {
	        e._submit_bubble && (delete e._submit_bubble, this.parentNode && !e.isTrigger && ue.event.simulate("submit", this.parentNode, e, !0))
	    },
	    teardown: function () {
	        return !ue.nodeName(this, "form") && (ue.event.remove(this, "._submit"), t)
	    }
	}),
	ue.support.changeBubbles || (ue.event.special.change = {
	    setup: function () {
	        return Fe.test(this.nodeName) ? (("checkbox" === this.type || "radio" === this.type) && (ue.event.add(this, "propertychange._change",
			function (e) {
			    "checked" === e.originalEvent.propertyName && (this._just_changed = !0)
			}), ue.event.add(this, "click._change",
			function (e) {
			    this._just_changed && !e.isTrigger && (this._just_changed = !1),
				ue.event.simulate("change", this, e, !0)
			})), !1) : (ue.event.add(this, "beforeactivate._change",
			function (e) {
			    var t = e.target;
			    Fe.test(t.nodeName) && !ue._data(t, "changeBubbles") && (ue.event.add(t, "change._change",
				function (e) {
				    !this.parentNode || e.isSimulated || e.isTrigger || ue.event.simulate("change", this.parentNode, e, !0)
				}), ue._data(t, "changeBubbles", !0))
			}), t)
	    },
	    handle: function (e) {
	        var n = e.target;
	        return this !== n || e.isSimulated || e.isTrigger || "radio" !== n.type && "checkbox" !== n.type ? e.handleObj.handler.apply(this, arguments) : t
	    },
	    teardown: function () {
	        return ue.event.remove(this, "._change"),
			!Fe.test(this.nodeName)
	    }
	}),
	ue.support.focusinBubbles || ue.each({
	    focus: "focusin",
	    blur: "focusout"
	},
	function (e, t) {
	    var n = 0,
		r = function (e) {
		    ue.event.simulate(t, e.target, ue.event.fix(e), !0)
		};
	    ue.event.special[t] = {
	        setup: function () {
	            0 === n++ && Y.addEventListener(e, r, !0)
	        },
	        teardown: function () {
	            0 === --n && Y.removeEventListener(e, r, !0)
	        }
	    }
	}),
	ue.fn.extend({
	    on: function (e, n, r, i, o) {
	        var a, s;
	        if ("object" == typeof e) {
	            "string" != typeof n && (r = r || n, n = t);
	            for (a in e) this.on(a, n, r, e[a], o);
	            return this
	        }
	        if (null == r && null == i ? (i = n, r = n = t) : null == i && ("string" == typeof n ? (i = r, r = t) : (i = r, r = n, n = t)), i === !1) i = u;
	        else if (!i) return this;
	        return 1 === o && (s = i, i = function (e) {
	            return ue().off(e),
				s.apply(this, arguments)
	        },
			i.guid = s.guid || (s.guid = ue.guid++)),
			this.each(function () {
			    ue.event.add(this, e, i, r, n)
			})
	    },
	    one: function (e, t, n, r) {
	        return this.on(e, t, n, r, 1)
	    },
	    off: function (e, n, r) {
	        var i, o;
	        if (e && e.preventDefault && e.handleObj) return i = e.handleObj,
			ue(e.delegateTarget).off(i.namespace ? i.origType + "." + i.namespace : i.origType, i.selector, i.handler),
			this;
	        if ("object" == typeof e) {
	            for (o in e) this.off(o, n, e[o]);
	            return this
	        }
	        return (n === !1 || "function" == typeof n) && (r = n, n = t),
			r === !1 && (r = u),
			this.each(function () {
			    ue.event.remove(this, e, r, n)
			})
	    },
	    bind: function (e, t, n) {
	        return this.on(e, null, t, n)
	    },
	    unbind: function (e, t) {
	        return this.off(e, null, t)
	    },
	    delegate: function (e, t, n, r) {
	        return this.on(t, e, n, r)
	    },
	    undelegate: function (e, t, n) {
	        return 1 === arguments.length ? this.off(e, "**") : this.off(t, e || "**", n)
	    },
	    trigger: function (e, t) {
	        return this.each(function () {
	            ue.event.trigger(e, t, this)
	        })
	    },
	    triggerHandler: function (e, n) {
	        var r = this[0];
	        return r ? ue.event.trigger(e, n, r, !0) : t
	    }
	}),
	function (e, t) {
	    function n(e) {
	        return he.test(e + "")
	    }
	    function r() {
	        var e, t = [];
	        return e = function (n, r) {
	            return t.push(n += " ") > C.cacheLength && delete e[t.shift()],
				e[n] = r
	        }
	    }
	    function i(e) {
	        return e[P] = !0,
			e
	    }
	    function o(e) {
	        var t = L.createElement("div");
	        try {
	            return e(t)
	        } catch (e) {
	            return !1
	        } finally {
	            t = null
	        }
	    }
	    function a(e, t, n, r) {
	        var i, o, a, s, u, l, c, d, h, g;
	        if ((t ? t.ownerDocument || t : R) !== L && D(t), t = t || L, n = n || [], !e || "string" != typeof e) return n;
	        if (1 !== (s = t.nodeType) && 9 !== s) return [];
	        if (!O && !r) {
	            if (i = ge.exec(e)) if (a = i[1]) {
	                if (9 === s) {
	                    if (o = t.getElementById(a), !o || !o.parentNode) return n;
	                    if (o.id === a) return n.push(o),
						n
	                } else if (t.ownerDocument && (o = t.ownerDocument.getElementById(a)) && _(t, o) && o.id === a) return n.push(o),
					n
	            } else {
	                if (i[2]) return K.apply(n, Z.call(t.getElementsByTagName(e), 0)),
					n;
	                if ((a = i[3]) && W.getByClassName && t.getElementsByClassName) return K.apply(n, Z.call(t.getElementsByClassName(a), 0)),
					n
	            }
	            if (W.qsa && !M.test(e)) {
	                if (c = !0, d = P, h = t, g = 9 === s && e, 1 === s && "object" !== t.nodeName.toLowerCase()) {
	                    for (l = f(e), (c = t.getAttribute("id")) ? d = c.replace(ve, "\\$&") : t.setAttribute("id", d), d = "[id='" + d + "'] ", u = l.length; u--;) l[u] = d + p(l[u]);
	                    h = de.test(e) && t.parentNode || t,
						g = l.join(",")
	                }
	                if (g) try {
	                    return K.apply(n, Z.call(h.querySelectorAll(g), 0)),
						n
	                } catch (e) { } finally {
	                    c || t.removeAttribute("id")
	                }
	            }
	        }
	        return x(e.replace(se, "$1"), t, n, r)
	    }
	    function s(e, t) {
	        var n = t && e,
			r = n && (~t.sourceIndex || J) - (~e.sourceIndex || J);
	        if (r) return r;
	        if (n) for (; n = n.nextSibling;) if (n === t) return -1;
	        return e ? 1 : -1
	    }
	    function u(e) {
	        return function (t) {
	            var n = t.nodeName.toLowerCase();
	            return "input" === n && t.type === e
	        }
	    }
	    function l(e) {
	        return function (t) {
	            var n = t.nodeName.toLowerCase();
	            return ("input" === n || "button" === n) && t.type === e
	        }
	    }
	    function c(e) {
	        return i(function (t) {
	            return t = +t,
				i(function (n, r) {
				    for (var i, o = e([], n.length, t), a = o.length; a--;) n[i = o[a]] && (n[i] = !(r[i] = n[i]))
				})
	        })
	    }
	    function f(e, t) {
	        var n, r, i, o, s, u, l, c = X[e + " "];
	        if (c) return t ? 0 : c.slice(0);
	        for (s = e, u = [], l = C.preFilter; s;) {
	            (!n || (r = $.exec(s))) && (r && (s = s.slice(r[0].length) || s), u.push(i = [])),
                    n = !1,
                    (r = le.exec(s)) && (n = r.shift(), i.push({
                        value: n,
                        type: r[0].replace(se, " ")
                    }), s = s.slice(n.length));
	            for (o in C.filter) !(r = pe[o].exec(s)) || l[o] && !(r = l[o](r)) || (n = r.shift(), i.push({
	                value: n,
	                type: o,
	                matches: r
	            }), s = s.slice(n.length));
	            if (!n) break
	        }
	        return t ? s.length : s ? a.error(e) : X(e, u).slice(0)
	    }
	    function p(e) {
	        for (var t = 0,
			n = e.length,
			r = ""; n > t; t++) r += e[t].value;
	        return r
	    }
	    function d(e, t, n) {
	        var r = t.dir,
			i = n && "parentNode" === r,
			o = z++;
	        return t.first ?
			function (t, n, o) {
			    for (; t = t[r];) if (1 === t.nodeType || i) return e(t, n, o)
			} : function (t, n, a) {
			    var s, u, l, c = I + " " + o;
			    if (a) {
			        for (; t = t[r];) if ((1 === t.nodeType || i) && e(t, n, a)) return !0
			    } else for (; t = t[r];) if (1 === t.nodeType || i) if (l = t[P] || (t[P] = {}), (u = l[r]) && u[0] === c) {
			        if ((s = u[1]) === !0 || s === N) return s === !0
			    } else if (u = l[r] = [c], u[1] = e(t, n, a) || N, u[1] === !0) return !0
			}
	    }
	    function h(e) {
	        return e.length > 1 ?
			function (t, n, r) {
			    for (var i = e.length; i--;) if (!e[i](t, n, r)) return !1;
			    return !0
			} : e[0]
	    }
	    function g(e, t, n, r, i) {
	        for (var o, a = [], s = 0, u = e.length, l = null != t; u > s; s++) (o = e[s]) && (!n || n(o, r, i)) && (a.push(o), l && t.push(s));
	        return a
	    }
	    function m(e, t, n, r, o, a) {
	        return r && !r[P] && (r = m(r)),
			o && !o[P] && (o = m(o, a)),
			i(function (i, a, s, u) {
			    var l, c, f, p = [],
				d = [],
				h = a.length,
				m = i || b(t || "*", s.nodeType ? [s] : s, []),
				y = !e || !i && t ? m : g(m, p, e, s, u),
				v = n ? o || (i ? e : h || r) ? [] : a : y;
			    if (n && n(y, v, s, u), r) for (l = g(v, d), r(l, [], s, u), c = l.length; c--;) (f = l[c]) && (v[d[c]] = !(y[d[c]] = f));
			    if (i) {
			        if (o || e) {
			            if (o) {
			                for (l = [], c = v.length; c--;) (f = v[c]) && l.push(y[c] = f);
			                o(null, v = [], l, u)
			            }
			            for (c = v.length; c--;) (f = v[c]) && (l = o ? ee.call(i, f) : p[c]) > -1 && (i[l] = !(a[l] = f))
			        }
			    } else v = g(v === a ? v.splice(h, v.length) : v),
				o ? o(null, a, v, u) : K.apply(a, v)
			})
	    }
	    function y(e) {
	        for (var t, n, r, i = e.length,
			o = C.relative[e[0].type], a = o || C.relative[" "], s = o ? 1 : 0, u = d(function (e) {
				return e === t
	        },
			a, !0), l = d(function (e) {
				return ee.call(t, e) > -1
	        },
			a, !0), c = [function (e, n, r) {
				return !o && (r || n !== j) || ((t = n).nodeType ? u(e, n, r) : l(e, n, r))
	        }]; i > s; s++) if (n = C.relative[e[s].type]) c = [d(h(c), n)];
	        else {
	            if (n = C.filter[e[s].type].apply(null, e[s].matches), n[P]) {
	                for (r = ++s; i > r && !C.relative[e[r].type]; r++);
	                return m(s > 1 && h(c), s > 1 && p(e.slice(0, s - 1)).replace(se, "$1"), n, r > s && y(e.slice(s, r)), i > r && y(e = e.slice(r)), i > r && p(e))
	            }
	            c.push(n)
	        }
	        return h(c)
	    }
	    function v(e, t) {
	        var n = 0,
			r = t.length > 0,
			o = e.length > 0,
			s = function (i, s, u, l, c) {
			    var f, p, d, h = [],
				m = 0,
				y = "0",
				v = i && [],
				b = null != c,
				x = j,
				w = i || o && C.find.TAG("*", c && s.parentNode || s),
				T = I += null == x ? 1 : Math.random() || .1;
			    for (b && (j = s !== L && s, N = n) ; null != (f = w[y]) ; y++) {
			        if (o && f) {
			            for (p = 0; d = e[p++];) if (d(f, s, u)) {
			                l.push(f);
			                break
			            }
			            b && (I = T, N = ++n)
			        }
			        r && ((f = !d && f) && m--, i && v.push(f))
			    }
			    if (m += y, r && y !== m) {
			        for (p = 0; d = t[p++];) d(v, h, s, u);
			        if (i) {
			            if (m > 0) for (; y--;) v[y] || h[y] || (h[y] = G.call(l));
			            h = g(h)
			        }
			        K.apply(l, h),
					b && !i && h.length > 0 && m + t.length > 1 && a.uniqueSort(l)
			    }
			    return b && (I = T, j = x),
				v
			};
	        return r ? i(s) : s
	    }
	    function b(e, t, n) {
	        for (var r = 0,
			i = t.length; i > r; r++) a(e, t[r], n);
	        return n
	    }
	    function x(e, t, n, r) {
	        var i, o, a, s, u, l = f(e);
	        if (!r && 1 === l.length) {
	            if (o = l[0] = l[0].slice(0), o.length > 2 && "ID" === (a = o[0]).type && 9 === t.nodeType && !O && C.relative[o[1].type]) {
	                if (t = C.find.ID(a.matches[0].replace(xe, we), t)[0], !t) return n;
	                e = e.slice(o.shift().value.length)
	            }
	            for (i = pe.needsContext.test(e) ? 0 : o.length; i-- && (a = o[i], !C.relative[s = a.type]) ;) if ((u = C.find[s]) && (r = u(a.matches[0].replace(xe, we), de.test(o[0].type) && t.parentNode || t))) {
	                if (o.splice(i, 1), e = r.length && p(o), !e) return K.apply(n, Z.call(r, 0)),
					n;
	                break
	            }
	        }
	        return S(e, l)(r, t, O, n, de.test(e)),
			n
	    }
	    function w() { }
	    var T, N, C, k, E, S, A, j, D, L, H, O, M, q, F, _, B, P = "sizzle" + -new Date,
		R = e.document,
		W = {},
		I = 0,
		z = 0,
		V = r(),
		X = r(),
		U = r(),
		Y = typeof t,
		J = 1 << 31,
		Q = [],
		G = Q.pop,
		K = Q.push,
		Z = Q.slice,
		ee = Q.indexOf ||
		function (e) {
		    for (var t = 0,
			n = this.length; n > t; t++) if (this[t] === e) return t;
		    return -1
		},
		te = "[\\x20\\t\\r\\n\\f]",
		ne = "(?:\\\\.|[\\w-]|[^\\x00-\\xa0])+",
		re = ne.replace("w", "w#"),
		ie = "([*^$|!~]?=)",
		oe = "\\[" + te + "*(" + ne + ")" + te + "*(?:" + ie + te + "*(?:(['\"])((?:\\\\.|[^\\\\])*?)\\3|(" + re + ")|)|)" + te + "*\\]",
		ae = ":(" + ne + ")(?:\\(((['\"])((?:\\\\.|[^\\\\])*?)\\3|((?:\\\\.|[^\\\\()[\\]]|" + oe.replace(3, 8) + ")*)|.*)\\)|)",
		se = RegExp("^" + te + "+|((?:^|[^\\\\])(?:\\\\.)*)" + te + "+$", "g"),
		$ = RegExp("^" + te + "*," + te + "*"),
		le = RegExp("^" + te + "*([\\x20\\t\\r\\n\\f>+~])" + te + "*"),
		ce = RegExp(ae),
		fe = RegExp("^" + re + "$"),
		pe = {
		    ID: RegExp("^#(" + ne + ")"),
		    CLASS: RegExp("^\\.(" + ne + ")"),
		    NAME: RegExp("^\\[name=['\"]?(" + ne + ")['\"]?\\]"),
		    TAG: RegExp("^(" + ne.replace("w", "w*") + ")"),
		    ATTR: RegExp("^" + oe),
		    PSEUDO: RegExp("^" + ae),
		    CHILD: RegExp("^:(only|first|last|nth|nth-last)-(child|of-type)(?:\\(" + te + "*(even|odd|(([+-]|)(\\d*)n|)" + te + "*(?:([+-]|)" + te + "*(\\d+)|))" + te + "*\\)|)", "i"),
		    needsContext: RegExp("^" + te + "*[>+~]|:(even|odd|eq|gt|lt|nth|first|last)(?:\\(" + te + "*((?:-\\d)?\\d*)" + te + "*\\)|)(?=[^-]|$)", "i")
		},
		de = /[\x20\t\r\n\f]*[+~]/,
		he = /^[^{]+\{\s*\[native code/,
		ge = /^(?:#([\w-]+)|(\w+)|\.([\w-]+))$/,
		me = /^(?:input|select|textarea|button)$/i,
		ye = /^h\d$/i,
		ve = /'|\\/g,
		be = /\=[\x20\t\r\n\f]*([^'"\]]*)[\x20\t\r\n\f]*\]/g,
		xe = /\\([\da-fA-F]{1,6}[\x20\t\r\n\f]?|.)/g,
		we = function (e, t) {
		    var n = "0x" + t - 65536;
		    return n !== n ? t : 0 > n ? String.fromCharCode(n + 65536) : String.fromCharCode(55296 | n >> 10, 56320 | 1023 & n)
		};
	    try {
	        Z.call(R.documentElement.childNodes, 0)[0].nodeType
	    } catch (e) {
	        Z = function (e) {
	            for (var t, n = []; t = this[e++];) n.push(t);
	            return n
	        }
	    }
	    E = a.isXML = function (e) {
	        var t = e && (e.ownerDocument || e).documentElement;
	        return !!t && "HTML" !== t.nodeName
	    },
		D = a.setDocument = function (e) {
		    var r = e ? e.ownerDocument || e : R;
		    return r !== L && 9 === r.nodeType && r.documentElement ? (L = r, H = r.documentElement, O = E(r), W.tagNameNoComments = o(function (e) {
		        return e.appendChild(r.createComment("")),
				!e.getElementsByTagName("*").length
		    }), W.attributes = o(function (e) {
		        e.innerHTML = "<select></select>";
		        var t = typeof e.lastChild.getAttribute("multiple");
		        return "boolean" !== t && "string" !== t
		    }), W.getByClassName = o(function (e) {
		        return e.innerHTML = "<div class='hidden e'></div><div class='hidden'></div>",
				!(!e.getElementsByClassName || !e.getElementsByClassName("e").length) && (e.lastChild.className = "e", 2 === e.getElementsByClassName("e").length)
		    }), W.getByName = o(function (e) {
		        e.id = P + 0,
				e.innerHTML = "<a name='" + P + "'></a><div name='" + P + "'></div>",
				H.insertBefore(e, H.firstChild);
		        var t = r.getElementsByName && r.getElementsByName(P).length === 2 + r.getElementsByName(P + 0).length;
		        return W.getIdNotName = !r.getElementById(P),
				H.removeChild(e),
				t
		    }), C.attrHandle = o(function (e) {
		        return e.innerHTML = "<a href='#'></a>",
				e.firstChild && typeof e.firstChild.getAttribute !== Y && "#" === e.firstChild.getAttribute("href")
		    }) ? {} : {
		        href: function (e) {
		            return e.getAttribute("href", 2)
		        },
		        type: function (e) {
		            return e.getAttribute("type")
		        }
		    },
			W.getIdNotName ? (C.find.ID = function (e, t) {
			    if (typeof t.getElementById !== Y && !O) {
			        var n = t.getElementById(e);
			        return n && n.parentNode ? [n] : []
			    }
			},
			C.filter.ID = function (e) {
			    var t = e.replace(xe, we);
			    return function (e) {
			        return e.getAttribute("id") === t
			    }
			}) : (C.find.ID = function (e, n) {
			    if (typeof n.getElementById !== Y && !O) {
			        var r = n.getElementById(e);
			        return r ? r.id === e || typeof r.getAttributeNode !== Y && r.getAttributeNode("id").value === e ? [r] : t : []
			    }
			},
			C.filter.ID = function (e) {
			    var t = e.replace(xe, we);
			    return function (e) {
			        var n = typeof e.getAttributeNode !== Y && e.getAttributeNode("id");
			        return n && n.value === t
			    }
			}), C.find.TAG = W.tagNameNoComments ?
			function (e, n) {
			    return typeof n.getElementsByTagName !== Y ? n.getElementsByTagName(e) : t
			} : function (e, t) {
			    var n, r = [],
				i = 0,
				o = t.getElementsByTagName(e);
			    if ("*" === e) {
			        for (; n = o[i++];) 1 === n.nodeType && r.push(n);
			        return r
			    }
			    return o
			},
			C.find.NAME = W.getByName &&
			function (e, n) {
			    return typeof n.getElementsByName !== Y ? n.getElementsByName(name) : t
			},
			C.find.CLASS = W.getByClassName &&
			function (e, n) {
			    return typeof n.getElementsByClassName === Y || O ? t : n.getElementsByClassName(e)
			},
			q = [], M = [":focus"], (W.qsa = n(r.querySelectorAll)) && (o(function (e) {
			    e.innerHTML = "<select><option selected=''></option></select>",
				e.querySelectorAll("[selected]").length || M.push("\\[" + te + "*(?:checked|disabled|ismap|multiple|readonly|selected|value)"),
				e.querySelectorAll(":checked").length || M.push(":checked")
			}), o(function (e) {
			    e.innerHTML = "<input type='hidden' i=''/>",
				e.querySelectorAll("[i^='']").length && M.push("[*^$]=" + te + "*(?:\"\"|'')"),
				e.querySelectorAll(":enabled").length || M.push(":enabled", ":disabled"),
				e.querySelectorAll("*,:x"),
				M.push(",.*:")
			})), (W.matchesSelector = n(F = H.matchesSelector || H.mozMatchesSelector || H.webkitMatchesSelector || H.oMatchesSelector || H.msMatchesSelector)) && o(function (e) {
			    W.disconnectedMatch = F.call(e, "div"),
				F.call(e, "[s!='']:x"),
				q.push("!=", ae)
			}), M = RegExp(M.join("|")), q = RegExp(q.join("|")), _ = n(H.contains) || H.compareDocumentPosition ?
			function (e, t) {
			    var n = 9 === e.nodeType ? e.documentElement : e,
				r = t && t.parentNode;
			    return e === r || !(!r || 1 !== r.nodeType || !(n.contains ? n.contains(r) : e.compareDocumentPosition && 16 & e.compareDocumentPosition(r)))
			} : function (e, t) {
			    if (t) for (; t = t.parentNode;) if (t === e) return !0;
			    return !1
			},
			B = H.compareDocumentPosition ?
			function (e, t) {
			    var n;
			    return e === t ? (A = !0, 0) : (n = t.compareDocumentPosition && e.compareDocumentPosition && e.compareDocumentPosition(t)) ? 1 & n || e.parentNode && 11 === e.parentNode.nodeType ? e === r || _(R, e) ? -1 : t === r || _(R, t) ? 1 : 0 : 4 & n ? -1 : 1 : e.compareDocumentPosition ? -1 : 1
			} : function (e, t) {
			    var n, i = 0,
				o = e.parentNode,
				a = t.parentNode,
				u = [e],
				l = [t];
			    if (e === t) return A = !0,
				0;
			    if (!o || !a) return e === r ? -1 : t === r ? 1 : o ? -1 : a ? 1 : 0;
			    if (o === a) return s(e, t);
			    for (n = e; n = n.parentNode;) u.unshift(n);
			    for (n = t; n = n.parentNode;) l.unshift(n);
			    for (; u[i] === l[i];) i++;
			    return i ? s(u[i], l[i]) : u[i] === R ? -1 : l[i] === R ? 1 : 0
			},
			A = !1, [0, 0].sort(B), W.detectDuplicates = A, L) : L
		},
		a.matches = function (e, t) {
		    return a(e, null, null, t)
		},
		a.matchesSelector = function (e, t) {
		    if ((e.ownerDocument || e) !== L && D(e), t = t.replace(be, "='$1']"), !(!W.matchesSelector || O || q && q.test(t) || M.test(t))) try {
		        var n = F.call(e, t);
		        if (n || W.disconnectedMatch || e.document && 11 !== e.document.nodeType) return n
		    } catch (e) { }
		    return a(t, L, null, [e]).length > 0
		},
		a.contains = function (e, t) {
		    return (e.ownerDocument || e) !== L && D(e),
			_(e, t)
		},
		a.attr = function (e, t) {
		    var n;
		    return (e.ownerDocument || e) !== L && D(e),
			O || (t = t.toLowerCase()),
			(n = C.attrHandle[t]) ? n(e) : O || W.attributes ? e.getAttribute(t) : ((n = e.getAttributeNode(t)) || e.getAttribute(t)) && e[t] === !0 ? t : n && n.specified ? n.value : null
		},
		a.error = function (e) {
		    throw Error("Syntax error, unrecognized expression: " + e)
		},
		a.uniqueSort = function (e) {
		    var t, n = [],
			r = 1,
			i = 0;
		    if (A = !W.detectDuplicates, e.sort(B), A) {
		        for (; t = e[r]; r++) t === e[r - 1] && (i = n.push(r));
		        for (; i--;) e.splice(n[i], 1)
		    }
		    return e
		},
		k = a.getText = function (e) {
		    var t, n = "",
			r = 0,
			i = e.nodeType;
		    if (i) {
		        if (1 === i || 9 === i || 11 === i) {
		            if ("string" == typeof e.textContent) return e.textContent;
		            for (e = e.firstChild; e; e = e.nextSibling) n += k(e)
		        } else if (3 === i || 4 === i) return e.nodeValue
		    } else for (; t = e[r]; r++) n += k(t);
		    return n
		},
		C = a.selectors = {
		    cacheLength: 50,
		    createPseudo: i,
		    match: pe,
		    find: {},
		    relative: {
		        ">": {
		            dir: "parentNode",
		            first: !0
		        },
		        " ": {
		            dir: "parentNode"
		        },
		        "+": {
		            dir: "previousSibling",
		            first: !0
		        },
		        "~": {
		            dir: "previousSibling"
		        }
		    },
		    preFilter: {
		        ATTR: function (e) {
		            return e[1] = e[1].replace(xe, we),
					e[3] = (e[4] || e[5] || "").replace(xe, we),
					"~=" === e[2] && (e[3] = " " + e[3] + " "),
					e.slice(0, 4)
		        },
		        CHILD: function (e) {
		            return e[1] = e[1].toLowerCase(),
					"nth" === e[1].slice(0, 3) ? (e[3] || a.error(e[0]), e[4] = +(e[4] ? e[5] + (e[6] || 1) : 2 * ("even" === e[3] || "odd" === e[3])), e[5] = +(e[7] + e[8] || "odd" === e[3])) : e[3] && a.error(e[0]),
					e
		        },
		        PSEUDO: function (e) {
		            var t, n = !e[5] && e[2];
		            return pe.CHILD.test(e[0]) ? null : (e[4] ? e[2] = e[4] : n && ce.test(n) && (t = f(n, !0)) && (t = n.indexOf(")", n.length - t) - n.length) && (e[0] = e[0].slice(0, t), e[2] = n.slice(0, t)), e.slice(0, 3))
		        }
		    },
		    filter: {
		        TAG: function (e) {
		            return "*" === e ?
					function () {
					    return !0
					} : (e = e.replace(xe, we).toLowerCase(),
					function (t) {
					    return t.nodeName && t.nodeName.toLowerCase() === e
					})
		        },
		        CLASS: function (e) {
		            var t = V[e + " "];
		            return t || (t = RegExp("(^|" + te + ")" + e + "(" + te + "|$)")) && V(e,
					function (e) {
					    return t.test(e.className || typeof e.getAttribute !== Y && e.getAttribute("class") || "")
					})
		        },
		        ATTR: function (e, t, n) {
		            return function (r) {
		                var i = a.attr(r, e);
		                return null == i ? "!=" === t : !t || (i += "", "=" === t ? i === n : "!=" === t ? i !== n : "^=" === t ? n && 0 === i.indexOf(n) : "*=" === t ? n && i.indexOf(n) > -1 : "$=" === t ? n && i.slice(-n.length) === n : "~=" === t ? (" " + i + " ").indexOf(n) > -1 : "|=" === t && (i === n || i.slice(0, n.length + 1) === n + "-"))
		            }
		        },
		        CHILD: function (e, t, n, r, i) {
		            var o = "nth" !== e.slice(0, 3),
					a = "last" !== e.slice(-4),
					s = "of-type" === t;
		            return 1 === r && 0 === i ?
					function (e) {
					    return !!e.parentNode
					} : function (t, n, u) {
					    var l, c, f, p, d, h, g = o !== a ? "nextSibling" : "previousSibling",
						m = t.parentNode,
						y = s && t.nodeName.toLowerCase(),
						v = !u && !s;
					    if (m) {
					        if (o) {
					            for (; g;) {
					                for (f = t; f = f[g];) if (s ? f.nodeName.toLowerCase() === y : 1 === f.nodeType) return !1;
					                h = g = "only" === e && !h && "nextSibling"
					            }
					            return !0
					        }
					        if (h = [a ? m.firstChild : m.lastChild], a && v) {
					            for (c = m[P] || (m[P] = {}), l = c[e] || [], d = l[0] === I && l[1], p = l[0] === I && l[2], f = d && m.childNodes[d]; f = ++d && f && f[g] || (p = d = 0) || h.pop() ;) if (1 === f.nodeType && ++p && f === t) {
					                c[e] = [I, d, p];
					                break
					            }
					        } else if (v && (l = (t[P] || (t[P] = {}))[e]) && l[0] === I) p = l[1];
					        else for (; (f = ++d && f && f[g] || (p = d = 0) || h.pop()) && ((s ? f.nodeName.toLowerCase() !== y : 1 !== f.nodeType) || !++p || (v && ((f[P] || (f[P] = {}))[e] = [I, p]), f !== t)) ;);
					        return p -= i,
							p === r || 0 === p % r && p / r >= 0
					    }
					}
		        },
		        PSEUDO: function (e, t) {
		            var n, r = C.pseudos[e] || C.setFilters[e.toLowerCase()] || a.error("unsupported pseudo: " + e);
		            return r[P] ? r(t) : r.length > 1 ? (n = [e, e, "", t], C.setFilters.hasOwnProperty(e.toLowerCase()) ? i(function (e, n) {
		                for (var i, o = r(e, t), a = o.length; a--;) i = ee.call(e, o[a]),
						e[i] = !(n[i] = o[a])
		            }) : function (e) {
		                return r(e, 0, n)
		            }) : r
		        }
		    },
		    pseudos: {
		        not: i(function (e) {
		            var t = [],
					n = [],
					r = S(e.replace(se, "$1"));
		            return r[P] ? i(function (e, t, n, i) {
		                for (var o, a = r(e, null, i, []), s = e.length; s--;) (o = a[s]) && (e[s] = !(t[s] = o))
		            }) : function (e, i, o) {
		                return t[0] = e,
						r(t, null, o, n),
						!n.pop()
		            }
		        }),
		        has: i(function (e) {
		            return function (t) {
		                return a(e, t).length > 0
		            }
		        }),
		        contains: i(function (e) {
		            return function (t) {
		                return (t.textContent || t.innerText || k(t)).indexOf(e) > -1
		            }
		        }),
		        lang: i(function (e) {
		            return fe.test(e || "") || a.error("unsupported lang: " + e),
					e = e.replace(xe, we).toLowerCase(),
					function (t) {
					    var n;
					    do
					        if (n = O ? t.getAttribute("xml:lang") || t.getAttribute("lang") : t.lang) return n = n.toLowerCase(),
                            n === e || 0 === n.indexOf(e + "-");
					    while ((t = t.parentNode) && 1 === t.nodeType);
					    return !1
					}
		        }),
		        target: function (t) {
		            var n = e.location && e.location.hash;
		            return n && n.slice(1) === t.id
		        },
		        root: function (e) {
		            return e === H
		        },
		        focus: function (e) {
		            return e === L.activeElement && (!L.hasFocus || L.hasFocus()) && !!(e.type || e.href || ~e.tabIndex)
		        },
		        enabled: function (e) {
		            return e.disabled === !1
		        },
		        disabled: function (e) {
		            return e.disabled === !0
		        },
		        checked: function (e) {
		            var t = e.nodeName.toLowerCase();
		            return "input" === t && !!e.checked || "option" === t && !!e.selected
		        },
		        selected: function (e) {
		            return e.parentNode && e.parentNode.selectedIndex,
					e.selected === !0
		        },
		        empty: function (e) {
		            for (e = e.firstChild; e; e = e.nextSibling) if (e.nodeName > "@" || 3 === e.nodeType || 4 === e.nodeType) return !1;
		            return !0
		        },
		        parent: function (e) {
		            return !C.pseudos.empty(e)
		        },
		        header: function (e) {
		            return ye.test(e.nodeName)
		        },
		        input: function (e) {
		            return me.test(e.nodeName)
		        },
		        button: function (e) {
		            var t = e.nodeName.toLowerCase();
		            return "input" === t && "button" === e.type || "button" === t
		        },
		        text: function (e) {
		            var t;
		            return "input" === e.nodeName.toLowerCase() && "text" === e.type && (null == (t = e.getAttribute("type")) || t.toLowerCase() === e.type)
		        },
		        first: c(function () {
		            return [0]
		        }),
		        last: c(function (e, t) {
		            return [t - 1]
		        }),
		        eq: c(function (e, t, n) {
		            return [0 > n ? n + t : n]
		        }),
		        even: c(function (e, t) {
		            for (var n = 0; t > n; n += 2) e.push(n);
		            return e
		        }),
		        odd: c(function (e, t) {
		            for (var n = 1; t > n; n += 2) e.push(n);
		            return e
		        }),
		        lt: c(function (e, t, n) {
		            for (var r = 0 > n ? n + t : n; --r >= 0;) e.push(r);
		            return e
		        }),
		        gt: c(function (e, t, n) {
		            for (var r = 0 > n ? n + t : n; t > ++r;) e.push(r);
		            return e
		        })
		    }
		};
	    for (T in {
	        radio: !0,
	        checkbox: !0,
	        file: !0,
	        password: !0,
	        image: !0
	    }) C.pseudos[T] = u(T);
	    for (T in {
	        submit: !0,
	        reset: !0
	    }) C.pseudos[T] = l(T);
	    S = a.compile = function (e, t) {
	        var n, r = [],
			i = [],
			o = U[e + " "];
	        if (!o) {
	            for (t || (t = f(e)), n = t.length; n--;) o = y(t[n]),
				o[P] ? r.push(o) : i.push(o);
	            o = U(e, v(i, r))
	        }
	        return o
	    },
		C.pseudos.nth = C.pseudos.eq,
		C.filters = w.prototype = C.pseudos,
		C.setFilters = new w,
		D(),
		a.attr = ue.attr,
		ue.find = a,
		ue.expr = a.selectors,
		ue.expr[":"] = ue.expr.pseudos,
		ue.unique = a.uniqueSort,
		ue.text = a.getText,
		ue.isXMLDoc = a.isXML,
		ue.contains = a.contains
	}(e);
    var We = /Until$/,
	$e = /^(?:parents|prev(?:Until|All))/,
	Ie = /^.[^:#\[\.,]*$/,
	ze = ue.expr.match.needsContext,
	Ve = {
	    children: !0,
	    contents: !0,
	    next: !0,
	    prev: !0
	};
    ue.fn.extend({
        find: function (e) {
            var t, n, r, i = this.length;
            if ("string" != typeof e) return r = this,
			this.pushStack(ue(e).filter(function () {
			    for (t = 0; i > t; t++) if (ue.contains(r[t], this)) return !0
			}));
            for (n = [], t = 0; i > t; t++) ue.find(e, this[t], n);
            return n = this.pushStack(i > 1 ? ue.unique(n) : n),
			n.selector = (this.selector ? this.selector + " " : "") + e,
			n
        },
        has: function (e) {
            var t, n = ue(e, this),
			r = n.length;
            return this.filter(function () {
                for (t = 0; r > t; t++) if (ue.contains(this, n[t])) return !0
            })
        },
        not: function (e) {
            return this.pushStack(c(this, e, !1))
        },
        filter: function (e) {
            return this.pushStack(c(this, e, !0))
        },
        is: function (e) {
            return !!e && ("string" == typeof e ? ze.test(e) ? ue(e, this.context).index(this[0]) >= 0 : ue.filter(e, this).length > 0 : this.filter(e).length > 0)
        },
        closest: function (e, t) {
            for (var n, r = 0,
			i = this.length,
			o = [], a = ze.test(e) || "string" != typeof e ? ue(e, t || this.context) : 0; i > r; r++) for (n = this[r]; n && n.ownerDocument && n !== t && 11 !== n.nodeType;) {
			    if (a ? a.index(n) > -1 : ue.find.matchesSelector(n, e)) {
			        o.push(n);
			        break
			    }
			    n = n.parentNode
			}
            return this.pushStack(o.length > 1 ? ue.unique(o) : o)
        },
        index: function (e) {
            return e ? "string" == typeof e ? ue.inArray(this[0], ue(e)) : ue.inArray(e.jquery ? e[0] : e, this) : this[0] && this[0].parentNode ? this.first().prevAll().length : -1
        },
        add: function (e, t) {
            var n = "string" == typeof e ? ue(e, t) : ue.makeArray(e && e.nodeType ? [e] : e),
			r = ue.merge(this.get(), n);
            return this.pushStack(ue.unique(r))
        },
        addBack: function (e) {
            return this.add(null == e ? this.prevObject : this.prevObject.filter(e))
        }
    }),
	ue.fn.andSelf = ue.fn.addBack,
	ue.each({
	    parent: function (e) {
	        var t = e.parentNode;
	        return t && 11 !== t.nodeType ? t : null
	    },
	    parents: function (e) {
	        return ue.dir(e, "parentNode")
	    },
	    parentsUntil: function (e, t, n) {
	        return ue.dir(e, "parentNode", n)
	    },
	    next: function (e) {
	        return l(e, "nextSibling")
	    },
	    prev: function (e) {
	        return l(e, "previousSibling")
	    },
	    nextAll: function (e) {
	        return ue.dir(e, "nextSibling")
	    },
	    prevAll: function (e) {
	        return ue.dir(e, "previousSibling");
	    },
	    nextUntil: function (e, t, n) {
	        return ue.dir(e, "nextSibling", n)
	    },
	    prevUntil: function (e, t, n) {
	        return ue.dir(e, "previousSibling", n)
	    },
	    siblings: function (e) {
	        return ue.sibling((e.parentNode || {}).firstChild, e)
	    },
	    children: function (e) {
	        return ue.sibling(e.firstChild)
	    },
	    contents: function (e) {
	        return ue.nodeName(e, "iframe") ? e.contentDocument || e.contentWindow.document : ue.merge([], e.childNodes)
	    }
	},
	function (e, t) {
	    ue.fn[e] = function (n, r) {
	        var i = ue.map(this, t, n);
	        return We.test(e) || (r = n),
			r && "string" == typeof r && (i = ue.filter(r, i)),
			i = this.length > 1 && !Ve[e] ? ue.unique(i) : i,
			this.length > 1 && $e.test(e) && (i = i.reverse()),
			this.pushStack(i)
	    }
	}),
	ue.extend({
	    filter: function (e, t, n) {
	        return n && (e = ":not(" + e + ")"),
			1 === t.length ? ue.find.matchesSelector(t[0], e) ? [t[0]] : [] : ue.find.matches(e, t)
	    },
	    dir: function (e, n, r) {
	        for (var i = [], o = e[n]; o && 9 !== o.nodeType && (r === t || 1 !== o.nodeType || !ue(o).is(r)) ;) 1 === o.nodeType && i.push(o),
			o = o[n];
	        return i
	    },
	    sibling: function (e, t) {
	        for (var n = []; e; e = e.nextSibling) 1 === e.nodeType && e !== t && n.push(e);
	        return n
	    }
	});
    var Xe = "abbr|article|aside|audio|bdi|canvas|data|datalist|details|figcaption|figure|footer|header|hgroup|mark|meter|nav|output|progress|section|summary|time|video",
	Ue = / jQuery\d+="(?:null|\d+)"/g,
	Ye = RegExp("<(?:" + Xe + ")[\\s/>]", "i"),
	Je = /^\s+/,
	Qe = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:]+)[^>]*)\/>/gi,
	Ge = /<([\w:]+)/,
	Ke = /<tbody/i,
	Ze = /<|&#?\w+;/,
	et = /<(?:script|style|link)/i,
	tt = /^(?:checkbox|radio)$/i,
	nt = /checked\s*(?:[^=]|=\s*.checked.)/i,
	rt = /^$|\/(?:java|ecma)script/i,
	it = /^true\/(.*)/,
	ot = /^\s*<!(?:\[CDATA\[|--)|(?:\]\]|--)>\s*$/g,
	at = {
	    option: [1, "<select multiple='multiple'>", "</select>"],
	    legend: [1, "<fieldset>", "</fieldset>"],
	    area: [1, "<map>", "</map>"],
	    param: [1, "<object>", "</object>"],
	    thead: [1, "<table>", "</table>"],
	    tr: [2, "<table><tbody>", "</tbody></table>"],
	    col: [2, "<table><tbody></tbody><colgroup>", "</colgroup></table>"],
	    td: [3, "<table><tbody><tr>", "</tr></tbody></table>"],
	    _default: ue.support.htmlSerialize ? [0, "", ""] : [1, "X<div>", "</div>"]
	},
	st = f(Y),
	ut = st.appendChild(Y.createElement("div"));
    at.optgroup = at.option,
	at.tbody = at.tfoot = at.colgroup = at.caption = at.thead,
	at.th = at.td,
	ue.fn.extend({
	    text: function (e) {
	        return ue.access(this,
			function (e) {
			    return e === t ? ue.text(this) : this.empty().append((this[0] && this[0].ownerDocument || Y).createTextNode(e))
			},
			null, e, arguments.length)
	    },
	    wrapAll: function (e) {
	        if (ue.isFunction(e)) return this.each(function (t) {
	            ue(this).wrapAll(e.call(this, t))
	        });
	        if (this[0]) {
	            var t = ue(e, this[0].ownerDocument).eq(0).clone(!0);
	            this[0].parentNode && t.insertBefore(this[0]),
				t.map(function () {
				    for (var e = this; e.firstChild && 1 === e.firstChild.nodeType;) e = e.firstChild;
				    return e
				}).append(this)
	        }
	        return this
	    },
	    wrapInner: function (e) {
	        return ue.isFunction(e) ? this.each(function (t) {
	            ue(this).wrapInner(e.call(this, t))
	        }) : this.each(function () {
	            var t = ue(this),
				n = t.contents();
	            n.length ? n.wrapAll(e) : t.append(e)
	        })
	    },
	    wrap: function (e) {
	        var t = ue.isFunction(e);
	        return this.each(function (n) {
	            ue(this).wrapAll(t ? e.call(this, n) : e)
	        })
	    },
	    unwrap: function () {
	        return this.parent().each(function () {
	            ue.nodeName(this, "body") || ue(this).replaceWith(this.childNodes)
	        }).end()
	    },
	    append: function () {
	        return this.domManip(arguments, !0,
			function (e) {
			    (1 === this.nodeType || 11 === this.nodeType || 9 === this.nodeType) && this.appendChild(e)
			})
	    },
	    prepend: function () {
	        return this.domManip(arguments, !0,
			function (e) {
			    (1 === this.nodeType || 11 === this.nodeType || 9 === this.nodeType) && this.insertBefore(e, this.firstChild)
			})
	    },
	    before: function () {
	        return this.domManip(arguments, !1,
			function (e) {
			    this.parentNode && this.parentNode.insertBefore(e, this)
			})
	    },
	    after: function () {
	        return this.domManip(arguments, !1,
			function (e) {
			    this.parentNode && this.parentNode.insertBefore(e, this.nextSibling)
			})
	    },
	    remove: function (e, t) {
	        for (var n, r = 0; null != (n = this[r]) ; r++) (!e || ue.filter(e, [n]).length > 0) && (t || 1 !== n.nodeType || ue.cleanData(v(n)), n.parentNode && (t && ue.contains(n.ownerDocument, n) && g(v(n, "script")), n.parentNode.removeChild(n)));
	        return this
	    },
	    empty: function () {
	        for (var e, t = 0; null != (e = this[t]) ; t++) {
	            for (1 === e.nodeType && ue.cleanData(v(e, !1)) ; e.firstChild;) e.removeChild(e.firstChild);
	            e.options && ue.nodeName(e, "select") && (e.options.length = 0)
	        }
	        return this
	    },
	    clone: function (e, t) {
	        return e = null != e && e,
			t = null == t ? e : t,
			this.map(function () {
			    return ue.clone(this, e, t)
			})
	    },
	    html: function (e) {
	        return ue.access(this,
			function (e) {
			    var n = this[0] || {},
				r = 0,
				i = this.length;
			    if (e === t) return 1 === n.nodeType ? n.innerHTML.replace(Ue, "") : t;
			    if (!("string" != typeof e || et.test(e) || !ue.support.htmlSerialize && Ye.test(e) || !ue.support.leadingWhitespace && Je.test(e) || at[(Ge.exec(e) || ["", ""])[1].toLowerCase()])) {
			        e = e.replace(Qe, "<$1></$2>");
			        try {
			            for (; i > r; r++) n = this[r] || {},
						1 === n.nodeType && (ue.cleanData(v(n, !1)), n.innerHTML = e);
			            n = 0
			        } catch (e) { }
			    }
			    n && this.empty().append(e)
			},
			null, e, arguments.length)
	    },
	    replaceWith: function (e) {
	        var t = ue.isFunction(e);
	        return t || "string" == typeof e || (e = ue(e).not(this).detach()),
			this.domManip([e], !0,
			function (e) {
			    var t = this.nextSibling,
				n = this.parentNode;
			    n && (ue(this).remove(), n.insertBefore(e, t))
			})
	    },
	    detach: function (e) {
	        return this.remove(e, !0)
	    },
	    domManip: function (e, n, r) {
	        e = te.apply([], e);
	        var i, o, a, s, u, l, c = 0,
			f = this.length,
			g = this,
			m = f - 1,
			y = e[0],
			b = ue.isFunction(y);
	        if (b || !(1 >= f || "string" != typeof y || ue.support.checkClone) && nt.test(y)) return this.each(function (i) {
	            var o = g.eq(i);
	            b && (e[0] = y.call(this, i, n ? o.html() : t)),
				o.domManip(e, n, r)
	        });
	        if (f && (l = ue.buildFragment(e, this[0].ownerDocument, !1, this), i = l.firstChild, 1 === l.childNodes.length && (l = i), i)) {
	            for (n = n && ue.nodeName(i, "tr"), s = ue.map(v(l, "script"), d), a = s.length; f > c; c++) o = l,
				c !== m && (o = ue.clone(o, !0, !0), a && ue.merge(s, v(o, "script"))),
				r.call(n && ue.nodeName(this[c], "table") ? p(this[c], "tbody") : this[c], o, c);
	            if (a) for (u = s[s.length - 1].ownerDocument, ue.map(s, h), c = 0; a > c; c++) o = s[c],
				rt.test(o.type || "") && !ue._data(o, "globalEval") && ue.contains(u, o) && (o.src ? ue.ajax({
				    url: o.src,
				    type: "GET",
				    dataType: "script",
				    async: !1,
				    global: !1,
				    throws: !0
				}) : ue.globalEval((o.text || o.textContent || o.innerHTML || "").replace(ot, "")));
	            l = i = null
	        }
	        return this
	    }
	}),
	ue.each({
	    appendTo: "append",
	    prependTo: "prepend",
	    insertBefore: "before",
	    insertAfter: "after",
	    replaceAll: "replaceWith"
	},
	function (e, t) {
	    ue.fn[e] = function (e) {
	        for (var n, r = 0,
			i = [], o = ue(e), a = o.length - 1; a >= r; r++) n = r === a ? this : this.clone(!0),
			ue(o[r])[t](n),
			ne.apply(i, n.get());
	        return this.pushStack(i)
	    }
	}),
	ue.extend({
	    clone: function (e, t, n) {
	        var r, i, o, a, s, u = ue.contains(e.ownerDocument, e);
	        if (ue.support.html5Clone || ue.isXMLDoc(e) || !Ye.test("<" + e.nodeName + ">") ? o = e.cloneNode(!0) : (ut.innerHTML = e.outerHTML, ut.removeChild(o = ut.firstChild)), !(ue.support.noCloneEvent && ue.support.noCloneChecked || 1 !== e.nodeType && 11 !== e.nodeType || ue.isXMLDoc(e))) for (r = v(o), s = v(e), a = 0; null != (i = s[a]) ; ++a) r[a] && y(i, r[a]);
	        if (t) if (n) for (s = s || v(e), r = r || v(o), a = 0; null != (i = s[a]) ; a++) m(i, r[a]);
	        else m(e, o);
	        return r = v(o, "script"),
			r.length > 0 && g(r, !u && v(e, "script")),
			r = s = i = null,
			o
	    },
	    buildFragment: function (e, t, n, r) {
	        for (var i, o, a, s, u, l, c, p = e.length,
			d = f(t), h = [], m = 0; p > m; m++) if (o = e[m], o || 0 === o) if ("object" === ue.type(o)) ue.merge(h, o.nodeType ? [o] : o);
			else if (Ze.test(o)) {
			    for (s = s || d.appendChild(t.createElement("div")), u = (Ge.exec(o) || ["", ""])[1].toLowerCase(), c = at[u] || at._default, s.innerHTML = c[1] + o.replace(Qe, "<$1></$2>") + c[2], i = c[0]; i--;) s = s.lastChild;
			    if (!ue.support.leadingWhitespace && Je.test(o) && h.push(t.createTextNode(Je.exec(o)[0])), !ue.support.tbody) for (o = "table" !== u || Ke.test(o) ? "<table>" !== c[1] || Ke.test(o) ? 0 : s : s.firstChild, i = o && o.childNodes.length; i--;) ue.nodeName(l = o.childNodes[i], "tbody") && !l.childNodes.length && o.removeChild(l);
			    for (ue.merge(h, s.childNodes), s.textContent = ""; s.firstChild;) s.removeChild(s.firstChild);
			    s = d.lastChild
			} else h.push(t.createTextNode(o));
	        for (s && d.removeChild(s), ue.support.appendChecked || ue.grep(v(h, "input"), b), m = 0; o = h[m++];) if ((!r || -1 === ue.inArray(o, r)) && (a = ue.contains(o.ownerDocument, o), s = v(d.appendChild(o), "script"), a && g(s), n)) for (i = 0; o = s[i++];) rt.test(o.type || "") && n.push(o);
	        return s = null,
			d
	    },
	    cleanData: function (e, t) {
	        for (var n, r, i, o, a = 0,
			s = ue.expando,
			u = ue.cache,
			l = ue.support.deleteExpando,
			c = ue.event.special; null != (n = e[a]) ; a++) if ((t || ue.acceptData(n)) && (i = n[s], o = i && u[i])) {
			    if (o.events) for (r in o.events) c[r] ? ue.event.remove(n, r) : ue.removeEvent(n, r, o.handle);
			    u[i] && (delete u[i], l ? delete n[s] : typeof n.removeAttribute !== U ? n.removeAttribute(s) : n[s] = null, Z.push(i))
			}
	    }
	});
    var lt, ct, ft, pt = /alpha\([^)]*\)/i,
	dt = /opacity\s*=\s*([^)]*)/,
	ht = /^(top|right|bottom|left)$/,
	gt = /^(none|table(?!-c[ea]).+)/,
	mt = /^margin/,
	yt = RegExp("^(" + le + ")(.*)$", "i"),
	vt = RegExp("^(" + le + ")(?!px)[a-z%]+$", "i"),
	bt = RegExp("^([+-])=(" + le + ")", "i"),
	xt = {
	    BODY: "block"
	},
	wt = {
	    position: "absolute",
	    visibility: "hidden",
	    display: "block"
	},
	Tt = {
	    letterSpacing: 0,
	    fontWeight: 400
	},
	Nt = ["Top", "Right", "Bottom", "Left"],
	Ct = ["Webkit", "O", "Moz", "ms"];
    ue.fn.extend({
        css: function (e, n) {
            return ue.access(this,
			function (e, n, r) {
			    var i, o, a = {},
				s = 0;
			    if (ue.isArray(n)) {
			        for (o = ct(e), i = n.length; i > s; s++) a[n[s]] = ue.css(e, n[s], !1, o);
			        return a
			    }
			    return r !== t ? ue.style(e, n, r) : ue.css(e, n)
			},
			e, n, arguments.length > 1)
        },
        show: function () {
            return T(this, !0)
        },
        hide: function () {
            return T(this)
        },
        toggle: function (e) {
            var t = "boolean" == typeof e;
            return this.each(function () {
                (t ? e : w(this)) ? ue(this).show() : ue(this).hide()
            })
        }
    }),
	ue.extend({
	    cssHooks: {
	        opacity: {
	            get: function (e, t) {
	                if (t) {
	                    var n = ft(e, "opacity");
	                    return "" === n ? "1" : n
	                }
	            }
	        }
	    },
	    cssNumber: {
	        columnCount: !0,
	        fillOpacity: !0,
	        fontWeight: !0,
	        lineHeight: !0,
	        opacity: !0,
	        orphans: !0,
	        widows: !0,
	        zIndex: !0,
	        zoom: !0
	    },
	    cssProps: {
	        float: ue.support.cssFloat ? "cssFloat" : "styleFloat"
	    },
	    style: function (e, n, r, i) {
	        if (e && 3 !== e.nodeType && 8 !== e.nodeType && e.style) {
	            var o, a, s, u = ue.camelCase(n),
				l = e.style;
	            if (n = ue.cssProps[u] || (ue.cssProps[u] = x(l, u)), s = ue.cssHooks[n] || ue.cssHooks[u], r === t) return s && "get" in s && (o = s.get(e, !1, i)) !== t ? o : l[n];
	            if (a = typeof r, "string" === a && (o = bt.exec(r)) && (r = (o[1] + 1) * o[2] + parseFloat(ue.css(e, n)), a = "number"), !(null == r || "number" === a && isNaN(r) || ("number" !== a || ue.cssNumber[u] || (r += "px"), ue.support.clearCloneStyle || "" !== r || 0 !== n.indexOf("background") || (l[n] = "inherit"), s && "set" in s && (r = s.set(e, r, i)) === t))) try {
	                l[n] = r
	            } catch (e) { }
	        }
	    },
	    css: function (e, n, r, i) {
	        var o, a, s, u = ue.camelCase(n);
	        return n = ue.cssProps[u] || (ue.cssProps[u] = x(e.style, u)),
			s = ue.cssHooks[n] || ue.cssHooks[u],
			s && "get" in s && (a = s.get(e, !0, r)),
			a === t && (a = ft(e, n, i)),
			"normal" === a && n in Tt && (a = Tt[n]),
			"" === r || r ? (o = parseFloat(a), r === !0 || ue.isNumeric(o) ? o || 0 : a) : a
	    },
	    swap: function (e, t, n, r) {
	        var i, o, a = {};
	        for (o in t) a[o] = e.style[o],
			e.style[o] = t[o];
	        i = n.apply(e, r || []);
	        for (o in t) e.style[o] = a[o];
	        return i
	    }
	}),
	e.getComputedStyle ? (ct = function (t) {
	    return e.getComputedStyle(t, null)
	},
	ft = function (e, n, r) {
	    var i, o, a, s = r || ct(e),
		u = s ? s.getPropertyValue(n) || s[n] : t,
		l = e.style;
	    return s && ("" !== u || ue.contains(e.ownerDocument, e) || (u = ue.style(e, n)), vt.test(u) && mt.test(n) && (i = l.width, o = l.minWidth, a = l.maxWidth, l.minWidth = l.maxWidth = l.width = u, u = s.width, l.width = i, l.minWidth = o, l.maxWidth = a)),
		u
	}) : Y.documentElement.currentStyle && (ct = function (e) {
	    return e.currentStyle
	},
	ft = function (e, n, r) {
	    var i, o, a, s = r || ct(e),
		u = s ? s[n] : t,
		l = e.style;
	    return null == u && l && l[n] && (u = l[n]),
		vt.test(u) && !ht.test(n) && (i = l.left, o = e.runtimeStyle, a = o && o.left, a && (o.left = e.currentStyle.left), l.left = "fontSize" === n ? "1em" : u, u = l.pixelLeft + "px", l.left = i, a && (o.left = a)),
		"" === u ? "auto" : u
	}),
	ue.each(["height", "width"],
	function (e, n) {
	    ue.cssHooks[n] = {
	        get: function (e, r, i) {
	            return r ? 0 === e.offsetWidth && gt.test(ue.css(e, "display")) ? ue.swap(e, wt,
				function () {
				    return k(e, n, i)
				}) : k(e, n, i) : t
	        },
	        set: function (e, t, r) {
	            var i = r && ct(e);
	            return N(e, t, r ? C(e, n, r, ue.support.boxSizing && "border-box" === ue.css(e, "boxSizing", !1, i), i) : 0)
	        }
	    }
	}),
	ue.support.opacity || (ue.cssHooks.opacity = {
	    get: function (e, t) {
	        return dt.test((t && e.currentStyle ? e.currentStyle.filter : e.style.filter) || "") ? .01 * parseFloat(RegExp.$1) + "" : t ? "1" : ""
	    },
	    set: function (e, t) {
	        var n = e.style,
			r = e.currentStyle,
			i = ue.isNumeric(t) ? "alpha(opacity=" + 100 * t + ")" : "",
			o = r && r.filter || n.filter || "";
	        n.zoom = 1,
			(t >= 1 || "" === t) && "" === ue.trim(o.replace(pt, "")) && n.removeAttribute && (n.removeAttribute("filter"), "" === t || r && !r.filter) || (n.filter = pt.test(o) ? o.replace(pt, i) : o + " " + i)
	    }
	}),
	ue(function () {
	    ue.support.reliableMarginRight || (ue.cssHooks.marginRight = {
	        get: function (e, n) {
	            return n ? ue.swap(e, {
	                display: "inline-block"
	            },
				ft, [e, "marginRight"]) : t
	        }
	    }),
		!ue.support.pixelPosition && ue.fn.position && ue.each(["top", "left"],
		function (e, n) {
		    ue.cssHooks[n] = {
		        get: function (e, r) {
		            return r ? (r = ft(e, n), vt.test(r) ? ue(e).position()[n] + "px" : r) : t
		        }
		    }
		})
	}),
	ue.expr && ue.expr.filters && (ue.expr.filters.hidden = function (e) {
	    return 0 >= e.offsetWidth && 0 >= e.offsetHeight || !ue.support.reliableHiddenOffsets && "none" === (e.style && e.style.display || ue.css(e, "display"))
	},
	ue.expr.filters.visible = function (e) {
	    return !ue.expr.filters.hidden(e)
	}),
	ue.each({
	    margin: "",
	    padding: "",
	    border: "Width"
	},
	function (e, t) {
	    ue.cssHooks[e + t] = {
	        expand: function (n) {
	            for (var r = 0,
				i = {},
				o = "string" == typeof n ? n.split(" ") : [n]; 4 > r; r++) i[e + Nt[r] + t] = o[r] || o[r - 2] || o[0];
	            return i
	        }
	    },
		mt.test(e) || (ue.cssHooks[e + t].set = N)
	});
    var kt = /%20/g,
	Et = /\[\]$/,
	St = /\r?\n/g,
	At = /^(?:submit|button|image|reset|file)$/i,
	jt = /^(?:input|select|textarea|keygen)/i;
    ue.fn.extend({
        serialize: function () {
            return ue.param(this.serializeArray())
        },
        serializeArray: function () {
            return this.map(function () {
                var e = ue.prop(this, "elements");
                return e ? ue.makeArray(e) : this
            }).filter(function () {
                var e = this.type;
                return this.name && !ue(this).is(":disabled") && jt.test(this.nodeName) && !At.test(e) && (this.checked || !tt.test(e))
            }).map(function (e, t) {
                var n = ue(this).val();
                return null == n ? null : ue.isArray(n) ? ue.map(n,
				function (e) {
				    return {
				        name: t.name,
				        value: e.replace(St, "\r\n")
				    }
				}) : {
				    name: t.name,
				    value: n.replace(St, "\r\n")
				}
            }).get()
        }
    }),
	ue.param = function (e, n) {
	    var r, i = [],
		o = function (e, t) {
		    t = ue.isFunction(t) ? t() : null == t ? "" : t,
			i[i.length] = encodeURIComponent(e) + "=" + encodeURIComponent(t)
		};
	    if (n === t && (n = ue.ajaxSettings && ue.ajaxSettings.traditional), ue.isArray(e) || e.jquery && !ue.isPlainObject(e)) ue.each(e,
		function () {
		    o(this.name, this.value)
		});
	    else for (r in e) A(r, e[r], n, o);
	    return i.join("&").replace(kt, "+")
	},
	ue.each("blur focus focusin focusout load resize scroll unload click dblclick mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave change select submit keydown keypress keyup error contextmenu".split(" "),
	function (e, t) {
	    ue.fn[t] = function (e, n) {
	        return arguments.length > 0 ? this.on(t, null, e, n) : this.trigger(t)
	    }
	}),
	ue.fn.hover = function (e, t) {
	    return this.mouseenter(e).mouseleave(t || e)
	};
    var Dt, Lt, Ht = ue.now(),
	Ot = /\?/,
	Mt = /#.*$/,
	qt = /([?&])_=[^&]*/,
	Ft = /^(.*?):[ \t]*([^\r\n]*)\r?$/gm,
	_t = /^(?:about|app|app-storage|.+-extension|file|res|widget):$/,
	Bt = /^(?:GET|HEAD)$/,
	Pt = /^\/\//,
	Rt = /^([\w.+-]+:)(?:\/\/([^\/?#:]*)(?::(\d+)|)|)/,
	Wt = ue.fn.load,
	$t = {},
	It = {},
	zt = "*/".concat("*");
    try {
        Lt = J.href
    } catch (e) {
        Lt = Y.createElement("a"),
		Lt.href = "",
		Lt = Lt.href
    }
    Dt = Rt.exec(Lt.toLowerCase()) || [],
	ue.fn.load = function (e, n, r) {
	    if ("string" != typeof e && Wt) return Wt.apply(this, arguments);
	    var i, o, a, s = this,
		u = e.indexOf(" ");
	    return u >= 0 && (i = e.slice(u, e.length), e = e.slice(0, u)),
		ue.isFunction(n) ? (r = n, n = t) : n && "object" == typeof n && (a = "POST"),
		s.length > 0 && ue.ajax({
		    url: e,
		    type: a,
		    dataType: "html",
		    data: n
		}).done(function (e) {
		    o = arguments,
			s.html(i ? ue("<div>").append(ue.parseHTML(e)).find(i) : e)
		}).complete(r &&
		function (e, t) {
		    s.each(r, o || [e.responseText, t, e])
		}),
		this
	},
	ue.each(["ajaxStart", "ajaxStop", "ajaxComplete", "ajaxError", "ajaxSuccess", "ajaxSend"],
	function (e, t) {
	    ue.fn[t] = function (e) {
	        return this.on(t, e)
	    }
	}),
	ue.each(["get", "post"],
	function (e, n) {
	    ue[n] = function (e, r, i, o) {
	        return ue.isFunction(r) && (o = o || i, i = r, r = t),
			ue.ajax({
			    url: e,
			    type: n,
			    dataType: o,
			    data: r,
			    success: i
			})
	    }
	}),
	ue.extend({
	    active: 0,
	    lastModified: {},
	    etag: {},
	    ajaxSettings: {
	        url: Lt,
	        type: "GET",
	        isLocal: _t.test(Dt[1]),
	        global: !0,
	        processData: !0,
	        async: !0,
	        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
	        accepts: {
	            "*": zt,
	            text: "text/plain",
	            html: "text/html",
	            xml: "application/xml, text/xml",
	            json: "application/json, text/javascript"
	        },
	        contents: {
	            xml: /xml/,
	            html: /html/,
	            json: /json/
	        },
	        responseFields: {
	            xml: "responseXML",
	            text: "responseText"
	        },
	        converters: {
	            "* text": e.String,
	            "text html": !0,
	            "text json": ue.parseJSON,
	            "text xml": ue.parseXML
	        },
	        flatOptions: {
	            url: !0,
	            context: !0
	        }
	    },
	    ajaxSetup: function (e, t) {
	        return t ? L(L(e, ue.ajaxSettings), t) : L(ue.ajaxSettings, e)
	    },
	    ajaxPrefilter: j($t),
	    ajaxTransport: j(It),
	    ajax: function (e, n) {
	        function r(e, n, r, i) {
	            var o, f, v, b, w, N = n;
	            2 !== x && (x = 2, u && clearTimeout(u), c = t, s = i || "", T.readyState = e > 0 ? 4 : 0, r && (b = H(p, T, r)), e >= 200 && 300 > e || 304 === e ? (p.ifModified && (w = T.getResponseHeader("Last-Modified"), w && (ue.lastModified[a] = w), w = T.getResponseHeader("etag"), w && (ue.etag[a] = w)), 204 === e ? (o = !0, N = "nocontent") : 304 === e ? (o = !0, N = "notmodified") : (o = O(p, b), N = o.state, f = o.data, v = o.error, o = !v)) : (v = N, (e || !N) && (N = "error", 0 > e && (e = 0))), T.status = e, T.statusText = (n || N) + "", o ? g.resolveWith(d, [f, N, T]) : g.rejectWith(d, [T, N, v]), T.statusCode(y), y = t, l && h.trigger(o ? "ajaxSuccess" : "ajaxError", [T, p, o ? f : v]), m.fireWith(d, [T, N]), l && (h.trigger("ajaxComplete", [T, p]), --ue.active || ue.event.trigger("ajaxStop")))
	        }
	        "object" == typeof e && (n = e, e = t),
			n = n || {};
	        var i, o, a, s, u, l, c, f, p = ue.ajaxSetup({},
			n),
			d = p.context || p,
			h = p.context && (d.nodeType || d.jquery) ? ue(d) : ue.event,
			g = ue.Deferred(),
			m = ue.Callbacks("once memory"),
			y = p.statusCode || {},
			v = {},
			b = {},
			x = 0,
			w = "canceled",
			T = {
			    readyState: 0,
			    getResponseHeader: function (e) {
			        var t;
			        if (2 === x) {
			            if (!f) for (f = {}; t = Ft.exec(s) ;) f[t[1].toLowerCase()] = t[2];
			            t = f[e.toLowerCase()]
			        }
			        return null == t ? null : t
			    },
			    getAllResponseHeaders: function () {
			        return 2 === x ? s : null
			    },
			    setRequestHeader: function (e, t) {
			        var n = e.toLowerCase();
			        return x || (e = b[n] = b[n] || e, v[e] = t),
					this
			    },
			    overrideMimeType: function (e) {
			        return x || (p.mimeType = e),
					this
			    },
			    statusCode: function (e) {
			        var t;
			        if (e) if (2 > x) for (t in e) y[t] = [y[t], e[t]];
			        else T.always(e[T.status]);
			        return this
			    },
			    abort: function (e) {
			        var t = e || w;
			        return c && c.abort(t),
					r(0, t),
					this
			    }
			};
	        if (g.promise(T).complete = m.add, T.success = T.done, T.error = T.fail, p.url = ((e || p.url || Lt) + "").replace(Mt, "").replace(Pt, Dt[1] + "//"), p.type = n.method || n.type || p.method || p.type, p.dataTypes = ue.trim(p.dataType || "*").toLowerCase().match(ce) || [""], null == p.crossDomain && (i = Rt.exec(p.url.toLowerCase()), p.crossDomain = !(!i || i[1] === Dt[1] && i[2] === Dt[2] && (i[3] || ("http:" === i[1] ? 80 : 443)) == (Dt[3] || ("http:" === Dt[1] ? 80 : 443)))), p.data && p.processData && "string" != typeof p.data && (p.data = ue.param(p.data, p.traditional)), D($t, p, n, T), 2 === x) return T;
	        l = p.global,
			l && 0 === ue.active++ && ue.event.trigger("ajaxStart"),
			p.type = p.type.toUpperCase(),
			p.hasContent = !Bt.test(p.type),
			a = p.url,
			p.hasContent || (p.data && (a = p.url += (Ot.test(a) ? "&" : "?") + p.data, delete p.data), p.cache === !1 && (p.url = qt.test(a) ? a.replace(qt, "$1_=" + Ht++) : a + (Ot.test(a) ? "&" : "?") + "_=" + Ht++)),
			p.ifModified && (ue.lastModified[a] && T.setRequestHeader("If-Modified-Since", ue.lastModified[a]), ue.etag[a] && T.setRequestHeader("If-None-Match", ue.etag[a])),
			(p.data && p.hasContent && p.contentType !== !1 || n.contentType) && T.setRequestHeader("Content-Type", p.contentType),
			T.setRequestHeader("Accept", p.dataTypes[0] && p.accepts[p.dataTypes[0]] ? p.accepts[p.dataTypes[0]] + ("*" !== p.dataTypes[0] ? ", " + zt + "; q=0.01" : "") : p.accepts["*"]);
	        for (o in p.headers) T.setRequestHeader(o, p.headers[o]);
	        if (p.beforeSend && (p.beforeSend.call(d, T, p) === !1 || 2 === x)) return T.abort();
	        w = "abort";
	        for (o in {
	            success: 1,
	            error: 1,
	            complete: 1
	        }) T[o](p[o]);
	        if (c = D(It, p, n, T)) {
	            T.readyState = 1,
				l && h.trigger("ajaxSend", [T, p]),
				p.async && p.timeout > 0 && (u = setTimeout(function () {
				    T.abort("timeout")
				},
				p.timeout));
	            try {
	                x = 1,
					c.send(v, r)
	            } catch (e) {
	                if (!(2 > x)) throw e;
	                r(-1, e)
	            }
	        } else r(-1, "No Transport");
	        return T
	    },
	    getScript: function (e, n) {
	        return ue.get(e, t, n, "script")
	    },
	    getJSON: function (e, t, n) {
	        return ue.get(e, t, n, "json")
	    }
	}),
	ue.ajaxSetup({
	    accepts: {
	        script: "text/javascript, application/javascript, application/ecmascript, application/x-ecmascript"
	    },
	    contents: {
	        script: /(?:java|ecma)script/
	    },
	    converters: {
	        "text script": function (e) {
	            return ue.globalEval(e),
				e
	        }
	    }
	}),
	ue.ajaxPrefilter("script",
	function (e) {
	    e.cache === t && (e.cache = !1),
		e.crossDomain && (e.type = "GET", e.global = !1)
	}),
	ue.ajaxTransport("script",
	function (e) {
	    if (e.crossDomain) {
	        var n, r = Y.head || ue("head")[0] || Y.documentElement;
	        return {
	            send: function (t, i) {
	                n = Y.createElement("script"),
					n.async = !0,
					e.scriptCharset && (n.charset = e.scriptCharset),
					n.src = e.url,
					n.onload = n.onreadystatechange = function (e, t) {
					    (t || !n.readyState || /loaded|complete/.test(n.readyState)) && (n.onload = n.onreadystatechange = null, n.parentNode && n.parentNode.removeChild(n), n = null, t || i(200, "success"))
					},
					r.insertBefore(n, r.firstChild)
	            },
	            abort: function () {
	                n && n.onload(t, !0)
	            }
	        }
	    }
	});
    var Vt = [],
	Xt = /(=)\?(?=&|$)|\?\?/;
    ue.ajaxSetup({
        jsonp: "callback",
        jsonpCallback: function () {
            var e = Vt.pop() || ue.expando + "_" + Ht++;
            return this[e] = !0,
			e
        }
    }),
	ue.ajaxPrefilter("json jsonp",
	function (n, r, i) {
	    var o, a, s, u = n.jsonp !== !1 && (Xt.test(n.url) ? "url" : "string" == typeof n.data && !(n.contentType || "").indexOf("application/x-www-form-urlencoded") && Xt.test(n.data) && "data");
	    return u || "jsonp" === n.dataTypes[0] ? (o = n.jsonpCallback = ue.isFunction(n.jsonpCallback) ? n.jsonpCallback() : n.jsonpCallback, u ? n[u] = n[u].replace(Xt, "$1" + o) : n.jsonp !== !1 && (n.url += (Ot.test(n.url) ? "&" : "?") + n.jsonp + "=" + o), n.converters["script json"] = function () {
	        return s || ue.error(o + " was not called"),
			s[0]
	    },
		n.dataTypes[0] = "json", a = e[o], e[o] = function () {
		    s = arguments
		},
		i.always(function () {
		    e[o] = a,
			n[o] && (n.jsonpCallback = r.jsonpCallback, Vt.push(o)),
			s && ue.isFunction(a) && a(s[0]),
			s = a = t
		}), "script") : t
	});
    var Ut, Yt, Jt = 0,
	Qt = e.ActiveXObject &&
	function () {
	    var e;
	    for (e in Ut) Ut[e](t, !0)
	};
    ue.ajaxSettings.xhr = e.ActiveXObject ?
	function () {
	    return !this.isLocal && M() || q()
	} : M,
	Yt = ue.ajaxSettings.xhr(),
	ue.support.cors = !!Yt && "withCredentials" in Yt,
	Yt = ue.support.ajax = !!Yt,
	Yt && ue.ajaxTransport(function (n) {
	    if (!n.crossDomain || ue.support.cors) {
	        var r;
	        return {
	            send: function (i, o) {
	                var a, s, u = n.xhr();
	                if (n.username ? u.open(n.type, n.url, n.async, n.username, n.password) : u.open(n.type, n.url, n.async), n.xhrFields) for (s in n.xhrFields) u[s] = n.xhrFields[s];
	                n.mimeType && u.overrideMimeType && u.overrideMimeType(n.mimeType),
					n.crossDomain || i["X-Requested-With"] || (i["X-Requested-With"] = "XMLHttpRequest");
	                try {
	                    for (s in i) u.setRequestHeader(s, i[s])
	                } catch (e) { }
	                u.send(n.hasContent && n.data || null),
					r = function (e, i) {
					    var s, l, c, f;
					    try {
					        if (r && (i || 4 === u.readyState)) if (r = t, a && (u.onreadystatechange = ue.noop, Qt && delete Ut[a]), i) 4 !== u.readyState && u.abort();
					        else {
					            f = {},
								s = u.status,
								l = u.getAllResponseHeaders(),
								"string" == typeof u.responseText && (f.text = u.responseText);
					            try {
					                c = u.statusText
					            } catch (e) {
					                c = ""
					            }
					            s || !n.isLocal || n.crossDomain ? 1223 === s && (s = 204) : s = f.text ? 200 : 404
					        }
					    } catch (e) {
					        i || o(-1, e)
					    }
					    f && o(s, c, f, l)
					},
					n.async ? 4 === u.readyState ? setTimeout(r) : (a = ++Jt, Qt && (Ut || (Ut = {},
					ue(e).unload(Qt)), Ut[a] = r), u.onreadystatechange = r) : r()
	            },
	            abort: function () {
	                r && r(t, !0)
	            }
	        }
	    }
	});
    var Gt, Kt, Zt = /^(?:toggle|show|hide)$/,
	en = RegExp("^(?:([+-])=|)(" + le + ")([a-z%]*)$", "i"),
	tn = /queueHooks$/,
	nn = [R],
	rn = {
	    "*": [function (e, t) {
	        var n, r, i = this.createTween(e, t),
			o = en.exec(t),
			a = i.cur(),
			s = +a || 0,
			u = 1,
			l = 20;
	        if (o) {
	            if (n = +o[2], r = o[3] || (ue.cssNumber[e] ? "" : "px"), "px" !== r && s) {
	                s = ue.css(i.elem, e, !0) || n || 1;
	                do u = u || ".5",
					s /= u,
					ue.style(i.elem, e, s + r);
	                while (u !== (u = i.cur() / a) && 1 !== u && --l)
	            }
	            i.unit = r,
				i.start = s,
				i.end = o[1] ? s + (o[1] + 1) * n : n
	        }
	        return i
	    }]
	};
    ue.Animation = ue.extend(B, {
        tweener: function (e, t) {
            ue.isFunction(e) ? (t = e, e = ["*"]) : e = e.split(" ");
            for (var n, r = 0,
			i = e.length; i > r; r++) n = e[r],
			rn[n] = rn[n] || [],
			rn[n].unshift(t)
        },
        prefilter: function (e, t) {
            t ? nn.unshift(e) : nn.push(e)
        }
    }),
	ue.Tween = W,
	W.prototype = {
	    constructor: W,
	    init: function (e, t, n, r, i, o) {
	        this.elem = e,
			this.prop = n,
			this.easing = i || "swing",
			this.options = t,
			this.start = this.now = this.cur(),
			this.end = r,
			this.unit = o || (ue.cssNumber[n] ? "" : "px")
	    },
	    cur: function () {
	        var e = W.propHooks[this.prop];
	        return e && e.get ? e.get(this) : W.propHooks._default.get(this)
	    },
	    run: function (e) {
	        var t, n = W.propHooks[this.prop];
	        return this.pos = t = this.options.duration ? ue.easing[this.easing](e, this.options.duration * e, 0, 1, this.options.duration) : e,
			this.now = (this.end - this.start) * t + this.start,
			this.options.step && this.options.step.call(this.elem, this.now, this),
			n && n.set ? n.set(this) : W.propHooks._default.set(this),
			this
	    }
	},
	W.prototype.init.prototype = W.prototype,
	W.propHooks = {
	    _default: {
	        get: function (e) {
	            var t;
	            return null == e.elem[e.prop] || e.elem.style && null != e.elem.style[e.prop] ? (t = ue.css(e.elem, e.prop, ""), t && "auto" !== t ? t : 0) : e.elem[e.prop]
	        },
	        set: function (e) {
	            ue.fx.step[e.prop] ? ue.fx.step[e.prop](e) : e.elem.style && (null != e.elem.style[ue.cssProps[e.prop]] || ue.cssHooks[e.prop]) ? ue.style(e.elem, e.prop, e.now + e.unit) : e.elem[e.prop] = e.now
	        }
	    }
	},
	W.propHooks.scrollTop = W.propHooks.scrollLeft = {
	    set: function (e) {
	        e.elem.nodeType && e.elem.parentNode && (e.elem[e.prop] = e.now)
	    }
	},
	ue.each(["toggle", "show", "hide"],
	function (e, t) {
	    var n = ue.fn[t];
	    ue.fn[t] = function (e, r, i) {
	        return null == e || "boolean" == typeof e ? n.apply(this, arguments) : this.animate(I(t, !0), e, r, i)
	    }
	}),
	ue.fn.extend({
	    fadeTo: function (e, t, n, r) {
	        return this.filter(w).css("opacity", 0).show().end().animate({
	            opacity: t
	        },
			e, n, r)
	    },
	    animate: function (e, t, n, r) {
	        var i = ue.isEmptyObject(e),
			o = ue.speed(t, n, r),
			a = function () {
			    var t = B(this, ue.extend({},
				e), o);
			    a.finish = function () {
			        t.stop(!0)
			    },
				(i || ue._data(this, "finish")) && t.stop(!0)
			};
	        return a.finish = a,
			i || o.queue === !1 ? this.each(a) : this.queue(o.queue, a)
	    },
	    stop: function (e, n, r) {
	        var i = function (e) {
	            var t = e.stop;
	            delete e.stop,
				t(r)
	        };
	        return "string" != typeof e && (r = n, n = e, e = t),
			n && e !== !1 && this.queue(e || "fx", []),
			this.each(function () {
			    var t = !0,
				n = null != e && e + "queueHooks",
				o = ue.timers,
				a = ue._data(this);
			    if (n) a[n] && a[n].stop && i(a[n]);
			    else for (n in a) a[n] && a[n].stop && tn.test(n) && i(a[n]);
			    for (n = o.length; n--;) o[n].elem !== this || null != e && o[n].queue !== e || (o[n].anim.stop(r), t = !1, o.splice(n, 1)); (t || !r) && ue.dequeue(this, e)
			})
	    },
	    finish: function (e) {
	        return e !== !1 && (e = e || "fx"),
			this.each(function () {
			    var t, n = ue._data(this),
				r = n[e + "queue"],
				i = n[e + "queueHooks"],
				o = ue.timers,
				a = r ? r.length : 0;
			    for (n.finish = !0, ue.queue(this, e, []), i && i.cur && i.cur.finish && i.cur.finish.call(this), t = o.length; t--;) o[t].elem === this && o[t].queue === e && (o[t].anim.stop(!0), o.splice(t, 1));
			    for (t = 0; a > t; t++) r[t] && r[t].finish && r[t].finish.call(this);
			    delete n.finish
			})
	    }
	}),
	ue.each({
	    slideDown: I("show"),
	    slideUp: I("hide"),
	    slideToggle: I("toggle"),
	    fadeIn: {
	        opacity: "show"
	    },
	    fadeOut: {
	        opacity: "hide"
	    },
	    fadeToggle: {
	        opacity: "toggle"
	    }
	},
	function (e, t) {
	    ue.fn[e] = function (e, n, r) {
	        return this.animate(t, e, n, r)
	    }
	}),
	ue.speed = function (e, t, n) {
	    var r = e && "object" == typeof e ? ue.extend({},
		e) : {
		    complete: n || !n && t || ue.isFunction(e) && e,
		    duration: e,
		    easing: n && t || t && !ue.isFunction(t) && t
		};
	    return r.duration = ue.fx.off ? 0 : "number" == typeof r.duration ? r.duration : r.duration in ue.fx.speeds ? ue.fx.speeds[r.duration] : ue.fx.speeds._default,
		(null == r.queue || r.queue === !0) && (r.queue = "fx"),
		r.old = r.complete,
		r.complete = function () {
		    ue.isFunction(r.old) && r.old.call(this),
			r.queue && ue.dequeue(this, r.queue)
		},
		r
	},
	ue.easing = {
	    linear: function (e) {
	        return e
	    },
	    swing: function (e) {
	        return .5 - Math.cos(e * Math.PI) / 2
	    }
	},
	ue.timers = [],
	ue.fx = W.prototype.init,
	ue.fx.tick = function () {
	    var e, n = ue.timers,
		r = 0;
	    for (Gt = ue.now() ; n.length > r; r++) e = n[r],
		e() || n[r] !== e || n.splice(r--, 1);
	    n.length || ue.fx.stop(),
		Gt = t
	},
	ue.fx.timer = function (e) {
	    e() && ue.timers.push(e) && ue.fx.start()
	},
	ue.fx.interval = 13,
	ue.fx.start = function () {
	    Kt || (Kt = setInterval(ue.fx.tick, ue.fx.interval))
	},
	ue.fx.stop = function () {
	    clearInterval(Kt),
		Kt = null
	},
	ue.fx.speeds = {
	    slow: 600,
	    fast: 200,
	    _default: 400
	},
	ue.fx.step = {},
	ue.expr && ue.expr.filters && (ue.expr.filters.animated = function (e) {
	    return ue.grep(ue.timers,
		function (t) {
		    return e === t.elem
		}).length
	}),
	ue.fn.offset = function (e) {
	    if (arguments.length) return e === t ? this : this.each(function (t) {
	        ue.offset.setOffset(this, e, t)
	    });
	    var n, r, i = {
	        top: 0,
	        left: 0
	    },
		o = this[0],
		a = o && o.ownerDocument;
	    return a ? (n = a.documentElement, ue.contains(n, o) ? (typeof o.getBoundingClientRect !== U && (i = o.getBoundingClientRect()), r = z(a), {
	        top: i.top + (r.pageYOffset || n.scrollTop) - (n.clientTop || 0),
	        left: i.left + (r.pageXOffset || n.scrollLeft) - (n.clientLeft || 0)
	    }) : i) : void 0
	},
	ue.offset = {
	    setOffset: function (e, t, n) {
	        var r = ue.css(e, "position");
	        "static" === r && (e.style.position = "relative");
	        var i, o, a = ue(e),
			s = a.offset(),
			u = ue.css(e, "top"),
			l = ue.css(e, "left"),
			c = ("absolute" === r || "fixed" === r) && ue.inArray("auto", [u, l]) > -1,
			f = {},
			p = {};
	        c ? (p = a.position(), i = p.top, o = p.left) : (i = parseFloat(u) || 0, o = parseFloat(l) || 0),
			ue.isFunction(t) && (t = t.call(e, n, s)),
			null != t.top && (f.top = t.top - s.top + i),
			null != t.left && (f.left = t.left - s.left + o),
			"using" in t ? t.using.call(e, f) : a.css(f)
	    }
	},
	ue.fn.extend({
	    position: function () {
	        if (this[0]) {
	            var e, t, n = {
	                top: 0,
	                left: 0
	            },
				r = this[0];
	            return "fixed" === ue.css(r, "position") ? t = r.getBoundingClientRect() : (e = this.offsetParent(), t = this.offset(), ue.nodeName(e[0], "html") || (n = e.offset()), n.top += ue.css(e[0], "borderTopWidth", !0), n.left += ue.css(e[0], "borderLeftWidth", !0)),
				{
				    top: t.top - n.top - ue.css(r, "marginTop", !0),
				    left: t.left - n.left - ue.css(r, "marginLeft", !0)
				}
	        }
	    },
	    offsetParent: function () {
	        return this.map(function () {
	            for (var e = this.offsetParent || Y.documentElement; e && !ue.nodeName(e, "html") && "static" === ue.css(e, "position") ;) e = e.offsetParent;
	            return e || Y.documentElement
	        })
	    }
	}),
	ue.each({
	    scrollLeft: "pageXOffset",
	    scrollTop: "pageYOffset"
	},
	function (e, n) {
	    var r = /Y/.test(n);
	    ue.fn[e] = function (i) {
	        return ue.access(this,
			function (e, i, o) {
			    var a = z(e);
			    return o === t ? a ? n in a ? a[n] : a.document.documentElement[i] : e[i] : (a ? a.scrollTo(r ? ue(a).scrollLeft() : o, r ? o : ue(a).scrollTop()) : e[i] = o, t)
			},
			e, i, arguments.length, null)
	    }
	}),
	ue.each({
	    Height: "height",
	    Width: "width"
	},
	function (e, n) {
	    ue.each({
	        padding: "inner" + e,
	        content: n,
	        "": "outer" + e
	    },
		function (r, i) {
		    ue.fn[i] = function (i, o) {
		        var a = arguments.length && (r || "boolean" != typeof i),
				s = r || (i === !0 || o === !0 ? "margin" : "border");
		        return ue.access(this,
				function (n, r, i) {
				    var o;
				    return ue.isWindow(n) ? n.document.documentElement["client" + e] : 9 === n.nodeType ? (o = n.documentElement, Math.max(n.body["scroll" + e], o["scroll" + e], n.body["offset" + e], o["offset" + e], o["client" + e])) : i === t ? ue.css(n, r, s) : ue.style(n, r, i, s)
				},
				n, a ? i : t, a, null)
		    }
		})
	}),
	e.jQuery = e.$ = ue,
	"function" == typeof define && define.amd && define.amd.jQuery && define("jquery", [],
	function () {
	    return ue
	})
}(window),
function (e) {
    if (!e.browser) {
        e.browser = {},
		e.browser.mozilla = !1,
		e.browser.webkit = !1,
		e.browser.opera = !1,
		e.browser.msie = !1;
        var t = navigator.userAgent;
        e.browser.name = navigator.appName,
		e.browser.fullVersion = "" + parseFloat(navigator.appVersion),
		e.browser.majorVersion = parseInt(navigator.appVersion, 10);
        var n, r, i; (r = t.indexOf("Opera")) != -1 ? (e.browser.opera = !0, e.browser.name = "Opera", e.browser.fullVersion = t.substring(r + 6), (r = t.indexOf("Version")) != -1 && (e.browser.fullVersion = t.substring(r + 8))) : (r = t.indexOf("MSIE")) != -1 ? (e.browser.msie = !0, e.browser.name = "Microsoft Internet Explorer", e.browser.fullVersion = t.substring(r + 5)) : (r = t.indexOf("Chrome")) != -1 ? (e.browser.webkit = !0, e.browser.name = "Chrome", e.browser.fullVersion = t.substring(r + 7)) : (r = t.indexOf("Safari")) != -1 ? (e.browser.webkit = !0, e.browser.name = "Safari", e.browser.fullVersion = t.substring(r + 7), (r = t.indexOf("Version")) != -1 && (e.browser.fullVersion = t.substring(r + 8))) : (r = t.indexOf("Firefox")) != -1 ? (e.browser.mozilla = !0, e.browser.name = "Firefox", e.browser.fullVersion = t.substring(r + 8)) : (n = t.lastIndexOf(" ") + 1) < (r = t.lastIndexOf("/")) && (e.browser.name = t.substring(n, r), e.browser.fullVersion = t.substring(r + 1), e.browser.name.toLowerCase() == e.browser.name.toUpperCase() && (e.browser.name = navigator.appName)),
		(i = e.browser.fullVersion.indexOf(";")) != -1 && (e.browser.fullVersion = e.browser.fullVersion.substring(0, i)),
		(i = e.browser.fullVersion.indexOf(" ")) != -1 && (e.browser.fullVersion = e.browser.fullVersion.substring(0, i)),
		e.browser.majorVersion = parseInt("" + e.browser.fullVersion, 10),
		isNaN(e.browser.majorVersion) && (e.browser.fullVersion = "" + parseFloat(navigator.appVersion), e.browser.majorVersion = parseInt(navigator.appVersion, 10)),
		e.browser.version = e.browser.majorVersion
    }
}(jQuery); !
function (t) {
    "use strict";
    function o(t, o) {
        return t + ".touchspin_" + o
    }
    function n(n, s) {
        return t.map(n,
		function (t) {
		    return o(t, s)
		})
    }
    var s = 0;
    t.fn.TouchSpin = function (o) {
        if ("destroy" === o) return void this.each(function () {
            var o = t(this),
			s = o.data();
            t(document).off(n(["mouseup", "touchend", "touchcancel", "mousemove", "touchmove", "scroll", "scrollstart"], s.spinnerid).join(" "))
        });
        var i = {
            min: 0,
            max: 100,
            initval: "",
            step: 1,
            decimals: 0,
            stepinterval: 100,
            forcestepdivisibility: "round",
            stepintervaldelay: 500,
            verticalbuttons: !1,
            verticalupclass: "glyphicon glyphicon-chevron-up",
            verticaldownclass: "glyphicon glyphicon-chevron-down",
            prefix: "",
            postfix: "",
            prefix_extraclass: "",
            postfix_extraclass: "",
            booster: !0,
            boostat: 10,
            maxboostedstep: !1,
            mousewheel: !0,
            buttondown_class: "btn btn-default",
            buttonup_class: "btn btn-default"
        },
		a = {
		    min: "min",
		    max: "max",
		    initval: "init-val",
		    step: "step",
		    decimals: "decimals",
		    stepinterval: "step-interval",
		    verticalbuttons: "vertical-buttons",
		    verticalupclass: "vertical-up-class",
		    verticaldownclass: "vertical-down-class",
		    forcestepdivisibility: "force-step-divisibility",
		    stepintervaldelay: "step-interval-delay",
		    prefix: "prefix",
		    postfix: "postfix",
		    prefix_extraclass: "prefix-extra-class",
		    postfix_extraclass: "postfix-extra-class",
		    booster: "booster",
		    boostat: "boostat",
		    maxboostedstep: "max-boosted-step",
		    mousewheel: "mouse-wheel",
		    buttondown_class: "button-down-class",
		    buttonup_class: "button-up-class"
		};
        return this.each(function () {
            function e() {
                if (!E.data("alreadyinitialized")) {
                    if (E.data("alreadyinitialized", !0), s += 1, E.data("spinnerid", s), !E.is("input")) return void console.log("Must be an input.");
                    r(),
					u(),
					w(),
					d(),
					b(),
					v(),
					m(),
					g(),
					P.input.css("display", "block")
                }
            }
            function u() {
                "" !== M.initval && "" === E.val() && E.val(M.initval)
            }
            function p(t) {
                l(t),
				w();
                var o = P.input.val();
                "" !== o && (o = Number(P.input.val()), P.input.val(o.toFixed(M.decimals)))
            }
            function r() {
                M = t.extend({},
				i, z, c(), o)
            }
            function c() {
                var o = {};
                return t.each(a,
				function (t, n) {
				    var s = "bts-" + n;
				    E.is("[data-" + s + "]") && (o[t] = E.data(s))
				}),
				o
            }
            function l(o) {
                M = t.extend({},
				M, o)
            }
            function d() {
                var t = E.val(),
				o = E.parent();
                "" !== t && (t = Number(t).toFixed(M.decimals)),
				E.data("initvalue", t).val(t),
				E.addClass("form-control"),
				o.hasClass("input-group") ? f(o) : h()
            }
            function f(o) {
                o.addClass("bootstrap-touchspin");
                var n, s, i = E.prev(),
				a = E.next(),
				e = '<span class="input-group-addon bootstrap-touchspin-prefix">' + M.prefix + "</span>",
				u = '<span class="input-group-addon bootstrap-touchspin-postfix">' + M.postfix + "</span>";
                i.hasClass("input-group-btn") ? (n = '<button class="' + M.buttondown_class + ' bootstrap-touchspin-down" type="button">-</button>', i.append(n)) : (n = '<span class="input-group-btn"><button class="' + M.buttondown_class + ' bootstrap-touchspin-down" type="button">-</button></span>', t(n).insertBefore(E)),
				a.hasClass("input-group-btn") ? (s = '<button class="' + M.buttonup_class + ' bootstrap-touchspin-up" type="button">+</button>', a.prepend(s)) : (s = '<span class="input-group-btn"><button class="' + M.buttonup_class + ' bootstrap-touchspin-up" type="button">+</button></span>', t(s).insertAfter(E)),
				t(e).insertBefore(E),
				t(u).insertAfter(E),
				N = o
            }
            function h() {
                var o;
                o = M.verticalbuttons ? '<div class="input-group bootstrap-touchspin"><span class="input-group-addon bootstrap-touchspin-prefix">' + M.prefix + '</span><span class="input-group-addon bootstrap-touchspin-postfix">' + M.postfix + '</span><span class="input-group-btn-vertical"><button class="' + M.buttondown_class + ' bootstrap-touchspin-up" type="button"><i class="' + M.verticalupclass + '"></i></button><button class="' + M.buttonup_class + ' bootstrap-touchspin-down" type="button"><i class="' + M.verticaldownclass + '"></i></button></span></div>' : '<div class="input-group bootstrap-touchspin"><span class="input-group-btn"><button class="' + M.buttondown_class + ' bootstrap-touchspin-down" type="button">-</button></span><span class="input-group-addon bootstrap-touchspin-prefix">' + M.prefix + '</span><span class="input-group-addon bootstrap-touchspin-postfix">' + M.postfix + '</span><span class="input-group-btn"><button class="' + M.buttonup_class + ' bootstrap-touchspin-up" type="button">+</button></span></div>',
				N = t(o).insertBefore(E),
				t(".bootstrap-touchspin-prefix", N).after(E),
				E.hasClass("input-sm") ? N.addClass("input-group-sm") : E.hasClass("input-lg") && N.addClass("input-group-lg")
            }
            function b() {
                P = {
                    down: t(".bootstrap-touchspin-down", N),
                    up: t(".bootstrap-touchspin-up", N),
                    input: t("input", N),
                    prefix: t(".bootstrap-touchspin-prefix", N).addClass(M.prefix_extraclass),
                    postfix: t(".bootstrap-touchspin-postfix", N).addClass(M.postfix_extraclass)
                }
            }
            function v() {
                "" === M.prefix && P.prefix.hide(),
				"" === M.postfix && P.postfix.hide()
            }
            function m() {
                E.on("keydown",
				function (t) {
				    var o = t.keyCode || t.which;
				    38 === o ? ("up" !== O && (_(), k()), t.preventDefault()) : 40 === o && ("down" !== O && (C(), D()), t.preventDefault())
				}),
				E.on("keyup",
				function (t) {
				    var o = t.keyCode || t.which;
				    38 === o ? F() : 40 === o && F()
				}),
				E.on("blur",
				function () {
				    w()
				}),
				P.down.on("keydown",
				function (t) {
				    var o = t.keyCode || t.which; (32 === o || 13 === o) && ("down" !== O && (C(), D()), t.preventDefault())
				}),
				P.down.on("keyup",
				function (t) {
				    var o = t.keyCode || t.which; (32 === o || 13 === o) && F()
				}),
				P.up.on("keydown",
				function (t) {
				    var o = t.keyCode || t.which; (32 === o || 13 === o) && ("up" !== O && (_(), k()), t.preventDefault())
				}),
				P.up.on("keyup",
				function (t) {
				    var o = t.keyCode || t.which; (32 === o || 13 === o) && F()
				}),
				P.down.on("mousedown.touchspin",
				function (t) {
				    P.down.off("touchstart.touchspin"),
					E.is(":disabled") || (C(), D(), t.preventDefault(), t.stopPropagation())
				}),
				P.down.on("touchstart.touchspin",
				function (t) {
				    P.down.off("mousedown.touchspin"),
					E.is(":disabled") || (C(), D(), t.preventDefault(), t.stopPropagation())
				}),
				P.up.on("mousedown.touchspin",
				function (t) {
				    P.up.off("touchstart.touchspin"),
					E.is(":disabled") || (_(), k(), t.preventDefault(), t.stopPropagation())
				}),
				P.up.on("touchstart.touchspin",
				function (t) {
				    P.up.off("mousedown.touchspin"),
					E.is(":disabled") || (_(), k(), t.preventDefault(), t.stopPropagation())
				}),
				P.up.on("mouseout touchleave touchend touchcancel",
				function (t) {
				    O && (t.stopPropagation(), F())
				}),
				P.down.on("mouseout touchleave touchend touchcancel",
				function (t) {
				    O && (t.stopPropagation(), F())
				}),
				P.down.on("mousemove touchmove",
				function (t) {
				    O && (t.stopPropagation(), t.preventDefault())
				}),
				P.up.on("mousemove touchmove",
				function (t) {
				    O && (t.stopPropagation(), t.preventDefault())
				}),
				t(document).on(n(["mouseup", "touchend", "touchcancel"], s).join(" "),
				function (t) {
				    O && (t.preventDefault(), F())
				}),
				t(document).on(n(["mousemove", "touchmove", "scroll", "scrollstart"], s).join(" "),
				function (t) {
				    O && (t.preventDefault(), F())
				}),
				E.on("mousewheel DOMMouseScroll",
				function (t) {
				    if (M.mousewheel && E.is(":focus")) {
				        var o = t.originalEvent.wheelDelta || -t.originalEvent.deltaY || -t.originalEvent.detail;
				        t.stopPropagation(),
						t.preventDefault(),
						0 > o ? C() : _()
				    }
				})
            }
            function g() {
                E.on("touchspin.uponce",
				function () {
				    F(),
					_()
				}),
				E.on("touchspin.downonce",
				function () {
				    F(),
					C()
				}),
				E.on("touchspin.startupspin",
				function () {
				    k()
				}),
				E.on("touchspin.startdownspin",
				function () {
				    D()
				}),
				E.on("touchspin.stopspin",
				function () {
				    F()
				}),
				E.on("touchspin.updatesettings",
				function (t, o) {
				    p(o)
				})
            }
            function x(t) {
                switch (M.forcestepdivisibility) {
                    case "round":
                        return (Math.round(t / M.step) * M.step).toFixed(M.decimals);
                    case "floor":
                        return (Math.floor(t / M.step) * M.step).toFixed(M.decimals);
                    case "ceil":
                        return (Math.ceil(t / M.step) * M.step).toFixed(M.decimals);
                    default:
                        return t
                }
            }
            function w() {
                var t, o, n;
                t = E.val(),
				"" !== t && (M.decimals > 0 && "." === t || (o = parseFloat(t), isNaN(o) && (o = 0), n = o, o.toString() !== t && (n = o), o < M.min && (n = M.min), o > M.max && (n = M.max), n = x(n), Number(t).toString() !== n.toString() && (E.val(n), E.trigger("change"))))
            }
            function y() {
                if (M.booster) {
                    var t = Math.pow(2, Math.floor(A / M.boostat)) * M.step;
                    return M.maxboostedstep && t > M.maxboostedstep && (t = M.maxboostedstep, S = Math.round(S / t) * t),
					Math.max(M.step, t)
                }
                return M.step
            }
            function _() {
                w(),
				S = parseFloat(P.input.val()),
				isNaN(S) && (S = 0);
                var t = S,
				o = y();
                S += o,
				S > M.max && (S = M.max, E.trigger("touchspin.on.max"), F()),
				P.input.val(Number(S).toFixed(M.decimals)),
				t !== S && E.trigger("change")
            }
            function C() {
                w(),
				S = parseFloat(P.input.val()),
				isNaN(S) && (S = 0);
                var t = S,
				o = y();
                S -= o,
				S < M.min && (S = M.min, E.trigger("touchspin.on.min"), F()),
				P.input.val(S.toFixed(M.decimals)),
				t !== S && E.trigger("change")
            }
            function D() {
                F(),
				A = 0,
				O = "down",
				E.trigger("touchspin.on.startspin"),
				E.trigger("touchspin.on.startdownspin"),
				I = setTimeout(function () {
				    T = setInterval(function () {
				        A++,
						C()
				    },
					M.stepinterval)
				},
				M.stepintervaldelay)
            }
            function k() {
                F(),
				A = 0,
				O = "up",
				E.trigger("touchspin.on.startspin"),
				E.trigger("touchspin.on.startupspin"),
				B = setTimeout(function () {
				    j = setInterval(function () {
				        A++,
						_()
				    },
					M.stepinterval)
				},
				M.stepintervaldelay)
            }
            function F() {
                switch (clearTimeout(I), clearTimeout(B), clearInterval(T), clearInterval(j), O) {
                    case "up":
                        E.trigger("touchspin.on.stopupspin"),
                        E.trigger("touchspin.on.stopspin");
                        break;
                    case "down":
                        E.trigger("touchspin.on.stopdownspin"),
                        E.trigger("touchspin.on.stopspin")
                }
                A = 0,
				O = !1
            }
            var M, N, P, S, T, j, I, B, E = t(this),
			z = E.data(),
			A = 0,
			O = !1;
            e()
        })
    }
}(jQuery); !
function (e, t) {
    "function" == typeof define && define.amd ? define(["jquery"],
	function (e) {
	    return t(e)
	}) : "object" == typeof exports ? module.exports = t(require("jquery")) : t(jQuery)
}(this,
function (e) {
    !
        function ($) {
            "use strict";
            function e(e) {
                var t = [{
                    re: /[\xC0-\xC6]/g,
                    ch: "A"
                },
                {
                    re: /[\xE0-\xE6]/g,
                    ch: "a"
                },
                {
                    re: /[\xC8-\xCB]/g,
                    ch: "E"
                },
                {
                    re: /[\xE8-\xEB]/g,
                    ch: "e"
                },
                {
                    re: /[\xCC-\xCF]/g,
                    ch: "I"
                },
                {
                    re: /[\xEC-\xEF]/g,
                    ch: "i"
                },
                {
                    re: /[\xD2-\xD6]/g,
                    ch: "O"
                },
                {
                    re: /[\xF2-\xF6]/g,
                    ch: "o"
                },
                {
                    re: /[\xD9-\xDC]/g,
                    ch: "U"
                },
                {
                    re: /[\xF9-\xFC]/g,
                    ch: "u"
                },
                {
                    re: /[\xC7-\xE7]/g,
                    ch: "c"
                },
                {
                    re: /[\xD1]/g,
                    ch: "N"
                },
                {
                    re: /[\xF1]/g,
                    ch: "n"
                }];
                return $.each(t,
                function () {
                    e = e.replace(this.re, this.ch)
                }),
                e
            }
            function t(e) {
                var t = {
                    "&": "&amp;",
                    "<": "&lt;",
                    ">": "&gt;",
                    '"': "&quot;",
                    "'": "&#x27;",
                    "`": "&#x60;"
                },
                n = "(?:" + Object.keys(t).join("|") + ")",
                i = new RegExp(n),
                s = new RegExp(n, "g"),
                o = null == e ? "" : "" + e;
                return i.test(o) ? o.replace(s,
                function (e) {
                    return t[e]
                }) : o
            }
            function n(e, t) {
                var n = arguments,
                s = e,
                o = t;[].shift.apply(n);
                var a, l = this.each(function () {
                    var e = $(this);
                    if (e.is("select")) {
                        var t = e.data("selectpicker"),
                        l = "object" == typeof s && s;
                        if (t) {
                            if (l) for (var r in l) l.hasOwnProperty(r) && (t.options[r] = l[r])
                        } else {
                            var d = $.extend({},
                            i.DEFAULTS, $.fn.selectpicker.defaults || {},
                            e.data(), l);
                            d.template = $.extend({},
                            i.DEFAULTS.template, $.fn.selectpicker.defaults ? $.fn.selectpicker.defaults.template : {},
                            e.data().template, l.template),
                            e.data("selectpicker", t = new i(this, d, o))
                        }
                        "string" == typeof s && (a = t[s] instanceof Function ? t[s].apply(t, n) : t.options[s])
                    }
                });
                return "undefined" != typeof a ? a : l
            }
            String.prototype.includes || !
            function () {
                var e = {}.toString,
                t = function () {
                    try {
                        var e = {},
                        t = Object.defineProperty,
                        n = t(e, e, e) && t
                    } catch (e) { }
                    return n
                }(),
                n = "".indexOf,
                i = function (t) {
                    if (null == this) throw new TypeError;
                    var i = String(this);
                    if (t && "[object RegExp]" == e.call(t)) throw new TypeError;
                    var s = i.length,
                    o = String(t),
                    a = o.length,
                    l = arguments.length > 1 ? arguments[1] : void 0,
                    r = l ? Number(l) : 0;
                    r != r && (r = 0);
                    var d = Math.min(Math.max(r, 0), s);
                    return !(a + d > s) && n.call(i, o, r) != -1
                };
                t ? t(String.prototype, "includes", {
                    value: i,
                    configurable: !0,
                    writable: !0
                }) : String.prototype.includes = i
            }(),
            String.prototype.startsWith || !
            function () {
                var e = function () {
                    try {
                        var e = {},
                        t = Object.defineProperty,
                        n = t(e, e, e) && t
                    } catch (e) { }
                    return n
                }(),
                t = {}.toString,
                n = function (e) {
                    if (null == this) throw new TypeError;
                    var n = String(this);
                    if (e && "[object RegExp]" == t.call(e)) throw new TypeError;
                    var i = n.length,
                    s = String(e),
                    o = s.length,
                    a = arguments.length > 1 ? arguments[1] : void 0,
                    l = a ? Number(a) : 0;
                    l != l && (l = 0);
                    var r = Math.min(Math.max(l, 0), i);
                    if (o + r > i) return !1;
                    for (var d = -1; ++d < o;) if (n.charCodeAt(r + d) != s.charCodeAt(d)) return !1;
                    return !0
                };
                e ? e(String.prototype, "startsWith", {
                    value: n,
                    configurable: !0,
                    writable: !0
                }) : String.prototype.startsWith = n
            }(),
            Object.keys || (Object.keys = function (e, t, n) {
                n = [];
                for (t in e) n.hasOwnProperty.call(e, t) && n.push(t);
                return n
            }),
            $.fn.triggerNative = function (e) {
                var t, n = this[0];
                n.dispatchEvent ? ("function" == typeof Event ? t = new Event(e, {
                    bubbles: !0
                }) : (t = document.createEvent("Event"), t.initEvent(e, !0, !1)), n.dispatchEvent(t)) : (n.fireEvent && (t = document.createEventObject(), t.eventType = e, n.fireEvent("on" + e, t)), this.trigger(e))
            },
            $.expr[":"].icontains = function (e, t, n) {
                var i = $(e),
                s = (i.data("tokens") || i.text()).toUpperCase();
                return s.includes(n[3].toUpperCase())
            },
            $.expr[":"].ibegins = function (e, t, n) {
                var i = $(e),
                s = (i.data("tokens") || i.text()).toUpperCase();
                return s.startsWith(n[3].toUpperCase())
            },
            $.expr[":"].aicontains = function (e, t, n) {
                var i = $(e),
                s = (i.data("tokens") || i.data("normalizedText") || i.text()).toUpperCase();
                return s.includes(n[3].toUpperCase())
            },
            $.expr[":"].aibegins = function (e, t, n) {
                var i = $(e),
                s = (i.data("tokens") || i.data("normalizedText") || i.text()).toUpperCase();
                return s.startsWith(n[3].toUpperCase())
            };
            var i = function (e, t, n) {
                n && (n.stopPropagation(), n.preventDefault()),
                this.$element = $(e),
                this.$newElement = null,
                this.$button = null,
                this.$menu = null,
                this.$lis = null,
                this.options = t,
                null === this.options.title && (this.options.title = this.$element.attr("title")),
                this.val = i.prototype.val,
                this.render = i.prototype.render,
                this.refresh = i.prototype.refresh,
                this.setStyle = i.prototype.setStyle,
                this.selectAll = i.prototype.selectAll,
                this.deselectAll = i.prototype.deselectAll,
                this.destroy = i.prototype.destroy,
                this.remove = i.prototype.remove,
                this.show = i.prototype.show,
                this.hide = i.prototype.hide,
                this.init()
            };
            i.VERSION = "1.9.3",
            i.DEFAULTS = {
                noneSelectedText: "Nothing selected",
                noneResultsText: "No results matched {0}",
                countSelectedText: function (e, t) {
                    return 1 == e ? "{0} item selected" : "{0} items selected"
                },
                maxOptionsText: function (e, t) {
                    return [1 == e ? "Limit reached ({n} item max)" : "Limit reached ({n} items max)", 1 == t ? "Group limit reached ({n} item max)" : "Group limit reached ({n} items max)"]
                },
                selectAllText: "Select All",
                deselectAllText: "Deselect All",
                doneButton: !1,
                doneButtonText: "Close",
                multipleSeparator: ", ",
                styleBase: "btn btn-sm",
                style: "btn-default",
                size: "auto",
                title: null,
                selectedTextFormat: "values",
                width: !1,
                container: !1,
                hideDisabled: !1,
                showSubtext: !1,
                showIcon: !0,
                showContent: !0,
                dropupAuto: !0,
                header: !1,
                liveSearch: !1,
                liveSearchPlaceholder: null,
                liveSearchNormalize: !1,
                liveSearchStyle: "contains",
                actionsBox: !1,
                iconBase: "glyphicon",
                tickIcon: "glyphicon-ok",
                template: {
                    caret: '<span class="caret"></span>'
                },
                maxOptions: !1,
                mobile: !1,
                selectOnTab: !1,
                dropdownAlignRight: !1
            },
            i.prototype = {
                constructor: i,
                init: function () {
                    var e = this,
                    t = this.$element.attr("id");
                    this.liObj = {},
                    this.multiple = this.$element.prop("multiple"),
                    this.autofocus = this.$element.prop("autofocus"),
                    this.$newElement = this.createView(),
                    this.$element.after(this.$newElement).appendTo(this.$newElement),
                    this.$button = this.$newElement.children("button"),
                    this.$menu = this.$newElement.children(".dropdown-menu"),
                    this.$menuInner = this.$menu.children(".inner"),
                    this.$searchbox = this.$menu.find("input"),
                    this.options.dropdownAlignRight && this.$menu.addClass("dropdown-menu-right"),
                    "undefined" != typeof t && (this.$button.attr("data-id", t), $('label[for="' + t + '"]').click(function (t) {
                        t.preventDefault(),
                        e.$button.focus()
                    })),
                    this.checkDisabled(),
                    this.clickListener(),
                    this.options.liveSearch && this.liveSearchListener(),
                    this.render(),
                    this.setStyle(),
                    this.setWidth(),
                    this.options.container && this.selectPosition(),
                    this.$menu.data("this", this),
                    this.$newElement.data("this", this),
                    this.options.mobile && this.mobile(),
                    this.$newElement.on({
                        "hide.bs.dropdown": function (t) {
                            e.$element.trigger("hide.bs.select", t)
                        },
                        "hidden.bs.dropdown": function (t) {
                            e.$element.trigger("hidden.bs.select", t)
                        },
                        "show.bs.dropdown": function (t) {
                            e.$element.trigger("show.bs.select", t)
                        },
                        "shown.bs.dropdown": function (t) {
                            e.$element.trigger("shown.bs.select", t)
                        }
                    }),
                    e.$element[0].hasAttribute("required") && this.$element.on("invalid",
                    function () {
                        e.$button.addClass("bs-invalid").focus(),
                        e.$element.on({
                            "focus.bs.select": function () {
                                e.$button.focus(),
                                e.$element.off("focus.bs.select")
                            },
                            "shown.bs.select": function () {
                                e.$element.val(e.$element.val()).off("shown.bs.select")
                            },
                            "rendered.bs.select": function () {
                                this.validity.valid && e.$button.removeClass("bs-invalid"),
                                e.$element.off("rendered.bs.select")
                            }
                        })
                    }),
                    setTimeout(function () {
                        e.$element.trigger("loaded.bs.select")
                    })
                },
                createDropdown: function () {
                    var e = this.multiple ? " show-tick" : "",
                    n = this.$element.parent().hasClass("input-group") ? " input-group-btn" : "",
                    i = this.autofocus ? " autofocus" : "",
                    s = this.options.header ? '<div class="popover-title"><button type="button" class="close" aria-hidden="true">&times;</button>' + this.options.header + "</div>" : "",
                    o = this.options.liveSearch ? '<div class="bs-searchbox"><input type="text" class="form-control" autocomplete="off"' + (null === this.options.liveSearchPlaceholder ? "" : ' placeholder="' + t(this.options.liveSearchPlaceholder) + '"') + "></div>" : "",
                    a = this.multiple && this.options.actionsBox ? '<div class="bs-actionsbox"><div class="btn-group btn-group-sm btn-block"><button type="button" class="actions-btn bs-select-all btn btn-default">' + this.options.selectAllText + '</button><button type="button" class="actions-btn bs-deselect-all btn btn-default">' + this.options.deselectAllText + "</button></div></div>" : "",
                    l = this.multiple && this.options.doneButton ? '<div class="bs-donebutton"><div class="btn-group btn-block"><button type="button" class="btn btn-sm btn-default">' + this.options.doneButtonText + "</button></div></div>" : "",
                    r = '<div class="btn-group bootstrap-select user-input ' + e + n + '"><button type="button" class="' + this.options.styleBase + ' dropdown-toggle" data-toggle="dropdown"' + i + '><span class="filter-option pull-left d-i-b cut-out"></span>&nbsp;<span class="bs-caret">' + this.options.template.caret + '</span></button><div class="dropdown-menu open">' + s + o + a + '<ul class="dropdown-menu inner" role="menu"></ul>' + l + "</div></div>";
                    return $(r)
                },
                createView: function () {
                    var e = this.createDropdown(),
                    t = this.createLi();
                    return e.find("ul")[0].innerHTML = t,
                    e
                },
                reloadLi: function () {
                    this.destroyLi();
                    var e = this.createLi();
                    this.$menuInner[0].innerHTML = e
                },
                destroyLi: function () {
                    this.$menu.find("li").remove()
                },
                createLi: function () {
                    var n = this,
                    i = [],
                    s = 0,
                    o = document.createElement("option"),
                    a = -1,
                    l = function (e, t, n, i) {
                        return "<li" + ("undefined" != typeof n & "" !== n ? ' class="' + n + '"' : "") + ("undefined" != typeof t & null !== t ? ' data-original-index="' + t + '"' : "") + ("undefined" != typeof i & null !== i ? 'data-optgroup="' + i + '"' : "") + ">" + e + "</li>"
                    },
                    r = function (i, s, o, a) {
                        return '<a tabindex="0"' + ("undefined" != typeof s ? ' class="' + s + '"' : "") + ("undefined" != typeof o ? ' style="' + o + '"' : "") + (n.options.liveSearchNormalize ? ' data-normalized-text="' + e(t(i)) + '"' : "") + ("undefined" != typeof a || null !== a ? ' data-tokens="' + a + '"' : "") + ">" + i + '<span class="' + n.options.iconBase + " " + n.options.tickIcon + ' check-mark"></span></a>'
                    };
                    if (this.options.title && !this.multiple && (a--, !this.$element.find(".bs-title-option").length)) {
                        var d = this.$element[0];
                        o.className = "bs-title-option",
                        o.appendChild(document.createTextNode(this.options.title)),
                        o.value = "",
                        d.insertBefore(o, d.firstChild),
                        void 0 === $(d.options[d.selectedIndex]).attr("selected") && (o.selected = !0)
                    }
                    return this.$element.find("option").each(function (e) {
                        var t = $(this);
                        if (a++, !t.hasClass("bs-title-option")) {
                            var o = this.className || "",
                            d = this.style.cssText,
                            h = t.data("content") ? t.data("content") : t.html(),
                            c = t.data("tokens") ? t.data("tokens") : null,
                            p = "undefined" != typeof t.data("subtext") ? '<small class="text-muted">' + t.data("subtext") + "</small>" : "",
                            u = "undefined" != typeof t.data("icon") ? '<span class="' + n.options.iconBase + " " + t.data("icon") + '"></span> ' : "",
                            f = this.disabled || "OPTGROUP" === this.parentNode.tagName && this.parentNode.disabled;
                            if ("" !== u && f && (u = "<span>" + u + "</span>"), n.options.hideDisabled && f) return void a--;
                            if (t.data("content") || (h = u + '<span class="text">' + h + p + "</span>"), "OPTGROUP" === this.parentNode.tagName && t.data("divider") !== !0) {
                                var m = " " + this.parentNode.className || "";
                                if (0 === t.index()) {
                                    s += 1;
                                    var v = this.parentNode.label,
                                    b = "undefined" != typeof t.parent().data("subtext") ? '<small class="text-muted">' + t.parent().data("subtext") + "</small>" : "",
                                    g = t.parent().data("icon") ? '<span class="' + n.options.iconBase + " " + t.parent().data("icon") + '"></span> ' : "";
                                    v = g + '<span class="text">' + v + b + "</span>",
                                    0 !== e && i.length > 0 && (a++, i.push(l("", null, "divider", s + "div"))),
                                    a++,
                                    i.push(l(v, null, "dropdown-header" + m, s))
                                }
                                i.push(l(r(h, "opt " + o + m, d, c), e, "", s))
                            } else t.data("divider") === !0 ? i.push(l("", e, "divider")) : t.data("hidden") === !0 ? i.push(l(r(h, o, d, c), e, "hidden is-hidden")) : (this.previousElementSibling && "OPTGROUP" === this.previousElementSibling.tagName && (a++, i.push(l("", null, "divider", s + "div"))), i.push(l(r(h, o, d, c), e)));
                            n.liObj[e] = a
                        }
                    }),
                    this.multiple || 0 !== this.$element.find("option:selected").length || this.options.title || this.$element.find("option").eq(0).prop("selected", !0).attr("selected", "selected"),
                    i.join("")
                },
                findLis: function () {
                    return null == this.$lis && (this.$lis = this.$menu.find("li")),
                    this.$lis
                },
                render: function (e) {
                    var t, n = this;
                    e !== !1 && this.$element.find("option").each(function (e) {
                        var t = n.findLis().eq(n.liObj[e]);
                        n.setDisabled(e, this.disabled || "OPTGROUP" === this.parentNode.tagName && this.parentNode.disabled, t),
                        n.setSelected(e, this.selected, t)
                    }),
                    this.tabIndex();
                    var i = this.$element.find("option").map(function () {
                        if (this.selected) {
                            if (n.options.hideDisabled && (this.disabled || "OPTGROUP" === this.parentNode.tagName && this.parentNode.disabled)) return;
                            var e, t = $(this),
                            i = t.data("icon") && n.options.showIcon ? '<i class="' + n.options.iconBase + " " + t.data("icon") + '"></i> ' : "";
                            return e = n.options.showSubtext && t.data("subtext") && !n.multiple ? ' <small class="text-muted">' + t.data("subtext") + "</small>" : "",
                            "undefined" != typeof t.attr("title") ? t.attr("title") : t.data("content") && n.options.showContent ? t.data("content") : i + t.html() + e
                        }
                    }).toArray(),
                    s = this.multiple ? i.join(this.options.multipleSeparator) : i[0];
                    if (this.multiple && this.options.selectedTextFormat.indexOf("count") > -1) {
                        var o = this.options.selectedTextFormat.split(">");
                        if (o.length > 1 && i.length > o[1] || 1 == o.length && i.length >= 2) {
                            t = this.options.hideDisabled ? ", [disabled]" : "";
                            var a = this.$element.find("option").not('[data-divider="true"], [data-hidden="true"]' + t).length,
                            l = "function" == typeof this.options.countSelectedText ? this.options.countSelectedText(i.length, a) : this.options.countSelectedText;
                            s = l.replace("{0}", i.length.toString()).replace("{1}", a.toString())
                        }
                    }
                    void 0 == this.options.title && (this.options.title = this.$element.attr("title")),
                    "static" == this.options.selectedTextFormat && (s = this.options.title),
                    s || (s = "undefined" != typeof this.options.title ? this.options.title : this.options.noneSelectedText),
                    this.$button.children(".filter-option").html(s),
                    this.$element.trigger("rendered.bs.select")
                },
                setStyle: function (e, t) {
                    this.$element.attr("class") && this.$newElement.addClass(this.$element.attr("class").replace(/selectpicker|mobile-device|bs-select-hidden|validate\[.*\]/gi, "")),
                    this.$element.attr("style") && this.$newElement.attr("style", this.$element.attr("style")),
                    this.$element.attr("onclick") && this.$newElement.attr("onclick", this.$element.attr("onclick")),
                    this.$element.attr("onmouseleave") && this.$newElement.attr("onmouseleave", this.$element.attr("onmouseleave")),
                    this.$element.attr("onmouseenter") && this.$newElement.attr("onmouseenter", this.$element.attr("onmouseenter"));
                    var n = e ? e : this.options.style;
                    "add" == t ? this.$button.addClass(n) : "remove" == t ? this.$button.removeClass(n) : (this.$button.removeClass(this.options.style), this.$button.addClass(n))
                },
                liHeight: function (e) {
                    if (e || this.options.size !== !1 && !this.sizeInfo) {
                        var t = document.createElement("div"),
                        n = document.createElement("div"),
                        i = document.createElement("ul"),
                        s = document.createElement("li"),
                        o = document.createElement("li"),
                        a = document.createElement("a"),
                        l = document.createElement("span"),
                        r = this.options.header && this.$menu.find(".popover-title").length > 0 ? this.$menu.find(".popover-title")[0].cloneNode(!0) : null,
                        d = this.options.liveSearch ? document.createElement("div") : null,
                        h = this.options.actionsBox && this.multiple && this.$menu.find(".bs-actionsbox").length > 0 ? this.$menu.find(".bs-actionsbox")[0].cloneNode(!0) : null,
                        c = this.options.doneButton && this.multiple && this.$menu.find(".bs-donebutton").length > 0 ? this.$menu.find(".bs-donebutton")[0].cloneNode(!0) : null;
                        if (l.className = "text", t.className = this.$menu[0].parentNode.className + " open", n.className = "dropdown-menu open", i.className = "dropdown-menu inner", s.className = "divider", l.appendChild(document.createTextNode("Inner text")), a.appendChild(l), o.appendChild(a), i.appendChild(o), i.appendChild(s), r && n.appendChild(r), d) {
                            var p = document.createElement("span");
                            d.className = "bs-searchbox",
                            p.className = "form-control",
                            d.appendChild(p),
                            n.appendChild(d)
                        }
                        h && n.appendChild(h),
                        n.appendChild(i),
                        c && n.appendChild(c),
                        t.appendChild(n),
                        document.body.appendChild(t);
                        var u = a.offsetHeight,
                        f = r ? r.offsetHeight : 0,
                        m = d ? d.offsetHeight : 0,
                        v = h ? h.offsetHeight : 0,
                        b = c ? c.offsetHeight : 0,
                        g = $(s).outerHeight(!0),
                        x = "function" == typeof getComputedStyle && getComputedStyle(n),
                        y = x ? null : $(n),
                        w = parseInt(x ? x.paddingTop : y.css("paddingTop")) + parseInt(x ? x.paddingBottom : y.css("paddingBottom")) + parseInt(x ? x.borderTopWidth : y.css("borderTopWidth")) + parseInt(x ? x.borderBottomWidth : y.css("borderBottomWidth")),
                        C = w + parseInt(x ? x.marginTop : y.css("marginTop")) + parseInt(x ? x.marginBottom : y.css("marginBottom")) + 2;
                        document.body.removeChild(t),
                        this.sizeInfo = {
                            liHeight: u,
                            headerHeight: f,
                            searchHeight: m,
                            actionsHeight: v,
                            doneButtonHeight: b,
                            dividerHeight: g,
                            menuPadding: w,
                            menuExtras: C
                        }
                    }
                },
                setSize: function () {
                    if (this.findLis(), this.liHeight(), this.options.header && this.$menu.css("padding-top", 0), this.options.size !== !1) {
                        var e, t, n, i, s = this,
                        o = this.$menu,
                        a = this.$menuInner,
                        l = $(window),
                        r = this.$newElement[0].offsetHeight,
                        d = this.sizeInfo.liHeight,
                        h = this.sizeInfo.headerHeight,
                        c = this.sizeInfo.searchHeight,
                        p = this.sizeInfo.actionsHeight,
                        u = this.sizeInfo.doneButtonHeight,
                        f = this.sizeInfo.dividerHeight,
                        m = this.sizeInfo.menuPadding,
                        v = this.sizeInfo.menuExtras,
                        b = this.options.hideDisabled ? ".disabled" : "",
                        g = function () {
                            n = s.$newElement.offset().top - l.scrollTop(),
                            i = l.height() - n - r
                        };
                        if (g(), "auto" === this.options.size) {
                            var x = function () {
                                var l, r = function (e, t) {
                                    return function (n) {
                                        return t ? n.classList ? n.classList.contains(e) : $(n).hasClass(e) : !(n.classList ? n.classList.contains(e) : $(n).hasClass(e))
                                    }
                                },
                                f = s.$menuInner[0].getElementsByTagName("li"),
                                b = Array.prototype.filter ? Array.prototype.filter.call(f, r("hidden", !1)) : s.$lis.not(".hidden"),
                                x = Array.prototype.filter ? Array.prototype.filter.call(b, r("dropdown-header", !0)) : b.filter(".dropdown-header");
                                g(),
                                e = i - v,
                                s.options.container ? (o.data("height") || o.data("height", o.height()), t = o.data("height")) : t = o.height(),
                                s.options.dropupAuto && s.$newElement.toggleClass("dropup", n > i && e - v < t),
                                s.$newElement.hasClass("dropup") && (e = n - v),
                                l = b.length + x.length > 3 ? 3 * d + v - 2 : 0,
                                o.css({
                                    "z-index": "999999",
                                    "max-height": (e <= 260 ? e : 260) + "px",
                                    overflow: "hidden",
                                    "min-height": l + h + c + p + u + "px"
                                }),
                                a.css({
                                    "max-height": (e <= 260 ? e : 260) - h - c - p - u - m + "px",
                                    "overflow-y": "auto",
                                    "min-height": Math.max(l - m, 0) + "px"
                                })
                            };
                            x(),
                            this.$searchbox.off("input.getSize propertychange.getSize").on("input.getSize propertychange.getSize", x),
                            l.off("resize.getSize scroll.getSize").on("resize.getSize scroll.getSize", x)
                        } else if (this.options.size && "auto" != this.options.size && this.$lis.not(b).length > this.options.size) {
                            var y = this.$lis.not(".divider").not(b).children().slice(0, this.options.size).last().parent().index(),
                            w = this.$lis.slice(0, y + 1).filter(".divider").length;
                            e = d * this.options.size + w * f + m,
                            s.options.container ? (o.data("height") || o.data("height", o.height()), t = o.data("height")) : t = o.height(),
                            s.options.dropupAuto && this.$newElement.toggleClass("dropup", n > i && e - v < t),
                            o.css({
                                "max-height": e + h + c + p + u + "px",
                                overflow: "hidden",
                                "min-height": ""
                            }),
                            a.css({
                                "max-height": e - m + "px",
                                "overflow-y": "auto",
                                "min-height": ""
                            })
                        }
                    }
                },
                setWidth: function () {
                    if ("auto" === this.options.width) {
                        this.$menu.css("min-width", "0");
                        var e = this.$menu.parent().clone().appendTo("body"),
                        t = this.options.container ? this.$newElement.clone().appendTo("body") : e,
                        n = e.children(".dropdown-menu").outerWidth(),
                        i = t.css("width", "auto").children("button").outerWidth();
                        e.remove(),
                        t.remove(),
                        this.$newElement.css("width", Math.max(n, i) + "px")
                    } else "fit" === this.options.width ? (this.$menu.css("min-width", ""), this.$newElement.css("width", "").addClass("fit-width")) : this.options.width ? (this.$menu.css("min-width", ""), this.$newElement.css("width", this.options.width)) : this.$menu.css("min-width", "");
                    this.$newElement.hasClass("fit-width") && "fit" !== this.options.width && this.$newElement.removeClass("fit-width")
                },
                selectPosition: function () {
                    this.$bsContainer = $('<div class="bs-container" />');
                    var e, t, n = this,
                    i = function (i) {
                        n.$bsContainer.addClass(i.attr("class").replace(/form-control|fit-width/gi, "")).toggleClass("dropup", i.hasClass("dropup")),
                        e = i.offset(),
                        t = i.hasClass("dropup") ? 0 : i[0].offsetHeight,
                        n.$bsContainer.css({
                            top: e.top + t,
                            left: e.left,
                            width: i[0].offsetWidth
                        })
                    };
                    this.$button.on("click",
                    function () {
                        var e = $(this);
                        n.isDisabled() || (i(n.$newElement), n.$bsContainer.appendTo(n.options.container).toggleClass("open", !e.hasClass("open")).append(n.$menu))
                    }),
                    $(window).on("resize scroll",
                    function () {
                        i(n.$newElement)
                    }),
                    this.$element.on("hide.bs.select",
                    function () {
                        n.$menu.data("height", n.$menu.height()),
                        n.$bsContainer.detach()
                    })
                },
                setSelected: function (e, t, n) {
                    n || (n = this.findLis().eq(this.liObj[e])),
                    n.toggleClass("selected", t)
                },
                setDisabled: function (e, t, n) {
                    n || (n = this.findLis().eq(this.liObj[e])),
                    t ? n.addClass("disabled").children("a").attr("href", "#").attr("tabindex", -1) : n.removeClass("disabled").children("a").removeAttr("href").attr("tabindex", 0)
                },
                isDisabled: function () {
                    return this.$element[0].disabled
                },
                checkDisabled: function () {
                    var e = this;
                    this.isDisabled() ? (this.$newElement.addClass("disabled"), this.$button.addClass("disabled").attr("tabindex", -1)) : (this.$button.hasClass("disabled") && (this.$newElement.removeClass("disabled"), this.$button.removeClass("disabled")), this.$button.attr("tabindex") != -1 || this.$element.data("tabindex") || this.$button.removeAttr("tabindex")),
                    this.$button.click(function () {
                        return !e.isDisabled()
                    })
                },
                tabIndex: function () {
                    this.$element.data("tabindex") !== this.$element.attr("tabindex") && this.$element.attr("tabindex") !== -98 && "-98" !== this.$element.attr("tabindex") && (this.$element.data("tabindex", this.$element.attr("tabindex")), this.$button.attr("tabindex", this.$element.data("tabindex"))),
                    this.$element.attr("tabindex", -98)
                },
                clickListener: function () {
                    var e = this,
                    t = $(document);
                    this.$newElement.on("touchstart.dropdown", ".dropdown-menu",
                    function (e) {
                        e.stopPropagation()
                    }),
                    t.data("spaceSelect", !1),
                    this.$button.on("keyup",
                    function (e) {
                        / (32) /.test(e.keyCode.toString(10)) && t.data("spaceSelect") && (e.preventDefault(), t.data("spaceSelect", !1))
                    }),
                    this.$button.on("click",
                    function () {
                        e.setSize(),
                        e.$element.on("shown.bs.select",
                        function () {
                            if (e.options.liveSearch || e.multiple) {
                                if (!e.multiple) {
                                    var t = e.liObj[e.$element[0].selectedIndex];
                                    if ("number" != typeof t || e.options.size === !1) return;
                                    var n = e.$lis.eq(t)[0].offsetTop - e.$menuInner[0].offsetTop;
                                    n = n - e.$menuInner[0].offsetHeight / 2 + e.sizeInfo.liHeight / 2,
                                    e.$menuInner[0].scrollTop = n
                                }
                            } else e.$menuInner.find(".selected a").focus()
                        })
                    }),
                    this.$menuInner.on("click", "li a",
                    function (t) {
                        var n = $(this),
                        i = n.parent().data("originalIndex"),
                        s = e.$element.val(),
                        o = e.$element.prop("selectedIndex");
                        if (e.multiple && t.stopPropagation(), t.preventDefault(), !e.isDisabled() && !n.parent().hasClass("disabled")) {
                            var a = e.$element.find("option"),
                            l = a.eq(i),
                            r = l.prop("selected"),
                            d = l.parent("optgroup"),
                            h = e.options.maxOptions,
                            c = d.data("maxOptions") || !1;
                            if (e.multiple) {
                                if (l.prop("selected", !r), e.setSelected(i, !r), n.blur(), h !== !1 || c !== !1) {
                                    var p = h < a.filter(":selected").length,
                                    u = c < d.find("option:selected").length;
                                    if (h && p || c && u) if (h && 1 == h) a.prop("selected", !1),
                                    l.prop("selected", !0),
                                    e.$menuInner.find(".selected").removeClass("selected"),
                                    e.setSelected(i, !0);
                                    else if (c && 1 == c) {
                                        d.find("option:selected").prop("selected", !1),
                                        l.prop("selected", !0);
                                        var f = n.parent().data("optgroup");
                                        e.$menuInner.find('[data-optgroup="' + f + '"]').removeClass("selected"),
                                        e.setSelected(i, !0)
                                    } else {
                                        var m = "function" == typeof e.options.maxOptionsText ? e.options.maxOptionsText(h, c) : e.options.maxOptionsText,
                                        v = m[0].replace("{n}", h),
                                        b = m[1].replace("{n}", c),
                                        g = $('<div class="notify"></div>');
                                        m[2] && (v = v.replace("{var}", m[2][h > 1 ? 0 : 1]), b = b.replace("{var}", m[2][c > 1 ? 0 : 1])),
                                        l.prop("selected", !1),
                                        e.$menu.append(g),
                                        h && p && (g.append($("<div>" + v + "</div>")), e.$element.trigger("maxReached.bs.select")),
                                        c && u && (g.append($("<div>" + b + "</div>")), e.$element.trigger("maxReachedGrp.bs.select")),
                                        setTimeout(function () {
                                            e.setSelected(i, !1)
                                        },
                                        10),
                                        g.delay(750).fadeOut(300,
                                        function () {
                                            $(this).remove()
                                        })
                                    }
                                }
                            } else a.prop("selected", !1),
                            l.prop("selected", !0),
                            e.$menuInner.find(".selected").removeClass("selected"),
                            e.setSelected(i, !0);
                            e.multiple ? e.options.liveSearch && e.$searchbox.focus() : e.$button.focus(),
                            (s != e.$element.val() && e.multiple || o != e.$element.prop("selectedIndex") && !e.multiple) && (e.$element.triggerNative("change"), e.$element.trigger("changed.bs.select", [i, l.prop("selected"), r]))
                        }
                    }),
                    this.$menu.on("click", "li.disabled a, .popover-title, .popover-title :not(.close)",
                    function (t) {
                        t.currentTarget == this && (t.preventDefault(), t.stopPropagation(), e.options.liveSearch && !$(t.target).hasClass("close") ? e.$searchbox.focus() : e.$button.focus())
                    }),
                    this.$menuInner.on("click", ".divider, .dropdown-header",
                    function (t) {
                        t.preventDefault(),
                        t.stopPropagation(),
                        e.options.liveSearch ? e.$searchbox.focus() : e.$button.focus()
                    }),
                    this.$menu.on("click", ".popover-title .close",
                    function () {
                        e.$button.click()
                    }),
                    this.$searchbox.on("click",
                    function (e) {
                        e.stopPropagation()
                    }),
                    this.$menu.on("click", ".actions-btn",
                    function (t) {
                        e.options.liveSearch ? e.$searchbox.focus() : e.$button.focus(),
                        t.preventDefault(),
                        t.stopPropagation(),
                        $(this).hasClass("bs-select-all") ? e.selectAll() : e.deselectAll(),
                        e.$element.triggerNative("change")
                    }),
                    this.$element.change(function () {
                        e.render(!1)
                    })
                },
                liveSearchListener: function () {
                    var n = this,
                    i = $('<li class="no-results"></li>');
                    this.$button.on("click.dropdown.data-api touchstart.dropdown.data-api",
                    function () {
                        n.$menuInner.find(".active").removeClass("active"),
                        n.$searchbox.val() && (n.$searchbox.val(""), n.$lis.not(".is-hidden").removeClass("hidden"), i.parent().length && i.remove()),
                        n.multiple || n.$menuInner.find(".selected").addClass("active"),
                        setTimeout(function () {
                            n.$searchbox.focus()
                        },
                        10)
                    }),
                    this.$searchbox.on("click.dropdown.data-api focus.dropdown.data-api touchend.dropdown.data-api",
                    function (e) {
                        e.stopPropagation()
                    }),
                    this.$searchbox.on("input propertychange",
                    function () {
                        if (n.$searchbox.val()) {
                            var s = n.$lis.not(".is-hidden").removeClass("hidden").children("a");
                            s = n.options.liveSearchNormalize ? s.not(":a" + n._searchStyle() + '("' + e(n.$searchbox.val()) + '")') : s.not(":" + n._searchStyle() + '("' + n.$searchbox.val() + '")'),
                            s.parent().addClass("hidden"),
                            n.$lis.filter(".dropdown-header").each(function () {
                                var e = $(this),
                                t = e.data("optgroup");
                                0 === n.$lis.filter("[data-optgroup=" + t + "]").not(e).not(".hidden").length && (e.addClass("hidden"), n.$lis.filter("[data-optgroup=" + t + "div]").addClass("hidden"))
                            });
                            var o = n.$lis.not(".hidden");
                            o.each(function (e) {
                                var t = $(this);
                                t.hasClass("divider") && (t.index() === o.first().index() || t.index() === o.last().index() || o.eq(e + 1).hasClass("divider")) && t.addClass("hidden")
                            }),
                            n.$lis.not(".hidden, .no-results").length ? i.parent().length && i.remove() : (i.parent().length && i.remove(), i.html(n.options.noneResultsText.replace("{0}", '"' + t(n.$searchbox.val()) + '"')).show(), n.$menuInner.append(i))
                        } else n.$lis.not(".is-hidden").removeClass("hidden"),
                        i.parent().length && i.remove();
                        n.$lis.filter(".active").removeClass("active"),
                        n.$searchbox.val() && n.$lis.not(".hidden, .divider, .dropdown-header").eq(0).addClass("active").children("a").focus(),
                        $(this).focus()
                    })
                },
                _searchStyle: function () {
                    var e = {
                        begins: "ibegins",
                        startsWith: "ibegins"
                    };
                    return e[this.options.liveSearchStyle] || "icontains"
                },
                val: function (e) {
                    return "undefined" != typeof e ? (this.$element.val(e), this.render(), this.$element) : this.$element.val()
                },
                changeAll: function (e) {
                    "undefined" == typeof e && (e = !0),
                    this.findLis();
                    for (var t = this.$element.find("option"), n = this.$lis.not(".divider, .dropdown-header, .disabled, .hidden").toggleClass("selected", e), i = n.length, s = [], o = 0; o < i; o++) {
                        var a = n[o].getAttribute("data-original-index");
                        s[s.length] = t.eq(a)[0]
                    }
                    $(s).prop("selected", e),
                    this.render(!1)
                },
                selectAll: function () {
                    return this.changeAll(!0)
                },
                deselectAll: function () {
                    return this.changeAll(!1)
                },
                keydown: function (t) {
                    var n, i, s, o, a, l, r, d, h, c = $(this),
                    p = c.is("input") ? c.parent().parent() : c.parent(),
                    u = p.data("this"),
                    f = ":not(.disabled, .hidden, .dropdown-header, .divider)",
                    m = {
                        32: " ",
                        48: "0",
                        49: "1",
                        50: "2",
                        51: "3",
                        52: "4",
                        53: "5",
                        54: "6",
                        55: "7",
                        56: "8",
                        57: "9",
                        59: ";",
                        65: "a",
                        66: "b",
                        67: "c",
                        68: "d",
                        69: "e",
                        70: "f",
                        71: "g",
                        72: "h",
                        73: "i",
                        74: "j",
                        75: "k",
                        76: "l",
                        77: "m",
                        78: "n",
                        79: "o",
                        80: "p",
                        81: "q",
                        82: "r",
                        83: "s",
                        84: "t",
                        85: "u",
                        86: "v",
                        87: "w",
                        88: "x",
                        89: "y",
                        90: "z",
                        96: "0",
                        97: "1",
                        98: "2",
                        99: "3",
                        100: "4",
                        101: "5",
                        102: "6",
                        103: "7",
                        104: "8",
                        105: "9"
                    };
                    if (u.options.liveSearch && (p = c.parent().parent()), u.options.container && (p = u.$menu), n = $("[role=menu] li", p), h = u.$newElement.hasClass("open"), !h && (t.keyCode >= 48 && t.keyCode <= 57 || t.keyCode >= 96 && t.keyCode <= 105 || t.keyCode >= 65 && t.keyCode <= 90) && (u.options.container ? u.$button.trigger("click") : (u.setSize(), u.$menu.parent().addClass("open"), h = !0), u.$searchbox.focus()), u.options.liveSearch && (/(^9$|27)/.test(t.keyCode.toString(10)) && h && 0 === u.$menu.find(".active").length && (t.preventDefault(), u.$menu.parent().removeClass("open"), u.options.container && u.$newElement.removeClass("open"), u.$button.focus()), n = $("[role=menu] li" + f, p), c.val() || /(38|40)/.test(t.keyCode.toString(10)) || 0 === n.filter(".active").length && (n = u.$menuInner.find("li"), n = u.options.liveSearchNormalize ? n.filter(":a" + u._searchStyle() + "(" + e(m[t.keyCode]) + ")") : n.filter(":" + u._searchStyle() + "(" + m[t.keyCode] + ")"))), n.length) {
                        if (/(38|40)/.test(t.keyCode.toString(10))) i = n.index(n.find("a").filter(":focus").parent()),
                        o = n.filter(f).first().index(),
                        a = n.filter(f).last().index(),
                        s = n.eq(i).nextAll(f).eq(0).index(),
                        l = n.eq(i).prevAll(f).eq(0).index(),
                        r = n.eq(s).prevAll(f).eq(0).index(),
                        u.options.liveSearch && (n.each(function (e) {
                            $(this).hasClass("disabled") || $(this).data("index", e)
                        }), i = n.index(n.filter(".active")), o = n.first().data("index"), a = n.last().data("index"), s = n.eq(i).nextAll().eq(0).data("index"), l = n.eq(i).prevAll().eq(0).data("index"), r = n.eq(s).prevAll().eq(0).data("index")),
                        d = c.data("prevIndex"),
                        38 == t.keyCode ? (u.options.liveSearch && i--, i != r && i > l && (i = l), i < o && (i = o), i == d && (i = a)) : 40 == t.keyCode && (u.options.liveSearch && i++, i == -1 && (i = 0), i != r && i < s && (i = s), i > a && (i = a), i == d && (i = o)),
                        c.data("prevIndex", i),
                        u.options.liveSearch ? (t.preventDefault(), c.hasClass("dropdown-toggle") || (n.removeClass("active").eq(i).addClass("active").children("a").focus(), c.focus())) : n.eq(i).children("a").focus();
                        else if (!c.is("input")) {
                            var v, b, g = [];
                            n.each(function () {
                                $(this).hasClass("disabled") || $.trim($(this).children("a").text().toLowerCase()).substring(0, 1) == m[t.keyCode] && g.push($(this).index())
                            }),
                            v = $(document).data("keycount"),
                            v++,
                            $(document).data("keycount", v),
                            b = $.trim($(":focus").text().toLowerCase()).substring(0, 1),
                            b != m[t.keyCode] ? (v = 1, $(document).data("keycount", v)) : v >= g.length && ($(document).data("keycount", 0), v > g.length && (v = 1)),
                            n.eq(g[v - 1]).children("a").focus()
                        }
                        if ((/(13|32)/.test(t.keyCode.toString(10)) || /(^9$)/.test(t.keyCode.toString(10)) && u.options.selectOnTab) && h) {
                            if (/(32)/.test(t.keyCode.toString(10)) || t.preventDefault(), u.options.liveSearch) / (32) /.test(t.keyCode.toString(10)) || (u.$menuInner.find(".active a").click(), c.focus());
                            else {
                                var x = $(":focus");
                                x.click(),
                                x.focus(),
                                t.preventDefault(),
                                $(document).data("spaceSelect", !0)
                            }
                            $(document).data("keycount", 0)
                        } (/(^9$|27)/.test(t.keyCode.toString(10)) && h && (u.multiple || u.options.liveSearch) || /(27)/.test(t.keyCode.toString(10)) && !h) && (u.$menu.parent().removeClass("open"), u.options.container && u.$newElement.removeClass("open"), u.$button.focus())
                    }
                },
                mobile: function () {
                    this.$element.addClass("mobile-device")
                },
                refresh: function () {
                    this.$lis = null,
                    this.liObj = {},
                    this.reloadLi(),
                    this.render(),
                    this.checkDisabled(),
                    this.liHeight(!0),
                    this.setStyle(),
                    this.setWidth(),
                    this.$lis && this.$searchbox.trigger("propertychange"),
                    this.$element.trigger("refreshed.bs.select")
                },
                hide: function () {
                    this.$newElement.hide()
                },
                show: function () {
                    this.$newElement.show()
                },
                remove: function () {
                    this.$newElement.remove(),
                    this.$element.remove()
                },
                destroy: function () {
                    this.$newElement.remove(),
                    this.$bsContainer ? this.$bsContainer.remove() : this.$menu.remove(),
                    this.$element.off(".bs.select").removeData("selectpicker").removeClass("bs-select-hidden selectpicker")
                }
            };
            var s = $.fn.selectpicker;
            $.fn.selectpicker = n,
            $.fn.selectpicker.Constructor = i,
            $.fn.selectpicker.noConflict = function () {
                return $.fn.selectpicker = s,
                this
            },
            $(document).data("keycount", 0).on("keydown.bs.select", '.bootstrap-select [data-toggle=dropdown], .bootstrap-select [role="menu"], .bs-searchbox input', i.prototype.keydown).on("focusin.modal", '.bootstrap-select [data-toggle=dropdown], .bootstrap-select [role="menu"], .bs-searchbox input',
            function (e) {
                e.stopPropagation()
            }),
            $(window).on("load.bs.select.data-api",
            function () {
                $(".selectpicker").each(function () {
                    var e = $(this);
                    n.call(e, e.data())
                })
            })
        }(e)
}),
function (e, t) {
    "function" == typeof define && define.amd ? define(["jquery"],
	function (e) {
	    return t(e)
	}) : "object" == typeof exports ? module.exports = t(require("jquery")) : t(jQuery)
}(this,
function (e) {
    !
        function ($) {
            $.fn.selectpicker.defaults = {
                noneSelectedText: "没有选中任何项",
                noneResultsText: "没有找到匹配项",
                countSelectedText: "选中{1}中的{0}项",
                maxOptionsText: ["超出限制 (最多选择{n}项)", "组选择超出限制(最多选择{n}组)"],
                multipleSeparator: ", "
            }
        }(e)
});
jQuery.cookie = function (e, i, o) {
    if ("undefined" == typeof i) {
        var n = null;
        if (document.cookie && "" != document.cookie) for (var r = document.cookie.split(";"), t = 0; t < r.length; t++) {
            var p = jQuery.trim(r[t]);
            if (p.substring(0, e.length + 1) == e + "=") {
                n = decodeURIComponent(p.substring(e.length + 1));
                break
            }
        }
        return n
    }
    o = o || {},
	null === i && (i = "", o.expires = -1);
    var u = "";
    if (o.expires && ("number" == typeof o.expires || o.expires.toUTCString)) {
        var s;
        "number" == typeof o.expires ? (s = new Date, s.setTime(s.getTime() + 1e3 * o.expires)) : s = o.expires,
		u = "; expires=" + s.toUTCString()
    }
    var a = o.path ? "; path=" + o.path : "",
	c = o.domain ? "; domain=" + o.domain : "",
	m = o.secure ? "; secure" : "";
    document.cookie = [e, "=", encodeURIComponent(i), u, a, c, m].join("")
}; !
function (r) {
    r.fn.__bind__ = r.fn.bind,
	r.fn.__unbind__ = r.fn.unbind,
	r.fn.__find__ = r.fn.find;
    var t = {
        version: "0.7.8",
        override: /keydown|keypress|keyup/g,
        triggersMap: {},
        specialKeys: {
            27: "esc",
            9: "tab",
            32: "space",
            13: "return",
            8: "backspace",
            145: "scroll",
            20: "capslock",
            144: "numlock",
            19: "pause",
            45: "insert",
            36: "home",
            46: "del",
            35: "end",
            33: "pageup",
            34: "pagedown",
            37: "left",
            38: "up",
            39: "right",
            40: "down",
            112: "f1",
            113: "f2",
            114: "f3",
            115: "f4",
            116: "f5",
            117: "f6",
            118: "f7",
            119: "f8",
            120: "f9",
            121: "f10",
            122: "f11",
            123: "f12"
        },
        shiftNums: {
            "`": "~",
            1: "!",
            2: "@",
            3: "#",
            4: "$",
            5: "%",
            6: "^",
            7: "&",
            8: "*",
            9: "(",
            0: ")",
            "-": "_",
            "=": "+",
            ";": ":",
            "'": '"',
            ",": "<",
            ".": ">",
            "/": "?",
            "\\": "|"
        },
        newTrigger: function (r, t, e) {
            var i = {};
            return i[r] = {},
			i[r][t] = {
			    cb: e,
			    disableInInput: !1
			},
			i
        }
    };
    return r.browser.mozilla && (t.specialKeys = r.extend(t.specialKeys, {
        96: "0",
        97: "1",
        98: "2",
        99: "3",
        100: "4",
        101: "5",
        102: "6",
        103: "7",
        104: "8",
        105: "9"
    })),
	r.fn.find = function (t) {
	    return this.query = t,
		r.fn.__find__.apply(this, arguments)
	},
	r.fn.unbind = function (e, i, n) {
	    if (r.isFunction(i) && (n = i, i = null), i && "string" == typeof i) for (var s = (this.prevObject && this.prevObject.query || this[0].id && this[0].id || this[0]).toString(), a = e.split(" "), f = 0; f < a.length; f++) delete t.triggersMap[s][a[f]][i];
	    return this.__unbind__(e, n)
	},
	r.fn.bind = function (e, i, n) {
	    var s = e.match(t.override);
	    if (r.isFunction(i) || !s) return this.__bind__(e, i, n);
	    var a = null,
		f = r.trim(e.replace(t.override, ""));
	    if (f && (a = this.__bind__(f, i, n)), "string" == typeof i && (i = {
	        combi: i
	    }), i.combi) for (var o = 0; o < s.length; o++) {
	        var p = s[o],
			d = i.combi.toLowerCase(),
			h = t.newTrigger(p, d, n),
			u = (this.prevObject && this.prevObject.query || this[0].id && this[0].id || this[0]).toString();
	        h[p][d].disableInInput = i.disableInInput,
			t.triggersMap[u] ? t.triggersMap[u][p] || (t.triggersMap[u][p] = h[p]) : t.triggersMap[u] = h;
	        var g = t.triggersMap[u][p][d];
	        g ? g.constructor !== Array ? t.triggersMap[u][p][d] = [g] : t.triggersMap[u][p][d][g.length] = h[p][d] : t.triggersMap[u][p][d] = [h[p][d]],
			this.each(function () {
			    var t = r(this);
			    t.attr("hkId") && t.attr("hkId") !== u && (u = t.attr("hkId") + ";" + u),
				t.attr("hkId", u)
			}),
			a = this.__bind__(s.join(" "), i, t.handler)
	    }
	    return a
	},
	t.findElement = function (t) {
	    if (!r(t).attr("hkId") && (r.browser.opera || r.browser.safari)) for (; !r(t).attr("hkId") && t.parentNode;) t = t.parentNode;
	    return t
	},
	t.handler = function (e) {
	    var i = t.findElement(e.currentTarget),
		n = r(i),
		s = n.attr("hkId");
	    if (s) {
	        s = s.split(";");
	        for (var a = e.which,
			f = e.type,
			o = t.specialKeys[a], p = !o && String.fromCharCode(a).toLowerCase(), d = e.shiftKey, h = e.ctrlKey, u = e.altKey || e.originalEvent.altKey, g = null, l = 0; l < s.length; l++) if (t.triggersMap[s[l]][f]) {
			    g = t.triggersMap[s[l]][f];
			    break
			}
	        if (g) {
	            var c;
	            if (d || h || u) {
	                var _ = "";
	                u && (_ += "alt+"),
					h && (_ += "ctrl+"),
					d && (_ += "shift+"),
					c = g[_ + o],
					c || p && (c = g[_ + p] || g[_ + t.shiftNums[p]] || "shift+" === _ && g[t.shiftNums[p]])
	            } else c = g[o] || p && g[p];
	            if (c) {
	                for (var b = !1,
					l = 0; l < c.length; l++) {
	                    if (c[l].disableInInput) {
	                        var v = r(e.target);
	                        if (n.is("input") || n.is("textarea") || v.is("input") || v.is("textarea")) return !0
	                    }
	                    b = b || c[l].cb.apply(this, [e])
	                }
	                return b
	            }
	        }
	    }
	},
	window.hotkeys = t,
	r
}(jQuery); !
function (e) {
    "function" == typeof define && define.amd ? define(["jquery"], e) : "object" == typeof exports ? module.exports = e : e(jQuery)
}(function ($) {
    function e(e) {
        var l = e || window.event,
		s = a.call(arguments, 1),
		h = 0,
		u = 0,
		r = 0,
		d = 0;
        if (e = $.event.fix(l), e.type = "mousewheel", "detail" in l && (r = l.detail * -1), "wheelDelta" in l && (r = l.wheelDelta), "wheelDeltaY" in l && (r = l.wheelDeltaY), "wheelDeltaX" in l && (u = l.wheelDeltaX * -1), "axis" in l && l.axis === l.HORIZONTAL_AXIS && (u = r * -1, r = 0), h = 0 === r ? u : r, "deltaY" in l && (r = l.deltaY * -1, h = r), "deltaX" in l && (u = l.deltaX, 0 === r && (h = u * -1)), 0 !== r || 0 !== u) {
            if (1 === l.deltaMode) {
                var f = $.data(this, "mousewheel-line-height");
                h *= f,
				r *= f,
				u *= f
            } else if (2 === l.deltaMode) {
                var c = $.data(this, "mousewheel-page-height");
                h *= c,
				r *= c,
				u *= c
            }
            return d = Math.max(Math.abs(r), Math.abs(u)),
			(!o || d < o) && (o = d, n(l, d) && (o /= 40)),
			n(l, d) && (h /= 40, u /= 40, r /= 40),
			h = Math[h >= 1 ? "floor" : "ceil"](h / o),
			u = Math[u >= 1 ? "floor" : "ceil"](u / o),
			r = Math[r >= 1 ? "floor" : "ceil"](r / o),
			e.deltaX = u,
			e.deltaY = r,
			e.deltaFactor = o,
			e.deltaMode = 0,
			s.unshift(e, h, u, r),
			i && clearTimeout(i),
			i = setTimeout(t, 200),
			($.event.dispatch || $.event.handle).apply(this, s)
        }
    }
    function t() {
        o = null
    }
    function n(e, t) {
        return u.settings.adjustOldDeltas && "mousewheel" === e.type && t % 120 === 0
    }
    var i, o, l = ["wheel", "mousewheel", "DOMMouseScroll", "MozMousePixelScroll"],
	s = "onwheel" in document || document.documentMode >= 9 ? ["wheel"] : ["mousewheel", "DomMouseScroll", "MozMousePixelScroll"],
	a = Array.prototype.slice;
    if ($.event.fixHooks) for (var h = l.length; h;) $.event.fixHooks[l[--h]] = $.event.mouseHooks;
    var u = $.event.special.mousewheel = {
        version: "3.1.9",
        setup: function () {
            if (this.addEventListener) for (var t = s.length; t;) this.addEventListener(s[--t], e, !1);
            else this.onmousewheel = e;
            $.data(this, "mousewheel-line-height", u.getLineHeight(this)),
			$.data(this, "mousewheel-page-height", u.getPageHeight(this))
        },
        teardown: function () {
            if (this.removeEventListener) for (var t = s.length; t;) this.removeEventListener(s[--t], e, !1);
            else this.onmousewheel = null
        },
        getLineHeight: function (e) {
            return parseInt($(e)["offsetParent" in $.fn ? "offsetParent" : "parent"]().css("fontSize"), 10)
        },
        getPageHeight: function (e) {
            return $(e).height()
        },
        settings: {
            adjustOldDeltas: !0
        }
    };
    $.fn.extend({
        mousewheel: function (e) {
            return e ? this.bind("mousewheel", e) : this.trigger("mousewheel")
        },
        unmousewheel: function (e) {
            return this.unbind("mousewheel", e)
        }
    })
}); !
function ($) {
    $.extend($.fn, {
        validate: function (t) {
            if (!this.length) return void (t && t.debug && window.console && console.warn("nothing selected, can't validate, returning nothing"));
            var e = $.data(this[0], "validator");
            if (e) return e;
            if ("undefined" != typeof Worker && this.attr("novalidate", "novalidate"), e = new $.validator(t, this[0]), $.data(this[0], "validator", e), e.settings.onsubmit) {
                var i = this.find("input, button");
                i.filter(".cancel").click(function () {
                    e.cancelSubmit = !0
                }),
				e.settings.submitHandler && i.filter(":submit").click(function () {
				    e.submitButton = this
				}),
				this.submit(function (t) {
				    function i() {
				        if (e.settings.submitHandler) {
				            if (e.submitButton) var t = $("<input type='hidden'/>").attr("name", e.submitButton.name).val(e.submitButton.value).appendTo(e.currentForm);
				            return e.settings.submitHandler.call(e, e.currentForm),
							e.submitButton && t.remove(),
							!1
				        }
				        return !0
				    }
				    return e.settings.debug && t.preventDefault(),
					e.cancelSubmit ? (e.cancelSubmit = !1, i()) : e.form() ? e.pendingRequest ? (e.formSubmitted = !0, !1) : i() : (e.focusInvalid(), !1)
				})
            }
            return e
        },
        valid: function () {
            if ($(this[0]).is("form")) return this.validate().form();
            var t = !0,
			e = $(this[0].form).validate();
            return this.each(function () {
                t &= e.element(this)
            }),
			t
        },
        removeAttrs: function (t) {
            var e = {},
			i = this;
            return $.each(t.split(/\s/),
			function (t, n) {
			    e[n] = i.attr(n),
				i.removeAttr(n)
			}),
			e
        },
        rules: function (t, e) {
            var i = this[0];
            if (t) {
                var n = $.data(i.form, "validator").settings,
				a = n.rules,
				r = $.validator.staticRules(i);
                switch (t) {
                    case "add":
                        $.extend(r, $.validator.normalizeRule(e)),
                        a[i.name] = r,
                        e.messages && (n.messages[i.name] = $.extend(n.messages[i.name], e.messages));
                        break;
                    case "remove":
                        if (!e) return delete a[i.name],
                        r;
                        var s = {};
                        return $.each(e.split(/\s/),
                        function (t, e) {
                            s[e] = r[e],
                            delete r[e]
                        }),
                        s
                }
            }
            var u = $.validator.normalizeRules($.extend({},
			$.validator.metadataRules(i), $.validator.classRules(i), $.validator.attributeRules(i), $.validator.staticRules(i)), i);
            if (u.required) {
                var o = u.required;
                delete u.required,
				u = $.extend({
				    required: o
				},
				u)
            }
            return u
        }
    }),
	$.extend($.expr[":"], {
	    blank: function (t) {
	        return !$.trim("" + t.value)
	    },
	    filled: function (t) {
	        return !!$.trim("" + t.value)
	    },
	    unchecked: function (t) {
	        return !t.checked
	    }
	}),
	$.validator = function (t, e) {
	    this.settings = $.extend(!0, {},
		$.validator.defaults, t),
		this.currentForm = e,
		this.init()
	},
	$.validator.format = function (t, e) {
	    return 1 == arguments.length ?
		function () {
		    var e = $.makeArray(arguments);
		    return e.unshift(t),
			$.validator.format.apply(this, e)
		} : (arguments.length > 2 && e.constructor != Array && (e = $.makeArray(arguments).slice(1)), e.constructor != Array && (e = [e]), $.each(e,
		function (e, i) {
		    t = t.replace(new RegExp("\\{" + e + "\\}", "g"), i)
		}), t)
	},
	$.extend($.validator, {
	    defaults: {
	        messages: {},
	        groups: {},
	        rules: {},
	        errorClass: "error",
	        validClass: "valid",
	        errorElement: "label",
	        focusInvalid: !0,
	        errorContainer: $([]),
	        errorLabelContainer: $([]),
	        onsubmit: !0,
	        ignore: ":hidden",
	        ignoreTitle: !1,
	        onfocusin: function (t, e) {
	            this.lastActive = t,
				this.settings.focusCleanup && !this.blockFocusCleanup && (this.settings.unhighlight && this.settings.unhighlight.call(this, t, this.settings.errorClass, this.settings.validClass), this.addWrapper(this.errorsFor(t)).hide())
	        },
	        onfocusout: function (t, e) {
	            this.checkable(t) || !(t.name in this.submitted) && this.optional(t) || this.element(t)
	        },
	        onkeyup: function (t, e) {
	            (t.name in this.submitted || t == this.lastElement) && this.element(t)
	        },
	        onclick: function (t, e) {
	            t.name in this.submitted ? this.element(t) : t.parentNode.name in this.submitted && this.element(t.parentNode)
	        },
	        highlight: function (t, e, i) {
	            "radio" === t.type ? this.findByName(t.name).addClass(e).removeClass(i) : $(t).addClass(e).removeClass(i)
	        },
	        unhighlight: function (t, e, i) {
	            "radio" === t.type ? this.findByName(t.name).removeClass(e).addClass(i) : $(t).removeClass(e).addClass(i)
	        }
	    },
	    setDefaults: function (t) {
	        $.extend($.validator.defaults, t)
	    },
	    messages: {
	        required: "This field is required.",
	        remote: "Please fix this field.",
	        email: "Please enter a valid email address.",
	        url: "Please enter a valid URL.",
	        date: "Please enter a valid date.",
	        dateISO: "Please enter a valid date (ISO).",
	        number: "Please enter a valid number.",
	        digits: "Please enter only digits.",
	        creditcard: "Please enter a valid credit card number.",
	        equalTo: "Please enter the same value again.",
	        notEqualTo: "Please enter the different value again.",
	        largerThan: "Please enter the larger value again.",
	        lessThan: "Please enter the less value again.",
	        accept: "Please enter a value with a valid extension.",
	        maxlength: $.validator.format("Please enter no more than {0} characters."),
	        minlength: $.validator.format("Please enter at least {0} characters."),
	        rangelength: $.validator.format("Please enter a value between {0} and {1} characters long."),
	        range: $.validator.format("Please enter a value between {0} and {1}."),
	        max: $.validator.format("Please enter a value less than or equal to {0}."),
	        min: $.validator.format("Please enter a value greater than or equal to {0}.")
	    },
	    autoCreateRanges: !1,
	    prototype: {
	        init: function () {
	            function t(t) {
	                var e = $.data(this[0].form, "validator"),
					i = "on" + t.type.replace(/^validate/, "");
	                e.settings[i] && e.settings[i].call(e, this[0], t)
	            }
	            this.labelContainer = $(this.settings.errorLabelContainer),
				this.errorContext = this.labelContainer.length && this.labelContainer || $(this.currentForm),
				this.containers = $(this.settings.errorContainer).add(this.settings.errorLabelContainer),
				this.submitted = {},
				this.valueCache = {},
				this.pendingRequest = 0,
				this.pending = {},
				this.invalid = {},
				this.reset();
	            var e = this.groups = {};
	            $.each(this.settings.groups,
				function (t, i) {
				    $.each(i.split(/\s/),
					function (i, n) {
					    e[n] = t
					})
				});
	            var i = this.settings.rules;
	            $.each(i,
				function (t, e) {
				    i[t] = $.validator.normalizeRule(e)
				}),
				$(this.currentForm).validateDelegate("[type='text'], [type='password'], [type='file'], select, textarea, [type='number'], [type='search'] ,[type='tel'], [type='url'], [type='email'], [type='datetime'], [type='date'], [type='month'], [type='week'], [type='time'], [type='datetime-local'], [type='range'], [type='color'] ", "focusin focusout keyup", t).validateDelegate("[type='radio'], [type='checkbox'], select, option", "click", t),
				this.settings.invalidHandler && $(this.currentForm).bind("invalid-form.validate", this.settings.invalidHandler)
	        },
	        form: function () {
	            return this.checkForm(),
				$.extend(this.submitted, this.errorMap),
				this.invalid = $.extend({},
				this.errorMap),
				this.valid() || $(this.currentForm).triggerHandler("invalid-form", [this]),
				this.showErrors(),
				this.valid()
	        },
	        checkForm: function () {
	            this.prepareForm();
	            for (var t = 0,
				e = this.currentElements = this.elements() ; e[t]; t++) this.check(e[t]);
	            return this.valid()
	        },
	        element: function (t) {
	            t = this.validationTargetFor(this.clean(t)),
				this.lastElement = t,
				this.prepareElement(t),
				this.currentElements = $(t);
	            var e = this.check(t);
	            return e ? delete this.invalid[t.name] : this.invalid[t.name] = !0,
				this.numberOfInvalids() || (this.toHide = this.toHide.add(this.containers)),
				this.showErrors(),
				e
	        },
	        showErrors: function (t) {
	            if (t) {
	                $.extend(this.errorMap, t),
					this.errorList = [];
	                for (var e in t) this.errorList.push({
	                    message: t[e],
	                    element: this.findByName(e)[0]
	                });
	                this.successList = $.grep(this.successList,
					function (e) {
					    return !(e.name in t)
					})
	            }
	            this.settings.showErrors ? this.settings.showErrors.call(this, this.errorMap, this.errorList) : this.defaultShowErrors()
	        },
	        resetForm: function () {
	            $.fn.resetForm && $(this.currentForm).resetForm(),
				this.submitted = {},
				this.lastElement = null,
				this.prepareForm(),
				this.hideErrors(),
				this.elements().removeClass(this.settings.errorClass)
	        },
	        numberOfInvalids: function () {
	            return this.objectLength(this.invalid)
	        },
	        objectLength: function (t) {
	            var e = 0;
	            for (var i in t) e++;
	            return e
	        },
	        hideErrors: function () {
	            this.addWrapper(this.toHide).hide()
	        },
	        valid: function () {
	            return 0 == this.size()
	        },
	        size: function () {
	            return this.errorList.length
	        },
	        focusInvalid: function () {
	            if (this.settings.focusInvalid) try {
	                $(this.findLastActive() || this.errorList.length && this.errorList[0].element || []).filter(":visible").focus().trigger("focusin")
	            } catch (t) { }
	        },
	        findLastActive: function () {
	            var t = this.lastActive;
	            return t && 1 == $.grep(this.errorList,
				function (e) {
				    return e.element.name == t.name
				}).length && t
	        },
	        elements: function () {
	            var t = this,
				e = {};
	            return $(this.currentForm).find("input, select, textarea").not(":submit, :reset, :image, [disabled]").not(this.settings.ignore).filter(function () {
	                return !this.name && t.settings.debug && window.console && console.error("%o has no name assigned", this),
					!(this.name in e || !t.objectLength($(this).rules())) && (e[this.name] = !0, !0)
	            })
	        },
	        clean: function (t) {
	            return $(t)[0]
	        },
	        errors: function () {
	            return $(this.settings.errorElement + "." + this.settings.errorClass, this.errorContext)
	        },
	        reset: function () {
	            this.successList = [],
				this.errorList = [],
				this.errorMap = {},
				this.toShow = $([]),
				this.toHide = $([]),
				this.currentElements = $([])
	        },
	        prepareForm: function () {
	            this.reset(),
				this.toHide = this.errors().add(this.containers)
	        },
	        prepareElement: function (t) {
	            this.reset(),
				this.toHide = this.errorsFor(t)
	        },
	        check: function (t) {
	            t = this.validationTargetFor(this.clean(t));
	            var e = $(t).rules(),
				i = !1;
	            for (var n in e) {
	                var a = {
	                    method: n,
	                    parameters: e[n]
	                };
	                try {
	                    if ("undefined" != typeof $.validator.methods[n]) {
	                        var r = $.validator.methods[n].call(this, t.value.replace(/\r/g, ""), t, a.parameters);
	                        if ("dependency-mismatch" == r) {
	                            i = !0;
	                            continue
	                        }
	                        if (i = !1, "pending" == r) return void (this.toHide = this.toHide.not(this.errorsFor(t)));
	                        if (!r) return this.formatAndAdd(t, a),
							!1
	                    }
	                } catch (e) {
	                    throw this.settings.debug && window.console && console.log("exception occured when checking element " + t.id + ", check the '" + a.method + "' method", e),
						e
	                }
	            }
	            if (!i) return this.objectLength(e) && this.successList.push(t),
				!0
	        },
	        customMetaMessage: function (t, e) {
	            if ($.metadata) {
	                var i = this.settings.meta ? $(t).metadata()[this.settings.meta] : $(t).metadata();
	                return i && i.messages && i.messages[e]
	            }
	        },
	        customMessage: function (t, e) {
	            var i = this.settings.messages[t];
	            return i && (i.constructor == String ? i : i[e])
	        },
	        findDefined: function () {
	            for (var t = 0; t < arguments.length; t++) if (void 0 !== arguments[t]) return arguments[t]
	        },
	        defaultMessage: function (t, e) {
	            return this.findDefined(this.customMessage(t.name, e), this.customMetaMessage(t, e), !this.settings.ignoreTitle && t.title || void 0, $.validator.messages[e], "<strong>Warning: No message defined for " + t.name + "</strong>")
	        },
	        formatAndAdd: function (t, e) {
	            var i = this.defaultMessage(t, e.method),
				n = /\$?\{(\d+)\}/g;
	            "function" == typeof i ? i = i.call(this, e.parameters, t) : n.test(i) && (i = jQuery.format(i.replace(n, "{$1}"), e.parameters)),
				this.errorList.push({
				    message: i,
				    element: t
				}),
				this.errorMap[t.name] = i,
				this.submitted[t.name] = i
	        },
	        addWrapper: function (t) {
	            return this.settings.wrapper && (t = t.add(t.parent(this.settings.wrapper))),
				t
	        },
	        defaultShowErrors: function () {
	            for (var t = 0; this.errorList[t]; t++) {
	                var e = this.errorList[t];
	                this.settings.highlight && this.settings.highlight.call(this, e.element, this.settings.errorClass, this.settings.validClass),
					this.showLabel(e.element, e.message)
	            }
	            if (this.errorList.length && (this.toShow = this.toShow.add(this.containers)), this.settings.success) for (var t = 0; this.successList[t]; t++) this.showLabel(this.successList[t]);
	            if (this.settings.unhighlight) for (var t = 0,
				i = this.validElements() ; i[t]; t++) this.settings.unhighlight.call(this, i[t], this.settings.errorClass, this.settings.validClass);
	            this.toHide = this.toHide.not(this.toShow),
				this.hideErrors(),
				this.addWrapper(this.toShow).show()
	        },
	        validElements: function () {
	            return this.currentElements.not(this.invalidElements())
	        },
	        invalidElements: function () {
	            return $(this.errorList).map(function () {
	                return this.element
	            })
	        },
	        showLabel: function (t, e) {
	            var i = this.errorsFor(t);
	            i.length ? (i.removeClass(this.settings.validClass).addClass(this.settings.errorClass), i.attr("generated") && i.html(e)) : (i = $("<" + this.settings.errorElement + "/>").attr({
	                for: this.idOrName(t),
	                generated: !0
	            }).addClass(this.settings.errorClass).html(e || ""), this.settings.wrapper && (i = i.hide().show().wrap("<" + this.settings.wrapper + "/>").parent()), this.labelContainer.append(i).length || (this.settings.errorPlacement ? this.settings.errorPlacement(i, $(t)) : i.insertAfter(t))),
				!e && this.settings.success && (i.text(""), "string" == typeof this.settings.success ? i.addClass(this.settings.success) : this.settings.success(i)),
				this.toShow = this.toShow.add(i)
	        },
	        errorsFor: function (t) {
	            var e = this.idOrName(t);
	            return this.errors().filter(function () {
	                return $(this).attr("for") == e
	            })
	        },
	        idOrName: function (t) {
	            return this.groups[t.name] || (this.checkable(t) ? t.name : t.id || t.name)
	        },
	        validationTargetFor: function (t) {
	            return this.checkable(t) && (t = this.findByName(t.name).not(this.settings.ignore)[0]),
				t
	        },
	        checkable: function (t) {
	            return /radio|checkbox/i.test(t.type)
	        },
	        findByName: function (t) {
	            var e = this.currentForm;
	            return $(document.getElementsByName(t)).map(function (i, n) {
	                return n.form == e && n.name == t && n || null
	            })
	        },
	        getLength: function (t, e) {
	            switch (e.nodeName.toLowerCase()) {
	                case "select":
	                    return $("option:selected", e).length;
	                case "input":
	                    if (this.checkable(e)) return this.findByName(e.name).filter(":checked").length
	            }
	            return t.length
	        },
	        depend: function (t, e) {
	            return !this.dependTypes[typeof t] || this.dependTypes[typeof t](t, e)
	        },
	        dependTypes: {
	            boolean: function (t, e) {
	                return t
	            },
	            string: function (t, e) {
	                return !!$(t, e.form).length
	            },
	            function: function (t, e) {
	                return t(e)
	            }
	        },
	        optional: function (t) {
	            return !$.validator.methods.required.call(this, $.trim(t.value), t) && "dependency-mismatch"
	        },
	        startRequest: function (t) {
	            this.pending[t.name] || (this.pendingRequest++, this.pending[t.name] = !0)
	        },
	        stopRequest: function (t, e) {
	            this.pendingRequest--,
				this.pendingRequest < 0 && (this.pendingRequest = 0),
				delete this.pending[t.name],
				e && 0 == this.pendingRequest && this.formSubmitted && this.form() ? ($(this.currentForm).submit(), this.formSubmitted = !1) : !e && 0 == this.pendingRequest && this.formSubmitted && ($(this.currentForm).triggerHandler("invalid-form", [this]), this.formSubmitted = !1)
	        },
	        previousValue: function (t) {
	            return $.data(t, "previousValue") || $.data(t, "previousValue", {
	                old: null,
	                valid: !0,
	                message: this.defaultMessage(t, "remote")
	            })
	        }
	    },
	    classRuleSettings: {
	        required: {
	            required: !0
	        },
	        email: {
	            email: !0
	        },
	        url: {
	            url: !0
	        },
	        date: {
	            date: !0
	        },
	        dateISO: {
	            dateISO: !0
	        },
	        dateDE: {
	            dateDE: !0
	        },
	        number: {
	            number: !0
	        },
	        numberDE: {
	            numberDE: !0
	        },
	        digits: {
	            digits: !0
	        },
	        creditcard: {
	            creditcard: !0
	        }
	    },
	    addClassRules: function (t, e) {
	        t.constructor == String ? this.classRuleSettings[t] = e : $.extend(this.classRuleSettings, t)
	    },
	    classRules: function (t) {
	        var e = {},
			i = $(t).attr("class");
	        return i && $.each(i.split(" "),
			function () {
			    this in $.validator.classRuleSettings && $.extend(e, $.validator.classRuleSettings[this])
			}),
			e
	    },
	    attributeRules: function (t) {
	        var e = {},
			i = $(t);
	        for (var n in $.validator.methods) {
	            var a;
	            a = "required" === n && "function" == typeof $.fn.prop ? i.prop(n) : i.attr(n),
				a ? e[n] = a : i[0].getAttribute("type") === n && (e[n] = !0)
	        }
	        return e.maxlength && /-1|2147483647|524288/.test(e.maxlength) && delete e.maxlength,
			e
	    },
	    metadataRules: function (t) {
	        if (!$.metadata) return {};
	        var e = $.data(t.form, "validator").settings.meta;
	        return e ? $(t).metadata()[e] : $(t).metadata()
	    },
	    staticRules: function (t) {
	        var e = {},
			i = $.data(t.form, "validator");
	        return i.settings.rules && (e = $.validator.normalizeRule(i.settings.rules[t.name]) || {}),
			e
	    },
	    normalizeRules: function (t, e) {
	        return $.each(t,
			function (i, n) {
			    if (n === !1) return void delete t[i];
			    if (n.param || n.depends) {
			        var a = !0;
			        switch (typeof n.depends) {
			            case "string":
			                a = !!$(n.depends, e.form).length;
			                break;
			            case "function":
			                a = n.depends.call(e, e)
			        }
			        a ? t[i] = void 0 === n.param || n.param : delete t[i]
			    }
			}),
			$.each(t,
			function (i, n) {
			    t[i] = $.isFunction(n) ? n(e) : n
			}),
			$.each(["minlength", "maxlength", "min", "max"],
			function () {
			    t[this] && (t[this] = Number(t[this]))
			}),
			$.each(["rangelength", "range"],
			function () {
			    t[this] && (t[this] = [Number(t[this][0]), Number(t[this][1])])
			}),
			$.validator.autoCreateRanges && (t.min && t.max && (t.range = [t.min, t.max], delete t.min, delete t.max), t.minlength && t.maxlength && (t.rangelength = [t.minlength, t.maxlength], delete t.minlength, delete t.maxlength)),
			t.messages && delete t.messages,
			t
	    },
	    normalizeRule: function (t) {
	        if ("string" == typeof t) {
	            var e = {};
	            $.each(t.split(/\s/),
				function () {
				    e[this] = !0
				}),
				t = e
	        }
	        return t
	    },
	    addMethod: function (t, e, i) {
	        $.validator.methods[t] = e,
			$.validator.messages[t] = void 0 != i ? i : $.validator.messages[t],
			e.length < 3 && $.validator.addClassRules(t, $.validator.normalizeRule(t))
	    },
	    methods: {
	        required: function (t, e, i) {
	            if (!this.depend(i, e)) return "dependency-mismatch";
	            switch (e.nodeName.toLowerCase()) {
	                case "select":
	                    var n = $(e).val();
	                    return n && n.length > 0;
	                case "input":
	                    if (this.checkable(e)) return this.getLength(t, e) > 0;
	                default:
	                    return $.trim(t).length > 0
	            }
	        },
	        remote: function (t, e, i) {
	            if (this.optional(e)) return "dependency-mismatch";
	            var n = this.previousValue(e);
	            if (this.settings.messages[e.name] || (this.settings.messages[e.name] = {}), n.originalMessage = this.settings.messages[e.name].remote, this.settings.messages[e.name].remote = n.message, i = "string" == typeof i && {
	                url: i
	            } || i, this.pending[e.name]) return "pending";
	            if (n.old === t) return n.valid;
	            n.old = t;
	            var a = this;
	            this.startRequest(e);
	            var r = {};
	            return r[e.name] = t,
				$.ajax($.extend(!0, {
				    url: i,
				    mode: "abort",
				    port: "validate" + e.name,
				    dataType: "json",
				    data: r,
				    success: function (i) {
				        a.settings.messages[e.name].remote = n.originalMessage;
				        var r = i === !0;
				        if (r) {
				            var s = a.formSubmitted;
				            a.prepareElement(e),
							a.formSubmitted = s,
							a.successList.push(e),
							a.showErrors()
				        } else {
				            var u = {},
							o = i || a.defaultMessage(e, "remote");
				            u[e.name] = n.message = $.isFunction(o) ? o(t) : o,
							a.showErrors(u)
				        }
				        n.valid = r,
						a.stopRequest(e, r)
				    }
				},
				i)),
				"pending"
	        },
	        minlength: function (t, e, i) {
	            return this.optional(e) || this.getLength($.trim(t), e) >= i
	        },
	        maxlength: function (t, e, i) {
	            return this.optional(e) || this.getLength($.trim(t), e) <= i
	        },
	        rangelength: function (t, e, i) {
	            var n = this.getLength($.trim(t), e);
	            return this.optional(e) || n >= i[0] && n <= i[1]
	        },
	        min: function (t, e, i) {
	            return this.optional(e) || t >= i
	        },
	        max: function (t, e, i) {
	            return this.optional(e) || t <= i
	        },
	        range: function (t, e, i) {
	            return this.optional(e) || t >= i[0] && t <= i[1]
	        },
	        email: function (t, e) {
	            return this.optional(e) || /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/i.test(t)
	        },
	        url: function (t, e) {
	            return this.optional(e) || /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(t)
	        },
	        date: function (t, e) {
	            return this.optional(e) || !/Invalid|NaN/.test(new Date(t))
	        },
	        dateISO: function (t, e) {
	            return this.optional(e) || /^\d{4}[\/-]\d{1,2}[\/-]\d{1,2}$/.test(t)
	        },
	        number: function (t, e) {
	            return this.optional(e) || /^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/.test(t)
	        },
	        digits: function (t, e) {
	            return this.optional(e) || /^\d+$/.test(t)
	        },
	        creditcard: function (t, e) {
	            if (this.optional(e)) return "dependency-mismatch";
	            if (/[^0-9 -]+/.test(t)) return !1;
	            var i = 0,
				n = 0,
				a = !1;
	            t = t.replace(/\D/g, "");
	            for (var r = t.length - 1; r >= 0; r--) {
	                var s = t.charAt(r),
					n = parseInt(s, 10);
	                a && (n *= 2) > 9 && (n -= 9),
					i += n,
					a = !a
	            }
	            return i % 10 == 0
	        },
	        accept: function (t, e, i) {
	            return i = "string" == typeof i ? i.replace(/,/g, "|") : "png|jpe?g|gif",
				this.optional(e) || t.match(new RegExp(".(" + i + ")$", "i"))
	        },
	        equalTo: function (t, e, i) {
	            var n = $(i).unbind(".validate-equalTo").bind("blur.validate-equalTo",
				function () {
				    $(e).valid()
				});
	            return t == n.val()
	        },
	        notEqualTo: function (t, e, i) {
	            var n = $(i).unbind(".validate-notEqualTo").bind("blur.validate-notEqualTo",
				function () {
				    $(e).valid()
				});
	            return !(t == n.val())
	        },
	        largerThan: function (value, element, param) {
	            var target = $(param).unbind(".validate-largerThan").bind("blur.validate-largerThan",
				function () {
				    $(element).valid()
				}),
				targetVal = target.val();
	            return !(!isNaN(value) && "" != value && "0" != value) || (!(!isNaN(targetVal) && "" != targetVal) || eval(value) > eval(targetVal))
	        },
	        lessThan: function (value, element, param) {
	            var target = $(param).unbind(".validate-lessThan").bind("blur.validate-lessThan",
				function () {
				    $(element).valid()
				}),
				targetVal = target.val();
	            return !(!isNaN(value) && "" != value && "0" != value) || (!(!isNaN(targetVal) && "" != targetVal) || eval(value) < eval(targetVal))
	        }
	    }
	}),
	$.format = $.validator.format
}(jQuery),
function ($) {
    var t = {};
    if ($.ajaxPrefilter) $.ajaxPrefilter(function (e, i, n) {
        var a = e.port;
        "abort" == e.mode && (t[a] && t[a].abort(), t[a] = n)
    });
    else {
        var e = $.ajax;
        $.ajax = function (i) {
            var n = ("mode" in i ? i : $.ajaxSettings).mode,
			a = ("port" in i ? i : $.ajaxSettings).port;
            return "abort" == n ? (t[a] && t[a].abort(), t[a] = e.apply(this, arguments)) : e.apply(this, arguments)
        }
    }
}(jQuery),
function ($) {
    jQuery.event.special.focusin || jQuery.event.special.focusout || !document.addEventListener || $.each({
        focus: "focusin",
        blur: "focusout"
    },
	function (t, e) {
	    function i(t) {
	        return t = $.event.fix(t),
			t.type = e,
			"undefined" != typeof $.event.handle ? $.event.handle.call(this, t) : null
	    }
	    $.event.special[e] = {
	        setup: function () {
	            this.addEventListener(t, i, !0)
	        },
	        teardown: function () {
	            this.removeEventListener(t, i, !0)
	        },
	        handler: function (t) {
	            return arguments[0] = $.event.fix(t),
				arguments[0].type = e,
				$.event.handle.apply(this, arguments)
	        }
	    }
	}),
	$.extend($.fn, {
	    validateDelegate: function (t, e, i) {
	        return this.bind(e,
			function (e) {
			    var n = $(e.target);
			    if (n.is(t)) return i.apply(n, arguments)
			})
	    }
	})
}(jQuery),
jQuery.validator.addMethod("stringCheck",
function (t, e) {
    return this.optional(e) || /^[\u0391-\uFFE5\w]+$/.test(t)
},
"只能包括中文字、英文字母、数字和下划线"),
jQuery.validator.addMethod("stringCheckTwo",
function (t, e) {
    return this.optional(e) || /^[a-zA-Z0-9_]{1,}$/.test(t)
},
"只能包括英文字母、数字和下划线"),
jQuery.validator.addMethod("stringCheckThree",
function (t, e) {
    return this.optional(e) || /^[a-zA-Z]*$/.test(t)
},
"只能是英文字母"),
jQuery.validator.addMethod("stringCheckFour",
function (t, e) {
    return this.optional(e) || /^[\u4E00-\u9FA5]+$/.test(t)
},
"只能是中文"),
jQuery.validator.addMethod("stringCheckFive",
function (t, e) {
    return this.optional(e) || /^[a-zA-Z0-9_.\/]{1,}$/.test(t)
},
"只能包括英文字母、数字和下划线、点、反斜杠"),
jQuery.validator.addMethod("byteRangeLength",
function (t, e, i) {
    for (var n = t.length,
	a = 0; a < t.length; a++) t.charCodeAt(a) > 127 && n++;
    return this.optional(e) || n >= i[0] && n <= i[1]
},
jQuery.validator.format("请确保输入的值在{0}-{1}个字节之间(一个中文字算2个字节)")),
jQuery.validator.addMethod("isIdCardNo",
function (t, e) {
    return this.optional(e) || /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/.test(t)
},
"请正确输入您的身份证号码"),
jQuery.validator.addMethod("isWebSite",
function (t, e) {
    return this.optional(e) || /^[a-z0-9-]{1,}$/.test(t)
},
"只能包括英文字母、数字和中横线"),
jQuery.validator.addMethod("isMobile",
function (t, e) {
    var i = t.length,
	n = /^((1[0-9]{1})+\d{9})$/;
    return this.optional(e) || 11 == i && n.test(t)
},
"请输入合法的手机号码"),
jQuery.validator.addMethod("isTel",
function (t, e) {
    var i = /^\d{3,4}-?\d{7,9}$/;
    return this.optional(e) || i.test(t)
},
"请正确填写您的电话号码"),
jQuery.validator.addMethod("isPhone",
function (t, e) {
    var i = t.length,
	n = /^((1[3|5|7|8][0-9]{1})+\d{8})$/,
	a = /^\d{3,4}-?\d{7,9}$/;
    return this.optional(e) || a.test(t) || 11 == i && n.test(t)
},
"请正确填写您的联系电话"),
jQuery.validator.addMethod("isZipCode",
function (t, e) {
    var i = /^[0-9]{6}$/;
    return this.optional(e) || i.test(t)
},
"请正确填写您的邮政编码"),
jQuery.validator.addMethod("isQQ",
function (t, e) {
    var i = /^[1-9]\d{4,11}$/;
    return this.optional(e) || i.test(t)
},
"QQ号码格式错误"),
jQuery.validator.addMethod("ints",
function (t, e) {
    var i = /^[-]{0,1}[0-9]{1,}$/;
    return this.optional(e) || i.test(t)
},
"请输入整数，包括负数、0、整数"),
jQuery.validator.addMethod("check_common_num",
function (t, e) {
    return t = $.trim(t),
	!t || /^[a-zA-Z0-9_\-\.]{1,}$/.test(t)
},
"编号只允许出现字母、数字和分割符（- _ .）"),
function ($) {
    $.extend({
        metadata: {
            defaults: {
                type: "class",
                name: "metadata",
                cre: /({.*})/,
                single: "metadata"
            },
            setType: function (t, e) {
                this.defaults.type = t,
				this.defaults.name = e
            },
            get: function (elem, opts) {
                var settings = $.extend({},
				this.defaults, opts);
                settings.single.length || (settings.single = "metadata");
                var data = $.data(elem, settings.single);
                if (data) return data;
                if (data = "{}", "class" == settings.type) {
                    var m = settings.cre.exec(elem.className);
                    m && (data = m[1])
                } else if ("elem" == settings.type) {
                    if (!elem.getElementsByTagName) return;
                    var e = elem.getElementsByTagName(settings.name);
                    e.length && (data = $.trim(e[0].innerHTML))
                } else if (void 0 != elem.getAttribute) {
                    var attr = elem.getAttribute(settings.name);
                    attr && (data = attr)
                }
                return data.indexOf("{") < 0 && (data = "{" + data + "}"),
				data = eval("(" + data + ")"),
				$.data(elem, settings.single, data),
				data
            }
        }
    }),
	$.fn.metadata = function (t) {
	    return $.metadata.get(this[0], t)
	}
}(jQuery),
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
jQuery.fn.pagination = function (e, t) {
    return t = jQuery.extend({
        items_per_page: 10,
        num_display_entries: 10,
        current_page: 0,
        num_edge_entries: 0,
        link_to: "#",
        prev_text: '<i class="fa fa-chevron-left"></i>',
        next_text: '<i class="fa fa-chevron-right"></i>',
        ellipse_text: "...",
        prev_show_always: !0,
        next_show_always: !0,
        init_apply: !1,
        callback: function () {
            return !1
        }
    },
	t || {}),
	this.each(function () {
	    function a() {
	        return Math.ceil(e / t.items_per_page)
	    }
	    function i() {
	        var e = Math.ceil(t.num_display_entries / 2),
			i = a(),
			n = i - t.num_display_entries,
			s = r > e ? Math.max(Math.min(r - e, n), 0) : 0,
			l = r > e ? Math.min(r + e, i) : Math.min(t.num_display_entries, i);
	        return [s, l]
	    }
	    function n(e, a) {
	        r = e,
			s();
	        var i = t.callback(e, l);
	        return i || (a.stopPropagation ? a.stopPropagation() : a.cancelBubble = !0),
			i
	    }
	    function s() {
	        l.empty();
	        var s = i(),
			p = a(),
			o = function (e) {
			    return function (t) {
			        return n(e, t)
			    }
			},
			c = function (e, a) {
			    if (e = e < 0 ? 0 : e < p ? e : p - 1, a = jQuery.extend({
			        text: e + 1,
			        classes: ""
			    },
				a || {}), e == r) var i = jQuery("<li class='" + (t.prev_text != a.text && t.next_text != a.text ? "" : "active") + "active'><a>" + a.text + "</a></li>");
			    else var i = jQuery("<li><a>" + a.text + "</a></li>").bind("click", o(e)).attr("href", t.link_to.replace(/__id__/, e));
			    a.classes && i.addClass(a.classes),
				l.append(i)
			};
	        if ("undefined" == typeof t.dialog_pagesize || !t.dialog_pagesize) var u = function (e) {
	            $.cookie("pagesize_cookie", e, {
	                expires: 94608e4,
	                path: "/"
	            }),
				$(_ + "#pageSize_").attr("data-value", e),
				$(_ + "#pageSize_ strong").text(e),
				$(_ + "#pageSize_").next().find("li").each(function () {
				    $(this).text() == e ? $(this).attr("class", "active") : $(this).attr("class", "")
				}),
				$.DHB.refresh()
	        };
	        e = !e || e < 0 ? 1 : e;
	        var d = jQuery("<li><a>共 <strong>" + e + " </strong>条</a></li>");
	        if (l.append(d), "undefined" == typeof t.dialog_pagesize || !t.dialog_pagesize) {
	            var g = $.cookie("pagesize_cookie");
	            g || (g = publicSettings.pageSize, $.cookie("pagesize_cookie", g, {
	                expires: 94608e4,
	                path: "/"
	            }));
	            for (var x = '<li class="dropdown dropup" style="width:100px;max-width:100;min-width:100;"><a data-toggle="dropdown" id="pageSize_" data-value="' + g + '">每页 <strong style="text-align:center;width:23px;display:inline-block;">' + g + '</strong> 条<span class="caret"></span></a><ul class="dropdown-menu">',
				f = new Array(10, 20, 30, 50, 80, 100), h = 0; h < f.length; h++) x += "<li " + (g == f[h] ? 'class="active"' : "") + "><a>" + f[h] + "</a></li>";
	            x += "</ul></li>",
				l.append($(x)),
				$(_ + "#pageSize_").next().find("li").unbind("click").bind("click",
				function (e) {
				    u($(this).text())
				})
	        }
	        if (t.prev_text && (r > 0 || t.prev_show_always) && c(r - 1, {
	            text: t.prev_text,
	            classes: "prev"
	        }), s[0] > 0 && t.num_edge_entries > 0) {
	            for (var v = Math.min(t.num_edge_entries, s[0]), h = 0; h < v; h++) c(h);
	            t.num_edge_entries < s[0] && t.ellipse_text && jQuery("<li><a>" + t.ellipse_text + "</a></li>").appendTo(l)
	        }
	        for (var h = s[0]; h < s[1]; h++) c(h);
	        if (s[1] < p && t.num_edge_entries > 0) {
	            p - t.num_edge_entries > s[1] && t.ellipse_text && jQuery("<li><a>" + t.ellipse_text + "</a></li>").appendTo(l);
	            for (var m = Math.max(p - t.num_edge_entries, s[1]), h = m; h < p; h++) c(h)
	        }
	        t.next_text && (r < p - 1 || t.next_show_always) && c(r + 1, {
	            text: t.next_text,
	            classes: "next"
	        })
	    }
	    var r = t.current_page;
	    t.items_per_page = !t.items_per_page || t.items_per_page < 0 ? 1 : t.items_per_page;
	    var l = jQuery(this);
	    this.selectPage = function (e) {
	        n(e)
	    },
		this.prevPage = function () {
		    return r > 0 && (n(r - 1), !0)
		},
		this.nextPage = function () {
		    return r < a() - 1 && (n(r + 1), !0)
		},
		s(),
		t.init_apply && t.callback(r, this)
	})
}; !
function ($) {
    $.fn.niceTitle = function (t) {
        var i, e = $.extend({},
		$.fn.niceTitle.defaults, t),
		r = this,
		s = "",
		a = "",
		o = !1,
		n = $(window).width(),
		p = $(window).height(),
		l = $(document).scrollTop();
        $(document).height();
        this.initialize = function (t) {
            $(window).scroll(function () {
                l = $(document).scrollTop()
            });
            var e = "";
            return e = jQuery.browser.msie ? '<div id="niceTitle" class="popover"><span><span class="r1"></span><span class="r2"></span><span class="r3"></span><span class="r4"></span></span><div id="niceTitle-ie"><p><em></em></p></div><span><span class="r4"></span><span class="r3"></span><span class="r2"></span><span class="r1"></span></span></div>' : '<div id="niceTitle" class="popover"><p><em></em></p></div>',
			$(r).mouseover(function (r) {
			    if ("undefined" != typeof $(this).attr("data-title")) var d = $(this).attr("data-title");
			    else if ("undefined" != typeof $(this).attr("title")) {
			        $(this).attr("data-title", $(this).attr("title"));
			        var d = $(this).attr("data-title");
			        $(this).attr("title", "")
			    }
			    if ("undefined" != typeof d && "undefined" != d && d) {
			        $(this).is("a") ? this.tmpHref = this.href : $(this).is("img") ? this.tmpHref = this.src : this.tmpHref = "",
					i = $(this).find("img"),
					i.length > 0 && (s = i.attr("alt"), i.attr("alt", ""), a = i.attr("title"), i.attr("title", ""), o = !0);
			        var h = t.urlSize;
			        $(this).attr("title", ""),
					this.tmpHref.length > 0 && t.showLink ? (this.tmpHref = this.tmpHref.length > h ? this.tmpHref.toString().substring(0, h) + "..." : this.tmpHref, $(e).appendTo("body").find("p").prepend($(this).data("title")).css({
					    color: t.titleColor
					}).find("em").text(this.tmpHref).css({
					    color: t.urlColor
					})) : $(e).appendTo("body").find("p").prepend($(this).data("title")).css({
					    color: t.titleColor
					}).find("em").remove();
			        var c = $("#niceTitle");
			        c.css({
			            position: "absolute",
			            "text-align": "left",
			            padding: "5px",
			            opacity: t.opacity,
			            top: p + l - r.pageY - t.y - 10 < c.height() ? r.pageY - c.height() - t.y + "px" : r.pageY + t.y + "px",
			            left: n - r.pageX - t.x - 10 < c.width() ? r.pageX - c.width() - t.x + "px" : r.pageX + t.x + "px",
			            "z-index": t.zIndex,
			            "max-width": t.maxWidth + "px",
			            width: "auto !important",
			            "min-height": t.minHeight + "px",
			            "-moz-border-radius": t.radius + "px",
			            "-webkit-border-radius": t.radius + "px",
			            "word-wrap": "break-word",
			            "white-space": "normal",
			            "word-break": "break-all"
			        }),
					jQuery.browser.msie ? ($("#niceTitle span").css({
					    "background-color": t.bgColor,
					    border: "1px solid " + t.borColor
					}), $("#niceTitle-ie").css({
					    background: t.bgColor,
					    border: "1px solid " + t.borColor
					})) : c.css({
					    background: t.bgColor,
					    border: "1px solid " + t.borColor
					}),
					c.show("fast")
			    }
			    return !1
			}).mouseout(function (t) {
			    return $("#niceTitle").remove(),
				o && (i.attr("alt", s), i.attr("title", a)),
				!1
			}).mousemove(function (i) {
			    var e = $("#niceTitle");
			    return e.css({
			        top: p + l - i.pageY - t.y - 10 < e.height() ? i.pageY - e.height() - t.y + "px" : i.pageY + t.y + "px",
			        left: n - i.pageX - t.x - 10 < e.width() ? i.pageX - e.width() - t.x + "px" : i.pageX + t.x + "px"
			    }),
				!1
			}),
			r
        },
		this.initialize(e)
    },
	$.fn.niceTitle.defaults = {
	    x: 10,
	    y: 10,
	    urlSize: 30,
	    bgColor: "#fff",
	    borColor: "#CDCDCD",
	    titleColor: "#58666e",
	    urlColor: "#F60",
	    zIndex: 999,
	    maxWidth: 450,
	    minHeight: 30,
	    opacity: 1,
	    radius: 6,
	    showLink: !0
	}
}(jQuery); !
function ($, e, t, o) {
    "use strict";
    var s = "treeview",
	n = {};
    n.settings = {
        injectStyle: !0,
        levels: 2,
        expandIcon: "glyphicon glyphicon-plus",
        collapseIcon: "glyphicon glyphicon-minus",
        emptyIcon: "glyphicon",
        nodeIcon: "",
        selectedIcon: "",
        checkedIcon: "glyphicon-check",
        uncheckedIcon: "glyphicon-unchecked",
        color: o,
        backColor: o,
        borderColor: o,
        onhoverColor: "#F5F5F5",
        selectedColor: "#428bca",
        selectedBackColor: "#edf1f2",
        searchResultColor: "#D9534F",
        searchResultBackColor: o,
        enableLinks: !1,
        highlightSelected: !0,
        highlightSearchResults: !0,
        showBorder: !0,
        showIcon: !0,
        showCheckbox: !1,
        showTags: !1,
        multiSelect: !1,
        onNodeChecked: o,
        onNodeCollapsed: o,
        onNodeDisabled: o,
        onNodeEnabled: o,
        onNodeExpanded: o,
        onNodeSelected: o,
        onNodeUnchecked: o,
        onNodeUnselected: o,
        onSearchComplete: o,
        onSearchCleared: o
    },
	n.options = {
	    silent: !1,
	    ignoreChildren: !1
	},
	n.searchOptions = {
	    ignoreCase: !0,
	    exactMatch: !1,
	    revealResults: !0
	};
    var i = function (e, t) {
        return this.$element = $(e),
		this.elementId = e.id,
		this.styleId = this.elementId + "-style",
		this.init(t),
		{
		    options: this.options,
		    init: $.proxy(this.init, this),
		    remove: $.proxy(this.remove, this),
		    getNode: $.proxy(this.getNode, this),
		    getParent: $.proxy(this.getParent, this),
		    getSiblings: $.proxy(this.getSiblings, this),
		    getSelected: $.proxy(this.getSelected, this),
		    getUnselected: $.proxy(this.getUnselected, this),
		    getExpanded: $.proxy(this.getExpanded, this),
		    getCollapsed: $.proxy(this.getCollapsed, this),
		    getChecked: $.proxy(this.getChecked, this),
		    getUnchecked: $.proxy(this.getUnchecked, this),
		    getDisabled: $.proxy(this.getDisabled, this),
		    getEnabled: $.proxy(this.getEnabled, this),
		    selectNode: $.proxy(this.selectNode, this),
		    unselectNode: $.proxy(this.unselectNode, this),
		    toggleNodeSelected: $.proxy(this.toggleNodeSelected, this),
		    collapseAll: $.proxy(this.collapseAll, this),
		    collapseNode: $.proxy(this.collapseNode, this),
		    expandAll: $.proxy(this.expandAll, this),
		    expandNode: $.proxy(this.expandNode, this),
		    toggleNodeExpanded: $.proxy(this.toggleNodeExpanded, this),
		    revealNode: $.proxy(this.revealNode, this),
		    checkAll: $.proxy(this.checkAll, this),
		    checkNode: $.proxy(this.checkNode, this),
		    uncheckAll: $.proxy(this.uncheckAll, this),
		    uncheckNode: $.proxy(this.uncheckNode, this),
		    toggleNodeChecked: $.proxy(this.toggleNodeChecked, this),
		    disableAll: $.proxy(this.disableAll, this),
		    disableNode: $.proxy(this.disableNode, this),
		    enableAll: $.proxy(this.enableAll, this),
		    enableNode: $.proxy(this.enableNode, this),
		    toggleNodeDisabled: $.proxy(this.toggleNodeDisabled, this),
		    search: $.proxy(this.search, this),
		    clearSearch: $.proxy(this.clearSearch, this)
		}
    };
    i.prototype.init = function (e) {
        this.tree = [],
		this.nodes = [],
		e.data && ("string" == typeof e.data && (e.data = $.parseJSON(e.data)), this.tree = $.extend(!0, [], e.data), delete e.data),
		this.options = $.extend({},
		n.settings, e),
		this.destroy(),
		this.subscribeEvents(),
		this.setInitialStates({
		    nodes: this.tree
		},
		0),
		this.render()
    },
	i.prototype.remove = function () {
	    this.destroy(),
		$.removeData(this, s),
		$("#" + this.styleId).remove()
	},
	i.prototype.destroy = function () {
	    this.initialized && (this.$wrapper.remove(), this.$wrapper = null, this.unsubscribeEvents(), this.initialized = !1)
	},
	i.prototype.unsubscribeEvents = function () {
	    this.$element.off("click"),
		this.$element.off("nodeChecked"),
		this.$element.off("nodeCollapsed"),
		this.$element.off("nodeDisabled"),
		this.$element.off("nodeEnabled"),
		this.$element.off("nodeExpanded"),
		this.$element.off("nodeSelected"),
		this.$element.off("nodeUnchecked"),
		this.$element.off("nodeUnselected"),
		this.$element.off("searchComplete"),
		this.$element.off("searchCleared")
	},
	i.prototype.subscribeEvents = function () {
	    this.unsubscribeEvents(),
		this.$element.on("click", $.proxy(this.clickHandler, this)),
		"function" == typeof this.options.onNodeChecked && this.$element.on("nodeChecked", this.options.onNodeChecked),
		"function" == typeof this.options.onNodeCollapsed && this.$element.on("nodeCollapsed", this.options.onNodeCollapsed),
		"function" == typeof this.options.onNodeDisabled && this.$element.on("nodeDisabled", this.options.onNodeDisabled),
		"function" == typeof this.options.onNodeEnabled && this.$element.on("nodeEnabled", this.options.onNodeEnabled),
		"function" == typeof this.options.onNodeExpanded && this.$element.on("nodeExpanded", this.options.onNodeExpanded),
		"function" == typeof this.options.onNodeSelected && this.$element.on("nodeSelected", this.options.onNodeSelected),
		"function" == typeof this.options.onNodeUnchecked && this.$element.on("nodeUnchecked", this.options.onNodeUnchecked),
		"function" == typeof this.options.onNodeUnselected && this.$element.on("nodeUnselected", this.options.onNodeUnselected),
		"function" == typeof this.options.onSearchComplete && this.$element.on("searchComplete", this.options.onSearchComplete),
		"function" == typeof this.options.onSearchCleared && this.$element.on("searchCleared", this.options.onSearchCleared)
	},
	i.prototype.setInitialStates = function (e, t) {
	    if (e.nodes) {
	        t += 1;
	        var o = e,
			s = this;
	        $.each(e.nodes,
			function (e, n) {
			    n.nodeId = s.nodes.length,
				n.parentId = o.nodeId,
				n.hasOwnProperty("selectable") || (n.selectable = !0),
				n.state = n.state || {},
				n.state.hasOwnProperty("checked") || (n.state.checked = !1),
				n.state.hasOwnProperty("disabled") || (n.state.disabled = !1),
				n.state.hasOwnProperty("expanded") || (!n.state.disabled && t < s.options.levels && n.nodes && n.nodes.length > 0 ? n.state.expanded = !0 : n.state.expanded = !1),
				n.state.hasOwnProperty("selected") || (n.state.selected = !1),
				s.nodes.push(n),
				n.nodes && s.setInitialStates(n, t)
			})
	    }
	},
	i.prototype.clickHandler = function (e) {
	    this.options.enableLinks || e.preventDefault();
	    var t = $(e.target),
		o = this.findNode(t);
	    if (o && !o.state.disabled) {
	        var s = t.attr("class") ? t.attr("class").split(" ") : [];
	        s.indexOf("expand-icon") !== -1 ? (this.toggleExpandedState(o, n.options), this.render()) : s.indexOf("check-icon") !== -1 ? (this.toggleCheckedState(o, n.options), this.render()) : (o.selectable ? this.toggleSelectedState(o, n.options) : this.toggleExpandedState(o, n.options), this.render()),
			t.hasClass("icon") && e.stopPropagation()
	    }
	},
	i.prototype.findNode = function (e) {
	    var t = e.closest("li.list-group-item").attr("data-nodeid"),
		o = this.nodes[t];
	    return o || console.log("Error: node does not exist"),
		o
	},
	i.prototype.toggleExpandedState = function (e, t) {
	    e && this.setExpandedState(e, !e.state.expanded, t)
	},
	i.prototype.setExpandedState = function (e, t, o) {
	    t !== e.state.expanded && (t && e.nodes ? (e.state.expanded = !0, o.silent || this.$element.trigger("nodeExpanded", $.extend(!0, {},
		e))) : t || (e.state.expanded = !1, o.silent || this.$element.trigger("nodeCollapsed", $.extend(!0, {},
		e)), e.nodes && !o.ignoreChildren && $.each(e.nodes, $.proxy(function (e, t) {
		    this.setExpandedState(t, !1, o)
		},
		this))))
	},
	i.prototype.toggleSelectedState = function (e, t) {
	    e && this.setSelectedState(e, !e.state.selected, t)
	},
	i.prototype.setSelectedState = function (e, t, o) {
	    t !== e.state.selected && (t ? (this.options.multiSelect || $.each(this.findNodes("true", "g", "state.selected"), $.proxy(function (e, t) {
	        this.setSelectedState(t, !1, o)
	    },
		this)), e.state.selected = !0, o.silent || this.$element.trigger("nodeSelected", $.extend(!0, {},
		e))) : (e.state.selected = !1, o.silent || this.$element.trigger("nodeUnselected", $.extend(!0, {},
		e))))
	},
	i.prototype.toggleCheckedState = function (e, t) {
	    e && this.setCheckedState(e, !e.state.checked, t)
	},
	i.prototype.setCheckedState = function (e, t, o) {
	    t !== e.state.checked && (t ? (e.state.checked = !0, o.silent || this.$element.trigger("nodeChecked", $.extend(!0, {},
		e))) : (e.state.checked = !1, o.silent || this.$element.trigger("nodeUnchecked", $.extend(!0, {},
		e))))
	},
	i.prototype.setDisabledState = function (e, t, o) {
	    t !== e.state.disabled && (t ? (e.state.disabled = !0, this.setExpandedState(e, !1, o), this.setSelectedState(e, !1, o), this.setCheckedState(e, !1, o), o.silent || this.$element.trigger("nodeDisabled", $.extend(!0, {},
		e))) : (e.state.disabled = !1, o.silent || this.$element.trigger("nodeEnabled", $.extend(!0, {},
		e))))
	},
	i.prototype.render = function () {
	    this.initialized || (this.$element.addClass(s), this.$wrapper = $(this.template.list), this.injectStyle(), this.initialized = !0),
		this.$element.empty().append(this.$wrapper.empty()),
		this.buildTree(this.tree, 0)
	},
	i.prototype.buildTree = function (e, t) {
	    if (e) {
	        t += 1;
	        var o = this;
	        $.each(e,
			function (e, s) {
			    for (var n = $(o.template.item).addClass("node-" + o.elementId).addClass(s.state.checked ? "node-checked" : "").addClass(s.state.disabled ? "node-disabled" : "").addClass(s.state.selected ? "node-selected" : "").addClass(s.searchResult ? "search-result" : "").attr("data-nodeid", s.nodeId).attr("data-href", s.href).attr("style", o.buildStyleOverride(s)), i = 0; i < t - 1; i++) n.append(o.template.indent);
			    var d = [];
			    if (s.nodes ? (d.push("expand-icon"), s.state.expanded ? d.push(o.options.collapseIcon) : d.push(o.options.expandIcon)) : d.push(o.options.emptyIcon), n.append($(o.template.icon).addClass(d.join(" "))), o.options.showIcon) {
			        var d = ["node-icon"];
			        d.push(s.icon || o.options.nodeIcon),
					s.state.selected && (d.pop(), d.push(s.selectedIcon || o.options.selectedIcon || s.icon || o.options.nodeIcon)),
					n.append($(o.template.icon).addClass(d.join(" ")))
			    }
			    if (o.options.showCheckbox) {
			        var d = ["check-icon"];
			        s.state.checked ? d.push(o.options.checkedIcon) : d.push(o.options.uncheckedIcon),
					n.append($(o.template.icon).addClass(d.join(" ")))
			    }
			    if (o.options.enableLinks ? n.append($(o.template.link).attr("href", s.href).append(s.text)) : n.append(s.text), o.options.showTags && s.tags && $.each(s.tags,
				function (e, t) {
					n.append($(o.template.badge).append(t))
			    }), o.$wrapper.append(n), n.find('span:not(":last")').css({
			        color: "rgb(213, 211, 213)"
			    }), s.nodes && s.state.expanded && !s.state.disabled) return o.buildTree(s.nodes, t)
			})
	    }
	},
	i.prototype.buildStyleOverride = function (e) {
	    if (e.state.disabled) return "";
	    var t = e.color,
		o = e.backColor;
	    return this.options.highlightSelected && e.state.selected && (this.options.selectedColor && (t = this.options.selectedColor), this.options.selectedBackColor && (o = this.options.selectedBackColor)),
		this.options.highlightSearchResults && e.searchResult && !e.state.disabled && (this.options.searchResultColor && (t = this.options.searchResultColor), this.options.searchResultBackColor && (o = this.options.searchResultBackColor)),
		"color:" + t + ";background-color:" + o + ";"
	},
	i.prototype.injectStyle = function () {
	    this.options.injectStyle && !t.getElementById(this.styleId) && $('<style type="text/css" id="' + this.styleId + '"> ' + this.buildStyle() + " </style>").appendTo("head")
	},
	i.prototype.buildStyle = function () {
	    var e = ".node-" + this.elementId + "{";
	    return this.options.color && (e += "color:" + this.options.color + ";"),
		this.options.backColor && (e += "background-color:" + this.options.backColor + ";"),
		this.options.showBorder ? this.options.borderColor && (e += "border:1px solid " + this.options.borderColor + ";") : e += "border:none;",
		e += "}",
		this.options.onhoverColor && (e += ".node-" + this.elementId + ":not(.node-disabled):hover{background-color:" + this.options.onhoverColor + ";}"),
		this.css + e
	},
	i.prototype.template = {
	    list: '<ul class="list-group"></ul>',
	    item: '<li class="list-group-item"></li>',
	    indent: '<span class="indent"></span>',
	    icon: '<span class="icon"></span>',
	    link: '<a href="#" style="color:inherit;"></a>',
	    badge: '<span class="badge"></span>'
	},
	i.prototype.css = ".treeview .list-group-item{cursor:pointer}.treeview span.indent{margin-left:10px;margin-right:10px}.treeview span.icon{width:12px;margin-right:2px}.treeview span.check-icon{width:12px;margin-right:3px}.treeview .node-disabled{color:silver;cursor:not-allowed}",
	i.prototype.getNode = function (e) {
	    return this.nodes[e]
	},
	i.prototype.getParent = function (e) {
	    var t = this.identifyNode(e);
	    return this.nodes[t.parentId]
	},
	i.prototype.getSiblings = function (e) {
	    var t = this.identifyNode(e),
		o = this.getParent(t),
		s = o ? o.nodes : this.tree;
	    return s.filter(function (e) {
	        return e.nodeId !== t.nodeId
	    })
	},
	i.prototype.getSelected = function () {
	    return this.findNodes("true", "g", "state.selected")
	},
	i.prototype.getUnselected = function () {
	    return this.findNodes("false", "g", "state.selected")
	},
	i.prototype.getExpanded = function () {
	    return this.findNodes("true", "g", "state.expanded")
	},
	i.prototype.getCollapsed = function () {
	    return this.findNodes("false", "g", "state.expanded")
	},
	i.prototype.getChecked = function () {
	    return this.findNodes("true", "g", "state.checked")
	},
	i.prototype.getUnchecked = function () {
	    return this.findNodes("false", "g", "state.checked")
	},
	i.prototype.getDisabled = function () {
	    return this.findNodes("true", "g", "state.disabled")
	},
	i.prototype.getEnabled = function () {
	    return this.findNodes("false", "g", "state.disabled")
	},
	i.prototype.selectNode = function (e, t) {
	    this.forEachIdentifier(e, t, $.proxy(function (e, t) {
	        this.setSelectedState(e, !0, t)
	    },
		this)),
		this.render()
	},
	i.prototype.unselectNode = function (e, t) {
	    this.forEachIdentifier(e, t, $.proxy(function (e, t) {
	        this.setSelectedState(e, !1, t)
	    },
		this)),
		this.render()
	},
	i.prototype.toggleNodeSelected = function (e, t) {
	    this.forEachIdentifier(e, t, $.proxy(function (e, t) {
	        this.toggleSelectedState(e, t)
	    },
		this)),
		this.render()
	},
	i.prototype.collapseAll = function (e) {
	    var t = this.findNodes("true", "g", "state.expanded");
	    this.forEachIdentifier(t, e, $.proxy(function (e, t) {
	        this.setExpandedState(e, !1, t)
	    },
		this)),
		this.render()
	},
	i.prototype.collapseNode = function (e, t) {
	    this.forEachIdentifier(e, t, $.proxy(function (e, t) {
	        this.setExpandedState(e, !1, t)
	    },
		this)),
		this.render()
	},
	i.prototype.expandAll = function (e) {
	    if (e = $.extend({},
		n.options, e), e && e.levels) this.expandLevels(this.tree, e.levels, e);
	    else {
	        var t = this.findNodes("false", "g", "state.expanded");
	        this.forEachIdentifier(t, e, $.proxy(function (e, t) {
	            this.setExpandedState(e, !0, t)
	        },
			this))
	    }
	    this.render()
	},
	i.prototype.expandNode = function (e, t) {
	    this.forEachIdentifier(e, t, $.proxy(function (e, t) {
	        this.setExpandedState(e, !0, t),
			e.nodes && t && t.levels && this.expandLevels(e.nodes, t.levels - 1, t)
	    },
		this)),
		this.render()
	},
	i.prototype.expandLevels = function (e, t, o) {
	    o = $.extend({},
		n.options, o),
		$.each(e, $.proxy(function (e, s) {
		    this.setExpandedState(s, t > 0, o),
			s.nodes && this.expandLevels(s.nodes, t - 1, o)
		},
		this))
	},
	i.prototype.revealNode = function (e, t) {
	    this.forEachIdentifier(e, t, $.proxy(function (e, t) {
	        for (var o = this.getParent(e) ; o;) this.setExpandedState(o, !0, t),
			o = this.getParent(o)
	    },
		this)),
		this.render()
	},
	i.prototype.toggleNodeExpanded = function (e, t) {
	    this.forEachIdentifier(e, t, $.proxy(function (e, t) {
	        this.toggleExpandedState(e, t)
	    },
		this)),
		this.render()
	},
	i.prototype.checkAll = function (e) {
	    var t = this.findNodes("false", "g", "state.checked");
	    this.forEachIdentifier(t, e, $.proxy(function (e, t) {
	        this.setCheckedState(e, !0, t)
	    },
		this)),
		this.render()
	},
	i.prototype.checkNode = function (e, t) {
	    this.forEachIdentifier(e, t, $.proxy(function (e, t) {
	        this.setCheckedState(e, !0, t)
	    },
		this)),
		this.render()
	},
	i.prototype.uncheckAll = function (e) {
	    var t = this.findNodes("true", "g", "state.checked");
	    this.forEachIdentifier(t, e, $.proxy(function (e, t) {
	        this.setCheckedState(e, !1, t)
	    },
		this)),
		this.render()
	},
	i.prototype.uncheckNode = function (e, t) {
	    this.forEachIdentifier(e, t, $.proxy(function (e, t) {
	        this.setCheckedState(e, !1, t)
	    },
		this)),
		this.render()
	},
	i.prototype.toggleNodeChecked = function (e, t) {
	    this.forEachIdentifier(e, t, $.proxy(function (e, t) {
	        this.toggleCheckedState(e, t)
	    },
		this)),
		this.render()
	},
	i.prototype.disableAll = function (e) {
	    var t = this.findNodes("false", "g", "state.disabled");
	    this.forEachIdentifier(t, e, $.proxy(function (e, t) {
	        this.setDisabledState(e, !0, t)
	    },
		this)),
		this.render()
	},
	i.prototype.disableNode = function (e, t) {
	    this.forEachIdentifier(e, t, $.proxy(function (e, t) {
	        this.setDisabledState(e, !0, t)
	    },
		this)),
		this.render()
	},
	i.prototype.enableAll = function (e) {
	    var t = this.findNodes("true", "g", "state.disabled");
	    this.forEachIdentifier(t, e, $.proxy(function (e, t) {
	        this.setDisabledState(e, !1, t)
	    },
		this)),
		this.render()
	},
	i.prototype.enableNode = function (e, t) {
	    this.forEachIdentifier(e, t, $.proxy(function (e, t) {
	        this.setDisabledState(e, !1, t)
	    },
		this)),
		this.render()
	},
	i.prototype.toggleNodeDisabled = function (e, t) {
	    this.forEachIdentifier(e, t, $.proxy(function (e, t) {
	        this.setDisabledState(e, !e.state.disabled, t)
	    },
		this)),
		this.render()
	},
	i.prototype.forEachIdentifier = function (e, t, o) {
	    t = $.extend({},
		n.options, t),
		e instanceof Array || (e = [e]),
		$.each(e, $.proxy(function (e, s) {
		    o(this.identifyNode(s), t)
		},
		this))
	},
	i.prototype.identifyNode = function (e) {
	    return "number" == typeof e ? this.nodes[e] : e
	},
	i.prototype.search = function (e, t) {
	    t = $.extend({},
		n.searchOptions, t),
		this.clearSearch({
		    render: !1
		});
	    var o = [];
	    if (e && e.length > 0) {
	        t.exactMatch && (e = "^" + e + "$");
	        var s = "g";
	        t.ignoreCase && (s += "i"),
			o = this.findNodes(e, s),
			$.each(o,
			function (e, t) {
			    t.searchResult = !0
			})
	    }
	    return t.revealResults ? this.revealNode(o) : this.render(),
		this.$element.trigger("searchComplete", $.extend(!0, {},
		o)),
		o
	},
	i.prototype.clearSearch = function (e) {
	    e = $.extend({},
		{
		    render: !0
		},
		e);
	    var t = $.each(this.findNodes("true", "g", "searchResult"),
		function (e, t) {
		    t.searchResult = !1
		});
	    e.render && this.render(),
		this.$element.trigger("searchCleared", $.extend(!0, {},
		t))
	},
	i.prototype.findNodes = function (e, t, o) {
	    t = t || "g",
		o = o || "text";
	    var s = this;
	    return $.grep(this.nodes,
		function (n) {
		    var i = s.getNodeValue(n, o);
		    if ("string" == typeof i) return i.match(new RegExp(e, t))
		})
	},
	i.prototype.getNodeValue = function (e, t) {
	    var s = t.indexOf(".");
	    if (s > 0) {
	        var n = e[t.substring(0, s)],
			i = t.substring(s + 1, t.length);
	        return this.getNodeValue(n, i)
	    }
	    return e.hasOwnProperty(t) ? e[t].toString() : o
	};
    var d = function (t) {
        e.console && e.console.error(t)
    };
    $.fn[s] = function (e, t) {
        var o;
        return this.each(function () {
            var n = $.data(this, s);
            "string" == typeof e ? n ? $.isFunction(n[e]) && "_" !== e.charAt(0) ? (t instanceof Array || (t = [t]), o = n[e].apply(n, t)) : d("No such method : " + e) : d("Not initialized, can not call method : " + e) : "boolean" == typeof e ? o = n : $.data(this, s, new i(this, $.extend(!0, {},
			e)))
        }),
		o || this
    }
}(jQuery, window, document); !
function ($) {
    var e, t, n, a, o, _, u, i, d, r, l, m, s, p, c, h, v, x, b = $.event.special,
	g = {},
	f = !1,
	y = -1,
	w = "ue_bound",
	z = !1,
	S = {
	    bound_ns_map: {},
	    wheel_ratio: 15,
	    px_radius: 3,
	    ignore_class: ":input",
	    tap_time: 200,
	    held_tap_time: 300
	},
	E = [],
	k = 1,
	D = 4;
    i = function (e) {
        var t, n = 0;
        for (t = this.length; t; 0) this[--t] === e && n++;
        return n
    },
	d = function (e) {
	    var t, n = 0;
	    for (t = this.length; t; 0) this[--t] === e && (this.splice(t, 1), n++, t++);
	    return n
	},
	r = function (e) {
	    return !i.call(this, e) && (this.push(e), !0)
	},
	l = function (e) {
	    if (e && $.isArray(e)) {
	        if (e.remove_val) return console.warn("The array appears to already have listPlus capabilities"),
			e
	    } else e = [];
	    return e.remove_val = d,
		e.match_val = i,
		e.push_uniq = r,
		e
	},
	e = l(),
	t = {
	    setup: function (t, n, a) {
	        var o, _, u, i, d, r = this,
			m = $(r),
			s = {};
	        if (!$.data(this, w)) {
	            for (o = {},
				$.extend(!0, o, S), $.data(r, w, o), d = l(n.slice(0)), d.length && "" !== d[0] || (d = ["000"]), _ = 0; _ < d.length; _++) u = d[_],
				u && (s.hasOwnProperty(u) || (s[u] = !0, i = ".__ue" + u, m.bind("mousedown" + i, h), m.bind("touchstart" + i, v), m.bind("mousewheel" + i, x)));
	            e.push_uniq(r),
				f || ($(document).bind("mousemove.__ue", h), $(document).bind("touchmove.__ue", v), $(document).bind("mouseup.__ue", h), $(document).bind("touchend.__ue", v), $(document).bind("touchcancel.__ue", v), f = !0)
	        }
	    },
	    add: function (e) {
	        var t, n, a, o, _ = this,
			u = $.data(_, w),
			i = e.namespace,
			d = e.type;
	        if (u && (t = u.bound_ns_map, t[d] || (t[d] = {}), i)) for (n = i.split("."), a = 0; a < n.length; a++) o = n[a],
			t[d][o] = !0
	    },
	    remove: function (e) {
	        var t, n, a, o = this,
			_ = $.data(o, w),
			u = _.bound_ns_map,
			i = e.type,
			d = e.namespace;
	        if (u[i]) {
	            if (!d) return void delete u[i];
	            for (t = d.split("."), n = 0; n < t.length; n++) a = t[n],
				u[i][a] && delete u[i][a];
	            $.isEmptyObject(u[i]) && delete u[i]
	        }
	    },
	    teardown: function (t) {
	        var n, a, o, _, u = this,
			i = $(u),
			d = $.data(u, w),
			r = d.bound_ns_map;
	        if ($.isEmptyObject(r)) {
	            for (_ = l(t), _.push_uniq("000"), n = 0; n < _.length; n++) a = _[n],
				a && (o = ".__ue" + a, i.unbind("mousedown" + o), i.unbind("touchstart" + o), i.unbind("mousewheel" + o));
	            $.removeData(u, w),
				e.remove_val(this),
				0 === e.length && ($(document).unbind("mousemove.__ue"), $(document).unbind("touchmove.__ue"), $(document).unbind("mouseup.__ue"), $(document).unbind("touchend.__ue"), $(document).unbind("touchcancel.__ue"), f = !1)
	        }
	    }
	},
	m = function (e) {
	    var t, n = +new Date,
		o = e.motion_id,
		_ = e.motion_map,
		u = e.bound_ns_map;
	    delete _.idto_tapheld,
		_.do_allow_tap && (_.px_end_x = _.px_start_x, _.px_end_y = _.px_start_y, _.ms_timestop = n, _.ms_elapsed = n - _.ms_timestart, u.uheld && (t = $.Event("uheld"), $.extend(t, _), $(_.elem_bound).trigger(t)), u.uheldstart ? (t = $.Event("uheldstart"), $.extend(t, _), $(_.elem_bound).trigger(t), a = o) : delete g[o])
	},
	s = function (e) {
	    var t, n, a, i, d = e.motion_id,
		r = e.event_src,
		l = e.request_dzoom,
		s = $.data(e.elem, w),
		p = s.bound_ns_map,
		c = $(r.target),
		h = !1;
	    if (!g[d] && (!l || p.uzoomstart) && !c.is(s.ignore_class)) {
	        for (a = !!(p.utap || p.uheld || p.uheldstart), n = E.pop() ; n;) c.is(n.selector_str) || $(e.elem).is(n.selector_str) ? n.callback_match && n.callback_match(e) : n.callback_nomatch && n.callback_nomatch(e),
			n = E.pop();
	        return t = {
	            do_allow_tap: a,
	            elem_bound: e.elem,
	            elem_target: r.target,
	            ms_elapsed: 0,
	            ms_timestart: r.timeStamp,
	            ms_timestop: void 0,
	            option_map: s,
	            orig_target: r.target,
	            px_current_x: r.clientX,
	            px_current_y: r.clientY,
	            px_end_x: void 0,
	            px_end_y: void 0,
	            px_start_x: r.clientX,
	            px_start_y: r.clientY,
	            timeStamp: r.timeStamp
	        },
			g[d] = t,
			p.uzoomstart && (l ? o = d : _ ? u || (u = d, i = $.Event("uzoomstart"), h = !0) : _ = d, h) ? (i = $.Event("uzoomstart"), t.px_delta_zoom = 0, $.extend(i, t), void $(t.elem_bound).trigger(i)) : void ((p.uheld || p.uheldstart) && (t.idto_tapheld = setTimeout(function () {
			    m({
			        motion_id: d,
			        motion_map: t,
			        bound_ns_map: p
			    })
			},
			s.held_tap_time)))
	    }
	},
	p = function (e) {
	    var t, i, d, r, l, m, s, p, c = e.motion_id,
		h = e.event_src,
		v = !1;
	    if (g[c]) {
	        if (t = g[c], i = t.option_map, d = i.bound_ns_map, t.timeStamp = h.timeStamp, t.elem_target = h.target, t.ms_elapsed = h.timeStamp - t.ms_timestart, t.px_delta_x = h.clientX - t.px_current_x, t.px_delta_y = h.clientY - t.px_current_y, t.px_current_x = h.clientX, t.px_current_y = h.clientY, t.timeStamp = h.timeStamp, t.do_allow_tap && (Math.abs(t.px_delta_x) > i.px_radius || Math.abs(t.pd_delta_y) > i.px_radius || t.ms_elapsed > i.tap_time) && (t.do_allow_tap = !1), _ && u && (c === _ || c === u) ? (g[c] = t, s = g[_], p = g[u], l = Math.floor(Math.sqrt(Math.pow(s.px_current_x - p.px_current_x, 2) + Math.pow(s.px_current_y - p.px_current_y, 2)) + .5), m = y === -1 ? 0 : (l - y) * D, y = l, v = !0) : o === c && d.uzoommove && (m = t.px_delta_y * k, v = !0), v) return r = $.Event("uzoommove"),
			t.px_delta_zoom = m,
			$.extend(r, t),
			void $(t.elem_bound).trigger(r);
	        a === c ? d.uheldmove && (r = $.Event("uheldmove"), $.extend(r, t), $(t.elem_bound).trigger(r), h.preventDefault()) : n === c && d.udragmove && (r = $.Event("udragmove"), $.extend(r, t), $(t.elem_bound).trigger(r), h.preventDefault()),
			n || a || !d.udragstart || t.do_allow_tap !== !1 || (n = c, r = $.Event("udragstart"), $.extend(r, t), $(t.elem_bound).trigger(r), h.preventDefault(), t.idto_tapheld && (clearTimeout(t.idto_tapheld), delete t.idto_tapheld))
	    }
	},
	c = function (e) {
	    var t, i, d, r, l = e.motion_id,
		m = e.event_src,
		s = !1;
	    z = !1,
		g[l] && (t = g[l], i = t.option_map, d = i.bound_ns_map, t.elem_target = m.target, t.ms_elapsed = m.timeStamp - t.ms_timestart, t.ms_timestop = m.timeStamp, t.px_current_x && (t.px_delta_x = m.clientX - t.px_current_x, t.px_delta_y = m.clientY - t.px_current_y), t.px_current_x = m.clientX, t.px_current_y = m.clientY, t.px_end_x = m.clientX, t.px_end_y = m.clientY, t.timeStamp = m.timeStamp, t.idto_tapheld && (clearTimeout(t.idto_tapheld), delete t.idto_tapheld), d.utap && t.ms_elapsed <= i.tap_time && t.do_allow_tap && (r = $.Event("utap"), $.extend(r, t), $(t.elem_bound).trigger(r)), l === n && (d.udragend && (r = $.Event("udragend"), $.extend(r, t), $(t.elem_bound).trigger(r), m.preventDefault()), n = void 0), l === a && (d.uheldend && (r = $.Event("uheldend"), $.extend(r, t), $(t.elem_bound).trigger(r)), a = void 0), l === o ? (s = !0, o = void 0) : l === _ && (u ? (_ = u, u = void 0, s = !0) : _ = void 0, y = -1), l === u && (u = void 0, y = -1, s = !0), s && d.uzoomend && (r = $.Event("uzoomend"), t.px_delta_zoom = 0, $.extend(r, t), $(t.elem_bound).trigger(r)), delete g[l])
	},
	v = function (e) {
	    var t, n, a, o, _ = this,
		u = +new Date,
		i = e.originalEvent,
		d = i ? i.changedTouches || [] : [],
		r = d.length;
	    switch (z = !0, e.timeStamp = u, e.type) {
	        case "touchstart":
	            o = s,
                e.preventDefault();
	            break;
	        case "touchmove":
	            o = p;
	            break;
	        case "touchend":
	        case "touchcancel":
	            o = c;
	            break;
	        default:
	            o = null
	    }
	    if (o) for (t = 0; t < r; t++) n = d[t],
		a = "touch" + String(n.identifier),
		e.clientX = n.clientX,
		e.clientY = n.clientY,
		o({
		    elem: _,
		    motion_id: a,
		    event_src: e
		})
	},
	h = function (e) {
	    var t, n = this,
		a = "mouse" + String(e.button),
		o = !1;
	    if (z) return void e.stopImmediatePropagation();
	    if (e.shiftKey && (o = !0), "mousemove" !== e.type && 0 !== e.button) return !0;
	    switch (e.type) {
	        case "mousedown":
	            t = s,
                e.preventDefault();
	            break;
	        case "mouseup":
	            t = c;
	            break;
	        case "mousemove":
	            t = p;
	            break;
	        default:
	            t = null
	    }
	    t && t({
	        elem: n,
	        event_src: e,
	        request_dzoom: o,
	        motion_id: a
	    })
	},
	b.ue = b.utap = b.uheld = b.uzoomstart = b.uzoommove = b.uzoomend = b.udragstart = b.udragmove = b.udragend = b.uheldstart = b.uheldmove = b.uheldend = t,
	$.ueSetGlobalCb = function (e, t, n) {
	    E.push({
	        selector_str: e || "",
	        callback_match: t || null,
	        callback_nomatch: n || null
	    })
	}
}(jQuery); !
function ($) {
    "use strict";
    var t = Math.floor,
	i = Math.min,
	n = Math.max;
    window.requestAnimationFrame = window.requestAnimationFrame ||
	function (t) {
	    return setTimeout(t, 10)
	},
	window.cancelAnimationFrame = window.cancelAnimationFrame ||
	function (t) {
	    return clearTimeout(t)
	};
    var o = function (t, i) {
        var n = this;
        this.el = t,
		this.$el = $(t),
		this.options = $.extend({},
		$.fn.udraggable.defaults, i),
		this.positionElement = this.options.positionElement || this.positionElement,
		this.getStartPosition = this.options.getStartPosition || this.getStartPosition,
		this.normalisePosition = this.options.normalisePosition || this.normalisePosition,
		this.updatePositionFrameHandler = function () {
		    delete n.queuedUpdate;
		    var t = n.ui.position;
		    n.positionElement(n.$el, n.started, t.left, t.top),
			n.options.dragUpdate && n.options.dragUpdate.apply(n.el, [n.ui])
		},
		this.queuePositionUpdate = function () {
		    n.queuedUpdate || (n.queuedUpdate = window.requestAnimationFrame(n.updatePositionFrameHandler))
		},
		this.init()
    };
    o.prototype = {
        constructor: o,
        init: function () {
            var t = this;
            this.disabled = !1,
			this.started = !1,
			this.normalisePosition();
            var i = this.options.handle ? this.$el.find(this.options.handle) : this.$el;
            this.options.longPress ? i.on("uheldstart.udraggable",
			function (i) {
			    t.start(i)
			}).on("uheldmove.udraggable",
			function (i) {
			    t.move(i)
			}).on("uheldend.udraggable",
			function (i) {
			    t.end(i)
			}) : i.on("udragstart.udraggable",
			function (i) {
			    t.start(i)
			}).on("udragmove.udraggable",
			function (i) {
			    t.move(i)
			}).on("udragend.udraggable",
			function (i) {
			    t.end(i)
			})
        },
        destroy: function () {
            var t = this.options.handle ? this.$el.find(this.options.handle) : this.$el;
            t.off(".udraggable"),
			this.$el.removeData("udraggable")
        },
        disable: function () {
            this.disabled = !0
        },
        enable: function () {
            this.disabled = !1
        },
        option: function () {
            var t;
            if (0 === arguments.length) return this.options;
            if (2 === arguments.length) return void (this.options[arguments[0]] = arguments[1]);
            if (1 === arguments.length) {
                if ("string" == typeof arguments[0]) return this.options[arguments[0]];
                if ("object" == typeof arguments[0]) for (t in arguments[0]) arguments[0].hasOwnProperty(t) && (this.options[t] = arguments[0][t])
            }
            this.options.containment && this._initContainment()
        },
        normalisePosition: function () { },
        start: function (t) {
            if (!this.disabled) {
                var i = this.getStartPosition(this.$el);
                return this._initContainment(),
				this.ui = {
				    helper: this.$el,
				    offset: {
				        top: i.y,
				        left: i.x
				    },
				    originalPosition: {
				        top: i.y,
				        left: i.x
				    },
				    position: {
				        top: i.y,
				        left: i.x
				    }
				},
				this.options.longPress && this._start(t),
				this._stopPropagation(t)
            }
        },
        move: function (t) {
            if (!this.disabled && (this.started || this._start(t))) {
                var i = t.px_current_x - t.px_start_x,
				n = t.px_current_y - t.px_start_y,
				o = this.options.axis;
                o && "x" === o && (n = 0),
				o && "y" === o && (i = 0);
                var e = {
                    left: this.ui.originalPosition.left,
                    top: this.ui.originalPosition.top
                };
                o && "x" !== o || (e.left += i),
				o && "y" !== o || (e.top += n),
				this._applyGrid(e),
				this._applyContainment(e);
                var s = this.ui.position;
                return e.top === s.top && e.left === s.left || (this.ui.position.left = e.left, this.ui.position.top = e.top, this.ui.offset.left = e.left, this.ui.offset.top = e.top, this.options.drag && this.options.drag.apply(this.el, [t, this.ui]), this.queuePositionUpdate()),
				this._stopPropagation(t)
            }
        },
        end: function (t) {
            return (this.started || this._start(t)) && (this.$el.removeClass("udraggable-dragging"), this.started = !1, this.queuedUpdate && window.cancelAnimationFrame(this.queuedUpdate), this.updatePositionFrameHandler(), this.options.stop && this.options.stop.apply(this.el, [t, this.ui])),
			this._stopPropagation(t)
        },
        _stopPropagation: function (t) {
            return t.stopPropagation(),
			t.preventDefault(),
			!1
        },
        _start: function (t) {
            if (this._mouseDistanceMet(t) && this._mouseDelayMet(t)) return this.started = !0,
			this.queuePositionUpdate(),
			this.options.start && this.options.start.apply(this.el, [t, this.ui]),
			this.$el.addClass("udraggable-dragging"),
			!0
        },
        _mouseDistanceMet: function (t) {
            return n(Math.abs(t.px_start_x - t.px_current_x), Math.abs(t.px_start_y - t.px_current_y)) >= this.options.distance
        },
        _mouseDelayMet: function (t) {
            return t.ms_elapsed > this.options.delay
        },
        _initContainment: function () {
            var t, i, n = this.options;
            return n.containment ? n.containment.constructor === Array ? void (this.containment = n.containment) : ("parent" === n.containment && (n.containment = this.$el.offsetParent()), t = $(n.containment), i = t[0], void (i && (this.containment = [0, 0, t.innerWidth() - this.$el.outerWidth(), t.innerHeight() - this.$el.outerHeight()]))) : void (this.containment = null)
        },
        _applyGrid: function (i) {
            if (this.options.grid) {
                var n = this.options.grid[0],
				o = this.options.grid[1];
                i.left = t((i.left + n / 2) / n) * n,
				i.top = t((i.top + o / 2) / o) * o
            }
        },
        _applyContainment: function (t) {
            var o = this.containment;
            o && (t.left = i(n(t.left, o[0]), o[2]), t.top = i(n(t.top, o[1]), o[3]))
        },
        getStartPosition: function (t) {
            return {
                x: parseInt(t.css("left"), 10) || 0,
                y: parseInt(t.css("top"), 10) || 0
            }
        },
        positionElement: function (t, i, n, o) {
            t.css({
                left: n,
                top: o
            })
        }
    },
	$.fn.udraggable = function (t) {
	    var i = Array.prototype.slice.call(arguments, 1),
		n = [];
	    return this.each(function () {
	        var e = $(this),
			s = e.data("udraggable");
	        if (s || (s = new o(this, t), e.data("udraggable", s)), "string" == typeof t) {
	            if ("function" != typeof s[t]) throw "jquery.udraggable has no '" + t + "' method";
	            var a = s[t].apply(s, i);
	            void 0 !== a && n.push(a)
	        }
	    }),
		n.length > 0 ? n[0] : this
	},
	$.fn.udraggable.defaults = {
	    axis: null,
	    delay: 0,
	    distance: 0,
	    longPress: !1,
	    drag: null,
	    start: null,
	    stop: null
	}
}(jQuery);
if ("undefined" == typeof jQuery) throw new Error("Bootstrap's JavaScript requires jQuery"); +
function ($) {
    var t = $.fn.jquery.split(" ")[0].split(".");
    if (t[0] < 2 && t[1] < 9 || 1 == t[0] && 9 == t[1] && t[2] < 1) throw new Error("Bootstrap's JavaScript requires jQuery version 1.9.1 or higher")
}(jQuery),
+
function ($) {
    "use strict";
    function t() {
        var t = document.createElement("bootstrap"),
		e = {
		    WebkitTransition: "webkitTransitionEnd",
		    MozTransition: "transitionend",
		    OTransition: "oTransitionEnd otransitionend",
		    transition: "transitionend"
		};
        for (var i in e) if (void 0 !== t.style[i]) return {
            end: e[i]
        };
        return !1
    }
    $.fn.emulateTransitionEnd = function (t) {
        var e = !1,
		i = this;
        $(this).one("bsTransitionEnd",
		function () {
		    e = !0
		});
        var o = function () {
            e || $(i).trigger($.support.transition.end)
        };
        return setTimeout(o, t),
		this
    },
	$(function () {
	    $.support.transition = t(),
		$.support.transition && ($.event.special.bsTransitionEnd = {
		    bindType: $.support.transition.end,
		    delegateType: $.support.transition.end,
		    handle: function (t) {
		        if ($(t.target).is(this)) return t.handleObj.handler.apply(this, arguments)
		    }
		})
	})
}(jQuery),
+
function ($) {
    "use strict";
    function t(t) {
        return this.each(function () {
            var e = $(this),
			o = e.data("bs.alert");
            o || e.data("bs.alert", o = new i(this)),
			"string" == typeof t && o[t].call(e)
        })
    }
    var e = '[data-dismiss="alert"]',
	i = function (t) {
	    $(t).on("click", e, this.close)
	};
    i.VERSION = "3.3.0",
	i.TRANSITION_DURATION = 150,
	i.prototype.close = function (t) {
	    function e() {
	        n.detach().trigger("closed.bs.alert").remove()
	    }
	    var o = $(this),
		s = o.attr("data-target");
	    s || (s = o.attr("href"), s = s && s.replace(/.*(?=#[^\s]*$)/, ""));
	    var n = $(s);
	    t && t.preventDefault(),
		n.length || (n = o.closest(".alert")),
		n.trigger(t = $.Event("close.bs.alert")),
		t.isDefaultPrevented() || (n.removeClass("in"), $.support.transition && n.hasClass("fade") ? n.one("bsTransitionEnd", e).emulateTransitionEnd(i.TRANSITION_DURATION) : e())
	};
    var o = $.fn.alert;
    $.fn.alert = t,
	$.fn.alert.Constructor = i,
	$.fn.alert.noConflict = function () {
	    return $.fn.alert = o,
		this
	},
	$(document).on("click.bs.alert.data-api", e, i.prototype.close)
}(jQuery),
+
function ($) {
    "use strict";
    function t(t) {
        return this.each(function () {
            var i = $(this),
			o = i.data("bs.button"),
			s = "object" == typeof t && t;
            o || i.data("bs.button", o = new e(this, s)),
			"toggle" == t ? o.toggle() : t && o.setState(t)
        })
    }
    var e = function (t, i) {
        this.$element = $(t),
		this.options = $.extend({},
		e.DEFAULTS, i),
		this.isLoading = !1
    };
    e.VERSION = "3.3.0",
	e.DEFAULTS = {
	    loadingText: "loading..."
	},
	e.prototype.setState = function (t) {
	    var e = "disabled",
		i = this.$element,
		o = i.is("input") ? "val" : "html",
		s = i.data();
	    t += "Text",
		null == s.resetText && i.data("resetText", i[o]()),
		setTimeout($.proxy(function () {
		    i[o](null == s[t] ? this.options[t] : s[t]),
			"loadingText" == t ? (this.isLoading = !0, i.addClass(e).attr(e, e)) : this.isLoading && (this.isLoading = !1, i.removeClass(e).removeAttr(e))
		},
		this), 0)
	},
	e.prototype.toggle = function () {
	    var t = !0,
		e = this.$element.closest('[data-toggle="buttons"]');
	    if (e.length) {
	        var i = this.$element.find("input");
	        "radio" == i.prop("type") && (i.prop("checked") && this.$element.hasClass("active") ? t = !1 : e.find(".active").removeClass("active")),
			t && i.prop("checked", !this.$element.hasClass("active")).trigger("change")
	    } else this.$element.attr("aria-pressed", !this.$element.hasClass("active"));
	    t && this.$element.toggleClass("active")
	};
    var i = $.fn.button;
    $.fn.button = t,
	$.fn.button.Constructor = e,
	$.fn.button.noConflict = function () {
	    return $.fn.button = i,
		this
	},
	$(document).on("click.bs.button.data-api", '[data-toggle^="button"]',
	function (e) {
	    var i = $(e.target);
	    i.hasClass("btn") || (i = i.closest(".btn")),
		t.call(i, "toggle"),
		e.preventDefault()
	}).on("focus.bs.button.data-api blur.bs.button.data-api", '[data-toggle^="button"]',
	function (t) {
	    $(t.target).closest(".btn").toggleClass("focus", "focus" == t.type)
	})
}(jQuery),
+
function ($) {
    "use strict";
    function t(t) {
        return this.each(function () {
            var i = $(this),
			o = i.data("bs.carousel"),
			s = $.extend({},
			e.DEFAULTS, i.data(), "object" == typeof t && t),
			n = "string" == typeof t ? t : s.slide;
            o || i.data("bs.carousel", o = new e(this, s)),
			"number" == typeof t ? o.to(t) : n ? o[n]() : s.interval && o.pause().cycle()
        })
    }
    var e = function (t, e) {
        this.$element = $(t),
		this.$indicators = this.$element.find(".carousel-indicators"),
		this.options = e,
		this.paused = this.sliding = this.interval = this.$active = this.$items = null,
		this.options.keyboard && this.$element.on("keydown.bs.carousel", $.proxy(this.keydown, this)),
		"hover" == this.options.pause && !("ontouchstart" in document.documentElement) && this.$element.on("mouseenter.bs.carousel", $.proxy(this.pause, this)).on("mouseleave.bs.carousel", $.proxy(this.cycle, this))
    };
    e.VERSION = "3.3.0",
	e.TRANSITION_DURATION = 600,
	e.DEFAULTS = {
	    interval: 5e3,
	    pause: "hover",
	    wrap: !0,
	    keyboard: !0
	},
	e.prototype.keydown = function (t) {
	    switch (t.which) {
	        case 37:
	            this.prev();
	            break;
	        case 39:
	            this.next();
	            break;
	        default:
	            return
	    }
	    t.preventDefault()
	},
	e.prototype.cycle = function (t) {
	    return t || (this.paused = !1),
		this.interval && clearInterval(this.interval),
		this.options.interval && !this.paused && (this.interval = setInterval($.proxy(this.next, this), this.options.interval)),
		this
	},
	e.prototype.getItemIndex = function (t) {
	    return this.$items = t.parent().children(".item"),
		this.$items.index(t || this.$active)
	},
	e.prototype.getItemForDirection = function (t, e) {
	    var i = "prev" == t ? -1 : 1,
		o = this.getItemIndex(e),
		s = (o + i) % this.$items.length;
	    return this.$items.eq(s)
	},
	e.prototype.to = function (t) {
	    var e = this,
		i = this.getItemIndex(this.$active = this.$element.find(".item.active"));
	    if (!(t > this.$items.length - 1 || t < 0)) return this.sliding ? this.$element.one("slid.bs.carousel",
		function () {
		    e.to(t)
		}) : i == t ? this.pause().cycle() : this.slide(t > i ? "next" : "prev", this.$items.eq(t))
	},
	e.prototype.pause = function (t) {
	    return t || (this.paused = !0),
		this.$element.find(".next, .prev").length && $.support.transition && (this.$element.trigger($.support.transition.end), this.cycle(!0)),
		this.interval = clearInterval(this.interval),
		this
	},
	e.prototype.next = function () {
	    if (!this.sliding) return this.slide("next")
	},
	e.prototype.prev = function () {
	    if (!this.sliding) return this.slide("prev")
	},
	e.prototype.slide = function (t, i) {
	    var o = this.$element.find(".item.active"),
		s = i || this.getItemForDirection(t, o),
		n = this.interval,
		a = "next" == t ? "left" : "right",
		r = "next" == t ? "first" : "last",
		l = this;
	    if (!s.length) {
	        if (!this.options.wrap) return;
	        s = this.$element.find(".item")[r]()
	    }
	    if (s.hasClass("active")) return this.sliding = !1;
	    var d = s[0],
		h = $.Event("slide.bs.carousel", {
		    relatedTarget: d,
		    direction: a
		});
	    if (this.$element.trigger(h), !h.isDefaultPrevented()) {
	        if (this.sliding = !0, n && this.pause(), this.$indicators.length) {
	            this.$indicators.find(".active").removeClass("active");
	            var c = $(this.$indicators.children()[this.getItemIndex(s)]);
	            c && c.addClass("active")
	        }
	        var p = $.Event("slid.bs.carousel", {
	            relatedTarget: d,
	            direction: a
	        });
	        return $.support.transition && this.$element.hasClass("slide") ? (s.addClass(t), s[0].offsetWidth, o.addClass(a), s.addClass(a), o.one("bsTransitionEnd",
			function () {
			    s.removeClass([t, a].join(" ")).addClass("active"),
				o.removeClass(["active", a].join(" ")),
				l.sliding = !1,
				setTimeout(function () {
				    l.$element.trigger(p)
				},
				0)
			}).emulateTransitionEnd(e.TRANSITION_DURATION)) : (o.removeClass("active"), s.addClass("active"), this.sliding = !1, this.$element.trigger(p)),
			n && this.cycle(),
			this
	    }
	};
    var i = $.fn.carousel;
    $.fn.carousel = t,
	$.fn.carousel.Constructor = e,
	$.fn.carousel.noConflict = function () {
	    return $.fn.carousel = i,
		this
	};
    var o = function (e) {
        var i, o = $(this),
		s = $(o.attr("data-target") || (i = o.attr("href")) && i.replace(/.*(?=#[^\s]+$)/, ""));
        if (s.hasClass("carousel")) {
            var n = $.extend({},
			s.data(), o.data()),
			a = o.attr("data-slide-to");
            a && (n.interval = !1),
			t.call(s, n),
			a && s.data("bs.carousel").to(a),
			e.preventDefault()
        }
    };
    $(document).on("click.bs.carousel.data-api", "[data-slide]", o).on("click.bs.carousel.data-api", "[data-slide-to]", o),
	$(window).on("load",
	function () {
	    $('[data-ride="carousel"]').each(function () {
	        var e = $(this);
	        t.call(e, e.data())
	    })
	})
}(jQuery),
+
function ($) {
    "use strict";
    function t(t) {
        var e, i = t.attr("data-target") || (e = t.attr("href")) && e.replace(/.*(?=#[^\s]+$)/, "");
        return $(i)
    }
    function e(t) {
        return this.each(function () {
            var e = $(this),
			o = e.data("bs.collapse"),
			s = $.extend({},
			i.DEFAULTS, e.data(), "object" == typeof t && t); !o && s.toggle && "show" == t && (s.toggle = !1),
			o || e.data("bs.collapse", o = new i(this, s)),
			"string" == typeof t && o[t]()
        })
    }
    var i = function (t, e) {
        this.$element = $(t),
		this.options = $.extend({},
		i.DEFAULTS, e),
		this.$trigger = $(this.options.trigger).filter('[href="#' + t.id + '"], [data-target="#' + t.id + '"]'),
		this.transitioning = null,
		this.options.parent ? this.$parent = this.getParent() : this.addAriaAndCollapsedClass(this.$element, this.$trigger),
		this.options.toggle && this.toggle()
    };
    i.VERSION = "3.3.0",
	i.TRANSITION_DURATION = 350,
	i.DEFAULTS = {
	    toggle: !0,
	    trigger: '[data-toggle="collapse"]'
	},
	i.prototype.dimension = function () {
	    var t = this.$element.hasClass("width");
	    return t ? "width" : "height"
	},
	i.prototype.show = function () {
	    if (!this.transitioning && !this.$element.hasClass("in")) {
	        var t, o = this.$parent && this.$parent.find("> .panel").children(".in, .collapsing");
	        if (!(o && o.length && (t = o.data("bs.collapse"), t && t.transitioning))) {
	            var s = $.Event("show.bs.collapse");
	            if (this.$element.trigger(s), !s.isDefaultPrevented()) {
	                o && o.length && (e.call(o, "hide"), t || o.data("bs.collapse", null));
	                var n = this.dimension();
	                this.$element.removeClass("collapse").addClass("collapsing")[n](0).attr("aria-expanded", !0),
					this.$trigger.removeClass("collapsed").attr("aria-expanded", !0),
					this.transitioning = 1;
	                var a = function () {
	                    this.$element.removeClass("collapsing").addClass("collapse in")[n](""),
						this.transitioning = 0,
						this.$element.trigger("shown.bs.collapse")
	                };
	                if (!$.support.transition) return a.call(this);
	                var r = $.camelCase(["scroll", n].join("-"));
	                this.$element.one("bsTransitionEnd", $.proxy(a, this)).emulateTransitionEnd(i.TRANSITION_DURATION)[n](this.$element[0][r])
	            }
	        }
	    }
	},
	i.prototype.hide = function () {
	    if (!this.transitioning && this.$element.hasClass("in")) {
	        var t = $.Event("hide.bs.collapse");
	        if (this.$element.trigger(t), !t.isDefaultPrevented()) {
	            var e = this.dimension();
	            this.$element[e](this.$element[e]())[0].offsetHeight,
				this.$element.addClass("collapsing").removeClass("collapse in").attr("aria-expanded", !1),
				this.$trigger.addClass("collapsed").attr("aria-expanded", !1),
				this.transitioning = 1;
	            var o = function () {
	                this.transitioning = 0,
					this.$element.removeClass("collapsing").addClass("collapse").trigger("hidden.bs.collapse")
	            };
	            return $.support.transition ? void this.$element[e](0).one("bsTransitionEnd", $.proxy(o, this)).emulateTransitionEnd(i.TRANSITION_DURATION) : o.call(this)
	        }
	    }
	},
	i.prototype.toggle = function () {
	    this[this.$element.hasClass("in") ? "hide" : "show"]()
	},
	i.prototype.getParent = function () {
	    return $(this.options.parent).find('[data-toggle="collapse"][data-parent="' + this.options.parent + '"]').each($.proxy(function (e, i) {
	        var o = $(i);
	        this.addAriaAndCollapsedClass(t(o), o)
	    },
		this)).end()
	},
	i.prototype.addAriaAndCollapsedClass = function (t, e) {
	    var i = t.hasClass("in");
	    t.attr("aria-expanded", i),
		e.toggleClass("collapsed", !i).attr("aria-expanded", i)
	};
    var o = $.fn.collapse;
    $.fn.collapse = e,
	$.fn.collapse.Constructor = i,
	$.fn.collapse.noConflict = function () {
	    return $.fn.collapse = o,
		this
	},
	$(document).on("click.bs.collapse.data-api", '[data-toggle="collapse"]',
	function (i) {
	    var o = $(this);
	    o.attr("data-target") || i.preventDefault();
	    var s = t(o),
		n = s.data("bs.collapse"),
		a = n ? "toggle" : $.extend({},
		o.data(), {
		    trigger: this
		});
	    e.call(s, a)
	})
}(jQuery),
+
function ($) {
    "use strict";
    function t(t) {
        t && 3 === t.which || ($(o).remove(), $(s).each(function () {
            var i = $(this),
			o = e(i),
			s = {
			    relatedTarget: this
			};
            o.hasClass("open") && (o.trigger(t = $.Event("hide.bs.dropdown", s)), t.isDefaultPrevented() || (i.attr("aria-expanded", "false"), o.removeClass("open").trigger("hidden.bs.dropdown", s)))
        }))
    }
    function e(t) {
        var e = t.attr("data-target");
        e || (e = t.attr("href"), e = e && /#[A-Za-z]/.test(e) && e.replace(/.*(?=#[^\s]*$)/, ""));
        var i = e && $(e);
        return i && i.length ? i : t.parent()
    }
    function i(t) {
        return this.each(function () {
            var e = $(this),
			i = e.data("bs.dropdown");
            i || e.data("bs.dropdown", i = new n(this)),
			"string" == typeof t && i[t].call(e)
        })
    }
    var o = ".dropdown-backdrop",
	s = '[data-toggle="dropdown"]',
	n = function (t) {
	    $(t).on("click.bs.dropdown", this.toggle)
	};
    n.VERSION = "3.3.0",
	n.prototype.toggle = function (i) {
	    var o = $(this);
	    if (!o.is(".disabled, :disabled")) {
	        var s = e(o),
			n = s.hasClass("open");
	        if (t(), !n) {
	            "ontouchstart" in document.documentElement && !s.closest(".navbar-nav").length && $('<div class="dropdown-backdrop"/>').insertAfter($(this)).on("click", t);
	            var a = {
	                relatedTarget: this
	            };
	            if (s.trigger(i = $.Event("show.bs.dropdown", a)), i.isDefaultPrevented()) return;
	            o.trigger("focus").attr("aria-expanded", "true"),
				s.toggleClass("open").trigger("shown.bs.dropdown", a)
	        }
	        return !1
	    }
	},
	n.prototype.keydown = function (t) {
	    if (/(38|40|27|32)/.test(t.which)) {
	        var i = $(this);
	        if (t.preventDefault(), t.stopPropagation(), !i.is(".disabled, :disabled")) {
	            var o = e(i),
				n = o.hasClass("open");
	            if (!n && 27 != t.which || n && 27 == t.which) return 27 == t.which && o.find(s).trigger("focus"),
				i.trigger("click");
	            var a = " li:not(.divider):visible a",
				r = o.find('[role="menu"]' + a + ', [role="listbox"]' + a);
	            if (r.length) {
	                var l = r.index(t.target);
	                38 == t.which && l > 0 && l--,
					40 == t.which && l < r.length - 1 && l++,
					~l || (l = 0),
					r.eq(l).trigger("focus")
	            }
	        }
	    }
	};
    var a = $.fn.dropdown;
    $.fn.dropdown = i,
	$.fn.dropdown.Constructor = n,
	$.fn.dropdown.noConflict = function () {
	    return $.fn.dropdown = a,
		this
	},
	$(document).on("click.bs.dropdown.data-api", t).on("click.bs.dropdown.data-api", ".dropdown form",
	function (t) {
	    t.stopPropagation()
	}).on("click.bs.dropdown.data-api", s, n.prototype.toggle).on("keydown.bs.dropdown.data-api", s, n.prototype.keydown).on("keydown.bs.dropdown.data-api", '[role="menu"]', n.prototype.keydown).on("keydown.bs.dropdown.data-api", '[role="listbox"]', n.prototype.keydown)
}(jQuery),
+
function ($) {
    "use strict";
    function t(t, i) {
        return this.each(function () {
            var o = $(this),
			s = o.data("bs.modal"),
			n = $.extend({},
			e.DEFAULTS, o.data(), "object" == typeof t && t);
            s || o.data("bs.modal", s = new e(this, n)),
			"string" == typeof t ? s[t](i) : n.show && s.show(i)
        })
    }
    var e = function (t, e) {
        this.options = e,
		this.$body = $(document.body),
		this.$element = $(t),
		this.$backdrop = this.isShown = null,
		this.scrollbarWidth = 0,
		this.options.remote && this.$element.find(".modal-content").load(this.options.remote, $.proxy(function () {
		    this.$element.trigger("loaded.bs.modal")
		},
		this))
    };
    e.VERSION = "3.3.0",
	e.TRANSITION_DURATION = 300,
	e.BACKDROP_TRANSITION_DURATION = 150,
	e.DEFAULTS = {
	    backdrop: !0,
	    keyboard: !0,
	    show: !0,
	    showBodyScroll: !0
	},
	e.prototype.toggle = function (t) {
	    return this.isShown ? this.hide() : this.show(t)
	},
	e.prototype.show = function (t) {
	    var i = this,
		o = $.Event("show.bs.modal", {
		    relatedTarget: t
		});
	    this.$element.trigger(o),
		this.isShown || o.isDefaultPrevented() || (this.isShown = !0, this.checkScrollbar(), this.setScrollbar(), this.escape(), this.$element.on("click.dismiss.bs.modal", '[data-dismiss="modal"]', $.proxy(this.hide, this)), this.backdrop(function () {
		    var o = $.support.transition && i.$element.hasClass("fade");
		    i.$element.parent().length || i.$element.appendTo(i.$body),
			i.$element.show().scrollTop(0),
			o && i.$element[0].offsetWidth,
			i.$element.addClass("in").attr("aria-hidden", !1),
			i.enforceFocus();
		    var s = $.Event("shown.bs.modal", {
		        relatedTarget: t
		    });
		    o ? i.$element.find(".modal-dialog").one("bsTransitionEnd",
			function () {
			    i.$element.trigger("focus").trigger(s)
			}).emulateTransitionEnd(e.TRANSITION_DURATION) : i.$element.trigger("focus").trigger(s)
		}), i.options.showBodyScroll || $(document.body).css({
		    "overflow-x": "hidden",
		    "overflow-y": "hidden"
		}), i.$element.udraggable({
		    handle: ".modal-header",
		    cursor: "move",
		    refreshPositions: !1,
		    start: function () {
		        i.$element.find(".modal-header").css("cursor", "crosshair")
		    },
		    stop: function () {
		        i.$element.find(".modal-header").css("cursor", "default")
		    }
		}))
	},
	e.prototype.hide = function (t) {
	    t && t.preventDefault(),
		t = $.Event("hide.bs.modal"),
		this.$element.trigger(t),
		this.isShown && !t.isDefaultPrevented() && (this.isShown = !1, this.escape(), $(document).off("focusin.bs.modal"), this.$element.removeClass("in").attr("aria-hidden", !0).off("click.dismiss.bs.modal"), $.support.transition && this.$element.hasClass("fade") ? this.$element.one("bsTransitionEnd", $.proxy(this.hideModal, this)).emulateTransitionEnd(e.TRANSITION_DURATION) : this.hideModal())
	},
	e.prototype.enforceFocus = function () {
	    $(document).off("focusin.bs.modal").on("focusin.bs.modal", $.proxy(function (t) {
	        this.$element[0] === t.target || this.$element.has(t.target).length || this.$element.trigger("focus")
	    },
		this))
	},
	e.prototype.escape = function () {
	    this.isShown && this.options.keyboard ? this.$element.on("keydown.dismiss.bs.modal", $.proxy(function (t) {
	        27 == t.which && this.hide()
	    },
		this)) : this.isShown || this.$element.off("keydown.dismiss.bs.modal")
	},
	e.prototype.hideModal = function () {
	    var t = this;
	    this.$element.hide(),
		this.backdrop(function () {
		    t.resetScrollbar(),
			t.$element.trigger("hidden.bs.modal")
		}),
		t.options.showBodyScroll || $(document.body).css({
		    "overflow-x": "auto",
		    "overflow-y": "auto"
		})
	},
	e.prototype.removeBackdrop = function () {
	    this.$backdrop && this.$backdrop.remove(),
		this.$backdrop = null
	},
	e.prototype.backdrop = function (t) {
	    var i = this,
		o = this.$element.hasClass("fade") ? "fade" : "";
	    if (this.isShown && this.options.backdrop) {
	        var s = $.support.transition && o;
	        if (this.$backdrop = $('<div class="modal-backdrop ' + o + '" />').prependTo(this.$element).on("click.dismiss.bs.modal", $.proxy(function (t) {
				t.target === t.currentTarget && ("static" == this.options.backdrop ? this.$element[0].focus.call(this.$element[0]) : this.hide.call(this))
	        },
			this)), s && this.$backdrop[0].offsetWidth, this.$backdrop.addClass("in"), !t) return;
	        s ? this.$backdrop.one("bsTransitionEnd", t).emulateTransitionEnd(e.BACKDROP_TRANSITION_DURATION) : t()
	    } else if (!this.isShown && this.$backdrop) {
	        this.$backdrop.removeClass("in");
	        var n = function () {
	            i.removeBackdrop(),
				t && t()
	        };
	        $.support.transition && this.$element.hasClass("fade") ? this.$backdrop.one("bsTransitionEnd", n).emulateTransitionEnd(e.BACKDROP_TRANSITION_DURATION) : n()
	    } else t && t()
	},
	e.prototype.checkScrollbar = function () {
	    this.scrollbarWidth = this.measureScrollbar()
	},
	e.prototype.setScrollbar = function () { },
	e.prototype.resetScrollbar = function () { },
	e.prototype.measureScrollbar = function () {
	    if (document.body.clientWidth >= window.innerWidth) return 0;
	    var t = document.createElement("div");
	    t.className = "modal-scrollbar-measure",
		this.$body.append(t);
	    var e = t.offsetWidth - t.clientWidth;
	    return this.$body[0].removeChild(t),
		e
	};
    var i = $.fn.modal;
    $.fn.modal = t,
	$.fn.modal.Constructor = e,
	$.fn.modal.noConflict = function () {
	    return $.fn.modal = i,
		this
	},
	$(document).on("click.bs.modal.data-api", '[data-toggle="modal"]',
	function (e) {
	    var i = $(this),
		o = i.attr("href"),
		s = $(i.attr("data-target") || o && o.replace(/.*(?=#[^\s]+$)/, "")),
		n = s.data("bs.modal") ? "toggle" : $.extend({
		    remote: !/#/.test(o) && o
		},
		s.data(), i.data());
	    i.is("a") && e.preventDefault(),
		s.one("show.bs.modal",
		function (t) {
		    t.isDefaultPrevented() || s.one("hidden.bs.modal",
			function () {
			    i.is(":visible") && i.trigger("focus")
			})
		}),
		t.call(s, n, this)
	})
}(jQuery),
+
function ($) {
    "use strict";
    function t(t) {
        return this.each(function () {
            var i = $(this),
			o = i.data("bs.tooltip"),
			s = "object" == typeof t && t,
			n = s && s.selector; (o || "destroy" != t) && (n ? (o || i.data("bs.tooltip", o = {}), o[n] || (o[n] = new e(this, s))) : o || i.data("bs.tooltip", o = new e(this, s)), "string" == typeof t && o[t]())
        })
    }
    var e = function (t, e) {
        this.type = this.options = this.enabled = this.timeout = this.hoverState = this.$element = null,
		this.init("tooltip", t, e)
    };
    e.VERSION = "3.3.0",
	e.TRANSITION_DURATION = 150,
	e.DEFAULTS = {
	    animation: !0,
	    placement: "top",
	    selector: !1,
	    template: '<div class="tooltip" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
	    trigger: "hover focus",
	    title: "",
	    delay: 0,
	    html: !1,
	    container: !1,
	    viewport: {
	        selector: "body",
	        padding: 0
	    }
	},
	e.prototype.init = function (t, e, i) {
	    this.enabled = !0,
		this.type = t,
		this.$element = $(e),
		this.options = this.getOptions(i),
		this.$viewport = this.options.viewport && $(this.options.viewport.selector || this.options.viewport);
	    for (var o = this.options.trigger.split(" "), s = o.length; s--;) {
	        var n = o[s];
	        if ("click" == n) this.$element.on("click." + this.type, this.options.selector, $.proxy(this.toggle, this));
	        else if ("manual" != n) {
	            var a = "hover" == n ? "mouseenter" : "focusin",
				r = "hover" == n ? "mouseleave" : "focusout";
	            this.$element.on(a + "." + this.type, this.options.selector, $.proxy(this.enter, this)),
				this.$element.on(r + "." + this.type, this.options.selector, $.proxy(this.leave, this))
	        }
	    }
	    this.options.selector ? this._options = $.extend({},
		this.options, {
		    trigger: "manual",
		    selector: ""
		}) : this.fixTitle()
	},
	e.prototype.getDefaults = function () {
	    return e.DEFAULTS
	},
	e.prototype.getOptions = function (t) {
	    return t = $.extend({},
		this.getDefaults(), this.$element.data(), t),
		t.delay && "number" == typeof t.delay && (t.delay = {
		    show: t.delay,
		    hide: t.delay
		}),
		t
	},
	e.prototype.getDelegateOptions = function () {
	    var t = {},
		e = this.getDefaults();
	    return this._options && $.each(this._options,
		function (i, o) {
		    e[i] != o && (t[i] = o)
		}),
		t
	},
	e.prototype.enter = function (t) {
	    var e = t instanceof this.constructor ? t : $(t.currentTarget).data("bs." + this.type);
	    return e && e.$tip && e.$tip.is(":visible") ? void (e.hoverState = "in") : (e || (e = new this.constructor(t.currentTarget, this.getDelegateOptions()), $(t.currentTarget).data("bs." + this.type, e)), clearTimeout(e.timeout), e.hoverState = "in", e.options.delay && e.options.delay.show ? void (e.timeout = setTimeout(function () {
	        "in" == e.hoverState && e.show()
	    },
		e.options.delay.show)) : e.show())
	},
	e.prototype.leave = function (t) {
	    var e = t instanceof this.constructor ? t : $(t.currentTarget).data("bs." + this.type);
	    return e || (e = new this.constructor(t.currentTarget, this.getDelegateOptions()), $(t.currentTarget).data("bs." + this.type, e)),
		clearTimeout(e.timeout),
		e.hoverState = "out",
		e.options.delay && e.options.delay.hide ? void (e.timeout = setTimeout(function () {
		    "out" == e.hoverState && e.hide()
		},
		e.options.delay.hide)) : e.hide()
	},
	e.prototype.show = function () {
	    var t = $.Event("show.bs." + this.type);
	    if (this.hasContent() && this.enabled) {
	        this.$element.trigger(t);
	        var i = $.contains(this.$element[0].ownerDocument.documentElement, this.$element[0]);
	        if (t.isDefaultPrevented() || !i) return;
	        var o = this,
			s = this.tip(),
			n = this.getUID(this.type);
	        this.setContent(),
			s.attr("id", n),
			this.$element.attr("aria-describedby", n),
			this.options.animation && s.addClass("fade");
	        var a = "function" == typeof this.options.placement ? this.options.placement.call(this, s[0], this.$element[0]) : this.options.placement,
			r = /\s?auto?\s?/i,
			l = r.test(a);
	        l && (a = a.replace(r, "") || "top"),
			s.detach().css({
			    top: 0,
			    left: 0,
			    display: "block"
			}).addClass(a).data("bs." + this.type, this),
			this.options.container ? s.appendTo(this.options.container) : s.insertAfter(this.$element);
	        var d = this.getPosition(),
			h = s[0].offsetWidth,
			c = s[0].offsetHeight;
	        if (l) {
	            var p = a,
				f = this.options.container ? $(this.options.container) : this.$element.parent(),
				u = this.getPosition(f);
	            a = "bottom" == a && d.bottom + c > u.bottom ? "top" : "top" == a && d.top - c < u.top ? "bottom" : "right" == a && d.right + h > u.width ? "left" : "left" == a && d.left - h < u.left ? "right" : a,
				s.removeClass(p).addClass(a)
	        }
	        var g = this.getCalculatedOffset(a, d, h, c);
	        this.applyPlacement(g, a);
	        var m = function () {
	            var t = o.hoverState;
	            o.$element.trigger("shown.bs." + o.type),
				o.hoverState = null,
				"out" == t && o.leave(o)
	        };
	        $.support.transition && this.$tip.hasClass("fade") ? s.one("bsTransitionEnd", m).emulateTransitionEnd(e.TRANSITION_DURATION) : m()
	    }
	},
	e.prototype.applyPlacement = function (t, e) {
	    var i = this.tip(),
		o = i[0].offsetWidth,
		s = i[0].offsetHeight,
		n = parseInt(i.css("margin-top"), 10),
		a = parseInt(i.css("margin-left"), 10);
	    isNaN(n) && (n = 0),
		isNaN(a) && (a = 0),
		t.top = t.top + n,
		t.left = t.left + a,
		$.offset.setOffset(i[0], $.extend({
		    using: function (t) {
		        i.css({
		            top: Math.round(t.top),
		            left: Math.round(t.left)
		        })
		    }
		},
		t), 0),
		i.addClass("in");
	    var r = i[0].offsetWidth,
		l = i[0].offsetHeight;
	    "top" == e && l != s && (t.top = t.top + s - l);
	    var d = this.getViewportAdjustedDelta(e, t, r, l);
	    d.left ? t.left += d.left : t.top += d.top;
	    var h = /top|bottom/.test(e),
		c = h ? 2 * d.left - o + r : 2 * d.top - s + l,
		p = h ? "offsetWidth" : "offsetHeight";
	    i.offset(t),
		this.replaceArrow(c, i[0][p], h)
	},
	e.prototype.replaceArrow = function (t, e, i) {
	    this.arrow().css(i ? "left" : "top", 50 * (1 - t / e) + "%").css(i ? "top" : "left", "")
	},
	e.prototype.setContent = function () {
	    var t = this.tip(),
		e = this.getTitle();
	    t.find(".tooltip-inner")[this.options.html ? "html" : "text"](e),
		t.removeClass("fade in top bottom left right")
	},
	e.prototype.hide = function (t) {
	    function i() {
	        "in" != o.hoverState && s.detach(),
			o.$element.removeAttr("aria-describedby").trigger("hidden.bs." + o.type),
			t && t()
	    }
	    var o = this,
		s = this.tip(),
		n = $.Event("hide.bs." + this.type);
	    if (this.$element.trigger(n), !n.isDefaultPrevented()) return s.removeClass("in"),
		$.support.transition && this.$tip.hasClass("fade") ? s.one("bsTransitionEnd", i).emulateTransitionEnd(e.TRANSITION_DURATION) : i(),
		this.hoverState = null,
		this
	},
	e.prototype.fixTitle = function () {
	    var t = this.$element; (t.attr("title") || "string" != typeof t.attr("data-original-title")) && t.attr("data-original-title", t.attr("title") || "").attr("title", "")
	},
	e.prototype.hasContent = function () {
	    return this.getTitle()
	},
	e.prototype.getPosition = function (t) {
	    t = t || this.$element;
	    var e = t[0],
		i = "BODY" == e.tagName,
		o = e.getBoundingClientRect();
	    null == o.width && (o = $.extend({},
		o, {
		    width: o.right - o.left,
		    height: o.bottom - o.top
		}));
	    var s = i ? {
	        top: 0,
	        left: 0
	    } : t.offset(),
		n = {
		    scroll: i ? document.documentElement.scrollTop || document.body.scrollTop : t.scrollTop()
		},
		a = i ? {
		    width: $(window).width(),
		    height: $(window).height()
		} : null;
	    return $.extend({},
		o, n, a, s)
	},
	e.prototype.getCalculatedOffset = function (t, e, i, o) {
	    return "bottom" == t ? {
	        top: e.top + e.height,
	        left: e.left + e.width / 2 - i / 2
	    } : "top" == t ? {
	        top: e.top - o,
	        left: e.left + e.width / 2 - i / 2
	    } : "left" == t ? {
	        top: e.top + e.height / 2 - o / 2,
	        left: e.left - i
	    } : {
	        top: e.top + e.height / 2 - o / 2,
	        left: e.left + e.width
	    }
	},
	e.prototype.getViewportAdjustedDelta = function (t, e, i, o) {
	    var s = {
	        top: 0,
	        left: 0
	    };
	    if (!this.$viewport) return s;
	    var n = this.options.viewport && this.options.viewport.padding || 0,
		a = this.getPosition(this.$viewport);
	    if (/right|left/.test(t)) {
	        var r = e.top - n - a.scroll,
			l = e.top + n - a.scroll + o;
	        r < a.top ? s.top = a.top - r : l > a.top + a.height && (s.top = a.top + a.height - l)
	    } else {
	        var d = e.left - n,
			h = e.left + n + i;
	        d < a.left ? s.left = a.left - d : h > a.width && (s.left = a.left + a.width - h)
	    }
	    return s
	},
	e.prototype.getTitle = function () {
	    var t, e = this.$element,
		i = this.options;
	    return t = e.attr("data-original-title") || ("function" == typeof i.title ? i.title.call(e[0]) : i.title)
	},
	e.prototype.getUID = function (t) {
	    do t += ~~(1e6 * Math.random());
	    while (document.getElementById(t));
	    return t
	},
	e.prototype.tip = function () {
	    return this.$tip = this.$tip || $(this.options.template)
	},
	e.prototype.arrow = function () {
	    return this.$arrow = this.$arrow || this.tip().find(".tooltip-arrow")
	},
	e.prototype.enable = function () {
	    this.enabled = !0
	},
	e.prototype.disable = function () {
	    this.enabled = !1
	},
	e.prototype.toggleEnabled = function () {
	    this.enabled = !this.enabled
	},
	e.prototype.toggle = function (t) {
	    var e = this;
	    t && (e = $(t.currentTarget).data("bs." + this.type), e || (e = new this.constructor(t.currentTarget, this.getDelegateOptions()), $(t.currentTarget).data("bs." + this.type, e))),
		e.tip().hasClass("in") ? e.leave(e) : e.enter(e)
	},
	e.prototype.destroy = function () {
	    var t = this;
	    clearTimeout(this.timeout),
		this.hide(function () {
		    t.$element.off("." + t.type).removeData("bs." + t.type)
		})
	};
    var i = $.fn.tooltip;
    $.fn.tooltip = t,
	$.fn.tooltip.Constructor = e,
	$.fn.tooltip.noConflict = function () {
	    return $.fn.tooltip = i,
		this
	}
}(jQuery),
+
function ($) {
    "use strict";
    function t(t) {
        return this.each(function () {
            var i = $(this),
			o = i.data("bs.popover"),
			s = "object" == typeof t && t,
			n = s && s.selector; (o || "destroy" != t) && (n ? (o || i.data("bs.popover", o = {}), o[n] || (o[n] = new e(this, s))) : o || i.data("bs.popover", o = new e(this, s)), "string" == typeof t && o[t]())
        })
    }
    var e = function (t, e) {
        this.init("popover", t, e)
    };
    if (!$.fn.tooltip) throw new Error("Popover requires tooltip.js");
    e.VERSION = "3.3.0",
	e.DEFAULTS = $.extend({},
	$.fn.tooltip.Constructor.DEFAULTS, {
	    placement: "right",
	    trigger: "click",
	    content: "",
	    template: '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'
	}),
	e.prototype = $.extend({},
	$.fn.tooltip.Constructor.prototype),
	e.prototype.constructor = e,
	e.prototype.getDefaults = function () {
	    return e.DEFAULTS
	},
	e.prototype.setContent = function () {
	    var t = this.tip(),
		e = this.getTitle(),
		i = this.getContent();
	    t.find(".popover-title")[this.options.html ? "html" : "text"](e),
		t.find(".popover-content").children().detach().end()[this.options.html ? "string" == typeof i ? "html" : "append" : "text"](i),
		t.removeClass("fade top bottom left right in"),
		t.find(".popover-title").html() || t.find(".popover-title").hide()
	},
	e.prototype.hasContent = function () {
	    return this.getTitle() || this.getContent()
	},
	e.prototype.getContent = function () {
	    var t = this.$element,
		e = this.options;
	    return t.attr("data-content") || ("function" == typeof e.content ? e.content.call(t[0]) : e.content)
	},
	e.prototype.arrow = function () {
	    return this.$arrow = this.$arrow || this.tip().find(".arrow")
	},
	e.prototype.tip = function () {
	    return this.$tip || (this.$tip = $(this.options.template)),
		this.$tip
	};
    var i = $.fn.popover;
    $.fn.popover = t,
	$.fn.popover.Constructor = e,
	$.fn.popover.noConflict = function () {
	    return $.fn.popover = i,
		this
	}
}(jQuery),
+
function ($) {
    "use strict";
    function t(e, i) {
        var o = $.proxy(this.process, this);
        this.$body = $("body"),
		this.$scrollElement = $($(e).is("body") ? window : e),
		this.options = $.extend({},
		t.DEFAULTS, i),
		this.selector = (this.options.target || "") + " .nav li > a",
		this.offsets = [],
		this.targets = [],
		this.activeTarget = null,
		this.scrollHeight = 0,
		this.$scrollElement.on("scroll.bs.scrollspy", o),
		this.refresh(),
		this.process()
    }
    function e(e) {
        return this.each(function () {
            var i = $(this),
			o = i.data("bs.scrollspy"),
			s = "object" == typeof e && e;
            o || i.data("bs.scrollspy", o = new t(this, s)),
			"string" == typeof e && o[e]()
        })
    }
    t.VERSION = "3.3.0",
	t.DEFAULTS = {
	    offset: 10
	},
	t.prototype.getScrollHeight = function () {
	    return this.$scrollElement[0].scrollHeight || Math.max(this.$body[0].scrollHeight, document.documentElement.scrollHeight)
	},
	t.prototype.refresh = function () {
	    var t = "offset",
		e = 0;
	    $.isWindow(this.$scrollElement[0]) || (t = "position", e = this.$scrollElement.scrollTop()),
		this.offsets = [],
		this.targets = [],
		this.scrollHeight = this.getScrollHeight();
	    var i = this;
	    this.$body.find(this.selector).map(function () {
	        var i = $(this),
			o = i.data("target") || i.attr("href"),
			s = /^#./.test(o) && $(o);
	        return s && s.length && s.is(":visible") && [[s[t]().top + e, o]] || null
	    }).sort(function (t, e) {
	        return t[0] - e[0]
	    }).each(function () {
	        i.offsets.push(this[0]),
			i.targets.push(this[1])
	    })
	},
	t.prototype.process = function () {
	    var t, e = this.$scrollElement.scrollTop() + this.options.offset,
		i = this.getScrollHeight(),
		o = this.options.offset + i - this.$scrollElement.height(),
		s = this.offsets,
		n = this.targets,
		a = this.activeTarget;
	    if (this.scrollHeight != i && this.refresh(), e >= o) return a != (t = n[n.length - 1]) && this.activate(t);
	    if (a && e < s[0]) return this.activeTarget = null,
		this.clear();
	    for (t = s.length; t--;) a != n[t] && e >= s[t] && (!s[t + 1] || e <= s[t + 1]) && this.activate(n[t])
	},
	t.prototype.activate = function (t) {
	    this.activeTarget = t,
		this.clear();
	    var e = this.selector + '[data-target="' + t + '"],' + this.selector + '[href="' + t + '"]',
		i = $(e).parents("li").addClass("active");
	    i.parent(".dropdown-menu").length && (i = i.closest("li.dropdown").addClass("active")),
		i.trigger("activate.bs.scrollspy")
	},
	t.prototype.clear = function () {
	    $(this.selector).parentsUntil(this.options.target, ".active").removeClass("active")
	};
    var i = $.fn.scrollspy;
    $.fn.scrollspy = e,
	$.fn.scrollspy.Constructor = t,
	$.fn.scrollspy.noConflict = function () {
	    return $.fn.scrollspy = i,
		this
	},
	$(window).on("load.bs.scrollspy.data-api",
	function () {
	    $('[data-spy="scroll"]').each(function () {
	        var t = $(this);
	        e.call(t, t.data())
	    })
	})
}(jQuery),
+
function ($) {
    "use strict";
    function t(t) {
        return this.each(function () {
            var i = $(this),
			o = i.data("bs.tab");
            o || i.data("bs.tab", o = new e(this)),
			"string" == typeof t && o[t]()
        })
    }
    var e = function (t) {
        this.element = $(t)
    };
    e.VERSION = "3.3.0",
	e.TRANSITION_DURATION = 150,
	e.prototype.show = function () {
	    var t = this.element,
		e = t.closest("ul:not(.dropdown-menu)"),
		i = t.data("target");
	    if (i || (i = t.attr("href"), i = i && i.replace(/.*(?=#[^\s]*$)/, "")), !t.parent("li").hasClass("active")) {
	        var o = e.find(".active:last a"),
			s = $.Event("hide.bs.tab", {
			    relatedTarget: t[0]
			}),
			n = $.Event("show.bs.tab", {
			    relatedTarget: o[0]
			});
	        if (o.trigger(s), t.trigger(n), !n.isDefaultPrevented() && !s.isDefaultPrevented()) {
	            var a = $(i);
	            this.activate(t.closest("li"), e),
				this.activate(a, a.parent(),
				function () {
				    o.trigger({
				        type: "hidden.bs.tab",
				        relatedTarget: t[0]
				    }),
					t.trigger({
					    type: "shown.bs.tab",
					    relatedTarget: o[0]
					})
				})
	        }
	    }
	},
	e.prototype.activate = function (t, i, o) {
	    function s() {
	        n.removeClass("active").find("> .dropdown-menu > .active").removeClass("active").end().find('[data-toggle="tab"]').attr("aria-expanded", !1),
			t.addClass("active").find('[data-toggle="tab"]').attr("aria-expanded", !0),
			a ? (t[0].offsetWidth, t.addClass("in")) : t.removeClass("fade"),
			t.parent(".dropdown-menu") && t.closest("li.dropdown").addClass("active").end().find('[data-toggle="tab"]').attr("aria-expanded", !0),
			o && o()
	    }
	    var n = i.find("> .active"),
		a = o && $.support.transition && (n.length && n.hasClass("fade") || !!i.find("> .fade").length);
	    n.length && a ? n.one("bsTransitionEnd", s).emulateTransitionEnd(e.TRANSITION_DURATION) : s(),
		n.removeClass("in");
	};
    var i = $.fn.tab;
    $.fn.tab = t,
	$.fn.tab.Constructor = e,
	$.fn.tab.noConflict = function () {
	    return $.fn.tab = i,
		this
	};
    var o = function (e) {
        e.preventDefault(),
		t.call($(this), "show")
    };
    $(document).on("click.bs.tab.data-api", '[data-toggle="tab"]', o).on("click.bs.tab.data-api", '[data-toggle="pill"]', o)
}(jQuery),
+
function ($) {
    "use strict";
    function t(t) {
        return this.each(function () {
            var i = $(this),
			o = i.data("bs.affix"),
			s = "object" == typeof t && t;
            o || i.data("bs.affix", o = new e(this, s)),
			"string" == typeof t && o[t]()
        })
    }
    var e = function (t, i) {
        this.options = $.extend({},
		e.DEFAULTS, i),
		this.$target = $(this.options.target).on("scroll.bs.affix.data-api", $.proxy(this.checkPosition, this)).on("click.bs.affix.data-api", $.proxy(this.checkPositionWithEventLoop, this)),
		this.$element = $(t),
		this.affixed = this.unpin = this.pinnedOffset = null,
		this.checkPosition()
    };
    e.VERSION = "3.3.0",
	e.RESET = "affix affix-top affix-bottom",
	e.DEFAULTS = {
	    offset: 0,
	    target: window
	},
	e.prototype.getState = function (t, e, i, o) {
	    var s = this.$target.scrollTop(),
		n = this.$element.offset(),
		a = this.$target.height();
	    if (null != i && "top" == this.affixed) return s < i && "top";
	    if ("bottom" == this.affixed) return null != i ? !(s + this.unpin <= n.top) && "bottom" : !(s + a <= t - o) && "bottom";
	    var r = null == this.affixed,
		l = r ? s : n.top,
		d = r ? a : e;
	    return null != i && l <= i ? "top" : null != o && l + d >= t - o && "bottom"
	},
	e.prototype.getPinnedOffset = function () {
	    if (this.pinnedOffset) return this.pinnedOffset;
	    this.$element.removeClass(e.RESET).addClass("affix");
	    var t = this.$target.scrollTop(),
		i = this.$element.offset();
	    return this.pinnedOffset = i.top - t
	},
	e.prototype.checkPositionWithEventLoop = function () {
	    setTimeout($.proxy(this.checkPosition, this), 1)
	},
	e.prototype.checkPosition = function () {
	    if (this.$element.is(":visible")) {
	        var t = this.$element.height(),
			i = this.options.offset,
			o = i.top,
			s = i.bottom,
			n = $("body").height();
	        "object" != typeof i && (s = o = i),
			"function" == typeof o && (o = i.top(this.$element)),
			"function" == typeof s && (s = i.bottom(this.$element));
	        var a = this.getState(n, t, o, s);
	        if (this.affixed != a) {
	            null != this.unpin && this.$element.css("top", "");
	            var r = "affix" + (a ? "-" + a : ""),
				l = $.Event(r + ".bs.affix");
	            if (this.$element.trigger(l), l.isDefaultPrevented()) return;
	            this.affixed = a,
				this.unpin = "bottom" == a ? this.getPinnedOffset() : null,
				this.$element.removeClass(e.RESET).addClass(r).trigger(r.replace("affix", "affixed") + ".bs.affix")
	        }
	        "bottom" == a && this.$element.offset({
	            top: n - t - s
	        })
	    }
	};
    var i = $.fn.affix;
    $.fn.affix = t,
	$.fn.affix.Constructor = e,
	$.fn.affix.noConflict = function () {
	    return $.fn.affix = i,
		this
	},
	$(window).on("load",
	function () {
	    $('[data-spy="affix"]').each(function () {
	        var e = $(this),
			i = e.data();
	        i.offset = i.offset || {},
			null != i.offsetBottom && (i.offset.bottom = i.offsetBottom),
			null != i.offsetTop && (i.offset.top = i.offsetTop),
			t.call(e, i)
	    })
	})
}(jQuery),
String.prototype.format || (String.prototype.format = function () {
    var t = arguments;
    return this.replace(/{(\d+)}/g,
	function (e, i) {
	    return "undefined" != typeof t[i] ? t[i] : e
	})
}),
function (t) {
    t.fn.dialog = function (e) {
        var i = this,
		o = t(i),
		s = (t(document.body), o.closest(".dialog")),
		n = "dialog-parent",
		a = arguments[1],
		r = arguments[2],
		l = function () {
		    var e = '<div class="dialog modal fade"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><button type="button" class="close">&times;</button><h4 class="modal-title"></h4></div><div class="modal-body"></div><div class="modal-footer"></div></div></div></div>';
		    s = t(e),
			t(document.body).append(s),
			s.find(".modal-body").append(o)
		},
		d = function (o) {
		    var n = (o || e || {}).buttons || {},
			a = s.find(".modal-footer");
		    a.html("");
		    for (var r in n) {
		        var l = n[r],
				d = "",
				h = "",
				c = "btn-default",
				p = "";
		        l.constructor == Object && (d = l.id, h = l.text, c = l.class || l.classed || c, p = l.click),
				l.constructor == Function && (h = r, p = l),
				$button = t('<button type="button" class="btn {1}">{0}</button>'.format(h, c)),
				d && $button.attr("id", d),
				p &&
				function (t) {
				    $button.click(function () {
				        t.call(i)
				    })
				}(p),
				a.append($button)
		    }
		    a.data("buttons", n)
		},
		h = function () {
		    s.modal("show")
		},
		c = function (t) {
		    s.modal("hide").on("hidden.bs.modal",
			function () {
			    t && (o.data(n).append(o), s.remove())
			})
		};
        if (e.constructor == Object && (!o.data(n) && o.data(n, o.parent()), s.size() < 1 && l(), d(), t(".modal-title", s).html(e.title || ""), t(".modal-dialog", s).addClass(e.dialogClass || ""), t(".modal-header .close", s).click(function () {
			var t = e.onClose || c;
			t.call(i)
        }), (e.class || e.classed) && s.addClass(e.class || e.classed), e.autoOpen !== !1 && h()), "destroy" == e && c(!0), "close" == e && c(), "open" == e && h(), "option" == e && "buttons" == a) {
            if (!r) return s.find(".modal-footer").data("buttons");
            d({
                buttons: r
            }),
			h()
        }
        return i
    }
}(jQuery),
$.messager = function () {
    var t, e, i = function (t, i, o) {
        var s = $.messager.model;
        arguments.length < 2 && (i = t || "", t = "&nbsp;"),
		$("<div>" + i + "</div>").dialog({
		    title: t,
		    onClose: function () {
		        $(this).dialog("destroy")
		    },
		    buttons: [{
		        text: s.ok.text,
		        classed: s.ok.classed || "btn-success",
		        click: function () {
		            $(this).dialog("destroy")
		        }
		    }]
		}),
		o && (clearTimeout(e), e = setTimeout(function () {
		    $(".dialog").css("display", "none")
		},
		o))
    },
	o = function (t, e, i) {
	    var o = $.messager.model;
	    $("<h4 class='text-center'>" + e + "</h4>").dialog({
	        title: t,
	        onClose: function () {
	            $(this).dialog("destroy")
	        },
	        buttons: [{
	            text: o.ok.text,
	            classed: o.ok.classed || "btn-success",
	            click: function () {
	                $(this).dialog("destroy"),
					i && i()
	            }
	        },
			{
			    text: o.cancel.text,
			    classed: o.cancel.classed || "btn-danger",
			    click: function () {
			        $(this).dialog("destroy")
			    }
			}]
	    })
	},
	s = '<div class="dialog modal fade msg-popup"><div class="modal-dialog modal-sm"><div class="modal-content"><div class="modal-body text-center"></div></div></div></div>',
	n = function (i) {
	    arrDefaultData = {
	        content: "",
	        time: 1500,
	        modal: 1,
	        close: 0,
	        css: "s"
	    };
	    var o = $.extend({},
		arrDefaultData, i);
	    t || (t = $(s), $("body").append(t)),
		o.css && ("s" == o.css ? t.removeClass("msg-popup-i") : t.removeClass("msg-popup-s"), t.addClass("msg-popup-" + o.css)),
		1 == i.close ? t.modal("hide") : (t.find(".modal-body").html(o.content), t.modal({
		    show: !0,
		    backdrop: "1" == o.modal || 0
		}), o.time && (clearTimeout(e), e = setTimeout(function () {
		    t.modal("hide")
		},
		o.time)))
	};
    return {
        alert: i,
        popup: n,
        confirm: o
    }
}(),
$.messager.model = {
    ok: {
        text: "确定",
        classed: "btn-info w-xs"
    },
    cancel: {
        text: "取消",
        classed: "btn-default w-xs"
    }
}; !
function (e) {
    function n() {
        var e = arguments[0],
		r = n.cache;
        return r[e] && r.hasOwnProperty(e) || (r[e] = n.parse(e)),
		n.format.call(null, r[e], arguments)
    }
    function r(e) {
        return Object.prototype.toString.call(e).slice(8, -1).toLowerCase()
    }
    function t(e, n) {
        return Array(n + 1).join(e)
    }
    var i = {
        not_string: /[^s]/,
        number: /[dief]/,
        text: /^[^\x25]+/,
        modulo: /^\x25{2}/,
        placeholder: /^\x25(?:([1-9]\d*)\$|\(([^\)]+)\))?(\+)?(0|'[^$])?(-)?(\d+)?(?:\.(\d+))?([b-fiosuxX])/,
        key: /^([a-z_][a-z_\d]*)/i,
        key_access: /^\.([a-z_][a-z_\d]*)/i,
        index_access: /^\[(\d+)\]/,
        sign: /^[\+\-]/
    };
    n.format = function (e, o) {
        var s, a, l, f, c, p, u, d = 1,
		g = e.length,
		h = "",
		x = [],
		b = !0,
		y = "";
        for (a = 0; g > a; a++) if (h = r(e[a]), "string" === h) x[x.length] = e[a];
        else if ("array" === h) {
            if (f = e[a], f[2]) for (s = o[d], l = 0; l < f[2].length; l++) {
                if (!s.hasOwnProperty(f[2][l])) throw new Error(n("[sprintf] property '%s' does not exist", f[2][l]));
                s = s[f[2][l]]
            } else s = f[1] ? o[f[1]] : o[d++];
            if ("function" == r(s) && (s = s()), i.not_string.test(f[8]) && "number" != r(s) && isNaN(s)) throw new TypeError(n("[sprintf] expecting number but found %s", r(s)));
            switch (i.number.test(f[8]) && (b = s >= 0), f[8]) {
                case "b":
                    s = s.toString(2);
                    break;
                case "c":
                    s = String.fromCharCode(s);
                    break;
                case "d":
                case "i":
                    s = parseInt(s, 10);
                    break;
                case "e":
                    s = f[7] ? s.toExponential(f[7]) : s.toExponential();
                    break;
                case "f":
                    s = f[7] ? parseFloat(s).toFixed(f[7]) : parseFloat(s);
                    break;
                case "o":
                    s = s.toString(8);
                    break;
                case "s":
                    s = (s = String(s)) && f[7] ? s.substring(0, f[7]) : s;
                    break;
                case "u":
                    s >>>= 0;
                    break;
                case "x":
                    s = s.toString(16);
                    break;
                case "X":
                    s = s.toString(16).toUpperCase()
            } !i.number.test(f[8]) || b && !f[3] ? y = "" : (y = b ? "+" : "-", s = s.toString().replace(i.sign, "")),
			p = f[4] ? "0" === f[4] ? "0" : f[4].charAt(1) : " ",
			u = f[6] - (y + s).length,
			c = f[6] && u > 0 ? t(p, u) : "",
			x[x.length] = f[5] ? y + s + c : "0" === p ? y + c + s : c + y + s
        }
        return x.join("")
    },
	n.cache = {},
	n.parse = function (e) {
	    for (var n = e,
		r = [], t = [], o = 0; n;) {
	        if (null !== (r = i.text.exec(n))) t[t.length] = r[0];
	        else if (null !== (r = i.modulo.exec(n))) t[t.length] = "%";
	        else {
	            if (null === (r = i.placeholder.exec(n))) throw new SyntaxError("[sprintf] unexpected placeholder");
	            if (r[2]) {
	                o |= 1;
	                var s = [],
					a = r[2],
					l = [];
	                if (null === (l = i.key.exec(a))) throw new SyntaxError("[sprintf] failed to parse named argument key");
	                for (s[s.length] = l[1];
					"" !== (a = a.substring(l[0].length)) ;) if (null !== (l = i.key_access.exec(a))) s[s.length] = l[1];
					else {
					    if (null === (l = i.index_access.exec(a))) throw new SyntaxError("[sprintf] failed to parse named argument key");
					    s[s.length] = l[1]
					}
	                r[2] = s
	            } else o |= 2;
	            if (3 === o) throw new Error("[sprintf] mixing positional and named placeholders is not (yet) supported");
	            t[t.length] = r
	        }
	        n = n.substring(r[0].length)
	    }
	    return t
	};
    var o = function (e, r, t) {
        return t = (r || []).slice(0),
		t.splice(0, 0, e),
		n.apply(null, t)
    };
    "undefined" != typeof exports ? (exports.sprintf = n, exports.vsprintf = o) : (e.sprintf = n, e.vsprintf = o, "function" == typeof define && define.amd && define(function () {
        return {
            sprintf: n,
            vsprintf: o
        }
    }))
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
    if ("Microsoft Internet Explorer" == C ? y = !0 : "Opera" == C ? w = !0 : b = !0, v = $.$dpPath || i(), $.$wdate && r(publicSettings.publicUrl + "static/js/My97DatePicker/skin/WdatePicker.css?" + publicSettings.version), m = M, $.$crossFrame) try {
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



!function ($) {
    $.Jcrop = function (e, t) {
        function n(e) {
            return "" + parseInt(e) + "px"
        }
        function r(e) {
            return "" + parseInt(e) + "%"
        }
        function s(e) {
            return j.baseClass + "-" + e
        }
        function o(e) {
            var t = $(e).offset();
            return [t.left, t.top]
        }
        function a(e) {
            return [e.pageX - se[0], e.pageY - se[1]]
        }
        function c(e) {
            e != L && (ae.setCursor(e), L = e)
        }
        function u(e, t) {
            if (se = o(K), ae.setCursor("move" == e ? e : e + "-resize"), "move" == e) return ae.activateHandlers(l(t), g);
            var n = oe.getFixed(),
			r = f(e),
			s = oe.getCorner(f(r));
            oe.setPressed(oe.getCorner(r)),
			oe.setCurrent(s),
			ae.activateHandlers(d(e, n), g)
        }
        function d(e, t) {
            return function (n) {
                if (j.aspectRatio) switch (e) {
                    case "e":
                        n[1] = t.y + 1;
                        break;
                    case "w":
                        n[1] = t.y + 1;
                        break;
                    case "n":
                        n[0] = t.x + 1;
                        break;
                    case "s":
                        n[0] = t.x + 1
                } else switch (e) {
                    case "e":
                        n[1] = t.y2;
                        break;
                    case "w":
                        n[1] = t.y2;
                        break;
                    case "n":
                        n[0] = t.x2;
                        break;
                    case "s":
                        n[0] = t.x2
                }
                oe.setCurrent(n),
				ie.update()
            }
        }
        function l(e) {
            var t = e;
            return ce.watchKeys(),
			function (e) {
			    oe.moveOffset([e[0] - t[0], e[1] - t[1]]),
				t = e,
				ie.update()
			}
        }
        function f(e) {
            switch (e) {
                case "n":
                    return "sw";
                case "s":
                    return "nw";
                case "e":
                    return "nw";
                case "w":
                    return "ne";
                case "ne":
                    return "sw";
                case "nw":
                    return "se";
                case "se":
                    return "nw";
                case "sw":
                    return "ne"
            }
        }
        function p(e) {
            return function (t) {
                return !j.disabled && (!("move" == e && !j.allowMove) && (G = !0, u(e, a(t)), t.stopPropagation(), t.preventDefault(), !1))
            }
        }
        function b(e, t, n) {
            var r = e.width(),
			s = e.height();
            r > t && t > 0 && (r = t, s = t / e.width() * e.height()),
			s > n && n > 0 && (s = n, r = n / e.height() * e.width()),
			Y = e.width() / r,
			q = e.height() / s,
			e.width(r).height(s)
        }
        function v(e) {
            return {
                x: parseInt(e.x * Y),
                y: parseInt(e.y * q),
                x2: parseInt(e.x2 * Y),
                y2: parseInt(e.y2 * q),
                w: parseInt(e.w * Y),
                h: parseInt(e.h * q)
            }
        }
        function g(e) {
            var t = oe.getFixed();
            t.w > j.minSelect[0] && t.h > j.minSelect[1] ? (ie.enableHandles(), ie.done()) : ie.release(),
			ae.setCursor(j.allowSelect ? "crosshair" : "default")
        }
        function m(e) {
            if (j.disabled) return !1;
            if (!j.allowSelect) return !1;
            G = !0,
			se = o(K),
			ie.disableHandles(),
			c("crosshair");
            var t = a(e);
            return oe.setPressed(t),
			ae.activateHandlers(y, g),
			ce.watchKeys(),
			ie.update(),
			e.stopPropagation(),
			e.preventDefault(),
			!1
        }
        function y(e) {
            oe.setCurrent(e),
			ie.update()
        }
        function x() {
            var e = $("<div></div>").addClass(s("tracker"));
            return $.browser.msie && e.css({
                opacity: 0,
                backgroundColor: "white"
            }),
			e
        }
        function C(e) {
            function t() {
                window.setTimeout(g, u)
            }
            var n = e[0] / Y,
			r = e[1] / q,
			s = e[2] / Y,
			o = e[3] / q;
            if (!N) {
                var i = oe.flipCoords(n, r, s, o),
				a = oe.getFixed(),
				c = initcr = [a.x, a.y, a.x2, a.y2],
				u = j.animationDelay,
				d = c[0],
				l = c[1],
				s = c[2],
				o = c[3],
				f = i[0] - initcr[0],
				h = i[1] - initcr[1],
				p = i[2] - initcr[2],
				w = i[3] - initcr[3],
				b = 0,
				v = j.swingSpeed;
                ie.animMode(!0);
                var g = function () {
                    return function () {
                        b += (100 - b) / v,
						c[0] = d + b / 100 * f,
						c[1] = l + b / 100 * h,
						c[2] = s + b / 100 * p,
						c[3] = o + b / 100 * w,
						b < 100 ? t() : ie.done(),
						b >= 99.8 && (b = 100),
						k(c)
                    }
                }();
                t()
            }
        }
        function S(e) {
            k([e[0] / Y, e[1] / q, e[2] / Y, e[3] / q])
        }
        function k(e) {
            oe.setPressed([e[0], e[1]]),
			oe.setCurrent([e[2], e[3]]),
			ie.update()
        }
        function z(e) {
            "object" != typeof e && (e = {}),
			j = $.extend(j, e),
			"function" != typeof j.onChange && (j.onChange = function () { }),
			"function" != typeof j.onSelect && (j.onSelect = function () { })
        }
        function M() {
            return v(oe.getFixed())
        }
        function I() {
            return oe.getFixed()
        }
        function H(e) {
            z(e),
			J()
        }
        function O() {
            j.disabled = !0,
			ie.disableHandles(),
			ie.setCursor("default"),
			ae.setCursor("default")
        }
        function D() {
            j.disabled = !1,
			J()
        }
        function F() {
            ie.done(),
			ae.activateHandlers(null, null)
        }
        function P() {
            E.remove(),
			B.show()
        }
        function J(e) {
            j.allowResize ? e ? ie.enableOnly() : ie.enableHandles() : ie.disableHandles(),
			ae.setCursor(j.allowSelect ? "crosshair" : "default"),
			ie.setCursor(j.allowMove ? "move" : "default"),
			E.css("backgroundColor", j.bgColor),
			"setSelect" in j && (S(t.setSelect), ie.done(), delete j.setSelect),
			"trueSize" in j && (Y = j.trueSize[0] / W, q = j.trueSize[1] / A),
			T = j.maxSize[0] || 0,
			V = j.maxSize[1] || 0,
			Q = j.minSize[0] || 0,
			X = j.minSize[1] || 0,
			"outerImage" in j && (K.attr("src", j.outerImage), delete j.outerImage),
			ie.refresh()
        }
        var e = e,
		t = t;
        "object" != typeof e && (e = $(e)[0]),
		"object" != typeof t && (t = {}),
		"trackDocument" in t || (t.trackDocument = !$.browser.msie, $.browser.msie && "8" == $.browser.version.split(".")[0] && (t.trackDocument = !0)),
		"keySupport" in t || (t.keySupport = !$.browser.msie);
        var R = {
            trackDocument: !1,
            baseClass: "jcrop",
            addClass: null,
            bgColor: "black",
            bgOpacity: .6,
            borderOpacity: .4,
            handleOpacity: .5,
            handlePad: 5,
            handleSize: 9,
            handleOffset: 5,
            edgeMargin: 14,
            aspectRatio: 0,
            keySupport: !0,
            cornerHandles: !0,
            sideHandles: !0,
            drawBorders: !0,
            dragEdges: !0,
            boxWidth: 0,
            boxHeight: 0,
            boundary: 8,
            animationDelay: 20,
            swingSpeed: 3,
            allowSelect: !0,
            allowMove: !0,
            allowResize: !0,
            minSelect: [0, 0],
            maxSize: [0, 0],
            minSize: [0, 0],
            onChange: function () { },
            onSelect: function () { }
        },
		j = R;
        z(t);
        var B = $(e),
		K = B.clone().removeAttr("id").css({
		    position: "absolute"
		});
        K.width(B.width()),
		K.height(B.height()),
		B.after(K).hide(),
		b(K, j.boxWidth, j.boxHeight);
        var W = K.width(),
		A = K.height(),
		E = $("<div />").width(W).height(A).addClass(s("holder")).css({
		    position: "relative",
		    backgroundColor: j.bgColor
		}).insertAfter(B).append(K);
        j.addClass && E.addClass(j.addClass);
        var T, V, Q, X, Y, q, G, L, N, U, Z = $("<img />").attr("src", K.attr("src")).css("position", "absolute").width(W).height(A),
		_ = $("<div />").width(r(100)).height(r(100)).css({
		    zIndex: 310,
		    position: "absolute",
		    overflow: "hidden"
		}).append(Z),
		ee = $("<div />").width(r(100)).height(r(100)).css("zIndex", 320),
		te = $("<div />").css({
		    position: "absolute",
		    zIndex: 300
		}).insertBefore(K).append(_, ee),
		ne = j.boundary,
		re = x().width(W + 2 * ne).height(A + 2 * ne).css({
		    position: "absolute",
		    top: n(-ne),
		    left: n(-ne),
		    zIndex: 290
		}).mousedown(m),
		se = o(K),
		oe = function () {
		    function e(e) {
		        var e = i(e);
		        b = f = e[0],
				v = p = e[1]
		    }
		    function t(e) {
		        var e = i(e);
		        d = e[0] - b,
				l = e[1] - v,
				b = e[0],
				v = e[1]
		    }
		    function n() {
		        return [d, l]
		    }
		    function r(e) {
		        var t = e[0],
				n = e[1];
		        0 > f + t && (t -= t + f),
				0 > p + n && (n -= n + p),
				A < v + n && (n += A - (v + n)),
				W < b + t && (t += W - (b + t)),
				f += t,
				b += t,
				p += n,
				v += n
		    }
		    function s(e) {
		        var t = o();
		        switch (e) {
		            case "ne":
		                return [t.x2, t.y];
		            case "nw":
		                return [t.x, t.y];
		            case "se":
		                return [t.x2, t.y2];
		            case "sw":
		                return [t.x, t.y2]
		        }
		    }
		    function o() {
		        if (!j.aspectRatio) return c();
		        var e, t, n = j.aspectRatio,
				r = j.minSize[0] / Y,
				s = (j.minSize[1] / q, j.maxSize[0] / Y),
				o = j.maxSize[1] / q,
				i = b - f,
				d = v - p,
				l = Math.abs(i),
				g = Math.abs(d),
				m = l / g;
		        return 0 == s && (s = 10 * W),
				0 == o && (o = 10 * A),
				m < n ? (t = v, w = g * n, e = i < 0 ? f - w : w + f, e < 0 ? (e = 0, h = Math.abs((e - f) / n), t = d < 0 ? p - h : h + p) : e > W && (e = W, h = Math.abs((e - f) / n), t = d < 0 ? p - h : h + p)) : (e = b, h = l / n, t = d < 0 ? p - h : p + h, t < 0 ? (t = 0, w = Math.abs((t - p) * n), e = i < 0 ? f - w : w + f) : t > A && (t = A, w = Math.abs(t - p) * n, e = i < 0 ? f - w : w + f)),
				e > f ? (e - f < r ? e = f + r : e - f > s && (e = f + s), t = t > p ? p + (e - f) / n : p - (e - f) / n) : e < f && (f - e < r ? e = f - r : f - e > s && (e = f - s), t = t > p ? p + (f - e) / n : p - (f - e) / n),
				e < 0 ? (f -= e, e = 0) : e > W && (f -= e - W, e = W),
				t < 0 ? (p -= t, t = 0) : t > A && (p -= t - A, t = A),
				last = u(a(f, p, e, t))
		    }
		    function i(e) {
		        return e[0] < 0 && (e[0] = 0),
				e[1] < 0 && (e[1] = 0),
				e[0] > W && (e[0] = W),
				e[1] > A && (e[1] = A),
				[e[0], e[1]]
		    }
		    function a(e, t, n, r) {
		        var s = e,
				o = n,
				i = t,
				a = r;
		        return n < e && (s = n, o = e),
				r < t && (i = r, a = t),
				[Math.round(s), Math.round(i), Math.round(o), Math.round(a)]
		    }
		    function c() {
		        var e = b - f,
				t = v - p;
		        if (T && Math.abs(e) > T && (b = e > 0 ? f + T : f - T), V && Math.abs(t) > V && (v = t > 0 ? p + V : p - V), X && Math.abs(t) < X && (v = t > 0 ? p + X : p - X), Q && Math.abs(e) < Q && (b = e > 0 ? f + Q : f - Q), f < 0 && (b -= f, f -= f), p < 0 && (v -= p, p -= p), b < 0 && (f -= b, b -= b), v < 0 && (p -= v, v -= v), b > W) {
		            var n = b - W;
		            f -= n,
					b -= n
		        }
		        if (v > A) {
		            var n = v - A;
		            p -= n,
					v -= n
		        }
		        if (f > W) {
		            var n = f - A;
		            v -= n,
					p -= n
		        }
		        if (p > A) {
		            var n = p - A;
		            v -= n,
					p -= n
		        }
		        return u(a(f, p, b, v))
		    }
		    function u(e) {
		        return {
		            x: e[0],
		            y: e[1],
		            x2: e[2],
		            y2: e[3],
		            w: e[2] - e[0],
		            h: e[3] - e[1]
		        }
		    }
		    var d, l, f = 0,
			p = 0,
			b = 0,
			v = 0;
		    return {
		        flipCoords: a,
		        setPressed: e,
		        setCurrent: t,
		        getOffset: n,
		        moveOffset: r,
		        getCorner: s,
		        getFixed: o
		    }
		}(),
		ie = function () {
		    function e(e) {
		        var t = $("<div />").css({
		            position: "absolute",
		            opacity: j.borderOpacity
		        }).addClass(s(e));
		        return _.append(t),
				t
		    }
		    function t(e, t) {
		        var n = $("<div />").mousedown(p(e)).css({
		            cursor: e + "-resize",
		            position: "absolute",
		            zIndex: t
		        });
		        return ee.append(n),
				n
		    }
		    function o(e) {
		        return t(e, M++).css({
		            top: n(-D + 1),
		            left: n(-D + 1),
		            opacity: j.handleOpacity
		        }).addClass(s("handle"))
		    }
		    function a(e) {
		        var s = j.handleSize,
				o = D,
				i = s,
				a = s,
				c = o,
				u = o;
		        switch (e) {
		            case "n":
		            case "s":
		                a = r(100);
		                break;
		            case "e":
		            case "w":
		                i = r(100)
		        }
		        return t(e, M++).width(a).height(i).css({
		            top: n(-c + 1),
		            left: n(-u + 1)
		        })
		    }
		    function c(e) {
		        for (i in e) H[e[i]] = o(e[i])
		    }
		    function u(e) {
		        var t = Math.round(e.h / 2 - D),
				r = Math.round(e.w / 2 - D),
				s = (west = -D + 1, e.w - D),
				o = e.h - D;
		        "e" in H && H.e.css({
		            top: n(t),
		            left: n(s)
		        }) && H.w.css({
		            top: n(t)
		        }) && H.s.css({
		            top: n(o),
		            left: n(r)
		        }) && H.n.css({
		            left: n(r)
		        }),
				"ne" in H && H.ne.css({
				    left: n(s)
				}) && H.se.css({
				    top: n(o),
				    left: n(s)
				}) && H.sw.css({
				    top: n(o)
				}),
				"b" in H && H.b.css({
				    top: n(o)
				}) && H.r.css({
				    left: n(s)
				})
		    }
		    function d(e, t) {
		        Z.css({
		            top: n(-t),
		            left: n(-e)
		        }),
				te.css({
				    top: n(t),
				    left: n(e)
				})
		    }
		    function l(e, t) {
		        te.width(e).height(t)
		    }
		    function f() {
		        var e = oe.getFixed();
		        oe.setPressed([e.x, e.y]),
				oe.setCurrent([e.x2, e.y2]),
				h()
		    }
		    function h() {
		        if (z) return w()
		    }
		    function w() {
		        var e = oe.getFixed();
		        l(e.w, e.h),
				d(e.x, e.y),
				j.drawBorders && I.right.css({
				    left: n(e.w - 1)
				}) && I.bottom.css({
				    top: n(e.h - 1)
				}),
				O && u(e),
				z || b(),
				j.onChange(v(e))
		    }
		    function b() {
		        te.show(),
				K.css("opacity", j.bgOpacity),
				z = !0
		    }
		    function g() {
		        C(),
				te.hide(),
				K.css("opacity", 1),
				z = !1
		    }
		    function m() {
		        O && (u(oe.getFixed()), ee.show())
		    }
		    function y() {
		        if (O = !0, j.allowResize) return u(oe.getFixed()),
				ee.show(),
				!0
		    }
		    function C() {
		        O = !1,
				ee.hide()
		    }
		    function S(e) {
		        (N = e) ? C() : y()
		    }
		    function k() {
		        S(!1),
				f()
		    }
		    var z, M = 370,
			I = {},
			H = {},
			O = !1,
			D = j.handleOffset;
		    j.drawBorders && (I = {
		        top: e("hline").css("top", n($.browser.msie ? -1 : 0)),
		        bottom: e("hline"),
		        left: e("vline"),
		        right: e("vline")
		    }),
			j.dragEdges && (H.t = a("n"), H.b = a("s"), H.r = a("e"), H.l = a("w")),
			j.sideHandles && c(["n", "s", "e", "w"]),
			j.cornerHandles && c(["sw", "nw", "ne", "se"]);
		    var F = x().mousedown(p("move")).css({
		        cursor: "move",
		        position: "absolute",
		        zIndex: 360
		    });
		    return _.append(F),
			C(),
			{
			    updateVisible: h,
			    update: w,
			    release: g,
			    refresh: f,
			    setCursor: function (e) {
			        F.css("cursor", e)
			    },
			    enableHandles: y,
			    enableOnly: function () {
			        O = !0
			    },
			    showHandles: m,
			    disableHandles: C,
			    animMode: S,
			    done: k
			}
		}(),
		ae = function () {
		    function e() {
		        re.css({
		            zIndex: 450
		        }),
				u && $(document).mousemove(n).mouseup(r)
		    }
		    function t() {
		        re.css({
		            zIndex: 290
		        }),
				u && $(document).unbind("mousemove", n).unbind("mouseup", r)
		    }
		    function n(e) {
		        i(a(e))
		    }
		    function r(e) {
		        return e.preventDefault(),
				e.stopPropagation(),
				G && (G = !1, c(a(e)), j.onSelect(v(oe.getFixed())), t(), i = function () { },
				c = function () { }),
				!1
		    }
		    function s(t, n) {
		        return G = !0,
				i = t,
				c = n,
				e(),
				!1
		    }
		    function o(e) {
		        re.css("cursor", e)
		    }
		    var i = function () { },
			c = function () { },
			u = j.trackDocument;
		    return u || re.mousemove(n).mouseup(r).mouseout(r),
			K.before(re),
			{
			    activateHandlers: s,
			    setCursor: o
			}
		}(),
		ce = function () {
		    function e() {
		        j.keySupport && (s.show(), s.focus())
		    }
		    function t(e) {
		        s.hide()
		    }
		    function n(e, t, n) {
		        j.allowMove && (oe.moveOffset([t, n]), ie.updateVisible()),
				e.preventDefault(),
				e.stopPropagation()
		    }
		    function r(e) {
		        if (e.ctrlKey) return !0;
		        U = !!e.shiftKey;
		        var t = U ? 10 : 1;
		        switch (e.keyCode) {
		            case 37:
		                n(e, -t, 0);
		                break;
		            case 39:
		                n(e, t, 0);
		                break;
		            case 38:
		                n(e, 0, -t);
		                break;
		            case 40:
		                n(e, 0, t);
		                break;
		            case 27:
		                ie.release();
		                break;
		            case 9:
		                return !0
		        }
		        return nothing(e)
		    }
		    var s = $('<input type="radio" />').css({
		        position: "absolute",
		        left: "-30px"
		    }).keypress(r).blur(t),
			o = $("<div />").css({
			    position: "absolute",
			    overflow: "hidden"
			}).append(s);
		    return j.keySupport && o.insertBefore(K),
			{
			    watchKeys: e
			}
		}();
        ee.hide(),
		J(!0);
        var ue = {
            animateTo: C,
            setSelect: S,
            setOptions: H,
            tellSelect: M,
            tellScaled: I,
            disable: O,
            enable: D,
            cancel: F,
            focus: ce.watchKeys,
            getBounds: function () {
                return [W * Y, A * q]
            },
            getWidgetSize: function () {
                return [W, A]
            },
            release: ie.release,
            destroy: P
        };
        return B.data("Jcrop", ue),
		ue
    },
	$.fn.Jcrop = function (e) {
	    function t(t) {
	        var n = e.useImg || t.src,
			r = new Image;
	        r.onload = function () {
	            $.Jcrop(t, e)
	        },
			r.src = n
	    }
	    return "object" != typeof e && (e = {}),
		this.each(function () {
		    if ($(this).data("Jcrop")) {
		        if ("api" == e) return $(this).data("Jcrop");
		        $(this).data("Jcrop").setOptions(e)
		    } else t(this)
		}),
		this
	}
}(jQuery);
!function ($) {
    "use strict";
    function e(e) {
        var t = e.data;
        e.isDefaultPrevented() || (e.preventDefault(), $(this).ajaxSubmit(t))
    }
    function t(e) {
        var t = e.target,
		a = $(t);
        if (!a.is(":submit,input:image")) {
            var r = a.closest(":submit");
            if (0 === r.length) return;
            t = r[0]
        }
        var n = this;
        if (n.clk = t, "image" == t.type) if (void 0 !== e.offsetX) n.clk_x = e.offsetX,
		n.clk_y = e.offsetY;
        else if ("function" == typeof $.fn.offset) {
            var i = a.offset();
            n.clk_x = e.pageX - i.left,
			n.clk_y = e.pageY - i.top
        } else n.clk_x = e.pageX - t.offsetLeft,
		n.clk_y = e.pageY - t.offsetTop;
        setTimeout(function () {
            n.clk = n.clk_x = n.clk_y = null
        },
		100)
    }
    function a() {
        if ($.fn.ajaxSubmit.debug) {
            var e = "[jquery.form] " + Array.prototype.join.call(arguments, "");
            window.console && window.console.log ? window.console.log(e) : window.opera && window.opera.postError && window.opera.postError(e)
        }
    }
    var r = {};
    r.fileapi = void 0 !== $("<input type='file'/>").get(0).files,
	r.formdata = void 0 !== window.FormData,
	$.fn.ajaxSubmit = function (e) {
	    function t(e) {
	        var t, a, r = $.param(e).split("&"),
			n = r.length,
			i = {};
	        for (t = 0; t < n; t++) a = r[t].split("="),
			i[decodeURIComponent(a[0])] = decodeURIComponent(a[1]);
	        return i
	    }
	    function n(a) {
	        for (var r = new FormData,
			n = 0; n < a.length; n++) r.append(a[n].name, a[n].value);
	        if (e.extraData) {
	            var i = t(e.extraData);
	            for (var o in i) i.hasOwnProperty(o) && r.append(o, i[o])
	        }
	        e.data = null;
	        var s = $.extend(!0, {},
			$.ajaxSettings, e, {
			    contentType: !1,
			    processData: !1,
			    cache: !1,
			    type: "POST"
			});
	        e.uploadProgress && (s.xhr = function () {
	            var t = jQuery.ajaxSettings.xhr();
	            return t.upload && (t.upload.onprogress = function (t) {
	                var a = 0,
					r = t.loaded || t.position,
					n = t.total;
	                t.lengthComputable && (a = Math.ceil(r / n * 100)),
					e.uploadProgress(t, r, n, a)
	            }),
				t
	        }),
			s.data = null;
	        var u = s.beforeSend;
	        s.beforeSend = function (e, t) {
	            t.data = r,
				u && u.call(this, e, t)
	        },
			$.ajax(s)
	    }
	    function i(t) {
	        function r(e) {
	            var t = e.contentWindow ? e.contentWindow.document : e.contentDocument ? e.contentDocument : e.document;
	            return t
	        }
	        function n() {
	            function e() {
	                try {
	                    var t = r(h).readyState;
	                    a("state = " + t),
						t && "uninitialized" == t.toLowerCase() && setTimeout(e, 50)
	                } catch (e) {
	                    a("Server abort: ", e, " (", e.name, ")"),
						i(S),
						y && clearTimeout(y),
						y = void 0
	                }
	            }
	            var t = l.attr("target"),
				n = l.attr("action");
	            T.setAttribute("target", m),
				o || T.setAttribute("method", "POST"),
				n != c.url && T.setAttribute("action", c.url),
				c.skipEncodingOverride || o && !/post/i.test(o) || l.attr({
				    encoding: "multipart/form-data",
				    enctype: "multipart/form-data"
				}),
				c.timeout && (y = setTimeout(function () {
				    b = !0,
					i(j)
				},
				c.timeout));
	            var s = [];
	            try {
	                if (c.extraData) for (var u in c.extraData) c.extraData.hasOwnProperty(u) && ($.isPlainObject(c.extraData[u]) && c.extraData[u].hasOwnProperty("name") && c.extraData[u].hasOwnProperty("value") ? s.push($('<input type="hidden" name="' + c.extraData[u].name + '">').attr("value", c.extraData[u].value).appendTo(T)[0]) : s.push($('<input type="hidden" name="' + u + '">').attr("value", c.extraData[u]).appendTo(T)[0]));
	                c.iframeTarget || (p.appendTo("body"), h.attachEvent ? h.attachEvent("onload", i) : h.addEventListener("load", i, !1)),
					setTimeout(e, 15),
					T.submit()
	            } finally {
	                T.setAttribute("action", n),
					t ? T.setAttribute("target", t) : l.removeAttr("target"),
					$(s).remove()
	            }
	        }
	        function i(e) {
	            if (!v.aborted && !E) {
	                try {
	                    L = r(h)
	                } catch (t) {
	                    a("cannot access response document: ", t),
						e = S
	                }
	                if (e === j && v) return void v.abort("timeout");
	                if (e == S && v) return void v.abort("server abort");
	                if (L && L.location.href != c.iframeSrc || b) {
	                    h.detachEvent ? h.detachEvent("onload", i) : h.removeEventListener("load", i, !1);
	                    var t, n = "success";
	                    try {
	                        if (b) throw "timeout";
	                        var o = "xml" == c.dataType || L.XMLDocument || $.isXMLDoc(L);
	                        if (a("isXml=" + o), !o && window.opera && (null === L.body || !L.body.innerHTML) && --M) return a("requeing onLoad callback, DOM not available"),
							void setTimeout(i, 250);
	                        var s = L.body ? L.body : L.documentElement;
	                        v.responseText = s ? s.innerHTML : null,
							v.responseXML = L.XMLDocument ? L.XMLDocument : L,
							o && (c.dataType = "xml"),
							v.getResponseHeader = function (e) {
							    var t = {
							        "content-type": c.dataType
							    };
							    return t[e]
							},
							s && (v.status = Number(s.getAttribute("status")) || v.status, v.statusText = s.getAttribute("statusText") || v.statusText);
	                        var u = (c.dataType || "").toLowerCase(),
							l = /(json|script|text)/.test(u);
	                        if (l || c.textarea) {
	                            var m = L.getElementsByTagName("textarea")[0];
	                            if (m) v.responseText = m.value,
								v.status = Number(m.getAttribute("status")) || v.status,
								v.statusText = m.getAttribute("statusText") || v.statusText;
	                            else if (l) {
	                                var d = L.getElementsByTagName("pre")[0],
									g = L.getElementsByTagName("body")[0];
	                                d ? v.responseText = d.textContent ? d.textContent : d.innerText : g && (v.responseText = g.textContent ? g.textContent : g.innerText)
	                            }
	                        } else "xml" == u && !v.responseXML && v.responseText && (v.responseXML = F(v.responseText));
	                        try {
	                            A = X(v, u, c)
	                        } catch (e) {
	                            n = "parsererror",
								v.error = t = e || n
	                        }
	                    } catch (e) {
	                        a("error caught: ", e),
							n = "error",
							v.error = t = e || n
	                    }
	                    v.aborted && (a("upload aborted"), n = null),
						v.status && (n = v.status >= 200 && v.status < 300 || 304 === v.status ? "success" : "error"),
						"success" === n ? (c.success && c.success.call(c.context, A, "success", v), f && $.event.trigger("ajaxSuccess", [v, c])) : n && (void 0 === t && (t = v.statusText), c.error && c.error.call(c.context, v, n, t), f && $.event.trigger("ajaxError", [v, c, t])),
						f && $.event.trigger("ajaxComplete", [v, c]),
						f && !--$.active && $.event.trigger("ajaxStop"),
						c.complete && c.complete.call(c.context, v, n),
						E = !0,
						c.timeout && clearTimeout(y),
						setTimeout(function () {
						    c.iframeTarget || p.remove(),
							v.responseXML = null
						},
						100)
	                }
	            }
	        }
	        var s, u, c, f, m, p, h, v, g, x, b, y, T = l[0],
			w = !!$.fn.prop;
	        if ($(":input[name=submit],:input[id=submit]", T).length) return void alert('Error: Form elements must not have name or id of "submit".');
	        if (t) for (u = 0; u < d.length; u++) s = $(d[u]),
			w ? s.prop("disabled", !1) : s.removeAttr("disabled");
	        if (c = $.extend(!0, {},
			$.ajaxSettings, e), c.context = c.context || c, m = "jqFormIO" + (new Date).getTime(), c.iframeTarget ? (p = $(c.iframeTarget), x = p.attr("name"), x ? m = x : p.attr("name", m)) : (p = $('<iframe name="' + m + '" src="' + c.iframeSrc + '" />'), p.css({
	            position: "absolute",
	            top: "-1000px",
	            left: "-1000px"
	        })), h = p[0], v = {
	            aborted: 0,
	            responseText: null,
	            responseXML: null,
	            status: 0,
	            statusText: "n/a",
	            getAllResponseHeaders: function () { },
	            getResponseHeader: function () { },
	            setRequestHeader: function () { },
	            abort: function (e) {
					var t = "timeout" === e ? "timeout" : "aborted";
					if (a("aborting upload... " + t), this.aborted = 1, h.contentWindow.document.execCommand) try {
						h.contentWindow.document.execCommand("Stop")
	        } catch (e) { }
					p.attr("src", c.iframeSrc),
					v.error = t,
					c.error && c.error.call(c.context, v, t, e),
					f && $.event.trigger("ajaxError", [v, c, t]),
					c.complete && c.complete.call(c.context, v, t)
	        }
	        },
			f = c.global, f && 0 === $.active++ && $.event.trigger("ajaxStart"), f && $.event.trigger("ajaxSend", [v, c]), c.beforeSend && c.beforeSend.call(c.context, v, c) === !1) return void (c.global && $.active--);
	        if (!v.aborted) {
	            g = T.clk,
				g && (x = g.name, x && !g.disabled && (c.extraData = c.extraData || {},
				c.extraData[x] = g.value, "image" == g.type && (c.extraData[x + ".x"] = T.clk_x, c.extraData[x + ".y"] = T.clk_y)));
	            var j = 1,
				S = 2,
				k = $("meta[name=csrf-token]").attr("content"),
				D = $("meta[name=csrf-param]").attr("content");
	            D && k && (c.extraData = c.extraData || {},
				c.extraData[D] = k),
				c.forceSync ? n() : setTimeout(n, 10);
	            var A, L, E, M = 50,
				F = $.parseXML ||
				function (e, t) {
				    return window.ActiveXObject ? (t = new ActiveXObject("Microsoft.XMLDOM"), t.async = "false", t.loadXML(e)) : t = (new DOMParser).parseFromString(e, "text/xml"),
					t && t.documentElement && "parsererror" != t.documentElement.nodeName ? t : null
				},
				O = $.parseJSON ||
				function (e) {
				    return window.eval("(" + e + ")")
				},
				X = function (e, t, a) {
				    var r = e.getResponseHeader("content-type") || "",
					n = "xml" === t || !t && r.indexOf("xml") >= 0,
					i = n ? e.responseXML : e.responseText;
				    return n && "parsererror" === i.documentElement.nodeName && $.error && $.error("parsererror"),
					a && a.dataFilter && (i = a.dataFilter(i, t)),
					"string" == typeof i && ("json" === t || !t && r.indexOf("json") >= 0 ? i = O(i) : ("script" === t || !t && r.indexOf("javascript") >= 0) && $.globalEval(i)),
					i
				}
	        }
	    }
	    if (!this.length) return a("ajaxSubmit: skipping submit process - no element selected"),
		this;
	    var o, s, u, l = this;
	    "function" == typeof e && (e = {
	        success: e
	    }),
		o = this.attr("method"),
		s = this.attr("action"),
		u = "string" == typeof s ? $.trim(s) : "",
		u = u || window.location.href || "",
		u && (u = (u.match(/^([^#]+)/) || [])[1]),
		e = $.extend(!0, {
		    url: u,
		    success: $.ajaxSettings.success,
		    type: o || "GET",
		    iframeSrc: /^https/i.test(window.location.href || "") ? "javascript:false" : "about:blank"
		},
		e);
	    var c = {};
	    if (this.trigger("form-pre-serialize", [this, e, c]), c.veto) return a("ajaxSubmit: submit vetoed via form-pre-serialize trigger"),
		this;
	    if (e.beforeSerialize && e.beforeSerialize(this, e) === !1) return a("ajaxSubmit: submit aborted via beforeSerialize callback"),
		this;
	    var f = e.traditional;
	    void 0 === f && (f = $.ajaxSettings.traditional);
	    var m, d = [],
		p = this.formToArray(e.semantic, d);
	    if (e.data && (e.extraData = e.data, m = $.param(e.data, f)), e.beforeSubmit && e.beforeSubmit(p, this, e) === !1) return a("ajaxSubmit: submit aborted via beforeSubmit callback"),
		this;
	    if (this.trigger("form-submit-validate", [p, this, e, c]), c.veto) return a("ajaxSubmit: submit vetoed via form-submit-validate trigger"),
		this;
	    var h = $.param(p, f);
	    m && (h = h ? h + "&" + m : m),
		"GET" == e.type.toUpperCase() ? (e.url += (e.url.indexOf("?") >= 0 ? "&" : "?") + h, e.data = null) : e.data = h;
	    var v = [];
	    if (e.resetForm && v.push(function () {
			l.resetForm()
	    }), e.clearForm && v.push(function () {
			l.clearForm(e.includeHidden)
	    }), !e.dataType && e.target) {
	        var g = e.success ||
			function () { };
	        v.push(function (t) {
	            var a = e.replaceTarget ? "replaceWith" : "html";
	            $(e.target)[a](t).each(g, arguments)
	        })
	    } else e.success && v.push(e.success);
	    e.success = function (t, a, r) {
	        for (var n = e.context || this,
			i = 0,
			o = v.length; i < o; i++) v[i].apply(n, [t, a, r || l, l])
	    };
	    var x = $("input:file:enabled[value]", this),
		b = x.length > 0,
		y = "multipart/form-data",
		T = l.attr("enctype") == y || l.attr("encoding") == y,
		w = r.fileapi && r.formdata;
	    a("fileAPI :" + w);
	    var j = (b || T) && !w;
	    e.iframe !== !1 && (e.iframe || j) ? e.closeKeepAlive ? $.get(e.closeKeepAlive,
		function () {
		    i(p)
		}) : i(p) : (b || T) && w ? n(p) : $.ajax(e);
	    for (var S = 0; S < d.length; S++) d[S] = null;
	    return this.trigger("form-submit-notify", [this, e]),
		this
	},
	$.fn.ajaxForm = function (r) {
	    if (r = r || {},
		r.delegation = r.delegation && $.isFunction($.fn.on), !r.delegation && 0 === this.length) {
	        var n = {
	            s: this.selector,
	            c: this.context
	        };
	        return !$.isReady && n.s ? (a("DOM not ready, queuing ajaxForm"), $(function () {
	            $(n.s, n.c).ajaxForm(r)
	        }), this) : (a("terminating; zero elements found by selector" + ($.isReady ? "" : " (DOM not ready)")), this)
	    }
	    return r.delegation ? ($(document).off("submit.form-plugin", this.selector, e).off("click.form-plugin", this.selector, t).on("submit.form-plugin", this.selector, r, e).on("click.form-plugin", this.selector, r, t), this) : this.ajaxFormUnbind().bind("submit.form-plugin", r, e).bind("click.form-plugin", r, t)
	},
	$.fn.ajaxFormUnbind = function () {
	    return this.unbind("submit.form-plugin click.form-plugin")
	},
	$.fn.formToArray = function (e, t) {
	    var a = [];
	    if (0 === this.length) return a;
	    var n = this[0],
		i = e ? n.getElementsByTagName("*") : n.elements;
	    if (!i) return a;
	    var o, s, u, l, c, f, m;
	    for (o = 0, f = i.length; o < f; o++) if (c = i[o], u = c.name) if (e && n.clk && "image" == c.type) c.disabled || n.clk != c || (a.push({
	        name: u,
	        value: $(c).val(),
	        type: c.type
	    }), a.push({
	        name: u + ".x",
	        value: n.clk_x
	    },
		{
		    name: u + ".y",
		    value: n.clk_y
		}));
	    else if (l = $.fieldValue(c, !0), l && l.constructor == Array) for (t && t.push(c), s = 0, m = l.length; s < m; s++) a.push({
	        name: u,
	        value: l[s]
	    });
	    else if (r.fileapi && "file" == c.type && !c.disabled) {
	        t && t.push(c);
	        var d = c.files;
	        if (d.length) for (s = 0; s < d.length; s++) a.push({
	            name: u,
	            value: d[s],
	            type: c.type
	        });
	        else a.push({
	            name: u,
	            value: "",
	            type: c.type
	        })
	    } else null !== l && "undefined" != typeof l && (t && t.push(c), a.push({
	        name: u,
	        value: l,
	        type: c.type,
	        required: c.required
	    }));
	    if (!e && n.clk) {
	        var p = $(n.clk),
			h = p[0];
	        u = h.name,
			u && !h.disabled && "image" == h.type && (a.push({
			    name: u,
			    value: p.val()
			}), a.push({
			    name: u + ".x",
			    value: n.clk_x
			},
			{
			    name: u + ".y",
			    value: n.clk_y
			}))
	    }
	    return a
	},
	$.fn.formSerialize = function (e) {
	    return $.param(this.formToArray(e))
	},
	$.fn.fieldSerialize = function (e) {
	    var t = [];
	    return this.each(function () {
	        var a = this.name;
	        if (a) {
	            var r = $.fieldValue(this, e);
	            if (r && r.constructor == Array) for (var n = 0,
				i = r.length; n < i; n++) t.push({
				    name: a,
				    value: r[n]
				});
	            else null !== r && "undefined" != typeof r && t.push({
	                name: this.name,
	                value: r
	            })
	        }
	    }),
		$.param(t)
	},
	$.fn.fieldValue = function (e) {
	    for (var t = [], a = 0, r = this.length; a < r; a++) {
	        var n = this[a],
			i = $.fieldValue(n, e);
	        null === i || "undefined" == typeof i || i.constructor == Array && !i.length || (i.constructor == Array ? $.merge(t, i) : t.push(i))
	    }
	    return t
	},
	$.fieldValue = function (e, t) {
	    var a = e.name,
		r = e.type,
		n = e.tagName.toLowerCase();
	    if (void 0 === t && (t = !0), t && (!a || e.disabled || "reset" == r || "button" == r || ("checkbox" == r || "radio" == r) && !e.checked || ("submit" == r || "image" == r) && e.form && e.form.clk != e || "select" == n && e.selectedIndex == -1)) return null;
	    if ("select" == n) {
	        var i = e.selectedIndex;
	        if (i < 0) return null;
	        for (var o = [], s = e.options, u = "select-one" == r, l = u ? i + 1 : s.length, c = u ? i : 0; c < l; c++) {
	            var f = s[c];
	            if (f.selected) {
	                var m = f.value;
	                if (m || (m = f.attributes && f.attributes.value && !f.attributes.value.specified ? f.text : f.value), u) return m;
	                o.push(m)
	            }
	        }
	        return o
	    }
	    return $(e).val()
	},
	$.fn.clearForm = function (e) {
	    return this.each(function () {
	        $("input,select,textarea", this).clearFields(e)
	    })
	},
	$.fn.clearFields = $.fn.clearInputs = function (e) {
	    var t = /^(?:color|date|datetime|email|month|number|password|range|search|tel|text|time|url|week)$/i;
	    return this.each(function () {
	        var a = this.type,
			r = this.tagName.toLowerCase();
	        t.test(a) || "textarea" == r ? this.value = "" : "checkbox" == a || "radio" == a ? this.checked = !1 : "select" == r ? this.selectedIndex = -1 : e && (e === !0 && /hidden/.test(a) || "string" == typeof e && $(this).is(e)) && (this.value = "")
	    })
	},
	$.fn.resetForm = function () {
	    return this.each(function () {
	        ("function" == typeof this.reset || "object" == typeof this.reset && !this.reset.nodeType) && this.reset()
	    })
	},
	$.fn.enable = function (e) {
	    return void 0 === e && (e = !0),
		this.each(function () {
		    this.disabled = !e
		})
	},
	$.fn.selected = function (e) {
	    return void 0 === e && (e = !0),
		this.each(function () {
		    var t = this.type;
		    if ("checkbox" == t || "radio" == t) this.checked = e;
		    else if ("option" == this.tagName.toLowerCase()) {
		        var a = $(this).parent("select");
		        e && a[0] && "select-one" == a[0].type && a.find("option").selected(!1),
				this.selected = e
		    }
		})
	},
	$.fn.ajaxSubmit.debug = !1
}(jQuery);
!function ($, t, e, s) {
    function i(e, s) {
        this.w = $(t),
		this.el = $(e),
		this.options = $.extend({},
		r, s),
		this.init()
    }
    var a = "ontouchstart" in t,
	o = function () {
	    var s = e.createElement("div"),
		i = e.documentElement;
	    if (!("pointerEvents" in s.style)) return !1;
	    s.style.pointerEvents = "auto",
		s.style.pointerEvents = "x",
		i.appendChild(s);
	    var a = t.getComputedStyle && "auto" === t.getComputedStyle(s, "").pointerEvents;
	    return i.removeChild(s),
		!!a
	}(),
	n = a ? "touchstart" : "mousedown",
	l = a ? "touchmove" : "mousemove",
	d = a ? "touchend" : "mouseup";
    eCancel = a ? "touchcancel" : "mouseup";
    var r = {
        listNodeName: "ol",
        itemNodeName: "li",
        rootClass: "dd",
        listClass: "dd-list",
        itemClass: "dd-item",
        dragClass: "dd-dragel",
        handleClass: "dd-handle",
        collapsedClass: "dd-collapsed",
        placeClass: "dd-placeholder",
        noDragClass: "dd-nodrag",
        emptyClass: "dd-empty",
        expandBtnHTML: '<button data-action="expand" type="button">Expand</button>',
        collapseBtnHTML: '<button data-action="collapse" type="button">Collapse</button>',
        group: 0,
        maxDepth: 5,
        threshold: 20
    };
    i.prototype = {
        init: function () {
            var e = this;
            e.reset(),
			e.el.data("nestable-group", this.options.group),
			e.placeEl = $('<div class="' + e.options.placeClass + '"/>'),
			$.each(this.el.find(e.options.itemNodeName),
			function (t, s) {
			    e.setParent($(s))
			}),
			e.el.on("click", "button",
			function (t) {
			    if (!e.dragEl && (a || 0 === t.button)) {
			        var s = $(t.currentTarget),
					i = s.data("action"),
					o = s.parent(e.options.itemNodeName);
			        "collapse" === i && e.collapseItem(o),
					"expand" === i && e.expandItem(o)
			    }
			});
            var s = function (t) {
                var s = $(t.target);
                if (!s.hasClass(e.options.handleClass)) {
                    if (s.closest("." + e.options.noDragClass).length) return;
                    s = s.closest("." + e.options.handleClass)
                } !s.length || e.dragEl || !a && 0 !== t.button || a && 1 !== t.touches.length || (t.preventDefault(), e.dragStart(a ? t.touches[0] : t))
            },
			i = function (t) {
			    e.dragEl && (t.preventDefault(), e.dragMove(a ? t.touches[0] : t))
			},
			o = function (t) {
			    e.dragEl && (t.preventDefault(), e.dragStop(a ? t.touches[0] : t))
			};
            a ? (e.el[0].addEventListener(n, s, !1), t.addEventListener(l, i, !1), t.addEventListener(d, o, !1), t.addEventListener(eCancel, o, !1)) : (e.el.on(n, s), e.w.on(l, i), e.w.on(d, o))
        },
        serialize: function () {
            var t, e = 0,
			s = this;
            return step = function (t, e) {
                var i = [],
				a = t.children(s.options.itemNodeName);
                return a.each(function () {
                    var t = $(this),
					a = $.extend({},
					t.data()),
					o = t.children(s.options.listNodeName);
                    o.length && (a.children = step(o, e + 1)),
					i.push(a)
                }),
				i
            },
			t = step(s.el.find(s.options.listNodeName).first(), e)
        },
        serialise: function () {
            return this.serialize()
        },
        reset: function () {
            this.mouse = {
                offsetX: 0,
                offsetY: 0,
                startX: 0,
                startY: 0,
                lastX: 0,
                lastY: 0,
                nowX: 0,
                nowY: 0,
                distX: 0,
                distY: 0,
                dirAx: 0,
                dirX: 0,
                dirY: 0,
                lastDirX: 0,
                lastDirY: 0,
                distAxX: 0,
                distAxY: 0
            },
			this.moving = !1,
			this.dragEl = null,
			this.dragRootEl = null,
			this.dragDepth = 0,
			this.hasNewRoot = !1,
			this.pointEl = null
        },
        expandItem: function (t) {
            t.removeClass(this.options.collapsedClass),
			t.children('[data-action="expand"]').hide(),
			t.children('[data-action="collapse"]').show(),
			t.children(this.options.listNodeName).show()
        },
        collapseItem: function (t) {
            var e = t.children(this.options.listNodeName);
            e.length && (t.addClass(this.options.collapsedClass), t.children('[data-action="collapse"]').hide(), t.children('[data-action="expand"]').show(), t.children(this.options.listNodeName).hide())
        },
        expandAll: function (t) {
            var e = this;
            "undefined" != typeof t ? t.each(function () {
                e.expandItem($(this))
            }) : e.el.find(e.options.itemNodeName).each(function () {
                e.expandItem($(this))
            })
        },
        collapseAll: function (t) {
            var e = this;
            "undefined" != typeof t ? t.each(function () {
                e.collapseItem($(this))
            }) : e.el.find(e.options.itemNodeName).each(function () {
                e.collapseItem($(this))
            })
        },
        setParent: function (t) {
            t.children(this.options.listNodeName).length && (t.prepend($(this.options.expandBtnHTML)), t.prepend($(this.options.collapseBtnHTML))),
			t.children('[data-action="expand"]').hide()
        },
        unsetParent: function (t) {
            t.removeClass(this.options.collapsedClass),
			t.children("[data-action]").remove(),
			t.children(this.options.listNodeName).remove()
        },
        dragStart: function (t) {
            var i = this.mouse,
			a = $(t.target),
			o = a.closest(this.options.itemNodeName);
            this.placeEl.css("height", o.height()),
			i.offsetX = t.offsetX !== s ? t.offsetX : t.pageX - a.offset().left,
			i.offsetY = t.offsetY !== s ? t.offsetY : t.pageY - a.offset().top,
			i.startX = i.lastX = t.pageX,
			i.startY = i.lastY = t.pageY,
			this.dragRootEl = this.el,
			this.dragEl = $(e.createElement(this.options.listNodeName)).addClass(this.options.listClass + " " + this.options.dragClass),
			this.dragEl.css("width", o.width()),
			o.after(this.placeEl),
			o[0].parentNode.removeChild(o[0]),
			o.appendTo(this.dragEl),
			$(e.body).append(this.dragEl),
			this.dragEl.css({
			    left: t.pageX - i.offsetX,
			    top: t.pageY - i.offsetY
			});
            var n, l, d = this.dragEl.find(this.options.itemNodeName);
            for (n = 0; n < d.length; n++) l = $(d[n]).parents(this.options.listNodeName).length,
			l > this.dragDepth && (this.dragDepth = l)
        },
        dragStop: function (t) {
            var e = this.dragEl.children(this.options.itemNodeName).first();
            e[0].parentNode.removeChild(e[0]),
			this.placeEl.replaceWith(e),
			this.dragEl.remove(),
			this.el.trigger("change"),
			this.hasNewRoot && this.dragRootEl.trigger("change"),
			this.reset()
        },
        dragMove: function (s) {
            var i, a, n, l, d, r = this.options,
			h = this.mouse;
            this.dragEl.css({
                left: s.pageX - h.offsetX,
                top: s.pageY - h.offsetY
            }),
			h.lastX = h.nowX,
			h.lastY = h.nowY,
			h.nowX = s.pageX,
			h.nowY = s.pageY,
			h.distX = h.nowX - h.lastX,
			h.distY = h.nowY - h.lastY,
			h.lastDirX = h.dirX,
			h.lastDirY = h.dirY,
			h.dirX = 0 === h.distX ? 0 : h.distX > 0 ? 1 : -1,
			h.dirY = 0 === h.distY ? 0 : h.distY > 0 ? 1 : -1;
            var p = Math.abs(h.distX) > Math.abs(h.distY) ? 1 : 0;
            if (!h.moving) return h.dirAx = p,
			void (h.moving = !0);
            h.dirAx !== p ? (h.distAxX = 0, h.distAxY = 0) : (h.distAxX += Math.abs(h.distX), 0 !== h.dirX && h.dirX !== h.lastDirX && (h.distAxX = 0), h.distAxY += Math.abs(h.distY), 0 !== h.dirY && h.dirY !== h.lastDirY && (h.distAxY = 0)),
			h.dirAx = p,
			h.dirAx && h.distAxX >= r.threshold && (h.distAxX = 0, n = this.placeEl.prev(r.itemNodeName), h.distX > 0 && n.length && !n.hasClass(r.collapsedClass) && (i = n.find(r.listNodeName).last(), d = this.placeEl.parents(r.listNodeName).length, d + this.dragDepth <= r.maxDepth && (i.length ? (i = n.children(r.listNodeName).last(), i.append(this.placeEl)) : (i = $("<" + r.listNodeName + "/>").addClass(r.listClass), i.append(this.placeEl), n.append(i), this.setParent(n)))), h.distX < 0 && (l = this.placeEl.next(r.itemNodeName), l.length || (a = this.placeEl.parent(), this.placeEl.closest(r.itemNodeName).after(this.placeEl), a.children().length || this.unsetParent(a.parent()))));
            var c = !1;
            if (o || (this.dragEl[0].style.visibility = "hidden"), this.pointEl = $(e.elementFromPoint(s.pageX - e.body.scrollLeft, s.pageY - (t.pageYOffset || e.documentElement.scrollTop))), o || (this.dragEl[0].style.visibility = "visible"), this.pointEl.hasClass(r.handleClass) && (this.pointEl = this.pointEl.parent(r.itemNodeName)), this.pointEl.hasClass(r.emptyClass)) c = !0;
            else if (!this.pointEl.length || !this.pointEl.hasClass(r.itemClass)) return;
            var f = this.pointEl.closest("." + r.rootClass),
			g = this.dragRootEl.data("nestable-id") !== f.data("nestable-id");
            if (!h.dirAx || g || c) {
                if (g && r.group !== f.data("nestable-group")) return;
                if (d = this.dragDepth - 1 + this.pointEl.parents(r.listNodeName).length, d > r.maxDepth) return;
                var m = s.pageY < this.pointEl.offset().top + this.pointEl.height() / 2;
                a = this.placeEl.parent(),
				c ? (i = $(e.createElement(r.listNodeName)).addClass(r.listClass), i.append(this.placeEl), this.pointEl.replaceWith(i)) : m ? this.pointEl.before(this.placeEl) : this.pointEl.after(this.placeEl),
				a.children().length || this.unsetParent(a.parent()),
				this.dragRootEl.find(r.itemNodeName).length || this.dragRootEl.append('<div class="' + r.emptyClass + '"/>'),
				g && (this.dragRootEl = f, this.hasNewRoot = this.el[0] !== this.dragRootEl[0])
            }
        }
    },
	$.fn.nestable = function (t, e) {
	    var s = this,
		a = this;
	    return s.each(function () {
	        var s = $(this).data("nestable");
	        s ? "string" == typeof t && "function" == typeof s[t] && (a = s[t](e)) : ($(this).data("nestable", new i(this, t)), $(this).data("nestable-id", (new Date).getTime()))
	    }),
		a || s
	}
}(window.jQuery || window.Zepto, window, document);