var WidgetApp;
(function (WidgetApp) {
    var WidgetController = (function () {
        function WidgetController($scope, widgetService, $timeout) {
            this.$scope = $scope;
            this.widgetService = widgetService;
            this.$timeout = $timeout;
            this._id = window.location.search.substr(window.location.search.indexOf('=') + 1);
            this.loadTargetDontaionInfos();
        }
        WidgetController.prototype.loadTargetDontaionInfos = function () {
            var _this = this;
            this.widgetService.getAll(this._id).then(function (res) {
                _this.$scope.targetDonationInfos = res.data;
            });
        };
        return WidgetController;
    }());
    WidgetApp.WidgetController = WidgetController;
})(WidgetApp || (WidgetApp = {}));
//# sourceMappingURL=widgetController.js.map