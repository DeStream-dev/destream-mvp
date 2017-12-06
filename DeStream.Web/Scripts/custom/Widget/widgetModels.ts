module WidgetApp {
    export class ListResponse<T> {
        Items: T[];
        TotalCount: number;
    }

    export class UserTargetDonation {
        TargetName: string;
        Code: string;
        DestinationTargetTotal: number;
        ActualTotal: number;
        LastDonateTotal: number;
        AvailableDonates: AvailableDonate[];
    }

    export class AvailableDonate {
        DonateTotal: number;
        Token: string;
    }

    export class WidgetAuthorizeModel {
        AuthorizationLoading: boolean;
        AuthError: string;
        AuthData: WidgetAuthData;
    }

    export class WidgetAuthData {
        Email: string;
        Password: string;
    }

    export class AddDonateModel {
        Code: string;
        DonationTotal: number;
        Token
    }
}