var App;
(function (App) {
    var Account;
    (function (Account) {
        var Targets;
        (function (Targets) {
            var Services;
            (function (Services) {
                var TargetsService = (function () {
                    function TargetsService($http) {
                        this.$http = $http;
                        this._apiUrl = "/api/demo/client/target/";
                    }
                    TargetsService.prototype.getAll = function () {
                        return this.$http.get(this._apiUrl + "getAll");
                    };
                    TargetsService.prototype.saveAll = function (targets) {
                        return this.$http.post(this._apiUrl + "saveall", targets);
                    };
                    return TargetsService;
                }());
                Services.TargetsService = TargetsService;
            })(Services = Targets.Services || (Targets.Services = {}));
        })(Targets = Account.Targets || (Account.Targets = {}));
    })(Account = App.Account || (App.Account = {}));
})(App || (App = {}));
//# sourceMappingURL=targetsService.js.map