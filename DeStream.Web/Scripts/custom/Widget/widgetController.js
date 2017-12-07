var WidgetApp;
(function (WidgetApp) {
    var WidgetController = (function () {
        function WidgetController($scope, widgetService, $timeout, collectionService) {
            var _this = this;
            this.$scope = $scope;
            this.widgetService = widgetService;
            this.$timeout = $timeout;
            this.collectionService = collectionService;
            this.TargetDonationsPerScreenCount = 3;
            this._id = window.location.href.substr(window.location.href.lastIndexOf('/') + 1).replace("#", "");
            this._hub = $.connection.hub.createHubProxy("donationhub");
            this._curUserId = $("body").data("curuserid");
            this.$scope.canSendDonation = this._id != this._curUserId;
            $scope.vm = this;
            $.connection.hub.start(function (res) {
                _this._hub.invoke("subscribe", _this._id);
            });
            $.connection.hub.disconnected(function () {
                _this.$timeout(function () {
                    $.connection.hub.start();
                }, 3000);
            });
            this._hub.on("donationAdded", function (res) {
                _this._newDonateArrivedToTargetCode = res.Code;
                _this.loadDonationTargets();
                var p = document.getElementById('notificationSound');
                p.play();
            });
            this.loadDonationTargets();
        }
        WidgetController.prototype.runCarousel = function (runImmediately) {
            var _this = this;
            if (runImmediately === void 0) { runImmediately = false; }
            var timeoutMs = !runImmediately ? 5000 : 0;
            this.$timeout.cancel(this._carouselTargetDonationsTimeout);
            this._carouselTargetDonationsTimeout = this.$timeout(function () {
                if (_this._rows.length == _this._rowIndex + 1)
                    _this._rowIndex = 0;
                else
                    _this._rowIndex += 1;
                _this.$scope.targetDonationInfos = _this._rows[_this._rowIndex];
                _this.showHiddenItems();
                _this.runCarousel();
            }, timeoutMs);
        };
        WidgetController.prototype.showHiddenItems = function (immeditely) {
            if (immeditely === void 0) { immeditely = false; }
            this.$timeout(function () {
                var timeoutFadeIn = !immeditely ? 2000 : 0;
                $('.targetItem').fadeIn(timeoutFadeIn).removeClass('hidden-target');
            });
        };
        WidgetController.prototype.addDonate = function (donation) {
            console.log(donation);
            if (!this._curUserId) {
                $('#authModal').modal('show');
                console.log('modal');
            }
            else {
                this.widgetService.donate(donation.Token).then(function (res) {
                }, function (err) {
                });
            }
        };
        WidgetController.prototype.authorize = function () {
            var _this = this;
            this.$scope.authorizationModel.AuthorizationLoading = true;
            this.widgetService.authorize(this.$scope.authorizationModel.AuthData).then(function (res) {
                _this.$scope.authorizationModel.AuthorizationLoading = false;
                $('#authModal').modal('hide');
                _this.$scope.authorizationModel = null;
                _this._curUserId = res.data;
                _this.$scope.canSendDonation = _this._curUserId != _this._id;
            }, function (err) {
                _this.$scope.authorizationModel.AuthorizationLoading = false;
                var msg = "Error happened.";
                if (err != null && err.data.ErrorMessage)
                    msg = err.data.ErrorMessage;
                _this.$scope.authorizationModel.AuthError = msg;
            });
        };
        WidgetController.prototype.loadDonationTargets = function () {
            var _this = this;
            this.widgetService.getAll(this._id).then(function (res) {
                _this._rows = _this.collectionService.toRows(res.data.Items, _this.TargetDonationsPerScreenCount);
                _this.$timeout.cancel(_this._carouselTargetDonationsTimeout);
                var indexToNotificate = -1;
                _this._rowIndex = 0;
                if (_this._newDonateArrivedToTargetCode) {
                    var itemForCode = res.data.Items.filter(function (x) { return x.Code == _this._newDonateArrivedToTargetCode; })[0];
                    var index = res.data.Items.indexOf(itemForCode);
                    if (index > -1) {
                        var rowIndex = 0;
                        if (index + 1 > _this.TargetDonationsPerScreenCount) {
                            rowIndex = Math.floor(index / _this.TargetDonationsPerScreenCount);
                            indexToNotificate = _this._rows[rowIndex].indexOf(_this._rows[rowIndex].filter(function (x) { return x.Code == _this._newDonateArrivedToTargetCode; })[0]);
                            _this._rowIndex = rowIndex;
                        }
                        else
                            indexToNotificate = index;
                    }
                }
                var manyScreens = _this._rows.length > 1;
                _this.$scope.targetDonationInfos = _this._rows[_this._rowIndex];
                _this.showHiddenItems(!manyScreens);
                if (manyScreens)
                    _this.runCarousel();
                if (indexToNotificate > -1) {
                    _this.$timeout(function () {
                        var itemBlock = $('.targetItem').eq(indexToNotificate);
                        itemBlock.toggleClass("notificated").find('.donate').fadeIn(1000);
                        _this.$timeout(function () {
                            itemBlock.toggleClass("notificated").find('.donate').fadeOut(3000);
                            ;
                        }, 3000);
                    });
                }
            });
        };
        return WidgetController;
    }());
    WidgetApp.WidgetController = WidgetController;
})(WidgetApp || (WidgetApp = {}));
//# sourceMappingURL=widgetController.js.map