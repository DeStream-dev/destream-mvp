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
        WidgetService.prototype.authorize = function (model) {
            return this.$http.post(this._donationApiUrl + "authorize", model);
        };
        WidgetService.prototype.donate = function (token) {
            return this.$http.post(this._donationApiUrl, new WidgetAddDonationPostModel(token));
        };
        return WidgetService;
    }());
    WidgetApp.WidgetService = WidgetService;
    var WidgetAddDonationPostModel = (function () {
        function WidgetAddDonationPostModel(Token) {
            this.Token = Token;
        }
        return WidgetAddDonationPostModel;
    }());
})(WidgetApp || (WidgetApp = {}));
//# sourceMappingURL=widgetService.js.map