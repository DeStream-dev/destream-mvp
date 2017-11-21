module WidgetApp {
    var app = angular.module("WidgetApp", [])
        .controller("WidgetController", WidgetApp.WidgetController)
        .service("widgetService", WidgetApp.WidgetService);
}