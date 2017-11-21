module WidgetApp {
    export class ListResponse<T> {
        Items: T[];
        TotalCount: number;
    }

    export class UserTargetDonation {
        TargetName: string;
        DestinationTargetTotal: number;
        ActualTotal: number;
        LastDonateFrom: string;
    }
}