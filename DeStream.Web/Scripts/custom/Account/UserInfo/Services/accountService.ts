module App.Account.UserInfo.Services {


    export class AccountService {
        private apiUrl: string ="/api/demo/client/userInfo/";
        constructor(private $http: ng.IHttpService) {
        }

        getUserInfo(): ng.IHttpPromise<Models.UserInfo> {
            return this.$http.get(this.apiUrl);
        }

        saveUserProfile(userProfile: Models.UserProfile): ng.IHttpPromise<{}> {
            return this.$http.post(this.apiUrl, userProfile);
        }
    }
}