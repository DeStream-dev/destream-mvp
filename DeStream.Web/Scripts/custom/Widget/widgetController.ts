module WidgetApp {
    export interface WidgetControllerScope {
        targetDonationInfos: ListResponse<UserTargetDonation>;
    }

    export class WidgetController {
        private _id: string;
        constructor(private $scope: WidgetControllerScope, private widgetService: WidgetService,
            private $timeout: ng.ITimeoutService) {

            this._id = window.location.search.substr(window.location.search.indexOf('=')+1);
            this.loadTargetDontaionInfos();

        }

        private loadTargetDontaionInfos() {
            this.widgetService.getAll(this._id).then(res => {
                this.$scope.targetDonationInfos = res.data;
            });
        }
    }
}