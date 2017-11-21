module App.Account.Targets {
    export interface ITargetsScope extends ng.IScope {
        loading: boolean;
        targets: Models.UserTarget[];
        addNewTarget();

        randomizeString(length: number): string;
        vm: TargetsController;
        allowSaveEmptyTargets: boolean;
    }

    export class TargetsController {
        constructor(private $scope: ITargetsScope, private targetsService: Services.TargetsService,
            private $timeout: ng.ITimeoutService, private modalService: Common.Services.ModalService,
            private errorService: Common.Services.ErrorsService,
            private notificationService: Common.Services.NotificationService) {

            this.$scope.vm = this;

            this.$scope.addNewTarget = () => {
                var newTarget = new Models.UserTarget();
                newTarget.Code = $scope.randomizeString(16);
                newTarget.TargetRequiredTotal = 1;
                $scope.targets.push(newTarget);
            };

            this.$scope.randomizeString = (length) => {
                var s = '';
                var randomchar = function () {
                    var n = Math.floor(Math.random() * 62);
                    if (n < 10) return n; //1-10
                    if (n < 36) return String.fromCharCode(n + 55); //A-Z
                    return String.fromCharCode(n + 61); //a-z
                }
                while (s.length < length) s += randomchar();
                return s.toLowerCase();
            };

            /*this.$scope.canSave = () => {
                
            };*/

            this.$scope.loading = true;
            $timeout(() => {
                this.loadTargets();
            }, 500);
        }

        saveTargets() {
            this.$scope.loading = true;
            this.targetsService.saveAll(this.$scope.targets).then(res => {
                this.loadTargets();

                this.notificationService.showSuccessNotification("Data saved.");
            }, err => {
                console.log(err);
                this.$scope.loading = false;

                var errMsg = this.errorService.getErrorMessage(err);
                if (!errMsg)
                    errMsg = "Error happened on targets save. Try to reload page and repeat operation.";
                this.modalService.showErrorModal(errMsg);
            });
        }

        loadTargets() {
            this.targetsService.getAll().then(res => {
                this.$scope.targets = res.data;

                this.$scope.loading = false;
                this.$scope.allowSaveEmptyTargets = false;
            }, err => {
                this.$scope.loading = false;
                this.modalService.showErrorModal("Error happened when load targets. Try to reload page.");

            });
        }

        removeTarget(target: Models.UserTarget) {
            var targetName = "";
            if (target.Name)
                targetName = " '" + target.Name + "'";
            this.modalService.showConfirm("", "Are you sure you want to delete target" + targetName + "?", () => {
                var index = this.$scope.targets.indexOf(target);
                if (index > -1) {
                    var allowSaveEmptyTargets = this.$scope.targets.length == 1 && target.Id > 0;
                    this.$scope.targets.splice(index, 1);
                    if (allowSaveEmptyTargets)
                        this.$scope.allowSaveEmptyTargets = allowSaveEmptyTargets;
                    this.$scope.$apply();
                }
            });
        }

    }
}