var WidgetApp;
(function (WidgetApp) {
    var app = angular.module("WidgetApp", [])
        .controller("WidgetController", WidgetApp.WidgetController)
        .service("widgetService", WidgetApp.WidgetService);
})(WidgetApp || (WidgetApp = {}));
//# sourceMappingURL=widgetApp.js.map