module App.Account.UserInfo {
    export interface IUserInfoScope {
        userInfo: Models.UserInfo;
        editedUserProfile: Models.UserProfile;
        loading: boolean;
        vm: UserInfoController;
    }

    export class UserInfoController {
        constructor(private $scope: IUserInfoScope, private accountService: Services.AccountService,
            private $timeout: ng.ITimeoutService, private modalService: Common.Services.ModalService,
            private errorService: Common.Services.ErrorsService, private notificationService: Common.Services.NotificationService) {
            console.log("userInfoCntroller");
            $scope.vm = this;
            $scope.loading = true;
            this.loadUserInfo();
            
        }

        private loadUserInfo() {
            this.accountService.getUserInfo().then(res => {
                this.$scope.userInfo = res.data;
                this.$scope.editedUserProfile = angular.copy(res.data.UserProfile);
                this.$scope.loading = false;
            }, err => {
                this.$scope.loading = false;
                var errMsg = this.errorService.getErrorMessage(err);
                if (!errMsg)
                    errMsg = "Error happened on data loading. Try to reload page";
                this.modalService.showErrorModal(errMsg);
            });
        }

        cancelChanges() {
            this.$scope.editedUserProfile = angular.copy(this.$scope.userInfo.UserProfile);
        }

        saveChanges() {
            this.$scope.loading = true;
            this.accountService.saveUserProfile(this.$scope.editedUserProfile).then(res => {
                this.$scope.userInfo.UserProfile = angular.copy(this.$scope.editedUserProfile);
                this.$scope.loading = false;
                this.notificationService.showSuccessNotification("Data saved.", "");
            }, err => {
                var errMsg = this.errorService.getErrorMessage(err);
                if (!errMsg)
                    errMsg = "Error happened on data saving process. Try to reload page and repeat operation.";
                this.modalService.showErrorModal(errMsg);
            });
        }


    }
}