module App.Account.UserInfo.Models {
    export class UserInfo {
        Email: string;
        UserName: string;
        UserProfile: UserProfile;
    }

    export class UserProfile {
        DisplayName: string;
        PurseNumber: string;
        UserInfo: string;
    }
}