/**
 * Angular Dynamic Locale - 0.1.38
 * https://github.com/lgalfaso/angular-dynamic-locale
 * License: MIT
 */
!function(e){"function"==typeof define&&define.amd?define([],e):"object"==typeof exports?module.exports=e():e()}(function(){"use strict";return angular.module("tmh.dynamicLocale",[]).config(["$provide",function(e){function t(e){return e.$stateful=!0,e}e.decorator("dateFilter",["$delegate",t]),e.decorator("numberFilter",["$delegate",t]),e.decorator("currencyFilter",["$delegate",t])}]).constant("tmhDynamicLocale.STORAGE_KEY","tmhDynamicLocale.locale").provider("tmhDynamicLocale",["tmhDynamicLocale.STORAGE_KEY",function(e){var u,y,p,$,s="angular/i18n/angular-locale_{{locale}}.js",d="tmhDynamicLocaleStorageCache",v=e,f="get",L="put",S={},g={};function h(e,t,o,n,a,r,c){function i(n,a){$===o&&(angular.forEach(n,function(e,t){a[t]?angular.isArray(a[t])&&(n[t].length=a[t].length):delete n[t]}),angular.forEach(a,function(e,t){angular.isArray(a[t])||angular.isObject(a[t])?(n[t]||(n[t]=angular.isArray(a[t])?[]:{}),i(n[t],a[t])):n[t]=a[t]}))}if(S[o])return S[$=o];var l,u,s,d,f,g,h,m=a.defer();return o===$?m.resolve(t):(l=r.get(o))?($=o,n.$evalAsync(function(){i(t,l),p[L](v,o),n.$broadcast("$localeChangeSuccess",o,t),m.resolve(t)})):(S[$=o]=m.promise,a=e,u=function(){var e=angular.injector(["ngLocale"]).get("$locale");i(t,e),r.put(o,e),delete S[o],n.$applyAsync(function(){p[L](v,o),n.$broadcast("$localeChangeSuccess",o,t),m.resolve(t)})},s=function(){delete S[o],n.$applyAsync(function(){$===o&&($=t.id),n.$broadcast("$localeChangeError",o),m.reject(o)})},d=c,f=document.createElement("script"),g=y||document.getElementsByTagName("body")[0],h=!1,f.type="text/javascript",f.readyState?f.onreadystatechange=function(){"complete"!==f.readyState&&"loaded"!==f.readyState||(f.onreadystatechange=null,d(function(){h||(h=!0,f.parentNode===g&&g.removeChild(f),u())},30,!1))}:(f.onload=function(){h||(h=!0,f.parentNode===g&&g.removeChild(f),u())},f.onerror=function(){h||(h=!0,f.parentNode===g&&g.removeChild(f),s())}),f.src=a,f.async=!0,g.appendChild(f)),m.promise}this.localeLocationPattern=function(e){return e?(s=e,this):s},this.appendScriptTo=function(e){y=e},this.useStorage=function(e){d=e,f="get",L="put"},this.useCookieStorage=function(){angular.version.minor<7?this.useStorage("$cookieStore"):(this.useStorage("$cookies"),f="getObject",L="putObject")},this.defaultLocale=function(e){u=e},this.storageKey=function(e){return e?(v=e,this):v},this.addLocalePatternValue=function(e,t){g[e]=t},this.$get=["$rootScope","$injector","$interpolate","$locale","$q","tmhDynamicLocaleCache","$timeout",function(n,e,t,a,o,r,c){var i=t(s);return p=e.get(d),n.$evalAsync(function(){var e;(e=p[f](v)||u)&&l(e)}),{set:l,get:function(){return $}};function l(e){var t={locale:e,angularVersion:angular.version.full};return h(i(angular.extend({},g,t)),a,e,n,o,r,c)}}]}]).provider("tmhDynamicLocaleCache",function(){this.$get=["$cacheFactory",function(e){return e("tmh.dynamicLocales")}]}).provider("tmhDynamicLocaleStorageCache",function(){this.$get=["$cacheFactory",function(e){return e("tmh.dynamicLocales.store")}]}).run(["tmhDynamicLocale",angular.noop]),"tmh.dynamicLocale"});
//# sourceMappingURL=/umbraco/lib/angular-dynamic-locale/tmhDynamicLocale.min.js.map;