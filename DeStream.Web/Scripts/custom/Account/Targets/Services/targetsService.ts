module App.Account.Targets.Services {
    export class TargetsService {
        private _apiUrl: string="/api/demo/client/target/";

        constructor(private $http: ng.IHttpService) {

        }

        getAll(): ng.IHttpPromise<Models.UserTarget[]> {
            return this.$http.get(this._apiUrl + "getAll");
        }

        saveAll(targets: Models.UserTarget[]) {
            return this.$http.post(this._apiUrl+"saveall", targets);
        }

    }
}