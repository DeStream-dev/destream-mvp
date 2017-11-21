var App;
(function (App) {
    var Account;
    (function (Account) {
        var UserInfo;
        (function (UserInfo) {
            var Services;
            (function (Services) {
                var AccountService = (function () {
                    function AccountService($http) {
                        this.$http = $http;
                        this.apiUrl = "/api/demo/client/userInfo/";
                    }
                    AccountService.prototype.getUserInfo = function () {
                        return this.$http.get(this.apiUrl);
                    };
                    AccountService.prototype.saveUserProfile = function (userProfile) {
                        return this.$http.post(this.apiUrl, userProfile);
                    };
                    return AccountService;
                }());
                Services.AccountService = AccountService;
            })(Services = UserInfo.Services || (UserInfo.Services = {}));
        })(UserInfo = Account.UserInfo || (Account.UserInfo = {}));
    })(Account = App.Account || (App.Account = {}));
})(App || (App = {}));
//# sourceMappingURL=accountService.js.map