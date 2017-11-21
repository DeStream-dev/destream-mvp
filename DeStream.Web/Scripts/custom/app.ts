module App {
    var app = angular.module("App", [])
        .controller("UserInfoController", App.Account.UserInfo.UserInfoController)
        .service("accountService", App.Account.UserInfo.Services.AccountService)
        .controller("TargetsController", App.Account.Targets.TargetsController)
        .service("targetsService", App.Account.Targets.Services.TargetsService)
        .service("modalService", App.Common.Services.ModalService)
        .service("errorService", App.Common.Services.ErrorsService)
        .service("notificationService", App.Common.Services.NotificationService);
}