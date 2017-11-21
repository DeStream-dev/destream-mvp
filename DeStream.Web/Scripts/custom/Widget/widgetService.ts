module WidgetApp {
    export class WidgetService {
        private _donationApiUrl: string = "/api/demo/widget/donation/";

        constructor(private $http: ng.IHttpService) {

        }

        getAll(userId: string): ng.IHttpPromise<WidgetApp.ListResponse<UserTargetDonation>> {
            return this.$http.get(this._donationApiUrl + "getall/" + userId);
        }
    }
}