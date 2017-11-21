var App;
(function (App) {
    var Account;
    (function (Account) {
        var UserInfo;
        (function (UserInfo) {
            var UserInfoController = (function () {
                function UserInfoController($scope, accountService, $timeout, modalService, errorService, notificationService) {
                    this.$scope = $scope;
                    this.accountService = accountService;
                    this.$timeout = $timeout;
                    this.modalService = modalService;
                    this.errorService = errorService;
                    this.notificationService = notificationService;
                    console.log("userInfoCntroller");
                    $scope.vm = this;
                    $scope.loading = true;
                    this.loadUserInfo();
                }
                UserInfoController.prototype.loadUserInfo = function () {
                    var _this = this;
                    this.accountService.getUserInfo().then(function (res) {
                        _this.$scope.userInfo = res.data;
                        _this.$scope.editedUserProfile = angular.copy(res.data.UserProfile);
                        _this.$scope.loading = false;
                    }, function (err) {
                        _this.$scope.loading = false;
                        var errMsg = _this.errorService.getErrorMessage(err);
                        if (!errMsg)
                            errMsg = "Error happened on data loading. Try to reload page";
                        _this.modalService.showErrorModal(errMsg);
                    });
                };
                UserInfoController.prototype.cancelChanges = function () {
                    this.$scope.editedUserProfile = angular.copy(this.$scope.userInfo.UserProfile);
                };
                UserInfoController.prototype.saveChanges = function () {
                    var _this = this;
                    this.$scope.loading = true;
                    this.accountService.saveUserProfile(this.$scope.editedUserProfile).then(function (res) {
                        _this.$scope.userInfo.UserProfile = angular.copy(_this.$scope.editedUserProfile);
                        _this.$scope.loading = false;
                        _this.notificationService.showSuccessNotification("Data saved.", "");
                    }, function (err) {
                        var errMsg = _this.errorService.getErrorMessage(err);
                        if (!errMsg)
                            errMsg = "Error happened on data saving process. Try to reload page and repeat operation.";
                        _this.modalService.showErrorModal(errMsg);
                    });
                };
                return UserInfoController;
            }());
            UserInfo.UserInfoController = UserInfoController;
        })(UserInfo = Account.UserInfo || (Account.UserInfo = {}));
    })(Account = App.Account || (App.Account = {}));
})(App || (App = {}));
//# sourceMappingURL=userInfoController.js.map