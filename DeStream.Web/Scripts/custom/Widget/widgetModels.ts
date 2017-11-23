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
    }
}