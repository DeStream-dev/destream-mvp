module WidgetApp {
    var app = angular.module("WidgetApp", [])
        .controller("WidgetController", WidgetApp.WidgetController)
        .service("widgetService", WidgetApp.WidgetService)
        .service("collectionService", App.Common.Services.CollectionService);
}