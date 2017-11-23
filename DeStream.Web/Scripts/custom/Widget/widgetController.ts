﻿module WidgetApp {
    export interface WidgetControllerScope {
        targetDonationInfos: UserTargetDonation[];
    }

    export class WidgetController {
        private _id: string;
        private _hub: HubProxy;
        private _indexForNotification: number;
        private readonly TargetDonationsPerScreenCount: number = 2;
        private _newDonateArrivedToTargetCode: string;

        constructor(private $scope: WidgetControllerScope, private widgetService: WidgetService,
            private $timeout: ng.ITimeoutService, private collectionService: App.Common.Services.CollectionService) {
            this._id = window.location.href.substr(window.location.href.lastIndexOf('/') + 1);
            this._hub = $.connection.hub.createHubProxy("donationhub");

            $.connection.hub.start(res => {
                this._hub.invoke("subscribe", this._id);

            });

            $.connection.hub.disconnected(() => {
                this.$timeout(() => {
                    $.connection.hub.start();
                }, 3000);
            });

            this._hub.on("donationAdded", res => {
                this._newDonateArrivedToTargetCode = res.Code;
                this.loadDonationTargets();
                var p = <any>document.getElementById('notificationSound');
                p.play();
            });
            //this.initLoadTargetsTimer(true);
            this.loadDonationTargets();
        }

        /*private _loadTargetsTimer: ng.IPromise<void>;
        private initLoadTargetsTimer(immediateUpdate: boolean = false) {
            var timeoutMs = 10000;
            if (immediateUpdate) timeoutMs = 0;
            this.$timeout.cancel(this._loadTargetsTimer);
            this._loadTargetsTimer= this.$timeout(() => {
                this.widgetService.getAll(this._id).then(res => {
                    this.$scope.targetDonationInfos = res.data;
                    this.initLoadTargetsTimer();
                });
            }, timeoutMs);
        }*/

        private _carouselTargetDonationsTimeout: ng.IPromise<void>;
        private _rowIndex: number;

        private runCarousel(runImmediately: boolean = false) {
            var timeoutMs: number = !runImmediately? 5000:0;
            this.$timeout.cancel(this._carouselTargetDonationsTimeout);
            this._carouselTargetDonationsTimeout = this.$timeout(() => {
                if (this._rows.length == this._rowIndex + 1)
                    this._rowIndex = 0;
                else
                    this._rowIndex += 1;
                this.$scope.targetDonationInfos = this._rows[this._rowIndex];
                this.showHiddenItems();

                this.runCarousel();
            }, timeoutMs);
        }

        private _rows: UserTargetDonation[][];

        private showHiddenItems(immeditely: boolean = false) {
            this.$timeout(() => {
                var timeoutFadeIn = !immeditely? 2000:0;
                
                $('.targetItem').fadeIn(timeoutFadeIn).removeClass('hidden-target');
            });
        }

        private loadDonationTargets() {
            this.widgetService.getAll(this._id).then(res => {
                this._rows = this.collectionService.toRows(res.data.Items, this.TargetDonationsPerScreenCount);
                this.$timeout.cancel(this._carouselTargetDonationsTimeout);
                var indexToNotificate = -1;
                this._rowIndex = 0;
                if (this._newDonateArrivedToTargetCode) {
                    var itemForCode = res.data.Items.filter(x => x.Code == this._newDonateArrivedToTargetCode)[0];
                    var index = res.data.Items.indexOf(itemForCode);
                    if (index > -1) {
                        var rowIndex = 0;
                        if (index + 1 > this.TargetDonationsPerScreenCount) {
                            rowIndex = Math.round(index / this.TargetDonationsPerScreenCount);
                            indexToNotificate = this._rows[rowIndex].indexOf(this._rows[rowIndex].filter(x => x.Code == this._newDonateArrivedToTargetCode)[0]);
                            this._rowIndex = rowIndex;
                        }
                        else indexToNotificate = index;
                    }
                }

                var manyScreens = this._rows.length > 1;
                this.$scope.targetDonationInfos = this._rows[this._rowIndex];
                this.showHiddenItems(!manyScreens);
                if (manyScreens)
                    this.runCarousel();

                if (indexToNotificate > -1) {
                    this.$timeout(() => {
                        var itemBlock = $('.targetItem').eq(indexToNotificate);
                        itemBlock.toggleClass("notificated").find('.donate').fadeIn(1000);
                        this.$timeout(() => {
                            itemBlock.toggleClass("notificated").find('.donate').fadeOut(3000);;
                        }, 3000);
                    });
                }
            });
        }

    }
}