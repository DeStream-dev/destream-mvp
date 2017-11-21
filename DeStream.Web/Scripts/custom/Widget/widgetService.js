var WidgetApp;
(function (WidgetApp) {
    var WidgetService = (function () {
        function WidgetService($http) {
            this.$http = $http;
            this._donationApiUrl = "/api/demo/widget/donation/";
        }
        WidgetService.prototype.getAll = function (userId) {
            return this.$http.get(this._donationApiUrl + "getall/" + userId);
        };
        return WidgetService;
    }());
    WidgetApp.WidgetService = WidgetService;
})(WidgetApp || (WidgetApp = {}));
//# sourceMappingURL=widgetService.js.map