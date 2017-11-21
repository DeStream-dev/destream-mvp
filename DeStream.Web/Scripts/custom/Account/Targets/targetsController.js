var App;
(function (App) {
    var Account;
    (function (Account) {
        var Targets;
        (function (Targets) {
            var TargetsController = (function () {
                function TargetsController($scope, targetsService, $timeout, modalService, errorService, notificationService) {
                    var _this = this;
                    this.$scope = $scope;
                    this.targetsService = targetsService;
                    this.$timeout = $timeout;
                    this.modalService = modalService;
                    this.errorService = errorService;
                    this.notificationService = notificationService;
                    this.$scope.vm = this;
                    this.$scope.addNewTarget = function () {
                        var newTarget = new Targets.Models.UserTarget();
                        newTarget.Code = $scope.randomizeString(16);
                        newTarget.TargetRequiredTotal = 1;
                        $scope.targets.push(newTarget);
                    };
                    this.$scope.randomizeString = function (length) {
                        var s = '';
                        var randomchar = function () {
                            var n = Math.floor(Math.random() * 62);
                            if (n < 10)
                                return n; //1-10
                            if (n < 36)
                                return String.fromCharCode(n + 55); //A-Z
                            return String.fromCharCode(n + 61); //a-z
                        };
                        while (s.length < length)
                            s += randomchar();
                        return s.toLowerCase();
                    };
                    /*this.$scope.canSave = () => {
                        
                    };*/
                    this.$scope.loading = true;
                    $timeout(function () {
                        _this.loadTargets();
                    }, 500);
                }
                TargetsController.prototype.saveTargets = function () {
                    var _this = this;
                    this.$scope.loading = true;
                    this.targetsService.saveAll(this.$scope.targets).then(function (res) {
                        _this.loadTargets();
                        _this.notificationService.showSuccessNotification("Data saved.");
                    }, function (err) {
                        console.log(err);
                        _this.$scope.loading = false;
                        var errMsg = _this.errorService.getErrorMessage(err);
                        if (!errMsg)
                            errMsg = "Error happened on targets save. Try to reload page and repeat operation.";
                        _this.modalService.showErrorModal(errMsg);
                    });
                };
                TargetsController.prototype.loadTargets = function () {
                    var _this = this;
                    this.targetsService.getAll().then(function (res) {
                        _this.$scope.targets = res.data;
                        _this.$scope.loading = false;
                        _this.$scope.allowSaveEmptyTargets = false;
                    }, function (err) {
                        _this.$scope.loading = false;
                        _this.modalService.showErrorModal("Error happened when load targets. Try to reload page.");
                    });
                };
                TargetsController.prototype.removeTarget = function (target) {
                    var _this = this;
                    var targetName = "";
                    if (target.Name)
                        targetName = " '" + target.Name + "'";
                    this.modalService.showConfirm("", "Are you sure you want to delete target" + targetName + "?", function () {
                        var index = _this.$scope.targets.indexOf(target);
                        if (index > -1) {
                            var allowSaveEmptyTargets = _this.$scope.targets.length == 1 && target.Id > 0;
                            _this.$scope.targets.splice(index, 1);
                            if (allowSaveEmptyTargets)
                                _this.$scope.allowSaveEmptyTargets = allowSaveEmptyTargets;
                            _this.$scope.$apply();
                        }
                    });
                };
                return TargetsController;
            }());
            Targets.TargetsController = TargetsController;
        })(Targets = Account.Targets || (Account.Targets = {}));
    })(Account = App.Account || (App.Account = {}));
})(App || (App = {}));
//# sourceMappingURL=targetsController.js.map