var App;
(function (App) {
    var Common;
    (function (Common) {
        var Services;
        (function (Services) {
            var ModalService = (function () {
                function ModalService() {
                }
                ModalService.prototype.showErrorModal = function (errorMessage) {
                    var modal = $('#errorModal');
                    if (modal.length) {
                        if (!modal.is(':visible')) {
                            modal.find('#errorModalMessageContainer').text(errorMessage);
                            modal.modal('show');
                        }
                    }
                    else {
                        console.error('modal-danger not founded');
                    }
                };
                ModalService.prototype.showConfirm = function (title, message, okCallback) {
                    var confirm = $('#confirmModal');
                    if (confirm.length) {
                        if (!confirm.is(':visible')) {
                            confirm.find('#confirmModalHeader').text(title);
                            confirm.find('#confirmModalMessage').text(message);
                            var okBtn = confirm.find('#confirmModalOkButton');
                            okBtn.unbind('click');
                            okBtn.click(function (eventObject) {
                                okCallback();
                            });
                            confirm.modal('show');
                        }
                    }
                };
                return ModalService;
            }());
            Services.ModalService = ModalService;
            var NotificationService = (function () {
                function NotificationService() {
                }
                NotificationService.prototype.showSuccessNotification = function (message, header, hideAfterMs) {
                    if (hideAfterMs === void 0) { hideAfterMs = 3000; }
                    $.toast({
                        heading: header,
                        text: message,
                        position: 'top-right',
                        loaderBg: '#ff6849',
                        icon: 'success',
                        hideAfter: hideAfterMs,
                        stack: 6
                    });
                };
                return NotificationService;
            }());
            Services.NotificationService = NotificationService;
            var ErrorsService = (function () {
                function ErrorsService() {
                }
                ErrorsService.prototype.getErrorMessage = function (err) {
                    var msg = null;
                    if (err && err.data && err.data.ErrorMessage)
                        msg = err.data.ErrorMessage;
                    return msg;
                };
                return ErrorsService;
            }());
            Services.ErrorsService = ErrorsService;
            var CollectionService = (function () {
                function CollectionService() {
                }
                CollectionService.prototype.toRows = function (items, rowItemsCount) {
                    var groups = [], inner;
                    for (var i = 0; i < items.length; i++) {
                        if (i % rowItemsCount === 0) {
                            inner = [];
                            groups.push(inner);
                        }
                        inner.push(items[i]);
                    }
                    return groups;
                };
                return CollectionService;
            }());
            Services.CollectionService = CollectionService;
        })(Services = Common.Services || (Common.Services = {}));
    })(Common = App.Common || (App.Common = {}));
})(App || (App = {}));
//# sourceMappingURL=commonServices.js.map