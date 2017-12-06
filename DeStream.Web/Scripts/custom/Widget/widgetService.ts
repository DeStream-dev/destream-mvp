module WidgetApp {
    export class WidgetService {
        private _donationApiUrl: string = "/api/demo/widget/donation/";

        constructor(private $http: ng.IHttpService) {

        }

        getAll(userId: string): ng.IHttpPromise<WidgetApp.ListResponse<UserTargetDonation>> {
            return this.$http.get(this._donationApiUrl + "getall/" + userId);
        }

        authorize(model: WidgetAuthData): ng.IHttpPromise<any> {
            return this.$http.post(this._donationApiUrl + "authorize", model);
        }

        donate(token: string): ng.IHttpPromise<any> {
            return this.$http.post(this._donationApiUrl, new WidgetAddDonationPostModel(token));
        }
    }

    class WidgetAddDonationPostModel {
        constructor(public Token: string) { }
    }
}